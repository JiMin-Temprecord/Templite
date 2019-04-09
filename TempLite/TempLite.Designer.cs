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
            this.bg = new System.Windows.Forms.Panel();
            this.ReadLoggerButton = new System.Windows.Forms.Button();
            this.readingError = new UserControls.ReadingError();
            this.emailDoneUserControl = new UserControls.EmailDoneUserControl();
            this.emailUserControl = new UserControls.EmailUserControl();
            this.loggerProgressBarUserControl = new UserControls.LoggerProgressBarUserControl();
            this.generateDocumentUserControl = new UserControls.GenerateDocumentUserControl();
            this.readerUserControl = new UserControls.ReaderUserControl();
            this.loggerUserControl = new UserControls.LoggerUserControl();
            this.readyStateMessage = new UserControls.ReadyStateMessage();
            this.Reading.SuspendLayout();
            this.bg.SuspendLayout();
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
            // bg
            // 
            this.bg.BackColor = System.Drawing.Color.White;
            this.bg.Controls.Add(this.readyStateMessage);
            this.bg.Controls.Add(this.readingError);
            this.bg.Controls.Add(this.emailDoneUserControl);
            this.bg.Controls.Add(this.emailUserControl);
            this.bg.Controls.Add(this.ReadLoggerButton);
            this.bg.Controls.Add(this.loggerProgressBarUserControl);
            this.bg.Controls.Add(this.generateDocumentUserControl);
            this.bg.Controls.Add(this.readerUserControl);
            this.bg.Controls.Add(this.loggerUserControl);
            resources.ApplyResources(this.bg, "bg");
            this.bg.Name = "bg";
            // 
            // ReadLoggerButton
            // 
            this.ReadLoggerButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            resources.ApplyResources(this.ReadLoggerButton, "ReadLoggerButton");
            this.ReadLoggerButton.Name = "ReadLoggerButton";
            this.ReadLoggerButton.UseVisualStyleBackColor = true;
            this.ReadLoggerButton.Click += new System.EventHandler(this.ReadLoggerButton_Click);
            // 
            // readingError
            // 
            resources.ApplyResources(this.readingError, "readingError");
            this.readingError.BackColor = System.Drawing.Color.White;
            this.readingError.Name = "readingError";
            // 
            // emailDoneUserControl
            // 
            resources.ApplyResources(this.emailDoneUserControl, "emailDoneUserControl");
            this.emailDoneUserControl.Name = "emailDoneUserControl";
            // 
            // emailUserControl
            // 
            this.emailUserControl.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.emailUserControl, "emailUserControl");
            this.emailUserControl.Name = "emailUserControl";
            this.emailUserControl.VisibleChanged += new System.EventHandler(this.emailUserControl_VisibleChanged);
            // 
            // loggerProgressBarUserControl
            // 
            this.loggerProgressBarUserControl.BackColor = System.Drawing.Color.White;
            this.loggerProgressBarUserControl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.loggerProgressBarUserControl, "loggerProgressBarUserControl");
            this.loggerProgressBarUserControl.Name = "loggerProgressBarUserControl";
            this.loggerProgressBarUserControl.VisibleChanged += new System.EventHandler(this.loggerProgressBarUserControl_VisibleChanged);
            // 
            // generateDocumentUserControl
            // 
            this.generateDocumentUserControl.BackColor = System.Drawing.Color.White;
            this.generateDocumentUserControl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.generateDocumentUserControl, "generateDocumentUserControl");
            this.generateDocumentUserControl.Name = "generateDocumentUserControl";
            this.generateDocumentUserControl.VisibleChanged += new System.EventHandler(this.generateDocumentUserControl_VisibleChanged);
            // 
            // readerUserControl
            // 
            this.readerUserControl.BackColor = System.Drawing.Color.White;
            this.readerUserControl.Cursor = System.Windows.Forms.Cursors.Default;
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
            // readyStateMessage
            // 
            this.readyStateMessage.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.readyStateMessage, "readyStateMessage");
            this.readyStateMessage.Name = "readyStateMessage";
            // 
            // TempLite
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.bg);
            this.ForeColor = System.Drawing.Color.Black;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.IsMdiContainer = true;
            this.MaximizeBox = false;
            this.Name = "TempLite";
            this.Reading.ResumeLayout(false);
            this.Reading.PerformLayout();
            this.bg.ResumeLayout(false);
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
        private System.Windows.Forms.Panel bg;
        private GenerateDocumentUserControl generateDocumentUserControl;
        private System.Windows.Forms.Button ReadLoggerButton;
        private EmailUserControl emailUserControl;
        private EmailDoneUserControl emailDoneUserControl;
        private ReadingError readingError;
        private ReadyStateMessage readyStateMessage;
    }
}

