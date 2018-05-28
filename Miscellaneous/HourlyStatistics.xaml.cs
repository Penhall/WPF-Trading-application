using CommonFrontEnd.Common;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CommonFrontEnd.View
{
    /// <summary>
    /// Interaction logic for HourlyStatistics.xaml
    /// </summary>
    public partial class HourlyStatistics : TitleBarHelperClass

    {
        public HourlyStatistics()
        {
            InitializeComponent();
            this.DataContext =new  HourlyStatisticsVM();
        }
    }
}
