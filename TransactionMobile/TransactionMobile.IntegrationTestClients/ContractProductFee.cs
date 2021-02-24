namespace TransactionMobile.IntegrationTestClients
{
    using System;

    public class ContractProductFee
    {
        public Guid ContractProductTransactionFeeId { get; set; }
        public Int32 CalculationType { get; set; }

        public String FeeDescription { get; set; }

        public Decimal Value { get; set; }
    }
}