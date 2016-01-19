namespace LGBConnect
{
    partial class assistantConfig
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.panel_Page1 = new System.Windows.Forms.Panel();
            this.btn_Suivant1 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lbl_resultatConnexion = new System.Windows.Forms.Label();
            this.btn_TestConnexion = new System.Windows.Forms.Button();
            this.textBox_Utilisateur = new System.Windows.Forms.TextBox();
            this.textBox_MotDePasse = new System.Windows.Forms.TextBox();
            this.textBox_Base = new System.Windows.Forms.TextBox();
            this.textBox_Hote = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.panel_Page2 = new System.Windows.Forms.Panel();
            this.btn_Terminer = new System.Windows.Forms.Button();
            this.btn_Precedant = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lbl_IP = new System.Windows.Forms.Label();
            this.lbl_Espace = new System.Windows.Forms.Label();
            this.btn_MAJ = new System.Windows.Forms.Button();
            this.comboBox_MAC = new System.Windows.Forms.ComboBox();
            this.comboBox_Poste = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.radioButton_PosteAnimateur = new System.Windows.Forms.RadioButton();
            this.radioButton_PosteUsager = new System.Windows.Forms.RadioButton();
            this.panel_principal = new System.Windows.Forms.Panel();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panel_Page1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.panel_Page2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.panel_principal.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.ItemSize = new System.Drawing.Size(58, 15);
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(624, 246);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.Visible = false;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.panel_Page1);
            this.tabPage1.Location = new System.Drawing.Point(4, 19);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(616, 223);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // panel_Page1
            // 
            this.panel_Page1.Controls.Add(this.btn_Suivant1);
            this.panel_Page1.Controls.Add(this.groupBox1);
            this.panel_Page1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_Page1.Location = new System.Drawing.Point(3, 3);
            this.panel_Page1.Name = "panel_Page1";
            this.panel_Page1.Size = new System.Drawing.Size(610, 217);
            this.panel_Page1.TabIndex = 7;
            // 
            // btn_Suivant1
            // 
            this.btn_Suivant1.Enabled = false;
            this.btn_Suivant1.Location = new System.Drawing.Point(475, 184);
            this.btn_Suivant1.Name = "btn_Suivant1";
            this.btn_Suivant1.Size = new System.Drawing.Size(130, 26);
            this.btn_Suivant1.TabIndex = 6;
            this.btn_Suivant1.Text = "Suivant >";
            this.btn_Suivant1.UseVisualStyleBackColor = true;
            this.btn_Suivant1.Click += new System.EventHandler(this.btn_Suivant1_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lbl_resultatConnexion);
            this.groupBox1.Controls.Add(this.btn_TestConnexion);
            this.groupBox1.Controls.Add(this.textBox_Utilisateur);
            this.groupBox1.Controls.Add(this.textBox_MotDePasse);
            this.groupBox1.Controls.Add(this.textBox_Base);
            this.groupBox1.Controls.Add(this.textBox_Hote);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(602, 175);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Configuration MySQL";
            // 
            // lbl_resultatConnexion
            // 
            this.lbl_resultatConnexion.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lbl_resultatConnexion.Location = new System.Drawing.Point(326, 25);
            this.lbl_resultatConnexion.Name = "lbl_resultatConnexion";
            this.lbl_resultatConnexion.Size = new System.Drawing.Size(257, 135);
            this.lbl_resultatConnexion.TabIndex = 8;
            this.lbl_resultatConnexion.Text = "En attente...";
            this.lbl_resultatConnexion.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btn_TestConnexion
            // 
            this.btn_TestConnexion.Location = new System.Drawing.Point(10, 135);
            this.btn_TestConnexion.Name = "btn_TestConnexion";
            this.btn_TestConnexion.Size = new System.Drawing.Size(264, 26);
            this.btn_TestConnexion.TabIndex = 5;
            this.btn_TestConnexion.Text = "Tester la connexion";
            this.btn_TestConnexion.UseVisualStyleBackColor = true;
            this.btn_TestConnexion.Click += new System.EventHandler(this.btn_TestConnexion_Click);
            // 
            // textBox_Utilisateur
            // 
            this.textBox_Utilisateur.Location = new System.Drawing.Point(101, 74);
            this.textBox_Utilisateur.Name = "textBox_Utilisateur";
            this.textBox_Utilisateur.Size = new System.Drawing.Size(174, 20);
            this.textBox_Utilisateur.TabIndex = 3;
            this.textBox_Utilisateur.TextChanged += new System.EventHandler(this.textBox_Utilisateur_TextChanged);
            // 
            // textBox_MotDePasse
            // 
            this.textBox_MotDePasse.Location = new System.Drawing.Point(101, 100);
            this.textBox_MotDePasse.Name = "textBox_MotDePasse";
            this.textBox_MotDePasse.Size = new System.Drawing.Size(174, 20);
            this.textBox_MotDePasse.TabIndex = 4;
            this.textBox_MotDePasse.TextChanged += new System.EventHandler(this.textBox_MotDePasse_TextChanged);
            // 
            // textBox_Base
            // 
            this.textBox_Base.Location = new System.Drawing.Point(101, 48);
            this.textBox_Base.Name = "textBox_Base";
            this.textBox_Base.Size = new System.Drawing.Size(174, 20);
            this.textBox_Base.TabIndex = 2;
            this.textBox_Base.TextChanged += new System.EventHandler(this.textBox_Base_TextChanged);
            // 
            // textBox_Hote
            // 
            this.textBox_Hote.Location = new System.Drawing.Point(101, 22);
            this.textBox_Hote.Name = "textBox_Hote";
            this.textBox_Hote.Size = new System.Drawing.Size(174, 20);
            this.textBox_Hote.TabIndex = 1;
            this.textBox_Hote.TextChanged += new System.EventHandler(this.textBox_Hote_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Base :";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 103);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Mot de passe :";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Hôte :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 77);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 13);
            this.label3.TabIndex = 210;
            this.label3.Text = "Utilisateur :";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.panel_Page2);
            this.tabPage2.Location = new System.Drawing.Point(4, 19);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(616, 223);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // panel_Page2
            // 
            this.panel_Page2.Controls.Add(this.btn_Terminer);
            this.panel_Page2.Controls.Add(this.btn_Precedant);
            this.panel_Page2.Controls.Add(this.groupBox2);
            this.panel_Page2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_Page2.Location = new System.Drawing.Point(3, 3);
            this.panel_Page2.Name = "panel_Page2";
            this.panel_Page2.Size = new System.Drawing.Size(610, 217);
            this.panel_Page2.TabIndex = 0;
            // 
            // btn_Terminer
            // 
            this.btn_Terminer.Enabled = false;
            this.btn_Terminer.Location = new System.Drawing.Point(475, 184);
            this.btn_Terminer.Name = "btn_Terminer";
            this.btn_Terminer.Size = new System.Drawing.Size(130, 26);
            this.btn_Terminer.TabIndex = 12;
            this.btn_Terminer.Text = "Terminer";
            this.btn_Terminer.UseVisualStyleBackColor = true;
            this.btn_Terminer.Click += new System.EventHandler(this.btn_Terminer_Click);
            // 
            // btn_Precedant
            // 
            this.btn_Precedant.Location = new System.Drawing.Point(339, 184);
            this.btn_Precedant.Name = "btn_Precedant";
            this.btn_Precedant.Size = new System.Drawing.Size(130, 26);
            this.btn_Precedant.TabIndex = 11;
            this.btn_Precedant.Text = "< Précédant";
            this.btn_Precedant.UseVisualStyleBackColor = true;
            this.btn_Precedant.Click += new System.EventHandler(this.btn_Precedant_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lbl_IP);
            this.groupBox2.Controls.Add(this.lbl_Espace);
            this.groupBox2.Controls.Add(this.btn_MAJ);
            this.groupBox2.Controls.Add(this.comboBox_MAC);
            this.groupBox2.Controls.Add(this.comboBox_Poste);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.radioButton_PosteAnimateur);
            this.groupBox2.Controls.Add(this.radioButton_PosteUsager);
            this.groupBox2.Location = new System.Drawing.Point(3, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(602, 175);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Configuration du poste";
            // 
            // lbl_IP
            // 
            this.lbl_IP.AutoSize = true;
            this.lbl_IP.Location = new System.Drawing.Point(145, 138);
            this.lbl_IP.Name = "lbl_IP";
            this.lbl_IP.Size = new System.Drawing.Size(0, 13);
            this.lbl_IP.TabIndex = 10;
            // 
            // lbl_Espace
            // 
            this.lbl_Espace.AutoSize = true;
            this.lbl_Espace.Location = new System.Drawing.Point(145, 58);
            this.lbl_Espace.Name = "lbl_Espace";
            this.lbl_Espace.Size = new System.Drawing.Size(0, 13);
            this.lbl_Espace.TabIndex = 9;
            // 
            // btn_MAJ
            // 
            this.btn_MAJ.Location = new System.Drawing.Point(506, 80);
            this.btn_MAJ.Name = "btn_MAJ";
            this.btn_MAJ.Size = new System.Drawing.Size(75, 23);
            this.btn_MAJ.TabIndex = 8;
            this.btn_MAJ.Text = "Mise à jour";
            this.btn_MAJ.UseVisualStyleBackColor = true;
            this.btn_MAJ.Click += new System.EventHandler(this.btn_MAJ_Click);
            // 
            // comboBox_MAC
            // 
            this.comboBox_MAC.FormattingEnabled = true;
            this.comboBox_MAC.Location = new System.Drawing.Point(148, 109);
            this.comboBox_MAC.Name = "comboBox_MAC";
            this.comboBox_MAC.Size = new System.Drawing.Size(352, 21);
            this.comboBox_MAC.TabIndex = 6;
            this.comboBox_MAC.SelectedIndexChanged += new System.EventHandler(this.comboBox_MAC_SelectedIndexChanged);
            // 
            // comboBox_Poste
            // 
            this.comboBox_Poste.FormattingEnabled = true;
            this.comboBox_Poste.Location = new System.Drawing.Point(148, 82);
            this.comboBox_Poste.Name = "comboBox_Poste";
            this.comboBox_Poste.Size = new System.Drawing.Size(352, 21);
            this.comboBox_Poste.TabIndex = 5;
            this.comboBox_Poste.SelectedIndexChanged += new System.EventHandler(this.comboBox_Poste_SelectedIndexChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(0, 138);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(64, 13);
            this.label8.TabIndex = 3;
            this.label8.Text = "Adresse IP :";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(0, 112);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(77, 13);
            this.label7.TabIndex = 4;
            this.label7.Text = "Adresse MAC :";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(0, 85);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(79, 13);
            this.label6.TabIndex = 3;
            this.label6.Text = "Nom du poste :";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(0, 58);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(49, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "Espace :";
            // 
            // radioButton_PosteAnimateur
            // 
            this.radioButton_PosteAnimateur.AutoSize = true;
            this.radioButton_PosteAnimateur.Location = new System.Drawing.Point(148, 32);
            this.radioButton_PosteAnimateur.Name = "radioButton_PosteAnimateur";
            this.radioButton_PosteAnimateur.Size = new System.Drawing.Size(101, 17);
            this.radioButton_PosteAnimateur.TabIndex = 1;
            this.radioButton_PosteAnimateur.TabStop = true;
            this.radioButton_PosteAnimateur.Text = "Poste animateur";
            this.radioButton_PosteAnimateur.UseVisualStyleBackColor = true;
            this.radioButton_PosteAnimateur.CheckedChanged += new System.EventHandler(this.radioButton_PosteAnimateur_CheckedChanged);
            // 
            // radioButton_PosteUsager
            // 
            this.radioButton_PosteUsager.AutoSize = true;
            this.radioButton_PosteUsager.Location = new System.Drawing.Point(3, 32);
            this.radioButton_PosteUsager.Name = "radioButton_PosteUsager";
            this.radioButton_PosteUsager.Size = new System.Drawing.Size(88, 17);
            this.radioButton_PosteUsager.TabIndex = 0;
            this.radioButton_PosteUsager.TabStop = true;
            this.radioButton_PosteUsager.Text = "poste Usager";
            this.radioButton_PosteUsager.UseVisualStyleBackColor = true;
            this.radioButton_PosteUsager.CheckedChanged += new System.EventHandler(this.radioButton_PosteUsager_CheckedChanged);
            // 
            // panel_principal
            // 
            this.panel_principal.Controls.Add(this.tabControl1);
            this.panel_principal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_principal.Location = new System.Drawing.Point(0, 0);
            this.panel_principal.Name = "panel_principal";
            this.panel_principal.Size = new System.Drawing.Size(624, 246);
            this.panel_principal.TabIndex = 1;
            // 
            // assistantConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(624, 246);
            this.Controls.Add(this.panel_principal);
            this.Name = "assistantConfig";
            this.Text = "Assistant de Configuration";
            this.Load += new System.EventHandler(this.assistantConfig_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.panel_Page1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.panel_Page2.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.panel_principal.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btn_TestConnexion;
        private System.Windows.Forms.TextBox textBox_Utilisateur;
        private System.Windows.Forms.TextBox textBox_MotDePasse;
        private System.Windows.Forms.TextBox textBox_Base;
        private System.Windows.Forms.TextBox textBox_Hote;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btn_Suivant1;
        private System.Windows.Forms.Label lbl_resultatConnexion;
        private System.Windows.Forms.Panel panel_Page1;
        private System.Windows.Forms.Panel panel_principal;
        private System.Windows.Forms.Panel panel_Page2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton radioButton_PosteAnimateur;
        private System.Windows.Forms.RadioButton radioButton_PosteUsager;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox comboBox_MAC;
        private System.Windows.Forms.ComboBox comboBox_Poste;
        private System.Windows.Forms.Button btn_Terminer;
        private System.Windows.Forms.Button btn_Precedant;
        private System.Windows.Forms.Button btn_MAJ;
        private System.Windows.Forms.Label lbl_Espace;
        private System.Windows.Forms.Label lbl_IP;
    }
}