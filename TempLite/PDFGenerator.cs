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
        private PdfDocument document = new PdfDocument();
        private PDFcoordinates information = new PDFcoordinates();
        private PDFvariables _pdfVariables = new PDFvariables();

        public void createPDF(_communicationServices _communicationService)
        {
            new HexfileDecoder(_communicationService, _pdfVariables);
            document.Info.Title = _communicationService.serialnumber;
            
            //Create an empty page
            PdfPage page = document.AddPage();
            page.Height = 1000;
            page.Width = 700;

            //get XGrapgics object for drawing
            XGraphics draw = XGraphics.FromPdfPage(page);

            //Create font
            XFont serialfont = new XFont("Roboto_Medium", 18, XFontStyle.Regular);
            XFont font = new XFont("Roboto_Medium", 12, XFontStyle.Regular);

            //create pen
            XPen pen = new XPen(XColors.Black, 1);
            XPen headerpen = new XPen(XColors.Blue, 3);
            XPen ch0 = new XPen(XColors.DarkGreen);
            XPen ch1 = new XPen(XColors.Lavender);
            XPen withinlimits = new XPen(XColors.ForestGreen);
            withinlimits.DashStyle = XDashStyle.Dash;
            XPen abovelimit = new XPen(XColors.Coral);
            abovelimit.DashStyle = XDashStyle.Dash;
            XPen belowlimit = new XPen(XColors.CornflowerBlue);
            belowlimit.DashStyle = XDashStyle.Dash;

            //create rectange
            XRect headerline = new XRect(10, 60, 680, 0);

            //Draw Image
            if (_pdfVariables.outsideLimits[0] == 0)
            {
                XImage greentick = XImage.FromFile("greentick.png");
                draw.DrawImage(greentick, information.sign_left, information.sign_top, 90, 80);
                draw.DrawString("Within Limits", font, XBrushes.Black, information.limitinfo_startX, information.limitinfo_startY);
            }
            else
            {
                XImage redwarning = XImage.FromFile("redwarning.png");
                draw.DrawImage(redwarning, information.sign_left, information.sign_top, 90, 80);
                draw.DrawString("Limits Exceeded", font, XBrushes.Black, information.limitinfo_startX, information.limitinfo_startY);
            }
            XImage logo = XImage.FromFile("logo.png");
            draw.DrawImage(logo, 320, 10, 65, 40);

            //Draw the Text
            draw.DrawString("Logger Report", serialfont, XBrushes.Blue, 10, 50);
            draw.DrawString("S/N: " + _communicationService.serialnumber, serialfont, XBrushes.Blue, 550, 50);
            draw.DrawRectangle(headerpen, headerline);

            draw.DrawString("Model :", font, XBrushes.Black, information.first_column, information.line_counter);
            draw.DrawString(_communicationService.loggername, font, XBrushes.Black, information.second_column, information.line_counter);
            information.line_counter += information.line_inc;
            draw.DrawString("Logger State :", font, XBrushes.Black, information.first_column, information.line_counter);
            draw.DrawString(_pdfVariables.loggerState, font, XBrushes.Black, information.second_column, information.line_counter);
            information.line_counter += information.line_inc;
            draw.DrawString("Battery :", font, XBrushes.Black, information.first_column, information.line_counter);
            draw.DrawString(_pdfVariables.batteryPercentage, font, XBrushes.Black, information.second_column, information.line_counter);
            information.line_counter += information.line_inc;
            draw.DrawString("Sample Period :", font, XBrushes.Black, information.first_column, information.line_counter);
            draw.DrawString(_pdfVariables.sameplePeriod + " (hh:mm:ss)", font, XBrushes.Black, information.second_column, information.line_counter);
            information.line_counter += information.line_inc;
            draw.DrawString("Start Delay :", font, XBrushes.Black, information.first_column, information.line_counter);
            draw.DrawString(_pdfVariables.startDelay + " (hh:mm:ss)", font, XBrushes.Black, information.second_column, information.line_counter);
            information.line_counter += information.line_inc;
            draw.DrawString("First Sample :", font, XBrushes.Black, information.first_column, information.line_counter);
            draw.DrawString(_pdfVariables.firstSample, font, XBrushes.Black, information.second_column, information.line_counter);
            information.line_counter += information.line_inc;
            draw.DrawString("Last Sample :", font, XBrushes.Black, information.first_column, information.line_counter);
            draw.DrawString(_pdfVariables.lastSample, font, XBrushes.Black, information.second_column, information.line_counter);
            information.line_counter += information.line_inc;
            draw.DrawString("Recorded Samples :", font, XBrushes.Black, information.first_column, information.line_counter);
            draw.DrawString(_pdfVariables.recordedSample.ToString(), font, XBrushes.Black, information.second_column, information.line_counter);
            information.line_counter += information.line_inc;
            draw.DrawString("Tags Placed :", font, XBrushes.Black, information.first_column, information.line_counter);
            draw.DrawString("0", font, XBrushes.Black, information.second_column, information.line_counter);
            information.line_counter += (information.line_inc*0.75);
            XRect break1 = new XRect(10, information.line_counter, 680, 0);
            draw.DrawRectangle(pen, break1);
            information.line_counter += information.line_inc*0.75;
            if (_pdfVariables.enabledChannels[0]) draw.DrawString("#1 - Temperature", font, XBrushes.Black, information.second_column - 25, information.line_counter);
            if (_pdfVariables.enabledChannels[1]) draw.DrawString("#2 - Humidity", font, XBrushes.Black, information.third_column - 25, information.line_counter);
            information.line_counter += information.line_inc;
            draw.DrawString("Preset Upper Limit :", font, XBrushes.Black, information.first_column, information.line_counter);
            if (_pdfVariables.enabledChannels[0]) draw.DrawString(_pdfVariables.presetUpperLimit[0].ToString("N2") + _pdfVariables.tempUnit + _pdfVariables.breachedAbove[0], font, XBrushes.Black, information.second_column, information.line_counter);
            if (_pdfVariables.enabledChannels[1]) draw.DrawString(_pdfVariables.presetUpperLimit[1].ToString("N2") + _pdfVariables.tempUnit + _pdfVariables.breachedAbove[1], font, XBrushes.Black, information.third_column, information.line_counter);
            information.line_counter += information.line_inc;
            draw.DrawString("Preset Lower Limit :", font, XBrushes.Black, information.first_column, information.line_counter);
            if (_pdfVariables.enabledChannels[0]) draw.DrawString(_pdfVariables.presetLowerLimit[0].ToString("N2") + _pdfVariables.tempUnit + _pdfVariables.breachedBelow[0], font, XBrushes.Black, information.second_column, information.line_counter);
            if (_pdfVariables.enabledChannels[1]) draw.DrawString(_pdfVariables.presetLowerLimit[1].ToString("N2") + _pdfVariables.tempUnit + _pdfVariables.breachedBelow[1], font, XBrushes.Black, information.third_column, information.line_counter);
            information.line_counter += information.line_inc;
            draw.DrawString("Mean Value :", font, XBrushes.Black, information.first_column, information.line_counter);
            if (_pdfVariables.enabledChannels[0]) draw.DrawString(_pdfVariables.Mean[0].ToString("N2") + _pdfVariables.tempUnit, font, XBrushes.Black, information.second_column, information.line_counter);
            if (_pdfVariables.enabledChannels[1]) draw.DrawString(_pdfVariables.Mean[1].ToString("N2") + _pdfVariables.tempUnit, font, XBrushes.Black, information.third_column, information.line_counter);
            information.line_counter += information.line_inc;
            draw.DrawString("MKT Value :", font, XBrushes.Black, information.first_column, information.line_counter);
            if (_pdfVariables.enabledChannels[0]) draw.DrawString(_pdfVariables.MKT_C[0].ToString("N2") + _pdfVariables.tempUnit, font, XBrushes.Black, information.second_column, information.line_counter);
            if (_pdfVariables.enabledChannels[1]) draw.DrawString(_pdfVariables.MKT_C[1].ToString("N2") + _pdfVariables.tempUnit, font, XBrushes.Black, information.third_column, information.line_counter);
            information.line_counter += information.line_inc;
            draw.DrawString("Max Recorded :", font, XBrushes.Black, information.first_column, information.line_counter);
            if (_pdfVariables.enabledChannels[0]) draw.DrawString(_pdfVariables.Max[0].ToString("N2") + _pdfVariables.tempUnit, font, XBrushes.Black, information.second_column, information.line_counter);
            if (_pdfVariables.enabledChannels[1]) draw.DrawString(_pdfVariables.Max[1].ToString("N2") + _pdfVariables.tempUnit, font, XBrushes.Black, information.third_column, information.line_counter);
            information.line_counter += information.line_inc;
            draw.DrawString("Min Recorded :", font, XBrushes.Black, information.first_column, information.line_counter);
            if (_pdfVariables.enabledChannels[0]) draw.DrawString(_pdfVariables.Min[0].ToString("N2") + _pdfVariables.tempUnit, font, XBrushes.Black, information.second_column, information.line_counter);
            if (_pdfVariables.enabledChannels[1]) draw.DrawString(_pdfVariables.Min[1].ToString("N2") + _pdfVariables.tempUnit, font, XBrushes.Black, information.third_column, information.line_counter);
            information.line_counter += information.line_inc;
            information.line_counter += (information.line_inc * 0.5);
            draw.DrawString("Total Samples within Limits :", font, XBrushes.Black, information.first_column, information.line_counter);
            if (_pdfVariables.enabledChannels[0]) draw.DrawString(_pdfVariables.withinLimits[0].ToString(), font, XBrushes.Black, information.second_column, information.line_counter);
            if (_pdfVariables.enabledChannels[1]) draw.DrawString(_pdfVariables.withinLimits[1].ToString(), font, XBrushes.Black, information.third_column, information.line_counter);
            information.line_counter += information.line_inc;
            draw.DrawString("Total Time within Limits :", font, XBrushes.Black, information.first_column, information.line_counter);
            if (_pdfVariables.enabledChannels[0]) draw.DrawString(_pdfVariables.timeWithinLimits[0] , font, XBrushes.Black, information.second_column, information.line_counter);
            if (_pdfVariables.enabledChannels[1]) draw.DrawString(_pdfVariables.timeWithinLimits[1], font, XBrushes.Black, information.third_column, information.line_counter);
            information.line_counter += information.line_inc;
            information.line_counter += (information.line_inc * 0.5);
            draw.DrawString("Total Samples out of Limits :", font, XBrushes.Black, information.first_column, information.line_counter);
            if (_pdfVariables.enabledChannels[0]) draw.DrawString(_pdfVariables.outsideLimits[0].ToString(), font, XBrushes.Black, information.second_column, information.line_counter);
            if (_pdfVariables.enabledChannels[1]) draw.DrawString(_pdfVariables.outsideLimits[1].ToString(), font, XBrushes.Black, information.third_column, information.line_counter);
            information.line_counter += information.line_inc;
            draw.DrawString("Total Time out of Limits :", font, XBrushes.Black, information.first_column, information.line_counter);
            if (_pdfVariables.enabledChannels[0]) draw.DrawString(_pdfVariables.timeOutLimits[0], font, XBrushes.Black, information.second_column, information.line_counter);
            if (_pdfVariables.enabledChannels[1]) draw.DrawString(_pdfVariables.timeOutLimits[1], font, XBrushes.Black, information.third_column, information.line_counter);
            information.line_counter += information.line_inc;
            information.line_counter += (information.line_inc * 0.5);
            draw.DrawString("Samples above upper Limit :", font, XBrushes.Black, information.first_column, information.line_counter);
            if (_pdfVariables.enabledChannels[0]) draw.DrawString(_pdfVariables.aboveLimits[0].ToString(), font, XBrushes.Black, information.second_column, information.line_counter);
            if (_pdfVariables.enabledChannels[1]) draw.DrawString(_pdfVariables.aboveLimits[1].ToString(), font, XBrushes.Black, information.third_column, information.line_counter);
            information.line_counter += information.line_inc;
            draw.DrawString("Time above Upper Limit :", font, XBrushes.Black, information.first_column, information.line_counter);
            if (_pdfVariables.enabledChannels[0]) draw.DrawString(_pdfVariables.timeAboveLimits[0], font, XBrushes.Black, information.second_column, information.line_counter);
            if (_pdfVariables.enabledChannels[1]) draw.DrawString(_pdfVariables.timeAboveLimits[1], font, XBrushes.Black, information.third_column, information.line_counter);
            information.line_counter += information.line_inc;
            information.line_counter += (information.line_inc * 0.5);
            draw.DrawString("Samples below Lower Limit :", font, XBrushes.Black, information.first_column, information.line_counter);
            if (_pdfVariables.enabledChannels[0]) draw.DrawString(_pdfVariables.belowLimits[0].ToString(), font, XBrushes.Black, information.second_column, information.line_counter);
            if (_pdfVariables.enabledChannels[1]) draw.DrawString(_pdfVariables.belowLimits[1].ToString(), font, XBrushes.Black, information.third_column, information.line_counter);
            information.line_counter += information.line_inc;
            draw.DrawString("Time below Lower Limit :", font, XBrushes.Black, information.first_column, information.line_counter);
            if (_pdfVariables.enabledChannels[0]) draw.DrawString(_pdfVariables.timeBelowLimits[0], font, XBrushes.Black, information.second_column, information.line_counter);
            if (_pdfVariables.enabledChannels[1]) draw.DrawString(_pdfVariables.timeBelowLimits[1], font, XBrushes.Black, information.third_column, information.line_counter);
            information.line_counter += information.line_inc;
            information.line_counter += (information.line_inc * 0.5);
            draw.DrawString("User Comments :", font, XBrushes.Black, information.first_column, information.line_counter);
            information.line_counter += information.line_inc;
            draw.DrawString(_pdfVariables.userData, font, XBrushes.Black, information.first_column, information.line_counter);
            information.line_counter += information.line_inc * 0.5;
            XRect break2 = new XRect(10, information.line_counter, 680, 0);
            draw.DrawRectangle(pen, break2);
            information.line_counter += information.line_inc * 0.75;
            if (_pdfVariables.enabledChannels[0]) draw.DrawString("_ Temperature " + _pdfVariables.tempUnit, font, XBrushes.DarkOliveGreen, information.second_column, information.line_counter);
            if (_pdfVariables.enabledChannels[1]) draw.DrawString("_ Humidity  %RH ", font, XBrushes.MediumPurple, information.second_column + 120, information.line_counter);
            information.line_counter += information.line_inc;

            draw.DrawString("Comment ", font, XBrushes.Black, information.commentX, information.commentY);
            draw.DrawString("Signature ", font, XBrushes.Black, information.sigX, information.sigY);

            //================================================================================================//

            //========================================== Draw the boxes on the first page=== =================//
            //drawing the box around the warning and tick sign
            draw.DrawLine(pen, information.box1_X1, information.box1_Y1, information.box1_X2, information.box1_Y1);
            draw.DrawLine(pen, information.box1_X1, information.box1_Y2, information.box1_X2, information.box1_Y2);
            draw.DrawLine(pen, information.box1_X1, information.box1_Y1, information.box1_X1, information.box1_Y2);
            draw.DrawLine(pen, information.box1_X2, information.box1_Y1, information.box1_X2, information.box1_Y2);

            //drawing the box for the comments at the bottom of first page
            draw.DrawLine(pen, information.box2_X1, information.box2_Y1, information.box2_X2, information.box2_Y1);
            draw.DrawLine(pen, information.box2_X1, information.box2_Y2, information.box2_X2, information.box2_Y2);
            draw.DrawLine(pen, information.box2_X1, information.box2_Y1, information.box2_X1, information.box2_Y2);
            draw.DrawLine(pen, information.box2_X2, information.box2_Y1, information.box2_X2, information.box2_Y2);

            //drawing the box for the signature at the bottom of the first page
            draw.DrawLine(pen, information.box3_X1, information.box3_Y1, information.box3_X2, information.box3_Y1);
            draw.DrawLine(pen, information.box3_X1, information.box3_Y2, information.box3_X2, information.box3_Y2);
            draw.DrawLine(pen, information.box3_X1, information.box3_Y1, information.box3_X1, information.box3_Y2);
            draw.DrawLine(pen, information.box3_X2, information.box3_Y1, information.box3_X2, information.box3_Y2);
            //================================================================================================//
            
            //=========================== HEADER / FOOTER ========================================//
            //draw.DrawLine(pen, 10, 80, 690, 80);
            draw.DrawString("Page 1 ", font, XBrushes.Black, 600, 980);
            draw.DrawString("www.temprecord.com", font, XBrushes.Black, information.siteX, information.siteY);
            draw.DrawString(DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:sss UTC"), font, XBrushes.Black, information.dateX, information.dateY);
            draw.DrawString("0.1.9.1", font, XBrushes.Black, information.versionX, information.versionY);
            //=====================================================================================//


            //========================================== Plotting graph ======================================//
            
            //calculating the highest and lowest values from the temperature and humidity values
            //Helps to decide the scale of the graph
            if (_pdfVariables.enabledChannels[0])  //First sensor
            {
                information.ch1_upper_L = _pdfVariables.presetUpperLimit[0];
                information.ch1_lower_L = _pdfVariables.presetLowerLimit[0];
                information.ch1_max = _pdfVariables.Max[0];
                information.ch1_min = _pdfVariables.Min[0];


                if (information.ch1_upper_L > information.ch1_max)
                {
                    information.ch1_highest = information.ch1_upper_L;
                }
                else
                {
                    information.ch1_highest = information.ch1_max;
                }

                if (information.ch1_lower_L < information.ch1_min)
                {
                    information.ch1_lowest = information.ch1_lower_L;
                }
                else
                {
                    information.ch1_lowest = information.ch1_min;
                }
            }

            if (_pdfVariables.enabledChannels[1]) //Second Sensor
            {

                information.ch2_upper_L = _pdfVariables.presetUpperLimit[1];
                information.ch2_lower_L = _pdfVariables.presetLowerLimit[1];
                information.ch2_max = _pdfVariables.Max[1];
                information.ch2_min = _pdfVariables.Min[1];

                if (information.ch2_upper_L > information.ch2_max)
                {
                    information.ch2_highest = information.ch2_upper_L;
                }
                else
                {
                    information.ch2_highest = information.ch2_max;
                }

                if (information.ch2_lower_L < information.ch2_min)
                {
                    information.ch2_lowest = information.ch2_lower_L;
                }
                else
                {
                    information.ch2_lowest = information.ch2_min;
                }


                if (information.ch1_highest > information.ch2_highest)
                {
                    information.ch_highest = information.ch1_highest;
                }
                else
                {
                    information.ch_highest = information.ch2_highest;
                }

                if (information.ch1_lowest < information.ch2_lowest)
                {
                    information.ch_lowest = information.ch1_lowest;
                }
                else
                {
                    information.ch_lowest = information.ch2_lowest;
                }
            }
            else
            {
                information.ch_highest = information.ch1_highest;
                information.ch_lowest = information.ch1_lowest;
            }

            //=========================== Drawing the axis for the graph =========================//
            draw.DrawLine(pen,information.G_axis_startX, information.G_axis_startY, information.G_axis_meetX, information.G_axis_meetY);//y axis
            draw.DrawLine(pen,information.G_axis_meetX, information.G_axis_meetY, information.G_axis_endX, information.G_axis_endY);//x axis
            information.graph_y_scale = (float)((information.graph_H - 20) / (information.ch_highest - information.ch_lowest));
            information.graph_x_scale = (float)information.graph_W / _pdfVariables.recordedSample;

            //
            if (_pdfVariables.enabledChannels[1] && _pdfVariables.recordedSample > 0)
            {
                // ================================= ch2 upper limit =============================//
                information.ch2_upper_Y = (information.graph_H - ((information.ch2_upper_L - information.ch_lowest) * information.graph_y_scale)) + information.graph_topY;

                // ================================= ch2 lower limit =============================//
                information.ch2_lower_Y = (information.graph_H - ((information.ch2_lower_L - information.ch_lowest) * information.graph_y_scale)) + information.graph_topY;


                // ================================= Draw the line for the upper limit ===========//
                draw.DrawLine(pen,information.graph_l_lineX_start, (float)information.ch2_upper_Y, information.graph_l_lineX_end, (float)information.ch2_upper_Y);
                // ================================= Draw the text ===============================//
                draw.DrawString(" %RH Upper Limit ", font, XBrushes.Black, information.third_column, (float)information.ch2_upper_Y - information.graph_limit_label);
                draw.DrawString(information.ch2_upper_L.ToString("N2"), font, XBrushes.Black, information.first_column, (float)information.ch2_upper_Y);//printing the values of limits at the y axis upper limit


                // ================================= Draw the line for the lower limit ===========//
                draw.DrawLine(pen,information.graph_l_lineX_start, (float)information.ch2_lower_Y, information.graph_l_lineX_end, (float)information.ch2_lower_Y);
                // ================================= Draw the text ===============================//
                draw.DrawString(" %RH Lower Limit ", font, XBrushes.Black, information.third_column, (float)information.ch2_lower_Y + information.graph_limit_label + 5 );
                draw.DrawString(information.ch2_lower_L.ToString("N2"), font, XBrushes.Black, information.first_column, (float)information.ch2_lower_Y);//printing the values of limits at the y axis lower limit


                //============================= mean value text ==================================//
                draw.DrawString(_pdfVariables.Mean[1].ToString("N2"), font, XBrushes.Black, information.first_column, (float)(information.graph_H - (((_pdfVariables.Mean[1] - information.ch_lowest) * information.graph_y_scale))) + information.graph_topY);//mean value of the humidity line

                //==================== max min dash line for channel 2 ===========================//
                float ch2_max = (float)(information.graph_H - ((_pdfVariables.Max[1] - information.ch_lowest) * information.graph_y_scale)) + information.graph_topY;
                draw.DrawLine(ch1, information.graph_l_lineX_start, ch2_max, information.graph_l_lineX_end, ch2_max);
                //draw.drawText(String.valueOf(mt2Mem_values.ch1Stats.Max.Value / 10.0),information.graph_l_lineX_end,ch2_max,information.max_min);


                float ch2_min = (float)(information.graph_H - ((_pdfVariables.Min[1] - information.ch_lowest) * information.graph_y_scale)) + information.graph_topY;
                draw.DrawLine(ch1, information.graph_l_lineX_start, ch2_min, information.graph_l_lineX_end, ch2_min);
                //draw.drawText(String.valueOf(mt2Mem_values.ch1Stats.Min.Value / 10.0),information.graph_l_lineX_end,ch2_min,information.max_min);

                // ============================= Draw label on top of the graph ==================//
                draw.DrawString("__ Humidity %RH ", font, XBrushes.Black, information.second_column + 120, information.graph_topY - 5);
            }


            if (_pdfVariables.enabledChannels[0] && _pdfVariables.recordedSample > 0)                                                                  //plotting temperature channel 1 limit line and labeling them and labeling the axis
            {

                // ================================= initial y location on the graph =============//
                information.temp_next_y = (float)(information.graph_H - (_pdfVariables.Data[0] - (information.ch_lowest)) * information.graph_y_scale) + information.graph_topY;

                // ================================= ch2 upper limit =============================//
                information.ch1_upper_Y = (information.graph_H - ((information.ch1_upper_L - information.ch_lowest) * information.graph_y_scale)) + information.graph_topY;

                // ================================= ch2 lower limit =============================//
                information.ch1_lower_Y = (information.graph_H - ((information.ch1_lower_L - information.ch_lowest) * information.graph_y_scale)) + information.graph_topY;


                // ================================= Draw the line for the upper limit ===========//
                draw.DrawLine(abovelimit, information.graph_l_lineX_start, (float)information.ch1_upper_Y, information.graph_l_lineX_end, (float)information.ch1_upper_Y);
                // ================================= Draw the text ===============================//
                draw.DrawString(_pdfVariables.tempUnit + " Upper Limit ", font, XBrushes.Coral, information.third_column, (float)information.ch1_upper_Y - information.graph_limit_label);
                draw.DrawString(information.ch1_upper_L.ToString("N2"), font, XBrushes.Black, information.first_column, (float)information.ch1_upper_Y);


                // ================================= Draw the line for the lower limit ===========//
                draw.DrawLine(belowlimit, information.graph_l_lineX_start, (float)information.ch1_lower_Y, information.graph_l_lineX_end, (float)information.ch1_lower_Y);
                // ================================= Draw the text ===============================//
                draw.DrawString(_pdfVariables.tempUnit + " Lower Limit ", font, XBrushes.CornflowerBlue, information.third_column, (float)information.ch1_lower_Y + information.graph_limit_label + 5);
                draw.DrawString(information.ch1_lower_L.ToString("N2"),font,XBrushes.Black, information.first_column, (float)information.ch1_lower_Y);


                //============================= mean value text ==================================//
                draw.DrawString(_pdfVariables.Mean[0].ToString("N2"), font, XBrushes.Black,
                        information.first_column,
                        (float)(information.graph_H - ((_pdfVariables.Mean[0] - information.ch_lowest) * information.graph_y_scale)) + information.graph_topY);

                //==================== max min dash line for channel 1 ===========================//
                float ch1_max = (float)(information.graph_H - ((_pdfVariables.Max[0] - information.ch_lowest) * information.graph_y_scale)) + information.graph_topY;
                draw.DrawLine(withinlimits, information.graph_l_lineX_start, ch1_max, information.graph_l_lineX_end, ch1_max);
                // draw.drawText(String.format("%.1f",((my_unit) ? mt2Mem_values.ch0Stats.Max.Value / 10.0 : QS.returnFD(mt2Mem_values.ch0Stats.Max.Value / 10.0))),information.graph_l_lineX_end,ch1_max,information.max_min);
                float ch1_min = (float)(information.graph_H - ((_pdfVariables.Min[0] - information.ch_lowest) * information.graph_y_scale)) + information.graph_topY;
                draw.DrawLine(withinlimits, information.graph_l_lineX_start, ch1_min, information.graph_l_lineX_end, ch1_min);
                // draw.drawText(String.format("%.1f",((my_unit) ? mt2Mem_values.ch0Stats.Min.Value / 10.0 : QS.returnFD(mt2Mem_values.ch0Stats.Min.Value / 10.0))),information.graph_l_lineX_end,ch1_min,information.max_min);

               
            }
            int k = 0;
            information.x_point_loc = information.graph_topX;
            // =========== loop which actually draws the temperature and humidity lines ==========//
            while (k < _pdfVariables.recordedSample)
            {
                if (_pdfVariables.enabledChannels[0])                                                              //Channel 1 graph
                {
                    draw.DrawLine(ch0,information.x_point_loc, information.temp_next_y, information.x_point_loc + information.graph_x_scale, (float)(information.graph_H - ((_pdfVariables.Data[k] - (information.ch_lowest)) * information.graph_y_scale)) + information.graph_topY);
                    information.temp_next_y = (float)(information.graph_H - ((_pdfVariables.Data[k] - (information.ch_lowest)) * information.graph_y_scale)) + information.graph_topY;
                }

                if (_pdfVariables.enabledChannels[1])                                                              //Channel 2 graph
                {
                    //draw.DrawLine(information.x_point_loc, information.rh_next_y, information.x_point_loc + information.graph_x_scale, (float)(information.graph_H - ((ChannelValues.get(1).get(k) - (information.ch_lowest)) * information.graph_y_scale)) + information.graph_topY, information.rh_line);
                    //information.rh_next_y = (float)(information.graph_H - ((ChannelValues.get(1).get(k) - (information.ch_lowest)) * information.graph_y_scale)) + information.graph_topY;
                }

                information.x_point_loc += information.graph_x_scale;
                k++;
            }

            //=========================== drawing the date and time on the x axis ================//
            int gap = _pdfVariables.recordedSample / 5;
            int i = gap;
            if (gap <= 0) gap = 1;
            while (i < _pdfVariables.recordedSample)
            {
                information.graph_date_x = (information.graph_x_scale * i) + information.graph_topX;
                //draw.DrawEllipse(pen, ((information.graph_x_scale * i) + information.graph_topX), information.G_axis_meetY, ((information.graph_x_scale * i) + information.graph_topX), information.G_axis_meetY);
                draw.DrawString(_pdfVariables.UNIXtoUTCDate(Convert.ToInt32(_pdfVariables.Time[i])), font, XBrushes.Black, information.graph_date_x - 40, information.G_axis_meetY + 15);
                draw.DrawString(_pdfVariables.UNIXtoUTCTime(Convert.ToInt32(_pdfVariables.Time[i])), font, XBrushes.Black, information.graph_date_x - 45, information.G_axis_meetY + 28);
                i += gap;
            }
            string filename = _communicationService.serialnumber + ".pdf";
            document.Save(filename);
            Process.Start(filename);
        }
    }
}
