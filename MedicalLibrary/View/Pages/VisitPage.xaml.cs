using MedicalLibrary.ViewModel.PagesViewModel;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace MedicalLibrary.View.Pages
{
    /// <summary>
    /// Interaction logic for VisitPage.xaml
    /// </summary>
    public partial class VisitPage : Page
    {
        private VisitPageViewModel viewModel = new VisitPageViewModel();

        public VisitPage()
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        private void calendarButton_Loaded(object sender, EventArgs e)
        {
            CalendarDayButton button = (CalendarDayButton)sender;
            DateTime date = (DateTime)button.DataContext;
            HighlightDay(button, date);
            button.DataContextChanged += new DependencyPropertyChangedEventHandler(calendarButton_DataContextChanged);
        }

        private void HighlightDay(CalendarDayButton button, DateTime date)
        {
            var compare = new DateTime(viewModel.MyTime.Year, viewModel.MyTime.Month, viewModel.MyTime.Day);
            if (compare == date)
            {
                button.Background = Brushes.LightBlue;
            }
            else
            {
                if (viewModel.HighlightedDates.Contains(date))
                    button.Background = Brushes.LightSkyBlue;
                else
                    button.Background = Brushes.White;
            }

        }

        public static void UpdateCalendar()
        {

        }

        private void calendarButton_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            CalendarDayButton button = (CalendarDayButton)sender;
            DateTime date = (DateTime)button.DataContext;
            HighlightDay(button, date);
        }
    }
}
