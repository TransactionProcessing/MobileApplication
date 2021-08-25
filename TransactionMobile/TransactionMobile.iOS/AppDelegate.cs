namespace TransactionMobile.iOS
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net.Mqtt;
    using System.Net.NetworkInformation;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading.Tasks;
    using Clients;
    using Common;
    using Database;
    using EstateManagement.Client;
    using Foundation;
    using Newtonsoft.Json;
    using SecurityService.Client;
    using Syncfusion.XForms.iOS.Border;
    using Syncfusion.XForms.iOS.Buttons;
    using Syncfusion.XForms.iOS.TabView;
    using TransactionMobile.Backdoor;
    using TransactionMobile.IntegrationTestClients;
    using UIKit;
    using Unity;
    using Unity.Lifetime;
    using Xamarin;
    using Xamarin.Forms;
    using Xamarin.Forms.Platform.iOS;

    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Xamarin.Forms.Platform.iOS.FormsApplicationDelegate" />
    [Register("AppDelegate")]
    public class AppDelegate : FormsApplicationDelegate
    {
        #region Fields
        
        /// <summary>
        /// The configuration
        /// </summary>
        private IConfiguration Configuration;

        /// <summary>
        /// The device
        /// </summary>
        private IDevice Device;

        /// <summary>
        /// The logging database
        /// </summary>
        private IDatabaseContext Database;

        #endregion

        #region Methods

        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        /// <summary>
        /// Finisheds the launching.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        public override Boolean FinishedLaunching(UIApplication app,
                                                  NSDictionary options)
        {
            AppDomain.CurrentDomain.UnhandledException += this.CurrentDomainOnUnhandledException;
            TaskScheduler.UnobservedTaskException += this.TaskSchedulerOnUnobservedTaskException;

            String connectionString = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "TransactionProcessing.db");
            this.Device = new iOSDevice();
            this.Database = new DatabaseContext(connectionString);
            
            Forms.Init();
            
            SfBorderRenderer.Init();
            SfButtonRenderer.Init();
            SfTabViewRenderer.Init();
            
            // TODO: Check for test data file
            //PushFile will be used
            FileInfo fi = new FileInfo("/data/local/tmp/testdata.txt");
            if (fi.Exists)
            {
                this.SetIntegrationTestModeOn();

                // Read the file 
                String fileData = File.ReadAllText(fi.FullName);

                TestData t = JsonConvert.DeserializeObject<TestData>(fileData);
                this.UpdateTestMerchant(t.Merchant);
                foreach (Contract contract in t.Contracts)
                {
                    this.UpdateTestContract(contract);
                }
            }
            
            // TODO: fix this
            this.LoadApplication(new App(this.Device, this.Database));

            return base.FinishedLaunching(app, options);
        }
        
        private void SetIntegrationTestModeOn()
        {
            App.IsIntegrationTestMode = true;
            App.Container = Bootstrapper.Run();

            IDevice device = new iOSDevice();
            String connectionString = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "TransactionProcessing.db");
            DatabaseContext database = new DatabaseContext(connectionString);
            App.Container.RegisterInstance(this.Database, new ContainerControlledLifetimeManager());
            App.Container.RegisterInstance(this.Device, new ContainerControlledLifetimeManager());

        }
        
        private void UpdateTestMerchant(Merchant merchant)
        {
            TestTransactionProcessorACLClient transactionProcessorAclClient = App.Container.Resolve<ITransactionProcessorACLClient>() as TestTransactionProcessorACLClient;
            transactionProcessorAclClient.UpdateTestMerchant(merchant);
            
            TestEstateClient estateClient = App.Container.Resolve<IEstateClient>() as TestEstateClient;
            estateClient.UpdateTestMerchant(merchant);
          
            TestSecurityServiceClient securityServiceClient = App.Container.Resolve<ISecurityServiceClient>() as TestSecurityServiceClient;
            Dictionary<String, String> claims = new Dictionary<String, String>();
            claims.Add("EstateId", merchant.EstateId.ToString());
            claims.Add("MerchantId", merchant.MerchantId.ToString());
            securityServiceClient.CreateUserDetails(merchant.MerchantUserName, claims);
        }
        
        private void UpdateTestContract(Contract contract)
        { 
            TestEstateClient estateClient = App.Container.Resolve<IEstateClient>() as TestEstateClient;
            estateClient.UpdateTestContract(contract);
        }

        /// <summary>
        /// Currents the domain on unhandled exception.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="unhandledExceptionEventArgs">The <see cref="UnhandledExceptionEventArgs"/> instance containing the event data.</param>
        private void CurrentDomainOnUnhandledException(Object sender,
                                                       UnhandledExceptionEventArgs unhandledExceptionEventArgs)
        {
            var newExc = new Exception("CurrentDomainOnUnhandledException", unhandledExceptionEventArgs.ExceptionObject as Exception);
            // TODO: Logging
            //this.AnalysisLogger.TrackCrash(newExc);
        }

        /// <summary>
        /// Tasks the scheduler on unobserved task exception.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="unobservedTaskExceptionEventArgs">The <see cref="UnobservedTaskExceptionEventArgs"/> instance containing the event data.</param>
        private void TaskSchedulerOnUnobservedTaskException(Object sender,
                                                            UnobservedTaskExceptionEventArgs unobservedTaskExceptionEventArgs)
        {
            var newExc = new Exception("TaskSchedulerOnUnobservedTaskException", unobservedTaskExceptionEventArgs.Exception);
            // TODO: Logging
            // this.AnalysisLogger.TrackCrash(newExc);
        }

        #endregion
    }

    public class TestData
    {
        public Merchant Merchant { get; set; }
        public List<Contract> Contracts { get; set; }
    }
}