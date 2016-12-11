using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalLibrary.Model.MappedModels
{
    public class Patient
    {
        public Patient(int vId ,int vId_Magazine, bool vActive,string vName,string vSurname, decimal vPesel, int vCardNum, int vNumOfUniqProp)
        {
            Id = vId;
            Id_Magazine = vId_Magazine;
            Active = vActive;
            Name = vName;
            Surname = vSurname;
            Pesel = vPesel;
            CardNum = vCardNum;
            NumOfUniqProp = vNumOfUniqProp;
        }
        public int Id { get; set; }
        public int Id_Magazine { get; set; }
        public bool Active { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public decimal Pesel { get; set; }
        public int CardNum { get; set; }
        public int NumOfUniqProp { get; set; }
    }
}
