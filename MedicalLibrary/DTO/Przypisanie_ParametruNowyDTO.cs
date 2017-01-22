using System.ComponentModel.DataAnnotations;

namespace MedicalLibrary.DTO
{
    public class Przypisanie_ParametruNowyDTO : ObjectNewDTOs
    {
        public int id { get; set; }
        public int? id_pacjent { get; set; }

        public int? id_parametr { get; set; }

        [StringLength(50)]
        public string wartosc { get; set; }
    }

    public class Przypisanie_ParametruToSendDTO : ObjectNewDTOs
    {
        public int? id_pacjent { get; set; }

        public int? id_parametr { get; set; }

        [StringLength(50)]
        public string wartosc { get; set; }
    }
}