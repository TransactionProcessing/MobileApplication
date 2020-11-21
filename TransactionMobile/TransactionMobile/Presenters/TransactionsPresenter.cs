namespace TransactionMobile.Presenters
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Common;
    using Events;
    using Models;
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

        private readonly IMobileTopupSelectProductPage MobileTopupSelectProductPage;

        private readonly MobileTopupSelectProductViewModel MobileTopupSelectProductViewModel;

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
        /// <param name="mobileTopupSelectProductPage">The mobile topup select product page.</param>
        /// <param name="mobileTopupSelectProductViewModel">The mobile topup select product view model.</param>
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
                                     IMobileTopupSelectProductPage mobileTopupSelectProductPage,
                                     MobileTopupSelectProductViewModel mobileTopupSelectProductViewModel,
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
            this.MobileTopupSelectProductPage = mobileTopupSelectProductPage;
            this.MobileTopupSelectProductViewModel = mobileTopupSelectProductViewModel;
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
            // Check the user has entered a value for the mobile number and amount
            if (String.IsNullOrEmpty(this.MobileTopupPerformTopupViewModel.CustomerMobileNumber) || this.MobileTopupPerformTopupViewModel.TopupAmount == 0)
            {
                this.MobileTopupPerformTopupViewModel.CustomerMobileNumber = String.Empty;
                this.MobileTopupPerformTopupViewModel.TopupAmount = 0;
                await App.Current.MainPage.DisplayAlert("Invalid Topup Details", "Please enter a mobile number and Topup Amount to continue", "OK");
            }
            else
            {
                Boolean mobileTopupResult = await this.PerformMobileTopup(this.MobileTopupPerformTopupViewModel.ContractProductModel);

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
        }

        /// <summary>
        /// Handles the OperatorSelected event of the MobileTopupSelectOperatorPage control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SelectedItemChangedEventArgs" /> instance containing the event data.</param>
        private async void MobileTopupSelectOperatorPage_OperatorSelected(Object sender,
                                                                          SelectedItemChangedEventArgs e)
        {
            // Get the selected item
            ContractProductModel selectedOperator = (ContractProductModel)e.SelectedItem;

            // Get the products for this operator contract
            List<ContractProductModel> products = App.ContractProducts.Where(c => c.ContractId == selectedOperator.ContractId).ToList();

            // Go to the product selection screen
            this.MobileTopupSelectProductViewModel.Products = products;
            this.MobileTopupSelectProductPage.Init(this.MobileTopupSelectProductViewModel);
            this.MobileTopupSelectProductPage.ProductSelected += this.MobileTopupSelectProductPage_ProductSelected;
            await Application.Current.MainPage.Navigation.PushAsync((Page)this.MobileTopupSelectProductPage);
            
        }

        /// <summary>
        /// Handles the ProductSelected event of the MobileTopupSelectProductPage control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SelectedItemChangedEventArgs"/> instance containing the event data.</param>
        private async void MobileTopupSelectProductPage_ProductSelected(object sender, SelectedItemChangedEventArgs e)
        {
            // Get the selected item
            ContractProductModel selectedProduct = (ContractProductModel)e.SelectedItem;

            // Go to the product selection screen
            this.MobileTopupPerformTopupViewModel.ContractProductModel = selectedProduct;

            if (selectedProduct.IsFixedValue)
            {
                this.MobileTopupPerformTopupViewModel.TopupAmount = selectedProduct.Value;
            }

            this.MobileTopupPerformTopupPage.Init(this.MobileTopupPerformTopupViewModel);
            this.MobileTopupPerformTopupPage.PerformTopupButtonClicked += this.MobileTopupPerformTopupPage_PerformTopupButtonClicked;
            await Application.Current.MainPage.Navigation.PushAsync((Page)this.MobileTopupPerformTopupPage);
        }

        /// <summary>
        /// Performs the mobile topup.
        /// </summary>
        /// <param name="contractProduct">The contract product.</param>
        /// <returns></returns>
        private async Task<Boolean> PerformMobileTopup(ContractProductModel contractProduct)
        {
            SaleTransactionRequestMessage saleTransactionRequestMessage = new SaleTransactionRequestMessage
                                                                          {
                                                                              ContractId = contractProduct.ContractId,
                                                                              ProductId = contractProduct.ProductId,
                                                                              Amount = this.MobileTopupPerformTopupViewModel.TopupAmount,
                                                                              CustomerAccountNumber = this.MobileTopupPerformTopupViewModel.CustomerMobileNumber,
                                                                              DeviceIdentifier = this.Device.GetDeviceIdentifier(),
                                                                              OperatorIdentifier = contractProduct.OperatorName,
                                                                              TransactionDateTime = DateTime.Now,
                                                                              TransactionNumber = "1", // TODO: Need to hold txn number somewhere
                                                                              CustomerEmailAddress = this.MobileTopupPerformTopupViewModel.CustomerEmailAddress
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
            this.MobileTopupSelectOperatorViewModel.Operators = new List<ContractProductModel>();

            // Get distinct operator names for Mobile Topup
            this.MobileTopupSelectOperatorViewModel.Operators = App.ContractProducts.Where(c => c.ProductType == 0).GroupBy(c => c.OperatorName).Select(g => g.First()) .ToList();
            
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