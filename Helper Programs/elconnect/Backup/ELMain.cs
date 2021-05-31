using System;

namespace elconnect
{
    class ELmain
    {
        [STAThread]
        static void Main(string[] args)
        {
            EyelinkWindow elW = new EyelinkWindow();
            elW.Show();

            SREYELINKLib.EL_EYE eye = SREYELINKLib.EL_EYE.EL_EYE_NONE;
            SREYELINKLib.EyeLinkUtil elutil = new SREYELINKLib.EyeLinkUtil();
            SREYELINKLib.EyeLink el = new SREYELINKLib.EyeLink();


            try
            {
                double st;
                double lastSampleTime = 0.0;


                el.open("100.1.1.1", 0);
                el.openDataFile("abc.edf");
                el.sendCommand("link_sample_data  = LEFT,RIGHT,GAZE");
                el.sendCommand("screen_pixel_coords=0,0," + elW.Width + "," + elW.Height);
                el.sendMessage("abc");



                SREYELINKLib.ELGDICal cal = elutil.getGDICal();
                cal.setCalibrationWindow(elW.Handle.ToInt32());
                cal.enableKeyCollection(true);
                el.doTrackerSetup();
                cal.enableKeyCollection(false);

                cal.enableKeyCollection(true);
                elutil.pumpDelay(1500);
                el.doDriftCorrect((short)(elW.Width / 2), (short)(elW.Height / 2), true, true);
                cal.enableKeyCollection(false);

                elW.setGazeCursor(true);

                el.setOfflineMode();
                elutil.pumpDelay(50);

                el.startRecording(false, false, true, false);

                st = elutil.currentTime();
                while ((st + 20000) > elutil.currentTime())
                {
                    SREYELINKLib.Sample s;
                    s = el.getNewestSample();
                    if (s != null && s.time != lastSampleTime)
                    {
                        if (eye != SREYELINKLib.EL_EYE.EL_EYE_NONE)
                        {
                            if (eye == SREYELINKLib.EL_EYE.EL_BINOCULAR)
                                eye = SREYELINKLib.EL_EYE.EL_LEFT;

                            float x = s.get_gx(eye);
                            float y = s.get_gy(eye);
                            Console.Write(s.time);
                            Console.Write("\t");
                            Console.Write(x);
                            Console.Write("\t");
                            Console.Write(y);
                            Console.WriteLine("");// New line

                            lastSampleTime = s.time;
                            elW.setGaze((int)x, (int)y);
                        }
                        else
                        {
                            eye = (SREYELINKLib.EL_EYE)el.eyeAvailable();
                        }
                    }
                }
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                el.stopRecording();
                el.close();
                el = null;
                elutil = null;
            }
        }
    }
}
