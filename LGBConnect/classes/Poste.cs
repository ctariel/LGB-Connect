using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace LGBConnect.classes
{
    public class Poste
    {
        private int _id;
        private String _nom;
        private String _commentaire;
        private String _os;
        private int _usage;
        private String _fonction;
        private int _idSalle;
        private String _adresseMAC;
        private String _adresseIP;
        private String _nomHote;
        private DateTime _dernierEtat;
        private Boolean _configurationLGBConnect;

        public int id
        {
            get { return _id; }
        }

        public Poste(int idPoste)
        {
            MySqlConnection cnn = new MySqlConnection(Parametres.connectionString);
            try
            {
                cnn.Open();

                string sql = "SELECT * FROM tab_computer WHERE id_computer = @idPoste";
                MySqlCommand cmd = new MySqlCommand(sql, cnn);
                cmd.Parameters.AddWithValue("@idPoste", idPoste);

                if (Parametres.debug == "all")
                {
                    MainForm.writeLog("Poste.cs->Poste(idPoste) : requete sql -------------------");

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
                        _id = rdr.GetInt32("id_computer");
                        _nom = rdr.GetString("nom_computer");
                        _commentaire = rdr.GetString("comment_computer");
                        _os = rdr.GetString("os_computer");
                        _usage = rdr.GetInt32("usage_computer");
                        _fonction = rdr.GetString("fonction_computer");
                        _idSalle = rdr.GetInt32("id_salle");
                        _adresseMAC = rdr.GetString("adresse_mac_computer");
                        _adresseIP = rdr.GetString("adresse_ip_computer");
                        _nomHote = rdr.GetString("nom_hote_computer");
                        _dernierEtat = rdr.GetDateTime("date_lastetat_computer").AddMinutes(rdr.GetInt32("lastetat_computer"));
                        _configurationLGBConnect = (rdr.GetInt32("configurer_epnconnect_computer") != 0);
                    }
                }
            }
            catch (Exception ex)
            {
                MainForm.writeLog("Poste.cs->Poste(idPoste) : Connexion echouée !!" + ex.ToString());
            }
            cnn.Close();
        }

        public void MAJEtat()
        {
            MySqlConnection cnn = new MySqlConnection(Parametres.connectionString);
            try
            {
                cnn.Open();

                String sql = "UPDATE `tab_computer` SET `date_lastetat_computer`= CURRENT_DATE(), `lastetat_computer`= TIME_TO_SEC(CURRENT_TIME()) WHERE `id_computer`= @idPoste";
                MySqlCommand cmd = new MySqlCommand(sql, cnn);
                //cmd.Parameters.AddWithValue("@dernierEtat", ((DateTime.Now.Minute + DateTime.Now.Hour * 60) * 60 + DateTime.Now.Second).ToString());
                cmd.Parameters.AddWithValue("@idPoste", _id);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Poste.cs->MAJEtat() : Connexion echouée !!" + ex.ToString());
            }
            cnn.Close();
        }
    }
}
