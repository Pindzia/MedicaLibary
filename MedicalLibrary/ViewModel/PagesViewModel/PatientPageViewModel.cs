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
        }
        public event PropertyChangedEventHandler PropertyChanged = null;
        private XElement _DataToBind = XElement.Load("lib.xml");
        public XElement DataToBind
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

        private ObservableCollection<XElement> _SelectedItem = new ObservableCollection<XElement>();
        public ObservableCollection<XElement> SelectedItem
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
