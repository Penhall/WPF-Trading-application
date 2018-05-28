using CommonFrontEnd.ViewModel.Settings;
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

namespace CommonFrontEnd.View.Settings
{
    /// <summary>
    /// Interaction logic for BowSettings.xaml
    /// </summary>
    public partial class BowSettings : UserControl
    {
        public BowSettings()
        {
            InitializeComponent();
#if BOW

            this.DataContext = new BowSettingsVM();

#endif
        }

      
    }
}
