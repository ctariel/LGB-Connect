namespace LGBConnect
{
    partial class frm_Temps
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
            this.lbl_utilisateur = new System.Windows.Forms.Label();
            this.lbl_Chrono = new System.Windows.Forms.Label();
            this.btn_deconnexion = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbl_utilisateur
            // 
            this.lbl_utilisateur.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_utilisateur.ForeColor = System.Drawing.Color.LightSeaGreen;
            this.lbl_utilisateur.Location = new System.Drawing.Point(3, 9);
            this.lbl_utilisateur.Name = "lbl_utilisateur";
            this.lbl_utilisateur.Size = new System.Drawing.Size(179, 26);
            this.lbl_utilisateur.TabIndex = 0;
            this.lbl_utilisateur.Text = "utilisateur";
            this.lbl_utilisateur.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_Chrono
            // 
            this.lbl_Chrono.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Chrono.ForeColor = System.Drawing.Color.LightSeaGreen;
            this.lbl_Chrono.Location = new System.Drawing.Point(3, 43);
            this.lbl_Chrono.Name = "lbl_Chrono";
            this.lbl_Chrono.Size = new System.Drawing.Size(179, 26);
            this.lbl_Chrono.TabIndex = 1;
            this.lbl_Chrono.Text = "00:00:00";
            this.lbl_Chrono.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btn_deconnexion
            // 
            this.btn_deconnexion.Location = new System.Drawing.Point(30, 72);
            this.btn_deconnexion.Name = "btn_deconnexion";
            this.btn_deconnexion.Size = new System.Drawing.Size(123, 28);
            this.btn_deconnexion.TabIndex = 2;
            this.btn_deconnexion.Text = "Déconnexion";
            this.btn_deconnexion.UseVisualStyleBackColor = true;
            this.btn_deconnexion.Click += new System.EventHandler(this.btn_deconnexion_Click);
            // 
            // frm_Temps
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(184, 112);
            this.Controls.Add(this.btn_deconnexion);
            this.Controls.Add(this.lbl_Chrono);
            this.Controls.Add(this.lbl_utilisateur);
            this.Name = "frm_Temps";
            this.Text = "frm_Temps";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frm_Temps_FormClosed);
            this.Load += new System.EventHandler(this.frm_Temps_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lbl_utilisateur;
        private System.Windows.Forms.Label lbl_Chrono;
        private System.Windows.Forms.Button btn_deconnexion;
    }
}