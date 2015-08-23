namespace MailExposure
{
    partial class Log_Form
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Log_Form));
            this.panel1 = new System.Windows.Forms.Panel();
            this.SearchFormStrip = new System.Windows.Forms.ToolStrip();
            this.SearchTxt = new System.Windows.Forms.ToolStripTextBox();
            this.SearchButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.DeleSelectButton = new System.Windows.Forms.ToolStripButton();
            this.DelAllButton = new System.Windows.Forms.ToolStripButton();
            this.BottomStrip = new System.Windows.Forms.ToolStrip();
            this.PrePage = new System.Windows.Forms.ToolStripButton();
            this.NextPage = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.CurrentPageNum = new System.Windows.Forms.ToolStripLabel();
            this.TotalPageNum = new System.Windows.Forms.ToolStripLabel();
            this.dataLogInfo = new System.Windows.Forms.DataGridView();
            this.colNo = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.panel1.SuspendLayout();
            this.SearchFormStrip.SuspendLayout();
            this.BottomStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataLogInfo)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.LightSteelBlue;
            this.panel1.Controls.Add(this.dataLogInfo);
            this.panel1.Controls.Add(this.BottomStrip);
            this.panel1.Controls.Add(this.SearchFormStrip);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(891, 526);
            this.panel1.TabIndex = 0;
            // 
            // SearchFormStrip
            // 
            this.SearchFormStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SearchTxt,
            this.SearchButton,
            this.toolStripSeparator1,
            this.DeleSelectButton,
            this.DelAllButton});
            this.SearchFormStrip.Location = new System.Drawing.Point(0, 0);
            this.SearchFormStrip.Name = "SearchFormStrip";
            this.SearchFormStrip.Size = new System.Drawing.Size(891, 25);
            this.SearchFormStrip.TabIndex = 0;
            this.SearchFormStrip.Text = "toolStrip1";
            // 
            // SearchTxt
            // 
            this.SearchTxt.Name = "SearchTxt";
            this.SearchTxt.Size = new System.Drawing.Size(210, 25);
            // 
            // SearchButton
            // 
            this.SearchButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.SearchButton.Image = ((System.Drawing.Image)(resources.GetObject("SearchButton.Image")));
            this.SearchButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.SearchButton.Name = "SearchButton";
            this.SearchButton.Size = new System.Drawing.Size(33, 22);
            this.SearchButton.Text = "搜索";
            this.SearchButton.Click += new System.EventHandler(this.SearchButton_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // DeleSelectButton
            // 
            this.DeleSelectButton.Image = global::MailExposure.XRDesignRibbonControllerResources.RibbonUserDesigner_HtmlStop;
            this.DeleSelectButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.DeleSelectButton.Name = "DeleSelectButton";
            this.DeleSelectButton.Size = new System.Drawing.Size(49, 22);
            this.DeleSelectButton.Text = "删除";
            this.DeleSelectButton.Click += new System.EventHandler(this.DeleSelectButton_Click);
            // 
            // DelAllButton
            // 
            this.DelAllButton.Image = global::MailExposure.XRDesignRibbonControllerResources.RibbonUserDesigner_Cut;
            this.DelAllButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.DelAllButton.Name = "DelAllButton";
            this.DelAllButton.Size = new System.Drawing.Size(73, 22);
            this.DelAllButton.Text = "全部删除";
            this.DelAllButton.Click += new System.EventHandler(this.DelAllButton_Click);
            // 
            // BottomStrip
            // 
            this.BottomStrip.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.BottomStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.PrePage,
            this.NextPage,
            this.toolStripSeparator2,
            this.CurrentPageNum,
            this.TotalPageNum});
            this.BottomStrip.Location = new System.Drawing.Point(0, 501);
            this.BottomStrip.Name = "BottomStrip";
            this.BottomStrip.Size = new System.Drawing.Size(891, 25);
            this.BottomStrip.TabIndex = 1;
            this.BottomStrip.Text = "toolStrip1";
            // 
            // PrePage
            // 
            this.PrePage.Image = global::MailExposure.XRDesignRibbonControllerResources.RibbonUserDesigner_HtmlBackward;
            this.PrePage.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.PrePage.Name = "PrePage";
            this.PrePage.Size = new System.Drawing.Size(61, 22);
            this.PrePage.Text = "上一页";
            this.PrePage.Click += new System.EventHandler(this.PrePage_Click);
            // 
            // NextPage
            // 
            this.NextPage.Image = global::MailExposure.XRDesignRibbonControllerResources.RibbonUserDesigner_HtmlForward;
            this.NextPage.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.NextPage.Name = "NextPage";
            this.NextPage.Size = new System.Drawing.Size(61, 22);
            this.NextPage.Text = "下一页";
            this.NextPage.Click += new System.EventHandler(this.NextPage_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // CurrentPageNum
            // 
            this.CurrentPageNum.Name = "CurrentPageNum";
            this.CurrentPageNum.Size = new System.Drawing.Size(35, 22);
            this.CurrentPageNum.Text = "     ";
            // 
            // TotalPageNum
            // 
            this.TotalPageNum.Name = "TotalPageNum";
            this.TotalPageNum.Size = new System.Drawing.Size(29, 22);
            this.TotalPageNum.Text = "    ";
            // 
            // dataLogInfo
            // 
            this.dataLogInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataLogInfo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colNo});
            this.dataLogInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataLogInfo.Location = new System.Drawing.Point(0, 25);
            this.dataLogInfo.Name = "dataLogInfo";
            this.dataLogInfo.RowTemplate.Height = 23;
            this.dataLogInfo.Size = new System.Drawing.Size(891, 476);
            this.dataLogInfo.TabIndex = 2;
            // 
            // colNo
            // 
            this.colNo.HeaderText = "";
            this.colNo.Name = "colNo";
            // 
            // Log_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(891, 526);
            this.Controls.Add(this.panel1);
            this.Name = "Log_Form";
            this.Text = "日志";
            this.Load += new System.EventHandler(this.Log_Form_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.SearchFormStrip.ResumeLayout(false);
            this.SearchFormStrip.PerformLayout();
            this.BottomStrip.ResumeLayout(false);
            this.BottomStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataLogInfo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView dataLogInfo;
        private System.Windows.Forms.ToolStrip BottomStrip;
        private System.Windows.Forms.ToolStripButton PrePage;
        private System.Windows.Forms.ToolStripButton NextPage;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripLabel CurrentPageNum;
        private System.Windows.Forms.ToolStripLabel TotalPageNum;
        private System.Windows.Forms.ToolStrip SearchFormStrip;
        private System.Windows.Forms.ToolStripTextBox SearchTxt;
        private System.Windows.Forms.ToolStripButton SearchButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton DeleSelectButton;
        private System.Windows.Forms.ToolStripButton DelAllButton;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colNo;
    }
}