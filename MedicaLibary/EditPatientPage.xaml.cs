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
using System.Xml.Linq;

namespace MedicaLibary
{
    /// <summary>
    /// Interaction logic for EditPatient.xaml
    /// </summary>
    public partial class EditPatient : Page
    {
        XElement database = XElement.Load(Environment.CurrentDirectory + "\\lib.xml");
        IEnumerable<XElement> result;

        public EditPatient()
        {
            InitializeComponent();
        }
        
        private void onInput(object sender, RoutedEventArgs e)
        {
            var Id = ID.Text;
            result = from c in database.Descendants("patient")
                         select c;

                result = result
                    .Where(b => b.Elements("id")
                        .Any(f => (string)f == Id));

            var res = result.FirstOrDefault();
            if (res != null)
            {
                Imię.Text = res.Element("imie").Value;
                Nazwisko.Text = res.Element("nazwisko").Value;
                Pesel.Text = res.Element("pesel").Value;
            } else {
                Imię.Text = "";
                Nazwisko.Text = "";
                Pesel.Text = "";
            }
        }

        private void saveToXML(object sender, RoutedEventArgs e)
        {
            var Id = ID.Text;
            //string Id;
            var imie = Imię.Text;
            var nazwisko = Nazwisko.Text;
            var pesel = Pesel.Text;

            //XElement database = XElement.Load(Environment.CurrentDirectory + "\\lib.xml");




            if (Id != "" && imie != "" && nazwisko != "" && pesel != "")
            {
                result.First().Element("imie").SetValue(imie);
                result.First().Element("nazwisko").SetValue(nazwisko);
                result.First().Element("pesel").SetValue(pesel);
                database.Save(Environment.CurrentDirectory + "\\lib.xml");
                MessageBox.Show("Pomyślnie zedytowano pacjenta");
            }
            else
            {
                MessageBox.Show("Wszystkie pola są wymagane");
            }
        }

        private void Label_TextInput(object sender, TextCompositionEventArgs e)
        {

        }
    }
}


