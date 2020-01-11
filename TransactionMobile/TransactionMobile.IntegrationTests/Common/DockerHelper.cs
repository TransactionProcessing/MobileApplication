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
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using Ductus.FluentDocker.Common;
    using Ductus.FluentDocker.Model.Builders;
    using Ductus.FluentDocker.Services.Extensions;
    using EstateManagement.Client;
    using NUnit.Framework.Internal;
    using SecurityService.Client;

    public abstract class DockerHelper
    {
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
        /// <returns></returns>
        public static IContainerService SetupEstateManagementContainer(String containerName,
                                                                       //ILogger logger,
                                                                       String imageName,
                                                                       List<INetworkService> networkServices,
                                                                       String hostFolder,
                                                                       (String URL, String UserName, String Password)? dockerCredentials,
                                                                       String securityServiceContainerName,
                                                                       String eventStoreContainerName,
                                                                       (String clientId, String clientSecret) clientDetails)
        {
            //logger.LogInformation("About to Start Estate Management Container");

            List<String> environmentVariables = new List<String>();
            environmentVariables
                .Add($"EventStoreSettings:ConnectionString=ConnectTo=tcp://admin:changeit@{eventStoreContainerName}:{DockerHelper.EventStoreTcpDockerPort};VerboseLogging=true;");
            environmentVariables.Add($"AppSettings:SecurityService=http://{securityServiceContainerName}:{DockerHelper.SecurityServiceDockerPort}");
            environmentVariables.Add($"SecurityConfiguration:Authority=http://{securityServiceContainerName}:{DockerHelper.SecurityServiceDockerPort}");
            environmentVariables.Add($"urls=http://*:{DockerHelper.EstateManagementDockerPort}");

            ContainerBuilder estateManagementContainer = new Builder().UseContainer().WithName(containerName).WithEnvironment(environmentVariables.ToArray())
                                                                      .UseImage(imageName).ExposePort(DockerHelper.EstateManagementDockerPort)
                                                                      .UseNetwork(networkServices.ToArray()).Mount(hostFolder, "/home", MountType.ReadWrite);

            if (dockerCredentials.HasValue)
            {
                estateManagementContainer.WithCredential(dockerCredentials.Value.URL, dockerCredentials.Value.UserName, dockerCredentials.Value.Password);
            }

            // Now build and return the container                
            IContainerService builtContainer = estateManagementContainer.Build().Start();//.WaitForPort($"{DockerHelper.EstateManagementDockerPort}/tcp", 30000);

            //logger.LogInformation("Estate Management Container Started");

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
        /// <returns></returns>
        public static IContainerService SetupEventStoreContainer(String containerName,
                                                                 //ILogger logger,
                                                                 String imageName,
                                                                 INetworkService networkService,
                                                                 String hostFolder)
        {
            //logger.LogInformation("About to Start Event Store Container");

            IContainerService eventStoreContainer = new Builder().UseContainer().UseImage(imageName).ExposePort(DockerHelper.EventStoreHttpDockerPort)
                                                                 .ExposePort(DockerHelper.EventStoreTcpDockerPort).WithName(containerName)
                                                                 .WithEnvironment("EVENTSTORE_RUN_PROJECTIONS=all", "EVENTSTORE_START_STANDARD_PROJECTIONS=true")
                                                                 .UseNetwork(networkService).Mount(hostFolder, "/var/log/eventstore", MountType.ReadWrite).Build()
                                                                 .Start();//.WaitForPort("2113/tcp", 30000);

            //logger.LogInformation("Event Store Container Started");

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
        /// <returns></returns>
        public static IContainerService SetupSecurityServiceContainer(String containerName,
                                                                      //ILogger logger,
                                                                      String imageName,
                                                                      INetworkService networkService,
                                                                      String hostFolder,
                                                                      (String URL, String UserName, String Password)? dockerCredentials)
        {
            //logger.LogInformation("About to Start Security Container");

            List<String> environmentVariables = new List<String>();
            environmentVariables.Add($"ServiceOptions:PublicOrigin=http://{containerName}:{DockerHelper.SecurityServiceDockerPort}");
            environmentVariables.Add($"ServiceOptions:IssuerUrl=http://{containerName}:{DockerHelper.SecurityServiceDockerPort}");
            environmentVariables.Add("ASPNETCORE_ENVIRONMENT=IntegrationTest");
            environmentVariables.Add("urls=http://*:5001");

            ContainerBuilder securityServiceContainer = new Builder().UseContainer().WithName(containerName)
                                                                     .WithEnvironment(environmentVariables.ToArray()).UseImage(imageName)
                                                                     .ExposePort(DockerHelper.SecurityServiceDockerPort).UseNetwork(new List<INetworkService>
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
            IContainerService builtContainer = securityServiceContainer.Build().Start();//.WaitForPort("5001/tcp", 30000);
            Thread.Sleep(20000); // This hack is in till health checks implemented :|

            //logger.LogInformation("Security Service Container Started");

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
        /// <returns></returns>
        public static IContainerService SetupTransactionProcessorACLContainer(String containerName,
                                                                              //ILogger logger,
                                                                              String imageName,
                                                                              INetworkService networkService,
                                                                              String hostFolder,
                                                                              (String URL, String UserName, String Password)? dockerCredentials,
                                                                              String securityServiceContainerName,
                                                                              String transactionProcessorContainerName,
                                                                              (String clientId, String clientSecret) clientDetails)
        {
            //logger.LogInformation("About to Start Transaction Processor ACL Container");

            List<String> environmentVariables = new List<String>();
            environmentVariables.Add($"AppSettings:SecurityService=http://{securityServiceContainerName}:{DockerHelper.SecurityServiceDockerPort}");
            environmentVariables.Add($"SecurityConfiguration:Authority=http://{securityServiceContainerName}:{DockerHelper.SecurityServiceDockerPort}");
            environmentVariables.Add($"urls=http://*:{DockerHelper.TransactionProcessorACLDockerPort}");
            environmentVariables.Add($"AppSettings:TransactionProcessorApi=http://{transactionProcessorContainerName}:{DockerHelper.TransactionProcessorDockerPort}");
            environmentVariables.Add($"AppSettings:ClientId={clientDetails.clientId}");
            environmentVariables.Add($"AppSettings:ClientSecret={clientDetails.clientSecret}");

            ContainerBuilder transactionProcessorACLContainer = new Builder()
                                                                .UseContainer().WithName(containerName).WithEnvironment(environmentVariables.ToArray())
                                                                .UseImage(imageName).ExposePort(DockerHelper.TransactionProcessorACLDockerPort)
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
                transactionProcessorACLContainer.Build().Start();//.WaitForPort($"{DockerHelper.TransactionProcessorACLDockerPort}/tcp", 30000);

            //logger.LogInformation("Transaction Processor Container ACL Started");

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
        /// <param name="eventStoreContainerName">Name of the event store container.</param>
        /// <param name="clientDetails">The client details.</param>
        /// <returns></returns>
        public static IContainerService SetupTransactionProcessorContainer(String containerName,
                                                                           //ILogger logger,
                                                                           String imageName,
                                                                           List<INetworkService> networkServices,
                                                                           String hostFolder,
                                                                           (String URL, String UserName, String Password)? dockerCredentials,
                                                                           String securityServiceContainerName,
                                                                           String eventStoreContainerName,
                                                                           (String clientId, String clientSecret) clientDetails)
        {
            //logger.LogInformation("About to Start Transaction Processor Container");

            List<String> environmentVariables = new List<String>();
            environmentVariables
                .Add($"EventStoreSettings:ConnectionString=ConnectTo=tcp://admin:changeit@{eventStoreContainerName}:{DockerHelper.EventStoreTcpDockerPort};VerboseLogging=true;");
            environmentVariables.Add($"AppSettings:SecurityService=http://{securityServiceContainerName}:{DockerHelper.SecurityServiceDockerPort}");
            environmentVariables.Add($"SecurityConfiguration:Authority=http://{securityServiceContainerName}:{DockerHelper.SecurityServiceDockerPort}");
            environmentVariables.Add($"urls=http://*:{DockerHelper.TransactionProcessorDockerPort}");

            ContainerBuilder transactionProcessorContainer = new Builder().UseContainer().WithName(containerName).WithEnvironment(environmentVariables.ToArray())
                                                                          .UseImage(imageName).ExposePort(DockerHelper.TransactionProcessorDockerPort)
                                                                          .UseNetwork(networkServices.ToArray()).Mount(hostFolder, "/home", MountType.ReadWrite);

            if (dockerCredentials.HasValue)
            {
                transactionProcessorContainer.WithCredential(dockerCredentials.Value.URL, dockerCredentials.Value.UserName, dockerCredentials.Value.Password);
            }

            // Now build and return the container                
            IContainerService builtContainer = transactionProcessorContainer.Build().Start();//.WaitForPort($"{DockerHelper.TransactionProcessorDockerPort}/tcp", 30000);

            //logger.LogInformation("Transaction Processor Container Started");

            return builtContainer;
        }

        /// <summary>
        /// Starts the containers for scenario run.
        /// </summary>
        /// <param name="scenarioName">Name of the scenario.</param>
        /// <returns></returns>
        public abstract Task StartContainersForScenarioRun(String scenarioName);

        /// <summary>
        /// Starts the SQL container with open connection.
        /// </summary>
        /// <param name="containerName">Name of the container.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="imageName">Name of the image.</param>
        /// <param name="networkService">The network service.</param>
        /// <param name="hostFolder">The host folder.</param>
        /// <param name="dockerCredentials">The docker credentials.</param>
        /// <returns></returns>
        public static String StartSqlContainerWithOpenConnection(String containerName,
                                                                 //ILogger logger,
                                                                 String imageName,
                                                                 INetworkService networkService,
                                                                 String hostFolder,
                                                                 (String URL, String UserName, String Password)? dockerCredentials)
        {
            IContainerService databaseServerContainer = new Builder().UseContainer().WithName(containerName).UseImage(imageName)
                                                                     .WithEnvironment("ACCEPT_EULA=Y", "SA_PASSWORD=thisisalongpassword123!").ExposePort(1433)
                                                                     .UseNetwork(networkService).KeepContainer().KeepRunning().ReuseIfExists().Build().Start();
                                                                     //.WaitForPort("1433/tcp", 30000);

            IPEndPoint sqlServerEndpoint = databaseServerContainer.ToHostExposedEndpoint("1433/tcp");

            // Try opening a connection
            Int32 maxRetries = 10;
            Int32 counter = 1;

            String server = "127.0.0.1";
            String database = "SubscriptionServiceConfiguration";
            String user = "sa";
            String password = "thisisalongpassword123!";
            String port = sqlServerEndpoint.Port.ToString();

            String connectionString = $"server={server},{port};user id={user}; password={password}; database={database};";

            SqlConnection connection = new SqlConnection(connectionString);

            using (StreamWriter sw = new StreamWriter("C:\\Temp\\testlog.log", true))
            {
                while (counter <= maxRetries)
                {
                    try
                    {
                        sw.WriteLine($"Attempt {counter}");
                        sw.WriteLine(DateTime.Now);

                        connection.Open();

                        SqlCommand command = connection.CreateCommand();
                        command.CommandText = "SELECT * FROM EventStoreServers";
                        command.ExecuteNonQuery();

                        sw.WriteLine("Connection Opened");

                        connection.Close();

                        break;
                    }
                    catch (SqlException ex)
                    {
                        if (connection.State == ConnectionState.Open)
                        {
                            connection.Close();
                        }

                        sw.WriteLine(ex);
                        Thread.Sleep(20000);
                    }
                    finally
                    {
                        counter++;
                    }
                }
            }

            return $"server={containerName};user id={user}; password={password};";
        }

        /// <summary>
        /// Stops the containers for scenario run.
        /// </summary>
        /// <returns></returns>
        public abstract Task StopContainersForScenarioRun();

        #endregion

        #region Others

        /// <summary>
        /// The estate management docker port
        /// </summary>
        public const Int32 EstateManagementDockerPort = 5000;

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

    public class TransactionMobileDockerHelper : DockerHelper
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
        /// The security service base address
        /// </summary>
        public String SecurityServiceBaseAddress;

        /// <summary>
        /// The transaction processor acl
        /// </summary>
        public String TransactionProcessorACLBaseAddress;

        /// <summary>
        /// The test identifier
        /// </summary>
        public Guid TestId;
        
        /// <summary>
        /// The containers
        /// </summary>
        protected List<IContainerService> Containers;

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

        /// <summary>
        /// The transaction processor port
        /// </summary>
        protected Int32 TransactionProcessorPort;

        /// <summary>
        /// The logger
        /// </summary>
        //private readonly NlogLogger Logger;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DockerHelper"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public TransactionMobileDockerHelper()
        {
            //this.Logger = logger;
            this.Containers = new List<IContainerService>();
            this.TestNetworks = new List<INetworkService>();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Starts the containers for scenario run.
        /// </summary>
        /// <param name="scenarioName">Name of the scenario.</param>
        public override async Task StartContainersForScenarioRun(String scenarioName)
        {
            String traceFolder = FdOs.IsWindows() ? $"D:\\home\\txnproc\\trace\\{scenarioName}" : $"//home//txnproc//trace//{scenarioName}";

            Logging.Enabled();

            Guid testGuid = Guid.NewGuid();
            this.TestId = testGuid;

            //this.Logger.LogInformation($"Test Id is {testGuid}");

            // Setup the container names
            String securityServiceContainerName = $"securityservice{testGuid:N}";
            String estateManagementApiContainerName = $"estate{testGuid:N}";
            String transactionProcessorContainerName = $"txnprocessor{testGuid:N}";
            String transactionProcessorACLContainerName = $"txnprocessoracl{testGuid:N}";
            String eventStoreContainerName = $"eventstore{testGuid:N}";

            (String, String, String) dockerCredentials = ("https://www.docker.com", "stuartferguson", "Sc0tland");

            INetworkService testNetwork = DockerHelper.SetupTestNetwork();
            this.TestNetworks.Add(testNetwork);
            IContainerService eventStoreContainer =
                DockerHelper.SetupEventStoreContainer(eventStoreContainerName, "eventstore/eventstore:release-5.0.2", testNetwork, traceFolder);

            IContainerService estateManagementContainer = DockerHelper.SetupEstateManagementContainer(estateManagementApiContainerName,
                                                                                                      "stuartferguson/estatemanagement",
                                                                                                      new List<INetworkService>
                                                                                                      {
                                                                                                          testNetwork
                                                                                                      },
                                                                                                      traceFolder,
                                                                                                      dockerCredentials,
                                                                                                      securityServiceContainerName,
                                                                                                      eventStoreContainerName,
                                                                                                      (null, null));

            IContainerService securityServiceContainer = DockerHelper.SetupSecurityServiceContainer(securityServiceContainerName,
                                                                                                    "stuartferguson/securityservice",
                                                                                                    testNetwork,
                                                                                                    traceFolder,
                                                                                                    dockerCredentials);

            IContainerService transactionProcessorContainer = DockerHelper.SetupTransactionProcessorContainer(transactionProcessorContainerName,
                                                                                                              "stuartferguson/transactionprocessor",
                                                                                                              new List<INetworkService>
                                                                                                              {
                                                                                                                  testNetwork
                                                                                                              },
                                                                                                              traceFolder,
                                                                                                              dockerCredentials,
                                                                                                              securityServiceContainerName,
                                                                                                              eventStoreContainerName,
                                                                                                              (null, null));

            IContainerService transactionProcessorACLContainer = DockerHelper.SetupTransactionProcessorACLContainer(transactionProcessorACLContainerName,
                                                                                                                    "stuartferguson/transactionprocessoracl",
                                                                                                                    testNetwork,
                                                                                                                    traceFolder,
                                                                                                                    dockerCredentials,
                                                                                                                    securityServiceContainerName,
                                                                                                                    transactionProcessorContainerName,
                                                                                                                    ("serviceClient", "Secret1"));

            this.Containers.AddRange(new List<IContainerService>
                                     {
                                         eventStoreContainer,
                                         estateManagementContainer,
                                         securityServiceContainer,
                                         transactionProcessorContainer,
                                         transactionProcessorACLContainer
                                     });

            // Cache the ports
            this.EstateManagementApiPort = estateManagementContainer.ToHostExposedEndpoint("5000/tcp").Port;
            this.SecurityServicePort = securityServiceContainer.ToHostExposedEndpoint("5001/tcp").Port;
            this.EventStoreHttpPort = eventStoreContainer.ToHostExposedEndpoint("2113/tcp").Port;
            this.TransactionProcessorPort = transactionProcessorContainer.ToHostExposedEndpoint("5002/tcp").Port;
            this.TransactionProcessorACLPort = transactionProcessorACLContainer.ToHostExposedEndpoint("5003/tcp").Port;

            // Setup the base address resolvers
            String localhostaddress = Environment.GetEnvironmentVariable("localhostaddress");
            if (String.IsNullOrEmpty(localhostaddress))
            {
                localhostaddress = "127.0.0.1";
            }
            String EstateManagementBaseAddressResolver(String api) => $"http://{localhostaddress}:{this.EstateManagementApiPort}";
            String SecurityServiceBaseAddressResolver(String api) => $"http://{localhostaddress}:{this.SecurityServicePort}";
            String TransactionProcessorAclBaseAddressResolver(String api) => $"http://{localhostaddress}:{this.TransactionProcessorACLPort}";

            HttpClient httpClient = new HttpClient();
            this.EstateClient = new EstateClient(EstateManagementBaseAddressResolver, httpClient);
            this.SecurityServiceClient = new SecurityServiceClient(SecurityServiceBaseAddressResolver, httpClient);
            this.SecurityServiceBaseAddress = SecurityServiceBaseAddressResolver(String.Empty);
            
            this.HttpClient = new HttpClient();
            this.HttpClient.BaseAddress = new Uri(TransactionProcessorAclBaseAddressResolver(string.Empty));
            this.TransactionProcessorACLBaseAddress = TransactionProcessorAclBaseAddressResolver(String.Empty);
        }

        /// <summary>
        /// Stops the containers for scenario run.
        /// </summary>
        public override async Task StopContainersForScenarioRun()
        {
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
    }
}
