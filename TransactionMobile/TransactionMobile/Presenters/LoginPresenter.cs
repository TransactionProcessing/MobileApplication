namespace TransactionMobile.Presenters
{
    using System;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Common;
    using Newtonsoft.Json;
    using Pages;
    using Plugin.Toast;
    using SecurityService.Client;
    using SecurityService.DataTransferObjects.Responses;
    using Services;
    using TransactionProcessorACL.DataTransferObjects;
    using TransactionProcessorACL.DataTransferObjects.Responses;
    using ViewModels;
    using Xamarin.Forms;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="TransactionMobile.Presenters.ILoginPresenter" />
    public class LoginPresenter : ILoginPresenter
    {
        #region Fields
        
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
        /// The security service client
        /// </summary>
        private readonly ISecurityServiceClient SecurityServiceClient;

        private readonly ITransactionProcessorACLClient TransactionProcessorAclClient;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginPresenter" /> class.
        /// </summary>
        /// <param name="loginPage">The login page.</param>
        /// <param name="loginViewModel">The login view model.</param>
        /// <param name="device">The device.</param>
        /// <param name="securityServiceClient">The security service client.</param>
        /// <param name="transactionProcessorAclClient">The transaction processor acl client.</param>
        public LoginPresenter(ILoginPage loginPage,
                              LoginViewModel loginViewModel,
                              IDevice device,
                              ISecurityServiceClient securityServiceClient,
                              ITransactionProcessorACLClient transactionProcessorAclClient)
        {
            this.LoginPage = loginPage;
            this.LoginViewModel = loginViewModel;
            this.Device = device;
            this.SecurityServiceClient = securityServiceClient;
            this.TransactionProcessorAclClient = transactionProcessorAclClient;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Starts this instance.
        /// </summary>
        public async Task Start()
        {
            this.Device.AddDebugInformation("In Start");

            this.LoginPage.LoginButtonClick += this.LoginPage_LoginButtonClick;
            this.LoginPage.Init(this.LoginViewModel);

            Application.Current.MainPage = new NavigationPage((Page)this.LoginPage);
        }

        /// <summary>
        /// Handles the LoginButtonClick event of the LoginPage control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private async void LoginPage_LoginButtonClick(Object sender,
                                                      EventArgs e)
        {
            try
            {
                this.Device.AddDebugInformation($"About to Get Configuration");
                await this.GetConfiguration();

                this.Device.AddDebugInformation($"About to Get Token");
                // Attempt to login with the user details
                TokenResponse tokenResponse = await this.SecurityServiceClient.GetToken(this.LoginViewModel.EmailAddress,
                                                                                        this.LoginViewModel.Password,
                                                                                        App.Configuration.ClientId,
                                                                                        App.Configuration.ClientSecret,
                                                                                        CancellationToken.None);

                this.Device.AddDebugInformation($"About to Cache Token {tokenResponse.AccessToken}");

                // Cache the user token
                App.TokenResponse = tokenResponse;

                this.Device.AddDebugInformation($"About to Set Instabug Details");
                // Set the details for Instabug
                this.Device.SetInstabugUserDetails(this.LoginViewModel.EmailAddress, this.LoginViewModel.EmailAddress);

                this.Device.AddDebugInformation($"About to Do Logon Transaction");
                // Do the intial logon transaction
                await this.PerformLogonTransaction();

                this.Device.AddDebugInformation($"Logon Completed");
                // Go to signed in page
                Application.Current.MainPage = new AppShell();
            }
            catch(Exception ex)
            {
                CrossToastPopUp.Current.ShowToastWarning($"Incorrect username or password entered, please try again!");
            }
        }

        /// <summary>
        /// Performs the logon transaction.
        /// </summary>
        /// <exception cref="Exception">Error during logon transaction</exception>
        private async Task PerformLogonTransaction()
        {
            LogonTransactionRequestMessage logonTransactionRequestMessage = new LogonTransactionRequestMessage
                                                                            {
                                                                                DeviceIdentifier = this.Device.GetDeviceIdentifier(),
                                                                                RequireConfigurationInResponse = false,
                                                                                TransactionDateTime = DateTime.Now,
                                                                                TransactionNumber = "1" // TODO: Need to hold txn number somewhere
                                                                            };

            String requestJson = JsonConvert.SerializeObject(logonTransactionRequestMessage);
            this.Device.AddDebugInformation($"Logon Request is {requestJson}");

            LogonTransactionResponseMessage response = await this.TransactionProcessorAclClient.PerformLogonTransaction(App.TokenResponse.AccessToken, logonTransactionRequestMessage, CancellationToken.None);

            String responseJson = JsonConvert.SerializeObject(response);
            this.Device.AddDebugInformation($"Logon Response is {responseJson}");

            if (response.ResponseCode != "0000")
            {
                throw new Exception("Error during logon transaction");
            }
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

            this.Device.AddDebugInformation($"Client Id is {App.Configuration.ClientId}");
            this.Device.AddDebugInformation($"Client Secret is {App.Configuration.ClientSecret}");
            this.Device.AddDebugInformation($"SecurityService Url is {App.Configuration.SecurityService}");
            this.Device.AddDebugInformation($"TransactionProcessorACL Url is {App.Configuration.TransactionProcessorACL}");

        }

        #endregion
    }
}