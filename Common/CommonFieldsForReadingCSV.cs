using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonFrontEnd.Common
{
    public class CommonFieldsForReadingCSV : INotifyPropertyChanged
    {

        private int _index;

        public int index
        {
            get { return _index; }
            set { _index = value; NotifyPropertyChanged(nameof(index)); }
        }

        private string _Exchange;

        public string Exchange
        {
            get { return _Exchange; }
            set { _Exchange = value; NotifyPropertyChanged(nameof(Exchange)); }
        }

        private string _Segment;

        public string Segment
        {
            get { return _Segment; }
            set { _Segment = value; NotifyPropertyChanged(nameof(Segment)); }
        }


        private string _Reply;

        public string Reply
        {
            get { return _Reply; }
            set { _Reply = value; NotifyPropertyChanged(nameof(Reply)); }
        }


        private string _ScripId;

        public string ScripId
        {
            get { return _ScripId; }
            set { _ScripId = value; NotifyPropertyChanged(nameof(ScripId)); }
        }

        private long _ScripCode;

        public long ScripCode
        {
            get { return _ScripCode; }
            set { _ScripCode = value; NotifyPropertyChanged(nameof(ScripCode)); }
        }

        private string _ScripName;

        public string ScripName
        {
            get { return _ScripName; }
            set { _ScripName = value; NotifyPropertyChanged(nameof(ScripName)); }
        }

        private string _BSFlag;

        public string BSFlag
        {
            get { return _BSFlag; }
            set { _BSFlag = value; NotifyPropertyChanged(nameof(BSFlag)); }
        }

        private int _TotQty;

        public int TotQty
        {
            get { return _TotQty; }
            set { _TotQty = value; NotifyPropertyChanged(nameof(TotQty)); }
        }

        private int _RevQty;

        public int RevQty
        {
            get { return _RevQty; }
            set { _RevQty = value; NotifyPropertyChanged(nameof(RevQty)); }
        }

        private long _Rate;

        public long Rate
        {
            get { return _Rate; }
            set { _Rate = value; NotifyPropertyChanged(nameof(Rate)); }
        }

        private string _ClientID;

        public string ClientID
        {
            get { return _ClientID; }
            set { _ClientID = value; NotifyPropertyChanged(nameof(ClientID)); }
        }

        private string _Retention;

        public string Retention
        {
            get { return _Retention; }
            set { _Retention = value; NotifyPropertyChanged(nameof(Retention)); }
        }

        private string _ClientType;

        public string ClientType
        {
            get { return _ClientType; }
            set { _ClientType = value; NotifyPropertyChanged(nameof(ClientType)); }
        }

        private string _OrderType;

        public string OrderType
        {
            get { return _OrderType; }
            set { _OrderType = value; NotifyPropertyChanged(nameof(OrderType)); }
        }

        private string _CPCode;

        public string CPCode
        {
            get { return _CPCode; }
            set { _CPCode = value; NotifyPropertyChanged(nameof(CPCode)); }
        }

        private long _TrgRate;

        public long TrgRate
        {
            get { return _TrgRate; }
            set { _TrgRate = value; NotifyPropertyChanged(nameof(TrgRate)); }
        }

        #region Notify Properties
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
