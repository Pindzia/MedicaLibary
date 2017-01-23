using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace MedicalLibrary.Model
{
    public class Rule
    {

        XElement database = XElementon.Instance.getDatabase();

        //Wyświetl wszystkie reguły magazynów
        public IEnumerable<XElement> Rules()
        {
            var rule = from qmeta in database.Elements("meta")
                       from qstorehouses in qmeta.Elements("storehouses") //zmieniamy z bezpośrednio storehouse do storehouse -> storehouses
                       from qstorehouse in qstorehouses.Elements("storehouse")
                       from qrules in qstorehouse.Elements("rule")
                       select qrules;
            return rule;
        }

        //Wyświetl zasadę o podanym ids
        public IEnumerable<XElement> WithIDS(int ids)
        {
            var sprule = from qmeta in database.Elements("meta")
                         from qstorehouses in qmeta.Elements("storehouses") //zmieniamy z bezpośrednio storehouse do storehouse -> storehouses
                         from qstorehouse in qstorehouses.Elements("storehouse")
                         from qrules in qstorehouse.Elements("rule")
                         where (int)qstorehouse.Element("ids") == ids
                         select qrules;
            return sprule;
        }

        //Wyświetl zasadę o podanym idr
        public IEnumerable<XElement> WithIDR(int idr)
        {
            var sprule = from qmeta in database.Elements("meta")
                         from qstorehouses in qmeta.Elements("storehouses") //zmieniamy z bezpośrednio storehouse do storehouse -> storehouses
                         from qstorehouse in qstorehouses.Elements("storehouse")
                         from qrules in qstorehouse.Elements("rule")
                         where (int)qrules.Element("idr") == idr
                         select qrules;
            return sprule;
        }

        //Dodaj zasadę
        public void Add(int ids, Tuple<string, string>[] data, bool log = true)
        {

            //Szczytywanie danych z źródła
            string attribute = "", operation = "", value = "";

            foreach (var dat in data)
            {
                if (dat.Item1 == "attribute")
                    attribute = dat.Item2;
                else if (dat.Item1 == "operation") //greater, equal, lesser, działa dla intów i equal dla stringów, do modyfikacji w regionie checking_rules
                    operation = dat.Item2;
                else if (dat.Item1 == "value")
                    value = dat.Item2;
            }

            if (attribute == "" || value == "" || (operation != "greater" && operation != "equal" && operation != "lesser"))
            {
                return;
            }

            //Autonumeracja ID
            var max_idr = database.Descendants("max_idr").First();
            var idr = (string)max_idr;
            max_idr.Value = (Convert.ToInt16(max_idr.Value) + 1).ToString();

            //Tworzymy element rule który później wszczepimy w nasz dokument
            XElement nowa_zasada = new XElement(
                new XElement("rule",
                    new XElement("idr", idr),
                    new XElement("attribute", attribute),
                    new XElement("operation", operation),
                    new XElement("value", value)
                ));

            //Dodanie modyfikacji na potrzeby Revertów i wysyłanie Logu zmian
            if (log)
            {
                XElement ramodification = new XElement("modification",
                    new XElement("idm", XElementon.Instance.AutonumerateModifications()),
                    new XElement("operation", "A"),
                    new XElement("node_type", "rule"),
                    new XElement("id", idr),
                    new XElement("ids", ids),
                    new XElement("olddata"),
                    new XElement("newdata", nowa_zasada.Elements())
                    );
                database.Descendants("modifications").First().Add(ramodification);
            }

            var a = XElementon.Instance.Storehouse.WithIDS(ids);
            a.First().Add(nowa_zasada); //- samo doddawanie, nie dodaje do Operations() coby przy odpalaniu tego dla debuga nie mieszać w bazie danych
            return;
        }

        //Zmiana wizyty przy użyciu tupli
        public void Change(int id, Tuple<string, string>[] modifications, bool log = true)
        {
            XElementon.Instance.ChangeX("rule", id, modifications, log);
        }

        //Usuwanie Zasady
        public void Delete(int id, bool log = true)
        {
            XElementon.Instance.DeleteX("rule", id, log);
        }


    }

}