using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MailExposure
{
    public class GlobalValue
    {
        public static CookieMailControl mainForm = null;
        public static POPMailForm.PopMailControl PopMainForm = null;
    }

    public class Model
    {
        // Fields
      /*  private string address;
        private string addressee;
        private string addresser;
        private string attachmentName;
        private string attachmentSrc;
        private string charSet;
        private string content;
        private int    iD;
        private string logParam;
        private string mailType;
        private string nickname;
        private string password;
        private string server;
        private string subject;
        private string type;
        private string userName;
        private string xSSCode;*/
        
        // Properties
        public string Address { get; set; }
        public string Addressee { get; set; }
        public string Addresser { get; set; }
        public string AttachmentName { get; set; }
        public string AttachmentSrc { get; set; }
        public string CharSet { get; set; }
        public string Content { get; set; }
        public int    ID { get; set; }
        public string LogParam { get; set; }
        public string MailType { get; set; }
        public string Nickname { get; set; }
        public string Password { get; set; }
        public string Server { get; set; }
        public string Subject { get; set; }
        public string Type { get; set; }
        public string UserName { get; set; }
        public string XSSCode { get; set; }

        // Methods
        public Model()
        {
        }
        public string EncodeBase64(string code)
        {
            byte[] bytes = Encoding.Default.GetBytes(code);
            try
            {
                return Convert.ToBase64String(bytes);
            }
            catch
            {
                return code;
            }
        }
    }

    public enum FontSize
    {
        One,
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        NA
    }
    public enum ReadyState
    {
        Uninitialized,
        Loading,
        Loaded,
        Interactive,
        Complete
    }
    public enum SelectionType
    {
        Text,
        Control,
        None
    }
}
