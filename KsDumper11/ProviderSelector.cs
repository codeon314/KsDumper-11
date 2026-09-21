using KsDumper11.Driver;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KsDumper11
{
    public partial class ProviderSelector : Form
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

        KduWrapper wrapper;

        JsonSettingsManager settingsManager;

        public ProviderSelector()
        {
            InitializeComponent();

            settingsManager = new JsonSettingsManager();

            KduSelfExtract.Extract();

            wrapper = new KduWrapper(KduSelfExtract.KduPath);
            wrapper.DriverLoaded += Wrapper_DriverLoaded;
            wrapper.ProvidersLoaded += Wrapper_ProvidersLoaded;

            wrapper.LoadProviders();
        }

        private void Wrapper_ProvidersLoaded(object sender, EventArgs e)
        {
            foreach (KduProvider p in wrapper.providers)
            {
                ListViewItem item = new ListViewItem(p.ProviderIndex.ToString());
                item.SubItems.Add(p.ProviderName);

                if (p.ProviderName.Contains("[NOT WORKING]"))
                {
                    item.ForeColor = Color.Red;
                }

                if (p.ProviderName.Contains("[WORKING]"))
                {
                    item.ForeColor = Color.Green;
                }

                providerList.Items.Add(item);
            }

            if (wrapper.DefaultProvider != -1)
            {
                providerList.SelectedIndices.Add(wrapper.DefaultProvider);
            }
            else
            {
                providerList.SelectedIndices.Add(0);
            }

            // Auto-size every column so the header text and the widest subitem
            // are always fully visible (nothing gets truncated with an ellipsis).
            // Width = -2 sizes each column to fit its header and longest
            // subitem. Re-applied here so widths stay fitted whenever the
            // provider list is (re)populated.
            for (int i = 0; i < providerList.Columns.Count; i++)
            {
                providerList.Columns[i].Width = -2;
            }
        }

        private void Wrapper_DriverLoaded(object sender, object[] e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new EventHandler<object[]>(Wrapper_DriverLoaded), new object[] { sender, e });
            }
            else
            {
                bool res = (bool)e[0];
                int providerIndex = (int)e[1];

                int idx = -1;
                ListViewItem item = null;

                for (int i = 0; i < providerList.Items.Count; i++)
                {
                    ListViewItem candidate = providerList.Items[i];
                    if (candidate.SubItems.Count == 0) continue;
                    if (candidate.SubItems[0].Text == providerIndex.ToString())
                    {
                        idx = i;
                        item = candidate;
                        break;
                    }
                }

                if (item == null) return;

                for (int i = 0; i < wrapper.providers.Count; i++)
                {
                    string non_W = "[NOT WORKING] ";
                    string W_ = "[WORKING] ";

                    if (wrapper.providers[i].ProviderName == item.SubItems[1].Text)
                    {
                        if (res)
                        {
                            wrapper.providers[i].ProviderName = W_ + wrapper.providers[i].ProviderName;
                            if (wrapper.IsDirty == false) wrapper.IsDirty = true;
                        }
                        else
                        {
                            wrapper.providers[i].ProviderName = non_W + wrapper.providers[i].ProviderName;
                            if (wrapper.IsDirty == false) wrapper.IsDirty = true;
                        }
                        break;
                    }
                }
                this.wipeSettingsBtn.Enabled = wrapper.IsDirty;
                if (res)
                {
                    driverLoadedLbl.ForeColor = Color.Green;
                    driverLoadedLbl.Text = "Driver Loaded!";

                    item.SubItems[1].Text = "[WORKING] " + item.SubItems[1].Text;

                    if (providerList.SelectedIndices.Count > 0 && providerList.SelectedIndices[0] == idx)
                    {
                        setDefaultProviderBtn.Enabled = true;
                    }

                    item.ForeColor = Color.Green;
                }
                else
                {
                    driverLoadedLbl.ForeColor = Color.Red;
                    driverLoadedLbl.Text = "Driver failed to load!";

                    item.SubItems[1].Text = "[NOT WORKING] " + item.SubItems[1].Text;

                    if (providerList.SelectedIndices.Count > 0 && providerList.SelectedIndices[0] == idx)
                    {
                        setDefaultProviderBtn.Enabled = false;
                    }

                    item.ForeColor = Color.Red;
                }

                // Provider Name string may have grown (e.g. "[WORKING] " or
                // "[NOT WORKING] " prefix added) - re-fit columns so the new
                // longer text is not truncated.
                for (int i = 0; i < providerList.Columns.Count; i++)
                {
                    providerList.Columns[i].Width = -2;
                }

                driverLoadedLbl.Visible = true;
                driverLoadedLblTimer.Start();
            }
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
        }

        private void providerList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (providerList.SelectedIndices.Count > 0)
            {
                int idx = providerList.SelectedIndices[0];

                KduProvider p = wrapper.providers[idx];

                if (p.ProviderName.Contains("[NOT WORKING]") || p.ProviderName.Contains("[WORKING]"))
                {
                    testProviderBtn.Enabled = false;
                }
                else
                {
                    testProviderBtn.Enabled = true;
                }

                if (p.ProviderName.Contains("[NOT WORKING]"))
                {
                    setDefaultProviderBtn.Enabled = false;
                }
                else if (p.ProviderName.Contains("[WORKING]"))
                {
                    setDefaultProviderBtn.Enabled = true;
                }
                else
                {
                    setDefaultProviderBtn.Enabled = false;
                }

                driverNameBox.Text = p.DriverName;
                deviceNameBox.Text = p.DeviceName;
                signerNameBox.Text = p.SignerName;
                minWinBuildBox.Text = p.MinWindowsBuild;
                maxWinBuildBox.Text = p.MaxWindowsBuild;
                driverWhqlSignedBox.Checked = p.IsWHQL_Signed;
                shellcodeMaskBox.Text = p.ShellcodeSupportMask;

                advisoryBox.Text = p.Advisory;
                imageSizeBox.Text = p.ImageSize;
                fileHashBox.Text = p.FileHashSHA1;
                authHashBox.Text = p.AuthenticodeHashSHA1;
                pageHashSha1Box.Text = p.PageHashSHA1;
                pageHashSha256Box.Text = p.PageHashSHA256;

                defaultProviderIDBox.Text = wrapper.DefaultProvider.ToString();

                providerExtraInfoBox.Clear();
                if (p.ExtraInfo != null && p.ExtraInfo.Length > 0)
                {
                    for (int i = 0; i < p.ExtraInfo.Length; i++)
                    {
                        providerExtraInfoBox.AppendText(p.ExtraInfo[i] + Environment.NewLine);
                    }
                }
            }
        }

        private void testProviderBtn_Click(object sender, EventArgs e)
        {
            if (providerList.SelectedIndices.Count > 0)
            {
                testProviderBtn.Enabled = false;

                int idx = providerList.SelectedIndices[0];

                KduProvider p = wrapper.providers[idx];

                wrapper.tryLoad(p.ProviderIndex);
            }
        }

        private void driverLoadedLblTimer_Tick(object sender, EventArgs e)
        {
            testProviderBtn.Enabled = true;

            driverLoadedLbl.Visible = false;

            driverLoadedLblTimer.Stop();
        }

        private void setDefaultProviderBtn_Click(object sender, EventArgs e)
        {
            if (providerList.SelectedIndices.Count > 0)
            {
                testProviderBtn.Enabled = false;

                int idx = providerList.SelectedIndices[0];

                wrapper.SetDefaultProvider(idx);

                defaultProviderIDBox.Text = wrapper.DefaultProvider.ToString();

                wrapper.Start();

                Program.ProviderIsClosing = true;

                this.Close();
            }
        }

        private void wipeSettingsBtn_Click(object sender, EventArgs e)
        {
            wrapper.ResetProviders();

            providerList.Items.Clear();

            Wrapper_ProvidersLoaded(sender, e);

            this.wipeSettingsBtn.Enabled = false;
        }

        private void ProviderSelector_Load(object sender, EventArgs e)
        {
            this.wipeSettingsBtn.Enabled = wrapper.IsDirty;

            if (settingsManager.JsonSettings.enableAntiAntiDebuggerTools)
            {
                SnifferBypass.SelfTitle(this.Handle);
                this.Text = SnifferBypass.GenerateRandomString(this.Text.Length);
            }

            providerFormFix.Start();
        }

        private void providerFormFix_Tick(object sender, EventArgs e)
        {
            this.Size = new Size(this.Size.Width + 3, this.Size.Height);
            this.Invalidate();
            providerFormFix.Stop();
        }
    }
}