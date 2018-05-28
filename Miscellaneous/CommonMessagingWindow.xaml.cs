using CommonFrontEnd.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace CommonFrontEnd.View
{
    /// <summary>
    /// Interaction logic for CommonMessagingWindow.xaml
    /// </summary>
    public partial class CommonMessagingWindow : Window
    {
        public CommonMessagingWindow()
        {
            InitializeComponent();
            DataContext = new CommonMessagingWindowVM();
            MouseLeftButtonDown += delegate { this.DragMove(); };
            Closing += delegate (object s, CancelEventArgs e) { e.Cancel = true; }; 
        }
#if TWS
        private void ApplyFilters(object sender, RoutedEventArgs e)
        {
            CommonMessagingWindow mWindow = System.Windows.Application.Current.Windows.OfType<CommonMessagingWindow>().FirstOrDefault();
            ((CommonMessagingWindowVM)mWindow.DataContext).viewSource.View.Refresh();
        }
#endif
    }
}
