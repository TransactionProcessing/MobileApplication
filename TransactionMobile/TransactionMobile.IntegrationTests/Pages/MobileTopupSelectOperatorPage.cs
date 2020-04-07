namespace TransactionMobile.IntegrationTests.Pages
{
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
}