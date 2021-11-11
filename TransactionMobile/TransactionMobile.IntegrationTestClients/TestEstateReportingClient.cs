namespace TransactionMobile.IntegrationTestClients
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using EstateReporting.Client;
    using EstateReporting.DataTransferObjects;

    public class TestEstateReportingClient : IEstateReportingClient
    {
        public TestEstateReportingClient()
        {
            this.Settlements = new List<Settlement>();
        }

        public async Task<SettlementResponse> GetSettlement(String accessToken,
                                                            Guid estateId,
                                                            Guid? merchantId,
                                                            Guid settlementId,
                                                            CancellationToken cancellationToken)
        {
            var result = this.Settlements.SingleOrDefault(s => s.EstateId == estateId && s.MerchantId == merchantId && 
                                                               s.SettlementId == settlementId);

            SettlementResponse response = new SettlementResponse
                                          {
                                              SettlementId = result.SettlementId,
                                              SettlementDate = result.SettlementDate,
                                              NumberOfFeesSettled = result.NumberOfFeesSettled,
                                              ValueOfFeesSettled = result.ValueOfFeesSettled,
                                              IsCompleted = result.IsCompleted,
                                              SettlementFees = new List<SettlementFeeResponse>()
                                          };

            foreach (SettlementFee resultSettlementFee in result.SettlementFees)
            {
                response.SettlementFees.Add(new SettlementFeeResponse
                                            {
                                                SettlementDate = resultSettlementFee.SettlementDate,
                                                SettlementId = resultSettlementFee.SettlementId,
                                                OperatorIdentifier = resultSettlementFee.OperatorIdentifier,
                                                FeeDescription = resultSettlementFee.FeeDescription,
                                                IsSettled = resultSettlementFee.IsSettled,
                                                TransactionId = resultSettlementFee.TransactionId,
                                                CalculatedValue = resultSettlementFee.CalculatedValue,
                                                MerchantId = resultSettlementFee.MerchantId,
                                                MerchantName = resultSettlementFee.MerchantName
                                            });
            }

            return response;
        }

        public async Task<SettlementByDayResponse> GetSettlementForEstateByDate(String accessToken,
                                                                                Guid estateId,
                                                                                String startDate,
                                                                                String endDate,
                                                                                CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<SettlementByMerchantResponse> GetSettlementForEstateByMerchant(String accessToken,
                                                                                         Guid estateId,
                                                                                         String startDate,
                                                                                         String endDate,
                                                                                         Int32 recordCount,
                                                                                         SortDirection sortDirection,
                                                                                         SortField sortField,
                                                                                         CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<SettlementByMonthResponse> GetSettlementForEstateByMonth(String accessToken,
                                                                                   Guid estateId,
                                                                                   String startDate,
                                                                                   String endDate,
                                                                                   CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<SettlementByOperatorResponse> GetSettlementForEstateByOperator(String accessToken,
                                                                                         Guid estateId,
                                                                                         String startDate,
                                                                                         String endDate,
                                                                                         Int32 recordCount,
                                                                                         SortDirection sortDirection,
                                                                                         SortField sortField,
                                                                                         CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<SettlementByWeekResponse> GetSettlementForEstateByWeek(String accessToken,
                                                                                 Guid estateId,
                                                                                 String startDate,
                                                                                 String endDate,
                                                                                 CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<SettlementByDayResponse> GetSettlementForMerchantByDate(String accessToken,
                                                                                  Guid estateId,
                                                                                  Guid merchantId,
                                                                                  String startDate,
                                                                                  String endDate,
                                                                                  CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<SettlementByMonthResponse> GetSettlementForMerchantByMonth(String accessToken,
                                                                                     Guid estateId,
                                                                                     Guid merchantId,
                                                                                     String startDate,
                                                                                     String endDate,
                                                                                     CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<SettlementByWeekResponse> GetSettlementForMerchantByWeek(String accessToken,
                                                                                   Guid estateId,
                                                                                   Guid merchantId,
                                                                                   String startDate,
                                                                                   String endDate,
                                                                                   CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<List<SettlementResponse>> GetSettlements(String accessToken,
                                                                   Guid estateId,
                                                                   Guid? merchantId,
                                                                   String startDate,
                                                                   String endDate,
                                                                   CancellationToken cancellationToken)
        {
            DateTime startDateAsDate = DateTime.ParseExact(startDate, "yyyyMMdd", null);
            DateTime endDateAsDate = DateTime.ParseExact(endDate, "yyyyMMdd", null);

            var result = this.Settlements.Where(s => s.EstateId == estateId && s.MerchantId == merchantId 
                                                                            && s.SettlementDate >= startDateAsDate 
                                                                            && s.SettlementDate <= endDateAsDate)
                             .ToList();
            
            List<SettlementResponse> responses = new List<SettlementResponse>();

            foreach (Settlement settlement in result)
            {
                responses.Add(new SettlementResponse
                              {
                                  SettlementId = settlement.SettlementId,
                                  SettlementDate = settlement.SettlementDate,
                                  NumberOfFeesSettled = settlement.NumberOfFeesSettled,
                                  ValueOfFeesSettled = settlement.ValueOfFeesSettled,
                                  IsCompleted = settlement.IsCompleted
                              });
            }

            return responses;
        }

        public async Task<TransactionsByDayResponse> GetTransactionsForEstateByDate(String accessToken,
                                                                                    Guid estateId,
                                                                                    String startDate,
                                                                                    String endDate,
                                                                                    CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<TransactionsByMerchantResponse> GetTransactionsForEstateByMerchant(String accessToken,
                                                                                             Guid estateId,
                                                                                             String startDate,
                                                                                             String endDate,
                                                                                             Int32 recordCount,
                                                                                             SortDirection sortDirection,
                                                                                             SortField sortField,
                                                                                             CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<TransactionsByMonthResponse> GetTransactionsForEstateByMonth(String accessToken,
                                                                                       Guid estateId,
                                                                                       String startDate,
                                                                                       String endDate,
                                                                                       CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<TransactionsByOperatorResponse> GetTransactionsForEstateByOperator(String accessToken,
                                                                                             Guid estateId,
                                                                                             String startDate,
                                                                                             String endDate,
                                                                                             Int32 recordCount,
                                                                                             SortDirection sortDirection,
                                                                                             SortField sortField,
                                                                                             CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<TransactionsByWeekResponse> GetTransactionsForEstateByWeek(String accessToken,
                                                                                     Guid estateId,
                                                                                     String startDate,
                                                                                     String endDate,
                                                                                     CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<TransactionsByDayResponse> GetTransactionsForMerchantByDate(String accessToken,
                                                                                      Guid estateId,
                                                                                      Guid merchantId,
                                                                                      String startDate,
                                                                                      String endDate,
                                                                                      CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<TransactionsByMonthResponse> GetTransactionsForMerchantByMonth(String accessToken,
                                                                                         Guid estateId,
                                                                                         Guid merchantId,
                                                                                         String startDate,
                                                                                         String endDate,
                                                                                         CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<TransactionsByWeekResponse> GetTransactionsForMerchantByWeek(String accessToken,
                                                                                       Guid estateId,
                                                                                       Guid merchantId,
                                                                                       String startDate,
                                                                                       String endDate,
                                                                                       CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public void AddSettlementFee(SettlementFee settlementFee)
        {
            var settlementId = ToGuid(settlementFee.SettlementDate.Date);

            var settlement = this.Settlements.SingleOrDefault(s => s.SettlementId == settlementId);

            if (settlement == null)
            {
                this.Settlements.Add(new Settlement
                                     {
                                            EstateId = settlementFee.EstateId,
                                            MerchantId = settlementFee.MerchantId,
                                            SettlementId = settlementId,
                                            SettlementDate = settlementFee.SettlementDate,
                                            NumberOfFeesSettled = 1,
                                            IsCompleted = true,
                                            ValueOfFeesSettled = settlementFee.CalculatedValue,
                                            SettlementFees = new List<SettlementFee>
                                                             {
                                                                 settlementFee
                                                             }
                                     });
            }
            else
            {
                settlement.NumberOfFeesSettled++;
                settlement.ValueOfFeesSettled += settlementFee.CalculatedValue;
                settlement.SettlementFees.Add(settlementFee);

            }
        }

        private List<Settlement> Settlements { get; set; }

        public static Guid ToGuid(DateTime dt)
        {
            var bytes = BitConverter.GetBytes(dt.Ticks);

            Array.Resize(ref bytes, 16);

            return new Guid(bytes);
        }
    }

    public class Settlement
    {
        public Settlement() => this.SettlementFees = new List<SettlementFee>();

        public Guid EstateId { get; set; }

        public Guid MerchantId { get; set; }
        public bool IsCompleted { get; set; }

        public int NumberOfFeesSettled { get; set; }

        public DateTime SettlementDate { get; set; }

        public List<SettlementFee> SettlementFees { get; set; }

        public Guid SettlementId { get; set; }

        public Decimal ValueOfFeesSettled { get; set; }
    }

    public class SettlementFee
    {
        public Decimal CalculatedValue { get; set; }

        public string FeeDescription { get; set; }

        public bool IsSettled { get; set; }
        public Guid EstateId { get; set; }
        public Guid MerchantId { get; set; }

        public string MerchantName { get; set; }

        public DateTime SettlementDate { get; set; }

        public Guid SettlementId { get; set; }

        public Guid TransactionId { get; set; }

        public string OperatorIdentifier { get; set; }
    }
}