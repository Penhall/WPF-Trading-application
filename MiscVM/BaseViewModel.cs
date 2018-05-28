using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonFrontEnd.ViewModel
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Notifying Property Change event
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(String propertyName = "")
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                this.PropertyChanged(this, e);
            }
        }

        /// <summary>
        /// Notifying Static Property Change event
        /// </summary>
        public static event EventHandler<PropertyChangedEventArgs> StaticPropertyChanged
                = delegate { };
        public static void NotifyStaticPropertyChanged(string propertyName)
        {
            StaticPropertyChanged(null, new PropertyChangedEventArgs(propertyName));
        }

    }
}
