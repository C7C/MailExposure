namespace MailExposure.POPMailForm
{
    partial class PopMailControl
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PopMailControl));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.ButtonShowAllUsers = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.ButtonReceiveEmail = new System.Windows.Forms.ToolStripButton();
            this.ButtonStopReceiveEmail = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.ButtonAddUser = new System.Windows.Forms.ToolStripButton();
            this.ButtonEditeUser = new System.Windows.Forms.ToolStripButton();
            this.ButtonDeleteUser = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.ButtonExit = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripComboBoxAutoText = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripComboBoxThreadsText = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonAllReceive = new System.Windows.Forms.ToolStripButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.listBoxView = new System.Windows.Forms.ListBox();
            this.statusBarMessage = new System.Windows.Forms.StatusStrip();
            this.statusBarPanelLoginFail = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusBarPanelLoginFailText = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusBarPanelEmailNumbers = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusBarPanelEmailNumbersText = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusBarPanelUserNumber = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusBarPanelUserNumberText = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusBarPanelThreads = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusBarPanelThreadsText = new System.Windows.Forms.ToolStripStatusLabel();
            this.timerAutoReceive = new System.Windows.Forms.Timer(this.components);
            this.toolStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.statusBarMessage.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ButtonShowAllUsers,
            this.toolStripSeparator1,
            this.ButtonReceiveEmail,
            this.ButtonStopReceiveEmail,
            this.toolStripSeparator2,
            this.ButtonAddUser,
            this.ButtonEditeUser,
            this.ButtonDeleteUser,
            this.toolStripSeparator3,
            this.ButtonExit,
            this.toolStripSeparator4,
            this.toolStripSeparator5,
            this.toolStripLabel1,
            this.toolStripComboBoxAutoText,
            this.toolStripSeparator6,
            this.toolStripLabel2,
            this.toolStripComboBoxThreadsText,
            this.toolStripSeparator7,
            this.toolStripButtonAllReceive});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(767, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // ButtonShowAllUsers
            // 
            this.ButtonShowAllUsers.Image = ((System.Drawing.Image)(resources.GetObject("ButtonShowAllUsers.Image")));
            this.ButtonShowAllUsers.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonShowAllUsers.Name = "ButtonShowAllUsers";
            this.ButtonShowAllUsers.Size = new System.Drawing.Size(85, 22);
            this.ButtonShowAllUsers.Text = "显示用户组";
            this.ButtonShowAllUsers.Click += new System.EventHandler(this.ButtonShowAllUsers_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // ButtonReceiveEmail
            // 
            this.ButtonReceiveEmail.Image = ((System.Drawing.Image)(resources.GetObject("ButtonReceiveEmail.Image")));
            this.ButtonReceiveEmail.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonReceiveEmail.Name = "ButtonReceiveEmail";
            this.ButtonReceiveEmail.Size = new System.Drawing.Size(49, 22);
            this.ButtonReceiveEmail.Text = "接收";
            this.ButtonReceiveEmail.Click += new System.EventHandler(this.ButtonReceiveEmail_Click);
            // 
            // ButtonStopReceiveEmail
            // 
            this.ButtonStopReceiveEmail.Image = ((System.Drawing.Image)(resources.GetObject("ButtonStopReceiveEmail.Image")));
            this.ButtonStopReceiveEmail.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonStopReceiveEmail.Name = "ButtonStopReceiveEmail";
            this.ButtonStopReceiveEmail.Size = new System.Drawing.Size(49, 22);
            this.ButtonStopReceiveEmail.Text = "停止";
            this.ButtonStopReceiveEmail.Click += new System.EventHandler(this.ButtonStopReceiveEmail_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // ButtonAddUser
            // 
            this.ButtonAddUser.Image = ((System.Drawing.Image)(resources.GetObject("ButtonAddUser.Image")));
            this.ButtonAddUser.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonAddUser.Name = "ButtonAddUser";
            this.ButtonAddUser.Size = new System.Drawing.Size(73, 22);
            this.ButtonAddUser.Text = "新增用户";
            this.ButtonAddUser.Click += new System.EventHandler(this.ButtonAddUser_Click);
            // 
            // ButtonEditeUser
            // 
            this.ButtonEditeUser.Image = global::MailExposure.XRDesignRibbonControllerResources.RibbonUserDesigner_Paste;
            this.ButtonEditeUser.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonEditeUser.Name = "ButtonEditeUser";
            this.ButtonEditeUser.Size = new System.Drawing.Size(49, 22);
            this.ButtonEditeUser.Text = "编辑";
            this.ButtonEditeUser.Click += new System.EventHandler(this.ButtonEditeUser_Click);
            // 
            // ButtonDeleteUser
            // 
            this.ButtonDeleteUser.Image = global::MailExposure.XRDesignRibbonControllerResources.RibbonUserDesigner_HtmlStop;
            this.ButtonDeleteUser.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonDeleteUser.Name = "ButtonDeleteUser";
            this.ButtonDeleteUser.Size = new System.Drawing.Size(49, 22);
            this.ButtonDeleteUser.Text = "删除";
            this.ButtonDeleteUser.Click += new System.EventHandler(this.ButtonDeleteUser_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // ButtonExit
            // 
            this.ButtonExit.Image = ((System.Drawing.Image)(resources.GetObject("ButtonExit.Image")));
            this.ButtonExit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonExit.Name = "ButtonExit";
            this.ButtonExit.Size = new System.Drawing.Size(73, 22);
            this.ButtonExit.Text = "退出登录";
            this.ButtonExit.Click += new System.EventHandler(this.ButtonExit_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(35, 22);
            this.toolStripLabel1.Text = "自动:";
            // 
            // toolStripComboBoxAutoText
            // 
            this.toolStripComboBoxAutoText.AutoSize = false;
            this.toolStripComboBoxAutoText.DropDownHeight = 50;
            this.toolStripComboBoxAutoText.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.toolStripComboBoxAutoText.DropDownWidth = 50;
            this.toolStripComboBoxAutoText.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.toolStripComboBoxAutoText.IntegralHeight = false;
            this.toolStripComboBoxAutoText.Items.AddRange(new object[] {
            "是",
            "否"});
            this.toolStripComboBoxAutoText.Name = "toolStripComboBoxAutoText";
            this.toolStripComboBoxAutoText.Size = new System.Drawing.Size(35, 20);
            this.toolStripComboBoxAutoText.SelectedIndexChanged += new System.EventHandler(this.toolStripComboBoxAutoText_SelectedIndexChanged);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(47, 22);
            this.toolStripLabel2.Text = "线程数:";
            // 
            // toolStripComboBoxThreadsText
            // 
            this.toolStripComboBoxThreadsText.AutoSize = false;
            this.toolStripComboBoxThreadsText.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "10",
            "15",
            "20",
            "35",
            "40",
            "50"});
            this.toolStripComboBoxThreadsText.Name = "toolStripComboBoxThreadsText";
            this.toolStripComboBoxThreadsText.Size = new System.Drawing.Size(50, 20);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButtonAllReceive
            // 
            this.toolStripButtonAllReceive.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonAllReceive.Image")));
            this.toolStripButtonAllReceive.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonAllReceive.Name = "toolStripButtonAllReceive";
            this.toolStripButtonAllReceive.Size = new System.Drawing.Size(73, 22);
            this.toolStripButtonAllReceive.Text = "全部接受";
            this.toolStripButtonAllReceive.Click += new System.EventHandler(this.toolStripButtonAllReceive_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.splitContainer1);
            this.panel1.Controls.Add(this.statusBarMessage);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 25);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(767, 415);
            this.panel1.TabIndex = 1;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.dataGridView);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.listBoxView);
            this.splitContainer1.Size = new System.Drawing.Size(767, 393);
            this.splitContainer1.SplitterDistance = 223;
            this.splitContainer1.TabIndex = 1;
            // 
            // dataGridView
            // 
            this.dataGridView.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.AllowUserToDeleteRows = false;
            this.dataGridView.AllowUserToResizeColumns = false;
            this.dataGridView.AllowUserToResizeRows = false;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView.Location = new System.Drawing.Point(0, 0);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.ReadOnly = true;
            this.dataGridView.RowTemplate.Height = 23;
            this.dataGridView.Size = new System.Drawing.Size(767, 223);
            this.dataGridView.TabIndex = 0;
            this.dataGridView.DoubleClick += new System.EventHandler(this.dataGridView_DoubleClick);
            // 
            // listBoxView
            // 
            this.listBoxView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxView.FormattingEnabled = true;
            this.listBoxView.ItemHeight = 12;
            this.listBoxView.Location = new System.Drawing.Point(0, 0);
            this.listBoxView.Name = "listBoxView";
            this.listBoxView.Size = new System.Drawing.Size(767, 166);
            this.listBoxView.TabIndex = 0;
            // 
            // statusBarMessage
            // 
            this.statusBarMessage.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusBarPanelLoginFail,
            this.statusBarPanelLoginFailText,
            this.statusBarPanelEmailNumbers,
            this.statusBarPanelEmailNumbersText,
            this.statusBarPanelUserNumber,
            this.statusBarPanelUserNumberText,
            this.statusBarPanelThreads,
            this.statusBarPanelThreadsText});
            this.statusBarMessage.Location = new System.Drawing.Point(0, 393);
            this.statusBarMessage.Name = "statusBarMessage";
            this.statusBarMessage.Size = new System.Drawing.Size(767, 22);
            this.statusBarMessage.TabIndex = 0;
            this.statusBarMessage.Text = "statusStrip1";
            // 
            // statusBarPanelLoginFail
            // 
            this.statusBarPanelLoginFail.Image = ((System.Drawing.Image)(resources.GetObject("statusBarPanelLoginFail.Image")));
            this.statusBarPanelLoginFail.Name = "statusBarPanelLoginFail";
            this.statusBarPanelLoginFail.Size = new System.Drawing.Size(75, 17);
            this.statusBarPanelLoginFail.Text = "登录失败:";
            // 
            // statusBarPanelLoginFailText
            // 
            this.statusBarPanelLoginFailText.Name = "statusBarPanelLoginFailText";
            this.statusBarPanelLoginFailText.Size = new System.Drawing.Size(17, 17);
            this.statusBarPanelLoginFailText.Text = "  ";
            // 
            // statusBarPanelEmailNumbers
            // 
            this.statusBarPanelEmailNumbers.Image = ((System.Drawing.Image)(resources.GetObject("statusBarPanelEmailNumbers.Image")));
            this.statusBarPanelEmailNumbers.Name = "statusBarPanelEmailNumbers";
            this.statusBarPanelEmailNumbers.Size = new System.Drawing.Size(75, 17);
            this.statusBarPanelEmailNumbers.Text = "收取邮件:";
            // 
            // statusBarPanelEmailNumbersText
            // 
            this.statusBarPanelEmailNumbersText.Name = "statusBarPanelEmailNumbersText";
            this.statusBarPanelEmailNumbersText.Size = new System.Drawing.Size(17, 17);
            this.statusBarPanelEmailNumbersText.Text = "  ";
            // 
            // statusBarPanelUserNumber
            // 
            this.statusBarPanelUserNumber.Image = ((System.Drawing.Image)(resources.GetObject("statusBarPanelUserNumber.Image")));
            this.statusBarPanelUserNumber.Name = "statusBarPanelUserNumber";
            this.statusBarPanelUserNumber.Size = new System.Drawing.Size(75, 17);
            this.statusBarPanelUserNumber.Text = "用户数量:";
            // 
            // statusBarPanelUserNumberText
            // 
            this.statusBarPanelUserNumberText.Name = "statusBarPanelUserNumberText";
            this.statusBarPanelUserNumberText.Size = new System.Drawing.Size(17, 17);
            this.statusBarPanelUserNumberText.Text = "  ";
            // 
            // statusBarPanelThreads
            // 
            this.statusBarPanelThreads.Image = ((System.Drawing.Image)(resources.GetObject("statusBarPanelThreads.Image")));
            this.statusBarPanelThreads.Name = "statusBarPanelThreads";
            this.statusBarPanelThreads.Size = new System.Drawing.Size(75, 17);
            this.statusBarPanelThreads.Text = "完成线程:";
            // 
            // statusBarPanelThreadsText
            // 
            this.statusBarPanelThreadsText.Name = "statusBarPanelThreadsText";
            this.statusBarPanelThreadsText.Size = new System.Drawing.Size(17, 17);
            this.statusBarPanelThreadsText.Text = "  ";
            // 
            // timerAutoReceive
            // 
            this.timerAutoReceive.Enabled = true;
            this.timerAutoReceive.Tick += new System.EventHandler(this.timerAutoReceive_Tick);
            // 
            // PopMailControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "PopMailControl";
            this.Size = new System.Drawing.Size(767, 440);
            this.Load += new System.EventHandler(this.PopMailControl_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.statusBarMessage.ResumeLayout(false);
            this.statusBarMessage.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton ButtonShowAllUsers;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton ButtonReceiveEmail;
        private System.Windows.Forms.ToolStripButton ButtonStopReceiveEmail;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton ButtonAddUser;
        private System.Windows.Forms.ToolStripButton ButtonEditeUser;
        private System.Windows.Forms.ToolStripButton ButtonDeleteUser;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton ButtonExit;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.StatusStrip statusBarMessage;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBoxAutoText;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBoxThreadsText;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripButton toolStripButtonAllReceive;
        private System.Windows.Forms.ToolStripStatusLabel statusBarPanelLoginFail;
        private System.Windows.Forms.ToolStripStatusLabel statusBarPanelEmailNumbers;
        private System.Windows.Forms.ToolStripStatusLabel statusBarPanelUserNumber;
        private System.Windows.Forms.ToolStripStatusLabel statusBarPanelThreads;
        public System.Windows.Forms.ToolStripStatusLabel statusBarPanelLoginFailText;
        public System.Windows.Forms.ToolStripStatusLabel statusBarPanelEmailNumbersText;
        public System.Windows.Forms.ToolStripStatusLabel statusBarPanelUserNumberText;
        public System.Windows.Forms.ToolStripStatusLabel statusBarPanelThreadsText;
        public System.Windows.Forms.ListBox listBoxView;
        public System.Windows.Forms.Timer timerAutoReceive;
        public System.Windows.Forms.DataGridView dataGridView;
    }
}
