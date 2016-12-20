using MedicaLibrary.Model;
using MedicalLibrary.View.Windows;
using MedicalLibrary.ViewModel.WindowsViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml.Linq;

namespace MedicalLibrary.ViewModel.PagesViewModel
{
    public class MagazinePageViewModel: BaseViewModel
    {
        public MagazinePageViewModel()
        {
            ShowMagazine = new RelayCommand(pars => ShowMagazineDetails((string)pars));
            AddMagazine = new RelayCommand(pars => Add());
            EditMagazine = new RelayCommand(pars => Edit());
            DeleteMagazine = new RelayCommand(pars => Delete());
            PushButton = new RelayCommand(pars =>Push());
            LoadedCommand = new RelayCommand(pars =>Load());
            UpdateData();
            
        }

        private ObservableCollection<XElement> _ListMagazine = new ObservableCollection<XElement>();
        public ObservableCollection<XElement> ListMagazine
        {
            get
            {
                return _ListMagazine;
            }
            set
            {
                _ListMagazine = value;
                ColMaxIndex = _ListMagazine.Count;
                OnPropertyChanged("ListMagazine");
            }
        }

        private int _ColMaxIndex = 0;
        public int ColMaxIndex
        {
            get
            {
                return _ColMaxIndex;
            }
            set
            {
                _ColMaxIndex = value;
                OnPropertyChanged("ColMaxIndex");
            }
        }

        private XElement _SelectedButton = null;
        public XElement SelectedButton
        {
            get
            {
                return _SelectedButton;
            }

            set
            {
                _SelectedButton = value;
                SelectedButtonIndex = ListMagazine.IndexOf(SelectedButton);
                OnPropertyChanged("SelectedButton");
            }
        }

        private int _SelectedButtonIndex = 0;
        public int SelectedButtonIndex
        {
            get
            {
                return _SelectedButtonIndex;
            }

            set
            {
                _SelectedButtonIndex = value;
                OnPropertyChanged("SelectedButtonIndex");
            }
        }

        private ObservableCollection<XElement> _PatientsOfMagazine = new ObservableCollection<XElement>();
        public ObservableCollection<XElement> PatientsOfMagazine
        {
            get
            {
                return _PatientsOfMagazine;
            }
            set
            {
                _PatientsOfMagazine = value;
                OnPropertyChanged("PatientsOfMagazine");
            }
        }

        private string _MagazineName = "";
        public string MagazineName
        {
            get
            {
                return _MagazineName;
            }
            set
            {
                _MagazineName = value;
                OnPropertyChanged("MagazineName");
            }
        }

        private string _MagazineId = "";
        public string MagazineId
        {
            get
            {
                return _MagazineId;
            }
            set
            {
                _MagazineId = value;
                OnPropertyChanged("MagazineId");
            }
        }

        private string _MagazineCount = "";
        public string MagazineCount
        {
            get
            {
                return _MagazineCount;
            }
            set
            {
                _MagazineCount = value;
                OnPropertyChanged("MagazineCount");
            }
        }

        private string _MagazinePriority = "";
        public string MagazinePriority
        {
            get
            {
                return _MagazinePriority;
            }
            set
            {
                _MagazinePriority = value;
                OnPropertyChanged("MagazinePriority");
            }
        }

        private XElement _NewRule = null;
        public XElement NewRule
        {
            get
            {
                return _NewRule;
            }
            set
            {
                _NewRule = value;
                OnPropertyChanged("NewRule");
            }
        }

        private XElement _NewMagazine = null;
        public XElement NewMagazine
        {
            get
            {
                return _NewMagazine;
            }
            set
            {
                _NewMagazine = value;
                OnPropertyChanged("NewMagazine");
            }
        }

        private int _NumOfWrong = 0;
        public int NumOfWrong
        {
            get
            {
                return _NumOfWrong;
            }

            set
            {
                _NumOfWrong = value;
                OnPropertyChanged("NumOfWrong");
            }
        }

        public ICommand ShowMagazine { get; set; }
        public ICommand AddMagazine { get; set; }
        public ICommand EditMagazine { get; set; }
        public ICommand DeleteMagazine { get; set; }
        public ICommand PushButton { get; set; }
        public ICommand LoadedCommand { get; set; }

        private void UpdateData()
        {
            ListMagazine = ObserverCollectionConverter.Instance.Observe(XElementon.Instance.Storehouse.Storehouses());
            ShowMagazineDetails(ListMagazine.FirstOrDefault().Element("ids").Value);
            NumOfWrong = XElementon.Instance.Patient.InWrongStorehouse().Count();
        }
        private void ShowMagazineDetails(string storehouseId)
        {
            int index;
            int.TryParse(storehouseId, out index);
            SelectedButton = ObserverCollectionConverter.Instance.Observe(XElementon.Instance.Storehouse.WithIDS(index)).FirstOrDefault();
            MagazineName = SelectedButton.Element("name").Value;
            PatientsOfMagazine = ObserverCollectionConverter.Instance.Observe(XElementon.Instance.Patient.WithStorehouseName(MagazineName));
            MagazineId = SelectedButton.Element("ids").Value;
            MagazineCount = PatientsOfMagazine.Count.ToString() + "/" + SelectedButton.Element("size").Value;
            MagazinePriority = SelectedButton.Element("priority").Value;
        }

        private void Add()
        {
            AddEditMagazineViewModel viewModel = new AddEditMagazineViewModel();
            AddEditMagazineWindow window = new AddEditMagazineWindow(ref viewModel);
            Nullable<bool> result = window.ShowDialog();
            if (result == true)
            {
                NewRule = viewModel.Rule;
                NewMagazine = viewModel.Magazine;
                Tuple<string, string> a = new Tuple<string, string>("name", (string)NewMagazine.Element("name"));
                Tuple<string, string> b = new Tuple<string, string>("size", (string)NewMagazine.Element("size"));
                Tuple<string, string> c = new Tuple<string, string>("priority", (string)NewMagazine.Element("priority"));
                Tuple<string, string>[] tup = { a, b, c };

                XElementon.Instance.Storehouse.Add(tup);

                Tuple<string, string> e = new Tuple<string, string>("attribute", (string)NewRule.Element("attribute"));
                Tuple<string, string> f = new Tuple<string, string>("operation", (string)NewRule.Element("operation"));
                Tuple<string, string> g = new Tuple<string, string>("value", (string)NewRule.Element("value"));
                Tuple<string, string>[] tup2 = { e, f, g };


                int addedIDS = XElementon.Instance.GetMaxIDS(); //TODO - wywalić te haxy, nie polecam
                XElementon.Instance.Rule.Add(addedIDS, tup2);
                UpdateData();
            }
        }

        private void Edit()
        {
            AddEditMagazineViewModel viewModel = new AddEditMagazineViewModel(SelectedButton);
            AddEditMagazineWindow window = new AddEditMagazineWindow(ref viewModel);
            Nullable<bool> result = window.ShowDialog();
            if (result == true)
            {
                NewRule = viewModel.Rule;
                NewMagazine = viewModel.Magazine;

                Tuple<string, string> a = new Tuple<string, string>("name", (string)NewMagazine.Element("name"));
                Tuple<string, string> b = new Tuple<string, string>("size", (string)NewMagazine.Element("size"));
                Tuple<string, string> c = new Tuple<string, string>("priority", (string)NewMagazine.Element("priority"));
                Tuple<string, string>[] tup = { a, b, c };

                XElementon.Instance.Storehouse.Change((int)SelectedButton.Element("ids"),tup);

                Tuple<string, string> e = new Tuple<string, string>("attribute", (string)NewRule.Element("attribute"));
                Tuple<string, string> f = new Tuple<string, string>("operation", (string)NewRule.Element("operation"));
                Tuple<string, string> g = new Tuple<string, string>("value", (string)NewRule.Element("value"));
                Tuple<string, string>[] tup2 = { e, f, g };


                int addedIDS = XElementon.Instance.GetMaxIDS(); //TODO - wywalić te haxy, nie polecam
                XElementon.Instance.Rule.Change(addedIDS, tup2);


                UpdateData();
            }
        }


        private void Delete()
        {
            if (MessageBox.Show("Czy chcesz wykasować Magazyn : " + SelectedButton.Element("name").Value , "Potwierdzenie", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                //SelectedButton <- data to delete
                XElementon.Instance.Storehouse.Delete((int)SelectedButton.Element("ids"));
                UpdateData();
            }
        }

        private void Push()
        {
            //
        }

        private void Load()
        {
            UpdateData();
        }
    }
}
