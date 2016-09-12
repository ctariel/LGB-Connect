﻿using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using LGBConnect.classes;

namespace LGBConnect
{
    /// <summary>
    /// Affichage de la fenetre de demande de mot de passe.
    /// 
    /// Pour un utilisateur standard, la fenêtre est maximisée 
    /// </summary>
    public partial class MainForm : Form
    {

        Timer timer_MAJEtat = new Timer();

        ToolStripMenuItem menuItemParametres, menuItemFinSession, menuItemQuitter;
        frm_Temps frmTemps;

        ConfigLogiciel configLogiciel;

        Poste poste;
        Resa prochaineResa;
        Utilisateur prochainUtilisateur;

        public MainForm()
        {
            InitializeComponent();
            CreateContextMenu();
        }


        private void MainForm_Load(object sender, EventArgs e)
        {
            Parametres.debug = "no"; // par défaut

            int retour = 0;

            frm_Splash splash = new frm_Splash();
            retour = splash.chargement(); // si retour == 0, alors les données de connexions sont ok
            splash.Close();
            if (retour != 0)
            {
                assistantConfig assistant = new assistantConfig();
                assistant.ShowDialog();
                // on relance une fois le splash, et en cas d'échec, on quitte
                splash = new frm_Splash();
                retour = splash.chargement();
                splash.Close();
                if (retour != 0)
                {
                    this.Close();
                }

            }

            if (Parametres.debug == "all")
            {
                MainForm.writeLog("mainForm.cs->load : chargement de la configuration terminé");
            }

            // timer pour mettre à jour la base de données quand le logiciel est actif (tab_computer.lastetat)
            timer_MAJEtat.Interval = 5000; 
            timer_MAJEtat.Tick += new EventHandler(timer_MAJEtat_Tick);
            timer_MAJEtat.Start();
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {

            if (Parametres.debug == "all")
            {
                MainForm.writeLog("mainForm.cs->MainForm_Shown");
            }


            notifyIcon1.Icon = Properties.Resources.logo_256;
            notifyIcon1.Text = "LGBConnect";
            notifyIcon1.Visible = true;
            notifyIcon1.ShowBalloonTip(5000, "LGB Connect", "Chargé !", ToolTipIcon.Info);
            groupBox_inscription.Hide();
            this.Hide();

            // récupération de l'id et du nom de la salle
            poste = new Poste(Parametres.poste_id);
            Salle salle = new Salle(Parametres.poste_nom);
            Espace espace = new Espace(salle.idEspace);

            lbl_Espace.Text = espace.nom + "\n" + salle.nom;

            // on vérifie que pour l'espace sélectionné, il y a une config logiciel dans cyberGestionnaire.
            // Pour le moment, cette configuration ne sert qu'à déterminer s'il faut afficher la page de préinscription
            configLogiciel = new ConfigLogiciel(salle.idEspace);

            if (configLogiciel.exists() && configLogiciel.pageInscription)
            {
                groupBox_inscription.Show();
                if (Parametres.debug == "all")
                {
                    MainForm.writeLog("mainForm.cs->MainForm_Shown : préinscription activée !");
                }
            }
            else
            {
                groupBox_inscription.Hide();
                if (Parametres.debug == "all")
                {
                    MainForm.writeLog("mainForm.cs->MainForm_Shown : préinscription désactivée");
                }
            }

            // verification en base si une reservation est deja active sur le poste
            // si c'est le cas, il y a eu un problème avec le logiciel...
            // 2 solutions possibles :
            // - on clot de force la réservation. On pourrait analyser le temps passé en fonction des données de la résa,
            //   mais si on considère qu'il y a eu un probleme, ce n'est sans doute pas la bonne approche.
            //   Il est sans doute préférable de clore la résa avec une durée = 0 pour ne pas pénaliser l'usager
            // - on rouvre le poste avec l'identifiant trouvé dans la résa. Charge ensuite à frm_temps de se dépatouiller avec ca.
            //   Ce n'est pas l'approche que je retiens

            int idResa = Resa.verifierResaEnCours(Parametres.poste_id);
            while (idResa != 0)
            {
                Resa resa = new Resa(idResa);
                resa.annuler();
                idResa = Resa.verifierResaEnCours(Parametres.poste_id);
            }


            if (Parametres.poste_type == "usager")
            {
                if (Parametres.debug == "all")
                {
                    MainForm.writeLog("mainForm.cs->MainForm_Shown : poste usager : blocage demandé");
                }

                Fonction.blocageGestionnaireDesTaches(true);
                Fonction.blocageChangementMotDePasse(true);
                goFullscreen(true);
            }
            if (Parametres.poste_type == "animateur")
            {
                if (Parametres.debug == "all")
                {
                    MainForm.writeLog("mainForm.cs->MainForm_Shown : poste animateur");
                }

            }
            this.Show();
            timer_MAJEtat_Tick(null, null);
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (Parametres.debug == "all")
            {
                MainForm.writeLog("mainForm.cs->MainForm_FormClosed : demande de déblocage et nettoyage");
            }

            // déblocage systématique
            blocageRaccourcisClavier(false);
            Fonction.blocageGestionnaireDesTaches(false);
            Fonction.blocageChangementMotDePasse(false);
            timer_MAJEtat.Stop();

            if (Parametres.debug == "all")
            {
                MainForm.writeLog("mainForm.cs->MainForm_FormClosed : fin du nettoyage");
            }

            Application.Exit();
        }

        private void btn_inscription_Click(object sender, EventArgs e)
        {
            if (Parametres.debug == "all")
            {
                MainForm.writeLog("mainForm.cs->btn_inscription_Click");
            }
            frm_preinscription form_preinscription = new frm_preinscription(this);
            this.TopMost = false;
            if (Parametres.poste_type == "usager")
            {
                form_preinscription.TopMost = true;
                form_preinscription.FormBorderStyle = FormBorderStyle.None;
                form_preinscription.WindowState = FormWindowState.Maximized;
                form_preinscription.Bounds = Screen.PrimaryScreen.Bounds;
            }
            form_preinscription.Show();
        }

        private void btn_Connexion_Click(object sender, EventArgs e)
        {
            if (Parametres.debug == "all")
            {
                MainForm.writeLog("mainForm.cs->btn_Connexion_Click");
            }

            Utilisateur utilisateur = new Utilisateur(textBox_Utilisateur.Text, textBox_MotDePasse.Text);

            if (utilisateur.id != 0)
            {

                if (utilisateur.statut != 1) // cas du login animateur/administrateur
                {
                    login(utilisateur);
                }
                else
                {
                    // est ce qu'il existe une résa future ?
                    if (prochaineResa != null && prochaineResa.id != 0)
                    {
                        // combien de temps reste t'il avant le début de la session ?
                        TimeSpan diff = DateTime.Now - prochaineResa.debutDeSession;
                        if (diff.TotalMinutes > -5 && diff.TotalMinutes < prochaineResa.duree) // on verrouille 5 minutes avant
                        {
                            // dans un intervalle de 5 minutes avant jusqu'à la fin théorique de la résa
                            // seul l'utilisateur ayant effectué la résa peut se logguer
                            if (utilisateur.login == prochainUtilisateur.login)
                            {
                                prochaineResa.activer();
                                login(utilisateur);
                            }
                            else
                            {
                                MessageBox.Show("Poste réservé à " + prochainUtilisateur.prenom + " " + prochainUtilisateur.nom + " !!", "Login impossible sur ce poste");
                            }
                        }
                        else
                        {
                            login(utilisateur);
                        }
                    }
                    else
                    {
                        // pas de résa à venir
                        login(utilisateur);
                    }
                }
            }
            else
            {
                MessageBox.Show("login ou mot de passe inconnu");
            }
        }

        private void login(Utilisateur utilisateur)
        {
            if (Parametres.debug == "all")
            {
                MainForm.writeLog("mainForm.cs->login");
                MainForm.writeLog("mainForm.cs->login : login ok");
                MainForm.writeLog("nom utilisateur : " + utilisateur.nom);
                MainForm.writeLog("prenom utilisateur : " + utilisateur.prenom);
                MainForm.writeLog("id utilisateur : " + utilisateur.id);
                MainForm.writeLog("statut utilisateur : " + utilisateur.statut);
            }

            utilisateur.majDerniereVisite();

            Boolean estAnimateur = (utilisateur.statut != 1);
            Boolean estPosteAnimateur = (Parametres.poste_type != "usager");


            if (Parametres.debug == "all")
            {
                if (estAnimateur)
                {
                    MainForm.writeLog("mainForm.cs->login : connexion animateur");
                }
                else
                {
                    MainForm.writeLog("mainForm.cs->login : usager standard");
                    MainForm.writeLog("temps restant : " + utilisateur.tempsRestant());
                }
            }

            if (estAnimateur || ( utilisateur.tempsRestant() > 0 && !utilisateur.estConnecte(Parametres.poste_id) ) )
            {
                if (Parametres.debug == "all")
                {
                    MainForm.writeLog("mainForm.cs->login : temps ok, demandes de blocages et affichage du temps");
                }
                goFullscreen(!estPosteAnimateur);
                blocageMenu(!estAnimateur);
                Fonction.blocageGestionnaireDesTaches(!estAnimateur);
                Fonction.blocageChangementMotDePasse(!estAnimateur);
                blocageRaccourcisClavier(false);

                this.Hide();
                frmTemps = new frm_Temps(utilisateur, configLogiciel);
                frmTemps.ShowInTaskbar = false;
                frmTemps.ShowDialog();

                utilisateur.majDerniereVisite();
                // remise en place des blocages en fonction de la configuration du poste
                if (Parametres.debug == "all")
                {
                    MainForm.writeLog("mainForm.cs->login : remise en place des blocages");
                }

                this.Show();

                goFullscreen(!estPosteAnimateur);
                blocageMenu(!estPosteAnimateur);
                blocageRaccourcisClavier(!estPosteAnimateur);
                Fonction.blocageGestionnaireDesTaches(!estPosteAnimateur);
                Fonction.blocageChangementMotDePasse(!estPosteAnimateur);

                resetFormLogin();
            }
            else
            {
                if (utilisateur.tempsRestant() <= 0)
                {
                    MessageBox.Show("Crédit temps dépassé !!");
                }
                if (utilisateur.estConnecte(Parametres.poste_id))
                {
                    MessageBox.Show("Utilisateur déjà connecté !!");
                }
            }
        }

        /// <summary>
        /// Menu pour l'icone
        /// </summary>
        private void CreateContextMenu()
        {
            if (Parametres.debug == "all")
            {
                MainForm.writeLog("mainForm.cs->CreateContextMenu");
            }
            ContextMenuStrip menuStrip = new ContextMenuStrip();
            menuItemParametres = new ToolStripMenuItem("Parametres");
            menuItemParametres.Name = "Parametres";
            menuItemFinSession = new ToolStripMenuItem("Fin de session");
            menuItemFinSession.Name = "FinSession";
            menuItemQuitter = new ToolStripMenuItem("Quitter");
            menuItemQuitter.Name = "Quitter";
            menuItemParametres.Click += new EventHandler(menuItem_Click);
            menuItemFinSession.Click += new EventHandler(menuItem_Click);
            menuItemQuitter.Click += new EventHandler(menuItem_Click);
            menuStrip.Items.Add(menuItemParametres);
            menuStrip.Items.Add(menuItemFinSession);
            menuStrip.Items.Add(menuItemQuitter);
            notifyIcon1.ContextMenuStrip = menuStrip;
        }

        void menuItem_Click(object sender, EventArgs e)
        {
            if (Parametres.debug == "all")
            {
                MainForm.writeLog("mainForm.cs->menuItem_Click");
            }
            ToolStripItem menuItem = (ToolStripItem)sender;
            if (menuItem.Name == "Parametres")
            {
                assistantConfig assistant = new assistantConfig();
                assistant.ShowDialog();
                if (Parametres.poste_type == "usager")
                {
                    goFullscreen(true);
                }
                if (Parametres.poste_type == "animateur")
                {
                    goFullscreen(false);
                }

            }
            if (menuItem.Name == "FinSession")
            {
                if (frmTemps != null) {
                    if (Parametres.poste_type == "usager")
                    {
                        goFullscreen(true);
                    }
                    if (Parametres.poste_type == "animateur")
                    {
                        goFullscreen(false);
                    }
                    frmTemps.Close();
                }
            }
            if (menuItem.Name == "Quitter")
            {
                Fonction.blocageGestionnaireDesTaches(false);
                Fonction.blocageChangementMotDePasse(false);
                this.Close();
            }
        }

        private void resetFormLogin()
        {
            if (Parametres.debug == "all")
            {
                MainForm.writeLog("mainForm.cs->resetFormLogin");
            }
            textBox_MotDePasse.Text = "";
            textBox_Utilisateur.Text = "";
            textBox_Utilisateur.Focus();
        }

        /// <summary>
        /// Forcage en plein écran
        /// </summary>
        /// <param name="go"></param>
        private void goFullscreen(bool go)
        {
            if (Parametres.debug == "all")
            {
                MainForm.writeLog("mainForm.cs->goFullscreen");
            }
            if (go)
            {
                this.TopMost = true;
                this.FormBorderStyle = FormBorderStyle.None;
                this.WindowState = FormWindowState.Maximized;
                this.Bounds = Screen.PrimaryScreen.Bounds;
                blocageRaccourcisClavier(true);
            }
            else
            {
                this.FormBorderStyle = FormBorderStyle.Sizable;
                this.TopMost = false;
                this.WindowState = FormWindowState.Normal;
                this.Size = new Size(640, 480);
                this.CenterToScreen();
                blocageRaccourcisClavier(false);
            }
        }

        private void blocageMenu(bool blocage)
        {
            if (Parametres.debug == "all")
            {
                MainForm.writeLog("mainForm.cs->blocageMenu");
            }
            if (blocage)
            {
                menuItemParametres.Enabled = false;
                menuItemQuitter.Enabled = false;
            }
            else
            {
                menuItemParametres.Enabled = true;
                menuItemQuitter.Enabled = true;
            }
        }

        private void blocageRaccourcisClavier(bool blocage)
        {
            if (Parametres.debug == "all")
            {
                MainForm.writeLog("mainForm.cs->blocageRaccourcisClavier");
            }

            if (blocage)
            {
                Program.kh.blocageActif = true;
            }
            else
            {
                Program.kh.blocageActif = false;
            }

        }



        /// <summary>
        /// Mise à jour des données de connexion dans la base CyberGestionnaire.
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_MAJEtat_Tick(object sender, EventArgs e)
        {
            if (Parametres.debug == "all")
            {
                MainForm.writeLog("mainForm.cs->timer_MAJEtat_Tick : Tick 5s !");
            }

            // vérification des réservations actives
            int idResa = Resa.prochaineResa(poste.id);
            if (idResa != 0)
            {
                prochaineResa = new Resa(idResa);
                prochainUtilisateur = new Utilisateur(prochaineResa.idUtilisateur);

                TimeSpan diff = DateTime.Now - prochaineResa.debutDeSession;

                lbl_resa_texte.Text = "Prochaine réservation du poste";
                lbl_resa.Text = prochaineResa.dateResa.AddMinutes(prochaineResa.debut).ToString("G") + " (durée : " + prochaineResa.duree + " mn)";

                if (diff.TotalMinutes > -5 && diff.TotalMinutes < prochaineResa.duree) // on verrouille 5 minutes avant
                {
                    lbl_resa.Text = lbl_resa.Text + "\n (poste verrouillé pour " + prochainUtilisateur.prenom + " " + prochainUtilisateur.nom + ")";
                    lbl_resa.ForeColor = System.Drawing.Color.Red;
                } else {
                    lbl_resa.ForeColor = System.Drawing.Color.Blue;
                }
            }
            else
            {
                lbl_resa.Text = "";
                lbl_resa_texte.Text = "";
            }
        }

        public static void writeLog(string message)
        {
            ServiceFonctionsAdmin.FonctionsAdminClient client = new ServiceFonctionsAdmin.FonctionsAdminClient();
            client.writeLog(message);
        }

    }
}
