using MedicalLibrary.View.Windows;
using MedicalLibrary.ViewModel.PagesViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml.Linq;

namespace MedicalLibrary.ViewModel.WindowsViewModel
{
    public class AddEditCustomFieldViewModel:BaseViewModel
    {
        public AddEditCustomFieldViewModel()
        {
            SaveField = new RelayCommand(pars => Save((AddEditCustomFieldWindow)pars));
            CancelField = new RelayCommand(pars => Cancel((AddEditCustomFieldWindow)pars));
            SelectedType = ListTypes.FirstOrDefault();
        }

        public AddEditCustomFieldViewModel(XElement EditCustomField)
        {

        }

        private List<string> _ListTypes = new List<string>() {"bool","int"};
        public List<string> ListTypes
        {
            get
            {
                return _ListTypes;
            }
            set
            {
                _ListTypes = value;
                OnPropertyChanged("ListTypes");
            }
        }

        private string _SelectedType = "";
        public string SelectedType
        {
            get
            {
                return _SelectedType;
            }
            set
            {
                _SelectedType = value;
                ChangeVisibility();
                OnPropertyChanged("SelectedType");
            }
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

        private bool _CheckDefault = false;
        public bool CheckDefault
        {
            get
            {
                return _CheckDefault;
            }
            set
            {
                _CheckDefault = value;
                OnPropertyChanged("CheckDefault");
            }
        }

        private string _TextDefault = "";
        public string TextDefault
        {
            get
            {
                return _TextDefault;
            }
            set
            {
                _TextDefault = value;
                OnPropertyChanged("TextDefault");
            }
        }

        private Visibility _CheckVisibility = Visibility.Visible;
        public Visibility CheckVisibility
        {
            get
            {
                return _CheckVisibility;
            }
            set
            {
                _CheckVisibility = value;
                OnPropertyChanged("CheckVisibility");
            }
        }

        private Visibility _TextVisibility = Visibility.Visible;
        public Visibility TextVisibility
        {
            get
            {
                return _TextVisibility;
            }
            set
            {
                _TextVisibility = value;
                OnPropertyChanged("TextVisibility");
            }
        }

        private XElement _CustomField = null;
        public XElement CustomField
        {
            get
            {
                return _CustomField;
            }
            set
            {
                _CustomField = value;
                OnPropertyChanged("CustomField");
            }
        }

        public ICommand SaveField { get; set; }
        public ICommand CancelField { get; set; }

        private void Save(AddEditCustomFieldWindow window)
        {

        }

        private void Cancel(AddEditCustomFieldWindow window)
        {
            window.DialogResult = false;
            window.Close();
        }


        private void ChangeVisibility()
        {
            switch(SelectedType)
            {
                case "int":
                    CheckVisibility = Visibility.Hidden;
                    TextVisibility = Visibility.Visible;
                    break;
                case "bool":
                    CheckVisibility = Visibility.Visible;
                    TextVisibility = Visibility.Hidden;
                    break;
            }
        }
    }
}
