namespace MedicalLibrary.DTO
{
    public class MagazynNowyDTO : ObjectNewDTOs
    {
        public int id { get; set; }
        public string nazwa { get; set; }
        public int? max_rozmiar { get; set; }
        public int? priorytet { get; set; }
    }

    public class MagazynToSendDTO : ObjectNewDTOs
    {
        public string nazwa { get; set; }
        public int? max_rozmiar { get; set; }
        public int? priorytet { get; set; }
    }
}