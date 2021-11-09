namespace TransactionMobile.Views.Admin
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Database;
    using Models;
    using Pages;
    using Pages.Admin;
    using Syncfusion.XForms.Buttons;
    using ViewModels;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Xamarin.Forms.ContentPage" />
    /// <seealso cref="IAdminPage" />
    /// <seealso cref="TransactionMobile.Pages.IPage" />
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [ExcludeFromCodeCoverage]
    public partial class AdminPage : ContentPage, IAdminPage, IPage
    {
        /// <summary>
        /// The logging database
        /// </summary>
        private readonly IDatabaseContext Database;

        #region Fields

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AdminPage" /> class.
        /// </summary>
        /// <param name="database">The logging database.</param>
        public AdminPage(IDatabaseContext database)
        {
            this.Database = database;
            this.Database.InsertLogMessage(DatabaseContext.CreateDebugLogMessage($"In {this.GetType().Name} ctor"));
            this.InitializeComponent();
        }

        #endregion

        #region Events


        #endregion

        #region Methods

        /// <summary>
        /// Occurs when [reconciliation button clicked].
        /// </summary>
        public event EventHandler ReconciliationButtonClicked;

        /// <summary>
        /// Initializes the specified view model.
        /// </summary>
        public void Init()
        {
            this.Database.InsertLogMessage(DatabaseContext.CreateDebugLogMessage($"In {this.GetType().Name} Init"));

            this.ReconciliationButton.Clicked += this.ReconciliationButton_Clicked;
        }

        /// <summary>
        /// Handles the Clicked event of the ReconciliationButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void ReconciliationButton_Clicked(object sender, EventArgs e)
        {
            this.ReconciliationButtonClicked(sender, e);
        }



        #endregion
    }
}