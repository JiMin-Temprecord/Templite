namespace PDF
{
    public class PDFConstant
    {
        public const string PDFStart = "%PDF-1.4%";
        public const string ObjectStart = "0 obj"; //charAt(0)
        public const string ObjectEnd = "endobj";
        public const string ObjectComma = " 0 R ";
        public const string ObjectOne = "<</Type/Catalog/Pages 2 0 R >>";
        //split up object to so it makes more sense
        public const string ObjectTwoMediaBox = "<</Type/Pages/MediaBox [0 0 "; // count represent the number of pages, and MediaBox and CropBox represents the size of the Page, which could alsp chan
        public const string ObjectTwoCropBox = "]/CropBox [0 0 "; // count represent the number of pages, and MediaBox and CropBox represents the size of the Page, which could alsp change.
        public const string ObjectTwoResources = "]/Resources 3 0 R"; // count represent the number of pages, and MediaBox and CropBox represents the size of the Page, which could alsp change.
        public const string ObjectTwoKids = "/Kids"; // count represent the number of pages, and MediaBox and CropBox represents the size of the Page, which could alsp change.
        public const string ObjectTwoCount = "/Count"; // count represent the number of pages, and MediaBox and CropBox represents the size of the Page, which could alsp change.
        public const string ObjectTwoClose = ">>"; 
        public const string ObjectThree = "<</Type/Resources/Font<</FH 4 0 R/FB 5 0 R>>>>";
        public const string ObjectFourOpen = "<</Type/Font/Subtype/Type1/Name/FB/BaseFont/"; //add font
        public const string ObjectFourClose = "/Encoding/WinAnsiEncoding >> "; //add font
        public const string ObjectFiveOpen = "<</Type/Font/Subtype/Type1/Name/FH/BaseFont/";  //add font
        public const string ObjectFiveClose = "/Encoding/WinAnsiEncoding>>";  //add font
        public const string PageContentOpen = "<</Type/Page/Parent 2 0 R/Contents "; //Contents is last object before xref
        public const string PageContentClose = "0 R>>";
        public const string LengthOpen = "<</Length "; // find 000 and replace accordingly
        public const string LengthClose = ">>";
        public const string StartStream = "stream";
        public const string EndStream = "endstream ";
        public const string Xref = "xref";
        public const string XrefMainObject = "000000 65535f";
        public const string XrefOject = "0 0n";
        public const string TrailerTag = "trailer";
        public const string FinalSizeOpen = "<</Size "; //find 00, number of documents object
        public const string FinalSizeClose = "/Root 1 0 R >> 0"; //find 00, number of documents object
        public const string BeginText = "BT/";
        public const string FontOne = "FH";
        public const string FontTwo = "FB";
        public const string TextLocation = "Tf";
        public const string TextOpen = "Td (";
        public const string TextClose = ") Tj";
        public const string EndText = "ET";
        public const string ColorInside = "rg";
        public const string ColorBorder = "RG";
        public const string StyleRegular = "[]";
        public const string StyleShortDash = "[3]";
        public const string StyleLongDash = "[10]";
        public const string DashStyle = "0 d";
        public const string LineWidth = "w";
        public const string LineStart = "m";
        public const string LineComma = "l";
        public const string LineEnd = "S";
        public const string ObjectArrayOpen = "[";
        public const string ObjectArrayClose = "]";
        public const string PDFEnd = "%%EOF";
        public const string Space = " ";
        public const string NewLine = "\r\n";
    }
}
