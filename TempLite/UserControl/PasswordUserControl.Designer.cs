namespace UserControls
{
    partial class PasswordUserControl
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
            this.keycodeLabel = new System.Windows.Forms.Label();
            this.keycodeTextbox = new System.Windows.Forms.TextBox();
            this.promptMessage = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // keycodeLabel
            // 
            this.keycodeLabel.AllowDrop = true;
            this.keycodeLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.keycodeLabel.AutoSize = true;
            this.keycodeLabel.BackColor = System.Drawing.Color.Transparent;
            this.keycodeLabel.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.keycodeLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.keycodeLabel.Location = new System.Drawing.Point(17, 63);
            this.keycodeLabel.Name = "keycodeLabel";
            this.keycodeLabel.Size = new System.Drawing.Size(139, 26);
            this.keycodeLabel.TabIndex = 3;
            this.keycodeLabel.Text = "Enter KeyCode";
            // 
            // keycodeTextbox
            // 
            this.keycodeTextbox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.keycodeTextbox.BackColor = System.Drawing.Color.WhiteSmoke;
            this.keycodeTextbox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.keycodeTextbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.keycodeTextbox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(33)))), ((int)(((byte)(99)))));
            this.keycodeTextbox.Location = new System.Drawing.Point(20, 108);
            this.keycodeTextbox.Margin = new System.Windows.Forms.Padding(5);
            this.keycodeTextbox.Name = "keycodeTextbox";
            this.keycodeTextbox.Size = new System.Drawing.Size(191, 26);
            this.keycodeTextbox.TabIndex = 4;
            this.keycodeTextbox.UseSystemPasswordChar = true;
            this.keycodeTextbox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.keycodeTextbox_KeyUp);
            // 
            // promptMessage
            // 
            this.promptMessage.AutoSize = true;
            this.promptMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.promptMessage.ForeColor = System.Drawing.Color.Tomato;
            this.promptMessage.Location = new System.Drawing.Point(18, 88);
            this.promptMessage.Name = "promptMessage";
            this.promptMessage.Size = new System.Drawing.Size(0, 13);
            this.promptMessage.TabIndex = 5;
            // 
            // PasswordUserControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(33)))), ((int)(((byte)(99)))));
            this.Controls.Add(this.promptMessage);
            this.Controls.Add(this.keycodeTextbox);
            this.Controls.Add(this.keycodeLabel);
            this.Name = "PasswordUserControl";
            this.Size = new System.Drawing.Size(323, 187);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        public System.Windows.Forms.Label promptMessage;
        public System.Windows.Forms.TextBox keycodeTextbox;
        public System.Windows.Forms.Label keycodeLabel;
    }
}
