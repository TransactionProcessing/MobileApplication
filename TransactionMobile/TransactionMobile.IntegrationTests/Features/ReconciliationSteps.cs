using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionMobile.IntegrationTests.Features
{
    using Pages;
    using TechTalk.SpecFlow;

    [Binding]
    [Scope(Tag = "reconciliation")]
    public class ReconciliationSteps
    {
        TransactionsPage TransactionsPage = new TransactionsPage();

        private AdminPage AdminPage = new AdminPage();

        [Given(@"I tap on the Admin button")]
        public void GivenITapOnTheAdminButton()
        {
            this.TransactionsPage.ClickAdminButton();
        }

        [Then(@"the Admin Page is displayed")]
        public async Task ThenTheAdminPageIsDisplayed()
        {
            await this.AdminPage.AssertOnPage();
        }

        [Given(@"I tap on the Reconciliation button")]
        public void GivenITapOnTheReconciliationButton()
        {
            this.AdminPage.ClickReconciliationButton();
        }

        [Then(@"the reconciliation success message toast will be displayed")]
        public void ThenTheReconciliationSuccessMessageToastWillBeDisplayed()
        {
            this.AdminPage.CheckReconciliationSuccessMessageToastIsDisplayed();
        }

    }
}
