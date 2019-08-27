using PDF;
using PDF.Drawing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using TempLite.Constant;

namespace TempLite
{
    public class PDFGenerator
    {
        PDFDocument pdfDocument = new PDFDocument();

        public bool CreatePDF(LoggerInformation loggerInformation)
        {
            double lineCounter = 80;

            var decoder = new HexFileDecoder(loggerInformation);
            var loggerVariables = decoder.AssignLoggerValue();

            var channelOne = loggerVariables.ChannelOne;
            var channelTwo = loggerVariables.ChannelTwo;
            var channelTwoEnabled = loggerVariables.IsChannelTwoEnabled;

            var pen = new PDFPen(Color.Black, 1);
            var font = new PDFFont(FontType.Helvetica, 10, PDF.Drawing.FontStyle.Regular);
            var boldFont = new PDFFont(FontType.Helvetica, 10, PDF.Drawing.FontStyle.Bold);
            
            if (loggerVariables.LoggerState != "Logging" && loggerVariables.LoggerState != "Stopped")
                return false;

            var pdfPage = CreateNewPage(font, loggerInformation.SerialNumber);

            void DrawChannelStatistics(string Label, Func<ChannelConfig, string> getString, double lineConterMultiplier = 1.0)
            {
                pdfPage.DrawString(Label, boldFont, Color.Black, PDFcoordinates.firstColumn, (float)lineCounter);
                pdfPage.DrawString(getString(channelOne) + channelOne.Unit, font, Color.Black, PDFcoordinates.secondColumn, (float)lineCounter);
                if ((channelTwoEnabled) && Label != LabelConstant.MKT)
                    pdfPage.DrawString(getString(channelTwo) + channelTwo.Unit, font, Color.Black, PDFcoordinates.thirdColumn, (float)lineCounter);

                lineCounter += PDFcoordinates.lineInc * lineConterMultiplier;
            }

            void DrawChannelLimits(string Label, Func<ChannelConfig, string> getString, double lineConterMultiplier = 1.0)
            {
                pdfPage.DrawString(Label, boldFont, Color.Black, PDFcoordinates.firstColumn, (float)lineCounter);
                pdfPage.DrawString(getString(channelOne), font, Color.Black, PDFcoordinates.secondColumn, (float)lineCounter);
                if (channelTwoEnabled)
                    pdfPage.DrawString(getString(channelTwo), font, Color.Black, PDFcoordinates.thirdColumn, (float)lineCounter);

                lineCounter += PDFcoordinates.lineInc * lineConterMultiplier;
            }

            void DrawSection(string firstColoumString, string secondColoumString, double lineConterMultiplier = 1.0)
            {
                pdfPage.DrawString(firstColoumString, boldFont, Color.Black, PDFcoordinates.firstColumn, (float)lineCounter);
                pdfPage.DrawString(secondColoumString, font, Color.Black, PDFcoordinates.secondColumn, (float)lineCounter);
                lineCounter += PDFcoordinates.lineInc * lineConterMultiplier;
            }

            if (channelOne.OutsideLimits == 0 && channelTwo.OutsideLimits == 0)
            {
                var greenPen = new PDFPen(Color.LimeGreen, 15);
                pdfPage.DrawLine(greenPen, 560, 163, 580, 180);
                pdfPage.DrawLine(greenPen, 580, 190, 642, 120);
                pdfPage.DrawString(LabelConstant.WithinLimit, font, Color.Black, PDFcoordinates.limitinfo_startX, PDFcoordinates.limitinfo_startY);
            }
            else
            {
                var redPen = new PDFPen(Color.OrangeRed, 15);
                pdfPage.DrawLine(redPen, 570, 125, 630, 190);
                pdfPage.DrawLine(redPen, 570, 190, 630, 125);
                pdfPage.DrawString(LabelConstant.LimitsExceeded, font, Color.Black, PDFcoordinates.limitinfo_startX, PDFcoordinates.limitinfo_startY);
            }

            //Draw the boxes
            pdfPage.DrawRectangle(pen, PDFcoordinates.box1_X1, PDFcoordinates.box1_Y1, PDFcoordinates.box1_X2 - PDFcoordinates.box1_X1, PDFcoordinates.box1_Y2 - PDFcoordinates.box1_Y1);
            pdfPage.DrawRectangle(pen, PDFcoordinates.box2_X1, PDFcoordinates.box2_Y1, PDFcoordinates.box2_X2 - PDFcoordinates.box2_X1, PDFcoordinates.box2_Y2 - PDFcoordinates.box2_Y1);
            pdfPage.DrawRectangle(pen, PDFcoordinates.box3_X1, PDFcoordinates.box3_Y1, PDFcoordinates.box3_X2 - PDFcoordinates.box3_X1, PDFcoordinates.box3_Y2 - PDFcoordinates.box3_Y1);

            //Draw the Text
            DrawSection(LabelConstant.Model, loggerInformation.LoggerName);
            DrawSection(LabelConstant.LoggerState, loggerVariables.LoggerState);
            DrawSection(LabelConstant.Battery, loggerVariables.BatteryPercentage);
            DrawSection(LabelConstant.SamplePeriod, loggerVariables.SameplePeriod + LabelConstant.TimeSuffix);
            DrawSection(LabelConstant.StartDelay, loggerVariables.StartDelay + LabelConstant.TimeSuffix);
            DrawSection(LabelConstant.FirstSample, loggerVariables.FirstSample);
            DrawSection(LabelConstant.LastSample, loggerVariables.LastSample);
            DrawSection(LabelConstant.RecordedSample, loggerVariables.RecordedSamples.ToString());
            DrawSection(LabelConstant.TotalTrips, loggerVariables.TotalTrip.ToString());
            DrawSection(LabelConstant.TagsPlaced, loggerVariables.TagsPlaced.ToString());

            lineCounter -= PDFcoordinates.lineInc * 0.75;
            pdfPage.DrawLine(pen, 10, lineCounter, 680, lineCounter);
            lineCounter += PDFcoordinates.lineInc * 0.75;

            pdfPage.DrawString(LabelConstant.Channel, boldFont, Color.Black, PDFcoordinates.firstColumn, lineCounter);
            pdfPage.DrawString(LabelConstant.ChannelOneLabel, boldFont, Color.Black, PDFcoordinates.secondColumn, lineCounter);
            if (channelTwoEnabled) pdfPage.DrawString(LabelConstant.ChannelTwoLabel, boldFont, Color.Black, PDFcoordinates.thirdColumn, (float)lineCounter);
            lineCounter += PDFcoordinates.lineInc;

            if (channelOne.AboveLimits > 0) pdfPage.DrawString(DecodeConstant.Exceeded, font, Color.Black, PDFcoordinates.secondColumn + 50, (float)lineCounter);
            if (channelTwo.AboveLimits > 0) pdfPage.DrawString(DecodeConstant.Exceeded, font, Color.Black, PDFcoordinates.thirdColumn + 50, (float)lineCounter);
            DrawChannelStatistics(LabelConstant.PresentUpperLimit, c => c.PresetUpperLimit.ToString("N2"));
            if (channelOne.BelowLimits > 0) pdfPage.DrawString(DecodeConstant.Exceeded, font, Color.Black, PDFcoordinates.secondColumn + 50, (float)lineCounter);
            if (channelTwo.BelowLimits > 0) pdfPage.DrawString(DecodeConstant.Exceeded, font, Color.Black, PDFcoordinates.thirdColumn + 50, (float)lineCounter);
            DrawChannelStatistics(LabelConstant.PresentLowerLimit, c => c.PresetLowerLimit.ToString("N2"));
            DrawChannelStatistics(LabelConstant.Mean, c => c.Mean.ToString("N2"));
            DrawChannelStatistics(LabelConstant.MKT, c => c.MKT_C.ToString("N2"));
            DrawChannelStatistics(LabelConstant.Max, c => c.Max.ToString("N2"));
            DrawChannelStatistics(LabelConstant.Min, c => c.Min.ToString("N2"));
            lineCounter += (PDFcoordinates.lineInc * 0.5);
            DrawChannelLimits(LabelConstant.SampleWithinLimits, c => c.WithinLimits.ToString());
            DrawChannelLimits(LabelConstant.TimeWithinLimits, c => c.TimeWithinLimits);
            lineCounter += (PDFcoordinates.lineInc * 0.5);
            DrawChannelLimits(LabelConstant.SampleOutofLimits, c => c.OutsideLimits.ToString());
            DrawChannelLimits(LabelConstant.TimeOutOfLimits, c => c.TimeOutLimits);
            lineCounter += (PDFcoordinates.lineInc * 0.5);
            DrawChannelLimits(LabelConstant.SampleAboveLimit, c => c.AboveLimits.ToString());
            DrawChannelLimits(LabelConstant.TimeAboveLimit, c => c.TimeAboveLimits);
            lineCounter += (PDFcoordinates.lineInc * 0.5);
            DrawChannelLimits(LabelConstant.SampleBelowLimit, c => c.BelowLimits.ToString());
            DrawChannelLimits(LabelConstant.TimeBelowLimit, c => c.TimeBelowLimits);
            DrawSection(LabelConstant.UserComment, string.Empty);

            if (loggerVariables.UserData.Length > 140)
            {
                var firstLine = loggerVariables.UserData.Substring(0, loggerVariables.UserData.Length / 2);
                var secondLine = loggerVariables.UserData.Substring(loggerVariables.UserData.Length / 2);

                pdfPage.DrawString(firstLine, font, Color.Black, PDFcoordinates.firstColumn, lineCounter);
                lineCounter += PDFcoordinates.lineInc;
                pdfPage.DrawString(secondLine, font, Color.Black, PDFcoordinates.firstColumn, lineCounter);
                lineCounter += PDFcoordinates.lineInc * 0.5;
            }
            else
            {
                pdfPage.DrawString(loggerVariables.UserData, font, Color.Black, PDFcoordinates.firstColumn, lineCounter);
                lineCounter += PDFcoordinates.lineInc;
            }

            pdfPage.DrawLine(pen, 10, lineCounter, 680, lineCounter);
            lineCounter += PDFcoordinates.lineInc * 0.75;

            pdfPage.DrawString(LabelConstant.ChannelOneLabel + channelOne.Unit, font, Color.DarkOliveGreen, PDFcoordinates.secondColumn, lineCounter);
            if (channelTwoEnabled) pdfPage.DrawString(LabelConstant.ChannelTwoLabel + channelTwo.Unit, font, Color.MediumPurple, PDFcoordinates.secondColumn + 120, lineCounter);
            lineCounter += PDFcoordinates.lineInc;

            pdfPage.DrawString(LabelConstant.Comment,font,Color.Black, PDFcoordinates.commentX, PDFcoordinates.commentY);
            pdfPage.DrawString(LabelConstant.Signature,font,Color.Black, PDFcoordinates.sigX, PDFcoordinates.sigY);

            //Draw graph
            DrawGraph(decoder, loggerVariables, pdfPage, pen, font);
            FillInValues(decoder, loggerVariables, loggerInformation.SerialNumber);

            string filename = Path.GetTempPath() + loggerInformation.SerialNumber + ".pdf";

            pdfDocument.Save(filename);
            return true;
        }

        void DrawGraph(HexFileDecoder decoder, LoggerVariables pdfVariables, PDFPage pdfPage, PDFPen pen, PDFFont font)
        {
            var ch1LimitsPen = new PDFPen(Color.Lavender, 1, PenStyle.ShortDash);
            var withinlimitsPen = new PDFPen(Color.ForestGreen, 1, PenStyle.ShortDash);
            var abovelimitPen = new PDFPen(Color.Coral, 1, PenStyle.ShortDash);
            var belowlimitPen = new PDFPen(Color.CornflowerBlue, 1, PenStyle.ShortDash);

            var presetUpperLimit = new double[8];
            var presetLowerLimit = new double[8];
            var presetUpperLimitY = new double[8];
            var presetLowerLimitY = new double[8];

            var maxRecorded = new double[8];
            var minRecorded = new double[8];
            var maxRecordedY = new double[8];
            var minRecordedY = new double[8];

            float dateX = 0;
            float maximumX = 55;
            float graphScaleX = 0;
            float graphScaleY = 0;
            float y0 = 0;
            float y1 = 0;

            int numberofDates = pdfVariables.RecordedSamples / 5;
            int dateGap;

            if (numberofDates == 0)
            {
                dateGap = 1;
            }
            else
            {
                dateGap = numberofDates;
            }

            presetUpperLimit[0] = pdfVariables.ChannelOne.PresetUpperLimit;
            presetLowerLimit[0] = pdfVariables.ChannelOne.PresetLowerLimit;
            maxRecorded[0] = pdfVariables.ChannelOne.Max;
            minRecorded[0] = pdfVariables.ChannelOne.Min;

            var yHighest = pdfVariables.ChannelOne.Max;
            var yLowest = pdfVariables.ChannelOne.Min;

            if (pdfVariables.IsChannelTwoEnabled) //Second Sensor
            {
                presetUpperLimit[1] = pdfVariables.ChannelTwo.PresetUpperLimit;
                presetLowerLimit[1] = pdfVariables.ChannelTwo.PresetLowerLimit;
                maxRecorded[1] = pdfVariables.ChannelTwo.Max;
                minRecorded[1] = pdfVariables.ChannelTwo.Min;

                if (pdfVariables.ChannelTwo.Max > yHighest) yHighest = pdfVariables.ChannelTwo.Max;
                if (pdfVariables.ChannelTwo.Min < yLowest) yLowest = pdfVariables.ChannelTwo.Min;
            }

            //draw graph
            pdfPage.DrawLine(pen, PDFcoordinates.Xstart, PDFcoordinates.Ystart, PDFcoordinates.Xstart, PDFcoordinates.Yfinish);
            pdfPage.DrawLine(pen, PDFcoordinates.Xstart, PDFcoordinates.Yfinish, PDFcoordinates.Xfinish, PDFcoordinates.Yfinish);
            graphScaleY = (float)((PDFcoordinates.graphHeight - 20) / (yHighest - yLowest));
            graphScaleX = (float)PDFcoordinates.graphWidth / pdfVariables.RecordedSamples;

            while (numberofDates < pdfVariables.RecordedSamples)
            {
                dateX = (graphScaleX * numberofDates) + maximumX;
                pdfPage.DrawString(decoder.UNIXtoUTCDate(Convert.ToInt32(pdfVariables.Time[numberofDates])), font, Color.Black, dateX - 40, PDFcoordinates.Yfinish + 15);
                pdfPage.DrawString(decoder.UNIXtoUTCTime(Convert.ToInt32(pdfVariables.Time[numberofDates])), font, Color.Black, dateX - 45, PDFcoordinates.Yfinish + 28);
                numberofDates += dateGap;
            }

            if (pdfVariables.IsChannelTwoEnabled && pdfVariables.RecordedSamples > 0)
            {
                y1 = (float)(PDFcoordinates.graphHeight - (pdfVariables.ChannelTwo.Data[0] - yLowest) * graphScaleY) + PDFcoordinates.Ymax;
                presetUpperLimitY[1] = (float)(PDFcoordinates.graphHeight - ((presetUpperLimit[1] - yLowest) * graphScaleY)) + PDFcoordinates.Ymax;
                presetLowerLimitY[1] = (float)(PDFcoordinates.graphHeight - ((presetLowerLimit[1] - yLowest) * graphScaleY)) + PDFcoordinates.Ymax;
                maxRecordedY[1] = (float)(PDFcoordinates.graphHeight - ((pdfVariables.ChannelTwo.Max - yLowest) * graphScaleY)) + PDFcoordinates.Ymax;
                minRecordedY[1] = (float)(PDFcoordinates.graphHeight - ((pdfVariables.ChannelTwo.Min - yLowest) * graphScaleY)) + PDFcoordinates.Ymax;

                pdfPage.DrawLine(ch1LimitsPen, PDFcoordinates.Xstart, maxRecordedY[1], PDFcoordinates.Xfinish, maxRecordedY[1]);
                pdfPage.DrawLine(ch1LimitsPen, PDFcoordinates.Xstart, minRecordedY[1], PDFcoordinates.Xfinish, minRecordedY[1]);
                pdfPage.DrawString(minRecorded[1].ToString("N2"), font, Color.Black, PDFcoordinates.firstColumn, minRecordedY[1]);
                pdfPage.DrawString(maxRecorded[1].ToString("N2"), font, Color.Black, PDFcoordinates.firstColumn, maxRecordedY[1]);

                if ((presetUpperLimit[1] < maxRecorded[1]) && (presetUpperLimit[1] > minRecorded[1]))
                {
                    pdfPage.DrawString(pdfVariables.ChannelTwo.Unit + LabelConstant.UpperLimit, font, Color.Coral, PDFcoordinates.thirdColumn, presetUpperLimitY[1] - 5);
                    pdfPage.DrawString(presetUpperLimit[1].ToString("N2"), font, Color.Black, PDFcoordinates.firstColumn, presetUpperLimitY[1]);
                    pdfPage.DrawLine(abovelimitPen, PDFcoordinates.Xstart, presetUpperLimitY[1], PDFcoordinates.Xfinish, presetUpperLimitY[1]);
                }

                if ((presetLowerLimit[1] > minRecorded[1]) && (presetLowerLimit[1] < maxRecorded[1]))
                {
                    pdfPage.DrawString(pdfVariables.ChannelTwo.Unit + LabelConstant.LowerLimit, font, Color.CornflowerBlue, PDFcoordinates.thirdColumn, presetLowerLimitY[1] + 5);
                    pdfPage.DrawString(presetLowerLimit[1].ToString("N2"), font, Color.Black, PDFcoordinates.firstColumn, presetLowerLimitY[1]);
                    pdfPage.DrawLine(belowlimitPen, PDFcoordinates.Xstart, presetLowerLimitY[1], PDFcoordinates.Xfinish, presetLowerLimitY[1]);
                }
            }


            if (pdfVariables.ChannelOne.Data != null)
            {
                y0 = (float)(PDFcoordinates.graphHeight - (pdfVariables.ChannelOne.Data[0] - yLowest) * graphScaleY) + PDFcoordinates.Ymax;
                presetUpperLimitY[0] = (float)(PDFcoordinates.graphHeight - ((presetUpperLimit[0] - yLowest) * graphScaleY)) + PDFcoordinates.Ymax;
                presetLowerLimitY[0] = (float)(PDFcoordinates.graphHeight - ((presetLowerLimit[0] - yLowest) * graphScaleY)) + PDFcoordinates.Ymax;
                maxRecordedY[0] = (float)(PDFcoordinates.graphHeight - ((pdfVariables.ChannelOne.Max - yLowest) * graphScaleY)) + PDFcoordinates.Ymax;
                minRecordedY[0] = (float)(PDFcoordinates.graphHeight - ((pdfVariables.ChannelOne.Min - yLowest) * graphScaleY)) + PDFcoordinates.Ymax;

                pdfPage.DrawLine(withinlimitsPen, PDFcoordinates.Xstart, maxRecordedY[0], PDFcoordinates.Xfinish, maxRecordedY[0]);
                pdfPage.DrawLine(withinlimitsPen, PDFcoordinates.Xstart, minRecordedY[0], PDFcoordinates.Xfinish, minRecordedY[0]);
                pdfPage.DrawString(minRecorded[0].ToString("N2"), font, Color.Black, PDFcoordinates.firstColumn, minRecordedY[0]);
                pdfPage.DrawString(maxRecorded[0].ToString("N2"), font, Color.Black, PDFcoordinates.firstColumn, maxRecordedY[0]);

                if ((presetUpperLimit[0] < maxRecorded[0]) && (presetUpperLimit[0] > minRecorded[0]))
                {
                    pdfPage.DrawString(pdfVariables.ChannelOne.Unit + LabelConstant.UpperLimit, font, Color.Coral, PDFcoordinates.thirdColumn, presetUpperLimitY[0] - 5);
                    pdfPage.DrawString(presetUpperLimit[0].ToString("N2"), font, Color.Black, PDFcoordinates.firstColumn, presetUpperLimitY[0]);
                    pdfPage.DrawLine(abovelimitPen, PDFcoordinates.Xstart, presetUpperLimitY[0], PDFcoordinates.Xfinish, presetUpperLimitY[0]);
                }

                if ((presetLowerLimit[0] > minRecorded[0]) && (presetLowerLimit[0] < maxRecorded[0]))
                {
                    pdfPage.DrawString(pdfVariables.ChannelOne.Unit + LabelConstant.LowerLimit, font, Color.CornflowerBlue, PDFcoordinates.thirdColumn, presetLowerLimitY[0] + 5);
                    pdfPage.DrawString(presetLowerLimit[0].ToString("N2"), font, Color.Black, PDFcoordinates.firstColumn, presetLowerLimitY[0]);
                    pdfPage.DrawLine(belowlimitPen, PDFcoordinates.Xstart, presetLowerLimitY[0], PDFcoordinates.Xfinish, presetLowerLimitY[0]);
                }
            }

            int i = 0;
            int tagCount = 0;
            var ch0Pen = new PDFPen(Color.DarkGreen, 1);
            var ch1Pen = new PDFPen(Color.MediumPurple, 1);
            var abovelimitDataPen = new PDFPen(Color.Coral, 1);
            var belowlimitDataPen = new PDFPen(Color.CornflowerBlue, 1);

            while (i < pdfVariables.RecordedSamples)
            {
                if (tagCount < pdfVariables.Tag.Count && pdfVariables.Tag[tagCount] == i)
                {
                    var tagPen = new PDFPen(Color.ForestGreen, 1);
                    pdfPage.DrawLine(tagPen, maximumX - 2, y0 - 2, maximumX + 2, y0 + 2);
                    pdfPage.DrawLine(tagPen, maximumX - 2, y0 + 2, maximumX + 2, y0 - 2);

                    tagCount++;
                }

                if (pdfVariables.ChannelOne.Data != null)
                {
                    if (pdfVariables.ChannelOne.Data[i] > pdfVariables.ChannelOne.PresetUpperLimit)
                        pdfPage.DrawLine(abovelimitDataPen, maximumX, y0, maximumX + graphScaleX, (float)(PDFcoordinates.graphHeight - ((pdfVariables.ChannelOne.Data[i] - (yLowest)) * graphScaleY)) + PDFcoordinates.Ymax);
                    else if (pdfVariables.ChannelOne.Data[i] < pdfVariables.ChannelOne.PresetLowerLimit)
                        pdfPage.DrawLine(belowlimitDataPen, maximumX, y0, maximumX + graphScaleX, (float)(PDFcoordinates.graphHeight - ((pdfVariables.ChannelOne.Data[i] - (yLowest)) * graphScaleY)) + PDFcoordinates.Ymax);
                    else
                        pdfPage.DrawLine(ch0Pen, maximumX, y0, maximumX + graphScaleX, (float)(PDFcoordinates.graphHeight - ((pdfVariables.ChannelOne.Data[i] - (yLowest)) * graphScaleY)) + PDFcoordinates.Ymax);

                    y0 = (float)(PDFcoordinates.graphHeight - ((pdfVariables.ChannelOne.Data[i] - (yLowest)) * graphScaleY)) + PDFcoordinates.Ymax;
                }

                if (pdfVariables.IsChannelTwoEnabled)
                {
                    if (pdfVariables.ChannelTwo.Data[i] > pdfVariables.ChannelTwo.PresetUpperLimit)
                        pdfPage.DrawLine(abovelimitDataPen, maximumX, y1, maximumX + graphScaleX, (float)(PDFcoordinates.graphHeight - ((pdfVariables.ChannelTwo.Data[i] - (yLowest)) * graphScaleY)) + PDFcoordinates.Ymax);
                    else if (pdfVariables.ChannelTwo.Data[i] < pdfVariables.ChannelTwo.PresetLowerLimit)
                        pdfPage.DrawLine(belowlimitDataPen, maximumX, y1, maximumX + graphScaleX, (float)(PDFcoordinates.graphHeight - ((pdfVariables.ChannelTwo.Data[i] - (yLowest)) * graphScaleY)) + PDFcoordinates.Ymax);
                    else
                        pdfPage.DrawLine(ch1Pen, maximumX, y1, maximumX + graphScaleX, (float)(PDFcoordinates.graphHeight - ((pdfVariables.ChannelTwo.Data[i] - (yLowest)) * graphScaleY)) + PDFcoordinates.Ymax);

                    y1 = (float)(PDFcoordinates.graphHeight - ((pdfVariables.ChannelTwo.Data[i] - (yLowest)) * graphScaleY)) + PDFcoordinates.Ymax;
                }
                
                maximumX += graphScaleX;
                i++;
            }
        }

        void FillInValues(HexFileDecoder decoder, LoggerVariables pdfVariables, String serialNumber)
        {
            var tagCount = 0;

            var dateColumn = 20;
            var timeColumn = 30;

            var columnStart = 75;
            var currentColumn = columnStart;
            var maxColumnValue = 650;
            var columnIncrement = 28;

            var rowStart = 65;
            var row = rowStart;
            var rowIncrement = 12;

            var pageFont = new PDFFont(FontType.Helvetica, 10, PDF.Drawing.FontStyle.Regular);
            var font = new PDFFont(FontType.Helvetica, 6, PDF.Drawing.FontStyle.Regular);
            var boldFont = new PDFFont(FontType.Helvetica, 6, PDF.Drawing.FontStyle.Bold);
            var tempPen = new PDFPen(Color.Black, 1);
            var humPen = new PDFPen(Color.Gray, 1);

            PDFPen abovelimit = new PDFPen(Color.Coral, 1);
            PDFPen belowlimit = new PDFPen(Color.CornflowerBlue, 1);


            var time = new List<string>();
            var date = new List<string>();

            var valuePage = CreateNewPage(pageFont, serialNumber);

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

                    valuePage.DrawString(date[i], boldFont, Color.Black, dateColumn, row);
                    row += rowIncrement;
                    valuePage.DrawString(time[i], boldFont, Color.Black, timeColumn, row);
                    currentColumn = columnStart;
                }

                if ((currentColumn > maxColumnValue))
                {
                    currentColumn = columnStart;

                    if (pdfVariables.IsChannelTwoEnabled)
                        row = row + rowIncrement * 2;
                    else
                        row += rowIncrement;

                    if (row > 950)
                    {
                        valuePage = CreateNewPage(pageFont, serialNumber);
                        row = rowStart + rowIncrement;
                        currentColumn = columnStart;
                    }

                    valuePage.DrawString(time[i], boldFont, Color.Black, timeColumn, row);
                }

                if (tagCount < pdfVariables.Tag.Count && pdfVariables.Tag[tagCount] == i)
                {
                    valuePage.DrawString(pdfVariables.ChannelOne.Data[i].ToString("N2") + pdfVariables.ChannelOne.Unit, font, Color.ForestGreen, currentColumn, row);
                    tagCount++;
                }
                else if (pdfVariables.ChannelOne.Data[i] > pdfVariables.ChannelOne.PresetUpperLimit)
                    valuePage.DrawString(pdfVariables.ChannelOne.Data[i].ToString("N2") + pdfVariables.ChannelOne.Unit, font, Color.Coral, currentColumn, row);
                else if (pdfVariables.ChannelOne.Data[i] < pdfVariables.ChannelOne.PresetLowerLimit)
                    valuePage.DrawString(pdfVariables.ChannelOne.Data[i].ToString("N2") + pdfVariables.ChannelOne.Unit, font, Color.CornflowerBlue, currentColumn, row);
                else
                    valuePage.DrawString(pdfVariables.ChannelOne.Data[i].ToString("N2") + pdfVariables.ChannelOne.Unit, font, Color.Black, currentColumn, row);

                if (pdfVariables.IsChannelTwoEnabled)
                {
                    if (pdfVariables.ChannelTwo.Data[i] > pdfVariables.ChannelTwo.PresetUpperLimit)
                        valuePage.DrawString(pdfVariables.ChannelTwo.Data[i].ToString("N2") + pdfVariables.ChannelTwo.Unit, font, Color.Coral, currentColumn, row + rowIncrement);
                    else if (pdfVariables.ChannelTwo.Data[i] < pdfVariables.ChannelTwo.PresetLowerLimit)
                        valuePage.DrawString(pdfVariables.ChannelTwo.Data[i].ToString("N2") + pdfVariables.ChannelTwo.Unit, font, Color.CornflowerBlue, currentColumn, row + rowIncrement);
                    else
                        valuePage.DrawString(pdfVariables.ChannelTwo.Data[i].ToString("N2") + pdfVariables.ChannelTwo.Unit, font, Color.Black, currentColumn, row + rowIncrement);
                }

                currentColumn += columnIncrement;
            }
        }

        int pageNumber = 0;
        PDFPage CreateNewPage(PDFFont font, string serialNumber)
        {
            var headerPen = new PDFPen(Color.DarkSlateBlue, 120);
            var serialfont = new PDFFont(FontType.Helvetica, 16, PDF.Drawing.FontStyle.Regular);
            var companyNameFont = new PDFFont(FontType.Helvetica, 30, PDF.Drawing.FontStyle.Bold);

            pageNumber++;

            PDFPage page = pdfDocument.AddPage();
            page.Height = 1000;
            page.Width = 700;

            page.DrawLine(headerPen, 0, 0, 700, 0);
            page.DrawString(LabelConstant.Title, serialfont, Color.White, 10, 50);
            page.DrawString(LabelConstant.SerialNumber + serialNumber, serialfont, Color.White, 550, 50);
            page.DrawString(LabelConstant.CompanyName, companyNameFont, Color.White, 230, 40);
            page.DrawString(LabelConstant.TradeMark, font, Color.White, 450, 20);
            page.DrawString(LabelConstant.Page + pageNumber, font, Color.Black, 600, 980);
            page.DrawString(LabelConstant.Website, font, Color.Black, PDFcoordinates.siteX, PDFcoordinates.siteY);
            page.DrawString(DateTime.UtcNow.ToString("dd/MM/yyy HH:mm:sss UTC"), font, Color.White, PDFcoordinates.dateX, PDFcoordinates.dateY);
            page.DrawString("0.1.9.1", font, Color.Black, PDFcoordinates.versionX, PDFcoordinates.versionY);

            return page;
        }
    }
}
