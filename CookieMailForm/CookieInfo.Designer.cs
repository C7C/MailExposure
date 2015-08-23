namespace MailExposure
{
    partial class CookieInfo
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.textBoxCookieRemarks = new System.Windows.Forms.TextBox();
            this.comboBoxUserlist = new System.Windows.Forms.ComboBox();
            this.buttonCookieTxt = new System.Windows.Forms.Button();
            this.checkBoxAuto = new System.Windows.Forms.CheckBox();
            this.comboBoxCookieType = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxCookiePath = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.panel1.Controls.Add(this.textBoxCookiePath);
            this.panel1.Controls.Add(this.buttonCancel);
            this.panel1.Controls.Add(this.buttonOK);
            this.panel1.Controls.Add(this.textBoxCookieRemarks);
            this.panel1.Controls.Add(this.comboBoxUserlist);
            this.panel1.Controls.Add(this.buttonCookieTxt);
            this.panel1.Controls.Add(this.checkBoxAuto);
            this.panel1.Controls.Add(this.comboBoxCookieType);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.ForeColor = System.Drawing.SystemColors.WindowText;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(429, 247);
            this.panel1.TabIndex = 0;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(222, 203);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 7;
            this.buttonCancel.Text = "取消";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(100, 203);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 7;
            this.buttonOK.Text = "确定";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // textBoxCookieRemarks
            // 
            this.textBoxCookieRemarks.Location = new System.Drawing.Point(100, 150);
            this.textBoxCookieRemarks.Name = "textBoxCookieRemarks";
            this.textBoxCookieRemarks.Size = new System.Drawing.Size(198, 21);
            this.textBoxCookieRemarks.TabIndex = 6;
            // 
            // comboBoxUserlist
            // 
            this.comboBoxUserlist.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxUserlist.FormattingEnabled = true;
            this.comboBoxUserlist.Location = new System.Drawing.Point(100, 108);
            this.comboBoxUserlist.Name = "comboBoxUserlist";
            this.comboBoxUserlist.Size = new System.Drawing.Size(197, 20);
            this.comboBoxUserlist.TabIndex = 5;
            // 
            // buttonCookieTxt
            // 
            this.buttonCookieTxt.Image = global::MailExposure.PrintRibbonControllerResources.RibbonPrintPreview_ExportTxt;
            this.buttonCookieTxt.Location = new System.Drawing.Point(316, 62);
            this.buttonCookieTxt.Name = "buttonCookieTxt";
            this.buttonCookieTxt.Size = new System.Drawing.Size(75, 31);
            this.buttonCookieTxt.TabIndex = 4;
            this.buttonCookieTxt.Text = "打开";
            this.buttonCookieTxt.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonCookieTxt.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buttonCookieTxt.UseVisualStyleBackColor = true;
            this.buttonCookieTxt.Click += new System.EventHandler(this.buttonCookieTxt_Click);
            // 
            // checkBoxAuto
            // 
            this.checkBoxAuto.AutoSize = true;
            this.checkBoxAuto.Checked = true;
            this.checkBoxAuto.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxAuto.Location = new System.Drawing.Point(319, 26);
            this.checkBoxAuto.Name = "checkBoxAuto";
            this.checkBoxAuto.Size = new System.Drawing.Size(72, 16);
            this.checkBoxAuto.TabIndex = 2;
            this.checkBoxAuto.Text = "自动收取";
            this.checkBoxAuto.UseVisualStyleBackColor = true;
            // 
            // comboBoxCookieType
            // 
            this.comboBoxCookieType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxCookieType.FormattingEnabled = true;
            this.comboBoxCookieType.Items.AddRange(new object[] {
            "Yahoo-Txt",
            "Yahoo(Japan)-txt",
            "Hotmail-Txt",
            "Gmail-Txt",
            "Hinet-Txt",
            "Ru-Txt",
            "FastMail-Txt",
            "TomMail-Txt",
            "SinaMail-Txt",
            "21CNMail-Txt",
            "163-Txt",
            "126-Txt",
            "263-Txt",
            "Aol-Txt"});
            this.comboBoxCookieType.Location = new System.Drawing.Point(101, 24);
            this.comboBoxCookieType.Name = "comboBoxCookieType";
            this.comboBoxCookieType.Size = new System.Drawing.Size(197, 20);
            this.comboBoxCookieType.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(17, 153);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 12);
            this.label4.TabIndex = 0;
            this.label4.Text = "Cookie标识:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 111);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "用户列表:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "Txt文件路径:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "Cookie类型:";
            // 
            // textBoxCookiePath
            // 
            this.textBoxCookiePath.Location = new System.Drawing.Point(100, 66);
            this.textBoxCookiePath.Multiline = true;
            this.textBoxCookiePath.Name = "textBoxCookiePath";
            this.textBoxCookiePath.Size = new System.Drawing.Size(198, 21);
            this.textBoxCookiePath.TabIndex = 8;
            // 
            // CookieInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(429, 247);
            this.Controls.Add(this.panel1);
            this.Name = "CookieInfo";
            this.Text = "FishCookie";
            this.Load += new System.EventHandler(this.CookieInfo_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox checkBoxAuto;
        private System.Windows.Forms.ComboBox comboBoxCookieType;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonCookieTxt;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.TextBox textBoxCookieRemarks;
        private System.Windows.Forms.ComboBox comboBoxUserlist;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxCookiePath;
    }
}