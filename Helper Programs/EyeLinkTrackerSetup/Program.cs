using System;
using System.Windows.Forms;


namespace EyeLinkTrackerSetup
{
    class Program
    {
        [STAThread]
        static int Main(string[] args)
        {
            EyeLinkTrackerSetupWindow elW = new EyeLinkTrackerSetupWindow();
            elW.Show();

            SREYELINKLib.EyeLinkUtil elutil = new SREYELINKLib.EyeLinkUtil();
            SREYELINKLib.EyeLink el = new SREYELINKLib.EyeLink();
            SREYELINKLib.ELGDICal cal = elutil.getGDICal();

            try
            {
                el.open("100.1.1.1", 0);
                el.sendCommand("link_sample_data  = LEFT,RIGHT,GAZE");
                el.sendCommand("screen_pixel_coords=0,0," + elW.Width + "," + elW.Height);

                cal.setCalibrationWindow(elW.Handle.ToInt32());

                cal.enableKeyCollection(true);

                if (args.Length == 0 || args[0] == "setup")
                {
                    el.doTrackerSetup();
                }
                else if (args[0] == "driftCorrect")
                {
                    el.doDriftCorrect((short)(elW.Width / 2), (short)(elW.Height / 2), true, false);
                    el.applyDriftCorrect();
                }

                cal.enableKeyCollection(false);
                el.setOfflineMode();
                elutil.pumpDelay(50);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return -1;
            }
            finally
            {
                el.close();
                el = null;
                elutil = null;
            }
            return 0;
        }
    }
}
