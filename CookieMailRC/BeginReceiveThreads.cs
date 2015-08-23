using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MailExposure.CookieMailRC
{
    class BeginReceiveThreads
    {
        private string m_emailUri;
        private int m_Mailno;
        private int m_NO;
        private string m_PassWd;
        private string m_Servaddr;
        private string m_Server;
        private string m_sNote;
        private string m_sType;
        private string m_userListName;
        private string m_UserName;
        private string m_UserType;
        private string m_validationLogin;
        public const int SERVER_126MAIL = 9;
        public const int SERVER_163MAIL = 8;
        public const int SERVER_21CNMAIL = 0x11;
        public const int SERVER_263MAIL = 11;
        public const int SERVER_AOLMAIL = 0x12;
        public const int SERVER_FASTMAIL = 14;
        public const int SERVER_GMAIL = 3;
        public const int SERVER_HINETMAIL = 7;
        public const int SERVER_HOTMAIL = 2;
        public const int SERVER_lIVEMAIL = 6;
        public const int SERVER_POPMAIL = 5;
        public const int SERVER_RuMAIL = 4;
        public const int SERVER_SINAMAIL = 0x10;
        public const int SERVER_SOHUMAIL = 10;
        public const int SERVER_TOMMAIL = 15;
        public const int SERVER_YAHOO = 1;

        // Methods
        public BeginReceiveThreads(int NO, string server, string seraddr, string username, string passwd, string snote, string stype, int mailno, string emailuri, string validationLogin, string usertype, string userListName)
        {
            this.m_NO = NO;
            this.m_Server = server;
            this.m_Servaddr = seraddr;
            this.m_UserName = username;
            this.m_PassWd = passwd;
            this.m_sNote = snote;
            this.m_sType = stype;
            this.m_Mailno = mailno;
            this.m_UserType = usertype;
            this.m_validationLogin = validationLogin;
            this.m_emailUri = emailuri;
            this.m_userListName = userListName;

        }
        public void Execute()
        {
            switch (this.GetServType(this.m_Server))
            {
                case 1:
                    {
                        JaPanYahooMailReceiver receiver5 = new JaPanYahooMailReceiver();
                        receiver5.m_NO = this.m_NO;
                        receiver5.m_username = this.m_UserName;
                        receiver5.m_passwd = this.m_PassWd;
                        receiver5.m_snote = this.m_sNote;
                        receiver5.m_stype = this.m_sType;
                        receiver5.m_emailno = this.m_Mailno;
                        receiver5.m_serv = this.m_Servaddr;
                        receiver5.mailType = "Yahoo邮件";
                        receiver5.server = "Y";
                        receiver5.listid = "YaHooID";
                        receiver5.validationLogin = this.m_validationLogin;
                        receiver5.emailuri = this.m_emailUri;
                        receiver5.m_UserType = this.m_UserType;
                        receiver5.m_userListName = this.m_userListName;
                        receiver5.START();
                        return;
                    }
                case 2:
                    {
                        HotMailReceiver receiver6 = new HotMailReceiver();
                        receiver6.m_NO = this.m_NO;
                        receiver6.m_username = this.m_UserName;
                        receiver6.m_passwd = this.m_PassWd;
                        receiver6.m_snote = this.m_sNote;
                        receiver6.m_stype = this.m_sType;
                        receiver6.m_emailno = this.m_Mailno;
                        receiver6.m_serv = this.m_Servaddr;
                        receiver6.server = "H";
                        receiver6.listid = "HotMailID";
                        receiver6.validationLogin = this.m_validationLogin;
                        receiver6.emailuri = this.m_emailUri;
                        receiver6.m_UserType = this.m_UserType;
                        receiver6.m_userListName = this.m_userListName;
                        receiver6.START();
                        return;
                    }
                case 3:
                    {
                        GmailReceiver receiver2 = new GmailReceiver();
                        receiver2.m_NO = this.m_NO;
                        receiver2.m_username = this.m_UserName;
                        receiver2.m_passwd = this.m_PassWd;
                        receiver2.m_snote = this.m_sNote;
                        receiver2.m_stype = this.m_sType;
                        receiver2.m_emailno = this.m_Mailno;
                        receiver2.m_serv = this.m_Servaddr;
                        receiver2.mailType = "GMail邮件";
                        receiver2.server = "G";
                        receiver2.listid = "GmailID";
                        receiver2.validationLogin = this.m_validationLogin;
                        receiver2.emailuri = this.m_emailUri;
                        receiver2.m_UserType = this.m_UserType;
                        receiver2.m_userListName = this.m_userListName;
                        receiver2.START();
                        return;
                    }
                case 4:
                    {
                        RuReceiver receiver4 = new RuReceiver();
                        receiver4.m_NO = this.m_NO;
                        receiver4.m_username = this.m_UserName;
                        receiver4.m_passwd = this.m_PassWd;
                        receiver4.m_snote = this.m_sNote;
                        receiver4.m_stype = this.m_sType;
                        receiver4.m_emailno = this.m_Mailno;
                        receiver4.m_serv = this.m_Servaddr;
                        receiver4.mailType = "Ru邮件";
                        receiver4.server = "RU";
                        receiver4.listid = "RuID";
                        receiver4.validationLogin = this.m_validationLogin;
                        receiver4.emailuri = this.m_emailUri;
                        receiver4.m_UserType = this.m_UserType;
                        receiver4.m_userListName = this.m_userListName;
                        receiver4.START();
                        return;
                    }
                case 5:
                    {
                        POPMailReceiver receiver = new POPMailReceiver();
                        receiver.m_NO = this.m_NO;
                        receiver.m_username = this.m_UserName;
                        receiver.m_passwd = this.m_PassWd;
                        receiver.m_snote = this.m_sNote;
                        receiver.m_stype = this.m_sType;
                        receiver.m_emailno = this.m_Mailno;
                        receiver.m_serv = this.m_Servaddr;
                        receiver.mailType = "POP邮件";
                        receiver.BoxName = "收件箱";
                        receiver.server = "POP";
                        receiver.listid = this.m_Servaddr;
                        receiver.START();
                        return;
                    }
                case 6:
                case 12:
                case 13:
                    break;

                case 7:
                    {
                        HiNetReceiver receiver3 = new HiNetReceiver();
                        receiver3.m_NO = this.m_NO;
                        receiver3.m_username = this.m_UserName;
                        receiver3.m_passwd = this.m_PassWd;
                        receiver3.m_snote = this.m_sNote;
                        receiver3.m_stype = this.m_sType;
                        receiver3.m_emailno = this.m_Mailno;
                        receiver3.m_serv = this.m_Servaddr;
                        receiver3.mailType = "HiNet邮件";
                        receiver3.server = "HN";
                        receiver3.listid = "HiNetID";
                        receiver3.validationLogin = this.m_validationLogin;
                        receiver3.emailuri = this.m_emailUri;
                        receiver3.m_UserType = this.m_UserType;
                        receiver3.m_userListName = this.m_userListName;
                        receiver3.START();
                        return;
                    }
                case 8:
                    {
                        Mail163Receiver receiver7 = new Mail163Receiver();
                        receiver7.m_NO = this.m_NO;
                        receiver7.m_username = this.m_UserName;
                        receiver7.m_passwd = this.m_PassWd;
                        receiver7.m_snote = this.m_sNote;
                        receiver7.m_stype = this.m_sType;
                        receiver7.m_emailno = this.m_Mailno;
                        receiver7.m_serv = this.m_Servaddr;
                        receiver7.mailType = "163邮件";
                        receiver7.server = "163";
                        receiver7.listid = "163ID";
                        receiver7.validationLogin = this.m_validationLogin;
                        receiver7.emailuri = this.m_emailUri;
                        receiver7.m_UserType = this.m_UserType;
                        receiver7.m_userListName = this.m_userListName;
                        receiver7.START();
                        return;
                    }
                case 9:
                    {
                        Mail126Receiver receiver8 = new Mail126Receiver();
                        receiver8.m_NO = this.m_NO;
                        receiver8.m_username = this.m_UserName;
                        receiver8.m_passwd = this.m_PassWd;
                        receiver8.m_snote = this.m_sNote;
                        receiver8.m_stype = this.m_sType;
                        receiver8.m_emailno = this.m_Mailno;
                        receiver8.m_serv = this.m_Servaddr;
                        receiver8.mailType = "126邮件";
                        receiver8.server = "126";
                        receiver8.listid = "126ID";
                        receiver8.validationLogin = this.m_validationLogin;
                        receiver8.emailuri = this.m_emailUri;
                        receiver8.m_UserType = this.m_UserType;
                        receiver8.m_userListName = this.m_userListName;
                        receiver8.START();
                        return;
                    }
                case 10:
                    {
                        SoHuMailReceiver receiver9 = new SoHuMailReceiver();
                        receiver9.m_NO = this.m_NO;
                        receiver9.m_username = this.m_UserName;
                        receiver9.m_passwd = this.m_PassWd;
                        receiver9.m_snote = this.m_sNote;
                        receiver9.m_stype = this.m_sType;
                        receiver9.m_emailno = this.m_Mailno;
                        receiver9.m_serv = this.m_Servaddr;
                        receiver9.mailType = "SoHu邮件";
                        receiver9.server = "SoHu";
                        receiver9.listid = "SoHuID";
                        receiver9.validationLogin = this.m_validationLogin;
                        receiver9.emailuri = this.m_emailUri;
                        receiver9.m_UserType = this.m_UserType;
                        receiver9.m_userListName = this.m_userListName;
                        receiver9.START();
                        return;
                    }
                case 11:
                    {
                        Mail263Receiver receiver10 = new Mail263Receiver();
                        receiver10.m_NO = this.m_NO;
                        receiver10.m_username = this.m_UserName;
                        receiver10.m_passwd = this.m_PassWd;
                        receiver10.m_snote = this.m_sNote;
                        receiver10.m_stype = this.m_sType;
                        receiver10.m_emailno = this.m_Mailno;
                        receiver10.m_serv = this.m_Servaddr;
                        receiver10.mailType = "263邮件";
                        receiver10.server = "263";
                        receiver10.listid = "263ID";
                        receiver10.validationLogin = this.m_validationLogin;
                        receiver10.emailuri = this.m_emailUri;
                        receiver10.m_UserType = this.m_UserType;
                        receiver10.m_userListName = this.m_userListName;
                        receiver10.START();
                        return;
                    }
                case 14:
                    {
                        FastMailReceiver receiver11 = new FastMailReceiver();
                        receiver11.m_NO = this.m_NO;
                        receiver11.m_username = this.m_UserName;
                        receiver11.m_passwd = this.m_PassWd;
                        receiver11.m_snote = this.m_sNote;
                        receiver11.m_stype = this.m_sType;
                        receiver11.m_emailno = this.m_Mailno;
                        receiver11.m_serv = this.m_Servaddr;
                        receiver11.mailType = "Fastmail邮件";
                        receiver11.server = "Fastmail";
                        receiver11.listid = "FastmailID";
                        receiver11.validationLogin = this.m_validationLogin;
                        receiver11.emailuri = this.m_emailUri;
                        receiver11.m_UserType = this.m_UserType;
                        receiver11.m_userListName = this.m_userListName;
                        receiver11.START();
                        return;
                    }
                case 15:
                    {
                        TomMailReceiver receiver12 = new TomMailReceiver();
                        receiver12.m_NO = this.m_NO;
                        receiver12.m_username = this.m_UserName;
                        receiver12.m_passwd = this.m_PassWd;
                        receiver12.m_snote = this.m_sNote;
                        receiver12.m_stype = this.m_sType;
                        receiver12.m_emailno = this.m_Mailno;
                        receiver12.m_serv = this.m_Servaddr;
                        receiver12.mailType = "Tommail邮件";
                        receiver12.server = "Tommail";
                        receiver12.listid = "TommailID";
                        receiver12.validationLogin = this.m_validationLogin;
                        receiver12.emailuri = this.m_emailUri;
                        receiver12.m_UserType = this.m_UserType;
                        receiver12.m_userListName = this.m_userListName;
                        receiver12.START();
                        return;
                    }
                case 0x10:
                    {
                        SinaMailReceiver receiver13 = new SinaMailReceiver();
                        receiver13.m_NO = this.m_NO;
                        receiver13.m_username = this.m_UserName;
                        receiver13.m_passwd = this.m_PassWd;
                        receiver13.m_snote = this.m_sNote;
                        receiver13.m_stype = this.m_sType;
                        receiver13.m_emailno = this.m_Mailno;
                        receiver13.m_serv = this.m_Servaddr;
                        receiver13.mailType = "Sinamail邮件";
                        receiver13.server = "Sinamail";
                        receiver13.listid = "SinamailID";
                        receiver13.validationLogin = this.m_validationLogin;
                        receiver13.emailuri = this.m_emailUri;
                        receiver13.m_UserType = this.m_UserType;
                        receiver13.m_userListName = this.m_userListName;
                        receiver13.START();
                        return;
                    }
                case 0x11:
                    {
                        CNMailReceiver receiver14 = new CNMailReceiver();
                        receiver14.m_NO = this.m_NO;
                        receiver14.m_username = this.m_UserName;
                        receiver14.m_passwd = this.m_PassWd;
                        receiver14.m_snote = this.m_sNote;
                        receiver14.m_stype = this.m_sType;
                        receiver14.m_emailno = this.m_Mailno;
                        receiver14.m_serv = this.m_Servaddr;
                        receiver14.mailType = "21CNmail邮件";
                        receiver14.server = "21CNmail";
                        receiver14.listid = "21CNmailID";
                        receiver14.validationLogin = this.m_validationLogin;
                        receiver14.emailuri = this.m_emailUri;
                        receiver14.m_UserType = this.m_UserType;
                        receiver14.m_userListName = this.m_userListName;
                        receiver14.START();
                        return;
                    }
                case 0x12:
                    {
                        AolReceiver receiver15 = new AolReceiver();
                        receiver15.m_NO = this.m_NO;
                        receiver15.m_username = this.m_UserName;
                        receiver15.m_passwd = this.m_PassWd;
                        receiver15.m_snote = this.m_sNote;
                        receiver15.m_stype = this.m_sType;
                        receiver15.m_emailno = this.m_Mailno;
                        receiver15.m_serv = this.m_Servaddr;
                        receiver15.mailType = "Aol邮件";
                        receiver15.server = "Aol";
                        receiver15.listid = "AolID";
                        receiver15.validationLogin = this.m_validationLogin;
                        receiver15.emailuri = this.m_emailUri;
                        receiver15.m_UserType = this.m_UserType;
                        receiver15.m_userListName = this.m_userListName;
                        receiver15.START();
                        break;
                    }
                default:
                    return;
            }

        }
        private int GetServType(string server)
        {
            if (server.Trim().ToUpper() == "G")
            {
                return 3;
            }
            if (server.Trim().ToUpper() == "HN")
            {
                return 7;
            }
            if (server.Trim().ToUpper() == "RU")
            {
                return 4;
            }
            if (server.Trim().ToUpper() == "Y")
            {
                return 1;
            }
            if (server.Trim().ToUpper() == "H")
            {
                return 2;
            }
            if (server.Trim() == "163")
            {
                return 8;
            }
            if (server.Trim() == "126")
            {
                return 9;
            }
            if (server.Trim().ToUpper() == "SOHU")
            {
                return 10;
            }
            if (server.Trim() == "263")
            {
                return 11;
            }
            if (server.Trim().ToUpper() == "FASTMAIL")
            {
                return 14;
            }
            if (server.Trim().ToUpper() == "TOMMAIL")
            {
                return 15;
            }
            if (server.Trim().ToUpper() == "SINAMAIL")
            {
                return 0x10;
            }
            if (server.Trim().ToUpper() == "21CNMAIL")
            {
                return 0x11;
            }
            if (server.Trim().ToUpper() == "AOL")
            {
                return 0x12;
            }
            return 5;

        }//Imp future
    }
}
