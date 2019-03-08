using System;
using System.ComponentModel;
using System.IO.Ports;
using System.Windows.Forms;
using TempLite.Services;

namespace TempLite
{
    public partial class TempLite : Form
    {
        PDFGenerator _pdfGenerator = new PDFGenerator();
        CommunicationServices _communicationService = new CommunicationServices();
        SerialPort _serialPort = new SerialPort();
       
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
            Reader _reader = new Reader();
            var findReader = false;
            
            while (findReader == false)
            {
                findReader = _reader.FindFTDI();
                if (findReader == true)
                {
                    _reader.SetUpCom(_serialPort);
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
            while (_communicationService.FindLogger == false)
            {
                _communicationService.ReadLogger(_serialPort,Command.WakeUp);
            }
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
            _communicationService.ReadLogger(_serialPort, Command.SetRead);
            _pdfGenerator.CreatePDF(_communicationService);
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
            Email _email = new Email();
            ExcelGenerator _excelGenerator = new ExcelGenerator();

            _excelGenerator.CreateExcel(_communicationService);
            _email.SetUpEmail(_communicationService.serialnumber);
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
