﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace MedicaLibrary.Model
{
    public class Storehouse
    {

        XElement database = XElementon.Instance.getDatabase();

        //Wyświetl wszystkie magazyny
        public IEnumerable<XElement> Storehouses()
        {
            var storehouse = from qmeta in database.Elements("meta") //optymalizacja?
                             from qstorehouses in qmeta.Elements("storehouses") //zmieniamy z bezpośrednio storehouse do storehouse -> storehouses
                             from qstorehouse in qstorehouses.Elements("storehouse")
                             select qstorehouse;
            return storehouse;
        }

        //Wyświetl magazyn o podanym ids
        public IEnumerable<XElement> WithIDS(int ids)
        {
            var spstorehouse = from qmeta in database.Elements("meta") //optymalizacja?
                               from qstorehouses in qmeta.Elements("storehouses") //zmieniamy z bezpośrednio storehouse do storehouse -> storehouses
                               from qstorehouse in qstorehouses.Elements("storehouse")
                               where (int)qstorehouse.Element("ids") == ids //TODO co gdy nie można rzutować stringa na inta?
                               select qstorehouse;
            return spstorehouse;
        }

        //Wyświetl magazyn o podanej nazwie
        public IEnumerable<XElement> WithName(string name)
        {
            var spstorehouse = from qmeta in database.Elements("meta") //optymalizacja?
                               from qstorehouses in qmeta.Elements("storehouses") //zmieniamy z bezpośrednio storehouse do storehouse -> storehouses
                               from qstorehouse in qstorehouses.Elements("storehouse")
                               where (string)qstorehouse.Element("name") == name
                               select qstorehouse;
            return spstorehouse;
        }

        //Dodaj magazyn
        public void Add(Tuple<string, string>[] data, bool log = true)
        {
            //Szczytywanie danych z źródła
            string nazwa = "", priority = "", size = "";

            foreach (var dat in data)
            {
                if (dat.Item1 == "name")
                    nazwa = dat.Item2;
                else if (dat.Item1 == "size")
                    size = dat.Item2;
                else if (dat.Item1 == "priority")
                    priority = dat.Item2;
            }

            if (nazwa == "" || size == "" || priority == "")
            {
                return;
            }

            //Autonumeracja ID
            var max_ids = database.Descendants("max_ids").First();
            var ids = max_ids.Value;
            max_ids.Value = (Convert.ToInt16(max_ids.Value) + 1).ToString();

            //TODO: Fragment kodu zapewniający brak kolizji na priority?

            //Tworzymy element storehouse który później wszczepimy w nasz dokument 
            XElement nowy_magazyn = new XElement(
                new XElement("storehouse",
                    new XElement("ids", ids),
                    new XElement("name", nazwa),
                        new XElement("autonumeration",
                            new XElement("max_envelope", 1),
                            new XElement("holes")),
                    new XElement("size", size),
                    new XElement("priority", priority)
                ));

            //Dodanie modyfikacji na potrzeby Revertów i wysyłanie Logu zmian
            if (log)
            {
                XElement samodification = new XElement("modification",
                    new XElement("idm", XElementon.Instance.AutonumerateModifications()),
                    new XElement("operation", "A"),
                    new XElement("node_type", "storehouse"),
                    new XElement("id", nowy_magazyn.Element("ids").Value),
                    new XElement("olddata"),
                    new XElement("newdata", nowy_magazyn.Elements())
                    );
                database.Descendants("modifications").First().Add(samodification);
            }

            database.Element("meta").Element("storehouses").Add(nowy_magazyn); //- samo doddawanie, nie dodaje do Operations() coby przy odpalaniu tego dla debuga nie mieszać w bazie danych
            return;
        }

        //Zmiana wizyty przy użyciu tupli
        public void Change(string datatype, int id, Tuple<string, string>[] modifications, bool log = true)
        {
            XElementon.Instance.ChangeX("storehouse", id, modifications, log);
        }

        //Zmiana pacjenta przy użyciu tupli
        public void Delete(string datatype, int id, bool log = true)
        {
            XElementon.Instance.DeleteX("storehouse", id, log);
        }


    }

}