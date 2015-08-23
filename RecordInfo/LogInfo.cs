using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;

namespace MailExposure.RecordInfo
{
    class LogInfo
    {
        // Fields
        private DBHelper db;
        private Model model;

        // Methods
        public LogInfo()
        {
            this.db = new DBHelper();
            this.model = new Model();

        }
        public string DelLog()
        {
            string str = "日志删除失败!";
            string sql = string.Format("Delete from LogInfo", new object[0]);
            if (this.db.ExecuteNonQuery(sql) > 0)
            {
                str = "日志删除成功!";
            }
            this.db.close();
            return str;
        }

        public string DelLog(string ID)
        {
            string str = "日志删除失败!";
            string sql = string.Format("Delete from LogInfo where ID in({0})", ID);
            if (this.db.ExecuteNonQuery(sql) > 0)
            {
                str = "日志删除成功!";
            }
            this.db.close();
            return str;
        }

        public List<string> GetLog()
        {
            List<string> list = new List<string>();
            string sql = "select ETime from LogInfo group by  ETime order by ETime desc ";
            OleDbDataReader reader = this.db.ExecuteRead(sql);
            while (reader.Read())
            {
                list.Add(reader["ETime"].ToString());
            }
            this.db.close();
            return list;

        }
        public DataTable GetLog(int page, int pageIndex)
        {
            string sql = "";
            if (pageIndex == 1)
            {
                sql = string.Format("select top {0} ID as 序号,Nickname as 昵称,Addresser as 发信人,Addressee as 收信人,Subject as 主题,AttachmentName as 附件名称,ETime as 时间 ,UserName as 用户名 from LogInfo  order by ETime desc", page);
            }
            else
            {
                sql = string.Format("select top {0} ID as 序号,Nickname as 昵称,Addresser as 发信人,Addressee as 收信人,Subject as 主题,AttachmentName as 附件名称,ETime as 时间,UserName as 用户名 from LogInfo where ID not in(select top {1} ID from LogInfo) order by ETime desc", page, page * (pageIndex - 1));
            }
            DataTable dataSet = this.db.GetDataSet(sql);
            this.db.close();
            return dataSet;

        }
        public string GetLog(string param1, string param2)
        {
            string sql = string.Format("select * from LogInfo where (Addressee='{0}') and (ETime like '{1}%') ", param1, param2);
            OleDbDataReader reader = this.db.ExecuteRead(sql);
            string str2 = "";
            while (reader.Read())
            {
                str2 = string.Format("您今日已经给该收信人的邮箱帐号发送过一封邮件了!\r\n具休信息如下:\r\n昵称:{0}\r\n发信人:{1}\r\n收信人:{2}\r\n主题:{3}\r\n时间:{4}\r\n\r\n您确认还要继续发送吗?", new object[] { reader["Nickname"], reader["Addressee"], reader["Addressee"], reader["Subject"], reader["ETime"] });
            }
            this.db.close();
            return str2;

        }
        public DataTable GetLog(int page, int pageIndex, string param)
        {
            string sql = "";
            if (pageIndex == 1)
            {
                sql = string.Format("select top {0} ID as 序号,Nickname as 昵称,Addresser as 发信人,Addressee as 收信人,Subject as 主题,AttachmentName as 附件名称,ETime as 时间,UserName as 用户名 from LogInfo where ((Nickname like '%{1}%') or (Addresser like '%{1}%') or (Addressee like '%{1}%') or (Subject like '%{1}%') or (AttachmentName like '%{1}%') or (ETime like '%{1}%')) order by ETime desc", page, param);
            }
            else
            {
                sql = string.Format("select top {0} ID as 序号,Nickname as 昵称,Addresser as 发信人,Addressee as 收信人,Subject as 主题,AttachmentName as 附件名称,ETime as 时间,UserName as 用户名 from LogInfo where (ID not in(select top {1} ID from LogInfo)) and ((Nickname like '{2}') or (Addresser like '{2}') or (Addressee like '{2}') or (Subject like '{2}') or (AttachmentName like '{2}') or (ETime like '{2}')) order by ETime desc", page, page * (pageIndex - 1), param);
            }
            DataTable dataSet = this.db.GetDataSet(sql);
            this.db.close();
            return dataSet;
        }

        public int LogCount()
        {
            string sql = "select count(*) from LogInfo";
            int num = this.db.ExecuteNonQuery(sql);
            this.db.close();
            return num;
        }

        public string SetLlog(Model model)
        {
            DateTime now = new DateTime();
            now = DateTime.Now;
            int year = now.Year;
            int month = now.Month;
            int day = now.Day;
            int hour = now.Hour;
            int minute = now.Minute;
            if (model.AttachmentName == null)
            {
                model.AttachmentName = null;
            }
            string sql = string.Format("Insert into LogInfo(Nickname,Addresser,Addressee,Server,Subject,AttachmentName,ETime,UserName) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}')", new object[] { model.Nickname, model.Addresser, model.Addressee, model.Server, model.Subject, model.AttachmentName, string.Concat(new object[] { year, "年", month, "月", day, "日 ", hour, "时", minute, "分" }), model.UserName });
            int num6 = this.db.ExecuteNonQuery(sql);
            string str2 = "日志保存失败!";
            if (num6 > 0)
            {
                str2 = "日志保存成功!";
            }
            this.db.close();
            return str2;
        }
    }
}
