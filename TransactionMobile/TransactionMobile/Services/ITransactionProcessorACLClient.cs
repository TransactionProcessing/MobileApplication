using System;
using System.Collections.Generic;
using System.Text;

namespace TransactionMobile.Services
{
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading;
    using System.Threading.Tasks;
    using ClientProxyBase;
    using Newtonsoft.Json;
    using TransactionProcessorACL.DataTransferObjects;
    using TransactionProcessorACL.DataTransferObjects.Responses;

    public interface ITransactionProcessorACLClient
    {
        /// <summary>
        /// Performs the logon transaction.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <param name="logonTransactionRequest">The logon transaction request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<LogonTransactionResponseMessage> PerformLogonTransaction(String accessToken,
                                                                      LogonTransactionRequestMessage logonTransactionRequest,
                                                                      CancellationToken cancellationToken);

    }

    public class TransactionProcessorACLClient : ClientProxyBase, ITransactionProcessorACLClient
    {
        private readonly Func<String, String> BaseAddressResolver;

        public TransactionProcessorACLClient(Func<String, String> baseAddressResolver,
                                             HttpClient httpClient) : base(httpClient)
        {
            this.BaseAddressResolver = baseAddressResolver;

            // Add the API version header
            this.HttpClient.DefaultRequestHeaders.Add("api-version", "1.0");
        }

        private String BuildRequestUrl(String route)
        {
            String baseAddress = this.BaseAddressResolver("TransactionProcessorACL");

            String requestUri = $"{baseAddress}{route}";

            return requestUri;
        }

        /// <summary>
        /// Performs the logon transaction.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <param name="logonTransactionRequest">The logon transaction request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<LogonTransactionResponseMessage> PerformLogonTransaction(String accessToken,
                                                                                   LogonTransactionRequestMessage logonTransactionRequest,
                                                                                   CancellationToken cancellationToken)
        {
            LogonTransactionResponseMessage response = null;
            String requestUri = this.BuildRequestUrl("/api/transactions");

            try
            {
                String requestSerialised = JsonConvert.SerializeObject(logonTransactionRequest, new JsonSerializerSettings
                                                                                                {
                                                                                                    TypeNameHandling = TypeNameHandling.All
                                                                                                });

                StringContent httpContent = new StringContent(requestSerialised, Encoding.UTF8, "application/json");

                // Add the access token to the client headers
                this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Make the Http Call here
                HttpResponseMessage httpResponse = await this.HttpClient.PostAsync(requestUri, httpContent, cancellationToken);

                // Process the response
                String content = await this.HandleResponse(httpResponse, cancellationToken);

                // call was successful so now deserialise the body to the response object
                response = JsonConvert.DeserializeObject<LogonTransactionResponseMessage>(content);
            }
            catch(Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception("Error posting logon transaction.", ex);

                throw exception;
            }

            return response;
        }
    }
}
