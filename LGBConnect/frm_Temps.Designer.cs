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
            this.lbl_temps_restant = new System.Windows.Forms.Label();
            this.btn_deconnexion = new System.Windows.Forms.Button();
            this.lbl_temps_utilise = new System.Windows.Forms.Label();
            this.lbl_text_utilise = new System.Windows.Forms.Label();
            this.lbl_text_restant = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbl_utilisateur
            // 
            this.lbl_utilisateur.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_utilisateur.ForeColor = System.Drawing.Color.LightSeaGreen;
            this.lbl_utilisateur.Location = new System.Drawing.Point(3, -2);
            this.lbl_utilisateur.Name = "lbl_utilisateur";
            this.lbl_utilisateur.Size = new System.Drawing.Size(179, 26);
            this.lbl_utilisateur.TabIndex = 0;
            this.lbl_utilisateur.Text = "utilisateur";
            this.lbl_utilisateur.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_temps_restant
            // 
            this.lbl_temps_restant.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_temps_restant.ForeColor = System.Drawing.Color.LightSeaGreen;
            this.lbl_temps_restant.Location = new System.Drawing.Point(82, 52);
            this.lbl_temps_restant.Name = "lbl_temps_restant";
            this.lbl_temps_restant.Size = new System.Drawing.Size(91, 26);
            this.lbl_temps_restant.TabIndex = 1;
            this.lbl_temps_restant.Text = "00:00:00";
            this.lbl_temps_restant.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btn_deconnexion
            // 
            this.btn_deconnexion.Location = new System.Drawing.Point(31, 81);
            this.btn_deconnexion.Name = "btn_deconnexion";
            this.btn_deconnexion.Size = new System.Drawing.Size(123, 28);
            this.btn_deconnexion.TabIndex = 2;
            this.btn_deconnexion.Text = "Déconnexion";
            this.btn_deconnexion.UseVisualStyleBackColor = true;
            this.btn_deconnexion.Click += new System.EventHandler(this.Btn_deconnexion_Click);
            // 
            // lbl_temps_utilise
            // 
            this.lbl_temps_utilise.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_temps_utilise.ForeColor = System.Drawing.Color.LightSeaGreen;
            this.lbl_temps_utilise.Location = new System.Drawing.Point(82, 26);
            this.lbl_temps_utilise.Name = "lbl_temps_utilise";
            this.lbl_temps_utilise.Size = new System.Drawing.Size(91, 26);
            this.lbl_temps_utilise.TabIndex = 3;
            this.lbl_temps_utilise.Text = "00:00:00";
            this.lbl_temps_utilise.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbl_text_utilise
            // 
            this.lbl_text_utilise.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_text_utilise.Location = new System.Drawing.Point(3, 26);
            this.lbl_text_utilise.Name = "lbl_text_utilise";
            this.lbl_text_utilise.Size = new System.Drawing.Size(82, 26);
            this.lbl_text_utilise.TabIndex = 4;
            this.lbl_text_utilise.Text = "Utilisé :";
            this.lbl_text_utilise.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbl_text_restant
            // 
            this.lbl_text_restant.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_text_restant.Location = new System.Drawing.Point(3, 52);
            this.lbl_text_restant.Name = "lbl_text_restant";
            this.lbl_text_restant.Size = new System.Drawing.Size(82, 26);
            this.lbl_text_restant.TabIndex = 5;
            this.lbl_text_restant.Text = "Restant :";
            this.lbl_text_restant.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // frm_Temps
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(184, 112);
            this.Controls.Add(this.lbl_text_restant);
            this.Controls.Add(this.lbl_text_utilise);
            this.Controls.Add(this.lbl_temps_utilise);
            this.Controls.Add(this.btn_deconnexion);
            this.Controls.Add(this.lbl_temps_restant);
            this.Controls.Add(this.lbl_utilisateur);
            this.Name = "frm_Temps";
            this.Text = "frm_Temps";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Frm_Temps_FormClosed);
            this.Load += new System.EventHandler(this.Frm_Temps_Load);
            this.Shown += new System.EventHandler(this.Frm_Temps_Shown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lbl_utilisateur;
        private System.Windows.Forms.Label lbl_temps_restant;
        private System.Windows.Forms.Button btn_deconnexion;
        private System.Windows.Forms.Label lbl_temps_utilise;
        private System.Windows.Forms.Label lbl_text_utilise;
        private System.Windows.Forms.Label lbl_text_restant;
    }
}