namespace TransactionMobile.IntegrationTests.WithAppium.Pages
{
    using System;
    using System.Threading.Tasks;

    public class TransactionsPage : BasePage
    {
        #region Fields

        private readonly String MobileTopupButton;
        private readonly String MobileWalletButton;
        private readonly String BillPaymentButton;
        private readonly String VoucherButton;
        private readonly String AdminButton;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionsPage"/> class.
        /// </summary>
        public TransactionsPage()
        {
            this.MobileTopupButton = "MobileTopupButton";
            this.MobileWalletButton = "MobileWalletButton";
            this.BillPaymentButton = "BillPaymentButton";
            this.VoucherButton = "VoucherButton";
            this.AdminButton = "AdminButton";
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the trait.
        /// </summary>
        /// <value>
        /// The trait.
        /// </value>
        protected override String Trait => "TransactionsLabel";

        #endregion

        public async Task ClickMobileTopupButton()
        {
            var element = await this.WaitForElementByAccessibilityId(this.MobileTopupButton);
            element.Click();
        }

        public async Task ClickMobileWalletButton()
        {
            var element = await this.WaitForElementByAccessibilityId(this.MobileWalletButton);
            element.Click();
        }

        public async Task ClickBillPaymentButton()
        {
            var element = await this.WaitForElementByAccessibilityId(this.BillPaymentButton);
            element.Click();
        }

        public async Task ClickAdminButton()
        {
            var element = await this.WaitForElementByAccessibilityId(this.AdminButton);
            element.Click();
        }

        public async Task ClickVoucherButton()
        {
            var element = await this.WaitForElementByAccessibilityId(this.VoucherButton);
            element.Click();
        }
    }
}