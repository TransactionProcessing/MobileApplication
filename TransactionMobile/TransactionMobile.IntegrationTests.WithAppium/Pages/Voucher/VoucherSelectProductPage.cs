namespace TransactionMobile.IntegrationTests.WithAppium.Pages.Voucher
{
    using System;
    using System.Threading.Tasks;

    public class VoucherSelectProductPage : BasePage
    {
        #region Fields

        private readonly String KES10ProductButton;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionMobile.IntegrationTests.Pages.MobileTopupSelectProductPage"/> class.
        /// </summary>
        public VoucherSelectProductPage()
        {
            this.KES10ProductButton = "10 KES";
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

        public async Task ClickKES10ProductButton()
        {
            var element = await this.app.WaitForElementByAccessibilityId(this.KES10ProductButton);

            element.Click();
        }
    }
}
