namespace TransactionMobile.Pages.Transactions
{
    using System;

    /// <summary>
    /// 
    /// </summary>
    public interface IMobileTopupPaymentFailedPage
    {
        #region Events

        /// <summary>
        /// Occurs when [cancel button clicked].
        /// </summary>
        event EventHandler CancelButtonClicked;

        #endregion

        #region Methods

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        void Init();

        #endregion
    }
}