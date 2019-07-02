using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TempLite;

namespace UserControls
{
    public partial class ListEmailUserControl : UserControl
    {
        BackgroundWorker waitForConfirmBW;

        public ListEmailUserControl()
        {
            InitializeComponent();
        }
        void emailDeleteButton_Click(object sender, EventArgs e)
        {
            Enabled = false;

            var ConfirmUserControl = new UserConfirmUserControl();
            this.Parent.Parent.Controls.Add(ConfirmUserControl);
            ConfirmUserControl.Size = new Size(250, 175);
            ConfirmUserControl.Location = new Point(225, 250);
            ConfirmUserControl.BringToFront();
            ConfirmUserControl.Visible = true;

            this.Parent.Enabled = false;

            waitForConfirmBW = new BackgroundWorker();
            waitForConfirmBW.DoWork += waitForConfirmBackgroundWorker_DoWork;
            waitForConfirmBW.RunWorkerCompleted += waitForConfirmBackgroundWorker_RunWorkerCompleted;
            waitForConfirmBW.WorkerReportsProgress = true;
            waitForConfirmBW.WorkerSupportsCancellation = true;
            waitForConfirmBW.RunWorkerAsync();
        }

        void waitForConfirmBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (UserConfirmUserControl.isVisible == true)
            {
            }
        }
        void waitForConfirmBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Parent.Enabled = true ;

            if (UserConfirmUserControl.shouldDelete)
            {
                Email.Delete(emailLabel.Text);
                this.Dispose();
            }
            else { Enabled = true;}
        }
    }
}
