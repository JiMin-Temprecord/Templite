using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using TempLite.Services;

namespace TempLite
{
    public class PDFGenerator
    {
        private PdfDocument _pdfDocument = new PdfDocument();
        private PDFvariables _pdfVariables = new PDFvariables();

        public void CreatePDF(CommunicationServices _communicationService)
        {
            double lineCounter = 80;

            new HexfileDecoder(_communicationService, _pdfVariables);
            _pdfDocument.Info.Title = _communicationService.serialnumber;
            
            //Create an empty page
            PdfPage page = _pdfDocument.AddPage();
            page.Height = 1000;
            page.Width = 700;
            
            XGraphics draw = XGraphics.FromPdfPage(page);

            //Create font
            XFont serialfont = new XFont("Roboto_Medium", 18, XFontStyle.Regular);
            XFont font = new XFont("Roboto_Medium", 12, XFontStyle.Regular);

            //create pen
            XPen pen = new XPen(XColors.Black, 1);
            XPen headerpen = new XPen(XColors.Blue, 3);
            
            if (_pdfVariables.outsideLimits[0] == 0)
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
            draw.DrawString("Model :", font, XBrushes.Black, PDFcoordinates.first_column, lineCounter);
            draw.DrawString(_communicationService.loggername, font, XBrushes.Black, PDFcoordinates.second_column, lineCounter);
            lineCounter += PDFcoordinates.line_inc;
            draw.DrawString("Logger State :", font, XBrushes.Black, PDFcoordinates.first_column, lineCounter);
            draw.DrawString(_pdfVariables.loggerState, font, XBrushes.Black, PDFcoordinates.second_column, lineCounter);
            lineCounter += PDFcoordinates.line_inc;
            draw.DrawString("Battery :", font, XBrushes.Black, PDFcoordinates.first_column, lineCounter);
            draw.DrawString(_pdfVariables.batteryPercentage, font, XBrushes.Black, PDFcoordinates.second_column, lineCounter);
            lineCounter += PDFcoordinates.line_inc;
            draw.DrawString("Sample Period :", font, XBrushes.Black, PDFcoordinates.first_column, lineCounter);
            draw.DrawString(_pdfVariables.sameplePeriod + " (hh:mm:ss)", font, XBrushes.Black, PDFcoordinates.second_column, lineCounter);
            lineCounter += PDFcoordinates.line_inc;
            draw.DrawString("Start Delay :", font, XBrushes.Black, PDFcoordinates.first_column, lineCounter);
            draw.DrawString(_pdfVariables.startDelay + " (hh:mm:ss)", font, XBrushes.Black, PDFcoordinates.second_column, lineCounter);
            lineCounter += PDFcoordinates.line_inc;
            draw.DrawString("First Sample :", font, XBrushes.Black, PDFcoordinates.first_column, lineCounter);
            draw.DrawString(_pdfVariables.firstSample, font, XBrushes.Black, PDFcoordinates.second_column, lineCounter);
            lineCounter += PDFcoordinates.line_inc;
            draw.DrawString("Last Sample :", font, XBrushes.Black, PDFcoordinates.first_column, lineCounter);
            draw.DrawString(_pdfVariables.lastSample, font, XBrushes.Black, PDFcoordinates.second_column, lineCounter);
            lineCounter += PDFcoordinates.line_inc;
            draw.DrawString("Recorded Samples :", font, XBrushes.Black, PDFcoordinates.first_column, lineCounter);
            draw.DrawString(_pdfVariables.recordedSample.ToString(), font, XBrushes.Black, PDFcoordinates.second_column, lineCounter);
            lineCounter += PDFcoordinates.line_inc;
            draw.DrawString("Tags Placed :", font, XBrushes.Black, PDFcoordinates.first_column, lineCounter);
            draw.DrawString("0", font, XBrushes.Black, PDFcoordinates.second_column, lineCounter);
            lineCounter += (PDFcoordinates.line_inc * 0.75);
            XRect break1 = new XRect(10, lineCounter, 680, 0);
            draw.DrawRectangle(pen, break1);
            lineCounter += PDFcoordinates.line_inc * 0.75;
            if (_pdfVariables.enabledChannels[0]) draw.DrawString("#1 - Temperature", font, XBrushes.Black, PDFcoordinates.second_column - 25, lineCounter);
            if (_pdfVariables.enabledChannels[1]) draw.DrawString("#2 - Humidity", font, XBrushes.Black, PDFcoordinates.third_column - 25, lineCounter);
            lineCounter += PDFcoordinates.line_inc;
            draw.DrawString("Preset Upper Limit :", font, XBrushes.Black, PDFcoordinates.first_column, lineCounter);
            if (_pdfVariables.enabledChannels[0]) draw.DrawString(_pdfVariables.presetUpperLimit[0].ToString("N2") + _pdfVariables.tempUnit + _pdfVariables.breachedAbove[0], font, XBrushes.Black, PDFcoordinates.second_column, lineCounter);
            if (_pdfVariables.enabledChannels[1]) draw.DrawString(_pdfVariables.presetUpperLimit[1].ToString("N2") + _pdfVariables.tempUnit + _pdfVariables.breachedAbove[1], font, XBrushes.Black, PDFcoordinates.third_column, lineCounter);
            lineCounter += PDFcoordinates.line_inc;
            draw.DrawString("Preset Lower Limit :", font, XBrushes.Black, PDFcoordinates.first_column, lineCounter);
            if (_pdfVariables.enabledChannels[0]) draw.DrawString(_pdfVariables.presetLowerLimit[0].ToString("N2") + _pdfVariables.tempUnit + _pdfVariables.breachedBelow[0], font, XBrushes.Black, PDFcoordinates.second_column, lineCounter);
            if (_pdfVariables.enabledChannels[1]) draw.DrawString(_pdfVariables.presetLowerLimit[1].ToString("N2") + _pdfVariables.tempUnit + _pdfVariables.breachedBelow[1], font, XBrushes.Black, PDFcoordinates.third_column, lineCounter);
            lineCounter += PDFcoordinates.line_inc;
            draw.DrawString("Mean Value :", font, XBrushes.Black, PDFcoordinates.first_column, lineCounter);
            if (_pdfVariables.enabledChannels[0]) draw.DrawString(_pdfVariables.Mean[0].ToString("N2") + _pdfVariables.tempUnit, font, XBrushes.Black, PDFcoordinates.second_column, lineCounter);
            if (_pdfVariables.enabledChannels[1]) draw.DrawString(_pdfVariables.Mean[1].ToString("N2") + _pdfVariables.tempUnit, font, XBrushes.Black, PDFcoordinates.third_column, lineCounter);
            lineCounter += PDFcoordinates.line_inc;
            draw.DrawString("MKT Value :", font, XBrushes.Black, PDFcoordinates.first_column, lineCounter);
            if (_pdfVariables.enabledChannels[0]) draw.DrawString(_pdfVariables.MKT_C[0].ToString("N2") + _pdfVariables.tempUnit, font, XBrushes.Black, PDFcoordinates.second_column, lineCounter);
            if (_pdfVariables.enabledChannels[1]) draw.DrawString(_pdfVariables.MKT_C[1].ToString("N2") + _pdfVariables.tempUnit, font, XBrushes.Black, PDFcoordinates.third_column, lineCounter);
            lineCounter += PDFcoordinates.line_inc;
            draw.DrawString("Max Recorded :", font, XBrushes.Black, PDFcoordinates.first_column, lineCounter);
            if (_pdfVariables.enabledChannels[0]) draw.DrawString(_pdfVariables.Max[0].ToString("N2") + _pdfVariables.tempUnit, font, XBrushes.Black, PDFcoordinates.second_column, lineCounter);
            if (_pdfVariables.enabledChannels[1]) draw.DrawString(_pdfVariables.Max[1].ToString("N2") + _pdfVariables.tempUnit, font, XBrushes.Black, PDFcoordinates.third_column, lineCounter);
            lineCounter += PDFcoordinates.line_inc;
            draw.DrawString("Min Recorded :", font, XBrushes.Black, PDFcoordinates.first_column, lineCounter);
            if (_pdfVariables.enabledChannels[0]) draw.DrawString(_pdfVariables.Min[0].ToString("N2") + _pdfVariables.tempUnit, font, XBrushes.Black, PDFcoordinates.second_column, lineCounter);
            if (_pdfVariables.enabledChannels[1]) draw.DrawString(_pdfVariables.Min[1].ToString("N2") + _pdfVariables.tempUnit, font, XBrushes.Black, PDFcoordinates.third_column, lineCounter);
            lineCounter += PDFcoordinates.line_inc;
            lineCounter += (PDFcoordinates.line_inc * 0.5);
            draw.DrawString("Total Samples within Limits :", font, XBrushes.Black, PDFcoordinates.first_column, lineCounter);
            if (_pdfVariables.enabledChannels[0]) draw.DrawString(_pdfVariables.withinLimits[0].ToString(), font, XBrushes.Black, PDFcoordinates.second_column, lineCounter);
            if (_pdfVariables.enabledChannels[1]) draw.DrawString(_pdfVariables.withinLimits[1].ToString(), font, XBrushes.Black, PDFcoordinates.third_column, lineCounter);
            lineCounter += PDFcoordinates.line_inc;
            draw.DrawString("Total Time within Limits :", font, XBrushes.Black, PDFcoordinates.first_column, lineCounter);
            if (_pdfVariables.enabledChannels[0]) draw.DrawString(_pdfVariables.timeWithinLimits[0], font, XBrushes.Black, PDFcoordinates.second_column, lineCounter);
            if (_pdfVariables.enabledChannels[1]) draw.DrawString(_pdfVariables.timeWithinLimits[1], font, XBrushes.Black, PDFcoordinates.third_column, lineCounter);
            lineCounter += PDFcoordinates.line_inc;
            lineCounter += (PDFcoordinates.line_inc * 0.5);
            draw.DrawString("Total Samples out of Limits :", font, XBrushes.Black, PDFcoordinates.first_column, lineCounter);
            if (_pdfVariables.enabledChannels[0]) draw.DrawString(_pdfVariables.outsideLimits[0].ToString("N1"), font, XBrushes.Black, PDFcoordinates.second_column, lineCounter);
            if (_pdfVariables.enabledChannels[1]) draw.DrawString(_pdfVariables.outsideLimits[1].ToString("N1"), font, XBrushes.Black, PDFcoordinates.third_column, lineCounter);
            lineCounter += PDFcoordinates.line_inc;
            draw.DrawString("Total Time out of Limits :", font, XBrushes.Black, PDFcoordinates.first_column, lineCounter);
            if (_pdfVariables.enabledChannels[0]) draw.DrawString(_pdfVariables.timeOutLimits[0], font, XBrushes.Black, PDFcoordinates.second_column, lineCounter);
            if (_pdfVariables.enabledChannels[1]) draw.DrawString(_pdfVariables.timeOutLimits[1], font, XBrushes.Black, PDFcoordinates.third_column, lineCounter);
            lineCounter += PDFcoordinates.line_inc;
            lineCounter += (PDFcoordinates.line_inc * 0.5);
            draw.DrawString("Samples above upper Limit :", font, XBrushes.Black, PDFcoordinates.first_column, lineCounter);
            if (_pdfVariables.enabledChannels[0]) draw.DrawString(_pdfVariables.aboveLimits[0].ToString("N1"), font, XBrushes.Black, PDFcoordinates.second_column, lineCounter);
            if (_pdfVariables.enabledChannels[1]) draw.DrawString(_pdfVariables.aboveLimits[1].ToString("N1"), font, XBrushes.Black, PDFcoordinates.third_column, lineCounter);
            lineCounter += PDFcoordinates.line_inc;
            draw.DrawString("Time above Upper Limit :", font, XBrushes.Black, PDFcoordinates.first_column, lineCounter);
            if (_pdfVariables.enabledChannels[0]) draw.DrawString(_pdfVariables.timeAboveLimits[0], font, XBrushes.Black, PDFcoordinates.second_column, lineCounter);
            if (_pdfVariables.enabledChannels[1]) draw.DrawString(_pdfVariables.timeAboveLimits[1], font, XBrushes.Black, PDFcoordinates.third_column, lineCounter);
            lineCounter += PDFcoordinates.line_inc;
            lineCounter += (PDFcoordinates.line_inc * 0.5);
            draw.DrawString("Samples below Lower Limit :", font, XBrushes.Black, PDFcoordinates.first_column, lineCounter);
            if (_pdfVariables.enabledChannels[0]) draw.DrawString(_pdfVariables.belowLimits[0].ToString("N1"), font, XBrushes.Black, PDFcoordinates.second_column, lineCounter);
            if (_pdfVariables.enabledChannels[1]) draw.DrawString(_pdfVariables.belowLimits[1].ToString("N1"), font, XBrushes.Black, PDFcoordinates.third_column, lineCounter);
            lineCounter += PDFcoordinates.line_inc;
            draw.DrawString("Time below Lower Limit :", font, XBrushes.Black, PDFcoordinates.first_column, lineCounter);
            if (_pdfVariables.enabledChannels[0]) draw.DrawString(_pdfVariables.timeBelowLimits[0], font, XBrushes.Black, PDFcoordinates.second_column, lineCounter);
            if (_pdfVariables.enabledChannels[1]) draw.DrawString(_pdfVariables.timeBelowLimits[1], font, XBrushes.Black, PDFcoordinates.third_column, lineCounter);
            lineCounter += PDFcoordinates.line_inc;
            lineCounter += (PDFcoordinates.line_inc * 0.5);
            draw.DrawString("User Comments :", font, XBrushes.Black, PDFcoordinates.first_column, lineCounter);
            lineCounter += PDFcoordinates.line_inc;
            draw.DrawString(_pdfVariables.userData, font, XBrushes.Black, PDFcoordinates.first_column, lineCounter);
            lineCounter += PDFcoordinates.line_inc * 0.5;
            XRect break2 = new XRect(10, lineCounter, 680, 0);
            draw.DrawRectangle(pen, break2);
            lineCounter += PDFcoordinates.line_inc * 0.75;
            if (_pdfVariables.enabledChannels[0]) draw.DrawString("_ Temperature " + _pdfVariables.tempUnit, font, XBrushes.DarkOliveGreen, PDFcoordinates.second_column, lineCounter);
            if (_pdfVariables.enabledChannels[1]) draw.DrawString("_ Humidity  %RH ", font, XBrushes.MediumPurple, PDFcoordinates.second_column + 120, lineCounter);
            lineCounter += PDFcoordinates.line_inc;
            
            //Draw graph
            DrawGraph(draw, pen, font);
            
            //Header/Footer
            draw.DrawString("Logger Report", serialfont, XBrushes.Blue, 10, 50);
            draw.DrawString("S/N: " + _communicationService.serialnumber, serialfont, XBrushes.Blue, 550, 50);
            draw.DrawRectangle(headerpen, new XRect(10, 60, 680, 0));
            draw.DrawString("Comment ", font, XBrushes.Black, PDFcoordinates.commentX, PDFcoordinates.commentY);
            draw.DrawString("Signature ", font, XBrushes.Black, PDFcoordinates.sigX, PDFcoordinates.sigY);
            draw.DrawString("Page 1/1 ", font, XBrushes.Black, 600, 980);
            draw.DrawString("www.temprecord.com", font, XBrushes.Black, PDFcoordinates.siteX, PDFcoordinates.siteY);
            draw.DrawString(DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:sss UTC"), font, XBrushes.Black, PDFcoordinates.dateX, PDFcoordinates.dateY);
            draw.DrawString("0.1.9.1", font, XBrushes.Black, PDFcoordinates.versionX, PDFcoordinates.versionY);

            string filename = _communicationService.serialnumber + ".pdf";
            _pdfDocument.Save(filename);
            Process.Start(filename); //Previews PDF
        }

        private void DrawGraph (XGraphics draw, XPen pen, XFont font)
        {
            XPen ch0 = new XPen(XColors.DarkGreen);
            XPen ch1 = new XPen(XColors.Lavender);
            XPen withinlimits = new XPen(XColors.ForestGreen);
            withinlimits.DashStyle = XDashStyle.Dash;
            XPen abovelimit = new XPen(XColors.Coral);
            abovelimit.DashStyle = XDashStyle.Dash;
            XPen belowlimit = new XPen(XColors.CornflowerBlue);
            belowlimit.DashStyle = XDashStyle.Dash;

            double ch1_upper_L = _pdfVariables.presetUpperLimit[0];
            double ch1_lower_L = _pdfVariables.presetLowerLimit[0];
            double ch1_max = _pdfVariables.Max[0];
            double ch1_min = _pdfVariables.Min[0];
            double ch1_highest = 0;
            double ch1_lowest = 0;
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

            int i = _pdfVariables.recordedSample / 5;
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

            if (_pdfVariables.enabledChannels[0])  //First sensor
            {
                if (ch1_upper_L > ch1_max)
                {
                    ch1_highest = ch1_upper_L;
                }
                else
                {
                    ch1_highest = ch1_max;
                }

                if (ch1_lower_L < ch1_min)
                {
                    ch1_lowest = ch1_lower_L;
                }
                else
                {
                    ch1_lowest = ch1_min;
                }
            }

            if (_pdfVariables.enabledChannels[1]) //Second Sensor
            {

                ch2_upper_L = _pdfVariables.presetUpperLimit[1];
                ch2_lower_L = _pdfVariables.presetLowerLimit[1];
                ch2_max = _pdfVariables.Max[1];
                ch2_min = _pdfVariables.Min[1];

                if (ch2_upper_L > ch2_max)
                {
                    ch2_highest = ch2_upper_L;
                }
                else
                {
                    ch2_highest = ch2_max;
                }

                if (ch2_lower_L < ch2_min)
                {
                    ch2_lowest = ch2_lower_L;
                }
                else
                {
                    ch2_lowest = ch2_min;
                }


                if (ch1_highest > ch2_highest)
                {
                    ch_highest = ch1_highest;
                }
                else
                {
                    ch_highest = ch2_highest;
                }

                if (ch1_lowest < ch2_lowest)
                {
                    ch_lowest = ch1_lowest;
                }
                else
                {
                    ch_lowest = ch2_lowest;
                }
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
            graph_x_scale = (float)PDFcoordinates.graph_W / _pdfVariables.recordedSample;
            
            while (i < _pdfVariables.recordedSample)
            {
                graph_date_x = (graph_x_scale * i) + graph_topX;
                draw.DrawString(_pdfVariables.UNIXtoUTCDate(Convert.ToInt32(_pdfVariables.Time[i])), font, XBrushes.Black, graph_date_x - 40, PDFcoordinates.G_axis_meetY + 15);
                draw.DrawString(_pdfVariables.UNIXtoUTCTime(Convert.ToInt32(_pdfVariables.Time[i])), font, XBrushes.Black, graph_date_x - 45, PDFcoordinates.G_axis_meetY + 28);
                i += gap;
            }

            if (_pdfVariables.enabledChannels[1] && _pdfVariables.recordedSample > 0)
            {
                ch2_upper_Y = (PDFcoordinates.graph_H - ((ch2_upper_L - ch_lowest) * graph_y_scale)) + PDFcoordinates.graph_topY;
                ch2_lower_Y = (PDFcoordinates.graph_H - ((ch2_lower_L - ch_lowest) * graph_y_scale)) + PDFcoordinates.graph_topY;
                ch2_max = (float)(PDFcoordinates.graph_H - ((_pdfVariables.Max[1] - ch_lowest) * graph_y_scale)) + PDFcoordinates.graph_topY;
                ch2_min = (float)(PDFcoordinates.graph_H - ((_pdfVariables.Min[1] - ch_lowest) * graph_y_scale)) + PDFcoordinates.graph_topY;

                draw.DrawString("__ Humidity %RH ", font, XBrushes.Black, PDFcoordinates.second_column + 120, PDFcoordinates.graph_topY - 5);
                draw.DrawString(" %RH Upper Limit ", font, XBrushes.Black, PDFcoordinates.third_column, (float)ch2_upper_Y - PDFcoordinates.graph_limit_label);
                draw.DrawString(ch2_upper_L.ToString("N2"), font, XBrushes.Black, PDFcoordinates.first_column, (float)ch2_upper_Y);
                draw.DrawString(" %RH Lower Limit ", font, XBrushes.Black, PDFcoordinates.third_column, (float)ch2_lower_Y + PDFcoordinates.graph_limit_label + 5);
                draw.DrawString(ch2_lower_L.ToString("N2"), font, XBrushes.Black, PDFcoordinates.first_column, (float)ch2_lower_Y);
                draw.DrawString(_pdfVariables.Mean[1].ToString("N2"), font, XBrushes.Black, PDFcoordinates.first_column, (float)(PDFcoordinates.graph_H - (((_pdfVariables.Mean[1] - ch_lowest) * graph_y_scale))) + PDFcoordinates.graph_topY);
                draw.DrawLine(pen, PDFcoordinates.graph_l_lineX_start, (float)ch2_lower_Y, PDFcoordinates.graph_l_lineX_end, (float)ch2_lower_Y);
                draw.DrawLine(pen, PDFcoordinates.graph_l_lineX_start, (float)ch2_upper_Y, PDFcoordinates.graph_l_lineX_end, (float)ch2_upper_Y);
                draw.DrawLine(ch1, PDFcoordinates.graph_l_lineX_start, ch2_max, PDFcoordinates.graph_l_lineX_end, ch2_max);
                draw.DrawLine(ch1, PDFcoordinates.graph_l_lineX_start, ch2_min, PDFcoordinates.graph_l_lineX_end, ch2_min);
            }


            if (_pdfVariables.enabledChannels[0] && _pdfVariables.recordedSample > 0)
            {
                temp_next_y = (float)(PDFcoordinates.graph_H - (_pdfVariables.Data[0] - (ch_lowest)) * graph_y_scale) + PDFcoordinates.graph_topY;
                ch1_upper_Y = (PDFcoordinates.graph_H - ((ch1_upper_L - ch_lowest) * graph_y_scale)) + PDFcoordinates.graph_topY;
                ch1_lower_Y = (PDFcoordinates.graph_H - ((ch1_lower_L - ch_lowest) * graph_y_scale)) + PDFcoordinates.graph_topY;
                ch1_max = (float)(PDFcoordinates.graph_H - ((_pdfVariables.Max[0] - ch_lowest) * graph_y_scale)) + PDFcoordinates.graph_topY;
                ch1_min = (float)(PDFcoordinates.graph_H - ((_pdfVariables.Min[0] - ch_lowest) * graph_y_scale)) + PDFcoordinates.graph_topY;

                draw.DrawString(_pdfVariables.tempUnit + " Upper Limit ", font, XBrushes.Coral, PDFcoordinates.third_column, (float)ch1_upper_Y - PDFcoordinates.graph_limit_label);
                draw.DrawString(ch1_upper_L.ToString("N2"), font, XBrushes.Black, PDFcoordinates.first_column, (float)ch1_upper_Y);
                draw.DrawString(_pdfVariables.tempUnit + " Lower Limit ", font, XBrushes.CornflowerBlue, PDFcoordinates.third_column, (float)ch1_lower_Y + PDFcoordinates.graph_limit_label + 5);
                draw.DrawString(ch1_lower_L.ToString("N2"), font, XBrushes.Black, PDFcoordinates.first_column, (float)ch1_lower_Y);
                draw.DrawString(_pdfVariables.Mean[0].ToString("N2"), font, XBrushes.Black,PDFcoordinates.first_column,(float)(PDFcoordinates.graph_H - ((_pdfVariables.Mean[0] - ch_lowest) * graph_y_scale)) + PDFcoordinates.graph_topY);
                draw.DrawLine(belowlimit, PDFcoordinates.graph_l_lineX_start, (float)ch1_lower_Y, PDFcoordinates.graph_l_lineX_end, (float)ch1_lower_Y);
                draw.DrawLine(abovelimit, PDFcoordinates.graph_l_lineX_start, (float)ch1_upper_Y, PDFcoordinates.graph_l_lineX_end, (float)ch1_upper_Y);
                draw.DrawLine(withinlimits, PDFcoordinates.graph_l_lineX_start, ch1_max, PDFcoordinates.graph_l_lineX_end, ch1_max);
                draw.DrawLine(withinlimits, PDFcoordinates.graph_l_lineX_start, ch1_min, PDFcoordinates.graph_l_lineX_end, ch1_min);
            }
            
            while (k < _pdfVariables.recordedSample)
            {
                if (_pdfVariables.enabledChannels[0])
                {
                    draw.DrawLine(ch0, graph_topX, temp_next_y, graph_topX + graph_x_scale, (float)(PDFcoordinates.graph_H - ((_pdfVariables.Data[k] - (ch_lowest)) * graph_y_scale)) + PDFcoordinates.graph_topY);
                    temp_next_y = (float)(PDFcoordinates.graph_H - ((_pdfVariables.Data[k] - (ch_lowest)) * graph_y_scale)) + PDFcoordinates.graph_topY;
                }

                if (_pdfVariables.enabledChannels[1])
                {
                    //draw.DrawLine(ch1, x_point_loc, rh_next_y, x_point_loc + graph_x_scale, (float)(PDFcoordinates.graph_H - ((_pdfVariables.Data[1][k] - (ch_lowest)) * graph_y_scale)) + PDFcoordinates.graph_topY);
                    //rh_next_y = (float)(PDFcoordinates.graph_H - ((_pdfVariables.Data[1][k] - (ch_lowest)) * graph_y_scale)) + PDFcoordinates.graph_topY;
                }

                graph_topX += graph_x_scale;
                k++;
            }
        }
    }
}
