namespace TransactionMobile.iOS
{
    using System;
    using System.Threading.Tasks;
    using Common;
    using Foundation;
    using Microsoft.AppCenter.Crashes;
    //using InstabugLib;
    using Syncfusion.XForms.iOS.Border;
    using Syncfusion.XForms.iOS.Buttons;
    using Syncfusion.XForms.iOS.TabView;
    using UIKit;
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
        /// The device
        /// </summary>
        private IDevice Device;

        /// <summary>
        /// The configuration
        /// </summary>
        private IConfiguration Configuration;

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
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
            TaskScheduler.UnobservedTaskException += TaskSchedulerOnUnobservedTaskException;

            this.Device = new iOSDevice();

            Forms.Init();

            Calabash.Start();

            SfBorderRenderer.Init();
            SfButtonRenderer.Init();
            SfTabViewRenderer.Init();
            
            this.LoadApplication(new App(this.Device));

            return base.FinishedLaunching(app, options);
        }

        private static void TaskSchedulerOnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs unobservedTaskExceptionEventArgs)
        {
            var newExc = new Exception("TaskSchedulerOnUnobservedTaskException", unobservedTaskExceptionEventArgs.Exception);
            LogUnhandledException(newExc);
        }

        private static void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs unhandledExceptionEventArgs)
        {
            var newExc = new Exception("CurrentDomainOnUnhandledException", unhandledExceptionEventArgs.ExceptionObject as Exception);
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

        [Export("SetConfiguration:")]
        public void SetConfiguration(NSString configuration)
        {
            String[] configItems = configuration.ToString().Split(',');
            Configuration configurationObject = new Configuration
                                                {
                                                    ClientId = configItems[0],
                                                    ClientSecret = configItems[1],
                                                    SecurityService = configItems[2],
                                                    TransactionProcessorACL = configItems[3]
                                                };

            App.Configuration = configurationObject;
        }
        
        #endregion
    }
}