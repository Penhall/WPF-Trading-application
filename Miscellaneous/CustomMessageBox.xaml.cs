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
using CommonFrontEnd.ViewModel;

namespace CommonFrontEnd.View
{
    /// <summary>
    /// Interaction logic for CustomMessageBox.xaml
    /// </summary>
    public partial class CustomMessageBox : Window
    {
        public CustomMessageBox()
        {
            InitializeComponent();
            this.DataContext = CustomMessageBoxVM.GetInstance;
            this.Owner = Application.Current.MainWindow;
            this.Closing += delegate (object sender, System.ComponentModel.CancelEventArgs e) { this.Hide(); e.Cancel = true; };
        }
    }
}
