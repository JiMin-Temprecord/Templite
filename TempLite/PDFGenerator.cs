using System;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using System.Collections.Generic;

namespace TempLite
{
    public class PDFGenerator
    {
        private PdfDocument pdfDocument = new PdfDocument();
        string fontType = "Roboto";
        int pageNumber = 0;

        public void CreatePDF(LoggerInformation loggerInformation)
        {
            double lineCounter = 80;

            var decoder = new HexFileDecoder(loggerInformation);
            decoder.ReadIntoJsonFileAndSetupDecoder();
            var pdfVariables = decoder.AssignPDFValue();
            var channelTwoEnabled = pdfVariables.IsChannelTwoEnabled;
            var channelOne = pdfVariables.ChannelOne;
            var channelTwo = pdfVariables.ChannelTwo;
            var font = new XFont(fontType, 11, XFontStyle.Regular);
            var boldFont = new XFont(fontType, 11, XFontStyle.Bold);
            
            //create pen
            var pen = new XPen(XColors.Black, 1);
            
            pdfDocument.Info.Title = loggerInformation.SerialNumber;
            var draw = CreateNewPage(font, loggerInformation.SerialNumber);

            void DrawChannelStatistics(string Label, Func<ChannelConfig, string> getString, double lineConterMultiplier = 1.0)
            {
                draw.DrawString(Label, boldFont, XBrushes.Black, PDFcoordinates.first_column, lineCounter);
                draw.DrawString(getString(channelOne) + channelOne.Unit , font, XBrushes.Black, PDFcoordinates.second_column, lineCounter);
                if ((channelTwoEnabled) && Label != "MKT Value :")
                    draw.DrawString(getString(channelTwo) + channelTwo.Unit, font, XBrushes.Black, PDFcoordinates.third_column, lineCounter);

                lineCounter += PDFcoordinates.line_inc * lineConterMultiplier;
            }

            void DrawChannelLimits(string Label, Func<ChannelConfig, string> getString, double lineConterMultiplier = 1.0)
            {
                draw.DrawString(Label, boldFont, XBrushes.Black, PDFcoordinates.first_column, lineCounter);
                draw.DrawString(getString(channelOne), font, XBrushes.Black, PDFcoordinates.second_column, lineCounter);
                if (channelTwoEnabled)
                    draw.DrawString(getString(channelTwo), font, XBrushes.Black, PDFcoordinates.third_column, lineCounter);

                lineCounter += PDFcoordinates.line_inc * lineConterMultiplier;
            }

            void DrawSection(string firstColoumString, string secondColoumString, double lineConterMultiplier = 1.0)
            {
                draw.DrawString(firstColoumString, boldFont, XBrushes.Black, PDFcoordinates.first_column, lineCounter);
                draw.DrawString(secondColoumString, font, XBrushes.Black, PDFcoordinates.second_column, lineCounter);
                lineCounter += PDFcoordinates.line_inc * lineConterMultiplier;
            }

            if ((int)channelOne.OutsideLimits == 0 && (int)channelTwo.OutsideLimits == 0)
            {
                XImage greentick = XImage.FromFile("..\\..\\Images\\greentick.png");
                draw.DrawImage(greentick, PDFcoordinates.sign_left, PDFcoordinates.sign_top, 90, 80);
                draw.DrawString("Within Limits", font, XBrushes.Black, PDFcoordinates.limitinfo_startX, PDFcoordinates.limitinfo_startY);
            }
            else
            {
                XImage redwarning = XImage.FromFile("..\\..\\Images\\redwarning.png");
                draw.DrawImage(redwarning, PDFcoordinates.sign_left, PDFcoordinates.sign_top, 90, 80);
                draw.DrawString("Limits Exceeded", font, XBrushes.Black, PDFcoordinates.limitinfo_startX, PDFcoordinates.limitinfo_startY);
            }
            
            //Draw the boxes
            draw.DrawRectangle(pen, PDFcoordinates.box1_X1, PDFcoordinates.box1_Y1, PDFcoordinates.box1_X2 - PDFcoordinates.box1_X1, PDFcoordinates.box1_Y2 - PDFcoordinates.box1_Y1);
            draw.DrawRectangle(pen, PDFcoordinates.box2_X1, PDFcoordinates.box2_Y1, PDFcoordinates.box2_X2 - PDFcoordinates.box2_X1, PDFcoordinates.box2_Y2 - PDFcoordinates.box2_Y1);
            draw.DrawRectangle(pen, PDFcoordinates.box3_X1, PDFcoordinates.box3_Y1, PDFcoordinates.box3_X2 - PDFcoordinates.box3_X1, PDFcoordinates.box3_Y2 - PDFcoordinates.box3_Y1);

            //Draw the Text
            DrawSection("Model :", loggerInformation.LoggerName);
            DrawSection("Logger State :", pdfVariables.LoggerState);
            DrawSection("Battery :", pdfVariables.BatteryPercentage);
            DrawSection("Sample Period :", pdfVariables.SameplePeriod + " (hh:mm:ss)");
            DrawSection("Start Delay :", pdfVariables.StartDelay + " (hh:mm:ss)");
            DrawSection("First Sample :", pdfVariables.FirstSample);
            DrawSection("Last Sample :", pdfVariables.LastSample);
            DrawSection("Recorded Samples :", pdfVariables.RecordedSamples.ToString());
            DrawSection("Total Trips :", pdfVariables.TotalTrip);
            DrawSection("Tags Placed :", pdfVariables.TagsPlaced);

            lineCounter -= PDFcoordinates.line_inc * 0.75;
            XRect break1 = new XRect(10, lineCounter, 680, 0);
            draw.DrawRectangle(pen, break1);
            lineCounter += PDFcoordinates.line_inc * 0.75;

            draw.DrawString("#1 - Temperature", font, XBrushes.Black, PDFcoordinates.second_column - 25, lineCounter);
            if (channelTwoEnabled) draw.DrawString("#2 - Humidity", font, XBrushes.Black, PDFcoordinates.third_column - 25, lineCounter);
            lineCounter += PDFcoordinates.line_inc;

            DrawChannelStatistics("Preset Upper Limit :", c => c.PresetUpperLimit.ToString("N2")); //need to add the breached
            DrawChannelStatistics("Preset Lower Limit :", c => c.PresetLowerLimit.ToString("N2")); // need to add the breached
            DrawChannelStatistics("Mean Value :", c => c.Mean.ToString("N2"));
            DrawChannelStatistics("MKT Value :", c => c.MKT_C.ToString("N2"));
            DrawChannelStatistics("Max Recorded :", c => c.Max.ToString("N2"));
            DrawChannelStatistics("Min Recorded :", c => c.Min.ToString("N2"));
            lineCounter += (PDFcoordinates.line_inc * 0.5);
            DrawChannelLimits("Total Samples within Limits :", c => c.WithinLimits.ToString("N1"));
            DrawChannelLimits("Total Time within Limits :", c => c.TimeWithinLimits);
            lineCounter += (PDFcoordinates.line_inc * 0.5);
            DrawChannelLimits("Total Samples out of Limits :", c => c.OutsideLimits.ToString("N1"));
            DrawChannelLimits("Total Time out of Limits :", c => c.TimeOutLimits);
            lineCounter += (PDFcoordinates.line_inc * 0.5);
            DrawChannelLimits("Samples above upper Limit :", c => c.AboveLimits.ToString("N1"));
            DrawChannelLimits("Time above Upper Limit :", c => c.TimeAboveLimits);
            lineCounter += (PDFcoordinates.line_inc * 0.5);
            DrawChannelLimits("Samples below Lower Limit :", c => c.BelowLimits.ToString("N1"));
            DrawChannelLimits("Time below Lower Limit :", c => c.TimeBelowLimits);
            DrawSection("User Comments :", string.Empty);

            if (pdfVariables.UserData.Length > 120)
            {
                var firstLine = pdfVariables.UserData.Substring(0, pdfVariables.UserData.Length/2);
                var secondLine = pdfVariables.UserData.Substring(pdfVariables.UserData.Length / 2);

                draw.DrawString(firstLine, font, XBrushes.Black, PDFcoordinates.first_column, lineCounter);
                lineCounter += PDFcoordinates.line_inc;
                draw.DrawString(secondLine, font, XBrushes.Black, PDFcoordinates.first_column, lineCounter);
                lineCounter += PDFcoordinates.line_inc*0.5;
            }
            else
            {
                draw.DrawString(pdfVariables.UserData, font, XBrushes.Black, PDFcoordinates.first_column, lineCounter);
                lineCounter += PDFcoordinates.line_inc;
            }

            XRect break2 = new XRect(10, lineCounter, 680, 0);
            draw.DrawRectangle(pen, break2);
            lineCounter += PDFcoordinates.line_inc * 0.75;

            draw.DrawString("_ Temperature " + channelOne.Unit, font, XBrushes.DarkOliveGreen, PDFcoordinates.second_column, lineCounter);
            if (channelTwoEnabled) draw.DrawString("_ Humidity  " + channelTwo.Unit, font, XBrushes.MediumPurple, PDFcoordinates.second_column + 120, lineCounter);
            lineCounter += PDFcoordinates.line_inc;

            //Draw graph
            DrawGraph(decoder, pdfVariables, draw, pen, font);
            FillInValues(decoder, pdfVariables, loggerInformation.SerialNumber);

            string filename = loggerInformation.SerialNumber + ".pdf";
            pdfDocument.Save(filename);
            //Process.Start(filename); //Previews PDF
            Console.WriteLine("PDF Created !");
        }


        void DrawGraph(HexFileDecoder decoder, PDFvariables pdfVariables, XGraphics draw, XPen pen, XFont font)
        {
            XPen ch0 = new XPen(XColors.DarkGreen);
            XPen ch1 = new XPen(XColors.MediumPurple);
            XPen ch1Limits = new XPen(XColors.Lavender);
            ch1Limits.DashStyle = XDashStyle.Dash;
            XPen withinlimits = new XPen(XColors.ForestGreen);
            withinlimits.DashStyle = XDashStyle.Dash;
            XPen abovelimit = new XPen(XColors.Coral);
            abovelimit.DashStyle = XDashStyle.Dash;
            XPen belowlimit = new XPen(XColors.CornflowerBlue);
            belowlimit.DashStyle = XDashStyle.Dash;

            double[] chUpperLimit = new double[8];
            double[] chLowerLimit = new double[8];
            double[] chUpperYLimit = new double[8];
            double[] chLowerYLimit = new double[8];

            double[] chMax = new double[8];
            double[] chMin = new double[8];
            double[] chYMax = new double[8];
            double[] chYMin = new double[8];
            double graphHighest = 0;
            double graphLowest = 0;

            float temp_next_y = 0;
            float rh_next_y = 0;
            float graph_topX = 55;
            float graph_date_x = 0;
            float graph_y_scale = 0;
            float graph_x_scale = 0;

            int numberofDates = pdfVariables.RecordedSamples / 5;
            int dateGap;
            if (numberofDates <= 0)
            {
                dateGap = 1;
            }
            else
            {
                dateGap = numberofDates;
            }

            chUpperLimit[0] = pdfVariables.ChannelOne.PresetUpperLimit;
            chLowerLimit[0] = pdfVariables.ChannelOne.PresetLowerLimit;
            chMax[0] = pdfVariables.ChannelOne.Max;
            chMin[0] = pdfVariables.ChannelOne.Min;

            graphHighest = pdfVariables.ChannelOne.Max;
            graphLowest = pdfVariables.ChannelOne.Min;

            if (pdfVariables.IsChannelTwoEnabled) //Second Sensor
            {
                chUpperLimit[1] = pdfVariables.ChannelTwo.PresetUpperLimit;
                chLowerLimit[1] = pdfVariables.ChannelTwo.PresetLowerLimit;
                chMax[1] = pdfVariables.ChannelTwo.Max;
                chMin[1] = pdfVariables.ChannelTwo.Min;

                if (pdfVariables.ChannelTwo.Max > graphHighest)
                    graphHighest = pdfVariables.ChannelTwo.Max;

                if(pdfVariables.ChannelTwo.Min < graphLowest)
                    graphLowest = pdfVariables.ChannelTwo.Min;
            }
            
            //draw graph
            draw.DrawLine(pen, PDFcoordinates.G_axis_startX, PDFcoordinates.G_axis_startY, PDFcoordinates.G_axis_meetX, PDFcoordinates.G_axis_meetY);
            draw.DrawLine(pen, PDFcoordinates.G_axis_meetX, PDFcoordinates.G_axis_meetY, PDFcoordinates.G_axis_endX, PDFcoordinates.G_axis_endY);
            graph_y_scale = (float)((PDFcoordinates.graph_H - 20) / (graphHighest - graphLowest));
            graph_x_scale = (float)PDFcoordinates.graph_W / pdfVariables.RecordedSamples;

            while (numberofDates < pdfVariables.RecordedSamples)
            {
                graph_date_x = (graph_x_scale * numberofDates) + graph_topX;
                draw.DrawString(decoder.UNIXtoUTCDate(Convert.ToInt32(pdfVariables.Time[numberofDates])), font, XBrushes.Black, graph_date_x - 40, PDFcoordinates.G_axis_meetY + 15);
                draw.DrawString(decoder.UNIXtoUTCTime(Convert.ToInt32(pdfVariables.Time[numberofDates])), font, XBrushes.Black, graph_date_x - 45, PDFcoordinates.G_axis_meetY + 28);
                numberofDates += dateGap;
            }

            if (pdfVariables.IsChannelTwoEnabled && pdfVariables.RecordedSamples > 0)
            {
                rh_next_y = (float)(PDFcoordinates.graph_H - (pdfVariables.ChannelTwo.Data[0] - graphLowest) * graph_y_scale) + PDFcoordinates.graph_topY;
                chUpperYLimit[1] = (float)(PDFcoordinates.graph_H - ((chUpperLimit[1] - graphLowest) * graph_y_scale)) + PDFcoordinates.graph_topY;
                chLowerYLimit[1] = (float)(PDFcoordinates.graph_H - ((chLowerLimit[1] - graphLowest) * graph_y_scale)) + PDFcoordinates.graph_topY;
                chYMax[1] = (float)(PDFcoordinates.graph_H - ((pdfVariables.ChannelTwo.Max - graphLowest) * graph_y_scale)) + PDFcoordinates.graph_topY;
                chYMin[1] = (float)(PDFcoordinates.graph_H - ((pdfVariables.ChannelTwo.Min - graphLowest) * graph_y_scale)) + PDFcoordinates.graph_topY;

                draw.DrawLine(ch1Limits, PDFcoordinates.graph_l_lineX_start, chYMax[1], PDFcoordinates.graph_l_lineX_end, chYMax[1]);
                draw.DrawLine(ch1Limits, PDFcoordinates.graph_l_lineX_start, chYMin[1], PDFcoordinates.graph_l_lineX_end, chYMin[1]);
                draw.DrawString(chMin[1].ToString("N2"), font, XBrushes.Black, PDFcoordinates.first_column, chYMin[1]);
                draw.DrawString(chMax[1].ToString("N2"), font, XBrushes.Black, PDFcoordinates.first_column, chYMax[1]);

                if (chLowerLimit[1] < chMin[1])
                {
                    draw.DrawString(pdfVariables.ChannelTwo.Unit + " Upper Limit ", font, XBrushes.Coral, PDFcoordinates.third_column, chUpperYLimit[1] - 5);
                    draw.DrawString(chUpperLimit[1].ToString("N2"), font, XBrushes.Black, PDFcoordinates.first_column, chUpperYLimit[1]);
                    draw.DrawLine(abovelimit, PDFcoordinates.graph_l_lineX_start, chUpperYLimit[1], PDFcoordinates.graph_l_lineX_end, chUpperYLimit[1]);
                }

                if (chUpperLimit[1] > chMax[1])
                {
                    draw.DrawString(pdfVariables.ChannelTwo.Unit + " Lower Limit ", font, XBrushes.CornflowerBlue, PDFcoordinates.third_column, chLowerYLimit[1] + 5); 
                    draw.DrawString(chLowerLimit[1].ToString("N2"), font, XBrushes.Black, PDFcoordinates.first_column, chLowerYLimit[1]);
                    draw.DrawLine(belowlimit, PDFcoordinates.graph_l_lineX_start, chLowerYLimit[1], PDFcoordinates.graph_l_lineX_end, chLowerYLimit[1]);
                }
            }


            if (pdfVariables.ChannelOne.Data != null)
            {
                temp_next_y = (float)(PDFcoordinates.graph_H - (pdfVariables.ChannelOne.Data[0] -graphLowest) * graph_y_scale) + PDFcoordinates.graph_topY;
                chUpperYLimit[0] = (float)(PDFcoordinates.graph_H - ((chUpperLimit[0] - graphLowest) * graph_y_scale)) + PDFcoordinates.graph_topY;
                chLowerYLimit[0] = (float)(PDFcoordinates.graph_H - ((chLowerLimit[0] - graphLowest) * graph_y_scale)) + PDFcoordinates.graph_topY;
                chYMax[0] = (float)(PDFcoordinates.graph_H - ((pdfVariables.ChannelOne.Max - graphLowest) * graph_y_scale)) + PDFcoordinates.graph_topY;
                chYMin[0] = (float)(PDFcoordinates.graph_H - ((pdfVariables.ChannelOne.Min - graphLowest) * graph_y_scale)) + PDFcoordinates.graph_topY;

                draw.DrawLine(withinlimits, PDFcoordinates.graph_l_lineX_start, chYMax[0], PDFcoordinates.graph_l_lineX_end, chYMax[0]);
                draw.DrawLine(withinlimits, PDFcoordinates.graph_l_lineX_start, chYMin[0], PDFcoordinates.graph_l_lineX_end, chYMin[0]);
                draw.DrawString(chMin[0].ToString("N2"), font, XBrushes.Black, PDFcoordinates.first_column, chYMin[0]);
                draw.DrawString(chMax[0].ToString("N2"), font, XBrushes.Black, PDFcoordinates.first_column, chYMax[0]);

                if (chUpperLimit[0] < chMax[0])
                {
                    draw.DrawString(pdfVariables.ChannelOne.Unit + " Upper Limit ", font, XBrushes.Coral, PDFcoordinates.third_column, chUpperYLimit[0] - 5);
                    draw.DrawString(chUpperLimit[0].ToString("N2"), font, XBrushes.Black, PDFcoordinates.first_column, chUpperYLimit[0]);
                    draw.DrawLine(abovelimit, PDFcoordinates.graph_l_lineX_start, chUpperYLimit[0], PDFcoordinates.graph_l_lineX_end, chUpperYLimit[0]);
                }

                if (chLowerLimit[0] > chMin[0])
                {
                    draw.DrawString(pdfVariables.ChannelOne.Unit + " Lower Limit ", font, XBrushes.CornflowerBlue, PDFcoordinates.third_column, chLowerYLimit[0] + 5);
                    draw.DrawString(chLowerLimit[0].ToString("N2"), font, XBrushes.Black, PDFcoordinates.first_column, chLowerYLimit[0]);
                    draw.DrawLine(belowlimit, PDFcoordinates.graph_l_lineX_start, chLowerYLimit[0], PDFcoordinates.graph_l_lineX_end, chLowerYLimit[0]);
                }
            }

            int i = 0;
            while (i < pdfVariables.RecordedSamples && (pdfVariables.ChannelOne.Data != null))
            {
                draw.DrawLine(ch0, graph_topX, temp_next_y, graph_topX + graph_x_scale, (float)(PDFcoordinates.graph_H - ((pdfVariables.ChannelOne.Data[i] - (graphLowest)) * graph_y_scale)) + PDFcoordinates.graph_topY);
                temp_next_y = (float)(PDFcoordinates.graph_H - ((pdfVariables.ChannelOne.Data[i] - (graphLowest)) * graph_y_scale)) + PDFcoordinates.graph_topY;

                if (pdfVariables.IsChannelTwoEnabled)
                {
                    draw.DrawLine(ch1, graph_topX, rh_next_y, graph_topX + graph_x_scale, (float)(PDFcoordinates.graph_H - ((pdfVariables.ChannelTwo.Data[i] - (graphLowest)) * graph_y_scale)) + PDFcoordinates.graph_topY);
                    rh_next_y = (float)(PDFcoordinates.graph_H - ((pdfVariables.ChannelTwo.Data[i] - (graphLowest)) * graph_y_scale)) + PDFcoordinates.graph_topY;
                }

                graph_topX += graph_x_scale;
                i++;
            }
        }

        void FillInValues(HexFileDecoder decoder, PDFvariables pdfVariables, String serialNumber)
        {
            var dateColumn = 20;
            var timeColumn = 30;

            var columnStart = 85;
            var currentColumn = columnStart;
            var maxColumnValue = 650;
            var columnIncrement = 45;

            var rowStart = 65;
            var row = rowStart;
            var rowIncrement = 16;

            var font = new XFont("Roboto", 10, XFontStyle.Regular);
            var boldFont = new XFont("Roboto", 10, XFontStyle.Bold);
            var tempPen = new XPen(XColors.Black, 1);
            var humPen = new XPen(XColors.Gray, 1);

            var time = new List<string>();
            var date = new List<string>();

            var valueDraw = CreateNewPage(font, serialNumber);
            
            for (int i = 0; i < pdfVariables.Time.Count; i++)
            {
                time.Add(decoder.UNIXtoUTCTime(Convert.ToInt32(pdfVariables.Time[i])));
                date.Add(decoder.UNIXtoUTCDate(Convert.ToInt32(pdfVariables.Time[i])));
            }

            for (int i = 0; i < pdfVariables.RecordedSamples; i++)
            {

                if ((i == 0) || (date[i - 1] != date[i]))
                {
                    if (pdfVariables.IsChannelTwoEnabled && i != 0)
                        row = row + rowIncrement * 2;
                    else
                        row += rowIncrement;

                    valueDraw.DrawString(date[i], boldFont, XBrushes.Black, dateColumn, row);
                    row += rowIncrement;
                    valueDraw.DrawString(time[i], boldFont, XBrushes.Black, timeColumn, row);
                    currentColumn = columnStart;
                }

                if ((currentColumn > maxColumnValue))
                {
                    currentColumn = columnStart;

                    if (pdfVariables.IsChannelTwoEnabled)
                        row = row + rowIncrement * 2;
                    else
                        row += rowIncrement;

                    if (row > 970)
                    {
                        valueDraw.Dispose();
                        valueDraw = CreateNewPage(font, serialNumber);
                        row = rowStart + rowIncrement;
                        currentColumn = columnStart;
                    }

                    valueDraw.DrawString(time[i], boldFont, XBrushes.Black, timeColumn, row);
                }

                valueDraw.DrawString(pdfVariables.ChannelOne.Data[i].ToString("N2")+ pdfVariables.ChannelOne.Unit, font, XBrushes.Black, currentColumn, row);

                if (pdfVariables.IsChannelTwoEnabled)
                {
                    valueDraw.DrawString(pdfVariables.ChannelTwo.Data[i].ToString("N2") + pdfVariables.ChannelTwo.Unit , font, XBrushes.Gray, currentColumn, row + rowIncrement);
                }

                currentColumn += columnIncrement;
            }
        }

        XGraphics CreateNewPage (XFont font, string serialNumber)
        {
            var serialfont = new XFont(fontType, 18, XFontStyle.Regular);
            var serialPen = new XPen(XColors.Blue, 3);
            var logo = XImage.FromFile("..\\..\\Images\\logo.png");

            pageNumber++;

            var page = pdfDocument.AddPage();
            page.Height = 1000;
            page.Width = 700;
            
            var draw = XGraphics.FromPdfPage(page);
            draw.DrawString("Logger Report", serialfont, XBrushes.Blue, 10, 50);
            draw.DrawString("S/N: " + serialNumber, serialfont, XBrushes.Blue, 550, 50);
            draw.DrawLine(serialPen, 10, 60, 690, 60);
            draw.DrawString("Page " + pageNumber , font, XBrushes.Black, 600, 980);
            draw.DrawString("www.temprecord.com", font, XBrushes.Black, PDFcoordinates.siteX, PDFcoordinates.siteY);
            draw.DrawString(DateTime.UtcNow.ToString("dd/MM/yyy HH:mm:sss UTC"), font, XBrushes.Black, PDFcoordinates.dateX, PDFcoordinates.dateY);
            draw.DrawString("0.1.9.1", font, XBrushes.Black, PDFcoordinates.versionX, PDFcoordinates.versionY);
            draw.DrawImage(logo, 320, 10, 65, 40);

            return draw;
        }
    }
}
