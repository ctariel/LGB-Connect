using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data;
using MySql.Data.MySqlClient;
using LGBConnect.classes;

namespace LGBConnect
{
    /// <summary>
    /// Affiche d'un splashscreen pour la forme.
    /// En réalité, la connexion est si rapide qu'on a à peine le temps de voir le splash.
    /// 
    /// Néanmoins, en cas d'accès lent à la base de données, ça devrait être utile.
    /// </summary>
    public partial class frm_Splash : Form
    {

        public frm_Splash()
        {
            InitializeComponent();
        }

        public int chargement()
        {
            this.Show();
            progressBar_Splash.Step = 33;
            int retour = 1; // echec par défaut

            if (verification_Service() == 0)
            {
                progressBar_Splash.PerformStep();

                if (verification_Config() == 0)
                {
                    if (Parametres.Debug == "all")
                    {
                        MainForm.WriteLog("SplashScreen.cs->chargement() : Config ok -----------------");
                        MainForm.WriteLog("mysql - hote :" + Parametres.Db_hote);
                        MainForm.WriteLog("mysql - base :" + Parametres.Db_base);
                        MainForm.WriteLog("mysql - utilisateur :" + Parametres.Db_utilisateur);
                        MainForm.WriteLog("mysql - mot de passe :" + Parametres.Db_motdepasse);
                        MainForm.WriteLog("poste - nom :" + Parametres.Poste_nom);
                        MainForm.WriteLog("poste - id :" + Parametres.Poste_id);
                        MainForm.WriteLog("poste - MAC :" + Parametres.Poste_adresse_MAC);
                        MainForm.WriteLog("poste - type :" + Parametres.Poste_type);
                        MainForm.WriteLog("poste - chrono :" + Parametres.Poste_chrono);
                        MainForm.WriteLog("poste - debug :" + Parametres.Debug);
                        MainForm.WriteLog("SplashScreen.cs->chargement() : Config ok -----------------");

                    }
                    progressBar_Splash.PerformStep();

                    if (verification_Connexion_Base() == 0)
                    {
                        if (Parametres.Debug == "all")
                        {
                            MainForm.WriteLog("SplashScreen.cs-> chargement() : connexion à la base ok ");
                        }
                        progressBar_Splash.PerformStep();
                        retour = 0;
                    }
                    else
                    {
                        MessageBox.Show("Problème dans la configuration : accès base de données impossible !");
                    }
                }
                else
                {
                    MessageBox.Show("Problème : pas de configuration trouvée !");
                }
            }
            else
            {
                MessageBox.Show("Problème : service LGBConnect inaccessible !");
            }
            this.Hide();
            return retour;
        }

        /// <summary>
        /// Vérification de la présence du service LGB
        /// </summary>
        /// <returns>0 en cas de succès, 1 en cas d'échec</returns>
        private int verification_Service()
        {
            try
            {
                ServiceFonctionsAdmin.FonctionsAdminClient client = new ServiceFonctionsAdmin.FonctionsAdminClient();
                if (client != null) { 
                    return 0;
                }
                else
                {
                    return 1;
                }
            }
            catch
            {
                return 1;
            }
        }


        /// <summary>
        /// Vérification de l'existence d'une configuration valide.
        /// </summary>
        /// <returns>0 en cas de succès, 1 en cas d'échec</returns>
        private int verification_Config()
        {
            return Parametres.LireConfiguration();
        }

        /// <summary>
        /// Test de la connexion à la base
        /// </summary>
        /// <returns>0 en cas de succès, 1 en cas d'échec</returns>
        private int verification_Connexion_Base()
        {
            MySqlConnection cnn;
            //System.Diagnostics.Debug.WriteLine("connection string = " + parentForm.connectionString);
            try
            {
                cnn = new MySqlConnection(Parametres.ConnectionString);
                cnn.Open();
                // MessageBox.Show("connexion réussie !! ");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Connexion echouée !! " + ex.ToString());
                return 1;
            }
            cnn.Close();
            return 0;
        }
    }
}
