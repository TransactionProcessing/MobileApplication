using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionMobile.IntegrationTests.Pages
{
    using Common;
    using Xamarin.UITest.Queries;
    using Query = System.Func<Xamarin.UITest.Queries.AppQuery, Xamarin.UITest.Queries.AppQuery>;

    public class TransactionsPage : BasePage
    {
        #region Fields

        private readonly Query MobileTopupButton;
        private readonly Query MobileWalletButton;
        private readonly Query BillPaymentButton;
        private readonly Query AdminButton;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionsPage"/> class.
        /// </summary>
        public TransactionsPage()
        {
            this.MobileTopupButton = x => x.Marked("MobileTopupButton");
            this.MobileWalletButton = x => x.Marked("MobileWalletButton");
            this.BillPaymentButton = x => x.Marked("BillPaymentButton");
            this.AdminButton = x => x.Marked("AdminButton");
        }

        #endregion

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
                Android = x => x.Marked("Transactions"),
                iOS = x => x.Marked("Transactions")
            };

        #endregion

        public void ClickMobileTopupButton()
        {
            app.WaitForElement(this.MobileTopupButton);
            app.Tap(this.MobileTopupButton);
        }

        public void ClickMobileWalletButton()
        {
            app.WaitForElement(this.MobileWalletButton);
            app.Tap(this.MobileWalletButton);
        }

        public void ClickBillPaymentButton()
        {
            app.WaitForElement(this.BillPaymentButton);
            app.Tap(this.BillPaymentButton);
        }

        public void ClickAdminButton()
        {
            app.WaitForElement(this.AdminButton);
            app.Tap(this.AdminButton);
        }
    }
}
