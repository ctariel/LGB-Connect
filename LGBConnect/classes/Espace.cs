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
    public class Espace
    {
        // les champs sont calqués sur le schéma de la base de données
        private int _id;
        private String _nom;
        private int _idVille;
        private String _adresse;
        private String _tel;
        private String _fax;
        private String _logo;
        private int _couleur;
        private String _mail;

        public String nom
        {
            get
            {
                return _nom;
            }
        }


        public Espace(int idEspace)
        {
            MySqlConnection cnn = new MySqlConnection(Parametres.connectionString);
            try
            {
                // on cherche la salle associé au poste
                string sql = "SELECT * FROM tab_espace WHERE id_espace = @idEspace";
                cnn.Open();
                MySqlCommand cmd = new MySqlCommand(sql, cnn);
                cmd.Parameters.AddWithValue("@idEspace", idEspace);

                MySqlDataReader rdr = cmd.ExecuteReader();

                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        _id = rdr.GetInt32("id_espace");
                        _nom = rdr.GetString("nom_espace");
                        _idVille = rdr.GetInt32("id_city");
                        _adresse = rdr.GetString("adresse");
                        _tel = rdr.GetString("tel_espace");
                        _fax = rdr.GetString("fax_espace");
                        _logo = rdr.GetString("logo_espace");
                        _couleur = rdr.GetInt32("couleur_espace");
                        _mail = rdr.GetString("mail_espace");

                        if (Parametres.debug == "all")
                        {
                            MainForm.writeLog("Espace.cs->Espace(idEspace) : espace trouvé : id = " + _id);
                        }
                    }
                }
                else
                {
                    MainForm.writeLog("Espace.cs->Espace(idEspace) : Pas d'espace trouvée pour l'id " + idEspace + " ! Veuillez revoir la configuration du logiciel !");
                }
            }
            catch (Exception ex)
            {
                MainForm.writeLog("Espace.cs->Espace(idEspace) : Connexion echouée !!" + ex.ToString());
            }
            cnn.Close();

        }

    }
}
