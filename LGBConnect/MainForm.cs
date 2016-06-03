﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using MySql.Data;
using MySql.Data.MySqlClient;
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

        public string login_utilisateur, login_motdepasse;
        public string nom_utilisateur, prenom_utilisateur;
        public int id_utilisateur, statut_utilisateur;
        public long id_resa;

        Timer timer_MAJEtat = new Timer();

        ToolStripMenuItem menuItemParametres, menuItemFinSession, menuItemQuitter;
        frm_Temps frmTemps;

        /* --- déclaration pour  le blocage des raccourcis claviers ---- */
        // Structure contain information about low-level keyboard input event
        [StructLayout(LayoutKind.Sequential)]
        private struct KBDLLHOOKSTRUCT
        {
            public Keys key;
            public int scanCode;
            public int flags;
            public int time;
            public IntPtr extra;
        }

        //System level functions to be used for hook and unhook keyboard input
        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int id, LowLevelKeyboardProc callback, IntPtr hMod, uint dwThreadId);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool UnhookWindowsHookEx(IntPtr hook);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hook, int nCode, IntPtr wp, IntPtr lp);
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string name);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern short GetAsyncKeyState(Keys key);


        //Declaring Global objects
        private IntPtr ptrHook;
        private LowLevelKeyboardProc objKeyboardProcess;

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
                    Application.Exit();
                }

            }

            if (Parametres.debug == "all")
            {
                MainForm.writeLog("mainForm.cs->load : chargement de la configuration terminé");
            }

            // timer pour mettre à jour la base de données quand le logiciel est actif (tab_computer.lastetat)
            timer_MAJEtat.Interval = 10000; 
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
            Salle salle = new Salle(Parametres.poste_nom);
            Espace espace = new Espace(salle.idEspace);

            lbl_Espace.Text = espace.nom + "\n" + salle.nom;

            // on vérifie que pour l'espace sélectionné, il y a une config logiciel dans cyberGestionnaire.
            // Pour le moment, cette configuration ne sert qu'à déterminer s'il faut afficher la page de préinscription
            ConfigLogiciel configLogiciel = new ConfigLogiciel(salle.idEspace);

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
        }


        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (Parametres.debug == "all")
            {
                MainForm.writeLog("mainForm.cs->MainForm_FormClosed : demande de déblocage et nettoyage");
            }

            // TODO : la form temps se ferme après l'application, ce qui fait que les registre sont réécrits après 
/*            if (frmTemps != null)
            {
                while (!frmTemps.IsDisposed)
                {
                    frmTemps.Close();
                }
            }*/
            // déblocage systématique
            blocageRaccourcisClavier(false);
            Fonction.blocageGestionnaireDesTaches(false);
            Fonction.blocageChangementMotDePasse(false);
            timer_MAJEtat.Stop();
            //exit application when form is closed
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

            this.login_utilisateur = textBox_Utilisateur.Text;
            this.login_motdepasse = textBox_MotDePasse.Text;

            MySqlConnection cnn = new MySqlConnection(Parametres.connectionString);
            try
            {
                //string sql = "SELECT `id_user`, `nom_user`, `prenom_user`, `status_user` FROM `tab_user` WHERE `login_user`= '" + this.login_utilisateur +"' AND `pass_user`='" + Fonction.MD5Hash(this.login_motdepasse) + "' LIMIT 0,1";
                string sql = "SELECT `id_user`, `nom_user`, `prenom_user`, `status_user` FROM `tab_user` WHERE `login_user`= @login_user AND `pass_user`= @pass_user LIMIT 0,1";
                cnn.Open();
                MySqlCommand cmd = new MySqlCommand(sql, cnn);
                cmd.Parameters.AddWithValue("@login_user", this.login_utilisateur);
                cmd.Parameters.AddWithValue("@pass_user", Fonction.MD5Hash(this.login_motdepasse));

                if (Parametres.debug == "all")
                {
                    MainForm.writeLog("mainForm.cs->btn_Connexion_Click : requete sql -------------------");

                    string query = cmd.CommandText;
                    foreach (MySqlParameter p in cmd.Parameters)
                    {
                        query = query.Replace(p.ParameterName, p.Value.ToString());
                    }
                    MainForm.writeLog(query);
                }

                MySqlDataReader rdr = cmd.ExecuteReader();

                if (rdr.HasRows)
                {
                    // authentification correcte
                    while (rdr.Read())
                    {
                        nom_utilisateur = (String)rdr["nom_user"];
                        prenom_utilisateur = (String)rdr["prenom_user"];
                        id_utilisateur = (int)rdr["id_user"];
                        statut_utilisateur = (int)rdr["status_user"];

                        if (Parametres.debug == "all")
                        {
                            MainForm.writeLog("mainForm.cs->btn_Connexion_Click : login ok");
                            MainForm.writeLog("nom utilisateur : " + nom_utilisateur);
                            MainForm.writeLog("prenom utilisateur : " + prenom_utilisateur);
                            MainForm.writeLog("id utilisateur : " + id_utilisateur);
                            MainForm.writeLog("statut utilisateur : " + statut_utilisateur);
                        }

                        // il y a un grand ménage à faire dans cette fonction !!! beaucoup de trop de redondances !!

                        if (statut_utilisateur != 1) // admin ou animateur
                        {
                            if (Parametres.debug == "all")
                            {
                                MainForm.writeLog("mainForm.cs->btn_Connexion_Click : connexion animateur");
                            }
                            goFullscreen(false);
                            blocageMenu(false);
                            Fonction.blocageGestionnaireDesTaches(false);
                            Fonction.blocageChangementMotDePasse(false);
                            this.Hide();
                            frmTemps = new frm_Temps(this);
                            frmTemps.ShowInTaskbar = false;
                            frmTemps.ShowDialog();
                            if (Parametres.poste_type == "usager")
                            {
                                if (Parametres.debug == "all")
                                {
                                    MainForm.writeLog("mainForm.cs->btn_Connexion_Click : remise en place des blocages après connexion animateur");
                                }
                                goFullscreen(true);
                                blocageMenu(true);
                                Fonction.blocageGestionnaireDesTaches(true);
                                Fonction.blocageChangementMotDePasse(true);
                            }
                            if (Parametres.poste_type == "animateur")
                            {
                                goFullscreen(false);
                            }

                            this.Show();
                            resetFormLogin();
                        }
                        else // usager standard
                        {
                            if (Parametres.debug == "all")
                            {
                                MainForm.writeLog("mainForm.cs->btn_Connexion_Click : usager standard");
                                MainForm.writeLog("temps restant : " + Fonction.get_temps_restant(id_utilisateur, Parametres.connectionString));
                            }
                            if (Fonction.get_temps_restant(id_utilisateur, Parametres.connectionString) > 0)
                            {
                                if (Parametres.debug == "all")
                                {
                                    MainForm.writeLog("mainForm.cs->btn_Connexion_Click : temps ok, demandes de blocages et affichage du temps");
                                }

                                blocageMenu(true);
                                Fonction.blocageGestionnaireDesTaches(true);
                                Fonction.blocageChangementMotDePasse(true);
                                this.Hide();
                                frmTemps = new frm_Temps(this);
                                frmTemps.ShowInTaskbar = false;
                                frmTemps.ShowDialog();
                                if (Parametres.debug == "all")
                                {
                                    MainForm.writeLog("mainForm.cs->btn_Connexion_Click : frmTemps fermée");
                                }

                                this.Show();
                                resetFormLogin();
                            }
                            else
                            {
                                MessageBox.Show("Crédit temps dépassé !!");
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("login ou mot de passe inconnu");
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Connexion echouée !!" + ex.ToString());
            }
            cnn.Close();

        }

        /// <summary>
        /// Menu pour l'icone
        /// </summary>
        private void CreateContextMenu()
        {
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
            //MessageBox.Show("Bloquer les raccourcis clavier : " + blocage);
            if (blocage)
            {
                ProcessModule objCurrentModule = Process.GetCurrentProcess().MainModule;
                objKeyboardProcess = new LowLevelKeyboardProc(captureKey);
                ptrHook = SetWindowsHookEx(13, objKeyboardProcess, GetModuleHandle(objCurrentModule.ModuleName), 0);

            }
            else
            {
                UnhookWindowsHookEx(ptrHook);
            }

        }
        /// <summary>
        /// Blocage des touches clavier 
        /// 
        /// Pas encore réussi à bloquer le Crtl-alt-suppr...
        /// </summary>
        /// <param name="nCode"></param>
        /// <param name="wp"></param>
        /// <param name="lp"></param>
        /// <returns></returns>
        private IntPtr captureKey(int nCode, IntPtr wp, IntPtr lp)
        {
            if (nCode >= 0)
            {
                KBDLLHOOKSTRUCT objKeyInfo = (KBDLLHOOKSTRUCT)Marshal.PtrToStructure(lp, typeof(KBDLLHOOKSTRUCT));

                if (objKeyInfo.key == Keys.RWin ||
                    objKeyInfo.key == Keys.LWin ||
                    objKeyInfo.key == Keys.Tab && HasAltModifier(objKeyInfo.flags) ||
                    objKeyInfo.key == Keys.Escape && (ModifierKeys & Keys.Control) == Keys.Control ||
                    objKeyInfo.key == Keys.Escape && HasAltModifier(objKeyInfo.flags) ||
                    objKeyInfo.key == Keys.F4 && HasAltModifier(objKeyInfo.flags)
                   )
                {
                    return (IntPtr)1;
                }
                if (objKeyInfo.key == Keys.Alt)
                {
                    return (IntPtr)1;
                }
            }
            return CallNextHookEx(ptrHook, nCode, wp, lp);
        }

        bool HasAltModifier(int flags)
        {
            return (flags & 0x20) == 0x20;
        }

        /// <summary>
        /// Mise à jour des données de connexion dans la base CyberGestionnaire.
        /// La date est mise par le moteur SQL, et les secondes du jour sont calculées dans la fonction
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_MAJEtat_Tick(object sender, EventArgs e)
        {
            MySqlConnection cnn = new MySqlConnection(Parametres.connectionString);
            try
            {
                cnn.Open();

                String sql = "UPDATE `tab_computer` SET `date_lastetat_computer`= CURRENT_DATE(), `lastetat_computer`= " + ((DateTime.Now.Minute + DateTime.Now.Hour * 60) * 60 + DateTime.Now.Second).ToString() + " WHERE `id_computer`= '" + Parametres.poste_id + "'";
                //sprintf(chainesql, "UPDATE tab_resa SET `duree_resa`='%d', `status_resa`='%d' WHERE `id_resa`='%d' ", *temps_passer, *status_resa, *id_resa);
                

                MySqlCommand cmd = new MySqlCommand(sql, cnn);
                MySqlDataReader rdr = cmd.ExecuteReader();
                rdr.Close();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Connexion echouée !!" + ex.ToString());
            }
            cnn.Close();
        }

        public static void writeLog(string message)
        {
            ServiceFonctionsAdmin.FonctionsAdminClient client = new ServiceFonctionsAdmin.FonctionsAdminClient();
            client.writeLog(message);
        }

    }
}
