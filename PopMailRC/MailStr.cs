using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Runtime.InteropServices;

namespace MailExposure.PopMailRC
{
    class MailStr
    {
        // Fields
        public string AttId;
        public string AttName;
        public string BoxName;
        public string charSet;
        public string cookie;
        public const string CRLF = "\r\n";
        public EmailInfo_Gmail[] emailInfo;
        public string emailuri;
        public string Host;
        public string listid;
        public int m_emailno;
        public int m_NO;
        public string m_passwd;
        public string m_serv;
        public string m_snote;
        public string m_stype;
        public string m_username;
        public string m_UserType;
        public string mailType;
        public StringBuilder MyStringBuilder;
        public string server;
        public bool streamControl;
        public string validationLogin;
        public static object write;

        // Methods
        static MailStr()
        {
            write = new object();
        }
        public MailStr()
        {
            this.MyStringBuilder = new StringBuilder(0xf4240);
            this.AttId = "";
            this.AttName = "";
        }
        public void completeThreads()
        {
            int num = Convert.ToInt32(GlobalValue.PopMainForm.statusBarPanelThreadsText.Text) + 1;
            GlobalValue.PopMainForm.statusBarPanelThreadsText.Text = Convert.ToString(num);

        }
        public string cook(string setcook)
        {
            int startIndex = 0;
            int length = setcook.IndexOf("=") + 1;
            string startStr = setcook.Substring(0, length);
            string newstring = this.putstr(setcook, startStr, ";", 0);
            if (newstring.IndexOf(",") != -1)
            {
                newstring = newstring.Substring(0, newstring.IndexOf(","));
            }
            if (this.upstr(startStr) && (newstring.Trim() != ""))
            {
                if ((this.cookie != null) && (this.cookie.IndexOf(" " + startStr) != -1))
                {
                    string oldstring = this.putstr(this.cookie + ";", startStr, ";", 0);
                    if (oldstring.Length != 0)
                    {
                        this.cookie = this.StringReplace(this.cookie, oldstring, newstring, 0);
                    }
                    else
                    {
                        this.cookie = this.StringReplace(this.cookie, startStr + oldstring + ";", startStr + newstring + ";", 0);
                    }
                }
                else
                {
                    if ((this.cookie != null) && (this.cookie.Trim() != ""))
                    {
                        this.cookie = this.cookie + "; ";
                    }
                    this.cookie = this.cookie + startStr;
                    this.cookie = this.cookie + newstring;
                }
            }
            while (true)
            {
                startIndex = setcook.IndexOf(",") + 1;
                if (startIndex == 0)
                {
                    if ((this.cookie != null) && (this.cookie.Trim() != ""))
                    {
                        this.cookie = this.cookie.Trim();
                    }
                    return this.cookie;
                }
                setcook = setcook.Substring(startIndex, setcook.Length - startIndex);
                length = setcook.IndexOf("=") + 1;
                startStr = setcook.Substring(0, length);
                while (true)
                {
                    if (startStr.IndexOf(",") == -1)
                    {
                        break;
                    }
                    startStr = startStr.Substring(startStr.IndexOf(",") + 1);
                }
                newstring = this.putstr(setcook, startStr, ";", 0);
                if (this.upstr(startStr) && (newstring.Trim() != ""))
                {
                    if (this.cookie.IndexOf(" " + startStr) != -1)
                    {
                        string str4 = this.putstr(this.cookie + ";", startStr, ";", 0);
                        if (str4.Length != 0)
                        {
                            this.cookie = this.StringReplace(this.cookie, str4, newstring, 0);
                        }
                        else
                        {
                            this.cookie = this.StringReplace(this.cookie, startStr + str4 + ";", startStr + newstring + ";", 0);
                        }
                    }
                    else
                    {
                        if ((this.cookie != null) && (this.cookie.Trim() != ""))
                        {
                            this.cookie = this.cookie + "; ";
                        }
                        this.cookie = this.cookie + startStr;
                        this.cookie = this.cookie + newstring;
                    }
                }
            }
        }
        public string DecodeBase64(string code_type, string code)
        {
            byte[] bytes = null;
            Convert.FromBase64String(code);
            try
            {
                return Encoding.GetEncoding(code_type).GetString(bytes);
            }
            catch
            {
                return code;
            }
        }
        public string EncodeBase64(string code_type, string code)
        {
            byte[] bytes = Encoding.GetEncoding(code_type).GetBytes(code);
            try
            {
                return Convert.ToBase64String(bytes);
            }
            catch
            {
                return code;
            }
        }
        public virtual void login()
        {
        }
        public void LoginFail()
        {
            try
            {
                string str;
                GlobalValue.PopMainForm.statusBarPanelLoginFailText.Text = (Convert.ToInt32(GlobalValue.PopMainForm.statusBarPanelLoginFailText.Text) + 1).ToString();
                string str2 = DateTime.Now.Date.ToString("yyy-MM-dd");
                if (GlobalValue.PopMainForm.saveFilePath != "")
                {
                    str = GlobalValue.PopMainForm.saveFilePath + "日志";
                }
                else
                {
                    str = AppDomain.CurrentDomain.BaseDirectory + "日志";
                }
                if (!Directory.Exists(str))
                {
                    Directory.CreateDirectory(str);
                }
                string filepath = str + @"\" + str2 + "登陆失败信息.txt";
                string filetext = "用户名：" + this.m_username + "\r\n";
                if (this.m_serv != null)
                {
                    filetext = filetext + "邮箱类型：" + this.m_serv + "\r\n";
                }
                else
                {
                    filetext = filetext + "邮箱类型：" + this.server + "\r\n";
                }
                filetext = filetext + "登陆失败时间：" + str2 + "\r\n";
                this.WriteLogFile(filepath, filetext);
            }
            catch (Exception exception)
            {
                this.ShowMessage(exception.Message);
            }

        }
        public void passwdErr()
        {
            try
            {
                string strSql = "select 邮箱状态 from users where 服务器='" + this.server + "' and 用户名='" + this.m_username + "'";
                string str2 = GlobalValue.PopMainForm.ExecuteSQL(strSql);
                bool flag = true;
                str2 = str2 + "?";
                strSql = string.Concat(new object[] { "update users set 邮箱状态='", str2, "',接收时间=", flag, " where 服务器='", this.server, "' and 用户名='", this.m_username, "'" });
                GlobalValue.PopMainForm.ExecuteSQL(strSql);
            }
            catch (Exception)
            {
            }

        }
        public void passwdOK()
        {
            try
            {
                string strSql = "";
                string str2 = "?";
                bool flag = true;
                strSql = string.Concat(new object[] { "update users set 邮箱状态='", str2, "',控守=", flag, ",接收时间=", flag, " where 服务器='", this.server, "' and 用户名='", this.m_username, "'" });
                GlobalValue.PopMainForm.ExecuteSQL(strSql);
            }
            catch (Exception)
            {
            }

        }
        public string putstr(string message, string startStr, string endStr, int startIndex)
        {
            if (startIndex < 0)
            {
                return "-1";
            }
            if (message.Length < startIndex)
            {
                return "-1";
            }
            string str = "";
            try
            {
                str = message.Substring(startIndex, message.Length - startIndex);
            }
            catch (Exception)
            {
                return "-1";
            }
            int index = str.IndexOf(startStr);
            if (index < 0)
            {
                return "-1";
            }
            index += startStr.Length;
            str = str.Substring(index, str.Length - index);
            int length = str.IndexOf(endStr);
            if (length < 0)
            {
                return "-1";
            }
            return str.Substring(0, length);

        }
        public void SaveEmail(string EmailText)
        {
            try
            {
                string filePath = "";
                if ((GlobalValue.PopMainForm.saveFilePath != "") | (GlobalValue.PopMainForm.saveFilePath != null))
                {
                    filePath = GlobalValue.PopMainForm.saveFilePath;
                }
                this.SaveFile(EmailText, filePath);
            }
            catch (Exception exception)
            {
                this.ShowMessage("保存邮件失败：" + exception.Message);
                return;
            }
            try
            {
                this.saveEmailCount();
                GlobalValue.PopMainForm.statusBarPanelEmailNumbersText.Text = (Convert.ToInt32(GlobalValue.PopMainForm.statusBarPanelEmailNumbersText.Text) + 1).ToString();
            }
            catch (Exception exception2)
            {
                this.ShowMessage(exception2.Message);
            }

        }
        public void saveEmailCount()
        {
            try
            {
                string str = "'" + DateTime.Today.ToString() + "'";
                string strSql = string.Concat(new object[] { "update users set 完成时间=", str, ",邮件数量='", this.m_emailno, "' where 服务器='", this.server, "' and 用户名='", this.m_username, "'" });
                GlobalValue.PopMainForm.ExecuteSQL(strSql);
            }
            catch (Exception exception)
            {
                this.ShowMessage(exception.Message);
            }
        }
        public void SaveFile(string EmailText, string filePath)
        {
            this.m_emailno++;
            string str = ".eml";
            if (this.server.Trim().ToUpper() == "YAHOO")
            {
                str = ".html";
            }
            string str2 = DateTime.Now.Date.ToString("yyy-MM-dd");
            filePath = filePath + str2 + "邮件";
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            filePath = filePath + @"\" + this.m_snote;
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            filePath = filePath + @"\" + this.m_stype;
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            filePath = filePath + @"\" + this.m_username;
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            filePath = filePath + @"\" + this.BoxName;
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            Stream stream = File.Create(string.Concat(new object[] { filePath, @"\", this.BoxName, '-', Convert.ToInt32(this.m_emailno), str }));
            using (StreamWriter writer = new StreamWriter(stream))
            {
                writer.Write(EmailText);
            }
            stream.Close();
            stream = null;

        }
        public void ShowMessage(string message)
        {
            try
            {
                string str3;
                string str = DateTime.Now.ToString();
                if (message.IndexOf(" Unable to process request at this time -- error 999") != -1)
                {
                    message = "服务器拒绝服务(999)!";
                }
                GlobalValue.PopMainForm.listBoxView.Items.Add("[" + this.listid + "：" + this.m_username + "]" + message + "  " + str);
                GlobalValue.PopMainForm.listBoxView.SelectedIndex = GlobalValue.PopMainForm.listBoxView.Items.Count - 1;
                string str2 = DateTime.Now.Date.ToString("yyy-MM-dd");
                if (GlobalValue.PopMainForm.saveFilePath != "")
                {
                    str3 = GlobalValue.PopMainForm.saveFilePath + "日志";
                }
                else
                {
                    str3 = AppDomain.CurrentDomain.BaseDirectory + "日志";
                }
                if (!Directory.Exists(str3))
                {
                    Directory.CreateDirectory(str3);
                }
                string filepath = str3 + @"\" + str2 + "日志文件.txt";
                string filetext = "[" + this.listid + "：" + this.m_username + "]" + message + "  " + str + "\r\n";
                this.WriteLogFile(filepath, filetext);
                if (message == "服务器拒绝服务!")
                {
                    Thread.CurrentThread.Abort();
                }
            }
            catch (Exception exception)
            {
                GlobalValue.PopMainForm.listBoxView.Items.Add(exception.Message + exception.StackTrace);
                GlobalValue.PopMainForm.listBoxView.SelectedIndex = GlobalValue.PopMainForm.listBoxView.Items.Count - 1;
            }

        }
        public void START()
        {
            string str = "1";
            string str2 = DateTime.Now.ToShortTimeString().Substring(0, 2);
            string str3 = "'" + str2 + "'";
            string strSql = "";
            try
            {
                strSql = "update users set 接收状态=" + str + ",接收=" + str3 + " where 服务器地址='" + this.m_serv + "'and 用户名='" + this.m_username + "'";
                GlobalValue.PopMainForm.ExecuteSQL(strSql);
                this.login();
                this.ShowMessage("完成!");
                str = "0";
                strSql = "update users set 接收状态=" + str + ",接收=" + str3 + " where 服务器地址='" + this.m_serv + "'and 用户名='" + this.m_username + "'";
                GlobalValue.PopMainForm.ExecuteSQL(strSql);
                this.completeThreads();
            }
            catch (Exception exception)
            {
                this.ShowMessage(exception.Message);
            }
            finally
            {
                str = "0";
                strSql = "update users set 接收状态=" + str + ",接收=" + str3 + " where 服务器地址='" + this.m_serv + "'and 用户名='" + this.m_username + "'";
                GlobalValue.PopMainForm.ExecuteSQL(strSql);
            }
        }
        public string StringReplace(string file, string oldstring, string newstring, int position)
        {
            if (file.Length < position)
            {
                return "-1";
            }
            string str = file.Substring(0, position);
            string str2 = file.Substring(position, file.Length - position);
            int index = str2.IndexOf(oldstring);
            if (index >= 0)
            {
                int length = oldstring.Length;
                int num3 = newstring.Length;
                str = str + str2.Substring(0, index);
                index += length;
                file = str + newstring + str2.Substring(index, str2.Length - index);
                position = ((index - length) + num3) + position;
                while (file.Length >= position)
                {
                    str = file.Substring(0, position);
                    str2 = file.Substring(position, file.Length - position);
                    index = str2.IndexOf(oldstring);
                    if (index < 0)
                    {
                        return file;
                    }
                    str = str + str2.Substring(0, index);
                    index += length;
                    file = str + newstring + str2.Substring(index, str2.Length - index);
                    position = ((index - length) + num3) + position;
                }
            }
            return file;

        }
        public string strPassParse(string passWdtext)
        {
            string str = null;
            string str2 = null;
            try
            {
                foreach (char ch in passWdtext.ToCharArray())
                {
                    if ((((ch < 'a') || (ch > 'z')) && ((ch < 'A') || (ch > 'Z'))) && (((ch < '0') || (ch > '9')) && (ch != '-')))
                    {
                        str2 = "%";
                        str2 = str2 + Convert.ToString((int)ch, 0x10);
                    }
                    else
                    {
                        str2 = ch.ToString();
                    }
                    str = str + str2;
                }
            }
            catch (Exception)
            {
                this.ShowMessage("特殊字符处理失败！");
            }
            return str;

        }
        public bool upstr(string str)
        {
            str = str.Trim();
            if ((str == null) || (str.Length == 0))
            {
                return false;
            }
            byte[] bytes = new ASCIIEncoding().GetBytes(str);
            if ((bytes[0] < 0x3a) && (bytes[0] > 0x2f))
            {
                return false;
            }
            return true;

        }
        public void WriteLogFile(string filepath, string filetext)
        {
            object obj2;
            Monitor.Enter(obj2 = write);
            try
            {
                byte[] bytes = Encoding.UTF8.GetBytes(filetext);
                FileStream stream = null;
                stream = File.Open(filepath, FileMode.Append, FileAccess.Write);
                stream.Write(bytes, 0, bytes.Length);
                stream.Close();
                stream.Dispose();
            }
            catch (Exception exception)
            {
                this.ShowMessage(exception.Message);
            }
            finally
            {
                Monitor.Exit(obj2);
            }

        }

        // Nested Types
        [StructLayout(LayoutKind.Sequential)]
        public struct EmailInfo_Gmail
        {
            public string EmailUrl;
            public string EmailID;
        }

    }
}
