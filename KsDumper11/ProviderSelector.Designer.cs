namespace KsDumper11
{
    partial class ProviderSelector
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProviderSelector));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.driverLoadedLbl = new System.Windows.Forms.Label();
            this.driverLoadedLblTimer = new System.Windows.Forms.Timer(this.components);
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.advisoryBox = new DarkControls.Controls.DarkTextBox();
            this.imageSizeBox = new DarkControls.Controls.DarkTextBox();
            this.fileHashBox = new DarkControls.Controls.DarkTextBox();
            this.authHashBox = new DarkControls.Controls.DarkTextBox();
            this.pageHashSha1Box = new DarkControls.Controls.DarkTextBox();
            this.pageHashSha256Box = new DarkControls.Controls.DarkTextBox();
            this.defaultProviderIDBox = new DarkControls.Controls.DarkTextBox();
            this.setDefaultProviderBtn = new DarkControls.Controls.DarkButton();
            this.testProviderBtn = new DarkControls.Controls.DarkButton();
            this.shellcodeMaskBox = new DarkControls.Controls.DarkTextBox();
            this.driverWhqlSignedBox = new DarkControls.Controls.DarkCheckBox();
            this.minWinBuildBox = new DarkControls.Controls.DarkTextBox();
            this.maxWinBuildBox = new DarkControls.Controls.DarkTextBox();
            this.signerNameBox = new DarkControls.Controls.DarkTextBox();
            this.deviceNameBox = new DarkControls.Controls.DarkTextBox();
            this.driverNameBox = new DarkControls.Controls.DarkTextBox();
            this.providerExtraInfoBox = new DarkControls.Controls.DarkTextBox();
            this.providerList = new DarkControls.Controls.DarkListView();
            this.provIdCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.provNameCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.appIcon1 = new DarkControls.Controls.AppIcon();
            this.titleLbl = new DarkControls.Controls.TransparentLabel();
            this.closeBtn = new DarkControls.Controls.WindowsDefaultTitleBarButton();
            this.wipeSettingsBtn = new DarkControls.Controls.DarkButton();
            ((System.ComponentModel.ISupportInitialize)(this.appIcon1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(609, 942);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(139, 20);
            this.label1.TabIndex = 45;
            this.label1.Text = "Provider Extra Info";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(609, 51);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 20);
            this.label2.TabIndex = 14;
            this.label2.Text = "Driver Name";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(609, 111);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(103, 20);
            this.label3.TabIndex = 16;
            this.label3.Text = "Device Name";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(609, 174);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(101, 20);
            this.label4.TabIndex = 18;
            this.label4.Text = "Signer Name";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(609, 302);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(177, 20);
            this.label5.TabIndex = 22;
            this.label5.Text = "Minimum Windows build";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(609, 238);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(181, 20);
            this.label6.TabIndex = 20;
            this.label6.Text = "Maximum Windows build";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(609, 371);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(179, 20);
            this.label7.TabIndex = 25;
            this.label7.Text = "Shellcode support mask";
            // 
            // driverLoadedLbl
            // 
            this.driverLoadedLbl.AutoSize = true;
            this.driverLoadedLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.driverLoadedLbl.Location = new System.Drawing.Point(122, 1131);
            this.driverLoadedLbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.driverLoadedLbl.Name = "driverLoadedLbl";
            this.driverLoadedLbl.Size = new System.Drawing.Size(200, 32);
            this.driverLoadedLbl.TabIndex = 48;
            this.driverLoadedLbl.Text = "Driver Loaded!";
            this.driverLoadedLbl.Visible = false;
            // 
            // driverLoadedLblTimer
            // 
            this.driverLoadedLblTimer.Interval = 2500;
            this.driverLoadedLblTimer.Tick += new System.EventHandler(this.driverLoadedLblTimer_Tick);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(609, 437);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(143, 20);
            this.label8.TabIndex = 29;
            this.label8.Text = "Default Provider ID";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(609, 505);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(68, 20);
            this.label9.TabIndex = 32;
            this.label9.Text = "Advisory";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(609, 568);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(89, 20);
            this.label10.TabIndex = 34;
            this.label10.Text = "Image Size";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(609, 631);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(133, 20);
            this.label11.TabIndex = 36;
            this.label11.Text = "File Hash (SHA1)";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(609, 694);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(203, 20);
            this.label12.TabIndex = 38;
            this.label12.Text = "Authenticode Hash (SHA1)";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(609, 757);
            this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(145, 20);
            this.label13.TabIndex = 40;
            this.label13.Text = "Page Hash (SHA1)";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(609, 820);
            this.label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(163, 20);
            this.label14.TabIndex = 42;
            this.label14.Text = "Page Hash (SHA256)";
            // 
            // advisoryBox
            // 
            this.advisoryBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.advisoryBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.advisoryBox.ForeColor = System.Drawing.Color.Silver;
            this.advisoryBox.Location = new System.Drawing.Point(614, 529);
            this.advisoryBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.advisoryBox.Name = "advisoryBox";
            this.advisoryBox.Size = new System.Drawing.Size(590, 26);
            this.advisoryBox.TabIndex = 33;
            // 
            // imageSizeBox
            // 
            this.imageSizeBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.imageSizeBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.imageSizeBox.ForeColor = System.Drawing.Color.Silver;
            this.imageSizeBox.Location = new System.Drawing.Point(614, 592);
            this.imageSizeBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.imageSizeBox.Name = "imageSizeBox";
            this.imageSizeBox.Size = new System.Drawing.Size(590, 26);
            this.imageSizeBox.TabIndex = 35;
            // 
            // fileHashBox
            // 
            this.fileHashBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.fileHashBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.fileHashBox.ForeColor = System.Drawing.Color.Silver;
            this.fileHashBox.Location = new System.Drawing.Point(614, 655);
            this.fileHashBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.fileHashBox.Name = "fileHashBox";
            this.fileHashBox.Size = new System.Drawing.Size(590, 26);
            this.fileHashBox.TabIndex = 37;
            // 
            // authHashBox
            // 
            this.authHashBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.authHashBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.authHashBox.ForeColor = System.Drawing.Color.Silver;
            this.authHashBox.Location = new System.Drawing.Point(614, 718);
            this.authHashBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.authHashBox.Name = "authHashBox";
            this.authHashBox.Size = new System.Drawing.Size(590, 26);
            this.authHashBox.TabIndex = 39;
            // 
            // pageHashSha1Box
            // 
            this.pageHashSha1Box.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.pageHashSha1Box.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pageHashSha1Box.ForeColor = System.Drawing.Color.Silver;
            this.pageHashSha1Box.Location = new System.Drawing.Point(614, 782);
            this.pageHashSha1Box.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pageHashSha1Box.Name = "pageHashSha1Box";
            this.pageHashSha1Box.Size = new System.Drawing.Size(590, 26);
            this.pageHashSha1Box.TabIndex = 41;
            // 
            // pageHashSha256Box
            // 
            this.pageHashSha256Box.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.pageHashSha256Box.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pageHashSha256Box.ForeColor = System.Drawing.Color.Silver;
            this.pageHashSha256Box.Location = new System.Drawing.Point(614, 845);
            this.pageHashSha256Box.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pageHashSha256Box.Name = "pageHashSha256Box";
            this.pageHashSha256Box.Size = new System.Drawing.Size(590, 26);
            this.pageHashSha256Box.TabIndex = 43;
            // 
            // defaultProviderIDBox
            // 
            this.defaultProviderIDBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.defaultProviderIDBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.defaultProviderIDBox.ForeColor = System.Drawing.Color.Silver;
            this.defaultProviderIDBox.Location = new System.Drawing.Point(614, 462);
            this.defaultProviderIDBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.defaultProviderIDBox.Name = "defaultProviderIDBox";
            this.defaultProviderIDBox.Size = new System.Drawing.Size(590, 26);
            this.defaultProviderIDBox.TabIndex = 30;
            // 
            // setDefaultProviderBtn
            // 
            this.setDefaultProviderBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.setDefaultProviderBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.setDefaultProviderBtn.ForeColor = System.Drawing.Color.Silver;
            this.setDefaultProviderBtn.Location = new System.Drawing.Point(1029, 1134);
            this.setDefaultProviderBtn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.setDefaultProviderBtn.Name = "setDefaultProviderBtn";
            this.setDefaultProviderBtn.Size = new System.Drawing.Size(176, 35);
            this.setDefaultProviderBtn.TabIndex = 49;
            this.setDefaultProviderBtn.Text = "Set Default Provider";
            this.setDefaultProviderBtn.UseVisualStyleBackColor = true;
            this.setDefaultProviderBtn.Click += new System.EventHandler(this.setDefaultProviderBtn_Click);
            // 
            // testProviderBtn
            // 
            this.testProviderBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.testProviderBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.testProviderBtn.ForeColor = System.Drawing.Color.Silver;
            this.testProviderBtn.Location = new System.Drawing.Point(0, 1131);
            this.testProviderBtn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.testProviderBtn.Name = "testProviderBtn";
            this.testProviderBtn.Size = new System.Drawing.Size(112, 35);
            this.testProviderBtn.TabIndex = 47;
            this.testProviderBtn.Text = "Test Driver";
            this.testProviderBtn.UseVisualStyleBackColor = true;
            this.testProviderBtn.Click += new System.EventHandler(this.testProviderBtn_Click);
            // 
            // shellcodeMaskBox
            // 
            this.shellcodeMaskBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.shellcodeMaskBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.shellcodeMaskBox.ForeColor = System.Drawing.Color.Silver;
            this.shellcodeMaskBox.Location = new System.Drawing.Point(614, 395);
            this.shellcodeMaskBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.shellcodeMaskBox.Name = "shellcodeMaskBox";
            this.shellcodeMaskBox.Size = new System.Drawing.Size(590, 26);
            this.shellcodeMaskBox.TabIndex = 24;
            // 
            // driverWhqlSignedBox
            // 
            this.driverWhqlSignedBox.Appearance = System.Windows.Forms.Appearance.Button;
            this.driverWhqlSignedBox.BoxBorderColor = System.Drawing.Color.DarkSlateBlue;
            this.driverWhqlSignedBox.BoxFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.driverWhqlSignedBox.CheckColor = System.Drawing.Color.CornflowerBlue;
            this.driverWhqlSignedBox.FlatAppearance.BorderSize = 0;
            this.driverWhqlSignedBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.driverWhqlSignedBox.Location = new System.Drawing.Point(980, 892);
            this.driverWhqlSignedBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.driverWhqlSignedBox.Name = "driverWhqlSignedBox";
            this.driverWhqlSignedBox.Size = new System.Drawing.Size(225, 42);
            this.driverWhqlSignedBox.TabIndex = 44;
            this.driverWhqlSignedBox.Text = "Driver is WHQL Signed";
            this.driverWhqlSignedBox.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.driverWhqlSignedBox.UseVisualStyleBackColor = true;
            // 
            // minWinBuildBox
            // 
            this.minWinBuildBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.minWinBuildBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.minWinBuildBox.ForeColor = System.Drawing.Color.Silver;
            this.minWinBuildBox.Location = new System.Drawing.Point(614, 326);
            this.minWinBuildBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.minWinBuildBox.Name = "minWinBuildBox";
            this.minWinBuildBox.Size = new System.Drawing.Size(590, 26);
            this.minWinBuildBox.TabIndex = 21;
            // 
            // maxWinBuildBox
            // 
            this.maxWinBuildBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.maxWinBuildBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.maxWinBuildBox.ForeColor = System.Drawing.Color.Silver;
            this.maxWinBuildBox.Location = new System.Drawing.Point(614, 263);
            this.maxWinBuildBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.maxWinBuildBox.Name = "maxWinBuildBox";
            this.maxWinBuildBox.Size = new System.Drawing.Size(590, 26);
            this.maxWinBuildBox.TabIndex = 19;
            // 
            // signerNameBox
            // 
            this.signerNameBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.signerNameBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.signerNameBox.ForeColor = System.Drawing.Color.Silver;
            this.signerNameBox.Location = new System.Drawing.Point(614, 198);
            this.signerNameBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.signerNameBox.Name = "signerNameBox";
            this.signerNameBox.Size = new System.Drawing.Size(590, 26);
            this.signerNameBox.TabIndex = 17;
            // 
            // deviceNameBox
            // 
            this.deviceNameBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.deviceNameBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.deviceNameBox.ForeColor = System.Drawing.Color.Silver;
            this.deviceNameBox.Location = new System.Drawing.Point(614, 135);
            this.deviceNameBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.deviceNameBox.Name = "deviceNameBox";
            this.deviceNameBox.Size = new System.Drawing.Size(590, 26);
            this.deviceNameBox.TabIndex = 15;
            // 
            // driverNameBox
            // 
            this.driverNameBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.driverNameBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.driverNameBox.ForeColor = System.Drawing.Color.Silver;
            this.driverNameBox.Location = new System.Drawing.Point(614, 75);
            this.driverNameBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.driverNameBox.Name = "driverNameBox";
            this.driverNameBox.Size = new System.Drawing.Size(590, 26);
            this.driverNameBox.TabIndex = 13;
            // 
            // providerExtraInfoBox
            // 
            this.providerExtraInfoBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.providerExtraInfoBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.providerExtraInfoBox.ForeColor = System.Drawing.Color.Silver;
            this.providerExtraInfoBox.Location = new System.Drawing.Point(609, 969);
            this.providerExtraInfoBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.providerExtraInfoBox.Multiline = true;
            this.providerExtraInfoBox.Name = "providerExtraInfoBox";
            this.providerExtraInfoBox.Size = new System.Drawing.Size(594, 130);
            this.providerExtraInfoBox.TabIndex = 46;
            // 
            // providerList
            // 
            this.providerList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.providerList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.provIdCol,
            this.provNameCol});
            this.providerList.ForeColor = System.Drawing.Color.Silver;
            this.providerList.FullRowSelect = true;
            this.providerList.HideSelection = false;
            this.providerList.Location = new System.Drawing.Point(0, 54);
            this.providerList.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.providerList.Name = "providerList";
            this.providerList.OwnerDraw = true;
            this.providerList.Size = new System.Drawing.Size(598, 1067);
            this.providerList.TabIndex = 10;
            this.providerList.UseCompatibleStateImageBehavior = false;
            this.providerList.View = System.Windows.Forms.View.Details;
            this.providerList.SelectedIndexChanged += new System.EventHandler(this.providerList_SelectedIndexChanged);
            // 
            // provIdCol
            // 
            this.provIdCol.Text = "ID";
            // 
            // provNameCol
            // 
            this.provNameCol.Text = "Provider Name";
            this.provNameCol.Width = 396;
            // 
            // appIcon1
            // 
            this.appIcon1.AppIconImage = ((System.Drawing.Image)(resources.GetObject("appIcon1.AppIconImage")));
            this.appIcon1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.appIcon1.DragForm = null;
            this.appIcon1.Image = ((System.Drawing.Image)(resources.GetObject("appIcon1.Image")));
            this.appIcon1.Location = new System.Drawing.Point(2, 2);
            this.appIcon1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.appIcon1.Name = "appIcon1";
            this.appIcon1.Scale = 3.5F;
            this.appIcon1.Size = new System.Drawing.Size(42, 43);
            this.appIcon1.TabIndex = 9;
            this.appIcon1.TabStop = false;
            // 
            // titleLbl
            // 
            this.titleLbl.AutoSize = true;
            this.titleLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.titleLbl.Location = new System.Drawing.Point(48, 6);
            this.titleLbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.titleLbl.Name = "titleLbl";
            this.titleLbl.Size = new System.Drawing.Size(363, 29);
            this.titleLbl.TabIndex = 8;
            this.titleLbl.Text = "KsDumper 11 Provider Selection";
            // 
            // closeBtn
            // 
            this.closeBtn.ButtonType = DarkControls.Controls.WindowsDefaultTitleBarButton.Type.Close;
            this.closeBtn.ClickColor = System.Drawing.Color.Red;
            this.closeBtn.ClickIconColor = System.Drawing.Color.Black;
            this.closeBtn.HoverColor = System.Drawing.Color.OrangeRed;
            this.closeBtn.HoverIconColor = System.Drawing.Color.Black;
            this.closeBtn.IconColor = System.Drawing.Color.Black;
            this.closeBtn.IconLineThickness = 2;
            this.closeBtn.Location = new System.Drawing.Point(1156, 0);
            this.closeBtn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.closeBtn.Name = "closeBtn";
            this.closeBtn.Size = new System.Drawing.Size(60, 62);
            this.closeBtn.TabIndex = 7;
            this.closeBtn.Text = "windowsDefaultTitleBarButton1";
            this.closeBtn.UseVisualStyleBackColor = true;
            this.closeBtn.Click += new System.EventHandler(this.closeBtn_Click);
            // 
            // wipeSettingsBtn
            // 
            this.wipeSettingsBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.wipeSettingsBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.wipeSettingsBtn.ForeColor = System.Drawing.Color.Silver;
            this.wipeSettingsBtn.Location = new System.Drawing.Point(896, 1134);
            this.wipeSettingsBtn.Name = "wipeSettingsBtn";
            this.wipeSettingsBtn.Size = new System.Drawing.Size(126, 35);
            this.wipeSettingsBtn.TabIndex = 50;
            this.wipeSettingsBtn.Text = "Wipe Settings";
            this.wipeSettingsBtn.UseVisualStyleBackColor = true;
            this.wipeSettingsBtn.Click += new System.EventHandler(this.wipeSettingsBtn_Click);
            // 
            // ProviderSelector
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.ClientSize = new System.Drawing.Size(1220, 1185);
            this.Controls.Add(this.wipeSettingsBtn);
            this.Controls.Add(this.defaultProviderIDBox);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.pageHashSha256Box);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.pageHashSha1Box);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.authHashBox);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.fileHashBox);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.imageSizeBox);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.advisoryBox);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.setDefaultProviderBtn);
            this.Controls.Add(this.driverLoadedLbl);
            this.Controls.Add(this.testProviderBtn);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.shellcodeMaskBox);
            this.Controls.Add(this.driverWhqlSignedBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.minWinBuildBox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.maxWinBuildBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.signerNameBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.deviceNameBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.driverNameBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.providerExtraInfoBox);
            this.Controls.Add(this.providerList);
            this.Controls.Add(this.appIcon1);
            this.Controls.Add(this.titleLbl);
            this.Controls.Add(this.closeBtn);
            this.DoubleBuffered = true;
            this.ForeColor = System.Drawing.Color.Silver;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "ProviderSelector";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "KsDumper 11 Provider Selection";
            this.Load += new System.EventHandler(this.ProviderSelector_Load);
            ((System.ComponentModel.ISupportInitialize)(this.appIcon1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private DarkControls.Controls.WindowsDefaultTitleBarButton closeBtn;
        private DarkControls.Controls.TransparentLabel titleLbl;
        private DarkControls.Controls.AppIcon appIcon1;
        private DarkControls.Controls.DarkListView providerList;
        private System.Windows.Forms.ColumnHeader provNameCol;
        private DarkControls.Controls.DarkTextBox providerExtraInfoBox;
        private System.Windows.Forms.Label label1;
        private DarkControls.Controls.DarkTextBox driverNameBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private DarkControls.Controls.DarkTextBox deviceNameBox;
        private System.Windows.Forms.Label label4;
        private DarkControls.Controls.DarkTextBox signerNameBox;
        private System.Windows.Forms.Label label5;
        private DarkControls.Controls.DarkTextBox minWinBuildBox;
        private System.Windows.Forms.Label label6;
        private DarkControls.Controls.DarkTextBox maxWinBuildBox;
        private DarkControls.Controls.DarkCheckBox driverWhqlSignedBox;
        private System.Windows.Forms.ColumnHeader provIdCol;
        private System.Windows.Forms.Label label7;
        private DarkControls.Controls.DarkTextBox shellcodeMaskBox;
        private DarkControls.Controls.DarkButton testProviderBtn;
        private System.Windows.Forms.Label driverLoadedLbl;
        private System.Windows.Forms.Timer driverLoadedLblTimer;
        private DarkControls.Controls.DarkButton setDefaultProviderBtn;
        private System.Windows.Forms.Label label8;
        private DarkControls.Controls.DarkTextBox defaultProviderIDBox;
        private DarkControls.Controls.DarkButton wipeSettingsBtn;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private DarkControls.Controls.DarkTextBox advisoryBox;
        private DarkControls.Controls.DarkTextBox imageSizeBox;
        private DarkControls.Controls.DarkTextBox fileHashBox;
        private DarkControls.Controls.DarkTextBox authHashBox;
        private DarkControls.Controls.DarkTextBox pageHashSha1Box;
        private DarkControls.Controls.DarkTextBox pageHashSha256Box;
    }
}

