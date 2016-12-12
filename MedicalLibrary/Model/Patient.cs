﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace MedicaLibrary.Model
{
    public class Patient
    {

        XElement database = XElementon.Instance.getDatabase();

        //Wyświetl wszystkich pacjentów:
        public IEnumerable<XElement> Patients()
        { //TODO - zwróć patienty bez wizyt (zawsze zwracaj elementy bez dzieciów?); //todo tak żeby autogenerate banglal
            var patients = database.Elements("patient");
            return patients;
        }

        //Wyświetl pacjenta o podanym idp
        public IEnumerable<XElement> WithIDP(int idp)
        {
            var sppatient = database.Elements("patient").Where(i => i.Element("idp").Value == idp.ToString());
            return sppatient;
        }

        //Wyświetl wszystkich pacjentów którzy są w niepoprawnym magazynie
        public IEnumerable<XElement> InWrongStorehouse()
        {
            var gjwarehouse = from qpatient in database.Elements("patient")
                              let left = XElementon.Instance.CheckingRules(qpatient)[0]
                              let right = qpatient.Element("storehouse")
                              where (left != null && right != null)
                              where (left.Value == right.Value)
                              select qpatient;

            var wrwarehouse1 = from qpatient in database.Elements("patient")
                               let left = XElementon.Instance.CheckingRules(qpatient)
                               let right = qpatient.Element("storehouse")
                               where (left[0] == null || right == null)
                               select qpatient; //select new { qpatient, left};
            var wrwarehouse2 = from qpatient in database.Elements("patient")
                               let left = XElementon.Instance.CheckingRules(qpatient)
                               let right = qpatient.Element("storehouse")
                               where (left[0] != null && right != null)
                               where (left[0].Value != right.Value)
                               select qpatient; //select new { qpatient, left};

            var wrwarehouse = wrwarehouse1.Concat(wrwarehouse2);
            //TODO - przekazywanie również wartości jakie powinny być oraz rysowanie ich w dodatkowych kolumnach
            //var a = wrwarehouse.Select(x => x.qpatient);
            //var b = wrwarehouse1.Select(x => x.left);

            //IEnumerable<XElement>[] z = new IEnumerable<XElement>[] { a, b.Select(x => x[0]) , b.Select(x=> x[1])};
            //var d = a.Concat(b.Select(x => x[0])).Concat(b.Select(x => x[1]));

            return wrwarehouse;
        }

        //Dodaj pacjenta
        public void Add(Tuple<string, string>[] data, bool log = true) //TODO argumenty?
        {
            string imie = "", nazwisko = "", pesel = "";
            var customfields = XElementon.Instance.Field.Fields();
            List<XElement> fieldsindata = new List<XElement>();

            //Szczytywanie danych z źródła
            foreach (var dat in data)
            {
                if (dat.Item1 == "imie")
                    imie = dat.Item2;
                else if (dat.Item1 == "nazwisko")
                    nazwisko = dat.Item2;
                else if (dat.Item1 == "pesel")
                    pesel = dat.Item2;
                else
                {
                    foreach (var customfield in customfields)
                    {
                        if (dat.Item1 == customfield.Element("fieldname").Value.ToString())
                        {
                            fieldsindata.Add(new XElement(dat.Item1, dat.Item2));
                        }
                    }
                }
            }

            if (imie == "" || nazwisko == "" || pesel == "")
            {
                return;
            }

            //Autonumeracja po id - olewamy 'dziury'
            var max = database.Descendants("max_idp").First();
            var idp = max.Value;
            max.Value = (Convert.ToInt16(idp) + 1).ToString();

            //Tworzymy element pacjent który później wszczepimy w nasz dokument
            XElement nowy_pacjent = new XElement(
                new XElement("patient",
                    new XElement("idp", idp),
                    new XElement("imie", imie),
                    new XElement("nazwisko", nazwisko),
                    new XElement("pesel", pesel)));

            //Przypinamy do niego wcześniej rozpoznane custom fieldy
            foreach (var field in fieldsindata)
            {
                nowy_pacjent.Add(field);
            }

            //Fragment sprawdzający rule na potrzeby określenia storehouse 
            XElement[] warenvelope = XElementon.Instance.CheckingRules(nowy_pacjent);

            //TODO - domyślny magazyn, rozpoznawanie error:error

            //Dodajemy otrzymany węzeł warehouse
            nowy_pacjent.Add(warenvelope[0]);
            //Dodajemy otrzymany węzeł envelope
            nowy_pacjent.Add(warenvelope[1]);

            //Dodanie modyfikacji na potrzeby Revertów i wysyłanie Logu zmian
            if (log)
            {
                XElement pamodification = new XElement("modification",
                    new XElement("idm", XElementon.Instance.AutonumerateModifications()),
                    new XElement("operation", "A"),
                    new XElement("node_type", "patient"),
                    new XElement("id", nowy_pacjent.Element("idp").Value),
                    new XElement("olddata"),
                    new XElement("newdata", nowy_pacjent.Elements())
                    );
                database.Descendants("modifications").First().Add(pamodification);
            }

            database.Add(nowy_pacjent); //- samo doddawanie, nie dodaje do Operations() coby przy odpalaniu tego dla debuga nie mieszać w bazie danych
            return;
        }

        //Zmiana pacjenta przy użyciu tupli
        public void Change(string datatype, int id, Tuple<string, string>[] modifications, bool log = true)
        {
            XElementon.Instance.ChangeX("patient", id, modifications, log);
        }


        
        //Zmiana pacjenta przy użyciu tupli
        public void Delete(string datatype, int id, bool log = true)
        {
            XElementon.Instance.DeleteX("patient", id, log);
        }
    }
}