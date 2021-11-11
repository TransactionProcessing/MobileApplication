namespace TransactionMobile.Pages.Reporting
{
    using System;
    using System.Collections.Generic;
    using Models;
    using Syncfusion.XForms.ComboBox;
    using ViewModels.Reporting;
    using Views.Reporting;

    public interface IMySettlementsListPage
    {
        #region Events

        event EventHandler<SettlementListItem> SettlementListItemSelected;

        event EventHandler<SelectionChangedEventArgs> SettlementListPeriodChanged;

        #endregion

        #region Methods

        /// <summary>
        /// Initializes the specified view model.
        /// </summary>
        void Init(List<DatePeriod> datePeriods);

        void LoadSettlementData(MySettlementListViewModel viewModel);

        #endregion
    }
}