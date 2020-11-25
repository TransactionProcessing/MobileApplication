namespace TransactionMobile.Views.MobileTopup
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Database;
    using Pages;
    using Xamarin.Forms;
    using Xamarin.Forms.Internals;
    using Xamarin.Forms.Xaml;

    /// <summary>
    /// Page to show the payment failure.
    /// </summary>
    /// <seealso cref="Xamarin.Forms.ContentPage" />
    /// <seealso cref="TransactionMobile.Pages.IMobileTopupPaymentFailedPage" />
    /// <seealso cref="TransactionMobile.Pages.IPage" />
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [ExcludeFromCodeCoverage]
    public partial class MobileTopupPaymentFailedPage : ContentPage, IMobileTopupPaymentFailedPage, IPage
    {
        private readonly ILoggingDatabaseContext LoggingDatabase;

        #region Fields

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MobileTopupPaymentFailedPage" /> class.
        /// </summary>
        /// <param name="loggingDatabase">The logging database.</param>
        public MobileTopupPaymentFailedPage(ILoggingDatabaseContext loggingDatabase)
        {
            this.LoggingDatabase = loggingDatabase;
            this.LoggingDatabase.InsertLogMessage(LoggingDatabaseContext.CreateDebugLogMessage($"In {this.GetType().Name} ctor"));
            this.InitializeComponent();
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when [cancel button clicked].
        /// </summary>
        public event EventHandler CancelButtonClicked;

        #endregion

        #region Methods

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public void Init()
        {
            this.LoggingDatabase.InsertLogMessage(LoggingDatabaseContext.CreateDebugLogMessage($"In {this.GetType().Name} Init"));
            this.CancelButton.Clicked += this.CancelButton_Clicked;
        }

        /// <summary>
        /// Handles the Clicked event of the CancelButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void CancelButton_Clicked(Object sender,
                                          EventArgs e)
        {
            this.CancelButtonClicked(sender, e);
        }

        #endregion
    }
}