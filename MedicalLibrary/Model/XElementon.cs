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
using MedicalLibrary.Model;

namespace MedicaLibrary.Model
{
    public sealed class XElementon
    {

        private Patient _Patient;
        public Patient Patient
        {
            get { return _Patient; }
            set { _Patient = value; }
        }

        private Visit _Visit;
        public Visit Visit
        {
            get { return _Visit; }
            set { _Visit = value; }
        }

        private Storehouse _Storehouse;
        public Storehouse Storehouse
        {
            get { return _Storehouse; }
            set { _Storehouse = value; }
        }

        private Rule _Rule;
        public Rule Rule
        {
            get { return _Rule; }
            set { _Rule = value; }
        }

        private Field _Field;
        public Field Field
        {
            get { return _Field; }
            set { _Field = value; }
        }

        private Modification _Modification;
        public Modification Modification
        {
            get { return _Modification; }
            set { _Modification = value; }
        }

        private SendModifications _SendModifications;
        public SendModifications SendModifications
        {
            get { return _SendModifications; }
            set { _SendModifications = value; }
        }


        public void Load()
        {
            LoadRaw();
            Save();
            InitializeComponents();
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

        public void setDatabase(XElement lib)
        {
            database = lib;
            InitializeComponents();

        }

        private void InitializeComponents()
        {
            Patient = new Patient();
            Visit = new Visit();
            Storehouse = new Storehouse();
            Rule = new Rule();
            Field = new Field();
            Modification = new Modification();
            SendModifications = new SendModifications();
        }


        public static XElementon Instance
        {
            get
            {
                if (instance == null)
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

        public XElement[] CheckingRules(XElement np, bool autonumeration = true, string storehouse = "")
        {
            XElement warehouse = null;
            XElement envelope = null;
            XElement nowy_pacjent = np;

            var sorted_storehouse_v0 = database.Elements("meta").Elements("storehouses").Elements("storehouse").Where(x => x.Elements("rule").Any()).OrderBy(x => (int)x.Element("priority")); //fluent syntax
            var sorted_storehouse = from qmeta in database.Elements("meta")
                                    from storehouses in database.Elements("storehouses")
                                    from qstorehouse in storehouses.Elements("storehouse")
                                        //where qstorehouse.Elements("rule").Any()
                                    orderby int.Parse((string)qstorehouse.Element("priority")) descending
                                    select qstorehouse;

            XElement forwarehouse = null;
            foreach (var qstorehouse in sorted_storehouse_v0)
            {
                if (storehouse == "")
                {
                    bool fits = false;

                    foreach (var qrule in qstorehouse.Elements("rule"))
                    {
                        fits = false;

                        if ((string)qrule.Element("attribute") == "lastvisit" && nowy_pacjent.Elements("visit").Any()) //Check does it work
                        {
                            if ((string)qrule.Element("operation") == "greater")
                            {
                                //nowy_pacjent.Element(qrule.Element("attribute").Value).Value //używamy (string)x a nie x.Value od dziś!
                                var a = nowy_pacjent.Elements("visit").Max(x => x.Element("visit_addition_date"));
                                var b = (string)a.Element("visit_addition_date");


                                var datetime = Convert.ToDateTime(Convert.ToInt64(b));
                                if ((DateTime.Now - datetime).TotalDays > Convert.ToInt64(qrule.Element("value")))
                                {
                                    fits = true;
                                }
                            }
                        }



                        if (nowy_pacjent.Elements((string)qrule.Element("attribute")).Any() && (string)nowy_pacjent.Elements((string)qrule.Element("attribute")).First() != "")
                        {
                            if ((string)qrule.Element("operation") == "greater")
                            {
                                if (Convert.ToInt64((string)nowy_pacjent.Element((string)qrule.Element("attribute"))) > Convert.ToInt64((string)qrule.Element("value")))
                                {
                                    fits = true;
                                }
                            }
                            else if ((string)qrule.Element("operation") == "lesser")
                            {
                                if (Convert.ToInt64((string)nowy_pacjent.Element((string)qrule.Element("attribute"))) < Convert.ToInt64((string)qrule.Element("value")))
                                {
                                    fits = true;
                                }
                            }
                            else if ((string)qrule.Element("operation") == "equal")
                            {
                                if ((string)nowy_pacjent.Element((string)qrule.Element("attribute")) == (string)qrule.Element("value"))
                                {
                                    fits = true;
                                }
                            }
                            if (fits == true)
                                break;
                        }
                    }
                    if (fits == true)
                        forwarehouse = qstorehouse.Element("name");
                }
                else if (storehouse == (string)qstorehouse.Element("name"))
                {
                    forwarehouse = qstorehouse.Element("name");
                }


                //Fragment przypisujący odpowiedni envelope na podstawie określonego storehouse
                if (envelope == null && forwarehouse != null)
                {
                    envelope = qstorehouse.Descendants("hole").FirstOrDefault(); //OrDefault żeby nie rzucał wyjątkami lecz przypisywał nulla w przypadku braku
                    if (envelope == null) //Jeśli nie ma dziur to chwytaj max
                    {
                        var maxe = qstorehouse.Descendants("max_envelope").First(); //TODO: gdzie istnieje max? zmienić nazwę naszego maxe.
                        envelope = new XElement(XElement.Parse("<envelope>" + (string)maxe + "</envelope>")); //kopia a nie wskaźnik
                        if (autonumeration)
                            maxe.Value = (Convert.ToInt32(maxe.Value) + 1).ToString(); //Zwiększamy element max o 1!
                    }
                    else
                    {
                        var temp = (string)envelope;
                        if (autonumeration)
                            envelope.Remove();
                        envelope = new XElement(XElement.Parse("<envelope>" + temp + "</envelope>")); //kopia a nie wskaźnik, whatever żeby envelope wciąż było XElement i dało się wykonać XElement.value
                    }
                    warehouse = new XElement(XElement.Parse("<storehouse>" + (string)forwarehouse + "</storehouse>"));

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
        } //TODO - protected

        public string AutonumerateModifications()
        {
            //Autonumeracja ID
            var max_idm = database.Descendants("max_idm").First();
            var idm = max_idm.Value;
            return max_idm.Value = (Convert.ToInt16(max_idm.Value) + 1).ToString();
        } //TODO - protected?


        //Zmień-cokolwiek
        public void ChangeX(string datatype, int id, Tuple<string, string>[] modifications, bool log = true)
        {
            XElement modify = null;
            var nodetype = "";
            if (datatype == "patient")
            {
                modify = XElementon.Instance.Patient.WithIDP(id).FirstOrDefault();
                nodetype = "patient";
            }
            else if (datatype == "visit")
            {
                modify = XElementon.Instance.Visit.WithIDV(id).FirstOrDefault();
                nodetype = "visit";
            }
            else if (datatype == "storehouse")
            {
                modify = XElementon.Instance.Storehouse.WithIDS(id).FirstOrDefault();
                nodetype = "storehouse";
            }
            else if (datatype == "rule")
            {
                modify = XElementon.Instance.Rule.WithIDR(id).FirstOrDefault();
                nodetype = "rule";
            }
            else if (datatype == "field")
            {
                modify = XElementon.Instance.Field.WithIDF(id).FirstOrDefault();
                nodetype = "field";
            }


            if (modify == null)
            {
                return;
            }

            List<XElement> olddatalist = new List<XElement>();
            List<XElement> newdatalist = new List<XElement>();
            foreach (var modification in modifications)
            {
                if (modify.Elements(modification.Item1).Any())
                {
                    olddatalist.Add(new XElement(XElement.Parse("<" + modification.Item1 + ">" + (string)modify.Element(modification.Item1) + "</" + modification.Item1 + ">")));
                    newdatalist.Add(new XElement(XElement.Parse("<" + modification.Item1 + ">" + modification.Item2 + "</" + modification.Item1 + ">")));
                    modify.Element(modification.Item1).Value = modification.Item2;
                }
                else
                {
                    olddatalist.Add(new XElement(XElement.Parse("<" + modification.Item1 + ">" + "</" + modification.Item1 + ">")));
                    newdatalist.Add(new XElement(XElement.Parse("<" + modification.Item1 + ">" + modification.Item2 + "</" + modification.Item1 + ">")));
                    modify.Add(new XElement(XElement.Parse("<" + modification.Item1 + ">" + modification.Item2 + "</" + modification.Item1 + ">")));
                }
            }
            //Dodanie modyfikacji na potrzeby Revertów i wysyłanie Logu zmian
            if (log)
            {
                XElement mdpamodification = new XElement("modification");
                mdpamodification.Add(new XElement("idm", AutonumerateModifications()));
                mdpamodification.Add(new XElement("operation", "E"));
                mdpamodification.Add(new XElement("node_type", nodetype));
                mdpamodification.Add(new XElement("id", id));

                if (nodetype == "visit")
                {
                    string idp = (string)modify.Parent.Element("idp");
                    mdpamodification.Add(new XElement("idp", idp));
                }
                else if (nodetype == "rule")
                {
                    string ids = (string)modify.Parent.Element("ids");
                    mdpamodification.Add(new XElement("ids", ids));
                }

                XElement olddata = new XElement("olddata");
                foreach (var mod in olddatalist)
                {
                    olddata.Add(mod);
                }
                XElement newdata = new XElement("newdata");
                foreach (var mod in newdatalist)
                {
                    newdata.Add(mod);
                }

                mdpamodification.Add(olddata);
                mdpamodification.Add(newdata);

                mdpamodification = XElementon.Instance.Modification.MergeModifications(mdpamodification);
                //if(mdpamodification != null) //Potrzebne?
                //{
                    database.Descendants("modifications").First().Add(mdpamodification);
                //}
            }
        }

        //Usuń-cokolwiek
        public void DeleteX(string datatype, int id, bool log = true)
        {
            XElement modify = null;
            var nodetype = "";
            if (datatype == "patient")
            {
                modify = XElementon.Instance.Patient.WithIDP(id).FirstOrDefault();
                nodetype = "patient";
            }
            else if (datatype == "visit")
            {
                modify = XElementon.Instance.Visit.WithIDV(id).FirstOrDefault();
                nodetype = "visit";
            }
            else if (datatype == "storehouse")
            {
                modify = XElementon.Instance.Storehouse.WithIDS(id).FirstOrDefault();
                nodetype = "storehouse";
            }
            else if (datatype == "rule")
            {
                modify = XElementon.Instance.Rule.WithIDR(id).FirstOrDefault();
                nodetype = "rule";
            }
            else if (datatype == "field")
            {
                modify = XElementon.Instance.Field.WithIDF(id).FirstOrDefault();
                nodetype = "field";
            }


            if (modify == null)
            {
                return;
            }

            if (nodetype == "patient")
            {
                var storename = (string)modify.Element("storehouse");
                var storehouse = XElementon.Instance.Storehouse.WithName(storename);
                if (storehouse.Any())
                {
                    storehouse.First().Element("autonumeration").Element("holes").Add(new XElement("hole", id));
                }
                else
                {
                    return;
                }
            }

            //Dodanie modyfikacji na potrzeby Revertów i wysyłanie Logu zmian
            if (log)
            {
                XElement mdpamodification = new XElement("modification");
                mdpamodification.Add(new XElement("idm", AutonumerateModifications()));
                mdpamodification.Add(new XElement("operation", "D"));
                mdpamodification.Add(new XElement("node_type", nodetype));
                mdpamodification.Add(new XElement("id", id));

                if (nodetype == "visit")
                {
                    string idp = (string)modify.Parent.Element("idp");
                    mdpamodification.Add(new XElement("idp", idp));
                }
                else if (nodetype == "rule")
                {
                    string ids = (string)modify.Parent.Element("ids");
                    mdpamodification.Add(new XElement("ids", ids));
                }

                XElement olddata = new XElement("olddata");
                foreach (var node in modify.Elements())
                {
                    olddata.Add(node);
                }
                mdpamodification.Add(olddata);
                mdpamodification.Add(new XElement("newdata"));


                mdpamodification = XElementon.Instance.Modification.ClearModificationsAfterDelete(mdpamodification);

                //if(mdpamodification != null) //Potrzebne?
                //{
                    database.Descendants("modifications").First().Add(mdpamodification);
                //}
            }
            
            modify.Remove();
        }

        //HAXY na potrzeby MagazinePageViewModel
        public int GetMaxIDS()
        {
            return (int)database.Element("meta").Element("max_ids") - 1;
        }


        public void SendModification() //idm? z góry na dół?
        {
            var modification = database.Element("meta").Element("modifications").Elements("modification").First();

            var operacja = (string)modification.Element("operation");
            var dana = (string)modification.Element("node_type");
            var id = (string)modification.Element("id");


            //Wyślij modyfikacje "modyfikacja.dbo"
            //Wyślij modyfikacje "dane_modyfikacji.dbo"

            //Pobierz z RESTa ów element (dana o podanym id)
            //Zmień wczytanego elementa
            //Przeczytaj operację i Wysłać nowego elementa (ze zmienionymi danymi na podstawie newdata)

        }

        public void FillDatabase()
        {
            Random RNG = new Random();
            
            var listaimion = new List<string> { "Jakub", "Julia", "Kacper", "Maja", "Szymon", "Zuzanna", "Mateusz", "Wiktoria", "Filip", "Oliwia", "Michał", "Amelia", "Bartosz", "Natalia", "Wiktor", "Aleksandra", "Piotr", "Lena", "Dawid", "Nikola", "Adam", "Zofia", "Maciej", "Martyna", "Jan", "Weronika", "Igor", "Anna", "Mikołaj", "Emilia", "Patryk", "Magdalena", "Paweł", "Hanna", "Dominik", "Karolina", "Oskar", "Gabriela", "Antoni", "Alicja" };
            var listanazwisk = new List<string> { "Nowak", "Kowalski", "Wiśniewski", "Dąbrowski", "Lewandowski", "Wójcik", "Kamiński", "Kowalczyk", "Zieliński", "Szymański", "Woźniak", "Kozłowski", "Jankowski", "Wojciechowski", "Kwiatkowski", "Kaczmarek", "Mazur", "Krawczyk", "Piotrowski", "Grabowski", "Nowakowski", "Pawłowski", "Michalski", "Nowicki", "Adamczyk", "Dudek", "Zając", "Wieczorek", "Jabłoński", "Król", "Majewski", "Olszewski", "Jaworski", "Wróbel", "Malinowski", "Pawlak", "Witkowski", "Walczak", "Stępień", "Górski", "Rutkowski", "Michalak", "Sikora", "Ostrowski", "Baran", "Duda", "Szewczyk", "Tomaszewski", "Pietrzak", "Marciniak", "Wróblewski", "Zalewski", "Jakubowski", "Jasiński", "Zawadzki", "Sadowski", "Bąk", "Chmielewski", "Włodarczyk", "Borkowski", "Czarnecki", "Sawicki", "Sokołowski", "Urbański", "Kubiak", "Maciejewski", "Szczepański", "Kucharski", "Wilk", "Kalinowski", "Lis" };
            var startingpesel = 12345678901;

            var listakomentarzy = new List<string> { "pytlakowski", "merkantylny", "słowianoznawczy", "taiwański", "karpi", "przedzlotowy", "saudyjskoarabski", "chlorooctowy", "burgrabski", "drobnorozpryskowy", "mgnieniowy", "niedosiężny", "truskolaski", "dioptryczny", "przewspaniały", "dziewięciostopniowy", "podażowy", "semazjologiczny", "podpróchniały", "adeński", "gwiazdorski", "aprowincjonalny", "bożeniny", "dźwiękoszczelny", "ujemny", "moręgowaty", "różnowierczy", "pokrowcowy", "salomonowy", "kamiński" };

            var listanazwfield = new List<string> {"waga",  "BMI",  "długość_nosa",  "kształt_uszu",  "kolor_oczu",  "kolor_skóry",  "kształt_nosa",  "długość_paznokci",  "długość_palców",  "IQ",  "ilość_palców",  "wada_wymowy",  "kształt_kręgosłupa",  "wada_wzroku"};
            //Pacjenci
            for (int i = 0; i < 50; i++)
            {
                Tuple<string, string> a = new Tuple<string,string> ("imie", listaimion[RNG.Next(listaimion.Count)]);
                Tuple<string, string> b = new Tuple<string, string>("nazwisko", listanazwisk[RNG.Next(listaimion.Count)]);
                Tuple<string, string> c = new Tuple<string, string>("pesel", startingpesel.ToString());
                startingpesel = startingpesel+ 11;
                Tuple<string, string>[] randomguy = { a, b, c };

                XElementon.instance.Patient.Add(randomguy);
            }

            //Wizyty
            for (int i = 0; i < 30; i++)
            {
                var listapacjentow = this.Patient.Patients().ToList();
                var losowypacjent = listapacjentow[RNG.Next(listapacjentow.Count)].Element("idp");

                var now = DateTime.Now;
                var randomtime = RNG.Next(1, 87600);
                randomtime = -randomtime;
                var randomdate = now.AddHours(randomtime).ToString();


                Tuple<string, string> a = new Tuple<string, string>("visit_addition_date", randomdate);
                Tuple<string, string> b = new Tuple<string, string>("comment", listakomentarzy[RNG.Next(listakomentarzy.Count)]);

                Tuple<string, string>[] randomvisit = { a, b };
                XElementon.instance.Visit.Add((int)losowypacjent, randomvisit);
            }

            //Customfields

            for (int i = 0; i<= 8; i++)
            {
                Tuple<string, string> a = new Tuple<string, string>("fieldname", listanazwfield[RNG.Next(listanazwfield.Count)]);
                Tuple<string, string> b = new Tuple<string, string>("fieldtype", "int");
                Tuple<string, string> c = new Tuple<string, string>("fielddefault", "8");

                Tuple<string, string>[] randomvisit = { a, b, c};

                XElementon.Instance.Field.Add(randomvisit);
            }
        }
    }
}