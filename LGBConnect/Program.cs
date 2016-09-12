using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace LGBConnect
{
    static class Program
    {

        public static KeyboardHook kh;
        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                //Application.Run(new frm_Splash());
                using (kh = new KeyboardHook())
                {
                    Application.Run(new MainForm());
                }
                Fonction.blocageGestionnaireDesTaches(false);
                Fonction.blocageChangementMotDePasse(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Program : plantage !!" + ex.ToString());
            }
        }
    }
}