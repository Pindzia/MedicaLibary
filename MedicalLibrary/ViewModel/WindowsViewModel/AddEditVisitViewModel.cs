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
        }

        public AddEditVisitViewModel(XElement visit)
        {
            FullDate = DateTime.Parse(visit.Element("visit_addition_date").Value);
            Comment = visit.Element("comment").Value;
            SaveVisit = new RelayCommand(pars => Save((AddEditVistitWindow)pars));
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

        private void Save(AddEditVistitWindow window)
        {
            if (FullDate != null)
            {
                if (Comment.Length >= 1)
                {
                    // data do wizyty do przypisania ew. zmienna do wytworzenia
                    Visit = new XElement(
                         new XElement("visit",
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


    }
}
