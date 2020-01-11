namespace TransactionMobile.IntegrationTests
{
    using System;
    using Common;
    using Ductus.FluentDocker.Services;
    using Shouldly;
    using TechTalk.SpecFlow;

    [Binding]
    public class Setup
    {
        public static IContainerService DatabaseServerContainer;
        private static String DbConnectionStringWithNoDatabase;
        public static INetworkService DatabaseServerNetwork;

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
            return $"{Setup.DbConnectionStringWithNoDatabase} database={databaseName};";
        }
    }
}