using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MailExposure.POPMailForm
{
    public partial class PopMailSetting : Form
    {
        public PopMailSetting()
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
            Setting.SetIniValue("Email", "File Path", this.textBoxFileofPath.Text, AppDomain.CurrentDomain.BaseDirectory + @"\EmailSetting.ini");
            Setting.SetIniValue("Email", "Receive IntervalTime", this.comboBoxReceiveinterval.Text, AppDomain.CurrentDomain.BaseDirectory + @"\EmailSetting.ini");
            Setting.SetIniValue("Email", "Save Attachment", this.comboBoxReceiveEnclosures.Text, AppDomain.CurrentDomain.BaseDirectory + @"\EmailSetting.ini");
            Setting.SetIniValue("Email", "EmailDateTime", text, AppDomain.CurrentDomain.BaseDirectory + @"\EmailSetting.ini");
            Setting.SetIniValue("Email", "Page Number", this.comboBoxPageNumber.Text, AppDomain.CurrentDomain.BaseDirectory + @"\EmailSetting.ini");
            Setting.SetIniValue("Email", "IsDataSave", this.checkDateSave.Checked.ToString(), AppDomain.CurrentDomain.BaseDirectory + @"\EmailSetting.ini");
            MessageBox.Show("保存成功");
            string text1 = this.dateTimePickerTime.Text;
            GlobalValue.PopMainForm.saveFilePath = this.textBoxFileofPath.Text;
            GlobalValue.PopMainForm.TimeInterval = this.comboBoxReceiveinterval.Text;
            GlobalValue.PopMainForm.PageNumber = this.comboBoxPageNumber.Text;
            GlobalValue.PopMainForm.checkDateSave = this.checkDateSave.Checked;
            string str2 = GlobalValue.PopMainForm.TimeInterval.Substring(0, GlobalValue.PopMainForm.TimeInterval.IndexOf(" "));
            if (str2 != "")
            {
                GlobalValue.PopMainForm.timerAutoReceive.Interval = 0x36ee80 * Convert.ToInt32(str2);
            }
            else
            {
                GlobalValue.PopMainForm.timerAutoReceive.Interval = 0x36ee80 * Convert.ToInt32("1");
            }
            string str3 = Setting.GetIniValue("Email", "EmailDateTime", "无限制", AppDomain.CurrentDomain.BaseDirectory + @"\EmailSetting.ini");
            if (str3.Trim() == "无限制")
            {
                GlobalValue.PopMainForm.EmailDateTime = DateTime.Now.AddYears(-50);
            }
            else if (str3.Trim().IndexOf(" ") != -1)
            {
                str3 = str3.Substring(0, str3.IndexOf(" "));
                if (str3 != "")
                {
                    GlobalValue.PopMainForm.EmailDateTime = DateTime.Now.AddMonths(Convert.ToInt32("-" + str3));
                }
                else
                {
                    GlobalValue.PopMainForm.EmailDateTime = DateTime.Now.AddYears(-50);
                }
            }
            else
            {
                GlobalValue.PopMainForm.EmailDateTime = Convert.ToDateTime(str3.Trim());
            }

        }

        private void PopMailSetting_Load(object sender, EventArgs e)
        {
            this.textBoxFileofPath.Text = Setting.GetIniValue("Email", "File Path", "", AppDomain.CurrentDomain.BaseDirectory + @"\EmailSetting.ini");
            this.comboBoxReceiveinterval.Text = Setting.GetIniValue("Email", "Receive IntervalTime", "1  小时", AppDomain.CurrentDomain.BaseDirectory + @"\EmailSetting.ini");
            this.comboBoxReceiveEnclosures.Text = Setting.GetIniValue("Email", "Save Attachment", "是", AppDomain.CurrentDomain.BaseDirectory + @"\EmailSetting.ini");
            string str = Setting.GetIniValue("Email", "EmailDateTime", "无限制", AppDomain.CurrentDomain.BaseDirectory + @"\EmailSetting.ini");
            this.comboBoxPageNumber.Text = Setting.GetIniValue("Email", "Page Number", "0", AppDomain.CurrentDomain.BaseDirectory + @"\EmailSetting.ini");
            this.checkDateSave.Checked = "True" == Setting.GetIniValue("Email", "IsDataSave", "True", AppDomain.CurrentDomain.BaseDirectory + @"\EmailSetting.ini");
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
            GlobalValue.PopMainForm.saveFilePath = this.textBoxFileofPath.Text;
            GlobalValue.PopMainForm.TimeInterval = this.comboBoxReceiveinterval.Text;
            GlobalValue.PopMainForm.SaveAttachment = this.comboBoxReceiveEnclosures.Text == "是";
            GlobalValue.PopMainForm.PageNumber = this.comboBoxPageNumber.Text;

        }
    }
}
