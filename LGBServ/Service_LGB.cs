using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Configuration;
using System.Configuration.Install;

namespace LGBServ
{
    /// <summary>
    /// Le service sert à effectuer toutes les opérations nécéssitant des privilèges elevés sur le système.
    /// 
    /// Elle fourni un servie WCF pour exposer les fonctions utiles (voir FonctionsAdmin)
    /// 
    /// Un log est créé dans les évenements windo192ws, mais est peu utilisé.
    /// En revanche, un autre log est écrit dans le répertoire de l'application (fichier logfile.txt)
    /// 
    /// </summary>
    public partial class Service_LGB : ServiceBase
    {
        public ServiceHost serviceHost = null;
        System.Diagnostics.EventLog eventLog1;
        int eventId = 0;

        public Service_LGB()
        {
            InitializeComponent();
            eventLog1 = new System.Diagnostics.EventLog();
            if (!System.Diagnostics.EventLog.SourceExists("LGBConnect"))
            {
                System.Diagnostics.EventLog.CreateEventSource("LGBConnect", "LGBConnectLog");
            }
            eventLog1.Source = "LGBConnect";
            eventLog1.Log = "LGBConnectLog";
        }

        protected override void OnStart(string[] args)
        {

            if (serviceHost != null)
            {
                serviceHost.Close();
            }

            // Create a ServiceHost for the CalculatorService type and 
            // provide the base address.
            serviceHost = new ServiceHost(typeof(FonctionsAdmin));

            // Open the ServiceHostBase to create listeners and start 
            // listening for messages.
            serviceHost.Open();

            WriteLog("LGB Service démarré");
            eventLog1.WriteEntry("Démarrage du service LGB_Connect", EventLogEntryType.Information, eventId++);

        }

        protected override void OnStop()
        {
            if (serviceHost != null)
            {
                serviceHost.Close();
                serviceHost = null;
            }
            WriteLog("LGB Service arrété");
            //System.IO.File.Move(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logFile.txt"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logFile-" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") +".txt"));
            eventLog1.WriteEntry("Arrêt du service LGB_Connect", EventLogEntryType.Information, eventId++);
        }

        public static void WriteLog(Exception ex)
        {
            StreamWriter sw = null;

            try
            {
                sw = new StreamWriter(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logFile.txt"), true);
                sw.WriteLine(DateTime.Now.ToString() + " : " + ex.Source.ToString().Trim() + " ; " + ex.Message.ToString().Trim());
                sw.Flush();
                sw.Close();
            }
            catch
            {
            }

        }

        public static void WriteLog(string message)
        {
            StreamWriter sw = null;

            try
            {
                sw = new StreamWriter(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logFile.txt"), true);
                sw.WriteLine(DateTime.Now.ToString() +  " ; " + message);
                sw.Flush();
                sw.Close();
            }
            catch
            {
            }

        }
    }

    /// <summary>
    /// Devrait démarrer le service juste parès l'installation. Ne fonctionne pas pour le moment.
    /// </summary>
    class ServInstaller : ServiceInstaller
    {
        protected override void OnCommitted(System.Collections.IDictionary savedState)
        {
            ServiceController sc = new ServiceController("Service_LGB");
            sc.Start();
        }
    }

}
