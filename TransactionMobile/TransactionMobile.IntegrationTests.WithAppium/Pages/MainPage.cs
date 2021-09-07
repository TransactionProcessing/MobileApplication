using System;
using System.Collections.Generic;
using System.Text;

namespace TransactionMobile.IntegrationTests.WithAppium.Pages
{
    using System.Threading;
    using System.Threading.Tasks;
    using Features;
    using Microsoft.VisualStudio.TestPlatform.ObjectModel;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Appium;
    using OpenQA.Selenium.Appium.Android;
    using OpenQA.Selenium.Appium.Android.UiAutomator;
    using OpenQA.Selenium.Appium.Interfaces;
    using OpenQA.Selenium.Appium.iOS;
    using OpenQA.Selenium.Interactions;
    using OpenQA.Selenium.Remote;
    using Shouldly;
    using Steps;

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
            var element = await this.WaitForElementByAccessibilityId(this.TransactionsButton);
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
            var element = await this.WaitForElementByAccessibilityId(this.AvailableBalanceLabel, timeout: TimeSpan.FromSeconds(30));
            
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
        // TODO: May need a platform switch
        public static AndroidElement GetAlert(this AndroidDriver<AndroidElement> driver)
        {
            return driver.FindElementByClassName("androidx.appcompat.widget.AppCompatTextView");
        }

        public static async Task<IWebElement> WaitForElementByAccessibilityId(this AndroidDriver<AndroidElement> driver, String selector, TimeSpan? timeout = null)
        {
            timeout ??= TimeSpan.FromSeconds(60);
            IWebElement element = null;
            await Retry.For(async () =>
                            {
                                element = driver.FindElementByAccessibilityId(selector);

                                TouchActions action = new TouchActions((AppiumDriver<AndroidElement>)driver);
                                action.Scroll(element, 10, 100);
                                action.Perform();

                                element.ShouldNotBeNull();
                            });

            return element;

        }

        public static async Task<IWebElement> WaitForElementByAccessibilityId(this IOSDriver<IOSElement> driver, String selector, TimeSpan? timeout = null)
        {
            timeout ??= TimeSpan.FromSeconds(60);
            IWebElement element = null;
            await Retry.For(async () =>
                            {
                                element = driver.FindElementByAccessibilityId(selector);

                                TouchActions action = new TouchActions((AppiumDriver<IOSElement>)driver);
                                action.Scroll(element, 10, 100);
                                action.Perform();

                                element.ShouldNotBeNull();
                            });

            return element;

        }

        public static async Task WaitForNoElementByAccessibilityId(this IOSDriver<IOSElement> driver, String selector, TimeSpan? timeout = null)
        {
            timeout ??= TimeSpan.FromSeconds(60);

            await Retry.For(async () =>
                            {
                                var element = driver.FindElementByAccessibilityId(selector);
                                element.ShouldBeNull();
                            });

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

        public static async Task WaitForToastMessage(this AndroidDriver<AndroidElement> driver,
                                                     String expectedToast)
        {
            await Retry.For(async () =>
                            {
                                var args = new Dictionary<string, object>
                                           {
                                               {"text", expectedToast},
                                               {"isRegexp", false}
                                           };
                                driver.ExecuteScript("mobile: isToastVisible", args);

                            });
        }

        public static async Task WaitForToastMessage(this IOSDriver<IOSElement> driver,
                                                     String expectedToast)
        {
            Boolean isDisplayed = false;
            int count = 0;
            do
            {
                if (driver.PageSource.Contains(expectedToast))
                {
                    Console.WriteLine(driver.PageSource);
                    isDisplayed = true;
                    break;
                }
                Thread.Sleep(200);//Add your custom wait if exists
                count++;

            } while (count < 10);

            Console.WriteLine(driver.PageSource);
            isDisplayed.ShouldBeTrue();
        }

        public static async Task<String> GetPageSource(this AndroidDriver<AndroidElement> driver)
        {
            return driver.PageSource;
        }

        public static async Task<String> GetPageSource(this IOSDriver<IOSElement> driver)
        {
            return driver.PageSource;
        }
    }
}
