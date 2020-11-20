using System;
using System.Collections.Generic;
using System.Text;

namespace TransactionMobile.Services
{
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using ClientProxyBase;
    using Common;
    using Newtonsoft.Json;

    public interface IConfigurationServiceClient
    {
        Task<Configuration> GetConfiguration(String deviceIdentifier,
                                             CancellationToken cancellationToken);
    }

    public class ConfigurationServiceClient : ClientProxyBase, IConfigurationServiceClient
    {
        private readonly Func<String, String> BaseAddressResolver;

        public ConfigurationServiceClient(Func<String, String> baseAddressResolver,
                                          HttpClient httpClient) : base(httpClient)
        {
            this.BaseAddressResolver = baseAddressResolver;
        }

        /// <summary>
        /// Builds the request URL.
        /// </summary>
        /// <param name="route">The route.</param>
        /// <returns></returns>
        private String BuildRequestUrl(String route)
        {
            String baseAddress = this.BaseAddressResolver("ConfigServiceUrl");

            String requestUri = $"{baseAddress}{route}";

            return requestUri;
        }

        public async Task<Configuration> GetConfiguration(String deviceIdentifier,
                                                          CancellationToken cancellationToken)
        {
            Console.WriteLine($"Getting config for device [{deviceIdentifier}]");
            Configuration response = null;
            String requestUri = this.BuildRequestUrl($"/configuration/{deviceIdentifier}");

            Console.WriteLine($"requestUri: {requestUri}");
            try
            {
                // Make the Http Call here
                HttpResponseMessage httpResponse = await this.HttpClient.GetAsync(requestUri, cancellationToken);
                    
                // Process the response
                String content = await this.HandleResponse(httpResponse, cancellationToken);

                Console.WriteLine($"content: {content}");

                // call was successful so now deserialise the body to the response object
                response = JsonConvert.DeserializeObject<Configuration>(content);
            }
            catch (Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error getting configuration for device Id {deviceIdentifier}.", ex);

                throw exception;
            }

            return response;
        }

        protected override async Task<String> HandleResponse(HttpResponseMessage responseMessage,
                                                       CancellationToken cancellationToken)
        {
            String content = await responseMessage.Content.ReadAsStringAsync();

            if (responseMessage.StatusCode == HttpStatusCode.NotFound)
            {
                // No error as maybe running under CI (which has no internet)
                return content;
            }
            
            return await base.HandleResponse(responseMessage, cancellationToken);
        }
    }
}
