namespace UserControls
{
    partial class GenerateDocumentUserControl
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
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.GeneratingDocumentLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // progressBar
            // 
            this.progressBar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(33)))), ((int)(((byte)(99)))));
            this.progressBar.Location = new System.Drawing.Point(48, 138);
            this.progressBar.MarqueeAnimationSpeed = 16;
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(234, 16);
            this.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressBar.TabIndex = 8;
            // 
            // GeneratingDocumentLabel
            // 
            this.GeneratingDocumentLabel.Font = new System.Drawing.Font("Myriad Pro", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GeneratingDocumentLabel.ForeColor = System.Drawing.Color.Black;
            this.GeneratingDocumentLabel.Location = new System.Drawing.Point(35, 47);
            this.GeneratingDocumentLabel.Name = "GeneratingDocumentLabel";
            this.GeneratingDocumentLabel.Size = new System.Drawing.Size(256, 80);
            this.GeneratingDocumentLabel.TabIndex = 6;
            this.GeneratingDocumentLabel.Text = "Generating Documents";
            this.GeneratingDocumentLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // GenerateDocumentUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.GeneratingDocumentLabel);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(33)))), ((int)(((byte)(99)))));
            this.Name = "GenerateDocumentUserControl";
            this.Size = new System.Drawing.Size(323, 198);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label GeneratingDocumentLabel;
    }
}
