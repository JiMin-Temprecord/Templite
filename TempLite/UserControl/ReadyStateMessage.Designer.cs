namespace UserControls
{
    partial class ReadyStateMessage
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
            this.warningImage = new System.Windows.Forms.PictureBox();
            this.warningLabel = new System.Windows.Forms.Label();
            this.readyStateText = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.warningImage)).BeginInit();
            this.SuspendLayout();
            // 
            // warningImage
            // 
            this.warningImage.BackColor = System.Drawing.Color.White;
            this.warningImage.Image = global::TempLite.Properties.Resources.errorWarning;
            this.warningImage.Location = new System.Drawing.Point(51, 3);
            this.warningImage.Name = "warningImage";
            this.warningImage.Size = new System.Drawing.Size(250, 250);
            this.warningImage.TabIndex = 13;
            this.warningImage.TabStop = false;
            // 
            // warningLabel
            // 
            this.warningLabel.BackColor = System.Drawing.Color.White;
            this.warningLabel.Font = new System.Drawing.Font("Myriad Pro", 21.75F, System.Drawing.FontStyle.Bold);
            this.warningLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.warningLabel.Location = new System.Drawing.Point(51, 256);
            this.warningLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.warningLabel.Name = "warningLabel";
            this.warningLabel.Size = new System.Drawing.Size(287, 36);
            this.warningLabel.TabIndex = 14;
            this.warningLabel.Text = "No Data Available";
            // 
            // readyStateText
            // 
            this.readyStateText.BackColor = System.Drawing.Color.White;
            this.readyStateText.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.readyStateText.Font = new System.Drawing.Font("Myriad Pro", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.readyStateText.Location = new System.Drawing.Point(51, 298);
            this.readyStateText.Multiline = true;
            this.readyStateText.Name = "readyStateText";
            this.readyStateText.ReadOnly = true;
            this.readyStateText.Size = new System.Drawing.Size(252, 54);
            this.readyStateText.TabIndex = 15;
            this.readyStateText.Text = "Logger is in Ready State or Start Delay.  There is no data available.";
            // 
            // ReadyStateMessage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.readyStateText);
            this.Controls.Add(this.warningLabel);
            this.Controls.Add(this.warningImage);
            this.Name = "ReadyStateMessage";
            this.Size = new System.Drawing.Size(350, 370);
            ((System.ComponentModel.ISupportInitialize)(this.warningImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox warningImage;
        private System.Windows.Forms.Label warningLabel;
        private System.Windows.Forms.TextBox readyStateText;
    }
}
