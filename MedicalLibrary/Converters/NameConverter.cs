using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MedicalLibrary.Converters
{
    public class NameConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            string returnValue = "";
            foreach(var value in values)
            {
                string convertValue = (string)value;
                if (convertValue.Count() < 15)
                {
                    int i = 15 - convertValue.Count();
                    for (int j = 1; j <= i; j++)
                    {
                        convertValue += " ";
                    }
                }
                returnValue += convertValue;
            }
            return returnValue;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
