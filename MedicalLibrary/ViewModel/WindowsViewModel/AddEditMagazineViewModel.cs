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
            SelectedAttribute = ListAttributes.FirstOrDefault();
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

        private string _SelectedAttribute = "";
        public string SelectedAttribute
        {
            get
            {
                return _SelectedAttribute;
            }
            set
            {
                _SelectedAttribute = value;
                // place to update lis oper
                OnPropertyChanged("SelectedAttribute");
            }
        }



        private List<string> _ListOperation = new List<string>();
        public List<string> ListOperation
        {
            get
            {
                return _ListOperation;
            }
            set
            {
                _ListOperation = value;
                OnPropertyChanged("ListOperation");
            }
        }

        private string _SelectedOperation = "";
        public string SelectedOperation
        {
            get
            {
                return _SelectedOperation;
            }
            set
            {
                _SelectedOperation = value;
                OnPropertyChanged("SelectedOperation");
            }
        }

        public ICommand SavePatient { get; set; }
    }
}
