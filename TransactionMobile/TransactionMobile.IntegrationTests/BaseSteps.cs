using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionMobile.IntegrationTests
{
    using NUnit.Framework;
    using TechTalk.SpecFlow;
    using Xamarin.UITest;
    using Xamarin.UITest.Configuration;

    //[Binding]
    //[Scope(Tag = "base")]
    //public class BaseSteps
    //{
    //    private readonly Platform Platform;

    //    private readonly FeatureContext FeatureContext;

    //    private readonly ScenarioContext ScenarioContext;

    //    private IApp App;

    //    public BaseSteps(Platform platform,
    //                     FeatureContext featureContext,
    //                     ScenarioContext scenarioContext)
    //    {
    //        this.Platform = platform;
    //        this.FeatureContext = featureContext;
    //        this.ScenarioContext = scenarioContext;
    //    }

    //    [BeforeScenario]
    //    public void BeforeScenario()
    //    {
    //        // Read the Is CI value from the TestContext
    //        Boolean.TryParse(TestContext.Parameters["IsCI"], out Boolean isCI);

    //        if (this.Platform == Platform.Android)
    //        {
    //            String appname = "com.transactionprocessing.transactionmobile";

    //            // Setup Android App
    //            if (isCI)
    //            {
    //                String apkFile = $"{appname}-Signed.apk";
    //                this.App = ConfigureApp.Android.ApkFile(apkFile).DeviceSerial("emulator-5554").EnableLocalScreenshots().StartApp(AppDataMode.Clear);
    //            }
    //            else
    //            {
    //                this.App = ConfigureApp.Android.InstalledApp(appname).DeviceSerial("emulator-5554").EnableLocalScreenshots().StartApp(AppDataMode.Clear);
    //            }
    //        }
    //        else
    //        {
    //            // Setup IOS App
    //        }


    //        //this.ScenarioContext.Add("App", this.App);
    //    }
    //}
}
