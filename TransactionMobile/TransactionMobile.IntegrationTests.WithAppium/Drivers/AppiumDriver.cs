using System;
using System.Collections.Generic;
using System.Text;

namespace TransactionMobile.IntegrationTests.WithAppium.Drivers
{
    using System.IO;
    using System.Reflection;
    using IntegrationTestClients;
    using Newtonsoft.Json;
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
            driverOptions.AddAdditionalCapability("adbExecTimeout", 180000);
            driverOptions.AddAdditionalCapability(MobileCapabilityType.AutomationName, "Espresso");
            driverOptions.AddAdditionalCapability("forceEspressoRebuild", true);
            driverOptions.AddAdditionalCapability("enforceAppInstall", true);
            driverOptions.AddAdditionalCapability(MobileCapabilityType.PlatformName, "Android");
            driverOptions.AddAdditionalCapability(MobileCapabilityType.PlatformVersion, "9.0");
            driverOptions.AddAdditionalCapability(MobileCapabilityType.DeviceName, "emulator-5554");
            
            String assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            String binariesFolder = Path.Combine(assemblyFolder, "..","..", "..", "..",@"TransactionMobile.Android/bin/Release");
            var apkPath = Path.Combine(binariesFolder, "com.transactionprocessing.transactionmobile.apk");
            driverOptions.AddAdditionalCapability(MobileCapabilityType.App, apkPath);

            //driverOptions.AddAdditionalCapability(AndroidMobileCapabilityType.AppPackage, "com.transactionprocessing.transactionmobile");
            //driverOptions.AddAdditionalCapability(AndroidMobileCapabilityType.AppActivity, "crc64a6ab01768311dfa8.MainActivity");

            Driver = new AndroidDriver<AndroidElement>(new Uri("http://127.0.0.1:4723/wd/hub"), driverOptions, TimeSpan.FromSeconds(180));
            //AppiumLocalService appiumService = new AppiumServiceBuilder().UsingAnyFreePort().Build();

            //if (appiumService.IsRunning == false)
            //{
            //    appiumService.Start();
            //}

            //Driver = new AndroidDriver<AndroidElement>(appiumService, driverOptions);
        }

        public void StopApp()
        {
            Driver.Quit();
        }
    }

    public class BackdoorDriver
    {
        private readonly AppiumDriver AppiumDriver;

        public BackdoorDriver(AppiumDriver appiumDriver)
        {
            AppiumDriver = appiumDriver;
        }

        public void SetIntegrationModeOn()
        {
            ExecuteBackdoor("SetIntegrationTestModeOn", "");
        }

        public void UpdateTestMerchant(Merchant merchant)
        {
            String merchantData = JsonConvert.SerializeObject(merchant);
            ExecuteBackdoor("UpdateTestMerchant", merchantData);
        }

        private void ExecuteBackdoor(string methodName, string value)
        {
            Dictionary<String, Object> args = CreateBackdoorArgs(methodName, value);
            
            AppiumDriver.Driver.ExecuteScript("mobile: backdoor", args);
        }

        private static Dictionary<string, object> CreateBackdoorArgs(string methodName, string value)
        {
            return new Dictionary<string, object>
                       {
                           {"target", "application"},
                           {
                               "methods", new List<Dictionary<string, object>>
                                          {
                                              new Dictionary<string, object>
                                              {
                                                  {"name", methodName},
                                                  {
                                                      "args", new List<Dictionary<string, object>>
                                                              {
                                                                  new Dictionary<string, object>
                                                                  {
                                                                      {"value", value},
                                                                      {"type", "String"}
                                                                  }
                                                              }
                                                  }
                                              }
                                          }
                           }
                       };
        }
    }
}
