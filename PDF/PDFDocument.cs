using System.Threading.Tasks;

namespace PDF
{
    public class PDFDocument
    {
        /**
         * DrawString (string text, PdfFont font, PdfColor color, int x, int y)
         * DrawImage (PdfImage image, int x, int y, int sizeX, int sizeY)
         * DrawLine (PdfPen pen, int x1, int y1, int x2, int y2)
         * DrawRectangle (PdfPen pen, int x1, int y1, int x2, int y2, int x3, int y3, int x4, int y4);
         * */
        PDFBuilder pdfBuilder = new PDFBuilder();

        public PDFDocument()
        {
            pdfBuilder.BuildPDFStart();
        }
        public string Title { get { return pdfBuilder.Title; } set { pdfBuilder.Title = value; } }
        public PDFPage AddPage ()
        {
            var pdfPage = new PDFPage(pdfBuilder);
            return pdfPage;
        }

        public async Task Save (string path)
        {
            await pdfBuilder.BuildPDFEndAsync(path);
        }
    }
}
