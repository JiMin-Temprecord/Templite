using PDF.Drawing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
namespace PDF
{
    public class PDFBuilder
    {
        //TODO : STORE OBJECT COUNT IN A ARRAY UNTIL PAGE NUMBER CHANGES
        StringBuilder finalPDFString = new StringBuilder();
        StringBuilder objectString = new StringBuilder();

        public string Title { get; set; }
        public int PageHeight { get; set; } = 842;
        public int PageWidth { get; set; } = 595;
        public int PageCount { get; set; } = 0;
        public string FontOne { get; set; } = string.Empty;
        public string FontTwo { get; set; } = string.Empty;

        private List<List<int>> PageObjectList = new List<List<int>>();
        private List<int> objectList = new List<int>();
        private int pageNumber = 1;

        int objectCount = 6;

        public void BuildPDFStart()
        {
            //So it have to be started here.
            finalPDFString.Append(PDFConstant.PDFStart);
            finalPDFString.Append(PDFConstant.NewLine);
            finalPDFString.Append(1);
            finalPDFString.Append(PDFConstant.Space);
            finalPDFString.Append(PDFConstant.ObjectStart);
            finalPDFString.Append(PDFConstant.Space);
            finalPDFString.Append(PDFConstant.ObjectOne);
            finalPDFString.Append(PDFConstant.NewLine);
        }
        public void BuildNewObject(StringBuilder os)
        {
            if (pageNumber != PageCount)
            {
                PageObjectList.Add(objectList);
                pageNumber = PageCount;
                objectList = new List<int>();
            }

            objectCount++;
            //START OBJECT;
            objectString.Append(PDFConstant.Space);
            objectString.Append(PDFConstant.ObjectStart);
            objectString.Append(PDFConstant.Space);
            objectString.Append(PDFConstant.LengthOpen);
            objectString.Append(PDFConstant.Space);
            objectString.Append(os.Length);
            objectString.Append(PDFConstant.Space);
            objectString.Append(PDFConstant.LengthClose);
            objectString.Append(PDFConstant.Space);
            objectString.Append(PDFConstant.StartStream);
            objectString.Append(PDFConstant.NewLine);
            //OBJECT
            objectString.Append(os);
            //END OBJECT
            objectString.Append(PDFConstant.EndStream);
            objectString.Append(PDFConstant.Space);
            objectString.Append(PDFConstant.ObjectEnd);

            if (pageNumber == PageCount)
                objectList.Add(objectCount);
        }
        public async Task BuildPDFEndAsync(string path)
        {
            var finalCount = 1;

            PageObjectList.Add(objectList);

            //OBJECT TWO
            finalCount++;
            finalPDFString.Append(finalCount);
            finalPDFString.Append(PDFConstant.Space);
            finalPDFString.Append(PDFConstant.ObjectStart);
            finalPDFString.Append(PDFConstant.Space);
            finalPDFString.Append(PDFConstant.ObjectTwoMediaBox);
            finalPDFString.Append(PageWidth);
            finalPDFString.Append(PDFConstant.Space);
            finalPDFString.Append(PageHeight);
            finalPDFString.Append(PDFConstant.ObjectTwoCropBox);
            finalPDFString.Append(PageWidth);
            finalPDFString.Append(PDFConstant.Space);
            finalPDFString.Append(PageHeight);
            finalPDFString.Append(PDFConstant.ObjectTwoResources);
            finalPDFString.Append(PDFConstant.Space);
            finalPDFString.Append(PDFConstant.ObjectTwoKids);
            //Object Array
            finalPDFString.Append(PDFConstant.ObjectArrayOpen);
            for (int i = 6; i < 6 + PageCount; i++)
                finalPDFString.Append(i + PDFConstant.ObjectComma);
            finalPDFString.Append(PDFConstant.ObjectArrayClose);
            finalPDFString.Append(PDFConstant.ObjectTwoCount);
            finalPDFString.Append(PDFConstant.Space);
            finalPDFString.Append(PageCount);
            finalPDFString.Append(PDFConstant.ObjectTwoClose);
            finalPDFString.Append(PDFConstant.Space);
            finalPDFString.Append(PDFConstant.ObjectEnd);
            finalPDFString.Append(PDFConstant.NewLine);

            //OBJECT THREE
            finalCount++;
            finalPDFString.Append(finalCount);
            finalPDFString.Append(PDFConstant.Space);
            finalPDFString.Append(PDFConstant.ObjectStart);
            finalPDFString.Append(PDFConstant.Space);
            finalPDFString.Append(PDFConstant.ObjectThree);
            finalPDFString.Append(PDFConstant.ObjectEnd);
            finalPDFString.Append(PDFConstant.NewLine);

            //OBJECT FOUR
            finalCount++;
            finalPDFString.Append(finalCount);
            finalPDFString.Append(PDFConstant.Space);
            finalPDFString.Append(PDFConstant.ObjectStart);
            finalPDFString.Append(PDFConstant.Space);
            finalPDFString.Append(PDFConstant.ObjectFourOpen);
            finalPDFString.Append(FontOne);
            finalPDFString.Append(PDFConstant.ObjectFourClose);
            finalPDFString.Append(PDFConstant.ObjectEnd);
            finalPDFString.Append(PDFConstant.NewLine);

            //OBJECT FIVE
            finalCount++;
            finalPDFString.Append(finalCount);
            finalPDFString.Append(PDFConstant.Space);
            finalPDFString.Append(PDFConstant.ObjectStart);
            finalPDFString.Append(PDFConstant.Space);
            finalPDFString.Append(PDFConstant.ObjectFiveOpen);
            finalPDFString.Append(FontTwo);
            finalPDFString.Append(PDFConstant.ObjectFiveClose);
            finalPDFString.Append(PDFConstant.ObjectEnd);
            finalPDFString.Append(PDFConstant.NewLine);

            //Page Content
            for (int i = 0; i < PageCount; i++)
            {
                finalCount++;
                objectCount++;
                finalPDFString.Append(finalCount);
                finalPDFString.Append(PDFConstant.Space);
                finalPDFString.Append(PDFConstant.ObjectStart);
                finalPDFString.Append(PDFConstant.Space);
                finalPDFString.Append(PDFConstant.PageContentOpen);
                finalPDFString.Append(objectCount+PageCount-1);
                finalPDFString.Append(PDFConstant.Space);
                finalPDFString.Append(PDFConstant.PageContentClose);
                finalPDFString.Append(PDFConstant.ObjectEnd);
                finalPDFString.Append(PDFConstant.NewLine);
            }

            var objectArry = objectString.ToString().Split(new[] { PDFConstant.ObjectEnd },StringSplitOptions.None);
            for(int i = 0; i < objectArry.Length-1; i++)
            {
                //Have to replace number at front
                finalCount++;
                finalPDFString.Append(PDFConstant.NewLine);
                finalPDFString.Append(finalCount + objectArry[i] + PDFConstant.ObjectEnd);
            }
            
            //finalPDFString.Append(objectString);
            finalPDFString.Append(PDFConstant.NewLine);
            //PDF FOOTER
            for (int i = 0; i < PageCount; i++)
            {
                finalCount++;
                finalPDFString.Append(finalCount);
                finalPDFString.Append(PDFConstant.Space);
                finalPDFString.Append(PDFConstant.ObjectStart);
                finalPDFString.Append(PDFConstant.Space);
                finalPDFString.Append(PDFConstant.ObjectArrayOpen);
                //PAGE OBJECT ARRAY
                for (int j = 0; j < PageObjectList[i].Count; j++)
                {
                    finalPDFString.Append(PageObjectList[i][j] + PageCount - 1);
                    finalPDFString.Append(PDFConstant.ObjectComma);
                }
                finalPDFString.Append(PDFConstant.ObjectArrayClose);
                finalPDFString.Append(PDFConstant.Space);
                finalPDFString.Append(PDFConstant.ObjectEnd);
                finalPDFString.Append(PDFConstant.NewLine);
            }
            finalCount++;
            finalPDFString.Append(PDFConstant.Xref);
            finalPDFString.Append(PDFConstant.NewLine);
            finalPDFString.Append("0 " + finalCount);
            finalPDFString.Append(PDFConstant.NewLine);
            finalPDFString.Append(PDFConstant.XrefMainObject);
            finalPDFString.Append(PDFConstant.NewLine);
            for (int i = 0; i < finalCount - 1; i++)
            {
                finalPDFString.Append(PDFConstant.XrefOject);
                finalPDFString.Append(PDFConstant.NewLine);
            }
            finalPDFString.Append(PDFConstant.TrailerTag);
            finalPDFString.Append(PDFConstant.NewLine);
            finalPDFString.Append(PDFConstant.FinalSizeOpen);
            finalPDFString.Append(finalCount);
            finalPDFString.Append(PDFConstant.FinalSizeClose);
            finalPDFString.Append(PDFConstant.NewLine);
            finalPDFString.Append(PDFConstant.PDFEnd);
            
            await Save(finalPDFString.ToString(), path);
        }
        public async Task Save(string content, string path)
        {
            Debug.WriteLine(content);
            var pdfPath = path;
            var pdfContentByte = Encoding.GetEncoding("iso-8859-1").GetBytes(content);
            
            try
            {
                using (var fs = new FileStream(pdfPath, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(pdfContentByte, 0, pdfContentByte.Length);
                    fs.Close();
                }

                //var pdfFile = await StorageFile.GetFileFromPathAsync(pdfPath);
            }
            catch { }
        }
        public string GetFont(PDFFont font)
        {
            string fontTypeandStyle;
            if (font.FontStyle == FontStyle.Regular)
                fontTypeandStyle = font.FontType.ToString();
            else
                fontTypeandStyle = font.FontType + "-" + font.FontStyle;

            if (FontOne == string.Empty)
            {
                FontOne = fontTypeandStyle;
                return PDFConstant.FontOne;
            }

            else
            {
                if (fontTypeandStyle == FontOne)
                    return PDFConstant.FontOne;

                else
                {
                    if (FontTwo == string.Empty)
                    {
                        FontTwo = fontTypeandStyle;
                        return PDFConstant.FontTwo;
                    }
                    else
                    {
                        return PDFConstant.FontTwo;
                    }
                }
            }
        }
    }
}
