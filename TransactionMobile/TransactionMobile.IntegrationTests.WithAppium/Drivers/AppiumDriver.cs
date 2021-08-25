using System;
using System.Text;

namespace TransactionMobile.IntegrationTests.WithAppium.Drivers
{
    using System.IO;
    using System.Reflection;
    using Common;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Appium;
    using OpenQA.Selenium.Appium.Android;
    using OpenQA.Selenium.Appium.Enums;
    using OpenQA.Selenium.Appium.iOS;
    using OpenQA.Selenium.Appium.Service;
    using OpenQA.Selenium.Remote;

    public class AppiumDriver
    {
        public static MobileTestPlatform MobileTestPlatform;

        public static AndroidDriver<AndroidElement> AndroidDriver;

        public static IOSDriver<IOSElement> iOSDriver;

        public AppiumDriver()
        {
        }

        public void StartApp()
        {
            AppiumLocalService appiumService = new AppiumServiceBuilder().UsingPort(4723).Build();

            if (appiumService.IsRunning == false)
            {
                appiumService.Start();

                //Console.WriteLine($"appiumService.IsRunning - {appiumService.IsRunning}");
            }

            appiumService.OutputDataReceived += AppiumService_OutputDataReceived;

            if (AppiumDriver.MobileTestPlatform == MobileTestPlatform.Android)
            {

                var driverOptions = new AppiumOptions();
                driverOptions.AddAdditionalCapability("adbExecTimeout", TimeSpan.FromMinutes(5).Milliseconds);
                driverOptions.AddAdditionalCapability(MobileCapabilityType.AutomationName, "Espresso");
                // TODO: Only do this locally
                //driverOptions.AddAdditionalCapability("forceEspressoRebuild", true);
                driverOptions.AddAdditionalCapability("enforceAppInstall", true);
                driverOptions.AddAdditionalCapability(MobileCapabilityType.PlatformName, "Android");
                driverOptions.AddAdditionalCapability(MobileCapabilityType.PlatformVersion, "9.0");
                driverOptions.AddAdditionalCapability(MobileCapabilityType.DeviceName, "emulator-5554");

                String assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                String binariesFolder = Path.Combine(assemblyFolder, "..", "..", "..", "..", @"TransactionMobile.Android/bin/Release");
                var apkPath = Path.Combine(binariesFolder, "com.transactionprocessing.transactionmobile.apk");
                driverOptions.AddAdditionalCapability(MobileCapabilityType.App, apkPath);
                driverOptions.AddAdditionalCapability("espressoBuildConfig",
                                                      "{ \"additionalAppDependencies\": [ \"com.google.android.material:material:1.0.0\", \"androidx.lifecycle:lifecycle-extensions:2.1.0\" ] }");

                //Driver = new AndroidDriver<AndroidElement>(new Uri("http://127.0.0.1:4723/wd/hub"), driverOptions, TimeSpan.FromMinutes(5));


                AppiumDriver.AndroidDriver = new AndroidDriver<AndroidElement>(appiumService, driverOptions, TimeSpan.FromMinutes(5));
            }

            if (AppiumDriver.MobileTestPlatform == MobileTestPlatform.iOS)
            {
                var driverOptions = new AppiumOptions();
                driverOptions.AddAdditionalCapability(MobileCapabilityType.PlatformName, "iOS");
                driverOptions.AddAdditionalCapability(MobileCapabilityType.DeviceName, "iPhone Simulator");
                driverOptions.AddAdditionalCapability(MobileCapabilityType.PlatformVersion, "14.4");

                String assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                String binariesFolder = Path.Combine(assemblyFolder, "..", "..", "..", "..", @"TransactionMobile.iOS/bin/iPhoneSimulator/Release");
                var apkPath = Path.Combine(binariesFolder, "TransactionMobile.iOS.app");
                driverOptions.AddAdditionalCapability(MobileCapabilityType.App, apkPath);
                driverOptions.AddAdditionalCapability(MobileCapabilityType.NoReset, true);
                driverOptions.AddAdditionalCapability(MobileCapabilityType.AutomationName, "XCUITest");
                driverOptions.AddAdditionalCapability("useNewWDA", false);
                driverOptions.AddAdditionalCapability("wdaStartupRetries", "4");
                driverOptions.AddAdditionalCapability("iosInstallPause", "8000");
                driverOptions.AddAdditionalCapability("wdaStartupRetryInterval", "20000");
                driverOptions.AddAdditionalCapability("showXcodeLog", true);

                AppiumDriver.iOSDriver = new IOSDriver<IOSElement>(appiumService, driverOptions, TimeSpan.FromMinutes(5));
            }
        }

        private void AppiumService_OutputDataReceived(object sender, System.Diagnostics.DataReceivedEventArgs e)
        {
            //Console.WriteLine(e.Data);
        }

        public void StopApp()
        {
            if (AppiumDriver.MobileTestPlatform == MobileTestPlatform.Android)
            {
                AppiumDriver.AndroidDriver.Quit();
            }
            else if (AppiumDriver.MobileTestPlatform == MobileTestPlatform.iOS)
            {
                AppiumDriver.iOSDriver.Quit();
            }
        }
    }
}
