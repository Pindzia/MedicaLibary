using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;
using MedicaLibrary.Model;
using System.Windows;
using System.Collections.ObjectModel;
using MedicalLibrary.View.Windows;
using MedicalLibrary.ViewModel.WindowsViewModel;

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

        public ICommand AddVisit { get; set; }
        public ICommand EditVisit { get; set; }
        public ICommand DeleteVisit { get; set; }

        private void UpdateData()
        {
            PatientList = ObserverCollectionConverter.Instance.Observe(XElementon.Instance.Patient.Patients());
            SelectedItem = PatientList.FirstOrDefault();
        }

        private void UpdateVisits()
        {
            VisitList = ObserverCollectionConverter.Instance.Observe(XElementon.Instance.Visit.WithIDP((int)SelectedItem.Element("idp")));
        }

        private void Add()
        {
            AddEditVisitViewModel viewModel = new AddEditVisitViewModel();
            AddEditVistitWindow window = new AddEditVistitWindow(ref viewModel);
            Nullable<bool> result = window.ShowDialog();
            if (result == true)
            {
                //NewVisit = viewModel.Patient; zobaczyc jak parsować

                UpdateVisits();
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
                   /* if (NewVisit != viewModel.)
                    {
                        //refactor Data in viemodel (jak zapisac i przeparsowac wizytę oknie wizyty i here)
                        UpdateVisits();
                    }*/

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

    }
}
