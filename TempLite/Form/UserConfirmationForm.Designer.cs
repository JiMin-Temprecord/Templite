namespace TempLite
{
    partial class UserConfirmationForm
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
            this.userConfirmationLabel = new System.Windows.Forms.Label();
            this.userConfirmationTextbox = new System.Windows.Forms.TextBox();
            this.cancelButton = new System.Windows.Forms.Button();
            this.deleteButton = new System.Windows.Forms.Button();
            this.bgPanel = new System.Windows.Forms.Panel();
            this.bgPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // userConfirmationLabel
            // 
            this.userConfirmationLabel.AutoSize = true;
            this.userConfirmationLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.userConfirmationLabel.ForeColor = System.Drawing.Color.Black;
            this.userConfirmationLabel.Location = new System.Drawing.Point(73, 48);
            this.userConfirmationLabel.Name = "userConfirmationLabel";
            this.userConfirmationLabel.Size = new System.Drawing.Size(111, 20);
            this.userConfirmationLabel.TabIndex = 3;
            this.userConfirmationLabel.Text = "Confirmation";
            // 
            // userConfirmationTextbox
            // 
            this.userConfirmationTextbox.BackColor = System.Drawing.Color.White;
            this.userConfirmationTextbox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.userConfirmationTextbox.Enabled = false;
            this.userConfirmationTextbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.userConfirmationTextbox.ForeColor = System.Drawing.Color.Black;
            this.userConfirmationTextbox.Location = new System.Drawing.Point(34, 72);
            this.userConfirmationTextbox.Multiline = true;
            this.userConfirmationTextbox.Name = "userConfirmationTextbox";
            this.userConfirmationTextbox.ReadOnly = true;
            this.userConfirmationTextbox.Size = new System.Drawing.Size(191, 39);
            this.userConfirmationTextbox.TabIndex = 4;
            this.userConfirmationTextbox.Text = "Are you sure you want to delete this email?";
            this.userConfirmationTextbox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // cancelButton
            // 
            this.cancelButton.BackColor = System.Drawing.Color.White;
            this.cancelButton.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.cancelButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cancelButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cancelButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(33)))), ((int)(((byte)(99)))));
            this.cancelButton.Location = new System.Drawing.Point(0, 140);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(125, 35);
            this.cancelButton.TabIndex = 6;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = false;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // deleteButton
            // 
            this.deleteButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(33)))), ((int)(((byte)(99)))));
            this.deleteButton.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.deleteButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.deleteButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.deleteButton.ForeColor = System.Drawing.Color.White;
            this.deleteButton.Location = new System.Drawing.Point(125, 140);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(125, 35);
            this.deleteButton.TabIndex = 5;
            this.deleteButton.Text = "Delete";
            this.deleteButton.UseVisualStyleBackColor = false;
            this.deleteButton.Click += new System.EventHandler(this.deleteButton_Click);
            // 
            // bgPanel
            // 
            this.bgPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.bgPanel.Controls.Add(this.userConfirmationLabel);
            this.bgPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bgPanel.Location = new System.Drawing.Point(0, 0);
            this.bgPanel.Name = "bgPanel";
            this.bgPanel.Size = new System.Drawing.Size(250, 175);
            this.bgPanel.TabIndex = 7;
            // 
            // UserConfirmationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(250, 175);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.userConfirmationTextbox);
            this.Controls.Add(this.bgPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "UserConfirmationForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Confirmation";
            this.bgPanel.ResumeLayout(false);
            this.bgPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label userConfirmationLabel;
        private System.Windows.Forms.TextBox userConfirmationTextbox;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button deleteButton;
        private System.Windows.Forms.Panel bgPanel;
    }
}