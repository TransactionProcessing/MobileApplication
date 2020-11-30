namespace TransactionMobile.Views.MobileTopup
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Controls;
    using Database;
    using Pages;
    using ViewModels;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;
    using static System.Char;

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
        private readonly IDatabaseContext Database;

        #region Fields


        /// <summary>
        /// The view model
        /// </summary>
        private MobileTopupPerformTopupViewModel ViewModel;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MobileTopupPerformTopupPage" /> class.
        /// </summary>
        /// <param name="loggingDatabase">The logging database.</param>
        public MobileTopupPerformTopupPage(IDatabaseContext loggingDatabase)
        {
            this.Database = loggingDatabase;
            this.Database.InsertLogMessage(DatabaseContext.CreateDebugLogMessage($"In {this.GetType().Name} ctor"));
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

        /// <summary>
        /// Initializes the specified view model.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        public void Init(MobileTopupPerformTopupViewModel viewModel)
        {
            this.Database.InsertLogMessage(DatabaseContext.CreateDebugLogMessage($"In {this.GetType().Name} Init"));

            this.ViewModel = viewModel;
            this.BindingContext = this.ViewModel;
            this.PerformTopupButton.Clicked += this.PerformTopupButton_Clicked;

            this.CustomerMobileNumberEntry.TextChanged += this.CustomerMobileNumberEntry_TextChanged;
            this.CustomerMobileNumberEntry.Completed += this.CustomerMobileNumberEntry_Completed;
            if (viewModel.ContractProductModel.IsFixedValue)
            {
                this.TopupAmountEntry.IsEnabled = false;
                this.CustomerMobileNumberEntry.Focus();
            }
            else
            {
                this.TopupAmountEntry.TextChanged += this.TopupAmountEntry_TextChanged;
                this.TopupAmountEntry.Completed += this.TopupAmountEntry_Completed;
                this.TopupAmountEntry.IsEnabled = true;
            }
        }

        /// <summary>
        /// Handles the Completed event of the CustomerMobileNumberEntry control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void CustomerMobileNumberEntry_Completed(Object sender,
                                                         EventArgs e)
        {
            this.TopupAmountEntry.Focus();
        }

        /// <summary>
        /// Handles the TextChanged event of the CustomerMobileNumberEntry control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="TextChangedEventArgs" /> instance containing the event data.</param>
        private void CustomerMobileNumberEntry_TextChanged(Object sender,
                                                           TextChangedEventArgs e)
        {
            //Make sure all characters are numbers
            Boolean isValid = e.NewTextValue.ToCharArray().All(Char.IsDigit);
            if (isValid && String.IsNullOrEmpty(e.NewTextValue) == false)
            {
                this.ViewModel.CustomerMobileNumber = e.NewTextValue;
            }
            else
            {
                ((BorderlessEntry)sender).Text = e.OldTextValue ?? String.Empty;
            }

            
        }

        /// <summary>
        /// Handles the Clicked event of the PerformTopupButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void PerformTopupButton_Clicked(Object sender,
                                                EventArgs e)
        {
            this.PerformTopupButtonClicked(sender, e);
        }

        /// <summary>
        /// Handles the Completed event of the TopupAmountEntry control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void TopupAmountEntry_Completed(Object sender,
                                                EventArgs e)
        {
            this.PerformTopupButton.Focus();
        }

        /// <summary>
        /// Handles the TextChanged event of the TopupAmountEntry control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="TextChangedEventArgs" /> instance containing the event data.</param>
        private void TopupAmountEntry_TextChanged(Object sender,
                                                  TextChangedEventArgs e)
        {
            //Make sure all characters are numbers
            Boolean isValid = e.NewTextValue.ToCharArray().All(Char.IsDigit);
            if (isValid && String.IsNullOrEmpty(e.NewTextValue) == false)
            {
                this.ViewModel.TopupAmount = Decimal.Parse(e.NewTextValue);
            }
            else
            {
                ((BorderlessEntry)sender).Text = e.OldTextValue ?? String.Empty;
            }
        }

        #endregion
    }
}