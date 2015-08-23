namespace MailExposure.CookieMailForm
{
    partial class frmSetting
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
            this.buttonQuit = new System.Windows.Forms.Button();
            this.buttonSaveSetting = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkDateSave = new System.Windows.Forms.CheckBox();
            this.comboBoxPageNumber = new System.Windows.Forms.ComboBox();
            this.dateTimePickerTime = new System.Windows.Forms.DateTimePicker();
            this.comboBoxSaveAttachment = new System.Windows.Forms.ComboBox();
            this.comboBoxMaxThreads = new System.Windows.Forms.ComboBox();
            this.comboBoxReceiveinterval = new System.Windows.Forms.ComboBox();
            this.buttonBrowse = new System.Windows.Forms.Button();
            this.textBoxFileofPath = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.panel1.Controls.Add(this.buttonQuit);
            this.panel1.Controls.Add(this.buttonSaveSetting);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(474, 257);
            this.panel1.TabIndex = 0;
            // 
            // buttonQuit
            // 
            this.buttonQuit.Location = new System.Drawing.Point(387, 225);
            this.buttonQuit.Name = "buttonQuit";
            this.buttonQuit.Size = new System.Drawing.Size(75, 23);
            this.buttonQuit.TabIndex = 1;
            this.buttonQuit.Text = "取消";
            this.buttonQuit.UseVisualStyleBackColor = true;
            this.buttonQuit.Click += new System.EventHandler(this.buttonQuit_Click);
            // 
            // buttonSaveSetting
            // 
            this.buttonSaveSetting.Location = new System.Drawing.Point(260, 225);
            this.buttonSaveSetting.Name = "buttonSaveSetting";
            this.buttonSaveSetting.Size = new System.Drawing.Size(75, 23);
            this.buttonSaveSetting.TabIndex = 1;
            this.buttonSaveSetting.Text = "保存设置";
            this.buttonSaveSetting.UseVisualStyleBackColor = true;
            this.buttonSaveSetting.Click += new System.EventHandler(this.buttonSaveSetting_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.groupBox1.Controls.Add(this.checkDateSave);
            this.groupBox1.Controls.Add(this.comboBoxPageNumber);
            this.groupBox1.Controls.Add(this.dateTimePickerTime);
            this.groupBox1.Controls.Add(this.comboBoxSaveAttachment);
            this.groupBox1.Controls.Add(this.comboBoxMaxThreads);
            this.groupBox1.Controls.Add(this.comboBoxReceiveinterval);
            this.groupBox1.Controls.Add(this.buttonBrowse);
            this.groupBox1.Controls.Add(this.textBoxFileofPath);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(450, 207);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "参数接收设置";
            // 
            // checkDateSave
            // 
            this.checkDateSave.AutoSize = true;
            this.checkDateSave.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkDateSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.checkDateSave.Location = new System.Drawing.Point(230, 165);
            this.checkDateSave.Name = "checkDateSave";
            this.checkDateSave.Size = new System.Drawing.Size(102, 16);
            this.checkDateSave.TabIndex = 7;
            this.checkDateSave.Text = "按日期保存   ";
            this.checkDateSave.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.checkDateSave.UseVisualStyleBackColor = true;
            // 
            // comboBoxPageNumber
            // 
            this.comboBoxPageNumber.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxPageNumber.FormattingEnabled = true;
            this.comboBoxPageNumber.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "15",
            "20",
            "30",
            "40",
            "50",
            "60",
            "70",
            "80",
            "90",
            "100"});
            this.comboBoxPageNumber.Location = new System.Drawing.Point(91, 162);
            this.comboBoxPageNumber.Name = "comboBoxPageNumber";
            this.comboBoxPageNumber.Size = new System.Drawing.Size(121, 20);
            this.comboBoxPageNumber.TabIndex = 6;
            // 
            // dateTimePickerTime
            // 
            this.dateTimePickerTime.Location = new System.Drawing.Point(299, 119);
            this.dateTimePickerTime.Name = "dateTimePickerTime";
            this.dateTimePickerTime.Size = new System.Drawing.Size(121, 21);
            this.dateTimePickerTime.TabIndex = 5;
            // 
            // comboBoxSaveAttachment
            // 
            this.comboBoxSaveAttachment.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSaveAttachment.FormattingEnabled = true;
            this.comboBoxSaveAttachment.Items.AddRange(new object[] {
            "是",
            "否"});
            this.comboBoxSaveAttachment.Location = new System.Drawing.Point(91, 120);
            this.comboBoxSaveAttachment.Name = "comboBoxSaveAttachment";
            this.comboBoxSaveAttachment.Size = new System.Drawing.Size(121, 20);
            this.comboBoxSaveAttachment.TabIndex = 4;
            // 
            // comboBoxMaxThreads
            // 
            this.comboBoxMaxThreads.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxMaxThreads.FormattingEnabled = true;
            this.comboBoxMaxThreads.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "8",
            "10",
            "15",
            "20",
            "25",
            "30"});
            this.comboBoxMaxThreads.Location = new System.Drawing.Point(299, 76);
            this.comboBoxMaxThreads.Name = "comboBoxMaxThreads";
            this.comboBoxMaxThreads.Size = new System.Drawing.Size(121, 20);
            this.comboBoxMaxThreads.TabIndex = 3;
            // 
            // comboBoxReceiveinterval
            // 
            this.comboBoxReceiveinterval.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxReceiveinterval.FormattingEnabled = true;
            this.comboBoxReceiveinterval.Items.AddRange(new object[] {
            "1  小时",
            "2  小时",
            "3  小时",
            "4  小时",
            "5  小时",
            "6  小时",
            "7  小时",
            "8  小时",
            "9  小时",
            "10 小时",
            "11 小时"});
            this.comboBoxReceiveinterval.Location = new System.Drawing.Point(91, 76);
            this.comboBoxReceiveinterval.Name = "comboBoxReceiveinterval";
            this.comboBoxReceiveinterval.Size = new System.Drawing.Size(121, 20);
            this.comboBoxReceiveinterval.TabIndex = 3;
            // 
            // buttonBrowse
            // 
            this.buttonBrowse.Location = new System.Drawing.Point(345, 28);
            this.buttonBrowse.Name = "buttonBrowse";
            this.buttonBrowse.Size = new System.Drawing.Size(75, 23);
            this.buttonBrowse.TabIndex = 2;
            this.buttonBrowse.Text = "浏览";
            this.buttonBrowse.UseVisualStyleBackColor = true;
            this.buttonBrowse.Click += new System.EventHandler(this.buttonBrowse_Click);
            // 
            // textBoxFileofPath
            // 
            this.textBoxFileofPath.Location = new System.Drawing.Point(91, 28);
            this.textBoxFileofPath.Name = "textBoxFileofPath";
            this.textBoxFileofPath.ReadOnly = true;
            this.textBoxFileofPath.Size = new System.Drawing.Size(232, 21);
            this.textBoxFileofPath.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(228, 80);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "最大线程数:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(228, 125);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(59, 12);
            this.label5.TabIndex = 0;
            this.label5.Text = "时间限制:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(26, 165);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(59, 12);
            this.label6.TabIndex = 0;
            this.label6.Text = "接收页数:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(26, 125);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 12);
            this.label4.TabIndex = 0;
            this.label4.Text = "接收附件:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(26, 80);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "接收间隔:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "保存路径:";
            // 
            // frmSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(474, 257);
            this.Controls.Add(this.panel1);
            this.Name = "frmSetting";
            this.Text = "frmSetting";
            this.Load += new System.EventHandler(this.frmSetting_Load);
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button buttonQuit;
        private System.Windows.Forms.Button buttonSaveSetting;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox checkDateSave;
        private System.Windows.Forms.ComboBox comboBoxPageNumber;
        private System.Windows.Forms.DateTimePicker dateTimePickerTime;
        private System.Windows.Forms.ComboBox comboBoxSaveAttachment;
        private System.Windows.Forms.ComboBox comboBoxMaxThreads;
        private System.Windows.Forms.ComboBox comboBoxReceiveinterval;
        private System.Windows.Forms.Button buttonBrowse;
        private System.Windows.Forms.TextBox textBoxFileofPath;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
    }
}