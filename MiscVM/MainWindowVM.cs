using BroadcastReceiver;
using CommonFrontEnd.Common;
using CommonFrontEnd.Controller;
using CommonFrontEnd.CSVReader;
using CommonFrontEnd.Global;
using CommonFrontEnd.Model;
using CommonFrontEnd.Model.Session;
using CommonFrontEnd.Model.Trade;
using CommonFrontEnd.Processor;
using CommonFrontEnd.Processor.Trade;
using CommonFrontEnd.SharedMemories;
using CommonFrontEnd.View;
using CommonFrontEnd.View.Admin;
using CommonFrontEnd.View.Login;
using CommonFrontEnd.View.Order;
using CommonFrontEnd.View.Session;
using CommonFrontEnd.View.Settings;
using CommonFrontEnd.View.Touchline;
using CommonFrontEnd.View.Trade;
using CommonFrontEnd.ViewModel.Login;
using CommonFrontEnd.ViewModel.Order;
using CommonFrontEnd.ViewModel.Session;
using CommonFrontEnd.ViewModel.Touchline;
using SubscribeList;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows;
using static CommonFrontEnd.Common.Enumerations;
using System.Windows.Input;
using CommonFrontEnd.ViewModel.PersonalDownload;
using CommonFrontEnd.ViewModel.Settings;
using CommonFrontEnd.View.BSEBulletin;
using System.Windows.Media.Imaging;
using CommonFrontEnd.ViewModel.Profiling;
using CommonFrontEnd.View.DigitalClock;
using System.Windows.Controls;

namespace CommonFrontEnd.ViewModel
{

    public sealed partial class MainWindowVM
    {
        public static int counter = 0;
        public static HotKey _hotKey { get; set; }
        static bool Load_CorporateAction = false;
        private static ReadFromCSV ObjCommon = new ReadFromCSV();
        internal static SynchronizationContext uiContext = SynchronizationContext.Current;
        private static bool InvokeTrade = false;
        public static LogOnStatus[] objLogOnStatus;
        public static string ShortCutKeysFlag;

        static bool isfirstnews = true;
        static int PreviousSessionNumber = -1;
        static int previousSessionPCAS = -1;
        static int PreviousMarketType = -1;
        double IndexValue = 0;
        double PrvClose = 0;
        double TrendDiff = 0;

        VarPercentageBroadcast objVarPercentageBroadcast = Application.Current.Windows.OfType<VarPercentageBroadcast>().FirstOrDefault();
        static MainWindow mwindow = null;
        private static MainWindowVM _getinstance;

        public static MainWindowVM GetInstance
        {
            get
            {
                if (_getinstance == null && counter == 0)
                {
                    counter++;
                    _getinstance = new MainWindowVM();
                }
                return _getinstance;
            }

        }


        #region Properties

        private static string _MenuVisibilityBeforeLogin;
        public static string MenuVisibilityBeforeLogin
        {
            get
            {
                return _MenuVisibilityBeforeLogin;
            }

            set
            {
                _MenuVisibilityBeforeLogin = value;
                NotifyStaticPropertyChanged("MenuVisibilityBeforeLogin");

            }
        }

        private static string _MenuVisibilityAfterLogin;
        public static string MenuVisibilityAfterLogin
        {
            get
            {
                return _MenuVisibilityAfterLogin;
            }

            set
            {
                _MenuVisibilityAfterLogin = value;
                NotifyStaticPropertyChanged("MenuVisibilityAfterLogin");

            }
        }

        private static string _sensexMargin;

        public static string sensexMargin
        {
            get { return _sensexMargin; }
            set { _sensexMargin = value; NotifyStaticPropertyChanged(nameof(sensexMargin)); }
        }

        private static string _VisibileToBow;
        public static string VisibileToBow
        {
            get
            {
                return _VisibileToBow;
            }

            set
            {
                _VisibileToBow = value;
                NotifyStaticPropertyChanged("VisibileToBow");
            }
        }
        public static Action<Model.IdicesDetailsMain> indicesBroadCast;
        public static Action<Model.VarPercentageBroadcastModel> varPercentageBroadcastModel;

        private static string _FixedSensexTxtBlock = "S&P BSE SENSEX";

        public static string FixedSensexTxtBlock
        {
            get { return _FixedSensexTxtBlock; }
            set { _FixedSensexTxtBlock = value; NotifyStaticPropertyChanged(nameof(FixedSensexTxtBlock)); }
        }

        private static string _CurrentValueTxtBlock;

        public static string CurrentValueTxtBlock
        {
            get { return _CurrentValueTxtBlock; }
            set { _CurrentValueTxtBlock = value; NotifyStaticPropertyChanged(nameof(CurrentValueTxtBlock)); }
        }

        private static string _ChangeInValueTxtBlock;

        public static string ChangeInValueTxtBlock
        {
            get { return _ChangeInValueTxtBlock; }
            set { _ChangeInValueTxtBlock = value; NotifyStaticPropertyChanged(nameof(ChangeInValueTxtBlock)); }
        }

        private string _imgTrend;

        public string imgTrend
        {
            get { return _imgTrend; }
            set { _imgTrend = value; NotifyStaticPropertyChanged(nameof(imgTrend)); }
        }

        #endregion

        #region ToolTipProperties
        private static string _ttLogin;

        public static string ttLogin
        {
            get { return _ttLogin; }
            set { _ttLogin = value; NotifyStaticPropertyChanged(nameof(ttLogin)); }
        }

        private static string _ttOrderEntryBuy;

        public static string ttOrderEntryBuy
        {
            get { return _ttOrderEntryBuy; }
            set { _ttOrderEntryBuy = value; NotifyStaticPropertyChanged(nameof(ttOrderEntryBuy)); }
        }

        private static string _ttOrderEntrySell;

        public static string ttOrderEntrySell
        {
            get { return _ttOrderEntrySell; }
            set { _ttOrderEntrySell = value; NotifyStaticPropertyChanged(nameof(ttOrderEntrySell)); }
        }

        private static string _ttPendingOrder;

        public static string ttPendingOrder
        {
            get { return _ttPendingOrder; }
            set { _ttPendingOrder = value; NotifyStaticPropertyChanged(nameof(ttPendingOrder)); }
        }

        private static string _ttTouchline;

        public static string ttTouchline
        {
            get { return _ttTouchline; }
            set { _ttTouchline = value; NotifyStaticPropertyChanged(nameof(ttTouchline)); }
        }

        private static string _ttReturnedOrder;

        public static string ttReturnedOrder
        {
            get { return _ttReturnedOrder; }
            set { _ttReturnedOrder = value; NotifyStaticPropertyChanged(nameof(ttReturnedOrder)); }
        }

        private static string _ttSaudas;

        public static string ttSaudas
        {
            get { return _ttSaudas; }
            set { _ttSaudas = value; NotifyStaticPropertyChanged(nameof(ttSaudas)); }
        }

        private static string _ttNews;

        public static string ttNews
        {
            get { return _ttNews; }
            set { _ttNews = value; NotifyStaticPropertyChanged(nameof(ttNews)); }
        }

        private static string _OrderEntryShortKeyBuy;

        public static string OrderEntryShortKeyBuy
        {
            get { return _OrderEntryShortKeyBuy; }
            set { _OrderEntryShortKeyBuy = value; NotifyStaticPropertyChanged(nameof(OrderEntryShortKeyBuy)); }
        }

        private static string _ttOrderEntryShortKeyBuy;

        public static string ttOrderEntryShortKeyBuy
        {
            get { return _ttOrderEntryShortKeyBuy; }
            set { _ttOrderEntryShortKeyBuy = value; NotifyStaticPropertyChanged(nameof(ttOrderEntryShortKeyBuy)); }
        }

        private static string _OrderEntryShortKeySell;

        public static string OrderEntryShortKeySell
        {
            get { return _OrderEntryShortKeySell; }
            set { _OrderEntryShortKeySell = value; NotifyStaticPropertyChanged(nameof(OrderEntryShortKeySell)); }
        }

        private static string _PendingOrderHeader;

        public static string PendingOrderHeader
        {
            get { return _PendingOrderHeader; }
            set { _PendingOrderHeader = value; NotifyStaticPropertyChanged(nameof(PendingOrderHeader)); }
        }


        private static string _ttOrderEntryShortKeySell;

        public static string ttOrderEntryShortKeySell
        {
            get { return _ttOrderEntryShortKeySell; }
            set { _ttOrderEntryShortKeySell = value; NotifyStaticPropertyChanged(nameof(ttOrderEntryShortKeySell)); }
        }

        private static string _ttIndexDetails;

        public static string ttIndexDetails
        {
            get { return _ttIndexDetails; }
            set { _ttIndexDetails = value; NotifyStaticPropertyChanged(nameof(ttIndexDetails)); }
        }

        private static string _ttNetposScripWise;

        public static string ttNetposScripWise
        {
            get { return _ttNetposScripWise; }
            set { _ttNetposScripWise = value; NotifyStaticPropertyChanged(nameof(ttNetposScripWise)); }
        }

        private static string _ttNetposClientWise;

        public static string ttNetposClientWise
        {
            get { return _ttNetposClientWise; }
            set { _ttNetposClientWise = value; NotifyStaticPropertyChanged(nameof(ttNetposClientWise)); }
        }

        private static string _ttSessionBroadcast;

        public static string ttSessionBroadcast
        {
            get { return _ttSessionBroadcast; }
            set { _ttSessionBroadcast = value; NotifyStaticPropertyChanged(nameof(ttSessionBroadcast)); }
        }

        private static string _ttScripProfiling;

        public static string ttScripProfiling
        {
            get { return _ttScripProfiling; }
            set { _ttScripProfiling = value; NotifyStaticPropertyChanged(nameof(ttScripProfiling)); }
        }

        private static string _ttScripHelp;

        public static string ttScripHelp
        {
            get { return _ttScripHelp; }
            set { _ttScripHelp = value; NotifyStaticPropertyChanged(nameof(ttScripHelp)); }
        }

        private static string _ttTradeLimits;

        public static string ttTradeLimits
        {
            get { return _ttTradeLimits; }
            set { _ttTradeLimits = value; NotifyStaticPropertyChanged(nameof(ttTradeLimits)); }
        }

        private static string _ttOpenInterest;

        public static string ttOpenInterest
        {
            get { return _ttOpenInterest; }
            set { _ttOpenInterest = value; NotifyStaticPropertyChanged(nameof(ttOpenInterest)); }
        }


        private static string _ttMarketReports;

        public static string ttMarketReports
        {
            get { return _ttMarketReports; }
            set { _ttMarketReports = value; NotifyStaticPropertyChanged(nameof(ttMarketReports)); }
        }

        private static string _ttWebApps;

        public static string ttWebApps
        {
            get { return _ttWebApps; }
            set { _ttWebApps = value; NotifyStaticPropertyChanged(nameof(ttWebApps)); }
        }

        private static string _ttVARPercetage;

        public static string ttVARPercetage
        {
            get { return _ttVARPercetage; }
            set { _ttVARPercetage = value; NotifyStaticPropertyChanged(nameof(ttVARPercetage)); }
        }

        private static string _ttCorporateAction;

        public static string ttCorporateAction
        {
            get { return _ttCorporateAction; }
            set { _ttCorporateAction = value; NotifyStaticPropertyChanged(nameof(ttCorporateAction)); }
        }

        #endregion
        #region RelayCommand
        //private static ObservableCollection<AllIndicesWindow> _ObjMinIndicesCollection;

        //public static ObservableCollection<AllIndicesWindow> ObjMinIndicesCollection
        //{
        //    get { return _ObjMinIndicesCollection; }
        //    set { _ObjMinIndicesCollection = value; /*NotifyStaticPropertyChanged(nameof(ObjMinIndicesCollection));*/ }
        //}

        private RelayCommand _OrderEntryBuyWindow;

        public RelayCommand OrderEntryBuyWindow
        {
            // get { return _OrderEntryBuyWindow; }
            get
            {
                return _OrderEntryBuyWindow ?? (_OrderEntryBuyWindow = new RelayCommand((object e) => OrderEntryBuyWindow_Click("Buy")));
            }
        }

        private RelayCommand _OrderEntrySellWindow;

        public RelayCommand OrderEntrySellWindow
        {
            // get { return _OrderEntryBuyWindow; }
            get
            {
                return _OrderEntrySellWindow ?? (_OrderEntrySellWindow = new RelayCommand((object e) => OrderEntrySellWindow_Click("Sell")));
            }
        }

        private RelayCommand _LoginScreenMenuClick;

        public RelayCommand LoginScreenMenuClick
        {
            get
            {
                return _LoginScreenMenuClick ?? (_LoginScreenMenuClick = new RelayCommand((object e) => LoginScreenClick(null)));
            }
        }
        private RelayCommand _Webapps;
        public RelayCommand Webapps

        {
            get
            {
                return _Webapps ?? (_Webapps = new RelayCommand(
                    (object e) => OpenWebappsWindow(null)
                        ));
            }
        }

        public void LoginScreenClick(HotKey _hotKey)
        {//   if (ByPassLogin == "1")

            LoginScreen oLoginScreen = System.Windows.Application.Current.Windows.OfType<LoginScreen>().FirstOrDefault();
            if (oLoginScreen != null)
            {
                if (oLoginScreen.WindowState == WindowState.Minimized)
                    oLoginScreen.WindowState = WindowState.Normal;
                oLoginScreen.Focus();
                //oLoginScreen.Activate();
                oLoginScreen.Show();
            }
            else
            {
                oLoginScreen = new LoginScreen();
                oLoginScreen.Owner = System.Windows.Application.Current.MainWindow;
                oLoginScreen.Activate();
                oLoginScreen.Show();

            }

        }

        private static RelayCommand _menuitemTouchline;

        public static RelayCommand MenuTouchline
        {
            get
            {
                return _menuitemTouchline ?? (_menuitemTouchline = new RelayCommand(
                    (object e) => MenuTouchlineClick(null)
                        ));
            }

        }
        private RelayCommand _OpenInterest;

        public RelayCommand OpenInterest
        {
            get
            {
                return _OpenInterest ?? (_OpenInterest = new RelayCommand(
                    (object e) => OpenInterestClick(null)
                        ));
            }

        }

        private RelayCommand _VARPercetage;

        public RelayCommand VARPercetage
        {
            get
            {
                return _VARPercetage ?? (_VARPercetage = new RelayCommand(
                    (object e) => OpenVARPercetageWindow(null)
                        ));
            }
        }


        private RelayCommand _SessionBroadcast;

        public RelayCommand SessionBroadcast
        {
            get
            {
                return _SessionBroadcast ?? (_SessionBroadcast = new RelayCommand((object e) => OpenSessionBroadcastWindow(null)));

            }
        }

        private RelayCommand _PendingOrder;

        public RelayCommand PendingOrder
        {
            get
            {
                return _PendingOrder ?? (_PendingOrder = new RelayCommand(
                    (object e) => PendingOrderWindowClick(null)
                        ));
            }

        }

        private RelayCommand _ReturnedOrder;

        public RelayCommand ReturnedOrder
        {
            get
            {
                return _ReturnedOrder ?? (_ReturnedOrder = new RelayCommand(
                    (object e) => ReturnedOrderWindowClick(null)
                        ));
            }

        }

        private RelayCommand _FullyExecutedOrder;

        public RelayCommand FullyExecutedOrder
        {
            get
            {
                return _FullyExecutedOrder ?? (_FullyExecutedOrder = new RelayCommand(
                    (object e) => FullyExecutedOrderWindowClick(null)
                        ));
            }

        }


        private RelayCommand _StopLossEntry;

        public RelayCommand StopLossEntry
        {
            get
            {
                return _StopLossEntry ?? (_StopLossEntry = new RelayCommand(
                    (object e) => StopLossEntryWindowClick(null)
                        ));
            }

        }


        private RelayCommand _RetStopLossOrder;

        public RelayCommand RetStopLossOrder
        {
            get
            {
                return _RetStopLossOrder ?? (_RetStopLossOrder = new RelayCommand(
                    (object e) => RetStopLossOrderWindowClick(null)
                        ));
            }

        }


        private RelayCommand _News;

        public RelayCommand News
        {
            get
            {
                return _News ?? (_News = new RelayCommand(
                  (object e) => OpenNews(null)));
            }
        }

        private RelayCommand _ScripHelp;

        public RelayCommand ScripHelp
        {
            get
            {
                return _ScripHelp ?? (_ScripHelp = new RelayCommand(
                  (object e) => OpenScripHelpWindow(null)));
            }
        }

        public RelayCommand _IndexDetails;
        public RelayCommand IndexDetails
        {
            get
            {
                return _IndexDetails ?? (_IndexDetails = new RelayCommand(
                    (object e) => OpenIndexDetails(null)));
            }
        }

        private RelayCommand _Bulletin;

        public RelayCommand Bulletin
        {
            get
            {
                return _Bulletin ?? (_Bulletin = new RelayCommand(
                  (object e) => OpenBulletin(null)));
            }

        }



        private RelayCommand _MenuMarketPicture;

        public RelayCommand MenuMarketPicture
        {
            get
            {
                return _MenuMarketPicture ?? (_MenuMarketPicture = new RelayCommand(
                    (object e) => OpenMenuMarketPicture(null)));

            }

        }

        private RelayCommand _CorporateActionW;
        public RelayCommand CorporateActionW
        {
            get
            {
                return _CorporateActionW ?? (_CorporateActionW = new RelayCommand((object e) => CorporateAction_Click(null)));
            }
        }

        private RelayCommand _TwsHelp;
        public RelayCommand TwsHelp
        {
            get
            {
                return _TwsHelp ?? (_TwsHelp = new RelayCommand((object e) =>
                {
                    if (File.Exists(DirectoryCHMFile.ToString()))
                        System.Windows.Forms.Help.ShowHelp(null, DirectoryCHMFile.ToString());
                }));
            }
        }

        private RelayCommand _ChangePasswordMenuClick;

        public RelayCommand ChangePasswordMenuClick
        {
            get
            {
                return _ChangePasswordMenuClick ?? (_ChangePasswordMenuClick = new RelayCommand(
                    (object e) => OpenPasswordWindow()));

            }

        }
        private RelayCommand _RBIReferanceRateClick;

        public RelayCommand RBIReferanceRateClick
        {
            get
            {
                return _RBIReferanceRateClick ?? (_RBIReferanceRateClick = new RelayCommand(
                    (object e) => OpenRBIReferanceRatedWindow()));

            }

        }



        private RelayCommand _settingsChange;

        public RelayCommand Settings
        {
            get
            {
                return _settingsChange ?? (_settingsChange = new RelayCommand((object e) => SettingChangeClick(null)));
            }

        }

        private RelayCommand _MenuFunctionKeys;

        public RelayCommand MenuFunctionKeys
        {
            get
            {
                return _MenuFunctionKeys ?? (_MenuFunctionKeys = new RelayCommand((object e) => MenuFunctionKeysClick(null)));
            }
        }
        private RelayCommand _MenuOrder;

        public RelayCommand MenuOrder
        {
            get
            {
                return _MenuOrder ?? (_MenuOrder = new RelayCommand((object e) => MenuOrderClick(null)));
            }
        }
        private RelayCommand _MenuEmail;

        public RelayCommand MenuEmail
        {
            get
            {
                return _MenuEmail ?? (_MenuEmail = new RelayCommand((object e) => MenuEmailClick(null)));
            }
        }
        private RelayCommand _MenuTheme;

        public RelayCommand MenuTheme
        {
            get
            {
                return _MenuTheme ?? (_MenuTheme = new RelayCommand((object e) => MenuThemeClick(null)));
            }
        }
        private RelayCommand _MenuColumn;

        public RelayCommand MenuColumn
        {
            get
            {
                return _MenuColumn ?? (_MenuTheme = new RelayCommand((object e) => MenuColumnClick(null)));
            }
        }

        private RelayCommand _MenuColors;

        public RelayCommand MenuColors
        {
            get
            {
                return _MenuColors ?? (_MenuColors = new RelayCommand((object e) => MenuColorsClick(null)));
            }
        }
        private RelayCommand _MenuIPSettings;

        public RelayCommand MenuIPSettings
        {
            get
            {
                return _MenuIPSettings ?? (_MenuIPSettings = new RelayCommand((object e) => MenuIPSettingsClick(null)));
            }
        }



        private RelayCommand _BatchOrders;

        public RelayCommand BatchOrders
        {
            get
            {
                return _BatchOrders ?? (_BatchOrders = new RelayCommand(
                    (object e) => OpenBatchOrderWindow(null)));

            }

        }

        private RelayCommand _MarketReportsV;

        public RelayCommand MarketReportsV
        {
            get
            {
                return _MarketReportsV ?? (_MarketReportsV = new RelayCommand(
                    (object e) => OpenMarketReports(null)));

            }

        }


        private RelayCommand _TrdEntitlement;

        public RelayCommand TrdEntitlement
        {
            get
            {
                return _TrdEntitlement ?? (_TrdEntitlement = new RelayCommand(
                  (object e) => OpenTrdEntitlementWindow(null)));
            }
        }

        //SaveWorkspace. Added by Gaurav Jadhav 21/3/2018
        private RelayCommand _SaveWorkspace;

        public RelayCommand SaveWorkspace
        {
            get
            {
                return _SaveWorkspace ?? (_SaveWorkspace = new RelayCommand(
               (object e) => SaveWorkspace_Click(null)));
            }

        }
        #endregion

        public void OpenTrdEntitlementWindow(HotKey _hotKey)
        {
            if (UtilityLoginDetails.GETInstance.IsLoggedIN)
            {
                //if (MemoryManager.IsPersonalDownloadComplete)
                //{
                if ((UtilityLoginDetails.GETInstance.Role == Role.Admin.ToString()))
                {
                    TraderEntitlementMenu oTraderEntitlementMenu = System.Windows.Application.Current.Windows.OfType<TraderEntitlementMenu>().FirstOrDefault();

                    if (oTraderEntitlementMenu != null)
                    {
                        oTraderEntitlementMenu.Activate();
                        oTraderEntitlementMenu.Show();
                    }
                    else
                    {
                        oTraderEntitlementMenu = new TraderEntitlementMenu();
                        oTraderEntitlementMenu.Activate();
                        oTraderEntitlementMenu.Owner = System.Windows.Application.Current.MainWindow;
                        oTraderEntitlementMenu.Activate();
                        oTraderEntitlementMenu.Show();
                    }
                }
            }
        }


        public void OpenMenuMarketPicture(HotKey _hotKey)
        {
            if (UtilityLoginDetails.GETInstance.IsLoggedIN)
            {
                BestFiveMarketPicture oLoginScreen = System.Windows.Application.Current.Windows.OfType<BestFiveMarketPicture>().FirstOrDefault();

                if (oLoginScreen != null)
                {
                    oLoginScreen.Activate();
                    oLoginScreen.Show();
                }
                else
                {
                    oLoginScreen = new BestFiveMarketPicture();
                    oLoginScreen.Activate();
                    oLoginScreen.Owner = System.Windows.Application.Current.MainWindow;
                    oLoginScreen.Activate();
                    oLoginScreen.Show();
                }
            }
        }


        public void OpenBatchOrderWindow(HotKey _hotKey)
        {
            BatchOrder oBatchOrderScreen = System.Windows.Application.Current.Windows.OfType<BatchOrder>().FirstOrDefault();

            if (oBatchOrderScreen != null)
            {
                oBatchOrderScreen.Activate();
                oBatchOrderScreen.Show();
            }
            else
            {
                oBatchOrderScreen = new BatchOrder();
                oBatchOrderScreen.Activate();
                oBatchOrderScreen.Owner = System.Windows.Application.Current.MainWindow;
                oBatchOrderScreen.Activate();
                oBatchOrderScreen.Show();
            }
        }

        public void OpenNews(HotKey _hotKey)
        {
            if (UtilityLoginDetails.GETInstance.IsLoggedIN)
            {
                try
                {
                    Globals.NewsWindowOpen = true;
                    News objNews = System.Windows.Application.Current.Windows.OfType<News>().FirstOrDefault();
                    if (objNews != null)
                    {
                        objNews.Activate();
                        objNews.Show();
                    }
                    else
                    {
                        objNews = new News();
                        objNews.Activate();
                        objNews.Owner = System.Windows.Application.Current.MainWindow;
                        objNews.Activate();
                        objNews.Show();
                    }

                    if (isfirstnews)
                    {

                        NewsModel objNewsModel = new NewsModel();

                        objNewsModel.NewsHeadline = "All Broadcast News";
                        //  string url = "http://10.1.101.125:3000/twsreports/exchangeAnn_news.aspx?id=" + objNewsModel.NewsId;
                        string url = "http://10.1.101.125:3000/twsreports/exchangeAnn.aspx?&memid=" + UtilityLoginDetails.GETInstance.MemberId;
                        objNewsModel.ObjNewsURL = url;
                        //ProcessStartInfo sInfo = new ProcessStartInfo(url);
                        //Process.Start(sInfo);
                        isfirstnews = false;
                        uiContext.Send(x => NewsVM.ObjNewsCollection.Add(objNewsModel), null);
                    }
                    EqtListener_NewsTick();
                }
                catch (Exception ex)
                {

                }
            }
        }

        public void OpenScripHelpWindow(HotKey key)
        {
            ScripHelp objScripHelp = System.Windows.Application.Current.Windows.OfType<ScripHelp>().FirstOrDefault();
            if (objScripHelp != null)
            {
                objScripHelp.Activate();
                objScripHelp.Show();
            }
            else
            {
                objScripHelp = new ScripHelp();
                objScripHelp.Activate();
                objScripHelp.Owner = System.Windows.Application.Current.MainWindow;
                //objScripHelp.Activate();
                objScripHelp.Show();
            }
        }

        private void OpenRBIReferanceRatedWindow()
        {
            if (UtilityLoginDetails.GETInstance.IsLoggedIN)
            {
                RBIReferanceRate oRBIReferanceRate = System.Windows.Application.Current.Windows.OfType<RBIReferanceRate>().FirstOrDefault();

                if (oRBIReferanceRate != null)
                {
                    oRBIReferanceRate.Activate();
                    oRBIReferanceRate.Show();
                }
                else
                {
                    oRBIReferanceRate = new RBIReferanceRate();
                    oRBIReferanceRate.Activate();
                    oRBIReferanceRate.Owner = System.Windows.Application.Current.MainWindow;
                    //oIndexDetails.Activate();
                    oRBIReferanceRate.Show();
                }
            }
        }

        public void OpenIndexDetails(HotKey _hotKey)
        {
            if (UtilityLoginDetails.GETInstance.IsLoggedIN)
            {
                IndexDetails oIndexDetails = System.Windows.Application.Current.Windows.OfType<IndexDetails>().FirstOrDefault();

                if (oIndexDetails != null)
                {
                    //oIndexDetails.Activate();
                    oIndexDetails.Focus();
                    oIndexDetails.Show();
                }
                else
                {
                    oIndexDetails = new IndexDetails();
                    oIndexDetails.Activate();
                    oIndexDetails.Owner = System.Windows.Application.Current.MainWindow;
                    //oIndexDetails.Activate();
                    oIndexDetails.Show();
                }
            }
        }
        public void OpenBulletin(HotKey _hotKey)
        {
            if (UtilityLoginDetails.GETInstance.IsLoggedIN)
            {
                BSEBulletinsBoard oBSEBulletin = System.Windows.Application.Current.Windows.OfType<BSEBulletinsBoard>().FirstOrDefault();

                if (oBSEBulletin != null)
                {
                    oBSEBulletin.Focus();
                    oBSEBulletin.Show();
                }
                else
                {
                    oBSEBulletin = new BSEBulletinsBoard();
                    oBSEBulletin.Activate();
                    oBSEBulletin.Owner = System.Windows.Application.Current.MainWindow;
                    //oIndexDetails.Activate();
                    oBSEBulletin.Show();
                }
            }
        }
        public void PendingOrderWindowClick(HotKey _hotKey)
        {
#if TWS
            if (UtilityLoginDetails.GETInstance.Role?.ToLower() == "trader")
            {
                View.Order.PendingOrderClassic oPendingOrder = System.Windows.Application.Current.Windows.OfType<View.Order.PendingOrderClassic>().FirstOrDefault();
                if (oPendingOrder != null)
                {
                    PendingOrderClassicVM.GETInstance.SelectedTouchLineScripID = UtilityLoginDetails.GETInstance.SelectedTouchLineScripID;
                    //oPendingOrder.Activate();
                    oPendingOrder.Focus();
                    oPendingOrder.Show();

                }
                else
                {
                    oPendingOrder = new View.Order.PendingOrderClassic();
                    oPendingOrder.Activate();
                    oPendingOrder.Owner = System.Windows.Application.Current.MainWindow;
                    oPendingOrder.Activate();
                    oPendingOrder.Show();
                }
            }
#endif
        }

        public void ReturnedOrderWindowClick(HotKey _hotKey)
        {
#if TWS
            if (UtilityLoginDetails.GETInstance.Role?.ToLower() == "trader")
            {
                ReturnedOrders oReturnedOrders = System.Windows.Application.Current.Windows.OfType<ReturnedOrders>().FirstOrDefault();
                if (oReturnedOrders != null)
                {
                    //oReturnedOrders.Activate();
                    oReturnedOrders.Focus();
                    oReturnedOrders.Show();
                }
                else
                {
                    oReturnedOrders = new View.Order.ReturnedOrders();
                    oReturnedOrders.Activate();
                    oReturnedOrders.Owner = System.Windows.Application.Current.MainWindow;
                    oReturnedOrders.Activate();
                    oReturnedOrders.Show();
                }
            }
#endif
        }

        public void FullyExecutedOrderWindowClick(HotKey _hotKey)
        {
#if TWS
            if (UtilityLoginDetails.GETInstance.Role?.ToLower() == "trader")
            {
                FullyExecutedOrder oFullyExecutedOrder = System.Windows.Application.Current.Windows.OfType<FullyExecutedOrder>().FirstOrDefault();
                if (oFullyExecutedOrder != null)
                {
                    //oFullyExecutedOrder.Activate();
                    oFullyExecutedOrder.Focus();
                    oFullyExecutedOrder.Show();
                }
                else
                {
                    oFullyExecutedOrder = new View.Order.FullyExecutedOrder();
                    oFullyExecutedOrder.Activate();
                    oFullyExecutedOrder.Owner = System.Windows.Application.Current.MainWindow;
                    oFullyExecutedOrder.Activate();
                    oFullyExecutedOrder.Show();
                }
            }
#endif
        }

        public void StopLossEntryWindowClick(HotKey e)
        {
#if TWS
            //if (UtilityLoginDetails.GETInstance.IsLoggedIN)
            //{
            if (UtilityLoginDetails.GETInstance.Role?.ToLower() == "trader")
            {
                StopLossOrderEntry oStopLossOrderEntry = System.Windows.Application.Current.Windows.OfType<StopLossOrderEntry>().FirstOrDefault();
                if (oStopLossOrderEntry != null)
                {
                    //oStopLossOrderEntry.Activate();
                    oStopLossOrderEntry.Focus();
                    oStopLossOrderEntry.Show();
                }
                else
                {
                    oStopLossOrderEntry = new View.Order.StopLossOrderEntry();
                    oStopLossOrderEntry.Activate();
                    oStopLossOrderEntry.Owner = System.Windows.Application.Current.MainWindow;
                    oStopLossOrderEntry.Activate();
                    oStopLossOrderEntry.Show();
                }
            }
            // }
#endif
        }

        public void RetStopLossOrderWindowClick(HotKey e)
        {
#if TWS
            if (UtilityLoginDetails.GETInstance.IsLoggedIN)
            {
                if (UtilityLoginDetails.GETInstance.Role.ToLower() == "trader")
                {
                    ReturnedStopLossOrder oReturnedStopLossOrder = System.Windows.Application.Current.Windows.OfType<ReturnedStopLossOrder>().FirstOrDefault();
                    if (oReturnedStopLossOrder != null)
                    {
                        //oReturnedStopLossOrder.Activate();
                        oReturnedStopLossOrder.Focus();
                        oReturnedStopLossOrder.Show();
                    }
                    else
                    {
                        oReturnedStopLossOrder = new View.Order.ReturnedStopLossOrder();
                        oReturnedStopLossOrder.Activate();
                        oReturnedStopLossOrder.Owner = System.Windows.Application.Current.MainWindow;
                        oReturnedStopLossOrder.Activate();
                        oReturnedStopLossOrder.Show();
                    }
                }
            }
#endif
        }


        public void SettingChangeClick(HotKey key)
        {
            //MemoryManager.IsPersonalDownloadComplete = true;
            //    if (MemoryManager.IsPersonalDownloadComplete)
            //    {
            ProfileSettings SettingsWindow = System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault();

            if (SettingsWindow != null)
            {
                // SettingsWindow.Activate();
                SettingsWindow.Focus();
                SettingsWindow.MainTabControl.SelectedIndex = (int)Enumerations.ScripHelpTab.ScripTab;
                SettingsWindow.Show();
            }
            else
            {
                //SettingsWindow = null;
                SettingsWindow = new ProfileSettings();
                SettingsWindow.ResizeMode = System.Windows.ResizeMode.NoResize;
                SettingsWindow.Owner = System.Windows.Application.Current.MainWindow;
                SettingsWindow.MainTabControl.SelectedIndex = (int)Enumerations.ScripHelpTab.ScripTab;
                SettingsWindow.Show();
            }
            // }
        }

        private void MenuFunctionKeysClick(HotKey key)
        {
            ProfileSettings SettingsWindow = System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault();

            if (SettingsWindow != null)
            {
                // SettingsWindow.Activate();
                SettingsWindow.Focus();
                SettingsWindow.MainTabControl.SelectedIndex = (int)Enumerations.ScripHelpTab.FunctionKeysTab;
                SettingsWindow.Show();
            }
            else
            {
                //SettingsWindow = null;
                SettingsWindow = new ProfileSettings();
                SettingsWindow.ResizeMode = System.Windows.ResizeMode.NoResize;
                SettingsWindow.Owner = System.Windows.Application.Current.MainWindow;
                SettingsWindowhttps://github.com/manojbaddi/WPF-Trading-applicationActivate();
                SettingsWindow.MainTabControl.SelectedIndex = (int)Enumerations.ScripHelpTab.FunctionKeysTab;
                SettingsWindow.Show();
            }
        }

        public void MenuOrderClick(HotKey key)
        {
            ProfileSettings SettingsWindow = System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault();

            if (SettingsWindow != null)
            {
                // SettingsWindow.Activate();
                SettingsWindow.Focus();
                SettingsWindow.MainTabControl.SelectedIndex = (int)Enumerations.ScripHelpTab.OrdersTab;
                SettingsWindow.Show();
            }
            else
            {
                //SettingsWindow = null;
                SettingsWindow = new ProfileSettings();
                SettingsWindow.ResizeMode = System.Windows.ResizeMode.NoResize;
                SettingsWindow.Owner = System.Windows.Application.Current.MainWindow;
                SettingsWindowhttps://github.com/manojbaddi/WPF-Trading-applicationActivate();
                SettingsWindow.MainTabControl.SelectedIndex = (int)Enumerations.ScripHelpTab.OrdersTab;
                SettingsWindow.Show();
            }
        }

        private void MenuEmailClick(HotKey key)
        {
            ProfileSettings SettingsWindow = System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault();

            if (SettingsWindow != null)
            {
                // SettingsWindow.Activate();
                SettingsWindow.Focus();
                SettingsWindow.MainTabControl.SelectedIndex = (int)Enumerations.ScripHelpTab.EmailProfilingTab;
                SettingsWindow.Show();
            }
            else
            {
                //SettingsWindow = null;
                SettingsWindow = new ProfileSettings();
                SettingsWindow.ResizeMode = System.Windows.ResizeMode.NoResize;
                SettingsWindow.Owner = System.Windows.Application.Current.MainWindow;
                SettingsWindowhttps://github.com/manojbaddi/WPF-Trading-applicationActivate();
                SettingsWindow.MainTabControl.SelectedIndex = (int)Enumerations.ScripHelpTab.EmailProfilingTab;
                SettingsWindow.Show();
            }
        }

        private void MenuThemeClick(HotKey key)
        {
            ProfileSettings SettingsWindow = System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault();

            if (SettingsWindow != null)
            {
                // SettingsWindow.Activate();
                SettingsWindow.Focus();
                SettingsWindow.MainTabControl.SelectedIndex = (int)Enumerations.ScripHelpTab.ThemesTab;
                SettingsWindow.Show();
            }
            else
            {
                //SettingsWindow = null;
                SettingsWindow = new ProfileSettings();
                SettingsWindow.ResizeMode = System.Windows.ResizeMode.NoResize;
                SettingsWindow.Owner = System.Windows.Application.Current.MainWindow;
                SettingsWindowhttps://github.com/manojbaddi/WPF-Trading-applicationActivate();
                SettingsWindow.MainTabControl.SelectedIndex = (int)Enumerations.ScripHelpTab.ThemesTab;
                SettingsWindow.Show();
            }
        }

        private void MenuColumnClick(HotKey key)
        {
            ProfileSettings SettingsWindow = System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault();

            if (SettingsWindow != null)
            {
                // SettingsWindow.Activate();
                SettingsWindow.Focus();
                SettingsWindow.MainTabControl.SelectedIndex = (int)Enumerations.ScripHelpTab.coloumnTab;
                SettingsWindow.Show();
            }
            else
            {
                //SettingsWindow = null;
                SettingsWindow = new ProfileSettings();
                SettingsWindow.ResizeMode = System.Windows.ResizeMode.NoResize;
                SettingsWindow.Owner = System.Windows.Application.Current.MainWindow;
                SettingsWindowhttps://github.com/manojbaddi/WPF-Trading-applicationActivate();
                SettingsWindow.MainTabControl.SelectedIndex = (int)Enumerations.ScripHelpTab.coloumnTab;
                SettingsWindow.Show();
            }
        }

        private void MenuColorsClick(HotKey key)
        {
            ProfileSettings SettingsWindow = System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault();

            if (SettingsWindow != null)
            {
                // SettingsWindow.Activate();
                SettingsWindow.Focus();
                SettingsWindow.MainTabControl.SelectedIndex = (int)Enumerations.ScripHelpTab.ColorTab;
                SettingsWindow.Show();
            }
            else
            {
                //SettingsWindow = null;
                SettingsWindow = new ProfileSettings();
                SettingsWindow.ResizeMode = System.Windows.ResizeMode.NoResize;
                SettingsWindow.Owner = System.Windows.Application.Current.MainWindow;
                SettingsWindowhttps://github.com/manojbaddi/WPF-Trading-applicationActivate();
                SettingsWindow.MainTabControl.SelectedIndex = (int)Enumerations.ScripHelpTab.ColorTab;
                SettingsWindow.Show();
            }
        }

        private void MenuIPSettingsClick(HotKey key)
        {
            ProfileSettings SettingsWindow = System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault();

            if (SettingsWindow != null)
            {
                SettingsWindow.Focus();
                SettingsWindow.MainTabControl.SelectedIndex = (int)Enumerations.ScripHelpTab.BoltSettingTab;
                SettingsWindow.Show();
            }
            else
            {
                //SettingsWindow = null;
                SettingsWindow = new ProfileSettings();
                SettingsWindow.ResizeMode = System.Windows.ResizeMode.NoResize;
                SettingsWindow.Owner = System.Windows.Application.Current.MainWindow;
                SettingsWindowhttps://github.com/manojbaddi/WPF-Trading-applicationActivate();
                SettingsWindow.MainTabControl.SelectedIndex = (int)Enumerations.ScripHelpTab.BoltSettingTab;
                SettingsWindow.Show();
            }
        }
        public void OpenWebappsWindow(HotKey _hotKey)
        {
            if (UtilityLoginDetails.GETInstance.IsPeerConnected == 1)
                Process.Start("http://10.1.101.125:3000/twsreports/webapp.htm");
            else
                Process.Start("http://10.1.101.125:3000/twsreports/webapp.htm");
        }


        public static void MenuVisibility()
        {
#if TWS
            VisibileToBow = "Collapsed";

            if (UtilityLoginDetails.GETInstance.IsEQXChecked)
            {
                MenuVisibilityBeforeLogin = "Visible";
                MenuVisibilityAfterLogin = "Visible";
            }
            else
            {
                MenuVisibilityBeforeLogin = "Visible";
                MenuVisibilityAfterLogin = "Collapsed";

            }
#elif BOW
            VisibileToBow = "Visible";
#endif


        }


        public static void MenuTouchlineClick(HotKey _hotKey)
        {
#if TWS
            if (UtilityLoginDetails.GETInstance.IsLoggedIN)
            {
                // if (MemoryManager.IsPersonalDownloadComplete)
                {
                    MarketWatch mainwindow = System.Windows.Application.Current.Windows.OfType<MarketWatch>().FirstOrDefault();

                    if (mainwindow != null)
                    {
                        if (mainwindow.WindowState == WindowState.Minimized)
                            mainwindow.WindowState = WindowState.Normal;
                        mainwindow.Focus();
                        mainwindow.Show();
                    }
                    else
                    {
                        mainwindow = new MarketWatch();
                        mainwindow.Owner = System.Windows.Application.Current.MainWindow;
                        mainwindow.dataGridView1.Sorting += new DataGridSortingEventHandler(MarketWatchVM.DataGrid_Sorting);
                        mainwindow.dataGridView1.PreviewKeyDown += new KeyEventHandler(MarketWatchVM.DataGrid_PreviewPageUpDown);
                        mainwindow.dataGridView1.AddHandler(ScrollViewer.ScrollChangedEvent, new ScrollChangedEventHandler(MarketWatchVM.DataGrid_ScrollChanged));

                        new Thread(() =>
                        {
                            Thread.CurrentThread.IsBackground = true;
                            System.Windows.Application.Current.Dispatcher.Invoke(() => mainwindow.Show());
                        }).Start();
                        //Task.Run(() => System.Windows.Application.Current.Dispatcher.Invoke(() => mainwindow.Show()));
                        // touchline.Show();
                    }

                }
            }
#endif
        }


        public void RegOrderEntryBuySellWindowF3(Key key)
        {
            if (key == Key.F3)
            {
                if (UtilityOrderDetails.GETInstance.CurrentOrderEntry?.ToLower() == Enumerations.OrderEntryWindow.Normal.ToString().ToLower())
                {
                    OrderEntryBuyWindow_Click("Buy");
                }

                if (key == Key.Add)
                    OrderEntryBuyWindow_Click("BUY");
                else if (key == Key.Subtract)
                    OrderEntrySellWindow_Click("SELL");
            }

        }

        public void RegOrderEntryBuySellWindow(Key key)
        {
            if (key == Key.F1 || key == Key.Add)
                OrderEntryBuyWindow_Click("BUY");
            else if (key == Key.F2 || key == Key.Subtract)
                OrderEntrySellWindow_Click("SELL");

        }

        /// <summary>
        /// Invoke OrderEntry Screen
        /// </summary>
        public void OrderEntryBuyWindow_Click(object e)
        {
            if (UtilityLoginDetails.GETInstance.IsLoggedIN)
            {
                //if (MemoryManager.IsPersonalDownloadComplete)
                //{
                if (!(UtilityLoginDetails.GETInstance.Role == Role.Admin.ToString()))
                {
                    if (UtilityOrderDetails.GETInstance.CurrentOrderEntry?.ToLower() == Enumerations.OrderEntryWindow.Normal.ToString().ToLower())
                    {
                        NormalOrderEntry objnormal = Application.Current.Windows.OfType<NormalOrderEntry>().FirstOrDefault();

                        if (objnormal != null)
                        {
                            if (objnormal.WindowState == WindowState.Minimized)
                                objnormal.WindowState = WindowState.Normal;

                            if (((NormalOrderEntryVM)objnormal.DataContext).WindowColour != "#89C4F4")
                                ((NormalOrderEntryVM)objnormal.DataContext).BuySellWindow(e);

                            ((NormalOrderEntryVM)objnormal.DataContext).SetScripCodeandID();

                            objnormal.Focus();
                            objnormal.Show();
                            ((NormalOrderEntryVM)objnormal?.DataContext)?.AssignDefaultFocusStart(null);
                            //MemoryManager.InvokeWindowOnScripCodeSelection(Convert.ToString(UtilityLoginDetails.GETInstance.SelectedTouchLineScripCode), "Normal");
                        }
                        else
                        {
                            objnormal = new NormalOrderEntry();
                            objnormal.Owner = System.Windows.Application.Current.MainWindow;
                            //objswift.CmbExcangeType.Focus();


                            if (((NormalOrderEntryVM)objnormal.DataContext).WindowColour != "#89C4F4")
                                ((NormalOrderEntryVM)objnormal.DataContext).BuySellWindow(e);

                            objnormal.Activate();
                            objnormal.Show();
                            //MemoryManager.InvokeWindowOnScripCodeSelection(Convert.ToString(UtilityLoginDetails.GETInstance.SelectedTouchLineScripCode), "Normal");
                        }
                    }
                    else if (UtilityOrderDetails.GETInstance.CurrentOrderEntry?.ToLower() == Enumerations.OrderEntryWindow.Swift.ToString().ToLower())
                    {
                        SwiftOrderEntry objswift = Application.Current.Windows.OfType<SwiftOrderEntry>().FirstOrDefault();

                        if (objswift != null)
                        {
                            if (objswift.WindowState == WindowState.Minimized)
                                objswift.WindowState = WindowState.Normal;

                            ((OrderEntryVM)objswift.DataContext).BuySellWindow(e);
                            ((OrderEntryVM)objswift.DataContext).SetScripCodeandID();

                            objswift.Focus();
                            objswift.Show();
                        }
                        else
                        {
                            objswift = new SwiftOrderEntry();
                            objswift.Owner = System.Windows.Application.Current.MainWindow;
                            //objswift.CmbExcangeType.Focus();

                            ((OrderEntryVM)objswift.DataContext).BuySellWindow(e);

                            objswift.Activate();
                            objswift.Show();
                        }
                    }

                }
            }
        }

        public void OrderEntrySellWindow_Click(object e)
        {
            if (UtilityLoginDetails.GETInstance.IsLoggedIN)
            {
                if (!(UtilityLoginDetails.GETInstance.Role == Role.Admin.ToString()))
                {
                    if (UtilityOrderDetails.GETInstance.CurrentOrderEntry?.ToLower() == Enumerations.OrderEntryWindow.Normal.ToString().ToLower())
                    {
                        NormalOrderEntry objnormal = Application.Current.Windows.OfType<NormalOrderEntry>().FirstOrDefault();

                        if (objnormal != null)
                        {
                            if (objnormal.WindowState == WindowState.Minimized)
                                objnormal.WindowState = WindowState.Normal;

                            if (((NormalOrderEntryVM)objnormal.DataContext).WindowColour != "#FFB3A7")
                                ((NormalOrderEntryVM)objnormal.DataContext).BuySellWindow(e);

                            ((NormalOrderEntryVM)objnormal.DataContext).SetScripCodeandID();

                            objnormal.Focus();
                            objnormal.Show();
                            ((NormalOrderEntryVM)objnormal?.DataContext)?.AssignDefaultFocusStart(null);
                        }
                        else
                        {
                            objnormal = new NormalOrderEntry();
                            objnormal.Owner = System.Windows.Application.Current.MainWindow;
                            //objswift.CmbExcangeType.Focus();

                            if (((NormalOrderEntryVM)objnormal.DataContext).WindowColour != "#FFB3A7")
                                ((NormalOrderEntryVM)objnormal.DataContext).BuySellWindow(e);

                            objnormal.Activate();
                            objnormal.Show();
                        }
                    }
                    else if (UtilityOrderDetails.GETInstance.CurrentOrderEntry?.ToLower() == Enumerations.OrderEntryWindow.Swift.ToString().ToLower())
                    {
                        SwiftOrderEntry objswift = Application.Current.Windows.OfType<SwiftOrderEntry>().FirstOrDefault();

                        if (objswift != null)
                        {
                            if (objswift.WindowState == WindowState.Minimized)
                                objswift.WindowState = WindowState.Normal;

                            ((OrderEntryVM)objswift.DataContext).BuySellWindow(e);
                            ((OrderEntryVM)objswift.DataContext).SetScripCodeandID();

                            objswift.Focus();
                            objswift.Show();
                        }
                        else
                        {
                            objswift = new SwiftOrderEntry();
                            objswift.Owner = System.Windows.Application.Current.MainWindow;
                            //objswift.CmbExcangeType.Focus();

                            ((OrderEntryVM)objswift.DataContext).BuySellWindow(e);

                            objswift.Activate();
                            objswift.Show();
                        }
                    }
                }
            }
        }

        public static void OpenPasswordWindow()
        {
            if (UtilityLoginDetails.GETInstance.IsLoggedIN)
            {
                View.Login.ChangePassword oChangePassword = System.Windows.Application.Current.Windows.OfType<View.Login.ChangePassword>().FirstOrDefault();

                if (oChangePassword != null)
                {
                    //ChangePassword.Close();
                    //ChangePassword = new ChangePassword();
                    //ChangePassword.Activate();
                    oChangePassword.Closing += delegate (object sender, CancelEventArgs e) { e.Cancel = true; };
                    oChangePassword.Focus();
                    oChangePassword.pwdBoxOldPassword.Focus();
                    oChangePassword.Show();
                }
                else
                {
                    oChangePassword = new View.Login.ChangePassword();
                    //if (System.Windows.Application.Current.MainWindow.IsLoaded)
                    oChangePassword.Owner = System.Windows.Application.Current.MainWindow;
                    oChangePassword.Closing += delegate (object sender, CancelEventArgs e) { e.Cancel = true; };
                    oChangePassword.Activate();
                    oChangePassword.pwdBoxOldPassword.Focus();
                    oChangePassword.Show();
                }
            }
        }

        public void OpenSessionBroadcastWindow(HotKey _HotKey)
        {
            if (UtilityLoginDetails.GETInstance.IsLoggedIN)
            {
                Globals.SessionWindowOpen = true;
                SessionBroadcast objSessionBroadcast = Application.Current.Windows.OfType<SessionBroadcast>().FirstOrDefault();
                if (objSessionBroadcast != null)
                {
                    if (objSessionBroadcast.WindowState == WindowState.Minimized)
                        objSessionBroadcast.WindowState = WindowState.Normal;

                    objSessionBroadcast.Focus();
                    objSessionBroadcast.Show();
                }
                else
                {
                    objSessionBroadcast = new SessionBroadcast();
                    objSessionBroadcast.Owner = System.Windows.Application.Current.MainWindow;
                    //objswift.CmbExcangeType.Focus();
                    objSessionBroadcast.Activate();
                    objSessionBroadcast.Show();
                }
                EqtListener_SessionTick();
            }
        }

        public void OpenVARPercetageWindow(HotKey _HotKey)
        {
            if (UtilityLoginDetails.GETInstance.IsLoggedIN)
            {
                Globals.VarWindowOpen = true;
                VarPercentageBroadcast objVarPercentageBroadcast = Application.Current.Windows.OfType<VarPercentageBroadcast>().FirstOrDefault();

                if (objVarPercentageBroadcast != null)
                {
                    if (objVarPercentageBroadcast.WindowState == WindowState.Minimized)
                        objVarPercentageBroadcast.WindowState = WindowState.Normal;

                    objVarPercentageBroadcast.Focus();
                    objVarPercentageBroadcast.Show();
                }
                else
                {
                    objVarPercentageBroadcast = new VarPercentageBroadcast();
                    objVarPercentageBroadcast.Owner = System.Windows.Application.Current.MainWindow;
                    //objswift.CmbExcangeType.Focus();
                    objVarPercentageBroadcast.Activate();
                    objVarPercentageBroadcast.Show();
                }

                VarPercentageBroadcastVM.EqtListener_VarTick();
            }
        }

        public void OpenInterestClick(HotKey _HotKey)
        {
            if (UtilityLoginDetails.GETInstance.IsLoggedIN)
            {
                Globals.IsOiWindowOPen = true;

                OpenInterest oOpenInterest = Application.Current.Windows.OfType<OpenInterest>().FirstOrDefault();
                if (oOpenInterest != null)
                {
                    if (oOpenInterest.WindowState == WindowState.Minimized)
                        oOpenInterest.WindowState = WindowState.Normal;

                    oOpenInterest.Focus();
                    oOpenInterest.Show();
                }
                else
                {
                    oOpenInterest = new OpenInterest();
                    oOpenInterest.Owner = System.Windows.Application.Current.MainWindow;
                    //objswift.CmbExcangeType.Focus();
                    oOpenInterest.Activate();
                    oOpenInterest.Show();
                }

                OpenInterestVM.Listener_OITick();
            }
        }

        public void TradeLimits_Click(HotKey _hotKey)
        {
#if TWS
            if (UtilityLoginDetails.GETInstance.Role?.ToLower() == "admin")
            {
                //TODO: Invoke this window only if loginflag is true and personal download is completed
                View.Login.TraderPasswordReset objTraderPasswordReset = Application.Current.Windows.OfType<View.Login.TraderPasswordReset>().FirstOrDefault();

                if (objTraderPasswordReset != null)
                {
                    if (objTraderPasswordReset.WindowState == WindowState.Minimized)
                        objTraderPasswordReset.WindowState = WindowState.Normal;

                    objTraderPasswordReset.Focus();
                    objTraderPasswordReset.Show();
                }
                else
                {
                    objTraderPasswordReset = new View.Login.TraderPasswordReset();
                    objTraderPasswordReset.Owner = System.Windows.Application.Current.MainWindow;
                    //objswift.CmbExcangeType.Focus();
                    objTraderPasswordReset.Activate();
                    objTraderPasswordReset.Show();
                }
            }
            else if (UtilityLoginDetails.GETInstance.Role?.ToLower() == "trader")
            {
                //TODO: Invoke this window only if loginflag is true and personal download is completed
                TradeLimits objTradeLimits = Application.Current.Windows.OfType<TradeLimits>().FirstOrDefault();

                if (objTradeLimits != null)
                {
                    if (objTradeLimits.WindowState == WindowState.Minimized)
                        objTradeLimits.WindowState = WindowState.Normal;

                    objTradeLimits.Focus();
                    objTradeLimits.Show();
                }
                else
                {
                    objTradeLimits = new TradeLimits();
                    objTradeLimits.Owner = System.Windows.Application.Current.MainWindow;
                    //objswift.CmbExcangeType.Focus();
                    objTradeLimits.Activate();
                    objTradeLimits.Show();
                }
            }
#endif
        }

        internal void OpenLockScreenWindow()
        {
            if (UtilityLoginDetails.GETInstance.IsLoggedIN)
            {
                CustomLockScreen ocustomLockScreen = Application.Current.Windows.OfType<CustomLockScreen>().FirstOrDefault();

                // Show WhiteBackGround Window
                WhiteBackGroundWindow owhiteBackground = Application.Current.Windows.OfType<WhiteBackGroundWindow>().FirstOrDefault();

                if (ocustomLockScreen != null)
                {

                    owhiteBackground?.Activate();
                    owhiteBackground.WindowState = WindowState.Maximized;
                    owhiteBackground?.Show();
                }
                else
                {
                    owhiteBackground = new WhiteBackGroundWindow();
                    owhiteBackground?.Activate();
                    owhiteBackground.WindowState = WindowState.Maximized;
                    owhiteBackground?.Show();
                }

                //Show LockScreenWindow
                if (ocustomLockScreen != null)
                {
                    if (ocustomLockScreen.WindowState == WindowState.Minimized)
                        ocustomLockScreen.WindowState = WindowState.Normal;

                    ocustomLockScreen.Focus();
                    ocustomLockScreen.LockScreePassword.Focus();
                    ocustomLockScreen.ShowDialog();
                }
                else
                {


                    ocustomLockScreen = new CustomLockScreen();
                    ocustomLockScreen.Owner = System.Windows.Application.Current.MainWindow;
                    //objswift.CmbExcangeType.Focus();
                    ocustomLockScreen.Activate();
                    ocustomLockScreen.LockScreePassword.Focus();
                    ocustomLockScreen.ShowDialog();
                }
            }
        }



#if TWS
        //private BroadcastReceiver.VarMain UpdateIndicesDatafromMemory(BroadcastReceiver.VarMain br)
        //{

        //}
        private static Model.IdicesDetailsMain UpdateIndicesDatafromMemory(BroadcastReceiver.IdicesDetailsMain br)
        {
            Model.IdicesDetailsMain idMain = new Model.IdicesDetailsMain();
            //Model.IndexMain i = new Model.IndexMain();
            //if (br == null)
            //    return i;

            //i.MessType = br.MessType;
            //i.NoOfRec = br.NoOfRec;
            //for (int index = 0; index < br.NoOfRec; index++)
            {

                idMain.IndexCode = br.IndexCode;
                idMain.IndexHigh = br.IndexHigh;
                idMain.IndexLow = br.IndexLow;
                idMain.IndexOpen = br.IndexOpen;
                idMain.PreviousIndexClose = br.PreviousIndexClose;
                idMain.IndexValue = br.IndexValue;
                idMain.IndexId = br.IndexId;
                if (!BroadcastMasterMemory.objIndexDetailsConDict.Keys.Contains(idMain.IndexCode))
                {
                    BroadcastMasterMemory.objIndexDetailsConDict.Add(idMain.IndexCode, idMain);
                }
                else
                {
                    BroadcastMasterMemory.objIndexDetailsConDict[idMain.IndexCode] = idMain;
                }
                //i.listindMain.Add(idMain);
            }
            return idMain;
        }
#endif
        public void CorporateAction_Click(HotKey _hotKey)
        {
            View.CorporateAction.CorporateAction corpaction = new View.CorporateAction.CorporateAction();
            if (!corpaction.IsLoaded == Load_CorporateAction)
                corpaction = new View.CorporateAction.CorporateAction();
            // corpaction = new AdvancedTWS.View.CorporateAction();

            corpaction.Activate();
            corpaction.Owner = System.Windows.Application.Current.MainWindow;
            corpaction.Activate();
            corpaction.Show();
            // Load_CorporateAction = true;
        }

        public void OpenMarketReports(HotKey _hotKey)
        {
            if (UtilityLoginDetails.GETInstance.IsLoggedIN)
            {
                MarketReports mkreport = new MarketReports();
                if (!mkreport.IsLoaded == Load_CorporateAction)
                    mkreport = new MarketReports();

                mkreport.Activate();
                mkreport.Owner = System.Windows.Application.Current.MainWindow;
                mkreport.Activate();
                //   mkreport.Show();
                // Load_CorporateAction = true;
            }
        }

        private void SaveWorkspace_Click(object p)
        {
            #region Commented


            //DataAccessLayer oDataAccessLayer = null;
            //try
            //{
            //    string traderid = Convert.ToString(UtilityLoginDetails.GETInstance.TraderId).PadLeft(5, '0');
            //    string userid = string.Format("{0}_{1}", UtilityLoginDetails.GETInstance.MemberId, traderid);
            //    if (!string.IsNullOrEmpty(userid) && userid != "0_00000")
            //    {
            //        oDataAccessLayer = new DataAccessLayer();
            //        oDataAccessLayer.ConnectSettings();
            //        if (oDataAccessLayer.OpenConnection())
            //        {
            //            string selectQuery = @"SELECT USERID,       WINDOWNAME,       OPENCLOSED  FROM TWS_OPEN_WINDOWS AS TWS_OPEN_WINDOWS WHERE USERID = '" + userid + "';";
            //            System.Data.DataSet ds = oDataAccessLayer.ExecuteDataSet(selectQuery, System.Data.CommandType.Text, null);
            //            if (ds.Tables[0] != null)
            //            {

            //            }

            //            //Order Entry Short Client Id Save
            //            selectQuery = @"SELECT USERID,       EQ_SHORTCLIENTID,       EQ_SHORTCLIENT_CHECK,       DER_SHORTCLIENTID,       DER_SHORTCLIENT_CHECK,       CUR_SHORTCLIENTID,       CUR_SHORTCLIENT_CHECK,       DEBT_SHORTCLIENTID,       DEBT_SHORTCLIENT_CHECK  FROM TWS_ORDER_ENTRY_SHORTCLIENT_ID WHERE USERID = '" + userid + "';";
            //            System.Data.DataSet ds1 = oDataAccessLayer.ExecuteDataSet(selectQuery, System.Data.CommandType.Text, null);
            //            if (ds1.Tables[0] != null)
            //            {
            //                if (ds1.Tables[0].Rows.Count > 0)//Fire Update Query
            //                {
            //                    string updateQuery = @"UPDATE TWS_ORDER_ENTRY_SHORTCLIENT_ID   SET EQ_SHORTCLIENTID = 'EQ_SHORTCLIENTID',       EQ_SHORTCLIENT_CHECK = 'EQ_SHORTCLIENT_CHECK',       DER_SHORTCLIENTID = 'DER_SHORTCLIENTID',       DER_SHORTCLIENT_CHECK = 'DER_SHORTCLIENT_CHECK',       CUR_SHORTCLIENTID = 'CUR_SHORTCLIENTID',       CUR_SHORTCLIENT_CHECK = 'CUR_SHORTCLIENT_CHECK',       DEBT_SHORTCLIENTID = 'DEBT_SHORTCLIENTID',       DEBT_SHORTCLIENT_CHECK = 'DEBT_SHORTCLIENT_CHECK' WHERE USERID = 'USERID' ";
            //                    int result = MasterSharedMemory.oDataAccessLayer.ExecuteNonQuery(updateQuery, System.Data.CommandType.Text, null);
            //                }
            //                else//insert new records
            //                {

            //                }

            //            }
            //        }
            //    }
            //    else
            //    {
            //        MessageBox.Show("PLEASE LOG IN TO SAVE WORKSPACE");
            //    }
            //}
            #endregion
            if (UtilityLoginDetails.GETInstance.IsLoggedIN)
            {
                try
                {
                    string path = string.Empty;
                    string userid = string.Format("{0}{1}", UtilityLoginDetails.GETInstance.MemberId, Convert.ToString(UtilityLoginDetails.GETInstance.TraderId).PadLeft(5, '0'));
                    if (!string.IsNullOrEmpty(userid) && userid != "000000")
                    {
                        DirectoryInfo TWSProfileDirectory = new DirectoryInfo(Path.GetFullPath(Path.Combine(UtilityApplicationDetails.GetInstance.CurrentDirectory, @"Profile/TwsProOOOOOO.ini")));
                        path = TWSProfileDirectory.ToString().Replace("OOOOOO", UtilityLoginDetails.GETInstance.FileName);
                        ReadConfigurations.SaveConfigurationTWSProfileINI(path);
                    }
                }
                catch (Exception ex)
                {
                    ExceptionUtility.LogError(ex);
                }
                finally
                {
                    //if (oDataAccessLayer != null)
                    //{
                    //    oDataAccessLayer.CloseConnection();
                    //}
                }
            }
        }

        public static event EventHandler<PropertyChangedEventArgs> StaticPropertyChanged
                 = delegate { };
        public event PropertyChangedEventHandler PropertyChanged;

        private static void NotifyStaticPropertyChanged(string propertyName)
        {
            StaticPropertyChanged(null, new PropertyChangedEventArgs(propertyName));
        }

    }



#if TWS
    public partial class MainWindowVM : INotifyPropertyChanged
    {

        public static DirectoryInfo DirectoryCHMFile = new DirectoryInfo(Path.GetFullPath(Path.Combine(System.Environment.CurrentDirectory, @"TWS.chm")));
        public static DirectoryInfo TwsINIPath = new DirectoryInfo(Path.GetFullPath(Path.Combine(System.Environment.CurrentDirectory, @"Profile/Tws.ini")));
        public static DirectoryInfo OrderSettingsINIPath = new DirectoryInfo(Path.GetFullPath(Path.Combine(System.Environment.CurrentDirectory, @"Profile/OrderSettings.ini")));
        public static DirectoryInfo AllIndicesINIPath = new DirectoryInfo(Path.GetFullPath(Path.Combine(System.Environment.CurrentDirectory, @"Profile/AllIndicesConfigure.ini")));
        DirectoryInfo IMLPath = new DirectoryInfo(Path.GetFullPath(Path.Combine(System.Environment.CurrentDirectory, @"imlPro.exe")));
        public static DirectoryInfo BoltINIPath = new DirectoryInfo(Path.GetFullPath(Path.Combine(System.Environment.CurrentDirectory, @"BOLT.ini")));
        public static DirectoryInfo CPRPath = new DirectoryInfo(Path.GetFullPath(Path.Combine(System.Environment.CurrentDirectory, @"Profile/defaultCpr.ini")));
        public static IniParser parser = new IniParser(TwsINIPath.ToString());
        public static IniParser parserOS = new IniParser(OrderSettingsINIPath.ToString());
        public static IniParser parserAllIndices = new IniParser(AllIndicesINIPath.ToString());
        public static IniParser ParserBoltIni = new IniParser(BoltINIPath.ToString());
        public static IniParser ParserDefaultCPR = new IniParser(CPRPath.ToString());
        public string IMLName = "imlPro";




        //int key = 0xFACA;
        int key = 10;

        public string ByPassLogin { get; set; }
        public static bool EqtyBcastFlag { get; set; }
        public bool DervBcastFlag { get; set; }
        public bool CurrBcastFlag { get; set; }
        public static bool EqtyFlag { get; set; }
        public static bool DervFlag { get; set; }
        public static bool CurrFlag { get; set; }
        public string MemberID { get; set; }
        public string TraderID { get; set; }
        public string DecrypterdPwd { get; set; }
        public static BroadcastListener EqtListener;
        public static BroadcastListener DerivativeListener;
        public static BroadcastListener CurrencyListener;

        public static Mutex TLMutex;
        public static Thread BcastUpdateThread;
        public static Thread BcastUpdateIndices;
        //  public static Thread BcastUpdateVar;
        //every two hours
        public event EventHandler BcastUpdateVarRecieved;



        // public static Thread BcastUpdateRBI;
        //every 125 minutes in derivates and currency
        public event EventHandler BcastUpdateOIRecieved;
        // public static Thread BcastUpdateOI;
        public static Thread BcastUpdateNews;
        public static Thread BcastUpdateSession;
        public static Thread CommonMessagingWindowThread;

        public ProcessStartInfo info = null;
        public static Process process = new Process();


        static int PrevMarketType;
        static int PrevSessionNumber;

        private static string _TitleTWSMainWindow;

        public static string TitleTWSMainWindow
        {
            get { return _TitleTWSMainWindow; }
            set { _TitleTWSMainWindow = value; NotifyStaticPropertyChanged("TitleTWSMainWindow"); }
        }

        private static string _MenuVisibility = "Visible";

        public static string MenuVisibility_Expander
        {
            get { return _MenuVisibility; }
            set { _MenuVisibility = value; NotifyStaticPropertyChanged("MenuVisibility_Expander"); }
        }

        private RelayCommand _LoadCMW;

        public RelayCommand LoadCMW
        {
            get
            {
                return _LoadCMW ?? (_LoadCMW = new RelayCommand((object e) => loadcommonmessaging(true)));
            }

        }
        private static RelayCommand _saudas;

        public static RelayCommand Saudas
        {
            get
            {
                return _saudas ?? (_saudas = new RelayCommand(
                    (object e) => MenuSaudasClick(null)
                        ));
            }
        }

        private RelayCommand _NetposScripWiseMenu;

        public RelayCommand NetposScripWiseMenu
        {
            get { return _NetposScripWiseMenu ?? (_NetposScripWiseMenu = new RelayCommand((object e) => NetPositionScripWiseClick(null))); }
        }

        private RelayCommand _NetposClientWiseMenu;

        public RelayCommand NetposClientWiseMenu
        {
            get { return _NetposClientWiseMenu ?? (_NetposClientWiseMenu = new RelayCommand((object e) => NetPositionClientWiseClick(null))); }
        }

        private RelayCommand _TradeLimits;

        public RelayCommand TradeLimits
        {
            get { return _TradeLimits ?? (_TradeLimits = new RelayCommand((object e) => TradeLimits_Click(null))); }
        }

        private RelayCommand _OpenLockScreenWindowClick;

        public RelayCommand OpenLockScreenWindowClick
        {
            get { return _OpenLockScreenWindowClick ?? (_OpenLockScreenWindowClick = new RelayCommand((object e) => OpenLockScreenWindow())); }
        }

        private RelayCommand<CancelEventArgs> _Window_Closing;

        public RelayCommand<CancelEventArgs> Window_Closing
        {
            get
            {
                return _Window_Closing ?? (_Window_Closing = new RelayCommand<CancelEventArgs>(Window_Closing_Event));
            }
        }

        private RelayCommand _CloseWindowsOnEscape;

        public RelayCommand CloseWindowsOnEscape
        {
            get { return _CloseWindowsOnEscape ?? (_CloseWindowsOnEscape = new RelayCommand((object e) => CloseWindowsOnEscape_Click())); }
        }

        private RelayCommand _DownMenu;

        public RelayCommand DownMenu
        {
            get
            {
                return _DownMenu ?? (_DownMenu = new RelayCommand((object e) => DownMenu_Click()));
            }

        }

        private RelayCommand _UpMenu;

        public RelayCommand UpMenu
        {
            get
            {
                return _UpMenu ?? (_UpMenu = new RelayCommand((object e) => UpMenu_Click()));
            }

        }

        private MainWindowVM()
        {

            //Step 1: Reading of masters
            var SegmentCount = Enum.GetNames(typeof(Common.Enumerations.Segment)).Length;
            objLogOnStatus = new LogOnStatus[SegmentCount];


            var arr = Enum.GetValues(typeof(Common.Enumerations.Segment)).Cast<Common.Enumerations.Segment>();
            for (int i = 0; i < SegmentCount; i++)
            {
                objLogOnStatus[i] = new LogOnStatus();
            }

            MasterSharedMemory.ReadAllMasters();

            //Read ETI Structure
            ReadConfigurations.ReadETIStructure();

            ReadConfigurations.ReadReturnedOrderMessages();
            MenuVisibility();
            // System.Windows.Application.Current.Resources.Source = new Uri("/Themes/BlackCurrent.xaml", UriKind.RelativeOrAbsolute);
            MemoryManager.OnLogonReplyReceived += MemoryManager_OnLogonReplyReceived;
            loadcommonmessaging(false);
            InvokeIML();
            // oLoginScreen.MyPasswordBox.Focus();


            AssignTitle();


            if (EqtListener == null)
            {
                var host = Dns.GetHostEntry(Dns.GetHostName());
                IPAddress McInterFaceIp_address = host.AddressList[1];
                //TradeFeedVM.Window_Loaded_Click();

                BoltSettingsVM objBoltSettingsVM = new BoltSettingsVM();
                string BroadCastIp = BoltSettingsVM.EquityIP1BCastProd;
                int BroadCastPort = Convert.ToInt32(BoltSettingsVM.EquityPort1BCastProd);
                string DrvBroadCastIp = BoltSettingsVM.DerivativeIP1BCastProd;
                int DrvBroadCastPort = Convert.ToInt32(BoltSettingsVM.DerivativePort1BCastProd);
                string CurrencyBroadCastIp = BoltSettingsVM.CurrencyIP1BCastProd;
                int CurrencyInterfaceIp = Convert.ToInt32(BoltSettingsVM.CurrencyPort1BCastProd);

                string McInterFaceIp = McInterFaceIp_address.ToString();

                EqtListener = new BroadcastListener(BroadCastIp, BroadCastPort, McInterFaceIp);
                DerivativeListener = new BroadcastListener(DrvBroadCastIp, DrvBroadCastPort, McInterFaceIp);
                CurrencyListener = new BroadcastListener(CurrencyBroadCastIp, CurrencyInterfaceIp, McInterFaceIp);

                TLMutex = new Mutex();
                BcastUpdateThread = new Thread(UpdateMktPicScripDetail);
                //   BcastUpdateIndices = new Thread(UpdateBcastIndicesDetails);
                VarPercentageBroadcastVM.ObjVarPercentageBroadcastCollection = new System.Collections.ObjectModel.ObservableCollection<VarPercentageBroadcastModel>();
                RBIReferanceRateVM.ObjRBIReferenceRateCollection = new System.Collections.ObjectModel.ObservableCollection<RBIReferanceRateModel>();
                OpenInterestVM.ObjOpenInterstCollection = new System.Collections.ObjectModel.ObservableCollection<OpenInterestModel>();
                SessionVM.ObjSessionBroadCastCollection = new System.Collections.ObjectModel.ObservableCollection<Model.Session.SessionModel>();
                StartCMWThread();


                SharedMemories.MemoryManager.ChangePassordResAction += SetResponceReplyText;
                SharedMemories.MemoryManager.TradePasswordResetAction += ResetPasswordReplyText;


            }
            ShortCutKeysFlag = parser.GetSetting("Login Settings", "NEWBUTTONS");
            if (ShortCutKeysFlag == null)
            {
                ShortCutKeysFlag = "0";
            }
            RegisterGlobalKeys();
            UtilityOrderDetails.GETInstance.MktProtection = parserOS.GetSetting("GENERAL OS", "MarketProtection") == null ? "1.0" : parserOS.GetSetting("GENERAL OS", "MarketProtection");
            UtilityOrderDetails.GETInstance.DefaultFocusOESettings = parserOS.GetSetting("NORMAL OS", "SelectedDefaultFocusforNormalOE") == null ? "ScripCode" : parserOS.GetSetting("NORMAL OS", "SelectedDefaultFocusforNormalOE");
            UtilityOrderDetails.GETInstance.Default5LChecked = Convert.ToBoolean(MainWindowVM.parserOS.GetSetting("NORMAL OS", "Default5LChecked") == null ? ("False") : MainWindowVM.parserOS.GetSetting("NORMAL OS", "Default5LChecked"));
            UtilityOrderDetails.GETInstance.SelectedTouchlineData = MainWindowVM.parserOS.GetSetting("NORMAL OS", "TouchlineDataFormat") == null ? ("B:[BQ@BR]//S:[SQ@SR]") : MainWindowVM.parserOS.GetSetting("NORMAL OS", "TouchlineDataFormat");
            UtilityOrderDetails.GETInstance.RevlQtyPercentage = parserOS.GetSetting("GENERAL OS", "RevQty") == null ? "100.00" : parserOS.GetSetting("GENERAL OS", "RevQty");
            UtilityOrderDetails.GETInstance.EQTYMaxQty = parserOS.GetSetting("SINGLE ORDER LIMIT", "EqMaxOrderQty");
            UtilityOrderDetails.GETInstance.EQTYMinQty = parserOS.GetSetting("SINGLE ORDER LIMIT", "EqMinOrderQty");
            UtilityOrderDetails.GETInstance.EQTYMaxOrderValue = parserOS.GetSetting("SINGLE ORDER LIMIT", "EqMaxOrderValue");
            UtilityOrderDetails.GETInstance.DERVMaxQty = parserOS.GetSetting("SINGLE ORDER LIMIT", "DerMaxOrderQty");
            UtilityOrderDetails.GETInstance.DERVMinQty = parserOS.GetSetting("SINGLE ORDER LIMIT", "DerMinOrderQty");
            UtilityOrderDetails.GETInstance.DERVMaxOrderValue = parserOS.GetSetting("SINGLE ORDER LIMIT", "DerMaxOrderValue");
            UtilityOrderDetails.GETInstance.CURRMaxQty = parserOS.GetSetting("SINGLE ORDER LIMIT", "CurrencyMaxOrderQty");
            UtilityOrderDetails.GETInstance.CURRMinQty = parserOS.GetSetting("SINGLE ORDER LIMIT", "CurrencyMinOrderQty");
            UtilityOrderDetails.GETInstance.CURRMaxOrderValue = parserOS.GetSetting("SINGLE ORDER LIMIT", "CurrencyMaxOrderValue");
            if (Convert.ToBoolean(parserOS.GetSetting("GENERAL OS", "WarningChecked"))) { UtilityOrderDetails.GETInstance.clientIdAllowed = 'W'; }
            if (Convert.ToBoolean(parserOS.GetSetting("GENERAL OS", "NotAllowedChecked"))) { UtilityOrderDetails.GETInstance.clientIdAllowed = 'N'; }

            if (Convert.ToBoolean(parserOS.GetSetting("GENERAL OS", "AllowedChecked"))) { UtilityOrderDetails.GETInstance.clientIdAllowed = 'A'; }

            setMargin();
            indicesBroadCast += objIndicesBroadCastProcessor_OnBroadCastRecievedNew;
        }

        private void setMargin()
        {
            try
            {
                int screenWidth = Convert.ToInt32(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width.ToString());
                int screenHeight = Convert.ToInt32(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height.ToString());
                int Top = screenHeight - 435;
                int Left = screenWidth - 350;
                sensexMargin = string.Format("{0},{1},0,0", Left, Top);
            }
            catch (Exception e)
            {
                ExceptionUtility.LogError(e);
                //throw;
            }

        }

        public void RegisterGlobalKeys()
        {
            if (ShortCutKeysFlag != null || ShortCutKeysFlag != string.Empty)
            {
                if (ShortCutKeysFlag == "0")
                {
                    //A Radio

                    ttLogin = "Login (F1)";

                    ttSaudas = "Saudas (Shift + F1)";

                    ttTouchline = "Touchline (F2)";
                    OrderEntryShortKeyBuy = "Order Entry Buy (F3)";
                    ttOrderEntryShortKeyBuy = "Order Entry Buy (F3)";
                    OrderEntryShortKeySell = "Order Entry Sell (F3)";
                    ttOrderEntryShortKeySell = "Order Entry Sell (F3)";
                    PendingOrderHeader = "Pending Order (F4)";
                    ttPendingOrder = "PendingOrder (F4)";
                    ttReturnedOrder = "Returned Order (F5)";
                    ttOrderEntryBuy = "Buy Order Entry (+)";
                    ttOrderEntrySell = "Sell Order Entry (-)";
                }
                else if (ShortCutKeysFlag == "1")
                {

                    ttPendingOrder = "PendingOrder (F3)";
                    PendingOrderHeader = "Pending Order (F3)";
                    ttSaudas = "Saudas (F8)";
                    ttTouchline = "Touchline (F4)";
                    ttReturnedOrder = "Returned Order (F5)";
                    ttLogin = "Login (Ctrl + L)";
                    OrderEntryShortKeyBuy = "Order Entry Buy (F1)";
                    ttOrderEntryShortKeyBuy = "Order Entry Buy (F1)";
                    OrderEntryShortKeySell = "Order Entry Sell (F2)";
                    ttOrderEntryShortKeySell = "Order Entry Sell (F2)";
                }
            }

            //Common Keys

            ttScripHelp = "Scrip Help  (Ctrl + F1)";
            ttTradeLimits = "Trade Limits (Ctrl + F9)";
            ttCorporateAction = "Corporate Action (Ctrl + F8)";
            ttWebApps = "Web Apps (Ctrl + B)";
            ttMarketReports = "Market Reports (F7)";
            //Shift Keys
            ttOpenInterest = "Open Interest (Shift + F3)";
            ttNews = "News (Shift + F6)";
            ttIndexDetails = "Index Details (Shift + F7)";
            ttNetposScripWise = "Net Position Scrip Wise (Shift + F9)";
            ttVARPercetage = "VAR Percetage (Shift + V)";
            ttNetposClientWise = "Net Position Client Wise (Shift + ' (Acute))";
            ttSessionBroadcast = "Session Broadcast (Shift + F11)";
            //Alt Keys
            ttScripProfiling = "Setting (Shift + F12)";

        }

        //public static void SubscribeVar(int Segment)
        //{
        //    if (Segment == 0)
        //        EqtListener.VarEquityTick += EqtListener_VarTick;
        //}

        public static void SubscribeSession(int Segment)
        {
            if (Segment == 0)
                EqtListener.SessionCurrencyTick += EqtListener_SessionTick;
            if (Segment == 1)
                DerivativeListener.SessionDerivativeTick += EqtListener_SessionTick;
            if (Segment == 2)
                CurrencyListener.SessionCurrencyTick += EqtListener_SessionTick;
        }

        public static void SubscribeNews(int Segment)
        {
            if (Segment == 0)
                EqtListener.NewsEquityTick += EqtListener_NewsTick;
            if (Segment == 1)
                DerivativeListener.NewsDerivativeTick += EqtListener_NewsTick;
            if (Segment == 2)
                CurrencyListener.NewsCurrencyTick += EqtListener_NewsTick;
        }

        private static void EqtListener_NewsTick()
        {
            try
            {
                if ((NewsMemory.SubscribeNewsMemoryDict.Count > 0 || isfirstnews == true) && Globals.NewsWindowOpen == true)
                {
                    objNewsBroadcastProcessor_OnBroadCastRecievedNew();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally { }
        }

        private static void EqtListener_SessionTick()
        {
            try
            {
                if (SessionMemory.SubscribeSessionMemoryDict.Count > 0)
                {
                    objSessionBroadcastProcessor_OnBroadCastRecievedNew();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally { }
        }

        private static void EqtListener_VarTick()
        {
            try
            {
                if (VarMemory.SubscribeVarMemoryDict.Count > 0)
                {
                    objvarPercentageBroadcastProcessor_OnBroadCastRecievedNew();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally { }
        }


        private void SetResponceReplyText(ChangePasswordReply oChangePasswordReply)
        {
            if (oChangePasswordReply.ReplyCode == 0)
            {
                UtilityLoginDetails.GETInstance.DecryptedPassword = ChangePasswordVM.NewPasswordChangePwd;
                string var = Encrypt(ChangePasswordVM.NewPasswordChangePwd.ToCharArray(), key);
                parser.AddSetting("Login Settings", "PASSWORD", var);
                parser.SaveSettings(TwsINIPath.ToString());
                ChangePasswordVM.ReplyTextBox = oChangePasswordReply.ReplyMsg;
                if (InvokeTrade)
                    SendTradeRequest();
                InvokeTrade = false;
            }
            else if (oChangePasswordReply.ReplyCode == 1)
            {
                ChangePasswordVM.ReplyTextBox = oChangePasswordReply.ReplyMsg;
            }
            //ChangePasswordVM.TxtReply = txtRply.MessageTag;
            //return ChangePasswordVM.TxtReply;
        }

        private void ResetPasswordReplyText(ChangePasswordReply oResetPasswordReply)
        {
            if (oResetPasswordReply.ReplyCode == 0)
            {
                TraderPasswordResetVM.txtReply = oResetPasswordReply.ReplyMsg;
            }
            else if (oResetPasswordReply.ReplyCode == 1)
            {
                TraderPasswordResetVM.txtReply = oResetPasswordReply.ReplyMsg;
            }
        }

        public static void LoadNPProgressBar(bool MainWindowLoaded)
        {
            if (System.Windows.Application.Current.MainWindow.IsLoaded)
            {
                View.Trade.PersonalDownloadProgessWindow oPersonalDownloadProgessWindow = System.Windows.Application.Current.Windows.OfType<View.Trade.PersonalDownloadProgessWindow>().FirstOrDefault();
                if (oPersonalDownloadProgessWindow == null)
                {
                    oPersonalDownloadProgessWindow = new View.Trade.PersonalDownloadProgessWindow();
                    oPersonalDownloadProgessWindow.Activate();
                    oPersonalDownloadProgessWindow.Show();
                }
                else
                {

                    if (!oPersonalDownloadProgessWindow.IsVisible)
                    {
                        oPersonalDownloadProgessWindow.Focus();
                        oPersonalDownloadProgessWindow.Show();
                    }
                    //oPersonalDownloadProgessWindow.Show();
                }
            }
        }


        //private void UpdateRBIDetails()
        //{
        //    try
        //    {
        //        while (true)
        //        {
        //            if (RBIMemory.SubscribeRBIMemoryQueue.Count > 0)
        //            {
        //                objRBIBroadcastProcessor_OnBroadCastRecievedNew();
        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e);
        //    }
        //    finally { }
        //}






        //#define LOGONSESSION  0
        //#define PREOPENING 1
        //#define PREOPENING Matching        2
        //#define NORMAL        3
        //#define CLOSING       4
        //#define POSTCLOSING   5
        //#define Trading Halted 6
        //#define MemberQuery 7
        //#define Broker UP OPENING  8
        //#define Broker UP QUERY    9

        public static void objSessionBroadcastProcessor_OnBroadCastRecievedNew()
        {
            try
            {

                foreach (SessionMain Br in SessionMemory.SubscribeSessionMemoryDict.Values)

                {
                    SessionModel objSessionModel = new SessionModel();
                    if (Br.MessType == 2002)
                    {
                        objSessionModel.MsgType = "Session Broadcast";
                    }
                    else
                    {
                        objSessionModel.MsgType = "Auction Broadcast";
                    }
                    objSessionModel.Time = Br.Hour + ":" + Br.Minute + ":" + Br.Second;
                    objSessionModel.ProductId = Br.ProductID;
                    // objSessionModel.SessionNo = Br.SessionNumber;
                    objSessionModel.MarketType = Br.MarketType;
                    objSessionModel.SessionNo = Br.SessionNumber;
                    objSessionModel.StartEndFlag = Br.StartEndFlag;

                    switch (objSessionModel.MarketType)
                    {
                        case 0:
                            if (PreviousSessionNumber != Br.SessionNumber)
                            {
                                if (Br.SessionNumber == 0)
                                {
                                    if (PreviousMarketType == -1 && PreviousSessionNumber == -1)
                                    {
                                        objSessionModel.SessionType = "Logon";
                                    }
                                    else
                                    {
                                        objSessionModel.SessionType = "Normal Call auction, SPOS Order Entry Session start";
                                    }
                                    PreviousSessionNumber = 0;
                                }
                                if (Br.SessionNumber == 2)
                                {
                                    objSessionModel.SessionType = "End of Matching Session of Normal Call auction";
                                    PreviousSessionNumber = 2;
                                }
                                if (Br.SessionNumber == 3)
                                {
                                    objSessionModel.SessionType = "Continuous Session";
                                    PreviousSessionNumber = 3;
                                }
                                if (Br.SessionNumber == 4)
                                {
                                    objSessionModel.SessionType = "Closing";
                                    PreviousSessionNumber = 4;
                                }
                                if (Br.SessionNumber == 5)
                                {
                                    objSessionModel.SessionType = "Post Closing session";
                                    PreviousSessionNumber = 5;
                                }
                                if (Br.SessionNumber == 6)
                                {
                                    objSessionModel.SessionType = "End of day";
                                    PreviousSessionNumber = 6;
                                }
                                if (Br.SessionNumber == 7)
                                {
                                    objSessionModel.SessionType = "Member Query Session";
                                    PreviousSessionNumber = 7;
                                }
                                if (Br.SessionNumber == 10)
                                {
                                    objSessionModel.SessionType = "Random End of SPOS Order Entry Session[Freeze Session]";
                                    PreviousSessionNumber = 10;
                                }
                                if (Br.SessionNumber == 12)
                                {
                                    objSessionModel.SessionType = "End of Matching Session of SPOS";
                                    PreviousSessionNumber = 12;
                                }
                                if (Br.SessionNumber == 13)
                                {
                                    objSessionModel.SessionType = "Continuous Session for SPOS";
                                    PreviousSessionNumber = 13;
                                }
                                PreviousMarketType = 0;
                                PrevMarketType = objSessionModel.MarketType;
                                PrevSessionNumber = objSessionModel.SessionNo;
                                if (Globals.SessionWindowOpen == true)
                                {
                                    uiContext.Send(x => SessionVM.ObjSessionBroadCastCollection.Add(objSessionModel), null);
                                }
                                else
                                {
                                    SessionVM.ObjSessionBroadCastCollection.Add(objSessionModel);
                                }
                            }
                            break;
                        case 20:

                            if (previousSessionPCAS != Br.SessionNumber)
                            {
                                if (Br.SessionNumber == 1)
                                {
                                    if (objSessionModel.StartEndFlag == 'S')
                                    {
                                        objSessionModel.SessionType = "PCAS session  Order Entry Session";
                                        //  objSessionModel.SessionType = "PCAS session 2 Order Entry Session[Freeze Session]";
                                    }
                                    if (objSessionModel.StartEndFlag == 'E')
                                    {
                                        objSessionModel.SessionType = "Random End of PCAS session  Order Entry Session [Freeze Session]";
                                        //objSessionModel.SessionType = "Random End of PCAS session 2 Order Entry Session";
                                    }
                                    PreviousSessionNumber = 1;
                                }
                                if (Br.SessionNumber == 2)
                                {
                                    if (objSessionModel.StartEndFlag == 'E')
                                    {
                                        objSessionModel.SessionType = "End of Matching Session of PCAS session ";
                                    }
                                    PreviousSessionNumber = 2;
                                }
                                previousSessionPCAS = 20;


                                if (Globals.SessionWindowOpen == true)
                                {
                                    uiContext.Send(x => SessionVM.ObjSessionBroadCastCollection.Add(objSessionModel), null);
                                }
                                else
                                {
                                    SessionVM.ObjSessionBroadCastCollection.Add(objSessionModel);
                                }
                            }
                            break;

                    }

                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
            }
        }

        public static void objNewsBroadcastProcessor_OnBroadCastRecievedNew()
        {
            try
            {
                NewsModel objNewsModel;

                for (int i = 0; i < NewsMemory.SubscribeNewsMemoryDict.Count; i++)
                {
                    objNewsModel = new NewsModel();
                    objNewsModel.NewsCategory = NewsMemory.SubscribeNewsMemoryDict[i].NewsCategory;
                    objNewsModel.NewsId = NewsMemory.SubscribeNewsMemoryDict[i].NewsID;
                    objNewsModel.NewsHeadline = NewsMemory.SubscribeNewsMemoryDict[i].NewsHeadline;
                    string url = "http://10.1.101.125:3000/twsreports/exchangeAnn_news.aspx?id=" + objNewsModel.NewsId;
                    //    ProcessStartInfo sInfo = new ProcessStartInfo(url);
                    //  Process.Start(sInfo);
                    uiContext.Send(x => NewsVM.ObjNewsCollection.Add(objNewsModel), null);

                }

            }
            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
            }


        }



        //public static void objRBIBroadcastProcessor_OnBroadCastRecievedNew()
        //{
        //    try
        //    {

        //        BroadcastReceiver.RBIMain Br;
        //        Br = RBIMemory.SubscribeRBIMemoryQueue.Dequeue();

        //        for (int i = 0; i < Br.NoOfRec; i++)
        //        {
        //            List<int> list = new List<int>();
        //            if (RBIReferanceRateVM.ObjRBIReferenceRateCollection != null)
        //            {
        //                list = RBIReferanceRateVM.ObjRBIReferenceRateCollection.Where(X => X.UnderlyingAssetId == Br.RBIMainDetailsObj[i].UnderlyingAssetId).Select(x => RBIReferanceRateVM.ObjRBIReferenceRateCollection.IndexOf(x)).ToList();
        //            }
        //            if (list.Count != 0)
        //            {
        //                RBIReferanceRateVM.ObjRBIReferenceRateCollection[list[0]].UnderlyingAssetId = Br.RBIMainDetailsObj[i].UnderlyingAssetId;
        //                RBIReferanceRateVM.ObjRBIReferenceRateCollection[list[0]].RBIRate = Br.RBIMainDetailsObj[i].RBIRate;
        //                RBIReferanceRateVM.ObjRBIReferenceRateCollection[list[0]].Date = Br.RBIMainDetailsObj[i].Date;

        //            }
        //            else
        //            {
        //                RBIReferanceRateModel objRBIReferanceRateModel = new RBIReferanceRateModel();
        //                objRBIReferanceRateModel.UnderlyingAssetId = Br.RBIMainDetailsObj[i].UnderlyingAssetId;
        //                objRBIReferanceRateModel.RBIRate = Br.RBIMainDetailsObj[i].RBIRate;
        //                objRBIReferanceRateModel.Date = Br.RBIMainDetailsObj[i].Date;

        //                uiContext.Send(x => RBIReferanceRateVM.ObjRBIReferenceRateCollection.Add(objRBIReferanceRateModel), null);
        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionUtility.LogError(ex);
        //    }

        //    //}
        //}

        public static void objvarPercentageBroadcastProcessor_OnBroadCastRecievedNew()
        {
            try
            {

                //   BroadcastReceiver.VarMain Br;

                //uiContext = SynchronizationContext.Current;


                foreach (VarMainDetails Br in VarMemory.SubscribeVarMemoryDict.Values)
                {
                    if (Br != null)
                    {
                        //for (int i = 0; i < Br.NoOfRec; i++)
                        {
                            List<int> list = new List<int>();
                            if (VarPercentageBroadcastVM.ObjVarPercentageBroadcastCollection != null)
                            {
                                list = VarPercentageBroadcastVM.ObjVarPercentageBroadcastCollection.Where(X => X.InstrumentCode == Br.InstrumentCode).Select(x => VarPercentageBroadcastVM.ObjVarPercentageBroadcastCollection.IndexOf(x)).ToList();
                            }
                            if (list.Count != 0)
                            {
                                if (Br.Identifier == 'E')
                                {
                                    VarPercentageBroadcastVM.ObjVarPercentageBroadcastCollection[list[0]].ScripName = CommonFunctions.GetScripName(Br.InstrumentCode, Enumerations.Exchange.BSE, Enumerations.Segment.Equity);
                                }
                                else
                                {
                                    VarPercentageBroadcastVM.ObjVarPercentageBroadcastCollection[list[0]].ScripName = CommonFunctions.GetScripName(Br.InstrumentCode, Enumerations.Exchange.BSE, Enumerations.Segment.Derivative);
                                }

                                if (Br.IMPercentage != 0)
                                    VarPercentageBroadcastVM.ObjVarPercentageBroadcastCollection[list[0]].VARIMPercetage = string.Format("{0:0.00}", Convert.ToDouble(Br.IMPercentage) / 100);
                                if (Br.ELMPercentage != 0)
                                    VarPercentageBroadcastVM.ObjVarPercentageBroadcastCollection[list[0]].ELMVARPercentage = string.Format("{0:0.00}", Convert.ToDouble(Br.ELMPercentage) / 100);
                                VarPercentageBroadcastVM.ObjVarPercentageBroadcastCollection[list[0]].Identifier = Br.Identifier;
                            }
                            else
                            {
                                VarPercentageBroadcastModel objVarPercentageBroadcastModel = new VarPercentageBroadcastModel();
                                if (Br.Identifier == 'E')
                                {
                                    objVarPercentageBroadcastModel.ScripName = CommonFunctions.GetScripName(Br.InstrumentCode, Enumerations.Exchange.BSE, Enumerations.Segment.Equity);
                                    objVarPercentageBroadcastModel.ScripID = CommonFunctions.GetScripId(Br.InstrumentCode, Enumerations.Exchange.BSE, Enumerations.Segment.Equity);
                                }
                                else
                                {
                                    objVarPercentageBroadcastModel.ScripName = CommonFunctions.GetScripName(Br.InstrumentCode, Enumerations.Exchange.BSE, Enumerations.Segment.Derivative);
                                    objVarPercentageBroadcastModel.ScripID = CommonFunctions.GetScripId(Br.InstrumentCode, Enumerations.Exchange.BSE, Enumerations.Segment.Derivative);
                                }
                                objVarPercentageBroadcastModel.InstrumentCode = Br.InstrumentCode;
                                objVarPercentageBroadcastModel.VARIMPercetage = string.Format("{0:0.00}", Convert.ToDouble(Br.IMPercentage) / 100);
                                objVarPercentageBroadcastModel.ELMVARPercentage = string.Format("{0:0.00}", Convert.ToDouble(Br.ELMPercentage) / 100);
                                objVarPercentageBroadcastModel.Identifier = Br.Identifier;
                                lock (VarPercentageBroadcastVM.ObjVarPercentageBroadcastCollection)
                                {
                                    if (VarPercentageBroadcastVM.ObjVarPercentageBroadcastCollection.Any(x => x.InstrumentCode == objVarPercentageBroadcastModel.InstrumentCode))
                                    {
                                        int index = VarPercentageBroadcastVM.ObjVarPercentageBroadcastCollection.IndexOf(VarPercentageBroadcastVM.ObjVarPercentageBroadcastCollection.Where(x => x.InstrumentCode == objVarPercentageBroadcastModel.InstrumentCode).FirstOrDefault());
                                        VarPercentageBroadcastVM.ObjVarPercentageBroadcastCollection[index] = objVarPercentageBroadcastModel;
                                    }
                                    else
                                    {
                                        VarPercentageBroadcastVM.ObjVarPercentageBroadcastCollection.Add(objVarPercentageBroadcastModel);
                                    }

                                    //if (Globals.VarWindowOpen == true)
                                    //{
                                    //    uiContext.Send(x => VarPercentageBroadcastVM.ObjVarPercentageBroadcastCollection.Add(objVarPercentageBroadcastModel), null);
                                    //}
                                    //else
                                    //{
                                    //    VarPercentageBroadcastVM.ObjVarPercentageBroadcastCollection.Add(objVarPercentageBroadcastModel);
                                    //}
                                }

                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
            }

            //}
        }


        private static void EqtListener_IndicesTick()
        {
            try
            {
                foreach (BroadcastReceiver.IdicesDetailsMain Br in SubscribeIndicesMemory.SubscribeIndiceDict.Values)
                {
                    //Br = SubscribeIndicesMemory.SubscribeIndiceQueue
                    //  Br = SubscribeIndicesMemory.SubscribeIndiceQueue.Dequeue();
                    if (Br != null)
                    {
                        Model.IdicesDetailsMain objIndexMain;
                        //Model.IdicesDetailsMain objIdicesDetailsMain;
                        objIndexMain = UpdateIndicesDatafromMemory(Br);
                        if (indicesBroadCast != null)
                        {
                            indicesBroadCast((Model.IdicesDetailsMain)objIndexMain);
                        }
                        //indicesBroadCast += indexDetailsVM.objIndicesBroadCastProcessor_OnBroadCastRecievedNew;
                        //objIndexMain = UpdateIndicesDatafromMemory(Br);
                        //add in dict 
                        //indexDetailsVM.objIndicesBroadCastProcessor_OnBroadCastRecievedNew(objIndexMain);
                        //if(BroadcastMasterMemory.objIndexDetailsConDict.Keys.Contains(objIndexMain.listindMain.))
                        //BroadcastMasterMemory.objIndexDetailsConDict.AddOrUpdate(objIndexMain)
                    }



                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally { }
        }

        public static void UpdateMktPicScripDetail()
        {
            try
            {
                while (true)
                {
                    //_pauseEvent.WaitOne(Timeout.Infinite);

                    if (SubscribeScripMemory.SubscribeScripQueue.Count > 0)
                    {
                        BroadcastReceiver.ScripDetails Br;
                        Br = SubscribeScripMemory.SubscribeScripQueue.Dequeue();
                        if (Br != null)
                        {
                            Model.ScripDetails s;
                            s = UpdateScripDataFromMemory(Br);
                            MarketWatchVM.objBroadCastProcessor_OnBroadCastRecievedNew(s);
                            BestFiveVM.objBroadCastProcessor_OnBroadCastRecieved(s);
                            if (NormalOrderEntryVM.OETouchlineValue != null)
                            {
                                NormalOrderEntryVM.OETouchlineValue?.Invoke(s);
                                NormalOrderEntryVM.GetInstance.objBroadCastProcessor_OnBroadCastRecievedNew(s);
                            }
                        }
                    }
                    else
                        Thread.Sleep(500);
                }
            }
            catch (Exception e)
            {
                ExceptionUtility.LogError(e);
            }
            finally
            {

            }
        }

        private static TradeRequest _tradeRequest;

        private static TradeRequestProcessor _tradeRequestProcessor;

        public static void MemoryManager_OnLogonReplyReceived(LogonReply oLogonReply)
        {
            LoginScreenVM.txtMemberEnability = false;
            LoginScreen loginscreen = System.Windows.Application.Current.Windows.OfType<LoginScreen>().SingleOrDefault();
            mwindow = System.Windows.Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            //TwsMainWindowVM.LoginFlag = 1;
            parser.AddSetting("Login Settings", "MEMBERID", UtilityLoginDetails.GETInstance.MemberId.ToString());
            parser.AddSetting("Login Settings", "TRADERID", UtilityLoginDetails.GETInstance.TraderId.ToString());
            parser.AddSetting("Login Settings", "EQCheck", LoginScreenVM.EquitySegChk.ToString());
            parser.AddSetting("Login Settings", "DrCheck", LoginScreenVM.DerSegChk.ToString());
            parser.AddSetting("Login Settings", "CurCheck", LoginScreenVM.CurSegChk.ToString());
            parser.AddSetting("Login Settings", "EQBrCheck", LoginScreenVM.EquityBrdChk.ToString());
            parser.AddSetting("Login Settings", "DrBrCheck", LoginScreenVM.DerBrdChk.ToString());
            parser.AddSetting("Login Settings", "CurBrCheck", LoginScreenVM.CurBrdChk.ToString());
            parser.AddSetting("Login Settings", "BYPASSLOGIN", "1");

            parser.SaveSettings(TwsINIPath.ToString());
            //if (loginscreen != null)
            //{
            //    loginscreen.Closing -= Login_Window_Closing_Event;
            //    loginscreen.Close();
            //}

            //LoginFlag = 1; // 1 for success login

            LoginScreenVM.CountLogin++;
            LogonReply oLogonReplyy = new LogonReply();
            oLogonReplyy = oLogonReply as LogonReply;

            UtilityLoginDetails.GETInstance.SenderLocationId = MainWindowVM.parser.GetSetting("Login Settings", string.Format("LOCID{0}{1}", UtilityLoginDetails.GETInstance.MemberId.ToString(), UtilityLoginDetails.GETInstance.TraderId.ToString()));
            UtilityOrderDetails.GETInstance.DefaultOrderEntry = MainWindowVM.parserOS.GetSetting("GENERAL OS", "SelectedOrderEntry");

            UtilityOrderDetails.GETInstance.CurrentOrderEntry = UtilityOrderDetails.GETInstance.DefaultOrderEntry;
            if (UtilityOrderDetails.GETInstance.DefaultOrderEntry == null)
            {
                UtilityOrderDetails.GETInstance.DefaultOrderEntry = Enumerations.OrderEntryWindow.Normal.ToString();
                UtilityOrderDetails.GETInstance.CurrentOrderEntry = UtilityOrderDetails.GETInstance.DefaultOrderEntry;
            }

            //UtilityOrderDetails.GETInstance.MktProtection = MainWindowVM.parserOS.GetSetting("GENERAL OS", "MarketProtection");

            //UtilityLoginDetails.GETInstance.TodaysDateTime = new DateTime(oLogonReplyy.Year, oLogonReplyy.Month, oLogonReplyy.Day, oLogonReplyy.Hour, oLogonReplyy.Minute, oLogonReplyy.Second, oLogonReplyy.Msecond, DateTimeKind.Local);

            System.Windows.Threading.DispatcherTimer myDispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            myDispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 1000);
            myDispatcherTimer.Tick += MyDispatcherTimer_Tick;
            myDispatcherTimer.Start();


            SettingsManager.Initialize();
            LoginProcessor oLoginProcessor = new LoginProcessor();

            TradeRequest oTradeRequest = new TradeRequest();
            AssignRole();
            MenuVisibility();

            if (oLogonReplyy != null)
            {
                if (oLogonReplyy.ReplyCode != 1)
                {
                    LoadAllIndicesWindowClick();
                }

            }
            if (oLogonReplyy != null)
            {
                MessageBoxResult oMessageBoxResult;
                //TODO Gaurav logon Response
                if (oLogonReply.ReplyCode == 0)//All segment Success
                {
                    UtilityLoginDetails.GETInstance.IsLoggedIN = true;
                    if (loginscreen != null)
                    {
                        loginscreen.Closing -= Login_Window_Closing_Event;
                        loginscreen.Close();
                    }

                }

                else if (oLogonReply.ReplyCode == 1)// All Segment login failed
                {
                    string repliedSegment = "";
                    if (LoginScreenVM.EquitySegChk == true)
                    {
                        repliedSegment += "Equity ";
                        if (LoginScreenVM.DerSegChk == true || LoginScreenVM.CurSegChk == true)
                            repliedSegment += "& ";
                    }
                    if (LoginScreenVM.DerSegChk == true)
                    {
                        repliedSegment += "Derivative ";
                        if (LoginScreenVM.CurSegChk == true)
                            repliedSegment += "& ";
                    }
                    if (LoginScreenVM.CurSegChk == true)
                    {
                        repliedSegment += "Currency ";
                    }
                    oMessageBoxResult = System.Windows.MessageBox.Show("Connection to " + repliedSegment + " failed", "Warning!", MessageBoxButton.OK, MessageBoxImage.Error);
                    //  UtilityLoginDetails.GETInstance.IsEQXChecked = false;
                    //     UtilityLoginDetails.GETInstance.IsCURChecked = false;
                    // UtilityLoginDetails.GETInstance.IsDERChecked = false;
                    UtilityLoginDetails.GETInstance.IsLoggedIN = false;
                    if (MessageBoxResult.No == oMessageBoxResult)
                    {
                        process = Process.GetProcessesByName("imlPro").FirstOrDefault();
                        if (process != null)
                            process.Kill();
                        Process.GetCurrentProcess().Kill();
                    }
                    else
                    {
                        if (loginscreen != null)
                            loginscreen?.Focus();
                        else
                        {
                            loginscreen = new LoginScreen();
                            loginscreen.Activate();
                            loginscreen.Show();
                        }
                    }
                    LoginScreenVM.GETInstanceLogin.txtLoginEnability = true;
                    return;
                }

                if (oLogonReply.ReplyCode != 1)
                {
                    //Start Thread When the Reply code is other than 1.i.e Connection authnetication failed
                    LoadDigitalClock();
                    StartBroadCast();

                }
                //Bcast is allowed for each segment for Now
                if (oLogonReply.ReplyCode == -1)// Equity Failed
                {
                    UtilityLoginDetails.GETInstance.IsEqConnected = false;
                    //UtilityLoginDetails.GETInstance.IsEQXChecked = false;
                    UtilityLoginDetails.GETInstance.IsLoggedIN = true;
                    CommonFunctions.setLoginStatus((int)Enumerations.Segment.Equity, false);
                    oMessageBoxResult = System.Windows.MessageBox.Show("Connection to Equity failed.\n Do you want to continue?", "Warning!", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (MessageBoxResult.No == oMessageBoxResult)
                    {
                        //Required to logoff all segment

                        LoginScreenVM.GETInstanceLogin.Btn_LogOff_Click(null);

                    }
                    else if (MessageBoxResult.Yes == oMessageBoxResult)
                    {
                        if (loginscreen != null)
                        {
                            loginscreen.Closing -= Login_Window_Closing_Event;
                            loginscreen.Close();
                        }
                    }

                }
                else if (oLogonReply.ReplyCode == -2)// Currency Login FAILED
                {
                    oMessageBoxResult = System.Windows.MessageBox.Show("Connection to Currency failed.\n Do you want to continue?", "Warning!", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    //UtilityLoginDetails.GETInstance.IsEQXloggedIn = false;
                    UtilityLoginDetails.GETInstance.IsLoggedIN = true;
                    //UtilityLoginDetails.GETInstance.IsCURChecked = false;
                    UtilityLoginDetails.GETInstance.IsCurrConnected = false;
                    //UtilityLoginDetails.GETInstance.IsLoggedIN = true;
                    if (MessageBoxResult.No == oMessageBoxResult)
                    {
                        //Required to logoff all segment
                        LoginScreenVM.GETInstanceLogin.Btn_LogOff_Click(null);
                    }
                    else if (MessageBoxResult.Yes == oMessageBoxResult)
                    {
                        if (loginscreen != null)
                        {
                            loginscreen.Closing -= Login_Window_Closing_Event;
                            loginscreen.Close();
                        }

                    }
                    CommonFunctions.setLoginStatus((int)Enumerations.Segment.Currency, false);
                }
                else if (oLogonReply.ReplyCode == -3)// Derivative Login FAILED
                {
                    oMessageBoxResult = System.Windows.MessageBox.Show("Connection to Derivative failed.\n Do you want to continue?", "Warning!", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    //UtilityLoginDetails.GETInstance.IsDERChecked = false;
                    UtilityLoginDetails.GETInstance.IsDerConnected = false;
                    UtilityLoginDetails.GETInstance.IsLoggedIN = true;
                    if (MessageBoxResult.No == oMessageBoxResult)
                    {
                        //Required to logoff all segment
                        LoginScreenVM.GETInstanceLogin.Btn_LogOff_Click(null);
                    }
                    else if (MessageBoxResult.Yes == oMessageBoxResult)
                    {
                        if (loginscreen != null)
                        {
                            loginscreen.Closing -= Login_Window_Closing_Event;
                            loginscreen.Close();
                        }

                    }
                    CommonFunctions.setLoginStatus((int)Enumerations.Segment.Derivative, false);
                }
                else if (oLogonReply.ReplyCode == -4)//Equity & Equity Derivative failed
                {
                    oMessageBoxResult = System.Windows.MessageBox.Show("Connection to Equity & Derivative failed.\n Do you want to continue?", "Warning!", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    //UtilityLoginDetails.GETInstance.IsEQXChecked = false;
                    // UtilityLoginDetails.GETInstance.IsDERChecked = false;

                    UtilityLoginDetails.GETInstance.IsEqConnected = false;
                    UtilityLoginDetails.GETInstance.IsDerConnected = false;

                    UtilityLoginDetails.GETInstance.IsLoggedIN = true;
                    if (MessageBoxResult.No == oMessageBoxResult)
                    {
                        //Required to logoff all segment
                        LoginScreenVM.GETInstanceLogin.Btn_LogOff_Click(null);
                    }
                    else if (MessageBoxResult.Yes == oMessageBoxResult)
                    {
                        if (loginscreen != null)
                        {
                            loginscreen.Closing -= Login_Window_Closing_Event;
                            loginscreen.Close();
                        }

                    }
                    CommonFunctions.setLoginStatus((int)Enumerations.Segment.Equity, false);
                    CommonFunctions.setLoginStatus((int)Enumerations.Segment.Derivative, false);
                }
                else if (oLogonReply.ReplyCode == -5)// Currency & Equity Derivatives failed 
                {
                    oMessageBoxResult = System.Windows.MessageBox.Show("Connection to Derivative & Currency failed.\n Do you want to continue?", "Warning!", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    // UtilityLoginDetails.GETInstance.IsDERChecked = false;
                    //UtilityLoginDetails.GETInstance.IsCURChecked = false;

                    UtilityLoginDetails.GETInstance.IsDerConnected = false;
                    UtilityLoginDetails.GETInstance.IsCurrConnected = false;

                    UtilityLoginDetails.GETInstance.IsLoggedIN = true;
                    if (MessageBoxResult.No == oMessageBoxResult)
                    {
                        //Required to logoff all segment
                        LoginScreenVM.GETInstanceLogin.Btn_LogOff_Click(null);
                    }
                    else if (MessageBoxResult.Yes == oMessageBoxResult)
                    {
                        if (loginscreen != null)
                        {
                            loginscreen.Closing -= Login_Window_Closing_Event;
                            loginscreen.Close();
                        }

                    }
                    CommonFunctions.setLoginStatus((int)Enumerations.Segment.Currency, false);
                    CommonFunctions.setLoginStatus((int)Enumerations.Segment.Derivative, false);
                }
                else if (oLogonReply.ReplyCode == -6)// Currency  & Equity failed
                {
                    oMessageBoxResult = System.Windows.MessageBox.Show("Connection to Equity & Currency failed.\n Do you want to continue?", "Warning!", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    //UtilityLoginDetails.GETInstance.IsEQXChecked = false;
                    //UtilityLoginDetails.GETInstance.IsCURChecked = false;

                    UtilityLoginDetails.GETInstance.IsEqConnected = false;
                    UtilityLoginDetails.GETInstance.IsCurrConnected = false;

                    UtilityLoginDetails.GETInstance.IsLoggedIN = true;
                    if (MessageBoxResult.No == oMessageBoxResult)
                    {
                        //Required to logoff all segment
                        LoginScreenVM.GETInstanceLogin.Btn_LogOff_Click(null);
                    }
                    else if (MessageBoxResult.Yes == oMessageBoxResult)
                    {
                        if (loginscreen != null)
                        {
                            loginscreen.Closing -= Login_Window_Closing_Event;
                            loginscreen.Close();
                        }
                    }

                    CommonFunctions.setLoginStatus((int)Enumerations.Segment.Equity, false);
                    CommonFunctions.setLoginStatus((int)Enumerations.Segment.Currency, false);

                }




                //Called before grace login
                AssignRole();
                //To be called before trade request Gaurav 24/04/2017
                MasterSharedMemory.ReadDPSetlMaster();
                MemoryManager.GetSettlementNoFromMaster();
                AssignTitle();
                LoginScreenVM.GETInstanceLogin.ControleEnability();

                if (oLogonReply.NormSettNo == 218 && oLogonReplyy.ReplyCode != 1)//Grace Login. Invoke change password window
                {
                    InvokeTrade = true;
                    oMessageBoxResult = MessageBox.Show("Password expired.Please change", "Change Password", MessageBoxButton.OK, MessageBoxImage.Warning);
                    UtilityLoginDetails.GETInstance.IsLoggedIN = true;
                    if (MessageBoxResult.OK == oMessageBoxResult)
                    {
                        //Open ChangePasswordWindow
                        OpenPasswordWindow();
                    }
                }
                else if (oLogonReply.NormSettNo != 218 && oLogonReplyy.ReplyCode != 1)
                {
                    SendTradeRequest();
                }

            }
        }

        private static void LoadDigitalClock()
        {
            CustomDigitalWindow objdigitalCLock = System.Windows.Application.Current.Windows.OfType<CustomDigitalWindow>().FirstOrDefault();

            if (objdigitalCLock != null)
            {
                objdigitalCLock.Activate();
                int screenWidth = Convert.ToInt32(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width.ToString());
                int screenHeight = Convert.ToInt32(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height.ToString());
                objdigitalCLock.Top = screenHeight - ((screenHeight * 90.9) / 100);
                objdigitalCLock.Left = screenWidth - ((screenWidth * 4.65) / 100);
                objdigitalCLock.Show();
            }

            else
            {
                objdigitalCLock = new CustomDigitalWindow();
                objdigitalCLock.Activate();
                int screenWidth = Convert.ToInt32(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width.ToString());
                int screenHeight = Convert.ToInt32(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height.ToString());
                objdigitalCLock.Top = screenHeight - ((screenHeight * 90.9) / 100);
                objdigitalCLock.Left = screenWidth - ((screenWidth * 4.65) / 100);
                objdigitalCLock.Show();
            }


        }

        private void objIndicesBroadCastProcessor_OnBroadCastRecievedNew(Model.IdicesDetailsMain obj)
        {
            if (objLogOnStatus[(int)Enumerations.Segment.Equity].isLoggedIn || objLogOnStatus[(int)Enumerations.Segment.Derivative].isLoggedIn
                || objLogOnStatus[(int)Enumerations.Segment.Currency].isLoggedIn)
            {
                //int IndexCode = MasterSharedMemory.objMasterDicSyb.Where(x => x.Value.IndexCode == "SENSEX").Select(z => z.Key).FirstOrDefault();

                if (SharedMemories.BroadcastMasterMemory.objIndexDetailsConDict != null)
                {
                    IndexValue = SharedMemories.BroadcastMasterMemory.objIndexDetailsConDict[UtilityLoginDetails.GETInstance.SensexIndexCode].IndexValue / Math.Pow(10, 2);
                    PrvClose = SharedMemories.BroadcastMasterMemory.objIndexDetailsConDict[UtilityLoginDetails.GETInstance.SensexIndexCode].PreviousIndexClose / Math.Pow(10, 2);
                    TrendDiff = IndexValue - PrvClose;
                    FixedSensexTxtBlock = string.Format("S&P BSE SENSEX");
                    CurrentValueTxtBlock = string.Format("{0:0.00}", IndexValue);


                    if (TrendDiff >= 0)
                    {
                        if (mwindow != null)
                            uiContext.Send(x => mwindow.trendImg.Source = new BitmapImage(new Uri(@"../Images/UpTrend.png", UriKind.Relative)), null);

                        ChangeInValueTxtBlock = string.Format("{0:0.00}", TrendDiff);
                        //imgTrend = "../Images/Up.png";
                    }
                    else
                    {
                        if (mwindow != null)
                            uiContext.Send(x => mwindow.trendImg.Source = new BitmapImage(new Uri(@"../Images/DownTrend.png", UriKind.Relative)), null);
                        //imgTrend = "../Images/Down.png";

                        TrendDiff = TrendDiff * -1;
                        ChangeInValueTxtBlock = string.Format("{0:0.00}", TrendDiff);
                    }
                }
            }
        }

        public static void StartBroadCast()
        {
            EqtListener.StartListener(1, (int)Enumerations.Segment.Equity); //0 - Tcp, 1 - Udp
            DerivativeListener.StartListener(1, (int)Enumerations.Segment.Derivative);
            CurrencyListener.StartListener(1, (int)Enumerations.Segment.Currency);
            BcastUpdateThread.Start();

            if (LoginScreenVM.EquityBrdChk)
            {
                SubscribeInces((int)Enumerations.Segment.Equity);
                SubscribeSession((int)Enumerations.Segment.Equity);
                SubscribeNews((int)Enumerations.Segment.Equity);
            }
            if (LoginScreenVM.DerBrdChk)
            {
                SubscribeSession((int)Enumerations.Segment.Derivative);
                SubscribeNews((int)Enumerations.Segment.Derivative);
            }
            if (LoginScreenVM.CurBrdChk)
            {
                SubscribeSession((int)Enumerations.Segment.Currency);
                SubscribeNews((int)Enumerations.Segment.Currency);
            }
        }

        public static void SubscribeInces(int Segment)
        {
            if (Segment == 0)
                EqtListener.IndicesTick += EqtListener_IndicesTick;
        }



        public static void SendTradeRequest()
        {
            _tradeRequest = new Model.Trade.TradeRequest();

            if (UtilityLoginDetails.GETInstance.Role == Role.Admin.ToString())
            {
                #region Commented
                //CommonFrontEnd.ViewModel.MainWindowVM.LoadNPProgressBar(true);
                //if (UtilityLoginDetails.GETInstance.IsEqConnected)//Equity
                //{
                //    _tradeRequest.AppBeginSequenceNum = 1;
                //    _tradeRequest.AppEndSequenceNum = 1;
                //    _tradeRequest.Exchange = 1;
                //    _tradeRequest.Market = 1;
                //    //_tradeRequest.MessageTag = MemoryManager.GetMesageTag();
                //    _tradeRequest.PartitionID = 1;
                //    _tradeRequest.ReservedField = 0;
                //    _tradeRequest.Hour = "0";
                //    _tradeRequest.Minute = "0";
                //    _tradeRequest.Second = "0";
                //    _tradeRequest.MessageTag = (uint)Enumerations.Trade.AdminTradeRequestMsgTag.EQTYSEGINDICATOR;
                //    _tradeRequestProcessor = new TradeRequestProcessor(new AdminTradePersonalDownload());
                //    _tradeRequestProcessor.ProcessRequest(_tradeRequest);
                //}
                ////Send request after response -gaurav
                //if (UtilityLoginDetails.GETInstance.IsDerConnected)//Derivative
                //{
                //    _tradeRequest.AppBeginSequenceNum = 1;
                //    _tradeRequest.AppEndSequenceNum = 1;
                //    _tradeRequest.Exchange = 1;
                //    _tradeRequest.Market = 2;
                //    //_tradeRequest.MessageTag = MemoryManager.GetMesageTag();
                //    _tradeRequest.PartitionID = 1;
                //    _tradeRequest.ReservedField = 0;
                //    _tradeRequest.Hour = "0";
                //    _tradeRequest.Minute = "0";
                //    _tradeRequest.Second = "0";
                //    _tradeRequest.MessageTag = (uint)Enumerations.Trade.AdminTradeRequestMsgTag.DERISEGINDICATOR;
                //    _tradeRequestProcessor = new TradeRequestProcessor(new AdminTradePersonalDownload());
                //    //_tradeRequestProcessor.ProcessRequest(_tradeRequest);
                //}
                //if (UtilityLoginDetails.GETInstance.IsCurrConnected)//Currency
                //{
                //    _tradeRequest.AppBeginSequenceNum = 1;
                //    _tradeRequest.AppEndSequenceNum = 1;
                //    _tradeRequest.Exchange = 1;
                //    _tradeRequest.Market = 3;
                //    //_tradeRequest.MessageTag = MemoryManager.GetMesageTag();
                //    _tradeRequest.PartitionID = 1;
                //    _tradeRequest.ReservedField = 0;
                //    _tradeRequest.Hour = "0";
                //    _tradeRequest.Minute = "0";
                //    _tradeRequest.Second = "0";
                //    _tradeRequest.MessageTag = (uint)Enumerations.Trade.AdminTradeRequestMsgTag.CURRSEGINDICATOR;
                //    _tradeRequestProcessor = new TradeRequestProcessor(new AdminTradePersonalDownload());
                //    // _tradeRequestProcessor.ProcessRequest(_tradeRequest);
                //}
                #endregion

                CommonFrontEnd.ViewModel.MainWindowVM.LoadNPProgressBar(true);

                Thread t1 = new Thread(new ThreadStart(PersonalDownloadVM.ProcessAdminPrsnlDwld));
                t1.Name = "PersonalDownloadThread";
                t1.Start();

            }
            else if (UtilityLoginDetails.GETInstance.Role == Role.Trader.ToString())
            {
                //Invoke BSE Bulletins Screen
                try
                {
                    View.BSEBulletin.BSEBulletinsBoard objBSEBulletinsBoard = new BSEBulletinsBoard();
                    objBSEBulletinsBoard.Activate();
                    objBSEBulletinsBoard.Show();
                }
                catch (Exception e)
                {
                    ExceptionUtility.LogError(e);
                }

                //Invoke Personal Download Screen 
                View.PersonalDownload.PersonalDownload opersonaldownload = new View.PersonalDownload.PersonalDownload();
                opersonaldownload.Activate();
                opersonaldownload.Show();

                CommonFrontEnd.ViewModel.MainWindowVM.LoadNPProgressBar(true);

                Thread t1 = new Thread(new ThreadStart(PersonalDownloadVM.ProcessTrdrPrsnlDwld));
                t1.Name = "PersonalDownloadThread";
                t1.Start();

            }
        }

        private static void LoadAllIndicesWindowClick()
        {

            All_Indices oAll_Indices = Application.Current.Windows.OfType<All_Indices>().FirstOrDefault();
            if (oAll_Indices != null)
            {
                if (oAll_Indices.WindowState == WindowState.Minimized)
                    oAll_Indices.WindowState = WindowState.Normal;
                int screenWidth = Convert.ToInt32(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width.ToString());
                int screenHeight = Convert.ToInt32(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height.ToString());
                oAll_Indices.Top = screenHeight - 338;
                oAll_Indices.Left = screenWidth - 290;
                oAll_Indices.Focus();
                oAll_Indices.Show();
            }
            else
            {
                oAll_Indices = new All_Indices();
                oAll_Indices.Owner = System.Windows.Application.Current.MainWindow;
                //objswift.CmbExcangeType.Focus();
                int screenWidth = Convert.ToInt32(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width.ToString());
                int screenHeight = Convert.ToInt32(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height.ToString());
                oAll_Indices.Top = screenHeight - 338;
                oAll_Indices.Left = screenWidth - 290;
                oAll_Indices.Activate();
                oAll_Indices.Show();
            }
        }

        public void loadcommonmessaging(bool MainWindowLoaded)
        {
            CommonMessagingWindow oCommonMessageWindow = System.Windows.Application.Current.Windows.OfType<CommonMessagingWindow>().FirstOrDefault();

            if (oCommonMessageWindow != null)
            {
                if (MainWindowLoaded)
                    oCommonMessageWindow.Owner = System.Windows.Application.Current.MainWindow;
                oCommonMessageWindow.ShowInTaskbar = false;
                oCommonMessageWindow.Activate();
                int screenWidth = Convert.ToInt32(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width.ToString());
                int screenHeight = Convert.ToInt32(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height.ToString());
                oCommonMessageWindow.Top = screenHeight - 190;
                oCommonMessageWindow.Left = screenWidth - screenWidth;
                oCommonMessageWindow.Show();
            }
            else
            {
                oCommonMessageWindow = new CommonMessagingWindow();
                if (MainWindowLoaded)
                    oCommonMessageWindow.Owner = System.Windows.Application.Current.MainWindow;
                oCommonMessageWindow.Activate();
                int screenWidth = Convert.ToInt32(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width.ToString());
                int screenHeight = Convert.ToInt32(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height.ToString());
                oCommonMessageWindow.Top = screenHeight - 190;
                oCommonMessageWindow.Left = screenWidth - screenWidth;
                oCommonMessageWindow.Show();
            }

            if (MainWindowLoaded)
            {
                System.Windows.Application.Current.MainWindow.Focus();
                //   AssignGlobalHotKeys();
                ByPassLogin = parser.GetSetting("Login Settings", "BYPASSLOGIN");
                if (ByPassLogin == "0")
                {
                    ReadTWIINI();
                }
                else if (ByPassLogin == "1")
                {
                    LoginScreen oLoginScreen = System.Windows.Application.Current.Windows.OfType<LoginScreen>().FirstOrDefault();
                    oLoginScreen = new LoginScreen();
                    oLoginScreen.Owner = System.Windows.Application.Current.MainWindow;
                    oLoginScreen.Activate();

                    //  oLoginScreen.MyPasswordBox.Focus();

                    oLoginScreen.Show();
                }

            }
        }

        private void InvokeIML()
        {
            try
            {
                process = Process.GetProcessesByName(IMLName).FirstOrDefault();
                if (process != null)
                    process.Kill();
                info = new ProcessStartInfo(IMLPath.ToString());
                info.Arguments = "tws";
                process = Process.Start(info);
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
            }

        }

        private static void AssignTitle()
        {
            if (UtilityLoginDetails.GETInstance.IsEQXChecked)//&& UtilityLoginDetails.GETInstance.IsDERloggedIn && UtilityLoginDetails.GETInstance.IsCURloggedIn
            {
                if (UtilityLoginDetails.GETInstance.SettlementNo != null)
                    TitleTWSMainWindow = "BOLTPro  -  Version 1.03      [Mem/Trader] - " + "[" + UtilityLoginDetails.GETInstance.MemberId + "/" + UtilityLoginDetails.GETInstance.TraderId + "]   " + UtilityLoginDetails.GETInstance.TodaysDateTime.ToString("dd MMMM yyyy") + "   EQ Sett.No : " + UtilityLoginDetails.GETInstance.SettlementNo.Split('/')[0];
                else
                    TitleTWSMainWindow = "BOLTPro  -  Version 1.03      [Mem/Trader] - " + "[" + UtilityLoginDetails.GETInstance.MemberId + "/" + UtilityLoginDetails.GETInstance.TraderId + "]   " + UtilityLoginDetails.GETInstance.TodaysDateTime.ToString("dd MMMM yyyy");
            }
            else
            {
                TitleTWSMainWindow = "BOLTPro  -  Version 1.03";
            }
        }

        protected static internal void AssignRole()
        {
            if (new[] { 0, 200 }.Any(traderId => traderId == UtilityLoginDetails.GETInstance.TraderId))
            {
                UtilityLoginDetails.GETInstance.Role = Role.Admin.ToString();
            }
            else
            {
                UtilityLoginDetails.GETInstance.Role = Role.Trader.ToString();
            }
        }

        public static void Login_Window_Closing_Event(object sender, CancelEventArgs obj)
        {
            //if (!UtilityLoginDetails.GETInstance.IsEQXloggedIn)//if (LoginFlag == 0)&& !UtilityLoginDetails.GETInstance.IsDERloggedIn && !UtilityLoginDetails.GETInstance.IsCURloggedIn
            //{
            //    if (!process.HasExited)
            //        process.Kill();
            //    Process.GetCurrentProcess().Kill();
            //}
            //LoginScreen oLoginScreen = System.Windows.Application.Current.Windows.OfType<LoginScreen>().FirstOrDefault();
            //if (oLoginScreen != null)
            //{
            //    oLoginScreen.Hide();
            //}
        }

        private static void MyDispatcherTimer_Tick(object sender, EventArgs e)
        {
            UtilityLoginDetails.GETInstance.TodaysDateTime = UtilityLoginDetails.GETInstance.TodaysDateTime.AddMilliseconds(1000);
        }

        private void ReadTWIINI()
        {

            //  string DecryptedPwd = string.Empty;
            try
            {

                ByPassLogin = parser.GetSetting("Login Settings", "BYPASSLOGIN");

                if (ByPassLogin == "0")
                {
                    MemoryManager.InitializeDefaultMemory();

                    LogonRequest oLogonRequest = new LogonRequest();
                    LoginProcessor oLoginProcessor = new LoginProcessor();
                    LoginRequestProcessor oLoginRequestProcessor = null;

                    //initialze socket
                    MemoryManager.AsynchronousClient.StartClient();

                    //initiate receive
                    ReceiverController oReceiverController = new ReceiverController();
                    oReceiverController.ReceiveMessage();

                    //initiate UMS
                    //UMSController oUMSController = new UMSController();
                    //oUMSController.ReceiveUMSMessage();


                    EqtyBcastFlag = UtilityLoginDetails.GETInstance.IsBcastEqChecked = Convert.ToBoolean(parser.GetSetting("Login Settings", "EQBrCheck"));
                    DervBcastFlag = UtilityLoginDetails.GETInstance.IsBcasDerChecked = Convert.ToBoolean(parser.GetSetting("Login Settings", "DrBrCheck"));
                    CurrBcastFlag = UtilityLoginDetails.GETInstance.IsBcastCurChecked = Convert.ToBoolean(parser.GetSetting("Login Settings", "CurBrCheck"));

                    LoginScreenVM.EquitySegChk = UtilityLoginDetails.GETInstance.IsEQXChecked = Convert.ToBoolean(parser.GetSetting("Login Settings", "EQCheck"));
                    LoginScreenVM.EquitySegChk = UtilityLoginDetails.GETInstance.IsDERChecked = Convert.ToBoolean(parser.GetSetting("Login Settings", "DrCheck"));
                    LoginScreenVM.EquitySegChk = UtilityLoginDetails.GETInstance.IsCURChecked = Convert.ToBoolean(parser.GetSetting("Login Settings", "CurCheck"));

                    MemberID = parser.GetSetting("Login Settings", "MEMBERID");
                    TraderID = parser.GetSetting("Login Settings", "TRADERID");
                    UtilityLoginDetails.GETInstance.DecryptedPassword = DecrypterdPwd = parser.GetSetting("Login Settings", "PASSWORD");
                    UtilityLoginDetails.GETInstance.DecryptedPassword = DecrypterdPwd = decrypt(DecrypterdPwd.ToCharArray(), key);

                    //if (EqtyFlag)
                    //{
                    //    LoginScreenVM.EquitySegChk = true;
                    //    CreateEQXloginRequest(EncrypterdPwd, ref oLogonRequest);
                    //    oLoginProcessor.ProcessData(oLogonRequest);
                    //}
                    //else
                    //{
                    //    System.Windows.MessageBox.Show("Please Check the Equity CheckBox in Launcher", "Error in Application", MessageBoxButton.OK, MessageBoxImage.Error);
                    //    if (!process.HasExited)
                    //        process.Kill();
                    //    parser.AddSetting("Login Settings", "BYPASSLOGIN", "1");
                    //    parser.SaveSettings(TwsINIPath.ToString());
                    //    Process.GetCurrentProcess().Kill();
                    //}

                    //Registration Request- Gaurav Jadhav 15/12/2017
                    LoginScreenVM.GETInstanceLogin.CreateRegistrationRequest(ref oLogonRequest);
                    oLoginRequestProcessor = new LoginRequestProcessor(new UserRegistration());
                    oLoginRequestProcessor.ProcessRequest(oLogonRequest);

                    ////Login Request- Gaurav Jadhav 15/12/2017
                    LoginScreenVM.GETInstanceLogin.CreateLoginRequest(DecrypterdPwd, ref oLogonRequest);
                    oLoginRequestProcessor = new LoginRequestProcessor(new LogOn());
                    oLoginRequestProcessor.ProcessRequest(oLogonRequest);
                    //TODO:Uncomment after Derivative and Currency segments are started
                    //if (DervFlag)
                    //{
                    //    CreateDERloginRequest(EncrypterdPwd, ref oLogonRequest);
                    //    oLoginProcessor.ProcessData(oLogonRequest);
                    //}
                    //if (CurrFlag)
                    //{
                    //    CreateCURloginRequest(EncrypterdPwd, ref oLogonRequest);
                    //    oLoginProcessor.ProcessData(oLogonRequest);

                    //}
                    //TODO:Uncomment after Derivative and Currency segments are started
                }
                //else if (ByPassLogin == "1")
                //{
                //    LoginScreen oLoginScreen = System.Windows.Application.Current.Windows.OfType<LoginScreen>().FirstOrDefault();
                //    oLoginScreen = new LoginScreen();
                //    //oLoginScreen.Owner = System.Windows.Application.Current.MainWindow;
                //    oLoginScreen.Activate();
                //    //oLoginScreen.Topmost = true;
                //    oLoginScreen.MyPasswordBox.Focus();
                //    //oLoginScreen.ShowActivated = true;
                //    oLoginScreen.Show();
                //}
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
                return;
            }

        }

        public void CreateEQXloginRequest(string pwd, ref LogonRequest oLogonRequest)
        {
            oLogonRequest.MemberID = UtilityLoginDetails.GETInstance.MemberId = Convert.ToUInt16(MemberID);
            oLogonRequest.TraderID = UtilityLoginDetails.GETInstance.TraderId = Convert.ToUInt16(TraderID); ;//204;
            oLogonRequest.Password = pwd;
            //oLogonRequest.MessageTag = MemoryManager.GetMesageTag();
            oLogonRequest.Market = 1;//1 - Equity, 2- Derv., 3. Curr
            oLogonRequest.Exchange = 1;//1- BSE, 2-BOW, 3-NSE
            oLogonRequest.NewPassword = "";
            oLogonRequest.Filler_c = "";

        }

        public void CreateDERloginRequest(string pwd, ref LogonRequest oLogonRequest)
        {
            oLogonRequest.MemberID = UtilityLoginDetails.GETInstance.MemberId = Convert.ToUInt16(MemberID);
            oLogonRequest.TraderID = UtilityLoginDetails.GETInstance.TraderId = Convert.ToUInt16(TraderID); ;//204;
            oLogonRequest.Password = pwd;
            //oLogonRequest.MessageTag = MemoryManager.GetMesageTag();
            oLogonRequest.Market = 2;//1 - Equity, 2- Derv., 3. Curr
            oLogonRequest.Exchange = 1;//1- BSE, 2-BOW, 3-NSE
            oLogonRequest.NewPassword = "";
            oLogonRequest.Filler_c = "";

        }

        public void CreateCURloginRequest(string pwd, ref LogonRequest oLogonRequest)
        {
            oLogonRequest.MemberID = UtilityLoginDetails.GETInstance.MemberId = Convert.ToUInt16(MemberID);
            oLogonRequest.TraderID = UtilityLoginDetails.GETInstance.TraderId = Convert.ToUInt16(TraderID); ;//204;
            oLogonRequest.Password = pwd;
            //oLogonRequest.MessageTag = MemoryManager.GetMesageTag();
            oLogonRequest.Market = 3;//1 - Equity, 2- Derv., 3. Curr
            oLogonRequest.Exchange = 1;//1- BSE, 2-BOW, 3-NSE
            oLogonRequest.NewPassword = "";
            oLogonRequest.Filler_c = "";
        }

        private string Encrypt(char[] p, int key)
        {
            for (int i = 0; i < p.Length; ++i)
            {
                p[i] += (char)key;
                //EncrypterdPwd[i] = (char)(EncrypterdPwd[i] + key);
            }

            return new string(p);
        }

        private string decrypt(char[] EncrypterdPwd, int key)
        {
            // char[] Decpwd = new char[EncrypterdPwd.Length];

            for (int i = 0; i < EncrypterdPwd.Length; ++i)
            {
                EncrypterdPwd[i] += (char)key;
                //EncrypterdPwd[i] = (char)(EncrypterdPwd[i] + key);
            }

            return new string(EncrypterdPwd);
        }

        public static void Window_Closing_Event(CancelEventArgs e)
        {
            var response = System.Windows.MessageBox.Show("Do you really want to exit?", "Exiting...",
                                  MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
            if (response == MessageBoxResult.Yes)
            {
                try
                {
                    parser.AddSetting("Login Settings", "BYPASSLOGIN", "1");
                    parser.SaveSettings(TwsINIPath.ToString());
                    SaveBSEBulletins();


                    AllIndicesSave();


                }
                catch (Exception ex)
                {
                    ExceptionUtility.LogError(ex);
                }

                // Process currProc = Process.GetCurrentProcess();
                // currProc.Kill();


                finally
                {
                    process = Process.GetProcessesByName("imlPro").FirstOrDefault();
                    if (process != null)
                        process.Kill();
                    Process.GetCurrentProcess().Kill();
                }
            }
            else if (response == MessageBoxResult.No)
            {
                e.Cancel = true;
            }
        }

        private static void AllIndicesSave()
        {
            try
            {
                parserAllIndices.AddSetting("Indices", "Number Of Indices", AllIndicesVM.ObjMinIndicesCollection.Count.ToString());
                parserAllIndices.AddSetting("Indices", "Configure Indexes", IndexConfiguration());
                parserAllIndices.AddSetting("Visibility", "GridLine", AllIndicesVM.GetInstance.GridLinesVisibility);
                parserAllIndices.SaveSettings(AllIndicesINIPath.ToString());
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
            }

        }

        private static string IndexConfiguration()
        {
            string indexes = string.Empty;
            try
            {

                for (int i = 0; i < AllIndicesVM.ObjMinIndicesCollection.Count; i++)
                {
                    indexes += "|" + AllIndicesVM.ObjMinIndicesCollection[i].IndexId;
                }
            }
            catch (Exception e)
            {
                ExceptionUtility.LogError(e);
            }

            return indexes;
        }

        private static void SaveBSEBulletins()
        {
            //using (StreamWriter sw = new StreamWriter(Path.GetFullPath(Path.Combine(System.Environment.CurrentDirectory, @"Sub Systems/BSEDATA.ini"))))
            //{

            //if (MemoryManager.lines.Contains("Linktodisplay"))
            //{
            if (MemoryManager.lines != null && MemoryManager.lines.Count > 0)
            {
                for (int Index = 0; Index < MemoryManager.lines.Count; Index++)
                {
                    if (MemoryManager.lines[Index].Contains("Linktodisplay"))
                    {
                        MemoryManager.lines[Index] = "Linktodisplay=" + UtilityLoginDetails.GETInstance.NextBulletinPoint;
                        if (File.Exists(Path.GetFullPath(Path.Combine(System.Environment.CurrentDirectory, @"Sub Systems/BSEDATA.ini"))))
                        {
                            File.WriteAllLines(Path.GetFullPath(Path.Combine(System.Environment.CurrentDirectory, @"Sub Systems/BSEDATA.ini")), MemoryManager.lines);
                        }
                        break;
                    }
                }

            }
            //}

            //}


        }

        public static Model.ScripDetails UpdateScripDataFromMemory(BroadcastReceiver.ScripDetails Br)
        {
            Model.ScripDetails s = new Model.ScripDetails();

            if (Br == null)
                return s;

            s.ScriptCode_BseToken_NseToken = Br.ScripCode_l;
            s.Segment_Market = Br.Segment.ToString();
            //  s.ScripID=Br.sc
            s.lastTradeRateL = Br.LastTradeRate_l;
            s.openRateL = Br.OpenRate_l;
            s.closeRateL = Br.CloseRate_l;
            s.highRateL = Br.HighRate_l;
            s.lowRateL = Br.LowRate_l;
            s.totBuyQtyL = Br.TotBuyQty_l;
            s.totSellQtyL = Br.TotSellQty_l;
            s.wtAvgRateL = Br.WtAvgRate_l;
            s.PrevcloseRateL = Br.PrevClosePrice_l;
            s.lastTradeQtyL = Br.LastTradeQty_l;
            s.lastTradeRateL = Br.LastTradeRate_l;
            s.TrdVolume = Br.TradedVolume_l;
            s.TrdValue = Br.TradedValue_l;
            s.LowerCtLmt = Br.LowerCktLmt_l;
            s.UprCtLmt = Br.UpperCktLmt_l;
            s.NoOfTrades = Br.NoOfTrades_l;
            s.IndicateEqPrice = Br.IndicativeEqPrice_l;
            s.IndicateEqQty = Br.IndicativeEqQuantity_l;
            s.MarketType = Br.MarketType_s;
            s.SessionNo = Br.SessionNo_s;
            s.BRP = Br.BRP;




            s.listBestFive = new List<Model.BestFive>();

            for (int i = 0; i < 5; i++)
            {
                Model.BestFive objbstfive = new Model.BestFive();
                if (Br.Det[i] != null)
                {
                    objbstfive.BuyRateL = Br.Det[i].BuyRate_l;
                    objbstfive.BuyQtyL = Br.Det[i].BuyQty_l;
                    objbstfive.NoOfBidBuyL = Br.Det[i].NoOfBidBuy_l;
                    objbstfive.FillerBuyL = Br.Det[i].FillerBuy_l;

                    objbstfive.SellRateL = Br.Det[i].SellRate_l;
                    objbstfive.SellQtyL = Br.Det[i].SellQty_l;
                    objbstfive.NoOfBidSellL = Br.Det[i].NoOfBidSell_l;
                    objbstfive.FillerSellL = Br.Det[i].FillerSell_l;

                    s.listBestFive.Add(objbstfive);
                }
            }
            if (Br.Det[0] != null)
            {
                s.BuyRateL = Br.Det[0].BuyRate_l;
                s.BuyQtyL = Br.Det[0].BuyQty_l;
                s.NoOfBidBuyL = Br.Det[0].NoOfBidBuy_l;
                s.FillerBuyL = Br.Det[0].FillerBuy_l;

                s.SellRateL = Br.Det[0].SellRate_l;
                s.SellQtyL = Br.Det[0].SellQty_l;
                s.NoOfBidSellL = Br.Det[0].NoOfBidSell_l;
                s.FillerSellL = Br.Det[0].FillerSell_l;
            }

            s.LastTradeTime = string.Format("{00}:{01}:{02}", Br.LTP_HH_s.ToString("00"), Br.LTP_MM_s.ToString("00"), Br.LTP_SS_s.ToString("00"));
            s.Unit_c = Br.Unit_c;

            return s;
        }

        public static Model.ScripDetails UpdateScripDataFromMemory(int ScripCode)
        {

            if (BroadcastMaster.BroadcastMasterMemory.objScripDetailsConDict.ContainsKey(ScripCode))
            {
                Model.ScripDetails s = new Model.ScripDetails();



                s.ScriptCode_BseToken_NseToken = BroadcastMaster.BroadcastMasterMemory.objScripDetailsConDict[ScripCode].ScripCode_l;
                s.Segment_Market = BroadcastMaster.BroadcastMasterMemory.objScripDetailsConDict[ScripCode].Segment.ToString();
                //  s.ScripID=BroadcastMaster.BroadcastMasterMemory.objScripDetailsConDict[ScripCode].sc
                s.lastTradeRateL = BroadcastMaster.BroadcastMasterMemory.objScripDetailsConDict[ScripCode].LastTradeRate_l;
                s.openRateL = BroadcastMaster.BroadcastMasterMemory.objScripDetailsConDict[ScripCode].OpenRate_l;
                s.closeRateL = BroadcastMaster.BroadcastMasterMemory.objScripDetailsConDict[ScripCode].CloseRate_l;
                s.highRateL = BroadcastMaster.BroadcastMasterMemory.objScripDetailsConDict[ScripCode].HighRate_l;
                s.lowRateL = BroadcastMaster.BroadcastMasterMemory.objScripDetailsConDict[ScripCode].LowRate_l;
                s.totBuyQtyL = BroadcastMaster.BroadcastMasterMemory.objScripDetailsConDict[ScripCode].TotBuyQty_l;
                s.totSellQtyL = BroadcastMaster.BroadcastMasterMemory.objScripDetailsConDict[ScripCode].TotSellQty_l;
                s.wtAvgRateL = BroadcastMaster.BroadcastMasterMemory.objScripDetailsConDict[ScripCode].WtAvgRate_l;
                s.PrevcloseRateL = BroadcastMaster.BroadcastMasterMemory.objScripDetailsConDict[ScripCode].PrevClosePrice_l;
                s.lastTradeQtyL = BroadcastMaster.BroadcastMasterMemory.objScripDetailsConDict[ScripCode].LastTradeQty_l;
                s.lastTradeRateL = BroadcastMaster.BroadcastMasterMemory.objScripDetailsConDict[ScripCode].LastTradeRate_l;
                s.TrdVolume = BroadcastMaster.BroadcastMasterMemory.objScripDetailsConDict[ScripCode].TradedVolume_l;
                s.TrdValue = BroadcastMaster.BroadcastMasterMemory.objScripDetailsConDict[ScripCode].TradedValue_l;
                s.LowerCtLmt = BroadcastMaster.BroadcastMasterMemory.objScripDetailsConDict[ScripCode].LowerCktLmt_l;
                s.UprCtLmt = BroadcastMaster.BroadcastMasterMemory.objScripDetailsConDict[ScripCode].UpperCktLmt_l;
                s.NoOfTrades = BroadcastMaster.BroadcastMasterMemory.objScripDetailsConDict[ScripCode].NoOfTrades_l;
                s.IndicateEqPrice = BroadcastMaster.BroadcastMasterMemory.objScripDetailsConDict[ScripCode].IndicativeEqPrice_l;
                s.IndicateEqQty = BroadcastMaster.BroadcastMasterMemory.objScripDetailsConDict[ScripCode].IndicativeEqQuantity_l;
                s.MarketType = BroadcastMaster.BroadcastMasterMemory.objScripDetailsConDict[ScripCode].MarketType_s;
                s.SessionNo = BroadcastMaster.BroadcastMasterMemory.objScripDetailsConDict[ScripCode].SessionNo_s;
                s.BRP = BroadcastMaster.BroadcastMasterMemory.objScripDetailsConDict[ScripCode].BRP;

                s.listBestFive = new List<Model.BestFive>();

                for (int i = 0; i < 5; i++)
                {
                    Model.BestFive objbstfive = new Model.BestFive();
                    if (BroadcastMaster.BroadcastMasterMemory.objScripDetailsConDict[ScripCode].Det[i] != null)
                    {
                        objbstfive.BuyRateL = BroadcastMaster.BroadcastMasterMemory.objScripDetailsConDict[ScripCode].Det[i].BuyRate_l;
                        objbstfive.BuyQtyL = BroadcastMaster.BroadcastMasterMemory.objScripDetailsConDict[ScripCode].Det[i].BuyQty_l;
                        objbstfive.NoOfBidBuyL = BroadcastMaster.BroadcastMasterMemory.objScripDetailsConDict[ScripCode].Det[i].NoOfBidBuy_l;
                        objbstfive.FillerBuyL = BroadcastMaster.BroadcastMasterMemory.objScripDetailsConDict[ScripCode].Det[i].FillerBuy_l;

                        objbstfive.SellRateL = BroadcastMaster.BroadcastMasterMemory.objScripDetailsConDict[ScripCode].Det[i].SellRate_l;
                        objbstfive.SellQtyL = BroadcastMaster.BroadcastMasterMemory.objScripDetailsConDict[ScripCode].Det[i].SellQty_l;
                        objbstfive.NoOfBidSellL = BroadcastMaster.BroadcastMasterMemory.objScripDetailsConDict[ScripCode].Det[i].NoOfBidSell_l;
                        objbstfive.FillerSellL = BroadcastMaster.BroadcastMasterMemory.objScripDetailsConDict[ScripCode].Det[i].FillerSell_l;

                        s.listBestFive.Add(objbstfive);
                    }
                }
                if (BroadcastMaster.BroadcastMasterMemory.objScripDetailsConDict[ScripCode].Det[0] != null)
                {
                    s.BuyRateL = BroadcastMaster.BroadcastMasterMemory.objScripDetailsConDict[ScripCode].Det[0].BuyRate_l;
                    s.BuyQtyL = BroadcastMaster.BroadcastMasterMemory.objScripDetailsConDict[ScripCode].Det[0].BuyQty_l;
                    s.NoOfBidBuyL = BroadcastMaster.BroadcastMasterMemory.objScripDetailsConDict[ScripCode].Det[0].NoOfBidBuy_l;
                    s.FillerBuyL = BroadcastMaster.BroadcastMasterMemory.objScripDetailsConDict[ScripCode].Det[0].FillerBuy_l;

                    s.SellRateL = BroadcastMaster.BroadcastMasterMemory.objScripDetailsConDict[ScripCode].Det[0].SellRate_l;
                    s.SellQtyL = BroadcastMaster.BroadcastMasterMemory.objScripDetailsConDict[ScripCode].Det[0].SellQty_l;
                    s.NoOfBidSellL = BroadcastMaster.BroadcastMasterMemory.objScripDetailsConDict[ScripCode].Det[0].NoOfBidSell_l;
                    s.FillerSellL = BroadcastMaster.BroadcastMasterMemory.objScripDetailsConDict[ScripCode].Det[0].FillerSell_l;
                }

                s.LastTradeTime = string.Format("{00}:{01}:{02}", BroadcastMaster.BroadcastMasterMemory.objScripDetailsConDict[ScripCode].LTP_HH_s.ToString("00"), BroadcastMaster.BroadcastMasterMemory.objScripDetailsConDict[ScripCode].LTP_MM_s.ToString("00"), BroadcastMaster.BroadcastMasterMemory.objScripDetailsConDict[ScripCode].LTP_SS_s.ToString("00"));
                s.Unit_c = BroadcastMaster.BroadcastMasterMemory.objScripDetailsConDict[ScripCode].Unit_c;

                return s;
            }

            return null;
        }

        public static void MenuSaudasClick(HotKey _hotKey)
        {
            //Window1 oWindow1 = System.Windows.Application.Current.Windows.OfType<Window1>().FirstOrDefault();
            //if (oWindow1 == null)
            //{
            //    oWindow1 = new Window1();
            //    oWindow1.Activate();
            //    oWindow1.Show();
            //}
            //else
            //{
            //    oWindow1.Activate();
            //    oWindow1.Show();
            //}
            if (UtilityLoginDetails.GETInstance.IsLoggedIN)
            {
                Saudas_Admin SaudasAdminWindow = null;
                UtilityTradeDetails.GetInstance.LoadManual_SaudasWindow = true;
                System.Threading.Tasks.Task.Factory.StartNew(() =>
                {
                    App.Current.Dispatcher.BeginInvoke((Action)delegate ()
                    {
                        SaudasAdminWindow = System.Windows.Application.Current.Windows.OfType<Saudas_Admin>().FirstOrDefault();

                        if (SaudasAdminWindow != null)
                        {
                            SaudasAdminWindow.Owner = System.Windows.Application.Current.MainWindow;
                            //  if (UtilityTradeDetails.GetInstance.SaudasWindow_PdCompleted)
                            //  {
                            //SaudasAdminWindow.Activate();
                            SaudasAdminWindow.Show();
                            SaudasAdminWindow.Focus();
                            if (SaudasAdminWindow.WindowState == WindowState.Minimized)
                            {
                                SaudasAdminWindow.WindowState = WindowState.Normal;
                            }
                            // }
                            // else
                            // {
                            //System.Windows.MessageBox.Show("Please wait while personal download is completed");
                            //  SaudasAdminWindow.Hide();
                            // }
                        }
                        else
                        {
                            SaudasAdminWindow = new Saudas_Admin();
                            SaudasAdminWindow.Owner = System.Windows.Application.Current.MainWindow;
                            //Load_SaudasScreen = true;

                            if (SaudasAdminWindow != null)
                            {
                                // if (UtilityTradeDetails.GetInstance.SaudasWindow_PdCompleted)
                                //{
                                SaudasAdminWindow.Activate();
                                SaudasAdminWindow.Show();
                                //}
                                //else
                                //{
                                //   SaudasAdminWindow.Hide();
                                //}
                            }
                            //TradePerformanceTest oTradePerformanceTest = new TradePerformanceTest();
                            //oTradePerformanceTest.Show();
                            //oTradePerformanceTest.Activate();
                            //oTradePerformanceTest.Owner = System.Windows.Application.Current.MainWindow;
                        }
                    });
                });
            }
        }

        public void NetPositionScripWiseClick(HotKey _HotKey)
        {

            if (UtilityLoginDetails.GETInstance.IsLoggedIN)
            {
                if (MemoryManager.IsPersonalDownloadComplete)
                {
                    NetPositionScripWise oNetPositionScripWise = System.Windows.Application.Current.Windows.OfType<NetPositionScripWise>().FirstOrDefault();
                    if (oNetPositionScripWise != null)
                    {
                        //oNetPositionScripWise.Activate();
                        oNetPositionScripWise.Focus();
                        oNetPositionScripWise.Show();
                        if (oNetPositionScripWise.WindowState == WindowState.Minimized)
                        {
                            oNetPositionScripWise.WindowState = WindowState.Normal;
                        }
                    }
                    else
                    {
                        oNetPositionScripWise = new NetPositionScripWise();
                        oNetPositionScripWise.Activate();
                        oNetPositionScripWise.Owner = System.Windows.Application.Current.MainWindow;
                        oNetPositionScripWise.Show();
                        UtilityTradeDetails.GetInstance.Load_NetPositionScripWise = true;
                    }
                }
            }
        }

        public void NetPositionClientWiseClick(HotKey _HitKey)
        {

            if (UtilityLoginDetails.GETInstance.IsLoggedIN)
            {
                NetPositionClientWise oNetPositionClientWise = System.Windows.Application.Current.Windows.OfType<NetPositionClientWise>().FirstOrDefault();
                if (oNetPositionClientWise != null)
                {
                    //oNetPositionClientWise.Activate();
                    oNetPositionClientWise.Focus();
                    oNetPositionClientWise.Show();
                    if (oNetPositionClientWise.WindowState == WindowState.Minimized)
                    {
                        oNetPositionClientWise.WindowState = WindowState.Normal;
                    }
                }
                else
                {
                    oNetPositionClientWise = new NetPositionClientWise();
                    oNetPositionClientWise.Activate();
                    oNetPositionClientWise.Owner = System.Windows.Application.Current.MainWindow;
                    oNetPositionClientWise.Show();
                    UtilityTradeDetails.GetInstance.Load_NetPositionClientWise = true;
                }

            }


        }

        public static void ProcessCMWQueue()
        {
            try
            {
                CommonMessagingWindowModel cmodel = null;
                while (true)
                {
                    try
                    {
                        if ((MemoryManager.CMWMessageBag != null))
                        {
                            while (MemoryManager.CMWMessageBag.Count > 0)
                            {
                                MemoryManager.CMWMessageBag.TryDequeue(out cmodel);
                                if ((cmodel != null))
                                {
                                    CommonMessagingWindowVM.ProcessCMWData(cmodel);
                                }
                                cmodel = null;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtility.LogError(ex);
                    }
                    Thread.Sleep(10);
                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
            }
        }

        private void StartCMWThread()
        {

            if (CommonMessagingWindowThread != null)
            {
                CommonMessagingWindowThread.Abort();
                CommonMessagingWindowThread = null;
            }

            CommonMessagingWindowThread = new Thread(ProcessCMWQueue);
            CommonMessagingWindowThread.Name = "Process UMS Messages for CMW";
            CommonMessagingWindowThread.Priority = ThreadPriority.Normal;
            CommonMessagingWindowThread.Start();
        }

        private void CloseWindowsOnEscape_Click()
        {
            bool Hiddenflag = false;
            WindowCollection oWindowCollection = new WindowCollection();
            oWindowCollection = System.Windows.Application.Current.Windows;
            int length = oWindowCollection.Count;
            for (int index = length - 1; index >= 0; index--)
            {
                Window oWindow = oWindowCollection[index];

                if (!new[] { "Microsoft.XamlDiagnostics.WpfTap.WpfVisualTreeService.Adorners.AdornerLayerWindow",
                    "CommonFrontEnd.View.MainWindow", "CommonFrontEnd.View.CommonMessagingWindow",
                "CommonFrontEnd.View.Trade.PersonalDownloadProgessWindow","System.Windows.Window"}.Any(x => x == oWindow.ToString()))
                {
                    if (oWindow.Visibility == Visibility.Visible)
                    {
                        if (oWindow.WindowState == WindowState.Maximized || oWindow.WindowState == WindowState.Normal)
                        {
                            if (Hiddenflag == false)
                            {
                                if (oWindow.ToString() == "CommonFrontEnd.View.BulkPriceChnage")
                                {
                                    oWindow.Close();
                                }
                                else
                                {
                                    oWindow.Hide();
                                }
                                Hiddenflag = true;
                            }
                            else
                            {
                                oWindow.Focus();
                                break;
                            }
                        }
                    }
                }
            }
        }

        public void UpMenu_Click()
        {
            MenuVisibility_Expander = "Hidden";
        }

        private void DownMenu_Click()
        {
            MenuVisibility_Expander = "Visible";
        }


    }


#elif BOW
    public partial class MainWindowVM
    {
       
        public static DirectoryInfo OrderSettingsINIPath = new DirectoryInfo(Path.GetFullPath(Path.Combine(System.Environment.CurrentDirectory, @"Profile/OrderSettings.ini")));
        public static IniParser parserOS = new IniParser(OrderSettingsINIPath.ToString());
        public static DirectoryInfo BOWSettingsINIPath = new DirectoryInfo(Path.GetFullPath(Path.Combine(System.Environment.CurrentDirectory, @"BOW.ini")));
        public static IniParser parser1OS = new IniParser(BOWSettingsINIPath.ToString());
        public static DirectoryInfo TwsINIPath = new DirectoryInfo(Path.GetFullPath(Path.Combine(System.Environment.CurrentDirectory, @"Profile/Tws.ini")));
        public static IniParser parser = new IniParser(TwsINIPath.ToString());
      
        
        // public static DirectoryInfo BOWSettingsINIPath = new DirectoryInfo(Path.GetFullPath(Path.Combine(System.Environment.CurrentDirectory, @"BOLT.ini")));

         UtilityLoginDetails objUtilityLoginDetails = UtilityLoginDetails.GETInstance;
        public bool gblnBroadcastMessageLog;
        public static int DecimalPnt { get; set; }

       
        private RelayCommand _LoadLoginWindow;

        public RelayCommand LoadLoginWindow
        {
            get
            {
                return _LoadLoginWindow ?? (_LoadLoginWindow = new RelayCommand((object e) => OpenLoginWindow()));
            }

        }
        //private RelayCommand _DownloadOrders;

        //public RelayCommand DownloadOrders
        //{
        //    get
        //    {
        //        return _DownloadOrders ?? (_DownloadOrders = new RelayCommand((object e) => DownloadOrders_Click()));
        //    }

        //}

        

        private MainWindowVM()
        {
          
            MenuVisibility();
            MasterSharedMemory.ReadAllMasters();
            AppSettingsReader objReader = new AppSettingsReader();
            if (Convert.ToBoolean(objReader.GetValue("LoadMWFromLocalDb", typeof(bool))) == true)
            {
                Globals.GETInstance.gblnMarketWatchLocal = true;
            }
            else
            {
                Globals.GETInstance.gblnMarketWatchLocal = false;
            }
            try
            {
                Globals.GETInstance.gblnBroadcastMessageLog = Convert.ToBoolean(objReader.GetValue("BroadCastMessagesLog", typeof(bool)));
            }
            catch (Exception ex)
            {
                Infrastructure.Logger.WriteLog("BroadCastMessagesLog dose not exists in the config file.");
             }
            Globals.GETInstance.gblnDirectBroadcastConfigured = Convert.ToBoolean(objReader.GetValue("DirectBroadcast", typeof(bool)));

            try
            {
                Globals.GETInstance.gblnUseConsizedMWMsg = Convert.ToBoolean(objReader.GetValue("UseConsizedMWMsg", typeof(bool)));
            }
            catch (Exception ex)
            {
                Infrastructure.Logger.WriteLog("UseConsizedMWMsg dose not exists in the config file.");
            }

           
        }

        public void OpenLoginWindow()
        {

            System.Windows.Application.Current.MainWindow.Focus();


            LoginScreen oLoginScreen = System.Windows.Application.Current.Windows.OfType<LoginScreen>().FirstOrDefault();
            oLoginScreen = new LoginScreen();
            //oLoginScreen.Owner = System.Windows.Application.Current.MainWindow;
            oLoginScreen.Activate();
            //oLoginScreen.Topmost = true;
            oLoginScreen.txtPasswordBow.Focus();
            //oLoginScreen.ShowActivated = true;
            oLoginScreen.Show();

        }
        //public static Model.IdicesDetailsMain UpdateIndicesDeatailsDataFromMemory(string[] larrstrCol)
        //{
        //    Model.IdicesDetailsMain obj = new Model.IdicesDetailsMain();
        //    if(larrstrCol !=null && larrstrCol[0] == "72")
        //    {

        //    }
        //    return obj;
        //}
        public static Model.IdicesDetailsMain UpdateIndicesDataFromMemory(string[] larrstrCol)
        {
            Model.IdicesDetailsMain obj = new Model.IdicesDetailsMain();
            if (larrstrCol != null && larrstrCol[0] == "69")
            {
                obj.IndexCode = Convert.ToInt32(larrstrCol[1]);
                if (string.IsNullOrEmpty(larrstrCol[2]))
                {
                    obj.IndexValue = 0;
                }
                else
                { obj.IndexValue = Convert.ToInt32(Convert.ToDecimal(larrstrCol[2]) * 100); }
                if (string.IsNullOrEmpty(larrstrCol[3]))
                {
                    obj.IndexChangeValue = 0;
                }
                else
                { obj.IndexChangeValue = Convert.ToDouble(larrstrCol[3]); }
               // obj.IndexChangeValue = Convert.ToDouble(string.IsNullOrEmpty(larrstrCol[3]));
                if (obj != null)
                {
                    if (!BroadcastMasterMemory.objIndexDetailsConDict.Keys.Contains(obj.IndexCode))
                    {
                        BroadcastMasterMemory.objIndexDetailsConDict.Add(obj.IndexCode, obj);
                    }
                    else
                    {
                        BroadcastMasterMemory.objIndexDetailsConDict[obj.IndexCode] = obj;
                    }
                }
                
                
                return obj;
            }
            return obj;
        }
        public static Model.ScripDetails UpdateScripDataFromMemory(String[] larrstrCol)
        {
            


            Model.ScripDetails s = MarketWatchConstants.objScripDetails.Where(p => p.CommonToken == larrstrCol[4]).FirstOrDefault();
            if (s != null)
            {
              
            }
            else
            {
                 s = new Model.ScripDetails();
            }

            try
            {
               
                if (larrstrCol == null)
                    return s;
                s.LogTime = larrstrCol[1];
                s.Exchange_Source = larrstrCol[2];
                s.Segment_Market = larrstrCol[3];
                s.CommonToken = larrstrCol[4];

                DecimalPnt = CommonFunctions.GetDecimal(s.ScriptCode_BseToken_NseToken, Enumerations.Exchange.BSE, Enumerations.Segment.Equity);
                if(larrstrCol[7]!="")
                s.TrdVolume = Convert.ToInt32(larrstrCol[7]);
                if (larrstrCol[8] != "")
                    s.lastTradeRateL = Convert.ToInt32(Convert.ToDouble(larrstrCol[8]) * Math.Pow(10, DecimalPnt));
             
                    s.NetChangeIndicator = larrstrCol[9];
                if (larrstrCol[10] != "")
                    s.NetChange = Convert.ToInt32(Convert.ToDouble(larrstrCol[10]) * Math.Pow(10, DecimalPnt));
                if (larrstrCol[11] != "")
                    s.ChangePercentage = Convert.ToInt32(Convert.ToDouble(larrstrCol[11]) * Math.Pow(10, DecimalPnt));
                if (larrstrCol[12] != "")
                    s.lastTradeQtyL = Convert.ToInt32(larrstrCol[12]);
              
                    s.LastTradeTime = larrstrCol[13];
                if (larrstrCol[14] != "")
                    s.wtAvgRateL = Convert.ToInt32(Convert.ToDouble(larrstrCol[14]) * Math.Pow(10, DecimalPnt));
                if (larrstrCol[15] != "")
                    s.BuyQtyL = Convert.ToInt32(larrstrCol[15]);
                if (larrstrCol[16] != "")
                    s.BuyRateL = Convert.ToInt32(Convert.ToDouble(larrstrCol[16]) * Math.Pow(10, DecimalPnt));
                if (larrstrCol[18] != "")
                    s.SellQtyL = Convert.ToInt32(larrstrCol[18]);
                if (larrstrCol[19] != "")
                    s.SellRateL = Convert.ToInt32(Convert.ToDouble(larrstrCol[19]) * Math.Pow(10, DecimalPnt));
                if (larrstrCol[17] != "")

                    s.totBuyQtyL = Convert.ToInt32(larrstrCol[17]);
                if (larrstrCol[20] != "")

                    s.totSellQtyL = Convert.ToInt32(larrstrCol[20]);
                if (larrstrCol[21] != "")
                    s.closeRateL = Convert.ToInt32(Convert.ToDouble(larrstrCol[21]) * Math.Pow(10, DecimalPnt));
                if (larrstrCol[22] != "")
                    s.openRateL = Convert.ToInt32(Convert.ToDouble(larrstrCol[22]) * Math.Pow(10, DecimalPnt));
                if (larrstrCol[23] != "")
                    s.highRateL = Convert.ToInt32(Convert.ToDouble(larrstrCol[23]) * Math.Pow(10, DecimalPnt));
                if (larrstrCol[24] != "")
                    s.lowRateL = Convert.ToInt32(Convert.ToDouble(larrstrCol[24]) * Math.Pow(10, DecimalPnt));
                if (larrstrCol[25] != "")
                    s.FiftyTwoLow = Convert.ToInt32(Convert.ToDouble(larrstrCol[25]) * Math.Pow(10, DecimalPnt));
                if (larrstrCol[26] != "")
                    s.FiftyTwoHigh = Convert.ToInt32(Convert.ToDouble(larrstrCol[26]) * Math.Pow(10, DecimalPnt));
                if (larrstrCol[27] != "")
                {
                    // s.TrdValue = Convert.ToInt32(Convert.ToDouble(larrstrCol[27]) * Math.Pow(10, DecimalPnt));
                    double TradeValue;
                    TradeValue = Convert.ToDouble(larrstrCol[27]);
                    if (TradeValue < 1.00)
                    {
                        s.TrdValue = Convert.ToInt32((TradeValue * 10000000) );
                        s.Unit_c = "";
                    }
                    else if (TradeValue >= 100.00)
                    {
                        s.TrdValue = Convert.ToInt32((TradeValue * 100) );
                      //  s.TrdValue = (TradeValue / 10000) * 100; //2 decimal rounding
                        s.Unit_c = "C";
                    }
                    else
                    {
                        s.TrdValue = Convert.ToInt32(TradeValue * 100);
                        s.Unit_c = "L";
                    }
                }
                if (larrstrCol[28] != "")
                    s.NoOfTrades = Convert.ToInt32(larrstrCol[28]);
                if (larrstrCol[29] != "")
                    s.UprCtLmt = Convert.ToInt32(Convert.ToDouble(larrstrCol[29]) * Math.Pow(10, DecimalPnt));
                if (larrstrCol[30] != "")
                    s.LowerCtLmt = Convert.ToInt32(Convert.ToDouble(larrstrCol[30]) * Math.Pow(10, DecimalPnt));
                if (larrstrCol[31] != "")
                    s.BuyExchange = larrstrCol[31];
                s.SellExchange = larrstrCol[32];
                if (larrstrCol[33] != "")
                    s.AbsoluteLTPDiff = Convert.ToInt32(Convert.ToDouble(larrstrCol[33]) * Math.Pow(10, DecimalPnt));
                if (larrstrCol[34] != "" && larrstrCol[34] !=" ")
                    s.OI = Convert.ToInt32(larrstrCol[34]);
                if (larrstrCol[35] != "")
                    s.UnderlyingLTP = Convert.ToInt32(Convert.ToDouble(larrstrCol[35]) * Math.Pow(10, DecimalPnt));
                s.LastUpdatedTime = larrstrCol[36];
                s.ScripID = larrstrCol[37];
                s.Series = larrstrCol[38];
               
                if (larrstrCol.Length > 39)
                {
                    s.InstrumentType = larrstrCol[39];
                    if (larrstrCol[48] == "NSE" || larrstrCol[48] == "NCDEX" || larrstrCol[48] == "MCX")
                        s.ScriptCode_BseToken_NseToken = Convert.ToInt32(larrstrCol[5]);
                    if (larrstrCol[48] == "BSE")
                        s.ScriptCode_BseToken_NseToken = Convert.ToInt32(larrstrCol[6]);
                    s.ExpiryDate = larrstrCol[40];
                    if (larrstrCol[41] != "")
                        s.StrikePrice = Convert.ToInt32(Convert.ToDouble(larrstrCol[41]) * Math.Pow(10, DecimalPnt));
                    s.OptionType = larrstrCol[42];
                    s.ISINNumber = larrstrCol[43];
                    s.PQFactor = larrstrCol[44];
                    s.BSESymbol = larrstrCol[45];
                    if (larrstrCol[46] != "")
                        s.MarketLot = Convert.ToInt32(larrstrCol[46]);
                    s.DaysToExpiry = larrstrCol[47];
                    s.ExchangeName = larrstrCol[48];
                    s.MarketName = larrstrCol[49];
                    s.Multiplier = larrstrCol[50];
                    s.PQFactor = larrstrCol[51];
                    s.TradingUnit = larrstrCol[52];
                    s.ExpiryDateDisplay = larrstrCol[53];
                    s.OpenInterestHigh = larrstrCol[55];
                    s.OpenInterestLow = larrstrCol[56];
                    s.SecurityKey_MW_EMT_Token = larrstrCol[57];
                    if (larrstrCol[60] != "" && larrstrCol[60] !="0.00")
                        s.Yield = Convert.ToInt32(larrstrCol[60]);
                    if (larrstrCol[61] != "")
                        s.Coupon = Convert.ToInt32(Convert.ToDouble(larrstrCol[61]) * Math.Pow(10, DecimalPnt));
                    s.NoOfDays = larrstrCol[62];
                    if (larrstrCol[63] != "")
                        s.FutureLTP = Convert.ToInt32(Convert.ToDouble(larrstrCol[63]) * Math.Pow(10, DecimalPnt));
                    s.VolLTP = larrstrCol[64];
                    s.VolBid = larrstrCol[65];
                    s.VolAsk = larrstrCol[66];
                  
                }
                Model.ScripDetails Newobj = MarketWatchConstants.objScripDetails.Where(p => p.CommonToken == larrstrCol[4]).FirstOrDefault();
                if(Newobj==null)
                MarketWatchConstants.objScripDetails.Add(s);
            }
            catch(Exception ex)
            {
                return s=null;
            }

            //60 messages BOW

            //s.FiftyTwoHigh = larrstrCol[];
            //s.FiftyTwoLow = larrstrCol[];
            //s.DailyPriceRange = larrstrCol[];
            //s.TrdValue = larrstrCol[];
            //s.IndicateEqPrice = larrstrCol[];
            //s.IndicateEqQty = larrstrCol[];
            //s.BuyAvgPrice = larrstrCol[];
            //s.SellAvgPrice = larrstrCol[];
            //s.CA = larrstrCol[];
            //s.Numerator = larrstrCol[];
            //s.Denominator = larrstrCol[];
            //for (int i = 0; i < 5; i++)
            //{
            //    Model.BestFive objbstfive = new Model.BestFive();
            //    objbstfive.BuyRateL = Br.Det[i].BuyRate_l;
            //    objbstfive.BuyQtyL = Br.Det[i].BuyQty_l;
            //    objbstfive.NoOfBidBuyL = Br.Det[i].NoOfBidBuy_l;
            //    objbstfive.FillerBuyL = Br.Det[i].FillerBuy_l;

            //    objbstfive.SellRateL = Br.Det[i].SellRate_l;
            //    objbstfive.SellQtyL = Br.Det[i].SellQty_l;
            //    objbstfive.NoOfBidSellL = Br.Det[i].NoOfBidSell_l;
            //    objbstfive.FillerSellL = Br.Det[i].FillerSell_l;

            //    s.listBestFive.Add(objbstfive);
            //}



             return s;
        }

        //private void DownloadOrders_Click()
        //{
        //     string[] mstrListOrderParameters = new string[7];
        //string[] mstrListOrderValues = new string[7];
        //string lstrResult = null;
        //    string lstrResult1 = null;
        //    string lstrDirectory = Application.StartupPath + "\\Downloads";
        //    string lstrUserId = null;
        //    bool lblnFileWritten = false;
        //    string[] lstrDownloadResult = null;
        //    string lstrFileName = null;
        //    string lstrFileNameList = "";
        //    string lstrData = null;
        //    int lintIndex = 0;
        //    StreamWriter lobjStreamWriter = default(StreamWriter);
        //    try
        //    {
        //        //:If Downloads directory does not exist then create the directory
        //        if (Directory.Exists(lstrDirectory) == false)
        //        {
        //            Directory.CreateDirectory(lstrDirectory);
        //        }

        //        lstrUserId = objUtilityLoginDetails.UserBackOfficeId;//ctlBackOffice.SubUserBackOfficeId;

        //        mstrListOrderParameters(0) = Constants.SearchConstants.PARAMUSERID;
        //        mstrListOrderValues(0) = lstrUserId;
        //        mstrListOrderParameters(1) = Constants.OrderConstants.PARAMORDERSOURCE;
        //        mstrListOrderValues(1) = gobjUtilityEMS.GetExchangeId(cboExchange.Text);
        //        mstrListOrderParameters(2) = Constants.OrderConstants.PARAMORDERMARKET;
        //        mstrListOrderValues(2) = gobjUtilityEMS.GetMarketId(cboMarket.Text);

        //        this.Cursor = Windows.Forms.Cursors.WaitCursor;
        //        lstrResult = GetDataFromServer("DownloadOrderFile", mstrListOrderParameters, mstrListOrderValues);
        //        if ((Convert.ToInt16(lstrResult.Substring(0, 1)) > 0))
        //        {
        //            this.Cursor = Windows.Forms.Cursors.Default;
        //            lstrDownloadResult = lstrResult.Split("|");
        //            MyMessage.Show(lstrDownloadResult(1), gstrMyTitle, this.Text, CustomDialogIcons.Warning);
        //            return;
        //        }
        //        else
        //        {
        //            //: Break-up into 2 files
        //            //:Check for filename and data
        //            if (lstrResult.IndexOf("~") != -1)
        //            {
        //                lstrResult1 = lstrResult.Substring(lstrResult.IndexOf('~') + 1);
        //                lstrDownloadResult = lstrResult1.Split('~');
        //                if (lstrDownloadResult.Length > 0)
        //                {
        //                    for (int lintCount = 0; lintCount <= lstrDownloadResult.Length - 1; lintCount++)
        //                    {
        //                        lstrFileName = "";
        //                        lstrData = "";
        //                        lintIndex = lstrDownloadResult(lintCount).IndexOf("|");
        //                        if (lintIndex > 0)
        //                        {
        //                            lstrFileName = lstrDownloadResult(lintCount).Substring(0, lintIndex);
        //                            if (!string.IsNullOrEmpty(lstrFileName.Trim))
        //                            {
        //                                lstrData = lstrDownloadResult(lintCount).Substring(lintIndex + 1);
        //                                SaveDwnldOrders.InitialDirectory = lstrDirectory;
        //                                SaveDwnldOrders.FileName = lstrFileName;
        //                                SaveDwnldOrders.Filter = "All files (*.*)|*.*";
        //                                if ((SaveDwnldOrders.ShowDialog == DialogResult.OK))
        //                                {
        //                                    if ((lstrData != null) && !string.IsNullOrEmpty(lstrData.Trim))
        //                                    {
        //                                        lobjStreamWriter = new StreamWriter(SaveDwnldOrders.FileName);
        //                                        lobjStreamWriter.WriteLine(lstrData);
        //                                        lobjStreamWriter.Flush();
        //                                        lobjStreamWriter.Close();
        //                                        lobjStreamWriter = null;
        //                                        lblnFileWritten = true;
        //                                        lstrFileNameList += lstrFileName + Constants.vbCrLf;
        //                                    }
        //                                }
        //                                else
        //                                {
        //                                    return;
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        if (lblnFileWritten == true)
        //        {
        //            MessageBox.Show("Downloaded the follwing Order file/files : " + Constants.vbCrLf + lstrFileNameList, gstrMyTitle, this.Text, CustomDialogIcons.Information);
        //        }
        //        else
        //        {
        //            MessageBox.Show("Unable to Download data for selected parameters", gstrMyTitle, this.Text, CustomDialogIcons.Information);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //this.Cursor = Windows.Forms.Cursors.Default;
        //        MyMessage.Show("Error in Download Orders : " + ex.Message, gstrMyTitle, this.Text, CustomDialogIcons.Warning);
        //    }
        //    //this.Cursor = Windows.Forms.Cursors.Default;
        //}


    }
#endif
}



