using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace StudentsManager.Converters
{
    [ValueConversion(typeof(object), typeof(string))]
    public class NumberToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string stringGrade;
            var number = value.ToString();

            switch (number)
            {
                case "1":
                    stringGrade = "First";
                    break;

                case "2":
                    stringGrade = "Second";
                    break;

                case "3":
                    stringGrade = "Third";
                    break;

                case "4":
                    stringGrade = "Fourth";
                    break;

                case "5":
                    stringGrade = "Fifth";
                    break;

                case "6":
                    stringGrade = "Sixth";
                    break;
                default:
                    stringGrade = null;
                    break;
            }

            return stringGrade;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
