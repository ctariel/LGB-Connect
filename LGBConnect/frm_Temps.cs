using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using MySql.Data;
using MySql.Data.MySqlClient;
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
        Utilisateur utilisateur;
        ConfigLogiciel configLogiciel;
        Resa resa;
        Resa prochaineResa;

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


            // vérification de la présence d'une résa sur le poste
            int idResa = Resa.prochaineResa(Parametres.poste_id);
            if (idResa != 0) // il existe une résa future pour le poste !
            {
                prochaineResa = new Resa(idResa);

                //on calcule le début de la prochaine réservation, on et regarde dans combien de temps ca commence
                DateTime debutDeSession = prochaineResa.dateResa.AddMinutes(prochaineResa.debut);
                TimeSpan diff = DateTime.Now - debutDeSession;

                if (diff.TotalMinutes > -5 && diff.TotalMinutes < prochaineResa.duree)
                {
                    // le seul qui peut se connecter dans cet interval de temps est le propriétaire de la résa
                    // (voir MainForm.btn_Connexion_Click() )
                    // c'est donc la résa qui commence !
                    prochaineResa.activer();
                    resa = prochaineResa;
                }
                else
                {
                    // inscription de la réservation dans la base
                    resa = new Resa(utilisateur.id, utilisateur.tempsRestant(), heureConnexion);


                    // on vérifie que la prochaine résa ne commence pas avant la fin théorique de la session
                    DateTime debutDeReservation = prochaineResa.dateResa.AddMinutes(prochaineResa.debut);
                    DateTime finDeSessionPrevue = resa.dateResa.AddMinutes(resa.debut).AddMinutes(resa.duree);

                    if (finDeSessionPrevue > debutDeReservation)
                    {

                        if (prochaineResa.idUtilisateur == utilisateur.id)
                        {
                            // cas ou l'utilisateur ayant réservé arrive plus tôt...
                            DialogResult dresult = MessageBox.Show("Vous avez réservé ce poste pour le " + debutDeReservation.ToString("G") + ". Voulez-vous utiliser cette réservation maintenant ?", "Attention !", MessageBoxButtons.YesNo);
                            if ( dresult == DialogResult.Yes)
                            {
                                prochaineResa.annuler();
                            }
                        }
                        else
                        {
                            diff = debutDeReservation.AddMinutes(-1) - heureConnexion;
                            MessageBox.Show("Ce poste a été reservé pour le " + debutDeReservation.ToString("G") + ". La session sur ce poste se terminera donc dans " + Math.Floor(diff.TotalMinutes) + " mn.", "Poste réservé prochainement !");
                        }
                    }
                }
            }
            else
            {
                // pas de resa, inscription d'une nouvelle réservation dans la base
                resa = new Resa(utilisateur.id, utilisateur.tempsRestant(), heureConnexion);
            }

        }

        void timer_Tick(object sender, EventArgs e)
        {

            String affichage_restant, affichage_utilise;
            System.TimeSpan diffRestant, diffUtilise;

            DateTime heureDeconnexion;

            heureDeconnexion = heureConnexion.AddMinutes(utilisateur.tempsRestant());

            if (prochaineResa == null || prochaineResa.id == 0 || prochaineResa.duree == 0) // pas de réservation active
            {
            }
            else
            {
                DateTime debutDeSession = prochaineResa.dateResa.AddMinutes(prochaineResa.debut);
                TimeSpan diff = DateTime.Now - debutDeSession;

                if (diff.TotalMinutes > -5 && diff.TotalMinutes < prochaineResa.duree) // on verrouille 5 minutes avant
                {
                    // c'est la résa qui commence !
                }
                else
                {
                    DateTime debutDeReservation = prochaineResa.dateResa.AddMinutes(prochaineResa.debut);
                    DateTime finDeSessionPrevue = resa.dateResa.AddMinutes(resa.debut).AddMinutes(resa.duree);
                    if (finDeSessionPrevue > debutDeReservation)
                    {
                        heureDeconnexion = debutDeReservation.AddMinutes(-1);
                    }
                }
            }

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

            verificationStatutResa();
        }

        private void verificationStatutResa()
        {
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
