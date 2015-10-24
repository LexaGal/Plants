using System;
using System.Globalization;
using System.Windows.Data;

namespace PlantsWpf.Converters
{
    public class StringToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string itemCode = (string)value;

            return (itemCode == "yes");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool toBeInvoiced = (bool)value;

            return toBeInvoiced ? "yes" : "no";
        }
    }
}