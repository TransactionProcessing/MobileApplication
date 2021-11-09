namespace TransactionMobile.Converters
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Syncfusion.ListView.XForms;
    using Syncfusion.SfChart.XForms;
    using Xamarin.Forms;

    public class SettlementListColourConverter : IValueConverter
    {
        #region Methods

        public Object Convert(Object value,
                              Type targetType,
                              Object parameter,
                              CultureInfo culture)
        {
            SfListView listview = parameter as SfListView;
            Boolean isCompleted = (Boolean)value;

            return isCompleted ? Color.LightGreen : Color.LightCoral;
        }

        public Object ConvertBack(Object value,
                                  Type targetType,
                                  Object parameter,
                                  CultureInfo culture)
        {
            return null;
        }

        #endregion
    }

    public class DataMarkerConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter != null && parameter.ToString() == "Label")
            {
                if (value is List<object>)
                {
                    return "Others";
                }
                else
                {
                    if (value != null)
                    {
                        return (value as ChartDataPoint).XValue;
                    }
                }
            }
            else
            {
                if (value is List<object>)
                {
                    return (value as List<object>).Sum(item => (item as ChartDataPoint).YValue).ToString() + "%";
                }
                else
                {
                    if (value != null)
                    {
                        return (value as ChartDataPoint).YValue + "%";
                    }
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}