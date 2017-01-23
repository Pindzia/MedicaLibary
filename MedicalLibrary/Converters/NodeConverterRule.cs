using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MedicalLibrary.Converters
{
    public class NodeConverterRule :IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((string)value == "patient")
                return "Pacjent";
            if ((string)value == "idp")
                return "Id";
            if ((string)value == "imie")
                return "Imię";
            if ((string)value == "nazwisko")
                return "Nazwisko";
            if ((string)value == "pesel")
                return "Pesel";
            if ((string)value == "storehouse")
                return "Magazyn";
            if ((string)value == "envelope")
                return "Koperta";
            if ((string)value == "visit")
                return "Wizyta";
            if ((string)value == "idv")
                return "Id";
            if ((string)value == "visit_addition_date")
                return "Data Dodania Wizyty";
            if ((string)value == "comment")
                return "komentarz";
            if ((string)value == "ids")
                return "Id:";
            if ((string)value == "size")
                return "Rozmiar Magazynu";
            if ((string)value == "name")
                return "Nazwa Magazynu";
            if ((string)value == "priority")
                return "Priorytet Magazynu";
            if ((string)value == "rule")
                return "Zasada Magazynu";
            if ((string)value == "idr")
                return "Id";
            if ((string)value == "attribute")
                return "Atrybut";
            if ((string)value == "operation")
                return "Operacja";
            if ((string)value == "value")
                return "Wartość";
            if ((string)value == "customfield")
                return "Własne Pole";
            if ((string)value == "idf")
                return "Id";
            if ((string)value == "fieldname")
                return "Nazwa Pola:";
            if ((string)value == "fieldtype")
                return "Typ Pola";
            if ((string)value == "fielddefault")
                return "Wartość Domyślna";
            return value ;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
