using MedicaLibrary.Model;
using MedicalLibrary.View.Windows;
using MedicalLibrary.ViewModel.PagesViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml.Linq;

namespace MedicalLibrary.ViewModel.WindowsViewModel
{
    public class AddEditMagazineViewModel : BaseViewModel
    {
        public AddEditMagazineViewModel()
        {
            ListAttributes = XElementon.Instance.Storehouse.Attributes();
            SelectedAttribute = ListAttributes.FirstOrDefault();
            SaveMagazine = new RelayCommand(pars => Save((AddEditMagazineWindow)pars));
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
                ListOperation = XElementon.Instance.Storehouse.Operations(_SelectedAttribute);
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

        private string _VarOfRule = "";
        public string VarOfRule
        {
            get
            {
                return _VarOfRule;
            }
            set
            {
                _VarOfRule = value;
                OnPropertyChanged("VarOfRule");
            }
        }

        private string _MagazineSize = "";
        public string MagazineSize
        {
            get
            {
                return _MagazineSize;
            }
            set
            {
                _MagazineSize = value;
                OnPropertyChanged("MagazineSize");
            }
        }


        public ICommand SaveMagazine { get; set; }

        private void Save(AddEditMagazineWindow window)
        {
            if (MagazineName.Length >= 4)
            {
                if (System.Text.RegularExpressions.Regex.IsMatch(MagazineSize, "\\d{1,}"))
                {
                    if (SelectedAttribute != "" && SelectedOperation != "" && VarOfRule != "")
                    {
                        //New magazine as Xelement implementation
                        window.DialogResult = true;
                        window.Close();
                    }
                    else
                    {
                        MessageBox.Show("Wartość w polu Zasada jest niewybrana");
                    }
                }
                else
                {
                    MessageBox.Show("Podaj wartość liczbową rozmiaru Magazynu");
                }
            }
            else
            {
                MessageBox.Show("Nazwa jest za krótka");
            }
        }
    }
}
