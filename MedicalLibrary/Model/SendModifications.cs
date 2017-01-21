using System.Collections.Generic;
using System.Linq;
using MedicalLibary.DTO;
using System.Xml.Linq;
using System;
using System.Data;
using MedicaLibrary.Model;

namespace MedicalLibrary.Model
{
    public class SendModifications
    {
        public async void SendAll(int idLekarz, string pass)
        {
            //PushREST sender = new PushREST();
            PushREST.SetClient();

            if (await PushREST.LoggedIn(idLekarz, pass) == false) //fix
            {
                System.Windows.MessageBox.Show("Błędne hasło!");
                return;
            }

            // Nowa wersja
            WersjToSendDTO obj = new WersjToSendDTO();
            string uri = "/wersja/" + idLekarz.ToString() + "/nowa";
            await PushREST.UniversalPost(obj, uri,idLekarz,pass);


            // Max id wersji = najnowasza wersja
            List<WersjaNowaDTO> lista = await PushREST.WersjaGET(idLekarz,pass);
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
                await PushREST.UniversalPost(mod, uri, idLekarz, pass);

                List<ModyfikacjaNowaDTO> modList = await PushREST.ModyfikacjeWszystkieGet(idLekarz,pass);
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
                    SendA(modyfikacja, typdanych, idLekarz,pass);
                if (typoperacji == "E")
                    SendE(modyfikacja, typdanych, idLekarz,pass);
                if (typoperacji == "D")
                    SendD(modyfikacja, typdanych, idLekarz,pass);

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
                    await PushREST.UniversalPost(daneMod, uri, idLekarz, pass);
                }
            }
            MedicaLibrary.Model.XElementon.Instance.Modification.Clean();
            System.Windows.MessageBox.Show("Modyfikacje wysłane!");
        }

        private async void SendA(XElement modyfikacja, string typdanych, int idLekarz, string pass)
        {
            if (typdanych == "patient") //Pacjenci
            {
                var pacjent = new PacjentToSendDTO();
                
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
                        var przypisanie = new Przypisanie_ParametruToSendDTO();
                        przypisanie.id_pacjent = (int)modyfikacja.Element("newdata").Element("idp"); //Todo WutFace
                        przypisanie.id_parametr = (int)XElementon.Instance.Field.Fields().Where(x => (string)x.Element("fieldname") == zmiana.Name).First().Element("idf"); //Todo WutFace
                        przypisanie.wartosc = (string)zmiana;
                        listaprzypisan.Add(przypisanie);
                    }
                }
                
                string uri = "/pacjent/" + idLekarz.ToString() + "/nowy";
                await PushREST.UniversalPost(pacjent, uri, idLekarz, pass);

                while (listaprzypisan.Any())
                {
                    uri = "/przypisanie/" + idLekarz.ToString() + "/nowy";
                    await PushREST.UniversalPost(listaprzypisan[0], uri, idLekarz, pass);
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
                await PushREST.UniversalPost(wizyta, uri, idLekarz, pass);
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
                await PushREST.UniversalPost(magazyn, uri, idLekarz, pass);
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
                await PushREST.UniversalPost(zasada, uri, idLekarz, pass);
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
                await PushREST.UniversalPost(parametr, uri, idLekarz, pass);
            }
        }

        private async void SendE(XElement modyfikacja, string typdanych, int idLekarz, string pass)
        {
            if (typdanych == "patient") //Pacjenci
            {
                var pacjent = new PacjentNowyDTO();
                var listaprzypisan = new List<Przypisanie_ParametruNowyDTO>();
                var listaprzypisan2 = new List<Przypisanie_ParametruToSendDTO>();
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
                        var przypisanie = new Przypisanie_ParametruNowyDTO();
                        przypisanie.id_pacjent = (int)modyfikacja.Element("id");
                        przypisanie.id_parametr = (int)XElementon.Instance.Field.Fields().Where(x => (string)x.Element("fieldname") == zmiana.Name).First().Element("idf"); // dafuq error?
                        przypisanie.wartosc = (string)zmiana;
                        try
                        {
                            var temp = new Przypisanie_ParametruToSendDTO();
                            temp.id_pacjent = przypisanie.id_pacjent;
                            temp.id_parametr = przypisanie.id_parametr;
                            temp.wartosc = null;
                            przypisanie.id = await PushREST.PrzypisanieID(temp, idLekarz,pass);
                            listaprzypisan.Add(przypisanie);
                        }
                        catch (InvalidOperationException)
                        {
                            var przypisanie2 = new Przypisanie_ParametruToSendDTO();
                            przypisanie2.id_pacjent = przypisanie.id_pacjent;
                            przypisanie2.id_parametr = przypisanie.id_parametr;
                            przypisanie2.wartosc = przypisanie.wartosc;
                            listaprzypisan2.Add(przypisanie2);
                        }
                        //listaprzypisan.Add(przypisanie);
                    }
                }
                pacjent.id = (int)modyfikacja.Element("id");
                await PushREST.PacjentPut(pacjent, idLekarz,pass);
                
                while (listaprzypisan.Any())
                {
                    await PushREST.PrzypisanieParametruPut(listaprzypisan[0], idLekarz,pass);
                    listaprzypisan.RemoveAt(0);
                }
                while (listaprzypisan2.Any())
                {
                    string uri = "/przypisanie/" + idLekarz.ToString() + "/nowy";
                    await PushREST.UniversalPost(listaprzypisan2[0], uri, idLekarz, pass);
                    listaprzypisan2.RemoveAt(0);
                }
                

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
                await PushREST.WizytaPut(wizyta, idLekarz,pass);
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
                await PushREST.MagazynPut(magazyn, idLekarz,pass);
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
                await PushREST.ZasadaPut(zasada, idLekarz,pass);
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
                await PushREST.ParametrPut(parametr, idLekarz,pass);
            }
        }

        private async void SendD(XElement modyfikacja, string typdanych, int idLekarz,string pass)
        {
            if (typdanych == "patient") //Pacjenci
            {
                var idp = (int)modyfikacja.Element("id");

                //przypisania pacjenta do usuniecia
                List<Przypisanie_ParametruNowyDTO> list = new List<Przypisanie_ParametruNowyDTO>();
                list = await PushREST.PrzypisanieParametruWszystkieGET(idLekarz,pass);
                var list2 = list.Where(e=>e.id_pacjent == idp).Select(e => e.id);
                foreach (var a in list2)
                {
                    await PushREST.PrzypisanieParametruDelete(idLekarz, a,pass);
                }
                list2 = null;
                List<WizytaNowaDTO> wizyty = new List<WizytaNowaDTO>();
                wizyty = await PushREST.WizytaWszystkieGET(idLekarz,pass);
                list2 = wizyty.Where(e => e.id_pacjent == idp).Select(e => e.id);
                foreach (var a in list2)
                {
                    await PushREST.WizytaDelete(idLekarz, a,pass);
                }
                await PushREST.PacjentDelete(idLekarz, idp,pass);
            }

            else if (typdanych == "visit") //Wizyty
            {
                var idp = (int) modyfikacja.Element("id");
                await PushREST.WizytaDelete(idLekarz, idp,pass);
            }
            else if (typdanych == "storehouse") //Magazyny
            {
                var idp = (int) modyfikacja.Element("id");
                //zasady
                List<ZasadaNowaDTO> list = new List<ZasadaNowaDTO>();
                list = await PushREST.ZasadaWszystkieGET(idLekarz,pass);
                var list2 = list.Where(e => e.id_magazynu == idp).Select(e => e.id);
                foreach (var a in list2)
                {
                    await PushREST.ZasadaDelete(idLekarz, a,pass);
                }
                //pacjenci
                List<PacjentNowyDTO> pacjenci = new List<PacjentNowyDTO>();
                pacjenci = await PushREST.PacjentWszyscyGET(idLekarz,pass);
                foreach (var a in pacjenci)
                {
                    if (a.id_magazyn == idp)
                    {
                        a.id_magazyn = 1;
                        await PushREST.PacjentPut(a, idLekarz,pass);
                    }
                }
                await PushREST.MagazynDelete(idLekarz, idp,pass);
            }
            else if (typdanych == "rule") //Zasady
            {
                var idp = (int) modyfikacja.Element("id");
                await PushREST.ZasadaDelete(idLekarz, idp,pass);
            }
            else if (typdanych == "field") //Pola
            {
                var idp = (int) modyfikacja.Element("id");
                // usuwanie przypisan tego czegos
                List<Przypisanie_ParametruNowyDTO> list = new List<Przypisanie_ParametruNowyDTO>();
                list = await PushREST.PrzypisanieParametruWszystkieGET(idLekarz,pass);
                var list2 = list.Where(e => e.id_parametr == idp).Select(e => e.id);
                foreach (var a in list2)
                {
                    await PushREST.PrzypisanieParametruDelete(idLekarz, a,pass);
                }

                await PushREST.ParametrDelete(idLekarz, idp,pass);
            }
        }
    }
}
