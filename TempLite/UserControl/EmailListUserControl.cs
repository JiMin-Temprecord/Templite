using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using TempLite;
using TempLite.Constant;
using TempLite.Services;

namespace UserControls
{
    public partial class EmailListUserControl : UserControl
    {
        public EmailListUserControl()
        {
            InitializeComponent();
        }
        private void EmailListUserControl_VisibleChanged(object sender, EventArgs e)
        {
            removeAllEmailfromList();
            addAllEmailtoList();
        }
        public void addAllEmailtoList()
        {
            string line;
            int y = 45;
            int i = 0;

            if (File.Exists(Email.path + EmailConstant.AllEmail))
            {
                using (StreamReader sr = File.OpenText(Email.path + EmailConstant.AllEmail))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        var listEmailUserControl = new ListEmailUserControl();
                        listEmailUserControl.emailLabel.Text = line;
                        listEmailUserControl.Location = new Point(-1, y);
                        listEmailUserControl.SendToBack();
                        emailListPanel.Controls.Add(listEmailUserControl);
                        Email.emailList.Add(listEmailUserControl);
                        y = y + 45;
                        i++;
                    }
                }
            }
        }
        public void removeAllEmailfromList()
        {         
            if (Email.emailList.Count > 0)
            {
                for (int i = 0; i < Email.emailList.Count; i++)
                {
                    emailListPanel.Controls.Remove(Email.emailList[i]);
                }

                Email.emailList.Clear();
            }
        }
    }
}
