using MedicaLibrary.Model;
using MedicalLibrary.View.CustomControls;
using MedicalLibrary.ViewModel.CustomControlsViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml;
using System.Xml.Linq;

namespace MedicalLibrary.ViewModel.PagesViewModel
{
    public class CustomFieldPageViewModel : BaseViewModel
    {
        public CustomFieldPageViewModel()
        {
            ListOfCustomFields = ObserverCollectionConverter.Instance.Observe(XElementon.Instance.Field.Fields());
            DeployFields();
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

        private List<UserControl> _ItemPreview = new List<UserControl>();
        public List<UserControl> ItemPreview
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

        private void DeployFields()
        {
            foreach(XElement field in ListOfCustomFields)
            {
                string fieldName = field.Element("fieldname").Value;
                string fieldValue = field.Element("fielddefault").Value;
                switch (field.Element("fieldtype").Value)
                {
                    case "bool":
                        CheckControl checkControl = new CheckControl(new CheckControlViewModel(fieldName, XmlConvert.ToBoolean(fieldValue)));
                        ItemPreview.Add(checkControl);
                        break;

                    case "int":
                        TextControl textControl = new TextControl(new TextControlViewModel(fieldName, fieldValue));
                        ItemPreview.Add(textControl);
                        break;
                }
            }
        }
    }
}
