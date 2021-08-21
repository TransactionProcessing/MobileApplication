using System;
using System.Collections.Generic;
using System.Text;

namespace TransactionMobile.IntegrationTests.WithAppium.Steps
{
    using System.Linq;
    using System.Threading.Tasks;
    using Common;
    using Pages;
    using Shouldly;
    using TechTalk.SpecFlow;

    [Binding]
    [Scope(Tag = "safaricomtopup")]
    public class SafaricomTopupSteps
    {
        MainPage MainPage = new MainPage();
        TransactionsPage TransactionsPage = new TransactionsPage();
        MobileTopupSelectOperatorPage SelectOperatorPage = new MobileTopupSelectOperatorPage();
        MobileTopupSelectProductPage SelectProductPage = new MobileTopupSelectProductPage();
        MobileTopupPerformTopupPage MobileTopupPerformTopupPage = new MobileTopupPerformTopupPage();
        MobileTopupSuccessPage MobileTopupSuccessPage = new MobileTopupSuccessPage();
        MobileTopupFailedPage MobileTopupFailedPage = new MobileTopupFailedPage();

        [Given(@"I tap on the Transactions button")]
        public async Task GivenITapOnTheTransactionsButton()
        {
            await this.MainPage.ClickTransactionsButton();
        }

        [Then(@"the Transactions Page is displayed")]
        public async Task ThenTheTransactionsPageIsDisplayed()
        {
            await this.TransactionsPage.AssertOnPage();
        }

        [Given(@"I tap on the Mobile Topup button")]
        public async Task GivenITapOnTheMobileTopupButton()
        {
            await this.TransactionsPage.ClickMobileTopupButton();
        }

        [Then(@"the Mobile Topup Select Operator Page is displayed")]
        public async Task ThenTheMobileTopupSelectOperatorPageIsDisplayed()
        {
            await this.SelectOperatorPage.AssertOnPage();
        }

        [Then(@"the Mobile Topup Select Product Page is displayed")]
        public async Task ThenTheMobileTopupSelectProductPageIsDisplayed()
        {
            await this.SelectProductPage.AssertOnPage();
        }

        [Given(@"I tap on the Safaricom button")]
        public async Task GivenITapOnTheSafaricomButton()
        {
            await this.SelectOperatorPage.ClickSafaricomOperatorButton();
        }

        [Given(@"I tap on the Custom button")]
        public async Task GivenITapOnTheCustomButton()
        {
            await this.SelectProductPage.ClickCustomProductButton();
        }

        [Then(@"the Mobile Topup Topup Details Page is displayed")]
        public async Task ThenTheMobileTopupTopupDetailsPageIsDisplayed()
        {
            await this.MobileTopupPerformTopupPage.AssertOnPage();
        }

        [When(@"I enter the following topup details")]
        public async Task WhenIEnterTheFollowingTopupDetails(Table table)
        {
            table.Rows.ShouldHaveSingleItem();
            TableRow topupDetails = table.Rows.Single();
            String customerMobileNumber = SpecflowTableHelper.GetStringRowValue(topupDetails, "CustomerMobileNumber");
            Decimal topupAmount = SpecflowTableHelper.GetDecimalValue(topupDetails, "TopupAmount");
            String customerEmailAddress = SpecflowTableHelper.GetStringRowValue(topupDetails, "CustomerEmailAddress");

            await this.MobileTopupPerformTopupPage.EnterCustomerMobileNumber(customerMobileNumber);
            await this.MobileTopupPerformTopupPage.EnterTopupAmount(topupAmount);
            await this.MobileTopupPerformTopupPage.EnterCustomerEmailAddress(customerEmailAddress);
        }

        [When(@"I click the back button")]
        public void WhenIClickTheBackButton()
        {
            //AppManager.App.Back();
        }

        [When(@"I tap on Perform Topup")]
        public async Task WhenITapOnPerformTopup()
        {
            await this.MobileTopupPerformTopupPage.ClickPerformTopupButton();
        }

        [Then(@"The Topup Validation Error will be displayed")]
        public async Task ThenTheTopupValidationErrorWillBeDisplayed()
        {
            this.MobileTopupPerformTopupPage.AssertTopupValidationErrorDisplayed();
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
