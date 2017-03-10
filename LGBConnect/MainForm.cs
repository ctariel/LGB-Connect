using System;
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
            Parametres.Debug = "no"; // par défaut

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

            if (Parametres.Debug == "all")
            {
                MainForm.WriteLog("mainForm.cs->load : chargement de la configuration terminé");
            }

            // timer pour mettre à jour la base de données quand le logiciel est actif (tab_computer.lastetat)
            timer_MAJEtat.Interval = 5000; 
            timer_MAJEtat.Tick += new EventHandler(Timer_MAJEtat_Tick);
            timer_MAJEtat.Start();
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {

            if (Parametres.Debug == "all")
            {
                MainForm.WriteLog("mainForm.cs->MainForm_Shown");
            }


            notifyIcon1.Icon = Properties.Resources.logo_256;
            notifyIcon1.Text = "LGBConnect";
            notifyIcon1.Visible = true;
            notifyIcon1.ShowBalloonTip(5000, "LGB Connect", "Chargé !", ToolTipIcon.Info);
            groupBox_inscription.Hide();
            this.Hide();

            // récupération de l'id et du nom de la salle
            poste = new Poste(Parametres.Poste_id);
            Salle salle = new Salle(Parametres.Poste_nom);
            Espace espace = new Espace(salle.IdEspace);

            lbl_Espace.Text = espace.Nom + "\n" + salle.Nom;

            // on vérifie que pour l'espace sélectionné, il y a une config logiciel dans cyberGestionnaire.
            // Pour le moment, cette configuration ne sert qu'à déterminer s'il faut afficher la page de préinscription
            configLogiciel = new ConfigLogiciel(salle.IdEspace);

            if (configLogiciel.Exists() && configLogiciel.PageInscription)
            {
                groupBox_inscription.Show();
                if (Parametres.Debug == "all")
                {
                    MainForm.WriteLog("mainForm.cs->MainForm_Shown : préinscription activée !");
                }
            }
            else
            {
                groupBox_inscription.Hide();
                if (Parametres.Debug == "all")
                {
                    MainForm.WriteLog("mainForm.cs->MainForm_Shown : préinscription désactivée");
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

            int idResa = Resa.VerifierResaEnCours(Parametres.Poste_id);
            while (idResa != 0)
            {
                Resa resa = new Resa(idResa);
                resa.Annuler();
                idResa = Resa.VerifierResaEnCours(Parametres.Poste_id);
            }


            if (Parametres.Poste_type == "usager")
            {
                if (Parametres.Debug == "all")
                {
                    MainForm.WriteLog("mainForm.cs->MainForm_Shown : poste usager : blocage demandé");
                }

                Fonction.blocageGestionnaireDesTaches(true);
                Fonction.blocageChangementMotDePasse(true);
                GoFullscreen(true);
            }
            if (Parametres.Poste_type == "animateur")
            {
                if (Parametres.Debug == "all")
                {
                    MainForm.WriteLog("mainForm.cs->MainForm_Shown : poste animateur");
                }

            }
            this.Show();
            Timer_MAJEtat_Tick(null, null);
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (Parametres.Debug == "all")
            {
                MainForm.WriteLog("mainForm.cs->MainForm_FormClosed : demande de déblocage et nettoyage");
            }

            // déblocage systématique
            BlocageRaccourcisClavier(false);
            Fonction.blocageGestionnaireDesTaches(false);
            Fonction.blocageChangementMotDePasse(false);
            timer_MAJEtat.Stop();

            if (Parametres.Debug == "all")
            {
                MainForm.WriteLog("mainForm.cs->MainForm_FormClosed : fin du nettoyage");
            }

            Application.Exit();
        }

        private void Btn_inscription_Click(object sender, EventArgs e)
        {
            if (Parametres.Debug == "all")
            {
                MainForm.WriteLog("mainForm.cs->btn_inscription_Click");
            }
            Frm_preinscription form_preinscription = new Frm_preinscription(this);
            this.TopMost = false;
            if (Parametres.Poste_type == "usager")
            {
                form_preinscription.TopMost = true;
                form_preinscription.FormBorderStyle = FormBorderStyle.None;
                form_preinscription.WindowState = FormWindowState.Maximized;
                form_preinscription.Bounds = Screen.PrimaryScreen.Bounds;
            }
            form_preinscription.Show();
        }

        private void Btn_Connexion_Click(object sender, EventArgs e)
        {
            if (Parametres.Debug == "all")
            {
                MainForm.WriteLog("mainForm.cs->btn_Connexion_Click");
            }

            Utilisateur utilisateur = new Utilisateur(textBox_Utilisateur.Text, textBox_MotDePasse.Text);

            if (utilisateur.Id != 0)
            {
                if (utilisateur.Statut != 1) // cas du login animateur/administrateur
                {
                    Login(utilisateur);
                }
                else
                {
                    if (utilisateur.AUnForfaitValide())
                    {
                        // est ce qu'il existe une résa future ?
                        if (prochaineResa != null && prochaineResa.Id != 0)
                        {
                            // combien de temps reste t'il avant le début de la session ?
                            TimeSpan diff = DateTime.Now - prochaineResa.DebutDeSession;
                            if (diff.TotalMinutes > -5 && diff.TotalMinutes < prochaineResa.Duree) // on verrouille 5 minutes avant
                            {
                                // dans un intervalle de 5 minutes avant jusqu'à la fin théorique de la résa
                                // seul l'utilisateur ayant effectué la résa peut se logguer
                                if (utilisateur.Login == prochainUtilisateur.Login)
                                {
                                    prochaineResa.Activer();
                                    Login(utilisateur);
                                }
                                else
                                {
                                    MessageBox.Show("Poste réservé à " + prochainUtilisateur.Prenom + " " + prochainUtilisateur.Nom + " !!", "Login impossible sur ce poste");
                                }
                            }
                            else
                            {
                                Login(utilisateur);
                            }
                        }
                        else
                        {
                            // pas de résa à venir
                            Login(utilisateur);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Forfait invalide ! Veuillez contacter l'animateur de l'espace.");
                    }
                }
            }
            else
            {
                MessageBox.Show("login ou mot de passe inconnu");
            }
        }

        private void Login(Utilisateur utilisateur)
        {
            if (Parametres.Debug == "all")
            {
                MainForm.WriteLog("mainForm.cs->login");
                MainForm.WriteLog("mainForm.cs->login : login ok");
                MainForm.WriteLog("nom utilisateur : " + utilisateur.Nom);
                MainForm.WriteLog("prenom utilisateur : " + utilisateur.Prenom);
                MainForm.WriteLog("id utilisateur : " + utilisateur.Id);
                MainForm.WriteLog("statut utilisateur : " + utilisateur.Statut);
                MainForm.WriteLog("Validité du forfait : " + utilisateur.AUnForfaitValide().ToString());
            }

            utilisateur.MajDerniereVisite();

            int tempsRestant = utilisateur.TempsRestant(); // je stocke la valeur pour éviter de multiples interrogations à la base


            Boolean estAnimateur = (utilisateur.Statut != 1);
            Boolean estPosteAnimateur = (Parametres.Poste_type != "usager");


            if (Parametres.Debug == "all")
            {
                if (estAnimateur)
                {
                    MainForm.WriteLog("mainForm.cs->login : connexion animateur");
                }
                else
                {
                    MainForm.WriteLog("mainForm.cs->login : usager standard");
                    MainForm.WriteLog("temps restant : " + tempsRestant);
                }
            }

            if (estAnimateur || (tempsRestant > 0 && !utilisateur.EstConnecte(Parametres.Poste_id) ) )
            {
                if (Parametres.Debug == "all")
                {
                    MainForm.WriteLog("mainForm.cs->login : temps ok, demandes de blocages et affichage du temps");
                }
                GoFullscreen(!estPosteAnimateur);
                BlocageMenu(!estAnimateur);
                Fonction.blocageGestionnaireDesTaches(!estAnimateur);
                Fonction.blocageChangementMotDePasse(!estAnimateur);
                BlocageRaccourcisClavier(false);

                this.Hide();

                // a cet endroit, prévoir un affichage pour statistiques (tab_utilisation dans cybergestionnaire)

                frmTemps = new frm_Temps(utilisateur, configLogiciel)
                {
                    ShowInTaskbar = false
                };
                frmTemps.ShowDialog();

                utilisateur.MajDerniereVisite();
                // remise en place des blocages en fonction de la configuration du poste
                if (Parametres.Debug == "all")
                {
                    MainForm.WriteLog("mainForm.cs->login : remise en place des blocages");
                }

                this.Show();

                GoFullscreen(!estPosteAnimateur);
                BlocageMenu(!estPosteAnimateur);
                BlocageRaccourcisClavier(!estPosteAnimateur);
                Fonction.blocageGestionnaireDesTaches(!estPosteAnimateur);
                Fonction.blocageChangementMotDePasse(!estPosteAnimateur);

                ResetFormLogin();
            }
            else
            {
                if (tempsRestant <= 0)
                {
                    MessageBox.Show("Crédit temps dépassé !!");
                }
                if (utilisateur.EstConnecte(Parametres.Poste_id))
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
            if (Parametres.Debug == "all")
            {
                MainForm.WriteLog("mainForm.cs->CreateContextMenu");
            }
            ContextMenuStrip menuStrip = new ContextMenuStrip();
            menuItemParametres = new ToolStripMenuItem("Parametres")
            {
                Name = "Parametres"
            };
            menuItemFinSession = new ToolStripMenuItem("Fin de session")
            {
                Name = "FinSession"
            };
            menuItemQuitter = new ToolStripMenuItem("Quitter")
            {
                Name = "Quitter"
            };
            menuItemParametres.Click += new EventHandler(MenuItem_Click);
            menuItemFinSession.Click += new EventHandler(MenuItem_Click);
            menuItemQuitter.Click += new EventHandler(MenuItem_Click);
            menuStrip.Items.Add(menuItemParametres);
            menuStrip.Items.Add(menuItemFinSession);
            menuStrip.Items.Add(menuItemQuitter);
            notifyIcon1.ContextMenuStrip = menuStrip;
        }

        void MenuItem_Click(object sender, EventArgs e)
        {
            if (Parametres.Debug == "all")
            {
                MainForm.WriteLog("mainForm.cs->menuItem_Click");
            }
            ToolStripItem menuItem = (ToolStripItem)sender;
            if (menuItem.Name == "Parametres")
            {
                assistantConfig assistant = new assistantConfig();
                assistant.ShowDialog();
                if (Parametres.Poste_type == "usager")
                {
                    GoFullscreen(true);
                }
                if (Parametres.Poste_type == "animateur")
                {
                    GoFullscreen(false);
                }

            }
            if (menuItem.Name == "FinSession")
            {
                if (frmTemps != null) {
                    if (Parametres.Poste_type == "usager")
                    {
                        GoFullscreen(true);
                    }
                    if (Parametres.Poste_type == "animateur")
                    {
                        GoFullscreen(false);
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

        private void ResetFormLogin()
        {
            if (Parametres.Debug == "all")
            {
                MainForm.WriteLog("mainForm.cs->resetFormLogin");
            }
            textBox_MotDePasse.Text = "";
            textBox_Utilisateur.Text = "";
            textBox_Utilisateur.Focus();
        }

        /// <summary>
        /// Forcage en plein écran
        /// </summary>
        /// <param name="go"></param>
        private void GoFullscreen(bool go)
        {
            if (Parametres.Debug == "all")
            {
                MainForm.WriteLog("mainForm.cs->goFullscreen");
            }
            if (go)
            {
                this.TopMost = true;
                this.FormBorderStyle = FormBorderStyle.None;
                this.WindowState = FormWindowState.Maximized;
                this.Bounds = Screen.PrimaryScreen.Bounds;
                BlocageRaccourcisClavier(true);
            }
            else
            {
                this.FormBorderStyle = FormBorderStyle.Sizable;
                this.TopMost = false;
                this.WindowState = FormWindowState.Normal;
                this.Size = new Size(640, 480);
                this.CenterToScreen();
                BlocageRaccourcisClavier(false);
            }
        }

        private void BlocageMenu(bool blocage)
        {
            if (Parametres.Debug == "all")
            {
                MainForm.WriteLog("mainForm.cs->blocageMenu");
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

        private void BlocageRaccourcisClavier(bool blocage)
        {
            if (Parametres.Debug == "all")
            {
                MainForm.WriteLog("mainForm.cs->blocageRaccourcisClavier");
            }

            if (blocage)
            {
                Program.kh.BlocageActif = true;
            }
            else
            {
                Program.kh.BlocageActif = false;
            }

        }



        /// <summary>
        /// Mise à jour des données de connexion dans la base CyberGestionnaire.
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timer_MAJEtat_Tick(object sender, EventArgs e)
        {
            if (Parametres.Debug == "all")
            {
                MainForm.WriteLog("mainForm.cs->timer_MAJEtat_Tick : Tick 5s !");
            }

            // vérification des réservations actives
            int idResa = Resa.ProchaineResa(poste.Id);
            if (idResa != 0)
            {
                prochaineResa = new Resa(idResa);
                prochainUtilisateur = new Utilisateur(prochaineResa.IdUtilisateur);

                TimeSpan diff = DateTime.Now - prochaineResa.DebutDeSession;

                lbl_resa_texte.Text = "Prochaine réservation du poste";
                lbl_resa.Text = prochaineResa.DateResa.AddMinutes(prochaineResa.Debut).ToString("G") + " (durée : " + prochaineResa.Duree + " mn)";

                if (diff.TotalMinutes > -5 && diff.TotalMinutes < prochaineResa.Duree) // on verrouille 5 minutes avant
                {
                    lbl_resa.Text = lbl_resa.Text + "\n (poste verrouillé pour " + prochainUtilisateur.Prenom + " " + prochainUtilisateur.Nom + ")";
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

        public static void WriteLog(string message)
        {
            ServiceFonctionsAdmin.FonctionsAdminClient client = new ServiceFonctionsAdmin.FonctionsAdminClient();
            client.writeLog(message);
        }

    }
}
