using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Web;
using System.IO;
using System.Net.Mail;
using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Text.RegularExpressions;

namespace MailExposure.CookieMailRC
{
    class TransformEml
    {
        // Fields
        private Queue _AnnexQueue;
        private EMLHEAD _EmlHead;
        private string _strEmlPath;
        private string _strHtmlPath;
        private string[][] Tostr;

        // Methods
        public TransformEml()
        {
            this._EmlHead = new EMLHEAD();
            this.Tostr = new string[][] { new string[] { "发件人:", "日期:", "收件人:", "主题:" }, new string[] { "From:<", "Date:<", "To:<", "Subject:<" } };
            this._AnnexQueue = new Queue();
        }
        public TransformEml(MailMessage msg, string emlFileAbsolutePath)
        {
            this._EmlHead = new EMLHEAD();
            this.Tostr = new string[][] { new string[] { "发件人:", "日期:", "收件人:", "主题:" }, new string[] { "From:<", "Date:<", "To:<", "Subject:<" } };
            SaveToEml(msg, emlFileAbsolutePath);
        }
        public TransformEml(string strHtmlPath, string strEmlPath)
        {
            this._EmlHead = new EMLHEAD();
            this.Tostr = new string[][] { new string[] { "发件人:", "日期:", "收件人:", "主题:" }, new string[] { "From:<", "Date:<", "To:<", "Subject:<" } };
            this._strHtmlPath = strHtmlPath;
            this._strEmlPath = strEmlPath;
            this._AnnexQueue = new Queue();
        }
        public void AddAnnexQueue(string strAnnexPath)
        {
            this._AnnexQueue.Enqueue(strAnnexPath);
        }
        public string EmlTheme(string s)
        {
            string str2 = "";
            try
            {
                if (s.Contains("&lt;"))
                {
                    string str = s.Replace("&lt;", "<");
                    if (str.Contains("&gt;"))
                    {
                        return str.Replace("&gt;", ">");
                    }
                    return str;
                }
                if (!s.Contains("&#"))
                {
                    return s;
                }
                int num = this.FondAsc(s, "&#", ";", 2);
                if (0 <= num)
                {
                    if (num > 0x7f)
                    {
                        return s;
                    }
                    return this.ExchangeAsc(s, "&#", ";", 2);
                }
                str2 = s;
            }
            catch
            {
            }
            return str2;
        }
        public string ExchangeAsc(string s, string start, string end, int startint)
        {
            start = "&#";
            end = ";";
            startint = 2;
            string str = "";
            int num = this.FondAsc(s, start, end, startint);
            string newValue = ((char)byte.Parse(Convert.ToString(num), NumberStyles.AllowTrailingWhite)).ToString();
            str = s.Replace(start + Convert.ToString(num) + end, newValue);
            if (str.Contains(start))
            {
                str = this.ExchangeAsc(str, start, end, startint);
            }
            return str;

        }
        public int FondAsc(string s, string start, string end, int startint)
        {
            return int.Parse(this.GetAscString(s, start, end, startint));
        }
        public string GetAscString(string s, string startstr, string endstr, int start)
        {
            int startIndex = s.IndexOf(startstr) + start;
            int index = s.IndexOf(endstr, startIndex);
            return s.Substring(startIndex, index - startIndex);
        }
        public string GetEmail(string s)
        {
            if (!s.Contains("&lt;"))
            {
                return s;
            }
            return this.GetString(s, "&lt;", "&gt;", 4);

        }
        public string GetString(string message, string startStr, string endStr, int startIndex)
        {
            if (startIndex < 0)
            {
                return "";
            }
            if (message.Length < startIndex)
            {
                return "";
            }
            string str = message.Substring(startIndex, message.Length - startIndex);
            int index = str.IndexOf(startStr);
            if (index < 0)
            {
                return "";
            }
            index += startStr.Length;
            str = str.Substring(index, str.Length - index);
            int length = str.IndexOf(endStr);
            if (length < 0)
            {
                return "";
            }
            return str.Substring(0, length);
        }
        public string putstr(string filestr, string Startstr, string Endstr, int Positionstr)
        {
            if (Positionstr < 0)
            {
                return "-1";
            }
            if (filestr.Length < Positionstr)
            {
                return "-1";
            }
            string str = filestr.Substring(Positionstr, filestr.Length - Positionstr);
            int index = str.IndexOf(Startstr);
            if (index < 0)
            {
                return "-1";
            }
            index += Startstr.Length;
            str = str.Substring(index, str.Length - index);
            int length = str.IndexOf(Endstr);
            if (length < 0)
            {
                return "-1";
            }
            return str.Substring(0, length);

        }
        public string ReadFromFile(string filename)
        {
            StreamReader reader = File.OpenText(filename);
            string str = reader.ReadLine();
            while (reader.Peek() != -1)
            {
                str = str + reader.ReadLine();
            }
            reader.Close();
            return str;

        }
        public bool SaveEml()
        {
            string str = this.ReadFromFile(this._strHtmlPath);
            if (str != null)
            {
                MailMessage msg = new MailMessage();
                msg.From = new MailAddress(this._EmlHead.strFrom);
                msg.To.Add(this._EmlHead.strTo);
                msg.Subject = this._EmlHead.strSub;
                msg.IsBodyHtml = true;
                msg.Body = str;
                msg.BodyEncoding = Encoding.UTF8;
                msg.Priority = MailPriority.High;
                while (this._AnnexQueue.Count > 0)
                {
                    string fileName = this._AnnexQueue.Dequeue() as string;
                    if (fileName != "")
                    {
                        msg.Attachments.Add(new Attachment(fileName));
                    }
                }
                SaveToEml(msg, this._strEmlPath);
            }
            return true;
        }
        private static void SaveToEml(MailMessage msg, string emlFileAbsolutePath)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                object obj2 = Activator.CreateInstance(typeof(SmtpClient).Assembly.GetType("System.Net.Mail.MailWriter"), BindingFlags.FlattenHierarchy | BindingFlags.NonPublic | BindingFlags.Instance, null, new object[] { stream }, CultureInfo.InvariantCulture);
                msg.GetType().GetMethod("Send", BindingFlags.FlattenHierarchy | BindingFlags.NonPublic | BindingFlags.Instance).Invoke(msg, new object[] { obj2, true });
                File.WriteAllText(emlFileAbsolutePath, Encoding.Default.GetString(stream.ToArray()), Encoding.UTF8);
            }
        }
        public int ScanStr(string file, string Strfind, int Positionstr)
        {
            if (Positionstr >= 0)
            {
                if (file.Length < Positionstr)
                {
                    return -1;
                }
                int index = file.Substring(Positionstr, file.Length - Positionstr).IndexOf(Strfind);
                if (index > 0)
                {
                    return (index + Positionstr);
                }
                if (index == 0)
                {
                    return this.ScanStr(file, Strfind, Strfind.Length + Positionstr);
                }
            }
            return -1;

        }
        public string StrNameEml(string str, string Name, int sp)
        {
            string str3;
            string str2 = "";
            try
            {
                sp = this.ScanStr(str, Name, sp);
                if (sp == -1)
                {
                    str2 = "-1";
                    goto Label_0051;
                }
            Label_001D:
                str2 = str.Substring(sp - 1, 1);
                if ((str2 != " ") && (str2 != ">"))
                {
                    sp = this.ScanStr(str, Name, sp + 3);
                    goto Label_001D;
                }
            Label_0051:
                do
                {
                    str2 = this.putstr(str, ">", "<", sp);
                    if (str2.Trim() != "")
                    {
                        goto Label_008F;
                    }
                    sp = this.ScanStr(str, ">", sp);
                }
                while (sp != -1);
                str2 = "-1";
            Label_008F:
                str3 = str2;
            }
            catch
            {
                str3 = str2 = "";
            }
            return str3;
        }
        public void Transform()
        {
            string str = this.ReadFromFile(this._strHtmlPath);
            if (str != null)
            {
                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(this._strHtmlPath);
                this.GetString(fileNameWithoutExtension, "", "-", 0);
                string[] strArray = new string[4];
                int index = str.IndexOf(this.Tostr[0][0]);
                int num2 = -1;
                if (index == -1)
                {
                    if (str.IndexOf(this.Tostr[1][0]) == -1)
                    {
                        num2 = -1;
                    }
                    else
                    {
                        num2 = 1;
                    }
                }
                else
                {
                    num2 = 0;
                }
                if (num2 != -1)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        strArray[i] = this.StrNameEml(str, this.Tostr[num2][i], 0);
                    }
                    strArray[0] = this.GetEmail(strArray[0]);
                    strArray[1] = strArray[1];
                    strArray[2] = this.GetEmail(strArray[2]);
                    strArray[3] = this.EmlTheme(strArray[3]);
                    string pattern = @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
                    Regex regex = new Regex(pattern);
                    string str4 = "";
                    if (!regex.IsMatch(strArray[0]))
                    {
                        str4 = str4 + "   发件人异常";
                    }
                    if ((strArray[1] == "") || (strArray[1] == "-1"))
                    {
                        str4 = str4 + "   日期异常  ";
                    }
                    if ((strArray[2] == "") || (strArray[2] == "-1"))
                    {
                        str4 = str4 + "   收件人异常";
                    }
                    if ((strArray[3] == "") || (strArray[3] == "-1"))
                    {
                        str4 = str4 + "    主题异常";
                    }
                    this._EmlHead.strFrom = strArray[0].Trim();
                    this._EmlHead.strTo = strArray[2].Trim();
                    this._EmlHead.strDate = strArray[1].Trim();
                    this._EmlHead.strSub = strArray[3].Trim();
                }
            }
        }

        // Nested Types
        [StructLayout(LayoutKind.Sequential)]
        private struct EMLHEAD
        {
            public string strFrom;
            public string strDate;
            public string strTo;
            public string strSub;
        }

    }
}
