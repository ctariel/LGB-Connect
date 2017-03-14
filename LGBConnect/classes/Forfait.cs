using System;
using MySql.Data;
using MySql.Data.Types;
using MySql.Data.MySqlClient;

namespace LGBConnect.classes
{
    public class Forfait
    {
        // les champs sont calqués sur le schéma de la base de données
        private int _id = 0;
        private DateTime _dateCreation;
        private int _type;
        private String _nom;
        private float _prix;
        private String _critere;
        private String _commentaire;
        private int _dureeValidite;
        private int _uniteValidite;
        private Boolean _tempsForfaitIllimite;
        private DateTime _dateDebut;
        private int _statut;
        private int _dureeConsultation;
        private int _uniteConsultation;
        private int _frequenceConsultation;
        private int _tempsAffectationOccasionnel;
        private int _nombreAtelier;

        public int Id
        {
            get { return _id; }
        }
        public DateTime DateCreation
        {
            get { return _dateCreation; }
        }
        public int Type
        {
            get { return _type; }
        }
        public String Nom
        {
            get { return _nom; }
        }
        public float Prix
        {
            get { return _prix; }
        }
        public int DureeValidite
        {
            get { return _dureeValidite; }
        }
        public int UniteValidite
        {
            get { return _uniteValidite; }
        }

        public Forfait(int idForfait)
        {
            MySqlConnection cnn = new MySqlConnection(Parametres.ConnectionString);
            try
            {
                cnn.Open();

                string sql = "SELECT * FROM tab_forfait WHERE id_forfait = @idForfait";
                MySqlCommand cmd = new MySqlCommand(sql, cnn);
                cmd.Parameters.AddWithValue("@idForfait", idForfait);

                if (Parametres.Debug == "all")
                {
                    MainForm.WriteLog("Forfait.cs->Forfait(id) : requete sql -------------------");

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
                        _id = rdr.GetInt32("id_forfait");
                        _dateCreation = DateTime.Parse(rdr.GetString("date_creation_forfait"));
                        _type = rdr.GetInt32("type_forfait");
                        _nom = rdr.GetString("nom_forfait");
                        _prix = rdr.GetFloat("prix_forfait");
                        _critere = rdr.GetString("critere_forfait");
                        _commentaire = rdr.GetString("commentaire_forfait");
                        _dureeValidite = rdr.GetInt32("nombre_duree_forfait");
                        _uniteValidite = rdr.GetInt32("unite_duree_forfait");
                        _tempsForfaitIllimite = (rdr.GetInt32("temps_forfait_illimite") != 0);
                        _dateDebut = DateTime.Parse(rdr.GetString("date_debut_forfait"));
                        _statut = rdr.GetInt32("status_forfait");
                        _dureeConsultation = rdr.GetInt32("nombre_temps_affectation");
                        _uniteConsultation = rdr.GetInt32("unite_temps_affectation");
                        _frequenceConsultation = rdr.GetInt32("frequence_temps_affectation");
                        _tempsAffectationOccasionnel = rdr.GetInt32("temps_affectation_occasionnel");
                        _nombreAtelier = rdr.GetInt32("nombre_atelier_forfait");
                    }

                }
            }
            catch (Exception ex)
            {
                MainForm.WriteLog("Forfait.cs->Forfait(id) : Connexion echouée !!" + ex.ToString());
            }
            cnn.Close();
        }

        public Forfait(Utilisateur utilisateur)
        {
            MySqlConnection cnn = new MySqlConnection(Parametres.ConnectionString);
            try
            {
                cnn.Open();

                string sql = "SELECT tab_forfait.* FROM tab_forfait, tab_transactions WHERE tab_forfait.id_forfait = tab_transactions.id_tarif and tab_transactions.type_transac = \"temps\" and tab_transactions.id_user = @idUtilisateur";
                MySqlCommand cmd = new MySqlCommand(sql, cnn);
                cmd.Parameters.AddWithValue("@idUtilisateur", utilisateur.Id);

                if (Parametres.Debug == "all")
                {
                    MainForm.WriteLog("Forfait.cs->Forfait(Utilisateur) : requete sql -------------------");

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
                        _id = rdr.GetInt32("id_forfait");
                        _dateCreation = DateTime.Parse(rdr.GetString("date_creation_forfait"));
                        _type = rdr.GetInt32("type_forfait");
                        _nom = rdr.GetString("nom_forfait");
                        _prix = rdr.GetFloat("prix_forfait");
                        _critere = rdr.GetString("critere_forfait");
                        _commentaire = rdr.GetString("commentaire_forfait");
                        _dureeValidite = rdr.GetInt32("nombre_duree_forfait");
                        _uniteValidite = rdr.GetInt32("unite_duree_forfait");
                        _tempsForfaitIllimite = (rdr.GetInt32("temps_forfait_illimite") != 0);
                        _dateDebut = DateTime.Parse(rdr.GetString("date_debut_forfait"));
                        _statut = rdr.GetInt32("status_forfait");
                        _dureeConsultation = rdr.GetInt32("nombre_temps_affectation");
                        _uniteConsultation = rdr.GetInt32("unite_temps_affectation");
                        _frequenceConsultation = rdr.GetInt32("frequence_temps_affectation");
                        _tempsAffectationOccasionnel = rdr.GetInt32("temps_affectation_occasionnel");
                        _nombreAtelier = rdr.GetInt32("nombre_atelier_forfait");
                    }

                }
            }
            catch (Exception ex)
            {
                MainForm.WriteLog("Forfait.cs->Forfait(Utilisateur) : Connexion echouée !!" + ex.ToString());
            }
            cnn.Close();
        }

        public Boolean EstValide()
        {
            Boolean estValide = false;




            return estValide;

        }

    }
}
