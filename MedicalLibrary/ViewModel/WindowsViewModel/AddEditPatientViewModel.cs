using MedicaLibrary.Model;
using MedicalLibrary.View.CustomControls;
using MedicalLibrary.View.Windows;
using MedicalLibrary.ViewModel.CustomControlsViewModel;
using MedicalLibrary.ViewModel.PagesViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml;
using System.Xml.Linq;

namespace MedicalLibrary.ViewModel.WindowsViewModel
{
    public class AddEditPatientViewModel : BaseViewModel
    {

        public AddEditPatientViewModel()
        {
            ListMagazines = XElementon.Instance.Storehouse.StorehouseNameList();
            SavePatient = new RelayCommand(pars => Save((AddEditPatientWindow)pars));
            DeployFields(null);
        }

        public AddEditPatientViewModel(XElement EditPatient)
        {
            ListMagazines = XElementon.Instance.Storehouse.StorehouseNameList();
            SavePatient = new RelayCommand(pars => Save((AddEditPatientWindow)pars));

            IDP = EditPatient.Element("idp").Value;
            LastName = EditPatient.Element("nazwisko").Value;
            FirstName = EditPatient.Element("imie").Value;
            Pesel = EditPatient.Element("pesel").Value;
            IsEnabled = true;
            SelectedAttribute = EditPatient.Element("storehouse").Value;
            Envelope = EditPatient.Element("envelope").Value;
            DeployFields(EditPatient);
        }

        public ICommand SavePatient { get; set; }
        public ICommand CheckMagazine { get; set; }

        private string _IDP = "";
        public string IDP
        {
            get
            {
                return _IDP;
            }
            set
            {
                _IDP = value;
                Check();

            }
        }

        private string _LastName = "";
        public string LastName
        {
            get
            {
                return _LastName;
            }
            set
            {
                _LastName = value;
                OnPropertyChanged("LastName");
                Check();
                VerifyName("LastName");
            }
        }

        private List<UserControl> _ListCustomField = new List<UserControl>();
        public List<UserControl> ListCustomField
        {
            get
            {
                return _ListCustomField;
            }
            set
            {
                _ListCustomField = value;
                OnPropertyChanged("ListCustomField");
            }
        }

        private string _FirstName = "";
        public string FirstName
        {
            get
            {
                return _FirstName;
            }
            set
            {
                _FirstName = value;
                OnPropertyChanged("FirstName");
                Check();
                VerifyName("FirstName");
            }
        }

        private string _Pesel = "";
        public string Pesel
        {
            get
            {
                return _Pesel;
            }
            set
            {
                _Pesel = value;
                OnPropertyChanged("Pesel");
                Check();
                VerifyName("Pesel");
            }
        }
        private XElement _Patient = null;
        public XElement Patient
        {
            get
            {
                return _Patient;
            }
            set
            {
                _Patient = value;
                OnPropertyChanged("Patient");
            }
        }

        private List<string> _ListMagazines = new List<string>();
        public List<string> ListMagazines
        {
            get
            {
                return _ListMagazines;
            }
            set
            {
                _ListMagazines = value;
                OnPropertyChanged("ListMagazines");
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
                if (SelectedAttribute != "")
                {
                    Envelope = XElementon.Instance.CheckingRules(new XElement("AnyElement"), false, SelectedAttribute)[1].Value;
                }
                else
                {
                    Envelope = "";
                }

                OnPropertyChanged("SelectedAttribute");
            }
        }

        private string _Envelope = null;
        public string Envelope
        {
            get
            {
                return _Envelope;
            }
            set
            {
                _Envelope = value;
                OnPropertyChanged("Envelope");
            }
        }

        private bool _IsEnabled = false;
        public bool IsEnabled
        {
            get
            {
                return _IsEnabled;
            }
            set
            {
                _IsEnabled = value;
                OnPropertyChanged("IsEnabled");
                Check();
            }
        }


        private XElement CreatePatient()
        {
            string Id;
            var imie = FirstName;
            var nazwisko = LastName;
            var pesel = Pesel;

            //Autonumeracja po id - olewamy 'dziury'
            var max = XElement.Load("lib.xml").Descendants("max_idp").First();
            Id = max.Value;
            max.Value = (Convert.ToInt16(Id) + 1).ToString();

            //Tworzymy element pacjent który później wszczepimy w nasz dokument
            var pacjent = new XElement(
                new XElement("patient",
                new XElement("idp", Id),
                new XElement("imie", imie),
                new XElement("nazwisko", nazwisko),
                new XElement("pesel", pesel)));

            //TODO
            //foreach (kontrolka)
            foreach(var control in ListCustomField)
            {
                IEnumerable<XElement> fields = XElementon.Instance.Field.Fields();
                switch (control.DataContext.GetType().Name)
                {
                    case "CheckControlViewModel":
                        CheckControlViewModel checkModel = (CheckControlViewModel)control.DataContext;
                        pacjent.Add(new XElement(checkModel.FieldName, checkModel.CheckValue));
                        break;
                    case "TextControlViewModel":
                        TextControlViewModel textModel = (TextControlViewModel)control.DataContext;
                        pacjent.Add(new XElement(textModel.FieldName, textModel.TextValue));
                        break;
                }
                    

            }
            

            return pacjent;
        }

        private void Save(AddEditPatientWindow window)
        {

            if (NameFlag)
            {
                if (LastFlag)
                {
                    if (SelectedAttribute != "")
                    {
                        if (PesFlag)
                        {



                            Patient = CreatePatient();


                            if (SelectedAttribute != "")
                            {
                                Patient.Add(new XElement("storehouse", SelectedAttribute));
                            }

                            window.DialogResult = true;
                            window.Close();
                        }
                        else
                        {
                            MessageBox.Show("Pesel jest za krótki lub nie posiada liczb");
                        }
                    }

                    else
                    {
                        MessageBox.Show("Musisz wybrać jakiś magazyn");
                    }
                }
                else
                {
                    MessageBox.Show("Nazwisko jest za krótkie");
                }
            }
            else
            {
                MessageBox.Show("Imię jest za krótkie");
            }
        }

        private void Check()//rozszerz o listę customów nadpisywanie Patienta pomyślec czy to dobrze bangla TODOS
        {
            string Id = IDP;

            if (FirstName != "" && LastName != "" && Pesel != "")
            {
                if (!IsEnabled)
                {

                    Patient = CreatePatient();

                    SelectedAttribute = XElementon.Instance.CheckingRules(Patient, false)[0].Value;
                }
            }
            else
            {
                SelectedAttribute = "";
            }
        }

        private bool _NameFlag = false;
        public bool NameFlag
        {
            get
            {
                return _NameFlag;
            }
            set
            {
                _NameFlag = value;
                OnPropertyChanged("NameFlag");
            }
        }
        private bool _LastFlag = false;
        public bool LastFlag
        {
            get
            {
                return _LastFlag;
            }
            set
            {
                _LastFlag = value;
                OnPropertyChanged("LastFlag");
            }
        }
        private bool _PesFlag = false;
        public bool PesFlag
        {
            get
            {
                return _PesFlag;
            }
            set
            {
                _PesFlag = value;
                OnPropertyChanged("PesFlag");
            }

        }


        private void VerifyName(string propName)
        {

            switch(propName)
            {
                case "FirstName":
                    NameFlag = (FirstName.Length >= 1) ? true : false;
                    break;
                case "LastName":
                    LastFlag = (LastName.Length >= 1) ? true : false;
                    break;
                case "Pesel":
                    PesFlag = (System.Text.RegularExpressions.Regex.IsMatch(Pesel, "^\\d{11}$")) ? true : false;
                    break;
                default:
                    break;

            }

        }

        private void DeployFields(XElement customPatient)
        {

            IEnumerable<XElement> fields = XElementon.Instance.Field.Fields();
            foreach(var field in fields)
            {
                string fieldName = field.Element("fieldname").Value;
                if (customPatient != null)
                {
                    XElement customField = customPatient.Element(fieldName);
                    if (customField!= null)
                    DefineField(field, customField.Value);
                }
                else
                {
                    DefineField(field);
                }
            }
        }

        private void DefineField(XElement field , string fieldValue = "")
        {
            string fieldName = field.Element("fieldname").Value;
            if(fieldValue == "")
            {
                fieldValue = field.Element("fielddefault").Value;
            }
            switch (field.Element("fieldtype").Value)
            {
                case "bool":
                    CheckControl checkControl = new CheckControl(new CheckControlViewModel(fieldName, XmlConvert.ToBoolean(fieldValue)));
                    ListCustomField.Add(checkControl);
                    break;

                case "int":
                    TextControl textControl = new TextControl(new TextControlViewModel(fieldName, fieldValue));
                    ListCustomField.Add(textControl);
                    break;
            }
        }

    }
}
