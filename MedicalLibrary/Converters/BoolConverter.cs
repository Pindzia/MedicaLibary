using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MedicalLibrary.Converters
{
    public class BoolConverter:IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((string)value == "true")
                return "Prawda";
            if ((string)value == "false")
                return "Fałsz";
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public string ConvertSpecial(string value)
        {
            if (value == "true")
                return "Prawda";
            if (value == "false")
                return "Fałsz";
            return value;
        }
    }
}
