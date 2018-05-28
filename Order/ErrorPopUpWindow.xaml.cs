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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using CommonFrontEnd.Global;
using System.ComponentModel;
using CommonFrontEnd.ViewModel.Order;

namespace CommonFrontEnd.View.Order
{
    /// <summary>
    /// Interaction logic for ErrorPopUpWindow.xaml
    /// </summary>
    public partial class ErrorPopUpWindow : Window
    {
       
      

        public ErrorPopUpWindow()
        {
            InitializeComponent();
            this.DataContext = new ErrorPopUpVm();
          
            //Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, new Action(() =>
            //{
            //    var workingArea = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea;
            //    var transform = PresentationSource.FromVisual(this).CompositionTarget.TransformFromDevice;
            //    var corner = transform.Transform(new Point(workingArea.Right, workingArea.Bottom));
            //    //to place it at bottom right corner
            //    // this.Left = corner.X - this.ActualWidth - 100;
            //    //this.Top = corner.Y - this.ActualHeight;
               
            //}));
        }
        private void DoubleAnimationCompleted(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsMouseOver)
                {
                    this.Close();
                }
            }
            catch(Exception ex)
            {

            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }

}

