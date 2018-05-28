using CommonFrontEnd.ViewModel.Session;
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

namespace CommonFrontEnd.View.Session
{
    /// <summary>
    /// Interaction logic for SessionBroadcast.xaml
    /// </summary>
    public partial class SessionBroadcast : Window
    {
        public SessionBroadcast()
        {
            InitializeComponent();
            this.DataContext = new SessionVM();
        }
    }
}
