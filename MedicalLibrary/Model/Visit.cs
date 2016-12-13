using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace MedicaLibrary.Model
{
    public class Visit
    {

        XElement database = XElementon.Instance.getDatabase();

        //Wyświetl wszystkie wizyty:
        public IEnumerable<XElement> Visits()
        {
            var spvisit = from qpatients in database.Elements("patient")
                          from qvisits in qpatients.Elements("visit")
                          select qvisits;
            return spvisit;
        }

        //Wyświetl wizytę o podanym idv:
        public IEnumerable<XElement> WithIDV(int idv)
        {
            var spvisit = from qpatients in database.Elements("patient")
                          from qvisits in qpatients.Elements("visit")
                          where (int)qvisits.Element("idv") == idv
                          select qvisits;
            return spvisit;
        }

        //Dodaj wizytę
        public void Add(int idp, Tuple<string, string>[] data, bool log = true)
        {

            //Szczytywanie danych z źródła
            string comment = "";
            foreach (var dat in data)
            {
                if (dat.Item1 == "comment")
                    comment = dat.Item2;
            }


            string time = DateTime.Now.ToString();

            //Autonumeracja ID
            var max_idv = database.Descendants("max_idv").First();
            var idv = max_idv.Value;
            max_idv.Value = (Convert.ToInt16(max_idv.Value) + 1).ToString();


            XElement nowa_wizyta = new XElement(
            new XElement("visit",
                new XElement("idv", idv),
                new XElement("visit_addition_date", time),
                new XElement("comment", comment), //fix
                new XElement("idp", idp)
            ));

            //Dodanie modyfikacji na potrzeby Revertów i wysyłanie Logu zmian
            if (log)
            {
                XElement vamodification = new XElement("modification",
                    new XElement("idm", XElementon.Instance.AutonumerateModifications()),
                    new XElement("operation", "A"),
                    new XElement("node_type", "visit"),
                    new XElement("id", idv),
                    new XElement("idp", idp),
                    new XElement("olddata"),
                    new XElement("newdata", nowa_wizyta.Elements())
                    );
                database.Descendants("modifications").First().Add(vamodification);
            }

            var pacjent = XElementon.Instance.Patient.WithIDP(idp);
            pacjent.First().Add(nowa_wizyta); // TODO- samo doddawanie, nie dodaje do Operations() coby przy odpalaniu tego dla debuga nie mieszać w bazie danych
            return;
        }

        //Zmiana wizyty przy użyciu tupli
        public void Change(string datatype, int id, Tuple<string, string>[] modifications, bool log = true)
        {
            XElementon.Instance.ChangeX("visit", id, modifications, log);
        }

        //Zmiana pacjenta przy użyciu tupli
        public void Delete(int id, bool log = true)
        {
            XElementon.Instance.DeleteX("visit", id, log);
        }
    }
}