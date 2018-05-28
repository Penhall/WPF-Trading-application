using LZ4Sharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CommonFrontEnd.SocketConnection
{
    /// <summary>
    /// This class is used to pass the details of the Message arrived
    /// to the class which will handle the Message.
    /// </summary>

    public enum SocketErrorCodes
    {
        InterruptedFunctionCall = 10004,
        PermissionDenied = 10013,
        BadAddress = 10014,
        InvalidArgument = 10022,
        TooManyOpenFiles = 10024,
        ResourceTemporarilyUnavailable = 10035,
        OperationNowInProgress = 10036,
        OperationAlreadyInProgress = 10037,
        SocketOperationOnNonSocket = 10038,
        DestinationAddressRequired = 10039,
        MessgeTooLong = 10040,
        WrongProtocolType = 10041,
        BadProtocolOption = 10042,
        ProtocolNotSupported = 10043,
        SocketTypeNotSupported = 10044,
        OperationNotSupported = 10045,
        ProtocolFamilyNotSupported = 10046,
        AddressFamilyNotSupported = 10047,
        AddressInUse = 10048,
        AddressNotAvailable = 10049,
        NetworkIsDown = 10050,
        NetworkIsUnreachable = 10051,
        NetworkReset = 10052,
        ConnectionAborted = 10053,
        ConnectionResetByPeer = 10054,
        NoBufferSpaceAvailable = 10055,
        AlreadyConnected = 10056,
        NotConnected = 10057,
        CannotSendAfterShutdown = 10058,
        ConnectionTimedOut = 10060,
        ConnectionRefused = 10061,
        HostIsDown = 10064,
        HostUnreachable = 10065,
        TooManyProcesses = 10067,
        NetworkSubsystemIsUnavailable = 10091,
        UnsupportedVersion = 10092,
        NotInitialized = 10093,
        ShutdownInProgress = 10101,
        ClassTypeNotFound = 10109,
        HostNotFound = 11001,
        HostNotFoundTryAgain = 11002,
        NonRecoverableError = 11003,
        NoDataOfRequestedType = 11004,
        SIZE = 43
    }

    public class MessageArrivedEventArgs : EventArgs
    {
        private string strMessageString;
        private int strMessageLength;
        private string strMessageCode;
        private bool mReceiver;
        private char lstrDataToRemove;

        public MessageArrivedEventArgs(string pMessageString)
        {
            lstrDataToRemove = Convert.ToChar(32);
            strMessageString = pMessageString.Trim();
            strMessageLength = strMessageString.Length;
            if (strMessageLength > 2)
            {
                if (MessageString.IndexOf("|") >= 0)
                {
                    strMessageCode = strMessageString.Substring(0, MessageString.IndexOf("|")).Trim();
                }
                else
                {
                    strMessageCode = "78";
                    strMessageString = "78|" + strMessageString;
                }
            }
            //pranav
            //			if(SocketMessagesCollection.LogSocketMessages == true)
            //			{
            //				WriteToLog("Message Arrived : -> " + strMessageString + " Time :->" + DateTime.Now.ToString());
            //				//SocketMessagesCollection.AddMessagesToArrayList(pMessageString);
            //			}
        }
        public string MessageString
        {
            get { return strMessageString; }

        }
        public int MessageLength
        {
            get { return strMessageLength; }
        }
        public string MessageCode
        {
            get { return strMessageCode; }
        }
        public bool Receiver
        {
            set { mReceiver = value; }
        }

    }


    //public delegate void MessageArrivedEventHandler(object pSender, MessageArrivedEventArgs pMessageArrivedEventArgs);
    public delegate void MessageArrivedEventHandler(string pData);
    /// <summary>
    /// This class is a wrapper class for Socket Connections
    /// to be established by the application.
    /// This class should be given the type of Socket connection
    /// to be established, the IP Address of the server and Port
    /// on which the connection should be established.
    /// This class can also send any message to the server once
    /// connected.
    /// This class can then wait for data to arrive from the server
    /// and when data is received from the server it raises an event
    /// to notify the listener.
    /// </summary>

    public class SocketHelper
    {
        #region " VARIABLE DECLARATION"
        private string socketType;
        private string serverAddress;
        private string serverPort;
        // List of IPs and Ports
        private string[] serverAddressList;
        private string[] serverPortList;
        private int mintServerIPPortListCount;
        private int mintServerListCurrentIndex = -1;

        private int localPort;
        private TcpClient tcpClient = null;
        private XUDPClient udpClient = null;
        private Socket socket;
        private NetworkStream tcpStream;
        private bool mStop;
        private bool mConnectedToServer;
        private bool mMarketWatchSent;
        private IPEndPoint serverEndPoint = null;
        private int mTimeOut;
        private int mConnectTimeOut;
        public string udpClientData;
        private string tcpClientData;
        public string message;
        public int sleepTime;
        private string mTCPMessage = "";
        private long mConnectTime;
        private long mStreamTime;
        //private string mMaxId;
        private Hashtable mDownloadStatus;
        private static Hashtable mobjErrorCodeHash;
        private object mobjLockObject = new object();
        // For Compression
        private LZ4Decompressor32 mobjCompressionHelper;
        private int lDeCompressedLength;
        private byte[] lWorkMemory;
        private byte[] lDecompressed;
        // Code for string encoding / decoding
        string mCompression = "";
        private int mintNoOfBytesToRead = 4;
        private int mInitializeCount;

        // For Debugging - Testing
        private long mlngBytesSent;
        private long mlngBytesReceived;
        private DateTime mdtLastConnectionStartTime;

        //private long mNoOfMessages = 0;


        // Compression Types
        public static string Compression_Compressed = "Y";
        public static string Compression_Encoded = "E";
        public static string Compression_CompressedEncoded = "YE";
        public static string Compression_Normal = "";

        private System.DateTime lLastPacketSendOrReceiveTime;

        // for Configuring the server pooling frequency (in secs.) default is 1 sec
        private int mintPoolFrequency = 10;
        #endregion

        #region " PROPERTIES "
        public string Name { get; set; }

        public int TimeOut
        {
            get { return mTimeOut; }
            set { mTimeOut = value; }
        }
        public int ConnectTimeOut
        {
            get { return mConnectTimeOut; }
            set { mConnectTimeOut = value; }
        }
        public bool ConnectedToServer
        {
            get { return mConnectedToServer; }
        }
        public string SocketType
        {
            get { return socketType; }
            set { socketType = value; }
        }
        public int TotalServerCount
        {
            get { return mintServerIPPortListCount; }
        }
        public int CurrentServerIndex
        {
            get
            {
                if (mintServerListCurrentIndex == -1)
                    return 1;
                else
                    return mintServerListCurrentIndex + 1;
            }
        }
        public string ServerAddress
        {
            get { return serverAddress; }
            set
            {
                int lintCount = 0;
                string[] lTemp;
                serverAddressList = value.Split(char.Parse(","));
                // if the IP and Port are received in the Address eg. IP:Port
                if (serverAddressList[0].IndexOf(":", 0, serverAddressList[0].Length - 1) > 0)
                {
                    serverPortList = new string[serverAddressList.Length];
                    for (lintCount = 0; lintCount <= serverAddressList.Length - 1; lintCount++)
                    {
                        lTemp = serverAddressList[lintCount].Split(char.Parse(":"));
                        if (lTemp.Length > 1)
                        {
                            serverAddressList[lintCount] = lTemp[0];
                            serverPortList[lintCount] = lTemp[1];
                        }
                    }
                    serverPort = serverPortList[0];
                    localPort = int.Parse(serverPort);
                }
                else
                {
                    serverPortList = null;
                }
                serverAddress = serverAddressList[0];
                mintServerIPPortListCount = serverAddressList.Length;
                mintServerListCurrentIndex = -1;
            }
        }
        public string ServerPort
        {
            get { return serverPort; }
            set
            {
                if (value != null)
                {
                    if (value.Length > 0)
                    {
                        string[] lTemp;
                        lTemp = value.Split(char.Parse(","));
                        if (lTemp.Length == serverAddressList.Length)
                        {
                            serverPortList = value.Split(char.Parse(","));
                            serverPort = serverPortList[0];
                            localPort = int.Parse(serverPort);
                        }
                    }
                }
            }
        }
        public int LocalPort
        {
            get { return localPort; }
            // The local port will get set automatically when the ServerIP is set
            set { localPort = value; }
        }
        public string UDPClientData
        {
            get { return udpClientData; }
            set { udpClientData = value; }
        }
        public string TCPClientData
        {
            get { return tcpClientData; }
            set { tcpClientData = value; }
        }
        public bool MarketWatchSent
        {
            get { return mMarketWatchSent; }
            set { mMarketWatchSent = value; }
        }
        public long BytesSent
        {
            //set{ mlngBytesSent = value;}
            get { return mlngBytesSent; }
        }
        public long BytesReceived
        {
            //set{ mlngBytesReceived = value;}
            get { return mlngBytesReceived; }
        }

        public DateTime LastConnectionStartTime
        {
            get { return mdtLastConnectionStartTime; }
        }

        public string Compression
        {
            set
            {
                if ((value == Compression_Normal) || (value == Compression_Encoded) || (value == Compression_Compressed) || (value == Compression_CompressedEncoded))
                {
                    mCompression = value;
                }
                else
                {
                    mCompression = Compression_Normal;
                }
            }
            get { return mCompression; }
        }

        //		public string MaxId
        //		{
        //			get{return mMaxId;}
        //			set{mMaxId = value;}
        //		}
        public int PoolFrequency
        {
            set { mintPoolFrequency = value; }
            get { return mintPoolFrequency; }
        }
        public bool NextServerInList()
        {
            if (mintServerIPPortListCount > 0)
            {
                if (mintServerIPPortListCount == 1)
                {
                    return true;
                }
                else if (mintServerIPPortListCount > 1)
                {
                    mintServerListCurrentIndex += 1;
                    if (mintServerListCurrentIndex + 1 > mintServerIPPortListCount)
                    {
                        mintServerListCurrentIndex = 0;
                    }
                    serverAddress = serverAddressList[mintServerListCurrentIndex];

                    if (serverPortList != null)
                    {
                        serverPort = serverPortList[mintServerListCurrentIndex];
                        localPort = int.Parse(serverPort);
                    }
                    WriteToLog("**************************************", true);
                    WriteToLog("Name = " + Name, true);
                    WriteToLog("mintServerIPPortListCount = " + mintServerIPPortListCount, true);
                    WriteToLog("mintServerListCurrentIndex = " + mintServerListCurrentIndex, true);
                    WriteToLog("serverAddress = " + serverAddress, true);
                    if (serverPort != null)
                    {
                        WriteToLog("serverPort = " + serverPort, true);
                    }
                    WriteToLog("**************************************", true);
                    return true;
                }
            }
            return false;
        }
        #endregion

        public SocketHelper()
        {
            mlngBytesReceived = 0;
            mlngBytesSent = 0;
            mDownloadStatus = new Hashtable();
        }
        /// <summary>
        /// This is the constructor which initializes everything is
        /// required for the communication.
        /// </summary>
        /// <param name="pSocketType">The type of Socket that should be
        /// used to establish the connection. The valid values are TCP,
        /// UDPSEND, UDPRECEIVE, MULTICAST, TCPSERVER, TCPCLIENT.</param>
        /// <param name="pServerAddress">The IP Address of of the Server to
        /// connect to.</param>
        /// <param name="pServerPort">The port on which to connect.</param>
        /// <param name="pLocalPort">The Port Local on the Local machine to which to bind Machine.</param>
        /// <param name="pConnectTimeOut">The time for which we must wait for the server to respond at the
        /// time of connect.</param>
        /// <param name="pTimeOut">The time for which we must wait for the server to respond at the time
        /// of receiving after connection is established.</param>
        public SocketHelper(string pSocketType, string pServerAddress, string pServerPort, int pLocalPort, int pConnectTimeOut, int pTimeOut)
        {
            mTimeOut = pTimeOut;
            mConnectTimeOut = pConnectTimeOut;
            mConnectedToServer = false;
            socketType = pSocketType.Trim().ToUpper();
            // Passing the Parameter to the Property, for generating the List
            serverAddress = pServerAddress;
            serverPort = pServerPort;
            //
            localPort = pLocalPort;
            //initialize(pSocketType, pServerAddress, pServerPort, pLocalPort);
            mDownloadStatus = new Hashtable();
        }
        private void InitializeErrorCodeHash()
        {
            mobjErrorCodeHash = new Hashtable();
            mobjErrorCodeHash.Add(SocketErrorCodes.AddressFamilyNotSupported, "AddressFamilyNotSupported");
            mobjErrorCodeHash.Add(SocketErrorCodes.AddressInUse, "AddressInUse");
            mobjErrorCodeHash.Add(SocketErrorCodes.AddressNotAvailable, "AddressNotAvailable");
            mobjErrorCodeHash.Add(SocketErrorCodes.AlreadyConnected, "AlreadyConnected");
            mobjErrorCodeHash.Add(SocketErrorCodes.BadAddress, "BadAddress");
            mobjErrorCodeHash.Add(SocketErrorCodes.BadProtocolOption, "BadProtocolOption");
            mobjErrorCodeHash.Add(SocketErrorCodes.CannotSendAfterShutdown, "CannotSendAfterShutdown");
            mobjErrorCodeHash.Add(SocketErrorCodes.ClassTypeNotFound, "ClassTypeNotFound");
            mobjErrorCodeHash.Add(SocketErrorCodes.ConnectionAborted, "ConnectionAborted");
            mobjErrorCodeHash.Add(SocketErrorCodes.ConnectionRefused, "ConnectionRefused");
            mobjErrorCodeHash.Add(SocketErrorCodes.ConnectionResetByPeer, "ConnectionResetByPeer");
            mobjErrorCodeHash.Add(SocketErrorCodes.ConnectionTimedOut, "ConnectionTimedOut");
            mobjErrorCodeHash.Add(SocketErrorCodes.DestinationAddressRequired, "DestinationAddressRequired");
            mobjErrorCodeHash.Add(SocketErrorCodes.HostIsDown, "HostIsDown");
            mobjErrorCodeHash.Add(SocketErrorCodes.HostNotFound, "HostNotFound");
            mobjErrorCodeHash.Add(SocketErrorCodes.HostNotFoundTryAgain, "HostNotFoundTryAgain");
            mobjErrorCodeHash.Add(SocketErrorCodes.HostUnreachable, "HostUnreachable");
            mobjErrorCodeHash.Add(SocketErrorCodes.InterruptedFunctionCall, "InterruptedFunctionCall");
            mobjErrorCodeHash.Add(SocketErrorCodes.InvalidArgument, "InvalidArgument");
            mobjErrorCodeHash.Add(SocketErrorCodes.MessgeTooLong, "MessgeTooLong");
            mobjErrorCodeHash.Add(SocketErrorCodes.NetworkIsDown, "NetworkIsDown");
            mobjErrorCodeHash.Add(SocketErrorCodes.NetworkIsUnreachable, "NetworkIsUnreachable");
            mobjErrorCodeHash.Add(SocketErrorCodes.NetworkReset, "NetworkReset");
            mobjErrorCodeHash.Add(SocketErrorCodes.NetworkSubsystemIsUnavailable, "NetworkSubsystemIsUnavailable");
            mobjErrorCodeHash.Add(SocketErrorCodes.NoBufferSpaceAvailable, "NoBufferSpaceAvailable");
            mobjErrorCodeHash.Add(SocketErrorCodes.NoDataOfRequestedType, "NoDataOfRequestedType");
            mobjErrorCodeHash.Add(SocketErrorCodes.NonRecoverableError, "NonRecoverableError");
            mobjErrorCodeHash.Add(SocketErrorCodes.NotConnected, "NotConnected");
            mobjErrorCodeHash.Add(SocketErrorCodes.NotInitialized, "NotInitialized");
            mobjErrorCodeHash.Add(SocketErrorCodes.OperationAlreadyInProgress, "OperationAlreadyInProgress");
            mobjErrorCodeHash.Add(SocketErrorCodes.OperationNotSupported, "OperationNotSupported");
            mobjErrorCodeHash.Add(SocketErrorCodes.OperationNowInProgress, "OperationNowInProgress");
            mobjErrorCodeHash.Add(SocketErrorCodes.PermissionDenied, "PermissionDenied");
            mobjErrorCodeHash.Add(SocketErrorCodes.ProtocolFamilyNotSupported, "ProtocolFamilyNotSupported");
            mobjErrorCodeHash.Add(SocketErrorCodes.ProtocolNotSupported, "ProtocolNotSupported");
            mobjErrorCodeHash.Add(SocketErrorCodes.ResourceTemporarilyUnavailable, "ResourceTemporarilyUnavailable");
            mobjErrorCodeHash.Add(SocketErrorCodes.ShutdownInProgress, "ShutdownInProgress");
            mobjErrorCodeHash.Add(SocketErrorCodes.SocketOperationOnNonSocket, "SocketOperationOnNonSocket");
            mobjErrorCodeHash.Add(SocketErrorCodes.SocketTypeNotSupported, "SocketTypeNotSupported");
            mobjErrorCodeHash.Add(SocketErrorCodes.TooManyOpenFiles, "TooManyOpenFiles");
            mobjErrorCodeHash.Add(SocketErrorCodes.TooManyProcesses, "TooManyProcesses");
            mobjErrorCodeHash.Add(SocketErrorCodes.UnsupportedVersion, "UnsupportedVersion");
            mobjErrorCodeHash.Add(SocketErrorCodes.WrongProtocolType, "WrongProtocolType");

        }
        public void initialize()
        {
            NextServerInList();
            initialize(socketType, serverAddress, serverPort, localPort);
        }

        public void initialize(string pSocketType, string pServerAddress, string pServerPort, int pLocalPort)
        {
            if (mlngBytesReceived > 0 || mlngBytesSent > 0)
            {
                mlngBytesReceived = 0;
                mlngBytesSent = 0;
            }
            string lTestString = udpClientData;
            IPHostEntry lRemoteHostEntry = null;
            IPEndPoint lAnyRemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
            try
            {
                lock (mobjLockObject)
                {
                    if (mobjErrorCodeHash == null)
                    {
                        InitializeErrorCodeHash();
                    }
                }
                //Compressor.CompressUtility.LZOInit();
                mobjCompressionHelper = new LZ4Decompressor32();
                if (lTestString == null) lTestString = "";
                socketType = pSocketType.Trim().ToUpper();
                serverAddress = pServerAddress;
                serverPort = pServerPort;
                localPort = pLocalPort;
                lDecompressed = new byte[4000];
                lWorkMemory = new byte[4000];
                if (socketType.Equals("UDPSEND"))
                {
                    if (pServerAddress != null)
                    {
                        lRemoteHostEntry = Dns.GetHostByName(pServerAddress);
                        serverEndPoint = new IPEndPoint(lRemoteHostEntry.AddressList[0], int.Parse(serverPort));
                    }
                    else
                    {
                        serverEndPoint = new IPEndPoint(IPAddress.Any, 0);
                    }
                }
                try
                {
                    if (socketType == "UDPSEND")
                    {
                        if (serverAddress != null)
                        {
                            udpClient = new XUDPClient(serverAddress, int.Parse(serverPort));
                            udpClient.setReceiveTimeOut(mTimeOut);
                            mConnectedToServer = true;
                        }
                        else
                        {
                            WriteToLog("1: " + System.DateTime.Now + " " + "No Details available to connect to server", true);
                        }
                    }
                    else if (socketType == "UDPRECEIVE")
                    {
                        if (pLocalPort <= 0)
                        {
                            serverEndPoint = lAnyRemoteIpEndPoint;
                            udpClient = new XUDPClient(lAnyRemoteIpEndPoint);
                        }
                        else
                        {
                            serverEndPoint = lAnyRemoteIpEndPoint;
                            udpClient = new XUDPClient(pLocalPort);
                        }
                        udpClient.setReceiveTimeOut(mTimeOut);
                        mConnectedToServer = true;
                    }
                    else if (socketType == "MULTICAST")
                    {
                        IPAddress lMulticastIPAddress = IPAddress.Parse(serverAddress);
                        if (pServerAddress != null)
                        {
                            lRemoteHostEntry = Dns.GetHostByName(pServerAddress);
                            serverEndPoint = new IPEndPoint(lRemoteHostEntry.AddressList[0], int.Parse(serverPort));
                        }
                        else
                        {
                            serverEndPoint = new IPEndPoint(IPAddress.Any, 0);
                        }
                        udpClient = new XUDPClient(pLocalPort);
                        udpClient.setReceiveTimeOut(mTimeOut);
                        udpClient.JoinMulticastGroup(lMulticastIPAddress);
                        mConnectedToServer = true;
                    }
                    else if (socketType == "TCP" || socketType == "TCPCLIENT")
                    {
                        try
                        {
                            // Precaution as some times there are multiple connections estabilished.
                            // for better precaution we can even send disconnect message prior to this.
                            // this is done so as to stop multilple connection over different ports.
                            if (tcpStream != null)
                            {
                                tcpStream.Close();
                                tcpStream = null;
                            }
                            if (tcpClient != null)
                            {
                                tcpClient.Close();
                                tcpClient = null;
                            }
                        }
                        catch (Exception e)
                        {
                            //pranav
                            WriteToLog("Initialize", true);
                            WriteToLog(e.StackTrace, true);
                            WriteToLog(e.Message, true);
                        }
                        long lStartTicks = System.DateTime.Now.Ticks;
                        tcpClient = new TcpClient();
                        tcpClient.ReceiveTimeout = mTimeOut;
                        tcpClient.SendTimeout = mTimeOut;
                        tcpClient.NoDelay = true;
                        tcpClient.LingerState = new System.Net.Sockets.LingerOption(false, 0);
                        try
                        {
                            tcpClient.Connect(serverAddress, int.Parse(serverPort));
                            mConnectTime = System.DateTime.Now.Ticks - lStartTicks;
                            tcpStream = tcpClient.GetStream();
                            mStreamTime = System.DateTime.Now.Ticks - lStartTicks;
                            mConnectedToServer = true;
                        }
                        catch (Exception e)
                        {
                            //pranav
                            WriteToLog("Initialize  1", true);
                            WriteToLog(e.StackTrace, true);
                            WriteToLog(e.Message, true);
                            WriteToLog("Exception While Trying To Connect Over Tcp" + e.Message, true);
                            closeAll(11);
                            return;
                        }

                    }
                    else if (socketType == "TCPSERVER")
                    {
                    }
                }
                catch (SocketException lSocketException)
                {
                    //pranav
                    WriteToLog("Initialize    2", true);
                    WriteToLog(lSocketException.StackTrace, true);
                    WriteToLog(lSocketException.Message, true);
                    if (lSocketException.ErrorCode != (int)SocketErrorCodes.ConnectionTimedOut)
                    {
                        if (mobjErrorCodeHash[lSocketException.ErrorCode] != null)
                            WriteToLog("Socket Error CODE : -- " + lSocketException.ErrorCode + " DISCRIPTION FROM ENUM : --" + mobjErrorCodeHash[lSocketException.ErrorCode], true);
                        else
                            WriteToLog("Socket Error CODE : -- " + lSocketException.ErrorCode, true);

                        WriteToLog("", true);
                        WriteToLog("2:  initialize " + lSocketException.Message + " " + serverAddress + " " + serverPort, true);
                        WriteToLog(lSocketException.StackTrace, true);
                        WriteToLog(lSocketException.Message, true);
                    }
                    else
                    {
                        mConnectedToServer = true;
                        if (udpClient != null)
                            udpClient.setReceiveTimeOut(mTimeOut);
                    }
                }
                catch (Exception lException)
                {
                    //pranav
                    WriteToLog("Initialize    3", true);
                    WriteToLog(lException.StackTrace, true);
                    WriteToLog(lException.Message, true);

                    WriteToLog("3:  initialize " + lException.Message + " " + serverAddress + " " + serverPort, true);
                    WriteToLog(lException.StackTrace, true);
                    WriteToLog(lException.Message, true);
                }
            }
            catch (Exception lException)
            {
                //pranav
                WriteToLog("Initialize  4", true);
                WriteToLog(lException.StackTrace, true);
                WriteToLog(lException.Message, true);

                WriteToLog("4: " + System.DateTime.Now + " " + lException.Message, true);
                WriteToLog(lException.StackTrace, true);
                WriteToLog(lException.Message, true);
            }
            WriteToLog(Name + " " + System.DateTime.Now + " Initialized");
        }

        public bool send(string pMessage, Boolean pblnSendWithoutTrim = false)
        {
            bool lSuccess = true;
            lock (mobjLockObject)
            {
                // Fail Safe
                if (pMessage == null || pMessage.Trim().Length == 0)
                {
                    pMessage = "\n";
                }
                else
                {
                    if (pblnSendWithoutTrim == false)
                        pMessage = pMessage.Trim();
                }


                try
                {
                    if (udpClient != null)
                    {
                        udpClient.Send(Encoding.ASCII.GetBytes(pMessage), pMessage.Length);
                    }
                    else if (tcpClient != null)
                    {
                        if (tcpStream != null)
                        {
                            //For TCP interface add a return character at the end of the message.
                            if (!pMessage.Substring(pMessage.Length - 1).Equals("\n"))
                                pMessage = pMessage + "\n";

                            tcpStream.Write(Encoding.ASCII.GetBytes(pMessage), 0, pMessage.Length);
                            tcpStream.Flush();
                        }
                    }
                    else if (socket != null)
                    {
                        //For TCP interface add a return character at the end of the message.
                        //WriteToLog("5a: " + System.DateTime.Now + " send ");
                        pMessage = pMessage + "\n";
                        socket.Send(Encoding.ASCII.GetBytes(pMessage), 0, pMessage.Length, System.Net.Sockets.SocketFlags.None);
                    }
                    //pranav
                    //if (SocketMessagesCollection.LogSocketMessages == true)
                    //{
                    if (!pMessage.Equals("\n")) WriteToLog("Message Sending  : -> " + pMessage.Replace("\n", ""), true);
                    //}
                    mlngBytesSent += pMessage.Length;
                    lLastPacketSendOrReceiveTime = System.DateTime.Now;
                }
                catch (Exception lException)
                {
                    //pranav
                    WriteToLog("Sending message :- " + pMessage, true);
                    WriteToLog(lException.StackTrace, true);
                    WriteToLog(lException.Message, true);

                    WriteToLog("5:  send " + lException.Message + " " + serverAddress + " " + serverPort, true);
                    WriteToLog(lException.StackTrace, true);
                    WriteToLog(lException.Message, true);
                    closeAll(5);
                    mConnectedToServer = false;
                    mStop = false;
                    lSuccess = false;
                }
            }
            //if (pMessage.Length > 2)
            // WriteToLog("Sending : " + pMessage);
            return lSuccess;
        }
        public void receive()
        {
            byte[] lBuffer = null;
            string lStringBuffer = null;
            int lNumberOfBytesReceived = 0;
            try
            {
                if (tcpClient != null)
                {
                    lBuffer = new byte[1024];
                }
                try
                {
                    if (udpClient != null)
                    {
                        //WriteToLog (Name + " receive UDPClient Hashcode "  + udpClient.GetHashCode());
                        lBuffer = udpClient.Receive(ref serverEndPoint);
                        lNumberOfBytesReceived = lBuffer.Length;
                    }
                    else if (tcpClient != null)
                    {
                        //WriteToLog("Going to receive as TCPClient");
                        lNumberOfBytesReceived = tcpStream.Read(lBuffer, 0, lBuffer.Length);
                    }
                    mlngBytesReceived += lNumberOfBytesReceived;
                    if (lBuffer != null)
                        lStringBuffer = Encoding.ASCII.GetString(lBuffer, 0, lNumberOfBytesReceived);
                    //WriteToLog(" Received from server " + lStringBuffer);
                    //Raise an event to notify that a message has been received.
                    if (lStringBuffer != null)
                    {
                        if (lStringBuffer.Length > 0)
                        {
                            lStringBuffer = lStringBuffer.Replace("\n", "/z");
                            onRawMessageArrived(lStringBuffer);
                            lStringBuffer = "";
                        }
                    }
                }
                catch (SocketException lSocketException)
                {
                    if (lSocketException.ErrorCode == (int)SocketErrorCodes.ConnectionTimedOut)
                    {
                        if
                            (udpClient != null) udpClient.setReceiveTimeOut(mTimeOut);
                    }
                    else
                    {
                        if (mobjErrorCodeHash[lSocketException.ErrorCode] != null)
                            WriteToLog("Socket Error CODE : -- " + lSocketException.ErrorCode + " DISCRIPTION FROM ENUM : --" + mobjErrorCodeHash[lSocketException.ErrorCode], true);
                        else
                            WriteToLog("Socket Error CODE : -- " + lSocketException.ErrorCode, true);

                        WriteToLog("6:  receive " + lSocketException.Message + " " + serverAddress, true);
                        WriteToLog(lSocketException.StackTrace, true);
                        WriteToLog(lSocketException.Message, true);
                        closeAll(1);
                    }
                }
                catch (IOException lIOException)
                {
                    //This code is required because in case of receive on TCP we get an IOException instead
                    //of a Socket Exception. The Socket Exception happens to be inner exception.
                    SocketException lSocketException = new SocketException();
                    if (
                        lIOException.InnerException.GetType().Equals(lSocketException.GetType()))
                    {
                        lSocketException = (SocketException)lIOException.InnerException;
                        if (lSocketException.ErrorCode != (int)SocketErrorCodes.ConnectionTimedOut)
                        {
                            if (mobjErrorCodeHash[lSocketException.ErrorCode] != null)
                                WriteToLog("Socket Error CODE : -- " + lSocketException.ErrorCode + " DISCRIPTION FROM ENUM : --" + mobjErrorCodeHash[lSocketException.ErrorCode], true);
                            else
                                WriteToLog("Socket Error CODE : -- " + lSocketException.ErrorCode, true);

                            closeAll(2);
                            WriteToLog(lSocketException.StackTrace, true);
                            WriteToLog(lSocketException.Message, true);
                        }
                    }
                    else
                    {
                        closeAll(3);
                        WriteToLog("7: Exception=" + lIOException, true);
                        WriteToLog(lIOException.StackTrace, true);
                        WriteToLog(lIOException.Message, true);
                    }
                }
            }
            catch (Exception e)
            {
            }
        }

        public void receiveThread()
        {
            byte[] lBuffer = null;
            //string lStringBuffer = null;
            int lNumberOfBytesReceived = 0;
            //int lFoundAt;
            byte[] lDummyMessage = null;
            int lLengthRead = 0;
            int lDataLength = 0;
            string lData = "";
            long lNoBytesBeforeBreak = 0;
            mInitializeCount = 0;
            lLastPacketSendOrReceiveTime = System.DateTime.Now;
            try
            {
                lDecompressed = new byte[25000];
                lWorkMemory = new byte[25000];
                lBuffer = new byte[200000];
                while (!mStop)
                {
                    // Estabilish connection
                    while (!mConnectedToServer && !mStop)
                    {

                        WriteToLog("InitCount " + ++mInitializeCount, true);
                        try
                        {
                            initialize();
                            if (TCPClientData != null && mConnectedToServer)
                            {
                                mDownloadStatus.Clear();// clear the hashtable								
                                                        //send(TCPClientData + mMaxId);
                                send(TCPClientData);
                            }
                            mMarketWatchSent = false;
                        }
                        catch (Exception lException)
                        {
                            //pranav
                            WriteToLog("Receive Thread 1", true);
                            WriteToLog(lException.StackTrace, true);
                            WriteToLog(lException.Message, true);
                            closeAll(5);
                        }
                        if (!mConnectedToServer)
                        {
                            Thread.Sleep(50);
                        }
                    } //end while (!mConnectedToServer && !mStop)
                      //To terminate the thread if mStop is true
                    if (mStop == true)
                    {
                        break;
                    }
                    //Clear any old message remaining in the buffer.
                    if (mTCPMessage != null)
                    {
                        mTCPMessage = "";
                    }

                    lDummyMessage = new byte[1];
                    lDummyMessage[0] = 10;
                    // lLastPacketSendOrReceiveTime = System.DateTime.Now;

                    // Read data from the connection UDP / TCP. Compressed / Uncompressed
                    mdtLastConnectionStartTime = System.DateTime.Now;
                    while (mConnectedToServer && !mStop)
                    {
                        try
                        {
                            if (udpClient != null)
                            {
                                lDecompressed = new byte[10000];
                                lWorkMemory = new byte[200000];
                                lBuffer = new byte[4000];

                                lBuffer = udpClient.Receive(ref serverEndPoint);
                                lNumberOfBytesReceived = lBuffer.Length;
                                mlngBytesReceived += lNumberOfBytesReceived;
                                /// WARNING : Ignore DUMMY messages sent from the server side 
                                /// to check whether the server is alive or not
                                if (lBuffer.Length > 50)
                                {
                                    if (mCompression.ToString() != Compression_Normal)
                                    {
                                        lDeCompressedLength = CompressUtility.DeCompress(lBuffer, lNumberOfBytesReceived, lDecompressed, lWorkMemory);
                                        lData = System.Text.Encoding.ASCII.GetString(lDecompressed, 0, lDeCompressedLength);
                                    }
                                    else
                                    {
                                        lData = System.Text.Encoding.ASCII.GetString(lBuffer, 0, lNumberOfBytesReceived);
                                    }
                                    //lFoundAt = lData.IndexOf("\n");
                                    //									while (lFoundAt >= 0)
                                    //									{
                                    //										lStringBuffer = lData.Substring(0, lFoundAt);
                                    //										if (lStringBuffer.Length > 0)
                                    //										{
                                    lData = lData.Replace("\n", "/z");
                                    onRawMessageArrived(lData);
                                    lData = "";
                                    //										}
                                    //										lData = lData.Substring(lFoundAt + 1);
                                    //										lFoundAt = lData.IndexOf("\n");
                                    //									}
                                    //									if (lData.Length > 1)
                                    //									{
                                    //										onMessageArrived(new MessageArrivedEventArgs(lData));
                                    //									}
                                }
                            }
                            else if (tcpClient != null)
                            {
                                // only UnCompressed and UnEncoded data
                                if (mCompression.ToString() == Compression_Normal)
                                {
                                    lBuffer = new byte[71680];
                                    if (tcpStream != null && tcpStream.DataAvailable == true)
                                    {

                                        lNumberOfBytesReceived = tcpStream.Read(lBuffer, 0, lBuffer.Length);
                                    }
                                    else if (tcpStream != null)
                                    {
                                        while (tcpStream != null && tcpStream.DataAvailable == false && !mStop)
                                        {
                                            if (SecondsChanged(System.DateTime.Now, lLastPacketSendOrReceiveTime))
                                            {
                                                send("\n");
                                                //WriteToLog("Pool Send : " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.fffff") + ":" + Thread.CurrentThread.Name
                                                //    + "IP : " + tcpClient.Client.RemoteEndPoint.ToString());
                                            }
                                            Thread.Sleep(10);
                                        }
                                        if (tcpStream != null && tcpStream.DataAvailable == true)
                                        {
                                            lNumberOfBytesReceived = tcpStream.Read(lBuffer, 0, lBuffer.Length);
                                        }
                                        else
                                            mStop = true;

                                    }
                                    else if (tcpStream == null)
                                    {
                                        mConnectedToServer = false;
                                        WriteToLog("mConnectedToServer to false since tcpstream is null", true);
                                    }
                                    if (lNumberOfBytesReceived == 0 && SecondsChanged(System.DateTime.Now, lLastPacketSendOrReceiveTime))
                                    {
                                        send("\n");
                                    }
                                    if (lNumberOfBytesReceived > 0)
                                        lLastPacketSendOrReceiveTime = System.DateTime.Now;
                                }
                                else
                                {
                                    if (mCompression.Equals(Compression_Encoded))
                                    {
                                        mintNoOfBytesToRead = 2;
                                    }
                                    else
                                    {
                                        mintNoOfBytesToRead = 4;
                                    }
                                    lLengthRead = 0;
                                    // read the Number of Bytes to receive  // mintNoOfBytesToRead = 4 - defined at module level 
                                    while (!mStop && lLengthRead < mintNoOfBytesToRead)
                                    {
                                        try
                                        {
                                            int lintTemp;
                                            if (tcpStream.DataAvailable == true)
                                            {
                                                lintTemp = tcpStream.Read(lBuffer, lLengthRead, mintNoOfBytesToRead - lLengthRead);
                                            }
                                            else
                                            {
                                                while (!mStop && tcpStream.DataAvailable == false)
                                                {
                                                    if (SecondsChanged(System.DateTime.Now, lLastPacketSendOrReceiveTime) && tcpClient != null)
                                                    {
                                                        send("\n");
                                                    }
                                                    Thread.Sleep(10);
                                                }
                                                lintTemp = tcpStream.Read(lBuffer, lLengthRead, mintNoOfBytesToRead - lLengthRead);
                                            }
                                            lLengthRead += lintTemp;
                                        }
                                        catch (SocketException lSocketException)
                                        {
                                            if (lSocketException.ErrorCode == (int)SocketErrorCodes.ConnectionTimedOut)
                                            {
                                                if (SecondsChanged(System.DateTime.Now, lLastPacketSendOrReceiveTime) && tcpClient != null)
                                                {
                                                    send("\n");
                                                }
                                            }
                                            else
                                            {
                                                //pranav
                                                WriteToLog("Receive Thread 2", true);
                                                WriteToLog(lSocketException.StackTrace, true);
                                                WriteToLog(lSocketException.Message, true);

                                                WriteToLog(" Throwing Exception ", true);
                                                if (mobjErrorCodeHash[lSocketException.ErrorCode] != null)
                                                    WriteToLog("Socket Error CODE : -- " + lSocketException.ErrorCode + " DISCRIPTION FROM ENUM : --" + mobjErrorCodeHash[lSocketException.ErrorCode], true);
                                                else
                                                    WriteToLog("Socket Error CODE : -- " + lSocketException.ErrorCode, true);

                                                throw;
                                            }
                                        }
                                        catch (IOException lIOException)
                                        {
                                            //pranav
                                            WriteToLog("Receive Thread 3", true);
                                            WriteToLog(lIOException.StackTrace, true);
                                            WriteToLog(lIOException.Message, true);

                                            //This code is required because in case of receive on TCP we get an IOException instead
                                            //of a Socket Exception. The Socket Exception happens to be inner exception.
                                            SocketException lSocketException = new SocketException();
                                            if (lIOException.InnerException.GetType().Equals(lSocketException.GetType()))
                                            {
                                                lSocketException = (SocketException)lIOException.InnerException;
                                                if (lSocketException.ErrorCode == (int)SocketErrorCodes.ConnectionTimedOut)
                                                {
                                                    if (SecondsChanged(System.DateTime.Now, lLastPacketSendOrReceiveTime) && tcpClient != null)
                                                    {
                                                        send("\n");
                                                    }
                                                }
                                                else
                                                {
                                                    WriteToLog(" Throwing IO Exception ", true);
                                                    throw lIOException;
                                                }
                                            }
                                            else
                                            {
                                                throw lIOException;
                                            }
                                        }
                                    }
                                    //To terminate the thread if mStop is true
                                    if (mStop == true)
                                    {
                                        break;
                                    }
                                    // Message Receive time
                                    // convert the string value of LengthOfData to Integer
                                    if (mCompression.Equals(Compression_Encoded)) // only encoding
                                    {
                                        lDataLength = ((int)lBuffer[0]) * 256 + ((int)lBuffer[1]);
                                    }
                                    else
                                    {
                                        lDataLength = Int16.Parse(Encoding.ASCII.GetString(lBuffer, 0, lLengthRead));
                                    }
                                    lLengthRead = 0;
                                    // Make buffer space for the Compressed data
                                    // lBuffer = new byte[lDataLength];
                                    // get the Compressed data
                                    while (!mStop && lLengthRead < lDataLength)
                                    {
                                        try
                                        {
                                            int lintTemp;
                                            lintTemp = tcpStream.Read(lBuffer, lLengthRead, lDataLength -
                                                lLengthRead);
                                            lLengthRead += lintTemp;
                                            if (lLengthRead > 0)
                                                lLastPacketSendOrReceiveTime = System.DateTime.Now;
                                        }
                                        catch (SocketException lSocketException)
                                        {

                                            if (lSocketException.ErrorCode == (int)SocketErrorCodes.ConnectionTimedOut)
                                            {
                                                if (SecondsChanged(System.DateTime.Now, lLastPacketSendOrReceiveTime) && tcpClient != null)
                                                {
                                                    send("\n");
                                                }
                                            }
                                            else
                                            {
                                                //pranav
                                                WriteToLog("Receive Thread 4", true);
                                                WriteToLog(lSocketException.StackTrace, true);
                                                WriteToLog(lSocketException.Message, true);

                                                WriteToLog(" Throwing Exception ", true);
                                                if (mobjErrorCodeHash[lSocketException.ErrorCode] != null)
                                                    WriteToLog("Socket Error CODE : -- " + lSocketException.ErrorCode + " DISCRIPTION FROM ENUM : --" + mobjErrorCodeHash[lSocketException.ErrorCode], true);
                                                else
                                                    WriteToLog("Socket Error CODE : -- " + lSocketException.ErrorCode, true);

                                                throw lSocketException;
                                            }
                                        }
                                        catch (IOException lIOException)
                                        {
                                            //This code is required because in case of receive on TCP we get an IOException instead
                                            //of a Socket Exception. The Socket Exception happens to be inner exception.
                                            SocketException lSocketException = new SocketException();
                                            if (lIOException.InnerException.GetType().Equals(lSocketException.GetType()))
                                            {
                                                lSocketException = (SocketException)lIOException.InnerException;
                                                if (lSocketException.ErrorCode == (int)SocketErrorCodes.ConnectionTimedOut)
                                                {
                                                    if (SecondsChanged(System.DateTime.Now, lLastPacketSendOrReceiveTime) && tcpClient != null)
                                                    {
                                                        send("\n");
                                                    }
                                                }
                                                else
                                                {
                                                    //pranav
                                                    WriteToLog("Receive Thread 5", true);
                                                    WriteToLog(lSocketException.StackTrace, true);
                                                    WriteToLog(lSocketException.Message, true);

                                                    WriteToLog(" Throwing IO Exception ", true);
                                                    if (mobjErrorCodeHash[lSocketException.ErrorCode] != null)
                                                        WriteToLog("Socket Error CODE : -- " + lSocketException.ErrorCode + " DISCRIPTION FROM ENUM : --" + mobjErrorCodeHash[lSocketException.ErrorCode], true);
                                                    else
                                                        WriteToLog("Socket Error CODE : -- " + lSocketException.ErrorCode, true);
                                                    throw lIOException;
                                                }
                                            }
                                            else
                                            {
                                                //pranav
                                                WriteToLog("Receive Thread 6", true);
                                                WriteToLog(lSocketException.StackTrace, true);
                                                WriteToLog(lSocketException.Message, true);

                                                WriteToLog(" Throwing IO Exception ", true);
                                                throw lIOException;
                                            }
                                        }
                                    }
                                    //To terminate the thread if mStop is true
                                    if (mStop == true)
                                    {
                                        break;
                                    }

                                    mlngBytesReceived += lLengthRead + mintNoOfBytesToRead;
                                    // decompress the data
                                    if (mCompression.Equals(Compression_Encoded)) // encoding only
                                    {
                                        lData = decode(lBuffer, lLengthRead);
                                        lDeCompressedLength = lData.Length;
                                    }
                                    else if (mCompression.Equals(Compression_Compressed)) // lzo compression
                                    {
                                        string lstrTemp;
                                        lstrTemp = System.Text.Encoding.ASCII.GetString(lBuffer, 0, lDataLength);
                                        if (lstrTemp != null && lstrTemp.Trim().Length > 0)
                                        {
                                            Stream stream = new MemoryStream(lBuffer);
                                            //LZ4.LZ4Stream lz4stream = new LZ4.LZ4Stream(stream, System.IO.Compression.CompressionMode.Decompress, true );
                                            //string resultde = ASCIIEncoding.UTF8.GetString(mobjCompressionHelper.Decompress(lBuffer));

                                            //var buf = new byte[2048];
                                            //for (; ; )
                                            //{
                                            //    int nRead = lz4stream.Read(buf, 0, buf.Length);
                                            //    if (nRead == 0)
                                            //        break;

                                            //    //do stuff with buff
                                            //}

                                            //lDeCompressedLength = Compressor.CompressUtility.DeCompress(lBuffer, lDataLength, lDecompressed, lWorkMemory);

                                            // Taking the Decompresed data in a string
                                            //lData = System.Text.Encoding.ASCII.GetString(lDecompressed, 0,lDeCompressedLength);					
                                            byte[] valuedata = new byte[lDataLength];
                                            Array.Copy(lBuffer, valuedata, lDataLength);
                                            lData = ASCIIEncoding.UTF8.GetString(mobjCompressionHelper.Decompress(valuedata));
                                        }
                                        else
                                        {
                                            lData = "";
                                        }
                                    }
                                    else // lzo compression on encoded data
                                    {
                                        lDeCompressedLength = CompressUtility.DeCompress(lBuffer, lDataLength, lDecompressed, lWorkMemory);
                                        lData = decode(lDecompressed, lDeCompressedLength);
                                        lDeCompressedLength = lData.Length;
                                        // Taking the Decompresed data in a string
                                        //lData = System.Text.Encoding.ASCII.GetString(lDecompressed, 0, lDeCompressedLength);
                                    }
                                    //									lFoundAt = 0;
                                    //									lFoundAt = lData.IndexOf("\n");
                                    //									while (lFoundAt >= 0)
                                    //									{
                                    //										lStringBuffer = lData.Substring(0, lFoundAt);
                                    //										if (lStringBuffer.Length > 0)
                                    //										{
                                    lData = lData.Replace("\n", "/z");
                                    onRawMessageArrived(lData);
                                    lData = "";
                                    //											mNoOfMessages += 1;
                                    //										}
                                    //										lData = lData.Substring(lFoundAt + 1);
                                    //										lFoundAt = lData.IndexOf("\n");
                                    //									}
                                    //									if (lData.Length > 1)
                                    //									{
                                    //										onMessageArrived(new MessageArrivedEventArgs(lData));
                                    //										mNoOfMessages += 1;
                                    //									}
                                }
                            }
                            else if (socket != null)
                            {
                                lBuffer = new byte[1024];
                                lNumberOfBytesReceived = socket.Receive(lBuffer, 0, lBuffer.Length, System.Net.Sockets.SocketFlags.None);
                            }
                            if ((mCompression == Compression_Normal) && lBuffer != null && lNumberOfBytesReceived > 0)
                            {
                                if (udpClient != null)
                                {
                                    ////									lMessageBlock.CompressedSizeRecd += lNumberOfBytesReceived;
                                    ////									/// WARNING : Ignore DUMMY messages sent from the server side 
                                    ////									/// to check whether the server is alive or not
                                    ////									if (lBuffer.Length > 50)
                                    ////									{
                                    ////										lDeCompressedLength = Compressor.CompressUtility.DeCompress(lBuffer, lNumberOfBytesReceived, lDecompressed, lWorkMemory);
                                    ////										lMessageBlock.UnCompressedSizeRecd += lDeCompressedLength;
                                    ////										lData = System.Text.Encoding.ASCII.GetString(lDecompressed, 0, lDeCompressedLength);
                                    ////										lFoundAt = lData.IndexOf("\n");
                                    ////										WriteToLog(lData);
                                    ////										while (lFoundAt >= 0)
                                    ////										{
                                    ////											lStringBuffer = lData.Substring(0, lFoundAt);
                                    ////											if (mblnPrintMessages == true)
                                    ////											{
                                    ////												WriteToLog("SB Message " + lStringBuffer);
                                    ////											}
                                    ////											if (lStringBuffer.Length > 0)
                                    ////											{
                                    ////												onMessageArrived(new MessageArrivedEventArgs(lStringBuffer));
                                    ////												lMessageBlock.NoOfMessages += 1;
                                    ////											}
                                    ////											lData = lData.Substring(lFoundAt + 1);
                                    ////											lFoundAt = lData.IndexOf("\n");
                                    ////										}
                                    ////										if (lData.Length > 1)
                                    ////										{
                                    ////											WriteToLog("DA Message " + lStringBuffer);
                                    ////											onMessageArrived(new MessageArrivedEventArgs(lData));
                                    ////											lMessageBlock.NoOfMessages += 1;
                                    ////										}
                                    ////									}
                                }
                                else //if TCP
                                {
                                    mTCPMessage += Encoding.ASCII.GetString(lBuffer, 0, lNumberOfBytesReceived);
                                    //									lFoundAt = mTCPMessage.IndexOf("\n");
                                    //									while (mTCPMessage.IndexOf("\n") >= 0)
                                    //									{
                                    //										lStringBuffer = mTCPMessage.Substring(0, lFoundAt);
                                    mTCPMessage = mTCPMessage.Replace("\n", "/z");
                                    //										if (lStringBuffer.Length > 0)
                                    //										{
                                    //WriteToLog("IP : " + tcpClient.Client.RemoteEndPoint.ToString() + "Thread : " + Thread.CurrentThread.Name + "Message Arrived Event Raised " + System.DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.fffff") + ":" + mTCPMessage);
                                    onRawMessageArrived(mTCPMessage);
                                    mTCPMessage = "";
                                    lBuffer = null;
                                    //										}
                                    //										mTCPMessage = mTCPMessage.Substring(lFoundAt + 1);
                                    //										lFoundAt = mTCPMessage.IndexOf("\n");
                                    //									}
                                }
                                mlngBytesReceived += lNumberOfBytesReceived;
                            }
                            if (udpClient == null && tcpClient == null)
                            {
                                mConnectedToServer = false;
                                WriteToLog("Some how tcp and udp streams got currupted", true);
                            }
                        }
                        catch (SocketException lSocketException)
                        {
                            if (lSocketException.ErrorCode == (int)SocketErrorCodes.ConnectionTimedOut)
                            {
                                if (udpClient != null) udpClient.setReceiveTimeOut(mTimeOut);
                                if (SecondsChanged(System.DateTime.Now, lLastPacketSendOrReceiveTime) && tcpClient != null)
                                {
                                    send("\n");
                                }
                            }
                            else
                            {
                                //pranav
                                WriteToLog("Receive Thread 7", true);
                                WriteToLog(lSocketException.StackTrace, true);
                                WriteToLog(lSocketException.Message, true);

                                if (mobjErrorCodeHash[lSocketException.ErrorCode] != null)
                                    WriteToLog("Socket Error CODE : -- " + lSocketException.ErrorCode + " DISCRIPTION FROM ENUM : --" + mobjErrorCodeHash[lSocketException.ErrorCode], true);
                                else
                                    WriteToLog("Socket Error CODE : -- " + lSocketException.ErrorCode, true);

                                WriteToLog("13:  receive Thread " + lSocketException.Message + " " + serverAddress, true);
                                WriteToLog(lSocketException.StackTrace, true);
                                WriteToLog(lSocketException.Message, true);

                                closeAll(6);
                            }
                            // No of bytes received inbetween breaks
                            WriteToLog(" (SocExp) Number of Bytes Received inbetween break - " + (mlngBytesReceived - lNoBytesBeforeBreak), true);
                            lNoBytesBeforeBreak = mlngBytesReceived;
                        }
                        catch (IOException lIOException)
                        {
                            //This code is required because in case of receive on TCP we get an IOException instead
                            //of a Socket Exception. The Socket Exception happens to be inner exception.
                            SocketException lSocketException = new SocketException();
                            if (lIOException.InnerException.GetType().Equals(lSocketException.GetType()))
                            {
                                lSocketException = (SocketException)lIOException.InnerException;
                                if (lSocketException.ErrorCode != (int)SocketErrorCodes.ConnectionTimedOut)
                                {
                                    if (mobjErrorCodeHash[lSocketException.ErrorCode] != null)
                                        WriteToLog("Socket Error CODE : -- " + lSocketException.ErrorCode + " DISCRIPTION FROM ENUM : --" + mobjErrorCodeHash[lSocketException.ErrorCode], true);
                                    else
                                        WriteToLog("Socket Error CODE : -- " + lSocketException.ErrorCode, true);

                                    WriteToLog("13a: receive Thread " + lSocketException.Message + " " + serverAddress, true);
                                    WriteToLog(lSocketException.StackTrace, true);
                                    WriteToLog(lSocketException.Message, true);
                                    closeAll(7);
                                }
                                else
                                {
                                    //pranav
                                    WriteToLog("Receive Thread 8", true);
                                    WriteToLog(lSocketException.StackTrace, true);
                                    WriteToLog(lSocketException.Message, true);

                                    if (udpClient != null) udpClient.setReceiveTimeOut(mTimeOut);
                                    if (SecondsChanged(System.DateTime.Now, lLastPacketSendOrReceiveTime) && (tcpClient != null))
                                    {
                                        send("\n");
                                    }
                                }
                            }
                            else
                            {
                                //pranav
                                WriteToLog("Receive Thread 9", true);
                                WriteToLog(lIOException.StackTrace, true);
                                WriteToLog(lIOException.Message, true);

                                WriteToLog("13b:  receive Thread " + lIOException.Message + " " + serverAddress, true);
                                WriteToLog(lIOException.StackTrace, true);
                                WriteToLog(lIOException.Message, true);
                                closeAll(8);
                            }
                        }//end of try catch
                    }//end while(mConnectedToServer && !mStop)
                     //To terminate the thread if mStop is true
                    if (mStop == true)
                    {
                        break;
                    }
                }//end while (!mStop)
            }//try catch
            catch (Exception e)
            {
                //pranav
                WriteToLog("Receive Thread 10", true);
                WriteToLog(e.StackTrace, true);
                WriteToLog(e.Message, true);
                closeAll(9);
            }
            finally
            {
                closeAll(10);
            }
        }

        public void onRawMessageArrivedreceiveThread()
        {
            byte[] lBuffer = null;
            //string lStringBuffer = null;
            int lNumberOfBytesReceived = 0;
            //int lFoundAt;
            byte[] lDummyMessage = null;
            int lLengthRead = 0;
            int lDataLength = 0;
            string lData = "";
            long lNoBytesBeforeBreak = 0;
            mInitializeCount = 0;
            lLastPacketSendOrReceiveTime = System.DateTime.Now;
            try
            {
                lDecompressed = new byte[25000];
                lWorkMemory = new byte[25000];
                lBuffer = new byte[200000];
                while (!mStop)
                {
                    // Estabilish connection
                    while (!mConnectedToServer && !mStop)
                    {

                        WriteToLog("InitCount " + ++mInitializeCount, true);
                        try
                        {
                            initialize();
                            if (TCPClientData != null && mConnectedToServer)
                            {
                                mDownloadStatus.Clear();// clear the hashtable								
                                                        //send(TCPClientData + mMaxId);
                                send(TCPClientData);
                            }
                            mMarketWatchSent = false;
                        }
                        catch (Exception lException)
                        {
                            //pranav
                            WriteToLog("Receive Thread 1", true);
                            WriteToLog(lException.StackTrace, true);
                            WriteToLog(lException.Message, true);
                            closeAll(5);
                        }
                        if (!mConnectedToServer)
                        {
                            Thread.Sleep(50);
                        }
                    } //end while (!mConnectedToServer && !mStop)
                      //To terminate the thread if mStop is true
                    if (mStop == true)
                    {
                        break;
                    }
                    //Clear any old message remaining in the buffer.
                    if (mTCPMessage != null)
                    {
                        mTCPMessage = "";
                    }

                    lDummyMessage = new byte[1];
                    lDummyMessage[0] = 10;
                    // lLastPacketSendOrReceiveTime = System.DateTime.Now;

                    // Read data from the connection UDP / TCP. Compressed / Uncompressed
                    mdtLastConnectionStartTime = System.DateTime.Now;
                    while (mConnectedToServer && !mStop)
                    {
                        try
                        {
                            if (udpClient != null)
                            {
                                lDecompressed = new byte[10000];
                                lWorkMemory = new byte[200000];
                                lBuffer = new byte[4000];

                                lBuffer = udpClient.Receive(ref serverEndPoint);
                                lNumberOfBytesReceived = lBuffer.Length;
                                mlngBytesReceived += lNumberOfBytesReceived;
                                /// WARNING : Ignore DUMMY messages sent from the server side 
                                /// to check whether the server is alive or not
                                if (lBuffer.Length > 50)
                                {
                                    if (mCompression.ToString() != Compression_Normal)
                                    {
                                        lDeCompressedLength = CompressUtility.DeCompress(lBuffer, lNumberOfBytesReceived, lDecompressed, lWorkMemory);
                                        lData = System.Text.Encoding.ASCII.GetString(lDecompressed, 0, lDeCompressedLength);
                                    }
                                    else
                                    {
                                        lData = System.Text.Encoding.ASCII.GetString(lBuffer, 0, lNumberOfBytesReceived);
                                    }
                                    //lFoundAt = lData.IndexOf("\n");
                                    //									while (lFoundAt >= 0)
                                    //									{
                                    //										lStringBuffer = lData.Substring(0, lFoundAt);
                                    //										if (lStringBuffer.Length > 0)
                                    //										{
                                    lData = lData.Replace("\n", "/z");
                                    onRawMessageArrived(lData);
                                    lData = "";
                                    //										}
                                    //										lData = lData.Substring(lFoundAt + 1);
                                    //										lFoundAt = lData.IndexOf("\n");
                                    //									}
                                    //									if (lData.Length > 1)
                                    //									{
                                    //										onMessageArrived(new MessageArrivedEventArgs(lData));
                                    //									}
                                }
                            }
                            else if (tcpClient != null)
                            {
                                // only UnCompressed and UnEncoded data
                                if (mCompression.ToString() == Compression_Normal)
                                {
                                    lBuffer = new byte[71680];
                                    if (tcpStream != null && tcpStream.DataAvailable == true)
                                    {

                                        lNumberOfBytesReceived = tcpStream.Read(lBuffer, 0, lBuffer.Length);
                                    }
                                    else if (tcpStream != null)
                                    {
                                        while (tcpStream != null && tcpStream.DataAvailable == false && !mStop)
                                        {
                                            if (SecondsChanged(System.DateTime.Now, lLastPacketSendOrReceiveTime))
                                            {
                                                send("\n");
                                                //WriteToLog("Pool Send : " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.fffff") + ":" + Thread.CurrentThread.Name
                                                //    + "IP : " + tcpClient.Client.RemoteEndPoint.ToString());
                                            }
                                            Thread.Sleep(10);
                                        }
                                        if (tcpStream != null && tcpStream.DataAvailable == true)
                                        {
                                            lNumberOfBytesReceived = tcpStream.Read(lBuffer, 0, lBuffer.Length);
                                        }
                                        else
                                            mStop = true;

                                    }
                                    else if (tcpStream == null)
                                    {
                                        mConnectedToServer = false;
                                        WriteToLog("mConnectedToServer to false since tcpstream is null", true);
                                    }
                                    if (lNumberOfBytesReceived == 0 && SecondsChanged(System.DateTime.Now, lLastPacketSendOrReceiveTime))
                                    {
                                        send("\n");
                                    }
                                    if (lNumberOfBytesReceived > 0)
                                        lLastPacketSendOrReceiveTime = System.DateTime.Now;
                                }
                                else
                                {
                                    if (mCompression.Equals(Compression_Encoded))
                                    {
                                        mintNoOfBytesToRead = 2;
                                    }
                                    else
                                    {
                                        mintNoOfBytesToRead = 4;
                                    }
                                    lLengthRead = 0;
                                    // read the Number of Bytes to receive  // mintNoOfBytesToRead = 4 - defined at module level 
                                    while (!mStop && lLengthRead < mintNoOfBytesToRead)
                                    {
                                        try
                                        {
                                            int lintTemp;
                                            if (tcpStream.DataAvailable == true)
                                            {
                                                lintTemp = tcpStream.Read(lBuffer, lLengthRead, mintNoOfBytesToRead - lLengthRead);
                                            }
                                            else
                                            {
                                                while (!mStop && tcpStream.DataAvailable == false)
                                                {
                                                    if (SecondsChanged(System.DateTime.Now, lLastPacketSendOrReceiveTime) && tcpClient != null)
                                                    {
                                                        send("\n");
                                                    }
                                                    Thread.Sleep(10);
                                                }
                                                lintTemp = tcpStream.Read(lBuffer, lLengthRead, mintNoOfBytesToRead - lLengthRead);
                                            }
                                            lLengthRead += lintTemp;
                                        }
                                        catch (SocketException lSocketException)
                                        {
                                            if (lSocketException.ErrorCode == (int)SocketErrorCodes.ConnectionTimedOut)
                                            {
                                                if (SecondsChanged(System.DateTime.Now, lLastPacketSendOrReceiveTime) && tcpClient != null)
                                                {
                                                    send("\n");
                                                }
                                            }
                                            else
                                            {
                                                //pranav
                                                WriteToLog("Receive Thread 2", true);
                                                WriteToLog(lSocketException.StackTrace, true);
                                                WriteToLog(lSocketException.Message, true);

                                                WriteToLog(" Throwing Exception ", true);
                                                if (mobjErrorCodeHash[lSocketException.ErrorCode] != null)
                                                    WriteToLog("Socket Error CODE : -- " + lSocketException.ErrorCode + " DISCRIPTION FROM ENUM : --" + mobjErrorCodeHash[lSocketException.ErrorCode], true);
                                                else
                                                    WriteToLog("Socket Error CODE : -- " + lSocketException.ErrorCode, true);

                                                throw;
                                            }
                                        }
                                        catch (IOException lIOException)
                                        {
                                            //pranav
                                            WriteToLog("Receive Thread 3", true);
                                            WriteToLog(lIOException.StackTrace, true);
                                            WriteToLog(lIOException.Message, true);

                                            //This code is required because in case of receive on TCP we get an IOException instead
                                            //of a Socket Exception. The Socket Exception happens to be inner exception.
                                            SocketException lSocketException = new SocketException();
                                            if (lIOException.InnerException.GetType().Equals(lSocketException.GetType()))
                                            {
                                                lSocketException = (SocketException)lIOException.InnerException;
                                                if (lSocketException.ErrorCode == (int)SocketErrorCodes.ConnectionTimedOut)
                                                {
                                                    if (SecondsChanged(System.DateTime.Now, lLastPacketSendOrReceiveTime) && tcpClient != null)
                                                    {
                                                        send("\n");
                                                    }
                                                }
                                                else
                                                {
                                                    WriteToLog(" Throwing IO Exception ", true);
                                                    throw lIOException;
                                                }
                                            }
                                            else
                                            {
                                                throw lIOException;
                                            }
                                        }
                                    }
                                    //To terminate the thread if mStop is true
                                    if (mStop == true)
                                    {
                                        break;
                                    }
                                    // Message Receive time
                                    // convert the string value of LengthOfData to Integer
                                    if (mCompression.Equals(Compression_Encoded)) // only encoding
                                    {
                                        lDataLength = ((int)lBuffer[0]) * 256 + ((int)lBuffer[1]);
                                    }
                                    else
                                    {
                                        lDataLength = Int16.Parse(Encoding.ASCII.GetString(lBuffer, 0, lLengthRead));
                                    }
                                    lLengthRead = 0;
                                    // Make buffer space for the Compressed data
                                    // lBuffer = new byte[lDataLength];
                                    // get the Compressed data
                                    while (!mStop && lLengthRead < lDataLength)
                                    {
                                        try
                                        {
                                            int lintTemp;
                                            lintTemp = tcpStream.Read(lBuffer, lLengthRead, lDataLength -
                                                lLengthRead);
                                            lLengthRead += lintTemp;
                                            if (lLengthRead > 0)
                                                lLastPacketSendOrReceiveTime = System.DateTime.Now;
                                        }
                                        catch (SocketException lSocketException)
                                        {

                                            if (lSocketException.ErrorCode == (int)SocketErrorCodes.ConnectionTimedOut)
                                            {
                                                if (SecondsChanged(System.DateTime.Now, lLastPacketSendOrReceiveTime) && tcpClient != null)
                                                {
                                                    send("\n");
                                                }
                                            }
                                            else
                                            {
                                                //pranav
                                                WriteToLog("Receive Thread 4", true);
                                                WriteToLog(lSocketException.StackTrace, true);
                                                WriteToLog(lSocketException.Message, true);

                                                WriteToLog(" Throwing Exception ", true);
                                                if (mobjErrorCodeHash[lSocketException.ErrorCode] != null)
                                                    WriteToLog("Socket Error CODE : -- " + lSocketException.ErrorCode + " DISCRIPTION FROM ENUM : --" + mobjErrorCodeHash[lSocketException.ErrorCode], true);
                                                else
                                                    WriteToLog("Socket Error CODE : -- " + lSocketException.ErrorCode, true);

                                                throw lSocketException;
                                            }
                                        }
                                        catch (IOException lIOException)
                                        {
                                            //This code is required because in case of receive on TCP we get an IOException instead
                                            //of a Socket Exception. The Socket Exception happens to be inner exception.
                                            SocketException lSocketException = new SocketException();
                                            if (lIOException.InnerException.GetType().Equals(lSocketException.GetType()))
                                            {
                                                lSocketException = (SocketException)lIOException.InnerException;
                                                if (lSocketException.ErrorCode == (int)SocketErrorCodes.ConnectionTimedOut)
                                                {
                                                    if (SecondsChanged(System.DateTime.Now, lLastPacketSendOrReceiveTime) && tcpClient != null)
                                                    {
                                                        send("\n");
                                                    }
                                                }
                                                else
                                                {
                                                    //pranav
                                                    WriteToLog("Receive Thread 5", true);
                                                    WriteToLog(lSocketException.StackTrace, true);
                                                    WriteToLog(lSocketException.Message, true);

                                                    WriteToLog(" Throwing IO Exception ", true);
                                                    if (mobjErrorCodeHash[lSocketException.ErrorCode] != null)
                                                        WriteToLog("Socket Error CODE : -- " + lSocketException.ErrorCode + " DISCRIPTION FROM ENUM : --" + mobjErrorCodeHash[lSocketException.ErrorCode], true);
                                                    else
                                                        WriteToLog("Socket Error CODE : -- " + lSocketException.ErrorCode, true);
                                                    throw lIOException;
                                                }
                                            }
                                            else
                                            {
                                                //pranav
                                                WriteToLog("Receive Thread 6", true);
                                                WriteToLog(lSocketException.StackTrace, true);
                                                WriteToLog(lSocketException.Message, true);

                                                WriteToLog(" Throwing IO Exception ", true);
                                                throw lIOException;
                                            }
                                        }
                                    }
                                    //To terminate the thread if mStop is true
                                    if (mStop == true)
                                    {
                                        break;
                                    }

                                    mlngBytesReceived += lLengthRead + mintNoOfBytesToRead;
                                    // decompress the data
                                    if (mCompression.Equals(Compression_Encoded)) // encoding only
                                    {
                                        lData = decode(lBuffer, lLengthRead);
                                        lDeCompressedLength = lData.Length;
                                    }
                                    else if (mCompression.Equals(Compression_Compressed)) // lzo compression
                                    {
                                        string lstrTemp;
                                        lstrTemp = System.Text.Encoding.ASCII.GetString(lBuffer, 0, lDataLength);
                                        if (lstrTemp != null && lstrTemp.Trim().Length > 0)
                                        {
                                            Stream stream = new MemoryStream(lBuffer);
                                            //LZ4.LZ4Stream lz4stream = new LZ4.LZ4Stream(stream, System.IO.Compression.CompressionMode.Decompress, true );
                                            //string resultde = ASCIIEncoding.UTF8.GetString(mobjCompressionHelper.Decompress(lBuffer));

                                            //var buf = new byte[2048];
                                            //for (; ; )
                                            //{
                                            //    int nRead = lz4stream.Read(buf, 0, buf.Length);
                                            //    if (nRead == 0)
                                            //        break;

                                            //    //do stuff with buff
                                            //}

                                            //lDeCompressedLength = Compressor.CompressUtility.DeCompress(lBuffer, lDataLength, lDecompressed, lWorkMemory);

                                            // Taking the Decompresed data in a string
                                            //lData = System.Text.Encoding.ASCII.GetString(lDecompressed, 0,lDeCompressedLength);					
                                            byte[] valuedata = new byte[lDataLength];
                                            Array.Copy(lBuffer, valuedata, lDataLength);
                                            lData = ASCIIEncoding.UTF8.GetString(mobjCompressionHelper.Decompress(valuedata));
                                        }
                                        else
                                        {
                                            lData = "";
                                        }
                                    }
                                    else // lzo compression on encoded data
                                    {
                                        lDeCompressedLength = CompressUtility.DeCompress(lBuffer, lDataLength, lDecompressed, lWorkMemory);
                                        lData = decode(lDecompressed, lDeCompressedLength);
                                        lDeCompressedLength = lData.Length;
                                        // Taking the Decompresed data in a string
                                        //lData = System.Text.Encoding.ASCII.GetString(lDecompressed, 0, lDeCompressedLength);
                                    }
                                    //									lFoundAt = 0;
                                    //									lFoundAt = lData.IndexOf("\n");
                                    //									while (lFoundAt >= 0)
                                    //									{
                                    //										lStringBuffer = lData.Substring(0, lFoundAt);
                                    //										if (lStringBuffer.Length > 0)
                                    //										{
                                    lData = lData.Replace("\n", "/z");
                                    onRawMessageArrived(lData);
                                    lData = "";
                                    //											mNoOfMessages += 1;
                                    //										}
                                    //										lData = lData.Substring(lFoundAt + 1);
                                    //										lFoundAt = lData.IndexOf("\n");
                                    //									}
                                    //									if (lData.Length > 1)
                                    //									{
                                    //										onMessageArrived(new MessageArrivedEventArgs(lData));
                                    //										mNoOfMessages += 1;
                                    //									}
                                }
                            }
                            else if (socket != null)
                            {
                                lBuffer = new byte[1024];
                                lNumberOfBytesReceived = socket.Receive(lBuffer, 0, lBuffer.Length, System.Net.Sockets.SocketFlags.None);
                            }
                            if ((mCompression == Compression_Normal) && lBuffer != null && lNumberOfBytesReceived > 0)
                            {
                                if (udpClient != null)
                                {
                                    ////									lMessageBlock.CompressedSizeRecd += lNumberOfBytesReceived;
                                    ////									/// WARNING : Ignore DUMMY messages sent from the server side 
                                    ////									/// to check whether the server is alive or not
                                    ////									if (lBuffer.Length > 50)
                                    ////									{
                                    ////										lDeCompressedLength = Compressor.CompressUtility.DeCompress(lBuffer, lNumberOfBytesReceived, lDecompressed, lWorkMemory);
                                    ////										lMessageBlock.UnCompressedSizeRecd += lDeCompressedLength;
                                    ////										lData = System.Text.Encoding.ASCII.GetString(lDecompressed, 0, lDeCompressedLength);
                                    ////										lFoundAt = lData.IndexOf("\n");
                                    ////										WriteToLog(lData);
                                    ////										while (lFoundAt >= 0)
                                    ////										{
                                    ////											lStringBuffer = lData.Substring(0, lFoundAt);
                                    ////											if (mblnPrintMessages == true)
                                    ////											{
                                    ////												WriteToLog("SB Message " + lStringBuffer);
                                    ////											}
                                    ////											if (lStringBuffer.Length > 0)
                                    ////											{
                                    ////												onMessageArrived(new MessageArrivedEventArgs(lStringBuffer));
                                    ////												lMessageBlock.NoOfMessages += 1;
                                    ////											}
                                    ////											lData = lData.Substring(lFoundAt + 1);
                                    ////											lFoundAt = lData.IndexOf("\n");
                                    ////										}
                                    ////										if (lData.Length > 1)
                                    ////										{
                                    ////											WriteToLog("DA Message " + lStringBuffer);
                                    ////											onMessageArrived(new MessageArrivedEventArgs(lData));
                                    ////											lMessageBlock.NoOfMessages += 1;
                                    ////										}
                                    ////									}
                                }
                                else //if TCP
                                {
                                    mTCPMessage += Encoding.ASCII.GetString(lBuffer, 0, lNumberOfBytesReceived);
                                    //									lFoundAt = mTCPMessage.IndexOf("\n");
                                    //									while (mTCPMessage.IndexOf("\n") >= 0)
                                    //									{
                                    //										lStringBuffer = mTCPMessage.Substring(0, lFoundAt);
                                    mTCPMessage = mTCPMessage.Replace("\n", "/z");
                                    //										if (lStringBuffer.Length > 0)
                                    //										{
                                    //WriteToLog("IP : " + tcpClient.Client.RemoteEndPoint.ToString() + "Thread : " + Thread.CurrentThread.Name + "Message Arrived Event Raised " + System.DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.fffff") + ":" + mTCPMessage);
                                    onRawMessageArrived(mTCPMessage);
                                    mTCPMessage = "";
                                    lBuffer = null;
                                    //										}
                                    //										mTCPMessage = mTCPMessage.Substring(lFoundAt + 1);
                                    //										lFoundAt = mTCPMessage.IndexOf("\n");
                                    //									}
                                }
                                mlngBytesReceived += lNumberOfBytesReceived;
                            }
                            if (udpClient == null && tcpClient == null)
                            {
                                mConnectedToServer = false;
                                WriteToLog("Some how tcp and udp streams got currupted", true);
                            }
                        }
                        catch (SocketException lSocketException)
                        {
                            if (lSocketException.ErrorCode == (int)SocketErrorCodes.ConnectionTimedOut)
                            {
                                if (udpClient != null) udpClient.setReceiveTimeOut(mTimeOut);
                                if (SecondsChanged(System.DateTime.Now, lLastPacketSendOrReceiveTime) && tcpClient != null)
                                {
                                    send("\n");
                                }
                            }
                            else
                            {
                                //pranav
                                WriteToLog("Receive Thread 7", true);
                                WriteToLog(lSocketException.StackTrace, true);
                                WriteToLog(lSocketException.Message, true);

                                if (mobjErrorCodeHash[lSocketException.ErrorCode] != null)
                                    WriteToLog("Socket Error CODE : -- " + lSocketException.ErrorCode + " DISCRIPTION FROM ENUM : --" + mobjErrorCodeHash[lSocketException.ErrorCode], true);
                                else
                                    WriteToLog("Socket Error CODE : -- " + lSocketException.ErrorCode, true);

                                WriteToLog("13:  receive Thread " + lSocketException.Message + " " + serverAddress, true);
                                WriteToLog(lSocketException.StackTrace, true);
                                WriteToLog(lSocketException.Message, true);

                                closeAll(6);
                            }
                            // No of bytes received inbetween breaks
                            WriteToLog(" (SocExp) Number of Bytes Received inbetween break - " + (mlngBytesReceived - lNoBytesBeforeBreak), true);
                            lNoBytesBeforeBreak = mlngBytesReceived;
                        }
                        catch (IOException lIOException)
                        {
                            //This code is required because in case of receive on TCP we get an IOException instead
                            //of a Socket Exception. The Socket Exception happens to be inner exception.
                            SocketException lSocketException = new SocketException();
                            if (lIOException.InnerException.GetType().Equals(lSocketException.GetType()))
                            {
                                lSocketException = (SocketException)lIOException.InnerException;
                                if (lSocketException.ErrorCode != (int)SocketErrorCodes.ConnectionTimedOut)
                                {
                                    if (mobjErrorCodeHash[lSocketException.ErrorCode] != null)
                                        WriteToLog("Socket Error CODE : -- " + lSocketException.ErrorCode + " DISCRIPTION FROM ENUM : --" + mobjErrorCodeHash[lSocketException.ErrorCode], true);
                                    else
                                        WriteToLog("Socket Error CODE : -- " + lSocketException.ErrorCode, true);

                                    WriteToLog("13a: receive Thread " + lSocketException.Message + " " + serverAddress, true);
                                    WriteToLog(lSocketException.StackTrace, true);
                                    WriteToLog(lSocketException.Message, true);
                                    closeAll(7);
                                }
                                else
                                {
                                    //pranav
                                    WriteToLog("Receive Thread 8", true);
                                    WriteToLog(lSocketException.StackTrace, true);
                                    WriteToLog(lSocketException.Message, true);

                                    if (udpClient != null) udpClient.setReceiveTimeOut(mTimeOut);
                                    if (SecondsChanged(System.DateTime.Now, lLastPacketSendOrReceiveTime) && (tcpClient != null))
                                    {
                                        send("\n");
                                    }
                                }
                            }
                            else
                            {
                                //pranav
                                WriteToLog("Receive Thread 9", true);
                                WriteToLog(lIOException.StackTrace, true);
                                WriteToLog(lIOException.Message, true);

                                WriteToLog("13b:  receive Thread " + lIOException.Message + " " + serverAddress, true);
                                WriteToLog(lIOException.StackTrace, true);
                                WriteToLog(lIOException.Message, true);
                                closeAll(8);
                            }
                        }//end of try catch
                    }//end while(mConnectedToServer && !mStop)
                     //To terminate the thread if mStop is true
                    if (mStop == true)
                    {
                        break;
                    }
                }//end while (!mStop)
            }//try catch
            catch (Exception e)
            {
                //pranav
                WriteToLog("Receive Thread 10", true);
                WriteToLog(e.StackTrace, true);
                WriteToLog(e.Message, true);
                closeAll(9);
            }
            finally
            {
                closeAll(10);
            }
        }

        public bool stop
        {
            set { mStop = value; }
        }
        public void closeAll(int pLocation)
        {
            WriteToLog("15:  Disconnecting from " + pLocation, true);
            WriteToLog("Closing All", true);
            mConnectedToServer = false;
            try
            {
                if (udpClient != null)
                {
                    udpClient.Close();
                    udpClient = null;
                }
            }
            catch (Exception innerException)
            {
                //pranav
                WriteToLog("UDPCLIENT Close ALl", true);
                WriteToLog(innerException.StackTrace, true);
                WriteToLog(innerException.Message, true);

                WriteToLog("16:  receive Thread Exception=" + innerException.Message, true);
            }
            try
            {
                if (tcpStream != null)
                {
                    tcpStream.Close();
                    tcpStream = null;
                }
            }
            catch (Exception innerException)
            {
                //pranav
                WriteToLog(" TCPSTREAM Close ALl", true);
                WriteToLog(innerException.StackTrace, true);
                WriteToLog(innerException.Message, true);

                WriteToLog("17:  receive Thread Exception=" + innerException.Message, true);
            }
            try
            {
                if (tcpClient != null)
                {
                    tcpClient.Close();
                    tcpClient = null;
                }
            }
            catch (Exception innerException)
            {
                //pranav
                WriteToLog("TCPCLIENT Close ALl", true);
                WriteToLog(innerException.StackTrace, true);
                WriteToLog(innerException.Message, true);

                WriteToLog("18:  receive Thread Exception=" + innerException.Message, true);
            }
            try
            {
                if (socket != null)
                {
                    socket.Close();
                    socket = null;
                }
            }
            catch (Exception innerException)
            {
                //pranav
                WriteToLog("SOCKET CLOSE ALl", true);
                WriteToLog(innerException.StackTrace, true);
                WriteToLog(innerException.Message, true);

                WriteToLog("18:  receive Thread Exception=" + innerException.Message, true);
            }
            socket = null;
            WriteToLog("19:  Disconnected", true);
        }

        public bool SecondsChanged(DateTime pCurrentTime, DateTime pSendTime)
        {
            //Ticks gives number of 100 nano seconds

            return (pCurrentTime.Ticks - pSendTime.Ticks) > (mintPoolFrequency * 10000000);
        }
        public event MessageArrivedEventHandler MessageArrived;
        protected virtual void onRawMessageArrived(string pstrData)
        {
            if (pstrData != null && pstrData.Length > 0)
            {
                MessageArrived(pstrData);
            }
        }
        public int getLocalPort()
        {
            if (udpClient != null) return udpClient.getLocalPort();
            else return -1;
        }

        private static string decode(byte[] pBytes, int pLength)
        {
            StringBuilder lBuffer = new StringBuilder();
            bool lEncodeOn = true;
            int lCharInt;
            char lChar;
            int lDecInt;
            for (int i = 0; i < pLength; i++)
            {
                lCharInt = (int)pBytes[i];
                if (lCharInt < 0) lCharInt += 256;
                lChar = (char)lCharInt;
                if (lEncodeOn)
                {
                    lDecInt = decodeInt((int)(lChar >> 4));
                    if (lDecInt != 255)
                        lBuffer.Append((char)lDecInt);
                    else
                        lEncodeOn = false;
                    if (lEncodeOn)
                    {
                        lDecInt = decodeInt((int)(lChar & 15));
                        if (lDecInt != 255)
                            lBuffer.Append((char)lDecInt);
                        else
                            lEncodeOn = false;
                    }
                }
                else
                {
                    if ((int)lChar != 255)
                        lBuffer.Append(lChar);
                    else
                        lEncodeOn = true;
                }
            }
            return lBuffer.ToString();
        }

        private static int decodeInt(int pInt)
        {
            if ((pInt >= 0) && (pInt <= 9)) // 0 .. 9
                return (pInt + 48);
            //		else if (pInt == 10) // .
            //			return 46;
            else if (pInt == 11) // +
                return 43;
            else if (pInt == 12) // -
                return 45;
            else if (pInt == 13) // :
                return 58;
            else if (pInt == 14) // |
                return 124;
            else if (pInt == 15)
                return 255;
            else
                return 255;
        }

        private void WriteToLog(string pstrMessage, bool pblnRaiseAlert = false)
        {
            Infrastructure.Logger.WriteLog(pstrMessage + " #### " + Name, pblnRaiseAlert);
        }
        private static SocketCompressionTypes[] mobjSocketCompressionTypes;
        public static SocketCompressionTypes[] GetCompressionTypes()
        {
            if (mobjSocketCompressionTypes == null)
            {
                SocketCompressionTypes lSocketCompressionTypes;
                mobjSocketCompressionTypes = new SocketCompressionTypes[4];
                lSocketCompressionTypes = new SocketCompressionTypes(Compression_Normal, "No Compression");
                mobjSocketCompressionTypes[0] = lSocketCompressionTypes;
                lSocketCompressionTypes = new SocketCompressionTypes(Compression_Encoded, "Encoding");
                mobjSocketCompressionTypes[1] = lSocketCompressionTypes;
                lSocketCompressionTypes = new SocketCompressionTypes(Compression_Compressed, "LZO Compression");
                mobjSocketCompressionTypes[2] = lSocketCompressionTypes;
                lSocketCompressionTypes = new SocketCompressionTypes(Compression_CompressedEncoded, "LZO and Encoding");
                mobjSocketCompressionTypes[3] = lSocketCompressionTypes;
            }
            return mobjSocketCompressionTypes;
        }

        public bool getDownloadStatus(string pTableName)
        {
            bool lStatus = false;
            if (mDownloadStatus.ContainsKey(pTableName))
            {
                lStatus = (bool)mDownloadStatus[pTableName];
            }
            return lStatus;
        }
        public void setDownloadStatus(string pTableName, bool pStatus)
        {
            mDownloadStatus[pTableName] = pStatus;
        }
    }

    public class SocketCompressionTypes
    {
        private string mstrCompressionType;
        private string mstrCompressionTypeName;

        public SocketCompressionTypes(string pCompressionType, string pCompressionTypeName)
        {
            mstrCompressionType = pCompressionType;
            mstrCompressionTypeName = pCompressionTypeName;
        }
        public string CompressionType
        {
            get { return mstrCompressionType; }
        }
        public string CompressionTypeName
        {
            get { return mstrCompressionTypeName; }
        }
    }
}
