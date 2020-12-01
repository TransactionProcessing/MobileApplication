using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Query = System.Func<Xamarin.UITest.Queries.AppQuery, Xamarin.UITest.Queries.AppQuery>;

namespace TransactionMobile.IntegrationTests.Pages
{
    using Common;

    public class AdminPage : BasePage
    {
        private readonly Query ReconciliationButton;

        #region Properties

        /// <summary>
        /// Gets the trait.
        /// </summary>
        /// <value>
        /// The trait.
        /// </value>
        protected override PlatformQuery Trait =>
            new PlatformQuery
            {
                Android = x => x.Marked("Admin"),
                iOS = x => x.Marked("Admin")
            };

        #endregion

        public AdminPage()
        {
            this.ReconciliationButton = x => x.Marked("Reconciliation");
        }

        public void ClickReconciliationButton()
        {
            app.WaitForElement(this.ReconciliationButton);
            app.Tap(this.ReconciliationButton);
        }

        public void CheckReconciliationSuccessMessageToastIsDisplayed()
        {
            app.WaitForElement("Reconciliation completed, totals reset!", timeout:TimeSpan.FromSeconds(5));
        }
    }
}
