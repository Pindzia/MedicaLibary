using System.ComponentModel.DataAnnotations;

namespace MedicalLibrary.DTO
{
    public class LekarzDTO : ObjectNewDTOs
    {
        public int Id { get; set; }
        [StringLength(50)]
        public string Nazwa { get; set; }
        [StringLength(255)]
        public string Haslo { get; set; }
    }

    public class LekarzNowyDTO : ObjectNewDTOs
    {
        [StringLength(50)]
        public string Nazwa { get; set; }
        [StringLength(255)]
        public string Haslo { get; set; }
    }
}