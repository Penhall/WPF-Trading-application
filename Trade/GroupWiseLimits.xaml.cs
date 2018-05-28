using CommonFrontEnd.ViewModel.Trade;
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

namespace CommonFrontEnd.View.Trade
{
    /// <summary>
    /// Interaction logic for GroupWiseLimits.xaml
    /// </summary>
    public partial class GroupWiseLimits : Window
    {
        public GroupWiseLimits()
        {
            InitializeComponent();
            this.DataContext = new GroupWiseLimitsVM();
        }
    }
}
