using CommonFrontEnd.Common;
using CommonFrontEnd.Model;
using CommonFrontEnd.View;
using SubscribeList;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonFrontEnd.ViewModel
{
   public  class RBIReferanceRateVM : INotifyPropertyChanged
    {
        #region Local Memory
        
        private static ObservableCollection<RBIReferanceRateModel> _ObjRBIReferenceRateCollection = new ObservableCollection<RBIReferanceRateModel>();

        public static ObservableCollection<RBIReferanceRateModel> ObjRBIReferenceRateCollection
        {
            get { return _ObjRBIReferenceRateCollection; }
            set { _ObjRBIReferenceRateCollection = value; /*NotifyStaticPropertyChanged(nameof(ObjIndicesCollection));*/ }
        }
        #endregion
        #region NotifyProperty
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

        #region RelayCommond
        private RelayCommand _ShortCut_Escape;

        public RelayCommand ShortCut_Escape
        {
            get
            {
                return _ShortCut_Escape ?? (_ShortCut_Escape = new RelayCommand(
                    (object e) => EscapeUsingUserControl(e)
                        ));
            }
        }
        
        #endregion
        public RBIReferanceRateVM()
        {
            ObjRBIReferenceRateCollection = new ObservableCollection<RBIReferanceRateModel>();
        }
        #region Methods
        private void EscapeUsingUserControl(object e)
        {
            RBIReferanceRate ORBIReferanceRate = e as RBIReferanceRate;
            ORBIReferanceRate?.Hide();
        }
        #endregion
    }
}
