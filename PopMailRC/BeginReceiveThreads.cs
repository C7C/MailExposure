using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MailExposure.PopMailRC
{
    class BeginReceiveThreads
    {
        // Fields
        private string m_emailUri;
        private int m_Mailno;
        private int m_NO;
        private string m_PassWd;
        private string m_Servaddr;
        private string m_Server;
        private string m_sNote;
        private string m_sType;
        private string m_UserName;
        private string m_UserType;
        private string m_validationLogin;
        public const int SERVER_126MAIL = 9;
        public const int SERVER_163MAIL = 8;
        public const int SERVER_21CNMAIL = 0x12;
        public const int SERVER_263MAIL = 13;
        public const int SERVER_AOLMAIL = 0x16;
        public const int SERVER_CTMMAIL = 11;
        public const int SERVER_FASTMAIL = 14;
        public const int SERVER_FREEPOPS = 12;
        public const int SERVER_GMAIL = 3;
        public const int SERVER_HANMAIL = 0x13;
        public const int SERVER_HINETMAIL = 7;
        public const int SERVER_HOTMAIL = 2;
        public const int SERVER_HUSHMAIL = 15;
        public const int SERVER_IMAPMAIL = 0x15;
        public const int SERVER_lIVEMAIL = 6;
        public const int SERVER_MAILHN = 0x19;
        public const int SERVER_POPMAIL = 5;
        public const int SERVER_QQMAIL = 20;
        public const int SERVER_RAMBLERMAIL = 0x1c;
        public const int SERVER_REDIFFMAIL = 0x1a;
        public const int SERVER_RuMAIL = 4;
        public const int SERVER_SINAMAIL = 0x11;
        public const int SERVER_SOHUMAIL = 10;
        public const int SERVER_TOMMAIL = 0x10;
        public const int SERVER_VIP163MAIL = 0x17;
        public const int SERVER_YAHOO = 1;
        public const int SERVER_YANDEXMAIL = 0x1b;
        public const int SERVER_YEAHMAIL = 0x18;

        // Methods
        public BeginReceiveThreads(int NO, string server, string seraddr, string username, string passwd, string snote, string stype, int mailno, string emailuri, string validationLogin, string usertype)
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

        }
        public void Execute()
        {
            switch (this.GetServType(this.m_Server))
            {
                case 1:
                    {
                        JaPanYahooMailReceiver receiver6 = new JaPanYahooMailReceiver();
                        receiver6.m_NO = this.m_NO;
                        receiver6.m_username = this.m_UserName;
                        receiver6.m_passwd = this.m_PassWd;
                        receiver6.m_snote = this.m_sNote;
                        receiver6.m_stype = this.m_sType;
                        receiver6.m_emailno = this.m_Mailno;
                        receiver6.m_serv = this.m_Servaddr;
                        receiver6.mailType = "Yahoo邮件";
                        receiver6.server = "Yahoo";
                        receiver6.listid = "YaHooID";
                        receiver6.validationLogin = this.m_validationLogin;
                        receiver6.emailuri = this.m_emailUri;
                        receiver6.m_UserType = this.m_UserType;
                        receiver6.START();
                        return;
                    }
                case 2:
                    {
                        HotMailReceiver receiver7 = new HotMailReceiver();
                        receiver7.m_NO = this.m_NO;
                        receiver7.m_username = this.m_UserName;
                        receiver7.m_passwd = this.m_PassWd;
                        receiver7.m_snote = this.m_sNote;
                        receiver7.m_stype = this.m_sType;
                        receiver7.m_emailno = this.m_Mailno;
                        receiver7.m_serv = this.m_Servaddr;
                        receiver7.server = "Hotmail";
                        receiver7.listid = "HotMailID";
                        receiver7.validationLogin = this.m_validationLogin;
                        receiver7.emailuri = this.m_emailUri;
                        receiver7.m_UserType = this.m_UserType;
                        receiver7.START();
                        return;
                    }
                case 3:
                    {
                        GmailReceiver receiver3 = new GmailReceiver();
                        receiver3.m_NO = this.m_NO;
                        receiver3.m_username = this.m_UserName;
                        receiver3.m_passwd = this.m_PassWd;
                        receiver3.m_snote = this.m_sNote;
                        receiver3.m_stype = this.m_sType;
                        receiver3.m_emailno = this.m_Mailno;
                        receiver3.m_serv = this.m_Servaddr;
                        receiver3.mailType = "GMail邮件";
                        receiver3.server = "Gmail";
                        receiver3.listid = "GmailID";
                        receiver3.validationLogin = this.m_validationLogin;
                        receiver3.emailuri = this.m_emailUri;
                        receiver3.m_UserType = this.m_UserType;
                        receiver3.START();
                        return;
                    }
                case 4:
                    {
                        RuReceiver receiver5 = new RuReceiver();
                        receiver5.m_NO = this.m_NO;
                        receiver5.m_username = this.m_UserName;
                        receiver5.m_passwd = this.m_PassWd;
                        receiver5.m_snote = this.m_sNote;
                        receiver5.m_stype = this.m_sType;
                        receiver5.m_emailno = this.m_Mailno;
                        receiver5.m_serv = this.m_Servaddr;
                        receiver5.mailType = "Ru邮件";
                        receiver5.server = "Ru";
                        receiver5.listid = "RuID";
                        receiver5.START();
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
                    break;

                case 7:
                    {
                        HiNetReceiver receiver4 = new HiNetReceiver();
                        receiver4.m_NO = this.m_NO;
                        receiver4.m_username = this.m_UserName;
                        receiver4.m_passwd = this.m_PassWd;
                        receiver4.m_snote = this.m_sNote;
                        receiver4.m_stype = this.m_sType;
                        receiver4.m_emailno = this.m_Mailno;
                        receiver4.m_serv = this.m_Servaddr;
                        receiver4.mailType = "HiNet邮件";
                        receiver4.server = "Hinet";
                        receiver4.listid = "HiNetID";
                        receiver4.START();
                        return;
                    }
                case 8:
                    {
                        Mail163Receiver receiver8 = new Mail163Receiver();
                        receiver8.m_NO = this.m_NO;
                        receiver8.m_username = this.m_UserName;
                        receiver8.m_passwd = this.m_PassWd;
                        receiver8.m_snote = this.m_sNote;
                        receiver8.m_stype = this.m_sType;
                        receiver8.m_emailno = this.m_Mailno;
                        receiver8.m_serv = this.m_Servaddr;
                        receiver8.mailType = "163邮件";
                        receiver8.server = "163";
                        receiver8.listid = "163ID";
                        receiver8.validationLogin = this.m_validationLogin;
                        receiver8.emailuri = this.m_emailUri;
                        receiver8.m_UserType = this.m_UserType;
                        receiver8.START();
                        return;
                    }
                case 9:
                    {
                        Mail126Receiver receiver10 = new Mail126Receiver();
                        receiver10.m_NO = this.m_NO;
                        receiver10.m_username = this.m_UserName;
                        receiver10.m_passwd = this.m_PassWd;
                        receiver10.m_snote = this.m_sNote;
                        receiver10.m_stype = this.m_sType;
                        receiver10.m_emailno = this.m_Mailno;
                        receiver10.m_serv = this.m_Servaddr;
                        receiver10.mailType = "126邮件";
                        receiver10.server = "126";
                        receiver10.listid = "126ID";
                        receiver10.validationLogin = this.m_validationLogin;
                        receiver10.emailuri = this.m_emailUri;
                        receiver10.m_UserType = this.m_UserType;
                        receiver10.START();
                        return;
                    }
                case 10:
                    {
                        SoHuMailReceiver receiver12 = new SoHuMailReceiver();
                        receiver12.m_NO = this.m_NO;
                        receiver12.m_username = this.m_UserName;
                        receiver12.m_passwd = this.m_PassWd;
                        receiver12.m_snote = this.m_sNote;
                        receiver12.m_stype = this.m_sType;
                        receiver12.m_emailno = this.m_Mailno;
                        receiver12.m_serv = this.m_Servaddr;
                        receiver12.mailType = "SoHu邮件";
                        receiver12.server = "SoHu";
                        receiver12.listid = "SoHuID";
                        receiver12.validationLogin = this.m_validationLogin;
                        receiver12.emailuri = this.m_emailUri;
                        receiver12.m_UserType = this.m_UserType;
                        receiver12.START();
                        return;
                    }
                case 11:
                    {
                        CtmMailReceiver receiver13 = new CtmMailReceiver();
                        receiver13.m_NO = this.m_NO;
                        receiver13.m_username = this.m_UserName;
                        receiver13.m_passwd = this.m_PassWd;
                        receiver13.m_snote = this.m_sNote;
                        receiver13.m_stype = this.m_sType;
                        receiver13.m_emailno = this.m_Mailno;
                        receiver13.m_serv = this.m_Servaddr;
                        receiver13.mailType = "CTM邮件";
                        receiver13.server = "CTM";
                        receiver13.listid = "CTMID";
                        receiver13.START();
                        return;
                    }
                case 12:
                    {
                        FreePOPS epops = new FreePOPS();
                        epops.m_NO = this.m_NO;
                        epops.m_username = this.m_UserName;
                        epops.m_passwd = this.m_PassWd;
                        epops.m_snote = this.m_sNote;
                        epops.m_stype = this.m_sType;
                        epops.m_emailno = this.m_Mailno;
                        epops.m_serv = this.m_Servaddr;
                        epops.mailType = "FreePOPS邮件";
                        epops.server = "FreePOPS";
                        epops.listid = "FreePOPSID";
                        epops.START();
                        return;
                    }
                case 13:
                    {
                        Mail263Receiver receiver14 = new Mail263Receiver();
                        receiver14.m_NO = this.m_NO;
                        receiver14.m_username = this.m_UserName;
                        receiver14.m_passwd = this.m_PassWd;
                        receiver14.m_snote = this.m_sNote;
                        receiver14.m_stype = this.m_sType;
                        receiver14.m_emailno = this.m_Mailno;
                        receiver14.m_serv = this.m_Servaddr;
                        receiver14.mailType = "263邮件";
                        receiver14.server = "263";
                        receiver14.listid = "263ID";
                        receiver14.START();
                        return;
                    }
                case 14:
                    {
                        FastMailReceiver receiver15 = new FastMailReceiver();
                        receiver15.m_NO = this.m_NO;
                        receiver15.m_username = this.m_UserName;
                        receiver15.m_passwd = this.m_PassWd;
                        receiver15.m_snote = this.m_sNote;
                        receiver15.m_stype = this.m_sType;
                        receiver15.m_emailno = this.m_Mailno;
                        receiver15.m_serv = this.m_Servaddr;
                        receiver15.mailType = "FastMail邮件";
                        receiver15.server = "FastMail";
                        receiver15.listid = "FastMailID";
                        receiver15.START();
                        return;
                    }
                case 15:
                    {
                        HushMailReceiver receiver16 = new HushMailReceiver();
                        receiver16.m_NO = this.m_NO;
                        receiver16.m_username = this.m_UserName;
                        receiver16.m_passwd = this.m_PassWd;
                        receiver16.m_snote = this.m_sNote;
                        receiver16.m_stype = this.m_sType;
                        receiver16.m_emailno = this.m_Mailno;
                        receiver16.m_serv = this.m_Servaddr;
                        receiver16.mailType = "HushMail邮件";
                        receiver16.server = "HushMail";
                        receiver16.listid = "HushMailID";
                        receiver16.START();
                        return;
                    }
                case 0x10:
                    {
                        TomMailReceiver receiver17 = new TomMailReceiver();
                        receiver17.m_NO = this.m_NO;
                        receiver17.m_username = this.m_UserName;
                        receiver17.m_passwd = this.m_PassWd;
                        receiver17.m_snote = this.m_sNote;
                        receiver17.m_stype = this.m_sType;
                        receiver17.m_emailno = this.m_Mailno;
                        receiver17.m_serv = this.m_Servaddr;
                        receiver17.mailType = "TomMail邮件";
                        receiver17.server = "Tom";
                        receiver17.listid = "TomMailID";
                        receiver17.START();
                        return;
                    }
                case 0x11:
                    {
                        SinaMailReceiver receiver18 = new SinaMailReceiver();
                        receiver18.m_NO = this.m_NO;
                        receiver18.m_username = this.m_UserName;
                        receiver18.m_passwd = this.m_PassWd;
                        receiver18.m_snote = this.m_sNote;
                        receiver18.m_stype = this.m_sType;
                        receiver18.m_emailno = this.m_Mailno;
                        receiver18.m_serv = this.m_Servaddr;
                        receiver18.mailType = "SinaMail邮件";
                        receiver18.server = "Sina";
                        receiver18.listid = "SinaMailID";
                        receiver18.START();
                        return;
                    }
                case 0x12:
                    {
                        CNMailReceiver receiver19 = new CNMailReceiver();
                        receiver19.m_NO = this.m_NO;
                        receiver19.m_username = this.m_UserName;
                        receiver19.m_passwd = this.m_PassWd;
                        receiver19.m_snote = this.m_sNote;
                        receiver19.m_stype = this.m_sType;
                        receiver19.m_emailno = this.m_Mailno;
                        receiver19.m_serv = this.m_Servaddr;
                        receiver19.mailType = "21CNMail邮件";
                        receiver19.server = "21CN";
                        receiver19.listid = "21CNMailID";
                        receiver19.START();
                        return;
                    }
                case 0x13:
                    {
                        HanMailReceiver receiver20 = new HanMailReceiver();
                        receiver20.m_NO = this.m_NO;
                        receiver20.m_username = this.m_UserName;
                        receiver20.m_passwd = this.m_PassWd;
                        receiver20.m_snote = this.m_sNote;
                        receiver20.m_stype = this.m_sType;
                        receiver20.m_emailno = this.m_Mailno;
                        receiver20.m_serv = this.m_Servaddr;
                        receiver20.mailType = "HanMail邮件";
                        receiver20.server = "HANMAIL";
                        receiver20.listid = "HanMailID";
                        receiver20.START();
                        return;
                    }
                case 20:
                    {
                        QQMailReceiver receiver21 = new QQMailReceiver();
                        receiver21.m_NO = this.m_NO;
                        receiver21.m_username = this.m_UserName;
                        receiver21.m_passwd = this.m_PassWd;
                        receiver21.m_snote = this.m_sNote;
                        receiver21.m_stype = this.m_sType;
                        receiver21.m_emailno = this.m_Mailno;
                        receiver21.m_serv = this.m_Servaddr;
                        receiver21.mailType = "QQMail邮件";
                        receiver21.server = "QQ";
                        receiver21.listid = "QQMailID";
                        receiver21.START();
                        return;
                    }
                case 0x15:
                    {
                        IMAPMailReceiver receiver2 = new IMAPMailReceiver();
                        receiver2.m_NO = this.m_NO;
                        receiver2.m_username = this.m_UserName;
                        receiver2.m_passwd = this.m_PassWd;
                        receiver2.m_snote = this.m_sNote;
                        receiver2.m_stype = this.m_sType;
                        receiver2.m_emailno = this.m_Mailno;
                        receiver2.m_serv = this.m_Servaddr;
                        receiver2.mailType = "IMAP邮件";
                        receiver2.BoxName = "收件箱";
                        receiver2.server = "IMAP";
                        receiver2.listid = this.m_Servaddr;
                        receiver2.START();
                        return;
                    }
                case 0x16:
                    {
                        MailAolReceiver receiver22 = new MailAolReceiver();
                        receiver22.m_NO = this.m_NO;
                        receiver22.m_username = this.m_UserName;
                        receiver22.m_passwd = this.m_PassWd;
                        receiver22.m_snote = this.m_sNote;
                        receiver22.m_stype = this.m_sType;
                        receiver22.m_emailno = this.m_Mailno;
                        receiver22.m_serv = this.m_Servaddr;
                        receiver22.mailType = "AolMail邮件";
                        receiver22.server = "Aol";
                        receiver22.listid = "AolMailID";
                        receiver22.START();
                        return;
                    }
                case 0x17:
                    {
                        MailVip163Receiver receiver9 = new MailVip163Receiver();
                        receiver9.m_NO = this.m_NO;
                        receiver9.m_username = this.m_UserName;
                        receiver9.m_passwd = this.m_PassWd;
                        receiver9.m_snote = this.m_sNote;
                        receiver9.m_stype = this.m_sType;
                        receiver9.m_emailno = this.m_Mailno;
                        receiver9.m_serv = this.m_Servaddr;
                        receiver9.mailType = "VIP163邮件";
                        receiver9.server = "VIP163";
                        receiver9.listid = "VIP163ID";
                        receiver9.validationLogin = this.m_validationLogin;
                        receiver9.emailuri = this.m_emailUri;
                        receiver9.m_UserType = this.m_UserType;
                        receiver9.START();
                        return;
                    }
                case 0x18:
                    {
                        YeahMailReceiver receiver11 = new YeahMailReceiver();
                        receiver11.m_NO = this.m_NO;
                        receiver11.m_username = this.m_UserName;
                        receiver11.m_passwd = this.m_PassWd;
                        receiver11.m_snote = this.m_sNote;
                        receiver11.m_stype = this.m_sType;
                        receiver11.m_emailno = this.m_Mailno;
                        receiver11.m_serv = this.m_Servaddr;
                        receiver11.mailType = "YEAH邮件";
                        receiver11.server = "Yeah";
                        receiver11.listid = "YeahID";
                        receiver11.validationLogin = this.m_validationLogin;
                        receiver11.emailuri = this.m_emailUri;
                        receiver11.m_UserType = this.m_UserType;
                        receiver11.START();
                        return;
                    }
                case 0x19:
                    {
                        MailHNReceiver receiver23 = new MailHNReceiver();
                        receiver23.m_NO = this.m_NO;
                        receiver23.m_username = this.m_UserName;
                        receiver23.m_passwd = this.m_PassWd;
                        receiver23.m_snote = this.m_sNote;
                        receiver23.m_stype = this.m_sType;
                        receiver23.m_emailno = this.m_Mailno;
                        receiver23.m_serv = this.m_Servaddr;
                        receiver23.mailType = "MailHN邮件";
                        receiver23.server = "MailHN";
                        receiver23.listid = "MailHNID";
                        receiver23.START();
                        return;
                    }
                case 0x1a:
                    {
                        MailRediffReceiver receiver24 = new MailRediffReceiver();
                        receiver24.m_NO = this.m_NO;
                        receiver24.m_username = this.m_UserName;
                        receiver24.m_passwd = this.m_PassWd;
                        receiver24.m_snote = this.m_sNote;
                        receiver24.m_stype = this.m_sType;
                        receiver24.m_emailno = this.m_Mailno;
                        receiver24.m_serv = this.m_Servaddr;
                        receiver24.mailType = "RediffMail邮件";
                        receiver24.server = "RediffMail";
                        receiver24.listid = "RediffMailID";
                        receiver24.START();
                        return;
                    }
                case 0x1b:
                    {
                        MailYandexRuReceiver receiver26 = new MailYandexRuReceiver();
                        receiver26.m_NO = this.m_NO;
                        receiver26.m_username = this.m_UserName;
                        receiver26.m_passwd = this.m_PassWd;
                        receiver26.m_snote = this.m_sNote;
                        receiver26.m_stype = this.m_sType;
                        receiver26.m_emailno = this.m_Mailno;
                        receiver26.m_serv = this.m_Servaddr;
                        receiver26.mailType = "Yandex邮件";
                        receiver26.server = "Yandex";
                        receiver26.listid = "YandexID";
                        receiver26.START();
                        break;
                    }
                case 0x1c:
                    {
                        MailRamblerRuReceiver receiver25 = new MailRamblerRuReceiver();
                        receiver25.m_NO = this.m_NO;
                        receiver25.m_username = this.m_UserName;
                        receiver25.m_passwd = this.m_PassWd;
                        receiver25.m_snote = this.m_sNote;
                        receiver25.m_stype = this.m_sType;
                        receiver25.m_emailno = this.m_Mailno;
                        receiver25.m_serv = this.m_Servaddr;
                        receiver25.mailType = "RamBler邮件";
                        receiver25.server = "Rambler";
                        receiver25.listid = "RamblerID";
                        receiver25.START();
                        return;
                    }
                default:
                    return;
            }

        }
        private int GetServType(string server)
        {
            if (server.Trim().ToUpper() == "GMAIL")
            {
                return 3;
            }
            if (server.Trim().ToUpper() == "HINET")
            {
                return 7;
            }
            if (server.Trim().ToUpper() == "RU")
            {
                return 4;
            }
            if (server.Trim().ToUpper() == "YANDEX")
            {
                return 0x1b;
            }
            if (server.Trim().ToUpper() == "RAMBLER")
            {
                return 0x1c;
            }
            if (server.Trim().ToUpper() == "YAHOO")
            {
                return 1;
            }
            if (server.Trim().ToUpper() == "HOTMAIL")
            {
                return 2;
            }
            if (server.Trim() == "163")
            {
                return 8;
            }
            if (server.Trim().ToUpper() == "VIP163")
            {
                return 0x17;
            }
            if (server.Trim() == "126")
            {
                return 9;
            }
            if (server.Trim().ToUpper() == "YEAH")
            {
                return 0x18;
            }
            if (server.Trim().ToUpper() == "SOHU")
            {
                return 10;
            }
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
            if (server.Trim().ToUpper() == "CTM")
            {
                return 11;
            }
            if (server.Trim().ToUpper() == "FREEPOPS")
            {
                return 12;
            }
            if (server.Trim().ToUpper() == "263")
            {
                return 13;
            }
            if (server.Trim().ToUpper() == "FASTMAIL")
            {
                return 14;
            }
            if (server.Trim().ToUpper() == "HUSHMAIL")
            {
                return 15;
            }
            if (server.Trim().ToUpper() == "TOM")
            {
                return 0x10;
            }
            if (server.Trim().ToUpper() == "SINA")
            {
                return 0x11;
            }
            if (server.Trim().ToUpper() == "21CN")
            {
                return 0x12;
            }
            if (server.Trim().ToUpper() == "HANMAIL")
            {
                return 0x13;
            }
            if (server.Trim().ToUpper() == "QQ")
            {
                return 20;
            }
            if (server.Trim().ToUpper() == "IMAP")
            {
                return 0x15;
            }
            if (server.Trim().ToUpper() == "AOL")
            {
                return 0x16;
            }
            if (server.Trim().ToUpper() == "MAILHN")
            {
                return 0x19;
            }
            if (server.Trim().ToUpper() == "REDIFFMAIL")
            {
                return 0x1a;
            }
            return 5;

        }

    }
}
