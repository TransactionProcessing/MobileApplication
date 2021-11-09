namespace TransactionMobile.IntegrationTests.WithAppium.Pages
{
    using System;
    using System.Threading.Tasks;
    using OpenQA.Selenium;
    using Shouldly;
    using Steps;

    public class MySettlementsListPage : BasePage
    {
        #region Fields

        private readonly String SettlementList;

        private readonly String SettlementListItem;

        private readonly String SettlementListItemNumberOfFeesSettled;

        private readonly String SettlementListItemSettlementDate;

        private readonly String SettlementListItemValueOfFeesSettled;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionsPage"/> class.
        /// </summary>
        public MySettlementsListPage()
        {
            this.SettlementList = "SettlementList";
            this.SettlementListItem = "SettlementListItem{0}";
            this.SettlementListItemSettlementDate = "SettlementListItem{0}SettlementDate";
            this.SettlementListItemNumberOfFeesSettled = "SettlementListItem{0}NumberOfFeesSettled";
            this.SettlementListItemValueOfFeesSettled = "SettlementListItem{0}Value";
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the trait.
        /// </summary>
        /// <value>
        /// The trait.
        /// </value>
        protected override String Trait => "MySettlementsLabel";

        #endregion

        #region Methods

        public async Task ClickOnSettlementItem(DateTime settlementDate)
        {
            var settlementList = await this.WaitForElementByAccessibilityId(this.SettlementList);
            settlementList.ShouldNotBeNull();

            // Now find the correct item
            var item = await this.WaitForElementByAccessibilityId(string.Format(this.SettlementListItem, settlementDate.ToString("yyyyMMdd")));
            item.ShouldNotBeNull();
            item.Click();
        }

        public async Task SettlementListEntryExists(DateTime settlementDate,
                                                    Int32 numberOfSettledFees,
                                                    Decimal valueOfSettledFees)
        {
            IWebElement settlementList = null;
            IWebElement item = null;
            await Retry.For(async () =>
                            {
                                settlementList = await this.WaitForElementByAccessibilityId(this.SettlementList);
                                settlementList.ShouldNotBeNull();

                                // Now find the correct item
                                var index = settlementDate.ToString("yyyyMMdd");
                                item = await this.WaitForElementByAccessibilityId(string.Format(this.SettlementListItem, index));
                                item.ShouldNotBeNull();

                                var settlementDateLabel = await this.WaitForElementByAccessibilityId(string.Format(this.SettlementListItemSettlementDate, index));
                                settlementDateLabel.ShouldNotBeNull();
                                settlementDateLabel.Text.ShouldBe(settlementDate.ToString("dd MMM yyyy"));

                                var numberOfFeesLabel = await this.WaitForElementByAccessibilityId(string.Format(this.SettlementListItemNumberOfFeesSettled, index));
                                numberOfFeesLabel.ShouldNotBeNull();
                                numberOfFeesLabel.Text.ShouldBe(numberOfSettledFees.ToString());

                                var valueOfFeesLabel = await this.WaitForElementByAccessibilityId(string.Format(this.SettlementListItemValueOfFeesSettled, index));
                                valueOfFeesLabel.ShouldNotBeNull();
                                valueOfFeesLabel.Text.ShouldBe($"Value: {valueOfSettledFees} KES");
                            });
        }

        #endregion
    }
}