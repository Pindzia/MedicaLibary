using MedicaLibrary.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;

namespace MedicalLibrary.ViewModel.PagesViewModel
{
     public class ModificationViewModel : BaseViewModel
    {
        public ModificationViewModel()
        {
            LoadedCommand = new RelayCommand(pars => Loaded());
            UpdateData();
        }


        private ObservableCollection<XElement> _ModificationList = new ObservableCollection<XElement>();
        public ObservableCollection<XElement> ModificationList
        {
            get
            {
                return _ModificationList;
            }

            set
            {
                _ModificationList = value;
                OnPropertyChanged("ModificationList");
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

        public ICommand LoadedCommand { get; set; }

        private void Loaded()
        {
            UpdateData();
        }

        private void UpdateData()
        {
            ModificationList = ObserverCollectionConverter.Instance.Observe(XElementon.Instance.Modification.Modifications());
        }
    }
}
