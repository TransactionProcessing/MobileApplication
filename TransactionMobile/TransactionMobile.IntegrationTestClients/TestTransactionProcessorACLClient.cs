namespace TransactionMobile.IntegrationTestClients
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Clients;
    using TransactionProcessorACL.DataTransferObjects;
    using TransactionProcessorACL.DataTransferObjects.Responses;

    public class TestTransactionProcessorACLClient : ITransactionProcessorACLClient
    {
        private List<Merchant> Merchants;

        public TestTransactionProcessorACLClient()
        {
            this.Merchants = new List<Merchant>();
        }

        public void UpdateTestMerchant(Merchant merchant)
        {
            Boolean alreadyExists = this.Merchants.Any(m => m.MerchantId == merchant.MerchantId);
            if (alreadyExists == true)
            {
                // Overwrite the merchant
                Merchant existingMerchant = this.Merchants.Single(m => m.MerchantId == merchant.MerchantId);
                this.Merchants.Remove(existingMerchant);
                this.Merchants.Add(merchant);
                Console.WriteLine($"Merchant {merchant.MerchantName} updated");
            }
            else
            {
                this.Merchants.Add(merchant);
                Console.WriteLine($"Merchant {merchant.MerchantName} added");
            }
        }

        public async Task<LogonTransactionResponseMessage> PerformLogonTransaction(String accessToken,
                                                                                   LogonTransactionRequestMessage logonTransactionRequest,
                                                                                   CancellationToken cancellationToken)
        {

            String[] splitToken = accessToken.Split('|');
            
            // TODO: Validate the merchant

            return new LogonTransactionResponseMessage
                   {
                       ResponseCode = "0000",
                       EstateId = Guid.Parse(splitToken[0]),
                       MerchantId = Guid.Parse(splitToken[1])
                   };
        }

        public async Task<SaleTransactionResponseMessage> PerformSaleTransaction(String accessToken,
                                                                                 SaleTransactionRequestMessage saleTransactionRequest,
                                                                                 CancellationToken cancellationToken)
        {
            String[] splitToken = accessToken.Split('|');

            saleTransactionRequest.AdditionalRequestMetaData.TryGetValue("Amount", out String amount);

            if (amount == "1000")
            {
                return new SaleTransactionResponseMessage
                       {
                           ResponseCode = "1000",
                           EstateId = Guid.Parse(splitToken[0]),
                           MerchantId = Guid.Parse(splitToken[1])
                       };
            }

            return new SaleTransactionResponseMessage
                   {
                       ResponseCode = "0000",
                       EstateId = Guid.Parse(splitToken[0]),
                       MerchantId = Guid.Parse(splitToken[1])
                   };
        }

        public async Task<ReconciliationResponseMessage> PerformReconcilaition(String accessToken,
                                                                               ReconciliationRequestMessage reconciliationRequest,
                                                                               CancellationToken cancellationToken)
        {
            String[] splitToken = accessToken.Split('|');

            return new ReconciliationResponseMessage
                   {
                       ResponseCode = "0000",
                       EstateId = Guid.Parse(splitToken[0]),
                       MerchantId = Guid.Parse(splitToken[1])
                   };
        }
    }
}