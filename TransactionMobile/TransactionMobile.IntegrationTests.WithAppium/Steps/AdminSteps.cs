﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TransactionMobile.IntegrationTests.WithAppium.Steps
{
    using System.Threading.Tasks;
    using Pages;
    using TechTalk.SpecFlow;

    [Binding]
    [Scope(Tag = "admin")]
    public class ReconciliationSteps
    {
        TransactionsPage TransactionsPage = new TransactionsPage();

        private MainPage MainPage = new MainPage();
        private AdminPage AdminPage = new AdminPage();

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

        [Given(@"I tap on the Admin button")]
        public async Task GivenITapOnTheAdminButton()
        {
            await this.TransactionsPage.ClickAdminButton();
        }

        [Then(@"the Admin Page is displayed")]
        public async Task ThenTheAdminPageIsDisplayed()
        {
            await this.AdminPage.AssertOnPage();
        }

        [Given(@"I tap on the Reconciliation button")]
        public async Task GivenITapOnTheReconciliationButton()
        {
            await this.AdminPage.ClickReconciliationButton();
        }

        [Then(@"the reconciliation success message toast will be displayed")]
        public async Task ThenTheReconciliationSuccessMessageToastWillBeDisplayed()
        {
            await this.AdminPage.CheckReconciliationSuccessMessageToastIsDisplayed();
        }

    }
}
