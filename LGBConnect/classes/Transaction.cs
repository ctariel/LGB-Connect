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
    class Transaction
    {
        private int _id = 0;
        private String _type;
        private int _idUtilisateur;
        private float _idTarif;
        private int _nombreForfait;
        private DateTime _date;
        private int _statut;

        public int Id
        {
            get { return _id; }
        }
        public DateTime Date
        {
            get { return _date; }
        }

        public Transaction(int idTransaction)
        {
            MySqlConnection cnn = new MySqlConnection(Parametres.ConnectionString);
            try
            {
                cnn.Open();

                string sql = "SELECT * FROM tab_transactions WHERE id_transac = @idTransaction";
                MySqlCommand cmd = new MySqlCommand(sql, cnn);
                cmd.Parameters.AddWithValue("@idTransaction", idTransaction);

                if (Parametres.Debug == "all")
                {
                    MainForm.WriteLog("Transaction.cs->Transaction(id) : requete sql -------------------");

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
                        _type = rdr.GetString("type_transac");
                        _idUtilisateur = rdr.GetInt32("id_user");
                        _idTarif = rdr.GetInt32("id_tarif");
                        _nombreForfait = rdr.GetInt32("nbr_forfait");
                        _date = DateTime.Parse(rdr.GetString("date_transac"));
                        _statut = rdr.GetInt32("status_transac");
                    }

                }
            }
            catch (Exception ex)
            {
                MainForm.WriteLog("Transaction.cs->Transaction(id) : Connexion echouée !!" + ex.ToString());
            }
            cnn.Close();
        }

        public Transaction(Utilisateur utilisateur)
        {
            MySqlConnection cnn = new MySqlConnection(Parametres.ConnectionString);
            try
            {
                cnn.Open();

                string sql = "SELECT tab_transactions.* FROM tab_transactions WHERE tab_transactions.type_transac = \"temps\" and tab_transactions.id_user = @idUtilisateur";
                MySqlCommand cmd = new MySqlCommand(sql, cnn);
                cmd.Parameters.AddWithValue("@idUtilisateur", utilisateur.Id);

                if (Parametres.Debug == "all")
                {
                    MainForm.WriteLog("Transaction.cs->Transaction(Utilisateur) : requete sql -------------------");

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
                        _id = rdr.GetInt32("id_transac");
                        _type = rdr.GetString("type_transac");
                        _idUtilisateur = rdr.GetInt32("id_user");
                        _idTarif = rdr.GetInt32("id_tarif");
                        _nombreForfait = rdr.GetInt32("nbr_forfait");
                        _date = DateTime.Parse(rdr.GetString("date_transac"));
                        _statut = rdr.GetInt32("status_transac");
                    }

                }
            }
            catch (Exception ex)
            {
                MainForm.WriteLog("Transaction.cs->Transaction(idUtilisateur : Connexion echouée !!" + ex.ToString());
            }
            cnn.Close();
        }
    }
}
