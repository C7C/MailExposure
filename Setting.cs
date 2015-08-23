using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MailExposure
{
    class Setting
    {
        // Methods
        public Setting()
        { 
        }
        public static string GetIniValue(string section, string key, string def, string filePath)
        {
            StringBuilder retVal = new StringBuilder(0x400);
            GetPrivateProfileString(section, key, def, retVal, 0x400, filePath);
            return retVal.ToString();

        }
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
     
        public static string GetValue(string name, string defaultValue)
        {
            return (string) Application.UserAppDataRegistry.GetValue(name, defaultValue);
        }
        public static void SetIniValue(string section, string key, string val, string filePath)
        {
             WritePrivateProfileString(section, key, val, filePath);
        }
        public static void SetValue(string name, string value)
        {
            Application.UserAppDataRegistry.SetValue(name, value);
        }
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

    }
}
