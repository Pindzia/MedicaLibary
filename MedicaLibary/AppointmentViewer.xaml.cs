﻿using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for AppointmentViewer.xaml
    /// </summary>
    public partial class AppointmentViewer : Page
    {
        public AppointmentViewer()
        {
            InitializeComponent();
            XElement TrackList = XElement.Load(Environment.CurrentDirectory + "\\visits.xml");
            DataGrid.DataContext = TrackList;
            DataGrid.AutoGenerateColumns = false;
        }
    }
}
