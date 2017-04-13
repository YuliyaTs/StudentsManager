using System;
using System.Globalization;
using System.Windows.Data;

namespace StudentsManager.Converters
{
    public class StudentNameSurnameConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var name = (string) values[0];
            var surname = (string)values[1];

            return name + " " + surname;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
