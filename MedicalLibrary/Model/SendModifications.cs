﻿using System.Collections.Generic;
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

                //Wszystko poza modyfiakcjami
                Send(modyfikacja, typdanych, idLekarz);

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

                    if ((string)modyfikacja.Element("newdata") == "pesel")
                    {

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

        private async void Send(XElement modyfikacja, string typdanych, int idLekarz)
        {
            if (typdanych == "patient")
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
                        
                        
                        pacjent.pesel = null;
                        pacjent.pesel = "12345678903";
                        pacjent.pesel = ((string)zmiana);
                    }
                    else if (zmiana.Name != "idp")
                    {
                        przypisanie.id_pacjent = 1; //Todo WutFace
                        przypisanie.id_parametr = 1; //Todo WutFace
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

            else if (typdanych == "visit")
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



            else if (typdanych == "storehouse")
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


            else if (typdanych == "rule")
            {
                var zasada = new ZasadaToSendDTO();
                foreach (var zmiana in modyfikacja.Elements("newdata").Elements())
                {
                    if (zmiana.Name == "fieldname")
                    {
                        zasada.id_magazynu = Convert.ToInt32((string)modyfikacja.Parent.Element("idm")); //Wololocode
                    }
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


            else if (typdanych == "field")
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
