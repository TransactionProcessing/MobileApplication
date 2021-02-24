using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionMobile.IntegrationTests.Common
{
    using EstateManagement.DataTransferObjects.Responses;
    using TransactionMobile.IntegrationTestClients;
    using ContractProduct = IntegrationTestClients.ContractProduct;

    public class TestingContext
    {
        public TestingContext()
        {
            this.Estates = new List<EstateModel>();
        }

        private List<EstateModel> Estates;

        public void AddEstate(Guid estateId,
                              String estateName)
        {
            this.Estates.Add(new EstateModel
                             {
                                 EstateId = estateId,
                                 EstateName = estateName
                             });
        }

        public EstateModel GetEstate(string estateName)
        {
            return this.Estates.Single(e => e.EstateName == estateName);
        }

        public void AddOperator(String estateName,
                                Guid operatorId,
                                String operatorName,
                                Boolean requireCustomMerchantNumber,
                                Boolean requireCustomTerminalNumber)
        {
            EstateModel estateModel = this.Estates.Single(e => e.EstateName == estateName);
            estateModel.AddOperator(operatorId, operatorName, requireCustomMerchantNumber, requireCustomTerminalNumber);
        }

        public void AddMerchant(String estateName,
                                Guid merchantId,
                                String merchantName,
                                String merchantUserName,
                                String password,
                                String givenName,
                                String familyName)
        {
            EstateModel estateModel = this.GetEstate(estateName);
            estateModel.AddMerchant(merchantId, merchantName, merchantUserName, password, givenName, familyName);
        }

        public Merchant GetMerchant(String estateName,
                                    String merchantName)
        {
            EstateModel estate = this.GetEstate(estateName);
            Merchant merchant = estate.GetMerchant(merchantName);

            return merchant;
        }

        public void AddMerchantDeposit(String estateName,
                                       String merchantName,
                                       Decimal depositAmount,
                                       DateTime depositDateTime)
        {
            EstateModel estate = this.GetEstate(estateName);
            Merchant merchant = estate.GetMerchant(merchantName);
            
            merchant.AddDeposit(depositAmount, depositDateTime);
        }
        
        public void AddContract(String estateName,
                                Guid contractId,
                                String operatorName,
                                String contactDescription)
        {
            EstateModel estate = this.Estates.Single(m => m.EstateName == estateName);
            estate.AddContract(contractId, operatorName,contactDescription);

            Contract contract = estate.GetContract(contactDescription);
            AppManager.UpdateTestContract(contract);
        }

        public void AddContractProduct(String estateName, String contractDescription, Guid contractProductId, String productName, String displayText, Decimal? value)
        {
            EstateModel estate = this.Estates.Single(m => m.EstateName == estateName);
            Contract contract = estate.GetContract(contractDescription);
            contract.AddContractProduct(contractProductId, productName, displayText,value);
            AppManager.UpdateTestContract(contract);
        }

        public void AddContractProductTransactionFee(String estateName, String contractDescription, String productName, Guid contractProductTransactionFeeId, String calculationType, String feeDescription , Decimal value)
        {
            EstateModel estate = this.Estates.Single(m => m.EstateName == estateName);
            Contract contract = estate.GetContract(contractDescription);
            ContractProduct contractProduct = contract.GetContractProduct(productName);
            // TODO: Convert calculation type (maybe an enum)
            contractProduct.AddTransactionFee(contractProductTransactionFeeId, 0, feeDescription, value);
            AppManager.UpdateTestContract(contract);
        }
    }
}