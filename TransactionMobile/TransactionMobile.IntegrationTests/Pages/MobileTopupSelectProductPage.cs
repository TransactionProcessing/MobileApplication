namespace TransactionMobile.IntegrationTests.Pages
{
    using System;
    using Common;
    using Xamarin.UITest.Queries;

    public class MobileTopupSelectProductPage : BasePage
    {
        #region Fields

        private readonly Func<AppQuery, AppQuery> CustomProductButton;
        private readonly Func<AppQuery, AppQuery> KES100ProductButton;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MobileTopupSelectProductPage"/> class.
        /// </summary>
        public MobileTopupSelectProductPage()
        {
            this.CustomProductButton = x => x.Marked("Custom");
            this.KES100ProductButton = x => x.Marked("100 KES");
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
                Android = x => x.Marked("Select Product"),
                iOS = x => x.Marked("Select Product")
            };

        #endregion

        public void ClickKES100ProductButton()
        {
            this.app.WaitForElement(this.KES100ProductButton);
            this.app.Tap(this.KES100ProductButton);
        }

        public void ClickCustomProductButton()
        {
            this.app.WaitForElement(this.CustomProductButton, timeout:TimeSpan.FromSeconds(60));
            this.app.Tap(this.CustomProductButton);
        }
    }

    public class VoucherSelectProductPage : BasePage
    {
        #region Fields

        private readonly Func<AppQuery, AppQuery> KES10ProductButton;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionMobile.IntegrationTests.Pages.MobileTopupSelectProductPage"/> class.
        /// </summary>
        public VoucherSelectProductPage()
        {
            this.KES10ProductButton = x => x.Marked("10 KES");
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
                Android = x => x.Marked("Select Product"),
                iOS = x => x.Marked("Select Product")
            };

        #endregion

        public void ClickKES10ProductButton()
        {
            this.app.WaitForElement(this.KES10ProductButton);
            this.app.Tap(this.KES10ProductButton);
        }
    }
}