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
    class Mail126Receiver:MailStr
    {
        // Fields
        public bool booPost;
        private BoxNameId[] boxList;
        public string m_emailTime;
        private string url;
        private string urls;

        // Methods
        public Mail126Receiver()
        {
            this.boxList = new BoxNameId[200];
            this.urls = "";
            this.url = "";
        }
        private void getBoxName()
        {
            int startIndex = 0;
            int index = 0;
            try
            {
                string message = "";
                if (this.urls.IndexOf("mail.126.com") != -1)
                {
                    if (this.urls.IndexOf("j/dm3/main") != -1)
                    {
                        this.urls = this.urls.Replace("j/dm3/main.jsp", "j/dm3/index.jsp");
                        base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                        base.streamControl = true;
                        base.MyStringBuilder = this.Request(this.urls);
                        message = base.MyStringBuilder.ToString();
                        this.urls = this.urls.Replace("j/dm3/index.jsp", "s");
                        this.url = this.urls + "&func=mbox:getAllFolders";
                        base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                        base.streamControl = true;
                        base.MyStringBuilder = this.Request(this.url);
                        int num3 = 0;
                        num3 = message.IndexOf("var gFolders");
                        message = base.putstr(message, "var gFolders", "</script><link", num3);
                    }
                    else
                    {
                        this.urls = this.urls.Replace("/js3/main.jsp", "/js3/index.jsp");
                        base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                        base.streamControl = true;
                        base.MyStringBuilder = this.Request(this.urls);
                        message = base.MyStringBuilder.ToString();
                        this.urls = this.urls.Replace("/js3/index.jsp", "/js3/s");
                        this.url = this.urls + "&func=mbox:getAllFolders";
                        base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                        base.streamControl = true;
                        base.MyStringBuilder = this.Request(this.url);
                        int num4 = 0;
                        num4 = message.IndexOf("folders : [{");
                        message = base.putstr(message, "folders : [{", "senders ", num4);
                    }
                    while ((startIndex = message.IndexOf("id", startIndex)) != -1)
                    {
                        string str2 = base.putstr(message, "id':", ",", startIndex).Trim();
                        string str3 = base.putstr(message, "name':'", "',", startIndex).Trim();
                        string str4 = base.putstr(message, "messageCount':", ",", startIndex).Trim();
                        if (((str2 != "-1") && (str3 != "-1")) && ((str2 != "") && (str3 != "")))
                        {
                            this.boxList[index].boxid = str2;
                            this.boxList[index].boxname = str3;
                            this.boxList[index].mailCount = Convert.ToInt32(str4);
                            index++;
                        }
                        startIndex++;
                    }
                    for (int i = 0; i < index; i++)
                    {
                        this.getEmailId(this.urls, this.boxList[i].boxname, this.boxList[i].boxid, this.boxList[i].mailCount);
                    }
                }
                else
                {
                    base.ShowMessage("取箱子失败！");
                }
            }
            catch (Exception exception)
            {
                base.ShowMessage("取箱子失败！" + exception.Message);
            }

        }
        private void getEmailId(string urlbox, string boxName, string boxId, int mailCount)
        {
            int startIndex = 0;
            string url = "";
            string str2 = "";
            try
            {
                base.BoxName = boxName;
                string indata = string.Concat(new object[] { "<?xml version=\"1.0\"?><object><int name=\"fid\">", boxId, "</int><string name=\"order\">date</string><boolean name=\"desc\">true</boolean><int name=\"start\">0</int><int name=\"limit\">", mailCount, "</int></object>" });
                url = urlbox + "&func=mbox:listMessages";
                base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                base.streamControl = true;
                base.MyStringBuilder = this.PostData(url, indata);
                indata = base.MyStringBuilder.ToString();
                while ((startIndex = indata.IndexOf("<object>", startIndex)) != -1)
                {
                    string str7 = base.putstr(indata, "<string name=\"id\">", "</string>", startIndex).Trim();
                    if ((str7 != "-1") && (str7 != ""))
                    {
                        string strSql = "select count(*) from 126mailId where MsgId='" + str7 + "'";
                        if (Convert.ToInt32(GlobalValue.PopMainForm.ExecuteSQL(strSql)) == 0)
                        {
                            string str3 = HttpUtility.HtmlDecode(base.putstr(indata, "<string name=\"from\">", "</string>", startIndex));
                            string str4 = HttpUtility.HtmlDecode(base.putstr(indata, "<string name=\"subject\">", "</string>", startIndex));
                            string str5 = base.putstr(indata, "<date name=\"receivedDate\">", "</date>", startIndex);
                            bool flag = false;
                            try
                            {
                                DateTime time = new DateTime();
                                time = Convert.ToDateTime(str5);
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
                                str2 = urlbox + "&func=mbox:getMessageData&mid=" + str7 + "&mode=download";
                                base.streamControl = true;
                                try
                                {
                                    if (this.RequestEmail(str2))
                                    {
                                        try
                                        {
                                            string str9 = DateTime.Now.ToString();
                                            strSql = "insert into 126mailId(Name,MsgId,DownTime,MailType) values('" + base.m_username + "','" + str7 + "','" + str9 + "','" + base.BoxName.Substring(0, base.BoxName.Length % 10) + "')";
                                            GlobalValue.PopMainForm.ExecuteSQL(strSql);
                                        }
                                        catch (Exception exception)
                                        {
                                            base.ShowMessage("添加失败：" + exception.Message);
                                        }
                                        base.ShowMessage(base.BoxName + ":" + str4 + "\t下载");
                                    }
                                    else
                                    {
                                        base.ShowMessage("邮件请求内容失败:" + str4);
                                    }
                                }
                                catch (Exception exception2)
                                {
                                    base.ShowMessage("邮件下载失败:" + str4 + ":\t" + exception2.Message);
                                }
                            }
                        }
                    }
                    startIndex++;
                }
            }
            catch (Exception exception3)
            {
                base.ShowMessage("取邮件失败!" + exception3.Message);
            }

        }
        public override void login()
        {
            try
            {
                base.cookie = "";
                string indata = "domain=126.com&language=0&bCookie=&username=" + base.m_username + "@126.com&user=" + base.m_username + "&password=" + base.m_passwd + "&style=-1&secure=&enter.x=%B5%C7+%C2%BC";
                this.url = "http://reg.163.com/logins.jsp?type=1&product=mail126&url=http://entry.mail.126.com/cgi/ntesdoor?hid%3D10010102%26lightweight%3D1%26verifycookie%3D1%26language%3D0%26style%3D-1";
                base.ShowMessage("开始登陆…………");
                base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                base.streamControl = true;
                base.MyStringBuilder = this.PostData(this.url, indata);
                if ((base.MyStringBuilder.ToString() != "") && (base.MyStringBuilder.ToString().IndexOf("@126.com") != -1))
                {
                    int startIndex = 0;
                    startIndex = base.MyStringBuilder.ToString().IndexOf("window.location.replace");
                    if (startIndex != -1)
                    {
                        this.url = base.putstr(base.MyStringBuilder.ToString(), "\"", "\"", startIndex);
                        if ((this.url != "-1") && (this.url != null))
                        {
                            base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                            base.streamControl = true;
                            base.MyStringBuilder = this.Request(this.url);
                            if ((base.MyStringBuilder.ToString() != "") && (base.MyStringBuilder.ToString().IndexOf("@126.com") != -1))
                            {
                                startIndex = base.MyStringBuilder.ToString().IndexOf("window.location.replace");
                                if (startIndex != -1)
                                {
                                    this.url = base.putstr(base.MyStringBuilder.ToString(), "\"", "\"", startIndex);
                                    if ((this.url != "-1") && (this.url != null))
                                    {
                                        base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                                        base.streamControl = true;
                                        base.MyStringBuilder = this.Request(this.url);
                                    }
                                }
                                base.ShowMessage("登陆成功!");
                                this.getBoxName();
                                base.streamControl = true;
                                this.RequestAddress(this.urls);
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
                    else if (base.MyStringBuilder.ToString().IndexOf("http://reg.163.com/RecoverPasswd1.shtml") != -1)
                    {
                        base.ShowMessage("请检查用户名和密码是否正确");
                        base.ShowMessage("登陆失败!");
                    }
                }
                else
                {
                    base.LoginFail();
                    base.passwdErr();
                    base.ShowMessage("登陆失败！");
                }
            }
            catch (Exception exception)
            {
                base.LoginFail();
                base.passwdErr();
                base.ShowMessage("登陆失败！" + exception.Message);
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
                if (url.IndexOf("http://entry.mail.126.com/") != -1)
                {
                    request.ContentType = "application/x-www-form-urlencoded";
                }
                else
                {
                    request.ContentType = "application/xml";
                }
                request.Headers.Add("Pragma:no-cache");
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
                    base.streamControl = false;
                    reader.Close();
                }
                reader.Close();
                this.booPost = true;
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
                    base.ShowMessage("Hpost:" + exception.Message);
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
                    reader.Close();
                }
            }
            catch (Exception exception)
            {
                base.ShowMessage("Hrequest:" + exception.Message);
            }
            return base.MyStringBuilder;

        }
        private bool RequestAddress(string urls)
        {
            if (urls.IndexOf("js3/s") != -1)
            {
                urls = urls.Replace("js3/s", "a/s");
            }
            this.url = urls + "&func=pab%3AexportContacts&outformat=8";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(this.url);
            request.Method = "GET";
            request.Headers.Add("cookie", base.cookie);
            request.Headers.Add("Accept-Language: zh-cn");
            request.ContentType = "Content-Type:application/octet-stream;charset=GBK";
            request.KeepAlive = true;
            request.AllowAutoRedirect = false;
            request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; EmbeddedWB 14.52 from: http://www.bsalsa.com/ EmbeddedWB 14.52; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022)";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Thread.Sleep(10);
            Stream responseStream = response.GetResponseStream();
            if (base.streamControl)
            {
                int num;
                byte[] buffer = new byte[0x1000];
                string text1 = DateTime.Now.ToString("yyyMMdd-HHmmss") + DateTime.Now.Millisecond.ToString();
                while ((num = responseStream.Read(buffer, 0, 0x1000)) > 0)
                {
                    if (!this.Save126Address(buffer, num))
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
                byte[] buffer = new byte[4096];
                base.m_emailno++;
                this.m_emailTime = DateTime.Now.ToString("yyyMMdd-HHmmss") + DateTime.Now.Millisecond.ToString();
                while ((num = responseStream.Read(buffer, 0, 4096)) > 0)
                {
                    if (Encoding.Default.GetString(buffer).IndexOf("<?xml version=") > -1)
                    {
                        return false;
                    }
                    if (!this.Save126mail(buffer, num))
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
        private bool Save126Address(byte[] buffer, int nbytes)
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
                stream = File.Open(path + @"\" + base.m_username + "@" + base.m_serv + "_address.csv", FileMode.Create, FileAccess.Write);
                stream.Write(buffer, 0, nbytes);
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
        public bool Save126mail(byte[] buffer, int nbytes)
        {
            try
            {
                string filePath = "";
                if ((GlobalValue.PopMainForm.saveFilePath != "") | (GlobalValue.PopMainForm.saveFilePath != null))
                {
                    filePath = GlobalValue.PopMainForm.saveFilePath;
                }
                this.Save126mailText(buffer, nbytes, filePath);
            }
            catch (Exception exception)
            {
                base.ShowMessage("保存邮件失败：" + exception.Message);
                return false;
            }
            return true;
        }
        public void Save126mailText(byte[] buffer, int nbytes, string filePath)
        {
            FileStream stream = null;
            try
            {
                string str = ".eml";
                if (filePath == null)
                {
                    filePath = "";
                }
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

        // Nested Types
        [StructLayout(LayoutKind.Sequential)]
        private struct BoxNameId
        {
            public string boxname;
            public string boxid;
            public int mailCount;
        }

    }
}
