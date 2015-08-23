using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Management;
using System.Runtime.InteropServices;

namespace MailExposure
{
    public partial class LoginFrm : Form
    {
        public string CookieEmail;
        [DllImport("kernel32.dll")]
        private static extern int GetVolumeInformation(string lpRootPathName, string lpVolumeNameBuffer, int nVolumeNameSize, ref int lpVolumeSerialNumber, int lpMaximumComponentLength, int lpFileSystemFlags, string lpFileSystemNameBuffer, int nFileSystemNameSize);
        int tryLogin = 0;
        public LoginFrm()
        {
            this.CookieEmail = "";
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;

        }

        private void LoginFrm_Load(object sender, EventArgs e)
        {
            string cstr = this.cpuyp();
            string str2 = this.encrypt(cstr);
            this.textBoxHardwareID.Text = str2;
        }

        public string CpuId()
        {
            ManagementClass class2 = new ManagementClass("Win32_Processor");
            foreach (ManagementObject obj2 in class2.GetInstances())
            {
                return obj2.Properties["ProcessorId"].Value.ToString();
            }
            return null;
        }

        public string cpuyp()
        {
            string str;
            string volOf;
            try
            {
                str = this.CpuId();
            }
            catch
            {
                str = "0";
            }
            try
            {
                volOf = this.GetVolOf("C");
            }
            catch
            {
                volOf = "0";
            }
            string str3 = str + volOf + "0";
            str3 = str3 + Convert.ToString((int)(str3.Length - 1));
            return this.str19(str3);
        }

        public string str19(string str)
        {
            string str2 = null;
            str = str.Trim();
            if ((str == null) || (str.Length == 0))
            {
                return "0";
            }
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] bytes = encoding.GetBytes(str);
            byte[] buffer2 = new byte[str.Length];
            int index = 0;
            for (int i = 0; i < str.Length; i++)
            {
                if ((bytes[i] < 0x3a) && (bytes[i] > 0x30))
                {
                    buffer2[index] = bytes[i];
                    index++;
                }
            }
            str2 = str2 + encoding.GetString(buffer2);
            int length = str2.IndexOf("\0");
            return str2.Substring(0, length);
        }

        public string GetVolOf(string drvID)
        {
            int lpVolumeSerialNumber = 0;
            int lpMaximumComponentLength = 0;
            int lpFileSystemFlags = 0;
            string lpVolumeNameBuffer = null;
            string lpFileSystemNameBuffer = null;
            GetVolumeInformation(drvID + @":\", lpVolumeNameBuffer, 0x100, ref lpVolumeSerialNumber, lpMaximumComponentLength, lpFileSystemFlags, lpFileSystemNameBuffer, 0x100);
            return lpVolumeSerialNumber.ToString("x");
        }

        public string DecodeBase64(string code_type, string code)
        {
            byte[] bytes = null;
            bytes = Convert.FromBase64String(code);
            try
            {
                return Encoding.GetEncoding(code_type).GetString(bytes);
            }
            catch
            {
                return code;
            }
        }

        public string encrypt(string Cstr)
        {
            string str = null;
            char[] chArray = Cstr.ToCharArray();
            int num = Cstr.Length * 2;
            for (int i = 0; i < chArray.Length; i++)
            {
                num = num % 0x1a;
                char ch = chArray[i];
                if (((ch >= 'a') && (ch <= 'z')) || ((ch >= 'A') && (ch <= 'Z')))
                {
                    ch = (char)(ch + num);
                    if (((ch > 'Z') && (ch <= (90 + num))) || (ch > 'z'))
                    {
                        ch = (char)(ch - '\x001a');
                    }
                }
                if ((ch >= '0') && (ch <= '9'))
                {
                    num = num % 10;
                    ch = (char)(ch + num);
                    if ((ch > '9') && (ch <= (0x39 + num)))
                    {
                        ch = (char)(ch - '\n');
                    }
                }
                str = str + ch;
            }
            return str;
        }

        private string explain(string Cstr)
        {
            string str = null;
            char[] chArray = Cstr.ToCharArray();
            int num = Cstr.Length * 2;
            for (int i = 0; i < chArray.Length; i++)
            {
                num = num % 0x1a;
                char ch = chArray[i];
                if (((ch >= 'a') && (ch <= 'z')) || ((ch >= 'A') && (ch <= 'Z')))
                {
                    ch = (char)(ch - num);
                    if (((ch < 'A') && (ch >= (0x41 - num))) || ((ch < 'a') && (ch >= (0x61 - num))))
                    {
                        ch = (char)(ch + '\x001a');
                    }
                }
                if ((ch >= '0') && (ch <= '9'))
                {
                    num = num % 10;
                    ch = (char)(ch - num);
                    if ((ch < '0') && (ch >= (0x30 - num)))
                    {
                        ch = (char)(ch + '\n');
                    }
                }
                str = str + ch;
            }
            return str;
        }

        public void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string cstr = this.cpuyp();
                string str2 = this.encrypt(cstr);
                string str3 = this.explain(str2);
                this.CookieEmail = Setting.GetValue("CookieEmailPassword", "");
                if ((this.textBoxLogin.Text.Length <= 0) && !(this.CookieEmail == ""))
                {
                    this.textBoxLogin.Text = this.CookieEmail.Trim();
                }
                string code = this.textBoxLogin.Text.Trim();
                string[] strArray = this.DecodeBase64("unicode", code).Split(new char[] { ' ' });
                string str6 = this.DecodeBase64("unicode", strArray[0]);
                string strTimeEnd = "";
                if (strArray.Length > 1)
                {
                    strTimeEnd = this.DecodeBase64("unicode", strArray[1]);
                }
                if (str6 == str3)
                {
                    Setting.SetValue("CookieEmailPassword", code);
                    switch (strTimeEnd)
                    {
                        case "":
                        case "-1":
                            try
                            {
                                MainForm form = new MainForm();
                               // if (form.conn.State == ConnectionState.Open)
                                {
                                    form.Show();
                                    base.Hide();
                                    goto Label_01C7;
                                }
                                MainForm.KillMailProcess();
                            }
                            catch (Exception)
                            {
                                MainForm.KillMailProcess();
                            }
                            return;
                    }
                    base.Hide();
                   /* if (new TrialSeting(strTimeEnd).init())
                    {
                        try
                        {
                            MainForm form2 = new MainForm();
                            if (form2.conn.State == ConnectionState.Open)
                            {
                                form2.Show();
                                goto Label_01C7;
                            }
                            MainForm.mainForm.KillMailProcess();
                        }
                        catch (Exception)
                        {
                            MainForm.mainForm.KillMailProcess();
                        }
                        return;
                    }*/
                   // new LoginFrm { TopLevel = true }.ShowDialog();
                }
                else if (this.tryLogin >= 2)
                {
                    Application.ExitThread();
                }
            Label_01C7:
                if (this.textBoxLogin.Text.Trim() != "")
                {
                    this.tryLogin++;
                }
                this.textBoxLogin.Clear();
                this.textBoxLogin.Focus();
            }
            catch (Exception)
            {
                base.Hide();
                new LoginFrm { TopLevel = true }.ShowDialog();
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void LoginFrm_FormClosing(object sender, FormClosingEventArgs e)
        {
         //  Application.ExitThread();
        }

        private void LoginFrm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.ExitThread();
        }
  
    }
}
