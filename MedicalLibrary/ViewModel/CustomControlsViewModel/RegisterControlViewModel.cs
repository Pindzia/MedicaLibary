using MedicalLibrary.Model;
using MedicalLibrary.View.Windows;
using MedicalLibrary.ViewModel.PagesViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MedicalLibrary.ViewModel.CustomControlsViewModel
{
    public class RegisterControlViewModel: BaseViewModel
    {

        public RegisterControlViewModel()
        {
            Register = new RelayCommandNoGlobal(pars => RegNew());
            NavLog = new RelayCommandNoGlobal(pars => Navigate());
            listaLekarzy();
        }

        private async void listaLekarzy()
        {
            _ListUser = await PushREST.LekarzNazwyGET();
        }


        private string _Username = "";
        public string Username
        {
            get
            {
                return _Username;
            }
            set
            {
                _Username = value;
                Check();
                OnPropertyChanged(nameof(Username));
            }
        }

        private List<string> _ListUser = new List<string>();//tutaj init nazw unikalnych
        public List<string> ListUser
        {
             private get
            {
                return _ListUser;
            }
            set
            {
                _ListUser = value;
                OnPropertyChanged(nameof(ListUser));
            }
        }

        private string _Password = "";
        public string Password
        {
            private get
            {
                return _Password;
            }

            set
            {
                _Password = value;
                Check();
                OnPropertyChanged(nameof(Password));
            }
        }

        private string _SecondPassword = "";
        public string SecondPassword
        {
            private get
            {
                return _SecondPassword;
            }

            set
            {
                _SecondPassword = value;
                Check();
                OnPropertyChanged(nameof(SecondPassword));
            }
        }

        private bool _UsernameFlag = false;
        public bool UsernameFlag
        {
            get
            {
                return _UsernameFlag;
            }
            set
            {
                _UsernameFlag = value;
                OnPropertyChanged(nameof(UsernameFlag));
            }
        }

        private bool _EmptyFlag = false;
        public bool EmptyFlag
        {
            get
            {
                return _EmptyFlag;
            }
            set
            {
                _EmptyFlag = value;
                OnPropertyChanged(nameof(EmptyFlag));
            }
        }

        private bool _LenghtFlag = false;
        public bool LenghtFlag
        {
            get
            {
                return _LenghtFlag;
            }
            set
            {
                _LenghtFlag = value;
                OnPropertyChanged(nameof(LenghtFlag));
            }
        }

        private bool _SpecCharFlag = false;
        public bool SpecCharFlag
        {
            get
            {
                return _SpecCharFlag;
            }
            set
            {
                _SpecCharFlag = value;
                OnPropertyChanged(nameof(SpecCharFlag));
            }
        }

        private bool _SameFlag = false;
        public bool SameFlag
        {
            get
            {
                return _SameFlag;
            }
            set
            {
                _SameFlag = value;
                OnPropertyChanged(nameof(SameFlag));
            }
        }

        public ICommand Register { get; set; }
        public ICommand NavLog { get; set; }


        private async void RegNew() //async!
        {

            if(_ListUser == null)
            {
                System.Windows.MessageBox.Show("_ListUser pusty?!");
                return;
            }

            //place do spinania
            if (UsernameFlag && SameFlag && EmptyFlag && LenghtFlag && SpecCharFlag)
            {
                //haszowanie
                var pass = CryptoClass.Instance.GetStringSha256Hash(_Password);
                int idLekarz;
                if ((idLekarz = await PushREST.Rejestracja(_Username, pass)) == 0) //Co zwraca rejestracja?
                {
                    System.Windows.MessageBox.Show("Błąd Rejestracji!");
                    return; // Close app?
                }
                else
                {
                    XElementon.Instance.idLekarz = idLekarz;
                    XElementon.Instance.nazwaLekarz = _Username;
                    XElementon.Instance.Haslo = pass;
                    await PullREST.PullAll(XElementon.Instance.idLekarz, XElementon.Instance.Haslo);
                    EntryWindow.Complete();
                }
                //Rejestracja
                
            }
        }
        private void Navigate()
        {
            EntryWindow.NavigateTo("Login");
        }

        private void Check()
        {
            UsernameFlag = (ListUser.Contains(Username)) ? false : true;
            SameFlag = (_Password == _SecondPassword && _Password !="" && SecondPassword!="") ? true : false;
            EmptyFlag = (Username != null && Password != null && SecondPassword != null && Username != "" && Password != "" && SecondPassword != "") ? true : false;
            LenghtFlag = (Password.Length >=7 && Password.Length <=16) ? true : false;
            Regex regex = new Regex("(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9]).*");
            SpecCharFlag = (regex.IsMatch(SecondPassword)) ? true : false;
        }

    }
}
