namespace TransactionMobile.Views.Transactions
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Database;
    using Pages;
    using Pages.Transactions;
    using ViewModels.Transactions;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Xamarin.Forms.ContentPage" />
    /// <seealso cref="IMobileTopupPerformTopupPage" />
    /// <seealso cref="TransactionMobile.Pages.IPage" />
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [ExcludeFromCodeCoverage]
    public partial class VoucherPerformVoucherIssuePage : ContentPage, IVoucherPerformVoucherIssuePage, IPage
    {
        private readonly IDatabaseContext Database;

        #region Fields


        /// <summary>
        /// The view model
        /// </summary>
        private VoucherPerformVoucherIssueViewModel ViewModel;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="VoucherPerformVoucherIssuePage" /> class.
        /// </summary>
        /// <param name="loggingDatabase">The logging database.</param>
        public VoucherPerformVoucherIssuePage(IDatabaseContext loggingDatabase)
        {
            this.Database = loggingDatabase;
            this.Database.InsertLogMessage(DatabaseContext.CreateDebugLogMessage($"In {this.GetType().Name} ctor"));
            this.InitializeComponent();
        }

        #endregion

        
        #region Methods

        public event EventHandler IssueVoucherButtonClicked;

        /// <summary>
        /// Initializes the specified view model.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        public void Init(VoucherPerformVoucherIssueViewModel viewModel)
        {
            this.Database.InsertLogMessage(DatabaseContext.CreateDebugLogMessage($"In {this.GetType().Name} Init"));

            this.ViewModel = viewModel;
            this.BindingContext = this.ViewModel;
            this.IssueVoucherButton.Clicked += this.IssueVoucherButton_Clicked;
        }

        private void IssueVoucherButton_Clicked(object sender, EventArgs e)
        {
            this.IssueVoucherButtonClicked(sender, e);
        }


        #endregion
    }
}