using System;
using System.Collections.Generic;
using System.Text;

namespace TransactionMobile.IntegrationTests.WithAppium.Steps
{
    using System.Linq;
    using System.Threading.Tasks;
    using Common;
    using Pages;
    using Pages.Voucher;
    using TechTalk.SpecFlow;

    [Binding]
    [Scope(Tag = "voucherissue")]
    public class VoucherIssueSteps
    {
        MainPage MainPage = new MainPage();
        TransactionsPage TransactionsPage = new TransactionsPage();
        VoucherSelectOperatorPage SelectOperatorPage = new VoucherSelectOperatorPage();
        VoucherSelectProductPage SelectProductPage = new VoucherSelectProductPage();
        VoucherIssueVoucherPage IssueVoucherPage = new VoucherIssueVoucherPage();
        VoucherSuccessPage VoucherSuccessPage = new VoucherSuccessPage();
        VoucherFailedPage VoucherFailedPage = new VoucherFailedPage();

        [Given(@"I tap on the Transactions button")]
        public async Task GivenITapOnTheTransactionsButton()
        {
            await this.MainPage.ClickTransactionsButton();
        }

        [Given(@"I tap on the Voucher button")]
        public async Task GivenITapOnTheVoucherButton()
        {
            await this.TransactionsPage.ClickVoucherButton();
        }

        [Given(@"I tap on the HealthcareCentre1 button")]
        public async Task GivenITapOnTheHealthcareCentre1Button()
        {
            await this.SelectOperatorPage.ClickHealthcareCentre1OperatorButton();
        }

        [Given(@"I tap on the (.*) KES button")]
        public async Task GivenITapOnTheKESButton(Int32 voucherValue)
        {
            if (voucherValue == 10)
            {
               await this.SelectProductPage.ClickKES10ProductButton();
            }
        }

        [When(@"I enter the following recipient details")]
        public async Task WhenIEnterTheFollowingRecipientDetails(Table table)
        {
            TableRow tableRow = table.Rows.SingleOrDefault();
            String recipientMobile = SpecflowTableHelper.GetStringRowValue(tableRow, "RecipientMobile");
            String recipientEmail = SpecflowTableHelper.GetStringRowValue(tableRow, "RecipientEmail");
            String customerEmailAddress = SpecflowTableHelper.GetStringRowValue(tableRow, "CustomerEmailAddress");

            await this.IssueVoucherPage.EnterRecipientEmailAddress(recipientEmail);
            await this.IssueVoucherPage.EnterRecipientMobileNumber(recipientMobile);
            await this.IssueVoucherPage.EnterCustomerEmailAddress(customerEmailAddress);
        }


        [When(@"I tap on Issue Voucher")]
        public async Task WhenITapOnIssueVoucher()
        {
            await this.IssueVoucherPage.ClickIssueVoucherButton();
        }

        [Then(@"the Transactions Page is displayed")]
        public async Task ThenTheTransactionsPageIsDisplayed()
        {
            await this.TransactionsPage.AssertOnPage();
        }

        [Then(@"the Voucher Select Operator Page is displayed")]
        public async Task ThenTheVoucherSelectOperatorPageIsDisplayed()
        {
            await this.SelectOperatorPage.AssertOnPage();
        }

        [Then(@"the Voucher Select Product Page is displayed")]
        public async Task ThenTheVoucherSelectProductPageIsDisplayed()
        {
            await this.SelectProductPage.AssertOnPage();
        }

        [Then(@"the Voucher Issue Page is displayed")]
        public async Task ThenTheVoucherIssuePageIsDisplayed()
        {
            await this.IssueVoucherPage.AssertOnPage();
        }

        [Then(@"The Voucher Issue Successful Screen will be displayed")]
        public async Task ThenTheVoucherIssueSuccessfulScreenWillBeDisplayed()
        {
            await this.VoucherSuccessPage.AssertOnPage();
        }
    }
}
