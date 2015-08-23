using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace MailExposure.CookieMailForm
{
    public partial class ConvertBaseForm : Form
    {
        public ConvertBaseForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox2.Text = ConvertBase64Code(textBox1.Text);
            this.button3.Enabled = true;
        }
        private String ConvertBase64Code(String str)
        {

            int[] numArray = new int[] { 
                    -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, 
                    -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, 
                    -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, 0x3e, -1, -1, -1, 0x3f, 
                    0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3a, 0x3b, 60, 0x3d, -1, -1, -1, -1, -1, -1, 
                    -1, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 
                    15, 0x10, 0x11, 0x12, 0x13, 20, 0x15, 0x16, 0x17, 0x18, 0x19, -1, -1, -1, -1, -1, 
                    -1, 0x1a, 0x1b, 0x1c, 0x1d, 30, 0x1f, 0x20, 0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 40, 
                    0x29, 0x2a, 0x2b, 0x2c, 0x2d, 0x2e, 0x2f, 0x30, 0x31, 50, 0x33, -1, -1, -1, -1, -1
            };
            int length = str.Length;
            int num5 = 0;
            string str2 = "";
            while (num5 < length)
            {
                int num;
                int num2;
                int num3;
                int num4;
                do
                {
                    num = numArray[str[num5++] & '\x00ff'];
                }
                while ((num5 < length) && (num == -1));
                if (num == -1)
                {
                    return str2;
                }
                do
                {
                    num2 = numArray[str[num5++] & '\x00ff'];
                }
                while ((num5 < length) && (num2 == -1));
                if (num2 == -1)
                {
                    return str2;
                }
                str2 = str2 + ((char)((num << 2) | ((num2 & 0x30) >> 4)));
                do
                {
                    num3 = str[num5++] & '\x00ff';
                    if (num3 == 0x3d)
                    {
                        return str2;
                    }
                    num3 = numArray[num3];
                }
                while ((num5 < length) && (num3 == -1));
                if (num3 == -1)
                {
                    return str2;
                }
                str2 = str2 + ((char)(((num2 & 15) << 4) | ((num3 & 60) >> 2)));
                do
                {
                    num4 = str[num5++] & '\x00ff';
                    if (num4 == 0x3d)
                    {
                        return str2;
                    }
                    num4 = numArray[num4];
                }
                while ((num5 < length) && (num4 == -1));
                if (num4 == -1)
                {
                    return str2;
                }
                str2 = str2 + ((char)(((num3 & 3) << 6) | num4));
            }
            return str2;
        }

        private void ConvertBaseForm_Load(object sender, EventArgs e)
        {
            this.button3.Enabled = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog
            {
                Filter = "文本文件(*.txt)|*.txt"
            };
            dialog.ShowDialog();
            if (dialog.FileName != "")
            {
                WriteTxtFile(dialog.FileName, textBox2.Text);
            }
        }

        public void WriteTxtFile(string filepath, string filetext)
        {
            try
            {
                byte[] bytes = Encoding.UTF8.GetBytes(filetext);
                FileStream stream = null;
                stream = File.Open(filepath, FileMode.Append, FileAccess.Write);
                stream.Write(bytes, 0, bytes.Length);
                stream.Close();
                stream.Dispose();
            }
            catch (Exception)
            {

            }    
        }

        private void button2_Click(object sender, EventArgs e)
        {
            base.Close();
        }
    }
}
