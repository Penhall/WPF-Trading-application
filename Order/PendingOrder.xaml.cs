using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
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

namespace CommonFrontEnd.View.Order
{
    /// <summary>
    /// Interaction logic for PendingOrder.xaml
    /// </summary>
    public partial class PendingOrder : Window
    {
        public PendingOrder()
        {
            InitializeComponent();
            this.DataContext = new ViewModel.Order.PendingOrderVM();
            this.Owner = Application.Current.MainWindow;
            
        }
    }
}
