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
    using System.Threading;
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
                String configuration = $"{clientId},{clientSecret},{securityServiceUri},{transactionProcessorAclUrl}";
                AppManager.app.Invoke("SetConfiguration:", configuration);
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
                      .ApkFile(Path.Combine(binariesFolder, "com.transactionprocessing.transactionmobile.apk"))
                      //.Debug()
                      .StartApp();
            }

            if (Platform == Platform.iOS)
            {
                String binariesFolder = Path.Combine(assemblyFolder, "..", "..", "..", @"TransactionMobile.iOS/bin/iPhoneSimulator/Release");
                app = ConfigureApp
                      .iOS
                      // Used to run a .app file on an ios simulator:
                      .AppBundle(Path.Combine(binariesFolder, "TransactionMobile.iOS.app"))
                      .DeviceIdentifier("E01E8404-86E0-4E70-A910-99F936E44EE3") // iPhone 11
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
        public async Task AssertOnPage(TimeSpan? timeout = default(TimeSpan?))
        {
            await Retry.For(async () =>
                            {
                                var message = "Unable to verify on page: " + this.GetType().Name;

                                Assert.DoesNotThrow(() => app.WaitForElement(Trait.Current, timeout:timeout), message);
                            },
                            TimeSpan.FromMinutes(1),
                            timeout).ConfigureAwait(false);

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

    public static class Retry
    {
        #region Fields

        /// <summary>
        /// The default retry for
        /// </summary>
        private static readonly TimeSpan DefaultRetryFor = TimeSpan.FromSeconds(60);

        /// <summary>
        /// The default retry interval
        /// </summary>
        private static readonly TimeSpan DefaultRetryInterval = TimeSpan.FromSeconds(5);

        #endregion

        #region Methods

        /// <summary>
        /// Fors the specified action.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="retryFor">The retry for.</param>
        /// <param name="retryInterval">The retry interval.</param>
        /// <returns></returns>
        public static async Task For(Func<Task> action,
                                     TimeSpan? retryFor = null,
                                     TimeSpan? retryInterval = null)
        {
            DateTime startTime = DateTime.Now;
            Exception lastException = null;

            if (retryFor == null)
            {
                retryFor = Retry.DefaultRetryFor;
            }

            while (DateTime.Now.Subtract(startTime).TotalMilliseconds < retryFor.Value.TotalMilliseconds)
            {
                try
                {
                    await action().ConfigureAwait(false);
                    lastException = null;
                    break;
                }
                catch (Exception e)
                {
                    lastException = e;

                    // wait before retrying
                    Thread.Sleep(retryInterval ?? Retry.DefaultRetryInterval);
                }
            }

            if (lastException != null)
            {
                throw lastException;
            }
        }

        #endregion
    }
}
