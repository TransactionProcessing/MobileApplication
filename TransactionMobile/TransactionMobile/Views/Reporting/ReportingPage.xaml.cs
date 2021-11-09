namespace TransactionMobile.Views.Reporting
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Common;
    using Database;
    using Pages;
    using Pages.Reporting;
    using Pages.Transactions;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Xamarin.Forms.ContentPage" />
    /// <seealso cref="ITransactionsPage" />
    /// <seealso cref="TransactionMobile.Pages.IPage" />
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [ExcludeFromCodeCoverage]
    public partial class ReportingPage : ContentPage, IReportingPage, IPage
    {
        private readonly IDatabaseContext Database;

        private readonly IDevice Device;

        #region Fields
        
        #endregion

        #region Constructors
        
        public ReportingPage(IDatabaseContext database,
                             IDevice device)
        {
            this.Database = database;
            this.Device = device;
            this.Database.InsertLogMessage(DatabaseContext.CreateDebugLogMessage($"In {this.GetType().Name} ctor"));
            this.InitializeComponent();
        }

        #endregion
        
        #region Methods

        public event EventHandler ViewMySettlementButtonClick;

        public event EventHandler ViewMyTransactionsButtonClick;

        public event EventHandler ViewMyBalanceHistoryButtonClick;

        /// <summary>
        /// Initializes the specified view model.
        /// </summary>
        public void Init()
        {
            this.Database.InsertLogMessage(DatabaseContext.CreateDebugLogMessage($"In {this.GetType().Name} Init"));

            this.ViewMyBalanceHistoryButton.Clicked += ViewMyBalanceHistoryButton_Clicked;
            this.ViewMySettlementsButton.Clicked += ViewMySettlementsButton_Clicked;
            this.ViewMyTransactionsButton.Clicked += ViewMyTransactionsButton_Clicked;
        }

        private void ViewMyTransactionsButton_Clicked(object sender, EventArgs e)
        {
            this.ViewMyTransactionsButtonClick(sender, e);
        }

        private void ViewMySettlementsButton_Clicked(object sender, EventArgs e)
        {
            this.ViewMySettlementButtonClick(sender,e);
        }

        private void ViewMyBalanceHistoryButton_Clicked(object sender, EventArgs e)
        {
            this.ViewMyBalanceHistoryButtonClick(sender, e);
        }

        #endregion
    }
}