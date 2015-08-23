using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Runtime.InteropServices;

namespace MailExposure.PopMailRC
{
    class MailHNReceiver:MailStr
    {
        // Fields
        private string Annex;
        private BoxNameId[] boxList;
        public string m_emailTime;
        private static object pob;
        private string url;
        private string urls;

        // Methods
        static MailHNReceiver()
        {
            pob = new object();
        }
        public MailHNReceiver()
        {
            this.Annex = "";
            this.boxList = new BoxNameId[200];
            this.urls = "";
            this.url = "";
        }
        private void getAttachment(string strEmail)
        {
            for (int i = 0; (i = strEmail.IndexOf("id=\"lnkAtmt\"", i)) != -1; i++)
            {
                this.Annex = "附件";
                string str = "";
                string str2 = "";
                str2 = base.putstr(strEmail, "href=\"", "\"", i);
                str = base.putstr(strEmail, "title=\"", "\"", i);
                if (((str2 != "-1") && (str2 != "")) && ((str != "-1") && (str != "")))
                {
                    base.AttName = str;
                    base.streamControl = true;
                    this.RequestEmail("https://mailhn.pvep.com.vn/owa/" + str2);
                }
            }
            this.Annex = "";
        }
        private void getBoxName(string strMessage)
        {
            int startIndex = 0;
            int index = 0;
            try
            {
                while ((startIndex = strMessage.IndexOf("<a name=\"lnkFldr\"", startIndex)) != -1)
                {
                    string str = base.putstr(strMessage, "href=\"", "\"", startIndex).Trim();
                    string str2 = base.putstr(strMessage, "title=\"", "\"", startIndex).Trim();
                    if (((str != "-1") && (str2 != "-1")) && ((str != "") && (str2 != "")))
                    {
                        this.boxList[index].boxid = str;
                        this.boxList[index].boxname = str2;
                        index++;
                    }
                    startIndex++;
                }
                for (int i = 0; i < 5; i++)
                {
                    base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                    base.streamControl = true;
                    base.MyStringBuilder = this.Request(base.Host + this.boxList[i].boxid);
                    if (base.MyStringBuilder.ToString() != "")
                    {
                        base.BoxName = this.boxList[i].boxname;
                        this.getPages(base.MyStringBuilder.ToString(), base.Host + this.boxList[i].boxid, 1);
                    }
                }
            }
            catch (Exception exception)
            {
                base.ShowMessage("取箱子失败!" + exception.Message);
            }
        }
        private void getPages(string strboxMessage, string boxUrl, int intpages)
        {
            int startIndex = 0;
            int index = 0;
            string str = "";
            index = strboxMessage.IndexOf("name=\"hidcanary\"", index);
            if (index != -1)
            {
                str = base.putstr(strboxMessage, "value=\"", "\">", index);
            }
            base.ShowMessage(string.Concat(new object[] { base.BoxName, ":第", intpages, "页开始下载" }));
            while ((startIndex = strboxMessage.IndexOf("<img class=\"sI\"", startIndex)) != -1)
            {
                bool flag = false;
                if (base.putstr(strboxMessage, "alt=\"", "\">", startIndex).IndexOf("Unread") != -1)
                {
                    flag = true;
                }
                if ((str != "") && (str != "-1"))
                {
                    string str3 = base.putstr(strboxMessage, "value=\"", "\"", startIndex);
                    string url = "https://mailhn.pvep.com.vn/owa/?ae=Item&t=IPM.Note&id=" + str3;
                    string strSql = "select count(*) from MailHNId where MsgId='" + str3 + "'";
                    if (Convert.ToInt32(GlobalValue.PopMainForm.ExecuteSQL(strSql)) == 0)
                    {
                        base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                        base.streamControl = true;
                        base.MyStringBuilder = this.Request(url);
                        this.m_emailTime = DateTime.Now.ToString("yyyMMdd-HHmmss") + DateTime.Now.Millisecond.ToString();
                        this.getAttachment(base.MyStringBuilder.ToString());
                        string emailText = base.MyStringBuilder.ToString().Replace("display:none;", "");
                        this.SaveMailHN(emailText);
                        if (flag)
                        {
                            string str7 = boxUrl + "&pg=" + intpages.ToString();
                            string indata = "hidpnst=&hidactbrfld=&chkmsg=" + str3 + "&hidcmdpst=markunread&hidcid=&hidso=&hidpid=MessageView&hidcanary=" + str;
                            base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                            base.streamControl = true;
                            base.MyStringBuilder = this.PostData(str7, indata);
                            base.ShowMessage("置未读：" + str3 + "\t");
                        }
                        else
                        {
                            base.ShowMessage("已读:" + str3 + "\t");
                        }
                        try
                        {
                            string str9 = DateTime.Now.ToString();
                            strSql = "insert into MailHNId(Name,MsgId,DownTime,MailType) values('" + base.m_username + "','" + str3 + "','" + str9 + "','" + base.BoxName.Substring(0, base.BoxName.Length % 10) + "')";
                            GlobalValue.PopMainForm.ExecuteSQL(strSql);
                        }
                        catch (Exception exception)
                        {
                            base.ShowMessage("添加失败：" + exception.Message);
                        }
                    }
                }
                startIndex++;
            }
            if (strboxMessage.IndexOf("src=\"8.1.393.1/themes/base/np.gif") != -1)
            {
                intpages++;
                if (Convert.ToInt32(GlobalValue.PopMainForm.PageNumber) == 0)
                {
                    string str10 = boxUrl + "&pg=" + intpages.ToString();
                    base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                    base.streamControl = true;
                    base.MyStringBuilder = this.Request(str10);
                    this.getPages(base.MyStringBuilder.ToString(), boxUrl, intpages);
                }
                else if (intpages <= Convert.ToInt32(GlobalValue.PopMainForm.PageNumber))
                {
                    string str11 = boxUrl + "&pg=" + intpages.ToString();
                    base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                    base.streamControl = true;
                    base.MyStringBuilder = this.Request(str11);
                    this.getPages(base.MyStringBuilder.ToString(), boxUrl, intpages);
                }
            }
        }
        public override void login()
        {
            base.ShowMessage("开始登陆……");
            base.Host = "https://mailhn.pvep.com.vn/owa/";
            this.url = "https://mailhn.pvep.com.vn/owa/auth/owaauth.dll";
            string indata = "";
            indata = "destination=https%3A%2F%2Fmailhn.pvep.com.vn%2Fowa%2F&flags=5&forcedownlevel=0&trusted=4&username=" + base.m_username + "&password=" + base.m_passwd + "&isUtf8=1";
            base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
            base.streamControl = true;
            base.MyStringBuilder = this.PostData(this.url, indata);
            string strMessage = base.MyStringBuilder.ToString();
            if (strMessage.IndexOf("inbox") != -1)
            {
                base.ShowMessage("登陆成功!");
                this.getBoxName(strMessage);
            }
            else if (strMessage.IndexOf("<tr id=\"trInvCrd\" class=\"wrng\">") != -1)
            {
                base.ShowMessage("用户名或密码错误!");
                base.LoginFail();
                base.passwdErr();
                base.ShowMessage("登陆失败!");
            }
            else
            {
                base.LoginFail();
                base.passwdErr();
                base.ShowMessage("登陆失败!");
            }
        }
        public StringBuilder PostData(string url, string indata)
        {
            StreamReader reader = null;
            try
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(MailHNReceiver.ValidateServerCertificate);
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
                base.ShowMessage("MailHNpost:" + exception.Message);
            }
            reader.Close();
            return base.MyStringBuilder;

        }
        private StringBuilder Request(string url)
        {
            StreamReader reader = null;
            try
            {
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
            }
            catch (Exception exception)
            {
                base.ShowMessage("MailHNrequest:" + exception.Message + url);
            }
            reader.Close();
            return base.MyStringBuilder;

        }
        private bool RequestEmail(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Headers.Add("Cookie", base.cookie);
            request.Method = "GET";
            request.Accept = "*/*";
            request.AllowAutoRedirect = false;
            request.KeepAlive = true;
            request.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; .NET CLR 2.0.50727)";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Headers.Add("Accept-Language: zh-cn");
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Thread.Sleep(10);
            WebHeaderCollection headers = response.Headers;
            if (((response.StatusCode == HttpStatusCode.Found) || (response.StatusCode == HttpStatusCode.MovedPermanently)) || ((response.StatusCode == HttpStatusCode.MovedPermanently) || (response.StatusCode == HttpStatusCode.Found)))
            {
                string str = "";
                str = headers["location"];
                if (str.IndexOf("http") != -1)
                {
                    this.RequestEmail(str);
                }
            }
            Stream responseStream = response.GetResponseStream();
            if (base.streamControl)
            {
                int num;
                byte[] buffer = new byte[0x400];
                if (!(this.Annex == "附件"))
                {
                    base.m_emailno++;
                }
                while ((num = responseStream.Read(buffer, 0, 0x400)) > 0)
                {
                    if (!this.SaveMailHN(buffer, num))
                    {
                        responseStream.Close();
                        response.Close();
                        return false;
                    }
                }
                responseStream.Close();
                response.Close();
                base.saveEmailCount();
                GlobalValue.PopMainForm.statusBarPanelEmailNumbersText.Text = (Convert.ToInt32(GlobalValue.PopMainForm.statusBarPanelEmailNumbersText.Text) + 1).ToString();
                base.streamControl = false;
                return true;
            }
            responseStream.Close();
            response.Close();
            return false;

        }
        private void SaveMailHN(string EmailText)
        {
            string filePath = "";
            try
            {
                if ((GlobalValue.PopMainForm.saveFilePath != "") | (GlobalValue.PopMainForm.saveFilePath != null))
                {
                    filePath = GlobalValue.PopMainForm.saveFilePath;
                }
                this.SaveMailHNText(EmailText, filePath);
            }
            catch (Exception exception)
            {
                base.ShowMessage("保存邮件失败：" + exception.Message + filePath);
                return;
            }
            try
            {
                base.saveEmailCount();
                GlobalValue.PopMainForm.statusBarPanelEmailNumbersText.Text = (Convert.ToInt32(GlobalValue.PopMainForm.statusBarPanelEmailNumbersText.Text) + 1).ToString();
            }
            catch (Exception exception2)
            {
                base.ShowMessage(exception2.Message);
            }

        }
        private bool SaveMailHN(byte[] buffer, int nbytes)
        {
            string filePath = "";
            try
            {
                if ((GlobalValue.PopMainForm.saveFilePath != "") | (GlobalValue.PopMainForm.saveFilePath != null))
                {
                    filePath = GlobalValue.PopMainForm.saveFilePath;
                }
                this.SaveMailHNText(buffer, nbytes, filePath);
            }
            catch (Exception exception)
            {
                base.ShowMessage("保存邮件失败：" + exception.Message + filePath);
                return false;
            }
            return true;

        }
        public void SaveMailHNText(string EmailText, string filePath)
        {
            Stream stream;
            string str = "";
            base.m_emailno++;
            string str2 = ".html";
            string str3 = DateTime.Now.Date.ToString("yyy-MM-dd");
            filePath = filePath + str3 + "邮件";
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
            string str4 = filePath;
            filePath = str4 + @"\" + base.m_username + "@" + base.m_serv;
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            filePath = filePath + @"\" + base.BoxName;
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            try
            {
                str = filePath;
                filePath = filePath + @"\" + this.m_emailTime + str2;
                stream = File.Create(filePath);
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(EmailText);
                }
                stream.Close();
                stream = null;
            }
            catch (Exception)
            {
                filePath = string.Concat(new object[] { str, @"\", base.m_emailno, str2 });
                stream = File.Create(filePath);
                using (StreamWriter writer2 = new StreamWriter(stream))
                {
                    writer2.Write(EmailText);
                }
                stream.Close();
                stream = null;
            }

        }
        public void SaveMailHNText(byte[] buffer, int nbytes, string filePath)
        {
            lock (pob)
            {
                FileStream stream = null;
                try
                {
                    string attName = ".html";
                    if (this.Annex == "附件")
                    {
                        attName = base.AttName;
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
                    filePath = filePath + @"\" + this.m_emailTime + "-" + attName;
                    stream = File.Open(filePath, FileMode.Append, FileAccess.Write);
                    stream.Write(buffer, 0, nbytes);
                    stream.Close();
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
        public static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
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
