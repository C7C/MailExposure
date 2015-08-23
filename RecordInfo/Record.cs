using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;

namespace MailExposure.RecordInfo
{
    class Record
    {
        // Fields
        private DBHelper db;

        // Methods
        public Record()
        {
            this.db = new DBHelper();
        }
        public string Dns(string para)
        {
            string str = "";
            if (para == "")
            {
                string str2 = "select * from ini";
                OleDbDataReader reader = this.db.ExecuteRead(str2);
                while (reader.Read())
                {
                    str = reader["dns"].ToString();
                }
                return str;
            }
            string sql = string.Format("insert into ini(dns) values('{0}')", para);
            if (this.db.ExecuteNonQuery(sql) > 0)
            {
                return "DNS自定义成功!";
            }
            return "DNS自定义失败!";

        }
        public string[] GetRecord()
        {
            string sql = "select * from Record";
            OleDbDataReader reader = this.db.ExecuteRead(sql);
            string[] strArray = new string[6];
            if (reader.Read())
            {
                strArray[0] = reader["Nickname"].ToString();
                strArray[1] = reader["Addresser"].ToString();
                strArray[2] = reader["Addressee"].ToString();
                strArray[3] = reader["UserName"].ToString();
                strArray[4] = reader["Subject"].ToString();
                strArray[5] = reader["Server"].ToString();
            }
            this.db.close();
            return strArray;

        }
        public string ini(string para)
        {
            string str = "";
            if (para == "get")
            {
                string str2 = "select * from ini";
                OleDbDataReader reader = this.db.ExecuteRead(str2);
                while (reader.Read())
                {
                    str = reader["infos"].ToString();
                    if (str != "")
                    {
                        return str;
                    }
                }
                return str;
            }
            string sql = string.Format("insert into ini(infos) values('{0}')", para);
            if (this.db.ExecuteNonQuery(sql) > 0)
            {
                str = "951";
            }
            return str;

        }

        public void SetRecord(string nickname, string addresser, string addressee, string username, string subject, string server)
        {
            string sql = "";
            if (this.GetRecord()[2] == null)
            {
                sql = string.Format("Insert into Record(ID,Nickname,Addresser,Addressee,UserName,Subject,Server) values({0},'{1}','{2}','{3}','{4}','{5}','{6}')", new object[] { 0, nickname, addresser, addressee, username, subject, server });
            }
            else
            {
                sql = string.Format("Update Record SET Nickname='{0}',Addresser='{1}',Addressee='{2}',UserName='{3}',Subject='{4}',Server='{5}' where ID=0 ", new object[] { nickname, addresser, addressee, username, subject, server });
            }
            this.db.ExecuteNonQuery(sql);
            this.db.close();
        }
    }
}
