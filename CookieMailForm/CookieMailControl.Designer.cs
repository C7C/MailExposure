namespace MailExposure
{
    partial class CookieMailControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("未过滤用户");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("过滤后用户");
            this.CookieMailMainCommand = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonReceive = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonStopRec = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonAllReceive = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonStop = new System.Windows.Forms.ToolStripButton();
            this.ButtonEditeUser = new System.Windows.Forms.ToolStripButton();
            this.ButtonDeleteUser = new System.Windows.Forms.ToolStripButton();
            this.ButtonAddUser = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.panel1 = new System.Windows.Forms.Panel();
            this.splitPanel = new System.Windows.Forms.SplitContainer();
            this.treeViewUsers = new System.Windows.Forms.TreeView();
            this.contextMenuStripUserList = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.编辑用户ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.编辑用户列表ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.添加用户列表ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.删除列表ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.删除用户ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.自动获取CookieToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControlusers = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.listBoxView = new System.Windows.Forms.ListBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.webBrowserView = new System.Windows.Forms.WebBrowser();
            this.panel2 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.addressTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.forwardToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.backToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.stopToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.refreshToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.homeToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.searchToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.printToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.printPreviewToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusBarPanelLoginFailText = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusBarPanelEmailNumbersText = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel4 = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusBarPanelUserNumberText = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusBarPanelThreadsText = new System.Windows.Forms.ToolStripStatusLabel();
            this.usersBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.cookieMailDataBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.cookieMailData = new MailExposure.CookieMailDB.CookieMailData();
            this.usersTableAdapter = new MailExposure.CookieMailDB.CookieMailDataTableAdapters.usersTableAdapter();
            this.timerGetCookiesAuto = new System.Windows.Forms.Timer(this.components);
            this.timerAutoReceive = new System.Windows.Forms.Timer(this.components);
            this.CookieMailMainCommand.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitPanel)).BeginInit();
            this.splitPanel.Panel1.SuspendLayout();
            this.splitPanel.Panel2.SuspendLayout();
            this.splitPanel.SuspendLayout();
            this.contextMenuStripUserList.SuspendLayout();
            this.tabControlusers.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.panel2.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.statusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.usersBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cookieMailDataBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cookieMailData)).BeginInit();
            this.SuspendLayout();
            // 
            // CookieMailMainCommand
            // 
            this.CookieMailMainCommand.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonReceive,
            this.toolStripButtonStopRec,
            this.toolStripButtonAllReceive,
            this.toolStripButtonStop,
            this.ButtonEditeUser,
            this.ButtonDeleteUser,
            this.ButtonAddUser,
            this.toolStripSeparator5});
            this.CookieMailMainCommand.Location = new System.Drawing.Point(0, 0);
            this.CookieMailMainCommand.Name = "CookieMailMainCommand";
            this.CookieMailMainCommand.Size = new System.Drawing.Size(875, 25);
            this.CookieMailMainCommand.TabIndex = 0;
            this.CookieMailMainCommand.Text = "toolStrip1";
            // 
            // toolStripButtonReceive
            // 
            this.toolStripButtonReceive.Image = global::MailExposure.XRDesignRibbonControllerResources.RibbonUserDesigner_HtmlHome;
            this.toolStripButtonReceive.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonReceive.Name = "toolStripButtonReceive";
            this.toolStripButtonReceive.Size = new System.Drawing.Size(49, 22);
            this.toolStripButtonReceive.Text = "接收";
            this.toolStripButtonReceive.Click += new System.EventHandler(this.toolStripButtonReceive_Click);
            // 
            // toolStripButtonStopRec
            // 
            this.toolStripButtonStopRec.Image = global::MailExposure.PrintRibbonControllerResources.RibbonPrintPreview_ClosePreview;
            this.toolStripButtonStopRec.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonStopRec.Name = "toolStripButtonStopRec";
            this.toolStripButtonStopRec.Size = new System.Drawing.Size(49, 22);
            this.toolStripButtonStopRec.Text = "停止";
            this.toolStripButtonStopRec.Click += new System.EventHandler(this.toolStripButtonStopRec_Click);
            // 
            // toolStripButtonAllReceive
            // 
            this.toolStripButtonAllReceive.Image = global::MailExposure.PrintRibbonControllerResources.RibbonPrintPreview_MultiplePagesLarge;
            this.toolStripButtonAllReceive.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonAllReceive.Name = "toolStripButtonAllReceive";
            this.toolStripButtonAllReceive.Size = new System.Drawing.Size(73, 22);
            this.toolStripButtonAllReceive.Text = "全部接收";
            this.toolStripButtonAllReceive.Click += new System.EventHandler(this.toolStripButtonAllReceive_Click);
            // 
            // toolStripButtonStop
            // 
            this.toolStripButtonStop.Image = global::MailExposure.XRDesignRibbonControllerResources.RibbonUserDesigner_HtmlRefresh;
            this.toolStripButtonStop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonStop.Name = "toolStripButtonStop";
            this.toolStripButtonStop.Size = new System.Drawing.Size(73, 22);
            this.toolStripButtonStop.Text = "全部停止";
            this.toolStripButtonStop.Click += new System.EventHandler(this.toolStripButtonStop_Click);
            // 
            // ButtonEditeUser
            // 
            this.ButtonEditeUser.Image = global::MailExposure.PrintRibbonControllerResources.RibbonPrintPreview_Parameters;
            this.ButtonEditeUser.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonEditeUser.Name = "ButtonEditeUser";
            this.ButtonEditeUser.Size = new System.Drawing.Size(49, 22);
            this.ButtonEditeUser.Text = "编辑";
            this.ButtonEditeUser.Click += new System.EventHandler(this.ButtonEditeUser_Click);
            // 
            // ButtonDeleteUser
            // 
            this.ButtonDeleteUser.Image = global::MailExposure.PrintRibbonControllerResources.RibbonPrintPreview_Watermark;
            this.ButtonDeleteUser.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonDeleteUser.Name = "ButtonDeleteUser";
            this.ButtonDeleteUser.Size = new System.Drawing.Size(49, 22);
            this.ButtonDeleteUser.Text = "删除";
            this.ButtonDeleteUser.Click += new System.EventHandler(this.ButtonDeleteUser_Click);
            // 
            // ButtonAddUser
            // 
            this.ButtonAddUser.Image = global::MailExposure.XRDesignRibbonControllerResources.RibbonUserDesigner_Paste;
            this.ButtonAddUser.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonAddUser.Name = "ButtonAddUser";
            this.ButtonAddUser.Size = new System.Drawing.Size(97, 22);
            this.ButtonAddUser.Text = "添加用户列表";
            this.ButtonAddUser.Click += new System.EventHandler(this.ButtonAddUser_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.splitPanel);
            this.panel1.Controls.Add(this.statusStrip);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 25);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(875, 543);
            this.panel1.TabIndex = 1;
            // 
            // splitPanel
            // 
            this.splitPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitPanel.Location = new System.Drawing.Point(0, 0);
            this.splitPanel.Name = "splitPanel";
            // 
            // splitPanel.Panel1
            // 
            this.splitPanel.Panel1.Controls.Add(this.treeViewUsers);
            // 
            // splitPanel.Panel2
            // 
            this.splitPanel.Panel2.Controls.Add(this.tabControlusers);
            this.splitPanel.Size = new System.Drawing.Size(875, 521);
            this.splitPanel.SplitterDistance = 291;
            this.splitPanel.TabIndex = 6;
            // 
            // treeViewUsers
            // 
            this.treeViewUsers.ContextMenuStrip = this.contextMenuStripUserList;
            this.treeViewUsers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewUsers.Location = new System.Drawing.Point(0, 0);
            this.treeViewUsers.Name = "treeViewUsers";
            treeNode3.Name = "节点0";
            treeNode3.Text = "未过滤用户";
            treeNode4.Name = "节点1";
            treeNode4.Text = "过滤后用户";
            this.treeViewUsers.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode3,
            treeNode4});
            this.treeViewUsers.Size = new System.Drawing.Size(291, 521);
            this.treeViewUsers.TabIndex = 1;
            // 
            // contextMenuStripUserList
            // 
            this.contextMenuStripUserList.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.编辑用户ToolStripMenuItem,
            this.编辑用户列表ToolStripMenuItem,
            this.添加用户列表ToolStripMenuItem,
            this.删除列表ToolStripMenuItem,
            this.删除用户ToolStripMenuItem,
            this.自动获取CookieToolStripMenuItem});
            this.contextMenuStripUserList.Name = "contextMenuStrip1";
            this.contextMenuStripUserList.Size = new System.Drawing.Size(143, 136);
            // 
            // 编辑用户ToolStripMenuItem
            // 
            this.编辑用户ToolStripMenuItem.Name = "编辑用户ToolStripMenuItem";
            this.编辑用户ToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.编辑用户ToolStripMenuItem.Text = "编辑用户";
            this.编辑用户ToolStripMenuItem.Click += new System.EventHandler(this.编辑用户ToolStripMenuItem_Click);
            // 
            // 编辑用户列表ToolStripMenuItem
            // 
            this.编辑用户列表ToolStripMenuItem.Name = "编辑用户列表ToolStripMenuItem";
            this.编辑用户列表ToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.编辑用户列表ToolStripMenuItem.Text = "编辑用户列表";
            this.编辑用户列表ToolStripMenuItem.Click += new System.EventHandler(this.编辑用户列表ToolStripMenuItem_Click);
            // 
            // 添加用户列表ToolStripMenuItem
            // 
            this.添加用户列表ToolStripMenuItem.Name = "添加用户列表ToolStripMenuItem";
            this.添加用户列表ToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.添加用户列表ToolStripMenuItem.Text = "添加用户列表";
            this.添加用户列表ToolStripMenuItem.Click += new System.EventHandler(this.添加用户列表ToolStripMenuItem_Click);
            // 
            // 删除列表ToolStripMenuItem
            // 
            this.删除列表ToolStripMenuItem.Name = "删除列表ToolStripMenuItem";
            this.删除列表ToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.删除列表ToolStripMenuItem.Text = "删除列表";
            this.删除列表ToolStripMenuItem.Click += new System.EventHandler(this.删除列表ToolStripMenuItem_Click);
            // 
            // 删除用户ToolStripMenuItem
            // 
            this.删除用户ToolStripMenuItem.Name = "删除用户ToolStripMenuItem";
            this.删除用户ToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.删除用户ToolStripMenuItem.Text = "删除用户";
            this.删除用户ToolStripMenuItem.Click += new System.EventHandler(this.删除用户ToolStripMenuItem_Click);
            // 
            // 自动获取CookieToolStripMenuItem
            // 
            this.自动获取CookieToolStripMenuItem.Name = "自动获取CookieToolStripMenuItem";
            this.自动获取CookieToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.自动获取CookieToolStripMenuItem.Text = "自动获取";
            this.自动获取CookieToolStripMenuItem.CheckStateChanged += new System.EventHandler(this.自动获取CookieToolStripMenuItem_CheckStateChanged);
            this.自动获取CookieToolStripMenuItem.Click += new System.EventHandler(this.自动获取ToolStripMenuItem_Click);
            // 
            // tabControlusers
            // 
            this.tabControlusers.Controls.Add(this.tabPage1);
            this.tabControlusers.Controls.Add(this.tabPage2);
            this.tabControlusers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlusers.Location = new System.Drawing.Point(0, 0);
            this.tabControlusers.Name = "tabControlusers";
            this.tabControlusers.SelectedIndex = 0;
            this.tabControlusers.Size = new System.Drawing.Size(580, 521);
            this.tabControlusers.TabIndex = 9;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.splitContainer2);
            this.tabPage1.Location = new System.Drawing.Point(4, 21);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(572, 496);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "用户";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(3, 3);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.dataGridView);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.listBoxView);
            this.splitContainer2.Size = new System.Drawing.Size(566, 490);
            this.splitContainer2.SplitterDistance = 295;
            this.splitContainer2.TabIndex = 0;
            // 
            // dataGridView
            // 
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.AllowUserToResizeRows = false;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView.EnableHeadersVisualStyles = false;
            this.dataGridView.Location = new System.Drawing.Point(0, 0);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.ReadOnly = true;
            this.dataGridView.RowHeadersVisible = false;
            this.dataGridView.RowTemplate.Height = 23;
            this.dataGridView.Size = new System.Drawing.Size(566, 295);
            this.dataGridView.TabIndex = 0;
            this.dataGridView.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.dataGridView_MouseDoubleClick);
            // 
            // listBoxView
            // 
            this.listBoxView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxView.FormattingEnabled = true;
            this.listBoxView.ItemHeight = 12;
            this.listBoxView.Location = new System.Drawing.Point(0, 0);
            this.listBoxView.Name = "listBoxView";
            this.listBoxView.Size = new System.Drawing.Size(566, 191);
            this.listBoxView.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.webBrowserView);
            this.tabPage2.Controls.Add(this.panel2);
            this.tabPage2.Controls.Add(this.toolStrip1);
            this.tabPage2.Location = new System.Drawing.Point(4, 21);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(572, 496);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Cookie浏览器";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // webBrowserView
            // 
            this.webBrowserView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowserView.Location = new System.Drawing.Point(3, 74);
            this.webBrowserView.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowserView.Name = "webBrowserView";
            this.webBrowserView.Size = new System.Drawing.Size(566, 419);
            this.webBrowserView.TabIndex = 3;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.button1);
            this.panel2.Controls.Add(this.addressTextBox);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(3, 28);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(566, 46);
            this.panel2.TabIndex = 2;
            // 
            // button1
            // 
            this.button1.Image = global::MailExposure.XRDesignRibbonControllerResources.RibbonUserDesigner_Redo;
            this.button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button1.Location = new System.Drawing.Point(556, 14);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(56, 21);
            this.button1.TabIndex = 3;
            this.button1.Text = "打开";
            this.button1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.button1.UseVisualStyleBackColor = true;
            // 
            // addressTextBox
            // 
            this.addressTextBox.Location = new System.Drawing.Point(69, 14);
            this.addressTextBox.Name = "addressTextBox";
            this.addressTextBox.Size = new System.Drawing.Size(481, 21);
            this.addressTextBox.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "地址栏:";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator1,
            this.forwardToolStripButton,
            this.backToolStripButton,
            this.toolStripSeparator2,
            this.stopToolStripButton,
            this.refreshToolStripButton,
            this.toolStripSeparator3,
            this.homeToolStripButton,
            this.searchToolStripButton,
            this.toolStripSeparator4,
            this.printToolStripButton,
            this.printPreviewToolStripButton});
            this.toolStrip1.Location = new System.Drawing.Point(3, 3);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(566, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // forwardToolStripButton
            // 
            this.forwardToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.forwardToolStripButton.Image = global::MailExposure.XRDesignRibbonControllerResources.RibbonUserDesigner_HtmlBackward;
            this.forwardToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.forwardToolStripButton.Name = "forwardToolStripButton";
            this.forwardToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.forwardToolStripButton.Text = "toolStripButton1";
            // 
            // backToolStripButton
            // 
            this.backToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.backToolStripButton.Image = global::MailExposure.XRDesignRibbonControllerResources.RibbonUserDesigner_HtmlForward;
            this.backToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.backToolStripButton.Name = "backToolStripButton";
            this.backToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.backToolStripButton.Text = "toolStripButton2";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // stopToolStripButton
            // 
            this.stopToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.stopToolStripButton.Image = global::MailExposure.PrintRibbonControllerResources.RibbonPrintPreview_ClosePreview;
            this.stopToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.stopToolStripButton.Name = "stopToolStripButton";
            this.stopToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.stopToolStripButton.Text = "toolStripButton3";
            // 
            // refreshToolStripButton
            // 
            this.refreshToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.refreshToolStripButton.Image = global::MailExposure.XRDesignRibbonControllerResources.RibbonUserDesigner_HtmlRefresh;
            this.refreshToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.refreshToolStripButton.Name = "refreshToolStripButton";
            this.refreshToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.refreshToolStripButton.Text = "toolStripButton4";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // homeToolStripButton
            // 
            this.homeToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.homeToolStripButton.Image = global::MailExposure.XRDesignRibbonControllerResources.RibbonUserDesigner_HtmlHome;
            this.homeToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.homeToolStripButton.Name = "homeToolStripButton";
            this.homeToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.homeToolStripButton.Text = "toolStripButton5";
            // 
            // searchToolStripButton
            // 
            this.searchToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.searchToolStripButton.Image = global::MailExposure.XRDesignRibbonControllerResources.RibbonUserDesigner_HtmlFind;
            this.searchToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.searchToolStripButton.Name = "searchToolStripButton";
            this.searchToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.searchToolStripButton.Text = "toolStripButton6";
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // printToolStripButton
            // 
            this.printToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.printToolStripButton.Image = global::MailExposure.PrintRibbonControllerResources.RibbonPrintPreview_PrintDirect;
            this.printToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.printToolStripButton.Name = "printToolStripButton";
            this.printToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.printToolStripButton.Text = "toolStripButton7";
            // 
            // printPreviewToolStripButton
            // 
            this.printPreviewToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.printPreviewToolStripButton.Image = global::MailExposure.PrintRibbonControllerResources.RibbonPrintPreview_Magnifier;
            this.printPreviewToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.printPreviewToolStripButton.Name = "printPreviewToolStripButton";
            this.printPreviewToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.printPreviewToolStripButton.Text = "toolStripButton8";
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel3,
            this.statusBarPanelLoginFailText,
            this.toolStripStatusLabel2,
            this.statusBarPanelEmailNumbersText,
            this.toolStripStatusLabel4,
            this.statusBarPanelUserNumberText,
            this.toolStripStatusLabel1,
            this.statusBarPanelThreadsText});
            this.statusStrip.Location = new System.Drawing.Point(0, 521);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(875, 22);
            this.statusStrip.TabIndex = 5;
            this.statusStrip.TabStop = true;
            this.statusStrip.Text = "statusStrip1";
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(59, 17);
            this.toolStripStatusLabel3.Text = "登录失败:";
            // 
            // statusBarPanelLoginFailText
            // 
            this.statusBarPanelLoginFailText.Name = "statusBarPanelLoginFailText";
            this.statusBarPanelLoginFailText.Size = new System.Drawing.Size(11, 17);
            this.statusBarPanelLoginFailText.Text = " ";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(59, 17);
            this.toolStripStatusLabel2.Text = "收取邮件:";
            // 
            // statusBarPanelEmailNumbersText
            // 
            this.statusBarPanelEmailNumbersText.Name = "statusBarPanelEmailNumbersText";
            this.statusBarPanelEmailNumbersText.Size = new System.Drawing.Size(11, 17);
            this.statusBarPanelEmailNumbersText.Text = " ";
            // 
            // toolStripStatusLabel4
            // 
            this.toolStripStatusLabel4.Name = "toolStripStatusLabel4";
            this.toolStripStatusLabel4.Size = new System.Drawing.Size(35, 17);
            this.toolStripStatusLabel4.Text = "用户:";
            // 
            // statusBarPanelUserNumberText
            // 
            this.statusBarPanelUserNumberText.Name = "statusBarPanelUserNumberText";
            this.statusBarPanelUserNumberText.Size = new System.Drawing.Size(11, 17);
            this.statusBarPanelUserNumberText.Text = " ";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(59, 17);
            this.toolStripStatusLabel1.Text = "活动线程:";
            // 
            // statusBarPanelThreadsText
            // 
            this.statusBarPanelThreadsText.Name = "statusBarPanelThreadsText";
            this.statusBarPanelThreadsText.Size = new System.Drawing.Size(11, 17);
            this.statusBarPanelThreadsText.Text = " ";
            // 
            // usersBindingSource
            // 
            this.usersBindingSource.DataMember = "users";
            this.usersBindingSource.DataSource = this.cookieMailDataBindingSource;
            // 
            // cookieMailDataBindingSource
            // 
            this.cookieMailDataBindingSource.DataSource = this.cookieMailData;
            this.cookieMailDataBindingSource.Position = 0;
            // 
            // cookieMailData
            // 
            this.cookieMailData.DataSetName = "CookieMailData";
            this.cookieMailData.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // usersTableAdapter
            // 
            this.usersTableAdapter.ClearBeforeFill = true;
            // 
            // timerGetCookiesAuto
            // 
            this.timerGetCookiesAuto.Tick += new System.EventHandler(this.timerGetCookiesAuto_Tick);
            // 
            // timerAutoReceive
            // 
            this.timerAutoReceive.Enabled = true;
            this.timerAutoReceive.Tick += new System.EventHandler(this.timerAutoReceive_Tick);
            // 
            // CookieMailControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.CookieMailMainCommand);
            this.Name = "CookieMailControl";
            this.Size = new System.Drawing.Size(875, 568);
            this.Load += new System.EventHandler(this.CookieMailControl_Load);
            this.CookieMailMainCommand.ResumeLayout(false);
            this.CookieMailMainCommand.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.splitPanel.Panel1.ResumeLayout(false);
            this.splitPanel.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitPanel)).EndInit();
            this.splitPanel.ResumeLayout(false);
            this.contextMenuStripUserList.ResumeLayout(false);
            this.tabControlusers.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.usersBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cookieMailDataBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cookieMailData)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip CookieMailMainCommand;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.BindingSource usersBindingSource;
        private System.Windows.Forms.BindingSource cookieMailDataBindingSource;
        private CookieMailDB.CookieMailData cookieMailData;
        private CookieMailDB.CookieMailDataTableAdapters.usersTableAdapter usersTableAdapter;
        private System.Windows.Forms.ToolStripButton toolStripButtonReceive;
        private System.Windows.Forms.ToolStripButton toolStripButtonStopRec;
        private System.Windows.Forms.ToolStripButton toolStripButtonAllReceive;
        private System.Windows.Forms.ToolStripButton toolStripButtonStop;
        private System.Windows.Forms.ToolStripButton ButtonEditeUser;
        private System.Windows.Forms.ToolStripButton ButtonDeleteUser;
        private System.Windows.Forms.ToolStripButton ButtonAddUser;
        public System.Windows.Forms.StatusStrip statusStrip;
        public System.Windows.Forms.ToolStripStatusLabel statusBarPanelThreadsText;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        public System.Windows.Forms.ToolStripStatusLabel statusBarPanelLoginFailText;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        public System.Windows.Forms.ToolStripStatusLabel statusBarPanelEmailNumbersText;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel4;
        private System.Windows.Forms.ToolStripStatusLabel statusBarPanelUserNumberText;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripUserList;
        private System.Windows.Forms.ToolStripMenuItem 编辑用户ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 编辑用户列表ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 添加用户列表ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 删除列表ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 删除用户ToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem 自动获取CookieToolStripMenuItem;
        private System.Windows.Forms.Timer timerGetCookiesAuto;
        public System.Windows.Forms.Timer timerAutoReceive;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.SplitContainer splitPanel;
        public System.Windows.Forms.TreeView treeViewUsers;
        private System.Windows.Forms.TabControl tabControlusers;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.DataGridView dataGridView;
        public System.Windows.Forms.ListBox listBoxView;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.WebBrowser webBrowserView;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox addressTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton forwardToolStripButton;
        private System.Windows.Forms.ToolStripButton backToolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton stopToolStripButton;
        private System.Windows.Forms.ToolStripButton refreshToolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton homeToolStripButton;
        private System.Windows.Forms.ToolStripButton searchToolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton printToolStripButton;
        private System.Windows.Forms.ToolStripButton printPreviewToolStripButton;
       
    }
}
