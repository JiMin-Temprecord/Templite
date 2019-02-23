namespace TempLite
{
    partial class TempLite
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TempLite));
            this.pdfpanel = new System.Windows.Forms.Panel();
            this.pdfdownload = new System.Windows.Forms.Button();
            this.pdfemail = new System.Windows.Forms.Button();
            this.pdfpreview = new System.Windows.Forms.Button();
            this.PDFimage = new System.Windows.Forms.Button();
            this.excelpanel = new System.Windows.Forms.Panel();
            this.exceldownload = new System.Windows.Forms.Button();
            this.excelemail = new System.Windows.Forms.Button();
            this.excelpreview = new System.Windows.Forms.Button();
            this.EXCEL = new System.Windows.Forms.Button();
            this.pdfpanel.SuspendLayout();
            this.excelpanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // pdfpanel
            // 
            this.pdfpanel.Controls.Add(this.pdfdownload);
            this.pdfpanel.Controls.Add(this.pdfemail);
            this.pdfpanel.Controls.Add(this.pdfpreview);
            this.pdfpanel.Controls.Add(this.PDFimage);
            this.pdfpanel.Location = new System.Drawing.Point(0, 0);
            this.pdfpanel.Margin = new System.Windows.Forms.Padding(10);
            this.pdfpanel.Name = "pdfpanel";
            this.pdfpanel.Size = new System.Drawing.Size(300, 300);
            this.pdfpanel.TabIndex = 0;
            // 
            // pdfdownload
            // 
            this.pdfdownload.Location = new System.Drawing.Point(233, 68);
            this.pdfdownload.Name = "pdfdownload";
            this.pdfdownload.Size = new System.Drawing.Size(50, 50);
            this.pdfdownload.TabIndex = 5;
            this.pdfdownload.UseVisualStyleBackColor = true;
            this.pdfdownload.Visible = false;
            this.pdfdownload.Click += new System.EventHandler(this.pdfdownload_Click);
            // 
            // pdfemail
            // 
            this.pdfemail.Location = new System.Drawing.Point(233, 12);
            this.pdfemail.Name = "pdfemail";
            this.pdfemail.Size = new System.Drawing.Size(50, 50);
            this.pdfemail.TabIndex = 4;
            this.pdfemail.UseVisualStyleBackColor = true;
            this.pdfemail.Visible = false;
            this.pdfemail.Click += new System.EventHandler(this.pdfemail_Click);
            // 
            // pdfpreview
            // 
            this.pdfpreview.Location = new System.Drawing.Point(177, 12);
            this.pdfpreview.Name = "pdfpreview";
            this.pdfpreview.Size = new System.Drawing.Size(50, 50);
            this.pdfpreview.TabIndex = 3;
            this.pdfpreview.UseVisualStyleBackColor = true;
            this.pdfpreview.Visible = false;
            this.pdfpreview.Click += new System.EventHandler(this.pdfpreview_Click);
            // 
            // PDFimage
            // 
            this.PDFimage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PDFimage.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.PDFimage.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.PDFimage.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("PDFimage.BackgroundImage")));
            this.PDFimage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.PDFimage.FlatAppearance.BorderSize = 0;
            this.PDFimage.Location = new System.Drawing.Point(100, 100);
            this.PDFimage.Margin = new System.Windows.Forms.Padding(5);
            this.PDFimage.Name = "PDFimage";
            this.PDFimage.Padding = new System.Windows.Forms.Padding(5);
            this.PDFimage.Size = new System.Drawing.Size(100, 100);
            this.PDFimage.TabIndex = 2;
            this.PDFimage.UseVisualStyleBackColor = false;
            this.PDFimage.Click += new System.EventHandler(this.PDF_Click);
            // 
            // excelpanel
            // 
            this.excelpanel.Controls.Add(this.exceldownload);
            this.excelpanel.Controls.Add(this.excelemail);
            this.excelpanel.Controls.Add(this.excelpreview);
            this.excelpanel.Controls.Add(this.EXCEL);
            this.excelpanel.Location = new System.Drawing.Point(323, 0);
            this.excelpanel.Name = "excelpanel";
            this.excelpanel.Size = new System.Drawing.Size(300, 300);
            this.excelpanel.TabIndex = 1;
            // 
            // exceldownload
            // 
            this.exceldownload.Location = new System.Drawing.Point(239, 68);
            this.exceldownload.Name = "exceldownload";
            this.exceldownload.Size = new System.Drawing.Size(50, 50);
            this.exceldownload.TabIndex = 8;
            this.exceldownload.UseVisualStyleBackColor = true;
            this.exceldownload.Visible = false;
            // 
            // excelemail
            // 
            this.excelemail.Location = new System.Drawing.Point(239, 12);
            this.excelemail.Name = "excelemail";
            this.excelemail.Size = new System.Drawing.Size(50, 50);
            this.excelemail.TabIndex = 7;
            this.excelemail.UseVisualStyleBackColor = true;
            this.excelemail.Visible = false;
            // 
            // excelpreview
            // 
            this.excelpreview.Location = new System.Drawing.Point(183, 12);
            this.excelpreview.Name = "excelpreview";
            this.excelpreview.Size = new System.Drawing.Size(50, 50);
            this.excelpreview.TabIndex = 6;
            this.excelpreview.UseVisualStyleBackColor = true;
            this.excelpreview.Visible = false;
            // 
            // EXCEL
            // 
            this.EXCEL.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.EXCEL.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.EXCEL.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.EXCEL.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("EXCEL.BackgroundImage")));
            this.EXCEL.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.EXCEL.FlatAppearance.BorderSize = 0;
            this.EXCEL.Location = new System.Drawing.Point(100, 100);
            this.EXCEL.Name = "EXCEL";
            this.EXCEL.Size = new System.Drawing.Size(100, 100);
            this.EXCEL.TabIndex = 1;
            this.EXCEL.UseVisualStyleBackColor = false;
            this.EXCEL.Click += new System.EventHandler(this.EXCEL_Click);
            // 
            // TempLite
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 300);
            this.Controls.Add(this.excelpanel);
            this.Controls.Add(this.pdfpanel);
            this.Name = "TempLite";
            this.Text = "Form1";
            this.pdfpanel.ResumeLayout(false);
            this.excelpanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pdfpanel;
        private System.Windows.Forms.Panel excelpanel;
        private System.Windows.Forms.Button PDFimage;
        private System.Windows.Forms.Button EXCEL;
        private System.Windows.Forms.Button pdfdownload;
        private System.Windows.Forms.Button pdfemail;
        private System.Windows.Forms.Button pdfpreview;
        private System.Windows.Forms.Button exceldownload;
        private System.Windows.Forms.Button excelemail;
        private System.Windows.Forms.Button excelpreview;
    }
}

