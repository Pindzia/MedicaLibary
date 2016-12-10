using MedicalLibrary.ViewModel.PagesViewModel;
using System.Windows.Controls;

namespace MedicalLibrary.View.Pages
{
    /// <summary>
    /// Interaction logic for VisitPage.xaml
    /// </summary>
    public partial class VisitPage : Page
    {
        public VisitPage()
        {
            InitializeComponent();
            this.DataContext = new VisitPageViewModel();
        }
    }
}
