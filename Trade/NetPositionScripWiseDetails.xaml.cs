using CommonFrontEnd.Common;
using CommonFrontEnd.Global;
using CommonFrontEnd.ViewModel.Trade;
using System.Windows;

namespace CommonFrontEnd.View.Trade
{
    /// <summary>
    /// Interaction logic for Net_Position___Details_ScripWise_.xaml
    /// </summary>
    public partial class NetPositionScripWiseDetails : TitleBarHelperClass
    {
        
        public NetPositionScripWiseDetails()
        {
            InitializeComponent();
#if TWS
            this.DataContext = new NetPositionScripWiseDetailsVM();
            this.Owner = Application.Current.MainWindow;
            this.SourceInitialized += (x, y) =>
            {
                WindowExtensions.HideMinimizeAndMaximizeButtons(this);
            };
            if (!string.IsNullOrEmpty(UtilityLoginDetails.GETInstance.Role))
            {
                if (UtilityLoginDetails.GETInstance.Role.ToLower() == "admin")
                {
                    foreach (var item in dgScripwiseDetails.Columns)
                    {
                        if (item.Header.ToString() == "Client ID")
                        {
                            item.Visibility = System.Windows.Visibility.Visible;
                        }
                        else if (item.Header.ToString() == "Client")
                        {
                            item.Visibility = System.Windows.Visibility.Collapsed;
                        }
                        else if (item.Header.ToString() == "Client Type")
                        {
                            item.Visibility = System.Windows.Visibility.Collapsed;
                        }
                        

                    }
                }
                else if (UtilityLoginDetails.GETInstance.Role.ToLower() == "trader")
                {
                    foreach (var item in dgScripwiseDetails.Columns)
                    {
                        if (item.Header.ToString() == "Client ID")
                        {
                            item.Visibility = System.Windows.Visibility.Collapsed;
                        }
                        else if (item.Header.ToString() == "Client")
                        {
                            item.Visibility = System.Windows.Visibility.Visible;
                        }
                        else if (item.Header.ToString() == "Client Type")
                        {
                            item.Visibility = System.Windows.Visibility.Visible;
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
