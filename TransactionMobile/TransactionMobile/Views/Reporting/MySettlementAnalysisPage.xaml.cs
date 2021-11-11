using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TransactionMobile.Views.Reporting
{
    using System.Collections.ObjectModel;
    using Pages;
    using Pages.Reporting;
    using Syncfusion.SfChart.XForms;
    using ViewModels.Reporting;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MySettlementAnalysisPage : ContentPage, IMySettlementsAnalysisPage, IPage
    {
        public MySettlementAnalysisPage()
        {
            InitializeComponent();
        }

        private MySettlementAnalysisViewModel ViewModel;

        public void Init(MySettlementAnalysisViewModel viewModel)
        {
            this.ViewModel = viewModel;

            var totalValue = viewModel.SettlementFeeListItems.Sum(x => x.CalculatedValue);
            var totalCount = Convert.ToDouble(viewModel.SettlementFeeListItems.Count());
            // Convert the data
            var valueData = viewModel.SettlementFeeListItems.GroupBy(x => new
                                                                          {
                                                                              x.OperatorIdentifier
                                                                          }).Select(f => 
                                                                                        new ChartDataPoint(
                                                                                                           f.Key.OperatorIdentifier,
                                                                                                           Math.Round(Convert.ToDouble(f.Sum(x => x.CalculatedValue) /
                                                                                                                    totalValue),
                                                                                                                2) * 100));

                                                                                var countData = viewModel.SettlementFeeListItems.GroupBy(x => new
                                                                                {
                                                                                    x.OperatorIdentifier
                                                                                }).Select(f =>
                                                                                              new ChartDataPoint(
                                                                                                                 f.Key.OperatorIdentifier,
                                                                                                                 Math.Round(Convert.ToDouble(Convert.ToDouble(f.Count()) /
                                                                                                           totalCount),
                                                                                                                     2) * 100));
            ObservableCollection<ChartDataPoint> valueDataPoints = new ObservableCollection<ChartDataPoint>();
            foreach (ChartDataPoint chartDataPoint in valueData)
            {
                valueDataPoints.Add(chartDataPoint);
            }

            ObservableCollection<ChartDataPoint> countDataPoints = new ObservableCollection<ChartDataPoint>();
            foreach (ChartDataPoint chartDataPoint in countData)
            {
                countDataPoints.Add(chartDataPoint);
            }

            this.SettlementValueByOperator.ItemsSource = valueDataPoints;
            this.SettlementCountByOperator.ItemsSource = countDataPoints;

        }
    }
}