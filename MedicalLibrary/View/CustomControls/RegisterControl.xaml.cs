using MedicalLibrary.ViewModel.CustomControlsViewModel;
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

namespace MedicalLibrary.View.CustomControls
{
    /// <summary>
    /// Interaction logic for RegisterControl.xaml
    /// </summary>
    public partial class RegisterControl : UserControl
    {
        public RegisterControl()
        {
            InitializeComponent();
            DataContext = new RegisterControlViewModel();
        }

        private void PasswordBox_FirstPasswordChanged(object sender, RoutedEventArgs e)
        {
            if(DataContext != null)
            {
                ((dynamic)DataContext).Password = ((PasswordBox)sender).Password;
            }

        }

        private void PasswordBox_SecondPasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext != null)
            {
                ((dynamic)DataContext).SecondPassword = ((PasswordBox)sender).Password;
            }

        }
    }
}
