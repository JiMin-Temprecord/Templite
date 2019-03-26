using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace TempLite
{
    public class ExcelGenerator
    {
        int row = 5;

        public void CreateExcel(LoggerInformation loggerInformation)
        {
            var excel = new ExcelPackage();
            var workSheet = excel.Workbook.Worksheets.Add("newSheet");
            
            CreateLayout(workSheet, loggerInformation, loggerInformation.LoggerName);

            excel.SaveAs(new FileInfo(Application.StartupPath + "\\" + loggerInformation.SerialNumber + ".xlsx"));

        }

        private void CreateLayout(ExcelWorksheet workSheet, LoggerInformation loggerInformation, string loggername)
        {
            var decoder = new G4HexDecoder(loggerInformation);
            decoder.ReadIntoJsonFileAndSetupDecoder();
            var pdfVariables = decoder.AssignPDFValue();
            var channelTwoEnabled = pdfVariables.IsChannelTwoEnabled;
            var channelOne = pdfVariables.ChannelOne;
            var channelTwo = pdfVariables.ChannelTwo;

            var imageRange = workSheet.Cells[5, 5];
            if (channelOne.OutsideLimits == 0 && channelTwo.OutsideLimits == 0)
            {
                var tickImage = Image.FromFile(Application.StartupPath + "\\greentick.png");
                workSheet.Drawings.AddPicture("Within Limitis" , tickImage);
                workSheet.Cells[12, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells[12, 5].Value = "Within Limits";
                workSheet.Cells[12, 5].Style.Font.Bold = true;
                workSheet.Cells[12, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            }
            else
            {
                var warningImage = Image.FromFile(Application.StartupPath + "\\redwarning.png");
                workSheet.Drawings.AddPicture("Outside Limits" , warningImage);
                workSheet.Cells[12, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells[12, 5].Value = "Outside Limits";
                workSheet.Cells[12, 5].Style.Font.Bold = true;
                workSheet.Cells[12, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            }

            var logoRange = workSheet.Cells[1, 3];
            var logoImage = Image.FromFile(Application.StartupPath + "\\logo.png");
            workSheet.Drawings.AddPicture(string.Empty, logoImage);

            workSheet.Cells[2,15,3,15].Style.Font.Bold = true;
            workSheet.Cells[2,50,3,50].Style.Font.Bold = true;
            workSheet.Cells[1,2,5,2].Style.Font.Size = 20;
            workSheet.Cells[1,2,5,2].Style.Font.Color.SetColor (Color.Blue);
            workSheet.Cells[1, 2, 5, 2].Style.Font.Bold = true;
            workSheet.Cells[1, 4, 5, 4].Style.Border.Top.Style = ExcelBorderStyle.Double;
            workSheet.Cells[1, 4, 5, 4].Style.Border.Top.Color.SetColor(Color.Blue);

            workSheet.Cells[1, 5].Value = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:sss UTC");
            workSheet.Cells[2, 1].Value = "Logger Report";
            workSheet.Cells[2, 5].Value = "S/N: " + pdfVariables.SerialNumber;

            FillCells(workSheet, "Model : ", loggername);
            FillCells(workSheet, "Logger State : ", pdfVariables.LoggerState);
            FillCells(workSheet, "Battery : ", pdfVariables.BatteryPercentage);
            FillCells(workSheet, "Sample Period : ", pdfVariables.SameplePeriod);
            FillCells(workSheet, "Start Delay : ", pdfVariables.StartDelay);
            FillCells(workSheet, "First Sample : ", pdfVariables.FirstSample);
            FillCells(workSheet, "Last Sample : ", pdfVariables.LastSample);
            FillCells(workSheet, "Recorder Samples : ", pdfVariables.RecordedSamples.ToString());
            FillCells(workSheet, "Tags Placed : ", pdfVariables.TagsPlaced);
            row++;

            workSheet.Cells[row, 2].Value = "#1 Temperature";
            if (channelTwoEnabled)
                workSheet.Cells[row, 3].Value = "#2 Humidity";
            row++;

            FillChannelCells(workSheet, channelOne, channelTwo, channelTwoEnabled, "Preset Upper Limit : ", c => c.PresetUpperLimit.ToString("N2"));
            FillChannelCells(workSheet, channelOne, channelTwo, channelTwoEnabled, "Preset Lower Limit : ", c => c.PresetLowerLimit.ToString("N2"));
            FillChannelCells(workSheet, channelOne, channelTwo, channelTwoEnabled, "Mean Value : ", c => c.Mean.ToString("N2"));
            FillChannelCells(workSheet, channelOne, channelTwo, channelTwoEnabled, "MKT Value :", c => c.MKT_C.ToString("N2"));
            FillChannelCells(workSheet, channelOne, channelTwo, channelTwoEnabled, "Max Recorded :", c => c.Max.ToString("N2"));
            FillChannelCells(workSheet, channelOne, channelTwo, channelTwoEnabled, "Min Recorded :", c => c.Min.ToString("N2"));
            FillChannelCells(workSheet, channelOne, channelTwo, channelTwoEnabled, "Total Samples within Limits :", c => c.WithinLimits.ToString("N1"));
            FillChannelCells(workSheet, channelOne, channelTwo, channelTwoEnabled, "Total Time within Limits :", c => c.TimeWithinLimits);
            FillChannelCells(workSheet, channelOne, channelTwo, channelTwoEnabled, "Total Samples out of Limits :", c => c.OutsideLimits.ToString("N1"));
            FillChannelCells(workSheet, channelOne, channelTwo, channelTwoEnabled, "Total Time out of Limits :", c => c.TimeOutLimits);
            FillChannelCells(workSheet, channelOne, channelTwo, channelTwoEnabled, "Samples above upper Limit :", c => c.AboveLimits.ToString("N1"));
            FillChannelCells(workSheet, channelOne, channelTwo, channelTwoEnabled, "Time above Upper Limit :", c => c.TimeAboveLimits);
            FillChannelCells(workSheet, channelOne, channelTwo, channelTwoEnabled, "Samples below Lower Limit :", c => c.BelowLimits.ToString("N1"));
            FillChannelCells(workSheet, channelOne, channelTwo, channelTwoEnabled, "Time below Lower Limit :", c => c.TimeBelowLimits);

            FillCells(workSheet, "User Comments :", string.Empty);
            FillCells(workSheet, pdfVariables.UserData, string.Empty);

            row = 50;
            FillCells(workSheet, "Date Time", " #1 Temperature");
            row++;

            var startRange = row;
            var endRange = (row + pdfVariables.RecordedSamples);
            FillValueCells(workSheet, decoder, pdfVariables, channelOne, channelTwo, startRange, endRange);
            CreateGraph(workSheet, channelOne, startRange, endRange);
        }

        void FillCells(ExcelWorksheet worksheet, string label, string value)
        {
            worksheet.Cells[row, 1].Style.Font.Bold = true;
            worksheet.Cells[row, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            worksheet.Cells[row, 1].Value = label;
            worksheet.Cells[row, 2].Value = value;
            row++;
        }

        void FillChannelCells(ExcelWorksheet worksheet, ChannelConfig channelOne, ChannelConfig channelTwo, bool channelTwoEnabled, string label, Func<ChannelConfig, string> getString)
        {
            worksheet.Cells[row, 1].Style.Font.Bold = true;
            worksheet.Cells[row, 1].Value = label;
            worksheet.Cells[row, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            worksheet.Cells[row, 2].Value = getString(channelOne);
            if (channelTwoEnabled)
            {
                worksheet.Cells[row, 3].Value = getString(channelTwo);
            }
            row++;
        }

        void FillValueCells(ExcelWorksheet worksheet, G4HexDecoder decoder, PDFvariables pdfVariables, ChannelConfig channelOne, ChannelConfig channelTwo, int start, int end)
        {
            var length = channelOne.Data.Count;
            var dataObject = new double[length, 1];
            var timeObject = new string[length, 1];

            for (int i = 0; i < length; i++)
            {
                timeObject[i, 0] = decoder.UNIXtoUTC(Convert.ToInt32(pdfVariables.Time[i]));
                dataObject[i, 0] = channelOne.Data[i];
            }

            var timeRange = worksheet.Cells[1,start, 1, (end - 1)];
            timeRange.Value = timeObject;
            var dataRange = worksheet.Cells[2 ,start, 2, (end - 1)];
            dataRange.Value = dataObject;
        }

        void CreateGraph(ExcelWorksheet worksheet, ChannelConfig channelOne, int start, int end)
        {
            var graph = worksheet.Drawings.AddChart("newGraph", OfficeOpenXml.Drawing.Chart.eChartType.Line);
            graph.SetPosition(0, 475);
            graph.SetSize(240, 240);

            var xSeries = worksheet.Cells[1,start,1,end];
            var ySeries = worksheet.Cells[2,start,2,end];
            graph.Series.Add(xSeries, ySeries);
        }
    }
}
