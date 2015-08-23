using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Windows.Forms;

namespace MailExposure
{
    public partial class frmAddUsers : Form
    {
        public frmAddUsers()
        {
            InitializeComponent();
       /*     if (!this.checkBoxReceiveTime.Checked)
            {
                this.comboBoxEndTime.Visible = false;
                this.comboBoxStartTime.Visible = false;
                this.comboBoxTimeInterval.Visible = false;
                this.labelSpaceTime.Visible = false;
                this.labelStartTime.Visible = false;
                this.labelTimeInterval.Visible = false;
            }*/

        }
        public bool EditeUser(DataGridView dataGridView, DataSet dataset)
        {
            this.AddUsersListNode();
            int y = dataGridView.CurrentCellAddress.Y;
            if (y >= 0)
            {
                this.comboBoxUserlist.Text = dataset.Tables["users"].Rows[y]["用户列表"].ToString();
                this.textBoxSequencenumberText.Text = dataset.Tables["users"].Rows[y]["序号"].ToString();
                this.textBoxUserName.Text = dataset.Tables["users"].Rows[y]["用户名"].ToString();
                this.textBoxOwnerText.Text = dataset.Tables["users"].Rows[y]["属主"].ToString();
          //    this.textBoxPassword.Text = MainForm.mainForm.Decryption(dataset.Tables["users"].Rows[y]["口令字"].ToString(), dataset.Tables["users"].Rows[y]["用户名"].ToString());
          //    this.textBoxdirectionText.Text = dataset.Tables["users"].Rows[y]["方向"].ToString();
          //    this.comboBoxServer.Text = dataset.Tables["users"].Rows[y]["服务器"].ToString();
          //    this.comboBoxServeradd.Text = dataset.Tables["users"].Rows[y]["服务器地址"].ToString();
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
           //         this.checkBoxReceiveAll.Checked = true;
                }
                else
                {
           //         this.checkBoxReceiveAll.Checked = false;
                }
                if (dataset.Tables["users"].Rows[y]["接收时间"].ToString() == "True")
                {
             ////       this.checkBoxReceiveTime.Checked = true;
             //       this.comboBoxStartTime.Text = dataset.Tables["users"].Rows[y]["开始时间"].ToString();
             //       this.comboBoxTimeInterval.Text = dataset.Tables["users"].Rows[y]["时间间隔"].ToString();
             //       this.comboBoxEndTime.Text = dataset.Tables["users"].Rows[y]["时间段"].ToString();
                }
                else
                {
              //      this.checkBoxReceiveTime.Checked = false;
                }
                return true;
            }
            MessageBox.Show("请选择用户！");
            return false;
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
             string str = this.textBoxSequencenumberText.Text.Trim();
          //  string str2 = this.textBoxdirectionText.Text.Trim();
         //   str2 = "'" + str2 + "'";
            string str3 = this.textBoxOwnerText.Text.Trim();
            str3 = "'" + str3 + "'";
            string userName = this.textBoxUserName.Text.Trim();
            string str5 = this.comboBoxUserlist.Text.Trim();
            str5 = "'" + str5 + "'";
          //  string passWd = this.textBoxPassword.Text.Trim();
          //  passWd = "'" + MainForm.mainForm.Encryption(passWd, userName) + "'";
            userName = "'" + userName + "'";
           // string str7 = this.comboBoxServeradd.Text.Trim();
           // str7 = "'" + str7 + "'";
           // string str8 = this.comboBoxServer.Text.Trim();
           // str8 = "'" + str8 + "'";
            string str9 = this.checkBoxControlGuard.Checked ? "1" : "0";
            str9 = "'" + str9 + "'";
         //   string str10 = this.checkBoxReceiveAll.Checked ? "是" : "否";
         //   str10 = "'" + str10 + "'";
          //  string str11 = "?";
         //   str11 = "'" + str11 + "'";
          //  string str12 = "0";
        //    str12 = "'" + str12 + "'";
        //    if (str2 == "''")
        //    {
        //        MessageBox.Show("方向不能为空！");
        //    }
            if (str3 == "''")
            {
                MessageBox.Show("Cookie标示不能为空！");
            }
            else if (userName == "''")
            {
                MessageBox.Show("用户名不能为空！");
            }
            else if (str5 == "''")
            {
                MessageBox.Show("用户列表不能为空！");
            }
            else
            {
                string text = "";
                string str14 = "";
                string str15 = "";
             //   bool flag1 = this.checkBoxReceiveTime.Checked;
             //   if (this.checkBoxReceiveTime.Checked)
             //   {
             //       text = this.comboBoxStartTime.Text;
            //        str15 = this.comboBoxTimeInterval.Text;
             //       str14 = this.comboBoxEndTime.Text;
             /*       if (text == "")
                    {
                        MessageBox.Show("开始时间不能为空！");
                        return;
                    }
                    if (str15 == "")
                    {
                        MessageBox.Show("间隔时间不能为空！");
                        return;
                    }
                    if (str14 == "")
                    {
                        MessageBox.Show("结束时间不能为空！");
                        return;
                    }
                    if (Convert.ToInt32(text) >= Convert.ToInt32(str14))
                    {
                        MessageBox.Show("开始时间大于结束时间！");
                        return;
                    }
                    if (Convert.ToInt32(str15) > (Convert.ToInt32(str14) - Convert.ToInt32(text)))
                    {
                        MessageBox.Show("时间间隔大于时间差！");
                        return;
                    }
                    if (((text == "") || (str14 == "")) || (str15 == ""))
                    {
                        MessageBox.Show("接收时间段不能为空！");
                        return;
                    }
                    text = "'" + text + "'";
                    str14 = "'" + str14 + "'";
                    str15 = "'" + str15 + "'";
                }
                else
                {
                    text = "'" + text + "'";
                    str14 = "'" + str14 + "'";
                    str15 = "'" + str15 + "'";
                }*/
                try
                {
                    if (this.textBoxSequencenumberText.Text != "")
                    {
                        string str16 = "属主=" + str3 + ",用户名=" + userName + ",控守=" + str9 + ",用户列表=" + str5;
                        string strSql = "update users set " + str16 + " where 序号=" + str;
                        GlobalValue.mainForm.ExecuteSQL(strSql);
                        foreach (TreeNode node in GlobalValue.mainForm.treeViewUsers.Nodes[0].Nodes)
                        {
                            if (node.Text == str5.Trim(new char[] { '\'' }))
                            {
                                GlobalValue.mainForm.treeViewUsers.SelectedNode = node;
                            }
                        }
                    }
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message + exception.StackTrace);
                }
            }

        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            base.Close();
            base.Dispose();
        }

    }
}
