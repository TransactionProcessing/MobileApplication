﻿namespace TransactionMobile.Views.Support
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Pages;
    using Pages.Support;
    using Pages.Transactions;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="ISupportPage" />
    /// <seealso cref="Xamarin.Forms.ContentPage" />
    /// <seealso cref="ITransactionsPage" />
    /// <seealso cref="TransactionMobile.Pages.IPage" />
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [ExcludeFromCodeCoverage]
    public partial class SupportPage : ContentPage, ISupportPage, IPage
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SupportPage" /> class.
        /// </summary>
        public SupportPage()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when [upload logs button click].
        /// </summary>
        public event EventHandler UploadLogsButtonClick;

        #endregion

        #region Methods

        /// <summary>
        /// Initializes the specified view model.
        /// </summary>
        public void Init()
        {
            this.UploadLogsButton.Clicked += this.UploadLogsButton_Clicked;
        }

        /// <summary>
        /// Handles the Clicked event of the UploadLogsButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void UploadLogsButton_Clicked(object sender, EventArgs e)
        {
            this.UploadLogsButtonClick(sender, e);
        }
        
        #endregion
    }
}