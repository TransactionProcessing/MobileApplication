namespace TransactionMobile.IntegrationTests
{
    using System;
    using Common;
    using Ductus.FluentDocker.Services;
    using Ductus.FluentDocker.Services.Extensions;
    using Shouldly;
    using TechTalk.SpecFlow;

    [Binding]
    public class Setup
    {
        public static IContainerService DatabaseServerContainer;
        private static String DbConnectionStringWithNoDatabase;
        public static INetworkService DatabaseServerNetwork;
        public static String SqlServerContainerName = "shareddatabasesqlserver";
        public const String SqlUserName = "sa";

        public const String SqlPassword = "thisisalongpassword123!";

        [BeforeTestRun]
        protected static void GlobalSetup()
        {
            ShouldlyConfiguration.DefaultTaskTimeout = TimeSpan.FromMinutes(1);

            //(String, String, String) dockerCredentials = ("https://www.docker.com", "stuartferguson", "Sc0tland");

            //// Setup a network for the DB Server
            //Setup.DatabaseServerNetwork = DockerHelper.SetupTestNetwork("sharednetwork", true);

            //// Start the Database Server here
            //Setup.DbConnectionStringWithNoDatabase = DockerHelper.StartSqlContainerWithOpenConnection("shareddatabasesqlserver",
            //                                                                                          "stuartferguson/subscriptionservicedatabasesqlserver",
            //                                                                                          Setup.DatabaseServerNetwork,
            //                                                                                          "",
            //                                                                                          dockerCredentials);
        }

        public static String GetConnectionString(String databaseName)
        {
            return $"server={Setup.DatabaseServerContainer.Name};database={databaseName};user id={Setup.SqlUserName};password={Setup.SqlPassword}";
        }

        public static String GetLocalConnectionString(String databaseName)
        {
            Int32 databaseHostPort = Setup.DatabaseServerContainer.ToHostExposedEndpoint("1433/tcp").Port;

            String localhostaddress = Environment.GetEnvironmentVariable("localhostaddress");
            if (String.IsNullOrEmpty(localhostaddress))
            {
                localhostaddress = "192.168.1.67";
            }

            return $"server={localhostaddress},{databaseHostPort};database={databaseName};user id={Setup.SqlUserName};password={Setup.SqlPassword}";
        }
    }
}