using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace EyeLinkTrackerSetup
{
    public partial class EyeLinkTrackerSetupWindow : Form
    {
        [DllImport("User32.dll")]
        public static extern void DisableProcessWindowsGhosting();

        public EyeLinkTrackerSetupWindow()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;

            Cursor.Hide();
            DisableProcessWindowsGhosting();
        }

    }
}
