using CommonFrontEnd;
using CommonFrontEnd.Common;
using CommonFrontEnd.Global;
using CommonFrontEnd.HTTPHlper;
using System;
using System.Collections;
using static CommonFrontEnd.HTTPHlper.HTTPHlpr;


namespace CommonFrontEnd.GetDataForStock
{
#if BOW
    public  class GetDataForStocks
    {

        private static UtilityConnParameters mobjUtilityConnParameters;
        private static Hashtable mobjSecureServletList = null;
        private static bool mblnUSESecureConnection;
        private static int mintHttpTimeOut = 3000;
        public static event ForceDoEventEventHandler ForceDoEvent;
        public  delegate void ForceDoEventEventHandler();
        private static GetDataForStocks mobjInstance;
        //: Do not delete this as we are creating a Singleton Object.
        private GetDataForStocks()
        {
        }

        public static GetDataForStocks GetInstance
        {
            get
            {
                if (mobjInstance == null)
                {
                    mobjInstance = new GetDataForStocks();
                }
                return mobjInstance;
            }
        }
        public void InitializeGetDataForStock(UtilityConnParameters pobjUtilityConnParameters, Hashtable pobjSecureServletList)
        {
            mobjUtilityConnParameters = pobjUtilityConnParameters;
            mobjSecureServletList = pobjSecureServletList;
        }

        public UtilityConnParameters ConnectionParameters
        {
            get { return mobjUtilityConnParameters; }
            set { mobjUtilityConnParameters = value; }
        }

        public Hashtable SecureServletList
        {
            get { return mobjSecureServletList; }
            set { mobjSecureServletList = value; }
        }

        public static bool Use_SecureConnection
        {
            get { return mblnUSESecureConnection; }
            set { mblnUSESecureConnection = value; }
        }

        public long UserId
        {
            get
            {
                if ((mobjUtilityConnParameters != null))
                {
                    return mobjUtilityConnParameters.UserId;
                }
                return 0;
            }
        }
        public static int HttpTimeOut
        {
            get { return mintHttpTimeOut; }
            set { mintHttpTimeOut = value; }
        }
        public static string GetDataFromServer(string pstrServlet, string[] pstrParameters, string[] pstrValues, bool pblnSecure = false, ResponseReturned pobjDelegate = null)
        {
            mobjUtilityConnParameters = UtilityConnParameters.GetInstance;
            //TODO: Need to Remove hardocded value
            mobjUtilityConnParameters.SERVER = "10.1.101.6:6080/stocks";
            //TODO: Need to Remove hardocded value

            if (ForceDoEvent != null)
            {
                ForceDoEvent();
            }
            if ((pstrParameters.Length != pstrValues.Length))
            {
                return "1|Parameter size mismatch|0";
            }
            //HTTPHlpr lobjHTTPHelper = default(HTTPHelper);
            HTTPHlpr lobjHTTPHelper = new HTTPHlpr();
            lobjHTTPHelper.Method = "Post";
            lobjHTTPHelper.TimeOut = mintHttpTimeOut;
            if ((mobjUtilityConnParameters.PROXYSERVER != null))
            {
                lobjHTTPHelper.ProxyServer = mobjUtilityConnParameters.PROXYSERVER;
                lobjHTTPHelper.ProxyPort = mobjUtilityConnParameters.PROXYPORT;
                if ((mobjUtilityConnParameters.PROXYUSER != null))
                {
                    lobjHTTPHelper.ProxyUserId = mobjUtilityConnParameters.PROXYUSER;
                    lobjHTTPHelper.ProxyUserPassword = mobjUtilityConnParameters.PROXYPASSWORD;
                }
            }
            //: Determines whethere to call servlet Securely or not
            pblnSecure = IsServletInSecureList(pstrServlet);
            if (pblnSecure == true || mblnUSESecureConnection == true)
            {
                lobjHTTPHelper.Protocol = "https://";
            }
            else
            {
                lobjHTTPHelper.Protocol = "http://";
            }
            lobjHTTPHelper.Server = mobjUtilityConnParameters.SERVER;
            lobjHTTPHelper.Servlet = pstrServlet;

            //: For Data to be received in Compressed form or not 
            lobjHTTPHelper.CompressData = mobjUtilityConnParameters.CompressData;
            pstrParameters[pstrParameters.Length - 1] = mobjUtilityConnParameters.ThickClientParameter;
            pstrValues[pstrValues.Length - 1] = mobjUtilityConnParameters.ThickClientValue;
            pstrParameters[pstrParameters.Length - 2] = mobjUtilityConnParameters.LoginKeyName;
            if ((mobjUtilityConnParameters.LoginKeyValue == null))
            {
                pstrValues[pstrValues.Length - 2] = "";
            }
            else
            {
                pstrValues[pstrValues.Length - 2] = mobjUtilityConnParameters.LoginKeyValue;
            }
            lobjHTTPHelper.setValues(pstrParameters, pstrValues);
            try
            {
                if ((pobjDelegate != null))
                {
                    lobjHTTPHelper.OnHTTPResponseCompleted += pobjDelegate;
                    lobjHTTPHelper.sendThread();
                    return "";
                }
                return lobjHTTPHelper.send();
            }
            catch (Exception ex)
            {
                return "1|" + ex.Message + "|0";
            }
        }

        public string GetDataFromServer(string pstrServlet, ArrayList pParamNamesList = null, ArrayList pParamValuesList = null, bool pblnSecure = false, ResponseReturned pobjDelegate = null)
        {
            if (ForceDoEvent != null)
            {
                ForceDoEvent();
            }
            if ((pParamNamesList != null) & (pParamValuesList != null))
            {
                if ((pParamNamesList.Count != pParamValuesList.Count))
                {
                    return "1|Parameter size mismatch|0";
                }
            }
            else
            {
                pParamNamesList = new ArrayList();
                pParamValuesList = new ArrayList();
            }
            HTTPHlpr lobjHTTPHelper = new HTTPHlpr();
            lobjHTTPHelper.Method = "Post";
            if ((mobjUtilityConnParameters.PROXYSERVER != null))
            {
                lobjHTTPHelper.ProxyServer = mobjUtilityConnParameters.PROXYSERVER;
                lobjHTTPHelper.ProxyPort = mobjUtilityConnParameters.PROXYPORT;
                if ((mobjUtilityConnParameters.PROXYUSER != null))
                {
                    lobjHTTPHelper.ProxyUserId = mobjUtilityConnParameters.PROXYUSER;
                    lobjHTTPHelper.ProxyUserPassword = mobjUtilityConnParameters.PROXYPASSWORD;
                }
            }
            //: Determines whethere to call servlet Securely or not
            pblnSecure = IsServletInSecureList(pstrServlet);
            if (pblnSecure == true || mblnUSESecureConnection == true)
            {
                lobjHTTPHelper.Protocol = "https://";
            }
            else
            {
                lobjHTTPHelper.Protocol = "http://";
            }
            lobjHTTPHelper.Server = mobjUtilityConnParameters.SERVER;
            lobjHTTPHelper.Servlet = pstrServlet;

            //: For Data to be received in Compressed form or not 
            lobjHTTPHelper.CompressData = mobjUtilityConnParameters.CompressData;

            pParamNamesList.Add(mobjUtilityConnParameters.ThickClientParameter);
            pParamValuesList.Add(mobjUtilityConnParameters.ThickClientValue);
            pParamNamesList.Add(mobjUtilityConnParameters.LoginKeyName);
            if ((mobjUtilityConnParameters.LoginKeyValue == null))
            {
                pParamValuesList.Add("");
            }
            else
            {
                pParamValuesList.Add(mobjUtilityConnParameters.LoginKeyValue);
            }
            lobjHTTPHelper.setValues(pParamNamesList, pParamValuesList);
            try
            {
                if ((pobjDelegate != null))
                {
                    lobjHTTPHelper.OnHTTPResponseCompleted += pobjDelegate;
                    lobjHTTPHelper.sendThread();
                    return "";
                }
                return lobjHTTPHelper.send();
            }
            catch (Exception ex)
            {
                return "1|" + ex.Message + "|0";
            }
        }

        public string GetDataFromServer(string pstrServlet, Hashtable pobjParameters, bool pblnSecure = false)
        {
            string[] lstrLoginKey = new string[2];
            string[] lstrThickClient = new string[2];
            string lstrReturnString = null;
            HTTPHlpr lobjHTTPHelper = new HTTPHlpr();
            if (ForceDoEvent != null)
            {
                ForceDoEvent();
            }
            //lobjHTTPHelper = new HTTPHelper();
            lobjHTTPHelper.Method = "Post";
            if ((mobjUtilityConnParameters.PROXYSERVER != null))
            {
                lobjHTTPHelper.ProxyServer = mobjUtilityConnParameters.PROXYSERVER;
                lobjHTTPHelper.ProxyPort = mobjUtilityConnParameters.PROXYPORT;
            }
            if ((mobjUtilityConnParameters.PROXYUSER != null) && !string.IsNullOrEmpty(mobjUtilityConnParameters.PROXYUSER))
            {
                lobjHTTPHelper.ProxyUserId = mobjUtilityConnParameters.PROXYUSER;
                lobjHTTPHelper.ProxyUserPassword = mobjUtilityConnParameters.PROXYPASSWORD;
            }
            //: Determines whethere to call servlet Securely or not
            pblnSecure = IsServletInSecureList(pstrServlet);
            if (pblnSecure == true || mblnUSESecureConnection == true)
            {
                lobjHTTPHelper.Protocol = "https://";
            }
            else
            {
                lobjHTTPHelper.Protocol = "http://";
            }
            lobjHTTPHelper.Server = mobjUtilityConnParameters.SERVER;
            lobjHTTPHelper.Servlet = pstrServlet;
            lstrThickClient[0] = mobjUtilityConnParameters.ThickClientValue;
            pobjParameters.Add(mobjUtilityConnParameters.ThickClientParameter, lstrThickClient);
            if ((mobjUtilityConnParameters.LoginKeyValue == null))
            {
                lstrLoginKey[0] = "";
            }
            else
            {
                lstrLoginKey[0] = mobjUtilityConnParameters.LoginKeyValue;
            }
            //: For Data to be received in Compressed form or not 
            lobjHTTPHelper.CompressData = mobjUtilityConnParameters.CompressData;

            pobjParameters.Add(mobjUtilityConnParameters.LoginKeyName, lstrLoginKey);
            lobjHTTPHelper.setValues(pobjParameters);
            try
            {
                lstrReturnString = lobjHTTPHelper.send();
            }
            catch (Exception lobjException)
            {
                lstrReturnString = "1|" + lobjException.Message + "|";
            }
            return lstrReturnString;
        }

        public string SendMessageOverHTTP(string pstrMessages, string pstrBroadCastServletName, ResponseReturned pobjDelegate = null, bool pblnSecure = false)
        {
            string lstrLoginKey = null;
            string lstrThickClient = "Y";
            string MESSAGE_STRING = "MESSAGE";
            string MESSAGECOUNT_STRING = "MESSAGECOUNT";
            string lstrReturnString = "";
            System.Collections.ArrayList lstrParameters = new System.Collections.ArrayList();
            System.Collections.ArrayList lstrValues = new System.Collections.ArrayList();
            int lintCount = 0;
            HTTPHlpr lobjHTTPHelper = new HTTPHlpr();
            //: Determines whethere to call servlet Securely or not
            pblnSecure = IsServletInSecureList(pstrBroadCastServletName);
            if (pblnSecure == true || mblnUSESecureConnection == true)
            {
                lobjHTTPHelper.Protocol = "https://";
            }
            else
            {
                lobjHTTPHelper.Protocol = "http://";
            }
            lobjHTTPHelper.Method = "Post";
            lobjHTTPHelper.Server = mobjUtilityConnParameters.SERVER;
            lobjHTTPHelper.Servlet = pstrBroadCastServletName;
            //: For Data to be received in Compressed form or not 
            lobjHTTPHelper.CompressData = mobjUtilityConnParameters.CompressData;

            if ((mobjUtilityConnParameters.PROXYSERVER != null))
            {
                lobjHTTPHelper.ProxyServer = mobjUtilityConnParameters.PROXYSERVER;
                lobjHTTPHelper.ProxyPort = mobjUtilityConnParameters.PROXYPORT;
            }
            if ((mobjUtilityConnParameters.PROXYUSER != null) && !string.IsNullOrEmpty(mobjUtilityConnParameters.PROXYUSER))
            {
                lobjHTTPHelper.ProxyUserId = mobjUtilityConnParameters.PROXYUSER;
                lobjHTTPHelper.ProxyUserPassword = mobjUtilityConnParameters.PROXYPASSWORD;
            }

            lstrThickClient = mobjUtilityConnParameters.ThickClientValue;
            //: Parameters are LK,USID,MWDET
            //: LK = LoginKey
            //: USID = mobjUtilityConnParameters.UserID()
            if ((mobjUtilityConnParameters.LoginKeyValue == null))
            {
                lstrLoginKey = "";
            }
            else
            {
                lstrLoginKey = mobjUtilityConnParameters.LoginKeyValue;
            }
            //: USID Added
            lstrParameters.Add("USID");
            lstrValues.Add(Convert.ToString(mobjUtilityConnParameters.UserId));
            //: MODE Proxy
            lstrParameters.Add("MODE");
            lstrValues.Add("PROXY");

            //For lintTemp As Integer = 0 To pstrMessages.Length - 1
            lstrParameters.Add(MESSAGE_STRING + lintCount);
            //lstrValues.Add(pstrMessages(lintTemp))
            lstrValues.Add(pstrMessages);
            lintCount += 1;
            //Next

            //: Add MESSAGE COUNT 
            lstrParameters.Add(MESSAGECOUNT_STRING);
            lstrValues.Add(lintCount.ToString());
            //: LK Added
            lstrParameters.Add(mobjUtilityConnParameters.LoginKeyName);
            lstrValues.Add(lstrLoginKey);
            //: Thick Client
            lstrParameters.Add(mobjUtilityConnParameters.ThickClientParameter);
            lstrValues.Add(mobjUtilityConnParameters.ThickClientValue);
            //:
            lobjHTTPHelper.setValues(lstrParameters, lstrValues);

            try
            {
                if ((pobjDelegate != null))
                {
                    lobjHTTPHelper.OnHTTPResponseCompleted += pobjDelegate;
                    lobjHTTPHelper.sendThread();
                    return "";
                }
                lobjHTTPHelper.send();
            }
            catch (Exception lobjException)
            {
                lstrReturnString = "1|" + lobjException.Message + "|";
            }
            return lstrReturnString;
        }

        private static bool IsServletInSecureList(string pstrServlet)
        {
            try
            {
                if ((pstrServlet != null))
                {
                    if (!string.IsNullOrEmpty(pstrServlet))
                    {
                        pstrServlet = pstrServlet.Replace("/", "");
                        if (!string.IsNullOrEmpty(pstrServlet))
                        {
                            if ((mobjSecureServletList != null))
                            {
                                return mobjSecureServletList.ContainsKey(pstrServlet);
                            }
                        }
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
#endif 

}
