namespace LGBConnect
{
    partial class frm_Splash
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
            this.picBox_splash = new System.Windows.Forms.PictureBox();
            this.lbl_Splash = new System.Windows.Forms.Label();
            this.progressBar_Splash = new System.Windows.Forms.ProgressBar();
            ((System.ComponentModel.ISupportInitialize)(this.picBox_splash)).BeginInit();
            this.SuspendLayout();
            // 
            // picBox_splash
            // 
            this.picBox_splash.Image = global::LGBConnect.Properties.Resources.splash;
            this.picBox_splash.Location = new System.Drawing.Point(0, 0);
            this.picBox_splash.Name = "picBox_splash";
            this.picBox_splash.Size = new System.Drawing.Size(640, 480);
            this.picBox_splash.TabIndex = 0;
            this.picBox_splash.TabStop = false;
            // 
            // lbl_Splash
            // 
            this.lbl_Splash.BackColor = System.Drawing.SystemColors.Window;
            this.lbl_Splash.Font = new System.Drawing.Font("Eras Medium ITC", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Splash.Location = new System.Drawing.Point(0, 347);
            this.lbl_Splash.Name = "lbl_Splash";
            this.lbl_Splash.Size = new System.Drawing.Size(640, 25);
            this.lbl_Splash.TabIndex = 1;
            this.lbl_Splash.Text = "Patientez, SVP...";
            this.lbl_Splash.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // progressBar_Splash
            // 
            this.progressBar_Splash.BackColor = System.Drawing.SystemColors.Control;
            this.progressBar_Splash.Location = new System.Drawing.Point(70, 400);
            this.progressBar_Splash.Name = "progressBar_Splash";
            this.progressBar_Splash.Size = new System.Drawing.Size(500, 20);
            this.progressBar_Splash.TabIndex = 2;
            // 
            // frm_Splash
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(640, 480);
            this.Controls.Add(this.progressBar_Splash);
            this.Controls.Add(this.lbl_Splash);
            this.Controls.Add(this.picBox_splash);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frm_Splash";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SplashScreen";
            ((System.ComponentModel.ISupportInitialize)(this.picBox_splash)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox picBox_splash;
        private System.Windows.Forms.Label lbl_Splash;
        private System.Windows.Forms.ProgressBar progressBar_Splash;
    }
}