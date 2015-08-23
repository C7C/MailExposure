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
    class HotMailReceiver:MailStr
    {
        // Fields
        private string AuthUserTemp;
        private BoxNameID[] boxList;
        private string cookie;
        private string cookieTemp;
        public string m_emailTime;
        private string mailTemp;
        private string mt;
        private static object SelMailBoxL;
        private string SessionIdTemp;
        private string urls;

        // Methods
        static HotMailReceiver()
        {
            SelMailBoxL = new object();
        }
        public HotMailReceiver()
        {
            this.cookie = "";
            this.urls = "";
            this.cookieTemp = "";
            this.SessionIdTemp = "";
            this.AuthUserTemp = "";
            this.mailTemp = "";
            this.mt = "";
            this.boxList = new BoxNameID[100];

        }
        private void downMail(string message, string BoxID, string unreadUrl, string unreadIndata, bool ifFirst)
        {
            int startIndex = 0;
            string mailID = "";
            string url = "";
            string str3 = "";
            bool flag = false;
            string str4 = unreadIndata;
            bool flag2 = false;
            string str8 = "";
            this.cookie = this.cookieTemp;
            startIndex = message.IndexOf("MessageListItems", startIndex);
            if (startIndex > 0)
            {
                while ((startIndex = message.IndexOf("<tr ", startIndex)) > 0)
                {
                    str8 = "";
                    string str9 = base.putstr(message, "<tr ", "/tr>", startIndex - 1);
                    unreadIndata = str4;
                    if (ifFirst)
                    {
                        mailID = base.putstr(str9, "id=\"", "\"", 0);
                        str3 = base.putstr(str9, "class=\"", "\"", 0);
                        if ((str3 != "-1") && (str3.IndexOf("mlUnrd") != -1))
                        {
                            flag = true;
                        }
                    }
                    else
                    {
                        mailID = base.putstr(message, "id=\\\"", "\\\"", startIndex);
                        str3 = base.putstr(message, "class=\\\"", "\\\"", startIndex);
                        if ((str3 != "-1") && (str3.IndexOf("mlUnrd") != -1))
                        {
                            flag = true;
                        }
                    }
                    string str5 = HttpUtility.HtmlDecode(base.putstr(message, "<td class=Fm><a>", "</a>", startIndex));
                    string str6 = HttpUtility.HtmlDecode(base.putstr(message, "<td class=Dt>", "</td>", startIndex));
                    string mailSubject = HttpUtility.HtmlDecode(base.putstr(message, "<a href=#>", "</a>", startIndex));
                    if (str9.IndexOf("<td class=Ct>(") > 0)
                    {
                        str8 = base.putstr(str9, "<td class=Ct>", "</td>", 0);
                    }
                    this.cookie = this.cookieTemp;
                    if (!(mailID != "-1") || !(mailID != ""))
                    {
                        goto Label_0762;
                    }
                    base.charSet = "GB2312";
                    string strSql = "select count(*) from HotmailId where MsgId='" + mailID + "'";
                    if (Convert.ToInt32(GlobalValue.PopMainForm.ExecuteSQL(strSql)) != 0)
                    {
                        goto Label_0773;
                    }
                    bool flag3 = false;
                    try
                    {
                        DateTime time = new DateTime();
                        time = Convert.ToDateTime(str6);
                        if (DateTime.Compare(time, GlobalValue.PopMainForm.EmailDateTime) >= 0)
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
                    if (str8.Length > 0)
                    {
                        int index = message.IndexOf("SessionId:");
                        string sessionIdTemp = "";
                        string authUserTemp = "";
                        string str13 = "";
                        if (index > 0)
                        {
                            sessionIdTemp = base.putstr(message, "\"", "\"", index);
                            this.SessionIdTemp = sessionIdTemp;
                        }
                        index = message.IndexOf("AuthUser:");
                        if (index > 0)
                        {
                            authUserTemp = base.putstr(message, "\"", "\"", index);
                            this.AuthUserTemp = authUserTemp;
                        }
                        index = message.IndexOf("PartnerID:");
                        if (index > 0)
                        {
                            str13 = base.putstr(message, "\"", "\"", index);
                        }
                        if (str13 == "")
                        {
                            str13 = "0";
                        }
                        if (sessionIdTemp == "")
                        {
                            sessionIdTemp = this.SessionIdTemp;
                        }
                        if (authUserTemp == "")
                        {
                            authUserTemp = this.AuthUserTemp;
                        }
                        string str14 = base.Host + "mail.fpp?cnmn=Microsoft.Msn.Hotmail.Ui.Fpp.MailBox.GetConversationInboxData&ptid=" + str13 + "&a=" + sessionIdTemp + "&au=" + authUserTemp;
                        string indata = "cn=Microsoft.Msn.Hotmail.Ui.Fpp.MailBox&mn=GetConversationInboxData&d=false,false,false,null,true,{\"" + mailID + "\",null,\"" + BoxID + "\",true,\"" + base.m_username + "@" + base.m_serv + "\"}&v=1";
                        base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                        base.streamControl = true;
                        base.MyStringBuilder = this.PostData(str14, indata);
                        string input = HttpUtility.HtmlDecode(base.MyStringBuilder.ToString());
                        string pattern = "mid=\\\\\\\"(?<mid>[\\w\\-]+)\\\\\\\"[\\s\\S]+?<div id=\\\\\\\"mp\\d+_Date\\\\\\\".?> ?(?<data>[\\s\\S]+?) ?<";
                        Regex regex = new Regex(pattern);
                        for (Match match = regex.Match(input); match.Success; match = match.NextMatch())
                        {
                            mailID = match.Groups["mid"].Value;
                            this.GetMidMail(mailID, match.Groups["data"].Value, mailSubject, "");
                        }
                        startIndex++;
                        continue;
                    }
                    if (!flag3)
                    {
                        goto Label_0773;
                    }
                    if (base.m_serv.IndexOf("hotmail.co.jp") != -1)
                    {
                        url = base.Host + "GetMessageSource.aspx?msgid=" + mailID;
                    }
                    else
                    {
                        url = base.Host + "GetMessageSource.aspx?msgid=" + mailID + "&gs=true";
                    }
                    base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                    base.streamControl = true;
                    if (base.m_serv.IndexOf("hotmail.co.jp") != -1)
                    {
                        base.MyStringBuilder = this.RequestEmail(url);
                    }
                    else
                    {
                        base.MyStringBuilder = this.RequestEmail(url);
                    }
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
                    }
                    else
                    {
                        flag2 = true;
                        base.ShowMessage("邮件：" + mailSubject + "下载失败");
                    }
                    if (flag)
                    {
                        try
                        {
                            flag = false;
                            base.ShowMessage("置未读：" + mailSubject + "\t");
                            goto Label_06A0;
                        }
                        catch (Exception exception)
                        {
                            base.ShowMessage("置未读失败：" + mailSubject + "\t" + exception.Message);
                            startIndex++;
                            continue;
                        }
                    }
                    base.ShowMessage("已读：" + mailSubject + "\t");
                Label_06A0:
                    startIndex++;
                    if (!flag2)
                    {
                        try
                        {
                            string str19 = DateTime.Now.ToString();
                            strSql = "insert into HotmailId(Name,MsgId,DownTime,MailType) values('" + base.m_username + "','" + mailID + "','" + str19 + "','" + base.BoxName.Substring(0, base.BoxName.Length % 10) + "')";
                            GlobalValue.PopMainForm.ExecuteSQL(strSql);
                            goto Label_0773;
                        }
                        catch (Exception exception2)
                        {
                            base.ShowMessage("添加失败：" + exception2.Message);
                            goto Label_0773;
                        }
                    }
                    flag2 = false;
                    goto Label_0773;
                Label_0762:
                    startIndex++;
                    base.ShowMessage("取邮件ID失败！");
                    continue;
                Label_0773:
                    startIndex++;
                }
            }

        }
        private void getBoxName(string message)
        {
            int index = 0;
            string str = "";
            string pattern = "<td class=\"ManageFoldersFolderNameCol\"><div.*?href=\"(?<BoxUrl>[^\"]+)\".*?>(?<BoxNamme>[^<]+)</a[\\s\\S]+?<\\/td>";
            Match match = new Regex(pattern).Match(message);
            if (match.Length < 1)
            {
                base.ShowMessage("取箱子失败!");
            }
            else
            {
                int num2 = 0;
                while (match.Success)
                {
                    this.boxList[num2].boxname = base.BoxName = match.Groups["BoxNamme"].Value;
                    this.boxList[num2].boxUrl = str = match.Groups["BoxUrl"].Value;
                    this.boxList[num2].boxid = base.putstr(str, "FolderID=", "&", 0);
                    match = match.NextMatch();
                    num2++;
                }
                index = 0;
                while (index < num2)
                {
                    string url = base.Host + this.boxList[index].boxUrl;
                    this.cookie = this.cookieTemp;
                    base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                    base.streamControl = true;
                    base.MyStringBuilder = this.Request(url);
                    this.getPages(base.MyStringBuilder.ToString(), index);
                    index++;
                }
                lock (SelMailBoxL)
                {
                    for (index = 0; index < num2; index++)
                    {
                        string strSql = string.Concat(new object[] { "select count(*) from MailBoxList where 序号='", base.m_NO, "' and MailBoxName = '", this.boxList[index].boxname, "'" });
                        if (Convert.ToInt32(GlobalValue.PopMainForm.ExecuteSQL(strSql)) == 0)
                        {
                            strSql = string.Concat(new object[] { "insert into MailBoxList (序号,MailBoxName)values('", base.m_NO, "','", this.boxList[index].boxname, "');" });
                            GlobalValue.PopMainForm.ExecuteSQL(strSql);
                        }
                    }
                }
            }

        }
        private void GetMidMail(string mailID, string mailDate, string mailSubject, string boxid)
        {
            string url = "";
            bool flag = false;
            base.charSet = "GB2312";
            string strSql = "select count(*) from HotmailId where MsgId='" + mailID + "'";
            if (Convert.ToInt32(GlobalValue.PopMainForm.ExecuteSQL(strSql)) == 0)
            {
                bool flag2 = false;
                try
                {
                    DateTime time = new DateTime();
                    if (DateTime.Compare(Convert.ToDateTime(mailDate), GlobalValue.PopMainForm.EmailDateTime) >= 0)
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
                    this.cookie = this.cookieTemp;
                    if (base.m_serv.IndexOf("hotmail.co.jp") != -1)
                    {
                        url = base.Host + "GetMessageSource.aspx?msgid=" + mailID;
                    }
                    else
                    {
                        url = base.Host + "GetMessageSource.aspx?msgid=" + mailID + "&gs=true";
                    }
                    base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                    base.streamControl = true;
                    if (base.m_serv.IndexOf("hotmail.co.jp") != -1)
                    {
                        base.MyStringBuilder = this.RequestEmail(url);
                    }
                    else
                    {
                        base.MyStringBuilder = this.RequestEmail(url);
                    }
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
                        base.ShowMessage("邮件：" + mailSubject + "\t");
                    }
                    else
                    {
                        flag = true;
                        base.ShowMessage("邮件：" + mailSubject + "下载失败");
                    }
                    if (!flag)
                    {
                        try
                        {
                            string str4 = DateTime.Now.ToString();
                            strSql = "insert into HotmailId(Name,MsgId,DownTime,MailType) values('" + base.m_username + "','" + mailID + "','" + str4 + "','" + base.BoxName.Substring(0, base.BoxName.Length % 10) + "')";
                            GlobalValue.PopMainForm.ExecuteSQL(strSql);
                            return;
                        }
                        catch (Exception exception)
                        {
                            base.ShowMessage("添加失败：" + exception.Message);
                            return;
                        }
                    }
                    flag = false;
                }
            }

        }
        private void getNextPages(string message, string boxID, string msgsPerPage, string trID, string mdt, string mCt, string mt, string pagesUrl, string unreadUrl, int pnCur)
        {
            Match match;
            string s = "";
            int num = 0;
            int num2 = 0;
            int index = 0;
            index = message.IndexOf("mdt=");
            trID = "";
            trID = base.putstr(message, "id=\"", "\"", index - 130);
            s = num.ToString();
            string pattern = "class=\\\\?\"NextPageEnabled\\\\?\" id=\\\\?\"nextPageLink\\\\?\"[^\\/]+?pnCur=\\\\?\"(?<pncur>[\\d]+)\\\\?\"";
            Regex regex = new Regex(pattern);
            for (match = regex.Match(message); match.Success; match = match.NextMatch())
            {
                s = match.Groups["pncur"].Value;
                num = int.Parse(s);
            }
            pattern = "class=\\\\?\"LastPageEnabled\\\\?\" id=\\\\?\"lastPageLink\\\\?\"[^\\/]+?pnCur=\\\\?\"(?<pncur>[\\d]+)\\\\?\"";
            regex = new Regex(pattern);
            for (match = regex.Match(message); match.Success; match = match.NextMatch())
            {
                num2 = int.Parse(match.Groups["pncur"].Value);
            }
            string str4 = "";
            pattern = "id=\\\\?\\\"(?<LastID>[\\w-]+)\\\\?\\\"msg=\\\\?\\\"msg\\\\?\\\"?.mad=\\\\?\\\"[^\\\"]+\\\\?\\\".mdt=\\\\?\\\"(?<mdt>[^\"]+)\\\\?\\\"";
            regex = new Regex(pattern);
            for (match = regex.Match(message); match.Success; match = match.NextMatch())
            {
                str4 = match.Groups["LastID"].Value;
                mdt = match.Groups["mdt"].Value;
                if (mdt.IndexOf(@"\") > 0)
                {
                    mdt = mdt.Substring(0, mdt.IndexOf(@"\"));
                }
            }
            trID = str4;
            if (num > 0)
            {
                string indata = "cn=Microsoft.Msn.Hotmail.Ui.Fpp.MailBox&mn=GetInboxData&d=true,false,true,{%22" + boxID + "%22,,,2,5," + s + ",%22" + trID + "%22,%22" + HttpUtility.UrlEncode(mdt) + "%22,Date,false,false,%22%22,null,-1,Bottom," + mCt + ",null,null,false},false,null&v=1";
                string unreadIndata = "";
                this.cookie = this.cookieTemp;
                base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                base.streamControl = true;
                base.MyStringBuilder = this.PostData(pagesUrl, indata);
                string str7 = base.MyStringBuilder.ToString();
                if (str7 != "")
                {
                    if (Convert.ToInt32(GlobalValue.PopMainForm.PageNumber) == 0)
                    {
                        if (num2 != num)
                        {
                            base.ShowMessage(base.BoxName + "：第" + s + "页开始下载");
                            this.downMail(str7, boxID, unreadUrl, unreadIndata, false);
                            base.ShowMessage(base.BoxName + "：第" + s + "页下载完毕");
                            this.getNextPages(str7, boxID, s, trID, mdt, mCt, mt, pagesUrl, unreadUrl, pnCur);
                        }
                        else
                        {
                            base.ShowMessage(base.BoxName + "：第" + s + "页开始下载");
                            this.downMail(str7, boxID, unreadUrl, unreadIndata, false);
                            base.ShowMessage(base.BoxName + "：第" + s + "页下载完毕");
                        }
                    }
                    else if (pnCur <= Convert.ToInt32(GlobalValue.PopMainForm.PageNumber))
                    {
                        if (num2 != num)
                        {
                            base.ShowMessage(base.BoxName + "：第" + s + "页开始下载");
                            this.downMail(str7, boxID, unreadUrl, unreadIndata, false);
                            base.ShowMessage(base.BoxName + "：第" + s + "页下载完毕");
                            this.getNextPages(str7, boxID, s, trID, mdt, mCt, mt, pagesUrl, unreadUrl, pnCur);
                        }
                        else
                        {
                            base.ShowMessage(base.BoxName + "：第" + s + "页开始下载");
                            this.downMail(str7, boxID, unreadUrl, unreadIndata, false);
                            base.ShowMessage(base.BoxName + "：第" + s + "页下载完毕");
                        }
                    }
                }
            }
        }

        private void getPages(string message, int ID)
        {
            int startIndex = 0;
            string pagesUrl = "";
            string str2 = "";
            string str3 = "";
            string str4 = "";
            string unreadUrl = "";
            string boxid = this.boxList[ID].boxid;
            base.BoxName = this.boxList[ID].boxname;
            message = HttpUtility.HtmlDecode(message);
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
                pagesUrl = base.Host + "mail.fpp?cnmn=Microsoft.Msn.Hotmail.Ui.Fpp.MailBox.GetInboxData&a=" + str2 + "&au=" + str3 + "&ptid=" + str4;
                unreadUrl = base.Host + "mail.fpp?cnmn=Microsoft.Msn.Hotmail.Ui.Fpp.MailBox.MarkMessagesReadState&ptid=" + str4 + "&a=" + str2 + "&au=" + str3;
                startIndex = message.IndexOf("mainContentContainer");
                string mCt = "";
                string mdt = "";
                if (startIndex > 0)
                {
                    base.putstr(message, "sBy=\"", "\"", startIndex);
                    base.putstr(message, "sAsc=\"", "\"", startIndex);
                }
                this.mt = base.putstr(this.cookieTemp, "mt=", ";", 0);
                mCt = base.putstr(message, "mCt=\"", "\"", 0);
                mdt = base.putstr(message, "mdt=\"", "\"", 0);
                int index = 0;
                index = message.IndexOf("mdt=");
                string trID = "";
                trID = base.putstr(message, "id=\"", "\"", index - 130);
                base.ShowMessage(base.BoxName + "：第1页开始下载");
                this.downMail(message, boxid, unreadUrl, "", true);
                base.ShowMessage(base.BoxName + "：第1页下载完成");
                this.getNextPages(message, boxid, "1", trID, mdt, mCt, this.mt, pagesUrl, unreadUrl, 1);
            }

        }
        private void getPagesa(string message, int ID)
        {
            Match match;
            string boxid = this.boxList[ID].boxid;
            string pattern = "SessionId: \"(?<SID>[^\"]+)\".*?AuthUser: \"(?<AU>[\\d]+)\"";
            Regex regex = new Regex(pattern);
            for (match = regex.Match(message); match.Success; match = match.NextMatch())
            {
                string text1 = match.Groups["SID"].Value;
                string text2 = match.Groups["AU"].Value;
            }
            this.downMail(message, boxid, "", "", false);
            string str3 = "<tr (class=\\?\"(?<unrd>mlUnrd)\")?.*?id=\"(?<msgID>[\\w\\d\\-]+)\\?\".*?>[\\s\\S]+?mad=\\?\"(?<msgmad>[\\w|]+)\\?\".*?class=Fm>(?<MailFrom>[\\s\\S]+?</td).*?class=Sb>(?<MailSubject>[\\s\\S]+?</td)[\\s\\S]+?class=Dt.*?>(?<MailDate>[^<]+)?</td>[\\s\\S]+?</tr>";
            Regex regex2 = new Regex(str3);
            for (match = regex2.Match(message); match.Success; match = match.NextMatch())
            {
                bool flag1 = match.Groups["unrd"].Value == "mlUnrd";
                string text3 = match.Groups["msgID"].Value;
                string str4 = HttpUtility.HtmlDecode(match.Groups["MailFrom"].Value);
                string str5 = HttpUtility.HtmlDecode(match.Groups["MailDate"].Value);
                string str6 = HttpUtility.HtmlDecode(match.Groups["MailSubject"].Value);
                string text4 = match.Groups["SID"].Value;
                string text5 = match.Groups["AU"].Value;
            }
        }
        public override void login()
        {
            this.cookie = "";
            if (base.m_serv.Trim() == "H")
            {
                base.m_serv = "hotmail.com";
            }
            base.m_UserType.IndexOf("无密用户");
            if (base.m_passwd.Length > 0x10)
            {
                base.m_passwd = base.m_passwd.Substring(0, 0x10);
            }
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
                if (base.m_serv.IndexOf("msn.com") != -1)
                {
                    url = url.Insert(8, "msnia.");
                }
            }
            if (((str2 != "-1") || (str3 != "-1")) || (url != "-1"))
            {
                if (((str3 == "") && (str2 == "")) && (url == ""))
                {
                    base.ShowMessage("取信息失败!");
                    base.LoginFail();
                    base.passwdErr();
                    base.ShowMessage("登陆失败…………");
                }
                else
                {
                    indata = "idsbho=1&PwdPad=IfYouAreReadingThisYouHaveTooMuc&LoginOptions=3&CS=&FedState=&PPSX=" + str3 + "&type=11&login=" + base.m_username.Trim() + "@" + base.m_serv.Trim() + "&passwd=" + base.m_passwd + "&NewUser=1&PPFT=" + str2 + "&i1=0&i2=2";
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
                                    string str7 = base.putstr(base.MyStringBuilder.ToString(), "href=\"", "\"", 0);
                                    if (str7 != "-1")
                                    {
                                        url = str7.Replace("&amp;", "&");
                                        base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                                        base.streamControl = true;
                                        base.MyStringBuilder = this.Request(url);
                                    }
                                }
                                else if (base.MyStringBuilder.ToString().IndexOf("self.location.href") >= 0)
                                {
                                    string str8 = base.putstr(base.MyStringBuilder.ToString(), "self.location.href = '", "'", 0);
                                    if (str8 != "-1")
                                    {
                                        url = HttpUtility.UrlDecode(str8.Replace(@"\x", "%"));
                                        string str9 = base.putstr(url, "http:", "/mail/", 0);
                                        if ((str9 != "-1") && (base.Host == null))
                                        {
                                            base.Host = "http:" + str9 + "/mail/";
                                        }
                                        base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                                        base.streamControl = true;
                                        base.MyStringBuilder = this.Request(url);
                                    }
                                    if (base.MyStringBuilder.ToString().IndexOf("Object moved") >= 0)
                                    {
                                        str8 = base.putstr(base.MyStringBuilder.ToString(), "href=\"", "\"", 0);
                                        if (str8 != "-1")
                                        {
                                            str8 = str8.Replace("%2f", "/").Replace("%3f", "?").Replace("%3d", "=");
                                            str8 = str8.Substring(str8.IndexOf("/mail/") + 6, str8.Length - (str8.IndexOf("/mail/") + 6));
                                            url = base.Host + str8;
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
                                if ((startIndex < 0) && (this.urls.IndexOf("default.aspx") > -1))
                                {
                                    base.Host = this.urls.Substring(0, this.urls.IndexOf("default.aspx"));
                                    if (message.IndexOf("id=\"h_commandbar\"") > -1)
                                    {
                                        string s = base.putstr(message, "localhref=\"", "\">", message.IndexOf("id=\"h_commandbar\""));
                                        if (s != "-1")
                                        {
                                            this.cookieTemp = this.cookie;
                                            s = HttpUtility.HtmlDecode(s);
                                            string str11 = base.Host + s;
                                            base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                                            base.streamControl = true;
                                            base.MyStringBuilder = this.Request(str11);
                                            message = base.MyStringBuilder.ToString();
                                            startIndex = message.IndexOf("class=\"ManageLink\"");
                                            if (startIndex < 0)
                                            {
                                                startIndex = message.IndexOf("href=\"ManageFoldersLight");
                                                base.Host = base.Host + "mail/";
                                            }
                                        }
                                    }
                                    else if (message.IndexOf("<base href=\"") > -1)
                                    {
                                        base.Host = base.Host + "mail/";
                                        this.mailTemp = base.putstr(message, "<base href=\"", "\"", message.IndexOf("<base href=\""));
                                        if (this.mailTemp != "-1")
                                        {
                                            this.mailTemp = HttpUtility.HtmlDecode(this.mailTemp);
                                            this.cookieTemp = this.cookie;
                                            base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                                            base.streamControl = true;
                                            base.MyStringBuilder = this.Request(this.mailTemp);
                                            message = base.MyStringBuilder.ToString();
                                            startIndex = message.IndexOf("class=\"ManageLink\"");
                                            if (startIndex < 0)
                                            {
                                                startIndex = message.IndexOf("href=\"ManageFoldersLight");
                                            }
                                        }
                                    }
                                }
                                if (startIndex > 0)
                                {
                                    string str12 = base.putstr(message, "href=\"", "\"", startIndex);
                                    if (str12 != "-1")
                                    {
                                        base.ShowMessage("登陆成功!");
                                        base.passwdOK();
                                        url = base.Host + str12;
                                        base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                                        this.cookie = this.cookieTemp;
                                        base.streamControl = true;
                                        base.MyStringBuilder = this.Request(url);
                                        message = HttpUtility.HtmlDecode(base.MyStringBuilder.ToString());
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
                base.ShowMessage("取信息失败!");
                base.LoginFail();
                base.passwdErr();
                base.ShowMessage("登陆失败…………");
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
                request.ContentType = "application/x-www-form-urlencoded";
                request.Headers.Add("x-fpp-command: 0");
                request.Headers.Add("Pragma:no-cache\r\n");
                request.Accept = "*/*";
                if (url.IndexOf("cnmn=Microsoft.Msn.Hotmail.Ui.Fpp.MailBox.GetInboxData") > -1)
                {
                    request.Headers.Add("mt: " + this.mt);
                }
                else if ((url.IndexOf("cnmn=Microsoft.Msn.Hotmail.Ui.Fpp.MailBox.GetConversationInboxData") > -1) || (url.IndexOf("cnmn=Microsoft.Msn.Hotmail.Ui.Fpp.MailBox.MarkMessagesReadState") > -1))
                {
                    request.Headers.Add("mt: " + this.mt);
                }
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)";
                request.Headers.Add("Accept-Language: zh-cn");
                request.ContentLength = indata.Length;
                request.AllowAutoRedirect = false;
                request.KeepAlive = true;
                request.Headers.Add("Cookie", this.cookie);
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
                    this.cookie = base.cook(response.Headers["Set-Cookie"]);
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
        public StringBuilder PostGroupData(string url, string indata)
        {
            StreamReader reader = null;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.ServicePoint.Expect100Continue = false;
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.Headers.Add("x-fpp-command: 0");
                request.Headers.Add("Pragma:no-cache\r\n");
                request.Accept = "*/*";
                if (url.IndexOf("cnmn=Microsoft.Msn.Hotmail.Ui.Fpp.MailBox.GetInboxData") > -1)
                {
                    request.Headers.Add("mt: " + this.mt);
                }
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)";
                request.Headers.Add("Accept-Language: zh-cn");
                request.ContentLength = indata.Length;
                request.AllowAutoRedirect = false;
                request.KeepAlive = true;
                request.Headers.Add("Cookie", this.cookie);
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
                    this.cookie = base.cook(response.Headers["Set-Cookie"]);
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
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)";
                request.ServicePoint.Expect100Continue = false;
                request.ContentType = "text/html; charset=iso-8859-1";
                request.Headers.Add("Accept-Language: zh-cn");
                request.Headers.Add("Cookie", this.cookie);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                WebHeaderCollection headers = response.Headers;
                Thread.Sleep(10);
                if (response.Headers["Set-Cookie"] != null)
                {
                    this.cookie = base.cook(response.Headers["Set-Cookie"]);
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
                    for (string str3 = reader.ReadToEnd(); str3 != null; str3 = null)
                    {
                        base.MyStringBuilder.Append(str3);
                    }
                    base.streamControl = false;
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
        public StringBuilder RequestEmail(string url)
        {
            StringBuilder builder = new StringBuilder();
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Headers.Add("Cookie", this.cookie);
            request.Method = "GET";
            request.Accept = "*/*";
            request.AllowAutoRedirect = false;
            request.KeepAlive = true;
            request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)";
            request.ServicePoint.Expect100Continue = false;
            request.ContentType = "text/html; charset=iso-8859-1";
            request.Headers.Add("Accept-Language: zh-cn");
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            WebHeaderCollection headers = response.Headers;
            Thread.Sleep(10);
            Stream responseStream = response.GetResponseStream();
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
                    this.RequestEmail(this.urls);
                }
            }
            responseStream = response.GetResponseStream();
            if (base.streamControl)
            {
                StreamReader reader;
                Stream stream = new MemoryStream();
                byte[] buffer = new byte[0x1000];
                try
                {
                    int num;
                    while (responseStream.CanRead && ((num = responseStream.Read(buffer, 0, 0x1000)) > 0))
                    {
                        int length = buffer.Length;
                        stream.Write(buffer, 0, num);
                    }
                }
                catch (Exception)
                {
                }
                stream.Seek(0L, SeekOrigin.Begin);
                if ((base.charSet == null) || (base.charSet == "-1"))
                {
                    reader = new StreamReader(stream, Encoding.Default);
                }
                else
                {
                    reader = new StreamReader(stream, Encoding.GetEncoding(base.charSet));
                }
                builder.Append(reader.ReadToEnd());
                responseStream.Close();
                base.streamControl = false;
                return builder;
            }
            responseStream.Close();
            return builder;

        }
        private StringBuilder Requests(string url)
        {
            StreamReader reader = null;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Headers.Add("Cookie", this.cookie);
                request.Method = "GET";
                request.Accept = "*/*";
                request.AllowAutoRedirect = false;
                request.KeepAlive = true;
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 5.1; Trident/4.0; GTB6; EmbeddedWB 14.52 from: http://www.bsalsa.com/ EmbeddedWB 14.52; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022; .NET CLR 3.0.4506.2152; .NET CLR 3.5.30729)";
                request.ServicePoint.Expect100Continue = false;
                request.ContentType = "text/html; charset=iso-8859-1";
                request.Headers.Add("Accept-Language: zh-cn");
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                WebHeaderCollection headers = response.Headers;
                Thread.Sleep(10);
                if (response.Headers["Set-Cookie"] != null)
                {
                    this.cookie = base.cook(response.Headers["Set-Cookie"]);
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
                        base.MyStringBuilder.Append(str3);
                        str3 = null;
                    }
                    base.streamControl = false;
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
                if ((GlobalValue.PopMainForm.saveFilePath != "") | (GlobalValue.PopMainForm.saveFilePath != null))
                {
                    filePath = GlobalValue.PopMainForm.saveFilePath;
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
                GlobalValue.PopMainForm.statusBarPanelEmailNumbersText.Text = (Convert.ToInt32(GlobalValue.PopMainForm.statusBarPanelEmailNumbersText.Text) + 1).ToString();
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
                GlobalValue.PopMainForm.ExecuteSQL(strSql);
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

        // Nested Types
        [StructLayout(LayoutKind.Sequential)]
        private struct BoxNameID
        {
            public string boxUrl;
            public string boxname;
            public string boxid;
        }
    }
}
