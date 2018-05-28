using CommonFrontEnd.Common;
using CommonFrontEnd.Model.Admin;
using CommonFrontEnd.View.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CommonFrontEnd.ViewModel.Profiling
{
    public class EmailProfilingVM : BaseViewModel
    {

        #region Properties

        private string _FromEmailID;

        public string FromEmailID
        {
            get { return _FromEmailID; }
            set { _FromEmailID = value; }
        }

        //private string _DirectChecked;

        //public string DirectChecked
        //{
        //    get { return _DirectChecked; }
        //    set { _DirectChecked = value; NotifyStaticPropertyChanged(nameof(DirectChecked)); }
        //}


        private string _ToEmailID;

        public string ToEmailID
        {
            get { return _ToEmailID; }
            set { _ToEmailID = value; }
        }

        private static string _GatewayIP;

        public static string GatewayIP
        {
            get { return _GatewayIP; }
            set { _GatewayIP = value; NotifyStaticPropertyChanged(nameof(GatewayIP)); }
        }

        private static string _InterfaceIP;

        public static string InterfaceIP
        {
            get { return _InterfaceIP; }
            set { _InterfaceIP = value; NotifyStaticPropertyChanged(nameof(InterfaceIP)); }
        }

        private static string _SMTPIP;

        public static string SMTPIP
        {
            get { return _SMTPIP; }
            set { _SMTPIP = value; NotifyStaticPropertyChanged(nameof(SMTPIP)); }
        }




        private string _txtPswd;

        public string txtPswd
        {
            get { return _txtPswd; }
            set { _txtPswd = value; }
        }
        private string PasswordDirect;

        public string _PasswordDirect
        {
            get { return PasswordDirect; }
            set { PasswordDirect = value; }
        }

        private static bool _DirectChecked;

        public static bool DirectChecked
        {
            get { return _DirectChecked; }
            set { _DirectChecked = value; NotifyStaticPropertyChanged(nameof(DirectChecked)); }
        }

        private static bool _SMTPChecked;

        public static bool SMTPChecked
        {
            get { return _SMTPChecked; }
            set { _SMTPChecked = value; NotifyStaticPropertyChanged(nameof(SMTPChecked)); }
        }


        private string  _DirectVisibility;

        public string  DirectVisibility
        {
            get { return _DirectVisibility; }
            set { _DirectVisibility = value; NotifyPropertyChanged(nameof(DirectVisibility)); }
        }

        private string _SMTPVisibility;

        public string  SMTPVisibility
        {
            get { return _SMTPVisibility; }
            set { _SMTPVisibility = value; NotifyPropertyChanged(nameof(SMTPVisibility)); }
        }

        private string _ctrEnable;

        public string ctrEnable
        {
            get { return _ctrEnable; }
            set { _ctrEnable = value; NotifyPropertyChanged(nameof(ctrEnable)); }
        }

        private static string _SMTPPort;

        public static string SMTPPort
        {
            get { return _SMTPPort; }
            set { _SMTPPort = value; NotifyStaticPropertyChanged(nameof(SMTPPort)); }
        }

        private static string _SMTPEmailID;

        public static string SMTPEmailID
        {
            get { return _SMTPEmailID; }
            set { _SMTPEmailID = value; NotifyStaticPropertyChanged(nameof(SMTPEmailID)); }
        }

        private static string _SMTPPassword;

        public static string SMTPPassword
        {
            get { return _SMTPPassword; }
            set { _SMTPPassword = value; NotifyStaticPropertyChanged(nameof(SMTPPassword)); }
        }





        #endregion

        #region RelayCommands

        private RelayCommand _TestEmailClick;

        public RelayCommand TestEmailClick
        {
            get
            {
                return _TestEmailClick ?? (_TestEmailClick = new RelayCommand(
                    (object e) => TestEmailClick_Click(e)));

            }

        }

        private RelayCommand _GuidelinesClick;

        public RelayCommand GuidelinesClick
        {
            get
            {
                return _GuidelinesClick ?? (_GuidelinesClick = new RelayCommand(
                    (object e) => GuidelinesClick_Click(e)));

            }

        }

        private RelayCommand _Save_Click;

        public RelayCommand Save_Click
        {
            get
            {
                return _Save_Click ?? (_Save_Click = new RelayCommand(
                    (object e) => SaveChanges(e)));

            }

        }

        private RelayCommand _SaveAll_Click;

        public RelayCommand SaveAll_Click
        {
            get
            {
                return _SaveAll_Click ?? (_SaveAll_Click = new RelayCommand(
                    (object e) => SaveAllChanges(e)));

            }

        }

        private RelayCommand _AddTWSRouteClick;

        public RelayCommand AddTWSRouteClick
        {
            get
            {
                return _AddTWSRouteClick ?? (_AddTWSRouteClick = new RelayCommand(
                    (object e) => AddTWSRoute(e)));

            }

        }

        private static RelayCommand _EmailProfilingLoaded;

        public static RelayCommand EmailProfilingLoaded
        {
            get
            {
                return _EmailProfilingLoaded ?? (_EmailProfilingLoaded = new RelayCommand((object e) => EmailProfilingLoaded_Load(e)));

            }

        }

        private static void EmailProfilingLoaded_Load(object e)
        {
            ((View.Profiling.EmailProfiling)(Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().EmailProfiling.Content)).SMTPIP.Text = SMTPIP;
            //((View.Profiling.EmailProfiling)(Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().EmailProfiling.Content)).InterfaceIP.Text = InterfaceIP;
            //((View.Profiling.EmailProfiling)(Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().EmailProfiling.Content)).GatewayIP.Text = GatewayIP;
        }

        private RelayCommand _ClearClick;

        public RelayCommand ClearClick
        {
            get
            {
                return _ClearClick ?? (_ClearClick = new RelayCommand(
                    (object e) => ClearClickChanges(e)));

            }

        }

        private RelayCommand _VisibilityCheck;

        public RelayCommand VisibilityCheck
        {
            get
            {
                return _VisibilityCheck ?? (_VisibilityCheck = new RelayCommand(
                    (object e) => VisibilityControl(e)));

            }

        }
      
      
     

        // public static string SMTPIP { get; private set; }

        private static string _DirectOrSMTPFlag;

        public static string DirectOrSMTPFlag
        {
            get { return _DirectOrSMTPFlag; }
            set { _DirectOrSMTPFlag = value; NotifyStaticPropertyChanged(nameof(DirectOrSMTPFlag)); }
        }

        private static string _FromEmail;

        public static string FromEmail
        {
            get { return _FromEmail; }
            set { _FromEmail = value; NotifyStaticPropertyChanged(nameof(FromEmail)); }
        }
        private static string _LiveSMTPIP;

        public static string LiveSMTPIP
        {
            get { return _LiveSMTPIP; }
            set { _LiveSMTPIP = value; NotifyStaticPropertyChanged(nameof(LiveSMTPIP)); }
        }

        private static string _LiveSMTPPort;

        public static string LiveSMTPPort
        {
            get { return _LiveSMTPPort; }
            set { _LiveSMTPPort = value; NotifyStaticPropertyChanged(nameof(LiveSMTPPort)); }
        }
        private static string _LiveSMTPEmailID;

        public static string LiveSMTPEmailID
        {
            get { return _LiveSMTPEmailID; }
            set { _LiveSMTPEmailID = value; NotifyStaticPropertyChanged(nameof(LiveSMTPEmailID)); }
        }

        private static string _LiveSMTPPassword;

        public static string LiveSMTPPassword
        {
            get { return _LiveSMTPPassword; }
            set { _LiveSMTPPassword = value; NotifyStaticPropertyChanged(nameof(LiveSMTPPassword)); }
        }
        private static string _LiveFromEmail;

        public static string LiveFromEmail
        {
            get { return _LiveFromEmail; }
            set { _LiveFromEmail = value; NotifyStaticPropertyChanged(nameof(LiveFromEmail)); }
        }

        private static string _LiveSettings;

        public static string LiveSettings
        {
            get { return _LiveSettings; }
            set { _LiveSettings = value; NotifyStaticPropertyChanged(nameof(LiveSettings)); }
        }
        private static string _GmailEmailID;

        public static string GmailEmailID
        {
            get { return _GmailEmailID; }
            set { _GmailEmailID = value; NotifyStaticPropertyChanged(nameof(GmailEmailID)); }
        }

        private static string _GmailPassword;

        public static string GmailPassword
        {
            get { return _GmailPassword; }
            set { _GmailPassword = value; NotifyStaticPropertyChanged(nameof(GmailPassword)); }
        }
        private static string _ToTestEmailID;

        public static string ToTestEmailID
        {
            get { return _ToTestEmailID; }
            set { _ToTestEmailID = value; NotifyStaticPropertyChanged(nameof(ToTestEmailID)); }
        }

        private static bool _IsPing;

        public static bool IsPing
        {
            get { return _IsPing; }
            set { _IsPing = value; NotifyStaticPropertyChanged(nameof(IsPing)); }
        }
        private static int _PortStatus = 2;

        public static int PortStatus
        {
            get { return _PortStatus; }
            set { _PortStatus = value; NotifyStaticPropertyChanged(nameof(PortStatus)); }
        }

        //  public static string FromEmail { get; private set; }
        //public static string LiveSMTPIP { get; private set; }
        // public static string LiveSMTPPort { get; private set; }
        //public static string LiveSMTPEmailID { get; private set; }
        // public static string LiveSMTPPassword { get; private set; }
        // public static string LiveFromEmail { get; private set; }
        //public static string LiveSettings { get; private set; }
        // public static string GmailEmailID { get; private set; }
        // public static string GmailPassword { get; private set; }
        //  public static string ToTestEmailID { get; private set; }
        // public static bool IsPing { get; private set; }
        //public static int PortStatus = 2;
        public static int TestMailSuccess = 0;

        #endregion

        #region Validations

      

       
        #endregion

        #region Methods

        private void GuidelinesClick_Click(object e)

        {
            MessageBoxResult result = MessageBox.Show("Direct Email:" + Environment.NewLine + "" + Environment.NewLine + "For Direct email to work it is preferred to create new gmail id" + Environment.NewLine + "or else follow the below steps for existing gmail account to work with" + Environment.NewLine + "trading application:" + Environment.NewLine + "" + Environment.NewLine +
                "1)Login to your gmail ID" + Environment.NewLine + " 2) Enter the below url in seperate browser"
                + Environment.NewLine + " https://www.google.com/settings/security/lesssecureapps" + Environment.NewLine +
                "3)Click on Turn On radio button and allow it to update." + Environment.NewLine + "" + Environment.NewLine + "Smtp Email:" + Environment.NewLine + "" + Environment.NewLine +
                "1)If BSE LAN card is default then add route for SMTP server." + Environment.NewLine + "2)If LAN card of SMTP server is default then add route for TWS" + Environment.NewLine + "application (Already a button is provided in Internet/Email Profiling" + Environment.NewLine + "window in Internet and Email section)"
                , "Guidelines");
        }



        public static string ReadEmailDetailFromINI()
        {


            //For Test SMTP : Start
            DirectOrSMTPFlag = MainWindowVM.parser.GetSetting("SMTP EMAIL PROFILING", "DirectOrSMTPFlag");
            // SMTPIP = ((EmailProfilingVM)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().EmailProfiling.Content)).SMTPIP.Text;
            SMTPIP = MainWindowVM.parser.GetSetting("SMTP EMAIL PROFILING", "TestSMTPServerIP");

            SMTPPort = MainWindowVM.parser.GetSetting("SMTP EMAIL PROFILING", "TestSMTPPort");
            SMTPEmailID = MainWindowVM.parser.GetSetting("SMTP EMAIL PROFILING", "TestSMTPEmailID");
            SMTPPassword = MainWindowVM.parser.GetSetting("SMTP EMAIL PROFILING", "TestSMTPPassword");
            if (!string.IsNullOrEmpty(SMTPPassword))//Vijayalakshmi - To implement Decryption of the encrypted password
            {
                string SMTPdecrypted = EncryptDecrypt(SMTPPassword);
                SMTPPassword = SMTPdecrypted;
            }//Vijayalakshmi - To implement Decryption of the encrypted password
            FromEmail = MainWindowVM.parser.GetSetting("SMTP EMAIL PROFILING", "TestFromEmailID");
            //For Test SMTP : End
            //For Live SMTP: Start
            //LiveSMTPIP = MainWindowVM.parser.GetSetting("SMTP EMAIL PROFILING", "LiveSMTPServerIP");
            //LiveSMTPPort = MainWindowVM.parser.GetSetting("SMTP EMAIL PROFILING", "LiveSMTPPort");
            //LiveSMTPEmailID = MainWindowVM.parser.GetSetting("SMTP EMAIL PROFILING", "LiveSMTPEmailID");
            //LiveSMTPPassword = MainWindowVM.parser.GetSetting("SMTP EMAIL PROFILING", "LiveSMTPPassword");
            //if (!string.IsNullOrEmpty(LiveSMTPPassword))//Vijayalakshmi - To implement Decryption of the encrypted password
            //{
            //    string LiveDecrypted = EncryptDecrypt(LiveSMTPPassword);
            //    LiveSMTPPassword = LiveDecrypted;
            //}//Vijayalakshmi - To implement Decryption of the encrypted password
            //LiveFromEmail = MainWindowVM.parser.GetSetting("SMTP EMAIL PROFILING", "LiveFromEmailID");
            //LiveSettings = MainWindowVM.parser.GetSetting("SMTP EMAIL PROFILING", "LiveSettings");
            //MessageBox.Show(LiveSettings, "Information");
            //For Live SMTP: End
            //For Direct Internent : Start
            GmailEmailID = MainWindowVM.parser.GetSetting("SMTP EMAIL PROFILING", "GmailID");
            GmailPassword = MainWindowVM.parser.GetSetting("SMTP EMAIL PROFILING", "Password");
            if (!string.IsNullOrEmpty(GmailPassword))//Vijayalakshmi - To implement Decryption of the encrypted password
            {
                string GmailDecrypted = EncryptDecrypt(GmailPassword);
                GmailPassword = GmailDecrypted;
            }//Vijayalakshmi - To implement Decryption of the encrypted password
            ToTestEmailID = MainWindowVM.parser.GetSetting("SMTP EMAIL PROFILING", "ToTestEmailID");
            //For Direct Internent : End


            return SMTPIP;

        }

        internal static string EncryptDecrypt(string input)
        {
            try
            {
                char[] key = { '9', '8', '7' };
                char[] output = new char[input.Length - 1];
                input = input.Remove(input.Length - 1, 1);

                for (int i = 0; i < input.Length; i++)
                {
                    output[i] = (char)(input[i] ^ key[i % (key.Length)] ^ (key[input.Length % (key.Length)] + key.Length));
                }

                return new string(output);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static bool PingValidation(string SMTPIP)
        {
            Ping pingSender = new Ping();
            string address = SMTPIP;
            try
            {
                PingReply reply = pingSender.Send(address);
                if (reply.Status == IPStatus.DestinationHostUnreachable)
                {
                    IsPing = false;
                    return false;
                }
                if (reply.Status == IPStatus.DestinationNetworkUnreachable)
                {
                    IsPing = false;
                    return false;
                }
                TcpClient tcpClient = new TcpClient();
                try
                {
                    tcpClient.Connect(SMTPIP, Convert.ToInt16(SMTPPort)); //Port Opened
                    PortStatus = 1;

                }
                catch (Exception)
                {
                    PortStatus = 0; //Port closed
                }
                if (reply.Status != IPStatus.Success)
                {
                    //MessageBox.Show("SMTP IP ping is not reachable");
                    return false;
                }

                if (reply.Status == IPStatus.Success && PortStatus == 1)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                IsPing = false;
                return false;
            }

        }

        public bool TestMailStatus(string ToTestMail)
        {
            try
            {
                if (DirectOrSMTPFlag == "1")
                {
                    try
                    {
                        //  MessageBox.Show("Indise TestMailStatus Direct", "Information");
                        MailMessage msg = new MailMessage(GmailEmailID, ToTestMail);
                        msg.Subject = "Test Email";
                        msg.Body = "Test Email";
                        msg.BodyEncoding = Encoding.UTF8;
                        msg.IsBodyHtml = true;

                        SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                        client.EnableSsl = true;
                        client.UseDefaultCredentials = false;
                        client.Credentials = new System.Net.NetworkCredential(GmailEmailID, GmailPassword);
                        client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                        //client.Credentials = basicCredential;
                        // MessageBox.Show("Indise TestMailStatus Direct before send", "Information");
                        client.Send(msg);
                        // MessageBox.Show("Indise TestMailStatus Direct after send", "Information");
                        return true;
                    }
                    catch (IOException ex)
                    {
                        MessageBox.Show("Unable To connect to the port as it is not open", "Information");
                        return false;
                    }
                    catch (SocketException ex)
                    {
                        MessageBox.Show("An attempt was made to access a socket in a way forbidden by its access permissions", "Information");
                        return false;
                    }
                    catch (Exception ex1)
                    {

                        if (ex1.ToString().Contains("The remote name could not be resolved: 'smtp.gmail.com'"))
                        {
                            MessageBox.Show("Please check your internet connection", "Information");
                        }


                        if (ex1.ToString().Contains("The SMTP server requires a secure connection or the client was not authenticated."))
                        {
                            MessageBox.Show("The gmail SMTP server requires a secure connection or the client was not authenticated.", "Information");
                        }

                        // MessageBox.Show(ex.Message.ToString(), "Information");
                        // MessageBox.Show(ex.InnerException.ToString(), "Information");
                        return false;
                    }
                }
                else
                {
                    try
                    {
                        MailMessage msg = new MailMessage(FromEmail, ToTestMail);
                        msg.Subject = "Test Email";
                        msg.Body = "Test Email";
                        msg.BodyEncoding = Encoding.UTF8;
                        msg.IsBodyHtml = true;
                        SmtpClient client = new SmtpClient(SMTPIP, Convert.ToInt16(SMTPPort));
                        client.UseDefaultCredentials = false;
                        System.Net.NetworkCredential basicCredential = new System.Net.NetworkCredential(SMTPEmailID, SMTPPassword);
                        client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                        client.Credentials = basicCredential;
                        client.Send(msg);
                        return true;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString(), "Information");
                        return false;
                    }


                }
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        private void VisibilityControl(object e)
        {
            if(e!=null)
            {
                System.Windows.Controls.RadioButton rb = e as System.Windows.Controls.RadioButton;
                if (rb.Name== "DirectCheckbox")
                {
                    DirectVisibility = "visible";// System.Windows.Visibility.Visible;
                    ;
                   

                }
                else if (rb.Name == "SMTPCheckbox")
                {
                    SMTPVisibility = "visible";
                    DirectVisibility = "hidden";
                }
            }
            //if (DirectChecked == true)
            //{
              
            //}
            //else if (SMTPChecked == true)
            //{
               


            //}
        }
        //{
        //    SMTPVisibility = Boolean.FalseString;
        //    DirectVisibility = "Visible";
        //}
        //else if(SMTPChecked == true)
        //{
        //    SMTPVisibility = "Visible";
        //    DirectVisibility = Boolean.FalseString;
        //}




        private void TestEmailClick_Click(object e)
        {
            if(e != null)
            {
                PasswordBox pwdbox = e as PasswordBox;
                GmailPassword = pwdbox.Password;
               
            }
            SaveInINI();
            try
            {
                //MessageBox.Show("Indise TestMailStatus Direct", "Information");
                //MailMessage msg = new MailMessage(FromEmailID, ToEmailID);
                //msg.Subject = "Test Email";
                //msg.Body = "Test Email";
                //msg.BodyEncoding = Encoding.UTF8;
                //msg.IsBodyHtml = true;
                //SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                //// client.EnableSsl = true;
                //client.UseDefaultCredentials = false;
                //System.Net.NetworkCredential basicCredential = new System.Net.NetworkCredential(FromEmailID, txtPswd);
                //client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                //client.Credentials = basicCredential;
                //MessageBox.Show("Indise TestMailStatus Direct before send", "Information");
                //client.Send(msg);
                //MessageBox.Show("Indise TestMailStatus Direct after send", "Information");


                SMTPIP = ReadEmailDetailFromINI();
                if (DirectOrSMTPFlag != "1") //i.e SMTP, not direct connection 
                {
                    // MessageBox.Show("Indise SMTP AM_EmailConfig", "Information");
                    IsPing = PingValidation(SMTPIP); //PORT and IP address checks are built
                    if (PortStatus == 0)
                        MessageBox.Show("Port is not open", "Information");

                    if (IsPing == true)
                    {
                        if (TestMailStatus(ToTestEmailID))
                        {   /////
                            TestMailSuccess = 1;
                            MessageBox.Show("Test Mail Successfull.", "Information");
                             SaveInINI();
                        }
                        else
                        {
                            MessageBox.Show("Test Mail not sent successfully.", "Information");
                        }
                    }
                    else
                    {
                        MessageBox.Show("SMTP IP ping is not reachable.", "Information");
                        // send message to TWS with 0
                    }
                }
                else
                {
                    // MessageBox.Show("Indise direct AM_EmailConfig", "Information");
                    if (TestMailStatus(ToTestEmailID))
                    {   /////
                        // MessageBox.Show("Indise TestMailStatus(ToTestEmailID) if true", "Information");
                        TestMailSuccess = 1;
                        MessageBox.Show("Test Mail Successfull.", "Information");
                    }
                    else
                    {
                        //MessageBox.Show("Indise TestMailStatus(ToTestEmailID) if false", "Information");
                        MessageBox.Show("Test Mail not sent successfully.", "Information");
                    }
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.InnerException.ToString(), "Information");
            }
        }

        private void SaveInINI()
        {

            SMTPIP = ((View.Profiling.EmailProfiling)(Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().EmailProfiling.Content)).SMTPIP.Text;
            InterfaceIP = ((View.Profiling.EmailProfiling)(Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().EmailProfiling.Content)).InterfaceIP.Text;
            // GatewayIP = ((View.Profiling.EmailProfiling)(Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().EmailProfiling.Content)).GatewayIP.Text;
            //For Test SMTP : Start
            MainWindowVM.parser.AddSetting("SMTP EMAIL PROFILING", "DirectOrSMTPFlag", DirectOrSMTPFlag);
            MainWindowVM.parser.AddSetting("SMTP EMAIL PROFILING", "TestSMTPServerIP", SMTPIP);
            MainWindowVM.parser.AddSetting("SMTP EMAIL PROFILING", "TestSMTPPort", SMTPPort);
            MainWindowVM.parser.AddSetting("SMTP EMAIL PROFILING", "TestSMTPEmailID", SMTPEmailID);
            MainWindowVM.parser.AddSetting("SMTP EMAIL PROFILING", "TestSMTPPassword", SMTPPassword);
            if (!string.IsNullOrEmpty(SMTPPassword))//Vijayalakshmi - To implement Decryption of the encrypted password
            {
                string SMTPdecrypted = EncryptDecrypt(SMTPPassword);
                SMTPPassword = SMTPdecrypted;
            }//Vijayalakshmi - To implement Decryption of the encrypted password
            MainWindowVM.parser.AddSetting("SMTP EMAIL PROFILING", "TestFromEmailID", FromEmail);
            //For Test SMTP : End
            //For Live SMTP: Start
            //MainWindowVM.parser.AddSetting("SMTP EMAIL PROFILING", "LiveSMTPServerIP", LiveSMTPIP);
            //MainWindowVM.parser.AddSetting("SMTP EMAIL PROFILING", "LiveSMTPPort", LiveSMTPPort);
            //MainWindowVM.parser.AddSetting("SMTP EMAIL PROFILING", "LiveSMTPEmailID", LiveSMTPEmailID);
            //MainWindowVM.parser.AddSetting("SMTP EMAIL PROFILING", "LiveSMTPPassword", LiveSMTPPassword);
            //if (!string.IsNullOrEmpty(LiveSMTPPassword))//Vijayalakshmi - To implement Decryption of the encrypted password
            //{
            //    string LiveDecrypted = EncryptDecrypt(LiveSMTPPassword);
            //    LiveSMTPPassword = LiveDecrypted;
            //}//Vijayalakshmi - To implement Decryption of the encrypted password
            //MainWindowVM.parser.AddSetting("SMTP EMAIL PROFILING", "LiveFromEmailID", LiveFromEmail);
            //MainWindowVM.parser.AddSetting("SMTP EMAIL PROFILING", "LiveSettings", LiveSettings);
            //MessageBox.Show(LiveSettings, "Information");
            //For Live SMTP: End
            //For Direct Internent : Start
            MainWindowVM.parser.AddSetting("SMTP EMAIL PROFILING", "GmailID", GmailEmailID);
            MainWindowVM.parser.AddSetting("SMTP EMAIL PROFILING", "Password", GmailPassword);
            //if (!string.IsNullOrEmpty(GmailPassword))//Vijayalakshmi - To implement Decryption of the encrypted password
            //{
            //    string GmailDecrypted = EncryptDecrypt(GmailPassword);
            //    GmailPassword = GmailDecrypted;
            //}//Vijayalakshmi - To implement Decryption of the encrypted password
            MainWindowVM.parser.AddSetting("SMTP EMAIL PROFILING", "ToTestEmailID", ToTestEmailID);
            MainWindowVM.parser.SaveSettings(MainWindowVM.TwsINIPath.ToString());
            //For Direct Internent : End
            
        }




        private void EmailProfilingLoaded_Load()
        {

        }

        private void SaveAllChanges(object e)
        {
            

        }

        private void SaveChanges(object e)
        {

        }

        private void AddTWSRoute(object e)
        {

        }

        private void ClearClickChanges(object e)
        {
            SMTPIP = string.Empty;
            SMTPPort = string.Empty;
            SMTPPassword = string.Empty;
            SMTPEmailID = string.Empty;
        }



        #endregion

        #region Constructor

        public EmailProfilingVM()

        {
            ReadEmailDetailFromINI();
            SMTPChecked = true;
        }

        #endregion

    }
}
