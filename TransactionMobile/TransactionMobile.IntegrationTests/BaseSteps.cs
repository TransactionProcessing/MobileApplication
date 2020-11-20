using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionMobile.IntegrationTests
{
    using System.Diagnostics;
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

            //    private IApp App;

            public BaseSteps(FeatureContext featureContext,
                             ScenarioContext scenarioContext,
                             TestingContext testingContext)
            {
                this.FeatureContext = featureContext;
                this.ScenarioContext = scenarioContext;
                this.TestingContext = testingContext;
            }

        [BeforeScenario()]
        public async Task StartSystem()
        {
            // Initialise a logger
            String scenarioName = this.ScenarioContext.ScenarioInfo.Title.Replace(" ", "");
            TestingLogger logger = new TestingLogger();

            this.TestingContext.DockerHelper = new TransactionMobileDockerHelper(logger, this.TestingContext);
            logger.LogInformation($"About to Start Containers for Scenario Run - {scenarioName}");
            await this.TestingContext.DockerHelper.StartContainersForScenarioRun(scenarioName).ConfigureAwait(false);
            logger.LogInformation($"Containers for Scenario Run Started  - {scenarioName}");
        }

        [AfterScenario()]
        public async Task StopSystem()
        {
            TestingLogger logger = new TestingLogger();
            if (this.ScenarioContext.TestError != null)
            {
                List<IContainerService> containers = this.TestingContext.DockerHelper.Containers.Where(c => c.Name == this.TestingContext.DockerHelper.EstateManagementContainerName).ToList();

                // The test has failed, grab the logs from all the containers
                foreach (IContainerService containerService in containers)
                {
                    ConsoleStream<String> logStream = containerService.Logs();
                    IList<String> logData = logStream.ReadToEnd();

                    foreach (String s in logData)
                    {
                        logger.LogInformation(s);
                    }
                }
            }

            String scenarioName = this.ScenarioContext.ScenarioInfo.Title.Replace(" ", "");

            logger.LogInformation($"About to Stop Containers for Scenario Run - {scenarioName}");
            await this.TestingContext.DockerHelper.StopContainersForScenarioRun().ConfigureAwait(false);
            logger.LogInformation($"Containers for Scenario Run Stopped  - {scenarioName}");

        }

        [AfterStep]
        public void CheckStepStatus()
        {
            if (Debugger.IsAttached == false)
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

        [Given(@"the following security roles exist")]
        public async Task GivenTheFollowingSecurityRolesExist(Table table)
        {
            foreach (TableRow tableRow in table.Rows)
            {
                String roleName = SpecflowTableHelper.GetStringRowValue(tableRow, "RoleName");

                CreateRoleRequest createRoleRequest = new CreateRoleRequest
                                                      {
                                                          RoleName = roleName
                                                      };

                CreateRoleResponse createRoleResponse = await this.TestingContext.DockerHelper.SecurityServiceClient.CreateRole(createRoleRequest, CancellationToken.None)
                                                                  .ConfigureAwait(false);

                createRoleResponse.RoleId.ShouldNotBe(Guid.Empty);
            }
        }

        [Given(@"the following api resources exist")]
        public async Task GivenTheFollowingApiResourcesExist(Table table)
        {
            foreach (TableRow tableRow in table.Rows)
            {
                String resourceName = SpecflowTableHelper.GetStringRowValue(tableRow, "ResourceName");
                String displayName = SpecflowTableHelper.GetStringRowValue(tableRow, "DisplayName");
                String secret = SpecflowTableHelper.GetStringRowValue(tableRow, "Secret");
                String scopes = SpecflowTableHelper.GetStringRowValue(tableRow, "Scopes");
                String userClaims = SpecflowTableHelper.GetStringRowValue(tableRow, "UserClaims");

                List<String> splitScopes = scopes.Split(',').ToList();
                List<String> splitUserClaims = userClaims.Split(',').ToList();

                CreateApiResourceRequest createApiResourceRequest = new CreateApiResourceRequest
                                                                    {
                                                                        Description = String.Empty,
                                                                        DisplayName = displayName,
                                                                        Name = resourceName,
                                                                        Scopes = new List<String>(),
                                                                        Secret = secret,
                                                                        UserClaims = new List<String>()
                                                                    };
                splitScopes.ForEach(a => { createApiResourceRequest.Scopes.Add(a.Trim()); });
                splitUserClaims.ForEach(a => { createApiResourceRequest.UserClaims.Add(a.Trim()); });

                CreateApiResourceResponse createApiResourceResponse = await this
                                                                            .TestingContext.DockerHelper.SecurityServiceClient
                                                                            .CreateApiResource(createApiResourceRequest, CancellationToken.None).ConfigureAwait(false);

                createApiResourceResponse.ApiResourceName.ShouldBe(resourceName);
            }
        }

        [Given(@"the following clients exist")]
        public async Task GivenTheFollowingClientsExist(Table table)
        {
            foreach (TableRow tableRow in table.Rows)
            {
                String clientId = SpecflowTableHelper.GetStringRowValue(tableRow, "ClientId");
                String clientName = SpecflowTableHelper.GetStringRowValue(tableRow, "ClientName");
                String secret = SpecflowTableHelper.GetStringRowValue(tableRow, "Secret");
                String allowedScopes = SpecflowTableHelper.GetStringRowValue(tableRow, "AllowedScopes");
                String allowedGrantTypes = SpecflowTableHelper.GetStringRowValue(tableRow, "AllowedGrantTypes");

                List<String> splitAllowedScopes = allowedScopes.Split(',').ToList();
                List<String> splitAllowedGrantTypes = allowedGrantTypes.Split(',').ToList();

                CreateClientRequest createClientRequest = new CreateClientRequest
                                                          {
                                                              Secret = secret,
                                                              AllowedGrantTypes = new List<String>(),
                                                              AllowedScopes = new List<String>(),
                                                              ClientDescription = String.Empty,
                                                              ClientId = clientId,
                                                              ClientName = clientName
                                                          };

                splitAllowedScopes.ForEach(a => { createClientRequest.AllowedScopes.Add(a.Trim()); });
                splitAllowedGrantTypes.ForEach(a => { createClientRequest.AllowedGrantTypes.Add(a.Trim()); });

                CreateClientResponse createClientResponse = await this.TestingContext.DockerHelper.SecurityServiceClient
                                                                      .CreateClient(createClientRequest, CancellationToken.None).ConfigureAwait(false);

                createClientResponse.ClientId.ShouldBe(clientId);

                this.TestingContext.AddClientDetails(clientId, secret, allowedGrantTypes);
            }

            var merchantClient = this.TestingContext.GetClientDetails("merchantClient");

            String securityService = this.TestingContext.DockerHelper.SecurityServiceBaseAddress;
            String transactionProcessorAcl = this.TestingContext.DockerHelper.TransactionProcessorACLBaseAddress;
            String estateManagementUrl = this.TestingContext.DockerHelper.EstateManagementBaseAddress;
            String mobileConfigUrl = this.TestingContext.DockerHelper.MobileConfigBaseAddress;

            Console.WriteLine($"securityService [{securityService}]");
            Console.WriteLine($"transactionProcessorAcl [{transactionProcessorAcl}]");
            Console.WriteLine($"estateManagementUrl [{estateManagementUrl}]");
            Console.WriteLine($"mobileConfigUrl [{mobileConfigUrl}]");

            // Setup the config host
            var deviceIdentifier = AppManager.GetDeviceIdentifier();
            var config = new
                         {
                             id = deviceIdentifier,
                             deviceIdentifier,
                             clientId = merchantClient.ClientId,
                             clientSecret = merchantClient.ClientSecret,
                             securityService = securityService,
                             estateManagement = estateManagementUrl,
                             transactionProcessorACL = transactionProcessorAcl

                         };
            StringContent content = new StringContent(JsonConvert.SerializeObject(config), Encoding.UTF8, "application/json");
            var resp = await this.TestingContext.DockerHelper.MobileConfigHttpClient.PostAsync("configuration", content, CancellationToken.None).ConfigureAwait(false);

            AppManager.SetConfiguration(mobileConfigUrl);
        }

        [Given(@"I have a token to access the estate management and transaction processor acl resources")]
        public async Task GivenIHaveATokenToAccessTheEstateManagementAndTransactionProcessorAclResources(Table table)
        {
            foreach (TableRow tableRow in table.Rows)
            {
                String clientId = SpecflowTableHelper.GetStringRowValue(tableRow, "ClientId");

                Common.ClientDetails clientDetails = this.TestingContext.GetClientDetails(clientId);

                if (clientDetails.GrantType == "client_credentials")
                {
                    TokenResponse tokenResponse = await this.TestingContext.DockerHelper.SecurityServiceClient
                                                            .GetToken(clientId, clientDetails.ClientSecret, CancellationToken.None).ConfigureAwait(false);

                    this.TestingContext.AccessToken = tokenResponse.AccessToken;
                }
            }
        }

        [Given(@"I have created the following estates")]
        [When(@"I create the following estates")]
        public async Task WhenICreateTheFollowingEstates(Table table)
        {
            foreach (TableRow tableRow in table.Rows)
            {
                String estateName = SpecflowTableHelper.GetStringRowValue(tableRow, "EstateName");

                CreateEstateRequest createEstateRequest = new CreateEstateRequest
                                                          {
                                                              EstateId = Guid.NewGuid(),
                                                              EstateName = estateName
                                                          };

                CreateEstateResponse response = null;
                try
                {
                    
                 response = await this.TestingContext.DockerHelper.EstateClient
                                                          .CreateEstate(this.TestingContext.AccessToken, createEstateRequest, CancellationToken.None)
                                                          .ConfigureAwait(false);
                }
                catch (Exception e)
                {
                    this.TestingContext.DockerHelper.Logger.LogInformation(e.Message);
                    if (e.InnerException != null)
                    {
                        this.TestingContext.DockerHelper.Logger.LogInformation(e.InnerException.Message);
                        if (e.InnerException.InnerException != null)
                        {
                            this.TestingContext.DockerHelper.Logger.LogInformation(e.InnerException.InnerException.Message);
                        }
                    }
                }

                response.ShouldNotBeNull();
                response.EstateId.ShouldNotBe(Guid.Empty);

                // Cache the estate id
                this.TestingContext.AddEstateDetails(response.EstateId, estateName);

                //this.TestingContext.Logger.LogInformation($"Estate {estateName} created with Id {response.EstateId}");
            }

            foreach (TableRow tableRow in table.Rows)
            {
                EstateDetails estateDetails = this.TestingContext.GetEstateDetails(tableRow);
                EstateResponse estate = null;
                await Retry.For(async () =>
                                {
                                    estate = await this.TestingContext.DockerHelper.EstateClient
                                                                      .GetEstate(this.TestingContext.AccessToken, estateDetails.EstateId, CancellationToken.None).ConfigureAwait(false);
                                }, TimeSpan.FromMinutes(2));
                
                estate.ShouldNotBeNull();
                estate.EstateName.ShouldBe(estateDetails.EstateName);
            }
        }

        [Given(@"I have created the following operators")]
        [When(@"I create the following operators")]
        public async Task WhenICreateTheFollowingOperators(Table table)
        {
            foreach (TableRow tableRow in table.Rows)
            {
                String operatorName = SpecflowTableHelper.GetStringRowValue(tableRow, "OperatorName");
                Boolean requireCustomMerchantNumber = SpecflowTableHelper.GetBooleanValue(tableRow, "RequireCustomMerchantNumber");
                Boolean requireCustomTerminalNumber = SpecflowTableHelper.GetBooleanValue(tableRow, "RequireCustomTerminalNumber");

                CreateOperatorRequest createOperatorRequest = new CreateOperatorRequest
                                                              {
                                                                  Name = operatorName,
                                                                  RequireCustomMerchantNumber = requireCustomMerchantNumber,
                                                                  RequireCustomTerminalNumber = requireCustomTerminalNumber
                                                              };

                // lookup the estate id based on the name in the table
                EstateDetails estateDetails = this.TestingContext.GetEstateDetails(tableRow);

                CreateOperatorResponse response = await this.TestingContext.DockerHelper.EstateClient
                                                            .CreateOperator(this.TestingContext.AccessToken,
                                                                            estateDetails.EstateId,
                                                                            createOperatorRequest,
                                                                            CancellationToken.None).ConfigureAwait(false);

                response.ShouldNotBeNull();
                response.EstateId.ShouldNotBe(Guid.Empty);
                response.OperatorId.ShouldNotBe(Guid.Empty);

                // Cache the estate id
                estateDetails.AddOperator(response.OperatorId, operatorName);

                //this.TestingContext.Logger.LogInformation($"Operator {operatorName} created with Id {response.OperatorId} for Estate {estateDetails.EstateName}");
            }
        }

        [Given(@"I create a contract with the following values")]
        public async Task GivenICreateAContractWithTheFollowingValues(Table table)
        {
            foreach (TableRow tableRow in table.Rows)
            {
                EstateDetails estateDetails = this.TestingContext.GetEstateDetails(tableRow);

                String token = this.TestingContext.AccessToken;
                if (String.IsNullOrEmpty(estateDetails.AccessToken) == false)
                {
                    token = estateDetails.AccessToken;
                }

                String operatorName = SpecflowTableHelper.GetStringRowValue(tableRow, "OperatorName");
                Guid operatorId = estateDetails.GetOperatorId(operatorName);

                CreateContractRequest createContractRequest = new CreateContractRequest
                                                              {
                                                                  OperatorId = operatorId,
                                                                  Description = SpecflowTableHelper.GetStringRowValue(tableRow, "ContractDescription")
                                                              };

                CreateContractResponse contractResponse =
                    await this.TestingContext.DockerHelper.EstateClient.CreateContract(token, estateDetails.EstateId, createContractRequest, CancellationToken.None);

                estateDetails.AddContract(contractResponse.ContractId, createContractRequest.Description, operatorId);
            }
        }

        [When(@"I create the following Products")]
        public async Task WhenICreateTheFollowingProducts(Table table)
        {
            foreach (TableRow tableRow in table.Rows)
            {
                EstateDetails estateDetails = this.TestingContext.GetEstateDetails(tableRow);

                String token = this.TestingContext.AccessToken;
                if (String.IsNullOrEmpty(estateDetails.AccessToken) == false)
                {
                    token = estateDetails.AccessToken;
                }

                String contractName = SpecflowTableHelper.GetStringRowValue(tableRow, "ContractDescription");
                Contract contract = estateDetails.GetContract(contractName);
                String productValue = SpecflowTableHelper.GetStringRowValue(tableRow, "Value");

                AddProductToContractRequest addProductToContractRequest = new AddProductToContractRequest
                {
                    ProductName = SpecflowTableHelper.GetStringRowValue(tableRow, "ProductName"),
                    DisplayText = SpecflowTableHelper.GetStringRowValue(tableRow, "DisplayText"),
                    Value = null
                };
                if (String.IsNullOrEmpty(productValue) == false)
                {
                    addProductToContractRequest.Value = Decimal.Parse(productValue);
                }

                AddProductToContractResponse addProductToContractResponse = await this.TestingContext.DockerHelper.EstateClient.AddProductToContract(token,
                                                                                                                                                     estateDetails.EstateId,
                                                                                                                                                     contract.ContractId,
                                                                                                                                                     addProductToContractRequest,
                                                                                                                                                     CancellationToken.None);

                contract.AddProduct(addProductToContractResponse.ProductId, addProductToContractRequest.ProductName, addProductToContractRequest.DisplayText,
                                    addProductToContractRequest.Value);
            }
        }

        [When(@"I add the following Transaction Fees")]
        public async Task WhenIAddTheFollowingTransactionFees(Table table)
        {
            foreach (TableRow tableRow in table.Rows)
            {
                EstateDetails estateDetails = this.TestingContext.GetEstateDetails(tableRow);

                String token = this.TestingContext.AccessToken;
                if (String.IsNullOrEmpty(estateDetails.AccessToken) == false)
                {
                    token = estateDetails.AccessToken;
                }

                String contractName = SpecflowTableHelper.GetStringRowValue(tableRow, "ContractDescription");
                String productName = SpecflowTableHelper.GetStringRowValue(tableRow, "ProductName");
                Contract contract = estateDetails.GetContract(contractName);

                Product product = contract.GetProduct(productName);

                AddTransactionFeeForProductToContractRequest addTransactionFeeForProductToContractRequest = new AddTransactionFeeForProductToContractRequest
                {
                    Value =
                                                                                                                    SpecflowTableHelper
                                                                                                                        .GetDecimalValue(tableRow, "Value"),
                    Description =
                                                                                                                    SpecflowTableHelper.GetStringRowValue(tableRow,
                                                                                                                                                          "FeeDescription"),
                    CalculationType =
                                                                                                                    SpecflowTableHelper
                                                                                                                        .GetEnumValue<CalculationType>(tableRow,
                                                                                                                                                       "CalculationType")
                };

                AddTransactionFeeForProductToContractResponse addTransactionFeeForProductToContractResponse =
                    await this.TestingContext.DockerHelper.EstateClient.AddTransactionFeeForProductToContract(token,
                                                                                                              estateDetails.EstateId,
                                                                                                              contract.ContractId,
                                                                                                              product.ProductId,
                                                                                                              addTransactionFeeForProductToContractRequest,
                                                                                                              CancellationToken.None);

                product.AddTransactionFee(addTransactionFeeForProductToContractResponse.TransactionFeeId,
                                          addTransactionFeeForProductToContractRequest.CalculationType,
                                          addTransactionFeeForProductToContractRequest.Description,
                                          addTransactionFeeForProductToContractRequest.Value);
            }
        }
        
        [Given("I create the following merchants")]
        [When(@"I create the following merchants")]
        public async Task WhenICreateTheFollowingMerchants(Table table)
        {
            foreach (TableRow tableRow in table.Rows)
            {
                // lookup the estate id based on the name in the table
                EstateDetails estateDetails = this.TestingContext.GetEstateDetails(tableRow);
                String token = this.TestingContext.AccessToken;
                if (String.IsNullOrEmpty(estateDetails.AccessToken) == false)
                {
                    token = estateDetails.AccessToken;
                }

                String merchantName = SpecflowTableHelper.GetStringRowValue(tableRow, "MerchantName");
                CreateMerchantRequest createMerchantRequest = new CreateMerchantRequest
                                                              {
                                                                  Name = merchantName,
                                                                  Contact = new Contact
                                                                            {
                                                                                ContactName = SpecflowTableHelper.GetStringRowValue(tableRow, "ContactName"),
                                                                                EmailAddress = SpecflowTableHelper.GetStringRowValue(tableRow, "EmailAddress")
                                                                            },
                                                                  Address = new Address
                                                                            {
                                                                                AddressLine1 = SpecflowTableHelper.GetStringRowValue(tableRow, "AddressLine1"),
                                                                                Town = SpecflowTableHelper.GetStringRowValue(tableRow, "Town"),
                                                                                Region = SpecflowTableHelper.GetStringRowValue(tableRow, "Region"),
                                                                                Country = SpecflowTableHelper.GetStringRowValue(tableRow, "Country")
                                                                            }
                                                              };

                CreateMerchantResponse response = await this.TestingContext.DockerHelper.EstateClient
                                                            .CreateMerchant(token, estateDetails.EstateId, createMerchantRequest, CancellationToken.None)
                                                            .ConfigureAwait(false);

                response.ShouldNotBeNull();
                response.EstateId.ShouldBe(estateDetails.EstateId);
                response.MerchantId.ShouldNotBe(Guid.Empty);

                // Cache the merchant id
                estateDetails.AddMerchant(response.MerchantId, merchantName);

                //this.TestingContext.Logger.LogInformation($"Merchant {merchantName} created with Id {response.MerchantId} for Estate {estateDetails.EstateName}");
            }

            foreach (TableRow tableRow in table.Rows)
            {
                EstateDetails estateDetails = this.TestingContext.GetEstateDetails(tableRow);

                String merchantName = SpecflowTableHelper.GetStringRowValue(tableRow, "MerchantName");

                Guid merchantId = estateDetails.GetMerchantId(merchantName);

                String token = this.TestingContext.AccessToken;
                if (String.IsNullOrEmpty(estateDetails.AccessToken) == false)
                {
                    token = estateDetails.AccessToken;
                }

                MerchantResponse merchant = await this.TestingContext.DockerHelper.EstateClient
                                                      .GetMerchant(token, estateDetails.EstateId, merchantId, CancellationToken.None).ConfigureAwait(false);

                merchant.MerchantName.ShouldBe(merchantName);
            }
        }

        [When(@"I create the following security users")]
        [Given("I have created the following security users")]
        public async Task WhenICreateTheFollowingSecurityUsers(Table table)
        {
            foreach (TableRow tableRow in table.Rows)
            {
                // lookup the estate id based on the name in the table
                EstateDetails estateDetails = this.TestingContext.GetEstateDetails(tableRow);

                if (tableRow.ContainsKey("EstateName") && tableRow.ContainsKey("MerchantName") == false)
                {
                    // Creating an Estate User
                    CreateEstateUserRequest createEstateUserRequest = new CreateEstateUserRequest
                                                                      {
                                                                          EmailAddress = SpecflowTableHelper.GetStringRowValue(tableRow, "EmailAddress"),
                                                                          FamilyName = SpecflowTableHelper.GetStringRowValue(tableRow, "FamilyName"),
                                                                          GivenName = SpecflowTableHelper.GetStringRowValue(tableRow, "GivenName"),
                                                                          MiddleName = SpecflowTableHelper.GetStringRowValue(tableRow, "MiddleName"),
                                                                          Password = SpecflowTableHelper.GetStringRowValue(tableRow, "Password")
                                                                      };

                    CreateEstateUserResponse createEstateUserResponse =
                        await this.TestingContext.DockerHelper.EstateClient.CreateEstateUser(this.TestingContext.AccessToken,
                                                                                             estateDetails.EstateId,
                                                                                             createEstateUserRequest,
                                                                                             CancellationToken.None);

                    createEstateUserResponse.EstateId.ShouldBe(estateDetails.EstateId);
                    createEstateUserResponse.UserId.ShouldNotBe(Guid.Empty);

                    estateDetails.SetEstateUser(createEstateUserRequest.EmailAddress, createEstateUserRequest.Password);

                    //this.TestingContext.Logger.LogInformation($"Security user {createEstateUserRequest.EmailAddress} assigned to Estate {estateDetails.EstateName}");
                }
                else if (tableRow.ContainsKey("MerchantName"))
                {
                    // Creating a merchant user
                    String token = this.TestingContext.AccessToken;
                    if (String.IsNullOrEmpty(estateDetails.AccessToken) == false)
                    {
                        token = estateDetails.AccessToken;
                    }

                    // lookup the merchant id based on the name in the table
                    String merchantName = SpecflowTableHelper.GetStringRowValue(tableRow, "MerchantName");
                    Guid merchantId = estateDetails.GetMerchantId(merchantName);

                    CreateMerchantUserRequest createMerchantUserRequest = new CreateMerchantUserRequest
                                                                          {
                                                                              EmailAddress = SpecflowTableHelper.GetStringRowValue(tableRow, "EmailAddress"),
                                                                              FamilyName = SpecflowTableHelper.GetStringRowValue(tableRow, "FamilyName"),
                                                                              GivenName = SpecflowTableHelper.GetStringRowValue(tableRow, "GivenName"),
                                                                              MiddleName = SpecflowTableHelper.GetStringRowValue(tableRow, "MiddleName"),
                                                                              Password = SpecflowTableHelper.GetStringRowValue(tableRow, "Password")
                                                                          };

                    CreateMerchantUserResponse createMerchantUserResponse =
                        await this.TestingContext.DockerHelper.EstateClient.CreateMerchantUser(token,
                                                                                               estateDetails.EstateId,
                                                                                               merchantId,
                                                                                               createMerchantUserRequest,
                                                                                               CancellationToken.None);

                    createMerchantUserResponse.EstateId.ShouldBe(estateDetails.EstateId);
                    createMerchantUserResponse.MerchantId.ShouldBe(merchantId);
                    createMerchantUserResponse.UserId.ShouldNotBe(Guid.Empty);

                    estateDetails.AddMerchantUser(merchantName, createMerchantUserRequest.EmailAddress, createMerchantUserRequest.Password);

                    //this.TestingContext.Logger.LogInformation($"Security user {createMerchantUserRequest.EmailAddress} assigned to Merchant {merchantName}");
                }
            }
        }

        [Given(@"I have assigned the following  operator to the merchants")]
        [When(@"I assign the following  operator to the merchants")]
        public async Task WhenIAssignTheFollowingOperatorToTheMerchants(Table table)
        {
            foreach (TableRow tableRow in table.Rows)
            {
                EstateDetails estateDetails = this.TestingContext.GetEstateDetails(tableRow);

                String token = this.TestingContext.AccessToken;
                if (String.IsNullOrEmpty(estateDetails.AccessToken) == false)
                {
                    token = estateDetails.AccessToken;
                }

                // Lookup the merchant id
                String merchantName = SpecflowTableHelper.GetStringRowValue(tableRow, "MerchantName");
                Guid merchantId = estateDetails.GetMerchantId(merchantName);

                // Lookup the operator id
                String operatorName = SpecflowTableHelper.GetStringRowValue(tableRow, "OperatorName");
                Guid operatorId = estateDetails.GetOperatorId(operatorName);

                AssignOperatorRequest assignOperatorRequest = new AssignOperatorRequest
                                                              {
                                                                  OperatorId = operatorId,
                                                                  MerchantNumber = SpecflowTableHelper.GetStringRowValue(tableRow, "MerchantNumber"),
                                                                  TerminalNumber = SpecflowTableHelper.GetStringRowValue(tableRow, "TerminalNumber"),
                                                              };

                AssignOperatorResponse assignOperatorResponse = await this.TestingContext.DockerHelper.EstateClient
                                                                          .AssignOperatorToMerchant(token,
                                                                                                    estateDetails.EstateId,
                                                                                                    merchantId,
                                                                                                    assignOperatorRequest,
                                                                                                    CancellationToken.None).ConfigureAwait(false);

                assignOperatorResponse.EstateId.ShouldBe(estateDetails.EstateId);
                assignOperatorResponse.MerchantId.ShouldBe(merchantId);
                assignOperatorResponse.OperatorId.ShouldBe(operatorId);

                //this.TestingContext.Logger.LogInformation($"Operator {operatorName} assigned to Estate {estateDetails.EstateName}");
            }
        }

        [Given(@"I make the following manual merchant deposits")]
        public async Task GivenIMakeTheFollowingManualMerchantDeposits(Table table)
        {
            foreach (TableRow tableRow in table.Rows)
            {
                EstateDetails estateDetails = this.TestingContext.GetEstateDetails(tableRow);

                String token = this.TestingContext.AccessToken;
                if (String.IsNullOrEmpty(estateDetails.AccessToken) == false)
                {
                    token = estateDetails.AccessToken;
                }

                // Lookup the merchant id
                String merchantName = SpecflowTableHelper.GetStringRowValue(tableRow, "MerchantName");
                Guid merchantId = estateDetails.GetMerchantId(merchantName);

                MakeMerchantDepositRequest makeMerchantDepositRequest = new MakeMerchantDepositRequest
                                                                        {
                                                                            DepositDateTime = SpecflowTableHelper.GetDateForDateString(SpecflowTableHelper.GetStringRowValue(tableRow, "DateTime"), DateTime.Now),
                                                                            Source = MerchantDepositSource.Manual,
                                                                            Reference = SpecflowTableHelper.GetStringRowValue(tableRow, "Reference"),
                                                                            Amount = SpecflowTableHelper.GetDecimalValue(tableRow, "Amount")
                                                                        };

                MakeMerchantDepositResponse makeMerchantDepositResponse = await this.TestingContext.DockerHelper.EstateClient.MakeMerchantDeposit(token, estateDetails.EstateId, merchantId, makeMerchantDepositRequest, CancellationToken.None).ConfigureAwait(false);

                makeMerchantDepositResponse.EstateId.ShouldBe(estateDetails.EstateId);
                makeMerchantDepositResponse.MerchantId.ShouldBe(merchantId);
                makeMerchantDepositResponse.DepositId.ShouldNotBe(Guid.Empty);
            }
        }

        [Then(@"the merchant balances are as follows")]
        public async Task ThenTheMerchantBalancesAreAsFollows(Table table)
        {
            foreach (TableRow tableRow in table.Rows)
            {
                EstateDetails estateDetails = this.TestingContext.GetEstateDetails(tableRow);

                String token = this.TestingContext.AccessToken;
                if (String.IsNullOrEmpty(estateDetails.AccessToken) == false)
                {
                    token = estateDetails.AccessToken;
                }

                // Lookup the merchant id
                String merchantName = SpecflowTableHelper.GetStringRowValue(tableRow, "MerchantName");
                Guid merchantId = estateDetails.GetMerchantId(merchantName);

                Decimal availableBalance = SpecflowTableHelper.GetDecimalValue(tableRow, "AvailableBalance");
                Decimal balance = SpecflowTableHelper.GetDecimalValue(tableRow, "Balance");

                MerchantBalanceResponse merchantBalanceResponse = await this.TestingContext.DockerHelper.EstateClient.GetMerchantBalance(token, estateDetails.EstateId, merchantId, CancellationToken.None).ConfigureAwait(false);

                merchantBalanceResponse.EstateId.ShouldBe(estateDetails.EstateId);
                merchantBalanceResponse.MerchantId.ShouldBe(merchantId);
                merchantBalanceResponse.AvailableBalance.ShouldBe(availableBalance);
                merchantBalanceResponse.Balance.ShouldBe(balance);
            }
        }
    }
}
