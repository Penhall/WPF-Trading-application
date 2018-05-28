using CommonFrontEnd;
using CommonFrontEnd.Common;
using CommonFrontEnd.Constants;
using CommonFrontEnd.Controller;
using CommonFrontEnd.Controller.Login;
using CommonFrontEnd.Controller.Order;
using CommonFrontEnd.GetDataForStock;
using CommonFrontEnd.Global;
using CommonFrontEnd.HTTPHlper;
using CommonFrontEnd.Infrastructure;
using CommonFrontEnd.Model;
using CommonFrontEnd.Model.Trade;
using CommonFrontEnd.Processor;
using CommonFrontEnd.Processor.Order;
using CommonFrontEnd.Processor.Trade;
using CommonFrontEnd.SharedMemories;
using CommonFrontEnd.Utility;
using CommonFrontEnd.View;
using CommonFrontEnd.View.Login;
using CommonFrontEnd.ViewModel.Profiling;
using CommonFrontEnd.ViewModel.Touchline;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using static CommonFrontEnd.SharedMemories.MemoryManager;
using static CommonFrontEnd.ViewModel.Touchline.MarketWatchHelper;

namespace CommonFrontEnd.ViewModel
{

    public partial class LoginScreenVM
    {
        static LoginScreen mWindow = null;
        private string _LeftPosition = "485";

        public string LeftPosition
        {
            get { return _LeftPosition; }
            set { _LeftPosition = value; NotifyPropertyChanged("LeftPosition"); }
        }

        private string _TopPosition = "175";

        public string TopPosition
        {
            get { return _TopPosition; }
            set { _TopPosition = value; NotifyPropertyChanged("TopPosition"); }
        }

        private string _Width = "500";

        public string Width
        {
            get { return _Width; }
            set { _Width = value; NotifyPropertyChanged("Width"); }
        }

        private string _pwdHint;

        public string pwdHint
        {
            get { return _pwdHint; }
            set { _pwdHint = value; NotifyPropertyChanged("pwdHint"); }
        }

        private string _Height = "750";

        public string Height
        {
            get { return _Height; }
            set { _Height = value; NotifyPropertyChanged("Height"); }
        }

        private bool _txtLoginEnability = true;

        public bool txtLoginEnability
        {
            get { return _txtLoginEnability; }
            set { _txtLoginEnability = value; NotifyPropertyChanged(nameof(txtLoginEnability)); }
        }

        private static bool _txtMemberEnability = true;

        public static bool txtMemberEnability
        {
            get { return _txtMemberEnability; }
            set { _txtMemberEnability = value; NotifyStaticPropertyChanged(nameof(txtMemberEnability)); }
        }


        private bool _LoginEnabilityOpp = false;

        public bool LoginEnabilityOpp
        {
            get { return _LoginEnabilityOpp; }
            set { _LoginEnabilityOpp = value; NotifyPropertyChanged(nameof(LoginEnabilityOpp)); }
        }

        public static DirectoryInfo TWSINIPath = new DirectoryInfo(Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"Profile/TWS.INI")));
        IniParser parser = new IniParser(TWSINIPath.ToString());
        internal static int CountLogin = 0;
        void OpenPasswordWindow()
        {
            View.Login.ChangePassword oChangePassword = System.Windows.Application.Current.Windows.OfType<View.Login.ChangePassword>().FirstOrDefault();

            if (oChangePassword != null)
            {
                //ChangePassword.Close();
                //ChangePassword = new ChangePassword();
                //ChangePassword.Activate();
                oChangePassword.Focus();
                oChangePassword.pwdBoxOldPassword.Focus();
                oChangePassword.Show();
            }
            else
            {
                oChangePassword = new View.Login.ChangePassword();
                //if (System.Windows.Application.Current.MainWindow.IsLoaded)
                oChangePassword.Owner = System.Windows.Application.Current.MainWindow;
                oChangePassword.Activate();
                oChangePassword.pwdBoxOldPassword.Focus();
                oChangePassword.Show();
            }
        }


    }

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

        private LoginProcessor _loginProcessor;
        private TradeRequestProcessor _tradeRequestProcessor;
        private TradeRequest _tradeRequest;


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
                UtilityLoginDetails.GETInstance.IsEQXChecked = value;

                UtilityLoginDetails.GETInstance.IsEqConnected = value;
                NotifyStaticPropertyChanged("EquitySegChk");
                if (value == true)
                    EquityBrdChk = true;
                if (value == false)
                { }
                CommonFunctions.setLoginStatus((int)Enumerations.Segment.Equity, value);
            }
        }



        private static LoginScreenVM _GETInstanceLogin;

        public static LoginScreenVM GETInstanceLogin
        {
            get
            {
                if (_GETInstanceLogin == null)
                {
                    _GETInstanceLogin = new LoginScreenVM();
                }
                return _GETInstanceLogin;
            }
        }

        //private static bool _DerSegChk;

        //public static bool DerSegChk
        //{
        //    get { return _DerSegChk; }
        //    set
        //    {
        //        _DerSegChk = value;
        //        UtilityLoginDetails.GETInstance.IsDERloggedIn = value;
        //        NotifyStaticPropertyChanged("DerSegChk");
        //        if (value == true)
        //            DerSegChk = true;
        //    }
        //}

        //private static bool _CurSegChk;

        //public static bool CurSegChk
        //{
        //    get { return _CurSegChk; }
        //    set
        //    {
        //        _CurSegChk = value;
        //        UtilityLoginDetails.GETInstance.IsCURloggedIn = value;
        //        NotifyStaticPropertyChanged("CurSegChk");
        //        if (value == true)
        //            CurSegChk = true;
        //    }
        //}


        private static bool _EquityBrdChk;

        public static bool EquityBrdChk
        {
            get { return _EquityBrdChk; }
            set
            {
                _EquityBrdChk = value;
                NotifyStaticPropertyChanged("EquityBrdChk");
                if (value == false)
                {
                    EquitySegChk = false;
                }

            }
        }

        private static bool _DerSegChk;

        public static bool DerSegChk
        {
            get { return _DerSegChk; }
            set
            {
                _DerSegChk = value;
                UtilityLoginDetails.GETInstance.IsDERChecked = value;

                UtilityLoginDetails.GETInstance.IsDerConnected = value;

                NotifyStaticPropertyChanged("DerSegChk");
                if (value == true)
                    DerBrdChk = true;
                if (value == false)
                { }
                CommonFunctions.setLoginStatus((int)Enumerations.Segment.Derivative, value);
            }
        }

        private static bool _DerBrdChk;

        public static bool DerBrdChk
        {
            get { return _DerBrdChk; }
            set
            {
                _DerBrdChk = value;
                NotifyStaticPropertyChanged("DerBrdChk");
                if (value == false)
                {
                    DerSegChk = false;
                }

            }
        }

        private static bool _CurSegChk;

        public static bool CurSegChk
        {
            get { return _CurSegChk; }
            set
            {
                _CurSegChk = value;
                NotifyStaticPropertyChanged(nameof(CurSegChk));
                if (value == true)
                {
                    CurBrdChk = true;
                }
                if (value == false)
                {

                }
                CommonFunctions.setLoginStatus((int)Enumerations.Segment.Currency, value);
            }
        }

        private static bool _CurBrdChk;

        public static bool CurBrdChk
        {
            get { return _CurBrdChk; }
            set
            {
                _CurBrdChk = value;
                UtilityLoginDetails.GETInstance.IsCURChecked = value;
                UtilityLoginDetails.GETInstance.IsCurrConnected = value;
                NotifyStaticPropertyChanged(nameof(CurBrdChk));
                if (value == false)
                {
                    CurSegChk = false;
                }

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

        private RelayCommand _btn_LogOffClick;

        public RelayCommand btn_LogOffClick
        {
            get
            {
                return _btn_LogOffClick ?? (_btn_LogOffClick = new RelayCommand(
                  (object e) => Btn_LogOff_Click(e)));
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


        private RelayCommand _btn_ChangePassword_Click;

        public RelayCommand btn_ChangePassword_Click
        {
            get
            {
                return _btn_ChangePassword_Click ?? (_btn_ChangePassword_Click = new RelayCommand(
                    (object e) => ChangePassword_Click(e)));

            }

        }

        private RelayCommand _myLocationChanged;

        public RelayCommand myLocationChanged
        {
            get
            {
                return _myLocationChanged ?? (_myLocationChanged = new RelayCommand(
                    (object e) => Windows_LoginScreenLocationChanged(e)));

            }
        }
        private RelayCommand _CloseWindowsOnEscape;

        public RelayCommand CloseWindowsOnEscape
        {
            get
            {
                return _CloseWindowsOnEscape ?? (_CloseWindowsOnEscape = new RelayCommand(
                   (object e) => CloseWindowsOnEscape_Click()
                       ));
            }
        }
        private void Windows_LoginScreenLocationChanged(object e)
        {
            if (CommonFrontEnd.SharedMemories.ConfigurationMasterMemory.ConfigurationDict.ContainsKey("WindowsPosition"))
            {
                BoltAppSettingsWindowsPosition oBoltAppSettingsWindowsPosition = (BoltAppSettingsWindowsPosition)CommonFrontEnd.SharedMemories.ConfigurationMasterMemory.ConfigurationDict["WindowsPosition"];
                if (oBoltAppSettingsWindowsPosition != null && oBoltAppSettingsWindowsPosition.ScrpHelpCorpAction != null && oBoltAppSettingsWindowsPosition.ScrpHelpCorpAction.WNDPOSITION != null)
                {
                    oBoltAppSettingsWindowsPosition.LOGONBOX.WNDPOSITION.Left = Convert.ToInt32(LeftPosition);
                    oBoltAppSettingsWindowsPosition.LOGONBOX.WNDPOSITION.Top = Convert.ToInt32(TopPosition);
                    oBoltAppSettingsWindowsPosition.LOGONBOX.WNDPOSITION.Right = Convert.ToInt32(Width);
                    oBoltAppSettingsWindowsPosition.LOGONBOX.WNDPOSITION.Down = Convert.ToInt32(Height);
                }
                //CommonFrontEnd.SharedMemories.SaveConfiguration.SaveUserConfiguration(@"D:\TWS_DotNetNewStructure\TWS_DOTNETT\CommonFrontEnd\bin\Debug\xml\Users\AppSettings\920200000.xml", "WindowsPosition");
                CommonFrontEnd.SharedMemories.SaveConfiguration.SaveUserConfiguration(CommonFrontEnd.SharedMemories.SettingsManager.AppSettingsXmlPath, "WindowsPosition");
            }

        }

        private RelayCommand _LoginScreenClosing;

        public RelayCommand LoginScreenClosing
        {
            get { return _LoginScreenClosing ?? (_LoginScreenClosing = new RelayCommand((object e) => LoginScreenClosing_Closing(e))); }
        }

        private void LoginScreenClosing_Closing(object e)
        {
            Windows_LoginScreenLocationChanged(e);
        }

        #endregion


            #region Methods

        private LoginScreenVM()
        {
            //Commented since invoked on MainWindowVM- Gaurav 08Jan2018
            //ReadConfigurations.ReadETIStructure();
            TwsVisibility = "Visible";
            BowVisibility = "Hidden";
            mWindow= System.Windows.Application.Current.Windows.OfType<LoginScreen>().FirstOrDefault();
            ReadMemberTraderIdINI();
            ControleEnability();

            if (CommonFrontEnd.SharedMemories.ConfigurationMasterMemory.ConfigurationDict.ContainsKey("WindowsPosition"))
            {
                CommonFrontEnd.Model.BoltAppSettingsWindowsPosition oBoltAppSettingsWindowsPosition = new Model.BoltAppSettingsWindowsPosition();
                oBoltAppSettingsWindowsPosition = (CommonFrontEnd.Model.BoltAppSettingsWindowsPosition)CommonFrontEnd.SharedMemories.ConfigurationMasterMemory.ConfigurationDict["WindowsPosition"];
                if (oBoltAppSettingsWindowsPosition != null && oBoltAppSettingsWindowsPosition.ScrpHelpCorpAction != null && oBoltAppSettingsWindowsPosition.ScrpHelpCorpAction.WNDPOSITION != null)
                {
                    Height = oBoltAppSettingsWindowsPosition.ScrpHelpCorpAction.WNDPOSITION.Down.ToString();
                    TopPosition = oBoltAppSettingsWindowsPosition.ScrpHelpCorpAction.WNDPOSITION.Top.ToString();
                    LeftPosition = oBoltAppSettingsWindowsPosition.ScrpHelpCorpAction.WNDPOSITION.Left.ToString();
                    Width = oBoltAppSettingsWindowsPosition.ScrpHelpCorpAction.WNDPOSITION.Right.ToString();
                }
            }
        }

        internal void ControleEnability()
        {
            if (UtilityLoginDetails.GETInstance.IsLoggedIN)
            {
                txtMemberEnability = false;
                txtLoginEnability = false;
                LoginEnabilityOpp = true;
            }
            else
            {
                txtMemberEnability = true;
                txtLoginEnability = true;
                LoginEnabilityOpp = false;
            }
        }
        private void CloseWindowsOnEscape_Click()
        {
            mWindow?.Close();
        }
        private void Btn_Login_Click(object e)
        {
            try
            {
                //LoginScreen loginscreen = System.Windows.Application.Current.Windows.OfType<LoginScreen>().SingleOrDefault();
                //if (loginscreen != null)
                //    loginscreen.btnTwsLoginTws.IsEnabled = false;
                txtLoginEnability = false;
                PasswordBox pwBox = e as PasswordBox;
                string pwd = UtilityLoginDetails.GETInstance.DecryptedPassword = pwBox.Password.Trim();

                bool validate = ValidateInputFields(pwd);
                if (!validate)
                {
                    txtLoginEnability = true;
                    return;
                }
                UtilityLoginDetails.GETInstance.TraderId = Convert.ToUInt16(TraderID);
                UtilityLoginDetails.GETInstance.MemberId = Convert.ToUInt16(MemberID);

                //Register Event for receiving Logon Response
                //MemoryManager.OnLogonReplyReceived += MemoryManager_OnLogonReplyReceived;
                LoginProcessor oLoginProcessor = new LoginProcessor();
                LoginRequestProcessor oLoginRequestProcessor = null;

                if (CountLogin == 0)
                {
                    CountLogin++;
                    MemoryManager.InitializeDefaultMemory();

                    LogonRequest oLogonRequest = new LogonRequest();
                    //TODO Temp
                    LogOffRequest oLogOffRequest = new LogOffRequest();
                    //UserRegistrationRequest oUserRegistrationRequest = new UserRegistrationRequest();

                    //initialze socket
                    AsynchronousClient.StartClient();

                    //initiate receive
                    ReceiverController oReceiverController = new ReceiverController();
                    oReceiverController.ReceiveMessage();

                    //initiate UMS
                    //UMSController oUMSController = new UMSController();
                    //oUMSController.ReceiveUMSMessage();

                    //Registration Request- Gaurav Jadhav 15/12/2017
                    CreateRegistrationRequest(ref oLogonRequest);
                    oLoginRequestProcessor = new LoginRequestProcessor(new UserRegistration());
                    oLoginRequestProcessor.ProcessRequest(oLogonRequest);

                    ////Login Request- Gaurav Jadhav 15/12/2017
                    CreateLoginRequest(pwd, ref oLogonRequest);
                    oLoginRequestProcessor = new LoginRequestProcessor(new LogOn());
                    oLoginRequestProcessor.ProcessRequest(oLogonRequest);
                }

                else
                {
                    LogonRequest oLogonRequest = new LogonRequest();
                    //TODO Temp
                    LogOffRequest oLogOffRequest = new LogOffRequest();
                    CreateLoginRequest(pwd, ref oLogonRequest);
                    oLoginRequestProcessor = new LoginRequestProcessor(new LogOn());
                    oLoginRequestProcessor.ProcessRequest(oLogonRequest);
                }
                ////LogOff Request testing only- Gaurav Jadhav 26/12/2017
                //CreateLogOffRequest(pwd, ref oLogOffRequest);
                //oLoginRequestProcessor = new LoginRequestProcessor(new LogOff());
                //oLoginRequestProcessor.ProcessRequest(oLogOffRequest);

                //OrderProfilingVM oOrderProfilingVM = new OrderProfilingVM();
                //oOrderProfilingVM.CheckUserInputs();

            }
            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
            }
        }

        private void ChangePassword_Click(object e)
        {
            //   if (ChngPwdBtnEnabled == true)
            OpenPasswordWindow();
        }


        public void CreateLoginRequest(string pwd, ref LogonRequest oLogonRequest)
        {
            try
            {
                oLogonRequest.EquitySegChk = EquitySegChk;
                oLogonRequest.DerSegChk = DerSegChk;
                oLogonRequest.CurSegChk = CurSegChk;
                oLogonRequest.Password = pwd;
                oLogonRequest.TraderID = UtilityLoginDetails.GETInstance.TraderId;
                oLogonRequest.MemberID = UtilityLoginDetails.GETInstance.MemberId;
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
            }

        }
        public void CreateLogOffRequest(string pwd, ref LogOffRequest oLogOffRequest)
        {
            try
            {
                oLogOffRequest.Password = pwd;
                oLogOffRequest.EquitySegChk = EquitySegChk;
                oLogOffRequest.DerSegChk = DerSegChk;
                oLogOffRequest.CurSegChk = CurSegChk;
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
            }

        }
        public void CreateRegistrationRequest(ref LogonRequest oLogonRequest)
        {
            try
            {
                if (oLogonRequest != null)
                {
                    oLogonRequest.MemberID = UtilityLoginDetails.GETInstance.MemberId = Convert.ToUInt16(MemberID);
                    oLogonRequest.TraderID = UtilityLoginDetails.GETInstance.TraderId = Convert.ToUInt16(TraderID);
                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
            }

        }
        private void ReadMemberTraderIdINI()
        {
            MemberID = parser.GetSetting("Login Settings", "MEMBERID");
            TraderID = parser.GetSetting("Login Settings", "TRADERID");
            EquitySegChk = UtilityLoginDetails.GETInstance.IsEQXChecked = Convert.ToBoolean(parser.GetSetting("Login Settings", "EQCheck"));
            DerSegChk = UtilityLoginDetails.GETInstance.IsDERChecked = Convert.ToBoolean(parser.GetSetting("Login Settings", "DrCheck"));
            CurSegChk = UtilityLoginDetails.GETInstance.IsCURChecked = Convert.ToBoolean(parser.GetSetting("Login Settings", "CurCheck"));
            EquityBrdChk = UtilityLoginDetails.GETInstance.IsEQXChecked = Convert.ToBoolean(parser.GetSetting("Login Settings", "EQBrCheck"));
            DerBrdChk = UtilityLoginDetails.GETInstance.IsDERChecked = Convert.ToBoolean(parser.GetSetting("Login Settings", "DrBrCheck"));
            CurBrdChk = UtilityLoginDetails.GETInstance.IsCURChecked = Convert.ToBoolean(parser.GetSetting("Login Settings", "CurBrCheck"));
        }
        private bool ValidateInputFields(string pwd)

        {
            if (!EquitySegChk && !DerSegChk && !CurSegChk)
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
                if (!string.IsNullOrEmpty(pwd.Password))
                    pwd.Tag = string.Empty;
                else
                    pwd.Tag = "Password";
            }
            if (System.Windows.Input.Keyboard.IsKeyToggled(System.Windows.Input.Key.CapsLock))
            {
                pwdHint = "Capslock is On";
            }
        }

        public void Btn_LogOff_Click(object e)
        {
            var response = System.Windows.MessageBox.Show("Do you really want to exit?", "Exiting...",
                                   MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
            if (response == MessageBoxResult.Yes)
            {
                try
                {
                    //LogOff Request testing only-Gaurav Jadhav 26 / 12 / 2017
                    // PasswordBox pwBox = e as PasswordBox;
                    LoginRequestProcessor oLoginRequestProcessor = null;
                    string pwd = UtilityLoginDetails.GETInstance.DecryptedPassword;
                    LogOffRequest oLogOffRequest = new LogOffRequest();
                    CreateLogOffRequest(pwd, ref oLogOffRequest);
                    oLoginRequestProcessor = new LoginRequestProcessor(new LogOff());
                    oLoginRequestProcessor.ProcessRequest(oLogOffRequest);
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
        private Thread OrderProcessThread;
        private Thread TCPManageMessageThread;
        static Thread gobjBroadCastMessagePumpThread;
        string[] OrderNameParameters = new string[25];
        string[] OrderValueParameters = new string[25];
        string[] TradeNameParameters = new string[23];
        string[] TradeValueParameters = new string[23];
        //private delegate void ListOrders(string resultstring, string ordnumber);

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

        public int gintHTTPTimeOut = BowConstants.HTTP_TIMEOUT_MIN;
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


        private string _LoginTransPassword;

        public string LoginTransPassword
        {
            get { return _LoginTransPassword; }
            set { _LoginTransPassword = value; NotifyPropertyChanged("LoginTransPassword"); }
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
                var tuple = (Tuple<object, object>)e;
                PasswordBox pwBox = tuple.Item1 as PasswordBox;
                PasswordBox pwtranBox = tuple.Item2 as PasswordBox;
                string pwd = objUtilityLoginDetails.DecryptedPassword = pwBox.Password;
                string tranPwd = objUtilityLoginDetails.GstrTransactionPwd = pwtranBox.Password;
                string lstrParameters = null;
                string _urlLogin = "/LoginServlet";


                bool lblnHasSubUsers = false;
                bool gblnIVRS_Configured = false;
                bool lblndynamicmenu = true;
                bool lblnMastersDownload = false;
                bool lblnValidateTrnsPwd = false;

                System.Configuration.AppSettingsReader lobjreader = new System.Configuration.AppSettingsReader();
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
                //string[] oParametersValue = new string[] { "", LoginID, pwd, "", "4.3031", "159", "8", "32", "0", "10", "1", "1", "2014", "0", "", "", "Y" };
                LoginIdValidation(LoginID);
                PasswordValidation(pwd);
                ReadSettingsInGlobalVars();
                lstrParameters = SettingsManager.GetDataFromServer(_urlLogin, oParameters, oParametersValue);
                RecordSplitter oRecordSplitter = new RecordSplitter(lstrParameters);

                //if Login is successfull
                if (oRecordSplitter.getField((int)LOGIN_RESPONSE.LOGIN_RESULT, 0) == "0")
                {
                    //UtilityLoginDetails.GETInstance.IsEQXloggedIn = true;
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
                    //  ProcessIndicesData(oRecordSplitter);
                    //   LoadAllIndicesWindowClick();
                    startRefreshThread();
                    StartPersonalDownload();

                }

                try
                {
                    //: Fetching the Permission from the app.config file
                    lblnValidateTrnsPwd = Convert.ToBoolean(lobjreader.GetValue("ValidateTransPwd", typeof(bool)));
                }
                catch
                {
                    Infrastructure.Logger.WriteLog("Error in Fetching the Permission form App.config file.");
                }

                //:if ValidateTransPwd key is false in app.config then assign some value to stop pwd validation in Buy/Sell control
                if (lblnValidateTrnsPwd == false)
                    objUtilityLoginDetails.GstrTransactionPwd = "PASSWORD";

                // Check if password expired
                //15- Intimation For Password Expiry
                //16- Password Expired then Prompt for Change Password
                //14- First Time Login

                int count = oRecordSplitter.numberOfRecords();
                if ((count > (int)LOGIN_RESPONSE.PASSWORD_EXPIRY))
                {

                    //Intimation For Password Expiry
                    if ((oRecordSplitter.getField((int)LOGIN_RESPONSE.PASSWORD_EXPIRY, 0) == BowConstants.PasswordExpiryIntimation))
                    {
                        //  lblnOpenDefaultMW = true;
                        MessageBox.Show("Password will expire in one day", "Change Password", MessageBoxButton.OK, MessageBoxImage.Warning);
                        //Password Expired then Prompt for Change Password
                    }
                    else if ((oRecordSplitter.getField((int)LOGIN_RESPONSE.PASSWORD_EXPIRY, 0) == BowConstants.PasswordExpired))
                    {

                        MessageBox.Show("Password expired.Please change", "Change Password", MessageBoxButton.OK, MessageBoxImage.Warning);
                        OpenPasswordWindow();

                    }

                    else if ((oRecordSplitter.getField((int)LOGIN_RESPONSE.PASSWORD_EXPIRY, 0) == BowConstants.FirstTimeLogIn))
                    {
                        //First Time Login 

                        MessageBox.Show("First Time Login.Please Change your password", "First Time Login", MessageBoxButton.OK, MessageBoxImage.Information);
                        OpenPasswordWindow();

                    }
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
                if (Globals.GETInstance.gblnMarketWatchLocal == false)
                {
                    if (oRecordSplitter.numberOfRecords() > (int)LOGIN_RESPONSE.MARKET_WATCH_DETAILS)
                    {
                        Globals.GETInstance.gintNumberOfMarketWatch = Convert.ToInt32(oRecordSplitter.getField((int)LOGIN_RESPONSE.MARKET_WATCH_DETAILS, 0));
                        UtilityLoginDetails.GETInstance.gobjMarketWatchColl = new MarketWatchCollection();
                        UtilityLoginDetails.GETInstance.gobjIndexMarketWatchColl = new MarketWatchCollection();
                        UtilityLoginDetails.GETInstance.gobjIndustrialMarketWatchColl = new MarketWatchCollection();

                        ArrayList lobjMarketWatchIdsNPositions = new ArrayList();
                        ArrayList lobjIndicesMarketWatchIdsNPositions = new ArrayList();
                        ArrayList lobjIndustriesMarketWatchIdsNPositions = new ArrayList();
                        MarketWatchHelper lobjMarketWatch;

                        for (int lintCount = 0; lintCount <= Globals.GETInstance.gintNumberOfMarketWatch - 1; lintCount++)
                        {
                            lobjMarketWatch = new MarketWatchHelper();
                            lobjMarketWatch.Id = Convert.ToInt32(oRecordSplitter.getField((int)LOGIN_RESPONSE.MARKET_WATCH_DETAILS, (lintCount * 3) + 1));
                            lobjMarketWatch.Type = oRecordSplitter.getField((int)LOGIN_RESPONSE.MARKET_WATCH_DETAILS, (lintCount * 3) + 2);
                            lobjMarketWatch.Name = oRecordSplitter.getField((int)LOGIN_RESPONSE.MARKET_WATCH_DETAILS, (lintCount * 3) + 3);
                            int UserDefinedval = (int)MWType.UserDefined;
                            int Indexval = (int)MWType.Index;
                            int Industrialval = (int)MWType.Industrial;

                            if (Convert.ToInt32(lobjMarketWatch.Type) == UserDefinedval)
                            {
                                UtilityLoginDetails.GETInstance.gobjMarketWatchColl.Add(lobjMarketWatch);
                                //lstrMWIds(lintCount) = lobjMarketWatch.Id
                                lobjMarketWatchIdsNPositions.Add(lintCount + "," + lobjMarketWatch.Id);
                            }
                            else if (Convert.ToInt32(lobjMarketWatch.Type) == Indexval)
                            {
                                UtilityLoginDetails.GETInstance.gobjIndexMarketWatchColl.Add(lobjMarketWatch);
                                lobjIndicesMarketWatchIdsNPositions.Add(lintCount + "," + lobjMarketWatch.Id);
                                //  UtilityLoginDetails.GETInstance.gintNumberOfMarketWatch -= 1;
                            }
                            else if (Convert.ToInt32(lobjMarketWatch.Type) == Industrialval)
                            {
                                UtilityLoginDetails.GETInstance.gobjIndustrialMarketWatchColl.Add(lobjMarketWatch);
                                lobjIndustriesMarketWatchIdsNPositions.Add(lintCount + "," + lobjMarketWatch.Id);
                                //  UtilityLoginDetails.GETInstance.gintNumberOfMarketWatch -= 1;
                            }
                            else
                            {
                                //    UtilityLoginDetails.GETInstance.gintNumberOfMarketWatch -= 1;
                            }
                        }

                        InitializeDefaultMemory();
                    }
                }


            }
            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
            }
        }
        private static void LoadAllIndicesWindowClick()
        {

            All_Indices oAll_Indices = Application.Current.Windows.OfType<All_Indices>().FirstOrDefault();
            if (oAll_Indices != null)
            {
                if (oAll_Indices.WindowState == WindowState.Minimized)
                    oAll_Indices.WindowState = WindowState.Normal;

                oAll_Indices.Focus();
                oAll_Indices.Show();
            }
            else
            {
                oAll_Indices = new All_Indices();
                oAll_Indices.Owner = System.Windows.Application.Current.MainWindow;
                //objswift.CmbExcangeType.Focus();
                oAll_Indices.Activate();
                oAll_Indices.Show();
            }
        }

        private void ProcessIndicesData(RecordSplitter oRecordSplitter)
        {
            // indicesDataDict = new Dictionary<int, IndexData>();

            if (oRecordSplitter != null)
            {
                int i = 15;
                for (int j = 0; j < 62; j++)
                {
                    IndexData oIndexdata = new IndexData();
                    oIndexdata.code = Convert.ToInt32(oRecordSplitter.records[i][j]);
                    oIndexdata.filler1 = Convert.ToInt32(oRecordSplitter.records[i + 1][j]);
                    oIndexdata.filler2 = Convert.ToInt32(oRecordSplitter.records[i + 2][j]);
                    oIndexdata.Id = oRecordSplitter.records[i + 3][j];
                    oIndexdata.name = oRecordSplitter.records[i + 4][j];
                    oIndexdata.filler3 = oRecordSplitter.records[i + 5][j];
                    oIndexdata.value = Convert.ToDecimal(oRecordSplitter.records[i + 6][j]);
                    oIndexdata.chngValue = Convert.ToDecimal(oRecordSplitter.records[i + 7][j]);
                    BroadcastMasterMemory.indicesDataDict.Add(oIndexdata.code, oIndexdata);

                }
            }
        }

        public void InitializeAfterLogin(bool pblnHasSubUsers, bool pblnDynamicmenu, bool pblnMastersDownload)
        {
            //: Showing the panel in which the label menus have been added.

            System.Configuration.AppSettingsReader lobjreader = new System.Configuration.AppSettingsReader();

            Globals.GETInstance.gblnIsNowModel = Convert.ToBoolean(lobjreader.GetValue("NowModel", typeof(bool)));
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


        private void startRefreshThread()
        {
            MemoryManager.OrderQueue = new ConcurrentQueue<RecordSplitter>();
            //mstrNetPositionRefreshData = new Concurrent.ConcurrentQueue<string>();
            //mstrNetPositionInsertData = new Concurrent.ConcurrentQueue<string>();
            //mstrMTMToolbarRefreshData = new Concurrent.ConcurrentQueue<string>();
            //mstrLTPData = new Concurrent.ConcurrentQueue<string>();
            //mstrMBPData = new Concurrent.ConcurrentQueue<SocketConnection.MessageArrivedEventArgs>();

            //TCP Receiveing Thread
            if (TCPManageMessageThread != null)
            {
                TCPManageMessageThread.Abort();
                TCPManageMessageThread = null;
            }

            TCPManageMessageThread = new Thread(SettingsManager.ManageAsynchronousBroadcast);
            TCPManageMessageThread.Name = "Manage InComming Messages";
            TCPManageMessageThread.Start();

            //Order Fetching Thread
            if (OrderProcessThread != null)
            {
                OrderProcessThread.Abort();
                OrderProcessThread = null;
            }
            OrderProcessThread = new Thread(OrderProcessor.processOrderQueue);
            OrderProcessThread.Name = "Order Refresh Thread";
            OrderProcessThread.Priority = ThreadPriority.AboveNormal;
            OrderProcessThread.Start();

            //Indices Fetching Thread
            if (gobjBroadCastMessagePumpThread != null)
            {
                gobjBroadCastMessagePumpThread.Abort();
                gobjBroadCastMessagePumpThread = null;
            }
            gobjBroadCastMessagePumpThread = new Thread(SettingsManager.BroadcastMessagePump);
            gobjBroadCastMessagePumpThread.Name = "BroadcastMessagePump";
            gobjBroadCastMessagePumpThread.Priority = ThreadPriority.AboveNormal;
            gobjBroadCastMessagePumpThread.Start();

            //if ((mNetPositionRefreshThread != null))
            //{
            //    mNetPositionRefreshThread.Abort();
            //    mNetPositionRefreshThread = null;
            //}
            //mNetPositionRefreshThread = new Thread(processNetPositionRefreshData);
            //mNetPositionRefreshThread.Name = "Net Position Book Refresh Thread";
            //mNetPositionRefreshThread.Priority = ThreadPriority.AboveNormal;
            //mNetPositionRefreshThread.Start();

            //if ((mNetPositionInsertThread != null))
            //{
            //    mNetPositionInsertThread.Abort();
            //    mNetPositionInsertThread = null;
            //}
            //mNetPositionInsertThread = new Thread(processNetpositionInsertData);
            //mNetPositionInsertThread.Name = "Net position insert thread";
            //mNetPositionInsertThread.Start();


            //if ((mUpdateLTPThread != null))
            //{
            //    mUpdateLTPThread.Abort();
            //    mUpdateLTPThread = null;
            //}
            //mUpdateLTPThread = new Thread(processUpdateLTP);
            //mUpdateLTPThread.Name = "Update LTP";
            //mUpdateLTPThread.Start();

            //if ((mUpdateMBPThread != null))
            //{
            //    mUpdateMBPThread.Abort();
            //    mUpdateMBPThread = null;
            //}
            //mUpdateMBPThread = new Thread(processUpdateMBP);
            //mUpdateMBPThread.Name = "Update MBP";
            //mUpdateMBPThread.Start();
        }

        private void StartPersonalDownload()
        {
            bool gblnBooksOverTCP = false;
            string pstrOrdNum = "";
            SetNameParameters();
            //TODO: Call TCP connection accordingly to download Orders

            if (gblnBooksOverTCP == true)
            {
                //if (QueryServerOverTCP(GetRequestBookFilterParameters) == false)
                //{
                //    GetDataByCallingServlet(Constants.OrderConstants.URL_LIST_ORDER, mstrNameParameters, mstrValueParameters, false, "", pstrOrdNum);
                //}
            }
            //TODO: Call TCP connection accordingly to download Orders
            else
            {
               // GetDataForStocks.GetDataFromServer()
                 GetDataByCallingServlet(BowServletsConstants.URL_LIST_ORDER, OrderNameParameters, OrderValueParameters, false, "", pstrOrdNum);
            }
            //TODO: trade personal Download
            //if (chkShowCombinedBook.Checked == true)
            //{
            //    GetDataByCallingServlet(BowServletsConstants.URL_LIST_TRADE, TradeNameParameters, TradeValueParameters, true);
            //}
            //TODO: trade personal Download

            //TODO: Call servlet to download Trades
        }

        private void SetNameParameters()
        {
            OrderNameParameters[0] = BowConstants.PARAMEXCHANGE;
            OrderNameParameters[1] = BowConstants.PARAMSEGMENT;
            OrderNameParameters[2] = BowConstants.PARAMSYMBOL;
            OrderNameParameters[3] = BowConstants.PARAMTOKEN;
            OrderNameParameters[4] = BowConstants.PARAMMARKET;
            OrderNameParameters[5] = BowConstants.PARAMSERIES;
            OrderNameParameters[6] = BowConstants.PARAMINSTRUMENTTYPE;
            OrderNameParameters[7] = BowConstants.PARAMOPTIONTYPE;
            OrderNameParameters[8] = BowConstants.PARAMUSER;
            OrderNameParameters[9] = BowConstants.PARAMACCOUNT;
            OrderNameParameters[10] = BowConstants.PARAMINTRADAY;
            OrderNameParameters[11] = BowConstants.PARAMBUYSELLINDICATOR;
            OrderNameParameters[12] = BowConstants.PARAMGROUP;
            OrderNameParameters[13] = BowOrderBean.f_BookType;
            OrderNameParameters[14] = BowConstants.PARAMAGGREGATE;
            OrderNameParameters[15] = BowOrderBean.f_Status;
            OrderNameParameters[16] = BowConstants.PARAMEXPIRYDATE;
            OrderNameParameters[17] = BowConstants.PARAMORDERNUMBER;
            OrderNameParameters[18] = "StartTime";
            OrderNameParameters[19] = "EndTime";
            OrderNameParameters[20] = BowConstants.PARAMSTRIKEPRICE;

            //mstrUnsentOrdersNameParameters(0) = "oeid";
            //mstrUnsentOrdersNameParameters(1) = "whattodo";


            TradeNameParameters[0] = BowConstants.PARAMEXCHANGE;
            TradeNameParameters[1] = BowConstants.PARAMSEGMENT;
            TradeNameParameters[2] = BowConstants.PARAMTOKEN;
            TradeNameParameters[3] = BowConstants.PARAMSYMBOL;
            TradeNameParameters[4] = BowConstants.PARAMSERIES;
            TradeNameParameters[5] = BowConstants.PARAMMARKET;
            TradeNameParameters[6] = BowConstants.PARAMINSTRUMENTTYPE;
            TradeNameParameters[7] = BowConstants.PARAMOPTIONTYPE;
            TradeNameParameters[8] = BowConstants.PARAMUSER;
            TradeNameParameters[9] = BowConstants.PARAMACCOUNT;
            TradeNameParameters[10] = BowConstants.PARAMINTRADAY;
            TradeNameParameters[11] = BowConstants.PARAMBUYSELLINDICATOR;
            TradeNameParameters[12] = BowConstants.PARAMGROUP;
            TradeNameParameters[13] = "TRBookType";
            TradeNameParameters[14] = BowConstants.PARAMAGGREGATE;
            TradeNameParameters[15] = BowConstants.PARAMEXPIRYDATE;
            TradeNameParameters[16] = BowConstants.PARAMORDERNUMBER;
            TradeNameParameters[17] = BowConstants.PARAMSTRIKEPRICE;
        }

        private bool GetDataByCallingServlet(string pstrServletName, string[] pstrParameterNames, string[] pstrParameterValues, bool pblnCalled_For_Trade, string pstrCancelledFrom = "", string pstrOrderNumber = "")
        {
            //: Call the servlet 
            try
            {
                //: A callback will be made to the PersonalDownloadOrderReceived Function
                //: lstrResult will return blank value
                bool lblnSendOverInteractiveIP = false;
                //ListOrders listOrders = null;
                //object o = null;
                object lobjDelegate = null;
                bool lblnOrdBookstatus = false;
                //EnabledDisable(false);
                switch (pstrCancelledFrom)
                {
                    case "":
                        if (pblnCalled_For_Trade == false)
                        {
                            //listOrders = new HTTPHlpr.ResponseReturnedForOrder(OrderProcessor.PersonalDownloadOrderReceived);
                            lobjDelegate = new HTTPHlpr.ResponseReturnedForOrder(OrderProcessor.PersonalDownloadOrderReceived);
                            lblnOrdBookstatus = true;
                        }
                        else
                        {
                            //TODO: Trade Personal download
                            //lobjDelegate = new HTTPHlpr.ResponseReturned(Targetmethod for trade);
                            //TODO: Trade Personal download
                        }
                        break;
                    case "CANCELALL":
                        lblnSendOverInteractiveIP = true;
                        //TODO: Cancel All 
                        // lobjDelegate = new HTTPHlpr.ResponseReturned(AsynchronousCancellALL);
                        //TODO: Cancel All 
                        break;
                    case "CANCELUNSEND":
                        //TODO: Cancel Unsend 
                        //lobjDelegate = new HTTPHlpr.ResponseReturned(AsynchronousCancellALL);
                        //TODO: Cancel Unsend
                        break;
                    case "RELEASEALL":
                        //TODO: Release All 
                        //lobjDelegate = new HTTPHlpr.ResponseReturned(AsynchronousCancellALL);
                        //TODO: Release All 
                        break;
                }
                if ((lobjDelegate != null))
                {
                    if (lblnSendOverInteractiveIP == true)//&& gblnSendOrderOverInteractiveIP == true)
                    {
                        //TODO: Check and call interactive server accordingly
                        //GetDataFromInteractiveServer(pstrServletName, pstrParameterNames, pstrParameterValues, false, lobjDelegate, lblnOrdBookstatus, pstrOrderNumber);
                        //TODO: Check and call interactive server accordingly
                    }
                    else
                    {
                        SettingsManager.GetDataFromServer(pstrServletName, pstrParameterNames, pstrParameterValues, false, (Delegate)lobjDelegate, lblnOrdBookstatus, pstrOrderNumber);
                    }

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                if (pblnCalled_For_Trade == false)
                {
                    Infrastructure.Logger.WriteLog("frmListOrders GetDataByCallingServlet Exception " + ex.Message);
                }
                else
                {
                    Infrastructure.Logger.WriteLog("frmListOrders GetDataByCallingServlet Exception for Trades " + ex.Message);
                }
                return false;
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
