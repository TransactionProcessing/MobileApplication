using System;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using TransactionMobile.IntegrationTests.WithAppium.Pages;

namespace TransactionMobile.IntegrationTests.WithAppium
{
    using Common;
    using Drivers;

    [Binding]
    public class MySettlementsSteps
    {
        MainPage MainPage = new MainPage();
        ReportingPage ReportingPage = new ReportingPage();
        MySettlementsListPage MySettlementsListPage = new MySettlementsListPage();
        MySettlementAnalysisPage MySettlementAnalysisPage = new MySettlementAnalysisPage();

        [Given(@"I tap on the Reporting button")]
        public async Task GivenITapOnTheReportingButton()
        {
            await this.MainPage.ClickReportsButton();
        }

        [Then(@"the Reporting Page is displayed")]
        public async Task ThenTheReportingPageIsDisplayed()
        {
            await this.ReportingPage.AssertOnPage();
        }

        [Given(@"I tap on the My Settlements button")]
        public async Task GivenITapOnTheMySettlementsButton()
        {
            await this.ReportingPage.ClickMySettlementsButton();
        }

        [Then(@"the My Settlements List Page is displayed")]
        public async void ThenTheMySettlementsListPageIsDisplayed()
        {
            await this.MySettlementsListPage.AssertOnPage();
        }

        [Then(@"the following entries are displayed")]
        public async Task ThenTheFollowingEntriesAreDisplayed(Table table)
        {
            var source = AppiumDriver.AndroidDriver.PageSource;

            foreach (TableRow tableRow in table.Rows)
            {
                //| SettlementDate | NumberFeesSettled | ValueOfFeesSettled |
                String settlementDateString = SpecflowTableHelper.GetStringRowValue(tableRow, "SettlementDate");
                DateTime settlementDate = SpecflowTableHelper.GetDateForDateString(settlementDateString, DateTime.Today);
                var numberFeesSettled = SpecflowTableHelper.GetIntValue(tableRow, "NumberFeesSettled");
                var valueOfFeesSettled = SpecflowTableHelper.GetDecimalValue(tableRow, "ValueOfFeesSettled");

                await this.MySettlementsListPage.SettlementListEntryExists(settlementDate, numberFeesSettled, valueOfFeesSettled);
            }
        }

        [When(@"I select the settlement from '([^']*)'")]
        public async Task WhenISelectTheSettlementFrom(String dateString)
        {
            DateTime settlementDate = SpecflowTableHelper.GetDateForDateString(dateString, DateTime.Today);

            await this.MySettlementsListPage.ClickOnSettlementItem(settlementDate);
        }

        [Then(@"the Settlement Analysis Page is displayed")]
        public async Task ThenTheSettlementAnalysisPageIsDisplayed()
        {
            await this.MySettlementAnalysisPage.AssertOnPage();
        }

    }
}
