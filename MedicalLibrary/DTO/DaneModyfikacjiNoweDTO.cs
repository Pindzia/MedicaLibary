using System.ComponentModel.DataAnnotations;

namespace MedicalLibary.DTO
{
    public class DaneModyfikacjiNoweDTO : ObjectNewDTOs 
    {
        public int id { get; set; }

        public int? id_modyfikacja { get; set; }

        [StringLength(50)]
        public string nazwa_danej { get; set; }

        [StringLength(50)]
        public string stara_wartosc { get; set; }

        [StringLength(50)]
        public string nowa_wartosc { get; set; }
    }

    public class DaneModyfikacjiSendDTO : ObjectNewDTOs
    {
        public int? id_modyfikacja { get; set; }

        [StringLength(50)]
        public string nazwa_danej { get; set; }

        [StringLength(50)]
        public string stara_wartosc { get; set; }

        [StringLength(50)]
        public string nowa_wartosc { get; set; }
    }
}