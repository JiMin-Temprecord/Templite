using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Windows.Forms;
using TempLite.Services;
using TempLite.Constant;
using UserControls;
using System.Drawing;
using System.Collections.Generic;

namespace TempLite
{
    public partial class TempLite : Form
    {
        PasswordUserControl passwordUserControl;

        CommunicationServices communicationService = new CommunicationServices();
        Email email = new Email();
        LoggerInformation loggerInformation; 
        SerialPort serialPort = new SerialPort();

        BackgroundWorker readerBW;
        BackgroundWorker loggerBW;
        BackgroundWorker progressBarBW;
        BackgroundWorker documentBW;
        BackgroundWorker sendingEmailBW;
        BackgroundWorker previewPanelBW;

        bool errorDectected = false;
        bool loggerHasStarted = true;

        bool pdfOnly = false;
        bool excelOnly = false;
        
        bool resetTrigger = false;
        bool addEmailTrigger = false;

        string path = AppDomain.CurrentDomain.BaseDirectory + "Email\\";

        public TempLite()
        {
            InitializeComponent();
            InitalizePasswordUserControl();
            readerUserControl.Visible = true;
            Log.Write(LogConstant.OpenApplication);
        }
        void TempLite_FormClosed(object sender, FormClosedEventArgs e)
        {
            Log.Write(LogConstant.CloseApplication);
            Log.Write("==================================================================");
        }

        #region Find Reader
        void readerUserControl_VisibleChanged(object sender, EventArgs e)
        {
            readerBW = new BackgroundWorker();
            readerBW.DoWork += readerBackgroundWorker_DoWork;
            readerBW.RunWorkerCompleted += readerBackgroundWorker_RunWorkerCompleted;
            readerBW.WorkerReportsProgress = true;

            readerBW.WorkerSupportsCancellation = true;
            if (readerUserControl.Visible)
                readerBW.RunWorkerAsync();
        }

        void readerBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                var reader = new Reader();
                var ftdiInfo = reader.FindFTDI();
                if (ftdiInfo != null)
                {
                    reader.SetUpCom(serialPort, ftdiInfo);
                }

                while (ftdiInfo == null)
                {
                    ftdiInfo = reader.FindFTDI();
                    if (ftdiInfo != null)
                    {
                        reader.SetUpCom(serialPort, ftdiInfo);
                    }
                }
            }
            catch
            {
                //Assembly.LoadFrom("FTD2XX_NET.dll");
                //MessageBox.Show("Please Plug in the Reader and Restart the Application");
            }
        }
        void readerBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            readerUserControl.Visible = false;
            loggerUserControl.Visible = true;
            loggerUserControl.BringToFront();
            readerBW.Dispose();
        }
        #endregion
        #region Find Logger
        void loggerUserControl_VisibleChanged(object sender, EventArgs e)
        {
            emailDoneUserControl.Visible = false;
            ReadLoggerButton.Visible = false;
            loggerBW = new BackgroundWorker();
            loggerBW.DoWork += loggerBackgroundWorker_DoWork;
            loggerBW.RunWorkerCompleted += loggerBackgroundWorker_RunWorkerCompleted;
            loggerBW.WorkerReportsProgress = true;
            loggerBW.WorkerSupportsCancellation = true;

            if (loggerUserControl.Visible)
                loggerBW.RunWorkerAsync();
        }

        void loggerBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            if(SerialPort.GetPortNames().Contains<string>(serialPort.PortName))
                communicationService.FindLogger(serialPort);
        }

        void loggerBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (SerialPort.GetPortNames().Contains<string>(serialPort.PortName))
            {
                loggerProgressBarUserControl.Visible = true;
                loggerProgressBarUserControl.BringToFront();
                emailListUserControl.Visible = false;
                passwordUserControl.Visible = false;
                ChangePreviewPanelEnable(true);
            }
            else
            {
                readerUserControl.Visible = true;
                loggerUserControl.Visible = false;
            }

            loggerBW.Dispose();
        }
        #endregion
        #region Reading Logger
        void loggerProgressBarUserControl_VisibleChanged(object sender, EventArgs e)
        {
            progressBarBW = new BackgroundWorker();
            progressBarBW.DoWork += progressBarBW_DoWork;
            progressBarBW.RunWorkerCompleted += progressBarBW_RunWorkerCompleted;
            progressBarBW.WorkerReportsProgress = true;
            progressBarBW.WorkerSupportsCancellation = true;

            if (loggerProgressBarUserControl.Visible)
                progressBarBW.RunWorkerAsync();
        }
        void progressBarBW_DoWork(object sender, DoWorkEventArgs e)
        {
            loggerInformation = new LoggerInformation();

            if (SerialPort.GetPortNames().Contains(serialPort.PortName))
            {
                errorDectected = communicationService.GenerateHexFile(serialPort, loggerInformation);
                Log.Write(LogConstant.ReadingLogger);
            }
        }

        void progressBarBW_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (errorDectected)
            {
                Log.Write(LogConstant.ErrorReadingLogger);
                loggerUserControl.Visible = false;
                loggerProgressBarUserControl.Visible = false;
                readingErrorUserControl.Visible = true;
                ReadLoggerButton.Visible = true;
            }

            else if (SerialPort.GetPortNames().Contains(serialPort.PortName))
            {
                Log.Write(LogConstant.ReadLogger + loggerInformation.SerialNumber);
                loggerProgressBarUserControl.Visible = false;
                generateDocumentUserControl.Visible = true;
                generateDocumentUserControl.BringToFront();
            }
            else
            {
                Log.Write(LogConstant.ReadLogger + loggerInformation.SerialNumber);
                loggerProgressBarUserControl.Visible = false;
                loggerUserControl.Visible = false;
                readerUserControl.Visible = true;
            }

            progressBarBW.Dispose();
        }
        #endregion
        #region Generating Documents
        void generateDocumentUserControl_VisibleChanged(object sender, EventArgs e)
        {
            documentBW = new BackgroundWorker();
            documentBW.DoWork += documentBW_DoWork;
            documentBW.RunWorkerCompleted += documentBW_RunWorkerCompleted;
            documentBW.WorkerReportsProgress = true;
            documentBW.WorkerSupportsCancellation = true;

            if (generateDocumentUserControl.Visible)
                documentBW.RunWorkerAsync();

        }

        void documentBW_DoWork(object sender, DoWorkEventArgs e)
        {
            var pdfGenerator = new PDFGenerator();
            var excelGenerator = new ExcelGenerator();
            
            loggerHasStarted = pdfGenerator.CreatePDF(loggerInformation);
            if(loggerHasStarted)
                excelGenerator.CreateExcel(loggerInformation);
        }

        void documentBW_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (loggerHasStarted)
            {
                if (loggerInformation.EmailId == string.Empty)
                {
                    loggerUserControl.Visible = false;
                    generateDocumentUserControl.Visible = false;
                    previewPanel.Visible = true;
                    ReadLoggerButton.Visible = true;
                }

                else if (loggerInformation.EmailId == null)
                {
                    loggerUserControl.Visible = false;
                    generateDocumentUserControl.Visible = false;
                    readingErrorUserControl.Visible = true;
                    ReadLoggerButton.Visible = true;
                }

                else
                {
                    var numberOfEmail = email.Count(loggerInformation.EmailId);
                    if (numberOfEmail > 0)
                    {
                        loggerUserControl.Visible = false;
                        generateDocumentUserControl.Visible = false;
                        sendingEmailUserControl.BorderStyle = BorderStyle.None;
                        sendingEmailUserControl.Visible = true;
                        sendingEmailUserControl.BringToFront();
                        pdfOnly = false;
                        excelOnly = false;
                    }
                    else
                    {
                        loggerUserControl.Visible = false;
                        generateDocumentUserControl.Visible = false;
                        previewPanel.Visible = true;
                        ReadLoggerButton.Visible = true;
                    }
                }
            }
            else
            {
                generateDocumentUserControl.Visible = false;
                loggerUserControl.Visible = false;
                readyStateMessageUserControl.Visible = true;
                ReadLoggerButton.Visible = true;
            }
            documentBW.Dispose();
        }
        #endregion
        #region Sending Email
        void emailUserControl_VisibleChanged(object sender, EventArgs e)
        {
            sendingEmailBW = new BackgroundWorker();
            sendingEmailBW.DoWork += sendingEmailBW_DoWork;
            sendingEmailBW.RunWorkerCompleted += sendingEmailBW_RunWorkerCompleted;
            sendingEmailBW.WorkerReportsProgress = true;
            sendingEmailBW.WorkerSupportsCancellation = true;

            if (sendingEmailUserControl.Visible)
                sendingEmailBW.RunWorkerAsync();

        }

        void sendingEmailBW_DoWork(object sender, DoWorkEventArgs e)
        {
            var email = new Email();
            if (pdfOnly)
                email.SendAutomatically(loggerInformation.SerialNumber, loggerInformation.EmailId, 0);
            else if (excelOnly)
                email.SendAutomatically(loggerInformation.SerialNumber, loggerInformation.EmailId, 1);
            else
                email.SendAutomatically(loggerInformation.SerialNumber, loggerInformation.EmailId);
        }

        void sendingEmailBW_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //instead of the logging the EmailID We need to log the EmailAddress itself, for all email address 
            emailDoneUserControl.successTextbox.Text = "Sent " + loggerInformation.SerialNumber + " to " + email.GetEmailAddress(loggerInformation.EmailId);
            Log.Write(LogConstant.EmailSuccessfull + loggerInformation.EmailId);
            loggerUserControl.Visible = false;
            sendingEmailUserControl.Visible = false;
            ReadLoggerButton.Visible = true;

            if (pdfOnly || excelOnly)
            {
                emailDoneUserControl.BorderStyle = BorderStyle.FixedSingle;
                emailDoneUserControl.BringToFront();
                emailDoneUserControl.Visible = true;
                emailDoneUserControl.emailCancelButton.Visible = true;
                emailDoneUserControl.emailCancelButton.Click += EmailDoneCancelButton_Click;
            }
            else
            {
                emailDoneUserControl.BorderStyle = BorderStyle.None;
                emailDoneUserControl.BringToFront();
                emailDoneUserControl.Visible = true;
                emailDoneUserControl.emailCancelButton.Visible = false;
            }
            sendingEmailBW.Dispose();
        }
        #endregion
        #region Preview Panel

        private void PreviewPanelPreviewPanel_VisibleChanged(object sender, EventArgs e)
        {
            previewPanelBW = new BackgroundWorker();
            previewPanelBW.DoWork += previewPanelBW_DoWork;
            previewPanelBW.RunWorkerCompleted += previewPanelBW_RunWorkerCompleted;
            previewPanelBW.WorkerReportsProgress = true;
            previewPanelBW.WorkerSupportsCancellation = true;

            if (previewPanel.Visible)
                previewPanelBW.RunWorkerAsync();
        }
        void previewPanelBW_DoWork(object sender, DoWorkEventArgs e)
        { 
            var pdfGenerator = new PDFGenerator();
            var excelGenerator = new ExcelGenerator();

            loggerHasStarted = pdfGenerator.CreatePDF(loggerInformation);
            if (loggerHasStarted)
                excelGenerator.CreateExcel(loggerInformation);
        }

        void previewPanelBW_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (loggerHasStarted == false)
            {
                loggerUserControl.Visible = false;
                previewPanel.Visible = false;
                readyStateMessageUserControl.Visible = true;
            }
        }

        private void previewPDF_Click(object sender, EventArgs e)
        {
            Log.Write(LogConstant.PreviewPDF);
            var filename = Path.GetTempPath() + loggerInformation.SerialNumber + ".pdf";
            try
            {
                Process.Start(filename);
            }
            catch
            {
                MessageBox.Show("Unable to detect a PDF reader ");
            }
        }

        private void emailPDF_Click(object sender, EventArgs e)
        {
            Log.Write(LogConstant.EmailPDFPreview);
            
            var emailCount = email.Count(loggerInformation.EmailId);
            if (emailCount> 0)
            {
                pdfOnly = true;
                excelOnly = false;
                sendingEmailUserControl.Visible = true;
                sendingEmailUserControl.BringToFront();
                sendingEmailUserControl.BorderStyle = BorderStyle.FixedSingle;
                ChangePreviewPanelEnable(false);
            }
            else
                email.OpenApplication(loggerInformation.SerialNumber, loggerInformation.EmailId, 0);
        }

        private void previewExcel_Click(object sender, EventArgs e)
        {
            Log.Write(LogConstant.PreviewExcel);

            var filename = Path.GetTempPath() + loggerInformation.SerialNumber + ".xlsx";
            try
            {
                Process.Start(filename);
            }
            catch
            {
                MessageBox.Show("Unable to detect an Excel reader.");
            }
        }

        private void emailExcel_Click(object sender, EventArgs e)
        {
            Log.Write(LogConstant.EmailExcelPreview);
            
            var emailCount = email.Count(loggerInformation.EmailId);
            if (emailCount > 0)
            {
                pdfOnly = false;
                excelOnly = true;
                sendingEmailUserControl.Visible = true;
                sendingEmailUserControl.BringToFront();
                sendingEmailUserControl.BorderStyle = BorderStyle.FixedSingle;
                ChangePreviewPanelEnable(false);
            }
            else
                email.OpenApplication(loggerInformation.SerialNumber, loggerInformation.EmailId, 1);
        }
        #endregion
        #region Hidden Add Email Feature

        string firstKeyDown = string.Empty;
        string secondKeyDown = string.Empty;

        void TempLite_KeyDown(object sender, KeyEventArgs e)
        {
            if (firstKeyDown == string.Empty && secondKeyDown == string.Empty)
                firstKeyDown = e.KeyCode.ToString();

            else if (secondKeyDown == string.Empty)
                secondKeyDown = e.KeyCode.ToString();
        }
        void TempLite_KeyUp(object sender, KeyEventArgs e)
        {
            Debug.WriteLine("KEYUP : " + e.KeyCode.ToString());

            if (e.KeyCode.ToString() == firstKeyDown)
            {
                firstKeyDown = string.Empty;
                secondKeyDown = string.Empty;
            }

            else if (e.KeyCode.ToString() == secondKeyDown)
            {
                secondKeyDown = string.Empty;
            }

            //change to case statement
            //how do I do this more clearly
            if (loggerProgressBarUserControl.Visible == false && generateDocumentUserControl.Visible == false && sendingEmailUserControl.Visible == false)
            {
                if (firstKeyDown == Keys.ControlKey.ToString() && secondKeyDown == Keys.ShiftKey.ToString() && e.KeyCode.ToString() == Keys.E.ToString()) { if (!resetTrigger) ChangeEmailListPanelVisible(); }
                else if (firstKeyDown == Keys.ControlKey.ToString() && secondKeyDown == Keys.ShiftKey.ToString() && e.KeyCode.ToString() == Keys.R.ToString()) { if (!addEmailTrigger) ResetEmailtoDefault(); }
                else if (emailListUserControl.Visible && firstKeyDown == Keys.ControlKey.ToString() && secondKeyDown == Keys.ShiftKey.ToString() && e.KeyCode.ToString() == Keys.A.ToString()) { ChangeAddEmailPanelVisible(sender, e); }
                else if (emailListUserControl.emailListPanel.Visible && firstKeyDown == Keys.ControlKey.ToString() && e.KeyCode.ToString() == Keys.D.ToString()) { ChangeDeleteButtonVisible(); }
                else if (firstKeyDown == Keys.ControlKey.ToString() && e.KeyCode.ToString() == Keys.L.ToString() && ReadLoggerButton.Enabled) { ChangeLogListVisible(); }
                else if (e.KeyCode.ToString() == Keys.Space.ToString() && ReadLoggerButton.Enabled) { ReadLoggerButton_Click(sender, e); }
                else if (e.KeyCode.ToString() == Keys.Enter.ToString() && emailListUserControl.AddEmailButton.Visible) { emailListUserControl.AddEmailButton_Click(sender, e); }
            }
        }

        void ChangeEmailListPanelVisible()
        {
            if (addEmailTrigger)
            {
                if (emailListUserControl.Visible)
                {
                    addEmailTrigger = false;

                    emailListUserControl.Visible = false;
                    ChangePreviewPanelEnable(true);
                }
                else
                {
                    addEmailTrigger = false;

                    passwordUserControl.Visible = false;
                    passwordUserControl.keycodeTextbox.Text = string.Empty;
                    passwordUserControl.promptMessage.Text = string.Empty;
                    ChangePreviewPanelEnable(true);
                }
            }
            else
            {
                addEmailTrigger = true;

                if (passwordUserControl.isFirstCopy)
                {
                    passwordUserControl.Visible = true;
                    passwordUserControl.Enabled = true;
                    passwordUserControl.keycodeLabel.Text = "Enter KeyCode";
                    passwordUserControl.BringToFront();
                    ChangePreviewPanelEnable(false);
                }
                else
                {
                    emailListUserControl.BringToFront();
                    emailListUserControl.Visible = true;
                    emailListUserControl.addEmailPanel.Visible = false;

                    ChangePreviewPanelEnable(false);
                }
            }
        }
        void ResetEmailtoDefault()
        {
            if (resetTrigger)
            {
                resetTrigger = false;
                passwordUserControl.isReset = false;
                passwordUserControl.Visible = false;
                passwordUserControl.keycodeTextbox.Text = string.Empty;
                passwordUserControl.promptMessage.Text = string.Empty;
                ChangePreviewPanelEnable(true);
            }
            else
            {
                resetTrigger = true;
                passwordUserControl.isReset = true;
                passwordUserControl.Visible = true;
                passwordUserControl.Enabled = true;
                passwordUserControl.BringToFront();
                passwordUserControl.keycodeLabel.Text = "Enter KeyCode to Reset Email";
                ChangePreviewPanelEnable(false);
            }
        }
        void ChangeAddEmailPanelVisible(object sender, KeyEventArgs e)
        {
            if (emailListUserControl.addEmailPanel.Visible == true)
            {
                Log.Write(LogConstant.LogViewClosed);
                emailListUserControl.addEmailPanel.Visible = false;
                emailListUserControl.emailListPanel.Visible = true;
            }
            else
            {
                Log.Write(LogConstant.LogViewOpen);
                emailListUserControl.promptMessage.Text = string.Empty;
                emailListUserControl.loggerIDTextbox.Text = EmailConstant.OwnerID;
                emailListUserControl.emailTextbox.Text = EmailConstant.EmailText;
                emailListUserControl.confirmEmailTextbox.Text = EmailConstant.ConfirmEmailText;
                emailListUserControl.addEmailPanel.Visible = true;
            }
        }
        void ChangeDeleteButtonVisible()
        {
            if (Email.emailList.Count > 0)
            {
                for (int i = 0; i < Email.emailList.Count; i++)
                {
                    Email.emailList[i].emailDeleteButton.Visible = !Email.emailList[i].emailDeleteButton.Visible;
                }
            }
        }
        void ChangeLogListVisible()
        {
            if (logUserControl.Visible == true)
            {
                Log.Write(LogConstant.LogViewClosed);
                logUserControl.Visible = false;
                ReadLoggerButton.Visible = true;
            }
            else
            {
                Log.Write(LogConstant.LogViewOpen);
                logUserControl.Visible = true;
                logUserControl.BringToFront();
                logUserControl.logTextBox.Text = Log.Read("log.txt");
                ReadLoggerButton.Visible = false;
                emailListUserControl.Visible = false;
            }
        }
        void ReadLoggerButton_Click(object sender, EventArgs e)
        {
            previewPanel.Visible = false;
            readyStateMessageUserControl.Visible = false;
            readingErrorUserControl.Visible = false;
            ReadLoggerButton.Visible = false;
            loggerUserControl.Visible = true;
        }
        void ChangePreviewPanelEnable (bool boolean)
        {
            previewPanel.Enabled = boolean;
            ReadLoggerButton.Enabled = boolean;
        }
        void InitalizePasswordUserControl()
        {
            passwordUserControl = new PasswordUserControl();
            bg.Controls.Add(passwordUserControl);
            passwordUserControl.Size = new Size(323, 187);
            passwordUserControl.Location = new Point(200, 250);
            passwordUserControl.BringToFront();
            passwordUserControl.Visible = false;

            passwordUserControl.keycodeTextbox.KeyUp += passowrdUserControl_KeyUp;
        }
        void passowrdUserControl_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.ToString() == Keys.Return.ToString())
            {
                if (passwordUserControl.promptMessage.Text == string.Empty)
                {
                    if (addEmailTrigger)
                    {
                        emailListUserControl.addEmailtoList();
                        emailListUserControl.Visible = true;
                        emailListUserControl.BringToFront();
                        emailListUserControl.addEmailPanel.Visible = false;
                        emailListUserControl.emailListPanel.Visible = true;
                        ReadLoggerButton.Enabled = false;
                        logUserControl.Visible = false;
                    }

                    else if (resetTrigger)
                    {
                        resetTrigger = false;
                        ChangePreviewPanelEnable(true);

                        if (Email.emailList.Count == 0)
                        {
                            emailListUserControl.addEmailtoList();
                        }
                    }

                    passwordUserControl.keycodeTextbox.Text = string.Empty;
                    passwordUserControl.Enabled = false;
                }
            }
        }       
        void EmailDoneCancelButton_Click(object sender, EventArgs e)
        {
            ChangePreviewPanelEnable(true);
        }


        #endregion
    }
}
