using MedicalLibrary.Model;
using MedicalLibrary.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MedicalLibrary.ViewModel.PagesViewModel
{
    public class ToolsPageViewModel :BaseViewModel
    {
        public ToolsPageViewModel()
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



        private void AddSome()
        {
            //Zapisz modyfikacje do pliku
            //XElementon.Instance.Modification.saveToFile();
            //Debug stuff :V
            XElementon.Instance.Patient.OutdatedPatients();
        }

        private void SaveEncrypted()
        {
            XElementon.Instance.SetKey(XElementon.Instance.Haslo);
            XElementon.Instance.SaveEncrypted();
        }

        private void LoadEncrypted()
        {
            XElementon.Instance.SetKey(XElementon.Instance.Haslo);
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

            XElementon.Instance.SendModifications.SendAll(XElementon.Instance.idLekarz, XElementon.Instance.Haslo); //TODO ID-lekarz TODO-pass
            
        }


        private async void Login(string nazwaLekarza, string pass)
        {

        }

        private async void Register(string nazwaLekarza, string pass)
        {

        }

        private async void Logout() //podpiąć do widoku?
        {
            XElementon.Instance.idLekarz = 0;
            XElementon.Instance.Haslo = "";
            XElementon.Instance.getDatabase();
            //Zmień zakładkę na pierwszą

            List<string> LoginyLekarzy = await PushREST.LekarzNazwyGET();
        }

        //

        private async void Pull()
        {

        }
    }
}
