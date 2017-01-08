using System.Collections.Generic;
using System.Linq;
using MedicalLibary.DTO;

namespace MedicalLibrary.Model
{
    class SendModifications
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
            while (true)
            {
                // uzupełnij prawe strony swoimi
                ModyfikacjaToSend mod = new ModyfikacjaToSend()
                {
                    id_obiekt = 0,
                    id_wersji = ver,
                    obiekt = "node_type",
                    operaca = "operacja"
                };

                uri = "/modyfikacja/" + idLekarz.ToString() + "/nowa";
                await PushREST.UniversalPost(mod, uri);

                List<ModyfikacjaNowaDTO> modList = await PushREST.ModyfikacjeWszystkieGet(idLekarz);
                int modId = modList.Max(e => e.id);

                // foreach nowe/stare dane
                while (true)
                {
                    DaneModyfikacjiSendDTO daneMod = new DaneModyfikacjiSendDTO()
                    {
                        id_modyfikacja = modId,
                        nazwa_danej = "nazwa w twojej bazie?",
                        nowa_wartosc = "nowa",
                        stara_wartosc = "stara"
                    };
                }

            }
        }
    }
}
