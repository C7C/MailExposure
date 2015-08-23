using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MailExposure
{
    public partial class AddSmtpServer : Form
    {
        public String strSMTP = "";
        public AddSmtpServer()
        {
            InitializeComponent();
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            strSMTP = this.SMTPTxt.Text.Trim().ToLower();
            Close();
        }
    }
}
