using System;
using System.Collections.Generic;
using System.Text;

namespace TransactionMobile.IntegrationTests.WithAppium.Pages
{
    using System.Threading.Tasks;

    public class AdminPage : BasePage
    {
        private readonly String ReconciliationButton;

        #region Properties

        /// <summary>
        /// Gets the trait.
        /// </summary>
        /// <value>
        /// The trait.
        /// </value>
        protected override String Trait => "AdminLabel";

        #endregion

        public AdminPage()
        {
            this.ReconciliationButton = "ReconciliationButton";
        }

        public async Task ClickReconciliationButton()
        {
            var element = await app.WaitForElementByAccessibilityId(this.ReconciliationButton);
            element.Click();
        }

        public async Task CheckReconciliationSuccessMessageToastIsDisplayed()
        {
            await app.WaitForToastMessage("Reconciliation completed, totals reset!");
        }
    }
}
