using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
//using Newtonsoft.Json;
using System.Xml;
using System.Linq;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MedicalLibrary.Model;

namespace MedicalLibrary.Model
{
    public class PullREST
    {
        public static async Task<XElement> PullAll(int idLekarz, string pass) //TODO ID-lekarz TODO-pass)
        {

            PushREST.SetClient();

            if (await PushREST.LoggedIn(idLekarz, pass) == false) //fix
            {
                System.Windows.MessageBox.Show("Błędne hasło!");
                return null;
            }


            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://medicalibaryrest.azurewebsites.net");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));


            XElement Patients = null;
            XElement Visits = null;
            XElement Storehouses = null;
            XElement ParamList = null;
            XElement Rules = null;
            XElement ParamBind = null;

            HttpResponseMessage result = await client.GetAsync("/pacjent/lista/" + XElementon.Instance.idLekarz.ToString());

            if (result.IsSuccessStatusCode)
            {
                var asd = await result.Content.ReadAsStringAsync();
                asd = Regex.Replace(asd, " xmlns[^> ]*", "");
                asd = Regex.Replace(asd, " i:nil=\"true\" ", "");
                Patients = XElement.Parse(asd);

                Console.WriteLine(Patients);
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)result.StatusCode, result.ReasonPhrase);
            }

            result = await client.GetAsync("/wizyta/lista/" + XElementon.Instance.idLekarz.ToString());

            if (result.IsSuccessStatusCode)
            {
                var asd = await result.Content.ReadAsStringAsync();
                asd = Regex.Replace(asd, " xmlns[^> ]*", "");
                asd = Regex.Replace(asd, " i:nil=\"true\" ", "");
                Visits = XElement.Parse(asd);

                //Console.WriteLine(asd);

            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)result.StatusCode, result.ReasonPhrase);
            }

            result = await client.GetAsync("/magazyn/lista/" + XElementon.Instance.idLekarz.ToString());

            if (result.IsSuccessStatusCode)
            {
                var asd = await result.Content.ReadAsStringAsync();
                asd = Regex.Replace(asd, " xmlns[^> ]*", "");
                asd = Regex.Replace(asd, " i:nil=\"true\" ", "");
                Storehouses = XElement.Parse(asd);

                //Console.WriteLine(asd);

            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)result.StatusCode, result.ReasonPhrase);
            }

            result = await client.GetAsync("/zasada/lista/" + XElementon.Instance.idLekarz.ToString());

            if (result.IsSuccessStatusCode)
            {
                var asd = await result.Content.ReadAsStringAsync();
                asd = Regex.Replace(asd, " xmlns[^> ]*", "");
                asd = Regex.Replace(asd, " i:nil=\"true\" ", "");
                Rules = XElement.Parse(asd);

                //    Console.WriteLine(asd);

            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)result.StatusCode, result.ReasonPhrase);
            }

            result = await client.GetAsync("/parametr/lista/" + XElementon.Instance.idLekarz.ToString());

            if (result.IsSuccessStatusCode)
            {
                var asd = await result.Content.ReadAsStringAsync();
                asd = Regex.Replace(asd, " xmlns[^> ]*", "");
                asd = Regex.Replace(asd, " i:nil=\"true\" ", "");
                ParamList = XElement.Parse(asd);

                //    Console.WriteLine(asd);

            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)result.StatusCode, result.ReasonPhrase);
            }

            result = await client.GetAsync("/przypisanie/lista/" + XElementon.Instance.idLekarz.ToString());

            if (result.IsSuccessStatusCode)
            {
                var asd = await result.Content.ReadAsStringAsync();
                asd = Regex.Replace(asd, " xmlns[^> ]*", "");
                asd = Regex.Replace(asd, " i:nil=\"true\" ", "");
                ParamBind = XElement.Parse(asd);

                //         Console.WriteLine(asd);

            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)result.StatusCode, result.ReasonPhrase);
            }

            var IEPatients = Patients.Elements("PacjentWyslijDTO");
            var IEVisits = Visits.Elements();
            var IEStorehouses = Storehouses.Elements();
            var IERules = Rules.Elements();
            var IEParamList = ParamList.Elements();
            var IEParamBind = ParamBind.Elements();

            //var IEFields = Storehouses.Elements("ArrayOfParametrNowyWyslij"); //coś odnośnie wersji?
            var root =
                from p in IEPatients
                join s in IEStorehouses
                on
                    (string)p.Element("id_magazyn")
                equals
                    (string)s.Element("id")
                select new XElement("patient",
                    new XElement("idp", (string)p.Element("id")),
                    new XElement("imie", (string)p.Element("imie")),
                    new XElement("nazwisko", (string)p.Element("nazwisko")),
                    new XElement("pesel", (string)p.Element("pesel")),
                    new XElement("storehouse", (string)s.Element("nazwa")),
                    new XElement("envelope", (string)p.Element("numer_koperty")));


            var rootlist = root.ToList();
            foreach (var visit in IEVisits ?? Enumerable.Empty<XElement>())
            {


                if (visit.Element("id_pacjent").Value != "")
                {
                    var debug = visit.Element("id_pacjent");
                    var id_pacjent = (int)visit.Element("id_pacjent");
                    var visita = new XElement("visit",
                        new XElement("idv", (string)visit.Element("id") ?? "ERROR"),
                        new XElement("visit_addition_date", (string)visit.Element("data_wizyty") ?? ""),
                        new XElement("visit_end_date", (string)visit.Element("koniec_wizyty") ?? ""),
                        new XElement("years_to_keep", (string)visit.Element("lata_waznosc") ?? ""),
                        new XElement("comment", (string)visit.Element("komentarz") ?? "")
                        );

                    var a = rootlist.Where(x => ((int)x.Element("idp") == id_pacjent));

                    a.First().Add(visita);
                }
            }


            //TODO - pacjenci  
            //przypisanie parametru

            // id_pacjent
            // id_parametr
            // wartosc

            //parametr
            // typ
            // nazwa
            // wartosc_domyslna

            //Join między  przypisanie parametru a parametr

            var parameters =
                from b in IEParamBind
                join l in IEParamList
                on
                    (int)b.Element("id_parametr")
                equals
                    (int)l.Element("id")
                select new XElement("parameter",
                    new XElement((string)l.Element("nazwa"), (string)b.Element("wartosc") ?? "DB2"),
                    new XElement("patient", (string)b.Element("id_pacjent") ?? "DB3"));



            //Wklejenie foreach customfield (wygenerowana lista) do naszego XML (where IDP = id_pacjent)
            foreach (var parameter in parameters ?? Enumerable.Empty<XElement>())
            {
                var id_pacjent = (int)rootlist.Where(x => (int)x.Elements("idp").First() == (int)parameter.Element("patient")).First().Element("idp");

                var parametera = parameter;
                parametera.Elements("patient").Remove();
                parametera = parametera.Elements().First();

                rootlist.Where(x => (int)x.Element("idp") == (int)id_pacjent).First().Add(parametera);
            }


            //WĘZEŁ META
            //wklej pusty węzeł NA GÓRZE liba

            var lib = new XElement("lib");
            lib.Add(new XElement("meta"));


            //WEZEŁ modifications
            lib.Elements("meta").First().Add(new XElement("modifications"));
            //węzłów modification NIE POBIERAMY - gdy robimy pulla ściągamy tylko dane a modifications mamy mieć puste

            //WĘZEŁ storehouses

            lib.Elements("meta").First().Add(new XElement("storehouses"));

            //Zasady wklejamy do magazynu
            var storehouselist = IEStorehouses;
            List<XElement> renamedstorehouse = new List<XElement>();

            foreach (var storehouse in storehouselist ?? Enumerable.Empty<XElement>())
            {

                var storehousea = new XElement("storehouse",
                    new XElement("ids", (string)storehouse.Element("id")),
                    new XElement("name", (string)storehouse.Element("nazwa")),
                    new XElement("size", (string)storehouse.Element("max_rozmiar")),
                    new XElement("priority", (string)storehouse.Element("priorytet")));

                renamedstorehouse.Add(storehousea);

            }

            foreach (var rule in IERules ?? Enumerable.Empty<XElement>())
            {
                var rulea = new XElement("rule",
                    new XElement("idr", (string)rule.Element("id")),
                    new XElement("attribute", (string)rule.Element("nazwa_atrybutu")),
                    new XElement("operation", (string)rule.Element("operacja_porownania")),
                    new XElement("value", (string)rule.Element("wartosc_porownania")));

                var id_storehouse = (int)rule.Element("id_magazynu");
                renamedstorehouse.Where(x => (int)x.Element("ids") == id_storehouse).First().Add(rulea);
            }



            //Węzeł autonumerations do magazynu
            foreach (var storehouse in renamedstorehouse ?? Enumerable.Empty<XElement>())
            {
                var storehousepatients =
                from patient in rootlist
                where (string)patient.Element("storehouse") == (string)storehouse.Element("name")
                orderby (int)patient.Element("envelope")
                select patient;
                int maxid = 1;


                if (storehousepatients.Any())
                    maxid = (int)storehousepatients.Last().Element("envelope");

                storehouse.Add(new XElement("autonumeration",
                    new XElement("max_envelope", maxid),
                    new XElement("holes")));


                if (storehousepatients.Any())
                {
                    int it = 1;
                    for (int iterator = 1; iterator <= maxid && it <= maxid; iterator++)
                    {
                        if ((string)storehousepatients.ElementAt(it - 1).Element("storehouse") == (string)storehouse.Element("name")
                            && (int)storehousepatients.ElementAt(it - 1).Element("envelope") == iterator)
                        {
                            it++;
                        }
                        else
                        {
                            storehouse.Element("autonumeration").Element("holes").Add(new XElement("hole", iterator));
                        }

                    }
                }
            }

            //Magazyn wklejamy do storehouses  
            lib.Elements("meta").First().Element("storehouses").Add(renamedstorehouse.Elements("storehouse"));





            //CUSTOMFIELDS
            var renamedcustomfields =
            from l in IEParamList
            select new XElement("customfield",
                new XElement("idf", (string)l.Element("id")),
                new XElement("fieldname", (string)l.Element("nazwa")),
                new XElement("fieldtype", (string)l.Element("typ")),
                new XElement("fielddefault", (string)l.Element("wartosc_domyslna")),
                new XElement("suffix", (string)l.Element("jednostka")));


            
            lib.Elements("meta").First().Add(new XElement("customfields"));

            //pobierz IDMAX - max_idp
            //        max_idv
            //        max_ids
            //        max_idr
            //        max_idf
            //        max_idm = 1 bo jak pobierasz to bez idm

            var max_idp = 0;
            var max_idv = 0;
            var max_ids = 0;
            var max_idr = 0;
            var max_idf = 0;
            var max_idm = 0;

            if (rootlist.Elements("idp").Any())
            {
                max_idp = rootlist.Max(x => (int?)x.Element("idp") ?? 0);
            }
            if (rootlist.Elements("visit").Any())
            {
                max_idv = rootlist.Max(x => x.Element("visit") != null ? (int)x.Element("visit").Element("idv") : 0);
            }
            if (renamedstorehouse.Elements("ids").Any())
            {
                max_ids = renamedstorehouse.Max(x => (int?)x.Element("ids") ?? 0);
            }
            if (renamedstorehouse.Elements("rule").Any())
            {
                max_idr = renamedstorehouse.Max(x => x.Element("rule") != null ? (int)x.Element("rule").Element("idr") : 0);
            }
            if (renamedcustomfields.Elements("idf").Any())
            {
                max_idf = renamedcustomfields.Max(x => (int?)x.Element("idf") ?? 0); //customfields
            }

            lib.Elements("meta").First().AddFirst(new XElement("max_idp", max_idp + 1));
            lib.Elements("meta").First().AddFirst(new XElement("max_idv", max_idv + 1));
            lib.Elements("meta").First().AddFirst(new XElement("max_ids", max_ids + 1));
            lib.Elements("meta").First().AddFirst(new XElement("max_idr", max_idr + 1));
            lib.Elements("meta").First().AddFirst(new XElement("max_idf", max_idf + 1));
            lib.Elements("meta").First().AddFirst(new XElement("max_idm", max_idm + 1));

            lib.Add(rootlist);
            lib.Element("meta").Element("storehouses").Add(renamedstorehouse);
            lib.Element("meta").Element("customfields").Add(renamedcustomfields);

            Console.ReadLine();

            return lib;
        }
    }
}

    //TODO - Fix customfieds, fix autonumerate & holes
