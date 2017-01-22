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

        public ICommand NavReg { get; set; }
        public ICommand LogIn { get; set; }

        private async void Login()
        {
            if(Username != null && Username !="" && _Password!=null && _Password!="")//zawrzeć warunek moze byc jakis dodatkowy
            {
                //logika zalogowania
                //haszowanie
                var pass = CryptoClass.Instance.GetStringSha256Hash(_Password);

                int idLekarz;
                if ((idLekarz = await PushREST.Login(Username, pass)) == 0)
                {
                    System.Windows.MessageBox.Show("Błędne hasło lub login!");
                    return;
                }
                else
                {

                    System.Windows.MessageBox.Show("Poprawne zalogowanie!!");
                    XElementon.Instance.idLekarz = idLekarz;
                    XElementon.Instance.nazwaLekarz = Username;
                    XElementon.Instance.Haslo = pass;

                    System.Windows.MessageBox.Show("Pobieranie danch w toku...");
                    var x = await PullREST.PullAll(XElementon.Instance.idLekarz, XElementon.Instance.Haslo);
                    if (x != null)
                    {
                        XElementon.Instance.setDatabase(x);
                        System.Windows.MessageBox.Show("Dane pobrane!");
                    } else
                    {
                        //ERROR!
                        System.Windows.MessageBox.Show("Błąd pobrania!");
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
