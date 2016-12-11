using System.ComponentModel;
using System.Windows.Input;
using System.Xml.Linq;
using MedicalLibrary.View.Windows;
using System;
using MedicalLibrary.ViewModel.PatientPageViewModel;
using System.Collections.ObjectModel;
using System.Linq;
using System.Collections.Generic;

namespace MedicalLibrary.ViewModel.WindowsViewModel
{
    public class PatientPageViewModel : INotifyPropertyChanged
    {

        public PatientPageViewModel()
        {
            AddPatient = new RelayCommand(pars => Add());
            EditPatient = new RelayCommand(pars => Edit());
            //foreach (var xelement in MedicaLibrary.Model.XElementon.Instance.GetAllPatients())
            //    DataToBind.Add(xelement);
            DataToBind = MedicaLibrary.Model.ObserverCollectionConverter.Instance.Observe(MedicaLibrary.Model.XElementon.Instance.GetAllPatients());
        }
        public event PropertyChangedEventHandler PropertyChanged = null;
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
        //todo chancge typesto select item to xelement and observablecollection jako databind

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


        virtual protected void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        public ICommand AddPatient { get; set; }
        public ICommand EditPatient { get; set; }

        private void Add()
        {
            AddPatientViewModel viewModel = new AddPatientViewModel();
            AddPatientWindow window = new AddPatientWindow(ref viewModel);
            Nullable<bool> result = window.ShowDialog();
            if (result == true)
            {
                NewPatient = viewModel.Patient;
                DataToBind.Add(NewPatient);
            }
        }

        private void Edit()
        {
           // MessageBox.Show();
        }
    }
}
