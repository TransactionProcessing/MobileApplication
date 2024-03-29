﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TransactionMobile.IntegrationTests.WithAppium.Hooks
{
    using Common;
    using TechTalk.SpecFlow;
    using TransactionMobile.IntegrationTests.WithAppium.Drivers;

    [Binding]
    public class AppiumHooks
    {
        private readonly AppiumDriver _appiumDriver;

        public AppiumHooks(AppiumDriver appiumDriver)
        {
            _appiumDriver = appiumDriver;
        }

        [BeforeScenario()]
        public void StartApp()
        {
            _appiumDriver.StartApp();
        }

        [AfterScenario()]
        public void ShutdownApp()
        {
            _appiumDriver.StopApp();
        }
    }
}
