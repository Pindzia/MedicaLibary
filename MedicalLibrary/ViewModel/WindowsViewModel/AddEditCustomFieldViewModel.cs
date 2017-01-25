using MedicalLibrary.Model;
using MedicalLibrary.View.Windows;
using MedicalLibrary.ViewModel.PagesViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml;
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
            SaveField = new RelayCommand(pars => Save((AddEditCustomFieldWindow)pars));
            CancelField = new RelayCommand(pars => Cancel((AddEditCustomFieldWindow)pars));
            CustomField = EditCustomField;
            FieldName = CustomField.Element("fieldname").Value;
            SelectedType = CustomField.Element("fieldtype").Value;
            switch(SelectedType)
            {
                case "int":
                    NumberDefault = CustomField.Element("fielddefault").Value;
                    break;
                case "bool":
                    CheckDefault = XmlConvert.ToBoolean(CustomField.Element("fielddefault").Value);
                    break;
                case "string":
                    TextDefault = CustomField.Element("fielddefault").Value;
                    break;
            }

        }

        private List<string> _ListTypes = new List<string>() {"bool","int","string"};
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

        private string _Suffix = "";
        public string Suffix
        {
            get
            {
                return _Suffix;
            }
            set
            {
                _Suffix = value;
                OnPropertyChanged("Suffix");
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
                Regex regex = new Regex("[a-zA-Z0-9_]+");
                IsGoodText = (!regex.IsMatch(_TextDefault)) ? true : false;
                OnPropertyChanged("TextDefault");
            }
        }

        private string _NumberDefault = "";
        public string NumberDefault
        {
            get
            {
                return _NumberDefault;
            }
            set
            {
                _NumberDefault = value;
                Regex regex = new Regex("-?[0-9,]+");
                IsGoodNum = (!regex.IsMatch(_NumberDefault)) ? true : false;
                OnPropertyChanged("NumberDefault");
            }
        }

        private bool _IsGoodNum = false;
        public bool IsGoodNum
        {
            get
            {
                return _IsGoodNum;
            }
            set
            {
                _IsGoodNum = value;
                OnPropertyChanged(nameof(IsGoodNum));
            }
        }

        private bool _IsGoodText = false;
        public bool IsGoodText
        {
            get
            {
                return _IsGoodText;
            }
            set
            {
                _IsGoodText = value;
                OnPropertyChanged(nameof(IsGoodText));
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

        private Visibility _NumberVisibility = Visibility.Visible;
        public Visibility NumberVisibility
        {
            get
            {
                return _NumberVisibility;
            }
            set
            {
                _NumberVisibility = value;
                OnPropertyChanged("NumberVisibility");
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
            Tuple<string, string> a = new Tuple<string, string>("fieldname", FieldName);
            Tuple<string, string> b = new Tuple<string, string>("fieldtype", SelectedType);
            Tuple<string, string> c = new Tuple<string, string>("","");
            Tuple<string, string> d = new Tuple<string, string>("suffix", Suffix);

            switch (SelectedType)
            {
                case "int":
                    c = new Tuple<string, string>("fielddefault", NumberDefault);
                    break;
                case "bool":
                    c = new Tuple<string, string>("fielddefault", XmlConvert.ToString(CheckDefault));
                    break;
                case "string":
                    c = new Tuple<string, string>("fielddefault", TextDefault);
                    break;
            }

            Tuple<string, string>[] newField = { a, b, c, d };

            if (CustomField !=null)
            {
                switch(SelectedType)
                {
                    case "int":
                        if(!IsGoodNum)
                        {
                            XElementon.Instance.Field.Change((int)CustomField.Element("idf"), newField);
                            window.DialogResult = true;
                            window.Close();
                        }
                        break;
                    case "bool":
                        XElementon.Instance.Field.Change((int)CustomField.Element("idf"), newField);
                        window.DialogResult = true;
                        window.Close();
                        break;
                    case "string":
                        if (!IsGoodText)
                        {
                            XElementon.Instance.Field.Change((int)CustomField.Element("idf"), newField);
                            window.DialogResult = true;
                            window.Close();
                        }
                        break;
                }
            }
            else
            {
                switch (SelectedType)
                {
                    case "int":
                        if (!IsGoodNum)
                        {
                            XElementon.Instance.Field.Add(newField);
                            window.DialogResult = true;
                            window.Close();
                        }
                        break;
                    case "bool":
                        XElementon.Instance.Field.Add(newField);
                        window.DialogResult = true;
                        window.Close();
                        break;
                    case "string":
                        if (!IsGoodText)
                        {
                            XElementon.Instance.Field.Add(newField);
                            window.DialogResult = true;
                            window.Close();
                        }
                        break;
                }
            }
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
                    NumberVisibility = Visibility.Visible;
                    CheckVisibility = Visibility.Hidden;
                    TextVisibility = Visibility.Hidden;
                    TextDefault = "";
                    break;
                case "string":
                    NumberVisibility = Visibility.Hidden;
                    CheckVisibility = Visibility.Hidden;
                    TextVisibility = Visibility.Visible;
                    NumberDefault = "";
                    break;
                case "bool":
                    NumberVisibility = Visibility.Hidden;
                    CheckVisibility = Visibility.Visible;
                    TextVisibility = Visibility.Hidden;
                    NumberDefault = "";
                    TextDefault = "";
                    break;
            }
        }
    }
}
