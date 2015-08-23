using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Web;
using System.Threading;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;

namespace MailExposure.PopMailRC
{
    class SinaMailReceiver:MailStr
    {
        // Fields
        private BoxNameID[] boxList;
        public string checkTime;
        private CookieContainer cookieContainer;
        public string m_emailTime;
        private int PageNum;
        public string sid;
        private string url;
        private string urls;

        // Methods
        public SinaMailReceiver()
        {
            this.url = "";
            this.urls = "";
            this.boxList = new BoxNameID[100];
            this.sid = "";
            this.checkTime = "";
            this.cookieContainer = new CookieContainer();
        }
        public void GetAddressBook()
        {
            try
            {
                string url = base.Host + "basic/addr_export.php?check_time=" + this.checkTime;
                string indata = "extype=csv";
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
                if (this.urls.IndexOf("sinamail.sina.com") != -1)
                {
                    this.url = base.Host + "classic/folderlist.php?check_time=" + this.checkTime;
                    base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                    base.streamControl = true;
                    base.MyStringBuilder = this.Request(this.url);
                    string message = this.U2CnCode(base.MyStringBuilder.ToString());
                    startIndex = message.IndexOf("folderlist");
                    if (startIndex != -1)
                    {
                        while ((startIndex = message.IndexOf("{", startIndex)) != -1)
                        {
                            string[] strArray = base.putstr(message, "\"", "}", startIndex).Split(new char[] { ',' });
                            string str3 = base.putstr(strArray[0], ":\"", "\"", 0);
                            string str4 = base.putstr(strArray[1], ":\"", "\"", 0);
                            if (((str3 != "-1") && (str4 != "-1")) && ((str3 != "") && (str4 != "")))
                            {
                                this.boxList[index].boxid = str3;
                                this.boxList[index].boxname = str4;
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
            string indata = "";
            string str5 = "";
            string str6 = "";
            int num2 = 0;
            int num3 = 0;
            int start = 1;
            try
            {
                string str7;
                bool flag;
                base.BoxName = boxName;
                url = urlbox + "classic/maillist.php";
                indata = string.Concat(new object[] { "fid=", boxId, "&by=htime&ascdesc=desc&pageno=", start, "&flag=0" });
                base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                base.streamControl = true;
                base.MyStringBuilder = this.PostData(url, indata);
                string message = this.U2CnCode(base.MyStringBuilder.ToString());
                int index = message.IndexOf("mailcount");
                if (index != -1)
                {
                    num2 = Convert.ToInt32(base.putstr(message, "\":", ",", index));
                }
                index = message.IndexOf("pagesize");
                if (index != -1)
                {
                    num3 = Convert.ToInt32(base.putstr(message, "\":", ",", index));
                }
                if (num2 != 0)
                {
                    goto Label_04F8;
                }
                base.ShowMessage(boxName + "没有邮件！");
                return;
            Label_0130:
                str7 = base.putstr(message, "mid", "},", startIndex);
                if (str7.IndexOf("isread\":true") != -1)
                {
                    flag = false;
                }
                else
                {
                    flag = true;
                }
                string str9 = base.putstr(str7, "\":\"", "\",", 0);
                string str10 = "";
                if ((str9 != "-1") && (str9 != ""))
                {
                    string strSql = "select count(*) from SinaMailId where MsgId='" + str9 + "'";
                    if (Convert.ToInt32(GlobalValue.PopMainForm.ExecuteSQL(strSql)) == 0)
                    {
                        string str4 = HttpUtility.HtmlDecode(base.putstr(str7, "&lt;", "&gt;", 0));
                        str5 = base.putstr(str7, "subject\":\"", "\",", 0);
                        str10 = HttpUtility.UrlEncode(Encoding.GetEncoding("UTF-8").GetBytes(str5.ToCharArray())).ToUpper();
                        str6 = base.putstr(str7, "date\":", ",", 0);
                        bool flag2 = false;
                        try
                        {
                            DateTime time = new DateTime();
                            time = Convert.ToDateTime(str6);
                            if (DateTime.Compare(time, GlobalValue.PopMainForm.EmailDateTime) >= 0)
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
                            base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                            base.streamControl = true;
                            string str12 = this.getGNlZtime();
                            str2 = base.Host + "/classic/read_mid.php?fid=" + boxId + "&mid=" + str9 + "&mailSubject=" + str10 + "&ts=" + str12;
                            try
                            {
                                this.RequestEmail(str2);
                                try
                                {
                                    string str13 = DateTime.Now.ToString();
                                    strSql = "insert into SinaMailId(Name,MsgId,DownTime,MailType) values('" + base.m_username + "','" + str9 + "','" + str13 + "','" + base.BoxName.Substring(0, base.BoxName.Length % 10) + "')";
                                    GlobalValue.PopMainForm.ExecuteSQL(strSql);
                                }
                                catch (Exception exception)
                                {
                                    base.ShowMessage("添加失败：" + exception.Message);
                                }
                                base.ShowMessage(base.BoxName + ":" + str5 + "\t下载");
                                if (flag)
                                {
                                    string str14 = "";
                                    if (str9.IndexOf(":2,S") != -1)
                                    {
                                        str14 = base.putstr(str9, "", ":2,S", 0) + "%3A2%2CS";
                                    }
                                    else
                                    {
                                        str14 = str9;
                                    }
                                    string str16 = "fid=" + boxId + "&mid[]=" + str14 + "&optype=mark&readflag=0";
                                    string str17 = base.Host + "classic/mail_op.php";
                                    base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                                    base.streamControl = true;
                                    base.MyStringBuilder = this.PostData(str17, str16);
                                    if (base.MyStringBuilder.ToString() == "")
                                    {
                                        base.ShowMessage("置未读：" + str5 + "失败\t");
                                    }
                                    else
                                    {
                                        base.ShowMessage("置未读：" + str5);
                                        flag = false;
                                    }
                                }
                            }
                            catch (Exception exception2)
                            {
                                base.ShowMessage("邮件下载失败:" + str5 + ":\t" + exception2.Message);
                            }
                        }
                    }
                }
                startIndex++;
            Label_04F8:
                if ((startIndex = message.IndexOf("mid", startIndex)) != -1)
                {
                    goto Label_0130;
                }
                if (num2 > num3)
                {
                    this.PageNum++;
                    base.ShowMessage(base.BoxName + "第" + this.PageNum.ToString() + "页邮件下载成功！");
                    start++;
                    while ((num2 - ((start - 1) * num3)) > 0)
                    {
                        if (Convert.ToInt32(GlobalValue.PopMainForm.PageNumber) == 0)
                        {
                            this.GetNextPage(urlbox, boxName, boxId, start);
                            start++;
                        }
                        else
                        {
                            if (this.PageNum < Convert.ToInt32(GlobalValue.PopMainForm.PageNumber))
                            {
                                this.GetNextPage(urlbox, boxName, boxId, start);
                                start++;
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
        public string getGNlZtime()
        {
            return Convert.ToInt64(DateTime.Now.Subtract(new DateTime(0x7b2, 1, 1, 8, 0, 0)).TotalMilliseconds).ToString();
        }
        public void GetNextPage(string urlbox, string boxName, string boxId, int start)
        {
            int startIndex = 0;
            string url = "";
            string str2 = "";
            string indata = "";
            string str5 = "";
            string str6 = "";
            try
            {
                base.BoxName = boxName;
                url = urlbox + "classic/maillist.php";
                indata = string.Concat(new object[] { "fid=", boxId, "&by=htime&ascdesc=desc&pageno=", start, "&flag=0" });
                base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                base.streamControl = true;
                base.MyStringBuilder = this.PostData(url, indata);
                string message = this.U2CnCode(base.MyStringBuilder.ToString());
                while ((startIndex = message.IndexOf("mid", startIndex)) != -1)
                {
                    bool flag;
                    string str7 = base.putstr(message, "mid", "},", startIndex);
                    if (str7.IndexOf("isread\":true") != -1)
                    {
                        flag = false;
                    }
                    else
                    {
                        flag = true;
                    }
                    string str9 = base.putstr(str7, "\":\"", "\",", 0);
                    string str10 = "";
                    if ((str9 != "-1") && (str9 != ""))
                    {
                        string strSql = "select count(*) from SinaMailId where MsgId='" + str9 + "'";
                        if (Convert.ToInt32(GlobalValue.PopMainForm.ExecuteSQL(strSql)) == 0)
                        {
                            string str4 = HttpUtility.HtmlDecode(base.putstr(str7, "&lt;", "&gt;", 0));
                            str5 = base.putstr(str7, "subject\":\"", "\",", 0);
                            str10 = HttpUtility.UrlEncode(Encoding.GetEncoding("UTF-8").GetBytes(str5.ToCharArray())).ToUpper();
                            str6 = base.putstr(str7, "date\":", ",", 0);
                            bool flag2 = false;
                            try
                            {
                                DateTime time = new DateTime();
                                time = Convert.ToDateTime(str6);
                                if (DateTime.Compare(time, GlobalValue.PopMainForm.EmailDateTime) >= 0)
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
                                base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                                base.streamControl = true;
                                string str12 = this.getGNlZtime();
                                str2 = base.Host + "/classic/read_mid.php?fid=" + boxId + "&mid=" + str9 + "&mailSubject=" + str10 + "&ts=" + str12;
                                try
                                {
                                    this.RequestEmail(str2);
                                    try
                                    {
                                        string str13 = DateTime.Now.ToString();
                                        strSql = "insert into SinaMailId(Name,MsgId,DownTime,MailType) values('" + base.m_username + "','" + str9 + "','" + str13 + "','" + base.BoxName.Substring(0, base.BoxName.Length % 10) + "')";
                                        GlobalValue.PopMainForm.ExecuteSQL(strSql);
                                    }
                                    catch (Exception exception)
                                    {
                                        base.ShowMessage("添加失败：" + exception.Message);
                                    }
                                    base.ShowMessage(base.BoxName + ":" + str5 + "\t下载");
                                    if (flag)
                                    {
                                        string str14 = "";
                                        if (str9.IndexOf(":2,S") != -1)
                                        {
                                            str14 = base.putstr(str9, "", ":2,S", 0) + "%3A2%2CS";
                                        }
                                        else
                                        {
                                            str14 = str9;
                                        }
                                        string str16 = "fid=" + boxId + "&mid[]=" + str14 + "&optype=mark&readflag=0";
                                        string str17 = base.Host + "classic/mail_op.php";
                                        base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                                        base.streamControl = true;
                                        base.MyStringBuilder = this.PostData(str17, str16);
                                        if (base.MyStringBuilder.ToString() == "")
                                        {
                                            base.ShowMessage("置未读：" + str5 + "失败\t");
                                        }
                                        else
                                        {
                                            base.ShowMessage("置未读：" + str5);
                                            flag = false;
                                        }
                                    }
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
                this.PageNum++;
                base.ShowMessage(base.BoxName + "第" + this.PageNum.ToString() + "页邮件下载成功！");
            }
            catch (Exception exception3)
            {
                base.ShowMessage("取邮件失败!" + exception3.Message);
            }

        }
        private void getvipBoxName()
        {
            this.GetAddressBook();
            int startIndex = 0;
            int index = 0;
            try
            {
                string message = "";
                if (this.urls.IndexOf("sinamail.sina.com") != -1)
                {
                    this.url = base.Host + "classic/folderlist.php?check_time=" + this.checkTime;
                    base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                    base.streamControl = true;
                    base.MyStringBuilder = this.Request(this.url);
                    string str2 = this.U2CnCode(base.MyStringBuilder.ToString());
                    startIndex = str2.IndexOf("ajax/maillist.php?");
                    if (startIndex != -1)
                    {
                        while ((startIndex = str2.IndexOf("ajax/maillist.php?", startIndex)) != -1)
                        {
                            message = base.putstr(str2, "ajax/maillist.php?", "</tr>", startIndex);
                            string str3 = base.putstr(message, "fid=", "\"", 0);
                            string str4 = base.putstr(message, "\">", "</a></td>", 0);
                            if (((str3 != "-1") && (str4 != "-1")) && ((str3 != "") && (str4 != "")))
                            {
                                this.boxList[index].boxid = str3;
                                this.boxList[index].boxname = str4;
                                index++;
                            }
                            startIndex++;
                        }
                        for (int i = 0; i < index; i++)
                        {
                            this.getvipEmailId(base.Host, this.boxList[i].boxname, this.boxList[i].boxid);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                base.ShowMessage("取箱子失败！" + exception.Message);
            }
        }
        private void getvipEmailId(string urlbox, string boxName, string boxId)
        {
            int startIndex = 0;
            string url = "";
            string str2 = "";
            string str4 = "";
            string str5 = "";
            int num2 = 0;
            int num3 = 0;
            int start = 1;
            try
            {
                string str6;
                bool flag;
                base.BoxName = boxName;
                string str7 = this.getGNlZtime();
                url = string.Concat(new object[] { urlbox, "/classic/maillist_json.php?fid=", boxId, "&by=dtime&ascdesc=desc&pageno=", start, "&checktime=", str7 });
                base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                base.streamControl = true;
                base.MyStringBuilder = this.Request(url);
                string message = this.U2CnCode(base.MyStringBuilder.ToString());
                int index = message.IndexOf("mailcount");
                if (index != -1)
                {
                    num2 = Convert.ToInt32(base.putstr(message, "\":", ",", index));
                }
                index = message.IndexOf("pagesize");
                if (index != -1)
                {
                    num3 = Convert.ToInt32(base.putstr(message, "\":", ",", index));
                }
                if (num2 != 0)
                {
                    goto Label_04E8;
                }
                base.ShowMessage(boxName + "没有邮件！");
                return;
            Label_0130:
                str6 = base.putstr(message, "mid", "},", startIndex);
                if (str6.IndexOf("isread\":true") != -1)
                {
                    flag = false;
                }
                else
                {
                    flag = true;
                }
                string str9 = base.putstr(str6, "\":\"", "\",", 0);
                string str10 = "";
                if ((str9 != "-1") && (str9 != ""))
                {
                    string strSql = "select count(*) from SinaMailId where MsgId='" + str9 + "'";
                    if (Convert.ToInt32(GlobalValue.PopMainForm.ExecuteSQL(strSql)) == 0)
                    {
                        int num7 = 0;
                        num7 = str6.IndexOf("from");
                        if (num7 != -1)
                        {
                            string str3 = HttpUtility.HtmlDecode(base.putstr(str6, "&lt;", "&gt;", num7));
                        }
                        str4 = base.putstr(str6, "subject\":\"", "\",", 0);
                        str10 = HttpUtility.UrlEncode(Encoding.GetEncoding("UTF-8").GetBytes(str4.ToCharArray())).ToUpper();
                        str5 = base.putstr(str6, "date\":\"", "\"", 0);
                        bool flag2 = false;
                        try
                        {
                            DateTime time = new DateTime();
                            time = Convert.ToDateTime(str5);
                            if (DateTime.Compare(time, GlobalValue.PopMainForm.EmailDateTime) >= 0)
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
                            base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                            base.streamControl = true;
                            str7 = this.getGNlZtime();
                            str2 = base.Host + "/classic/read_mid.php?fid=" + boxId + "&mid=" + str9 + "&mailSubject=" + str10 + "&ts=" + str7;
                            try
                            {
                                this.RequestEmail(str2);
                                try
                                {
                                    string str12 = DateTime.Now.ToString();
                                    strSql = "insert into SinaMailId(Name,MsgId,DownTime,MailType) values('" + base.m_username + "','" + str9 + "','" + str12 + "','" + base.BoxName.Substring(0, base.BoxName.Length % 10) + "')";
                                    GlobalValue.PopMainForm.ExecuteSQL(strSql);
                                }
                                catch (Exception exception)
                                {
                                    base.ShowMessage("添加失败：" + exception.Message);
                                }
                                base.ShowMessage(base.BoxName + ":" + str4 + "\t下载");
                                if (flag)
                                {
                                    string str13 = "";
                                    if (str9.IndexOf(":2,S") != -1)
                                    {
                                        str13 = base.putstr(str9, "", ":2,S", 0) + "%3A2%2CS";
                                    }
                                    else
                                    {
                                        str13 = str9;
                                    }
                                    string indata = "optype=mark&readflag=0&mid%5B%5D=" + str13 + "%3A2%2CS&fid=" + boxName;
                                    string str16 = base.Host + "classic/mail_op_json.php";
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
            Label_04E8:
                if ((startIndex = message.IndexOf("mid", startIndex)) != -1)
                {
                    goto Label_0130;
                }
                if (num2 > num3)
                {
                    this.PageNum++;
                    base.ShowMessage(base.BoxName + "第" + this.PageNum.ToString() + "页邮件下载成功！");
                    start++;
                    while ((num2 - ((start - 1) * num3)) > 0)
                    {
                        if (Convert.ToInt32(GlobalValue.PopMainForm.PageNumber) == 0)
                        {
                            this.GetvipNextPage(urlbox, boxName, boxId, start);
                            start++;
                        }
                        else
                        {
                            if (this.PageNum < Convert.ToInt32(GlobalValue.PopMainForm.PageNumber))
                            {
                                this.GetvipNextPage(urlbox, boxName, boxId, start);
                                start++;
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
        public void GetvipNextPage(string urlbox, string boxName, string boxId, int start)
        {
            int startIndex = 0;
            string url = "";
            string str2 = "";
            string str4 = "";
            string str5 = "";
            try
            {
                base.BoxName = boxName;
                string str7 = this.getGNlZtime();
                url = string.Concat(new object[] { urlbox, "/classic/maillist_json.php?fid=", boxId, "&by=dtime&ascdesc=desc&pageno=", start, "&checktime=", str7 });
                base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                base.streamControl = true;
                base.MyStringBuilder = this.Request(url);
                string message = this.U2CnCode(base.MyStringBuilder.ToString());
                while ((startIndex = message.IndexOf("mid", startIndex)) != -1)
                {
                    bool flag;
                    string str6 = base.putstr(message, "mid", "},", startIndex);
                    if (str6.IndexOf("isread\":true") != -1)
                    {
                        flag = false;
                    }
                    else
                    {
                        flag = true;
                    }
                    string str9 = base.putstr(str6, "\":\"", "\",", 0);
                    string str10 = "";
                    if ((str9 != "-1") && (str9 != ""))
                    {
                        string strSql = "select count(*) from SinaMailId where MsgId='" + str9 + "'";
                        if (Convert.ToInt32(GlobalValue.PopMainForm.ExecuteSQL(strSql)) == 0)
                        {
                            int index = 0;
                            index = str6.IndexOf("from");
                            if (index != -1)
                            {
                                string str3 = HttpUtility.HtmlDecode(base.putstr(str6, "&lt;", "&gt;", index));
                            }
                            str4 = base.putstr(str6, "subject\":\"", "\",", 0);
                            str10 = HttpUtility.UrlEncode(Encoding.GetEncoding("UTF-8").GetBytes(str4.ToCharArray())).ToUpper();
                            str5 = base.putstr(str6, "date\":", ",", 0);
                            bool flag2 = false;
                            try
                            {
                                DateTime time = new DateTime();
                                time = Convert.ToDateTime(str5);
                                if (DateTime.Compare(time, GlobalValue.PopMainForm.EmailDateTime) >= 0)
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
                                base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                                base.streamControl = true;
                                str7 = this.getGNlZtime();
                                str2 = base.Host + "/classic/read_mid.php?fid=" + boxId + "&mid=" + str9 + "&mailSubject=" + str10 + "&ts=" + str7;
                                try
                                {
                                    this.RequestEmail(str2);
                                    try
                                    {
                                        string str12 = DateTime.Now.ToString();
                                        strSql = "insert into SinaMailId(Name,MsgId,DownTime,MailType) values('" + base.m_username + "','" + str9 + "','" + str12 + "','" + base.BoxName.Substring(0, base.BoxName.Length % 10) + "')";
                                        GlobalValue.PopMainForm.ExecuteSQL(strSql);
                                    }
                                    catch (Exception exception)
                                    {
                                        base.ShowMessage("添加失败：" + exception.Message);
                                    }
                                    base.ShowMessage(base.BoxName + ":" + str4 + "\t下载");
                                    if (flag)
                                    {
                                        string str13 = "";
                                        if (str9.IndexOf(":2,S") != -1)
                                        {
                                            str13 = base.putstr(str9, "", ":2,S", 0) + "%3A2%2CS";
                                        }
                                        else
                                        {
                                            str13 = str9;
                                        }
                                        string indata = "optype=mark&readflag=0&mid%5B%5D=" + str13 + "%3A2%2CS&fid=" + boxName;
                                        string str16 = base.Host + "classic/mail_op_json.php";
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
                startIndex = base.m_serv.IndexOf("vip.sina.com");
                if (startIndex != -1)
                {
                    this.viplogin();
                }
                else
                {
                    string indata = "domain=sina.cn&logintype=uid&u=" + base.m_username + "&domain=" + base.m_serv + "&psw=" + base.m_passwd + "&savelogin=&btnloginfree=%B5%C7+%C2%BC";
                    this.url = "http://mail.sina.com.cn/cgi-bin/login.cgi";
                    base.ShowMessage("开始登陆…………");
                    base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                    base.streamControl = true;
                    base.MyStringBuilder = this.PostData(this.url, indata);
                    startIndex = base.MyStringBuilder.ToString().IndexOf("http://mail.sina.com.cn/freemail/loginbefore/css/login_error.css");
                    if (startIndex == -1)
                    {
                        startIndex = this.urls.IndexOf("check_time=");
                        if (startIndex != -1)
                        {
                            this.urls = this.urls + " ";
                            this.checkTime = base.putstr(this.urls, "=", " ", startIndex);
                            base.Host = this.urls.Substring(0, this.urls.IndexOf("classic"));
                            if ((base.MyStringBuilder.ToString() != "") && (base.MyStringBuilder.ToString().IndexOf(base.m_username + "@" + base.m_serv) != -1))
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
                request.Accept = "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, application/x-ms-application, application/x-ms-xbap, application/vnd.ms-xpsdocument, application/xaml+xml, */*";
                request.Headers.Add("Accept-Language: en-us,ar-SA;q=0.9,de-DE;q=0.8,es-ES;q=0.7,tr-TR;q=0.6,ja-JP;q=0.5,en-GB;q=0.4,fr-FR;q=0.3,zh-CN;q=0.2,zh-TW;q=0.1");
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; EmbeddedWB 14.52 from: http://www.bsalsa.com/ EmbeddedWB 14.52; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022)";
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
                    reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8"));
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
                    reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8"));
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
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Headers.Add("Cookie", base.cookie);
                request.Method = "GET";
                request.Accept = "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, application/x-ms-application, application/x-ms-xbap, application/vnd.ms-xpsdocument, application/xaml+xml, */*";
                request.AllowAutoRedirect = false;
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1)";
                request.ContentType = "application/octet-stream";
                request.KeepAlive = true;
                request.Referer = "http://mail234-233.sinamail.sina.com.cn/uc/gettpl.php?t=readmail&ts=1251769957639_2";
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
                    GlobalValue.PopMainForm.statusBarPanelEmailNumbersText.Text = (Convert.ToInt32(GlobalValue.PopMainForm.statusBarPanelEmailNumbersText.Text) + 1).ToString();
                    base.streamControl = false;
                    return true;
                }
                responseStream.Close();
                return false;
            }
            catch (Exception exception)
            {
                base.ShowMessage(exception.Message);
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
            Stream stream = File.Create(path + @"\" + base.m_username + "address.csv");
            using (StreamWriter writer = new StreamWriter(stream, Encoding.UTF8))
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
        public string U2CnCode(string str)
        {
            Regex regex = new Regex(@"(?<code>\\u[a-z0-9]{4})", RegexOptions.IgnoreCase);
            for (Match match = regex.Match(str); match.Success; match = match.NextMatch())
            {
                string oldValue = match.Result("${code}");
                int num = int.Parse(oldValue.Substring(2, 4), NumberStyles.HexNumber);
                string newValue = string.Format("{0}", (char)num);
                str = str.Replace(oldValue, newValue);
            }
            return str;
        }
        public void viplogin()
        {
            try
            {
                base.cookie = "";
                int startIndex = 0;
                string indata = "u=" + base.m_username + "&domain=" + base.m_serv + "&psw=" + base.m_passwd;
                this.url = "http://vip.sina.com.cn/cgi-bin/login.php";
                base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
                base.streamControl = true;
                base.MyStringBuilder = this.PostData(this.url, indata);
                startIndex = base.MyStringBuilder.ToString().IndexOf("loginerror_error_info");
                if (startIndex == -1)
                {
                    startIndex = base.MyStringBuilder.ToString().IndexOf("check_time=");
                    if (startIndex != -1)
                    {
                        this.checkTime = base.putstr(base.MyStringBuilder.ToString(), "=", "\"", startIndex);
                        base.Host = this.urls.Substring(0, this.urls.IndexOf("classic"));
                        if ((base.MyStringBuilder.ToString() != "") && (base.MyStringBuilder.ToString().IndexOf(base.m_username + "@" + base.m_serv) != -1))
                        {
                            base.ShowMessage("登陆成功…………");
                            this.getvipBoxName();
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
            catch (Exception exception)
            {
                base.LoginFail();
                base.passwdErr();
                base.ShowMessage("登陆失败…………" + exception.Message);
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
