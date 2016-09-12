namespace LGBConnect
{
    partial class MainForm
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btn_Connexion = new System.Windows.Forms.Button();
            this.textBox_MotDePasse = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_Utilisateur = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lbl_Espace = new System.Windows.Forms.Label();
            this.groupBox_inscription = new System.Windows.Forms.GroupBox();
            this.btn_inscription = new System.Windows.Forms.Button();
            this.tableLayoutPanel_resa = new System.Windows.Forms.TableLayoutPanel();
            this.lbl_resa = new System.Windows.Forms.Label();
            this.lbl_resa_texte = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox_inscription.SuspendLayout();
            this.tableLayoutPanel_resa.SuspendLayout();
            this.SuspendLayout();
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Text = "notifyIcon_LGBConnect";
            this.notifyIcon1.Visible = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 300F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.lbl_Espace, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBox_inscription, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel_resa, 1, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(684, 462);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.groupBox1.Controls.Add(this.btn_Connexion);
            this.groupBox1.Controls.Add(this.textBox_MotDePasse);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.textBox_Utilisateur);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(195, 121);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(294, 144);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Vos identifiants";
            // 
            // btn_Connexion
            // 
            this.btn_Connexion.Location = new System.Drawing.Point(70, 106);
            this.btn_Connexion.Name = "btn_Connexion";
            this.btn_Connexion.Size = new System.Drawing.Size(158, 22);
            this.btn_Connexion.TabIndex = 3;
            this.btn_Connexion.Text = "connexion";
            this.btn_Connexion.UseVisualStyleBackColor = true;
            this.btn_Connexion.Click += new System.EventHandler(this.btn_Connexion_Click);
            // 
            // textBox_MotDePasse
            // 
            this.textBox_MotDePasse.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.textBox_MotDePasse.Location = new System.Drawing.Point(25, 71);
            this.textBox_MotDePasse.Name = "textBox_MotDePasse";
            this.textBox_MotDePasse.Size = new System.Drawing.Size(248, 20);
            this.textBox_MotDePasse.TabIndex = 2;
            this.textBox_MotDePasse.UseSystemPasswordChar = true;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label2.Location = new System.Drawing.Point(22, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(251, 16);
            this.label2.TabIndex = 1;
            this.label2.Text = "Mot de Passe";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // textBox_Utilisateur
            // 
            this.textBox_Utilisateur.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.textBox_Utilisateur.Location = new System.Drawing.Point(25, 29);
            this.textBox_Utilisateur.Name = "textBox_Utilisateur";
            this.textBox_Utilisateur.Size = new System.Drawing.Size(248, 20);
            this.textBox_Utilisateur.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label1.Location = new System.Drawing.Point(22, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(251, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Utilisateur";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_Espace
            // 
            this.lbl_Espace.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbl_Espace.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.lbl_Espace, 3);
            this.lbl_Espace.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Espace.ForeColor = System.Drawing.Color.LightSeaGreen;
            this.lbl_Espace.Location = new System.Drawing.Point(296, 46);
            this.lbl_Espace.Name = "lbl_Espace";
            this.lbl_Espace.Size = new System.Drawing.Size(91, 26);
            this.lbl_Espace.TabIndex = 2;
            this.lbl_Espace.Text = "Espace";
            this.lbl_Espace.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox_inscription
            // 
            this.groupBox_inscription.Controls.Add(this.btn_inscription);
            this.groupBox_inscription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox_inscription.Location = new System.Drawing.Point(195, 271);
            this.groupBox_inscription.Name = "groupBox_inscription";
            this.groupBox_inscription.Size = new System.Drawing.Size(294, 69);
            this.groupBox_inscription.TabIndex = 3;
            this.groupBox_inscription.TabStop = false;
            this.groupBox_inscription.Text = "Vous n\'avez pas encore de compte ?";
            // 
            // btn_inscription
            // 
            this.btn_inscription.Location = new System.Drawing.Point(70, 28);
            this.btn_inscription.Name = "btn_inscription";
            this.btn_inscription.Size = new System.Drawing.Size(158, 22);
            this.btn_inscription.TabIndex = 4;
            this.btn_inscription.Text = "Inscrivez-vous !";
            this.btn_inscription.UseVisualStyleBackColor = true;
            this.btn_inscription.Click += new System.EventHandler(this.btn_inscription_Click);
            // 
            // tableLayoutPanel_resa
            // 
            this.tableLayoutPanel_resa.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.tableLayoutPanel_resa.AutoSize = true;
            this.tableLayoutPanel_resa.ColumnCount = 1;
            this.tableLayoutPanel_resa.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel_resa.Controls.Add(this.lbl_resa, 0, 1);
            this.tableLayoutPanel_resa.Controls.Add(this.lbl_resa_texte, 0, 0);
            this.tableLayoutPanel_resa.Location = new System.Drawing.Point(244, 383);
            this.tableLayoutPanel_resa.Name = "tableLayoutPanel_resa";
            this.tableLayoutPanel_resa.RowCount = 2;
            this.tableLayoutPanel_resa.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel_resa.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel_resa.Size = new System.Drawing.Size(195, 38);
            this.tableLayoutPanel_resa.TabIndex = 5;
            // 
            // lbl_resa
            // 
            this.lbl_resa.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbl_resa.AutoSize = true;
            this.lbl_resa.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_resa.Location = new System.Drawing.Point(69, 19);
            this.lbl_resa.Name = "lbl_resa";
            this.lbl_resa.Padding = new System.Windows.Forms.Padding(3);
            this.lbl_resa.Size = new System.Drawing.Size(56, 19);
            this.lbl_resa.TabIndex = 5;
            this.lbl_resa.Text = "Aucune";
            this.lbl_resa.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_resa_texte
            // 
            this.lbl_resa_texte.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbl_resa_texte.AutoSize = true;
            this.lbl_resa_texte.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_resa_texte.Location = new System.Drawing.Point(3, 0);
            this.lbl_resa_texte.Name = "lbl_resa_texte";
            this.lbl_resa_texte.Padding = new System.Windows.Forms.Padding(3);
            this.lbl_resa_texte.Size = new System.Drawing.Size(189, 19);
            this.lbl_resa_texte.TabIndex = 4;
            this.lbl_resa_texte.Text = "prochaine réservation du poste";
            this.lbl_resa_texte.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MainForm
            // 
            this.AcceptButton = this.btn_Connexion;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(684, 462);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "LGB Connect";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox_inscription.ResumeLayout(false);
            this.tableLayoutPanel_resa.ResumeLayout(false);
            this.tableLayoutPanel_resa.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btn_Connexion;
        private System.Windows.Forms.TextBox textBox_MotDePasse;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_Utilisateur;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbl_Espace;
        private System.Windows.Forms.GroupBox groupBox_inscription;
        private System.Windows.Forms.Button btn_inscription;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel_resa;
        private System.Windows.Forms.Label lbl_resa;
        private System.Windows.Forms.Label lbl_resa_texte;
    }
}

