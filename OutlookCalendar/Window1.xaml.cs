﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using OutlookCalendar.Model;

namespace OutlookCalendar
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        private Appointments appointments = new Appointments();

        public Window1()
        {
            InitializeComponent();

            DataContext = appointments;
        }

        private void Calendar_AddAppointment(object sender, RoutedEventArgs e)
        {
            Appointment appointment = new Appointment();
            appointment.Subject = "Subject?";
            appointment.StartTime = DateTime.Now;
            appointment.EndTime = DateTime.Now.AddMinutes(20);

            AddAppointmentWindow aaw = new AddAppointmentWindow();
            aaw.DataContext = appointment;
            aaw.ShowDialog();

            appointments.Add(appointment);

            cal.CurrentDate = DateTime.Now;
            

        }
    }
}
