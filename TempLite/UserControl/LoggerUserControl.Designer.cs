namespace UserControls
{
    partial class LoggerUserControl
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
            this.loggerText = new System.Windows.Forms.TextBox();
            this.loggerLabel = new System.Windows.Forms.Label();
            this.loggerPanel = new System.Windows.Forms.Panel();
            this.loggerImage = new System.Windows.Forms.PictureBox();
            this.loggerPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.loggerImage)).BeginInit();
            this.SuspendLayout();
            // 
            // loggerText
            // 
            this.loggerText.BackColor = System.Drawing.Color.White;
            this.loggerText.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.loggerText.Font = new System.Drawing.Font("Myriad Pro", 12F);
            this.loggerText.ForeColor = System.Drawing.Color.Black;
            this.loggerText.Location = new System.Drawing.Point(100, 675);
            this.loggerText.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.loggerText.Multiline = true;
            this.loggerText.Name = "loggerText";
            this.loggerText.ReadOnly = true;
            this.loggerText.Size = new System.Drawing.Size(597, 83);
            this.loggerText.TabIndex = 7;
            this.loggerText.Text = "Insert a G4 or MonT logger into the reader. \n Reading should start automatically";
            // 
            // loggerLabel
            // 
            this.loggerLabel.BackColor = System.Drawing.Color.Transparent;
            this.loggerLabel.Font = new System.Drawing.Font("Myriad Pro", 21.75F, System.Drawing.FontStyle.Bold);
            this.loggerLabel.ForeColor = System.Drawing.Color.Black;
            this.loggerLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.loggerLabel.Location = new System.Drawing.Point(100, 625);
            this.loggerLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.loggerLabel.Name = "loggerLabel";
            this.loggerLabel.Size = new System.Drawing.Size(533, 36);
            this.loggerLabel.TabIndex = 6;
            this.loggerLabel.Text = "Insert Logger";
            // 
            // loggerPanel
            // 
            this.loggerPanel.Controls.Add(this.loggerImage);
            this.loggerPanel.Location = new System.Drawing.Point(0, 0);
            this.loggerPanel.Name = "loggerPanel";
            this.loggerPanel.Size = new System.Drawing.Size(725, 600);
            this.loggerPanel.TabIndex = 8;
            // 
            // loggerImage
            // 
            this.loggerImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.loggerImage.Image = global::TempLite.Properties.Resources.logger_01;
            this.loggerImage.Location = new System.Drawing.Point(0, 0);
            this.loggerImage.Name = "loggerImage";
            this.loggerImage.Size = new System.Drawing.Size(725, 600);
            this.loggerImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.loggerImage.TabIndex = 0;
            this.loggerImage.TabStop = false;
            // 
            // LoggerUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.loggerPanel);
            this.Controls.Add(this.loggerText);
            this.Controls.Add(this.loggerLabel);
            this.Name = "LoggerUserControl";
            this.Size = new System.Drawing.Size(725, 775);
            this.loggerPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.loggerImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox loggerText;
        private System.Windows.Forms.Label loggerLabel;
        private System.Windows.Forms.Panel loggerPanel;
        private System.Windows.Forms.PictureBox loggerImage;
    }
}
