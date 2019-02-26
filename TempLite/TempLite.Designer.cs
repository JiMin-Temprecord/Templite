namespace TempLite
{
    partial class TempLite
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.readerImage = new System.Windows.Forms.PictureBox();
            this.readerLabel = new System.Windows.Forms.Label();
            this.readerTextbox = new System.Windows.Forms.TextBox();
            this.readerPanel = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.readerImage)).BeginInit();
            this.readerPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // readerImage
            // 
            this.readerImage.Image = global::TempLite.Properties.Resources.reader_l;
            this.readerImage.Location = new System.Drawing.Point(0, 0);
            this.readerImage.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.readerImage.Name = "readerImage";
            this.readerImage.Size = new System.Drawing.Size(783, 590);
            this.readerImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.readerImage.TabIndex = 0;
            this.readerImage.TabStop = false;
            // 
            // readerLabel
            // 
            this.readerLabel.AutoSize = true;
            this.readerLabel.BackColor = System.Drawing.Color.Transparent;
            this.readerLabel.Font = new System.Drawing.Font("Myriad Pro", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.readerLabel.Location = new System.Drawing.Point(18, 615);
            this.readerLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.readerLabel.Name = "readerLabel";
            this.readerLabel.Size = new System.Drawing.Size(217, 36);
            this.readerLabel.TabIndex = 1;
            this.readerLabel.Text = "Connect Reader";
            // 
            // readerTextbox
            // 
            this.readerTextbox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.readerTextbox.Font = new System.Drawing.Font("Myriad Pro", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.readerTextbox.Location = new System.Drawing.Point(27, 675);
            this.readerTextbox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.readerTextbox.Name = "readerTextbox";
            this.readerTextbox.Size = new System.Drawing.Size(717, 20);
            this.readerTextbox.TabIndex = 2;
            this.readerTextbox.Text = "Use a suitable adaptor to connect a Temprecord reader to your device.";
            // 
            // readerPanel
            // 
            this.readerPanel.BackColor = System.Drawing.Color.White;
            this.readerPanel.Controls.Add(this.readerImage);
            this.readerPanel.Controls.Add(this.readerTextbox);
            this.readerPanel.Controls.Add(this.readerLabel);
            this.readerPanel.Location = new System.Drawing.Point(18, 22);
            this.readerPanel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.readerPanel.Name = "readerPanel";
            this.readerPanel.Size = new System.Drawing.Size(783, 738);
            this.readerPanel.TabIndex = 3;
            // 
            // TempLite
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(821, 884);
            this.Controls.Add(this.readerPanel);
            this.Font = new System.Drawing.Font("Myriad Pro", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "TempLite";
            this.Text = "TempLite";
            ((System.ComponentModel.ISupportInitialize)(this.readerImage)).EndInit();
            this.readerPanel.ResumeLayout(false);
            this.readerPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox readerImage;
        private System.Windows.Forms.Label readerLabel;
        private System.Windows.Forms.TextBox readerTextbox;
        private System.Windows.Forms.Panel readerPanel;
    }
}

