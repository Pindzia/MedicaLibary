using MedicalLibrary.ViewModel.PagesViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;

namespace MedicalLibrary.ViewModel.WindowsViewModel
{
    public class AddEditVisitViewModel: BaseViewModel
    {
        public AddEditVisitViewModel()
        {
            SaveVisit = new RelayCommand(pars => Add((AddEditVisitViewModel)pars));
        }

        public AddEditVisitViewModel(XElement visit)
        {

        }

        private DateTime _FullDate = DateTime.Now;
        public DateTime FullDate
        {
            get
            {
                return _FullDate;
            }
            set
            {
                _FullDate = value;
                OnPropertyChanged("FullDate");
            }
        }

        private string _Comment = "";
        public string Comment
        {
            get
            {
                return _Comment;
            }
            set
            {
                _Comment = value;
                OnPropertyChanged("Comment");
            }
        }

        public ICommand SaveVisit;

        private void Add(AddEditVisitViewModel pars)
        {
            throw new NotImplementedException();
        }


    }
}
