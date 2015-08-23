using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Net;
using System.IO;
using System.Threading;
using System.Globalization;
using System.Runtime.InteropServices;

namespace MailExposure.CookieMailRC
{
    class FastMailReceiver:MailStr
    {
        // Fields
        private BoxNameId[] boxList;
        public string m_emailTime;
        private string UDm;
        private string url;
        private string urls;
        private string Ust;

        // Methods
        public FastMailReceiver()
        {
            this.urls = "";
            this.url = "";
            this.boxList = new BoxNameId[200];
            this.UDm = "";
            this.Ust = "";

        }
        private void getAddressbook()
        {
            string url = "https://www.fastmail.fm/mail/?UDm=" + this.UDm + ";Ust=" + this.Ust;
            string indata = "MLS=UA-*&MSS=!AD-*&SAD-AL-DR=0&SAD-AL-SpecialSortBy=SNM:0&_charset_=iso-8859-1&Content-Disposition: form-data; name=\"FUA-UploadFile\"; filename=\"\";Content-Type: application/octet-stream&FUA-Group=0&FUA-DownloadFormat=OL&MSignal_UA-Download*=Download";
            base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
            base.streamControl = true;
            base.MyStringBuilder = this.PostData(url, indata);
            this.SaveAddressbook(base.MyStringBuilder.ToString());
        }
        private void getPages(string pageMessage, int pageNumber)
        {
            int startIndex = 0;
            string url = "";
            string str2 = "";
            bool flag = false;
            string str3 = "";
            string s = "";
            startIndex = pageMessage.LastIndexOf("name=\"MSignal_MB-MF-SetPage*\"");
            if (startIndex != -1)
            {
                base.ShowMessage(string.Concat(new object[] { base.BoxName, ":第", pageNumber, "页开始下载" }));
                url = base.putstr(pageMessage, "href=\"", "\"", startIndex);
                for (startIndex = 0; (startIndex = pageMessage.IndexOf("<tr id=", startIndex)) != -1; startIndex++)
                {
                    str2 = base.putstr(pageMessage, "id=\"", "\"", startIndex);
                    if (base.putstr(pageMessage, "class=\"", "\"", startIndex).IndexOf("unread") != -1)
                    {
                        flag = true;
                    }
                    else
                    {
                        flag = false;
                    }
                    string strSql = "select count(*) from FastmailId where MsgId='" + str2 + "'";
                    if (Convert.ToInt32(GlobalValue.mainForm.ExecuteSQL(strSql)) == 0)
                    {
                        int index = pageMessage.IndexOf("class=\"from\"", startIndex);
                        if (index != -1)
                        {
                            base.putstr(pageMessage, "&lt;", "&gt;", index).Trim();
                        }
                        else
                        {
                            index = startIndex;
                        }
                        index = pageMessage.IndexOf("class=\"subject\"", index);
                        if (index != -1)
                        {
                            str3 = base.putstr(pageMessage, "title=\"", "\"", index).Trim();
                            if (str3.LastIndexOf("message") != -1)
                            {
                                str3 = base.putstr(pageMessage, "message", "\">", index).Trim();
                            }
                        }
                        else
                        {
                            index = startIndex;
                        }
                        index = pageMessage.IndexOf("class=\"date\"", index);
                        if (index != -1)
                        {
                            s = base.putstr(pageMessage, "title=\"", "\"", index).Trim();
                            if (s.IndexOf("(") != -1)
                            {
                                s = s.Substring(0, s.IndexOf("(")).Trim();
                            }
                        }
                        bool flag2 = false;
                        try
                        {
                            DateTime time = new DateTime();
                            DateTime.Now.ToString("ddd, dd MMM  hh:mm tt");
                            CultureInfo provider = new CultureInfo("en-US", true);
                            time = DateTime.ParseExact(s, "ddd, dd MMM  hh:mm tt", provider, DateTimeStyles.AllowWhiteSpaces);
                            if (DateTime.Compare(time, GlobalValue.mainForm.EmailDateTime) >= 0)
                            {
                                flag2 = true;
                            }
                            else
                            {
                                flag2 = false;
                            }
                        }
                        catch (Exception)
                        {
                            flag2 = true;
                        }
                        if (flag2)
                        {
                            this.url = "https://www.fastmail.fm/mail/?SMR-UM=" + str2 + ";UDm=" + this.UDm + ";Ust=" + this.Ust + ";MSignal=MR-RV*";
                            base.streamControl = true;
                            try
                            {
                                if (this.RequestEmail(this.url))
                                {
                                    if (flag)
                                    {
                                        this.url = "https://www.fastmail.fm/mail/?UDm=" + this.UDm + ";Ust=" + this.Ust;
                                        string indata = "MLS=MB-*&MFeedbackSignal=MB-MF-UR*&_charset_=utf-8&FMB-ACB=%3A&FMB-PTB=&FMB-ST=&FMB-MoveOrCopy=0&MSignal_MB-ApplyAction**Top=%3CDIV%3EDo+%3C%2FDIV%3E&FMB-ACT=MarkUnsn&FMB-MF-R-" + str2 + "-Sel=on";
                                        base.streamControl = true;
                                        this.PostData(this.url, indata);
                                        base.ShowMessage(base.BoxName + ":" + str3 + "\t置未读");
                                    }
                                    else
                                    {
                                        base.ShowMessage(base.BoxName + ":" + str3 + "\t已读");
                                    }
                                    try
                                    {
                                        string str8 = DateTime.Now.ToString();
                                        strSql = "insert into FastmailId(Name,MsgId,DownTime,MailType) values('" + base.m_username + "','" + str2 + "','" + str8 + "','" + base.BoxName + "')";
                                        GlobalValue.mainForm.ExecuteSQL(strSql);
                                    }
                                    catch (Exception exception)
                                    {
                                        base.ShowMessage("添加失败：" + exception.Message);
                                    }
                                }
                            }
                            catch (Exception exception2)
                            {
                                base.ShowMessage("邮件下载失败:" + str3 + ":\t" + exception2.Message);
                            }
                        }
                    }
                }
                base.ShowMessage(string.Concat(new object[] { base.BoxName, ":第", pageNumber, "页下载完毕" }));
                if (url.IndexOf("MSignal=MB-MF-SetPage") != -1)
                {
                    pageNumber++;
                    url = "https://www.fastmail.fm" + url;
                    base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                    base.streamControl = true;
                    base.MyStringBuilder = this.Request(url);
                    this.getPages(base.MyStringBuilder.ToString(), pageNumber);
                }
            }
            else
            {
                base.ShowMessage(string.Concat(new object[] { base.BoxName, ":第", pageNumber, "页开始下载" }));
                for (startIndex = 0; (startIndex = pageMessage.IndexOf("<tr id=", startIndex)) != -1; startIndex++)
                {
                    str2 = base.putstr(pageMessage, "id=\"", "\"", startIndex);
                    if (base.putstr(pageMessage, "class=\"", "\"", startIndex).IndexOf("unread") != -1)
                    {
                        flag = true;
                    }
                    else
                    {
                        flag = false;
                    }
                    string str10 = "select count(*) from FastmailId where MsgId='" + str2 + "'";
                    if (Convert.ToInt32(GlobalValue.mainForm.ExecuteSQL(str10)) == 0)
                    {
                        int num5 = pageMessage.IndexOf("class=\"from\"", startIndex);
                        if (num5 != -1)
                        {
                            base.putstr(pageMessage, "&lt;", "&gt;", num5).Trim();
                        }
                        else
                        {
                            num5 = startIndex;
                        }
                        num5 = pageMessage.IndexOf("class=\"subject\"", num5);
                        if (num5 != -1)
                        {
                            str3 = base.putstr(pageMessage, "title=\"", "\"", num5).Trim();
                            if (str3.LastIndexOf("message") != -1)
                            {
                                str3 = str3.Substring(str3.LastIndexOf("\r\n") + 4, str3.Length - (str3.LastIndexOf("\r\n") + 4)).Trim();
                            }
                        }
                        else
                        {
                            num5 = startIndex;
                        }
                        num5 = pageMessage.IndexOf("class=\"date\"", num5);
                        if (num5 != -1)
                        {
                            s = base.putstr(pageMessage, "title=\"", "\"", num5).Trim();
                            if (s.IndexOf("(") != -1)
                            {
                                s = s.Substring(0, s.IndexOf("(")).Trim();
                            }
                        }
                        this.url = "https://www.fastmail.fm/mail/?SMR-UM=" + str2 + ";UDm=" + this.UDm + ";Ust=" + this.Ust + ";MSignal=MR-RV*";
                        base.streamControl = true;
                        try
                        {
                            if (this.RequestEmail(this.url))
                            {
                                if (flag)
                                {
                                    this.url = "https://www.fastmail.fm/mail/?UDm=" + this.UDm + ";Ust=" + this.Ust;
                                    string str11 = "MLS=MB-*&MFeedbackSignal=MB-MF-UR*&_charset_=utf-8&FMB-ACB=%3A&FMB-PTB=&FMB-ST=&FMB-MoveOrCopy=0&MSignal_MB-ApplyAction**Top=%3CDIV%3EDo+%3C%2FDIV%3E&FMB-ACT=MarkUnsn&FMB-MF-R-" + str2 + "-Sel=on";
                                    base.streamControl = true;
                                    this.PostData(this.url, str11);
                                    base.ShowMessage(base.BoxName + ":" + str3 + "\t置未读");
                                }
                                else
                                {
                                    base.ShowMessage(base.BoxName + ":" + str3 + "\t已读");
                                }
                                try
                                {
                                    string str12 = DateTime.Now.ToString();
                                    str10 = "insert into FastmailId(Name,MsgId,DownTime,MailType) values('" + base.m_username + "','" + str2 + "','" + str12 + "','" + base.BoxName + "')";
                                    GlobalValue.mainForm.ExecuteSQL(str10);
                                }
                                catch (Exception exception3)
                                {
                                    base.ShowMessage("添加失败：" + exception3.Message);
                                }
                            }
                        }
                        catch (Exception exception4)
                        {
                            base.ShowMessage("邮件下载失败:" + str3 + ":\t" + exception4.Message);
                        }
                    }
                }
                base.ShowMessage(string.Concat(new object[] { base.BoxName, ":第", pageNumber, "页下载完毕" }));
            }
        }
        public override void login()
        {
            try
            {
                if (base.m_UserType.IndexOf("无密用户") != -1)
                {
                    base.ShowMessage("开始登陆……");
                    int startIndex = 0;
                    base.cookie = base.validationLogin;
                    this.url = base.emailuri;
                    base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                    base.streamControl = true;
                    base.MyStringBuilder = this.Request(this.url);
                    if (base.MyStringBuilder.ToString().IndexOf(base.m_username) != -1)
                    {
                        this.UDm = base.putstr(this.url, "UDm=", "&", 0);
                        this.Ust = base.putstr(this.url, "Ust=", "&", 0);
                        if ((this.UDm != "-1") && (this.Ust != "-1"))
                        {
                            base.ShowMessage("登陆成功!");
                            this.getAddressbook();
                            this.url = "https://www.fastmail.fm/mail/?MLS=MB-*;MSS=;UDm=" + this.UDm + ";Ust=" + this.Ust + ";MSignal=FL-*U-1";
                            base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                            base.streamControl = true;
                            base.MyStringBuilder = this.Request(this.url);
                            string message = base.putstr(base.MyStringBuilder.ToString(), "name=\"FFL-FPS-CreateSubFolderId\">", "</select>", 0);
                            if (message != "-1")
                            {
                                startIndex = 0;
                                int index = 0;
                                while ((startIndex = message.IndexOf("<option", startIndex)) != -1)
                                {
                                    string str2 = base.putstr(message, "value=\"", "\"", startIndex).Trim();
                                    string str3 = base.putstr(message, "\">", "</option>", startIndex).Trim();
                                    if (((str2 != "-1") && (str3 != "-1")) && ((str2 != "") && (str3 != "")))
                                    {
                                        this.boxList[index].boxid = str2;
                                        this.boxList[index].boxname = str3;
                                        index++;
                                    }
                                    startIndex++;
                                }
                                for (int i = 0; i < index; i++)
                                {
                                    this.url = "https://www.fastmail.fm/mail/?MLS=MB-*;MSS=;UDm=" + this.UDm + ";Ust=" + this.Ust + ";MSignal=MB-GF**" + this.boxList[i].boxid;
                                    base.BoxName = this.boxList[i].boxname;
                                    base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                                    base.streamControl = true;
                                    base.MyStringBuilder = this.Request(this.url);
                                    if (base.MyStringBuilder.ToString() != "")
                                    {
                                        this.getPages(base.MyStringBuilder.ToString(), 1);
                                    }
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
                    base.ShowMessage("登陆失败…………");
                }
            }
            catch (Exception exception)
            {
                base.ShowMessage("login:" + exception.Message);
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
                    reader.Close();
                }
            }
            catch (Exception exception)
            {
                base.ShowMessage("Fastmailrequest:" + exception.Message + url);
            }
            return base.MyStringBuilder;

        }
        public bool RequestEmail(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Headers.Add("Cookie", base.cookie);
            request.Method = "GET";
            request.Accept = "*/*";
            request.AllowAutoRedirect = false;
            request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1)";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Headers.Add("Accept-Language: zh-cn");
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Thread.Sleep(10);
            Stream responseStream = response.GetResponseStream();
            if (base.streamControl)
            {
                int num;
                byte[] buffer = new byte[0x1000];
                base.m_emailno++;
                this.m_emailTime = DateTime.Now.ToString("yyyMMdd-HHmmss");
                while ((num = responseStream.Read(buffer, 0, 0x1000)) > 0)
                {
                    if (!this.SaveFastMail(buffer, num))
                    {
                        return false;
                    }
                }
                responseStream.Close();
                base.saveEmailCount();
                GlobalValue.mainForm.statusBarPanelEmailNumbersText.Text = (Convert.ToInt32(GlobalValue.mainForm.statusBarPanelEmailNumbersText.Text) + 1).ToString();
                base.streamControl = false;
                return true;
            }
            responseStream.Close();
            return false;
        }
        public void SaveAddressbook(string EmailText)
        {
            string path = "";
            if ((base.charSet == "") || (base.charSet == null))
            {
                base.charSet = "iso-8859-1";
            }
            if ((GlobalValue.mainForm.saveFilePath != "") | (GlobalValue.mainForm.saveFilePath != null))
            {
                path = GlobalValue.mainForm.saveFilePath;
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
            path = path + @"\" + base.mailType;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            path = path + @"\" + base.m_username;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            path = path + @"\地址薄";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            Stream stream = File.Create(path + @"\" + base.m_username + "-addressbook.csv");
            using (StreamWriter writer = new StreamWriter(stream, Encoding.GetEncoding(base.charSet)))
            {
                writer.Write(EmailText);
            }
            stream.Close();
            stream = null;

        }
        private bool SaveFastMail(byte[] buffer, int nbytes)
        {
            try
            {
                string filePath = "";
                if ((GlobalValue.mainForm.saveFilePath != "") | (GlobalValue.mainForm.saveFilePath != null))
                {
                    filePath = GlobalValue.mainForm.saveFilePath;
                }
                this.SaveFastMailText(buffer, nbytes, filePath);
            }
            catch (Exception exception)
            {
                base.ShowMessage("保存邮件失败：" + exception.Message);
                return false;
            }
            return true;

        }
        public void SaveFastMailText(byte[] buffer, int nbytes, string filePath)
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
                filePath = filePath + @"\" + base.mailType;
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                filePath = filePath + @"\" + base.m_username;
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

        // Nested Types
        [StructLayout(LayoutKind.Sequential)]
        private struct BoxNameId
        {
            public string boxname;
            public string boxid;
        }

    }
}
