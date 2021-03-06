﻿using MedicalLibrary.ViewModel.PagesViewModel;
using System;
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

namespace MedicalLibrary.View.Pages
{
    /// <summary>
    /// Interaction logic for MagazinePage.xaml
    /// </summary>
    public partial class MagazinePage : Page
    {
        public MagazinePage()
        {
            InitializeComponent();
            DataContext = new MagazinePageViewModel();
        }

        private void List_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }
    }
}
