using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;
using System.Xml;
using System.Security.Cryptography;
using System.IO;

namespace MedicaLibrary.Model
{
    public sealed class XElementon
    {
        public void Load()
        {
                LoadRaw();
                Save();
        }

        public void LoadRaw()
        {
            FileStream file = new FileStream("lib.xml", FileMode.Open, FileAccess.Read);
            int length = (int)file.Length;

            byte[] buffer = new byte[length];
            file.Read(buffer, 0, length);
            file.Close();

            string test = "";

            test = test + Encoding.UTF8.GetString(buffer);

            setDatabase(buffer);
        }

        public void Save()
        {
            database.Save(Environment.CurrentDirectory + "\\lib.xml");
            //this.changeRaw();
        }

        public void setDatabase(byte[] xml)
        {
            XDocument document = XDocument.Load(new MemoryStream(xml));
            database = document.Root;
        }

        public void setDatabase(string xml)
        {
            XDocument document = XDocument.Parse(xml);
            database = document.Root;
        }

        public XElement getDatabase()
        {
            return database;
        }


        public static XElementon Instance
        {
            get
            {
                if (instance==null)
                {
                    instance = new XElementon();
                }
                return instance;
            }
        }

        private static XElementon instance;

        private XElementon() { }
        private XElement database = null;


        //XML
        //TODO: Sprawdzić .Element vs .Elements - kiedy działają

        private XElement[] CheckingRules(XElement np)
        {
            XElement warehouse = null;
            XElement envelope = null;
            XElement nowy_pacjent = np;

            var sorted_storehouse_v0 = database.Elements("meta").Elements("storehouses").Elements("storehouse").Where(x => x.Elements("rule").Any()).OrderBy(x => x.Element("priority").Value); //fluent syntax
            var sorted_storehouse = from qmeta in database.Elements("meta")
                                    from storehouses in database.Elements("storehouses")
                                    from qstorehouse in storehouses.Elements("storehouse")
                                    //where qstorehouse.Elements("rule").Any()
                                    orderby int.Parse(qstorehouse.Element("priority").Value) descending
                                    select qstorehouse;

            XElement forwarehouse = null;
            foreach (var qstorehouse in sorted_storehouse_v0)
            {
                bool fits = false;

                foreach (var qrule in qstorehouse.Elements("rule"))
                {
                    fits = false;
                    if (nowy_pacjent.Elements(qrule.Element("attribute").Value).Any()){
                        if (qrule.Element("operation").Value == "greater")
                        {
                            if (Convert.ToInt64(nowy_pacjent.Element(qrule.Element("attribute").Value).Value) > Convert.ToInt64(qrule.Element("value").Value)) //TODO - krzaczy się gdy lewa strona nie istnieje!
                            {
                                fits = true;
                            }
                        }
                        else if (qrule.Element("operation").Value == "lesser")
                        {
                            if (Convert.ToInt64(nowy_pacjent.Element(qrule.Element("attribute").Value).Value) < Convert.ToInt64(qrule.Element("value").Value))
                            {
                                fits = true;
                            }
                        }
                        else if (qrule.Element("operation").Value == "equal")
                        {
                            if (nowy_pacjent.Element(qrule.Element("attribute").Value).Value == qrule.Element("value").Value)
                            {
                                fits = true;
                            }
                        }
                        if (fits == false)
                            break;
                    }
                }
                if (fits == true)
                    forwarehouse = qstorehouse.Element("name");

                //Fragment przypisujący odpowiedni envelope na podstawie określonego storehouse
                if (envelope == null && forwarehouse != null)
                {
                    envelope = qstorehouse.Descendants("hole").FirstOrDefault(); //OrDefault żeby nie rzucał wyjątkami lecz przypisywał nulla w przypadku braku
                    if (envelope == null) //Jeśli nie ma dziur to chwytaj max
                    {
                        var maxe = qstorehouse.Descendants("max_envelope").First(); //TODO: gdzie istnieje max? zmienić nazwę naszego maxe.
                        envelope = new XElement(XElement.Parse("<envelope>" + maxe.Value + "</envelope>")); //kopia a nie wskaźnik
                        maxe.Value = (Convert.ToInt32(maxe.Value) + 1).ToString(); //Zwiększamy element max o 1!
                    }
                    else
                    {
                        var temp = envelope.Value;
                        envelope.Remove();
                        envelope = new XElement(XElement.Parse("<envelope>" + temp + "</envelope>")); //kopia a nie wskaźnik, whatever żeby envelope wciąż było XElement i dało się wykonać XElement.value
                    }
                    warehouse = new XElement(XElement.Parse("<storehouse>" + forwarehouse.Value + "</storehouse>"));

                    XElement[] result = new XElement[2];
                    result[0] = warehouse;
                    result[1] = envelope;

                    return result;
                }
            }

            XElement[] error = new XElement[2];
            error[0] = new XElement(XElement.Parse("<storehouse>" + "error:storehousenotfound" + "</storehouse>"));
            error[1] = new XElement(XElement.Parse("<envelope>" + "error:envelopenotfound" + "</envelope>"));

            return error;
        }


        //Wyświetl wszystkich pacjentów:
        public IEnumerable<XElement> GetAllPatients() { //TODO - zwróć patienty bez wizyt (zawsze zwracaj elementy bez dzieciów?); //todo tak żeby autogenerate banglal
            var patient = database.Elements("patient");

            foreach (var xelement in patient)
            {
                var b = xelement.Value;
            }

            return patient;
        }


        //Wyświetl pacjenta spełniającego filtr: - w tym magazyn
        public IEnumerable<XElement> GetFilteredPatients(int idp)
        {
            var sppatient = database.Elements("patient").Where(i => i.Element("idp").Value == idp.ToString());
            return sppatient;
        }

        //Wyświetl wszystkie wizyty
        public IEnumerable<XElement> GetAllVisits() { 
        var visits = from qpatients in database.Elements("patient")
                         from qvisits in qpatients.Elements("visit")
                         select qvisits;
            return visits;
        }

        //Wyświetl wszystkie wizyty danego pacjenta
        public IEnumerable<XElement> GetFilteredVisits(int idv)
        {
            var spvisit = from qpatients in database.Elements("patient")
                          where (int)qpatients.Element("idp") == idv
                          from qvisits in qpatients.Elements("visit")
                          select qvisits;
            return spvisit;
        }

        //Wyświetl wszystkie magazyny
        public IEnumerable<XElement> GetAllStorehouses()
        {
            var storehouse = from qmeta in database.Elements("meta") //optymalizacja?
                             from qstorehouses in qmeta.Elements("storehouses") //zmieniamy z bezpośrednio storehouse do storehouse -> storehouses
                             from qstorehouse in qstorehouses.Elements("storehouse")
                             select qstorehouse;
            return storehouse;
        }

        //Wyświetl magazyn o podanej nazwie
        public IEnumerable<XElement> GetFilteredStorehouses(int ids)
        {
            var spstorehouse = from qmeta in database.Elements("meta") //optymalizacja?
                               from qstorehouses in qmeta.Elements("storehouses") //zmieniamy z bezpośrednio storehouse do storehouse -> storehouses
                               from qstorehouse in qstorehouses.Elements("storehouse")
                               where (string)qstorehouse.Element("ids") == ids.ToString()
                               select qstorehouse;
            return spstorehouse;
        }

        public IEnumerable<XElement> GetFilteredStorehouses(string name)
        {
            var spstorehouse = from qmeta in database.Elements("meta") //optymalizacja?
                               from qstorehouses in qmeta.Elements("storehouses") //zmieniamy z bezpośrednio storehouse do storehouse -> storehouses
                               from qstorehouse in qstorehouses.Elements("storehouse")
                               where (string)qstorehouse.Element("name") == name
                               select qstorehouse;
            return spstorehouse;
        }

        //Wyświetl wszystkie reguły magazynów
        public IEnumerable<XElement> GetAllRules()
        {
            var rule = from qmeta in database.Elements("meta")
                       from qstorehouses in qmeta.Elements("storehouses") //zmieniamy z bezpośrednio storehouse do storehouse -> storehouses
                       from qstorehouse in qstorehouses.Elements("storehouse")
                       from qrules in qstorehouse.Elements("rule")
                       select qrules;
            return rule;
        }

        //Wyświetl wszystkie reguły magazynu o podanym id
        public IEnumerable<XElement> GetFilteredRules(int idr)
        {
            var sprule = from qmeta in database.Elements("meta")
                         from qstorehouses in qmeta.Elements("storehouses") //zmieniamy z bezpośrednio storehouse do storehouse -> storehouses
                         from qstorehouse in qstorehouses.Elements("storehouse")
                         from qrules in qstorehouse.Elements("rule")
                         where (string)qstorehouse.Element("ids") == idr.ToString()
                         select qrules;
            return sprule;
        }

        //Wyświetl wszystkie customowe pola pacjenta
        public IEnumerable<XElement> GetAllFields()
        {
            var field = from qmeta in database.Elements("meta")
                       from qstorehouses in qmeta.Elements("customfields") //zmieniamy z bezpośrednio storehouse do storehouse -> storehouses
                       from qstorehouse in qstorehouses.Elements("customfield")
                       select qstorehouse;
            return field;
        }

        public IEnumerable<XElement> GetFilteredFields(int idf)
        {
            var sprule = from qmeta in database.Elements("meta")
                         from qstorehouses in qmeta.Elements("customfields") //zmieniamy z bezpośrednio storehouse do storehouse -> storehouses
                         from qstorehouse in qstorehouses.Elements("customfield")
                         where (string)qstorehouse.Element("idf") == idf.ToString()
                         select qstorehouse;
            return sprule;
        }

        //Wyświetl wszystkich pacjentów którzy są w niepoprawnym magazynie
        public IEnumerable<XElement> GetWrongStorage()
        {
            var gjwarehouse = from qpatient in database.Elements("patient")
                              let left = CheckingRules(qpatient)[0]
                              let right = qpatient.Element("storehouse")
                              where (left != null && right != null)
                              where (left.Value == right.Value)
                              select qpatient;

            var wrwarehouse1 = from qpatient in database.Elements("patient")
                               let left = CheckingRules(qpatient)
                               let right = qpatient.Element("storehouse")
                               where (left[0] == null || right == null)
                               select qpatient; //select new { qpatient, left};
            var wrwarehouse2 = from qpatient in database.Elements("patient")
                               let left = CheckingRules(qpatient)
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

        //Ostatnio zmienieni pacjencji
        public IEnumerable<XElement> GetChangedPatients()
        {
            var cpatients = from qmeta in database.Elements("meta")
                            from qmodifications in qmeta.Elements("modifications")
                            where (string)qmodifications.Element("node_type") == "patient"
                            select qmodifications;
            return cpatients;
        }

        //Ostatnio zmienione wizyty
        public IEnumerable<XElement> GetChangedVisits()
        {
            var cvisits = from qmeta in database.Elements("meta")
                          from qmodifications in qmeta.Elements("modifications")
                          where (string)qmodifications.Element("node_type") == "visits"
                          select qmodifications;
            return cvisits;
        }

        //Ostatnio zmienione magazyny
        public IEnumerable<XElement> GetChangedStorehouses()
        {
            var cstorehouses = from qmeta in database.Elements("meta")
                               from qmodifications in qmeta.Elements("modifications")
                               where (string)qmodifications.Element("node_type") == "storehouse"
                               select qmodifications;
            return cstorehouses;
        }

        //Ostatnio zmienione zasady
        public IEnumerable<XElement> GetChangedRules()
        {
            var crules = from qmeta in database.Elements("meta")
                         from qmodifications in qmeta.Elements("modifications")
                         where (string)qmodifications.Element("node_type") == "rule"
                         select qmodifications;
            return crules;
        }

        //Dodaj pacjenta
        public void AddPatient(Tuple<string, string>[] data) //TODO argumenty
        {
            string imie = "", nazwisko = "", pesel = "";
            var customfields = XElementon.Instance.GetAllFields();
            List<XElement> fieldsindata = new List<XElement>();

            //Szczytywanie danych z źródła
            foreach (var dat in data)
            {
                if(dat.Item1 == "imie")
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
            
            if(imie == "" || nazwisko == "" || pesel == "")
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
            XElement[] warenvelope = CheckingRules(nowy_pacjent);

            //TODO - domyślny magazyn, rozpoznawanie error:error

            //Dodajemy otrzymany węzeł warehouse
            nowy_pacjent.Add(warenvelope[0]);
            //Dodajemy otrzymany węzeł envelope
            nowy_pacjent.Add(warenvelope[1]);

            //Dodanie modyfikacji na potrzeby Revertów i wysyłanie Logu zmian
            XElement pamodification = new XElement("modification",
                new XElement("operation", "A"),
                new XElement("node_type", "patient"),
                new XElement("id", nowy_pacjent.Element("idp").Value)
                );
            database.Descendants("modifications").First().Add(pamodification);

            database.Add(nowy_pacjent); //- samo doddawanie, nie dodaje do Operations() coby przy odpalaniu tego dla debuga nie mieszać w bazie danych
            return;
        }

        //Dodaj wizytę
        public void AddVisit(int idp, Tuple<string, string>[] data) {

            //Szczytywanie danych z źródła
            string comment = "";
            foreach (var dat in data)
            {
                if (dat.Item1 == "comment")
                    comment = dat.Item2;
            }


            string time = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");

            //Autonumeracja ID
            var max_idv = database.Descendants("max_idv").First();
            var idv = max_idv.Value;
            max_idv.Value = (Convert.ToInt16(max_idv.Value) + 1).ToString();


            XElement nowa_wizyta = new XElement(
            new XElement("visit",
                new XElement("idv", idv),
                new XElement("visit_addition_date", time),
                new XElement("comment", comment) //fix
            ));

            //Dodanie modyfikacji na potrzeby Revertów i wysyłanie Logu zmian
            XElement vamodification = new XElement("modification",
                new XElement("operation", "A"),
                new XElement("node_type", "visit"),
                new XElement("id", nowa_wizyta.Element("idv").Value)
                );
            database.Descendants("modifications").First().Add(vamodification);

            var pacjent = GetFilteredPatients(idp);
            pacjent.First().Add(nowa_wizyta); // TODO- samo doddawanie, nie dodaje do Operations() coby przy odpalaniu tego dla debuga nie mieszać w bazie danych
            return;
        }

        //Dodaj magazyn
        public void AddStorehouse(Tuple<string, string>[] data) {
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

            if(nazwa == "" || size == "" || priority == "")
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
            XElement samodification = new XElement("modification",
                new XElement("operation", "A"),
                new XElement("node_type", "storehouse"),
                new XElement("id", nowy_magazyn.Element("ids").Value)
                );
            database.Descendants("modifications").First().Add(samodification);

            database.Element("meta").Element("storehouses").Add(nowy_magazyn); //- samo doddawanie, nie dodaje do Operations() coby przy odpalaniu tego dla debuga nie mieszać w bazie danych
            return;
        }

        //Dodaj zasadę
        public void AddRule(int ids, Tuple<string, string>[] data) {

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

            if(attribute =="" || value == "" || (operation != "greater" && operation != "equal" && operation != "lesser"))
            {
                return;
            }

            //Autonumeracja ID
            var max_idr = database.Descendants("max_idr").First();
            var idr = max_idr.Value;
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
            XElement ramodification = new XElement("modification",
                new XElement("operation", "A"),
                new XElement("node_type", "rule"),
                new XElement("id", idr)
                );
            database.Descendants("modifications").First().Add(ramodification);

            var a = GetAllStorehouses();
            a.First().Add(nowa_zasada); //- samo doddawanie, nie dodaje do Operations() coby przy odpalaniu tego dla debuga nie mieszać w bazie danych
            return;
        }

        //Dodaj customowe pole (f - field - customfield)
        public void AddField(Tuple<string, string>[] data)
        {

            //Szczytywanie danych z źródła
            string fieldname = "", fieldtype = "", fielddefault = "";

            foreach (var dat in data)
            {
                if (dat.Item1 == "fieldname")
                    fieldname = dat.Item2;
                else if (dat.Item1 == "fieldtype")
                    fieldtype = dat.Item2;
                else if (dat.Item1 == "fielddefault")
                    fielddefault = dat.Item2;
            }

            if (fieldname == "") //fieldtype - bool. Int? uInt? String? Inne? //Jak dotyczczas jest podział na bool - checkboxy i niebool - liczby
            {
                return;
            }

            //Autonumeracja ID
            var max_idf = database.Descendants("max_idf").First();
            var idf = max_idf.Value;
            max_idf.Value = (Convert.ToInt16(max_idf.Value) + 1).ToString();

            //Tworzymy element rule który później wszczepimy w nasz dokument
            XElement nowe_pole = new XElement(
                new XElement("customfield",
                    new XElement("idf", idf),
                    new XElement("fieldname", fieldname),
                    new XElement("fieldtype", fieldtype),
                    new XElement("fielddefault", fielddefault)
                ));

            //Dodanie modyfikacji na potrzeby Revertów i wysyłanie Logu zmian
            XElement famodification = new XElement("modification",
                new XElement("operation", "A"),
                new XElement("node_type", "customfield"),
                new XElement("id", nowe_pole.Element("idf").Value)
                );
            database.Descendants("modifications").First().Add(famodification);

            database.Descendants("customfields").First().Add(nowe_pole);
            return;
        }

        //Zmień-cokolwiek
        public void ChangeX(string datatype, int id, Tuple<string,string>[] modifications)
        {
            XElement modify = null;
            var nodetype = "";
            if (datatype == "patient")
            {
                modify = GetFilteredPatients(id).FirstOrDefault();
                nodetype = "patient";
            }
            else if (datatype == "visit")
            {
                modify = GetFilteredVisits(id).FirstOrDefault();
                nodetype = "visit";
            }
            else if (datatype == "storehouse")
            {
                modify = GetFilteredStorehouses(id).FirstOrDefault();
                nodetype = "storehouse";
            }
            else if (datatype == "rule")
            {
                modify = GetFilteredRules(id).FirstOrDefault();
                nodetype = "rule";
            }
            else if (datatype == "field")
            {
                modify = GetFilteredFields(id).FirstOrDefault();
                nodetype = "field";
            }


            if (modify == null)
            {
                return;
            }

            List<XElement> modlist = new List<XElement>();
            foreach (var modification in modifications)
            {
                if (modify.Elements(modification.Item1).Any())
                {
                    modlist.Add(new XElement(XElement.Parse("<" + modification.Item1 + ">" + modify.Element(modification.Item1).Value + "</" + modification.Item1 + ">")));
                    modify.Element(modification.Item1).Value = modification.Item2;
                }
                else
                {
                    modlist.Add(new XElement(XElement.Parse("<" + modification.Item1 + ">" + "</" + modification.Item1 + ">")));
                    modify.Add(new XElement(XElement.Parse("<" + modification.Item1 + ">" + modification.Item2 + "</" + modification.Item1 + ">")));
                }
            }

            XElement mdpamodification = new XElement("modification");
            mdpamodification.Add(new XElement("operation", "E"));
            mdpamodification.Add(new XElement("node_type", nodetype));
            mdpamodification.Add(new XElement("id", id));

            XElement changes = new XElement("changes");
            foreach (var mod in modlist)
            {
                changes.Add(mod);
            }
            mdpamodification.Add(changes);

            database.Descendants("modifications").First().Add(mdpamodification);
        }

        //Usuń-cokolwiek
        public void DeleteX(string datatype, int id)
        {
            XElement modify = null;
            var nodetype = "";
            if (datatype == "patient")
            {
                modify = GetFilteredPatients(id).FirstOrDefault();
                nodetype = "patient";
            }
            else if (datatype == "visit")
            {
                modify = GetFilteredVisits(id).FirstOrDefault();
                nodetype = "visit";
            }
            else if (datatype == "storehouse")
            {
                modify = GetFilteredStorehouses(id).FirstOrDefault();
                nodetype = "storehouse";
            }
            else if (datatype == "rule")
            {
                modify = GetFilteredRules(id).FirstOrDefault();
                nodetype = "rule";
            }
            else if (datatype == "field")
            {
                modify = GetFilteredFields(id).FirstOrDefault();
                nodetype = "field";
            }


            if (modify == null)
            {
                return;
            }

            if (nodetype == "patient")
            {
                var storename = modify.Element("storehouse").Value;
                var storehouse = GetFilteredStorehouses(storename);
                if(storehouse.Any()) { 
                    storehouse.First().Element("autonumeration").Element("holes").Add(new XElement("hole",id));
                }
                else
                {
                    return;
                }
            }

            XElement mdpamodification = new XElement("modification");
            mdpamodification.Add(new XElement("operation", "D"));
            mdpamodification.Add(new XElement("node_type", nodetype));
            mdpamodification.Add(new XElement("id", id));

            XElement changes = new XElement("changes");
            foreach (var node in modify.Elements())
            {
                changes.Add(node);
            }
            mdpamodification.Add(changes);

            modify.Remove();
            database.Descendants("modifications").First().Add(mdpamodification);
        }
    }
}
