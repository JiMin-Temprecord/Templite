using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace TempLite
{
    public class ExcelGenerator
    {
        public void CreateExcel(LoggerInformation loggerInformation)
        {
            var excelApp = new Excel.Application();

            Excel.Workbook excelWorkbook;
            Excel.Worksheet excelWorksheet;
            object misValue = System.Reflection.Missing.Value;

            excelWorkbook = excelApp.Workbooks.Add(misValue);
            excelWorksheet = (Excel.Worksheet)excelWorkbook.Worksheets.get_Item(1);
            excelWorksheet.Name = loggerInformation.SerialNumber;

            CreateLayout(excelWorksheet, loggerInformation, loggerInformation.LoggerName);
          
            excelWorksheet.Columns.ColumnWidth = 26;
            excelWorkbook.SaveAs(Application.StartupPath+"\\" +loggerInformation.SerialNumber + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
            excelWorkbook.Close(true, misValue, misValue);
            excelApp.Quit();

            Marshal.ReleaseComObject(excelWorksheet);
            Marshal.ReleaseComObject(excelWorkbook);
            Marshal.ReleaseComObject(excelApp);
        }

        private void CreateLayout(Excel.Worksheet excelWorksheet, LoggerInformation loggerInformation, string loggername)
        {
            var decoder = new HexfileDecoder(loggerInformation);
            decoder.ReadIntoJsonFileAndSetupDecoder();
            var pdfVariables = decoder.AssignPDFValue();
            var channelTwoEnabled = pdfVariables.IsChannelTwoEnabled;
            var channelOne = pdfVariables.ChannelOne;
            var channelTwo = pdfVariables.ChannelTwo;
            var row = 5;

            var imageRange = excelWorksheet.Cells[5,5];
            if (channelOne.OutsideLimits ==0 && channelTwo.OutsideLimits == 0)
            {
                excelWorksheet.Shapes.AddPicture(Application.StartupPath + "\\greentick.png", Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, imageRange.Left+26 , imageRange.Top, 90, 80);
                excelWorksheet.Cells[12, 5].Style.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                excelWorksheet.Cells[12, 5] = "Within Limits";
                excelWorksheet.Cells[12, 5].Font.Bold = true;
                excelWorksheet.Cells[12, 5].Style.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

            }
            else
            {
                excelWorksheet.Shapes.AddPicture(Application.StartupPath +"\\redwarning.png", Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, imageRange.Left+26, imageRange.Top, 90, 80);
                excelWorksheet.Cells[12, 5].Style.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                excelWorksheet.Cells[12, 5] = "Outside Limits";
                excelWorksheet.Cells[12, 5].Font.Bold = true;
                excelWorksheet.Cells[12, 5].Style.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            }

            var logoRange = excelWorksheet.Cells[1, 3];
            excelWorksheet.Shapes.AddPicture(Application.StartupPath + "\\logo.png", Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, logoRange.Left+26, logoRange.Top, 65, 40);

            void FillCells(string label, string value)
            {
                excelWorksheet.Cells[row, 1].Font.Bold = true;
                excelWorksheet.Cells[row, 2 ].Style.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                excelWorksheet.Cells[row, 1] = label;
                excelWorksheet.Cells[row, 2] = value;
                row++;
            }

            void FillChannelCells(string label,Func< ChannelConfig, string> getString)
            {
                excelWorksheet.Cells[row, 1].Font.Bold = true;
                excelWorksheet.Cells[row, 1] = label;
                excelWorksheet.Cells[row, 2].Style.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                excelWorksheet.Cells[row, 2] = getString(channelOne);
                if(channelTwoEnabled)
                {
                    excelWorksheet.Cells[row, 3] = getString(channelTwo);
                }
                row++;
            }

            void FillValueCells(int start, int end)
            {
                var length = channelOne.Data.Count;
                var dataObject = new double[length, 1];
                var timeObject = new string[length, 1];

                for (int i = 0; i < length; i++)
                {
                    timeObject[i, 0] = decoder.UNIXtoUTC(Convert.ToInt32(pdfVariables.Time[i]));
                    dataObject[i, 0] = channelOne.Data[i];
                }
                
                var timeRange = excelWorksheet.Range["A" + start, "A" + (end-1)];
                timeRange.Value2 = timeObject;
                var dataRange = excelWorksheet.Range["B" + start, "B" + (end-1)];
                dataRange.Value = dataObject;
            }
            
            excelWorksheet.Range["B15", "C15"].Font.Bold = true;
            excelWorksheet.Range["B50", "C50"].Font.Bold = true;
            excelWorksheet.Range["A2","E2"].Font.Size = 20;
            excelWorksheet.Range["A2","E2"].Font.Color = Color.Blue;
            excelWorksheet.Range["A2","E2"].Font.Bold = true;
            excelWorksheet.Range["A4", "E4"].Borders[Excel.XlBordersIndex.xlEdgeTop].Color = Color.Blue;
            excelWorksheet.Range["A4", "E4"].Borders[Excel.XlBordersIndex.xlEdgeTop].LineStyle = Excel.XlLineStyle.xlContinuous;
            excelWorksheet.Range["A4", "E4"].Borders[Excel.XlBordersIndex.xlEdgeTop].Weight = 2;

            excelWorksheet.Cells[1, 5] = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:sss UTC");
            excelWorksheet.Cells[2, 1] = "Logger Report";
            excelWorksheet.Cells[2, 5] = "S/N: " + pdfVariables.SerialNumber;
            
            FillCells("Model : ", loggername);
            FillCells("Logger State : ", pdfVariables.LoggerState);
            FillCells("Battery : ", pdfVariables.BatteryPercentage );
            FillCells("Sample Period : ", pdfVariables.SameplePeriod);
            FillCells("Start Delay : ", pdfVariables.StartDelay);
            FillCells("First Sample : ", pdfVariables.FirstSample);
            FillCells("Last Sample : ", pdfVariables.LastSample);
            FillCells("Recorder Samples : ", pdfVariables.RecordedSamples.ToString());
            FillCells("Tags Placed : ", pdfVariables.TagsPlaced);
            row++;
            
            excelWorksheet.Cells[row, 2] = "#1 Temperature";
            if(channelTwoEnabled)
                excelWorksheet.Cells[row, 3] = "#2 Humidity";
            row++;

            FillChannelCells("Preset Upper Limit : ", c => c.PresetUpperLimit.ToString("N2"));
            FillChannelCells("Preset Lower Limit : ", c => c.PresetLowerLimit.ToString("N2"));
            FillChannelCells("Mean Value : ", c => c.Mean.ToString("N2"));
            FillChannelCells("MKT Value :", c => c.MKT_C.ToString("N2"));
            FillChannelCells("Max Recorded :", c => c.Max.ToString("N2"));
            FillChannelCells("Min Recorded :", c => c.Min.ToString("N2"));
            FillChannelCells("Total Samples within Limits :", c => c.WithinLimits.ToString("N1"));
            FillChannelCells("Total Time within Limits :", c => c.TimeWithinLimits);
            FillChannelCells("Total Samples out of Limits :", c => c.OutsideLimits.ToString("N1"));
            FillChannelCells("Total Time out of Limits :", c => c.TimeOutLimits);
            FillChannelCells("Samples above upper Limit :", c => c.AboveLimits.ToString("N1"));
            FillChannelCells("Time above Upper Limit :", c => c.TimeAboveLimits);
            FillChannelCells("Samples below Lower Limit :", c => c.BelowLimits.ToString("N1"));
            FillChannelCells("Time below Lower Limit :", c => c.TimeBelowLimits);

            FillCells("User Comments :", string.Empty);
            FillCells(pdfVariables.UserData, string.Empty);
            
            row = 50;
            FillCells("Date Time", " #1 Temperature");
            row++;

            var startRange = row;
            var endRange = (row + pdfVariables.RecordedSamples);
            FillValueCells(startRange,endRange);
            CreateGraph(excelWorksheet,channelOne,startRange, endRange);
        }

        void CreateGraph(Excel.Worksheet excelWorksheet, ChannelConfig channelOne, int start, int end)
        {
            var xlGraph = excelWorksheet.ChartObjects(Type.Missing);
            var LineGraph = xlGraph.Add(0,475,240,240);
            var chartPage = LineGraph.Chart;
            var xSeries = excelWorksheet.Range["A" + start, "A" + end];
            var ySeries = excelWorksheet.Range["B" + start, "B" + end];

            chartPage.ChartType = Excel.XlChartType.xlLine;
            var seriesCollection = chartPage.SeriesCollection();
            var seriesOne = seriesCollection.NewSeries();
            seriesOne.XValues = xSeries;
            seriesOne.Values = ySeries;
        }
    }
}
