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
            LoadedCommand = new RelayCommand(pars => Load());
            UpdateData();
            
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
                OnPropertyChanged("_PatientList");
            }
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
                if (SelectedQuery != null) { Search(); }
                OnPropertyChanged("FindQuery");
            }
        }

        private List<string> _QueryOptionList = new List<string>();
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
                OnPropertyChanged("SelectedQuery");
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

        private void UpdateData()
        {
            PatientList = ObserverCollectionConverter.Instance.Observe(XElementon.Instance.Patient.Patients());
            DeployData(PatientList);

        }


        private void DeployData( ObservableCollection<XElement> ListToShow)
        {
            DataToBind = ListToShow;
        }

        public ICommand AddPatient { get; set; }
        public ICommand EditPatient { get; set; }
        public ICommand DeletePatient { get; set; }
        public ICommand LoadedCommand { get; set; }

        private Tuple<string, string>[] TupleList ()
        {
            Tuple<string, string> a = new Tuple<string, string>("imie", (string)NewPatient.Element("imie"));
            Tuple<string, string> b = new Tuple<string, string>("nazwisko", (string)NewPatient.Element("nazwisko"));
            Tuple<string, string> c = new Tuple<string, string>("pesel", (string)NewPatient.Element("pesel"));
            Tuple<string, string>[] tup = { a, b, c };
            return tup;
        }

        private void Add()
        {
            AddEditPatientViewModel viewModel = new AddEditPatientViewModel();
            AddEditPatientWindow window = new AddEditPatientWindow(ref viewModel);
            Nullable<bool> result = window.ShowDialog();
            if (result == true)
            {
                NewPatient = viewModel.Patient;
                XElementon.Instance.Patient.Add(TupleList(),true,(string)NewPatient.Element("storehouse"));
                UpdateData();
            }
        }

        private void Edit()
        {
            if(SelectedItem != null)
            {
                AddEditPatientViewModel viewModel = new AddEditPatientViewModel(SelectedItem);
                AddEditPatientWindow window = new AddEditPatientWindow(ref viewModel);
                Nullable<bool> result = window.ShowDialog();
                if (result == true)
                {
                    if (viewModel.Patient != NewPatient)
                    {
                        NewPatient = viewModel.Patient;
                        XElementon.Instance.Patient.Change((int)SelectedItem.Element("idp"), TupleList());
                        UpdateData();
                    }
                }
            }
            else
            {
                MessageBox.Show("Wybierz Pacjenta by edytować");
            }
        }

        private void Delete()
        {
            if (SelectedItem != null)
            {
                if (MessageBox.Show("Czy chcesz wykasować Pacjenta : " + SelectedItem.Element("imie").Value + " " + SelectedItem.Element("nazwisko").Value, "Potwierdzenie", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    XElementon.Instance.Patient.Delete((int)SelectedItem.Element("idp"));
                    UpdateData();
                }
            }
            else
            {
                MessageBox.Show("Wybierz Pacjenta by usunąć");
            }
        }

        private void Load()
        {
            UpdateData();
        }

        private void Search()
        {
            throw new NotImplementedException();
        }

    }
}
