namespace MailExposure.POPMailForm
{
    partial class PopfrmAddUsers
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
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxSequencenumberText = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttonQuit = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.checkBoxReceiveTime = new System.Windows.Forms.CheckBox();
            this.checkBoxReceiveAll = new System.Windows.Forms.CheckBox();
            this.comboBoxServeradd = new System.Windows.Forms.ComboBox();
            this.comboBoxServer = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxPassword = new System.Windows.Forms.TextBox();
            this.textBoxUserName = new System.Windows.Forms.TextBox();
            this.textBoxOwnerText = new System.Windows.Forms.TextBox();
            this.textBoxdirectionText = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.checkBoxControlGuard = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "序号:";
            // 
            // textBoxSequencenumberText
            // 
            this.textBoxSequencenumberText.Location = new System.Drawing.Point(64, 31);
            this.textBoxSequencenumberText.Name = "textBoxSequencenumberText";
            this.textBoxSequencenumberText.ReadOnly = true;
            this.textBoxSequencenumberText.Size = new System.Drawing.Size(100, 21);
            this.textBoxSequencenumberText.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.panel1.Controls.Add(this.buttonQuit);
            this.panel1.Controls.Add(this.buttonOK);
            this.panel1.Controls.Add(this.checkBoxReceiveTime);
            this.panel1.Controls.Add(this.checkBoxReceiveAll);
            this.panel1.Controls.Add(this.comboBoxServeradd);
            this.panel1.Controls.Add(this.comboBoxServer);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.textBoxPassword);
            this.panel1.Controls.Add(this.textBoxUserName);
            this.panel1.Controls.Add(this.textBoxOwnerText);
            this.panel1.Controls.Add(this.textBoxdirectionText);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.checkBoxControlGuard);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.textBoxSequencenumberText);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(349, 227);
            this.panel1.TabIndex = 2;
            // 
            // buttonQuit
            // 
            this.buttonQuit.Location = new System.Drawing.Point(251, 184);
            this.buttonQuit.Name = "buttonQuit";
            this.buttonQuit.Size = new System.Drawing.Size(75, 23);
            this.buttonQuit.TabIndex = 14;
            this.buttonQuit.Text = "取消";
            this.buttonQuit.UseVisualStyleBackColor = true;
            this.buttonQuit.Click += new System.EventHandler(this.buttonQuit_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(155, 184);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 13;
            this.buttonOK.Text = "确定";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // checkBoxReceiveTime
            // 
            this.checkBoxReceiveTime.AutoSize = true;
            this.checkBoxReceiveTime.Location = new System.Drawing.Point(11, 186);
            this.checkBoxReceiveTime.Name = "checkBoxReceiveTime";
            this.checkBoxReceiveTime.Size = new System.Drawing.Size(72, 16);
            this.checkBoxReceiveTime.TabIndex = 12;
            this.checkBoxReceiveTime.Text = "自动接收";
            this.checkBoxReceiveTime.UseVisualStyleBackColor = true;
            // 
            // checkBoxReceiveAll
            // 
            this.checkBoxReceiveAll.AutoSize = true;
            this.checkBoxReceiveAll.Checked = true;
            this.checkBoxReceiveAll.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxReceiveAll.Location = new System.Drawing.Point(251, 145);
            this.checkBoxReceiveAll.Name = "checkBoxReceiveAll";
            this.checkBoxReceiveAll.Size = new System.Drawing.Size(72, 16);
            this.checkBoxReceiveAll.TabIndex = 11;
            this.checkBoxReceiveAll.Text = "接收全部";
            this.checkBoxReceiveAll.UseVisualStyleBackColor = true;
            // 
            // comboBoxServeradd
            // 
            this.comboBoxServeradd.FormattingEnabled = true;
            this.comboBoxServeradd.Location = new System.Drawing.Point(173, 143);
            this.comboBoxServeradd.Name = "comboBoxServeradd";
            this.comboBoxServeradd.Size = new System.Drawing.Size(72, 20);
            this.comboBoxServeradd.TabIndex = 10;
            // 
            // comboBoxServer
            // 
            this.comboBoxServer.FormattingEnabled = true;
            this.comboBoxServer.Items.AddRange(new object[] {
            "Yahoo",
            "Hotmail",
            "Gmail",
            "Ru",
            "Yandex",
            "Rambler",
            "Hinet",
            "163",
            "Vip163",
            "126",
            "QQ",
            "Yeah",
            "Aol",
            "MailHN",
            "POP",
            "IMAP",
            "Tom",
            "Sina",
            "21CN",
            "263",
            "HanMail",
            "FastMail",
            "RediffMail"});
            this.comboBoxServer.Location = new System.Drawing.Point(63, 143);
            this.comboBoxServer.Name = "comboBoxServer";
            this.comboBoxServer.Size = new System.Drawing.Size(102, 20);
            this.comboBoxServer.TabIndex = 9;
            this.comboBoxServer.SelectedIndexChanged += new System.EventHandler(this.comboBoxServer_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(9, 147);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(47, 12);
            this.label6.TabIndex = 8;
            this.label6.Text = "服务器:";
            // 
            // textBoxPassword
            // 
            this.textBoxPassword.Location = new System.Drawing.Point(215, 107);
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.Size = new System.Drawing.Size(114, 21);
            this.textBoxPassword.TabIndex = 7;
            // 
            // textBoxUserName
            // 
            this.textBoxUserName.Location = new System.Drawing.Point(64, 107);
            this.textBoxUserName.Name = "textBoxUserName";
            this.textBoxUserName.Size = new System.Drawing.Size(100, 21);
            this.textBoxUserName.TabIndex = 6;
            // 
            // textBoxOwnerText
            // 
            this.textBoxOwnerText.Location = new System.Drawing.Point(215, 69);
            this.textBoxOwnerText.Name = "textBoxOwnerText";
            this.textBoxOwnerText.Size = new System.Drawing.Size(114, 21);
            this.textBoxOwnerText.TabIndex = 5;
            // 
            // textBoxdirectionText
            // 
            this.textBoxdirectionText.Location = new System.Drawing.Point(64, 69);
            this.textBoxdirectionText.Name = "textBoxdirectionText";
            this.textBoxdirectionText.Size = new System.Drawing.Size(100, 21);
            this.textBoxdirectionText.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(174, 72);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 12);
            this.label3.TabIndex = 3;
            this.label3.Text = "属主:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(174, 112);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 12);
            this.label5.TabIndex = 3;
            this.label5.Text = "密码:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 110);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 12);
            this.label4.TabIndex = 3;
            this.label4.Text = "用户名:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "方向:";
            // 
            // checkBoxControlGuard
            // 
            this.checkBoxControlGuard.AutoSize = true;
            this.checkBoxControlGuard.Checked = true;
            this.checkBoxControlGuard.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxControlGuard.Location = new System.Drawing.Point(215, 34);
            this.checkBoxControlGuard.Name = "checkBoxControlGuard";
            this.checkBoxControlGuard.Size = new System.Drawing.Size(48, 16);
            this.checkBoxControlGuard.TabIndex = 2;
            this.checkBoxControlGuard.Text = "控守";
            this.checkBoxControlGuard.UseVisualStyleBackColor = true;
            // 
            // PopfrmAddUsers
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(349, 227);
            this.Controls.Add(this.panel1);
            this.Name = "PopfrmAddUsers";
            this.Text = "添加用户信息";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxSequencenumberText;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button buttonQuit;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.CheckBox checkBoxReceiveTime;
        private System.Windows.Forms.CheckBox checkBoxReceiveAll;
        private System.Windows.Forms.ComboBox comboBoxServeradd;
        private System.Windows.Forms.ComboBox comboBoxServer;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBoxPassword;
        private System.Windows.Forms.TextBox textBoxUserName;
        private System.Windows.Forms.TextBox textBoxOwnerText;
        private System.Windows.Forms.TextBox textBoxdirectionText;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox checkBoxControlGuard;
    }
}