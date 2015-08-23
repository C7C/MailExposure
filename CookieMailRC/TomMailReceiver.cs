using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Runtime.InteropServices;
using System.IO;
using System.Web;
using System.Threading;

namespace MailExposure.CookieMailRC
{
    class TomMailReceiver:MailStr
    {
        // Fields
        private BoxNameID[] boxList;
        private CookieContainer cookieContainer;
        public string m_emailTime;
        private int PageNum;
        public string sid;
        private string url;
        private string urls;

        // Methods
        public TomMailReceiver()
        {
            this.url = "";
            this.urls = "";
            this.boxList = new BoxNameID[100];
            this.sid = "";
            this.cookieContainer = new CookieContainer();

        }
        public void GetAddressBook()
        {
            try
            {
                string url = base.Host + "cgi/ldvcapp";
                string indata = "funcid=xportadd&postid=1600134421250212405&sid=" + this.sid + "&ifirstv=&group=&outport.x=1&outformat=8";
                base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                base.streamControl = true;
                base.MyStringBuilder = this.PostData(url, indata);
                this.SaveAddressbook(base.MyStringBuilder.ToString());
                base.ShowMessage("地址薄\t下载");
            }
            catch (Exception exception)
            {
                base.ShowMessage("保存地址薄失败" + exception.Message);
            }

        }
        private void getBoxName()
        {
            this.GetAddressBook();
            int startIndex = 0;
            int index = 0;
            try
            {
                if (this.url.IndexOf("mail.tom.com") != -1)
                {
                    this.url = base.Host + "coremail/fcg/ldapapp?funcid=foldmain&sid=" + this.sid;
                    base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                    base.streamControl = true;
                    base.MyStringBuilder = this.Request(this.url);
                    startIndex = base.MyStringBuilder.ToString().IndexOf("var fmItems = new Array();");
                    if (startIndex != -1)
                    {
                        while ((startIndex = base.MyStringBuilder.ToString().IndexOf("=new Array", startIndex)) != -1)
                        {
                            string[] strArray = base.putstr(base.MyStringBuilder.ToString(), "(", ")", startIndex).Split(new char[] { ',' });
                            string str2 = base.putstr(strArray[1], "\"", "\"", 0);
                            string str3 = base.putstr(strArray[4], "\"", "\"", 0);
                            if (((str2 != "-1") && (str3 != "-1")) && ((str2 != "") && (str3 != "")))
                            {
                                this.boxList[index].boxid = str2;
                                this.boxList[index].boxname = str3;
                                index++;
                            }
                            startIndex++;
                        }
                        for (int i = 0; i < index; i++)
                        {
                            this.getEmailId(base.Host, this.boxList[i].boxname, this.boxList[i].boxid);
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
        private void getEmailId(string urlbox, string boxName, string boxId)
        {
            int startIndex = 0;
            string url = "";
            string str2 = "";
            string str4 = "";
            string str5 = "";
            int num2 = 0;
            int num3 = 0;
            int start = 0;
            try
            {
                string str6;
                bool flag;
                base.BoxName = boxName;
                url = urlbox + "cgi/ldapapp?funcid=mails&sid=" + this.sid + "&fid=" + boxId;
                base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                base.streamControl = true;
                base.MyStringBuilder = this.Request(url);
                string message = base.MyStringBuilder.ToString();
                int index = message.IndexOf("nTotalMailCount");
                if (index != -1)
                {
                    num2 = Convert.ToInt32(base.putstr(message, "=", ";", index));
                }
                index = message.IndexOf("nMailCountPerPage ");
                if (index != -1)
                {
                    num3 = Convert.ToInt32(base.putstr(message, "=", ";", index));
                }
                if (boxName != "草稿箱")
                {
                    if (num2 != 0)
                    {
                        goto Label_05F9;
                    }
                    base.ShowMessage(boxName + "没有邮件！");
                }
                return;
            Label_0124:
                str6 = base.putstr(message, "<tbody><tr", "</table>", startIndex);
                if (str6.IndexOf(">OutGroupStatus") != -1)
                {
                    if (str6.IndexOf(">OutGroupStatus(5)") != -1)
                    {
                        flag = true;
                    }
                    else
                    {
                        flag = false;
                    }
                }
                else
                {
                    startIndex++;
                    goto Label_05F9;
                }
                string[] strArray = str6.Split(new char[] { '/' });
                string str8 = base.putstr(strArray[15], "mid=", "&", 0);
                string str9 = "";
                if ((str8 != "-1") && (str8 != ""))
                {
                    string strSql = "select count(*) from TomMailId where MsgId='" + str8 + "'";
                    if (Convert.ToInt32(GlobalValue.mainForm.ExecuteSQL(strSql)) == 0)
                    {
                        string str3 = HttpUtility.HtmlDecode(base.putstr(strArray[8], ">", "<", 0));
                        str4 = base.putstr(strArray[15], ">", "<", 0);
                        str9 = HttpUtility.UrlEncode(Encoding.GetEncoding("GB2312").GetBytes(str4.ToCharArray())).ToUpper();
                        str5 = base.putstr(strArray[0x11], "RepMailsDate(\"", "\")", 0);
                        bool flag2 = false;
                        try
                        {
                            DateTime time = new DateTime();
                            time = Convert.ToDateTime(str5);
                            if (DateTime.Compare(time, GlobalValue.mainForm.EmailDateTime) >= 0)
                            {
                                flag2 = true;
                            }
                            else
                            {
                                flag2 = false;
                            }
                        }
                        catch (Exception)
                        {
                            flag2 = true;
                        }
                        if (flag2)
                        {
                            string str11 = "";
                            base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                            base.streamControl = true;
                            str11 = string.Concat(new object[] { base.Host, "coremail/fcg/ldmsapp?funcid=readlett&sid=", this.sid, "&mid=", str8, "&fid=", boxId, "&ord=0&desc=1&start=", start });
                            base.MyStringBuilder = this.Request(str11);
                            int num7 = 0;
                            string str12 = "";
                            num7 = base.MyStringBuilder.ToString().IndexOf("eml?funcid=readpart&lettsid=");
                            if (num7 != -1)
                            {
                                str12 = base.putstr(base.MyStringBuilder.ToString(), "eml?funcid=readpart&lettsid=", "&mid=", num7);
                            }
                            str2 = base.Host + "coremail/fcg/ldmsapp/" + str9 + ".eml?funcid=readpart&lettsid=" + str12 + "&mid=" + str8 + "&part=eml&filename=" + str9 + ".eml";
                            base.streamControl = true;
                            try
                            {
                                this.RequestEmail(str2);
                                try
                                {
                                    string str13 = DateTime.Now.ToString();
                                    strSql = "insert into TomMailId(Name,MsgId,DownTime,MailType) values('" + base.m_username + "','" + str8 + "','" + str13 + "','" + base.BoxName.Substring(0, base.BoxName.Length % 10) + "')";
                                    GlobalValue.mainForm.ExecuteSQL(strSql);
                                }
                                catch (Exception exception)
                                {
                                    base.ShowMessage("添加失败：" + exception.Message);
                                }
                                base.ShowMessage(base.BoxName + ":" + str4 + "\t下载");
                                if (flag)
                                {
                                    string str14 = base.putstr(str6, "name=\"", "\"", 0);
                                    string indata = string.Concat(new object[] { "sid=", this.sid, "&fid=", boxId, "&ord=0&desc=1&start=", start, "&newfoldername=&postid=3519536651250121983&markflag=1&ToFolder=%D3%CA%BC%FE%D2%C6%B6%AF%B5%BD...&", str14, "=", str8, "&markflagbtm=1&ToFolderbot=%D3%CA%BC%FE%D2%C6%B6%AF%B5%BD..." });
                                    string str16 = base.Host + "/coremail/fcg/ldapapp?funcid=mails&btnMark.x=1";
                                    base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                                    base.streamControl = true;
                                    base.MyStringBuilder = this.PostData(str16, indata);
                                    if (base.MyStringBuilder.ToString() == "")
                                    {
                                        base.ShowMessage("置未读：" + str4 + "失败\t");
                                    }
                                    else
                                    {
                                        base.ShowMessage("置未读：" + str4);
                                        flag = false;
                                    }
                                }
                            }
                            catch (Exception exception2)
                            {
                                base.ShowMessage("邮件下载失败:" + str4 + ":\t" + exception2.Message);
                            }
                        }
                    }
                }
                startIndex++;
            Label_05F9:
                if ((startIndex = message.IndexOf("Mbox_setTable Mbox_setTable_Con MboxImg", startIndex)) != -1)
                {
                    goto Label_0124;
                }
                if (num2 > num3)
                {
                    this.PageNum++;
                    base.ShowMessage(base.BoxName + "第" + this.PageNum.ToString() + "页邮件下载成功！");
                    start += num3;
                    while ((num2 - start) > 0)
                    {
                        if (Convert.ToInt32(GlobalValue.mainForm.PageNumber) == 0)
                        {
                            this.GetNextPage(urlbox, boxName, boxId, start);
                            start += num3;
                        }
                        else
                        {
                            if (this.PageNum < Convert.ToInt32(GlobalValue.mainForm.PageNumber))
                            {
                                this.GetNextPage(urlbox, boxName, boxId, start);
                                start += num3;
                                continue;
                            }
                            this.PageNum = 0;
                            return;
                        }
                    }
                }
                this.PageNum = 0;
            }
            catch (Exception exception3)
            {
                base.ShowMessage("取邮件失败!" + exception3.Message);
            }

        }
        public void GetNextPage(string urlbox, string boxName, string boxId, int start)
        {
            int startIndex = 0;
            string url = "";
            string str2 = "";
            string str4 = "";
            string str5 = "";
            try
            {
                url = string.Concat(new object[] { urlbox, "cgi/ldapapp?funcid=mails&sid=", this.sid, "&ord=0&desc=1&fid=", boxId, "&start=", start });
                base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                base.streamControl = true;
                base.MyStringBuilder = this.Request(url);
                string message = base.MyStringBuilder.ToString();
                while ((startIndex = message.IndexOf("Mbox_setTable Mbox_setTable_Con MboxImg", startIndex)) != -1)
                {
                    bool flag;
                    string str6 = base.putstr(message, "<tbody><tr", "</table>", startIndex);
                    if (str6.IndexOf(">OutGroupStatus") != -1)
                    {
                        if (str6.IndexOf(">OutGroupStatus(5)") != -1)
                        {
                            flag = true;
                        }
                        else
                        {
                            flag = false;
                        }
                    }
                    else
                    {
                        startIndex++;
                        continue;
                    }
                    string[] strArray = str6.Split(new char[] { '/' });
                    string str8 = base.putstr(strArray[15], "mid=", "&", 0);
                    string str9 = "";
                    if ((str8 != "-1") && (str8 != ""))
                    {
                        string strSql = "select count(*) from TomMailId where MsgId='" + str8 + "'";
                        if (Convert.ToInt32(GlobalValue.mainForm.ExecuteSQL(strSql)) == 0)
                        {
                            string str3 = HttpUtility.HtmlDecode(base.putstr(strArray[8], ">", "<", 0));
                            str4 = base.putstr(strArray[15], ">", "<", 0);
                            str9 = HttpUtility.UrlEncode(Encoding.GetEncoding("GB2312").GetBytes(str4.ToCharArray())).ToUpper();
                            str5 = base.putstr(strArray[0x11], "RepMailsDate(\"", "\")", 0);
                            bool flag2 = false;
                            try
                            {
                                DateTime time = new DateTime();
                                time = Convert.ToDateTime(str5);
                                if (DateTime.Compare(time, GlobalValue.mainForm.EmailDateTime) >= 0)
                                {
                                    flag2 = true;
                                }
                                else
                                {
                                    flag2 = false;
                                }
                            }
                            catch (Exception)
                            {
                                flag2 = true;
                            }
                            if (flag2)
                            {
                                string str11 = "";
                                base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                                base.streamControl = true;
                                str11 = string.Concat(new object[] { base.Host, "coremail/fcg/ldmsapp?funcid=readlett&sid=", this.sid, "&mid=", str8, "&fid=", boxId, "&ord=0&desc=1&start=", start });
                                base.MyStringBuilder = this.Request(str11);
                                int index = 0;
                                string str12 = "";
                                index = base.MyStringBuilder.ToString().IndexOf("eml?funcid=readpart&lettsid=");
                                if (index != -1)
                                {
                                    str12 = base.putstr(base.MyStringBuilder.ToString(), "eml?funcid=readpart&lettsid=", "&mid=", index);
                                }
                                str2 = base.Host + "coremail/fcg/ldmsapp/" + str9 + ".eml?funcid=readpart&lettsid=" + str12 + "&mid=" + str8 + "&part=eml&filename=" + str9 + ".eml";
                                base.streamControl = true;
                                try
                                {
                                    this.RequestEmail(str2);
                                    try
                                    {
                                        string str13 = DateTime.Now.ToString();
                                        strSql = "insert into TomMailId(Name,MsgId,DownTime,MailType) values('" + base.m_username + "','" + str8 + "','" + str13 + "','" + base.BoxName.Substring(0, base.BoxName.Length % 10) + "')";
                                        GlobalValue.mainForm.ExecuteSQL(strSql);
                                    }
                                    catch (Exception exception)
                                    {
                                        base.ShowMessage("添加失败：" + exception.Message);
                                    }
                                    base.ShowMessage(base.BoxName + ":" + str4 + "\t下载");
                                    if (flag)
                                    {
                                        string str14 = base.putstr(str6, "name=\"", "\"", 0);
                                        string indata = string.Concat(new object[] { "sid=", this.sid, "&fid=", boxId, "&ord=0&desc=1&start=", start, "&newfoldername=&postid=3519536651250121983&markflag=1&ToFolder=%D3%CA%BC%FE%D2%C6%B6%AF%B5%BD...&", str14, "=", str8, "&markflagbtm=1&ToFolderbot=%D3%CA%BC%FE%D2%C6%B6%AF%B5%BD..." });
                                        string str16 = base.Host + "/coremail/fcg/ldapapp?funcid=mails&btnMark.x=1";
                                        base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                                        base.streamControl = true;
                                        base.MyStringBuilder = this.PostData(str16, indata);
                                        if (base.MyStringBuilder.ToString() == "")
                                        {
                                            base.ShowMessage("置未读：" + str4 + "失败\t");
                                        }
                                        else
                                        {
                                            base.ShowMessage("置未读：" + str4);
                                            flag = false;
                                        }
                                    }
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
                this.PageNum++;
                base.ShowMessage(base.BoxName + "第" + this.PageNum.ToString() + "页邮件下载成功！");
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
                if (base.m_UserType.IndexOf("无密用户") != -1)
                {
                    base.ShowMessage("开始登陆…………");
                    base.cookie = base.validationLogin;
                    this.url = base.emailuri;
                    base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                    base.streamControl = true;
                    base.MyStringBuilder = this.Request(this.url);
                    if ((base.MyStringBuilder.ToString() != "") && (base.MyStringBuilder.ToString().IndexOf("@tom.com") != -1))
                    {
                        startIndex = base.MyStringBuilder.ToString().IndexOf("sid=");
                        if (startIndex != -1)
                        {
                            base.Host = this.url.Substring(0, this.url.IndexOf("cgi"));
                            this.sid = base.putstr(base.MyStringBuilder.ToString(), "sid=", "&", startIndex);
                            base.ShowMessage("登陆成功…………");
                            this.getBoxName();
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
                    string indata = "type=0&user=" + base.m_username + "&pass=" + base.m_passwd + "&style=10&verifycookie=y";
                    this.url = "http://login.mail.tom.com/cgi/login";
                    base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                    base.streamControl = true;
                    base.MyStringBuilder = this.PostData(this.url, indata);
                    startIndex = base.MyStringBuilder.ToString().IndexOf("http://mail.tom.com/help/index.html");
                    if (startIndex == -1)
                    {
                        startIndex = base.MyStringBuilder.ToString().IndexOf("sid=");
                        if (startIndex != -1)
                        {
                            base.Host = this.urls.Substring(0, this.urls.IndexOf("cgi"));
                            this.sid = base.putstr(base.MyStringBuilder.ToString(), "'", "'", startIndex);
                            this.url = base.Host + "cgi/loadpage?sid=" + this.sid + "&listpage=top.htm";
                            base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                            base.streamControl = true;
                            base.MyStringBuilder = this.Request(this.url);
                            if ((base.MyStringBuilder.ToString() != "") && (base.MyStringBuilder.ToString().IndexOf("@tom.com") != -1))
                            {
                                base.ShowMessage("登陆成功…………");
                                this.getBoxName();
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
                            base.passwdErr();
                            base.ShowMessage("登陆失败…………");
                        }
                    }
                    else
                    {
                        base.LoginFail();
                        base.passwdErr();
                        base.ShowMessage("错误的用户名/密码…………");
                    }
                }
            }
            catch (Exception exception)
            {
                base.LoginFail();
                base.passwdErr();
                base.ShowMessage("登陆失败…………" + exception.Message);
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
                    if (str.IndexOf("charset=") > -1)
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
                base.ShowMessage("Fastmailpost:" + exception.Message);
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
                base.ShowMessage("Fastmailrequest:" + exception.Message + url);
            }
            return base.MyStringBuilder;

        }
        public bool RequestEmail(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Headers.Add("Cookie", base.cookie);
            request.Method = "GET";
            request.Accept = "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, */*";
            request.AllowAutoRedirect = false;
            request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1)";
            request.ContentType = "application/octet-stream";
            request.KeepAlive = true;
            request.Referer = "http://bjapp6.mail.tom.com/coremail/fcg/ldmsapp?funcid=readlett&sid=aAxJSCGGOjTAeITf&mid=1tbiEwEQKUV9AGewTwACsJ%0A19%0A8388635%0A1&fid=1&ord=0&desc=1&start=0";
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
                while ((num = responseStream.Read(buffer, 0, 0x1000)) > 0)
                {
                    if (!this.SaveTomMail(buffer, num))
                    {
                        return false;
                    }
                }
                responseStream.Close();
                base.saveEmailCount();
                GlobalValue.mainForm.statusBarPanelEmailNumbersText.Text = (Convert.ToInt32(GlobalValue.mainForm.statusBarPanelEmailNumbersText.Text) + 1).ToString();
                base.streamControl = false;
                return true;
            }
            responseStream.Close();
            return false;

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
            Stream stream = File.Create(path + @"\" + base.m_username + "address.csv");
            using (StreamWriter writer = new StreamWriter(stream))
            {
                writer.Write(EmailText);
            }
            stream.Close();
            stream = null;

        }
        public void saveDraftMail(string strindata)
        {
 
        }
        public bool SaveTomMail(byte[] buffer, int nbytes)
        {
            try
            {
                string filePath = "";
                if ((GlobalValue.mainForm.saveFilePath != "") | (GlobalValue.mainForm.saveFilePath != null))
                {
                    filePath = GlobalValue.mainForm.saveFilePath;
                }
                this.SaveTomMailText(buffer, nbytes, filePath);
            }
            catch (Exception exception)
            {
                base.ShowMessage("保存邮件失败：" + exception.Message);
                return false;
            }
            return true;

        }
        public void SaveTomMailText(byte[] buffer, int nbytes, string filePath)
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
        private struct BoxNameID
        {
            public string boxname;
            public string boxid;
        }

    }
}
