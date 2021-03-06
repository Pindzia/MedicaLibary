﻿using MedicalLibrary.Model;
using MedicalLibrary.ViewModel;
using MedicalLibrary.ViewModel.PagesViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Globalization;//Ivalueconverter
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;//Ivalueconverter
using System.Windows.Input;
using System.Xml.Linq;

namespace MedicalLibrary.ViewModel.PagesViewModel
{
    public class WrongStorehousPageViewModel : BaseViewModel
    {


        public WrongStorehousPageViewModel()
        {
            UpdateData();
            WhatStorehouse = new RelayCommand(pars => What());
            FixStorehouse = new RelayCommand(pars => Fix());
            LoadedCommand = new RelayCommand(pars => Loaded());
        }

        private void UpdateData()
        {
            WrongPatients = ObserverCollectionConverter.Instance.Observe(XElementon.Instance.Patient.InWrongStorehouse());
        }

        private ObservableCollection<XElement> _WrongPatients = new ObservableCollection<XElement>();
        public ObservableCollection<XElement> WrongPatients
        {
            get
            {
                return _WrongPatients;
            }

            set
            {
                _WrongPatients = value;
                NumOfWrong = WrongPatients.Count;
                OnPropertyChanged("WrongPatients");
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
                SelectedItemIndex = WrongPatients.IndexOf(SelectedItem);
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
                string change = value.ToString();
                OnPropertyChanged("NumOfWrong");
            }
        }

        public ICommand WhatStorehouse { get; set; }
        public ICommand FixStorehouse { get; set; }
        public ICommand LoadedCommand { get; set; }

        private void What()
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

        private void Fix()
        {
            if (SelectedItem != null)
            {
              XElementon.Instance.Patient.FixStorehouseEnvelope((int)SelectedItem.Element("idp"));
              UpdateData();
            }            else
            {
                MessageBox.Show("Wybierz Pacjenta by naprawić jego Magazyn i Kopertę");
            }
        }

        private void Loaded()
        {
            UpdateData();
        }

    }
}
