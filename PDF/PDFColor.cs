using System.Drawing;

namespace PDF
{
    public class PDFColor
    {
        private Color Color;
        private float r;
        private float g;
        private float b;

        public PDFColor (Color color)
        {
            Color = color;
            R = color.R;
            G = color.G;
            B = color.B;
        }
        public float R
        {
            get
            {
                return r;
            }
            set
            {
                r = value / 255;
            }
        }
        public float G
        {
            get
            {
                return g;
            }
            set
            {
                g = value/ 255;
            }
        }
        public float B
        {
            get
            {
                return b;
            }
            set
            {
                b = value/ 255;
            }
        }
    }
}
