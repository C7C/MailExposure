using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Net;
using System.Threading;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Net.Sockets;

namespace MailExposure.PopMailRC
{
    class IMAPMailReceiver:MailStr
    {
        // Fields
        private ArrayList boxlist;
        private bool download;
        private string folderSelected;
        public string m_emailTime;
        private int mailCount;
        private ArrayList mailList;
        private NetworkStream netWorkStream;
        private int newMailCount;
        private const int port = 0x8f;
        private string request;
        private byte[] requestData;
        private string response;
        private string SQL;
        public const int sslPort = 0x3e1;
        private SslStream SslStream;
        private StreamReader streamReader;
        private string[] strVal;
        private TcpClient tcpServer;
        private string UUID;

        // Methods
        public IMAPMailReceiver()
        {
            this.boxlist = new ArrayList();
            this.mailList = new ArrayList();
        }
        private string Authenticate(string userName, string passWord)
        {
            this.request = "a001 LOGIN " + userName + " " + passWord + "\r\n";
            this.SendRequest(this.request);
            this.response = this.streamReader.ReadLine();
            return this.response;
        }
        private string Connect(string host, int port)
        {
            try
            {
                this.tcpServer = new TcpClient(base.m_serv, port);
            }
            catch (Exception exception)
            {
                base.ShowMessage("与服务器连接失败……" + exception.Message);
            }
            this.netWorkStream = this.tcpServer.GetStream();
            this.streamReader = new StreamReader(this.netWorkStream, Encoding.GetEncoding("GB2312"));
            return (this.response = this.streamReader.ReadLine());

        }
        private void Disconnect()
        {
            this.request = "a001 LOGOUT\r\n";
            this.SendRequest(this.request);
            this.response = this.streamReader.ReadLine();
            if (this.response.IndexOf("BYE LOGOUT") != -1)
            {
                base.ShowMessage("与服务器断开连接！");
            }
        }
        private StringBuilder GetBody(string UUID)
        {
            this.request = "a001 FETCH " + UUID + " BODY[TEXT]\r\n";
            this.SendRequest(this.request);
            this.response = this.streamReader.ReadLine();
            if (this.response.IndexOf("BODY[TEXT]") != -1)
            {
                this.response = null;
                this.response = this.streamReader.ReadLine();
                while (this.response != null)
                {
                    if (this.response.IndexOf("OK FETCH completed") == -1)
                    {
                        base.MyStringBuilder.Append(this.response + "\r\n");
                        this.response = this.streamReader.ReadLine();
                    }
                    else
                    {
                        this.response = null;
                    }
                }
            }
            else
            {
                this.download = true;
            }
            return base.MyStringBuilder;

        }
        private string[] Getfolders()
        {
            this.request = "a002 LIST \"\" \"*\"\r\n";
            this.SendRequest(this.request);
            this.response = this.streamReader.ReadLine();
            if (this.response.StartsWith("*"))
            {
                while (this.response.StartsWith("*"))
                {
                    this.response = this.response.Substring(this.response.IndexOf(")") + 1).Trim();
                    this.response = this.response.Substring(this.response.IndexOf(" ")).Trim();
                    if (this.response.IndexOf("\"") > -1)
                    {
                        this.boxlist.Add(this.response.Substring(this.response.IndexOf("\"") + 1, (this.response.Length - this.response.IndexOf("\"")) - 2));
                    }
                    else if (this.response.IndexOf(" ") > -1)
                    {
                        this.boxlist.Add(this.response.Substring(0, this.response.IndexOf(" ")).Trim());
                    }
                    else
                    {
                        this.boxlist.Add(this.response.Trim());
                    }
                    this.response = this.streamReader.ReadLine();
                }
            }
            this.response = this.response.Substring(this.response.IndexOf(" ")).Trim();
            if (!this.response.ToUpper().StartsWith("OK"))
            {
                throw new Exception("server return:" + this.response);
            }
            string[] array = new string[this.boxlist.Count];
            this.boxlist.CopyTo(array);
            return array;

        }
        private StringBuilder GetHeader(string UUID)
        {
            this.request = "a001 FETCH " + UUID + " BODY[HEADER]\r\n";
            this.SendRequest(this.request);
            this.response = this.streamReader.ReadLine();
            if (this.response.IndexOf("BODY[HEADER]") != -1)
            {
                this.response = null;
                this.response = this.streamReader.ReadLine();
                while (this.response != null)
                {
                    if (this.response.IndexOf("FETCH completed") == -1)
                    {
                        base.MyStringBuilder.Append(this.response + "\r\n");
                        this.response = this.streamReader.ReadLine();
                    }
                    else
                    {
                        this.response = null;
                    }
                }
            }
            else
            {
                this.download = true;
            }
            return base.MyStringBuilder;

        }
        public override void login()
        {
            base.ShowMessage("开始登陆......");
            if ((base.m_serv.IndexOf("gmail") != -1) || (base.m_serv.IndexOf("yahoo") != -1))
            {
                this.Ssllogin();
            }
            else
            {
                this.response = this.Connect(base.m_serv, 0x8f);
                if (this.response.IndexOf("* OK") != -1)
                {
                    base.ShowMessage("与服务器连接成功！");
                    try
                    {
                        this.response = this.Authenticate(base.m_username, base.m_passwd);
                        if (this.response.IndexOf("OK") != -1)
                        {
                            base.ShowMessage("登陆成功！");
                            this.strVal = this.Getfolders();
                            foreach (string str in this.strVal)
                            {
                                if (str != "")
                                {
                                    this.mailCount = this.SelectFolder(str);
                                    this.folderSelected = str;
                                    for (int i = 1; i <= this.mailCount; i++)
                                    {
                                        this.download = false;
                                        try
                                        {
                                            base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                                            this.UUID = Convert.ToString(i);
                                            this.SQL = "select count(*) from ImapmailID where MsgId='" + this.UUID + "'and Name='" + base.m_username + "' and MailType='" + str + "'";
                                            if (Convert.ToInt32(GlobalValue.PopMainForm.ExecuteSQL(this.SQL)) <= 0)
                                            {
                                                base.MyStringBuilder = this.GetHeader(this.UUID);
                                                if (this.download)
                                                {
                                                    break;
                                                }
                                                base.MyStringBuilder = this.GetBody(this.UUID);
                                                if (this.download)
                                                {
                                                    break;
                                                }
                                                this.m_emailTime = DateTime.Now.ToString("yyyMMdd-HHmmss") + DateTime.Now.Millisecond.ToString();
                                                this.SaveImapEmail(base.MyStringBuilder.ToString());
                                                base.ShowMessage(this.folderSelected + "第" + i.ToString("00") + "封:" + this.UUID + " 下载完毕");
                                                try
                                                {
                                                    string str2 = DateTime.Now.ToString();
                                                    this.SQL = "insert into ImapmailID (Name,MsgId,DownTime,MailType) values";
                                                    string str3 = "('" + base.m_username + "','" + this.UUID + "','" + str2 + "','" + this.folderSelected + "')";
                                                    this.SQL = this.SQL + str3;
                                                    GlobalValue.PopMainForm.ExecuteSQL(this.SQL);
                                                }
                                                catch (Exception exception)
                                                {
                                                    base.ShowMessage("添加失败！" + exception.Message);
                                                    return;
                                                }
                                            }
                                        }
                                        catch (Exception exception2)
                                        {
                                            base.ShowMessage("添加失败" + exception2.Message);
                                            return;
                                        }
                                    }
                                }
                                else
                                {
                                    base.ShowMessage("取箱子失败！");
                                    return;
                                }
                            }
                        }
                        else
                        {
                            base.ShowMessage("登陆失败……");
                            this.Disconnect();
                        }
                    }
                    catch (Exception exception3)
                    {
                        base.ShowMessage("登陆失败……" + exception3.Message);
                    }
                }
            }

        }
        public void saveEmailCount()
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
        public void SaveImapEmail(string EmailText)
        {
            try
            {
                string filePath = "";
                if ((GlobalValue.PopMainForm.saveFilePath != "") | (GlobalValue.PopMainForm.saveFilePath != null))
                {
                    filePath = GlobalValue.PopMainForm.saveFilePath;
                }
                this.SaveIMAPFile(EmailText, filePath);
            }
            catch (Exception exception)
            {
                base.ShowMessage("保存邮件失败：" + exception.Message);
                return;
            }
            try
            {
                this.saveEmailCount();
                GlobalValue.PopMainForm.statusBarPanelEmailNumbersText.Text = (Convert.ToInt32(GlobalValue.PopMainForm.statusBarPanelEmailNumbersText.Text) + 1).ToString();
            }
            catch (Exception exception2)
            {
                base.ShowMessage(exception2.Message);
            }
        }
        public void SaveIMAPFile(string EmailText, string filePath)
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
            string str3 = filePath;
            filePath = str3 + @"\" + base.m_username + "@" + base.m_serv;
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            filePath = filePath + @"\" + this.folderSelected;
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            Stream stream = File.Create(filePath + @"\" + this.m_emailTime + str);
            using (StreamWriter writer = new StreamWriter(stream))
            {
                writer.Write(EmailText);
            }
            stream.Close();
            stream = null;
        }
        private int SelectFolder(string folderName)
        {
            int num = 0;
            this.request = "a001 SELECT \"" + folderName + "\"\r\n";
            this.SendRequest(this.request);
            this.response = this.streamReader.ReadLine();
            if (this.response.StartsWith("*"))
            {
                while (this.response.StartsWith("*"))
                {
                    this.response = this.response.Substring(1).Trim();
                    if (this.response.ToUpper().IndexOf("EXISTS") != -1)
                    {
                        num = Convert.ToInt32(this.response.Substring(0, this.response.IndexOf(" ")).Trim());
                    }
                    if (this.response.ToUpper().IndexOf("RECENT") != -1)
                    {
                        this.newMailCount = Convert.ToInt32(this.response.Substring(0, this.response.IndexOf(" ")).Trim());
                    }
                    this.response = this.streamReader.ReadLine();
                }
            }
            this.response = this.response.Substring(this.response.IndexOf(" ")).Trim();
            if (this.response.ToUpper().IndexOf("OK") == -1)
            {
                throw new Exception("server return" + this.response);
            }
            return num;
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
        private void SendSslRequest(string requestStr)
        {
            try
            {
                this.requestData = Encoding.ASCII.GetBytes(requestStr.ToCharArray());
                this.SslStream.Write(this.requestData, 0, this.requestData.Length);
            }
            catch (Exception exception)
            {
                base.ShowMessage("连接失败" + exception.Message);
                this.SslStream.Close();
                this.streamReader.Close();
                this.tcpServer.Close();
            }
        }
        private string SslAuthenticate(string userName, string passWord)
        {
            this.request = "a001 LOGIN " + userName + "@gmail.com " + passWord + "\r\n";
            this.SendSslRequest(this.request);
            this.response = this.streamReader.ReadLine();
            return this.response;
        }
        private string SslConnect(string host, int sslPort)
        {
            try
            {
                this.tcpServer = new TcpClient(base.m_serv, sslPort);
            }
            catch (Exception exception)
            {
                base.ShowMessage("与服务器连接失败……" + exception.Message);
            }
            this.SslStream = new SslStream(this.tcpServer.GetStream());
            this.SslStream.AuthenticateAsClient(base.m_serv);
            this.streamReader = new StreamReader(this.SslStream, Encoding.GetEncoding("GB2312"));
            return (this.response = this.streamReader.ReadLine());

        }
        private void SslDisconnect()
        {
            this.request = "a001 LOGOUT\r\n";
            this.SendSslRequest(this.request);
            this.response = this.streamReader.ReadLine();
            if (this.response.IndexOf("BYE LOGOUT") != -1)
            {
                base.ShowMessage("与服务器断开连接！");
            }

        }
        private StringBuilder SslGetBody(string UUID)
        {
            this.request = "a001 FETCH " + UUID + " BODY[TEXT]\r\n";
            this.SendSslRequest(this.request);
            this.response = this.streamReader.ReadLine();
            if (this.response.IndexOf("BODY[TEXT]") != -1)
            {
                this.response = null;
                this.response = this.streamReader.ReadLine();
                while (this.response != null)
                {
                    if (this.response.IndexOf("a001 OK") == -1)
                    {
                        base.MyStringBuilder.Append(this.response + "\r\n");
                        this.response = this.streamReader.ReadLine();
                    }
                    else
                    {
                        this.response = null;
                    }
                }
            }
            else
            {
                this.download = true;
            }
            return base.MyStringBuilder;

        }
        private string[] SslGetfolders()
        {
            this.request = "a002 LIST \"\" \"*\"\r\n";
            this.SendSslRequest(this.request);
            this.response = this.streamReader.ReadLine();
            if (this.response.StartsWith("*"))
            {
                while (this.response.StartsWith("*"))
                {
                    this.response = this.response.Substring(this.response.IndexOf(")") + 1).Trim();
                    this.response = this.response.Substring(this.response.IndexOf(" ")).Trim();
                    if (this.response.IndexOf("\"") > -1)
                    {
                        if (this.response.IndexOf("[Gmail]") != -1)
                        {
                            string str = "";
                            str = base.putstr(this.response, "[Gmail]/", "\"", 0);
                            if ((str != "") && (str != "-1"))
                            {
                                this.boxlist.Add(this.response.Substring(this.response.IndexOf("\"") + 1, (this.response.Length - this.response.IndexOf("\"")) - 2));
                                goto Label_01CB;
                            }
                            this.response = this.streamReader.ReadLine();
                            continue;
                        }
                        this.boxlist.Add(this.response.Substring(this.response.IndexOf("\"") + 1, (this.response.Length - this.response.IndexOf("\"")) - 2));
                    }
                    else
                    {
                        this.boxlist.Add(this.response.Substring(this.response.IndexOf(" ")).Trim());
                    }
                Label_01CB:
                    this.response = this.streamReader.ReadLine();
                }
            }
            this.response = this.response.Substring(this.response.IndexOf(" ")).Trim();
            if (!this.response.ToUpper().StartsWith("OK"))
            {
                throw new Exception("server return:" + this.response);
            }
            string[] array = new string[this.boxlist.Count];
            this.boxlist.CopyTo(array);
            return array;
        }
        private StringBuilder SslGetHeader(string UUID)
        {
            this.request = "a001 FETCH " + UUID + " BODY[HEADER]\r\n";
            this.SendSslRequest(this.request);
            this.response = this.streamReader.ReadLine();
            if (this.response.IndexOf("BODY[HEADER]") != -1)
            {
                this.response = null;
                this.response = this.streamReader.ReadLine();
                while (this.response != null)
                {
                    if (this.response.IndexOf("a001 OK") == -1)
                    {
                        base.MyStringBuilder.Append(this.response + "\r\n");
                        this.response = this.streamReader.ReadLine();
                    }
                    else
                    {
                        this.response = null;
                    }
                }
            }
            else
            {
                this.download = true;
            }
            return base.MyStringBuilder;
        }
        private void Ssllogin()
        {
            this.response = this.SslConnect(base.m_serv, 0x3e1);
            if (this.response.IndexOf("* OK") != -1)
            {
                base.ShowMessage("与服务器连接成功！");
                try
                {
                    this.response = this.SslAuthenticate(base.m_username, base.m_passwd);
                    if (this.response.IndexOf("CAPABILITY") != -1)
                    {
                        base.ShowMessage("登陆成功！");
                        this.strVal = this.SslGetfolders();
                        if (this.strVal.Length == 0)
                        {
                            this.strVal = this.SslGetfolders();
                        }
                        foreach (string str in this.strVal)
                        {
                            if (str != "")
                            {
                                this.mailCount = this.SslSelectFolder(str);
                                this.folderSelected = str;
                                for (int i = 1; i <= this.mailCount; i++)
                                {
                                    this.download = false;
                                    try
                                    {
                                        base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                                        this.UUID = Convert.ToString(i);
                                        this.SQL = "select count(*) from ImapmailID where MsgId='" + this.UUID + "'and Name='" + base.m_username + "'and MailType='" + this.folderSelected + "'";
                                        if (Convert.ToInt32(GlobalValue.PopMainForm.ExecuteSQL(this.SQL)) <= 0)
                                        {
                                            base.MyStringBuilder = this.SslGetHeader(this.UUID);
                                            if (this.download)
                                            {
                                                break;
                                            }
                                            base.MyStringBuilder = this.SslGetBody(this.UUID);
                                            if (this.download)
                                            {
                                                break;
                                            }
                                            this.m_emailTime = DateTime.Now.ToString("yyyMMdd-HHmmss") + DateTime.Now.Millisecond.ToString();
                                            this.SaveImapEmail(base.MyStringBuilder.ToString());
                                            base.ShowMessage(this.folderSelected + "第" + i.ToString("00") + "封:" + this.UUID + " 下载完毕");
                                            try
                                            {
                                                string str2 = DateTime.Now.ToString();
                                                this.SQL = "insert into ImapmailID (Name,MsgId,DownTime,MailType) values";
                                                string str3 = "('" + base.m_username + "','" + this.UUID + "','" + str2 + "','" + this.folderSelected + "')";
                                                this.SQL = this.SQL + str3;
                                                GlobalValue.PopMainForm.ExecuteSQL(this.SQL);
                                            }
                                            catch (Exception exception)
                                            {
                                                base.ShowMessage("添加失败！" + exception.Message);
                                                return;
                                            }
                                        }
                                    }
                                    catch (Exception exception2)
                                    {
                                        base.ShowMessage("添加失败" + exception2.Message);
                                        return;
                                    }
                                }
                            }
                            else
                            {
                                base.ShowMessage("取箱子失败！");
                                return;
                            }
                        }
                    }
                    else
                    {
                        base.ShowMessage("登陆失败……");
                        this.SslDisconnect();
                    }
                }
                catch (Exception exception3)
                {
                    base.ShowMessage("登陆失败……" + exception3.Message);
                }
            }
        }
        private void SslNoop()
        {
            this.request = "a001 NOOP\r\n";
            this.SendSslRequest(this.request);
        }
        private int SslSelectFolder(string folderName)
        {
            int num = 0;
            this.request = "a001 SELECT \"" + folderName + "\"\r\n";
            this.SendSslRequest(this.request);
            this.response = this.streamReader.ReadLine();
            if (this.response.StartsWith("*"))
            {
                this.request = "a001 SELECT \"" + folderName + "\"\r\n";
                this.SendSslRequest(this.request);
                while (this.response.StartsWith("*"))
                {
                    this.response = this.response.Substring(1).Trim();
                    if (this.response.ToUpper().IndexOf("EXISTS") != -1)
                    {
                        num = Convert.ToInt32(this.response.Substring(0, this.response.IndexOf(" ")).Trim());
                    }
                    if (this.response.ToUpper().IndexOf("RECENT") != -1)
                    {
                        this.newMailCount = Convert.ToInt32(this.response.Substring(0, this.response.IndexOf(" ")).Trim());
                    }
                    this.response = this.streamReader.ReadLine();
                }
            }
            this.response = this.response.Substring(this.response.IndexOf(" ")).Trim();
            if (this.response.ToUpper().IndexOf("OK") == -1)
            {
                throw new Exception("server return" + this.response);
            }
            return num;
        }

    }
}
