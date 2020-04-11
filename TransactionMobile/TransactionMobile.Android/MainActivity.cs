namespace TransactionMobile.Droid
{
    using System;
    using System.Threading.Tasks;
    using Android.App;
    using Android.Content.PM;
    using Android.OS;
    using Android.Runtime;
    //using Com.Instabug.Library;
    //using Com.Instabug.Library.Invocation;
    //using Com.Instabug.Library.UI.Onboarding;
    using Common;
    using Java.Interop;
    using Microsoft.AppCenter.Crashes;
    using Plugin.Toast;
    using Unity;
    using Unity.Lifetime;
    using Xamarin.Forms;
    using Xamarin.Forms.Platform.Android;
    using Platform = Xamarin.Essentials.Platform;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Xamarin.Forms.Platform.Android.FormsAppCompatActivity" />
    [Activity(Label = "TransactionMobile",
              Icon = "@mipmap/icon",
              Theme = "@style/MainTheme",
              MainLauncher = true,
              ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : FormsAppCompatActivity
    {
        #region Fields

        /// <summary>
        /// The device
        /// </summary>
        private IDevice Device;

        /// <summary>
        /// The configuration
        /// </summary>
        private IConfiguration Configuration;

        #endregion

        #region Methods

        /// <summary>
        /// Called when [request permissions result].
        /// </summary>
        /// <param name="requestCode">The request code.</param>
        /// <param name="permissions">The permissions.</param>
        /// <param name="grantResults">The grant results.</param>
        public override void OnRequestPermissionsResult(Int32 requestCode,
                                                        String[] permissions,
                                                        [GeneratedEnum] Permission[] grantResults)
        {
            Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        /// <summary>
        /// Called when [create].
        /// </summary>
        /// <param name="savedInstanceState">State of the saved instance.</param>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            FormsAppCompatActivity.TabLayoutResource = Resource.Layout.Tabbar;
            FormsAppCompatActivity.ToolbarResource = Resource.Layout.Toolbar;

            this.Device = new AndroidDevice();

            base.OnCreate(savedInstanceState);

            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
            TaskScheduler.UnobservedTaskException += TaskSchedulerOnUnobservedTaskException;

            Platform.Init(this, savedInstanceState);
            Forms.Init(this, savedInstanceState);
            this.LoadApplication(new App(this.Device));
        }

        [Export("SetConfiguration")]
        public void SetConfiguration(String configuration)
        {
            var configItems = configuration.Split(',');
            var configurationObject = new Configuration
                                      {
                                          ClientId = configItems[0],
                                          ClientSecret = configItems[1],
                                          SecurityService = configItems[2],
                                          TransactionProcessorACL = configItems[3]
                                      };

            App.Configuration = configurationObject;
        }

        [Export("GetDebugInformation")]
        public String GetDebugInformation()
        {
            return this.Device.DebugInformation;
        }

        #endregion

        private static void TaskSchedulerOnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs unobservedTaskExceptionEventArgs)
        {
            Exception newExc = new Exception("TaskSchedulerOnUnobservedTaskException", unobservedTaskExceptionEventArgs.Exception);
            LogUnhandledException(newExc);
        }

        private static void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs unhandledExceptionEventArgs)
        {
            Exception newExc = new Exception("CurrentDomainOnUnhandledException", unhandledExceptionEventArgs.ExceptionObject as Exception);
            LogUnhandledException(newExc);
        }

        internal static void LogUnhandledException(Exception exception)
        {
            try
            {
                Crashes.TrackError(exception);
            }
            catch
            {
                // just suppress any error logging exceptions
            }
        }
    }
}