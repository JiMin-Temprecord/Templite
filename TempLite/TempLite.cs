using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TempLite
{
    public partial class TempLite : Form
    {
        public TempLite()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void PDF_Click(object sender, EventArgs e)
        {
            if (pdfdownload.Visible == false)
            {
                pdfdownload.Visible = true;
                pdfemail.Visible = true;
                pdfpreview.Visible = true;
            }
            else
            {
                pdfdownload.Visible = false;
                pdfemail.Visible = false;
                pdfpreview.Visible = false;
            }
        }

        private void EXCEL_Click(object sender, EventArgs e)
        {
            if (exceldownload.Visible == false)
            {
                exceldownload.Visible = true;
                excelemail.Visible = true;
                excelpreview.Visible = true;
            }
            else
            {
                exceldownload.Visible = false;
                excelemail.Visible = false;
                excelpreview.Visible = false;
            }
        }

        private void pdfpreview_Click(object sender, EventArgs e)
        {
            PDF.Preview();
        }

        private void pdfemail_Click(object sender, EventArgs e)
        {
            PDF.Email();
        }

        private void pdfdownload_Click(object sender, EventArgs e)
        {
            PDF.Download();
        }
    }
}
