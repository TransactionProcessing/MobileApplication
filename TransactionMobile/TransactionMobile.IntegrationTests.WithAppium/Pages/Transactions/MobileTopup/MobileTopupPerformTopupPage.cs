﻿namespace TransactionMobile.IntegrationTests.WithAppium.Pages.MobileTopup
{
    using System;
    using System.Threading.Tasks;
    using Common;
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
            this.HideKeyboard();
            var element = await this.WaitForElementByAccessibilityId(this.CustomerMobileNumberEntry);

            element.SendKeys(customerMobileNumber);
        }

        public async Task EnterCustomerEmailAddress(String customerEmailAddress)
        {
            this.HideKeyboard();
            var element = await this.WaitForElementByAccessibilityId(this.CustomerEmailAddressEntry);

            element.SendKeys(customerEmailAddress);
        }

        public async Task EnterTopupAmount(Decimal topupAmount)
        {
            this.HideKeyboard();
            var element = await this.WaitForElementByAccessibilityId(this.TopupAmountEntry);

            element.SendKeys(topupAmount.ToString());
        }

        public async Task ClickPerformTopupButton()
        {
            this.HideKeyboard();
            var element = await this.WaitForElementByAccessibilityId(this.PerformTopupButton);

            element.Click();
        }

        public void AssertTopupValidationErrorDisplayed()
        {
            this.HideKeyboard();
            String errorMessage = "Please enter a mobile number and Topup Amount to continue";
            var alertElement = this.GetAlert();
            alertElement.ShouldNotBeNull();
            
            if (AppiumDriver.MobileTestPlatform == MobileTestPlatform.iOS)
            {
                alertElement.Click();
            }
            else
            {
                alertElement.Text.ShouldBe(errorMessage);
                IAlert alert = null;
                Should.NotThrow(() =>
                                {
                                    alert = this.SwitchToAlert();
                                    alert.ShouldNotBeNull();
                                    alert.Accept();
                                });
            }
        }
    }
}