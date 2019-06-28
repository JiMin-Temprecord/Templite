using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.IO;
using TempLite.Constant;

namespace TempLite
{
    public class PDFGenerator
    {
        PdfDocument pdfDocument = new PdfDocument();

        readonly string fontType = "Roboto";
        readonly string path = AppDomain.CurrentDomain.BaseDirectory + "\\Images\\";
        int pageNumber = 0;
        
        public bool CreatePDF(LoggerInformation loggerInformation)
        {
            var decoder = new HexFileDecoder(loggerInformation);
            decoder.ReadIntoJsonFileAndSetupDecoder();
            var loggerVariables = decoder.AssignLoggerValue();

            double lineCounter = 80;
            var channelTwoEnabled = loggerVariables.IsChannelTwoEnabled;
            var channelOne = loggerVariables.ChannelOne;
            var channelTwo = loggerVariables.ChannelTwo;

            var pen = new XPen(XColors.Black, 1);
            var font = new XFont(fontType, 11, XFontStyle.Regular);
            var boldFont = new XFont(fontType, 11, XFontStyle.Bold);

            if (loggerVariables.LoggerState == "Ready" || loggerVariables.LoggerState == "Delay")
                return false;
            pdfDocument.Info.Title = loggerInformation.SerialNumber;
            var pdfPage = CreateNewPage(font, loggerInformation.SerialNumber);

            void DrawChannelStatistics(string Label, Func<ChannelConfig, string> getString, double lineConterMultiplier = 1.0)
            {
                pdfPage.DrawString(Label, boldFont, XBrushes.Black, PDFcoordinates.first_column, lineCounter);
                pdfPage.DrawString(getString(channelOne) + channelOne.Unit , font, XBrushes.Black, PDFcoordinates.second_column, lineCounter);
                if ((channelTwoEnabled) && Label != DocumentLabelConstant.MKT)
                    pdfPage.DrawString(getString(channelTwo) + channelTwo.Unit, font, XBrushes.Black, PDFcoordinates.third_column, lineCounter);

                lineCounter += PDFcoordinates.line_inc * lineConterMultiplier;
            }

            void DrawChannelLimits(string Label, Func<ChannelConfig, string> getString, double lineConterMultiplier = 1.0)
            {
                pdfPage.DrawString(Label, boldFont, XBrushes.Black, PDFcoordinates.first_column, lineCounter);
                pdfPage.DrawString(getString(channelOne), font, XBrushes.Black, PDFcoordinates.second_column, lineCounter);
                if (channelTwoEnabled)
                    pdfPage.DrawString(getString(channelTwo), font, XBrushes.Black, PDFcoordinates.third_column, lineCounter);

                lineCounter += PDFcoordinates.line_inc * lineConterMultiplier;
            }

            void DrawSection(string firstColoumString, string secondColoumString, double lineConterMultiplier = 1.0)
            {
                pdfPage.DrawString(firstColoumString, boldFont, XBrushes.Black, PDFcoordinates.first_column, lineCounter);
                pdfPage.DrawString(secondColoumString, font, XBrushes.Black, PDFcoordinates.second_column, lineCounter);
                lineCounter += PDFcoordinates.line_inc * lineConterMultiplier;
            }

            if ((int)channelOne.OutsideLimits == 0 && (int)channelTwo.OutsideLimits == 0)
            {
                XImage greentick = XImage.FromFile(path+ DocumentLabelConstant.WithinLimitImage);
                pdfPage.DrawImage(greentick, PDFcoordinates.sign_left, PDFcoordinates.sign_top, 90, 80);
                pdfPage.DrawString(DocumentLabelConstant.WithinLimit, font, XBrushes.Black, PDFcoordinates.limitinfo_startX, PDFcoordinates.limitinfo_startY);
            }
            else
            {
                XImage redwarning = XImage.FromFile(path+DocumentLabelConstant.LimitsExceededImage);
                pdfPage.DrawImage(redwarning, PDFcoordinates.sign_left, PDFcoordinates.sign_top, 90, 80);
                pdfPage.DrawString(DocumentLabelConstant.LimitsExceeded, font, XBrushes.Black, PDFcoordinates.limitinfo_startX, PDFcoordinates.limitinfo_startY);
            }
            
            //Draw the boxes
            pdfPage.DrawRectangle(pen, PDFcoordinates.box1_X1, PDFcoordinates.box1_Y1, PDFcoordinates.box1_X2 - PDFcoordinates.box1_X1, PDFcoordinates.box1_Y2 - PDFcoordinates.box1_Y1);
            pdfPage.DrawRectangle(pen, PDFcoordinates.box2_X1, PDFcoordinates.box2_Y1, PDFcoordinates.box2_X2 - PDFcoordinates.box2_X1, PDFcoordinates.box2_Y2 - PDFcoordinates.box2_Y1);
            pdfPage.DrawRectangle(pen, PDFcoordinates.box3_X1, PDFcoordinates.box3_Y1, PDFcoordinates.box3_X2 - PDFcoordinates.box3_X1, PDFcoordinates.box3_Y2 - PDFcoordinates.box3_Y1);

            //Draw the Text
            DrawSection(DocumentLabelConstant.Model, loggerInformation.LoggerName);
            DrawSection(DocumentLabelConstant.LoggerState, loggerVariables.LoggerState);
            DrawSection(DocumentLabelConstant.Battery, loggerVariables.BatteryPercentage);
            DrawSection(DocumentLabelConstant.SamplePeriod, loggerVariables.SameplePeriod + DocumentLabelConstant.TimeSuffix);
            DrawSection(DocumentLabelConstant.StartDelay, loggerVariables.StartDelay + DocumentLabelConstant.TimeSuffix);
            DrawSection(DocumentLabelConstant.FirstSample, loggerVariables.FirstSample);
            DrawSection(DocumentLabelConstant.LastSample, loggerVariables.LastSample);
            DrawSection(DocumentLabelConstant.RecordedSample, loggerVariables.RecordedSamples.ToString());
            DrawSection(DocumentLabelConstant.TotalTrips, loggerVariables.TotalTrip.ToString());
            DrawSection(DocumentLabelConstant.TagsPlaced, loggerVariables.TagsPlaced.ToString());

            lineCounter -= PDFcoordinates.line_inc * 0.75;
            var break1 = new XRect(10, lineCounter, 680, 0);
            pdfPage.DrawRectangle(pen, break1);
            lineCounter += PDFcoordinates.line_inc * 0.75;
            
            pdfPage.DrawString(DocumentLabelConstant.Channel, boldFont, XBrushes.Black, PDFcoordinates.first_column, lineCounter);
            pdfPage.DrawString(DocumentLabelConstant.ChannelOneLabel, boldFont, XBrushes.Black, PDFcoordinates.second_column, lineCounter);
            if (channelTwoEnabled) pdfPage.DrawString(DocumentLabelConstant.ChannelTwoLabel, boldFont, XBrushes.Black, PDFcoordinates.third_column, lineCounter);
            lineCounter += PDFcoordinates.line_inc;

            if (channelOne.AboveLimits > 0) pdfPage.DrawString(DecodeConstant.Breached, font, XBrushes.Black, PDFcoordinates.second_column + 50, lineCounter);
            if (channelTwo.AboveLimits > 0) pdfPage.DrawString(DecodeConstant.Breached, font, XBrushes.Black, PDFcoordinates.third_column + 50, lineCounter);
            DrawChannelStatistics(DocumentLabelConstant.PresentUpperLimit, c => c.PresetUpperLimit.ToString("N2")); 
            if (channelOne.BelowLimits > 0) pdfPage.DrawString(DecodeConstant.Breached, font, XBrushes.Black, PDFcoordinates.second_column + 50, lineCounter);
            if (channelTwo.BelowLimits > 0) pdfPage.DrawString(DecodeConstant.Breached, font, XBrushes.Black, PDFcoordinates.third_column + 50, lineCounter);
            DrawChannelStatistics(DocumentLabelConstant.PresentLowerLimit, c => c.PresetLowerLimit.ToString("N2"));
            DrawChannelStatistics(DocumentLabelConstant.Mean, c => c.Mean.ToString("N2"));
            DrawChannelStatistics(DocumentLabelConstant.MKT, c => c.MKT_C.ToString("N2"));
            DrawChannelStatistics(DocumentLabelConstant.Max, c => c.Max.ToString("N2"));
            DrawChannelStatistics(DocumentLabelConstant.Min, c => c.Min.ToString("N2"));
            lineCounter += (PDFcoordinates.line_inc * 0.5);
            DrawChannelLimits(DocumentLabelConstant.SampleWithinLimits, c => c.WithinLimits.ToString("N1"));
            DrawChannelLimits(DocumentLabelConstant.TimeWithinLimits, c => c.TimeWithinLimits);
            lineCounter += (PDFcoordinates.line_inc * 0.5);
            DrawChannelLimits(DocumentLabelConstant.SampleOutofLimits, c => c.OutsideLimits.ToString("N1"));
            DrawChannelLimits(DocumentLabelConstant.TimeOutOfLimits, c => c.TimeOutLimits);
            lineCounter += (PDFcoordinates.line_inc * 0.5);
            DrawChannelLimits(DocumentLabelConstant.SampleAboveLimit, c => c.AboveLimits.ToString("N1"));
            DrawChannelLimits(DocumentLabelConstant.TimeAboveLimit, c => c.TimeAboveLimits);
            lineCounter += (PDFcoordinates.line_inc * 0.5);
            DrawChannelLimits(DocumentLabelConstant.SampleBelowLimit, c => c.BelowLimits.ToString("N1"));
            DrawChannelLimits(DocumentLabelConstant.TimeBelowLimit, c => c.TimeBelowLimits);
            DrawSection(DocumentLabelConstant.UserComment, string.Empty);

            if (loggerVariables.UserData.Length > 120)
            {
                var firstLine = loggerVariables.UserData.Substring(0, loggerVariables.UserData.Length/2);
                var secondLine = loggerVariables.UserData.Substring(loggerVariables.UserData.Length / 2);

                pdfPage.DrawString(firstLine, font, XBrushes.Black, PDFcoordinates.first_column, lineCounter);
                lineCounter += PDFcoordinates.line_inc;
                pdfPage.DrawString(secondLine, font, XBrushes.Black, PDFcoordinates.first_column, lineCounter);
                lineCounter += PDFcoordinates.line_inc*0.5;
            }
            else
            {
                pdfPage.DrawString(loggerVariables.UserData, font, XBrushes.Black, PDFcoordinates.first_column, lineCounter);
                lineCounter += PDFcoordinates.line_inc;
            }

            XRect break2 = new XRect(10, lineCounter, 680, 0);
            pdfPage.DrawRectangle(pen, break2);
            lineCounter += PDFcoordinates.line_inc * 0.75;

            pdfPage.DrawString(DocumentLabelConstant.ChannelOneLabel+ channelOne.Unit, font, XBrushes.DarkOliveGreen, PDFcoordinates.second_column, lineCounter);
            if (channelTwoEnabled) pdfPage.DrawString(DocumentLabelConstant.ChannelTwoLabel + channelTwo.Unit, font, XBrushes.MediumPurple, PDFcoordinates.second_column + 120, lineCounter);
            lineCounter += PDFcoordinates.line_inc;

            //Draw graph
            DrawGraph(decoder, loggerVariables, pdfPage, pen, font);
            FillInValues(decoder, loggerVariables, loggerInformation.SerialNumber);
            
            string filename = Path.GetTempPath() + "\\" + loggerInformation.SerialNumber + ".pdf";

            pdfDocument.Save(filename);
            return true;
        }
        
        void DrawGraph(HexFileDecoder decoder, LoggerVariables pdfVariables, XGraphics draw, XPen pen, XFont font)
        {
            var ch0 = new XPen(XColors.DarkGreen);
            var ch1 = new XPen(XColors.MediumPurple);
            var ch1Limits = new XPen(XColors.Lavender);
            var withinlimits = new XPen(XColors.ForestGreen);
            var abovelimit = new XPen(XColors.Coral);
            var belowlimit = new XPen(XColors.CornflowerBlue);

            ch1Limits.DashStyle = XDashStyle.Dash;
            withinlimits.DashStyle = XDashStyle.Dash;
            abovelimit.DashStyle = XDashStyle.Dash;
            belowlimit.DashStyle = XDashStyle.Dash;

            var chUpperLimit = new double[8];
            var chLowerLimit = new double[8];
            var chUpperYLimit = new double[8];
            var chLowerYLimit = new double[8];

            var chMax = new double[8];
            var chMin = new double[8];
            var chYMax = new double[8];
            var chYMin = new double[8];
            
            float yCH0 = 0;
            float yCH1 = 0;
            float xGraphMaximum = 55;
            float xGraphDate = 0;
            float xGraphScale = 0;
            float yGraphScale = 0;

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

            var yHighest = pdfVariables.ChannelOne.Max;
            var yLowest = pdfVariables.ChannelOne.Min;

            if (pdfVariables.IsChannelTwoEnabled) //Second Sensor
            {
                chUpperLimit[1] = pdfVariables.ChannelTwo.PresetUpperLimit;
                chLowerLimit[1] = pdfVariables.ChannelTwo.PresetLowerLimit;
                chMax[1] = pdfVariables.ChannelTwo.Max;
                chMin[1] = pdfVariables.ChannelTwo.Min;

                if (pdfVariables.ChannelTwo.Max > yHighest)
                    yHighest = pdfVariables.ChannelTwo.Max;

                if(pdfVariables.ChannelTwo.Min < yLowest)
                    yLowest = pdfVariables.ChannelTwo.Min;
            }
            
            //draw graph
            draw.DrawLine(pen, PDFcoordinates.G_axis_startX, PDFcoordinates.G_axis_startY, PDFcoordinates.G_axis_meetX, PDFcoordinates.G_axis_meetY);
            draw.DrawLine(pen, PDFcoordinates.G_axis_meetX, PDFcoordinates.G_axis_meetY, PDFcoordinates.G_axis_endX, PDFcoordinates.G_axis_endY);
            yGraphScale = (float)((PDFcoordinates.graph_H - 20) / (yHighest - yLowest));
            xGraphScale = (float)PDFcoordinates.graph_W / pdfVariables.RecordedSamples;

            while (numberofDates < pdfVariables.RecordedSamples)
            {
                xGraphDate = (xGraphScale * numberofDates) + xGraphMaximum;
                draw.DrawString(decoder.UNIXtoUTCDate(Convert.ToInt32(pdfVariables.Time[numberofDates])), font, XBrushes.Black, xGraphDate - 40, PDFcoordinates.G_axis_meetY + 15);
                draw.DrawString(decoder.UNIXtoUTCTime(Convert.ToInt32(pdfVariables.Time[numberofDates])), font, XBrushes.Black, xGraphDate - 45, PDFcoordinates.G_axis_meetY + 28);
                numberofDates += dateGap;
            }

            if (pdfVariables.IsChannelTwoEnabled && pdfVariables.RecordedSamples > 0)
            {
                yCH1 = (float)(PDFcoordinates.graph_H - (pdfVariables.ChannelTwo.Data[0] - yLowest) * yGraphScale) + PDFcoordinates.graph_topY;
                chUpperYLimit[1] = (float)(PDFcoordinates.graph_H - ((chUpperLimit[1] - yLowest) * yGraphScale)) + PDFcoordinates.graph_topY;
                chLowerYLimit[1] = (float)(PDFcoordinates.graph_H - ((chLowerLimit[1] - yLowest) * yGraphScale)) + PDFcoordinates.graph_topY;
                chYMax[1] = (float)(PDFcoordinates.graph_H - ((pdfVariables.ChannelTwo.Max - yLowest) * yGraphScale)) + PDFcoordinates.graph_topY;
                chYMin[1] = (float)(PDFcoordinates.graph_H - ((pdfVariables.ChannelTwo.Min - yLowest) * yGraphScale)) + PDFcoordinates.graph_topY;

                draw.DrawLine(ch1Limits, PDFcoordinates.graph_l_lineX_start, chYMax[1], PDFcoordinates.graph_l_lineX_end, chYMax[1]);
                draw.DrawLine(ch1Limits, PDFcoordinates.graph_l_lineX_start, chYMin[1], PDFcoordinates.graph_l_lineX_end, chYMin[1]);
                draw.DrawString(chMin[1].ToString("N2"), font, XBrushes.Black, PDFcoordinates.first_column, chYMin[1]);
                draw.DrawString(chMax[1].ToString("N2"), font, XBrushes.Black, PDFcoordinates.first_column, chYMax[1]);

                if ((chUpperLimit[1] < chMax[1]) && (chUpperLimit[1] > chMin[1]))
                {
                    draw.DrawString(pdfVariables.ChannelTwo.Unit + DocumentLabelConstant.UpperLimit, font, XBrushes.Coral, PDFcoordinates.third_column, chUpperYLimit[1] - 5);
                    draw.DrawString(chUpperLimit[1].ToString("N2"), font, XBrushes.Black, PDFcoordinates.first_column, chUpperYLimit[1]);
                    draw.DrawLine(abovelimit, PDFcoordinates.graph_l_lineX_start, chUpperYLimit[1], PDFcoordinates.graph_l_lineX_end, chUpperYLimit[1]);
                }

                if ((chLowerLimit[1] > chMin[1])&&(chLowerLimit[1] < chMax[1]))
                {
                    draw.DrawString(pdfVariables.ChannelTwo.Unit + DocumentLabelConstant.LowerLimit, font, XBrushes.CornflowerBlue, PDFcoordinates.third_column, chLowerYLimit[1] + 5); 
                    draw.DrawString(chLowerLimit[1].ToString("N2"), font, XBrushes.Black, PDFcoordinates.first_column, chLowerYLimit[1]);
                    draw.DrawLine(belowlimit, PDFcoordinates.graph_l_lineX_start, chLowerYLimit[1], PDFcoordinates.graph_l_lineX_end, chLowerYLimit[1]);
                }
            }


            if (pdfVariables.ChannelOne.Data != null)
            {
                yCH0 = (float)(PDFcoordinates.graph_H - (pdfVariables.ChannelOne.Data[0] -yLowest) * yGraphScale) + PDFcoordinates.graph_topY;
                chUpperYLimit[0] = (float)(PDFcoordinates.graph_H - ((chUpperLimit[0] - yLowest) * yGraphScale)) + PDFcoordinates.graph_topY;
                chLowerYLimit[0] = (float)(PDFcoordinates.graph_H - ((chLowerLimit[0] - yLowest) * yGraphScale)) + PDFcoordinates.graph_topY;
                chYMax[0] = (float)(PDFcoordinates.graph_H - ((pdfVariables.ChannelOne.Max - yLowest) * yGraphScale)) + PDFcoordinates.graph_topY;
                chYMin[0] = (float)(PDFcoordinates.graph_H - ((pdfVariables.ChannelOne.Min - yLowest) * yGraphScale)) + PDFcoordinates.graph_topY;

                draw.DrawLine(withinlimits, PDFcoordinates.graph_l_lineX_start, chYMax[0], PDFcoordinates.graph_l_lineX_end, chYMax[0]);
                draw.DrawLine(withinlimits, PDFcoordinates.graph_l_lineX_start, chYMin[0], PDFcoordinates.graph_l_lineX_end, chYMin[0]);
                draw.DrawString(chMin[0].ToString("N2"), font, XBrushes.Black, PDFcoordinates.first_column, chYMin[0]);
                draw.DrawString(chMax[0].ToString("N2"), font, XBrushes.Black, PDFcoordinates.first_column, chYMax[0]);

                if ((chUpperLimit[0] < chMax[0]) && (chUpperLimit[0] > chMin[0]))
                {
                    draw.DrawString(pdfVariables.ChannelOne.Unit + DocumentLabelConstant.UpperLimit, font, XBrushes.Coral, PDFcoordinates.third_column, chUpperYLimit[0] - 5);
                    draw.DrawString(chUpperLimit[0].ToString("N2"), font, XBrushes.Black, PDFcoordinates.first_column, chUpperYLimit[0]);
                    draw.DrawLine(abovelimit, PDFcoordinates.graph_l_lineX_start, chUpperYLimit[0], PDFcoordinates.graph_l_lineX_end, chUpperYLimit[0]);
                }

                if ((chLowerLimit[0] > chMin[0]) && (chLowerLimit[0] < chMax[0])) 
                {
                    draw.DrawString(pdfVariables.ChannelOne.Unit + DocumentLabelConstant.LowerLimit, font, XBrushes.CornflowerBlue, PDFcoordinates.third_column, chLowerYLimit[0] + 5);
                    draw.DrawString(chLowerLimit[0].ToString("N2"), font, XBrushes.Black, PDFcoordinates.first_column, chLowerYLimit[0]);
                    draw.DrawLine(belowlimit, PDFcoordinates.graph_l_lineX_start, chLowerYLimit[0], PDFcoordinates.graph_l_lineX_end, chLowerYLimit[0]);
                }
            }

            int i = 0;
            while (i < pdfVariables.RecordedSamples && (pdfVariables.ChannelOne.Data != null))
            {
                if(pdfVariables.ChannelOne.Data[i] > pdfVariables.ChannelOne.PresetUpperLimit)
                    draw.DrawLine(abovelimit, xGraphMaximum, yCH0, xGraphMaximum + xGraphScale, (float)(PDFcoordinates.graph_H - ((pdfVariables.ChannelOne.Data[i] - (yLowest)) * yGraphScale)) + PDFcoordinates.graph_topY);
                else if (pdfVariables.ChannelOne.Data[i] < pdfVariables.ChannelOne.PresetLowerLimit)
                    draw.DrawLine(belowlimit, xGraphMaximum, yCH0, xGraphMaximum + xGraphScale, (float)(PDFcoordinates.graph_H - ((pdfVariables.ChannelOne.Data[i] - (yLowest)) * yGraphScale)) + PDFcoordinates.graph_topY);
                else
                    draw.DrawLine(ch0, xGraphMaximum, yCH0, xGraphMaximum + xGraphScale, (float)(PDFcoordinates.graph_H - ((pdfVariables.ChannelOne.Data[i] - (yLowest)) * yGraphScale)) + PDFcoordinates.graph_topY);

                yCH0 = (float)(PDFcoordinates.graph_H - ((pdfVariables.ChannelOne.Data[i] - (yLowest)) * yGraphScale)) + PDFcoordinates.graph_topY;

                if (pdfVariables.IsChannelTwoEnabled)
                {
                    if(pdfVariables.ChannelTwo.Data[i] > pdfVariables.ChannelTwo.PresetUpperLimit)
                        draw.DrawLine(abovelimit, xGraphMaximum, yCH1, xGraphMaximum + xGraphScale, (float)(PDFcoordinates.graph_H - ((pdfVariables.ChannelTwo.Data[i] - (yLowest)) * yGraphScale)) + PDFcoordinates.graph_topY);
                    else if(pdfVariables.ChannelTwo.Data[i] < pdfVariables.ChannelTwo.PresetLowerLimit)
                        draw.DrawLine(belowlimit, xGraphMaximum, yCH1, xGraphMaximum + xGraphScale, (float)(PDFcoordinates.graph_H - ((pdfVariables.ChannelTwo.Data[i] - (yLowest)) * yGraphScale)) + PDFcoordinates.graph_topY);
                    else
                        draw.DrawLine(ch1, xGraphMaximum, yCH1, xGraphMaximum + xGraphScale, (float)(PDFcoordinates.graph_H - ((pdfVariables.ChannelTwo.Data[i] - (yLowest)) * yGraphScale)) + PDFcoordinates.graph_topY);

                    yCH1 = (float)(PDFcoordinates.graph_H - ((pdfVariables.ChannelTwo.Data[i] - (yLowest)) * yGraphScale)) + PDFcoordinates.graph_topY;
                }

                xGraphMaximum += xGraphScale;
                i++;
            }
        }

        void FillInValues(HexFileDecoder decoder, LoggerVariables pdfVariables, String serialNumber)
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

            XPen abovelimit = new XPen(XColors.Coral);
            XPen belowlimit = new XPen(XColors.CornflowerBlue);


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

                if(pdfVariables.ChannelOne.Data[i] > pdfVariables.ChannelOne.PresetUpperLimit)
                    valueDraw.DrawString(pdfVariables.ChannelOne.Data[i].ToString("N2")+ pdfVariables.ChannelOne.Unit, font, XBrushes.Coral, currentColumn, row);
                else if (pdfVariables.ChannelOne.Data[i] < pdfVariables.ChannelOne.PresetLowerLimit)
                    valueDraw.DrawString(pdfVariables.ChannelOne.Data[i].ToString("N2") + pdfVariables.ChannelOne.Unit, font, XBrushes.CornflowerBlue, currentColumn, row);
                else
                    valueDraw.DrawString(pdfVariables.ChannelOne.Data[i].ToString("N2") + pdfVariables.ChannelOne.Unit, font, XBrushes.Black, currentColumn, row);

                if (pdfVariables.IsChannelTwoEnabled)
                {
                    if (pdfVariables.ChannelTwo.Data[i] > pdfVariables.ChannelTwo.PresetUpperLimit)
                        valueDraw.DrawString(pdfVariables.ChannelTwo.Data[i].ToString("N2") + pdfVariables.ChannelTwo.Unit, font, XBrushes.Coral, currentColumn, row + rowIncrement);
                    else if (pdfVariables.ChannelTwo.Data[i] < pdfVariables.ChannelTwo.PresetLowerLimit)
                        valueDraw.DrawString(pdfVariables.ChannelTwo.Data[i].ToString("N2") + pdfVariables.ChannelTwo.Unit, font, XBrushes.CornflowerBlue, currentColumn, row + rowIncrement);
                    else
                        valueDraw.DrawString(pdfVariables.ChannelTwo.Data[i].ToString("N2") + pdfVariables.ChannelTwo.Unit, font, XBrushes.Black, currentColumn, row + rowIncrement);
                }

                currentColumn += columnIncrement;
            }
        }

        XGraphics CreateNewPage (XFont font, string serialNumber)
        {
            var serialfont = new XFont(fontType, 18, XFontStyle.Regular);
            var serialPen = new XPen(XColors.Blue, 3);
            var logo = XImage.FromFile(path+DocumentLabelConstant.LogoIcon);

            pageNumber++;

            var page = pdfDocument.AddPage();
            page.Height = 1000;
            page.Width = 700;
            
            var draw = XGraphics.FromPdfPage(page);
            draw.DrawString(DocumentLabelConstant.Title, serialfont, XBrushes.Blue, 10, 50);
            draw.DrawString(DocumentLabelConstant.SerialNumber+ serialNumber, serialfont, XBrushes.Blue, 550, 50);
            draw.DrawLine(serialPen, 10, 60, 690, 60);
            draw.DrawString(DocumentLabelConstant.Page + pageNumber , font, XBrushes.Black, 600, 980);
            draw.DrawString(DocumentLabelConstant.Website, font, XBrushes.Black, PDFcoordinates.siteX, PDFcoordinates.siteY);
            draw.DrawString(DateTime.UtcNow.ToString("dd/MM/yyy HH:mm:sss UTC"), font, XBrushes.Black, PDFcoordinates.dateX, PDFcoordinates.dateY);
            draw.DrawString("0.1.9.1", font, XBrushes.Black, PDFcoordinates.versionX, PDFcoordinates.versionY);
            draw.DrawImage(logo, 320, 10, 65, 40);

            return draw;
        }
    }
}
