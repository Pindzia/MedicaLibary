using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalLibrary.DTO
{
    public class PacjentNowyDTO : ObjectNewDTOs
    {
        public int id { get; set; }
        public int? id_magazyn { get; set; }
        public bool? aktywny { get; set; }
        [StringLength(50)]
        public string imie { get; set; }
        [StringLength(50)]
        public string nazwisko { get; set; }
        public string pesel { get; set; }
        public int? numer_koperty { get; set; }
        public int? ilosc_dodatkowych_parametrow { get; set; }
    }

    public class PacjentToSendDTO : ObjectNewDTOs
    {
        public int? id_magazyn { get; set; }
        public bool? aktywny { get; set; }
        [StringLength(50)]
        public string imie { get; set; }
        [StringLength(50)]
        public string nazwisko { get; set; }
        public string pesel { get; set; }
        public int? numer_koperty { get; set; }
        public int? ilosc_dodatkowych_parametrow { get; set; }
    }
}