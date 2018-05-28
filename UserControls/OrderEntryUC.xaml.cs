using CommonFrontEnd.ViewModel.Order;
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

namespace CommonFrontEnd.View.UserControls
{
    /// <summary>
    /// Interaction logic for OrderEntryUC.xaml
    /// </summary>
    public partial class OrderEntryUC : UserControl
    {
        public OrderEntryUC()
        {
            InitializeComponent();
            this.DataContext = new OrderEntryUC_VM();
        }

    }
}
