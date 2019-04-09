using System;
using System.Drawing;
using System.IO;
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
            var workSheet = excel.Workbook.Worksheets.Add(loggerInformation.SerialNumber);
            CreateLayout(workSheet, loggerInformation, loggerInformation.LoggerName);
            
            excel.SaveAs(new FileInfo(Path.GetTempPath() + "\\" + loggerInformation.SerialNumber + ".xlsx"));
            excel.SaveAs(new FileInfo(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + loggerInformation.SerialNumber + ".xlsx"));
            Console.WriteLine("EXCEL Created !");

        }

        private void CreateLayout(ExcelWorksheet workSheet, LoggerInformation loggerInformation, string loggername)
        {
            var decoder = new HexFileDecoder(loggerInformation);
            decoder.ReadIntoJsonFileAndSetupDecoder();
            var pdfVariables = decoder.AssignPDFValue();
            var channelTwoEnabled = pdfVariables.IsChannelTwoEnabled;
            var channelOne = pdfVariables.ChannelOne;
            var channelTwo = pdfVariables.ChannelTwo;
            var path = AppDomain.CurrentDomain.BaseDirectory;

            var imageRange = workSheet.Cells[5, 5];
            if (channelOne.OutsideLimits == 0 && channelTwo.OutsideLimits == 0)
            {
                var tickImage = Image.FromFile(path + "\\Images\\greentick.png");
                var setPosition = workSheet.Drawings.AddPicture("Within Limitis" , tickImage);
                setPosition.SetSize(145, 128);
                setPosition.SetPosition(80, 275);
                workSheet.Cells[12, 5].Value = "Within Limits";
                workSheet.Cells[12, 5].Style.Font.Bold = true;
                workSheet.Cells[12, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            }
            else
            {
                var warningImage = Image.FromFile(path + "\\Images\\redwarning.png");
                var setPosition = workSheet.Drawings.AddPicture("Outside Limits" , warningImage);
                setPosition.SetSize(145, 128);
                setPosition.SetPosition(80, 275);
                workSheet.Cells[12, 5].Value = "Outside Limits";
                workSheet.Cells[12, 5].Style.Font.Bold = true;
                workSheet.Cells[12, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            }

            var logoRange = workSheet.Cells[1, 3];
            var logoImage = Image.FromFile(path + "\\Images\\logo.png");
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

            if (pdfVariables.UserData.Length > 120)
            {
                var firstLine = pdfVariables.UserData.Substring(0, pdfVariables.UserData.Length / 2);
                var secondLine = pdfVariables.UserData.Substring(pdfVariables.UserData.Length / 2);

                FillCells(workSheet, firstLine, string.Empty);
                FillCells(workSheet, secondLine, string.Empty);
            }

            row = 50;
            if (channelTwoEnabled)
                workSheet.Cells[row, 3].Value = "#2 Humidity";
            FillCells(workSheet, "Date Time", " #1 Temperature");
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

        void FillValueCells(ExcelWorksheet worksheet, HexFileDecoder decoder, PDFvariables pdfVariables, ChannelConfig channelOne, ChannelConfig channelTwo, int start, int end)
        {
            for (int i = 0; i < pdfVariables.RecordedSamples; i++)
            {
                worksheet.Cells[start+i,1].Value = decoder.UNIXtoUTC(Convert.ToInt32(pdfVariables.Time[i]));
                worksheet.Cells[start +i,2].Value = channelOne.Data[i];

                if (pdfVariables.IsChannelTwoEnabled == true)
                    worksheet.Cells[start + i, 3].Value = channelTwo.Data[i];
            }
            
        }

        void CreateGraph(ExcelWorksheet worksheet, PDFvariables pdfVariables, ChannelConfig channelOne, ChannelConfig channelTwo, int start, int end)
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
                graph.Series[1].Header = "Humditiy"; // change to channelTwo.sensorName
            }

            graph.Series[0].Header = "Temperature"; // change to channelOne.sensorName
        }
    }
}
