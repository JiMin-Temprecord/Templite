using PDF.Drawing;
using System.Drawing;
using System.Text;

namespace PDF
{
    public class PDFPage
    {
        /**Pages start at 6. Keep count of  number of And Pages that have been created
         * Have multiple stringbuilders for each page? and at first we can just try and build it without checking for effeciency
         * Then we can check for duplications
        **/
        readonly PDFBuilder pdfBuilder;
        PDFColor pdfColor;
        public PDFPage(PDFBuilder PdfBuilder)
        {
            pdfBuilder = PdfBuilder;
            pdfBuilder.PageCount++;
        }
        public int Height { get { return pdfBuilder.PageHeight; } set { pdfBuilder.PageHeight = value; } }
        public int Width { get { return pdfBuilder.PageWidth; } set { pdfBuilder.PageWidth = value; } }
        public void DrawString(string text, PDFFont font, Color color, double x, double y)
        {
            var initString = new StringBuilder();
            pdfColor = new PDFColor(color);

            var pdfFont = pdfBuilder.GetFont(font);
            
            initString.Append(pdfColor.R.ToString("N2"));
            initString.Append(PDFConstant.Space);
            initString.Append(pdfColor.G.ToString("N2"));
            initString.Append(PDFConstant.Space);
            initString.Append(pdfColor.B.ToString("N2"));
            initString.Append(PDFConstant.Space);
            initString.Append(PDFConstant.ColorInside);
            initString.Append(PDFConstant.Space);

            initString.Append(PDFConstant.BeginText);
            initString.Append(pdfFont); // to distinguish if FontOne or FontTwo later
            initString.Append(PDFConstant.Space);
            initString.Append(font.FontSize);
            initString.Append(PDFConstant.Space);
            initString.Append(PDFConstant.TextLocation);
            initString.Append(PDFConstant.Space);
            initString.Append(x);
            initString.Append(PDFConstant.Space);
            initString.Append(Height - y);
            initString.Append(PDFConstant.Space);
            initString.Append(PDFConstant.TextOpen);
            initString.Append(PDFConstant.Space);
            initString.Append(text);
            initString.Append(PDFConstant.Space);
            initString.Append(PDFConstant.TextClose);
            initString.Append(PDFConstant.Space);
            initString.Append(PDFConstant.EndText);
            initString.Append(PDFConstant.NewLine);

            pdfBuilder.BuildNewObject(initString);
        }
        public void DrawLine(PDFPen pen, double x1, double y1, double x2, double y2)
        {
            var initString = new StringBuilder();

            initString.Append(pen.pdfColor.R.ToString("N2"));
            initString.Append(PDFConstant.Space);
            initString.Append(pen.pdfColor.G.ToString("N2"));
            initString.Append(PDFConstant.Space);
            initString.Append(pen.pdfColor.B.ToString("N2"));
            initString.Append(PDFConstant.Space);
            initString.Append(PDFConstant.ColorBorder);
            initString.Append(PDFConstant.Space);
            if (pen.Style == PenStyle.ShortDash)
                initString.Append(PDFConstant.StyleShortDash);
            else if (pen.Style == PenStyle.LongDash)
                initString.Append(PDFConstant.StyleLongDash);
            else
                initString.Append(PDFConstant.StyleRegular);

            initString.Append(PDFConstant.Space);
            initString.Append(PDFConstant.DashStyle);
            initString.Append(PDFConstant.Space);
            initString.Append(pen.Thickness);
            initString.Append(PDFConstant.Space);
            initString.Append(PDFConstant.LineWidth);
            initString.Append(PDFConstant.Space);
            initString.Append(x1);
            initString.Append(PDFConstant.Space);
            initString.Append(Height - y1);
            initString.Append(PDFConstant.Space);
            initString.Append(PDFConstant.LineStart);
            initString.Append(PDFConstant.Space);
            initString.Append(x2);
            initString.Append(PDFConstant.Space);
            initString.Append(Height - y2);
            initString.Append(PDFConstant.Space);
            initString.Append(PDFConstant.LineComma);
            initString.Append(PDFConstant.Space);
            initString.Append(PDFConstant.LineEnd);
            initString.Append(PDFConstant.NewLine);

            pdfBuilder.BuildNewObject(initString);
        }
        public void DrawRectangle(PDFPen pen, float x1, float y1, float width, float height)
        {
            var initString = new StringBuilder();

            initString.Append(pen.pdfColor.R.ToString("N2"));
            initString.Append(PDFConstant.Space);
            initString.Append(pen.pdfColor.G.ToString("N2"));
            initString.Append(PDFConstant.Space);
            initString.Append(pen.pdfColor.B.ToString("N2"));
            initString.Append(PDFConstant.Space);
            initString.Append(PDFConstant.ColorBorder);
            initString.Append(PDFConstant.Space);
            if (pen.Style == PenStyle.ShortDash)
                initString.Append(PDFConstant.StyleShortDash);
            else if (pen.Style == PenStyle.LongDash)
                initString.Append(PDFConstant.StyleLongDash);
            else
                initString.Append(PDFConstant.StyleRegular);

            initString.Append(PDFConstant.Space);
            initString.Append(PDFConstant.DashStyle);
            initString.Append(PDFConstant.Space);
            initString.Append(pen.Thickness);
            initString.Append(PDFConstant.Space);
            initString.Append(PDFConstant.LineWidth);
            initString.Append(PDFConstant.Space);
            initString.Append(x1);
            initString.Append(PDFConstant.Space);
            initString.Append(Height - y1);
            initString.Append(PDFConstant.Space);
            initString.Append(PDFConstant.LineStart);
            initString.Append(PDFConstant.Space);
            initString.Append(x1+width);
            initString.Append(PDFConstant.Space);
            initString.Append(Height - y1);
            initString.Append(PDFConstant.Space);
            initString.Append(PDFConstant.LineComma);
            initString.Append(PDFConstant.Space);
            initString.Append(x1+width);
            initString.Append(PDFConstant.Space);
            initString.Append(Height - (y1+ height));
            initString.Append(PDFConstant.Space);
            initString.Append(PDFConstant.LineComma);
            initString.Append(PDFConstant.Space);
            initString.Append(x1);
            initString.Append(PDFConstant.Space);
            initString.Append(Height - (y1 + height));
            initString.Append(PDFConstant.Space);
            initString.Append(PDFConstant.LineComma);
            initString.Append(PDFConstant.Space);
            initString.Append(x1);
            initString.Append(PDFConstant.Space);
            initString.Append(Height - y1);
            initString.Append(PDFConstant.Space);
            initString.Append(PDFConstant.LineComma);
            initString.Append(PDFConstant.Space);
            initString.Append(PDFConstant.LineEnd);
            initString.Append(PDFConstant.NewLine);

            pdfBuilder.BuildNewObject(initString);
        }
    }
}
