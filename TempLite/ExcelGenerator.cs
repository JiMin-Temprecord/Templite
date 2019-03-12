using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;
using TempLite.Services;
using System;

namespace TempLite
{
    public class ExcelGenerator
    {
        public void CreateExcel(LoggerInformation loggerInformation)
        {
            var decoder = new HexfileDecoder(loggerInformation);
            decoder.ReadIntoJsonFileAndSetupDecoder();
            var pdfVariables = decoder.AssignPDFValue();

            var excelApp = new Excel.Application();

            Excel.Workbook excelWorkbook;
            Excel.Worksheet excelWorksheet;
            object misValue = System.Reflection.Missing.Value;

            excelWorkbook = excelApp.Workbooks.Add(misValue);
            excelWorksheet = (Excel.Worksheet)excelWorkbook.Worksheets.get_Item(1);
            excelWorksheet.Name = loggerInformation.SerialNumber;

            CreateLayout(excelWorksheet, pdfVariables, loggerInformation.LoggerName);

            excelWorkbook.SaveAs(loggerInformation.SerialNumber + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
            excelWorkbook.Close(true, misValue, misValue);
            excelApp.Quit();

            Marshal.ReleaseComObject(excelWorksheet);
            Marshal.ReleaseComObject(excelWorkbook);
            Marshal.ReleaseComObject(excelApp);
        }

        private void CreateLayout(Excel.Worksheet excelWorksheet, PDFvariables pdfVariables, string loggername)
        {
            excelWorksheet.Cells[1, 5] = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:sss UTC");
            excelWorksheet.Cells[2, 1] = "Logger Report";
            excelWorksheet.Cells[2, 5] = "S/N: " + pdfVariables.SerialNumber;

            excelWorksheet.Cells[3, 1] = "Model : ";
            excelWorksheet.Cells[3, 2] = loggername;

            excelWorksheet.Cells[4, 1] = "Logger State : ";
            excelWorksheet.Cells[4, 2] = pdfVariables.LoggerState;

            excelWorksheet.Cells[5, 1] = "Battery : ";
            excelWorksheet.Cells[5, 2] = pdfVariables.BatteryPercentage + "%";

            excelWorksheet.Cells[6, 1] = "Sample Period : ";
            excelWorksheet.Cells[6, 2] = pdfVariables.SameplePeriod;

            excelWorksheet.Cells[7, 1] = "Start Delay : ";
            excelWorksheet.Cells[7, 2] = pdfVariables.StartDelay;

            excelWorksheet.Cells[8, 1] = "First Sample : ";
            excelWorksheet.Cells[8, 2] = pdfVariables.FirstSample;

            excelWorksheet.Cells[9, 1] = "Last Sample : ";
            excelWorksheet.Cells[9, 2] = pdfVariables.LoggerState;

            excelWorksheet.Cells[10, 1] = "Recorder Samples : ";
            excelWorksheet.Cells[10, 2] = pdfVariables.RecordedSamples;

            excelWorksheet.Cells[11, 1] = "Tags Placed : ";
            excelWorksheet.Cells[11, 2] = pdfVariables.TagsPlaced;

        }
    }
}
