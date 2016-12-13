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

        public AddEditMagazineViewModel(XElement ParsedMagazine)
        {
            ListAttributes = XElementon.Instance.Storehouse.Attributes();
            MagazineName = ParsedMagazine.Element("name").Value;
            SelectedAttribute = ParsedMagazine.Element("rule").Element("attribute").Value;
            SelectedOperation = ParsedMagazine.Element("rule").Element("operation").Value;
            VarOfRule = ParsedMagazine.Element("rule").Element("value").Value;
            MagazineSize = ParsedMagazine.Element("size").Value;
            SaveMagazine = new RelayCommand(pars => Save((AddEditMagazineWindow)pars));
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

        private XElement _Rule = null;
        public XElement Rule
        {
            get
            {
                return _Rule;
            }
            set
            {
                _Rule = value;
                OnPropertyChanged("Rule");
            }
        }

        private XElement _Magazine = null;
        public XElement Magazine
        {
            get
            {
                return _Magazine;
            }
            set
            {
                _Magazine = value;
                OnPropertyChanged("Magazine");
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
                        //dodawanie XElementa
                        //New magazine as Xelement implementation


                        //Tworzymy element Rule który później wszczepimy w nasz dokument
                        Rule = new XElement(
                            new XElement("rule",
                            new XElement("attribute", ListAttributes),
                            new XElement("operation", ListOperation),
                            new XElement("value", VarOfRule)));


                        //Tworzymy element Storehouse który później wszczepimy w nasz dokument
                        Magazine = new XElement(
                            new XElement("storehouse",
                            new XElement("name", MagazineName),
                            new XElement("size", MagazineSize),
                            new XElement("priority", -1))); //nocolision! napisać to smart!
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
