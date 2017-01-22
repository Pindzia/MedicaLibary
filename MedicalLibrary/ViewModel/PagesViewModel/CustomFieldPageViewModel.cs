using MedicalLibrary.Model;
using MedicalLibrary.View.CustomControls;
using MedicalLibrary.View.Windows;
using MedicalLibrary.ViewModel.CustomControlsViewModel;
using MedicalLibrary.ViewModel.WindowsViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml;
using System.Xml.Linq;

namespace MedicalLibrary.ViewModel.PagesViewModel
{
    public class CustomFieldPageViewModel : BaseViewModel
    {
        public CustomFieldPageViewModel()
        {
            LoadedCommand = new RelayCommand(pars =>Load());
            AddField = new RelayCommand(pars => Add());
            EditField = new RelayCommand(pars => Edit());
            DeleteField = new RelayCommand(pars => Delete());
            UpdateData();
        }

        private ObservableCollection<XElement> _ListOfCustomFields = new ObservableCollection<XElement>();
        public ObservableCollection<XElement> ListOfCustomFields
        {
            get
            {
                return _ListOfCustomFields;
            }

            set
            {
                _ListOfCustomFields = value;
                OnPropertyChanged("ListOfCustomFields");
            }
        }
        private XElement _SelectedField = null;
        public XElement SelectedField
        {
            get
            {
                return _SelectedField;
            }

            set
            {
                _SelectedField = value;
                OnPropertyChanged("SelectedField");
            }
        }

        private ObservableCollection<UserControl> _ItemPreview = new ObservableCollection<UserControl>();
        public ObservableCollection<UserControl> ItemPreview
        {
            get
            {
                return _ItemPreview;
            }

            set
            {
                _ItemPreview = value;
                OnPropertyChanged("ItemPreview");
            }
        }

        private XElement _NewField = null;
        public XElement NewField
        {
            get
            {
                return _NewField;
            }

            set
            {
                _NewField = value;
                OnPropertyChanged("NewField");
            }
        }
        public ICommand LoadedCommand { get; set; }
        public ICommand AddField { get; set; }
        public ICommand EditField { get; set; }
        public ICommand DeleteField { get; set; }

        private void Load()
        {
            UpdateData();
        }

        private void UpdateData()
        {
            ListOfCustomFields = ObserverCollectionConverter.Instance.Observe(XElementon.Instance.Field.Fields());
            DeployFields();
        }

        private void Add()
        {
            AddEditCustomFieldViewModel viewModel = new AddEditCustomFieldViewModel();
            AddEditCustomFieldWindow window = new AddEditCustomFieldWindow(ref viewModel);
            Nullable<bool> result = window.ShowDialog();
            if (result == true)
            {
                UpdateData();
            }
        }

        private void Edit()
        {
            if (SelectedField != null)
            {
                AddEditCustomFieldViewModel viewModel = new AddEditCustomFieldViewModel(SelectedField);
                AddEditCustomFieldWindow window = new AddEditCustomFieldWindow(ref viewModel);
                Nullable<bool> result = window.ShowDialog();
                if (result == true)
                {
                    UpdateData();
                }
            }
            else
            {
                MessageBox.Show("Wybierz Pole by edytować");
            }
        }

        private void Delete()
        {
            if (SelectedField != null)
            {
                if (MessageBox.Show("Czy chcesz wykasować Pole : " + SelectedField.Element("fieldname").Value , "Potwierdzenie", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    XElementon.Instance.Field.Delete((int)SelectedField.Element("idf"));
                    UpdateData();
                }
            }
            else
            {
                MessageBox.Show("Wybierz Pole by usunąć");
            }
        }


        private void DeployFields()
        {
            ItemPreview.Clear();
            foreach(XElement field in ListOfCustomFields)
            {
                string fieldName = field.Element("fieldname").Value;
                string fieldValue = field.Element("fielddefault").Value;
                switch (field.Element("fieldtype").Value)
                {
                    case "bool":
                        CheckControl checkControl = new CheckControl(new CheckControlViewModel(fieldName, XmlConvert.ToBoolean(fieldValue), false));
                        ItemPreview.Add(checkControl);
                        break;

                    case "int":
                        NumberControl numberControl = new NumberControl(new NumberControlViewModel(fieldName, fieldValue,false));
                        ItemPreview.Add(numberControl);
                        break;

                    case "string":
                        TextControl textControl = new TextControl(new TextControlViewModel(fieldName, fieldValue, false));
                        ItemPreview.Add(textControl);
                        break;
                }
            }
        }
    }
}
