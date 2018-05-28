using CommonFrontEnd.ViewModel.Trade;
using System.Windows;

namespace CommonFrontEnd.View.Trade
{
    /// <summary>
    /// Interaction logic for SpinnerUC.xaml
    /// </summary>
    public partial class PersonalDownloadProgessWindow : Window
    {
        public PersonalDownloadProgessWindow()
        {
            InitializeComponent();
            this.DataContext = new PersonalDownloadProgressVM();

            if (Application.Current.MainWindow.IsLoaded)
            {
                this.Owner = Application.Current.MainWindow;
                this.Height = Application.Current.MainWindow.Height;
                this.Width = Application.Current.MainWindow.Width;
                MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
                MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;
            }
        }
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);
            e.Cancel = true;
            this.Hide();
        }
    }
}
