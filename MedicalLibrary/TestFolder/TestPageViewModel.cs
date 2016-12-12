using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Globalization;//Ivalueconverter
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;//Ivalueconverter
using System.Xml.Linq;

namespace MedicalLibrary.TestFolder
{
    class TestPageViewModel: INotifyPropertyChanged
    {


        public TestPageViewModel()
        {
            //DataView = MedicaLibrary.Model.ObserverCollectionConverter.Instance.Observe(MedicaLibrary.Model.XElementon.Instance.GetAllPatients());
           // var a  = (MedicaLibrary.Model.XElementon.Instance.Patient.GetAllPatients()).ToList();
            //DataView = (MedicaLibrary.Model.XElementon.Instance.Patient.GetAllPatients()).ToList(); //ObservableCollection<XElement>
        }
        public event PropertyChangedEventHandler PropertyChanged = null;
        private List<XElement> _DataView = new List<XElement>();
        public List<XElement> DataView
        {
            get
            {
                return _DataView;
            }

            set
            {
                _DataView = value;
                OnPropertyChanged("DataView");
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

        virtual protected void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}
