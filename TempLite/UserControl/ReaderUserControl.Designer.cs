namespace UserControls
{
    partial class ReaderUserControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReaderUserControl));
            this.readerLabel = new System.Windows.Forms.Label();
            this.readerTextbox = new System.Windows.Forms.TextBox();
            this.readerPanel = new System.Windows.Forms.Panel();
            this.readerImage = new System.Windows.Forms.PictureBox();
            this.readerPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.readerImage)).BeginInit();
            this.SuspendLayout();
            // 
            // readerLabel
            // 
            resources.ApplyResources(this.readerLabel, "readerLabel");
            this.readerLabel.BackColor = System.Drawing.Color.Transparent;
            this.readerLabel.Name = "readerLabel";
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
            // readerPanel
            // 
            this.readerPanel.Controls.Add(this.readerImage);
            resources.ApplyResources(this.readerPanel, "readerPanel");
            this.readerPanel.Name = "readerPanel";
            // 
            // readerImage
            // 
            resources.ApplyResources(this.readerImage, "readerImage");
            this.readerImage.Name = "readerImage";
            this.readerImage.TabStop = false;
            // 
            // ReaderUserControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.readerPanel);
            this.Controls.Add(this.readerLabel);
            this.Controls.Add(this.readerTextbox);
            this.Name = "ReaderUserControl";
            this.readerPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.readerImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label readerLabel;
        private System.Windows.Forms.TextBox readerTextbox;
        private System.Windows.Forms.Panel readerPanel;
        private System.Windows.Forms.PictureBox readerImage;
    }
}
