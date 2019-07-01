namespace UserControls
{
    partial class SendingEmailUserControl
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
            this.emailProgressBar = new System.Windows.Forms.ProgressBar();
            this.EmailImage = new System.Windows.Forms.PictureBox();
            this.emailLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.EmailImage)).BeginInit();
            this.SuspendLayout();
            // 
            // emailProgressBar
            // 
            this.emailProgressBar.Location = new System.Drawing.Point(122, 255);
            this.emailProgressBar.MarqueeAnimationSpeed = 16;
            this.emailProgressBar.Name = "emailProgressBar";
            this.emailProgressBar.Size = new System.Drawing.Size(183, 18);
            this.emailProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.emailProgressBar.TabIndex = 6;
            // 
            // EmailImage
            // 
            this.EmailImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EmailImage.Image = global::TempLite.Properties.Resources.email;
            this.EmailImage.Location = new System.Drawing.Point(0, 0);
            this.EmailImage.Name = "EmailImage";
            this.EmailImage.Size = new System.Drawing.Size(425, 350);
            this.EmailImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.EmailImage.TabIndex = 0;
            this.EmailImage.TabStop = false;
            // 
            // emailLabel
            // 
            this.emailLabel.BackColor = System.Drawing.Color.Transparent;
            this.emailLabel.Font = new System.Drawing.Font("Myriad Pro Light", 18F, System.Drawing.FontStyle.Bold);
            this.emailLabel.ForeColor = System.Drawing.Color.Black;
            this.emailLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.emailLabel.Location = new System.Drawing.Point(122, 80);
            this.emailLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.emailLabel.Name = "emailLabel";
            this.emailLabel.Size = new System.Drawing.Size(183, 29);
            this.emailLabel.TabIndex = 8;
            this.emailLabel.Text = "Sending Email";
            this.emailLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // EmailUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.emailLabel);
            this.Controls.Add(this.emailProgressBar);
            this.Controls.Add(this.EmailImage);
            this.Name = "EmailUserControl";
            this.Size = new System.Drawing.Size(425, 350);
            ((System.ComponentModel.ISupportInitialize)(this.EmailImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ProgressBar emailProgressBar;
        private System.Windows.Forms.PictureBox EmailImage;
        private System.Windows.Forms.Label emailLabel;
    }
}
