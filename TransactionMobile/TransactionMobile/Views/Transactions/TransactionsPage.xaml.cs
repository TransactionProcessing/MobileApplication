namespace TransactionMobile.Views.Transactions
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Common;
    using Database;
    using Pages;
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
    public partial class TransactionsPage : ContentPage, ITransactionsPage, IPage
    {
        private readonly IDatabaseContext Database;

        #region Fields
        
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionsPage" /> class.
        /// </summary>
        /// <param name="analysisLogger">The analysis logger.</param>
        /// <param name="device">The device.</param>
        public TransactionsPage(IDatabaseContext database,
                                IDevice device)
        {
            this.Database = database;
            this.Database.InsertLogMessage(DatabaseContext.CreateDebugLogMessage($"In {this.GetType().Name} ctor"));
            this.InitializeComponent();
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when [admin button click].
        /// </summary>
        public event EventHandler AdminButtonClick;

        /// <summary>
        /// Occurs when [bill payment button click].
        /// </summary>
        public event EventHandler BillPaymentButtonClick;

        /// <summary>
        /// Occurs when [mobile topup button click].
        /// </summary>
        public event EventHandler MobileTopupButtonClick;

        /// <summary>
        /// Occurs when [mobile wallet button click].
        /// </summary>
        public event EventHandler MobileWalletButtonClick;

        /// <summary>
        /// Occurs when [voucher button click].
        /// </summary>
        public event EventHandler VoucherButtonClick;
        #endregion

        #region Methods

        /// <summary>
        /// Initializes the specified view model.
        /// </summary>
        public void Init()
        {
            this.Database.InsertLogMessage(DatabaseContext.CreateDebugLogMessage($"In {this.GetType().Name} Init"));

            this.MobileTopupButton.Clicked += this.MobileTopupButton_Clicked;
            this.MobileWalletButton.Clicked += this.MobileWalletButton_Clicked;
            this.BillPaymentButton.Clicked += this.BillPaymentButton_Clicked;
            this.VoucherButton.Clicked += this.VoucherButton_Clicked;
            this.AdminButton.Clicked += this.AdminButton_Clicked;
        }

        /// <summary>
        /// Handles the Clicked event of the VoucherButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void VoucherButton_Clicked(object sender, EventArgs e)
        {
            this.VoucherButtonClick(sender, e);
        }

        /// <summary>
        /// Handles the Clicked event of the AdminButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void AdminButton_Clicked(Object sender,
                                         EventArgs e)
        {
            this.AdminButtonClick(sender, e);
        }

        /// <summary>
        /// Handles the Clicked event of the BillPaymentButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void BillPaymentButton_Clicked(Object sender,
                                               EventArgs e)
        {
            this.BillPaymentButtonClick(sender, e);
        }

        /// <summary>
        /// Handles the Clicked event of the MobileTopupButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void MobileTopupButton_Clicked(Object sender,
                                               EventArgs e)
        {
            this.MobileTopupButtonClick(sender, e);
        }

        /// <summary>
        /// Handles the Clicked event of the MobileWalletButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void MobileWalletButton_Clicked(Object sender,
                                                EventArgs e)
        {
            this.MobileWalletButtonClick(sender, e);
        }

        #endregion
    }
}