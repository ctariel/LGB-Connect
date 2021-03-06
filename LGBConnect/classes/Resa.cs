﻿using System;
using MySql.Data;
using MySql.Data.MySqlClient;
using LGBConnect.classes;

namespace LGBConnect.classes
{
    public class Resa
    {
        private int _id = 0;
        private int _idPoste;
        private int _idUtilisateur;
        private DateTime _dateResa; // date réelle de la réservation
        private int _debut;
        private int _duree;
        private DateTime _date; // date à laquelle a été faite la réservation
        private int _statut;

        private System.Timers.Timer _timer = new System.Timers.Timer();


        public int Id {
            get { return _id;}
        }
        public int IdUtilisateur
        {
            get { return _idUtilisateur; }
        }
        public DateTime DateResa
        {
            get { return _dateResa; }
        }
        public int Debut
        {
            get { return _debut; }
        }
        public int Duree
        {
            get { return _duree; }
            set { _duree = value; }
        }
        public int Statut
        {
            get { return _statut;  }
        }
        public DateTime DebutDeSession
        {
            get
            {
                return _dateResa.AddMinutes(_debut);
            }
        }
        public DateTime FinDeSession
        {
            get
            {
                return _dateResa.AddMinutes(_debut).AddMinutes(_duree);
            }
        }

        

        public Resa(int idResa)
        {
            MySqlConnection cnn = new MySqlConnection(Parametres.ConnectionString);
            try
            {
                // on cherche la salle associé au poste
                string sql = "SELECT * FROM tab_resa WHERE id_resa = @idResa";
                cnn.Open();
                MySqlCommand cmd = new MySqlCommand(sql, cnn);
                cmd.Parameters.AddWithValue("@idResa", idResa);

                MySqlDataReader rdr = cmd.ExecuteReader();

                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        _id = rdr.GetInt32("id_resa");
                        _idPoste = rdr.GetInt32("id_computer_resa");
                        _idUtilisateur = rdr.GetInt32("id_user_resa");
                        _dateResa = rdr.GetDateTime("dateresa_resa");
                        _debut = rdr.GetInt32("debut_resa");
                        _duree = rdr.GetInt32("duree_resa");
                        _date = rdr.GetDateTime("date_resa");
                        _statut = Int32.Parse(rdr.GetString("status_resa"));

                        if (Parametres.Debug == "all")
                        {
                            MainForm.WriteLog("Resa.cs->Resa(idResa) : Resa trouvée : id = " + _id);
                        }
                    }
                }
                else
                {
                    MainForm.WriteLog("Resa.cs->Resa(idResa) : Pas de Réservation trouvée pour l'id " + idResa + " !");
                }
                Timer_Start();
            }
            catch (Exception ex)
            {
                MainForm.WriteLog("Resa.cs->Resa(idResa) : Connexion echouée !!" + ex.ToString());
            }
            cnn.Close();


        }

        public Resa(int idUtilisateur, int tempsRestant, DateTime heureConnexion)
        {
            MySqlConnection cnn = new MySqlConnection(Parametres.ConnectionString);
            try
            {
                cnn.Open();

                String sql = "INSERT into `tab_resa` (`id_computer_resa`, `id_user_resa`, `dateresa_resa`, `debut_resa`, `duree_resa`, `date_resa`, `status_resa`) VALUES (@idPoste, @idUtilisateur, CURRENT_DATE(), floor(TIME_TO_SEC(CURRENT_TIME())/60), @tempsRestant, CURRENT_DATE(),'0')";
                if (Parametres.Debug == "all")
                {
                    MainForm.WriteLog("frm_Temps.cs->frm_Temps_Load : inscription du début de la résa sql =  " + sql);
                }

                MySqlCommand cmd = new MySqlCommand(sql, cnn);
                cmd.Parameters.AddWithValue("@idPoste", Parametres.Poste_id);
                cmd.Parameters.AddWithValue("@idUtilisateur", idUtilisateur);
                cmd.Parameters.AddWithValue("@tempsRestant", tempsRestant.ToString());

                if (Parametres.Debug == "all")
                {
                    MainForm.WriteLog("Resa.cs->Resa(idUtilisateur ,tempsRestant, heureConnexion) : requete sql -------------------");

                    string query = cmd.CommandText;
                    foreach (MySqlParameter p in cmd.Parameters)
                    {
                        query = query.Replace(p.ParameterName, p.Value.ToString());
                    }
                    MainForm.WriteLog(query);
                }

                MySqlDataReader rdr = cmd.ExecuteReader();
                _id = (int)cmd.LastInsertedId;
                _date = heureConnexion;
                _dateResa = heureConnexion.Date;
                _debut = heureConnexion.Minute + heureConnexion.Hour * 60;
                _duree = tempsRestant;
                _idPoste = Parametres.Poste_id;
                _idUtilisateur = idUtilisateur;
                _statut = 0;

                if (Parametres.Debug == "all")
                {
                    MainForm.WriteLog("Resa.cs->Resa(idUtilisateur, tempsRestant, heureConnexion) : id resa en cours =  " + _id);
                }
                rdr.Close();
                Timer_Start();
            }
            catch (Exception ex)
            {
                MainForm.WriteLog("Resa.cs->Resa(idUtilisateur, tempsRestant, heureConnexion) :Connexion echouée !! " + ex.ToString());
            }
            cnn.Close();
        }

        private void Timer_Start()
        {

            _timer.Interval = 1000; // specify Interval time as you want
            _timer.Elapsed += new System.Timers.ElapsedEventHandler(Timer_Elapsed);
            _timer.Start();

        }


        public void Clore(int tempsPasse)
        {
            Utilisateur utilisateur = new Utilisateur(_idUtilisateur);

            MySqlConnection cnn = new MySqlConnection(Parametres.ConnectionString);
            try
            {
                cnn.Open();
                String sql = "UPDATE tab_resa SET `duree_resa`= @tempsPasse, `status_resa`= @statutResa WHERE `id_resa` = @idResa";
                MySqlCommand cmd = new MySqlCommand(sql, cnn);
                cmd.Parameters.AddWithValue("@tempsPasse", tempsPasse);
                if(utilisateur.Statut != 1 )
                {
                    _statut = 2; // animateur ou plus
                }
                else
                {
                    _statut = 1; // usager
                }
                cmd.Parameters.AddWithValue("@statutResa", _statut.ToString());
                cmd.Parameters.AddWithValue("@idResa", _id);
                if (Parametres.Debug == "all")
                {
                    MainForm.WriteLog("Resa.cs->Clore(tempsPasse) : requete sql -------------------");

                    string query = cmd.CommandText;
                    foreach (MySqlParameter p in cmd.Parameters)
                    {
                        query = query.Replace(p.ParameterName, p.Value.ToString());
                    }
                    MainForm.WriteLog(query);
                }
                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                MainForm.WriteLog("Resa.cs->clore(tempsPasse) : Connexion echouée !!" + ex.ToString());
            }
            cnn.Close();

        }

        public void Annuler()
        {
            MySqlConnection cnn = new MySqlConnection(Parametres.ConnectionString);
            try
            {
                cnn.Open();

                String sql = "UPDATE tab_resa SET duree_resa = '0', status_resa = '1' where id_resa = @idResa";
                MySqlCommand cmd = new MySqlCommand(sql, cnn);
                cmd.Parameters.AddWithValue("@idResa", _id);
                cmd.ExecuteNonQuery();
                _duree = 0;
                _statut = 1;

            }
            catch (Exception ex)
            {
                MainForm.WriteLog("Resa.cs->annuler() : Connexion echouée !!" + ex.ToString());
            }
            cnn.Close();

        }


        public void Activer()
        {
            MySqlConnection cnn = new MySqlConnection(Parametres.ConnectionString);
            try
            {
                cnn.Open();

                String sql = "UPDATE tab_resa SET status_resa = '0' WHERE id_resa = @idResa";
                MySqlCommand cmd = new MySqlCommand(sql, cnn);
                cmd.Parameters.AddWithValue("@idResa", _id);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MainForm.WriteLog("Resa.cs->activer() : Connexion echouée !!" + ex.ToString());
            }
            cnn.Close();
        }


        public static int VerifierResaEnCours(int idPoste)
        {
            int idResa = 0;
            MySqlConnection cnn = new MySqlConnection(Parametres.ConnectionString);
            try
            {
                cnn.Open();

                String sql = "SELECT * FROM tab_resa WHERE id_computer_resa = @idPoste AND status_resa = '0'";
                MySqlCommand cmd = new MySqlCommand(sql, cnn);
                cmd.Parameters.AddWithValue("@idPoste", idPoste);
                
                MySqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.HasRows) // une resa en cours a ete trouvée !
                {
                    if (Parametres.Debug == "all")
                    {
                        MainForm.WriteLog("Resa.cs->verifierResaEnCours(idPoste) : resa en cours trouvée pour le poste " + idPoste);
                    }
                    while (rdr.Read())
                    {
                        idResa = rdr.GetInt32("id_resa");
                    }
                    rdr.Close();
                }
            }
            catch (Exception ex)
            {
                MainForm.WriteLog("Resa.cs->verifierResaEnCours(idPoste) : Connexion echouée !!" + ex.ToString());
            }
            cnn.Close();

            return idResa;
        }

        public static int ProchaineResa(int idPoste)
        {
            int idResa = 0;
            MySqlConnection cnn = new MySqlConnection(Parametres.ConnectionString);
            try
            {
                // on cherche la salle associé au poste
                cnn.Open();


                // on demande la prochaine résa (limit 1 et order by date) pour le poste
                String sql = "SELECT * FROM tab_resa WHERE id_computer_resa = @idPoste AND status_resa ='1' AND duree_resa > 0 AND (dateresa_resa > CURRENT_DATE() OR (dateresa_resa = CURRENT_DATE() AND debut_resa >= ( floor( TIME_TO_SEC( CURRENT_TIME() ) /60) - duree_resa + 1))) order by dateresa_resa ASC, debut_resa ASC Limit 0,1";
                MySqlCommand cmd = new MySqlCommand(sql, cnn);
                cmd.Parameters.AddWithValue("@idPoste", idPoste);

                MySqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.HasRows) // une resa en cours a ete trouvée !
                {
                    if (Parametres.Debug == "all")
                    {
                        MainForm.WriteLog("Resa.cs->prochaineResa(idPoste) : resa a venir trouvée pour le poste " + idPoste);
                    }
                    while (rdr.Read())
                    {
                        idResa = rdr.GetInt32("id_resa");
                    }
                    rdr.Close();
                }
            }
            catch (Exception ex)
            {
                MainForm.WriteLog("Resa.cs->prochaineResa(idPoste) : Connexion echouée !!" + ex.ToString());
            }
            cnn.Close();

            return idResa;
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            MAJStatut();
        }

        private void MAJStatut() { 
            MySqlConnection cnn = new MySqlConnection(Parametres.ConnectionString);
            try { 
                cnn.Open();
                String sql = "SELECT `status_resa` FROM `tab_resa` WHERE `id_resa`= '" + _id + "'";

                MySqlCommand cmd = new MySqlCommand(sql, cnn);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {

                    if (!Convert.IsDBNull(rdr["status_resa"]))
                    {
                        _statut = System.Convert.ToInt32(rdr["status_resa"]);
                    }
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                MainForm.WriteLog("Resa.cs->MAJStatut() : Connexion echouée !!" + ex.ToString());
            }
            cnn.Close();
        }

        ~Resa()
        {
            _timer.Stop();
        }
    }
}
