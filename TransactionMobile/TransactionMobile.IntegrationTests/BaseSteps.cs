using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionMobile.IntegrationTests
{
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Net.Sockets;
    using System.Reflection;
    using System.Threading;
    using Common;
    using Ductus.FluentDocker.Executors;
    using Ductus.FluentDocker.Extensions;
    using Ductus.FluentDocker.Services;
    using Ductus.FluentDocker.Services.Extensions;
    using EstateManagement.DataTransferObjects;
    using EstateManagement.DataTransferObjects.Requests;
    using EstateManagement.DataTransferObjects.Responses;
    using IntegrationTestClients;
    using Newtonsoft.Json;
    using NUnit.Framework;
    using SecurityService.DataTransferObjects;
    using SecurityService.DataTransferObjects.Requests;
    using SecurityService.DataTransferObjects.Responses;
    using Shouldly;
    using TechTalk.SpecFlow;
    using Xamarin.UITest;
    using Xamarin.UITest.Configuration;

    [Binding]
    [Scope(Tag = "base")]
    public class BaseSteps
    {

        private readonly FeatureContext FeatureContext;

        private readonly ScenarioContext ScenarioContext;

        private readonly TestingContext TestingContext;
        
        public BaseSteps(FeatureContext featureContext,
                         ScenarioContext scenarioContext,
                         TestingContext testingContext)
        {
            this.FeatureContext = featureContext;
            this.ScenarioContext = scenarioContext;
            this.TestingContext = testingContext;
        }
        
        //[AfterStep]
        public void CheckStepStatus()
        {
            // Build the screenshot name
            String featureName = this.FeatureContext.GetFeatureNameForScreenshot();
            String scenarioName = this.ScenarioContext.GetScenarioNameForScreenshot();
            String stepName = this.ScenarioContext.GetStepNameForScreenshot();

            // Capture screen shot on exception
            FileInfo screenshot = AppManager.App.Screenshot($"{scenarioName}:{stepName}");

            String screenshotPath = Environment.GetEnvironmentVariable("ScreenshotFolder");
            if (String.IsNullOrEmpty(screenshotPath))
            {
                // Get the executing directory
                String currentDirectory = $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}";

                String screenshotDirectory = $"{currentDirectory}\\Screenshots\\{featureName}";

                if (!Directory.Exists(screenshotDirectory))
                {
                    Directory.CreateDirectory(screenshotDirectory);
                }

                // Now copy the screenshot
                FileInfo fi = screenshot.CopyTo($"{screenshotDirectory}\\{DateTime.Now:yyyMMddHHmmssfff}-{scenarioName}-{stepName}.jpg", true);

                Console.WriteLine($"{fi.FullName} exists");
            }
            else
            {
                screenshotPath = $"{screenshotPath}//{featureName}";
                if (!Directory.Exists(screenshotPath))
                {
                    Directory.CreateDirectory(screenshotPath);
                }

                String fileName = $"{screenshotPath}//{DateTime.Now:yyyMMddHHmmssfff}-{scenarioName}-{stepName}.jpg";
                Console.WriteLine($"About to copy to {fileName}");
                FileInfo fi = screenshot.CopyTo(fileName, true);
                Console.WriteLine($"{fi.FullName} exists");
            }
        }
    }

    public static class Extensions
    {
        public static String GetFeatureNameForLogging(this FeatureContext featureContext)
        {
            return featureContext.FeatureInfo.Title.Replace(" ", "");
        }

        public static String GetFeatureNameForScreenshot(this FeatureContext featureContext)
        {
            String featureName = featureContext.GetFeatureNameForLogging();
            featureName = String.Join("", featureName.Split(Path.GetInvalidFileNameChars()));

            // Remove other characters that are valid for a path but not a screenshot name (/ and : for example)
            featureName = featureName.Replace("/", "");
            featureName = featureName.Replace(":", "");

            return featureName;
        }

        public static String GetScenarioNameForLogging(this ScenarioContext scenarioContext)
        {
            return scenarioContext.ScenarioInfo.Title.Replace(" ", "");
        }

        /// <summary>
        /// Gets the scenario name for screenshot.
        /// </summary>
        /// <param name="scenarioContext">The scenario context.</param>
        /// <returns></returns>
        public static String GetScenarioNameForScreenshot(this ScenarioContext scenarioContext)
        {
            String scenarioName = scenarioContext.GetScenarioNameForLogging();

            scenarioName = String.Join("", scenarioName.Split(Path.GetInvalidFileNameChars()));

            // Remove other characters that are valid for a path but not a screenshot name (/ and : for example)
            scenarioName = scenarioName.Replace("/", "");
            scenarioName = scenarioName.Replace(":", "");

            return scenarioName;
        }

        /// <summary>
        /// Gets the step name for logging.
        /// </summary>
        /// <param name="scenarioContext">The scenario context.</param>
        /// <returns></returns>
        public static String GetStepNameForLogging(this ScenarioContext scenarioContext)
        {
            return scenarioContext.StepContext.StepInfo.Text.Replace(" ", "");
        }

        /// <summary>
        /// Gets the step name for screenshot.
        /// </summary>
        /// <param name="scenarioContext">The scenario context.</param>
        /// <returns></returns>
        public static String GetStepNameForScreenshot(this ScenarioContext scenarioContext)
        {
            String stepName = scenarioContext.GetStepNameForLogging();

            stepName = String.Join("", stepName.Split(Path.GetInvalidFileNameChars()));

            // Remove other characters that are valid for a path but not a screenshot name (/ and : for example)
            stepName = stepName.Replace("/", "");
            stepName = stepName.Replace(":", "");

            return stepName;
        }
    }

    [Binding]
    [Scope(Tag = "shared")]
    public class SharedSteps
    {
        private readonly ScenarioContext ScenarioContext;

        private readonly TestingContext TestingContext;

        public SharedSteps(ScenarioContext scenarioContext,
                           TestingContext testingContext)
        {
            this.ScenarioContext = scenarioContext;
            this.TestingContext = testingContext;
        }
        
        [Given(@"I have created the following estates")]
        [When(@"I create the following estates")]
        public async Task WhenICreateTheFollowingEstates(Table table)
        {
            foreach (TableRow tableRow in table.Rows)
            {
                Guid estateId = Guid.NewGuid();
                String estateName = SpecflowTableHelper.GetStringRowValue(tableRow, "EstateName");

                this.TestingContext.AddEstate(estateId,estateName);
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

                this.TestingContext.AddOperator(estateName, operatorId,operatorName,requireCustomMerchantNumber, requireCustomTerminalNumber);
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

                this.TestingContext.AddContract(estateName, contractId, operatorName, contractDescription);
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
                this.TestingContext.AddContractProduct(estateName,contractDescription, contractProductId, productName,
                                                       displayText,productValue);
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
                this.TestingContext.AddContractProductTransactionFee(estateName,
                                                                     contractDescription,
                                                                     productName,
                                                                     contractProductTransactionFeeId,
                                                                     calculationType,
                                                                     feeDescription,
                                                                     value);

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

                this.TestingContext.AddMerchant(estateName,merchantId,merchantName,emailAddress,password,givenName,familyName);

                Merchant merchant = this.TestingContext.GetMerchant(estateName, merchantName);
                AppManager.UpdateTestMerchant(merchant);
            }
        }

        //[When(@"I create the following security users")]
        //[Given("I have created the following security users")]
        //public async Task WhenICreateTheFollowingSecurityUsers(Table table)
        //{
        //    foreach (TableRow tableRow in table.Rows)
        //    {
        //        // lookup the estate id based on the name in the table
        //        EstateDetails estateDetails = this.TestingContext.GetEstateDetails(tableRow);

        //        if (tableRow.ContainsKey("EstateName") && tableRow.ContainsKey("MerchantName") == false)
        //        {
        //            // Creating an Estate User
        //            CreateEstateUserRequest createEstateUserRequest = new CreateEstateUserRequest
        //                                                              {
        //                                                                  EmailAddress = SpecflowTableHelper.GetStringRowValue(tableRow, "EmailAddress"),
        //                                                                  FamilyName = SpecflowTableHelper.GetStringRowValue(tableRow, "FamilyName"),
        //                                                                  GivenName = SpecflowTableHelper.GetStringRowValue(tableRow, "GivenName"),
        //                                                                  MiddleName = SpecflowTableHelper.GetStringRowValue(tableRow, "MiddleName"),
        //                                                                  Password = SpecflowTableHelper.GetStringRowValue(tableRow, "Password")
        //                                                              };

        //            CreateEstateUserResponse createEstateUserResponse =
        //                await this.TestingContext.DockerHelper.EstateClient.CreateEstateUser(this.TestingContext.AccessToken,
        //                                                                                     estateDetails.EstateId,
        //                                                                                     createEstateUserRequest,
        //                                                                                     CancellationToken.None);

        //            createEstateUserResponse.EstateId.ShouldBe(estateDetails.EstateId);
        //            createEstateUserResponse.UserId.ShouldNotBe(Guid.Empty);

        //            estateDetails.SetEstateUser(createEstateUserRequest.EmailAddress, createEstateUserRequest.Password);

        //            //this.TestingContext.Logger.LogInformation($"Security user {createEstateUserRequest.EmailAddress} assigned to Estate {estateDetails.EstateName}");
        //        }
        //        else if (tableRow.ContainsKey("MerchantName"))
        //        {
        //            // Creating a merchant user
        //            String token = this.TestingContext.AccessToken;
        //            if (String.IsNullOrEmpty(estateDetails.AccessToken) == false)
        //            {
        //                token = estateDetails.AccessToken;
        //            }

        //            // lookup the merchant id based on the name in the table
        //            String merchantName = SpecflowTableHelper.GetStringRowValue(tableRow, "MerchantName");
        //            Guid merchantId = estateDetails.GetMerchantId(merchantName);

        //            CreateMerchantUserRequest createMerchantUserRequest = new CreateMerchantUserRequest
        //                                                                  {
        //                                                                      EmailAddress = SpecflowTableHelper.GetStringRowValue(tableRow, "EmailAddress"),
        //                                                                      FamilyName = SpecflowTableHelper.GetStringRowValue(tableRow, "FamilyName"),
        //                                                                      GivenName = SpecflowTableHelper.GetStringRowValue(tableRow, "GivenName"),
        //                                                                      MiddleName = SpecflowTableHelper.GetStringRowValue(tableRow, "MiddleName"),
        //                                                                      Password = SpecflowTableHelper.GetStringRowValue(tableRow, "Password")
        //                                                                  };

        //            CreateMerchantUserResponse createMerchantUserResponse =
        //                await this.TestingContext.DockerHelper.EstateClient.CreateMerchantUser(token,
        //                                                                                       estateDetails.EstateId,
        //                                                                                       merchantId,
        //                                                                                       createMerchantUserRequest,
        //                                                                                       CancellationToken.None);

        //            createMerchantUserResponse.EstateId.ShouldBe(estateDetails.EstateId);
        //            createMerchantUserResponse.MerchantId.ShouldBe(merchantId);
        //            createMerchantUserResponse.UserId.ShouldNotBe(Guid.Empty);

        //            estateDetails.AddMerchantUser(merchantName, createMerchantUserRequest.EmailAddress, createMerchantUserRequest.Password);

        //            //this.TestingContext.Logger.LogInformation($"Security user {createMerchantUserRequest.EmailAddress} assigned to Merchant {merchantName}");
        //        }
        //    }
        //}

        //[Given(@"I have assigned the following  operator to the merchants")]
        //[When(@"I assign the following  operator to the merchants")]
        //public async Task WhenIAssignTheFollowingOperatorToTheMerchants(Table table)
        //{
        //    foreach (TableRow tableRow in table.Rows)
        //    {
        //        EstateDetails estateDetails = this.TestingContext.GetEstateDetails(tableRow);

        //        String token = this.TestingContext.AccessToken;
        //        if (String.IsNullOrEmpty(estateDetails.AccessToken) == false)
        //        {
        //            token = estateDetails.AccessToken;
        //        }

        //        // Lookup the merchant id
        //        String merchantName = SpecflowTableHelper.GetStringRowValue(tableRow, "MerchantName");
        //        Guid merchantId = estateDetails.GetMerchantId(merchantName);

        //        // Lookup the operator id
        //        String operatorName = SpecflowTableHelper.GetStringRowValue(tableRow, "OperatorName");
        //        Guid operatorId = estateDetails.GetOperatorId(operatorName);

        //        AssignOperatorRequest assignOperatorRequest = new AssignOperatorRequest
        //                                                      {
        //                                                          OperatorId = operatorId,
        //                                                          MerchantNumber = SpecflowTableHelper.GetStringRowValue(tableRow, "MerchantNumber"),
        //                                                          TerminalNumber = SpecflowTableHelper.GetStringRowValue(tableRow, "TerminalNumber"),
        //                                                      };

        //        AssignOperatorResponse assignOperatorResponse = await this.TestingContext.DockerHelper.EstateClient
        //                                                                  .AssignOperatorToMerchant(token,
        //                                                                                            estateDetails.EstateId,
        //                                                                                            merchantId,
        //                                                                                            assignOperatorRequest,
        //                                                                                            CancellationToken.None).ConfigureAwait(false);

        //        assignOperatorResponse.EstateId.ShouldBe(estateDetails.EstateId);
        //        assignOperatorResponse.MerchantId.ShouldBe(merchantId);
        //        assignOperatorResponse.OperatorId.ShouldBe(operatorId);

        //        //this.TestingContext.Logger.LogInformation($"Operator {operatorName} assigned to Estate {estateDetails.EstateName}");
        //    }
        //}

        [Given(@"I make the following manual merchant deposits")]
        public async Task GivenIMakeTheFollowingManualMerchantDeposits(Table table)
        {
            foreach (TableRow tableRow in table.Rows)
            {
                String estateName = SpecflowTableHelper.GetStringRowValue(tableRow, "EstateName");
                String merchantName = SpecflowTableHelper.GetStringRowValue(tableRow, "MerchantName");
                DateTime depositDateTime = SpecflowTableHelper.GetDateForDateString(SpecflowTableHelper.GetStringRowValue(tableRow, "DateTime"), DateTime.Now);
                Decimal amount = SpecflowTableHelper.GetDecimalValue(tableRow, "Amount");

                this.TestingContext.AddMerchantDeposit(estateName,merchantName, amount,depositDateTime);
                Merchant merchant = this.TestingContext.GetMerchant(estateName, merchantName);
                AppManager.UpdateTestMerchant(merchant);
            }
        }

        //[Then(@"the merchant balances are as follows")]
        //public async Task ThenTheMerchantBalancesAreAsFollows(Table table)
        //{
        //    foreach (TableRow tableRow in table.Rows)
        //    {
        //        EstateDetails estateDetails = this.TestingContext.GetEstateDetails(tableRow);

        //        String token = this.TestingContext.AccessToken;
        //        if (String.IsNullOrEmpty(estateDetails.AccessToken) == false)
        //        {
        //            token = estateDetails.AccessToken;
        //        }

        //        // Lookup the merchant id
        //        String merchantName = SpecflowTableHelper.GetStringRowValue(tableRow, "MerchantName");
        //        Guid merchantId = estateDetails.GetMerchantId(merchantName);

        //        Decimal availableBalance = SpecflowTableHelper.GetDecimalValue(tableRow, "AvailableBalance");
        //        Decimal balance = SpecflowTableHelper.GetDecimalValue(tableRow, "Balance");

        //        MerchantBalanceResponse merchantBalanceResponse = await this.TestingContext.DockerHelper.EstateClient.GetMerchantBalance(token, estateDetails.EstateId, merchantId, CancellationToken.None).ConfigureAwait(false);

        //        merchantBalanceResponse.EstateId.ShouldBe(estateDetails.EstateId);
        //        merchantBalanceResponse.MerchantId.ShouldBe(merchantId);
        //        merchantBalanceResponse.AvailableBalance.ShouldBe(availableBalance);
        //        merchantBalanceResponse.Balance.ShouldBe(balance);
        //    }
        //}
    }
}
