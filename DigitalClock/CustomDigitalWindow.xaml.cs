using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace CommonFrontEnd.View.DigitalClock
{
    /// <summary>
    /// Interaction logic for CustomDigitalWindow.xaml
    /// </summary>
    public partial class CustomDigitalWindow : Window, INotifyPropertyChanged
    {

        private static DateTime _TodaysDateTime;

        public static DateTime TodaysDateTime
        {
            get { return _TodaysDateTime; }
            set { _TodaysDateTime = value; NotifyStaticPropertyChanged("TodaysDateTime"); }
        }

        public CustomDigitalWindow()
        {
            InitializeComponent();
            this.Owner = Application.Current.MainWindow;
            this.DataContext = this;
            Closing += delegate (object s, CancelEventArgs e) { e.Cancel = true; };
            Focusable = false;
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        #region NotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        public static event EventHandler<PropertyChangedEventArgs> StaticPropertyChanged
                = delegate { };
        private static void NotifyStaticPropertyChanged(string propertyName)
        {
            StaticPropertyChanged(null, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

    }
}
