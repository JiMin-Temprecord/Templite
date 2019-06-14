﻿namespace UserControls
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
            this.emailCancelButton = new System.Windows.Forms.Button();
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
            this.successLabel.Font = new System.Drawing.Font("Myriad Pro Light", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.successLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.successLabel.Location = new System.Drawing.Point(34, 23);
            this.successLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.successLabel.Name = "successLabel";
            this.successLabel.Size = new System.Drawing.Size(257, 38);
            this.successLabel.TabIndex = 9;
            this.successLabel.Text = "Sent to ";
            this.successLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // emailCancelButton
            // 
            this.emailCancelButton.BackColor = System.Drawing.Color.Crimson;
            this.emailCancelButton.FlatAppearance.BorderSize = 0;
            this.emailCancelButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.emailCancelButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.emailCancelButton.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.emailCancelButton.Location = new System.Drawing.Point(298, 10);
            this.emailCancelButton.Name = "emailCancelButton";
            this.emailCancelButton.Size = new System.Drawing.Size(27, 28);
            this.emailCancelButton.TabIndex = 10;
            this.emailCancelButton.Text = "X";
            this.emailCancelButton.UseVisualStyleBackColor = false;
            // 
            // EmailDoneUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.emailCancelButton);
            this.Controls.Add(this.successLabel);
            this.Controls.Add(this.emailDoneImage);
            this.Name = "EmailDoneUserControl";
            this.Size = new System.Drawing.Size(325, 250);
            ((System.ComponentModel.ISupportInitialize)(this.emailDoneImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox emailDoneImage;
        public System.Windows.Forms.Label successLabel;
        public System.Windows.Forms.Button emailCancelButton;
    }
}
