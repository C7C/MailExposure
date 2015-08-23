using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Data;
using System.Threading;
using System.IO;

namespace MailExposure.PopMailRC
{
    class ExplainPasswd
    {
        // Fields
        private OleDbConnection conn;
        private string CRLF;
        public DataSet dataSet;
        private string Flag;
        private OleDbCommand oleDbCommand;
        private OleDbDataAdapter oleDbDataAdapter;
        private OleDbDataReader oledReader;
        public static object write;

        // Methods
        static ExplainPasswd()
        {
            write = new object();
        }
        public ExplainPasswd()
        {
            this.CRLF = "\r\n";
            this.Flag = "\t";
        }
        public void ConnectMdb()
        {
            try
            {
                OleDbConnectionStringBuilder builder = new OleDbConnectionStringBuilder();
                builder.DataSource = "./user.mdb";
                builder.Provider = "Microsoft.Jet.OLEDB.4.0";
                this.conn = new OleDbConnection(builder.ConnectionString);
                this.conn.Open();
                ConnectionState state = this.conn.State;
            }
            catch (Exception)
            {
            }

        }
        public string explain(string Cstr, string CCstr)
        {
            string str = null;
            char[] chArray = Cstr.ToCharArray();
            int length = CCstr.Length;
            for (int i = 0; i < chArray.Length; i++)
            {
                length = length % 0x1a;
                char ch = chArray[i];
                if (((ch >= 'a') && (ch <= 'z')) || ((ch >= 'A') && (ch <= 'Z')))
                {
                    ch = (char)(ch - length);
                    if (((ch < 'A') && (ch >= (0x41 - length))) || ((ch < 'a') && (ch >= (0x61 - length))))
                    {
                        ch = (char)(ch + '\x001a');
                    }
                }
                if ((ch >= '0') && (ch <= '9'))
                {
                    length = length % 10;
                    ch = (char)(ch - length);
                    if ((ch < '0') && (ch >= (0x30 - length)))
                    {
                        ch = (char)(ch + '\n');
                    }
                }
                str = str + ch;
            }
            return str;

        }
        public void Start()
        {
            string filepath = "用户信息.txt";
            this.ConnectMdb();
            string str2 = "select * from users order by 服务器地址 asc";
            this.oleDbCommand = new OleDbCommand();
            this.oleDbCommand.CommandText = str2;
            this.oleDbCommand.Connection = this.conn;
            if (this.conn.State == ConnectionState.Closed)
            {
                Console.WriteLine("数据库连接失败！");
            }
            else
            {
                this.oledReader = this.oleDbCommand.ExecuteReader();
                while (this.oledReader.Read())
                {
                    string cCstr = this.oledReader[1].ToString();
                    string cstr = this.oledReader[2].ToString();
                    string str5 = this.oledReader[3].ToString();
                    string str6 = this.oledReader[5].ToString();
                    string str7 = this.oledReader[6].ToString();
                    string str8 = this.explain(cstr, cCstr);
                    string filetext = cCstr + "@" + str6 + this.Flag + str8 + this.Flag + str5 + this.Flag + str7 + this.Flag + this.CRLF;
                    this.WriteLogFile(filepath, filetext);
                    Console.WriteLine(filetext);
                    cCstr = "";
                    cstr = "";
                    str8 = "";
                    filetext = "";
                }
                Console.WriteLine("完毕！");
                if (this.conn.State == ConnectionState.Open)
                {
                    this.conn.Close();
                }
            }

        }
        public void WriteLogFile(string filepath, string filetext)
        {
            object obj2;
            Monitor.Enter(obj2 = write);
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
            finally
            {
                Monitor.Exit(obj2);
            }

        }

    }
}
