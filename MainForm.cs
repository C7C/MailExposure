using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net.Security;
using System.Diagnostics;
using System.Text.RegularExpressions;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using DevExpress.Skins;
using DevExpress.Utils.Drawing;
using MailExposure.RecordInfo;
using MailExposure.MailXssCode;
using MailExposure.CookieMailForm;
using MailExposure.POPMailForm;
using MailExposure.PopMailRC;

namespace MailExposure {
	public partial class MainForm : RibbonForm 
    {
		RibbonDemos.SkinGalleryHelper skinGalleryHelper;
        MailEditorControl editor = null;
        CookieMailControl cookieMailControl = null;
        PopMailControl popMailControl = null;

        Model modelRecord = null;
        bool IsAutoAddCommuRecordChecked;
        bool IsAutoAddSendMailLogChecked;
        private TcpClient conn;
        public SslStream sslStream;
        private StreamReader strReader;
        private NetworkStream networkStream;
        private FileStream fileStream;
        private AddressList address;
        private UserNameList userName;
        private int num;                  //�ʼ����ͼ���
        private byte[] strData;           //������ת���洢
        private LogInfo logInfo;
        private List<string> mailInfo;
        private struct SelectTypeRecord
        {
            public bool IsPassword;
            public bool IsCookie ;
            public bool IsMail;
            public bool IsCP ;
            public bool IsNull;
            public void SetAllFlase()
            {
                IsPassword = false;
                IsCookie = false;
                IsMail = false;
                IsCP = false;
                IsNull = false;
            }
        }
        private SelectTypeRecord selectType;

		public MainForm() 
        {
			InitializeComponent();
            modelRecord = new Model();
            this.logInfo = new LogInfo();
            this.address = new AddressList();
            this.userName = new UserNameList();
            this.mailInfo = new List<string>();
            Control.CheckForIllegalCrossThreadCalls = false;

			skinGalleryHelper = new RibbonDemos.SkinGalleryHelper(ribbonGallerySkins);
            
            ((DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit)ReceiveNameEdit.Edit).ButtonClick+=new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(ImportNameButton_Click);
            ((DevExpress.XtraEditors.Repository.RepositoryItemRadioGroup)MailCollectionType.Edit).SelectedIndexChanged += new EventHandler(CollectionRadioGroup_SelectIndexChanged);
            ((DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit)AddCommuRecord.Edit).CheckedChanged += new EventHandler(AutoAddCommuRecord_CheckedChanged);
            ((DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit)AddSendMailLog.Edit).CheckedChanged += new EventHandler(AutoAddSendMailLog_CheckedChanged);
            ((DevExpress.XtraEditors.Repository.RepositoryItemComboBox)FliterUsersComboBox.Edit).SelectedIndexChanged+=new EventHandler(FliterUsers_SelectedIndexChanged);
   
            this.CookieMailPanel.Visible = true;
          //this.PopMailPanel.Visible = false;
         // this.MailEditorPanel.Visible = true;
		}

        void FliterUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            String str = ((DevExpress.XtraEditors.ComboBoxEdit)sender).EditValue.ToString();
            if(str=="YahooMail�û�")
            {
                GlobalValue.PopMainForm.ButtonStopReceiveEmail_Click(sender, e);
                string sqlShowUser = "SELECT * FROM users WHERE ������='Yahoo'";
                string sqlCountUsers = "select count(*) from users where ������='Yahoo'";
                GlobalValue.PopMainForm.UsersShow(sqlShowUser);
                GlobalValue.PopMainForm.UsersCount(sqlCountUsers);
            }
            else
                if(str=="HotMail�û�")
                {
                    GlobalValue.PopMainForm.ButtonStopReceiveEmail_Click(sender, e);
                    string sqlShowUser = "SELECT * FROM users WHERE ������='Hotmail'";
                    string sqlCountUsers = "select count(*) from users where ������='Hotmail'";
                    GlobalValue.PopMainForm.UsersShow(sqlShowUser);
                    GlobalValue.PopMainForm.UsersCount(sqlCountUsers);

                }
                else 
                    if(str=="GMail�û�")
                    {
                        GlobalValue.PopMainForm.ButtonStopReceiveEmail_Click(sender, e);
                        string sqlShowUser = "SELECT * FROM users WHERE ������='Gmail'";
                        string sqlCountUsers = "select count(*) from users where ������='Gmail'";
                        GlobalValue.PopMainForm.UsersShow(sqlShowUser);
                        GlobalValue.PopMainForm.UsersCount(sqlCountUsers);
                    }
                    else 
                        if(str=="PopMail�û�")
                        {
                            GlobalValue.PopMainForm.ButtonStopReceiveEmail_Click(sender, e);
                            string sqlShowUser = "SELECT * FROM users WHERE ������='POP'";
                            string sqlCountUsers = "select count(*) from users where ������='POP'";
                            GlobalValue.PopMainForm.UsersShow(sqlShowUser);
                            GlobalValue.PopMainForm.UsersCount(sqlCountUsers);

                        }
                        else if(str=="RuMail�û�")
                        {
                            GlobalValue.PopMainForm.ButtonStopReceiveEmail_Click(sender, e);
                            string sqlShowUser = "SELECT * FROM users WHERE ������='Ru'";
                            string sqlCountUsers = "select count(*) from users where ������='Ru'";
                            GlobalValue.PopMainForm.UsersShow(sqlShowUser);
                            GlobalValue.PopMainForm.UsersCount(sqlCountUsers);
                        }
                        else
                        {
                            GlobalValue.PopMainForm.ButtonStopReceiveEmail_Click(sender, e);
                            string sqlShowUser = "SELECT * FROM users WHERE �û�����='��ͨ�û�'";
                            string sqlCountUsers = "select count(*) from users where �û�����='��ͨ�û�'";
                            GlobalValue.PopMainForm.UsersShow(sqlShowUser);
                            GlobalValue.PopMainForm.UsersCount(sqlCountUsers);
                        }
        }

        void AutoAddCommuRecord_CheckedChanged(object sender, EventArgs e)
        {
            IsAutoAddCommuRecordChecked =  ((DevExpress.XtraEditors.CheckEdit)sender).Checked;
        }

        void AutoAddSendMailLog_CheckedChanged(object sender, EventArgs e)
        {
            IsAutoAddSendMailLogChecked = ((DevExpress.XtraEditors.CheckEdit)sender).Checked;
        }

        void ImportNameButton_Click(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            this.importFileDlg.FileName = "";
            this.importFileDlg.Filter = "txt�ļ�|*.txt";
            this.importFileDlg.ShowDialog();
            this.importFileDlg.CheckFileExists = true;
            this.importFileDlg.AddExtension = true;
            if ((this.importFileDlg.FileName != null) && (this.importFileDlg.FileName != ""))
            {
                string str = new StreamReader(this.importFileDlg.FileName, Encoding.GetEncoding("GBK")).ReadToEnd();
                this.ReceiveNameEdit.EditValue = str;
            }
        }

        void CollectionRadioGroup_SelectIndexChanged(object sender, EventArgs e)                                   //���û��ı�ȡ֤��ʽʱ
        {
            int selectIndex = ((DevExpress.XtraEditors.RadioGroup)sender).SelectedIndex;
            selectType.SetAllFlase();
            switch (selectIndex)
            { 
                case 0:
                    selectType.IsPassword = true;
                    break;
                case 1:
                    selectType.IsCookie = true;
                    break;
                case 2:
                    selectType.IsMail = true;
                    break;
                case 3:
                    selectType.IsCP = true;
                    break;
                case 4:
                    selectType.IsNull = true;
                    break;
            }
           // foreach (DevExpress.XtraEditors.Controls.RadioGroupItem rg in ((DevExpress.XtraEditors.Repository.RepositoryItemRadioGroup)MailCollectionType.Edit).Items)
           //{
           //   rg.Value = false;
           // }
           //  ((DevExpress.XtraEditors.Repository.RepositoryItemRadioGroup)MailCollectionType.Edit).Items[selectIndex].Value = !((bool)((DevExpress.XtraEditors.RadioGroup)sender).Properties.Items[selectIndex].Value);   //��ԭ����ֵȡ��
        }

        private void LoadMainEditor()
        {
            if (editor == null)
            {
                this.editor = new MailEditorControl();
                this.editor.BodyHtml = "& nbsp; ";
                this.editor.BodyText = "  ";
                editor.BackColor = System.Drawing.Color.Gainsboro;
                editor.Dock = System.Windows.Forms.DockStyle.Fill;
                editor.Location = new System.Drawing.Point(0, 0);
                editor.Name = "MailEditor";
                this.CookieMailPanel.Controls.Add((Control)this.editor);
                this.CookieMailPanel.Dock = System.Windows.Forms.DockStyle.Fill;

            }        
       //    this.MailEditorPanel.Visible = true;
       //    this.CookieMailPanel.Visible = false;
          
        // this.PopMailPanel.Visible = false;
        }

        private void LoadMailCookietControl()
        {
          //  this.MailEditorPanel.Controls.Clear();
            if (cookieMailControl == null)
            {
                cookieMailControl = new CookieMailControl();
                this.cookieMailControl.BackColor = System.Drawing.Color.Gainsboro;
                cookieMailControl.Dock = System.Windows.Forms.DockStyle.Fill;
                cookieMailControl.Location = new System.Drawing.Point(0, 0);
                cookieMailControl.Name = "CookieMail";
                this.CookieMailPanel.Controls.Add((Control)cookieMailControl);
                this.CookieMailPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            }         
          //  this.MailEditorPanel.Visible = false;
          //  this.CookieMailPanel.Visible = true;
            
        }


        private void LoadPopMailControl()
        {
            if (popMailControl == null)
            {
               // CookieMailPanel.Controls.Clear();
                popMailControl = new PopMailControl();
                this.popMailControl.BackColor = System.Drawing.Color.Gainsboro;
                popMailControl.Dock = System.Windows.Forms.DockStyle.Fill;
                popMailControl.Location = new System.Drawing.Point(0, 0);
                popMailControl.Name = "PopMail";
                this.CookieMailPanel.Controls.Add((Control)popMailControl);
                this.CookieMailPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            }
         //   this.MailEditorPanel.Visible = false;
         //   this.CookieMailPanel.Visible = true;
            
          //this.PopMailPanel.Visible = true;         //����Panel������⣿��   
        }
        private void DisplayPopMailControl()
        {
            if (this.CookieMailPanel.Controls.Count > 1)
            {
                this.CookieMailPanel.Controls["MailEditor"].Visible = false;
                if (this.CookieMailPanel.Controls["CookieMail"] != null) this.CookieMailPanel.Controls["CookieMail"].Visible = false;
                this.CookieMailPanel.Controls["PopMail"].Visible = true;
            }
        }
        private void DisplayMailCookieControl()
        {
            if (this.CookieMailPanel.Controls.Count > 1)
            {
                this.CookieMailPanel.Controls["CookieMail"].Visible = true;
                if (this.CookieMailPanel.Controls["PopMail"] != null) this.CookieMailPanel.Controls["PopMail"].Visible = false;
                this.CookieMailPanel.Controls["MailEditor"].Visible = false;
            }
        }
        private void DisplayMainEditor()
        {
            if (this.CookieMailPanel.Controls.Count > 1)
            {
                this.CookieMailPanel.Controls["CookieMail"].Visible = false;
                if (this.CookieMailPanel.Controls["PopMail"] != null) this.CookieMailPanel.Controls["PopMail"].Visible = false;
                if (this.CookieMailPanel.Controls["MailEditor"] != null) this.CookieMailPanel.Controls["MailEditor"].Visible = true;
            }
        }
		private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) 
        {
			Close();
		}

        private void MainForm_Load(object sender, EventArgs e)
        {
            LoadMainEditor();
            LoadMailCookietControl();
            LoadPopMailControl();
            
            Record record = new Record();
            record.ini("get");
         //   this.notifyIcon1.Visible = true;
            this.trvListInfo.ExpandAll();
            this.BindTreeView();
            string[] strArray = record.GetRecord();
            this.NickNameEdit.EditValue = strArray[0];
            this.SendMailNameEdit.EditValue = strArray[1];
            this.ReceiveNameEdit.EditValue = strArray[2];
            this.UserNameEdit.EditValue = strArray[3];
            this.SubjectEdit.EditValue = strArray[4];
            this.ServerNameEdit.EditValue = strArray[5];
            this.FliterUsersComboBox.EditValue = "��ʾȫ���û�";
        }

        private void ImportNameButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            MessageBox.Show("����ֱ�����룬Ҳ�ɴ�TXT�ļ��е���","��ʾ");
        }

        private void SendMail_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            this.Isnull();
            if (selectType.IsNull == true)
            {
                this.modelRecord.Type = "";
            }
            else if (selectType.IsCookie == true)
            {
                this.modelRecord.Type = "Cookie";
            }
            else if (selectType.IsPassword == true)
            {
                this.modelRecord.Type = "Password";
            }
            else if (selectType.IsMail == true)
            {
                this.modelRecord.Type = "Mail";
            }
            else if (selectType.IsCP == true)
            {
                this.modelRecord.Type = "C+P";
            }
            else
            {
                this.MailInfoOutput.Text = this.MailInfoOutput.Text + "δѡ��ȡ֤����!\r\n";
                return;
            }
            string[] strArray = ((String)ReceiveNameEdit.EditValue).Trim().Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < strArray.Length; i++)
            {
                if (!this.ValidationMail(strArray[i]))
                {
                    this.MailInfoOutput.Text = this.MailInfoOutput.Text + "�ռ�����Ϣ����!\r\n";
                    this.MailInfoOutput.Refresh();
                    return;
                }
                this.modelRecord.Addressee = strArray[i];
                DateTime now = new DateTime();
                now = DateTime.Now;
                string log = this.logInfo.GetLog(this.modelRecord.Addressee, string.Concat(new object[] { now.Year, "��", now.Month, "��", now.Day, "��" }));
                if (log == "")
                {
                    this.SendEmail();
                }
                else if (MessageBox.Show(log, "������ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    this.SendEmail();
                }
                else
                {
                    return;
                }
            }
        }

        public void SendEmail()
        {
            string stream = "";
            string str2 = "\r\n";
            try
            {
                if (this.ServerNameEdit.EditValue.ToString() == "smtp.gmail.com")
                {
                    this.conn = new TcpClient(this.ServerNameEdit.EditValue.ToString(), 0x1d1);
                    this.sslStream = new SslStream(this.conn.GetStream(), false);
                    this.sslStream.AuthenticateAsClient(this.ServerNameEdit.EditValue.ToString());
                    this.strReader = new StreamReader(this.sslStream, Encoding.GetEncoding("gbk"));
                }
                else
                {
                    this.conn = new TcpClient(this.ServerNameEdit.EditValue.ToString(), 0x19);
                    this.networkStream = this.conn.GetStream();
                    this.strReader = new StreamReader(this.networkStream, Encoding.GetEncoding("gbk"));
                }
                if (!this.strReader.ReadLine().StartsWith("220"))
                {
                    this.MailInfoOutput.Text = this.MailInfoOutput.Text + "����������ʧ��!\r\n";
                }
                else
                {
                    stream = "HELO server" + str2;
                    this.SendRequest(stream);
                    string str3 = this.strReader.ReadLine();
                    stream = "AUTH LOGIN" + str2;
                    this.SendRequest(stream);
                    str3 = this.strReader.ReadLine();
                    stream = this.EncodeBase64(this.modelRecord.UserName) + str2;
                    this.SendRequest(stream);
                    str3 = this.strReader.ReadLine();
                    stream = this.EncodeBase64(this.modelRecord.Password) + str2;
                    this.SendRequest(stream);
                    if (!this.strReader.ReadLine().StartsWith("235"))
                    {
                        this.MailInfoOutput.Text = this.MailInfoOutput.Text + "�û���/�������!\r\n";
                    }
                    else
                    {
                        stream = string.Concat(new object[] { "MAIL FROM: ", '<', this.modelRecord.UserName, '>', str2 });
                        this.SendRequest(stream);
                        if (this.strReader.ReadLine().StartsWith("500"))
                        {
                            this.MailInfoOutput.Text = this.MailInfoOutput.Text + "��������ȷ�ķ�����ַ!\r\n";
                        }
                        else
                        {
                            stream = string.Concat(new object[] { "RCPT TO: ", '<', this.modelRecord.Addressee, '>', str2 });
                            this.SendRequest(stream);
                            if (this.strReader.ReadLine().StartsWith("503"))
                            {
                                this.MailInfoOutput.Text = this.MailInfoOutput.Text + "��������ȷ���ռ���ַ\r\n";
                            }
                            else
                            {
                                stream = "Data" + str2;
                                this.SendRequest(stream);
                                if (this.strReader.ReadLine().StartsWith("354"))
                                {
                                    stream = this.MailType(this.modelRecord);
                                }
                                this.SendRequest(stream);
                                str3 = this.strReader.ReadLine();
                                this.num++;
                                if (str3.StartsWith("250"))
                                {
                                    object text = this.MailInfoOutput.Text;
                                    this.MailInfoOutput.Text = string.Concat(new object[] { text, "(��", this.num, "��:", this.modelRecord.Addresser, "��", this.modelRecord.Addressee, "[", this.modelRecord.Subject, "]�ʼ�,���ͳɹ�)\r\n" });
                                    this.MailInfoOutput.Refresh();
                                    if (IsAutoAddSendMailLogChecked==true)
                                    {
                                        this.MailInfoOutput.Text = this.MailInfoOutput.Text + this.logInfo.SetLlog(this.modelRecord);
                                        this.MailInfoOutput.Refresh();
                                        this.BindTreeView();
                                    }
                                    if (IsAutoAddCommuRecordChecked==true)
                                    {
                                        this.MailInfoOutput.Text = this.MailInfoOutput.Text + this.address.SetAddressList(this.modelRecord);
                                        this.MailInfoOutput.Text = this.MailInfoOutput.Text + this.userName.SetUserNameList(this.modelRecord);
                                        this.MailInfoOutput.Refresh();
                                        this.BindTreeView();
                                    }
                                }
                                else
                                {
                                    object obj3 = this.MailInfoOutput.Text;
                                    this.MailInfoOutput.Text = string.Concat(new object[] { obj3, "(��", this.num, "��:", this.modelRecord.Addresser, "��", this.modelRecord.Addressee, "[", this.modelRecord.Subject, "]�ʼ�,���ڷ�������ͣ���ͣ��������������ʼ��򼸷��Ӻ��ִη���)\r\n" });
                                    this.MailInfoOutput.Refresh();
                                }
                                this.strReader.Close();
                                this.conn.Close();
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                this.MailInfoOutput.Text = this.MailInfoOutput.Text + "�ʼ������쳣!\r\n";
                this.MailInfoOutput.Refresh();
            }
        }

        public string EncodeBase64(string code)
        {
            string str = "";
            byte[] bytes = Encoding.Default.GetBytes(code);
            try
            {
                str = Convert.ToBase64String(bytes);
            }
            catch
            {
                str = code;
                this.MailInfoOutput.Text = this.MailInfoOutput.Text + "Base64�������!\r\n";
            }
            return str;
        }

        private void SendRequest(string stream)
        {
            try
            {
                if (this.ServerNameEdit.EditValue.ToString() == "smtp.gmail.com")
                {
                    this.strData = Encoding.Default.GetBytes(stream.ToCharArray());
                    this.sslStream.Write(this.strData, 0, this.strData.Length);
                }
                else
                {
                    this.strData = Encoding.Default.GetBytes(stream.ToCharArray());
                    this.conn.GetStream().Write(this.strData, 0, this.strData.Length);
                }
            }
            catch (Exception)
            {
                this.MailInfoOutput.Text = this.MailInfoOutput.Text + "�ʼ������ݷ��ͳ���!";
                this.networkStream.Close();
                this.strReader.Close();
                this.conn.Close();
            }
        }

        private string MailType(Model model)
        {
            string str2 = model.Addressee.ToLower();
            if (selectType.IsNull == true)
            {
                ss_mail _mail = new ss_mail();
                return _mail.MailPackage(model);
            }
            if (str2.IndexOf("@163") > 0)
            {
                _163 _ = new _163();
                return _.MailPackage(model);
            }
            if (str2.IndexOf("@126") > 0)
            {
                _126 _2 = new _126();
                return _2.MailPackage(model);
            }
            if (str2.IndexOf(".ru") > 0)
            {
                Ru ru = new Ru();
                return ru.MailPackage(model);
            }
            if (str2.IndexOf("@aol") > 0)
            {
                Aol aol = new Aol();
                return aol.MailPackage(model);
            }
            if (str2.IndexOf("@hanmail") > 0)
            {
                HanMail hanmail = new HanMail();
                return hanmail.MailPackage(model);
            }
            if (((str2.IndexOf("@hotmail") > 0) || (str2.IndexOf("@msn") > 0)) || (str2.IndexOf("@live") > 0))
            {
                Hotmail hotmail = new Hotmail();
                return hotmail.MailPackage(model);
            }
            if (str2.IndexOf("@yahoo") > 0)
            {
                Yahoo yahoo = new Yahoo();
                return yahoo.MailPackage(model);
            }
            if (str2.IndexOf("@msa.hinet") > 0)
            {
                Hinet hinet = new Hinet();
                return hinet.MailPackage(model);
            }
            if (str2.IndexOf("@gmail") > 0)
            {
                Gmail gmail = new Gmail();
                return gmail.MailPackage(model);
            }
            this.MailInfoOutput.Text = this.MailInfoOutput.Text + "�����ڴ��������͵�ȡ֤!\r\n";
            return "";
        }

        public void BindTreeView()
        {
            try
            {
                this.trvLogList.ExpandAll();
                this.trvListInfo.ExpandAll();
                this.trvLogList.Nodes.Clear();
                this.trvListInfo.Nodes.Clear();
                TreeNode node = new TreeNode();
                TreeNode node2 = new TreeNode();
                TreeNode node3 = new TreeNode();
                node.Text = "������־";
                node2.Text = "�����ռ���";
                node3.Text = "�����û���";
                node.Tag = "0";
                node2.Tag = "0";
                node3.Tag = "0";
                this.trvLogList.Nodes.Add(node);
                this.trvListInfo.Nodes.Add(node2);
                this.trvListInfo.Nodes.Add(node3);
                AddressList list = new AddressList();
                UserNameList list2 = new UserNameList();
                List<string> log = this.logInfo.GetLog();
                List<string> addressList = list.GetAddressList();
                List<string> userNameList = list2.GetUserNameList();
                for (int i = 0; i < log.Count; i++)
                {
                    TreeNode node4 = new TreeNode();
                    node4.Text = log[i];
                    this.trvLogList.Nodes[0].Nodes.Add(node4);
                }
                for (int j = 0; j < addressList.Count; j++)
                {
                    TreeNode node5 = new TreeNode();
                    node5.Text = addressList[j];
                    this.trvListInfo.Nodes[0].Nodes.Add(node5);
                }
                for (int k = 0; k < userNameList.Count; k++)
                {
                    TreeNode node6 = new TreeNode();
                    node6.Text = userNameList[k];
                    this.trvListInfo.Nodes[1].Nodes.Add(node6);
                }
            }
            catch (Exception)
            {
                this.MailInfoOutput.Text = this.MailInfoOutput.Text + "ͨѶ¼/��־����ʧ��!";
                throw;
            }
        }
        private bool ValidationMail(string Mail)
        {
            Regex regex = new Regex(@"^\w+((-\w+)|(\.\w+))*\@\w+((\.|-)\w+)*\.\w+$");
            if (!regex.Match(Mail).Success)
            {
                return false;
            }
            return true;
        }

        private void Isnull()
        {
            if (((String)NickNameEdit.EditValue).Trim() == "")
            {
                this.MailInfoOutput.Text = this.MailInfoOutput.Text + "����д�ǳ�\r\n";
                this.NickNameEdit.Manager.Form.Focus();
            }
            else if (this.SendMailNameEdit.EditValue.ToString().Trim() == "")
            {
                this.MailInfoOutput.Text = this.MailInfoOutput.Text + "����д������!\r\n";
                this.SendMailNameEdit.Manager.Form.Focus();
            }
            else if (this.ServerNameEdit.EditValue.ToString().Trim() == "")
            {
                this.MailInfoOutput.Text = this.MailInfoOutput.Text + "����д��������ַ!\r\n";
                this.ServerNameEdit.Manager.Form.Focus();
            }
            else if (this.UserNameEdit.EditValue.ToString().Trim() == "")
            {
                this.MailInfoOutput.Text = this.MailInfoOutput.Text + "����д�û�!\r\n";
                this.UserNameEdit.Manager.Form.Focus();
            }
            else if (this.PasswordEdit.EditValue.ToString().Trim() == "")
            {
                this.MailInfoOutput.Text = this.MailInfoOutput.Text + "����д����!\r\n";
                this.PasswordEdit.Manager.Form.Focus();
            }
            else if (this.SubjectEdit.EditValue.ToString().Trim() == "")
            {
                this.MailInfoOutput.Text = this.MailInfoOutput.Text + "����д����!\r\n";
                this.SubjectEdit.Manager.Form.Focus();
            }
            else if (((String)ReceiveNameEdit.EditValue).Trim() == "")
            {
                this.MailInfoOutput.Text = "����д������!\r\n";
                this.ReceiveNameEdit.Manager.Form.Focus();
            }
            else
            {
                this.modelRecord.Nickname = this.NickNameEdit.EditValue.ToString().Trim();
                this.modelRecord.Server = this.ServerNameEdit.EditValue.ToString().Trim();
                this.modelRecord.Subject = this.SubjectEdit.EditValue.ToString().Trim();
                this.modelRecord.UserName = this.UserNameEdit.EditValue.ToString().Trim();
                this.modelRecord.Password = this.PasswordEdit.EditValue.ToString().Trim();
                this.modelRecord.Addresser = this.SendMailNameEdit.EditValue.ToString().Trim();
                this.modelRecord.Content = this.editor.BodyHtml;
            }
        }

        private void tlsDelAddreess_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("ȷ��Ҫɾ����?", "������ʾ", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                string text = "";
                string param = this.trvListInfo.SelectedNode.Text;
                if (this.trvListInfo.SelectedNode.Parent.Text == "�����û���")
                {
                    text = new UserNameList().DelUserNameList(param);
                }
                else if (this.trvListInfo.SelectedNode.Parent.Text == "�����ռ���")
                {
                    text = new AddressList().DelAddressList(param);
                }
                MessageBox.Show(text, "�������", MessageBoxButtons.OK, MessageBoxIcon.None);
            }
            this.trvListInfo.ExpandAll();
            this.BindTreeView();
        }

        private void tlsAllDelAddreess_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("ȷ��Ҫɾ����?", "������ʾ", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                string text = "";
                string text1 = this.trvListInfo.SelectedNode.Text;
                if (this.trvListInfo.SelectedNode.Parent.Text == "�����û���")
                {
                    text = new UserNameList().DelUserNameList();
                }
                else if (this.trvListInfo.SelectedNode.Parent.Text == "�����ռ���")
                {
                    text = new AddressList().DelAddressList();
                }
                MessageBox.Show(text, "�������", MessageBoxButtons.OK, MessageBoxIcon.None);
            }
            this.trvListInfo.ExpandAll();
            this.BindTreeView();
        }

        private void AddAccessory_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            try
            {
                DeleteAccessory.Enabled = true;
                this.importFileDlg.FileName = "";
                this.importFileDlg.ShowDialog();
                this.importFileDlg.CheckFileExists = true;
                this.importFileDlg.Filter = "(*.*)|*.*";
                this.importFileDlg.AddExtension = true;
                if ((this.importFileDlg.FileName != null) && (this.importFileDlg.FileName != ""))
                {
                    this.fileStream = new FileStream(this.importFileDlg.FileName, FileMode.Open, FileAccess.Read);
                    this.AccStatic.Caption = this.importFileDlg.FileName.Substring(this.importFileDlg.FileName.LastIndexOf(@"\") + 1) + this.FileLenght(this.fileStream.Length);
                    this.modelRecord.AttachmentSrc = this.importFileDlg.FileName;
                    this.modelRecord.AttachmentName = this.importFileDlg.FileName.Substring(this.importFileDlg.FileName.LastIndexOf(@"\") + 1);
                    this.importFileDlg.RestoreDirectory = true;
                }
            }
            catch (Exception)
            {
                this.MailInfoOutput.Text = this.MailInfoOutput.Text + "�������ʧ��!\r\n";
            }
        }

        private string FileLenght(long param)
        {
            if ((param / 0x400L) > 0x400L)
            {
                if ((param / 0x100000L) > 0x400L)
                {
                    return "(�����ѳ������ͷ�Χ)";
                }
                double num = ((double)param) / 1048576.0;
                return ("(�ļ���С:" + num.ToString("#0.00") + "MB)");
            }
            double num2 = ((double)param) / 1024.0;
            return ("(�ļ���С:" + num2.ToString("#0.00") + "KB)");
        }

        private void LogRecord_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            Log_Form form = new Log_Form();
            this.modelRecord.LogParam = null;
            form.ShowDialog();
        }

        private void DeleteAccessory_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.AccStatic.Caption = "�޸���";
            this.MailInfoOutput.Text = this.MailInfoOutput.Text + "�������!";
            this.modelRecord.AttachmentName = null;
            this.modelRecord.AttachmentSrc = null;
            DeleteAccessory.Enabled = false;
        }

        private void AddSMTPServer_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            AddSmtpServer TmpDlg = new AddSmtpServer();
            TmpDlg.ShowDialog();
            bool flag = false;
            if (TmpDlg.strSMTP != "")
            {
                for (int i = 0; i < (this.MailServerComboBox).Items.Count; i++)
                {
                    if (TmpDlg.strSMTP == (this.MailServerComboBox).Items[i].ToString())
                    {
                        flag = false;
                        break;
                    }
                    flag = true;
                }
                if (flag)
                {
                    (this.MailServerComboBox).Items.Add(TmpDlg.strSMTP);
                }
                else
                {
                    this.MailInfoOutput.Text = this.MailInfoOutput.Text + "SMTP�����������Ѵ���!";
                }
            }
        }

        private void AddDNS_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            AddDNS TmpDlg = new AddDNS();
            TmpDlg.ShowDialog();

            if (TmpDlg.strDNS != "")
            {
                Record record = new Record();
                MessageBox.Show(record.Dns(TmpDlg.strDNS), "�����ʾ", MessageBoxButtons.OK, MessageBoxIcon.None);
            }
            else
            {
                this.MailInfoOutput.Text = this.MailInfoOutput.Text + "DNS����Ϊ��!\r\n";
            }

        }

        private void TimeSend_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {

        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            string str = "Time";
            if (MessageBox.Show("ȷ��Ҫ�˳���?", "��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
            {
                Record record = new Record();
                record.SetRecord(this.NickNameEdit.EditValue.ToString(), this.SendMailNameEdit.EditValue.ToString(), this.ReceiveNameEdit.EditValue.ToString(), this.UserNameEdit.EditValue.ToString(), this.SubjectEdit.EditValue.ToString(), this.ServerNameEdit.EditValue.ToString());
                DateTime now = new DateTime();
                now = DateTime.Now;
                int year = now.Year;
                int month = now.Month;
                int day = now.Day;
                string str2 = string.Concat(new object[] { str, year, "��", month, "��", day, "��" });
                record.ini("get");
                Application.ExitThread();
            }
            else
            {
                e.Cancel = true;
            }

        }

        private void ClearStatusButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.MailInfoOutput.Text = "";
        }

        private void ExitButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            new Record().SetRecord(this.NickNameEdit.EditValue.ToString(), this.SendMailNameEdit.EditValue.ToString(), this.ReceiveNameEdit.EditValue.ToString(), this.UserNameEdit.EditValue.ToString(), this.SubjectEdit.EditValue.ToString(), this.ServerNameEdit.EditValue.ToString());
            Application.Exit();
        }

        private void trvListInfo_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            this.trvListInfo.SelectedNode = e.Node;
            string str = this.trvListInfo.SelectedNode.Text.Trim().ToString();
            if (((str != "") && (str != "�����ռ���")) && (str != "�����û���"))
            {
                if (this.trvListInfo.SelectedNode.Parent.Text == "�����û���")
                {
                    if ((this.UserNameEdit.EditValue.ToString() == "") && (str != "�����û���"))
                    {
                        this.UserNameEdit.EditValue = str;
                    }
                    else
                    {
                        this.MailInfoOutput.Text = this.MailInfoOutput.Text + "�û����Ѿ�����!\r\n";
                    }
                }
                else if (this.trvListInfo.SelectedNode.Parent.Text == "�����ռ���")
                {
                    if ((this.ReceiveNameEdit.EditValue.ToString() == "") && (str != "�����ռ���"))
                    {
                        this.ReceiveNameEdit.EditValue = str;
                    }
                    else
                    {
                        this.ReceiveNameEdit.EditValue = this.ReceiveNameEdit.EditValue + ";" + str;
                    }
                }
            }

        }

        private void trvLogList_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            this.trvLogList.SelectedNode = e.Node;
            string str = this.trvLogList.SelectedNode.Text.Trim().ToString();
            if (str != "������־")
            {
                this.modelRecord.LogParam = str;
                new Log_Form().ShowDialog();
            }
        }

        private void MainRibbonControl_SelectedPageChanged(object sender, EventArgs e)
        {
            String SelectedPageName = ((DevExpress.XtraBars.Ribbon.RibbonControl)sender).SelectedPage.Name;
            if (SelectedPageName == "MailXssPage")
            {
                hideContainerLeft.Visible = true;
                StatusInfo.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Visible;
                MailToolBox.Visibility = DevExpress.XtraBars.Docking.DockVisibility.AutoHide;
                InfoRecord.Visibility = DevExpress.XtraBars.Docking.DockVisibility.AutoHide;
                SendMailRecord.Visibility = DevExpress.XtraBars.Docking.DockVisibility.AutoHide;
                hideContainerRight.Visible = true;
                DisplayMainEditor();
                
            }
            else if (SelectedPageName == "CookieMail")
            {
                hideContainerLeft.Visible = false;
                StatusInfo.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Hidden;
                MailToolBox.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Hidden;
                InfoRecord.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Hidden;
                SendMailRecord.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Hidden;
                hideContainerRight.Visible = false;
                DisplayMailCookieControl();
            }
            else
            {
                hideContainerLeft.Visible = false;
                StatusInfo.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Hidden;
                MailToolBox.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Hidden;
                InfoRecord.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Hidden;
                SendMailRecord.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Hidden;
                hideContainerRight.Visible = false;
                DisplayPopMailControl();
            }
        }

        private void showCookieInfo(object sender, EventArgs e)
        {
            GlobalValue.mainForm.UserList = GlobalValue.mainForm.treeViewUsers.SelectedNode.Text.Trim();
            new CookieInfo().Show();
        }

        private void DeleteRepeatInfo_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string str = "";
            string s = "";
            string path = "";
            int num = 0;
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "�ı��ļ�(*.txt)|*.txt";
            dialog.ShowDialog();
            if (dialog.FileName != "")
            {
                StreamReader reader = new StreamReader(dialog.FileName, Encoding.Default);
                while ((str = reader.ReadLine()) != null)
                {
                    if (s.IndexOf(str) == -1)
                    {
                        s = s + str + "\r\n";
                    }
                }
                reader.Close();
                num = dialog.FileName.LastIndexOf(@"\");
                if (num != -1)
                {
                    path = dialog.FileName.Substring(0, num + 1) + "NewCookie.txt";
                    FileStream stream = null;
                    stream = File.Open(path, FileMode.Append, FileAccess.Write);
                    byte[] bytes = Encoding.UTF8.GetBytes(s);
                    stream.Write(bytes, 0, bytes.Length);
                    stream.Close();
                }
            }
        }

        private void ParamOptionSetting_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            new frmSetting().ShowDialog();
        }

        private void FishCookieSetting_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            new FishCookie().Show();
        }

        private void ImportCookieTxt_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.showCookieInfo(sender, e);
        }

        private void AddUserBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            PopfrmAddUsers users = new PopfrmAddUsers();
            users.Show();
            users.TopMost = true;
        }

        private void UserEditBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            PopfrmAddUsers users = new PopfrmAddUsers
            {
                Text = "�༭�û�"
            };
            if (users.EditeUser(GlobalValue.PopMainForm.dataGridView, GlobalValue.PopMainForm.userDataSet))
            {
                users.Show();
                users.TopMost = true;
            }
        }

        private void DeleteUserBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalValue.PopMainForm.ButtonStopReceiveEmail_Click(sender, e);
            int y = GlobalValue.PopMainForm.dataGridView.CurrentCellAddress.Y;
            if (y >= 0)
            {
                string str = GlobalValue.PopMainForm.userDataSet.Tables["users"].Rows[y]["���"].ToString();
                string strSql = "delete from users where ���=" + str;
                string str3 = "select �û��� from users where ���=" + str;
                string str4 = GlobalValue.PopMainForm.ExecuteSQL(str3);

                if (MessageBox.Show("��ȷ��ɾ�����û�:" + str4, "����", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    GlobalValue.PopMainForm.ExecuteSQL(strSql);
                }
                string sqlCountUsers = "select count(*) from users where �û�����='��ͨ�û�'";
                GlobalValue.PopMainForm.UsersCount(sqlCountUsers);
            }
            else
            {
                MessageBox.Show("��ѡ���û���");
            }
        }

        private void ExportUserBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ExplainPasswd passwd = new ExplainPasswd();
            Console.WriteLine("��ʼִ��");
            passwd.Start();
        }

        private void ImportUserBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string str = "";
            string userName = "";
            string passWd = "";
            string str4 = "";
            string str5 = "";
            string str6 = "";
            string str7 = "'1'";
            string str8 = "'?'";
            string str9 = "'0'";
            string str10 = "1";
            string str11 = "";
            string str12 = "";
            string str13 = "";
            string str14 = "'��'";
            string str15 = "";
            string str16 = "";
            string str17 = "";
            string strSql = "";
            str11 = "'" + str11 + "'";
            str12 = "'" + str12 + "'";
            str13 = "'" + str13 + "'";
            string str19 = "";
            OpenFileDialog dialog = new OpenFileDialog
            {
                Filter = "�ı��ļ�(*.txt)|*.txt"
            };
            dialog.ShowDialog();
            if (dialog.FileName != "")
            {
                StreamReader reader = new StreamReader(dialog.FileName, Encoding.UTF8);
                while ((str19 = reader.ReadLine()) != null)
                {
                    try
                    {
                        if (str19.IndexOf('\t') != -1)
                        {
                            int startIndex = 0;
                            int index = str19.IndexOf('\t');
                            str15 = str19.Substring(startIndex, index - startIndex).Trim();
                            startIndex = index + 1;
                            index = str19.IndexOf('\t', startIndex);
                            passWd = str19.Substring(startIndex, index - startIndex).Trim();
                            startIndex = index + 1;
                            index = str19.IndexOf('\t', startIndex);
                            str = str19.Substring(startIndex, index - startIndex).Trim();
                            startIndex = index + 1;
                            index = str19.IndexOf('\t', startIndex);
                            if (index == -1)
                            {
                                str4 = str19.Substring(startIndex).Trim();
                            }
                            else
                            {
                                str4 = str19.Substring(startIndex, index - startIndex).Trim();
                            }
                            if (str15.IndexOf("@") != -1)
                            {
                                userName = str15.Substring(0, str15.IndexOf("@")).Trim();
                                passWd = GlobalValue.PopMainForm.Encryption(passWd, userName);
                                userName = "'" + userName + "'";
                                passWd = "'" + passWd + "'";
                                str = "'" + str + "'";
                                str4 = "'" + str4 + "'";
                                str16 = str15.Substring(str15.IndexOf("@") + 1, str15.Length - (str15.IndexOf("@") + 1)).Trim();
                                if (str16.ToUpper().IndexOf("POP") != -1)
                                {
                                    str5 = "POP";
                                }
                                else if (str16.IndexOf("yahoo") != -1)
                                {
                                    str5 = "Yahoo";
                                }
                                else if (str16.IndexOf("hotmail") != -1)
                                {
                                    str5 = "Hotmail";
                                }
                                else if (str16.IndexOf("gmail") != -1)
                                {
                                    str5 = "Gmail";
                                }
                                else if (str16.IndexOf("163.com") != -1)
                                {
                                    str5 = "163";
                                }
                                else if (str16.IndexOf("126.com") != -1)
                                {
                                    str5 = "126";
                                }
                                else if (str16.IndexOf("ru") != -1)
                                {
                                    str5 = "Ru";
                                }
                                else if (str16.IndexOf("hinet") != -1)
                                {
                                    str5 = "Hinet";
                                }
                                else if (str16.IndexOf("msn") != -1)
                                {
                                    str5 = "Hotmail";
                                }
                                else
                                {
                                    str5 = "POP";
                                }
                                str5 = "'" + str5 + "'";
                                str6 = "'" + str16 + "'";
                                string str20 = "SELECT  count(*)  FROM users WHERE ��������ַ=" + str6 + "and  �û���=" + userName;
                                if (Convert.ToInt32(GlobalValue.PopMainForm.ExecuteSQL(str20)) > 0)
                                {
                                    GlobalValue.PopMainForm.listBoxView.Items.Add("���û��Ѿ�����" + str19);
                                    GlobalValue.PopMainForm.listBoxView.SelectedIndex = GlobalValue.PopMainForm.listBoxView.Items.Count - 1;
                                }
                                else
                                {
                                    str17 = "(" + str + "," + userName + "," + passWd + "," + str4 + "," + str5 + "," + str6 + "," + str7 + "," + str8 + "," + str9 + "," + str10 + "," + str11 + "," + str12 + "," + str13 + "," + str14 + ",'��ͨ�û�')";
                                    strSql = "insert into users (����,�û���,������,����,������,��������ַ,����,����״̬,�ʼ�����,����ʱ��,��ʼʱ��,ʱ���,ʱ����,ȫ������,�û�����)values" + str17;
                                    GlobalValue.PopMainForm.ExecuteSQL(strSql);
                                    GlobalValue.PopMainForm.listBoxView.Items.Add("����û�:" + userName.Trim(new char[] { '\'' }) + "@" + str6.Trim(new char[] { '\'' }));
                                }
                            }
                        }
                        continue;
                    }
                    catch (Exception exception)
                    {
                        GlobalValue.PopMainForm.listBoxView.Items.Add("�쳣" + exception.Message + ":" + str19);
                        continue;
                    }
                }
                reader.Close();
            }

        }

        private void ParamSettingBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            new PopMailSetting { TopMost = true }.ShowDialog();
        }

        private void ConvertBase64BarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ConvertBaseForm convertForm = new ConvertBaseForm();
            convertForm.ShowDialog();
        }

        static public void KillMailProcess()
        {
            foreach (Process process in Process.GetProcesses())
            {
                if (process.Id == Process.GetCurrentProcess().Id)
                {
                    process.Kill();
                }
                process.Dispose();
            }
        }
	}
}
