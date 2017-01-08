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
            FullDate = DateTime.Parse(visit.Element("visit_addition_date").Value);
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
                OnPropertyChanged("Comment");
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
                if (Comment.Length >= 1)
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
                    Visit = new XElement(
                         new XElement("visit",
                         new XElement("idv", Id),
                         new XElement("visit_addition_date", FullDate),
                         new XElement("comment", Comment)));

                    window.DialogResult = true;
                    window.Close();
                }
                else
                {
                    MessageBox.Show("Komentarz jest za krótki");
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
