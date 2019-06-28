namespace UserControls
{
    partial class LoggerProgressBarUserControl
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
            this.progressBarText = new System.Windows.Forms.TextBox();
            this.progressBarLabel = new System.Windows.Forms.Label();
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
            this.progressBar.TabIndex = 5;
            // 
            // progressBarText
            // 
            this.progressBarText.BackColor = System.Drawing.Color.White;
            this.progressBarText.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.progressBarText.Font = new System.Drawing.Font("Myriad Pro", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.progressBarText.ForeColor = System.Drawing.Color.Black;
            this.progressBarText.Location = new System.Drawing.Point(48, 86);
            this.progressBarText.Multiline = true;
            this.progressBarText.Name = "progressBarText";
            this.progressBarText.ReadOnly = true;
            this.progressBarText.Size = new System.Drawing.Size(234, 46);
            this.progressBarText.TabIndex = 4;
            this.progressBarText.Text = "Do not unplug the reader or remove the logger";
            // 
            // progressBarLabel
            // 
            this.progressBarLabel.Font = new System.Drawing.Font("Myriad Pro", 21.75F, System.Drawing.FontStyle.Bold);
            this.progressBarLabel.ForeColor = System.Drawing.Color.Black;
            this.progressBarLabel.Location = new System.Drawing.Point(42, 47);
            this.progressBarLabel.Name = "progressBarLabel";
            this.progressBarLabel.Size = new System.Drawing.Size(260, 36);
            this.progressBarLabel.TabIndex = 3;
            this.progressBarLabel.Text = "Reading Logger";
            // 
            // LoggerProgressBarUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.progressBarText);
            this.Controls.Add(this.progressBarLabel);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(33)))), ((int)(((byte)(99)))));
            this.Name = "LoggerProgressBarUserControl";
            this.Size = new System.Drawing.Size(323, 198);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.TextBox progressBarText;
        private System.Windows.Forms.Label progressBarLabel;
    }
}
