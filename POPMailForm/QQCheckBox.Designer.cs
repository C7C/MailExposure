namespace MailExposure.POPMailForm
{
    partial class QQCheckBox
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBoxCheck = new System.Windows.Forms.PictureBox();
            this.textBoxCheck = new System.Windows.Forms.TextBox();
            this.buttonSetCheck = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCheck)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.buttonSetCheck);
            this.panel1.Controls.Add(this.textBoxCheck);
            this.panel1.Controls.Add(this.pictureBoxCheck);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(284, 120);
            this.panel1.TabIndex = 0;
            // 
            // pictureBoxCheck
            // 
            this.pictureBoxCheck.Location = new System.Drawing.Point(13, 12);
            this.pictureBoxCheck.Name = "pictureBoxCheck";
            this.pictureBoxCheck.Size = new System.Drawing.Size(139, 56);
            this.pictureBoxCheck.TabIndex = 0;
            this.pictureBoxCheck.TabStop = false;
            // 
            // textBoxCheck
            // 
            this.textBoxCheck.Location = new System.Drawing.Point(76, 86);
            this.textBoxCheck.Name = "textBoxCheck";
            this.textBoxCheck.Size = new System.Drawing.Size(125, 21);
            this.textBoxCheck.TabIndex = 1;
            // 
            // buttonSetCheck
            // 
            this.buttonSetCheck.Location = new System.Drawing.Point(206, 85);
            this.buttonSetCheck.Name = "buttonSetCheck";
            this.buttonSetCheck.Size = new System.Drawing.Size(75, 23);
            this.buttonSetCheck.TabIndex = 2;
            this.buttonSetCheck.Text = "确定";
            this.buttonSetCheck.UseVisualStyleBackColor = true;
            this.buttonSetCheck.Click += new System.EventHandler(this.buttonSetCheck_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(2, 91);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "输入验证码:";
            // 
            // QQCheckBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 120);
            this.Controls.Add(this.panel1);
            this.Name = "QQCheckBox";
            this.Text = "验证码";
            this.Load += new System.EventHandler(this.QQCheckBox_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCheck)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonSetCheck;
        private System.Windows.Forms.TextBox textBoxCheck;
        private System.Windows.Forms.PictureBox pictureBoxCheck;
    }
}