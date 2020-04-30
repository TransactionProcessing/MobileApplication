namespace TransactionMobile.Presenters
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Threading.Tasks;
    using Common;
    using Events;
    using Newtonsoft.Json;
    using Pages;
    using Plugin.Toast;
    using Services;
    using TransactionProcessorACL.DataTransferObjects;
    using TransactionProcessorACL.DataTransferObjects.Responses;
    using ViewModels;
    using Xamarin.Forms;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="TransactionMobile.Presenters.ITransactionsPresenter" />
    [ExcludeFromCodeCoverage]
    public class TransactionsPresenter : ITransactionsPresenter
    {
        #region Fields

        /// <summary>
        /// The analysis logger
        /// </summary>
        private readonly IAnalysisLogger AnalysisLogger;

        /// <summary>
        /// The device
        /// </summary>
        private readonly IDevice Device;

        /// <summary>
        /// The mobile topup payment failed page
        /// </summary>
        private readonly IMobileTopupPaymentFailedPage MobileTopupPaymentFailedPage;

        /// <summary>
        /// The mobile topup payment success page
        /// </summary>
        private readonly IMobileTopupPaymentSuccessPage MobileTopupPaymentSuccessPage;

        /// <summary>
        /// The mobile topup perform topup page
        /// </summary>
        private readonly IMobileTopupPerformTopupPage MobileTopupPerformTopupPage;

        /// <summary>
        /// The mobile topup perform topup view model
        /// </summary>
        private readonly MobileTopupPerformTopupViewModel MobileTopupPerformTopupViewModel;

        /// <summary>
        /// The mobile topup select operator page
        /// </summary>
        private readonly IMobileTopupSelectOperatorPage MobileTopupSelectOperatorPage;

        /// <summary>
        /// The mobile topup select operator view model
        /// </summary>
        private readonly MobileTopupSelectOperatorViewModel MobileTopupSelectOperatorViewModel;

        /// <summary>
        /// The transaction processor acl client
        /// </summary>
        private readonly ITransactionProcessorACLClient TransactionProcessorAclClient;

        /// <summary>
        /// The transactions page
        /// </summary>
        private readonly ITransactionsPage TransactionsPage;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionsPresenter" /> class.
        /// </summary>
        /// <param name="transactionsPage">The transactions page.</param>
        /// <param name="mobileTopupSelectOperatorPage">The mobile topup select operator page.</param>
        /// <param name="mobileTopupSelectOperatorViewModel">The mobile topup select operator view model.</param>
        /// <param name="mobileTopupPerformTopupPage">The mobile topup perform topup page.</param>
        /// <param name="mobileTopupPerformTopupViewModel">The mobile topup perform topup view model.</param>
        /// <param name="mobileTopupPaymentSuccessPage">The mobile topup payment success page.</param>
        /// <param name="mobileTopupPaymentFailedPage">The mobile topup payment failed page.</param>
        /// <param name="device">The device.</param>
        /// <param name="transactionProcessorAclClient">The transaction processor acl client.</param>
        /// <param name="analysisLogger">The analysis logger.</param>
        public TransactionsPresenter(ITransactionsPage transactionsPage,
                                     IMobileTopupSelectOperatorPage mobileTopupSelectOperatorPage,
                                     MobileTopupSelectOperatorViewModel mobileTopupSelectOperatorViewModel,
                                     IMobileTopupPerformTopupPage mobileTopupPerformTopupPage,
                                     MobileTopupPerformTopupViewModel mobileTopupPerformTopupViewModel,
                                     IMobileTopupPaymentSuccessPage mobileTopupPaymentSuccessPage,
                                     IMobileTopupPaymentFailedPage mobileTopupPaymentFailedPage,
                                     IDevice device,
                                     ITransactionProcessorACLClient transactionProcessorAclClient,
                                     IAnalysisLogger analysisLogger)
        {
            this.TransactionsPage = transactionsPage;
            this.MobileTopupSelectOperatorPage = mobileTopupSelectOperatorPage;
            this.MobileTopupSelectOperatorViewModel = mobileTopupSelectOperatorViewModel;
            this.MobileTopupPerformTopupPage = mobileTopupPerformTopupPage;
            this.MobileTopupPerformTopupViewModel = mobileTopupPerformTopupViewModel;
            this.MobileTopupPaymentSuccessPage = mobileTopupPaymentSuccessPage;
            this.MobileTopupPaymentFailedPage = mobileTopupPaymentFailedPage;
            this.Device = device;
            this.TransactionProcessorAclClient = transactionProcessorAclClient;
            this.AnalysisLogger = analysisLogger;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Starts this instance.
        /// </summary>
        public async Task Start()
        {
            this.TransactionsPage.MobileTopupButtonClick += this.TransactionsPage_MobileTopupButtonClick;
            this.TransactionsPage.MobileWalletButtonClick += this.TransactionsPage_MobileWalletButtonClick;
            this.TransactionsPage.BillPaymentButtonClick += this.TransactionsPage_BillPaymentButtonClick;
            this.TransactionsPage.AdminButtonClick += this.TransactionsPage_AdminButtonClick;

            this.TransactionsPage.Init();
            await Application.Current.MainPage.Navigation.PushAsync((Page)this.TransactionsPage);
        }

        /// <summary>
        /// Handles the CancelButtonClicked event of the MobileTopupPaymentFailedPage control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private async void MobileTopupPaymentFailedPage_CancelButtonClicked(Object sender,
                                                                            EventArgs e)
        {
            await Application.Current.MainPage.Navigation.PopToRootAsync();
        }

        /// <summary>
        /// Handles the CompleteButtonClicked event of the MobileTopupPaymentSuccessPage control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private async void MobileTopupPaymentSuccessPage_CompleteButtonClicked(Object sender,
                                                                               EventArgs e)
        {
            await Application.Current.MainPage.Navigation.PopToRootAsync();
        }

        /// <summary>
        /// Handles the PerformTopupButtonClicked event of the MobileTopupPerformTopupPage control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private async void MobileTopupPerformTopupPage_PerformTopupButtonClicked(Object sender,
                                                                                 EventArgs e)
        {
            Boolean mobileTopupResult = await this.PerformMobileTopup();

            this.AnalysisLogger.TrackEvent(DebugInformationEvent.Create($"Mobile Topup Result is [{mobileTopupResult}]"));

            if (mobileTopupResult)
            {
                this.MobileTopupPaymentSuccessPage.Init();
                this.MobileTopupPaymentSuccessPage.CompleteButtonClicked += this.MobileTopupPaymentSuccessPage_CompleteButtonClicked;
                await Application.Current.MainPage.Navigation.PushAsync((Page)this.MobileTopupPaymentSuccessPage);
            }
            else
            {
                this.MobileTopupPaymentFailedPage.Init();
                this.MobileTopupPaymentFailedPage.CancelButtonClicked += this.MobileTopupPaymentFailedPage_CancelButtonClicked;
                await Application.Current.MainPage.Navigation.PushAsync((Page)this.MobileTopupPaymentFailedPage);
            }
        }

        /// <summary>
        /// Handles the OperatorSelected event of the MobileTopupSelectOperatorPage control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SelectedItemChangedEventArgs" /> instance containing the event data.</param>
        private async void MobileTopupSelectOperatorPage_OperatorSelected(Object sender,
                                                                          SelectedItemChangedEventArgs e)
        {
            this.MobileTopupPerformTopupViewModel.OperatorName = e.SelectedItem as String;
            this.MobileTopupPerformTopupPage.Init(this.MobileTopupPerformTopupViewModel);
            this.MobileTopupPerformTopupPage.PerformTopupButtonClicked += this.MobileTopupPerformTopupPage_PerformTopupButtonClicked;

            await Application.Current.MainPage.Navigation.PushAsync((Page)this.MobileTopupPerformTopupPage);
        }

        /// <summary>
        /// Performs the mobile topup.
        /// </summary>
        /// <returns></returns>
        private async Task<Boolean> PerformMobileTopup()
        {
            SaleTransactionRequestMessage saleTransactionRequestMessage = new SaleTransactionRequestMessage
                                                                          {
                                                                              Amount = this.MobileTopupPerformTopupViewModel.TopupAmount,
                                                                              CustomerAccountNumber = this.MobileTopupPerformTopupViewModel.CustomerMobileNumber,
                                                                              DeviceIdentifier = this.Device.GetDeviceIdentifier(),
                                                                              OperatorIdentifier = this.MobileTopupPerformTopupViewModel.OperatorName,
                                                                              TransactionDateTime = DateTime.Now,
                                                                              TransactionNumber = "1" // TODO: Need to hold txn number somewhere
                                                                          };

            String requestJson = JsonConvert.SerializeObject(saleTransactionRequestMessage);
            this.AnalysisLogger.TrackEvent(MessageSentToHostEvent.Create(App.Configuration.TransactionProcessorACL, requestJson, DateTime.Now));

            SaleTransactionResponseMessage response =
                await this.TransactionProcessorAclClient.PerformSaleTransaction(App.TokenResponse.AccessToken, saleTransactionRequestMessage, CancellationToken.None);

            String responseJson = JsonConvert.SerializeObject(response);
            this.AnalysisLogger.TrackEvent(MessageReceivedFromHostEvent.Create(responseJson, DateTime.Now));

            if (response.ResponseCode != "0000")
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Handles the AdminButtonClick event of the TransactionsPage control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
        private void TransactionsPage_AdminButtonClick(Object sender,
                                                       EventArgs e)
        {
            CrossToastPopUp.Current.ShowToastMessage("Admin Clicked");
        }

        /// <summary>
        /// Handles the BillPaymentButtonClick event of the TransactionsPage control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
        private void TransactionsPage_BillPaymentButtonClick(Object sender,
                                                             EventArgs e)
        {
            CrossToastPopUp.Current.ShowToastMessage("Bill Payment Clicked");
        }

        /// <summary>
        /// Handles the MobileTopupButtonClick event of the TransactionsPage control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
        private async void TransactionsPage_MobileTopupButtonClick(Object sender,
                                                                   EventArgs e)
        {
            // TODO: Get the merchants supported operators
            this.MobileTopupSelectOperatorViewModel.Operators = new List<String>();
            this.MobileTopupSelectOperatorViewModel.Operators.Add("Safaricom");

            this.MobileTopupSelectOperatorPage.Init(this.MobileTopupSelectOperatorViewModel);
            this.MobileTopupSelectOperatorPage.OperatorSelected += this.MobileTopupSelectOperatorPage_OperatorSelected;

            await Application.Current.MainPage.Navigation.PushAsync((Page)this.MobileTopupSelectOperatorPage);
        }

        /// <summary>
        /// Handles the MobileWalletButtonClick event of the TransactionsPage control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
        private void TransactionsPage_MobileWalletButtonClick(Object sender,
                                                              EventArgs e)
        {
            CrossToastPopUp.Current.ShowToastMessage("Mobile Wallet Clicked");
        }

        #endregion
    }
}