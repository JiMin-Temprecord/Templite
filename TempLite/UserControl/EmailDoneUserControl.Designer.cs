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
            this.successLabel = new System.Windows.Forms.Label();
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
            this.emailDoneImage.Size = new System.Drawing.Size(325, 250);
            this.emailDoneImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.emailDoneImage.TabIndex = 0;
            this.emailDoneImage.TabStop = false;
            // 
            // successLabel
            // 
            this.successLabel.BackColor = System.Drawing.Color.White;
            this.successLabel.Font = new System.Drawing.Font("Myriad Pro", 21.75F, System.Drawing.FontStyle.Bold);
            this.successLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.successLabel.Location = new System.Drawing.Point(107, 203);
            this.successLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.successLabel.Name = "successLabel";
            this.successLabel.Size = new System.Drawing.Size(115, 36);
            this.successLabel.TabIndex = 9;
            this.successLabel.Text = "Success";
            // 
            // EmailDoneUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.successLabel);
            this.Controls.Add(this.emailDoneImage);
            this.Name = "EmailDoneUserControl";
            this.Size = new System.Drawing.Size(325, 250);
            ((System.ComponentModel.ISupportInitialize)(this.emailDoneImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox emailDoneImage;
        private System.Windows.Forms.Label successLabel;
    }
}
