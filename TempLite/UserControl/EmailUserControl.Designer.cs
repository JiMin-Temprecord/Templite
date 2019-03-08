﻿namespace UserControls
{
    partial class EmailUserControl
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.EmailImage = new System.Windows.Forms.PictureBox();
            this.emailProgressBar = new System.Windows.Forms.ProgressBar();
            this.emailLabel = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.EmailImage)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.emailLabel);
            this.panel1.Controls.Add(this.emailProgressBar);
            this.panel1.Controls.Add(this.EmailImage);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(725, 600);
            this.panel1.TabIndex = 0;
            // 
            // EmailImage
            // 
            this.EmailImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EmailImage.Image = global::TempLite.Properties.Resources.email;
            this.EmailImage.Location = new System.Drawing.Point(0, 0);
            this.EmailImage.Name = "EmailImage";
            this.EmailImage.Size = new System.Drawing.Size(725, 600);
            this.EmailImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.EmailImage.TabIndex = 0;
            this.EmailImage.TabStop = false;
            // 
            // emailProgressBar
            // 
            this.emailProgressBar.Location = new System.Drawing.Point(238, 388);
            this.emailProgressBar.MarqueeAnimationSpeed = 16;
            this.emailProgressBar.Name = "emailProgressBar";
            this.emailProgressBar.Size = new System.Drawing.Size(249, 18);
            this.emailProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.emailProgressBar.TabIndex = 6;
            // 
            // emailLabel
            // 
            this.emailLabel.AutoSize = true;
            this.emailLabel.BackColor = System.Drawing.Color.Transparent;
            this.emailLabel.Font = new System.Drawing.Font("Myriad Pro", 21.75F, System.Drawing.FontStyle.Bold);
            this.emailLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.emailLabel.Location = new System.Drawing.Point(262, 185);
            this.emailLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.emailLabel.Name = "emailLabel";
            this.emailLabel.Size = new System.Drawing.Size(199, 36);
            this.emailLabel.TabIndex = 8;
            this.emailLabel.Text = "Sending Email";
            // 
            // EmailUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.panel1);
            this.Name = "EmailUserControl";
            this.Size = new System.Drawing.Size(725, 600);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.EmailImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox EmailImage;
        private System.Windows.Forms.ProgressBar emailProgressBar;
        private System.Windows.Forms.Label emailLabel;
    }
}