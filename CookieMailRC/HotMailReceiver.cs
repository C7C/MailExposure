using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Threading;
using System.IO;
using System.Net;

namespace MailExposure.CookieMailRC
{
    class HotMailReceiver:MailStr
    {
        // Fields
        private string cookieTemp;
        public string m_emailTime;
        private string urls;

        // Methods
        public HotMailReceiver()
        {
            this.urls = "";
            this.cookieTemp = "";
        }
        private void downMail(string message, string unreadUrl, string unreadIndata)
        {
            int startIndex = 0;
            string str = "";
            string url = "";
            string str3 = "";
            bool flag = false;
            string str4 = unreadIndata;
            bool flag2 = false;
            startIndex = message.IndexOf("MessageListItems", startIndex);
            if (startIndex > 0)
            {
                while ((startIndex = message.IndexOf("<tr ", startIndex)) > 0)
                {
                    unreadIndata = str4;
                    str = base.putstr(message, "id=\\\"", "\\\"", startIndex);
                    str3 = base.putstr(message, "class=\\\"", "\\\"", startIndex);
                    if ((str3 != "-1") && (str3.IndexOf("Unread") != -1))
                    {
                        flag = true;
                    }
                    if (!(str != "-1") || !(str != ""))
                    {
                        goto Label_049C;
                    }
                    base.charSet = "GB2312";
                    string strSql = "select count(*) from HotmailId where MsgId='" + str + "'";
                    if (Convert.ToInt32(GlobalValue.mainForm.ExecuteSQL(strSql)) != 0)
                    {
                        goto Label_04AD;
                    }
                    string str5 = HttpUtility.HtmlDecode(base.putstr(message, "<td class=\\\"Frm\\\"><a>", "</a>", startIndex));
                    string str6 = HttpUtility.HtmlDecode(base.putstr(message, "<td class=\\\"Dat\\\">", "</td>", startIndex));
                    string str7 = HttpUtility.HtmlDecode(base.putstr(message, "<a href=\\\"#\\\">", "</a>", startIndex));
                    bool flag3 = false;
                    try
                    {
                        DateTime time = new DateTime();
                        time = Convert.ToDateTime(str6);
                        if (DateTime.Compare(time, GlobalValue.mainForm.EmailDateTime) >= 0)
                        {
                            flag3 = true;
                        }
                        else
                        {
                            flag3 = false;
                        }
                    }
                    catch (Exception)
                    {
                        flag3 = true;
                    }
                    if (!flag3)
                    {
                        goto Label_04AD;
                    }
                    if (base.m_serv.IndexOf("hotmail.co.jp") != -1)
                    {
                        url = base.Host + "GetMessageSource.aspx?msgid=" + str;
                    }
                    else
                    {
                        url = base.Host + "GetMessageSource.aspx?msgid=" + str + "&gs=true";
                    }
                    base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                    base.streamControl = true;
                    base.cookie = this.cookieTemp;
                    base.MyStringBuilder = this.Request(url);
                    if (base.MyStringBuilder.ToString() != "")
                    {
                        string emailText = base.MyStringBuilder.ToString();
                        char[] trimChars = new char[] { '<', 'p', 'r', 'e', '>' };
                        char[] chArray2 = new char[] { '<', '/', 'p', 'r', 'e', '>', '\r', '\n' };
                        char[] chArray3 = new char[] { '<', '/', 'p', 'r', 'e', '\r', '\n' };
                        char[] chArray4 = new char[] { '<', '/', 'p', 'r', '\r', '\n' };
                        char[] chArray5 = new char[] { '<', '/', 'p', '\r', '\n' };
                        char[] chArray6 = new char[] { '<', '/', '\r', '\n' };
                        char[] chArray7 = new char[] { '<', '\r', '\n' };
                        emailText = HttpUtility.HtmlDecode(emailText.TrimStart(trimChars).Trim(chArray2).TrimEnd(chArray3).TrimEnd(chArray4).TrimEnd(chArray5).TrimEnd(chArray6).TrimEnd(chArray7));
                        this.m_emailTime = DateTime.Now.ToString("yyyMMdd-HHmmss") + DateTime.Now.Millisecond.ToString();
                        this.SaveHotmailEmail(emailText);
                        base.cookie = this.cookieTemp;
                    }
                    else
                    {
                        flag2 = true;
                        base.ShowMessage("邮件：" + str7 + "下载失败");
                    }
                    if (flag)
                    {
                        try
                        {
                            unreadIndata = unreadIndata.Insert(unreadIndata.IndexOf("%22") + 3, str);
                            base.streamControl = true;
                            base.cookie = this.cookieTemp;
                            this.PostData(unreadUrl, unreadIndata);
                            base.ShowMessage("置未读：" + str7 + "\t");
                            flag = false;
                            goto Label_03DA;
                        }
                        catch (Exception exception)
                        {
                            base.ShowMessage("置未读失败：" + str7 + "\t" + exception.Message);
                            startIndex++;
                            continue;
                        }
                    }
                    base.ShowMessage("已读：" + str7 + "\t");
                Label_03DA:
                    startIndex++;
                    if (!flag2)
                    {
                        try
                        {
                            string str10 = DateTime.Now.ToString();
                            strSql = "insert into HotmailId(Name,MsgId,DownTime,MailType) values('" + base.m_username + "','" + str + "','" + str10 + "','" + base.BoxName.Substring(0, base.BoxName.Length % 10) + "')";
                            GlobalValue.mainForm.ExecuteSQL(strSql);
                            goto Label_04AD;
                        }
                        catch (Exception exception2)
                        {
                            base.ShowMessage("添加失败：" + exception2.Message);
                            goto Label_04AD;
                        }
                    }
                    flag2 = false;
                    goto Label_04AD;
                Label_049C:
                    startIndex++;
                    base.ShowMessage("取邮件ID失败！");
                    continue;
                Label_04AD:
                    startIndex++;
                }
            }
        }
        private void getBoxName(string message)
        {
            int startIndex = 0;
            string str = "";
            string boxID = "";
            string[] strArray = new string[] { "收件箱", "垃圾邮件", "草稿", "已发送邮件", "删除的邮件" };
            startIndex = message.IndexOf("<tbody>");
            if (startIndex > 0)
            {
                int index = 0;
                while ((startIndex = message.IndexOf("?FolderID", startIndex)) > 0)
                {
                    str = base.putstr(message, "FolderID", "\"", startIndex);
                    if (str != "-1")
                    {
                        boxID = str.Substring(1, str.IndexOf("&") - 1);
                        if (index > 4)
                        {
                            base.BoxName = "自建箱" + (index - 4);
                        }
                        else
                        {
                            base.BoxName = strArray[index];
                        }
                        string url = base.Host + "InboxLight.aspx?FolderID" + str;
                        base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                        base.streamControl = true;
                        base.MyStringBuilder = this.Request(url);
                        this.getPages(base.MyStringBuilder.ToString(), boxID);
                    }
                    else
                    {
                        base.ShowMessage("取箱子失败!");
                    }
                    index++;
                    startIndex++;
                }
            }
            else
            {
                base.ShowMessage("取箱子失败!");
            }

        }
        private void getNextPages(string message, string boxID, string msgsPerPage, string sAsc, string sBy, string mCt, string mt, string pagesUrl, string unreadUrl, int pnCur)
        {
            int startIndex = 0;
            startIndex = message.IndexOf("pnDir=\\\"NextPage\\\"");
            if (startIndex > 0)
            {
                int num2 = pnCur + 1;
                startIndex = message.IndexOf("pnCur=\\\"" + num2 + "\\\"", startIndex);
                if (startIndex > 0)
                {
                    string str = base.putstr(message, "pnCur=\\\"", "\\\"", startIndex);
                    if (str != "-1")
                    {
                        int num3 = Convert.ToInt32(str);
                        if (num3 > pnCur)
                        {
                            pnCur = num3;
                            string str2 = base.putstr(message, "pnAm=\\\"", "\\\"", startIndex);
                            string str3 = base.putstr(message, "pnAd=\\\"", "\\\"", startIndex).Replace("&#58;", @"\: ");
                            string str4 = base.putstr(message, "pnDir=\\\"", "\\\"", startIndex);
                            string str5 = base.putstr(message, "pnMid=\\\"", "\\\"", startIndex);
                            string indata = "cn=Microsoft.Msn.Hotmail.Ui.Fpp.MailBox&mn=GetInboxData&d=true,false,true,{%22" + boxID + "%22" + msgsPerPage.Trim() + "," + str4 + "," + sAsc.Trim() + "," + sBy.Trim() + ",false,%22" + str2 + "%22,%22" + str3 + "%22," + str.Trim() + "," + str5.Trim() + ",false,%22%22," + mCt + ",-1,Off},false,null&v=1&mt=" + mt;
                            string unreadIndata = "cn=Microsoft.Msn.Hotmail.Ui.Fpp.MailBox&mn=MarkMessagesReadState&d=false,[%22%22],{%22" + boxID + "%22" + msgsPerPage.Trim() + "," + str4 + "," + sAsc.Trim() + "," + sBy.Trim() + ",false,%2200000000-0000-0000-0000-000000000000%22,%22%22," + str.Trim() + "," + str5.Trim() + ",false,%22%22," + mCt + ",-1,Off}&v=1&mt=" + mt;
                            base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                            base.streamControl = true;
                            base.MyStringBuilder = this.PostData(pagesUrl, indata);
                            string str8 = base.MyStringBuilder.ToString();
                            if (str8 != "")
                            {
                                base.ShowMessage(string.Concat(new object[] { base.BoxName, "：第", pnCur, "页开始下载" }));
                                this.downMail(str8, unreadUrl, unreadIndata);
                                this.getNextPages(str8, boxID, msgsPerPage, sAsc, sBy, mCt, mt, pagesUrl, unreadUrl, pnCur);
                            }
                        }
                    }
                }
                else
                {
                    startIndex = message.IndexOf("\\\"nextPageLink\\\"");
                    if (startIndex > 0)
                    {
                        string str9 = base.putstr(message, "pnCur=\\\"", "\\\"", startIndex);
                        if (str9 != "-1")
                        {
                            int num4 = Convert.ToInt32(str9);
                            if (num4 > pnCur)
                            {
                                pnCur = num4;
                                string str10 = base.putstr(message, "pnAm=\\\"", "\\\"", startIndex);
                                string str11 = base.putstr(message, "pnAd=\\\"", "\\\"", startIndex).Replace("&#58;", @"\: ");
                                string str12 = base.putstr(message, "pnDir=\\\"", "\\\"", startIndex);
                                string str13 = base.putstr(message, "pnMid=\\\"", "\\\"", startIndex);
                                string str14 = "cn=Microsoft.Msn.Hotmail.Ui.Fpp.MailBox&mn=GetInboxData&d=true,false,true,{%22" + boxID + "%22" + msgsPerPage.Trim() + "," + str12 + "," + sAsc.Trim() + "," + sBy.Trim() + ",false,%22" + str10 + "%22,%22" + str11 + "%22," + str9.Trim() + "," + str13.Trim() + ",false,%22%22," + mCt + ",-1,Off},false,null&v=1&mt=" + mt;
                                string str15 = "cn=Microsoft.Msn.Hotmail.Ui.Fpp.MailBox&mn=MarkMessagesReadState&d=false,[%22%22],{%22" + boxID + "%22" + msgsPerPage.Trim() + "," + str12 + "," + sAsc.Trim() + "," + sBy.Trim() + ",false,%2200000000-0000-0000-0000-000000000000%22,%22%22," + str9.Trim() + "," + str13.Trim() + ",false,%22%22," + mCt + ",-1,Off}&v=1&mt=" + mt;
                                base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                                base.streamControl = true;
                                base.MyStringBuilder = this.PostData(pagesUrl, str14);
                                string str16 = base.MyStringBuilder.ToString();
                                if (str16 != "")
                                {
                                    if (Convert.ToInt32(GlobalValue.mainForm.PageNumber) == 0)
                                    {
                                        base.ShowMessage(string.Concat(new object[] { base.BoxName, "：第", pnCur, "页开始下载" }));
                                        this.downMail(str16, unreadUrl, str15);
                                        this.getNextPages(str16, boxID, msgsPerPage, sAsc, sBy, mCt, mt, pagesUrl, unreadUrl, pnCur);
                                    }
                                    else if (pnCur <= Convert.ToInt32(GlobalValue.mainForm.PageNumber))
                                    {
                                        base.ShowMessage(string.Concat(new object[] { base.BoxName, "：第", pnCur, "页开始下载" }));
                                        this.downMail(str16, unreadUrl, str15);
                                        this.getNextPages(str16, boxID, msgsPerPage, sAsc, sBy, mCt, mt, pagesUrl, unreadUrl, pnCur);
                                    }
                                }
                            }
                        }
                    }
                }
            }

        }
        private void getPages(string message, string boxID)
        {
            int startIndex = 0;
            string url = "";
            string str2 = "";
            string str3 = "";
            string str4 = "";
            string unreadUrl = "";
            startIndex = message.IndexOf("SessionId:");
            if (startIndex > 0)
            {
                str2 = base.putstr(message, "\"", "\"", startIndex);
            }
            startIndex = message.IndexOf("AuthUser:");
            if (startIndex > 0)
            {
                str3 = base.putstr(message, "\"", "\"", startIndex);
            }
            startIndex = message.IndexOf("PartnerID:");
            if (startIndex > 0)
            {
                str4 = base.putstr(message, "\"", "\"", startIndex);
            }
            if (((str2 == "-1") || (str3 == "-1")) || (str4 == "-1"))
            {
                base.ShowMessage("取页数失败!");
            }
            else
            {
                if (str4 == "")
                {
                    str4 = "0";
                }
                url = base.Host + "mail.fpp?cnmn=Microsoft.Msn.Hotmail.Ui.Fpp.MailBox.GetInboxData&a=" + str2 + "&au=" + str3 + "&ptid=" + str4;
                unreadUrl = base.Host + "mail.fpp?cnmn=Microsoft.Msn.Hotmail.Ui.Fpp.MailBox.MarkMessagesReadState&a=" + str2 + "&au=" + str3 + "&ptid=" + str4;
                string msgsPerPage = "";
                startIndex = message.IndexOf("msgsPerPage :");
                if (startIndex > 0)
                {
                    msgsPerPage = base.putstr(message, "msgsPerPage :", ",", startIndex);
                }
                startIndex = message.IndexOf("mainContentContainer");
                string sBy = "Date";
                string sAsc = "0";
                string mCt = "";
                if (startIndex > 0)
                {
                    sBy = base.putstr(message, "sBy=\"", "\"", startIndex);
                    sAsc = base.putstr(message, "sAsc=\"", "\"", startIndex);
                    mCt = base.putstr(message, "mCt=\"", "\"", startIndex);
                }
                string mt = base.putstr(base.cookie, "mt=", ";", 0);
                if (((sBy == "-1") || (sAsc == "-1")) || (mt == "-1"))
                {
                    base.ShowMessage("取页信息失败!");
                }
                else
                {
                    int num2 = 0;
                    int pnCur = 0;
                    while ((num2 = message.IndexOf("pnCur", num2)) > 0)
                    {
                        string str11 = base.putstr(message, "pnCur=\"", "\"", num2);
                        if (str11 != "-1")
                        {
                            int num4 = Convert.ToInt32(str11);
                            if (num4 > pnCur)
                            {
                                pnCur = num4;
                                string str12 = base.putstr(message, "pnAm=\"", "\"", num2);
                                string str13 = base.putstr(message, "pnAd=\"", "\"", num2).Replace("&#58;", "%5C%3A");
                                string str14 = base.putstr(message, "pnDir=\"", "\"", num2);
                                string str15 = base.putstr(message, "pnMid=\"", "\"", num2);
                                string indata = "cn=Microsoft.Msn.Hotmail.Ui.Fpp.MailBox&mn=GetInboxData&d=true,false,true,{%22" + boxID + "%22" + msgsPerPage.Trim() + "," + str14 + "," + sAsc.Trim() + "," + sBy.Trim() + ",false,%22" + str12 + "%22,%22" + str13 + "%22," + str11.Trim() + "," + str15.Trim() + ",false,%22%22," + mCt + ",-1,Off},false,null&v=1&mt=" + mt;
                                string unreadIndata = "cn=Microsoft.Msn.Hotmail.Ui.Fpp.MailBox&mn=MarkMessagesReadState&d=false,[%22%22],{%22" + boxID + "%22" + msgsPerPage.Trim() + "," + str14 + "," + sAsc.Trim() + "," + sBy.Trim() + ",false,%2200000000-0000-0000-0000-000000000000%22,%22%22," + str11.Trim() + "," + str15.Trim() + ",false,%22%22," + mCt + ",-1,Off}&v=1&mt=" + mt;
                                base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                                base.streamControl = true;
                                base.MyStringBuilder = this.PostData(url, indata);
                                string str18 = base.MyStringBuilder.ToString();
                                if (str18 != "")
                                {
                                    if (Convert.ToInt32(GlobalValue.mainForm.PageNumber) == 0)
                                    {
                                        base.ShowMessage(string.Concat(new object[] { base.BoxName, "：第", pnCur, "页开始下载" }));
                                        this.downMail(str18, unreadUrl, unreadIndata);
                                        this.getNextPages(str18, boxID, msgsPerPage, sAsc, sBy, mCt, mt, url, unreadUrl, pnCur);
                                        return;
                                    }
                                    if (pnCur <= Convert.ToInt32(GlobalValue.mainForm.PageNumber))
                                    {
                                        base.ShowMessage(string.Concat(new object[] { base.BoxName, "：第", pnCur, "页开始下载" }));
                                        this.downMail(str18, unreadUrl, unreadIndata);
                                        this.getNextPages(str18, boxID, msgsPerPage, sAsc, sBy, mCt, mt, url, unreadUrl, pnCur);
                                    }
                                }
                                return;
                            }
                            num2++;
                        }
                        else
                        {
                            num2++;
                        }
                    }
                }
            }

        }
        public override void login()
        {
            base.cookie = "";
            if (base.m_UserType.IndexOf("无密用户") != -1)
            {
                base.ShowMessage("开始登陆…………");
                base.cookie = base.validationLogin;
                base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                base.streamControl = true;
                base.MyStringBuilder = this.Request("http://mail.live.com/");
                string url = "";
                string indata = "";
                int startIndex = 0;
                string message = "";
                if (base.MyStringBuilder.ToString() != "")
                {
                    message = base.MyStringBuilder.ToString();
                    if (message.IndexOf("id=\"UIFrame\"") >= 0)
                    {
                        string s = base.putstr(message.ToString(), "<base href=\"", "\"", 0);
                        if (s != "-1")
                        {
                            s = HttpUtility.HtmlDecode(s);
                            if (base.Host == null)
                            {
                                string str5 = base.putstr(s, "http:", "/mail/", 0);
                                if ((str5 != "-1") && (base.Host == null))
                                {
                                    base.Host = "http:" + str5 + "/mail/";
                                }
                            }
                        }
                        if (message.IndexOf("Object moved") >= 0)
                        {
                            string str = base.putstr(message, "href=\"", "\"", 0);
                            if (str != "-1")
                            {
                                str = HttpUtility.UrlDecode(str);
                                if (base.Host == null)
                                {
                                    string str7 = base.putstr(s, "http:", "/mail/", 0);
                                    if ((str7 != "-1") && (base.Host == null))
                                    {
                                        base.Host = "http:" + str7 + "/mail/";
                                    }
                                }
                                url = (base.Host + str).Replace("mail//mail", "mail");
                                base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                                base.streamControl = true;
                                base.MyStringBuilder = this.Request(url);
                                message = base.MyStringBuilder.ToString();
                                startIndex = message.IndexOf("name=\"main\"");
                                if (startIndex >= 0)
                                {
                                    string str8 = base.putstr(message, "src=\"", "\"", startIndex);
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
                                                str = base.putstr(base.MyStringBuilder.ToString(), "href=\"/mail/", "\"", 0).Replace("&amp;", "&");
                                                url = base.Host + str;
                                                base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                                                base.streamControl = true;
                                                base.MyStringBuilder = this.Request(url);
                                            }
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
                                str10 = str10.Replace("%2f", "/").Replace("%3f", "?").Replace("%3d", "=");
                                str10 = str10.Substring(str10.IndexOf("/mail/") + 6, str10.Length - (str10.IndexOf("/mail/") + 6));
                                url = base.Host + str10;
                                base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                                base.streamControl = true;
                                base.MyStringBuilder = this.Request(url);
                            }
                        }
                    }
                    message = base.MyStringBuilder.ToString();
                    int index = message.IndexOf("class=\"ManageLink\"");
                    if (index < 0)
                    {
                        index = message.IndexOf("href=\"ManageFoldersLight");
                    }
                    if (index < 0)
                    {
                        string str12 = "";
                        string str13 = "";
                        string str14 = "";
                        startIndex = message.IndexOf("__VIEWSTATE");
                        if (startIndex > 0)
                        {
                            str12 = base.putstr(message, "value=\"", "\"", startIndex);
                        }
                        startIndex = message.IndexOf("TakeMeToInbox");
                        if (startIndex > 0)
                        {
                            str13 = base.putstr(message, "value=\"", "\"", startIndex);
                        }
                        startIndex = message.IndexOf("__EVENTVALIDATION");
                        if (startIndex > 0)
                        {
                            str14 = base.putstr(message, "value=\"", "\"", startIndex);
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
                                    str15 = str15.Replace("%2f", "/").Replace("%3f", "?").Replace("%3d", "=");
                                    url = (base.Host + str15).Replace("mail//mail", "mail");
                                    base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                                    base.streamControl = true;
                                    base.MyStringBuilder = this.Request(url);
                                }
                            }
                            message = base.MyStringBuilder.ToString();
                            index = message.IndexOf("class=\"ManageLink\"");
                            if (index < 0)
                            {
                                index = message.IndexOf("href=\"ManageFoldersLight");
                            }
                        }
                    }
                    if (index > 0)
                    {
                        string str16 = base.putstr(message, "href=\"", "\"", index);
                        if (str16 != "-1")
                        {
                            base.ShowMessage("登陆成功!");
                            this.cookieTemp = base.cookie;
                            url = base.Host + str16;
                            base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                            base.streamControl = true;
                            base.MyStringBuilder = this.Request(url);
                            message = base.MyStringBuilder.ToString();
                            this.getBoxName(message);
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
            }

        }
        public StringBuilder PostData(string url, string indata)
        {
            StreamReader reader = null;
            try
            {
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
                    reader.Close();
                    base.streamControl = false;
                }
            }
            catch (Exception exception)
            {
                base.ShowMessage("Hpost:" + exception.Message);
            }
            reader.Close();
            return base.MyStringBuilder;

        }
        private StringBuilder Request(string url)
        {
            StreamReader reader = null;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                request.Accept = "*/*";
                request.AllowAutoRedirect = false;
                request.KeepAlive = true;
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 5.1; Trident/4.0; GTB6; EmbeddedWB 14.52 from:  http://www.bsalsa.com/ EmbeddedWB 14.52; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022; .NET CLR 3.0.4506.2152; .NET CLR 3.5.30729)";
                request.ServicePoint.Expect100Continue = true;
                request.Headers.Add("Accept-Language: zh-cn");
                request.Headers.Add("Cookie", base.cookie);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                WebHeaderCollection headers = response.Headers;
                Thread.Sleep(10);
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
                base.ShowMessage(string.Concat(new object[] { "Hrequest:", exception.Message, "\r\n", exception.Source, "\r\n", exception.StackTrace, "\r\n", exception.HelpLink, "\r\n", exception.TargetSite }));
            }
            base.streamControl = false;
            reader.Close();
            return base.MyStringBuilder;

        }
        public void SaveHotmailEmail(string EmailText)
        {
            try
            {
                string filePath = "";
                if ((GlobalValue.mainForm.saveFilePath != "") | (GlobalValue.mainForm.saveFilePath != null))
                {
                    filePath = GlobalValue.mainForm.saveFilePath;
                }
                this.SaveHotmailFile(EmailText, filePath);
            }
            catch (Exception exception)
            {
                base.ShowMessage("保存邮件失败：" + exception.Message);
                return;
            }
            try
            {
                this.saveHotmailEmailCount();
                GlobalValue.mainForm.statusBarPanelEmailNumbersText.Text = (Convert.ToInt32(GlobalValue.mainForm.statusBarPanelEmailNumbersText.Text) + 1).ToString();
            }
            catch (Exception exception2)
            {
                base.ShowMessage(exception2.Message);
            }

        }
        public void saveHotmailEmailCount()
        {
            try
            {
                string str = "'" + DateTime.Today.ToString() + "'";
                string strSql = string.Concat(new object[] { "update users set 完成时间=", str, ",邮件数量='", base.m_emailno, "' where 服务器='", base.server, "' and 用户名='", base.m_username, "'" });
                GlobalValue.mainForm.ExecuteSQL(strSql);
            }
            catch (Exception exception)
            {
                base.ShowMessage(exception.Message);
            }

        }
        public void SaveHotmailFile(string EmailText, string filePath)
        {
            base.m_emailno++;
            string str = ".eml";
            string str2 = DateTime.Now.Date.ToString("yyy-MM-dd");
            filePath = filePath + str2 + "邮件";
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            filePath = filePath + @"\" + base.m_userListName;
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            if (base.m_snote != "capH")
            {
                string str3 = filePath;
                filePath = str3 + @"\" + base.m_username + "(" + base.m_snote + ")";
            }
            else
            {
                filePath = filePath + @"\" + base.m_username;
            }
            filePath = filePath + @"\" + base.m_username;
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
            if ((base.charSet == "") || (base.charSet == null))
            {
                base.charSet = Encoding.Default.BodyName;
            }
            using (StreamWriter writer = new StreamWriter(stream, Encoding.GetEncoding(base.charSet)))
            {
                writer.Write(EmailText);
            }
            stream.Close();
            stream = null;

        }
    }
}
