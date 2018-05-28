using CommonFrontEnd;
using CommonFrontEnd.Common;
using CommonFrontEnd.Controller;
using CommonFrontEnd.Controller.Login;
using CommonFrontEnd.GetDataForStock;
using CommonFrontEnd.Global;
using CommonFrontEnd.Infrastructure;
using CommonFrontEnd.Model;
using CommonFrontEnd.Model.Trade;
using CommonFrontEnd.Processor;
using CommonFrontEnd.SharedMemories;
using CommonFrontEnd.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using static CommonFrontEnd.MemoryManager;
using System.Text.RegularExpressions;

namespace CommonFrontEnd.ViewModel
{



#if TWS
    public partial class LoginScreenVM : INotifyPropertyChanged
    { 
    #region Properties    

      
        private string _MemberID;

        public string MemberID
        {
            get { return _MemberID; }
            set { _MemberID = value; NotifyPropertyChanged("MemberID"); }
        }

        private string _TraderID;

        public string TraderID
        {
            get { return _TraderID; }
            set { _TraderID = value; NotifyPropertyChanged("TraderID"); }
        }

        private string _Password;

        public string Password
        {
            get { return _Password; }
            set { _Password = value; NotifyPropertyChanged("Password"); }
        }



        public string IMLName = "imlPro";
        public static Process process = new Process();
        public ProcessStartInfo info = null;
        DirectoryInfo IMLPath = new DirectoryInfo(Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"imlPro.exe")));

        private static bool _EquitySegChk;

        public static bool EquitySegChk
        {
            get { return _EquitySegChk; }
            set
            {
                _EquitySegChk = value;
                UtilityLoginDetails.GETInstance.IsEqChecked = value;
                NotifyStaticPropertyChanged("EquitySegChk");
                if (value == true)
                    EquityBrdChk = true;
            }
        }

        private static bool _EquityBrdChk;

        public static  bool EquityBrdChk
        {
            get { return _EquityBrdChk; }
            set
            {
                _EquityBrdChk = value;
                NotifyStaticPropertyChanged("EquityBrdChk");
            }
        }

    private string _twsVisibility;

    public string TwsVisibility
    {
        get { return _twsVisibility; }
        set { _twsVisibility = value; NotifyPropertyChanged("TwsVisibility"); }
    }
    private string _bowVisibility;

    public string BowVisibility
    {
        get { return _bowVisibility; }
        set { _bowVisibility = value; NotifyPropertyChanged("BowVisibility"); }
    }

    private string _loginID;

    public string LoginID
    {
        get { return _loginID; }
        set { _loginID = value; NotifyPropertyChanged("LoginID"); }
    }


    #endregion

    #region Relay Commands

    private RelayCommand _btn_Login;

        public RelayCommand Btn_Login
        {
            get
            {
                return _btn_Login ?? (_btn_Login = new RelayCommand(
                    (object e) => Btn_Login_Click(e)));

            }

        }

        private RelayCommand _txtMemberId_TextChanged;

        public RelayCommand txtMemberId_TextChanged
        {
            get
            {
                return _txtMemberId_TextChanged ?? (_txtMemberId_TextChanged = new RelayCommand((object e) => MemberIdValidation(e)));
            }

        }

        private RelayCommand _txtTraderId_TextChanged;

        public RelayCommand txtTraderId_TextChanged
        {
            get
            {
                return _txtTraderId_TextChanged ?? (_txtTraderId_TextChanged = new RelayCommand((object e) => TraderIdValidation(e)));
            }

        }

        private RelayCommand _LoginPasswordTWS;

        public RelayCommand LoginPasswordTWS
        {
            get
            {
                return _LoginPasswordTWS ?? (_LoginPasswordTWS = new RelayCommand((object e) => GetLoginPassword(e)));
            }

        }

    #endregion


    #region Methods

    public LoginScreenVM()
    {

        ReadConfigurations.ReadETIStructure();
        this.InvokeIML();
        TwsVisibility = "Visible";
        BowVisibility = "Hidden";
    }

       private void Btn_Login_Click(object e)
        {
            try
            {

           
                PasswordBox pwBox = e as PasswordBox;
                string pwd = UtilityLoginDetails.GETInstance.DecryptedPassword = pwBox.Password;

                bool validate = ValidateInputFields(pwd);
                if (!validate)
                {
                    return;
                }

                //Register Event for receiving Logon Response
              //  MemoryManager.OnLogonReplyReceived += MemoryManager_OnLogonReplyReceived;
                LoginProcessor oLoginProcessor = new LoginProcessor();

                MemoryManager.InitializeDefaultMemory();

                LogonRequest oLogonRequest = new LogonRequest();
             

                //initialze socket
                AsynchronousClient.StartClient();

                //initiate receive
                ReceiverController oReceiverController = new ReceiverController();
                oReceiverController.ReceiveMessage();

                //initiate UMS
                UMSController oUMSController = new UMSController();
                oUMSController.ReceiveUMSMessage();

                //if (BcastEquityChkBx)
                //{
                //    //start receiving broadcast
                //    BroadCastProcessor.objBroadcastController = new BroadcastController();
                //    BroadCastProcessor.objBroadcastController.ConnectToBroadCastServer();
                //}

                if (EquitySegChk)
                {
                    //oLogonRequest.Exchange = 1; //1 - Equity, 2- Derv., 3. Curr
                    CreateEQXloginRequest(pwd, ref oLogonRequest);
                    oLoginProcessor.ProcessData(oLogonRequest);
                }


                //if (DerivativeChkBx)
                //{
                //    CreateDERloginRequest(pwd, ref oLogonRequest);
                //    oLoginProcessor.ProcessData(oLogonRequest);
                //}
                //if (CurrencyChkBx)
                //{
                //    CreateCURloginRequest(pwd, ref oLogonRequest);
                //    oLoginProcessor.ProcessData(oLogonRequest);

                //}
    }
             catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
            }
        
           
        }

    public void CreateEQXloginRequest(string pwd, ref LogonRequest oLogonRequest)
    {

        oLogonRequest.MemberID = UtilityLoginDetails.GETInstance.MemberId = Convert.ToUInt16(MemberID);
        oLogonRequest.TraderID = UtilityLoginDetails.GETInstance.TraderId = Convert.ToUInt16(TraderID); ;//204;
        oLogonRequest.Password = pwd;
        oLogonRequest.MessageTag = MemoryManager.GetMesageTag();
        oLogonRequest.Market = 1;//1 - Equity, 2- Derv., 3. Curr
        oLogonRequest.Exchange = 1;//1- BSE, 2-BOW, 3-NSE
        oLogonRequest.NewPassword = "";
        oLogonRequest.Filler_c = "";


    }
    private bool ValidateInputFields(string pwd)
        {
            if (!EquitySegChk)
            {
                MessageBox.Show("Please Check the Segment CheckBox", "Segment Required", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            else if (string.IsNullOrWhiteSpace(MemberID))
            {
                MessageBox.Show("Please Enter Valid MemberID", "MemberID Required", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            else if (!MemberID.All(c => "0123456789".Contains(c)))
            {
                MessageBox.Show("Please Enter Numeric Member ID", "Numeric MemberID Required", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            else if (!TraderID.All(c => "0123456789".Contains(c)))
            {
                MessageBox.Show("Please Enter Numeric Trader Id", "Numeric TraderId Required", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            else if (string.IsNullOrWhiteSpace(TraderID))
            {
                MessageBox.Show("Please Enter Valid TraderID", "TraderID Required", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            else if (string.IsNullOrWhiteSpace(pwd))
            {
                MessageBox.Show("Please Enter Valid Password", "Password Required", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;

        }



        public void MemoryManager_OnLogonReplyReceived(object objReply)
        {
            //AdvancedTWS.SharedMemories.SettingsManager.Initialize();
            //AdvancedTWS.SharedMemories.ReadConfigurations.ReadDefaultConfigurations();
            LogonReply oLogonReply = new LogonReply();
            oLogonReply = objReply as LogonReply;

            UtilityLoginDetails.GETInstance.TodaysDateTime = new DateTime(oLogonReply.Year, oLogonReply.Month, oLogonReply.Day, oLogonReply.Hour, oLogonReply.Minute, oLogonReply.Second, oLogonReply.Msecond, DateTimeKind.Local);

            System.Windows.Threading.DispatcherTimer myDispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            myDispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 1000);
            myDispatcherTimer.Tick += MyDispatcherTimer_Tick;
            myDispatcherTimer.Start();

            SettingsManager.Initialize();
            AssignRole();


            MessageBox.Show("Logged on with MemberID: " + UtilityLoginDetails.GETInstance.MemberId + " and TraderID: " + UtilityLoginDetails.GETInstance.TraderId);

            //tRADE rEQUEST cOMMENTED

            //LoginProcessor oLoginProcessor = new LoginProcessor();
            //TradeRequest oTradeRequest = new TradeRequest();


            //// AdvancedTWS.ViewModelMainWindowVM.LoadNPProgressBar(true);
            //if (oLogonReply.Market == 1)//Equity
            //{
            //    UtilityLoginDetails.GETInstance.IsEQXloggedIn = true;
            //    oTradeRequest.AppBeginSequenceNum = 1;
            //    oTradeRequest.AppEndSequenceNum = 1;
            //    oTradeRequest.Exchange = 1;
            //    oTradeRequest.Market = 1;
            //    oTradeRequest.MessageTag = MemoryManager.GetMesageTag();
            //    oTradeRequest.PartitionID = 1;
            //    oTradeRequest.ReservedField = 0;
            //    oLoginProcessor.ProcessData(oTradeRequest);
            //}
            //tRADE rEQUEST cOMMENTED

        }

        private void MyDispatcherTimer_Tick(object sender, EventArgs e)
        {
            UtilityLoginDetails.GETInstance.TodaysDateTime = UtilityLoginDetails.GETInstance.TodaysDateTime.AddMilliseconds(1000);
        }

        protected internal void AssignRole()
        {
            if (new[] { 0, 200 }.Any(traderId => traderId == UtilityLoginDetails.GETInstance.TraderId))
            {
                UtilityLoginDetails.GETInstance.Role = Enumerations.Role.Admin.ToString();
            }
            else
            {
                UtilityLoginDetails.GETInstance.Role = Enumerations.Role.Trader.ToString();
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

        private bool ValidateInputFields(string memberID, string traderId, string pwd)
        {
            //if (!EquityChkBx)
            //{
            //    MessageBox.Show("Please Check the Segment CheckBox", "Segment Required", MessageBoxButton.OK, MessageBoxImage.Warning);
            //    return false;
            //}
            if (string.IsNullOrWhiteSpace(MemberID))
            {
                MessageBox.Show("Please Enter Valid MemberID", "MemberID Required", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            else if (string.IsNullOrWhiteSpace(TraderID))
            {
                MessageBox.Show("Please Enter Valid TraderID", "TraderID Required", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            else if (string.IsNullOrWhiteSpace(pwd))
            {
                MessageBox.Show("Please Enter Valid Password", "Password Required", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            else if (pwd.Length < 8)
            {
                MessageBox.Show("Password should be more than 8 characters", "Password Length", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }

        private void MemberIdValidation(object e)
        {
            MemberID = Regex.Replace(MemberID, "[^0-9]+", "");
        }

        private void TraderIdValidation(object e)
        {
            TraderID = Regex.Replace(TraderID, "[^0-9]+", "");
        }

        private void GetLoginPassword(object e)
        {
            PasswordBox pwd = null;
            string controlName = e.GetType().Name;
            if (controlName == "PasswordBox")
            {
                pwd = e as PasswordBox;
                Password = pwd.Password;
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

}

#elif BOW
    public partial class LoginScreenVM
    {
        UtilityLoginDetails objUtilityLoginDetails = UtilityLoginDetails.GETInstance;
        UtilityConnParameters gobjUtilityConnParameters = UtilityConnParameters.GetInstance;


        enum LOGIN_RESPONSE : int
        {
            LOGIN_RESULT = 0,
            LOGIN_DETAILS = 1,
            MARKET_WATCH_DETAILS = 2,
            DEFAULT_MW_ID = 3,
            SUBUSER_DETAILS = 4,
            PASSWORD_EXPIRY = 5,
            EMS = 6,
            INDICES = 7
        }

        enum LOGIN_DETAILS_RECD
        {
            UserId = 0,
            LoginKeyValue = 1,
            UserFirstName = 2,
            UserLastName = 3,
            UserBackOfficeId = 4,
            UserType = 5,
            AllowDownload = 6,
            LDBAll = 7,
            LDBSelf = 8,
            Speculative = 9,
            ProOrders = 10,
            BrokerId = 11,
            InstitutionalOrders = 12,
            DownloadScripts = 13,
            TransactionPassword = 14
        }

        enum UserSettingsEnum
        {
            onemember,
            Server = 3,
            ConnectionMode = 4,
            BroadcastMode = 5,
            InteractiveIP = 6,
            //'InteractivePort = 7
            BroadcastServerIP = 7,
            //'BroadcastServerPort = 9
            BroadcastReceiveIP = 8,
            BroadcastReceivePort = 9,
            FtpServer = 10,
            FtpUserName = 11,
            FtpPassword = 12,
            FtpPath = 13,
            FtpMastersFileName = 14,
            FtpScreensFileName = 15,
            HttpCompressed = 16,
            SocketCompressed = 17,
            //Charts = 18
            //ChartsBroadcastServerIP = 19
            //ChartsBroadcastServerPort = 20
            //ChartsQueryServerIP = 21
            //ChartsQueryServerPort = 22
            MarketWatchMaxOpenLimit = 23,
            MarketWatchTokenLimitPerMW = 24,
            TickerBSEVisible = 25,
            TickerNSEVisible = 26,
            SockPoolInteractive = 27,
            SockPoolBroadcast = 28,
            HttpScripFileLocation = 29
        }

        #region " FTP Download Details "
        //: For Automatic Downloading of SQLite Db files
        public string gstrFtpServer;
        public string gstrFtpUserName;
        public string gstrFtpPassword;
        public string gstrFtpPath = "";
        public string gstrFtpMastersFileName = "Masters.zip";
        public string gstrFtpScreensFileName = "Screens.zip";
        public string gstrFtpCommoditiesFileName;
        #endregion

        public const string EXE_VERSION = "4.3031";
        public bool gblnOrdOverTCP = false;
        public string gstrBroadcastServerIP = "";
        public string gstrInteractiveIP = "";
        private Thread mobjInteractiveStartThread;
        String gstrCurrSettingsFile = "";


        public bool gblnVSATUser;

        public long glngLockInterval = ConnSettings.DEFAULT_LOCK_INTERVAL;
        public long glngLogOutInterval = ConnSettings.DEFAULT_LOGOUT_INTERVAL;

        public int gintHTTPTimeOut = Constants.HTTP_TIMEOUT_MIN;
        public bool gblnHTTPCompressed = false;
        public bool gblnSecureConnection;

        public string SocketCompressionType
        {
            get { return SocketConnection.SocketHelper.Compression_Normal; }
            set { }
        }
        private string _twsVisibility;

        public string TwsVisibility
        {
            get { return _twsVisibility; }
            set { _twsVisibility = value; NotifyPropertyChanged("TwsVisibility"); }
        }
        private string _bowVisibility;

        public string BowVisibility
        {
            get { return _bowVisibility; }
            set { _bowVisibility = value; NotifyPropertyChanged("BowVisibility"); }
        }

        private string _loginID;

        public string LoginID
        {
            get { return _loginID; }
            set { _loginID = value; NotifyPropertyChanged("LoginID"); }
        }

        public bool gblnCombinedMBP;
        public GetDataForStocks gobjGetDataForStocks;

        //  public UtilityScript gobjUtilityScript;

        public bool gblnDirectBroadcastConfigured;


        private string _LoginPassword;

        public string LoginPassword
        {
            get { return _LoginPassword; }
            set { _LoginPassword = value; NotifyPropertyChanged("LoginPassword"); }
        }

        private RelayCommand _txtLoginId_TextChanged;

        public RelayCommand txtLoginId_TextChanged
        {
            get
            {
                return _txtLoginId_TextChanged ?? (_txtLoginId_TextChanged = new RelayCommand((object e) => LoginIdValidation(e)));
            }

        }

        private RelayCommand _txtPassword_PasswordChanged;

        public RelayCommand txtPassword_PasswordChanged
        {
            get
            {
                return _txtPassword_PasswordChanged ?? (_txtPassword_PasswordChanged = new RelayCommand((object e) => PasswordValidation(e)));
            }

        }

        private RelayCommand _btn_Login;

        public RelayCommand Btn_Login
        {
            get
            {
                return _btn_Login ?? (_btn_Login = new RelayCommand(
                    (object e) => Btn_Login_Click(e)));

            }

        }


        public LoginScreenVM()
        {
            TwsVisibility = "Hidden";
            BowVisibility = "Visible";
        }

        private void Btn_Login_Click(object e)
        {
            try
            {

                PasswordBox pwBox = e as PasswordBox;
                string pwd = UtilityLoginDetails.GETInstance.DecryptedPassword = pwBox.Password;
                string lstrParameters = null;
                string _urlLogin = "/LoginServlet";


                bool lblnHasSubUsers = false;
                bool gblnIVRS_Configured = false;
                bool lblndynamicmenu = true;
                bool lblnMastersDownload = false;
                //Dictionary<string, string> oParameters = new Dictionary<string, string>();
                //oParameters.Add("USBackOfficeId", "");
                //oParameters.Add("USLOGINID",MemberID);      
                //oParameters.Add("USPassword", pwd);
                //oParameters.Add("USTransactionPassword","");
                //oParameters.Add("version","4.3031");  
                //oParameters.Add("Login Key","");
                //oParameters.Add("Thick Client","Y");



                string[] oParameters = new string[] { "USBackOfficeId" , "USLOGINID" , "USPassword" , "USTransactionPassword" , "version" , "SecuritiesMaxSequenceId",
                    "NSEContractsMaxSequenceId","NcdexContractsMaxSequenceId","NMCEContractsMaxSequenceId","MCXContractsMaxSequenceId","BSEContractsMaxSequenceId",
                    "BSECurrencyContractsMaxSequenceId","NSECurrencyContractsMaxSequenceId","GBOTContractsMaxSequenceId","", "LoginKey" , "Thick Client" };

                string[] oParametersValue = new string[] { "", LoginID, pwd, "", "4.3031", "48", "3", "18", "0", "9", "1", "1", "2007", "0", "", "", "Y" };
                LoginIdValidation(LoginID);
                PasswordValidation(pwd);
                ReadSettingsInGlobalVars();
                lstrParameters = SettingsManager.GetDataFromServer(_urlLogin, oParameters, oParametersValue);
                RecordSplitter oRecordSplitter = new RecordSplitter(lstrParameters);

                //if Login is successfull
                if (oRecordSplitter.getField((int)LOGIN_RESPONSE.LOGIN_RESULT, 0) == "0")
                {
                    // REM: For checking license expiry
                    //if (oRecordSplitter.getField(0,1).ToString().Trim())
                    //{
                    //    MyMessage.Show(GUI.Constants.MY_TITLE & " License is getting expired on " & objRecordHelper.getField(0, 1).ToString & vbCrLf & "Please contact your vendor to extend your license", gstrMyTitle, Me.Content, CustomDialogIcons.Information);
                    //}

                    // Creating an instance of UtilityLoginDetails

                    objUtilityLoginDetails.UserId = Convert.ToInt64(oRecordSplitter.getField((int)LOGIN_RESPONSE.LOGIN_DETAILS, (int)LOGIN_DETAILS_RECD.UserId));
                    //Setting Connection Parameters
                    gobjUtilityConnParameters.UserId = Convert.ToInt64(oRecordSplitter.getField((int)LOGIN_RESPONSE.LOGIN_DETAILS, (int)LOGIN_DETAILS_RECD.UserId));
                    gobjUtilityConnParameters.LoginKeyValue = oRecordSplitter.getField((int)LOGIN_RESPONSE.LOGIN_DETAILS, (int)LOGIN_DETAILS_RECD.LoginKeyValue);
                    if (oRecordSplitter.getField((int)LOGIN_RESPONSE.LOGIN_RESULT, (int)UserSettingsEnum.Server) != "")
                    {
                        gobjUtilityConnParameters.SERVER = oRecordSplitter.getField((int)LOGIN_RESPONSE.LOGIN_RESULT, (int)UserSettingsEnum.Server);
                    }
                    objUtilityLoginDetails.LoginKeyValue = oRecordSplitter.getField((int)LOGIN_RESPONSE.LOGIN_DETAILS, (int)LOGIN_DETAILS_RECD.LoginKeyValue);
                    objUtilityLoginDetails.LoginDone = true;
                    objUtilityLoginDetails.UserFirstName = oRecordSplitter.getField((int)LOGIN_RESPONSE.LOGIN_DETAILS, (int)LOGIN_DETAILS_RECD.UserFirstName);
                    objUtilityLoginDetails.UserLastName = oRecordSplitter.getField((int)LOGIN_RESPONSE.LOGIN_DETAILS, (int)LOGIN_DETAILS_RECD.UserLastName);
                    objUtilityLoginDetails.UserBackOfficeId = oRecordSplitter.getField((int)LOGIN_RESPONSE.LOGIN_DETAILS, (int)LOGIN_DETAILS_RECD.UserBackOfficeId).Trim().ToUpper();
                    // Try and User Type and Sub Users.
                    objUtilityLoginDetails.UserType = oRecordSplitter.getField((int)LOGIN_RESPONSE.LOGIN_DETAILS, (int)LOGIN_DETAILS_RECD.UserType);
                    // gblnAllowDownload = oRecordSplitter.getField(LOGIN_RESPONSE.LOGIN_DETAILS, LOGIN_DETAILS_RECD.AllowDownload);
                    // gblnLDBAll = oRecordSplitter.getField(LOGIN_RESPONSE.LOGIN_DETAILS, LOGIN_DETAILS_RECD.LDBAll);
                    //  gblnLDBSelf = oRecordSplitter.getField(LOGIN_RESPONSE.LOGIN_DETAILS, LOGIN_DETAILS_RECD.LDBSelf);
                    Logger.WriteLog("Application Details:" + EXE_VERSION, true);
                    Logger.WriteLog("Login ID:" + objUtilityLoginDetails.UserLoginId, true);
                    Logger.WriteLog("User Type:" + objUtilityLoginDetails.UserType, true);
                }


                //Socket Connection to broadcast Server
                bool gblnBroadcastOverHTTP = false;
                if (gblnBroadcastOverHTTP == false)
                {
                    Thread objConnectThread = new Thread(SettingsManager.ConnectToServer);
                    objConnectThread.Name = "ConnectToServerOverSocket";
                    objConnectThread.Start();
                }
                else
                {
                    //gblnStopHTTPThread = false;
                    //gobjConnectOverHTTP = new Thread(gfrmMDIMain.ConnectOverHTTP);
                    //gobjConnectOverHTTP.Name = "ConnectToServerOverHTTP";
                    //gobjConnectOverHTTP.Start();
                }

                //Socket connection to TCP server
                if (gblnIVRS_Configured == false)
                {
                    if ((oRecordSplitter.getField((int)LOGIN_RESPONSE.SUBUSER_DETAILS, 0).Trim() == "Y"))
                    {
                        objUtilityLoginDetails.HasSubUsers = true;
                        lblnHasSubUsers = true;
                    }
                }

                InitializeAfterLogin(lblnHasSubUsers, lblndynamicmenu, lblnMastersDownload);



            }
            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
            }
        }

        public void InitializeAfterLogin(bool pblnHasSubUsers, bool pblnDynamicmenu, bool pblnMastersDownload)
        {
            //: Showing the panel in which the label menus have been added.

            System.Configuration.AppSettingsReader lobjreader = new System.Configuration.AppSettingsReader();
            gblnOrdOverTCP = Convert.ToBoolean(lobjreader.GetValue("OrderOverTCP", typeof(bool)));
            // if (gblnOrdOverTCP == true && gstrInteractiveIP.Trim() == gstrBroadcastServerIP.Trim())
            {
                gblnOrdOverTCP = false;
                CommonFrontEnd.Infrastructure.Logger.WriteLog("Since Interactive IP N Broadcast IP are sent hence setting Order over TCP as false.");
            }

            //:For interactive server
            if (gblnOrdOverTCP == true)
            {
                mobjInteractiveStartThread = new Thread(SettingsManager.ConnectToIntractiveServer);
                mobjInteractiveStartThread.Name = "ConnectToInteractiveServer";
                mobjInteractiveStartThread.Start();
            }
        }
        public void ReadSettingsInGlobalVars()
        {
            ConnSettings lobjConnSettings = new ConnSettings();

            try
            {
                gstrCurrSettingsFile = "LAN";
                lobjConnSettings.ReadSettings(gstrCurrSettingsFile);

                gobjUtilityConnParameters.SERVER = lobjConnSettings.Server;
                gobjUtilityConnParameters.PROXYSERVER = lobjConnSettings.ProxyServer;
                gobjUtilityConnParameters.PROXYPORT = lobjConnSettings.ProxyPort;
                gobjUtilityConnParameters.PROXYUSER = lobjConnSettings.ProxyUser;
                gobjUtilityConnParameters.PROXYPASSWORD = lobjConnSettings.ProxyPassword;
                gblnVSATUser = lobjConnSettings.VSATUser;
                gstrFtpServer = lobjConnSettings.FtpServer;
                gstrFtpUserName = lobjConnSettings.FtpUserName;
                gstrFtpPassword = lobjConnSettings.FtpPassword;
                gstrFtpPath = lobjConnSettings.FtpPath;
                gstrFtpMastersFileName = lobjConnSettings.FtpMastersFileName;
                gstrFtpScreensFileName = lobjConnSettings.FtpScreensFileName;
                gstrFtpCommoditiesFileName = lobjConnSettings.FtpCommoditiesFileName;
                glngLockInterval = lobjConnSettings.LockInterval;
                glngLogOutInterval = lobjConnSettings.LogOutTime;
                gintHTTPTimeOut = lobjConnSettings.HTTPTimeOut;
                GetDataForStocks.HttpTimeOut = lobjConnSettings.HTTPTimeOut;
                gblnHTTPCompressed = lobjConnSettings.HTTPCompressed;
                gblnSecureConnection = lobjConnSettings.UseSecureHTTP;
                SocketCompressionType = lobjConnSettings.SocketCompression;
                gblnCombinedMBP = lobjConnSettings.CombinedMBP;
                //:
                //    gobjGetDataForStocks = GetNewDataForStocksObject();
                if (gblnSecureConnection == true)
                {
                    GetDataForStocks.Use_SecureConnection = true;
                    gobjUtilityConnParameters.UserSecureHTTP = true;
                }
                else
                {
                    GetDataForStocks.Use_SecureConnection = false;
                    gobjUtilityConnParameters.UserSecureHTTP = false;
                }
                gblnDirectBroadcastConfigured = lobjConnSettings.Multicast;
                //:
            }
            catch (Exception ex)
            {
                CommonFrontEnd.Infrastructure.Logger.WriteLog("Error in ReadSettingsInGlobalVars : " + ex.Message);
            }
            finally
            {
                lobjConnSettings = null;
            }
        }

        private void LoginIdValidation(object e)
        {
            if (string.IsNullOrWhiteSpace(LoginID))
            {
                MessageBox.Show("Please Enter Valid UserID", "UserID Required", MessageBoxButton.OK, MessageBoxImage.Warning);

            }
        }

        private void PasswordValidation(object e)
        {
            string pwd = UtilityLoginDetails.GETInstance.DecryptedPassword;
            if (string.IsNullOrWhiteSpace(pwd))
            {
                MessageBox.Show("Please Enter Valid Password", "Password Required", MessageBoxButton.OK, MessageBoxImage.Warning);

            }
        }

        //public GetDataForStocks GetNewDataForStocksObject()
        //   {

        //       try
        //       {
        //           GetDataForStocks lobjGetDataForStocks = GetDataForStocks.GetInstance;
        //           if (gobjUtilityScript == null || gobjUtilityScript.SecureServlets == null)
        //           {
        //               lobjGetDataForStocks.InitializeGetDataForStock(gobjUtilityConnParameters, null);
        //           }
        //           else
        //           {
        //               lobjGetDataForStocks.InitializeGetDataForStock(gobjUtilityConnParameters, gobjUtilityScript.SecureServlets);
        //           }
        //           return lobjGetDataForStocks;
        //       }
        //       catch (Exception ex)
        //       {
        //       }
        //       return null;
        //   }

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
    }
#endif
    
}
