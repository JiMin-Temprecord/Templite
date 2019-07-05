namespace UserControls
{
    partial class EmailListUserControl
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
            this.EmailListHeaderPanel = new System.Windows.Forms.Panel();
            this.EmailListLabel = new System.Windows.Forms.Label();
            this.emailListPanel = new System.Windows.Forms.Panel();
            this.EmailListHeaderPanel.SuspendLayout();
            this.emailListPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // EmailListHeaderPanel
            // 
            this.EmailListHeaderPanel.BackColor = System.Drawing.Color.White;
            this.EmailListHeaderPanel.Controls.Add(this.EmailListLabel);
            this.EmailListHeaderPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.EmailListHeaderPanel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(33)))), ((int)(((byte)(99)))));
            this.EmailListHeaderPanel.Location = new System.Drawing.Point(0, 0);
            this.EmailListHeaderPanel.Name = "EmailListHeaderPanel";
            this.EmailListHeaderPanel.Size = new System.Drawing.Size(710, 44);
            this.EmailListHeaderPanel.TabIndex = 4;
            // 
            // EmailListLabel
            // 
            this.EmailListLabel.AutoSize = true;
            this.EmailListLabel.Font = new System.Drawing.Font("Myriad Pro", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EmailListLabel.Location = new System.Drawing.Point(10, 10);
            this.EmailListLabel.Name = "EmailListLabel";
            this.EmailListLabel.Size = new System.Drawing.Size(60, 24);
            this.EmailListLabel.TabIndex = 0;
            this.EmailListLabel.Text = "Email ";
            // 
            // emailListPanel
            // 
            this.emailListPanel.BackColor = System.Drawing.Color.Transparent;
            this.emailListPanel.Controls.Add(this.EmailListHeaderPanel);
            this.emailListPanel.Location = new System.Drawing.Point(0, 0);
            this.emailListPanel.Name = "emailListPanel";
            this.emailListPanel.Size = new System.Drawing.Size(710, 760);
            this.emailListPanel.TabIndex = 6;
            // 
            // EmailListUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.emailListPanel);
            this.Name = "EmailListUserControl";
            this.Size = new System.Drawing.Size(710, 760);
            this.VisibleChanged += new System.EventHandler(this.EmailListUserControl_VisibleChanged);
            this.EmailListHeaderPanel.ResumeLayout(false);
            this.EmailListHeaderPanel.PerformLayout();
            this.emailListPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel EmailListHeaderPanel;
        private System.Windows.Forms.Label EmailListLabel;
        public System.Windows.Forms.Panel emailListPanel;
    }
}
