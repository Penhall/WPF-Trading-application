using CommonFrontEnd;
using CommonFrontEnd.Common;
using CommonFrontEnd.Constants;
using CommonFrontEnd.Global;
using CommonFrontEnd.Model;
using System;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;
using static CommonFrontEnd.ControllerModel.SenderControllerModel;


namespace CommonFrontEnd.SharedMemories
{
#if TWS
    public static class ReadConfigurations
    {
        static string currentDir = Environment.CurrentDirectory;
        //Reading Bolt.xml
        //Read TWS.ini
        //Read TWSProfile.ini

        //keeping an xml file for this too instead of ini file.
        //create a dictationary with object serialized from xml
        public static void ReadDefaultConfigurations()
        {
            BoltAppSettings oBoltAppSettings = new BoltAppSettings();
            BoltUserSettings oBoltUserSettings = new BoltUserSettings();

            DirectoryInfo appDirectory = new DirectoryInfo(Path.GetFullPath(Path.Combine(currentDir, @"xml/Users/AppSettings/AAAAAA.xml")));
            DirectoryInfo userDirectory = new DirectoryInfo(Path.GetFullPath(Path.Combine(currentDir, @"xml/Users/UserSettings/UUUUUU.xml")));
            DirectoryInfo TWSProfileDirectory = new DirectoryInfo(Path.GetFullPath(Path.Combine(currentDir, @"Profile/TwsProOOOOOO.ini")));

            oBoltAppSettings = ReadConfiguration(oBoltAppSettings, appDirectory.ToString().Replace("AAAAAA", UtilityLoginDetails.GETInstance.FileName));
            oBoltUserSettings = ReadConfiguration(oBoltUserSettings, userDirectory.ToString().Replace("UUUUUU", UtilityLoginDetails.GETInstance.FileName));
            ReadConfigurationTWSProfileINI(TWSProfileDirectory.ToString().Replace("OOOOOO", UtilityLoginDetails.GETInstance.FileName));
            ConfigurationMasterMemory.ConfigurationDict.AddOrUpdate("WindowsPosition", oBoltAppSettings.WindowsPosition, (key, oldValue) => oBoltAppSettings.WindowsPosition);
            ConfigurationMasterMemory.ConfigurationDict.AddOrUpdate("Bolt", oBoltUserSettings.Bolt, (key, oldValue) => oBoltUserSettings.Bolt);
            ConfigurationMasterMemory.ConfigurationDict.AddOrUpdate("TWS", oBoltUserSettings.TWSSettings, (key, oldValue) => oBoltUserSettings.TWSSettings);
        }

        public static void ReadETIStructure()
        {
            Messages msgStructure = null;
            DirectoryInfo directory = new DirectoryInfo(
          Path.GetFullPath(Path.Combine(currentDir, @"xml/ETI_Structure/ETI_Structure.xml")));
            //string xmlPath = @"E:\TWS_DotNetNewStructure\TWS_DOTNETT\CommonFrontEnd\xml\ETI_Structure\ETI_Structure.xml";
            msgStructure = ReadConfiguration(msgStructure, directory.ToString());

            foreach (MessagesMessage msgItem in msgStructure.Message)
            {
                if (msgItem.Type.ToLower().Equals("request"))
                {
                    ConfigurationMasterMemory.RequestDict.TryAdd(Convert.ToInt64(msgItem.Number), msgItem);
                }
                if (msgItem.Type.ToLower().Equals("reply"))
                {
                    ConfigurationMasterMemory.ReplyDict.TryAdd(Convert.ToInt64(msgItem.Number), msgItem);
                }
                else if (msgItem.Type.ToLower().Equals("ums"))
                {
                    ConfigurationMasterMemory.UmsDict.TryAdd(Convert.ToInt64(msgItem.Number), msgItem);
                }
                else if (msgItem.Type.ToLower().Equals("repeat"))
                {
                    ConfigurationMasterMemory.RepeatDict.TryAdd(Convert.ToInt64(msgItem.Number), msgItem);
                }
            }
        }
        public static void ReadReturnedOrderMessages()
        {
            try
            {
                ReturnedOrderMessages msgStructure = null;
                DirectoryInfo directory = new DirectoryInfo(Path.GetFullPath(Path.Combine(currentDir, @"xml/ReturnOrderMessages/ReturnOrderMessages.xml")));
                msgStructure = ReadConfiguration(msgStructure, directory.ToString());

                foreach (ReturnedOrderMessagesFilter msgItem in msgStructure.MessageFilters)
                {
                    if (msgItem.Type.ToLower().Equals("other"))
                    {
                        foreach (ReturnedOrderMessagesFilterCode item in msgItem.Code)
                        {

                            string key = string.Format("{0}_{1}", item.ReplyCode, msgItem.Type);
                            CommonFrontEnd.Model.OtherMessageStructure.OrderTypeMessage oOrderTypeMessage = new Model.OtherMessageStructure.OrderTypeMessage();
                            oOrderTypeMessage.MessageList = new System.Collections.Generic.Dictionary<uint, string>();
                            foreach (ReturnedOrderMessagesFilterCodeItems omessages in item.Items)
                            {
                                oOrderTypeMessage.MessageList.Add(omessages.MessageType, omessages.Message);
                            }

                            MemoryManager.ReturnedOrderDictionaryFilter.Add(key, oOrderTypeMessage);
                        }
                    }
                    if (msgItem.Type.ToLower().Equals("spos"))
                    {
                        //ConfigurationMasterMemory.ReplyDict.TryAdd(Convert.ToInt64(msgItem.Number), msgItem);
                        foreach (ReturnedOrderMessagesFilterCode item in msgItem.Code)
                        {

                            string key = string.Format("{0}_{1}", item.ReplyCode, msgItem.Type);
                            CommonFrontEnd.Model.OtherMessageStructure.OrderTypeMessage oOrderTypeMessage = new Model.OtherMessageStructure.OrderTypeMessage();
                            oOrderTypeMessage.MessageList = new System.Collections.Generic.Dictionary<uint, string>();
                            foreach (ReturnedOrderMessagesFilterCodeItems omessages in item.Items)
                            {
                                oOrderTypeMessage.MessageList.Add(omessages.MessageType, omessages.Message);
                            }

                            MemoryManager.ReturnedOrderDictionaryFilter.Add(key, oOrderTypeMessage);
                        }
                    }
                    if (msgItem.Type.ToLower().Equals("rrm"))
                    {
                        //ConfigurationMasterMemory.UmsDict.TryAdd(Convert.ToInt64(msgItem.Number), msgItem);
                        foreach (ReturnedOrderMessagesFilterCode item in msgItem.Code)
                        {

                            string key = string.Format("{0}_{1}", item.ReplyCode, msgItem.Type);
                            CommonFrontEnd.Model.OtherMessageStructure.OrderTypeMessage oOrderTypeMessage = new Model.OtherMessageStructure.OrderTypeMessage();
                            oOrderTypeMessage.MessageList = new System.Collections.Generic.Dictionary<uint, string>();
                            foreach (ReturnedOrderMessagesFilterCodeItems omessages in item.Items)
                            {
                                oOrderTypeMessage.MessageList.Add(omessages.MessageType, omessages.Message);
                            }

                            MemoryManager.ReturnedOrderDictionaryFilter.Add(key, oOrderTypeMessage);
                        }
                    }
                    if (msgItem.Type.ToLower().Equals("eossess"))
                    {
                        //ConfigurationMasterMemory.RepeatDict.TryAdd(Convert.ToInt64(msgItem.Number), msgItem);
                        foreach (ReturnedOrderMessagesFilterCode item in msgItem.Code)
                        {

                            string key = string.Format("{0}_{1}", item.ReplyCode, msgItem.Type);
                            CommonFrontEnd.Model.OtherMessageStructure.OrderTypeMessage oOrderTypeMessage = new Model.OtherMessageStructure.OrderTypeMessage();
                            oOrderTypeMessage.MessageList = new System.Collections.Generic.Dictionary<uint, string>();
                            foreach (ReturnedOrderMessagesFilterCodeItems omessages in item.Items)
                            {
                                oOrderTypeMessage.MessageList.Add(omessages.MessageType, omessages.Message);
                            }

                            MemoryManager.ReturnedOrderDictionaryFilter.Add(key, oOrderTypeMessage);
                        }
                    }
                    if (msgItem.Type.ToLower().Equals("masscancell"))
                    {
                        //ConfigurationMasterMemory.RepeatDict.TryAdd(Convert.ToInt64(msgItem.Number), msgItem);
                        foreach (ReturnedOrderMessagesFilterCode item in msgItem.Code)
                        {

                            string key = string.Format("{0}_{1}", item.ReplyCode, msgItem.Type);
                            CommonFrontEnd.Model.OtherMessageStructure.OrderTypeMessage oOrderTypeMessage = new Model.OtherMessageStructure.OrderTypeMessage();
                            oOrderTypeMessage.MessageList = new System.Collections.Generic.Dictionary<uint, string>();
                            foreach (ReturnedOrderMessagesFilterCodeItems omessages in item.Items)
                            {
                                oOrderTypeMessage.MessageList.Add(omessages.MessageType, omessages.Message);
                            }

                            MemoryManager.ReturnedOrderDictionaryFilter.Add(key, oOrderTypeMessage);
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);

            }
        }
        public static void SaveAllConfigSettings()
        {

        }
        public static void ReadConfigurationTWSProfileINI(string path)
        {
            IniParser ParserTWSProfileini = new IniParser(path);
            //ParserTWSProfileini.GetSetting("OPENWINDOW SETTINGS", "NORMALOEOPEN");
            UtilityOrderDetails.GETInstance.IsEqtShortClientIDChecked = ParserTWSProfileini.GetSetting("OE SETTINGS", "IsEqtShortClientIDChecked");
            UtilityOrderDetails.GETInstance.EqtShortClientID = ParserTWSProfileini.GetSetting("OE SETTINGS", "EqtShortClientID");

            UtilityOrderDetails.GETInstance.IsDervShortClientIDChecked = ParserTWSProfileini.GetSetting("OE SETTINGS", "IsDervShortClientIDChecked");
            UtilityOrderDetails.GETInstance.DervShortClientID = ParserTWSProfileini.GetSetting("OE SETTINGS", "DervShortClientID");

            UtilityOrderDetails.GETInstance.IsCurrShortClientIDChecked = ParserTWSProfileini.GetSetting("OE SETTINGS", "IsCurrShortClientIDChecked");
            UtilityOrderDetails.GETInstance.CurrShortClientID = ParserTWSProfileini.GetSetting("OE SETTINGS", "CurrShortClientID");

            UtilityOrderDetails.GETInstance.IsDebtShortClientIDChecked = ParserTWSProfileini.GetSetting("OE SETTINGS", "IsDebtShortClientIDChecked");
            UtilityOrderDetails.GETInstance.DebtShortClientID = ParserTWSProfileini.GetSetting("OE SETTINGS", "DebtShortClientID");
        }
        public static void SaveConfigurationTWSProfileINI(string path)
        {
            IniParser ParserTWSProfileini = new IniParser(path);
            //ParserTWSProfileini.GetSetting("OPENWINDOW SETTINGS", "NORMALOEOPEN");
            ParserTWSProfileini.AddSetting("OE SETTINGS", "IsEqtShortClientIDChecked",UtilityOrderDetails.GETInstance.IsEqtShortClientIDChecked);
            ParserTWSProfileini.AddSetting("OE SETTINGS", "EqtShortClientID", UtilityOrderDetails.GETInstance.EqtShortClientID);

            ParserTWSProfileini.AddSetting("OE SETTINGS", "IsDervShortClientIDChecked", UtilityOrderDetails.GETInstance.IsDervShortClientIDChecked);
            ParserTWSProfileini.AddSetting("OE SETTINGS", "DervShortClientID", UtilityOrderDetails.GETInstance.DervShortClientID);

            ParserTWSProfileini.AddSetting("OE SETTINGS", "IsCurrShortClientIDChecked", UtilityOrderDetails.GETInstance.IsCurrShortClientIDChecked);
            ParserTWSProfileini.AddSetting("OE SETTINGS", "CurrShortClientID", UtilityOrderDetails.GETInstance.CurrShortClientID);

            ParserTWSProfileini.AddSetting("OE SETTINGS", "IsDebtShortClientIDChecked", UtilityOrderDetails.GETInstance.IsDebtShortClientIDChecked);
            ParserTWSProfileini.AddSetting("OE SETTINGS", "DebtShortClientID", UtilityOrderDetails.GETInstance.DebtShortClientID);

            ParserTWSProfileini.SaveSettings(path);
        }
        public static T ReadConfiguration<T>(T objects, string xmlPath)
        {
            try
            {


                XmlSerializer serializer = new XmlSerializer(typeof(T));
                serializer.UnknownNode += new XmlNodeEventHandler(serializer_UnknownNode);
                serializer.UnknownAttribute += new XmlAttributeEventHandler(serializer_UnknownAttribute);

                using (FileStream fs = new FileStream(xmlPath, FileMode.Open))
                {
                    objects = (T)serializer.Deserialize(fs);
                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
                throw;
            }
            return objects;
        }

        private static void serializer_UnknownNode(object sender, XmlNodeEventArgs e)
        {
            //log unknown node
            String unknownNode = "Unknown Node:" + e.Name + " " + e.Text;
        }

        private static void serializer_UnknownAttribute(object sender, XmlAttributeEventArgs e)
        {
            //log unknown attribute
            System.Xml.XmlAttribute attr = e.Attr;
            String unknownAttribute = "Unknown attribute " + attr.Name + "='" + attr.Value + "'";
        }
    }
#elif BOW


    public class ConnSettings
{
    //: File Names
    public static readonly string FILENAME_SETTINGS = "Settings.txt";
    //:
    private const string LAN_SETTINGS = "Settings.txt";
    private const string INTERNET_SETTINGS = "ISettings.txt";
    private const string LEASE_SETTINGS = "LSettings.txt";
    private const string DR_SETTINGS = "DRSettings.txt";
    private string mstrServer;
    private bool mblnVSATUser;
    //: Proxy
    private string mstrProxyServer;
    private int mintProxyPort;
    private string mstrProxyUser;
    private string mstrProxyPassword;
    //: FTP
    private string mstrFtpServer = "";
    private string mstrFtpUserName = "";
    private string mstrFtpPassword = "";
    private string mstrFtpPath = "";
    private string mstrFtpMastersFileName = "Masters.zip";
    private string mstrFtpScreensFileName = "Screens.zip";
    private string mstrFtpCommoditiesFileName = "";
    //:
    public static readonly long MIN_LOCK_INTERVAL = 60000;
    public static readonly long MIN_HTTP_TIMEOUT = 30000;
    public static readonly long MIN_LOGOUT_TIME = 120000;
    //: This is the default time which is set if any value below MIN_LOCK_INTERVAL
    public static readonly long DEFAULT_LOCK_INTERVAL = 36000000;
    public static readonly long DEFAULT_LOGOUT_INTERVAL = 42000000;
    //:
    private long mlngLockInterval = DEFAULT_LOCK_INTERVAL;
    private int mintHTTPTimeOut = BowConstants.HTTP_TIMEOUT_MIN;
    private long mlngLogOutTime = DEFAULT_LOGOUT_INTERVAL;
    private bool mblnUSESecureHTTP;
    private bool mblnHTTPCompressed;
    private string mstrSocketCompression = "";
    private bool mblnCombinedMBP;
    //:
    private int mintDeltaBatchSize;
    private int mintDeltaMaxTries;
    private string mstrMyTitle;
    private string mstrSebiRegNo;
    private bool mblnShowArbitrage;
    private bool mblnMulticast;
    //: CONSTANTS
    private const string SETTINGS_SERVER = "SERVER";
    private const string SETTINGS_VSAT = "VSAT";
    //: PROXY - CONSTANTS
    private const string SETTINGS_PROXYSERVER = "PROXYSERVER";
    private const string SETTINGS_PROXYPORT = "PROXYPORT";
    private const string SETTINGS_PROXYUSER = "PROXYUSER";
    private const string SETTINGS_PROXYPASSWORD = "PROXYPASSWORD";
    //: FTP - CONSTANTS
    private const string SETTINGS_FTPSERVER = "FTPSERVER";
    private const string SETTINGS_FTPUSERNAME = "FTPUSERNAME";
    private const string SETTINGS_FTPPASSWORD = "FTPPASSWORD";
    private const string SETTINGS_FTPPATH = "FTPPATH";
    private const string SETTINGS_FTPMASTERSFILENAME = "FTPMASTERSFILENAME";
    private const string SETTINGS_FTPSCREENSFILENAME = "FTPSCREENSFILENAME";
    private const string SETTINGS_FTPCOMMODITIESFILENAME = "FTPCOMMODITIESFILENAME";
    //: 
    private const string SETTINGS_LOCKTERMINAL = "LOCKTERMINAL";
    private const string SETTINGS_HTTPTIMEOUT = "HTTPTIMEOUT";
    private const string SETTINGS_LOGOUTTIME = "LOGOUTTIME";
    //:
    private const string SETTINGS_DELTADOWNLOADBATCHSIZE = "DELTADOWNLOADBATCHSIZE";
    private const string SETTINGS_DELTADOWNLOADMAXRETRIES = "DELTADOWNLOADMAXRETRIES";
    private const string SETTINGS_MYTITLE = "MYTITLE";
    private const string SETTINGS_SEBIREGNO = "SEBIREGNO";
    #region " Properties / Read / Save for Settings.txt "
    private const string SETTINGS_MULTICAST = "MULTICAST";
    #region " Properties "
    public string Server {
        get { return mstrServer; }
        set { mstrServer = value; }
    }
    public bool VSATUser {
        get { return mblnVSATUser; }
        set { mblnVSATUser = value; }
    }
    //: Proxy
    public string ProxyServer {
        get { return mstrProxyServer; }
        set { mstrProxyServer = value; }
    }
    public int ProxyPort {
        get { return mintProxyPort; }
        set { mintProxyPort = value; }
    }
    public string ProxyUser {
        get { return mstrProxyUser; }
        set { mstrProxyUser = value; }
    }
    public string ProxyPassword {
        get { return mstrProxyPassword; }
        set { mstrProxyPassword = value; }
    }
    //: FTP
    public string FtpServer {
        get { return mstrFtpServer; }
        set { mstrFtpServer = value; }
    }
    public string FtpUserName {
        get { return mstrFtpUserName; }
        set { mstrFtpUserName = value; }
    }
    public string FtpPassword {
        get { return mstrFtpPassword; }
        set { mstrFtpPassword = value; }
    }
    public string FtpPath {
        get { return mstrFtpPath; }
        set { mstrFtpPath = value; }
    }
    public string FtpMastersFileName {
        get { return mstrFtpMastersFileName; }
        set { mstrFtpMastersFileName = value; }
    }
    public string FtpScreensFileName {
        get { return mstrFtpScreensFileName; }
        set { mstrFtpScreensFileName = value; }
    }
    public string FtpCommoditiesFileName {
        get { return mstrFtpCommoditiesFileName; }
        set { mstrFtpCommoditiesFileName = value; }
    }
    //:
    public long LogOutTime {
        get { return mlngLogOutTime; }
        set { mlngLogOutTime = value; }
    }
    public long LockInterval {
        get { return mlngLockInterval; }
        set { mlngLockInterval = value; }
    }
    public int HTTPTimeOut {
        get { return mintHTTPTimeOut; }
        set { mintHTTPTimeOut = value; }
    }
    public bool HTTPCompressed {
        get { return mblnHTTPCompressed; }
        set { mblnHTTPCompressed = value; }
    }

    public bool UseSecureHTTP {
        get { return mblnUSESecureHTTP; }
        set { mblnUSESecureHTTP = value; }
    }
    public string SocketCompression {
        get { return mstrSocketCompression; }
        set { mstrSocketCompression = value; }
    }
    public bool CombinedMBP {
        get { return mblnCombinedMBP; }
        set { mblnCombinedMBP = value; }
    }
    public int DeltaBatchSize {
        get { return mintDeltaBatchSize; }
        set { mintDeltaBatchSize = value; }
    }
    public int DeltaMaxTries {
        get { return mintDeltaMaxTries; }
        set { mintDeltaBatchSize = value; }
    }
    public string MyTitle {
        get { return mstrMyTitle; }
        set { mstrMyTitle = value; }
    }
    public string SebiRegNo {
        get { return mstrSebiRegNo; }
        set { mstrSebiRegNo = value; }
    }
    public bool ShowArbitrage {
        get { return mblnShowArbitrage; }
        set { mblnShowArbitrage = value; }
    }
    public bool Multicast {
        get { return mblnMulticast; }
        set { mblnMulticast = value; }
    }
    #endregion

    public void ReadSettings(string pstrFilename)
    {
        StreamReader lobjStreamReader = null;
        string lstrBuffer = null;
        string[] lstrTempValues = null;
        StackTrace lobjStackTrace = new StackTrace();

        try {
            string lstrCurrFile = null;
            if (pstrFilename == "LAN") {
                lstrCurrFile = LAN_SETTINGS;
            } else if (pstrFilename == "INTERNET") {
                lstrCurrFile = INTERNET_SETTINGS;
            } else if (pstrFilename == "LEASED LINE") {
                lstrCurrFile = LEASE_SETTINGS;
            } else if (pstrFilename == "DR") {
                lstrCurrFile = DR_SETTINGS;
            } else {
                lstrCurrFile = LAN_SETTINGS;
            }

            if ((File.Exists(Environment.CurrentDirectory + "\\" + lstrCurrFile))) {
                // : Initializing the values that we are going to find in the file
                InitializeValuesFoundInSettingTxt();

                lobjStreamReader = new StreamReader(Environment.CurrentDirectory + "\\" + lstrCurrFile);
                lstrBuffer = lobjStreamReader.ReadLine();
                //Infrastructure.Logger.WriteLog(vbCrLf & "Called from = " & lobjStackTrace.ToString)
                Infrastructure.Logger.WriteLog("Current File Selection" + lstrCurrFile);
                Infrastructure.Logger.WriteLog("Started Reading Setting" + lstrBuffer);
                while (((lstrBuffer != null) && lstrBuffer.Trim().Length > 0)) {
                    lstrTempValues = lstrBuffer.Split('=');
                    Infrastructure.Logger.WriteLog("Reading Setting.txt " + lstrBuffer);
                    if ((lstrTempValues.Length == 2)) {
                        lstrTempValues[0] = lstrTempValues[0].Trim().ToUpper();
                        if (lstrTempValues[0].Trim().Length > 0) {
                            ProcessLine(lstrTempValues);
                        }
                    } else {
                        Infrastructure.Logger.WriteLog("Error while reading the parameter " + lstrTempValues[0] + " from the " + lstrCurrFile + ".txt file. ");
                    }
                    lstrBuffer = "";
                    lstrBuffer = lobjStreamReader.ReadLine();
                }
            } else {
                Infrastructure.Logger.WriteLog("Error in ReadSettings. File Dose Not Exists." + lstrCurrFile);
            }
        } catch (Exception ex) {
            Infrastructure.Logger.WriteLog("Error in Read Settings" + ex.Message);
        } finally {
            if ((lobjStreamReader != null)) {
                lobjStreamReader.Close();
                lobjStreamReader = null;
            }
        }
    }

    public void SaveSettings(string pstrFilename)
    {
        string lstrCurrFile = null;
        if (pstrFilename == "LAN") {
            lstrCurrFile = LAN_SETTINGS;
        } else if (pstrFilename == "INTERNET") {
            lstrCurrFile = INTERNET_SETTINGS;
        } else if (pstrFilename == "LEASED LINE") {
            lstrCurrFile = LEASE_SETTINGS;
        } else if (pstrFilename == "DR") {
            lstrCurrFile = DR_SETTINGS;
        } else {
            lstrCurrFile = LAN_SETTINGS;
            Infrastructure.Logger.WriteLog("Value of FileName was " + pstrFilename + " because of which Current File was selected as LAN.");
            StackTrace lobjCallStack = new StackTrace();
            Infrastructure.Logger.WriteLog("The call was made from " + lobjCallStack.ToString());
        }
        StreamWriter lStreamWriter = null;
        try {
            //: If the File Exists removing the ReadOnly so that the Settings can be Overriden
            if (File.Exists(Environment.CurrentDirectory + "\\" + lstrCurrFile) == true) {
                File.SetAttributes(Environment.CurrentDirectory + "\\" + lstrCurrFile, FileAttributes.Normal);
            }

            lStreamWriter = new StreamWriter(Environment.CurrentDirectory + "\\" + lstrCurrFile);
            lStreamWriter.WriteLine(SETTINGS_SERVER + "=" + mstrServer);

            //: PROXY
            if ((mstrProxyServer.Trim().Length > 0)) {
                lStreamWriter.WriteLine(SETTINGS_PROXYSERVER + "=" + mstrProxyServer);
                lStreamWriter.WriteLine(SETTINGS_PROXYPORT + "=" + mintProxyPort);
            }
            if ((mstrProxyUser.Trim().Length > 0)) {
                lStreamWriter.WriteLine(SETTINGS_PROXYUSER + "=" + mstrProxyUser);
            }
            if ((mstrProxyPassword.Trim().Length > 0)) {
                lStreamWriter.WriteLine(SETTINGS_PROXYPASSWORD + "=" + mstrProxyPassword);
            }

            //: FTP 
            if (mstrFtpServer.Trim().Length > 0) {
                lStreamWriter.WriteLine(SETTINGS_FTPSERVER + "=" + mstrFtpServer);
            }
            if (mstrFtpUserName.Trim().Length > 0) {
                lStreamWriter.WriteLine(SETTINGS_FTPUSERNAME + "=" + mstrFtpUserName);
            }
            if (mstrFtpPassword.Trim().Length > 0) {
                lStreamWriter.WriteLine(SETTINGS_FTPPASSWORD + "=" + mstrFtpPassword);
            }
            if (mstrFtpPath.Trim().Length > 0) {
                lStreamWriter.WriteLine(SETTINGS_FTPPATH + "=" + mstrFtpPath);
            }
            if (mstrFtpMastersFileName.Trim().Length > 0) {
                lStreamWriter.WriteLine(SETTINGS_FTPMASTERSFILENAME + "=" + mstrFtpMastersFileName);
            }
            if (mstrFtpScreensFileName.Trim().Length > 0) {
                lStreamWriter.WriteLine(SETTINGS_FTPSCREENSFILENAME + "=" + mstrFtpScreensFileName);
            }
            if (mstrFtpCommoditiesFileName.Trim().Length > 0) {
                lStreamWriter.WriteLine(SETTINGS_FTPCOMMODITIESFILENAME + "=" + mstrFtpCommoditiesFileName);
            }

            //: Others
            if (mlngLockInterval > 0) {
                lStreamWriter.WriteLine(SETTINGS_LOCKTERMINAL + "=" + mlngLockInterval);
            }

            if (mintHTTPTimeOut >= 0) {
                if (mintHTTPTimeOut < MIN_HTTP_TIMEOUT && mintHTTPTimeOut != 0) {
                    lStreamWriter.WriteLine(SETTINGS_HTTPTIMEOUT + "=" + MIN_HTTP_TIMEOUT);
                } else {
                    lStreamWriter.WriteLine(SETTINGS_HTTPTIMEOUT + "=" + mintHTTPTimeOut);
                }
            }

            if (mlngLogOutTime > 0) {
                lStreamWriter.WriteLine(SETTINGS_LOGOUTTIME + "=" + mlngLogOutTime);
            }

            if (mblnHTTPCompressed == true) {
                lStreamWriter.WriteLine(BowConstants.HTTP_COMPRESSED + "=" + BowConstants.YESNO_Y);
            } else {
                lStreamWriter.WriteLine(BowConstants.HTTP_COMPRESSED + "=" + BowConstants.YESNO_N);
            }

            if (mblnUSESecureHTTP == true) {
                lStreamWriter.WriteLine(BowConstants.USE_SECURE_HTTP + "=" + BowConstants.YESNO_Y);
            } else {
                lStreamWriter.WriteLine(BowConstants.USE_SECURE_HTTP + "=" + BowConstants.YESNO_N);
            }

            lStreamWriter.WriteLine(BowConstants.SOCKET_COMPRESSED + "=" + mstrSocketCompression);
            lStreamWriter.WriteLine(SETTINGS_MULTICAST + "=" + Multicast);

            if (mblnCombinedMBP == true) {
                lStreamWriter.WriteLine(BowConstants.COMBINED_MBP + "=" + BowConstants.BOOLEAN_TRUE_STRING);
            } else {
                lStreamWriter.WriteLine(BowConstants.COMBINED_MBP + "=" + BowConstants.BOOLEAN_FALSE_STRING);
            }
        } catch (Exception ex) {
            Infrastructure.Logger.WriteLog("Error in SaveSettings : " + ex.Message);
        } finally {
            if ((lStreamWriter != null)) {
                lStreamWriter.Close();
            }
        }
    }

    private void InitializeValuesFoundInSettingTxt()
    {
        mstrServer = "";
        mstrProxyServer = "";
        mintProxyPort = 0;
        mstrProxyUser = "";
        mstrProxyPassword = "";
        mblnVSATUser = false;
        mstrFtpServer = "";
        mstrFtpUserName = "";
        mstrFtpPassword = "";
        mstrFtpPath = "";
        mstrFtpMastersFileName = "Masters.zip";
        mstrFtpScreensFileName = "Screens.zip";
        mstrFtpCommoditiesFileName = "";
        mlngLockInterval = MIN_LOCK_INTERVAL;
        mintHTTPTimeOut =(int)MIN_HTTP_TIMEOUT;
        mlngLogOutTime = MIN_LOGOUT_TIME;
        mblnHTTPCompressed = false;
        mblnUSESecureHTTP = false;

        mstrSocketCompression = "N";
        mblnCombinedMBP = false;
        mintDeltaBatchSize = 0;
        mintDeltaMaxTries = 0;
        mstrMyTitle = "";
        mstrSebiRegNo = "";
        mblnShowArbitrage = false;
    }

    private void ProcessLine(string[] pstrSettingsParameters)
    {
        string lstrParameterName = null;
        string lstrParameterValue = null;
        try {
            lstrParameterName = pstrSettingsParameters[0].Trim().ToUpper();
            lstrParameterValue = pstrSettingsParameters[1].Trim();

            switch (lstrParameterName) {
                case SETTINGS_SERVER:
                        mstrServer = lstrParameterValue;
                    break;
                case SETTINGS_PROXYSERVER:
                        mstrProxyServer = lstrParameterValue;
                    break;
                case SETTINGS_PROXYPORT:
                        mintProxyPort = Convert.ToInt16(lstrParameterValue);
                    break;
                case SETTINGS_PROXYUSER:
                        mstrProxyUser = lstrParameterValue;
                    break;
                case SETTINGS_PROXYPASSWORD:
                        mstrProxyPassword = lstrParameterValue;
                    break;
                case SETTINGS_VSAT:
                        mblnVSATUser = false;
                    if ((lstrParameterValue == "Y" || lstrParameterValue == "YES" || lstrParameterValue == "TRUE" || lstrParameterValue == "T" || lstrParameterValue == "1")) {
                            mblnVSATUser = true;
                    }
                    break;
                //: For Automatic Downloading of SQLite Db files
                case SETTINGS_FTPSERVER:
                        mstrFtpServer = lstrParameterValue;
                    break;
                case SETTINGS_FTPUSERNAME:
                        mstrFtpUserName = lstrParameterValue;
                    break;
                case SETTINGS_FTPPASSWORD:
                        mstrFtpPassword = lstrParameterValue;
                    break;
                case SETTINGS_FTPPATH:
                        mstrFtpPath = lstrParameterValue;
                    break;
                case SETTINGS_FTPMASTERSFILENAME:
                        mstrFtpMastersFileName = lstrParameterValue;
                    break;
                case SETTINGS_FTPSCREENSFILENAME:
                        mstrFtpScreensFileName = lstrParameterValue;
                    break;
                case SETTINGS_FTPCOMMODITIESFILENAME:
                        mstrFtpCommoditiesFileName = lstrParameterValue;
                    break;
                case SETTINGS_LOCKTERMINAL:
                    if (lstrParameterValue.Length == 0) {
                            mlngLockInterval = MIN_LOCK_INTERVAL;
                    } else {
                            mlngLockInterval = Convert.ToInt64(lstrParameterValue);
                    }
                    break;
                case SETTINGS_HTTPTIMEOUT:
                    if (lstrParameterValue.Length == 0) {
                            mintHTTPTimeOut = (int)MIN_HTTP_TIMEOUT;
                    } else {
                        if (Convert.ToInt64(lstrParameterValue) < MIN_HTTP_TIMEOUT && Convert.ToInt32(lstrParameterValue) != 0) {
                                mintHTTPTimeOut = (int)MIN_HTTP_TIMEOUT;
                        } else {
                                mintHTTPTimeOut = Convert.ToInt32(lstrParameterValue);
                        }
                    }
                    break;
                case SETTINGS_LOGOUTTIME:
                    if (lstrParameterValue.Length == 0) {
                            mlngLogOutTime = MIN_LOGOUT_TIME;
                    } else {
                            mlngLogOutTime = Convert.ToInt32(lstrParameterValue);
                    }
                    break;
                case BowConstants.HTTP_COMPRESSED:
                    if (lstrParameterValue.Length == 0) {
                            mblnHTTPCompressed = false;
                    } else {
                        if (lstrParameterValue.ToUpper() == BowConstants.YESNO_Y) {
                                mblnHTTPCompressed = true;
                        } else {
                                mblnHTTPCompressed = false;
                        }
                    }
                    break;
                case BowConstants.USE_SECURE_HTTP:
                    if (lstrParameterValue.Length == 0) {
                            mblnUSESecureHTTP = false;
                    } else {
                        if (lstrParameterValue.ToUpper() == BowConstants.YESNO_Y) {
                                mblnUSESecureHTTP = true;
                        } else {
                                mblnUSESecureHTTP = false;
                        }
                    }
                    break;
                case BowConstants.SOCKET_COMPRESSED:
                        mstrSocketCompression = lstrParameterValue;
                    break;
                case BowConstants.SHOW_CHARTS:

                    break;
                case BowConstants.COMBINED_MBP:
                    if (lstrParameterValue.Length == 0) {
                            mblnCombinedMBP = false;
                    } else {
                        if (lstrParameterValue.ToUpper() == BowConstants.BOOLEAN_TRUE_STRING) {
                                mblnCombinedMBP = true;
                        } else {
                                mblnCombinedMBP = false;
                        }
                    }
                    break;
                case SETTINGS_DELTADOWNLOADBATCHSIZE:
                    if (lstrParameterValue.Length > 0) {
                            mintDeltaBatchSize = int.Parse(lstrParameterValue);
                    }
                    break;
                case SETTINGS_DELTADOWNLOADMAXRETRIES:
                    if (lstrParameterValue.Length > 0) {
                            mintDeltaMaxTries = int.Parse(lstrParameterValue);
                    }
                    break;
                case SETTINGS_MYTITLE:
                    if (!(lstrParameterValue.Length == 0)) {
                            mstrMyTitle = lstrParameterValue;
                    }
                    break;
                case SETTINGS_SEBIREGNO:
                    if (!(lstrParameterValue.Length == 0)) {
                            mstrSebiRegNo = lstrParameterValue;
                    }
                    break;
                //:Arbitrage
                case BowConstants.SHOW_ARBITRAGE:
                    if (lstrParameterValue == BowConstants.YESNO_Y) {
                            mblnShowArbitrage = true;
                    } else {
                            mblnShowArbitrage = false;
                    }
                        mblnShowArbitrage = false;
                    break;
                case SETTINGS_MULTICAST:
                    if (lstrParameterValue.ToUpper() == "TRUE") {
                            mblnMulticast = true;
                    } else {
                            mblnMulticast = false;
                    }
                    break;
                default:
                        Infrastructure.Logger.WriteLog("Some how the ParameterName did not matched with the Cases provided. Value of ParameterName =" + lstrParameterName + " .Value for which was =" + lstrParameterValue);
                    break;
            }
        } catch (Exception ex) {
            Infrastructure.Logger.WriteLog("Error in Process Line " + ex.Message);
        }
    }
    #endregion
}

#endif
}
