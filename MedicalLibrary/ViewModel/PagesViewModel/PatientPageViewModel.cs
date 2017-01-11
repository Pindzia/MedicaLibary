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
using System.Windows.Controls;
using System.Windows.Data;
using System.Threading.Tasks;

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
            LoadedCustomFields = new RelayCommand(pars => LoadCustomFields((DataGrid)pars));
            ClearSearch = new RelayCommand(pars => Clear());
            QueryOptionList = XElementon.Instance.Patient.PatientAttributeList();

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
                if (SelectedQuery != "" && SelectedQuery != null) { Search(); }
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
                if (FindQuery != null && FindQuery != "") { SearchAsync(); }
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

        private int _ColumnAdded = 0;
        public int ColumnAdded
        {
            get
            {
                return _ColumnAdded;
            }

            set
            {
                _ColumnAdded = value;
                OnPropertyChanged("ColumnAdded");
            }
        }

        public ICommand AddPatient { get; set; }
        public ICommand EditPatient { get; set; }
        public ICommand DeletePatient { get; set; }
        public ICommand LoadedCommand { get; set; }
        public ICommand LoadedCustomFields { get; set; }
        public ICommand ClearSearch { get; set; }

        private Tuple<string, string>[] TupleList ()
        {
            var list = new List<Tuple<string, string>>();
            Tuple<string, string> a = new Tuple<string, string>("imie", (string)NewPatient.Element("imie"));
            Tuple<string, string> b = new Tuple<string, string>("nazwisko", (string)NewPatient.Element("nazwisko"));
            Tuple<string, string> c = new Tuple<string, string>("pesel", (string)NewPatient.Element("pesel"));
            list.Add(a);
            list.Add(b);
            list.Add(c);

            var Fields = XElementon.Instance.Field.Fields();
            foreach (var element in NewPatient.Elements())
            {
                foreach (var field in Fields)
                if(element.Name.LocalName == (string)field.Element("fieldname"))
                {
                        list.Add(new Tuple<string, string>((string)element.Name.LocalName, (string)element));
                }
            }

            Tuple<string, string>[] tup = list.ToArray();
            return tup;
        }

        private async void UpdateDataAsync()
        {
            System.Threading.Tasks.Task.Run(() => UpdateData());
        }

        private void UpdateData()
        {
            PatientList = ObserverCollectionConverter.Instance.Observe(XElementon.Instance.Patient.Patients());
            DeployData(PatientList);
        }

        private void DeployData(ObservableCollection<XElement> ListToShow)
        {
            DataToBind = ListToShow;
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
                UpdateDataAsync();
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
                    var compare = new XElement(SelectedItem);
                    compare.Element("envelope").Remove();
                    if (viewModel.Patient.ToString() != compare.ToString()) //SelectedItem ma w sobie envelope a Patient nie //TODO czy to w ogóle potrzebne? Czy pozwolić na to i cancele?
                    {
                        NewPatient = viewModel.Patient;
                        XElementon.Instance.Patient.Change((int)SelectedItem.Element("idp"), TupleList());
                        UpdateDataAsync();
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
                    UpdateDataAsync();
                }
            }
            else
            {
                MessageBox.Show("Wybierz Pacjenta by usunąć");
            }
        }

        private void Load()
        {
            UpdateDataAsync();
        }

        private async void SearchAsync()
        {
            await System.Threading.Tasks.Task.Run(() => Search());
        }

        private void Search()
        {
            if (SelectedQuery != "")
                DeployData(ObserverCollectionConverter.Instance.Observe(XElementon.Instance.Patient.Filtered(SelectedQuery, FindQuery)));
        }

        private void Clear()
        {
            FindQuery = "";
            SelectedQuery = null;
        }

        private void LoadCustomFields(DataGrid grid)
        {
            if (ColumnAdded > 0)
            {
                int loop = ColumnAdded;
                for(int i=0;i<loop;i++)
                {
                    grid.Columns.RemoveAt((grid.Columns.Count) - 1);
                    ColumnAdded -= 1;
                }
            }
            IEnumerable<XElement> fields = XElementon.Instance.Field.Fields();
            foreach(XElement field in fields)
            {
                DataGridTextColumn fieldColumn = new DataGridTextColumn();
                fieldColumn.Header = field.Element("fieldname").Value;
                var binding = new Binding();
                binding.Path = new PropertyPath("Element.[" + field.Element("fieldname").Value + "].Value");
                binding.FallbackValue = field.Element("fielddefault").Value;
                fieldColumn.Binding = binding;
                grid.Columns.Add(fieldColumn);
                ColumnAdded += 1;
            }
        }

    }
}
