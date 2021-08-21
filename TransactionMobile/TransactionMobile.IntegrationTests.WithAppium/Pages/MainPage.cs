using System;
using System.Collections.Generic;
using System.Text;

namespace TransactionMobile.IntegrationTests.WithAppium.Pages
{
    using System.Threading.Tasks;
    using Microsoft.VisualStudio.TestPlatform.ObjectModel;
    using OpenQA.Selenium.Appium.Android;
    using OpenQA.Selenium.Appium.Android.UiAutomator;
    using OpenQA.Selenium.Appium.Interfaces;
    using Shouldly;
    using Steps;
    using Xunit;

    public class MainPage : BasePage
    {
        protected override String Trait => "HomeLabel";
        private readonly String TransactionsButton;
        private readonly String ReportsButton;
        private readonly String ProfileButton;
        private readonly String SupportButton;
        private readonly String AvailableBalanceLabel;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainPage"/> class.
        /// </summary>
        public MainPage()
        {
            this.TransactionsButton ="TransactionsButton";
            this.ReportsButton = "ReportsButton";
            this.ProfileButton = "ProfileButton";
            this.SupportButton = "SupportButton";
            this.AvailableBalanceLabel = "AvailableBalanceValueLabel";
        }

        public async Task ClickTransactionsButton()
        {
            var element = await app.WaitForElementByAccessibilityId(this.TransactionsButton);
            element.Click();
        }

        public void ClickReportsButton()
        {
            //app.WaitForElement(this.ReportsButton);
            //app.Tap(this.ReportsButton);
        }

        public void ClickProfileButton()
        {
            //app.WaitForElement(this.ProfileButton);
            //app.Tap(this.ProfileButton);
        }

        public void ClickSupportButton()
        {
            //app.WaitForElement(this.SupportButton);
            //app.Tap(this.SupportButton);
        }

        public async Task<Decimal> GetAvailableBalanceValue(TimeSpan? timeout = default(TimeSpan?))
        {
            //AppResult label = null;
            //app.ScrollDownTo(this.AvailableBalanceLabel);
            var element = await app.WaitForElementByAccessibilityId(this.AvailableBalanceLabel, timeout: TimeSpan.FromSeconds(30));
            
            String availableBalanceText = element.Text.Replace(" KES", String.Empty);

            if (Decimal.TryParse(availableBalanceText, out Decimal balanceValue) == false)
            {
                throw new Exception($"Failed to parse [{availableBalanceText}] as a Decimal");
            }

            return balanceValue;
        }
    }

    public static class Extenstion
    {
        public static async Task<AndroidElement> WaitForElementByAccessibilityId(this AndroidDriver<AndroidElement> driver, String selector, TimeSpan? timeout = null)
        {
            timeout ??= TimeSpan.FromSeconds(60);
            AndroidElement element = null;
            await Retry.For(async () =>
                            {
                                element = driver.FindElementByAccessibilityId(selector);
                                element.ShouldNotBeNull();
                            });

            return element;

        }

        public static async Task WaitForNoElementByAccessibilityId(this AndroidDriver<AndroidElement> driver, String selector, TimeSpan? timeout = null)
        {
            timeout ??= TimeSpan.FromSeconds(60);

            await Retry.For(async () =>
                            {
                                var element = driver.FindElementByAccessibilityId(selector);
                                element.ShouldBeNull();
                            });

        }
    }
}
