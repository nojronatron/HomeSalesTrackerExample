using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace HomeSalesTrackerApp.Converters
{
    public class CurrencyDisplayConverter : IValueConverter
    {
        //  The object to convert will be a Decimal
        //  TODO: Convert the input Decimal into an object that will display currency with commas but no 10ths or lesser rations
        
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
