namespace TransactionMobile.Services
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using ClientProxyBase;
    using Newtonsoft.Json;
    using TransactionProcessorACL.DataTransferObjects;
    using TransactionProcessorACL.DataTransferObjects.Responses;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="ClientProxyBase.ClientProxyBase" />
    /// <seealso cref="TransactionMobile.Services.ITransactionProcessorACLClient" />
    public class TransactionProcessorACLClient : ClientProxyBase, ITransactionProcessorACLClient
    {
        #region Fields

        /// <summary>
        /// The base address resolver
        /// </summary>
        private readonly Func<String, String> BaseAddressResolver;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionProcessorACLClient"/> class.
        /// </summary>
        /// <param name="baseAddressResolver">The base address resolver.</param>
        /// <param name="httpClient">The HTTP client.</param>
        public TransactionProcessorACLClient(Func<String, String> baseAddressResolver,
                                             HttpClient httpClient) : base(httpClient)
        {
            this.BaseAddressResolver = baseAddressResolver;

            // Add the API version header
            this.HttpClient.DefaultRequestHeaders.Add("api-version", "1.0");
        }

        #endregion

        #region Methods

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
                String requestSerialised = JsonConvert.SerializeObject(logonTransactionRequest,
                                                                       new JsonSerializerSettings
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

        /// <summary>
        /// Performs the logon transaction.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <param name="saleTransactionRequest"></param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<SaleTransactionResponseMessage> PerformSaleTransaction(String accessToken,
                                                                                 SaleTransactionRequestMessage saleTransactionRequest,
                                                                                 CancellationToken cancellationToken)
        {
            SaleTransactionResponseMessage response = null;
            String requestUri = this.BuildRequestUrl("/api/transactions");

            try
            {
                String requestSerialised = JsonConvert.SerializeObject(saleTransactionRequest,
                                                                       new JsonSerializerSettings
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
                response = JsonConvert.DeserializeObject<SaleTransactionResponseMessage>(content);
            }
            catch(Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception("Error posting sale transaction.", ex);

                throw exception;
            }

            return response;
        }

        /// <summary>
        /// Builds the request URL.
        /// </summary>
        /// <param name="route">The route.</param>
        /// <returns></returns>
        private String BuildRequestUrl(String route)
        {
            String baseAddress = this.BaseAddressResolver("TransactionProcessorACL");

            String requestUri = $"{baseAddress}{route}";

            return requestUri;
        }

        #endregion
    }
}