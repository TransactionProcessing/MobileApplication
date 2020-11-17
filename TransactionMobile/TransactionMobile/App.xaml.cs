namespace TransactionMobile
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Net.Http;
    using System.Threading;
    using Common;
    using Events;
    using Models;
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
        private readonly IAnalysisLogger AnalysisLogger;

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
        /// <param name="analysisLogger">The analysis logger.</param>
        public App(IDevice device,
                   IAnalysisLogger analysisLogger)
        {
            this.Device = device;
            this.AnalysisLogger = analysisLogger;

            this.InitializeComponent();

            SyncfusionLicenseProvider.RegisterLicense("MjcxMTQzQDMxMzgyZTMxMmUzMFJmTmpPay9hVk9xRlRoRDA4YjhhTmxzTldnOUc2UURBOGQ0S3NpR3BENEk9");

            App.Container = Bootstrapper.Run();

            App.Container.RegisterInstance(this.Device, new ContainerControlledLifetimeManager());
            App.Container.RegisterInstance(this.AnalysisLogger, new ContainerControlledLifetimeManager());
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
            this.AnalysisLogger.Initialise(true, true, "e5e42a79-6306-4795-a4e1-4988146ec234");
            
            // Handle when your app starts
            ILoginPresenter loginPresenter = App.Container.Resolve<ILoginPresenter>();
            
            // show the login page
            await loginPresenter.Start();

            IConfigurationServiceClient configurationServiceClient = App.Container.Resolve<IConfigurationServiceClient>();

            App.Configuration = await configurationServiceClient.GetConfiguration(this.Device.GetDeviceIdentifier(), CancellationToken.None);
            
        }

        #endregion
    }
}