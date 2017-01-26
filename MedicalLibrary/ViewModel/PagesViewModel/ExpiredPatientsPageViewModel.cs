using MedicalLibrary.Model;
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
    public class ExpiredPatientsPageViewModel :BaseViewModel
    {
        public ExpiredPatientsPageViewModel()
        {
            DestroyPatient = new RelayCommand(pars => Destroy());
            LoadedCommand = new RelayCommand(pars => Loaded());
            UpdateData();
        }

        private void UpdateData()
        {
            ExpiredPatients = ObserverCollectionConverter.Instance.Observe(XElementon.Instance.Patient.OutdatedPatients());
        }

        private ObservableCollection<XElement> _ExpiredPatients = new ObservableCollection<XElement>();
        public ObservableCollection<XElement> ExpiredPatients
        {
            get
            {
                return _ExpiredPatients;
            }

            set
            {
                _ExpiredPatients = value;
                SelectedItem = ExpiredPatients.FirstOrDefault();
                OnPropertyChanged("ExpiredPatients");
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
                OnPropertyChanged("SelectedItem");
            }
        }


        public ICommand DestroyPatient { get; set; }
        public ICommand LoadedCommand { get; set; }

        private void Destroy()
        {
            if (SelectedItem != null)
            {
                if (MessageBox.Show("Czy zniszczyłeś daną dokumentacje Pacjenta: " + SelectedItem.Element("imie").Value + " " + SelectedItem.Element("nazwisko").Value, "Potwierdzenie", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    XElementon.Instance.Patient.Delete((int)SelectedItem.Element("idp"));
                    UpdateData();
                }
            }
            else
            {
                MessageBox.Show("Wybierz Pacjenta aby usunąć jego dokumentacje");
            }
        }

        private void Loaded()
        {
            UpdateData();
        }
    }
}
