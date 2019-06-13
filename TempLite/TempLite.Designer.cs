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
            this.readyStateMessage = new UserControls.ReadyStateMessage();
            this.readingError = new UserControls.ReadingError();
            this.emailDoneUserControl = new UserControls.EmailDoneUserControl();
            this.emailUserControl = new UserControls.EmailUserControl();
            this.loggerProgressBarUserControl = new UserControls.LoggerProgressBarUserControl();
            this.generateDocumentUserControl = new UserControls.GenerateDocumentUserControl();
            this.readerUserControl = new UserControls.ReaderUserControl();
            this.loggerUserControl = new UserControls.LoggerUserControl();
            this.previewPanel = new System.Windows.Forms.Panel();
            this.addEmailUserControl = new UserControls.AddEmailUserControl();
            this.excelPanel = new System.Windows.Forms.Panel();
            this.emailExcel = new System.Windows.Forms.Button();
            this.previewExcel = new System.Windows.Forms.Button();
            this.pdfPanel = new System.Windows.Forms.Panel();
            this.emailPDF = new System.Windows.Forms.Button();
            this.previewPDF = new System.Windows.Forms.Button();
            this.Reading.SuspendLayout();
            this.bg.SuspendLayout();
            this.previewPanel.SuspendLayout();
            this.excelPanel.SuspendLayout();
            this.pdfPanel.SuspendLayout();
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
            this.bg.Controls.Add(this.ReadLoggerButton);
            this.bg.Controls.Add(this.readyStateMessage);
            this.bg.Controls.Add(this.readingError);
            this.bg.Controls.Add(this.emailDoneUserControl);
            this.bg.Controls.Add(this.emailUserControl);
            this.bg.Controls.Add(this.loggerProgressBarUserControl);
            this.bg.Controls.Add(this.generateDocumentUserControl);
            this.bg.Controls.Add(this.readerUserControl);
            this.bg.Controls.Add(this.loggerUserControl);
            this.bg.Controls.Add(this.previewPanel);
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
            // readyStateMessage
            // 
            this.readyStateMessage.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.readyStateMessage, "readyStateMessage");
            this.readyStateMessage.Name = "readyStateMessage";
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
            this.emailDoneUserControl.emailCancelButton.Click += new System.EventHandler(this.emailCancelButton_Click);
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
            // previewPanel
            // 
            this.previewPanel.BackColor = System.Drawing.Color.White;
            this.previewPanel.Controls.Add(this.addEmailUserControl);
            this.previewPanel.Controls.Add(this.excelPanel);
            this.previewPanel.Controls.Add(this.pdfPanel);
            resources.ApplyResources(this.previewPanel, "previewPanel");
            this.previewPanel.Name = "previewPanel";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TempLite_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TempLite_KeyUp);
            // 
            // addEmailUserControl
            // 
            this.addEmailUserControl.BackColor = System.Drawing.Color.White;
            this.addEmailUserControl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.addEmailUserControl, "addEmailUserControl");
            this.addEmailUserControl.Name = "addEmailUserControl";
            this.addEmailUserControl.AddEmailButton.Click += new System.EventHandler(this.addEmailButton_Click);
            this.addEmailUserControl.loggerIdTextbox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TempLite_KeyDown);
            this.addEmailUserControl.loggerIdTextbox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TempLite_KeyUp);
            this.addEmailUserControl.emailTextbox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TempLite_KeyDown);
            this.addEmailUserControl.emailTextbox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TempLite_KeyUp);
            // 
            // excelPanel
            // 
            this.excelPanel.BackColor = System.Drawing.Color.White;
            this.excelPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.excelPanel.Controls.Add(this.emailExcel);
            this.excelPanel.Controls.Add(this.previewExcel);
            resources.ApplyResources(this.excelPanel, "excelPanel");
            this.excelPanel.Name = "excelPanel";
            // 
            // emailExcel
            // 
            resources.ApplyResources(this.emailExcel, "emailExcel");
            this.emailExcel.BackColor = System.Drawing.Color.White;
            this.emailExcel.BackgroundImage = global::TempLite.Properties.Resources.TempLite_04;
            this.emailExcel.Name = "emailExcel";
            this.emailExcel.UseVisualStyleBackColor = false;
            this.emailExcel.Click += new System.EventHandler(this.emailExcel_Click);
            // 
            // previewExcel
            // 
            resources.ApplyResources(this.previewExcel, "previewExcel");
            this.previewExcel.BackColor = System.Drawing.Color.White;
            this.previewExcel.BackgroundImage = global::TempLite.Properties.Resources.TempLite_051;
            this.previewExcel.Name = "previewExcel";
            this.previewExcel.UseVisualStyleBackColor = false;
            this.previewExcel.Click += new System.EventHandler(this.previewExcel_Click);
            // 
            // pdfPanel
            // 
            this.pdfPanel.BackColor = System.Drawing.Color.White;
            this.pdfPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pdfPanel.Controls.Add(this.emailPDF);
            this.pdfPanel.Controls.Add(this.previewPDF);
            resources.ApplyResources(this.pdfPanel, "pdfPanel");
            this.pdfPanel.Name = "pdfPanel";
            // 
            // emailPDF
            // 
            resources.ApplyResources(this.emailPDF, "emailPDF");
            this.emailPDF.BackColor = System.Drawing.Color.White;
            this.emailPDF.BackgroundImage = global::TempLite.Properties.Resources.TempLite_01;
            this.emailPDF.Name = "emailPDF";
            this.emailPDF.UseVisualStyleBackColor = false;
            this.emailPDF.Click += new System.EventHandler(this.emailPDF_Click);
            // 
            // previewPDF
            // 
            resources.ApplyResources(this.previewPDF, "previewPDF");
            this.previewPDF.BackColor = System.Drawing.Color.White;
            this.previewPDF.BackgroundImage = global::TempLite.Properties.Resources.TempLite_021;
            this.previewPDF.Name = "previewPDF";
            this.previewPDF.UseVisualStyleBackColor = false;
            this.previewPDF.Click += new System.EventHandler(this.previewPDF_Click);
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
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TempLite_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TempLite_KeyUp);
            this.Reading.ResumeLayout(false);
            this.Reading.PerformLayout();
            this.bg.ResumeLayout(false);
            this.previewPanel.ResumeLayout(false);
            this.excelPanel.ResumeLayout(false);
            this.pdfPanel.ResumeLayout(false);
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
        private System.Windows.Forms.Panel previewPanel;
        private System.Windows.Forms.Panel pdfPanel;
        private System.Windows.Forms.Panel excelPanel;
        private System.Windows.Forms.Button previewPDF;
        private System.Windows.Forms.Button emailPDF;
        private System.Windows.Forms.Button emailExcel;
        private System.Windows.Forms.Button previewExcel;
        private AddEmailUserControl addEmailUserControl;
    }
}