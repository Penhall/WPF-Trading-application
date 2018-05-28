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
using System.Windows.Shapes;

namespace CommonFrontEnd.View
{
    /// <summary>
    /// Interaction logic for BulkPriceChnage.xaml
    /// </summary>
    public partial class BulkPriceChnage : TitleBarHelperClass
    {
        public BulkPriceChnage()
        {
            InitializeComponent();
            this.DataContext = BulkPriceChnageVM.GETInstance;
            this.Owner = Application.Current.MainWindow;
            
            this.SourceInitialized += (x, y) =>
            {
                WindowExtensions.HideMinimizeAndMaximizeButtons(this);
            };
        }
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            //BulkPriceChnage oBulkPriceChnage = System.Windows.Application.Current.Windows.OfType<BulkPriceChnage>().FirstOrDefault();
            //if (oBulkPriceChnage != null)
            //{
            //    oBulkPriceChnage.Hide();
            //    ViewModel.Order.PendingOrderClassicVM.bulkChangeWindowOpen = false;
            //    ViewModel.Order.PendingOrderClassicVM.GETInstance.ShowPendingWindowButtons();
            //}
            //View.MainWindow objPendingWindow = System.Windows.Application.Current.Windows.OfType<View.MainWindow>().FirstOrDefault();
            ViewModel.Order.PendingOrderClassicVM.bulkChangeWindowOpen = false;
            ViewModel.Order.PendingOrderClassicVM.GETInstance.ShowPendingWindowButtons();
            base.OnClosing(e);
            e.Cancel = true;
            this.Hide();
        }

        
    }
}
