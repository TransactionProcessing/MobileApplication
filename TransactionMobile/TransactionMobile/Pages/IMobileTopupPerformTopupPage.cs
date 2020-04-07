namespace TransactionMobile.Pages
{
    using System;
    using Common;
    using ViewModels;

    /// <summary>
    /// 
    /// </summary>
    public interface IMobileTopupPerformTopupPage
    {
        #region Events

        /// <summary>
        /// Occurs when [perform topup button clicked].
        /// </summary>
        event EventHandler PerformTopupButtonClicked;

        #endregion

        #region Methods

        /// <summary>
        /// Initializes the specified view model.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        void Init(MobileTopupPerformTopupViewModel viewModel, IDevice device);

        #endregion
    }
}