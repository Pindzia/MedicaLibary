using MedicalLibrary.Model;
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
            LoadedCommand = new RelayCommand(pars => UpdateData());
            UpdateData();

        }

        private ObservableCollection<XElement> _RuleList = new ObservableCollection<XElement>();
        public ObservableCollection<XElement> RuleList
        {
            get
            {
                return _RuleList;
            }
            set
            {
                _RuleList = value;
                OnPropertyChanged(nameof(RuleList));
            }
        }

        private ObservableCollection<XElement> _MagazineList = new ObservableCollection<XElement>();
        public ObservableCollection<XElement> MagazineList
        {
            get
            {
                return _MagazineList;
            }
            set
            {
                _MagazineList = value;
                OnPropertyChanged(nameof(MagazineList));
            }
        }

        private ObservableCollection<XElement> _RulesOfMagazine = new ObservableCollection<XElement>();
        public ObservableCollection<XElement> RulesOfMagazine
        {
            get
            {
                return _RulesOfMagazine;
            }
            set
            {
                _RulesOfMagazine = value;
                OnPropertyChanged(nameof(RulesOfMagazine));
                SelectedRuleOfMagazine = RulesOfMagazine.FirstOrDefault();
            }
        }

        private XElement _SelectedRule = null;
        public XElement SelectedRule
        {
            get
            {
                return _SelectedRule;
            }
            set
            {
                _SelectedRule = value;
                OnPropertyChanged(nameof(SelectedRule));
            }
        }

        private XElement _SelectedMagazine = null;
        public XElement SelectedMagazine
        {
            get
            {
                return _SelectedMagazine;
            }
            set
            {
                _SelectedMagazine = value;
                RulesOfMagazine = ObserverCollectionConverter.Instance.Observe(XElementon.Instance.Rule.WithIDS((int)SelectedMagazine.Element("ids")));
                OnPropertyChanged(nameof(SelectedMagazine));
            }
        }

        private XElement _SelectedRuleOfMagazine = null;
        public XElement SelectedRuleOfMagazine
        {
            get
            {
                return _SelectedRuleOfMagazine;
            }
            set
            {
                _SelectedRuleOfMagazine = value;
                OnPropertyChanged(nameof(SelectedRuleOfMagazine));
            }
        }



        public ICommand AddRuleToMagazine { get; set; }
        public ICommand DeleteRuleFromMagazine { get; set; }
        public ICommand AddRule { get; set; }
        public ICommand EditRule { get; set; }
        public ICommand DeleteRule { get; set; }
        public ICommand LoadedCommand { get; set; }

        private void UpdateData()
        {
            RuleList = ObserverCollectionConverter.Instance.Observe(XElementon.Instance.Rule.Rules());
            SelectedRule = RuleList.FirstOrDefault();
            MagazineList = ObserverCollectionConverter.Instance.Observe(XElementon.Instance.Storehouse.Storehouses());
            SelectedMagazine = MagazineList.FirstOrDefault();
        }
    }
}