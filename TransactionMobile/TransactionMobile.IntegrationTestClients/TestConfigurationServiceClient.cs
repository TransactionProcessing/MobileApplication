using System;
using System.Collections.Generic;
using System.Text;

namespace TransactionMobile.IntegrationTestClients
{
    using System.Security.Cryptography;
    using System.Threading;
    using System.Threading.Tasks;
    using Clients;

    public class TestConfigurationServiceClient : IConfigurationServiceClient
    {
        public async Task<Configuration> GetConfiguration(String deviceIdentifier,
                                                          CancellationToken cancellationToken)
        {
            return new Configuration
                   {
                       EnableAutoUpdates = false,
                   };
        }

        public async Task PostDiagnosticLogs(String deviceIdentifier,
                                             List<LogMessage> logMessages,
                                             CancellationToken cancellationToken)
        {
            return;
        }
    }
}
