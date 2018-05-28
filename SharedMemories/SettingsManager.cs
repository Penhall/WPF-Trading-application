using CommonFrontEnd;
using CommonFrontEnd.Common;
using CommonFrontEnd.Constants;
using CommonFrontEnd.Model;
using CommonFrontEnd.GetDataForStock;
using CommonFrontEnd.Global;
using CommonFrontEnd.HTTPHlper;
using CommonFrontEnd.SocketConnection;
using CommonFrontEnd.Utility;
using CommonFrontEnd.Utility.Interfaces_Abstract;
using CommonFrontEnd.ViewModel.Touchline;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using static CommonFrontEnd.HTTPHlper.HTTPHlpr;
using CommonFrontEnd.Controller.Order;
using CommonFrontEnd.ViewModel;

namespace CommonFrontEnd.SharedMemories
{
    public partial class SettingsManager
    {
        internal static string AppSettingsXmlPath { get; set; }
    }
#if TWS
    public partial class SettingsManager
    {
    #region Properties
      
        private static string DefaultAppSettingsXmlPath { get; set; }
        private static string DefaultUserSettingsXmlPath { get; set; }
        private static string DefaultUserOrderSettingsINIPath { get; set; }

        private static string UsersDefAppSettingsXmlPath { get; set; }
        private static string UsersDefUserSettingsXmlPath { get; set; }
        private static string UsersDefUserOrderSettingsINIPath { get; set; }

        internal static string UserSettingsXmlPath { get; set; }
        internal static string UserOrderSettingsINIPath { get; set; }
        private static string FileName { get; set; }


    #endregion

        /// <summary>
        /// Invoked on default TWS load
        /// </summary>
        //public SettingsManager()
        //{

        //}
        /// <summary>
        /// Invoked once, after successful Login only
        /// </summary>
        public static void Initialize()
        {

            DirectoryInfo appDefaultDirectory = new DirectoryInfo(Path.GetFullPath(Path.Combine(UtilityLoginDetails.GETInstance.CurrentDirectory, @"xml/AppSettings/DefaultAppSettings.xml")));
            DirectoryInfo userDefaultDirectory = new DirectoryInfo(Path.GetFullPath(Path.Combine(UtilityLoginDetails.GETInstance.CurrentDirectory, @"xml/UserSettings/DefaultUserSettings.xml")));
            DirectoryInfo userOrderSettingDefaultDirectory = new DirectoryInfo(Path.GetFullPath(Path.Combine(UtilityLoginDetails.GETInstance.CurrentDirectory, @"Profile/TwsPro.ini")));

            DirectoryInfo appDirectory = new DirectoryInfo(Path.GetFullPath(Path.Combine(UtilityLoginDetails.GETInstance.CurrentDirectory, @"xml/Users/AppSettings/AAAAAA.xml")));
            DirectoryInfo userDirectory = new DirectoryInfo(Path.GetFullPath(Path.Combine(UtilityLoginDetails.GETInstance.CurrentDirectory, @"xml/Users/UserSettings/UUUUUU.xml")));
            DirectoryInfo userOrderSettingDirectory = new DirectoryInfo(Path.GetFullPath(Path.Combine(UtilityLoginDetails.GETInstance.CurrentDirectory, @"Profile/TwsProOOOOOO.ini")));

            //DefaultAppSettingsXmlPath = ConfigurationManager.AppSettings["DefaultAppSettingsXmlPath"].ToString();
            //DefaultUserSettingsXmlPath = ConfigurationManager.AppSettings["DefaultUserSettingsXmlPath"].ToString();
            //UsersDefAppSettingsXmlPath = ConfigurationManager.AppSettings["UsersDefAppSettingsXmlPath"].ToString();
            //UsersDefUserSettingsXmlPath = ConfigurationManager.AppSettings["UsersDefUserSettingsXmlPath"].ToString();

            DefaultAppSettingsXmlPath = appDefaultDirectory.ToString();
            DefaultUserSettingsXmlPath = userDefaultDirectory.ToString();
            DefaultUserOrderSettingsINIPath = userOrderSettingDefaultDirectory.ToString();

            UsersDefAppSettingsXmlPath = appDirectory.ToString();
            UsersDefUserSettingsXmlPath = userDirectory.ToString();
            UsersDefUserOrderSettingsINIPath = userOrderSettingDirectory.ToString();

            if (UtilityLoginDetails.GETInstance.MemberId != null && UtilityLoginDetails.GETInstance.TraderId != null)
            {
                UtilityLoginDetails.GETInstance.FileName = FileName = string.Format("{0}{1}", UtilityLoginDetails.GETInstance.MemberId, UtilityLoginDetails.GETInstance.TraderId.ToString().PadLeft(5, '0'));
            }

            AppSettingsXmlPath = UsersDefAppSettingsXmlPath.Replace("AAAAAA", FileName);
            UserSettingsXmlPath = UsersDefUserSettingsXmlPath.Replace("UUUUUU", FileName);
            UserOrderSettingsINIPath = UsersDefUserOrderSettingsINIPath.Replace("OOOOOO", FileName);

            CreateUserSpecificAppProfile(DefaultAppSettingsXmlPath, UsersDefAppSettingsXmlPath.Replace("AAAAAA", FileName));
            CreateUserSpecificUserProfile(DefaultUserSettingsXmlPath, UsersDefUserSettingsXmlPath.Replace("UUUUUU", FileName));
            CreateUserSpecificOrderSettingsINI(DefaultUserOrderSettingsINIPath, UserOrderSettingsINIPath.Replace("OOOOOO", FileName));
            ReadConfigurations.ReadDefaultConfigurations();

        }


        /// <summary>
        /// Copies default/master xml into new user specific App settings xml
        /// Invoked only once on successful Login
        /// </summary>
        /// <param name="defaultAppFile"></param>
        /// <param name="newAppFile"></param>
        public static void CreateUserSpecificAppProfile(string defaultAppFile, string newAppFile)
        {
            XmlDocument document = new XmlDocument();
            document.Load(defaultAppFile);

            // Modify XML file using XmlDocument here

            if (File.Exists(newAppFile))
            {
                //File.Delete(newAppFile);
            }
            else
            {
                document.Save(newAppFile);
            }
        }


        /// <summary>
        /// Copies default/master xml into new user specific User settings xml
        /// Invoked only once on successful Login
        /// </summary>
        /// <param name="defaultUserFile"></param>
        /// <param name="newUserFile"></param>
        public static void CreateUserSpecificUserProfile(string defaultUserFile, string newUserFile)
        {
            XmlDocument document = new XmlDocument();
            document.Load(defaultUserFile);

            // Modify XML file using XmlDocument here

            if (File.Exists(newUserFile))
            {
                //File.Delete(newAppFile);
            }
            else
            {
                document.Save(newUserFile);
            }
        }

        public static void CreateUserSpecificOrderSettingsINI(string defaultUserFile, string newUserFile)
        {
            //bool exists = System.IO.Directory.Exists(defaultUserFile);

            //if (!exists)
            //{
            //    System.IO.Directory.CreateDirectory(defaultUserFile);
            //}
            if (!File.Exists(defaultUserFile))
            {
                IniParser ParserTWSProfileini = new IniParser(defaultUserFile);
                //ParserTWSProfileini.GetSetting("OPENWINDOW SETTINGS", "NORMALOEOPEN");
                ParserTWSProfileini.AddSetting("OE SETTINGS", "IsEqtShortClientIDChecked", "0");
                ParserTWSProfileini.AddSetting("OE SETTINGS", "EqtShortClientID", string.Empty);

                ParserTWSProfileini.AddSetting("OE SETTINGS", "IsDervShortClientIDChecked", "0");
                ParserTWSProfileini.AddSetting("OE SETTINGS", "DervShortClientID", string.Empty);

                ParserTWSProfileini.AddSetting("OE SETTINGS", "IsCurrShortClientIDChecked", "0");
                ParserTWSProfileini.AddSetting("OE SETTINGS", "CurrShortClientID", string.Empty);

                ParserTWSProfileini.AddSetting("OE SETTINGS", "IsDebtShortClientIDChecked", "0");
                ParserTWSProfileini.AddSetting("OE SETTINGS", "DebtShortClientID", string.Empty);

                ParserTWSProfileini.SaveSettings(defaultUserFile);
            }

            if (!File.Exists(newUserFile))
            {
                IniParser o = new IniParser(defaultUserFile);
                o.SaveSettings(newUserFile);
            }
            
            
        }
    }
#endif

#if BOW
    public partial class SettingsManager
    {
        private string _ProductHelp_Internet_Mode;
        private string _ProductHelp_LeasedLine_Mode;
        private static string mstrPreviousMessage { get; set; }

        // For HTTP TimeOut

        public static bool _100Continues = true;
        UtilityLoginDetails objUtilityLoginDetails = UtilityLoginDetails.GETInstance;
        static Thread gobjInteractiveThread;
        public static ConcurrentQueue<string> mobjSync_MsgQueue = new ConcurrentQueue<string>();

        public static System.Threading.Thread[] gobjBroadCastMessagePumpAdditionalThreads = new System.Threading.Thread[1];
        public static int mintActualMWOpen;
        public static UtilityScript gobjUtilityScript;
        public static Action<IdicesDetailsMain> indicesBroadCastBow;
        internal static ConcurrentQueue<SocketConnection.MessageArrivedEventArgs> mobjSync_InteractiveQueue;
        public enum LOGIN_PARAMETERS
        {
            BackOfficeId = 0,
            LoginId = 1,
            Password = 2,
            TransactionPassword = 3,
            Version = 4,
            SecuritiesMaxSequenceId = 5,
            NSEContractsMaxSequenceId = 6,
            NCDEXContractsMaxSequenceId = 7,
            NMCEContractsMaxSequenceId = 8,
            MCXContractsMaxSequenceId = 9,
            BSEContractsMaxSequenceId = 10,
            BSECurrencyContractsMaxSequenceId = 11,
            NSECurrencyContractsMaxSequenceId = 12,
            GBOTContractsMaxSequenceId = 13,
            SIZE = 14
        }
        public Dictionary<string, string> oLoginParamDic = new Dictionary<string, string>();
        public static string broadcastMode = "TCP";
        public static string strTCPConnectMessage = "";
        public static SocketHelper objSocketHelper = null;
        public static bool gblnMDIClosing = false;
        private static string mstrTCPBookRefreshData = null;


        public static string GetDataFromServer(string pstrServlet, string[] pstrParameters, string[] pstrValues, bool pblnSecure = false, Delegate pobjDelegate = null, bool pblnIsOrderBook = false, string pstrOrderNumber = "")
        {
            if ((pstrParameters.Length != pstrValues.Length))
            {
                return "1|Parameter size mismatch|0";
            }

            HTTPHlpr ohttpHelper = new HTTPHlpr();
            ohttpHelper.Method = "Post";
            ohttpHelper.TimeOut = BowConstants.HTTP_TIMEOUT_MIN;
            ohttpHelper.IsExpect100Continue = _100Continues;
            // if ((gobjUtilityConnParameters.PROXYSERVER != null))
            //   {
            //ohttpHelper.ProxyServer = "";
            //ohttpHelper.ProxyPort = 0;
            //        if ((gobjUtilityConnParameters.PROXYUSER != null))
            //        {
            //ohttpHelper.ProxyUserId = "";
            //ohttpHelper.ProxyUserPassword = "";
            //        }
            //    }

            ohttpHelper.Protocol = "http://";
           ohttpHelper.Server = "10.1.101.6:6080//stocks";
            //ohttpHelper.Server = "10.1.101.118:6080//stocks";
            ohttpHelper.Servlet = pstrServlet;
            ohttpHelper.CompressData = false;
            ohttpHelper.OrderNumberForCancel = pstrOrderNumber;
            ohttpHelper.isOrderBook = pblnIsOrderBook;
            pstrParameters[pstrParameters.Length - 1] = UtilityConnParameters.GetInstance.ThickClientParameter;
            pstrValues[pstrValues.Length - 1] = UtilityConnParameters.GetInstance.ThickClientValue;
            pstrParameters[pstrParameters.Length - 2] = UtilityConnParameters.GetInstance.LoginKeyName;
            if (UtilityConnParameters.GetInstance.LoginKeyValue == null)
            {
                pstrValues[pstrValues.Length - 2] = "";
            }
            else
            {
                pstrValues[pstrValues.Length - 2] = UtilityConnParameters.GetInstance.LoginKeyValue;
            }
            ohttpHelper.setValues(pstrParameters, pstrValues);
            try
            {
                if ((pobjDelegate != null))
                {
                    if (pblnIsOrderBook == false)
                    {
                        ohttpHelper.OnHTTPResponseCompleted += pobjDelegate as ResponseReturned;
                        ohttpHelper.sendThread();
                        return "";
                    }
                    else
                    {
                        ohttpHelper.OnHTTPOrderResponseCompleted += pobjDelegate as ResponseReturnedForOrder;
                        ohttpHelper.sendThread();
                        return "";
                    }
                }
                return ohttpHelper.send();
            }
            catch (Exception ex)
            {
                if ((ex.Message).IndexOf("CONNECTION WAS CLOSED") > -1)
                {
                    return "1|Connection with Server broken|0";
                }
                else
                {
                    return "1|" + ex.Message + "|0";
                }
            }


        }

        public static void ConnectToServer()
        {
            if (broadcastMode.Trim().ToUpper() == "TCP")
            {
                UtilityLoginDetails objUtilityLoginDetails = UtilityLoginDetails.GETInstance;
                string SocketCompressionType = "";
                strTCPConnectMessage = "CONNECT" + "|" + objUtilityLoginDetails.UserId + "|" + objUtilityLoginDetails.LoginKeyValue + "|TCP|" + SocketCompressionType + "|||Y";
            }
            if (objSocketHelper == null)
            {
                objSocketHelper = new SocketHelper();
                objSocketHelper.Name = "Broadcast Receiver";
                objSocketHelper.SocketType = "TCP";
                objSocketHelper.ServerAddress = "10.1.101.6:9092";
                //objSocketHelper.ServerAddress = "10.1.101.118:9092";
                objSocketHelper.Compression = "";
                objSocketHelper.ConnectTimeOut = 500;
                objSocketHelper.TimeOut = 300;
                objSocketHelper.PoolFrequency = 10;
                objSocketHelper.MessageArrived += AddMessageToQueue;
                objSocketHelper.TCPClientData = strTCPConnectMessage;
                Thread objBroadcastReceiveThread = null;
                if (objBroadcastReceiveThread == null)
                {
                    objBroadcastReceiveThread = new Thread(objSocketHelper.receiveThread);
                    objBroadcastReceiveThread.Name = "Message Receive Thread";
                    objBroadcastReceiveThread.Priority = ThreadPriority.AboveNormal;
                    objBroadcastReceiveThread.Start();
                }
            }
        }

        public static void SendOpenBroadcastMsg(int pintMarketWatchId)
        {
            string lstrOpenMsg = null;

            string mstrSecurities = null;
            if (mstrSecurities == null)
                mstrSecurities = Globals.GETInstance.MW_Rows_Array;

            lstrOpenMsg = "OPEN|" + UtilityLoginDetails.GETInstance.UserId + "|" + UtilityConnParameters.GetInstance.LoginKeyValue + "|" + pintMarketWatchId + "|" + mstrSecurities;
            SendMessagesOverSocket(ref lstrOpenMsg);

        }

        public static void SendCloseBroadcastMsg(int pintMarketWatchId)
        {
            string lstrOpenMsg = null;

            lstrOpenMsg = "CLOSE|" + UtilityLoginDetails.GETInstance.UserId + "|" + UtilityConnParameters.GetInstance.LoginKeyValue + "|" + pintMarketWatchId ;
            SendMessagesOverSocket(ref lstrOpenMsg);

        }



        public static void SendMessagesOverSocket(ref string pstrMessage)
        {
            if (string.IsNullOrWhiteSpace(pstrMessage) == false)
            {
                //System.Threading.Tasks.Task.Factory.StartNew(Sub(pstrMessage)
                try
                {
                    string lstrDirectMessage = "";
                    // : To be called only if BOLTPlus is set
                    // : Will have to incorporate the SendOpenBroadcastMsg in frmMultilegMW, logic which determines that we keep on reconnecting to the server and sending the open message when ever there is a disconnection.
                    bool lblnIsToBeHandledByBoltPlusWay = false;
                    bool lblIsOpenMessage = false;
                    bool lblWasOpenMessage = false;

                    if (pstrMessage.StartsWith("OPEN|"))
                    {
                        lblWasOpenMessage = true;
                    }

                    if (Globals.GETInstance.gblnDirectBroadcastConfigured)
                    {
                        // AppendActualTokenToMessage(ref pstrMessage, ref lstrDirectMessage, ref lblnIsToBeHandledByBoltPlusWay, ref lblIsOpenMessage);
                    }
                    else
                    {
                        string[] lstrTemp = pstrMessage.Split('|');
                        StringBuilder lobjStringBuilder = new StringBuilder();
                        if (lstrTemp[0].Trim().ToUpper() == "OPENCONCISEINDEX" || lstrTemp[0].Trim().ToUpper() == "CLOSECONCISEINDEX")
                        {
                            lobjStringBuilder.Append(lstrTemp[0]);
                            //Message_Indentifier
                            lobjStringBuilder.Append("|");
                            lobjStringBuilder.Append(lstrTemp[1]);
                            //ID
                            lobjStringBuilder.Append("|");
                            lobjStringBuilder.Append(lstrTemp[2]);
                            // LoginKey
                            lobjStringBuilder.Append("|");

                            string[] lstrKeys = null;
                            if (lblIsOpenMessage)
                            {
                                lstrKeys = lstrTemp[4].Split('~');
                            }
                            else
                            {
                                lstrKeys = lstrTemp[3].Split('~');
                            }
                            for (int lintCount = 0; lintCount <= lstrKeys.Length - 1; lintCount++)
                            {
                                if (string.IsNullOrWhiteSpace(lstrKeys[lintCount]) == false)
                                {
                                    string[] lstrEMT = null;
                                    //: T^E^M
                                    lstrEMT = lstrKeys[lintCount].Split('^');
                                    lobjStringBuilder.Append(lstrEMT[0]);
                                    lobjStringBuilder.Append("~");
                                }
                            }
                            pstrMessage = lobjStringBuilder.ToString();

                        }
                        else if (lstrTemp[0].Trim().ToUpper() == "OPENINDEX" || lstrTemp[0].Trim().ToUpper() == "CLOSEINDEX")
                        {
                            pstrMessage = pstrMessage.Substring(0, pstrMessage.IndexOf("~"));
                        }
                        lblIsOpenMessage = lblWasOpenMessage;
                    }
                    string gstrBroadcastMode = "TCP";
                    //even if there are few scips whose broadacast are meant to be called from server only.
                    if (lblnIsToBeHandledByBoltPlusWay == false || string.IsNullOrWhiteSpace(pstrMessage) == false)
                    {
                        if (MarketWatchConstants.gblnConnectOnHTTP == false)
                        {
                            //: Done so that in all the cases the request should go over TCP and not on UDP
                            //if (gstrBroadcastMode == "UDP" | gstrBroadcastMode == "MULTICAST")
                            //{
                            //    //if ((gobjHBSocket != null))
                            //    //{
                            //    //    gobjHBSocket.send(pstrMessage);
                            //    //    if (lblIsOpenMessage == true)
                            //    //    {
                            //    //        gobjHBSocket.MarketWatchSent = true;
                            //    //    }
                            //    //}
                            //}
                            //else
                            //{
                            //if ((gobjHBSocket != null))
                            //{
                            //    //gobjHBSocket.send(pstrMessage);
                            //    //if (lblIsOpenMessage == true)
                            //    //{
                            //    //    gobjHBSocket.MarketWatchSent = true;
                            //    //}
                            //}
                            //else 
                            if ((objSocketHelper != null))
                            {
                                objSocketHelper.send(pstrMessage);
                                if (lblIsOpenMessage == true)
                                {
                                    objSocketHelper.MarketWatchSent = true;
                                }
                            }
                        }
                        //}
                        //else
                        //{
                        //   // SendMSGOverHTTP(pstrMessage);
                        //}

                        // }
                        //else
                        //{
                        //}
                        // : Will have to use a different socket in case of BOTLPLus

                        //if ((gobjReceiveDirectBroadcastSocket != null) && gobjReceiveDirectBroadcastSocket.ConnectedToServer)
                        //{
                        //    if (string.IsNullOrWhiteSpace(lstrDirectMessage) == false)
                        //    {
                        //        gobjReceiveDirectBroadcastSocket.send(lstrDirectMessage);
                        //    }

                        //    if (lblIsOpenMessage == true)
                        //    {
                        //        gobjReceiveDirectBroadcastSocket.MarketWatchSent = true;
                        //    }
                        //}
                    }
                }
                catch (Exception e)
                {
                    Infrastructure.Logger.WriteLog("Error in SendMessagesOverSocket" + e.Message);
                }
                //End Sub, pstrMsg, Threading.Tasks.TaskCreationOptions.PreferFairness)
            }

        }
        //public static void AppendActualTokenToMessage(ref string pstrFasTradeMessage, ref string pstrDirectBroadcastMessage, ref bool pblnIstobeHandledByBoltPlusWay, ref bool pblnIsOpenMessage)
        //{
        //    //OPEN|" & gobjUtilityLoginDetails.UserId & "|" & .LoginKeyValue & "|" & mlngMarketWatchId & "|" & mstrSecurities
        //    //OPENMBP|2|1825467245-02070020|22^1^4
        //    //OPENCONCISEINDEX|863|1331125512-02068012066063|1~3000
        //    //OPENINDEX|863|2039416771-01211267265262|1

        //    StringBuilder lobjDirectBroadcastMessage = new StringBuilder();
        //    StringBuilder lobjStringBuilder = new StringBuilder();
        //    bool lblnIS_RemoveMessage = false;


        //    if (string.IsNullOrWhiteSpace(pstrFasTradeMessage) == false)
        //    {
        //        string[] lstrKeys = null;
        //        string[] lstrTemp = pstrFasTradeMessage.Split('|');

        //        if (lstrTemp[0].Trim().ToUpper() == "OPEN" || lstrTemp[0].Trim().ToUpper() == "CLOSE" || lstrTemp[0].Trim().ToUpper() == "OPENMBP" || lstrTemp[0].Trim().ToUpper() == "CLOSEMBP" || lstrTemp[0].Trim().ToUpper() == "REMOVE" || lstrTemp[0].Trim().ToUpper() == "OPENCONCISEINDEX" || lstrTemp[0].Trim().ToUpper() == "CLOSECONCISEINDEX" || lstrTemp[0].Trim().ToUpper() == "OPENINDEX" || lstrTemp[0].Trim().ToUpper() == "CLOSEINDEX")
        //        {
        //            if (lstrTemp[0].Trim().ToUpper() == "OPEN")
        //            {
        //                pblnIsOpenMessage = true;
        //            }
        //            else if (lstrTemp[0].Trim().ToUpper() == "REMOVE")
        //            {
        //                lblnIS_RemoveMessage = true;
        //            }

        //            if (lstrTemp.Length > 3)
        //            {
        //                pblnIstobeHandledByBoltPlusWay = true;
        //                if (lstrTemp[0].Trim().ToUpper() != "CLOSE")
        //                {
        //                    if (pblnIsOpenMessage == true || lblnIS_RemoveMessage == true)
        //                    {
        //                        lstrKeys = lstrTemp[4].Split('~');
        //                    }
        //                    else
        //                    {
        //                        lstrKeys = lstrTemp[3].Split('~');
        //                    }
        //                    lobjStringBuilder.Append(lstrTemp[0]);
        //                //Message_Indentifier
        //                lobjStringBuilder.Append("|");
        //                lobjStringBuilder.Append(lstrTemp[1]);
        //                    //ID
        //                    lobjStringBuilder.Append("|");
        //                    lobjStringBuilder.Append(lstrTemp[2]);
        //                    // LoginKey
        //                    lobjStringBuilder.Append("|");
        //                    if (pblnIsOpenMessage == true || lblnIS_RemoveMessage == true)
        //                    {
        //                        lobjStringBuilder.Append(lstrTemp[3]);
        //                        // MarketWatchID
        //                        lobjStringBuilder.Append("|");
        //                    }
        //                    lobjDirectBroadcastMessage.Insert(0, lobjStringBuilder.ToString());
        //                    int lintLengthOfMessageBeforeSeggregation = lobjStringBuilder.Length;
        //                    for (int lintCount = 0; lintCount <= lstrKeys.Length - 1; lintCount++)
        //                    {
        //                        if (string.IsNullOrWhiteSpace(lstrKeys[lintCount]) == false)
        //                        {
        //                            CreateRequestMessageAsPerSpecificRestrictions(ref lstrTemp[0].Trim().ToUpper(), ref lstrKeys[lintCount], lobjStringBuilder, lobjDirectBroadcastMessage);
        //                        }

        //                    }
        //                    //TODO insert FastTrade Message at top of DirectMessage so that open an all can be prefixed
        //                    if (lobjDirectBroadcastMessage.Length > 0)
        //                    {
        //                        //lobjDirectBroadcastMessage.Insert(0, lobjStringBuilder.ToString())
        //                        lobjDirectBroadcastMessage.Remove(lobjDirectBroadcastMessage.Length - 1, 1);
        //                        // : removing the last tilda
        //                    }
        //                    if (lobjStringBuilder.Length > lintLengthOfMessageBeforeSeggregation)
        //                    {
        //                        // : removing the last tilda
        //                        lobjStringBuilder.Remove(lobjStringBuilder.Length - 1, 1);
        //                    }
        //                    else
        //                    {
        //                        // This means that every key was for Direct Broadcast and we do not want to send any thing to the FastTradeBroadcastSocket.
        //                        // TODO we will have to see how dose it affects the Resending logic of MWOpenSent in ConnectToSocketMessage
        //                        lobjStringBuilder.Clear();
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    pstrFasTradeMessage = lobjStringBuilder.ToString();
        //    pstrDirectBroadcastMessage = lobjDirectBroadcastMessage.ToString();
        //}

        //public static void CreateRequestMessageAsPerSpecificRestrictions(ref string pstrMessageType, ref string pstrKey, ref StringBuilder pobjFasTradeMessage, ref StringBuilder pobjDirectBroadcastMessage)
        //{
        //    if (pstrMessageType != "OPENCONCISEINDEX" && pstrMessageType != "CLOSECONCISEINDEX" && pstrMessageType != "OPENINDEX" && pstrMessageType != "CLOSEINDEX")
        //    {
        //        string[] lstrEMT = null;
        //        IScript lobjIScrip = default(IScript);
        //        //: T^E^M
        //        lstrEMT = pstrKey.Split('^');
        //        //AndAlso CInt(lstrEMT(0)) < 10000000) Then ' Reutres Feed are in 1 cr series
        //        if (lstrEMT[1] == BowConstants.EX_BSE_VALUE.ToString() && (lstrEMT[2] == BowConstants.MKT_EQUITY_VALUE.ToString() || lstrEMT[2] == BowConstants.MKT_DERIVATIVE_VALUE.ToString() || lstrEMT[2] == BowConstants.MKT_CURRENCY_VALUE.ToString() || lstrEMT[2] == BowConstants.MKT_COMMODITIES_VALUE.ToString()))
        //        {

        //            lobjIScrip = gobjUtilityScript.GetScript(lstrEMT[1], lstrEMT[0], lstrEMT[2]);
        //            if ((lobjIScrip != null))
        //            {
        //                if (lobjIScrip.MarketId == BowConstants.MKT_EQUITY_VALUE)
        //                {
        //                    pobjDirectBroadcastMessage.Append(pstrKey);
        //                    //Key
        //                    pobjDirectBroadcastMessage.Append("^");
        //                    pobjDirectBroadcastMessage.Append(lobjIScrip.Token);
        //                }
        //                else
        //                {
        //                    try
        //                    {
        //                        if (((ContractBase)lobjIScrip).IsSpreadContract)
        //                        {
        //                            if ((gobjUtilityScript.gobjSpreadScripIDTokenMap != null) && (gobjUtilityScript.gobjSpreadScripIDTokenMap(lobjIScrip.ExchangeId + "^" + lobjIScrip.MarketId) != null))
        //                            {
        //                                long llngScriptID = 0;
        //                                if (gobjUtilityScript.gobjSpreadScripIDTokenMap.Item(lobjIScrip.ExchangeId + "^" + lobjIScrip.MarketId).ContainsKey(lobjIScrip.Token))
        //                                {
        //                                    llngScriptID = gobjUtilityScript.gobjSpreadScripIDTokenMap.Item(lobjIScrip.ExchangeId + "^" + lobjIScrip.MarketId).Item(lobjIScrip.Token);
        //                                }
        //                                pobjDirectBroadcastMessage.Append(pstrKey);
        //                                //Key
        //                                pobjDirectBroadcastMessage.Append("^");
        //                                pobjDirectBroadcastMessage.Append(llngScriptID);
        //                            }
        //                            else
        //                            {
        //                                pobjFasTradeMessage.Append(pstrKey);
        //                                //Key
        //                                pobjFasTradeMessage.Append("~");
        //                                return;
        //                            }
        //                        }
        //                        else
        //                        {
        //                            //If (lstrEMT(2) = Constants.General.MKT_CURRENCY_VALUE AndAlso lobjIScrip.Token >= 10000000) Then
        //                            // pobjFasTradeMessage.Append(pstrKey) 'Key
        //                            // 'pobjFasTradeMessage.Append("^")
        //                            // pobjFasTradeMessage.Append("~")
        //                            //Else
        //                            pobjDirectBroadcastMessage.Append(pstrKey);
        //                            //Key
        //                            pobjDirectBroadcastMessage.Append("^");
        //                            pobjDirectBroadcastMessage.Append(lobjIScrip.Token);
        //                            //End If
        //                        }
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                pobjDirectBroadcastMessage.Append(0);
        //            }
        //            pobjDirectBroadcastMessage.Append('~');
        //        }
        //        else
        //        {
        //            pobjFasTradeMessage.Append(pstrKey);
        //            //Key
        //            //pobjFasTradeMessage.Append("^")
        //            pobjFasTradeMessage.Append('~');
        //        }
        //    }
        //    else
        //    {
        //        string[] lstrEMT = null;
        //        //: T^E^M
        //        if (pstrMessageType == "OPENINDEX" || pstrMessageType == "CLOSEINDEX")
        //        {
        //            pobjDirectBroadcastMessage.Append(pstrKey);
        //            pobjDirectBroadcastMessage.Append("~");
        //        }
        //        else
        //        {
        //            lstrEMT = pstrKey.Split('^');
        //            if ((lstrEMT[1] == BowConstants.EX_BSE_VALUE.ToString() && lstrEMT[2] == BowConstants.MKT_CURRENCY_VALUE.ToString()) ||
        //                lstrEMT[1] == BowConstants.EX_NSE_VALUE.ToString())
        //            {
        //                pobjFasTradeMessage.Append(lstrEMT[0]);
        //                pobjFasTradeMessage.Append("~");
        //            }
        //            else
        //            {
        //                pobjDirectBroadcastMessage.Append(pstrKey);
        //                pobjDirectBroadcastMessage.Append("~");
        //            }
        //        }
        //    }
        //}

        //public static void SendMSGOverHTTP(string pstrMessage)
        //{
        //    ArrayList lstrParameters = new ArrayList();
        //    ArrayList lstrValues = new ArrayList();
        //    GetDataForStocks lobjDataFromStock = default(GetDataForStocks);
        //    if (pstrMessage.Trim().Length > 0)
        //    {
        //        lobjDataFromStock = GetNewDataForStocksObject();
        //        lstrParameters.Add(MarketWatchConstants.MODE_STRING);
        //        lstrValues.Add(MarketWatchConstants.PROXY_STRING);
        //        lstrParameters.Add(MarketWatchConstants.MESSAGECOUNT_STRING);
        //        lstrValues.Add("1");
        //        lstrParameters.Add(MarketWatchConstants.MESSAGECOUNT_STRING);
        //        lstrParameters.Add(MarketWatchConstants.MESSAGE_STRING + "0");
        //        lstrValues.Add(pstrMessage);
        //        //: Login key and Think client
        //        lstrParameters.Add(UtilityConnParameters.GetInstance.LoginKeyName);
        //        lstrValues.Add(UtilityConnParameters.GetInstance.LoginKeyValue);
        //        lstrParameters.Add(UtilityConnParameters.GetInstance.ThickClientParameter);
        //        lstrValues.Add(UtilityConnParameters.GetInstance.ThickClientValue);
        //        GetDataFromServer(MarketWatchConstants.URL_BROADCAST_SERVLET, (string[])lstrParameters.ToArray(typeof(string)), (string[])lstrValues.ToArray(typeof(string)));
        //    }
        //}

        //public static GetDataForStocks GetNewDataForStocksObject()
        //{

        //    try
        //    {
        //        GetDataForStocks lobjGetDataForStocks = GetDataForStocks.GetInstance;
        //        if (gobjUtilityScript == null || gobjUtilityScript.SecureServlets == null)
        //        {
        //            lobjGetDataForStocks.InitializeGetDataForStock(UtilityConnParameters.GetInstance, null);
        //        }
        //        else
        //        {
        //            lobjGetDataForStocks.InitializeGetDataForStock(UtilityConnParameters.GetInstance, gobjUtilityScript.SecureServlets);
        //        }
        //        return lobjGetDataForStocks;
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    return null;
        //}

        private static void AddMessageToQueue(string pstrData)
        {

            if ((pstrData != null) && pstrData.Trim().Length > 0)
            {
                mobjSync_MsgQueue.Enqueue(pstrData);
            }
        }

        public static void ConnectToIntractiveServer()
        {
            try
            {
                UtilityLoginDetails objUtilityLoginDetails = UtilityLoginDetails.GETInstance;
                UtilityConnParameters objUtilityConnParameters = UtilityConnParameters.GetInstance;
                SocketHelper gobjInteractiveSocket = new SocketHelper();
                string SocketCompressionType = "";
                string lstrTCPConnectMessage = "CONNECT" + "|" + objUtilityLoginDetails.UserId + "|" + objUtilityConnParameters.LoginKeyValue + "|TCP|" + SocketCompressionType + "|||Y";
                //& Last Ids
                while (!gblnMDIClosing)
                {
                    try
                    {
                        if (gobjInteractiveSocket == null)
                        {
                            gobjInteractiveSocket = new SocketHelper();
                            gobjInteractiveSocket.Name = "InterActive Receiver";
                            gobjInteractiveSocket.SocketType = "TCP";
                            gobjInteractiveSocket.ServerAddress = "10.1.101.6:9092";
                            gobjInteractiveSocket.Compression = SocketCompressionType;
                            gobjInteractiveSocket.ConnectTimeOut = 500;
                            gobjInteractiveSocket.TimeOut = 100;
                            gobjInteractiveSocket.PoolFrequency = 10;
                            gobjInteractiveSocket.TCPClientData = lstrTCPConnectMessage;
                            gobjInteractiveSocket.MessageArrived -= HandleInteractiveData;
                            gobjInteractiveSocket.MessageArrived += HandleInteractiveData;
                            if (gobjInteractiveThread == null || gobjInteractiveThread.IsAlive == false)
                            {
                                gobjInteractiveThread = new System.Threading.Thread(gobjInteractiveSocket.receiveThread);
                                gobjInteractiveThread.Name = "Interactive Message Send Thread";
                                gobjInteractiveThread.Priority = ThreadPriority.AboveNormal;
                                gobjInteractiveThread.Start();
                            }
                        }
                        else if (gobjInteractiveThread == null || !gobjInteractiveThread.IsAlive)
                        {
                            gobjInteractiveSocket = null;
                            gobjInteractiveSocket = new SocketHelper();
                            gobjInteractiveSocket.Name = "InterActive Receiver";
                            gobjInteractiveSocket.SocketType = "TCP";
                            gobjInteractiveSocket.ServerAddress = "";
                            gobjInteractiveSocket.Compression = SocketCompressionType;
                            gobjInteractiveSocket.ConnectTimeOut = 500;
                            gobjInteractiveSocket.TimeOut = 100;
                            gobjInteractiveSocket.TCPClientData = lstrTCPConnectMessage;
                            //  gobjInteractiveSocket.PoolFrequency = gintSocketPoolFreqBroadcast;
                            gobjInteractiveSocket.MessageArrived -= HandleInteractiveData;
                            gobjInteractiveSocket.MessageArrived += HandleInteractiveData;
                            gobjInteractiveThread = new System.Threading.Thread(gobjInteractiveSocket.receiveThread);
                            gobjInteractiveThread.Name = "Interactive Message Send Thread";
                            gobjInteractiveThread.Priority = ThreadPriority.AboveNormal;
                            gobjInteractiveThread.Start();
                        }
                        System.Threading.Thread.Sleep(1000);
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private static void HandleInteractiveData(string pstrData)
        {
            int lintFoundAt = 0;
            string lstrOneCompleteData = null;
            try
            {
                mstrTCPBookRefreshData += pstrData;
                lintFoundAt = mstrTCPBookRefreshData.IndexOf("/z");

                while ((lintFoundAt >= 0))
                {
                    lstrOneCompleteData = mstrTCPBookRefreshData.Substring(0, lintFoundAt);
                    if (lstrOneCompleteData.Trim().Length > 0)
                    {
                        //         Task.Factory.StartNew(() => { RefreshBookFromTCPData(lstrOneCompleteData); }, null, TaskCreationOptions.PreferFairness);
                    }
                    mstrTCPBookRefreshData = mstrTCPBookRefreshData.Substring(lintFoundAt + 2);
                    lintFoundAt = mstrTCPBookRefreshData.IndexOf("/z");
                }
            }
            catch (Exception innerEx)
            {
                Infrastructure.Logger.WriteLog("Error in Inner HandleInteractiveData : " + innerEx.StackTrace);
                Infrastructure.Logger.WriteLog("Error in Inner HandleInteractiveData : " + innerEx.Message);
            }

        }

        private void RefreshBookFromTCPData(string pstrData)
        {

            try
            {
                if (string.IsNullOrWhiteSpace(pstrData) == false)
                {
                    int lintPositionOfFirstTilda = pstrData.IndexOf("~");
                    if (lintPositionOfFirstTilda > 0)
                    {
                        string lstrMessageType = pstrData.Substring(0, lintPositionOfFirstTilda);
                        if (string.IsNullOrWhiteSpace(lstrMessageType) == false)
                        {
                            string lstrData = pstrData.Substring(lintPositionOfFirstTilda + 1, pstrData.Length - lintPositionOfFirstTilda - 1);
                            //switch (lstrMessageType)
                            //{
                            //    case "7511":
                            //        RefeshOrderBookWithServerResponse(lstrData);
                            //        break;
                            //    case "7611":
                            //        RefeshTradeBookWithServerResponse(lstrData);
                            //        break;
                            //    case "8611":
                            //        RefeshNetPositionWithServerResponse(lstrData);
                            //        break;
                            //}
                        }
                    }
                    else
                    {
                        Infrastructure.Logger.WriteLog("No Tilda found in HandleInteractiveData");
                    }
                }
            }
            catch (Exception ex)
            {
                Infrastructure.Logger.WriteLog("Error in HandleInteractiveData . Exception = " + ex.Message);
                Infrastructure.Logger.WriteLog("Error in HandleInteractiveData . Message = " + pstrData);
            }
        }

        private delegate void del_setsbpHeartBeat();

        private void setsbpHeartBeat()
        {
            //  if (InvokeRequired)
            {
                del_setsbpHeartBeat objdel = setsbpHeartBeat;

                IAsyncResult IAsyncResult = default(IAsyncResult);
                IAsyncResult = objdel.BeginInvoke(null, null);
                while (IAsyncResult.IsCompleted == false)
                {
                    Thread.Sleep(5);
                }
                objdel.EndInvoke(IAsyncResult);
            }
            //  else
            {
                //      sbpHeartBeat.ToolTipText = "Connected to Server";
            }
        }

        public static void MessageReceived(object sender, SocketConnection.MessageArrivedEventArgs pobjMessageArrivedEventArgs)
        {
            if ((pobjMessageArrivedEventArgs.MessageLength > 2))
            {
                //For lintTemp As Integer = 0 To pobjMessageArrivedEventArgs.MessageString.Length - 2
                // Infrastructure.Logger.WriteLog(Asc(pobjMessageArrivedEventArgs.MessageString.Substring(lintTemp, lintTemp + 1].ToString))
                //Next
                //Infrastructure.Logger.WriteLog("Message MessageReceived : -> " & pobjMessageArrivedEventArgs.MessageString & " Time :->" & DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.fffff"))
                switch (pobjMessageArrivedEventArgs.MessageCode)
                {
                    case "60":

                        //        StringBuilder lobjKey = new StringBuilder();
                        //        RecordSplitter lobjRecordSplitter = new RecordSplitter(pobjMessageArrivedEventArgs.MessageString);
                        //        lobjKey.Append(lobjRecordSplitter.getField(0, frmMarketByPrice.MktByPrice.Segment));
                        //        lobjKey.Append("||");
                        //        lobjKey.Append(lobjRecordSplitter.getField(0, frmMarketByPrice.MktByPrice.CommonToken));
                        //        lobjKey.Append("||");
                        //        lobjKey.Append(gobjUtilityEMS.GetExchangeString(lobjRecordSplitter.getField(0, frmMarketByPrice.MktByPrice.Exchange)));

                        //        if (Convert.ToString(gfrmMarketByPrice.grpBSE.Tag].Trim().ToUpper() == lobjKey.ToString().ToUpper || Convert.ToString(gfrmMarketByPrice.grpNSE.Tag].Trim().ToUpper() == lobjKey.ToString.ToUpper)
                        //        {
                        AddMessageToBroadCastQueue(pobjMessageArrivedEventArgs);
                        setExchangeTime(60, pobjMessageArrivedEventArgs);
                        //        }

                        //AddMessageToMBPQueue(pobjMessageArrivedEventArgs);
                        //if (gblnSensexMessageLog == true)
                        //{
                        //    RecordSplitter lobjRH = default(RecordSplitter);
                        //    lobjRH = new RecordSplitter(pobjMessageArrivedEventArgs.MessageString);
                        //    string lstrKeyForWhichBCRecd = null;
                        //    lstrKeyForWhichBCRecd = lobjRH.getField(0, frmMarketByPrice.MktByPrice.CommonToken) + "^" + lobjRH.getField(0, frmMarketByPrice.MktByPrice.Exchange) + "^" + lobjRH.getField(0, frmMarketByPrice.MktByPrice.Segment);
                        //    WriteSensexMessageLog(lstrKeyForWhichBCRecd, pobjMessageArrivedEventArgs.MessageString);
                        //}
                        //if ((gobjfrmmultilegorderEntry != null) && !gobjfrmmultilegorderEntry.IsDisposed && gobjfrmmultilegorderEntry.Visible)
                        //{
                        //    RecordSplitter lobjRH = default(RecordSplitter);
                        //    lobjRH = new RecordSplitter(pobjMessageArrivedEventArgs.MessageString);
                        //    string lstrKeyForWhichBCRecd = null;
                        //    //lstrExchange = lobjRH.getField(5, gfrmMarketByPrice.MktByPrice.Exchange)
                        //    lstrKeyForWhichBCRecd = lobjRH.getField(0, frmMarketByPrice.MktByPrice.CommonToken) + "^" + lobjRH.getField(0, frmMarketByPrice.MktByPrice.Exchange) + "^" + lobjRH.getField(0, frmMarketByPrice.MktByPrice.Segment);
                        //    //MessageBox.Show(lstrKeyForWhichBCRecd)
                        //    if (gobjfrmmultilegorderEntry.mobjTokenList.Contains(lstrKeyForWhichBCRecd.ToString))
                        //    {
                        //        gobjfrmmultilegorderEntry.UpdateTochLine(lobjRH.getField(0, frmMarketByPrice.MktByPrice.Price1 - 6) + "|" + lobjRH.getField(0, frmMarketByPrice.MktByPrice.Price6 - 6), lstrKeyForWhichBCRecd.ToString);
                        //    }
                        //}
                        break;
                    //case "59":
                    //case "70":
                    //case "72":
                    //case "99":
                    //case "11":
                    //case "4141":
                    //    AddMessageToBroadCastQueue(pobjMessageArrivedEventArgs);
                    //    break;
                    //case "69":
                    //    // : For UserAlerts
                    //    if ((gobjUserAlertController != null) && gobjUserAlertController.IS_AlertAddedForScrip(pobjMessageArrivedEventArgs.MessageString) == true)
                    //    {
                    //        gobjUserAlertController.AddBroadcast(pobjMessageArrivedEventArgs.MessageString);
                    //    }
                    //    AddMessageToBroadCastQueue(pobjMessageArrivedEventArgs);
                    //    break;
                    case "61":
                        AddMessageToBroadCastQueue(pobjMessageArrivedEventArgs);
                        setExchangeTime(61, pobjMessageArrivedEventArgs);
                        //if ((mobjfrmActionWatch != null) && !mobjfrmActionWatch.IsDisposed)
                        //{
                        //    mobjfrmActionWatch.CalculateNewHighLow(pobjMessageArrivedEventArgs.MessageString);
                        //}
                        break;
                    //case "74":
                    //    setExchangeTime(74, pobjMessageArrivedEventArgs);
                    //    break;
                    case "75":
                    case "76":
                    case "82":

                        //    Infrastructure.Logger.WriteLog("Message Arrived : -> " + pobjMessageArrivedEventArgs.MessageString, true);

                        RecordSplitter lobjRecordSplitter = new RecordSplitter(pobjMessageArrivedEventArgs.MessageString);
                        if (pobjMessageArrivedEventArgs.MessageCode != "82")
                        {
                            if (pobjMessageArrivedEventArgs.MessageCode == Constants.MessageIdentifiers.MESSAGE_ORDER_STREAM || pobjMessageArrivedEventArgs.MessageCode == Constants.MessageIdentifiers.MESSAGE_TRADE_STREAM)
                            {
                                //: So that the Refresh cache is not populated with unnecessory data.
                                string lstrTransactionCode = lobjRecordSplitter.getField(0, 1);
                                if ((lstrTransactionCode != StockConstants.ORDER_ENTRY_REQUEST.ToString() &&
                                    lstrTransactionCode != StockConstants.ORDER_CANCELLATION_REQUEST.ToString() &&
                                    lstrTransactionCode != StockConstants.ORDER_CANCELLATION_REJECTION.ToString() &&
                                    lstrTransactionCode != StockConstants.ORDER_MIRAGE.ToString()))
                                {
                                    //OrderBowController.ProcessOrderQueue(lobjRecordSplitter);
                                    MemoryManager.OrderQueue.Enqueue(lobjRecordSplitter);
                                }
                                //else
                                //{
                                //    if (Is_ExcelOpen() == true && gblnOpenExcelOrderBook == true)
                                //    {
                                //        if ((gobjSync_ExcelAndChartMsgQueue != null))
                                //        {
                                //            gobjSync_ExcelAndChartMsgQueue.Enqueue(pobjMessageArrivedEventArgs);
                                //        }
                                //    }
                                //}
                            }
                            //else
                            //{
                            //    mstrRefreshData.Enqueue(lobjRecordSplitter);
                            //}

                            ////: Dont insert in case of server side
                            //if ((gblnLDBSelf == true || gblnLDBAll == true))
                            //{
                            //    mstrInsertData.Enqueue(pobjMessageArrivedEventArgs.MessageString);
                            //}
                            return;
                        }
                        //    else
                        //    {
                        //        bool lblnDataQuedFornetpos = false;
                        //        if (lobjRecordSplitter.numberOfRecords > 1)
                        //        {
                        //            if (lobjRecordSplitter.getField(1, 0) == "N")
                        //            {
                        //                mstrNetPositionRefreshData.Enqueue(pobjMessageArrivedEventArgs.MessageString);
                        //                lblnDataQuedFornetpos = true;
                        //            }
                        //        }
                        //        if (lblnDataQuedFornetpos == false)
                        //        {
                        //            mstrNetPositionInsertData.Enqueue(pobjMessageArrivedEventArgs.MessageString);
                        //        }
                        //        //: This code will only run in case of Excel hence we can live with the over head of business logic that is here.
                        //        if (Is_ExcelOpen() == true && gblnOpenExcelNetPosition == true)
                        //        {
                        //            RecordSplitter.RecordHelper lobjRecHelper = default(RecordSplitter.RecordHelper);
                        //            lobjRecHelper = frmListNetPosition.ReCalculateValues(pobjMessageArrivedEventArgs.MessageString, false, 0);
                        //            SocketConnection.MessageArrivedEventArgs lobjMessageEvent = new SocketConnection.MessageArrivedEventArgs(string.Join("|", lobjRecHelper.getRecord(0)));
                        //            if ((gobjSync_ExcelAndChartMsgQueue != null))
                        //            {
                        //                gobjSync_ExcelAndChartMsgQueue.Enqueue(lobjMessageEvent);
                        //            }
                        //        }
                        //    }

                        break;
                    default:
                        Infrastructure.Logger.WriteLog("Message Arrived : -> " + pobjMessageArrivedEventArgs.MessageString, true);
                        //AddMessageToInteractiveQueue(pobjMessageArrivedEventArgs)
                             InteractiveMessageReceived(null, pobjMessageArrivedEventArgs);
                        break;
                }
            }
        }

        private static void setExchangeTime(Int32 pintMsgId, SocketConnection.MessageArrivedEventArgs pobjMessageArrivedEventArgs)
        {
            try
            {
                //  if ((gfrmExchangeTime != null))
                {
                    string[] lstrexchangearray = new string[6];
                    if (pintMsgId == 61 && Globals.GETInstance.gblnIsNowModel == false)
                    {
                        int lintFoundAt = pobjMessageArrivedEventArgs.MessageString.IndexOf("|", 3);
                        string lstrExchangeID = pobjMessageArrivedEventArgs.MessageString.Substring(lintFoundAt + 1, 1);
                        if (lstrExchangeID == BowConstants.EX_NSE_VALUE.ToString() || lstrExchangeID == BowConstants.EX_NCDEX_VALUE.ToString()
                            || lstrExchangeID == BowConstants.EX_NMCE_VALUE.ToString() || lstrExchangeID == BowConstants.EX_DGCX_VALUE.ToString()
                            || lstrExchangeID == BowConstants.EX_USE_VALUE.ToString() || lstrExchangeID == BowConstants.EX_MCX_VALUE.ToString())
                        {
                            string lstrMarket = pobjMessageArrivedEventArgs.MessageString.Substring(lintFoundAt + 3, 1);

                            if (pobjMessageArrivedEventArgs.MessageString.Contains("PM"))
                            {
                                if (!pobjMessageArrivedEventArgs.MessageString.Substring(3, lintFoundAt - 3).Contains("PM"))
                                {
                                    lstrexchangearray[1] = pobjMessageArrivedEventArgs.MessageString.Substring(3, lintFoundAt - 3) + " PM";
                                }
                                else
                                {
                                    lstrexchangearray[1] = pobjMessageArrivedEventArgs.MessageString.Substring(3, lintFoundAt - 3);
                                }
                            }
                            else if (pobjMessageArrivedEventArgs.MessageString.Contains("AM"))
                            {
                                if (!pobjMessageArrivedEventArgs.MessageString.Substring(3, lintFoundAt - 3).Contains("AM"))
                                {
                                    lstrexchangearray[1] = pobjMessageArrivedEventArgs.MessageString.Substring(3, lintFoundAt - 3) + " AM";
                                }
                                else
                                {
                                    lstrexchangearray[1] = pobjMessageArrivedEventArgs.MessageString.Substring(3, lintFoundAt - 3);
                                }
                            }
                            else
                            {
                                if (DateTime.Now.ToString().Contains("AM"))
                                {
                                    lstrexchangearray[1] = pobjMessageArrivedEventArgs.MessageString.Substring(3, lintFoundAt - 3) + " AM";
                                }
                                else
                                {
                                    lstrexchangearray[1] = pobjMessageArrivedEventArgs.MessageString.Substring(3, lintFoundAt - 3) + " PM";
                                }

                            }
                            //if (Math.Abs(DateDiff(DateInterval.Minute, DateTime.Now.TimeOfDay, ((DateTime)lstrexchangearray[1]].TimeOfDay)) > 10)
                            //{
                            //    return;
                            //}
                            lstrexchangearray[2] = lstrExchangeID;
                            lstrexchangearray[3] = lstrMarket;
                        }
                        else
                        {
                            return;
                        }
                    }
                    else if (pintMsgId == 74)
                    {
                        lstrexchangearray = pobjMessageArrivedEventArgs.MessageString.Split('|');
                        if (Globals.GETInstance.gblnDirectBroadcastConfigured && Convert.ToUInt32(lstrexchangearray[2]) != 99)
                        {
                            return;
                        }
                    }
                    else
                    {
                        return;
                    }
                    //   gfrmExchangeTime.CtlTime1.StartTimeDisplay("Time", Convert.ToDateTime(lstrexchangearray[1]].ToLongTimeString());
                    //Infrastructure.Logger.WriteLog("Time:-" + pobjMessageArrivedEventArgs.MessageString)
                }
            }
            catch (Exception ex)
            {
                Infrastructure.Logger.WriteLog("Error in setExchangeTime." + ex.Message);
            }
        }

        private static void AddMessageToBroadCastQueue(SocketConnection.MessageArrivedEventArgs pobjMessageArrivedEventArgs)
        {
            if ((MarketWatchConstants.mobjSync_BroadcastMsgQueue != null))
            {
                if (Globals.GETInstance.gblnBroadcastMessageLog == true)
                {
                    Infrastructure.Logger.WriteLog(pobjMessageArrivedEventArgs.MessageString);
                }
                MarketWatchConstants.mobjSync_BroadcastMsgQueue.Enqueue(pobjMessageArrivedEventArgs);
            }
        }

        public static void BroadcastMessagePump()
        {
            Stopwatch lobjStopWatch = new Stopwatch();
            try
            {
                //Dim lintDegreeOfParallism As Integer = 2
                int lintMessageThreasholdValue = 1000;
                // : Assuming that within 30 seconds the queue's bottel neck would be cleared; if not then we need to bring in an additional thread.
                int lintCheckTimeThreasholdValueInSeconds = 60;
                SocketConnection.MessageArrivedEventArgs lobjMessageArrivedEventArgs = default(SocketConnection.MessageArrivedEventArgs);
                while (true)
                {
                    if (gblnMDIClosing == true)
                    {
                        break; // TODO: might not be correct. Was : Exit While
                    }

                    if ((MarketWatchConstants.mobjSync_BroadcastMsgQueue != null))
                    {
                        while ((MarketWatchConstants.mobjSync_BroadcastMsgQueue.IsEmpty == false))
                        {
                            //If mobjSync_BroadcastMsgQueue.Count > lintDegreeOfParallism Then
                            // Try
                            // Dim lobjTasks(lintDegreeOfParallism - 1) As Task
                            // For lintTemp As Integer = 0 To lintDegreeOfParallism - 1
                            // lobjTasks(lintTemp) = Task.Factory.StartNew(Sub()
                            // Try
                            // Dim lobjMessageArrivedEventArgs As SocketConnection.MessageArrivedEventArgs
                            // lobjMessageArrivedEventArgs = mobjSync_BroadcastMsgQueue.Dequeue
                            // If Not lobjMessageArrivedEventArgs Is Nothing Then
                            // BroadcastMessageReceived(Nothing, lobjMessageArrivedEventArgs)
                            // End If
                            // Catch ex As Exception
                            // End Try
                            // End Sub)
                            // Next
                            // 'Task.WaitAll(lobjTasks)
                            // Catch ex As Exception
                            // Trace.WriteLine("Error in Broadcast Pump while sliting the Dequeue process into Tasks." & ex.Message)
                            // End Try
                            //Else
                            try
                            {
                                lobjMessageArrivedEventArgs = null;
                                MarketWatchConstants.mobjSync_BroadcastMsgQueue.TryDequeue(out lobjMessageArrivedEventArgs);

                                string[] larrstrCol = lobjMessageArrivedEventArgs.MessageString.Split('|');
                                ScripDetails objscrip = new ScripDetails();
                                if (larrstrCol[0] == "60")
                                {
                                    //  if (lstrReturn.Substring(0, 1) == BowConstants.SUCCESS_FLAG)


                                    // IAsyncResult IAsyncResult = default(IAsyncResult);
                                    //  lock (lobjArgs)

                                    ArrayList lobjArgs = new ArrayList(2);
                                    lock (lobjArgs)
                                    {
                                        RecordSplitter lobjRecordHelper = new RecordSplitter("0|~0|1|54~" + lobjMessageArrivedEventArgs.MessageString);
                                        lobjArgs.Add(lobjRecordHelper);
                                        lobjArgs.Add(false);

                                        objscrip = MarketWatchVM.UpdateData(lobjArgs, MarketWatchConstants.lstrTokenString);
                                        MarketWatchVM.objBroadCastProcessor_OnBroadCastRecievedNew(objscrip);


                                        BestFiveVM.OnBroadCastRecieved(objscrip.ScriptCode_BseToken_NseToken, objscrip);
                                    }


                                }
                                else if (larrstrCol[0] == "61")
                                {
                                    objscrip = MainWindowVM.UpdateScripDataFromMemory(larrstrCol);
                                    MarketWatchVM.objBroadCastProcessor_OnBroadCastRecievedNew(objscrip);
                                }
                                else if (larrstrCol != null && larrstrCol[0] == "69")
                                {
                                    Model.IdicesDetailsMain objIndices = new Model.IdicesDetailsMain();
                                    objIndices = MainWindowVM.UpdateIndicesDataFromMemory(larrstrCol);
                                    if (indicesBroadCastBow != null)
                                    {
                                        indicesBroadCastBow(objIndices);
                                    }

                                }
                             




                                // Model.ScripDetails s;
                                //s.ScriptCode_BseToken_NseToken = lobjMessageArrivedEventArgs[4];
                                //MarketWatchVM.objBroadCastProcessor_OnBroadCastRecievedNew(s);
                                //if ((lobjMessageArrivedEventArgs != null))
                                //{
                                //    BroadcastMessageReceived(null, lobjMessageArrivedEventArgs);
                                //}

                                //if (mobjSync_BroadcastMsgQueue.Count > lintMessageThreasholdValue)
                                //{
                                //    if (lobjStopWatch.IsRunning == true)
                                //    {
                                //        if (lobjStopWatch.ElapsedMilliseconds / 1000 >= lintCheckTimeThreasholdValueInSeconds)
                                //        {
                                //            StartAdditionalBroadcastPump();
                                //            lobjStopWatch.Restart();
                                //        }
                                //    }
                                //    else
                                //    {
                                //        lobjStopWatch.Start();
                                //    }
                                //}


                            }
                            catch (AggregateException ex)
                            {
                                foreach (Exception e in ex.InnerExceptions)
                                {
                                    Infrastructure.Logger.WriteLog("Error in one of the Broadcast in BroadcastMessagePump" + ex.Message);
                                    Infrastructure.Logger.WriteLog("Error in one of the Broadcast in BroadcastMessagePump" + ex.StackTrace);
                                }
                            }
                            catch (InvalidOperationException ex)
                            {
                                if (ex.Message == "Queue empty.")
                                {
                                    // do nothing 
                                }
                                else
                                {
                                    Infrastructure.Logger.WriteLog("Error in one of the Broadcast in BroadcastMessagePump" + ex.Message);
                                    Infrastructure.Logger.WriteLog("Error in one of the Broadcast in BroadcastMessagePump" + ex.StackTrace);
                                }
                            }
                            catch (Exception ex)
                            {
                                Infrastructure.Logger.WriteLog("Error in one of the Broadcast in BroadcastMessagePump" + ex.Message);
                                Infrastructure.Logger.WriteLog("Error in one of the Broadcast in BroadcastMessagePump" + ex.StackTrace);
                            }
                            //End If
                        }
                    }
                    Thread.Sleep(5);
                }
            }
            catch (Exception ex)
            {
                if ((lobjStopWatch != null))
                {
                    lobjStopWatch.Stop();
                    lobjStopWatch = null;
                }
                Infrastructure.Logger.WriteLog("Error in BroadcastMessagePump : " + ex.Message);
                Infrastructure.Logger.WriteLog("Error in BroadcastMessagePump : " + ex.StackTrace);
            }
        }

        public static void ManageAsynchronousBroadcast()
        {
            try
            {
                string lstrSocketData = null;
                int lintFoundAt = 0;
                string lstrOneCompleteData = null;
                SocketConnection.MessageArrivedEventArgs lobjMessageArrivedEventArgs = default(SocketConnection.MessageArrivedEventArgs);
                string lstrCurrentMsg = null;
                while (true)
                {
                    //if (gblnMDIClosing == true)
                    //{
                    //    break; // TODO: might not be correct. Was : Exit While
                    //}
                    if ((SettingsManager.mobjSync_MsgQueue != null))
                    {
                        while (SettingsManager.mobjSync_MsgQueue.IsEmpty == false)
                        {
                            try
                            {
                                lintFoundAt = 0;
                                SettingsManager.mobjSync_MsgQueue.TryDequeue(out lstrCurrentMsg);
                                if (string.IsNullOrWhiteSpace(lstrCurrentMsg) == false)
                                {
                                    lstrSocketData += lstrCurrentMsg;
                                    lintFoundAt = lstrSocketData.IndexOf("/z");

                                    while ((lintFoundAt >= 0))
                                    {
                                        lstrOneCompleteData = lstrSocketData.Substring(0, lintFoundAt);

                                        if (lstrOneCompleteData.Trim().Length > 0)
                                        {
                                            lobjMessageArrivedEventArgs = new SocketConnection.MessageArrivedEventArgs(lstrOneCompleteData);
                                            if ((lobjMessageArrivedEventArgs != null))
                                            {
                                                SettingsManager.MessageReceived(null, lobjMessageArrivedEventArgs);
                                            }
                                        }
                                        lstrSocketData = lstrSocketData.Substring(lintFoundAt + 2);
                                        lintFoundAt = lstrSocketData.IndexOf("/z");
                                    }
                                }
                            }
                            catch (Exception innerEx)
                            {
                                Infrastructure.Logger.WriteLog("Error in Inner ManageAsynchronousBroadcast : " + innerEx.StackTrace);
                                Infrastructure.Logger.WriteLog("Error in Inner ManageAsynchronousBroadcast : " + innerEx.Message);
                            }
                        }
                    }
                    Thread.Sleep(5);
                }
            }
            catch (Exception ex)
            {
                Infrastructure.Logger.WriteLog("Error in ManageAsynchronousBroadcast : " + ex.StackTrace);
                Infrastructure.Logger.WriteLog("Error in ManageAsynchronousBroadcast : " + ex.Message);
            }
        }


        private static void StartAdditionalBroadcastPump()
        {
            System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                gobjBroadCastMessagePumpAdditionalThreads[gobjBroadCastMessagePumpAdditionalThreads.Length - 1] = new Thread(BroadcastMessagePumpAdditional);
                gobjBroadCastMessagePumpAdditionalThreads[gobjBroadCastMessagePumpAdditionalThreads.Length - 1].Name = "BroadcastMessagePumpAdditional" + gobjBroadCastMessagePumpAdditionalThreads.Length;
                gobjBroadCastMessagePumpAdditionalThreads[gobjBroadCastMessagePumpAdditionalThreads.Length - 1].Priority = ThreadPriority.AboveNormal;
                gobjBroadCastMessagePumpAdditionalThreads[gobjBroadCastMessagePumpAdditionalThreads.Length - 1].Start(gobjBroadCastMessagePumpAdditionalThreads.Length);
                Array.Resize(ref gobjBroadCastMessagePumpAdditionalThreads, gobjBroadCastMessagePumpAdditionalThreads.Length + 1);
            });
        }

        public static void BroadcastMessagePumpAdditional(object pobjArgs)
        {
            int lintAdditionalBroadCastPumpThreadNo = Convert.ToInt32(pobjArgs);
            try
            {
                while (true)
                {
                    if (gblnMDIClosing == true)
                    {
                        break; // TODO: might not be correct. Was : Exit While
                    }
                    if ((MarketWatchConstants.mobjSync_BroadcastMsgQueue != null))
                    {
                        try
                        {
                            while ((MarketWatchConstants.mobjSync_BroadcastMsgQueue.IsEmpty == false))
                            {
                                try
                                {
                                    SocketConnection.MessageArrivedEventArgs lobjMessageArrivedEventArgs = null;
                                    MarketWatchConstants.mobjSync_BroadcastMsgQueue.TryDequeue(out lobjMessageArrivedEventArgs);
                                    if ((lobjMessageArrivedEventArgs != null))
                                    {
                                        BroadcastMessageReceived(null, lobjMessageArrivedEventArgs);
                                    }


                                }
                                catch (InvalidOperationException ex)
                                {
                                    if (ex.Message == "Queue Empty")
                                    {
                                        // do nothing 
                                    }
                                    else
                                    {
                                        Infrastructure.Logger.WriteLog("Error in one of the Broadcast in BroadcastMessagePumpAdditional_" + lintAdditionalBroadCastPumpThreadNo + " :" + ex.Message);
                                        Infrastructure.Logger.WriteLog("Error in one of the Broadcast in BroadcastMessagePumpAdditional_" + lintAdditionalBroadCastPumpThreadNo + " :" + ex.StackTrace);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Infrastructure.Logger.WriteLog("Error in one of the Broadcast in BroadcastMessagePumpAdditional_" + lintAdditionalBroadCastPumpThreadNo + " :" + ex.Message);
                                    Infrastructure.Logger.WriteLog("Error in one of the Broadcast in BroadcastMessagePumpAdditional_" + lintAdditionalBroadCastPumpThreadNo + " :" + ex.StackTrace);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                    Thread.Sleep(5);
                }
            }
            catch (Exception ex)
            {
                Infrastructure.Logger.WriteLog("Error in BroadcastMessagePumpAdditional_" + lintAdditionalBroadCastPumpThreadNo + " :" + ex.Message);
                Infrastructure.Logger.WriteLog("Error in BroadcastMessagePumpAdditional_" + lintAdditionalBroadCastPumpThreadNo + " :" + ex.StackTrace);
            }
        }

        public static void BroadcastMessageReceived(Object sender, MessageArrivedEventArgs pobjMessageArrivedEventArgs)
        {
            RecordSplitter lobjRecordHelper = default(RecordSplitter);
            long llngUserId = 0;
            IAsyncResult IAsyncResult = default(IAsyncResult);
            StringBuilder lobjStringBuilder = default(StringBuilder);
            //Dim lobjStopWatch As New Stopwatch
            try
            {
                //lobjStopWatch.Start()
                if ((pobjMessageArrivedEventArgs.MessageLength > 0))
                {
                    if (!(pobjMessageArrivedEventArgs.MessageString.StartsWith("Dummy")))
                    {
                        //: for skipping the msg if not for ldb user
                        string lstrMsgId = pobjMessageArrivedEventArgs.MessageCode;
                        if (lstrMsgId.Length > 0)
                        {
                            //Select Case lstrMsgId
                            // Case "75", "76", "78", "80", "82", "109", "110"
                            // REM: if the user is not an ldb user then, no need to parse the msg received
                            // If gblnLDBSelf = False Then
                            // Exit Sub
                            // End If
                            //End Select

                            lobjRecordHelper = new RecordSplitter(pobjMessageArrivedEventArgs.MessageString);
                            if ((lstrMsgId == MessageIdentifiers.MESSAGE_MARKET_WATCH_TEXT) || (lstrMsgId == MessageIdentifiers.MESSAGE_OPEN_INTEREST_TEXT))
                            {
                                //:
                                string lstrMessage = null;
                                string lstrInitialMessage = null;
                                lstrInitialMessage = pobjMessageArrivedEventArgs.MessageString;
                                //if (Globals.GETInstance.gblnUseConsizedMWMsg == true)
                                //{
                                //    if (lstrMsgId == Constants.MessageIdentifiers.MESSAGE_OPEN_INTEREST_TEXT || gblnConnectOnHTTP == true)
                                //    {
                                //        lstrMessage = lstrInitialMessage;
                                //    }
                                //    else
                                //    {
                                //        lstrMessage = MessageConverter.ConstructFull61Message(lstrInitialMessage);
                                //    }
                                //}
                                //else
                                //{
                                lstrMessage = lstrInitialMessage;
                                //}
                                //: 61
                                mintActualMWOpen = 0;
                                ArrayList lobjExcelPaintMessage = new ArrayList();
                                string lstrExcelPaintMessage = null;

                                lobjStringBuilder = new StringBuilder();
                                lobjStringBuilder.Append(lobjRecordHelper.getField(0, 4)).Append("^");
                                lobjStringBuilder.Append(lobjRecordHelper.getField(0, 2)).Append("^");
                                lobjStringBuilder.Append(lobjRecordHelper.getField(0, 3));

                                try
                                {
                                    // for (int intCount = 0; intCount <= gfrmMarketWatch.Count - 1; intCount++)
                                    //{
                                    //    // if ((gfrmMarketWatch(intCount) != null))
                                    //    {
                                    //        // if (!gfrmMarketWatch(intCount].IsDisposed)
                                    //        {
                                    //            //:changes made for ARBITRAGE marketwatch.
                                    //            //   CheckForIllegalCrossThreadCalls = false;
                                    MarketWatchVM objMarketWatchForm = new MarketWatchVM();
                                    //            //if (objMarketWatchForm.Loaded == true && objMarketWatchForm.CheckToken(lobjStringBuilder.ToString) == true)
                                    //            {
                                    string[] lobjMWRefColl = default(string[]);
                                    objMarketWatchForm.MessageArrived(lstrMessage);
                                    mintActualMWOpen += 1;
                                    //if ((lobjMWRefColl != null) && Is_ExcelOpen() == true)
                                    //{
                                    //    for (int lintTemp = 0; lintTemp <= lobjMWRefColl.Count - 1; lintTemp++)
                                    //    {
                                    //        lstrExcelPaintMessage = objMarketWatchForm.GetExcelPaintMessage((MultiLegMW.MWRef)lobjMWRefColl.Item[lintTemp]);
                                    //        if (string.IsNullOrWhiteSpace(lstrExcelPaintMessage) == false)
                                    //        {
                                    //            lobjExcelPaintMessage.Add(lstrExcelPaintMessage);
                                    //        }
                                    //    }
                                    //                //}
                                    //            }
                                    //        }
                                    //    }
                                    //    //:For checking actual no of MW are open
                                    //    //if (mintActualMWOpen == gintNoOfMWOpen)
                                    //    //    break; // TODO: might not be correct. Was : Exit For
                                    //}
                                    //Parallel.For(0, gfrmMarketWatch.Count - 1, Sub(intCount, pobjState)
                                    // If Not gfrmMarketWatch(intCount) Is Nothing Then
                                    // If Not gfrmMarketWatch(intCount].IsDisposed Then
                                    // REM:changes made for ARBITRAGE marketwatch.
                                    // CheckForIllegalCrossThreadCalls = False
                                    // Dim objMarketWatchForm As frmMultiLegMW = gfrmMarketWatch(intCount)
                                    // If objMarketWatchForm.Loaded = True AndAlso objMarketWatchForm.CheckToken(lobjStringBuilder.ToString) = True Then
                                    // Dim lobjMWRefColl As ArrayList
                                    // lobjMWRefColl = objMarketWatchForm.MessageArrived(lstrMessage)
                                    // mintActualMWOpen += 1
                                    // If Not lobjMWRefColl Is Nothing AndAlso Is_ExcelOpen() = True Then
                                    // For lintTemp As Integer = 0 To lobjMWRefColl.Count - 1
                                    // lstrExcelPaintMessage = objMarketWatchForm.GetExcelPaintMessage(CType(lobjMWRefColl.Item(lintTemp), MultiLegMW.MWRef))
                                    // If String.IsNullOrWhiteSpace(lstrExcelPaintMessage) = False Then
                                    // lobjExcelPaintMessage.Add(lstrExcelPaintMessage)
                                    // End If
                                    // Next
                                    // End If
                                    // End If
                                    // End If
                                    // End If
                                    // REM:For checking actual no of MW are open
                                    // If mintActualMWOpen = gintNoOfMWOpen Then pobjState.Break()
                                    // End Sub)

                                }
                                catch (Exception ex)
                                {
                                }

                                //if (Is_ExcelOpen() == true)
                                //{
                                //    for (int lintCount = 0; lintCount <= lobjExcelPaintMessage.Count - 1; lintCount++)
                                //    {
                                //        gobjSync_ExcelAndChartMsgQueue.Enqueue(new SocketConnection.MessageArrivedEventArgs(lobjExcelPaintMessage.Item(lintCount)));
                                //    }
                                //}
                                //: If User is Ldb User, Store the LTP in local database only for 61 msg.
                                //   if (lstrMsgId == Constants.MessageIdentifiers.MESSAGE_MARKET_WATCH_TEXT)
                                //{
                                //    if (gblnLDBSelf || gblnLDBAll)
                                //    {
                                //        mstrLTPData.Enqueue(lstrMessage);
                                //    }
                                //    else
                                //{
                                //    StringBuilder lstrStringBuilder = new StringBuilder();

                                //    string lstrLTP = lobjRecordHelper.getField(0, BROADCAST_MSG_NEW.LastTradePrice);
                                //    string lstrFutureLtp = lobjRecordHelper.getField(0, BROADCAST_MSG_NEW.FutureLTP);
                                //    string lstrUnderleingLtp = lobjRecordHelper.getField(0, BROADCAST_MSG_NEW.UnderlyingLTP);

                                //if ((!string.IsNullOrEmpty(lstrLTP) && lstrLTP.Trim(].Length > 0) || (!string.IsNullOrEmpty(lstrFutureLtp) && lstrFutureLtp.Trim(].Length > 0) || (!string.IsNullOrEmpty(lstrUnderleingLtp) && lstrUnderleingLtp.Trim(].Length > 0))
                                {
                                    //lstrStringBuilder.Append(lobjRecordHelper.getField(0, BROADCAST_MSG_NEW.Token));
                                    //lstrStringBuilder.Append("-");
                                    //lstrStringBuilder.Append(lobjRecordHelper.getField(0, BROADCAST_MSG_NEW.Source));
                                    //lstrStringBuilder.Append("-");
                                    //lstrStringBuilder.Append(lobjRecordHelper.getField(0, BROADCAST_MSG_NEW.Market));

                                    //if ((gfrmOptionAid != null))
                                    //{
                                    //    if (frmOptionAid.CurrentTokenString.Contains(lobjStringBuilder.ToString()) || frmOptionAid.CurrentEQTokenString.Contains(lobjStringBuilder.ToString()) || frmOptionAid.CurrentFAOTokenString.Contains(lobjStringBuilder.ToString()))
                                    //    {
                                    //        gfrmOptionAid.MessageArrived(lstrMessage);
                                    //    }
                                    //}

                                    //if (string.IsNullOrWhiteSpace(lstrLTP) == false)
                                    //{
                                    //    LDB.CurrentMarketByPrice.structLTPValues lobjLTPStruct = default(LDB.CurrentMarketByPrice.structLTPValues);
                                    //    lobjLTPStruct.LTP = lstrLTP;
                                    //    if (string.IsNullOrWhiteSpace(lobjRecordHelper.getField(0, BROADCAST_MSG_NEW.ClosePrice)) == false)
                                    //    {
                                    //        lobjLTPStruct.ClosePrice = lobjRecordHelper.getField(0, BROADCAST_MSG_NEW.ClosePrice);
                                    //    }

                                    //    if (LDB.CurrentMarketByPrice.mobjLTPDataHash.ContainsKey(lstrStringBuilder.ToString(].Trim) == false)
                                    //    {
                                    //        LDB.CurrentMarketByPrice.mobjLTPDataHash.Add(lstrStringBuilder.ToString, lobjLTPStruct);
                                    //    }
                                    //    else
                                    //    {
                                    //        LDB.CurrentMarketByPrice.mobjLTPDataHash.Item(lstrStringBuilder.ToString) = lobjLTPStruct;
                                    //    }
                                    //}
                                }
                                //: OFS
                                //if ((gfrmOFS != null) && gfrmOFS.Visible == true)
                                //{
                                //    gfrmOFS.MessageArrived(lstrMessage);
                                //}
                            }
                        }
                        //if (lstrMsgId == Constants.MessageIdentifiers.MESSAGE_OPEN_INTEREST_TEXT)
                        //{
                        //    if ((gfrmMarketByPrice != null))
                        //    {
                        //        if (gfrmMarketByPrice.Visible == true)
                        //        {
                        //            //gfrmMarketByPrice.UpdateMBP(ConvertOIMessageToMBPMessageStrunture(pobjMessageArrivedEventArgs.MessageString))
                        //            gfrmMarketByPrice.UpdateOpenInterest(pobjMessageArrivedEventArgs.MessageString);
                        //        }
                        //    }
                        //}
                        //lstrMessage = null;
                        //lstrInitialMessage = null;
                        //lobjStopWatch.Stop()
                        //Infrastructure.Logger.WriteLog("One Single MSG All MW Mill" & lobjStopWatch.ElapsedMilliseconds & " Ticks " & lobjStopWatch.ElapsedTicks)
                    }
                    //            else if ((lstrMsgId == "99"))
                    //            {
                    //                lobjRecordHelper = new RecordSplitter.RecordHelper(pobjMessageArrivedEventArgs.MessageString);
                    //                gobjUtilityLoginDetails.LoginDone = false;
                    //                llngUserId = Convert.ToInt64(lobjRecordHelper.getField(0, 1));
                    //                if ((llngUserId == gobjUtilityLoginDetails.UserId))
                    //                {
                    //                    LogOutDelegate lobjLogOutDelegate = new LogOutDelegate(LogOut);
                    //                    IAsyncResult = this.BeginInvoke(lobjLogOutDelegate, new object[] {
                    //                Convert.ToInt32(LOG_OUT.AFTER_LOGIN_EXIT_ELSE_LOGIN),
                    //                lobjRecordHelper.getField(0, 3) + ". The Application is shutting down.",
                    //                true
                    //            });
                    //                    while (IAsyncResult.IsCompleted == false)
                    //                    {
                    //                        Threading.Thread.Sleep(5);
                    //                    }
                    //                    this.EndInvoke(IAsyncResult);
                    //                    IAsyncResult = null;
                    //                    lobjLogOutDelegate = null;
                    //                    //LogOut(LOG_OUT.AFTER_LOGIN_EXIT_ELSE_LOGIN, lobjRecordHelper.getField(0, 2) & ". The Application is shutting down.", True)
                    //                }
                    //            }
                    //            else if (lstrMsgId == "11")
                    //            {
                    //                //: Skipping the first 6 characters : 11|61|
                    //                MessageConverter.SetNew61MsgStructureAsCurrent(pobjMessageArrivedEventArgs.MessageString.Substring(6, pobjMessageArrivedEventArgs.MessageString.Trim().Length - 6));
                    //            }
                    //            else if (lstrMsgId == "4141")
                    //            {
                    //                //Auction MBP Arrived. Update the MBP form if it exists.
                    //                if ((gfrmAuctionMBP != null))
                    //                {
                    //                    if (gfrmAuctionMBP.Visible == true)
                    //                    {
                    //                        RecordSplitter.RecordHelper lobjRH = default(RecordSplitter.RecordHelper);
                    //                        lobjRH = new RecordSplitter.RecordHelper(pobjMessageArrivedEventArgs.MessageString);
                    //                        string lstrKeyForWhichBCRecd = null;
                    //                        //lstrExchange = lobjRH.getField(5, gfrmMarketByPrice.MktByPrice.Exchange)
                    //                        lstrKeyForWhichBCRecd = lobjRH.getField(0, frmAuctionMBP.AuctionMktByPrice1.MARKET) + "||" + lobjRH.getField(0, frmAuctionMBP.AuctionMktByPrice1.TOKEN) + "||" + gobjUtilityEMS.GetExchangeString(lobjRH.getField(0, frmAuctionMBP.AuctionMktByPrice1.SOURCE));
                    //                        if (Convert.ToString(gfrmAuctionMBP.grpBSE.Tag].Trim().ToUpper() == lstrKeyForWhichBCRecd.Trim().ToUpper)
                    //                        {
                    //                            gfrmAuctionMBP.UpdateMBP(pobjMessageArrivedEventArgs.MessageString);
                    //                        }
                    //                    }
                    //                }
                    //            }
                    //            else if ((lstrMsgId == Constants.MessageIdentifiers.MESSAGE_MARKET_BY_PRICE_TEXT))
                    //            {
                    //                //lobjStopWatch.Start()
                    //                //MBP Arrived. Update the MBP form if it exists.
                    //                if ((gfrmMarketByPrice != null))
                    //                {
                    //                    if (gfrmMarketByPrice.Visible == true)
                    //                    {
                    //                        RecordSplitter lobjRH = default(RecordSplitter);
                    //                        lobjRH = new RecordSplitter.RecordHelper(pobjMessageArrivedEventArgs.MessageString);
                    //                        string lstrKeyForWhichBCRecd = null;
                    //                        //lstrExchange = lobjRH.getField(5, gfrmMarketByPrice.MktByPrice.Exchange)
                    //                        lstrKeyForWhichBCRecd = lobjRH.getField(0, frmMarketByPrice.MktByPrice.Segment) + "||" + lobjRH.getField(0, frmMarketByPrice.MktByPrice.CommonToken) + "||" + gobjUtilityEMS.GetExchangeString(lobjRH.getField(0, frmMarketByPrice.MktByPrice.Exchange));
                    //                        if (Convert.ToString(gfrmMarketByPrice.grpBSE.Tag].Trim().ToUpper() == lstrKeyForWhichBCRecd.Trim().ToUpper | Convert.ToString(gfrmMarketByPrice.grpNSE.Tag].Trim().ToUpper() == lstrKeyForWhichBCRecd.Trim().ToUpper)
                    //                        {
                    //                            gfrmMarketByPrice.UpdateMBP(pobjMessageArrivedEventArgs.MessageString);
                    //                        }
                    //                    }
                    //                }

                    //                // Dim lobjRecordSplitter As New RecordSplitter.RecordHelper(pobjMessageArrivedEventArgs.MessageString)
                    //                // If gblnLDBSelf OrElse gblnLDBAll Then
                    //                // 'lobjUpdateLTPDelegateMBP = New MessageArrivedDelegate(AddressOf UpdateLTPinDatabase)
                    //                // 'Me.Invoke(lobjUpdateLTPDelegateMBP, New String() {ConvertMBPMessageToMWMessageStructure(pobjMessageArrivedEventArgs.MessageString)})
                    //                // 'UpdateLTPinDatabase(ConvertMBPMessageToMWMessageStructure(pobjMessageArrivedEventArgs.MessageString))
                    //                // mstrLTPData.Enqueue(ConvertMBPMessageToMWMessageStructure(pobjMessageArrivedEventArgs.MessageString))
                    //                // Else
                    //                // Dim lstrKey As String
                    //                // Dim lstrLTP As String
                    //                // Dim lstrStringBuilder As New StringBuilder

                    //                // lstrStringBuilder.Append(lobjRecordSplitter.getField(0, frmMarketByPrice.MktByPrice.CommonToken))
                    //                // lstrStringBuilder.Append("-")
                    //                // lstrStringBuilder.Append(lobjRecordSplitter.getField(0, frmMarketByPrice.MktByPrice.Exchange))
                    //                // lstrStringBuilder.Append("-")
                    //                // lstrStringBuilder.Append(lobjRecordSplitter.getField(0, frmMarketByPrice.MktByPrice.Segment))
                    //                // lstrKey = lstrStringBuilder.ToString
                    //                // lstrLTP = lobjRecordSplitter.getField(0, frmMarketByPrice.MktByPrice.LastTradePrice - frmMarketByPrice.DISPLACEMENT_UPDATE)
                    //                // If Not lstrLTP Is Nothing AndAlso lstrLTP.Trim().Length > 0 Then
                    //                // If LDB.CurrentMarketByPrice.mobjLTPDataHash.Contains(lstrKey.Trim) = False Then
                    //                // LDB.CurrentMarketByPrice.mobjLTPDataHash.Add(lstrKey, lstrLTP)
                    //                // Else
                    //                // LDB.CurrentMarketByPrice.mobjLTPDataHash.Item(lstrKey) = lstrLTP
                    //                // End If
                    //                // 'REM:refresh CMP and M2M values in Netposition
                    //                // 'If gblnRefreshMTM = True OrElse gblnLDBSelf OrElse gblnLDBAll Then RefreshMTMinNetposition(pobjMessageArrivedEventArgs.MessageString)
                    //                // 'REM:Refresh online M2M and PL
                    //                // 'If gblnOnlineMTMPL = True AndAlso Not gfrmOnlineMTMPL Is Nothing AndAlso gfrmOnlineMTMPL.Visible = True Then gfrmOnlineMTMPL.UpdateOnlineMTM(pobjMessageArrivedEventArgs.MessageString)
                    //                // End If
                    //                // End If
                    //                // If Not gobjSync_ExcelAndChartMsgQueue Is Nothing Then
                    //                // gobjSync_ExcelAndChartMsgQueue.Enqueue(pobjMessageArrivedEventArgs.MessageString)
                    //                // End If
                    //                // 'SendDataToExcel(AddScripDataToMBPMessage(lobjRecordSplitter))
                    //                // 'SendDataToChart(pobjMessageArrivedEventArgs.MessageString)
                    //                //lobjStopWatch.Stop()
                    //                //Infrastructure.Logger.WriteLog("One MBP msg Mill" & lobjStopWatch.ElapsedMilliseconds & " Ticks " & lobjStopWatch.ElapsedTicks)
                    //            }
                    //            else if ((lstrMsgId == "59"))
                    //            {
                    //                //Broadcast Message
                    //                //lobjStopWatch.Start()
                    //                if ((gfrmBroadCastMessages != null))
                    //                {
                    //                    if ((!gfrmBroadCastMessages.IsDisposed()))
                    //                    {
                    //                        MessageArrivedDelegate lobjMsgDelegate = new MessageArrivedDelegate(gfrmBroadCastMessages.MessageArrived);
                    //                        IAsyncResult = gfrmBroadCastMessages.BeginInvoke(lobjMsgDelegate, new string[] { pobjMessageArrivedEventArgs.MessageString });
                    //                        while (IAsyncResult.IsCompleted == false)
                    //                        {
                    //                        }
                    //                        gfrmBroadCastMessages.EndInvoke(IAsyncResult);
                    //                        IAsyncResult = null;
                    //                        lobjMsgDelegate = null;
                    //                        //gfrmBroadCastMessages.MessageArrived(pobjMessageArrivedEventArgs.MessageString)
                    //                    }
                    //                }
                    //                //: This is shifted to the InteractiveMessageReceived Function
                    //                //ElseIf (pobjMessageArrivedEventArgs.MessageCode.Equals(Constants.MessageIdentifiers.MESSAGE_MARKET_STATUS_CHANGE_TEXT)) Then
                    //                // 'Market status change messages
                    //                // 'AddMessages(pobjMessageArrivedEventArgs.MessageString)
                    //                // CallAddMessageOnUIThread(pobjMessageArrivedEventArgs.MessageString, Nothing)
                    //                // ShowBroadCastMsgInpnlmsg(pobjMessageArrivedEventArgs.MessageString)
                    //                //lobjStopWatch.Stop()
                    //                //Infrastructure.Logger.WriteLog("One 59 msg Mill" & lobjStopWatch.ElapsedMilliseconds & " Ticks " & lobjStopWatch.ElapsedTicks)
                    //            }
                    //            else if ((lstrMsgId == "69"))
                    //            {
                    //                //lobjStopWatch.Start()
                    //                Indices lobjIndice = null;

                    //                lobjRecordHelper = new RecordSplitter.RecordHelper(pobjMessageArrivedEventArgs.MessageString);
                    //                if (!string.IsNullOrEmpty(lobjRecordHelper.getField(0, IndicesConcise.IndexValue].Trim) && !string.IsNullOrEmpty(lobjRecordHelper.getField(0, IndicesConcise.IndexChangeValue].Trim))
                    //                {
                    //                    //:Added For Chart
                    //                    if (Is_ChartsOpen() == true)
                    //                    {
                    //                        if ((gobjSync_ExcelAndChartMsgQueue != null))
                    //                        {
                    //                            gobjSync_ExcelAndChartMsgQueue.Enqueue(pobjMessageArrivedEventArgs);
                    //                        }
                    //                    }
                    //                    //: Update the indice from IdHash, NameHash will get updated automatically
                    //                    if ((gobjUtilityScript.gobjIndicesById != null))
                    //                    {
                    //                        lobjIndice = gobjUtilityScript.gobjIndicesById.Item(lobjRecordHelper.getField(0, IndicesConcise.IndexId].Trim().ToString);
                    //                        if ((lobjIndice != null))
                    //                        {
                    //                            lobjIndice.Value = lobjRecordHelper.getField(0, IndicesConcise.IndexValue].Trim;
                    //                            lobjIndice.ChangeValue = lobjRecordHelper.getField(0, IndicesConcise.IndexChangeValue].Trim;
                    //                            //: Update the Index Controls
                    //                            UpdateIndexControl(lobjIndice);
                    //                        }
                    //                    }
                    //                }

                    //                //:
                    //                //lobjStopWatch.Stop()
                    //                //Infrastructure.Logger.WriteLog("One 69 msg Mill" & lobjStopWatch.ElapsedMilliseconds & " Ticks " & lobjStopWatch.ElapsedTicks)
                    //            }
                    //            else if ((lstrMsgId == Constants.MessageIdentifiers.MESSAGE_INDEX_TEXT))
                    //            {
                    //                //: Index messages
                    //                //: Updating the Indice value by retriving from NameHash, IdHash will get updated automatically since it is same object
                    //                //lobjStopWatch.Start()
                    //                if (pobjMessageArrivedEventArgs.MessageString.IndexOf("/") < 0)
                    //                {
                    //                    Indices lobjIndice = null;
                    //                    lobjRecordHelper = new RecordSplitter.RecordHelper(pobjMessageArrivedEventArgs.MessageString);

                    //                    // Stop Time Update
                    //                    //lstrTime = lobjRecordHelper.getField(0, 1)
                    //                    //If Not lstrTime Is Nothing AndAlso lstrTime.Trim().Length > 0 Then
                    //                    // Try
                    //                    // SetTextToToolStripStatusLabel(sbpBSETime, lstrTime)
                    //                    // Catch ex As Exception
                    //                    // Infrastructure.Logger.WriteLog("Status Bar threw an exception" & ex.Message)
                    //                    // End Try

                    //                    //End If

                    //                    if ((gobjUtilityScript.gobjIndicesByName != null))
                    //                    {
                    //                        lobjIndice = gobjUtilityScript.gobjIndicesByName.Item(lobjRecordHelper.getField(0, 4].Trim().ToUpper);
                    //                        if ((lobjIndice != null))
                    //                        {
                    //                            if (lobjRecordHelper.getField(0, 5].Trim().Length > 0 && lobjRecordHelper.getField(0, 5) != "0.0")
                    //                            {
                    //                                lobjIndice.Value = lobjRecordHelper.getField(0, 5].Trim;
                    //                            }
                    //                            if (lobjRecordHelper.getField(0, 10].Trim().Length > 0 && lobjRecordHelper.getField(0, 10) != "0.0")
                    //                            {
                    //                                lobjIndice.ChangeValue = lobjRecordHelper.getField(0, 10].Trim;
                    //                            }
                    //                            //: Update the Index controls
                    //                            UpdateIndexControl(lobjIndice);
                    //                        }
                    //                    }

                    //                    //: Update the Index Forms
                    //                    //UpdateIndexForms(lobjIndice, pobjMessageArrivedEventArgs.MessageString)
                    //                    UpdateIndexForms(pobjMessageArrivedEventArgs.MessageString);
                    //                    //lobjStopWatch.Stop()
                    //                    //Infrastructure.Logger.WriteLog("One MESSAGE_INDEX_TEXT msg Mill" & lobjStopWatch.ElapsedMilliseconds & " Ticks " & lobjStopWatch.ElapsedTicks)


                    //                }
                    //                // stop update time
                    //                //ElseIf (lstrMsgId = Constants.MessageIdentifiers.MESSAGE_TIME_TEXT) Then
                    //                // lobjRecordHelper = New RecordSplitter.RecordHelper(pobjMessageArrivedEventArgs.MessageString)
                    //                // REM stop update time
                    //                // 'lstrTime = lobjRecordHelper.getField(0, 1)
                    //                // 'If Not lstrTime Is Nothing AndAlso lstrTime.Trim().Length > 0 Then
                    //                // ' Try
                    //                // ' SetTextToToolStripStatusLabel(sbpBSETime, lstrTime)
                    //                // ' Catch ex As Exception
                    //                // ' Infrastructure.Logger.WriteLog("Status Bar threw an exception" & ex.Message)
                    //                // ' End Try
                    //                // 'End If
                    //            }
                    //        }
                    //        //lobjStopWatch.Stop()
                    //        //Infrastructure.Logger.WriteLog("One Single Broadcast Message Code " & lstrMsgId & " Millisecond " & lobjStopWatch.ElapsedMilliseconds & " Ticks " & lobjStopWatch.ElapsedTicks)
                    //    }
                }
                lobjRecordHelper = null;

            }
            catch (Exception ex)
            {
                Infrastructure.Logger.WriteLog("BroadCastMessageReceived");
                Infrastructure.Logger.WriteLog(ex.StackTrace);
                Infrastructure.Logger.WriteLog(ex.Message);
                if ((pobjMessageArrivedEventArgs != null) && !string.IsNullOrEmpty(pobjMessageArrivedEventArgs.MessageString))
                {
                    Infrastructure.Logger.WriteLog("Error in BroadcastMessageReceived : Message = " + pobjMessageArrivedEventArgs.MessageString);
                }
                Infrastructure.Logger.WriteLog("Error in 2 : Exception = " + ex.Message);
            }
        }

        public static void InteractiveMessageReceived(object sender, SocketConnection.MessageArrivedEventArgs pobjMessageArrivedEventArgs)
        {
            //Get message id

            try
            {
                if (pobjMessageArrivedEventArgs.MessageLength > 0 && (mstrPreviousMessage != pobjMessageArrivedEventArgs.MessageString || pobjMessageArrivedEventArgs.MessageCode == "57"))
                {
                    mstrPreviousMessage = pobjMessageArrivedEventArgs.MessageString;

                    string lstrMsgId = pobjMessageArrivedEventArgs.MessageCode.Trim();
                    //Dim lobjRecordHelper1 As RecordSplitter.RecordHelper = New RecordSplitter.RecordHelper(pobjMessageArrivedEventArgs.MessageString)
                    //: This is handled differently beacuase synclock is put inside the function
                    if (lstrMsgId.Trim().Length > 0)
                    {
                        switch (lstrMsgId)
                        {
                            case "57":
                            case "63":
                            case "64":
                            case "78":
                            case "81":
                            case "71":
                                //if (gblnShowAdminOrderTradeMessages)
                                //{
                                //    switch (lstrMsgId)
                                //    {

                                CommonMessagingWindowVM.AddMessages(pobjMessageArrivedEventArgs.MessageString);

                                //}
                                //}
                                //else
                                //{
                                //    AddMessages(pobjMessageArrivedEventArgs.MessageString);
                                //}
                                //CallAddMessageOnUIThread(pobjMessageArrivedEventArgs.MessageString)


                                break;
                        }
                        //If lstrMsgId.Trim() <> "82" Then
                        //    If lstrMsgId = Constants.MessageIdentifiers.MESSAGE_ORDER_STREAM Then
                        //        REM: So that the Refresh cache is not populated with unnecessory data.
                        //        Dim lstrTransactionCode As String = lobjRecordHelper1.getField(0, 1)
                        //        If (lstrTransactionCode <> Constants.StockConstants.ORDER_ENTRY_REQUEST AndAlso _
                        //            lstrTransactionCode <> Constants.StockConstants.ORDER_CANCELLATION_REQUEST AndAlso _
                        //            lstrTransactionCode <> Constants.StockConstants.ORDER_CANCELLATION_REJECTION AndAlso _
                        //            lstrTransactionCode <> Constants.StockConstants.ORDER_MIRAGE) Then
                        //            mstrRefreshData.Enqueue(pobjMessageArrivedEventArgs.MessageString)
                        //        Else
                        //            If Is_ExcelOpen() = True AndAlso gblnOpenExcelOrderBook = True Then
                        //                Dim lobjMessageEvent As New SocketConnection.MessageArrivedEventArgs(pobjMessageArrivedEventArgs.MessageString)
                        //                If Not gobjSync_ExcelAndChartMsgQueue Is Nothing Then
                        //                    gobjSync_ExcelAndChartMsgQueue.Enqueue(lobjMessageEvent)
                        //                End If
                        //            End If
                        //        End If
                        //    Else
                        //        mstrRefreshData.Enqueue(pobjMessageArrivedEventArgs.MessageString)
                        //    End If

                        //    REM: Dont insert in case of server side
                        //    If (gblnLDBSelf = True OrElse gblnLDBAll = True) Then
                        //        mstrInsertData.Enqueue(pobjMessageArrivedEventArgs.MessageString)
                        //    End If
                        //    Exit Sub
                        //Else
                        //    Dim lblnDataQuedFornetpos As Boolean = False
                        //    If lobjRecordHelper1.numberOfRecords > 1 Then
                        //        If lobjRecordHelper1.getField(1, 0) = "N" Then
                        //            mstrNetPositionRefreshData.Enqueue(pobjMessageArrivedEventArgs.MessageString)
                        //            lblnDataQuedFornetpos = True
                        //        End If
                        //    End If
                        //    If lblnDataQuedFornetpos = False Then
                        //        mstrNetPositionInsertData.Enqueue(pobjMessageArrivedEventArgs.MessageString)
                        //    End If
                        //    If Is_ExcelOpen() = True AndAlso gblnOpenExcelNetPosition = True Then
                        //        Dim lobjRecHelper As RecordSplitter.RecordHelper
                        //        lobjRecHelper = frmListNetPosition.ReCalculateValues(pobjMessageArrivedEventArgs.MessageString, False, 0)
                        //        Dim lobjMessageEvent As New SocketConnection.MessageArrivedEventArgs(String.Join("|", lobjRecHelper.getRecord(0)))
                        //        If Not gobjSync_ExcelAndChartMsgQueue Is Nothing Then
                        //            gobjSync_ExcelAndChartMsgQueue.Enqueue(lobjMessageEvent)
                        //        End If
                        //    End If
                        //End If
                    }
                }
            }
            catch (Exception ex)
            {
                Infrastructure.Logger.WriteLog("Error in In InteractiveMessageReceived : Exception = " + ex.Message);
            }
        }


    }
#endif
}
