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
    public partial class PopfrmAddUsers : Form
    {
        public PopfrmAddUsers()
        {
            InitializeComponent();
            if (!this.checkBoxReceiveTime.Checked)
            {
              //  this.comboBoxEndTime.Visible = false;
              //  this.comboBoxStartTime.Visible = false;
              //  this.comboBoxTimeInterval.Visible = false;
              //  this.labelSpaceTime.Visible = false;
               // this.labelStartTime.Visible = false;
              //  this.labelTimeInterval.Visible = false;
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            string str = this.textBoxSequencenumberText.Text.Trim();
            string str2 = this.textBoxdirectionText.Text.Trim();
            str2 = "'" + str2 + "'";
            string str3 = this.textBoxOwnerText.Text.Trim();
            str3 = "'" + str3 + "'";
            string userName = this.textBoxUserName.Text.Trim();
            string passWd = this.textBoxPassword.Text.Trim();
            passWd = "'" + GlobalValue.PopMainForm.Encryption(passWd, userName) + "'";
            userName = "'" + userName + "'";
            string str6 = this.comboBoxServeradd.Text.Trim();
            str6 = "'" + str6 + "'";
            string str7 = this.comboBoxServer.Text.Trim();
            str7 = "'" + str7 + "'";
            if (((userName.IndexOf("@") != -1) && (str7.Trim(new char[] { '\'' }) != "POP")) && (str7.Trim(new char[] { '\'' }) != "FreePOPS"))
            {
                userName = userName.Trim(new char[] { '\'' });
                userName = userName.Substring(0, userName.IndexOf("@"));
                userName = "'" + userName + "'";
            }
            string str8 = this.checkBoxControlGuard.Checked ? "1" : "0";
            str8 = "'" + str8 + "'";
            string str9 = this.checkBoxReceiveAll.Checked ? "是" : "否";
            str9 = "'" + str9 + "'";
            string str10 = "?";
            str10 = "'" + str10 + "'";
            string str11 = "0";
            str11 = "'" + str11 + "'";
            if (str2 == "''")
            {
                MessageBox.Show("方向不能为空！");
            }
            else if (str3 == "''")
            {
                MessageBox.Show("属主不能为空！");
            }
            else if (userName == "''")
            {
                MessageBox.Show("用户名不能为空！");
            }
            else if (passWd == "''")
            {
                MessageBox.Show("密码不能为空！");
            }
            else if (str7 == "''")
            {
                MessageBox.Show("服务器不能为空！");
            }
            else
            {
                string str12 = "";
                string str13 = "";
                string str14 = "";
                string str15 = this.checkBoxReceiveTime.Checked ? "1" : "0";
                if (this.checkBoxReceiveTime.Checked)
                {
                    str12 = "'" + str12 + "'";
                    str13 = "'" + str13 + "'";
                    str14 = "'" + str14 + "'";
                }
                else
                {
                    str12 = "'" + str12 + "'";
                    str13 = "'" + str13 + "'";
                    str14 = "'" + str14 + "'";
                }
                try
                {
                    if (this.textBoxSequencenumberText.Text != "")
                    {
                        string str16 = "属主=" + str3 + ",用户名=" + userName + ",口令字=" + passWd + ",方向=" + str2 + ",服务器=" + str7 + ",服务器地址=" + str6 + ",控守=" + str8 + ",邮箱状态=" + str10 + ",邮件数量=" + str11 + ",接收时间=" + str15 + ",开始时间=" + str12 + ",时间段=" + str13 + ",时间间隔=" + str14 + ",全部接收=" + str9;
                        string strSql = "update users set " + str16 + "where 序号=" + str;
                        GlobalValue.PopMainForm.ExecuteSQL(strSql);
                        MessageBox.Show("编辑用户" + userName.Substring(userName.IndexOf('\'') + 1, userName.IndexOf('\'', userName.IndexOf('\'') + 1) - 1) + "成功");
                    }
                    else
                    {
                        string str18 = "SELECT  count(*)  FROM users WHERE 服务器地址=" + str6 + "and  用户名=" + userName;
                        if ((Convert.ToInt32(GlobalValue.PopMainForm.ExecuteSQL(str18)) > 0) && (this.textBoxSequencenumberText.Text == ""))
                        {
                            MessageBox.Show("此用户已经存在！", "警告");
                        }
                        else
                        {
                            string str20 = "(" + str3 + "," + userName + "," + passWd + "," + str2 + "," + str7 + "," + str6 + "," + str8 + "," + str10 + "," + str11 + "," + str15 + "," + str12 + "," + str13 + "," + str14 + "," + str9 + ",'普通用户')";
                            string str21 = "insert into users (属主,用户名,口令字,方向,服务器,服务器地址,控守,邮箱状态,邮件数量,接收时间,开始时间,时间段,时间间隔,全部接收,用户类型)values" + str20;
                            GlobalValue.PopMainForm.ExecuteSQL(str21);
                            string sqlCountUsers = "select count(*) from users where 用户类型='普通用户'";
                            GlobalValue.PopMainForm.UsersCount(sqlCountUsers);
                            MessageBox.Show("添加用户：" + userName.Substring(userName.IndexOf('\'') + 1, userName.IndexOf('\'', userName.IndexOf('\'') + 1) - 1) + "  成功");
                        }
                    }
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message + exception.StackTrace);
                }
            }

        }

        private void buttonQuit_Click(object sender, EventArgs e)
        {
            base.Close();
            base.Dispose();
        }

        private void comboBoxServer_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (this.comboBoxServer.Text.Trim())
            {
                case "Yahoo":
                    this.comboBoxServeradd.Items.Clear();
                    this.comboBoxServeradd.Items.Add("yahoo.com.cn");
                    this.comboBoxServeradd.Items.Add("yahoo.cn");
                    this.comboBoxServeradd.Items.Add("yahoo.com");
                    this.comboBoxServeradd.Items.Add("yahoo.de");
                    this.comboBoxServeradd.Items.Add("yahoo.fr");
                    this.comboBoxServeradd.Items.Add("yahoo.com.tw");
                    this.comboBoxServeradd.Items.Add("yahoo.com.hk");
                    this.comboBoxServeradd.Items.Add("yahoo.com.au");
                    this.comboBoxServeradd.Items.Add("yahoo.co.in");
                    this.comboBoxServeradd.Items.Add("yahoo.it");
                    this.comboBoxServeradd.Items.Add("yahoo.co.jp");
                    this.comboBoxServeradd.Items.Add("yahoo.co.uk");
                    this.comboBoxServeradd.Items.Add("yahoo.co.kr");
                    this.comboBoxServeradd.Items.Add("btinternet.com");
                    this.comboBoxServeradd.Items.Add("ymail.com");
                    this.comboBoxServeradd.Items.Add("yahoo.com.vn");
                    this.comboBoxServeradd.Items.Add("rocketmail.com");
                    this.comboBoxServeradd.DropDownStyle = ComboBoxStyle.DropDownList;
                    this.comboBoxServeradd.SelectedIndex = 0;
                    return;

                case "Hotmail":
                    this.comboBoxServeradd.Items.Clear();
                    this.comboBoxServeradd.Items.Add("hotmail.com");
                    this.comboBoxServeradd.Items.Add("hotmail.fr");
                    this.comboBoxServeradd.Items.Add("hotmail.de");
                    this.comboBoxServeradd.Items.Add("hotmail.it");
                    this.comboBoxServeradd.Items.Add("hotmail.co.jp");
                    this.comboBoxServeradd.Items.Add("hotmail.co.uk");
                    this.comboBoxServeradd.Items.Add("live.cn");
                    this.comboBoxServeradd.Items.Add("live.com");
                    this.comboBoxServeradd.Items.Add("live.ru");
                    this.comboBoxServeradd.Items.Add("live.hk");
                    this.comboBoxServeradd.Items.Add("live.nl");
                    this.comboBoxServeradd.Items.Add("live.co.kr");
                    this.comboBoxServeradd.Items.Add("msn.com");
                    this.comboBoxServeradd.Items.Add("msn.cn");
                    this.comboBoxServeradd.Items.Add("livemail.tw");
                    this.comboBoxServeradd.Items.Add("livemail.it");
                    this.comboBoxServeradd.Items.Add("windowslive.com");
                    this.comboBoxServeradd.DropDownStyle = ComboBoxStyle.DropDownList;
                    this.comboBoxServeradd.SelectedIndex = 0;
                    return;

                case "Gmail":
                    this.comboBoxServeradd.Items.Clear();
                    this.comboBoxServeradd.Items.Add("gmail.com");
                    this.comboBoxServeradd.DropDownStyle = ComboBoxStyle.DropDownList;
                    this.comboBoxServeradd.SelectedIndex = 0;
                    return;

                case "Ru":
                    this.comboBoxServeradd.Items.Clear();
                    this.comboBoxServeradd.Items.Add("mail.ru");
                    this.comboBoxServeradd.Items.Add("inbox.ru");
                    this.comboBoxServeradd.Items.Add("bk.ru");
                    this.comboBoxServeradd.Items.Add("list.ru");
                    this.comboBoxServeradd.DropDownStyle = ComboBoxStyle.DropDownList;
                    this.comboBoxServeradd.SelectedIndex = 0;
                    return;

                case "Yandex":
                    this.comboBoxServeradd.Items.Clear();
                    this.comboBoxServeradd.Items.Add("yandex.ru");
                    this.comboBoxServeradd.DropDownStyle = ComboBoxStyle.DropDownList;
                    this.comboBoxServeradd.SelectedIndex = 0;
                    return;

                case "Rambler":
                    this.comboBoxServeradd.Items.Clear();
                    this.comboBoxServeradd.Items.Add("rambler.ru");
                    this.comboBoxServeradd.DropDownStyle = ComboBoxStyle.DropDownList;
                    this.comboBoxServeradd.SelectedIndex = 0;
                    return;

                case "163":
                    this.comboBoxServeradd.Items.Clear();
                    this.comboBoxServeradd.Items.Add("163.com");
                    this.comboBoxServeradd.DropDownStyle = ComboBoxStyle.DropDownList;
                    this.comboBoxServeradd.SelectedIndex = 0;
                    return;

                case "Vip163":
                    this.comboBoxServeradd.Items.Clear();
                    this.comboBoxServeradd.Items.Add("vip.163.com");
                    this.comboBoxServeradd.DropDownStyle = ComboBoxStyle.DropDownList;
                    this.comboBoxServeradd.SelectedIndex = 0;
                    return;

                case "126":
                    this.comboBoxServeradd.Items.Clear();
                    this.comboBoxServeradd.Items.Add("126.com");
                    this.comboBoxServeradd.DropDownStyle = ComboBoxStyle.DropDownList;
                    this.comboBoxServeradd.SelectedIndex = 0;
                    return;

                case "263":
                    this.comboBoxServeradd.Items.Clear();
                    this.comboBoxServeradd.Items.Add("263.net");
                    this.comboBoxServeradd.DropDownStyle = ComboBoxStyle.DropDownList;
                    this.comboBoxServeradd.SelectedIndex = 0;
                    return;

                case "Yeah":
                    this.comboBoxServeradd.Items.Clear();
                    this.comboBoxServeradd.Items.Add("yeah.net");
                    this.comboBoxServeradd.DropDownStyle = ComboBoxStyle.DropDownList;
                    this.comboBoxServeradd.SelectedIndex = 0;
                    return;

                case "Hinet":
                    this.comboBoxServeradd.Items.Clear();
                    this.comboBoxServeradd.Items.Add("hinet.com");
                    this.comboBoxServeradd.DropDownStyle = ComboBoxStyle.DropDownList;
                    this.comboBoxServeradd.SelectedIndex = 0;
                    return;

                case "Aol":
                    this.comboBoxServeradd.Items.Clear();
                    this.comboBoxServeradd.Items.Add("aol.com");
                    this.comboBoxServeradd.DropDownStyle = ComboBoxStyle.DropDownList;
                    this.comboBoxServeradd.SelectedIndex = 0;
                    return;

                case "POP":
                    this.comboBoxServeradd.Items.Clear();
                    this.comboBoxServeradd.Items.Add("pop.163.com");
                    this.comboBoxServeradd.Items.Add("pop.sohu.com");
                    this.comboBoxServeradd.Items.Add("pop.sina.com");
                    this.comboBoxServeradd.Items.Add("pop.gmail.com");
                    this.comboBoxServeradd.Items.Add("pop3.live.com");
                    this.comboBoxServeradd.Items.Add("pop.qq.com");
                    this.comboBoxServeradd.DropDownStyle = ComboBoxStyle.DropDown;
                    return;

                case "Sohu":
                    this.comboBoxServeradd.Items.Clear();
                    this.comboBoxServeradd.Items.Add("sohu.com");
                    this.comboBoxServeradd.DropDownStyle = ComboBoxStyle.DropDownList;
                    this.comboBoxServeradd.SelectedIndex = 0;
                    return;

                case "CTM":
                    this.comboBoxServeradd.Items.Clear();
                    this.comboBoxServeradd.Items.Add("macau.ctm.net");
                    this.comboBoxServeradd.DropDownStyle = ComboBoxStyle.DropDownList;
                    this.comboBoxServeradd.SelectedIndex = 0;
                    return;

                case "FreePOPS":
                    this.comboBoxServeradd.Items.Clear();
                    this.comboBoxServeradd.Items.Add("127.0.0.1");
                    this.comboBoxServeradd.DropDownStyle = ComboBoxStyle.DropDown;
                    this.comboBoxServeradd.SelectedIndex = 0;
                    return;

                case "FastMail":
                    this.comboBoxServeradd.Items.Clear();
                    this.comboBoxServeradd.Items.Add("fastmail.fm");
                    this.comboBoxServeradd.DropDownStyle = ComboBoxStyle.DropDownList;
                    this.comboBoxServeradd.SelectedIndex = 0;
                    return;

                case "HushMail":
                    this.comboBoxServeradd.Items.Clear();
                    this.comboBoxServeradd.Items.Add("hushmail.com");
                    this.comboBoxServeradd.DropDownStyle = ComboBoxStyle.DropDownList;
                    this.comboBoxServeradd.SelectedIndex = 0;
                    return;

                case "Tom":
                    this.comboBoxServeradd.Items.Clear();
                    this.comboBoxServeradd.Items.Add("tom.com");
                    this.comboBoxServeradd.DropDownStyle = ComboBoxStyle.DropDownList;
                    this.comboBoxServeradd.SelectedIndex = 0;
                    return;

                case "21CN":
                    this.comboBoxServeradd.Items.Clear();
                    this.comboBoxServeradd.Items.Add("21cn.com");
                    this.comboBoxServeradd.Items.Add("21cn.net");
                    this.comboBoxServeradd.DropDownStyle = ComboBoxStyle.DropDownList;
                    this.comboBoxServeradd.SelectedIndex = 0;
                    return;

                case "Sina":
                    this.comboBoxServeradd.Items.Clear();
                    this.comboBoxServeradd.Items.Add("sina.com");
                    this.comboBoxServeradd.Items.Add("vip.sina.com");
                    this.comboBoxServeradd.DropDownStyle = ComboBoxStyle.DropDownList;
                    this.comboBoxServeradd.SelectedIndex = 0;
                    return;

                case "HanMail":
                    this.comboBoxServeradd.Items.Clear();
                    this.comboBoxServeradd.Items.Add("hanmail.net");
                    this.comboBoxServeradd.DropDownStyle = ComboBoxStyle.DropDownList;
                    this.comboBoxServeradd.SelectedIndex = 0;
                    return;

                case "QQ":
                    this.comboBoxServeradd.Items.Clear();
                    this.comboBoxServeradd.Items.Add("qq.com");
                    this.comboBoxServeradd.DropDownStyle = ComboBoxStyle.DropDownList;
                    this.comboBoxServeradd.SelectedIndex = 0;
                    return;

                case "IMAP":
                    this.comboBoxServeradd.Items.Clear();
                    this.comboBoxServeradd.Items.Add("imap.qq.com");
                    this.comboBoxServeradd.Items.Add("imap.gmail.com");
                    this.comboBoxServeradd.DropDownStyle = ComboBoxStyle.DropDown;
                    this.comboBoxServeradd.SelectedIndex = 0;
                    return;

                case "MailHN":
                    this.comboBoxServeradd.Items.Clear();
                    this.comboBoxServeradd.Items.Add("pvep.com.vn");
                    this.comboBoxServeradd.DropDownStyle = ComboBoxStyle.DropDownList;
                    this.comboBoxServeradd.SelectedIndex = 0;
                    return;

                case "RediffMail":
                    this.comboBoxServeradd.Items.Clear();
                    this.comboBoxServeradd.Items.Add("rediff.com");
                    this.comboBoxServeradd.Items.Add("rediffmail.com");
                    this.comboBoxServeradd.DropDownStyle = ComboBoxStyle.DropDownList;
                    this.comboBoxServeradd.SelectedIndex = 0;
                    return;
            }

        }

        public bool EditeUser(DataGridView dataGridView, DataSet dataset)
        {
            int y = dataGridView.CurrentCellAddress.Y;
            if (y >= 0)
            {
                this.textBoxSequencenumberText.Text = dataset.Tables["users"].Rows[y]["序号"].ToString();
                this.textBoxOwnerText.Text = dataset.Tables["users"].Rows[y]["属主"].ToString();
                this.textBoxUserName.Text = dataset.Tables["users"].Rows[y]["用户名"].ToString();
                this.textBoxPassword.Text = GlobalValue.PopMainForm.Decryption(dataset.Tables["users"].Rows[y]["口令字"].ToString(), dataset.Tables["users"].Rows[y]["用户名"].ToString());
                this.textBoxdirectionText.Text = dataset.Tables["users"].Rows[y]["方向"].ToString();
                this.comboBoxServer.Text = dataset.Tables["users"].Rows[y]["服务器"].ToString();
                this.comboBoxServeradd.Text = dataset.Tables["users"].Rows[y]["服务器地址"].ToString();
                dataset.Tables["users"].Rows[y]["控守"].ToString();
                if (dataset.Tables["users"].Rows[y]["控守"].ToString() == "True")
                {
                    this.checkBoxControlGuard.Checked = true;
                }
                else
                {
                    this.checkBoxControlGuard.Checked = false;
                }
                if (dataset.Tables["users"].Rows[y]["全部接收"].ToString() == "是")
                {
                    this.checkBoxReceiveAll.Checked = true;
                }
                else
                {
                    this.checkBoxReceiveAll.Checked = false;
                }
                if (dataset.Tables["users"].Rows[y]["接收时间"].ToString() == "True")
                {
                    this.checkBoxReceiveTime.Checked = true;
                   // this.comboBoxStartTime.Text = dataset.Tables["users"].Rows[y]["开始时间"].ToString();
                   // this.comboBoxTimeInterval.Text = dataset.Tables["users"].Rows[y]["时间间隔"].ToString();
                   // this.comboBoxEndTime.Text = dataset.Tables["users"].Rows[y]["时间段"].ToString();
                }
                else
                {
                    this.checkBoxReceiveTime.Checked = false;
                }
                return true;
            }
            MessageBox.Show("请选择用户!");
            return false;
        }

    }
}
