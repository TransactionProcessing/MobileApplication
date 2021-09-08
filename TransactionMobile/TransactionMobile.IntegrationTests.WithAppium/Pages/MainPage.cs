using System;
using System.Text;

namespace TransactionMobile.IntegrationTests.WithAppium.Pages
{
    using System.Drawing;
    using System.Threading.Tasks;
    using Features;
    using Microsoft.VisualStudio.TestPlatform.ObjectModel;
    using OpenQA.Selenium.Appium;
    using OpenQA.Selenium.Appium.Android.UiAutomator;
    using OpenQA.Selenium.Appium.Interactions;
    using OpenQA.Selenium.Appium.Interfaces;
    using OpenQA.Selenium.Appium.MultiTouch;
    using OpenQA.Selenium.Interactions;
    using OpenQA.Selenium.Remote;
    using PointerInputDevice = OpenQA.Selenium.Interactions.PointerInputDevice;

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
            this.TransactionsButton = "TransactionsButton";
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
            //await this.ScrollTo(this.Trait, this.AvailableBalanceLabel);
            var element = await this.WaitForElementByAccessibilityId(this.AvailableBalanceLabel, timeout:TimeSpan.FromSeconds(30));

            String availableBalanceText = element.Text.Replace(" KES", String.Empty);

            if (Decimal.TryParse(availableBalanceText, out Decimal balanceValue) == false)
            {
                throw new Exception($"Failed to parse [{availableBalanceText}] as a Decimal");
            }

            return balanceValue;
        }
    }
}
