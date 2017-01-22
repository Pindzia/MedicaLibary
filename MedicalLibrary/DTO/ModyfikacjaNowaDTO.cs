using System.ComponentModel.DataAnnotations;

namespace MedicalLibrary.DTO
{
    public class ModyfikacjaNowaDTO : ObjectNewDTOs
    {
        //public int? id_lekarz { get; set; }
        public int id { get; set; }

        public int? id_wersji { get; set; }

        [StringLength(50)]
        public string obiekt { get; set; }

        public int? id_obiekt { get; set; }

        [StringLength(50)]
        public string operaca { get; set; }
    }

    public class ModyfikacjaToSend : ObjectNewDTOs
    {
        public int? id_wersji { get; set; }

        [StringLength(50)]
        public string obiekt { get; set; }

        public int? id_obiekt { get; set; }

        [StringLength(50)]
        public string operaca { get; set; }
    }
}