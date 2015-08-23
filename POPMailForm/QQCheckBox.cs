using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MailExposure.POPMailForm
{
    public partial class QQCheckBox : Form
    {
        private Image image;

        public QQCheckBox()
        {
            InitializeComponent();
        }
        public QQCheckBox(Image image)
        {
            this.InitializeComponent();
            this.image = image;
        }

        private void buttonSetCheck_Click(object sender, EventArgs e)
        {
            base.Tag = this.textBoxCheck.Text;
            base.Dispose();
        }

        private void QQCheckBox_Load(object sender, EventArgs e)
        {
            this.pictureBoxCheck.Image = this.image;
        }

    }
}
