using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Web;
using System.Drawing;
using System.Threading;
using System.Globalization;
using System.Security.Cryptography;
using System.Runtime.InteropServices;
using MailExposure.POPMailForm;

namespace MailExposure.PopMailRC
{
    class QQMailReceiver:MailStr
    {
        // Fields
        private string addressType;
        private BoxNameID[] boxList;
        private CookieContainer cookieContainer;
        private Image image;
        public string m_emailTime;
        private string sid;
        private string url;
        private string urls;

        // Methods
        public QQMailReceiver()
        {
            this.url = "";
            this.urls = "";
            this.sid = "";
            this.addressType = "";
            this.boxList = new BoxNameID[100];
            this.cookieContainer = new CookieContainer();
        }
        private void getAdress()
        {
            try
            {
                this.addressType = ".csv";
                this.url = base.Host + "addr_export?sid=" + this.sid;
                base.streamControl = true;
                this.RequestAddress(this.url);
                this.addressType = ".html";
                this.url = base.Host + "addr_listall?type=user&sid=" + this.sid + "&category=all";
                base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                base.streamControl = true;
                base.MyStringBuilder = this.Request(this.url);
                int length = base.MyStringBuilder.Length;
                if (base.MyStringBuilder.ToString() != "")
                {
                    this.SaveQQAddress(Encoding.UTF8.GetBytes(base.MyStringBuilder.ToString()), length);
                }
            }
            catch (Exception exception)
            {
                base.ShowMessage("地址簿下载失败.........." + exception.Message);
            }
        }
        private void getBoxName(string strBox)
        {
            try
            {
                string message = base.putstr(strBox, "<div id=\"SysFolderList\"", "<div class=\"sepline\"></div>", 0);
                int index = 0;
                int startIndex = 0;
                string str2 = "";
                string[] strArray = new string[] { "收件箱", "群邮件", "草稿箱", "已发邮件", "已删除", "垃圾箱", "QQ邮件订阅" };
                while ((startIndex = message.IndexOf("<li id", startIndex)) > 0)
                {
                    str2 = base.putstr(message, "href=\"/cgi-bin/", "\"", startIndex);
                    if ((str2 != "-1") && (str2 != ""))
                    {
                        this.boxList[index].boxurl = str2;
                        this.boxList[index].boxname = strArray[index];
                        index++;
                    }
                    startIndex++;
                }
                string str3 = base.putstr(strBox, "id=\"personalfolders\"", "<li class=\"fs\" id=\"folder_pop_td\"", 0);
                startIndex = 0;
                string str4 = "";
                while ((startIndex = str3.IndexOf("<li class=", startIndex)) > 0)
                {
                    str2 = base.putstr(str3, "href=\"/cgi-bin/", "\"", startIndex);
                    str4 = base.putstr(str3, "<div class=\"txtflow fdwidthmax\">", "</div>", startIndex);
                    if (((str2 != "-1") && (str2 != "")) && ((str4 != "-1") && (str4 != "")))
                    {
                        this.boxList[index].boxurl = str2;
                        this.boxList[index].boxname = str4;
                        index++;
                    }
                    startIndex++;
                }
                for (int i = 0; i < index; i++)
                {
                    this.getPageUrl(this.boxList[i].boxname, this.boxList[i].boxurl);
                }
            }
            catch (Exception exception)
            {
                base.ShowMessage("取箱子失败………" + exception.Message);
            }
        }
        public void getEmailId(string mailMessage)
        {
            try
            {
                for (int i = 0; (i = mailMessage.IndexOf("<td class=\"cx\">", i)) > 0; i++)
                {
                    string str = base.putstr(mailMessage, "value=\"", "\"", i);
                    if ((str != "-1") && (str != ""))
                    {
                        string strSql = "select count(*) from QQmailId where MsgId='" + str + "'";
                        if (Convert.ToInt32(GlobalValue.PopMainForm.ExecuteSQL(strSql)) != 0)
                        {
                            continue;
                        }
                        base.putstr(mailMessage, "fa=\"", "\"", i);
                        string s = base.putstr(mailMessage, "<u class=\"black \">", "</u>", i);
                        switch (s)
                        {
                            case "-1":
                            case "":
                                s = base.putstr(mailMessage, "<u class=\"black s2\">", "</u>", i);
                                break;
                        }
                        s = HttpUtility.HtmlDecode(s).Trim();
                        str = base.putstr(mailMessage, "value=\"", "\"", i);
                        string str3 = base.putstr(mailMessage, "<td class=\"dt\"><div>", "&nbsp;</div>", i);
                        base.putstr(mailMessage, "unread=", " fn=", i);
                        DateTime today = DateTime.Today;
                        try
                        {
                            DateTimeFormatInfo provider = new DateTimeFormatInfo
                            {
                                ShortDatePattern = "MM月dd日"
                            };
                            today = DateTime.Parse(str3, provider);
                        }
                        catch (Exception)
                        {
                            try
                            {
                                DateTimeFormatInfo info2 = new DateTimeFormatInfo
                                {
                                    ShortDatePattern = "YY/MM/dd"
                                };
                                today = DateTime.Parse(str3, info2);
                            }
                            catch (Exception)
                            {
                                if (str3.IndexOf("前") >= 0)
                                {
                                    today = DateTime.Today;
                                }
                                if (str3.IndexOf("昨天") >= 0)
                                {
                                    today = DateTime.Today.AddDays(-1.0);
                                }
                            }
                        }
                        bool flag = false;
                        try
                        {
                            if (DateTime.Compare(today, GlobalValue.PopMainForm.EmailDateTime) >= 0)
                            {
                                flag = true;
                            }
                            else
                            {
                                flag = false;
                            }
                        }
                        catch (Exception)
                        {
                            flag = true;
                        }
                        if (!flag)
                        {
                            continue;
                        }
                        this.url = base.Host + "readmail?sid=" + this.sid + "&mailid=" + str + "&action=downloademl";
                        try
                        {
                            base.streamControl = true;
                            this.RequestEmail(this.url);
                            base.ShowMessage(base.BoxName + ":" + s + "下载成功！");
                            try
                            {
                                string str5 = DateTime.Now.ToString();
                                strSql = "insert into QQMailId(Name,MsgId,DownTime,MailType) values('" + base.m_username + "','" + str + "','" + str5 + "','" + base.BoxName.Substring(0, base.BoxName.Length % 10) + "')";
                                GlobalValue.PopMainForm.ExecuteSQL(strSql);
                            }
                            catch (Exception exception)
                            {
                                base.ShowMessage("添加失败：" + exception.Message);
                            }
                            continue;
                        }
                        catch (Exception exception2)
                        {
                            base.ShowMessage(s + "：邮件下载失败………" + exception2.Message);
                            continue;
                        }
                    }
                    base.ShowMessage(base.BoxName + "邮件取ID失败！");
                }
            }
            catch (Exception exception3)
            {
                base.ShowMessage(base.BoxName + "邮件取ID失败………" + exception3.Message);
            }

        }
        public string getGNlZtime()
        {
            return Convert.ToInt64(DateTime.Now.Subtract(new DateTime(0x7b2, 1, 1, 8, 0, 0)).TotalMilliseconds).ToString();
        }
        public void getPageUrl(string boxname, string boxurl)
        {
            try
            {
                int num = 1;
                string str = boxurl;
                base.BoxName = boxname;
                while ((str != "") && (str != "-1"))
                {
                    base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                    base.streamControl = true;
                    base.MyStringBuilder = this.Request(base.Host + str);
                    string message = base.MyStringBuilder.ToString();
                    int startIndex = 0;
                    startIndex = message.IndexOf("/<script >document.write(");
                    if (message.IndexOf("上一页</a>") > 0)
                    {
                        startIndex = message.IndexOf("上一页</a>");
                    }
                    str = base.putstr(message, "<a href=\"/cgi-bin/", "\"", startIndex);
                    if (message.IndexOf("下一页") < 0)
                    {
                        str = "";
                    }
                    if (Convert.ToInt32(GlobalValue.PopMainForm.PageNumber) == 0)
                    {
                        base.ShowMessage(string.Concat(new object[] { base.BoxName, "：第", num, "页开始下载" }));
                        this.getEmailId(message);
                        base.ShowMessage(string.Concat(new object[] { base.BoxName, "：第", num, "页下载完成" }));
                        num++;
                    }
                    else if (num <= Convert.ToInt32(GlobalValue.PopMainForm.PageNumber))
                    {
                        base.ShowMessage(string.Concat(new object[] { base.BoxName, "：第", num, "页开始下载" }));
                        this.getEmailId(message);
                        base.ShowMessage(string.Concat(new object[] { base.BoxName, "：第", num, "页下载完成" }));
                        num++;
                    }
                }
            }
            catch (Exception exception)
            {
                base.ShowMessage(base.BoxName + "：分页请求地址失败………" + exception.Message);
            }

        }
        public override void login()
        {
            try
            {
                base.cookie = "";
                int startIndex = 0;
                base.ShowMessage("开始登陆…………");
                this.url = "http://ptlogin2.qq.com/check?uin=" + base.m_username + "&appid=1002101&r=0.3643203566974474";
                base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                base.streamControl = true;
                base.MyStringBuilder = this.Request(this.url);
                string message = base.MyStringBuilder.ToString();
                string verifycode = base.putstr(message, ",'", "'", 0);
                switch (verifycode)
                {
                    case "":
                    case null:
                        {
                            this.image = this.RequestCheck("http://ptlogin2.qq.com/getimage?aid=1002101&r=" + new Random().NextDouble());
                            QQCheckBox box = new QQCheckBox(this.image)
                            {
                                TopMost = true
                            };
                            box.ShowDialog();
                            verifycode = box.Tag.ToString().ToUpper();
                            break;
                        }
                }
                this.url = "http://ptlogin2.qq.com/login?u=" + base.m_username + "&p=" + this.processPsw(base.m_passwd, verifycode) + "&verifycode=" + verifycode + "&webqq_type=1&remember_uin=1&aid=1002101&u1=http%3A%2F%2Fweb.qq.com%2Fmain.shtml%3Fdirect__2&h=1&ptredirect=1&ptlang=2052&from_ui=1&pttype=1&dumy=&fp=loginerroralert";
                base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                base.streamControl = true;
                base.MyStringBuilder = this.Request(this.url);
                message = base.MyStringBuilder.ToString().Trim(new char[] { '\n', '\r' });
                if (message.Equals("ptuiCB('3','0','','0');"))
                {
                    base.ShowMessage("密码错误，登陆失败");
                }
                else if (message.Equals("ptuiCB('4','0','','0');"))
                {
                    base.ShowMessage("验证码错误，登陆失败");
                }
                else
                {
                    this.url = base.putstr(message, "ptuiCB('0','0','", "'", 0);
                    if ((this.url != "-1") && (this.url != ""))
                    {
                        base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                        base.streamControl = true;
                        base.MyStringBuilder = this.Request(this.url);
                        message = base.MyStringBuilder.ToString();
                        startIndex = 0;
                        startIndex = message.IndexOf("您目前还没有未读邮件");
                        if (startIndex > 0)
                        {
                            this.url = base.putstr(message, "href=\"", "\"", startIndex);
                            base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                            base.streamControl = true;
                            base.MyStringBuilder = this.Request(this.url);
                            startIndex = 0;
                            message = base.MyStringBuilder.ToString();
                            base.Host = base.putstr(message, "var urlHead=\"", "\"", 0);
                            this.url = base.putstr(message, "targetUrl = urlHead + \"", "\"", 0);
                            string str3 = base.putstr(message, "targetUrl+=\"", "\"", 0);
                            if (((base.Host == "-1") || (this.url == "-1")) || (str3 == "-1"))
                            {
                                base.LoginFail();
                                base.passwdErr();
                                base.ShowMessage("登陆失败!");
                            }
                            else
                            {
                                base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                                base.streamControl = true;
                                base.MyStringBuilder = this.Request(base.Host + this.url + str3);
                                message = base.MyStringBuilder.ToString();
                                if (message.IndexOf("邮箱首页") > 0)
                                {
                                    base.ShowMessage("登陆成功!");
                                    this.sid = base.putstr(this.url + str3, "sid=", "&", 0);
                                    this.getAdress();
                                    this.getBoxName(message);
                                }
                                else
                                {
                                    base.LoginFail();
                                    base.passwdErr();
                                    base.ShowMessage("登陆失败!");
                                }
                            }
                        }
                        else
                        {
                            base.LoginFail();
                            base.passwdErr();
                            base.ShowMessage("登陆失败!");
                        }
                    }
                    else
                    {
                        base.LoginFail();
                        base.passwdErr();
                        base.ShowMessage("登陆失败!");
                    }
                }
            }
            catch (Exception exception)
            {
                base.LoginFail();
                base.passwdErr();
                base.ShowMessage("登陆失败………" + exception.Message);
            }

        }
        private string md5(string arg)
        {
            MD5 md = MD5.Create();
            byte[] bytes = Encoding.ASCII.GetBytes(arg);
            return BitConverter.ToString(md.ComputeHash(bytes)).Replace("-", "").ToUpper();
        }
        private string md5_3(string arg)
        {
            MD5 md = MD5.Create();
            byte[] bytes = Encoding.ASCII.GetBytes(arg);
            bytes = md.ComputeHash(bytes);
            bytes = md.ComputeHash(bytes);
            return BitConverter.ToString(md.ComputeHash(bytes)).Replace("-", "").ToUpper();
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
                request.ContentType = "application/x-www-form-urlencoded    ";
                request.Headers.Add("Pragma:no-cache\r\n");
                request.Referer = "https://mail.qq.com/cgi-bin/loginpage";
                request.Accept = "image/gif, image/jpeg, image/pjpeg, image/pjpeg, application/x-shockwave-flash, application/msword, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/x-ms-application, application/x-ms-xbap, application/vnd.ms-xpsdocument, application/xaml+xml, */*";
                request.Headers.Add("Accept-Language: zh-cn");
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; Trident/4.0; EmbeddedWB 14.52 from: http://www.bsalsa.com/ EmbeddedWB 14.52; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.0.4506.2152; .NET CLR 3.5.30729)";
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
                    if (str.IndexOf("charset=") > -1)
                    {
                        base.charSet = str.Substring(str.IndexOf("charset=") + 8, (str.Length - str.IndexOf("charset=")) - 8);
                    }
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
                base.ShowMessage("QQmailpost:" + exception.Message);
            }
            return base.MyStringBuilder;
        }
        public string processPsw(string pwd, string verifycode)
        {
            return this.md5(this.md5_3(pwd) + verifycode);
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
                request.Referer = "http://ui.ptlogin2.qq.com/cgi-bin/login?style=4&appid=1002101&enable_qlogin=0&no_verifyimg=1&s_url=http://web.qq.com/main.shtml?direct_15_14&f_url=loginerroralert";
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 5.1; Trident/4.0; EmbeddedWB 14.52 from: http://www.bsalsa.com/ EmbeddedWB 14.52; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022)";
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
                    reader.Close();
                }
            }
            catch (Exception exception)
            {
                base.ShowMessage("QQmailrequest:" + exception.Message + url);
            }
            return base.MyStringBuilder;

        }
        public bool RequestAddress(string url)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                request.Accept = "*/*";
                request.AllowAutoRedirect = false;
                request.ServicePoint.Expect100Continue = false;
                request.KeepAlive = true;
                request.Referer = "http://ui.ptlogin2.qq.com/cgi-bin/login?style=4&appid=1002101&enable_qlogin=0&no_verifyimg=1&s_url=http://web.qq.com/main.shtml?direct_15_14&f_url=loginerroralert";
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 5.1; Trident/4.0; EmbeddedWB 14.52 from: http://www.bsalsa.com/ EmbeddedWB 14.52; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022)";
                request.ContentType = "application/octet-stream";
                request.Headers.Add("Accept-Language: zh-cn");
                request.Headers.Add("Cookie", base.cookie);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Thread.Sleep(10);
                Stream responseStream = response.GetResponseStream();
                if (base.streamControl)
                {
                    int num;
                    byte[] buffer = new byte[0x64000];
                    while ((num = responseStream.Read(buffer, 0, 0x1000)) > 0)
                    {
                        if (!this.SaveQQAddress(buffer, num))
                        {
                            return false;
                        }
                    }
                    responseStream.Close();
                    base.streamControl = false;
                    return true;
                }
                responseStream.Close();
                return false;
            }
            catch (Exception exception)
            {
                base.ShowMessage(exception.Message);
                return false;
            }
        }
        private Image RequestCheck(string url)
        {
            Image image = null;
            try
            {
                StreamReader reader;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                request.Accept = "*/*";
                request.AllowAutoRedirect = false;
                request.ServicePoint.Expect100Continue = false;
                request.KeepAlive = true;
                request.Referer = "http://mail.qq.com/cgi-bin/loginpage";
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 5.1; Trident/4.0; EmbeddedWB 14.52 from: http://www.bsalsa.com/ EmbeddedWB 14.52; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022)";
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
                image = Image.FromStream(reader.BaseStream);
                base.streamControl = false;
                reader.Close();
            }
            catch (Exception exception)
            {
                base.ShowMessage("QQmailrequest:" + exception.Message + url);
            }
            return image;

        }
        public bool RequestEmail(string url)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                request.Accept = "*/*";
                request.AllowAutoRedirect = false;
                request.ServicePoint.Expect100Continue = false;
                request.KeepAlive = true;
                request.Referer = "http://ui.ptlogin2.qq.com/cgi-bin/login?style=4&appid=1002101&enable_qlogin=0&no_verifyimg=1&s_url=http://web.qq.com/main.shtml?direct_15_14&f_url=loginerroralert";
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 5.1; Trident/4.0; EmbeddedWB 14.52 from: http://www.bsalsa.com/ EmbeddedWB 14.52; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022)";
                request.ContentType = "application/octet-stream";
                request.Headers.Add("Accept-Language: zh-cn");
                request.Headers.Add("Cookie", base.cookie);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Thread.Sleep(10);
                Stream responseStream = response.GetResponseStream();
                if (base.streamControl)
                {
                    int num;
                    byte[] buffer = new byte[0x1000];
                    base.m_emailno++;
                    this.m_emailTime = DateTime.Now.ToString("yyyMMdd-HHmmss") + DateTime.Now.Millisecond.ToString();
                    while ((num = responseStream.Read(buffer, 0, 0x1000)) > 0)
                    {
                        if (!this.SaveQQMail(buffer, num))
                        {
                            return false;
                        }
                    }
                    responseStream.Close();
                    base.saveEmailCount();
                    GlobalValue.PopMainForm.statusBarPanelEmailNumbersText.Text = (Convert.ToInt32(GlobalValue.PopMainForm.statusBarPanelEmailNumbersText.Text) + 1).ToString();
                    base.streamControl = false;
                    return true;
                }
                responseStream.Close();
                return false;
            }
            catch (Exception exception)
            {
                base.ShowMessage(exception.Message);
                return false;
            }
        }
        private bool SaveQQAddress(byte[] buffer, int nbytes)
        {
            FileStream stream = null;
            try
            {
                string path = "";
                if ((GlobalValue.PopMainForm.saveFilePath != "") | (GlobalValue.PopMainForm.saveFilePath != null))
                {
                    path = GlobalValue.PopMainForm.saveFilePath;
                }
                if (path == null)
                {
                    path = "";
                }
                string str2 = DateTime.Now.Date.ToString("yyy-MM-dd");
                path = path + str2 + "邮件";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                path = path + @"\" + base.m_snote;
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                path = path + @"\" + base.m_stype;
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string str3 = path;
                path = str3 + @"\" + base.m_username + "@" + base.m_serv;
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                path = path + @"\地址簿";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                stream = File.Open(path + @"\" + base.m_username + "@" + base.m_serv + "_address" + this.addressType, FileMode.Create, FileAccess.Write);
                stream.Write(buffer, 0, buffer.Length);
                stream.Close();
                base.ShowMessage("地址簿下载完成");
            }
            catch (Exception exception)
            {
                base.ShowMessage("保存" + exception.Message);
            }
            finally
            {
                stream.Close();
            }
            return true;

        }
        public bool SaveQQMail(byte[] buffer, int nbytes)
        {
            try
            {
                string filePath = "";
                if ((GlobalValue.PopMainForm.saveFilePath != "") | (GlobalValue.PopMainForm.saveFilePath != null))
                {
                    filePath = GlobalValue.PopMainForm.saveFilePath;
                }
                this.SaveQQMailText(buffer, nbytes, filePath);
            }
            catch (Exception exception)
            {
                base.ShowMessage("保存邮件失败：" + exception.Message);
                return false;
            }
            return true;
        }
        public void SaveQQMailText(byte[] buffer, int nbytes, string filePath)
        {
            FileStream stream = null;
            try
            {
                if (filePath == null)
                {
                    filePath = "";
                }
                string str = DateTime.Now.Date.ToString("yyy-MM-dd");
                filePath = filePath + str + "邮件";
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                filePath = filePath + @"\" + base.m_snote;
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                filePath = filePath + @"\" + base.m_stype;
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                string str2 = filePath;
                filePath = str2 + @"\" + base.m_username + "@" + base.m_serv;
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                filePath = filePath + @"\" + base.BoxName;
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                filePath = filePath + @"\" + this.m_emailTime + ".eml";
                stream = File.Open(filePath, FileMode.Append, FileAccess.Write);
                stream.Write(buffer, 0, nbytes);
                stream.Close();
            }
            catch (Exception exception)
            {
                base.ShowMessage("保存" + exception.Message);
            }
            finally
            {
                stream.Close();
            }

        }

        // Nested Types
        [StructLayout(LayoutKind.Sequential)]
        private struct BoxNameID
        {
            public string boxname;
            public string boxurl;
        }

    }
}
