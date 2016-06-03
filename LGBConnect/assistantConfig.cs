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
using LGBConnect.classes;

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

        public assistantConfig()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Si la configuration existe déjà, on pré rempli les champs.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void assistantConfig_Load(object sender, EventArgs e)
        {
            panel_Page1.Parent = panel_principal;

            if (Parametres.lireConfiguration() == 0)
            {
                textBox_Hote.Text = Parametres.db_hote;
                textBox_Base.Text = Parametres.db_base;
                textBox_Utilisateur.Text = Parametres.db_utilisateur;
                textBox_MotDePasse.Text = Parametres.db_motdepasse;
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
            Parametres.db_hote = textBox_Hote.Text;
            Parametres.db_base = textBox_Base.Text;
            Parametres.db_utilisateur = textBox_Utilisateur.Text;
            Parametres.db_motdepasse = textBox_MotDePasse.Text;

            MySqlConnection cnn;

            cnn = new MySqlConnection(Parametres.connectionString);
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

            MySqlConnection cnn = new MySqlConnection(Parametres.connectionString);
            try
            {
                if (Parametres.poste_nom == "") { 
                    sql = "SELECT  `id_computer`,`nom_computer` FROM `tab_computer` WHERE `configurer_epnconnect_computer`!='1'";
                }
                else
                {
                    sql = "SELECT `id_computer`,`nom_computer` FROM `tab_computer` WHERE `configurer_epnconnect_computer`!='1' or nom_computer='" + Parametres.poste_nom + "'";

                }
                MySqlDataAdapter da_Postes = new MySqlDataAdapter(sql, cnn);
                MySqlCommandBuilder cb = new MySqlCommandBuilder(da_Postes);

                ds_Postes = new DataSet();
                da_Postes.Fill(ds_Postes);

                comboBox_Poste.DisplayMember = "nom_computer";
                comboBox_Poste.ValueMember = "id_computer";
                //                comboBox_Poste.ValueMember = "id_computer";

                comboBox_Poste.DataSource = ds_Postes.Tables[0];

                if (Parametres.poste_nom != "")
                {
                    comboBox_Poste.SelectedIndex = comboBox_Poste.FindStringExact(Parametres.poste_nom);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Connexion echouée !!" + ex.ToString());
            }

        }

        private void configureLabelEspace()
        {
            MySqlConnection cnn = new MySqlConnection(Parametres.connectionString);
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

                if (nic.OperationalStatus == OperationalStatus.Up)
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
            if (Parametres.poste_type == "animateur")
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
            String usage = "";

            Parametres.poste_nom = comboBox_Poste.Text;
            Parametres.poste_id = (int)comboBox_Poste.SelectedValue;

            if (radioButton_PosteAnimateur.Checked)
            {
                Parametres.poste_type = "animateur";
                usage = "2";
            }
            else
            {
                if (radioButton_PosteUsager.Checked)
                {
                    Parametres.poste_type = "usager";
                    usage = "1";
                }
            }


            if (comboBox_MAC.Items.Count > 0)
            {
                Parametres.poste_adresse_MAC = (comboBox_MAC.SelectedItem as ComboboxItem).Value.ToString();
            }
            else
            {
                Parametres.poste_adresse_MAC = "";
            }

            Parametres.ecrireConfiguration();

            // mise à jour de la base de donnnées

            MySqlConnection cnn = new MySqlConnection(Parametres.connectionString);
            try
            {
                cnn.Open();

                string sql = "UPDATE tab_computer " +
                    "SET usage_computer='" + usage + "', " +
                    "adresse_mac_computer='" + Parametres.poste_adresse_MAC + "', " +
                    "adresse_ip_computer='" + lbl_IP.Text + "', " +
                    "nom_hote_computer='" + Parametres.poste_nom + "', " +
                    "configurer_epnconnect_computer='1'" +
                    " WHERE nom_computer='" + Parametres.poste_nom + "'";
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
