namespace TransactionMobile.IntegrationTestClients
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using EstateManagement.Client;
    using EstateManagement.DataTransferObjects;
    using EstateManagement.DataTransferObjects.Requests;
    using EstateManagement.DataTransferObjects.Responses;
    using Newtonsoft.Json;

    public class TestEstateClient : IEstateClient
    {
        private List<Merchant> Merchants;

        public List<Contract> Contracts;

        public TestEstateClient()
        {
            this.Merchants = new List<Merchant>();
            this.Contracts = new List<Contract>();
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
                Console.WriteLine($"Merchant {merchant.MerchantName} updated [{merchant.MerchantId}]");
            }
            else
            {
                this.Merchants.Add(merchant);
                Console.WriteLine($"Merchant {merchant.MerchantName} added [{merchant.MerchantId}]");
            }
        }

        public void UpdateTestContract(Contract contract)
        {
            Boolean alreadyExists = this.Contracts.Any(c => c.ContractId== contract.ContractId);
            if (alreadyExists == true)
            {
                // Overwrite the contract
                Contract existingContract = this.Contracts.Single(c => c.ContractId == contract.ContractId);
                this.Contracts.Remove(existingContract);
                this.Contracts.Add(contract);
                Console.WriteLine($"Contract {contract.ContractDescription} updated");
            }
            else
            {
                this.Contracts.Add(contract);
                Console.WriteLine($"Contract {contract.ContractDescription} added");
            }
        }

        public async Task<AddMerchantDeviceResponse> AddDeviceToMerchant(String accessToken,
                                                                         Guid estateId,
                                                                         Guid merchantId,
                                                                         AddMerchantDeviceRequest request,
                                                                         CancellationToken cancellationToken)
        {
            return null;
        }

        public async Task<AddProductToContractResponse> AddProductToContract(String accessToken,
                                                                             Guid estateId,
                                                                             Guid contractId,
                                                                             AddProductToContractRequest addProductToContractRequest,
                                                                             CancellationToken cancellationToken)
        {
            return null;
        }

        public async Task<AddTransactionFeeForProductToContractResponse> AddTransactionFeeForProductToContract(String accessToken,
                                                                                                               Guid estateId,
                                                                                                               Guid contractId,
                                                                                                               Guid productId,
                                                                                                               AddTransactionFeeForProductToContractRequest addTransactionFeeForProductToContractRequest,
                                                                                                               CancellationToken cancellationToken)
        {
            return null;
        }

        public async Task<AssignOperatorResponse> AssignOperatorToMerchant(String accessToken,
                                                                           Guid estateId,
                                                                           Guid merchantId,
                                                                           AssignOperatorRequest assignOperatorRequest,
                                                                           CancellationToken cancellationToken)
        {
            return null;
        }

        public async Task<CreateContractResponse> CreateContract(String accessToken,
                                                                 Guid estateId,
                                                                 CreateContractRequest createContractRequest,
                                                                 CancellationToken cancellationToken)
        {
            return null;
        }

        public async Task<CreateEstateResponse> CreateEstate(String accessToken,
                                                             CreateEstateRequest createEstateRequest,
                                                             CancellationToken cancellationToken)
        {
            return null;
        }

        public async Task<CreateEstateUserResponse> CreateEstateUser(String accessToken,
                                                                     Guid estateId,
                                                                     CreateEstateUserRequest createEstateUserRequest,
                                                                     CancellationToken cancellationToken)
        {
            return null;
        }

        public async Task<CreateMerchantResponse> CreateMerchant(String accessToken,
                                                                 Guid estateId,
                                                                 CreateMerchantRequest createMerchantRequest,
                                                                 CancellationToken cancellationToken)
        {
            return null;
        }

        public async Task<CreateMerchantUserResponse> CreateMerchantUser(String accessToken,
                                                                         Guid estateId,
                                                                         Guid merchantId,
                                                                         CreateMerchantUserRequest createMerchantUserRequest,
                                                                         CancellationToken cancellationToken)
        {
            return null;
        }

        public async Task<CreateOperatorResponse> CreateOperator(String accessToken,
                                                                 Guid estateId,
                                                                 CreateOperatorRequest createOperatorRequest,
                                                                 CancellationToken cancellationToken)
        {
            return null;
        }

        public async Task DisableTransactionFeeForProduct(String accessToken,
                                                          Guid estateId,
                                                          Guid contractId,
                                                          Guid productId,
                                                          Guid transactionFeeId,
                                                          CancellationToken cancellationToken)
        {
        }

        public async Task<List<ContractResponse>> GetContracts(String accessToken,
                                                               Guid estateId,
                                                               CancellationToken cancellationToken)
        {
            return null;
        }

        public async Task<ContractResponse> GetContract(String accessToken,
                                                        Guid estateId,
                                                        Guid contractId,
                                                        Boolean includeProducts,
                                                        Boolean includeProductsWithFees,
                                                        CancellationToken cancellationToken)
        {
            return null;
        }

        public async Task<EstateResponse> GetEstate(String accessToken,
                                                    Guid estateId,
                                                    CancellationToken cancellationToken)
        {
            return null;
        }

        public async Task<MerchantResponse> GetMerchant(String accessToken,
                                                        Guid estateId,
                                                        Guid merchantId,
                                                        CancellationToken cancellationToken)
        {
            return null;
        }

        public async Task<MerchantBalanceResponse> GetMerchantBalance(String accessToken,
                                                                      Guid estateId,
                                                                      Guid merchantId,
                                                                      CancellationToken cancellationToken)
        {
            Console.WriteLine($"Estate Id is [{estateId}]");
            Console.WriteLine($"Merchant Id is [{merchantId}]");
            Merchant merchant = this.Merchants.Single(m => m.MerchantId == merchantId);
            var depositSum = merchant.MerchantDeposits.Sum(d => d.Amount);
            return new MerchantBalanceResponse
                   {
                       AvailableBalance = depositSum
                   };
        }

        public async Task<List<ContractResponse>> GetMerchantContracts(String accessToken,
                                                                       Guid estateId,
                                                                       Guid merchantId,
                                                                       CancellationToken cancellationToken)
        {
            List<ContractResponse> result = new List<ContractResponse>();
            this.Contracts.ForEach(contract =>
                                   {
                                       var response = new ContractResponse
                                                      {
                                                          ContractId = contract.ContractId,
                                                          EstateId = contract.EstateId,
                                                          OperatorName = contract.OperatorName,
                                                          Description = contract.ContractDescription,
                                                          OperatorId = contract.OperatorId,
                                                          Products = new List<EstateManagement.DataTransferObjects.Responses.ContractProduct>()
                                                      };
                                       if (contract.ContractProducts.Any())
                                       {
                                           contract.ContractProducts.ForEach(contractProduct =>
                                                                             {
                                                                                 var product = new EstateManagement.DataTransferObjects.Responses.ContractProduct
                                                                                               {
                                                                                                   Value = contractProduct.Value,
                                                                                                   DisplayText = contractProduct.DisplayText,
                                                                                                   Name = contractProduct.ProductName,
                                                                                                   ProductId = contractProduct.ContractProductId,
                                                                                                   TransactionFees = new List<ContractProductTransactionFee>(),
                                                                                               };

                                                                                 if (contractProduct.ContractProductTransactionFees.Any())
                                                                                 {
                                                                                     contractProduct.ContractProductTransactionFees.ForEach(fee =>
                                                                                     {
                                                                                         product.TransactionFees.Add(new ContractProductTransactionFee
                                                                                             {
                                                                                                 Value = fee.Value,
                                                                                                 CalculationType = CalculationType.Fixed,
                                                                                                 Description = fee.FeeDescription,
                                                                                                 FeeType = FeeType.Merchant,
                                                                                                 TransactionFeeId =
                                                                                                     fee.ContractProductTransactionFeeId
                                                                                             });
                                                                                     });
                                                                                 }
                                                                                 response.Products.Add(product);
                                                                             });
                                       }

                                       result.Add(response);
                                   });
            return result;
        }

        public async Task<List<MerchantResponse>> GetMerchants(String accessToken,
                                                               Guid estateId,
                                                               CancellationToken cancellationToken)
        {
            return null;
        }

        public async Task<List<ContractProductTransactionFee>> GetTransactionFeesForProduct(String accessToken,
                                                                                            Guid estateId,
                                                                                            Guid merchantId,
                                                                                            Guid contractId,
                                                                                            Guid productId,
                                                                                            CancellationToken cancellationToken)
        {
            return null;
        }

        public async Task<MakeMerchantDepositResponse> MakeMerchantDeposit(String accessToken,
                                                                           Guid estateId,
                                                                           Guid merchantId,
                                                                           MakeMerchantDepositRequest makeMerchantDepositRequest,
                                                                           CancellationToken cancellationToken)
        {
            return null;
        }
    }
}