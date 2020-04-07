namespace TransactionMobile.Pages
{
    using System;

    /// <summary>
    /// 
    /// </summary>
    public interface IMainPage
    {
        #region Events

        /// <summary>
        /// Occurs when [profile button clicked].
        /// </summary>
        event EventHandler ProfileButtonClicked;

        /// <summary>
        /// Occurs when [reports button clicked].
        /// </summary>
        event EventHandler ReportsButtonClicked;

        /// <summary>
        /// Occurs when [support button clicked].
        /// </summary>
        event EventHandler SupportButtonClicked;

        /// <summary>
        /// Occurs when [transactions button clicked].
        /// </summary>
        event EventHandler TransactionsButtonClicked;

        #endregion

        #region Methods

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        void Init();

        #endregion
    }
}