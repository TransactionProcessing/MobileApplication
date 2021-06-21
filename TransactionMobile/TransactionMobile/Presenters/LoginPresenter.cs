namespace TransactionMobile.Presenters
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Timers;
    using Common;
    using Database;
    using EstateManagement.Client;
    using EstateManagement.DataTransferObjects.Responses;
    using Microsoft.AppCenter.Distribute;
    using Models;
    using Newtonsoft.Json;
    using Pages;
    using Plugin.Toast;
    using Plugin.Toast.Abstractions;
    using SecurityService.Client;
    using SecurityService.DataTransferObjects.Responses;
    using Services;
    using TransactionProcessorACL.DataTransferObjects;
    using TransactionProcessorACL.DataTransferObjects.Responses;
    using ViewModels;
    using Xamarin.Forms;
    using Timer = System.Timers.Timer;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="TransactionMobile.Presenters.ILoginPresenter" />
    [ExcludeFromCodeCoverage]
    public class LoginPresenter : ILoginPresenter
    {
        #region Fields

        /// <summary>
        /// The device
        /// </summary>
        private readonly IDevice Device;

        /// <summary>
        /// The estate client
        /// </summary>
        private readonly IEstateClient EstateClient;

        /// <summary>
        /// The database
        /// </summary>
        private readonly IDatabaseContext Database;

        /// <summary>
        /// The login page
        /// </summary>
        private readonly ILoginPage LoginPage;

        /// <summary>
        /// The login view model
        /// </summary>
        private readonly LoginViewModel LoginViewModel;

        /// <summary>
        /// The main page
        /// </summary>
        private readonly IMainPage MainPage;

        private readonly ISupportPage SupportPage;

        /// <summary>
        /// The main page view model
        /// </summary>
        private readonly MainPageViewModel MainPageViewModel;
        
        /// <summary>
        /// The transaction processor acl client
        /// </summary>
        private readonly ITransactionProcessorACLClient TransactionProcessorAclClient;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginPresenter" /> class.
        /// </summary>
        /// <param name="loginPage">The login page.</param>
        /// <param name="mainPage">The main page.</param>
        /// <param name="supportPage">The support page.</param>
        /// <param name="loginViewModel">The login view model.</param>
        /// <param name="mainPageViewModel">The main page view model.</param>
        /// <param name="device">The device.</param>
        /// <param name="transactionProcessorAclClient">The transaction processor acl client.</param>
        /// <param name="estateClient">The estate client.</param>
        /// <param name="database">The logging database.</param>
        public LoginPresenter(ILoginPage loginPage,
                              IMainPage mainPage,
                              ISupportPage supportPage,
                              LoginViewModel loginViewModel,
                              MainPageViewModel mainPageViewModel,
                              IDevice device,
                              ITransactionProcessorACLClient transactionProcessorAclClient,
                              IEstateClient estateClient,
                              IDatabaseContext database)
        {
            this.MainPage = mainPage;
            this.SupportPage = supportPage;
            this.LoginPage = loginPage;
            this.LoginViewModel = loginViewModel;
            this.MainPageViewModel = mainPageViewModel;
            this.Device = device;
            this.TransactionProcessorAclClient = transactionProcessorAclClient;
            this.EstateClient = estateClient;
            this.Database = database;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Starts this instance.
        /// </summary>
        public async Task Start()
        {
            await this.Database.InsertLogMessage(DatabaseContext.CreateDebugLogMessage("In Start"));

            this.LoginPage.LoginButtonClick += this.LoginPage_LoginButtonClick;
            this.LoginPage.SupportButtonClick += this.LoginPage_SupportButtonClick;
            this.LoginPage.Init(this.LoginViewModel);

            Application.Current.MainPage = new NavigationPage((Page)this.LoginPage);
        }

        /// <summary>
        /// Handles the SupportButtonClick event of the LoginPage control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void LoginPage_SupportButtonClick(object sender, EventArgs e)
        {
            ISupportPresenter supportPresenter = App.Container.GetInstance<ISupportPresenter>();
            supportPresenter.Start();
        }

        /// <summary>
        /// Handles the Elapsed event of the BalanceTimer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Timers.ElapsedEventArgs" /> instance containing the event data.</param>
        private async void BalanceTimer_Elapsed(Object sender,
                                                ElapsedEventArgs e)
        {
            // Go to the API and get the merchant's balance
            MerchantBalanceResponse balanceResponse =
                await this.EstateClient.GetMerchantBalance(App.TokenResponse.AccessToken, App.EstateId, App.MerchantId, CancellationToken.None);

            // get the merchant balance
            this.MainPageViewModel.AvailableBalance = $"{balanceResponse.AvailableBalance:N2} KES";
        }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        private async Task GetConfiguration()
        {
            String config = JsonConvert.SerializeObject(App.Configuration);
            await this.Database.InsertLogMessage(DatabaseContext.CreateInformationLogMessage(config));
        }

        /// <summary>
        /// Gets the merchant balance.
        /// </summary>
        private async Task GetMerchantBalance()
        {
            this.BalanceTimer_Elapsed(null, null);

            Timer balanceTimer = new Timer(10000);

            balanceTimer.Elapsed += this.BalanceTimer_Elapsed;
            balanceTimer.AutoReset = true;
            balanceTimer.Enabled = true;
        }

        /// <summary>
        /// Gets the merchant contract.
        /// </summary>
        private async Task GetMerchantContract()
        {
            this.MerchantContractTimer_Elapsed(null, null);

            Timer merchantContractTimer = new Timer(60000);

            merchantContractTimer.Elapsed += this.MerchantContractTimer_Elapsed;
            merchantContractTimer.AutoReset = true;
            merchantContractTimer.Enabled = true;
        }

        /// <summary>
        /// Handles the LoginButtonClick event of the LoginPage control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private async void LoginPage_LoginButtonClick(Object sender,
                                                      EventArgs e)
        {
            try
            {
                ISecurityServiceClient securityServiceClient = App.Container.GetInstance<ISecurityServiceClient>();
                //this.LoginViewModel.EmailAddress = "merchantuser@emulatormerchant.co.uk";
                //this.LoginViewModel.Password = "123456";

                await this.Database.InsertLogMessage(DatabaseContext.CreateDebugLogMessage("About to Get Configuration"));
                await this.GetConfiguration();

                await this.Database.InsertLogMessage(DatabaseContext.CreateDebugLogMessage($"About to Get Token for User [{this.LoginViewModel.EmailAddress} with Password [{this.LoginViewModel.Password}]]"));

                var g = await securityServiceClient.GetClients(CancellationToken.None);
                foreach (ClientDetails clientDetails in g)
                {
                    await this.Database.InsertLogMessage(DatabaseContext.CreateDebugLogMessage($"{clientDetails.ClientId}"));
                }

                
                // Attempt to login with the user details
                TokenResponse tokenResponse = await securityServiceClient.GetToken(this.LoginViewModel.EmailAddress,
                                                                                   this.LoginViewModel.Password,
                                                                                   App.Configuration.ClientId,
                                                                                   App.Configuration.ClientSecret,
                                                                                   CancellationToken.None);

                await this.Database.InsertLogMessage(DatabaseContext.CreateDebugLogMessage($"About to Cache Token {tokenResponse.AccessToken}"));

                // Cache the user token
                App.TokenResponse = tokenResponse;

                // Do the initial logon transaction
                await this.PerformLogonTransaction();

                // Get the merchants current balance
                await this.GetMerchantBalance();

                // Get the merchants contract details
                await this.GetMerchantContract();

                await this.Database.InsertLogMessage(DatabaseContext.CreateDebugLogMessage("Logon Completed"));

                // Go to signed in page
                this.MainPage.Init(this.MainPageViewModel);
                this.MainPage.TransactionsButtonClicked += this.MainPage_TransactionsButtonClicked;
                this.MainPage.ReportsButtonClicked += this.MainPage_ReportsButtonClicked;
                this.MainPage.SupportButtonClicked += this.MainPage_SupportButtonClicked;
                this.MainPage.ProfileButtonClicked += this.MainPage_ProfileButtonClicked;

                Application.Current.MainPage = new NavigationPage((Page)this.MainPage);
            }
            catch(Exception ex)
            {
                await this.Database.InsertLogMessages(DatabaseContext.CreateErrorLogMessages(ex));

                if (ex.InnerException != null && ex.InnerException is ApplicationException)
                {
                    // Application needs to be upgraded to latest version
                    CrossToastPopUp.Current.ShowToastError("Application version is incompatible, please upgrade to the latest version!!",ToastLength.Long);
;
                }
                else
                {
                    CrossToastPopUp.Current.ShowToastWarning("Incorrect username or password entered, please try again!");
                }
            }
        }

        /// <summary>
        /// Handles the ProfileButtonClicked event of the MainPage control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void MainPage_ProfileButtonClicked(Object sender,
                                                   EventArgs e)
        {
            CrossToastPopUp.Current.ShowToastMessage("Profile Clicked");
        }

        /// <summary>
        /// Handles the ReportsButtonClicked event of the MainPage control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void MainPage_ReportsButtonClicked(Object sender,
                                                   EventArgs e)
        {
            CrossToastPopUp.Current.ShowToastMessage("Reports Clicked");
        }

        /// <summary>
        /// Handles the SupportButtonClicked event of the MainPage control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void MainPage_SupportButtonClicked(Object sender,
                                                   EventArgs e)
        {
            ISupportPresenter supportPresenter = App.Container.GetInstance<ISupportPresenter>();
            supportPresenter.Start();
        }

        /// <summary>
        /// Handles the TransactionsButtonClicked event of the MainPage control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void MainPage_TransactionsButtonClicked(Object sender,
                                                        EventArgs e)
        {
            try
            {
                ITransactionsPresenter transactionsPresenter = App.Container.GetInstance<ITransactionsPresenter>();
                transactionsPresenter.Start();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Handles the Elapsed event of the MerchantContractTimer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Timers.ElapsedEventArgs" /> instance containing the event data.</param>
        private async void MerchantContractTimer_Elapsed(Object sender,
                                                         ElapsedEventArgs e)
        {
            // Go to the API and get the merchant's contract details
            List<ContractResponse> contractResponses =
                await this.EstateClient.GetMerchantContracts(App.TokenResponse.AccessToken, App.EstateId, App.MerchantId, CancellationToken.None);

            App.ContractProducts = new List<ContractProductModel>();

            foreach (ContractResponse contractResponse in contractResponses)
            {
                foreach (ContractProduct contractResponseProduct in contractResponse.Products)
                {
                    App.ContractProducts.Add(new ContractProductModel
                                             {
                                                 OperatorId = contractResponse.OperatorId,
                                                 ContractId = contractResponse.ContractId,
                                                 ProductId = contractResponseProduct.ProductId,
                                                 OperatorIdentfier = contractResponse.OperatorName,
                                                 OperatorName = this.GetOperatorName(contractResponse, contractResponseProduct),
                                                 Value = contractResponseProduct.Value ?? 0,
                                                 IsFixedValue = contractResponseProduct.Value.HasValue,
                                                 ProductDisplayText = contractResponseProduct.DisplayText,
                                                 ProductType = this.GetProductType(contractResponse.OperatorName)
                    });
                }
            }
        }

        /// <summary>
        /// Gets the name of the operator.
        /// </summary>
        /// <param name="contractResponse">The contract response.</param>
        /// <param name="contractProduct">The contract product.</param>
        /// <returns></returns>
        private String GetOperatorName(ContractResponse contractResponse,  ContractProduct contractProduct)
        {
            String operatorName = null;
            ProductType productType = this.GetProductType(contractResponse.OperatorName);
            switch (productType)
            {
                case ProductType.Voucher:
                    operatorName = contractResponse.Description;
                    break;
                default:
                    operatorName = contractResponse.OperatorName;
                    break;

            }

            return operatorName;
        }

        /// <summary>
        /// Gets the type of the product.
        /// </summary>
        /// <param name="operatorName">Name of the operator.</param>
        /// <returns></returns>
        private ProductType GetProductType(String operatorName)
        {
            ProductType productType = ProductType.NotSet;
            switch(operatorName)
            {
                case "Safaricom":
                    productType = ProductType.MobileTopup;
                    break;
                case "Voucher":
                    productType = ProductType.Voucher;
                    break;
            }

            return productType;
        }

        /// <summary>
        /// Performs the logon transaction.
        /// </summary>
        /// <exception cref="Exception">Error during logon transaction</exception>
        /// <exception cref="System.Exception">Error during logon transaction. Response Code [{response.ResponseCode}] Response Message [{response.ResponseMessage}]</exception>
        private async Task PerformLogonTransaction()
        {
            await this.Database.InsertLogMessage(DatabaseContext.CreateInformationLogMessage("About to perform Logon Transaction"));

            LogonTransactionRequestMessage logonTransactionRequestMessage = new LogonTransactionRequestMessage
                                                                            {
                                                                                DeviceIdentifier = this.Device.GetDeviceIdentifier(),
                                                                                TransactionDateTime = DateTime.Now,
                                                                                TransactionNumber = App.GetNextTransactionNumber().ToString(),
                                                                                ApplicationVersion = this.Device.GetSoftwareVersion()
            };
            
            String requestJson = JsonConvert.SerializeObject(logonTransactionRequestMessage);
            await this.Database.InsertLogMessage(DatabaseContext.CreateInformationLogMessage($"Message Sent to Host [{requestJson}]"));

            LogonTransactionResponseMessage response =
                await this.TransactionProcessorAclClient.PerformLogonTransaction(App.TokenResponse.AccessToken, logonTransactionRequestMessage, CancellationToken.None);

            String responseJson = JsonConvert.SerializeObject(response);
            await this.Database.InsertLogMessage(DatabaseContext.CreateInformationLogMessage($"Message Rcv from Host [{responseJson}]"));

            if (response.ResponseCode != "0000")
            {
                throw new Exception($"Error during logon transaction. Response Code [{response.ResponseCode}] Response Message [{response.ResponseMessage}]");
            }

            // Set the application values
            App.EstateId = response.EstateId;
            App.MerchantId = response.MerchantId;
            App.IncrementTransactionNumber();
        }

        #endregion
    }
}