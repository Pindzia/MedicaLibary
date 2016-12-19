using MedicaLibrary.Model;
using MedicalLibrary.View.Windows;
using MedicalLibrary.ViewModel.PagesViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml.Linq;

namespace MedicalLibrary.ViewModel.WindowsViewModel
{
    public class AddEditPatientViewModel : BaseViewModel
    {

        public AddEditPatientViewModel()
        {
            ListMagazines = XElementon.Instance.Storehouse.StorehouseNameList();
            SavePatient = new RelayCommand(pars=>Save((AddEditPatientWindow)pars));
            
        }

        public AddEditPatientViewModel(XElement EditPatient)
        {
            ListMagazines = XElementon.Instance.Storehouse.StorehouseNameList();
            SavePatient = new RelayCommand(pars => Save((AddEditPatientWindow)pars));
            LastName = EditPatient.Element("nazwisko").Value;
            FirstName = EditPatient.Element("imie").Value;
            Pesel = EditPatient.Element("pesel").Value;
        }

        public ICommand SavePatient { get; set; }

        private string _LastName = "";
        public string LastName
        {
            get
            {
                return _LastName;
            }
            set
            {
                _LastName = value;
                OnPropertyChanged("LastName");
            }
        }

        private string _FirstName = "";
        public string FirstName
        {
            get
            {
                return _FirstName;
            }
            set
            {
                _FirstName = value;
                OnPropertyChanged("FirstName");
            }
        }

        private string _Pesel = "";
        public string Pesel
        {
            get
            {
                return _Pesel;
            }
            set
            {
                _Pesel = value;
                OnPropertyChanged("Pesel");
            }
        }
        private XElement _Patient = null;
        public XElement Patient
        {
            get
            {
                return _Patient;
            }
            set
            {
                _Patient = value;
                OnPropertyChanged("Patient");
            }
        }

        private List<string> _ListMagazines = new List<string>();
        public List<string> ListMagazines
        {
            get
            {
                return _ListMagazines;
            }
            set
            {
                _ListMagazines = value;
                OnPropertyChanged("ListMagazines");
            }
        }

        private string _Envelope = null;
        public string Envelope
        {
            get
            {
                return _Envelope;
            }
            set
            {
                _Envelope = value;
                OnPropertyChanged("Envelope");
            }
        }

        private void Save(AddEditPatientWindow window)
        {

            if (FirstName.Length >= 4)
            {
                if (LastName.Length >= 4)
                {
                    if (System.Text.RegularExpressions.Regex.IsMatch(Pesel,"\\d{11}"))
                    {
                        string Id;
                        var imie = FirstName;
                        var nazwisko = LastName;
                        var pesel = Pesel;

                        //Autonumeracja po id - olewamy 'dziury'
                        var max = XElement.Load("lib.xml").Descendants("max_idp").First();
                        Id = max.Value;
                        max.Value = (Convert.ToInt16(Id) + 1).ToString();

                        //Tworzymy element pacjent który później wszczepimy w nasz dokument
                        Patient = new XElement(
                            new XElement("patient",
                            new XElement("idp", Id),
                            new XElement("imie", imie),
                            new XElement("nazwisko", nazwisko),
                            new XElement("pesel", pesel)));
                        window.DialogResult = true;
                        window.Close();
                    }
                    else
                    {
                        MessageBox.Show("Pesel jest za krótki lub nie posiada liczb");
                    }
                }else
                {
                    MessageBox.Show("Nazwisko jest za krótkie");
                }
            }
            else
            {
                MessageBox.Show("Imię jest za krótkie");
            }
        }
    }
}
