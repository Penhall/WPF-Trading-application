using CommonFrontEnd.Common;
using CommonFrontEnd.ViewModel.Trade;
using System.Windows;

namespace CommonFrontEnd.View.Trade
{
    /// <summary>
    /// Interaction logic for NetPositionScripWise.xaml
    /// </summary>
    public partial class NetPositionScripWise : TitleBarHelperClass
    {
        public NetPositionScripWise()
        {
            InitializeComponent();
#if TWS
            this.DataContext = new NetPositionScripWiseVM();
            this.Owner = Application.Current.MainWindow;
            this.SourceInitialized += (x, y) =>
            {
                WindowExtensions.HideMinimizeAndMaximizeButtons(this);
            };
#endif
        }
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);
            e.Cancel = true;
            this.Hide();
        }
    }
}
