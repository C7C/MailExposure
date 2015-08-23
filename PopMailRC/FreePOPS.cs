using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Web;

namespace MailExposure.PopMailRC
{
    class FreePOPS:MailStr
    {
        // Fields
        private bool China;
        private CookieContainer cookieContainer;
        private string cookieTemp;
        private bool download;
        private int emailCount;
        private NetworkStream netWorkStream;
        private string request;
        private byte[] requestData;
        private string response;
        private string SQL;
        private StreamReader streamReader;
        private TcpClient tcpServer;
        private string urls;
        private string userName;
        private string UUID;

        // Methods
        public FreePOPS()
        {
            this.urls = "";
            this.cookieTemp = "";
        }
        private int GetEmailnum(string mess)
        {
            return Convert.ToInt32(mess.Split(new char[] { ' ' })[1]);

        }
        public void getHotmail()
        {
            base.cookie = "";
            base.m_passwd = base.strPassParse(base.m_passwd);
            base.streamControl = true;
            base.ShowMessage("开始登陆…………");
            base.MyStringBuilder = this.Request("http://mail.live.com/");
            string message = base.MyStringBuilder.ToString();
            string str2 = "";
            string str3 = "";
            string url = "";
            string indata = "";
            int index = message.IndexOf("PPFT");
            if (index > 0)
            {
                str2 = base.putstr(message, "value=\"", "\"", index);
            }
            index = message.IndexOf("srf_sRBlob=");
            if (index > 0)
            {
                str3 = base.putstr(message, "srf_sRBlob='", "'", index);
            }
            index = message.IndexOf("srf_uPost");
            if (index > 0)
            {
                url = base.putstr(message, "srf_uPost='", "'", index);
                if (base.m_serv.IndexOf("msn") != -1)
                {
                    url = url.Insert(8, "msnia.");
                }
            }
            if (((str2 != "-1") || (str3 != "-1")) || (url != "-1"))
            {
                if (((str3 == "") && (str2 == "")) && (url == ""))
                {
                    base.ShowMessage("取信息失败！");
                    base.LoginFail();
                    base.passwdErr();
                    base.ShowMessage("登陆失败…………");
                }
                else
                {
                    indata = "idsbho=1&PwdPad=IfYouAreReadingThisYouHaveTooMuc&LoginOptions=3&CS=&FedState=&PPSX=" + str3 + "&type=11&login=" + base.m_username.Trim() + "&passwd=" + base.m_passwd + "&NewUser=1&PPFT=" + str2 + "&i1=0&i2=2";
                    base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                    base.streamControl = true;
                    base.MyStringBuilder = this.PostData(url, indata);
                    if (base.MyStringBuilder.ToString() != "")
                    {
                        message = base.MyStringBuilder.ToString();
                        index = message.IndexOf("window.location.replace");
                        if (index > 0)
                        {
                            string str6 = base.putstr(message, "(\"", "\")", index);
                            if (str6 != "-1")
                            {
                                base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                                base.streamControl = true;
                                base.MyStringBuilder = this.Request(str6);
                                if (base.MyStringBuilder.ToString().IndexOf("Object moved") >= 0)
                                {
                                    string str7 = base.putstr(base.MyStringBuilder.ToString(), "href=\"/mail/", "\"", 0);
                                    if (str7 != "-1")
                                    {
                                        str7 = str7.Replace("&amp;", "&");
                                        url = base.Host + str7;
                                        base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                                        base.streamControl = true;
                                        base.MyStringBuilder = this.Request(url);
                                        message = base.MyStringBuilder.ToString();
                                        index = message.IndexOf("name=\"main\"");
                                        if (index >= 0)
                                        {
                                            string str8 = base.putstr(message, "src=\"", "\"", index);
                                            if (str8 != "-1")
                                            {
                                                url = base.Host + str8;
                                                base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                                                base.streamControl = true;
                                                base.MyStringBuilder = this.Request(url);
                                                if (base.MyStringBuilder.ToString().IndexOf("class=\"ManageLink\"") < 0)
                                                {
                                                    url = base.Host + "SwitchClassicVersion.aspx";
                                                    base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                                                    base.streamControl = true;
                                                    base.MyStringBuilder = this.Request(url);
                                                    if (base.MyStringBuilder.ToString().IndexOf("Object moved") >= 0)
                                                    {
                                                        str7 = base.putstr(base.MyStringBuilder.ToString(), "href=\"/mail/", "\"", 0).Replace("&amp;", "&");
                                                        url = base.Host + str7;
                                                        base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                                                        base.streamControl = true;
                                                        base.MyStringBuilder = this.Request(url);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                if (base.MyStringBuilder.ToString().IndexOf("Object moved") >= 0)
                                {
                                    string str9 = base.putstr(base.MyStringBuilder.ToString(), "href=\"", "\"", 0);
                                    if (str9 != "-1")
                                    {
                                        url = str9.Replace("&amp;", "&");
                                        base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                                        base.streamControl = true;
                                        base.MyStringBuilder = this.Request(url);
                                    }
                                }
                                else if (base.MyStringBuilder.ToString().IndexOf("self.location.href") >= 0)
                                {
                                    string str10 = base.putstr(base.MyStringBuilder.ToString(), "self.location.href = '", "'", 0);
                                    if (str10 != "-1")
                                    {
                                        url = HttpUtility.UrlDecode(str10.Replace(@"\x", "%"));
                                        string str11 = base.putstr(url, "http:", "/mail/", 0);
                                        if ((str11 != "-1") && (base.Host == null))
                                        {
                                            base.Host = "http:" + str11 + "/mail/";
                                        }
                                        base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                                        base.streamControl = true;
                                        base.MyStringBuilder = this.Request(url);
                                    }
                                    if (base.MyStringBuilder.ToString().IndexOf("Object moved") >= 0)
                                    {
                                        str10 = base.putstr(base.MyStringBuilder.ToString(), "href=\"", "\"", 0);
                                        if (str10 != "-1")
                                        {
                                            str10 = str10.Substring(str10.IndexOf("/mail/") + 6, str10.Length - (str10.IndexOf("/mail/") + 6));
                                            url = base.Host + str10;
                                            base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                                            base.streamControl = true;
                                            base.MyStringBuilder = this.Request(url);
                                        }
                                    }
                                }
                                message = base.MyStringBuilder.ToString();
                                int startIndex = message.IndexOf("class=\"ManageLink\"");
                                if (startIndex < 0)
                                {
                                    startIndex = message.IndexOf("href=\"ManageFoldersLight");
                                }
                                if (startIndex < 0)
                                {
                                    string str12 = "";
                                    string str13 = "";
                                    string str14 = "";
                                    index = message.IndexOf("__VIEWSTATE");
                                    if (index > 0)
                                    {
                                        str12 = base.putstr(message, "value=\"", "\"", index);
                                    }
                                    index = message.IndexOf("TakeMeToInbox");
                                    if (index > 0)
                                    {
                                        str13 = base.putstr(message, "value=\"", "\"", index);
                                    }
                                    index = message.IndexOf("__EVENTVALIDATION");
                                    if (index > 0)
                                    {
                                        str14 = base.putstr(message, "value=\"", "\"", index);
                                    }
                                    if ((((str12 != "-1") && (str13 != "-1")) && ((str14 != "-1") && (str12 != ""))) && (((str13 != "") && (str14 != "")) && ((this.urls != null) && (this.urls != ""))))
                                    {
                                        indata = ("__VIEWSTATE=" + str12 + "&TakeMeToInbox=%E7%BB%A7%E7%BB%AD&__EVENTVALIDATION=" + str14).Replace("/", "%2F").Replace("+", "%2B");
                                        base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                                        base.streamControl = true;
                                        base.MyStringBuilder = this.PostData(this.urls, indata);
                                        if (base.MyStringBuilder.ToString().IndexOf("Object moved") >= 0)
                                        {
                                            string str15 = base.putstr(base.MyStringBuilder.ToString(), "href=\"", "\"", 0);
                                            if (str15 != "-1")
                                            {
                                                url = (base.Host + str15).Replace("mail//mail", "mail");
                                                base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                                                base.streamControl = true;
                                                base.MyStringBuilder = this.Request(url);
                                            }
                                        }
                                        message = base.MyStringBuilder.ToString();
                                        startIndex = message.IndexOf("class=\"ManageLink\"");
                                        if (startIndex < 0)
                                        {
                                            startIndex = message.IndexOf("href=\"ManageFoldersLight");
                                        }
                                    }
                                }
                                if (startIndex > 0)
                                {
                                    string str16 = base.putstr(message, "href=\"", "\"", startIndex);
                                    if (str16 != "-1")
                                    {
                                        base.ShowMessage("登陆成功！");
                                        this.cookieTemp = base.cookie;
                                        url = base.Host + str16;
                                        base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                                        base.streamControl = true;
                                        base.MyStringBuilder = this.Request(url);
                                        message = base.MyStringBuilder.ToString();
                                        this.getHotmailBoxName(message);
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
                                base.ShowMessage("登陆失败…………");
                            }
                        }
                        else
                        {
                            if ((message.IndexOf("The e-mail address or password is incorrect. Please try again.") != -1) || (message.IndexOf("电子邮件地址或密码不正确") != -1))
                            {
                                base.ShowMessage(base.m_username + ":电子邮件地址或密码不正确");
                            }
                            base.LoginFail();
                            base.passwdErr();
                            base.ShowMessage("登陆失败…………");
                        }
                    }
                }
            }
            else
            {
                base.ShowMessage("取信息失败！");
                base.LoginFail();
                base.passwdErr();
                base.ShowMessage("登陆失败…………");
            }

        }
        private void getHotmailBoxName(string message)
        {
            int startIndex = 0;
            string s = "";
            string[] strArray = new string[100];
            startIndex = message.IndexOf("<tbody>");
            if (startIndex > 0)
            {
                int index = 0;
                while ((startIndex = message.IndexOf("?FolderID", startIndex)) > 0)
                {
                    s = base.putstr(message, ">", "</a>", startIndex);
                    if (s != "-1")
                    {
                        strArray[index] = HttpUtility.HtmlDecode(s);
                    }
                    else
                    {
                        base.ShowMessage("取箱子失败！");
                    }
                    index++;
                    startIndex++;
                }
            }
            else
            {
                base.ShowMessage("取箱子失败！");
                return;
            }
            for (int i = 0; i < strArray.Length; i++)
            {
                if (strArray[i] != null)
                {
                    base.BoxName = strArray[i];
                    this.userName = base.m_username + "?folder=" + base.BoxName + "&keepmsgstatus=1";
                    this.loginFreepops();
                }
            }
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
        private void getYahoo()
        {
            this.China = true;
            this.cookieContainer = new CookieContainer();
            base.ShowMessage("开始登陆…………");
            string url = "https://login.yahoo.com/config/login?";
            base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
            base.streamControl = true;
            base.MyStringBuilder = this.Request("http://mail.yahoo.com/");
            string message = base.MyStringBuilder.ToString();
            if ((base.MyStringBuilder == null) || (base.MyStringBuilder.ToString() == ""))
            {
                base.LoginFail();
                base.passwdErr();
                base.ShowMessage("登陆失败…………");
            }
            else
            {
                int startIndex = 0;
                string str3 = "";
                string str4 = "";
                string str5 = "";
                string str6 = "";
                string str7 = "";
                string str8 = "";
                string str9 = "";
                string str10 = "";
                startIndex = message.IndexOf("post");
                if (startIndex > 0)
                {
                    startIndex = message.IndexOf("\".tries\"", startIndex);
                    if (startIndex > 0)
                    {
                        str3 = base.putstr(message, "value=\"", "\">", startIndex);
                        startIndex = message.IndexOf("\".src\"", startIndex);
                        if (startIndex > 0)
                        {
                            str4 = base.putstr(message, "value=\"", "\">", startIndex);
                            startIndex = message.IndexOf("\".intl\"", startIndex);
                            if (startIndex > 0)
                            {
                                str5 = base.putstr(message, "value=\"", "\">", startIndex);
                                startIndex = message.IndexOf("\".u\"", startIndex);
                                if (startIndex > 0)
                                {
                                    str6 = base.putstr(message, "value=\"", "\">", startIndex);
                                    startIndex = message.IndexOf("\".v\"", startIndex);
                                    if (startIndex > 0)
                                    {
                                        str7 = base.putstr(message, "value=\"", "\">", startIndex);
                                    }
                                    startIndex = message.IndexOf("\".challenge\"", startIndex);
                                    if (startIndex > 0)
                                    {
                                        str8 = base.putstr(message, "value=\"", "\">", startIndex);
                                    }
                                    startIndex = message.IndexOf("\"hasMsgr\"", startIndex);
                                    if (startIndex > 0)
                                    {
                                        str10 = base.putstr(message, "value=\"", "\">", startIndex);
                                    }
                                    startIndex = message.IndexOf("\".chkP\"", startIndex);
                                    if (startIndex > 0)
                                    {
                                        str9 = base.putstr(message, "value=\"", "\">", startIndex);
                                    }
                                    if ((((str3 == "-1") || (str4 == "-1")) || ((str5 == "-1") || (str6 == "-1"))) || (((str7 == "-1") || (str8 == "-1")) || ((str9 == "-1") || (str10 == "-1"))))
                                    {
                                        base.ShowMessage("取消息失败！");
                                    }
                                }
                            }
                        }
                    }
                }
                if (base.m_serv.Trim().ToUpper().IndexOf("YMAIL.COM") != -1)
                {
                    base.m_username = base.m_username + "@ymail.com";
                }
                else if (base.m_serv.Trim().ToUpper().IndexOf("YAHOO.CN") != -1)
                {
                    base.m_username = base.m_username + "@yahoo.cn";
                }
                else if (base.m_serv.Trim().ToUpper().IndexOf("ROCKETMAIL.COM") != -1)
                {
                    base.m_username = base.m_username + "@rocketmail.com";
                }
                string indata = ".tries=" + str3 + "&.src=" + str4 + "&.md5=&.hash=&.js=&.last=&promo=&.intl=" + str5 + "&.bypass=&.partner=&.u=" + str6 + "&.v=" + str7 + "&.challenge=" + str8 + "&.yplus=&.emailCode=&pkg=&stepid=&.ev=&hasMsgr=" + str10 + "&.chkP=" + str9 + "&.done=http%3A%2F%2Fmail.yahoo.com&.pd=ym_ver%3D0%26c%3D&login=" + base.m_username + "&passwd=" + base.m_passwd + "&.save=Sign+In";
                base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                base.streamControl = true;
                base.MyStringBuilder = this.PostYahooData(url, indata, this.cookieContainer);
                if (base.MyStringBuilder != null)
                {
                    url = base.putstr(base.MyStringBuilder.ToString(), "window.location.replace(\"", "\")", 0);
                    if (url == "-1")
                    {
                        base.LoginFail();
                        base.passwdErr();
                        base.ShowMessage("登陆失败…………");
                    }
                    else
                    {
                        if (url.IndexOf("http") >= 0)
                        {
                            if (url.IndexOf("/ym/login?") <= 0)
                            {
                                if (url.IndexOf("/mc/welcome?") > 0)
                                {
                                    base.Host = url.Substring(0, url.IndexOf("welcome?"));
                                    if (base.Host == "")
                                    {
                                        base.ShowMessage("取主机失败！");
                                        return;
                                    }
                                }
                            }
                            else
                            {
                                base.Host = url.Substring(0, url.IndexOf("/ym/login?"));
                                if (base.Host == "")
                                {
                                    base.ShowMessage("取主机失败！");
                                    return;
                                }
                            }
                        }
                        base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                        base.streamControl = true;
                        base.MyStringBuilder = this.Request(url);
                        string str12 = base.MyStringBuilder.ToString();
                        if (url.IndexOf("/dc/") > 0)
                        {
                            base.Host = url.Substring(0, url.IndexOf("/dc/"));
                            int num2 = str12.IndexOf("<a href='/ym/login?");
                            string str13 = base.putstr(str12, "href='", "'", num2);
                            if (str13 != "-1")
                            {
                                base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                                base.streamControl = true;
                                base.MyStringBuilder = this.Request(base.Host + str13);
                                if (this.urls.IndexOf("http") >= 0)
                                {
                                    if (this.urls.IndexOf("/ym/login?") > 0)
                                    {
                                        base.Host = this.urls.Substring(0, this.urls.IndexOf("/ym/login?"));
                                        if (base.Host == "")
                                        {
                                            base.ShowMessage("取主机失败！");
                                            return;
                                        }
                                    }
                                    else if (this.urls.IndexOf("/mc/welcome?") > 0)
                                    {
                                        base.Host = this.urls.Substring(0, this.urls.IndexOf("welcome?"));
                                        if (base.Host == "")
                                        {
                                            base.ShowMessage("取主机失败！");
                                            return;
                                        }
                                    }
                                }
                                base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                                base.streamControl = true;
                                base.MyStringBuilder = this.Request(this.urls + "&noFlush");
                            }
                        }
                        if (this.urls.IndexOf("se.mc") != -1)
                        {
                            if (this.urls.IndexOf("/mc/welcome?") > 0)
                            {
                                base.Host = this.urls.Substring(0, this.urls.IndexOf("welcome?"));
                                if (base.Host == "")
                                {
                                    base.ShowMessage("取主机失败！");
                                    return;
                                }
                            }
                            base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                            base.streamControl = true;
                            base.MyStringBuilder = this.Request(this.urls + "&noFlush");
                        }
                        string str14 = "";
                        string cookie = "";
                        if (base.MyStringBuilder != null)
                        {
                            str14 = base.MyStringBuilder.ToString();
                            cookie = base.cookie;
                        }
                        this.getYahooAddressbook(str14);
                        int index = str14.IndexOf("<a href=\"/ym/Folders?YY=");
                        if (base.Host != null)
                        {
                            if (base.Host.IndexOf("mc") > 0)
                            {
                                index = str14.IndexOf("<a href=\"folders?");
                            }
                            else if (base.Host.IndexOf("se") != -1)
                            {
                                index = str14.IndexOf("<a href=\"folders?");
                            }
                        }
                        if (index > 0)
                        {
                            string str16 = base.putstr(str14, "<a href=\"", "\"", index);
                            if (str16 != "-1")
                            {
                                base.ShowMessage("登陆成功！");
                                base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                                base.streamControl = true;
                                base.cookie = cookie;
                                base.MyStringBuilder = this.Request(base.Host + str16 + "&noFlush");
                                this.getYahooBoxName(base.MyStringBuilder.ToString());
                            }
                        }
                        else
                        {
                            base.LoginFail();
                            base.passwdErr();
                            base.ShowMessage("登陆失败…………");
                        }
                    }
                }
            }

        }
        private void getYahooAddressbook(string message)
        {
            int startIndex = 0;
            startIndex = message.IndexOf("addressbooktab");
            if (startIndex < 0)
            {
                startIndex = message.IndexOf("addresses");
            }
            if (startIndex > 0)
            {
                string url = base.putstr(message, "href=\"", "\"", startIndex);
                if (url != "-1")
                {
                    base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                    base.streamControl = true;
                    base.MyStringBuilder = this.Request(url);
                    startIndex = base.MyStringBuilder.ToString().IndexOf("id=\"misc\"");
                    url = base.putstr(base.MyStringBuilder.ToString(), "href=\"", "\"", startIndex);
                    if (url != "-1")
                    {
                        url = url.Replace("&amp;", "&").Replace("VPC=sync_info", "VPC=import_export");
                        url = this.urls + "/" + url;
                        base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                        base.streamControl = true;
                        base.MyStringBuilder = this.Request(url);
                        string str2 = base.MyStringBuilder.ToString();
                        string str3 = "";
                        string str4 = "";
                        string str5 = "";
                        int index = 0;
                        index = str2.IndexOf("\".crumb\"");
                        if (index > 0)
                        {
                            str3 = base.putstr(str2, "value=\"", "\"", index);
                            index = str2.IndexOf("\"VPC\"");
                            if (index > 0)
                            {
                                str4 = base.putstr(str2, "value=\"", "\"", index);
                            }
                            index = str2.IndexOf("submit[action_export_yahoo]");
                            if (index > 0)
                            {
                                str5 = base.putstr(str2, "value=\"", "\"", index);
                            }
                        }
                        if (((str3 != "-1") && (str4 != "-1")) && (str5 != "-1"))
                        {
                            string str6 = this.urls + "/index.php";
                            string indata = ".crumb=" + str3 + "&VPC=" + str4 + "&submit%5Baction_export_yahoo%5D=" + str5;
                            base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                            base.streamControl = true;
                            base.MyStringBuilder = this.PostYahooData(str6, indata, this.cookieContainer);
                            this.SaveYahooAddressbook(base.MyStringBuilder.ToString());
                        }
                    }
                }
            }

        }
        private void getYahooBoxName(string message)
        {
            string str;
            int startIndex = 0;
            if (base.Host.IndexOf("/mc/") > 0)
            {
                startIndex = 0;
                if (message.IndexOf("href=\"showFolder?fid=", startIndex) > 0)
                {
                    while ((startIndex = message.IndexOf("href=\"showFolder?fid=", startIndex)) > 0)
                    {
                        str = base.putstr(message, "href=\"", "\"", startIndex);
                        base.BoxName = base.putstr(message, "<h3>", "</h3>", startIndex).Trim();
                        if ((str == "-1") || (base.BoxName == "-1"))
                        {
                            startIndex++;
                            base.ShowMessage("取箱子失败…………");
                        }
                        else
                        {
                            this.userName = base.m_username + "?folder=" + base.BoxName + "&keepmsgstatus=1";
                            this.loginFreepops();
                            startIndex++;
                        }
                    }
                }
            }
            else
            {
                startIndex = 0;
                if (message.IndexOf("<a href=\"/ym/ShowFolder?box=", startIndex) > 0)
                {
                    while ((startIndex = message.IndexOf("<a href=\"/ym/ShowFolder?box=", startIndex)) > 0)
                    {
                        str = base.putstr(message, "href=\"", "\"", startIndex);
                        base.BoxName = base.putstr(message, "<b>", "</b>", startIndex);
                        if ((str == "-1") || (base.BoxName == "-1"))
                        {
                            startIndex++;
                            base.ShowMessage("取箱子失败…………");
                        }
                        else
                        {
                            this.userName = base.m_username + "?folder=" + base.BoxName + "&keepmsgstatus=1";
                            this.loginFreepops();
                            startIndex++;
                        }
                    }
                }
                else
                {
                    base.ShowMessage("取箱子失败…………");
                }
            }

        }
        public override void login()
        {
            if (base.m_username.IndexOf("hotmail") != -1)
            {
                if (base.m_username.IndexOf("?") != -1)
                {
                    this.userName = base.m_username;
                    if (base.m_username.IndexOf("&") != -1)
                    {
                        base.BoxName = base.putstr(base.m_username, "folder=", "&", 0);
                        if (base.BoxName != "-1")
                        {
                            this.loginFreepops();
                        }
                        else
                        {
                            base.ShowMessage("用户名格式错误。。。。。。");
                        }
                    }
                    else if (base.m_username.IndexOf("folder=") != -1)
                    {
                        base.BoxName = base.m_username.Substring(base.m_username.IndexOf("folder=") + 7, base.m_username.Length - (base.m_username.IndexOf("folder=") + 7));
                        this.loginFreepops();
                    }
                    else
                    {
                        base.ShowMessage("用户名格式错误。。。。。。");
                    }
                }
                else
                {
                    this.getHotmail();
                }
            }
            else if (base.m_username.IndexOf("yahoo") != -1)
            {
                if (base.m_username.IndexOf("?") != -1)
                {
                    this.userName = base.m_username;
                    if (base.m_username.IndexOf("&") != -1)
                    {
                        base.BoxName = base.putstr(base.m_username, "folder=", "&", 0);
                        if (base.BoxName != "-1")
                        {
                            this.loginFreepops();
                        }
                        else
                        {
                            base.ShowMessage("用户名格式错误。。。。。。");
                        }
                    }
                    else if (base.m_username.IndexOf("folder=") != -1)
                    {
                        base.BoxName = base.m_username.Substring(base.m_username.IndexOf("folder=") + 7, base.m_username.Length - (base.m_username.IndexOf("folder=") + 7));
                        this.loginFreepops();
                    }
                    else
                    {
                        base.ShowMessage("用户名格式错误。。。。。。");
                    }
                }
                else
                {
                    this.getYahoo();
                }
            }
            else
            {
                base.m_username.IndexOf("gmail");
            }

        }
        public void loginFreepops()
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
            if (this.response.IndexOf("+OK") != -1)
            {
                try
                {
                    this.request = "USER " + this.userName + "\r\n";
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
                            if (Convert.ToInt32(GlobalValue.PopMainForm.ExecuteSQL(this.SQL)) > 0)
                            {
                                continue;
                            }
                            this.request = "RETR " + i + "\r\n";
                            this.SendRequest(this.request);
                            this.response = this.streamReader.ReadLine();
                            if (this.response.StartsWith("+OK"))
                            {
                                base.ShowMessage(base.BoxName + "第" + i.ToString("00") + "封:" + this.UUID + " 开始下载");
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
                                        base.ShowMessage(base.BoxName + "第" + i.ToString("00") + "封:" + this.UUID + "下载失败");
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
                            base.ShowMessage(base.BoxName + "第" + i.ToString("00") + "封:" + this.UUID + " 下载完毕");
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
                    try
                    {
                        if (!this.download && (base.m_serv.ToLower().Trim().IndexOf("pop.sohu.com") < 0))
                        {
                            this.request = "QUIT\r\n";
                            this.SendRequest(this.request);
                            this.response = this.streamReader.ReadLine();
                            this.response.StartsWith("+OK");
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
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
            }
            catch (Exception exception)
            {
                base.ShowMessage("Hpost:" + exception.Message);
            }
            return base.MyStringBuilder;

        }
        public StringBuilder PostYahooData(string url, string indata, CookieContainer cookieContainer)
        {
            try
            {
                StreamReader reader;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.ServicePoint.Expect100Continue = false;
                request.Method = "POST";
                request.Accept = "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, application/x-silverlight, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, */*";
                request.Referer = "https://login.yahoo.co.jp/config/login_verify2?.src=ym";
                request.Headers.Add("Accept-Language: zh-cn");
                request.ContentType = "application/x-www-form-urlencoded";
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; EmbeddedWB 14.52 from: http://www.bsalsa.com/ EmbeddedWB 14.52; .NET CLR 2.0.50727)";
                request.ContentLength = indata.Length;
                request.AllowAutoRedirect = false;
                request.KeepAlive = true;
                request.Headers.Add("Cookie", base.cookie);
                Stream requestStream = request.GetRequestStream();
                char[] chars = Encoding.ASCII.GetChars(Encoding.ASCII.GetBytes(indata));
                StreamWriter writer = new StreamWriter(requestStream);
                writer.Write(chars, 0, chars.Length);
                writer.Close();
                requestStream.Close();
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Thread.Sleep(10);
                if (response.Headers["Set-Cookie"] != null)
                {
                    base.cookie = base.cook(response.Headers["Set-Cookie"]);
                }
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
            catch (Exception exception)
            {
                base.ShowMessage(exception.Message);
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
                request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                request.AllowAutoRedirect = false;
                request.ServicePoint.Expect100Continue = false;
                request.KeepAlive = true;
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1)";
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
            }
            catch (Exception exception)
            {
                base.ShowMessage("Hrequest:" + exception.Message);
            }
            return base.MyStringBuilder;

        }
        public void SaveYahooAddressbook(string EmailText)
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
            path = path + @"\" + base.m_username;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            path = path + @"\地址薄";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            Stream stream = File.Create(path + @"\" + base.m_username + "-yahoo_ab.csv");
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
    }
}
