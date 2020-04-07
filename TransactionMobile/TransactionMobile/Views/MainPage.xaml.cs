namespace TransactionMobile.Views
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Pages;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Xamarin.Forms.ContentPage" />
    /// <seealso cref="TransactionMobile.Pages.IMainPage" />
    /// <seealso cref="TransactionMobile.Pages.IPage" />
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [ExcludeFromCodeCoverage]
    public partial class MainPage : ContentPage, IMainPage, IPage
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MainPage"/> class.
        /// </summary>
        public MainPage()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when [profile button clicked].
        /// </summary>
        public event EventHandler ProfileButtonClicked;

        /// <summary>
        /// Occurs when [reports button clicked].
        /// </summary>
        public event EventHandler ReportsButtonClicked;

        /// <summary>
        /// Occurs when [support button clicked].
        /// </summary>
        public event EventHandler SupportButtonClicked;

        /// <summary>
        /// Occurs when [transactions button clicked].
        /// </summary>
        public event EventHandler TransactionsButtonClicked;

        #endregion

        #region Methods

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public void Init()
        {
            this.TransactionsButton.Clicked += this.TransactionsButton_Clicked;
            this.ReportsButton.Clicked += this.ReportsButton_Clicked;
            this.ProfileButton.Clicked += this.ProfileButton_Clicked;
            this.SupportButton.Clicked += this.SupportButton_Clicked;
        }

        /// <summary>
        /// Handles the Clicked event of the ProfileButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void ProfileButton_Clicked(Object sender,
                                           EventArgs e)
        {
            this.ProfileButtonClicked(sender, e);
        }

        /// <summary>
        /// Handles the Clicked event of the ReportsButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void ReportsButton_Clicked(Object sender,
                                           EventArgs e)
        {
            this.ReportsButtonClicked(sender, e);
        }

        /// <summary>
        /// Handles the Clicked event of the SupportButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void SupportButton_Clicked(Object sender,
                                           EventArgs e)
        {
            this.SupportButtonClicked(sender, e);
        }

        /// <summary>
        /// Handles the Clicked event of the TransactionsButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void TransactionsButton_Clicked(Object sender,
                                                EventArgs e)
        {
            this.TransactionsButtonClicked(sender, e);
        }

        #endregion
    }
}