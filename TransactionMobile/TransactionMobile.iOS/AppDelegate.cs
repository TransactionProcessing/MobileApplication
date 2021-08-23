namespace TransactionMobile.iOS
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net.Mqtt;
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

            Calabash.Start();

            SfBorderRenderer.Init();
            SfButtonRenderer.Init();
            SfTabViewRenderer.Init();

            // Initialize the MQTT backdoor
            Task t = Backdoor.Instance.Initialize(mqttHost: "http://127.0.0.1:1883");

            // Handle backdoor events
            Backdoor.Instance.BackdoorEvent += HandleBackdoorEvent;
            
            // TODO: fix this
            this.LoadApplication(new App(this.Device, this.Database));

            return base.FinishedLaunching(app, options);
        }

        private void HandleBackdoorEvent(object sender, BackdoorEventArgs e)
        {
            // here is where you implement the backdoors
            if (e.Subtopic == "SetIntegrationTestModeOn")
            {
                SetIntegrationTestModeOn();
            }

            if (e.Subtopic == "UpdateTestMerchant")
            { 
                UpdateTestMerchant(e.Payload);
            }

            if (e.Subtopic == "UpdateTestContract")
            {
                UpdateTestContract(e.Payload);
            }
        }

        //[Export("SetIntegrationTestModeOn:")]
        public void SetIntegrationTestModeOn()
        {
            Console.WriteLine($"Inside SetIntegrationTestModeOn");
            App.IsIntegrationTestMode = true;
            App.Container = Bootstrapper.Run();

            IDevice device = new iOSDevice();
            String connectionString = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "TransactionProcessing.db");
            DatabaseContext database = new DatabaseContext(connectionString);
            App.Container.RegisterInstance(this.Database, new ContainerControlledLifetimeManager());
            App.Container.RegisterInstance(this.Device, new ContainerControlledLifetimeManager());
            SendResponse("Integration Test Set to On");
            }

        private void SendResponse(String response)
        {
            var client = MqttClient.CreateAsync("127.0.0.1", 1883).Result;
            client.ConnectAsync().Wait();
            MqttApplicationMessage m = new MqttApplicationMessage($"IOSBackdoor/responses", Encoding.Default.GetBytes(response));
            client.PublishAsync(m, MqttQualityOfService.ExactlyOnce).Wait();
        }

        //[Export("UpdateTestMerchant:")]
        public void UpdateTestMerchant(String merchantData)
        {
            if (App.IsIntegrationTestMode == true)
            {
                Merchant merchant = JsonConvert.DeserializeObject<Merchant>(merchantData);
                TestTransactionProcessorACLClient transactionProcessorAclClient = App.Container.Resolve<ITransactionProcessorACLClient>() as TestTransactionProcessorACLClient;
                transactionProcessorAclClient.UpdateTestMerchant(merchant);
                SendResponse("merchant added 1");

                TestEstateClient estateClient = App.Container.Resolve<IEstateClient>() as TestEstateClient;
                estateClient.UpdateTestMerchant(merchant);
                SendResponse("merchant added 2");

                TestSecurityServiceClient securityServiceClient = App.Container.Resolve<ISecurityServiceClient>() as TestSecurityServiceClient;
                Dictionary<String, String> claims = new Dictionary<String, String>();
                claims.Add("EstateId", merchant.EstateId.ToString());
                claims.Add("MerchantId", merchant.MerchantId.ToString());
                securityServiceClient.CreateUserDetails(merchant.MerchantUserName, claims);

                SendResponse("User Created");
            }
        }

        //[Export("UpdateTestContract:")]
        public void UpdateTestContract(String contractData)
        {
            if (App.IsIntegrationTestMode == true)
            {
                //ContractResponse contract = JsonConvert.DeserializeObject<ContractResponse>(contractData);
                Contract contract = JsonConvert.DeserializeObject<Contract>(contractData);
                TestEstateClient estateClient = App.Container.Resolve<IEstateClient>() as TestEstateClient;
                estateClient.UpdateTestContract(contract);
                SendResponse("contract added");
            }
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
}