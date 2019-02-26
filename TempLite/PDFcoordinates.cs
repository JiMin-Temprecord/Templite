using PdfSharp.Charting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TempLite
{
    public class PDFcoordinates
    {
         public String TEMP_LINECOLOR = "#0000FF";
        public String TEMP_LINETHICKNESS = "1";
        public String TEMP_LINETYPE = "solid";
        public String TEMP_LIMITLINECOLOR = "FF0000";
        public String TEMP_LIMITLINETHICKNESS = "1";
        public String TEMP_LIMITLINETYPE = "dash";
        public String TEMP_MARKERCOLOR = "#191919";
        public String TEMP_MARKERSIZE = "5";
        public String TEMP_MARKERTYPE = "triangle";
        
        public String HUM_LINECOLOR = "#FFA500";
        public String HUM_LINETHICKNESS = "1";
        public String HUM_LINETYPE = "solid";
        public String HUM_LIMITLINECOLOR = "FF0000";
        public String HUM_LIMITLINETHICKNESS = "1";
        public String HUM_LIMITLINETYPE = "dash";
        public String HUM_MARKERCOLOR = "#191919";
        public String HUM_MARKERSIZE = "5";
        public String HUM_MARKERTYPE = "triangle";
        
        public String TAG_COLOR = "#FFFFFF";
        public String TAG_SIZE = "5";
        public String TAG_TYPE = "triangle";
         
        public int first_column = 20;
        public int second_column = 275;
        public int third_column = 550;
        
        public double line_counter = 80;
        public double line_inc = 17;
        public int section_inc = 25;
        
        public int limitinfo_startX = 557;
        public int limitinfo_startY = 210;

        public int sign_left = 550;
        public int sign_top = 110;
        public int sign_right = 640;
        public int sign_bottom = 190;
 
        public int line2_startX = 10;
        public int line2_startY = 230;
        public int line2_endX = 690;
        public int line2_endY = 230;

        public int line3_startX = 10;
        public int line3_startY = 567;
        public int line3_endX = 690;
        public int line3_endY = 567;

        public int box1_X1 = 490;
        public int box1_X2 = 690;
        public int box1_Y1 = 100;
        public int box1_Y2 = 226;
 
        public int box2_X1 = 10;
        public int box2_X2 = 480;
        public int box2_Y1 = 900;
        public int box2_Y2 = 960;
  
        public int box3_X1 = 490;
        public int box3_X2 = 690;
        public int box3_Y1 = 900;
        public int box3_Y2 = 960;

        public int commentX = 15;
        public int commentY = 915;
 
        public int sigX = 500;
        public int sigY = 915;

        public int siteX = 300;
        public int siteY = 980;

        public int dateX = 555;
        public int dateY = 25;

        public int versionX = 10;
        public int versionY = 980;

        public int graph_topX = 55;
        public int graph_topY = 585;
        public int graph_W = 610;
        public int graph_H = 270;

        public int graph_brush_thickness = 1;
        public int graph_limit_label = 5;

        public double ch1_upper_L = 0;
        public double ch1_max = 0;
        public double ch1_lower_L = 0;
        public double ch1_min = 0;
        public double ch1_highest = 0;
        public double ch1_lowest = 0;

        public double ch2_upper_L = 0;
        public double ch2_max = 0;
        public double ch2_lower_L = 0;
        public double ch2_min = 0;
        public double ch2_highest = 0;
        public double ch2_lowest = 0;

        public double ch_highest = 0;
        public double ch_lowest = 0;

        public double ch1_upper_Y = 0;
        public double ch1_lower_Y = 0;

        public double ch2_upper_Y = 0;
        public double ch2_lower_Y = 0;

        public float graph_y_scale = 0;
        public float graph_x_scale = 0;

        public int graph_l_lineX_start = 55;
        public int graph_l_lineX_end = 55 + 610;

        public float temp_next_y = 0;
        public float rh_next_y = 0;

        public float x_point_loc = 0;
        public float graph_date_x = 0;

        public int G_axis_startX = 55;
        public int G_axis_startY = 585;
        public int G_axis_meetX = 55;
        public int G_axis_meetY = 585 + 270 + 10;
        public int G_axis_endX = 55 + 610;
        public int G_axis_endY = 585 + 270 + 10;
    }
}
