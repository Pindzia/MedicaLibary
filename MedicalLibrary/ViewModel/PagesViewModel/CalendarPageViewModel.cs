using MedicaLibrary.Model;
using OutlookCalendar.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;

namespace MedicalLibrary.ViewModel.PagesViewModel
{
    public class CalendarPageViewModel: BaseViewModel
    {
        public CalendarPageViewModel()
        {
            LoadedCommand = new RelayCommand(pars => Load());
        }

        private void Load()
        {
            UpdateData();
        }

        private void UpdateData()
        {
            HighlightedDates = PrepareHighlight();
            Collection = PrepareAppointments();
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

        public ICommand LoadedCommand { get; set; }

        private ObservableCollection<DateTime> PrepareHighlight()
        {
            ObservableCollection<DateTime> dates = new ObservableCollection<DateTime>();
            foreach(DateTime date in XElementon.Instance.Visit.UniqueDates())
            {
                DateTime newDate = new DateTime(date.Year, date.Month, date.Day);
                dates.Add(newDate);
            }
            return dates;
        }

        private Appointments PrepareAppointments()
        {
            Appointments collection = new Appointments();
            Appointment appoint = new Appointment();
            foreach(XElement visit in XElementon.Instance.Visit.Visits())
            {
                XElement startTime = visit.Element("visit_addition_date");
                appoint.Subject = "Wizyta"+ visit.Element("comment").Value;
                appoint.StartTime = (DateTime)startTime;
                appoint.EndTime = appoint.StartTime.AddMinutes(16);
                collection.Add(appoint);
            }
            return collection;
        }

    }
}
