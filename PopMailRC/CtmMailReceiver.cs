using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.IO;

namespace MailExposure.PopMailRC
{
    class CtmMailReceiver:MailStr
    {
        // Fields
        private CookieContainer cookieContainer;
        private string urls;

        // Methods
        public CtmMailReceiver()
        {
            this.urls = "";
        }
        public override void login()
        {
            base.m_passwd = base.strPassParse(base.m_passwd);
            try
            {
                base.ShowMessage("开始登陆…………");
                string url = "https://commexpress.cyberctm.com/uwc/auth?username=vakiopou%40macau.ctm.net&password=kv9817";
                this.cookieContainer = new CookieContainer();
                base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                base.streamControl = true;
                base.MyStringBuilder = this.Request(url);
                int startIndex = 0;
                startIndex = base.MyStringBuilder.ToString().IndexOf("window.location");
                if (startIndex != -1)
                {
                    url = base.putstr(base.MyStringBuilder.ToString(), "\"", "\"", startIndex);
                    if ((url != "-1") && (url != null))
                    {
                        base.Host = this.urls.Substring(0, this.urls.IndexOf("/uwc"));
                        url = base.Host + url;
                        base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                        base.streamControl = true;
                        base.MyStringBuilder = this.Request(url);
                        startIndex = base.MyStringBuilder.ToString().IndexOf("window.location");
                        if (startIndex != -1)
                        {
                            url = base.putstr(base.MyStringBuilder.ToString(), "\"", "\"", startIndex);
                            if ((url != "-1") && (url != null))
                            {
                                base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                                base.streamControl = true;
                                base.MyStringBuilder = this.Request(url);
                            }
                        }
                    }
                }
                else if (base.MyStringBuilder.ToString().IndexOf("(01406)") != -1)
                {
                    base.ShowMessage("您已經執行登出或連線逾時，請重新登入。");
                    base.LoginFail();
                    base.passwdErr();
                    base.ShowMessage("登陆失败…………无法进入");
                }
                else if (base.MyStringBuilder.ToString().IndexOf("(01604)") != -1)
                {
                    base.ShowMessage("由於網頁郵件服務不提供同一帳號、同時間在不同視窗登入，若有重複登入、不正常登出(直接關閉視窗或是因系統錯誤而離開)之情形，請重新登入");
                    base.LoginFail();
                    base.passwdErr();
                    base.ShowMessage("登陆失败…………无法进入");
                }
                else if (this.urls.IndexOf("/mailService") != -1)
                {
                    base.Host = this.urls.Substring(0, this.urls.IndexOf("/mailService"));
                    url = base.Host + "/mailService/mail/M_main_8.jsp";
                    base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                    base.streamControl = true;
                    base.MyStringBuilder = this.Request(url);
                }
                else
                {
                    base.LoginFail();
                    base.passwdErr();
                    base.ShowMessage("登陆失败…………无法进入");
                }
            }
            catch (Exception exception)
            {
                base.LoginFail();
                base.passwdErr();
                base.ShowMessage("登陆失败!" + exception.Message + exception.StackTrace);
            }

        }
        private StringBuilder Request(string url)
        {
            try
            {
                StreamReader reader;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.CookieContainer = this.cookieContainer;
                request.Method = "GET";
                request.Accept = "*/*";
                request.AllowAutoRedirect = false;
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1)";
                request.ContentType = "application/x-www-form-urlencoded";
                request.Headers.Add("Accept-Language: zh-cn");
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                response.Cookies = this.cookieContainer.GetCookies(request.RequestUri);
                WebHeaderCollection headers = response.Headers;
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
                    else if (this.urls.IndexOf("/zh-TW/mail.html") != -1)
                    {
                        this.urls = base.Host + this.urls;
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
            }
            catch (Exception)
            {
            }
            return base.MyStringBuilder;

        }

    }
}
