using System.ComponentModel.DataAnnotations;

namespace MedicalLibrary.DTO
{
    public class ParametrNowyDTO : ObjectNewDTOs
    {
        //public int? id_lekarz { get; set; }
        public int id { get; set; }
        [StringLength(16)]
        public string typ { get; set; }

        [StringLength(50)]
        public string nazwa { get; set; }

        [StringLength(50)]
        public string wartosc_domyslna { get; set; }
    }

    public class ParametrToSendDTO : ObjectNewDTOs
    {
        [StringLength(16)]
        public string typ { get; set; }

        [StringLength(50)]
        public string nazwa { get; set; }

        [StringLength(50)]
        public string wartosc_domyslna { get; set; }
    }
}