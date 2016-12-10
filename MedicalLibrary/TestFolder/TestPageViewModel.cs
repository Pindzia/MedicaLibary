using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MedicalLibrary.TestFolder
{
    class TestPageViewModel: INotifyPropertyChanged
    {
        public TestPageViewModel()
        {
            ObservableCollection<XElement> listToBind = new ObservableCollection<XElement>();
            foreach(var p in XElement.Load("lib.xml").Elements())
            {
                listToBind.Add((XElement)p);
            }
            DataView = listToBind;

        }
        public event PropertyChangedEventHandler PropertyChanged = null;
        private ObservableCollection<XElement> _DataView = null;
        public ObservableCollection<XElement> DataView
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

        virtual protected void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}
