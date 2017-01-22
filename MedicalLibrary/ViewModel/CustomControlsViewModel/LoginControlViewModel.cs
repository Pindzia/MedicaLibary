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

        private void Login()
        {
            if(Username != null && Username !="" && _Password!=null && _Password!="")//zawrzeć warunek moze byc jakis dodatkowy
            {
                //logika zalogowania
                EntryWindow.Complete();
            }
        }

        private void Navigate()
        {
            EntryWindow.NavigateTo("Register");
        }

    }
}
