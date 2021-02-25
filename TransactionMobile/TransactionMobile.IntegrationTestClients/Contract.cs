namespace TransactionMobile.IntegrationTestClients
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Contract
    {
        public Guid EstateId { get; set; }
        public Guid OperatorId { get; set; }
        public Guid ContractId { get; set; }
        public String OperatorName { get; set; }
        public String ContractDescription { get; set; }
        public List<ContractProduct> ContractProducts { get; set; }

        public Contract()
        {
            this.ContractProducts = new List<ContractProduct>();
        }

        public void AddContractProduct(Guid contractProductId, String productName, String displayText, Decimal? value)
        {
            // TODO: Do we need a duplicate check
            this.ContractProducts.Add(new ContractProduct
                                      {
                                          ContractProductId = contractProductId,
                                          DisplayText = displayText,
                                          ProductName = productName,
                                          Value = value
                                      });
        }

        public ContractProduct GetContractProduct(String productName)
        {
            return this.ContractProducts.Single(cp => cp.ProductName == productName);
        }
    }
}