using System;
using System.Collections.Generic;
using System.Text;

namespace TransactionMobile.IntegrationTests.WithAppium.Steps
{
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.Threading.Tasks;
    using Common;
    using Drivers;
    using IntegrationTestClients;
    using Newtonsoft.Json;
    using Pages;
    using TechTalk.SpecFlow;

    [Binding]
    [Scope(Tag = "background")]
    public class BackgroundSteps
    {
        private readonly BackdoorDriver Backdoor;

        private readonly ScenarioContext ScenarioContext;

        private readonly TestingContext TestingContext;

        private LoginPage LoginPage = new LoginPage();

        private TestModePage TestModePage = new TestModePage();

        public BackgroundSteps(BackdoorDriver backdoor,
                               ScenarioContext scenarioContext,
                               TestingContext testingContext)
        {
            this.Backdoor = backdoor;
            this.ScenarioContext = scenarioContext;
            this.TestingContext = testingContext;
        }

        [Given(@"I have created the following estates")]
        public async Task GivenIHaveCreatedTheFollowingEstates(Table table)
        {
            await this.Backdoor.SetIntegrationModeOn();

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

        [Given(@"the following transaction fees have been settled")]
        public async Task GivenTheFollowingTransactionFeesHaveBeenSettled(Table table)
        {
            foreach (TableRow tableRow in table.Rows)
            {
                String estateName = SpecflowTableHelper.GetStringRowValue(tableRow, "EstateName");
                String merchantName = SpecflowTableHelper.GetStringRowValue(tableRow, "MerchantName");
                Decimal calculatedValue = SpecflowTableHelper.GetDecimalValue(tableRow, "CalculatedValue");
                String settlementDateString = SpecflowTableHelper.GetStringRowValue(tableRow, "SettlementDate");
                DateTime settlementDate = SpecflowTableHelper.GetDateForDateString(settlementDateString, DateTime.Today);
                String operatorIdentifier = SpecflowTableHelper.GetStringRowValue(tableRow, "OperatorIdentifier");
                String feeDescription = SpecflowTableHelper.GetStringRowValue(tableRow, "FeeDescription");

                Merchant merchant = this.TestingContext.GetMerchant(estateName, merchantName);
                var settlementFee = new SettlementFee
                                    {
                                        SettlementDate = settlementDate,
                                        OperatorIdentifier = operatorIdentifier,
                                        FeeDescription = feeDescription,
                                        CalculatedValue = calculatedValue,
                                        IsSettled = true,
                                        MerchantName = merchantName,
                                        MerchantId = merchant.MerchantId,
                                        EstateId = merchant.EstateId
                                    };
                this.TestingContext.AddSettlementFee(estateName, settlementFee);
                await this.Backdoor.UpdateSettlementData(settlementFee);
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

                var contract = this.TestingContext.AddContractProduct(estateName, contractDescription, contractProductId, productName, displayText, productValue);
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

        [Given(@"the application in in test mode")]
        public async Task GivenTheApplicationInInTestMode()
        {
            if (AppiumDriver.MobileTestPlatform == MobileTestPlatform.iOS)
            {
                String stage = null;
                try
                {
                    stage = "1";
                    await this.LoginPage.ClickTestModeButton();
                    stage = "2";
                    await this.TestModePage.AssertOnPage();
                    stage = "3";
                    await this.TestModePage.EnterPin("1234");
                    stage = "4";
                    var c = this.TestingContext.GetContracts();
                    var contractData = JsonConvert.SerializeObject(c);
                    contractData = StringCompression.Compress(contractData);
                    await this.TestModePage.EnterTestContractData(contractData);
                    stage = "5";
                    var m = this.TestingContext.GetMerchant();
                    var merchantData = JsonConvert.SerializeObject(m);
                    merchantData = StringCompression.Compress(merchantData);
                    await this.TestModePage.EnterTestMerchantData(merchantData);
                    stage = "6";
                    var s = this.TestingContext.GetSettlementFees();
                    var settlementFeeData = JsonConvert.SerializeObject(s);
                    settlementFeeData = StringCompression.Compress(settlementFeeData);
                    await this.TestModePage.EnterSettlementFeeData(settlementFeeData);
                    stage = "7";
                    await this.TestModePage.ClickSetTestModeButton();
                }
                catch(Exception e)
                {
                    throw new Exception($"Failed to find element. Stage {stage}. Source [{AppiumDriver.iOSDriver.PageSource}]");
                }
            }
        }
    }

    public static class StringCompression
        {
            /// <summary>
            /// Compresses a string and returns a deflate compressed, Base64 encoded string.
            /// </summary>
            /// <param name="uncompressedString">String to compress</param>
            public static string Compress(string uncompressedString)
            {
                byte[] compressedBytes;

                using (var uncompressedStream = new MemoryStream(Encoding.UTF8.GetBytes(uncompressedString)))
                {
                    using (var compressedStream = new MemoryStream())
                    {
                        // setting the leaveOpen parameter to true to ensure that compressedStream will not be closed when compressorStream is disposed
                        // this allows compressorStream to close and flush its buffers to compressedStream and guarantees that compressedStream.ToArray() can be called afterward
                        // although MSDN documentation states that ToArray() can be called on a closed MemoryStream, I don't want to rely on that very odd behavior should it ever change
                        using (var compressorStream = new DeflateStream(compressedStream, CompressionLevel.Optimal, true))
                        {
                            uncompressedStream.CopyTo(compressorStream);
                        }

                        // call compressedStream.ToArray() after the enclosing DeflateStream has closed and flushed its buffer to compressedStream
                        compressedBytes = compressedStream.ToArray();
                    }
                }

                return Convert.ToBase64String(compressedBytes);
            }

            /// <summary>
            /// Decompresses a deflate compressed, Base64 encoded string and returns an uncompressed string.
            /// </summary>
            /// <param name="compressedString">String to decompress.</param>
            public static string Decompress(string compressedString)
            {
                byte[] decompressedBytes;

                var compressedStream = new MemoryStream(Convert.FromBase64String(compressedString));

                using (var decompressorStream = new DeflateStream(compressedStream, CompressionMode.Decompress))
                {
                    using (var decompressedStream = new MemoryStream())
                    {
                        decompressorStream.CopyTo(decompressedStream);

                        decompressedBytes = decompressedStream.ToArray();
                    }
                }

                return Encoding.UTF8.GetString(decompressedBytes);
            }
        }
}
