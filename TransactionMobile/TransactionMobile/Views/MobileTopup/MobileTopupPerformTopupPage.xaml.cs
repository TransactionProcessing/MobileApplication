namespace TransactionMobile.Views.MobileTopup
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Common;
    using Pages;
    using ViewModels;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Xamarin.Forms.ContentPage" />
    /// <seealso cref="TransactionMobile.Pages.IMobileTopupPerformTopupPage" />
    /// <seealso cref="TransactionMobile.Pages.IPage" />
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [ExcludeFromCodeCoverage]
    public partial class MobileTopupPerformTopupPage : ContentPage, IMobileTopupPerformTopupPage, IPage
    {
        #region Fields

        /// <summary>
        /// The view model
        /// </summary>
        private MobileTopupPerformTopupViewModel ViewModel;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MobileTopupPerformTopupPage"/> class.
        /// </summary>
        public MobileTopupPerformTopupPage()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when [perform topup button clicked].
        /// </summary>
        public event EventHandler PerformTopupButtonClicked;

        #endregion

        #region Methods

        private IDevice Device;
        /// <summary>
        /// Initializes the specified view model.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        public void Init(MobileTopupPerformTopupViewModel viewModel, IDevice device)
        {
            this.Device = device;
            this.ViewModel = viewModel;
            this.PerformTopupButton.Clicked += this.PerformTopupButton_Clicked;

            this.CustomerMobileNumberEntry.TextChanged += this.CustomerMobileNumberEntry_TextChanged;
            this.CustomerMobileNumberEntry.Completed += this.CustomerMobileNumberEntry_Completed;
            this.TopupAmountEntry.TextChanged += this.TopupAmountEntry_TextChanged;
            this.TopupAmountEntry.Completed += this.TopupAmountEntry_Completed;

            this.Device.AddDebugInformation("MobileTopupPerformTopupPage Init completed");
        }

        /// <summary>
        /// Handles the Completed event of the CustomerMobileNumberEntry control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void CustomerMobileNumberEntry_Completed(Object sender,
                                                         EventArgs e)
        {
            this.TopupAmountEntry.Focus();
        }

        /// <summary>
        /// Handles the TextChanged event of the CustomerMobileNumberEntry control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="TextChangedEventArgs"/> instance containing the event data.</param>
        private void CustomerMobileNumberEntry_TextChanged(Object sender,
                                                           TextChangedEventArgs e)
        {
            this.ViewModel.CustomerMobileNumber = e.NewTextValue;
        }

        /// <summary>
        /// Handles the Clicked event of the PerformTopupButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void PerformTopupButton_Clicked(Object sender,
                                                EventArgs e)
        {
            this.Device.AddDebugInformation("MobileTopupPerformTopupPage PerformTopupButton_Clicked");
            this.Device.AddDebugInformation($"Customer Mobile Number is {this.ViewModel.CustomerMobileNumber}");
            this.Device.AddDebugInformation($"Topup Amount is {this.ViewModel.TopupAmount}");
            this.PerformTopupButtonClicked(sender, e);

        }

        /// <summary>
        /// Handles the Completed event of the TopupAmountEntry control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void TopupAmountEntry_Completed(Object sender,
                                                EventArgs e)
        {
            this.PerformTopupButton.Focus();
        }

        /// <summary>
        /// Handles the TextChanged event of the TopupAmountEntry control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="TextChangedEventArgs"/> instance containing the event data.</param>
        private void TopupAmountEntry_TextChanged(Object sender,
                                                  TextChangedEventArgs e)
        {
            this.ViewModel.TopupAmount = decimal.Parse(e.NewTextValue);
        }

        #endregion
    }
}