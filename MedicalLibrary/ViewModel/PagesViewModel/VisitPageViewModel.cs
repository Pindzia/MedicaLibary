using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;
using MedicaLibrary.Model;
using System.Windows;

namespace MedicalLibrary.ViewModel.PagesViewModel
{
    public class VisitPageViewModel : BaseViewModel
    {

        public VisitPageViewModel()
        {
            ShowVisit = new RelayCommand(pars => ShowVisitInfo((string)pars));
        }
        private IEnumerable<XElement> _VisitToBind = XElementon.Instance.GetAllVisits();
        public IEnumerable<XElement> VisitToBind
        {
            get
            {
                return _VisitToBind;
            }

            set
            {
                _VisitToBind = value;
                OnPropertyChanged("VisitToBind");
            }
        }

        private string _VisitInfo = null;
        public string VisitInfo
        {
            get
            {
                return _VisitInfo;
            }

            set
            {
                _VisitInfo = value;
                OnPropertyChanged("VisitInfo");
            }
        }

        public ICommand ShowVisit { get; set; }
        private void ShowVisitInfo(string idv)
        {
            int idV = 0;

            if (int.TryParse(idv, out idV))
            {
                VisitInfo = XElementon.Instance.GetSpecificVisit(idV).Elements("idv").FirstOrDefault().Value;
            }
            else
            {
                MessageBox.Show("zły parsing");
            }
        }

    }
}
