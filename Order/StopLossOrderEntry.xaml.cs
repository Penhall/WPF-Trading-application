using CommonFrontEnd.Common;
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
    /// Interaction logic for StopLossOrderEntry.xaml
    /// </summary>
    public partial class StopLossOrderEntry : TitleBarHelperClass
    {
        public StopLossOrderEntry()
        {
            InitializeComponent();
            this.DataContext = new StopLossOrderVM();
            dataGrid.GotFocus += delegate (object sender, RoutedEventArgs e) { ((StopLossOrderVM)DataContext).AssignDataToForm(); };
            this.SourceInitialized += (x, y) =>
            {
                WindowExtensions.HideMinimizeAndMaximizeButtons(this);
            };
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);
            e.Cancel = true;
            this.Hide();
        }
    }
}
