using System.Collections.Generic;
using System.Linq;
using MedicalLibary.DTO;

namespace MedicalLibrary.Model
{
    public class SendModifications
    {
        public async void SendAll(int idLekarz)
        {
            PushREST sender = new PushREST();

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
                    obiekt = (string)modyfikacja.Element("nodetype"),
                    operaca = (string)modyfikacja.Element("operation")
                };

                uri = "/modyfikacja/" + idLekarz.ToString() + "/nowa";
                await PushREST.UniversalPost(mod, uri);

                List<ModyfikacjaNowaDTO> modList = await PushREST.ModyfikacjeWszystkieGet(idLekarz);
                int modId = modList.Max(e => e.id);


                bool oldbigger;

                var a = modyfikacja.Elements("olddata").Count();
                var b = modyfikacja.Elements("newdata").Count();
                int x;
                if(a > b)
                {
                    x = a;
                    oldbigger = true;
                } else
                {
                    x = b;
                    oldbigger = false;
                }


                    // foreach nowe/stare dane
                    for (int i = 0; i <= x; i++)
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
                }
            }

            MedicaLibrary.Model.XElementon.Instance.Modification.Clean();
        }
    }
}
