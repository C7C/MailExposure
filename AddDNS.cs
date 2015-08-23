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
    public partial class AddDNS : Form
    {
        public String strDNS = "";
        public AddDNS()
        {
            InitializeComponent();
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            strDNS = DNSTxt.Text;
            Close();
        }
    }
}
