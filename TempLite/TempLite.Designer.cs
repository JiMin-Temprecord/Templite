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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TempLite));
            this.readerTextbox = new System.Windows.Forms.TextBox();
            this.readerImage = new System.Windows.Forms.PictureBox();
            this.readerLabel = new System.Windows.Forms.Label();
            this.readerPanel = new System.Windows.Forms.Panel();
            this.loggerPanel = new System.Windows.Forms.Panel();
            this.loggerImage = new System.Windows.Forms.PictureBox();
            this.loggerText = new System.Windows.Forms.TextBox();
            this.loggerLabel = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ReadingTextBox = new System.Windows.Forms.TextBox();
            this.ReadingProgressBar = new System.Windows.Forms.ProgressBar();
            this.ReadingLabel = new System.Windows.Forms.Label();
            this.readerBackgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.readingBackgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.loggerBackgroundWorker = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)(this.readerImage)).BeginInit();
            this.readerPanel.SuspendLayout();
            this.loggerPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.loggerImage)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // readerTextbox
            // 
            this.readerTextbox.BackColor = System.Drawing.Color.White;
            this.readerTextbox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.readerTextbox.Cursor = System.Windows.Forms.Cursors.Default;
            resources.ApplyResources(this.readerTextbox, "readerTextbox");
            this.readerTextbox.Name = "readerTextbox";
            this.readerTextbox.ReadOnly = true;
            this.readerTextbox.TabStop = false;
            // 
            // readerImage
            // 
            this.readerImage.Image = global::TempLite.Properties.Resources.reader_l;
            resources.ApplyResources(this.readerImage, "readerImage");
            this.readerImage.Name = "readerImage";
            this.readerImage.TabStop = false;
            // 
            // readerLabel
            // 
            resources.ApplyResources(this.readerLabel, "readerLabel");
            this.readerLabel.BackColor = System.Drawing.Color.Transparent;
            this.readerLabel.Name = "readerLabel";
            // 
            // readerPanel
            // 
            this.readerPanel.AllowDrop = true;
            this.readerPanel.BackColor = System.Drawing.Color.White;
            this.readerPanel.Controls.Add(this.loggerPanel);
            this.readerPanel.Controls.Add(this.readerLabel);
            this.readerPanel.Controls.Add(this.readerImage);
            this.readerPanel.Controls.Add(this.readerTextbox);
            resources.ApplyResources(this.readerPanel, "readerPanel");
            this.readerPanel.Name = "readerPanel";
            // 
            // loggerPanel
            // 
            this.loggerPanel.AllowDrop = true;
            this.loggerPanel.BackColor = System.Drawing.Color.White;
            this.loggerPanel.Controls.Add(this.loggerImage);
            this.loggerPanel.Controls.Add(this.loggerText);
            this.loggerPanel.Controls.Add(this.loggerLabel);
            resources.ApplyResources(this.loggerPanel, "loggerPanel");
            this.loggerPanel.Name = "loggerPanel";
            this.loggerPanel.TabStop = true;
            this.loggerPanel.VisibleChanged += new System.EventHandler(this.loggerPanel_VisibleChanged);
            // 
            // loggerImage
            // 
            this.loggerImage.AccessibleRole = System.Windows.Forms.AccessibleRole.Window;
            resources.ApplyResources(this.loggerImage, "loggerImage");
            this.loggerImage.Image = global::TempLite.Properties.Resources.loggers_l;
            this.loggerImage.Name = "loggerImage";
            this.loggerImage.TabStop = false;
            // 
            // loggerText
            // 
            this.loggerText.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.loggerText, "loggerText");
            this.loggerText.Name = "loggerText";
            // 
            // loggerLabel
            // 
            resources.ApplyResources(this.loggerLabel, "loggerLabel");
            this.loggerLabel.BackColor = System.Drawing.Color.Transparent;
            this.loggerLabel.Name = "loggerLabel";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.ReadingTextBox);
            this.panel1.Controls.Add(this.ReadingProgressBar);
            this.panel1.Controls.Add(this.ReadingLabel);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // ReadingTextBox
            // 
            this.ReadingTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ReadingTextBox.Cursor = System.Windows.Forms.Cursors.Arrow;
            resources.ApplyResources(this.ReadingTextBox, "ReadingTextBox");
            this.ReadingTextBox.Name = "ReadingTextBox";
            // 
            // ReadingProgressBar
            // 
            resources.ApplyResources(this.ReadingProgressBar, "ReadingProgressBar");
            this.ReadingProgressBar.Name = "ReadingProgressBar";
            this.ReadingProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.ReadingProgressBar.Value = 35;
            // 
            // ReadingLabel
            // 
            resources.ApplyResources(this.ReadingLabel, "ReadingLabel");
            this.ReadingLabel.Name = "ReadingLabel";
            // 
            // TempLite
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.readerPanel);
            this.IsMdiContainer = true;
            this.Name = "TempLite";
            this.Shown += new System.EventHandler(this.TempLite_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.readerImage)).EndInit();
            this.readerPanel.ResumeLayout(false);
            this.readerPanel.PerformLayout();
            this.loggerPanel.ResumeLayout(false);
            this.loggerPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.loggerImage)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox readerImage;
        private System.Windows.Forms.Label readerLabel;
        private System.Windows.Forms.PictureBox loggerImage;
        private System.Windows.Forms.TextBox loggerText;
        private System.Windows.Forms.Label loggerLabel;
        private System.Windows.Forms.Panel loggerPanel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label ReadingLabel;
        private System.Windows.Forms.ProgressBar ReadingProgressBar;
        private System.Windows.Forms.TextBox ReadingTextBox;
        private System.Windows.Forms.TextBox readerTextbox;
        private System.ComponentModel.BackgroundWorker readerBackgroundWorker;
        private System.ComponentModel.BackgroundWorker readingBackgroundWorker;
        private System.ComponentModel.BackgroundWorker loggerBackgroundWorker;
        public System.Windows.Forms.Panel readerPanel;
    }
}

