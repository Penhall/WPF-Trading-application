using CommonFrontEnd.Common;
using CommonFrontEnd.Global;
using CommonFrontEnd.ViewModel.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
    /// Interaction logic for PendingOrderClassic.xaml
    /// </summary>
    public partial class PendingOrderClassic : TitleBarHelperClass
    {
        public PendingOrderClassic()
        {
            InitializeComponent();
            this.DataContext = PendingOrderClassicVM.GETInstance;
            this.Owner = Application.Current.MainWindow;
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
            BulkPriceChnage objBulkPriceChnage = Application.Current.Windows.OfType<View.BulkPriceChnage>().FirstOrDefault();
            objBulkPriceChnage?.Close();
            PendingOrderClassicVM.GETInstance.ClearAllFields();
        }
    }

    
}
