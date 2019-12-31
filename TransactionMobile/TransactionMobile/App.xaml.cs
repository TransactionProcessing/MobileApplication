using Xamarin.Forms;

namespace TransactionMobile
{
    using System;
    using Common;
    using Pages;
    using Presenters;
    using Unity;
    using Unity.Lifetime;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Xamarin.Forms.Application" />
    public partial class App : Application
    {
        /// <summary>
        /// Unity container
        /// </summary>
        public static IUnityContainer Container;


        /// <summary>
        /// The device
        /// </summary>
        private readonly IDevice Device;

        /// <summary>
        /// Initializes a new instance of the <see cref="App"/> class.
        /// </summary>
        /// <param name="device">The device.</param>
        public App(IDevice device)
        {
            this.Device = device;
            InitializeComponent();

            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MTkwMDU4QDMxMzcyZTM0MmUzMENHeFNiTzVPdWtQdk5td09TY0RjRVhUQ1hORFF5cFFKMW5QdnN0RDRLMGc9");

            App.Container = Bootstrapper.Run();

            App.Container.RegisterInstance(this.Device, new ContainerControlledLifetimeManager());
        }

        /// <summary>
        /// Application developers override this method to perform actions when the application starts.
        /// </summary>
        /// <remarks>
        /// To be added.
        /// </remarks>
        protected override async void OnStart()
        {
            // Handle when your app starts
           ILoginPresenter loginPresenter = App.Container.Resolve<ILoginPresenter>();

            // show the login page
            await loginPresenter.Start();
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
        /// Application developers override this method to perform actions when the application resumes from a sleeping state.
        /// </summary>
        /// <remarks>
        /// To be added.
        /// </remarks>
        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
