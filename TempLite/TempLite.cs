using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Windows.Forms;
using TempLite.Services;

namespace TempLite
{
    public partial class TempLite : Form
    {
        CommunicationServices communicationService = new CommunicationServices();
        LoggerInformation loggerInformation = new LoggerInformation();
        SerialPort serialPort = new SerialPort();
        LoggerVariables loggerVariables;

        BackgroundWorker readerBW;
        BackgroundWorker loggerBW;
        BackgroundWorker progressBarBW;
        BackgroundWorker documentBW;
        BackgroundWorker sendingEmailBW;
        BackgroundWorker previewPanelBW;

        bool errorDectected = false;
        bool loggerHasStarted = true;

        public TempLite()
        {
            InitializeComponent();
         
            readerUserControl.Visible = true;
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
            if(SerialPort.GetPortNames().Contains(serialPort.PortName))
                errorDectected = communicationService.GenerateHexFile(serialPort, loggerInformation);

            var decoder = new HexFileDecoder(loggerInformation);
            decoder.ReadIntoJsonFileAndSetupDecoder();
            loggerVariables = decoder.AssignPDFValue();
        }

        void progressBarBW_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (errorDectected)
            {
                loggerUserControl.Visible = false;
                loggerProgressBarUserControl.Visible = false;
                readingError.Visible = true;
                ReadLoggerButton.Visible = true;
            }

            else if (SerialPort.GetPortNames().Contains(serialPort.PortName))
            {
                Console.WriteLine("EMAIL : " + loggerInformation.EmailId);
                if (loggerInformation.EmailId == string.Empty)
                {
                    loggerUserControl.Visible = false;
                    loggerProgressBarUserControl.Visible = false;
                    previewPanel.Visible = true;
                    ReadLoggerButton.Visible = true;
                }
                else
                {
                    loggerProgressBarUserControl.Visible = false;
                    generateDocumentUserControl.Visible = true;
                }
            }
            else
            {
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
            
            loggerHasStarted = pdfGenerator.CreatePDF(loggerInformation,loggerVariables);
            if(loggerHasStarted)
                excelGenerator.CreateExcel(loggerInformation,loggerVariables);
        }

        void documentBW_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (loggerHasStarted)
            {
                generateDocumentUserControl.Visible = false;
                emailUserControl.Visible = true;
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
            email.SetUpEmail(loggerInformation.SerialNumber, loggerInformation.EmailId);
        }

        void sendingEmailBW_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            loggerUserControl.Visible = false;
            emailUserControl.Visible = false;
            emailDoneUserControl.Visible = true;
            ReadLoggerButton.Visible = true;
            sendingEmailBW.Dispose();
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

        #region Preview Panel

        private void PreviewPanel_VisibleChanged(object sender, EventArgs e)
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

            loggerHasStarted = pdfGenerator.CreatePDF(loggerInformation,loggerVariables);
            if (loggerHasStarted)
                excelGenerator.CreateExcel(loggerInformation, loggerVariables);
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
            var filename = Path.GetTempPath() + "\\" + loggerInformation.SerialNumber + ".pdf";
            Process.Start(filename);
        }

        private void emailPDF_Click(object sender, EventArgs e)
        {
            var email = new Email();
            email.OpenEmailApplication(loggerInformation.SerialNumber, loggerInformation.EmailId,0);
        }

        private void previewExcel_Click(object sender, EventArgs e)
        {
            var filename = Path.GetTempPath() + "\\" + loggerInformation.SerialNumber + ".xlsx";
            Process.Start(filename);
        }

        private void emailExcel_Click(object sender, EventArgs e)
        {
            var email = new Email();
            email.OpenEmailApplication(loggerInformation.SerialNumber, loggerInformation.EmailId, 1);
        }
        #endregion
    }
}
