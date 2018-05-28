using CommonFrontEnd.Common;
using CommonFrontEnd.Global;
using CommonFrontEnd.ViewModel.Trade;
using System.Windows;

namespace CommonFrontEnd.View.Trade
{
    /// <summary>
    /// Interaction logic for NetPositionClientWise.xaml
    /// </summary>
    public partial class NetPositionClientWise : TitleBarHelperClass
    {
        public bool IsWindowLoaded { get; set; }
        public NetPositionClientWise()
        {

            InitializeComponent();
#if TWS
            this.DataContext = new NetPositionClientWiseVM();
            this.Owner = Application.Current.MainWindow;
            IsWindowLoaded = true;
            this.SourceInitialized += (x, y) =>
            {
                WindowExtensions.HideMinimizeAndMaximizeButtons(this);
            };
            //TODO workaround
            if (!string.IsNullOrEmpty(UtilityLoginDetails.GETInstance.Role))
            {
                if (UtilityLoginDetails.GETInstance.Role.ToLower() == "admin")
                {
                    foreach (var item in dgClientWise.Columns)
                    {
                        if (item.Header.ToString() == "ClientType")
                        {
                            item.Visibility = System.Windows.Visibility.Collapsed;
                        }
                        else if (item.Header.ToString() == "Client")
                        {
                            item.Visibility = System.Windows.Visibility.Collapsed;
                        }
                        else if (item.Header.ToString() == "Trader ID")
                        {
                            item.Visibility = System.Windows.Visibility.Visible;
                        }
                    }
                }
                else if (UtilityLoginDetails.GETInstance.Role.ToLower() == "trader")
                {
                    foreach (var item in dgClientWise.Columns)
                    {
                        if (item.Header.ToString() == "ClientType")
                        {
                            item.Visibility = System.Windows.Visibility.Visible;
                        }
                        else if (item.Header.ToString() == "Client")
                        {
                            item.Visibility = System.Windows.Visibility.Visible;
                        }
                        else if (item.Header.ToString() == "Trader ID")
                        {
                            item.Visibility = System.Windows.Visibility.Collapsed;
                        }
                    }
                }
            }

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
