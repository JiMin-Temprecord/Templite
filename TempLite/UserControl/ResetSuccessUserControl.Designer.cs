namespace UserControls
{
    partial class ResetSuccessUserControl
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
            this.emailCancelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // keycodeLabel
            // 
            this.keycodeLabel.AllowDrop = true;
            this.keycodeLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.keycodeLabel.AutoSize = true;
            this.keycodeLabel.BackColor = System.Drawing.Color.Transparent;
            this.keycodeLabel.Font = new System.Drawing.Font("Myriad Pro Cond", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.keycodeLabel.ForeColor = System.Drawing.Color.White;
            this.keycodeLabel.Location = new System.Drawing.Point(65, 64);
            this.keycodeLabel.Name = "keycodeLabel";
            this.keycodeLabel.Size = new System.Drawing.Size(176, 25);
            this.keycodeLabel.TabIndex = 3;
            this.keycodeLabel.Text = "Email succesfully reset";
            // 
            // emailCancelButton
            // 
            this.emailCancelButton.BackColor = System.Drawing.Color.Crimson;
            this.emailCancelButton.FlatAppearance.BorderSize = 0;
            this.emailCancelButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.emailCancelButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.emailCancelButton.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.emailCancelButton.Location = new System.Drawing.Point(273, 12);
            this.emailCancelButton.Name = "emailCancelButton";
            this.emailCancelButton.Size = new System.Drawing.Size(27, 28);
            this.emailCancelButton.TabIndex = 11;
            this.emailCancelButton.Text = "X";
            this.emailCancelButton.UseVisualStyleBackColor = false;
            this.emailCancelButton.Click += new System.EventHandler(this.emailCancelButton_Click);
            // 
            // ResetSuccessUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(33)))), ((int)(((byte)(99)))));
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.emailCancelButton);
            this.Controls.Add(this.keycodeLabel);
            this.ForeColor = System.Drawing.Color.White;
            this.Name = "ResetSuccessUserControl";
            this.Size = new System.Drawing.Size(298, 148);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        public System.Windows.Forms.Label keycodeLabel;
        public System.Windows.Forms.Button emailCancelButton;
    }
}
