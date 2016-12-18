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
            AddEditVistitWindow window = new AddEditVistitWindow();
            window.Show();
        }

        private void Edit()
        {
            throw new NotImplementedException();
        }

        private void Delete()
        {
            throw new NotImplementedException();
        }

    }
}
