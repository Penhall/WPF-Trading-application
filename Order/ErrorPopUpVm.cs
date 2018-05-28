using CommonFrontEnd.Common;
using CommonFrontEnd.View.Order;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace CommonFrontEnd.ViewModel.Order
{
    class ErrorPopUpVm
    {
        
        #region properties
        private static string _msg;
        public static string msg
        {
            get { return _msg; }
            set
            {
                _msg = value;
                NotifyStaticPropertyChanged("msg");

            }
        }
        #endregion
        #region Constructor
        public ErrorPopUpVm()
        {
           
           
          
           //var workingArea = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea;
           //var transform = PresentationSource.FromVisual(Application.Current.Windows.OfType<SwiftOrderEntry>().FirstOrDefault()).CompositionTarget.TransformFromDevice;
           // var corner = transform.Transform(new Point(workingArea.Right, workingArea.Top));
           

        }

       
        #endregion
      
        #region NotifyPropertyChange
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String propertyName = "")
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                this.PropertyChanged(this, e);
            }
        }
        #endregion
        #region StaticNotifyPropertyChangedEvent
        public static event EventHandler<PropertyChangedEventArgs> StaticPropertyChanged
                 = delegate { };
        private static void NotifyStaticPropertyChanged(string propertyName)
        {
            StaticPropertyChanged(null, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
