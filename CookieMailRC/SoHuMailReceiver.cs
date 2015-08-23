using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Runtime.InteropServices;
using System.Net;
using System.IO;
using System.Globalization;
using System.Threading;
using System.Text.RegularExpressions;

namespace MailExposure.CookieMailRC
{
    class SoHuMailReceiver:MailStr
    {
        // Fields
        private BoxInfo[] boxList;
        private string url;
        private string urls;

        // Methods
        public SoHuMailReceiver()
        {
            this.url = "";
            this.urls = "";
            this.boxList = new BoxInfo[200];

        }
        private void getBoxName()
        {
            int index = 0;
            int startIndex = 0;
            this.url = this.urls.Replace("main", "folder");
            base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
            base.streamControl = true;
            base.MyStringBuilder = this.Request(this.url);
            string message = base.MyStringBuilder.ToString();
            while ((startIndex = message.IndexOf("folder_id", startIndex)) != -1)
            {
                string str2 = base.putstr(message, "folder_id\":", ",", startIndex).Trim();
                string str = base.putstr(message, "\"name\": \"", "\"}", startIndex).Trim();
                if (((str2 != "-1") && (str != "-1")) && ((str2 != "") && (str != "")))
                {
                    this.boxList[index].boxid = str2;
                    str = this.UtoCnCode(str);
                    this.boxList[index].boxname = str;
                    index++;
                }
                startIndex++;
            }
            for (int i = 0; i < index; i++)
            {
                this.getEmailId(this.boxList[i].boxname, this.boxList[i].boxid);
            }

        }
        private void getEmailId(string boxName, string boxId)
        {
            string str = "";
            try
            {
                base.BoxName = boxName;
                int startIndex = 0;
                this.url = this.url + "/" + boxId.Trim() + "?sortMode=3&sortDirection=true&startIndex=0&length=1000";
                base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                base.streamControl = true;
                base.MyStringBuilder = this.Request(this.url);
                string message = base.MyStringBuilder.ToString();
                while ((startIndex = message.IndexOf("{\"status\":", startIndex)) != -1)
                {
                    base.putstr(message, "⋚te\":", ",", startIndex).Trim();
                    base.putstr(message, "\"size\":", ",", startIndex).Trim();
                    str = base.putstr(message, "\"id\":", ",", startIndex).Trim();
                    this.url = "http://mail.sohu.com/bapp/50/mail/" + str + "?disp_source=2";
                    base.streamControl = true;
                    this.RequestEmail(this.url);
                    startIndex++;
                }
            }
            catch (Exception exception)
            {
                base.ShowMessage("邮件下载失败：" + exception.Message);
            }

        }
        public override void login()
        {
            base.cookie = "";
            base.ShowMessage("开始登陆……");
            string str = this.PasswdMd5(base.m_passwd);
            this.url = "http://passport.sohu.com/sso/login.jsp?userid=" + base.m_username + "%40sohu.com&password=" + str + "&appid=1000&persistentcookie=0&s=1221027794515&b=2&w=1024&pwdtype=1";
            base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
            base.streamControl = true;
            base.MyStringBuilder = this.Request(this.url);
            if (base.MyStringBuilder.ToString().IndexOf("login_status='success'") != -1)
            {
                this.url = "http://login.mail.sohu.com/servlet/LoginServlet";
                base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                base.streamControl = true;
                base.MyStringBuilder = this.Request(this.url);
                if (this.urls.IndexOf("http://mail.sohu.com/bapp/") != -1)
                {
                    base.Host = "http://mail.sohu.com/bapp/50/main";
                    if (base.MyStringBuilder.ToString().IndexOf("@sohu.com") != -1)
                    {
                        base.ShowMessage("登陆成功!");
                        this.getBoxName();
                    }
                }
            }

        }
        private string PasswdMd5(string passwd)
        {
            string str = "";
            byte[] buffer = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(passwd));
            for (int i = 0; i < buffer.Length; i++)
            {
                str = str + buffer[i].ToString("x");
            }
            return str;
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
                while ((num = responseStream.Read(buffer, 0, 0x1000)) > 0)
                {
                    if (!this.SaveSoHumail(buffer, num))
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
        public bool SaveSoHumail(byte[] buffer, int nbytes)
        {
            try
            {
                string filePath = "";
                if ((GlobalValue.mainForm.saveFilePath != "") | (GlobalValue.mainForm.saveFilePath != null))
                {
                    filePath = GlobalValue.mainForm.saveFilePath;
                }
                this.SaveSoHumailText(buffer, nbytes, filePath);
            }
            catch (Exception exception)
            {
                base.ShowMessage("保存邮件失败：" + exception.Message);
                return false;
            }
            return true;

        }
        public void SaveSoHumailText(byte[] buffer, int nbytes, string filePath)
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
                filePath = string.Concat(new object[] { filePath, @"\", base.BoxName, '-', Convert.ToInt32(base.m_emailno), str });
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
        private string UtoCnCode(string str)
        {
            try
            {
                Regex regex = new Regex(@"(?<code>\\u[a-z0-9]{4})", RegexOptions.IgnoreCase);
                for (Match match = regex.Match(str); match.Success; match = match.NextMatch())
                {
                    string oldValue = match.Result("${code}");
                    int num = int.Parse(oldValue.Substring(2, 4), NumberStyles.HexNumber);
                    string newValue = string.Format("{0}", (char)num);
                    str = str.Replace(oldValue, newValue);
                }
            }
            catch (Exception exception)
            {
                base.ShowMessage("Unicode失败：" + exception.Message);
            }
            return str;

        }

        // Nested Types
        [StructLayout(LayoutKind.Sequential)]
        private struct BoxInfo
        {
            public string boxname;
            public string boxid;
        }
    }
}
