using CommonFrontEnd.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonFrontEnd.Global
{
    public class UtilityOrderDetails
    {

        #region Normal OE ShortClientID

        
        private string _EqtShortClientID;

        public string EqtShortClientID
        {
            get { return _EqtShortClientID; }
            set { _EqtShortClientID = value; }
        }


        private string _DervShortClientID;

        public string DervShortClientID
        {
            get { return _DervShortClientID; }
            set { _DervShortClientID = value; }
        }


        private string _CurrShortClientID;

        public string CurrShortClientID
        {
            get { return _CurrShortClientID; }
            set { _CurrShortClientID = value; }
        }


        private string _DebtShortClientID;

        public string DebtShortClientID
        {
            get { return _DebtShortClientID; }
            set { _DebtShortClientID = value; }
        }


        private string _IsEqtShortClientIDChecked;

        public string IsEqtShortClientIDChecked
        {
            get { return _IsEqtShortClientIDChecked; }
            set { _IsEqtShortClientIDChecked = value; }
        }


        private string _IsDervShortClientIDChecked;

        public string IsDervShortClientIDChecked
        {
            get { return _IsDervShortClientIDChecked; }
            set { _IsDervShortClientIDChecked = value; }
        }


        private string _IsCurrShortClientIDChecked;

        public string IsCurrShortClientIDChecked
        {
            get { return _IsCurrShortClientIDChecked; }
            set { _IsCurrShortClientIDChecked = value; }
        }


        private string _IsDebtShortClientIDChecked;

        public string IsDebtShortClientIDChecked
        {
            get { return _IsDebtShortClientIDChecked; }
            set { _IsDebtShortClientIDChecked = value; }
        }

        private string _EQClientType;

        public string EQClientType
        {
            get { return _EQClientType; }
            set { _EQClientType = value; }
        }

        private string _DERClientType;

        public string DERClientType
        {
            get { return _DERClientType; }
            set { _DERClientType = value; }
        }

        private string _DEBTClientType;

        public string DEBTClientType
        {
            get { return _DEBTClientType; }
            set { _DEBTClientType = value; }
        }

        private string _CURClientType;

        public string CURClientType
        {
            get { return _CURClientType; }
            set { _CURClientType = value; }
        }
        #endregion

        private string _stopLossOEReplyText;

        public string StopLossOEReplyText
        {
            get { return _stopLossOEReplyText; }
            set { _stopLossOEReplyText = value; }
        }

        private string _normalLossOEReplyText;

        public string NormalOEReplyText
        {
            get { return _normalLossOEReplyText; }
            set { _normalLossOEReplyText = value; }
        }

        private string _swiftOEReplyText;

        public string SwiftOEReplyText
        {
            get { return _swiftOEReplyText; }
            set { _swiftOEReplyText = value; }
        }

        private string _defaultOrderEntry;

        public string DefaultOrderEntry
        {
            get { return _defaultOrderEntry; }
            set { _defaultOrderEntry = value; }
        }

        private string _CurrentOrderEntry;

        public string CurrentOrderEntry
        {
            get { return _CurrentOrderEntry; }
            set { _CurrentOrderEntry = value; }
        }

        private long _GlobalScripSelectedCode;

        public long GlobalScripSelectedCode
        {
            get { return _GlobalScripSelectedCode; }
            set { _GlobalScripSelectedCode = value; }
        }


        private long _GlobalScripSelectedCodeDerv;

        public long GlobalScripSelectedCodeDerv
        {
            get { return _GlobalScripSelectedCodeDerv; }
            set { _GlobalScripSelectedCodeDerv = value; }
        }


        private string _mktProtection;

        public string MktProtection
        {
            get { return _mktProtection; }
            set { _mktProtection= value; }
        }

        private string _RevlQtyPercentage;

        public string RevlQtyPercentage
        {
            get { return _RevlQtyPercentage; }
            set { _RevlQtyPercentage = value; }
        }


        private string _EQTYMaxQty;

        public string EQTYMaxQty
        {
            get { return _EQTYMaxQty; }
            set { _EQTYMaxQty = value; }
        }


        private string _EQTYMinQty;

        public string EQTYMinQty
        {
            get { return _EQTYMinQty; }
            set { _EQTYMinQty = value; }
        }


        private string _DERVMaxQty;

        public string DERVMaxQty
        {
            get { return _DERVMaxQty; }
            set { _DERVMaxQty = value; }
        }


        private string _DERVMinQty;

        public string DERVMinQty
        {
            get { return _DERVMinQty; }
            set { _DERVMinQty = value; }
        }


        private string _CURRVMaxQty;

        public string CURRMaxQty
        {
            get { return _CURRVMaxQty; }
            set { _CURRVMaxQty = value; }
        }


        private string _CURRMinQty;

        public string CURRMinQty
        {
            get { return _CURRMinQty; }
            set { _CURRMinQty = value; }
        }


        private string _EQTYMaxOrderValue;

        public string EQTYMaxOrderValue
        {
            get { return _EQTYMaxOrderValue; }
            set { _EQTYMaxOrderValue = value; }
        }


        private string _DERVMaxOrderValue;

        public string DERVMaxOrderValue
        {
            get { return _DERVMaxOrderValue; }
            set { _DERVMaxOrderValue = value; }
        }


        private string _CURRMaxOrderValue;

        public string CURRMaxOrderValue
        {
            get { return _CURRMaxOrderValue; }
            set { _CURRMaxOrderValue = value; }
        }

        private string _defaultFocusOESettings;

        public string DefaultFocusOESettings
        {
            get { return _defaultFocusOESettings; }
            set { _defaultFocusOESettings = value; }
        }
        private Char _clientIdAllowed = 'A';
        public Char clientIdAllowed
        {
            get { return _clientIdAllowed; }
            set { _clientIdAllowed = value; NotifyPropertyChanged("clientIdAllowed"); }
        }

        private bool _Default5LChecked;

        public bool Default5LChecked
        {
            get { return _Default5LChecked; }
            set { _Default5LChecked = value; }
        }
        
        private string _SelectedTouchlineData;//default

        public string SelectedTouchlineData
        {
            get { return _SelectedTouchlineData; }
            set { _SelectedTouchlineData = value; }
        }

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

        public readonly long MAXQTYLIMIT = 1000000000;

        private static UtilityOrderDetails mobjInstance;
        private UtilityOrderDetails()
        {
        }
        public static UtilityOrderDetails GETInstance
        {
            get
            {
                if (mobjInstance == null)
                {
                    mobjInstance = new UtilityOrderDetails();
                }
                return mobjInstance;
            }
        }
    }
}
