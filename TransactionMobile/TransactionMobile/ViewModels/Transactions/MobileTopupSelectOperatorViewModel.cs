namespace TransactionMobile.ViewModels.Transactions
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Models;

    /// <summary>
    /// 
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class MobileTopupSelectOperatorViewModel
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