namespace TransactionMobile.Views
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Events;
    using Pages;
    using ViewModels;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Xamarin.Forms.ContentPage" />
    /// <seealso cref="TransactionMobile.Pages.ILoginPage" />
    /// <seealso cref="TransactionMobile.Pages.IPage" />
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [ExcludeFromCodeCoverage]
    public partial class LoginPage : ContentPage, ILoginPage, IPage
    {
        #region Fields

        /// <summary>
        /// The analysis logger
        /// </summary>
        private readonly IAnalysisLogger AnalysisLogger;

        /// <summary>
        /// The view model
        /// </summary>
        private LoginViewModel ViewModel;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginPage" /> class.
        /// </summary>
        /// <param name="analysisLogger">The analysis logger.</param>
        public LoginPage(IAnalysisLogger analysisLogger)
        {
            this.AnalysisLogger = analysisLogger;
            this.AnalysisLogger.TrackEvent(PageRequestedEvent.Create(this.GetType().Name));
            this.InitializeComponent();
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when [login button click].
        /// </summary>
        public event EventHandler LoginButtonClick;

        #endregion

        #region Methods

        /// <summary>
        /// Initializes the specified view model.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        public void Init(LoginViewModel viewModel)
        {
            this.AnalysisLogger.TrackEvent(PageInitialisedEvent.Create(this.GetType().Name));

            this.ViewModel = viewModel;

            this.EmailEntry.TextChanged += this.Email_TextChanged;
            this.EmailEntry.Completed += this.Email_Completed;

            this.PasswordEntry.TextChanged += this.Password_TextChanged;
            this.PasswordEntry.Completed += this.Password_Completed;

            this.LoginButton.Clicked += this.Login_Clicked;
        }

        /// <summary>
        /// Sets the registration failure message.
        /// </summary>
        /// <param name="failureMessage">The failure message.</param>
        public void SetSignInFailureMessage(String failureMessage)
        {
        }

        /// <summary>
        /// When overridden, allows application developers to customize behavior immediately prior to the <see cref="T:Xamarin.Forms.Page" /> becoming visible.
        /// </summary>
        /// <remarks>
        /// To be added.
        /// </remarks>
        protected override void OnAppearing()
        {
            base.OnAppearing();

            this.EmailEntry.Focus();
        }

        /// <summary>
        /// Handles the Completed event of the Email control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void Email_Completed(Object sender,
                                     EventArgs e)
        {
            this.PasswordEntry.Focus();
        }

        /// <summary>
        /// Handles the TextChanged event of the Email control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="TextChangedEventArgs" /> instance containing the event data.</param>
        private void Email_TextChanged(Object sender,
                                       TextChangedEventArgs e)
        {
            this.ViewModel.EmailAddress = e.NewTextValue;
        }

        /// <summary>
        /// Handles the Clicked event of the Login control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void Login_Clicked(Object sender,
                                   EventArgs e)
        {
            this.LoginButtonClick(sender, e);
        }

        /// <summary>
        /// Handles the Completed event of the Password control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void Password_Completed(Object sender,
                                        EventArgs e)
        {
            this.LoginButton.Focus();
        }

        /// <summary>
        /// Handles the TextChanged event of the Password control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="TextChangedEventArgs" /> instance containing the event data.</param>
        private void Password_TextChanged(Object sender,
                                          TextChangedEventArgs e)
        {
            this.ViewModel.Password = e.NewTextValue;
        }

        #endregion
    }
}