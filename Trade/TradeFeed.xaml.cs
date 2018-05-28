using CommonFrontEnd.Common;
using CommonFrontEnd.ViewModel.Trade;
using System.Windows;

namespace CommonFrontEnd.View.Trade
{
    /// <summary>
    /// Interaction logic for TradeFeed.xaml
    /// </summary>
    public partial class TradeFeed : TitleBarHelperClass
    {
        public TradeFeed()
        {
            InitializeComponent();
#if TWS
            this.DataContext = TradeFeedVM.GetInstance;
#endif
            this.Owner = Application.Current.MainWindow;
        }
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);
            e.Cancel = true;
            this.Hide();
        }

    }
}
