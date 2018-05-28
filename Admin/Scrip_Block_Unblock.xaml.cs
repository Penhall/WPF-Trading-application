using CommonFrontEnd.ViewModel.Admin;
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

namespace CommonFrontEnd.View.Admin
{
    /// <summary>
    /// Interaction logic for Scrip_Block_Unblock.xaml
    /// </summary>
    public partial class Scrip_Block_Unblock : Window
    {
        public Scrip_Block_Unblock()
        {
            InitializeComponent();
            this.DataContext = new ScripBlockUnblockVM();
        }
    }
}
