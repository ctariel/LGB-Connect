using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using LGBConnect.classes;

namespace LGBConnect
{
    /// <summary>
    /// Boite d'affichage du temps restant
    /// </summary>
    public partial class frm_Temps : Form
    {
        System.Windows.Forms.Timer t = new System.Windows.Forms.Timer();
        DateTime heureConnexion, heureDeconnexion;
        Utilisateur utilisateur = null;
        ConfigLogiciel configLogiciel = null;
        Resa resa = null;
        Resa prochaineResa = null;

        frm_MsgBox msgBox_deconnexion;

        public frm_Temps(Utilisateur unUtilisateur, ConfigLogiciel uneConfigLogicielle)
        {
            InitializeComponent();
            utilisateur = unUtilisateur;
            configLogiciel = uneConfigLogicielle;
        }

        private void frm_Temps_Load(object sender, EventArgs e)
        {
            if (Parametres.debug == "all")
            {
                MainForm.writeLog("frm_Temps.cs->frm_Temps_Load");
            }

            this.FormBorderStyle = FormBorderStyle.None;
            this.Left = Screen.PrimaryScreen.WorkingArea.Width - this.Width;
            this.Top = Screen.PrimaryScreen.WorkingArea.Height - this.Height;
            this.StartPosition = FormStartPosition.Manual;
            this.TopMost = true;
            lbl_utilisateur.Text = utilisateur.prenom + " " + utilisateur.nom;
            this.ShowInTaskbar = false;



        }

        private void frm_Temps_Shown(object sender, EventArgs e)
        {
            if (Parametres.debug == "all")
            {
                MainForm.writeLog("frm_Temps.cs->frm_Temps_Shown");
            }
            // début de la session
            heureConnexion = DateTime.Now;
            if (Parametres.debug == "all")
            {
                MainForm.writeLog("frm_Temps.cs->frm_Temps_Load : début de session à " + heureConnexion.ToString());
            }

            t.Interval = 1000; // specify interval time as you want
            t.Tick += new EventHandler(timer_Tick);
            t.Start();

            if (Parametres.debug == "all")
            {
                MainForm.writeLog("frm_Temps.cs->frm_Temps_Load : temps restant = " + utilisateur.tempsRestant());
            }

            if (utilisateur.statut == 1)
            {
                // vérification de la présence d'une résa sur le poste
                // cas ou c'est une réservation future qui commence
                int idResaEnCours = Resa.verifierResaEnCours(Parametres.poste_id);
                if (idResaEnCours != 0)
                {
                    // il y a effectivement une résa en cours sur le poste...
                    resa = new Resa(idResaEnCours);
                    if (resa.duree > utilisateur.tempsRestant())
                    {
                        resa.duree = utilisateur.tempsRestant();
                    }
                    if (resa.idUtilisateur != utilisateur.id)
                    {
                        // ca ne devrait pas arriver ! Seul l'utilisateur ayant fait la résa future
                        // peut l'utiliser !
                        MessageBox.Show("Erreur dans le codage ! Ce message ne devrait jamais apparaitre !");
                        this.Close();
                    }
                }
                else
                {
                    // pas de resa, inscription d'une nouvelle réservation dans la base
                    resa = new Resa(utilisateur.id, utilisateur.tempsRestant(), heureConnexion);


                    // on essaye de voir combien de temps il reste avant
                    // une éventuelle future résa
                    int idResaFuture = Resa.prochaineResa(Parametres.poste_id);
                    if (idResaFuture != 0) // il existe une résa future pour le poste !
                    {
                        prochaineResa = new Resa(idResaFuture);

                        if (resa.finDeSession > prochaineResa.debutDeSession)
                        {

                            if (prochaineResa.idUtilisateur == utilisateur.id)
                            {
                                // cas ou l'utilisateur ayant réservé arrive plus tôt...
                                DialogResult dresult = MessageBox.Show("Vous avez réservé ce poste pour le " + prochaineResa.debutDeSession.ToString("G") + ". Voulez-vous utiliser cette réservation maintenant ?", "Attention !", MessageBoxButtons.YesNo);
                                if (dresult == DialogResult.Yes)
                                {
                                    prochaineResa.annuler();
                                }
                            }
                            else
                            {
                                TimeSpan diff = prochaineResa.debutDeSession.AddMinutes(-1) - heureConnexion;
                                MessageBox.Show("Ce poste a été reservé pour le " + prochaineResa.debutDeSession.ToString("G") + ". La session sur ce poste se terminera donc dans " + Math.Floor(diff.TotalMinutes) + " mn.", "Poste réservé prochainement !");
                                resa.duree = (int)Math.Floor(diff.TotalMinutes);
                            }
                        }
                    }
                }
            }
            else
            {
                // connexion admin
                resa = new Resa(utilisateur.id, utilisateur.tempsRestant(), heureConnexion);
            }


            if (resa == null)
            {
                MessageBox.Show("erreur dans l'initialisation de la résa.");
                this.Close();
            }

        }

        void timer_Tick(object sender, EventArgs e)
        {
            if (Parametres.debug == "all")
            {
                MainForm.writeLog("frm_Temps.cs->timer_Tick : 1s");
            }

            String affichage_restant, affichage_utilise;
            System.TimeSpan diffRestant, diffUtilise;

            DateTime heureDeconnexion = heureConnexion.AddMinutes(resa.duree);

            diffRestant = heureDeconnexion - DateTime.Now;
            diffUtilise = DateTime.Now - heureConnexion;


            if (utilisateur.tempsRestant() == 1440)  // le temps affecté est de 24h00, donc infini dans la pratique
            {
                affichage_restant = "";
                lbl_text_restant.Text = "";
            }
            else
            {
                if (diffRestant.Ticks >= DateTime.MinValue.Ticks)
                {
                    affichage_restant = new DateTime(diffRestant.Ticks).ToString("HH:mm:ss");
                }
                else
                {
                    affichage_restant = "00:00:00";
                }
            }

            affichage_utilise = new DateTime(diffUtilise.Ticks).ToString("HH:mm:ss");

            affichageChronos(affichage_restant, affichage_utilise);

            /*if (Parametres.debug == "all")
            {
                MainForm.writeLog("frm_Temps.cs->timer_Tick : " + lbl_temps_restant.Text);
            }*/

            // si la déconnexion automatique est activée
            if (configLogiciel.deconnexionAuto)
            {

                if (Parametres.debug == "all")
                {
                    MainForm.writeLog("frm_Temps.cs->timer_Tick : déconnexion auto active.");
                    MainForm.writeLog("frm_Temps.cs->timer_Tick : diffRestant.TotalMinutes = " + diffRestant.TotalMinutes + " / diffRestant.Seconds" + diffRestant.Seconds);
                }
                if ((Math.Floor(diffRestant.TotalMinutes)) == 2 && diffRestant.Seconds == 0)
                {
                    if (Parametres.debug == "all")
                    {
                        MainForm.writeLog("frm_Temps.cs->timer_Tick : déconnexion programmée dans 2 minutes.");
                    }

                    msgBox_deconnexion = new frm_MsgBox();
                    msgBox_deconnexion.Show("Déconnexion automatique dans 2 minutes ! Pensez à sauvegarder vos documents !", "Avertissement", 119000);
                    msgBox_deconnexion.TopMost = true;
                    lbl_temps_restant.ForeColor = System.Drawing.Color.Red;
                }
                if ((Math.Floor(diffRestant.TotalMinutes)) < 0)
                {

                    if (Parametres.debug == "all")
                    {
                        MainForm.writeLog("frm_Temps.cs->timer_Tick : message de déconnexion");
                    }

                    if (msgBox_deconnexion != null) { 
                        if (msgBox_deconnexion.IsDisposed)
                        {
                            msgBox_deconnexion = new frm_MsgBox();
                        }
                    }
                    msgBox_deconnexion.Show("Déconnexion automatique !! Au revoir !", "Avertissement", 10000);

                }
                if (diffRestant.TotalMinutes < -0.16) // environ 10 secondes
                {
                    if (!msgBox_deconnexion.IsDisposed)
                        msgBox_deconnexion.Close();
                    if (Parametres.debug == "all")
                    {
                        MainForm.writeLog("frm_Temps.cs->timer_Tick : déconnexion réelle");
                    }
                    this.Close();
                }
            }
            else
            {
                if (Parametres.debug == "all")
                {
                    MainForm.writeLog("frm_Temps.cs->timer_Tick : déconnexion auto desactive.");
                }
            }

            verificationStatutResa();
        }

        private void verificationStatutResa()
        {
            if (Parametres.debug == "all")
            {
                MainForm.writeLog("frm_Temps.cs->verificationStatutResa");
            }
            /// vérification du statut. Si le poste a été libéré depuis la console, status_resa est différent de zéro
            if (resa.statut != 0)
            {
                if (Parametres.debug == "all")
                {
                    MainForm.writeLog("frm_Temps.cs->timer_Tick : déconnexion forcée depuis la console");
                }
                msgBox_deconnexion = new frm_MsgBox();
                msgBox_deconnexion.Show("Déconnexion forcée depuis la console !", "Avertissement", 10000);
                // deconnexion forcée
                this.Close();
            }
        }

        private void frm_Temps_FormClosed(object sender, FormClosedEventArgs e)
        {

            if (Parametres.debug == "all")
            {
                MainForm.writeLog("frm_Temps.cs->frm_Temps_FormClosed");
            }
            heureDeconnexion = DateTime.Now;
            System.TimeSpan diff = heureDeconnexion - heureConnexion;
            Double temp_passe = Math.Floor(diff.TotalMinutes);
            //MessageBox.Show("diff " + diff.ToString());
            if (Parametres.debug == "all")
            {
                MainForm.writeLog("frm_Temps.cs->frm_Temps_FormClosed : heure de déconnexion : " + heureDeconnexion.ToString());
                MainForm.writeLog("frm_Temps.cs->frm_Temps_FormClosed : temps passé : " + temp_passe);
            }

            resa.clore((int)temp_passe);
            t.Stop();


            if (utilisateur.statut == 1)
            { // utilisateur standard
              //tuerLesProcess();
            }

        }

        private void affichageChronos(String affichage_restant, String affichage_utilise)
        {
            if (Parametres.debug == "all")
            {
                MainForm.writeLog("frm_Temps.cs->affichageChronos");
            }
            if (Parametres.poste_chrono == "complet")
            {
                lbl_temps_restant.Text = affichage_restant;
                lbl_temps_utilise.Text = affichage_utilise;
            }

            if (Parametres.poste_chrono == "restant")
            {
                lbl_temps_restant.Text = affichage_restant;
                lbl_temps_utilise.Text = "";
                lbl_text_utilise.Text = "";
            }
            if (Parametres.poste_chrono == "utilise")
            {
                lbl_temps_restant.Text = "";
                lbl_text_restant.Text = "";
                lbl_temps_utilise.Text = affichage_utilise;
            }
            if (Parametres.poste_chrono == "aucun")
            {
                lbl_temps_restant.Text = "";
                lbl_text_restant.Text = "";
                lbl_temps_utilise.Text = "";
                lbl_text_utilise.Text = "";
            }

        }
        private void btn_deconnexion_Click(object sender, EventArgs e)
        {
            if (Parametres.debug == "all")
            {
                MainForm.writeLog("frm_Temps.cs->btn_deconnexion_Click");
            }
            this.Close();
        }



        /// <summary>
        /// méthode un peu bourrin pour fermer les fenêtres ouvertes par l'utilisateur.
        /// Donne des résultats mitigés, désactivée pour le moment
        /// </summary>
        private void tuerLesProcess()
        {
            int nb_process = 0;

            // on essaie une fermeture propre
            // note : les explorateurs de  fichiers ne se ferment pas avec cette commande (MainWindowTitle = 0 ?)
            Process[] processes = Process.GetProcesses();
            foreach (var item in processes)
            {
                if (item != Process.GetCurrentProcess())   // don't kill me !!!!
                {
                    if (item.MainWindowTitle.Length > 0)
                    {
                        Console.WriteLine(item.MainWindowTitle);
                        if (item.MainWindowTitle != "frm_Temps" && item.MainWindowTitle != "LGBConnect (Débogage) - Microsoft Visual Studio (Administrateur)" && item.MainWindowTitle != "LGBConnect (Exécution) - Microsoft Visual Studio (Administrateur)")
                        {
                            // MessageBox.Show("Closing " + item.MainWindowTitle);
                            item.CloseMainWindow();
                            int nb_wait = 0;
                            while (!item.HasExited && nb_wait < 10)
                            {
                                Application.DoEvents();
                                Thread.Sleep(100);
                                nb_wait++;
                            }
                            if (!item.HasExited)
                            {
                                // MessageBox.Show("Killing " + item.MainWindowTitle);
                                item.Kill();
                            }

                            nb_process++;
                        }
                    }
                }
            }
        }
    }
}
