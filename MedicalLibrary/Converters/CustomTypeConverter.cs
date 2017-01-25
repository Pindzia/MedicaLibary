using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MedicalLibrary.Converters
{
    public class CustomTypeConverter:IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((string)value == "int")
                return "Liczbowy";
            if ((string)value == "string")
                return "Tekstowy";
            if ((string)value == "bool")
                return "Prawda/Fałsz";
            return "Error Konwersji PV - NodeConverter";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((string)value == "Pacjenta")
                return "patient";
            if ((string)value == "Wizyty")
                return "visit";
            return "Error Konwersji Wstecznej PV";
        }
    }
}
