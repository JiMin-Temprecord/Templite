using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Windows.Forms;
using TempLite.Services;
using TempLite.Constant;
using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace TempLite
{
    public partial class TempLite : Form
    {
        CommunicationServices communicationService = new CommunicationServices();
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

        public TempLite()
        {
            InitializeComponent();
            readerUserControl.Visible = true;
            Log.WritetoLog(LogConstant.OpenApplication);
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
            if(SerialPort.GetPortNames().Contains<string>(serialPort.PortName))
                loggerProgressBarUserControl.Visible = true;
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
                Log.WritetoLog(LogConstant.ReadingLogger);
            }
        }

        void progressBarBW_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (errorDectected)
            {
                Log.WritetoLog(LogConstant.ErrorReadingLogger);
                loggerUserControl.Visible = false;
                loggerProgressBarUserControl.Visible = false;
                readingError.Visible = true;
                ReadLoggerButton.Visible = true;
            }

            else if (SerialPort.GetPortNames().Contains(serialPort.PortName))
            {
                Log.WritetoLog(LogConstant.ReadLogger + loggerInformation.SerialNumber);
                loggerProgressBarUserControl.Visible = false;
                generateDocumentUserControl.Visible = true;
            }
            else
            {
                Log.WritetoLog(LogConstant.ReadLogger + loggerInformation.SerialNumber);
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
                Console.WriteLine("EMAIL : " + loggerInformation.EmailId);
                //Need to check if ARCBS LoggerIDs if not go the preview panel. 

                if (loggerInformation.EmailId == string.Empty) //|| loggerInformation.EmailId == "TBS-TEST")
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
                    readingError.Visible = true;
                    ReadLoggerButton.Visible = true;
                }

                else
                {
                    generateDocumentUserControl.Visible = false;
                    emailUserControl.Visible = true;
                    pdfOnly = false;
                    excelOnly = false;
                }
            }
            else
            {
                generateDocumentUserControl.Visible = false;
                loggerUserControl.Visible = false;
                readyStateMessage.Visible = true;
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

            if (emailUserControl.Visible)
                sendingEmailBW.RunWorkerAsync();

        }

        void sendingEmailBW_DoWork(object sender, DoWorkEventArgs e)
        {
            var email = new Email();
            if (pdfOnly)
                email.SendEmailAutomatically(loggerInformation.SerialNumber, loggerInformation.EmailId, 0);
            else if (excelOnly)
                email.SendEmailAutomatically(loggerInformation.SerialNumber, loggerInformation.EmailId, 1);
            else
                email.SendEmailAutomatically(loggerInformation.SerialNumber, loggerInformation.EmailId);

        }

        void sendingEmailBW_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            emailDoneUserControl.successLabel.Text +=  loggerInformation.EmailId;
            Log.WritetoLog(LogConstant.EmailSuccessfull + loggerInformation.EmailId);
            loggerUserControl.Visible = false;
            emailUserControl.Visible = false;
            ReadLoggerButton.Visible = true;

            if (pdfOnly || excelOnly)
            {
                emailDoneUserControl.BorderStyle = BorderStyle.FixedSingle;
                emailDoneUserControl.Visible = true;
                emailDoneUserControl.emailCancelButton.Visible = true;
                previewPanel.Enabled = false;
                ReadLoggerButton.Enabled = false;
            }
            else
            {
                emailDoneUserControl.BorderStyle = BorderStyle.None;
                emailDoneUserControl.Visible = true;
                emailDoneUserControl.emailCancelButton.Visible = false;
                previewPanel.Enabled = false;
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
                readyStateMessage.Visible = true;
            }
        }

        private void previewPDF_Click(object sender, EventArgs e)
        {
            Log.WritetoLog(LogConstant.PreviewPDF);
            var filename = Path.GetTempPath() + "\\" + loggerInformation.SerialNumber + ".pdf";
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
            Log.WritetoLog(LogConstant.EmailPDFPreview);

            var email = new Email();
            if (File.Exists(loggerInformation.EmailId + ".txt"))
            {
                pdfOnly = true;
                excelOnly = false;
                emailUserControl.Visible = true;
            }
            else
                email.OpenEmailApplication(loggerInformation.SerialNumber, loggerInformation.EmailId, 0);
        }

        private void previewExcel_Click(object sender, EventArgs e)
        {
            Log.WritetoLog(LogConstant.PreviewExcel);

            var filename = Path.GetTempPath() + "\\" + loggerInformation.SerialNumber + ".xlsx";
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
            Log.WritetoLog(LogConstant.EmailExcelPreview);

            var email = new Email();
            if (File.Exists(loggerInformation.EmailId + ".txt"))
            {
                pdfOnly = false;
                excelOnly = true;
                emailUserControl.Visible = true;
            }
            else
                email.OpenEmailApplication(loggerInformation.SerialNumber, loggerInformation.EmailId, 1);
        }
        #endregion
        #region Hidden Add Email Feature
        string keyDown = string.Empty;
        private void TempLite_KeyDown(object sender, KeyEventArgs e)
        {
            if(keyDown == string.Empty)
                keyDown = e.KeyCode.ToString();
        }

        private void TempLite_KeyUp(object sender, KeyEventArgs e)
        {
            Debug.WriteLine("KEYUP : " + e.KeyCode.ToString());
            Debug.WriteLine("KEYDOWN : " + keyDown);

            if (e.KeyCode.ToString() == keyDown)
                keyDown = string.Empty;

            if (keyDown == Keys.ControlKey.ToString() && e.KeyCode.ToString() == Keys.A.ToString())
            {
                if (addEmailUserControl.Visible == true)
                {
                    Log.WritetoLog(LogConstant.AddEmailPanelClosed);
                    addEmailUserControl.Visible = false;
                    ReadLoggerButton.Visible = true;
                }
                else
                {
                    Log.WritetoLog(LogConstant.AddEmailPanelOpen);
                    addEmailUserControl.Visible = true;
                    ReadLoggerButton.Visible = false;
                    logUserControl.Visible = false;
                }
            }

            else if (keyDown == Keys.ControlKey.ToString() && e.KeyCode.ToString() == Keys.L.ToString())
            {
                if (logUserControl.Visible == true)
                {
                    Log.WritetoLog(LogConstant.LogViewClosed);
                    logUserControl.Visible = false;
                    ReadLoggerButton.Visible = true;
                }
                else
                {
                    Log.WritetoLog(LogConstant.LogViewOpen);
                    logUserControl.Visible = true;
                    logUserControl.logTextBox.Text = Log.ReadFromlog("log.txt");
                    ReadLoggerButton.Visible = false;
                    addEmailUserControl.Visible = false;
                }
            }

            else if (e.KeyCode.ToString() == Keys.Space.ToString() && ReadLoggerButton.Visible)
                ReadLoggerButton_Click(sender, e);

            else if (e.KeyCode.ToString() == Keys.Return.ToString() && addEmailUserControl.Visible)
                AddEmailButton_Click(sender, e);
        }
        
        private void AddEmailButton_Click (object sender, EventArgs e)
        {
            var loggerID = addEmailUserControl.loggerIdTextbox.Text.ToUpper(); //unless we will in the future have case sensitive ids
            var emailAddress = addEmailUserControl.emailTextbox.Text;
            var textFile = loggerID + ".txt";

            var isEmailValid = Email.IsValid(emailAddress);

            if (addEmailUserControl.loggerIdTextbox.Text == loggerInformation.EmailId)
            {
                if (emailAddress != string.Empty && isEmailValid)
                {
                    if (File.Exists(textFile) && Log.CheckEmail(textFile, emailAddress))
                    {
                            addEmailUserControl.promptMessage.Text = LogConstant.EmailAlreadyExists;
                            addEmailUserControl.promptMessage.ForeColor = Color.Orange;
                    }
                    else
                    {
                        Log.AddEmail(textFile, emailAddress);
                        Log.WritetoLog(LogConstant.EmailAddressAdded);
                        addEmailUserControl.promptMessage.Text = LogConstant.EmailAddressAdded;
                        addEmailUserControl.promptMessage.ForeColor = Color.Green;
                    }
                }
                else
                {
                    Log.WritetoLog(LogConstant.AddEmailThrewError);
                    addEmailUserControl.promptMessage.Text = LogConstant.MissTypeEmail;
                    addEmailUserControl.promptMessage.ForeColor = Color.Red;
                }
                //Log.ReadFromlog(textFile);
            }
            else if (addEmailUserControl.loggerIdTextbox.Text == string.Empty || addEmailUserControl.emailTextbox.Text == string.Empty)
            {
                addEmailUserControl.promptMessage.Text = LogConstant.FieldsEmpty;
                addEmailUserControl.promptMessage.ForeColor = Color.Red;
            }
            else
            {
                Log.WritetoLog(LogConstant.AddEmailThrewError);
                addEmailUserControl.promptMessage.Text = LogConstant.LoggerIdMissMatch;
                addEmailUserControl.promptMessage.ForeColor = Color.Red;
            }
        }
        #endregion

        private void ReadLoggerButton_Click(object sender, EventArgs e)
        {
            previewPanel.Visible = false;
            readyStateMessage.Visible = false;
            readingError.Visible = false;
            ReadLoggerButton.Visible = false;
            loggerUserControl.Visible = true;
        }

        private void TempLite_FormClosed(object sender, FormClosedEventArgs e)
        {
            Log.WritetoLog(LogConstant.CloseApplication);
            Log.WritetoLog("==================================================================");
        }

        private void emailCancelButton_Click(object sender, EventArgs e)
        {
            emailDoneUserControl.Visible = false;
            previewPanel.Enabled = true;
            ReadLoggerButton.Enabled = true;
        }
    }
}
