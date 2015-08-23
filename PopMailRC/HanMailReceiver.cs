using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading;
using System.IO;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Runtime.InteropServices;

namespace MailExposure.PopMailRC
{
    class HanMailReceiver:MailStr
    {
        // Fields
        private BoxNameID[] boxList;
        private string checkTime;
        private CookieContainer cookieContainer;
        public string m_emailTime;
        private static object SelMailBoxL;
        private string tempTime;
        private string url;
        private string urls;

        // Methods
        static HanMailReceiver()
        {
            SelMailBoxL = new object();
        }
        public HanMailReceiver()
        {
            this.url = "";
            this.urls = "";
            this.boxList = new BoxNameID[100];
            this.cookieContainer = new CookieContainer();

        }
        private string base64decode(string str)
        {
            int[] numArray = new int[] { 
                -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, 
                -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, 
                -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, 0x3e, -1, -1, -1, 0x3f, 
                0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3a, 0x3b, 60, 0x3d, -1, -1, -1, -1, -1, -1, 
                -1, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 
                15, 0x10, 0x11, 0x12, 0x13, 20, 0x15, 0x16, 0x17, 0x18, 0x19, -1, -1, -1, -1, -1, 
                -1, 0x1a, 0x1b, 0x1c, 0x1d, 30, 0x1f, 0x20, 0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 40, 
                0x29, 0x2a, 0x2b, 0x2c, 0x2d, 0x2e, 0x2f, 0x30, 0x31, 50, 0x33, -1, -1, -1, -1, -1
            };
            int length = str.Length;
            int num5 = 0;
            string str2 = "";
            while (num5 < length)
            {
                int num;
                int num2;
                int num3;
                int num4;
                do
                {
                    num = numArray[str[num5++] & '\x00ff'];
                }
                while ((num5 < length) && (num == -1));
                if (num == -1)
                {
                    return str2;
                }
                do
                {
                    num2 = numArray[str[num5++] & '\x00ff'];
                }
                while ((num5 < length) && (num2 == -1));
                if (num2 == -1)
                {
                    return str2;
                }
                str2 = str2 + ((char)((num << 2) | ((num2 & 0x30) >> 4)));
                do
                {
                    num3 = str[num5++] & '\x00ff';
                    if (num3 == 0x3d)
                    {
                        return str2;
                    }
                    num3 = numArray[num3];
                }
                while ((num5 < length) && (num3 == -1));
                if (num3 == -1)
                {
                    return str2;
                }
                str2 = str2 + ((char)(((num2 & 15) << 4) | ((num3 & 60) >> 2)));
                do
                {
                    num4 = str[num5++] & '\x00ff';
                    if (num4 == 0x3d)
                    {
                        return str2;
                    }
                    num4 = numArray[num4];
                }
                while ((num5 < length) && (num4 == -1));
                if (num4 == -1)
                {
                    return str2;
                }
                str2 = str2 + ((char)(((num3 & 3) << 6) | num4));
            }
            return str2;

        }
        public void DownEmal(string maiUrl, string mailId, string mailSubject, string mailDate)
        {
            string str = mailId + Convert.ToString(mailId.GetHashCode(), 0x10);
            try
            {
                if ((mailId != "-1") && (mailId != ""))
                {
                    string strSql = "select count(*) from HanMailId where MsgId='" + str + "'";
                    if (Convert.ToInt32(GlobalValue.PopMainForm.ExecuteSQL(strSql)) == 0)
                    {
                        string indata = "callCount=1\npage=/hanmailex/Top.daum?dummy=-64225891\nhttpSessionId=\nscriptSessionId=361D8A41157BE69CA42EEA39BA104BDA384\nc0-scriptName=MailService\nc0-methodName=getDownloadURL\nc0-id=0\nc0-param0=string:" + mailId + "\nbatchId=4";
                        base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                        base.streamControl = true;
                        base.MyStringBuilder = this.PostData(maiUrl, indata);
                        if (base.MyStringBuilder.ToString().IndexOf("dwr.engine._remoteHandleCallback") > 0)
                        {
                            maiUrl = base.putstr(base.MyStringBuilder.ToString(), "\"", "\"", 0);
                        }
                        else
                        {
                            base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                            base.streamControl = true;
                            base.MyStringBuilder = this.PostData(maiUrl, indata);
                            if (base.MyStringBuilder.ToString().IndexOf("dwr.engine._remoteHandleCallback") > 0)
                            {
                                maiUrl = base.putstr(base.MyStringBuilder.ToString(), "\"", "\"", 0);
                            }
                            else
                            {
                                base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                                base.streamControl = true;
                                base.MyStringBuilder = this.PostData(maiUrl, indata);
                                if (base.MyStringBuilder.ToString().IndexOf("dwr.engine._remoteHandleCallback") > 0)
                                {
                                    maiUrl = base.putstr(base.MyStringBuilder.ToString(), "\"", "\"", 0);
                                }
                            }
                        }
                        DateTime time = new DateTime();
                        time = Convert.ToDateTime(mailDate);
                        this.m_emailTime = DateTime.Now.ToString("yyyMMdd-HHmmss") + DateTime.Now.Millisecond.ToString();
                        bool flag = false;
                        try
                        {
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
                            base.streamControl = true;
                            this.RequestEmail(maiUrl);
                            try
                            {
                                string str4 = DateTime.Now.ToString();
                                strSql = "insert into HanMailId(Name,MsgId,DownTime,MailType) values('" + base.m_username + "','" + str + "','" + str4 + "','" + base.BoxName.Substring(0, base.BoxName.Length % 10) + "')";
                                GlobalValue.PopMainForm.ExecuteSQL(strSql);
                            }
                            catch (Exception exception)
                            {
                                base.ShowMessage("添加失败：" + exception.Message);
                            }
                            base.ShowMessage(base.BoxName + ":" + this.U2CnCode(mailSubject) + "下载完成");
                        }
                    }
                }
                else
                {
                    base.ShowMessage(this.U2CnCode(mailSubject) + ":邮件下载错误");
                }
            }
            catch (Exception)
            {
                base.ShowMessage("下载邮件失败！" + mailId);
            }

        }
        public void GetAddressBook()
        {
            try
            {
                string url = base.Host + "/hanmail/Index.daum?frame=addr";
                base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                base.streamControl = true;
                base.MyStringBuilder = this.Request(url);
                string message = base.MyStringBuilder.ToString();
                if (message.IndexOf("var dataSeq = '") > 0)
                {
                    message = base.putstr(message, "var dataSeq = '", "'", 0);
                }
                string str3 = this.base64decode(message);
                this.url = "http://addrbook.daum.net/aplus/xmlhttp/getExpressCsvFile.do?dataSeq=" + str3;
                base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                base.streamControl = true;
                base.MyStringBuilder = this.Request(this.url);
                this.SaveAddressbook(base.MyStringBuilder.ToString());
                base.ShowMessage("地址薄\t下载");
            }
            catch (Exception exception)
            {
                base.ShowMessage("保存地址薄失败" + exception.Message);
            }

        }
        private void getBoxName(string strBoxs)
        {
            this.GetAddressBook();
            int index = 0;
            string pattern = "<td.*?>([^>]+)?<input.*?name=\"D\\d\".*?value=\"(?<boxname>[^\"]+)\".*?>[\\s\\S]+?</td>";
            Regex regex = new Regex(pattern);
            string input = strBoxs;
            Match match = regex.Match(input);
            int num2 = 0;
            while (match.Success)
            {
                this.boxList[num2++].boxname = match.Groups["boxname"].Value;
                match = match.NextMatch();
            }
            int num3 = num2;
            index = 0;
            for (int i = 0; i < num3; i++)
            {
                this.getEmailId(base.Host, this.boxList[i].boxname);
            }
            lock (SelMailBoxL)
            {
                for (index = 0; index < num2; index++)
                {
                    string strSql = string.Concat(new object[] { "select count(*) from MailBoxList where 序号='", base.m_NO, "' and MailBoxName = '", this.boxList[index].boxname, "'" });
                    if (Convert.ToInt32(GlobalValue.PopMainForm.ExecuteSQL(strSql)) == 0)
                    {
                        strSql = string.Concat(new object[] { "insert into MailBoxList (序号,MailBoxName)values('", base.m_NO, "','", this.boxList[index].boxname, "');" });
                        GlobalValue.PopMainForm.ExecuteSQL(strSql);
                    }
                }
            }

        }
        private void getEmailId(string urlbox, string boxName)
        {
            string maiUrl = "";
            string mailSubject = "";
            string mailDate = "";
            try
            {
                byte[] bytes = Encoding.GetEncoding("utf-8").GetBytes(boxName);
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    char character = Convert.ToChar(int.Parse(bytes[i].ToString()));
                    builder.Append(Uri.HexEscape(character));
                }
                base.Host = "http://hmail2.daum.net";
                base.BoxName = boxName;
                this.url = base.Host + "/hanmailex/js/ajax/call/plaincall/FolderService.getMailList.dwr";
                string str5 = new Regex(@"JSESSIONID=(?<SessionID>[\w-^;]+)").Match(base.cookie).Groups["SessionID"].Value;
                object[] objArray = new object[7];
                objArray[0] = "callCount=1\npage=/hanmailex/Top.daum?\nhttpSessionId=";
                objArray[1] = str5;
                objArray[2] = "\nscriptSessionId=361D8A41157BE69CA42EEA39BA104BDA";
                double num4 = new Random().NextDouble() * 1000.0;
                objArray[3] = num4.ToString().Substring(0, 3).ToUpper();
                objArray[4] = "\nc0-scriptName=FolderService\nc0-methodName=getMailList\nc0-id=0\nc0-param0=string:";
                objArray[5] = builder;
                objArray[6] = "\nc0-param1=number:0\nc0-param2=number:30\nbatchId=2";
                string indata = string.Concat(objArray);
                base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                base.streamControl = true;
                base.MyStringBuilder = this.PostData(this.url, indata);
                string message = base.MyStringBuilder.ToString();
                int num2 = Convert.ToInt32(base.putstr(message, "totalCount=", ";", 0));
                if (-1 == num2)
                {
                    base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                    base.streamControl = true;
                    base.MyStringBuilder = this.PostData(this.url, indata);
                    message = base.MyStringBuilder.ToString();
                    num2 = Convert.ToInt32(base.putstr(message, "totalCount=", ";", 0));
                }
                if (-1 == num2)
                {
                    base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                    base.streamControl = true;
                    base.MyStringBuilder = this.PostData(this.url, indata);
                    message = base.MyStringBuilder.ToString();
                    num2 = Convert.ToInt32(base.putstr(message, "totalCount=", ";", 0));
                }
                if (num2 == 0)
                {
                    base.ShowMessage(base.BoxName + "是空的。");
                }
                else
                {
                    if (num2 > 15)
                    {
                        indata = string.Concat(new object[] { "callCount=1\npage=/hanmailex/Top.daum?\nhttpSessionId=\nscriptSessionId=361D8A41157BE69CA42EEA39BA104BDA769\nc0-scriptName=FolderService\nc0-methodName=getMailList\nc0-id=0\nc0-param0=string:", builder, "\nc0-param1=number:0\nc0-param2=number:", num2, "\nbatchId=79" });
                        base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                        base.streamControl = true;
                        base.MyStringBuilder = this.PostData(this.url, indata);
                    }
                    message = base.MyStringBuilder.ToString();
                    int startIndex = 0;
                    while ((startIndex = message.IndexOf("alreadyAttach", startIndex)) > 0)
                    {
                        string str8 = base.putstr(message, ";", ".", startIndex);
                        base.ShowMessage(str8);
                        mailDate = base.putstr(message, "dateStr=\"", "\"", startIndex);
                        base.putstr(message, "escapedFromAddr=\"", "\"", startIndex);
                        string mailId = base.putstr(message, "mailId=\"", "\"", startIndex);
                        mailSubject = base.putstr(message, ".subject=\"", "\"", startIndex);
                        startIndex++;
                        maiUrl = "http://hmail2.daum.net/hanmailex/js/ajax/call/plaincall/MailService.getDownloadURL.dwr";
                        this.DownEmal(maiUrl, mailId, mailSubject, mailDate);
                    }
                }
            }
            catch (Exception)
            {
                base.ShowMessage(base.BoxName + "取邮件ID错误。");
            }

        }
        public string getGNlZtime()
        {
            return Convert.ToInt64(DateTime.Now.Subtract(new DateTime(0x7b2, 1, 2, 8, 0, 0)).TotalMilliseconds).ToString();
        }
        public override void login()
        {
            base.cookie = "";
            int startIndex = 0;
            base.DecodeBase64("euc-kr", "361D8A41157BE69CA42EEA39BA104BDA344=");
            string indata = "url=http%3A%2F%2Fwww.daum.net%2F%3Ft__nil_top%3Dlogin&pw=" + base.m_passwd + "&id=" + base.m_username + "&enpw=" + base.m_passwd;
            this.tempTime = this.getGNlZtime();
            this.url = "https://logins.daum.net/Mail-bin/login.cgi?dummy=" + this.tempTime;
            base.ShowMessage("开始登陆…………");
            base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
            base.streamControl = true;
            base.MyStringBuilder = this.PostData(this.url, indata);
            startIndex = base.MyStringBuilder.ToString().IndexOf("document.location.replace");
            if (startIndex != -1)
            {
                this.url = base.putstr(base.MyStringBuilder.ToString(), "'", "'", startIndex);
                base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                base.streamControl = true;
                base.MyStringBuilder = this.Request(this.url);
                if (base.MyStringBuilder.ToString() != "")
                {
                    string message = base.MyStringBuilder.ToString();
                    startIndex = 0;
                    startIndex = message.IndexOf("id=\"logonForm\"");
                    this.url = base.putstr(message, "href=\"", "\"", startIndex);
                    if ((this.url != "-1") && (this.url != ""))
                    {
                        this.url = "http://hmail2.daum.net/hanmailex/Top.daum?";
                        base.Host = "http://hmail2.daum.net";
                        base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                        base.streamControl = true;
                        base.MyStringBuilder = this.Request(this.url);
                        this.url = base.Host + "/hanmail/mail/FolderManage.daum?_top_hm=l_fol";
                        base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                        base.streamControl = true;
                        base.MyStringBuilder = this.Request(this.url);
                        base.ShowMessage("登陆成功…………");
                        this.getBoxName(base.MyStringBuilder.ToString());
                    }
                    else
                    {
                        base.ShowMessage("登陆失败…………");
                    }
                }
            }
            else
            {
                base.ShowMessage("密码错误或登陆跳转失败…………");
            }
        }
        public void loginold()
        {
            try
            {
                base.cookie = "";
                int startIndex = 0;
                base.DecodeBase64("euc-kr", "361D8A41157BE69CA42EEA39BA104BDA344=");
                string indata = "url=http%3A%2F%2Fwww.daum.net%2F%3Ft__nil_top%3Dlogin&pw=" + base.m_passwd + "&id=" + base.m_username + "&enpw=" + base.m_passwd;
                this.tempTime = this.getGNlZtime();
                this.url = "https://logins.daum.net/Mail-bin/login.cgi?dummy=" + this.tempTime;
                base.ShowMessage("开始登陆…………");
                base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                base.streamControl = true;
                base.MyStringBuilder = this.PostData(this.url, indata);
                startIndex = base.MyStringBuilder.ToString().IndexOf("입력하신 아이디 혹은 비밀번호가 일치하지 않습니다");
                if (startIndex == -1)
                {
                    startIndex = base.MyStringBuilder.ToString().IndexOf("document.location.replace");
                    if (startIndex != -1)
                    {
                        this.url = base.putstr(base.MyStringBuilder.ToString(), "'", "'", startIndex);
                        base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                        base.streamControl = true;
                        base.MyStringBuilder = this.Request(this.url);
                        if (base.MyStringBuilder.ToString() != "")
                        {
                            base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                            base.streamControl = true;
                            base.MyStringBuilder = this.Request("http://mail2.daum.net/hanmail/Goto.daum");
                            this.url = base.putstr(base.MyStringBuilder.ToString(), "top.location.href=\"", "\"", 0);
                            base.Host = this.url.Substring(0, this.url.LastIndexOf("/") + 1);
                            if ((this.url != "-1") && (this.url != ""))
                            {
                                base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                                base.streamControl = true;
                                base.MyStringBuilder = this.Request(this.url);
                                base.ShowMessage("登陆成功!");
                                this.getBoxName(base.MyStringBuilder.ToString());
                            }
                            else
                            {
                                this.url = " http://www.daum.net/?t__nil_top=login";
                                base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                                base.streamControl = true;
                                base.MyStringBuilder = this.Request(this.url);
                                string message = base.MyStringBuilder.ToString();
                                startIndex = 0;
                                startIndex = message.IndexOf("<a class=\"mail ir");
                                this.url = base.putstr(message, "href=\"", "\"", startIndex);
                                if ((this.url != "-1") && (this.url != ""))
                                {
                                    base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                                    base.streamControl = true;
                                    base.MyStringBuilder = this.Request(this.url);
                                    base.Host = this.url.Substring(0, this.url.LastIndexOf("/"));
                                    if (base.MyStringBuilder.ToString().IndexOf("location.href=\"") != -1)
                                    {
                                        string str3 = base.putstr(base.MyStringBuilder.ToString(), "location.href=\"", "\"", 0);
                                        if (str3 != "-1")
                                        {
                                            this.url = base.Host + str3;
                                        }
                                    }
                                    base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                                    base.streamControl = true;
                                    base.MyStringBuilder = this.Request(this.url);
                                    this.url = base.Host + "/hanmail/mail/FolderManage.daum?_top_hm=l_fol";
                                    base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                                    base.streamControl = true;
                                    base.MyStringBuilder = this.Request(this.url);
                                    if (base.MyStringBuilder.ToString() != "")
                                    {
                                        base.ShowMessage("登陆成功！");
                                        this.Request("http://mail2.daum.net/hanmail/env/ToHanmailExpress.daum");
                                        base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                                        base.streamControl = true;
                                        base.MyStringBuilder = this.Request("http://mail2.daum.net/hanmail/Goto.daum");
                                        this.url = base.putstr(base.MyStringBuilder.ToString(), "top.location.href=\"", "\"", 0);
                                        base.Host = this.url.Substring(0, this.url.LastIndexOf("/") + 1);
                                        if ((this.url != "-1") && (this.url != ""))
                                        {
                                            base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                                            base.streamControl = true;
                                            base.MyStringBuilder = this.Request(this.url);
                                            this.getBoxName(base.MyStringBuilder.ToString());
                                            this.Request("http://hmail2.daum.net/hanmailex/GoClassic.daum?");
                                        }
                                    }
                                    else
                                    {
                                        base.LoginFail();
                                        base.passwdErr();
                                        base.ShowMessage("登陆失败…………");
                                    }
                                }
                                else
                                {
                                    base.LoginFail();
                                    base.passwdErr();
                                    base.ShowMessage("登陆失败…………");
                                }
                            }
                        }
                        else
                        {
                            base.LoginFail();
                            base.passwdErr();
                            base.ShowMessage("登陆失败…………");
                        }
                    }
                    else
                    {
                        base.LoginFail();
                        base.passwdErr();
                        base.ShowMessage("登陆失败…………");
                    }
                }
                else
                {
                    base.LoginFail();
                    base.passwdErr();
                    base.ShowMessage("错误的用户名/密码…………");
                }
            }
            catch (Exception exception)
            {
                base.LoginFail();
                base.passwdErr();
                base.ShowMessage("登陆失败…………" + exception.Message);
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
                request.Headers.Add("Cache-Control:no-cache\r\n");
                request.Accept = "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, application/x-ms-application, application/x-ms-xbap, application/vnd.ms-xpsdocument, application/xaml+xml, */*";
                request.Headers.Add("Accept-Language: en-us,ar-SA;q=0.9,de-DE;q=0.8,es-ES;q=0.7,tr-TR;q=0.6,ja-JP;q=0.5,en-GB;q=0.4,fr-FR;q=0.3,zh-CN;q=0.2,zh-TW;q=0.1");
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; EmbeddedWB 14.52 from: http://www.bsalsa.com/ EmbeddedWB 14.52; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022)";
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
                base.ShowMessage("Fastmailpost:" + exception.Message);
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
                request.Accept = "image/jpeg, application/x-ms-application, image/gif, application/xaml+xml, image/pjpeg, application/x-ms-xbap, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, application/QVOD, application/QVOD, */*";
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
                    reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8"));
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
                base.ShowMessage("Fastmailrequest:" + exception.Message + url);
            }
            return base.MyStringBuilder;

        }
        private bool RequestEmail(string url)
        {
            try
            {
                string path = "";
                this.m_emailTime = DateTime.Now.ToString("yyyMMdd-HHmmss") + DateTime.Now.Millisecond.ToString();
                if ((GlobalValue.PopMainForm.saveFilePath != "") | (GlobalValue.PopMainForm.saveFilePath != null))
                {
                    path = GlobalValue.PopMainForm.saveFilePath;
                }
                string str2 = ".eml";
                if (GlobalValue.PopMainForm.checkDateSave)
                {
                    string str3 = DateTime.Now.Date.ToString("yyy-MM-dd");
                    path = path + str3 + @"邮件\";
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                }
                path = path + base.m_snote;
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                path = path + @"\" + base.m_stype;
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string str4 = path;
                path = str4 + @"\" + base.m_username + "@" + base.m_serv;
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                path = path + @"\" + base.BoxName;
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                path = path + @"\" + this.m_emailTime + str2;
                WebClient client = new WebClient();
                client.Headers.Add("Cookie", base.cookie);
                client.DownloadFile(url, path);
                base.m_emailno++;
                base.saveEmailCount();
                GlobalValue.PopMainForm.statusBarPanelEmailNumbersText.Text = (Convert.ToInt32(GlobalValue.PopMainForm.statusBarPanelEmailNumbersText.Text) + 1).ToString();
                base.streamControl = false;
                return true;
            }
            catch (Exception exception)
            {
                base.ShowMessage("保存文件失败：" + exception.Message);
                return false;
            }

        }
        public void SaveAddressbook(string EmailText)
        {
            string path = "";
            if ((GlobalValue.PopMainForm.saveFilePath != "") | (GlobalValue.PopMainForm.saveFilePath != null))
            {
                path = GlobalValue.PopMainForm.saveFilePath;
            }
            string str2 = DateTime.Now.Date.ToString("yyyy-MM-dd");
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
            path = path + @"\地址薄";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            Stream stream = File.Create(path + @"\" + base.m_username + "address.csv");
            using (StreamWriter writer = new StreamWriter(stream))
            {
                writer.Write(EmailText);
            }
            stream.Close();
            stream = null;

        }
        public bool SaveHanmail(byte[] buffer, int nbytes)
        {
            try
            {
                string filePath = "";
                if ((GlobalValue.PopMainForm.saveFilePath != "") | (GlobalValue.PopMainForm.saveFilePath != null))
                {
                    filePath = GlobalValue.PopMainForm.saveFilePath;
                }
                this.SaveHanmailText(buffer, nbytes, filePath);
            }
            catch (Exception exception)
            {
                base.ShowMessage("保存邮件失败：" + exception.Message);
                return false;
            }
            return true;

        }
        public void SaveHanmailText(byte[] buffer, int nbytes, string filePath)
        {
            FileStream stream = null;
            try
            {
                if (filePath == null)
                {
                    filePath = "";
                }
                string str = DateTime.Now.Date.ToString("yyyy-MM-dd");
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
        private string U2CnCode(string str)
        {
            Regex regex = new Regex(@"(?<code>\\u[a-z0-9]{4})", RegexOptions.IgnoreCase);
            for (Match match = regex.Match(str); match.Success; match = match.NextMatch())
            {
                string oldValue = match.Result("${code}");
                int num = int.Parse(oldValue.Substring(2, 4), NumberStyles.HexNumber);
                string newValue = string.Format("{0}", (char)num);
                str = str.Replace(oldValue, newValue);
            }
            return str;

        }

        // Nested Types
        [StructLayout(LayoutKind.Sequential)]
        private struct BoxNameID
        {
            public string boxname;
            public string boxid;
        }

    }
}
