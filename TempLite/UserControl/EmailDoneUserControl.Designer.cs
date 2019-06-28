namespace UserControls
{
    partial class EmailDoneUserControl
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
            this.emailDoneImage = new System.Windows.Forms.PictureBox();
            this.emailCancelButton = new System.Windows.Forms.Button();
            this.successTextbox = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.emailDoneImage)).BeginInit();
            this.SuspendLayout();
            // 
            // emailDoneImage
            // 
            this.emailDoneImage.BackColor = System.Drawing.Color.White;
            this.emailDoneImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.emailDoneImage.Image = global::TempLite.Properties.Resources.emailSuccess;
            this.emailDoneImage.Location = new System.Drawing.Point(0, 0);
            this.emailDoneImage.Name = "emailDoneImage";
            this.emailDoneImage.Size = new System.Drawing.Size(425, 350);
            this.emailDoneImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.emailDoneImage.TabIndex = 0;
            this.emailDoneImage.TabStop = false;
            // 
            // emailCancelButton
            // 
            this.emailCancelButton.BackColor = System.Drawing.Color.Crimson;
            this.emailCancelButton.FlatAppearance.BorderSize = 0;
            this.emailCancelButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.emailCancelButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.emailCancelButton.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.emailCancelButton.Location = new System.Drawing.Point(398, 14);
            this.emailCancelButton.Name = "emailCancelButton";
            this.emailCancelButton.Size = new System.Drawing.Size(27, 28);
            this.emailCancelButton.TabIndex = 10;
            this.emailCancelButton.Text = "X";
            this.emailCancelButton.UseVisualStyleBackColor = false;
            this.emailCancelButton.Click += new System.EventHandler(this.emailCancelButton_Click);
            // 
            // successTextbox
            // 
            this.successTextbox.BackColor = System.Drawing.Color.White;
            this.successTextbox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.successTextbox.Font = new System.Drawing.Font("Myriad Pro Light", 12F, System.Drawing.FontStyle.Bold);
            this.successTextbox.ForeColor = System.Drawing.Color.Black;
            this.successTextbox.Location = new System.Drawing.Point(109, 261);
            this.successTextbox.Multiline = true;
            this.successTextbox.Name = "successTextbox";
            this.successTextbox.ReadOnly = true;
            this.successTextbox.Size = new System.Drawing.Size(208, 73);
            this.successTextbox.TabIndex = 11;
            this.successTextbox.Text = "Sent ";
            this.successTextbox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // EmailDoneUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.successTextbox);
            this.Controls.Add(this.emailCancelButton);
            this.Controls.Add(this.emailDoneImage);
            this.Name = "EmailDoneUserControl";
            this.Size = new System.Drawing.Size(425, 350);
            ((System.ComponentModel.ISupportInitialize)(this.emailDoneImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox emailDoneImage;
        public System.Windows.Forms.Button emailCancelButton;
        public System.Windows.Forms.TextBox successTextbox;
    }
}
