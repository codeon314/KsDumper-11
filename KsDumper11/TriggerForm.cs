using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace KsDumper11
{
    public partial class TriggerForm : Form
    {
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000; // WS_EX_COMPOSITED
                return cp;
            }
        }

        JsonSettingsManager settingsManager;

        public TriggerForm()
        {
            InitializeComponent();

            settingsManager = new JsonSettingsManager();
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
        }

        private void closeBtn_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void TriggerForm_Load(object sender, EventArgs e)
        {
            if (settingsManager.JsonSettings.enableAntiAntiDebuggerTools)
            {
                SnifferBypass.SelfTitle(this.Handle);
                this.Text = SnifferBypass.GenerateRandomString(this.Text.Length);
            }
        }
    }
}