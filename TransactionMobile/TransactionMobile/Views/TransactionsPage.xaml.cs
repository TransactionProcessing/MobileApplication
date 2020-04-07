namespace TransactionMobile.Views
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Common;
    using Pages;
    using Unity;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Xamarin.Forms.ContentPage" />
    /// <seealso cref="TransactionMobile.Pages.ITransactionsPage" />
    /// <seealso cref="TransactionMobile.Pages.IPage" />
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [ExcludeFromCodeCoverage]
    public partial class TransactionsPage : ContentPage, ITransactionsPage, IPage
    {
        #region Fields

        /// <summary>
        /// The device
        /// </summary>
        private readonly IDevice Device;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionsPage"/> class.
        /// </summary>
        public TransactionsPage()
        {
            this.Device = (IDevice)App.Container.Resolve(typeof(IDevice));
            this.InitializeComponent();
            this.Device.AddDebugInformation("In TransactionsPage ctor");
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

        #endregion

        #region Methods

        /// <summary>
        /// Initializes the specified view model.
        /// </summary>
        public void Init()
        {
            this.Device.AddDebugInformation("In TransactionsPage init");

            this.MobileTopupButton.Clicked += this.MobileTopupButton_Clicked;
            this.MobileWalletButton.Clicked += this.MobileWalletButton_Clicked;
            this.BillPaymentButton.Clicked += this.BillPaymentButton_Clicked;
            this.AdminButton.Clicked += this.AdminButton_Clicked;
        }

        /// <summary>
        /// Handles the Clicked event of the AdminButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void AdminButton_Clicked(Object sender,
                                         EventArgs e)
        {
            this.AdminButtonClick(sender, e);
        }

        /// <summary>
        /// Handles the Clicked event of the BillPaymentButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void BillPaymentButton_Clicked(Object sender,
                                               EventArgs e)
        {
            this.BillPaymentButtonClick(sender, e);
        }

        /// <summary>
        /// Handles the Clicked event of the MobileTopupButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void MobileTopupButton_Clicked(Object sender,
                                               EventArgs e)
        {
            this.MobileTopupButtonClick(sender, e);
        }

        /// <summary>
        /// Handles the Clicked event of the MobileWalletButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void MobileWalletButton_Clicked(Object sender,
                                                EventArgs e)
        {
            this.MobileWalletButtonClick(sender, e);
        }

        #endregion
    }
}