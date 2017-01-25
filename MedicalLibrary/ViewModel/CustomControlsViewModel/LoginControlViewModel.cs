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
    public class LoginControlViewModel : BaseViewModel
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
        private async void Login()
        {
            if (Username != null && Username != "" && _Password != null && _Password != "")//zawrzeć warunek moze byc jakis dodatkowy
            {
                //logika zalogowania
                //haszowanie

                LoginMessage = "Logowanie w toku...";
                TurnProgress();
                var pass = CryptoClass.Instance.GetStringSha256Hash(_Password);
                int idLekarz = 0;
                if ((idLekarz = await PushREST.Login(Username, pass)) == 0)
                {
                    LoginMessage = "Błędne hasło lub login!";
                    TurnOffProgress();
                    return;
                }
                else
                {
                    LoginMessage = "Poprawne zalogowanie!";
                    await PutTaskDelay();
                    XElementon.Instance.idLekarz = idLekarz;
                    XElementon.Instance.nazwaLekarz = Username;
                    XElementon.Instance.Haslo = pass;

                    LoginMessage = "Pobieranie danch w toku...";

                    if ((XElementon.Instance.numerWersji = await PushREST.MaxWersja(idLekarz, pass)) == 0)
                    {
                        LoginMessage = "Błąd w pobieraniu maksymalnej wersji!";
                        TurnOffProgress();
                        return;
                    }


                    var x = await PullREST.PullAll(XElementon.Instance.idLekarz, XElementon.Instance.Haslo);
                    if (x != null)
                    {
                        XElementon.Instance.setDatabase(x);
                        LoginMessage = "Dane pobrane! Uruchamianie aplikacji...";
                        await PutTaskDelay();
                    }
                    else
                    {
                        //ERROR!
                        TurnOffProgress();
                        LoginMessage = "Błąd pobrania!";
                        return;
                    }
                }
                LoginMessage = "";
                EntryWindow.Complete();
            }
            else
            {
                LoginMessage = "Uzupełnij dane logowania";
                await PutTaskDelay();
                LoginMessage = "";
            }
        }

        private void Navigate()
        {
            EntryWindow.NavigateTo("Register");
        }

    }
}