using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace LGBServ
{
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom d'interface "IService1" à la fois dans le code et le fichier de configuration.
    [ServiceContract]
    public interface IFonctionsAdmin
    {
        [OperationContract]
        bool desactiverGestionnaireDesTaches(String SSID, bool desactive);

        [OperationContract]
        bool desactiverChangementMotDePasse(String SSID, bool desactive);

        [OperationContract]
        Dictionary<String, String> lireProfiles();

        [OperationContract]
        bool ecrireConfiguration(Dictionary<string, string> config);

        [OperationContract]
        Dictionary<string, string> lireConfiguration();

        [OperationContract]
        void writeLog(string message);
    }

}
