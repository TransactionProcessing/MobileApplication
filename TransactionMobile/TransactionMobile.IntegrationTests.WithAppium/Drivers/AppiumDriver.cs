using System;
using System.Text;

namespace TransactionMobile.IntegrationTests.WithAppium.Drivers
{
    using System.IO;
    using System.Reflection;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Appium;
    using OpenQA.Selenium.Appium.Android;
    using OpenQA.Selenium.Appium.Enums;
    using OpenQA.Selenium.Appium.Service;

    public class AppiumDriver
    {
        public static AndroidDriver<AndroidElement> Driver { get; private set; }

        public AppiumDriver()
        {
        }

        public void StartApp()
        {
            var driverOptions = new AppiumOptions();
            driverOptions.AddAdditionalCapability("adbExecTimeout", TimeSpan.FromMinutes(5).Milliseconds);
            driverOptions.AddAdditionalCapability(MobileCapabilityType.AutomationName, "Espresso");
            // TODO: Only do this locally
            driverOptions.AddAdditionalCapability("forceEspressoRebuild", true);
            driverOptions.AddAdditionalCapability("enforceAppInstall", true);
            driverOptions.AddAdditionalCapability(MobileCapabilityType.PlatformName, "Android");
            driverOptions.AddAdditionalCapability(MobileCapabilityType.PlatformVersion, "9.0");
            driverOptions.AddAdditionalCapability(MobileCapabilityType.DeviceName, "emulator-5554");
            
            String assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            String binariesFolder = Path.Combine(assemblyFolder, "..","..", "..", "..",@"TransactionMobile.Android/bin/Release");
            var apkPath = Path.Combine(binariesFolder, "com.transactionprocessing.transactionmobile.apk");
            driverOptions.AddAdditionalCapability(MobileCapabilityType.App, apkPath);
            driverOptions.AddAdditionalCapability("espressoBuildConfig", "{ \"additionalAppDependencies\": [ \"com.google.android.material:material:1.0.0\", \"androidx.lifecycle:lifecycle-extensions:2.1.0\" ] }");

            //driverOptions.AddAdditionalCapability(AndroidMobileCapabilityType.AppPackage, "com.transactionprocessing.transactionmobile");
            //driverOptions.AddAdditionalCapability(AndroidMobileCapabilityType.AppActivity, "crc64a6ab01768311dfa8.MainActivity");

            //Driver = new AndroidDriver<AndroidElement>(new Uri("http://127.0.0.1:4723/wd/hub"), driverOptions, TimeSpan.FromMinutes(5));
            AppiumLocalService appiumService = new AppiumServiceBuilder().UsingPort(4723).Build();

            if (appiumService.IsRunning == false)
            {
                appiumService.Start();

                Console.WriteLine($"appiumService.IsRunning - {appiumService.IsRunning}");
            }
            appiumService.OutputDataReceived += AppiumService_OutputDataReceived;

            Driver = new AndroidDriver<AndroidElement>(appiumService, driverOptions, TimeSpan.FromMinutes(5));
        }

        private void AppiumService_OutputDataReceived(object sender, System.Diagnostics.DataReceivedEventArgs e)
        {
            Console.WriteLine(e.Data);
        }

        public void StopApp()
        {
            Driver.Quit();
        }
    }
}
