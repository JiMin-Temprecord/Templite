namespace TempLite
{
    partial class KeycodeInputForm
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
            this.bgKeycodeInput = new System.Windows.Forms.Panel();
            this.promptMessage = new System.Windows.Forms.Label();
            this.keycodeTextbox = new System.Windows.Forms.TextBox();
            this.keycodeLabel = new System.Windows.Forms.Label();
            this.bgKeycodeInput.SuspendLayout();
            this.SuspendLayout();
            // 
            // bgKeycodeInput
            // 
            this.bgKeycodeInput.BackColor = System.Drawing.Color.White;
            this.bgKeycodeInput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.bgKeycodeInput.Controls.Add(this.promptMessage);
            this.bgKeycodeInput.Controls.Add(this.keycodeTextbox);
            this.bgKeycodeInput.Controls.Add(this.keycodeLabel);
            this.bgKeycodeInput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bgKeycodeInput.Location = new System.Drawing.Point(0, 0);
            this.bgKeycodeInput.Name = "bgKeycodeInput";
            this.bgKeycodeInput.Size = new System.Drawing.Size(250, 175);
            this.bgKeycodeInput.TabIndex = 0;
            // 
            // promptMessage
            // 
            this.promptMessage.AutoSize = true;
            this.promptMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.promptMessage.ForeColor = System.Drawing.Color.Tomato;
            this.promptMessage.Location = new System.Drawing.Point(16, 80);
            this.promptMessage.Name = "promptMessage";
            this.promptMessage.Size = new System.Drawing.Size(0, 13);
            this.promptMessage.TabIndex = 7;
            // 
            // keycodeTextbox
            // 
            this.keycodeTextbox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.keycodeTextbox.BackColor = System.Drawing.Color.WhiteSmoke;
            this.keycodeTextbox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.keycodeTextbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.keycodeTextbox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(33)))), ((int)(((byte)(99)))));
            this.keycodeTextbox.Location = new System.Drawing.Point(16, 98);
            this.keycodeTextbox.Margin = new System.Windows.Forms.Padding(5);
            this.keycodeTextbox.Name = "keycodeTextbox";
            this.keycodeTextbox.Size = new System.Drawing.Size(211, 26);
            this.keycodeTextbox.TabIndex = 6;
            this.keycodeTextbox.UseSystemPasswordChar = true;
            this.keycodeTextbox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.keycodeTextbox_KeyUp);
            // 
            // keycodeLabel
            // 
            this.keycodeLabel.AllowDrop = true;
            this.keycodeLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.keycodeLabel.AutoSize = true;
            this.keycodeLabel.BackColor = System.Drawing.Color.Transparent;
            this.keycodeLabel.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.keycodeLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(33)))), ((int)(((byte)(99)))));
            this.keycodeLabel.Location = new System.Drawing.Point(15, 52);
            this.keycodeLabel.Name = "keycodeLabel";
            this.keycodeLabel.Size = new System.Drawing.Size(139, 26);
            this.keycodeLabel.TabIndex = 5;
            this.keycodeLabel.Text = "Enter KeyCode";
            // 
            // KeycodeInputForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(250, 175);
            this.Controls.Add(this.bgKeycodeInput);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "KeycodeInputForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.KeycodeInputForm_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.KeycodeInputForm_KeyUp);
            this.bgKeycodeInput.ResumeLayout(false);
            this.bgKeycodeInput.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel bgKeycodeInput;
        public System.Windows.Forms.TextBox keycodeTextbox;
        public System.Windows.Forms.Label keycodeLabel;
        public System.Windows.Forms.Label promptMessage;
    }
}