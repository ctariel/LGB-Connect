﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Microsoft.Win32;


namespace LGBConnect
{
    public static class Fonction
    {
        /// <summary>
        /// Fonction inutilisée. Devrait permettre à terme l'obfuscation des données du ficher de config
        /// </summary>
        /// <param name="plainText"></param>
        /// <returns></returns>
        public static string Base64Encode(string plainText)
        {
            if (plainText != null) {
                var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
                return System.Convert.ToBase64String(plainTextBytes);
            } else { return null; }
        }

        /// <summary>
        /// Fonction inutilisée. Devrait permettre à terme l'obfuscation des données du ficher de config
        /// </summary>
        /// <param name="base64EncodedData"></param>
        /// <returns></returns>
        public static string Base64Decode(string base64EncodedData)
        {
            if (base64EncodedData != null)
            {
                var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
                return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
            }
            else { return null; }
        }

        /// <summary>
        /// Fonction utilisée pour l'authentification Mysql
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string MD5Hash(string text)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            //compute hash from the bytes of text
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));

            //get hash result after compute it
            byte[] result = md5.Hash;

            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                //change it into 2 hexadecimal digits
                //for each byte
                strBuilder.Append(result[i].ToString("x2"));
            }

            return strBuilder.ToString();
        }

        /// <summary>
        /// Fonction simple pour la vérification d'un email
        /// </summary>
        /// <param name="strIn"></param>
        /// <returns></returns>
        public static bool IsValidEmail(string strIn)
        {
            // Return true if strIn is in valid e-mail format.
            return Regex.IsMatch(strIn,
                   @"^(?("")("".+?""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-zA-Z])@))" +
                   @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,6}))$");
        }


        /// <summary>
        /// Bloquer la possibilité d'accèder au gestionnaire des taches
        /// </summary>
        /// <param name="blocage"></param>
        public static void blocageGestionnaireDesTaches(bool blocage)
        {
            //MessageBox.Show("Début du blocage");

            ServiceFonctionsAdmin.FonctionsAdminClient client = new ServiceFonctionsAdmin.FonctionsAdminClient();

            foreach (KeyValuePair<string, string> entry in client.lireProfiles())
            {
                string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name.Split('\\').Last();

                if (userName.Equals(entry.Key))
                {
                    client.desactiverGestionnaireDesTaches(entry.Value, blocage);
                }
            }
        }

        /// <summary>
        /// Bloquer la possibilité de changer le mot de passe de l'utilisateur en cours.
        /// </summary>
        /// <param name="blocage"></param>
        public static void blocageChangementMotDePasse(bool blocage)
        {
            ServiceFonctionsAdmin.FonctionsAdminClient client = new ServiceFonctionsAdmin.FonctionsAdminClient();

            foreach (KeyValuePair<string, string> entry in client.lireProfiles())
            {
                string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name.Split('\\').Last();
                if (userName.Equals(entry.Key))
                {
                    client.desactiverChangementMotDePasse(entry.Value, blocage);
                }
            }
        }

        /// <summary>
        /// Fonction inutilisée pour le moment. Doit bloquer l'accès à l'explorateur de fichier.
        /// 
        /// Attention : le jour ou elle sera utilisée, il risque d'y avoir un problème de droit avec l'UAC.
        /// Il sera sans doute nécessaire de déporter le code dans LGBServ
        /// </summary>
        /// <param name="blocage"></param>
        private static void blocageExplorer(bool blocage)
        {
            MessageBox.Show("Supprimer l'accès à explorer : " + blocage);
            try
            {
                RegistryKey reg;
                String keyDir = "Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\Explorer\\DisallowRun";
                reg = Registry.CurrentUser.CreateSubKey(keyDir);
                reg = Registry.CurrentUser.OpenSubKey(keyDir, true);
                if (blocage)
                {
                    reg.SetValue("1", "explorer.exe", RegistryValueKind.String);
                }
                else
                {
                    reg.SetValue("1", "", RegistryValueKind.String);
                }
                MessageBox.Show(reg.GetValue("1").ToString());
                reg.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }
    }
}
