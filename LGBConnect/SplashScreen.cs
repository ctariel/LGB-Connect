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
        MainForm parentForm;

        public frm_Splash(MainForm mainForm)
        {
            InitializeComponent();
            parentForm = mainForm;
        }

        public int chargement()
        {
            this.Show();
            progressBar_Splash.Step = 50;

            if (verification_Config() == 0 ) {

                progressBar_Splash.PerformStep();
                if (verification_Connexion_Base() == 0)
                {
                    progressBar_Splash.PerformStep();
                    this.Hide();
                    return 0;
                }
                else
                {
                    MessageBox.Show("Problème dans la configuration : accès base de données impossible !");
                    this.Hide();
                    return 1;
                }
            }
            else
            {
                this.Hide();
                return 1;
            }
        }
        /// <summary>
        /// Vérification de l'existence d'une configuration valide.
        /// </summary>
        /// <returns>0 en cas de succès, 1 en cas d'échec</returns>
        private int verification_Config()
        {
            Dictionary<string, string> config = new Dictionary<string, string>();

            //MessageBox.Show(RunningDir + "\\" + fileConfigName);

            ServiceFonctionsAdmin.FonctionsAdminClient client = new ServiceFonctionsAdmin.FonctionsAdminClient();
            config = client.lireConfiguration();

            if (config != null)
            {
                parentForm.db_hote = config["mysql_hote"];
                parentForm.db_base = config["mysql_base"];
                parentForm.db_utilisateur = config["mysql_utilisateur"];
                parentForm.db_motdepasse = config["mysql_mot_de_passe"];
                parentForm.nom_poste = config["poste_nom"];
                parentForm.id_poste = config["poste_id"];
                parentForm.type_poste = config["poste_type"];
                return 0;
            }
            else
            {
                return 1;
            }

        }

        /// <summary>
        /// Test de la connexion à la base
        /// </summary>
        /// <returns>0 en cas de succès, 1 en cas d'échec</returns>
        private int verification_Connexion_Base()
        {
            parentForm.connectionString = "server=" + parentForm.db_hote + ";database=" + parentForm.db_base + ";uid=" + parentForm.db_utilisateur + ";pwd=" + parentForm.db_motdepasse + ";"; ;
            MySqlConnection cnn;
            System.Diagnostics.Debug.WriteLine("connection string = " + parentForm.connectionString);
            try
            {
                cnn = new MySqlConnection(parentForm.connectionString);
                cnn.Open();
                // MessageBox.Show("connexion réussie !! ");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Connexion echouée !!" + ex.ToString());
                return 1;
            }
            cnn.Close();
            return 0;
        }
    }
}
