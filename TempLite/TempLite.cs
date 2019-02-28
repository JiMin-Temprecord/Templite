using System;
using System.Drawing;
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

        public TempLite()
        {
            InitializeComponent();
            _communicationService = new CommunicationServices();
        }

        private void TempLite_Shown(object sender, EventArgs e)
        {
            _serialPort = new SerialPort();
            _reader = new Reader();
            string portname = string.Empty;

            while (portname == string.Empty)
            {
                portname = _reader.FindReader(_serialPort);
                System.Threading.Thread.Sleep(500);
            }
            _reader.SetUpCom(_serialPort);
            
            loggerPanel.Visible = true;
            readerPanel.Visible = false;
        }

        private void loggerPanel_VisibleChanged(object sender, EventArgs e)
        {
            _communicationService.ReadLogger(_serialPort);
            _pdfGenerator.CreatePDF(_communicationService);
        }
    }
}
