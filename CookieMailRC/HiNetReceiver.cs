using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Threading;

namespace MailExposure.CookieMailRC
{
    class HiNetReceiver:MailStr
    {
        // Fields
        private CookieContainer cookieContainer;
        public string m_emailTime;
        private int page;
        private string url;

        // Methods
        public HiNetReceiver()
        {
            this.url = "";
        }
        private void DownMail(string message)
        {
            int startIndex = 0;
            int num2 = 0;
            int num3 = 0;
            startIndex = message.IndexOf("<a href=\"/mailService/mail/M_main_1.do?next_page=menu_page&function_name=receive");
            if (startIndex < 0)
            {
                base.LoginFail();
                base.passwdErr();
                base.ShowMessage("登陆失败…… 无法进入");
                return;
            }
            base.ShowMessage("登陆成功");
            do
            {
                num2 = startIndex + 1;
                startIndex = message.IndexOf("<a href=\"/mailService/mail/M_main_1.do?next_page=menu_page&function_name=receive", num2);
            }
            while (startIndex >= 0);
            this.page = 0;
            string str = base.putstr(message, "\"", "\"", num2);
            base.BoxName = base.putstr(message, "\">", "</a>", num2).Trim();
            base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
            base.streamControl = true;
            base.MyStringBuilder = this.Request(base.Host + str);
            this.page++;
            base.ShowMessage(string.Concat(new object[] { base.BoxName, "：第", this.page, "页开始下载" }));
            this.getHiNetID(base.MyStringBuilder.ToString());
            base.ShowMessage(string.Concat(new object[] { base.BoxName, "：第", this.page, "页下载完毕" }));
            int[] numArray = new int[100];
            string str2 = base.MyStringBuilder.ToString();
            while ((num3 = str2.IndexOf(":go_page(", num3)) > 0)
            {
                string str3 = base.putstr(str2, ":go_page(", ")", num3).Trim();
                if (str3 == "-1")
                {
                    num3++;
                }
                else
                {
                    numArray[this.page] = Convert.ToInt32(str3);
                    if (numArray[this.page] > numArray[this.page - 1])
                    {
                        base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                        base.streamControl = true;
                        base.MyStringBuilder = this.Request(string.Concat(new object[] { base.Host, str, "&start=", numArray[this.page] }));
                        this.page++;
                        if (Convert.ToInt32(GlobalValue.mainForm.PageNumber) != 0)
                        {
                            if (this.page >= Convert.ToInt32(GlobalValue.mainForm.PageNumber))
                            {
                                return;
                            }
                            base.ShowMessage(string.Concat(new object[] { base.BoxName, "：第", this.page, "页开始下载" }));
                            this.getHiNetID(base.MyStringBuilder.ToString());
                            base.ShowMessage(string.Concat(new object[] { base.BoxName, "：第", this.page, "页下载完毕" }));
                        }
                        else
                        {
                            base.ShowMessage(string.Concat(new object[] { base.BoxName, "：第", this.page, "页开始下载" }));
                            this.getHiNetID(base.MyStringBuilder.ToString());
                            base.ShowMessage(string.Concat(new object[] { base.BoxName, "：第", this.page, "页下载完毕" }));
                        }
                        continue;
                    }
                    num3++;
                }
            }
        Label_03B1:
            num2++;
            startIndex = message.IndexOf("<a href=\"/mailService/mail/M_main_1.do?next_page=menu_page&function_name", num2);
            if (startIndex >= 0)
            {
                str = base.putstr(message, "\"", "\"", startIndex);
                if (str == "-1")
                {
                    startIndex++;
                    goto Label_03B1;
                }
                base.BoxName = base.putstr(message, "\">", "</a>", startIndex).Trim();
                if (base.BoxName == "-1")
                {
                    startIndex++;
                    goto Label_03B1;
                }
                base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                base.streamControl = true;
                base.MyStringBuilder = this.Request(base.Host + str);
                this.page = 0;
                num3 = 0;
                this.page++;
                if (Convert.ToInt32(GlobalValue.mainForm.PageNumber) == 0)
                {
                    base.ShowMessage(string.Concat(new object[] { base.BoxName, "：第", this.page, "页开始下载" }));
                    this.getHiNetID(base.MyStringBuilder.ToString());
                    base.ShowMessage(string.Concat(new object[] { base.BoxName, "：第", this.page, "页下载完毕" }));
                    goto Label_05CB;
                }
                if (this.page <= Convert.ToInt32(GlobalValue.mainForm.PageNumber))
                {
                    base.ShowMessage(string.Concat(new object[] { base.BoxName, "：第", this.page, "页开始下载" }));
                    this.getHiNetID(base.MyStringBuilder.ToString());
                    base.ShowMessage(string.Concat(new object[] { base.BoxName, "：第", this.page, "页下载完毕" }));
                    goto Label_05CB;
                }
            }
            return;
        Label_05CB:
            numArray = new int[100];
            try
            {
                while ((num3 = base.MyStringBuilder.ToString().IndexOf(":go_page(", num3)) > 0)
                {
                    string str4 = base.putstr(base.MyStringBuilder.ToString(), ":go_page(", ")", num3).Trim();
                    if (str4 == "-1")
                    {
                        num3++;
                    }
                    else
                    {
                        numArray[this.page] = Convert.ToInt32(str4);
                        if (numArray[this.page] > numArray[this.page - 1])
                        {
                            base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                            base.streamControl = true;
                            base.MyStringBuilder = this.Request(string.Concat(new object[] { base.Host, str, "&start=", numArray[this.page] }));
                            this.page++;
                            if (Convert.ToInt32(GlobalValue.mainForm.PageNumber) != 0)
                            {
                                if (this.page >= Convert.ToInt32(GlobalValue.mainForm.PageNumber))
                                {
                                    return;
                                }
                                base.ShowMessage(string.Concat(new object[] { base.BoxName, "：第", this.page, "页开始下载" }));
                                this.getHiNetID(base.MyStringBuilder.ToString());
                                base.ShowMessage(string.Concat(new object[] { base.BoxName, "：第", this.page, "页下载完毕" }));
                            }
                            else
                            {
                                base.ShowMessage(string.Concat(new object[] { base.BoxName, "：第", this.page, "页开始下载" }));
                                this.getHiNetID(base.MyStringBuilder.ToString());
                                base.ShowMessage(string.Concat(new object[] { base.BoxName, "：第", this.page, "页下载完毕" }));
                            }
                            continue;
                        }
                        num3++;
                    }
                }
            }
            catch (Exception exception)
            {
                base.ShowMessage("下载失败：" + exception.Message + exception.StackTrace);
            }
            num2 = startIndex;
            goto Label_03B1;

        }
        private void getHinetCookie(string strCookie)
        {
            string str = "";
            strCookie = " " + strCookie + "; ";
            str = base.putstr(strCookie, " JSESSIONID=", ";", 0);
            if (str != "-1")
            {
                Cookie cookie = new Cookie("JSESSIONID", str);
                cookie.Domain = ".webmail.hinet.net";
                this.cookieContainer.Add(cookie);
            }
            str = base.putstr(strCookie, " Latest=", ";", 0);
            if (str != "-1")
            {
                Cookie cookie2 = new Cookie("Latest", str);
                cookie2.Domain = ".webmail.hinet.net";
                this.cookieContainer.Add(cookie2);
            }
            str = base.putstr(strCookie, " U=", ";", 0);
            if (str != "-1")
            {
                Cookie cookie3 = new Cookie("U", str);
                cookie3.Domain = ".webmail.hinet.net";
                this.cookieContainer.Add(cookie3);
            }
            str = base.putstr(strCookie, " S=", ";", 0);
            if (str != "-1")
            {
                Cookie cookie4 = new Cookie("S", str);
                cookie4.Domain = ".webmail.hinet.net";
                this.cookieContainer.Add(cookie4);
            }
            str = base.putstr(strCookie, " T=", ";", 0);
            if (str != "-1")
            {
                Cookie cookie5 = new Cookie("T", str);
                cookie5.Domain = ".webmail.hinet.net";
                this.cookieContainer.Add(cookie5);
            }
            str = base.putstr(strCookie, " N=", ";", 0);
            if (str != "-1")
            {
                Cookie cookie6 = new Cookie("N", str);
                cookie6.Domain = ".webmail.hinet.net";
                this.cookieContainer.Add(cookie6);
            }
            str = base.putstr(strCookie, " D=", ";", 0);
            if (str != "-1")
            {
                Cookie cookie7 = new Cookie("D", str);
                cookie7.Domain = ".webmail.hinet.net";
                this.cookieContainer.Add(cookie7);
            }

        }
        private void getHiNetID(string message)
        {
            int num = 0;
            int startIndex = 0;
            string str2 = base.Host + "/mailService/sendAttach.do?pid=-1&msg=";
            while (true)
            {
                int index = message.IndexOf("name=\"mailid\"", startIndex);
                if (index < 0)
                {
                    return;
                }
                string str = base.putstr(message, "value=\"", "\"", index);
                if (str == "-1")
                {
                    index++;
                }
                else
                {
                    string strSql = "select count(*) from HiNetId where MsgId='" + str + "'";
                    if (Convert.ToInt32(GlobalValue.mainForm.ExecuteSQL(strSql)) == 0)
                    {
                        string str4 = DateTime.Now.ToString();
                        base.streamControl = true;
                        string[] strArray = new string[] { base.BoxName, "第", (num + 1).ToString("00"), "封:", str, " 开始下载" };
                        base.ShowMessage(string.Concat(strArray));
                        if (this.RequestEmail(str2 + str))
                        {
                            string[] strArray2 = new string[] { base.BoxName, "第", (num + 1).ToString("00"), "封:", str, " 下载完成" };
                            base.ShowMessage(string.Concat(strArray2));
                            try
                            {
                                strSql = "insert into HiNetId(Name,MsgId,DownTime,MailType) values('" + base.m_username + "','" + str + "','" + str4 + "','" + base.BoxName + "')";
                                GlobalValue.mainForm.ExecuteSQL(strSql);
                                str = "";
                            }
                            catch (Exception exception)
                            {
                                base.ShowMessage("添加失败：" + exception.Message);
                            }
                        }
                    }
                    startIndex = index + 1;
                    num++;
                }
            }

        }
        public override void login()
        {
            if (base.m_UserType.IndexOf("无密用户") != -1)
            {
                this.cookieContainer = new CookieContainer();
                base.ShowMessage("开始登陆…………");
                base.cookie = base.validationLogin;
                this.getHinetCookie(base.cookie);
                this.url = base.emailuri;
                base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                base.streamControl = true;
                base.MyStringBuilder = this.Request(this.url);
                if (base.MyStringBuilder.ToString().IndexOf("/mailService/mail/") > 0)
                {
                    base.Host = base.putstr(base.MyStringBuilder.ToString(), "<BASE HREF=\"", "/mailService", 0);
                    if (base.Host != "-1")
                    {
                        this.url = base.Host + "/mailService/mail/M_main_8.jsp";
                        base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                        base.streamControl = true;
                        base.MyStringBuilder = this.Request(this.url);
                        this.DownMail(base.MyStringBuilder.ToString());
                    }
                    else
                    {
                        base.LoginFail();
                        base.passwdErr();
                        base.ShowMessage("登陆失败.无法进入");
                    }
                }
                else
                {
                    if (base.MyStringBuilder.ToString().IndexOf("(01406)") != -1)
                    {
                        base.ShowMessage("您已經執行登出或連線逾時，請重新登入。");
                    }
                    else if (base.MyStringBuilder.ToString().IndexOf("(01604)") != -1)
                    {
                        base.ShowMessage("由於網頁郵件服務不提供同一帳號、同時間在不同視窗登入，若有重複登入、不正常登出(直接關閉視窗或是因系統錯誤而離開)之情形，請重新登入");
                    }
                    base.LoginFail();
                    base.passwdErr();
                    base.ShowMessage("登陆失败…………无法进入");
                }
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
                    string str2 = "";
                    str2 = headers["location"];
                    if (str2.IndexOf("http") != -1)
                    {
                        this.Request(str2);
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
            }
            catch (Exception)
            {
            }
            return base.MyStringBuilder;

        }
        private bool RequestEmail(string url)
        {
            int num;
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
            Thread.Sleep(10);
            Stream responseStream = response.GetResponseStream();
            if (!base.streamControl)
            {
                return false;
            }
            byte[] buffer = new byte[0x1000];
            base.m_emailno++;
            this.m_emailTime = DateTime.Now.ToString("yyyMMdd-HHmmss") + DateTime.Now.Millisecond.ToString();
            while ((num = responseStream.Read(buffer, 0, 0x1000)) > 0)
            {
                if (!this.SaveHiNet(buffer, num))
                {
                    return false;
                }
            }
            base.saveEmailCount();
            GlobalValue.mainForm.statusBarPanelEmailNumbersText.Text = (Convert.ToInt32(GlobalValue.mainForm.statusBarPanelEmailNumbersText.Text) + 1).ToString();
            base.streamControl = false;
            return true;
        }

        private bool SaveHiNet(byte[] buffer, int nbytes)
        {
            try
            {
                string filePath = "";
                if ((GlobalValue.mainForm.saveFilePath != "") | (GlobalValue.mainForm.saveFilePath != null))
                {
                    filePath = GlobalValue.mainForm.saveFilePath;
                }
                this.SaveHiNetText(buffer, nbytes, filePath);
            }
            catch (Exception exception)
            {
                base.ShowMessage("保存邮件失败：" + exception.Message);
                return false;
            }
            return true;

        }
        public void SaveHiNetText(byte[] buffer, int nbytes, string filePath)
        {
            FileStream stream = null;
            try
            {
                string str = ".eml";
                string str2 = DateTime.Now.Date.ToString("yyy-MM-dd");
                filePath = filePath + str2 + "邮件";
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                filePath = filePath + @"\" + base.m_userListName;
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                if (base.m_snote != "capHninet")
                {
                    string str3 = filePath;
                    filePath = str3 + @"\" + base.m_username + "(" + base.m_snote + ")";
                }
                else
                {
                    filePath = filePath + @"\" + base.m_username;
                }
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                filePath = filePath + @"\" + base.BoxName;
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                filePath = filePath + @"\" + this.m_emailTime + str;
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
    }
}
