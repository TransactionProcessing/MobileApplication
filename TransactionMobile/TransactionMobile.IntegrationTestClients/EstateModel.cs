namespace TransactionMobile.IntegrationTestClients
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class EstateModel
    {
        public EstateModel()
        {
            this.Operators = new List<Operator>();
            this.Merchants = new List<Merchant>();
            this.Contracts = new List<Contract>();
        }
        public Guid EstateId { get; set; }
        public String EstateName { get; set; }

        public List<Operator> Operators;
        public List<Merchant> Merchants;
        public List<Contract> Contracts;

        public void AddOperator(Guid operatorId,
                                String operatorName,
                                Boolean requireCustomMerchantNumber,
                                Boolean requireCustomTerminalNumber)
        {
            // TODO: Duplicate check
            this.Operators.Add(new Operator
                               {
                                   OperatorId = operatorId,
                                   OperatorName = operatorName,
                                   RequireCustomMerchantNumber = requireCustomMerchantNumber,
                                   RequireCustomTerminalNumber = requireCustomTerminalNumber
                               });
        }

        public void AddMerchant(Guid merchantId, String merchantName, String merchantUserName, String password, String givenName, String familyName)
        {
            // TODO: Duplicate check
            this.Merchants.Add(new Merchant
                               {
                                   EstateId = this.EstateId,
                                   FamilyName = familyName,
                                   GivenName = givenName,
                                   MerchantId = merchantId,
                                   MerchantName = merchantName,
                                   MerchantUserName = merchantUserName,
                                   Password = password
                               });
        }

        public void AddContract(Guid contractId, String operatorName, String contactDescription)
        {
            var @operator = this.GetOperator(operatorName);
            // TODO: Duplicate check
            this.Contracts.Add(new Contract
                               {
                                   OperatorName = operatorName,
                                   ContractDescription = contactDescription,
                                   ContractId = contractId,
                                   EstateId = this.EstateId,
                                   OperatorId = @operator.OperatorId
                               });

        }
        
        public Contract GetContract(String contractDescription)
        {
            return this.Contracts.Single(c => c.ContractDescription == contractDescription);
        }

        public Merchant GetMerchant(String merchantName)
        {
            return this.Merchants.Single(m => m.MerchantName == merchantName);
        }

        public Operator GetOperator(String operatorName)
        {
            return this.Operators.Single(m => m.OperatorName == operatorName);
        }
    }
}