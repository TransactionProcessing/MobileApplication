namespace TransactionMobile.IntegrationTestClients
{
    using System;

    public class Operator
    {
        public Guid OperatorId { get; set; }

        public String OperatorName { get; set; }
        public Boolean RequireCustomMerchantNumber { get; set; }
        public Boolean RequireCustomTerminalNumber { get; set; }
    }
}