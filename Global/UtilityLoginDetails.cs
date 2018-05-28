using CommonFrontEnd;
using CommonFrontEnd.Common;
using CommonFrontEnd.View.DigitalClock;
using CommonFrontEnd.ViewModel;
using CommonFrontEnd.ViewModel.Touchline;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace CommonFrontEnd.Global
{
    [Serializable()]
    partial class UtilityLoginDetails
    {
        public int SensexIndexCode = 1;
        private string _senderLocationId;

        public string SenderLocationId
        {
            get { return _senderLocationId; }
            set { _senderLocationId = value; }
        }


        private string _FileName;
        public string FileName
        {
            get { return _FileName; }
            set { _FileName = value; }
        }

        private uint _TraderId;

        public uint TraderId
        {
            get { return mobjInstance._TraderId; }
            set { mobjInstance._TraderId = value; }
        }

        private string _NextBulletinPoint;

        public string NextBulletinPoint
        {
            get { return _NextBulletinPoint; }
            set { _NextBulletinPoint = value; }
        }

        private uint _RequestTraderId;

        public uint RequestTraderId
        {
            get { return _RequestTraderId; }
            set { _RequestTraderId = value; }
        }
        private string _DecryptedPassword;

        public string DecryptedPassword
        {
            get { return _DecryptedPassword; }
            set { _DecryptedPassword = value; }
        }
        private string _gstrTransactionPwd;

        public string GstrTransactionPwd
        {
            get
            {
                return _gstrTransactionPwd;
            }

            set
            {
                _gstrTransactionPwd = value;
            }
        }
        private string upTrendColorGlobal;
        public string UpTrendColorGlobal
        {
            get { return upTrendColorGlobal; }
            set { upTrendColorGlobal = value; }
        }

        private string downTrendColorGlobal;
        public string DownTrendColorGlobal
        {
            get { return downTrendColorGlobal; }
            set { downTrendColorGlobal = value; }
        }


        private Color upTrendHexa;
        public Color UpTrendHexa
        {
            get { return mobjInstance.upTrendHexa; }
            set { mobjInstance.upTrendHexa = value; }
        }

        private Color downTrendHexa;
        public Color DownTrendHexa
        {
            get { return mobjInstance.downTrendHexa; }
            set { mobjInstance.downTrendHexa = value; }
        }

        /// <summary>
        /// Populated on successful login
        /// </summary>
        private DateTime _TodaysDateTime;

        public DateTime TodaysDateTime
        {
            get { return _TodaysDateTime; }
            set { _TodaysDateTime = CustomDigitalWindow.TodaysDateTime = value; NotifyStaticPropertyChanged("TodaysDateTime"); }
        }

        private uint _MemberId;
        public uint MemberId
        {
            get { return mobjInstance._MemberId; }
            set { mobjInstance._MemberId = value; }
        }

        private int _MaxMessageTag;
        public int MaxMessageTag
        {
            get { return _MaxMessageTag; }
            set { _MaxMessageTag = value; }
        }

        private string _SelectedTouchLineScripID;
        public string SelectedTouchLineScripID
        {
            get { return _SelectedTouchLineScripID; }
            set { _SelectedTouchLineScripID = value; }
        }

        private long _SelectedTouchLineScripCode;

        public long SelectedTouchLineScripCode
        {
            get { return _SelectedTouchLineScripCode; }
            set { _SelectedTouchLineScripCode = value; }
        }

        private static UtilityLoginDetails mobjInstance;
        private UtilityLoginDetails()
        {
        }
        public static UtilityLoginDetails GETInstance
        {
            get
            {
                if (mobjInstance == null)
                {
                    mobjInstance = new UtilityLoginDetails();
                }
                return mobjInstance;
            }
        }

        #region StaticNotifyPropertyChangedEvent
        public static event EventHandler<PropertyChangedEventArgs> StaticPropertyChanged
                 = delegate { };
        private static void NotifyStaticPropertyChanged(string propertyName)
        {
            StaticPropertyChanged(null, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

    }




    partial class UtilityLoginDetails
    {
#if TWS

        #region "Properties Set During Login Details of TWS"


        //protected override void OnExit(ExitEventArgs e)
        //{
        //    //CommonFrontEnd.ExceptionUtility.LogException();
        //    //base.OnExit(e);
        //}




        private string _Role;

        public string Role
        {
            get { return _Role; }
            set { _Role = value; }
        }

        private bool _IsLoggedIN = false;

        public bool IsLoggedIN
        {
            get { return _IsLoggedIN; }
            set { _IsLoggedIN = value; }
        }


        private string _CurrentDirectory;

        public string CurrentDirectory
        {
            get
            {
                return (mobjInstance._CurrentDirectory = Environment.CurrentDirectory);
            }
        }


        private bool _IsCURloggedIn;

        public bool IsCURChecked
        {
            get { return _IsCURloggedIn; }
            set { _IsCURloggedIn = value; }
        }



        private bool _IsDERloggedIn;

        public bool IsDERChecked
        {
            get { return _IsDERloggedIn; }
            set { _IsDERloggedIn = value; }
        }


        private bool _IsEQXloggedIn;

        public bool IsEQXChecked
        {
            get { return _IsEQXloggedIn; }
            set { _IsEQXloggedIn = value; }
        }

        private bool _IsBcastEqChecked;

        public bool IsBcastEqChecked
        {
            get { return _IsBcastEqChecked; }
            set { _IsBcastEqChecked = value; }
        }

        private bool _IsBcastDerChecked;

        public bool IsBcasDerChecked
        {
            get { return _IsBcastDerChecked; }
            set { _IsBcastDerChecked = value; }
        }

        private bool _IsBcastCurChecked;

        public bool IsBcastCurChecked
        {
            get { return _IsBcastCurChecked; }
            set { _IsBcastCurChecked = value; }
        }
        private string _SettlementNo;

        public string SettlementNo
        {
            get { return _SettlementNo; }
            set { _SettlementNo = value; }
        }

        private bool _IsEqLogged;

        public bool IsEqConnected
        {
            get { return _IsEqLogged; }
            set { _IsEqLogged = value; }
        }

        private bool _isderlogged;

        public bool IsDerConnected
        {
            get { return _isderlogged; }
            set { _isderlogged = value; }
        }

        private bool iscurrlogged;

        public bool IsCurrConnected
        {
            get { return iscurrlogged; }
            set { iscurrlogged = value; }
        }

        private bool _PersonalReplyReceived;

        public bool PersonalReplyReceived
        {
            get { return _PersonalReplyReceived; }
            set { _PersonalReplyReceived = value; }
        }

        private bool _endofdownloadReceived;

        public bool EndofdownloadReceived
        {
            get { return _endofdownloadReceived; }
            set { _endofdownloadReceived = value; }
        }

        private int _SegmentIndex;

        public int SegmentIndex
        {
            get { return _SegmentIndex; }
            set { _SegmentIndex = value; }
        }

        private int _requestedMessage;

        public int RequestedMessage
        {
            get { return _requestedMessage; }
            set { _requestedMessage = value; }
        }
        /// <summary>
        /// Represents flag for live or DR connected.
        /// </summary>
        private ushort _isPeerConnected;

        public ushort IsPeerConnected
        {
            get { return _isPeerConnected; }
            set { _isPeerConnected = value; }
        }

        private bool _allowOnlineTradeProcessingAfterPD = false;

        public bool AllowOnlineTradeProcessingAfterPD
        {
            get { return _allowOnlineTradeProcessingAfterPD; }
            set { _allowOnlineTradeProcessingAfterPD = value; }
        }

        #endregion



        private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            string errorMessage = string.Format("An unhandled exception occurred: {0}", e.Exception.Message);
            ExceptionUtility.LogError(e.Exception);
            MessageBox.Show("Error Occured in the Application", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            e.Handled = true;
        }

#endif
    }




    partial class UtilityLoginDetails
    {
#if BOW
        #region "Properties Set During Login Details"
        private long mlngUserId;
        private string mstrUserType = "";
        private string mstrUserLoginId = "";
        //Private mstrUserBackOfficeId As String = ""
        private string mstrUserFirstName = "";
        private string mstrUserLastName = "";
        private bool mblnHasSubUsers;
        private int mintNumberOfSubUsers;
        private string[] mstrSubUserIds;
        private string[] mstrSubUserLoginIds;
        private string[] mstrSubUserBackOfficeIds;
        private string[] mstrDMABrokers;
        //: LoginId-FirstName-LastName
        //: BackOfficeId-FirstName-LastName
        private UserDetails[] mobjSubUserDetailsForSmartSearch;
        // : Set Default as 99 Just to prevent the Format Exceptions that take place in frmListMarketWatch
        private string mstrDefaultMWID = "99";
        private string mstrLoginKey = "";
        private bool mblnLoginDone;
        private bool mblnInstitutionalClient;
        private UserDetails[] mobjInstitutionalClients;
        private const string PARAMETER_LOGIN_KEY = "LoginKey";
       

        public MarketWatchCollection gobjMarketWatchColl;
        public MarketWatchCollection gobjIndexMarketWatchColl;
        public MarketWatchCollection gobjIndustrialMarketWatchColl; 





        public long UserId
        {
            get { return mlngUserId; }
            set { mlngUserId = value; }
        }
        public bool LoginDone
        {
            get { return mblnLoginDone; }
            set { mblnLoginDone = value; }
        }
        public string LoginKey
        {
            get { return PARAMETER_LOGIN_KEY; }
        }
        public string LoginKeyValue
        {
            get { return mstrLoginKey; }
            set { mstrLoginKey = value; }
        }
        public string UserType
        {
            get { return mstrUserType; }
            set { mstrUserType = value; }
        }
        public string UserLoginId
        {
            get { return mstrUserLoginId; }
            set { mstrUserLoginId = value; }
        }
        public string UserBackOfficeId
        {
            get { return mstrUserLoginId; }
            set { mstrUserLoginId = value; }
        }
        public string UserFirstName
        {
            get { return mstrUserFirstName; }
            set { mstrUserFirstName = value; }
        }
        public string UserLastName
        {
            get { return mstrUserLastName; }
            set { mstrUserLastName = value; }
        }
        public string DefaultMWID
        {
            get { return mstrDefaultMWID; }
            set
            {
                if (value == null || value.Trim().Length == 0)
                    value = "0";
                mstrDefaultMWID = value;
            }
        }
        public bool HasSubUsers
        {
            get { return mblnHasSubUsers; }
            set { mblnHasSubUsers = value; }
        }
        public int NumberOfSubUsers
        {
            get { return mintNumberOfSubUsers; }
            set { mintNumberOfSubUsers = value; }
        }
        public string[] SubUserIds
        {
            get { return mstrSubUserIds; }
            set { mstrSubUserIds = value; }
        }
        public string[] SubUserLoginIds
        {
            get { return mstrSubUserLoginIds; }
            set { mstrSubUserLoginIds = value; }
        }
        public string[] SubUserBackOfficeIds
        {
            get { return mstrSubUserBackOfficeIds; }
            set { mstrSubUserBackOfficeIds = value; }
        }
        public bool InstitutionalClient
        {
            get { return mblnInstitutionalClient; }
            set { mblnInstitutionalClient = value; }
        }
        public UserDetails[] InstitutionalClients
        {
            get { return mobjInstitutionalClients; }
            set { mobjInstitutionalClients = value; }
        }
        public UserDetails[] SubUserDetailsForSmartSearch
        {
            get { return mobjSubUserDetailsForSmartSearch; }
            set { mobjSubUserDetailsForSmartSearch = value; }
        }
        public string[] DMA_Brokers
        {
            get { return mstrDMABrokers; }
            set { mstrDMABrokers = value; }
        }
        #endregion
#endif
    }


}


