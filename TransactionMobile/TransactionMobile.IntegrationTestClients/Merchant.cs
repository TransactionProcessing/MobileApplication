namespace TransactionMobile.IntegrationTestClients
{
    using System;
    using System.Collections.Generic;

    public class Merchant
    {
        public Guid EstateId { get; set; }

        public Guid MerchantId { get; set; }

        public String MerchantName { get; set; }

        public String MerchantUserName { get; set; }

        public String Password { get; set; }

        public String GivenName { get; set; }

        public String FamilyName { get; set; }

        public List<MerchantDeposit> MerchantDeposits { get; set; }

        public Merchant()
        {
            this.MerchantDeposits = new List<MerchantDeposit>();
        }

        public void AddDeposit(Decimal amount,
                               DateTime dateTime)
        {
            this.MerchantDeposits.Add(new MerchantDeposit
                                      {
                                          Amount = amount,
                                          DateTime = dateTime
                                      });
        }
    }
}