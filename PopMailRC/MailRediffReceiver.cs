using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Net;
using System.Web;
using System.Runtime.InteropServices;

namespace MailExposure.PopMailRC
{
    class MailRediffReceiver:MailStr
    {
        // Fields
        private bool booPost;
        private BoxNameId[] boxList;
        public string m_emailTime;
        private string mailSub;
        private static object pob;
        private string url;
        private string urls;

        // Methods
        static MailRediffReceiver()
        {
            pob = new object();
        }
        public MailRediffReceiver()
        {
            this.boxList = new BoxNameId[200];
            this.urls = "";
            this.url = "";
            this.mailSub = "";
        }
        private void getAddressBook()
        {
            this.url = base.Host + "exportaddrbook?service=moutlook";
            base.streamControl = true;
            this.RequestEmail(this.url, ".csv");
        }
        private void getBoxName(string strBoxs)
        {
            try
            {
                this.getAddressBook();
                int startIndex = -1;
                int index = 0;
                strBoxs = base.putstr(strBoxs, "Start listing folders", "<TR class=addrnewcon>", 0);
                Console.WriteLine("dasdasd");
                strBoxs = strBoxs.Substring(strBoxs.LastIndexOf("</SCRIPT>"), strBoxs.Length - strBoxs.LastIndexOf("</SCRIPT>"));
                for (startIndex = 0; (startIndex = strBoxs.IndexOf("<tr", startIndex)) > 0; startIndex++)
                {
                    string str = base.putstr(strBoxs, "<a href=\"", "\"", startIndex).Trim();
                    string str2 = str.Substring(str.LastIndexOf("=") + 1, (str.Length - str.LastIndexOf("=")) - 1);
                    string str3 = base.putstr(strBoxs, str2 + "\">", "<", startIndex).Trim();
                    char[] trimChars = new char[] { '\r', '\n' };
                    string str4 = base.putstr(strBoxs, "<TD  colspan=2 class=sb2>&nbsp;&nbsp;&nbsp;&nbsp;", "</TD>", startIndex).Trim(trimChars);
                    if (((str2 != "-1") && (str3 != "-1")) && ((str2 != "") && (str3 != "")))
                    {
                        this.boxList[index].boxurl = str;
                        this.boxList[index].boxid = str2;
                        this.boxList[index].boxname = str3;
                        this.boxList[index].mailCount = Convert.ToInt32(str4);
                        index++;
                    }
                    else
                    {
                        base.ShowMessage("取箱子失败！");
                        return;
                    }
                }
                for (int i = 0; i < index; i++)
                {
                    this.getPageUrl(this.boxList[i].boxurl, this.boxList[i].boxname, this.boxList[i].boxid, this.boxList[i].mailCount);
                }
            }
            catch (Exception exception)
            {
                base.ShowMessage("取箱子失败！" + exception.Message);
            }
        }
        private void getEmailId(string mailStr)
        {
            try
            {
                int startIndex = 0;
                startIndex = mailStr.IndexOf("<!-- Size -->");
                mailStr = base.putstr(mailStr, "</tr>", "</TABLE>", startIndex);
                for (startIndex = 0; (startIndex = mailStr.IndexOf("<INPUT TYPE=checkbox", startIndex)) > 0; startIndex++)
                {
                    string str = base.putstr(mailStr, "VALUE=\"", "\"", startIndex);
                    switch (str)
                    {
                        case "-1":
                        case "":
                            base.ShowMessage(base.BoxName + "邮件取ID失败！");
                            return;
                    }
                    if (str.EndsWith(".old"))
                    {
                        string strSql = "select count(*) from RediffmailId where MsgId='" + str + "'";
                        if (Convert.ToInt32(GlobalValue.PopMainForm.ExecuteSQL(strSql)) == 0)
                        {
                            base.putstr(mailStr, "<u>", "</u>", startIndex).Trim();
                            string str4 = base.putstr(mailStr, "<a href=\"", "\"", startIndex);
                            string message = base.putstr(mailStr, "</a>", "<TD colspan=8 class=addrmobnum>", startIndex);
                            message = base.putstr(message, "class=sb1>", "</TR>", 0) + "</TR>";
                            string str2 = HttpUtility.HtmlDecode(base.putstr(message, "class=sb1>", "</TD>", 0)).Trim();
                            this.mailSub = str2;
                            message = base.putstr(message, "class=sb1>", "</TR>", 0);
                            string str3 = HttpUtility.HtmlDecode(base.putstr(message, "class=sb1>", "</TD>", 0)).Trim();
                            this.url = base.Host + "readmail?printable=1&block_images=1&file_name=" + str + "&folder=" + base.BoxName;
                            if (base.BoxName == "Sent Mail")
                            {
                                this.url = base.Host + "readmail?printable=1&block_images=1&file_name=" + str + "&folder=Sent_messages";
                            }
                            base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                            base.streamControl = true;
                            try
                            {
                                this.RequestEmail(this.url, ".html");
                            }
                            catch (Exception)
                            {
                                base.ShowMessage(base.BoxName + ":" + str2 + "下载邮件失败！");
                            }
                            this.url = base.Host + str4;
                            base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                            base.streamControl = true;
                            base.MyStringBuilder = this.Request(this.url);
                            if (base.MyStringBuilder.ToString().IndexOf("Download all attachments") > 0)
                            {
                                string str7 = base.MyStringBuilder.ToString();
                                int index = str7.IndexOf("Go to Attachment(s)");
                                str7 = base.putstr(str7, "<a href=\"", "\"", index);
                                if ((str7 != "") && (str7 != "-1"))
                                {
                                    base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                                    base.streamControl = true;
                                    try
                                    {
                                        this.RequestEmail(str7, "-附件.zip");
                                    }
                                    catch (Exception)
                                    {
                                        try
                                        {
                                            this.RequestEmail(str7, "-附件.zip");
                                        }
                                        catch (Exception)
                                        {
                                            base.ShowMessage(base.BoxName + ":" + str2 + "附件下载失败！");
                                        }
                                    }
                                }
                            }
                            try
                            {
                                base.saveEmailCount();
                                GlobalValue.PopMainForm.statusBarPanelEmailNumbersText.Text = (Convert.ToInt32(GlobalValue.PopMainForm.statusBarPanelEmailNumbersText.Text) + 1).ToString();
                                base.m_emailno++;
                                string str8 = DateTime.Now.ToString();
                                strSql = "insert into RediffmailId(Name,MsgId,DownTime,MailType) values('" + base.m_username + "','" + str + "','" + str8 + "','" + base.BoxName.Substring(0, base.BoxName.Length % 10) + "')";
                                GlobalValue.PopMainForm.ExecuteSQL(strSql);
                            }
                            catch (Exception exception)
                            {
                                base.ShowMessage("添加失败：" + exception.Message);
                            }
                            base.ShowMessage(base.BoxName + ":" + str2 + "下载完毕！");
                        }
                    }
                }
            }
            catch (Exception exception2)
            {
                base.ShowMessage(base.BoxName + "邮件取ID失败………" + exception2.Message);
            }

        }
        private void getPageUrl(string boxUrl, string boxName, string boxId, int mailCount)
        {
            try
            {
                int num = 1;
                string message = boxUrl;
                base.BoxName = boxName;
                if (mailCount != 0)
                {
                    goto Label_01C6;
                }
                base.ShowMessage(boxName + "箱子是空的！");
                return;
            Label_0028:
                base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                base.streamControl = true;
                base.MyStringBuilder = this.Request(base.Host + message);
                string str2 = base.MyStringBuilder.ToString();
                message = base.putstr(str2, "Prev", "Next", 0);
                message = base.putstr(message, "<a href=\"", "\"", 0).Trim();
                if (Convert.ToInt32(GlobalValue.PopMainForm.PageNumber) == 0)
                {
                    base.ShowMessage(string.Concat(new object[] { base.BoxName, "：第", num, "页开始下载" }));
                    this.getEmailId(str2);
                    base.ShowMessage(string.Concat(new object[] { base.BoxName, "：第", num, "页下载完成" }));
                    num++;
                }
                else if (num <= Convert.ToInt32(GlobalValue.PopMainForm.PageNumber))
                {
                    base.ShowMessage(string.Concat(new object[] { base.BoxName, "：第", num, "页开始下载" }));
                    this.getEmailId(str2);
                    base.ShowMessage(string.Concat(new object[] { base.BoxName, "：第", num, "页下载完成" }));
                    num++;
                }
            Label_01C6:
                if ((message != "") && (message != "-1"))
                {
                    goto Label_0028;
                }
            }
            catch (Exception exception)
            {
                base.ShowMessage("取页数失败！" + exception.Message);
            }
        }
        public override void login()
        {
            base.cookie = "";
            base.ShowMessage("开始登陆……");
            this.url = "http://mail.rediff.com/cgi-bin/login.cgi";
            string indata = "";
            if (base.m_serv.Equals("rediffmail.com"))
            {
                indata = "login=" + base.m_username + "@" + base.m_serv + "&passwd=" + base.m_passwd + "&remember=1&FormName=existing";
            }
            else
            {
                indata = "login=" + base.m_username + "&passwd=" + base.m_passwd + "&remember=1&FormName=existing";
            }
            base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
            base.streamControl = true;
            base.MyStringBuilder = this.PostData(this.url, indata);
            string message = base.MyStringBuilder.ToString();
            if (message.IndexOf("Your login failed.") != -1)
            {
                base.LoginFail();
                base.passwdErr();
                base.ShowMessage("用户名或密码错误！");
            }
            else if (message.IndexOf("window.location.replace(\"") != -1)
            {
                this.url = base.putstr(message, "window.location.replace(\"", "\"", 0);
                if ((this.url != "") && (this.url != "-1"))
                {
                    base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                    base.streamControl = true;
                    base.MyStringBuilder = this.Request(this.url);
                    message = base.MyStringBuilder.ToString();
                    if (message.IndexOf("This is to protect you from an on going phishing attack on web mail services") != -1)
                    {
                        base.ShowMessage("登陆成功！");
                        int index = message.IndexOf("href=\"folder?els=");
                        this.url = base.putstr(message, "\"", "\"", index - 15);
                        this.url = base.Host + this.url;
                        base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                        base.streamControl = true;
                        base.MyStringBuilder = this.Request(this.url);
                        message = base.MyStringBuilder.ToString();
                        if ((base.MyStringBuilder.ToString() != "") && (base.MyStringBuilder.ToString() != null))
                        {
                            this.getBoxName(message);
                        }
                        else
                        {
                            base.ShowMessage("去箱子失败！");
                        }
                    }
                    else
                    {
                        base.LoginFail();
                        base.passwdErr();
                        base.ShowMessage("登录失败！");
                    }
                }
                else
                {
                    base.LoginFail();
                    base.passwdErr();
                    base.ShowMessage("登录失败！");
                }
            }
            else
            {
                base.LoginFail();
                base.passwdErr();
                base.ShowMessage("登录失败！");
            }
        }
        private StringBuilder PostData(string url, string indata)
        {
            try
            {
                StreamReader reader;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.ServicePoint.Expect100Continue = false;
                request.Method = "POST";
                request.Headers.Add("Cookie", base.cookie);
                if (url.IndexOf("http://mail.rediff.com/") != -1)
                {
                    request.ContentType = "application/x-www-form-urlencoded";
                }
                else
                {
                    request.ContentType = "application/xml";
                }
                request.Headers.Add("Pragma:no-cache");
                request.Accept = "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, application/x-ms-application, application/x-ms-xbap, application/vnd.ms-xpsdocument, application/xaml+xml, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, */*";
                request.Headers.Add("Accept-Language:zh-cn");
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1) ; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022; InfoPath.3)";
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
                if (headers["Content-Type"] != null)
                {
                    string str = "";
                    str = headers["Content-Type"];
                    if (str.IndexOf("charset=") >= 0)
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
                }
                response.Close();
                response = null;
                base.streamControl = false;
                reader.Close();
                reader = null;
            }
            catch (Exception exception)
            {
                if (!this.booPost)
                {
                    this.booPost = true;
                    this.PostData(url, indata);
                }
                else
                {
                    base.ShowMessage("RediffPost:" + exception.Message);
                }
            }
            return base.MyStringBuilder;

        }
        private StringBuilder Request(string url)
        {
            try
            {
                StreamReader reader;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "get";
                request.AllowAutoRedirect = false;
                request.ServicePoint.Expect100Continue = false;
                request.KeepAlive = true;
                request.Headers.Add("Pragma:no-cache");
                request.Accept = "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, application/x-ms-application, application/x-ms-xbap, application/vnd.ms-xpsdocument, application/xaml+xml, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, */*";
                request.Headers.Add("Accept-Language:zh-cn");
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1) ; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022; InfoPath.3)";
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
                    if (this.urls.IndexOf("/prism/") != -1)
                    {
                        string str2 = base.putstr(this.urls, "http:", "/prism/", 0);
                        if ((str2 != "-1") && (base.Host == null))
                        {
                            base.Host = "http:" + str2 + "/prism/";
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
                }
                response.Close();
                response = null;
                base.streamControl = false;
                reader.Close();
                reader = null;
            }
            catch (Exception exception)
            {
                base.ShowMessage("RediffRequest:" + exception.Message);
            }
            return base.MyStringBuilder;

        }
        private bool RequestEmail(string url, string type)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Headers.Add("Cookie", base.cookie);
            request.Method = "GET";
            request.Accept = "*/*";
            request.AllowAutoRedirect = false;
            request.KeepAlive = true;
            request.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; .NET CLR 2.0.50727)";
            request.Headers.Add("Accept-Language: zh-cn");
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Thread.Sleep(10);
            WebHeaderCollection headers = response.Headers;
            Stream responseStream = response.GetResponseStream();
            if (base.streamControl)
            {
                int num;
                byte[] buffer = new byte[0x400];
                while ((num = responseStream.Read(buffer, 0, 0x400)) > 0)
                {
                    if (!this.SaveRediffAnnux(buffer, num, type))
                    {
                        responseStream.Close();
                        response.Close();
                        return false;
                    }
                }
                if (type == ".csv")
                {
                    base.ShowMessage("地址簿下载完成！");
                }
                responseStream.Close();
                response.Close();
                base.streamControl = false;
                return true;
            }
            responseStream.Close();
            response.Close();
            return false;

        }
        private bool SaveRediffAnnux(byte[] buffer, int nbytes, string type)
        {
            string filePath = "";
            try
            {
                if ((GlobalValue.PopMainForm.saveFilePath != "") | (GlobalValue.PopMainForm.saveFilePath != null))
                {
                    filePath = GlobalValue.PopMainForm.saveFilePath;
                }
                this.SaveRediffAnnux(buffer, nbytes, filePath, type);
            }
            catch (Exception exception)
            {
                base.ShowMessage("保存邮件失败：" + exception.Message + filePath);
                return false;
            }
            return true;

        }
        public void SaveRediffAnnux(byte[] buffer, int nbytes, string filePath, string type)
        {
            lock (pob)
            {
                FileStream stream = null;
                try
                {
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
                    if (type == ".csv")
                    {
                        base.BoxName = "地址簿";
                        this.mailSub = "";
                        filePath = filePath + @"\" + base.BoxName;
                        if (!Directory.Exists(filePath))
                        {
                            Directory.CreateDirectory(filePath);
                        }
                        filePath = filePath + @"\" + base.m_username + "@" + base.m_serv + type;
                        stream = File.Open(filePath, FileMode.Append, FileAccess.Write);
                        stream.Write(buffer, 0, nbytes);
                        stream.Close();
                    }
                    else
                    {
                        filePath = filePath + @"\" + base.BoxName;
                        if (!Directory.Exists(filePath))
                        {
                            Directory.CreateDirectory(filePath);
                        }
                        filePath = string.Concat(new object[] { filePath, @"\", base.m_emailno, type });
                        stream = File.Open(filePath, FileMode.Append, FileAccess.Write);
                        stream.Write(buffer, 0, nbytes);
                        stream.Close();
                    }
                }
                catch (Exception)
                {
                }
                finally
                {
                    if (stream != null)
                    {
                        stream.Close();
                    }
                }
            }
        }

        // Nested Types
        [StructLayout(LayoutKind.Sequential)]
        private struct BoxNameId
        {
            public string boxname;
            public string boxid;
            public int mailCount;
            public string boxurl;
        }

    }
}
