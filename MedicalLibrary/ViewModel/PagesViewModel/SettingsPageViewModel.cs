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
            AddSomeToDatabase = new RelayCommand(pars => AddSome());
            AddALotToDatabase = new RelayCommand(pars => AddALot());
            CleanModifications = new RelayCommand(pars => Clean());
            PushModifications = new RelayCommand(pars => Push());
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

        public ICommand AddSomeToDatabase { get; set; }
        public ICommand AddALotToDatabase { get; set; }
        public ICommand CleanModifications { get; set; }
        public ICommand PushModifications { get; set; }
        public ICommand PullDatabase { get; set; }



        bool toggle = true;
        private void AddSome()
        {

            //var x = XElementon.Instance.Visit.UniqueDates();
            //return;
            if (toggle)
            {
                XElementon.Instance.SetKey("test567890123456");
                XElementon.Instance.SaveEncrypted();
                toggle = false;
            }
            else
            {
                XElementon.Instance.SetKey("test567890123456");
                XElementon.Instance.LoadEncrypted();
                toggle = true;
            }

        }

        private void AddALot()
        {
            XElementon.Instance.FillDatabase();
        }

        private void Clean()
        {
            XElementon.Instance.Modification.Clean();
        }

        private void Push()
        {
            XElementon.Instance.SendModifications.SendAll(1); //TODO ID-lekarz
        }

        private async void Pull()
        {
            var x = await PullREST.Pull();
            XElementon.Instance.setDatabase(x);
        }
    }
}
