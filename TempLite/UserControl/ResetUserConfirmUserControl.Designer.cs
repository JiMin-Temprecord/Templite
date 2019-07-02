namespace UserControls
{
    partial class ResetUserConfirmUserControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.resetButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.resetLabel = new System.Windows.Forms.Label();
            this.resetConfirmationTextbox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // resetButton
            // 
            this.resetButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(33)))), ((int)(((byte)(99)))));
            this.resetButton.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.resetButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.resetButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.resetButton.ForeColor = System.Drawing.Color.White;
            this.resetButton.Location = new System.Drawing.Point(125, 140);
            this.resetButton.Name = "resetButton";
            this.resetButton.Size = new System.Drawing.Size(125, 35);
            this.resetButton.TabIndex = 0;
            this.resetButton.Text = "Reset";
            this.resetButton.UseVisualStyleBackColor = false;
            this.resetButton.Click += new System.EventHandler(this.resetButton_Click);
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
            this.cancelButton.TabIndex = 1;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = false;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // resetLabel
            // 
            this.resetLabel.AutoSize = true;
            this.resetLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.resetLabel.ForeColor = System.Drawing.Color.Black;
            this.resetLabel.Location = new System.Drawing.Point(50, 49);
            this.resetLabel.Name = "resetLabel";
            this.resetLabel.Size = new System.Drawing.Size(164, 20);
            this.resetLabel.TabIndex = 2;
            this.resetLabel.Text = "Reset Confirmation";
            // 
            // resetConfirmationTextbox
            // 
            this.resetConfirmationTextbox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.resetConfirmationTextbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.resetConfirmationTextbox.ForeColor = System.Drawing.Color.Black;
            this.resetConfirmationTextbox.Location = new System.Drawing.Point(34, 72);
            this.resetConfirmationTextbox.Multiline = true;
            this.resetConfirmationTextbox.Name = "resetConfirmationTextbox";
            this.resetConfirmationTextbox.Size = new System.Drawing.Size(191, 39);
            this.resetConfirmationTextbox.TabIndex = 3;
            this.resetConfirmationTextbox.Text = "Are you sure you want to reset the email list?";
            this.resetConfirmationTextbox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // ResetUserConfirmUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.resetConfirmationTextbox);
            this.Controls.Add(this.resetLabel);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.resetButton);
            this.Name = "ResetUserConfirmUserControl";
            this.Size = new System.Drawing.Size(248, 173);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button resetButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label resetLabel;
        private System.Windows.Forms.TextBox resetConfirmationTextbox;
    }
}
