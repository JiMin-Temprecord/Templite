using System;
using System.IO.Ports;
using System.Windows.Forms;
using TempLite.Services;

namespace TempLite
{
    public partial class TempLite : Form
    {
        private PDFService _pdfService;
        private _communicationServices _communicationService;
        private SerialPort _serialPort;

        public TempLite()
        {
            InitializeComponent();

            //Initialise services
            _communicationService = new _communicationServices();
            _pdfService = new PDFService(_communicationService);

            //Set up serial port
            _serialPort = new SerialPort();
            new Reader().SetupCom(_serialPort);
        }
    }
}
