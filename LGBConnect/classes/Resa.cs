using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;
using LGBConnect.classes;

namespace LGBConnect.classes
{
    class Resa
    {
        private int _id = 0;
        private int _idPoste;
        private int _idUtilisateur;
        private DateTime _dateResa;
        private int _debut;
        private int _duree;
        private DateTime _date; // ??? quelle utilité ?
        private int _statut;

        public Resa(int idResa)
        {
            MySqlConnection cnn = new MySqlConnection(Parametres.connectionString);
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
                        _id = (int)rdr["id_resa"];
                        _idPoste = (int)rdr["id_computer_resa"];
                        _idUtilisateur = (int)rdr["id_user_resa"];
                        _dateResa = (DateTime)rdr["dateresa_resa"];
                        _debut = (int)rdr["debut_resa"];
                        _duree = (int)rdr["duree_resa"];
                        _date = (DateTime)rdr["date_resa"];
                        _statut = Int32.Parse((String)rdr["status_resa"]);

                        if (Parametres.debug == "all")
                        {
                            MainForm.writeLog("Resa.cs->Resa(idResa) : Resa trouvée : id = " + _id);
                        }
                    }
                }
                else
                {
                    MainForm.writeLog("Pas de Réservation trouvée pour l'id " + idResa + " !");
                }
            }
            catch (Exception ex)
            {
                MainForm.writeLog("Resa.cs->Resa(idEspace) : Connexion echouée !!" + ex.ToString());
            }
            cnn.Close();

        }

        public Resa()
        {

        }

        public void annuler()
        {
            MySqlConnection cnn = new MySqlConnection(Parametres.connectionString);
            try
            {
                cnn.Open();

                String sql = "UPDATE tab_resa SET duree_resa = '0', status_resa = '1' where id_resa = @idResa";
                MySqlCommand cmd = new MySqlCommand(sql, cnn);
                cmd.Parameters.AddWithValue("@idResa", _id);
                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                MainForm.writeLog("Connexion echouée !!" + ex.ToString());
            }
            cnn.Close();

        }



        public static int verifierResaEnCours(int idPoste)
        {
            int idResa = 0;
            MySqlConnection cnn = new MySqlConnection(Parametres.connectionString);
            try
            {
                // on cherche la salle associé au poste
                cnn.Open();

                String sql = "SELECT * FROM tab_resa WHERE id_computer_resa = @idPoste AND status_resa = '0'";
                MySqlCommand cmd = new MySqlCommand(sql, cnn);
                cmd.Parameters.AddWithValue("@idPoste", idPoste);
                
                MySqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.HasRows) // une resa en cours a ete trouvée !
                {
                    if (Parametres.debug == "all")
                    {
                        MainForm.writeLog("mainForm.cs->MainForm_Shown : resa en cours trouvée");
                    }
                    while (rdr.Read())
                    {
                        idResa = (int)rdr["id_resa"];
                    }
                    rdr.Close();
                }
            }
            catch (Exception ex)
            {
                MainForm.writeLog("Salle.cs->Salle(nom_poste) : Connexion echouée !!" + ex.ToString());
            }
            cnn.Close();

            return idResa;
        }

    }
}
