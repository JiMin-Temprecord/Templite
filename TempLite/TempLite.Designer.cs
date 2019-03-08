using UserControls; 

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
            this.Reading = new System.Windows.Forms.Panel();
            this.ReadingTextBox = new System.Windows.Forms.TextBox();
            this.ReadingProgressBar = new System.Windows.Forms.ProgressBar();
            this.ReadingLabel = new System.Windows.Forms.Label();
            this.loggerProgressBarUserControl = new UserControls.LoggerProgressBarUserControl();
            this.readerUserControl = new UserControls.ReaderUserControl();
            this.loggerUserControl = new UserControls.LoggerUserControl();
            this.emailUserControl = new UserControls.EmailUserControl();
            this.emailDoneUserControl = new UserControls.EmailDoneUserControl();
            this.bg = new System.Windows.Forms.Panel();
            this.Reading.SuspendLayout();
            this.SuspendLayout();
            // 
            // Reading
            // 
            this.Reading.BackColor = System.Drawing.Color.White;
            this.Reading.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Reading.Controls.Add(this.ReadingTextBox);
            this.Reading.Controls.Add(this.ReadingProgressBar);
            this.Reading.Controls.Add(this.ReadingLabel);
            resources.ApplyResources(this.Reading, "Reading");
            this.Reading.Name = "Reading";
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
            // loggerProgressBarUserControl
            // 
            this.loggerProgressBarUserControl.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.loggerProgressBarUserControl, "loggerProgressBarUserControl");
            this.loggerProgressBarUserControl.Name = "loggerProgressBarUserControl";
            this.loggerProgressBarUserControl.Load += new System.EventHandler(this.loggerProgressBarUserControl_Load);
            // 
            // readerUserControl
            // 
            this.readerUserControl.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.readerUserControl, "readerUserControl");
            this.readerUserControl.Name = "readerUserControl";
            this.readerUserControl.VisibleChanged += new System.EventHandler(this.readerUserControl_VisibleChanged);
            // 
            // loggerUserControl
            // 
            this.loggerUserControl.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.loggerUserControl, "loggerUserControl");
            this.loggerUserControl.Name = "loggerUserControl";
            this.loggerUserControl.VisibleChanged += new System.EventHandler(this.loggerUserControl_VisibleChanged);
            // 
            // emailUserControl
            // 
            this.emailUserControl.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.emailUserControl, "emailUserControl");
            this.emailUserControl.Name = "emailUserControl";
            this.emailUserControl.Load += new System.EventHandler(this.emailUserControl_Load);
            // 
            // emailDoneUserControl
            // 
            resources.ApplyResources(this.emailDoneUserControl, "emailDoneUserControl");
            this.emailDoneUserControl.Name = "emailDoneUserControl";
            // 
            // panel1
            // 
            this.bg.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.bg, "panel1");
            this.bg.Name = "panel1";
            // 
            // TempLite
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.emailDoneUserControl);
            this.Controls.Add(this.emailUserControl);
            this.Controls.Add(this.loggerProgressBarUserControl);
            this.Controls.Add(this.readerUserControl);
            this.Controls.Add(this.loggerUserControl);
            this.Controls.Add(this.bg);
            this.ForeColor = System.Drawing.Color.Black;
            this.IsMdiContainer = true;
            this.Name = "TempLite";
            this.Reading.ResumeLayout(false);
            this.Reading.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel Reading;
        private System.Windows.Forms.Label ReadingLabel;
        private System.Windows.Forms.ProgressBar ReadingProgressBar;
        private System.Windows.Forms.TextBox ReadingTextBox;
        private ReaderUserControl readerUserControl;
        private LoggerUserControl loggerUserControl;
        private LoggerProgressBarUserControl loggerProgressBarUserControl;
        private EmailUserControl emailUserControl;
        private EmailDoneUserControl emailDoneUserControl;
        private System.Windows.Forms.Panel bg;
    }
}

