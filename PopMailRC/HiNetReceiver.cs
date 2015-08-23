using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Threading;

namespace MailExposure.PopMailRC
{
    class HiNetReceiver:MailStr
    {
        // Fields
        private CookieContainer cookieContainer;
        public string m_emailTime;
        private int page;
        private string urls;

        // Methods
        public HiNetReceiver()
        {
            this.urls = "";
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
                        if (Convert.ToInt32(GlobalValue.PopMainForm.PageNumber) != 0)
                        {
                            if (this.page >= Convert.ToInt32(GlobalValue.PopMainForm.PageNumber))
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
                    }
                    num3++;
                }
            }
        Label_03B1:
            num2++;
            startIndex = message.IndexOf("<a href=\"/mailService/mail/M_main_1.do?next_page=menu_page&function_name", num2);
            if (startIndex < 0)
            {
                string url = base.Host + "/mailService/logout.do";
                base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                base.streamControl = true;
                base.MyStringBuilder = this.Request(url);
                base.ShowMessage("退出!");
            }
            else
            {
                str = base.putstr(message, "\"", "\"", startIndex);
                if (str == "-1")
                {
                    startIndex++;
                }
                else
                {
                    base.BoxName = base.putstr(message, "\">", "</a>", startIndex).Trim();
                    if (base.BoxName == "-1")
                    {
                        startIndex++;
                    }
                    else
                    {
                        base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                        base.streamControl = true;
                        base.MyStringBuilder = this.Request(base.Host + str);
                        this.page = 0;
                        num3 = 0;
                        this.page++;
                        if (Convert.ToInt32(GlobalValue.PopMainForm.PageNumber) == 0)
                        {
                            base.ShowMessage(string.Concat(new object[] { base.BoxName, "：第", this.page, "页开始下载" }));
                            this.getHiNetID(base.MyStringBuilder.ToString());
                            base.ShowMessage(string.Concat(new object[] { base.BoxName, "：第", this.page, "页下载完毕" }));
                        }
                        else if (this.page <= Convert.ToInt32(GlobalValue.PopMainForm.PageNumber))
                        {
                            base.ShowMessage(string.Concat(new object[] { base.BoxName, "：第", this.page, "页开始下载" }));
                            this.getHiNetID(base.MyStringBuilder.ToString());
                            base.ShowMessage(string.Concat(new object[] { base.BoxName, "：第", this.page, "页下载完毕" }));
                        }
                        else
                        {
                            return;
                        }
                        numArray = new int[100];
                        try
                        {
                            while ((num3 = base.MyStringBuilder.ToString().IndexOf(":go_page(", num3)) > 0)
                            {
                                string str5 = base.putstr(base.MyStringBuilder.ToString(), ":go_page(", ")", num3).Trim();
                                if (str5 == "-1")
                                {
                                    num3++;
                                }
                                else
                                {
                                    numArray[this.page] = Convert.ToInt32(str5);
                                    if (numArray[this.page] > numArray[this.page - 1])
                                    {
                                        base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                                        base.streamControl = true;
                                        base.MyStringBuilder = this.Request(string.Concat(new object[] { base.Host, str, "&start=", numArray[this.page] }));
                                        this.page++;
                                        if (Convert.ToInt32(GlobalValue.PopMainForm.PageNumber) == 0)
                                        {
                                            base.ShowMessage(string.Concat(new object[] { base.BoxName, "：第", this.page, "页开始下载" }));
                                            this.getHiNetID(base.MyStringBuilder.ToString());
                                            base.ShowMessage(string.Concat(new object[] { base.BoxName, "：第", this.page, "页下载完毕" }));
                                        }
                                        else if (this.page < Convert.ToInt32(GlobalValue.PopMainForm.PageNumber))
                                        {
                                            base.ShowMessage(string.Concat(new object[] { base.BoxName, "：第", this.page, "页开始下载" }));
                                            this.getHiNetID(base.MyStringBuilder.ToString());
                                            base.ShowMessage(string.Concat(new object[] { base.BoxName, "：第", this.page, "页下载完毕" }));
                                        }
                                        else
                                        {
                                            return;
                                        }
                                        num3 = 0;
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
                    }
                }
                goto Label_03B1;
            }

        }
        private void getHiNetID(string message)
        {
            int num = 0;
            int startIndex = 0;
            string str2 = "";
            string str4 = base.Host + "/mailService/sendAttach.do?pid=-1&msg=";
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
                    if (Convert.ToInt32(GlobalValue.PopMainForm.ExecuteSQL(strSql)) == 0)
                    {
                        int num5 = message.IndexOf("name=\"blacklist\"", index);
                        if (num5 > 0)
                        {
                            base.putstr(message, "value=\"", "\"", num5).Trim();
                        }
                        num5 = message.IndexOf("<a href=\"javascript:mail_read", index);
                        if (num5 > 0)
                        {
                            str2 = base.putstr(message, "\">", "</a>", num5).Trim();
                            if (str2.LastIndexOf("\r\n") != -1)
                            {
                                str2 = str2.Substring(str2.LastIndexOf("\r\n") + 4, str2.Length - (str2.LastIndexOf("\r\n") + 4)).Trim();
                            }
                        }
                        string str3 = base.putstr(message, "<td height=\"24\" align=\"right\" nowrap >", "</td>", index).Trim();
                        bool flag = false;
                        try
                        {
                            DateTime time = new DateTime();
                            time = Convert.ToDateTime(DateTime.Now.Year.ToString() + "/" + str3);
                            if (DateTime.Compare(time, GlobalValue.PopMainForm.EmailDateTime) >= 0)
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
                        if (flag)
                        {
                            string str6 = DateTime.Now.ToString();
                            base.streamControl = true;
                            string[] strArray = new string[] { base.BoxName, "第", (num + 1).ToString("00"), "封:", str2, "\t下载" };
                            base.ShowMessage(string.Concat(strArray));
                            if (this.RequestEmail(str4 + str))
                            {
                                try
                                {
                                    strSql = "insert into HiNetId(Name,MsgId,DownTime,MailType) values('" + base.m_username + "','" + str + "','" + str6 + "','" + base.BoxName + "')";
                                    GlobalValue.PopMainForm.ExecuteSQL(strSql);
                                    str = "";
                                }
                                catch (Exception exception)
                                {
                                    base.ShowMessage("添加失败：" + exception.Message);
                                }
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
            base.m_passwd = base.strPassParse(base.m_passwd);
            try
            {
                base.ShowMessage("开始登陆…………");
                string url = "http://www.webmail.hinet.net/login.do?usertype=1&mailid=" + base.m_username + "&password=" + base.m_passwd + "&OK.x=15&OK.y=12";
                this.cookieContainer = new CookieContainer();
                base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                base.streamControl = true;
                base.MyStringBuilder = this.Request(url);
                if (base.MyStringBuilder.ToString().IndexOf("/mailService/mail/") > 0)
                {
                    base.Host = this.urls.Substring(0, this.urls.IndexOf("/mailService/mail/"));
                    url = base.Host + "/mailService/mail/M_main_8.jsp";
                    base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                    base.streamControl = true;
                    base.MyStringBuilder = this.Request(url);
                    this.DownMail(base.MyStringBuilder.ToString());
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
                    this.DownMail(base.MyStringBuilder.ToString());
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
            GlobalValue.PopMainForm.statusBarPanelEmailNumbersText.Text = (Convert.ToInt32(GlobalValue.PopMainForm.statusBarPanelEmailNumbersText.Text) + 1).ToString();
            base.streamControl = false;
            return true;

        }
        private bool SaveHiNet(byte[] buffer, int nbytes)
        {
            try
            {
                string filePath = "";
                if ((GlobalValue.PopMainForm.saveFilePath != "") | (GlobalValue.PopMainForm.saveFilePath != null))
                {
                    filePath = GlobalValue.PopMainForm.saveFilePath;
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
                string str3 = filePath;
                filePath = str3 + @"\" + base.m_username + "@" + base.m_serv;
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
