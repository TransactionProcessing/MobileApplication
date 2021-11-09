namespace TransactionMobile.IntegrationTests.WithAppium.Pages.MobileTopup
{
    using System;
    using System.Threading.Tasks;

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
            var element = await this.WaitForElementByAccessibilityId(this.SafaricomOperatorButton);
            element.Click();
        }
    }
}