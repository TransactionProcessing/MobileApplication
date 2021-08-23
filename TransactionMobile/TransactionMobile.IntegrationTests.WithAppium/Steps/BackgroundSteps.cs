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

            this.Backdoor.SetIntegrationModeOn().Wait();
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
        [When(@"I create the following operators")]
        public async Task WhenICreateTheFollowingOperators(Table table)
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
                await this.Backdoor.UpdateTestMerchant(merchant);
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
                await this.Backdoor.UpdateTestMerchant(merchant);
            }
        }

        
        [Given(@"I create a contract with the following values")]
        public async Task GivenICreateAContractWithTheFollowingValues(Table table)
        {
            foreach (TableRow tableRow in table.Rows)
            {
                String estateName = SpecflowTableHelper.GetStringRowValue(tableRow, "EstateName");
                Guid contractId = Guid.NewGuid();
                String operatorName = SpecflowTableHelper.GetStringRowValue(tableRow, "OperatorName");
                String contractDescription = SpecflowTableHelper.GetStringRowValue(tableRow, "ContractDescription");

                var contract = this.TestingContext.AddContract(estateName, contractId, operatorName, contractDescription);
                await this.Backdoor.UpdateTestContract(contract);
            }
        }

        [When(@"I create the following Products")]
        public async Task WhenICreateTheFollowingProducts(Table table)
        {
            foreach (TableRow tableRow in table.Rows)
            {
                String estateName = SpecflowTableHelper.GetStringRowValue(tableRow, "EstateName");
                String contractDescription = SpecflowTableHelper.GetStringRowValue(tableRow, "ContractDescription");
                Guid contractProductId = Guid.NewGuid();
                String productName = SpecflowTableHelper.GetStringRowValue(tableRow, "ProductName");
                String displayText = SpecflowTableHelper.GetStringRowValue(tableRow, "DisplayText");

                Decimal? productValue = null;
                String productValueString = SpecflowTableHelper.GetStringRowValue(tableRow, "Value");
                if (String.IsNullOrEmpty(SpecflowTableHelper.GetStringRowValue(tableRow, "Value")) == false)
                {
                    productValue = Decimal.Parse(productValueString);
                }
                var contract = this.TestingContext.AddContractProduct(estateName, contractDescription, contractProductId, productName,
                                                       displayText, productValue);
                await this.Backdoor.UpdateTestContract(contract);
            }
        }

        [When(@"I add the following Transaction Fees")]
        public async Task WhenIAddTheFollowingTransactionFees(Table table)
        {
            foreach (TableRow tableRow in table.Rows)
            {
                String estateName = SpecflowTableHelper.GetStringRowValue(tableRow, "EstateName");
                String contractDescription = SpecflowTableHelper.GetStringRowValue(tableRow, "ContractDescription");
                String productName = SpecflowTableHelper.GetStringRowValue(tableRow, "ProductName");
                Guid contractProductTransactionFeeId = Guid.NewGuid();
                String calculationType = SpecflowTableHelper.GetStringRowValue(tableRow, "CalculationType");
                String feeDescription = SpecflowTableHelper.GetStringRowValue(tableRow, "FeeDescription");
                Decimal value = SpecflowTableHelper.GetDecimalValue(tableRow, "Value");
                var contract = this.TestingContext.AddContractProductTransactionFee(estateName,
                                                                     contractDescription,
                                                                     productName,
                                                                     contractProductTransactionFeeId,
                                                                     calculationType,
                                                                     feeDescription,
                                                                     value);
                await this.Backdoor.UpdateTestContract(contract);

            }
        }
    }
}
