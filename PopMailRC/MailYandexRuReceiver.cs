using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Web;
using System.Threading;
using System.Globalization;
using System.Runtime.InteropServices;

namespace MailExposure.PopMailRC
{
    class MailYandexRuReceiver:MailStr
    {
        // Fields
        private string Annex;
        private BoxNameId[] boxList;
        private CookieContainer cookieContainer;
        public string m_emailTime;
        private static object pob;

        // Methods
        static MailYandexRuReceiver()
        {
            pob = new object();
        }
        public MailYandexRuReceiver()
        {
            this.Annex = "";
            this.boxList = new BoxNameId[200];
        }
        private void getAddressbook()
        {
            string url = "http://mail.yandex.ru/neo/ajax/action_abook_export";
            string indata = "tp=0&rus=1";
            base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
            base.streamControl = true;
            base.MyStringBuilder = this.PostData(url, indata, this.cookieContainer);
            this.SaveAddressbook(base.MyStringBuilder.ToString());
        }
        private void getBoxName(string message)
        {
            this.getAddressbook();
            try
            {
                int index = 0;
                int startIndex = message.IndexOf("Mail.Data.importData({'f");
                string[] strArray = base.putstr(message, "(", ")", startIndex).Split(new char[] { '}' });
                for (int i = 0; i < strArray.Length; i++)
                {
                    string str = base.putstr(strArray[i], "'", "'", 0);
                    string str2 = base.putstr(message, str + "'name':'", "','", 0);
                    if (((str != "-1") && (str2 != "-1")) && ((str != null) && (str2 != null)))
                    {
                        this.boxList[index].boxid = str;
                        this.boxList[index].boxname = str2;
                        index++;
                    }
                }
                for (int j = 0; j < strArray.Length; j++)
                {
                    string str3 = base.putstr(strArray[j], "'fid-", "'", 0);
                    string str4 = base.putstr(strArray[j], "'name':'", "','", 0);
                    if (((str3 != "-1") && (str4 != "-1")) && ((str3 != null) && (str4 != null)))
                    {
                        this.boxList[index].boxid = str3;
                        this.boxList[index].boxname = str4;
                        index++;
                    }
                }
                for (int k = 1; k < index; k++)
                {
                    this.GetMailId(this.boxList[k].boxid, this.boxList[k].boxname);
                }
                this.GetMailId(this.boxList[0].boxid, this.boxList[0].boxname);
            }
            catch (Exception exception)
            {
                base.LoginFail();
                base.passwdErr();
                base.ShowMessage("取箱子失败…………" + exception.Message);
            }
        }
        private void GetMailId(string boxid, string boxname)
        {
            int startIndex = 0;
            string message = "";
            string url = "";
            string str3 = "";
            url = base.Host + "messages?current_folder=" + boxid;
            try
            {
                base.BoxName = boxname;
                base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                base.streamControl = true;
                base.MyStringBuilder = this.Request(url);
                string str5 = base.MyStringBuilder.ToString();
                startIndex = str5.IndexOf("<tbody");
                if (startIndex >= 0)
                {
                    while ((startIndex = str5.IndexOf("<tbody", startIndex)) > 0)
                    {
                        string str4 = base.putstr(str5, "<tbody", "</tbody>", startIndex);
                        if (str4.IndexOf("class=\"table-rows-body\"") < 0)
                        {
                            startIndex++;
                        }
                        else
                        {
                            for (int i = 0; (i = str4.IndexOf("<tr", i)) > 0; i++)
                            {
                                try
                                {
                                    string str6 = base.putstr(str4, "<tr", "</tr>", i);
                                    string str7 = base.putstr(str6, "value=\"", "\"", 0);
                                    if ((str7 != "-1") && (str7 != ""))
                                    {
                                        string strSql = "select count(*) from YandexRumailId where MsgId='" + str7 + "'";
                                        if (Convert.ToInt32(GlobalValue.PopMainForm.ExecuteSQL(strSql)) == 0)
                                        {
                                            string str9 = base.putstr(str6, "class=\"b-grid__from\"", ">", 0);
                                            str9 = base.putstr(str9, "title=\"", "\"", 0);
                                            string str10 = base.putstr(str6, "class=\"b-grid__subject\"", ">", 0);
                                            str10 = base.putstr(str10, "title=\"", "\"", 0);
                                            string str11 = base.putstr(str6, "class=\"b-grid__date\"", ">", 0);
                                            str11 = base.putstr(str11, "title=\"", "\"", 0);
                                            message = base.putstr(str6, "class=\"b-grid__message\"", ">", 0);
                                            message = base.putstr(message, "href=\"/neo/", "\"", 0);
                                            bool flag = false;
                                            try
                                            {
                                                DateTime time = new DateTime();
                                                time = Convert.ToDateTime(str11.ToString(DateTimeFormatInfo.InvariantInfo));
                                                if (DateTime.Compare(time, GlobalValue.PopMainForm.EmailDateTime) >= 0)
                                                {
                                                    flag = true;
                                                }
                                                else
                                                {
                                                    flag = false;
                                                }
                                            }
                                            catch (Exception)
                                            {
                                                flag = true;
                                            }
                                            if (flag)
                                            {
                                                if (message.IndexOf("ids=") > 0)
                                                {
                                                    string str12 = message.Substring(message.IndexOf("ids=") + 4);
                                                    if (str12 != null)
                                                    {
                                                        str3 = base.Host + "source_message_content?ids=" + str12;
                                                    }
                                                }
                                                base.streamControl = true;
                                                base.ShowMessage(base.BoxName + "  " + str10 + "\t 开始下载");
                                                if (this.RequestEmail(str3))
                                                {
                                                    try
                                                    {
                                                        string str13 = DateTime.Now.ToString();
                                                        strSql = "insert into YandexRumailId(Name,MsgId,DownTime,MailType) values('" + base.m_username + "','" + str7 + "','" + str13 + "','" + base.BoxName.Substring(0, base.BoxName.Length % 10) + "')";
                                                        GlobalValue.PopMainForm.ExecuteSQL(strSql);
                                                    }
                                                    catch (Exception exception)
                                                    {
                                                        base.ShowMessage("添加失败：" + exception.Message);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                catch (Exception exception2)
                                {
                                    base.ShowMessage("取邮件失败…………" + exception2.Message);
                                }
                            }
                            startIndex++;
                        }
                    }
                }
                if (str5.IndexOf("<div class=\"b-pager\">") > 0)
                {
                    int num3 = 0;
                    string str14 = base.putstr(str5, "<div class=\"b-pager\">", "</div>", 0);
                    if (str14.IndexOf("id=\"nextPage\"") > 0)
                    {
                        while ((num3 = str14.IndexOf("<a", num3)) > 0)
                        {
                            string str15 = base.putstr(str14, "<a ", "</a>", num3);
                            if (str15.IndexOf("b-pager__next") > 0)
                            {
                                this.GetNextPageMailId(base.Host + base.putstr(str15, "href=\"/neo/", "\"", 0), boxname);
                            }
                            num3++;
                        }
                    }
                }
            }
            catch (Exception exception3)
            {
                base.ShowMessage("取邮件失败…………" + exception3.Message);
            }

        }
        private void GetNextPageMailId(string boxid, string boxname)
        {
            int startIndex = 0;
            string message = "";
            string url = "";
            string str3 = "";
            url = boxid.Replace("amp;", "&");
            try
            {
                base.BoxName = boxname;
                base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                base.streamControl = true;
                base.MyStringBuilder = this.Request(url);
                string str5 = base.MyStringBuilder.ToString();
                startIndex = str5.IndexOf("<tbody");
                if (startIndex >= 0)
                {
                    while ((startIndex = str5.IndexOf("<tbody", startIndex)) > 0)
                    {
                        string str4 = base.putstr(str5, "<tbody", "</tbody>", startIndex);
                        if (str4.IndexOf("class=\"table-rows-body\"") < 0)
                        {
                            startIndex++;
                        }
                        else
                        {
                            for (int i = 0; (i = str4.IndexOf("<tr", i)) > 0; i++)
                            {
                                try
                                {
                                    string str6 = base.putstr(str4, "<tr", "</tr>", i);
                                    string str7 = base.putstr(str6, "value=\"", "\"", 0);
                                    if ((str7 != "-1") && (str7 != ""))
                                    {
                                        string strSql = "select count(*) from YandexRumailId where MsgId='" + str7 + "'";
                                        if (Convert.ToInt32(GlobalValue.PopMainForm.ExecuteSQL(strSql)) == 0)
                                        {
                                            string str9 = base.putstr(str6, "class=\"b-grid__from\"", ">", 0);
                                            str9 = base.putstr(str9, "title=\"", "\"", 0);
                                            string str10 = base.putstr(str6, "class=\"b-grid__subject\"", ">", 0);
                                            str10 = base.putstr(str10, "title=\"", "\"", 0);
                                            string str11 = base.putstr(str6, "class=\"b-grid__date\"", ">", 0);
                                            str11 = base.putstr(str11, "title=\"", "\"", 0);
                                            message = base.putstr(str6, "class=\"b-grid__message\"", ">", 0);
                                            message = base.putstr(message, "href=\"/neo/", "\"", 0);
                                            bool flag = false;
                                            try
                                            {
                                                DateTime time = new DateTime();
                                                time = Convert.ToDateTime(str11.ToString(DateTimeFormatInfo.InvariantInfo));
                                                if (DateTime.Compare(time, GlobalValue.PopMainForm.EmailDateTime) >= 0)
                                                {
                                                    flag = true;
                                                }
                                                else
                                                {
                                                    flag = false;
                                                }
                                            }
                                            catch (Exception)
                                            {
                                                flag = true;
                                            }
                                            if (flag)
                                            {
                                                if (message.IndexOf("ids=") > 0)
                                                {
                                                    string str12 = message.Substring(message.IndexOf("ids=") + 4);
                                                    if (str12 != null)
                                                    {
                                                        str3 = base.Host + "source_message_content?ids=" + str12;
                                                    }
                                                }
                                                base.streamControl = true;
                                                base.ShowMessage(base.BoxName + "  " + str10 + "\t 开始下载");
                                                if (this.RequestEmail(str3))
                                                {
                                                    try
                                                    {
                                                        string str13 = DateTime.Now.ToString();
                                                        strSql = "insert into YandexRumailId(Name,MsgId,DownTime,MailType) values('" + base.m_username + "','" + str7 + "','" + str13 + "','" + base.BoxName.Substring(0, base.BoxName.Length % 10) + "')";
                                                        GlobalValue.PopMainForm.ExecuteSQL(strSql);
                                                    }
                                                    catch (Exception exception)
                                                    {
                                                        base.ShowMessage("添加失败：" + exception.Message);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                catch (Exception exception2)
                                {
                                    base.ShowMessage("取邮件失败…………" + exception2.Message);
                                }
                            }
                            startIndex++;
                        }
                    }
                }
                if (str5.IndexOf("<div class=\"b-pager\">") > 0)
                {
                    int num3 = 0;
                    string str14 = base.putstr(str5, "<div class=\"b-pager\">", "</div>", 0);
                    if (str14.IndexOf("id=\"nextPage\"") > 0)
                    {
                        while ((num3 = str14.IndexOf("<a", num3)) > 0)
                        {
                            string str15 = base.putstr(str14, "<a ", "</a>", num3);
                            if (str15.IndexOf("b-pager__next") > 0)
                            {
                                this.GetNextPageMailId(base.Host + base.putstr(str15, "href=\"/neo/", "\"", 0), boxname);
                            }
                            num3++;
                        }
                    }
                }
            }
            catch (Exception exception3)
            {
                base.ShowMessage("取邮件失败…………" + exception3.Message);
            }

        }
        public override void login()
        {
            base.m_passwd = base.strPassParse(base.m_passwd);
            try
            {
                base.ShowMessage("开始登陆…………");
                if (base.m_serv.IndexOf("yandex.ru") != -1)
                {
                    string url = "https://passport.yandex.ru/passport?mode=auth";
                    string indata = "login=" + base.m_username + "&passwd=" + base.m_passwd + "&retpath=http%253A%252F%252Fmail.yandex.ru%252F%253Ffrom%253Dmail%2526r%253Did1170615853920%2526orig%253Dwmi";
                    this.cookieContainer = new CookieContainer();
                    base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                    base.streamControl = true;
                    base.MyStringBuilder = this.PostData(url, indata, this.cookieContainer);
                    int index = base.MyStringBuilder.ToString().IndexOf("window.location.replace");
                    if (index < 0)
                    {
                        string str3 = "";
                        if (base.MyStringBuilder.ToString().IndexOf("name=\"idkey\"") > 0)
                        {
                            str3 = base.putstr(base.MyStringBuilder.ToString(), "value=\"", "\"", base.MyStringBuilder.ToString().IndexOf("name=\"idkey\""));
                            indata = "filled=yes&timestamp=1283237686645&idkey=" + str3 + "&no=%D0%9D%D0%B5%D1%82";
                            base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                            base.streamControl = true;
                            base.MyStringBuilder = this.PostData(url, indata, this.cookieContainer);
                        }
                    }
                    index = base.MyStringBuilder.ToString().IndexOf("window.location.replace");
                    if (index > 0)
                    {
                        url = base.putstr(base.MyStringBuilder.ToString(), "\"", "\"", index);
                        if (url.IndexOf("http") != -1)
                        {
                            base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                            base.streamControl = true;
                            base.MyStringBuilder = this.Request(url);
                            if (base.MyStringBuilder.ToString().IndexOf("folders") > 0)
                            {
                                base.Host = "http://mail.yandex.ru/neo/";
                                if (base.MyStringBuilder.ToString().IndexOf("Написать") < 0)
                                {
                                    base.LoginFail();
                                    base.passwdErr();
                                    base.ShowMessage("登陆失败！");
                                }
                                else
                                {
                                    base.ShowMessage("登陆成功！");
                                    this.getBoxName(base.MyStringBuilder.ToString());
                                }
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
                            base.ShowMessage("登陆失败…………");
                        }
                    }
                    else
                    {
                        base.LoginFail();
                        base.ShowMessage("登陆失败…………");
                    }
                }
                else
                {
                    base.LoginFail();
                    base.passwdErr();
                    base.ShowMessage("登陆失败…………");
                }
            }
            catch (Exception)
            {
                base.LoginFail();
                base.passwdErr();
                base.ShowMessage("登陆失败…………");
            }

        }
        public StringBuilder PostData(string url, string indata, CookieContainer cookieContainer)
        {
            try
            {
                StreamReader reader;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = indata.Length;
                request.Method = "POST";
                request.AllowAutoRedirect = false;
                request.ServicePoint.Expect100Continue = false;
                request.CookieContainer = cookieContainer;
                Stream requestStream = request.GetRequestStream();
                char[] chars = Encoding.ASCII.GetChars(Encoding.ASCII.GetBytes(indata));
                StreamWriter writer = new StreamWriter(requestStream);
                writer.Write(chars, 0, chars.Length);
                writer.Close();
                requestStream.Close();
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                response.Cookies = cookieContainer.GetCookies(request.RequestUri);
                response.GetResponseStream();
                WebHeaderCollection headers = response.Headers;
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
                    string str2 = "";
                    str2 = headers["location"];
                    if (str2.IndexOf("http") != -1)
                    {
                        base.MyStringBuilder = this.Request(str2);
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
                }
            }
            catch (Exception)
            {
            }
            return base.MyStringBuilder;

        }
        private StringBuilder Request(string url)
        {
            try
            {
                StreamReader reader;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.CookieContainer = this.cookieContainer;
                request.Method = "GET";
                request.Accept = "*/*";
                request.AllowAutoRedirect = false;
                request.ServicePoint.Expect100Continue = false;
                request.KeepAlive = true;
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1)";
                request.ContentType = "application/x-www-form-urlencoded";
                request.Headers.Add("Accept-Language: zh-cn");
                request.Referer = "http://www.mail.ru";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                try
                {
                    response.Cookies = this.cookieContainer.GetCookies(request.RequestUri);
                }
                catch (Exception)
                {
                }
                WebHeaderCollection headers = response.Headers;
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
                    string str2 = "";
                    str2 = headers["location"];
                    if (str2.IndexOf("http") != -1)
                    {
                        this.Request(str2);
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
                }
            }
            catch (Exception)
            {
            }
            return base.MyStringBuilder;

        }
        private bool RequestEmail(string url)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.CookieContainer = this.cookieContainer;
                request.Method = "GET";
                request.Accept = "*/*";
                request.AllowAutoRedirect = false;
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1)";
                request.ContentType = "application/x-www-form-urlencoded";
                request.Headers.Add("Accept-Language: zh-cn");
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                response.Cookies = this.cookieContainer.GetCookies(request.RequestUri);
                Thread.Sleep(10);
                Stream responseStream = response.GetResponseStream();
                if (base.streamControl)
                {
                    int num;
                    byte[] buffer = new byte[0x400];
                    base.m_emailno++;
                    this.m_emailTime = DateTime.Now.ToString("yyyMMdd-HHmmss") + DateTime.Now.Millisecond.ToString();
                    while ((num = responseStream.Read(buffer, 0, 0x400)) > 0)
                    {
                        if (!this.SaveRu(buffer, num))
                        {
                            return false;
                        }
                    }
                    this.saveRuEmailCount();
                    GlobalValue.PopMainForm.statusBarPanelEmailNumbersText.Text = (Convert.ToInt32(GlobalValue.PopMainForm.statusBarPanelEmailNumbersText.Text) + 1).ToString();
                    base.streamControl = false;
                    return true;
                }
                return false;
            }
            catch (Exception exception)
            {
                base.ShowMessage("邮件下载失败" + exception.StackTrace);
                return false;
            }

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
            string str3 = path;
            path = str3 + @"\" + base.m_username + "@" + base.m_serv;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            path = path + @"\地址薄";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            Stream stream = File.Create(path + @"\" + base.m_username + "-addressbook.csv");
            using (StreamWriter writer = new StreamWriter(stream))
            {
                writer.Write(EmailText);
            }
            stream.Close();
            stream = null;
        }
        private bool SaveRu(byte[] buffer, int nbytes)
        {
            try
            {
                string filePath = "";
                if ((GlobalValue.PopMainForm.saveFilePath != "") | (GlobalValue.PopMainForm.saveFilePath != null))
                {
                    filePath = GlobalValue.PopMainForm.saveFilePath;
                }
                this.SaveRuText(buffer, nbytes, filePath);
            }
            catch (Exception exception)
            {
                base.ShowMessage("保存邮件失败：" + exception.Message);
                return false;
            }
            return true;
        }
        public void saveRuEmailCount()
        {
            try
            {
                string str = "'" + DateTime.Today.ToString() + "'";
                string strSql = string.Concat(new object[] { "update users set 完成时间=", str, ",邮件数量='", base.m_emailno, "' where 服务器地址='", base.m_serv, "' and 用户名='", base.m_username, "'" });
                GlobalValue.PopMainForm.ExecuteSQL(strSql);
            }
            catch (Exception exception)
            {
                base.ShowMessage(exception.Message);
            }

        }
        public void SaveRuText(byte[] buffer, int nbytes, string filePath)
        {
            lock (pob)
            {
                FileStream stream = null;
                try
                {
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
                    filePath = filePath + @"\" + this.m_emailTime + str;
                    stream = File.Open(filePath, FileMode.Append, FileAccess.Write);
                    stream.Write(buffer, 0, nbytes);
                    stream.Close();
                }
                catch (Exception exception)
                {
                    base.ShowMessage("保存文件失败：" + exception.Message);
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
        // Nested Types
        [StructLayout(LayoutKind.Sequential)]
        private struct BoxNameId
        {
            public string boxname;
            public string boxid;
        }
    }
}
