using MedicalLibrary.ViewModel.PagesViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MedicalLibrary.ViewModel.CustomControlsViewModel
{
    public class NumberControlViewModel : BaseViewModel
    {
        public NumberControlViewModel()
        {

        }

        public NumberControlViewModel(string name, string value, bool enabled, bool check = false)
        {
            IsEnabled = enabled;
            IsChecked = check;
            FieldName = name;
            FieldValue = value;
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

        private string _FieldValue ="";
        public string FieldValue
        {
            get
            {
                return _FieldValue;
            }
            set
            {
                _FieldValue = value;
                Regex regex = new Regex("-?[0-9,]+");
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
