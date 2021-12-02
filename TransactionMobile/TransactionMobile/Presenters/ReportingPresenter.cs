namespace TransactionMobile.Presenters
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Common;
    using Database;
    using EstateReporting.Client;
    using EstateReporting.DataTransferObjects;
    using Extensions;
    using Models;
    using Pages.Reporting;
    using Plugin.Toast;
    using ViewModels.Reporting;
    using Views.Reporting;
    using Xamarin.Forms;
    using SelectionChangedEventArgs = Syncfusion.XForms.ComboBox.SelectionChangedEventArgs;

    public class ReportingPresenter : IReportingPresenter
    {
        #region Fields

        private readonly IDatabaseContext Database;

        private readonly IDevice Device;

        private readonly IEstateReportingClient EstateReportingClient;

        private readonly MySettlementListViewModel MySettlementListViewModel;

        private readonly IMySettlementsAnalysisPage MySettlementsAnalysisPage;

        private readonly MySettlementAnalysisViewModel MySettlementAnalysisViewModel;

        private readonly IMySettlementsListPage MySettlementsListPage;

        private readonly IReportingPage ReportingPage;

        #endregion

        #region Constructors

        public ReportingPresenter(IReportingPage reportingPage,
                                  IMySettlementsListPage mySettlementsListPage,
                                  MySettlementListViewModel mySettlementListViewModel,
                                  IEstateReportingClient estateReportingClient,
                                  IMySettlementsAnalysisPage mySettlementsAnalysisPage,
                                  //MySettlementAnalysisViewModel mySettlementAnalysisViewModel,
                                  IDatabaseContext database,
                                  IDevice device)
        {
            this.ReportingPage = reportingPage;
            this.MySettlementsListPage = mySettlementsListPage;
            this.MySettlementListViewModel = mySettlementListViewModel;
            this.MySettlementsAnalysisPage = mySettlementsAnalysisPage;
            this.MySettlementAnalysisViewModel = new MySettlementAnalysisViewModel();
            this.EstateReportingClient = estateReportingClient;
            this.Database = database;
            this.Device = device;
        }

        #endregion
        
        #region Methods

        public async Task Start()
        {
            await this.Database.InsertLogMessage(DatabaseContext.CreateDebugLogMessage("In Start"));

            this.ReportingPage.ViewMySettlementButtonClick += this.ReportingPage_ViewMySettlementButtonClick;
            this.ReportingPage.ViewMyBalanceHistoryButtonClick += this.ReportingPage_ViewMyBalanceHistoryButtonClick;
            this.ReportingPage.ViewMyTransactionsButtonClick += this.ReportingPage_ViewMyTransactionsButtonClick;
            this.ReportingPage.Init();

            Application.Current.MainPage = new NavigationPage((Page)this.ReportingPage);
        }

        private List<DatePeriod> GenerateDatePeriods(Int32 numberHistoricalMonths)
        {
            DateTime currentDate = DateTime.Now.Date;

            List<DatePeriod> datePeriods = new List<DatePeriod>();

            List<(String displayText, DateTime startDate, DateTime endDate)> generatedPeriods = currentDate.GenerateDatePeriods(numberHistoricalMonths);

            generatedPeriods.ForEach(p => datePeriods.Add(new DatePeriod
                                                          {
                                                              DisplayText = p.displayText,
                                                              EndDate = p.endDate,
                                                              StartDate = p.startDate
                                                          }));

            return datePeriods;
        }

        private async Task LoadSettlementData(DatePeriod datePeriod = null)
        {
            // Get the selected date range
            List<SettlementResponse> settlementData = await this.EstateReportingClient.GetSettlements(App.TokenResponse.AccessToken,
                                                                                                      App.EstateId,
                                                                                                      App.MerchantId,
                                                                                                      datePeriod.StartDate.ToString("yyyyMMdd"),
                                                                                                      datePeriod.EndDate.ToString("yyyyMMdd"),
                                                                                                      CancellationToken.None);
            // Call translation factory
            foreach (SettlementResponse settlementResponse in settlementData)
            {
                this.MySettlementListViewModel.SettlementListItems.Add(new SettlementListItem
                                                                       {
                                                                           IsComplete = settlementResponse.IsCompleted,
                                                                           SettlementId = settlementResponse.SettlementId,
                                                                           NumberOfFeesSettled = settlementResponse.NumberOfFeesSettled,
                                                                           SettlementDate = settlementResponse.SettlementDate,
                                                                           Value = settlementResponse.ValueOfFeesSettled
                                                                       });
            }
        }

        private async void MySettlementsListPage_SettlementListItemSelected(Object sender,
                                                                            SettlementListItem e)
        {
            // Get the items details (do a remote call here)
            // call translation factory here
            this.MySettlementAnalysisViewModel.SettlementFeeListItems = new List<SettlementFeeListItem>();
            SettlementResponse settlement =
                await this.EstateReportingClient.GetSettlement(App.TokenResponse.AccessToken, App.EstateId, App.MerchantId, e.SettlementId, CancellationToken.None);
            foreach (SettlementFeeResponse settlementSettlementFee in settlement.SettlementFees)
            {
                this.MySettlementAnalysisViewModel.SettlementFeeListItems.Add(new SettlementFeeListItem
                                                                              {
                                                                                  CalculatedValue = settlementSettlementFee.CalculatedValue,
                                                                                  FeeDescription = settlementSettlementFee.FeeDescription,
                                                                                  IsSettled = settlementSettlementFee.IsSettled,
                                                                                  OperatorIdentifier = settlementSettlementFee.OperatorIdentifier,
                                                                                  SettlementDate = settlementSettlementFee.SettlementDate,
                                                                                  SettlementId = settlementSettlementFee.SettlementId,
                                                                                  TransactionId = settlementSettlementFee.TransactionId
                                                                              });
            }

            this.MySettlementsAnalysisPage.Init(this.MySettlementAnalysisViewModel);

            // Show the settlement date analysis screen
            await Application.Current.MainPage.Navigation.PushAsync((Page)this.MySettlementsAnalysisPage);
        }

        private async void MySettlementsListPage_SettlementListPeriodChanged(Object sender,
                                                                             SelectionChangedEventArgs e)
        {
            await this.LoadSettlementData(e.Value as DatePeriod);
            this.MySettlementsListPage.LoadSettlementData(this.MySettlementListViewModel);
        }

        private void ReportingPage_ViewMyBalanceHistoryButtonClick(Object sender,
                                                                   EventArgs e)
        {
            CrossToastPopUp.Current.ShowToastMessage("ViewMyBalanceHistory Clicked");
        }

        private async void ReportingPage_ViewMySettlementButtonClick(Object sender,
                                                                     EventArgs e)
        {
            this.MySettlementListViewModel.SettlementListItems = new List<SettlementListItem>();
            List<DatePeriod> datePeriods = this.GenerateDatePeriods(3);

            this.MySettlementsListPage.Init(datePeriods);
            this.MySettlementsListPage.SettlementListItemSelected += this.MySettlementsListPage_SettlementListItemSelected;
            this.MySettlementsListPage.SettlementListPeriodChanged += this.MySettlementsListPage_SettlementListPeriodChanged;

            await Application.Current.MainPage.Navigation.PushAsync((Page)this.MySettlementsListPage);
        }

        private void ReportingPage_ViewMyTransactionsButtonClick(Object sender,
                                                                 EventArgs e)
        {
            CrossToastPopUp.Current.ShowToastMessage("ViewMyTransactions Clicked");
        }

        #endregion
    }
}