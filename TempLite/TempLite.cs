using System;
using System.ComponentModel;
using System.IO.Ports;
using System.Windows.Forms;
using TempLite.Services;

namespace TempLite
{
    public partial class TempLite : Form
    {
        CommunicationServices communicationService = new CommunicationServices();
        LoggerInformation loggerInformation = new LoggerInformation();
        SerialPort serialPort = new SerialPort();
       
        BackgroundWorker readerBW;
        BackgroundWorker loggerBW;
        BackgroundWorker progressBarBW;
        BackgroundWorker sendingEmailBW;



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
            Reader reader = new Reader();
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
                communicationService.FindLogger(serialPort);
        }

        void loggerBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            loggerProgressBarUserControl.Visible = true;
            loggerBW.Dispose();
        }
        #endregion
        #region Reading Logger
        private void loggerProgressBarUserControl_Load(object sender, EventArgs e)
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
            communicationService.GenerateHexFile(serialPort, loggerInformation);
        }

        void progressBarBW_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            loggerUserControl.Visible = false;
            loggerProgressBarUserControl.Visible = false;
            emailUserControl.Visible = true;
            progressBarBW.Dispose();
        }
        #endregion
        #region Sending Email
        void emailUserControl_Load(object sender, EventArgs e)
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
            var pdfGenerator = new PDFGenerator();
            var excelGenerator = new ExcelGenerator();
            //var email = new Email();

            pdfGenerator.CreatePDF(loggerInformation);
            excelGenerator.CreateExcel(loggerInformation);
            //email.SetUpEmail(loggerInformation.SerialNumber);
        }

        void sendingEmailBW_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            emailUserControl.Visible = false;
            emailDoneUserControl.Visible = true;
            sendingEmailBW.Dispose();
        }
        #endregion
    }
}
