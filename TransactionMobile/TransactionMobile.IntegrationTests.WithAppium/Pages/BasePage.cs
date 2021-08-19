namespace TransactionMobile.IntegrationTests.WithAppium.Pages
{
    using System;
    using System.Threading.Tasks;
    using Drivers;
    using OpenQA.Selenium.Appium.Android;
    using Shouldly;
    using Steps;

    public abstract class BasePage
    {
        protected AndroidDriver<AndroidElement> app => AppiumDriver.Driver;

        protected abstract String Trait { get; }

        public async Task AssertOnPage(TimeSpan? timeout = null)
        {
            timeout = timeout ?? TimeSpan.FromSeconds(60);

            await Retry.For(async () =>
                            {
                                String message = "Unable to verify on page: " + this.GetType().Name;

                                Should.NotThrow(() =>this.app.WaitForElementByAccessibilityId(this.Trait), message);
                            },
                            TimeSpan.FromMinutes(1),
                            timeout).ConfigureAwait(false);

        }

        /// <summary>
        /// Verifies that the trait is no longer present. Defaults to a 5 second wait.
        /// </summary>
        /// <param name="timeout">Time to wait before the assertion fails</param>
        public void WaitForPageToLeave(TimeSpan? timeout = null)
        {
            timeout = timeout ?? TimeSpan.FromSeconds(5);
            var message = "Unable to verify *not* on page: " + this.GetType().Name;

            Should.NotThrow(() => this.app.WaitForNoElementByAccessibilityId(this.Trait), message);
        }
    }
}