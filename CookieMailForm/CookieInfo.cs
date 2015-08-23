using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;

namespace MailExposure
{
    public partial class CookieInfo : Form
    {
        private string strType;
        private string cookieRemarks;
        private OpenFileDialog openFileDialogCookieTxt = new OpenFileDialog();

        public CookieInfo()
        {
            InitializeComponent();
            this.strType = "";
            this.cookieRemarks = "";
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

        private void buttonCookieTxt_Click(object sender, EventArgs e)
        {
            if (this.openFileDialogCookieTxt.ShowDialog() == DialogResult.OK)
            {
                this.textBoxCookiePath.Text = this.openFileDialogCookieTxt.FileName;
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            try
            {
                bool auto = false;
                if (this.checkBoxAuto.Checked)
                {
                    auto = true;
                }
                if (this.textBoxCookiePath.Text.Length > 0)
                {
                    if (this.comboBoxUserlist.Text.Trim() != "")
                    {
                        GlobalValue.mainForm.UserList = this.comboBoxUserlist.Text.Trim();
                        this.strType = this.comboBoxCookieType.Text.Trim();
                        this.cookieRemarks = this.textBoxCookieRemarks.Text.Trim();
                        switch (this.strType)
                        {
                            case "Yahoo-Txt":
                                GlobalValue.mainForm.yahoo_txt(this.textBoxCookiePath.Text, auto, this.cookieRemarks);
                                base.Close();
                                return;

                            case "GlobalValue(Japan)-txt":
                                GlobalValue.mainForm.yahooJapan_txt(this.textBoxCookiePath.Text, auto, this.cookieRemarks);
                                base.Close();
                                return;

                            case "Hotmail-Txt":
                                GlobalValue.mainForm.hotmail_txt(this.textBoxCookiePath.Text, auto, this.cookieRemarks);
                                base.Close();
                                return;

                            case "Gmail-Txt":
                                GlobalValue.mainForm.gmail_txt(this.textBoxCookiePath.Text, auto, this.cookieRemarks);
                                base.Close();
                                return;

                            case "Hinet-Txt":
                                GlobalValue.mainForm.hinet_txt(this.textBoxCookiePath.Text, auto, this.cookieRemarks);
                                base.Close();
                                return;

                            case "Ru-Txt":
                                GlobalValue.mainForm.ru_txt(this.textBoxCookiePath.Text, auto, this.cookieRemarks);
                                base.Close();
                                return;

                            case "FastMail-Txt":
                                GlobalValue.mainForm.fastmail_txt(this.textBoxCookiePath.Text, auto, this.cookieRemarks);
                                base.Close();
                                return;

                            case "126-Txt":
                                GlobalValue.mainForm.mail126_txt(this.textBoxCookiePath.Text, auto, this.cookieRemarks);
                                base.Close();
                                return;

                            case "163-Txt":
                                GlobalValue.mainForm.mail163_txt(this.textBoxCookiePath.Text, auto, this.cookieRemarks);
                                base.Close();
                                return;

                            case "263-Txt":
                                GlobalValue.mainForm.mail263_txt(this.textBoxCookiePath.Text, auto, this.cookieRemarks);
                                base.Close();
                                return;

                            case "TomMail-Txt":
                                GlobalValue.mainForm.tommail_txt(this.textBoxCookiePath.Text, auto, this.cookieRemarks);
                                base.Close();
                                return;

                            case "SinaMail-Txt":
                                GlobalValue.mainForm.sinamail_txt(this.textBoxCookiePath.Text, auto, this.cookieRemarks);
                                base.Close();
                                return;

                            case "21CNMail-Txt":
                                GlobalValue.mainForm.CNmail_txt(this.textBoxCookiePath.Text, auto, this.cookieRemarks);
                                base.Close();
                                return;

                            case "Aol-Txt":
                                GlobalValue.mainForm.Aolmail_txt(this.textBoxCookiePath.Text, auto, this.cookieRemarks);
                                base.Close();
                                return;
                        }
                    }
                    else
                    {
                        MessageBox.Show("请选择用户列表");
                    }
                }
                else
                {
                    MessageBox.Show("请选择txt文件");
                }
            }
            catch (Exception exception)
            {
                GlobalValue.mainForm.listBoxView.Items.Add(exception.Message + exception.StackTrace);
                GlobalValue.mainForm.listBoxView.SelectedIndex = GlobalValue.mainForm.listBoxView.Items.Count - 1;
            }

        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void CookieInfo_Load(object sender, EventArgs e)
        {
            this.comboBoxCookieType.SelectedIndex = 0;
            this.AddUsersListNode();
        }

    }
}
