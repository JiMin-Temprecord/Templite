using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Drawing;
using System.IO;
using TempLite.Constant;

namespace TempLite
{
    public class ExcelGenerator
    {
        int row = 5;

        public void CreateExcel(LoggerInformation loggerInformation)
        {
            var decoder = new HexFileDecoder(loggerInformation);
            decoder.ReadIntoJsonFileAndSetupDecoder();
            var loggerVariables = decoder.AssignLoggerValue();

            var excelPath = Path.GetTempPath() + "\\" + loggerInformation.SerialNumber + ".xlsx";
            var excel = new ExcelPackage();
            var workSheet = excel.Workbook.Worksheets.Add(loggerInformation.SerialNumber);
            CreateLayout(workSheet, loggerInformation, loggerVariables);
            
            excel.SaveAs(new FileInfo(excelPath));
            //Console.WriteLine("EXCEL Created !");
        }

        private void CreateLayout(ExcelWorksheet workSheet, LoggerInformation loggerInformation, LoggerVariables pdfVariables)
        {
            var decoder = new HexFileDecoder(loggerInformation);
            var channelTwoEnabled = pdfVariables.IsChannelTwoEnabled;
            var channelOne = pdfVariables.ChannelOne;
            var channelTwo = pdfVariables.ChannelTwo;
            var path = AppDomain.CurrentDomain.BaseDirectory + "\\Images\\";

            var imageRange = workSheet.Cells[5, 5];
            if (channelOne.OutsideLimits == 0 && channelTwo.OutsideLimits == 0)
            {
                var tickImage = Image.FromFile(path + LabelConstant.WithinLimitImage);
                var setPosition = workSheet.Drawings.AddPicture(LabelConstant.WithinLimit, tickImage);
                setPosition.SetSize(145, 128);
                setPosition.SetPosition(80, 275);
                workSheet.Cells[12, 5].Value = LabelConstant.WithinLimit;
                workSheet.Cells[12, 5].Style.Font.Bold = true;
                workSheet.Cells[12, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            }
            else
            {
                var warningImage = Image.FromFile(path + LabelConstant.LimitsExceededImage);
                var setPosition = workSheet.Drawings.AddPicture(LabelConstant.LimitsExceeded, warningImage);
                setPosition.SetSize(145, 128);
                setPosition.SetPosition(80, 275);
                workSheet.Cells[12, 5].Value = LabelConstant.LimitsExceeded;
                workSheet.Cells[12, 5].Style.Font.Bold = true;
                workSheet.Cells[12, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            }

            var logoRange = workSheet.Cells[1, 3];
            var logoImage = Image.FromFile(path + LabelConstant.LogoIcon);
            var setLogoPosition = workSheet.Drawings.AddPicture(string.Empty, logoImage);
            setLogoPosition.SetSize(103, 63);
            setLogoPosition.SetPosition(10, 130);

            workSheet.Cells[15,2,15,3].Style.Font.Bold = true;
            workSheet.Cells[50,2,50,3].Style.Font.Bold = true;
            workSheet.Cells[2,1,2,5].Style.Font.Size = 20;
            workSheet.Cells[2,1,2,5].Style.Font.Color.SetColor (Color.Blue);
            workSheet.Cells[2,1 ,5,2].Style.Font.Bold = true;
            workSheet.Cells[4,1,4,5].Style.Border.Top.Style = ExcelBorderStyle.Double;
            workSheet.Cells[4,1,4,5].Style.Border.Top.Color.SetColor(Color.Blue);

            workSheet.Cells[1, 5].Value = DateTime.UtcNow.ToString("dd/MM/yyyy HH:mm:sss UTC");
            workSheet.Cells[2, 1].Value = LabelConstant.Title;
            workSheet.Cells[2, 5].Value = LabelConstant.SerialNumber + pdfVariables.SerialNumber;

            FillCells(workSheet, LabelConstant.Model, loggerInformation.LoggerName);
            FillCells(workSheet, LabelConstant.LoggerState, pdfVariables.LoggerState);
            FillCells(workSheet, LabelConstant.Battery, pdfVariables.BatteryPercentage);
            FillCells(workSheet, LabelConstant.SamplePeriod, pdfVariables.SameplePeriod);
            FillCells(workSheet, LabelConstant.StartDelay, pdfVariables.StartDelay);
            FillCells(workSheet, LabelConstant.FirstSample, pdfVariables.FirstSample);
            FillCells(workSheet, LabelConstant.LastSample, pdfVariables.LastSample);
            FillCells(workSheet, LabelConstant.RecordedSample, pdfVariables.RecordedSamples.ToString());
            FillCells(workSheet, LabelConstant.TagsPlaced, pdfVariables.TagsPlaced.ToString());
            row++;

            FillCells(workSheet, LabelConstant.Channel, LabelConstant.ChannelOneLabel);
            if (channelTwoEnabled)workSheet.Cells[row, 3].Value = LabelConstant.ChannelTwoLabel;
            row++;

            FillChannelStatCells(workSheet, channelOne, channelTwo, channelTwoEnabled, LabelConstant.PresentUpperLimit, c => c.PresetUpperLimit.ToString("N2"));
            FillChannelStatCells(workSheet, channelOne, channelTwo, channelTwoEnabled, LabelConstant.PresentLowerLimit, c => c.PresetLowerLimit.ToString("N2"));
            FillChannelStatCells(workSheet, channelOne, channelTwo, channelTwoEnabled, LabelConstant.Mean, c => c.Mean.ToString("N2"));
            FillChannelStatCells(workSheet, channelOne, channelTwo, channelTwoEnabled, LabelConstant.MKT, c => c.MKT_C.ToString("N2"));
            FillChannelStatCells(workSheet, channelOne, channelTwo, channelTwoEnabled, LabelConstant.Max, c => c.Max.ToString("N2"));
            FillChannelStatCells(workSheet, channelOne, channelTwo, channelTwoEnabled, LabelConstant.Min, c => c.Min.ToString("N2"));
            FillChannelCells(workSheet, channelOne, channelTwo, channelTwoEnabled, LabelConstant.SampleWithinLimits, c => c.WithinLimits.ToString("N1"));
            FillChannelCells(workSheet, channelOne, channelTwo, channelTwoEnabled, LabelConstant.TimeWithinLimits, c => c.TimeWithinLimits);
            FillChannelCells(workSheet, channelOne, channelTwo, channelTwoEnabled, LabelConstant.SampleOutofLimits, c => c.OutsideLimits.ToString("N1"));
            FillChannelCells(workSheet, channelOne, channelTwo, channelTwoEnabled, LabelConstant.TimeOutOfLimits, c => c.TimeOutLimits);
            FillChannelCells(workSheet, channelOne, channelTwo, channelTwoEnabled, LabelConstant.SampleAboveLimit, c => c.AboveLimits.ToString("N1"));
            FillChannelCells(workSheet, channelOne, channelTwo, channelTwoEnabled, LabelConstant.TimeAboveLimit, c => c.TimeAboveLimits);
            FillChannelCells(workSheet, channelOne, channelTwo, channelTwoEnabled, LabelConstant.SampleBelowLimit, c => c.BelowLimits.ToString("N1"));
            FillChannelCells(workSheet, channelOne, channelTwo, channelTwoEnabled, LabelConstant.TimeBelowLimit, c => c.TimeBelowLimits);

            FillCells(workSheet, LabelConstant.UserComment, string.Empty);

            if (pdfVariables.UserData.Length > 120)
            {
                var firstLine = pdfVariables.UserData.Substring(0, pdfVariables.UserData.Length / 2);
                var secondLine = pdfVariables.UserData.Substring(pdfVariables.UserData.Length / 2);

                FillCells(workSheet, firstLine, string.Empty);
                FillCells(workSheet, secondLine, string.Empty);
            }
            else
            {
                FillCells(workSheet, pdfVariables.UserData, string.Empty);
            }

            row = 50;
            if (channelTwoEnabled)workSheet.Cells[row, 3].Value = LabelConstant.ChannelTwoLabel;
            FillCells(workSheet, LabelConstant.DateTime, LabelConstant.ChannelOneLabel);
            row++;

            var startRange = row;
            var endRange = (row + pdfVariables.RecordedSamples);
            FillValueCells(workSheet, decoder, pdfVariables, channelOne, channelTwo, startRange, endRange);
            CreateGraph(workSheet, pdfVariables, channelOne, channelTwo, startRange, endRange);
            workSheet.Cells[workSheet.Dimension.Address].AutoFitColumns();
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

        void FillChannelStatCells(ExcelWorksheet worksheet, ChannelConfig channelOne, ChannelConfig channelTwo, bool channelTwoEnabled, string label, Func<ChannelConfig, string> getString)
        {
            worksheet.Cells[row, 1].Style.Font.Bold = true;
            worksheet.Cells[row, 1].Value = label;
            worksheet.Cells[row, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            worksheet.Cells[row, 2].Value = getString(channelOne) + channelOne.Unit;
            if (channelTwoEnabled && label!=LabelConstant.MKT)
            {
                worksheet.Cells[row, 3].Value = getString(channelTwo) + channelTwo.Unit;
            }
            row++;
        }

        void FillValueCells(ExcelWorksheet worksheet, HexFileDecoder decoder, LoggerVariables pdfVariables, ChannelConfig channelOne, ChannelConfig channelTwo, int start, int end)
        {
            for (int i = 0; i < pdfVariables.RecordedSamples; i++)
            {
                worksheet.Cells[start+i,1].Value = decoder.UNIXtoUTC(Convert.ToInt32(pdfVariables.Time[i]));
                worksheet.Cells[start +i,2].Value = channelOne.Data[i];

                if (pdfVariables.IsChannelTwoEnabled == true)
                    worksheet.Cells[start + i, 3].Value = channelTwo.Data[i];
            }
            
        }

        void CreateGraph(ExcelWorksheet worksheet, LoggerVariables pdfVariables, ChannelConfig channelOne, ChannelConfig channelTwo, int start, int end)
        {
            var graph = worksheet.Drawings.AddChart(pdfVariables.SerialNumber, OfficeOpenXml.Drawing.Chart.eChartType.Line);
            graph.SetPosition(675,0);
            graph.SetSize(500, 300);

            var xSeries = worksheet.Cells[start,1,end-1,1];
            var ySeries = worksheet.Cells[start,2,end-1,2];
            graph.Series.Add(ySeries,xSeries);
 
            if (pdfVariables.IsChannelTwoEnabled)
            {
                var ySeries2 = worksheet.Cells[start, 3, end - 1, 3];
                graph.Series.Add(ySeries2, xSeries);
                graph.Series[1].Header = LabelConstant.ChannelTwoLabel + channelTwo.Unit;
            }

            graph.Series[0].Header = LabelConstant.ChannelOneLabel + channelOne.Unit;
        }
    }
}
