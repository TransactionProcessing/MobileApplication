namespace TransactionMobile.Presenters
{
    using System;
    using System.Threading.Tasks;
    using Common;
    using Pages;
    using Plugin.Toast;
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

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginPresenter"/> class.
        /// </summary>
        /// <param name="loginPage">The login page.</param>
        /// <param name="loginViewModel">The login view model.</param>
        /// <param name="device">The device.</param>
        public LoginPresenter(ILoginPage loginPage,
                              LoginViewModel loginViewModel,
                              IDevice device)
        {
            this.LoginPage = loginPage;
            this.LoginViewModel = loginViewModel;
            this.Device = device;
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
        private void LoginPage_LoginButtonClick(Object sender,
                                                EventArgs e)
        {
            CrossToastPopUp.Current.ShowToastError("Registration Failed");
        }

        #endregion
    }
}