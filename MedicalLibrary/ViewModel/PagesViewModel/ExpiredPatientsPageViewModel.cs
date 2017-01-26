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
            UpdateData();
            DestroyPatient = new RelayCommand(pars => Destroy());
            LoadedCommand = new RelayCommand(pars => Loaded());
        }

        private void UpdateData()
        {
            ExpiredPatients = ObserverCollectionConverter.Instance.Observe(XElementon.Instance.Patient.InWrongStorehouse());
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
                Tuple<string, string> Answer = XElementon.Instance.Patient.WhatStorehouseEnvelope((int)SelectedItem.Element("idp"));
                MessageBox.Show("Powinno sie przenieś wybranego pacjenta do magazynu o nazwie: \n" + Answer.Item1 + "\nw kopercie o numerze: " + Answer.Item2);
            }
            else
            {
                MessageBox.Show("Wybierz Pacjenta by dowiedzieć się do jakiego magazynu należy");
            }
        }

        private void Loaded()
        {
            UpdateData();
        }
    }
}
