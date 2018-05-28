using LZ4Sharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CommonFrontEnd.Common
{
    /// <summary>
    /// This class fetches opens a URL specified with the
    /// parameters specified and returns the string returned
    /// from the URL.
    /// </summary>

    public class HTTPHelper
    {
#if BOW
        // http or https
        private string mProtocol = "http://";
        private string mServer = "";
        private static string mServerCurrentStatic = "";
        private string mServlet = "";
        // for switching between multiple tomcat servers 
        // Object Level is set to false, so that the request from a working server, by any object of this type is used.
        private string[] mServerList;
        private int mintServerCurrentIndex = -1;
        private static int mintLastWorkingServerIndex = -1;
        private bool mblnObjectLevel;
        //
        private string proxyServer;
        private int proxyPort;
        private string proxyUserId;
        private string proxyUserPassword;
        private string method;
        private string[] parameters;
        private string[] values;
        private bool sendRedirect;
        private bool mblnTerminating;

        private bool mblnResponseCompleted;
        private bool mblnConnected;
        private bool mblnContinueStreaming = true;
        string mstrUrl;
        private string mstrOrderNumForCancel = "";
        private bool mblnisOrderBook = false;



        // TimeOut in MilliSeconds
        private int mintTimeOut = 3000;
        public delegate void ResponseReturned(string pstrResult);
        public delegate void ResponseReturnedForOrder(string pstrResult, string pstrOrderNumber);
        public delegate void BroadcastMessageArrived(string pstrResult);

        public event ResponseReturned OnHTTPResponseCompleted;
        public event ResponseReturnedForOrder OnHTTPOrderResponseCompleted;

        public event BroadcastMessageArrived OnHTTPMessageArrived;
        private bool mblnCompressData;
        private int mintNoOfBytesToRead = Constants.NO_OF_BYTES_TO_READ;

        public int mintCurrentRequestId;
        private static int mintRequestCount;
        private static MyRequestHash mobjRequestHash = new MyRequestHash();
        private static MyResponseHash mobjResponseHash = new MyResponseHash();

        private static bool mblnLogHttpSendReceive = HTTPRequestResponseLogging();
        private static System.Threading.Mutex mobjMutex = new System.Threading.Mutex();
        private LZ4Decompressor32 mobjCompressionHelper;

        #region "RequestResponseLogging"

        public static bool LogHttpSendReceive
        {
            get { return mblnLogHttpSendReceive; }
            set { mblnLogHttpSendReceive = value; }
        }
        public static MyRequestHash Request
        {
            get { return mobjRequestHash; }
        }
        public static MyResponseHash Response
        {
            get { return mobjResponseHash; }
        }

        private static bool HTTPRequestResponseLogging()
        {
            bool lblnLog = false;
            try
            {

                System.Configuration.AppSettingsReader objReader;
                objReader = new System.Configuration.AppSettingsReader();
                lblnLog = (bool)objReader.GetValue("HTTPRequestResponseLog", lblnLog.GetType());
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Write("Exception in RequestResponse logging" + ex.Message);
                lblnLog = false;
            }
            return lblnLog;
        }


        private void RequestLog()
        {
            //Logging Request and Resopons only if HTTPRequestResponseLog is set to true in  UtilityLoginDetails.GETInstance.config.
            if (mblnLogHttpSendReceive == true)
            {
                try
                {
                    MyRequest lobjRequest;
                    lobjRequest = new MyRequest();
                    string lstrServlet;
                    lstrServlet = mServlet.Replace("/", "");
                    //Feeding data in the Request Class
                    lobjRequest.URL = mstrUrl;
                    lobjRequest.ServerName = mServer;
                    lobjRequest.ServletName = lstrServlet;
                    lobjRequest.MethodCalled = method;
                    lobjRequest.TimeOfRequest = System.DateTime.Now.ToString() + "." + System.DateTime.Now.Millisecond.ToString();

                    if (parameters != null && values != null)
                    {
                        if (parameters.Length == values.Length)
                        {
                            lobjRequest.ParameterNames.AddRange(parameters);
                            lobjRequest.ParameterValues.AddRange(values);
                        }
                    }
                    mobjMutex.WaitOne();
                    mintRequestCount += 1;
                    mintCurrentRequestId = mintRequestCount;
                    lobjRequest.RequestId = mintCurrentRequestId;
                    // Adding Requests to the hashtable based on RequestId
                    mobjRequestHash.AddToHash(lobjRequest.RequestId, lobjRequest);
                    mobjMutex.ReleaseMutex();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Write("ERROR IN REQUEST LOG" + ex.Message);
                }
            }
        }

        private void ResponseLog(string pstrReturnValue)
        {
            //Logging Request and Resopons only if HTTPRequestResponseLog is set to true in  UtilityLoginDetails.GETInstance.config.
            if (mblnLogHttpSendReceive == true)
            {
                try
                {
                    string lstrServlet;
                    lstrServlet = mServlet.Replace("/", "");
                    MyResponse lobjResponse;
                    lobjResponse = new MyResponse();
                    lobjResponse.RequestId = mintCurrentRequestId;
                    lobjResponse.ServerName = mServer;
                    lobjResponse.ServletName = lstrServlet;
                    lobjResponse.MethodCalling = method;
                    lobjResponse.ResultString = pstrReturnValue;
                    lobjResponse.TimeOfResponse = System.DateTime.Now.ToString() + "." + System.DateTime.Now.Millisecond.ToString();

                    mobjMutex.WaitOne();

                    //Adding Response to the ResponseHash based on RequestId
                    mobjResponseHash.AddToHash(lobjResponse.RequestId, lobjResponse);
                    mobjMutex.ReleaseMutex();

                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Write("ERROR IN RESPONSE LOG" + ex.Message);
                }
            }
        }


        public static Boolean WriteToTextFile()
        {
            Boolean lblSuccess = true;
            System.IO.StreamWriter lobjFileStream;
            MyResponse lobjResponse;
           StringBuilder lstrRequestResponseData;

            lstrRequestResponseData = new StringBuilder();

            lobjFileStream = new System.IO.StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\RequestResponse.txt", false, Encoding.ASCII);
            try
            {
                foreach (MyRequest lobjRequest in mobjRequestHash.Values)
                {
                    lstrRequestResponseData.Append("----------REQUEST ID :-  " + lobjRequest.RequestId + "-----------------------");
                    lstrRequestResponseData.Append("\n");
                    lstrRequestResponseData.Append("URL  :- " + lobjRequest.URL);
                    lstrRequestResponseData.Append("\n");
                    lstrRequestResponseData.Append("Time Of Request :- " + lobjRequest.TimeOfRequest);
                    lstrRequestResponseData.Append("\n");
                    lstrRequestResponseData.Append("Server Name :- " + lobjRequest.ServerName);
                    lstrRequestResponseData.Append("\n");
                    lstrRequestResponseData.Append("Servlet Name :- " + lobjRequest.ServletName);
                    lstrRequestResponseData.Append("\n");
                    lstrRequestResponseData.Append("ConfigureLogger Called :- " + lobjRequest.MethodCalled);
                    lstrRequestResponseData.Append("\n");
                    lstrRequestResponseData.Append("Parameter Name      :      Parameter Values");

                    for (int lintTemp = 0; lintTemp < lobjRequest.ParameterNames.Count - 1; lintTemp++)
                    {
                        lstrRequestResponseData.Append(lobjRequest.ParameterNames[lintTemp] + ":" + lobjRequest.ParameterValues[lintTemp]);
                        lstrRequestResponseData.Append("\n");
                    }

                    lobjResponse = mobjResponseHash.RequestIdValue(lobjRequest.RequestId);
                    if (lobjResponse != null && lobjResponse.ResultString.Trim().Length > 0)
                    {
                        lstrRequestResponseData.Append("\n");
                        lstrRequestResponseData.Append("----------RESPONSE ID :-  " + lobjRequest.RequestId + "-----------------------");
                        lstrRequestResponseData.Append("\n");
                        lstrRequestResponseData.Append("Time Of Response  :- " + lobjResponse.TimeOfResponse);
                        lstrRequestResponseData.Append("\n");
                        lstrRequestResponseData.Append("Server Name  :- " + lobjResponse.ServerName);
                        lstrRequestResponseData.Append("\n");
                        lstrRequestResponseData.Append("Servlet Name  :- " + lobjResponse.ServletName);
                        lstrRequestResponseData.Append("\n");
                        lstrRequestResponseData.Append("ConfigureLogger Called  :- " + lobjResponse.MethodCalling);
                        lstrRequestResponseData.Append("\n");
                        lstrRequestResponseData.Append("Result String  :- " + lobjResponse.ResultString);
                        lstrRequestResponseData.Append("\n");
                    }
                }
                lobjFileStream.Write(lstrRequestResponseData.ToString());
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Write("Error in Writing The RequestResponseLog File" + ex.Message);
                lblSuccess = false;
            }
            finally
            {
                lobjFileStream.Flush();
                lobjFileStream.Close();
                lobjFileStream = null;
            }

            return lblSuccess;
        }

        #endregion

        public HTTPHelper()
        {
            mobjCompressionHelper = new LZ4Decompressor32();
        }

        public static int LastWorkingServerIndex
        {
            get { return mintLastWorkingServerIndex; }
        }
        public int TimeOut
        {
            get { return mintTimeOut; }
            set { mintTimeOut = value; }
        }
        public bool IsExpect100Continue = true;
        public void sendThread()
        {
            Thread lobjThread = new Thread(new ThreadStart(callSend));
            lobjThread.SetApartmentState(ApartmentState.STA);
            lobjThread.Name = "HTTPThread " + this.GetHashCode().ToString();
            lobjThread.Start();
        }

        private void callSend()
        {
            send();
        }

        public bool CompressData
        {
            set
            {
                mblnCompressData = value;
            }
            get
            {
                return mblnCompressData;
            }
        }
        public bool ObjectLevel
        {
            set { mblnObjectLevel = value; }
            get { return mblnObjectLevel; }
        }
        /// <summary>
        /// ReadOnly IS_ErrorLoggingEnabled which indicates the HTTP has TimedOut.
        /// </summary>
        /// 

        public bool Terminating
        {
            set { mblnTerminating = value; }
            get { return mblnTerminating; }
        }
        public bool ResponseCompleted
        {
            set { mblnResponseCompleted = value; }
            get { return mblnResponseCompleted; }
        }
        public bool Connected
        {
            get { return mblnConnected; }
            set { mblnConnected = value; }
        }
        public bool ContinueStreaming
        {
            get { return mblnContinueStreaming; }
            set { mblnContinueStreaming = value; }
        }
        public X509Certificate GetCertificate()
        {
            X509Certificate mobjCertificate = null;
            int lintLoopCount;

            if (mblnObjectLevel == false)
            {
                SetLastWorkingServer();
            }
            // this loop automates the switching between the list of servers. 
            // ie. Error is returned only if all the servers in the list fail
            for (lintLoopCount = 1; lintLoopCount <= mServerList.Length; ++lintLoopCount)
            {
                HttpWebRequest lHttpWebRequest;
                HttpWebResponse lHttpWebResponse;
                WebProxy lWebProxy;
                StringBuilder lReturnString;
                StringBuilder lParameterString;
                byte[] lParameterBytes;
                Stream lStream;

                lReturnString = new StringBuilder();
                lParameterString = new StringBuilder();

                if (mServer == null)
                {
                    throw new Exception("Please specify a URL to fetch data from.");
                }
                mstrUrl = mProtocol + mServer + "/" + mServlet;

                if (method == null)
                {
                    method = "GET";
                }
                if (method.Trim().Length == 0)
                {
                    method = "GET";
                }
                method = method.ToUpper();
                lParameterString.Append("");

                if (parameters != null)
                {
                    for (int lCount = 0; lCount < parameters.Length; lCount++)
                    {
                        if (lCount > 0)
                            lParameterString.Append("&");

                        if (values[lCount] != null)
                        {
                            if (values[lCount].IndexOf("&") != -1)
                            {
                                values[lCount] = values[lCount].Replace("&", "%26");
                            }
                            lParameterString.Append(parameters[lCount]);
                            lParameterString.Append("=");
                            lParameterString.Append(values[lCount]);
                        }
                    }
                    // for sending whether the data to be received is compressed or not
                    lParameterString.Append("&");

                    lParameterString.Append(Constants.COMPRESSED_PARAMETER);
                    lParameterString.Append("=");
                    if (mblnCompressData == true)
                    {
                        lParameterString.Append(Constants.YESNO_Y);
                    }
                    else
                    {
                        lParameterString.Append(Constants.YESNO_N);
                    }
                }
                if (method == "GET")
                {
                    mstrUrl = mstrUrl + "?" + lParameterString.ToString();
                }
                lHttpWebRequest = (HttpWebRequest)WebRequest.Create(mstrUrl);

                MyCertificateValidation lCertificate = new MyCertificateValidation();
                ICertificatePolicy lOldCertificatePolicy = ServicePointManager.CertificatePolicy;
                ICertificatePolicy lNewCertificatePolicy = new MyCertificateValidation();
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;

                lHttpWebRequest.Method = method;
                lHttpWebRequest.ServicePoint.Expect100Continue = IsExpect100Continue;
                lHttpWebRequest.AllowAutoRedirect = sendRedirect;
                lHttpWebRequest.Timeout = mintTimeOut;
                lHttpWebRequest.ReadWriteTimeout = mintTimeOut;
                if (proxyServer != null)
                {
                    if (proxyServer.Trim().Length > 0)
                    {
                        lWebProxy = new WebProxy(proxyServer + ":" + proxyPort, true);
                        if (proxyUserId.Trim().Length > 0)
                        {
                            lWebProxy.Credentials = new NetworkCredential(proxyUserId, proxyUserPassword);
                            lHttpWebRequest.Credentials = lWebProxy.Credentials;
                        }
                        lHttpWebRequest.Proxy = lWebProxy;
                    }
                }
                else
                {
                    lWebProxy = System.Net.WebProxy.GetDefaultProxy();
                    if (lWebProxy != null)
                    {
                        if (lWebProxy.Address != null)
                        {
                            if (proxyUserId != null)
                            {
                                if (proxyUserPassword == null)
                                {
                                    proxyUserPassword = "";
                                }
                                lWebProxy.Credentials = new NetworkCredential(proxyUserId, proxyUserPassword);
                                lHttpWebRequest.Credentials = lWebProxy.Credentials;
                                lHttpWebRequest.Proxy = lWebProxy;
                            }
                        }
                    }
                }
                try
                {
                    if (method == "POST" && lParameterString.Length > 0)
                    {

                        lHttpWebRequest.ContentType = "application/x-www-form-urlencoded";
                        lStream = lHttpWebRequest.GetRequestStream();
                        mblnResponseCompleted = false;
                        mblnConnected = true;
                        lParameterBytes = Encoding.ASCII.GetBytes(lParameterString.ToString());

                        lStream.Write(lParameterBytes, 0, lParameterString.Length);
                        lStream.Flush();
                        lStream.Close();
                    }


                    lHttpWebResponse = (HttpWebResponse)lHttpWebRequest.GetResponse();
                    ServicePointManager.CertificatePolicy = lOldCertificatePolicy;
                    mobjCertificate = ((MyCertificateValidation)lNewCertificatePolicy).Certificate;
                }
                catch (Exception lException)
                {
                    Infrastructure.Logger.WriteLog("Error while Geting Certificate" + lException.Message);
                    return null;
                }
            }
            return mobjCertificate;
        }
        public String send()
        {
            int lintLoopCount;
            string lstrReturnMessage;
            Exception lobjException;
            lstrReturnMessage = "";
            lobjException = null;
            string lData = "";

            if (mblnObjectLevel == false)
            {
                SetLastWorkingServer();
            }
            // this loop automates the switching between the list of servers. 
            // ie. Error is returned only if all the servers in the list fail
            for (lintLoopCount = 1; lintLoopCount <= mServerList.Length; ++lintLoopCount)
            {
                HttpWebRequest lHttpWebRequest;
                HttpWebResponse lHttpWebResponse;
                StreamReader lStreamReader = null;
                BinaryReader lBinaryReader = null;
                WebProxy lWebProxy;
                StringBuilder lReturnString;
                StringBuilder lParameterString;
                byte[] lParameterBytes;
                Stream lStream;

                byte[] lDeCompressed;
                byte[] lWorkMemory;
                byte[] lByteBuffer;
                int lDataLength;
                int lDeCompressedLength;
                lReturnString = new StringBuilder();
                lParameterString = new StringBuilder();

                if (mServer == null)
                {
                    throw new Exception("Please specify a URL to fetch data from.");
                }
                mstrUrl = mProtocol + mServer + "/" + mServlet;

                if (method == null)
                {
                    method = "GET";
                }
                if (method.Trim().Length == 0)
                {
                    method = "GET";
                }
                method = method.ToUpper();
                lParameterString.Append("");
                if (mblnLogHttpSendReceive == true)
                {
                    RequestLog();
                }

                if (parameters != null)
                {
                    for (int lCount = 0; lCount < parameters.Length; lCount++)
                    {
                        if (lCount > 0)
                            lParameterString.Append("&");

                        if (values[lCount] != null)
                        {
                            if (values[lCount].IndexOf("&") != -1)
                            {
                                values[lCount] = values[lCount].Replace("&", "%26");
                            }
                            lParameterString.Append(parameters[lCount]);
                            lParameterString.Append("=");
                            lParameterString.Append(values[lCount]);
                        }
                    }
                    // for sending whether the data to be received is compressed or not
                    lParameterString.Append("&");

                    lParameterString.Append(Constants.COMPRESSED_PARAMETER);
                    lParameterString.Append("=");
                    if (mblnCompressData == true)
                    {
                        lParameterString.Append(Constants.YESNO_Y);
                    }
                    else
                    {
                        lParameterString.Append(Constants.YESNO_N);
                    }
                }
                if (method == "GET")
                {
                    mstrUrl = mstrUrl + "?" + lParameterString.ToString();
                }
                lHttpWebRequest = (HttpWebRequest)WebRequest.Create(mstrUrl);
                lHttpWebRequest.Method = method;
                lHttpWebRequest.ServicePoint.Expect100Continue = IsExpect100Continue;
                lHttpWebRequest.ServicePoint.ConnectionLimit = 10;
                lHttpWebRequest.AllowAutoRedirect = sendRedirect;
                lHttpWebRequest.Timeout = mintTimeOut;
                lHttpWebRequest.ReadWriteTimeout = mintTimeOut;
                if (proxyServer != null)
                {
                    if (proxyServer.Trim().Length > 0)
                    {
                        lWebProxy = new WebProxy(proxyServer + ":" + proxyPort, true);
                        if (proxyUserId.Trim().Length > 0)
                        {
                            lWebProxy.Credentials = new NetworkCredential(proxyUserId, proxyUserPassword);
                            lHttpWebRequest.Credentials = lWebProxy.Credentials;
                        }
                        lHttpWebRequest.Proxy = lWebProxy;
                    }
                }
                else
                {
                    lWebProxy = WebProxy.GetDefaultProxy();
                    if (lWebProxy != null)
                    {
                        if (lWebProxy.Address != null)
                        {
                            if (proxyUserId != null)
                            {
                                if (proxyUserPassword == null)
                                {
                                    proxyUserPassword = "";
                                }
                                lWebProxy.Credentials = new NetworkCredential(proxyUserId, proxyUserPassword);
                                lHttpWebRequest.Credentials = lWebProxy.Credentials;
                                lHttpWebRequest.Proxy = lWebProxy;
                            }
                        }
                    }
                }
                try
                {
                    if (method == "POST" && lParameterString.Length > 0)
                    {

                        lHttpWebRequest.ContentType = "application/x-www-form-urlencoded";
                        lStream = lHttpWebRequest.GetRequestStream();
                        mblnResponseCompleted = false;
                        mblnConnected = true;

                        /*					
                                                lParameterBytes = new byte[lParameterString.Length];
                                                char[] lstrParamCharArray;
                                                lstrParamCharArray = lParameterString.ToString().ToCharArray();
                                                for (int lCount = 0; lCount < lParameterString.Length; lCount++)
                                                {
                                                    lParameterBytes[lCount] = (byte)lstrParamCharArray[lCount];
                                                }
                        */
                        lParameterBytes = Encoding.ASCII.GetBytes(lParameterString.ToString());
                        Infrastructure.Logger.WriteLog("Request Sent     : -> " + lHttpWebRequest.Address.AbsoluteUri.ToString() + "  " + lParameterString.ToString(), true);
                        lStream.Write(lParameterBytes, 0, lParameterString.Length);
                        lStream.Flush();
                        lStream.Close();
                        //Infrastructure.Logger.WriteLog(" Http Message Sent " + DateTime.Now.ToString() );
                    }

                    //Infrastructure.Logger.WriteLog(" Http Message Sent " + DateTime.Now.ToString() + '-' + DateTime.Now.Millisecond.ToString());
                    lHttpWebResponse = (HttpWebResponse)lHttpWebRequest.GetResponse();
                    if (!mblnCompressData)
                        lStreamReader = new StreamReader(lHttpWebResponse.GetResponseStream(),Encoding.ASCII);
                    else
                        lBinaryReader = new BinaryReader(lHttpWebResponse.GetResponseStream());

                    if (mblnCompressData == false)
                    {
                        if (OnHTTPMessageArrived != null)
                        {
                            while (mblnContinueStreaming)
                            {
                                try
                                {
                                    string lMessageString = lStreamReader.ReadLine();
                                    if (lMessageString != null)
                                        OnHTTPMessageArrived(lMessageString);
                                    else
                                        break;
                                }
                                catch (Exception innerException)
                                {
                                    if (innerException.ToString().ToUpper().IndexOf("TIMEOUT") >= 0)
                                    {
                                        //ignore
                                    }
                                    else
                                    {
                                        throw innerException;
                                    }
                                }
                            }
                        }
                        else
                        {
                            lReturnString.Append(lStreamReader.ReadToEnd());
                        }
                    }
                    else
                    {
                        lDeCompressed = new byte[20000];
                        lWorkMemory = new byte[400000];

                        // read the Number of Bytes to receive 
                        lByteBuffer = lBinaryReader.ReadBytes(mintNoOfBytesToRead);
                        // Get the Length of data to be received from the first mintNoOfBytesToRead=4 bits
                        lDataLength = Int16.Parse(Encoding.ASCII.GetString(lByteBuffer, 0, mintNoOfBytesToRead));
                        // Read the Compressed data in Bytes Buffer
                        lByteBuffer = lBinaryReader.ReadBytes(lDataLength);

                        // decompress the data
                        byte[] valuedata = new byte[lDataLength];
                        Infrastructure.Logger.WriteLog("Before Decompress");
                        Array.Copy(lByteBuffer, valuedata, lDataLength);
                        lData = ASCIIEncoding.UTF8.GetString(mobjCompressionHelper.Decompress(valuedata));

                        //lDeCompressedLength = Compressor.CompressUtility.DeCompress(lByteBuffer, lDataLength,  lDeCompressed, lWorkMemory);

                        // Taking the Decompresed data in a string
                        //lReturnString.Append(System.Content.Encoding.ASCII.GetString(lDeCompressed, 0, lDeCompressedLength));
                        lReturnString.Append(lData);
                        Infrastructure.Logger.WriteLog("After Decompress");
                        //System.Console.WriteLine("Length of Compressed String = " + lDataLength );
                        //System.Console.WriteLine("Length of DeCompressed String = " + lDeCompressedLength ) ;
                        //System.Console.WriteLine("ACTUAL DATA = " + lReturnString ) ;
                    }

                    Infrastructure.Logger.WriteLog("Response Received: -> " + lHttpWebRequest.Address.AbsoluteUri.ToString(), true);
                    // Vaibhav : Check for TERMINATING from Server side.
                    if (lReturnString.ToString().ToUpper().IndexOf("TERMINATING") >= 0)
                    {
                        mblnTerminating = true;
                    }


                    if (mblnisOrderBook == false)
                    {
                        if (OnHTTPResponseCompleted != null)
                        {
                            mblnResponseCompleted = true;

                            OnHTTPResponseCompleted(lReturnString.ToString());
                            if (mblnLogHttpSendReceive == true)
                            {
                                ResponseLog(lReturnString.ToString());
                            }
                            return "";
                        }
                    }
                    else
                    {
                        if (OnHTTPOrderResponseCompleted != null)
                        {
                            mblnResponseCompleted = true;

                            OnHTTPOrderResponseCompleted(lReturnString.ToString(), mstrOrderNumForCancel);
                            if (mblnLogHttpSendReceive == true)
                            {
                                ResponseLog(lReturnString.ToString());
                            }
                            return "";
                        }
                    }

                    mblnConnected = false;
                    mblnResponseCompleted = true;
                    if (mblnLogHttpSendReceive == true)
                    {
                        ResponseLog(lReturnString.ToString());
                    }
                    return lReturnString.ToString();
                }
                catch (Exception lException)
                {
                    if (OnHTTPResponseCompleted != null)
                    {
                        mblnResponseCompleted = true;
                        lstrReturnMessage = "1|" + lException.Message;
                    }
                    else if (mblnisOrderBook == true)
                    {
                        if (OnHTTPOrderResponseCompleted != null)
                        {
                            mblnResponseCompleted = true;
                            mblnResponseCompleted = true;
                            lstrReturnMessage = "1|" + lException.Message;
                        }
                    }
                    else
                    {
                        lobjException = lException;
                        Infrastructure.Logger.WriteLog("--------NETWORK EXCEPTION----------");
                        Infrastructure.Logger.WriteLog(lException.ToString());
                        Infrastructure.Logger.WriteLog("--------STACK TRACE----------");
                        Infrastructure.Logger.WriteLog(lException.StackTrace);
                        Infrastructure.Logger.WriteLog("--------MESSAGE----------");
                        Infrastructure.Logger.WriteLog("Exception in HTTPRequest." + lException.Message);
                        Infrastructure.Logger.WriteLog("--------SUGGESTION----------");
                        Infrastructure.Logger.WriteLog("If Unable to connect to remote host. After primary investigation; please check if there is any FireWall blocking the UtilityLoginDetails.GETInstance. Check the Deny Rules on TCP.");
                        Infrastructure.Logger.WriteLog("If Unable to Resolve Host Name. Try and add entry in C:\\Windows\\System32\\drivers\\etc\\hosts file");
                    }
                    SetNewServer();
                }
            }
            // the actual return of  error
            if (OnHTTPResponseCompleted != null)
            {
                mblnResponseCompleted = true;
                OnHTTPResponseCompleted(lstrReturnMessage);
                if (mblnLogHttpSendReceive == true)
                {
                    ResponseLog(lstrReturnMessage);
                }
                return "";
            }
            else if (mblnisOrderBook == true)
            {
                if (OnHTTPOrderResponseCompleted != null)
                {
                    mblnResponseCompleted = true;
                    OnHTTPOrderResponseCompleted(lstrReturnMessage, mstrOrderNumForCancel);
                    if (mblnLogHttpSendReceive == true)
                    {
                        ResponseLog(lstrReturnMessage);
                    }
                    return "";
                }
                else
                { return ""; }
            }
            else
            {
                throw lobjException;
            }
        }

        public string UploadFile(string uploadfile, string url, System.Collections.Specialized.NameValueCollection querystring)
        {
            String fileFormName = "filename";
            String contenttype;
            try
            {
                switch (uploadfile.Substring(uploadfile.Length - 3, 3))
                {
                    case "jpg":
                        contenttype = "image/jpeg";
                        break;
                    case "peg":
                        contenttype = "image/jpeg";
                        break;
                    case "gif":
                        contenttype = "image/gif";
                        break;
                    case "png":
                        contenttype = "image/png";
                        break;
                    case "bmp":
                        contenttype = "image/bmp";
                        break;
                    case "tif":
                        contenttype = "image/tiff";
                        break;
                    case "iff":
                        contenttype = "image/tiff";
                        break;
                    default:
                        contenttype = "image/unknown";
                        break;
                }

                if ((contenttype == null) ||
                    (contenttype.Length == 0))
                {
                    contenttype = "application/octet-stream";
                }

                mstrUrl = mProtocol + mServer + "/" + mServlet;

                string postdata;
                postdata = "?";
                if (querystring != null)
                {
                    foreach (string key in querystring.Keys)
                    {
                        postdata += key + "=" + querystring.Get(key) + "&";
                    }
                }
                Uri uri = new Uri(mstrUrl + postdata);
                WebProxy lWebProxy;

                string boundary = "----------" + DateTime.Now.Ticks.ToString("x");
                HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(uri);
                if (proxyServer != null)
                {
                    if (proxyServer.Trim().Length > 0)
                    {
                        lWebProxy = new WebProxy(proxyServer + ":" + proxyPort, true);
                        if (proxyUserId.Trim().Length > 0)
                        {
                            lWebProxy.Credentials = new NetworkCredential(proxyUserId, proxyUserPassword);
                            webrequest.Credentials = lWebProxy.Credentials;
                        }
                        webrequest.Proxy = lWebProxy;
                    }
                }
                else
                {
                    lWebProxy = System.Net.WebProxy.GetDefaultProxy();
                    if (lWebProxy != null)
                    {
                        if (lWebProxy.Address != null)
                        {
                            if (proxyUserId != null)
                            {
                                if (proxyUserPassword == null)
                                {
                                    proxyUserPassword = "";
                                }
                                lWebProxy.Credentials = new NetworkCredential(proxyUserId, proxyUserPassword);
                                webrequest.Credentials = lWebProxy.Credentials;
                                webrequest.Proxy = lWebProxy;
                            }
                        }
                    }
                }


                webrequest.CookieContainer = new CookieContainer();
                webrequest.ContentType = "multipart/form-data; boundary=" + boundary;
                webrequest.Method = "POST";
                webrequest.ServicePoint.Expect100Continue = IsExpect100Continue;

                // Build up the post message header
                StringBuilder sb = new StringBuilder();
                sb.Append("--");
                sb.Append(boundary);
                sb.Append("\r\n");
                sb.Append("Content-Disposition: form-data; name=\"");
                sb.Append(fileFormName);
                sb.Append("\"; filename=\"");
                sb.Append(Path.GetFileName(uploadfile));
                sb.Append("\"");
                sb.Append("\r\n");
                sb.Append("Content-Type: ");
                sb.Append(contenttype);
                sb.Append("\r\n");
                sb.Append("\r\n");

                string postHeader = sb.ToString();
                byte[] postHeaderBytes = Encoding.UTF8.GetBytes(postHeader);

                // Build the trailing boundary string as a byte array
                // ensuring the boundary appears on a line by itself
                byte[] boundaryBytes =
                       Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

                FileStream fileStream = new FileStream(uploadfile,
                                            FileMode.Open, FileAccess.Read);
                long length = postHeaderBytes.Length + fileStream.Length +
                                                       boundaryBytes.Length;
                webrequest.ContentLength = length;

                Stream requestStream = webrequest.GetRequestStream();

                // Write out our post header
                requestStream.Write(postHeaderBytes, 0, postHeaderBytes.Length);

                // Write out the file contents
                byte[] buffer = new Byte[checked((uint)Math.Min(4096,
                                         (int)fileStream.Length))];
                int bytesRead = 0;
                while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                    requestStream.Write(buffer, 0, bytesRead);

                fileStream.Close();
                // Write out the trailing boundary
                requestStream.Write(boundaryBytes, 0, boundaryBytes.Length);
                requestStream.Flush();
                requestStream.Close();

                WebResponse responce = webrequest.GetResponse();
                Stream s = responce.GetResponseStream();
                StreamReader sr = new StreamReader(s);
                return sr.ReadToEnd();
            }
            catch (Exception ex)
            {
                Infrastructure.Logger.WriteLog("Error while uploading File to Server." + ex.Message);
                Infrastructure.Logger.WriteLog("Error while uploading File to Server." + ex.StackTrace);
                return "1|" + ex.Message;
            }
            return "";
        }

        public string Protocol
        {
            get { return mProtocol; }
            set { mProtocol = value; }
        }
        public static string ServerCurrent
        {
            get { return mServerCurrentStatic; }
        }
        public string Server
        {
            get { return mServer; }
            set
            {
                if (value != null)
                {
                    mServerList = value.Split(char.Parse(","));
                    if (mblnObjectLevel == true)
                    {
                        mintServerCurrentIndex = 0;
                    }
                    else
                    {
                        if (mintLastWorkingServerIndex == -1)
                            mintLastWorkingServerIndex = 0;
                        if (mintLastWorkingServerIndex >= mServerList.Length)
                            mintLastWorkingServerIndex = 0;
                        mintServerCurrentIndex = mintLastWorkingServerIndex;
                    }
                    mServer = mServerList[mintServerCurrentIndex];
                    mServerCurrentStatic = mServer;
                }
            }
        }
        public string Servlet
        {
            get { return mServlet; }
            set { mServlet = value; }
        }

        public string OrderNumberForCancel
        {
            set { mstrOrderNumForCancel = value; }
        }
        public bool isOrderBook
        {
            set { mblnisOrderBook = value; }
        }



        private void SetNewServer()
        {
            if (mblnObjectLevel == true)
            {
                ++mintServerCurrentIndex;
                if (mintServerCurrentIndex >= mServerList.Length)
                    mintServerCurrentIndex = 0;
            }
            else
            {
                ++mintLastWorkingServerIndex;
                if (mintLastWorkingServerIndex >= mServerList.Length)
                    mintLastWorkingServerIndex = 0;
                mintServerCurrentIndex = mintLastWorkingServerIndex;
            }
            mServer = mServerList[mintServerCurrentIndex];
        }
        private void SetLastWorkingServer()
        {
            if (mblnObjectLevel == false && mintLastWorkingServerIndex != mintServerCurrentIndex)
            {
                mintServerCurrentIndex = mintLastWorkingServerIndex;
                mServer = mServerList[mintServerCurrentIndex];
            }
        }

        public String URL
        {
            get { return mstrUrl; }
            set { mstrUrl = value; }
        }
        public String ProxyServer
        {
            get { return proxyServer; }
            set { proxyServer = value; }
        }
        public int ProxyPort
        {
            get { return proxyPort; }
            set { proxyPort = value; }
        }
        public String ProxyUserId
        {
            get { return proxyUserId; }
            set { proxyUserId = value; }
        }
        public String ProxyUserPassword
        {
            get { return proxyUserPassword; }
            set { proxyUserPassword = value; }
        }
        public String Method
        {
            get { return method; }
            set { method = value; }
        }
        public bool SendRedirect
        {
            get { return sendRedirect; }
            set { sendRedirect = value; }
        }

        public static object General { get; private set; }

        public bool setValues(String[] pParameters,
            String[] pValues)
        {
            if (pParameters.Length == pValues.Length)
            {
                parameters = pParameters;
                values = pValues;
                return true;
            }
            return false;
        }
        public static ArrayList convetToString(ArrayList lobjArrayList)
        {
            for (int lCount = 0; lCount < lobjArrayList.Count - 1; lCount++)
            {
                lobjArrayList[lCount] = lobjArrayList[lCount].ToString();
            }
            return lobjArrayList;
        }
        public bool setValues(ArrayList pParamNamesList, ArrayList pParamsValuesList)
        {
            if (pParamNamesList.Count == pParamsValuesList.Count)
            {
                parameters = (string[])pParamNamesList.ToArray(typeof(string));
                try
                {
                    values = (string[])pParamsValuesList.ToArray(typeof(string));
                }
                catch (InvalidCastException)
                {
                    values = (string[])convetToString(pParamsValuesList).ToArray(typeof(string));
                }
                catch (Exception)
                {
                    throw;
                }
                return true;
            }
            return false;
        }
        public void setValues(Hashtable pParameters)
        {
            String lKey;
            String[] lValues;
            IDictionaryEnumerator lDictionaryEnumerator;
            int lTotalParameters;
            if (pParameters != null)
            {
                lDictionaryEnumerator = pParameters.GetEnumerator();
                lTotalParameters = 0;
                while (lDictionaryEnumerator.MoveNext())
                {
                    lKey = (String)lDictionaryEnumerator.Key;
                    lValues = (String[])lDictionaryEnumerator.Value;
                    if (lValues != null)
                        lTotalParameters = lTotalParameters + lValues.Length;
                }
                parameters = new String[lTotalParameters];
                values = new String[lTotalParameters];
                lTotalParameters = 0;
                lDictionaryEnumerator = pParameters.GetEnumerator();
                while (lDictionaryEnumerator.MoveNext())
                {
                    lKey = (String)lDictionaryEnumerator.Key;
                    lValues = (String[])lDictionaryEnumerator.Value;
                    if (lValues != null)
                    {
                        for (int lCount = 0; lCount < lValues.Length; lCount++)
                        {
                            if (lValues[lCount] != null)
                            {
                                parameters[lTotalParameters] = lKey;
                                values[lTotalParameters++] = lValues[lCount];
                            }
                        }
                    }
                }
            }
        }
    }

        #region"RequestResponseData"

    public class MyRequest
    {
        int mintRequestId;
        string mstrURL;
        string mstrServerName;
        string mstrServletName;
        string mstrMethodCalled;
        string mDateTime;
        System.Collections.ArrayList mobjParameterNames = new System.Collections.ArrayList();
        System.Collections.ArrayList mobjParameterValues = new System.Collections.ArrayList();

        public string TimeOfRequest
        {
            set { mDateTime = value; }
            get { return mDateTime; }
        }

        public int RequestId
        {
            set { mintRequestId = value; }
            get { return mintRequestId; }
        }
        public string URL
        {
            set { mstrURL = value; }
            get { return mstrURL; }
        }
        public string ServerName
        {
            set { mstrServerName = value; }
            get { return mstrServerName; }
        }
        public string ServletName
        {
            set { mstrServletName = value; }
            get { return mstrServletName; }
        }
        public string MethodCalled
        {
            set { mstrMethodCalled = value; }
            get { return mstrMethodCalled; }
        }

        public System.Collections.ArrayList ParameterNames
        {
            set { mobjParameterNames = value; }
            get { return mobjParameterNames; }
        }
        public System.Collections.ArrayList ParameterValues
        {
            set { mobjParameterValues = value; }
            get { return mobjParameterValues; }
        }
        public override string ToString()
        {
            return mintRequestId + " : " + mstrServletName + " : " + mDateTime;
        }
    }


    public class MyResponse
    {
        int mintRequestId;
        string mstrServerName;
        string mstrServletName;
        string mstrMethodCalled;
        string mstrResultString;
        string mDateTime;

        public string TimeOfResponse
        {
            set { mDateTime = value; }
            get { return mDateTime; }
        }
        public int RequestId
        {
            set { mintRequestId = value; }
            get { return mintRequestId; }
        }
        public string ServerName
        {
            set { mstrServerName = value; }
            get { return mstrServerName; }
        }
        public string ServletName
        {
            set { mstrServletName = value; }
            get { return mstrServletName; }
        }
        public string MethodCalling
        {
            set { mstrMethodCalled = value; }
            get { return mstrMethodCalled; }
        }
        public string ResultString
        {
            set { mstrResultString = value; }
            get { return mstrResultString; }
        }

        public override string ToString()
        {
            return mintRequestId + " : " + mstrServletName;
        }

    }


        #endregion

        #region "RequestResponseHash"

    public class MyRequestHash : System.Collections.Hashtable
    {
        public void AddToHash(int pintKey, MyRequest pobjMyRequest)
        {
            this.Add(pintKey, pobjMyRequest);
        }

        public System.Collections.ArrayList GetAllKeys()
        {
            System.Collections.ArrayList lobjKeys = new System.Collections.ArrayList();
            System.Collections.ICollection lobjKeyCollection;
            lobjKeyCollection = this.Keys;

            foreach (int lintTemp in lobjKeyCollection)
            {
                lobjKeys.Add(lintTemp);
            }
            return lobjKeys;
        }


        public bool Contains(int pintKey)
        {
            return this.Contains(pintKey);
        }

        public MyRequest RequestIdValue(int pintKey)
        {
            int lintRequestKey;
            MyRequest lobjMyRequest;
            System.Collections.IEnumerator lobjEnumertor;
            lobjEnumertor = this.GetEnumerator();
            while (lobjEnumertor.MoveNext())
            {
                lintRequestKey = ((MyRequest)((System.Collections.DictionaryEntry)(lobjEnumertor.Current)).Value).RequestId;
                if (lintRequestKey == (pintKey))
                {
                    lobjMyRequest = (MyRequest)((System.Collections.DictionaryEntry)(lobjEnumertor.Current)).Value;
                    return (lobjMyRequest);
                }
            }
            return null;
        }


        public ArrayList RequestOnServletName(string pstrServletName)
        {
            MyRequest lobjMyRequest;
            string lstrServletName;
            ArrayList lobjRequest;
            System.Collections.IDictionaryEnumerator lobjEnumertor;

            lobjRequest = new ArrayList();
            lobjEnumertor = this.GetEnumerator();

            while (lobjEnumertor.MoveNext())
            {
                lstrServletName = ((MyRequest)((System.Collections.DictionaryEntry)(lobjEnumertor.Current)).Value).ServletName;
                if (lstrServletName.Trim().ToUpper() == pstrServletName.Trim().ToUpper())
                {
                    lobjMyRequest = (MyRequest)((System.Collections.DictionaryEntry)(lobjEnumertor.Current)).Value;
                    lobjRequest.Add(lobjMyRequest);
                }
            }
            return lobjRequest;
        }
    }

    public class MyResponseHash : System.Collections.Hashtable
    {
        public void AddToHash(int pintKey, MyResponse pobjMyResponse)
        {
            this.Add(pintKey, pobjMyResponse);
        }

        public System.Collections.ArrayList GetAllKeys()
        {
            System.Collections.ArrayList lobjKeys = new System.Collections.ArrayList();
            System.Collections.ICollection lobjKeyCollection;
            lobjKeyCollection = this.Keys;

            foreach (int lintTemp in lobjKeyCollection)
            {
                lobjKeys.Add(lintTemp);
            }
            return lobjKeys;
        }

        public bool Contains(int pintKey)
        {
            return this.Contains(pintKey);
        }

        public MyResponse RequestIdValue(int pintKey)
        {
            int lintRequestKey;
            MyResponse lobjMyResponse;
            System.Collections.IEnumerator lobjEnumertor;
            lobjEnumertor = this.GetEnumerator();
            while (lobjEnumertor.MoveNext())
            {
                lintRequestKey = ((MyResponse)((System.Collections.DictionaryEntry)(lobjEnumertor.Current)).Value).RequestId;
                if (lintRequestKey == (pintKey))
                {
                    lobjMyResponse = (MyResponse)((System.Collections.DictionaryEntry)(lobjEnumertor.Current)).Value;
                    return (lobjMyResponse);
                }
            }
            return null;
        }

        public ArrayList ResponseOnServletName(string pstrServletName)
        {
            MyResponse lobjMyResponse;
            string lstrServletName;
            ArrayList lobjResponse;
            System.Collections.IDictionaryEnumerator lobjEnumertor;

            lobjResponse = new ArrayList();
            lobjEnumertor = this.GetEnumerator();

            while (lobjEnumertor.MoveNext())
            {
                lstrServletName = ((MyResponse)((System.Collections.DictionaryEntry)(lobjEnumertor.Current)).Value).ServletName;
                if (lstrServletName.Trim().ToUpper() == pstrServletName.Trim().ToUpper())
                {
                    lobjMyResponse = (MyResponse)((System.Collections.DictionaryEntry)(lobjEnumertor.Current)).Value;
                    lobjResponse.Add(lobjMyResponse);
                }
            }
            return lobjResponse;
        }


    }


        #endregion

    public enum CertificateProblem : long
    {
        CertEXPIRED = 0x800B0101,
        CertVALIDITYPERIODNESTING = 0x800B0102,
        CertROLE = 0x800B0103,
        CertPATHLENCONST = 0x800B0104,
        CertCRITICAL = 0x800B0105,
        CertPURPOSE = 0x800B0106,
        CertISSUERCHAINING = 0x800B0107,
        CertMALFORMED = 0x800B0108,
        CertUNTRUSTEDROOT = 0x800B0109,
        CertCHAINING = 0x800B010A,
        CertREVOKED = 0x800B010C,
        CertUNTRUSTEDTESTROOT = 0x800B010D,
        CertREVOCATION_FAILURE = 0x800B010E,
        CertCN_NO_MATCH = 0x800B010F,
        CertWRONG_USAGE = 0x800B0110,
        CertUNTRUSTEDCA = 0x800B0112
    }

    public class MyCertificateValidation : ICertificatePolicy
    {
        // Default policy for certificate validation.
        // Allowing the certificate even if there is an error. Done so as to let the user dispaly SSL setting in the certifiacate page.
        public static bool DefaultValidate = true;
        private X509Certificate mobjCertificate;
        public X509Certificate Certificate { get { return mobjCertificate; } set { mobjCertificate = value; } }

        public bool CheckValidationResult(ServicePoint sp, X509Certificate cert, HttpWebRequest request, int problem)
        {
            return CheckValidationResult(sp, cert, (WebRequest)request, problem);
        }

        private String GetProblemMessage(CertificateProblem Problem)
        {
            String ProblemMessage = "";
            CertificateProblem problemList = new CertificateProblem();
            String ProblemCodeName = Enum.GetName(problemList.GetType(), Problem);
            if (ProblemCodeName != null)
                ProblemMessage = ProblemMessage + "-Certificateproblem:" + ProblemCodeName;
            else
                ProblemMessage = "Unknown Certificate Problem";
            return ProblemMessage;
        }

        public bool CheckValidationResult(ServicePoint srvPoint, X509Certificate certificate, WebRequest request, int certificateProblem)
        {
            bool ValidationResult = false;
            try
            {
                Infrastructure.Logger.WriteLog("Certificate Problem with accessing " + request.RequestUri);
                Infrastructure.Logger.WriteLog("Problem code :-" + certificateProblem);
                Infrastructure.Logger.WriteLog(GetProblemMessage((CertificateProblem)certificateProblem));
            }
            catch (Exception ex) { }
            ValidationResult = DefaultValidate;
            return ValidationResult;
        }
#endif
    }



}
