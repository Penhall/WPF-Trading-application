using CommonFrontEnd.Common;
using CommonFrontEnd.Global;
using CommonFrontEnd.ViewModel.Trade;

namespace CommonFrontEnd.View.Trade
{
    /// <summary>
    /// Interaction logic for Saudas_Admin.xaml
    /// </summary>
    public partial class Saudas_Admin : TitleBarHelperClass
    {
        public Saudas_Admin()
        {
            InitializeComponent();
#if TWS
            this.DataContext = new AdminTradeViewVM();
            this.SourceInitialized += (x, y) =>
            {
                WindowExtensions.HideMinimizeAndMaximizeButtons(this);
            };
            //TODO workaround
            if (!string.IsNullOrEmpty(UtilityLoginDetails.GETInstance.Role))
            {
                if (UtilityLoginDetails.GETInstance.Role.ToLower() == "admin")
                {
                    foreach (var item in dataGridView1.Columns)
                    {
                        if(item.Header.ToString() == "TraderID")
                        {
                            item.Visibility = System.Windows.Visibility.Visible;
                        }
                        else if(item.Header.ToString()== "Scrip Code")
                        {
                            item.Visibility = System.Windows.Visibility.Visible;
                        }
                        else if (item.Header.ToString() == "Scrip Id")
                        {
                            item.Visibility = System.Windows.Visibility.Collapsed;
                        }
                        else if (item.Header.ToString() == "LocationID")
                        {
                            item.Visibility = System.Windows.Visibility.Visible;
                        }
                        else if (item.Header.ToString() == "DateTime")
                        {
                            item.Visibility = System.Windows.Visibility.Visible;
                        }
                        else if (item.Header.ToString() == "Time")
                        {
                            item.Visibility = System.Windows.Visibility.Collapsed;
                        }

                    }
                }
                else if (UtilityLoginDetails.GETInstance.Role.ToLower() == "trader")
                {
                    foreach (var item in dataGridView1.Columns)
                    {
                        if (item.Header.ToString() == "TraderID")
                        {
                            item.Visibility = System.Windows.Visibility.Collapsed;
                        }
                        else if (item.Header.ToString() == "Scrip Code")
                        {
                            item.Visibility = System.Windows.Visibility.Collapsed;
                        }
                        else if (item.Header.ToString() == "Scrip Id")
                        {
                            item.Visibility = System.Windows.Visibility.Visible;
                        }
                        else if (item.Header.ToString() == "LocationID")
                        {
                            item.Visibility = System.Windows.Visibility.Collapsed;
                        }
                        else if (item.Header.ToString() == "DateTime")
                        {
                            item.Visibility = System.Windows.Visibility.Collapsed;
                        }
                        else if (item.Header.ToString() == "Time")
                        {
                            item.Visibility = System.Windows.Visibility.Visible;
                        }
                    }
                }
            }
            
            //if (Application.Current.MainWindow.IsLoaded)
            //    this.Owner = Application.Current.MainWindow;
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
