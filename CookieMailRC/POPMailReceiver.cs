using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net.Sockets;

namespace MailExposure.CookieMailRC
{
    class POPMailReceiver:MailStr
    {
        // Fields
        private bool download;
        private int emailCount;
        private NetworkStream netWorkStream;
        private string request;
        private byte[] requestData;
        private string response;
        private string SQL;
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
            return mess.Split(new char[] { ' ' })[2];
        }
        public override void login()
        {
            try
            {
                this.tcpServer = new TcpClient(base.m_serv, 110);
            }
            catch (Exception exception)
            {
                base.ShowMessage("与服务器连接失败：" + exception.Message);
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
                    catch (Exception exception3)
                    {
                        base.ShowMessage("取邮件数目失败！" + exception3.Message);
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
                                base.ShowMessage("取邮件ID失败！");
                                continue;
                            }
                            this.SQL = "select count(*) from PmailID where MsgId='" + this.UUID + "'and Name='" + base.m_username + "'";
                            if (Convert.ToInt32(GlobalValue.mainForm.ExecuteSQL(this.SQL)) > 0)
                            {
                                continue;
                            }
                            this.request = "RETR " + i + "\r\n";
                            this.SendRequest(this.request);
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
                                break;
                            }
                            base.SaveEmail(base.MyStringBuilder.ToString());
                            base.ShowMessage("收件箱第" + i.ToString("00") + "封:" + this.UUID + " 下载完毕");
                            try
                            {
                                string str = DateTime.Now.ToString();
                                this.SQL = "insert into PmailID (Name,MsgId,DownTime,MailType,MailLen) values";
                                string str2 = "('" + base.m_username + "','" + this.UUID + "','" + str + "','" + base.BoxName + "','" + base.m_serv + "')";
                                this.SQL = this.SQL + str2;
                                GlobalValue.mainForm.ExecuteSQL(this.SQL);
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
                    catch (Exception exception6)
                    {
                        base.ShowMessage("退出失败！" + exception6.Message);
                        return;
                    }
                }
                base.LoginFail();
                base.passwdErr();
                base.ShowMessage("登陆失败：" + this.response);
            }

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

    }
}
