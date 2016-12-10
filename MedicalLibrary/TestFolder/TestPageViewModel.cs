using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalLibrary.TestFolder
{
    class TestPageViewModel: INotifyPropertyChanged
    {
        public TestPageViewModel()
        {
           
        }
        public event PropertyChangedEventHandler PropertyChanged = null;
        private DataView _DataView = new DataView(DataTableTemplate.Table);
        public DataView DataView
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
