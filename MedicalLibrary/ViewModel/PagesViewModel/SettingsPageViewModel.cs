using MedicaLibrary.Model;
using MedicalLibrary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MedicalLibrary.ViewModel.PagesViewModel
{
    public class SettingsPageViewModel :BaseViewModel
    {
        public SettingsPageViewModel()
        {
            AddToDatabase = new RelayCommand(pars => Add());
            CleanModifications = new RelayCommand(pars => Clean());
            PullDatabase = new RelayCommand(pars => Pull());
        }

        private bool _RevertConfirmation = false;
        public bool RevertConfirmation
        {
            get
            {
                return _RevertConfirmation;
            }

            set
            {
                _RevertConfirmation = value;
                OnPropertyChanged("RevertConfirmation");
            }
        }

        public ICommand AddToDatabase { get; set; }
        public ICommand CleanModifications { get; set; }
        public ICommand PullDatabase { get; set; }

        private void Add()
        {
            XElementon.Instance.FillDatabase();
        }

        private void Clean()
        {
            XElementon.Instance.Modification.Clean();
        }

        private async void Pull()
        {
            var x = await PullREST.Pull();
            XElementon.Instance.setDatabase(x);
        }
    }
}
