namespace LGBServ
{
    partial class ProjectInstaller
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur de composants

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.serviceProcessInstaller_LGB = new System.ServiceProcess.ServiceProcessInstaller();
            this.serviceInstaller_LGB = new System.ServiceProcess.ServiceInstaller();
            // 
            // serviceProcessInstaller_LGB
            // 
            this.serviceProcessInstaller_LGB.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.serviceProcessInstaller_LGB.Password = null;
            this.serviceProcessInstaller_LGB.Username = null;
            this.serviceProcessInstaller_LGB.AfterInstall += new System.Configuration.Install.InstallEventHandler(this.serviceProcessInstaller_LGB_AfterInstall);
            // 
            // serviceInstaller_LGB
            // 
            this.serviceInstaller_LGB.Description = "Service de connexion à Cyber-Gestionnaire - fonctions nécéssitant des privilèges";
            this.serviceInstaller_LGB.DisplayName = "Service LGB";
            this.serviceInstaller_LGB.ServiceName = "Service_LGB";
            this.serviceInstaller_LGB.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.serviceProcessInstaller_LGB,
            this.serviceInstaller_LGB});
            this.Committed += new System.Configuration.Install.InstallEventHandler(this.ProjectInstaller_Committed);

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller serviceProcessInstaller_LGB;
        private System.ServiceProcess.ServiceInstaller serviceInstaller_LGB;
    }
}