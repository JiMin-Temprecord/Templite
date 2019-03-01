using System;
using System.IO.Ports;
using System.Windows.Forms;
using TempLite.Services;
using System.Threading;

namespace TempLite
{
    public partial class TempLite : Form
    {
        private PDFGenerator _pdfGenerator = new PDFGenerator();
        private CommunicationServices _communicationService;
        private SerialPort _serialPort;
        private Reader _reader;
        private string portname = string.Empty;



        public TempLite()
        {
            InitializeComponent();
            _communicationService = new CommunicationServices();
        }

        private void TempLite_Shown(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(FindReader));
            thread.IsBackground = true;
            thread.Start();
        }

        private void FindReader()
        {
            _serialPort = new SerialPort();
            _reader = new Reader();

            while (portname == string.Empty)
            {
                portname = _reader.FindReader(_serialPort);

                if (portname != string.Empty)
                {
                    _reader.SetUpCom(_serialPort);
                    Thread.CurrentThread.Abort();
                }
            }
        }


        public static void State()
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
