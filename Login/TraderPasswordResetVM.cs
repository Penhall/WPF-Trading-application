using CommonFrontEnd.Common;
using CommonFrontEnd.Global;
using CommonFrontEnd.Model;
using CommonFrontEnd.Processor;
using CommonFrontEnd.SharedMemories;
using CommonFrontEnd.View.Login;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CommonFrontEnd.ViewModel.Login
{
#if TWS
    public class TraderPasswordResetVM
    {
        #region Properties
        System.Windows.Controls.PasswordBox oDefaultPasswordBox;
        System.Windows.Controls.PasswordBox oNewPasswordBox;
        System.Windows.Controls.PasswordBox oConfirmPwd;
        private static string _DefaultPwd;

        public static string DefaultPwd
        {
            get { return _DefaultPwd; }
            set { _DefaultPwd = value; NotifyStaticPropertyChanged("DefaultPwd"); }
        }

        //public string DefaultPwd { get; set; }// admin change password

        public static DirectoryInfo ResetPwdINIPath = new DirectoryInfo(Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"Profile/ResetPwd.ini")));
        IniParser parser = new IniParser(ResetPwdINIPath.ToString());
#if TWS
        private LoginRequestProcessor oLoginRequestProcessor;
#endif
        private string _LeftPosition = "350";

        public string LeftPosition
        {
            get { return _LeftPosition; }
            set { _LeftPosition = value; NotifyPropertyChanged("LeftPosition"); }
        }

        private string _TopPosition = "205";

        public string TopPosition
        {
            get { return _TopPosition; }
            set { _TopPosition = value; NotifyPropertyChanged("TopPosition"); }
        }

        private string _Width = "450";

        public string Width
        {
            get { return _Width; }
            set { _Width = value; NotifyPropertyChanged("Width"); }
        }
        private static string _txtReply;

        public static string txtReply
        {
            get { return _txtReply; }
            set { _txtReply = value; NotifyStaticPropertyChanged("txtReply"); }
        }


        private string _Height = "358";

        public string Height
        {
            get { return _Height; }
            set { _Height = value; NotifyPropertyChanged("Height"); }
        }

        private string _OldPassword;

        public string OldPassword
        {
            get { return _OldPassword; }
            set { _OldPassword = value; NotifyPropertyChanged("OldPassword"); }
        }

        private static string _PanelDefaultPwdVisibility;

        public static string PanelDefaultPwdVisibility
        {
            get { return _PanelDefaultPwdVisibility; }
            set { _PanelDefaultPwdVisibility = value; NotifyStaticPropertyChanged("PanelDefaultPwdVisibility"); }
        }

        private static string _PanelNewPwdVisibility;
        public static string PanelNewPwdVisibility
        {
            get { return _PanelNewPwdVisibility; }
            set { _PanelNewPwdVisibility = value; NotifyStaticPropertyChanged("PanelNewPwdVisibility"); }
        }

        private static string _PanelConfirmPwdVisibility;

        public static string PanelConfirmPwdVisibility
        {
            get { return _PanelConfirmPwdVisibility; }
            set { _PanelConfirmPwdVisibility = value; NotifyStaticPropertyChanged("PanelConfirmPwdVisibility"); }
        }


        private static string _TraderId;

        public static string TraderId
        {
            get { return _TraderId; }
            set { _TraderId = value; NotifyStaticPropertyChanged("TraderId"); }
        }

        private static string _ConfirmPassword;

        public static string ConfirmPassword
        {
            get { return _ConfirmPassword; }
            set { _ConfirmPassword = value; NotifyStaticPropertyChanged("ConfirmPassword"); }
        }

        private static string _btnChangeDefaultPasswordEnabled = Boolean.TrueString;

        public static string btnChangeDefaultPasswordEnabled
        {
            get { return _btnChangeDefaultPasswordEnabled; }
            set { _btnChangeDefaultPasswordEnabled = value; NotifyStaticPropertyChanged("btnChangeDefaultPasswordEnabled"); }
        }
        #endregion

        #region RelayCommond



        private RelayCommand _btnChangeDefaultPasswordClick;

        public RelayCommand btnChangeDefaultPasswordClick
        {
            get { return _btnChangeDefaultPasswordClick ?? (_btnChangeDefaultPasswordClick = new RelayCommand((object e) => ChangeDefaultPasswordClick(e))); }

        }


        private RelayCommand _btnApplyClick;

        public RelayCommand btnApplyClick
        {
            get { return _btnApplyClick ?? (_btnApplyClick = new RelayCommand((object e) => ApplyDefaultPassword(e))); }

        }

        private RelayCommand _btnCancelClick;

        public RelayCommand btnCancelClick
        {
            get { return _btnCancelClick ?? (_btnCancelClick = new RelayCommand((object e) => WindowCloseClick(e))); }

        }

        private RelayCommand _btnclose;

        public RelayCommand btnclose
        {
            get { return _btnclose ?? (_btnclose = new RelayCommand((object e) => WindowCloseEvent(e))); }

        }

        private RelayCommand _ShortCut_Escape;

        public RelayCommand ShortCut_Escape
        {
            get
            {
                return _ShortCut_Escape ?? (_ShortCut_Escape = new RelayCommand((object e) => WindowCloseClick(e)));
            }
        }


        private RelayCommand _TraderPasswordResetWindowLoad;

        public RelayCommand TraderPasswordResetWindowLoad
        {
            get
            {
                return _TraderPasswordResetWindowLoad ?? (_TraderPasswordResetWindowLoad = new RelayCommand((object e) => Window_load(e)));
            }

        }


        private RelayCommand _TraderPasswordResetClosing;

        public RelayCommand TraderPasswordResetClosing
        {

            get { return _TraderPasswordResetClosing ?? (_TraderPasswordResetClosing = new RelayCommand((object e) => _TraderPasswordReset_Closing(e))); }

        }

        private RelayCommand _txtTraderId_TextChanged;
        public RelayCommand txtTraderId_TextChanged
        {
            get { return _txtTraderId_TextChanged ?? (_txtTraderId_TextChanged = new RelayCommand((object e) => txtTraderIdValidation(e))); }
        }
        #endregion

        #region Constructor
        public TraderPasswordResetVM()
        {
            if (CommonFrontEnd.SharedMemories.ConfigurationMasterMemory.ConfigurationDict.ContainsKey("WindowsPosition"))
            {
                CommonFrontEnd.Model.BoltAppSettingsWindowsPosition oBoltAppSettingsWindowsPosition = new Model.BoltAppSettingsWindowsPosition();
                oBoltAppSettingsWindowsPosition = (CommonFrontEnd.Model.BoltAppSettingsWindowsPosition)CommonFrontEnd.SharedMemories.ConfigurationMasterMemory.ConfigurationDict["WindowsPosition"];
                if (oBoltAppSettingsWindowsPosition != null && oBoltAppSettingsWindowsPosition.TRADERPASSWORDRESET != null && oBoltAppSettingsWindowsPosition.TRADERPASSWORDRESET.WNDPOSITION != null)
                {
                    Height = oBoltAppSettingsWindowsPosition.TRADERPASSWORDRESET.WNDPOSITION.Down.ToString();
                    TopPosition = oBoltAppSettingsWindowsPosition.TRADERPASSWORDRESET.WNDPOSITION.Top.ToString();
                    LeftPosition = oBoltAppSettingsWindowsPosition.TRADERPASSWORDRESET.WNDPOSITION.Left.ToString();
                    Width = oBoltAppSettingsWindowsPosition.TRADERPASSWORDRESET.WNDPOSITION.Right.ToString();
                }
            }

            DefaultHideControls();
            //ReadResetPwdINI();
            // btnChangeDefaultPasswordEnabled = Boolean.TrueString;
            //oLoginRequestProcessor = new LoginRequestProcessor();
        }

        #endregion

        #region Method

        public void DefaultHideControls()
        {
            ReadResetPwdINI();
            PanelDefaultPwdVisibility = "Visible";
            PanelNewPwdVisibility = "Collapsed";
            PanelConfirmPwdVisibility = "Collapsed";
            if (PanelNewPwdVisibility == "Collapsed" && PanelConfirmPwdVisibility == "Collapsed" && PanelDefaultPwdVisibility == "Visible")
            {
                btnChangeDefaultPasswordEnabled = Boolean.TrueString.ToString();
            }
        }

        private void ReadResetPwdINI()
        {
            DefaultPwd = parser.GetSetting("Password Settings", "DEFAULTPWD");
            //defaultPwd = DefaultPwd;
        }

        public void WindowCloseClick(object e)
        {
            View.Login.TraderPasswordReset oTraderPasswordReset = System.Windows.Application.Current.Windows.OfType<View.Login.TraderPasswordReset>().FirstOrDefault();
            if (oTraderPasswordReset != null) oTraderPasswordReset.Close();
            //if (string.IsNullOrEmpty(TraderId))
            //    TraderId = string.Empty;
            //else
            //    TraderId = string.Empty;

            //if (string.IsNullOrEmpty(txtReply))
            //{
            //    txtReply = string.Empty;
            //}
           // else { txtReply = string.Empty; }
            //oNewPasswordBox.Password = string.Empty;
            //oConfirmPwd.Password = string.Empty;
            //txtReply = string.Empty;
            //TraderId = string.Empty;
        }

        private RelayCommand _myLocationChanged;

        public RelayCommand myLocationChanged
        {
            get
            {
                return _myLocationChanged ?? (_myLocationChanged = new RelayCommand(
                    (object e) => TraderPwdReset_LocationChanged(e)));

            }
        }

        private void TraderPwdReset_LocationChanged(object e)
        {
            if (CommonFrontEnd.SharedMemories.ConfigurationMasterMemory.ConfigurationDict.ContainsKey("WindowsPosition"))
            {
                BoltAppSettingsWindowsPosition oBoltAppSettingsWindowsPosition = (BoltAppSettingsWindowsPosition)CommonFrontEnd.SharedMemories.ConfigurationMasterMemory.ConfigurationDict["WindowsPosition"];
                if (oBoltAppSettingsWindowsPosition != null && oBoltAppSettingsWindowsPosition.TRADERPASSWORDRESET != null && oBoltAppSettingsWindowsPosition.TRADERPASSWORDRESET.WNDPOSITION != null)
                {
                    oBoltAppSettingsWindowsPosition.TRADERPASSWORDRESET.WNDPOSITION.Left = Convert.ToInt32(LeftPosition);
                    oBoltAppSettingsWindowsPosition.TRADERPASSWORDRESET.WNDPOSITION.Top = Convert.ToInt32(TopPosition);
                    oBoltAppSettingsWindowsPosition.TRADERPASSWORDRESET.WNDPOSITION.Right = Convert.ToInt32(Width);
                    oBoltAppSettingsWindowsPosition.TRADERPASSWORDRESET.WNDPOSITION.Down = Convert.ToInt32(Height);
                }
                //CommonFrontEnd.SharedMemories.SaveConfiguration.SaveUserConfiguration(@"D:\TWS_DotNetNewStructure\TWS_DOTNETT\CommonFrontEnd\bin\Debug\xml\Users\AppSettings\920200000.xml", "WindowsPosition");
                CommonFrontEnd.SharedMemories.SaveConfiguration.SaveUserConfiguration(CommonFrontEnd.SharedMemories.SettingsManager.AppSettingsXmlPath, "WindowsPosition");
            }
        }

        private void WindowCloseEvent(object e)
        {
            TraderPwdReset_LocationChanged(e);

            if (string.IsNullOrEmpty(TraderId))
                TraderId = string.Empty;
            else
                TraderId = string.Empty;

            if (string.IsNullOrEmpty(txtReply))
            {
                txtReply = string.Empty;
            }
            else { txtReply = string.Empty; }

        }


        public void Window_load(object e)
        {
            System.Windows.Controls.PasswordBox oPasswordBox = (System.Windows.Controls.PasswordBox)e;
            oPasswordBox.Password = DefaultPwd;
        }

        private void _TraderPasswordReset_Closing(object e)
        {
            TraderPwdReset_LocationChanged(e);
            if (string.IsNullOrEmpty(txtReply))
            {
                txtReply = string.Empty;
            }
            else { txtReply = string.Empty; }
        }

        private void txtTraderIdValidation(object e)
        {
            TraderId = Regex.Replace(TraderId, "[^0-9]+", "");
        }

        public void ChangeDefaultPasswordClick(object e)
        {
            ClearData(e);
            DefaultHideControlsChangePwd();

            btnChangeDefaultPasswordEnabled = Boolean.FalseString.ToString();
        }
        public void ApplyDefaultPassword(object e)
        {
            ChangePasswordRequest oChangePasswordRequest = new ChangePasswordRequest();

            var param = (Tuple<object, object, object>)e;
            bool validation;
            System.Windows.Controls.PasswordBox oDefaultPasswordBox = (System.Windows.Controls.PasswordBox)param.Item1;
            System.Windows.Controls.PasswordBox oNewPasswordBox = (System.Windows.Controls.PasswordBox)param.Item2;
            System.Windows.Controls.PasswordBox oConfirmPwd = (System.Windows.Controls.PasswordBox)param.Item3;
            if (PanelDefaultPwdVisibility == "Visible")
            {
                ReadResetPwdINI();
                validation = ValidatePasswords(TraderId, DefaultPwd);

            }
            else
            {
                validation = ValidatePasswords(TraderId, oNewPasswordBox, oConfirmPwd);
            }
            //Use New password if Change default password is clicked
            if (!validation)
                return;
            if (btnChangeDefaultPasswordEnabled == Boolean.FalseString)
            {
                CreateChangePasswordRequest(oNewPasswordBox, ref oChangePasswordRequest);
                DefaultPwd = oNewPasswordBox.Password.Trim();
            }
            else
            {
                oDefaultPasswordBox.Password = DefaultPwd.Trim();
                CreateChangePasswordRequest(oDefaultPasswordBox, ref oChangePasswordRequest);
            }

            oLoginRequestProcessor = new LoginRequestProcessor(new Processor.TraderPasswordReset());
            oLoginRequestProcessor.ProcessRequest(oChangePasswordRequest);

            btnChangeDefaultPasswordEnabled = Boolean.TrueString;
            SaveResetPwdINI();
            DefaultHideControls();
            oNewPasswordBox.Password = string.Empty;
            oConfirmPwd.Password = string.Empty;
            TraderId = string.Empty;

        }
        private bool ValidatePasswords(string TraderId, string defaultPwd)
        {
            if (string.IsNullOrEmpty(TraderId))
            {
                txtReply = "Trader Id is Mandatory";
                return false;
            }
            if (string.IsNullOrEmpty(defaultPwd))
            {
                txtReply = "Default Password is Mandatory";
                return false;
            }
            if (Convert.ToUInt32(TraderId) == UtilityLoginDetails.GETInstance.TraderId)
            {
                txtReply = "Cannot change Password for " + TraderId + " Trader Id";
                return false;
            }


            if (!TraderId.All(c => "0123456789".Contains(c)))
            {
                txtReply = "Please Enter Numeric Trader Id";
                return false;
            }

            if (UtilityLoginDetails.GETInstance.TraderId == 200 && TraderId == "0")
            {
                txtReply = "Cannot change Password for " + TraderId + " Trader Id";
                return false;
            }

            if (defaultPwd.All(c => "0123456789".Contains(c)))
            {
                txtReply = "Please Enter AlphaNumeric Password";
                return false;
            }
            if (Regex.IsMatch(defaultPwd, @"^[a-zA-Z]+$"))
            {
                txtReply = "Please Enter AlphaNumeric Password";
                return false;
            }
            //if (Regex.IsMatch(defaultPwd, @"^[ ]+$"))
            //{
            //    txtReply = "Password Contains space";
            //    return false;
            //}
            if (defaultPwd.Length < 8)
            {
                txtReply = "Password should be more than 8 digits";
                return false;
            }

            txtReply = "";
            return true;
        }

        private bool ValidatePasswords(string TraderId, PasswordBox oNewPasswordBox, PasswordBox oConfirmPwd)
        {

            if (string.IsNullOrEmpty(TraderId))
            {
                txtReply = "Trader Id is Mandatory";
                return false;
            }
            if (string.IsNullOrEmpty(oNewPasswordBox.Password.ToString()))
            {
                txtReply = "New Password is Mandatory";
                return false;
            }
            if (string.IsNullOrEmpty(oConfirmPwd.Password.ToString()))
            {
                txtReply = "Confirm Password is Mandatory";
                return false;
            }

            if (Convert.ToUInt32(TraderId) == UtilityLoginDetails.GETInstance.TraderId)
            {
                txtReply = "Cannot change Password for " + TraderId + " Trader Id";
                return false;
            }
            if (UtilityLoginDetails.GETInstance.TraderId == 200 && TraderId == "0")
            {
                txtReply = "Cannot change Password for " + TraderId + " Trader Id";
                return false;
            }

            if (!TraderId.All(c => "0123456789".Contains(c)))
            {
                txtReply = "Please Enter Numeric Trader Id";
                return false;
            }
            if (oNewPasswordBox.Password.ToString().All(c => "0123456789".Contains(c)))
            {
                txtReply = "New Password should be AlphaNumeric";
                return false;
            }
            if (oConfirmPwd.Password.ToString().All(c => "0123456789".Contains(c)))
            {
                txtReply = "Confirm Password should be AlphaNumeric";
                return false;
            }
            if (Regex.IsMatch(oNewPasswordBox.Password.ToString(), @"^[a-zA-Z]+$"))
            {
                txtReply = "New Password should be AlphaNumeric";
                return false;
            }
            if (Regex.IsMatch(oConfirmPwd.Password, @"^[a-zA-Z]+$"))
            {
                txtReply = "Confirm Password should be AlphaNumeric";
                return false;
            }

            //if (oNewPasswordBox.Password.ToString() != Regex.Replace(oNewPasswordBox.Password.ToString(), "^[ ]+", ""))
            //{
            //    txtReply = "New Password should not contain space";

            //    return false;
            //}
            //if (oConfirmPwd.Password.ToString() != Regex.Replace(oConfirmPwd.Password.ToString(), "^[ ]+", ""))
            //{
            //    txtReply = "Confirm Password should not contain space";
            //    return false;
            //}
            if (oNewPasswordBox != null && !string.IsNullOrEmpty(oNewPasswordBox.Password))
            {
                if (oNewPasswordBox.Password.Any(x => Char.IsWhiteSpace(x)))
                {
                    txtReply = "New Password should not contain space";
                    return false;
                }
            }
            if (oConfirmPwd != null && !string.IsNullOrEmpty(oConfirmPwd.Password))
            {
                if (oConfirmPwd.Password.Any(x => Char.IsWhiteSpace(x)))
                {
                    txtReply = "Confirm Password should not contain space";
                    return false;
                }
            }
            if (oNewPasswordBox.ToString().Length < 8)
            {
                txtReply = "New Password should be more than 8 digits";
                return false;
            }
            if (oConfirmPwd.Password.ToString().Length < 8)
            {
                txtReply = "Confirm Password should be more than 8 digits";
                return false;
            }
            if (oNewPasswordBox.Password.ToString() != oConfirmPwd.Password.ToString())
            {
                txtReply = "Password Mismatch";
                return false;
            }

            txtReply = "";
            return true;
        }


        public void DefaultHideControlsChangePwd()
        {
            PanelDefaultPwdVisibility = "Collapsed";
            PanelNewPwdVisibility = "Visible";
            PanelConfirmPwdVisibility = "Visible";
            if (PanelNewPwdVisibility == "Visible" && PanelConfirmPwdVisibility == "Visible" && PanelDefaultPwdVisibility == "Collapsed")
            {
                btnChangeDefaultPasswordEnabled = Boolean.FalseString.ToString();
            }
        }
        public void ClearData(object e)
        {
            var param = (Tuple<object, object, object>)e;
            oDefaultPasswordBox = (System.Windows.Controls.PasswordBox)param.Item1;
            oNewPasswordBox = (System.Windows.Controls.PasswordBox)param.Item2;
            oConfirmPwd = (System.Windows.Controls.PasswordBox)param.Item3;
            //TraderId = string.Empty;
            ConfirmPassword = string.Empty;

            if (oDefaultPasswordBox != null)
            {
                oDefaultPasswordBox.Password = string.Empty;
            }
            if (oNewPasswordBox != null)
            {
                oNewPasswordBox.Password = string.Empty;
            }
            if (oConfirmPwd != null)
            {
                oConfirmPwd.Password = string.Empty;
            }
            txtReply = string.Empty;
        }
        private void SaveResetPwdINI()
        {
            parser.AddSetting("Password Settings", "DEFAULTPWD", DefaultPwd);
            parser.SaveSettings(ResetPwdINIPath.ToString());
        }
        public void CreateChangePasswordRequest(System.Windows.Controls.PasswordBox oPasswordBox, ref ChangePasswordRequest objChangePasswordRequest)
        {

            if (objChangePasswordRequest != null)
            {
                objChangePasswordRequest.TraderID = Convert.ToUInt32(TraderId.Trim());
                objChangePasswordRequest.NewPassword = oPasswordBox.Password;
                objChangePasswordRequest.Exchange = 1;//1- BSE, 2-BOW, 3-NSE
                objChangePasswordRequest.Filler_c = "";
                objChangePasswordRequest.Market = 1;//1 - Equity, 2- Derv., 3. Curr
                objChangePasswordRequest.MemberID = UtilityLoginDetails.GETInstance.MemberId;
                //objChangePasswordRequest.MessageTag = MemoryManager.GetMesageTag();
                objChangePasswordRequest.Password = oPasswordBox.Password;
                UtilityLoginDetails.GETInstance.RequestTraderId = Convert.ToUInt16(objChangePasswordRequest.TraderID);
            }
        }
        #endregion

        #region StaticNotifyPropertyChangedEvent
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

        public static event EventHandler<PropertyChangedEventArgs> StaticPropertyChanged
                 = delegate { };
        private static void NotifyStaticPropertyChanged(string propertyName)
        {
            StaticPropertyChanged(null, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
#elif BOW
    public class TraderPasswordResetVM
    { }
#endif

}
