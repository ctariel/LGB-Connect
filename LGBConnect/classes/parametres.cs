using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LGBConnect.classes
{
    static class Parametres
    {
        public static String db_hote { get; set; }
        public static String db_base { get; set; }
        public static String db_utilisateur { get; set; }
        public static String db_motdepasse { get; set; }
        public static String poste_nom { get; set; }
        public static int poste_id { get; set; }
        public static String poste_adresse_MAC { get; set; }
        public static String poste_type { get; set; }
        public static String poste_chrono { get; set; }
        public static String debug { get; set; }

        public static int lireConfiguration()
        {
            Dictionary<string, string> config = new Dictionary<string, string>();

            try
            {
                ServiceFonctionsAdmin.FonctionsAdminClient client = new ServiceFonctionsAdmin.FonctionsAdminClient();
                config = client.lireConfiguration();

                if (config != null)
                {
                    db_hote = config["mysql_hote"];
                    db_base = config["mysql_base"];
                    db_utilisateur = config["mysql_utilisateur"];
                    db_motdepasse = config["mysql_mot_de_passe"];
                    poste_nom = config["poste_nom"];
                    poste_id = Int32.Parse(config["poste_id"]);
                    poste_adresse_MAC = config["poste_adresse_MAC"];
                    poste_type = config["poste_type"];
                    poste_chrono = config["poste_chrono"];
                    debug = config["debug"];
                    return 0;
                }
                else
                {
                    return 1;
                }
            }
            catch (Exception ex)
            {
                return 1;
            }

        }

        public static int ecrireConfiguration()
        {
            Dictionary<string, string> config = new Dictionary<string, string>();

            try
            {
                // ecriture du fichier de configuration

                config["mysql_hote"] = db_hote;
                config["mysql_base"] = db_base;
                config["mysql_utilisateur"] = db_utilisateur;
                config["mysql_mot_de_passe"] = db_motdepasse;
                config["poste_nom"] = poste_nom;
                config["poste_id"] = poste_id.ToString();
                config["poste_adresse_MAC"] = poste_adresse_MAC;
                config["poste_type"] = poste_type;


                ServiceFonctionsAdmin.FonctionsAdminClient client = new ServiceFonctionsAdmin.FonctionsAdminClient();
                client.ecrireConfiguration(config);

                return 0;

            }
            catch (Exception ex)
            {
                return 1;
            }
            // mise à jour de la base de donnnées
        }

        public static String connectionString
        {
            get
            {
                if ( String.IsNullOrEmpty(db_hote) || String.IsNullOrEmpty(db_base) || String.IsNullOrEmpty(db_utilisateur) || String.IsNullOrEmpty(db_motdepasse)) {
                    return null;
                }
                else
                {
                    return "server=" + db_hote + ";database=" + db_base + ";uid=" + db_utilisateur + ";pwd=" + db_motdepasse + ";";
                }
            }
        }

    }
}
