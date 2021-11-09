namespace TransactionMobile.ViewModels.Transactions
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Models;

    /// <summary>
    /// 
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class MobileTopupSelectProductViewModel
    {
        #region Properties

        /// <summary>
        /// Gets or sets the products.
        /// </summary>
        /// <value>
        /// The operators.
        /// </value>
        public List<ContractProductModel> Products { get; set; }

        #endregion
    }
}