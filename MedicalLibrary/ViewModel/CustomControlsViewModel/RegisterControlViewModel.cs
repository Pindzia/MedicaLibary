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

        private bool _IsActive = false;
        public bool IsActive
        {
            private get
            {
                return _IsActive;
            }

            set
            {
                _IsActive = value;
                OnPropertyChanged(nameof(IsActive));
            }
        }

        private bool _IsEnabled = true;
        public bool IsEnabled
        {
            private get
            {
                return _IsEnabled;
            }

            set
            {
                _IsEnabled = value;
                OnPropertyChanged(nameof(IsEnabled));
            }
        }

        private string _LoginMessage = "";
        public string LoginMessage
        {
            get
            {
                return _LoginMessage;
            }

            set
            {
                _LoginMessage = value;
                OnPropertyChanged(nameof(LoginMessage));
            }
        }

        public ICommand Register { get; set; }
        public ICommand NavLog { get; set; }


        async Task PutTaskDelay()
        {
            await Task.Delay(2000);
        }

        private void TurnProgress()
        {
            IsEnabled = false;
            IsActive = true;
        }

        private void TurnOffProgress()
        {
            IsEnabled = true;
            IsActive = false;
        }
        private void Finish()
        {
            IsEnabled = false;
            IsActive = false;
        }

        private async void RegNew() //async!
        {

            if(_ListUser.Count == 0)
            {
                LoginMessage = "Nie pobrano jeszcze listy nazw Lekarzy?!";
                await PutTaskDelay();
                LoginMessage = "";
                return;
            }

            //place do spinania
            if (UsernameFlag && SameFlag && EmptyFlag && LenghtFlag && SpecCharFlag)
            {
                //haszowanie
                TurnProgress();
                LoginMessage = "Rejestracja w toku...";
                var pass = CryptoClass.Instance.GetStringSha256Hash(_Password);
                int idLekarz;

                if ((idLekarz = await PushREST.Rejestracja(_Username, pass)) == 0) //Co zwraca rejestracja?
                {
                    LoginMessage = "Błąd Rejestracji!";
                    TurnOffProgress();
                    return; // Close app?
                }
                else
                {
                    LoginMessage = "Zarejestrowano Pomyślnie!!";
                    Finish();
                    XElementon.Instance.idLekarz = idLekarz;
                    XElementon.Instance.nazwaLekarz = _Username;
                    XElementon.Instance.Haslo = pass;
                    await PullREST.PullAll(XElementon.Instance.idLekarz, XElementon.Instance.Haslo);
                    await PutTaskDelay();
                    EntryWindow.Complete();
                }
                //Rejestracja
                
            }
            else
            {
                LoginMessage = "Warunki rejestracji nie zostały spełnione";
                await PutTaskDelay();
                LoginMessage = "";
                return;
            }
        }
        private void Navigate()
        {
            EntryWindow.NavigateTo("Login");
        }

        private void Check()
        {
            Regex usernameRegex = new Regex("[a-zA-Z0-9_ ]{3,20}");
            UsernameFlag = (ListUser.Contains(Username) || !usernameRegex.IsMatch(Username)) ? false : true;
            SameFlag = (_Password == _SecondPassword && _Password !="" && SecondPassword!="") ? true : false;
            EmptyFlag = (Username != null && Password != null && SecondPassword != null && Username != "" && Password != "" && SecondPassword != "") ? true : false;
            LenghtFlag = (Password.Length >=7 && Password.Length <=16) ? true : false;
            Regex regex = new Regex("(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9]).*");
            SpecCharFlag = (regex.IsMatch(SecondPassword)) ? true : false;
        }

    }
}
