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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CommonFrontEnd.View.DigitalClock
{
    /// <summary>
    /// Interaction logic for WhiteBackGroundWindow.xaml
    /// </summary>
    public partial class WhiteBackGroundWindow : Window
    {
        public WhiteBackGroundWindow()
        {
            InitializeComponent();
            Height = Application.Current.MainWindow.ActualHeight;
            Width = Application.Current.MainWindow.ActualWidth;
            MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;
            Closing += Window_Closing;
            this.Owner = Application.Current.MainWindow;
        }
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            Closing -= Window_Closing;
            e.Cancel = true;
            var anim = new DoubleAnimation(0, (Duration)TimeSpan.FromMilliseconds(500));
            anim.Completed += (s, _) => this.Close();
            this.BeginAnimation(UIElement.OpacityProperty, anim);
        }
    }
}
