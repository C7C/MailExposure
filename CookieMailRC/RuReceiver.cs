using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Globalization;
using System.Runtime.InteropServices;
using System.IO;
using System.Threading;

namespace MailExposure.CookieMailRC
{
    class RuReceiver:MailStr
    {
        // Fields
        private string Annex;
        private BoxNameId[] boxList;
        private CookieContainer cookieContainer;
        public string m_emailTime;
        private static object pob;
        private string url;
        private string urls;

        // Methods
        static RuReceiver()
        {
            pob = new object();

        }
        public RuReceiver()
        {
            this.Annex = "";
            this.urls = "";
            this.url = "";
            this.boxList = new BoxNameId[200];
        }
        private void downMail(string strBoxPages, string folder, int page)
        {
            int startIndex = 0;
            bool flag = false;
            string str4 = "";
            while ((startIndex = strBoxPages.IndexOf("class=letavtor title", startIndex)) > 0)
            {
                try
                {
                    string url = base.putstr(strBoxPages, "<a href=\"", "\"", startIndex);
                    if (url == "-1")
                    {
                        startIndex++;
                        continue;
                    }
                    if (base.putstr(strBoxPages, "class=dat title", "</td>", startIndex).IndexOf("<b>") > 0)
                    {
                        flag = true;
                    }
                    string str = url.Substring(url.IndexOf("id=") + 3, url.Length - (url.IndexOf("id=") + 3));
                    string strSql = "select count(*) from RumailId where MsgId='" + str + "'";
                    if (Convert.ToInt32(GlobalValue.mainForm.ExecuteSQL(strSql)) != 0)
                    {
                        goto Label_091D;
                    }
                    base.putstr(strBoxPages, "title=\"", "\"", startIndex);
                    int index = strBoxPages.IndexOf("<td class=lettem>", startIndex);
                    if (index != -1)
                    {
                        str4 = base.putstr(strBoxPages, "\">", "</a>", index);
                    }
                    string str5 = base.putstr(strBoxPages, "class=dat title=\"", "\"", startIndex);
                    bool flag2 = false;
                    try
                    {
                        DateTime time = new DateTime();
                        str5 = str5.ToString(DateTimeFormatInfo.InvariantInfo);
                        string[] strArray = str5.Split(new char[] { ' ' });
                        if (strArray[2].IndexOf("Янв") != -1)
                        {
                            str5 = (strArray[3] + "-" + strArray[2] + "-" + strArray[1] + " " + strArray[4]).Replace(strArray[2], "1");
                        }
                        else if (strArray[2].IndexOf("Фев") != -1)
                        {
                            str5 = (strArray[3] + "-" + strArray[2] + "-" + strArray[1] + " " + strArray[4]).Replace(strArray[2], "2");
                        }
                        else if (strArray[2].IndexOf("Мар") != -1)
                        {
                            str5 = (strArray[3] + "-" + strArray[2] + "-" + strArray[1] + " " + strArray[4]).Replace(strArray[2], "3");
                        }
                        else if (strArray[2].IndexOf("Апр") != -1)
                        {
                            str5 = (strArray[3] + "-" + strArray[2] + "-" + strArray[1] + " " + strArray[4]).Replace(strArray[2], "4");
                        }
                        else if (strArray[2].IndexOf("Май") != -1)
                        {
                            str5 = (strArray[3] + "-" + strArray[2] + "-" + strArray[1] + " " + strArray[4]).Replace(strArray[2], "5");
                        }
                        else if (strArray[2].IndexOf("Мая") != -1)
                        {
                            str5 = (strArray[3] + "-" + strArray[2] + "-" + strArray[1] + " " + strArray[4]).Replace(strArray[2], "5");
                        }
                        else if (strArray[2].IndexOf("Июн") != -1)
                        {
                            str5 = (strArray[3] + "-" + strArray[2] + "-" + strArray[1] + " " + strArray[4]).Replace(strArray[2], "6");
                        }
                        else if (strArray[2].IndexOf("Июл") != -1)
                        {
                            str5 = (strArray[3] + "-" + strArray[2] + "-" + strArray[1] + " " + strArray[4]).Replace(strArray[2], "7");
                        }
                        else if (strArray[2].IndexOf("Авг") != -1)
                        {
                            str5 = (strArray[3] + "-" + strArray[2] + "-" + strArray[1] + " " + strArray[4]).Replace(strArray[2], "8");
                        }
                        else if (strArray[2].IndexOf("Сен") != -1)
                        {
                            str5 = (strArray[3] + "-" + strArray[2] + "-" + strArray[1] + " " + strArray[4]).Replace(strArray[2], "9");
                        }
                        else if (strArray[2].IndexOf("Окт") != -1)
                        {
                            str5 = (strArray[3] + "-" + strArray[2] + "-" + strArray[1] + " " + strArray[4]).Replace(strArray[2], "10");
                        }
                        else if (strArray[2].IndexOf("Ноя") != -1)
                        {
                            str5 = (strArray[3] + "-" + strArray[2] + "-" + strArray[1] + " " + strArray[4]).Replace(strArray[2], "11");
                        }
                        else if (strArray[2].IndexOf("Дек") != -1)
                        {
                            str5 = (strArray[3] + "-" + strArray[2] + "-" + strArray[1] + " " + strArray[4]).Replace(strArray[2], "12");
                        }
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
                    if (!flag2)
                    {
                        goto Label_091D;
                    }
                    url = base.Host + "getmsg?id=" + str;
                    base.streamControl = true;
                    base.ShowMessage(base.BoxName + "  " + str4 + "\t 开始下载");
                    this.RequestEmail(url);
                    if (flag)
                    {
                        try
                        {
                            string indata = string.Concat(new object[] { "confirm=on&page=", page, "&back=msglist%3Ffolder%3D0%26page%3D0%26sortby%3D&folder=", folder, "&markmessage=1&mark=+%EE%EA+&id=", str, "&moveto=abook" });
                            base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                            base.streamControl = true;
                            base.MyStringBuilder = this.PostData("http://win.mail.ru/cgi-bin/movemsg", indata, this.cookieContainer);
                            base.ShowMessage("置未读：" + str4);
                            flag = false;
                            goto Label_085B;
                        }
                        catch (Exception exception)
                        {
                            base.ShowMessage("置未读失败：" + str4 + "\t" + exception.Message);
                            startIndex++;
                            continue;
                        }
                    }
                    base.ShowMessage("已读：" + str4);
                Label_085B:
                    try
                    {
                        string str8 = DateTime.Now.ToString();
                        strSql = "insert into RumailId(Name,MsgId,DownTime,MailType) values('" + base.m_username + "','" + str + "','" + str8 + "','" + base.BoxName.Substring(0, base.BoxName.Length % 10) + "')";
                        GlobalValue.mainForm.ExecuteSQL(strSql);
                    }
                    catch (Exception exception2)
                    {
                        base.ShowMessage("添加失败：" + exception2.Message);
                    }
                }
                catch (Exception)
                {
                    base.ShowMessage("邮件下载失败");
                }
            Label_091D:
                startIndex++;
            }

        }
        private void downMailAnnex(string url)
        {
            try
            {
                int num = 0;
                base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                base.streamControl = true;
                base.MyStringBuilder = this.Request(url);
                int startIndex = 0;
                while ((startIndex = base.MyStringBuilder.ToString().IndexOf("Прикрепленные данные:", startIndex)) > 0)
                {
                    base.AttId = base.putstr(base.MyStringBuilder.ToString(), "<a href=\"", "\"", startIndex);
                    string str = base.putstr(base.MyStringBuilder.ToString(), "<b>", "</b>", startIndex);
                    if ((base.AttId == "-1") || (base.AttName == "-1"))
                    {
                        startIndex++;
                    }
                    else
                    {
                        num++;
                        this.Annex = "附件";
                        base.AttName = string.Concat(new object[] { "附件-", num, "_", str });
                        base.streamControl = true;
                        this.RequestEmail(base.AttId + "&notype");
                        this.Annex = "";
                        startIndex++;
                    }
                }
            }
            catch (Exception exception)
            {
                this.Annex = "";
                base.ShowMessage("附件下载失败：" + exception.Message);
            }

        }
        private void getAddressbook()
        {
            string url = "http://win.mail.ru/cgi-bin/abexport/addressbook.csv";
            string indata = "confirm=1&abtype=6&export=%DD%EA%F1%EF%EE%F0%F2%E8%F0%EE%E2%E0%F2%FC";
            base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
            base.streamControl = true;
            base.MyStringBuilder = this.PostData(url, indata, this.cookieContainer);
            this.SaveAddressbook(base.MyStringBuilder.ToString());
        }
        private void getBoxName(string message)
        {
            this.getAddressbook();
            this.SaveAddressbook(message, "");
            try
            {
                int index = 0;
                for (int i = message.IndexOf("Название"); (i = message.IndexOf("<tr>", i)) > 0; i++)
                {
                    string str = base.putstr(message, "<td><a href=\"", "\">", i);
                    string str2 = base.putstr(message, str + "\"><b>", "</b></a></td>", i);
                    if (str2 == "-1")
                    {
                        str2 = base.putstr(message, "\">", "</a>", i);
                    }
                    if (((str != "-1") && (str2 != "-1")) && ((str != null) && (str2 != null)))
                    {
                        this.boxList[index].boxid = str;
                        this.boxList[index].boxname = str2;
                        index++;
                    }
                }
                for (int j = 1; j < index; j++)
                {
                    this.GetMailId(this.boxList[j].boxid, this.boxList[j].boxname);
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
            string url = "";
            string str6 = "";
            url = base.Host + boxid;
            try
            {
                string str2;
                string str3;
                string str5;
                base.BoxName = boxname;
                base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                base.streamControl = true;
                base.MyStringBuilder = this.Request(url);
                string message = base.MyStringBuilder.ToString();
                startIndex = message.IndexOf("<tbody>");
                if (startIndex >= 0)
                {
                    goto Label_10F0;
                }
                startIndex = message.IndexOf("id=\"id-messages-list\"");
                if (startIndex < 0)
                {
                    startIndex = 0;
                }
                while ((startIndex = message.IndexOf("<div class=\"msgLine", startIndex)) > 0)
                {
                    try
                    {
                        str2 = base.putstr(message, "id=\"str", "\"", startIndex);
                        if ((str2 != "-1") && (str2 != ""))
                        {
                            string strSql = "select count(*) from RumailId where MsgId='" + str2 + "'";
                            if (Convert.ToInt32(GlobalValue.mainForm.ExecuteSQL(strSql)) == 0)
                            {
                                str5 = base.putstr(message, "msg-D\" title=\"", "\">", startIndex);
                                string str10 = base.putstr(message, "msgList msg-F", "</span>", startIndex);
                                base.putstr(str10, "title=\"", "\">", 0);
                                string str11 = base.putstr(message, "msgList msg-S", "</span>", startIndex);
                                str3 = base.putstr(str11, "title=\"", "\"", 0);
                                bool flag = false;
                                try
                                {
                                    DateTime time = new DateTime();
                                    str5 = str5.ToString(DateTimeFormatInfo.InvariantInfo);
                                    string[] strArray = str5.Split(new char[] { ' ' });
                                    if (strArray[2].IndexOf("Янв") != -1)
                                    {
                                        str5 = (strArray[3] + "-" + strArray[2] + "-" + strArray[1] + " " + strArray[4]).Replace(strArray[2], "1");
                                    }
                                    else if (strArray[2].IndexOf("Фев") != -1)
                                    {
                                        str5 = (strArray[3] + "-" + strArray[2] + "-" + strArray[1] + " " + strArray[4]).Replace(strArray[2], "2");
                                    }
                                    else if (strArray[2].IndexOf("Мар") != -1)
                                    {
                                        str5 = (strArray[3] + "-" + strArray[2] + "-" + strArray[1] + " " + strArray[4]).Replace(strArray[2], "3");
                                    }
                                    else if (strArray[2].IndexOf("Апр") != -1)
                                    {
                                        str5 = (strArray[3] + "-" + strArray[2] + "-" + strArray[1] + " " + strArray[4]).Replace(strArray[2], "4");
                                    }
                                    else if (strArray[2].IndexOf("Май") != -1)
                                    {
                                        str5 = (strArray[3] + "-" + strArray[2] + "-" + strArray[1] + " " + strArray[4]).Replace(strArray[2], "5");
                                    }
                                    else if (strArray[2].IndexOf("Мая") != -1)
                                    {
                                        str5 = (strArray[3] + "-" + strArray[2] + "-" + strArray[1] + " " + strArray[4]).Replace(strArray[2], "5");
                                    }
                                    else if (strArray[2].IndexOf("Июн") != -1)
                                    {
                                        str5 = (strArray[3] + "-" + strArray[2] + "-" + strArray[1] + " " + strArray[4]).Replace(strArray[2], "6");
                                    }
                                    else if (strArray[2].IndexOf("Июл") != -1)
                                    {
                                        str5 = (strArray[3] + "-" + strArray[2] + "-" + strArray[1] + " " + strArray[4]).Replace(strArray[2], "7");
                                    }
                                    else if (strArray[2].IndexOf("Авг") != -1)
                                    {
                                        str5 = (strArray[3] + "-" + strArray[2] + "-" + strArray[1] + " " + strArray[4]).Replace(strArray[2], "8");
                                    }
                                    else if (strArray[2].IndexOf("Сен") != -1)
                                    {
                                        str5 = (strArray[3] + "-" + strArray[2] + "-" + strArray[1] + " " + strArray[4]).Replace(strArray[2], "9");
                                    }
                                    else if (strArray[2].IndexOf("Окт") != -1)
                                    {
                                        str5 = (strArray[3] + "-" + strArray[2] + "-" + strArray[1] + " " + strArray[4]).Replace(strArray[2], "10");
                                    }
                                    else if (strArray[2].IndexOf("Ноя") != -1)
                                    {
                                        str5 = (strArray[3] + "-" + strArray[2] + "-" + strArray[1] + " " + strArray[4]).Replace(strArray[2], "11");
                                    }
                                    else if (strArray[2].IndexOf("Дек") != -1)
                                    {
                                        str5 = (strArray[3] + "-" + strArray[2] + "-" + strArray[1] + " " + strArray[4]).Replace(strArray[2], "12");
                                    }
                                    time = Convert.ToDateTime(str5);
                                    if (DateTime.Compare(time, GlobalValue.mainForm.EmailDateTime) >= 0)
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
                                    str6 = base.Host + "getmsg?id=" + str2;
                                    base.streamControl = true;
                                    base.ShowMessage(base.BoxName + "  " + str3 + "\t 开始下载");
                                    if (this.RequestEmail(str6))
                                    {
                                        try
                                        {
                                            string str12 = DateTime.Now.ToString();
                                            strSql = "insert into RumailId(Name,MsgId,DownTime,MailType) values('" + base.m_username + "','" + str2 + "','" + str12 + "','" + base.BoxName.Substring(0, base.BoxName.Length % 10) + "')";
                                            GlobalValue.mainForm.ExecuteSQL(strSql);
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
                    startIndex++;
                }
                goto Label_1105;
            Label_08C4:
                try
                {
                    string str4 = base.putstr(message, "<tr", "</tr>", startIndex);
                    str2 = base.putstr(str4, "value=\"", "\"", 0);
                    if ((str2 != "-1") && (str2 != ""))
                    {
                        string str13 = "select count(*) from RumailId where MsgId='" + str2 + "'";
                        if (Convert.ToInt32(GlobalValue.mainForm.ExecuteSQL(str13)) == 0)
                        {
                            base.putstr(str4, "letavtor title=\"", "\"", 0);
                            str3 = base.putstr(str4, "class=lettem", "</td>", 0);
                            base.putstr(str3, "<a href=\"", "\">", 0);
                            str3 = base.putstr(str3, "\">", "</a>", 0);
                            str5 = base.putstr(str4, "dat title=\"", "\">", 0);
                            bool flag2 = false;
                            try
                            {
                                DateTime time2 = new DateTime();
                                str5 = str5.ToString(DateTimeFormatInfo.InvariantInfo);
                                string[] strArray2 = str5.Split(new char[] { ' ' });
                                if (strArray2[2].IndexOf("Янв") != -1)
                                {
                                    str5 = (strArray2[3] + "-" + strArray2[2] + "-" + strArray2[1] + " " + strArray2[4]).Replace(strArray2[2], "1");
                                }
                                else if (strArray2[2].IndexOf("Фев") != -1)
                                {
                                    str5 = (strArray2[3] + "-" + strArray2[2] + "-" + strArray2[1] + " " + strArray2[4]).Replace(strArray2[2], "2");
                                }
                                else if (strArray2[2].IndexOf("Мар") != -1)
                                {
                                    str5 = (strArray2[3] + "-" + strArray2[2] + "-" + strArray2[1] + " " + strArray2[4]).Replace(strArray2[2], "3");
                                }
                                else if (strArray2[2].IndexOf("Апр") != -1)
                                {
                                    str5 = (strArray2[3] + "-" + strArray2[2] + "-" + strArray2[1] + " " + strArray2[4]).Replace(strArray2[2], "4");
                                }
                                else if (strArray2[2].IndexOf("Май") != -1)
                                {
                                    str5 = (strArray2[3] + "-" + strArray2[2] + "-" + strArray2[1] + " " + strArray2[4]).Replace(strArray2[2], "5");
                                }
                                else if (strArray2[2].IndexOf("Мая") != -1)
                                {
                                    str5 = (strArray2[3] + "-" + strArray2[2] + "-" + strArray2[1] + " " + strArray2[4]).Replace(strArray2[2], "5");
                                }
                                else if (strArray2[2].IndexOf("Июн") != -1)
                                {
                                    str5 = (strArray2[3] + "-" + strArray2[2] + "-" + strArray2[1] + " " + strArray2[4]).Replace(strArray2[2], "6");
                                }
                                else if (strArray2[2].IndexOf("Июл") != -1)
                                {
                                    str5 = (strArray2[3] + "-" + strArray2[2] + "-" + strArray2[1] + " " + strArray2[4]).Replace(strArray2[2], "7");
                                }
                                else if (strArray2[2].IndexOf("Авг") != -1)
                                {
                                    str5 = (strArray2[3] + "-" + strArray2[2] + "-" + strArray2[1] + " " + strArray2[4]).Replace(strArray2[2], "8");
                                }
                                else if (strArray2[2].IndexOf("Сен") != -1)
                                {
                                    str5 = (strArray2[3] + "-" + strArray2[2] + "-" + strArray2[1] + " " + strArray2[4]).Replace(strArray2[2], "9");
                                }
                                else if (strArray2[2].IndexOf("Окт") != -1)
                                {
                                    str5 = (strArray2[3] + "-" + strArray2[2] + "-" + strArray2[1] + " " + strArray2[4]).Replace(strArray2[2], "10");
                                }
                                else if (strArray2[2].IndexOf("Ноя") != -1)
                                {
                                    str5 = (strArray2[3] + "-" + strArray2[2] + "-" + strArray2[1] + " " + strArray2[4]).Replace(strArray2[2], "11");
                                }
                                else if (strArray2[2].IndexOf("Дек") != -1)
                                {
                                    str5 = (strArray2[3] + "-" + strArray2[2] + "-" + strArray2[1] + " " + strArray2[4]).Replace(strArray2[2], "12");
                                }
                                time2 = Convert.ToDateTime(str5);
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
                                str6 = base.Host + "getmsg?id=" + str2;
                                base.streamControl = true;
                                base.ShowMessage(base.BoxName + "  " + str3 + "\t 开始下载");
                                this.RequestEmail(str6);
                                try
                                {
                                    string str14 = DateTime.Now.ToString();
                                    str13 = "insert into RumailId(Name,MsgId,DownTime,MailType) values('" + base.m_username + "','" + str2 + "','" + str14 + "','" + base.BoxName.Substring(0, base.BoxName.Length % 10) + "')";
                                    GlobalValue.mainForm.ExecuteSQL(str13);
                                }
                                catch (Exception exception3)
                                {
                                    base.ShowMessage("添加失败：" + exception3.Message);
                                }
                            }
                        }
                    }
                }
                catch (Exception exception4)
                {
                    base.ShowMessage("取邮件失败…………" + exception4.Message);
                }
                startIndex++;
            Label_10F0:
                if ((startIndex = message.IndexOf("<tr", startIndex)) > 0)
                {
                    goto Label_08C4;
                }
            Label_1105:
                if (message.IndexOf("<b class=odin>") > 0)
                {
                    int num4 = 0;
                    string str15 = base.putstr(message, "<b class=odin>", "</td>", 0);
                    while ((num4 = str15.IndexOf("<a href=", num4)) > 0)
                    {
                        string str16 = base.putstr(str15, "<a href=", "</a>", num4);
                        if (str16.IndexOf("nextbut") > 0)
                        {
                            string str8 = base.putstr(str16, "\"", "\"", 0);
                            this.GetNextPageMailId(str8, boxname);
                        }
                        num4++;
                    }
                }
                int index = 0;
                index = message.IndexOf("<a class=\"dIB mr10\"");
                if (index > 0)
                {
                    string str17 = base.putstr(message, "href=\"", "\"", index);
                    if ((str17 != "") && (str17 != "-1"))
                    {
                        this.GetNextPageMailId(str17, boxname);
                    }
                    else
                    {
                        base.ShowMessage(base.BoxName + "取分页失败。");
                    }
                }
            }
            catch (Exception exception5)
            {
                base.ShowMessage("取邮件失败…………" + exception5.Message);
            }

        }
        private void GetNextPageMailId(string boxid, string boxname)
        {
            string str2;
            string str3;
            string str5;
            int startIndex = 0;
            string url = "";
            string str6 = "";
            url = base.Host + boxid;
            base.BoxName = boxname;
            base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
            base.streamControl = true;
            base.MyStringBuilder = this.Request(url);
            string message = base.MyStringBuilder.ToString();
            startIndex = message.IndexOf("<tbody>");
            if (startIndex >= 0)
            {
                while ((startIndex = message.IndexOf("<tr", startIndex)) > 0)
                {
                    try
                    {
                        string str4 = base.putstr(message, "<tr", "</tr>", startIndex);
                        str2 = base.putstr(str4, "value=\"", "\"", 0);
                        if ((str2 != "-1") && (str2 != ""))
                        {
                            string strSql = "select count(*) from RumailId where MsgId='" + str2 + "'";
                            if (Convert.ToInt32(GlobalValue.mainForm.ExecuteSQL(strSql)) == 0)
                            {
                                base.putstr(str4, "letavtor title=\"", "\"", 0);
                                str3 = base.putstr(str4, "class=lettem", "</td>", 0);
                                base.putstr(str3, "<a href=\"", "\">", 0);
                                str3 = base.putstr(str3, "\">", "</a>", 0);
                                str5 = base.putstr(str4, "dat title=\"", "\">", 0);
                                bool flag2 = false;
                                try
                                {
                                    DateTime time2 = new DateTime();
                                    str5 = str5.ToString(DateTimeFormatInfo.InvariantInfo);
                                    string[] strArray2 = str5.Split(new char[] { ' ' });
                                    if (strArray2[2].IndexOf("Янв") != -1)
                                    {
                                        str5 = (strArray2[3] + "-" + strArray2[2] + "-" + strArray2[1] + " " + strArray2[4]).Replace(strArray2[2], "1");
                                    }
                                    else if (strArray2[2].IndexOf("Фев") != -1)
                                    {
                                        str5 = (strArray2[3] + "-" + strArray2[2] + "-" + strArray2[1] + " " + strArray2[4]).Replace(strArray2[2], "2");
                                    }
                                    else if (strArray2[2].IndexOf("Мар") != -1)
                                    {
                                        str5 = (strArray2[3] + "-" + strArray2[2] + "-" + strArray2[1] + " " + strArray2[4]).Replace(strArray2[2], "3");
                                    }
                                    else if (strArray2[2].IndexOf("Апр") != -1)
                                    {
                                        str5 = (strArray2[3] + "-" + strArray2[2] + "-" + strArray2[1] + " " + strArray2[4]).Replace(strArray2[2], "4");
                                    }
                                    else if (strArray2[2].IndexOf("Май") != -1)
                                    {
                                        str5 = (strArray2[3] + "-" + strArray2[2] + "-" + strArray2[1] + " " + strArray2[4]).Replace(strArray2[2], "5");
                                    }
                                    else if (strArray2[2].IndexOf("Мая") != -1)
                                    {
                                        str5 = (strArray2[3] + "-" + strArray2[2] + "-" + strArray2[1] + " " + strArray2[4]).Replace(strArray2[2], "5");
                                    }
                                    else if (strArray2[2].IndexOf("Июн") != -1)
                                    {
                                        str5 = (strArray2[3] + "-" + strArray2[2] + "-" + strArray2[1] + " " + strArray2[4]).Replace(strArray2[2], "6");
                                    }
                                    else if (strArray2[2].IndexOf("Июл") != -1)
                                    {
                                        str5 = (strArray2[3] + "-" + strArray2[2] + "-" + strArray2[1] + " " + strArray2[4]).Replace(strArray2[2], "7");
                                    }
                                    else if (strArray2[2].IndexOf("Авг") != -1)
                                    {
                                        str5 = (strArray2[3] + "-" + strArray2[2] + "-" + strArray2[1] + " " + strArray2[4]).Replace(strArray2[2], "8");
                                    }
                                    else if (strArray2[2].IndexOf("Сен") != -1)
                                    {
                                        str5 = (strArray2[3] + "-" + strArray2[2] + "-" + strArray2[1] + " " + strArray2[4]).Replace(strArray2[2], "9");
                                    }
                                    else if (strArray2[2].IndexOf("Окт") != -1)
                                    {
                                        str5 = (strArray2[3] + "-" + strArray2[2] + "-" + strArray2[1] + " " + strArray2[4]).Replace(strArray2[2], "10");
                                    }
                                    else if (strArray2[2].IndexOf("Ноя") != -1)
                                    {
                                        str5 = (strArray2[3] + "-" + strArray2[2] + "-" + strArray2[1] + " " + strArray2[4]).Replace(strArray2[2], "11");
                                    }
                                    else if (strArray2[2].IndexOf("Дек") != -1)
                                    {
                                        str5 = (strArray2[3] + "-" + strArray2[2] + "-" + strArray2[1] + " " + strArray2[4]).Replace(strArray2[2], "12");
                                    }
                                    time2 = Convert.ToDateTime(str5);
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
                                    str6 = base.Host + "getmsg?id=" + str2;
                                    base.streamControl = true;
                                    base.ShowMessage(base.BoxName + "  " + str3 + "\t 开始下载");
                                    this.RequestEmail(str6);
                                    try
                                    {
                                        string str14 = DateTime.Now.ToString();
                                        strSql = "insert into RumailId(Name,MsgId,DownTime,MailType) values('" + base.m_username + "','" + str2 + "','" + str14 + "','" + base.BoxName.Substring(0, base.BoxName.Length % 10) + "')";
                                        GlobalValue.mainForm.ExecuteSQL(strSql);
                                    }
                                    catch (Exception exception3)
                                    {
                                        base.ShowMessage("添加失败：" + exception3.Message);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception)
                    {
                    }
                    startIndex++;
                }
            }
            else
            {
                for (startIndex = message.IndexOf("id=\"id-messages-list\""); (startIndex = message.IndexOf("<div class=\"msgLine", startIndex)) > 0; startIndex++)
                {
                    try
                    {
                        str2 = base.putstr(message, "id=\"str", "\"", startIndex);
                        if ((str2 != "-1") && (str2 != ""))
                        {
                            string str9 = "select count(*) from RumailId where MsgId='" + str2 + "'";
                            if (Convert.ToInt32(GlobalValue.mainForm.ExecuteSQL(str9)) == 0)
                            {
                                str5 = base.putstr(message, "msg-D\" title=\"", "\">", startIndex);
                                string str10 = base.putstr(message, "msgList msg-F", "</span>", startIndex);
                                base.putstr(str10, "title=\"", "\">", 0);
                                string str11 = base.putstr(message, "msgList msg-S", "</span>", startIndex);
                                str3 = base.putstr(str11, "title=\"", "\"", 0);
                                bool flag = false;
                                try
                                {
                                    DateTime time = new DateTime();
                                    str5 = str5.ToString(DateTimeFormatInfo.InvariantInfo);
                                    string[] strArray = str5.Split(new char[] { ' ' });
                                    if (strArray[2].IndexOf("Янв") != -1)
                                    {
                                        str5 = (strArray[3] + "-" + strArray[2] + "-" + strArray[1] + " " + strArray[4]).Replace(strArray[2], "1");
                                    }
                                    else if (strArray[2].IndexOf("Фев") != -1)
                                    {
                                        str5 = (strArray[3] + "-" + strArray[2] + "-" + strArray[1] + " " + strArray[4]).Replace(strArray[2], "2");
                                    }
                                    else if (strArray[2].IndexOf("Мар") != -1)
                                    {
                                        str5 = (strArray[3] + "-" + strArray[2] + "-" + strArray[1] + " " + strArray[4]).Replace(strArray[2], "3");
                                    }
                                    else if (strArray[2].IndexOf("Апр") != -1)
                                    {
                                        str5 = (strArray[3] + "-" + strArray[2] + "-" + strArray[1] + " " + strArray[4]).Replace(strArray[2], "4");
                                    }
                                    else if (strArray[2].IndexOf("Май") != -1)
                                    {
                                        str5 = (strArray[3] + "-" + strArray[2] + "-" + strArray[1] + " " + strArray[4]).Replace(strArray[2], "5");
                                    }
                                    else if (strArray[2].IndexOf("Мая") != -1)
                                    {
                                        str5 = (strArray[3] + "-" + strArray[2] + "-" + strArray[1] + " " + strArray[4]).Replace(strArray[2], "5");
                                    }
                                    else if (strArray[2].IndexOf("Июн") != -1)
                                    {
                                        str5 = (strArray[3] + "-" + strArray[2] + "-" + strArray[1] + " " + strArray[4]).Replace(strArray[2], "6");
                                    }
                                    else if (strArray[2].IndexOf("Июл") != -1)
                                    {
                                        str5 = (strArray[3] + "-" + strArray[2] + "-" + strArray[1] + " " + strArray[4]).Replace(strArray[2], "7");
                                    }
                                    else if (strArray[2].IndexOf("Авг") != -1)
                                    {
                                        str5 = (strArray[3] + "-" + strArray[2] + "-" + strArray[1] + " " + strArray[4]).Replace(strArray[2], "8");
                                    }
                                    else if (strArray[2].IndexOf("Сен") != -1)
                                    {
                                        str5 = (strArray[3] + "-" + strArray[2] + "-" + strArray[1] + " " + strArray[4]).Replace(strArray[2], "9");
                                    }
                                    else if (strArray[2].IndexOf("Окт") != -1)
                                    {
                                        str5 = (strArray[3] + "-" + strArray[2] + "-" + strArray[1] + " " + strArray[4]).Replace(strArray[2], "10");
                                    }
                                    else if (strArray[2].IndexOf("Ноя") != -1)
                                    {
                                        str5 = (strArray[3] + "-" + strArray[2] + "-" + strArray[1] + " " + strArray[4]).Replace(strArray[2], "11");
                                    }
                                    else if (strArray[2].IndexOf("Дек") != -1)
                                    {
                                        str5 = (strArray[3] + "-" + strArray[2] + "-" + strArray[1] + " " + strArray[4]).Replace(strArray[2], "12");
                                    }
                                    time = Convert.ToDateTime(str5);
                                    if (DateTime.Compare(time, GlobalValue.mainForm.EmailDateTime) >= 0)
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
                                    str6 = base.Host + "getmsg?id=" + str2;
                                    base.streamControl = true;
                                    base.ShowMessage(base.BoxName + "  " + str3 + "\t 开始下载");
                                    this.RequestEmail(str6);
                                    try
                                    {
                                        string str12 = DateTime.Now.ToString();
                                        str9 = "insert into RumailId(Name,MsgId,DownTime,MailType) values('" + base.m_username + "','" + str2 + "','" + str12 + "','" + base.BoxName.Substring(0, base.BoxName.Length % 10) + "')";
                                        GlobalValue.mainForm.ExecuteSQL(str9);
                                    }
                                    catch (Exception exception)
                                    {
                                        base.ShowMessage("添加失败：" + exception.Message);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception exception2)
                    {
                        base.ShowMessage("邮件下载失败" + exception2.StackTrace);
                    }
                }
            }
            if (message.IndexOf("<b class=odin>") > 0)
            {
                int num4 = 0;
                string str15 = base.putstr(message, "<b class=odin>", "</td>", 0);
                while ((num4 = str15.IndexOf("<a href=", num4)) > 0)
                {
                    string str16 = base.putstr(str15, "<a href=", "</a>", num4);
                    if (str16.IndexOf("nextbut") > 0)
                    {
                        string str8 = base.putstr(str16, "\"", "\"", 0);
                        this.GetNextPageMailId(str8, boxname);
                    }
                    num4++;
                }
            }
            int index = 0;
            index = message.LastIndexOf("Назад");
            index = message.IndexOf("<a class=\"dIB mr10\"", (int)(index + 1));
            if (index > 0)
            {
                string str17 = base.putstr(message, "href=\"", "\"", index);
                base.putstr(message, "href=\"", "\"", index);
                if ((str17 != "") && (str17 != "-1"))
                {
                    this.GetNextPageMailId(str17, boxname);
                }
                else
                {
                    base.ShowMessage(base.BoxName + "取分页失败。");
                }
            }

        }
        private void getPages(string strBoxPages, string folder)
        {
            int page = 0;
            int index = 0;
            try
            {
                base.ShowMessage(string.Concat(new object[] { base.BoxName, "：第", page, 1, "页开始下载" }));
                this.downMail(strBoxPages, folder, page);
                base.ShowMessage(string.Concat(new object[] { base.BoxName, "：第", page, 1, "页下载完毕" }));
                int num3 = strBoxPages.IndexOf("class=odin");
                if (num3 > 0)
                {
                    index = 0;
                    int startIndex = num3;
                    int[] numArray = new int[100];
                    while ((startIndex = strBoxPages.IndexOf("<a href=\"", startIndex)) > 0)
                    {
                        string message = base.putstr(strBoxPages, "<a href=\"", "\"", startIndex);
                        string str2 = base.putstr(message, "&page=", "&sortby", 0);
                        if ((message == "-1") || (str2 == "-1"))
                        {
                            startIndex++;
                        }
                        else
                        {
                            page = Convert.ToInt32(str2);
                            if (page == 0)
                            {
                                startIndex++;
                            }
                            else
                            {
                                index++;
                                numArray[index] = page;
                                if (numArray[index] > numArray[index - 1])
                                {
                                    base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                                    base.streamControl = true;
                                    base.MyStringBuilder = this.Request(base.Host + message);
                                    base.ShowMessage(string.Concat(new object[] { base.BoxName, "：第", page, "页开始下载" }));
                                    this.downMail(base.MyStringBuilder.ToString(), folder, page);
                                    base.ShowMessage(string.Concat(new object[] { base.BoxName, "：第", page, "页下载完毕" }));
                                    startIndex++;
                                }
                                startIndex++;
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                base.ShowMessage("取页数失败…………" + exception.Message);
            }

        }
        public override void login()
        {
            if (base.m_UserType.IndexOf("无密用户") != -1)
            {
                string str = "";
                this.cookieContainer = new CookieContainer();
                base.ShowMessage("开始登陆…………");
                this.url = base.emailuri;
                str = base.putstr(base.validationLogin, "=", ";", 0);
                base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                base.streamControl = true;
                Cookie cookie = new Cookie("Mpop", str);
                cookie.Domain = ".mail.ru";
                this.cookieContainer.Add(cookie);
                base.MyStringBuilder = this.Request(this.url);
                if (base.MyStringBuilder.ToString() != "")
                {
                    int startIndex = 0;
                    startIndex = base.MyStringBuilder.ToString().IndexOf("folders");
                    if (startIndex < 0)
                    {
                        base.LoginFail();
                        base.passwdErr();
                        base.ShowMessage("登陆失败…………");
                    }
                    else
                    {
                        base.Host = "http://win.mail.ru/cgi-bin/";
                        string str2 = base.putstr(base.MyStringBuilder.ToString(), "folders", "\"", startIndex);
                        base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                        base.streamControl = true;
                        base.MyStringBuilder = this.Request(base.Host + "folders" + str2);
                        if (base.MyStringBuilder.ToString().IndexOf("Написать&nbsp;письмо") < 0)
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
                base.m_passwd = base.strPassParse(base.m_passwd);
                try
                {
                    base.ShowMessage("开始登陆…………");
                    string url = "http://win.mail.ru/cgi-bin/auth";
                    string indata = "Login=" + base.m_username + "&Domain=mail.ru&Password=" + base.m_passwd;
                    this.cookieContainer = new CookieContainer();
                    base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                    base.streamControl = true;
                    base.MyStringBuilder = this.PostData(url, indata, this.cookieContainer);
                    int index = base.MyStringBuilder.ToString().IndexOf("http://img.mail.ru/mail/ru/images/start_inbox.gif");
                    if (index < 0)
                    {
                        base.LoginFail();
                        base.passwdErr();
                        base.ShowMessage("登陆失败…………");
                    }
                    else
                    {
                        base.Host = "http://win.mail.ru/cgi-bin/";
                        string str5 = base.putstr(base.MyStringBuilder.ToString(), "href=\"", "\"", index);
                        base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                        base.streamControl = true;
                        base.MyStringBuilder = this.Request(base.Host + str5);
                        if (base.MyStringBuilder.ToString().IndexOf("class=mark") < 0)
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
                }
                catch (Exception)
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
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = indata.Length;
                request.Method = "POST";
                request.AllowAutoRedirect = false;
                request.ServicePoint.Expect100Continue = false;
                request.Referer = "http://www.mail.ru";
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
                response.Cookies = this.cookieContainer.GetCookies(request.RequestUri);
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
                    byte[] buffer = new byte[0x1000];
                    base.m_emailno++;
                    this.m_emailTime = DateTime.Now.ToString("yyyMMdd-HHmmss") + DateTime.Now.Millisecond.ToString();
                    while ((num = responseStream.Read(buffer, 0, 0x1000)) > 0)
                    {
                        if (!this.SaveRu(buffer, num))
                        {
                            return false;
                        }
                    }
                    this.saveRuEmailCount();
                    GlobalValue.mainForm.statusBarPanelEmailNumbersText.Text = (Convert.ToInt32(GlobalValue.mainForm.statusBarPanelEmailNumbersText.Text) + 1).ToString();
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
            if ((GlobalValue.mainForm.saveFilePath != "") | (GlobalValue.mainForm.saveFilePath != null))
            {
                path = GlobalValue.mainForm.saveFilePath;
            }
            if (GlobalValue.mainForm.checkDataSave)
            {
                string str2 = DateTime.Now.Date.ToString("yyy-MM-dd");
                path = path + str2 + @"邮件\";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }
            path = path + base.m_snote;
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
        public void SaveAddressbook(string EmailText, string path)
        {
            string saveFilePath = "";
            if ((GlobalValue.mainForm.saveFilePath != "") | (GlobalValue.mainForm.saveFilePath != null))
            {
                saveFilePath = GlobalValue.mainForm.saveFilePath;
            }
            if (GlobalValue.mainForm.checkDataSave)
            {
                string str2 = DateTime.Now.Date.ToString("yyy-MM-dd");
                saveFilePath = saveFilePath + str2 + @"邮件\";
                if (!Directory.Exists(saveFilePath))
                {
                    Directory.CreateDirectory(saveFilePath);
                }
            }
            saveFilePath = saveFilePath + base.m_snote;
            if (!Directory.Exists(saveFilePath))
            {
                Directory.CreateDirectory(saveFilePath);
            }
            saveFilePath = saveFilePath + @"\" + base.m_stype;
            if (!Directory.Exists(saveFilePath))
            {
                Directory.CreateDirectory(saveFilePath);
            }
            string str3 = saveFilePath;
            saveFilePath = str3 + @"\" + base.m_username + "@" + base.m_serv;
            if (!Directory.Exists(saveFilePath))
            {
                Directory.CreateDirectory(saveFilePath);
            }
            saveFilePath = saveFilePath + @"\地址薄";
            if (!Directory.Exists(saveFilePath))
            {
                Directory.CreateDirectory(saveFilePath);
            }
            Stream stream = File.Create(saveFilePath + @"\" + base.m_username + "boxlist.html");
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
                if ((GlobalValue.mainForm.saveFilePath != "") | (GlobalValue.mainForm.saveFilePath != null))
                {
                    filePath = GlobalValue.mainForm.saveFilePath;
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
                GlobalValue.mainForm.ExecuteSQL(strSql);
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
                    if (GlobalValue.mainForm.checkDataSave)
                    {
                        string str2 = DateTime.Now.Date.ToString("yyy-MM-dd");
                        filePath = filePath + str2 + @"邮件\";
                        if (!Directory.Exists(filePath))
                        {
                            Directory.CreateDirectory(filePath);
                        }
                    }
                    filePath = filePath + base.m_snote;
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
