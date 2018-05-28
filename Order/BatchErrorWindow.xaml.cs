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
using System.Windows.Shapes;

namespace CommonFrontEnd.View.Order
{
    /// <summary>
    /// Interaction logic for BatchErrorWindow.xaml
    /// </summary>
    public partial class BatchErrorWindow : Window
    {
        public BatchErrorWindow()
        {
            InitializeComponent();
            this.DataContext = new BatchErrorWindowVM();
        }
    }
}
