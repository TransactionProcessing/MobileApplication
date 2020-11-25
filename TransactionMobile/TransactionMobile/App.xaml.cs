namespace TransactionMobile
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Common;
    using Database;
    using Models;
    using Newtonsoft.Json;
    using Presenters;
    using SecurityService.DataTransferObjects.Responses;
    using Services;
    using Syncfusion.Licensing;
    using Unity;
    using Unity.Lifetime;
    using Xamarin.Forms;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Xamarin.Forms.Application" />
    [ExcludeFromCodeCoverage]
    public partial class App : Application
    {
        #region Fields

        /// <summary>
        /// The configuration
        /// </summary>
        public static IConfiguration Configuration;
        
        /// <summary>
        /// Unity container
        /// </summary>
        public static IUnityContainer Container;

        /// <summary>
        /// The token response
        /// </summary>
        public static TokenResponse TokenResponse;

        /// <summary>
        /// The estate identifier
        /// </summary>
        public static Guid EstateId;

        /// <summary>
        /// The merchant identifier
        /// </summary>
        public static Guid MerchantId;

        /// <summary>
        /// The contract products
        /// </summary>
        public static List<ContractProductModel> ContractProducts;

        /// <summary>
        /// The analysis logger
        /// </summary>
        private readonly ILoggingDatabaseContext LoggingDatabase;

        /// <summary>
        /// The device
        /// </summary>
        private readonly IDevice Device;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="App" /> class.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="loggingDatabase">The logging database.</param>
        public App(IDevice device,
                   ILoggingDatabaseContext loggingDatabase)
        {
            this.Device = device;
            this.LoggingDatabase = loggingDatabase;

            // Init the logging DB
            this.LoggingDatabase.InitialiseDatabase();

            this.InitializeComponent();

            SyncfusionLicenseProvider.RegisterLicense("MjcxMTQzQDMxMzgyZTMxMmUzMFJmTmpPay9hVk9xRlRoRDA4YjhhTmxzTldnOUc2UURBOGQ0S3NpR3BENEk9");

            App.Container = Bootstrapper.Run();

            App.Container.RegisterInstance(this.Device, new ContainerControlledLifetimeManager());
            App.Container.RegisterInstance(this.LoggingDatabase, new ContainerControlledLifetimeManager());
            
            if (App.Configuration == null)
            {
                Task.Run(async () =>
                         {
                             // TODO: Logging
                             Console.WriteLine("Config is null");

                             IConfigurationServiceClient configurationServiceClient = App.Container.Resolve<IConfigurationServiceClient>();

                             App.Configuration = await configurationServiceClient.GetConfiguration(this.Device.GetDeviceIdentifier(), CancellationToken.None);

                             // TODO: Logging
                             Console.WriteLine("Config retrieved");
                         });
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Application developers override this method to perform actions when the application resumes from a sleeping state.
        /// </summary>
        /// <remarks>
        /// To be added.
        /// </remarks>
        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        /// <summary>
        /// Application developers override this method to perform actions when the application enters the sleeping state.
        /// </summary>
        /// <remarks>
        /// To be added.
        /// </remarks>
        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        /// <summary>
        /// Application developers override this method to perform actions when the application starts.
        /// </summary>
        /// <remarks>
        /// To be added.
        /// </remarks>
        protected override async void OnStart()
        {
            // TODO: Logging
            Console.WriteLine("In On Start");
            
            // Handle when your app starts
            ILoginPresenter loginPresenter = App.Container.Resolve<ILoginPresenter>();
            
            // show the login page
            await loginPresenter.Start();
        }

        #endregion
    }
}