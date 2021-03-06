﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace MedicalLibrary.Model
{
    public class Storehouse
    {

        XElement database = XElementon.Instance.getDatabase();

        //Wyświetl wszystkie magazyny, posortowane rosnąco po priority
        public IEnumerable<XElement> Storehouses()
        {
            var storehouse = from qmeta in database.Elements("meta") //optymalizacja?
                             from qstorehouses in qmeta.Elements("storehouses") //zmieniamy z bezpośrednio storehouse do storehouse -> storehouses
                             from qstorehouse in qstorehouses.Elements("storehouse")
                             orderby (int)qstorehouse.Element("priority")
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
            string nazwa = "", priority = "", size = "", ids = null;

            foreach (var dat in data)
            {
                if (dat.Item1 == "name")
                    nazwa = dat.Item2;
                else if (dat.Item1 == "size")
                    size = dat.Item2;
                else if (dat.Item1 == "priority")
                    priority = dat.Item2;
                else if (dat.Item1 == "ids" && !log)
                {
                    ids = dat.Item2;
                }
            }

            if (nazwa == "" || size == "" || priority == "")
            {
                return;
            }

            if (log)
            {
                //Autonumeracja ID
                var max_ids = database.Descendants("max_ids").First();
                ids = (string)max_ids;
                max_ids.Value = (Convert.ToInt16((string)max_ids) + 1).ToString();
            }

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
                    new XElement("id", (string)nowy_magazyn.Element("ids")),
                    new XElement("olddata"),
                    new XElement("newdata", nowy_magazyn.Elements())
                    );
                database.Descendants("modifications").First().Add(samodification);
            }

            database.Element("meta").Element("storehouses").Add(nowy_magazyn); //- samo doddawanie, nie dodaje do Operations() coby przy odpalaniu tego dla debuga nie mieszać w bazie danych
            return;
        }

        //Zmiana wizyty przy użyciu tupli
        public void Change(int id, Tuple<string, string>[] modifications, bool log = true)
        {
            XElementon.Instance.ChangeX("storehouse", id, modifications, log);
        }

        //Zmiana pacjenta przy użyciu tupli
        public void Delete(int id, bool log = true)
        {

            var storehouse_to_delete = this.WithIDS(id).First();
            var storehouses_with_worse_prio = this.Storehouses().Where((x => (int)x.Element("priority") > (int)storehouse_to_delete.Element("priority") && (int)x.Element("priority") < 5000));
            foreach (var storehouse in storehouses_with_worse_prio)
            {
                var up = new Tuple<string, string>("priority", (Convert.ToInt32(storehouse.Element("priority").Value)-1).ToString());

                var modifications = new Tuple<string, string>[] { up };
                XElementon.Instance.ChangeX("storehouse", (int)storehouse.Element("ids"), modifications);
            }

                XElementon.Instance.DeleteX("storehouse", id, log);
        }


        //Otrzymanie listy możliwych Atybutów
        public List<string> Attributes()
        {
            List<List<string>> attributes = new List<List<string>>();
            List<string> attributelist = new List<string> { "idp", "imie", "nazwisko", "pesel" };
            List<string> typelist = new List<string> { "int", "string", "string", "int" };
            
            foreach (var field in XElementon.Instance.Field.Fields())
            {
                attributelist.Add((string)field.Element("fieldname")); //Todo - rozpoznywanie typów?
            }
            return attributelist;
        }

        public List<string> StorehouseNameList()
        {
            List<string> a = this.Storehouses().Select(el => (string)el.Element("name")).ToList();
            a.Add("");
            return a;
        }

        public List<string> Operations(string attribute)
        {
            List<string> operationlist = new List<string>();
            string typ ="";

            if(attribute == "idp" || attribute == "pesel")
            {
                typ = "int";
            } else if (attribute == "imie" || attribute == "nazwisko")
            {
                typ = "string";
            } else
            {
                foreach (var field in XElementon.Instance.Field.Fields())
                {
                    if (attribute == (string)field.Element("fieldname"))
                    {
                        typ = (string)field.Element("fieldtype");
                    }
                }
            }

            if (typ == "int")
            {
                operationlist.Add("greater");
                operationlist.Add("equal");
                operationlist.Add("lesser");
            }
            else //if (type == "imie" || type == "nazwisko")
            {
                operationlist.Add("equal");
            }
            return operationlist;
        }


        public void MovePrioDown(XElement magazyn)
        {
            var id = (int)magazyn.Element("ids");
            var priority = (int)magazyn.Element("priority");

            var down = new Tuple<string, string>("priority", (priority+1).ToString());

            
            var a = this.Storehouses().Where(x => (int)x.Element("priority") > priority).First();
            var up = new Tuple<string, string>("priority", (priority).ToString());

            var modifications = new Tuple<string, string>[] {down};
            XElementon.Instance.ChangeX("storehouse", id, modifications);

            modifications = new Tuple<string, string>[] {up};
            XElementon.Instance.ChangeX("storehouse", (int)a.Element("ids"), modifications);
        }

        public void MovePrioUp(XElement magazyn)
        {
            var id = (int)magazyn.Element("ids");
            var priority = (int)magazyn.Element("priority");

            var down = new Tuple<string, string>("priority", (priority - 1).ToString());


            var a = this.Storehouses().Where(x => (int)x.Element("priority") < priority).Last();
            var up = new Tuple<string, string>("priority", (priority).ToString());

            var modifications = new Tuple<string, string>[] { down };
            XElementon.Instance.ChangeX("storehouse", id, modifications);

            modifications = new Tuple<string, string>[] { up };
            XElementon.Instance.ChangeX("storehouse", (int)a.Element("ids"), modifications);
        }



    }

}