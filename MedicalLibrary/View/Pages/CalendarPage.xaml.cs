using MedicalLibrary.ViewModel.PagesViewModel;
using OutlookCalendar.Model;
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
    /// Interaction logic for CalendarPage.xaml
    /// </summary>
    public partial class CalendarPage : Page
    {
        private CalendarPageViewModel viewModel= new CalendarPageViewModel();

        public CalendarPage()
        {

            InitializeComponent();
            DataContext = viewModel;
            
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            cal.CurrentDate = DateTime.Now;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            viewModel.MyTime = viewModel.MyTime.AddDays(1);
        }
    }
}
