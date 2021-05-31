using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace elconnect
{
    public partial class EyelinkWindow : Form
    {
        [DllImport("User32.dll")]
        public static extern void DisableProcessWindowsGhosting();


        bool draw = false;
        int x = 0;
        int y = 0;
        public EyelinkWindow()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;

            Cursor.Hide();
            DisableProcessWindowsGhosting();
        }
        public void setGaze(int x, int y)
        {
            if (this.x != x && this.y != y )
            {
                this.x = x;
                this.y = y;
                this.Refresh();
            }
        }
        public void setGazeCursor(bool b)
        {
            this.draw = b;
        }
        protected override void OnPaint(PaintEventArgs paintEvnt)
        {
            if (this.draw)
            {
                if ((int)x != (int)SREYELINKLib.EL_CONSTANT.EL_MISSING_DATA && (int)y != (int)SREYELINKLib.EL_CONSTANT.EL_MISSING_DATA)
                {
                    Color brushColor = Color.FromArgb(0, 55, 0);
                    SolidBrush brush = new SolidBrush(brushColor);
                    Rectangle r = new Rectangle(x - 10, y - 10, 20, 20);
                    Graphics gfx = paintEvnt.Graphics;
                    gfx.FillEllipse(brush, r);
                }
            }

        }
    }
}
