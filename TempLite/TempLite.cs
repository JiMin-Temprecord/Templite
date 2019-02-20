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

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void PDF_Click(object sender, EventArgs e)
        {
            if (pdfdownload.Visible == false)
            {
                pdfdownload.Visible = true;
                pdfemail.Visible = true;
                pdfpreview.Visible = true;
            }
            else
            {
                pdfdownload.Visible = false;
                pdfemail.Visible = false;
                pdfpreview.Visible = false;
            }
        }

        private void EXCEL_Click(object sender, EventArgs e)
        {
            if (exceldownload.Visible == false)
            {
                exceldownload.Visible = true;
                excelemail.Visible = true;
                excelpreview.Visible = true;
            }
            else
            {
                exceldownload.Visible = false;
                excelemail.Visible = false;
                excelpreview.Visible = false;
            }
        }

        private void pdfpreview_Click(object sender, EventArgs e)
        {
            _pdfService.Preview(_serialPort);
        }

        private void pdfemail_Click(object sender, EventArgs e)
        {
            _pdfService.Email(_serialPort);
        }

        private void pdfdownload_Click(object sender, EventArgs e)
        {
            _pdfService.Download(_serialPort);
        }
    }
}
