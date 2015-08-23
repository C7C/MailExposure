using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MailExposure.CookieMailForm
{
    public partial class frmSetting : Form
    {
        public frmSetting()
        {
            InitializeComponent();
        }

        private void buttonBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.ShowDialog();
            if (dialog.SelectedPath.Trim() != "")
            {
                string str = dialog.SelectedPath.Trim();
                if (str.LastIndexOf(@"\") != (str.Length - 1))
                {
                    str = str + @"\";
                }
                this.textBoxFileofPath.Text = str;
            }

        }

        private void buttonQuit_Click(object sender, EventArgs e)
        {
            base.Close();
            base.Dispose();
        }

        private void buttonSaveSetting_Click(object sender, EventArgs e)
        {
            string text = this.dateTimePickerTime.Text;
            int index = text.IndexOf("星期");
            if (index > 0)
            {
                text = text.Substring(0, index);
            }
            Setting.SetIniValue("CookieEmail", "File Path", this.textBoxFileofPath.Text, AppDomain.CurrentDomain.BaseDirectory + @"\cookieSetting.ini");
            Setting.SetIniValue("CookieEmail", "Receive IntervalTime", this.comboBoxReceiveinterval.Text, AppDomain.CurrentDomain.BaseDirectory + @"\cookieSetting.ini");
            Setting.SetIniValue("CookieEmail", "Max Threads", this.comboBoxMaxThreads.Text, AppDomain.CurrentDomain.BaseDirectory + @"\cookieSetting.ini");
            Setting.SetIniValue("CookieEmail", "Save Attachment", this.comboBoxSaveAttachment.Text, AppDomain.CurrentDomain.BaseDirectory + @"\cookieSetting.ini");
            Setting.SetIniValue("CookieEmail", "EmailDateTime", text, AppDomain.CurrentDomain.BaseDirectory + @"\cookieSetting.ini");
            Setting.SetIniValue("CookieEmail", "Page Number", this.comboBoxPageNumber.Text, AppDomain.CurrentDomain.BaseDirectory + @"\cookieSetting.ini");
           // Setting.SetIniValue("CookieEmail", "MP3", this.textBoxMp3.Text, AppDomain.CurrentDomain.BaseDirectory + @"\cookieSetting.ini");
            Setting.SetIniValue("CookieEmail", "IsDataSave", this.checkDateSave.Checked.ToString(), AppDomain.CurrentDomain.BaseDirectory + @"\cookieSetting.ini");
            MessageBox.Show("保存成功");
            if (this.textBoxFileofPath.Text != "")
            {
                GlobalValue.mainForm.saveFilePath = this.textBoxFileofPath.Text;
            }
            GlobalValue.mainForm.TimeInterval = this.comboBoxReceiveinterval.Text;
            GlobalValue.mainForm.MaxThreads = Convert.ToInt32(this.comboBoxMaxThreads.Text);
            GlobalValue.mainForm.PageNumber = this.comboBoxPageNumber.Text;
            GlobalValue.mainForm.checkDataSave = this.checkDateSave.Checked;
            GlobalValue.mainForm.SaveAttachment = this.comboBoxSaveAttachment.Text == "是";
            string str2 = GlobalValue.mainForm.TimeInterval.Substring(0, GlobalValue.mainForm.TimeInterval.IndexOf(" "));
            if (str2 != "")
            {
                GlobalValue.mainForm.timerAutoReceive.Interval = 0x36ee80 * Convert.ToInt32(str2);
            }
            else
            {
                GlobalValue.mainForm.timerAutoReceive.Interval = 0x36ee80 * Convert.ToInt32("1");
            }
            string str3 = Setting.GetIniValue("CookieEmail", "EmailDateTime", "无限制", AppDomain.CurrentDomain.BaseDirectory + @"\cookieSetting.ini");
            if (str3.Trim() == "无限制")
            {
                GlobalValue.mainForm.EmailDateTime = DateTime.Now.AddYears(-50);
            }
            else if (str3.Trim().IndexOf(" ") != -1)
            {
                str3 = str3.Substring(0, str3.IndexOf(" "));
                if (str3 != "")
                {
                    GlobalValue.mainForm.EmailDateTime = DateTime.Now.AddMonths(Convert.ToInt32("-" + str3));
                }
                else
                {
                    GlobalValue.mainForm.EmailDateTime = DateTime.Now.AddYears(-50);
                }
            }
            else
            {
                GlobalValue.mainForm.EmailDateTime = Convert.ToDateTime(str3.Trim());
            }
            base.Close();

        }

        private void frmSetting_Load(object sender, EventArgs e)
        {
            this.textBoxFileofPath.Text = Setting.GetIniValue("CookieEmail", "File Path", "", AppDomain.CurrentDomain.BaseDirectory + @"\cookieSetting.ini");
            this.comboBoxReceiveinterval.Text = Setting.GetIniValue("CookieEmail", "Receive IntervalTime", "1  小时", AppDomain.CurrentDomain.BaseDirectory + @"\cookieSetting.ini");
            this.comboBoxMaxThreads.Text = Setting.GetIniValue("CookieEmail", "Max Threads", "10", AppDomain.CurrentDomain.BaseDirectory + @"\cookieSetting.ini");
            this.comboBoxSaveAttachment.Text = Setting.GetIniValue("CookieEmail", "Save Attachment", "是", AppDomain.CurrentDomain.BaseDirectory + @"\cookieSetting.ini");
            string str = Setting.GetIniValue("CookieEmail", "EmailDateTime", "无限制", AppDomain.CurrentDomain.BaseDirectory + @"\cookieSetting.ini");
            this.comboBoxPageNumber.Text = Setting.GetIniValue("CookieEmail", "Page Number", "0", AppDomain.CurrentDomain.BaseDirectory + @"\cookieSetting.ini");
         //   this.textBoxMp3.Text = Setting.GetIniValue("CookieEmail", "MP3", AppDomain.CurrentDomain.BaseDirectory + @"\Soud\msg.mp3", AppDomain.CurrentDomain.BaseDirectory + @"\cookieSetting.ini");
            this.checkDateSave.Checked = "True" == Setting.GetIniValue("CookieEmail", "IsDataSave", "True", AppDomain.CurrentDomain.BaseDirectory + @"\cookieSetting.ini");
            if (str.Trim() == "无限制")
            {
                this.dateTimePickerTime.Text = DateTime.Now.AddYears(-50).ToString();
            }
            else if (str.Trim().IndexOf(" ") != -1)
            {
                str = str.Substring(0, str.IndexOf(" "));
                if (str != "")
                {
                    this.dateTimePickerTime.Text = DateTime.Now.AddMonths(Convert.ToInt32("-" + str)).ToString();
                }
                else
                {
                    this.dateTimePickerTime.Text = DateTime.Now.AddYears(-50).ToString();
                }
            }
            else
            {
                this.dateTimePickerTime.Text = Convert.ToDateTime(str.Trim()).ToString();
            }
            if (this.textBoxFileofPath.Text != "")
            {
                GlobalValue.mainForm.saveFilePath = this.textBoxFileofPath.Text;
            }
            GlobalValue.mainForm.TimeInterval = this.comboBoxReceiveinterval.Text;
            GlobalValue.mainForm.MaxThreads = Convert.ToInt32(this.comboBoxMaxThreads.Text);
            GlobalValue.mainForm.SaveAttachment = this.comboBoxSaveAttachment.Text == "是";
            GlobalValue.mainForm.PageNumber = this.comboBoxPageNumber.Text;

        }
    }
}
