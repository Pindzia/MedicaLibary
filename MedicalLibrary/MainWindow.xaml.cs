﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FirstFloor.ModernUI.Windows.Controls;
using MedicalLibrary.Model;
using FirstFloor.ModernUI.Presentation;
using MedicalLibrary.testFolder;
using MedicalLibrary.View.Windows;

namespace MedicalLibrary
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ModernWindow
    {
        public MainWindow()
        {
            PushREST.SetClient();
            EntryWindow window = new EntryWindow();
            Nullable<bool> result = window.ShowDialog();
            if(result == true && result!=null)
            {
                InitializeComponent();
                //XElementon.Instance.LoadRaw();
                //XElementon.Instance.LoadEncrypted();
                ContentSource = MenuLinkGroups.First().Links.First().Source;
                linkWrong = WrongPatient;
                linkMod = Modifications;
                linkRest = RestAPI;
                GlobalUpdate();
            }
            else
            {
                Close();
            }
            
        }
        public static Link linkWrong;
        public static Link linkMod;
        public static Link linkRest;

        public static void GlobalUpdate()
        {
            ChangeWrong();
            ChangeMod();
            ChangeRest();
        }

        private static void ChangeWrong()
        {
            linkWrong.DisplayName = "Źle Umieszczeni Pacjenci: " + XElementon.Instance.Patient.InWrongStorehouse().Count().ToString();
        }
        private static void ChangeMod()
        {
            linkMod.DisplayName = "Ilość Modyfikacji: " + XElementon.Instance.Modification.Modifications().Count().ToString();
        }
        private static void ChangeRest()
        {
            linkRest.DisplayName = "Zalogowany jako: " + XElementon.Instance.nazwaLekarz;
        }
    }
}
