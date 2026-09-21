namespace KsDumper11
{
    partial class ProviderSelector
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            label5 = new System.Windows.Forms.Label();
            label6 = new System.Windows.Forms.Label();
            label7 = new System.Windows.Forms.Label();
            driverLoadedLbl = new System.Windows.Forms.Label();
            driverLoadedLblTimer = new System.Windows.Forms.Timer(components);
            label8 = new System.Windows.Forms.Label();
            label9 = new System.Windows.Forms.Label();
            label10 = new System.Windows.Forms.Label();
            label11 = new System.Windows.Forms.Label();
            label12 = new System.Windows.Forms.Label();
            label13 = new System.Windows.Forms.Label();
            label14 = new System.Windows.Forms.Label();
            advisoryBox = new System.Windows.Forms.TextBox();
            imageSizeBox = new System.Windows.Forms.TextBox();
            fileHashBox = new System.Windows.Forms.TextBox();
            authHashBox = new System.Windows.Forms.TextBox();
            pageHashSha1Box = new System.Windows.Forms.TextBox();
            pageHashSha256Box = new System.Windows.Forms.TextBox();
            defaultProviderIDBox = new System.Windows.Forms.TextBox();
            setDefaultProviderBtn = new System.Windows.Forms.Button();
            testProviderBtn = new System.Windows.Forms.Button();
            shellcodeMaskBox = new System.Windows.Forms.TextBox();
            driverWhqlSignedBox = new System.Windows.Forms.CheckBox();
            minWinBuildBox = new System.Windows.Forms.TextBox();
            maxWinBuildBox = new System.Windows.Forms.TextBox();
            signerNameBox = new System.Windows.Forms.TextBox();
            deviceNameBox = new System.Windows.Forms.TextBox();
            driverNameBox = new System.Windows.Forms.TextBox();
            providerExtraInfoBox = new System.Windows.Forms.TextBox();
            providerList = new System.Windows.Forms.ListView();
            provIdCol = new System.Windows.Forms.ColumnHeader();
            provNameCol = new System.Windows.Forms.ColumnHeader();
            wipeSettingsBtn = new System.Windows.Forms.Button();
            providerFormFix = new System.Windows.Forms.Timer(components);
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(676, 748);
            label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(147, 20);
            label1.TabIndex = 45;
            label1.Text = "Provider Extra Info";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(676, 12);
            label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(104, 20);
            label2.TabIndex = 14;
            label2.Text = "Driver Name";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(676, 68);
            label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(110, 20);
            label3.TabIndex = 16;
            label3.Text = "Device Name";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(676, 124);
            label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(106, 20);
            label4.TabIndex = 18;
            label4.Text = "Signer Name";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(676, 236);
            label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(190, 20);
            label5.TabIndex = 22;
            label5.Text = "Minimum Windows build";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new System.Drawing.Point(676, 180);
            label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(194, 20);
            label6.TabIndex = 20;
            label6.Text = "Maximum Windows build";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new System.Drawing.Point(676, 292);
            label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(188, 20);
            label7.TabIndex = 25;
            label7.Text = "Shellcode support mask";
            // 
            // driverLoadedLbl
            // 
            driverLoadedLbl.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            driverLoadedLbl.AutoSize = true;
            driverLoadedLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            driverLoadedLbl.Location = new System.Drawing.Point(164, 900);
            driverLoadedLbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            driverLoadedLbl.Name = "driverLoadedLbl";
            driverLoadedLbl.Size = new System.Drawing.Size(200, 32);
            driverLoadedLbl.TabIndex = 48;
            driverLoadedLbl.Text = "Driver Loaded!";
            driverLoadedLbl.Visible = false;
            // 
            // driverLoadedLblTimer
            // 
            driverLoadedLblTimer.Interval = 2500;
            driverLoadedLblTimer.Tick += driverLoadedLblTimer_Tick;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new System.Drawing.Point(676, 348);
            label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label8.Name = "label8";
            label8.Size = new System.Drawing.Size(152, 20);
            label8.TabIndex = 29;
            label8.Text = "Default Provider ID";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new System.Drawing.Point(676, 404);
            label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label9.Name = "label9";
            label9.Size = new System.Drawing.Size(73, 20);
            label9.TabIndex = 32;
            label9.Text = "Advisory";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new System.Drawing.Point(676, 460);
            label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label10.Name = "label10";
            label10.Size = new System.Drawing.Size(92, 20);
            label10.TabIndex = 34;
            label10.Text = "Image Size";
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new System.Drawing.Point(676, 516);
            label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label11.Name = "label11";
            label11.Size = new System.Drawing.Size(142, 20);
            label11.TabIndex = 36;
            label11.Text = "File Hash (SHA1)";
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new System.Drawing.Point(676, 572);
            label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label12.Name = "label12";
            label12.Size = new System.Drawing.Size(212, 20);
            label12.TabIndex = 38;
            label12.Text = "Authenticode Hash (SHA1)";
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Location = new System.Drawing.Point(676, 628);
            label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label13.Name = "label13";
            label13.Size = new System.Drawing.Size(153, 20);
            label13.TabIndex = 40;
            label13.Text = "Page Hash (SHA1)";
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Location = new System.Drawing.Point(676, 684);
            label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label14.Name = "label14";
            label14.Size = new System.Drawing.Size(171, 20);
            label14.TabIndex = 42;
            label14.Text = "Page Hash (SHA256)";
            // 
            // advisoryBox
            // 
            advisoryBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            advisoryBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            advisoryBox.Location = new System.Drawing.Point(681, 426);
            advisoryBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            advisoryBox.Name = "advisoryBox";
            advisoryBox.ReadOnly = true;
            advisoryBox.Size = new System.Drawing.Size(655, 26);
            advisoryBox.TabIndex = 33;
            // 
            // imageSizeBox
            // 
            imageSizeBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            imageSizeBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            imageSizeBox.Location = new System.Drawing.Point(681, 482);
            imageSizeBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            imageSizeBox.Name = "imageSizeBox";
            imageSizeBox.ReadOnly = true;
            imageSizeBox.Size = new System.Drawing.Size(655, 26);
            imageSizeBox.TabIndex = 35;
            // 
            // fileHashBox
            // 
            fileHashBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            fileHashBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            fileHashBox.Location = new System.Drawing.Point(681, 538);
            fileHashBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            fileHashBox.Name = "fileHashBox";
            fileHashBox.ReadOnly = true;
            fileHashBox.Size = new System.Drawing.Size(655, 26);
            fileHashBox.TabIndex = 37;
            // 
            // authHashBox
            // 
            authHashBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            authHashBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            authHashBox.Location = new System.Drawing.Point(681, 594);
            authHashBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            authHashBox.Name = "authHashBox";
            authHashBox.ReadOnly = true;
            authHashBox.Size = new System.Drawing.Size(655, 26);
            authHashBox.TabIndex = 39;
            // 
            // pageHashSha1Box
            // 
            pageHashSha1Box.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            pageHashSha1Box.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            pageHashSha1Box.Location = new System.Drawing.Point(681, 650);
            pageHashSha1Box.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            pageHashSha1Box.Name = "pageHashSha1Box";
            pageHashSha1Box.ReadOnly = true;
            pageHashSha1Box.Size = new System.Drawing.Size(655, 26);
            pageHashSha1Box.TabIndex = 41;
            // 
            // pageHashSha256Box
            // 
            pageHashSha256Box.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            pageHashSha256Box.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            pageHashSha256Box.Location = new System.Drawing.Point(681, 706);
            pageHashSha256Box.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            pageHashSha256Box.Name = "pageHashSha256Box";
            pageHashSha256Box.ReadOnly = true;
            pageHashSha256Box.Size = new System.Drawing.Size(655, 26);
            pageHashSha256Box.TabIndex = 43;
            // 
            // defaultProviderIDBox
            // 
            defaultProviderIDBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            defaultProviderIDBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            defaultProviderIDBox.Location = new System.Drawing.Point(681, 370);
            defaultProviderIDBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            defaultProviderIDBox.Name = "defaultProviderIDBox";
            defaultProviderIDBox.ReadOnly = true;
            defaultProviderIDBox.Size = new System.Drawing.Size(655, 26);
            defaultProviderIDBox.TabIndex = 30;
            // 
            // setDefaultProviderBtn
            // 
            setDefaultProviderBtn.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            setDefaultProviderBtn.Location = new System.Drawing.Point(1133, 900);
            setDefaultProviderBtn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            setDefaultProviderBtn.Name = "setDefaultProviderBtn";
            setDefaultProviderBtn.Size = new System.Drawing.Size(209, 34);
            setDefaultProviderBtn.TabIndex = 49;
            setDefaultProviderBtn.Text = "Set Default Provider";
            setDefaultProviderBtn.UseVisualStyleBackColor = true;
            setDefaultProviderBtn.Click += setDefaultProviderBtn_Click;
            // 
            // testProviderBtn
            // 
            testProviderBtn.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            testProviderBtn.Location = new System.Drawing.Point(13, 900);
            testProviderBtn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            testProviderBtn.Name = "testProviderBtn";
            testProviderBtn.Size = new System.Drawing.Size(143, 34);
            testProviderBtn.TabIndex = 47;
            testProviderBtn.Text = "Test Driver";
            testProviderBtn.UseVisualStyleBackColor = true;
            testProviderBtn.Click += testProviderBtn_Click;
            // 
            // shellcodeMaskBox
            // 
            shellcodeMaskBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            shellcodeMaskBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            shellcodeMaskBox.Location = new System.Drawing.Point(681, 314);
            shellcodeMaskBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            shellcodeMaskBox.Name = "shellcodeMaskBox";
            shellcodeMaskBox.ReadOnly = true;
            shellcodeMaskBox.Size = new System.Drawing.Size(655, 26);
            shellcodeMaskBox.TabIndex = 24;
            // 
            // driverWhqlSignedBox
            // 
            driverWhqlSignedBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            driverWhqlSignedBox.AutoSize = true;
            driverWhqlSignedBox.Location = new System.Drawing.Point(1121, 744);
            driverWhqlSignedBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            driverWhqlSignedBox.Name = "driverWhqlSignedBox";
            driverWhqlSignedBox.Size = new System.Drawing.Size(212, 24);
            driverWhqlSignedBox.TabIndex = 44;
            driverWhqlSignedBox.Text = "Driver is WHQL Signed";
            driverWhqlSignedBox.UseVisualStyleBackColor = true;
            // 
            // minWinBuildBox
            // 
            minWinBuildBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            minWinBuildBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            minWinBuildBox.Location = new System.Drawing.Point(681, 258);
            minWinBuildBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            minWinBuildBox.Name = "minWinBuildBox";
            minWinBuildBox.ReadOnly = true;
            minWinBuildBox.Size = new System.Drawing.Size(655, 26);
            minWinBuildBox.TabIndex = 21;
            // 
            // maxWinBuildBox
            // 
            maxWinBuildBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            maxWinBuildBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            maxWinBuildBox.Location = new System.Drawing.Point(681, 202);
            maxWinBuildBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            maxWinBuildBox.Name = "maxWinBuildBox";
            maxWinBuildBox.ReadOnly = true;
            maxWinBuildBox.Size = new System.Drawing.Size(655, 26);
            maxWinBuildBox.TabIndex = 19;
            // 
            // signerNameBox
            // 
            signerNameBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            signerNameBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            signerNameBox.Location = new System.Drawing.Point(681, 146);
            signerNameBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            signerNameBox.Name = "signerNameBox";
            signerNameBox.ReadOnly = true;
            signerNameBox.Size = new System.Drawing.Size(655, 26);
            signerNameBox.TabIndex = 17;
            // 
            // deviceNameBox
            // 
            deviceNameBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            deviceNameBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            deviceNameBox.Location = new System.Drawing.Point(681, 90);
            deviceNameBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            deviceNameBox.Name = "deviceNameBox";
            deviceNameBox.ReadOnly = true;
            deviceNameBox.Size = new System.Drawing.Size(655, 26);
            deviceNameBox.TabIndex = 15;
            // 
            // driverNameBox
            // 
            driverNameBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            driverNameBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            driverNameBox.Location = new System.Drawing.Point(681, 34);
            driverNameBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            driverNameBox.Name = "driverNameBox";
            driverNameBox.ReadOnly = true;
            driverNameBox.Size = new System.Drawing.Size(655, 26);
            driverNameBox.TabIndex = 13;
            // 
            // providerExtraInfoBox
            // 
            providerExtraInfoBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            providerExtraInfoBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            providerExtraInfoBox.Location = new System.Drawing.Point(681, 770);
            providerExtraInfoBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            providerExtraInfoBox.Multiline = true;
            providerExtraInfoBox.Name = "providerExtraInfoBox";
            providerExtraInfoBox.ReadOnly = true;
            providerExtraInfoBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            providerExtraInfoBox.Size = new System.Drawing.Size(655, 117);
            providerExtraInfoBox.TabIndex = 46;
            // 
            // providerList
            // 
            providerList.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            providerList.BackColor = System.Drawing.Color.FromArgb(33, 33, 33);
            providerList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { provIdCol, provNameCol });
            providerList.ForeColor = System.Drawing.Color.Silver;
            providerList.FullRowSelect = true;
            providerList.Location = new System.Drawing.Point(13, 12);
            providerList.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            providerList.Name = "providerList";
            providerList.Size = new System.Drawing.Size(644, 875);
            providerList.TabIndex = 10;
            providerList.UseCompatibleStateImageBehavior = false;
            providerList.View = System.Windows.Forms.View.Details;
            providerList.SelectedIndexChanged += providerList_SelectedIndexChanged;
            // 
            // provIdCol
            // 
            provIdCol.Text = "ID";
            provIdCol.Width = 38;
            // 
            // provNameCol
            // 
            provNameCol.Text = "Provider Name";
            provNameCol.Width = 602;
            // 
            // wipeSettingsBtn
            // 
            wipeSettingsBtn.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            wipeSettingsBtn.Location = new System.Drawing.Point(916, 900);
            wipeSettingsBtn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            wipeSettingsBtn.Name = "wipeSettingsBtn";
            wipeSettingsBtn.Size = new System.Drawing.Size(209, 34);
            wipeSettingsBtn.TabIndex = 50;
            wipeSettingsBtn.Text = "Wipe Settings";
            wipeSettingsBtn.UseVisualStyleBackColor = true;
            wipeSettingsBtn.Click += wipeSettingsBtn_Click;
            // 
            // providerFormFix
            // 
            providerFormFix.Interval = 125;
            providerFormFix.Tick += providerFormFix_Tick;
            // 
            // ProviderSelector
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1356, 950);
            Controls.Add(wipeSettingsBtn);
            Controls.Add(setDefaultProviderBtn);
            Controls.Add(defaultProviderIDBox);
            Controls.Add(label8);
            Controls.Add(pageHashSha256Box);
            Controls.Add(label14);
            Controls.Add(pageHashSha1Box);
            Controls.Add(label13);
            Controls.Add(authHashBox);
            Controls.Add(label12);
            Controls.Add(fileHashBox);
            Controls.Add(label11);
            Controls.Add(imageSizeBox);
            Controls.Add(label10);
            Controls.Add(advisoryBox);
            Controls.Add(label9);
            Controls.Add(driverLoadedLbl);
            Controls.Add(testProviderBtn);
            Controls.Add(label7);
            Controls.Add(shellcodeMaskBox);
            Controls.Add(driverWhqlSignedBox);
            Controls.Add(label5);
            Controls.Add(minWinBuildBox);
            Controls.Add(label6);
            Controls.Add(maxWinBuildBox);
            Controls.Add(label4);
            Controls.Add(signerNameBox);
            Controls.Add(label3);
            Controls.Add(deviceNameBox);
            Controls.Add(label2);
            Controls.Add(driverNameBox);
            Controls.Add(label1);
            Controls.Add(providerExtraInfoBox);
            Controls.Add(providerList);
            DoubleBuffered = true;
            MinimumSize = new System.Drawing.Size(998, 700);
            Name = "ProviderSelector";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "KsDumper 11 - Provider Selection";
            Load += ProviderSelector_Load;
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ListView providerList;
        private System.Windows.Forms.ColumnHeader provNameCol;
        private System.Windows.Forms.TextBox providerExtraInfoBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox driverNameBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox deviceNameBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox signerNameBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox minWinBuildBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox maxWinBuildBox;
        private System.Windows.Forms.CheckBox driverWhqlSignedBox;
        private System.Windows.Forms.ColumnHeader provIdCol;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox shellcodeMaskBox;
        private System.Windows.Forms.Button testProviderBtn;
        private System.Windows.Forms.Label driverLoadedLbl;
        private System.Windows.Forms.Timer driverLoadedLblTimer;
        private System.Windows.Forms.Button setDefaultProviderBtn;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox defaultProviderIDBox;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox advisoryBox;
        private System.Windows.Forms.TextBox imageSizeBox;
        private System.Windows.Forms.TextBox fileHashBox;
        private System.Windows.Forms.TextBox authHashBox;
        private System.Windows.Forms.TextBox pageHashSha1Box;
        private System.Windows.Forms.TextBox pageHashSha256Box;
        private System.Windows.Forms.Button wipeSettingsBtn;
        private System.Windows.Forms.Timer providerFormFix;
    }
}