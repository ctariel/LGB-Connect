using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data;
using MySql.Data.MySqlClient;
using LGBConnect.classes;

namespace LGBConnect
{
    public partial class Frm_preinscription : Form
    {
        MainForm parentForm;

        public Frm_preinscription(MainForm mainForm)
        {
            InitializeComponent();
            parentForm = mainForm;
        }

        private void Frm_preinscription_Load(object sender, EventArgs e)
        {
            RemplirComboCodePostal();
            RemplirComboVille();
            RemplirComboPays();
            RemplirComboEPN();
            RemplirComboMetier();

        }

        private void Frm_preinscription_FormClosed(object sender, FormClosedEventArgs e)
        {
            parentForm.TopMost = true;
        }

        private void Btn_annuler_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ComboBox_codePostal_TextChanged(object sender, EventArgs e)
        {
            RemplirComboVille();
        }

        private void ComboBox_ville_SelectedIndexChanged(object sender, EventArgs e)
        {
            RemplirComboPays();
        }

        private void CheckBox_equipement_aucun_CheckedChanged(object sender, EventArgs e)
        {
            checkBox_equipement_ordinateur.Enabled = !checkBox_equipement_aucun.Checked;
            checkBox_equipement_tablette.Enabled = !checkBox_equipement_aucun.Checked;
            checkBox_equipement_smartphone.Enabled = !checkBox_equipement_aucun.Checked;
            checkBox_equipement_television.Enabled = !checkBox_equipement_aucun.Checked;
        }

        private void CheckBox_equipement_ordinateur_CheckedChanged(object sender, EventArgs e)
        {
            Enable_checkBox_equipement_aucun();
        }
        private void CheckBox_equipement_tablette_CheckedChanged(object sender, EventArgs e)
        {
            Enable_checkBox_equipement_aucun();
        }

        private void CheckBox_equipement_smartphone_CheckedChanged(object sender, EventArgs e)
        {
            Enable_checkBox_equipement_aucun();
        }

        private void CheckBox_equipement_television_CheckedChanged(object sender, EventArgs e)
        {
            Enable_checkBox_equipement_aucun();
        }

        private void CheckBox_equipement_internetMaison_CheckedChanged(object sender, EventArgs e)
        {
            Enable_checkBox_equipement_pasdeconnexion();
        }

        private void CheckBox_equipement_internetMobile_CheckedChanged(object sender, EventArgs e)
        {
            Enable_checkBox_equipement_pasdeconnexion();

        }

        private void CheckBox_equipement_pasdeconnexion_CheckedChanged(object sender, EventArgs e)
        {
            checkBox_equipement_internetMobile.Enabled = !checkBox_equipement_pasdeconnexion.Checked;
            checkBox_equipement_internetMaison.Enabled = !checkBox_equipement_pasdeconnexion.Checked;
        }

        private void Enable_checkBox_equipement_aucun()
        {
            if (checkBox_equipement_ordinateur.Checked || checkBox_equipement_smartphone.Checked || checkBox_equipement_tablette.Checked || checkBox_equipement_television.Checked)
            {
                checkBox_equipement_aucun.Enabled = false;
            }
            else
            {
                checkBox_equipement_aucun.Enabled = true;
            }
        }

        private void Enable_checkBox_equipement_pasdeconnexion()
        {
            if (checkBox_equipement_internetMaison.Checked || checkBox_equipement_internetMobile.Checked)
            {
                checkBox_equipement_pasdeconnexion.Enabled = false;
            }
            else
            {
                checkBox_equipement_pasdeconnexion.Enabled = true;
            }
        }

        private void RemplirComboCodePostal()
        {

            MySqlConnection cnn = new MySqlConnection(Parametres.ConnectionString);
            DataSet ds_codesPostaux;
            string sql = string.Empty;

            try
            {

                sql = "SELECT DISTINCT(code_postale_city) FROM `tab_city` WHERE code_postale_city LIKE '" + comboBox_codePostal.Text + "%' ORDER BY code_postale_city";
                MySqlDataAdapter da_codesPostaux = new MySqlDataAdapter(sql, cnn);
                MySqlCommandBuilder cb = new MySqlCommandBuilder(da_codesPostaux);

                ds_codesPostaux = new DataSet();
                da_codesPostaux.Fill(ds_codesPostaux);

                comboBox_codePostal.DisplayMember = "code_postale_city";
                comboBox_codePostal.ValueMember = "";
                //                comboBox_Poste.ValueMember = "id_computer";

                comboBox_codePostal.DataSource = ds_codesPostaux.Tables[0];
                comboBox_codePostal.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
                comboBox_codePostal.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                comboBox_codePostal.AutoCompleteSource = AutoCompleteSource.ListItems;

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Connexion echouée !!" + ex.ToString());
            }
        }

        private void RemplirComboVille()
        {
            MySqlConnection cnn = new MySqlConnection(Parametres.ConnectionString);
            DataSet ds_Villes;
            string sql = string.Empty;

            try
            {
                if (comboBox_codePostal.Text == "")
                {
                    //sql = "SELECT nom_city FROM `tab_city` ORDER BY nom_city";
                    comboBox_ville.DataSource = null;
                    comboBox_ville.Text = "";
                }
                else
                {

                    sql = "SELECT id_city, nom_city FROM `tab_city` WHERE code_postale_city LIKE '" + comboBox_codePostal.Text + "%' ORDER BY nom_city";
                    MySqlDataAdapter da_Villes = new MySqlDataAdapter(sql, cnn);
                    MySqlCommandBuilder cb = new MySqlCommandBuilder(da_Villes);

                    ds_Villes = new DataSet();
                    da_Villes.Fill(ds_Villes);

                    comboBox_ville.DisplayMember = "nom_city";
                    comboBox_ville.ValueMember = "id_city";
                    //                comboBox_Poste.ValueMember = "id_computer";

                    comboBox_ville.DataSource = ds_Villes.Tables[0];
                    if (ds_Villes.Tables[0].Rows.Count == 0)
                    {
                        comboBox_ville.Text = "";
                    }

                }

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Connexion echouée !!" + ex.ToString());
            }
        }
        private void RemplirComboPays()
        {
            MySqlConnection cnn = new MySqlConnection(Parametres.ConnectionString);
            DataSet ds_Pays;
            string sql = string.Empty;

            try
            {
                if (comboBox_codePostal.Text == "" || comboBox_ville.Text == "")
                {
                    //sql = "SELECT nom_city FROM `tab_city` ORDER BY nom_city";
                    comboBox_pays.DataSource = null;
                    comboBox_pays.Text = "";
                }
                else
                {

                    sql = "SELECT DISTINCT(pays_city) FROM `tab_city` WHERE code_postale_city = '" + comboBox_codePostal.Text + "' AND nom_city = '" + comboBox_ville.Text + "'  ORDER BY pays_city";
                    MySqlDataAdapter da_Pays = new MySqlDataAdapter(sql, cnn);
                    MySqlCommandBuilder cb = new MySqlCommandBuilder(da_Pays);

                    ds_Pays = new DataSet();
                    da_Pays.Fill(ds_Pays);

                    comboBox_pays.DisplayMember = "pays_city";
                    comboBox_pays.ValueMember = "";

                    comboBox_pays.DataSource = ds_Pays.Tables[0];
                    if (ds_Pays.Tables[0].Rows.Count == 0)
                    {
                        comboBox_pays.Text = "";
                    }

                }

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Connexion echouée !!" + ex.ToString());
            }
        }

        private void RemplirComboEPN()
        {
            MySqlConnection cnn = new MySqlConnection(Parametres.ConnectionString);
            DataSet ds_EPN;
            string sql = string.Empty;
            
            // récupération de l'id et du nom de la salle
            Salle salle = new Salle(Parametres.Poste_nom);

            try
            {
                sql = "SELECT tab_espace.* FROM tab_espace WHERE tab_espace.id_espace ='" + salle.IdEspace + "'";

                MySqlDataAdapter da_EPN = new MySqlDataAdapter(sql, cnn);
                MySqlCommandBuilder cb = new MySqlCommandBuilder(da_EPN);

                ds_EPN = new DataSet();
                da_EPN.Fill(ds_EPN);

                comboBox_EPN.DisplayMember = "nom_espace";
                comboBox_EPN.ValueMember = "id_espace";

                comboBox_EPN.DataSource = ds_EPN.Tables[0];
                if (ds_EPN.Tables[0].Rows.Count == 0)
                {
                    comboBox_EPN.Text = "";
                }

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Connexion echouée !!" + ex.ToString());
            }

        }

        private void RemplirComboMetier()
        {
            MySqlConnection cnn = new MySqlConnection(Parametres.ConnectionString);
            DataSet ds_Metier;
            string sql = string.Empty;

            try
            {
                sql = "SELECT `id_csp`,`csp` FROM `tab_csp` ORDER BY `csp` ASC";

                MySqlDataAdapter da_Metier = new MySqlDataAdapter(sql, cnn);
                MySqlCommandBuilder cb = new MySqlCommandBuilder(da_Metier);

                ds_Metier = new DataSet();
                da_Metier.Fill(ds_Metier);

                foreach (DataRow row in ds_Metier.Tables[0].Rows) {
                    row[1] = System.Net.WebUtility.HtmlDecode(row[1].ToString());
                }

                comboBox_metier.DisplayMember = "csp";
                comboBox_metier.ValueMember = "id_csp";

                comboBox_metier.DataSource = ds_Metier.Tables[0];
                if (comboBox_metier.Items.Count > 13)
                {
                    comboBox_metier.SelectedIndex = 12;
                }
                if (ds_Metier.Tables[0].Rows.Count == 0)
                {
                    comboBox_metier.Text = "";
                }

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Connexion echouée !!" + ex.ToString());
            }

        }

        private void Btn_envoyer_Click(object sender, EventArgs e)
        {
            bool form_valide = true;
            String sexe;
            lbl_email.ForeColor = Color.Black;
            lbl_nom.ForeColor = Color.Black;
            lbl_prenom.ForeColor = Color.Black;
            lbl_adresse.ForeColor = Color.Black;
            lbl_CPVille.ForeColor = Color.Black;
            lbl_pays.ForeColor = Color.Black;

            if (!Fonction.IsValidEmail(txt_email.Text))
            {
                form_valide = false;
                lbl_email.ForeColor = Color.Red;
            }

            if (txt_nom.Text.Trim() == string.Empty)
            {
                form_valide = false;
                lbl_nom.ForeColor = Color.Red;
            }

            if (txt_prenom.Text.Trim() == string.Empty)
            {
                form_valide = false;
                lbl_prenom.ForeColor = Color.Red;
            }

            if (radioButton_monsieur.Checked)
            {
                sexe = "H";
            }
            else
            {
                sexe = "F";
            }

            if (txt_adresse.Text.Trim() == string.Empty)
            {
                form_valide = false;
                lbl_adresse.ForeColor = Color.Red;
            }

            if (comboBox_codePostal.Text.Trim() == string.Empty || comboBox_ville.Text.Trim() == string.Empty)
            {
                form_valide = false;
                lbl_CPVille.ForeColor = Color.Red;
            }

            if (comboBox_pays.Text.Trim() == string.Empty)
            {
                form_valide = false;
                lbl_pays.ForeColor = Color.Red;
            }

            // divers hacks pour respecter la façon de faire de Cybergestionnaire....
            String ville = "vide";
            String codePostal = "vide";
            String pays = "vide";
            String id_ville = "0";

            if (comboBox_ville.Items.Count > 0)
            {
                if ((comboBox_ville.SelectedItem as DataRowView).Row[0].ToString() != "0")
                {
                    id_ville = (comboBox_ville.SelectedItem as DataRowView).Row[0].ToString();
                }
            }
            else
            {
                codePostal = comboBox_codePostal.Text;
                ville = comboBox_ville.Text;
                pays = comboBox_pays.Text;
            }



            int lieu = 0;
            if (radioButton_lieu_maison.Checked)
            {
                lieu = 1;
            }
            if (radioButton_lieu_bureau.Checked)
            {
                lieu = 2;
            }
            if (radioButton_lieu_maisonbureau.Checked)
            {
                lieu = 3;
            }

            int niveau = 0;
            if (radioButton_niveau_confirme.Checked)
            {
                niveau = 2;
            }
            if (radioButton_niveau_intermediaire.Checked)
            {
                niveau = 1;
            }

            String equipement = string.Empty;

            if (checkBox_equipement_aucun.Checked)
            {
                equipement = "0";
            }
            if (checkBox_equipement_ordinateur.Checked)
            {
                if (equipement != string.Empty)
                {
                    equipement = equipement + "-";
                }
                equipement = equipement + "1";
            }
            if (checkBox_equipement_tablette.Checked)
            {
                if (equipement != string.Empty)
                {
                    equipement = equipement + "-";
                }
                equipement = equipement + "2";
            }
            if (checkBox_equipement_smartphone.Checked)
            {
                if (equipement != string.Empty)
                {
                    equipement = equipement + "-";
                }
                equipement = equipement + "3";
            }
            if (checkBox_equipement_television.Checked)
            {
                if (equipement != string.Empty)
                {
                    equipement = equipement + "-";
                }
                equipement = equipement + "4";
            }
            if (checkBox_equipement_internetMaison.Checked)
            {
                if (equipement != string.Empty)
                {
                    equipement = equipement + "-";
                }
                equipement = equipement + "5";
            }
            if (checkBox_equipement_internetMobile.Checked)
            {
                if (equipement != string.Empty)
                {
                    equipement = equipement + "-";
                }
                equipement = equipement + "6";
            }
            if (checkBox_equipement_pasdeconnexion.Checked)
            {
                if (equipement != string.Empty)
                {
                    equipement = equipement + "-";
                }
                equipement = equipement + "7";
            }



            if (form_valide)
            {
                MySqlConnection cnn = new MySqlConnection(Parametres.ConnectionString);
                cnn.Open();
                try
                {

                    String sql = "INSERT INTO `tab_inscription_user`(" +
                                    "`id_inscription_user`, `date_inscription_user`, `nom_inscription_user`, `prenom_inscription_user`, `sexe_inscription_user`, `jour_naissance_inscription_user`, `mois_naissance_inscription_user`, `annee_naissance_inscription_user`, `adresse_inscription_user`, `quartier_inscription_user`, `code_postal_inscription`, `commune_inscription_autres`, `ville_inscription_user`, `tel_inscription_user`, `tel_port_inscription_user`, `mail_inscription_user`, `temps_inscription_user`, `login_inscription_user`, `pass_inscription_user`, `status_inscription_user`, `lastvisit_inscription_user`, `csp_inscription_user`, `equipement_inscription_user`, `utilisation_inscription_user`, `connaissance_inscription_user`, `info_inscription_user`, `id_inscription_computer`" +
                                    ") VALUES (" +
                                    "'', @date_inscription_user, @nom_inscription_user, @prenom_inscription_user, @sexe_inscription_user, @jour_naissance_inscription_user, @mois_naissance_inscription_user, @annee_naissance_inscription_user, @adresse_inscription_user, @quartier_inscription_user, @code_postal_inscription, @commune_inscription_autres, @ville_inscription_user, @tel_inscription_user, @tel_port_inscription_user, @mail_inscription_user, @temps_inscription_user, @login_inscription_user, @pass_inscription_user, @status_inscription_user, @lastvisit_inscription_user, @csp_inscription_user, @equipement_inscription_user, @utilisation_inscription_user, @connaissance_inscription_user, @info_inscription_user, @id_inscription_computer" +
                                    ")";

                    MySqlCommand cmd = new MySqlCommand(sql, cnn);

                    cmd.Parameters.AddWithValue("@date_inscription_user", DateTime.Now.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@nom_inscription_user", txt_nom.Text);
                    cmd.Parameters.AddWithValue("@prenom_inscription_user", txt_prenom.Text);
                    cmd.Parameters.AddWithValue("@sexe_inscription_user", sexe);
                    cmd.Parameters.AddWithValue("@jour_naissance_inscription_user", DTP_dateNaissance.Value.Day.ToString());
                    cmd.Parameters.AddWithValue("@mois_naissance_inscription_user", DTP_dateNaissance.Value.Month.ToString());
                    cmd.Parameters.AddWithValue("@annee_naissance_inscription_user", DTP_dateNaissance.Value.Year.ToString());
                    cmd.Parameters.AddWithValue("@adresse_inscription_user", txt_adresse.Text);
                    cmd.Parameters.AddWithValue("@quartier_inscription_user",pays);
                    cmd.Parameters.AddWithValue("@code_postal_inscription", codePostal);
                    cmd.Parameters.AddWithValue("@commune_inscription_autres", ville);
                    cmd.Parameters.AddWithValue("@ville_inscription_user", id_ville);
                    cmd.Parameters.AddWithValue("@tel_inscription_user",txt_telFixe.Text);
                    cmd.Parameters.AddWithValue("@tel_port_inscription_user",txt_telPortable.Text);
                    cmd.Parameters.AddWithValue("@mail_inscription_user",txt_email.Text);
                    cmd.Parameters.AddWithValue("@temps_inscription_user", "1");
                    cmd.Parameters.AddWithValue("@login_inscription_user", txt_nom.Text);
                    cmd.Parameters.AddWithValue("@pass_inscription_user", txt_prenom.Text);
                    cmd.Parameters.AddWithValue("@status_inscription_user", "2");
                    cmd.Parameters.AddWithValue("@lastvisit_inscription_user", DateTime.Now.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@csp_inscription_user",(comboBox_metier.SelectedItem as DataRowView).Row[0].ToString());
                    cmd.Parameters.AddWithValue("@equipement_inscription_user",equipement);
                    cmd.Parameters.AddWithValue("@utilisation_inscription_user",lieu);
                    cmd.Parameters.AddWithValue("@connaissance_inscription_user",niveau);
                    cmd.Parameters.AddWithValue("@info_inscription_user",txt_message.Text);
                    cmd.Parameters.AddWithValue("@id_inscription_computer", (comboBox_EPN.SelectedItem as DataRowView).Row[0].ToString());

                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Votre pré-inscription est enregistrée. Veuillez vous adresser au responsable de l'espace pour sa validation.");

                    this.Close();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Connexion echouée !!" + ex.ToString());
                    MessageBox.Show("problème d'enregistrement !" + ex.ToString());
                }
                cnn.Close();
            }
        }

    }



}
