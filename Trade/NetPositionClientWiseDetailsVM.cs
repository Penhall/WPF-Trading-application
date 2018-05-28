using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdvancedTWS.SharedMemories;

namespace AdvancedTWS.ViewModel.Trade
{
    public class NetPositionClientWiseDetailsVM : INotifyPropertyChanged
    {
        public NetPositionClientWiseDetailsVM()
        {
           
        }

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
    }
}
