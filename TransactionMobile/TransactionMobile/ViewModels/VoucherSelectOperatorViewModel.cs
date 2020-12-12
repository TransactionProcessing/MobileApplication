﻿namespace TransactionMobile.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Models;

    /// <summary>
    /// 
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class VoucherSelectOperatorViewModel
    {
        #region Properties

        /// <summary>
        /// Gets or sets the operators.
        /// </summary>
        /// <value>
        /// The operators.
        /// </value>
        public List<ContractProductModel> Operators { get; set; }

        #endregion
    }
}