using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LGBConnect.classes
{
    public static class Parametres
    {
        public static String Db_hote { get; set; }
        public static String Db_base { get; set; }
        public static String Db_utilisateur { get; set; }
        public static String Db_motdepasse { get; set; }
        public static String Poste_nom { get; set; }
        public static int  Poste_id { get; set; }
        public static String Poste_adresse_MAC { get; set; }
        public static String Poste_type { get; set; }
        public static String Poste_chrono { get; set; }
        public static String Debug { get; set; }

        public static int LireConfiguration()
        {
            Dictionary<string, string> config = new Dictionary<string, string>();

            try
            {
                ServiceFonctionsAdmin.FonctionsAdminClient client = new ServiceFonctionsAdmin.FonctionsAdminClient();
                config = client.lireConfiguration();

                if (config != null)
                {
                    Db_hote = config["mysql_hote"];
                    Db_base = config["mysql_base"];
                    Db_utilisateur = config["mysql_utilisateur"];
                    Db_motdepasse = config["mysql_mot_de_passe"];
                    Poste_nom = config["poste_nom"];
                    Poste_id = Int32.Parse(config["poste_id"]);
                    Poste_adresse_MAC = config["poste_adresse_MAC"];
                    Poste_type = config["poste_type"];
                    Poste_chrono = config["poste_chrono"];
                    Debug = config["debug"];
                    return 0;
                }
                else
                {
                    return 1;
                }
            }
            catch 
            {
                return 1;
            }

        }

        public static int EcrireConfiguration()
        {
            Dictionary<string, string> config = new Dictionary<string, string>();

            try
            {
                // ecriture du fichier de configuration

                config["mysql_hote"] = Db_hote;
                config["mysql_base"] = Db_base;
                config["mysql_utilisateur"] = Db_utilisateur;
                config["mysql_mot_de_passe"] = Db_motdepasse;
                config["poste_nom"] = Poste_nom;
                config["poste_id"] = Poste_id.ToString();
                config["poste_adresse_MAC"] = Poste_adresse_MAC;
                config["poste_type"] = Poste_type;


                ServiceFonctionsAdmin.FonctionsAdminClient client = new ServiceFonctionsAdmin.FonctionsAdminClient();
                client.ecrireConfiguration(config);

                return 0;

            }
            catch
            {
                return 1;
            }
            // mise à jour de la base de donnnées
        }

        public static String ConnectionString
        {
            get
            {
                if ( String.IsNullOrEmpty(Db_hote) || String.IsNullOrEmpty(Db_base) || String.IsNullOrEmpty(Db_utilisateur) || String.IsNullOrEmpty(Db_motdepasse)) {
                    return null;
                }
                else
                {
                    return "server=" + Db_hote + ";database=" + Db_base + ";uid=" + Db_utilisateur + ";pwd=" + Db_motdepasse + ";Convert Zero Datetime=True";
                }
            }
        }

    }
}
