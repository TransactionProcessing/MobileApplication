﻿namespace TransactionMobile.Pages
{
    using System;
    using ViewModels;

    /// <summary>
    /// 
    /// </summary>
    public interface ILoginPage
    {
        #region Events

        /// <summary>
        /// Occurs when [login button click].
        /// </summary>
        event EventHandler LoginButtonClick;

        #endregion

        #region Methods

        /// <summary>
        /// Initializes the specified view model.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        void Init(LoginViewModel viewModel);

        /// <summary>
        /// Sets the registration failure message.
        /// </summary>
        /// <param name="failureMessage">The failure message.</param>
        void SetSignInFailureMessage(String failureMessage);

        #endregion
    }
}