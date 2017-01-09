using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MedicalLibrary.Converters
{
    public class NodeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((string)value == "patient")
                return "Pacjenci";
            if ((string)value == "visit")
                return "Wizyty";
            if ((string)value == "storehouse")
                return "Magazyny";
            if ((string)value == "rule")
                return "Zasady";
            if ((string)value == "field")
                return "Własne Pola";
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
