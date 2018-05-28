using CommonFrontEnd.Common;
using CommonFrontEnd.Model;
using CommonFrontEnd.SharedMemories;
using CommonFrontEnd.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CommonFrontEnd.Controller
{
#if TWS
    public class UdpState
    {

        // Client  socket.
        public UdpClient WorkSocket = null;
        // Size of receive buffer.
        public const int BufferSize = 2048;
        // Receive buffer.
        public byte[] Buffer = new byte[BufferSize];
        // Received data string.
        public StringBuilder Sb = new StringBuilder();
    }
    class Constants
    {
        //public static Dictionary<int, ChartPointsOrder> DicChartControlos { get; set; }
        //public static ObservableCollection<IndexDetail> IndexDic { get; set; }
        //public static AddChangeScrip AddChangeScrip { get; set; }
        //public static EnlargedGraph BiggerGraph;
        public const int MaxCount = 4;
        //public static ChartPointsOrder BiggerGraphPoints;
        //public static AddChangeScrip _addChange;


        public const String ConfigFileName = "GraphConfig.xml";

        public static int _sessionId { get; set; }
        public static int ContinuousSession = 3;
        public static int ClosingSession = 4;
        public static int MemberQuerySession = 7;
    }
    public class BroadcastController
    {
        private UdpClient _client;
        public IPAddress BroadCastIp { get; set; }
        public IPAddress InterfaceIp { get; set; }
        public int BroadCastPort { get; set; }

        private UdpClient _clientDer;
        public IPAddress BroadCastIpDer { get; set; }
        public IPAddress InterfaceIpDer { get; set; }
        public int BroadCastPortDer { get; set; }

        private UdpClient _clientCurr;
        public IPAddress BroadCastIpCurr { get; set; }
        public IPAddress InterfaceIpCurr { get; set; }
        public int BroadCastPortCurr { get; set; }

        private string starttime = "";

        Dictionary<int, string> ScripCodeDict = new Dictionary<int, string>();

        // public static DirectoryInfo BoltINIPath = new DirectoryInfo(
        //Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"BOLT.ini")));

        // public  Action<ScripDetails> OnMessageTransmitted;
        public static Action<int> OnMessageTransmitted;

        public BroadcastController()
        {
            Messenger.Default.Register<Dictionary<int, string>>(this, GetScriptCodesFromMstr);
            Messenger.Default.Send(MasterSharedMemory.objMastertxtDictBaseBSE);//BSE
            Messenger.Default.Unregister(this);
        }

        private void GetScriptCodesFromMstr(Dictionary<int, string> scripCodeDict)
        {
            ScripCodeDict = scripCodeDict;
        }
        public void ConnectToBroadCastServer()
        {
            //IniParser parser = new IniParser(BoltINIPath.ToString());

            var host = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress objInterfaceIpAddr = host.AddressList[1];
            //IPAddress objInterfaceIpAddr = IPAddress.Parse("10.228.37.94");


            // IPAddress objBroadcastIpAddr = IPAddress.Parse("226.1.0.1");// simulation equity ip
            //BindSockets(objInterfaceIpAddr, objBroadcastIpAddr, 12401);//simulation

            //Code For COunters
            //StopTicking = new RelayCommand<string>(CountLastTick);

            //Equity
            //IPAddress objBroadcastIpAddr = IPAddress.Parse("227.0.0.21");//live production equity ip
            //BindSockets(objInterfaceIpAddr, objBroadcastIpAddr, 12996);//live port    

            IPAddress objBroadcastIpAddr = IPAddress.Parse(MainWindowVM.ParserBoltIni.GetSetting("RSC", "EQTMultiCastAdd"));//live production equity ip
            BindSockets(objInterfaceIpAddr, objBroadcastIpAddr, Convert.ToInt32(MainWindowVM.ParserBoltIni.GetSetting("RSC", "EQTUDPPort")));//live port    

            //CommonMessageWindowVM.CMWCollection.Add(new Model.CommonMessageWindowModel { Category = "BroadCast", Message = "Connecting to BroadCast server -  IP: " + objBroadcastIpAddr.ToString() + " Port: " + Convert.ToString(TwsMainWindowVM.ParserBoltIni.GetSetting("RSC", "EQTUDPPort")), Time = CommonFunctions.GetDate().ToLongTimeString(), ColorChange = "Black" });
            //CommonMessageWindowVM.CMWCollection.Add(new Model.CommonMessageWindowModel { Category = "TouchLine", Message = "Connected to BroadCast server", Time = DateTime.Now.ToString() });

            //Derivative Section
            //  IPAddress objBroadcastIpAddrDer = IPAddress.Parse("228.0.0.21");//live production equity ip
            // BindSocketsDerivative(objInterfaceIpAddr, objBroadcastIpAddrDer, 11996);

            //CurrencySection
            //            IPAddress objBroadcastIpAddrCurr = IPAddress.Parse("229.0.0.21");//live production equity ip
            //          BindSocketsCurrency(objInterfaceIpAddr, objBroadcastIpAddrCurr, 10996);

        }

        private void CountLastTick(string emptystring)
        {
            //Code For COunters
            // calculatetime1 = true;
            // WriteCount=CountUpdatedperView;
        }

        private void BindSockets(IPAddress interfaceIp, IPAddress broadCastIp, int broadCastPort)
        {
#region Commented


            //try
            //{
            //    InterfaceIp = interfaceIp;
            //    BroadCastIp = broadCastIp;
            //    BroadCastPort = broadCastPort;
            //    //sw = File.AppendText("SuhasOrders.txt");
            //    //sw.AutoFlush = true;

            //    if (_client == null)
            //        _client = new UdpClient();
            //    _client.JoinMulticastGroup(BroadCastIp);
            //    _client.Client.Bind(new IPEndPoint(InterfaceIp, BroadCastPort));
            //    UdpState state = new UdpState { WorkSocket = _client };
            //    _client.BeginReceive(DataReceived, state);
            //}
            //catch (Exception e)
            //{
            //    ExceptionUtility.LogError(e);
            //    //MessageBox.Show(e.Message);
            //}
#endregion

            try
            {
                InterfaceIp = interfaceIp;
                BroadCastIp = broadCastIp;
                BroadCastPort = broadCastPort;
                //sw = File.AppendText("SuhasOrders.txt");
                //sw.AutoFlush = true;

                if (_client == null)
                    _client = new UdpClient();
                _client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                _client.JoinMulticastGroup(BroadCastIp);
                _client.Client.Bind(new IPEndPoint(InterfaceIp, BroadCastPort));
                UdpState state = new UdpState { WorkSocket = _client };
                _client.BeginReceive(DataReceived, state);
            }
            catch (Exception e)
            {
                ExceptionUtility.LogError(e);
                //MessageBox.Show(e.Message);
            }
        }

        private void BindSocketsDerivative(IPAddress objInterfaceIpAddr, IPAddress objBroadcastIpAddrDer, int port)
        {
            try
            {
                InterfaceIpDer = objInterfaceIpAddr;
                BroadCastIpDer = objBroadcastIpAddrDer;
                BroadCastPortDer = port;
                //sw = File.AppendText("SuhasOrders.txt");
                //sw.AutoFlush = true;

                if (_clientDer == null)
                    _clientDer = new UdpClient();
                _clientDer.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                _clientDer.JoinMulticastGroup(BroadCastIpDer);
                _clientDer.Client.Bind(new IPEndPoint(InterfaceIpDer, BroadCastPortDer));
                UdpState state = new UdpState { WorkSocket = _clientDer };
                _clientDer.BeginReceive(DataReceivedDer, state);
            }
            catch (Exception e)
            {
                ExceptionUtility.LogError(e);
                //MessageBox.Show(e.Message);
            }
        }



        private void BindSocketsCurrency(IPAddress objInterfaceIpAddr, IPAddress objBroadcastIpAddrCurr, int port)
        {
#region Commented


            //try
            //{
            //    InterfaceIpCurr = objInterfaceIpAddr;
            //    BroadCastIpCurr = objBroadcastIpAddrCurr;
            //    BroadCastPortCurr = port;
            //    //sw = File.AppendText("SuhasOrders.txt");
            //    //sw.AutoFlush = true;

            //    if (_clientCurr == null)
            //        _clientCurr = new UdpClient();
            //    _clientCurr.JoinMulticastGroup(BroadCastIpCurr);
            //    _clientCurr.Client.Bind(new IPEndPoint(InterfaceIpCurr, BroadCastPortCurr));
            //    UdpState state = new UdpState { WorkSocket = _clientCurr };
            //    _clientCurr.BeginReceive(DataReceivedCurr, state);
            //}
            //catch (Exception e)
            //{
            //    ExceptionUtility.LogError(e);
            //    //MessageBox.Show(e.Message);
            //}
#endregion

            try
            {
                InterfaceIpCurr = objInterfaceIpAddr;
                BroadCastIpCurr = objBroadcastIpAddrCurr;
                BroadCastPortCurr = port;
                //sw = File.AppendText("SuhasOrders.txt");
                //sw.AutoFlush = true;

                if (_clientCurr == null)
                    _clientCurr = new UdpClient();
                _clientCurr.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                _clientCurr.JoinMulticastGroup(BroadCastIpCurr);
                _clientCurr.Client.Bind(new IPEndPoint(InterfaceIpCurr, BroadCastPortCurr));
                UdpState state = new UdpState { WorkSocket = _clientCurr };
                _clientCurr.BeginReceive(DataReceivedCurr, state);
            }
            catch (Exception e)
            {
                ExceptionUtility.LogError(e);
                //MessageBox.Show(e.Message);
            }
        }


        private void DataReceived(IAsyncResult ar)
        {
            try
            {
                // tBcast.Stop();
                UdpClient c = ((UdpState)ar.AsyncState).WorkSocket;
                IPEndPoint receivedIpEndPoint = new IPEndPoint(BroadCastIp, 0);
                Byte[] receiveBytes = c.EndReceive(ar, ref receivedIpEndPoint);

                Int32 msgType = Swap32(BitConverter.ToInt32(receiveBytes, 0));

                switch (msgType)
                {

                    case 2020:
                        try
                        {
                            if (Constants._sessionId >= Constants.MemberQuerySession)
                                return;
                            //Commented as this causes duplication in recieving the data at the same millisecond
                            //   ThreadPool.QueueUserWorkItem(UpdateScripTicker, receiveBytes);
                            // Common objCommon = new Common();

                            UpdateScripTicker(receiveBytes);
                        }
                        catch (Exception e)
                        {
                            ExceptionUtility.LogError(e);
                            Console.WriteLine(e);
                        }
                        break;
                }

                UdpState state = new UdpState { WorkSocket = c };
                c.BeginReceive(DataReceived, state);
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
                Console.WriteLine(ex.Message);
            }

        }

        private void DataReceivedDer(IAsyncResult ar)
        {
            throw new NotImplementedException();
        }


        private void DataReceivedCurr(IAsyncResult ar)
        {
            throw new NotImplementedException();
        }

        private void UpdateScripTicker(object objreceiveBytes)
        {

            byte[] receiveBytes = objreceiveBytes as byte[];
            int offset = 0;
            if (receiveBytes == null)
                return;
            Int32 msgtype = Swap32(BitConverter.ToInt32(receiveBytes, offset));
            offset += sizeof(Int32);

            try
            {
                Int32 reservefieldL1 = Swap32(BitConverter.ToInt32(receiveBytes, offset));
                offset += sizeof(Int32);
                Int32 sequenceNoS = Swap32(BitConverter.ToInt32(receiveBytes, offset));
                offset += sizeof(Int32);
                offset += sizeof(Int16); // Not Processsed Reservefield_us
                Int16 hourS = Swap16(BitConverter.ToInt16(receiveBytes, offset));
                offset += sizeof(Int16);
                Int16 minS = Swap16(BitConverter.ToInt16(receiveBytes, offset));
                offset += sizeof(Int16);
                Int16 secS = Swap16(BitConverter.ToInt16(receiveBytes, offset));
                offset += sizeof(Int16);
                Int16 mSecS = Swap16(BitConverter.ToInt16(receiveBytes, offset));
                starttime = hourS + ":" + minS + ":" + secS + ":" + mSecS;
                offset += sizeof(Int16);
                Int16 fillerS = Swap16(BitConverter.ToInt16(receiveBytes, offset));
                offset += sizeof(Int16);
                Int16 tradingSessionS = Swap16(BitConverter.ToInt16(receiveBytes, offset));
                offset += sizeof(Int16);
                Int16 noofRecsS = Swap16(BitConverter.ToInt16(receiveBytes, offset));
                offset += sizeof(Int16);
                short noRecS;
                for (noRecS = 0; noRecS < noofRecsS; noRecS++)
                {
                    //Code For COunters
                    //CountbeforeMaterFilter++;
                    ScripDetails buysellObj = new ScripDetails();
                    if (offset < receiveBytes.Length)
                    {

                        Int32 scripCodeL = Swap32(BitConverter.ToInt32(receiveBytes, offset));
                        offset += sizeof(Int32);

                        Int32 noOfTradesL = Swap32(BitConverter.ToInt32(receiveBytes, offset));
                        offset += sizeof(Int32);

                        Int32 tradedVolumeL = Swap32(BitConverter.ToInt32(receiveBytes, offset));
                        offset += sizeof(Int32);

                        Int32 tradedValueL = Swap32(BitConverter.ToInt32(receiveBytes, offset));
                        offset += sizeof(Int32);

                        List<byte> lst = new List<byte>();
                        lst.Add(receiveBytes[offset]);
                        lst.Add(new byte());
                        char Unit_c;
                        Unit_c = BitConverter.ToChar(lst.ToArray<byte>(), 0);
                        // return cc.ToString();

                        // char Unit_c = BitConverter.ToChar(receiveBytes[offset]);//(TmpDataPtr1[DataPtr]);
                        offset += sizeof(byte);

                        //char Trend_c = (TmpDataPtr1[DataPtr]);
                        offset += sizeof(byte);

                        //char SunShine_c = (TmpDataPtr1[DataPtr]); //for sunshine flag which is not used.
                        offset += sizeof(byte);

                        //char AlNnFlag_c = (TmpDataPtr1[DataPtr]);
                        offset += sizeof(byte);

                        Int16 marketTypeS = Swap16(BitConverter.ToInt16(receiveBytes, offset));
                        offset += sizeof(Int16);

                        Int16 sessionNoS = Swap16(BitConverter.ToInt16(receiveBytes, offset));
                        offset += sizeof(Int16);

                        Int32 ltphr = receiveBytes[offset];
                        offset += sizeof(byte); // Not Processsed   Hour_c;
                        Int32 ltpmin = receiveBytes[offset];
                        offset += sizeof(byte); // Not Processsed   Min_c;
                        Int32 ltpsec = receiveBytes[offset];
                        offset += sizeof(byte); // Not Processsed   Sec_c;

                        string ltpmsec = string.Empty;

                        ltpmsec += receiveBytes[offset];
                        offset += sizeof(byte); // Not Processsed   LTPMillisec_c[3];
                        ltpmsec += receiveBytes[offset];
                        offset += sizeof(byte); // Not Processsed   LTPMillisec_c[3];
                        ltpmsec += receiveBytes[offset];
                        offset += sizeof(byte); // Not Processsed   LTPMillisec_c[3];
                        offset += sizeof(byte); // Not Processsed   ReserveField_c[3];
                        offset += sizeof(byte); // Not Processsed   ReserveField_c[3];
                        offset += sizeof(short); // Not Processsed    Filler_s1;
                        offset += sizeof(short); // Not Processsed    No_of_PricePoints_s;
                        offset += sizeof(Int64); // Not Processsed    ReserveField_ll;

                        Int32 closePriceL = Swap32(BitConverter.ToInt32(receiveBytes, offset));
                        offset += sizeof(Int32);

                        Int32 lastTradeQtyL = Swap32(BitConverter.ToInt32(receiveBytes, offset));
                        offset += sizeof(Int32);

                        Int32 lastTradeRateL = Swap32(BitConverter.ToInt32(receiveBytes, offset));
                        offset += sizeof(Int32);

                        Int32 openRateL = DecryptInt32(lastTradeRateL, receiveBytes, ref offset);
                        Int32 closeRateL = DecryptInt32(lastTradeRateL, receiveBytes, ref offset);
                        Int32 highRateL = DecryptInt32(lastTradeRateL, receiveBytes, ref offset);
                        Int32 lowRateL = DecryptInt32(lastTradeRateL, receiveBytes, ref offset);
                        Int32 reserveFieldL = DecryptInt32(lastTradeRateL, receiveBytes, ref offset);
                        Int32 indicativeEqPriceL = DecryptInt32(lastTradeRateL, receiveBytes, ref offset);
                        Int32 indicativeEqQuantityL = DecryptInt32(lastTradeQtyL, receiveBytes, ref offset);
                        Int32 totBuyQtyL = DecryptInt32(lastTradeQtyL, receiveBytes, ref offset);
                        Int32 totSellQtyL = DecryptInt32(lastTradeQtyL, receiveBytes, ref offset);
                        Int32 lowerCktLmtL = DecryptInt32(lastTradeRateL, receiveBytes, ref offset);
                        Int32 upperCktLmtL = DecryptInt32(lastTradeRateL, receiveBytes, ref offset);
                        Int32 wtAvgRateL = DecryptInt32(lastTradeRateL, receiveBytes, ref offset);

                        buysellObj.ScriptCode_BseToken_NseToken = scripCodeL;
                        buysellObj.lastTradeRateL = lastTradeRateL;
                        buysellObj.openRateL = openRateL;
                        buysellObj.closeRateL = closeRateL;
                        buysellObj.highRateL = highRateL;
                        buysellObj.lowRateL = lowRateL;
                        buysellObj.totBuyQtyL = totBuyQtyL;
                        buysellObj.totSellQtyL = totSellQtyL;
                        buysellObj.wtAvgRateL = wtAvgRateL;
                        buysellObj.Unit_c = Unit_c.ToString();

                        buysellObj.lastTradeQtyL = lastTradeQtyL;
                        buysellObj.TrdVolume = tradedVolumeL;
                        buysellObj.TrdValue = tradedValueL;
                        buysellObj.LowerCtLmt = lowerCktLmtL;
                        buysellObj.UprCtLmt = upperCktLmtL;
                        buysellObj.NoOfTrades = noOfTradesL;
                        buysellObj.IndicateEqPrice = indicativeEqPriceL;
                        buysellObj.IndicateEqQty = indicativeEqQuantityL;
                        buysellObj.LastTradeTime = string.Format("{0:00}", ltphr) + string.Format("{0:00}", ltpmin) + string.Format("{0:00}", ltpsec);  //ltpmin.ToString() + ltpsec.ToString();

                        List<BestFive> lstBstFive = new List<BestFive>(5);

                        for (int i = 0; i < 5; i++)
                        {
                            short conv = BitConverter.ToInt16(receiveBytes, offset);
                            if (Swap16(conv) != 32766)
                            {
                                if (i == 0)
                                {

                                    BestFive bfObj1 = new BestFive
                                    {
                                        BuyRateL = DecryptInt32(lastTradeRateL, receiveBytes, ref offset),
                                        BuyQtyL = DecryptInt32(lastTradeQtyL, receiveBytes, ref offset),
                                        NoOfBidBuyL = DecryptInt32(lastTradeQtyL, receiveBytes, ref offset),
                                        FillerBuyL = DecryptInt32(lastTradeQtyL, receiveBytes, ref offset),
                                    };

                                    lstBstFive.Add(bfObj1);
                                    buysellObj.BuyRateL = bfObj1.BuyRateL;
                                    buysellObj.BuyQtyL = bfObj1.BuyQtyL;
                                    buysellObj.NoOfBidBuyL = bfObj1.NoOfBidBuyL;
                                    buysellObj.FillerBuyL = bfObj1.FillerBuyL;
                                    //lstBstFive.Add(buysellObj);
                                    //Code For COunters
                                    //buysellObj.startTime = starttime;

                                }
                                else
                                {
                                    BestFive bfObj1 = new BestFive();

                                    try
                                    {

                                        bfObj1.BuyRateL = DecryptInt32(lstBstFive[i - 1].BuyRateL,
                                                                      receiveBytes, ref offset);
                                        bfObj1.BuyQtyL = DecryptInt32(lstBstFive[i - 1].BuyQtyL, receiveBytes,
                                                                     ref offset);
                                        bfObj1.NoOfBidBuyL = DecryptInt32(lstBstFive[i - 1].NoOfBidBuyL,
                                                                         receiveBytes, ref offset);
                                        bfObj1.FillerBuyL = DecryptInt32(lstBstFive[i - 1].FillerBuyL,
                                                                        receiveBytes, ref offset);
                                        //Code For COunters
                                        //bfObj1.startTime = starttime;
                                        lstBstFive.Add(bfObj1);
                                    }
                                    catch (Exception e)
                                    {
                                        ExceptionUtility.LogError(e);
                                        MessageBox.Show(e.Message);
                                    }
                                }
                            }
                            else
                            {
                                offset += sizeof(short);

                                for (int j = 0; j < 5 - i; j++)
                                {
                                    BestFive bfObj = new BestFive();
                                    //Code For COunters
                                    // bfObj.startTime = starttime;
                                    lstBstFive.Add(bfObj);
                                }
                                break;
                            }
                        }

                        for (int i = 0; i < 5; i++)
                        {
                            if (Swap16(BitConverter.ToInt16(receiveBytes, offset)) != -32766)
                            {
                                if (i == 0)
                                {
                                    lstBstFive[i].SellRateL = DecryptInt32(lastTradeRateL, receiveBytes,
                                                                            ref offset);
                                    lstBstFive[i].SellQtyL = DecryptInt32(lastTradeQtyL, receiveBytes,
                                                                           ref offset);
                                    lstBstFive[i].NoOfBidSellL = DecryptInt32(lastTradeQtyL, receiveBytes,
                                                                               ref offset);
                                    lstBstFive[i].FillerSellL = DecryptInt32(lastTradeQtyL, receiveBytes,
                                                                              ref offset);
                                    buysellObj.SellRateL = lstBstFive[i].SellRateL;
                                    buysellObj.SellQtyL = lstBstFive[i].SellQtyL;
                                    buysellObj.NoOfBidSellL = lstBstFive[i].NoOfBidSellL;
                                    buysellObj.FillerSellL = lstBstFive[i].FillerSellL;
                                    //Code For COunters
                                    //buysellObj.startTime=lstBstFive[i].startTime;

                                }
                                else
                                {
                                    lstBstFive[i].SellRateL = DecryptInt32(lstBstFive[i - 1].SellRateL,
                                                                            receiveBytes, ref offset);
                                    lstBstFive[i].SellQtyL = DecryptInt32(lstBstFive[i - 1].SellQtyL,
                                                                           receiveBytes, ref offset);
                                    lstBstFive[i].NoOfBidSellL = DecryptInt32(
                                        lstBstFive[i - 1].NoOfBidSellL, receiveBytes, ref offset);
                                    lstBstFive[i].FillerSellL = DecryptInt32(lstBstFive[i - 1].FillerSellL,
                                                                              receiveBytes, ref offset);
                                    //Code For COunters
                                    // buysellObj.startTime = starttime;
                                }
                            }
                            else
                            {

                                offset += sizeof(short);
                                break;
                            }
                        }

                        buysellObj.listBestFive = lstBstFive;
                        if (BroadcastMasterMemory.objScripDetailsConDict.ContainsKey(buysellObj.ScriptCode_BseToken_NseToken))
                        {
                            BroadcastMasterMemory.objScripDetailsConDict[buysellObj.ScriptCode_BseToken_NseToken] = buysellObj;
                        }
                        else
                        {
                            BroadcastMasterMemory.objScripDetailsConDict.TryAdd(buysellObj.ScriptCode_BseToken_NseToken, buysellObj);
                        }
                        //BestFiveWindowVM.UpdateBestWindow(lstBstFive1); 
                        if (OnMessageTransmitted != null)
                            OnMessageTransmitted(buysellObj.ScriptCode_BseToken_NseToken);

                    }
                }
            }
            catch (Exception e)
            {
                ExceptionUtility.LogError(e);
                MessageBox.Show(e.Message);
            }
        }

        private static Int32 Swap32(Int32 value)
        {
            return (Int32)((value & 0x000000FFU) << 24 | (value & 0x0000FF00U) << 8 |
                            (value & 0x00FF0000U) >> 8 | (value & 0xFF000000U) >> 24);
        }

        private static Int16 Swap16(Int16 value)
        {
            return (Int16)((value & 0xFFU) << 8 | (value & 0xFF00U) >> 8);
        }

        private static string GetStringFromCharArray(char[] cArray)
        {
            var temp = new string(cArray);
            try
            {
                if (temp.Contains("\0"))
                {
                    temp = temp.Remove(temp.IndexOf("\0", StringComparison.Ordinal));
                    temp = temp.Replace("\0", string.Empty);
                }
            }
            catch (Exception e)
            {
                ExceptionUtility.LogError(e);
                Console.WriteLine(e);
            }
            return temp.Trim();
        }

        private Int32 DecryptInt32(Int32 baseValue, byte[] receiveBytes, ref Int32 offset)
        {
            Int32 returnValue = 0;
            try
            {
                Int16 test = Swap16(BitConverter.ToInt16(receiveBytes, offset));

                if (test == 32767)
                {
                    offset += sizeof(short);
                    returnValue = Swap32(BitConverter.ToInt32(receiveBytes, offset));
                    offset += sizeof(Int32);
                }
                else
                {
                    returnValue = baseValue + test;
                    offset += sizeof(Int16);
                }
            }
            catch (Exception e)
            {
                ExceptionUtility.LogError(e);
                Console.WriteLine(e);
            }

            return returnValue;
        }
    }

#endif
}
