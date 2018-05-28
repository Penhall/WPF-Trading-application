using CommonFrontEnd.Common;
using CommonFrontEnd.ViewModel.Trade;
using System.Windows;

namespace CommonFrontEnd.View.Trade
{
    /// <summary>
    /// Interaction logic for Net_Position___Details_ClientWise_.xaml
    /// </summary>
    public partial class NetPositionClientWiseDetails : TitleBarHelperClass
    {
        public NetPositionClientWiseDetails()
        {
            InitializeComponent();
#if TWS
            this.DataContext = new NetPositionCWSWDetailsVM();
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
