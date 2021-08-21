namespace TransactionMobile.IntegrationTests.WithAppium.Pages.Voucher
{
    using System;
    using System.Threading.Tasks;
    using Drivers;
    using MobileTopup;
    using OpenQA.Selenium;
    using Shouldly;

    public class VoucherIssueVoucherPage : BasePage
    {
        #region Fields

        private readonly String RecipientMobileNumberEntry;
        private readonly String RecipientEmailAddressEntry;
        private readonly String CustomerEmailAddressEntry;
        private readonly String IssueVoucherButton;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MobileTopupPerformTopupPage"/> class.
        /// </summary>
        public VoucherIssueVoucherPage()
        {
            this.RecipientMobileNumberEntry = "RecipientMobileNumberEntry";
            this.RecipientEmailAddressEntry = "RecipientEmailAddressEntry";
            this.CustomerEmailAddressEntry = "CustomerEmailAddressEntry";
            this.IssueVoucherButton = "IssueVoucherButton";
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the trait.
        /// </summary>
        /// <value>
        /// The trait.
        /// </value>
        protected override String Trait => "VoucherIssueDetailsLabel";

        #endregion

        public async Task EnterRecipientMobileNumber(String recipientMobileNumber)
        {
            var element = await this.app.WaitForElementByAccessibilityId(this.RecipientMobileNumberEntry);

            element.SendKeys(recipientMobileNumber);
        }

        public async Task EnterRecipientEmailAddress(String recipientEmailAddress)
        {
            var element = await this.app.WaitForElementByAccessibilityId(this.RecipientEmailAddressEntry);

            element.SendKeys(recipientEmailAddress);
        }

        public async Task EnterCustomerEmailAddress(String customerEmailAddress)
        {
            var element = await this.app.WaitForElementByAccessibilityId(this.CustomerEmailAddressEntry);

            element.SendKeys(customerEmailAddress);
        }


        public async Task ClickIssueVoucherButton()
        {
            this.app.HideKeyboard();
            var element = await this.app.WaitForElementByAccessibilityId(this.IssueVoucherButton);

            element.Click();
        }

        public void AssertVoucherValidationErrorDisplayed()
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