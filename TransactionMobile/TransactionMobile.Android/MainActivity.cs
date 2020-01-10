namespace TransactionMobile.Droid
{
    using System;
    using Android.App;
    using Android.Content.PM;
    using Android.OS;
    using Android.Runtime;
    using Com.Instabug.Library;
    using Com.Instabug.Library.Invocation;
    using Com.Instabug.Library.UI.Onboarding;
    using Common;
    using Java.Interop;
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

            new Instabug.Builder(this.Application, "8b2be04a38a0948fa3af42dec91a4685")
                .SetInvocationEvents(InstabugInvocationEvent.FloatingButton, InstabugInvocationEvent.Shake)
                .Build();

            Instabug.SetWelcomeMessageState(WelcomeMessage.State.Disabled);

            base.OnCreate(savedInstanceState);

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

        

        #endregion
    }
}