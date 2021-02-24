namespace TransactionMobile.IntegrationTestClients
{
    using System;
    using System.Collections.Generic;

    public class ContractProduct
    {
        public Guid ContractProductId { get; set; }
        public String ProductName { get; set; }
        public String DisplayText { get; set; }
        public Decimal? Value { get; set; }

        public List<ContractProductFee> ContractProductTransactionFees { get; set; }

        public ContractProduct()
        {
            this.ContractProductTransactionFees = new List<ContractProductFee>();
        }

        public void AddTransactionFee(Guid contractProductTransactionFeeId, Int32 calculationType, String description , Decimal value)
        {
            this.ContractProductTransactionFees.Add(new ContractProductFee
                                                    {
                                                        Value = value,
                                                        CalculationType = calculationType,
                                                        ContractProductTransactionFeeId = contractProductTransactionFeeId,
                                                        FeeDescription = description
                                                    });
        }
    }
}