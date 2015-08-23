using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net.Sockets;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace MailExposure.PopMailRC
{
    class POPMailReceiver:MailStr
    {
        // Fields
        private bool download;
        private int emailCount;
        public string m_emailTime;
        private NetworkStream netWorkStream;
        private string request;
        private byte[] requestData;
        private string response;
        private string SQL;
        private SslStream sslStream;
        private StreamReader streamReader;
        private TcpClient tcpServer;
        private string UUID;

        // Methods
        public POPMailReceiver()
        {
 
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
            base.ShowMessage("开始登陆……");
            if ((base.m_serv.IndexOf("Gmail") != -1) || (base.m_serv.IndexOf("live") != -1))
            {
                try
                {
                    this.tcpServer = new TcpClient(base.m_serv, 0x3e3);
                }
                catch (Exception exception)
                {
                    base.ShowMessage("与服务器连接失败：" + exception.Message);
                    return;
                }
                this.sslStream = new SslStream(this.tcpServer.GetStream(), false, (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)=>
                {
                    return true;
                });
                this.sslStream.AuthenticateAsClient(base.m_serv);
                this.streamReader = new StreamReader(this.sslStream, Encoding.GetEncoding("GB2312"), true);
                this.response = this.streamReader.ReadLine();
                if (this.response.IndexOf("+OK") != -1)
                {
                    base.ShowMessage("与服务器连接成功！");
                    try
                    {
                        this.request = "USER " + base.m_username + "\r\n";
                        this.SendSslRequest(this.request);
                        this.response = this.streamReader.ReadLine();
                        this.request = "PASS " + base.m_passwd + "\r\n";
                        this.SendSslRequest(this.request);
                        this.response = this.streamReader.ReadLine();
                    }
                    catch (Exception exception2)
                    {
                        base.LoginFail();
                        base.passwdErr();
                        base.ShowMessage("登陆失败：" + exception2.Message);
                        return;
                    }
                    if (this.response.StartsWith("+OK"))
                    {
                        base.ShowMessage("登陆成功！");
                        try
                        {
                            this.request = "STAT \r\n";
                            this.SendSslRequest(this.request);
                            this.response = this.streamReader.ReadLine();
                            if (this.response.StartsWith("+OK"))
                            {
                                this.emailCount = this.GetEmailnum(this.response);
                            }
                        }
                        catch (Exception exception3)
                        {
                            base.ShowMessage("取邮件数目失败！" + exception3.Message);
                            return;
                        }
                    }
                    for (int i = 1; i <= this.emailCount; i++)
                    {
                        this.download = false;
                        try
                        {
                            base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                            this.request = "UIDL " + i + "\r\n";
                            this.SendSslRequest(this.request);
                            this.response = this.streamReader.ReadLine();
                            if (this.response != null)
                            {
                                this.UUID = this.GetUUID(this.response);
                            }
                            else
                            {
                                base.ShowMessage("取邮件ID失败！");
                                continue;
                            }
                            this.SQL = "select count(*) from PmailID where MsgId='" + this.UUID + "'and Name='" + base.m_username + "'";
                            if (Convert.ToInt32(GlobalValue.PopMainForm.ExecuteSQL(this.SQL)) > 0)
                            {
                                continue;
                            }
                            this.request = "RETR " + i + "\r\n";
                            this.SendSslRequest(this.request);
                            this.response = this.streamReader.ReadLine();
                            if (this.response.StartsWith("+OK"))
                            {
                                base.ShowMessage("收件箱第" + i.ToString("00") + "封:" + this.UUID + " 开始下载");
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
                                        base.ShowMessage("收件箱第" + i.ToString("00") + "封:" + this.UUID + "下载失败");
                                        this.download = true;
                                        break;
                                    }
                                }
                            }
                            if (this.download)
                            {
                                return;
                            }
                            this.m_emailTime = DateTime.Now.ToString("yyyMMdd-HHmmss") + DateTime.Now.Millisecond.ToString();
                            this.SavePOPEmail(base.MyStringBuilder.ToString());
                            base.ShowMessage("收件箱第" + i.ToString("00") + "封:" + this.UUID + " 下载完毕");
                            try
                            {
                                string str = DateTime.Now.ToString();
                                this.SQL = "insert into PmailID (Name,MsgId,DownTime,MailType,MailLen) values";
                                string str2 = "('" + base.m_username + "','" + this.UUID + "','" + str + "','" + base.BoxName + "','" + base.m_serv + "')";
                                this.SQL = this.SQL + str2;
                                GlobalValue.PopMainForm.ExecuteSQL(this.SQL);
                            }
                            catch (Exception exception4)
                            {
                                base.ShowMessage("添加失败！" + exception4.Message);
                                return;
                            }
                        }
                        catch (Exception exception5)
                        {
                            base.ShowMessage(exception5.Message);
                        }
                    }
                }
                else
                {
                    base.LoginFail();
                    base.passwdErr();
                    base.ShowMessage("登陆失败：" + this.response);
                }
            }
            else
            {
                try
                {
                    this.tcpServer = new TcpClient(base.m_serv, 110);
                }
                catch (Exception exception6)
                {
                    base.ShowMessage("与服务器连接失败：" + exception6.Message);
                    return;
                }
                this.netWorkStream = this.tcpServer.GetStream();
                this.streamReader = new StreamReader(this.netWorkStream, Encoding.GetEncoding("GB2312"));
                this.response = this.streamReader.ReadLine();
                if (this.response.IndexOf("+OK") == -1)
                {
                    base.ShowMessage("与服务器连接失败：" + this.response);
                }
                else
                {
                    base.ShowMessage("与服务器连接成功！");
                    try
                    {
                        this.request = "USER " + base.m_username + "\r\n";
                        this.SendRequest(this.request);
                        this.response = this.streamReader.ReadLine();
                        this.request = "PASS " + base.m_passwd + "\r\n";
                        this.SendRequest(this.request);
                        this.response = this.streamReader.ReadLine();
                    }
                    catch (Exception exception7)
                    {
                        base.LoginFail();
                        base.passwdErr();
                        base.ShowMessage("登陆失败：" + exception7.Message);
                        return;
                    }
                    if (this.response.StartsWith("+OK"))
                    {
                        base.ShowMessage("登陆成功！");
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
                                base.ShowMessage("取邮件数目失败！");
                                return;
                            }
                        }
                        catch (Exception exception8)
                        {
                            base.ShowMessage("取邮件数目失败！" + exception8.Message);
                            return;
                        }
                        for (int j = 1; j <= this.emailCount; j++)
                        {
                            this.download = false;
                            try
                            {
                                base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                                this.request = "UIDL " + j + "\r\n";
                                this.SendRequest(this.request);
                                this.response = this.streamReader.ReadLine();
                                if (this.response != null)
                                {
                                    this.UUID = this.GetUUID(this.response);
                                }
                                else
                                {
                                    base.ShowMessage("取邮件ID失败！");
                                    continue;
                                }
                                this.SQL = "select count(*) from PmailID where MsgId='" + this.UUID + "'and Name='" + base.m_username + "'";
                                if (Convert.ToInt32(GlobalValue.PopMainForm.ExecuteSQL(this.SQL)) > 0)
                                {
                                    continue;
                                }
                                this.request = "RETR " + j + "\r\n";
                                this.SendRequest(this.request);
                                this.response = this.streamReader.ReadLine();
                                if (this.response.StartsWith("+OK"))
                                {
                                    base.ShowMessage("收件箱第" + j.ToString("00") + "封:" + this.UUID + " 开始下载");
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
                                            base.ShowMessage("收件箱第" + j.ToString("00") + "封:" + this.UUID + "下载失败");
                                            this.download = true;
                                            break;
                                        }
                                    }
                                }
                                if (this.download)
                                {
                                    break;
                                }
                                this.m_emailTime = DateTime.Now.ToString("yyyMMdd-HHmmss") + DateTime.Now.Millisecond.ToString();
                                this.SavePOPEmail(base.MyStringBuilder.ToString());
                                base.ShowMessage("收件箱第" + j.ToString("00") + "封:" + this.UUID + " 下载完毕");
                                try
                                {
                                    string str3 = DateTime.Now.ToString();
                                    this.SQL = "insert into PmailID (Name,MsgId,DownTime,MailType,MailLen) values";
                                    string str4 = "('" + base.m_username + "','" + this.UUID + "','" + str3 + "','" + base.BoxName + "','" + base.m_serv + "')";
                                    this.SQL = this.SQL + str4;
                                    GlobalValue.PopMainForm.ExecuteSQL(this.SQL);
                                }
                                catch (Exception exception9)
                                {
                                    base.ShowMessage("添加失败！" + exception9.Message);
                                    return;
                                }
                            }
                            catch (Exception exception10)
                            {
                                base.ShowMessage(exception10.Message);
                            }
                        }
                        try
                        {
                            if (!this.download)
                            {
                                if (base.m_serv.ToLower().Trim().IndexOf("pop.sohu.com") < 0)
                                {
                                    this.request = "QUIT\r\n";
                                    this.SendRequest(this.request);
                                    this.response = this.streamReader.ReadLine();
                                    if (this.response.StartsWith("+OK"))
                                    {
                                        base.ShowMessage("退出成功！");
                                    }
                                }
                                else
                                {
                                    base.ShowMessage("退出成功！");
                                }
                            }
                            else
                            {
                                base.ShowMessage("下载失败退出！");
                            }
                            return;
                        }
                        catch (Exception exception11)
                        {
                            base.ShowMessage("退出失败！" + exception11.Message);
                            return;
                        }
                    }
                    base.LoginFail();
                    base.passwdErr();
                    base.ShowMessage("登陆失败：" + this.response);
                }
            }

        }
        public void SavePOPEmail(string EmailText)
        {
            try
            {
                string filePath = "";
                if ((GlobalValue.PopMainForm.saveFilePath != "") | (GlobalValue.PopMainForm.saveFilePath != null))
                {
                    filePath = GlobalValue.PopMainForm.saveFilePath;
                }
                this.SavePOPFile(EmailText, filePath);
            }
            catch (Exception exception)
            {
                base.ShowMessage("保存邮件失败：" + exception.Message);
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
        public void SavePOPFile(string EmailText, string filePath)
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
            filePath = filePath + @"\" + base.BoxName;
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
                this.sslStream.Write(this.requestData, 0, this.requestData.Length);
            }
            catch (Exception exception)
            {
                base.ShowMessage("连接失败" + exception.Message);
                this.sslStream.Close();
                this.streamReader.Close();
                this.tcpServer.Close();
            }
        }
    }
}
