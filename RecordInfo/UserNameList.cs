using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;

namespace MailExposure.RecordInfo
{
    class UserNameList
    {
        // Fields
        private DBHelper db;

        // Methods
        public UserNameList()
        {
            this.db = new DBHelper();
        }
        public string DelUserNameList()
        {
            string str = "用户名删除失败!";
            string sql = "Delete from UserNameList";
            if (this.db.ExecuteNonQuery(sql) > 0)
            {
                str = "用户名删除成功!";
            }
            this.db.close();
            return str;

        }
        public string DelUserNameList(string param)
        {
            string str = "用户名删除失败!";
            string sql = "Delete from UserNameList where UserName='" + param + "'";
            if (this.db.ExecuteNonQuery(sql) > 0)
            {
                str = "用户名删除成功!";
            }
            this.db.close();
            return str;

        }
        public List<string> GetUserNameList()
        {
            List<string> list = new List<string>();
            string sql = "select UserName from UserNameList";
            OleDbDataReader reader = this.db.ExecuteRead(sql);
            while (reader.Read())
            {
                list.Add(reader["UserName"].ToString());
            }
            this.db.close();
            return list;

        }

        public string SetUserNameList(Model model)
        {
            bool flag = false;
            string str = "用户名保存失败!";
            List<string> userNameList = new List<string>();
            userNameList = this.GetUserNameList();
            for (int i = 0; i < userNameList.Count; i++)
            {
                if (userNameList[i] == model.UserName)
                {
                    flag = true;
                    break;
                }
            }
            if (flag)
            {
                str = "此用户名帐号已经存!";
            }
            else
            {
                string sql = string.Format("Insert into UserNameList(UserName) values('{0}')", model.UserName);
                if (this.db.ExecuteNonQuery(sql) > 0)
                {
                    str = "用户名保存成功!";
                }
            }
            this.db.close();
            return str;
        }
    }
}
