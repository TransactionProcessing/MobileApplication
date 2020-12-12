using System;
using TechTalk.SpecFlow;

namespace TransactionMobile.IntegrationTests.Features
{
    using System.Linq;
    using System.Threading.Tasks;
    using Common;
    using Pages;

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
        public void GivenITapOnTheTransactionsButton()
        {
            this.MainPage.ClickTransactionsButton();
        }
        
        [Given(@"I tap on the Voucher button")]
        public void GivenITapOnTheVoucherButton()
        {
            this.TransactionsPage.ClickVoucherButton();
        }

        [Given(@"I tap on the HealthcareCentre1 button")]
        public void GivenITapOnTheHealthcareCentre1Button()
        {
            this.SelectOperatorPage.ClickHealthcareCentre1OperatorButton();
        }

        [Given(@"I tap on the (.*) KES button")]
        public void GivenITapOnTheKESButton(Int32 voucherValue)
        {
            if (voucherValue == 10)
            {
                this.SelectProductPage.ClickKES10ProductButton();
            }
        }

        [When(@"I enter the following recipient details")]
        public void WhenIEnterTheFollowingRecipientDetails(Table table)
        {
            TableRow tableRow = table.Rows.SingleOrDefault();
            String recipientMobile = SpecflowTableHelper.GetStringRowValue(tableRow, "RecipientMobile");
            String recipientEmail = SpecflowTableHelper.GetStringRowValue(tableRow, "RecipientEmail");
            String customerEmailAddress = SpecflowTableHelper.GetStringRowValue(tableRow, "CustomerEmailAddress");

            this.IssueVoucherPage.EnterRecipientEmailAddress(recipientEmail);
            this.IssueVoucherPage.EnterRecipientMobileNumber(recipientMobile);
            this.IssueVoucherPage.EnterCustomerEmailAddress(customerEmailAddress);
        }


        [When(@"I tap on Issue Voucher")]
        public void WhenITapOnIssueVoucher()
        {
            this.IssueVoucherPage.ClickIssueVoucherButton();
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
