using System;
using System.Collections.Generic;
using System.Text;

namespace TransactionMobile.IntegrationTests.WithAppium.Pages
{
    using System.Threading.Tasks;
    using Drivers;
    using OpenQA.Selenium;
    using Shouldly;

    public class MobileTopupSelectOperatorPage : BasePage
    {
        #region Fields

        private readonly String SafaricomOperatorButton;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MobileTopupSelectOperatorPage"/> class.
        /// </summary>
        public MobileTopupSelectOperatorPage()
        {
            this.SafaricomOperatorButton = "Safaricom";
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the trait.
        /// </summary>
        /// <value>
        /// The trait.
        /// </value>
        protected override String Trait => "SelectOperatorLabel";

        #endregion

        public async Task ClickSafaricomOperatorButton()
        {
            var element = await this.app.WaitForElementByAccessibilityId(this.SafaricomOperatorButton);
            element.Click();
        }
    }
    public class MobileTopupSelectProductPage : BasePage
    {
        #region Fields

        private readonly String CustomProductButton;
        private readonly String KES100ProductButton;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MobileTopupSelectProductPage"/> class.
        /// </summary>
        public MobileTopupSelectProductPage()
        {
            this.CustomProductButton = "Custom";
            this.KES100ProductButton = "100 KES";
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the trait.
        /// </summary>
        /// <value>
        /// The trait.
        /// </value>
        protected override String Trait => "SelectProductLabel";

        #endregion

        public async Task ClickKES100ProductButton()
        {
            var element = await this.app.WaitForElementByAccessibilityId(this.KES100ProductButton);
            element.Click();
        }

        public async Task ClickCustomProductButton()
        {
            var element = await this.app.WaitForElementByAccessibilityId(this.CustomProductButton);
            element.Click();
        }
    }

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
            var element = await app.WaitForElementByAccessibilityId(this.CustomerMobileNumberEntry);

            element.SendKeys(customerMobileNumber);
        }

        public async Task EnterCustomerEmailAddress(String customerEmailAddress)
        {
            var element = await app.WaitForElementByAccessibilityId(this.CustomerEmailAddressEntry);

            element.SendKeys(customerEmailAddress);
        }

        public async Task EnterTopupAmount(Decimal topupAmount)
        {
            var element = await app.WaitForElementByAccessibilityId(this.TopupAmountEntry);

            element.SendKeys(topupAmount.ToString());
        }

        public async Task ClickPerformTopupButton()
        {
            app.HideKeyboard();
            var element = await app.WaitForElementByAccessibilityId(this.PerformTopupButton);

            element.Click();
        }

        public void AssertTopupValidationErrorDisplayed()
        {
            app.HideKeyboard();
            String errorMessage = "Please enter a mobile number and Topup Amount to continue";
            
            IAlert alert = null;
            Should.NotThrow(() =>
                            {
                                alert = AppiumDriver.Driver.SwitchTo().Alert();
                            });

            alert.ShouldNotBeNull();
            alert.Text.ShouldBe(errorMessage);
            alert.Accept();
        }
    }

    public class MobileTopupSuccessPage : BasePage
    {
        protected override String Trait => "MobileTopupSuccessful";
    }

    public class MobileTopupFailedPage : BasePage
    {
        protected override String Trait => "MobileTopupFailed";
    }
}
