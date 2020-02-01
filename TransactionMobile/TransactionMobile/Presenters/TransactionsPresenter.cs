namespace TransactionMobile.Presenters
{
    using System.Threading.Tasks;
    using Common;
    using Pages;
    using Xamarin.Forms;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="TransactionMobile.Presenters.ITransactionsPresenter" />
    public class TransactionsPresenter : ITransactionsPresenter
    {
        #region Fields

        /// <summary>
        /// The device
        /// </summary>
        private readonly IDevice Device;

        /// <summary>
        /// The transactions page
        /// </summary>
        private readonly ITransactionsPage TransactionsPage;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionsPresenter"/> class.
        /// </summary>
        /// <param name="transactionsPage">The transactions page.</param>
        /// <param name="device">The device.</param>
        public TransactionsPresenter(ITransactionsPage transactionsPage,
                                     IDevice device)
        {
            this.TransactionsPage = transactionsPage;
            this.Device = device;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Starts this instance.
        /// </summary>
        public async Task Start()
        {
            this.Device.AddDebugInformation("About to Init Transactions Page");
            this.TransactionsPage.Init();
            this.Device.AddDebugInformation("About to Push Transactions Page");
            await Shell.Current.Navigation.PushAsync((Page)this.TransactionsPage);
        }

        #endregion
    }
}