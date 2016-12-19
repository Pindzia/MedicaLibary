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
                return "Pacjenta";
            if ((string)value == "visit")
                return "Wizyty";
            return "Error Konwersji PV";
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
