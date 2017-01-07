using MedicalLibrary.ViewModel.PagesViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalLibrary.ViewModel.CustomControlsViewModel
{
    public class CheckControlViewModel : BaseViewModel
    {

        public CheckControlViewModel()
        {
        }

        public CheckControlViewModel(string name, bool value)
        {
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

        private bool _FieldValue = false;
        public bool FieldValue
        {
            get
            {
                return _FieldValue;
            }
            set
            {
                _FieldValue = value;
                OnPropertyChanged("FieldValue");
            }
        }
    }
}
