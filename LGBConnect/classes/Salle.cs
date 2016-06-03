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
    class Salle
    {
        private int _id;
        private String _nom;
        private int _idEspace;
        private String _commentaire;

        public String nom
        {
            get
            {
                return _nom;
            }
        }

        public int idEspace
        {
            get
            {
                return _idEspace;
            }
        }

        public Salle(String nomPoste)
        {
            MySqlConnection cnn = new MySqlConnection(Parametres.connectionString);
            try
            {
                // on cherche la salle associé au poste
                cnn.Open();
                string sql = "SELECT tab_salle.* FROM `tab_salle`, tab_computer WHERE tab_computer.id_salle = tab_salle.id_salle AND tab_computer.nom_computer = @nomPoste";
                MySqlCommand cmd = new MySqlCommand(sql, cnn);
                cmd.Parameters.AddWithValue("@nomPoste", nomPoste);

                MySqlDataReader rdr = cmd.ExecuteReader();

                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        _id = (int)rdr["id_salle"];
                        _nom = (String)rdr["nom_salle"];
                        _idEspace = (int)rdr["id_espace"];
                        _commentaire = (String)rdr["comment_salle"];

                        if (Parametres.debug == "all")
                        {
                            MainForm.writeLog("Salle.cs->Salle(nomPoste) : salle trouvée : id = " + _id);
                        }
                    }
                }
                else
                {
                    MainForm.writeLog("Pas de salle trouvée pour ce poste ! Veuillez revoir la configuration du logiciel !");
                }
            }
            catch (Exception ex)
            {
                MainForm.writeLog("Salle.cs->Salle(nom_poste) : Connexion echouée !!" + ex.ToString());
            }
            cnn.Close();
        }
    }
}

