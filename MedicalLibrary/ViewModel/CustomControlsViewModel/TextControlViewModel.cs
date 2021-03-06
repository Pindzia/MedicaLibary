﻿using MedicalLibrary.ViewModel.PagesViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MedicalLibrary.ViewModel.CustomControlsViewModel
{
    public class TextControlViewModel : BaseViewModel
    {
        public TextControlViewModel()
        {

        }

        public TextControlViewModel(string fieldName, string textValue, bool enabled, bool check = false)
        {
            IsEnabled = enabled;
            IsChecked = check;
            FieldName = fieldName;
            FieldValue = textValue;
        }

        private string _FieldName = "";
        public string FieldName
        {
            get
            {
                return _FieldName;
            }
            set
            {
                _FieldName = value;
                OnPropertyChanged("FieldName");
            }
        }

        private string _FieldValue = "";
        public string FieldValue
        {
            get
            {
                return _FieldValue;
            }
            set
            {
                _FieldValue = value;
                Regex regex = new Regex("[a-zA-Z0-9_]+");
                IsGood = (!regex.IsMatch(FieldValue)) ? true : false;
                OnPropertyChanged("FieldValue");
            }
        }

        private bool _IsChecked = false;
        public bool IsChecked
        {
            get
            {
                return _IsChecked;
            }
            set
            {
                _IsChecked = value;
                OnPropertyChanged("IsChecked");
            }
        }

        private bool _IsEnabled = false;
        public bool IsEnabled
        {
            get
            {
                return _IsEnabled;
            }
            set
            {
                _IsEnabled = value;
                OnPropertyChanged("IsEnabled");
            }
        }

        private bool _IsGood = false;
        public bool IsGood
        {
            get
            {
                return _IsGood;
            }
            set
            {
                _IsGood = value;
                OnPropertyChanged("IsGood");
            }
        }
    }
}
