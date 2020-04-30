namespace TransactionMobile.Presenters
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Threading.Tasks;
    using Common;
    using Events;
    using Microsoft.AppCenter.Crashes;
    using Newtonsoft.Json;
    using Pages;
    using Plugin.Toast;
    using SecurityService.Client;
    using SecurityService.DataTransferObjects.Responses;
    using Services;
    using TransactionProcessorACL.DataTransferObjects;
    using TransactionProcessorACL.DataTransferObjects.Responses;
    using Unity;
    using ViewModels;
    using Xamarin.Forms;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="TransactionMobile.Presenters.ILoginPresenter" />
    [ExcludeFromCodeCoverage]
    public class LoginPresenter : ILoginPresenter
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

        /// <summary>
        /// The security service client
        /// </summary>
        private readonly ISecurityServiceClient SecurityServiceClient;

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
        /// <param name="loginViewModel">The login view model.</param>
        /// <param name="device">The device.</param>
        /// <param name="securityServiceClient">The security service client.</param>
        /// <param name="transactionProcessorAclClient">The transaction processor acl client.</param>
        /// <param name="analysisLogger">The analysis logger.</param>
        public LoginPresenter(ILoginPage loginPage,
                              IMainPage mainPage,
                              LoginViewModel loginViewModel,
                              IDevice device,
                              ISecurityServiceClient securityServiceClient,
                              ITransactionProcessorACLClient transactionProcessorAclClient,
                              IAnalysisLogger analysisLogger)
        {
            this.MainPage = mainPage;
            this.LoginPage = loginPage;
            this.LoginViewModel = loginViewModel;
            this.Device = device;
            this.SecurityServiceClient = securityServiceClient;
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
            this.AnalysisLogger.TrackEvent(DebugInformationEvent.Create("In Start"));

            this.LoginPage.LoginButtonClick += this.LoginPage_LoginButtonClick;
            this.LoginPage.Init(this.LoginViewModel);

            Application.Current.MainPage = new NavigationPage((Page)this.LoginPage);
        }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        private async Task GetConfiguration()
        {
            // TODO: this may well make a server call of some kind in future....

            if (App.Configuration == null)
            {
                App.Configuration = new DevelopmentConfiguration();
            }

            this.AnalysisLogger.TrackEvent(DebugInformationEvent.Create($"Client Id is {App.Configuration.ClientId}"));
            this.AnalysisLogger.TrackEvent(DebugInformationEvent.Create($"Client Secret is {App.Configuration.ClientSecret}"));
            this.AnalysisLogger.TrackEvent(DebugInformationEvent.Create($"SecurityService Url is {App.Configuration.SecurityService}"));
            this.AnalysisLogger.TrackEvent(DebugInformationEvent.Create($"TransactionProcessorACL Url is {App.Configuration.TransactionProcessorACL}"));
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
                //this.LoginViewModel.EmailAddress = "merchantuser@testmerchant3.co.uk";
                //this.LoginViewModel.Password = "123456";

                this.AnalysisLogger.TrackEvent(DebugInformationEvent.Create("About to Get Configuration"));
                await this.GetConfiguration();

                this.AnalysisLogger.TrackEvent(DebugInformationEvent.Create("About to Get Token"));
                // Attempt to login with the user details
                TokenResponse tokenResponse = await this.SecurityServiceClient.GetToken(this.LoginViewModel.EmailAddress,
                                                                                        this.LoginViewModel.Password,
                                                                                        App.Configuration.ClientId,
                                                                                        App.Configuration.ClientSecret,
                                                                                        CancellationToken.None);

                this.AnalysisLogger.TrackEvent(DebugInformationEvent.Create($"About to Cache Token {tokenResponse.AccessToken}"));
                this.AnalysisLogger.SetUserName(this.LoginViewModel.EmailAddress);

                // Cache the user token
                App.TokenResponse = tokenResponse;

                // Do the initial logon transaction
                await this.PerformLogonTransaction();

                this.AnalysisLogger.TrackEvent(DebugInformationEvent.Create("Logon Completed"));

                // Go to signed in page
                this.MainPage.Init();
                this.MainPage.TransactionsButtonClicked += this.MainPage_TransactionsButtonClicked;
                this.MainPage.ReportsButtonClicked += this.MainPage_ReportsButtonClicked;
                this.MainPage.SupportButtonClicked += this.MainPage_SupportButtonClicked;
                this.MainPage.ProfileButtonClicked += this.MainPage_ProfileButtonClicked;

                Application.Current.MainPage = new NavigationPage((Page)this.MainPage);
            }
            catch(Exception ex)
            {
                this.AnalysisLogger.TrackEvent(DebugInformationEvent.Create(ex.Message));
                if (ex.InnerException != null)
                {
                    this.AnalysisLogger.TrackEvent(DebugInformationEvent.Create(ex.InnerException.Message));
                }

                CrossToastPopUp.Current.ShowToastWarning("Incorrect username or password entered, please try again!");
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
            CrossToastPopUp.Current.ShowToastMessage("Support Clicked");
        }

        /// <summary>
        /// Handles the TransactionsButtonClicked event of the MainPage control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void MainPage_TransactionsButtonClicked(Object sender,
                                                        EventArgs e)
        {
            ITransactionsPresenter transactionsPresenter = App.Container.Resolve<ITransactionsPresenter>();
            transactionsPresenter.Start();
        }

        /// <summary>
        /// Performs the logon transaction.
        /// </summary>
        /// <exception cref="System.Exception">Error during logon transaction. Response Code [{response.ResponseCode}] Response Message [{response.ResponseMessage}]</exception>
        /// <exception cref="Exception">Error during logon transaction</exception>
        private async Task PerformLogonTransaction()
        {
            this.AnalysisLogger.TrackEvent(DebugInformationEvent.Create("About to Do Logon Transaction"));

            LogonTransactionRequestMessage logonTransactionRequestMessage = new LogonTransactionRequestMessage
                                                                            {
                                                                                DeviceIdentifier = this.Device.GetDeviceIdentifier(),
                                                                                RequireConfigurationInResponse = false,
                                                                                TransactionDateTime = DateTime.Now,
                                                                                TransactionNumber = "1" // TODO: Need to hold txn number somewhere
                                                                            };

            String requestJson = JsonConvert.SerializeObject(logonTransactionRequestMessage);
            this.AnalysisLogger.TrackEvent(MessageSentToHostEvent.Create(App.Configuration.TransactionProcessorACL, requestJson, DateTime.Now));

            LogonTransactionResponseMessage response =
                await this.TransactionProcessorAclClient.PerformLogonTransaction(App.TokenResponse.AccessToken, logonTransactionRequestMessage, CancellationToken.None);

            String responseJson = JsonConvert.SerializeObject(response);
            this.AnalysisLogger.TrackEvent(MessageReceivedFromHostEvent.Create(responseJson, DateTime.Now));

            if (response.ResponseCode != "0000")
            {
                throw new Exception($"Error during logon transaction. Response Code [{response.ResponseCode}] Response Message [{response.ResponseMessage}]");
            }
        }

        #endregion
    }
}