namespace Strafe
{
    partial class PopupErrorBox
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
            this.pb_ok = new System.Windows.Forms.Button();
            this.lb_errorDetails = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // pb_ok
            // 
            this.pb_ok.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.pb_ok.Location = new System.Drawing.Point(1097, 505);
            this.pb_ok.Name = "pb_ok";
            this.pb_ok.Size = new System.Drawing.Size(75, 23);
            this.pb_ok.TabIndex = 0;
            this.pb_ok.Text = "Ok";
            this.pb_ok.UseVisualStyleBackColor = true;
            this.pb_ok.Click += new System.EventHandler(this.Pb_ok_Click);
            // 
            // lb_errorDetails
            // 
            this.lb_errorDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lb_errorDetails.FormattingEnabled = true;
            this.lb_errorDetails.Location = new System.Drawing.Point(14, 12);
            this.lb_errorDetails.Name = "lb_errorDetails";
            this.lb_errorDetails.Size = new System.Drawing.Size(1157, 485);
            this.lb_errorDetails.TabIndex = 1;
            this.lb_errorDetails.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.Lb_errorDetils_DrawItem);
            this.lb_errorDetails.MeasureItem += new System.Windows.Forms.MeasureItemEventHandler(this.Lb_errorDetils_MeasureItem);
            // 
            // PopupErrorBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1184, 540);
            this.Controls.Add(this.lb_errorDetails);
            this.Controls.Add(this.pb_ok);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PopupErrorBox";
            this.Text = "Game Error has occurred - Please take not of error and report";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button pb_ok;
        private System.Windows.Forms.ListBox lb_errorDetails;
    }
}