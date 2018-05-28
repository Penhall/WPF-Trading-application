using CommonFrontEnd.Common;
using CommonFrontEnd.Model.Trade;
using CommonFrontEnd.SharedMemories;
using CommonFrontEnd.View.Trade;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows;

namespace CommonFrontEnd.ViewModel.Trade
{
#if TWS
    class TradeFeedVM : BaseViewModel
    {

        const int reservedBuffer = 30720;
        static byte[] remData = new byte[reservedBuffer];
        //public static DirectoryInfo TwsINIPath = new DirectoryInfo(Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"Profile/Tws.ini")));
        //IniParser parser = new IniParser(TwsINIPath.ToString());

        //public int noOfTradeFeedSent=0;


        #region RelayCommands

        private RelayCommand _btnCloseClick;

        public RelayCommand btnCloseClick
        {
            get
            {
                return _btnCloseClick ?? (_btnCloseClick = new RelayCommand(
                    (object e) => TradeFeedWindow_Close(e)));

            }

        }

        private void TradeFeedWindow_Close(object e)
        {
            TradeFeed oTradeFeed = System.Windows.Application.Current.Windows.OfType<TradeFeed>().FirstOrDefault();
            if (oTradeFeed != null)
            {
                oTradeFeed.Hide();
            }
        }

        private RelayCommand _SocketConnection;
        public RelayCommand SocketConnection
        {
            get
            {
                return _SocketConnection ?? (_SocketConnection = new RelayCommand((object e) => OpenSocketConnection()));
            }
        }

        private RelayCommand _AbortSocketConnection;
        public RelayCommand AbortSocketConnection
        {
            get
            {
                return _AbortSocketConnection ?? (_AbortSocketConnection = new RelayCommand((object e) => CloseSocketConnection()));
            }
        }

        private RelayCommand _Window_Loaded;
        public RelayCommand Window_Loaded
        {
            get
            {
                return _Window_Loaded ?? (_Window_Loaded = new RelayCommand((object e) => Window_Loaded_Click()));
            }
        }

        #endregion


        #region Properties
        private string _ReplyText;
        public string ReplyText
        {
            get { return _ReplyText; }
            set
            {
                _ReplyText = value;
                NotifyPropertyChanged("ReplyText");
            }
        }


        private static string _txtIpAddress;
        public static string txtIpAddress
        {
            get { return _txtIpAddress; }
            set
            {
                _txtIpAddress = value;
                NotifyStaticPropertyChanged("txtIpAddress");
            }
        }

        private int _txtIpPort;
        public int txtIpPort
        {
            get { return _txtIpPort; }
            set
            {
                _txtIpPort = value;
                NotifyPropertyChanged(nameof(txtIpPort));
            }
        }

        private static TradeFeedVM _getinstance;

        public static TradeFeedVM GetInstance
        {
            get
            {
                if (_getinstance == null)
                {
                    _getinstance = new TradeFeedVM();
                }
                return _getinstance;
            }
        }

        private static int _txtHour = 0;
        public static int txtHour
        {
            get { return _txtHour; }
            set
            {
                _txtHour = value;
                NotifyStaticPropertyChanged("txtHour");
            }
        }

        private static int _txtMinute = 0;
        public static int txtMinute
        {
            get { return _txtMinute; }
            set
            {
                _txtMinute = value;
                NotifyStaticPropertyChanged("txtMinute");
            }
        }

        private static int _txtSeconds = 0;
        public static int txtSeconds
        {
            get { return _txtSeconds; }
            set
            {
                _txtSeconds = value;
                NotifyStaticPropertyChanged("txtSeconds");
            }
        }

        public static TradeFeed oTradeFeed = System.Windows.Application.Current.Windows.OfType<TradeFeed>().FirstOrDefault();
        #endregion


        private static void OpenSocketConnection()
        {
            //Controller.NetPositionCalculate.ProcessOnlineTrades();
            AsynchronousTradeFeed.checkIP();
            AsynchronousTradeFeed.StartTradeFeed();
            if (AsynchronousTradeFeed.setIP)
                SaveTradeFeedIPPORT();

            /*    if (AsynchronousTradeFeed.sockTradeFeed.Connected == true)
                {
                    AsynchronousTradeFeed.SendTradeToTradeFeed();
                }*/
        }


        public static void SaveTradeFeedIPPORT()
        {
            // txtIpAddress = Application.Current.Windows.OfType<TradeFeed>().FirstOrDefault().txtIpAddress.Text;
            MainWindowVM.parser.AddSetting("Login Settings", "txtIpAddress", oTradeFeed.txtIpAddress.Text.ToString());

            MainWindowVM.parser.AddSetting("Login Settings", "txtIpPort", GetInstance.txtIpPort.ToString());
            MainWindowVM.parser.SaveSettings(MainWindowVM.TwsINIPath.ToString());


        }

        public void CloseSocketConnection()
        {
            MemoryManager.onlineSendFeed = false;

            if (AsynchronousTradeFeed.sockTradeFeed != null && AsynchronousTradeFeed.sockTradeFeed.Connected == true)
            {
                AsynchronousTradeFeed.sockTradeFeed.Shutdown(SocketShutdown.Both);
                AsynchronousTradeFeed.sockTradeFeed.Close();
                ReplyText = "OnLine Trade Feed has Stopped";
            }
            else
            {
                ReplyText = "Socket Connection is Aborted";
            }
        }

        #region Constructor
        public TradeFeedVM()
        {
            //GetIPPORT();
            //    TwsMainWindowVM.parser.AddSetting("Login Settings", "txtIpAddress", txtIpAddress.ToString());
            //    TwsMainWindowVM.parser.AddSetting("Login Settings", "txtIpPort", txtIpPort.ToString());
            //TwsMainWindowVM.parser.SaveSettings(TwsMainWindowVM.TwsINIPath.ToString());
        }
        #endregion

        public static void Window_Loaded_Click()
        {
            GetIPPORT();
        }

        private static void GetIPPORT()
        {

            txtIpAddress = MainWindowVM.parser.GetSetting("Login Settings", "txtIpAddress");
            if (!string.IsNullOrEmpty(txtIpAddress))
            {
                if (oTradeFeed != null)
                {
                    //Application.Current.Windows.OfType<TradeFeed>().FirstOrDefault().txtIpAddress.Text = txtIpAddress;
                    oTradeFeed.txtIpAddress.Text = txtIpAddress;
                }
            }
            else
            {
                oTradeFeed.txtIpAddress.Text = string.Empty;
            }

            string tempIpPort = MainWindowVM.parser.GetSetting("Login Settings", "txtIpPort");
            if (!string.IsNullOrEmpty(tempIpPort))
            {
                GetInstance.txtIpPort = Convert.ToInt32(tempIpPort);
            }
            else
            {
                GetInstance.txtIpPort = 0;
            }

        }



        public class StateObjectTradeFeed
        {
            // Client socket.
            public Socket workSocket = null;
            // Size of receive buffer.
            public const int BufferSize = reservedBuffer;
            // Receive buffer.
            public byte[] buffer = new byte[BufferSize];
        }

        public static class AsynchronousTradeFeed
        {

            public static Socket sockTradeFeed;
            public static bool setIP = false;

            //The IP Address for the remote device.
            // private static string TradeFeedhost = "10.228.37.18";

            //Accepting IP Address from user
            private static string TradeFeedhost = String.Empty;
            private static int TradeFeedport = 0;

            //private static string TradeFeedhost = txtIpAddress;
            // The port number for the remote device.
            // private const int TradeFeedport = 5001;

            //Accepting Port Number from user

            //private static int TradeFeedport = txtIpPort;

            // ManualResetEvent instances signal completion.
            private static ManualResetEvent connectDone = new ManualResetEvent(false);
            public static ManualResetEvent sendDone = new ManualResetEvent(false);
            public static ManualResetEvent receiveDone = new ManualResetEvent(false);

            public static IEnumerable<TradeUMS> TradeViewDataCollection { get; private set; }


            public static void checkIP()
            {
                try
                {
                    if (oTradeFeed.txtIpAddress.Text != null)
                    {
                        TradeFeedhost = oTradeFeed.txtIpAddress.Text;
                    }
                    else
                    {
                        GetInstance.ReplyText = "Invalid IP Address";
                        return;
                    }
                    if (Convert.ToInt32(GetInstance.txtIpPort) != 0)
                    {
                        TradeFeedport = Convert.ToInt32(GetInstance.txtIpPort);
                    }
                    else
                    {
                        GetInstance.ReplyText = "Invalid IP Port";
                        return;
                    }
                }
                catch (Exception)
                {
                    throw;
                }

            }

            public static void StartTradeFeed()
            {
                // Connect to a remote device.
                try
                {
                    // Establish the remote endpoint for the socket.
                    IPEndPoint remoteEP = new IPEndPoint(IPAddress.Parse(TradeFeedhost), TradeFeedport);

                    // Create a TCP/IP socket.
                    sockTradeFeed = new Socket(AddressFamily.InterNetwork,
                        SocketType.Stream, ProtocolType.Tcp);

                    // Connect to the remote endpoint.
                    sockTradeFeed.BeginConnect(remoteEP,
                        new AsyncCallback(ConnectCallback), sockTradeFeed);
                    connectDone.WaitOne();

                    Receive(sockTradeFeed);
                    //   receiveDone.WaitOne();

                    //timer.Interval = new TimeSpan(0, 0, 0, 0, 1);
                    //timer.Tick += new EventHandler(timer_Tick);
                    //timer.Start();

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
                    //Console.WriteLine(ex.ToString());
                }
            }

            public static void ConnectCallback(IAsyncResult ar)
            {
                try
                {
                    // Retrieve the socket from the state object.
                    Socket sockTradeFeed = (Socket)ar.AsyncState;

                    // Complete the connection.
                    sockTradeFeed.EndConnect(ar);
                    GetInstance.ReplyText = "Connected Successfully";
                    setIP = true;
                    //SaveTradeFeedIPPORT();
                    //  TwsMainWindowVM.parser.SaveSettings(TwsMainWindowVM.TwsINIPath.ToString());
                    MemoryManager.onlineSendFeed = true;
                    // Signal that the connection has been made.
                    connectDone.Set();
                    AsynchronousTradeFeed.SendTradeToTradeFeed();
                }
                catch (Exception e)
                {
                    GetInstance.ReplyText = "Connection Failed";
                    MemoryManager.onlineSendFeed = false;
                    connectDone.Set();
                    Console.WriteLine(e.ToString());
                }
                finally
                {
                    //SaveTradeFeedIPPORT();
                }
            }

            //private static void AssignIPPORT()
            //{
            //    txtIpAddress = TwsMainWindowVM.parser.GetSetting("Login Settings", "txtIpAddress");
            //    txtIpPort = Convert.ToInt32(TwsMainWindowVM.parser.GetSetting("Login Settings", "txtIpPort"));
            //}

            public static void Send(Socket sockTradeFeed, byte[] byteData)
            {
                try
                {
                    if (sockTradeFeed.Connected)
                    {
                        // Begin sending the data to the remote device.
                        sockTradeFeed.BeginSend(byteData, 0, byteData.Length, 0,
                            new AsyncCallback(SendCallback), sockTradeFeed);
                    }
                    else
                    {
                        //"Connection Dropped";
                        MemoryManager.onlineSendFeed = false;
                        MessageBox.Show("Connection Dropped");
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            private static void SendCallback(IAsyncResult ar)
            {
                try
                {
                    // Retrieve the socket from the state object.
                    Socket sockTradeFeed = (Socket)ar.AsyncState;

                    // Complete sending the data to the remote device.
                    int bytesSent = sockTradeFeed.EndSend(ar);

                    // Signal that all bytes have been sent.
                    sendDone.Set();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }


            public static void Receive(Socket sockTradeFeed)
            {
                try
                {
                    // Create the state object.
                    StateObjectTradeFeed state = new StateObjectTradeFeed();
                    state.workSocket = sockTradeFeed;

                    // Begin receiving the data from the remote device.
                    sockTradeFeed.BeginReceive(state.buffer, 0, StateObjectTradeFeed.BufferSize, 0,
                        new AsyncCallback(ReceiveCallback), state);

                }
                catch (Exception e)
                {

                }
            }

            private static void ReceiveCallback(IAsyncResult ar)
            {
                try
                {
                    // Retrieve the state object and the client socket 
                    // from the asynchronous state object.
                    StateObjectTradeFeed state = (StateObjectTradeFeed)ar.AsyncState;
                    Socket sockTradeFeed = state.workSocket;

                    // Read data from the remote device.
                    int bytesRead = sockTradeFeed.EndReceive(ar);

                    if (bytesRead <= 0)
                    {
                        GetInstance.ReplyText = "Connection Dropped";
                        if (sockTradeFeed.Connected)
                        {
                            MemoryManager.onlineSendFeed = false;
                            sockTradeFeed.Shutdown(SocketShutdown.Both);
                            sockTradeFeed.Close();

                        }

                        // Signal that all bytes have been received.
                        // receiveDone.Set();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
                finally
                {
                }
            }


            public static void SendTradeToTradeFeed()
            {
                try
                {
                    #region Fetch Index
                    //maintain send index in MemoryManager 27/03/2017 
                    if (MemoryManager.onlineSendFeed == true)
                    {
                        MemoryManager.NoOfTradeFeedSent = 0;
                        for (int index = 0; index < MemoryManager.TradeMemoryConDict.Count; index++) //(KeyValuePair<long, object> dr in MemoryManager.TradeMemoryConDict)
                        {
                            TradeUMS oTradeUMS = new TradeUMS();
                            oTradeUMS = (TradeUMS)MemoryManager.TradeMemoryConDict[index];
                            if (MemoryManager.onlineSendFeed == true)
                            {
                                SendTradeToTradeFeedFinal(oTradeUMS);
                                MemoryManager.SendTradeFeedIndex = MemoryManager.SendTradeFeedIndex + 1;
                            }
                            else
                                break;
                        }
                        #endregion
                    }
                }
                catch (SocketException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            public static void SendTradeToTradeFeedFinal(TradeUMS oTradeUMS)
            {

                try
                {
                    List<byte> tradeFeedONReq = new List<byte>();
                    int msgLength = 182;
                    int NoOfTrades = 1;
                    int Filler1 = 0;
                    char[] StrData;
                    byte[] buffer;
                    string time = oTradeUMS.TimeOnly;
                    string orderTimeStamp = String.Empty;
                    var timearray = time.Split(':');
                    if (timearray != null && timearray.Count() > 0)
                    {


                        int hour = Convert.ToInt16(timearray[0]);
                        int min = Convert.ToInt16(timearray[1]);
                        int sec = Convert.ToInt16(timearray[2]);
                        char char_space = '\0';


                        if (((hour) > (txtHour) && ((min) >= (txtMinute) || (min) < (txtMinute)) && ((sec) >= (txtSeconds) ||
                            (sec) < (txtSeconds))) || ((hour) == (txtHour) && ((min) == (txtMinute)) && ((sec) >= (txtSeconds))) ||
                            ((hour) == (txtHour) && ((min) > (txtMinute)) && (((sec) >= (txtSeconds)) || ((sec) < (txtSeconds)))))
                        {
                            tradeFeedONReq.AddRange(BitConverter.GetBytes(msgLength));
                            tradeFeedONReq.AddRange(BitConverter.GetBytes(NoOfTrades));
                            tradeFeedONReq.AddRange(BitConverter.GetBytes(oTradeUMS.TraderId));
                            tradeFeedONReq.AddRange(BitConverter.GetBytes(Convert.ToInt32(oTradeUMS.ScripCode)));

                            StrData = oTradeUMS.ScripName.PadRight(11, ' ').ToCharArray();
                            foreach (char c in StrData)
                            {
                                buffer = BitConverter.GetBytes(c);
                                tradeFeedONReq.Add(buffer[0]);
                            }
                            tradeFeedONReq.Add(BitConverter.GetBytes(char_space)[0]);

                            tradeFeedONReq.AddRange(BitConverter.GetBytes(oTradeUMS.SideTradeID));
                            tradeFeedONReq.AddRange(BitConverter.GetBytes(Convert.ToInt32(oTradeUMS.LastPx / 1000000)));
                            tradeFeedONReq.AddRange(BitConverter.GetBytes(oTradeUMS.LastQty));
                            tradeFeedONReq.AddRange(BitConverter.GetBytes(Filler1));
                            tradeFeedONReq.AddRange(BitConverter.GetBytes(Filler1));

                            //tradeFeedONReq.AddRange(BitConverter.GetBytes(CommonFunctions.GetGroupName(oTradeUMS.ScripCode)));


                            // add 1 more byte
                            // Array.Resize(ref StrData, 9);
                            // StrData = new string(StrData).PadLeft(StrData.Length, '\t').ToCharArray();
                            StrData = oTradeUMS.TimeOnly.PadRight(8, ' ').ToCharArray();
                            foreach (char c in StrData)
                            {
                                buffer = BitConverter.GetBytes(c);
                                tradeFeedONReq.Add(buffer[0]);
                            }
                            tradeFeedONReq.Add(BitConverter.GetBytes(char_space)[0]);

                            StrData = oTradeUMS.DateOnly.PadRight(10, ' ').ToCharArray();
                            foreach (char c in StrData)
                            {
                                buffer = BitConverter.GetBytes(c);
                                tradeFeedONReq.Add(buffer[0]);
                            }
                            tradeFeedONReq.Add(BitConverter.GetBytes(char_space)[0]);

                            StrData = orderTimeStamp.PadRight(19, ' ').ToCharArray();
                            foreach (char c in StrData)
                            {
                                buffer = BitConverter.GetBytes(c);
                                tradeFeedONReq.Add(buffer[0]);
                            }
                            tradeFeedONReq.Add(BitConverter.GetBytes(char_space)[0]);

                            StrData = oTradeUMS.Client.PadRight(11, ' ').ToCharArray();
                            foreach (char c in StrData)
                            {
                                buffer = BitConverter.GetBytes(c);
                                tradeFeedONReq.Add(buffer[0]);
                            }
                            tradeFeedONReq.Add(BitConverter.GetBytes(char_space)[0]);

                            StrData = oTradeUMS.BSFlag.PadRight(0, ' ').ToCharArray();
                            foreach (char c in StrData)
                            {
                                buffer = BitConverter.GetBytes(c);
                                tradeFeedONReq.Add(buffer[0]);
                            }

                            StrData = oTradeUMS.OrderType.PadRight(0, ' ').ToCharArray();
                            foreach (char c in StrData)
                            {
                                buffer = BitConverter.GetBytes(c);
                                tradeFeedONReq.Add(buffer[0]);
                            }

                            tradeFeedONReq.AddRange(BitConverter.GetBytes(oTradeUMS.OrderID));

                            StrData = oTradeUMS.ClientType.PadRight(9, ' ').ToCharArray();
                            foreach (char c in StrData)
                            {
                                buffer = BitConverter.GetBytes(c);
                                tradeFeedONReq.Add(buffer[0]);
                            }
                            tradeFeedONReq.Add(BitConverter.GetBytes(char_space)[0]);

                            StrData = oTradeUMS.ISIN.PadRight(12, ' ').ToCharArray();
                            foreach (char c in StrData)
                            {
                                buffer = BitConverter.GetBytes(c);
                                tradeFeedONReq.Add(buffer[0]);
                            }
                            tradeFeedONReq.Add(BitConverter.GetBytes(char_space)[0]);

                            tradeFeedONReq.Add(BitConverter.GetBytes(char_space)[0]);
                            //  tradeFeedONReq.Add(BitConverter.GetBytes(char_space)[0]);


                            //StrData = string.Join("/", oTradeUMS.SettlNo).ToCharArray();
                            //Array.Resize(ref StrData, 12);
                            //foreach (char c in StrData)
                            //{
                            //    buffer = BitConverter.GetBytes(c);
                            //    tradeFeedONReq.Add(buffer[0]);
                            //}
                            //Filler1 = Convert.ToInt32(oTradeUMS.SettlNo[0]);

                            if (!string.IsNullOrEmpty(oTradeUMS.SettlNo[0]))
                                tradeFeedONReq.AddRange(BitConverter.GetBytes(Convert.ToInt32(oTradeUMS.SettlNo[0])));
                            else
                                tradeFeedONReq.AddRange(BitConverter.GetBytes(Filler1));
                            //tradeFeedONReq.AddRange(BitConverter.GetBytes(Convert.ToInt32(oTradeUMS.SettlNo[0])));
                            /* byte[] tmpbyte;
                             tmpbyte = BitConverter.GetBytes(Filler1);
                             tradeFeedONReq.AddRange(tmpbyte);*/

                            /*   StrData = oTradeUMS.ScripGroup.PadRight(2, ' ').ToCharArray();
                               foreach (char c in StrData)
                               {
                                   buffer = BitConverter.GetBytes(c);
                                   tradeFeedONReq.Add(buffer[0]);
                               }*/
                            tradeFeedONReq.Add(BitConverter.GetBytes(char_space)[0]);
                            tradeFeedONReq.Add(BitConverter.GetBytes(char_space)[0]);
                            tradeFeedONReq.Add(BitConverter.GetBytes(char_space)[0]);

                            StrData = oTradeUMS.ScripGroup.PadRight(2, ' ').ToCharArray();
                            foreach (char c in StrData)
                            {
                                buffer = BitConverter.GetBytes(c);
                                tradeFeedONReq.Add(buffer[0]);
                            }
                            tradeFeedONReq.Add(BitConverter.GetBytes(char_space)[0]);
                            if (!string.IsNullOrEmpty(oTradeUMS.SettlNo[1]))
                                tradeFeedONReq.AddRange(BitConverter.GetBytes(Convert.ToInt32(oTradeUMS.SettlNo[1])));
                            else
                                tradeFeedONReq.AddRange(BitConverter.GetBytes(Filler1));
                            tradeFeedONReq.AddRange(BitConverter.GetBytes(oTradeUMS.SenderLocationID));
                            if (oTradeUMS.CPCode == null)
                                oTradeUMS.CPCode = string.Empty;
                            StrData = oTradeUMS.CPCode?.PadRight(12, ' ').ToCharArray();
                            foreach (char c in StrData)
                            {
                                buffer = BitConverter.GetBytes(c);
                                tradeFeedONReq.Add(buffer[0]);
                            }
                            tradeFeedONReq.Add(BitConverter.GetBytes(char_space)[0]);
                            tradeFeedONReq.Add(BitConverter.GetBytes(char_space)[0]);

                            tradeFeedONReq.AddRange(BitConverter.GetBytes(Filler1));
                            tradeFeedONReq.AddRange(BitConverter.GetBytes(oTradeUMS.UnderlyingDirtyPrice));
                            tradeFeedONReq.AddRange(BitConverter.GetBytes(oTradeUMS.DecimalLocator));

                            AsynchronousTradeFeed.Send(sockTradeFeed, tradeFeedONReq.ToArray<byte>());
                            MemoryManager.NoOfTradeFeedSent = MemoryManager.NoOfTradeFeedSent + 1;
                            GetInstance.ReplyText = "No of Trades Sent : " + MemoryManager.NoOfTradeFeedSent;
                        }
                    }
                }
                catch (Exception ex)
                {
                    ExceptionUtility.LogError(ex);
                }
            }



        }
        //public static void Receive(Socket sockTradeFeed)
        //{
        //    try
        //    {
        //        // Create the state object.
        //        StateObjectTradeFeed state = new StateObjectTradeFeed();
        //        state.workSocket = sockTradeFeed;

        //        // Begin receiving the data from the remote device.
        //        sockTradeFeed.BeginReceive(state.buffer, 0, StateObjectTradeFeed.BufferSize, 0,
        //            new AsyncCallback(ReceiveCallback), state);


        //    }
        //    catch (Exception e)
        //    {

        //    }
        //}

        //private static void ReceiveCallback(IAsyncResult ar)
        //{
        //    try
        //    {
        //        // Retrieve the state object and the client socket 
        //        // from the asynchronous state object.
        //        StateObjectTradeFeed state = (StateObjectTradeFeed)ar.AsyncState;
        //        Socket sockTradeFeed = state.workSocket;

        //        // Read data from the remote device.
        //        int bytesRead = sockTradeFeed.EndReceive(ar);

        //        if (bytesRead <= 0)
        //        {
        //            if (!sockTradeFeed.Connected)
        //            {
        //                ReplyText = "Connection Dropped";
        //            }

        //            // Signal that all bytes have been received.
        //            receiveDone.Set();
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.ToString());
        //    }
        //    finally
        //    {
        //    }
        //}
    }
#endif
}

