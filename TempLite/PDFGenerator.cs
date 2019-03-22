using System;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using EZFontResolver1;
using PdfSharp.Fonts;

namespace TempLite
{
    public class PDFGenerator
    {
        private PdfDocument pdfDocument = new PdfDocument();

        public void CreatePDF(LoggerInformation loggerInformation)
        {
            double lineCounter = 80;

            var decoder = new G4HexDecoder(loggerInformation);
            decoder.ReadIntoJsonFileAndSetupDecoder();
            var pdfVariables = decoder.AssignPDFValue();
            var channelTwoEnabled = pdfVariables.IsChannelTwoEnabled;
            var channelOne = pdfVariables.ChannelOne;
            var channelTwo = pdfVariables.ChannelTwo;

            pdfDocument.Info.Title = loggerInformation.SerialNumber;

            //Create an empty page
            PdfPage page = pdfDocument.AddPage();
            page.Height = 1000;
            page.Width = 700;

            XGraphics draw = XGraphics.FromPdfPage(page);

            //---------------------------------//
            //TODO What if Font does not exist     
            //TODO CHECK SYSTEM OS
            //---------------------------------//
            EZFontResolver fontResolver = EZFontResolver.Get;
            GlobalFontSettings.FontResolver = fontResolver;
            //fontResolver.AddFont("Ubuntu", XFontStyle.Regular, @"fonts/ubuntufontfamily0.80/Ubuntu-R.ttf");
            //---------------------------------//
            //TODO CHECK SYSTEM OS
            //---------------------------------//
            fontResolver.AddFont("Roboto", XFontStyle.Regular, @"fonts/roboto_medium.ttf");
            XFont font = new XFont("Roboto", 11, XFontStyle.Regular);
            XFont serialfont = new XFont("Roboto", 18, XFontStyle.Regular);

            //create pen
            XPen pen = new XPen(XColors.Black, 1);
            XPen headerpen = new XPen(XColors.Blue, 3);

            void DrawChannelSection(string Label, Func<ChannelConfig, string> getString, double lineConterMultiplier = 1.0)
            {
                draw.DrawString(Label, font, XBrushes.Black, PDFcoordinates.first_column, lineCounter);
                draw.DrawString(getString(channelOne), font, XBrushes.Black, PDFcoordinates.second_column, lineCounter);
                if (channelTwoEnabled)
                    draw.DrawString(getString(channelTwo), font, XBrushes.Black, PDFcoordinates.third_column, lineCounter);

                lineCounter += PDFcoordinates.line_inc * lineConterMultiplier;
            }

            void DrawSection(string firstColoumString, string secondColoumString, double lineConterMultiplier = 1.0)
            {
                draw.DrawString(firstColoumString, font, XBrushes.Black, PDFcoordinates.first_column, lineCounter);
                draw.DrawString(secondColoumString, font, XBrushes.Black, PDFcoordinates.second_column, lineCounter);
                lineCounter += PDFcoordinates.line_inc * lineConterMultiplier;
            }

            if ((int)channelOne.OutsideLimits == 0 && (int)channelTwo.OutsideLimits == 0)
            {
                XImage greentick = XImage.FromFile("greentick.png");
                draw.DrawImage(greentick, PDFcoordinates.sign_left, PDFcoordinates.sign_top, 90, 80);
                draw.DrawString("Within Limits", font, XBrushes.Black, PDFcoordinates.limitinfo_startX, PDFcoordinates.limitinfo_startY);
            }
            else
            {
                XImage redwarning = XImage.FromFile("redwarning.png");
                draw.DrawImage(redwarning, PDFcoordinates.sign_left, PDFcoordinates.sign_top, 90, 80);
                draw.DrawString("Limits Exceeded", font, XBrushes.Black, PDFcoordinates.limitinfo_startX, PDFcoordinates.limitinfo_startY);
            }
            XImage logo = XImage.FromFile("logo.png");
            draw.DrawImage(logo, 320, 10, 65, 40);

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
            DrawSection("Tags Placed :", "0");

            XRect break1 = new XRect(10, lineCounter, 680, 0);
            draw.DrawRectangle(pen, break1);
            lineCounter += PDFcoordinates.line_inc * 0.75;

            draw.DrawString("#1 - Temperature", font, XBrushes.Black, PDFcoordinates.second_column - 25, lineCounter);
            if (channelTwoEnabled) draw.DrawString("#2 - Humidity", font, XBrushes.Black, PDFcoordinates.third_column - 25, lineCounter);
            lineCounter += PDFcoordinates.line_inc;

            DrawChannelSection("Preset Upper Limit :", c => c.PresetUpperLimit.ToString("N2") + pdfVariables.TempUnit); //need to add the breached
            DrawChannelSection("Preset Lower Limit :", c => c.PresetLowerLimit.ToString("N2") + pdfVariables.TempUnit); // need to add the breached
            DrawChannelSection("Mean Value :", c => c.Mean.ToString("N2"));
            DrawChannelSection("MKT Value :", c => c.MKT_C.ToString("N2"));
            DrawChannelSection("Max Recorded :", c => c.Max.ToString("N2"));
            DrawChannelSection("Min Recorded :", c => c.Min.ToString("N2"));
            lineCounter += (PDFcoordinates.line_inc * 0.5);
            DrawChannelSection("Total Samples within Limits :", c => c.WithinLimits.ToString("N1"));
            DrawChannelSection("Total Time within Limits :", c => c.TimeWithinLimits);
            lineCounter += (PDFcoordinates.line_inc * 0.5);
            DrawChannelSection("Total Samples out of Limits :", c => c.OutsideLimits.ToString("N1"));
            DrawChannelSection("Total Time out of Limits :", c => c.TimeOutLimits);
            lineCounter += (PDFcoordinates.line_inc * 0.5);
            DrawChannelSection("Samples above upper Limit :", c => c.AboveLimits.ToString("N1"));
            DrawChannelSection("Time above Upper Limit :", c => c.TimeAboveLimits);
            lineCounter += (PDFcoordinates.line_inc * 0.5);
            DrawChannelSection("Samples below Lower Limit :", c => c.BelowLimits.ToString("N1"));
            DrawChannelSection("Time below Lower Limit :", c => c.TimeBelowLimits);
            lineCounter += (PDFcoordinates.line_inc * 0.5);

            DrawSection("User Comments :", string.Empty);
            DrawSection(pdfVariables.UserData, string.Empty, 0.5);

            XRect break2 = new XRect(10, lineCounter, 680, 0);
            draw.DrawRectangle(pen, break2);
            lineCounter += PDFcoordinates.line_inc * 0.75;

            draw.DrawString("_ Temperature " + pdfVariables.TempUnit, font, XBrushes.DarkOliveGreen, PDFcoordinates.second_column, lineCounter);
            if (channelTwoEnabled) draw.DrawString("_ Humidity  %RH ", font, XBrushes.MediumPurple, PDFcoordinates.second_column + 120, lineCounter);
            lineCounter += PDFcoordinates.line_inc;

            //Draw graph
            DrawGraph(decoder, pdfVariables, draw, pen, font);

            //Header/Footer
            draw.DrawString("Logger Report", serialfont, XBrushes.Blue, 10, 50);
            draw.DrawString("S/N: " + loggerInformation.SerialNumber, serialfont, XBrushes.Blue, 550, 50);
            draw.DrawRectangle(headerpen, new XRect(10, 60, 680, 0));
            draw.DrawString("Comment ", font, XBrushes.Black, PDFcoordinates.commentX, PDFcoordinates.commentY);
            draw.DrawString("Signature ", font, XBrushes.Black, PDFcoordinates.sigX, PDFcoordinates.sigY);
            draw.DrawString("Page 1/1 ", font, XBrushes.Black, 600, 980);
            draw.DrawString("www.temprecord.com", font, XBrushes.Black, PDFcoordinates.siteX, PDFcoordinates.siteY);
            draw.DrawString(DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:sss UTC"), font, XBrushes.Black, PDFcoordinates.dateX, PDFcoordinates.dateY);
            draw.DrawString("0.1.9.1", font, XBrushes.Black, PDFcoordinates.versionX, PDFcoordinates.versionY);

            string filename = loggerInformation.SerialNumber + ".pdf";
            pdfDocument.Save(filename);
            //Process.Start(filename); //Previews PDF
            Console.WriteLine("PDF Created !");
        }


        private void DrawGraph(G4HexDecoder decoder, PDFvariables pdfVariables, XGraphics draw, XPen pen, XFont font)
        {
            XPen ch0 = new XPen(XColors.DarkGreen);
            XPen ch1 = new XPen(XColors.Lavender);
            XPen withinlimits = new XPen(XColors.ForestGreen);
            withinlimits.DashStyle = XDashStyle.Dash;
            XPen abovelimit = new XPen(XColors.Coral);
            abovelimit.DashStyle = XDashStyle.Dash;
            XPen belowlimit = new XPen(XColors.CornflowerBlue);
            belowlimit.DashStyle = XDashStyle.Dash;

            double ch1_upper_L = pdfVariables.ChannelOne.PresetUpperLimit;
            double ch1_lower_L = pdfVariables.ChannelOne.PresetLowerLimit;
            double ch1_max = pdfVariables.ChannelOne.Max;
            double ch1_min = pdfVariables.ChannelOne.Min;
            double ch1_highest = ch1_max;
            double ch1_lowest = ch1_min;
            double ch1_upper_Y = 0;
            double ch1_lower_Y = 0;

            double ch2_upper_L = 0;
            double ch2_lower_L = 0;
            double ch2_max = 0;
            double ch2_min = 0;
            double ch2_highest = 0;
            double ch2_lowest = 0;
            double ch2_upper_Y = 0;
            double ch2_lower_Y = 0;

            double ch_highest = 0;
            double ch_lowest = 0;

            float temp_next_y = 0;
            float rh_next_y = 0;
            float graph_topX = 55;
            float graph_date_x = 0;
            float graph_y_scale = 0;
            float graph_x_scale = 0;

            int i = pdfVariables.RecordedSamples / 5;
            int k = 0;
            int gap;
            if (i <= 0)
            {
                gap = 1;
            }
            else
            {
                gap = i;
            }

            if (pdfVariables.IsChannelTwoEnabled) //Second Sensor
            {
                ch2_upper_L = pdfVariables.ChannelTwo.PresetUpperLimit;
                ch2_lower_L = pdfVariables.ChannelTwo.PresetLowerLimit;
                ch2_max = pdfVariables.ChannelTwo.Max;
                ch2_min = pdfVariables.ChannelTwo.Min;
                ch2_highest = ch2_max;
                ch2_lowest = ch2_min;
            }
            else
            {
                ch_highest = ch1_highest;
                ch_lowest = ch1_lowest;
            }

            //draw graph
            draw.DrawLine(pen, PDFcoordinates.G_axis_startX, PDFcoordinates.G_axis_startY, PDFcoordinates.G_axis_meetX, PDFcoordinates.G_axis_meetY);
            draw.DrawLine(pen, PDFcoordinates.G_axis_meetX, PDFcoordinates.G_axis_meetY, PDFcoordinates.G_axis_endX, PDFcoordinates.G_axis_endY);
            graph_y_scale = (float)((PDFcoordinates.graph_H - 20) / (ch_highest - ch_lowest));
            graph_x_scale = (float)PDFcoordinates.graph_W / pdfVariables.RecordedSamples;

            while (i < pdfVariables.RecordedSamples)
            {
                graph_date_x = (graph_x_scale * i) + graph_topX;
                draw.DrawString(decoder.UNIXtoUTCDate(Convert.ToInt32(pdfVariables.Time[i])), font, XBrushes.Black, graph_date_x - 40, PDFcoordinates.G_axis_meetY + 15);
                draw.DrawString(decoder.UNIXtoUTCTime(Convert.ToInt32(pdfVariables.Time[i])), font, XBrushes.Black, graph_date_x - 45, PDFcoordinates.G_axis_meetY + 28);
                i += gap;
            }

            if (pdfVariables.IsChannelTwoEnabled && pdfVariables.RecordedSamples > 0)
            {
                ch2_upper_Y = (PDFcoordinates.graph_H - ((ch2_upper_L - ch_lowest) * graph_y_scale)) + PDFcoordinates.graph_topY;
                ch2_lower_Y = (PDFcoordinates.graph_H - ((ch2_lower_L - ch_lowest) * graph_y_scale)) + PDFcoordinates.graph_topY;
                ch2_max = (float)(PDFcoordinates.graph_H - ((pdfVariables.ChannelTwo.Max - ch_lowest) * graph_y_scale)) + PDFcoordinates.graph_topY;
                ch2_min = (float)(PDFcoordinates.graph_H - ((pdfVariables.ChannelTwo.Min - ch_lowest) * graph_y_scale)) + PDFcoordinates.graph_topY;

                draw.DrawString("__ Humidity %RH ", font, XBrushes.Black, PDFcoordinates.second_column + 120, PDFcoordinates.graph_topY - 5);
                draw.DrawString(" %RH Upper Limit ", font, XBrushes.Black, PDFcoordinates.third_column, (float)ch2_upper_Y - PDFcoordinates.graph_limit_label);
                draw.DrawString(ch2_upper_L.ToString("N2"), font, XBrushes.Black, PDFcoordinates.first_column, (float)ch2_upper_Y);
                draw.DrawString(" %RH Lower Limit ", font, XBrushes.Black, PDFcoordinates.third_column, (float)ch2_lower_Y + PDFcoordinates.graph_limit_label + 5);
                draw.DrawString(ch2_lower_L.ToString("N2"), font, XBrushes.Black, PDFcoordinates.first_column, (float)ch2_lower_Y);
                //draw.DrawString(pdfVariables.ChannelTwo.Mean.ToString("N2"), font, XBrushes.Black, PDFcoordinates.first_column, (float)(PDFcoordinates.graph_H - (((pdfVariables.ChannelTwo.Mean - ch_lowest) * graph_y_scale))) + PDFcoordinates.graph_topY);
                draw.DrawLine(pen, PDFcoordinates.graph_l_lineX_start, (float)ch2_lower_Y, PDFcoordinates.graph_l_lineX_end, (float)ch2_lower_Y);
                draw.DrawLine(pen, PDFcoordinates.graph_l_lineX_start, (float)ch2_upper_Y, PDFcoordinates.graph_l_lineX_end, (float)ch2_upper_Y);
                draw.DrawLine(ch1, PDFcoordinates.graph_l_lineX_start, ch2_max, PDFcoordinates.graph_l_lineX_end, ch2_max);
                draw.DrawLine(ch1, PDFcoordinates.graph_l_lineX_start, ch2_min, PDFcoordinates.graph_l_lineX_end, ch2_min);
            }


            if (pdfVariables.RecordedSamples > 0)
            {
                temp_next_y = (float)(PDFcoordinates.graph_H - (pdfVariables.ChannelOne.Data[0] - (ch_lowest)) * graph_y_scale) + PDFcoordinates.graph_topY;
                ch1_upper_Y = (PDFcoordinates.graph_H - ((ch1_upper_L - ch_lowest) * graph_y_scale)) + PDFcoordinates.graph_topY;
                ch1_lower_Y = (PDFcoordinates.graph_H - ((ch1_lower_L - ch_lowest) * graph_y_scale)) + PDFcoordinates.graph_topY;
                ch1_max = (float)(PDFcoordinates.graph_H - ((pdfVariables.ChannelOne.Max - ch_lowest) * graph_y_scale)) + PDFcoordinates.graph_topY;
                ch1_min = (float)(PDFcoordinates.graph_H - ((pdfVariables.ChannelOne.Min - ch_lowest) * graph_y_scale)) + PDFcoordinates.graph_topY;

                draw.DrawString(pdfVariables.TempUnit + " Upper Limit ", font, XBrushes.Coral, PDFcoordinates.third_column, (float)ch1_upper_Y - PDFcoordinates.graph_limit_label);
                draw.DrawString(ch1_upper_L.ToString("N2"), font, XBrushes.Black, PDFcoordinates.first_column, (float)ch1_upper_Y);
                draw.DrawString(pdfVariables.TempUnit + " Lower Limit ", font, XBrushes.CornflowerBlue, PDFcoordinates.third_column, (float)ch1_lower_Y + PDFcoordinates.graph_limit_label + 5);
                draw.DrawString(ch1_lower_L.ToString("N2"), font, XBrushes.Black, PDFcoordinates.first_column, (float)ch1_lower_Y);
                draw.DrawString(ch_lowest.ToString("N2"), font, XBrushes.Black, PDFcoordinates.first_column, ch1_min);
                draw.DrawString(ch_highest.ToString("N2"), font, XBrushes.Black, PDFcoordinates.first_column, ch1_max);
                //draw.DrawString(pdfVariables.ChannelOne.Mean.ToString("N2"), font, XBrushes.Black, PDFcoordinates.first_column, (float)(PDFcoordinates.graph_H - ((pdfVariables.ChannelOne.Mean - ch_lowest) * graph_y_scale)) + PDFcoordinates.graph_topY);
                draw.DrawLine(belowlimit, PDFcoordinates.graph_l_lineX_start, (float)ch1_lower_Y, PDFcoordinates.graph_l_lineX_end, (float)ch1_lower_Y);
                draw.DrawLine(abovelimit, PDFcoordinates.graph_l_lineX_start, (float)ch1_upper_Y, PDFcoordinates.graph_l_lineX_end, (float)ch1_upper_Y);
                draw.DrawLine(withinlimits, PDFcoordinates.graph_l_lineX_start, ch1_max, PDFcoordinates.graph_l_lineX_end, ch1_max);
                draw.DrawLine(withinlimits, PDFcoordinates.graph_l_lineX_start, ch1_min, PDFcoordinates.graph_l_lineX_end, ch1_min);
            }

            while (k < pdfVariables.RecordedSamples)
            {
                draw.DrawLine(ch0, graph_topX, temp_next_y, graph_topX + graph_x_scale, (float)(PDFcoordinates.graph_H - ((pdfVariables.ChannelOne.Data[k] - (ch_lowest)) * graph_y_scale)) + PDFcoordinates.graph_topY);
                temp_next_y = (float)(PDFcoordinates.graph_H - ((pdfVariables.ChannelOne.Data[k] - (ch_lowest)) * graph_y_scale)) + PDFcoordinates.graph_topY;

                if (pdfVariables.IsChannelTwoEnabled)
                {
                    draw.DrawLine(ch1, graph_topX, rh_next_y, graph_topX + graph_x_scale, (float)(PDFcoordinates.graph_H - ((pdfVariables.ChannelTwo.Data[k] - (ch_lowest)) * graph_y_scale)) + PDFcoordinates.graph_topY);
                    rh_next_y = (float)(PDFcoordinates.graph_H - ((pdfVariables.ChannelTwo.Data[k] - (ch_lowest)) * graph_y_scale)) + PDFcoordinates.graph_topY;
                }

                graph_topX += graph_x_scale;
                k++;
            }
        }
    }
}
