using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Data;
using System.Windows.Forms;

namespace MailExposure.RecordInfo
{
    public class DBHelper
    {
       // Fields
        private OleDbConnection conn;
        private OleDbConnectionStringBuilder connectStringBuilder;

        // Methods
        public DBHelper()
        { }
        public void close()
        {
            this.conn.Close();
        }
        public int ExecuteNonQuery(string sql) 
        {
            try
            {
                this.connectStringBuilder = new OleDbConnectionStringBuilder();
                this.connectStringBuilder.DataSource = AppDomain.CurrentDomain.BaseDirectory + "MailData.mdb";
                this.connectStringBuilder.Provider = "Microsoft.Jet.OLEDB.4.0";
                this.conn = new OleDbConnection(this.connectStringBuilder.ConnectionString);
                this.conn.Open();
            }
            catch (Exception)
            {
                MessageBox.Show("数据库连接失败！数据库文件不存在！", "警告");
            }
            OleDbCommand command = new OleDbCommand(sql, this.conn);
            return command.ExecuteNonQuery();
        }
        public OleDbDataReader ExecuteRead(string sql)
        {
            try
            {
                this.connectStringBuilder = new OleDbConnectionStringBuilder();
                this.connectStringBuilder.DataSource = AppDomain.CurrentDomain.BaseDirectory + "MailData.mdb";
                this.connectStringBuilder.Provider = "Microsoft.Jet.OLEDB.4.0";
                this.conn = new OleDbConnection(this.connectStringBuilder.ConnectionString);
                this.conn.Open();
            }
            catch (Exception exception)
            {
                exception.ToString();
                MessageBox.Show("数据库连接失败！数据库文件不存在！", "警告");
            }
            OleDbCommand command = new OleDbCommand(sql, this.conn);
            return command.ExecuteReader();
        }
        public int ExecuteScalar(string sql)
        {
            try
            {
                this.connectStringBuilder = new OleDbConnectionStringBuilder();
                this.connectStringBuilder.DataSource = AppDomain.CurrentDomain.BaseDirectory + "MailData.mdb";
                this.connectStringBuilder.Provider = "Microsoft.Jet.OLEDB.4.0";
                this.conn = new OleDbConnection(this.connectStringBuilder.ConnectionString);
                this.conn.Open();
            }
            catch (Exception)
            {
                MessageBox.Show("数据库连接失败！数据库文件不存在！", "警告");
                return -1;
            }
            OleDbCommand command = new OleDbCommand(sql, this.conn);
            return Convert.ToInt32(command.ExecuteScalar());

        }

        public DataTable GetDataSet(string sql)
        {
            try
            {
                this.connectStringBuilder = new OleDbConnectionStringBuilder();
                this.connectStringBuilder.DataSource = AppDomain.CurrentDomain.BaseDirectory + "MailData.mdb";
                this.connectStringBuilder.Provider = "Microsoft.Jet.OLEDB.4.0";
                this.conn = new OleDbConnection(this.connectStringBuilder.ConnectionString);
                this.conn.Open();
            }
            catch (Exception)
            {
                MessageBox.Show("数据库连接失败！数据库文件不存在！", "警告");
            }
            OleDbDataAdapter adapter = new OleDbDataAdapter(sql, this.conn);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            return dataTable;
        }
    }
}
