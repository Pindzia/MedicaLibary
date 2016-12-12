using System.ComponentModel;
using System.Windows.Input;
using System.Xml.Linq;
using MedicalLibrary.View.Windows;
using System;
using MedicalLibrary.ViewModel.WindowsViewModel;
using System.Collections.ObjectModel;
using System.Linq;
using System.Collections.Generic;
using System.Windows;
using MedicalLibrary.ViewModel.PagesViewModel;
using MedicaLibrary.Model;

namespace MedicalLibrary.ViewModel.WindowsViewModel
{
    public class PatientPageViewModel : BaseViewModel
    {

        public PatientPageViewModel()
        {
            AddPatient = new RelayCommand(pars => Add());
            EditPatient = new RelayCommand(pars => Edit());
            DeletePatient = new RelayCommand(pars => Delete());
            DataToBind = ObserverCollectionConverter.Instance.Observe(MedicaLibrary.Model.XElementon.Instance.Patient.GetAllPatients());
        }
        private ObservableCollection<XElement> _DataToBind = new ObservableCollection<XElement>();
        public ObservableCollection<XElement> DataToBind
        {
            get
            {
                return _DataToBind;
            }

            set
            {
                _DataToBind = value;
                OnPropertyChanged("DataToBind");
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
                SelectedItemIndex = DataToBind.IndexOf(SelectedItem);
                OnPropertyChanged("SelectedItem");
            }
        }

        private int _SelectedItemIndex = 0;
        public int SelectedItemIndex
        {
            get
            {
                return _SelectedItemIndex;
            }

            set
            {
                _SelectedItemIndex = value;
                OnPropertyChanged("SelectedItemIndex");
            }
        }

        private XElement _NewPatient = null;
        public XElement NewPatient
        {
            get
            {
                return _NewPatient;
            }

            set
            {
                _NewPatient = value;
                OnPropertyChanged("NewPatient");
            }
        }


        public ICommand AddPatient { get; set; }
        public ICommand EditPatient { get; set; }
        public ICommand DeletePatient { get; set; }

        private void Add()
        {
            AddEditPatientViewModel viewModel = new AddEditPatientViewModel();
            AddEditPatientWindow window = new AddEditPatientWindow(ref viewModel);
            Nullable<bool> result = window.ShowDialog();
            if (result == true)
            {
                NewPatient = viewModel.Patient;

                Tuple<string, string> a = new Tuple<string, string>("imie", NewPatient.Element("imie").Value);
                Tuple<string, string> b = new Tuple<string, string>("nazwisko", NewPatient.Element("nazwisko").Value);
                Tuple<string, string> c = new Tuple<string, string>("pesel", NewPatient.Element("pesel").Value);
                Tuple<string, string>[] tup = { a, b, c };

                XElementon.Instance.AddPatient(tup);

                DataToBind = ObserverCollectionConverter.Instance.Observe(XElementon.Instance.Patient.GetAllPatients());

                //DataToBind.Add(NewPatient);
            }
        }

        private void Edit()
        {
            AddEditPatientViewModel viewModel = new AddEditPatientViewModel(SelectedItem);
            AddEditPatientWindow window = new AddEditPatientWindow(ref viewModel);
            Nullable<bool> result = window.ShowDialog();
            if(result == true)
            {
                NewPatient = viewModel.Patient;
                DataToBind[SelectedItemIndex] = NewPatient;
            }
            //only downloading Data to do wait for implement
        }

        private void Delete()
        {
            if(MessageBox.Show("Czy chcesz wykasować Pacjenta :"+SelectedItem.Element("imie").Value +" "+SelectedItem.Element("nazwisko").Value,"Potwierdzenie",MessageBoxButton.YesNo,MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                DataToBind.RemoveAt(SelectedItemIndex);
            }
        }
    }
}
