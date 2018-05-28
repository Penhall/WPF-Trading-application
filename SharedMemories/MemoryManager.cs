using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.IO;
using System.Diagnostics;
using CommonFrontEnd.SharedMemories;
using CommonFrontEnd.Model;
using CommonFrontEnd.Global;
using CommonFrontEnd.Common;
using CommonFrontEnd.ViewModel;
using CommonFrontEnd.ViewModel.Touchline;
using CommonFrontEnd.Model.Order;
using CommonFrontEnd.Processor.Order;
using CommonFrontEnd.Processor;
using CommonFrontEnd.Model.Trade;
using CommonFrontEnd.Controller;
using System.Data.SQLite;
using CommonFrontEnd.Model.CorporateAction;
using System.Collections.ObjectModel;
using System.Text;
using System.Globalization;
using CommonFrontEnd.Model.ETIMessageStructure;
using CommonFrontEnd.Processor.Trade;
using CommonFrontEnd.ViewModel.Trade;
using CommonFrontEnd.ViewModel.PersonalDownload;
using System.Windows;
using static CommonFrontEnd.Common.Enumerations.Trade;
using CommonFrontEnd.View.PersonalDownload;
using static CommonFrontEnd.Common.Enumerations;
using static CommonFrontEnd.SharedMemories.DataAccessLayer;

namespace CommonFrontEnd.SharedMemories
{
    /// <summary>
    /// 1. Create a mapping dictionary with key: slotId and value:Y/N (free/occupied)
    /// 2. Get Free memory slot by calling method GetFreeMemory
    /// 3. Send slotID to ViewModel
    /// Cannot inherit from this class
    /// </summary>

    public static partial class MemoryManager
    {
        #region Events
        public static Action<ChangePasswordReply> ChangePassordResAction;
        public static Action<ChangePasswordReply> TradePasswordResetAction;
        public static Action<LogonReply> OnLogonReplyReceived;
        #endregion
        public static ConcurrentQueue<RecordSplitter> OrderQueue;
        public static ConcurrentDictionary<long, OrderModel> DummyOrderDictionary = null;//new ConcurrentDictionary<long, OrderModel>();
        public static Dictionary<string, CommonFrontEnd.Model.OtherMessageStructure.OrderTypeMessage> ReturnedOrderDictionaryFilter = new Dictionary<string, Model.OtherMessageStructure.OrderTypeMessage>();// Wholesale Debt Market
        public static ConcurrentDictionary<string, OrderModel> OrderDictionary = null;//new ConcurrentDictionary<long, OrderModel>();
        public static ConcurrentDictionary<string, OrderModel> OrderDictionaryBackupMemory = null;// new ConcurrentDictionary<long, OrderModel>();//incase of order update 
        public static ConcurrentDictionary<long, TradeBowModel> TradeDictionary = null;
        public static ConcurrentDictionary<int, string> ExecRestatementReasonMemory = null;
        public static Dictionary<string, CorporateActionModel> objCorpActBSE;
        public static Dictionary<long, string> CorpActForOE;
        // public static ObservableCollection<string> CorpActForOE;
        public static string CurrentDate = string.Empty;
        public static string pastDate = string.Empty;
        public static DateTime CDate;
        internal static bool IsPersonalDownloadComplete;
        public static List<string> lines = null;
        public static bool StateCACall = true;
        #region Order
        public static Action<string, string> InvokeWindowOnScripCodeSelection;
        #endregion
        /// <summary>
        /// ConcurrentDictionary: order Memory with Key:UniqueMessageTag, Value:object
        /// </summary>
        public static void CreateOrderMemory()
        {
            DummyOrderDictionary = new ConcurrentDictionary<long, OrderModel>();
            OrderDictionary = new ConcurrentDictionary<string, OrderModel>();
            OrderDictionaryBackupMemory = new ConcurrentDictionary<string, OrderModel>();
            ExecRestatementReasonMemory = new ConcurrentDictionary<int, string>();
            //OrderMemoryConDict = new ConcurrentDictionary<int, object>();
        }


        //public static void CreateTradeMemory()
        //{
        //    TradeDictionary = new ConcurrentDictionary<long, TradeBowModel>();
        //}

    }


#if TWS
    public static partial class MemoryManager
    {
        static LoginResponseProcessor oLoginResponseProcessor;
        static TradeResponseProcessor oTradeResponseProcessor;
        static OrderPerResponseProcessor oOrderPerResponseProcessor;
        #region Properties

        public const int RequestReplyQueueSize = 5;

        //Temp
        public static long TradeCount = 0;
        private static bool TradeResponseReceivedFlag;
        //Socket Properties
        static int remLen = 0;
        const int reservedBuffer = 65536;//64KB//30720;
        static byte[] remData = new byte[reservedBuffer];
        static int destIndex = 0;

        #endregion
        public static ConcurrentDictionary<string, object> NetPositionAllScripDict;
        public static ConcurrentDictionary<string, object> NetPositionCWDict;

        //TODO Code optimized for performance 20/04/2014
        internal static ConcurrentDictionary<string, Type> _assemblyConDict;
        //internal static ConcurrentDictionary<short, Tuple<PropertyInfo[], PropertyInfo[], PropertyInfo[], PropertyInfo[]>> _assemblyTypeMetadata;
        internal static ConcurrentDictionary<long, Tuple<PropertyInfo[], PropertyInfo[], PropertyInfo[], Dictionary<string, PropertyInfo[]>, Dictionary<string, PropertyInfo[]>, Dictionary<string, PropertyInfo[]>>> _assemblyTypeMetadata;

        public static ConcurrentDictionary<string, object> NetPositionSWDemoDict;
        public static ConcurrentDictionary<string, object> NetPositionSWDetailsDemoDict;
        public static ConcurrentDictionary<string, object> NetPositionCWDemoDict;
        public static ConcurrentDictionary<string, object> NetPositionCWDetailsDemoDict;
        //public static ConcurrentDictionary<string, object> AdminTradeDemoDict;

        //TODO Response Mapping (Msg Type 99 and others to respective screen)

        //Handle trade response before order response
        public static ConcurrentDictionary<string, object> OnlineTradeParkMemory;

        //Used for parking online trade until personal download is complete
        public static Queue<object> OnlineTradeQueue;
        public static HashSet<long> OnlineTradesKeySet;

        public static ConcurrentQueue<CommonMessagingWindowModel> CMWMessageBag;
        public static ConcurrentDictionary<int, bool> RequestReplyMappingDict;
        public static ConcurrentDictionary<long, long> MessageTagResponseMappingDict;
        public static ConcurrentQueue<byte[]> UMSMemoryConDict;
        //public static ConcurrentDictionary<int, object> OrderMemoryConDict;

        public static ConcurrentDictionary<long, object> TradeMemoryConDict;

        //TODO Scripwise only memory
        //public static ConcurrentDictionary<string, object> TradeMemoryConDictDemo;
        public static ConcurrentDictionary<string, object> NetPositionSWDict;
        //public static ConcurrentDictionary<string, object> NetPositionCWDict;
        public static int EndOfDownloadCount;

        //TODO AD2TR
        public static Action<TradeUMS> AD2TRDataUpdation;

        public static Action<TradeUMS> SendTradeFeedUpdation;

        public static ConcurrentBag<object> UnProcessedTradeMemoryConDict;

        public static Dictionary<int, byte[]> RequestDict = new Dictionary<int, byte[]>(5);
        public static int receiveOffset = 0;
        public static Queue<byte[]> msgQueue = new Queue<byte[]>();
        public static Queue<byte[]> RequestQueue = new Queue<byte[]>();
        public static Queue<byte[]> ReplyQueue = new Queue<byte[]>();
        public static Queue<byte[]> UmsQueue = new Queue<byte[]>();

        //TODO UMSQUEUE CONCURRENT
        public static ConcurrentQueue<byte[]> UmsQueueConQueue = new ConcurrentQueue<byte[]>();
        public static int CountTrade;

        //Todo NP scrip only
        public static double TotalGrossBuyVal;
        public static double TotalGrossSellVal;
        public static double TotalNetVal;
        public static double TotalGrossVal;

        //todo NP Client only
        public static long NetRealPL;
        public static long NetUnRealPL;
        public static long NetPL;

        //TODO SendTradeFeedIndex
        public static int SendTradeFeedIndex = 0;
        public static int NoOfTradeFeedSent = 0;
        public static bool onlineSendFeed;
        //TODO NP Clientwise ScripWise only
        public static long TotalNetRealPL;
        public static long TotalNetUnRealPL;
        public static long TotalNetPL;

        public static bool LoadNPProgressBar;

        //TODO MessageTag
        public static uint MessageTag = 0;


        #region EventHandler

        public delegate void UpdateDataGridEventHandler(long Count);
        public static event UpdateDataGridEventHandler OnTradeUMSRecieved;

        //public delegate void logonReplyEventHandler(object oLogonReply);
        //public static event logonReplyEventHandler OnLogonReplyReceived;



        public delegate void CMWEventHandler(object oMessage, short msgType);
        public static event CMWEventHandler OnResponseMessageReceived;

        public static Action<object> OnLogonResponseForMainScreen;

        public delegate void NetPositionEventHandler(object oTrade);
        public static event NetPositionEventHandler OnTradeNetPositionReceived;

        //Event after Receiving Group limits
        public static Action<Model.Trade.GroupWiseLimitsModel> OnGroupwiseLimitReceive;

        #endregion

        //public static System.IO.DirectoryInfo BoltINIPath = new System.IO.DirectoryInfo(Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"BOLT.ini")));

        //public static double t;
        public static System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer(System.Windows.Threading.DispatcherPriority.Send);
        #region Socket
        /// <summary>
        /// State object for receiving data from remote device.
        /// </summary>
        public class StateObject
        {

            // Client socket.
            public Socket workSocket = null;
            // Size of receive buffer.
            public const int BufferSize = reservedBuffer;
            // Receive buffer.
            public byte[] buffer = new byte[BufferSize];

        }
        public static class AsynchronousClient
        {
            //public static IniParser parser = new IniParser(BoltINIPath.ToString());

            public static Socket sockXML;
            //The IP Address for the remote device.
            //127.0.0.1
            //private const string host = "10.228.37.85";//"127.0.0.1";
            //private static string host = "10.228.37.20";//"127.0.0.1";
            //private const string host = "10.228.37.102";//"127.0.0.1";mukesh
            private static string host = "127.0.0.1";//Dns.GetHostEntry(Dns.GetHostName()).AddressList[1].ToString();//"127.0.0.1";//Dns.GetHostEntry(Dns.GetHostName()).AddressList[1].ToString();

            // The port number for the remote device.
            // private const int port = 6002;
            public static DirectoryInfo BoltINIPath = new DirectoryInfo(Path.GetFullPath(Path.Combine(System.Environment.CurrentDirectory, @"BOLT.ini")));
            public static IniParser ParserBoltIni = new IniParser(BoltINIPath.ToString());
            private static int port = Convert.ToInt32(ParserBoltIni.GetSetting("RSC", "IMLPort"));//6001;

            // ManualResetEvent instances signal completion.
            private static ManualResetEvent connectDone =
                new ManualResetEvent(false);
            public static ManualResetEvent sendDone =
                new ManualResetEvent(false);
            public static ManualResetEvent receiveDone =
                new ManualResetEvent(false);

            // The response from the remote device.
            //private static String response = String.Empty;

            public static void StartClient()
            {

                // Connect to a remote device.
                try
                {
                    // Establish the remote endpoint for the socket.
                    IPEndPoint remoteEP = new IPEndPoint(IPAddress.Parse(host), port);

                    // Create a TCP/IP socket.
                    sockXML = new Socket(AddressFamily.InterNetwork,
                        SocketType.Stream, ProtocolType.Tcp);

                    // Connect to the remote endpoint.
                    sockXML.BeginConnect(remoteEP,
                        new AsyncCallback(ConnectCallback), sockXML);
                    connectDone.WaitOne();

                    timer.Interval = new TimeSpan(0, 0, 0, 0, 1);
                    timer.Tick += new EventHandler(timer_Tick);
                    timer.Start();

                    // Send test data to the remote device.
                    //Send(sockXML, "This is a test<EOF>");
                    //sendDone.WaitOne();

                    // Receive the response from the remote device.
                    //Receive(sockXML);
                    //receiveDone.WaitOne();

                    // Release the socket.
                    //sockXML.Shutdown(SocketShutdown.Both);
                    //sockXML.Close();
                }
                catch (Exception ex)
                {
                    ExceptionUtility.LogError(ex);
                }

            }

            public static void ConnectCallback(IAsyncResult ar)
            {
                try
                {
                    // Retrieve the socket from the state object.
                    Socket sockXML = (Socket)ar.AsyncState;

                    // Complete the connection.
                    sockXML.EndConnect(ar);

                    // Signal that the connection has been made.
                    connectDone.Set();
                }
                catch (Exception e)
                {
                    ExceptionUtility.LogError(e);
                    connectDone.Set();
                    Console.WriteLine(e.ToString());
                }
            }

            public static void Send(Socket sockXML, byte[] byteData)
            {
                try
                {
                    if (sockXML.Connected)
                    {
                        // Begin sending the data to the remote device.
                        sockXML.BeginSend(byteData, 0, byteData.Length, 0,
                            new AsyncCallback(SendCallback), sockXML);
                    }
                    else
                    {
                        //"Connection Dropped";
                    }
                }
                catch (Exception ex)
                {
                    ExceptionUtility.LogError(ex);
                    throw ex;
                }

            }

            private static void SendCallback(IAsyncResult ar)
            {
                try
                {
                    // Retrieve the socket from the state object.
                    Socket sockXML = (Socket)ar.AsyncState;

                    // Complete sending the data to the remote device.
                    int bytesSent = sockXML.EndSend(ar);

                    // Signal that all bytes have been sent.
                    sendDone.Set();
                }
                catch (Exception e)
                {
                    ExceptionUtility.LogError(e);
                }
            }

            public static void Receive(Socket sockXML)
            {
                try
                {
                    // Create the state object.
                    StateObject state = new StateObject();
                    state.workSocket = sockXML;

                    // Begin receiving the data from the remote device.
                    sockXML.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                        new AsyncCallback(ReceiveCallback), state);

                }
                catch (Exception e)
                {
                    ExceptionUtility.LogError(e);
                }
            }

            private static void ReceiveCallback(IAsyncResult ar)
            {
                try
                {
                    // Retrieve the state object and the client socket 
                    // from the asynchronous state object.
                    StateObject state = (StateObject)ar.AsyncState;
                    Socket sockXML = state.workSocket;

                    // Read data from the remote device.
                    int bytesRead = sockXML.EndReceive(ar);
                    //ExceptionUtility.LogTrace(bytesRead);
                    if (bytesRead > 0)
                    {
                        // There might be more data, so store the data received so far.
                        //state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));
                        byte[] msg = new byte[bytesRead];
                        Array.Copy(state.buffer, 0, msg, 0, bytesRead);
                        receiveOffset = receiveOffset + bytesRead;
                        msgQueue.Enqueue(msg);

                        StateObject newState = new StateObject();
                        newState.workSocket = sockXML;

                        // Get the rest of the data.
                        sockXML.BeginReceive(newState.buffer, 0, StateObject.BufferSize, 0,
                            new AsyncCallback(ReceiveCallback), newState);
                    }
                    else
                    {
                        bool connected = SocketConnected(sockXML);
                        if (sockXML != null && !connected)
                        {
                            MessageBox.Show("Socket Disconnected. Aborting TWSPro!","Abort",MessageBoxButton.OK,MessageBoxImage.Warning);
                            //"Connection Dropped";
                            string IMLName = "imlPro";
                            Process process = Process.GetProcessesByName(IMLName).FirstOrDefault();
                            if (process != null)
                                process.Kill();

                            Process.GetCurrentProcess().Kill();
                        }
                        else
                        {
                            if (sockXML != null)
                            {
                                sockXML.Close();
                            }
                        }

                        // Signal that all bytes have been received.
                        receiveDone.Set();
                    }
                }
                catch (Exception e)
                {
                    ExceptionUtility.LogError(e);
                    Console.WriteLine(e.ToString());
                }
                finally
                {
                }

            }
        }
        static bool SocketConnected(Socket s)
        {
            bool part1 = s.Poll(1000, SelectMode.SelectRead);
            bool part2 = (s.Available == 0);
            if (part1 & part2)
            {//connection is closed
                return false;
            }
            return true;
        }
        static void timer_Tick(object sender, EventArgs e)
        {
            byte[] recvData;
            for (int i = 0; i < 100; i++)
            {
                if (msgQueue.Count > 0)
                {
                    recvData = msgQueue.Dequeue();
                    if (recvData != null)
                    {
                        lock (msgQueue)
                        {
                            ProcessRecdBuffer(recvData);
                        }
                    }
                }
                else
                {
                    break;
                }
            }
        }
        #region Commented BoltPro ProcessRecdBuffer
        //private static void ProcessRecdBuffer(byte[] data)//KeyValuePair<byte[], int> data
        //{

        //    byte[] buffer = data;//data.Key as byte[];
        //    int bytesRead = data.Length;//Convert.ToInt32(data.Value);
        //    int SrcIndex = 0;
        //    int remainingBytes = 0;
        //    int packLen = 0;

        //    remainingBytes = bytesRead;
        //    try
        //    {
        //        if (remLen == 0)
        //        {
        //            remData = new byte[reservedBuffer];
        //        }
        //        while (remainingBytes > 0)
        //        {
        //            if (destIndex == 0 || destIndex < 8)//read header
        //            {
        //                if (destIndex > 0)
        //                {
        //                    if (remainingBytes >= remLen)
        //                    {
        //                        Array.Copy(buffer, SrcIndex, remData, destIndex, remLen);
        //                        destIndex += remLen;
        //                        remainingBytes -= remLen;
        //                        SrcIndex += remLen;
        //                        remLen = 0;
        //                        packLen = BitConverter.ToInt32(remData, 4) - 8;
        //                    }
        //                    else
        //                    {
        //                        Array.Copy(buffer, SrcIndex, remData, destIndex, remainingBytes);
        //                        destIndex += remainingBytes;
        //                        remLen -= remainingBytes;
        //                        SrcIndex += remainingBytes;
        //                        remainingBytes = 0;
        //                        break;
        //                    }

        //                }
        //                else if (destIndex == 0)
        //                {
        //                    if (remainingBytes >= 8)
        //                    {
        //                        Array.Copy(buffer, SrcIndex, remData, destIndex, 8);
        //                        destIndex += 8;
        //                        SrcIndex += 8;
        //                        remainingBytes -= 8;
        //                        packLen = BitConverter.ToInt32(remData, 4) - 8;
        //                    }
        //                    else
        //                    {
        //                        Array.Copy(buffer, SrcIndex, remData, destIndex, remainingBytes);
        //                        destIndex += remainingBytes;
        //                        SrcIndex += remainingBytes;
        //                        remLen = 8 - remainingBytes;
        //                        remainingBytes = 0;
        //                        break;
        //                    }
        //                }

        //                packLen = BitConverter.ToInt32(remData, 4) - 8;

        //                if (remainingBytes < packLen)//partial  data part on socket
        //                {
        //                    Array.Copy(buffer, SrcIndex, remData, destIndex, remainingBytes);
        //                    SrcIndex += remainingBytes;
        //                    destIndex += remainingBytes;
        //                    remLen = packLen - remainingBytes;
        //                    remainingBytes = 0;
        //                    break;
        //                }

        //            }
        //            else
        //            {
        //                if (remLen > 0)
        //                {
        //                    if (remainingBytes >= remLen)
        //                    {
        //                        Array.Copy(buffer, SrcIndex, remData, destIndex, remLen);
        //                        SrcIndex += remLen;
        //                        destIndex += remLen;
        //                        remainingBytes -= remLen;
        //                        packLen = destIndex - 8;
        //                        remLen = 0;
        //                    }
        //                    else
        //                    {
        //                        Array.Copy(buffer, SrcIndex, remData, destIndex, remainingBytes);
        //                        SrcIndex += remainingBytes;
        //                        destIndex += remainingBytes;
        //                        remLen -= remainingBytes;
        //                        remainingBytes = 0;
        //                        break;
        //                    }
        //                }
        //                else
        //                {
        //                    if (remainingBytes >= packLen)
        //                    {
        //                        Array.Copy(buffer, SrcIndex, remData, destIndex, packLen);
        //                        SrcIndex += packLen;
        //                        destIndex += packLen;
        //                        remainingBytes -= packLen;

        //                    }
        //                    else
        //                    {
        //                        Array.Copy(buffer, SrcIndex, remData, destIndex, remainingBytes);
        //                        SrcIndex += remainingBytes;
        //                        destIndex += remainingBytes;
        //                        remLen -= remainingBytes;
        //                        remainingBytes = 0;
        //                        break;
        //                    }
        //                }
        //                destIndex = 0;


        //                byte[] newData = new byte[packLen + 8];

        //                IntPtr abc = Marshal.AllocHGlobal(newData.Length);
        //                Marshal.Copy(remData, 0, abc, newData.Length);
        //                Marshal.Copy(abc, newData, 0, newData.Length);
        //                Marshal.FreeHGlobal(abc);

        //                //DecodeResponse(newData);

        //                var SlotNum = BitConverter.ToInt16(newData, 0);
        //                short msgType = BitConverter.ToInt16(newData, 8);
        //                //if (msgType == 501)
        //                //{
        //                //    CountTrade++;
        //                //}
        //                if (SlotNum == 0)
        //                {
        //                    if (!new[] { 14 }.Any(m => m == msgType))
        //                    {
        //                        UmsQueue.Enqueue(newData);
        //                    }
        //                }
        //                else
        //                {
        //                    ReplyQueue.Enqueue(newData);
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionUtility.LogError(ex);
        //        //CommonFrontEnd.ExceptionUtility.LogException();
        //        //   CommonFrontEnd.ExceptionUtility.LogException(ex, "PrcosessRecdBuffer");
        //    }

        //}
        #endregion
        #region ILP
        private static void ProcessRecdBuffer(byte[] data)//KeyValuePair<byte[], int> data
        {

            byte[] buffer = data;//data.Key as byte[];
            int bytesRead = data.Length;//Convert.ToInt32(data.Value);
            int SrcIndex = 0;
            int remainingBytes = 0;
            int packLen = 0;

            remainingBytes = bytesRead;
            try
            {
                if (remLen == 0)
                {
                    remData = new byte[reservedBuffer];
                }
                while (remainingBytes > 0)
                {
                    if (destIndex == 0 || destIndex < 8)//read header
                    {
                        if (destIndex > 0)
                        {
                            if (remainingBytes >= remLen)
                            {
                                Array.Copy(buffer, SrcIndex, remData, destIndex, remLen);
                                destIndex += remLen;
                                remainingBytes -= remLen;
                                SrcIndex += remLen;
                                remLen = 0;
                                packLen = BitConverter.ToInt32(remData, 4);
                            }
                            else
                            {
                                Array.Copy(buffer, SrcIndex, remData, destIndex, remainingBytes);
                                destIndex += remainingBytes;
                                remLen -= remainingBytes;
                                SrcIndex += remainingBytes;
                                remainingBytes = 0;
                                break;
                            }

                        }
                        else if (destIndex == 0)
                        {
                            if (remainingBytes >= 8)
                            {
                                Array.Copy(buffer, SrcIndex, remData, destIndex, 8);
                                destIndex += 8;
                                SrcIndex += 8;
                                remainingBytes -= 8;
                                packLen = BitConverter.ToInt32(remData, 4);
                            }
                            else
                            {
                                Array.Copy(buffer, SrcIndex, remData, destIndex, remainingBytes);
                                destIndex += remainingBytes;
                                SrcIndex += remainingBytes;
                                remLen = 8 - remainingBytes;
                                remainingBytes = 0;
                                break;
                            }
                        }

                        packLen = BitConverter.ToInt32(remData, 4);

                        if (remainingBytes < packLen)//partial  data part on socket
                        {
                            Array.Copy(buffer, SrcIndex, remData, destIndex, remainingBytes);
                            SrcIndex += remainingBytes;
                            destIndex += remainingBytes;
                            remLen = packLen - remainingBytes;
                            remainingBytes = 0;
                            break;
                        }

                    }
                    else
                    {
                        if (remLen > 0)
                        {
                            if (remainingBytes >= remLen)
                            {
                                Array.Copy(buffer, SrcIndex, remData, destIndex, remLen);
                                SrcIndex += remLen;
                                destIndex += remLen;
                                remainingBytes -= remLen;
                                remLen = 0;
                            }
                            else
                            {
                                Array.Copy(buffer, SrcIndex, remData, destIndex, remainingBytes);
                                SrcIndex += remainingBytes;
                                destIndex += remainingBytes;
                                remLen -= remainingBytes;
                                remainingBytes = 0;
                                break;
                            }
                        }
                        else
                        {
                            if (remainingBytes >= packLen)
                            {
                                Array.Copy(buffer, SrcIndex, remData, destIndex, packLen);
                                SrcIndex += packLen;
                                destIndex += packLen;
                                remainingBytes -= packLen;

                            }
                            else
                            {
                                Array.Copy(buffer, SrcIndex, remData, destIndex, remainingBytes);
                                SrcIndex += remainingBytes;
                                destIndex += remainingBytes;
                                remLen -= remainingBytes;
                                remainingBytes = 0;
                                break;
                            }
                        }
                        destIndex = 0;


                        byte[] newData = new byte[packLen + 8];

                        IntPtr abc = Marshal.AllocHGlobal(newData.Length);
                        Marshal.Copy(remData, 0, abc, newData.Length);
                        Marshal.Copy(abc, newData, 0, newData.Length);
                        Marshal.FreeHGlobal(abc);

                        //DecodeResponse(newData);

                        var SlotNum = BitConverter.ToInt16(newData, 0);
                        long msgType = BitConverter.ToInt32(newData, 8);
                        //if (msgType == 501)
                        //{
                        //    CountTrade++;
                        //}
                        if (SlotNum == 0)
                        {
                            if (!new[] { 14 }.Any(m => m == msgType))
                            {
                                //UmsQueue.Enqueue(newData);
                                ReplyQueue.Enqueue(newData);
                            }
                        }
                        else
                        {
                            ReplyQueue.Enqueue(newData);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
                //CommonFrontEnd.ExceptionUtility.LogException();
                //   CommonFrontEnd.ExceptionUtility.LogException(ex, "PrcosessRecdBuffer");
            }

        }
        #endregion

        public static void InvokeAD2TR(TradeUMS oTradeUMS)
        {
            if (oTradeUMS != null)
            {
                if (MemoryManager.AD2TRDataUpdation != null)
                {
                    MemoryManager.AD2TRDataUpdation((TradeUMS)oTradeUMS);
                    if (MemoryManager.onlineSendFeed)
                    {
                        MemoryManager.SendTradeFeedUpdation((TradeUMS)oTradeUMS);
                    }
                }
            }
        }


        public static void DecodeResponse(byte[] responseBytes)
        {

            int SlotNum = BitConverter.ToInt32(responseBytes, 0);

            uint BodyLen = BitConverter.ToUInt32(responseBytes, 4);

            uint msgType = BitConverter.ToUInt32(responseBytes, 8);

            if (SlotNum == 0)//UMS
            {


                if (ConfigurationMasterMemory.UmsDict.Keys.Any(x => x == msgType))
                {
                    var modelName = ConfigurationMasterMemory.UmsDict[msgType].Name;

                    Type type = _assemblyConDict[modelName];
                    object oModel = Activator.CreateInstance(type);

                    #region Commented


                    //Type t = typeof(modelName);
                    //Type type = Assembly.Load("CommonFrontEnd").GetTypes().FirstOrDefault(t => t.Name == modelName);
                    //object oModel = Activator.CreateInstance("CommonFrontEnd", modelName);
                    //if (msgType == 501)
                    //{
                    //    TradeUMS oTradeUMS = new TradeUMS();
                    //    oModel = CommonFrontEnd.Converter.GetTradeUMSObjectFromBytes(responseBytes, CommonFrontEnd.SharedMemories.ConfigurationMasterMemory.UmsDict[msgType], oTradeUMS);
                    //}
                    //else
                    //{
                    // }
                    //TODO added queue for CMW


                    #endregion
                    oModel = Converter.GetObjectFromBytes(responseBytes, ConfigurationMasterMemory.UmsDict[msgType], oModel, null);
                    if (msgType != 5555)
                        CMWMessageBag.Enqueue(new CommonMessagingWindowModel { Data = oModel, MessageType = msgType, TypeResponse = 0 });

                    //      if (OnResponseMessageReceived != null)
                    //        OnResponseMessageReceived(oModel, msgType);

                    switch (Convert.ToInt32(msgType))
                    {
                        case 1132:
                            ELogOffUMS oELogOffUMS = new ELogOffUMS();
                            oELogOffUMS = oModel as ELogOffUMS;
                            oLoginResponseProcessor = new LoginResponseProcessor(new LogOffUMS());
                            oLoginResponseProcessor.ProcessResponse(oELogOffUMS);
                            break;
                        case 50005://Admin Trade EndOfDownload
                            EAdminEndOfDownload oEAdminEndOfDownload = new EAdminEndOfDownload();
                            oEAdminEndOfDownload = oModel as EAdminEndOfDownload;
                            oTradeResponseProcessor = new TradeResponseProcessor(new AdminTradeEndOfDownload());
                            oTradeResponseProcessor.ProcessResponse(oEAdminEndOfDownload);

                            UtilityLoginDetails.GETInstance.EndofdownloadReceived = true;

                            if (UtilityLoginDetails.GETInstance.PersonalReplyReceived && UtilityLoginDetails.GETInstance.EndofdownloadReceived)
                                PersonalDownloadVM.autoReset.Set();
                            break;
                        case 50010://Admin Online Trade
                            EAdminTradeOnlineUMS oEAdminTradeOnlineUMS = new EAdminTradeOnlineUMS();
                            oEAdminTradeOnlineUMS = oModel as EAdminTradeOnlineUMS;
                            oTradeResponseProcessor = new TradeResponseProcessor(new AdminTradeProcessOnlineTrade());
                            oTradeResponseProcessor.ProcessResponse(oEAdminTradeOnlineUMS);
                            TradeCount = TradeCount + 1;
                            if (OnTradeUMSRecieved != null)
                                OnTradeUMSRecieved(TradeCount);
                            break;
                        case 50004://Admin Offline Trade
                            EAdminTradeUMS oEAdminTradeUMS = new EAdminTradeUMS();
                            oEAdminTradeUMS = oModel as EAdminTradeUMS;
                            oTradeResponseProcessor = new TradeResponseProcessor(new AdminTradeProcessOfflineTrade());
                            oTradeResponseProcessor.ProcessResponse(oEAdminTradeUMS);
                            TradeCount = TradeCount + 1;
                            if (OnTradeUMSRecieved != null)
                                OnTradeUMSRecieved(TradeCount);
                            break;
                        case 5555:
                            TimeSyncUMS oTimeSyncUMS = new TimeSyncUMS();
                            oTimeSyncUMS = oModel as TimeSyncUMS;
                            long unixTimeStamp = oTimeSyncUMS.MilliSecs;

                            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                            var timeSpan = TimeSpan.FromMilliseconds(Convert.ToDouble(unixTimeStamp));
                            var localDateTime = epoch.Add(timeSpan);
                            UtilityLoginDetails.GETInstance.TodaysDateTime = localDateTime;
                            break;
                        case 1520://Trader End of Download
                            ETraderEndOfDownload oETraderEndOfDownload = new ETraderEndOfDownload();
                            oETraderEndOfDownload = oModel as ETraderEndOfDownload;

                            //TODO: PRocess the count and show in personal download screen
                            // ETraderTradeEndOfDownload oETraderTradeEndOfDownload = new ETraderTradeEndOfDownload();
                            // oETraderTradeEndOfDownload = oModel as ETraderTradeEndOfDownload;
                            //oTradeResponseProcessor = new TradeResponseProcessor(new TraderTradeEndOfDownload());
                            //oTradeResponseProcessor.ProcessResponse(oETraderTradeEndOfDownload);


                            UtilityLoginDetails.GETInstance.EndofdownloadReceived = true;

                            if (UtilityLoginDetails.GETInstance.PersonalReplyReceived && UtilityLoginDetails.GETInstance.EndofdownloadReceived)
                                PersonalDownloadVM.autoReset.Set();
                            break;
                        case 1095://Offline Trader Trade UMS
                            ETraderTradeUMS oETraderTradeUMS = new ETraderTradeUMS();
                            oETraderTradeUMS = oModel as ETraderTradeUMS;
                            oTradeResponseProcessor = new TradeResponseProcessor(new TraderTradeProcessOfflineTrade());
                            oTradeResponseProcessor.ProcessResponse(oETraderTradeUMS);
                            TradeCount = TradeCount + 1;
                            oTradeResponseProcessor = new TradeResponseProcessor(new UpdateOrderMemoryOfflineTrade());
                            oTradeResponseProcessor.ProcessResponse(oETraderTradeUMS);
                            if (OnTradeUMSRecieved != null)
                                OnTradeUMSRecieved(TradeCount);
                            break;
                        case 1521:// Online Trader Trade UMS
                            if (UtilityLoginDetails.GETInstance.AllowOnlineTradeProcessingAfterPD)
                            {
                                ETraderTradeOnlineUMS oETraderTradeOnlineUMS = new ETraderTradeOnlineUMS();
                                oETraderTradeOnlineUMS = oModel as ETraderTradeOnlineUMS;
                                oTradeResponseProcessor = new TradeResponseProcessor(new TraderTradeProcessOnlineTrade());
                                oTradeResponseProcessor.ProcessResponse(oETraderTradeOnlineUMS);
                                TradeCount = TradeCount + 1;
                                oTradeResponseProcessor = new TradeResponseProcessor(new UpdateOrderMemoryOnlineTrade());
                                oTradeResponseProcessor.ProcessResponse(oETraderTradeOnlineUMS);
                                if (OnTradeUMSRecieved != null)
                                    OnTradeUMSRecieved(TradeCount);
                            }
                            else
                            {
                                ETraderTradeOnlineUMS oETraderTradeOnlineUMS = new ETraderTradeOnlineUMS();
                                oETraderTradeOnlineUMS = oModel as ETraderTradeOnlineUMS;
                                OnlineTradeQueue.Enqueue(oETraderTradeOnlineUMS);
                            }
                            break;
                        case 9051:
                            OrderRateMsg oOrderRateMsg = new OrderRateMsg();
                            oOrderRateMsg = oModel as OrderRateMsg;
                            break;

                        #region OrderPersonal Download
                        case (int)Enumerations.OrderTypeDownload.NormalOrders:
                            EOrderNomralUMS oEOrderNomralUMS = new EOrderNomralUMS();
                            oEOrderNomralUMS = oModel as EOrderNomralUMS;

                            oOrderPerResponseProcessor = new OrderPerResponseProcessor(new OrderNormalPersonalDownload());
                            oOrderPerResponseProcessor.ProcessResponse(oEOrderNomralUMS);
                            break;

                        case (int)Enumerations.OrderTypeDownload.ReturnOrders:
                            EOrderRetUMS oEOrderRetUMS = new EOrderRetUMS();
                            oEOrderRetUMS = oModel as EOrderRetUMS;
                            oOrderPerResponseProcessor = new OrderPerResponseProcessor(new OrderRetPersonalDownload());
                            oOrderPerResponseProcessor.ProcessResponse(oEOrderRetUMS);
                            break;

                        case (int)Enumerations.OrderTypeDownload.StopLossOrders:
                            EOrderStopLossUMS oEOrderStopLossUMS = new EOrderStopLossUMS();
                            oEOrderStopLossUMS = oModel as EOrderStopLossUMS;
                            oOrderPerResponseProcessor = new OrderPerResponseProcessor(new OrderStopLossPersonalDownload());
                            oOrderPerResponseProcessor.ProcessResponse(oEOrderStopLossUMS);
                            break;

                        case (int)Enumerations.OrderTypeDownload.ReturnStopLossOrders:
                            EOrderRetStopLossUMS oEOrderRetStopLossUMS = new EOrderRetStopLossUMS();
                            oEOrderRetStopLossUMS = oModel as EOrderRetStopLossUMS;
                            oOrderPerResponseProcessor = new OrderPerResponseProcessor(new OrderRetStopPersonalDownload());
                            oOrderPerResponseProcessor.ProcessResponse(oEOrderRetStopLossUMS);
                            break;


                        #endregion

                        #region Online Limit Personal Download 
                        case 22001:
                            ETradeLimitOnlineUMS oETradeLimitOnlineUMS = new ETradeLimitOnlineUMS();
                            oETradeLimitOnlineUMS = oModel as ETradeLimitOnlineUMS;
                            if (oETradeLimitOnlineUMS != null)
                            {
                                CommonFunctions.PopulateLimitMemory(msgType, oETradeLimitOnlineUMS);

                                //if (oETradeLimitOnlineUMS.MsgTag == (int)AdminTradeRequestMsgTag.EQTYSEGINDICATOR)
                                //{
                                //    MainWindowVM.objLogOnStatus[(int)Segment.Equity].GrossBuyLimit = oETradeLimitOnlineUMS.GrossLimitBuy;
                                //    MainWindowVM.objLogOnStatus[(int)Segment.Equity].GrossSellLimit = oETradeLimitOnlineUMS.GrossLimitSell;
                                //    MainWindowVM.objLogOnStatus[(int)Segment.Equity].NetValue = oETradeLimitOnlineUMS.NetValue;
                                //}

                                //else if (oETradeLimitOnlineUMS.MsgTag == (int)AdminTradeRequestMsgTag.DERISEGINDICATOR)
                                //{
                                //    MainWindowVM.objLogOnStatus[(int)Segment.Derivative].GrossBuyLimit = oETradeLimitOnlineUMS.GrossLimitBuy;
                                //    MainWindowVM.objLogOnStatus[(int)Segment.Derivative].GrossSellLimit = oETradeLimitOnlineUMS.GrossLimitSell;
                                //    MainWindowVM.objLogOnStatus[(int)Segment.Derivative].NetValue = oETradeLimitOnlineUMS.NetValue;
                                //}
                                //else if (oETradeLimitOnlineUMS.MsgTag == (int)AdminTradeRequestMsgTag.CURRSEGINDICATOR)
                                //{
                                //    MainWindowVM.objLogOnStatus[(int)Segment.Currency].GrossBuyLimit = oETradeLimitOnlineUMS.GrossLimitBuy;
                                //    MainWindowVM.objLogOnStatus[(int)Segment.Currency].GrossSellLimit = oETradeLimitOnlineUMS.GrossLimitSell;
                                //    MainWindowVM.objLogOnStatus[(int)Segment.Currency].NetValue = oETradeLimitOnlineUMS.NetValue;
                                //}
                            }
                            break;

                        case 22016:
                            ETradeGWLimitOnlineUMS oETradeGWLimitOnlineUMS = new ETradeGWLimitOnlineUMS();
                            oETradeGWLimitOnlineUMS = oModel as ETradeGWLimitOnlineUMS;
                            if (oETradeGWLimitOnlineUMS != null)
                            {
                                if (oETradeGWLimitOnlineUMS.lstTraderGwLimitGrp != null)
                                {
                                    CommonFunctions.PopulateLimitMemory(msgType, oETradeGWLimitOnlineUMS);
                                    /*
                                    for (int i = 0; i < oETradeGWLimitOnlineUMS.lstTraderGwLimitGrp.Count; i++)
                                    {
                                        EGroupWiseLimitOnlineDet oEGroupWiseLimitOnlineDet = new EGroupWiseLimitOnlineDet();
                                        oEGroupWiseLimitOnlineDet = (EGroupWiseLimitOnlineDet)oETradeGWLimitOnlineUMS.lstTraderGwLimitGrp[i];
                                        if (oEGroupWiseLimitOnlineDet != null)
                                        {
                                            if (MasterSharedMemory.GroupWiseLimitDict.ContainsKey(oEGroupWiseLimitOnlineDet.GroupName.Trim().ToUpper()))
                                            {
                                                GroupWiseLimitsModel oGroupWiseLimitsModel = new GroupWiseLimitsModel();
                                                oGroupWiseLimitsModel.Group = oEGroupWiseLimitOnlineDet.GroupName.Trim();
                                                oGroupWiseLimitsModel.BuyValue = oEGroupWiseLimitOnlineDet.BuyValue;
                                                oGroupWiseLimitsModel.SellValue = oEGroupWiseLimitOnlineDet.SellValue;

                                                MasterSharedMemory.GroupWiseLimitDict.TryUpdate(oGroupWiseLimitsModel.Group, oGroupWiseLimitsModel, MasterSharedMemory.GroupWiseLimitDict[oGroupWiseLimitsModel.Group]);
                                                //Call Re-Calculate Function Here
                                            }

                                        }
                                    }*/
                                }

                            }
                            break;
                        #endregion

                        #region MarketOrderUMS
                        case 1530://Market Order UMS
                            EMarketOrderUMS oEMarketOrderUMS = new EMarketOrderUMS();
                            oEMarketOrderUMS = oModel as EMarketOrderUMS;
                            OrderUMSProcessor oOrderUMSProcessor = new OrderUMSProcessor(new MarketOrderUMS());
                            oOrderUMSProcessor.ProcessOrderUMS(oEMarketOrderUMS);
                            break;
                        #endregion

                        #region IOCCancelOrderUMS
                        case 1531://IOC Cancel Order UMS
                            EIOCCancelOrderUMS oEIOCCancelOrderUMS = new EIOCCancelOrderUMS();
                            oEIOCCancelOrderUMS = oModel as EIOCCancelOrderUMS;
                            oOrderUMSProcessor = new OrderUMSProcessor(new IOCCancelOrderUMS());
                            oOrderUMSProcessor.ProcessOrderUMS(oEIOCCancelOrderUMS);
                            break;
                        case 2507:
                            ESLTriggerUMS oESLTriggerUMS = new ESLTriggerUMS();
                            oESLTriggerUMS = oModel as ESLTriggerUMS;
                            oOrderUMSProcessor = new OrderUMSProcessor(new SLTrigger());
                            oOrderUMSProcessor.ProcessOrderUMS(oESLTriggerUMS);
                            break;
                        case 3233:
                            ECancelledOrderUMS oECancelledOrderUMS = new ECancelledOrderUMS();
                            oECancelledOrderUMS = oModel as ECancelledOrderUMS;
                            oOrderUMSProcessor = new OrderUMSProcessor(new CancelledOrder());
                            oOrderUMSProcessor.ProcessOrderUMS(oECancelledOrderUMS);
                            break;
                        #endregion

                        #region ReturnOrderUMS
                        case 1853:
                            EOnlineReturnOrder oEOnlineReturnOrder = new EOnlineReturnOrder();
                            oEOnlineReturnOrder = oModel as EOnlineReturnOrder;
                            oOrderUMSProcessor = new OrderUMSProcessor(new ReturnOrderUMS());
                            oOrderUMSProcessor.ProcessOrderUMS(oEOnlineReturnOrder);
                            break;

                        case 1856:
                            EOnlineBlockReturnOrder oEOnlineBlockReturnOrder = new EOnlineBlockReturnOrder();
                            oEOnlineBlockReturnOrder = oModel as EOnlineBlockReturnOrder;
                            oOrderUMSProcessor = new OrderUMSProcessor(new ReturnOrderUMS());
                            oOrderUMSProcessor.ProcessOrderUMS(oEOnlineBlockReturnOrder);
                            //oOrderUMSProcessor.ProcessOrderUMS(oEOnlineBlockReturnOrder);
                            break;
                        #endregion

                        #region Capital Info UMS
                        case 1922:
                            ECapitalInfoUMS oECapitalInfoUMS = new ECapitalInfoUMS();
                            oECapitalInfoUMS = oModel as ECapitalInfoUMS;
                            UMSProcessor.ProcessCapInfoUMS(oECapitalInfoUMS);
                            break;
                        case 1920:
                            ERRMUMS oERRMUMS = new ERRMUMS();
                            oERRMUMS = oModel as ERRMUMS;
                            oOrderUMSProcessor = new OrderUMSProcessor(new RRMOrder());
                            oOrderUMSProcessor.ProcessOrderUMS(oERRMUMS);
                            break;
                        case 1921:
                            EMemLevelPosUMS oEMemLevelPosUMS = new EMemLevelPosUMS();
                            oEMemLevelPosUMS = oModel as EMemLevelPosUMS;
                            oOrderUMSProcessor = new OrderUMSProcessor(new PositionLimitNotification());
                            oOrderUMSProcessor.ProcessOrderUMS(oEMemLevelPosUMS);
                            break;
                        case 1927:
                            EMassCancelUMS oEMassCancelUMS = new EMassCancelUMS();
                            oEMassCancelUMS = oModel as EMassCancelUMS;
                            oOrderUMSProcessor = new OrderUMSProcessor(new MassCancelUMS());
                            oOrderUMSProcessor.ProcessOrderUMS(oEMassCancelUMS);
                            //OrderProcessor.OnOrderResponseReceived?.Invoke(Enumerations.OrderExecutionStatus.Exits.ToString());
                            //OrderProcessor.OnOrderResponseReceived?.Invoke(Enumerations.OrderExecutionStatus.StopExist.ToString());
                            //OrderProcessor.OnOrderResponseReceived?.Invoke(Enumerations.OrderExecutionStatus.Return.ToString());
                            break;
                        #endregion
                        #region Broker Suspension and Reactivation
                        case 24004:
                            EBrokerSuspension oEBrokerSuspension = new EBrokerSuspension();
                            oEBrokerSuspension = oModel as EBrokerSuspension;
                            UMSProcessor.ProcessBrokerSuspension(oEBrokerSuspension);
                            break;
                        case 1528:
                            EBrokerReactivation oEBrokerReactivation = new EBrokerReactivation();
                            oEBrokerReactivation = oModel as EBrokerReactivation;
                            UMSProcessor.ProcessBrokerReactivation(oEBrokerReactivation);
                            break;
                        #endregion
                        default:
                            //oModel = CommonFrontEnd.Common.Converter.GetObjectFromBytes(responseBytes, CommonFrontEnd.SharedMemories.ConfigurationMasterMemory.UmsDict[msgType], oModel);
                            break;
                    }

                    //TODO change its calling position
                    if (msgType == 1520 || msgType == 50005)//1520 for trader
                    {
                        //CA Main memory
                        if (StateCACall)
                            PopulateCAMainMemory();
                        StateCACall = false;
                    }
                    #region Commented


                    //if (msgType == 501)
                    //{
                    //    ////TradeUMS oTradeUMS = new TradeUMS();
                    //    ////oTradeUMS = CommonFrontEnd.Converter.GetTradeUMSObjectFromBytes(responseBytes, CommonFrontEnd.SharedMemories.ConfigurationMasterMemory.UmsDict[msgType], oTradeUMS);
                    //    //oModel = CommonFrontEnd.Converter.GetObjectFromBytes(responseBytes, CommonFrontEnd.SharedMemories.ConfigurationMasterMemory.UmsDict[msgType], oModel);
                    //    //TradeCount++;
                    //    //AdminTradeViewVM.Title = "Admin trade View " + TradeCount.ToString();

                    //    //if (((TradeUMS)oModel).ApplResendFlag == 1)//offline
                    //    //{
                    //    //    TradeMemoryConDict.TryAdd(TradeMemoryConDict.Count, oModel);//OrderId
                    //    //}
                    //    //else if (((TradeUMS)oModel).ApplResendFlag == 0)//online
                    //    //{
                    //    //    if (!TradeResponseReceivedFlag)
                    //    //    {
                    //    //        OnlineTradeQueue.Enqueue(oModel);
                    //    //    }
                    //    //    else
                    //    //    {
                    //    //        TradeMemoryConDict.TryAdd(TradeMemoryConDict.Count, oModel);
                    //    //        UMSProcessor.ProcessNetPositionCWDemo(oModel);
                    //    //    }
                    //    //}

                    //}
                    //else
                    //{
                    //    oModel = CommonFrontEnd.Converter.GetObjectFromBytes(responseBytes, CommonFrontEnd.SharedMemories.ConfigurationMasterMemory.UmsDict[msgType], oModel);
                    //}
                    #endregion
                    #region Commented
                    #endregion
                }

            }
            else
            {
                if (ConfigurationMasterMemory.ReplyDict.Keys.Contains(msgType))
                {

                    var modelName = ConfigurationMasterMemory.ReplyDict[msgType].Name;

                    //Type type = Assembly.Load("CommonFrontEnd").GetTypes().FirstOrDefault(t => t.Name == modelName);
                    //object oModel = Activator.CreateInstance(type);

                    Type type = _assemblyConDict[modelName];
                    object oModel = Activator.CreateInstance(type);

                    oModel = Converter.GetObjectFromBytes(responseBytes, ConfigurationMasterMemory.ReplyDict[msgType], oModel, null);
                    if (msgType != 5555)
                        CMWMessageBag.Enqueue(new CommonMessagingWindowModel { Data = oModel, MessageType = msgType, TypeResponse = 1 });
                    //TODO added queue for CMW
                    //     if (OnResponseMessageReceived != null)
                    //        OnResponseMessageReceived(oModel, msgType);

                    //oLogonResponse = CommonFrontEnd.Converter.GetObjectFromBytes(responseBytes, CommonFrontEnd.SharedMemories.ConfigurationMasterMemory.ReplyDict[msgType], oLogonResponse);
                    //Messenger.Default.Send(oLogonResponse);
                    switch (Convert.ToInt32(msgType))
                    {
                        //case 101:
                        case 0:
                            EUserRegistrationReply oEUserRegistrationReply = new EUserRegistrationReply();
                            oEUserRegistrationReply = oModel as EUserRegistrationReply;
                            oLoginResponseProcessor = new LoginResponseProcessor(new UserRegistration());
                            oLoginResponseProcessor.ProcessResponse(oEUserRegistrationReply);
                            break;
                        case 1131:
                            ELogonReply oELogonReply = new ELogonReply();
                            oELogonReply = oModel as ELogonReply;
                            oLoginResponseProcessor = new LoginResponseProcessor(new LogOn());
                            oLoginResponseProcessor.ProcessResponse(oELogonReply);
                            break;
                        case 2111:
                            EPCASQUERYMKTINFOREPLY oEPCASQUERYMKTINFOREPLY = new EPCASQUERYMKTINFOREPLY();
                            oEPCASQUERYMKTINFOREPLY = oModel as EPCASQUERYMKTINFOREPLY;
                            MemberQueryResponseProcessor objMemberQueryResponseProcessor = new MemberQueryResponseProcessor(new MemberQuery());
                            objMemberQueryResponseProcessor.ProcessResponse(oEPCASQUERYMKTINFOREPLY);
                            break;
                        case 1132:
                            ELogOffReply oELogOffReply = new ELogOffReply();
                            oELogOffReply = oModel as ELogOffReply;
                            oLoginResponseProcessor = new LoginResponseProcessor(new LogOff());
                            oLoginResponseProcessor.ProcessResponse(oELogOffReply);
                            break;
                        case 1133://1133-Mandatory \ forceful change of password incase password expires
                            EChangePwdReply oEChangePwdReplyForce = new EChangePwdReply();
                            oEChangePwdReplyForce = oModel as EChangePwdReply;
                            oLoginResponseProcessor = new LoginResponseProcessor(new ChangePassword());
                            oLoginResponseProcessor.ProcessResponse(oEChangePwdReplyForce);
                            break;
                        case 1134://1134-Optional change of Password. 
                            EChangePwdReply oEChangePwdReply = new EChangePwdReply();
                            oEChangePwdReply = oModel as EChangePwdReply;
                            oLoginResponseProcessor = new LoginResponseProcessor(new ChangePassword());
                            oLoginResponseProcessor.ProcessResponse(oEChangePwdReply);
                            break;
                        case 1135://1134-Optional change of Password. 
                            ETraderPwdResetReply oETraderPwdResetReply = new ETraderPwdResetReply();
                            oETraderPwdResetReply = oModel as ETraderPwdResetReply;
                            oLoginResponseProcessor = new LoginResponseProcessor(new TraderPasswordReset());
                            oLoginResponseProcessor.ProcessResponse(oETraderPwdResetReply);
                            break;
                        case 50004:
                            EAdminTradeReply oEAdminTradeReply = new EAdminTradeReply();
                            oEAdminTradeReply = oModel as EAdminTradeReply;
                            oTradeResponseProcessor = new TradeResponseProcessor(new AdminTradePersonalDownload());
                            oTradeResponseProcessor.ProcessResponse(oEAdminTradeReply);
                            UtilityLoginDetails.GETInstance.PersonalReplyReceived = true;
                            break;
                        case 1095://Trader Trade Response
                            ETraderTradeReply oETraderTradeReply = new ETraderTradeReply();
                            oETraderTradeReply = oModel as ETraderTradeReply;
                            if (oETraderTradeReply.ReplyCode == 0)
                            {
                                UtilityLoginDetails.GETInstance.PersonalReplyReceived = true;

                                if (oETraderTradeReply.MessageTag == (int)AdminTradeRequestMsgTag.EQTYSEGINDICATOR)
                                {
                                    PersonalDownloadVM.GetInstance.imgticksTrdEqVisibility = "Visible";

                                }
                                else if (oETraderTradeReply.MessageTag == (int)AdminTradeRequestMsgTag.DERISEGINDICATOR)
                                {
                                    PersonalDownloadVM.GetInstance.imgticksTrdDerVisibility = "Visible";
                                }
                                else if (oETraderTradeReply.MessageTag == (int)AdminTradeRequestMsgTag.CURRSEGINDICATOR)
                                {
                                    PersonalDownloadVM.GetInstance.imgticksTrdCurVisibility = "Visible";
                                }
                                if (UtilityLoginDetails.GETInstance.PersonalReplyReceived && UtilityLoginDetails.GETInstance.EndofdownloadReceived)
                                    PersonalDownloadVM.autoReset.Set();
                            }
                            else
                            {
                                MessageBoxResult oMessageBoxResult = System.Windows.MessageBox.Show("Personal Download Failed.\n Do you want to continue?", "Warning!", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                                if (oMessageBoxResult == MessageBoxResult.Yes)
                                {
                                    PersonalDownloadVM.autoReset.Set();
                                    UtilityLoginDetails.GETInstance.PersonalReplyReceived = false;
                                    UtilityLoginDetails.GETInstance.EndofdownloadReceived = false;

                                    if (oETraderTradeReply.MessageTag == (int)AdminTradeRequestMsgTag.EQTYSEGINDICATOR)
                                    {
                                        PersonalDownloadVM.GetInstance.imgCrossTrdEqVisibility = "Visible";

                                    }
                                    else if (oETraderTradeReply.MessageTag == (int)AdminTradeRequestMsgTag.DERISEGINDICATOR)
                                    {
                                        PersonalDownloadVM.GetInstance.imgCrossTrdDerVisibility = "Visible";
                                    }
                                    else if (oETraderTradeReply.MessageTag == (int)AdminTradeRequestMsgTag.CURRSEGINDICATOR)
                                    {
                                        PersonalDownloadVM.GetInstance.imgCrossTrdCurVisibility = "Visible";
                                    }
                                }
                                else
                                {
                                    Process.GetCurrentProcess().Kill();
                                }
                            }

                            oTradeResponseProcessor = new TradeResponseProcessor(new TraderTradePersonalDownload());
                            oTradeResponseProcessor.ProcessResponse(oETraderTradeReply);
                            break;
                        case 1025: //Order Request and Reply
                            EOrderReply oEOrderReply = new EOrderReply();
                            oEOrderReply = oModel as EOrderReply;

                            //Added by Gaurav Jadhav 15/3/2018
                            OrderResponseProcessor oOrderResponseProcessor = null;
                            var AUDCode = string.Empty;
                            if (oEOrderReply.ReplyCode > 0 && !new[] { "A", "U", "D" }.Any(x => x == oEOrderReply.AUDCode))//fail case
                            {
                                var orderErrorDetails = new OrderBaseProcessor().FetchDataFromDummyOrderMemory(oEOrderReply.MessageTag);
                                AUDCode = orderErrorDetails.OrderAction;
                            }

                            //Check for "A", "U", "D"
                            if (oEOrderReply.AUDCode == Enumerations.Order.Modes.A.ToString() || AUDCode == Enumerations.Order.Modes.A.ToString())
                            {
                                oOrderResponseProcessor = new OrderResponseProcessor(new AddOrder());
                                oOrderResponseProcessor.ProcessResponse(oEOrderReply);
                            }
                            else if (oEOrderReply.AUDCode == Enumerations.Order.Modes.U.ToString() || AUDCode == Enumerations.Order.Modes.U.ToString())
                            {
                                oOrderResponseProcessor = new OrderResponseProcessor(new ModifyOrder());
                                oOrderResponseProcessor.ProcessResponse(oEOrderReply);
                            }
                            else if (oEOrderReply.AUDCode == Enumerations.Order.Modes.D.ToString() || AUDCode == Enumerations.Order.Modes.D.ToString())
                            {
                                oOrderResponseProcessor = new OrderResponseProcessor(new DeleteOrder());
                                oOrderResponseProcessor.ProcessResponse(oEOrderReply);
                            }

                            #region Commented 


                            //OrderModel oOrderResponse = new OrderModel();
                            //oOrderResponse.MessageTag = Convert.ToUInt32(oEOrderReply.MessageTag);
                            //oOrderResponse.ReplyCode = oEOrderReply.ReplyCode;
                            //oOrderResponse.Quantity = oEOrderReply.AcceptedQty;
                            //oOrderResponse.OrderId = oEOrderReply.TransactionId.ToString();
                            //oOrderResponse.OrderNumber = oEOrderReply.TransactionId.ToString();
                            //oOrderResponse.Day = oEOrderReply.Day;
                            //oOrderResponse.Month = oEOrderReply.Month;
                            //oOrderResponse.Year = oEOrderReply.Year;
                            //oOrderResponse.Hour = oEOrderReply.Hour;
                            //oOrderResponse.Minute = oEOrderReply.Minute;
                            //oOrderResponse.Second = oEOrderReply.Second;
                            //oOrderResponse.Msecond = oEOrderReply.MilliSecond;
                            //oOrderResponse.OrdStatus = oEOrderReply.ReplyTxt;
                            //oOrderResponse.ParticipantCode = oEOrderReply.Cpcode;
                            //oOrderResponse.PendingQuantity = oEOrderReply.PendingQty;
                            //oOrderResponse.BuySellIndicator = oEOrderReply.BuyOrSell;

                            //if (!string.IsNullOrEmpty(oEOrderReply.OrderType))
                            //    oOrderResponse.OrderType = Convert.ToChar(Convert.ToInt32(oEOrderReply.OrderType)).ToString();

                            //if (!string.IsNullOrEmpty(oEOrderReply.AUDCode))
                            //    oOrderResponse.OrderAction = Convert.ToChar(Convert.ToInt32(oEOrderReply.AUDCode)).ToString();//(Convert.ToChar(oEOrderReply.AUDCode)).ToString();

                            //if (oEOrderReply.BuyOrSell == Enumerations.SideShort.B.ToString())
                            //{
                            //    oOrderResponse.BuySellIndicator = Enumerations.Order.BuySellFlag.BUY.ToString();
                            //}
                            //else if (oEOrderReply.BuyOrSell == Enumerations.SideShort.S.ToString())
                            //{
                            //    oOrderResponse.BuySellIndicator = Enumerations.Order.BuySellFlag.SELL.ToString();
                            //}


                            //if (!string.IsNullOrEmpty(oEOrderReply.OrdType))
                            //{
                            //    oOrderResponse.OrderType = Convert.ToChar(Convert.ToInt32(oEOrderReply.OrdType)).ToString();
                            //    if (oOrderResponse.OrderType == Enumerations.OrderTypeShort.L.ToString())
                            //    {
                            //        oOrderResponse.OrderType = Enumerations.Order.OrderTypes.LIMIT.ToString();
                            //    }
                            //    else if (oOrderResponse.OrderType == Enumerations.OrderTypeShort.G.ToString())
                            //    {
                            //        oOrderResponse.OrderType = Enumerations.Order.OrderTypes.MARKET.ToString();
                            //    }
                            //    else if (oOrderResponse.OrderType == Enumerations.OrderTypeShort.K.ToString())
                            //    {
                            //        oOrderResponse.OrderType = Enumerations.Order.OrderTypes.BLOCKDEAL.ToString();
                            //    }
                            //    else if (oOrderResponse.OrderType == Enumerations.OrderTypeShort.SL.ToString())
                            //    {
                            //        oOrderResponse.OrderType = Enumerations.Order.OrderTypes.STOPLOSS.ToString();
                            //    }
                            //    else if (oOrderResponse.OrderType == Enumerations.OrderTypeShort.SLMkt.ToString())
                            //    {
                            //        oOrderResponse.OrderType = Enumerations.Order.OrderTypes.STOPLOSSMKT.ToString();
                            //    }
                            //}





                            //if (oOrderResponse.OrderAction ==)
                            //{
                            //    OrderResponseProcessor oOrderResponseProcessor = new OrderResponseProcessor(new AddOrder());
                            //    oOrderResponseProcessor.ProcessResponse(oOrderResponse);
                            //    //if (oOrderResponse != null)
                            //    //{
                            //    //    //var GroupWiseLimit = MasterSharedMemory.GroupWiseLimitDict[oOrderResponse.Group];
                            //    //    oOrderResponse = MemoryManager.OrderDictionary[oOrderResponse.MessageTag];
                            //    //    if (oOrderResponse != null)
                            //    //    {
                            //    //        var GroupWiseLimit = MasterSharedMemory.GroupWiseLimitDict[oOrderResponse.Group];
                            //    //        OnGroupwiseLimitReceive?.Invoke(GroupWiseLimit);
                            //    //    }

                            //    //}

                            //}
                            //else if (oOrderResponse.OrderAction == Enumerations.Order.Modes.U.ToString())
                            //{
                            //    OrderResponseProcessor oOrderResponseProcessor = new OrderResponseProcessor(new ModifyOrder());
                            //    oOrderResponseProcessor.ProcessResponse(oOrderResponse);
                            //    //if (oOrderResponse != null)
                            //    //{
                            //    //    oOrderResponse = MemoryManager.OrderDictionary[oOrderResponse.MessageTag];
                            //    //    if (oOrderResponse != null)
                            //    //    {
                            //    //        var GroupWiseLimit = MasterSharedMemory.GroupWiseLimitDict[oOrderResponse.Group];
                            //    //        OnGroupwiseLimitReceive?.Invoke(GroupWiseLimit);
                            //    //    }
                            //    //}
                            //}
                            //else if (oOrderResponse.OrderAction == Enumerations.Order.Modes.D.ToString())
                            //{
                            //    OrderResponseProcessor oOrderResponseProcessor = new OrderResponseProcessor(new DeleteOrder());
                            //    oOrderResponseProcessor.ProcessResponse(oOrderResponse);
                            //    //if (oOrderResponse != null)
                            //    //{
                            //    //    oOrderResponse = MemoryManager.OrderDictionary[oOrderResponse.MessageTag];
                            //    //    if (oOrderResponse != null)
                            //    //    {
                            //    //        var GroupWiseLimit = MasterSharedMemory.GroupWiseLimitDict[oOrderResponse.Group];
                            //    //        OnGroupwiseLimitReceive?.Invoke(GroupWiseLimit);
                            //    //    }
                            //    //}
                            //}

                            #endregion
                            break;
                        case 210:
                            // A group and limit(2)

                            GroupWiseLimitsModel gmodfel = new GroupWiseLimitsModel();
                            gmodfel.Group = "A";//back
                            gmodfel.BuyValue = 20000;//back
                            gmodfel.SellValue = 20000; //back 
                            gmodfel.AvlBuy = 20000;
                            gmodfel.AvlSell = 20000;


                            //update ;
                            bool update = MasterSharedMemory.GroupWiseLimitDict.TryUpdate(gmodfel.Group, gmodfel, MasterSharedMemory.GroupWiseLimitDict[gmodfel.Group]);
                            //gmodfel = new GroupWiseLimitsModel();
                            //OnGroupwiseLimitReceive(gmodfel);
                            if (OnGroupwiseLimitReceive != null)
                            {
                                OnGroupwiseLimitReceive.Invoke(gmodfel);
                            }
                            //OnGroupwiseLimitReceive?.Invoke();
                            update = false;
                            break;

                        #region OrderPersonal Download
                        case (int)Enumerations.OrderTypeDownload.NormalOrders:
                        case (int)Enumerations.OrderTypeDownload.StopLossOrders:
                        case (int)Enumerations.OrderTypeDownload.ReturnOrders:
                        case (int)Enumerations.OrderTypeDownload.ReturnStopLossOrders:
                            EOrderNomralReply oEOrderNomralReply = new EOrderNomralReply();
                            oEOrderNomralReply = oModel as EOrderNomralReply;
                            if (oEOrderNomralReply.ReplyCode == 0)
                            {
                                UtilityLoginDetails.GETInstance.PersonalReplyReceived = true;
                                switch (msgType)
                                {
                                    case (int)Enumerations.OrderTypeDownload.NormalOrders:
                                        if (oEOrderNomralReply.MessageTag == (int)AdminTradeRequestMsgTag.EQTYSEGINDICATOR)
                                        {
                                            PersonalDownloadVM.GetInstance.imgticksOrdEqVisibility = "Visible";

                                        }
                                        else if (oEOrderNomralReply.MessageTag == (int)AdminTradeRequestMsgTag.DERISEGINDICATOR)
                                        {
                                            PersonalDownloadVM.GetInstance.imgticksOrdDerVisibility = "Visible";
                                        }
                                        else if (oEOrderNomralReply.MessageTag == (int)AdminTradeRequestMsgTag.CURRSEGINDICATOR)
                                        {
                                            PersonalDownloadVM.GetInstance.imgticksOrdCurVisibility = "Visible";
                                        }
                                        break;
                                    case (int)Enumerations.OrderTypeDownload.ReturnOrders:
                                        if (oEOrderNomralReply.MessageTag == (int)AdminTradeRequestMsgTag.EQTYSEGINDICATOR)
                                        {
                                            PersonalDownloadVM.GetInstance.imgticksRtOrdEqVisibility = "Visible";

                                        }
                                        else if (oEOrderNomralReply.MessageTag == (int)AdminTradeRequestMsgTag.DERISEGINDICATOR)
                                        {
                                            PersonalDownloadVM.GetInstance.imgticksRtOrdDerVisibility = "Visible";
                                        }
                                        else if (oEOrderNomralReply.MessageTag == (int)AdminTradeRequestMsgTag.CURRSEGINDICATOR)
                                        {
                                            PersonalDownloadVM.GetInstance.imgticksRtOrdCurVisibility = "Visible";
                                        }
                                        break;
                                    case (int)Enumerations.OrderTypeDownload.StopLossOrders:
                                        if (oEOrderNomralReply.MessageTag == (int)AdminTradeRequestMsgTag.EQTYSEGINDICATOR)
                                        {
                                            PersonalDownloadVM.GetInstance.imgticksSLOrdEqVisibility = "Visible";

                                        }
                                        else if (oEOrderNomralReply.MessageTag == (int)AdminTradeRequestMsgTag.DERISEGINDICATOR)
                                        {
                                            PersonalDownloadVM.GetInstance.imgticksSLOrdDerVisibility = "Visible";
                                        }
                                        else if (oEOrderNomralReply.MessageTag == (int)AdminTradeRequestMsgTag.CURRSEGINDICATOR)
                                        {
                                            PersonalDownloadVM.GetInstance.imgticksSLOrdCurVisibility = "Visible";
                                        }
                                        break;

                                    case (int)Enumerations.OrderTypeDownload.ReturnStopLossOrders:
                                        if (oEOrderNomralReply.MessageTag == (int)AdminTradeRequestMsgTag.EQTYSEGINDICATOR)
                                        {
                                            PersonalDownloadVM.GetInstance.imgticksRtSLOrdEqVisibility = "Visible";
                                        }
                                        else if (oEOrderNomralReply.MessageTag == (int)AdminTradeRequestMsgTag.DERISEGINDICATOR)
                                        {
                                            PersonalDownloadVM.GetInstance.imgticksRtSLOrdDerVisibility = "Visible";
                                        }
                                        else if (oEOrderNomralReply.MessageTag == (int)AdminTradeRequestMsgTag.CURRSEGINDICATOR)
                                        {
                                            PersonalDownloadVM.GetInstance.imgticksRtSLOrdCurVisibility = "Visible";
                                        }
                                        break;
                                }

                                if (UtilityLoginDetails.GETInstance.PersonalReplyReceived && UtilityLoginDetails.GETInstance.EndofdownloadReceived)
                                    PersonalDownloadVM.autoReset.Set();
                            }
                            else
                            {
                                MessageBoxResult oMessageBoxResult = System.Windows.MessageBox.Show("Personal Download Failed.\n Do you want to continue?", "Warning!", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                                if (oMessageBoxResult == MessageBoxResult.Yes)
                                {
                                    PersonalDownloadVM.autoReset.Set();
                                    UtilityLoginDetails.GETInstance.PersonalReplyReceived = false;
                                    UtilityLoginDetails.GETInstance.EndofdownloadReceived = false;

                                    switch (msgType)
                                    {
                                        case (int)Enumerations.OrderTypeDownload.NormalOrders:
                                            if (oEOrderNomralReply.MessageTag == (int)AdminTradeRequestMsgTag.EQTYSEGINDICATOR)
                                            {
                                                PersonalDownloadVM.GetInstance.imgCrossOrdEqVisibility = "Visible";

                                            }
                                            else if (oEOrderNomralReply.MessageTag == (int)AdminTradeRequestMsgTag.DERISEGINDICATOR)
                                            {
                                                PersonalDownloadVM.GetInstance.imgCrossOrdDerVisibility = "Visible";
                                            }
                                            else if (oEOrderNomralReply.MessageTag == (int)AdminTradeRequestMsgTag.CURRSEGINDICATOR)
                                            {
                                                PersonalDownloadVM.GetInstance.imgCrossOrdCurVisibility = "Visible";
                                            }
                                            break;
                                        case (int)Enumerations.OrderTypeDownload.ReturnOrders:
                                            if (oEOrderNomralReply.MessageTag == (int)AdminTradeRequestMsgTag.EQTYSEGINDICATOR)
                                            {
                                                PersonalDownloadVM.GetInstance.imgCrossRtOrdEqVisibility = "Visible";

                                            }
                                            else if (oEOrderNomralReply.MessageTag == (int)AdminTradeRequestMsgTag.DERISEGINDICATOR)
                                            {
                                                PersonalDownloadVM.GetInstance.imgCrossRtOrdDerVisibility = "Visible";
                                            }
                                            else if (oEOrderNomralReply.MessageTag == (int)AdminTradeRequestMsgTag.CURRSEGINDICATOR)
                                            {
                                                PersonalDownloadVM.GetInstance.imgCrossRtOrdCurVisibility = "Visible";
                                            }
                                            break;
                                        case (int)Enumerations.OrderTypeDownload.StopLossOrders:
                                            if (oEOrderNomralReply.MessageTag == (int)AdminTradeRequestMsgTag.EQTYSEGINDICATOR)
                                            {
                                                PersonalDownloadVM.GetInstance.imgCrossSLOrdEqVisibility = "Visible";
                                            }
                                            else if (oEOrderNomralReply.MessageTag == (int)AdminTradeRequestMsgTag.DERISEGINDICATOR)
                                            {
                                                PersonalDownloadVM.GetInstance.imgCrossSLOrdDerVisibility = "Visible";
                                            }
                                            else if (oEOrderNomralReply.MessageTag == (int)AdminTradeRequestMsgTag.CURRSEGINDICATOR)
                                            {
                                                PersonalDownloadVM.GetInstance.imgCrossSLOrdCurVisibility = "Visible";
                                            }
                                            break;

                                        case (int)Enumerations.OrderTypeDownload.ReturnStopLossOrders:
                                            if (oEOrderNomralReply.MessageTag == (int)AdminTradeRequestMsgTag.EQTYSEGINDICATOR)
                                            {
                                                PersonalDownloadVM.GetInstance.imgCrossRtSLOrdEqVisibility = "Visible";
                                            }
                                            else if (oEOrderNomralReply.MessageTag == (int)AdminTradeRequestMsgTag.DERISEGINDICATOR)
                                            {
                                                PersonalDownloadVM.GetInstance.imgCrossRtSLOrdDerVisibility = "Visible";
                                            }
                                            else if (oEOrderNomralReply.MessageTag == (int)AdminTradeRequestMsgTag.CURRSEGINDICATOR)
                                            {
                                                PersonalDownloadVM.GetInstance.imgCrossRtSLOrdCurVisibility = "Visible";
                                            }
                                            break;
                                    }
                                }
                                else
                                {
                                    Process.GetCurrentProcess().Kill();
                                }
                            }




                            break;
                        //case 1097:
                        //    //TODO: Process the response accordingly
                        //    break;
                        //case 1170:
                        //    oEOrderNomralReply = new EOrderNomralReply();
                        //    oEOrderNomralReply = oModel as EOrderNomralReply;

                        //    if (oEOrderNomralReply.ReplyCode == 0)
                        //        UtilityLoginDetails.GETInstance.PersonalReplyReceived = true;

                        //    if (UtilityLoginDetails.GETInstance.PersonalReplyReceived && UtilityLoginDetails.GETInstance.EndofdownloadReceived)
                        //        PersonalDownloadVM.autoReset.Set();

                        //    break;
                        //case 1173:
                        //    //TODO: Process the response accordingly
                        //    break;
                        #endregion

                        #region offline TradeLimit Download

                        case 22002: // Offline trade limit
                            ETradeLimitReply oETradeLimitReply = new ETradeLimitReply();
                            oETradeLimitReply = oModel as ETradeLimitReply;

                            if (oETradeLimitReply != null && oETradeLimitReply.ReplyCode == 0)
                            {
                                PersonalDownloadVM.GetInstance.TradeWiseLimitCount += 1;

                                UtilityLoginDetails.GETInstance.PersonalReplyReceived = true;
                                UtilityLoginDetails.GETInstance.EndofdownloadReceived = true;

                                CommonFunctions.PopulateLimitMemory(msgType, oETradeLimitReply);

                                if (oETradeLimitReply.MsgTag == (int)AdminTradeRequestMsgTag.EQTYSEGINDICATOR)
                                {
                                    //MainWindowVM.objLogOnStatus[(int)Segment.Equity].GrossBuyLimit = oETradeLimitReply.GrossLimitBuy;
                                    //MainWindowVM.objLogOnStatus[(int)Segment.Equity].GrossSellLimit = oETradeLimitReply.GrossLimitSell;
                                    //MainWindowVM.objLogOnStatus[(int)Segment.Equity].NetValue = oETradeLimitReply.NetValue;
                                    PersonalDownloadVM.GetInstance.imgticksTrdLmtEqVisibility = "Visible";
                                }

                                else if (oETradeLimitReply.MsgTag == (int)AdminTradeRequestMsgTag.DERISEGINDICATOR)
                                {
                                    //MainWindowVM.objLogOnStatus[(int)Segment.Derivative].GrossBuyLimit = oETradeLimitReply.GrossLimitBuy;
                                    //MainWindowVM.objLogOnStatus[(int)Segment.Derivative].GrossSellLimit = oETradeLimitReply.GrossLimitSell;
                                    //MainWindowVM.objLogOnStatus[(int)Segment.Derivative].NetValue = oETradeLimitReply.NetValue;
                                    PersonalDownloadVM.GetInstance.imgticksTrdLmtDerVisibility = "Visible";
                                }
                                else if (oETradeLimitReply.MsgTag == (int)AdminTradeRequestMsgTag.CURRSEGINDICATOR)
                                {
                                    //MainWindowVM.objLogOnStatus[(int)Segment.Currency].GrossBuyLimit = oETradeLimitReply.GrossLimitBuy;
                                    //MainWindowVM.objLogOnStatus[(int)Segment.Currency].GrossSellLimit = oETradeLimitReply.GrossLimitSell;
                                    //MainWindowVM.objLogOnStatus[(int)Segment.Currency].NetValue = oETradeLimitReply.NetValue;
                                    PersonalDownloadVM.GetInstance.imgticksTrdLmtCurVisibility = "Visible";
                                }

                                if (UtilityLoginDetails.GETInstance.PersonalReplyReceived && UtilityLoginDetails.GETInstance.EndofdownloadReceived)
                                    PersonalDownloadVM.autoReset.Set();
                            }

                            else
                            {
                                string Segment = string.Empty;
                                if (oETradeLimitReply.MsgTag == (int)AdminTradeRequestMsgTag.EQTYSEGINDICATOR)
                                    Segment = "Equity";
                                else if (oETradeLimitReply.MsgTag == (int)AdminTradeRequestMsgTag.DERISEGINDICATOR)
                                    Segment = "Derivative";
                                else if (oETradeLimitReply.MsgTag == (int)AdminTradeRequestMsgTag.CURRSEGINDICATOR)
                                    Segment = "Currency";

                                MessageBoxResult oMessageBoxResult = System.Windows.MessageBox.Show(Segment + " Limits Download Failed, Not able to trade in " + Segment + "\n Do you want to continue?", "!Error!", MessageBoxButton.YesNo, MessageBoxImage.Error);
                                if (oMessageBoxResult == MessageBoxResult.Yes)
                                {
                                    PersonalDownloadVM.autoReset.Set();
                                    UtilityLoginDetails.GETInstance.PersonalReplyReceived = false;
                                    UtilityLoginDetails.GETInstance.EndofdownloadReceived = false;
                                    if (oETradeLimitReply.MsgTag == (int)AdminTradeRequestMsgTag.EQTYSEGINDICATOR)
                                    {
                                        PersonalDownloadVM.GetInstance.imgCrossTrdLmtEqVisibility = "Visible";
                                    }

                                    else if (oETradeLimitReply.MsgTag == (int)AdminTradeRequestMsgTag.DERISEGINDICATOR)
                                    {
                                        PersonalDownloadVM.GetInstance.imgCrossTrdLmtDerVisibility = "Visible";
                                    }
                                    else if (oETradeLimitReply.MsgTag == (int)AdminTradeRequestMsgTag.CURRSEGINDICATOR)
                                    {
                                        PersonalDownloadVM.GetInstance.imgCrossTrdLmtCurVisibility = "Visible";
                                    }
                                }
                                else
                                {
                                    Process.GetCurrentProcess().Kill();
                                }
                            }
                            break;

                        case 22012: // Offline Groupwise Trade limit
                            ETradeGWLimitReply oETradeGWLimitReply = new ETradeGWLimitReply();
                            oETradeGWLimitReply = oModel as ETradeGWLimitReply;

                            if (oETradeGWLimitReply != null && oETradeGWLimitReply.ReplyCode == 0)
                            {
                                UtilityLoginDetails.GETInstance.PersonalReplyReceived = true;
                                UtilityLoginDetails.GETInstance.EndofdownloadReceived = true;
                                if (oETradeGWLimitReply.lstTraderGwLimitGrp != null)
                                {
                                    CommonFunctions.PopulateLimitMemory(msgType, oETradeGWLimitReply);
                                    for (int i = 0; i < oETradeGWLimitReply.lstTraderGwLimitGrp.Count; i++)
                                    {
                                        PersonalDownloadVM.GetInstance.GroupWiseLimitCount += 1;
                                        /*
                                        EGroupWiseLimitDet oEGroupWiseLimitDet = new EGroupWiseLimitDet();
                                        oEGroupWiseLimitDet = (EGroupWiseLimitDet)oETradeGWLimitReply.lstTraderGwLimitGrp[i];
                                        if (oEGroupWiseLimitDet != null)
                                        {
                                            if (MasterSharedMemory.GroupWiseLimitDict.ContainsKey(oEGroupWiseLimitDet.GroupName.Trim().ToUpper()))
                                            {
                                                GroupWiseLimitsModel oGroupWiseLimitsModel = new GroupWiseLimitsModel();
                                                oGroupWiseLimitsModel.Group = oEGroupWiseLimitDet.GroupName.Trim();
                                                oGroupWiseLimitsModel.BuyValue = oEGroupWiseLimitDet.BuyValue;
                                                oGroupWiseLimitsModel.SellValue = oEGroupWiseLimitDet.SellValue;

                                                MasterSharedMemory.GroupWiseLimitDict.TryUpdate(oGroupWiseLimitsModel.Group, oGroupWiseLimitsModel, MasterSharedMemory.GroupWiseLimitDict[oGroupWiseLimitsModel.Group]);
                                            }


                                            //MasterSharedMemory.GroupWiseLimitDict.Where(x=>x.Key.Trim().ToUpper().Equals(oEGroupWiseLimitDet.GroupName.Trim().ToUpper()))
                                            //oEGroupWiseLimitDet.BuyValue = 
                                            //UPdate dict accordingly
                                        }*/
                                    }

                                    if (oETradeGWLimitReply.MsgTag == (int)AdminTradeRequestMsgTag.EQTYSEGINDICATOR)
                                    {
                                        PersonalDownloadVM.GetInstance.imgticksGrpLmtEqVisibility = "Visible";
                                    }

                                    else if (oETradeGWLimitReply.MsgTag == (int)AdminTradeRequestMsgTag.DERISEGINDICATOR)
                                    {
                                        PersonalDownloadVM.GetInstance.imgticksGrpLmtDerVisibility = "Visible";
                                    }
                                    else if (oETradeGWLimitReply.MsgTag == (int)AdminTradeRequestMsgTag.CURRSEGINDICATOR)
                                    {
                                        PersonalDownloadVM.GetInstance.imgticksGrpLmtCurVisibility = "Visible";
                                    }
                                }

                                if (UtilityLoginDetails.GETInstance.PersonalReplyReceived && UtilityLoginDetails.GETInstance.EndofdownloadReceived)
                                    PersonalDownloadVM.autoReset.Set();
                            }

                            else
                            {
                                string Segment = string.Empty;
                                if (oETradeGWLimitReply.MsgTag == (int)AdminTradeRequestMsgTag.EQTYSEGINDICATOR)
                                    Segment = "Equity";
                                else if (oETradeGWLimitReply.MsgTag == (int)AdminTradeRequestMsgTag.DERISEGINDICATOR)
                                    Segment = "Derivative";
                                else if (oETradeGWLimitReply.MsgTag == (int)AdminTradeRequestMsgTag.CURRSEGINDICATOR)
                                    Segment = "Currency";

                                MessageBoxResult oMessageBoxResult = System.Windows.MessageBox.Show(Segment + " Group Limits Download Failed, Not able to trade in " + Segment + "\n Do you want to continue?", "!Error!", MessageBoxButton.YesNo, MessageBoxImage.Error);

                                if (oMessageBoxResult == MessageBoxResult.Yes)
                                {
                                    PersonalDownloadVM.autoReset.Set();
                                    UtilityLoginDetails.GETInstance.PersonalReplyReceived = false;
                                    UtilityLoginDetails.GETInstance.EndofdownloadReceived = false;
                                    if (oETradeGWLimitReply.MsgTag == (int)AdminTradeRequestMsgTag.EQTYSEGINDICATOR)
                                    {
                                        PersonalDownloadVM.GetInstance.imgCrossGrpLmtEqVisibility = "Visible";
                                    }

                                    else if (oETradeGWLimitReply.MsgTag == (int)AdminTradeRequestMsgTag.DERISEGINDICATOR)
                                    {
                                        PersonalDownloadVM.GetInstance.imgCrossGrpLmtDerVisibility = "Visible";
                                    }
                                    else if (oETradeGWLimitReply.MsgTag == (int)AdminTradeRequestMsgTag.CURRSEGINDICATOR)
                                    {
                                        PersonalDownloadVM.GetInstance.imgCrossGrpLmtCurVisibility = "Visible";
                                    }

                                }
                                else
                                {
                                    Process.GetCurrentProcess().Kill();
                                }
                            }
                            break;

                        #endregion

                        default:
                            break;
                    }
                }
            }
            //update/empty the memory slot
            UpdateRequestReplyMappingMemory((SlotNum), true, false);
            //if (MemoryManager.RequestDict != null && MemoryManager.RequestDict.Count > 0)
            //    MemoryManager.RequestDict.Remove(SlotNum);
        }
        #endregion

        private static void PopulateCAMainMemory()//after personal download
        {
            DataAccessLayer oDataAccessLayer = new DataAccessLayer();
            oDataAccessLayer.Connect((int)DataAccessLayer.ConnectionDB.Masters);
            try
            {
                Thread.Sleep(10); // Wait for picking up date time after successfull login
                objCorpActBSE = new Dictionary<string, CorporateActionModel>();
                // CorpActForOE = new ObservableCollection<string>();
                CorpActForOE = new Dictionary<long, string>();
                StringBuilder sb;
                CDate = CommonFunctions.GetDate();//after successfull login

                if (CDate != null)
                    CurrentDate = CDate.ToString("dd-MM-yyyy");
                else
                {
                    CDate = DateTime.Now.Date;
                    CurrentDate = CDate.ToString("dd-MM-yyyy");
                }

                pastDate = CDate.AddDays(-10).ToString("dd-MM-yyyy");

                #region change
                oDataAccessLayer.OpenConnection((int)DataAccessLayer.ConnectionDB.Masters);
                try
                {
                    string strQuery = @"SELECT * FROM BSE_EQ_CORPACT_CFE where ((ScripCode BETWEEN 500000 AND 600000) OR (ScripCode>=700000));";

                    SQLiteDataReader oSQLiteDataReader = oDataAccessLayer.ExecuteDataReader((int)ConnectionDB.Masters,strQuery, System.Data.CommandType.Text, null);

                    while (oSQLiteDataReader.Read())
                    {
                        sb = new StringBuilder();
                        CorporateActionModel objCorpAct = new CorporateActionModel();

                        var ScripCode = oSQLiteDataReader["ScripCode"]?.ToString().Trim();
                        if (!string.IsNullOrEmpty(ScripCode))
                            objCorpAct.scripCode = Convert.ToInt64(ScripCode);

                        var BookClosureFrom = oSQLiteDataReader["BookClosureFrom"]?.ToString().Trim();
                        if (!string.IsNullOrEmpty(BookClosureFrom))
                        {
                            objCorpAct.bookClosureFrom = Convert.ToDateTime(BookClosureFrom).ToString("dd-MM-yyyy");
                            sb.Append("BCStartDate: " + objCorpAct.bookClosureFrom + "  | ");
                            //objCorpAct.bookClosureFrom = DateTime.ParseExact(BookClosureFrom, @"M/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString("dd-MM-yyyy");
                        }

                        var BookClosureTo = oSQLiteDataReader["BookClosureTo"]?.ToString().Trim();
                        if (!string.IsNullOrEmpty(BookClosureTo))
                            objCorpAct.bookClosureTo = Convert.ToDateTime(BookClosureTo).ToString("dd-MM-yyyy");
                        //objCorpAct.bookClosureTo = DateTime.ParseExact(BookClosureTo, @"M/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString("dd-MM-yyyy");

                        var PurposeOrEvent = oSQLiteDataReader["PurposeOrEvent"]?.ToString().Trim();
                        if (!string.IsNullOrEmpty(PurposeOrEvent))
                        {
                            objCorpAct.purposeOrEvent = PurposeOrEvent;
                            sb.Append("Purpose: " + objCorpAct.purposeOrEvent + "  | ");
                        }
                        var BcOrRdFlag = oSQLiteDataReader["BcOrRdFlag"]?.ToString().Trim();
                        if (!string.IsNullOrEmpty(BcOrRdFlag))
                            objCorpAct.bcOrRdFlag = BcOrRdFlag;

                        var ExDate = oSQLiteDataReader["ExDate"]?.ToString().Trim();
                        if (!string.IsNullOrEmpty(ExDate))
                        {
                            objCorpAct.exDate = Convert.ToDateTime(ExDate).ToString("dd-MM-yyyy");
                            sb.Append("ExDate: " + objCorpAct.exDate + "  | ");
                        }
                        //objCorpAct.exDate = DateTime.ParseExact(ExDate, @"M/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString("dd-MM-yyyy");
                        if (BcOrRdFlag.Equals("RD") && string.IsNullOrEmpty(ExDate) && !string.IsNullOrEmpty(BookClosureFrom))
                        {
                            objCorpAct.exDate = Convert.ToDateTime(BookClosureFrom).ToString("dd-MM-yyyy");
                            objCorpAct.recordDate = Convert.ToDateTime(BookClosureFrom).ToString("dd-MM-yyyy");
                            //objCorpAct.exDate = DateTime.ParseExact(BookClosureFrom, @"M/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString("dd-MM-yyyy"); ;
                            //objCorpAct.recordDate = DateTime.ParseExact(BookClosureFrom, @"M/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString("dd-MM-yyyy");
                        }
                        if (BcOrRdFlag.Equals("RD") && !string.IsNullOrEmpty(ExDate))
                        {
                            objCorpAct.recordDate = Convert.ToDateTime(BookClosureFrom).ToString("dd-MM-yyyy");
                            sb.Append("RecDate: " + objCorpAct.recordDate + "  | ");
                        }
                        // objCorpAct.recordDate = DateTime.ParseExact(BookClosureFrom, @"M/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString("dd-MM-yyyy");
                        if (BcOrRdFlag.Equals("BC") && string.IsNullOrEmpty(ExDate) && !string.IsNullOrEmpty(BookClosureFrom))
                            objCorpAct.exDate = Convert.ToDateTime(BookClosureFrom).ToString("dd-MM-yyyy");
                        //objCorpAct.exDate = DateTime.ParseExact(BookClosureFrom, @"M/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString("dd-MM-yyyy");

                        var ScripName = oSQLiteDataReader["ScripName"]?.ToString().Trim();
                        if (!string.IsNullOrEmpty(ScripName))
                            objCorpAct.scripName = ScripName;

                        var ApplicableFor = oSQLiteDataReader["ApplicableFor"]?.ToString().Trim();
                        if (!string.IsNullOrEmpty(ApplicableFor))
                            objCorpAct.applicableFor = ApplicableFor;

                        var NDStartSetlNumber = oSQLiteDataReader["NDStartSetlNumber"]?.ToString().Trim();
                        if (!string.IsNullOrEmpty(NDStartSetlNumber))
                            objCorpAct.NDStartSetlNumber = NDStartSetlNumber;

                        var NDEndSetlNumber = oSQLiteDataReader["NDEndSetlNumber"]?.ToString().Trim();
                        if (!string.IsNullOrEmpty(NDEndSetlNumber))
                            objCorpAct.NDEndSetlNumber = NDEndSetlNumber;

                        var DeliverySettlement = oSQLiteDataReader["DeliverySettlement"]?.ToString().Trim();
                        if (!string.IsNullOrEmpty(DeliverySettlement))
                            objCorpAct.deliverySettlement = DeliverySettlement;

                        var NdStartDate = oSQLiteDataReader["NdStartDate"]?.ToString().Trim();
                        if (!string.IsNullOrEmpty(NdStartDate))
                            objCorpAct.NdStartDate = Convert.ToDateTime(NdStartDate).ToString("dd-MM-yyyy");
                        //objCorpAct.NdStartDate = DateTime.ParseExact(NdStartDate, @"M/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString("dd-MM-yyyy");

                        var NdEndDate = oSQLiteDataReader["NdEndDate"]?.ToString().Trim();
                        if (!string.IsNullOrEmpty(NdEndDate))
                            objCorpAct.ndEndDate = Convert.ToDateTime(NdEndDate).ToString("dd-MM-yyyy");
                        //objCorpAct.ndEndDate = DateTime.ParseExact(NdEndDate, @"M/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString("dd-MM-yyyy");

                        var NdOrNDAndExFlag = oSQLiteDataReader["NdOrNDAndExFlag"]?.ToString().Trim();
                        if (!string.IsNullOrEmpty(NdOrNDAndExFlag))
                        {
                            objCorpAct.NdOrNDAndExFlag = NdOrNDAndExFlag;
                            sb.Append("NDPeriod: " + objCorpAct.NdOrNDAndExFlag);
                        }

                        var grp = CommonFunctions.GetGroupName(objCorpAct.scripCode, Enumerations.Exchange.BSE, Enumerations.Segment.Equity);
                        if (!string.IsNullOrEmpty(grp))
                            objCorpAct.Group = grp;
                        else
                            objCorpAct.Group = "NA";

                        var scripId = CommonFunctions.GetScripId(objCorpAct.scripCode, Enumerations.Exchange.BSE, Enumerations.Segment.Equity);
                        if (!string.IsNullOrEmpty(scripId))
                            objCorpAct.ScripID = scripId;
                        else
                            objCorpAct.ScripID = "NA";

                        var key = string.Format("{0}_{1}", objCorpAct.scripCode, objCorpAct.bcOrRdFlag);
                        if (!objCorpActBSE.ContainsKey(key))
                        {
                            int res = CommonFunctions.CompareDate(objCorpAct.exDate, CurrentDate);
                            if (res == 0 || res == 1 || res == 2 || res == 5)
                            {
                                objCorpActBSE.Add(key, objCorpAct);
                                if (!CorpActForOE.ContainsKey(objCorpAct.scripCode))
                                    CorpActForOE.Add(objCorpAct.scripCode, sb.ToString());
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    ExceptionUtility.LogError(ex);
                }
                finally
                {
                    oDataAccessLayer.CloseConnection((int)DataAccessLayer.ConnectionDB.Masters);
                }
                #endregion
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
            }
        }

        private static string Encrypt(char[] p, int key)
        {
            for (int i = 0; i < p.Length; ++i)
            {
                p[i] -= (char)key;
                //EncrypterdPwd[i] = (char)(EncrypterdPwd[i] + key);
            }

            return new string(p);
        }
        public static uint GetMesageTag()
        {
            return MessageTag = MessageTag + 1;
        }

        public static void ProcessOnlineTrade()
        {

            #region Commented as Per New Online trades logic based on unique keys
            // string matchkeyOnlineTrade = string.Empty;
            //object result;
            //do
            //{



            //    if (OnlineTradeQueue.TryDequeue(out result))
            //    {
            //        var scripcode = ((TradeUMS)result).SecurityID;
            //        var tradeID = ((TradeUMS)result).TradeID;
            //        var BSFlag = ((TradeUMS)result).Side;// Enum.GetName(typeof(Enumerations.Side), ((TradeUMS)result).Side);

            //        #region Commented 27/03/2017 Gaurav


            //        //matchkeyOnlineTrade = string.Format("{0}_{1}_{2}", scripcode, tradeID, BSFLAG);

            //        //if (!TradeMemoryConDict.Any(x => ((TradeUMS)x.Value).SecurityID == scripcode)
            //        //                            && !TradeMemoryConDict.Any(x => ((TradeUMS)x.Value).TradeID == tradeID)
            //        //                            && !TradeMemoryConDict.Any(x => ((TradeUMS)x.Value).Side == BSFlag)
            //        //    )
            //        //{
            //        //    TradeMemoryConDict.TryAdd(TradeMemoryConDict.Count, result);
            //        //}
            //        #endregion


            //        foreach (var item in TradeMemoryConDict)
            //        {
            //            TradeUMS oTempTradeUMS = new TradeUMS();
            //            oTempTradeUMS = (TradeUMS)item.Value;
            //            if (oTempTradeUMS.SecurityID != scripcode && oTempTradeUMS.TradeID != tradeID && oTempTradeUMS.Side != BSFlag)
            //            {
            //                TradeMemoryConDict.TryAdd(TradeMemoryConDict.Count, result);
            //            }
            //        }


            //    }
            //} while (OnlineTradeQueue.Count > 0);
            #endregion

            //Process parked trade during personal download
            TradeResponseProcessor oTradeResponseProcessor = null;
            var onlineTradeQueueCount = OnlineTradeQueue.Count;
            for (int i = 0; i < onlineTradeQueueCount; i++)
            {
                var onlineTrade = OnlineTradeQueue.Dequeue();

                ETraderTradeOnlineUMS oETraderTradeOnlineUMS = new ETraderTradeOnlineUMS();
                oETraderTradeOnlineUMS = onlineTrade as ETraderTradeOnlineUMS;

                oTradeResponseProcessor = new TradeResponseProcessor(new TraderTradeProcessOnlineTrade());
                oTradeResponseProcessor.ProcessResponse(oETraderTradeOnlineUMS);

                SharedMemories.MemoryManager.TradeCount = SharedMemories.MemoryManager.TradeCount + 1;

                oTradeResponseProcessor = new TradeResponseProcessor(new UpdateOrderMemoryOnlineTrade());
                oTradeResponseProcessor.ProcessResponse(oETraderTradeOnlineUMS);

                if (SharedMemories.MemoryManager.OnTradeUMSRecieved != null)
                    SharedMemories.MemoryManager.OnTradeUMSRecieved(TradeCount);
            }

            //sorting logic
            var sortedData = TradeMemoryConDict.OrderByDescending(x => ((TradeUMS)x.Value).TimeOnly).ToList();
            TradeMemoryConDict.Clear();
            for (int i = 0; i < sortedData.Count(); i++)
            {
                TradeMemoryConDict.TryAdd(i, sortedData[i].Value);
            }

            #region Commented


            //TradeMemoryConDict.OrderBy(x => ((TradeUMS)x.Value).TimeOnly);

            //foreach (KeyValuePair<long, object> item in TradeMemoryConDict)
            // {
            // Task.Factory.StartNew(() => UMSProcessor.ProcessTraderTradeData(item.Value));

            ////UMSProcessor.ProcessNetPositionCWDemo(item.Value);
            ////UMSProcessor.ProcessNetPositionSWDemo(item.Value);
            ////UMSProcessor.ProcessNetPositionSWCWDetailsDemo(item.Value);
            ////UMSProcessor.ProcessNetPositionCWSWDetailsDemo(item.Value);
            //UMSProcessor.ProcessTraderTradeData(item.Value);

            //Thread th = new Thread(()=>UMSProcessor.ProcessNetPositionCWDemo(item.Value));
            //th.Start();

            //Parallel.Invoke(() =>
            //                   UMSProcessor.ProcessNetPositionCWDemo(item.Value),
            //                   () => UMSProcessor.ProcessNetPositionSWDemo(item.Value),
            //                   () => UMSProcessor.ProcessNetPositionSWCWDetailsDemo(item.Value),
            //                   () => UMSProcessor.ProcessNetPositionCWSWDetailsDemo(item.Value),
            //                   () => UMSProcessor.ProcessTraderTradeData(item.Value));
            //  }

            //sTask.Factory.StartNew(() => Display());

            //ThreadStart thstart = new ThreadStart(Display);
            //Thread th = new Thread(thstart);
            //th.IsBackground = true;
            //th.Start();

            //NetPositionCalculate oNetPositionCalculate = new NetPositionCalculate();
            //oNetPositionCalculate.ProcessClientWiseTrade();
            //oNetPositionCalculate.ProcessScripWiseTrade();

            //ParallelOptions oParallelOptions = new ParallelOptions();
            //oParallelOptions.MaxDegreeOfParallelism = 1;



            //Parallel.Invoke(oParallelOptions,
            //             new Action(() => Run())
            //         );     

            //CommonFrontEnd.Controller.ProcessTradeMemoryPartition.CreatePartition();
            #endregion

            NetPositionCalculateAsync oNetPositionCalculateAsync = new NetPositionCalculateAsync();

            oNetPositionCalculateAsync.ProcessScripWiseTradeAsyncPD();
            oNetPositionCalculateAsync.ProcessClientWiseTradeAsyncPD();
            oNetPositionCalculateAsync.ProcessSaudasTradeAsyncPD();
            IsPersonalDownloadComplete = true;
            TradeResponseReceivedFlag = true;
            OnlineTradeQueue = null;



            #region Commented for partitions


            //     int[] nums = Enumerable.Range(0, 1000000).ToArray();
            //long total = 0;

            //// Use type parameter to make subtotal a long, not an int
            //Parallel.For<long>(0, nums.Length, () => 0, (j, loop, subtotal) =>
            //{
            //    subtotal += nums[j];
            //    return subtotal;
            //},
            //    (x) => Interlocked.Add(ref total, x)
            //);
            #endregion

        }
        //public static void Run()
        //{
        //    NetPositionCalculateAsync oNetPositionCalculateAsync = new NetPositionCalculateAsync();
        //   // Task CWTask = oNetPositionCalculateAsync.ProcessClientWiseTradeAsync();
        //    Task SWTask = oNetPositionCalculateAsync.ProcessScripWiseTradeAsync();
        //}


        #region Delegates

        #endregion
        /// <summary>
        ///1. initialize Sender  memory
        ///2. initialize Receiver  memory
        ///3. initialize UMS  memory
        ///4. initialize Broadcast  memory
        ///5. initialize RequestReplyMapping  memory
        /// </summary>5
        /// 
        #region Commented
        //public static void InitializeDefaultMemory()
        //{
        //    try
        //    {


        //        CreateRequestReplyMappingMemory();
        //        CreateSenderMemory();
        //        CreateReceiverMemory();
        //        CreateUMSMemory();
        //        CreateOrderMemory();
        //        PopulateExecRestatementReason();
        //        CreateTradeMemory();
        //        CreateUnprocessedTradeMemory();
        //        //CMWMessageBag = new ConcurrentQueue<CMWModel>();

        //        MemoryManager.NetPositionCWDict = new ConcurrentDictionary<string, object>();
        //        MemoryManager.NetPositionAllScripDict = new ConcurrentDictionary<string, object>();

        //        MemoryManager.NetPositionCWDemoDict = new System.Collections.Concurrent.ConcurrentDictionary<string, object>();
        //        MemoryManager.NetPositionSWDemoDict = new System.Collections.Concurrent.ConcurrentDictionary<string, object>();
        //        MemoryManager.NetPositionCWDetailsDemoDict = new System.Collections.Concurrent.ConcurrentDictionary<string, object>();
        //        MemoryManager.NetPositionSWDetailsDemoDict = new System.Collections.Concurrent.ConcurrentDictionary<string, object>();

        //        //TODO Scripwise only memory
        //        MemoryManager.NetPositionSWDict = new ConcurrentDictionary<string, object>();

        //        NetPositionMemory.Initialize();
        //        BestFiveVM.Initialize();
        //        // CommonMessageWindowVM.Initialize();
        //        MarketWatchVM.Initialize();

        //        OnlineTradeQueue = new ConcurrentQueue<object>();
        //        //CommonFrontEnd.Controller.NetPositionCalculateAsync oNetPositionCalculateAsync = new NetPositionCalculateAsync();
        //        //oNetPositionCalculateAsync.ProcessSaudasTradeAsync();

        //        //TODO AD2TR
        //        // AD2TRDataUpdation += AdminTradeViewVM.ExecuteAD2TR;

        //        //  SendTradeFeedUpdation += TradeFeedVM.AsynchronousTradeFeed.SendTradeToTradeFeedFinal;

        //        OnlineTradesKeySet = new HashSet<long>();

        //        _assemblyConDict = new ConcurrentDictionary<string, Type>();
        //        _assemblyTypeMetadata = new ConcurrentDictionary<short, Tuple<PropertyInfo[], PropertyInfo[], PropertyInfo[], PropertyInfo[]>>();
        //        var arrayTypes = Assembly.Load(Assembly.GetExecutingAssembly().GetName().Name).GetTypes();
        //        var length = arrayTypes.Count();

        //        for (int index = 0; index < length; index++)
        //        {
        //            _assemblyConDict.TryAdd(arrayTypes[index].Name, arrayTypes[index]);
        //        }

        //        for (int index = 0; index < length; index++)
        //        {
        //            var _modelName = arrayTypes[index].Name;
        //            Type type = _assemblyConDict[_modelName];

        //            var _model = arrayTypes[index];
        //            PropertyInfo[] _propertyInfoRequest = null;
        //            PropertyInfo[] _propertyInfoReply = null;//= type.GetProperties();
        //            PropertyInfo[] _propertyInfoUMS = null;// = type.GetProperties();
        //            PropertyInfo[] _propertyInfoRepeat = null;// = type.GetProperties();
        //            //var _fieldInfo = type.GetFields();
        //            // var _methodInfo = type.GetMethods();

        //            int _msgType = -1;

        //            if (ConfigurationMasterMemory.RequestDict.Any(x => x.Value.Name.ToLower() == _modelName.ToLower()))
        //            {
        //                _msgType = ConfigurationMasterMemory.RequestDict.Where(x => x.Value.Name.ToLower() == _modelName.ToLower()).FirstOrDefault().Key;
        //                _propertyInfoRequest = type.GetProperties();
        //            }
        //            if (ConfigurationMasterMemory.ReplyDict.Any(x => x.Value.Name.ToLower() == _modelName.ToLower()))
        //            {
        //                _msgType = ConfigurationMasterMemory.ReplyDict.Where(x => x.Value.Name.ToLower() == _modelName.ToLower()).FirstOrDefault().Key;
        //                _propertyInfoReply = type.GetProperties();
        //            }
        //            if (ConfigurationMasterMemory.UmsDict.Any(x => x.Value.Name.ToLower() == _modelName.ToLower()))
        //            {
        //                _msgType = ConfigurationMasterMemory.UmsDict.Where(x => x.Value.Name.ToLower() == _modelName.ToLower()).FirstOrDefault().Key;
        //                _propertyInfoUMS = type.GetProperties();
        //            }
        //            if (ConfigurationMasterMemory.RepeatDict.Any(x => x.Value.Name.ToLower() == _modelName.ToLower()))
        //            {
        //                _msgType = ConfigurationMasterMemory.RepeatDict.Where(x => x.Value.Name.ToLower() == _modelName.ToLower()).FirstOrDefault().Key;
        //                _propertyInfoRepeat = type.GetProperties();
        //            }

        //            //else if (_msgType == 0 && ConfigurationMasterMemory.ReplyDict.Any(x => x.Value.Name.ToLower() == _modelName.ToLower()))
        //            //{
        //            //    _msgType = ConfigurationMasterMemory.ReplyDict.Where(x => x.Value.Name.ToLower() == _modelName.ToLower()).FirstOrDefault().Key;
        //            //    _propertyInfoReply = type.GetProperties();
        //            //}
        //            //else if (_msgType == 0 && ConfigurationMasterMemory.UmsDict.Any(x => x.Value.Name.ToLower() == _modelName.ToLower()))
        //            //{
        //            //    _msgType = ConfigurationMasterMemory.UmsDict.Where(x => x.Value.Name.ToLower() == _modelName.ToLower()).FirstOrDefault().Key;
        //            //    _propertyInfoUMS = type.GetProperties();
        //            //}
        //            //else if (_msgType == 0 && ConfigurationMasterMemory.RepeatDict.Any(x => x.Value.Name.ToLower() == _modelName.ToLower()))
        //            //{
        //            //    _msgType = ConfigurationMasterMemory.RepeatDict.Where(x => x.Value.Name.ToLower() == _modelName.ToLower()).FirstOrDefault().Key;
        //            //    _propertyInfoRepeat = type.GetProperties();
        //            //}



        //            if (_msgType != -1)
        //            {
        //                if (_assemblyTypeMetadata.ContainsKey(Convert.ToInt16(_msgType)))
        //                {
        //                    if (_propertyInfoRequest == null)
        //                    {
        //                        _propertyInfoRequest = _assemblyTypeMetadata[Convert.ToInt16(_msgType)].Item1;
        //                    }

        //                    if (_propertyInfoReply == null)
        //                    {
        //                        _propertyInfoReply = _assemblyTypeMetadata[Convert.ToInt16(_msgType)].Item2;
        //                    }

        //                    if (_propertyInfoUMS == null)
        //                    {
        //                        _propertyInfoUMS = _assemblyTypeMetadata[Convert.ToInt16(_msgType)].Item3;
        //                    }

        //                    if (_propertyInfoRepeat == null)
        //                    {
        //                        _propertyInfoRepeat = _assemblyTypeMetadata[Convert.ToInt16(_msgType)].Item4;
        //                    }
        //                }

        //                var _data = Tuple.Create<PropertyInfo[], PropertyInfo[], PropertyInfo[], PropertyInfo[]>(_propertyInfoRequest, _propertyInfoReply, _propertyInfoUMS, _propertyInfoRepeat);
        //                //_assemblyTypeMetadata.TryAdd(Convert.ToInt16(_msgType), _data);

        //                _assemblyTypeMetadata.AddOrUpdate(Convert.ToInt16(_msgType), _data, (key, oldValue) => _data);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionUtility.LogError(ex);

        //    }
        //}
        #endregion

        public static void InitializeDefaultMemory()
        {
            try
            {


                CreateRequestReplyMappingMemory();
                CreateSenderMemory();
                CreateReceiverMemory();
                CreateUMSMemory();
                CreateOrderMemory();
                PopulateExecRestatementReason();
                CreateTradeMemory();
                CreateUnprocessedTradeMemory();

                CMWMessageBag = new ConcurrentQueue<CommonMessagingWindowModel>();

                MemoryManager.NetPositionCWDict = new ConcurrentDictionary<string, object>();
                MemoryManager.NetPositionAllScripDict = new ConcurrentDictionary<string, object>();

                MemoryManager.NetPositionCWDemoDict = new System.Collections.Concurrent.ConcurrentDictionary<string, object>();
                MemoryManager.NetPositionSWDemoDict = new System.Collections.Concurrent.ConcurrentDictionary<string, object>();
                MemoryManager.NetPositionCWDetailsDemoDict = new System.Collections.Concurrent.ConcurrentDictionary<string, object>();
                MemoryManager.NetPositionSWDetailsDemoDict = new System.Collections.Concurrent.ConcurrentDictionary<string, object>();


                //TODO Scripwise only memory
                MemoryManager.NetPositionSWDict = new ConcurrentDictionary<string, object>();

                NetPositionMemory.Initialize();
                BestFiveVM.Initialize();
                // CommonMessageWindowVM.Initialize();
                MarketWatchVM.Initialize();

                OnlineTradeQueue = new Queue<object>();
                OnlineTradeParkMemory = new ConcurrentDictionary<string, object>();
                //CommonFrontEnd.Controller.NetPositionCalculateAsync oNetPositionCalculateAsync = new NetPositionCalculateAsync();
                //oNetPositionCalculateAsync.ProcessSaudasTradeAsync();

                //TODO AD2TR
                AD2TRDataUpdation += AdminTradeViewVM.ExecuteAD2TR;

                //  SendTradeFeedUpdation += TradeFeedVM.AsynchronousTradeFeed.SendTradeToTradeFeedFinal;

                OnlineTradesKeySet = new HashSet<long>();

                _assemblyConDict = new ConcurrentDictionary<string, Type>();
                _assemblyTypeMetadata = new ConcurrentDictionary<long, Tuple<PropertyInfo[], PropertyInfo[], PropertyInfo[], Dictionary<string, PropertyInfo[]>, Dictionary<string, PropertyInfo[]>, Dictionary<string, PropertyInfo[]>>>();
                var arrayTypes = Assembly.Load(Assembly.GetExecutingAssembly().GetName().Name).GetTypes();
                var length = arrayTypes.Count();

                for (int index = 0; index < length; index++)
                {
                    _assemblyConDict.TryAdd(arrayTypes[index].Name, arrayTypes[index]);
                }

                for (int index = 0; index < length; index++)
                {
                    var _modelName = arrayTypes[index].Name;
                    Type type = _assemblyConDict[_modelName];

                    var _model = arrayTypes[index];
                    PropertyInfo[] _propertyInfoRequest = null;
                    PropertyInfo[] _propertyInfoReply = null;//= type.GetProperties();
                    PropertyInfo[] _propertyInfoUMS = null;// = type.GetProperties();
                    //PropertyInfo[] _propertyInfoRepeat = null;// = type.GetProperties();
                    Dictionary<string, PropertyInfo[]> _propertyInfoRequestRepeat = null;
                    Dictionary<string, PropertyInfo[]> _propertyInfoReplyRepeat = null;
                    Dictionary<string, PropertyInfo[]> _propertyInfoUMSRepeat = null;
                    //var _fieldInfo = type.GetFields();
                    // var _methodInfo = type.GetMethods();

                    long _msgType = -1;

                    if (ConfigurationMasterMemory.RequestDict.Any(x => x.Value.Name.ToLower() == _modelName.ToLower()))
                    {
                        _msgType = ConfigurationMasterMemory.RequestDict.Where(x => x.Value.Name.ToLower() == _modelName.ToLower()).FirstOrDefault().Key;
                        _propertyInfoRequest = type.GetProperties();
                    }
                    if (ConfigurationMasterMemory.ReplyDict.Any(x => x.Value.Name.ToLower() == _modelName.ToLower()))
                    {
                        _msgType = ConfigurationMasterMemory.ReplyDict.Where(x => x.Value.Name.ToLower() == _modelName.ToLower()).FirstOrDefault().Key;
                        _propertyInfoReply = type.GetProperties();
                    }
                    if (ConfigurationMasterMemory.UmsDict.Any(x => x.Value.Name.ToLower() == _modelName.ToLower()))
                    {
                        _msgType = ConfigurationMasterMemory.UmsDict.Where(x => x.Value.Name.ToLower() == _modelName.ToLower()).FirstOrDefault().Key;
                        _propertyInfoUMS = type.GetProperties();
                    }
                    if (ConfigurationMasterMemory.RepeatDict.Any(x => x.Value.Name.ToLower() == _modelName.ToLower()))
                    {
                        int RequestRepeatCount = 0;
                        int ReplyRepeatCount = 0;
                        int UMSRepeatCount = 0;
                        _msgType = ConfigurationMasterMemory.RepeatDict.Where(x => x.Value.Name.ToLower() == _modelName.ToLower()).FirstOrDefault().Key;
                        //var RequestRepeatCount = AdvancedTWS.SharedMemories.ConfigurationMasterMemory.RepeatDict[_msgType].Items.Where(x => x.Source == "REQUESTQ").Count();
                        //var ReplyRepeatCount = AdvancedTWS.SharedMemories.ConfigurationMasterMemory.RepeatDict[_msgType].Items.Where(x => x.Source == "REPLYQ").Count();
                        //var UMSRepeatCount = AdvancedTWS.SharedMemories.ConfigurationMasterMemory.RepeatDict[_msgType].Items.Where(x => x.Source == "UMSQ").Count();
                        if (ConfigurationMasterMemory.RequestDict.ContainsKey(_msgType))
                        {
                            RequestRepeatCount = ConfigurationMasterMemory.RequestDict[_msgType].Items.Where(x => x.Source == "REQUESTQ").Count();
                        }
                        if (ConfigurationMasterMemory.ReplyDict.ContainsKey(_msgType))
                        {
                            ReplyRepeatCount = ConfigurationMasterMemory.ReplyDict[_msgType].Items.Where(x => x.Source == "REPLYQ").Count();
                        }
                        if (ConfigurationMasterMemory.UmsDict.ContainsKey(_msgType))
                        {
                            UMSRepeatCount = ConfigurationMasterMemory.UmsDict[_msgType].Items.Where(x => x.Source == "UMSQ").Count();
                        }
                        if (RequestRepeatCount > 0)
                        {
                            _propertyInfoRequestRepeat = new Dictionary<string, PropertyInfo[]>();
                            var dataRequest = ConfigurationMasterMemory.RequestDict[_msgType].Items.Where(x => x.Source == "REQUESTQ").ToList();
                            var dataRequestCount = dataRequest.Count;
                            for (int indexReq = 0; indexReq < dataRequestCount; indexReq++)
                            {
                                Type reqType = _assemblyConDict[dataRequest[indexReq].Type];
                                PropertyInfo[] requestPropertyInfo = reqType.GetProperties();
                                _propertyInfoRequestRepeat.Add(_modelName, requestPropertyInfo);
                            }
                        }
                        if (ReplyRepeatCount > 0)
                        {
                            _propertyInfoReplyRepeat = new Dictionary<string, PropertyInfo[]>();
                            var dataReply = ConfigurationMasterMemory.ReplyDict[_msgType].Items.Where(x => x.Source == "REPLYQ").ToList();
                            var dataReplyCount = dataReply.Count;
                            for (int indexReply = 0; indexReply < dataReplyCount; indexReply++)
                            {
                                Type replyType = _assemblyConDict[dataReply[indexReply].Type];
                                PropertyInfo[] replyPropertyInfo = replyType.GetProperties();
                                _propertyInfoReplyRepeat.Add(_modelName, replyPropertyInfo);
                            }
                        }
                        if (UMSRepeatCount > 0)
                        {
                            _propertyInfoUMSRepeat = new Dictionary<string, PropertyInfo[]>();
                            var dataUMS = ConfigurationMasterMemory.UmsDict[_msgType].Items.Where(x => x.Source == "UMSQ").ToList();
                            var dataUMSCount = dataUMS.Count;
                            for (int indexUMS = 0; indexUMS < dataUMSCount; indexUMS++)
                            {
                                Type umsType = _assemblyConDict[dataUMS[indexUMS].Type];
                                PropertyInfo[] umsPropertyInfo = umsType.GetProperties();
                                _propertyInfoUMSRepeat.Add(_modelName, umsPropertyInfo);
                            }
                        }
                        //        _propertyInfoRepeat = type.GetProperties();
                        //REPLYQ
                        //REQUESTQ
                        //UMSQ

                    }



                    if (_msgType != -1)
                    {
                        if (_assemblyTypeMetadata.ContainsKey(Convert.ToInt64(_msgType)))
                        {
                            if (_propertyInfoRequest == null)
                            {
                                _propertyInfoRequest = _assemblyTypeMetadata[Convert.ToInt64(_msgType)].Item1;
                            }

                            if (_propertyInfoReply == null)
                            {
                                _propertyInfoReply = _assemblyTypeMetadata[Convert.ToInt64(_msgType)].Item2;
                            }

                            if (_propertyInfoUMS == null)
                            {
                                _propertyInfoUMS = _assemblyTypeMetadata[Convert.ToInt64(_msgType)].Item3;
                            }

                            //if (_propertyInfoRepeat == null)
                            //{
                            //    _propertyInfoRepeat = _assemblyTypeMetadata[Convert.ToInt16(_msgType)].Item4;
                            //}
                            if (_propertyInfoRequestRepeat == null)
                            {
                                _propertyInfoRequestRepeat = _assemblyTypeMetadata[Convert.ToInt64(_msgType)].Item4;
                            }
                            if (_propertyInfoReplyRepeat == null)
                            {
                                _propertyInfoReplyRepeat = _assemblyTypeMetadata[Convert.ToInt64(_msgType)].Item5;
                            }
                            if (_propertyInfoUMSRepeat == null)
                            {
                                _propertyInfoUMSRepeat = _assemblyTypeMetadata[Convert.ToInt64(_msgType)].Item6;
                            }
                        }

                        //var _data = Tuple.Create<PropertyInfo[], PropertyInfo[], PropertyInfo[], PropertyInfo[]>(_propertyInfoRequest, _propertyInfoReply, _propertyInfoUMS, _propertyInfoRepeat);
                        var _data = Tuple.Create<PropertyInfo[], PropertyInfo[], PropertyInfo[], Dictionary<string, PropertyInfo[]>, Dictionary<string, PropertyInfo[]>, Dictionary<string, PropertyInfo[]>>(_propertyInfoRequest, _propertyInfoReply, _propertyInfoUMS, _propertyInfoRequestRepeat, _propertyInfoReplyRepeat, _propertyInfoUMSRepeat);
                        //_assemblyTypeMetadata.TryAdd(Convert.ToInt16(_msgType), _data);

                        _assemblyTypeMetadata.AddOrUpdate(Convert.ToInt64(_msgType), _data, (key, oldValue) => _data);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);

            }
        }
        private static void PopulateExecRestatementReason()
        {
            ExecRestatementReasonMemory.TryAdd(101, "Order Added");
            ExecRestatementReasonMemory.TryAdd(102, "Order Replaced");
            ExecRestatementReasonMemory.TryAdd(103, "Order Cancelled");
            ExecRestatementReasonMemory.TryAdd(105, "IOC Order Cancelled");
            ExecRestatementReasonMemory.TryAdd(212, "BOC Order Cancelled");
            ExecRestatementReasonMemory.TryAdd(135, "Market Order Triggered");
            ExecRestatementReasonMemory.TryAdd(215, "RRM Order Added");
            ExecRestatementReasonMemory.TryAdd(221, "Provisional Order Added");
            ExecRestatementReasonMemory.TryAdd(246, "Self Trade Order Deleted");
            ExecRestatementReasonMemory.TryAdd(247, "Reverse Trade Order Deleted");
            ExecRestatementReasonMemory.TryAdd(102, "Order Replaced");
            ExecRestatementReasonMemory.TryAdd(197, "Order Cancellation Pending");
        }

        /// <summary>
        /// Memory Mapped Table
        /// </summary>
        public static void CreateRequestReplyMappingMemory()
        {
            RequestReplyMappingDict = new ConcurrentDictionary<int, bool>(1, 5);
            for (int i = 1; i <= RequestReplyQueueSize; i++)
            {
                RequestReplyMappingDict.TryAdd(i, true);
            }
            MessageTagResponseMappingDict = new ConcurrentDictionary<long, long>();
        }
        public static void UpdateRequestReplyMappingMemory(int key, bool newVal, bool oldVal)
        {
            RequestReplyMappingDict.TryUpdate(key, newVal, oldVal);
        }
        /// <summary>
        /// Returns Empty slot number from Request reply Mapping table
        /// </summary>
        /// <returns>Slot Number</returns>
        public static int FindingFreeMemory()
        {
            //1000 success
            //1001 No free slots are available error
            //1002 Memory Mapped table not intialised
            int emptySlot = -1;
            if (RequestReplyMappingDict != null && RequestReplyMappingDict.Count > 0)
            {
                if (RequestReplyMappingDict.Any(x => x.Value == true))
                {
                    emptySlot = RequestReplyMappingDict.Where(x => x.Value == true).OrderBy(y => y.Key).Select(x => x.Key).FirstOrDefault();
                }
                else
                {
                    emptySlot = Convert.ToInt32(ConstantErrorMessages.ErrorCd1001);
                }
            }
            else
            {
                emptySlot = Convert.ToInt32(ConstantErrorMessages.ErrorCd1002);
            }
            return emptySlot;
        }

        /// <summary>
        /// Shared Memory(IPC): Request will be queued up at sender memory.
        /// </summary>
        public static void CreateSenderMemory()
        {

        }

        /// <summary>
        ///Shared Memory(IPC): Response Login Success, order id 
        /// </summary>
        public static void CreateReceiverMemory()
        {

        }

        /// <summary>
        ///ConcurrentDictionary: Trades(1521) and Other UMS Messages
        ///Bifurcate Trade and Other UMS message
        /// </summary>
        public static void CreateUMSMemory()
        {
            UMSMemoryConDict = new ConcurrentQueue<byte[]>();
        }

        /// <summary>
        /// ConcurrentDictionary: order Memory with Key:UniqueMessageTag, Value:object
        /// </summary>
        //public static void CreateOrderMemory()
        //{
        //    OrderDictionary = new ConcurrentDictionary<long, OrderModel>();
        //    OrderDictionaryBackupMemory = new ConcurrentDictionary<long, OrderModel>();
        //    //OrderMemoryConDict = new ConcurrentDictionary<int, object>();
        //}

        /// <summary>
        ///Create Trade Memory. Note:- Check duplicate entry validation
        ///only Trade messages(1521)
        /// </summary>
        public static void CreateTradeMemory()
        {
            TradeMemoryConDict = new ConcurrentDictionary<long, object>();

            //oCWConcurrentQueue = new ConcurrentQueue<object>();
            //oSWConcurrentQueue = new ConcurrentQueue<object>();
            //oSWCWConcurrentQueue = new ConcurrentQueue<object>();
            //oCWSWConcurrentQueue = new ConcurrentQueue<object>();
        }

        /// <summary>
        /// unprocessed trade Memory will be used in case received Trade first instead of Response(e.g. Order id) from exchange
        /// </summary>
        public static void CreateUnprocessedTradeMemory()
        {
            UnProcessedTradeMemoryConDict = new ConcurrentBag<object>();
        }

        internal static void GetSettlementNoFromMaster()
        {
            try
            {
                CultureInfo arDe = new CultureInfo("de-DE");
                if (MasterSharedMemory.listSetlMas != null && MasterSharedMemory.listSetlMas.Count > 0)
                {
                    var filteredData = MasterSharedMemory.listSetlMas.Where(x => x.Field1 == "DR").ToList();
                    if (filteredData != null && filteredData.Count > 0)
                    {
                        var data = filteredData.Where(y => (Convert.ToDateTime(y.Field3, arDe).ToString("dd/MM/yyyy") == CommonFunctions.GetDate().ToString("dd/MM/yyyy"))).ToList();
                        if (data != null && data.Count > 0)
                        {
                            var strSettlementNo = data.Select(z => z.Field2).FirstOrDefault();
                            if (!string.IsNullOrEmpty(strSettlementNo) && strSettlementNo.Contains('/'))
                            {
                                UtilityLoginDetails.GETInstance.SettlementNo = strSettlementNo;

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
                throw ex;
            }
        }
    }

#endif
#if BOW
    public static partial class MemoryManager
    {
        public static void InitializeDefaultMemory()
        {
            MarketWatchVM.Initialize();
            CreateOrderMemory();
            //CreateTradeMemory();
        }
    }
#endif


}
