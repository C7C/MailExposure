using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;

namespace MailExposure.CookieMailForm
{
    public partial class FishCookie : Form
    {
        public FishCookie()
        {
            InitializeComponent();
        }
        private void AddUsersListNode()
        {
            string str = "select * from UsersList";
            GlobalValue.mainForm.oleDbCommand = new OleDbCommand();
            GlobalValue.mainForm.oleDbCommand.CommandText = str;
            GlobalValue.mainForm.oleDbCommand.Connection = GlobalValue.mainForm.oleDbConnection;
            OleDbDataReader reader = GlobalValue.mainForm.oleDbCommand.ExecuteReader();
            this.comboBoxUserlist.Items.Clear();
            this.comboBoxUserlist.DropDownStyle = ComboBoxStyle.DropDown;
            while (reader.Read())
            {
                this.comboBoxUserlist.Items.Add(reader[1].ToString());
            }
            this.comboBoxUserlist.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxUserlist.SelectedIndex = 0;
            reader.Close();
        }


        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (this.textBoxCookieUrl.Text.StartsWith("http://"))
            {
                if (this.textBoxCookieUrl.Text.Length > 7)
                {
                    if (this.textBoxPassWord.Text.Length > 0)
                    {
                        Setting.SetIniValue("FishCookie", "CookieUrl", this.textBoxCookieUrl.Text, AppDomain.CurrentDomain.BaseDirectory + @"\cookieSetting.ini");
                        Setting.SetIniValue("FishCookie", "CookieTime", this.comboBoxCookieTime.Text, AppDomain.CurrentDomain.BaseDirectory + @"\cookieSetting.ini");
                        Setting.SetIniValue("FishCookie", "CookieUserlist", this.comboBoxUserlist.Text, AppDomain.CurrentDomain.BaseDirectory + @"\cookieSetting.ini");
                        GlobalValue.mainForm.CookieUrl = this.textBoxCookieUrl.Text.Trim();
                        GlobalValue.mainForm.CookieTime = this.comboBoxCookieTime.Text.Trim();
                        GlobalValue.mainForm.CookieUserlist = this.comboBoxUserlist.Text.Trim();
                        GlobalValue.mainForm.CookiePassWord = this.textBoxPassWord.Text.Trim();
                        MessageBox.Show("配置信息保存成功!", "提示");
                        GlobalValue.mainForm.自动获取CookieToolStripMenuItem.CheckState = CheckState.Checked;
                        base.Close();
                    }
                    else
                    {
                        MessageBox.Show("密码不能为空!", "警告");
                    }
                }
                else
                {
                    MessageBox.Show("地址不能为空!", "警告");
                }
            }
            else
            {
                MessageBox.Show("地址格式不正确!", "警告");
            }

        }

        private void buttonQuit_Click(object sender, EventArgs e)
        {
            if (GlobalValue.mainForm.自动获取CookieToolStripMenuItem.CheckState == CheckState.Checked)
            {
                if ((((GlobalValue.mainForm.CookieUrl == "") || (GlobalValue.mainForm.CookieTime == "")) || ((GlobalValue.mainForm.CookiePassWord == "") || (GlobalValue.mainForm.CookieUserlist == ""))) && (MessageBox.Show("关闭自动获取Cookies吗?", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes))
                {
                    GlobalValue.mainForm.自动获取CookieToolStripMenuItem.CheckState = CheckState.Unchecked;
                    base.Close();
                    base.Dispose();
                }
            }
            else
            {
                base.Close();
                base.Dispose();
            }

        }

        private void FishCookie_Load(object sender, EventArgs e)
        {
            string str = "";
            string str2 = "";
            string str3 = "";
            str = Setting.GetIniValue("FishCookie", "CookieUrl", "http://", AppDomain.CurrentDomain.BaseDirectory + @"\cookieSetting.ini");
            if (str.Length > 7)
            {
                this.textBoxCookieUrl.Text = str;
            }
            str2 = Setting.GetIniValue("FishCookie", "CookieTime", "10", AppDomain.CurrentDomain.BaseDirectory + @"\cookieSetting.ini");
            if (str2.Length < 1)
            {
                this.comboBoxCookieTime.Text = "10";
            }
            else
            {
                this.comboBoxCookieTime.Text = str2;
            }
            str3 = Setting.GetIniValue("FishCookie", "CookieUserlist", "", AppDomain.CurrentDomain.BaseDirectory + @"\cookieSetting.ini");
            if (str3.Length < 1)
            {
                this.AddUsersListNode();
            }
            else
            {
                this.AddUsersListNode();
                this.comboBoxUserlist.Text = str3;
            }
            this.textBoxPassWord.Text = GlobalValue.mainForm.CookiePassWord;
        }
    }
}
