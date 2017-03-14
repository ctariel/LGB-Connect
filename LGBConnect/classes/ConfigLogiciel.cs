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
    public class ConfigLogiciel
    {
        private int _id = 0;
        private int _idEspace;
        private int _configMenu;
        private int _pageInscription;
        private int _pageRenseignement;
        private int _connexionAnim;
        private int _bloquageTouche;
        private int _affichageTemps;
        private int _deconnexionAuto;
        private int _fermetureSesssion;

        public Boolean PageInscription
        {
            get
            {
                return (_pageInscription != 0);
            }
        }
        public Boolean Exists()
        {
            return _id != 0;
        }

        public Boolean DeconnexionAuto
        {
            get
            {
                return (_deconnexionAuto != 0);
            }
        }

        public ConfigLogiciel(int idEspace)
        {
            MySqlConnection cnn = new MySqlConnection(Parametres.ConnectionString);
            try
            {
                // on cherche la salle associé au poste
                string sql = "SELECT * FROM tab_config_logiciel WHERE id_espace = @idEspace";
                cnn.Open();
                MySqlCommand cmd = new MySqlCommand(sql, cnn);
                cmd.Parameters.AddWithValue("@idEspace", idEspace);

                MySqlDataReader rdr = cmd.ExecuteReader();

                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        _id = rdr.GetInt32("id_config_logiciel");
                        _idEspace = rdr.GetInt32("id_espace");
                        _configMenu = rdr.GetInt32("config_menu_logiciel");
                        _pageInscription = rdr.GetInt32("page_inscription_logiciel");
                        _pageRenseignement = rdr.GetInt32("page_renseignement_logiciel");
                        _connexionAnim = rdr.GetInt32("connexion_anim_logiciel");
                        _bloquageTouche = rdr.GetInt32("bloquage_touche_logiciel");
                        _affichageTemps = rdr.GetInt32("affichage_temps_logiciel");
                        _deconnexionAuto = rdr.GetInt32("deconnexion_auto_logiciel");
                        _fermetureSesssion = rdr.GetInt32("fermeture_session_auto");

                        if (Parametres.Debug == "all")
                        {
                            MainForm.WriteLog("ConfigLogiciel.cs->ConfigLogiciel(idEspace) : ConfigLogiciel trouvée : id = " + _id);
                            MainForm.WriteLog("ConfigLogiciel.cs->ConfigLogiciel(idEspace) : idEspace = " + _id);
                            MainForm.WriteLog("ConfigLogiciel.cs->ConfigLogiciel(idEspace) : _configMenu = " + _configMenu);
                            MainForm.WriteLog("ConfigLogiciel.cs->ConfigLogiciel(idEspace) : _pageInscription = " + _pageInscription);
                            MainForm.WriteLog("ConfigLogiciel.cs->ConfigLogiciel(idEspace) : _pageRenseignement = " + _pageRenseignement);
                            MainForm.WriteLog("ConfigLogiciel.cs->ConfigLogiciel(idEspace) : _connexionAnim = " + _connexionAnim);
                            MainForm.WriteLog("ConfigLogiciel.cs->ConfigLogiciel(idEspace) : _bloquageTouche = " + _bloquageTouche);
                            MainForm.WriteLog("ConfigLogiciel.cs->ConfigLogiciel(idEspace) : _affichageTemps = " + _affichageTemps);
                            MainForm.WriteLog("ConfigLogiciel.cs->ConfigLogiciel(idEspace) : _deconnexionAuto = " + _deconnexionAuto);
                            MainForm.WriteLog("ConfigLogiciel.cs->ConfigLogiciel(idEspace) : _fermetureSesssion = " + _fermetureSesssion);

                        }
                    }
                }
                else
                {
                    MainForm.WriteLog("Pas de config logiciel trouvée pour l'espace id = " + idEspace + " !");
                }
            }
            catch (Exception ex)
            {
                MainForm.WriteLog("ConfigLogiciel.cs->ConfigLogiciel(idEspace) : Connexion echouée !!" + ex.ToString());
            }
            cnn.Close();

        }
    }
}
