using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Web;
using System.IO;
using System.Threading;
using System.Runtime.InteropServices;

namespace MailExposure.PopMailRC
{
    class CNMailReceiver:MailStr
    {
        // Fields
        private BoxNameID[] boxList;
        private CookieContainer cookieContainer;
        public string m_emailTime;
        private int PageNum;
        public string template;
        private string url;
        private string urls;

        // Methods
        public CNMailReceiver()
        {
            this.url = "";
            this.urls = "";
            this.boxList = new BoxNameID[100];
            this.template = "";
            this.cookieContainer = new CookieContainer();
            this.PageNum = 1;

        }
        public void businessLogin()
        {
            base.cookie = "";
            int startIndex = 0;
            string indata = "DomainName=" + base.m_serv + "&LoginName1=" + base.m_username + "@" + base.m_serv + "&LoginName=" + base.m_username + "@" + base.m_serv + "&passwd=" + base.m_passwd + "&Template=uudenyong&NeedMoreSecurity=on&NeedIpCheck&imageField.x=43&imageField.y=19";
            this.url = "http://hermes.webmail.21cn.net/webmail/business_router.jsp ";
            base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
            base.streamControl = true;
            base.MyStringBuilder = this.PostData(this.url, indata);
            startIndex = base.MyStringBuilder.ToString().IndexOf("http://hermes.webmail.21cn.net/");
            if (startIndex != -1)
            {
                this.url = base.putstr(base.MyStringBuilder.ToString(), "", "'", startIndex);
                startIndex = base.MyStringBuilder.ToString().IndexOf("template");
                if (startIndex != -1)
                {
                    this.template = base.putstr(base.MyStringBuilder.ToString(), "value='", "'", startIndex);
                }
                indata = "UserName=" + base.m_username + "@" + base.m_serv + "&passwd=" + base.m_passwd + "&template=" + this.template;
                base.ShowMessage("正在登陆…………");
                base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                base.streamControl = true;
                base.MyStringBuilder = this.PostData(this.url, indata);
                startIndex = base.MyStringBuilder.ToString().IndexOf("window.location.href");
                if (startIndex != -1)
                {
                    this.url = base.putstr(base.MyStringBuilder.ToString(), "\"/", "\"", startIndex);
                    base.Host = this.urls.Substring(0, this.urls.IndexOf("webmail/"));
                    this.url = base.Host + this.url;
                    base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                    base.streamControl = true;
                    base.MyStringBuilder = this.Request(this.url);
                    startIndex = base.MyStringBuilder.ToString().IndexOf("jsp/pbussmail1/css/signOn.css");
                    if (startIndex != -1)
                    {
                        base.ShowMessage("登陆成功…………");
                        this.getBoxName();
                    }
                }
            }

        }
        public void GetAddressBook()
        {
            try
            {
                string url = base.Host + "/webmail/contactList.do?command=export&args=csv&outport.x=%E5%BC%80%E5%A7%8B%E5%AF%BC%E5%87%BA";
                base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                base.streamControl = true;
                base.MyStringBuilder = this.Request(url);
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
                string message = "";
                if (this.urls.IndexOf("webmail.21cn") != -1)
                {
                    this.url = base.Host + "/webmail/accountState.do";
                    base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                    base.streamControl = true;
                    base.MyStringBuilder = this.Request(this.url);
                    startIndex = base.MyStringBuilder.ToString().IndexOf("<a href=\"javascript:{gotoMailList");
                    if (startIndex != -1)
                    {
                        while ((startIndex = base.MyStringBuilder.ToString().IndexOf("<a href=\"javascript:{gotoMailList", startIndex)) != -1)
                        {
                            message = base.MyStringBuilder.ToString();
                            string str2 = base.putstr(message, "'", "'", startIndex);
                            string str3 = base.putstr(message, "\">", "</a>", startIndex);
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
                    else
                    {
                        startIndex = base.MyStringBuilder.ToString().IndexOf("http://fmail.21cn.com:8080/scoreactive/sun.html");
                        if (startIndex == -1)
                        {
                            startIndex = base.MyStringBuilder.ToString().IndexOf("mailList.do?label");
                            if (startIndex != -1)
                            {
                                while ((startIndex = base.MyStringBuilder.ToString().IndexOf("<a href=\"mailList.do?", startIndex)) != -1)
                                {
                                    message = base.putstr(base.MyStringBuilder.ToString(), "mailList.do?", "</tr>", startIndex);
                                    string str4 = base.putstr(message, "label=", "\"", 0);
                                    string str5 = base.putstr(message, "\">", "</a>", 0);
                                    if (((str4 != "-1") && (str5 != "-1")) && ((str4 != "") && (str5 != "")))
                                    {
                                        this.boxList[index].boxid = str4;
                                        this.boxList[index].boxname = str5;
                                        index++;
                                    }
                                    startIndex++;
                                }
                                for (int j = 0; j < index; j++)
                                {
                                    this.getoldEmailId(base.Host, this.boxList[j].boxname, this.boxList[j].boxid);
                                }
                            }
                        }
                        else
                        {
                            startIndex = base.MyStringBuilder.ToString().IndexOf("<a href=\"mailList.do?");
                            if (startIndex != -1)
                            {
                                while ((startIndex = base.MyStringBuilder.ToString().IndexOf("<a href=\"mailList.do?", startIndex)) != -1)
                                {
                                    message = base.putstr(base.MyStringBuilder.ToString(), "mailList.do?", "</tr>", startIndex);
                                    string str6 = base.putstr(message, "label=", "\"", 0);
                                    string str7 = base.putstr(message, "\">", "</a>", 0);
                                    if (((str6 != "-1") && (str7 != "-1")) && ((str6 != "") && (str7 != "")))
                                    {
                                        this.boxList[index].boxid = str6;
                                        this.boxList[index].boxname = str7;
                                        index++;
                                    }
                                    startIndex++;
                                }
                                for (int k = 0; k < index; k++)
                                {
                                    this.getoldEmailId(base.Host, this.boxList[k].boxname, this.boxList[k].boxid);
                                }
                            }
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
            string str3 = "";
            string message = "";
            string str5 = "";
            int num2 = 0;
            int num3 = 0;
            int start = 0;
            string str7 = "";
            try
            {
                string str6;
                base.BoxName = boxName;
                url = urlbox + "/webmail/mailList.do?label=" + boxId;
                base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                base.streamControl = true;
                base.MyStringBuilder = this.Request(url);
                string str8 = base.MyStringBuilder.ToString();
                int index = str8.IndexOf("NYnewInfo\"");
                if (index != -1)
                {
                    string str9 = base.putstr(str8, "NYnewInfo\">", "span>", index);
                    num2 = Convert.ToInt32(base.putstr(str9, "\">", "</", 0));
                }
                if (num2 != 0)
                {
                    goto Label_0431;
                }
                base.ShowMessage(boxName + "没有邮件！");
                return;
            Label_00DE:
                str6 = base.putstr(str8, "<tr class", "</script>", startIndex);
                str6.IndexOf("status_5");
                string str10 = base.putstr(str6, "avascript:fComPlaint", ");", 0);
                str7 = base.putstr(str10, ",'", "'", 0);
                str10 = base.putstr(str10, "'", "'", 0);
                string str11 = "";
                if ((str10 != "-1") && (str10 != ""))
                {
                    string strSql = "select count(*) from 21CNMailId where MsgId='" + str10 + "'";
                    if (Convert.ToInt32(GlobalValue.PopMainForm.ExecuteSQL(strSql)) == 0)
                    {
                        int num7 = str6.IndexOf("fromList");
                        if (num7 != -1)
                        {
                            str3 = HttpUtility.HtmlDecode(base.putstr(str6, "value=\"", "\"", num7));
                        }
                        message = base.putstr(str6, "主题", "/a>", 0);
                        int num8 = message.IndexOf("readMail.do?messageid");
                        message = base.putstr(message, ">", "<", num8).Trim();
                        str11 = HttpUtility.UrlEncode(Encoding.GetEncoding("GB2312").GetBytes(message.ToCharArray())).ToUpper();
                        str5 = base.putstr(str6, "日期", "d>", 0);
                        str5 = base.putstr(str5, "\">", "</t", 0);
                        num8 = str5.IndexOf("<span id=");
                        if (num8 > 0)
                        {
                            str5 = base.putstr(str5, "\">", "<", num8).Trim();
                        }
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
                            base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                            base.streamControl = true;
                            str2 = base.Host + "/webmail/getAttachment.do?messageid=" + str10 + "&msid=" + str7 + "&partid=9999";
                            base.streamControl = true;
                            try
                            {
                                this.RequestEmail(str2);
                                try
                                {
                                    string str13 = DateTime.Now.ToString();
                                    strSql = "insert into 21CNMailId(Name,MsgId,DownTime,MailType) values('" + base.m_username + "','" + str10 + "','" + str13 + "','" + base.BoxName.Substring(0, base.BoxName.Length % 10) + "')";
                                    GlobalValue.PopMainForm.ExecuteSQL(strSql);
                                }
                                catch (Exception exception)
                                {
                                    base.ShowMessage("添加失败：" + exception.Message);
                                }
                                base.ShowMessage(base.BoxName + ":" + message + "\t下载");
                            }
                            catch (Exception exception2)
                            {
                                base.ShowMessage("邮件下载失败:" + message + ":\t" + exception2.Message);
                            }
                        }
                    }
                }
                num3++;
                startIndex++;
            Label_0431:
                if ((startIndex = str8.IndexOf("script type=\"text/javascript\">", startIndex)) != -1)
                {
                    goto Label_00DE;
                }
                if (num2 > num3)
                {
                    base.ShowMessage(base.BoxName + "第" + this.PageNum.ToString() + "页邮件下载成功！");
                    start += num3;
                    while ((num2 - start) > 0)
                    {
                        this.PageNum++;
                        if (Convert.ToInt32(GlobalValue.PopMainForm.PageNumber) == 0)
                        {
                            this.GetNextPage(urlbox, boxName, boxId, start);
                            start += num3;
                            base.ShowMessage(base.BoxName + "第" + this.PageNum.ToString() + "页邮件下载成功！");
                        }
                        else
                        {
                            if (this.PageNum <= Convert.ToInt32(GlobalValue.PopMainForm.PageNumber))
                            {
                                this.GetNextPage(urlbox, boxName, boxId, start);
                                start += num3;
                                base.ShowMessage(base.BoxName + "第" + this.PageNum.ToString() + "页邮件下载成功！");
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
            string message = "";
            string str5 = "";
            string str7 = "";
            try
            {
                url = string.Concat(new object[] { urlbox, "/webmail/mailList.do?orderField=0&label=", boxId, "&page=", this.PageNum, "&start=", start });
                base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                base.streamControl = true;
                base.MyStringBuilder = this.Request(url);
                string str8 = base.MyStringBuilder.ToString();
                while ((startIndex = str8.IndexOf("script type=\"text/javascript\">", startIndex)) != -1)
                {
                    string str6 = base.putstr(str8, "<tr class", "</script>", startIndex);
                    str6.IndexOf("status_5");
                    string str9 = base.putstr(str6, "avascript:fComPlaint", ");", 0);
                    str7 = base.putstr(str9, ",'", "'", 0);
                    str9 = base.putstr(str9, "'", "'", 0);
                    string str10 = "";
                    if ((str9 != "-1") && (str9 != ""))
                    {
                        string strSql = "select count(*) from 21CNMailId where MsgId='" + str9 + "'";
                        if (Convert.ToInt32(GlobalValue.PopMainForm.ExecuteSQL(strSql)) == 0)
                        {
                            int index = str6.IndexOf("fromList");
                            if (index != -1)
                            {
                                string str3 = HttpUtility.HtmlDecode(base.putstr(str6, "value=\"", "\"", index));
                            }
                            message = base.putstr(str6, "主题", "/a>", 0);
                            int num4 = message.IndexOf("readMail.do?messageid");
                            message = base.putstr(message, ">", "<", num4).Trim();
                            str10 = HttpUtility.UrlEncode(Encoding.GetEncoding("GB2312").GetBytes(message.ToCharArray())).ToUpper();
                            str5 = base.putstr(str6, "日期", "d>", 0);
                            str5 = base.putstr(str5, "\">", "</t", 0);
                            num4 = str5.IndexOf("<span id=");
                            if (num4 > 0)
                            {
                                str5 = base.putstr(str5, "\">", "<", num4).Trim();
                            }
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
                                base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                                base.streamControl = true;
                                str2 = base.Host + "/webmail/getAttachment.do?messageid=" + str9 + "&msid=" + str7 + "&partid=9999";
                                base.streamControl = true;
                                try
                                {
                                    this.RequestEmail(str2);
                                    try
                                    {
                                        string str12 = DateTime.Now.ToString();
                                        strSql = "insert into 21CNMailId(Name,MsgId,DownTime,MailType) values('" + base.m_username + "','" + str9 + "','" + str12 + "','" + base.BoxName.Substring(0, base.BoxName.Length % 10) + "')";
                                        GlobalValue.PopMainForm.ExecuteSQL(strSql);
                                    }
                                    catch (Exception exception)
                                    {
                                        base.ShowMessage("添加失败：" + exception.Message);
                                    }
                                    base.ShowMessage(base.BoxName + ":" + message + "\t下载");
                                }
                                catch (Exception exception2)
                                {
                                    base.ShowMessage("邮件下载失败:" + message + ":\t" + exception2.Message);
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
        private void getoldEmailId(string urlbox, string boxName, string boxId)
        {
            int startIndex = 0;
            string url = "";
            string str2 = "";
            string str3 = "";
            string str4 = "";
            int num2 = 0;
            int num3 = 0;
            int start = 0;
            try
            {
                base.BoxName = boxName;
                url = urlbox + "webmail/showOption.do";
                base.streamControl = true;
                base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                base.MyStringBuilder = this.Request(url);
                int index = base.MyStringBuilder.ToString().IndexOf("input name=\"mailsPerPage\"");
                if (index != -1)
                {
                    index = base.MyStringBuilder.ToString().IndexOf("checked>");
                    if (index != -1)
                    {
                        index = Convert.ToInt32(index) - 6;
                    }
                    num3 = Convert.ToInt32(base.putstr(base.MyStringBuilder.ToString(), "\"", "\"", index));
                }
                url = urlbox + "webmail/mailList.do?label=" + boxId;
                base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                base.streamControl = true;
                base.MyStringBuilder = this.Request(url);
                string message = base.MyStringBuilder.ToString();
                index = message.IndexOf("<span class=\"red\"");
                if (index != -1)
                {
                    num2 = Convert.ToInt32(base.putstr(message, ">", "</span>", index));
                }
                if (num2 == 0)
                {
                    base.ShowMessage(base.BoxName + "没有邮件");
                }
                startIndex = message.IndexOf("document.write(gMailLstAd)");
                if (startIndex != -1)
                {
                    message = base.putstr(message, "document.write(gMailLstAd)", "</table>", startIndex);
                }
                startIndex = message.IndexOf("<tr>");
                if (startIndex != -1)
                {
                    while ((startIndex = message.IndexOf("<tr>", startIndex)) != -1)
                    {
                        string str7;
                        string str5 = base.putstr(message, "<tr>", "</tr>", startIndex);
                        string str8 = base.putstr(str5, "readMail.do?messageid=", "\"", 0);
                        if (str8.IndexOf("21cn.com") != -1)
                        {
                            str8 = str8 + " ";
                            str7 = base.putstr(str8, ".com@", " ", 0);
                            str8 = base.putstr(str8, "", ".com", 0);
                        }
                        else
                        {
                            str8 = str8 + " ";
                            str7 = base.putstr(str8, ".net@", " ", 0);
                            str8 = base.putstr(str8, "", ".net", 0);
                        }
                        if ((str8 != "-1") && (str8 != ""))
                        {
                            string strSql = "select count(*) from 21CNMailId where MsgId='" + str8 + "'";
                            if (Convert.ToInt32(GlobalValue.PopMainForm.ExecuteSQL(strSql)) == 0)
                            {
                                int num7 = str5.IndexOf("SendTime");
                                if (num7 != -1)
                                {
                                    str4 = base.putstr(str5, "<strong>", "</strong>", num7).Trim();
                                }
                                if (str4 == "-1")
                                {
                                    str4 = base.putstr(str5, "nowrap>", "&nbsp;", num7).Trim();
                                }
                                num7 = str5.IndexOf("MailContent");
                                if (num7 != -1)
                                {
                                    str3 = base.putstr(str5, "target=\"mainFrame\">", "</a>", num7).Trim();
                                }
                                bool flag = false;
                                try
                                {
                                    DateTime time = new DateTime();
                                    time = Convert.ToDateTime(str4);
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
                                    base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                                    base.streamControl = true;
                                    if (base.Host.IndexOf("21cn.com") != -1)
                                    {
                                        str2 = base.Host + "webmail/getAttachment.do?messageid=" + str8 + ".com&msid=" + str7 + "&partid=9999";
                                    }
                                    else
                                    {
                                        str2 = base.Host + "webmail/getAttachment.do?messageid=" + str8 + ".net&msid=" + str7 + "&partid=9999";
                                    }
                                    base.streamControl = true;
                                    try
                                    {
                                        this.RequestEmail(str2);
                                        try
                                        {
                                            string str10 = DateTime.Now.ToString();
                                            strSql = "insert into 21CNMailId(Name,MsgId,DownTime,MailType) values('" + base.m_username + "','" + str8 + "','" + str10 + "','" + base.BoxName.Substring(0, base.BoxName.Length % 10) + "')";
                                            GlobalValue.PopMainForm.ExecuteSQL(strSql);
                                        }
                                        catch (Exception exception)
                                        {
                                            base.ShowMessage("添加失败：" + exception.Message);
                                        }
                                        base.ShowMessage(base.BoxName + ":" + str3 + "\t下载");
                                    }
                                    catch (Exception exception2)
                                    {
                                        base.ShowMessage("邮件下载失败:" + str3 + ":\t" + exception2.Message);
                                    }
                                }
                            }
                        }
                        startIndex++;
                    }
                }
                if (num2 > num3)
                {
                    base.ShowMessage(base.BoxName + "第" + this.PageNum.ToString() + "页邮件下载成功！");
                    start += num3;
                    while ((num2 - start) > 0)
                    {
                        this.PageNum++;
                        if (Convert.ToInt32(GlobalValue.PopMainForm.PageNumber) == 0)
                        {
                            this.GetoldNextPage(urlbox, boxName, boxId, start);
                            start += num3;
                            base.ShowMessage(base.BoxName + "第" + this.PageNum.ToString() + "页邮件下载成功！");
                        }
                        else
                        {
                            if (this.PageNum <= Convert.ToInt32(GlobalValue.PopMainForm.PageNumber))
                            {
                                this.GetoldNextPage(urlbox, boxName, boxId, start);
                                start += num3;
                                base.ShowMessage(base.BoxName + "第" + this.PageNum.ToString() + "页邮件下载成功！");
                                continue;
                            }
                            this.PageNum = 1;
                            return;
                        }
                    }
                }
                this.PageNum = 1;
            }
            catch (Exception exception3)
            {
                base.ShowMessage("取邮件失败" + exception3.Message);
            }

        }
        private void GetoldNextPage(string urlbox, string boxName, string boxId, int start)
        {
            int startIndex = 0;
            string url = "";
            string str2 = "";
            string str3 = "";
            string str4 = "";
            try
            {
                base.BoxName = boxName;
                url = string.Concat(new object[] { urlbox, "webmail/mailList.do?label=", boxId, "&page=", this.PageNum });
                base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                base.streamControl = true;
                base.MyStringBuilder = this.Request(url);
                string message = base.MyStringBuilder.ToString();
                startIndex = message.IndexOf("document.write(gMailLstAd)");
                if (startIndex != -1)
                {
                    message = base.putstr(message, "document.write(gMailLstAd)", "</table>", startIndex);
                }
                startIndex = message.IndexOf("<tr>");
                if (startIndex != -1)
                {
                    while ((startIndex = message.IndexOf("<tr>", startIndex)) != -1)
                    {
                        string str7;
                        string str5 = base.putstr(message, "<tr>", "</tr>", startIndex);
                        string str8 = base.putstr(str5, "readMail.do?messageid=", "\"", 0);
                        if (str8.IndexOf("21cn.com") != -1)
                        {
                            str8 = str8 + " ";
                            str7 = base.putstr(str8, ".com@", " ", 0);
                            str8 = base.putstr(str8, "", ".com", 0);
                        }
                        else
                        {
                            str8 = str8 + " ";
                            str7 = base.putstr(str8, ".net@", " ", 0);
                            str8 = base.putstr(str8, "", ".net", 0);
                        }
                        if ((str8 != "-1") && (str8 != ""))
                        {
                            string strSql = "select count(*) from 21CNMailId where MsgId='" + str8 + "'";
                            if (Convert.ToInt32(GlobalValue.PopMainForm.ExecuteSQL(strSql)) == 0)
                            {
                                int index = str5.IndexOf("SendTime");
                                if (index != -1)
                                {
                                    str4 = base.putstr(str5, "<strong>", "</strong>", index).Trim();
                                }
                                if (str4 == "-1")
                                {
                                    str4 = base.putstr(str5, "nowrap>", "&nbsp;", index).Trim();
                                }
                                index = str5.IndexOf("MailContent");
                                if (index != -1)
                                {
                                    str3 = base.putstr(str5, "target=\"mainFrame\">", "</a>", index).Trim();
                                }
                                bool flag = false;
                                try
                                {
                                    DateTime time = new DateTime();
                                    time = Convert.ToDateTime(str4);
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
                                    base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                                    base.streamControl = true;
                                    if (base.Host.IndexOf("21cn.com") != -1)
                                    {
                                        str2 = base.Host + "webmail/getAttachment.do?messageid=" + str8 + ".com&msid=" + str7 + "&partid=9999";
                                    }
                                    else
                                    {
                                        str2 = base.Host + "webmail/getAttachment.do?messageid=" + str8 + ".net&msid=" + str7 + "&partid=9999";
                                    }
                                    base.streamControl = true;
                                    try
                                    {
                                        this.RequestEmail(str2);
                                        try
                                        {
                                            string str10 = DateTime.Now.ToString();
                                            strSql = "insert into 21CNMailId(Name,MsgId,DownTime,MailType) values('" + base.m_username + "','" + str8 + "','" + str10 + "','" + base.BoxName.Substring(0, base.BoxName.Length % 10) + "')";
                                            GlobalValue.PopMainForm.ExecuteSQL(strSql);
                                        }
                                        catch (Exception exception)
                                        {
                                            base.ShowMessage("添加失败：" + exception.Message);
                                        }
                                        base.ShowMessage(base.BoxName + ":" + str3 + "\t下载");
                                    }
                                    catch (Exception exception2)
                                    {
                                        base.ShowMessage("邮件下载失败:" + str3 + ":\t" + exception2.Message);
                                    }
                                }
                            }
                        }
                        startIndex++;
                    }
                }
            }
            catch (Exception exception3)
            {
                base.ShowMessage("取邮件失败" + exception3.Message);
            }

        }
        public override void login()
        {
            try
            {
                base.cookie = "";
                int startIndex = 0;
                startIndex = base.m_serv.IndexOf("21cn.net");
                if (startIndex != -1)
                {
                    this.businessLogin();
                }
                else
                {
                    string indata = "LoginName=" + base.m_username + "&DomainName=" + base.m_serv + "&Template=&UserName=" + base.m_username + "&passwd=" + base.m_passwd + "&remUser=";
                    this.url = "http://passport.21cn.com/maillogin.jsp";
                    base.ShowMessage("开始登陆…………");
                    Thread.Sleep(0xbb8);
                    base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                    base.streamControl = true;
                    base.MyStringBuilder = this.PostData(this.url, indata);
                    startIndex = base.MyStringBuilder.ToString().IndexOf("window.location.href");
                    if (startIndex != -1)
                    {
                        this.url = base.putstr(base.MyStringBuilder.ToString(), "=\"", "\"", startIndex);
                        base.Host = this.urls.Substring(0, this.urls.IndexOf("/webmail"));
                        this.url = base.Host + this.url;
                        base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                        base.streamControl = true;
                        base.MyStringBuilder = this.Request(this.url);
                        startIndex = base.MyStringBuilder.ToString().IndexOf("faviconFreemail");
                        if (startIndex != -1)
                        {
                            base.ShowMessage("登陆成功…………");
                            this.getBoxName();
                        }
                        else if (((startIndex = base.MyStringBuilder.ToString().IndexOf("navigator.appVersion")) != -1) && ((startIndex = base.MyStringBuilder.ToString().IndexOf("mail.21cn.com")) != -1))
                        {
                            base.ShowMessage("登陆成功…………");
                            this.getBoxName();
                        }
                        else
                        {
                            startIndex = base.MyStringBuilder.ToString().IndexOf("http://mail.21cn.com/js/21cnmenu.html");
                            if (startIndex != -1)
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
                request.ContentType = "application/x-www-form-urlencoded";
                request.Headers.Add("Pragma:no-cache\r\n");
                request.Accept = "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, application/vnd.ms-excel, application/msword, */*";
                request.Headers.Add("Accept-Language: en-us,ar-SA;q=0.9,de-DE;q=0.8,es-ES;q=0.7,tr-TR;q=0.6,ja-JP;q=0.5,en-GB;q=0.4,fr-FR;q=0.3,zh-CN;q=0.2,zh-TW;q=0.1");
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 5.1; Trident/4.0; GTB6; EmbeddedWB 14.52 from: http://www.bsalsa.com/ EmbeddedWB 14.52; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022)";
                request.ContentLength = indata.Length;
                request.AllowAutoRedirect = false;
                request.KeepAlive = true;
                request.CookieContainer = this.cookieContainer;
                Stream requestStream = request.GetRequestStream();
                char[] chars = Encoding.ASCII.GetChars(Encoding.ASCII.GetBytes(indata));
                StreamWriter writer = new StreamWriter(requestStream);
                writer.Write(chars, 0, chars.Length);
                writer.Close();
                requestStream.Close();
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                response.Cookies = this.cookieContainer.GetCookies(request.RequestUri);
                Thread.Sleep(10);
                response.GetResponseStream();
                WebHeaderCollection headers = response.Headers;
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
                base.ShowMessage("21CNmail:" + exception.Message + "请再次尝试！");
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
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 5.1; Trident/4.0; GTB6; EmbeddedWB 14.52 from: http://www.bsalsa.com/ EmbeddedWB 14.52; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022)";
                request.ContentType = "application/x-www-form-urlencoded";
                request.Headers.Add("Accept-Language: zh-cn");
                request.CookieContainer = this.cookieContainer;
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
                    else
                    {
                        base.charSet = null;
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
            request.Method = "GET";
            request.Accept = "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, */*";
            request.AllowAutoRedirect = false;
            request.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 5.1; Trident/4.0; GTB6; EmbeddedWB 14.52 from: http://www.bsalsa.com/ EmbeddedWB 14.52; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022)";
            request.ContentType = "application/octet-stream";
            request.KeepAlive = true;
            request.Headers.Add("Accept-Language: zh-cn");
            request.CookieContainer = this.cookieContainer;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            response.Cookies = this.cookieContainer.GetCookies(response.ResponseUri);
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
                GlobalValue.PopMainForm.statusBarPanelEmailNumbersText.Text = (Convert.ToInt32(GlobalValue.PopMainForm.statusBarPanelEmailNumbersText.Text) + 1).ToString();
                base.streamControl = false;
                return true;
            }
            responseStream.Close();
            return false;

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
                if ((GlobalValue.PopMainForm.saveFilePath != "") | (GlobalValue.PopMainForm.saveFilePath != null))
                {
                    filePath = GlobalValue.PopMainForm.saveFilePath;
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
