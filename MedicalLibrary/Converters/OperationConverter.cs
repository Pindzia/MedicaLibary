using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MedicalLibrary.Converters
{
    public class OperationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((string)value == "A")
                return "Dodanie";
            if ((string)value == "E")
                return "Edycja";
            if ((string)value == "D")
                return "Usunięcie";
            return "Error Konwersji AED";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((string)value == "Dodanie")
                return "A";
            if ((string)value == "Edycja")
                return "E";
            if ((string)value == "Usunięcie")
                return "D";
            return "Error Konwersji Wstecznej AED";
        }
    }
}
