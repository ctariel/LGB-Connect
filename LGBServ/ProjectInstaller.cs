using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace LGBServ
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
            this.Committed += new InstallEventHandler(ProjectInstaller_Committed);
        }

        private void serviceProcessInstaller_LGB_AfterInstall(object sender, InstallEventArgs e)
        {

        }

        protected override void OnCommitted(System.Collections.IDictionary savedState)
        {
            ServiceController sc = new ServiceController("Service_LGB");
            sc.Start();
        }
        private void ProjectInstaller_Committed(object sender, InstallEventArgs e)
        {
            ServiceController sc = new ServiceController("Service_LGB");
            if (sc.Status == ServiceControllerStatus.Stopped)
            {
                sc.Start();
            }
        }
    }
}
