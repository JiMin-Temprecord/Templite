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
            System.Windows.Forms.TextBox readerTextbox;
            this.readerImage = new System.Windows.Forms.PictureBox();
            this.readerLabel = new System.Windows.Forms.Label();
            this.readerPanel = new System.Windows.Forms.Panel();
            this.loggerPanel = new System.Windows.Forms.Panel();
            this.loggerImage = new System.Windows.Forms.PictureBox();
            this.loggerText = new System.Windows.Forms.TextBox();
            this.loggerLabel = new System.Windows.Forms.Label();
            readerTextbox = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.readerImage)).BeginInit();
            this.readerPanel.SuspendLayout();
            this.loggerPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.loggerImage)).BeginInit();
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
            this.readerLabel.Location = new System.Drawing.Point(21, 610);
            this.readerLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.readerLabel.Name = "readerLabel";
            this.readerLabel.Size = new System.Drawing.Size(217, 36);
            this.readerLabel.TabIndex = 1;
            this.readerLabel.Text = "Connect Reader";
            // 
            // readerTextbox
            // 
            readerTextbox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            readerTextbox.CausesValidation = false;
            readerTextbox.Cursor = System.Windows.Forms.Cursors.Arrow;
            readerTextbox.Font = new System.Drawing.Font("Myriad Pro", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            readerTextbox.Location = new System.Drawing.Point(27, 675);
            readerTextbox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            readerTextbox.Name = "readerTextbox";
            readerTextbox.Size = new System.Drawing.Size(717, 20);
            readerTextbox.TabIndex = 2;
            readerTextbox.Text = "Use a suitable adaptor to connect a Temprecord reader to your device.";
            // 
            // readerPanel
            // 
            this.readerPanel.BackColor = System.Drawing.Color.White;
            this.readerPanel.Controls.Add(this.readerLabel);
            this.readerPanel.Controls.Add(this.readerImage);
            this.readerPanel.Controls.Add(readerTextbox);
            this.readerPanel.Location = new System.Drawing.Point(18, 22);
            this.readerPanel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.readerPanel.Name = "readerPanel";
            this.readerPanel.Size = new System.Drawing.Size(783, 738);
            this.readerPanel.TabIndex = 3;
            // 
            // loggerPanel
            // 
            this.loggerPanel.BackColor = System.Drawing.Color.White;
            this.loggerPanel.Controls.Add(this.loggerImage);
            this.loggerPanel.Controls.Add(this.loggerText);
            this.loggerPanel.Controls.Add(this.loggerLabel);
            this.loggerPanel.Location = new System.Drawing.Point(18, 22);
            this.loggerPanel.Name = "loggerPanel";
            this.loggerPanel.Size = new System.Drawing.Size(783, 738);
            this.loggerPanel.TabIndex = 5;
            this.loggerPanel.Visible = false;
            this.loggerPanel.VisibleChanged += new System.EventHandler(this.loggerPanel_VisibleChanged);
            // 
            // loggerImage
            // 
            this.loggerImage.Image = global::TempLite.Properties.Resources.loggers_l;
            this.loggerImage.Location = new System.Drawing.Point(18, 0);
            this.loggerImage.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.loggerImage.Name = "loggerImage";
            this.loggerImage.Size = new System.Drawing.Size(744, 590);
            this.loggerImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.loggerImage.TabIndex = 3;
            this.loggerImage.TabStop = false;
            // 
            // loggerText
            // 
            this.loggerText.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.loggerText.Font = new System.Drawing.Font("Myriad Pro", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.loggerText.Location = new System.Drawing.Point(27, 675);
            this.loggerText.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.loggerText.Name = "loggerText";
            this.loggerText.Size = new System.Drawing.Size(717, 20);
            this.loggerText.TabIndex = 5;
            this.loggerText.Text = "Insert a G4 or MonT logger into the reader. \n Readering should start automaticall" +
    "y";
            // 
            // loggerLabel
            // 
            this.loggerLabel.AutoSize = true;
            this.loggerLabel.BackColor = System.Drawing.Color.Transparent;
            this.loggerLabel.Font = new System.Drawing.Font("Myriad Pro", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.loggerLabel.Location = new System.Drawing.Point(21, 610);
            this.loggerLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.loggerLabel.Name = "loggerLabel";
            this.loggerLabel.Size = new System.Drawing.Size(189, 36);
            this.loggerLabel.TabIndex = 4;
            this.loggerLabel.Text = "Insert Logger";
            // 
            // TempLite
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(821, 783);
            this.Controls.Add(this.loggerPanel);
            this.Controls.Add(this.readerPanel);
            this.Font = new System.Drawing.Font("Myriad Pro", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "TempLite";
            this.Text = "TempLite";
            this.Shown += new System.EventHandler(this.TempLite_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.readerImage)).EndInit();
            this.readerPanel.ResumeLayout(false);
            this.readerPanel.PerformLayout();
            this.loggerPanel.ResumeLayout(false);
            this.loggerPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.loggerImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox readerImage;
        private System.Windows.Forms.Label readerLabel;
        public System.Windows.Forms.Panel readerPanel;
        private System.Windows.Forms.PictureBox loggerImage;
        private System.Windows.Forms.TextBox loggerText;
        private System.Windows.Forms.Label loggerLabel;
        public System.Windows.Forms.Panel loggerPanel;
    }
}

