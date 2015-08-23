using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;

namespace MailExposure.RecordInfo
{
    class AddressList
    {
        // Fields
        private DBHelper db;
        // Methods
        public AddressList()
        {
            this.db = new DBHelper();
        }
        public string DelAddressList()
        {
            string str = "通讯录删除失败!";
            string sql = "Delete from AddressBook";
            if (this.db.ExecuteNonQuery(sql) > 0)
            {
                str = "通讯录删除成功!";
            }
            this.db.close();
            return str;

        }
        public string DelAddressList(string param)
        {
            string str = "通讯录删除失败!";
            string sql = "Delete from AddressBook where Addressee='" + param + "'";
            if (this.db.ExecuteNonQuery(sql) > 0)
            {
                str = "通讯录删除成功!";
            }
            this.db.close();
            return str;

        }
        public List<string> GetAddressList()
        {
            List<string> list = new List<string>();
            string sql = "select Addressee from AddressBook";
            OleDbDataReader reader = this.db.ExecuteRead(sql);
            while (reader.Read())
            {
                list.Add(reader["Addressee"].ToString());
            }
            this.db.close();
            return list;

        }
        public string SetAddressList(Model model)
        {
            bool flag = false;
            string str = "通讯录保存失败!";
            List<string> addressList = new List<string>();
            addressList = this.GetAddressList();
            for (int i = 0; i < addressList.Count; i++)
            {
                if (addressList[i] == model.Addressee)
                {
                    flag = true;
                    break;
                }
            }
            if (flag)
            {
                str = "此邮箱帐号已经存!";
            }
            else
            {
                string sql = string.Format("Insert into AddressBook(Addressee) values('{0}')", model.Addressee);
                if (this.db.ExecuteNonQuery(sql) > 0)
                {
                    str = "通讯录保存成功!";
                }
            }
            this.db.close();
            return str;

        }
    }
}
