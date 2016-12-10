using System.ComponentModel;
using System.Windows.Input;
using System.Xml.Linq;
using MedicalLibrary.View.Windows;
using System;
using MedicalLibrary.ViewModel.PatientPageViewModel;
using System.Collections.ObjectModel;
using System.Linq;
using System.Collections.Generic;
using MedicalLibrary.Model.MappedModels;

namespace MedicalLibrary.ViewModel.WindowsViewModel
{
    public class PatientPageViewModel : INotifyPropertyChanged
    {

        public PatientPageViewModel()
        {
            AddPatient = new RelayCommand(pars => Add());
            EditPatient = new RelayCommand(pars => Edit());
            
            for(int i=0;i<=9;i++)
            {
                Patient first = new Patient(i, 1, true, "Name", "Surname", 10111213141, 1, 0);
                DataToBind.Add(first);
            }
        }
        public event PropertyChangedEventHandler PropertyChanged = null;
        private ObservableCollection<Patient> _DataToBind = new ObservableCollection<Patient>();
        public ObservableCollection<Patient> DataToBind
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

        private Patient _SelectedItem = null;
        public Patient SelectedItem
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
                //DataToBind.Add(NewPatient);
            }
        }

        private void Edit()
        {
           // MessageBox.Show();
        }
    }
}
