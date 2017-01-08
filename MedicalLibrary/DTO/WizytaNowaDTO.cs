using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalLibary.DTO
{
    public class WizytaNowaDTO : ObjectNewDTOs
    {
        public int id { get; set; }
        public int? id_pacjent { get; set; }

        [Column(TypeName = "date")]
        public DateTime? data_wizyty { get; set; }

        [StringLength(255)]
        public string komentarz { get; set; }
    }

    public class WizytaToSend : ObjectNewDTOs
    {
        public int? id_pacjent { get; set; }

        [Column(TypeName = "date")]
        public DateTime? data_wizyty { get; set; }

        [StringLength(255)]
        public string komentarz { get; set; }
    }
}