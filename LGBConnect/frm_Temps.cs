﻿using System;
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
        MainForm parentForm;

        frm_MsgBox msgBox_deconnexion;

        int temps_restant = 0;
        int deconnexion_auto = 0;

        public frm_Temps(MainForm mainForm)
        {
            InitializeComponent();
            parentForm = mainForm;
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
            lbl_utilisateur.Text = parentForm.prenom_utilisateur + " " + parentForm.nom_utilisateur;
            //this.ShowInTaskbar = false;

            // début de la session
            heureConnexion = DateTime.Now;
            if (Parametres.debug == "all")
            {
                MainForm.writeLog("frm_Temps.cs->frm_Temps_Load : début de session à " + heureConnexion.ToString());
            }

            t.Interval = 1000; // specify interval time as you want
            t.Tick += new EventHandler(timer_Tick);
            t.Start();

            if (parentForm.statut_utilisateur == 1)
            {//utilisateur
                temps_restant = Fonction.get_temps_restant(parentForm.id_utilisateur, Parametres.connectionString);
                if (Parametres.debug == "all")
                {
                    MainForm.writeLog("frm_Temps.cs->frm_Temps_Load : temps restant = " + temps_restant);
                }
            }
            else // admin ou animateur
                temps_restant = 1440; //arbitraire --> 24h00

            // inscription de la réservation dans la base
            MySqlConnection cnn = new MySqlConnection(Parametres.connectionString);
            try
            {
                cnn.Open();

                String sql = "SELECT deconnexion_auto_logiciel FROM tab_config_logiciel";
                MySqlCommand cmd = new MySqlCommand(sql, cnn);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    if (!Convert.IsDBNull(rdr["deconnexion_auto_logiciel"]))
                    {
                        deconnexion_auto = System.Convert.ToInt32(rdr["deconnexion_auto_logiciel"]);
                        if (Parametres.debug == "all")
                        {
                            MainForm.writeLog("frm_Temps.cs->frm_Temps_Load : deconnexion_auto =  " + deconnexion_auto);
                        }

                    }
                }

                rdr.Close();

                sql = "INSERT INTO `tab_resa` (`id_computer_resa`, `id_user_resa`, `dateresa_resa`, `debut_resa`, `duree_resa`, `date_resa`, `status_resa`) VALUES ('" + Parametres.poste_id + "',   '" + parentForm.id_utilisateur + "', '" + heureConnexion.ToString("yyyy-MM-dd") + "', '" + (heureConnexion.Minute + heureConnexion.Hour * 60).ToString() + "', " + temps_restant.ToString() +", '" + heureConnexion.ToString("yyyy-MM-dd") + "','0')";
                if (Parametres.debug == "all")
                {
                    MainForm.writeLog("frm_Temps.cs->frm_Temps_Load : inscription du début de la résa sql =  " + sql);
                }
                
                cmd = new MySqlCommand(sql, cnn);
                rdr = cmd.ExecuteReader();
                parentForm.id_resa = cmd.LastInsertedId;
                if (Parametres.debug == "all")
                {
                    MainForm.writeLog("frm_Temps.cs->frm_Temps_Load : id resa en cours =  " + parentForm.id_resa);
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Connexion echouée !! " + ex.ToString());
            }
            cnn.Close();

        }

        void timer_Tick(object sender, EventArgs e)
        {

            String affichage_restant, affichage_utilise;

            DateTime heureDeconnexion = heureConnexion.AddMinutes(this.temps_restant);

            System.TimeSpan diff = heureDeconnexion - DateTime.Now;
            System.TimeSpan diff1 = DateTime.Now - heureConnexion;


            if (this.temps_restant == 1440)  // le temps affecté est de 24h00, donc infini dans la pratique
            {
                affichage_restant = "";
                lbl_text_restant.Text = "";
            }
            else
            {
                if (diff.Ticks >= DateTime.MinValue.Ticks)
                {
                    affichage_restant = new DateTime(diff.Ticks).ToString("HH:mm:ss");
                }
                else
                {
                    affichage_restant = "00:00:00";
                }
            }

            affichage_utilise = new DateTime(diff1.Ticks).ToString("HH:mm:ss");

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


            /*if (Parametres.debug == "all")
            {
                MainForm.writeLog("frm_Temps.cs->timer_Tick : " + lbl_temps_restant.Text);
            }*/

            /// vérification du statut. Si le poste a été libéré depuis la console, status_resa est différent de zéro
            MySqlConnection cnn = new MySqlConnection(Parametres.connectionString);
            try
            {
                cnn.Open();
                String sql = "SELECT `status_resa` FROM `tab_resa` WHERE `id_user_resa`= '"+ parentForm.id_utilisateur + "' AND `id_resa`= '" + parentForm.id_resa +"'";

                MySqlCommand cmd = new MySqlCommand(sql, cnn);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
     
                    int statut = 0;

                    if (!Convert.IsDBNull(rdr["status_resa"]))
                    {
                        statut = System.Convert.ToInt32(rdr["status_resa"]);
                    }
                    if (statut != 0)
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

                rdr.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Connexion echouée !! " + ex.ToString());
            }
            cnn.Close();


            // si la déconnexion automatique est activée
            if (deconnexion_auto == 1)
            {
                if ((Math.Floor(diff.TotalMinutes)) == 2 && diff.Seconds == 0)
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
                if ((Math.Floor(diff.TotalMinutes)) < 0)
                {

                    if (Parametres.debug == "all")
                    {
                        MainForm.writeLog("frm_Temps.cs->timer_Tick : message de déconnexion");
                    }

                    if (msgBox_deconnexion.IsDisposed)
                    {
                        msgBox_deconnexion = new frm_MsgBox();
                    }
                    msgBox_deconnexion.Show("Déconnexion automatique !! Au revoir !", "Avertissement", 10000);

                }
                if (diff.TotalMinutes < -0.16) // environ 10 secondes
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
        }

        private void frm_Temps_FormClosed(object sender, FormClosedEventArgs e)
        {

            if (parentForm.statut_utilisateur == 1) { // utilisateur standard
               //tuerLesProcess();
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

            MySqlConnection cnn = new MySqlConnection(Parametres.connectionString);
            try
            {
                cnn.Open();
                int statut_resa = 0;
                if (parentForm.statut_utilisateur == 1)
                    statut_resa = 1;
                else
                    statut_resa = 2;

                String sql = "UPDATE tab_resa SET `duree_resa`='" + temp_passe + "', `status_resa`='" + statut_resa.ToString() + "' WHERE `id_resa`='" + parentForm.id_resa + "'";
                //sprintf(chainesql, "UPDATE tab_resa SET `duree_resa`='%d', `status_resa`='%d' WHERE `id_resa`='%d' ", *temps_passer, *status_resa, *id_resa);
                if (Parametres.debug == "all")
                {
                    MainForm.writeLog("frm_Temps.cs->frm_Temps_FormClosed : inscription de la fin de la résa sql =  " + sql);
                }

                MySqlCommand cmd = new MySqlCommand(sql, cnn);
                MySqlDataReader rdr = cmd.ExecuteReader();
                rdr.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Connexion echouée !! " + ex.ToString());
            }
            cnn.Close();
            t.Stop();
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
