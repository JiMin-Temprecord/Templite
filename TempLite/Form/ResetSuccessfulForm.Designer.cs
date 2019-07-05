namespace TempLite
{
    partial class ResetSuccessfulForm
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
            this.emailCancelButton = new System.Windows.Forms.Button();
            this.keycodeLabel = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // emailCancelButton
            // 
            this.emailCancelButton.BackColor = System.Drawing.Color.Tomato;
            this.emailCancelButton.FlatAppearance.BorderSize = 0;
            this.emailCancelButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.emailCancelButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.emailCancelButton.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.emailCancelButton.Location = new System.Drawing.Point(215, 9);
            this.emailCancelButton.Name = "emailCancelButton";
            this.emailCancelButton.Size = new System.Drawing.Size(34, 28);
            this.emailCancelButton.TabIndex = 13;
            this.emailCancelButton.Text = "X";
            this.emailCancelButton.UseVisualStyleBackColor = false;
            this.emailCancelButton.Click += new System.EventHandler(this.emailCancelButton_Click);
            // 
            // keycodeLabel
            // 
            this.keycodeLabel.AllowDrop = true;
            this.keycodeLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.keycodeLabel.AutoSize = true;
            this.keycodeLabel.BackColor = System.Drawing.Color.Transparent;
            this.keycodeLabel.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.keycodeLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(33)))), ((int)(((byte)(99)))));
            this.keycodeLabel.Location = new System.Drawing.Point(23, 74);
            this.keycodeLabel.Name = "keycodeLabel";
            this.keycodeLabel.Size = new System.Drawing.Size(207, 26);
            this.keycodeLabel.TabIndex = 12;
            this.keycodeLabel.Text = "Email reset succesfully";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.emailCancelButton);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.ForeColor = System.Drawing.SystemColors.MenuText;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(250, 175);
            this.panel1.TabIndex = 14;
            // 
            // ResetSuccessfulForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(250, 175);
            this.Controls.Add(this.keycodeLabel);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ResetSuccessfulForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ResetSuccessfulForm";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Button emailCancelButton;
        public System.Windows.Forms.Label keycodeLabel;
        private System.Windows.Forms.Panel panel1;
    }
}