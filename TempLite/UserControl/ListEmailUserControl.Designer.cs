namespace UserControls
{
    partial class ListEmailUserControl
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
            this.emailDeleteButton = new System.Windows.Forms.Button();
            this.emailLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // emailDeleteButton
            // 
            this.emailDeleteButton.BackColor = System.Drawing.Color.OrangeRed;
            this.emailDeleteButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.emailDeleteButton.FlatAppearance.BorderSize = 0;
            this.emailDeleteButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.emailDeleteButton.Font = new System.Drawing.Font("Myriad Pro", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.emailDeleteButton.ForeColor = System.Drawing.Color.White;
            this.emailDeleteButton.Location = new System.Drawing.Point(685, 0);
            this.emailDeleteButton.Name = "emailDeleteButton";
            this.emailDeleteButton.Size = new System.Drawing.Size(25, 50);
            this.emailDeleteButton.TabIndex = 0;
            this.emailDeleteButton.Text = "-";
            this.emailDeleteButton.UseVisualStyleBackColor = false;
            this.emailDeleteButton.Visible = false;
            this.emailDeleteButton.Click += new System.EventHandler(this.emailDeleteButton_Click);
            // 
            // emailLabel
            // 
            this.emailLabel.AutoSize = true;
            this.emailLabel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.emailLabel.Font = new System.Drawing.Font("Myriad Pro", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.emailLabel.ForeColor = System.Drawing.Color.Black;
            this.emailLabel.Location = new System.Drawing.Point(13, 17);
            this.emailLabel.Name = "emailLabel";
            this.emailLabel.Size = new System.Drawing.Size(0, 18);
            this.emailLabel.TabIndex = 1;
            this.emailLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ListEmailUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.emailLabel);
            this.Controls.Add(this.emailDeleteButton);
            this.Name = "ListEmailUserControl";
            this.Size = new System.Drawing.Size(710, 50);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Button emailDeleteButton;
        public System.Windows.Forms.Label emailLabel;
    }
}
