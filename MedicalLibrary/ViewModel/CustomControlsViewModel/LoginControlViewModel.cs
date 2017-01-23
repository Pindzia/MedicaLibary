using MedicalLibrary.Model;
using MedicalLibrary.View.Windows;
using MedicalLibrary.ViewModel.PagesViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MedicalLibrary.ViewModel.CustomControlsViewModel
{
    public class LoginControlViewModel :BaseViewModel
    {
        public LoginControlViewModel()
        {
            NavReg = new RelayCommandNoGlobal(pars => Navigate());
            LogIn = new RelayCommandNoGlobal(pars => Login());
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
                OnPropertyChanged(nameof(Username));
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
                OnPropertyChanged(nameof(Password));
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

        public ICommand NavReg { get; set; }
        public ICommand LogIn { get; set; }

        async Task PutTaskDelay()
        {
            await Task.Delay(2000);
        }

        private async void Login()
        {
            if(Username != null && Username !="" && _Password!=null && _Password!="")//zawrzeć warunek moze byc jakis dodatkowy
            {
                //logika zalogowania
                //haszowanie
                IsEnabled = false;
                IsActive = true;
                LoginMessage = "Logowanie w toku...";
                var pass = CryptoClass.Instance.GetStringSha256Hash(_Password);
                int idLekarz;
                if ((idLekarz = await PushREST.Login(Username, pass)) == 0)
                {
                    LoginMessage = "Błędne hasło lub login!";
                    IsEnabled = true;
                    IsActive = false;
                    return;
                }
                else
                {
                    DateTime timer = DateTime.Now.AddSeconds(2);
                    LoginMessage = "Poprawne zalogowanie!!";
                    await PutTaskDelay();
                    XElementon.Instance.idLekarz = idLekarz;
                    XElementon.Instance.nazwaLekarz = Username;
                    XElementon.Instance.Haslo = pass;

                    LoginMessage = "Pobieranie danch w toku...";
                    var x = await PullREST.PullAll(XElementon.Instance.idLekarz, XElementon.Instance.Haslo);
                    if (x != null)
                    {
                        XElementon.Instance.setDatabase(x);
                        IsActive = false;
                        LoginMessage = "Dane pobrane!";
                        DateTime secondtimer = DateTime.Now.AddSeconds(2);
                        await PutTaskDelay();
                    } else
                    {
                        //ERROR!
                        IsActive = false;
                        IsEnabled = true;
                        LoginMessage ="Błąd pobrania!";
                        return;
                    }
                }

                EntryWindow.Complete();
            }
        }

        private void Navigate()
        {
            EntryWindow.NavigateTo("Register");
        }

    }
}
