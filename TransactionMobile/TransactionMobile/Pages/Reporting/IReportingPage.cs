using System;
using System.Collections.Generic;
using System.Text;

namespace TransactionMobile.Pages.Reporting
{
    public interface IReportingPage
    {
        event EventHandler ViewMySettlementButtonClick;
        event EventHandler ViewMyTransactionsButtonClick;
        event EventHandler ViewMyBalanceHistoryButtonClick;

        #region Methods

        /// <summary>
        /// Initializes the specified view model.
        /// </summary>
        void Init();

        #endregion
    }
}
