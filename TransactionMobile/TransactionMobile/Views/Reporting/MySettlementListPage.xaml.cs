namespace TransactionMobile.Views.Reporting
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Common;
    using Database;
    using Models;
    using Pages;
    using Pages.Reporting;
    using Syncfusion.ListView.XForms;
    using Syncfusion.XForms.ComboBox;
    using ViewModels.Reporting;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;
    using SelectionChangedEventArgs = Syncfusion.XForms.ComboBox.SelectionChangedEventArgs;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MySettlementListPage : ContentPage, IMySettlementsListPage, IPage
    {
        #region Fields

        private readonly IDatabaseContext Database;

        private readonly IDevice Device;

        private MySettlementListViewModel ViewModel;

        #endregion

        #region Constructors

        public MySettlementListPage(IDatabaseContext database,
                                    IDevice device)
        {
            this.Database = database;
            this.Device = device;
            this.Database.InsertLogMessage(DatabaseContext.CreateDebugLogMessage($"In {this.GetType().Name} ctor"));
            this.InitializeComponent();
        }

        #endregion

        #region Events

        public event EventHandler<SettlementListItem> SettlementListItemSelected;

        public event EventHandler<SelectionChangedEventArgs> SettlementListPeriodChanged;

        #endregion

        #region Methods

        public void Init(List<DatePeriod> datePeriods)
        {
            this.Database.InsertLogMessage(DatabaseContext.CreateDebugLogMessage($"In {this.GetType().Name} Init"));
            this.SettlementList.SelectionChanged += this.SettlementList_SelectionChanged;
            this.DateSelection.SelectionChanged += this.DateSelection_SelectionChanged;

            this.InitialiseDatePicker(datePeriods);
        }

        public void LoadSettlementData(MySettlementListViewModel viewModel)
        {
            this.ViewModel = viewModel;

            ObservableCollection<SettlementListItem> data = new ObservableCollection<SettlementListItem>();
            foreach (SettlementListItem viewModelSettlementListItem in viewModel.SettlementListItems)
            {
                viewModelSettlementListItem.AutomationId = $"SettlementListItem{viewModelSettlementListItem.SettlementDate.ToString("yyyyMMdd")}";
                data.Add(viewModelSettlementListItem);
            }

            this.SettlementList.ItemsSource = data;
        }

        private void DateSelection_SelectionChanged(Object sender,
                                                    SelectionChangedEventArgs e)
        {
            this.SettlementListPeriodChanged(sender, e);
        }

        private void InitialiseDatePicker(List<DatePeriod> datePeriods)
        {
            

            this.DateSelection.DataSource = datePeriods;
            this.DateSelection.DisplayMemberPath = "DisplayText";
            this.DateSelection.SelectedIndex = 0;
        }

        private void SettlementList_SelectionChanged(Object sender,
                                                     ItemSelectionChangedEventArgs e)
        {
            SfListView listView = sender as SfListView;
            SettlementListItem item = listView.SelectedItem as SettlementListItem;

            this.SettlementListItemSelected(sender, item);
        }

        #endregion
    }
}