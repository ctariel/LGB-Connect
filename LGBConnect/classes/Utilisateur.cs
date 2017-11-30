using System;
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

        public int Id
        {
            get { return _id; }
        }
        public String Nom
        {
            get { return _nom; }
        }
        public String Prenom
        {
            get { return _prenom; }
        }
        public String Login
        {
            get { return _login; }
        }
        public int Statut
        {
            get { return _statut; }
        }

        public Utilisateur(int idUtilisateur)
        {
            MySqlConnection cnn = new MySqlConnection(Parametres.ConnectionString);
            try
            {
                cnn.Open();

                string sql = "SELECT * FROM tab_user WHERE id_user = @idUser";
                MySqlCommand cmd = new MySqlCommand(sql, cnn);
                cmd.Parameters.AddWithValue("@idUser", idUtilisateur);

                if (Parametres.Debug == "all")
                {
                    MainForm.WriteLog("Utilisateur.cs->Utilisateur(id) : requete sql -------------------");

                    string query = cmd.CommandText;
                    foreach (MySqlParameter p in cmd.Parameters)
                    {
                        query = query.Replace(p.ParameterName, p.Value.ToString());
                    }
                    MainForm.WriteLog(query);
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
                MainForm.WriteLog("Utilisateur.cs->Utilisateur(id) : Connexion echouée !!" + ex.ToString());
            }
            cnn.Close();
        }

        public Utilisateur(String login, String motDePasse)
        {
            MySqlConnection cnn = new MySqlConnection(Parametres.ConnectionString);
            try
            {
                cnn.Open();

                string sql = "SELECT * FROM `tab_user` WHERE `login_user`= @login_user AND `pass_user`= @pass_user LIMIT 0,1";
                MySqlCommand cmd = new MySqlCommand(sql, cnn);
                cmd.Parameters.AddWithValue("@login_user", login);
                cmd.Parameters.AddWithValue("@pass_user", Fonction.MD5Hash(motDePasse));

                if (Parametres.Debug == "all" || Parametres.Debug == "sql")
                {
                    MainForm.WriteLog("Utilisateur.cs->Utilisateur(login,motdepasse) : requete sql -------------------");

                    string query = cmd.CommandText;
                    foreach (MySqlParameter p in cmd.Parameters)
                    {
                        query = query.Replace(p.ParameterName, p.Value.ToString());
                    }
                    MainForm.WriteLog(query);
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
                MainForm.WriteLog("Utilisateur.cs->Utilisateur(login,motdepasse) : Connexion echouée !!" + ex.ToString());
            }
            cnn.Close();
        }

        public void MajDerniereVisite()
        {
            MySqlConnection cnn = new MySqlConnection(Parametres.ConnectionString);
            try
            {
                cnn.Open();
                String sql = "UPDATE tab_user SET `lastvisit_user`= @derniereVisite WHERE `id_user` = @idUtilisateur";
                MySqlCommand cmd = new MySqlCommand(sql, cnn);
                cmd.Parameters.AddWithValue("@derniereVisite", DateTime.Now);
                cmd.Parameters.AddWithValue("@idUtilisateur", _id);
                if (Parametres.Debug == "all" || Parametres.Debug == "sql")
                {
                    MainForm.WriteLog("Utilisateur.cs->majDerniereVisite() : requete sql -------------------");

                    string query = cmd.CommandText;
                    foreach (MySqlParameter p in cmd.Parameters)
                    {
                        query = query.Replace(p.ParameterName, p.Value.ToString());
                    }
                    MainForm.WriteLog(query);
                }
                cmd.ExecuteNonQuery();
                _derniereVisite = DateTime.Now;
            }
            catch (Exception ex)
            {
                MainForm.WriteLog("Utilisateur.cs->majDerniereVisite() : Connexion echouée !!" + ex.ToString());
            }
            cnn.Close();
        }

        public Boolean AUnForfaitValide()
        {
            Boolean aUnForfaitValide = false;

            Forfait forfait = new Forfait(this);
            Transaction transaction = new Transaction(this);
            DateTime dateDeFin = DateTime.Now;

            if (forfait.Id != 0 && transaction.Id != 0)
            {
                // ok, on a une transaction et un forfait attaché

                // point de départ de validité = transaction.date
                // duree de validité = forfait.dureeValidite * forfait.uniteValidite

                if (transaction.Date < DateTime.Now)
                {
                    // ok, la date de début est passée
                    if (forfait.DureeValidite != 0)
                    {
                        switch (forfait.UniteValidite)
                        {
                            case 1: // unite = jour
                                dateDeFin = transaction.Date.AddDays(forfait.DureeValidite);
                                break;

                            case 2: // unite = semaine
                                dateDeFin = transaction.Date.AddDays(forfait.DureeValidite * 7);
                                break;
                            case 3: // unite = mois
                                dateDeFin = transaction.Date.AddMonths(forfait.DureeValidite);
                                break;
                            case 4: // unite = illimité !
                                dateDeFin = DateTime.MaxValue;
                                break;
                        }
                    }
                    else
                    {
                        dateDeFin = DateTime.MaxValue;
                    }

                    if (dateDeFin > DateTime.Now)
                    {
                        // ok, la date de fin n'est pas dépassée
                        aUnForfaitValide = true;
                    }
                }
            }
            return aUnForfaitValide;
        }

        public int TempsRestant()
        {
            int temps_restant = 0;
            int frequence = 0;

            if (_statut == 1)
            {  //usager standard
                MySqlConnection cnn = new MySqlConnection(Parametres.ConnectionString);
                try
                {
                    cnn.Open();
                    String sql = "SELECT tab_forfait.nombre_temps_affectation, tab_forfait.unite_temps_affectation, tab_forfait.frequence_temps_affectation FROM tab_user, tab_transactions, tab_forfait WHERE tab_user.id_user = tab_transactions.id_user AND tab_transactions.type_transac = 'temps' AND tab_transactions.id_tarif = tab_forfait.id_forfait AND tab_user.id_user = @idUser";
                    MySqlCommand cmd = new MySqlCommand(sql, cnn);
                    cmd.Parameters.AddWithValue("@idUser", _id);

                    if (Parametres.Debug == "all" || Parametres.Debug == "sql")
                    {
                        MainForm.WriteLog("Utilisateur.cs->tempsRestant() : requete sql -------------------");

                        string query = cmd.CommandText;
                        foreach (MySqlParameter p in cmd.Parameters)
                        {
                            query = query.Replace(p.ParameterName, p.Value.ToString());
                        }
                        MainForm.WriteLog(query);
                    }


                    MySqlDataReader rdr = cmd.ExecuteReader();
                    if (rdr.HasRows)
                    {
                        while (rdr.Read())
                        {
                            int temps = rdr.GetInt32("nombre_temps_affectation");
                            int unite = rdr.GetInt32("unite_temps_affectation");
                            frequence = rdr.GetInt32("frequence_temps_affectation");
                            if (unite == 1) // configuré en minutes
                            {
                                temps_restant = temps;
                            }
                            if (unite == 2) // configuré en heures
                            {
                                temps_restant = temps * 60;
                            }
                        }
                        rdr.Close();
                    }

                    if (temps_restant > 0) 
                    {
                        // ok, on a du temps configuré

                        if (frequence == 1) {
                            // duree par jour
                            sql = "SELECT sum(duree_resa) as dureeConsommee FROM `tab_resa` WHERE id_user_resa = @idUser AND dateresa_resa = CURRENT_DATE() AND status_resa = '1' AND (debut_resa +duree_resa ) <= floor( TIME_TO_SEC( CURRENT_TIME() ) /60) ";
                        }
                        if (frequence == 2) {
                            // duree par semaine
                            sql = "SELECT sum(duree_resa) as dureeConsommee FROM `tab_resa` WHERE id_user_resa = @idUser AND `dateresa_resa`>= (CURRENT_DATE() - INTERVAL 7 DAY) AND status_resa = '1' AND (debut_resa + duree_resa) <= floor(TIME_TO_SEC(CURRENT_TIME()) / 60)";
                        }
                        if (frequence == 3)
                        {
                            // duree par mois
                            sql = "SELECT sum(duree_resa) as dureeConsommee FROM `tab_resa` WHERE id_user_resa = @idUser AND `dateresa_resa`>= (CURRENT_DATE() - INTERVAL 1 MONTH) AND status_resa = '1' AND (debut_resa + duree_resa) <= floor(TIME_TO_SEC(CURRENT_TIME()) / 60)";
                        }
                        cmd = new MySqlCommand(sql, cnn);
                        cmd.Parameters.AddWithValue("@idUser", _id);

                        if (Parametres.Debug == "all" || Parametres.Debug == "sql")
                        {
                            MainForm.WriteLog("Utilisateur.cs->tempsRestant() : requete sql -------------------");

                            string query = cmd.CommandText;
                            foreach (MySqlParameter p in cmd.Parameters)
                            {
                                query = query.Replace(p.ParameterName, p.Value.ToString());
                            }
                            MainForm.WriteLog(query);
                        }

                        rdr = cmd.ExecuteReader();
                        while (rdr.Read())
                        {
                            if (!Convert.IsDBNull(rdr["dureeConsommee"]))
                            {
                                int dureeConsommee = rdr.GetInt32("dureeConsommee");
                                temps_restant = temps_restant - dureeConsommee;
                            }
                        }

                        rdr.Close();

                    }
                }
                catch (Exception ex)
                {
                    MainForm.WriteLog("Utilisateur.cs->tempsRestant() : Connexion echouée !! " + ex.ToString());
                }
                cnn.Close();
            }
            else if (_statut == 3 || _statut == 4)
            {
                temps_restant = 1440; // on donne 24h aux animateurs
            }
            return temps_restant;

        }

        public Boolean EstConnecte(int idPoste)
        {
            Boolean estConnecte = false;
            MySqlConnection cnn = new MySqlConnection(Parametres.ConnectionString);
            try
            {
                cnn.Open();
                String sql = "SELECT * FROM tab_resa WHERE id_user_resa = @idUser AND status_resa = '0' AND id_computer_resa != " + idPoste;
                MySqlCommand cmd = new MySqlCommand(sql, cnn);
                cmd.Parameters.AddWithValue("@idUser", _id);
                if (Parametres.Debug == "all" || Parametres.Debug == "sql")
                {
                    MainForm.WriteLog("Utilisateur.cs->tempsRestant() : requete sql -------------------");

                    string query = cmd.CommandText;
                    foreach (MySqlParameter p in cmd.Parameters)
                    {
                        query = query.Replace(p.ParameterName, p.Value.ToString());
                    }
                    MainForm.WriteLog(query);
                }

                MySqlDataReader rdr = cmd.ExecuteReader();
                estConnecte = rdr.HasRows;
            }
            catch (Exception ex)
            {
                MainForm.WriteLog("Utilisateur.cs->estConnecte() : Connexion echouée !! " + ex.ToString());
            }
            cnn.Close();

            return estConnecte;
        }

    }
}
