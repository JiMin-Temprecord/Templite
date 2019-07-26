using PDF.Drawing;

namespace PDF
{
    public class PDFFont
    {
        public FontType FontType { get; set; } = FontType.Helvetica;
        public int FontSize { get; set; } = 11;
        public FontStyle FontStyle { get; set; } = FontStyle.Regular;
        
        public PDFFont (FontType fontType, int fontSize, FontStyle fontStyle)
        {
            FontType = fontType;
            FontSize = fontSize;
            FontStyle = fontStyle;
            /*so there should be a count here of 0 and 1.*
             * 0 is FB
             * 1 is FH 
             * if it is more, thr0w an exception
             * 
             *  for init 0 <</Type/Font/Subtype/Type1/Name/FB/BaseFont/Helvetica>> endobj
             *  for init 1 <</Type/Font/Subtype/Type1/Name/FH/BaseFont/Helvetica-Bold>> endobj
             *   
             *  BT/FB fontSize
             *  BT/FH fontSize
             */
        }
    }
}
