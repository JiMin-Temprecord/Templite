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
            this.loggerIDTextbox = new System.Windows.Forms.TextBox();
            this.emailTextbox = new System.Windows.Forms.TextBox();
            this.AddEmailButton = new System.Windows.Forms.Button();
            this.addEmailPanel = new System.Windows.Forms.Panel();
            this.confirmEmailTextbox = new System.Windows.Forms.TextBox();
            this.addEmailLabel = new System.Windows.Forms.Label();
            this.promptMessage = new System.Windows.Forms.Label();
            this.EmailListHeaderPanel = new System.Windows.Forms.Panel();
            this.EmailListLabel = new System.Windows.Forms.Label();
            this.emailListPanel = new System.Windows.Forms.Panel();
            this.addEmailPanel.SuspendLayout();
            this.EmailListHeaderPanel.SuspendLayout();
            this.emailListPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // loggerIDTextbox
            // 
            this.loggerIDTextbox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.loggerIDTextbox.BackColor = System.Drawing.Color.WhiteSmoke;
            this.loggerIDTextbox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.loggerIDTextbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.loggerIDTextbox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(33)))), ((int)(((byte)(99)))));
            this.loggerIDTextbox.Location = new System.Drawing.Point(182, 291);
            this.loggerIDTextbox.Margin = new System.Windows.Forms.Padding(0);
            this.loggerIDTextbox.MaximumSize = new System.Drawing.Size(350, 40);
            this.loggerIDTextbox.MinimumSize = new System.Drawing.Size(350, 40);
            this.loggerIDTextbox.Name = "loggerIDTextbox";
            this.loggerIDTextbox.Size = new System.Drawing.Size(350, 26);
            this.loggerIDTextbox.TabIndex = 2;
            this.loggerIDTextbox.Text = "Logger ID";
            this.loggerIDTextbox.WordWrap = false;
            // 
            // emailTextbox
            // 
            this.emailTextbox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.emailTextbox.BackColor = System.Drawing.Color.WhiteSmoke;
            this.emailTextbox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.emailTextbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.emailTextbox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(33)))), ((int)(((byte)(99)))));
            this.emailTextbox.Location = new System.Drawing.Point(182, 348);
            this.emailTextbox.Margin = new System.Windows.Forms.Padding(5);
            this.emailTextbox.MaximumSize = new System.Drawing.Size(350, 40);
            this.emailTextbox.MinimumSize = new System.Drawing.Size(350, 40);
            this.emailTextbox.Name = "emailTextbox";
            this.emailTextbox.Size = new System.Drawing.Size(350, 26);
            this.emailTextbox.TabIndex = 3;
            this.emailTextbox.Text = "Email";
            // 
            // AddEmailButton
            // 
            this.AddEmailButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.AddEmailButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(33)))), ((int)(((byte)(99)))));
            this.AddEmailButton.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.AddEmailButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AddEmailButton.Font = new System.Drawing.Font("Myriad Pro", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AddEmailButton.ForeColor = System.Drawing.Color.White;
            this.AddEmailButton.Location = new System.Drawing.Point(182, 465);
            this.AddEmailButton.Margin = new System.Windows.Forms.Padding(0);
            this.AddEmailButton.MinimumSize = new System.Drawing.Size(350, 40);
            this.AddEmailButton.Name = "AddEmailButton";
            this.AddEmailButton.Size = new System.Drawing.Size(350, 40);
            this.AddEmailButton.TabIndex = 5;
            this.AddEmailButton.Text = "Add Email";
            this.AddEmailButton.UseVisualStyleBackColor = false;
            this.AddEmailButton.Click += new System.EventHandler(this.AddEmailButton_Click);
            // 
            // addEmailPanel
            // 
            this.addEmailPanel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.addEmailPanel.BackColor = System.Drawing.Color.White;
            this.addEmailPanel.Controls.Add(this.confirmEmailTextbox);
            this.addEmailPanel.Controls.Add(this.addEmailLabel);
            this.addEmailPanel.Controls.Add(this.promptMessage);
            this.addEmailPanel.Controls.Add(this.AddEmailButton);
            this.addEmailPanel.Controls.Add(this.emailTextbox);
            this.addEmailPanel.Controls.Add(this.loggerIDTextbox);
            this.addEmailPanel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.addEmailPanel.Location = new System.Drawing.Point(0, 0);
            this.addEmailPanel.Name = "addEmailPanel";
            this.addEmailPanel.Size = new System.Drawing.Size(710, 760);
            this.addEmailPanel.TabIndex = 5;
            this.addEmailPanel.Visible = false;
            this.addEmailPanel.VisibleChanged += new System.EventHandler(this.addEmailPanel_VisibleChanged);
            // 
            // confirmEmailTextbox
            // 
            this.confirmEmailTextbox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.confirmEmailTextbox.BackColor = System.Drawing.Color.WhiteSmoke;
            this.confirmEmailTextbox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.confirmEmailTextbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.confirmEmailTextbox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(33)))), ((int)(((byte)(99)))));
            this.confirmEmailTextbox.Location = new System.Drawing.Point(182, 407);
            this.confirmEmailTextbox.Margin = new System.Windows.Forms.Padding(5, 5, 5, 0);
            this.confirmEmailTextbox.MaximumSize = new System.Drawing.Size(350, 40);
            this.confirmEmailTextbox.MinimumSize = new System.Drawing.Size(350, 40);
            this.confirmEmailTextbox.Name = "confirmEmailTextbox";
            this.confirmEmailTextbox.Size = new System.Drawing.Size(350, 26);
            this.confirmEmailTextbox.TabIndex = 4;
            this.confirmEmailTextbox.Text = "Confirm Email";
            // 
            // addEmailLabel
            // 
            this.addEmailLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.addEmailLabel.AutoSize = true;
            this.addEmailLabel.Font = new System.Drawing.Font("Calibri", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.addEmailLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(33)))), ((int)(((byte)(99)))));
            this.addEmailLabel.Location = new System.Drawing.Point(238, 220);
            this.addEmailLabel.Name = "addEmailLabel";
            this.addEmailLabel.Size = new System.Drawing.Size(244, 36);
            this.addEmailLabel.TabIndex = 7;
            this.addEmailLabel.Text = "Add Email Address";
            // 
            // promptMessage
            // 
            this.promptMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.promptMessage.AutoSize = true;
            this.promptMessage.BackColor = System.Drawing.Color.Transparent;
            this.promptMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.promptMessage.ForeColor = System.Drawing.Color.Red;
            this.promptMessage.Location = new System.Drawing.Point(184, 267);
            this.promptMessage.Name = "promptMessage";
            this.promptMessage.Size = new System.Drawing.Size(0, 16);
            this.promptMessage.TabIndex = 6;
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
            this.emailListPanel.Controls.Add(this.addEmailPanel);
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
            this.addEmailPanel.ResumeLayout(false);
            this.addEmailPanel.PerformLayout();
            this.EmailListHeaderPanel.ResumeLayout(false);
            this.EmailListHeaderPanel.PerformLayout();
            this.emailListPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        public System.Windows.Forms.TextBox loggerIDTextbox;
        public System.Windows.Forms.TextBox emailTextbox;
        public System.Windows.Forms.Button AddEmailButton;
        public System.Windows.Forms.Label promptMessage;
        public System.Windows.Forms.Panel addEmailPanel;
        public System.Windows.Forms.TextBox confirmEmailTextbox;
        private System.Windows.Forms.Panel EmailListHeaderPanel;
        private System.Windows.Forms.Label EmailListLabel;
        public System.Windows.Forms.Panel emailListPanel;
        private System.Windows.Forms.Label addEmailLabel;
    }
}
