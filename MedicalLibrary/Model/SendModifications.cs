using System.Collections.Generic;
using System.Linq;
using MedicalLibary.DTO;
using System.Xml.Linq;
using System;
using MedicaLibrary.Model;

namespace MedicalLibrary.Model
{
    public class SendModifications
    {
        public async void SendAll(int idLekarz)
        {
            //PushREST sender = new PushREST();
            PushREST.SetClient();

            // Nowa wersja
            WersjToSendDTO obj = new WersjToSendDTO();
            string uri = "/wersja/" + idLekarz.ToString() + "/nowa";
            await PushREST.UniversalPost(obj, uri);

            // Max id wersji = najnowasza wersja
            List<WersjaNowaDTO> lista = await PushREST.WersjaGET(idLekarz);
            int ver = lista.Max(e => e.id);

            // foreach modyfikacje :v
            foreach (var modyfikacja in MedicaLibrary.Model.XElementon.Instance.Modification.Modifications())
            {
                // uzupełnij prawe strony swoimi
                ModyfikacjaToSend mod = new ModyfikacjaToSend()
                {
                    id_obiekt = 0,
                    id_wersji = ver,

                    obiekt = (string)modyfikacja.Element("node_type"),
                    operaca = (string)modyfikacja.Element("operation")
                };

                uri = "/modyfikacja/" + idLekarz.ToString() + "/nowa";
                await PushREST.UniversalPost(mod, uri);

                List<ModyfikacjaNowaDTO> modList = await PushREST.ModyfikacjeWszystkieGet(idLekarz);
                int modId = modList.Max(e => e.id);


                bool oldbigger;
                var a = modyfikacja.Elements("olddata").Elements().Count();
                var b = modyfikacja.Elements("newdata").Elements().Count();
                int x;
                if (a > b)
                {
                    x = a;
                    oldbigger = true;
                }
                else
                {
                    x = b;
                    oldbigger = false;
                }

                var typdanych = (string)modyfikacja.Element("node_type");
                var typoperacji = (string)modyfikacja.Element("operation");

                //Wszystko poza modyfiakcjami
                if (typoperacji == "A")
                    SendA(modyfikacja, typdanych, idLekarz);
                if (typoperacji == "E")
                    SendE(modyfikacja, typdanych, idLekarz);
                if (typoperacji == "D")
                    SendD(modyfikacja, typdanych, idLekarz);

                //Wszystkie Modyfikacje
                for (int i = 0; i < x; i++)
                {
                    string nazwa;
                    if (oldbigger)
                    {
                        nazwa = (string)modyfikacja.Element("olddata").Elements().ElementAt(i).Name.ToString();
                    }
                    else
                    {
                        nazwa = (string)modyfikacja.Element("newdata").Elements().ElementAt(i).Name.ToString();
                    }

                    DaneModyfikacjiSendDTO daneMod = new DaneModyfikacjiSendDTO()
                    {
                        id_modyfikacja = modId,
                        nazwa_danej = nazwa,
                        stara_wartosc = (string)modyfikacja.Element("olddata").Elements().ElementAtOrDefault(i),
                        nowa_wartosc = (string)modyfikacja.Element("newdata").Elements().ElementAtOrDefault(i)
                    };
                    uri = "/danemodyfikacji/" + idLekarz.ToString() + "/nowy";
                    await PushREST.UniversalPost(daneMod, uri);
                }
            }
            MedicaLibrary.Model.XElementon.Instance.Modification.Clean();
        }

        private async void SendA(XElement modyfikacja, string typdanych, int idLekarz)
        {
            if (typdanych == "patient") //Pacjenci
            {
                var pacjent = new PacjentToSendDTO();
                var przypisanie = new Przypisanie_ParametruToSendDTO();
                var listaprzypisan = new List<Przypisanie_ParametruToSendDTO>();
                foreach (var zmiana in modyfikacja.Elements("newdata").Elements())
                {
                    if (zmiana.Element("active") != null) //NotImplemented
                    {
                        //pacjent.aktywny = Convert.ToBoolean((string)zmiana.Element("active"));
                    }
                    else if (zmiana.Name == "storehouse")
                    {
                        var a = (string)XElementon.Instance.Storehouse.WithName((string)zmiana).First().Element("ids");
                        pacjent.id_magazyn = Convert.ToInt32(a);
                    }
                    else if (zmiana.Element("active") != null) //NotImplemented
                    {
                        //pacjent.ilosc_dodatkowych_parametrow = null;
                    }
                    else if (zmiana.Name == "imie")
                    {
                        pacjent.imie = (string)zmiana;
                    }
                    else if (zmiana.Name == "nazwisko")
                    {
                        pacjent.nazwisko = (string)zmiana;
                    }
                    else if (zmiana.Name == "envelope")
                    {
                        pacjent.numer_koperty = Convert.ToInt32((string)zmiana);
                    }
                    else if (zmiana.Name == "pesel")
                    {
                        pacjent.pesel = ((string)zmiana);
                    }
                    else if (zmiana.Name != "idp")
                    {
                        przypisanie.id_pacjent = (int)modyfikacja.Element("newdata").Element("idp"); //Todo WutFace
                        przypisanie.id_parametr = (int)XElementon.Instance.Field.Fields().Where(x => (string)x.Element("fieldname") == zmiana.Name).First().Element("idf"); //Todo WutFace
                        przypisanie.wartosc = (string)zmiana;
                        listaprzypisan.Add(przypisanie);
                    }
                }
                
                string uri = "/pacjent/" + idLekarz.ToString() + "/nowy";
                await PushREST.UniversalPost(pacjent, uri);

                while (listaprzypisan.Any())
                {
                    uri = "/przypisanie/" + idLekarz.ToString() + "/nowy";
                    await PushREST.UniversalPost(listaprzypisan[0], uri);
                    listaprzypisan.RemoveAt(0);
                }


            }

            else if (typdanych == "visit") //Wizyty
            {
                var wizyta = new WizytaToSendDTO();
                foreach (var zmiana in modyfikacja.Elements("newdata").Elements())
                {
                    if (zmiana.Name.LocalName == "visit_addition_date")
                    {
                        wizyta.data_wizyty = (Convert.ToDateTime((string)zmiana));
                    }
                    if (zmiana.Name.LocalName == "idp")
                    {
                        wizyta.id_pacjent = Convert.ToInt32((string)zmiana); //bo modyfikacja od wizyty zawiera .Element("idp")
                    }
                    if (zmiana.Name.LocalName == "comment")
                    {
                        wizyta.komentarz = (string)zmiana;
                    }
                }

                string uri = "/wizyta/" + idLekarz.ToString() + "/nowa";
                await PushREST.UniversalPost(wizyta, uri);
            }
            else if (typdanych == "storehouse") //Magazyny
            {
                var magazyn = new MagazynToSendDTO();
                foreach (var zmiana in modyfikacja.Elements("newdata").Elements())
                {
                    if (zmiana.Name == "size")
                    {
                        magazyn.max_rozmiar = Convert.ToInt32((string)zmiana);
                    }
                    if (zmiana.Name == "name")
                    {
                        magazyn.nazwa = (string)zmiana;
                    }
                    if (zmiana.Name == "priority")
                    {
                        magazyn.priorytet = Convert.ToInt32((string)zmiana);
                    }
                }
                string uri = "/magazyn/" + idLekarz.ToString() + "/nowy";
                await PushREST.UniversalPost(magazyn, uri);
            }
            else if (typdanych == "rule") //Zasady
            {
                var zasada = new ZasadaToSendDTO();
                zasada.id_magazynu = Convert.ToInt32((string)modyfikacja.Element("ids")); //Wololocode
                foreach (var zmiana in modyfikacja.Elements("newdata").Elements())
                {
                    if (zmiana.Name == "attribute")
                    {
                        zasada.nazwa_atrybutu = (string)zmiana;
                    }
                    if (zmiana.Name == "operation")
                    {
                        zasada.operacja_porownania = (string)zmiana;
                    }
                    if (zmiana.Name == "NotImplemented") //NotImplemented
                    {
                        //parametr.spelnialnosc_operacji = (string)zmiana; 
                    }
                    if (zmiana.Name == "value")
                    {
                        zasada.wartosc_porownania = (string)zmiana;
                    }
                }
                string uri = "/zasada/" + idLekarz.ToString() + "/nowy";
                await PushREST.UniversalPost(zasada, uri);
            }
            else if (typdanych == "field") //Pola
            {
                var parametr = new ParametrToSendDTO();
                foreach (var zmiana in modyfikacja.Elements("newdata").Elements())
                {
                    if (zmiana.Name == "fieldname")
                    {
                        parametr.nazwa = (string)zmiana;
                    }
                    if (zmiana.Name == "fieldtype")
                    {
                        parametr.typ = (string)zmiana;
                    }
                    if (zmiana.Name == "fielddefault")
                    {
                        parametr.wartosc_domyslna = (string)zmiana;
                    }
                }
                string uri = "/parametr/" + idLekarz.ToString() + "/nowy";
                await PushREST.UniversalPost(parametr, uri);
            }
        }

        private async void SendE(XElement modyfikacja, string typdanych, int idLekarz)
        {
            if (typdanych == "patient") //Pacjenci
            {
                var pacjent = new PacjentNowyDTO();
                var przypisanie = new Przypisanie_ParametruNowyDTO();
                var listaprzypisan = new List<Przypisanie_ParametruNowyDTO>();
                foreach (var zmiana in modyfikacja.Elements("newdata").Elements())
                {
                    if (zmiana.Element("active") != null) //NotImplemented
                    {
                        //pacjent.aktywny = Convert.ToBoolean((string)zmiana.Element("active"));
                    }
                    else if (zmiana.Name == "storehouse")
                    {
                        var a = (string)XElementon.Instance.Storehouse.WithName((string)zmiana).First().Element("ids");
                        pacjent.id_magazyn = Convert.ToInt32(a);
                    }
                    else if (zmiana.Element("active") != null) //NotImplemented
                    {
                        //pacjent.ilosc_dodatkowych_parametrow = null;
                    }
                    else if (zmiana.Name == "imie")
                    {
                        pacjent.imie = (string)zmiana;
                    }
                    else if (zmiana.Name == "nazwisko")
                    {
                        pacjent.nazwisko = (string)zmiana;
                    }
                    else if (zmiana.Name == "envelope")
                    {
                        pacjent.numer_koperty = Convert.ToInt32((string)zmiana);
                    }
                    else if (zmiana.Name == "pesel")
                    {
                        pacjent.pesel = ((string)zmiana);
                    }
                    else if (zmiana.Name != "idp")
                    {
                        przypisanie.id_pacjent = (int)modyfikacja.Element("newdata").Element("idp");  // nullException
                        przypisanie.id_parametr = (int)XElementon.Instance.Field.Fields().Where(x => (string)x.Element("fieldname") == zmiana.Name).First().Element("idf");
                        przypisanie.wartosc = (string)zmiana;
                        //listaprzypisan.Add(przypisanie);
                    }
                }
                pacjent.id = (int)modyfikacja.Element("id");
                await PushREST.PacjentPut(pacjent, idLekarz);
                /*
                while (listaprzypisan.Any())
                {
                    //string uri = "/przypisanie/" + idLekarz.ToString() + "/nowy";
                    //await PushREST.UniversalPost(listaprzypisan[0], uri);
                    //listaprzypisan[0].id = ???;
                    //await PushREST.PrzypisanieParametruPut(listaprzypisan[0],uri)
                    listaprzypisan.RemoveAt(0);
                }
                */

            }

            else if (typdanych == "visit") //Wizyty
            {
                var wizyta = new WizytaNowaDTO();
                foreach (var zmiana in modyfikacja.Elements("newdata").Elements())
                {
                    if (zmiana.Name.LocalName == "visit_addition_date")
                    {
                        wizyta.data_wizyty = (Convert.ToDateTime((string)zmiana));
                    }
                    if (zmiana.Name.LocalName == "idp")
                    {
                        wizyta.id_pacjent = Convert.ToInt32((string)zmiana); //bo modyfikacja od wizyty zawiera .Element("idp")
                    }
                    if (zmiana.Name.LocalName == "comment")
                    {
                        wizyta.komentarz = (string)zmiana;
                    }
                }
                wizyta.id = (int) modyfikacja.Element("id");
                await PushREST.WizytaPut(wizyta, idLekarz);
            }
            else if (typdanych == "storehouse") //Magazyny
            {
                var magazyn = new MagazynNowyDTO();
                foreach (var zmiana in modyfikacja.Elements("newdata").Elements())
                {
                    if (zmiana.Name == "size")
                    {
                        magazyn.max_rozmiar = Convert.ToInt32((string)zmiana);
                    }
                    if (zmiana.Name == "name")
                    {
                        magazyn.nazwa = (string)zmiana;
                    }
                    if (zmiana.Name == "priority")
                    {
                        magazyn.priorytet = Convert.ToInt32((string)zmiana);
                    }
                }
                magazyn.id = (int)modyfikacja.Element("id");
                await PushREST.MagazynPut(magazyn, idLekarz);
            }
            else if (typdanych == "rule") //Zasady
            {
                var zasada = new ZasadaNowaDTO();
                zasada.id_magazynu = Convert.ToInt32((string)modyfikacja.Element("ids")); //Wololocode
                foreach (var zmiana in modyfikacja.Elements("newdata").Elements())
                {
                    if (zmiana.Name == "attribute")
                    {
                        zasada.nazwa_atrybutu = (string)zmiana;
                    }
                    if (zmiana.Name == "operation")
                    {
                        zasada.operacja_porownania = (string)zmiana;
                    }
                    if (zmiana.Name == "NotImplemented") //NotImplemented
                    {
                        //parametr.spelnialnosc_operacji = (string)zmiana; 
                    }
                    if (zmiana.Name == "value")
                    {
                        zasada.wartosc_porownania = (string)zmiana;
                    }
                }
                zasada.id = (int) modyfikacja.Element("id");
                await PushREST.ZasadaPut(zasada, idLekarz);
            }
            else if (typdanych == "field") //Pola
            {
                var parametr = new ParametrNowyDTO();
                foreach (var zmiana in modyfikacja.Elements("newdata").Elements())
                {
                    if (zmiana.Name == "fieldname")
                    {
                        parametr.nazwa = (string)zmiana;
                    }
                    if (zmiana.Name == "fieldtype")
                    {
                        parametr.typ = (string)zmiana;
                    }
                    if (zmiana.Name == "fielddefault")
                    {
                        parametr.wartosc_domyslna = (string)zmiana;
                    }
                }
                parametr.id = (int) modyfikacja.Element("id");
                await PushREST.ParametrPut(parametr, idLekarz);
            }
        }

        private async void SendD(XElement modyfikacja, string typdanych, int idLekarz)
        {
            if (typdanych == "patient") //Pacjenci
            {
                var pacjent = new PacjentToSendDTO();
                var przypisanie = new Przypisanie_ParametruToSendDTO();
                var listaprzypisan = new List<Przypisanie_ParametruToSendDTO>();
                foreach (var zmiana in modyfikacja.Elements("newdata").Elements())
                {
                    if (zmiana.Element("active") != null) //NotImplemented
                    {
                        //pacjent.aktywny = Convert.ToBoolean((string)zmiana.Element("active"));
                    }
                    else if (zmiana.Name == "storehouse")
                    {
                        var a = (string)XElementon.Instance.Storehouse.WithName((string)zmiana).First().Element("ids");
                        pacjent.id_magazyn = Convert.ToInt32(a);
                    }
                    else if (zmiana.Element("active") != null) //NotImplemented
                    {
                        //pacjent.ilosc_dodatkowych_parametrow = null;
                    }
                    else if (zmiana.Name == "imie")
                    {
                        pacjent.imie = (string)zmiana;
                    }
                    else if (zmiana.Name == "nazwisko")
                    {
                        pacjent.nazwisko = (string)zmiana;
                    }
                    else if (zmiana.Name == "envelope")
                    {
                        pacjent.numer_koperty = Convert.ToInt32((string)zmiana);
                    }
                    else if (zmiana.Name == "pesel")
                    {
                        pacjent.pesel = ((string)zmiana);
                    }
                    else if (zmiana.Name != "idp")
                    {
                        przypisanie.id_pacjent = (int)modyfikacja.Element("newdata").Element("idp"); //Todo WutFace
                        przypisanie.id_parametr = (int)XElementon.Instance.Field.Fields().Where(x => (string)x.Element("fieldname") == zmiana.Name).First().Element("idf"); //Todo WutFace
                        przypisanie.wartosc = (string)zmiana;
                        listaprzypisan.Add(przypisanie);
                    }
                }

                string uri = "/pacjent/" + idLekarz.ToString() + "/nowy";
                await PushREST.UniversalPost(pacjent, uri);

                while (listaprzypisan.Any())
                {
                    uri = "/przypisanie/" + idLekarz.ToString() + "/nowy";
                    await PushREST.UniversalPost(listaprzypisan[0], uri);
                    listaprzypisan.RemoveAt(0);
                }


            }

            else if (typdanych == "visit") //Wizyty
            {
                var wizyta = new WizytaToSendDTO();
                foreach (var zmiana in modyfikacja.Elements("newdata").Elements())
                {
                    if (zmiana.Name.LocalName == "visit_addition_date")
                    {
                        wizyta.data_wizyty = (Convert.ToDateTime((string)zmiana));
                    }
                    if (zmiana.Name.LocalName == "idp")
                    {
                        wizyta.id_pacjent = Convert.ToInt32((string)zmiana); //bo modyfikacja od wizyty zawiera .Element("idp")
                    }
                    if (zmiana.Name.LocalName == "comment")
                    {
                        wizyta.komentarz = (string)zmiana;
                    }
                }

                string uri = "/wizyta/" + idLekarz.ToString() + "/nowa";
                await PushREST.UniversalPost(wizyta, uri);
            }
            else if (typdanych == "storehouse") //Magazyny
            {
                var magazyn = new MagazynToSendDTO();
                foreach (var zmiana in modyfikacja.Elements("newdata").Elements())
                {
                    if (zmiana.Name == "size")
                    {
                        magazyn.max_rozmiar = Convert.ToInt32((string)zmiana);
                    }
                    if (zmiana.Name == "name")
                    {
                        magazyn.nazwa = (string)zmiana;
                    }
                    if (zmiana.Name == "priority")
                    {
                        magazyn.priorytet = Convert.ToInt32((string)zmiana);
                    }
                }
                string uri = "/magazyn/" + idLekarz.ToString() + "/nowy";
                await PushREST.UniversalPost(magazyn, uri);
            }
            else if (typdanych == "rule") //Zasady
            {
                var zasada = new ZasadaToSendDTO();
                zasada.id_magazynu = Convert.ToInt32((string)modyfikacja.Element("ids")); //Wololocode
                foreach (var zmiana in modyfikacja.Elements("newdata").Elements())
                {
                    if (zmiana.Name == "attribute")
                    {
                        zasada.nazwa_atrybutu = (string)zmiana;
                    }
                    if (zmiana.Name == "operation")
                    {
                        zasada.operacja_porownania = (string)zmiana;
                    }
                    if (zmiana.Name == "NotImplemented") //NotImplemented
                    {
                        //parametr.spelnialnosc_operacji = (string)zmiana; 
                    }
                    if (zmiana.Name == "value")
                    {
                        zasada.wartosc_porownania = (string)zmiana;
                    }
                }
                string uri = "/zasada/" + idLekarz.ToString() + "/nowy";
                await PushREST.UniversalPost(zasada, uri);
            }
            else if (typdanych == "field") //Pola
            {
                var parametr = new ParametrToSendDTO();
                foreach (var zmiana in modyfikacja.Elements("newdata").Elements())
                {
                    if (zmiana.Name == "fieldname")
                    {
                        parametr.nazwa = (string)zmiana;
                    }
                    if (zmiana.Name == "fieldtype")
                    {
                        parametr.typ = (string)zmiana;
                    }
                    if (zmiana.Name == "fielddefault")
                    {
                        parametr.wartosc_domyslna = (string)zmiana;
                    }
                }
                string uri = "/parametr/" + idLekarz.ToString() + "/nowy";
                await PushREST.UniversalPost(parametr, uri);
            }
        }
    }
}
