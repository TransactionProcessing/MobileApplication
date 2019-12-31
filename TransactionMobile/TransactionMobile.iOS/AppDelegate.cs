using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;

namespace TransactionMobile.iOS
{
    using Common;
    using InstabugLib;

    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            this.Device = new iOSDevice();

            Instabug.StartWithToken("8b2be04a38a0948fa3af42dec91a4685",
                                    IBGInvocationEvent.Shake);
            
            global::Xamarin.Forms.Forms.Init();
            LoadApplication(new App(this.Device));
            
            return base.FinishedLaunching(app, options);
        }

        private IDevice Device;
    }
}
