using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace MedicalLibrary.Model
{
    public class Field
    {

        XElement database = XElementon.Instance.getDatabase();

        //Wyświetl wszystkie customowe pola pacjentów
        public IEnumerable<XElement> Fields()
        {
            var field = from qmeta in database.Elements("meta")
                        from qstorehouses in qmeta.Elements("customfields") //zmieniamy z bezpośrednio storehouse do storehouse -> storehouses
                        from qstorehouse in qstorehouses.Elements("customfield")
                        select qstorehouse;
            return field;
        }

        //Wyświetl customowe pole o podanym idf
        public IEnumerable<XElement> WithIDF(int idf)
        {
            var sprule = from qmeta in database.Elements("meta")
                         from qstorehouses in qmeta.Elements("customfields") //zmieniamy z bezpośrednio storehouse do storehouse -> storehouses
                         from qstorehouse in qstorehouses.Elements("customfield")
                         where (string)qstorehouse.Element("idf") == idf.ToString()
                         select qstorehouse;
            return sprule;
        }

        //Dodaj customowe pole (f - field - customfield)
        public void Add(Tuple<string, string>[] data, bool log = true)
        {

            //Szczytywanie danych z źródła
            string fieldname = "", fieldtype = "", fielddefault = "", idf = null;

            foreach (var dat in data)
            {
                if (dat.Item1 == "fieldname")
                    fieldname = dat.Item2;
                else if (dat.Item1 == "fieldtype")
                    fieldtype = dat.Item2;
                else if (dat.Item1 == "fielddefault")
                    fielddefault = dat.Item2;
                else if (dat.Item1 == "idf" && !log)
                {
                    idf = dat.Item2;
                }
            }

            if (fieldname == "") //fieldtype - bool. Int? uInt? String? Inne? //Jak dotyczczas jest podział na bool - checkboxy i niebool - liczby
            {
                return;
            }

            if (log)
            {
                //Autonumeracja ID
                var max_idf = database.Descendants("max_idf").First();
                idf = max_idf.Value;
                max_idf.Value = (Convert.ToInt16(max_idf.Value) + 1).ToString();
            }


            //Tworzymy element rule który później wszczepimy w nasz dokument
            XElement nowe_pole = new XElement(
                new XElement("customfield",
                    new XElement("idf", idf),
                    new XElement("fieldname", fieldname),
                    new XElement("fieldtype", fieldtype),
                    new XElement("fielddefault", fielddefault)
                ));

            //Dodanie modyfikacji na potrzeby Revertów i wysyłanie Logu zmian
            if (log)
            {
                XElement famodification = new XElement("modification",
                    new XElement("idm", XElementon.Instance.AutonumerateModifications()),
                    new XElement("operation", "A"),
                    new XElement("node_type", "field"),
                    new XElement("id", idf),
                    new XElement("olddata"),
                    new XElement("newdata", nowe_pole.Elements())
                    );
                database.Descendants("modifications").First().Add(famodification);
            }

            database.Descendants("customfields").First().Add(nowe_pole);
            return;
        }

        //Zmiana wizyty przy użyciu tupli
        public void Change(int id, Tuple<string, string>[] modifications, bool log = true)
        {
            XElementon.Instance.ChangeX("field", id, modifications, log);
        }

        //Zmiana pacjenta przy użyciu tupli
        public void Delete(int id, bool log = true)
        {
            XElementon.Instance.DeleteX("field", id, log);
        }


    }

}