namespace TransactionMobile.iOS
{
    using System;
    using Common;
    using Foundation;
    using InstabugLib;
    using Syncfusion.XForms.iOS.Border;
    using Syncfusion.XForms.iOS.Buttons;
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
            this.Device = new iOSDevice();

            Instabug.StartWithToken("8b2be04a38a0948fa3af42dec91a4685", IBGInvocationEvent.Shake);

            Forms.Init();

            Calabash.Start();

            SfBorderRenderer.Init();
            SfButtonRenderer.Init();
            this.LoadApplication(new App(this.Device));

            return base.FinishedLaunching(app, options);
        }

        [Export("SetConfiguration:")]
        public void SetConfiguration(NSString configuration)
        {
            var configItems = configuration.ToString().Split(',');
            var configurationObject = new Configuration
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