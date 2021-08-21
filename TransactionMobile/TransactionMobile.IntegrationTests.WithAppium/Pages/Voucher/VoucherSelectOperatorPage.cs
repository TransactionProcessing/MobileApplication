namespace TransactionMobile.IntegrationTests.WithAppium.Pages.Voucher
{
    using System;
    using System.Threading.Tasks;

    public class VoucherSelectOperatorPage : BasePage
    {
        #region Fields

        private readonly String HealthcareCentre1OperatorButton;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MobileTopupSelectOperatorPage"/> class.
        /// </summary>
        public VoucherSelectOperatorPage()
        {
            this.HealthcareCentre1OperatorButton = "Healthcare Centre 1";
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

        public async Task ClickHealthcareCentre1OperatorButton()
        {
            var element = await this.app.WaitForElementByAccessibilityId(this.HealthcareCentre1OperatorButton);

            element.Click();
        }
    }
}