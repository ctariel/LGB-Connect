﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace LGBServ
{
    /// <summary>
    /// Cette classe doit contenir toutes les actions bloquées par l'UAC
    /// </summary>


    public class FonctionsAdmin : IFonctionsAdmin
    {
        /// <summary>
        /// Lors du blocage du poste, on désactive les fonctionnalitées suivantes
        /// lors d'un ctrl-alt-suppr :
        /// - gestionnaire des taches
        /// - verrouillage du poste
        /// </summary>
        public bool desactiverGestionnaireDesTaches(String SSID, bool desactive)
        {
            //MessageBox.Show("Supprimer le gestionnaire des taches : " + blocage);
            try
            {
                RegistryKey regSSID;
                RegistryKey reg;

                String keyDir = "Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System";

                regSSID = Registry.Users.OpenSubKey(SSID, true);
                reg = regSSID.CreateSubKey(keyDir);
                reg = regSSID.OpenSubKey(keyDir, true);
                

                if (desactive)
                {
                    reg.SetValue("DisableTaskMgr", 1, RegistryValueKind.DWord);
                    reg.SetValue("DisableLockWorkstation", 1, RegistryValueKind.DWord);
                    Service_LGB.WriteLog("Gestionnaire des taches désactivé pour le SSID : " + SSID);
                }
                else
                {
                    reg.SetValue("DisableTaskMgr", 0, RegistryValueKind.DWord);
                    reg.SetValue("DisableLockWorkstation", 0, RegistryValueKind.DWord);
                    Service_LGB.WriteLog("Gestionnaire des taches ré-activé pour le SSID : " + SSID);
                }
                reg.Close();
                return true;
            }
            catch (Exception ex)
            {
                Service_LGB.WriteLog("Erreur d'écriture DisableTaskMgr : " + ex.ToString());
                return false;
            }
        }

        
        /// <summary>
        /// Pour évitre un changement de mot de passe de l'utilisateur publique
        /// </summary>
        public bool desactiverChangementMotDePasse(String SSID, bool desactive)
        {
            //MessageBox.Show("Supprimer le gestionnaire des taches : " + blocage);
            try
            {
                RegistryKey regSSID;
                RegistryKey reg;

                String keyDir = "Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System";

                regSSID = Registry.Users.OpenSubKey(SSID, true);
                reg = regSSID.CreateSubKey(keyDir);
                reg = regSSID.OpenSubKey(keyDir, true);


                if (desactive)
                {
                    reg.SetValue("DisableChangePassword", 1, RegistryValueKind.DWord);
                    Service_LGB.WriteLog("Changement du mot de passe désactivé pour le SSID : " + SSID);
                }
                else
                {
                    reg.SetValue("DisableChangePassword", 0, RegistryValueKind.DWord);
                    Service_LGB.WriteLog("Changement du mot de passe ré-activé pour le SSID : " + SSID);
                }
                reg.Close();
                return true;
            }
            catch (Exception ex)
            {
                Service_LGB.WriteLog("Erreur d'écriture DisableTaskMgr : " + ex.ToString());
                return false;
            }
        }

        public Dictionary<String, String> lireProfiles()
        {
            Service_LGB.WriteLog("Demande de lecture des profiles");

            RegistryKey OurKey = Registry.LocalMachine;
            OurKey = OurKey.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\ProfileList", true); // refusé par l'UAC sous windows 10....
            Dictionary<String, String> liste = new Dictionary<string, string>();

            try {
                foreach (string Keyname in OurKey.GetSubKeyNames())
                {
                    if (Keyname.StartsWith("S-1-5-21"))
                    {
                        RegistryKey key = Registry.Users.OpenSubKey(Keyname).OpenSubKey("Volatile Environment");
                        if (key.GetValue("USERNAME") != null)
                        {
                            Service_LGB.WriteLog("Profil trouvé : name  = " + key.GetValue("USERNAME").ToString() + " / SSID = " + Keyname);
                            liste[key.GetValue("USERNAME").ToString()] = Keyname;
                        }
                    }
                }
                return liste;
            }
            catch (Exception ex)
            {
                Service_LGB.WriteLog("Erreur de lecture des profiles : " + ex.ToString());
                return null;
            }
        }

        public bool ecrireConfiguration(Dictionary<string, string> config)
        {
            // le dictionnaire doit contenir les champs  
            try
            {
                String configFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.txt");
                Service_LGB.WriteLog("Création du fichier Ini.");

                IniFile ini = new IniFile(configFile);

                Service_LGB.WriteLog("Ecriture dans le fichier Ini.");
                ini.IniWriteValue("mysql", "hote", config["mysql_hote"]);
                ini.IniWriteValue("mysql", "base", config["mysql_base"]);
                ini.IniWriteValue("mysql", "utilisateur", config["mysql_utilisateur"]);
                ini.IniWriteValue("mysql", "mot_de_passe", config["mysql_mot_de_passe"]);
                ini.IniWriteValue("poste", "nom", config["poste_nom"]);
                ini.IniWriteValue("poste", "id", config["poste_id"]);
                ini.IniWriteValue("poste", "adresse_MAC", config["poste_adresse_MAC"]);
                ini.IniWriteValue("poste", "type", config["poste_type"]);
                return true;
            }
            catch (Exception ex)
            {
                Service_LGB.WriteLog("Erreur d'écriture du fichier Ini : " + ex.ToString());
                return false;
            }

        }

        public Dictionary<string, string> lireConfiguration()
        {
            String configFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.txt");
            Dictionary<string, string> config = new Dictionary<string, string>();

            Service_LGB.WriteLog("Demande de lecture de la configration");

            if (File.Exists(configFile))
            {
                IniFile ini = new IniFile(configFile);

                config["mysql_hote"] = ini.IniReadValue("mysql", "hote").Trim('\0');
                config["mysql_base"] = ini.IniReadValue("mysql", "base").Trim('\0');
                config["mysql_utilisateur"] = ini.IniReadValue("mysql", "utilisateur").Trim('\0');
                config["mysql_mot_de_passe"] = ini.IniReadValue("mysql", "mot_de_passe").Trim('\0');
                config["poste_nom"] = ini.IniReadValue("poste", "nom").Trim('\0');
                config["poste_id"] = ini.IniReadValue("poste", "id").Trim('\0');
                config["poste_type"] = ini.IniReadValue("poste", "type").Trim('\0');
                return config;
            }
            else
            {
                return null;
            }

        }
    }
}