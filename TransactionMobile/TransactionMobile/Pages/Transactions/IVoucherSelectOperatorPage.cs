namespace TransactionMobile.Pages.Transactions
{
    using System;
    using ViewModels.Transactions;
    using Xamarin.Forms;

    /// <summary>
    /// 
    /// </summary>
    public interface IVoucherSelectOperatorPage
    {
        #region Events

        /// <summary>
        /// Occurs when [operator selected].
        /// </summary>
        event EventHandler<SelectedItemChangedEventArgs> OperatorSelected;

        #endregion

        #region Methods

        /// <summary>
        /// Initializes the specified view model.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        void Init(VoucherSelectOperatorViewModel viewModel);

        #endregion
    }
}