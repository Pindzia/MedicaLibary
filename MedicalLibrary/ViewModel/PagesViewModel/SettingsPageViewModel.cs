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
            SaveEncryptedDatabase = new RelayCommand(pars => SaveEncrypted());
            LoadEncryptedDatabase = new RelayCommand(pars => LoadEncrypted());
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
        public ICommand SaveEncryptedDatabase { get; set; }
        public ICommand LoadEncryptedDatabase { get; set; }
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
                
                XElementon.Instance.SaveEncrypted();
                toggle = false;
            }
            else
            {
                
                XElementon.Instance.LoadEncrypted();
                toggle = true;
            }
        }

        private void SaveEncrypted()
        {
            XElementon.Instance.SetKey("test567890123456");
            XElementon.Instance.SaveEncrypted();
        }

        private void LoadEncrypted()
        {
            XElementon.Instance.SetKey("test567890123456");
            XElementon.Instance.LoadEncrypted();
        }

        private void AddALot()
        {
            XElementon.Instance.FillDatabase();
        }

        private void Clean()
        {
            while (XElementon.Instance.Modification.Modifications().Any())
            {
                var a = XElementon.Instance.Modification.Modifications().First();
                XElementon.Instance.Modification.RevertX((int)a.Element("idm"));
            }
            //XElementon.Instance.Modification.Clean();
        }

        private void Push()
        {
            System.Windows.MessageBox.Show("Wysyłanie modyfikacji na serwer w toku...");
            XElementon.Instance.SendModifications.SendAll(1, "string"); //TODO ID-lekarz TODO-pass
            
        }

        private async void Pull()
        {
            System.Windows.MessageBox.Show("Pobieranie danch w toku...");
            var x = await PullREST.Pull(1, "string"); //TODO ID-lekarz TODO-pass;
            if(x != null)
            {
                XElementon.Instance.setDatabase(x);
                System.Windows.MessageBox.Show("Dane pobrane!");
            } else
            {
                System.Windows.MessageBox.Show("Błędne hasło!");
            }
            
        }
    }
}
