using CommonFrontEnd.Common;
using CommonFrontEnd.ViewModel.Order;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CommonFrontEnd.View.Order
{
    /// <summary>
    /// Interaction logic for SwiftOrderEntry.xaml
    /// </summary>
    public partial class SwiftOrderEntry : TitleBarHelperClass
    {
        public SwiftOrderEntry()
        {
            InitializeComponent();
            this.DataContext = new OrderEntryVM();
            this.Owner = Application.Current.MainWindow;

        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);
            e.Cancel = true;
            this.Hide();
        }
        //private void OnCloseExecuted(object sender, ExecutedRoutedEventArgs e)
        //{
        //    this.Close();
        //}


    }
}
