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
    public class RulePageViewModel : BaseViewModel
    {
        public RulePageViewModel()
        {

        }

       private ObservableCollection<XElement> _Propname = new ObservableCollection<XElement>();
       public ObservableCollection<XElement> Propname
       {
            get
            {
                 return _Propname;
            }
            set
            {
                 _Propname=value;
                 OnPropertyChanged(nameof(Propname));
            }
       }
       



        public ICommand AddRuleToMagazine { get; set; }
        public ICommand DeleteRuleFromMagazine { get; set; }
        public ICommand AddRule { get; set; }
        public ICommand EditRule { get; set; }
        public ICommand DeleteRule { get; set; }
    }
}