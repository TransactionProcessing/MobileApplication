using System;
using System.Collections.Generic;

namespace TransactionMobile.Services
{
    using System.Threading;
    using System.Threading.Tasks;
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

        /// <summary>
        /// Performs the logon transaction.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <param name="saleTransactionRequest">The sale transaction request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<SaleTransactionResponseMessage> PerformSaleTransaction(String accessToken,
                                                                      SaleTransactionRequestMessage saleTransactionRequest,
                                                                      CancellationToken cancellationToken);

        /// <summary>
        /// Performs the reconcilaition.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <param name="reconciliationRequest">The reconciliation request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<ReconciliationResponseMessage> PerformReconcilaition(String accessToken,
                                                                    ReconciliationRequestMessage reconciliationRequest,
                                                                    CancellationToken cancellationToken);
    }
}
