using MedicalLibrary.View.Windows;
using MedicalLibrary.ViewModel.PagesViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml.Linq;

namespace MedicalLibrary.ViewModel.WindowsViewModel
{
    public class AddEditVisitViewModel: BaseViewModel
    {
        public AddEditVisitViewModel()
        {
            SaveVisit = new RelayCommand(pars => Save((AddEditVistitWindow)pars));
            CancelVisit = new RelayCommand(pars => Cancel((AddEditVistitWindow)pars));
        }

        public AddEditVisitViewModel(XElement visit)
        {
            Visit = visit; //Tworzymy wcześniej XElementa a'la SelectedItem w AddEditPatientViewModel?
            DateTime dataToParse = DateTime.Parse(visit.Element("visit_addition_date").Value);
            FullDate = new DateTime(dataToParse.Year, dataToParse.Month, dataToParse.Day, dataToParse.Hour, dataToParse.Minute, dataToParse.Second);
            DateTime dataToMinutes = DateTime.Parse(visit.Element("visit_time").Value);
            DateTime dataToCompare = new DateTime(dataToMinutes.Year, dataToMinutes.Month, dataToMinutes.Day, dataToMinutes.Hour, dataToMinutes.Minute, dataToMinutes.Second);
            TimeSpan interval = dataToCompare.Subtract(FullDate);
            Minutes = interval.Minutes.ToString();
            Years = visit.Element("years_to_keep").Value;
            Comment = visit.Element("comment").Value;
            SaveVisit = new RelayCommand(pars => Save((AddEditVistitWindow)pars));
            CancelVisit = new RelayCommand(pars => Cancel((AddEditVistitWindow)pars));
        }

        private DateTime _FullDate = DateTime.Now;
        public DateTime FullDate
        {
            get
            {
                return _FullDate;
            }
            set
            {
                _FullDate = value;
                OnPropertyChanged("FullDate");
            }
        }

        private string _Comment = "";
        public string Comment
        {
            get
            {
                return _Comment;
            }
            set
            {
                _Comment = value;
                Regex regex = new Regex("^[a-zA-Z0-9_ ]*$");
                IsGoodK = (!regex.IsMatch(_Comment)) ? true : false;
                OnPropertyChanged("Comment");
            }
        }

        private bool _IsGoodK = false;
        public bool IsGoodK
        {
            get
            {
                return _IsGoodK;
            }
            set
            {
                _IsGoodK = value;
                OnPropertyChanged("IsGoodK");
            }
        }

        private string _Years = "0";
        public string Years
        {
            get
            {
                return _Years;
            }
            set
            {
                _Years = value;
                Regex regex = new Regex("^[0-9]+$");
                IsGoodY = (!regex.IsMatch(_Years)) ? true : false;
                OnPropertyChanged("Years");
            }
        }

        private bool _IsGoodY = false;
        public bool IsGoodY
        {
            get
            {
                return _IsGoodY;
            }
            set
            {
                _IsGoodY = value;
                OnPropertyChanged("IsGoodY");
            }
        }

        private string _Minutes = "15";
        public string Minutes
        {
            get
            {
                return _Minutes;
            }
            set
            {
                _Minutes = value;
                Regex regex = new Regex("^[0-9]+$");
                IsGoodM = (!regex.IsMatch(_Minutes)) ? true : false;
                OnPropertyChanged("Minutes");
            }
        }

        private bool _IsGoodM = false;
        public bool IsGoodM
        {
            get
            {
                return _IsGoodM;
            }
            set
            {
                _IsGoodM = value;
                OnPropertyChanged("IsGoodM");
            }
        }

        private XElement _Visit = null;
        public XElement Visit
        {
            get
            {
                return _Visit;
            }
            set
            {
                _Visit = value;
                OnPropertyChanged("Visit");
            }
        }

        public ICommand SaveVisit { get; set; }
        public ICommand CancelVisit { get; set; }

        private void Save(AddEditVistitWindow window)
        {
            if (FullDate != null)
            {
                if (IsGoodK&&IsGoodM&&IsGoodY)
                {

                    string Id = ""; //TODO - zmiana na zapis obiektowy a';a AddEditPatientViewMode z ichnijszym IDP?

                    //Autonumeracja po id - olewamy 'dziury'
                    if (Visit == null)
                    {
                        var max = XElement.Load("lib.xml").Descendants("max_idp").First();
                        Id = max.Value;
                        max.Value = (Convert.ToInt16(Id) + 1).ToString();
                    } else
                    {
                        Id = (string)Visit.Element("idv");
                    }

                    // data do wizyty do przypisania ew. zmienna do wytworzenia
                    int minutes;
                    int.TryParse(Minutes,out minutes);
                    Visit = new XElement(
                         new XElement("visit",
                         new XElement("idv", Id),
                         new XElement("visit_addition_date", FullDate),
                         new XElement("years_to_keep", Years),
                         new XElement("comment", Comment), 
                         new XElement("visit_time", FullDate.AddMinutes(minutes));

                    window.DialogResult = true;
                    window.Close();
                }
                else
                {
                    MessageBox.Show("Conajmniej Jedno z pól jest źle wypełnione");
                }
            }
            else
            {
                MessageBox.Show("Data jest pusta");
            }
        }

        private void Cancel(AddEditVistitWindow window)
        {
            window.DialogResult = false;
            window.Close();
        }


    }
}
