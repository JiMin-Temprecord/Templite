using System;
using System.ComponentModel;
using System.IO.Ports;
using System.Windows.Forms;
using TempLite.Services;

namespace TempLite
{
    public partial class TempLite : Form
    {
        private PDFGenerator _pdfGenerator = new PDFGenerator();
        private CommunicationServices _communicationService;
        private SerialPort _serialPort;
        private Reader _reader;
        private bool findReader = false;



        public TempLite()
        {
            InitializeComponent();
            _communicationService = new CommunicationServices();
            readerBackgroundWorker.DoWork += new DoWorkEventHandler(readerBackgroundWorker_DoWork);
            readerBackgroundWorker.Disposed += new EventHandler(readerBackgroundWorker_Disposed);

        }

        private void TempLite_Shown(object sender, EventArgs e)
        {
            //readerBackgroundWorker.RunWorkerAsync();
            _serialPort = new SerialPort();
            _reader = new Reader();

            while (findReader == false)
            {
                findReader = _reader.FindFTDI();
                if (findReader == true)
                {
                    _reader.SetUpCom(_serialPort);
                    readerPanel.Visible = false;
                    loggerPanel.Visible = true;
                    //worker.Dispose();
                }
            }
        }

        private void readerBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            //BackgroundWorker worker = sender as BackgroundWorker;
            
            _serialPort = new SerialPort();
            _reader = new Reader();

            while (findReader == false)
            {
                findReader = _reader.FindFTDI();
                if (findReader == true)
                {
                    _reader.SetUpCom(_serialPort);
                    readerPanel.Visible = false;
                    loggerPanel.Visible = true;
                    //worker.Dispose();
                }
            }
        }

        private void readerBackgroundWorker_Disposed(object sender, EventArgs e)
        {
            readerPanel.Visible = false;
            loggerPanel.Visible = true;
        }

        private void loggerPanel_VisibleChanged(object sender, EventArgs e)
        {
            _communicationService.ReadLogger(_serialPort);
            _pdfGenerator.CreatePDF(_communicationService);
        }

    }
}
