using MedicalLibrary.Model;
using MedicalLibrary.View.Windows;
using MedicalLibrary.ViewModel.PagesViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml.Linq;

namespace MedicalLibrary.ViewModel.WindowsViewModel
{
    public class AddEditRuleWindowViewModel : BaseViewModel
    {

        public AddEditRuleWindowViewModel()
        {
            ListAttributes = XElementon.Instance.Storehouse.Attributes();
            SelectedAttribute = ListAttributes.FirstOrDefault();
            SaveRule = new RelayCommand(pars => Save((AddEditRuleWindow)pars));
            CancelRule = new RelayCommand(pars => Cancel((AddEditRuleWindow)pars));
        }


        public AddEditRuleWindowViewModel(XElement ParsedRule)
        {
            Rule = ParsedRule;
            ListAttributes = XElementon.Instance.Storehouse.Attributes();
            SelectedAttribute = ParsedRule.Element("attribute").Value;
            SelectedOperation = ParsedRule.Element("operation").Value;
            VarOfRule = ParsedRule.Element("value").Value;
            SaveRule = new RelayCommand(pars => Save((AddEditRuleWindow)pars));
            CancelRule = new RelayCommand(pars => Cancel((AddEditRuleWindow)pars));
        }

        private List<string> _ListAttributes = new List<string>();
        public List<string> ListAttributes
        {
            get
            {
                return _ListAttributes;
            }
            set
            {
                _ListAttributes = value;
                OnPropertyChanged("ListAttributes");
            }
        }

        private string _SelectedAttribute = "";
        public string SelectedAttribute
        {
            get
            {
                return _SelectedAttribute;
            }
            set
            {
                _SelectedAttribute = value;
                ListOperation = XElementon.Instance.Storehouse.Operations(_SelectedAttribute);
                OnPropertyChanged("SelectedAttribute");
            }
        }

        private List<string> _ListOperation = new List<string>();
        public List<string> ListOperation
        {
            get
            {
                return _ListOperation;
            }
            set
            {
                _ListOperation = value;
                OnPropertyChanged("ListOperation");
            }
        }

        private string _SelectedOperation = "";
        public string SelectedOperation
        {
            get
            {
                return _SelectedOperation;
            }
            set
            {
                _SelectedOperation = value;
                OnPropertyChanged("SelectedOperation");
            }
        }

        private string _VarOfRule = "";
        public string VarOfRule
        {
            get
            {
                return _VarOfRule;
            }
            set
            {
                _VarOfRule = value;
                OnPropertyChanged("VarOfRule");
            }
        }

        private XElement _Rule = null;
        public XElement Rule
        {
            get
            {
                return _Rule;
            }
            set
            {
                _Rule = value;
                OnPropertyChanged("Rule");
            }
        }

        public ICommand SaveRule { get; set; }
        public ICommand CancelRule { get; set; }

        private void Save(AddEditRuleWindow window)
        {
            if (SelectedAttribute != "" && SelectedOperation != "" && VarOfRule != "")
            {
                CreateRule();
                window.DialogResult = true;
                window.Close();
            }
            else
            {
                MessageBox.Show("Wartość w polach nie są wybrane");
            }
        }

        private void Cancel(AddEditRuleWindow window)
        {
            window.DialogResult = false;
            window.Close();
        }

        private void CreateRule()
        {
            if (Rule != null) //Edycja
            {

                var list = new List<Tuple<string, string>>();
                Tuple<string, string> a = new Tuple<string, string>("attribute", SelectedAttribute);
                Tuple<string, string> b = new Tuple<string, string>("operation", SelectedOperation);
                Tuple<string, string> c = new Tuple<string, string>("value", VarOfRule);
                list.Add(a);
                list.Add(b);
                list.Add(c);

                XElementon.Instance.Rule.Change((int)Rule.Element("idr"), list.ToArray());

                //Rule.Element("attribute").Value = SelectedAttribute;
                //Rule.Element("operation").Value = SelectedOperation;
                //Rule.Element("value").Value = VarOfRule;
            }
            else //Dodawanie
            {
                Rule = new XElement(
                    new XElement("rule",
                    new XElement("attribute", SelectedAttribute),
                    new XElement("operation", SelectedOperation),
                    new XElement("value", VarOfRule)));
            }
        }

    }
}
