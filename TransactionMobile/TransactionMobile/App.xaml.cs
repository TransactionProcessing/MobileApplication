﻿namespace TransactionMobile
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Clients;
    using Common;
    using Database;
    using Microsoft.AppCenter;
    using Microsoft.AppCenter.Distribute;
    using Models;
    using Newtonsoft.Json;
    using Plugin.Toast;
    using Plugin.Toast.Abstractions;
    using Presenters;
    using SecurityService.DataTransferObjects.Responses;
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
        /// The is integration test mode
        /// </summary>
        public static Boolean IsIntegrationTestMode;

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
        /// The transaction number
        /// </summary>
        private static Int32 TransactionNumber;

        /// <summary>
        /// The contract products
        /// </summary>
        public static List<ContractProductModel> ContractProducts;

        /// <summary>
        /// The database
        /// </summary>
        private readonly IDatabaseContext Database;

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
        /// <param name="database">The database.</param>
        public App(IDevice device,
                   IDatabaseContext database)
        {
            this.Device = device;
            this.Database = database;

            // Init the logging DB
            this.Database.InitialiseDatabase();

            this.InitializeComponent();

            SyncfusionLicenseProvider.RegisterLicense("MzcxNTgwQDMxMzgyZTM0MmUzMEpNaXFsaFQ5bFErcVord05LSkVUeVd0MWRyaVE0NllvcFJmRzVZTWVTNlE9");

            App.Container = Bootstrapper.Run();

            App.Container.RegisterInstance(this.Database, new ContainerControlledLifetimeManager());
            App.Container.RegisterInstance(this.Device, new ContainerControlledLifetimeManager());
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
            if (App.Configuration != null)
            {
                if (App.Configuration.EnableAutoUpdates)
                {
                    Distribute.SetEnabledAsync(true).Wait();
                    Distribute.CheckForUpdate();
                }
                else
                {
                    Distribute.DisableAutomaticCheckForUpdate();
                }

                Distribute.ReleaseAvailable = OnReleaseAvailable;
                Distribute.UpdateTrack = UpdateTrack.Public;

                AppCenter.Start("android=10210e06-8a11-422b-b005-14081dc56375;", typeof(Distribute));
            }

            App.TransactionNumber = 1;

            // Handle when your app starts
            ILoginPresenter loginPresenter = App.Container.Resolve<ILoginPresenter>();

            // show the login page
            await loginPresenter.Start();
        }

        bool OnReleaseAvailable(ReleaseDetails releaseDetails)
        {
            // Look at releaseDetails public properties to get version information, release notes text or release notes URL
            string versionName = releaseDetails.ShortVersion;
            string versionCodeOrBuildNumber = releaseDetails.Version;
            string releaseNotes = releaseDetails.ReleaseNotes;
            Uri releaseNotesUrl = releaseDetails.ReleaseNotesUrl;

            // custom dialog
            var title = "Version " + versionName + " available!";
            Task answer;

            // On mandatory update, user can't postpone
            if (releaseDetails.MandatoryUpdate)
            {
                answer = App.Current.MainPage.DisplayAlert(title, releaseNotes, "Download and Install");
            }
            else
            {
                answer = App.Current.MainPage.DisplayAlert(title, releaseNotes, "Download and Install", "Later");
            }
            answer.ContinueWith((task) =>
            {
                // If mandatory or if answer was positive
                if (releaseDetails.MandatoryUpdate || (task as Task<bool>).Result)
                {
                    // Notify SDK that user selected update
                    Distribute.NotifyUpdateAction(UpdateAction.Update);
                }
                else
                {
                    // Notify SDK that user selected postpone (for 1 day)
                    // This method call is ignored by the SDK if the update is mandatory
                    Distribute.NotifyUpdateAction(UpdateAction.Postpone);
                }
            });

            // Return true if you're using your own dialog, false otherwise
            return true;
        }

        public static Int32 GetNextTransactionNumber()
        {
            return App.TransactionNumber;
        }

        public static void IncrementTransactionNumber()
        {
            Interlocked.Increment(ref App.TransactionNumber);
        }

        #endregion
    }
}