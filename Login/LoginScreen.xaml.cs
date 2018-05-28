using CommonFrontEnd.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace CommonFrontEnd.View.Login
{
    /// <summary>
    /// Interaction logic for LoginScreen.xaml
    /// </summary>
    public partial class LoginScreen : Window
    {
        public LoginScreen()
        {
            InitializeComponent();
#if TWS
            this.DataContext = LoginScreenVM.GETInstanceLogin;
#endif
            this.Owner = Application.Current.MainWindow;
            comboBoxLogSegTws.SelectionChanged += delegate (object sender, System.Windows.Controls.SelectionChangedEventArgs e)
            {
                ComboBox comboBox = (ComboBox)sender;
                comboBox.SelectedItem = null;
            };
        }
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);
            e.Cancel = true;
            this.Hide();
        }
    }
}
