using MedicaLibrary.Model;
using MedicalLibrary.ViewModel.PagesViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;

namespace MedicalLibrary.ViewModel.WindowsViewModel
{
    public class AddEditMagazineViewModel : BaseViewModel
    {
        public AddEditMagazineViewModel()
        {
            ListAttributes = XElementon.Instance.Storehouse.Attributes()[0];
        }

        public AddEditMagazineViewModel(XElement magazine)
        {

        }

        private string _MagazineName = "";
        public string MagazineName
        {
            get
            {
                return _MagazineName;
            }
            set
            {
                _MagazineName = value;
                OnPropertyChanged("MagazineName");
            }
        }

        private List<string> _ListAttributes = new List<string>();
        public List<string> ListAttributes
        {
            get
            {
                return _ListAttributes;
            }
            set
            {
                _ListAttributes = value;
                OnPropertyChanged("ListAttributes");
            }
        }


        public ICommand SavePatient { get; set; }
    }
}
