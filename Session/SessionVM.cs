using CommonFrontEnd.Common;
using CommonFrontEnd.Model.Session;
using CommonFrontEnd.View.Session;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonFrontEnd.ViewModel.Session
{
  public  class SessionVM : INotifyPropertyChanged
    {
        #region Memmories
        private  ObservableCollection<string> _ExchangeList = new ObservableCollection<string>();

        public  ObservableCollection<string> ExchangeList
        {
            get { return _ExchangeList; }
            set { _ExchangeList = value; NotifyPropertyChanged(nameof(ExchangeList)); }
        }

        private static ObservableCollection<SessionModel> _ObjSessionBroadCastCollection;

        public static ObservableCollection<SessionModel> ObjSessionBroadCastCollection
        {
            get { return _ObjSessionBroadCastCollection; }
            set
            {
                _ObjSessionBroadCastCollection = value;// NotifyPropertyChanged(nameof(ObjSessionBroadCastCollection)); }
            }
        }
        #endregion

        #region Properties
        private string _SelectedExchange;

        public string SelectedExchange
        {
            get { return _SelectedExchange; }
            set { _SelectedExchange = value; NotifyPropertyChanged(nameof(SelectedExchange)); }
        }


        #endregion
        #region relayCommand
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

        #region Constructor
        public SessionVM()
        {
            populateExchange();
        }
        #endregion

        #region Methods

        private void EscapeUsingUserControl(object e)
        {
            SessionBroadcast oSessionBroadcast = e as SessionBroadcast;
            oSessionBroadcast.Hide();
        }
        private void populateExchange()
        {
            

            ExchangeList.Add(Common.Enumerations.Exchange.BSE.ToString());
#if BOW
            ExchangeList.Add(Common.Enumerations.Exchange.NSE.ToString());
            ExchangeList.Add(Common.Enumerations.Exchange.NCDEX.ToString());
            ExchangeList.Add(Common.Enumerations.Exchange.MCX.ToString());
#endif
            SelectedExchange = Common.Enumerations.Exchange.BSE.ToString();
        }
        #endregion

        #region NotifyProperty
        public event PropertyChangedEventHandler PropertyChanged;
        private  void NotifyPropertyChanged(String propertyName = "")
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                this.PropertyChanged(this, e);
            }
        }
        #endregion
    }
}
