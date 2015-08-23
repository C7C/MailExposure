using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Net.Mail;
using System.Globalization;
using System.Web;
using System.Threading;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using MailExposure.CookieMailRC;

namespace MailExposure.CookieMailRC
{
    class JaPanYahooMailReceiver:MailStr
    {
        // Fields
        private string Annex;
        private AnnexUrlName[] annexlist;
        private bool China;
        private CookieContainer cookieContainer;
        public string m_emailTime;
        private string mailSubject;
        private static object pob;
        private string urls;

        // Methods
        static JaPanYahooMailReceiver()
        {
            pob = new object();
        }
        public JaPanYahooMailReceiver()
        {
            this.Annex = "";
            this.urls = "";
            this.annexlist = new AnnexUrlName[200];

        }
        private void downMail(string message)
        {
            int startIndex = 0;
            int num2 = 0;
            bool flag = false;
            bool flag2 = false;
            int num3 = 0;
            int num4 = 0;
            int index = 0;
            string str = "";
            string str2 = "";
            string str3 = "";
            string str4 = "";
            string str5 = "";
            string str6 = "";
            index = message.IndexOf("messageList");
            if (index > 0)
            {
                index = message.IndexOf(".crumb", index);
                if (index > 0)
                {
                    str = base.putstr(message, "value=\"", "\">", index);
                    index = message.IndexOf("warnondelete", index);
                    if (index > 0)
                    {
                        str2 = base.putstr(message, "value=\"", "\">", index);
                        index = message.IndexOf("newfoldermessage", index);
                        if (index > 0)
                        {
                            str3 = base.putstr(message, "value=\"", "\">", index);
                            index = message.IndexOf("ymuri", index);
                            if (index > 0)
                            {
                                str4 = base.putstr(message, "value=\"", "\">", index);
                                index = message.IndexOf("folderscrumb", index);
                                if (index > 0)
                                {
                                    str5 = base.putstr(message, "value=\"", "\">", index);
                                }
                                if (((str == "-1") || (str2 == "-1")) || (((str3 == "-1") || (str4 == "-1")) || (str5 == "-1")))
                                {
                                    base.ShowMessage("取消息失败！");
                                }
                            }
                        }
                    }
                }
            }
            try
            {
                startIndex = 0;
                startIndex = message.IndexOf("<table id=\"datatable\"");
                if (startIndex < 0)
                {
                    base.ShowMessage("取邮件列表失败！");
                }
                else
                {
                    string str7 = base.putstr(message, "<table", "</table>", startIndex);
                    str7 = base.putstr(str7, "<tbody>", "</tbody>", 0);
                    num2 = 0;
                    while ((num2 = str7.IndexOf("href=\"/ym/ShowLetter?MsgId", num2)) > 0)
                    {
                        string str8;
                        string str9;
                        string str10;
                        string str11;
                        MailMessage message2;
                        string str12;
                        string saveFilePath;
                        string str14;
                        string str15;
                        string str16 = base.putstr(str7, "<tr", "</tr>", num2);
                        int num6 = 0;
                        if (str16.IndexOf("class=msgnew", 0) > 0)
                        {
                            flag = true;
                        }
                        str6 = base.putstr(str16, "<td>", "</td>", num6);
                        bool flag1 = str6 == "-1";
                        base.putstr(str16, "<td nowrap class=\"sortcol\">", "</td>", num6);
                        this.mailSubject = base.putstr(str16, "href=\"/ym/ShowLetter?MsgId", "</a>", num6);
                        if ((this.mailSubject != "-1") && (this.mailSubject.IndexOf(">") > 0))
                        {
                            this.mailSubject = this.mailSubject.Substring(this.mailSubject.IndexOf(">") + 1);
                            this.mailSubject = this.mailSubject.Replace("\r\n", "");
                            this.mailSubject = this.mailSubject.Replace("\t", "");
                        }
                        string str17 = base.putstr(str16, "href=\"", "\"", num6);
                        if (str17 == "-1")
                        {
                            num2++;
                        }
                        else
                        {
                            if (str17.IndexOf("#") > 0)
                            {
                                num4 = 0;
                                num3 = 0;
                                this.Annex = "附件";
                                flag2 = true;
                            }
                            str8 = base.putstr(str17, "MsgId=", "&Idx", 0);
                            str10 = "select count(*) from YmailId where MsgId='" + str8 + "'";
                            if (Convert.ToInt32(GlobalValue.mainForm.ExecuteSQL(str10)) != 0)
                            {
                                goto Label_1453;
                            }
                            if (!(str8 != "-1"))
                            {
                                goto Label_1444;
                            }
                            base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                            base.streamControl = true;
                            base.MyStringBuilder = this.Request(base.Host + str17 + "&Nhead=f&PRINT=1");
                            if (base.MyStringBuilder.ToString() == "")
                            {
                                return;
                            }
                            this.m_emailTime = DateTime.Now.ToString("yyyMMdd-HHmmss") + DateTime.Now.Millisecond.ToString();
                            str9 = base.MyStringBuilder.ToString().Replace("CONTENT=\"0; URL=/ym/login?nojs=1\"", "");
                            int num7 = 0;
                            int num8 = 0;
                            if (!this.China)
                            {
                                num7 = str9.IndexOf("<script");
                                num8 = str9.IndexOf("</script>");
                                if ((num7 < 0) || (num8 < 0))
                                {
                                    num2++;
                                    base.ShowMessage("邮件下载失败！");
                                }
                            }
                            if (this.China)
                            {
                                str9 = str9.Replace("hidden", "open").Replace("charset=gb2312", "charset=UTF-8");
                            }
                            else
                            {
                                str9 = str9.Remove(num7, (num8 - num7) + 9).Replace("charset=euc-jp", "charset=UTF-8");
                            }
                            if (!(str9 != "") && (str9 == null))
                            {
                                goto Label_1214;
                            }
                            int num9 = 0;
                            num9 = str9.IndexOf("Message-ID:");
                            if (num9 == -1)
                            {
                                goto Label_0B98;
                            }
                            str11 = base.putstr(str9, "&lt;", "&gt;", num9);
                            if (!(str11 != "-1"))
                            {
                                goto Label_1214;
                            }
                            str10 = "select count(*) from YmailId where MsgId='" + str11 + "'";
                            if (Convert.ToInt32(GlobalValue.mainForm.ExecuteSQL(str10)) != 0)
                            {
                                goto Label_1214;
                            }
                            str9 = str9.Replace("\"Print();\"", " ");
                            MailMessage message3 = new MailMessage();
                            message3.IsBodyHtml = true;
                            message3.Body = str9;
                            message3.BodyEncoding = Encoding.UTF8;
                            message2 = message3;
                            try
                            {
                                message2.From = new MailAddress("t@t.com");
                                if (this.IsEmail(str6))
                                {
                                    message2.From = new MailAddress(str6);
                                }
                                else
                                {
                                    message2.From = new MailAddress("\"" + str6 + "\"");
                                }
                            }
                            catch (Exception)
                            {
                                message2.From = new MailAddress("t@t.com");
                            }
                            try
                            {
                                if (this.mailSubject != "")
                                {
                                    this.mailSubject = this.mailSubject.Replace("\r\n", "");
                                    this.mailSubject = this.mailSubject.Replace("\t", "");
                                    message2.Subject = this.mailSubject;
                                }
                            }
                            catch (Exception)
                            {
                                message2.Subject = " ";
                            }
                            if (!flag2)
                            {
                                goto Label_090A;
                            }
                            num3 = str9.IndexOf("class=\"tabfoldercontent\"", num3);
                            if (num3 >= 0)
                            {
                                goto Label_08E6;
                            }
                            this.Annex = "";
                            flag2 = false;
                            num2++;
                            try
                            {
                                string str18 = DateTime.Now.ToString();
                                str10 = "insert into YmailId(Name,MsgId,DownTime,MailType) values('" + base.m_username + "','" + str11 + "','" + str18 + "','" + base.BoxName.Substring(0, base.BoxName.Length % 10) + "')";
                                GlobalValue.mainForm.ExecuteSQL(str10);
                            }
                            catch (Exception exception)
                            {
                                base.ShowMessage("添加失败：" + exception.Message);
                            }
                            base.ShowMessage("取附件失败！1");
                        }
                        continue;
                    Label_070E:
                        str12 = base.putstr(str9, "<a href=\"", "\"", num3);
                        base.AttName = base.putstr(str9, "\">", "</a>", num3);
                        if ((str12 == "-1") || (base.AttName == "-1"))
                        {
                            num3++;
                            try
                            {
                                string str19 = DateTime.Now.ToString();
                                str10 = "insert into YmailId(Name,MsgId,DownTime,MailType) values('" + base.m_username + "','" + str11 + "','" + str19 + "','" + base.BoxName.Substring(0, base.BoxName.Length % 10) + "')";
                                GlobalValue.mainForm.ExecuteSQL(str10);
                            }
                            catch (Exception exception2)
                            {
                                base.ShowMessage("添加失败：" + exception2.Message);
                            }
                            base.ShowMessage("取附件失败！2");
                        }
                        else
                        {
                            try
                            {
                                num4++;
                                if (this.China)
                                {
                                    str12 = (base.Host + str12 + "&download=1").Replace("ym/ShowLetter?", "/ya/download?");
                                }
                                else
                                {
                                    str12 = base.Host + str12 + "&download=1&filename=" + base.AttName;
                                }
                                base.streamControl = true;
                                Stream contentStream = this.RequestEmail(str12, ref message2);
                                if ((contentStream != null) && contentStream.CanRead)
                                {
                                    message2.Attachments.Add(new Attachment(contentStream, base.AttName));
                                }
                                num3++;
                            }
                            catch (Exception exception3)
                            {
                                num3++;
                                this.Annex = "";
                                flag2 = false;
                                base.ShowMessage("附件下载失败:" + exception3.Message);
                            }
                        }
                    Label_08E6:
                        if ((num3 = str9.IndexOf("<b><a href=\"", num3)) > 0)
                        {
                            goto Label_070E;
                        }
                        this.Annex = "";
                        flag2 = false;
                    Label_090A:
                        saveFilePath = "";
                    if ((GlobalValue.mainForm.saveFilePath != "") | (GlobalValue.mainForm.saveFilePath != null))
                        {
                            saveFilePath = GlobalValue.mainForm.saveFilePath;
                        }
                        string str20 = DateTime.Now.Date.ToString("yyy-MM-dd");
                        this.m_emailTime = DateTime.Now.ToString("yyyMMdd-HHmmss") + DateTime.Now.Millisecond.ToString();
                        string str21 = saveFilePath;
                        saveFilePath = str21 + str20 + @"邮件\" + base.m_snote + @"\" + base.m_stype + @"\" + base.m_username + "@" + base.m_serv + @"\" + DateTime.Now.Date.ToString("yyy-MM-dd") + @"\" + base.BoxName;
                        string emlFileAbsolutePath = saveFilePath + @"\" + this.m_emailTime + ".eml";
                        DirectoryInfo info = new DirectoryInfo(saveFilePath);
                        if (!info.Exists)
                        {
                            info.Create();
                        }
                        new TransformEml(message2, emlFileAbsolutePath);
                        base.m_emailno++;
                        base.saveEmailCount();
                        int num10 = Convert.ToInt32(GlobalValue.mainForm.statusBarPanelEmailNumbersText.Text) + 1;
                        GlobalValue.mainForm.statusBarPanelEmailNumbersText.Text = num10.ToString();
                        base.ShowMessage("下载：" + this.mailSubject + "\t");
                        try
                        {
                            string str23 = DateTime.Now.ToString();
                            str10 = "insert into YmailId(Name,MsgId,DownTime,MailType) values('" + base.m_username + "','" + str11 + "','" + str23 + "','" + base.BoxName.Substring(0, base.BoxName.Length % 10) + "')";
                            GlobalValue.mainForm.ExecuteSQL(str10);
                        }
                        catch (Exception exception4)
                        {
                            base.ShowMessage("添加失败：" + exception4.Message);
                        }
                        goto Label_1214;
                    Label_0B98:
                        str9 = str9.Replace("\"Print();\"", " ");
                        MailMessage message5 = new MailMessage();
                        message5.IsBodyHtml = true;
                        message5.Body = str9;
                        message5.BodyEncoding = Encoding.UTF8;
                        MailMessage mailMsg = message5;
                        try
                        {
                            mailMsg.From = new MailAddress("t@t.com");
                            if (this.IsEmail(str6))
                            {
                                mailMsg.From = new MailAddress(str6);
                            }
                            else
                            {
                                mailMsg.From = new MailAddress("\"" + str6 + "\"");
                            }
                        }
                        catch (Exception)
                        {
                            mailMsg.From = new MailAddress("t@t.com");
                        }
                        try
                        {
                            if (this.mailSubject != "")
                            {
                                mailMsg.Subject = this.mailSubject;
                            }
                        }
                        catch (Exception)
                        {
                            mailMsg.Subject = " ";
                        }
                        if (!flag2)
                        {
                            goto Label_103F;
                        }
                        num3 = str9.IndexOf("class=\"tabfoldercontent\"", num3);
                        if (num3 >= 0)
                        {
                            goto Label_101B;
                        }
                        this.Annex = "";
                        flag2 = false;
                        startIndex++;
                        try
                        {
                            string str24 = DateTime.Now.ToString();
                            str10 = "insert into YmailId(Name,MsgId,DownTime,MailType) values('" + base.m_username + "','" + str8 + "','" + str24 + "','" + base.BoxName.Substring(0, base.BoxName.Length % 10) + "')";
                            GlobalValue.mainForm.ExecuteSQL(str10);
                        }
                        catch (Exception exception5)
                        {
                            base.ShowMessage("添加失败：" + exception5.Message);
                        }
                        base.ShowMessage("取附件失败！3");
                        continue;
                    Label_0D53:
                        str14 = base.putstr(str9, "<a href=\"", "\"", num3);
                        base.AttName = base.putstr(str9, "\">", "</a>", num3);
                        if ((str14 == "-1") || (base.AttName == "-1"))
                        {
                            num3++;
                            try
                            {
                                string str25 = DateTime.Now.ToString();
                                str10 = "insert into YmailId(Name,MsgId,DownTime,MailType) values('" + base.m_username + "','" + str8 + "','" + str25 + "','" + base.BoxName.Substring(0, base.BoxName.Length % 10) + "')";
                                GlobalValue.mainForm.ExecuteSQL(str10);
                            }
                            catch (Exception exception6)
                            {
                                base.ShowMessage("添加失败：" + exception6.Message);
                            }
                            base.ShowMessage("取附件失败4！");
                            try
                            {
                                string str26 = DateTime.Now.ToString();
                                str10 = "insert into YmailId(Name,MsgId,DownTime,MailType) values('" + base.m_username + "','" + str8 + "','" + str26 + "','" + base.BoxName.Substring(0, base.BoxName.Length % 10) + "')";
                                GlobalValue.mainForm.ExecuteSQL(str10);
                                goto Label_101B;
                            }
                            catch (Exception exception7)
                            {
                                base.ShowMessage("添加失败：" + exception7.Message);
                                goto Label_101B;
                            }
                        }
                        try
                        {
                            num4++;
                            base.AttName = string.Concat(new object[] { "附件-", num4, "_", base.AttName });
                            if (this.China)
                            {
                                str14 = (base.Host + str14 + "&download=1").Replace("ym/ShowLetter?", "/ya/download?");
                            }
                            else
                            {
                                str14 = base.Host + str14 + "&download=1&filename=" + base.AttName;
                            }
                            base.streamControl = true;
                            Stream stream2 = this.RequestEmail(str14, ref mailMsg);
                            if ((stream2 != null) && stream2.CanRead)
                            {
                                mailMsg.Attachments.Add(new Attachment(stream2, base.AttName));
                            }
                            num3++;
                        }
                        catch (Exception exception8)
                        {
                            num3++;
                            this.Annex = "";
                            flag2 = false;
                            base.ShowMessage("附件下载失败:" + exception8.Message);
                        }
                    Label_101B:
                        if ((num3 = str9.IndexOf("<b><a href=\"", num3)) > 0)
                        {
                            goto Label_0D53;
                        }
                        this.Annex = "";
                        flag2 = false;
                    Label_103F:
                        str15 = "";
                    if ((GlobalValue.mainForm.saveFilePath != "") | (GlobalValue.mainForm.saveFilePath != null))
                        {
                            str15 = GlobalValue.mainForm.saveFilePath;
                        }
                        string str27 = DateTime.Now.Date.ToString("yyy-MM-dd");
                        this.m_emailTime = DateTime.Now.ToString("yyyMMdd-HHmmss") + DateTime.Now.Millisecond.ToString();
                        str21 = str15;
                        str15 = str21 + str27 + @"邮件\" + base.m_snote + @"\" + base.m_stype + @"\" + base.m_username + "@" + base.m_serv + @"\" + DateTime.Now.Date.ToString("yyy-MM-dd") + @"\" + base.BoxName;
                        string str28 = str15 + @"\" + this.m_emailTime + ".eml";
                        DirectoryInfo info2 = new DirectoryInfo(str15);
                        if (!info2.Exists)
                        {
                            info2.Create();
                        }
                        new TransformEml(mailMsg, str28);
                        base.m_emailno++;
                        base.saveEmailCount();
                        GlobalValue.mainForm.statusBarPanelEmailNumbersText.Text = (Convert.ToInt32(GlobalValue.mainForm.statusBarPanelEmailNumbersText.Text) + 1).ToString();
                        base.ShowMessage("下载：" + this.mailSubject + "\t");
                    Label_1214:
                        if (flag)
                        {
                            try
                            {
                                string str29 = str17.Substring(str17.IndexOf("YY="), str17.Length - str17.IndexOf("YY="));
                                string url = base.Host + "/ym/ShowFolder?" + str29;
                                str29 = str29.Replace("=", "%3D").Replace("&", "%26");
                                string indata = ".crumb=" + str + "&DEL=&FLG=1&MOV=&NewFol=&destBox=&flags=unread&warnondelete=" + str2 + "&newfoldermessage=" + str3 + "&ymuri=" + str4 + "&folderscrumb=" + str5 + "&urlextras=" + str29 + "&delete=%BA%EF%BD%FC&spam=%CC%C2%CF%C7%A5%E1%A1%BC%A5%EB%CA%F3%B9%F0&mark=%A5%D5%A5%E9%A5%B0&Mid=" + str8 + "&delete=%BA%EF%BD%FC&spam=%CC%C2%CF%C7%A5%E1%A1%BC%A5%EB%CA%F3%B9%F0&mark=%A5%D5%A5%E9%A5%B0&move=%B0%DC%C6%B0";
                                base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                                base.streamControl = true;
                                base.MyStringBuilder = this.PostData(url, indata, this.cookieContainer);
                                if (base.MyStringBuilder.ToString() == "")
                                {
                                    return;
                                }
                                base.ShowMessage("置未读：" + this.mailSubject);
                                flag = false;
                            }
                            catch (Exception exception9)
                            {
                                base.ShowMessage("置未读失败：" + str8 + exception9.Message);
                            }
                        }
                        try
                        {
                            string str32 = DateTime.Now.ToString();
                            str10 = "insert into YmailId(Name,MsgId,DownTime,MailType) values('" + base.m_username + "','" + str8 + "','" + str32 + "','" + base.BoxName.Substring(0, base.BoxName.Length % 10) + "')";
                            GlobalValue.mainForm.ExecuteSQL(str10);
                        }
                        catch (Exception exception10)
                        {
                            base.ShowMessage("添加失败：" + exception10.Message);
                        }
                        num2++;
                        goto Label_1453;
                    Label_1444:
                        num2++;
                        base.ShowMessage("取邮件失败！1");
                    Label_1453:
                        num2++;
                    }
                }
            }
            catch (Exception exception11)
            {
                base.ShowMessage("取邮件失败！2" + exception11.Message);
            }

        }
        private void downTWmail(string message)
        {
            int startIndex = 0;
            bool flag = false;
            string str = "";
            string str2 = "";
            int index = 0;
            string str3 = "";
            string str4 = "";
            string str5 = "";
            string str6 = "";
            string str7 = "";
            string str8 = "";
            string str9 = "";
            index = message.IndexOf("aCrumb =", 0);
            if (index > 0)
            {
                str9 = base.putstr(message, "\"", "\"", index);
            }
            index = message.IndexOf("name=\"mcrumb\"", 0);
            if (index > 0)
            {
                str3 = base.putstr(message, "value=\"", "\">", index);
            }
            index = message.IndexOf("spamCleanupPeriod", 0);
            if (index > 0)
            {
                str4 = base.putstr(message, "value=\"", "\">", index);
            }
            index = message.IndexOf("prefNumOfMid", 0);
            if (index > 0)
            {
                str5 = base.putstr(message, "value=\"", "\">", index);
            }
            index = message.IndexOf("name=\"pSize\"", 0);
            if (index > 0)
            {
                str6 = base.putstr(message, "value=\"", "\">", index);
            }
            index = message.IndexOf("totalCount", 0);
            if (index > 0)
            {
                str7 = base.putstr(message, "value=\"", "\">", index);
            }
            index = message.IndexOf("name=\"tt\"", 0);
            if (index > 0)
            {
                str8 = base.putstr(message, "value=\"", "\">", index);
            }
            if ((((str3 == "-1") || (str4 == "-1")) || ((str5 == "-1") || (str6 == "-1"))) || (((str7 == "-1") || (str8 == "-1")) || (str9 == "-1")))
            {
                base.ShowMessage("取消息失败！");
            }
            message = base.putstr(message, "<tbody>", "</tbody>", 0);
            while ((startIndex = message.IndexOf("<tr", startIndex)) > -1)
            {
                try
                {
                    if (message.Substring(startIndex + 11, 6) == "msgnew")
                    {
                        flag = true;
                    }
                    str = base.putstr(message, "value=\"", "\"", startIndex);
                    if (str != "-1")
                    {
                        string strSql = "select count(*) from YmailId where MsgId='" + str + "'";
                        if (Convert.ToInt32(GlobalValue.mainForm.ExecuteSQL(strSql)) == 0)
                        {
                            string str11 = base.putstr(message, "<a href=\"", "\"", startIndex);
                            if (str11 != "-1")
                            {
                                this.m_emailTime = DateTime.Now.ToString("yyyMMdd-HHmmss") + DateTime.Now.Millisecond.ToString();
                                str11 = str11 + "&head=f&pView=1&view=print";
                                base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                                base.streamControl = true;
                                base.MyStringBuilder = this.Request(base.Host + str11);
                                string str12 = base.MyStringBuilder.ToString();
                                str2 = HttpUtility.UrlEncode(str);
                                str = str.Substring(str.LastIndexOf('_') + 1, str.Length - (str.LastIndexOf('_') + 1));
                                strSql = "select count(*) from YmailId where MsgId='" + str + "'";
                                if (Convert.ToInt32(GlobalValue.mainForm.ExecuteSQL(strSql)) == 0)
                                {
                                    string str13 = base.putstr(message, "&lt;", "&gt;", startIndex);
                                    if (str13 == "-1")
                                    {
                                        str13 = base.putstr(message, "<div>", "</div>", startIndex);
                                    }
                                    string str14 = base.putstr(message, "<td class=\"sortcol\">", "</td>", startIndex);
                                    this.mailSubject = base.putstr(message, "\"title=\"", "\">", startIndex);
                                    if (this.mailSubject == "-1")
                                    {
                                        string str15 = base.putstr(message, "<a href=", "</a>", startIndex);
                                        this.mailSubject = base.putstr(str15, "title=\"", "\"", 0);
                                    }
                                    bool flag2 = false;
                                    try
                                    {
                                        DateTime time = new DateTime();
                                        string[] strArray = str14.Split(new char[] { ' ' });
                                        try
                                        {
                                            time = Convert.ToDateTime(strArray[0]);
                                        }
                                        catch (Exception)
                                        {
                                            try
                                            {
                                                DateTimeFormatInfo info2 = new DateTimeFormatInfo();
                                                info2.ShortDatePattern = "MM/dd/yy";
                                                DateTimeFormatInfo provider = info2;
                                                time = DateTime.Parse(strArray[1].Trim(), provider);
                                            }
                                            catch (Exception)
                                            {
                                                strArray[1] = strArray[1].Replace(".'", "-");
                                                strArray[1] = strArray[1].Replace(".", "-");
                                                DateTimeFormatInfo info4 = new DateTimeFormatInfo();
                                                info4.ShortDatePattern = "dd/MM/yy";
                                                DateTimeFormatInfo info3 = info4;
                                                time = DateTime.Parse(strArray[1].Trim(), info3);
                                            }
                                        }
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
                                        string str16;
                                        int num3 = 0;
                                        num3 = str12.IndexOf("Message-ID:");
                                        if (num3 != -1)
                                        {
                                            string str17 = base.putstr(str12, "&lt;", "&gt;", num3).Trim();
                                            if (!(str17 != "-1"))
                                            {
                                                goto Label_18EA;
                                            }
                                            strSql = "select count(*) from YmailId where MsgId='" + str17 + "'";
                                            if (Convert.ToInt32(GlobalValue.mainForm.ExecuteSQL(strSql)) != 0)
                                            {
                                                goto Label_18EA;
                                            }
                                            str12 = str12.Replace("\"Print();\"", " ");
                                            MailMessage message3 = new MailMessage();
                                            message3.IsBodyHtml = true;
                                            message3.Body = str12;
                                            message3.BodyEncoding = Encoding.UTF8;
                                            MailMessage message2 = message3;
                                            message2.From = new MailAddress("t@t.com");
                                            if (this.IsEmail(str13))
                                            {
                                                message2.From = new MailAddress(str13);
                                            }
                                            if (this.mailSubject != "")
                                            {
                                                message2.Subject = this.mailSubject;
                                            }
                                            if (str12.IndexOf("class=\"icons attachicon\"") > 0)
                                            {
                                                this.Annex = "附件";
                                                string str18 = base.putstr(str11, "fid=", "&", 0);
                                                string str19 = base.putstr(str11, "&mid=", "&", 0);
                                                if ((str18 != "-1") && (str19 != "-1"))
                                                {
                                                    string str20 = "";
                                                    if (str12.IndexOf("classicHost") != -1)
                                                    {
                                                        str20 = base.putstr(str12, "\"", "\"", str12.IndexOf("classicHost"));
                                                        if (str20 == "-1")
                                                        {
                                                            str20 = base.Host.Replace("/mc", "/ya").Replace("mc", "f");
                                                        }
                                                        else if (str20.IndexOf("http://") != -1)
                                                        {
                                                            str20 = str20 + "ya/";
                                                        }
                                                        else
                                                        {
                                                            str20 = base.Host.Replace("/mc", "/ya").Replace("mc", "f");
                                                        }
                                                    }
                                                    else
                                                    {
                                                        str20 = base.Host.Replace("/mc", "/ya").Replace("mc", "f");
                                                    }
                                                    string urlAnnex = str20 + "download?clean=0&fid=" + str18 + "&mid=" + str19;
                                                    string str22 = str11;
                                                    base.putstr(str22, "sMid=", "&", 0);
                                                    string str23 = base.putstr(str22, ".rand=", "&", 0);
                                                    base.putstr(str22, "midIndex=", "&", 0);
                                                    base.putstr(str22, "sort=", "&", 0);
                                                    string url = base.Host + "showMessage?sMid=1&&filterBy=&.rand=" + str23 + "&midIndex=0&mid=" + str19 + "&fromId=" + base.m_username + "@" + base.m_serv + "&m=" + str19 + "&sort=date&order=down&startMid=0&acrumb=" + str9 + "&op=data";
                                                    base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                                                    base.streamControl = true;
                                                    base.MyStringBuilder = this.Request(url);
                                                    if (!this.getAnnexS(base.MyStringBuilder.ToString(), urlAnnex, ref message2))
                                                    {
                                                        startIndex++;
                                                        if (flag)
                                                        {
                                                            try
                                                            {
                                                                str18 = base.putstr(str11, "fid=", "&", 0);
                                                                str23 = base.putstr(str11, ".rand=", "&", 0);
                                                                string str25 = "&fid=" + str18 + "&.rand=" + str23;
                                                                string str26 = base.Host + "showFolder?" + str25 + "&needG&acrumb=" + str9 + "&op=data";
                                                                string indata = "startMid=0&sort=date&order=down&mcrumb=" + str3 + "&spamCleanupPeriod=" + str4 + "&prefNumOfMid=" + str5 + "&pSize=" + str6 + "&totalCount=" + str7 + "&tt=" + str8 + "&cmd=mask&top_mark_select=msg.markunread&top_move_select=&mid=" + str2 + "&bottom_mark_select=msg.markunread&bottom_move_select=&self_action_msg_topmark=%E6%A0%87%E8%AE%B0";
                                                                base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                                                                base.streamControl = true;
                                                                base.MyStringBuilder = this.PostData(str26, indata, this.cookieContainer);
                                                                bool flag1 = base.MyStringBuilder.ToString() == "";
                                                                base.ShowMessage("置未读：" + this.mailSubject + "\t");
                                                                flag = false;
                                                            }
                                                            catch (Exception exception)
                                                            {
                                                                base.ShowMessage("置未读失败：" + str + exception.Message);
                                                            }
                                                        }
                                                        continue;
                                                    }
                                                }
                                                this.Annex = "";
                                            }
                                            string saveFilePath = "";
                                            if ((GlobalValue.mainForm.saveFilePath != "") | (GlobalValue.mainForm.saveFilePath != null))
                                            {
                                                saveFilePath = GlobalValue.mainForm.saveFilePath;
                                            }
                                            string str29 = DateTime.Now.Date.ToString("yyy-MM-dd");
                                            this.m_emailTime = DateTime.Now.ToString("yyyMMdd-HHmmss") + DateTime.Now.Millisecond.ToString();
                                            str16 = saveFilePath;
                                            saveFilePath = str16 + str29 + @"邮件\" + base.m_snote + @"\" + base.m_stype + @"\" + base.m_username + "@" + base.m_serv + @"\" + base.BoxName;
                                            string str30 = saveFilePath + @"\" + this.m_emailTime + ".eml";
                                            DirectoryInfo info5 = new DirectoryInfo(saveFilePath);
                                            if (!info5.Exists)
                                            {
                                                info5.Create();
                                            }
                                            new TransformEml(message2, str30);
                                            base.m_emailno++;
                                            base.saveEmailCount();
                                            GlobalValue.mainForm.statusBarPanelEmailNumbersText.Text = (Convert.ToInt32(GlobalValue.mainForm.statusBarPanelEmailNumbersText.Text) + 1).ToString();
                                            base.ShowMessage("下载：" + this.mailSubject + "\t");
                                            if (flag)
                                            {
                                                try
                                                {
                                                    string str31 = base.putstr(str11, "fid=", "&", 0);
                                                    string str32 = base.putstr(str11, ".rand=", "&", 0);
                                                    string str33 = "&fid=" + str31 + "&.rand=" + str32;
                                                    string str34 = base.Host + "showFolder?" + str33 + "&needG&acrumb=" + str9 + "&op=data";
                                                    string str35 = "";
                                                    if (base.Host.IndexOf("http://cn") != -1)
                                                    {
                                                        str35 = "startMid=0&sort=date&order=down&mcrumb=" + str3 + "&spamCleanupPeriod=" + str4 + "&prefNumOfMid=" + str5 + "&pSize=" + str6 + "&totalCount=" + str7 + "&tt=" + str8 + "&cmd=mask&top_mark_select=msg.markunread&top_move_select=&mid=" + str2 + "&bottom_mark_select=msg.markunread&bottom_move_select=&self_action_msg_topmark=%E6%A0%87%E8%AE%B0";
                                                    }
                                                    else
                                                    {
                                                        str35 = "startMid=0&sort=date&order=down&mcrumb=" + str3 + "&spamCleanupPeriod=" + str4 + "&prefNumOfMid=" + str5 + "&pSize=" + str6 + "&totalCount=" + str7 + "&tt=" + str8 + "&cmd=mask&top_mark_select=msg.markunread&top_move_select=&mid=" + str2 + "&bottom_mark_select=msg.markunread&bottom_move_select=&self_action_msg_topmark=Mark";
                                                    }
                                                    base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                                                    base.streamControl = true;
                                                    base.MyStringBuilder = this.PostData(str34, str35, this.cookieContainer);
                                                    if (base.MyStringBuilder.ToString() == "")
                                                    {
                                                        break;
                                                    }
                                                    base.ShowMessage("置未读：" + this.mailSubject + "\t");
                                                    flag = false;
                                                }
                                                catch (Exception exception2)
                                                {
                                                    base.ShowMessage("置未读失败：" + str + exception2.Message);
                                                }
                                            }
                                            try
                                            {
                                                string str36 = DateTime.Now.ToString();
                                                strSql = "insert into YmailId(Name,MsgId,DownTime,MailType) values('" + base.m_username + "','" + str + "','" + str36 + "','" + base.BoxName.Substring(0, base.BoxName.Length % 10) + "')";
                                                GlobalValue.mainForm.ExecuteSQL(strSql);
                                                goto Label_18EA;
                                            }
                                            catch (Exception exception3)
                                            {
                                                base.ShowMessage("添加失败：" + exception3.Message);
                                                goto Label_18EA;
                                            }
                                        }
                                        str12 = str12.Replace("\"Print();\"", " ");
                                        MailMessage message5 = new MailMessage();
                                        message5.IsBodyHtml = true;
                                        message5.Body = str12;
                                        message5.BodyEncoding = Encoding.UTF8;
                                        MailMessage mailMsg = message5;
                                        mailMsg.From = new MailAddress("t@t.com");
                                        if (this.IsEmail(str13))
                                        {
                                            mailMsg.From = new MailAddress(str13);
                                        }
                                        if (this.mailSubject != "")
                                        {
                                            mailMsg.Subject = this.mailSubject;
                                        }
                                        if (str12.IndexOf("class=\"icons attachicon\"") > 0)
                                        {
                                            this.Annex = "附件";
                                            string str37 = base.putstr(str11, "fid=", "&", 0);
                                            string str38 = base.putstr(str11, "&mid=", "&", 0);
                                            if ((str37 != "-1") && (str38 != "-1"))
                                            {
                                                string str39 = "";
                                                if (str12.IndexOf("classicHost") != -1)
                                                {
                                                    str39 = base.putstr(str12, "\"", "\"", str12.IndexOf("classicHost"));
                                                    if (str39 == "-1")
                                                    {
                                                        str39 = base.Host.Replace("/mc", "/ya").Replace("mc", "f");
                                                    }
                                                    else if (str39.IndexOf("http://") != -1)
                                                    {
                                                        str39 = str39 + "ya/";
                                                    }
                                                    else
                                                    {
                                                        str39 = base.Host.Replace("/mc", "/ya").Replace("mc", "f");
                                                    }
                                                }
                                                else
                                                {
                                                    str39 = base.Host.Replace("/mc", "/ya").Replace("mc", "f");
                                                }
                                                string str40 = str39 + "download?clean=0&fid=" + str37 + "&mid=" + str38;
                                                string str41 = str11;
                                                base.putstr(str41, "sMid=", "&", 0);
                                                string str42 = base.putstr(str41, ".rand=", "&", 0);
                                                base.putstr(str41, "midIndex=", "&", 0);
                                                base.putstr(str41, "sort=", "&", 0);
                                                string str43 = base.Host + "showMessage?sMid=1&&filterBy=&.rand=" + str42 + "&midIndex=0&mid=" + str38 + "&fromId=" + base.m_username + "@" + base.m_serv + "&m=" + str38 + "&sort=date&order=down&startMid=0&acrumb=" + str9 + "&op=data";
                                                base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                                                base.streamControl = true;
                                                base.MyStringBuilder = this.Request(str43);
                                                if (!this.getAnnexS(base.MyStringBuilder.ToString(), str40, ref mailMsg))
                                                {
                                                    startIndex++;
                                                    if (flag)
                                                    {
                                                        try
                                                        {
                                                            str37 = base.putstr(str11, "fid=", "&", 0);
                                                            str42 = base.putstr(str11, ".rand=", "&", 0);
                                                            string str44 = "&fid=" + str37 + "&.rand=" + str42;
                                                            string str45 = base.Host + "showFolder?" + str44 + "&needG&acrumb=" + str9 + "&op=data";
                                                            string str46 = "";
                                                            if (base.Host.IndexOf("http://cn") != -1)
                                                            {
                                                                str46 = "startMid=0&sort=date&order=down&mcrumb=" + str3 + "&spamCleanupPeriod=" + str4 + "&prefNumOfMid=" + str5 + "&pSize=" + str6 + "&totalCount=" + str7 + "&tt=" + str8 + "&cmd=mask&top_mark_select=msg.markunread&top_move_select=&mid=" + str2 + "&bottom_mark_select=msg.markunread&bottom_move_select=&self_action_msg_topmark=%E6%A0%87%E8%AE%B0";
                                                            }
                                                            else
                                                            {
                                                                str46 = "startMid=0&sort=date&order=down&mcrumb=" + str3 + "&spamCleanupPeriod=" + str4 + "&prefNumOfMid=" + str5 + "&pSize=" + str6 + "&totalCount=" + str7 + "&tt=" + str8 + "&cmd=mask&top_mark_select=msg.markunread&top_move_select=&mid=" + str2 + "&bottom_mark_select=msg.markunread&bottom_move_select=&self_action_msg_topmark=Mark";
                                                            }
                                                            base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                                                            base.streamControl = true;
                                                            base.MyStringBuilder = this.PostData(str45, str46, this.cookieContainer);
                                                            bool flag3 = base.MyStringBuilder.ToString() == "";
                                                            base.ShowMessage("置未读：" + this.mailSubject + "\t");
                                                            flag = false;
                                                        }
                                                        catch (Exception exception4)
                                                        {
                                                            base.ShowMessage("置未读失败：" + str + exception4.Message);
                                                        }
                                                    }
                                                    continue;
                                                }
                                            }
                                            this.Annex = "";
                                        }
                                        string path = "";
                                        if ((GlobalValue.mainForm.saveFilePath != "") | (GlobalValue.mainForm.saveFilePath != null))
                                        {
                                            path = GlobalValue.mainForm.saveFilePath;
                                        }
                                        string str48 = DateTime.Now.Date.ToString("yyy-MM-dd");
                                        this.m_emailTime = DateTime.Now.ToString("yyyMMdd-HHmmss") + DateTime.Now.Millisecond.ToString();
                                        str16 = path;
                                        path = str16 + str48 + @"邮件\" + base.m_snote + @"\" + base.m_stype + @"\" + base.m_username + "@" + base.m_serv + @"\" + base.BoxName;
                                        string emlFileAbsolutePath = path + @"\" + this.m_emailTime + ".eml";
                                        DirectoryInfo info6 = new DirectoryInfo(path);
                                        if (!info6.Exists)
                                        {
                                            info6.Create();
                                        }
                                        new TransformEml(mailMsg, emlFileAbsolutePath);
                                        base.m_emailno++;
                                        base.saveEmailCount();
                                        GlobalValue.mainForm.statusBarPanelEmailNumbersText.Text = (Convert.ToInt32(GlobalValue.mainForm.statusBarPanelEmailNumbersText.Text) + 1).ToString();
                                        base.ShowMessage("下载：" + this.mailSubject + "\t");
                                        if (flag)
                                        {
                                            try
                                            {
                                                string str50 = base.putstr(str11, "fid=", "&", 0);
                                                string str51 = base.putstr(str11, ".rand=", "&", 0);
                                                string str52 = "&fid=" + str50 + "&.rand=" + str51;
                                                string str53 = base.Host + "showFolder?" + str52 + "&needG&acrumb=" + str9 + "&op=data";
                                                string str54 = "";
                                                if (base.Host.IndexOf("http://cn") != -1)
                                                {
                                                    str54 = "startMid=0&sort=date&order=down&mcrumb=" + str3 + "&spamCleanupPeriod=" + str4 + "&prefNumOfMid=" + str5 + "&pSize=" + str6 + "&totalCount=" + str7 + "&tt=" + str8 + "&cmd=mask&top_mark_select=msg.markunread&top_move_select=&mid=" + str2 + "&bottom_mark_select=msg.markunread&bottom_move_select=&self_action_msg_topmark=%E6%A0%87%E8%AE%B0";
                                                }
                                                else
                                                {
                                                    str54 = "startMid=0&sort=date&order=down&mcrumb=" + str3 + "&spamCleanupPeriod=" + str4 + "&prefNumOfMid=" + str5 + "&pSize=" + str6 + "&totalCount=" + str7 + "&tt=" + str8 + "&cmd=mask&top_mark_select=msg.markunread&top_move_select=&mid=" + str2 + "&bottom_mark_select=msg.markunread&bottom_move_select=&self_action_msg_topmark=Mark";
                                                }
                                                base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                                                base.streamControl = true;
                                                base.MyStringBuilder = this.PostData(str53, str54, this.cookieContainer);
                                                bool flag4 = base.MyStringBuilder.ToString() == "";
                                                base.ShowMessage("置未读：" + this.mailSubject + "\t");
                                                flag = false;
                                            }
                                            catch (Exception exception5)
                                            {
                                                base.ShowMessage("置未读失败：" + str + exception5.Message);
                                            }
                                        }
                                        try
                                        {
                                            string str55 = DateTime.Now.ToString();
                                            strSql = "insert into YmailId(Name,MsgId,DownTime,MailType) values('" + base.m_username + "','" + str + "','" + str55 + "','" + base.BoxName.Substring(0, base.BoxName.Length % 10) + "')";
                                            GlobalValue.mainForm.ExecuteSQL(strSql);
                                        }
                                        catch (Exception exception6)
                                        {
                                            base.ShowMessage("添加失败：" + exception6.Message);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception exception7)
                {
                    base.ShowMessage("邮件下载失败：" + exception7.Message);
                }
            Label_18EA:
                startIndex++;
            }

        }
        private void getAddressbook(string message)
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
                    url = "?_src=&VPC=tools_print";
                    url = this.urls + url;
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
                            str4 = "import_export";
                        }
                        index = str2.IndexOf("submit[action_export_yahoo]");
                        if (index > 0)
                        {
                            str5 = base.putstr(str2, "value=\"", "\"", index);
                        }
                    }
                    if (((str3 != "-1") && (str4 != "-1")) && (str5 != "-1"))
                    {
                        string str6 = this.urls + "?_src=&VPC=tools_export";
                        string indata = ".crumb=" + str3 + "&VPC=" + str4 + "&submit%5Baction_export_yahoo%5D=" + str5;
                        base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                        base.streamControl = true;
                        base.MyStringBuilder = this.PostData(str6, indata, this.cookieContainer);
                        this.SaveAddressbook(base.MyStringBuilder.ToString(), ".csv");
                    }
                }
            }

        }
        private bool getAnnex(string message, string urlAnnex)
        {
            int startIndex = 0;
            int num2 = 0;
            int num3 = 0;
            string str = "";
            string url = "";
            int num4 = 0;
            try
            {
                while ((startIndex = message.IndexOf("<nobr><nobr>", startIndex)) >= 0)
                {
                    url = urlAnnex;
                    num3 = 1;
                    num2++;
                    num3 += num2;
                    str = num3.ToString();
                    string str3 = base.putstr(message, "<nobr><nobr>", "</nobr>", startIndex);
                    if ((str3 != "-1") && ((num4 = str3.IndexOf(".")) > 0))
                    {
                        str3 = str3.Substring(0, str3.IndexOf("(", num4)).Trim();
                        base.AttName = string.Concat(new object[] { "附件-", num2, "_", str3 });
                        string str5 = url;
                        url = str5 + "&pid=" + str + "&tnef=&prefFilename=" + str3;
                        base.streamControl = true;
                        this.RequestEmail(url);
                    }
                    startIndex++;
                }
                for (startIndex = 0; (startIndex = message.IndexOf("<span class=\"imgname\">", startIndex)) >= 0; startIndex++)
                {
                    url = urlAnnex;
                    num3 = 1;
                    num2++;
                    str = (num3 + num2).ToString();
                    string str4 = base.putstr(message, "/>", "</span>", startIndex);
                    if ((str4 != "-1") && ((num4 = str4.IndexOf(".")) > 0))
                    {
                        base.AttName = string.Concat(new object[] { "附件-", num2, "_", str4 });
                        string str6 = url;
                        url = str6 + "&pid=" + str + "&tnef=&prefFilename=" + str4;
                        base.streamControl = true;
                        this.RequestEmail(url);
                    }
                }
            }
            catch (Exception exception)
            {
                base.ShowMessage("下载附件失败！" + exception.Message);
                return false;
            }
            return true;

        }
        private bool getAnnexS(string message, string urlAnnex)
        {
            int startIndex = 0;
            int index = 0;
            int num3 = 0;
            try
            {
                while ((startIndex = message.IndexOf("class=\\\"imgname\\\"", startIndex)) > 0)
                {
                    num3++;
                    string str = base.putstr(message, "href", "title", startIndex);
                    base.putstr(str, ".rand=", "&", 0);
                    base.putstr(str, "midIndex=", "&", 0);
                    base.putstr(str, "sort=", "&", 0);
                    string str2 = base.putstr(str, "pid=", "&", 0);
                    string str3 = base.putstr(str, "fn=", "\\\"", 0);
                    string str4 = base.putstr(str, "&mid=", "&", 0);
                    string str5 = urlAnnex + "&pid=" + str2 + "&tnef=&prefFilename=" + str3 + "&redirectURL=" + base.Host + "showMessage%3Fcmd%3Ddownload.failure%26fid%3D" + base.BoxName + "%26mid%3D" + str4 + "%26pid%3D" + str2 + "%26tnef%3D%26prefFilename%3D" + str3;
                    if ((str3 != "-1") && (str3.IndexOf(".") > 0))
                    {
                        string str6 = string.Concat(new object[] { "附件-", num3, "_", HttpUtility.UrlDecode(str3) });
                        this.annexlist[index].annexname = str6;
                        this.annexlist[index].annexurl = str5;
                    }
                    index++;
                    startIndex++;
                }
                for (int i = 0; i < index; i++)
                {
                    try
                    {
                        base.AttName = this.annexlist[i].annexname;
                        base.streamControl = true;
                        this.RequestEmail(this.annexlist[i].annexurl);
                    }
                    catch (Exception)
                    {
                        int num5 = 0;
                        while (num5 < 2)
                        {
                            try
                            {
                                base.AttName = this.annexlist[i].annexname;
                                base.streamControl = true;
                                this.RequestEmail(this.annexlist[i].annexurl);
                            }
                            catch (Exception exception)
                            {
                                if (num5 == 1)
                                {
                                    base.ShowMessage(base.AttName + "附件下载失败！" + exception.Message);
                                    return true;
                                }
                            }
                            i++;
                        }
                    }
                }
            }
            catch (Exception exception2)
            {
                base.ShowMessage("下载附件失败！" + exception2.Message);
                return true;
            }
            return true;

        }
        private bool getAnnexS(string message, string urlAnnex, ref MailMessage MailMsg)
        {
            int startIndex = 0;
            int index = 0;
            int num3 = 0;
            string str = "";
            try
            {
                while ((startIndex = message.IndexOf("class=\\\"imgname\\\"", startIndex)) > 0)
                {
                    num3++;
                    string str2 = base.putstr(message, "href", "title", startIndex);
                    base.putstr(str2, ".rand=", "&", 0);
                    base.putstr(str2, "midIndex=", "&", 0);
                    base.putstr(str2, "sort=", "&", 0);
                    string str3 = base.putstr(str2, "pid=", "&", 0);
                    str = base.putstr(str2, "fn=", "\\\"", 0);
                    string str4 = base.putstr(str2, "&mid=", "&", 0);
                    string str5 = urlAnnex + "&pid=" + str3 + "&tnef=&prefFilename=" + str + "&redirectURL=" + base.Host + "showMessage%3Fcmd%3Ddownload.failure%26fid%3D" + base.BoxName + "%26mid%3D" + str4 + "%26pid%3D" + str3 + "%26tnef%3D%26prefFilename%3D" + str;
                    if ((str != "-1") && (str.IndexOf(".") > 0))
                    {
                        string str6 = HttpUtility.UrlDecode(str);
                        this.annexlist[index].annexname = str6;
                        this.annexlist[index].annexurl = str5;
                    }
                    index++;
                    startIndex++;
                }
                for (int i = 0; i < index; i++)
                {
                    try
                    {
                        base.AttName = this.annexlist[i].annexname;
                        base.streamControl = true;
                        Stream contentStream = this.RequestEmail(this.annexlist[i].annexurl, ref MailMsg);
                        if ((contentStream != null) && contentStream.CanRead)
                        {
                            MailMsg.Attachments.Add(new Attachment(contentStream, this.annexlist[i].annexname));
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            catch (Exception exception)
            {
                base.ShowMessage("下载附件失败！" + exception.Message);
                return true;
            }
            return true;

        }

        private void getBoxName(string message)
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
                        }
                        else if (str.IndexOf("fmgt.empty") != -1)
                        {
                            startIndex++;
                        }
                        else
                        {
                            base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                            base.streamControl = true;
                            base.MyStringBuilder = this.Request(base.Host + str + "&noFlush");
                            this.getPages(base.MyStringBuilder.ToString());
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
                            base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                            base.streamControl = true;
                            base.MyStringBuilder = this.Request(base.Host + str);
                            this.getPages(base.MyStringBuilder.ToString());
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
        private void getJPAddressbook(string message)
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
                            base.MyStringBuilder = this.PostData(str6, indata, this.cookieContainer);
                            this.SaveAddressbook(base.MyStringBuilder.ToString(), ".csv");
                        }
                    }
                }
            }

        }
        private void getNextPage(string message, int page)
        {
            int startIndex = 0;
            try
            {
                startIndex = message.IndexOf("checkall", startIndex);
                if (startIndex > 0)
                {
                    string str2 = base.putstr(message, "checkall", "</div>", startIndex);
                    if (str2 != "-1")
                    {
                        startIndex = 0;
                        if (str2.IndexOf("<a href=\"", startIndex) > 0)
                        {
                            for (startIndex = 0; (startIndex = str2.IndexOf("<a href=\"", startIndex)) > 0; startIndex++)
                            {
                                string str = base.putstr(str2, "<a href=\"", "\">", startIndex);
                                if (str != "-1")
                                {
                                    string str3 = base.putstr(str, "Npos=", "&", 0);
                                    if (str3 != "-1")
                                    {
                                        int num2 = Convert.ToInt32(str3);
                                        if (num2 > page)
                                        {
                                            page = num2 + 1;
                                            if (Convert.ToInt32(GlobalValue.mainForm.PageNumber) == 0)
                                            {
                                                base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                                                base.streamControl = true;
                                                base.MyStringBuilder = this.Request(base.Host + str);
                                                string str4 = base.MyStringBuilder.ToString();
                                                base.ShowMessage(string.Concat(new object[] { base.BoxName, "：第", page, " 页开始下载" }));
                                                this.downMail(str4);
                                                base.ShowMessage(string.Concat(new object[] { base.BoxName, "：第", page, " 页下载完毕" }));
                                                this.getNextPage(str4, num2);
                                            }
                                            else if (page <= Convert.ToInt32(GlobalValue.mainForm.PageNumber))
                                            {
                                                base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                                                base.streamControl = true;
                                                base.MyStringBuilder = this.Request(base.Host + str);
                                                string str5 = base.MyStringBuilder.ToString();
                                                base.ShowMessage(string.Concat(new object[] { base.BoxName, "：第", page, " 页开始下载" }));
                                                this.downMail(str5);
                                                base.ShowMessage(string.Concat(new object[] { base.BoxName, "：第", page, " 页下载完毕" }));
                                                this.getNextPage(str5, num2);
                                            }
                                            return;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                base.ShowMessage("取页失败！");
            }

        }
        private void getPages(string message)
        {
            int startIndex = 0;
            int page = 0;
            try
            {
                if (base.Host.IndexOf("mc") > 0)
                {
                    page = 0;
                    startIndex = 0;
                    startIndex = message.IndexOf("checkall", startIndex);
                    if (startIndex > 0)
                    {
                        if (message.IndexOf("<a href=\"showFolder;_ylc=", startIndex) > 0)
                        {
                            page++;
                            if (Convert.ToInt32(GlobalValue.mainForm.PageNumber) == 0)
                            {
                                base.ShowMessage(string.Concat(new object[] { base.BoxName, "：第", page, " 页开始下载" }));
                                this.downTWmail(message);
                                base.ShowMessage(string.Concat(new object[] { base.BoxName, "：第", page, " 页下载完毕" }));
                                this.getTWpage(message, page, 0);
                            }
                            else if (page <= Convert.ToInt32(GlobalValue.mainForm.PageNumber))
                            {
                                base.ShowMessage(string.Concat(new object[] { base.BoxName, "：第", page, " 页开始下载" }));
                                this.downTWmail(message);
                                base.ShowMessage(string.Concat(new object[] { base.BoxName, "：第", page, " 页下载完毕" }));
                                this.getTWpage(message, page, 0);
                            }
                        }
                        else
                        {
                            base.ShowMessage(base.BoxName + "：第 1 页开始下载");
                            this.downTWmail(message);
                            base.ShowMessage(base.BoxName + "：第 1 页下载完毕");
                        }
                    }
                    else
                    {
                        base.ShowMessage(base.BoxName + ": 没有邮件！");
                    }
                }
                else
                {
                    page = 0;
                    startIndex = 0;
                    startIndex = message.IndexOf("checkall", startIndex);
                    if (startIndex > 0)
                    {
                        string str = base.putstr(message, "checkall", "</div>", startIndex);
                        if (str != "-1")
                        {
                            startIndex = 0;
                            if (str.IndexOf("<a href=\"", startIndex) > 0)
                            {
                                startIndex = 0;
                                int[] numArray = new int[100];
                                base.ShowMessage(string.Concat(new object[] { base.BoxName, "：第", page, 1, " 页开始下载" }));
                                this.downMail(message);
                                base.ShowMessage(string.Concat(new object[] { base.BoxName, "：第", page, 1, " 页下载完毕" }));
                                while ((startIndex = str.IndexOf("<a href=\"", startIndex)) > 0)
                                {
                                    string str2 = base.putstr(str, "<a href=\"", "\">", startIndex);
                                    if (str != "-1")
                                    {
                                        string str3 = base.putstr(str2, "Npos=", "&", 0);
                                        if (str3 != "-1")
                                        {
                                            int num3 = Convert.ToInt32(str3);
                                            if (num3 > numArray[page])
                                            {
                                                page++;
                                                if (Convert.ToInt32(GlobalValue.mainForm.PageNumber) == 0)
                                                {
                                                    numArray[page] = num3;
                                                    page = num3 + 1;
                                                    base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                                                    base.streamControl = true;
                                                    base.MyStringBuilder = this.Request(base.Host + str2);
                                                    string str4 = base.MyStringBuilder.ToString();
                                                    base.ShowMessage(string.Concat(new object[] { base.BoxName, "：第", page, " 页开始下载" }));
                                                    this.downMail(str4);
                                                    base.ShowMessage(string.Concat(new object[] { base.BoxName, "：第", page, " 页下载完毕" }));
                                                    this.getNextPage(str4, num3);
                                                }
                                                else if (page <= Convert.ToInt32(GlobalValue.mainForm.PageNumber))
                                                {
                                                    numArray[page] = num3;
                                                    page = num3 + 1;
                                                    base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                                                    base.streamControl = true;
                                                    base.MyStringBuilder = this.Request(base.Host + str2);
                                                    string str5 = base.MyStringBuilder.ToString();
                                                    base.ShowMessage(string.Concat(new object[] { base.BoxName, "：第", page, " 页开始下载" }));
                                                    this.downMail(str5);
                                                    base.ShowMessage(string.Concat(new object[] { base.BoxName, "：第", page, " 页下载完毕" }));
                                                    this.getNextPage(str5, num3);
                                                }
                                                return;
                                            }
                                        }
                                    }
                                    startIndex++;
                                }
                            }
                            else
                            {
                                base.ShowMessage(base.BoxName + "：第 1 页开始下载");
                                this.downMail(message);
                                base.ShowMessage(base.BoxName + "：第 1 页下载完毕");
                            }
                        }
                    }
                    else
                    {
                        base.ShowMessage(base.BoxName + ": 没有邮件!");
                    }
                }
            }
            catch (Exception)
            {
                base.ShowMessage("取页失败！");
            }

        }
        private void getTWpage(string message, int page, int mid)
        {
            int startIndex = 0;
            int num2 = 0;
            try
            {
                if (message.IndexOf("checkall", startIndex) > 0)
                {
                    startIndex = 0;
                    if (message.IndexOf("<a href=\"showFolder;_ylc=", startIndex) > 0)
                    {
                        startIndex = 0;
                        for (startIndex = message.IndexOf("checkall", startIndex); (startIndex = message.IndexOf("<a href=\"showFolder;_ylc=", startIndex)) > 0; startIndex++)
                        {
                            string str = base.putstr(message, "<a href=\"", "\">", startIndex);
                            if (str != "-1")
                            {
                                string str2 = base.putstr(str, "&startMid=", "&", 0);
                                if (str2 != "-1")
                                {
                                    num2 = Convert.ToInt32(str2.Trim());
                                    if (num2 > mid)
                                    {
                                        page++;
                                        if (Convert.ToInt32(GlobalValue.mainForm.PageNumber) == 0)
                                        {
                                            base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                                            base.streamControl = true;
                                            base.MyStringBuilder = this.Request(base.Host + str);
                                            string str3 = base.MyStringBuilder.ToString();
                                            base.ShowMessage(string.Concat(new object[] { base.BoxName, "：第", page, " 页开始下载" }));
                                            this.downTWmail(str3);
                                            base.ShowMessage(string.Concat(new object[] { base.BoxName, "：第", page, " 页下载完毕" }));
                                            this.getTWpage(str3, page, num2);
                                        }
                                        else if (page <= Convert.ToInt32(GlobalValue.mainForm.PageNumber))
                                        {
                                            base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                                            base.streamControl = true;
                                            base.MyStringBuilder = this.Request(base.Host + str);
                                            string str4 = base.MyStringBuilder.ToString();
                                            base.ShowMessage(string.Concat(new object[] { base.BoxName, "：第", page, " 页开始下载" }));
                                            this.downTWmail(str4);
                                            base.ShowMessage(string.Concat(new object[] { base.BoxName, "：第", page, " 页下载完毕" }));
                                            this.getTWpage(str4, page, num2);
                                        }
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                base.ShowMessage("取页失败！");
            }

        }
        private string getYCookie(string strCookie)
        {
            string str = "";
            string str2 = "";
            strCookie = " " + strCookie + "; ";
            str = base.putstr(strCookie, " B=", " ", 0);
            if (str != "-1")
            {
                str2 = str2 + "B=" + str + " ";
            }
            str = base.putstr(strCookie, " F=", " ", 0);
            if (str != "-1")
            {
                str2 = str2 + "F=" + str + " ";
            }
            str = base.putstr(strCookie, " Y=", " ", 0);
            if (str != "-1")
            {
                str2 = str2 + "Y=" + str + " ";
            }
            str = base.putstr(strCookie, " T=", " ", 0);
            if (str != "-1")
            {
                str2 = str2 + "T=" + str + " ";
            }
            return str2;

        }
        public bool IsEmail(string str_Email)
        {
            return Regex.IsMatch(str_Email, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }
        public override void login()
        {
            base.cookie = "";
            if (base.m_UserType.IndexOf("无密用户") != -1)
            {
                base.cookie = base.validationLogin;
                string emailuri = base.emailuri;
                base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                base.streamControl = true;
                base.ShowMessage("开始登陆…………");
                base.MyStringBuilder = this.Request(emailuri);
                if ((base.MyStringBuilder.ToString().Length < 800) && (base.MyStringBuilder.ToString().IndexOf("document.location.href") != -1))
                {
                    this.urls = base.putstr(base.MyStringBuilder.ToString(), "'", "'", base.MyStringBuilder.ToString().IndexOf("document.location.href"));
                    if (this.urls != "-1")
                    {
                        base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                        base.streamControl = true;
                        base.MyStringBuilder = this.Request(this.urls);
                    }
                }
                this.China = true;
                if (this.urls.IndexOf("/ym/login?") > 0)
                {
                    base.Host = this.urls.Substring(0, this.urls.IndexOf("/ym/login?"));
                    if (base.Host == "")
                    {
                        base.ShowMessage("取主机失败！");
                        base.LoginFail();
                        base.passwdErr();
                        base.ShowMessage("登陆失败！");
                        return;
                    }
                    if (base.Host.IndexOf("jp") != -1)
                    {
                        this.China = false;
                        int startIndex = base.MyStringBuilder.ToString().IndexOf("<a href=\"/ym/Folders?YY=");
                        if (startIndex > 0)
                        {
                            string str2 = base.putstr(base.MyStringBuilder.ToString(), "<a href=\"", "\">", startIndex);
                            if (str2 != "-1")
                            {
                                base.ShowMessage("登陆成功！");
                                base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                                base.streamControl = true;
                                base.MyStringBuilder = this.Request(base.Host + str2);
                                this.getBoxName(base.MyStringBuilder.ToString());
                                return;
                            }
                        }
                    }
                }
                else if (this.urls.IndexOf("hkr/login?") > 0)
                {
                    int num2 = 0;
                    num2 = base.MyStringBuilder.ToString().IndexOf("<wssid>");
                    if (num2 != -1)
                    {
                        string str3 = base.putstr(base.MyStringBuilder.ToString(), "<wssid>", "</wssid>", num2);
                        emailuri = "http://jp.mg3.mail.yahoo.co.jp/hkr/local/optout?_in=94089&wssid=" + str3;
                        base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                        base.streamControl = true;
                        base.MyStringBuilder = this.Request(emailuri);
                        emailuri = "http://jp.mg3.mail.yahoo.co.jp/ym/login?ymv=1";
                        base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                        base.streamControl = true;
                        base.MyStringBuilder = this.Request(emailuri);
                    }
                    base.Host = this.urls.Substring(0, this.urls.IndexOf("ym/login?"));
                    if (base.Host == "")
                    {
                        base.ShowMessage("取主机失败！");
                        base.LoginFail();
                        base.passwdErr();
                        base.ShowMessage("登陆失败！");
                        return;
                    }
                    if (base.Host.IndexOf("jp") != -1)
                    {
                        this.China = false;
                        int num3 = base.MyStringBuilder.ToString().IndexOf("<a href=\"/ym/Folders?YY=");
                        if (num3 > 0)
                        {
                            string str4 = base.putstr(base.MyStringBuilder.ToString(), "<a href=\"", "\">", num3);
                            if (str4 != "-1")
                            {
                                base.ShowMessage("登陆成功！");
                                base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                                base.streamControl = true;
                                base.MyStringBuilder = this.Request(base.Host + str4);
                                this.getBoxName(base.MyStringBuilder.ToString());
                                return;
                            }
                        }
                    }
                }
                else if (this.urls.IndexOf("/mc/welcome?") > 0)
                {
                    base.Host = this.urls.Substring(0, this.urls.IndexOf("welcome?"));
                    if (base.Host == "")
                    {
                        base.ShowMessage("取主机失败！");
                        base.LoginFail();
                        base.passwdErr();
                        base.ShowMessage("登陆失败！");
                        return;
                    }
                }
                string message = base.MyStringBuilder.ToString();
                if (this.urls.IndexOf("/dc/") > 0)
                {
                    base.Host = this.urls.Substring(0, this.urls.IndexOf("/dc/"));
                    int num4 = message.IndexOf("\"wssid\":");
                    string str6 = base.putstr(message, "\"", "\"", num4 + 7);
                    if (str6 != "-1")
                    {
                        base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                        base.streamControl = true;
                        string indata = "_in=94089&_xml=1&optOutReason=fromCGClient&plt=531&lt=94";
                        base.MyStringBuilder = this.PostData(base.Host + "/dc/optout.php?wssid=" + str6, indata, this.cookieContainer);
                        if (base.MyStringBuilder.ToString().IndexOf("<redirectUrl>") >= 0)
                        {
                            emailuri = base.putstr(base.MyStringBuilder.ToString(), "<redirectUrl>", "</redirectUrl>", 0);
                            if (emailuri != "-1")
                            {
                                emailuri = HttpUtility.HtmlDecode(emailuri);
                                base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                                base.streamControl = true;
                                base.MyStringBuilder = this.Request(emailuri);
                                if (emailuri.IndexOf("/mc/welcome?") > 0)
                                {
                                    base.Host = emailuri.Substring(0, emailuri.IndexOf("welcome?"));
                                }
                            }
                        }
                    }
                }
                string str8 = base.MyStringBuilder.ToString();
                this.getAddressbook(str8);
                int index = str8.IndexOf("<a href=\"/ym/Folders?YY=");
                if (base.Host != null)
                {
                    if (base.Host.IndexOf("mc") > 0)
                    {
                        index = str8.IndexOf("<a href=\"folders?");
                    }
                    else if (base.Host.IndexOf("se") != -1)
                    {
                        index = str8.IndexOf("<a href=\"folders?");
                    }
                    if ((index < 0) && ((index = str8.IndexOf("<a id=\"skip\"")) != -1))
                    {
                        string url = base.putstr(str8, "href=\"", "\"", index);
                        if (url != "-1")
                        {
                            url = url.TrimStart(new char[] { '.' }).Trim(new char[] { '/' });
                            url = "http://tb.mail.yahoo.com/ytoolbar/configserver/ext/mail_interstitial/" + url;
                            base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                            base.streamControl = true;
                            base.MyStringBuilder = this.Request(url);
                            str8 = base.MyStringBuilder.ToString();
                            index = str8.IndexOf("<a href=\"folders?");
                            if ((index < 0) && (this.urls.IndexOf("*") != -1))
                            {
                                url = this.urls.Substring(this.urls.IndexOf("*") + 1, this.urls.Length - (this.urls.IndexOf("*") + 1));
                                base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                                base.streamControl = true;
                                base.MyStringBuilder = this.Request(url);
                                str8 = base.MyStringBuilder.ToString();
                                index = str8.IndexOf("<a href=\"folders?");
                            }
                        }
                    }
                }
                if (index > 0)
                {
                    string str10 = base.putstr(str8, "<a href=\"", "\"", index);
                    if (str10 != "-1")
                    {
                        base.ShowMessage("登陆成功！");
                        base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                        base.streamControl = true;
                        base.MyStringBuilder = this.Request(base.Host + str10 + "&noFlush");
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

        }
        private void loginCom()
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
                else if (base.m_serv.Trim().ToUpper().IndexOf("BTINTERNET.COM") != -1)
                {
                    base.m_username = base.m_username + "@btinternet.com";
                }
                string indata = ".tries=" + str3 + "&.src=" + str4 + "&.md5=&.hash=&.js=&.last=&promo=&.intl=" + str5 + "&.bypass=&.partner=&.u=" + str6 + "&.v=" + str7 + "&.challenge=" + str8 + "&.yplus=&.emailCode=&pkg=&stepid=&.ev=&hasMsgr=" + str10 + "&.chkP=" + str9 + "&.done=http%3A%2F%2Fmail.yahoo.com&.pd=ym_ver%3D0%26c%3D&login=" + base.m_username + "&passwd=" + base.m_passwd + "&.save=Sign+In";
                if (base.m_username.IndexOf("@") != -1)
                {
                    base.m_username = base.m_username.Substring(0, base.m_username.IndexOf("@"));
                }
                base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                base.streamControl = true;
                base.MyStringBuilder = this.PostData(url, indata, this.cookieContainer);
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
                            int num2 = str12.IndexOf("\"wssid\":");
                            string str13 = base.putstr(str12, "\"", "\"", num2 + 7);
                            if (str13 != "-1")
                            {
                                base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                                base.streamControl = true;
                                indata = "_in=94089&_xml=1&optOutReason=fromCGClient&plt=531&lt=94";
                                base.MyStringBuilder = this.PostData(base.Host + "/dc/optout.php?wssid=" + str13, indata, this.cookieContainer);
                                if (base.MyStringBuilder.ToString().IndexOf("<redirectUrl>") >= 0)
                                {
                                    url = base.putstr(base.MyStringBuilder.ToString(), "<redirectUrl>", "</redirectUrl>", 0);
                                    if (url != "-1")
                                    {
                                        url = HttpUtility.HtmlDecode(url);
                                        base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                                        base.streamControl = true;
                                        base.MyStringBuilder = this.Request(url);
                                        if (url.IndexOf("/mc/welcome?") > 0)
                                        {
                                            base.Host = url.Substring(0, url.IndexOf("welcome?"));
                                        }
                                    }
                                }
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
                        if (this.China)
                        {
                            base.streamControl = true;
                            string str16 = "http://address.mail.yahoo.com/?_src=&VPC=tools_print";
                            string str17 = ".src=&VPC=print&field%5Ballc%5D=1&field%5Bcatid%5D=0&field%5Bstyle%5D=detailed&submit%5Baction_display%5D=%E6%89%93%E5%8D%B0%E9%A2%84%E8%A7%88";
                            this.SaveAddressbook(this.PostData(str16, str17, this.cookieContainer).ToString(), ".html");
                        }
                        else
                        {
                            this.getJPAddressbook(str14);
                        }
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
                            if ((index < 0) && ((index = str14.IndexOf("<a id=\"skip\"")) != -1))
                            {
                                string str18 = base.putstr(str14, "href=\"", "\"", index);
                                if (str18 != "-1")
                                {
                                    str18 = str18.TrimStart(new char[] { '.' }).Trim(new char[] { '/' });
                                    str18 = "http://tb.mail.yahoo.com/ytoolbar/configserver/ext/mail_interstitial/" + str18;
                                    base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                                    base.streamControl = true;
                                    base.MyStringBuilder = this.Request(str18);
                                    str14 = base.MyStringBuilder.ToString();
                                    index = str14.IndexOf("<a href=\"folders?");
                                    if ((index < 0) && (this.urls.IndexOf("*") != -1))
                                    {
                                        str18 = this.urls.Substring(this.urls.IndexOf("*") + 1, this.urls.Length - (this.urls.IndexOf("*") + 1));
                                        base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                                        base.streamControl = true;
                                        base.MyStringBuilder = this.Request(str18);
                                        str14 = base.MyStringBuilder.ToString();
                                        index = str14.IndexOf("<a href=\"folders?");
                                    }
                                }
                            }
                        }
                        if (index > 0)
                        {
                            string str19 = base.putstr(str14, "<a href=\"", "\"", index);
                            if (str19 != "-1")
                            {
                                base.ShowMessage("登陆成功！");
                                base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                                base.streamControl = true;
                                base.cookie = cookie;
                                base.MyStringBuilder = this.Request(base.Host + str19 + "&noFlush");
                                this.getBoxName(base.MyStringBuilder.ToString());
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
        private void loginJP()
        {
            base.cookie = "";
            this.China = false;
            base.ShowMessage("开始登陆…………");
            this.cookieContainer = new CookieContainer();
            string url = "https://login.yahoo.co.jp/config/login?";
            base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
            base.streamControl = true;
            base.MyStringBuilder = this.Request("http://mail.yahoo.co.jp/");
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
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }
                string indata = ".tries=" + str3 + "&.src=" + str4 + "&.md5=&.hash=&.js=&.last=&promo=&.intl=" + str5 + "&.bypass=&.partner=&.u=" + str6 + "&.v=" + str7 + "&.challenge=" + str8 + "&.yplus=&.emailCode=&pkg=&stepid=&.ev=&hasMsgr=" + str10 + "&.chkP=" + str9 + "&.done=http%3A%2F%2Fmail.yahoo.co.jp&.pd=&.protoctl=&login=" + base.m_username + "&passwd=" + base.m_passwd;
                base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                base.streamControl = true;
                base.MyStringBuilder = this.PostData(url, indata, this.cookieContainer);
                url = base.putstr(base.MyStringBuilder.ToString(), "url=", "\">", 0);
                if (url != "-1")
                {
                    if (url.IndexOf("http") >= 0)
                    {
                        if (url.IndexOf("/ym/login?") > 0)
                        {
                            base.Host = url.Substring(0, url.IndexOf("/ym/login?"));
                            if (base.Host == "")
                            {
                                base.ShowMessage("取主机失败！");
                                return;
                            }
                        }
                        base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                        base.streamControl = true;
                        base.MyStringBuilder = this.Request(url);
                        string str12 = base.MyStringBuilder.ToString();
                        if (str12.IndexOf("ベータ") != -1)
                        {
                            int num2 = 0;
                            num2 = base.MyStringBuilder.ToString().IndexOf("<wssid>");
                            if (num2 != -1)
                            {
                                string str13 = base.putstr(base.MyStringBuilder.ToString(), "<wssid>", "</wssid>", num2);
                                url = "http://jp.mg3.mail.yahoo.co.jp/hkr/local/optout?_in=94089&wssid=" + str13;
                                base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                                base.streamControl = true;
                                base.MyStringBuilder = this.Request(url);
                                url = "http://jp.mg3.mail.yahoo.co.jp/ym/login?ymv=1";
                                base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                                base.streamControl = true;
                                base.MyStringBuilder = this.Request(url);
                            }
                            base.Host = this.urls.Substring(0, this.urls.IndexOf("ym/login?"));
                            if (base.Host == "")
                            {
                                base.ShowMessage("取主机失败！");
                                base.LoginFail();
                                base.passwdErr();
                                base.ShowMessage("登陆失败！");
                                return;
                            }
                            if (base.Host.IndexOf("jp") != -1)
                            {
                                this.China = false;
                                int num3 = base.MyStringBuilder.ToString().IndexOf("<a href=\"/ym/Folders?YY=");
                                if (num3 > 0)
                                {
                                    string str14 = base.putstr(base.MyStringBuilder.ToString(), "<a href=\"", "\">", num3);
                                    if (str14 != "-1")
                                    {
                                        base.ShowMessage("登陆成功！");
                                        base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                                        base.streamControl = true;
                                        base.MyStringBuilder = this.Request(base.Host + str14);
                                        this.getBoxName(base.MyStringBuilder.ToString());
                                        return;
                                    }
                                }
                            }
                        }
                        string cookie = "";
                        cookie = base.cookie;
                        if (!this.China)
                        {
                            this.getJPAddressbook(str12);
                        }
                        int index = str12.IndexOf("<a href=\"/ym/Folders?YY=");
                        if (index > 0)
                        {
                            string str16 = base.putstr(str12, "<a href=\"", "\">", index);
                            if (str16 != "-1")
                            {
                                base.ShowMessage("登陆成功！");
                                base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                                base.streamControl = true;
                                base.cookie = cookie;
                                base.MyStringBuilder = this.Request(base.Host + str16);
                                this.getBoxName(base.MyStringBuilder.ToString());
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
                    base.LoginFail();
                    base.passwdErr();
                    base.ShowMessage("登陆失败…………");
                }
            }

        }
        public StringBuilder PostData(string url, string indata, CookieContainer cookieContainer)
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
                    this.urls = "";
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
                request.Headers.Add("Cookie", base.cookie);
                request.Method = "GET";
                request.Accept = "*/*";
                request.AllowAutoRedirect = false;
                request.KeepAlive = true;
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1)";
                request.ContentType = "application/x-www-form-urlencoded";
                request.Headers.Add("Accept-Language: zh-cn");
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
                    this.urls = "";
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
                }
                reader.Close();
            }
            catch (Exception exception)
            {
                base.streamControl = false;
                base.ShowMessage(exception.Message);
            }
            return base.MyStringBuilder;

        }
        private bool RequestEmail(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Headers.Add("Cookie", base.cookie);
            request.Method = "GET";
            request.Accept = "*/*";
            request.AllowAutoRedirect = false;
            request.KeepAlive = true;
            request.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; .NET CLR 2.0.50727)";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Headers.Add("Accept-Language: zh-cn");
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Thread.Sleep(10);
            WebHeaderCollection headers = response.Headers;
            if (((response.StatusCode == HttpStatusCode.Found) || (response.StatusCode == HttpStatusCode.MovedPermanently)) || ((response.StatusCode == HttpStatusCode.MovedPermanently) || (response.StatusCode == HttpStatusCode.Found)))
            {
                string str = "";
                str = headers["location"];
                if (str.IndexOf("http") != -1)
                {
                    this.RequestEmail(str);
                }
            }
            Stream responseStream = response.GetResponseStream();
            if (base.streamControl)
            {
                int num;
                byte[] buffer = new byte[0x400];
                if (!(this.Annex == "附件"))
                {
                    base.m_emailno++;
                }
                while ((num = responseStream.Read(buffer, 0, 0x400)) > 0)
                {
                    if (!this.SaveYahoo(buffer, num))
                    {
                        responseStream.Close();
                        response.Close();
                        return false;
                    }
                }
                responseStream.Close();
                response.Close();
                base.saveEmailCount();
                GlobalValue.mainForm.statusBarPanelEmailNumbersText.Text = (Convert.ToInt32(GlobalValue.mainForm.statusBarPanelEmailNumbersText.Text) + 1).ToString();
                base.streamControl = false;
                return true;
            }
            responseStream.Close();
            response.Close();
            return false;

        }
        private Stream RequestEmail(string url, ref MailMessage MailMsg)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Headers.Add("Cookie", base.cookie);
            request.Method = "GET";
            request.Accept = "*/*";
            request.AllowAutoRedirect = false;
            request.KeepAlive = true;
            request.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; .NET CLR 2.0.50727)";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Headers.Add("Accept-Language: zh-cn");
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Thread.Sleep(10);
            WebHeaderCollection headers = response.Headers;
            if (((response.StatusCode == HttpStatusCode.Found) || (response.StatusCode == HttpStatusCode.MovedPermanently)) || ((response.StatusCode == HttpStatusCode.MovedPermanently) || (response.StatusCode == HttpStatusCode.Found)))
            {
                string str = "";
                str = headers["location"];
                if (str.IndexOf("http") != -1)
                {
                    return this.RequestEmail(str, ref MailMsg);
                }
            }
            Stream responseStream = response.GetResponseStream();
            if (base.streamControl)
            {
                int num;
                long contentLength = response.ContentLength;
                string pattern = ".[^;]*;\\s+filename=\"(?<file>.*)\"";
                Match match = new Regex(pattern).Match(response.ContentType);
                if (match.Success)
                {
                    string text1 = match.Groups["file"].Value;
                }
                base.m_emailno++;
                this.m_emailTime = DateTime.Now.ToString("yyyMMdd-HHmmss") + DateTime.Now.Millisecond.ToString();
                Stream stream2 = new MemoryStream();
                byte[] buffer = new byte[0x400];
                if (!(this.Annex == "附件"))
                {
                    base.m_emailno++;
                }
                while ((num = responseStream.Read(buffer, 0, 0x400)) > 0)
                {
                    int length = buffer.Length;
                    stream2.Write(buffer, 0, num);
                }
                stream2.Seek(0L, SeekOrigin.Begin);
                response.Close();
                return stream2;
            }
            response.Close();
            return null;

        }
        public void SaveAddressbook(string EmailText, string type)
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
            Stream stream = File.Create(path + @"\" + base.m_username + "-yahoo_ab" + type);
            using (StreamWriter writer = new StreamWriter(stream, Encoding.Unicode))
            {
                writer.Write(EmailText);
            }
            stream.Close();
            stream = null;

        }
        private void SaveYahoo(string EmailText)
        {
            string filePath = "";
            try
            {
                if ((GlobalValue.mainForm.saveFilePath != "") | (GlobalValue.mainForm.saveFilePath != null))
                {
                    filePath = GlobalValue.mainForm.saveFilePath;
                }
                this.SaveYahooText(EmailText, filePath);
            }
            catch (Exception exception)
            {
                base.ShowMessage("保存邮件失败：" + exception.Message + filePath);
                return;
            }
            try
            {
                base.saveEmailCount();
                GlobalValue.mainForm.statusBarPanelEmailNumbersText.Text = (Convert.ToInt32(GlobalValue.mainForm.statusBarPanelEmailNumbersText.Text) + 1).ToString();
            }
            catch (Exception exception2)
            {
                base.ShowMessage(exception2.Message);
            }

        }
        private bool SaveYahoo(byte[] buffer, int nbytes)
        {
            string filePath = "";
            try
            {
                if ((GlobalValue.mainForm.saveFilePath != "") | (GlobalValue.mainForm.saveFilePath != null))
                {
                    filePath = GlobalValue.mainForm.saveFilePath;
                }
                this.SaveYahooText(buffer, nbytes, filePath);
            }
            catch (Exception exception)
            {
                base.ShowMessage("保存邮件失败：" + exception.Message + filePath);
                return false;
            }
            return true;

        }
        public void SaveYahooText(string EmailText, string filePath)
        {
            Stream stream;
            string str = "";
            base.m_emailno++;
            string str2 = ".html";
            string str3 = DateTime.Now.Date.ToString("yyy-MM-dd");
            filePath = filePath + str3 + "邮件";
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
            string str4 = filePath;
            filePath = str4 + @"\" + base.m_username + "@" + base.m_serv;
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            filePath = filePath + @"\" + DateTime.Now.Date.ToString("yyy-MM-dd");
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            filePath = filePath + @"\" + base.BoxName;
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            try
            {
                str = filePath;
                this.mailSubject = this.UrnHtml(this.mailSubject);
                filePath = string.Concat(new object[] { filePath, @"\", base.m_emailno, "-", this.mailSubject, str2 });
                stream = File.Create(filePath);
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(EmailText);
                }
                stream.Close();
                stream = null;
            }
            catch (Exception)
            {
                filePath = string.Concat(new object[] { str, @"\", base.m_emailno, str2 });
                stream = File.Create(filePath);
                using (StreamWriter writer2 = new StreamWriter(stream))
                {
                    writer2.Write(EmailText);
                }
                stream.Close();
                stream = null;
            }

        }
        public void SaveYahooText(byte[] buffer, int nbytes, string filePath)
        {
            lock (pob)
            {
                FileStream stream = null;
                try
                {
                    string attName = ".html";
                    if (this.Annex == "附件")
                    {
                        attName = base.AttName;
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
                    filePath = filePath + @"\" + DateTime.Now.Date.ToString("yyy-MM-dd");
                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }
                    filePath = filePath + @"\" + base.BoxName;
                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }
                    filePath = string.Concat(new object[] { filePath, @"\", base.m_emailno, "-", attName });
                    stream = File.Open(filePath, FileMode.Append, FileAccess.Write);
                    stream.Write(buffer, 0, nbytes);
                    stream.Close();
                }
                catch (Exception)
                {
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

        public string UrnHtml(string strHtml)
        {
            string[] strArray = new string[] { "'", "<", ">", ",", ".", ";", "/", "|", "*", "\"", @"\", "?", ":" };
            for (int i = 0; i < strArray.Length; i++)
            {
                strHtml = strHtml.Replace(strArray[i], string.Empty);
            }
            return strHtml;
        }

        // Nested Types
        [StructLayout(LayoutKind.Sequential)]
        private struct AnnexUrlName
        {
            public string annexname;
            public string annexurl;
        }

    }
}
