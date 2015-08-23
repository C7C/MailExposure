using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Web;
using System.Threading;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;

namespace MailExposure.PopMailRC
{
    class Mail163Receiver:MailStr
    {
        // Fields
        public bool booPost;
        private BoxNameId[] boxList;
        public string m_emailTime;
        private string sid;
        private string url;
        private string urls;

        // Methods
        public Mail163Receiver()
        {
            this.boxList = new BoxNameId[200];
            this.urls = "";
            this.url = "";
            this.sid = "";
        }
        private void getBoxName()
        {
            int startIndex = 0;
            int index = 0;
            try
            {
                string message = "";
                if (this.urls.IndexOf("mail.163.com") != -1)
                {
                    message = base.MyStringBuilder.ToString();
                    this.urls = this.urls.Replace("main.jsp?", "mbox/foldmain.jsp?");
                    this.url = this.urls + "&fr=welcome";
                    base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                    base.streamControl = true;
                    base.MyStringBuilder = this.Request(this.url);
                    message = base.MyStringBuilder.ToString();
                    int num3 = 0;
                    num3 = message.IndexOf("Ibx_gTable Ibx_gTable_Con");
                    string str2 = base.putstr(message, "<tbody>", "</tbody>", num3);
                    while ((startIndex = str2.IndexOf("<tr", startIndex)) != -1)
                    {
                        string str3 = base.putstr(str2, "fid=", "\"", startIndex).Trim();
                        string str4 = base.putstr(str2, "Ibx_Td_fdFile\" title=\"", "\"", startIndex).Trim();
                        string str5 = base.putstr(str2, "Ibx_Td_fdAllMail\">", "</td>", startIndex).Trim();
                        string str6 = "";
                        if (str5.IndexOf(",") != -1)
                        {
                            string[] strArray = str5.Split(new char[] { ',' });
                            for (int j = 0; j < strArray.Length; j++)
                            {
                                str6 = str6 + strArray[j];
                            }
                        }
                        else
                        {
                            str6 = str5;
                        }
                        if (((str3 != "-1") && (str4 != "-1")) && ((str3 != "") && (str4 != "")))
                        {
                            this.boxList[index].boxid = str3;
                            this.boxList[index].boxname = str4;
                            this.boxList[index].mailCount = Convert.ToInt32(str6);
                            index++;
                        }
                        startIndex++;
                    }
                    num3 += 20;
                    startIndex = message.IndexOf("Ibx_gTable Ibx_gTable_Con", num3);
                    if (startIndex != -1)
                    {
                        string str7 = base.putstr(message, "<tbody>", "</table>", startIndex);
                        for (startIndex = 0; (startIndex = str7.IndexOf("<tr", startIndex)) != -1; startIndex++)
                        {
                            string str8 = base.putstr(str7, "fid=", "\"", startIndex).Trim();
                            string str9 = base.putstr(str7, "Ibx_Td_fdFile\" title=\"", "\"", startIndex).Trim();
                            string str10 = base.putstr(str7, "Ibx_Td_fdAllMail\">", "</td>", startIndex).Trim();
                            string str11 = "";
                            if (str10.IndexOf(",") != -1)
                            {
                                string[] strArray2 = str10.Split(new char[] { ',' });
                                for (int k = 0; k < strArray2.Length; k++)
                                {
                                    str11 = str11 + strArray2[k];
                                }
                            }
                            else
                            {
                                str11 = str10;
                            }
                            if (((str8 != "-1") && (str9 != "-1")) && ((str8 != "") && (str9 != "")))
                            {
                                this.boxList[index].boxid = str8;
                                this.boxList[index].boxname = str9;
                                this.boxList[index].mailCount = Convert.ToInt32(str11);
                                index++;
                            }
                        }
                    }
                    for (int i = 0; i < index; i++)
                    {
                        this.getEmailId(this.urls, this.boxList[i].boxname, this.boxList[i].boxid, this.boxList[i].mailCount);
                    }
                }
                else
                {
                    base.ShowMessage("取箱子失败！");
                }
            }
            catch (Exception exception)
            {
                base.ShowMessage("取箱子失败！" + exception.Message);
            }

        }
        private void getEmailId(string urlbox, string boxName, string boxId, int mailCount)
        {
            int startIndex = 0;
            string url = "";
            string str2 = "";
            string str3 = "";
            int num2 = 0;
            try
            {
                str3 = urlbox;
                base.BoxName = boxName;
                urlbox = urlbox.Replace("mbox/foldmain.jsp?", "mbox/list.jsp?");
                url = urlbox + "&fid=" + boxId;
                base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                base.streamControl = true;
                base.MyStringBuilder = this.Request(url);
                string message = base.MyStringBuilder.ToString();
                string str8 = base.putstr(message, "selected>", "option>", 0);
                num2 = Convert.ToInt32(base.putstr(str8, "/", "<", 0));
                int index = 0;
                index = message.IndexOf("Ibx_gTable Ibx_gTable_Con");
                message = base.putstr(message, "<tbody>", "Ibx_Lst_bExtra", index);
                while ((startIndex = message.IndexOf("<tr", startIndex)) != -1)
                {
                    string str9 = base.putstr(message, "value=\"", "\"", startIndex).Trim();
                    if ((str9 != "-1") && (str9 != ""))
                    {
                        string strSql = "select count(*) from 163mailId where MsgId='" + str9 + "'";
                        if (Convert.ToInt32(GlobalValue.PopMainForm.ExecuteSQL(strSql)) == 0)
                        {
                            string str4 = HttpUtility.HtmlDecode(base.putstr(message, "Ibx_Td_From\" title=\"", "\">", startIndex));
                            string str5 = base.putstr(message, "Ibx_Td_Subject\" title=\"", "\">", startIndex);
                            string str6 = base.putstr(message, "Ibx_Td_Date\" title=\"", "\">", startIndex);
                            bool flag = false;
                            try
                            {
                                DateTime time = new DateTime();
                                time = Convert.ToDateTime(str6);
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
                                urlbox = urlbox.Replace("mbox/list.jsp?", "read/readdata.jsp?");
                                str2 = urlbox + "&mid=" + str9 + "&mode=download&part=0";
                                base.streamControl = true;
                                try
                                {
                                    this.RequestEmail(str2);
                                    try
                                    {
                                        string str11 = DateTime.Now.ToString();
                                        strSql = "insert into 163mailId(Name,MsgId,DownTime,MailType) values('" + base.m_username + "','" + str9 + "','" + str11 + "','" + base.BoxName.Substring(0, base.BoxName.Length % 10) + "')";
                                        GlobalValue.PopMainForm.ExecuteSQL(strSql);
                                    }
                                    catch (Exception exception)
                                    {
                                        base.ShowMessage("添加失败：" + exception.Message);
                                    }
                                    base.ShowMessage(base.BoxName + ":" + str5 + "\t下载");
                                }
                                catch (Exception exception2)
                                {
                                    base.ShowMessage("邮件下载失败:" + str5 + ":\t" + exception2.Message);
                                }
                            }
                        }
                    }
                    startIndex++;
                }
                if (num2 != -1)
                {
                    int pageno = 2;
                    for (int i = 1; i < num2; i++)
                    {
                        this.GetNextPage(str3, boxName, boxId, pageno);
                        pageno++;
                    }
                }
            }
            catch (Exception exception3)
            {
                base.ShowMessage("取邮件失败!" + exception3.Message);
            }

        }
        private void getNewBoxName(string strfolder)
        {
            int index = 0;
            try
            {
                string input = "";
                if (this.urls.IndexOf("mail.163.com") != -1)
                {
                    input = strfolder;
                    string pattern = "<object>[\\s\\S]+?name=\"id\">(?<BoxID>[\\d]+)?<\\/int>[\\s\\S]+?name=\"name\">(?<BoxName>[^<]+)?<\\/str[\\s\\S]+?<\\/object>";
                    Match match = new Regex(pattern).Match(input);
                    if (match.Length < 1)
                    {
                        base.ShowMessage("取箱子失败!");
                    }
                    else
                    {
                        index = 0;
                        while (match.Success)
                        {
                            this.boxList[index].boxname = match.Groups["BoxName"].Value;
                            this.boxList[index].boxid = match.Groups["BoxID"].Value;
                            match = match.NextMatch();
                            index++;
                        }
                        for (int i = 0; i < index; i++)
                        {
                            this.getNewMailId(this.urls, this.boxList[i].boxname, this.boxList[i].boxid);
                        }
                    }
                }
                else
                {
                    base.ShowMessage("取箱子失败！");
                }
            }
            catch (Exception exception)
            {
                base.ShowMessage("取箱子失败！" + exception.Message);
            }

        }
        private void getNewMailId(string urlbox, string boxName, string boxId)
        {
            int startIndex = 0;
            string url = "";
            string str2 = "";
            try
            {
                base.BoxName = boxName;
                string indata = "<?xml version=\"1.0\"?><object><int name=\"fid\">" + boxId + "</int><string name=\"order\">date</string><boolean name=\"desc\">true</boolean><int name=\"start\">0</int><int name=\"limit\">9999</int></object>";
                urlbox = urlbox.Replace("js3/main.jsp?", "js3/s?");
                url = urlbox + "&func=mbox:listMessages";
                base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                base.streamControl = true;
                base.MyStringBuilder = this.PostData(url, indata);
                indata = base.MyStringBuilder.ToString();
                while ((startIndex = indata.IndexOf("<object>", startIndex)) != -1)
                {
                    string str7 = base.putstr(indata, "<string name=\"id\">", "</string>", startIndex).Trim();
                    if ((str7 != "-1") && (str7 != ""))
                    {
                        string strSql = "select count(*) from 163mailId where MsgId='" + str7 + "'";
                        if (Convert.ToInt32(GlobalValue.PopMainForm.ExecuteSQL(strSql)) == 0)
                        {
                            string str3 = HttpUtility.HtmlDecode(base.putstr(indata, "<string name=\"from\">", "</string>", startIndex));
                            string str4 = base.putstr(indata, "<string name=\"subject\">", "</string>", startIndex);
                            string str5 = base.putstr(indata, "<date name=\"receivedDate\">", "</date>", startIndex);
                            bool flag = false;
                            try
                            {
                                DateTime time = new DateTime();
                                time = Convert.ToDateTime(str5);
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
                                str2 = urlbox + "&func=mbox:getMessageData&mid=" + str7 + "&mode=download";
                                base.streamControl = true;
                                try
                                {
                                    this.RequestEmail(str2);
                                    try
                                    {
                                        string str9 = DateTime.Now.ToString();
                                        strSql = "insert into 163mailId(Name,MsgId,DownTime,MailType) values('" + base.m_username + "','" + str7 + "','" + str9 + "','" + base.BoxName.Substring(0, base.BoxName.Length % 10) + "')";
                                        GlobalValue.PopMainForm.ExecuteSQL(strSql);
                                    }
                                    catch (Exception exception)
                                    {
                                        base.ShowMessage("添加失败：" + exception.Message);
                                    }
                                    base.ShowMessage(base.BoxName + ":" + str4 + "\t下载");
                                }
                                catch (Exception exception2)
                                {
                                    base.ShowMessage("邮件下载失败:" + str4 + ":\t" + exception2.Message);
                                }
                            }
                        }
                    }
                    startIndex++;
                }
            }
            catch (Exception exception3)
            {
                base.ShowMessage("取邮件失败!" + exception3.Message);
            }

        }
        private void GetNextPage(string urlbox, string boxName, string boxId, int pageno)
        {
            int startIndex = 0;
            string url = "";
            string str2 = "";
            try
            {
                base.BoxName = boxName;
                urlbox = urlbox.Replace("mbox/foldmain.jsp?", "mbox/list.jsp?");
                url = string.Concat(new object[] { urlbox, "&fid=", boxId, "&page_no=", pageno });
                base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                base.streamControl = true;
                base.MyStringBuilder = this.Request(url);
                string message = base.MyStringBuilder.ToString();
                int index = 0;
                index = message.IndexOf("Ibx_gTable Ibx_gTable_Con");
                message = base.putstr(message, "<tbody>", "<tbody>", index);
                while ((startIndex = message.IndexOf("<tr", startIndex)) != -1)
                {
                    string str7 = base.putstr(message, "value=\"", "\"", startIndex).Trim();
                    if ((str7 != "-1") && (str7 != ""))
                    {
                        string strSql = "select count(*) from 163mailId where MsgId='" + str7 + "'";
                        if (Convert.ToInt32(GlobalValue.PopMainForm.ExecuteSQL(strSql)) == 0)
                        {
                            string str3 = HttpUtility.HtmlDecode(base.putstr(message, "Ibx_Td_From\" title=\"", "\">", startIndex));
                            string str4 = base.putstr(message, "Ibx_Td_Subject\" title=\"", "\">", startIndex);
                            string str5 = base.putstr(message, "Ibx_Td_Date\" title=\"", "\">", startIndex);
                            bool flag = false;
                            try
                            {
                                DateTime time = new DateTime();
                                time = Convert.ToDateTime(str5);
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
                                urlbox = urlbox.Replace("mbox/list.jsp?", "read/readdata.jsp?");
                                str2 = urlbox + "&mid=" + str7 + "&mode=download&part=0";
                                base.streamControl = true;
                                try
                                {
                                    this.RequestEmail(str2);
                                    try
                                    {
                                        string str9 = DateTime.Now.ToString();
                                        strSql = "insert into 163mailId(Name,MsgId,DownTime,MailType) values('" + base.m_username + "','" + str7 + "','" + str9 + "','" + base.BoxName.Substring(0, base.BoxName.Length % 10) + "')";
                                        GlobalValue.PopMainForm.ExecuteSQL(strSql);
                                    }
                                    catch (Exception exception)
                                    {
                                        base.ShowMessage("添加失败：" + exception.Message);
                                    }
                                    base.ShowMessage(base.BoxName + ":" + str4 + "\t下载");
                                }
                                catch (Exception exception2)
                                {
                                    base.ShowMessage("邮件下载失败:" + str4 + ":\t" + exception2.Message);
                                }
                            }
                        }
                    }
                    startIndex++;
                }
            }
            catch (Exception exception3)
            {
                base.ShowMessage("取邮件失败!" + exception3.Message);
            }
        }
        public override void login()
        {
            try
            {
                base.cookie = "";
                int startIndex = 0;
                string indata = "verifycookie=1&style=-1&product=mail163&username=" + base.m_username + "&password=" + base.m_passwd + "&selType=-1&secure=on&%B5%C7%C2%BC%D3%CA%CF%E4=";
                this.url = "http://reg.163.com/logins.jsp?type=1&url=http://fm163.163.com/coremail/fcg/ntesdoor2?lightweight%3D1%26verifycookie%3D1%26language%3D-1%26style%3D-1";
                base.ShowMessage("开始登陆…………");
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
                        if ((base.MyStringBuilder.ToString() != "") && (base.MyStringBuilder.ToString().IndexOf("163.com") != -1))
                        {
                            this.sid = base.putstr(base.MyStringBuilder.ToString(), "folder.jsp?sid=", "\"", 0);
                            base.ShowMessage("登陆成功!");
                            this.getBoxName();
                        }
                        else
                        {
                            startIndex = base.MyStringBuilder.ToString().IndexOf("window.location.replace");
                            if (startIndex != -1)
                            {
                                this.url = base.putstr(base.MyStringBuilder.ToString(), "\"", "\"", startIndex);
                                if ((this.url != "-1") && (this.url != null))
                                {
                                    base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                                    base.streamControl = true;
                                    base.MyStringBuilder = this.Request(this.url);
                                    if ((base.MyStringBuilder.ToString() != "") && (base.MyStringBuilder.ToString().IndexOf("@163.com") != -1))
                                    {
                                        base.ShowMessage("登陆成功!");
                                        this.getBoxName();
                                    }
                                }
                                else
                                {
                                    base.LoginFail();
                                    base.passwdErr();
                                    base.ShowMessage("登陆失败！");
                                }
                            }
                            else if (this.urls.IndexOf("j/js3/main.jsp") != -1)
                            {
                                this.url = this.urls.Replace("main.jsp", "index.jsp");
                                if ((this.url != "-1") && (this.url != null))
                                {
                                    base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                                    base.streamControl = true;
                                    base.MyStringBuilder = this.Request(this.url);
                                    if ((base.MyStringBuilder.ToString() != "") && (base.MyStringBuilder.ToString().IndexOf("@163.com") != -1))
                                    {
                                        base.ShowMessage("登陆成功!");
                                        this.getBoxName();
                                    }
                                }
                                else
                                {
                                    base.LoginFail();
                                    base.passwdErr();
                                    base.ShowMessage("登陆失败！");
                                }
                            }
                            else if (base.MyStringBuilder.ToString().IndexOf("3.5") != -1)
                            {
                                startIndex = base.MyStringBuilder.ToString().IndexOf("sid");
                                if (startIndex != -1)
                                {
                                    base.ShowMessage("登陆成功!");
                                    this.sid = base.putstr(base.MyStringBuilder.ToString(), "=", "\"", startIndex);
                                    this.url = this.urls.Replace("main.jsp", "s");
                                    this.url = this.url + "&func=mbox:getAllFolders";
                                    base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                                    base.streamControl = true;
                                    base.MyStringBuilder = this.Request(this.url);
                                    string strfolder = base.MyStringBuilder.ToString();
                                    this.getNewBoxName(strfolder);
                                    base.streamControl = true;
                                    this.RequestAddress(this.urls);
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
            catch (Exception exception)
            {
                base.LoginFail();
                base.passwdErr();
                base.ShowMessage("登陆失败！" + exception.Message);
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
                if (url.IndexOf("http://reg.163.com/logins.jsp") != -1)
                {
                    request.ContentType = "application/x-www-form-urlencoded";
                }
                else
                {
                    request.ContentType = "application/xml";
                }
                request.Headers.Add("Pragma:no-cache");
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
                    base.charSet = str.Substring(str.IndexOf("charset=") + 8, (str.Length - str.IndexOf("charset=")) - 8);
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
                    reader.Close();
                }
                reader.Close();
                this.booPost = true;
            }
            catch (Exception exception)
            {
                if (!this.booPost)
                {
                    this.booPost = true;
                    this.PostData(url, indata);
                }
                else
                {
                    base.ShowMessage("Hpost:" + exception.Message);
                }
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
                base.ShowMessage("Hrequest:" + exception.Message);
            }
            return base.MyStringBuilder;

        }
        private bool RequestAddress(string urls)
        {
            if (urls.IndexOf("js3/main.jsp") != -1)
            {
                urls = urls.Replace("js3/main.jsp", "js3/s");
            }
            this.url = urls + "&func=pab%3AexportContacts&outformat=8";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(this.url);
            request.Method = "GET";
            request.Headers.Add("cookie", base.cookie);
            request.Headers.Add("Accept-Language: zh-cn");
            request.ContentType = "Content-Type:application/octet-stream;charset=GBK";
            request.KeepAlive = true;
            request.AllowAutoRedirect = false;
            request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; EmbeddedWB 14.52 from: http://www.bsalsa.com/ EmbeddedWB 14.52; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022; InfoPath.3)";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Thread.Sleep(10);
            Stream responseStream = response.GetResponseStream();
            if (base.streamControl)
            {
                int num;
                byte[] buffer = new byte[0x1000];
                string text1 = DateTime.Now.ToString("yyyMMdd-HHmmss") + DateTime.Now.Millisecond.ToString();
                while ((num = responseStream.Read(buffer, 0, 0x1000)) > 0)
                {
                    if (!this.Save163Address(buffer, num))
                    {
                        return false;
                    }
                }
                responseStream.Close();
                base.streamControl = false;
                return true;
            }
            responseStream.Close();
            return false;

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
                this.m_emailTime = DateTime.Now.ToString("yyyMMdd-HHmmss") + DateTime.Now.Millisecond.ToString();
                while ((num = responseStream.Read(buffer, 0, 0x400)) > 0)
                {
                    if (!this.Save163mail(buffer, num))
                    {
                        return false;
                    }
                }
                responseStream.Close();
                base.saveEmailCount();
                GlobalValue.PopMainForm.statusBarPanelEmailNumbersText.Text = (Convert.ToInt32(GlobalValue.PopMainForm.statusBarPanelEmailNumbersText.Text) + 1).ToString();
                base.streamControl = false;
                return true;
            }
            responseStream.Close();
            return false;

        }
        private bool Save163Address(byte[] buffer, int nbytes)
        {
            FileStream stream = null;
            try
            {
                string path = "";
                if ((GlobalValue.PopMainForm.saveFilePath != "") | (GlobalValue.PopMainForm.saveFilePath != null))
                {
                    path = GlobalValue.PopMainForm.saveFilePath;
                }
                if (path == null)
                {
                    path = "";
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
                path = path + @"\地址簿";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                stream = File.Open(path + @"\" + base.m_username + "@" + base.m_serv + "_address.csv", FileMode.Create, FileAccess.Write);
                stream.Write(buffer, 0, nbytes);
                stream.Close();
                base.ShowMessage("地址簿下载完成");
            }
            catch (Exception exception)
            {
                base.ShowMessage("保存" + exception.Message);
            }
            finally
            {
                stream.Close();
            }
            return true;

        }
        public bool Save163mail(byte[] buffer, int nbytes)
        {
            try
            {
                string filePath = "";
                if ((GlobalValue.PopMainForm.saveFilePath != "") | (GlobalValue.PopMainForm.saveFilePath != null))
                {
                    filePath = GlobalValue.PopMainForm.saveFilePath;
                }
                this.Save163mailText(buffer, nbytes, filePath);
            }
            catch (Exception exception)
            {
                base.ShowMessage("保存邮件失败：" + exception.Message);
                return false;
            }
            return true;

        }
        public void Save163mailText(byte[] buffer, int nbytes, string filePath)
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
                base.ShowMessage("保存" + exception.Message);
            }
            finally
            {
                stream.Close();
            }
        }

        // Nested Types
        [StructLayout(LayoutKind.Sequential)]
        private struct BoxNameId
        {
            public string boxname;
            public string boxid;
            public int mailCount;
        }
    }
}
