namespace TransactionMobile.Database
{
    using System;
    using SQLite;

    /// <summary>
    /// 
    /// </summary>
    public class OperatorTotals
    {
        #region Properties

        /// <summary>
        /// Gets or sets the last update date time.
        /// </summary>
        /// <value>
        /// The last update date time.
        /// </value>
        public DateTime LastUpdateDateTime { get; set; }

        /// <summary>
        /// Gets or sets the operator identifier.
        /// </summary>
        /// <value>
        /// The operator identifier.
        /// </value>
        [PrimaryKey]
        public String OperatorId { get; set; }

        /// <summary>
        /// Gets or sets the transaction count.
        /// </summary>
        /// <value>
        /// The transaction count.
        /// </value>
        public Int32 TransactionCount { get; set; }

        /// <summary>
        /// Gets or sets the transaction value.
        /// </summary>
        /// <value>
        /// The transaction value.
        /// </value>
        public Decimal TransactionValue { get; set; }

        #endregion
    }
}