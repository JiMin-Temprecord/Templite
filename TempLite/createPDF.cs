﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    public class createPDF
    {
        //================================================================================================//
        //New Variables

        //private ArrayList<ArrayList<Double>> ChannelValues = new ArrayList<ArrayList<Double>>();
        //private ArrayList<Long> Time = new ArrayList<Long>();
        private long StartTimeUNIX = 0;
        private int NumberChannel = 0; //NumberChannel
        private bool[] EnabledChannels = { false, false, false, false, false, false, false, false };
        private bool Fahrenheit = false;
        private string[] SamplingPeriods = { "", "", "", "", "", "", "", "" };
        private int[] SamplesNumber = { 0, 0, 0, 0, 0, 0, 0, 0 };
        private bool[] WithinUpper = { true, true, true, true, true, true, true, true };
        private bool[] WithinLower = { true, true, true, true, true, true, true, true };
        private int WithinUpperCounter = 0;
        private int WithinLowerCounter = 0;
        private string[] PresetUpperLimit = { "", "", "", "", "", "", "", "" };
        private string[] PresetLowerLimit = { "", "", "", "", "", "", "", "" };
        private string[] Mean = { "", "", "", "", "", "", "", "" };
        private string[] Max = { "", "", "", "", "", "", "", "" };
        private string[] Min = { "", "", "", "", "", "", "", "" };
        private string[] MKT_C = { "", "", "", "", "", "", "", "" };
        private String State = "";
        private String SerialNumber = "";
        private String UserData = "";
        private String UserDataLength = "";
        private String TempUnit = "";
        private String StartDelay = "";
        private String BatteryPercentage = "";

        private double[] WithinLimits = { 0, 0, 0, 0, 0, 0, 0, 0 };
        private double[] OutsideLimits = { 0, 0, 0, 0, 0, 0, 0, 0 };
        private double[] AboveLimits = { 0, 0, 0, 0, 0, 0, 0, 0 };
        private double[] BelowLimits = { 0, 0, 0, 0, 0, 0, 0, 0 };



        //================================================================================================//


        private void writetovariable()
        {
            NumberChannel = Int32.Parse(createJSON.readJson("SENSOR,SensorNumber"));
            State = createJSON.readJson("HEADER,State");
            SerialNumber = createJSON.readJson("HEADER,SerialNumber");
            BatteryPercentage = createJSON.readJson("BATTERY_INFO,Battery") + "%";
            SamplingPeriods[0] = createJSON.readJson("USER_SETTINGS,SamplingPeriod");
            StartDelay = createJSON.readJson("USER_SETTINGS,StartDelay");
            UserData = createJSON.readJson("USER_DATA,UserData");

            for (int i = 0; i < NumberChannel; i++)
            {
                EnabledChannels[i] = true;
            }

            // Get if its Fahrenheit or celsius
            //Fahrenheit = Convert.ToBoolean(createJSON.readJson("USER_SETTINGS,Fahrenheit"));

            if (Fahrenheit)
                TempUnit = " °F";
            else
                TempUnit = " °C";

            PresetLowerLimit[0] =  createJSON.readJson("CHANNEL_INFO,LowerLimit");
            PresetUpperLimit[0] =  createJSON.readJson("CHANNEL_INFO,UpperLimit");
            
            Console.WriteLine("LAST SAMEPLE AT : " + createJSON.readJson("DATA_INFO,LastSampleAt"));
            Console.WriteLine("UPPER T LIMIT EXCEEDED : " + createJSON.readJson("DATA_INFO,LastSampleAt"));
            Console.WriteLine("Lower T LIMIT COUNTER : " + createJSON.readJson("CHANNEL_INFO,LowerLimitCounter"));
            Console.WriteLine("UPPER T LIMIT COUNTER : " + createJSON.readJson("CHANNEL_INFO,UpperLimitCounter"));
            Console.WriteLine("TICKS AT LAST SAMPLE : " + createJSON.readJson("DATA_INFO,TicksAtLastSample"));
            Console.WriteLine("RECORDED SAMPLES : " + createJSON.readJson("DATA_INFO,TicksAtLastSample"));

            /*for (int i = 0; i < NumberChannel; i++)
            {
                PresetLowerLimit[i] =
                PresetUpperLimit[i] =
                Mean[i] =
                Max[i] =
                Min[i] =
                MKT_C[i] =
                WithinLimits[i] =
                OutsideLimits[i] =
                AboveLimits[i] =
                BelowLimits[i] = 
            }*/
        }

        public void getPDF(_communicationServices _communicationService)
        {
            PdfDocument document = new PdfDocument();
            document.Info.Title = _communicationService.serialnumber.ToString();
            info.infomation information = new info.infomation();
            writetovariable();

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
            XPen ch0 = new XPen(XColors.DarkOliveGreen);
            XPen ch1 = new XPen(XColors.Lavender);

            //create rectange
            XRect headerline = new XRect(10, 80, 680, 0);

            //Draw Image
            if (WithinUpperCounter == 0 && WithinLowerCounter == 0)
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
            draw.DrawString("Logger Report", serialfont, XBrushes.Blue, 10, 70);
            draw.DrawString("S/N: " + _communicationService.serialnumber.ToString(), serialfont, XBrushes.Blue, 550, 70);
            draw.DrawRectangle(headerpen, headerline);

            draw.DrawString("Model :", font, XBrushes.Black, information.first_column, information.line_counter);
            draw.DrawString(_communicationService.loggername.ToString(), font, XBrushes.Black, information.second_column, information.line_counter);
            information.line_counter += information.line_inc;
            draw.DrawString("Logger State :", font, XBrushes.Black, information.first_column, information.line_counter);
            draw.DrawString(State, font, XBrushes.Black, information.second_column, information.line_counter);
            information.line_counter += information.line_inc;
            draw.DrawString("Battery :", font, XBrushes.Black, information.first_column, information.line_counter);
            draw.DrawString(BatteryPercentage, font, XBrushes.Black, information.second_column, information.line_counter);
            information.line_counter += information.line_inc;
            draw.DrawString("Sample Period :", font, XBrushes.Black, information.first_column, information.line_counter);
            draw.DrawString(SamplingPeriods[0].ToString(), font, XBrushes.Black, information.second_column, information.line_counter);
            information.line_counter += information.line_inc;
            draw.DrawString("Start Delay :", font, XBrushes.Black, information.first_column, information.line_counter);
            draw.DrawString(StartDelay, font, XBrushes.Black, information.second_column, information.line_counter);
            information.line_counter += information.line_inc;
            draw.DrawString("First Sample :", font, XBrushes.Black, information.first_column, information.line_counter);
            information.line_counter += information.line_inc;
            draw.DrawString("Last Sample :", font, XBrushes.Black, information.first_column, information.line_counter);
            information.line_counter += information.line_inc;
            draw.DrawString("Recorded Samples :", font, XBrushes.Black, information.first_column, information.line_counter);
            draw.DrawString(SamplesNumber[0].ToString(), font, XBrushes.Black, information.second_column, information.line_counter);
            information.line_counter += information.line_inc;
            draw.DrawString("Tags Placed :", font, XBrushes.Black, information.first_column, information.line_counter);
            information.line_counter += information.line_inc;
            XRect break1 = new XRect(10, information.line_counter, 680, 0);
            draw.DrawRectangle(pen, break1);
            information.line_counter += information.line_inc;
            if (EnabledChannels[0]) draw.DrawString("#1 - Temperature", font, XBrushes.Black, information.second_column - 25, information.line_counter);
            if (EnabledChannels[1]) draw.DrawString("#2 - Humidity", font, XBrushes.Black, information.third_column - 25, information.line_counter);
            information.line_counter += information.line_inc;
            draw.DrawString("Preset Upper Limit :", font, XBrushes.Black, information.first_column, information.line_counter);
            if (EnabledChannels[0]) draw.DrawString(PresetUpperLimit[0].ToString() + TempUnit, font, XBrushes.Black, information.second_column, information.line_counter);
            if (EnabledChannels[1]) draw.DrawString(PresetUpperLimit[1].ToString() + TempUnit, font, XBrushes.Black, information.third_column, information.line_counter);
            information.line_counter += information.line_inc;
            draw.DrawString("Preset Lower Limit :", font, XBrushes.Black, information.first_column, information.line_counter);
            if (EnabledChannels[0]) draw.DrawString(PresetLowerLimit[0].ToString() + TempUnit, font, XBrushes.Black, information.second_column, information.line_counter);
            if (EnabledChannels[1]) draw.DrawString(PresetLowerLimit[1].ToString() + TempUnit, font, XBrushes.Black, information.third_column, information.line_counter);
            information.line_counter += information.line_inc;
            draw.DrawString("Mean Value :", font, XBrushes.Black, information.first_column, information.line_counter);
            if (EnabledChannels[0]) draw.DrawString(Mean[0].ToString() + TempUnit, font, XBrushes.Black, information.second_column, information.line_counter);
            if (EnabledChannels[1]) draw.DrawString(Mean[1].ToString() + TempUnit, font, XBrushes.Black, information.third_column, information.line_counter);
            information.line_counter += information.line_inc;
            draw.DrawString("MKT Value :", font, XBrushes.Black, information.first_column, information.line_counter);
            if (EnabledChannels[0]) draw.DrawString(MKT_C[0].ToString() + TempUnit, font, XBrushes.Black, information.second_column, information.line_counter);
            if (EnabledChannels[1]) draw.DrawString(MKT_C[1].ToString() + TempUnit, font, XBrushes.Black, information.third_column, information.line_counter);
            information.line_counter += information.line_inc;
            draw.DrawString("Max Recorded :", font, XBrushes.Black, information.first_column, information.line_counter);
            if (EnabledChannels[0]) draw.DrawString(Max[0].ToString() + TempUnit, font, XBrushes.Black, information.second_column, information.line_counter);
            if (EnabledChannels[1]) draw.DrawString(Max[1].ToString() + TempUnit, font, XBrushes.Black, information.third_column, information.line_counter);
            information.line_counter += information.line_inc;
            draw.DrawString("Min Recorded :", font, XBrushes.Black, information.first_column, information.line_counter);
            if (EnabledChannels[0]) draw.DrawString(Min[0].ToString() + TempUnit, font, XBrushes.Black, information.second_column, information.line_counter);
            if (EnabledChannels[1]) draw.DrawString(Min[1].ToString() + TempUnit, font, XBrushes.Black, information.third_column, information.line_counter);
            information.line_counter += information.line_inc;
            information.line_counter += information.line_inc;
            draw.DrawString("Total Samples within Limits :", font, XBrushes.Black, information.first_column, information.line_counter);
            if (EnabledChannels[0]) draw.DrawString(WithinLimits[0].ToString(), font, XBrushes.Black, information.second_column, information.line_counter);
            if (EnabledChannels[1]) draw.DrawString(WithinLimits[1].ToString(), font, XBrushes.Black, information.third_column, information.line_counter);
            information.line_counter += information.line_inc;
            draw.DrawString("Total Time within Limits :", font, XBrushes.Black, information.first_column, information.line_counter);
            if (EnabledChannels[0]) draw.DrawString(WithinLimits[0].ToString(), font, XBrushes.Black, information.second_column, information.line_counter);
            if (EnabledChannels[1]) draw.DrawString(WithinLimits[1].ToString(), font, XBrushes.Black, information.third_column, information.line_counter);
            information.line_counter += information.line_inc;
            information.line_counter += information.line_inc;
            draw.DrawString("Total Samples out of Limits :", font, XBrushes.Black, information.first_column, information.line_counter);
            if (EnabledChannels[0]) draw.DrawString(OutsideLimits[0].ToString(), font, XBrushes.Black, information.second_column, information.line_counter);
            if (EnabledChannels[1]) draw.DrawString(OutsideLimits[1].ToString(), font, XBrushes.Black, information.third_column, information.line_counter);
            information.line_counter += information.line_inc;
            draw.DrawString("Total Time out of Limits :", font, XBrushes.Black, information.first_column, information.line_counter);
            if (EnabledChannels[0]) draw.DrawString(OutsideLimits[0].ToString(), font, XBrushes.Black, information.second_column, information.line_counter);
            if (EnabledChannels[1]) draw.DrawString(OutsideLimits[1].ToString(), font, XBrushes.Black, information.third_column, information.line_counter);
            information.line_counter += information.line_inc;
            information.line_counter += information.line_inc;
            draw.DrawString("Samples above upper Limit :", font, XBrushes.Black, information.first_column, information.line_counter);
            if (EnabledChannels[0]) draw.DrawString(AboveLimits[0].ToString(), font, XBrushes.Black, information.second_column, information.line_counter);
            if (EnabledChannels[1]) draw.DrawString(AboveLimits[1].ToString(), font, XBrushes.Black, information.third_column, information.line_counter);
            information.line_counter += information.line_inc;
            draw.DrawString("Time above Upper Limit :", font, XBrushes.Black, information.first_column, information.line_counter);
            if (EnabledChannels[0]) draw.DrawString(AboveLimits[0].ToString(), font, XBrushes.Black, information.second_column, information.line_counter);
            if (EnabledChannels[1]) draw.DrawString(AboveLimits[1].ToString(), font, XBrushes.Black, information.third_column, information.line_counter);
            information.line_counter += information.line_inc;
            information.line_counter += information.line_inc;
            draw.DrawString("Samples below Lower Limit :", font, XBrushes.Black, information.first_column, information.line_counter);
            if (EnabledChannels[0]) draw.DrawString(BelowLimits[0].ToString(), font, XBrushes.Black, information.second_column, information.line_counter);
            if (EnabledChannels[1]) draw.DrawString(BelowLimits[1].ToString(), font, XBrushes.Black, information.third_column, information.line_counter);
            information.line_counter += information.line_inc;
            draw.DrawString("Time below Lower Limit :", font, XBrushes.Black, information.first_column, information.line_counter);
            if (EnabledChannels[0]) draw.DrawString(BelowLimits[0].ToString(), font, XBrushes.Black, information.second_column, information.line_counter);
            if (EnabledChannels[1]) draw.DrawString(BelowLimits[1].ToString(), font, XBrushes.Black, information.third_column, information.line_counter);
            information.line_counter += information.line_inc;
            information.line_counter += information.line_inc;
            draw.DrawString("User Comments :", font, XBrushes.Black, information.first_column, information.line_counter);
            information.line_counter += information.line_inc;
            draw.DrawString(UserData, font, XBrushes.Black, information.first_column, information.line_counter);
            information.line_counter += information.line_inc;
            XRect break2 = new XRect(10, information.line_counter, 680, 0);
            draw.DrawRectangle(pen, break2);
            information.line_counter += information.line_inc;
            draw.DrawString("_ Temperature " + TempUnit, font, XBrushes.DarkOliveGreen, information.second_column, information.line_counter);
            draw.DrawString("_ Humidity  %RH ", font, XBrushes.MediumPurple, information.second_column + 120, information.line_counter);
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

            //=========================== Drawing the axis for the graph =========================//
            //XRect graph = new XRect(55, 585, 610, 270);
            //draw.DrawRectangle(pen, graph);
            draw.DrawLine(pen, information.G_axis_startX, information.line_counter, information.G_axis_meetX, information.G_axis_meetY);//y axis
            draw.DrawLine(pen, information.G_axis_meetX, information.G_axis_meetY, information.G_axis_endX, information.G_axis_endY);//x axis


            //=========================== HEADER / FOOTER ========================================//
            draw.DrawLine(pen, 10, 80, 690, 80);
            draw.DrawString("Page 1 ", font, XBrushes.Black, 600, 980);
            draw.DrawString("www.temprecord.com", font, XBrushes.Black, information.siteX, information.siteY);
            draw.DrawString(DateTime.UtcNow.ToShortDateString() + " " + DateTime.UtcNow.ToLongTimeString(), font, XBrushes.Black, information.dateX, information.dateY);
            draw.DrawString("0.1.9.1", font, XBrushes.Black, information.versionX, information.versionY);
            //=====================================================================================//
            string filename = _communicationService.serialnumber.ToString() + ".pdf";
            document.Save(filename);
            Process.Start(filename);
        }
    }
}
