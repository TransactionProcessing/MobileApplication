using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace TransactionMobile.IntegrationTests.Common
{
    using NUnit.Framework;

    static class AppManager
    {
        static IApp app;
        public static IApp App
        {
            get
            {
                if (app == null)
                    throw new NullReferenceException("'AppManager.App' not set. Call 'AppManager.StartApp()' before trying to access it.");
                return app;
            }
        }

        static Platform? platform;
        public static Platform Platform
        {
            get
            {
                if (platform == null)
                    throw new NullReferenceException("'AppManager.Platform' not set.");
                return platform.Value;
            }

            set
            {
                platform = value;
            }
        }

        public static void SetConfiguration(String clientId, String clientSecret, String securityServiceUri, String transactionProcessorAclUrl)
        {
            if (AppManager.platform == Platform.Android)
            {
                String configuration = $"{clientId},{clientSecret},{securityServiceUri},{transactionProcessorAclUrl}";
                AppManager.app.Invoke("SetConfiguration", configuration);
            }
            else if(AppManager.platform == Platform.iOS)
            {
                AppManager.app.Invoke("SetClientId:", clientId );
                AppManager.app.Invoke("SetClientSecret:", clientSecret );
                AppManager.app.Invoke("SetSecurityServiceUrl:", securityServiceUri );
                AppManager.app.Invoke("SetTransactionProcessorAclUrl:", transactionProcessorAclUrl);
            }
        }

        public static void StartApp()
        {
            string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            //string binariesFolder = Path.Combine(assemblyFolder, "..", "..", "..", "Binaries");

            if (Platform == Platform.Android)
            {
                String binariesFolder = Path.Combine(assemblyFolder, "..", "..", "..", @"TransactionMobile.Android/bin/Release");
                app = ConfigureApp
                      .Android
                      // Used to run a .apk file:
                      .ApkFile(Path.Combine(binariesFolder, "com.transactionprocessing.transactionmobile-Signed.apk"))
                      .StartApp();
            }

            if (Platform == Platform.iOS)
            {
                String binariesFolder = Path.Combine(assemblyFolder, "..", "..", "..", @"TransactionMobile.iOS/bin/iPhoneSimulator/Release");
                app = ConfigureApp
                      .iOS
                      // Used to run a .app file on an ios simulator:
                      .AppBundle(Path.Combine(binariesFolder, "TransactionMobile.iOS.app"))
                      // Used to run a .ipa file on a physical ios device:
                      //.InstalledApp("com.company.bundleid")
                      .StartApp();
            }
        }
    }

    public class PlatformQuery
    {
        public Func<AppQuery, AppQuery> Android
        {
            set
            {
                if (AppManager.Platform == Platform.Android)
                    current = value;
            }
        }

        public Func<AppQuery, AppQuery> iOS
        {
            set
            {
                if (AppManager.Platform == Platform.iOS)
                    current = value;
            }
        }

        Func<AppQuery, AppQuery> current;
        public Func<AppQuery, AppQuery> Current
        {
            get
            {
                if (current == null)
                    throw new NullReferenceException("Trait not set for current platform");

                return current;
            }
        }
    }

    public abstract class BasePage
    {
        protected IApp app => AppManager.App;
        protected bool OnAndroid => AppManager.Platform == Platform.Android;
        protected bool OniOS => AppManager.Platform == Platform.iOS;
        protected abstract PlatformQuery Trait { get; }

        protected BasePage()
        {
        }

        /// <summary>
        /// Verifies that the trait is present. Uses the default wait time.
        /// </summary>
        /// <param name="timeout">Time to wait before the assertion fails</param>
        public void AssertOnPage(TimeSpan? timeout = default(TimeSpan?))
        {
            var message = "Unable to verify on page: " + this.GetType().Name;
            
            Assert.DoesNotThrow(() => app.WaitForElement(Trait.Current, timeout: timeout), message);
        }

        /// <summary>
        /// Verifies that the trait is no longer present. Defaults to a 5 second wait.
        /// </summary>
        /// <param name="timeout">Time to wait before the assertion fails</param>
        public void WaitForPageToLeave(TimeSpan? timeout = default(TimeSpan?))
        {
            timeout = timeout ?? TimeSpan.FromSeconds(5);
            var message = "Unable to verify *not* on page: " + this.GetType().Name;

            Assert.DoesNotThrow(() => app.WaitForNoElement(Trait.Current, timeout: timeout), message);
        }
    }

    public abstract class BaseTestFixture
    {
        protected BaseTestFixture(Platform platform)
        {
            AppManager.Platform = platform;
        }

        [SetUp]
        public virtual void BeforeEachTest()
        {
            AppManager.StartApp();
        }
    }
}
