using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MailExposure
{
    public partial class UsersList : Form
    {
        public UsersList()
        {
            InitializeComponent();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            try
            {
                string strSql = "";
                if (this.textBoxUsersList.Text.Trim().Length > 0)
                {
                    if (this.textBoxUsersList.Text.Trim().IndexOfAny(new char[] { '@', '.', ',', '!' }) == -1)
                    {
                        strSql = "select count(*) from UsersList where UsersListName='" + this.textBoxUsersList.Text.Trim() + "'";
                        if (Convert.ToInt32(GlobalValue.mainForm.ExecuteSQL(strSql)) == 0)
                        {
                            strSql = "insert into UsersList(UsersListName)values('" + this.textBoxUsersList.Text.Trim() + "')";
                            if (GlobalValue.mainForm.ExecuteSQL(strSql).IndexOf("false") != -1)
                            {
                                MessageBox.Show("添加失败！请检查是否有特殊字符", "警告");
                            }
                            else
                            {
                                TreeNode node = new TreeNode();
                                node.Text = this.textBoxUsersList.Text.Trim();
                                node.ImageIndex = 0;
                                TreeNode node2 = new TreeNode();
                                node2.Text = this.textBoxUsersList.Text.Trim();
                                node2.ImageIndex = 0;
                                GlobalValue.mainForm.treeViewUsers.Nodes[0].Nodes.Add(node);
                                MessageBox.Show("添加成功!");
                                base.Close();
                            }
                        }
                        else
                        {
                            MessageBox.Show("用户列表已存在！", "警告");
                        }
                    }
                    else
                    {
                        MessageBox.Show("无效的字符: '@','.', ',', '!'", "警告!");
                    }
                }
                else
                {
                    MessageBox.Show("用户列表不能为空！", "警告");
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message + exception.StackTrace);
            }

        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            base.Close();
        }
    }
}
