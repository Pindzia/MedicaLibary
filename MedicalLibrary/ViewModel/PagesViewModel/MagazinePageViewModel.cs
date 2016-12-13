using MedicaLibrary.Model;
using MedicalLibrary.View.Windows;
using MedicalLibrary.ViewModel.WindowsViewModel;
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
    public class MagazinePageViewModel: BaseViewModel
    {
        public MagazinePageViewModel()
        {
            ShowMagazine = new RelayCommand(pars => ShowMagazineDetails((string)pars));
            AddMagazine = new RelayCommand(pars => Add());
            PushButton = new RelayCommand(pars =>Push());
            ListMagazine = ObserverCollectionConverter.Instance.Observe(XElementon.Instance.Storehouse.Storehouses());
            ShowMagazineDetails(ListMagazine.FirstOrDefault().Element("ids").Value);
            
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

        public ICommand ShowMagazine { get; set; }
        public ICommand AddMagazine { get; set; }
        public ICommand EditMagazine { get; set; }
        public ICommand DeleteMagazine { get; set; }
        public ICommand PushButton { get; set; }

        private void ShowMagazineDetails(string storehouseId)
        {
            int index;
            int.TryParse(storehouseId, out index);
            SelectedButton = ObserverCollectionConverter.Instance.Observe(XElementon.Instance.Storehouse.WithIDS(index)).FirstOrDefault();
            MagazineName = SelectedButton.Element("name").Value;
            PatientsOfMagazine = ObserverCollectionConverter.Instance.Observe(XElementon.Instance.Patient.WithStorehouseName(MagazineName));
            MagazineId = SelectedButton.Element("ids").Value;
            MagazineCount = PatientsOfMagazine.Count.ToString() + "/" + SelectedButton.Element("size").Value;
        }

        private void Add()
        {
            AddEditMagazineViewModel viewModel = new AddEditMagazineViewModel();
            AddEditMagazineWindow window = new AddEditMagazineWindow(ref viewModel);
            Nullable<bool> result = window.ShowDialog();
            if (result == true)
            {
                
            }
        }

        private void Push()
        {
            //
        }
    }
}
