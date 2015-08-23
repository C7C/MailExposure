using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading;
using System.Web;
using System.IO;
using System.Net;

namespace MailExposure.CookieMailRC
{
    class AolReceiver : MailStr
    {
        // Fields
        public bool booPost;
        private BoxNameId[] boxList;
        public string m_emailTime;
        private string url;
        private string urls;
        private string user;

        // Methods
        public AolReceiver()
        {
            this.boxList = new BoxNameId[200];
            this.urls = "";
            this.url = "";
            this.user = "";
        }
        private void getBoxName()
        {
            int startIndex = 0;
            int index = 0;
            int num3 = 1;
            if (this.urls.IndexOf("Suite.aspx") > 0)
            {
                this.url = this.urls.Replace("Suite.aspx", "Lite/Folders.aspx");
                base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                base.streamControl = true;
                base.MyStringBuilder = this.Request(this.url);
                string message = base.MyStringBuilder.ToString();
                startIndex = message.IndexOf("var gFolderInfo");
                if (startIndex > 0)
                {
                    message = base.putstr(message, "[", "]", startIndex);
                }
                for (startIndex = 0; (startIndex = message.IndexOf("folder:", startIndex)) != -1; startIndex++)
                {
                    string str2 = num3.ToString();
                    string str3 = base.putstr(message, "folder:\"", "\"", startIndex).Trim();
                    string str4 = base.putstr(message, "folderDisplay: \"", "\"", startIndex).Trim();
                    string str5 = base.putstr(message, "msgCount:\"", "\"", startIndex).Trim();
                    if (((str2 != "-1") && (str4 != "-1")) && ((str2 != "") && (str4 != "")))
                    {
                        this.boxList[index].folder = str3;
                        this.boxList[index].boxid = str2;
                        this.boxList[index].boxname = str4;
                        this.boxList[index].mailCount = Convert.ToInt32(str5);
                        index++;
                    }
                    num3++;
                }
                for (int i = 0; i < index; i++)
                {
                    this.getFirstPage(this.urls, this.boxList[i].boxname, this.boxList[i].boxid, this.boxList[i].mailCount, this.boxList[i].folder);
                }
            }

        }
        private void getEmailId(string urlbox, string boxName, string boxId, int mailCount, string folder, string message)
        {
            int startIndex = 0;
            base.BoxName = boxName;
            message = base.MyStringBuilder.ToString();
            int index = 0;
            index = message.IndexOf("</thead>");
            message = base.putstr(message, "</thead>", "aol.wsl.asRowViewUrls.push", index);
            if (message != "-1")
            {
                while ((startIndex = message.IndexOf("<tr", startIndex)) != -1)
                {
                    string mailId = base.putstr(message, "value=\"", ":", startIndex).Trim();
                    if ((mailId != "-1") && (mailId != ""))
                    {
                        string strSql = "select count(*) from AolmailId where MsgId='" + mailId + "'";
                        if (Convert.ToInt32(GlobalValue.mainForm.ExecuteSQL(strSql)) == 0)
                        {
                            bool flag = base.putstr(message, "row-unselected", "\"", startIndex).Trim().Equals("row-unread");
                            string str = HttpUtility.HtmlDecode(base.putstr(message, "message from", "subject", startIndex).Trim());
                            string str2 = base.putstr(message, "subject: \"", "\"", startIndex);
                            string str3 = base.putstr(message, "ShortestDate(", ",", startIndex).Trim();
                            DateTime time2 = new DateTime(0x7b2, 1, 1, 8, 0, 0, 0).AddMilliseconds(Convert.ToDouble(str3) + Convert.ToDouble(DateTime.Now.Millisecond));
                            this.m_emailTime = time2.ToString("yyyMMdd-HHmmss");
                            bool flag2 = false;
                            try
                            {
                                DateTime emailDateTime = GlobalValue.mainForm.EmailDateTime;
                                if (DateTime.Compare(time2, GlobalValue.mainForm.EmailDateTime) >= 0)
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
                                this.url = urlbox.Replace("Suite.aspx", "Lite/ViewSource.aspx");
                                this.url = this.url + "?user=" + this.user + "&folder=" + folder + "&uid=" + mailId;
                                base.streamControl = true;
                                try
                                {
                                    this.RequestEmail(this.url, mailId);
                                    try
                                    {
                                        if (flag)
                                        {
                                            this.urls = this.urls.Substring(0, 0x2d) + "Suite.aspx";
                                            string url = this.urls.Replace("Suite.aspx", "common/rpc/RPC.aspx") + "?user=" + this.user + "&transport=xmlhttp&r=0.9981021919620518&a=MessageAction&s=328&l=30452";
                                            string indata = "requests=%5B%7B%22messageAction%22%3A%22unseen%22%2C%22folder%22%3A%22" + folder + "%22%2C%22uids%22%3A%7B%22" + boxName + "%22%3A%5B%22" + mailId + "%22%5D%7D%2C%22destFolder%22%3Aundefined%2C%22isSpam%22%3Aundefined%2C%22checkUndo%22%3Afalse%2C%22screenName%22%3A%22heihei2010%22%2C%22reason%22%3Aundefined%2C%22isAllSelected%22%3Afalse%2C%22isUndoAction%22%3Afalse%2C%22action%22%3A%22MessageAction%22%7D%5D&automatic=false";
                                            this.PostData(url, indata);
                                            Thread.Sleep(10);
                                            base.ShowMessage(str2 + "置未读");
                                        }
                                    }
                                    catch (Exception exception)
                                    {
                                        base.ShowMessage("置未读失败：" + exception.Message);
                                    }
                                    try
                                    {
                                        string str8 = DateTime.Now.ToString();
                                        strSql = "insert into AolmailId(Name,MsgId,DownTime,MailType) values('" + base.m_username + "','" + mailId + "','" + str8 + "','" + base.BoxName.Substring(0, base.BoxName.Length % 10) + "')";
                                        GlobalValue.mainForm.ExecuteSQL(strSql);
                                    }
                                    catch (Exception exception2)
                                    {
                                        base.ShowMessage("添加失败：" + exception2.Message);
                                    }
                                    base.ShowMessage(base.BoxName + ":" + str2 + "\t下载");
                                }
                                catch (Exception exception3)
                                {
                                    base.ShowMessage("邮件下载失败:" + str2 + ":\t" + exception3.Message);
                                }
                            }
                        }
                    }
                    startIndex++;
                }
            }
            else
            {
                base.ShowMessage(boxName + " 是空的!");
            }

        }
        private bool getFirstPage(string urls, string boxName, string boxId, int mailCount, string folder)
        {
            urls = urls.Substring(0, 0x2d) + "Suite.aspx";
            this.url = urls.Replace("Suite.aspx", "Lite/MsgList.aspx");
            string indata = "user=" + this.user + "&folder=" + folder + "&msgActionRequest=Go&toolbarUniquifier=&msgActionMenuL10n=Actions&filterAction=none";
            base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
            base.streamControl = true;
            base.MyStringBuilder = this.PostData(this.url, indata);
            string message = base.MyStringBuilder.ToString();
            if ((base.MyStringBuilder != null) || (base.MyStringBuilder.ToString() != ""))
            {
                base.ShowMessage(boxName + "：第1页开始下载");
                this.getEmailId(urls, boxName, boxId, mailCount, folder, base.MyStringBuilder.ToString());
                base.ShowMessage(base.BoxName + "：第1页下载完毕");
            }
            int index = message.IndexOf("title=\"Previous page\"");
            message = base.putstr(message, "\"Previous page\"", "id=\"nextPage\"", index);
            index = 0;
            index = message.IndexOf("href='", index);
            string str3 = base.putstr(message, "href='", "'", index);
            if ((str3 != "") && (str3 != "-1"))
            {
                this.getNextPage(str3, boxName, boxId, mailCount, folder);
            }
            else
            {
                base.ShowMessage(boxName + "：下载完毕");
            }
            return true;

        }
        private bool getNextPage(string urls, string boxName, string boxId, int mailCount, string folder)
        {
            base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
            base.streamControl = true;
            base.MyStringBuilder = this.Request(urls);
            string message = base.MyStringBuilder.ToString();
            int index = message.IndexOf("title=\"Previous page\"");
            message = base.putstr(message, "\"Previous page\"", "id=\"nextPage\"", index);
            string[] strArray = new string[5];
            int[] numArray = new int[5];
            index = 0;
            for (int i = 0; i < 5; i++)
            {
                index = message.IndexOf("href='", index);
                strArray[i] = base.putstr(message, "href='", "'", index);
                numArray[i] = Convert.ToInt16(base.putstr(message, "title='Page ", "'", index));
                index++;
            }
            for (int j = 0; j < 5; j++)
            {
                if ("-1" == strArray[j + 1])
                {
                    return true;
                }
                if (urls.Equals(strArray[j]))
                {
                    if (Convert.ToInt32(GlobalValue.mainForm.PageNumber) == 0)
                    {
                        base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                        base.streamControl = true;
                        base.MyStringBuilder = this.Request(strArray[j + 1]);
                        if ((base.MyStringBuilder != null) || (base.MyStringBuilder.ToString() != ""))
                        {
                            base.ShowMessage(string.Concat(new object[] { base.BoxName, "：第", numArray[j + 1], "页开始下载" }));
                            this.getEmailId(urls, boxName, boxId, mailCount, folder, base.MyStringBuilder.ToString());
                            base.ShowMessage(string.Concat(new object[] { base.BoxName, "：第", numArray[j + 1], "页下载完毕" }));
                            this.getNextPage(strArray[j + 1], boxName, boxId, mailCount, folder);
                        }
                    }
                    else if (numArray[j + 1] <= Convert.ToInt32(GlobalValue.mainForm.PageNumber))
                    {
                        base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                        base.streamControl = true;
                        base.MyStringBuilder = this.Request(strArray[j + 1]);
                        if ((base.MyStringBuilder != null) || (base.MyStringBuilder.ToString() != ""))
                        {
                            base.ShowMessage(string.Concat(new object[] { base.BoxName, "：第", numArray[j + 1], "页开始下载" }));
                            this.getEmailId(urls, boxName, boxId, mailCount, folder, base.MyStringBuilder.ToString());
                            base.ShowMessage(string.Concat(new object[] { base.BoxName, "：第", numArray[j + 1], "页下载完毕" }));
                            this.getNextPage(strArray[j + 1], boxName, boxId, mailCount, folder);
                        }
                    }
                }
            }
            return true;

        }
        public override void login()
        {
            base.cookie = "";
            if (base.m_UserType.IndexOf("无密用户") != -1)
            {
                base.ShowMessage("开始登陆…………");
                base.cookie = base.validationLogin;
                string message = base.cookie.ToString();
                this.user = base.putstr(message, "uid:", "&", 0);
                this.url = base.emailuri;
                base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                this.urls = this.url.Substring(0, 0x2d) + "Suite.aspx";
                base.streamControl = true;
                base.MyStringBuilder = this.Request(this.url);
                if ((base.MyStringBuilder.ToString() != "") && (base.MyStringBuilder.ToString().IndexOf("@aol.com") != -1))
                {
                    base.ShowMessage("登陆成功!");
                    this.getBoxName();
                    base.streamControl = true;
                    this.RequestAddress("http://webmail.aol.com/31423-111/aol-1/en-us/Lite/ABExport.aspx?command=all&format=csv&user=" + this.user);
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
                base.ShowMessage("开始登陆……");
                this.url = "http://webmail.aol.com/";
                base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                base.streamControl = true;
                base.MyStringBuilder = this.Request(this.url);
                string str2 = base.MyStringBuilder.ToString();
                string str3 = "";
                string str4 = "";
                int startIndex = 0;
                startIndex = str2.IndexOf("name=\"siteState\"");
                if (startIndex > 0)
                {
                    str3 = HttpUtility.UrlEncode(base.putstr(str2, "value=\"", "\"", startIndex));
                }
                startIndex = str2.IndexOf("name=\"usrd\"");
                if (startIndex > 0)
                {
                    str4 = base.putstr(str2, "value=\"", "\"", startIndex);
                }
                if (((str3 != "") && (str4 != "")) && ((str3 != "-1") && (str4 != "-1")))
                {
                    this.url = "https://my.screenname.aol.com/_cqr/login/login.psp";
                    string indata = "sitedomain=sns.webmail.aol.com&siteId=&lang=en&locale=us&authLev=0&siteState=" + str3 + "&isSiteStateEncoded=true&mcState=initialized&uitype=std&use_aam=0&_sns_fg_color_=&_sns_err_color_=&_sns_link_color_=&_sns_width_=&_sns_height_=&_sns_bg_color_=&offerId=newmail-en-us-v2&seamless=novl&regPromoCode=&idType=SN&usrd=" + str4 + "&doSSL=&redirType=&xchk=false&tab=aol&loginId=" + base.m_username + "&password=" + base.m_passwd;
                    base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                    base.streamControl = true;
                    base.MyStringBuilder = this.PostData(this.url, indata);
                    str2 = base.MyStringBuilder.ToString();
                    if (str2.IndexOf("Invalid Username or Password. Please try again.") != -1)
                    {
                        base.LoginFail();
                        base.passwdErr();
                        base.ShowMessage("用户名或密码错误！");
                    }
                    else if (str2.IndexOf("checkErrorAndSubmitForm(") != -1)
                    {
                        string[] strArray = base.putstr(str2, "checkErrorAndSubmitForm('", ")", 0).Split(new char[] { ',' });
                        this.url = base.putstr(strArray[2], "'", "'", 0);
                        if ((this.url != "") && (this.url != "-1"))
                        {
                            base.streamControl = true;
                            base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                            base.MyStringBuilder = this.Request(this.url);
                            string str7 = base.cookie.ToString();
                            this.user = base.putstr(str7, "uid:", "&", 0);
                            str2 = base.MyStringBuilder.ToString();
                            int index = str2.IndexOf("var gPreferredHost");
                            if (index > 0)
                            {
                                base.Host = "http://" + base.putstr(str2, "\"", "\"", index);
                                index = str2.IndexOf("var gSuccessPath");
                                if (index > 0)
                                {
                                    this.urls = base.Host + base.putstr(str2, "\"", "\"", index);
                                    base.streamControl = true;
                                    base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                                    base.MyStringBuilder = this.Request(this.urls);
                                    str2 = base.MyStringBuilder.ToString();
                                    index = str2.IndexOf("el.innerHTML");
                                    if (index > 0)
                                    {
                                        this.url = base.putstr(str2, "'", "'", index);
                                        if ((this.url != "") && (this.url != "-1"))
                                        {
                                            base.ShowMessage("登陆成功！");
                                            base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                                            base.streamControl = true;
                                            base.MyStringBuilder = this.Request(this.url);
                                            this.getBoxName();
                                            this.url = this.urls.Replace("Suite.aspx", "AB/ABExport.aspx");
                                            this.url = this.url + "?command=all&format=csv&user=" + this.user;
                                            base.streamControl = true;
                                            this.RequestAddress(this.url);
                                        }
                                        else
                                        {
                                            base.LoginFail();
                                            base.passwdErr();
                                            base.ShowMessage("登陆失败……");
                                        }
                                    }
                                    else
                                    {
                                        base.LoginFail();
                                        base.passwdErr();
                                        base.ShowMessage("登陆失败……");
                                    }
                                }
                                else
                                {
                                    base.LoginFail();
                                    base.passwdErr();
                                    base.ShowMessage("登陆失败……");
                                }
                            }
                            else
                            {
                                base.LoginFail();
                                base.passwdErr();
                                base.ShowMessage("登陆失败……");
                            }
                        }
                        else
                        {
                            base.LoginFail();
                            base.passwdErr();
                            base.ShowMessage("登陆失败……");
                        }
                    }
                    else
                    {
                        base.LoginFail();
                        base.passwdErr();
                        base.ShowMessage("登陆失败……");
                    }
                }
                else
                {
                    base.LoginFail();
                    base.passwdErr();
                    base.ShowMessage("登陆失败……");
                }
            }

        }
        private StringBuilder PostData(string url, string indata)
        {
            try
            {
                StreamReader reader;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.ServicePoint.Expect100Continue = false;
                request.Method = "POST";
                request.Headers.Add("Cookie", base.cookie);
                request.ContentType = "application/x-www-form-urlencoded";
                request.Accept = "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, application/x-ms-application, application/x-ms-xbap, application/vnd.ms-xpsdocument, application/xaml+xml, */*";
                request.Headers.Add("Accept-Language: en-us,ar-SA;q=0.9,de-DE;q=0.8,es-ES;q=0.7,tr-TR;q=0.6,ja-JP;q=0.5,en-GB;q=0.4,fr-FR;q=0.3,zh-CN;q=0.2,zh-TW;q=0.1");
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1) ; EmbeddedWB 14.52 from: http://www.bsalsa.com/ EmbeddedWB 14.52; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022; InfoPath.3)";
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
                request.Method = "get";
                request.Accept = "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, application/x-ms-application, application/x-ms-xbap, application/vnd.ms-xpsdocument, application/xaml+xml, */*";
                request.AllowAutoRedirect = false;
                request.ServicePoint.Expect100Continue = false;
                request.KeepAlive = true;
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1) ; EmbeddedWB 14.52 from: http://www.bsalsa.com/ EmbeddedWB 14.52; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022; InfoPath.3)";
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
                        string str2 = base.putstr(this.urls, "http:", "/mail/", 0);
                        if ((str2 != "-1") && (base.Host == null))
                        {
                            base.Host = "http:" + str2 + "/mail/";
                        }
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
                    for (string str3 = reader.ReadLine(); str3 != null; str3 = reader.ReadLine())
                    {
                        base.MyStringBuilder.Append(str3 + "\r\n");
                        str3 = null;
                    }
                    base.streamControl = false;
                    reader.Close();
                }
                reader.Close();
            }
            catch (Exception exception)
            {
                base.ShowMessage("Hrequest:" + exception.Message);
            }
            return base.MyStringBuilder;

        }
        private bool RequestAddress(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.Headers.Add("cookie", base.cookie);
            request.Headers.Add("Accept-Language: zh-cn");
            request.ContentType = "text/csv; charset=UTF-8";
            request.KeepAlive = true;
            request.AllowAutoRedirect = false;
            request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1) ; EmbeddedWB 14.52 from: http://www.bsalsa.com/ EmbeddedWB 14.52; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022; InfoPath.3)";
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
                    if (!this.SaveAolAddress(buffer, num))
                    {
                        return false;
                    }
                }
                base.ShowMessage("地址簿下载完成");
                responseStream.Close();
                base.streamControl = false;
                return true;
            }
            responseStream.Close();
            return false;

        }
        private bool RequestEmail(string url, string mailId)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Headers.Add("Cookie", base.cookie);
            request.Method = "GET";
            request.Accept = "*/*";
            request.AllowAutoRedirect = false;
            request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1) ; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022; InfoPath.3)";
            request.ContentType = "text/plain; name='Message" + mailId + ".txt'";
            request.Headers.Add("Accept-Language: zh-cn");
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Thread.Sleep(10);
            Stream responseStream = response.GetResponseStream();
            if (base.streamControl)
            {
                int num;
                byte[] buffer = new byte[0x1000];
                base.m_emailno++;
                while ((num = responseStream.Read(buffer, 0, 0x1000)) > 0)
                {
                    if (!this.SaveAolmail(buffer, num))
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
        private bool SaveAolAddress(byte[] buffer, int nbytes)
        {
            FileStream stream = null;
            try
            {
                string path = "";
                if ((GlobalValue.mainForm.saveFilePath != "") | (GlobalValue.mainForm.saveFilePath != null))
                {
                    path = GlobalValue.mainForm.saveFilePath;
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

        public bool SaveAolmail(byte[] buffer, int nbytes)
        {
            try
            {
                string filePath = "";
                if ((GlobalValue.mainForm.saveFilePath != "") | (GlobalValue.mainForm.saveFilePath != null))
                {
                    filePath = GlobalValue.mainForm.saveFilePath;
                }
                this.SaveAolmailText(buffer, nbytes, filePath);
            }
            catch (Exception exception)
            {
                base.ShowMessage("保存邮件失败：" + exception.Message);
                return false;
            }
            return true;

        }

        public void SaveAolmailText(byte[] buffer, int nbytes, string filePath)
        {
            FileStream stream = null;
            try
            {
                string str = ".eml";
                if (filePath == null)
                {
                    filePath = "";
                }
                string str2 = DateTime.Now.Date.ToString("yyyy-MM-dd");
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
            public string folder;
        }

    }
}
