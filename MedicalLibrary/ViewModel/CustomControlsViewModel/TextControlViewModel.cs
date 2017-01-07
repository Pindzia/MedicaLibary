using MedicalLibrary.ViewModel.PagesViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalLibrary.ViewModel.CustomControlsViewModel
{
    public class TextControlViewModel : BaseViewModel
    {
        public TextControlViewModel()
        {

        }

        public TextControlViewModel(string fieldName, string textValue)
        {
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
                OnPropertyChanged("FieldValue");
            }
        }
    }
}
