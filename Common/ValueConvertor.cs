using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace CommonFrontEnd.Common
{
    public class ValueConvertor : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int length = 0;
            length = values.Count();

            if (length == 3)
            {
                Tuple<object, object, object> tuple = new Tuple<object, object, object>((object)values[0], (object)values[1], (object)values[2]);
                return (object)tuple;
            }
            else
            {
                Tuple<object, object> tuple = new Tuple<object, object>((object)values[0], (object)values[1]);
                return (object)tuple;
            }

        }


        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class MyValueConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string input = value as string;
            switch (input)
            {
                case "Blue":
                    return Brushes.LightGreen;
                default:
                    return DependencyProperty.UnsetValue;
            }
        }


        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
        #endregion
    }
}
