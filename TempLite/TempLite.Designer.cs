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
            this.readerTextbox = new System.Windows.Forms.TextBox();
            this.readerImage = new System.Windows.Forms.PictureBox();
            this.readerLabel = new System.Windows.Forms.Label();
            this.readerPanel = new System.Windows.Forms.Panel();
            this.loggerPanel = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ReadingTextBox = new System.Windows.Forms.TextBox();
            this.ReadingProgressBar = new System.Windows.Forms.ProgressBar();
            this.ReadingLabel = new System.Windows.Forms.Label();
            this.loggerImage = new System.Windows.Forms.PictureBox();
            this.loggerText = new System.Windows.Forms.TextBox();
            this.loggerLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.readerImage)).BeginInit();
            this.readerPanel.SuspendLayout();
            this.loggerPanel.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.loggerImage)).BeginInit();
            this.SuspendLayout();
            // 
            // readerTextbox
            // 
            this.readerTextbox.BackColor = System.Drawing.Color.White;
            this.readerTextbox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.readerTextbox.Cursor = System.Windows.Forms.Cursors.Default;
            this.readerTextbox.Enabled = false;
            this.readerTextbox.Font = new System.Drawing.Font("Myriad Pro", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.readerTextbox.Location = new System.Drawing.Point(27, 675);
            this.readerTextbox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.readerTextbox.Name = "readerTextbox";
            this.readerTextbox.ReadOnly = true;
            this.readerTextbox.Size = new System.Drawing.Size(717, 20);
            this.readerTextbox.TabIndex = 1;
            this.readerTextbox.TabStop = false;
            this.readerTextbox.Text = "Use a suitable adaptor to connect a Temprecord reader to your device.";
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
            // readerPanel
            // 
            this.readerPanel.BackColor = System.Drawing.Color.White;
            this.readerPanel.Controls.Add(this.loggerPanel);
            this.readerPanel.Controls.Add(this.readerLabel);
            this.readerPanel.Controls.Add(this.readerImage);
            this.readerPanel.Controls.Add(this.readerTextbox);
            this.readerPanel.Location = new System.Drawing.Point(18, 22);
            this.readerPanel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.readerPanel.Name = "readerPanel";
            this.readerPanel.Size = new System.Drawing.Size(783, 738);
            this.readerPanel.TabIndex = 3;
            // 
            // loggerPanel
            // 
            this.loggerPanel.BackColor = System.Drawing.Color.White;
            this.loggerPanel.Controls.Add(this.panel1);
            this.loggerPanel.Controls.Add(this.loggerImage);
            this.loggerPanel.Controls.Add(this.loggerText);
            this.loggerPanel.Controls.Add(this.loggerLabel);
            this.loggerPanel.Location = new System.Drawing.Point(0, 0);
            this.loggerPanel.Name = "loggerPanel";
            this.loggerPanel.Size = new System.Drawing.Size(783, 738);
            this.loggerPanel.TabIndex = 5;
            this.loggerPanel.TabStop = true;
            this.loggerPanel.Visible = false;
            this.loggerPanel.VisibleChanged += new System.EventHandler(this.loggerPanel_VisibleChanged);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.ReadingTextBox);
            this.panel1.Controls.Add(this.ReadingProgressBar);
            this.panel1.Controls.Add(this.ReadingLabel);
            this.panel1.Location = new System.Drawing.Point(252, 226);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(319, 179);
            this.panel1.TabIndex = 6;
            this.panel1.Visible = false;
            // 
            // ReadingTextBox
            // 
            this.ReadingTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ReadingTextBox.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.ReadingTextBox.Font = new System.Drawing.Font("Myriad Pro", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ReadingTextBox.Location = new System.Drawing.Point(29, 66);
            this.ReadingTextBox.Multiline = true;
            this.ReadingTextBox.Name = "ReadingTextBox";
            this.ReadingTextBox.Size = new System.Drawing.Size(263, 43);
            this.ReadingTextBox.TabIndex = 2;
            this.ReadingTextBox.Text = "Do not unplug the reader or remove the logger";
            // 
            // ReadingProgressBar
            // 
            this.ReadingProgressBar.Location = new System.Drawing.Point(29, 127);
            this.ReadingProgressBar.Name = "ReadingProgressBar";
            this.ReadingProgressBar.Size = new System.Drawing.Size(263, 17);
            this.ReadingProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.ReadingProgressBar.TabIndex = 1;
            this.ReadingProgressBar.Value = 35;
            // 
            // ReadingLabel
            // 
            this.ReadingLabel.AutoSize = true;
            this.ReadingLabel.Font = new System.Drawing.Font("Myriad Pro", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ReadingLabel.Location = new System.Drawing.Point(23, 30);
            this.ReadingLabel.Name = "ReadingLabel";
            this.ReadingLabel.Size = new System.Drawing.Size(205, 33);
            this.ReadingLabel.TabIndex = 0;
            this.ReadingLabel.Text = "Reading Logger";
            // 
            // loggerImage
            // 
            this.loggerImage.Image = global::TempLite.Properties.Resources.loggers_l;
            this.loggerImage.Location = new System.Drawing.Point(16, 0);
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
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
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
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label ReadingLabel;
        private System.Windows.Forms.ProgressBar ReadingProgressBar;
        private System.Windows.Forms.TextBox ReadingTextBox;
        private System.Windows.Forms.TextBox readerTextbox;
    }
}

