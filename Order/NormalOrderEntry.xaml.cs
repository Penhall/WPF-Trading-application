using CommonFrontEnd.Common;
using CommonFrontEnd.ViewModel.Order;
using System;
using System.Collections.Generic;
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

namespace CommonFrontEnd.View.Order
{
    /// <summary>
    /// Interaction logic for NormalOrderEntry.xaml
    /// </summary>
    public partial class NormalOrderEntry : TitleBarHelperClass
    {
        public NormalOrderEntry()
        {
            InitializeComponent();

            this.DataContext = NormalOrderEntryVM.GetInstance;
            this.Owner = Application.Current.MainWindow;
        }
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);
            e.Cancel = true;
            this.Hide();
        }

        private void TitleBarHelperClass_Loaded(object sender, RoutedEventArgs e)
        {
            ((TextBox)drpdwnShortClient.Template.FindName("PART_EditableTextBox", drpdwnShortClient)).CharacterCasing = CharacterCasing.Upper;
            textBox2.CharacterCasing = CharacterCasing.Upper;
            txtMessage.CharacterCasing = CharacterCasing.Upper;

            foreach (TextBlock tb in FindVisualChildren<TextBlock>(this))
            {
                // do something with tb here
                if (tb.Name == "txtMarque")
                {
                    DoubleAnimation doubleAnimation = new DoubleAnimation();
                    doubleAnimation.From = 400;// this.ActualWidth;
                    doubleAnimation.To =tb.ActualWidth;
                    doubleAnimation.RepeatBehavior = RepeatBehavior.Forever;
                    doubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(10)); // provide an appropriate  duration 
                    tb.BeginAnimation(Canvas.LeftProperty, doubleAnimation);
                }
            }

        }
        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

    }
}
