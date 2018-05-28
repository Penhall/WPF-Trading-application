using CommonFrontEnd.Global;
using CommonFrontEnd.ViewModel;
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
using System.Windows.Shapes;

namespace CommonFrontEnd.View.DigitalClock
{
    /// <summary>
    /// Interaction logic for CustomLockScreen.xaml
    /// </summary>
    public partial class CustomLockScreen : Window
    {
        public CustomLockScreen()
        {
            InitializeComponent();
            Owner = System.Windows.Application.Current.MainWindow;
            KeyDown += CustomLockScreen_KeyDown;
            secondmessage.Text = $"The WorkStation can only be Unlocked by user {UtilityLoginDetails.GETInstance.MemberId + "/" + UtilityLoginDetails.GETInstance.TraderId}";
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
        }

        private void CustomLockScreen_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.System && e.SystemKey == Key.F4)
                e.Handled = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            if (LockScreePassword.Password.Trim().ToUpper() == UtilityLoginDetails.GETInstance.DecryptedPassword.Trim().ToUpper())
            {
                WhiteBackGroundWindow owhiteBackground = Application.Current.Windows.OfType<WhiteBackGroundWindow>().FirstOrDefault();
                owhiteBackground?.Close();
                Close();
                Application.Current.MainWindow.Focus();
            }

            else
                MessageBox.Show("Invalid Password", "Warning!", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
