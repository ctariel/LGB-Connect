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
using System.Net.NetworkInformation;
using System.Diagnostics;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace LGBConnect
{
    /// <summary>
    /// Montre un assistant de configuration en 2 étapes :
    /// - configuration de l'accès à la base de données de Cybergestionnaire
    /// - configuration du poste
    /// </summary>
    public partial class assistantConfig : Form
    {
        DataSet ds_Postes;
        MainForm parentForm;

        public assistantConfig(MainForm mainForm)
        {
            InitializeComponent();
            parentForm = mainForm;
        }

        /// <summary>
        /// Toutes les fenêtres sont liées à MainForm pour éviter des passages de parametres à tous bouts de champ...
        /// 
        /// Si la configuration existe déjà, on pré rempli les champs.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void assistantConfig_Load(object sender, EventArgs e)
        {
            panel_Page1.Parent = panel_principal;


            Dictionary<string, string> config = new Dictionary<string, string>();

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
                textBox_Hote.Text = parentForm.db_hote;
                textBox_Base.Text = parentForm.db_base;
                textBox_Utilisateur.Text = parentForm.db_utilisateur;
                textBox_MotDePasse.Text = parentForm.db_motdepasse;
            }

        }

        private void btn_Suivant1_Click(object sender, EventArgs e)
        {
            panel_Page1.Parent = tabControl1.GetControl(0);
            panel_Page2.Parent = panel_principal;
            configureListePostes();
            configureAdresseMAC();
            configureTypePoste();
        }

        private void btn_Precedant_Click(object sender, EventArgs e)
        {
            panel_Page2.Parent = tabControl1.GetControl(0);
            panel_Page1.Parent = panel_principal;
        }

        private void btn_MAJ_Click(object sender, EventArgs e)
        {
            configureListePostes();
            configureAdresseMAC();
        }


        /// <summary>
        /// On utilise les paramètres rentrés dans le champs pour tester la connection Mysql.
        /// Si c'est bon, on autorise le passage à l'étape suivante
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_TestConnexion_Click(object sender, EventArgs e)
        {
            parentForm.db_hote = textBox_Hote.Text;
            parentForm.db_base = textBox_Base.Text;
            parentForm.db_utilisateur = textBox_Utilisateur.Text;
            parentForm.db_motdepasse = textBox_MotDePasse.Text;

            parentForm.connectionString = "server=" + parentForm.db_hote + ";database=" + parentForm.db_base + ";uid=" + parentForm.db_utilisateur + ";pwd=" + parentForm.db_motdepasse + ";"; ;
            MySqlConnection cnn;

            cnn = new MySqlConnection(parentForm.connectionString);
            try
            {
                cnn.Open();
                lbl_resultatConnexion.Text = "Connexion réussie !!";
                lbl_resultatConnexion.ForeColor = Color.Green;
                btn_Suivant1.Enabled = true;
            }
            catch (Exception ex)
            {
                lbl_resultatConnexion.Text = "Connexion echouée !!" + ex.ToString();
                lbl_resultatConnexion.ForeColor = Color.Red;
                btn_Suivant1.Enabled = false;
            }
            cnn.Close();
        }

        private void btn_Terminer_Click(object sender, EventArgs e)
        {
            ecrireConfiguration();
            this.Close();
        }

        private void comboBox_Poste_SelectedIndexChanged(object sender, EventArgs e)
        {
            configureLabelEspace();
        }

        private void comboBox_MAC_SelectedIndexChanged(object sender, EventArgs e)
        {
            configureLabelIP();
        }

        private void textBox_Hote_TextChanged(object sender, EventArgs e)
        {
            btn_Suivant1.Enabled = false;
        }

        private void textBox_Base_TextChanged(object sender, EventArgs e)
        {
            btn_Suivant1.Enabled = false;
        }

        private void textBox_Utilisateur_TextChanged(object sender, EventArgs e)
        {
            btn_Suivant1.Enabled = false;
        }

        private void textBox_MotDePasse_TextChanged(object sender, EventArgs e)
        {
            btn_Suivant1.Enabled = false;
        }

        private void radioButton_PosteAnimateur_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_PosteAnimateur.Checked || radioButton_PosteUsager.Checked)
                btn_Terminer.Enabled = true;
            else
                btn_Terminer.Enabled = false;
        }

        private void radioButton_PosteUsager_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_PosteAnimateur.Checked || radioButton_PosteUsager.Checked)
                btn_Terminer.Enabled = true;
            else
                btn_Terminer.Enabled = false;

        }

        /// <summary>
        /// On récupére la liste des postes configurés dans CyberGestionnaire,
        /// mais qui n'ont pas encore été affectés à LGBConnect ou EPN Connect
        /// </summary>
        private void configureListePostes()
        {
            string sql = string.Empty;

            MySqlConnection cnn = new MySqlConnection(parentForm.connectionString);
            try
            {
                if (parentForm.nom_poste == "") { 
                    sql = "SELECT  `id_computer`,`nom_computer` FROM `tab_computer` WHERE `configurer_epnconnect_computer`!='1'";
                }
                else
                {
                    sql = "SELECT `id_computer`,`nom_computer` FROM `tab_computer` WHERE `configurer_epnconnect_computer`!='1' or nom_computer='" + parentForm.nom_poste + "'";

                }
                MySqlDataAdapter da_Postes = new MySqlDataAdapter(sql, cnn);
                MySqlCommandBuilder cb = new MySqlCommandBuilder(da_Postes);

                ds_Postes = new DataSet();
                da_Postes.Fill(ds_Postes);

                comboBox_Poste.DisplayMember = "nom_computer";
                comboBox_Poste.ValueMember = "id_computer";
                //                comboBox_Poste.ValueMember = "id_computer";

                comboBox_Poste.DataSource = ds_Postes.Tables[0];

                if (parentForm.nom_poste != "")
                {
                    comboBox_Poste.SelectedIndex = comboBox_Poste.FindStringExact(parentForm.nom_poste);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Connexion echouée !!" + ex.ToString());
            }

        }

        private void configureLabelEspace()
        {
            MySqlConnection cnn = new MySqlConnection(parentForm.connectionString);
            try
            {
                cnn.Open();

                string sql = "SELECT `nom_salle` FROM `tab_computer`, `tab_salle` WHERE tab_computer.id_salle = tab_salle.id_salle AND `nom_computer`='" + comboBox_Poste.Text + "'";

                MySqlCommand cmd = new MySqlCommand(sql, cnn);
                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    lbl_Espace.Text = (String)rdr["nom_salle"];
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Connexion echouée !!" + ex.ToString());
            }
            cnn.Close();
        }
        /// <summary>
        /// On récupére les adresses MAC des cartes réseau de Windows
        /// et on sélectionne par défaut la plus rapide
        /// </summary>
        private void configureAdresseMAC()
        {
            long maxSpeed = -1;
            int i = 0;
            comboBox_MAC.Items.Clear();

            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {

                if (nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet && nic.OperationalStatus == OperationalStatus.Up)
                {
                    ComboboxItem item = new ComboboxItem();
                    if (nic.GetPhysicalAddress().ToString().Length == 12) {
                        // découpage propre pour affichage
                        item.Text = nic.GetPhysicalAddress().ToString().Substring(0, 2) + "-"
                                  + nic.GetPhysicalAddress().ToString().Substring(2, 2) + "-"
                                  + nic.GetPhysicalAddress().ToString().Substring(4, 2) + "-"
                                  + nic.GetPhysicalAddress().ToString().Substring(6, 2) + "-"
                                  + nic.GetPhysicalAddress().ToString().Substring(8, 2) + "-"
                                  + nic.GetPhysicalAddress().ToString().Substring(10, 2);
                    }
                    else
                    {
                        item.Text = nic.GetPhysicalAddress().ToString();
                    }
                    item.Value = nic.GetPhysicalAddress().ToString();

                    comboBox_MAC.Items.Add(item);
                    if (nic.Speed > maxSpeed)
                    {
                        maxSpeed = nic.Speed;
                        comboBox_MAC.SelectedIndex = i;
                    }
                }
            }

        }

        /// <summary>
        /// On récupére l'adresse IP de l'adresse MAC sélectionnée
        /// </summary>
        private void configureLabelIP()
        {
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet && nic.OperationalStatus == OperationalStatus.Up)
                {
                    if (nic.GetPhysicalAddress().ToString() == (comboBox_MAC.SelectedItem as ComboboxItem).Value.ToString())
                    {
                        foreach (UnicastIPAddressInformation ip in nic.GetIPProperties().UnicastAddresses)
                        {
                            if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                            {
                                lbl_IP.Text = ip.Address.ToString();
                            }
                        }
                    }
                }
            }

        }

        private void configureTypePoste()
        {
            if (parentForm.type_poste == "animateur")
            {
                radioButton_PosteAnimateur.Checked = true;
            }
            else
            {
                radioButton_PosteUsager.Checked = true;
            }
        }

        /// <summary>
        /// On écrit la configuration à plusieurs endroits :
        /// - dans un fichier INI. C'est le service LGBServ qui s'occupe de ça pour éviter les problèmes de droits
        /// - dans la base Mysql de CyberGestionnaire
        /// </summary>

        private void ecrireConfiguration()
        {
            // mise à jour des variables principales
            String usage = "";

            parentForm.nom_poste = comboBox_Poste.Text;
            parentForm.id_poste = comboBox_Poste.SelectedValue.ToString();

            if (radioButton_PosteAnimateur.Checked)
            {
                parentForm.type_poste = "animateur";
                usage = "2";
            }
            else
            {
                if (radioButton_PosteUsager.Checked)
                {
                    parentForm.type_poste = "usager";
                    usage = "1";
                }
            }

            try {
                // ecriture du fichier de configuration

                Dictionary<string, string> config = new Dictionary<string, string>();

                config["mysql_hote"] = parentForm.db_hote;
                config["mysql_base"] = parentForm.db_base;
                config["mysql_utilisateur"] = parentForm.db_utilisateur;
                config["mysql_mot_de_passe"] = parentForm.db_motdepasse;
                config["poste_nom"] = parentForm.nom_poste;
                config["poste_id"] = parentForm.id_poste;
                config["poste_adresse_MAC"] = (comboBox_MAC.SelectedItem as ComboboxItem).Value.ToString();
                config["poste_type"] = parentForm.type_poste;
                //Fonction.Base64Encode("");

                ServiceFonctionsAdmin.FonctionsAdminClient client = new ServiceFonctionsAdmin.FonctionsAdminClient();
                client.ecrireConfiguration(config);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur d'écriture du fichier Ini : " + ex.ToString());
            }
            // mise à jour de la base de donnnées

            MySqlConnection cnn = new MySqlConnection(parentForm.connectionString);
            try
            {
                cnn.Open();

                string sql = "UPDATE tab_computer " +
                    "SET usage_computer='" + usage + "', " +
                    "adresse_mac_computer='" + (comboBox_MAC.SelectedItem as ComboboxItem).Text.ToString() + "', " +
                    "adresse_ip_computer='" + lbl_IP.Text + "', " +
                    "nom_hote_computer='" + parentForm.nom_poste + "', " +
                    "configurer_epnconnect_computer='1'" +
                    " WHERE nom_computer='" + parentForm.nom_poste + "'";
                Debug.WriteLine("SQL : " + sql);

                MySqlCommand cmd = new MySqlCommand(sql, cnn);
                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Connexion echouée !!" + ex.ToString());
            }
            cnn.Close();
        }

    }

}
