namespace TransactionMobile.Presenters
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Common;
    using Pages;
    using Plugin.Toast;
    using SecurityService.Client;
    using SecurityService.DataTransferObjects.Responses;
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

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginPresenter" /> class.
        /// </summary>
        /// <param name="loginPage">The login page.</param>
        /// <param name="loginViewModel">The login view model.</param>
        /// <param name="device">The device.</param>
        /// <param name="securityServiceClient">The security service client.</param>
        public LoginPresenter(ILoginPage loginPage,
                              LoginViewModel loginViewModel,
                              IDevice device,
                              ISecurityServiceClient securityServiceClient)
        {
            this.LoginPage = loginPage;
            this.LoginViewModel = loginViewModel;
            this.Device = device;
            this.SecurityServiceClient = securityServiceClient;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Starts this instance.
        /// </summary>
        public async Task Start()
        {
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
                // Attempt to login with the user details
                TokenResponse tokenResponse = await this.SecurityServiceClient.GetToken(this.LoginViewModel.EmailAddress,
                                                                                        this.LoginViewModel.Password,
                                                                                        App.Configuration.ClientId,
                                                                                        App.Configuration.ClientSecret,
                                                                                        CancellationToken.None);

                // Cache the user token
                App.TokenResponse = tokenResponse;

                // Set the details for Instabug
                this.Device.SetInstabugUserDetails(this.LoginViewModel.EmailAddress, this.LoginViewModel.EmailAddress);

                // Go to signed in page
                Application.Current.MainPage = new AppShell();
            }
            catch(Exception ex)
            {
                CrossToastPopUp.Current.ShowToastWarning($"Incorrect username or password entered, please try again!");
            }
        }

        #endregion
    }
}