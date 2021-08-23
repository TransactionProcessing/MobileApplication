namespace TransactionMobile.IntegrationTests.WithAppium.Pages.MobileTopup
{
    using System;
    using System.Threading.Tasks;

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
            var element = await this.WaitForElementByAccessibilityId(this.KES100ProductButton);
            element.Click();
        }

        public async Task ClickCustomProductButton()
        {
            var element = await this.WaitForElementByAccessibilityId(this.CustomProductButton);
            element.Click();
        }
    }
}