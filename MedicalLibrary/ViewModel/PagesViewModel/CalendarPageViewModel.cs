using MedicaLibrary.Model;
using OutlookCalendar.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalLibrary.ViewModel.PagesViewModel
{
    public class CalendarPageViewModel: BaseViewModel
    {
        public CalendarPageViewModel()
        {
            HighlightedDates = PrepareDate();

            //dopisać updatery
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

        private ObservableCollection<DateTime> PrepareDate()
        {
            ObservableCollection<DateTime> dates = new ObservableCollection<DateTime>();
            foreach(DateTime date in XElementon.Instance.Visit.UniqueDates())
            {
                DateTime newDate = new DateTime(date.Year, date.Month, date.Day);
                dates.Add(newDate);
            }
            return dates;
        }

    }
}
