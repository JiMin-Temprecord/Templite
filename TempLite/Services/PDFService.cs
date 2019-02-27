using System.IO.Ports;

namespace TempLite.Services
{
    public class PDFService
    {
        private _communicationServices _communicationService;
        private PDFGenerator _pdfGenerator = new PDFGenerator();
  
        public PDFService(_communicationServices communicationService)
        {
            _communicationService = communicationService;
        }

        public void Preview(SerialPort serialPort)
        {
            _communicationService.ReadLogger(serialPort);
            _pdfGenerator.createPDF(_communicationService);
        }

        public void Email(SerialPort serialPort)
        {
            _communicationService.ReadLogger(serialPort);
        }

        public void Download(SerialPort serialPort)
        {
            _communicationService.ReadLogger(serialPort);
        }


    }
}
