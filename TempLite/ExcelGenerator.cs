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
            //Must I re-read this? 
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
            var row = 3;

            excelWorksheet.Cells[1, 5] = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:sss UTC");
            excelWorksheet.Cells[2, 1] = "Logger Report";
            excelWorksheet.Cells[2, 5] = "S/N: " + pdfVariables.SerialNumber;

            FillCells(excelWorksheet, row, "Model : ", loggername);
            FillCells(excelWorksheet, row, "Logger State : ", pdfVariables.LoggerState);
            FillCells(excelWorksheet, row, "Battery : ", pdfVariables.BatteryPercentage + "%");
            FillCells(excelWorksheet, row, "Sample Period : ", pdfVariables.SameplePeriod);
            FillCells(excelWorksheet, row, "Start Delay : ", pdfVariables.StartDelay);
            FillCells(excelWorksheet, row, "First Sample : ", pdfVariables.FirstSample);
            FillCells(excelWorksheet, row, "Last Sample : ", pdfVariables.LoggerState);
            FillCells(excelWorksheet, row, "Recorder Samples : ", pdfVariables.RecordedSamples.ToString());
            FillCells(excelWorksheet, row, "Tags Placed : ", loggername);

        }

        void FillCells (Excel.Worksheet excelWorksheet,int row, string label, string value)
        {
            excelWorksheet.Cells[row, 1] = label;
            excelWorksheet.Cells[row, 2] = value;
            row++;
        }
    }
}
