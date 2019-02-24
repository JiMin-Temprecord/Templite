using System;
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
        private double[] SamplingPeriods = { 0, 0, 0, 0, 0, 0, 0, 0 };
        private string[] SamplesNumber = { "", "", "", "", "", "", "", "" };
        private bool[] WithinUpper = { true, true, true, true, true, true, true, true };
        private bool[] WithinLower = { true, true, true, true, true, true, true, true };
        private int WithinUpperCounter = 0;
        private int WithinLowerCounter = 0;
        private double[] PresetUpperLimit = { 0, 0, 0, 0, 0, 0, 0, 0 };
        private double[] PresetLowerLimit = { 0, 0, 0, 0, 0, 0, 0, 0 };
        private double[] Mean = { 0, 0, 0, 0, 0, 0, 0, 0 };
        private double[] Max = { 0, 0, 0, 0, 0, 0, 0, 0 };
        private double[] Min = { 0, 0, 0, 0, 0, 0, 0, 0 };
        private double[] MKT_C = { 0, 0, 0, 0, 0, 0, 0, 0 };
        private String State = "";
        private String SerialNumber = "";
        private String UserData = "";
        private String UserDataLength = "";
        private String TempUnit = "";
        private String StartDelay = "";
        private String BatteryPercentage = "";
        private String FirstSample = "";
        private String LastSample = "";

        private double[] WithinLimits = { 0, 0, 0, 0, 0, 0, 0, 0 };
        private double[] OutsideLimits = { 0, 0, 0, 0, 0, 0, 0, 0 };
        private double[] AboveLimits = { 0, 0, 0, 0, 0, 0, 0, 0 };
        private double[] BelowLimits = { 0, 0, 0, 0, 0, 0, 0, 0 };

        private decodeHEX _decodeHex;
        private PdfDocument document = new PdfDocument();
        private info.infomation information = new info.infomation();

        //================================================================================================//


        public void getPDF(_communicationServices _communicationService)
        {
            _decodeHex = new decodeHEX(_communicationService);
            document.Info.Title = _communicationService.serialnumber;
            
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
            if (OutsideLimits[0] == 0)
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
            draw.DrawString("S/N: " + _communicationService.serialnumber, serialfont, XBrushes.Blue, 550, 70);
            draw.DrawRectangle(headerpen, headerline);

            draw.DrawString("Model :", font, XBrushes.Black, information.first_column, information.line_counter);
            draw.DrawString(_communicationService.loggername, font, XBrushes.Black, information.second_column, information.line_counter);
            information.line_counter += information.line_inc;
            draw.DrawString("Logger State :", font, XBrushes.Black, information.first_column, information.line_counter);
            draw.DrawString(State, font, XBrushes.Black, information.second_column, information.line_counter);
            information.line_counter += information.line_inc;
            draw.DrawString("Battery :", font, XBrushes.Black, information.first_column, information.line_counter);
            draw.DrawString(BatteryPercentage, font, XBrushes.Black, information.second_column, information.line_counter);
            information.line_counter += information.line_inc;
            draw.DrawString("Sample Period :", font, XBrushes.Black, information.first_column, information.line_counter);
            draw.DrawString(HHMMSS(SamplingPeriods[0]), font, XBrushes.Black, information.second_column, information.line_counter);
            information.line_counter += information.line_inc;
            draw.DrawString("Start Delay :", font, XBrushes.Black, information.first_column, information.line_counter);
            draw.DrawString(StartDelay, font, XBrushes.Black, information.second_column, information.line_counter);
            information.line_counter += information.line_inc;
            draw.DrawString("First Sample :", font, XBrushes.Black, information.first_column, information.line_counter);
            draw.DrawString(FirstSample, font, XBrushes.Black, information.second_column, information.line_counter);
            information.line_counter += information.line_inc;
            draw.DrawString("Last Sample :", font, XBrushes.Black, information.first_column, information.line_counter);
            draw.DrawString(LastSample, font, XBrushes.Black, information.second_column, information.line_counter);
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
            if (EnabledChannels[0]) draw.DrawString(PresetUpperLimit[0].ToString("0.##") + TempUnit, font, XBrushes.Black, information.second_column, information.line_counter);
            if (EnabledChannels[1]) draw.DrawString(PresetUpperLimit[1].ToString("0.##") + TempUnit, font, XBrushes.Black, information.third_column, information.line_counter);
            information.line_counter += information.line_inc;
            draw.DrawString("Preset Lower Limit :", font, XBrushes.Black, information.first_column, information.line_counter);
            if (EnabledChannels[0]) draw.DrawString(PresetLowerLimit[0].ToString("0.##") + TempUnit, font, XBrushes.Black, information.second_column, information.line_counter);
            if (EnabledChannels[1]) draw.DrawString(PresetLowerLimit[1].ToString("0.##") + TempUnit, font, XBrushes.Black, information.third_column, information.line_counter);
            information.line_counter += information.line_inc;
            draw.DrawString("Mean Value :", font, XBrushes.Black, information.first_column, information.line_counter);
            if (EnabledChannels[0]) draw.DrawString(Mean[0].ToString("0.##") + TempUnit, font, XBrushes.Black, information.second_column, information.line_counter);
            if (EnabledChannels[1]) draw.DrawString(Mean[1].ToString("0.##") + TempUnit, font, XBrushes.Black, information.third_column, information.line_counter);
            information.line_counter += information.line_inc;
            draw.DrawString("MKT Value :", font, XBrushes.Black, information.first_column, information.line_counter);
            if (EnabledChannels[0]) draw.DrawString(MKT_C[0].ToString("0.##") + TempUnit, font, XBrushes.Black, information.second_column, information.line_counter);
            if (EnabledChannels[1]) draw.DrawString(MKT_C[1].ToString("0.##") + TempUnit, font, XBrushes.Black, information.third_column, information.line_counter);
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
            if (EnabledChannels[0]) draw.DrawString((HHMMSS(WithinLimits[0]*SamplingPeriods[0])), font, XBrushes.Black, information.second_column, information.line_counter);
            if (EnabledChannels[1]) draw.DrawString((HHMMSS(WithinLimits[1] * SamplingPeriods[1])), font, XBrushes.Black, information.third_column, information.line_counter);
            information.line_counter += information.line_inc;
            information.line_counter += information.line_inc;
            draw.DrawString("Total Samples out of Limits :", font, XBrushes.Black, information.first_column, information.line_counter);
            if (EnabledChannels[0]) draw.DrawString(OutsideLimits[0].ToString(), font, XBrushes.Black, information.second_column, information.line_counter);
            if (EnabledChannels[1]) draw.DrawString(OutsideLimits[1].ToString(), font, XBrushes.Black, information.third_column, information.line_counter);
            information.line_counter += information.line_inc;
            draw.DrawString("Total Time out of Limits :", font, XBrushes.Black, information.first_column, information.line_counter);
            if (EnabledChannels[0]) draw.DrawString((HHMMSS(OutsideLimits[0] * SamplingPeriods[0])), font, XBrushes.Black, information.second_column, information.line_counter);
            if (EnabledChannels[1]) draw.DrawString((HHMMSS(OutsideLimits[1] * SamplingPeriods[1])), font, XBrushes.Black, information.third_column, information.line_counter);
            information.line_counter += information.line_inc;
            information.line_counter += information.line_inc;
            draw.DrawString("Samples above upper Limit :", font, XBrushes.Black, information.first_column, information.line_counter);
            if (EnabledChannels[0]) draw.DrawString(AboveLimits[0].ToString(), font, XBrushes.Black, information.second_column, information.line_counter);
            if (EnabledChannels[1]) draw.DrawString(AboveLimits[1].ToString(), font, XBrushes.Black, information.third_column, information.line_counter);
            information.line_counter += information.line_inc;
            draw.DrawString("Time above Upper Limit :", font, XBrushes.Black, information.first_column, information.line_counter);
            if (EnabledChannels[0]) draw.DrawString((HHMMSS(AboveLimits[0] * SamplingPeriods[0])), font, XBrushes.Black, information.second_column, information.line_counter);
            if (EnabledChannels[1]) draw.DrawString((HHMMSS(AboveLimits[1] * SamplingPeriods[1])), font, XBrushes.Black, information.third_column, information.line_counter);
            information.line_counter += information.line_inc;
            information.line_counter += information.line_inc;
            draw.DrawString("Samples below Lower Limit :", font, XBrushes.Black, information.first_column, information.line_counter);
            if (EnabledChannels[0]) draw.DrawString(BelowLimits[0].ToString(), font, XBrushes.Black, information.second_column, information.line_counter);
            if (EnabledChannels[1]) draw.DrawString(BelowLimits[1].ToString(), font, XBrushes.Black, information.third_column, information.line_counter);
            information.line_counter += information.line_inc;
            draw.DrawString("Time below Lower Limit :", font, XBrushes.Black, information.first_column, information.line_counter);
            if (EnabledChannels[0]) draw.DrawString((HHMMSS(BelowLimits[0] * SamplingPeriods[0])), font, XBrushes.Black, information.second_column, information.line_counter);
            if (EnabledChannels[1]) draw.DrawString((HHMMSS(BelowLimits[1] * SamplingPeriods[1])), font, XBrushes.Black, information.third_column, information.line_counter);
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


        private void writetovariable()
        {
            NumberChannel = Int32.Parse(_decodeHex.readJson("SENSOR,SensorNumber"));
            State = _decodeHex.readJson("HEADER,State");
            SerialNumber = _decodeHex.readJson("HEADER,SerialNumber");
            BatteryPercentage = _decodeHex.readJson("BATTERY_INFO,Battery") + "%";
            SamplingPeriods[0] = Convert.ToInt32(_decodeHex.readJson("USER_SETTINGS,SamplingPeriod"),16);
            StartDelay = HHMMSS(Convert.ToInt32(_decodeHex.readJson("USER_SETTINGS,StartDelay"),16));
            UserData = _decodeHex.readJson("USER_DATA,UserData");
            SamplesNumber[0] = _decodeHex.readJson("DATA_INFO,SamplesNumber");

            FirstSample = _decodeHex.readJson("DATA_INFO,Time_FirstSample_MonT");
            LastSample = _decodeHex.readJson("DATA_INFO,LastSampleAt");

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
            
            for (int i = 0; i < NumberChannel; i++)
            {
                PresetLowerLimit[i] = _decodeHex.m_lower_limit[i];
                PresetUpperLimit[i] = _decodeHex.m_upper_limit[i];
                Mean[i] = _decodeHex.m_mean[i];
                Max[i] = _decodeHex.m_sensor_max[i];
                Min[i] = _decodeHex.m_sensor_min[i];
                MKT_C[i] = _decodeHex.m_MKT[i]- 273.15;
                WithinLimits[i] = _decodeHex.m_nb_within_limit[i];
                OutsideLimits[i] = _decodeHex.m_nb_above_limit[i] + _decodeHex.m_nb_below_limit[i];
                AboveLimits[i] = _decodeHex.m_nb_above_limit[i];
                BelowLimits[i] = _decodeHex.m_nb_below_limit[i];
            }
        }

        public String HHMMSS(double mseconds)
        {
            int hours = (int)mseconds / 3600;
            int minutes = (int)(mseconds % 3600) / 60;
            int seconds = (int)mseconds % 60;

            String timeString = (hours.ToString("00") + ":" + minutes.ToString("00") + ":" + seconds.ToString("00") + " (hh:mm:ss)");
            return timeString;
        }
    }
}
