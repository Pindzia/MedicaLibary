using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;
using MedicalLibrary.Model;
using System.Windows;
using System.Collections.ObjectModel;
using MedicalLibrary.View.Windows;
using MedicalLibrary.ViewModel.WindowsViewModel;
using OutlookCalendar.Model;

namespace MedicalLibrary.ViewModel.PagesViewModel
{
    public class VisitPageViewModel : BaseViewModel
    {

        public VisitPageViewModel()
        {
            UpdateData();
            AddVisit = new RelayCommand(pars => Add());
            EditVisit = new RelayCommand(pars => Edit());
            DeleteVisit = new RelayCommand(pars => Delete());
            LoadedCommand = new RelayCommand(pars => Load());
        }

        private ObservableCollection<XElement> _PatientList = new ObservableCollection<XElement>();
        public ObservableCollection<XElement> PatientList
        {
            get
            {
                return _PatientList;
            }

            set
            {
                _PatientList = value;
                OnPropertyChanged("PatientList");
            }
        }

        private XElement _SelectedItem = null;
        public XElement SelectedItem
        {
            get
            {
                return _SelectedItem;
            }

            set
            {
                _SelectedItem = value;
                UpdateVisits();
                OnPropertyChanged("SelectedItem");
            }
        }

        private ObservableCollection<XElement> _VisitList = new ObservableCollection<XElement>();
        public ObservableCollection<XElement> VisitList
        {
            get
            {
                return _VisitList;
            }

            set
            {
                _VisitList = value;
                OnPropertyChanged("VisitList");
            }
        }

        private XElement _SelectedVisit = null;
        public XElement SelectedVisit
        {
            get
            {
                return _SelectedVisit;
            }

            set
            {
                _SelectedVisit = value;
                OnPropertyChanged("SelectedVisit");
            }
        }

        private XElement _NewVisit = null;
        public XElement NewVisit
        {
            get
            {
                return _NewVisit;
            }

            set
            {
                _NewVisit = value;
                OnPropertyChanged("NewVisit");
            }
        }

        private string _FindQuery = "";
        public string FindQuery
        {
            get
            {
                return _FindQuery;
            }

            set
            {
                _FindQuery = value;
                if (SelectedQuery != "" && SelectedQuery != null) { Search(); }
                OnPropertyChanged("FindQuery");
            }
        }

        private List<string> _QueryOptionList = new List<string>() { "imie" ,"nazwisko","pesel" };
        public List<string> QueryOptionList
        {
            get
            {
                return _QueryOptionList;
            }

            set
            {
                _QueryOptionList = value;
                OnPropertyChanged("QueryOptionList");
            }
        }

        private string _SelectedQuery = "";
        public string SelectedQuery
        {
            get
            {
                return _SelectedQuery;
            }

            set
            {
                _SelectedQuery = value;
                if (FindQuery != null && FindQuery != "") { SearchAsync(); }
                OnPropertyChanged("SelectedQuery");
            }
        }

        private Appointments _Collection = new Appointments();
        public Appointments Collection
        {
            get
            {
                return _Collection;
            }
            set
            {
                _Collection = value;
                OnPropertyChanged(nameof(Collection)); // zapamiętać !!
            }
        }

        private DateTime _MyTime = DateTime.Now;
        public DateTime MyTime
        {
            get
            {
                return _MyTime;
            }
            set
            {
                _MyTime = value;
                OnPropertyChanged(nameof(MyTime)); // zapamiętać !!
            }
        }

        private ObservableCollection<DateTime> _HighlightedDates = new ObservableCollection<DateTime>();
        public ObservableCollection<DateTime> HighlightedDates
        {
            get
            {
                return _HighlightedDates;
            }
            set
            {
                _HighlightedDates = value;
                OnPropertyChanged(nameof(HighlightedDates)); // zapamiętać !!
            }
        }

        public ICommand AddVisit { get; set; }
        public ICommand EditVisit { get; set; }
        public ICommand DeleteVisit { get; set; }
        public ICommand LoadedCommand { get; set; }

        private void UpdateData()
        {
            PatientList = ObserverCollectionConverter.Instance.Observe(XElementon.Instance.Patient.Patients());
            SelectedItem = PatientList.FirstOrDefault();
            HighlightedDates = PrepareHighlight();
            Collection = PrepareAppointments();
        }
        private void UpdateData(ObservableCollection<XElement> data)
        {
            PatientList = data;
        }

        private void UpdateVisits()
        {
            if(SelectedItem != null)
                VisitList = ObserverCollectionConverter.Instance.Observe(XElementon.Instance.Visit.WithIDP((int)SelectedItem.Element("idp")));
        }

        private Tuple<string, string>[] TupleList()
        {
            Tuple<string, string> a = new Tuple<string, string>("visit_addition_date", (string)NewVisit.Element("visit_addition_date"));
            Tuple<string, string> b = new Tuple<string, string>("comment", (string)NewVisit.Element("comment"));
            Tuple<string, string> c = new Tuple<string, string>("years_to_keep", (string)NewVisit.Element("years_to_keep"));
            Tuple<string, string> d = new Tuple<string, string>("visit_end_date", (string)NewVisit.Element("visit_end_date"));
            
            Tuple<string, string>[] tup = { a, b, c, d };
            return tup;
        }

        private void Add()
        {
            if (SelectedItem != null)
            {
                AddEditVisitViewModel viewModel = new AddEditVisitViewModel();
                AddEditVistitWindow window = new AddEditVistitWindow(ref viewModel);
                Nullable<bool> result = window.ShowDialog();
                if (result == true)
                {
                    NewVisit = viewModel.Visit; //zobaczyc jak parsować
                    XElementon.Instance.Visit.Add((int)SelectedItem.Element("idp"), TupleList());
                    UpdateVisits();// sprawdzić logikę
                    UpdateData();
                    MyTime = MyTime.AddMilliseconds(1);//trick to update Calendar highlights :)
                }
            }else
            {
                MessageBox.Show("Wybierz Pacjenta Aby dodać wizytę");
            }
        }

        private void Edit()
        {
            if(SelectedVisit != null)
            {
                AddEditVisitViewModel viewModel = new AddEditVisitViewModel(SelectedVisit);
                AddEditVistitWindow window = new AddEditVistitWindow(ref viewModel);
                Nullable<bool> result = window.ShowDialog();
                if (result == true)
                {
                    if (viewModel.Visit != NewVisit)
                    {
                        NewVisit = viewModel.Visit;
                        XElementon.Instance.Visit.Change((int)SelectedVisit.Element("idv"), TupleList());
                        UpdateData();
                    }
                }
            }
            else
            {
                MessageBox.Show("Wybierz Wizytę by edytować");
            }
        }

        private void Delete()
        {
            if (SelectedVisit != null)
            {
                if (MessageBox.Show("Czy chcesz wykasować daną Wizytę : " + SelectedVisit.Element("visit_addition_date").Value + " " + SelectedVisit.Element("comment").Value, "Potwierdzenie", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    XElementon.Instance.Visit.Delete((int)SelectedVisit.Element("idv"));
                    UpdateData();
                }
            }
            else
            {
                MessageBox.Show("Wybierz Wizytę by usunąć");
            }
        }

        private void Load()
        {
            UpdateData();
        }

        private async void SearchAsync()
        {
            System.Threading.Tasks.Task.Run(() => Search());
        }

        private void Search()
        {
            if (SelectedQuery != "")
                UpdateData(ObserverCollectionConverter.Instance.Observe(XElementon.Instance.Patient.Filtered(SelectedQuery, FindQuery)));
        }

        private ObservableCollection<DateTime> PrepareHighlight()
        {
            ObservableCollection<DateTime> dates = new ObservableCollection<DateTime>();
            foreach (DateTime date in XElementon.Instance.Visit.UniqueDates())
            {
                DateTime newDate = new DateTime(date.Year, date.Month, date.Day);
                dates.Add(newDate);
            }
            return dates;
        }

        private Appointments PrepareAppointments()
        {
            Appointments collection = new Appointments();

            foreach (XElement visit in XElementon.Instance.Visit.Visits())
            {
                Appointment appoint = new Appointment();
                XElement startTime = visit.Element("visit_addition_date");
                appoint.Subject = "Wizyta Pacjenta " + visit.Parent.Element("imie").Value + " " + visit.Parent.Element("nazwisko").Value + " " + visit.Parent.Element("pesel").Value;
                appoint.StartTime = Convert.ToDateTime((string)startTime);
                appoint.EndTime = Convert.ToDateTime(visit.Element("visit_end_date").Value);
                collection.Add(appoint);
            }
            return collection;
        }

    }
}
