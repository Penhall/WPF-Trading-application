using CommonFrontEnd.ViewModel;
using System.Windows;

namespace CommonFrontEnd.View
{
    /// <summary>
    /// Interaction logic for LoginScreen.xaml
    /// </summary>
    public partial class LoginScreen : Window
    {
        public LoginScreen()
        {
            InitializeComponent();
            this.DataContext = new LoginScreenVM();
        }
    }
}
