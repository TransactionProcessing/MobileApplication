using System;
using System.Collections.Generic;
using System.Text;

namespace TransactionMobile.IntegrationTests.WithAppium.Steps
{
    using System.Threading.Tasks;
    using Common;
    using Drivers;
    using IntegrationTestClients;
    using TechTalk.SpecFlow;

    [Binding]
    [Scope(Tag = "background")]
    public class BackgroundSteps
    {
        private readonly BackdoorDriver Backdoor;

        private readonly ScenarioContext ScenarioContext;

        private readonly TestingContext TestingContext;

        public BackgroundSteps(BackdoorDriver backdoor,
                               ScenarioContext scenarioContext,
                               TestingContext testingContext)
        {
            this.Backdoor = backdoor;
            this.ScenarioContext = scenarioContext;
            this.TestingContext = testingContext;

            this.Backdoor.SetIntegrationModeOn();
        }

        [Given(@"I have created the following estates")]
        public void GivenIHaveCreatedTheFollowingEstates(Table table)
        {
            foreach (TableRow tableRow in table.Rows)
            {
                Guid estateId = Guid.NewGuid();
                String estateName = SpecflowTableHelper.GetStringRowValue(tableRow, "EstateName");

                this.TestingContext.AddEstate(estateId, estateName);
            }
        }

        [Given(@"I have created the following operators")]
        public void GivenIHaveCreatedTheFollowingOperators(Table table)
        {
            foreach (TableRow tableRow in table.Rows)
            {
                String estateName = SpecflowTableHelper.GetStringRowValue(tableRow, "EstateName");
                Guid operatorId = Guid.NewGuid();
                String operatorName = SpecflowTableHelper.GetStringRowValue(tableRow, "OperatorName");
                Boolean requireCustomMerchantNumber = SpecflowTableHelper.GetBooleanValue(tableRow, "RequireCustomMerchantNumber");
                Boolean requireCustomTerminalNumber = SpecflowTableHelper.GetBooleanValue(tableRow, "RequireCustomTerminalNumber");

                this.TestingContext.AddOperator(estateName, operatorId, operatorName, requireCustomMerchantNumber, requireCustomTerminalNumber);
            }
        }

        [Given("I create the following merchants")]
        [When(@"I create the following merchants")]
        public async Task WhenICreateTheFollowingMerchants(Table table)
        {
            foreach (TableRow tableRow in table.Rows)
            {
                String estateName = SpecflowTableHelper.GetStringRowValue(tableRow, "EstateName");
                String merchantName = SpecflowTableHelper.GetStringRowValue(tableRow, "MerchantName");
                String emailAddress = SpecflowTableHelper.GetStringRowValue(tableRow, "EmailAddress");
                String familyName = SpecflowTableHelper.GetStringRowValue(tableRow, "FamilyName");
                String givenName = SpecflowTableHelper.GetStringRowValue(tableRow, "GivenName");
                String password = SpecflowTableHelper.GetStringRowValue(tableRow, "Password");
                Guid merchantId = Guid.NewGuid();

                this.TestingContext.AddMerchant(estateName, merchantId, merchantName, emailAddress, password, givenName, familyName);

                Merchant merchant = this.TestingContext.GetMerchant(estateName, merchantName);
                this.Backdoor.UpdateTestMerchant(merchant);
            }
        }

        [Given(@"I make the following manual merchant deposits")]
        public async Task GivenIMakeTheFollowingManualMerchantDeposits(Table table)
        {
            foreach (TableRow tableRow in table.Rows)
            {
                String estateName = SpecflowTableHelper.GetStringRowValue(tableRow, "EstateName");
                String merchantName = SpecflowTableHelper.GetStringRowValue(tableRow, "MerchantName");
                DateTime depositDateTime = SpecflowTableHelper.GetDateForDateString(SpecflowTableHelper.GetStringRowValue(tableRow, "DateTime"), DateTime.Now);
                Decimal amount = SpecflowTableHelper.GetDecimalValue(tableRow, "Amount");

                this.TestingContext.AddMerchantDeposit(estateName, merchantName, amount, depositDateTime);
                Merchant merchant = this.TestingContext.GetMerchant(estateName, merchantName);
                this.Backdoor.UpdateTestMerchant(merchant);
            }
        }
    }
}
