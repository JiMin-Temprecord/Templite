namespace UserControls
{
    partial class AddEmailUserControl
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
            this.LoggerIdLabel = new System.Windows.Forms.Label();
            this.EmailLabel = new System.Windows.Forms.Label();
            this.loggerIdTextbox = new System.Windows.Forms.TextBox();
            this.emailTextbox = new System.Windows.Forms.TextBox();
            this.AddEmailButton = new System.Windows.Forms.Button();
            this.addEmailPanel = new System.Windows.Forms.Panel();
            this.promptMessage = new System.Windows.Forms.Label();
            this.addEmailPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // LoggerIdLabel
            // 
            this.LoggerIdLabel.AutoSize = true;
            this.LoggerIdLabel.Font = new System.Drawing.Font("Myriad Pro", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LoggerIdLabel.Location = new System.Drawing.Point(136, 49);
            this.LoggerIdLabel.Name = "LoggerIdLabel";
            this.LoggerIdLabel.Size = new System.Drawing.Size(87, 20);
            this.LoggerIdLabel.TabIndex = 0;
            this.LoggerIdLabel.Text = "Logger ID : ";
            // 
            // EmailLabel
            // 
            this.EmailLabel.AutoSize = true;
            this.EmailLabel.Font = new System.Drawing.Font("Myriad Pro", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EmailLabel.Location = new System.Drawing.Point(165, 75);
            this.EmailLabel.Name = "EmailLabel";
            this.EmailLabel.Size = new System.Drawing.Size(58, 20);
            this.EmailLabel.TabIndex = 1;
            this.EmailLabel.Text = "Email : ";
            // 
            // loggerIdTextbox
            // 
            this.loggerIdTextbox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.loggerIdTextbox.Location = new System.Drawing.Point(229, 49);
            this.loggerIdTextbox.Multiline = true;
            this.loggerIdTextbox.Name = "loggerIdTextbox";
            this.loggerIdTextbox.Size = new System.Drawing.Size(242, 20);
            this.loggerIdTextbox.TabIndex = 2;
            // 
            // emailTextbox
            // 
            this.emailTextbox.Location = new System.Drawing.Point(229, 75);
            this.emailTextbox.Multiline = true;
            this.emailTextbox.Name = "emailTextbox";
            this.emailTextbox.Size = new System.Drawing.Size(242, 20);
            this.emailTextbox.TabIndex = 3;
            // 
            // AddEmailButton
            // 
            this.AddEmailButton.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.AddEmailButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AddEmailButton.Font = new System.Drawing.Font("Myriad Pro", 9.749999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AddEmailButton.Location = new System.Drawing.Point(477, 49);
            this.AddEmailButton.Name = "AddEmailButton";
            this.AddEmailButton.Size = new System.Drawing.Size(70, 45);
            this.AddEmailButton.TabIndex = 4;
            this.AddEmailButton.Text = "ADD";
            this.AddEmailButton.UseVisualStyleBackColor = true;
            // 
            // addEmailPanel
            // 
            this.addEmailPanel.Controls.Add(this.promptMessage);
            this.addEmailPanel.Controls.Add(this.AddEmailButton);
            this.addEmailPanel.Controls.Add(this.LoggerIdLabel);
            this.addEmailPanel.Controls.Add(this.emailTextbox);
            this.addEmailPanel.Controls.Add(this.EmailLabel);
            this.addEmailPanel.Controls.Add(this.loggerIdTextbox);
            this.addEmailPanel.Location = new System.Drawing.Point(0, 316);
            this.addEmailPanel.Name = "addEmailPanel";
            this.addEmailPanel.Size = new System.Drawing.Size(710, 140);
            this.addEmailPanel.TabIndex = 5;
            // 
            // promptMessage
            // 
            this.promptMessage.AutoSize = true;
            this.promptMessage.ForeColor = System.Drawing.Color.Red;
            this.promptMessage.Location = new System.Drawing.Point(229, 30);
            this.promptMessage.Name = "promptMessage";
            this.promptMessage.Size = new System.Drawing.Size(0, 13);
            this.promptMessage.TabIndex = 5;
            // 
            // AddEmailUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.addEmailPanel);
            this.Name = "AddEmailUserControl";
            this.Size = new System.Drawing.Size(710, 760);
            this.addEmailPanel.ResumeLayout(false);
            this.addEmailPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label LoggerIdLabel;
        private System.Windows.Forms.Label EmailLabel;
        public System.Windows.Forms.TextBox loggerIdTextbox;
        public System.Windows.Forms.TextBox emailTextbox;
        public System.Windows.Forms.Button AddEmailButton;
        public System.Windows.Forms.Label promptMessage;
        public System.Windows.Forms.Panel addEmailPanel;
    }
}
