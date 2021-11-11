namespace TransactionMobile.IntegrationTests.WithAppium.Pages
{
    using System;
    using System.Threading.Tasks;
    using Common;
    using Drivers;
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
            String stage = "";
            try
            {
                await Retry.For(async () =>
                                {
                                    stage = "1";
                                    settlementList = await this.WaitForElementByAccessibilityId(this.SettlementList);
                                    settlementList.ShouldNotBeNull();
                                    stage = "2";
                                    // Now find the correct item
                                    var index = settlementDate.ToString("yyyyMMdd");
                                    item = await this.WaitForElementByAccessibilityId(string.Format(this.SettlementListItem, index));
                                    item.ShouldNotBeNull();
                                    stage = "3";
                                    var settlementDateLabel = await this.WaitForElementByAccessibilityId(string.Format(this.SettlementListItemSettlementDate, index));
                                    settlementDateLabel.ShouldNotBeNull();
                                    settlementDateLabel.Text.ShouldBe(settlementDate.ToString("dd MMM yyyy"));
                                    stage = "4";
                                    var numberOfFeesLabel = await this.WaitForElementByAccessibilityId(string.Format(this.SettlementListItemNumberOfFeesSettled, index));
                                    numberOfFeesLabel.ShouldNotBeNull();
                                    numberOfFeesLabel.Text.ShouldBe(numberOfSettledFees.ToString());
                                    stage = "5";
                                    var valueOfFeesLabel = await this.WaitForElementByAccessibilityId(string.Format(this.SettlementListItemValueOfFeesSettled, index));
                                    valueOfFeesLabel.ShouldNotBeNull();
                                    valueOfFeesLabel.Text.ShouldBe($"Value: {valueOfSettledFees} KES");
                                    stage = "6";
                                });
            }
            catch(Exception ex)
            {
                String source = "";
                if (AppiumDriver.MobileTestPlatform == MobileTestPlatform.iOS)
                {
                    source = AppiumDriver.iOSDriver.PageSource;
                }
                else
                {
                    source = AppiumDriver.AndroidDriver.PageSource;
                }

                throw new Exception($"Element not found stage {stage}. source [{source}]");
            }
        }

        #endregion
    }
}