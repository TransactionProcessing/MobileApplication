using Ductus.FluentDocker.Builders;
using Ductus.FluentDocker.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionMobile.IntegrationTests.Common
{
    using System.Data;
    using System.Data.SqlClient;
    using System.Diagnostics;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Reflection;
    using System.Threading;
    using Ductus.FluentDocker.Common;
    using Ductus.FluentDocker.Model.Builders;
    using Ductus.FluentDocker.Services.Extensions;
    using EstateManagement.Client;
    using NUnit.Framework.Internal;
    using SecurityService.Client;
    using Shouldly;
    using TechTalk.SpecFlow.Plugins;

    public partial class TransactionMobileDockerHelper
    {
        #region Fields

        /// <summary>
        /// The estate client
        /// </summary>
        public IEstateClient EstateClient;

        /// <summary>
        /// The HTTP client
        /// </summary>
        public HttpClient HttpClient;

        /// <summary>
        /// The security service client
        /// </summary>
        public ISecurityServiceClient SecurityServiceClient;

        /// <summary>
        /// The test identifier
        /// </summary>
        public Guid TestId;

        /// <summary>
        /// The transaction processor client
        /// </summary>
        //public ITransactionProcessorACLClient ITransactionProcessorACLClient;

        /// <summary>
        /// The containers
        /// </summary>
        public List<IContainerService> Containers;

        /// <summary>
        /// The estate management API port
        /// </summary>
        protected Int32 EstateManagementApiPort;

        /// <summary>
        /// The event store HTTP port
        /// </summary>
        protected Int32 EventStoreHttpPort;

        /// <summary>
        /// The security service port
        /// </summary>
        protected Int32 SecurityServicePort;

        /// <summary>
        /// The test networks
        /// </summary>
        protected List<INetworkService> TestNetworks;

        /// <summary>
        /// The transaction processor acl port
        /// </summary>
        protected Int32 TransactionProcessorACLPort;

        protected String SecurityServiceContainerName;

        public String EstateManagementContainerName;

        protected String EventStoreContainerName;

        protected String EstateReportingContainerName;

        protected String SubscriptionServiceContainerName;

        protected String TransactionProcessorContainerName;

        protected String TransactionProcessorACLContainerName;

        protected String TestHostContainerName;

        public const Int32 TestHostPort = 9000;

        public static IContainerService SetupTestHostContainer(String containerName, ILogger logger, String imageName,
                                                               List<INetworkService> networkServices,
                                                               String hostFolder,
                                                               (String URL, String UserName, String Password)? dockerCredentials,
                                                               Boolean forceLatestImage = false)
        {
            logger.LogInformation("About to Start Test Hosts Container");

            ContainerBuilder testHostContainer = new Builder().UseContainer().WithName(containerName)
                                                              .UseImage(imageName, forceLatestImage).ExposePort(TransactionMobileDockerHelper.TestHostPort)
                                                              .UseNetwork(networkServices.ToArray()).Mount(hostFolder, "/home", MountType.ReadWrite);

            if (dockerCredentials.HasValue)
            {
                testHostContainer.WithCredential(dockerCredentials.Value.URL, dockerCredentials.Value.UserName, dockerCredentials.Value.Password);
            }

            // Now build and return the container                
            IContainerService builtContainer = testHostContainer.Build().Start();

            logger.LogInformation("Test Hosts Container Started");

            return builtContainer;
        }

        /// <summary>
        /// The transaction processor port
        /// </summary>
        protected Int32 TransactionProcessorPort;

        /// <summary>
        /// The logger
        /// </summary>
        public readonly TestingLogger Logger;

        protected readonly TestingContext TestingContext;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DockerHelper"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public TransactionMobileDockerHelper(TestingLogger logger, TestingContext testingContext)
        {
            this.Logger = logger;
            this.TestingContext = testingContext;
            this.Containers = new List<IContainerService>();
            this.TestNetworks = new List<INetworkService>();
        }

        #endregion

        private static String LocalHostAddress;
        #region Methods

        /// <summary>
        /// Starts the containers for scenario run.
        /// </summary>
        /// <param name="scenarioName">Name of the scenario.</param>
        public async Task StartContainersForScenarioRun(String scenarioName)
        {
            // Setup the base address resolvers
            TransactionMobileDockerHelper.LocalHostAddress = Environment.GetEnvironmentVariable("localhostaddress");
            if (String.IsNullOrEmpty(TransactionMobileDockerHelper.LocalHostAddress))
            {
                TransactionMobileDockerHelper.LocalHostAddress = "192.168.1.67";
            }

            this.Logger.LogInformation("In StartContainersForScenarioRun");
            String traceFolder = FdOs.IsWindows() ? $"D:\\home\\txnproc\\trace\\{scenarioName}" : $"//home//txnproc//trace//{scenarioName}";

            Logging.Enabled();

            Guid testGuid = Guid.NewGuid();
            this.TestId = testGuid;

            this.Logger.LogInformation($"Test Id is {testGuid}");

            // Setup the container names
            this.SecurityServiceContainerName = $"securityservice{testGuid:N}";
            this.EstateManagementContainerName = $"estate{testGuid:N}";
            this.EventStoreContainerName = $"eventstore{testGuid:N}";
            this.EstateReportingContainerName = $"estatereporting{testGuid:N}";
            this.SubscriptionServiceContainerName = $"subscription{testGuid:N}";
            this.TransactionProcessorContainerName = $"txnprocessor{testGuid:N}";
            this.TransactionProcessorACLContainerName = $"txnprocessoracl{testGuid:N}";
            this.TestHostContainerName = $"testhosts{testGuid:N}";

            (String, String, String) dockerCredentials = ("https://www.docker.com", "stuartferguson", "Sc0tland");

            INetworkService testNetwork = TransactionMobileDockerHelper.SetupTestNetwork();
            this.TestNetworks.Add(testNetwork);
            IContainerService eventStoreContainer =
                await TransactionMobileDockerHelper.SetupEventStoreContainer(this.EventStoreContainerName, this.Logger, "eventstore/eventstore:release-5.0.2", testNetwork, traceFolder);

            IContainerService estateManagementContainer = TransactionMobileDockerHelper.SetupEstateManagementContainer(this.EstateManagementContainerName, this.Logger,
                                                                                                                       "stuartferguson/estatemanagement", new List<INetworkService>
                                                                                                                                                          {
                                                                                                                                                              testNetwork,
                                                                                                                                                              Setup.DatabaseServerNetwork
                                                                                                                                                          }, traceFolder, null,
                                                                                                                       this.SecurityServiceContainerName,
                                                                                                                       this.EventStoreContainerName,
                                                                                                                       Setup.SqlServerContainerName,
                                                                                                                       "sa",
                                                                                                                       "thisisalongpassword123!",
                                                                                                                       ("serviceClient", "Secret1"),
                                                                                                                       true);

            IContainerService securityServiceContainer = TransactionMobileDockerHelper.SetupSecurityServiceContainer(this.SecurityServiceContainerName,
                                                                                                                     this.Logger,
                                                                                                                     "stuartferguson/securityservice",
                                                                                                                     testNetwork,
                                                                                                                     traceFolder,
                                                                                                                     dockerCredentials,
                                                                                                                     true);

            IContainerService transactionProcessorContainer = TransactionMobileDockerHelper.SetupTransactionProcessorContainer(this.TransactionProcessorContainerName,
                                                                                                                               this.Logger,
                                                                                                                               "stuartferguson/transactionprocessor",
                                                                                                                               new List<INetworkService>
                                                                                                                               {
                                                                                                                                   testNetwork
                                                                                                                               },
                                                                                                                               traceFolder,
                                                                                                                               dockerCredentials,
                                                                                                                               this.SecurityServiceContainerName,
                                                                                                                               this.EstateManagementContainerName,
                                                                                                                               this.EventStoreContainerName,
                                                                                                                               ("serviceClient", "Secret1"),
                                                                                                                               this.TestHostContainerName,
                                                                                                                               true);

            IContainerService estateReportingContainer = TransactionMobileDockerHelper.SetupEstateReportingContainer(this.EstateReportingContainerName,
                                                                                                                     this.Logger,
                                                                                                                     "stuartferguson/estatereporting",
                                                                                                                     new List<INetworkService>
                                                                                                                     {
                                                                                                                         testNetwork,
                                                                                                                         Setup.DatabaseServerNetwork
                                                                                                                     },
                                                                                                                     traceFolder,
                                                                                                                     dockerCredentials,
                                                                                                                     this.SecurityServiceContainerName,
                                                                                                                     Setup.SqlServerContainerName,
                                                                                                                     "sa",
                                                                                                                     "thisisalongpassword123!",
                                                                                                                     ("serviceClient", "Secret1"),
                                                                                                                     true);

            IContainerService transactionProcessorACLContainer = TransactionMobileDockerHelper.SetupTransactionProcessorACLContainer(this.TransactionProcessorACLContainerName,
                                                                                                                                     this.Logger,
                                                                                                                                     "stuartferguson/transactionprocessoracl",
                                                                                                                                     testNetwork,
                                                                                                                                     traceFolder,
                                                                                                                                     null,
                                                                                                                                     this.SecurityServiceContainerName,
                                                                                                                                     this.TransactionProcessorContainerName,
                                                                                                                                     ("serviceClient", "Secret1"),
                                                                                                                                     true);

            IContainerService testhostContainer = SetupTestHostContainer(this.TestHostContainerName,
                                                                         this.Logger,
                                                                         "stuartferguson/testhosts",
                                                                         new List<INetworkService>
                                                                         {
                                                                             testNetwork
                                                                         },
                                                                         traceFolder,
                                                                         dockerCredentials,
                                                                         true);

            this.Containers.AddRange(new List<IContainerService>
                                     {
                                         eventStoreContainer,
                                         estateManagementContainer,
                                         securityServiceContainer,
                                         transactionProcessorContainer,
                                         transactionProcessorACLContainer,
                                         estateReportingContainer,
                                         testhostContainer
                                     });

            // Cache the ports
            this.EstateManagementApiPort = estateManagementContainer.ToHostExposedEndpoint("5000/tcp").Port;
            this.SecurityServicePort = securityServiceContainer.ToHostExposedEndpoint("5001/tcp").Port;
            this.EventStoreHttpPort = eventStoreContainer.ToHostExposedEndpoint("2113/tcp").Port;
            this.TransactionProcessorPort = transactionProcessorContainer.ToHostExposedEndpoint("5002/tcp").Port;
            this.TransactionProcessorACLPort = transactionProcessorACLContainer.ToHostExposedEndpoint("5003/tcp").Port;

            String EstateManagementBaseAddressResolver(String api) => $"http://{TransactionMobileDockerHelper.LocalHostAddress}:{this.EstateManagementApiPort}";
            String SecurityServiceBaseAddressResolver(String api) => $"http://{TransactionMobileDockerHelper.LocalHostAddress}:{this.SecurityServicePort}";
            String TransactionProcessorAclBaseAddressResolver(String api) => $"http://{TransactionMobileDockerHelper.LocalHostAddress}:{this.TransactionProcessorACLPort}";

            this.SecurityServiceBaseAddress = SecurityServiceBaseAddressResolver(String.Empty);
            this.TransactionProcessorACLBaseAddress = TransactionProcessorAclBaseAddressResolver(String.Empty);

            HttpClient httpClient = new HttpClient();
            this.EstateClient = new EstateClient(EstateManagementBaseAddressResolver, httpClient);
            this.SecurityServiceClient = new SecurityServiceClient(SecurityServiceBaseAddressResolver, httpClient);
            //this.ITransactionProcessorACLClient = new TransactionProcessorACLClient(TransactionProcessorAclBaseAddressResolver, httpClient);

            this.HttpClient = new HttpClient();
            this.HttpClient.BaseAddress = new Uri(TransactionProcessorAclBaseAddressResolver(string.Empty));

            await PopulateSubscriptionServiceConfiguration().ConfigureAwait(false);

            IContainerService subscriptionServiceContainer = TransactionMobileDockerHelper.SetupSubscriptionServiceContainer(this.SubscriptionServiceContainerName,
                                                                                                                             this.Logger,
                                                                                                                             "stuartferguson/subscriptionservicehost",
                                                                                                                             new List<INetworkService>
                                                                                                                             {
                                                                                                                                 testNetwork,
                                                                                                                                 Setup.DatabaseServerNetwork
                                                                                                                             },
                                                                                                                             traceFolder,
                                                                                                                             dockerCredentials,
                                                                                                                             this.SecurityServiceContainerName,
                                                                                                                             Setup.SqlServerContainerName,
                                                                                                                             "sa",
                                                                                                                             "thisisalongpassword123!",
                                                                                                                             this.TestId,
                                                                                                                             ("serviceClient", "Secret1"),
                                                                                                                             true);

            this.Containers.Add(subscriptionServiceContainer);
        }

        public String SecurityServiceBaseAddress;

        public String TransactionProcessorACLBaseAddress;

        protected async Task PopulateSubscriptionServiceConfiguration()
        {
            String connectionString = Setup.GetLocalConnectionString("SubscriptionServiceConfiguration");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync(CancellationToken.None).ConfigureAwait(false);

                    // Create an Event Store Server
                    await this.InsertEventStoreServer(connection, this.EventStoreContainerName).ConfigureAwait(false);

                    String endPointUri = $"http://{this.EstateReportingContainerName}:5005/api/domainevents";
                    // Add Route for Estate Aggregate Events
                    await this.InsertSubscription(connection, "$ce-EstateAggregate", "Reporting", endPointUri).ConfigureAwait(false);

                    // Add Route for Merchant Aggregate Events
                    await this.InsertSubscription(connection, "$ce-MerchantAggregate", "Reporting", endPointUri).ConfigureAwait(false);

                    connection.Close();
                }
                catch (Exception e)
                {
                    throw;
                }
            }
        }
        protected async Task CleanUpSubscriptionServiceConfiguration()
        {
            String connectionString = Setup.GetLocalConnectionString("SubscriptionServiceConfiguration");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync(CancellationToken.None).ConfigureAwait(false);

                // Delete the Event Store Server
                await this.DeleteEventStoreServer(connection).ConfigureAwait(false);

                // Delete the Subscriptions
                await this.DeleteSubscriptions(connection).ConfigureAwait(false);

                connection.Close();
            }
        }

        protected async Task InsertEventStoreServer(SqlConnection openConnection, String eventStoreContainerName)
        {
            String esConnectionString = $"ConnectTo=tcp://admin:changeit@{eventStoreContainerName}:{TransactionMobileDockerHelper.EventStoreTcpDockerPort};VerboseLogging=true;";
            SqlCommand command = openConnection.CreateCommand();
            command.CommandText = $"INSERT INTO EventStoreServer(EventStoreServerId, ConnectionString,Name) SELECT '{this.TestId}', '{esConnectionString}', 'TestEventStore'";
            command.CommandType = CommandType.Text;
            await command.ExecuteNonQueryAsync(CancellationToken.None).ConfigureAwait(false);
        }

        protected async Task DeleteEventStoreServer(SqlConnection openConnection)
        {
            SqlCommand command = openConnection.CreateCommand();
            command.CommandText = $"DELETE FROM EventStoreServer WHERE EventStoreServerId = '{this.TestId}'";
            command.CommandType = CommandType.Text;
            await command.ExecuteNonQueryAsync(CancellationToken.None).ConfigureAwait(false);
        }

        protected async Task DeleteSubscriptions(SqlConnection openConnection)
        {
            SqlCommand command = openConnection.CreateCommand();
            command.CommandText = $"DELETE FROM Subscription WHERE EventStoreId = '{this.TestId}'";
            command.CommandType = CommandType.Text;
            await command.ExecuteNonQueryAsync(CancellationToken.None).ConfigureAwait(false);
        }

        protected async Task InsertSubscription(SqlConnection openConnection, String streamName, String groupName, String endPointUri)
        {
            SqlCommand command = openConnection.CreateCommand();
            command.CommandText = $"INSERT INTO subscription(SubscriptionId, EventStoreId, StreamName, GroupName, EndPointUri, StreamPosition) SELECT '{Guid.NewGuid()}', '{this.TestId}', '{streamName}', '{groupName}', '{endPointUri}', null";
            command.CommandType = CommandType.Text;
            await command.ExecuteNonQueryAsync(CancellationToken.None).ConfigureAwait(false);
        }

        //private async Task RemoveEstateReadModel()
        //{
        //    List<Guid> estateIdList = this.TestingContext.GetAllEstateIds();

        //    foreach (Guid estateId in estateIdList)
        //    {
        //        String databaseName = $"EstateReportingReadModel{estateId}";

        //        // Build the connection string (to master)
        //        String connectionString = Setup.GetLocalConnectionString(databaseName);
        //        EstateReportingContext context = new EstateReportingContext(connectionString);
        //        await context.Database.EnsureDeletedAsync(CancellationToken.None);
        //    }
        //}

        /// <summary>
        /// Stops the containers for scenario run.
        /// </summary>
        public async Task StopContainersForScenarioRun()
        {
            String debug = AppManager.GetDebug();
            this.TestingContext.DockerHelper.Logger.LogInformation(debug);

            await CleanUpSubscriptionServiceConfiguration().ConfigureAwait(false);

            //await RemoveEstateReadModel().ConfigureAwait(false);

            if (this.Containers.Any())
            {
                foreach (IContainerService containerService in this.Containers)
                {
                    containerService.StopOnDispose = true;
                    containerService.RemoveOnDispose = true;
                    containerService.Dispose();
                }
            }

            if (this.TestNetworks.Any())
            {
                foreach (INetworkService networkService in this.TestNetworks)
                {
                    networkService.Stop();
                    networkService.Remove(true);
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Setups the estate management container.
        /// </summary>
        /// <param name="containerName">Name of the container.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="imageName">Name of the image.</param>
        /// <param name="networkServices">The network services.</param>
        /// <param name="hostFolder">The host folder.</param>
        /// <param name="dockerCredentials">The docker credentials.</param>
        /// <param name="securityServiceContainerName">Name of the security service container.</param>
        /// <param name="eventStoreContainerName">Name of the event store container.</param>
        /// <param name="clientDetails">The client details.</param>
        /// <param name="forceLatestImage">if set to <c>true</c> [force latest image].</param>
        /// <param name="securityServicePort">The security service port.</param>
        /// <returns></returns>
        public static IContainerService SetupEstateManagementContainer(String containerName,
                                                                       ILogger logger,
                                                                       String imageName,
                                                                       List<INetworkService> networkServices,
                                                                       String hostFolder,
                                                                       (String URL, String UserName, String Password)? dockerCredentials,
                                                                       String securityServiceContainerName,
                                                                       String eventStoreContainerName,
                                                                       String sqlServerContainerName,
                                                                       String sqlServerUserName,
                                                                       String sqlServerPassword,
                                                                       (String clientId, String clientSecret) clientDetails,
                                                                       Boolean forceLatestImage = false,
                                                                       Int32 securityServicePort = TransactionMobileDockerHelper.SecurityServiceDockerPort)
        {
            logger.LogInformation("About to Start Estate Management Container");

            List<String> environmentVariables = new List<String>();
            environmentVariables
                .Add($"EventStoreSettings:ConnectionString=ConnectTo=tcp://admin:changeit@{eventStoreContainerName}:{TransactionMobileDockerHelper.EventStoreTcpDockerPort};VerboseLogging=true;");
            environmentVariables.Add($"AppSettings:SecurityService=http://{securityServiceContainerName}:{securityServicePort}");
            environmentVariables.Add($"SecurityConfiguration:Authority=http://{securityServiceContainerName}:{securityServicePort}");
            environmentVariables.Add($"urls=http://*:{TransactionMobileDockerHelper.EstateManagementDockerPort}");
            environmentVariables
                .Add($"ConnectionStrings:EstateReportingReadModel=\"server={sqlServerContainerName};user id={sqlServerUserName};password={sqlServerPassword};database=EstateReportingReadModel\"");

            ContainerBuilder estateManagementContainer = new Builder().UseContainer().WithName(containerName).WithEnvironment(environmentVariables.ToArray())
                                                                      .UseImage(imageName, forceLatestImage).ExposePort(TransactionMobileDockerHelper.EstateManagementDockerPort)
                                                                      .UseNetwork(networkServices.ToArray()).Mount(hostFolder, "/home", MountType.ReadWrite);

            if (dockerCredentials.HasValue)
            {
                estateManagementContainer.WithCredential(dockerCredentials.Value.URL, dockerCredentials.Value.UserName, dockerCredentials.Value.Password);
            }

            // Now build and return the container                
            IContainerService builtContainer = estateManagementContainer.Build().Start();

            logger.LogInformation("Estate Management Container Started");

            return builtContainer;
        }

        /// <summary>
        /// Setups the estate reporting container.
        /// </summary>
        /// <param name="containerName">Name of the container.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="imageName">Name of the image.</param>
        /// <param name="networkServices">The network services.</param>
        /// <param name="hostFolder">The host folder.</param>
        /// <param name="dockerCredentials">The docker credentials.</param>
        /// <param name="securityServiceContainerName">Name of the security service container.</param>
        /// <param name="sqlServerContainerName">Name of the SQL server container.</param>
        /// <param name="sqlServerUserName">Name of the SQL server user.</param>
        /// <param name="sqlServerPassword">The SQL server password.</param>
        /// <param name="clientDetails">The client details.</param>
        /// <param name="forceLatestImage">if set to <c>true</c> [force latest image].</param>
        /// <param name="securityServicePort">The security service port.</param>
        /// <returns></returns>
        public static IContainerService SetupEstateReportingContainer(String containerName,
                                                                      ILogger logger,
                                                                      String imageName,
                                                                      List<INetworkService> networkServices,
                                                                      String hostFolder,
                                                                      (String URL, String UserName, String Password)? dockerCredentials,
                                                                      String securityServiceContainerName,
                                                                      String sqlServerContainerName,
                                                                      String sqlServerUserName,
                                                                      String sqlServerPassword,
                                                                      (String clientId, String clientSecret) clientDetails,
                                                                      Boolean forceLatestImage = false,
                                                                      Int32 securityServicePort = TransactionMobileDockerHelper.SecurityServiceDockerPort)
        {
            logger.LogInformation("About to Start Estate Reporting Container");

            List<String> environmentVariables = new List<String>();
            environmentVariables.Add($"AppSettings:SecurityService=http://{securityServiceContainerName}:{securityServicePort}");
            environmentVariables.Add($"SecurityConfiguration:Authority=http://{securityServiceContainerName}:{securityServicePort}");
            environmentVariables.Add($"urls=http://*:{TransactionMobileDockerHelper.EstateReportingDockerPort}");
            environmentVariables
                .Add($"ConnectionStrings:EstateReportingReadModel=\"server={sqlServerContainerName};user id={sqlServerUserName};password={sqlServerPassword};database=EstateReportingReadModel\"");

            ContainerBuilder estateReportingContainer = new Builder().UseContainer().WithName(containerName).WithEnvironment(environmentVariables.ToArray())
                                                                     .UseImage(imageName, forceLatestImage).ExposePort(TransactionMobileDockerHelper.EstateReportingDockerPort)
                                                                     .UseNetwork(networkServices.ToArray()).Mount(hostFolder, "/home", MountType.ReadWrite);

            if (dockerCredentials.HasValue)
            {
                estateReportingContainer.WithCredential(dockerCredentials.Value.URL, dockerCredentials.Value.UserName, dockerCredentials.Value.Password);
            }

            // Now build and return the container                
            IContainerService builtContainer = estateReportingContainer.Build().Start();

            logger.LogInformation("Estate Reporting Container Started");

            return builtContainer;
        }

        /// <summary>
        /// Setups the event store container.
        /// </summary>
        /// <param name="containerName">Name of the container.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="imageName">Name of the image.</param>
        /// <param name="networkService">The network service.</param>
        /// <param name="hostFolder">The host folder.</param>
        /// <param name="forceLatestImage">if set to <c>true</c> [force latest image].</param>
        /// <returns></returns>
        public static async Task<IContainerService> SetupEventStoreContainer(String containerName,
                                                                 ILogger logger,
                                                                 String imageName,
                                                                 INetworkService networkService,
                                                                 String hostFolder,
                                                                 Boolean forceLatestImage = false)
        {
            logger.LogInformation("About to Start Event Store Container");

            IContainerService eventStoreContainer = new Builder().UseContainer().UseImage(imageName, forceLatestImage).ExposePort(TransactionMobileDockerHelper.EventStoreHttpDockerPort)
                                                                 .ExposePort(TransactionMobileDockerHelper.EventStoreTcpDockerPort).WithName(containerName)
                                                                 .WithEnvironment("EVENTSTORE_RUN_PROJECTIONS=all", "EVENTSTORE_START_STANDARD_PROJECTIONS=true")
                                                                 .UseNetwork(networkService).Mount(hostFolder, "/var/log/eventstore", MountType.ReadWrite).Build().Start();

            await Task.Delay(20000);

            var eventStoreHttpPort = eventStoreContainer.ToHostExposedEndpoint("2113/tcp").Port;

            // Verify the Event Store is running
            await Retry.For(async () =>
                            {
                                String url = $"http://{TransactionMobileDockerHelper.LocalHostAddress}:{eventStoreHttpPort}/ping";

                                HttpClient client = new HttpClient();

                                HttpResponseMessage pingResponse = await client.GetAsync(url).ConfigureAwait(false);
                                pingResponse.StatusCode.ShouldBe(HttpStatusCode.OK);
                            }).ConfigureAwait(false);

            await Retry.For(async () =>
                            {
                                String url = $"http://{TransactionMobileDockerHelper.LocalHostAddress}:{eventStoreHttpPort}/info";
                                HttpClient client = new HttpClient();

                                HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
                                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Authorization", "Basic YWRtaW46Y2hhbmdlaXQ=");

                                HttpResponseMessage infoResponse = await client.SendAsync(requestMessage, CancellationToken.None).ConfigureAwait(false);

                                infoResponse.StatusCode.ShouldBe(HttpStatusCode.OK);
                                String infoData = await infoResponse.Content.ReadAsStringAsync().ConfigureAwait(false);

                                logger.LogInformation(infoData);
                            }).ConfigureAwait(false);

            logger.LogInformation("Event Store Container Started");

            return eventStoreContainer;
        }

        /// <summary>
        /// Setups the security service container.
        /// </summary>
        /// <param name="containerName">Name of the container.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="imageName">Name of the image.</param>
        /// <param name="networkService">The network service.</param>
        /// <param name="hostFolder">The host folder.</param>
        /// <param name="dockerCredentials">The docker credentials.</param>
        /// <param name="forceLatestImage">if set to <c>true</c> [force latest image].</param>
        /// <returns></returns>
        public static IContainerService SetupSecurityServiceContainer(String containerName,
                                                                      ILogger logger,
                                                                      String imageName,
                                                                      INetworkService networkService,
                                                                      String hostFolder,
                                                                      (String URL, String UserName, String Password)? dockerCredentials,
                                                                      Boolean forceLatestImage = false)
        {
            logger.LogInformation("About to Start Security Container");

            List<String> environmentVariables = new List<String>();
            environmentVariables.Add($"ServiceOptions:PublicOrigin=http://{containerName}:{TransactionMobileDockerHelper.SecurityServiceDockerPort}");
            environmentVariables.Add($"ServiceOptions:IssuerUrl=http://{containerName}:{TransactionMobileDockerHelper.SecurityServiceDockerPort}");
            environmentVariables.Add("ASPNETCORE_ENVIRONMENT=IntegrationTest");
            environmentVariables.Add("urls=http://*:5001");

            ContainerBuilder securityServiceContainer = new Builder().UseContainer().WithName(containerName)
                                                                     .WithEnvironment(environmentVariables.ToArray()).UseImage(imageName, forceLatestImage)
                                                                     .ExposePort(TransactionMobileDockerHelper.SecurityServiceDockerPort).UseNetwork(new List<INetworkService>
                                                                                                                                                     {
                                                                                                                                                         networkService
                                                                                                                                                     }.ToArray()).Mount(hostFolder,
                                                                                                                                                                        "/home/txnproc/trace",
                                                                                                                                                                        MountType
                                                                                                                                                                            .ReadWrite);

            if (dockerCredentials.HasValue)
            {
                securityServiceContainer.WithCredential(dockerCredentials.Value.URL, dockerCredentials.Value.UserName, dockerCredentials.Value.Password);
            }

            // Now build and return the container                
            IContainerService builtContainer = securityServiceContainer.Build().Start();
            Thread.Sleep(20000); // This hack is in till health checks implemented :|

            logger.LogInformation("Security Service Container Started");

            return builtContainer;
        }

        /// <summary>
        /// Setups the subscription service container.
        /// </summary>
        /// <param name="containerName">Name of the container.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="imageName">Name of the image.</param>
        /// <param name="networkServices">The network services.</param>
        /// <param name="hostFolder">The host folder.</param>
        /// <param name="dockerCredentials">The docker credentials.</param>
        /// <param name="securityServiceContainerName">Name of the security service container.</param>
        /// <param name="sqlServerContainerName">Name of the SQL server container.</param>
        /// <param name="sqlServerUserName">Name of the SQL server user.</param>
        /// <param name="sqlServerPassword">The SQL server password.</param>
        /// <param name="eventStoreServerId">The event store server identifier.</param>
        /// <param name="clientDetails">The client details.</param>
        /// <param name="forceLatestImage">if set to <c>true</c> [force latest image].</param>
        /// <param name="securityServicePort">The security service port.</param>
        /// <returns></returns>
        public static IContainerService SetupSubscriptionServiceContainer(String containerName,
                                                                          ILogger logger,
                                                                          String imageName,
                                                                          List<INetworkService> networkServices,
                                                                          String hostFolder,
                                                                          (String URL, String UserName, String Password)? dockerCredentials,
                                                                          String securityServiceContainerName,
                                                                          String sqlServerContainerName,
                                                                          String sqlServerUserName,
                                                                          String sqlServerPassword,
                                                                          Guid eventStoreServerId,
                                                                          (String clientId, String clientSecret) clientDetails,
                                                                          Boolean forceLatestImage = false,
                                                                          Int32 securityServicePort = TransactionMobileDockerHelper.SecurityServiceDockerPort)
        {
            logger.LogInformation("About to Start Subscription Service Container");

            List<String> environmentVariables = new List<String>();
            environmentVariables.Add($"AppSettings:SecurityService=http://{securityServiceContainerName}:{securityServicePort}");
            environmentVariables.Add($"AppSettings:EventStoreServerId={eventStoreServerId}");
            environmentVariables.Add($"SecurityConfiguration:Authority=http://{securityServiceContainerName}:{securityServicePort}");
            environmentVariables
                .Add($"ConnectionStrings:SubscriptionService=\"server={sqlServerContainerName};user id={sqlServerUserName};password={sqlServerPassword};database=SubscriptionServiceConfiguration\"");

            ContainerBuilder subscriptionServiceContainer = new Builder().UseContainer().WithName(containerName).WithEnvironment(environmentVariables.ToArray())
                                                                         .UseImage(imageName, forceLatestImage).UseNetwork(networkServices.ToArray())
                                                                         .Mount(hostFolder, "/home", MountType.ReadWrite);

            if (dockerCredentials.HasValue)
            {
                subscriptionServiceContainer.WithCredential(dockerCredentials.Value.URL, dockerCredentials.Value.UserName, dockerCredentials.Value.Password);
            }

            // Now build and return the container                
            IContainerService builtContainer = subscriptionServiceContainer.Build().Start();

            logger.LogInformation("Subscription Service Container Started");

            return builtContainer;
        }

        /// <summary>
        /// Setups the test network.
        /// </summary>
        /// <param name="networkName">Name of the network.</param>
        /// <param name="reuseIfExists">if set to <c>true</c> [reuse if exists].</param>
        /// <returns></returns>
        public static INetworkService SetupTestNetwork(String networkName = null,
                                                       Boolean reuseIfExists = false)
        {
            networkName = string.IsNullOrEmpty(networkName) ? $"testnetwork{Guid.NewGuid()}" : networkName;

            // Build a network
            NetworkBuilder networkService = new Builder().UseNetwork(networkName);

            if (reuseIfExists)
            {
                networkService.ReuseIfExist();
            }

            return networkService.Build();
        }

        /// <summary>
        /// Setups the transaction processor acl container.
        /// </summary>
        /// <param name="containerName">Name of the container.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="imageName">Name of the image.</param>
        /// <param name="networkService">The network service.</param>
        /// <param name="hostFolder">The host folder.</param>
        /// <param name="dockerCredentials">The docker credentials.</param>
        /// <param name="securityServiceContainerName">Name of the security service container.</param>
        /// <param name="transactionProcessorContainerName">Name of the transaction processor container.</param>
        /// <param name="clientDetails">The client details.</param>
        /// <param name="forceLatestImage">if set to <c>true</c> [force latest image].</param>
        /// <param name="securityServicePort">The security service port.</param>
        /// <returns></returns>
        public static IContainerService SetupTransactionProcessorACLContainer(String containerName,
                                                                              ILogger logger,
                                                                              String imageName,
                                                                              INetworkService networkService,
                                                                              String hostFolder,
                                                                              (String URL, String UserName, String Password)? dockerCredentials,
                                                                              String securityServiceContainerName,
                                                                              String transactionProcessorContainerName,
                                                                              (String clientId, String clientSecret) clientDetails,
                                                                              Boolean forceLatestImage = false,
                                                                              Int32 securityServicePort = TransactionMobileDockerHelper.SecurityServiceDockerPort)
        {
            logger.LogInformation("About to Start Transaction Processor ACL Container");

            List<String> environmentVariables = new List<String>();
            environmentVariables.Add($"AppSettings:SecurityService=http://{securityServiceContainerName}:{securityServicePort}");
            environmentVariables.Add($"SecurityConfiguration:Authority=http://{securityServiceContainerName}:{securityServicePort}");
            environmentVariables.Add($"urls=http://*:{TransactionMobileDockerHelper.TransactionProcessorACLDockerPort}");
            environmentVariables.Add($"AppSettings:TransactionProcessorApi=http://{transactionProcessorContainerName}:{TransactionMobileDockerHelper.TransactionProcessorDockerPort}");
            environmentVariables.Add($"AppSettings:ClientId={clientDetails.clientId}");
            environmentVariables.Add($"AppSettings:ClientSecret={clientDetails.clientSecret}");

            ContainerBuilder transactionProcessorACLContainer = new Builder()
                                                                .UseContainer().WithName(containerName).WithEnvironment(environmentVariables.ToArray())
                                                                .UseImage(imageName, forceLatestImage).ExposePort(TransactionMobileDockerHelper.TransactionProcessorACLDockerPort)
                                                                .UseNetwork(new List<INetworkService>
                                                                            {
                                                                                networkService
                                                                            }.ToArray()).Mount(hostFolder, "/home", MountType.ReadWrite);

            if (dockerCredentials.HasValue)
            {
                transactionProcessorACLContainer.WithCredential(dockerCredentials.Value.URL, dockerCredentials.Value.UserName, dockerCredentials.Value.Password);
            }

            // Now build and return the container                
            IContainerService builtContainer =
                transactionProcessorACLContainer.Build().Start();

            logger.LogInformation("Transaction Processor Container ACL Started");

            return builtContainer;
        }

        /// <summary>
        /// Setups the transaction processor container.
        /// </summary>
        /// <param name="containerName">Name of the container.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="imageName">Name of the image.</param>
        /// <param name="networkServices">The network services.</param>
        /// <param name="hostFolder">The host folder.</param>
        /// <param name="dockerCredentials">The docker credentials.</param>
        /// <param name="securityServiceContainerName">Name of the security service container.</param>
        /// <param name="estateManagementContainerName">Name of the estate management container.</param>
        /// <param name="eventStoreContainerName">Name of the event store container.</param>
        /// <param name="clientDetails">The client details.</param>
        /// <param name="testhostContainerName">Name of the testhost container.</param>
        /// <param name="forceLatestImage">if set to <c>true</c> [force latest image].</param>
        /// <param name="securityServicePort">The security service port.</param>
        /// <returns></returns>
        public static IContainerService SetupTransactionProcessorContainer(String containerName,
                                                                           ILogger logger,
                                                                           String imageName,
                                                                           List<INetworkService> networkServices,
                                                                           String hostFolder,
                                                                           (String URL, String UserName, String Password)? dockerCredentials,
                                                                           String securityServiceContainerName,
                                                                           String estateManagementContainerName,
                                                                           String eventStoreContainerName,
                                                                           (String clientId, String clientSecret) clientDetails,
                                                                           String testhostContainerName,
                                                                           Boolean forceLatestImage = false,
                                                                           Int32 securityServicePort = TransactionMobileDockerHelper.SecurityServiceDockerPort)
        {
            logger.LogInformation("About to Start Transaction Processor Container");

            List<String> environmentVariables = new List<String>();
            environmentVariables
                .Add($"EventStoreSettings:ConnectionString=ConnectTo=tcp://admin:changeit@{eventStoreContainerName}:{TransactionMobileDockerHelper.EventStoreTcpDockerPort};VerboseLogging=true;");
            environmentVariables.Add($"AppSettings:SecurityService=http://{securityServiceContainerName}:{securityServicePort}");
            environmentVariables.Add($"AppSettings:EstateManagementApi=http://{estateManagementContainerName}:{TransactionMobileDockerHelper.EstateManagementDockerPort}");
            environmentVariables.Add($"SecurityConfiguration:Authority=http://{securityServiceContainerName}:{securityServicePort}");
            environmentVariables.Add($"urls=http://*:{TransactionMobileDockerHelper.TransactionProcessorDockerPort}");
            environmentVariables.Add($"AppSettings:ClientId={clientDetails.clientId}");
            environmentVariables.Add($"AppSettings:ClientSecret={clientDetails.clientSecret}");

            environmentVariables.Add($"OperatorConfiguration:Safaricom:Url=http://{testhostContainerName}:9000/api/safaricom");

            ContainerBuilder transactionProcessorContainer = new Builder().UseContainer().WithName(containerName).WithEnvironment(environmentVariables.ToArray())
                                                                          .UseImage(imageName, forceLatestImage).ExposePort(TransactionMobileDockerHelper.TransactionProcessorDockerPort)
                                                                          .UseNetwork(networkServices.ToArray()).Mount(hostFolder, "/home", MountType.ReadWrite);

            if (dockerCredentials.HasValue)
            {
                transactionProcessorContainer.WithCredential(dockerCredentials.Value.URL, dockerCredentials.Value.UserName, dockerCredentials.Value.Password);
            }

            // Now build and return the container                
            IContainerService builtContainer = transactionProcessorContainer.Build().Start();

            logger.LogInformation("Transaction Processor Container Started");

            return builtContainer;
        }

        /// <summary>
        /// Starts the SQL container with open connection.
        /// </summary>
        /// <param name="containerName">Name of the container.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="imageName">Name of the image.</param>
        /// <param name="networkService">The network service.</param>
        /// <param name="hostFolder">The host folder.</param>
        /// <param name="dockerCredentials">The docker credentials.</param>
        /// <param name="sqlUserName">Name of the SQL user.</param>
        /// <param name="sqlPassword">The SQL password.</param>
        /// <returns></returns>
        public static IContainerService StartSqlContainerWithOpenConnection(String containerName,
                                                                            ILogger logger,
                                                                            String imageName,
                                                                            INetworkService networkService,
                                                                            String hostFolder,
                                                                            (String URL, String UserName, String Password)? dockerCredentials,
                                                                            String sqlUserName = "sa",
                                                                            String sqlPassword = "thisisalongpassword123!")
        {
            logger.LogInformation("About to start SQL Server Container");
            IContainerService databaseServerContainer = new Builder().UseContainer().WithName(containerName).UseImage(imageName)
                                                                     .WithEnvironment("ACCEPT_EULA=Y", $"SA_PASSWORD={sqlPassword}").ExposePort(1433)
                                                                     .UseNetwork(networkService).KeepContainer().KeepRunning().ReuseIfExists().Build().Start();

            logger.LogInformation("SQL Server Container Started");

            logger.LogInformation("About to SQL Server Container is running");
            IPEndPoint sqlServerEndpoint = null;
            Retry.For(async () => { sqlServerEndpoint = databaseServerContainer.ToHostExposedEndpoint("1433/tcp"); }).Wait();

            // Try opening a connection
            Int32 maxRetries = 10;
            Int32 counter = 1;

            String localhostaddress = Environment.GetEnvironmentVariable("localhostaddress");
            if (String.IsNullOrEmpty(localhostaddress))
            {
                localhostaddress = "192.168.1.67";
            }

            String server = localhostaddress;
            //String database = "SubscriptionServiceConfiguration";
            String database = "master";
            String user = sqlUserName;
            String password = sqlPassword;
            String port = sqlServerEndpoint.Port.ToString();

            String connectionString = $"server={server},{port};user id={user}; password={password}; database={database};";
            logger.LogInformation($"Connection String {connectionString}");
            SqlConnection connection = new SqlConnection(connectionString);

            while (counter <= maxRetries)
            {
                try
                {
                    logger.LogInformation($"Database Connection Attempt {counter}");

                    connection.Open();

                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = "select * from sys.databases";
                    SqlDataReader dataReader = command.ExecuteReader(CommandBehavior.Default);
                    while (dataReader.Read())
                    {
                        Console.WriteLine(dataReader.GetValue(0));
                    }

                    logger.LogInformation("Connection Opened");

                    connection.Close();
                    logger.LogInformation("SQL Server Container Running");
                    break;
                }
                catch (SqlException ex)
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }

                    logger.LogError(ex);
                    Thread.Sleep(20000);
                }
                finally
                {
                    counter++;
                }
            }

            // Create the SS database here
            // Read the SQL File
            String sqlToExecute = null;
            String executableLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            String sqlFileLocation = Path.Combine(executableLocation, "DbScripts");
            IOrderedEnumerable<String> files = Directory.GetFiles(sqlFileLocation).OrderBy(x => x);

            try
            {
                SqlConnection ssconnection = new SqlConnection(connectionString);
                ssconnection.Open();
                SqlCommand sscommand = ssconnection.CreateCommand();
                sscommand.CommandText = "CREATE DATABASE SubscriptionServiceConfiguration";
                sscommand.ExecuteNonQuery();

                sscommand.CommandText = "USE SubscriptionServiceConfiguration";
                sscommand.ExecuteNonQuery();

                foreach (String file in files)
                {
                    using (StreamReader sr = new StreamReader(file))
                    {
                        sqlToExecute = sr.ReadToEnd();
                    }

                    sscommand.CommandText = sqlToExecute;
                    sscommand.ExecuteNonQuery();
                }

                connection.Close();

                Console.WriteLine("SS Database Created");
            }
            catch (Exception e)
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }

                Console.WriteLine(e);
                throw;
            }

            return databaseServerContainer;
        }

        #endregion

        #region Others

        /// <summary>
        /// The estate management docker port
        /// </summary>
        public const Int32 EstateManagementDockerPort = 5000;

        /// <summary>
        /// The estate reporting docker port
        /// </summary>
        public const Int32 EstateReportingDockerPort = 5005;

        /// <summary>
        /// The event store HTTP docker port
        /// </summary>
        public const Int32 EventStoreHttpDockerPort = 2113;

        /// <summary>
        /// The event store TCP docker port
        /// </summary>
        public const Int32 EventStoreTcpDockerPort = 1113;

        /// <summary>
        /// The security service docker port
        /// </summary>
        public const Int32 SecurityServiceDockerPort = 5001;

        /// <summary>
        /// The transaction processor acl docker port
        /// </summary>
        public const Int32 TransactionProcessorACLDockerPort = 5003;

        /// <summary>
        /// The transaction processor docker port
        /// </summary>
        public const Int32 TransactionProcessorDockerPort = 5002;

        #endregion
    }
}
