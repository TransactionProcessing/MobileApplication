namespace TransactionMobile.IntegrationTests.WithAppium.Pages.MobileTopup
{
    using System;
    using System.Threading.Tasks;
    using Drivers;
    using OpenQA.Selenium;
    using Shouldly;

    public class MobileTopupPerformTopupPage : BasePage
    {
        #region Fields

        private readonly String CustomerMobileNumberEntry;
        private readonly String CustomerEmailAddressEntry;
        private readonly String TopupAmountEntry;
        private readonly String PerformTopupButton;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MobileTopupPerformTopupPage"/> class.
        /// </summary>
        public MobileTopupPerformTopupPage()
        {
            this.CustomerMobileNumberEntry = "CustomerMobileNumberEntry";
            this.CustomerEmailAddressEntry = "CustomerEmailAddressEntry";
            this.TopupAmountEntry = "TopupAmountEntry";
            this.PerformTopupButton = "PerformTopupButton";
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the trait.
        /// </summary>
        /// <value>
        /// The trait.
        /// </value>
        protected override String Trait => "TopupDetailsLabel";

        #endregion

        public async Task EnterCustomerMobileNumber(String customerMobileNumber)
        {
            var element = await this.app.WaitForElementByAccessibilityId(this.CustomerMobileNumberEntry);

            element.SendKeys(customerMobileNumber);
        }

        public async Task EnterCustomerEmailAddress(String customerEmailAddress)
        {
            var element = await this.app.WaitForElementByAccessibilityId(this.CustomerEmailAddressEntry);

            element.SendKeys(customerEmailAddress);
        }

        public async Task EnterTopupAmount(Decimal topupAmount)
        {
            var element = await this.app.WaitForElementByAccessibilityId(this.TopupAmountEntry);

            element.SendKeys(topupAmount.ToString());
        }

        public async Task ClickPerformTopupButton()
        {
            this.app.HideKeyboard();
            var element = await this.app.WaitForElementByAccessibilityId(this.PerformTopupButton);

            element.Click();
        }

        public void AssertTopupValidationErrorDisplayed()
        {
            this.app.HideKeyboard();
            String errorMessage = "Please enter a mobile number and Topup Amount to continue";
            var alertElement = this.app.GetAlert();
            alertElement.ShouldNotBeNull();
            alertElement.Text.ShouldBe(errorMessage);
            IAlert alert = null;
            Should.NotThrow(() =>
                            {
                                alert = AppiumDriver.Driver.SwitchTo().Alert();
                                alert.ShouldNotBeNull();
                                alert.Accept();
                            });
        }
    }
}