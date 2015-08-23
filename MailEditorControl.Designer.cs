namespace MailExposure
{
    partial class MailEditorControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MailEditorControl));
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.cmsContent = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cutToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteToolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripDesignType = new System.Windows.Forms.ToolStripButton();
            this.fontComboBox = new System.Windows.Forms.ToolStripComboBox();
            this.fontSizeComboBox = new System.Windows.Forms.ToolStripComboBox();
            this.boldButton = new System.Windows.Forms.ToolStripButton();
            this.italicButton = new System.Windows.Forms.ToolStripButton();
            this.underlineButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.FontColor = new System.Windows.Forms.ToolStripButton();
            this.BackgroundColor = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.linkButton = new System.Windows.Forms.ToolStripButton();
            this.InsertImage = new System.Windows.Forms.ToolStripButton();
            this.MailTextPrint = new System.Windows.Forms.ToolStripButton();
            this.SaveMailText = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.justifyLeftButton = new System.Windows.Forms.ToolStripButton();
            this.justifyCenterButton = new System.Windows.Forms.ToolStripButton();
            this.justifyRightButton = new System.Windows.Forms.ToolStripButton();
            this.justifyFullButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.indentButton = new System.Windows.Forms.ToolStripButton();
            this.outdentButton = new System.Windows.Forms.ToolStripButton();
            this.tspToolMenu = new System.Windows.Forms.ToolStrip();
            this.txtSource = new System.Windows.Forms.TextBox();
            this.wbrContent1 = new System.Windows.Forms.WebBrowser();
            this.cmsContent.SuspendLayout();
            this.tspToolMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer
            // 
            this.timer.Enabled = true;
            // 
            // cmsContent
            // 
            this.cmsContent.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cutToolStripMenuItem1,
            this.copyToolStripMenuItem2,
            this.pasteToolStripMenuItem3,
            this.deleteToolStripMenuItem});
            this.cmsContent.Name = "contextMenuStrip1";
            this.cmsContent.Size = new System.Drawing.Size(95, 92);
            // 
            // cutToolStripMenuItem1
            // 
            this.cutToolStripMenuItem1.Name = "cutToolStripMenuItem1";
            this.cutToolStripMenuItem1.Size = new System.Drawing.Size(94, 22);
            this.cutToolStripMenuItem1.Text = "剪切";
            this.cutToolStripMenuItem1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.cutToolStripMenuItem1.ToolTipText = "剪切";
            this.cutToolStripMenuItem1.Click += new System.EventHandler(this.cutToolStripMenuItem1_Click);
            // 
            // copyToolStripMenuItem2
            // 
            this.copyToolStripMenuItem2.Name = "copyToolStripMenuItem2";
            this.copyToolStripMenuItem2.Size = new System.Drawing.Size(94, 22);
            this.copyToolStripMenuItem2.Text = "复制";
            this.copyToolStripMenuItem2.Click += new System.EventHandler(this.copyToolStripMenuItem2_Click);
            // 
            // pasteToolStripMenuItem3
            // 
            this.pasteToolStripMenuItem3.Name = "pasteToolStripMenuItem3";
            this.pasteToolStripMenuItem3.Size = new System.Drawing.Size(94, 22);
            this.pasteToolStripMenuItem3.Text = "粘贴";
            this.pasteToolStripMenuItem3.Click += new System.EventHandler(this.pasteToolStripMenuItem3_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(94, 22);
            this.deleteToolStripMenuItem.Text = "删除";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // ToolStripDesignType
            // 
            this.ToolStripDesignType.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.ToolStripDesignType.ImageTransparentColor = System.Drawing.Color.DarkSlateBlue;
            this.ToolStripDesignType.Name = "ToolStripDesignType";
            this.ToolStripDesignType.Size = new System.Drawing.Size(33, 22);
            this.ToolStripDesignType.Text = "HTML";
            this.ToolStripDesignType.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
            this.ToolStripDesignType.ToolTipText = "Source Code";
            this.ToolStripDesignType.Click += new System.EventHandler(this.ToolStripDesignType_Click);
            // 
            // fontComboBox
            // 
            this.fontComboBox.AutoToolTip = true;
            this.fontComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.fontComboBox.Name = "fontComboBox";
            this.fontComboBox.Size = new System.Drawing.Size(110, 25);
            // 
            // fontSizeComboBox
            // 
            this.fontSizeComboBox.Name = "fontSizeComboBox";
            this.fontSizeComboBox.Size = new System.Drawing.Size(75, 25);
            // 
            // boldButton
            // 
            this.boldButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.boldButton.Image = global::MailExposure.Properties.Resources.ButtonBold;
            this.boldButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.boldButton.Name = "boldButton";
            this.boldButton.Size = new System.Drawing.Size(23, 22);
            this.boldButton.Text = "toolStripButton1";
            this.boldButton.ToolTipText = "粗体";
            this.boldButton.Click += new System.EventHandler(this.boldButton_Click);
            // 
            // italicButton
            // 
            this.italicButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.italicButton.Image = global::MailExposure.Properties.Resources.ButtonItalic;
            this.italicButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.italicButton.Name = "italicButton";
            this.italicButton.Size = new System.Drawing.Size(23, 22);
            this.italicButton.Text = "toolStripButton2";
            this.italicButton.ToolTipText = "斜体";
            this.italicButton.Click += new System.EventHandler(this.italicButton_Click);
            // 
            // underlineButton
            // 
            this.underlineButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.underlineButton.Image = global::MailExposure.Properties.Resources.ButtonUnderline;
            this.underlineButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.underlineButton.Name = "underlineButton";
            this.underlineButton.Size = new System.Drawing.Size(23, 22);
            this.underlineButton.Text = "toolStripButton3";
            this.underlineButton.ToolTipText = "下划线";
            this.underlineButton.Click += new System.EventHandler(this.underlineButton_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // FontColor
            // 
            this.FontColor.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.FontColor.Image = global::MailExposure.XRDesignRibbonControllerResources.RibbonUserDesigner_ForeColor;
            this.FontColor.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.FontColor.Name = "FontColor";
            this.FontColor.Size = new System.Drawing.Size(23, 22);
            this.FontColor.Text = "toolStripButton5";
            this.FontColor.ToolTipText = "字体颜色";
            this.FontColor.Click += new System.EventHandler(this.FontColor_Click);
            // 
            // BackgroundColor
            // 
            this.BackgroundColor.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.BackgroundColor.Image = global::MailExposure.PrintRibbonControllerResources.RibbonPrintPreview_FillBackground;
            this.BackgroundColor.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BackgroundColor.Name = "BackgroundColor";
            this.BackgroundColor.Size = new System.Drawing.Size(23, 22);
            this.BackgroundColor.Text = "toolStripButton6";
            this.BackgroundColor.ToolTipText = "背景颜色";
            this.BackgroundColor.Click += new System.EventHandler(this.BackgroundColor_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // linkButton
            // 
            this.linkButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.linkButton.Image = global::MailExposure.PrintRibbonControllerResources.RibbonPrintPreview_ScaleLarge;
            this.linkButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.linkButton.Name = "linkButton";
            this.linkButton.Size = new System.Drawing.Size(23, 22);
            this.linkButton.Text = "toolStripButton13";
            this.linkButton.ToolTipText = "超级链接";
            this.linkButton.Click += new System.EventHandler(this.linkButton_Click);
            // 
            // InsertImage
            // 
            this.InsertImage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.InsertImage.Image = global::MailExposure.XRDesignRibbonControllerResources.RibbonUserDesigner_WindowsLarge;
            this.InsertImage.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.InsertImage.Name = "InsertImage";
            this.InsertImage.Size = new System.Drawing.Size(23, 22);
            this.InsertImage.Text = "toolStripButton4";
            this.InsertImage.ToolTipText = "插入图片";
            this.InsertImage.Click += new System.EventHandler(this.InsertImage_Click);
            // 
            // MailTextPrint
            // 
            this.MailTextPrint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.MailTextPrint.Image = global::MailExposure.PrintRibbonControllerResources.RibbonPrintPreview_PrintDirect;
            this.MailTextPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.MailTextPrint.Name = "MailTextPrint";
            this.MailTextPrint.Size = new System.Drawing.Size(23, 22);
            this.MailTextPrint.Text = "toolStripButton7";
            this.MailTextPrint.ToolTipText = "打印邮件";
            this.MailTextPrint.Click += new System.EventHandler(this.MailTextPrint_Click);
            // 
            // SaveMailText
            // 
            this.SaveMailText.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.SaveMailText.Image = global::MailExposure.PrintRibbonControllerResources.RibbonPrintPreview_Save;
            this.SaveMailText.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.SaveMailText.Name = "SaveMailText";
            this.SaveMailText.Size = new System.Drawing.Size(23, 22);
            this.SaveMailText.Text = "toolStripButton8";
            this.SaveMailText.ToolTipText = "保存邮件";
            this.SaveMailText.Click += new System.EventHandler(this.SaveMailText_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // justifyLeftButton
            // 
            this.justifyLeftButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.justifyLeftButton.Image = global::MailExposure.Properties.Resources.ButtonLeft;
            this.justifyLeftButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.justifyLeftButton.Name = "justifyLeftButton";
            this.justifyLeftButton.Size = new System.Drawing.Size(23, 22);
            this.justifyLeftButton.Text = "toolStripButton9";
            this.justifyLeftButton.ToolTipText = "左对齐";
            this.justifyLeftButton.Click += new System.EventHandler(this.justifyLeftButton_Click);
            // 
            // justifyCenterButton
            // 
            this.justifyCenterButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.justifyCenterButton.Image = global::MailExposure.Properties.Resources.ButtonCenter;
            this.justifyCenterButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.justifyCenterButton.Name = "justifyCenterButton";
            this.justifyCenterButton.Size = new System.Drawing.Size(23, 22);
            this.justifyCenterButton.Text = "toolStripButton10";
            this.justifyCenterButton.ToolTipText = "居中";
            this.justifyCenterButton.Click += new System.EventHandler(this.justifyCenterButton_Click);
            // 
            // justifyRightButton
            // 
            this.justifyRightButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.justifyRightButton.Image = ((System.Drawing.Image)(resources.GetObject("justifyRightButton.Image")));
            this.justifyRightButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.justifyRightButton.Name = "justifyRightButton";
            this.justifyRightButton.Size = new System.Drawing.Size(23, 22);
            this.justifyRightButton.Text = "toolStripButton11";
            this.justifyRightButton.ToolTipText = "右对齐";
            this.justifyRightButton.Click += new System.EventHandler(this.justifyRightButton_Click);
            // 
            // justifyFullButton
            // 
            this.justifyFullButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.justifyFullButton.Image = global::MailExposure.Properties.Resources.ButtonJustify1;
            this.justifyFullButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.justifyFullButton.Name = "justifyFullButton";
            this.justifyFullButton.Size = new System.Drawing.Size(23, 22);
            this.justifyFullButton.Text = "toolStripButton12";
            this.justifyFullButton.ToolTipText = "分散";
            this.justifyFullButton.Click += new System.EventHandler(this.justifyFullButton_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // indentButton
            // 
            this.indentButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.indentButton.Image = global::MailExposure.XRDesignRibbonControllerResources.RibbonUserDesigner_HorizSpaceDecrease;
            this.indentButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.indentButton.Name = "indentButton";
            this.indentButton.Size = new System.Drawing.Size(23, 22);
            this.indentButton.Text = "toolStripButton1";
            this.indentButton.ToolTipText = "缩进";
            this.indentButton.Click += new System.EventHandler(this.indentButton_Click);
            // 
            // outdentButton
            // 
            this.outdentButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.outdentButton.Image = global::MailExposure.XRDesignRibbonControllerResources.RibbonUserDesigner_HorizSpaceIncrease;
            this.outdentButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.outdentButton.Name = "outdentButton";
            this.outdentButton.Size = new System.Drawing.Size(23, 22);
            this.outdentButton.Text = "toolStripButton2";
            this.outdentButton.ToolTipText = "取消缩进";
            this.outdentButton.Click += new System.EventHandler(this.outdentButton_Click);
            // 
            // tspToolMenu
            // 
            this.tspToolMenu.BackColor = System.Drawing.SystemColors.GrayText;
            this.tspToolMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripDesignType,
            this.fontComboBox,
            this.fontSizeComboBox,
            this.boldButton,
            this.italicButton,
            this.underlineButton,
            this.toolStripSeparator1,
            this.FontColor,
            this.BackgroundColor,
            this.toolStripSeparator2,
            this.linkButton,
            this.InsertImage,
            this.MailTextPrint,
            this.SaveMailText,
            this.toolStripSeparator3,
            this.justifyLeftButton,
            this.justifyCenterButton,
            this.justifyRightButton,
            this.justifyFullButton,
            this.toolStripSeparator4,
            this.indentButton,
            this.outdentButton});
            this.tspToolMenu.Location = new System.Drawing.Point(0, 0);
            this.tspToolMenu.Name = "tspToolMenu";
            this.tspToolMenu.Size = new System.Drawing.Size(619, 25);
            this.tspToolMenu.TabIndex = 0;
            this.tspToolMenu.Text = "toolStrip1";
            // 
            // txtSource
            // 
            this.txtSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSource.Location = new System.Drawing.Point(0, 25);
            this.txtSource.Multiline = true;
            this.txtSource.Name = "txtSource";
            this.txtSource.Size = new System.Drawing.Size(619, 334);
            this.txtSource.TabIndex = 4;
            // 
            // wbrContent1
            // 
            this.wbrContent1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wbrContent1.Location = new System.Drawing.Point(0, 25);
            this.wbrContent1.MinimumSize = new System.Drawing.Size(20, 20);
            this.wbrContent1.Name = "wbrContent1";
            this.wbrContent1.Size = new System.Drawing.Size(619, 334);
            this.wbrContent1.TabIndex = 5;
            // 
            // MailEditorControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.wbrContent1);
            this.Controls.Add(this.txtSource);
            this.Controls.Add(this.tspToolMenu);
            this.Name = "MailEditorControl";
            this.Size = new System.Drawing.Size(619, 359);
            this.cmsContent.ResumeLayout(false);
            this.tspToolMenu.ResumeLayout(false);
            this.tspToolMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.ContextMenuStrip cmsContent;
        private System.Windows.Forms.ToolStripMenuItem cutToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton ToolStripDesignType;
        private System.Windows.Forms.ToolStripComboBox fontComboBox;
        private System.Windows.Forms.ToolStripComboBox fontSizeComboBox;
        private System.Windows.Forms.ToolStripButton boldButton;
        private System.Windows.Forms.ToolStripButton italicButton;
        private System.Windows.Forms.ToolStripButton underlineButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton FontColor;
        private System.Windows.Forms.ToolStripButton BackgroundColor;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton linkButton;
        private System.Windows.Forms.ToolStripButton InsertImage;
        private System.Windows.Forms.ToolStripButton MailTextPrint;
        private System.Windows.Forms.ToolStripButton SaveMailText;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton justifyLeftButton;
        private System.Windows.Forms.ToolStripButton justifyCenterButton;
        private System.Windows.Forms.ToolStripButton justifyRightButton;
        private System.Windows.Forms.ToolStripButton justifyFullButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton indentButton;
        private System.Windows.Forms.ToolStripButton outdentButton;
        private System.Windows.Forms.ToolStrip tspToolMenu;
        private System.Windows.Forms.TextBox txtSource;
        private System.Windows.Forms.WebBrowser wbrContent1;
    }
}
