using CommonFrontEnd.Common;
using CommonFrontEnd.Constants;
using CommonFrontEnd.Global;
using CommonFrontEnd.Model;
using CommonFrontEnd.Processor;
using CommonFrontEnd.SharedMemories;
using CommonFrontEnd.View;
using CommonFrontEnd.View.Login;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CommonFrontEnd.ViewModel
{

    partial class PasswordResetVM
    {
        int mintNoofDays;

        private string _LeftPosition = "345";

        public string LeftPosition
        {
            get { return _LeftPosition; }
            set { _LeftPosition = value; NotifyPropertyChanged("LeftPosition"); }
        }


        private string _TopPosition = "200";

        public string TopPosition
        {
            get { return _TopPosition; }
            set { _TopPosition = value; NotifyPropertyChanged("TopPosition"); }
        }

        private string _Width = "449";

        public string Width
        {
            get { return _Width; }
            set { _Width = value; NotifyPropertyChanged("Width"); }
        }


        private string _Height = "358.914";

        public string Height
        {
            get { return _Height; }
            set { _Height = value; NotifyPropertyChanged("Height"); }
        }
        System.Windows.Controls.PasswordBox oOldPasswordBox;
        System.Windows.Controls.PasswordBox oNewPasswordBox;
        System.Windows.Controls.PasswordBox oConfirmPwdBox;
        public static string NewPasswordChangePwd;

        private static string _ReplyTextBox;

        public static string ReplyTextBox
        {
            get { return _ReplyTextBox; }
            set { _ReplyTextBox = value; NotifyStaticPropertyChanged("ReplyTextBox"); }
        }

        private RelayCommand _ChgPwdWindowClosing;

        public RelayCommand ChgPwdWindowClosing
        {

            get { return _ChgPwdWindowClosing ?? (_ChgPwdWindowClosing = new RelayCommand((object e) => ChgPwdWindow_Closing(e))); }

        }


        public PasswordResetVM()
        {
            if (ConfigurationMasterMemory.ConfigurationDict.ContainsKey("WindowsPosition"))
            {
                BoltAppSettingsWindowsPosition oBoltAppSettingsWindowsPosition = new BoltAppSettingsWindowsPosition();
                oBoltAppSettingsWindowsPosition = (BoltAppSettingsWindowsPosition)ConfigurationMasterMemory.ConfigurationDict["WindowsPosition"];
                if (oBoltAppSettingsWindowsPosition != null && oBoltAppSettingsWindowsPosition.CHGPASSWORD != null && oBoltAppSettingsWindowsPosition.CHGPASSWORD.WNDPOSITION != null)
                {
                    Height = oBoltAppSettingsWindowsPosition.CHGPASSWORD.WNDPOSITION.Down.ToString();
                    TopPosition = oBoltAppSettingsWindowsPosition.CHGPASSWORD.WNDPOSITION.Top.ToString();
                    LeftPosition = oBoltAppSettingsWindowsPosition.CHGPASSWORD.WNDPOSITION.Left.ToString();
                    Width = oBoltAppSettingsWindowsPosition.CHGPASSWORD.WNDPOSITION.Right.ToString();
                }
            }
            System.Configuration.AppSettingsReader lobjreader = new System.Configuration.AppSettingsReader();
            try
            {
                mintNoofDays = (int)lobjreader.GetValue("PasswordExpiryDays", typeof(int));
                if (mintNoofDays == 0)
                {
                    mintNoofDays = 14;
                }
            }
            catch (Exception ex)
            {
                mintNoofDays = 14;
            }
        }

        private void Windows_CHPasswordLocationChanged(object e)
        {
            if (ConfigurationMasterMemory.ConfigurationDict.ContainsKey("WindowsPosition"))
            {
                BoltAppSettingsWindowsPosition oBoltAppSettingsWindowsPosition = (BoltAppSettingsWindowsPosition)ConfigurationMasterMemory.ConfigurationDict["WindowsPosition"];
                if (oBoltAppSettingsWindowsPosition != null && oBoltAppSettingsWindowsPosition.CHGPASSWORD != null && oBoltAppSettingsWindowsPosition.CHGPASSWORD.WNDPOSITION != null)
                {
                    oBoltAppSettingsWindowsPosition.CHGPASSWORD.WNDPOSITION.Left = Convert.ToInt32(LeftPosition);
                    oBoltAppSettingsWindowsPosition.CHGPASSWORD.WNDPOSITION.Top = Convert.ToInt32(TopPosition);
                    oBoltAppSettingsWindowsPosition.CHGPASSWORD.WNDPOSITION.Right = Convert.ToInt32(Width);
                    oBoltAppSettingsWindowsPosition.CHGPASSWORD.WNDPOSITION.Down = Convert.ToInt32(Height);
                }
                //CommonFrontEnd.SharedMemories.SaveConfiguration.SaveUserConfiguration(@"D:\TWS_DotNetNewStructure\TWS_DOTNETT\CommonFrontEnd\bin\Debug\xml\Users\AppSettings\920200000.xml", "WindowsPosition");
                //SaveConfiguration.SaveUserConfiguration(SettingsManager.AppSettingsXmlPath, "WindowsPosition");
            }
        

        }

        private void ChgPwdWindow_Closing(object e)
        {
            PasswordReset oChangePassword = System.Windows.Application.Current.Windows.OfType<PasswordReset>().FirstOrDefault();

            if (oChangePassword != null)
            {
                if (oChangePassword.pwdBoxConfirmPassword != null)
                {
                    oChangePassword.pwdBoxConfirmPassword.Password = string.Empty;
                }
                if (oChangePassword.pwdBoxNewPassword != null)
                {
                    oChangePassword.pwdBoxNewPassword.Password = string.Empty;
                }
                if (oChangePassword.pwdBoxOldPassword != null)
                {
                    oChangePassword.pwdBoxOldPassword.Password = string.Empty;
                }
                oChangePassword.Hide();
            }

            //Windows_CHPasswordLocationChanged(e);

            //if (oOldPasswordBox != null)
            //{ if (!string.IsNullOrEmpty(oOldPasswordBox.Password))
            //    { oOldPasswordBox.Password = string.Empty; }
            //}
            //if(oNewPasswordBox != null)
            //{
            //    if (!string.IsNullOrEmpty(oNewPasswordBox.Password))
            //    { oNewPasswordBox.Password = string.Empty; }
            //}
            ////oNewPasswordBox.Password = string.Empty;
            //if (oConfirmPwdBox != null)
            //{
            //    if (!string.IsNullOrEmpty(oConfirmPwdBox.Password) )
            //    {
            //        oConfirmPwdBox.Password = string.Empty;
            //    }
            //}
            ReplyTextBox = string.Empty;
        }


        #region StaticNotifyPropertyChangedEvent
        public static event EventHandler<PropertyChangedEventArgs> StaticPropertyChanged
                 = delegate { };
        private static void NotifyStaticPropertyChanged(string propertyName)
        {
            StaticPropertyChanged(null, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

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

#if TWS
     partial class PasswordResetVM
    {
            private string _ReenterNewPassword = string.Empty;

            public string ReenterNewPassword
            {
                get { return _ReenterNewPassword; }
                set { _ReenterNewPassword = value; NotifyPropertyChanged("ReenterNewPassword"); }
            }

            private string _TxtReply;

            public string TxtReply
            {
                get { return _TxtReply; }
                set { _TxtReply = value; NotifyPropertyChanged("TxtReply"); }
            }


            private RelayCommand _myLocationChanged;

            public RelayCommand myLocationChanged
            {
                get
                {
                    return _myLocationChanged ?? (_myLocationChanged = new RelayCommand(
                        (object e) => Windows_CHPasswordLocationChanged(e)));

                }
            }

            private RelayCommand _btnChangePassword;

            public RelayCommand btnChangePassword
            {
                get
                {
                    return _btnChangePassword ?? (_btnChangePassword = new RelayCommand(
                        (object e) => ChangePassword_Click(e)));

                }

            }
            
            private RelayCommand _click_Close;

            public RelayCommand click_Close
            {
                get
                {
                    return _click_Close ?? (_click_Close = new RelayCommand(
                        (object e) => ChangePasswordClose_Click(e)));

                }

            }

           
        
            private void ChangePasswordClose_Click(object e)
            {
                Windows_CHPasswordLocationChanged(e);

            }


            //private void NewPasswordGet(object e)
            //{
            //    PasswordBox pwdbo = null;
            //   // NewPassword = e.GetType().Name;
            //    string controlName = e.GetType().Name;
            //        if (controlName == "PasswordBox")
            //        {
            //            pwdbo = e as PasswordBox;
            //            NewPassword = pwdbo.Password;
            //         }
            //    }
            private void ChangePassword_Click(object e)
            {
                ChangePasswordRequest oChangePasswordRequest = new ChangePasswordRequest();
               LoginProcessor oLoginProcessor = new LoginProcessor();

                var param = (Tuple<object, object, object>)e;

                oOldPasswordBox = (System.Windows.Controls.PasswordBox)param.Item1;
                oNewPasswordBox = (System.Windows.Controls.PasswordBox)param.Item2;
                NewPasswordChangePwd = oNewPasswordBox.Password;
                oConfirmPwdBox = (System.Windows.Controls.PasswordBox)param.Item3;
                var OldPassword = oOldPasswordBox.Password;
                var NewPassword = oNewPasswordBox.Password;
                var confirmPwd = oConfirmPwdBox.Password;

                bool validation = ValidatePasswords(OldPassword, NewPassword, confirmPwd);
                if (!validation)
                    return;
                if (NewPassword == confirmPwd)
                {
               
                if (UtilityLoginDetails.GETInstance.IsEQXloggedIn)
                    {
                        //oLogonRequest.Exchange = 1; //1 - Equity, 2- Derv., 3. Curr
                        CreateEQXChangePwdRequest(ref oChangePasswordRequest, OldPassword, NewPassword);
                        oLoginProcessor.ProcessData(oChangePasswordRequest);
                    }
                    if (UtilityLoginDetails.GETInstance.IsDERloggedIn)//(DerivativeChkBx)
                    {
                    //TODO for Derivative
                        CreateEQXChangePwdRequest(ref oChangePasswordRequest, OldPassword, NewPassword);
                        oLoginProcessor.ProcessData(oChangePasswordRequest);
                    }
                    if (UtilityLoginDetails.GETInstance.IsCURloggedIn)//(CurrencyChkBx)
                    {
                    //TODO for Currency
                        CreateEQXChangePwdRequest(ref oChangePasswordRequest, OldPassword, NewPassword);
                        oLoginProcessor.ProcessData(oChangePasswordRequest);
                    }
                    //oLoginProcessor.ProcessData(oChangePasswordRequest);

                }
                else
                {
                    ReplyTextBox = "Password Mismatch";
                } //MessageBox.Show("Confirm Password does not match."); }
                oOldPasswordBox.Password = string.Empty;
                oNewPasswordBox.Password = string.Empty;
                oConfirmPwdBox.Password = string.Empty;
                ReplyTextBox = string.Empty;
            }

            private bool ValidatePasswords(string oldPassword, string newPassword, string confirmPwd)
            {

                if (string.IsNullOrEmpty(oldPassword.ToString()))
                {
                    ReplyTextBox = "Old Password Field cannot be blank";
                    return false;
                }
                if (string.IsNullOrEmpty(newPassword.ToString()))
                {
                    ReplyTextBox = "New Password Field cannot be blank";
                    return false;
                }
                if (string.IsNullOrEmpty(confirmPwd.ToString()))
                {
                    ReplyTextBox = "Confirm Password Field cannot be blank";
                    return false;
                }

                if (oldPassword.All(c => "0123456789".Contains(c)))
                {
                    ReplyTextBox = "Old Password should be AlphaNumeric";
                    return false;
                }
                if (newPassword.All(c => "0123456789".Contains(c)))
                {
                    ReplyTextBox = "New Password should be AlphaNumeric";
                    return false;
                }
                if (confirmPwd.All(c => "0123456789".Contains(c)))
                {
                    ReplyTextBox = "Confirm Password should be AlphaNumeric";
                    return false;
                }
                if (Regex.IsMatch(oldPassword, @"^[a-zA-Z]+$"))
                {
                    ReplyTextBox = "Old Password should be AlphaNumeric";
                    return false;
                }
                if (Regex.IsMatch(newPassword, @"^[a-zA-Z]+$"))
                {
                    ReplyTextBox = "New Password should be AlphaNumeric";
                    return false;
                }
                if (Regex.IsMatch(confirmPwd, @"^[a-zA-Z]+$"))
                {
                    ReplyTextBox = "Confirm Password should be AlphaNumeric";
                    return false;
                }
                if (newPassword.ToString().Length < 8)
                {
                    ReplyTextBox = "New Password should be more than 8 digits";
                    return false;
                }
                if (confirmPwd.ToString().Length < 8)
                {
                    ReplyTextBox = "Confirm Password should be more than 8 digits";
                    return false;
                }
                if (UtilityLoginDetails.GETInstance.DecryptedPassword != oldPassword)
                {
                    ReplyTextBox = "Old Password doesn't match";
                    return false;
                }
                if ((UtilityLoginDetails.GETInstance.DecryptedPassword == newPassword) && (UtilityLoginDetails.GETInstance.DecryptedPassword == confirmPwd))
                {
                    ReplyTextBox = "Password is in history of old password";
                    return false;
                }
                if (newPassword != confirmPwd)
                {
                    ReplyTextBox = "Password Mismatch";
                    return false;
                }
                ReplyTextBox = "";
                return true;
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
            public void CreateEQXChangePwdRequest(ref ChangePasswordRequest oChangePasswordRequest, string OldPassword, string NewPassword)
            {
                oChangePasswordRequest.MemberID = Convert.ToUInt16(UtilityLoginDetails.GETInstance.MemberId);
                oChangePasswordRequest.TraderID = Convert.ToUInt16(UtilityLoginDetails.GETInstance.TraderId);//204;
                oChangePasswordRequest.Password = OldPassword;
                //oChangePasswordRequest.MessageTag = MemoryManager.GetMesageTag();
                oChangePasswordRequest.Market = 1;//1 - Equity, 2- Derv., 3. Curr
                oChangePasswordRequest.Exchange = 1;//1- BSE, 2-BOW, 3-NSE
                oChangePasswordRequest.NewPassword = NewPassword;
                oChangePasswordRequest.Filler_c = "";
               UtilityLoginDetails.GETInstance.RequestTraderId = UtilityLoginDetails.GETInstance.TraderId;
          
        }



        
    }
#elif BOW
    partial class  PasswordResetVM
    {
        string[] lstrParaName = new string[6];
        string[] lstrParaValue = new string[6];
     

        private RelayCommand _btnChangePassword;

        public RelayCommand btnChangePassword
        {
            get
            {
                return _btnChangePassword ?? (_btnChangePassword = new RelayCommand(
                    (object e) => ChangePassword_Click(e)));

            }

        }

       

        private void ChangePassword_Click(object e)
        {
            ChangePasswordRequest oChangePasswordRequest = new ChangePasswordRequest();
            string lstrMsg;
            string[] laryMsg;

            var param = (Tuple<object, object, object>)e;

            oOldPasswordBox = (System.Windows.Controls.PasswordBox)param.Item1;
            oNewPasswordBox = (System.Windows.Controls.PasswordBox)param.Item2;
 
            oConfirmPwdBox = (System.Windows.Controls.PasswordBox)param.Item3;
            var OldPassword = oOldPasswordBox.Password;
            var NewPassword = oNewPasswordBox.Password;
            var confirmPwd = oConfirmPwdBox.Password;

            bool validation = ValidatePasswords(OldPassword, NewPassword, confirmPwd);
            try
            {
                if (!validation)
                    return;
                if (NewPassword == confirmPwd)
                {
                    lstrParaName[0] = ChangePasswordConstants.OLD_PASSWORD;
                    lstrParaName[1] = ChangePasswordConstants.NEW_PASSWORD;
                    lstrParaName[2] = ChangePasswordConstants.CONFIRM_PASSWORD;

                    lstrParaName[3] = ChangePasswordConstants.USERFLAG;

                    lstrParaValue[0] = OldPassword;
                    lstrParaValue[1] = NewPassword;
                    lstrParaValue[2] = confirmPwd;
                    lstrParaValue[3] = "1";


                    lstrMsg = SettingsManager.GetDataFromServer(ChangePasswordConstants.SERVLET_NAME_PASSWORD, lstrParaName, lstrParaValue);
                    laryMsg = lstrMsg.Split('|');

                    if (laryMsg[0] != BowConstants.SUCCESS_FLAG)
                    {

                        ReplyTextBox = "Password : " + laryMsg[1].ToString();
                      
                    }
                    else
                    {

                        //: Storing the Password for Application Lock
                        UtilityLoginDetails.GETInstance.DecryptedPassword = NewPassword;
                        ReplyTextBox = "Sucessfully changed the password.This password will Expire in " + mintNoofDays.ToString() + " days.";
                        
                    }

                }
            }
            catch(Exception ex)
            {
                ReplyTextBox = ex.Message;
            }
        }

      private bool  ValidatePasswords(string OldPassword, string NewPassword, string confirmPwd)
        {
            if (string.IsNullOrEmpty(OldPassword.ToString()))
            {
                ReplyTextBox = "Old Password Field cannot be blank";
                return false;
            }
            if (string.IsNullOrEmpty(NewPassword.ToString()))
            {
                ReplyTextBox = "New Password Field cannot be blank and should be > 6 characters";
                return false;
            }
            if (string.IsNullOrEmpty(confirmPwd.ToString()))
            {
                ReplyTextBox = "Confirm Password Field cannot be blank";
                return false;
            }
            if (NewPassword != confirmPwd)
            {
                ReplyTextBox = "New Password And Confirm Password Are Not Equal";
                return false;
            }
       
            return true;
        }

    }
#endif
}
