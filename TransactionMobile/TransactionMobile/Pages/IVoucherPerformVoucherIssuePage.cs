﻿namespace TransactionMobile.Pages
{
    using System;
    using Common;
    using ViewModels;

    /// <summary>
    /// 
    /// </summary>
    public interface IVoucherPerformVoucherIssuePage
    {
        #region Events

        /// <summary>
        /// Occurs when [issue voucher button clicked].
        /// </summary>
        event EventHandler IssueVoucherButtonClicked;

        #endregion

        #region Methods

        /// <summary>
        /// Initializes the specified view model.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        void Init(VoucherPerformVoucherIssueViewModel viewModel);

        #endregion
    }
}