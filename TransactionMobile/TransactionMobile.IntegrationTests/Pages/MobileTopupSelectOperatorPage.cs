namespace TransactionMobile.IntegrationTests.Pages
{
    using System;
    using Common;
    using Query = System.Func<Xamarin.UITest.Queries.AppQuery, Xamarin.UITest.Queries.AppQuery>;

    public class MobileTopupSelectOperatorPage : BasePage
    {
        #region Fields

        private readonly Query SafaricomOperatorButton;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MobileTopupSelectOperatorPage"/> class.
        /// </summary>
        public MobileTopupSelectOperatorPage()
        {
            this.SafaricomOperatorButton = x => x.Marked("Safaricom");
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
                Android = x => x.Marked("Select Operator"),
                iOS = x => x.Marked("Select Operator")
            };

        #endregion

        public void ClickSafaricomOperatorButton()
        {
            app.WaitForElement(this.SafaricomOperatorButton);
            app.Tap(this.SafaricomOperatorButton);
        }
    }

    public class MobileTopupSelectProductPage : BasePage
    {
        #region Fields

        private readonly Query CustomProductButton;
        private readonly Query KES100ProductButton;

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
            app.WaitForElement(this.KES100ProductButton);
            app.Tap(this.KES100ProductButton);
        }

        public void ClickCustomProductButton()
        {
            app.WaitForElement(this.CustomProductButton, timeout: TimeSpan.FromSeconds(45));
            app.Tap(this.CustomProductButton);
        }
    }
}