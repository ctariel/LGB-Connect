using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.Types;
using MySql.Data.MySqlClient;

namespace LGBConnect.classes
{
    public class Utilisateur
    {
        private int _id = 0;
        private DateTime _dateInscription;
        private String _nom;
        private String _prenom;
        private String _sexe;
        private DateTime _dateNaissance;
        private String _adresse;
        private int _ville;
        private String _telephone;
        private String _mail;
        private int _temps; // ?? quoi qu'est-ce ? A priori, l'id du tarif utilisé
        private String _login;
        private String _motDePasse;
        private int _statut;
        private DateTime _derniereVisite;
        private int _csp; // ?? quoi qu'est-ce ?
        private String _equipement;
        private int _utilisation;
        private int _connaissance;
        private String _info;
        private int _tarif;
        private DateTime _dateRen;  // ?? quoi qu'est-ce ?
        private int _epn;
        private Boolean _newsletter;

        public int id
        {
            get { return _id; }
        }
        public String nom
        {
            get { return _nom; }
        }
        public String prenom
        {
            get { return _prenom; }
        }
        public int statut
        {
            get { return _statut; }
        }

        public Utilisateur(int idUtilisateur)
        {
            MySqlConnection cnn = new MySqlConnection(Parametres.connectionString);
            try
            {
                cnn.Open();

                string sql = "SELECT * FROM tab_user WHERE id_user = @idUser";
                MySqlCommand cmd = new MySqlCommand(sql, cnn);
                cmd.Parameters.AddWithValue("@idUser", idUtilisateur);

                if (Parametres.debug == "all")
                {
                    MainForm.writeLog("Utilisateur.cs->Utilisateur(id) : requete sql -------------------");

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
                        _id = rdr.GetInt32("id_user");
                        _dateInscription = DateTime.Parse(rdr.GetString("date_insc_user"));
                        _nom = rdr.GetString("nom_user");
                        _prenom = rdr.GetString("prenom_user");
                        _sexe = rdr.GetString("sexe_user");
                        _dateNaissance = new DateTime(rdr.GetInt32("annee_naissance_user"), rdr.GetInt32("mois_naissance_user"), rdr.GetInt32("jour_naissance_user"));
                        _adresse = rdr.GetString("adresse_user");
                        _ville = rdr.GetInt32("ville_user");
                        _telephone = rdr.GetString("tel_user");
                        _mail = rdr.GetString("mail_user");
                        _temps = rdr.GetInt32("temps_user");
                        _login = rdr.GetString("login_user");
                        _motDePasse = rdr.GetString("pass_user");
                        _statut = rdr.GetInt32("status_user");
                        _derniereVisite = rdr.GetDateTime("lastvisit_user");
                        _csp = rdr.GetInt32("csp_user");
                        _equipement = rdr.GetString("equipement_user");
                        _utilisation = rdr.GetInt32("utilisation_user");
                        _connaissance = rdr.GetInt32("connaissance_user");
                        _info = rdr.GetString("info_user");
                        _tarif = rdr.GetInt32("tarif_user");
                        _dateRen = rdr.GetDateTime("dateRen_user");
                        _epn = rdr.GetInt32("epn_user");
                        _newsletter = (rdr.GetInt32("newsletter_user") != 0);
                    }

                }
            }
            catch (Exception ex)
            {
                MainForm.writeLog("Utilisateur.cs->Utilisateur(id) : Connexion echouée !!" + ex.ToString());
            }
            cnn.Close();
        }

        public Utilisateur(String login, String motDePasse)
        {
            MySqlConnection cnn = new MySqlConnection(Parametres.connectionString);
            try
            {
                cnn.Open();

                string sql = "SELECT * FROM `tab_user` WHERE `login_user`= @login_user AND `pass_user`= @pass_user LIMIT 0,1";
                MySqlCommand cmd = new MySqlCommand(sql, cnn);
                cmd.Parameters.AddWithValue("@login_user", login);
                cmd.Parameters.AddWithValue("@pass_user", Fonction.MD5Hash(motDePasse));

                if (Parametres.debug == "all")
                {
                    MainForm.writeLog("Utilisateur.cs->Utilisateur(login,motdepasse) : requete sql -------------------");

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
                        _id = rdr.GetInt32("id_user");
                        _dateInscription = DateTime.Parse(rdr.GetString("date_insc_user"));
                        _nom = rdr.GetString("nom_user");
                        _prenom = rdr.GetString("prenom_user");
                        _sexe = rdr.GetString("sexe_user");
                        _dateNaissance = new DateTime(rdr.GetInt32("annee_naissance_user"), rdr.GetInt32("mois_naissance_user"), rdr.GetInt32("jour_naissance_user"));
                        _adresse = rdr.GetString("adresse_user");
                        _ville = rdr.GetInt32("ville_user");
                        _telephone = rdr.GetString("tel_user");
                        _mail = rdr.GetString("mail_user");
                        _temps = rdr.GetInt32("temps_user");
                        _login = rdr.GetString("login_user");
                        _motDePasse = rdr.GetString("pass_user");
                        _statut = rdr.GetInt32("status_user");
                        _derniereVisite = rdr.GetDateTime("lastvisit_user");
                        _csp = rdr.GetInt32("csp_user");
                        _equipement = rdr.GetString("equipement_user");
                        _utilisation = rdr.GetInt32("utilisation_user");
                        _connaissance = rdr.GetInt32("connaissance_user");
                        _info = rdr.GetString("info_user");
                        _tarif = rdr.GetInt32("tarif_user");
                        _dateRen = rdr.GetDateTime("dateRen_user");
                        _epn = rdr.GetInt32("epn_user");
                        _newsletter = (rdr.GetInt32("newsletter_user") != 0);
                    }

                }
            }
            catch (Exception ex)
            {
                MainForm.writeLog("Utilisateur.cs->Utilisateur(login,motdepasse) : Connexion echouée !!" + ex.ToString());
            }
            cnn.Close();
        }

        public void majDerniereVisite()
        {
            MySqlConnection cnn = new MySqlConnection(Parametres.connectionString);
            try
            {
                cnn.Open();
                String sql = "UPDATE tab_user SET `lastvisit_user`= @derniereVisite WHERE `id_user` = @idUtilisateur";
                MySqlCommand cmd = new MySqlCommand(sql, cnn);
                cmd.Parameters.AddWithValue("@derniereVisite", DateTime.Now);
                cmd.Parameters.AddWithValue("@idUtilisateur", _id);
                if (Parametres.debug == "all")
                {
                    MainForm.writeLog("Utilisateur.cs->majDerniereVisite() : requete sql -------------------");

                    string query = cmd.CommandText;
                    foreach (MySqlParameter p in cmd.Parameters)
                    {
                        query = query.Replace(p.ParameterName, p.Value.ToString());
                    }
                    MainForm.writeLog(query);
                }
                cmd.ExecuteNonQuery();
                _derniereVisite = DateTime.Now;
            }
            catch (Exception ex)
            {
                MainForm.writeLog("Utilisateur.cs->majDerniereVisite() : Connexion echouée !!" + ex.ToString());
            }
            cnn.Close();
        }

        public int tempsRestant()
        {
            int temps_restant = 1440; // on donne 24 heures par défaut. Si on a un usager standar, alors on calcule le temps restant
            if (_statut == 1)
            {  //usager standard
                MySqlConnection cnn = new MySqlConnection(Parametres.connectionString);
                try
                {
                    cnn.Open();
                    String sql = "SELECT tab_forfait.nombre_temps_affectation, tab_forfait.unite_temps_affectation FROM tab_user, rel_forfait_user, tab_forfait WHERE tab_user.id_user = rel_forfait_user.id_user AND rel_forfait_user.id_forfait = tab_forfait.id_forfait AND tab_user.id_user = @idUser";
                    MySqlCommand cmd = new MySqlCommand(sql, cnn);
                    cmd.Parameters.AddWithValue("@idUser", _id);
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        int temps = rdr.GetInt32("nombre_temps_affectation");
                        int unite = rdr.GetInt32("unite_temps_affectation");
                        if (unite == 1) // configuré en minutes
                        {
                            temps_restant = temps;
                        }
                        if (unite == 2) // configuré en heures
                        {
                            temps_restant = temps * 60;
                        }
                    }
                    if (temps_restant > 0) // si le temps restant = 0, ca veut dire temps infini dans cybergestionnaire !!
                    {
                        rdr.Close();

                        sql = "SELECT sum(duree_resa) as dureedujour FROM `tab_resa` WHERE id_user_resa = @idUser AND date_resa = '" + DateTime.Today.ToString("yyyy-MM-dd") + "'AND status_resa = '1'";
                        cmd = new MySqlCommand(sql, cnn);
                        cmd.Parameters.AddWithValue("@idUser", _id);
                        rdr = cmd.ExecuteReader();
                        while (rdr.Read())
                        {
                            if (!Convert.IsDBNull(rdr["dureedujour"]))
                            {
                                int dureedujour = rdr.GetInt32("dureedujour");
                                temps_restant = temps_restant - dureedujour;
                            }
                        }

                        rdr.Close();

                    }
                    else
                    {
                        temps_restant = 1440; // si le temps n'est pas configuré, on donne 24h00
                    }


                }
                catch (Exception ex)
                {
                    MainForm.writeLog("Utilisateur.cs->tempsRestant() : Connexion echouée !! " + ex.ToString());
                }
                cnn.Close();
            }
            return temps_restant;

        }

        public Boolean estConnecte()
        {
            Boolean estConnecte = false;
            MySqlConnection cnn = new MySqlConnection(Parametres.connectionString);
            try
            {
                cnn.Open();
                String sql = "SELECT * FROM tab_resa WHERE id_user_resa = @idUser AND status_resa = '0'";
                MySqlCommand cmd = new MySqlCommand(sql, cnn);
                cmd.Parameters.AddWithValue("@idUser", _id);
                MySqlDataReader rdr = cmd.ExecuteReader();
                estConnecte = rdr.HasRows;
            }
            catch (Exception ex)
            {
                MainForm.writeLog("Utilisateur.cs->estConnecte() : Connexion echouée !! " + ex.ToString());
            }
            cnn.Close();

            return estConnecte;
        }

    }
}
