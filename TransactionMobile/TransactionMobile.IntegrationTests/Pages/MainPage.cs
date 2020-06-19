namespace TransactionMobile.IntegrationTests.Pages
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Common;
    using Shouldly;
    using Xamarin.UITest.Queries;
    using Query = System.Func<Xamarin.UITest.Queries.AppQuery, Xamarin.UITest.Queries.AppQuery>;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="TransactionMobile.IntegrationTests.Common.BasePage" />
    public class MainPage : BasePage
    {
        #region Fields

        private readonly Query TransactionsButton;
        private readonly Query ReportsButton;
        private readonly Query ProfileButton;
        private readonly Query SupportButton;
        private readonly Query AvailableBalanceLabel;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MainPage"/> class.
        /// </summary>
        public MainPage()
        {
            this.TransactionsButton = x => x.Marked("TransactionsButton");
            this.ReportsButton = x => x.Marked("ReportsButton");
            this.ProfileButton = x => x.Marked("ProfileButton");
            this.SupportButton = x => x.Marked("SupportButton");
            this.AvailableBalanceLabel = x => x.Marked("AvailableBalanceLabel");
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
                Android = x => x.Marked("Main Menu"),
                iOS = x => x.Marked("Main Menu")
            };

        #endregion

        public void ClickTransactionsButton()
        {
            app.WaitForElement(this.TransactionsButton);
            app.Tap(this.TransactionsButton);
        }

        public void ClickReportsButton()
        {
            app.WaitForElement(this.ReportsButton);
            app.Tap(this.ReportsButton);
        }

        public void ClickProfileButton()
        {
            app.WaitForElement(this.ProfileButton);
            app.Tap(this.ProfileButton);
        }

        public void ClickSupportButton()
        {
            app.WaitForElement(this.SupportButton);
            app.Tap(this.SupportButton);
        }

        public async Task<Decimal> GetAvailableBalanceValue(TimeSpan? timeout = default(TimeSpan?))
        {
            AppResult label = null;

            app.WaitForElement(this.AvailableBalanceLabel, timeout: timeout);

            AppResult[] queryResults = app.Query(this.AvailableBalanceLabel);
            label = queryResults.SingleOrDefault();

            label.ShouldNotBeNull();

            String availableBalanceText = label.Text.Replace(" KES", String.Empty);

            if (Decimal.TryParse(availableBalanceText, out Decimal balanceValue) == false)
            {
                // TODO: Throw some error
            }

            //await Task.Delay(TimeSpan.FromSeconds(60));

            //AppManager.App.Print.Tree();

            return balanceValue;
        }
    }
}