using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace MailExposure {
	static class Program {
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() {
			DevExpress.UserSkins.OfficeSkins.Register();
			DevExpress.UserSkins.BonusSkins.Register();
			DevExpress.Skins.SkinManager.EnableFormSkins();
			//Application.EnableVisualStyles();
			//Application.SetCompatibleTextRenderingDefault(false);
			//Application.Run(new MainForm());
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                LoginFrm login = new LoginFrm();
                if (Setting.GetValue("CookieEmailPassword", "") == "")
                {
                    login.TopLevel = true;
                    login.ShowDialog();
                }
                else
                {
                    object sender = new object();
                    EventArgs e = new EventArgs();
                    login.button1_Click(sender, e);
                }
                Application.Run();
            }
            catch (Exception)
            {
            }

		}
	}
}
