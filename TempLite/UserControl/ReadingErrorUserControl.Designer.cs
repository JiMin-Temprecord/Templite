namespace UserControls
{
    partial class ReadingErrorUserControl
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
            this.warningLabel = new System.Windows.Forms.Label();
            this.progressBarText = new System.Windows.Forms.TextBox();
            this.warningImage = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.warningImage)).BeginInit();
            this.SuspendLayout();
            // 
            // warningLabel
            // 
            this.warningLabel.BackColor = System.Drawing.Color.White;
            this.warningLabel.Font = new System.Drawing.Font("Myriad Pro", 21.75F, System.Drawing.FontStyle.Bold);
            this.warningLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.warningLabel.Location = new System.Drawing.Point(45, 253);
            this.warningLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.warningLabel.Name = "warningLabel";
            this.warningLabel.Size = new System.Drawing.Size(258, 36);
            this.warningLabel.TabIndex = 10;
            this.warningLabel.Text = "Error";
            // 
            // progressBarText
            // 
            this.progressBarText.BackColor = System.Drawing.Color.White;
            this.progressBarText.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.progressBarText.Font = new System.Drawing.Font("Myriad Pro", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.progressBarText.Location = new System.Drawing.Point(51, 292);
            this.progressBarText.Multiline = true;
            this.progressBarText.Name = "progressBarText";
            this.progressBarText.ReadOnly = true;
            this.progressBarText.Size = new System.Drawing.Size(252, 72);
            this.progressBarText.TabIndex = 11;
            this.progressBarText.Text = "Please make sure that the logger is placed fully inside the reader and check that" +
    " the battery isnt flat";
            // 
            // warningImage
            // 
            this.warningImage.Image = global::TempLite.Properties.Resources.errorWarning;
            this.warningImage.Location = new System.Drawing.Point(51, 3);
            this.warningImage.Name = "warningImage";
            this.warningImage.Size = new System.Drawing.Size(250, 250);
            this.warningImage.TabIndex = 12;
            this.warningImage.TabStop = false;
            // 
            // ReadingError
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.warningImage);
            this.Controls.Add(this.progressBarText);
            this.Controls.Add(this.warningLabel);
            this.Name = "ReadingError";
            this.Size = new System.Drawing.Size(350, 370);
            ((System.ComponentModel.ISupportInitialize)(this.warningImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label warningLabel;
        private System.Windows.Forms.TextBox progressBarText;
        private System.Windows.Forms.PictureBox warningImage;
    }
}
