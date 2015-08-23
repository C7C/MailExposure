using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Threading;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace MailExposure.PopMailRC
{
    class Mail263Receiver:MailStr
    {
        // Fields
        private BoxNameId[] boxList;
        private CookieContainer cookieContainer;
        private bool download;
        private int emailCount;
        private NetworkStream netWorkStream;
        private string request;
        private byte[] requestData;
        private string response;
        private string SID;
        private string SQL;
        private StreamReader streamReader;
        private TcpClient tcpServer;
        private string url;
        private string urls;
        private string UUID;

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
                this.login263Mail();
                for (int i = 2; i < index; i++)
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
                                    if (Convert.ToInt32(GlobalValue.PopMainForm.ExecuteSQL(strSql)) == 0)
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
                                            GlobalValue.PopMainForm.ExecuteSQL(strSql);
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
        private int GetEmailnum(string mess)
        {
            return Convert.ToInt32(mess.Split(new char[] { ' ' })[1]);
        }
        private string GetUUID(string mess)
        {
            string[] strArray = mess.Split(new char[] { ' ' });
            if (base.m_serv.IndexOf("sohu.com") != -1)
            {
                return strArray[1];
            }
            return strArray[2];
        }
        public override void login()
        {
            this.cookieContainer = new CookieContainer();
            int startIndex = 0;
            base.ShowMessage("开始登陆…………");
            this.url = "http://mailbeta.263.net/xmweb";
            string indata = "usr=" + base.m_username + "&sel_domain=" + base.m_serv + "&domain=" + base.m_serv + "&func=login&pass=" + base.m_passwd + "&submitnew=";
            base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
            base.streamControl = true;
            base.MyStringBuilder = this.PostData(this.url, indata);
            startIndex = base.MyStringBuilder.ToString().IndexOf("window.location.replace");
            if (startIndex != -1)
            {
                this.url = base.putstr(base.MyStringBuilder.ToString(), "\"", "\"", startIndex);
                if ((this.url != "-1") && (this.url != null))
                {
                    base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                    base.streamControl = true;
                    base.MyStringBuilder = this.Request(this.url);
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
                    if ((base.Host != null) && (this.SID != ""))
                    {
                        base.ShowMessage("登陆成功.....");
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
                }
            }
        }
        public void login263Mail()
        {
            base.BoxName = "收件箱";
            try
            {
                this.tcpServer = new TcpClient("pop.263.net", 110);
            }
            catch (Exception)
            {
                return;
            }
            this.netWorkStream = this.tcpServer.GetStream();
            this.streamReader = new StreamReader(this.netWorkStream, Encoding.GetEncoding("GB2312"));
            this.response = this.streamReader.ReadLine();
            if (this.response.IndexOf("+OK") != -1)
            {
                try
                {
                    this.request = "USER " + base.m_username + "\r\n";
                    this.SendRequest(this.request);
                    this.response = this.streamReader.ReadLine();
                    this.request = "PASS " + base.m_passwd + "\r\n";
                    this.SendRequest(this.request);
                    this.response = this.streamReader.ReadLine();
                }
                catch (Exception)
                {
                    base.LoginFail();
                    base.passwdErr();
                    return;
                }
                if (this.response.StartsWith("+OK"))
                {
                    try
                    {
                        this.request = "STAT\r\n";
                        this.SendRequest(this.request);
                        this.response = this.streamReader.ReadLine();
                        if (this.response.StartsWith("+OK"))
                        {
                            this.emailCount = this.GetEmailnum(this.response);
                        }
                        else
                        {
                            return;
                        }
                    }
                    catch (Exception)
                    {
                        return;
                    }
                    for (int i = 1; i <= this.emailCount; i++)
                    {
                        this.download = false;
                        try
                        {
                            base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                            this.request = "UIDL " + i + "\r\n";
                            this.SendRequest(this.request);
                            this.response = this.streamReader.ReadLine();
                            if (this.response != null)
                            {
                                this.UUID = this.GetUUID(this.response);
                            }
                            else
                            {
                                continue;
                            }
                            this.SQL = "select count(*) from 263mailId where MsgId='" + this.UUID + "'";
                            if (Convert.ToInt32(GlobalValue.PopMainForm.ExecuteSQL(this.SQL)) > 0)
                            {
                                continue;
                            }
                            this.request = "RETR " + i + "\r\n";
                            this.SendRequest(this.request);
                            this.response = this.streamReader.ReadLine();
                            if (this.response.StartsWith("+OK"))
                            {
                                while (this.response != ".")
                                {
                                    this.response = null;
                                    this.response = this.streamReader.ReadLine();
                                    if (this.response != null)
                                    {
                                        base.MyStringBuilder.Append(this.response + "\r\n");
                                    }
                                    else
                                    {
                                        base.ShowMessage(base.BoxName + ":" + this.UUID + "下载失败");
                                        this.download = true;
                                        break;
                                    }
                                }
                            }
                            if (this.download)
                            {
                                break;
                            }
                            this.Save263Email(base.MyStringBuilder.ToString());
                            base.ShowMessage(base.BoxName + ":" + this.UUID + " 下载");
                            try
                            {
                                string str = DateTime.Now.ToString();
                                this.SQL = "insert into 263mailId(Name,MsgId,DownTime,MailType) values('" + base.m_username + "','" + this.UUID + "','" + str + "','" + base.BoxName.Substring(0, base.BoxName.Length % 10) + "')";
                                GlobalValue.PopMainForm.ExecuteSQL(this.SQL);
                            }
                            catch (Exception exception)
                            {
                                base.ShowMessage("添加失败：" + exception.Message);
                            }
                        }
                        catch (Exception exception2)
                        {
                            base.ShowMessage(exception2.Message);
                        }
                    }
                    try
                    {
                        if (!this.download && (base.m_serv.ToLower().Trim().IndexOf("pop.sohu.com") < 0))
                        {
                            this.request = "QUIT\r\n";
                            this.SendRequest(this.request);
                            this.response = this.streamReader.ReadLine();
                            this.response.StartsWith("+OK");
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }
        public StringBuilder PostData(string url, string indata)
        {
            try
            {
                StreamReader reader;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.CookieContainer = new CookieContainer();
                request.ServicePoint.Expect100Continue = false;
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.Headers.Add("Pragma:no-cache\r\n");
                request.Accept = "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, application/vnd.ms-excel, application/msword, */*";
                request.Headers.Add("Accept-Language: en-us,ar-SA;q=0.9,de-DE;q=0.8,es-ES;q=0.7,tr-TR;q=0.6,ja-JP;q=0.5,en-GB;q=0.4,fr-FR;q=0.3,zh-CN;q=0.2,zh-TW;q=0.1");
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; TencentTraveler ; .NET CLR 1.1.4322)";
                request.ContentLength = indata.Length;
                request.AllowAutoRedirect = false;
                request.KeepAlive = true;
                request.CookieContainer = this.cookieContainer;
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
                request.CookieContainer = new CookieContainer();
                request.Method = "GET";
                request.Accept = "*/*";
                request.AllowAutoRedirect = false;
                request.ServicePoint.Expect100Continue = false;
                request.KeepAlive = true;
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1)";
                request.ContentType = "application/x-www-form-urlencoded";
                request.Headers.Add("Accept-Language: zh-cn");
                request.CookieContainer = this.cookieContainer;
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
                if ((GlobalValue.PopMainForm.saveFilePath != "") | (GlobalValue.PopMainForm.saveFilePath != null))
                {
                    filePath = GlobalValue.PopMainForm.saveFilePath;
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
                GlobalValue.PopMainForm.statusBarPanelEmailNumbersText.Text = (Convert.ToInt32(GlobalValue.PopMainForm.statusBarPanelEmailNumbersText.Text) + 1).ToString();
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
                GlobalValue.PopMainForm.ExecuteSQL(strSql);
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
            if ((GlobalValue.PopMainForm.saveFilePath != "") | (GlobalValue.PopMainForm.saveFilePath != null))
            {
                path = GlobalValue.PopMainForm.saveFilePath;
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
        private void SendRequest(string requestStr)
        {
            try
            {
                this.requestData = Encoding.ASCII.GetBytes(requestStr.ToCharArray());
                this.netWorkStream.Write(this.requestData, 0, this.requestData.Length);
            }
            catch (Exception exception)
            {
                base.ShowMessage("连接失败" + exception.Message);
                this.netWorkStream.Close();
                this.streamReader.Close();
                this.tcpServer.Close();
            }
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
