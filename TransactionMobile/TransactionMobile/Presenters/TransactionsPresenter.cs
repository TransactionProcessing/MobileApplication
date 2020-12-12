namespace TransactionMobile.Presenters
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Common;
    using Database;
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
        /// The admin page
        /// </summary>
        private readonly IAdminPage AdminPage;

        /// <summary>
        /// The device
        /// </summary>
        private readonly IDevice Device;

        /// <summary>
        /// The database
        /// </summary>
        private readonly IDatabaseContext Database;

        /// <summary>
        /// The mobile topup payment failed page
        /// </summary>
        private readonly IMobileTopupPaymentFailedPage MobileTopupPaymentFailedPage;

        private readonly IVoucherSelectOperatorPage VoucherSelectOperatorPage;

        private readonly VoucherSelectOperatorViewModel VoucherSelectOperatorViewModel;

        private readonly IVoucherSelectProductPage VoucherSelectProductPage;

        private readonly VoucherSelectProductViewModel VoucherSelectProductViewModel;

        private readonly IVoucherPerformVoucherIssuePage VoucherPerformVoucherIssuePage;

        private readonly VoucherPerformVoucherIssueViewModel VoucherPerformVoucherIssueViewModel;

        private readonly IVoucherSuccessPage VoucherSuccessPage;

        private readonly IVoucherFailedPage VoucherFailedPage;

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
        /// The mobile topup select product page
        /// </summary>
        private readonly IMobileTopupSelectProductPage MobileTopupSelectProductPage;

        /// <summary>
        /// The mobile topup select product view model
        /// </summary>
        private readonly MobileTopupSelectProductViewModel MobileTopupSelectProductViewModel;

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
        /// <param name="voucherSelectOperatorPage">The voucher select operator page.</param>
        /// <param name="voucherSelectOperatorViewModel">The voucher select operator view model.</param>
        /// <param name="voucherSelectProductPage">The voucher select product page.</param>
        /// <param name="voucherSelectProductViewModel">The voucher select product view model.</param>
        /// <param name="voucherPerformVoucherIssuePage">The voucher perform voucher issue page.</param>
        /// <param name="voucherPerformVoucherIssueViewModel">The voucher perform voucher issue view model.</param>
        /// <param name="voucherSuccessPage">The voucher success page.</param>
        /// <param name="voucherFailedPage">The voucher failed page.</param>
        /// <param name="adminPage">The admin page.</param>
        /// <param name="device">The device.</param>
        /// <param name="transactionProcessorAclClient">The transaction processor acl client.</param>
        /// <param name="database">The logging database.</param>
        public TransactionsPresenter(ITransactionsPage transactionsPage,
                                     IMobileTopupSelectOperatorPage mobileTopupSelectOperatorPage,
                                     MobileTopupSelectOperatorViewModel mobileTopupSelectOperatorViewModel,
                                     IMobileTopupPerformTopupPage mobileTopupPerformTopupPage,
                                     MobileTopupPerformTopupViewModel mobileTopupPerformTopupViewModel,
                                     IMobileTopupSelectProductPage mobileTopupSelectProductPage,
                                     MobileTopupSelectProductViewModel mobileTopupSelectProductViewModel,
                                     IMobileTopupPaymentSuccessPage mobileTopupPaymentSuccessPage,
                                     IMobileTopupPaymentFailedPage mobileTopupPaymentFailedPage,
                                     IVoucherSelectOperatorPage voucherSelectOperatorPage,
                                     VoucherSelectOperatorViewModel voucherSelectOperatorViewModel,
                                     IVoucherSelectProductPage voucherSelectProductPage,
                                     VoucherSelectProductViewModel voucherSelectProductViewModel,
                                     IVoucherPerformVoucherIssuePage voucherPerformVoucherIssuePage,
                                     VoucherPerformVoucherIssueViewModel voucherPerformVoucherIssueViewModel,
                                     IVoucherSuccessPage voucherSuccessPage,
                                     IVoucherFailedPage voucherFailedPage,
                                     IAdminPage adminPage,
                                     IDevice device,
                                     ITransactionProcessorACLClient transactionProcessorAclClient,
                                     IDatabaseContext database)
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
            this.VoucherSelectOperatorPage = voucherSelectOperatorPage;
            this.VoucherSelectOperatorViewModel = voucherSelectOperatorViewModel;
            this.VoucherSelectProductPage = voucherSelectProductPage;
            this.VoucherSelectProductViewModel = voucherSelectProductViewModel;
            this.VoucherPerformVoucherIssuePage = voucherPerformVoucherIssuePage;
            this.VoucherPerformVoucherIssueViewModel = voucherPerformVoucherIssueViewModel;
            this.VoucherSuccessPage = voucherSuccessPage;
            this.VoucherFailedPage = voucherFailedPage;
            this.AdminPage = adminPage;
            this.Device = device;
            this.TransactionProcessorAclClient = transactionProcessorAclClient;
            this.Database = database;
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
            this.TransactionsPage.VoucherButtonClick += this.TransactionsPage_VoucherButtonClick;
            this.TransactionsPage.AdminButtonClick += this.TransactionsPage_AdminButtonClick;

            this.TransactionsPage.Init();
            await Application.Current.MainPage.Navigation.PushAsync((Page)this.TransactionsPage);
        }

        private async void TransactionsPage_VoucherButtonClick(object sender, EventArgs e)
        {
            this.MobileTopupSelectOperatorViewModel.Operators = new List<ContractProductModel>();

            // Get distinct operator names for Mobile Topup
            this.VoucherSelectOperatorViewModel.Operators =
                App.ContractProducts.Where(c => c.ProductType == ProductType.Voucher).GroupBy(c => c.OperatorName).Select(g => g.First()).ToList();

            this.VoucherSelectOperatorPage.Init(this.VoucherSelectOperatorViewModel);
            this.VoucherSelectOperatorPage.OperatorSelected += this.VoucherSelectOperatorPage_OperatorSelected;

            await Application.Current.MainPage.Navigation.PushAsync((Page)this.VoucherSelectOperatorPage);
        }

        private async void VoucherSelectOperatorPage_OperatorSelected(object sender, SelectedItemChangedEventArgs e)
        {
            // Get the selected item
            ContractProductModel selectedOperator = (ContractProductModel)e.SelectedItem;

            // Get the products for this operator contract
            List<ContractProductModel> products = App.ContractProducts.Where(c => c.ContractId == selectedOperator.ContractId && c.ProductType == ProductType.Voucher).ToList();

            products = products.OrderBy(p => p.Value).ToList();

            // Go to the product selection screen
            this.VoucherSelectProductViewModel.Products = products;
            this.VoucherSelectProductPage.Init(this.VoucherSelectProductViewModel);
            this.VoucherSelectProductPage.ProductSelected += this.VoucherSelectProductPage_ProductSelected;
            await Application.Current.MainPage.Navigation.PushAsync((Page)this.VoucherSelectProductPage);
        }

        private async void VoucherSelectProductPage_ProductSelected(object sender, SelectedItemChangedEventArgs e)
        {
            // Get the selected item
            ContractProductModel selectedProduct = (ContractProductModel)e.SelectedItem;

            // Go to the product selection screen
            this.VoucherPerformVoucherIssueViewModel.ContractProductModel = selectedProduct;

            this.VoucherPerformVoucherIssueViewModel.VoucherAmount = selectedProduct.Value;

            this.VoucherPerformVoucherIssuePage.Init(this.VoucherPerformVoucherIssueViewModel);
            this.VoucherPerformVoucherIssuePage.IssueVoucherButtonClicked += this.VoucherPerformVoucherIssuePage_IssueVoucherButtonClicked;
            await Application.Current.MainPage.Navigation.PushAsync((Page)this.VoucherPerformVoucherIssuePage);
        }

        private async void VoucherPerformVoucherIssuePage_IssueVoucherButtonClicked(object sender, EventArgs e)
        {
            // Check the user has entered a value for the mobile number and amount
            if (String.IsNullOrEmpty(this.VoucherPerformVoucherIssueViewModel.RecipientMobileNumber) && String.IsNullOrEmpty(this.VoucherPerformVoucherIssueViewModel.RecipientEmailAddress))
            {
                this.VoucherPerformVoucherIssueViewModel.RecipientMobileNumber = String.Empty;
                this.VoucherPerformVoucherIssueViewModel.RecipientEmailAddress = String.Empty;
                await Application.Current.MainPage.DisplayAlert("Invalid Voucher Issue Details", "Please enter a recipient mobile number or email address to continue", "OK");
            }
            else
            {
                Boolean voucherIssueResult = await this.PerformVoucherIssue(this.VoucherPerformVoucherIssueViewModel.ContractProductModel);
                
                App.IncrementTransactionNumber();

                await this.Database.InsertLogMessage(DatabaseContext.CreateInformationLogMessage($"Voucher Issue Result is [{voucherIssueResult}]"));

                if (voucherIssueResult)
                {
                    this.VoucherSuccessPage.Init();
                    this.VoucherSuccessPage.CompleteButtonClicked += this.VoucherSuccessPage_CompleteButtonClicked;
                    await Application.Current.MainPage.Navigation.PushAsync((Page)this.VoucherSuccessPage);
                }
                else
                {
                    this.VoucherFailedPage.Init();
                    this.VoucherFailedPage.CancelButtonClicked += this.VoucherFailedPage_CancelButtonClicked;
                    await Application.Current.MainPage.Navigation.PushAsync((Page)this.VoucherFailedPage);
                }
            }
        }

        private async void VoucherFailedPage_CancelButtonClicked(object sender, EventArgs e)
        {
            await Application.Current.MainPage.Navigation.PopToRootAsync();
        }

        private async void VoucherSuccessPage_CompleteButtonClicked(object sender, EventArgs e)
        {
            await Application.Current.MainPage.Navigation.PopToRootAsync();
        }

        /// <summary>
        /// Handles the ReconciliationButtonClicked event of the AdminPage control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private async void AdminPage_ReconciliationButtonClicked(Object sender,
                                                                 EventArgs e)
        {
            // Get the current totals
            OperatorTotals totals = await this.Database.GetTotals();

            // Send a recon message here
            ReconciliationRequestMessage reconciliationRequest = new ReconciliationRequestMessage
                                                                 {
                                                                     DeviceIdentifier = this.Device.GetDeviceIdentifier(),
                                                                     TransactionDateTime = DateTime.Now,
                                                                     TransactionCount = totals.TransactionCount,
                                                                     TransactionValue = totals.TransactionValue
                                                                 };

            String requestJson = JsonConvert.SerializeObject(reconciliationRequest);

            await this.Database.InsertLogMessage(DatabaseContext.CreateInformationLogMessage($"Message Sent to Host [{requestJson}]"));

            ReconciliationResponseMessage response =
                await this.TransactionProcessorAclClient.PerformReconcilaition(App.TokenResponse.AccessToken, reconciliationRequest, CancellationToken.None);

            String responseJson = JsonConvert.SerializeObject(response);
            await this.Database.InsertLogMessage(DatabaseContext.CreateInformationLogMessage($"Message Rcv from Host [{responseJson}]"));

            // Clear the totals if the recon was successful
            if (response.ResponseCode == "0000")
            {
                await this.Database.ResetTotals();
                CrossToastPopUp.Current.ShowToastSuccess("Reconciliation completed, totals reset!");
            }
            else
            {
                CrossToastPopUp.Current.ShowToastWarning("Reconciliation failed, totals not reset!");
            }
            

            await Application.Current.MainPage.Navigation.PopToRootAsync();
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
            if (string.IsNullOrEmpty(this.MobileTopupPerformTopupViewModel.CustomerMobileNumber) || this.MobileTopupPerformTopupViewModel.TopupAmount == 0)
            {
                this.MobileTopupPerformTopupViewModel.CustomerMobileNumber = string.Empty;
                this.MobileTopupPerformTopupViewModel.TopupAmount = 0;
                await Application.Current.MainPage.DisplayAlert("Invalid Topup Details", "Please enter a mobile number and Topup Amount to continue", "OK");
            }
            else
            {
                Boolean mobileTopupResult = await this.PerformMobileTopup(this.MobileTopupPerformTopupViewModel.ContractProductModel);

                App.IncrementTransactionNumber();

                await this.Database.InsertLogMessage(DatabaseContext.CreateInformationLogMessage($"Mobile Topup Result is [{mobileTopupResult}]"));

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
            List<ContractProductModel> products = App.ContractProducts.Where(c => c.ContractId == selectedOperator.ContractId && c.ProductType == ProductType.MobileTopup).ToList();

            products = products.OrderBy(p => p.Value).ToList();

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
        /// <param name="e">The <see cref="SelectedItemChangedEventArgs" /> instance containing the event data.</param>
        private async void MobileTopupSelectProductPage_ProductSelected(Object sender,
                                                                        SelectedItemChangedEventArgs e)
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
            await this.Database.InsertLogMessage(DatabaseContext.CreateInformationLogMessage("About to perform Mobile Topup"));

            SaleTransactionRequestMessage saleTransactionRequestMessage = new SaleTransactionRequestMessage
                                                                          {
                                                                              ContractId = contractProduct.ContractId,
                                                                              ProductId = contractProduct.ProductId,
                                                                              DeviceIdentifier = this.Device.GetDeviceIdentifier(),
                                                                              OperatorIdentifier = contractProduct.OperatorIdentfier,
                                                                              TransactionDateTime = DateTime.Now,
                                                                              TransactionNumber = App.GetNextTransactionNumber().ToString(),
                                                                              CustomerEmailAddress = this.MobileTopupPerformTopupViewModel.CustomerEmailAddress
                                                                          };

            // Add the additional request data
            saleTransactionRequestMessage.AdditionalRequestMetaData = new Dictionary<String, String>();
            saleTransactionRequestMessage.AdditionalRequestMetaData.Add("Amount", this.MobileTopupPerformTopupViewModel.TopupAmount.ToString());
            saleTransactionRequestMessage.AdditionalRequestMetaData.Add("CustomerAccountNumber", this.MobileTopupPerformTopupViewModel.CustomerMobileNumber);

            String requestJson = JsonConvert.SerializeObject(saleTransactionRequestMessage);

            await this.Database.InsertLogMessage(DatabaseContext.CreateInformationLogMessage($"Message Sent to Host [{requestJson}]"));

            SaleTransactionResponseMessage response =
                await this.TransactionProcessorAclClient.PerformSaleTransaction(App.TokenResponse.AccessToken, saleTransactionRequestMessage, CancellationToken.None);

            String responseJson = JsonConvert.SerializeObject(response);
            await this.Database.InsertLogMessage(DatabaseContext.CreateInformationLogMessage($"Message Rcv from Host [{responseJson}]"));

            if (response.ResponseCode != "0000")
            {
                return false;
            }

            // Update the totals
            await this.Database.UpdateOperatorTotals(contractProduct.OperatorId.ToString(), 1, this.MobileTopupPerformTopupViewModel.TopupAmount);

            return true;
        }

        private async Task<Boolean> PerformVoucherIssue(ContractProductModel contractProduct)
        {
            await this.Database.InsertLogMessage(DatabaseContext.CreateInformationLogMessage("About to perform Voucher Issue"));

            SaleTransactionRequestMessage saleTransactionRequestMessage = new SaleTransactionRequestMessage
            {
                ContractId = contractProduct.ContractId,
                ProductId = contractProduct.ProductId,
                DeviceIdentifier = this.Device.GetDeviceIdentifier(),
                OperatorIdentifier = contractProduct.OperatorIdentfier,
                TransactionDateTime = DateTime.Now,
                TransactionNumber = App.GetNextTransactionNumber().ToString(),
                CustomerEmailAddress = this.VoucherPerformVoucherIssueViewModel.CustomerEmailAddress
            };

            // Add the additional request data
            saleTransactionRequestMessage.AdditionalRequestMetaData = new Dictionary<String, String>();
            saleTransactionRequestMessage.AdditionalRequestMetaData.Add("Amount", this.VoucherPerformVoucherIssueViewModel.VoucherAmount.ToString());
            saleTransactionRequestMessage.AdditionalRequestMetaData.Add("RecipientEmail", this.VoucherPerformVoucherIssueViewModel.RecipientEmailAddress);
            saleTransactionRequestMessage.AdditionalRequestMetaData.Add("RecipientMobile", this.VoucherPerformVoucherIssueViewModel.RecipientMobileNumber);

            String requestJson = JsonConvert.SerializeObject(saleTransactionRequestMessage);

            await this.Database.InsertLogMessage(DatabaseContext.CreateInformationLogMessage($"Message Sent to Host [{requestJson}]"));

            SaleTransactionResponseMessage response =
                await this.TransactionProcessorAclClient.PerformSaleTransaction(App.TokenResponse.AccessToken, saleTransactionRequestMessage, CancellationToken.None);

            String responseJson = JsonConvert.SerializeObject(response);
            await this.Database.InsertLogMessage(DatabaseContext.CreateInformationLogMessage($"Message Rcv from Host [{responseJson}]"));

            if (response.ResponseCode != "0000")
            {
                return false;
            }

            // Update the totals
            await this.Database.UpdateOperatorTotals(contractProduct.OperatorId.ToString(), 1, this.VoucherPerformVoucherIssueViewModel.VoucherAmount);

            return true;
        }

        /// <summary>
        /// Handles the AdminButtonClick event of the TransactionsPage control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
        private async void TransactionsPage_AdminButtonClick(Object sender,
                                                             EventArgs e)
        {
            this.AdminPage.ReconciliationButtonClicked += this.AdminPage_ReconciliationButtonClicked;

            this.AdminPage.Init();
            await Application.Current.MainPage.Navigation.PushAsync((Page)this.AdminPage);
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
            this.MobileTopupSelectOperatorViewModel.Operators =
                App.ContractProducts.Where(c => c.ProductType == ProductType.MobileTopup).GroupBy(c => c.OperatorName).Select(g => g.First()).ToList();

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