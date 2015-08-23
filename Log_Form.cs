using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MailExposure.RecordInfo;

namespace MailExposure
{
    public partial class Log_Form : Form
    {
        private DBHelper db;
        private int pageIndex;
        private Model model;
        private LogInfo log;
        private int page;
 
        public Log_Form()
        {
            this.page = 20;
            this.pageIndex = 1;
            this.model = new Model();
            this.db = new DBHelper();
            this.log = new LogInfo();
            this.InitializeComponent();
        }

        private void Log_Form_Load(object sender, EventArgs e)
        {
            this.TotalPageNum.Text = "共" + this.LogCount() + "页";
            if (this.LogCount() < 1)
            {
                this.NextPage.Enabled = false;
                this.PrePage.Enabled = false;
            }
            else
            {
                this.PrePage.Enabled = false;
            }
            this.CurrentPageNum.Text = "第" + this.pageIndex + "页";
            this.BindData();
        }

        private int LogCount()
        {
            string sql = "select count(*) from LogInfo";
            int num = this.db.ExecuteScalar(sql);
            return (((num + this.page) - 1) / this.page);
        }

        private void BindData()
        {
            if (this.model.LogParam == null)
            {
                DataTable log = this.log.GetLog(this.page, this.pageIndex);
                this.dataLogInfo.DataSource = log;
                for (int i = 1; i < this.dataLogInfo.Columns.Count; i++)
                {
                    this.dataLogInfo.Columns[i].ReadOnly = true;
                }
            }
            else
            {
                DataTable table2 = this.log.GetLog(this.page, this.pageIndex, this.model.LogParam);
                this.dataLogInfo.DataSource = table2;
                for (int j = 1; j < this.dataLogInfo.Columns.Count; j++)
                {
                    this.dataLogInfo.Columns[j].ReadOnly = true;
                }
            }
            this.dataLogInfo.Columns[0].Width = 20;
            this.dataLogInfo.Columns[1].Width = 0x23;
            this.dataLogInfo.Columns[2].Width = 70;
            this.dataLogInfo.Columns[3].Width = 150;
            this.dataLogInfo.Columns[4].Width = 150;
            this.dataLogInfo.Columns[5].Width = 200;
            this.dataLogInfo.Columns[6].Width = 150;
            this.dataLogInfo.Columns[7].Width = 150;
            this.dataLogInfo.Columns[8].Width = 150;
        }

        private void DelAllButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认要删除吗?", "操作提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                MessageBox.Show(this.log.DelLog(), "操作结果", MessageBoxButtons.OK, MessageBoxIcon.None);
            }
            this.BindData();
        }

        private void DeleSelectButton_Click(object sender, EventArgs e)
        {
            string iD = "";
            if (MessageBox.Show("确认要删除吗?", "操作提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                for (int i = 0; i < this.dataLogInfo.RowCount; i++)
                {
                    if (this.dataLogInfo.Rows[i].Cells[0].EditedFormattedValue.ToString() == "True")
                    {
                        iD = iD + this.dataLogInfo.Rows[i].Cells[1].Value + ",";
                    }
                }
                if (iD.Length == 0)
                {
                    MessageBox.Show("请选择一行或多行后再删除!");
                    return;
                }
                iD = iD.Substring(0, iD.Length - 1);
                MessageBox.Show(this.log.DelLog(iD), "操作结果", MessageBoxButtons.OK, MessageBoxIcon.None);
            }
            this.BindData();

        }

        private void PrePage_Click(object sender, EventArgs e)
        {
            if (this.pageIndex == 1)
            {
                this.PrePage.Enabled = false;
            }
            else
            {
                this.PrePage.Enabled = true;
                this.pageIndex--;
                if (this.pageIndex == 1)
                {
                    this.NextPage.Enabled = true;
                    this.PrePage.Enabled = false;
                }
            }
            this.CurrentPageNum.Text = "第" + this.pageIndex + "页";
            this.BindData();

        }

        private void NextPage_Click(object sender, EventArgs e)
        {
            if (this.pageIndex == this.LogCount())
            {
                this.NextPage.Enabled = false;
                this.PrePage.Enabled = true;
            }
            else
            {
                this.PrePage.Enabled = true;
                this.pageIndex++;
                if (this.pageIndex == this.LogCount())
                {
                    this.NextPage.Enabled = false;
                    this.PrePage.Enabled = true;
                }
            }
            this.CurrentPageNum.Text = "第" + this.pageIndex + "页";
            this.BindData();
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            this.model.LogParam = this.SearchTxt.Text.Trim();
            this.BindData();
        }
    }
}
