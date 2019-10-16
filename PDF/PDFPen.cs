using PDF.Drawing;
using System.Drawing;

namespace PDF
{
    public class PDFPen
    {
        public PDFColor pdfColor;
        public Color Color { get; set; } = Color.Black;
        public PenStyle Style { get; set; } = PenStyle.Solid;
        public double Thickness { get; set; } = 1;
        public PDFPen(Color color, double thickness, PenStyle style = PenStyle.Solid)
        {
            Color = color;
            Thickness = thickness;
            Style = style;

            pdfColor = new PDFColor(Color);

            //and somehow this have to translate into  0 0 0 RG [] 0 d 1 w thickness
            // color  = 0 0 0 RG color.R + color.G + color.B + RG 
            // style either normal or dashed for now = [] or [3] the number inside the parenthesis represent the length of each each dash
            //thickness will replace number infront of w
        }
    }
}
