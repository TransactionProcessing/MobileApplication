namespace TransactionMobile.IntegrationTests.WithAppium.Pages
{
    using System;
    using System.Reactive.Subjects;
    using System.Threading.Tasks;
    using Drivers;
    using OpenQA.Selenium.Appium;
    using OpenQA.Selenium.Appium.Android;
    using Syncfusion.ListView.XForms;

    public class ReportingPage : BasePage
    {
        #region Fields

        private readonly String MySettlementsButton;
        private readonly String MyTransactionsButton;
        private readonly String MyBalanceHistoryButton;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionsPage"/> class.
        /// </summary>
        public ReportingPage()
        {
            this.MySettlementsButton = "ViewMySettlementsButton";
            this.MyTransactionsButton= "ViewMyTransactionsButton";
            this.MyBalanceHistoryButton = "ViewMyBalanceHistoryButton";
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the trait.
        /// </summary>
        /// <value>
        /// The trait.
        /// </value>
        protected override String Trait => "ReportingLabel";

        #endregion

        public async Task ClickMySettlementsButton()
        {
            var element = await this.WaitForElementByAccessibilityId(this.MySettlementsButton);
            element.Click();
        }

        public async Task ClickMyTransactionsButton()
        {
            var element = await this.WaitForElementByAccessibilityId(this.MyTransactionsButton);
            element.Click();
        }

        public async Task ClickMyBalanceHistoryButton()
        {
            var element = await this.WaitForElementByAccessibilityId(this.MyBalanceHistoryButton);
            element.Click();
        }
    }
}