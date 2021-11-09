namespace TransactionMobile.Pages.Admin
{
    using System;

    /// <summary>
    /// 
    /// </summary>
    public interface IAdminPage
    {
        #region Events

        /// <summary>
        /// Occurs when [reconciliation button clicked].
        /// </summary>
        event EventHandler ReconciliationButtonClicked;

        #endregion

        #region Methods

        /// <summary>
        /// Initializes the specified view model.
        /// </summary>
        void Init();

        #endregion
    }
}