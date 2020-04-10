namespace TransactionMobile.IntegrationTests
{
    using System;
    using System.Linq;
    using Pages;
    using System.Threading.Tasks;
    using Common;
    using Shouldly;
    using TechTalk.SpecFlow;

    [Binding]
    [Scope(Tag = "safaricomtopup")]
    public class SafaricomTopupSteps
    {
        MainPage MainPage = new MainPage();
        TransactionsPage TransactionsPage = new TransactionsPage();
        MobileTopupSelectOperatorPage SelectOperatorPage = new MobileTopupSelectOperatorPage();
        MobileTopupPerformTopupPage MobileTopupPerformTopupPage = new MobileTopupPerformTopupPage();
        MobileTopupSuccessPage MobileTopupSuccessPage = new MobileTopupSuccessPage();
        MobileTopupFailedPage MobileTopupFailedPage = new MobileTopupFailedPage();

        [Given(@"I tap on the Transactions button")]
        public void GivenITapOnTheTransactionsButton()
        {
            this.MainPage.ClickTransactionsButton();
        }

        [Then(@"the Transactions Page is displayed")]
        public async Task ThenTheTransactionsPageIsDisplayed()
        {
            await this.TransactionsPage.AssertOnPage();
        }

        [Given(@"I tap on the Mobile Topup button")]
        public void GivenITapOnTheMobileTopupButton()
        {
            this.TransactionsPage.ClickMobileTopupButton();
        }

        [Then(@"the Mobile Topup Select Operator Page is displayed")]
        public async Task ThenTheMobileTopupSelectOperatorPageIsDisplayed()
        {
            await this.SelectOperatorPage.AssertOnPage();
        }

        [Given(@"I tap on the Safaricom button")]
        public void GivenITapOnTheSafaricomButton()
        {
            this.SelectOperatorPage.ClickSafaricomOperatorButton();
        }

        [Then(@"the Mobile Topup Topup Details Page is displayed")]
        public async Task ThenTheMobileTopupTopupDetailsPageIsDisplayed()
        {
            await this.MobileTopupPerformTopupPage.AssertOnPage();
        }

        [When(@"I enter the following topup details")]
        public void WhenIEnterTheFollowingTopupDetails(Table table)
        {
            table.Rows.ShouldHaveSingleItem();
            TableRow topupDetails = table.Rows.Single();
            String customerMobileNumber = SpecflowTableHelper.GetStringRowValue(topupDetails, "CustomerMobileNumber");
            Decimal topupAmount = SpecflowTableHelper.GetDecimalValue(topupDetails, "TopupAmount");
            
            this.MobileTopupPerformTopupPage.EnterCustomerMobileNumber(customerMobileNumber);
            this.MobileTopupPerformTopupPage.EnterTopupAmount(topupAmount);
        }

        [When(@"I tap on Perform Topup")]
        public void WhenITapOnPerformTopup()
        {
            this.MobileTopupPerformTopupPage.ClickPerformTopupButton();
        }

        [Then(@"The Topup Successful Screen will be displayed")]
        public async Task ThenTheTopupSuccessfulScreenWillBeDisplayed()
        {
            await this.MobileTopupSuccessPage.AssertOnPage();
        }

        [Then(@"The Topup Failed Screen will be displayed")]
        public async Task ThenTheTopupFailedScreenWillBeDisplayed()
        {
            await this.MobileTopupFailedPage.AssertOnPage();
        }


    }
}