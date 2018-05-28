using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.Timers;
using System.ComponentModel;
using System.Collections.Concurrent;
using BroadcastReceiver;
using BroadcastMaster;
using SubscribeList;
using System.IO;
using static BroadcastReceiver.RBIMainDetails;

namespace BroadcastReceiver
{
    public class BroadcastListener
    {
       
        private IPAddress BroadCastIp { get; set; }
        private IPAddress InterFaceIp { get; set; }
        private int BroadCastPort;
        
        UdpClient UdpListner = null;
        public Thread m_ListeningThread;
        public System.Timers.Timer _timerData;
        
        private bool listening;

        public long Datacounter = 0;
        ManualResetEvent _shutdownEvent = new ManualResetEvent(false);
        ManualResetEvent _pauseEvent = new ManualResetEvent(true);


        private long UserId { get; set; }
        private string LoginKey { get; set; }
        private string SockCompressionType { get; set; }
        private string TCPConnectMessage { get; set; }
        private string TCPMBPMessage { get; set; }
        private int TimeOut { get; set; }

        public long Datacounter60 = 0, Datacounter61 = 0, Datacounter69 = 0, Datacounter72 = 0;

        TcpClient tcpClient = null;
        private NetworkStream tcpStream;



        // Action<MyMessageArgs> OnDataReceived;
        public delegate void BcastEventHandler(byte[] rBytes,string empty,int Segment);
        public event BcastEventHandler ReceivedDataEvent;

        public const short BYTEOFFSET8 = 8;
        public const short BYTEOFFSET4 = 4;
        public const short BYTEOFFSET2 = 2;
        public const short BYTEOFFSET1 = 1;
        public const short BYTEOFFSET7 = 7;
        public const short BYTEOFFSET40 = 40;
        public byte[] BYTEOFFSET7a = new byte[7];

        public const int MKTPIC_EQT = 2020;
        public const int SENSEXBCAST = 2011, ALLINDEXBCAST = 2012;
        public const int OPENPRICEBCAST = 2013, CLOSPRICEBCAST = 2014;
        public const int OIRBCAST = 2015, VARBCAST = 2016;
        public const int RBIBCAST = 2022;
        public const int NEWSBCAST = 2004;
        public const int SESSIONBCAST = 2002;
        public const int AUCTIONSESSION = 2003;

        public const short NoOfRecords2012_2011 = 24;
        public const short NoOfRecords2013_2014 = 80;
        public const short NoOfRecords2015 = 26;
        public const short NoOfRecords2016 = 40;
        public const short NoOfRecords2022 = 12;

        //public delegate void OICurrencyTickHandler();
        //public event OICurrencyTickHandler OICurrencyTick;
        //public delegate void OIDerivativeTickHandler();
        //public event OIDerivativeTickHandler OIDerivateTick;
        //public delegate void VarEquityTickHandler();
        //public event VarEquityTickHandler VarEquityTick;
        public delegate void SessionEquityTickHandler();
        public event SessionEquityTickHandler SessionEquityTick;
        public delegate void SessionCurrencyTickHandler();
        public event SessionCurrencyTickHandler SessionCurrencyTick;
        public delegate void SessionDerivativeTickHandler();
        public event SessionDerivativeTickHandler SessionDerivativeTick;
        public delegate void NewsEquityTickHandler();
        public event NewsEquityTickHandler NewsEquityTick;
        public delegate void NewsCurrencyTickHandler();
        public event NewsCurrencyTickHandler NewsCurrencyTick;
        public delegate void NewsDerivativeTickHandler();
        public event NewsDerivativeTickHandler NewsDerivativeTick;
        public delegate void IndicesTickHandler();
        public event IndicesTickHandler IndicesTick;


        private Int64 Swap64(Int64 value)
        {

            return (Int64)((value & 0x00000000000000ff) << 56 | (value & 0x000000000000ff00) << 40 |
                            (value & 0x0000000000ff0000) << 24 | (value & 0x00000000ff000000) << 8 |
                            (value & 0x000000ff00000000) >> 8 | (value & 0x0000ff0000000000) >> 24 |
                            (value & 0x00ff000000000000) >> 40 | (value & 0xff000000000000)>>56);
        }                                                                

        private Int32 Swap32(Int32 value)
        {
            return (Int32)((value & 0x000000FFU) << 24 | (value & 0x0000FF00U) << 8 |
                            (value & 0x00FF0000U) >> 8 | (value & 0xFF000000U) >> 24);
        }

        private Int16 Swap16(Int16 value)
        {
            return (Int16)((value & 0xFFU) << 8 | (value & 0xFF00U) >> 8);
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
                Console.WriteLine(e);
            }

            return returnValue;
        }

        private void UpdateSessionBcast(byte[] receiveBytes, int offset, int Segment)
        {
            SessionMain objSessionMain = new SessionMain();
            offset = 0;
            objSessionMain.MessType = Swap32(BitConverter.ToInt32(receiveBytes, offset)); offset += BYTEOFFSET4;
            objSessionMain.ReservedF1 = Swap32(BitConverter.ToInt32(receiveBytes, offset)); offset += BYTEOFFSET4;
            objSessionMain.ReservedF2 = Swap32(BitConverter.ToInt32(receiveBytes, offset)); offset += BYTEOFFSET4;
            offset += BYTEOFFSET2;

            objSessionMain.Hour = Swap16(BitConverter.ToInt16(receiveBytes, offset)); offset += BYTEOFFSET2;
            objSessionMain.Minute = Swap16(BitConverter.ToInt16(receiveBytes, offset)); offset += BYTEOFFSET2;
            objSessionMain.Second = Swap16(BitConverter.ToInt16(receiveBytes, offset)); offset += BYTEOFFSET2;
            objSessionMain.Millisecond = Swap16(BitConverter.ToInt16(receiveBytes, offset)); offset += BYTEOFFSET2;
            objSessionMain.ProductID= Swap16(BitConverter.ToInt16(receiveBytes, offset)); offset += BYTEOFFSET2;
            objSessionMain.ReservedField4 = Swap16(BitConverter.ToInt16(receiveBytes, offset)); offset += BYTEOFFSET2;
            objSessionMain.Filler = Swap16(BitConverter.ToInt16(receiveBytes, offset)); offset += BYTEOFFSET2;
            objSessionMain.MarketType = Swap16(BitConverter.ToInt16(receiveBytes, offset)); offset += BYTEOFFSET2;
            objSessionMain.SessionNumber = Swap16(BitConverter.ToInt16(receiveBytes, offset)); offset += BYTEOFFSET2;
            objSessionMain.ReservedField5 = Swap32(BitConverter.ToInt32(receiveBytes, offset)); offset += BYTEOFFSET4;
            objSessionMain.StartEndFlag = BitConverter.ToChar(receiveBytes, offset);  offset += BYTEOFFSET1;
    
            objSessionMain.ReservedField6= BitConverter.ToChar(receiveBytes, offset); offset += BYTEOFFSET1;
            offset += BYTEOFFSET2;
            SessionMemory.SubscribeSessionMemoryDict.TryAdd(objSessionMain.ProductID,objSessionMain);
            if (Segment == 0)
            {
                if (SessionEquityTick != null)
                {
                    SessionEquityTick();
                }
            }
            if (Segment == 1)
            {
                if (SessionDerivativeTick != null)
                {
                    SessionDerivativeTick();
                }
            }
            if (Segment == 2)
            {
                if (SessionCurrencyTick != null)
                {
                    SessionCurrencyTick();
                }
            }
        }

         private void UpdateNewsPrice(byte[] receiveBytes, int offset, int Segment)
        {
            NewsMain objNewsMain = new NewsMain();

            offset = 0;
            objNewsMain.MessType = Swap32(BitConverter.ToInt32(receiveBytes, offset)); offset += BYTEOFFSET4;
            objNewsMain.ReservedF1 = Swap32(BitConverter.ToInt32(receiveBytes, offset)); offset += BYTEOFFSET4;
            objNewsMain.ReservedF2 = Swap32(BitConverter.ToInt32(receiveBytes, offset)); offset += BYTEOFFSET4;
            offset += BYTEOFFSET2;

            objNewsMain.Hour = Swap16(BitConverter.ToInt16(receiveBytes, offset)); offset += BYTEOFFSET2;
            objNewsMain.Minute = Swap16(BitConverter.ToInt16(receiveBytes, offset)); offset += BYTEOFFSET2;
            objNewsMain.Second = Swap16(BitConverter.ToInt16(receiveBytes, offset)); offset += BYTEOFFSET2;
            objNewsMain.Millisecond = Swap16(BitConverter.ToInt16(receiveBytes, offset)); offset += BYTEOFFSET2;
            objNewsMain.ReservedF4 = Swap16(BitConverter.ToInt16(receiveBytes, offset)); offset += BYTEOFFSET2;
            objNewsMain.ReservedF5 = Swap16(BitConverter.ToInt16(receiveBytes, offset)); offset += BYTEOFFSET2;
            objNewsMain.ReservedF6 = Swap16(BitConverter.ToInt16(receiveBytes, offset)); offset += BYTEOFFSET2;
            objNewsMain.NewsCategory= Swap16(BitConverter.ToInt16(receiveBytes, offset)); offset += BYTEOFFSET2;
            objNewsMain.ReservedF7 = Swap16(BitConverter.ToInt16(receiveBytes, offset)); offset += BYTEOFFSET2;
            objNewsMain.NewsID = Swap32(BitConverter.ToInt32(receiveBytes, offset)); offset += BYTEOFFSET4;
            objNewsMain.NewsHeadline = Encoding.UTF8.GetString(receiveBytes, offset, BYTEOFFSET40); offset += BYTEOFFSET40;
            offset += BYTEOFFSET1;
            offset += BYTEOFFSET1;
            offset += BYTEOFFSET2;
            NewsMemory.SubscribeNewsMemoryDict.TryAdd(objNewsMain.NewsID,objNewsMain);
            if (Segment == 0)
            {
                if (NewsEquityTick != null)
                {
                    NewsEquityTick();
                }
            }
            if (Segment == 1)
            {
                if (NewsDerivativeTick != null)
                {
                    NewsDerivativeTick();
                }
            }
            if (Segment == 2)
            {
                if (NewsCurrencyTick != null)
                {
                    NewsCurrencyTick();
                }
            }

        }

        private void UpdateOIPrice(byte[] receiveBytes, int offset,int Segment)
        {
            try
            {
                OIMain objOIMain = new OIMain();

                offset = 0;
                objOIMain.MessType = Swap32(BitConverter.ToInt32(receiveBytes, offset)); offset += BYTEOFFSET4;
                objOIMain.ReservedF1 = Swap32(BitConverter.ToInt32(receiveBytes, offset)); offset += BYTEOFFSET4;
                objOIMain.ReservedF2 = Swap32(BitConverter.ToInt32(receiveBytes, offset)); offset += BYTEOFFSET4;
                offset += BYTEOFFSET2;

                objOIMain.Hour = Swap16(BitConverter.ToInt16(receiveBytes, offset)); offset += BYTEOFFSET2;
                objOIMain.Minute = Swap16(BitConverter.ToInt16(receiveBytes, offset)); offset += BYTEOFFSET2;
                objOIMain.Second = Swap16(BitConverter.ToInt16(receiveBytes, offset)); offset += BYTEOFFSET2;
                objOIMain.Millisecond = Swap16(BitConverter.ToInt16(receiveBytes, offset)); offset += BYTEOFFSET2;
                objOIMain.ReservedF4 = Swap16(BitConverter.ToInt16(receiveBytes, offset)); offset += BYTEOFFSET2;
                objOIMain.ReservedF5 = Swap16(BitConverter.ToInt16(receiveBytes, offset)); offset += BYTEOFFSET2;
                objOIMain.NoOfRec = Swap16(BitConverter.ToInt16(receiveBytes, offset)); offset += BYTEOFFSET2;
                objOIMain.Segment = Segment;
                if (objOIMain.NoOfRec > NoOfRecords2015)
                {
                    objOIMain.NoOfRec = NoOfRecords2015;
                }
                
                for (short rec = 0; rec < objOIMain.NoOfRec; rec++)
                {
                    objOIMain.ObjOIMainDetails[rec].InstrumentID = Swap32(BitConverter.ToInt32(receiveBytes, offset)); offset += BYTEOFFSET4;
                    objOIMain.ObjOIMainDetails[rec].OpenInterestQuantity = Swap32(BitConverter.ToInt32(receiveBytes, offset)); offset += BYTEOFFSET4;
                    objOIMain.ObjOIMainDetails[rec].OpenInterestValue = Swap64(BitConverter.ToInt64(receiveBytes, offset)); offset += BYTEOFFSET8;
                    objOIMain.ObjOIMainDetails[rec].OpenInterestChange = Swap32(BitConverter.ToInt32(receiveBytes, offset)); offset += BYTEOFFSET4;

                    offset += BYTEOFFSET4;
                    objOIMain.ObjOIMainDetails[rec].ReservedField2 = Swap32(BitConverter.ToInt32(receiveBytes, offset)); offset += BYTEOFFSET4;
                    objOIMain.ObjOIMainDetails[rec].ReservedField3 = Swap16(BitConverter.ToInt16(receiveBytes, offset)); offset += BYTEOFFSET2;
                    objOIMain.ObjOIMainDetails[rec].ReservedField4 = Swap16(BitConverter.ToInt16(receiveBytes, offset)); offset += BYTEOFFSET2;
                    offset += BYTEOFFSET1;
                    offset += BYTEOFFSET1;
                    offset += BYTEOFFSET2;
                    ScripDetails objScripDetails = new ScripDetails();
                    if (BroadcastMasterMemory.objScripDetailsConDict != null && BroadcastMasterMemory.objScripDetailsConDict.Count > 0)
                    {

                        objScripDetails = BroadcastMasterMemory.objScripDetailsConDict.Values.Where(x => x.ScripCode_l == objOIMain.ObjOIMainDetails[rec].InstrumentID).FirstOrDefault();
                        if(objScripDetails != null)
                        objOIMain.ObjOIMainDetails[rec].TotalTradedValue = objScripDetails.TradedVolume_l;

                    }


                    if (OIMemory.SubscribeOIMemoryDict.Keys.Contains(objOIMain.ObjOIMainDetails[rec].InstrumentID))
                    {
                        OIMemory.SubscribeOIMemoryDict[objOIMain.ObjOIMainDetails[rec].InstrumentID] = objOIMain;
                    }
                    else
                    {
                        OIMemory.SubscribeOIMemoryDict.TryAdd(objOIMain.ObjOIMainDetails[rec].InstrumentID, objOIMain);
                    }
                  
                    
                }

                //if (Segment == 1)
                //{
                //    if (OIDerivateTick != null)
                //    {
                //        OIDerivateTick();
                //    }
                //}
                //if (Segment == 2)
                //{
                //    if (OICurrencyTick != null)
                //    {
                //        OICurrencyTick();
                //    }
                //}

            }
            catch(Exception ex)
            {

            }
        }

        //private void UpdateRBI(byte[] receiveBytes, int offset)
        //{
        //    RBIMain objRBIMain = new RBIMain();

        //    offset = 0;
        //    objRBIMain.MessType = Swap32(BitConverter.ToInt32(receiveBytes, offset)); offset += BYTEOFFSET4;
        //    objRBIMain.ReservedF1 = Swap32(BitConverter.ToInt32(receiveBytes, offset)); offset += BYTEOFFSET4;
        //    objRBIMain.ReservedF2 = Swap32(BitConverter.ToInt32(receiveBytes, offset)); offset += BYTEOFFSET4;
        //    offset += BYTEOFFSET2;

        //    objRBIMain.Hour = Swap16(BitConverter.ToInt16(receiveBytes, offset)); offset += BYTEOFFSET2;
        //    objRBIMain.Minute = Swap16(BitConverter.ToInt16(receiveBytes, offset)); offset += BYTEOFFSET2;
        //    objRBIMain.Second = Swap16(BitConverter.ToInt16(receiveBytes, offset)); offset += BYTEOFFSET2;
        //    objRBIMain.Millisecond = Swap16(BitConverter.ToInt16(receiveBytes, offset)); offset += BYTEOFFSET2;
        //    objRBIMain.ReservedF4 = Swap16(BitConverter.ToInt16(receiveBytes, offset)); offset += BYTEOFFSET2;
        //    objRBIMain.ReservedF5 = Swap16(BitConverter.ToInt16(receiveBytes, offset)); offset += BYTEOFFSET2;
        //    objRBIMain.NoOfRec = Swap16(BitConverter.ToInt16(receiveBytes, offset)); offset += BYTEOFFSET2;

        //    if (objRBIMain.NoOfRec > NoOfRecords2022)
        //    {
        //        objRBIMain.NoOfRec = NoOfRecords2022;
        //    }
        //    for (short rec = 0; rec < objRBIMain.NoOfRec; rec++)
        //    {
        //        objRBIMain.RBIMainDetailsObj[rec].UnderlyingAssetId = Swap32(BitConverter.ToInt32(receiveBytes, offset)); offset += BYTEOFFSET4;
        //        objRBIMain.RBIMainDetailsObj[rec].RBIRate = Swap32(BitConverter.ToInt32(receiveBytes, offset)); offset += BYTEOFFSET4;
        //         offset += BYTEOFFSET2;
        //        offset += BYTEOFFSET2;
              
        //        objRBIMain.RBIMainDetailsObj[rec].Date = Encoding.UTF8.GetString(receiveBytes, offset, BYTEOFFSET7); offset += BYTEOFFSET7;
        //        objRBIMain.RBIMainDetailsObj[rec].Filler = BitConverter.ToChar(receiveBytes, offset); offset += BYTEOFFSET1;

        //        RBIMemory.SubscribeRBIMemoryQueue.Enqueue(objRBIMain);

        //    }
        //}

        private void UpdateVarPrice(byte[] receiveBytes, int offset, int Segment)
        {
            VarMain objVarMain = new VarMain();

            offset = 0;
            objVarMain.MessType = Swap32(BitConverter.ToInt32(receiveBytes, offset)); offset += BYTEOFFSET4;
            objVarMain.ReservedF1 = Swap32(BitConverter.ToInt32(receiveBytes, offset)); offset += BYTEOFFSET4;
            objVarMain.ReservedF2 = Swap32(BitConverter.ToInt32(receiveBytes, offset)); offset += BYTEOFFSET4;
            offset += BYTEOFFSET2;

            objVarMain.Hour = Swap16(BitConverter.ToInt16(receiveBytes, offset)); offset += BYTEOFFSET2;
            objVarMain.Minute = Swap16(BitConverter.ToInt16(receiveBytes, offset)); offset += BYTEOFFSET2;
            objVarMain.Second = Swap16(BitConverter.ToInt16(receiveBytes, offset)); offset += BYTEOFFSET2;
            objVarMain.Millisecond = Swap16(BitConverter.ToInt16(receiveBytes, offset)); offset += BYTEOFFSET2;
            objVarMain.ReservedF4 = Swap16(BitConverter.ToInt16(receiveBytes, offset)); offset += BYTEOFFSET2;
            objVarMain.ReservedF5 = Swap16(BitConverter.ToInt16(receiveBytes, offset)); offset += BYTEOFFSET2;
            objVarMain.NoOfRec = Swap16(BitConverter.ToInt16(receiveBytes, offset)); offset += BYTEOFFSET2;

            if (objVarMain.NoOfRec > NoOfRecords2016)
            {
                objVarMain.NoOfRec = NoOfRecords2016;
            }
            for (short rec = 0; rec < objVarMain.NoOfRec; rec++)
            {
                objVarMain.VarMainDetailsObj[rec].InstrumentCode= Swap32(BitConverter.ToInt32(receiveBytes, offset)); offset += BYTEOFFSET4;
                objVarMain.VarMainDetailsObj[rec].IMPercentage = Swap32(BitConverter.ToInt32(receiveBytes, offset)); offset += BYTEOFFSET4;
                objVarMain.VarMainDetailsObj[rec].ELMPercentage = Swap32(BitConverter.ToInt32(receiveBytes, offset)); offset += BYTEOFFSET4;
                offset += BYTEOFFSET4;
                offset += BYTEOFFSET2;
                offset += BYTEOFFSET2;
                offset += BYTEOFFSET1;
                objVarMain.VarMainDetailsObj[rec].Identifier= BitConverter.ToChar(receiveBytes, offset); offset += BYTEOFFSET1;
                offset += BYTEOFFSET2;

                if (VarMemory.SubscribeVarMemoryDict.Keys.Contains(objVarMain.VarMainDetailsObj[rec].InstrumentCode))
                {
                    VarMemory.SubscribeVarMemoryDict[objVarMain.VarMainDetailsObj[rec].InstrumentCode] = objVarMain;
                }
                else
                {
                    VarMemory.SubscribeVarMemoryDict.TryAdd(objVarMain.VarMainDetailsObj[rec].InstrumentCode, objVarMain);
                }

             
                //if (Segment == 0)
                //{
                //    if (VarEquityTick != null)
                //    {
                //        VarEquityTick();
                //    }
                //}
              
            }
           
        }

        private void UpdateClosePrice(byte[] receiveBytes, int offset)
        {
            try
            {
                ClosePriceMain oOpenClosePriceMain = new ClosePriceMain();
                offset = 0;
                oOpenClosePriceMain.MessType = Swap32(BitConverter.ToInt32(receiveBytes, offset)); offset += BYTEOFFSET4;
                oOpenClosePriceMain.ReservedF1 = Swap32(BitConverter.ToInt32(receiveBytes, offset)); offset += BYTEOFFSET4;
                oOpenClosePriceMain.ReservedF2 = Swap32(BitConverter.ToInt32(receiveBytes, offset)); offset += BYTEOFFSET4;
                offset += BYTEOFFSET2;

                oOpenClosePriceMain.Hour = Swap16(BitConverter.ToInt16(receiveBytes, offset)); offset += BYTEOFFSET2;
                oOpenClosePriceMain.Minute = Swap16(BitConverter.ToInt16(receiveBytes, offset)); offset += BYTEOFFSET2;
                oOpenClosePriceMain.Second = Swap16(BitConverter.ToInt16(receiveBytes, offset)); offset += BYTEOFFSET2;
                oOpenClosePriceMain.Millisecond = Swap16(BitConverter.ToInt16(receiveBytes, offset)); offset += BYTEOFFSET2;
                oOpenClosePriceMain.ReservedF4 = Swap16(BitConverter.ToInt16(receiveBytes, offset)); offset += BYTEOFFSET2;
                oOpenClosePriceMain.ReservedF5 = Swap16(BitConverter.ToInt16(receiveBytes, offset)); offset += BYTEOFFSET2;
                oOpenClosePriceMain.NoOfRec = Swap16(BitConverter.ToInt16(receiveBytes, offset)); offset += BYTEOFFSET2;
                if (oOpenClosePriceMain.NoOfRec > NoOfRecords2013_2014)
                {
                    oOpenClosePriceMain.NoOfRec = NoOfRecords2013_2014;
                }
                for (short rec = 0; rec < oOpenClosePriceMain.NoOfRec; rec++)
                {
                    //IdicesDetails oIndex.ind[rec] = new IdicesDetails();
                    //IdicesDetails oIdicesDetails = new IdicesDetails();
                    oOpenClosePriceMain.CloseMain[rec].InstrumentCode = Swap32(BitConverter.ToInt32(receiveBytes, offset)); offset += BYTEOFFSET4;
                    oOpenClosePriceMain.CloseMain[rec].Price = Swap32(BitConverter.ToInt32(receiveBytes, offset)); offset += BYTEOFFSET4;
                    oOpenClosePriceMain.CloseMain[rec].ReservedField1 = BitConverter.ToChar(receiveBytes, offset); offset += BYTEOFFSET1;
                    oOpenClosePriceMain.CloseMain[rec].Traded = BitConverter.ToChar(receiveBytes, offset); offset += BYTEOFFSET1;
                    oOpenClosePriceMain.CloseMain[rec].PrecisionIndicator = BitConverter.ToChar(receiveBytes, offset); offset += BYTEOFFSET1;
                    oOpenClosePriceMain.CloseMain[rec].ReservedField2 = BitConverter.ToChar(receiveBytes, offset); offset += BYTEOFFSET1;

                    // offset += BYTEOFFSET1;
                    if (BroadcastMasterMemory.objScripDetailsConDict.ContainsKey(oOpenClosePriceMain.CloseMain[rec].InstrumentCode))
                    {
                        if (oOpenClosePriceMain.MessType == CLOSPRICEBCAST)
                        {
                            BroadcastMasterMemory.objScripDetailsConDict[oOpenClosePriceMain.CloseMain[rec].InstrumentCode].CloseRate_l = oOpenClosePriceMain.CloseMain[rec].Price;

                        }
                        else
                        {
                            BroadcastMasterMemory.objScripDetailsConDict[oOpenClosePriceMain.CloseMain[rec].InstrumentCode].OpenRate_l = oOpenClosePriceMain.CloseMain[rec].Price;

                        }

                    }
                    else
                    {
                        ScripDetails s = new ScripDetails();
                        if (oOpenClosePriceMain.MessType == CLOSPRICEBCAST)
                        {
                            s.CloseRate_l = oOpenClosePriceMain.CloseMain[rec].Price;

                        }
                        else
                        {
                            s.OpenRate_l = oOpenClosePriceMain.CloseMain[rec].Price;

                        }
                        s.ScripCode_l = oOpenClosePriceMain.CloseMain[rec].InstrumentCode;
                        BroadcastMasterMemory.objScripDetailsConDict.TryAdd(oOpenClosePriceMain.CloseMain[rec].InstrumentCode, s);
                    }
                    if (SubscribeScripMemory.objMasterSubscribeScrip.ContainsKey(oOpenClosePriceMain.CloseMain[rec].InstrumentCode))
                    {
                        SubscribeScripMemory.SubscribeScripQueue.Enqueue(BroadcastMasterMemory.objScripDetailsConDict[oOpenClosePriceMain.CloseMain[rec].InstrumentCode]);
                    }
                 
                }



                //if (BroadcastMasterMemory.objScripDetailsConDict.ContainsKey(s.ScripCode_l))
                //{
                //    BroadcastMasterMemory.objScripDetailsConDict[s.ScripCode_l] = s;
                //}

            }
            catch(Exception ex)
            {

            }
        }

        private void UpdateIndexTicker(byte[] receiveBytes, int offset,int Segment)
        {
            Index oIndex = new Index();
            IndexMain oIndexMain = new IndexMain();
            //short HH_s, MM_s, SS_s, SSS_s;
            //short Session_s, NoOfRecs_s;
            //Char charRange = '\';
            offset = 0;

            oIndex.MessType = Swap32(BitConverter.ToInt32(receiveBytes, offset)); offset += BYTEOFFSET4;
            oIndex.ReservedF1 = Swap32(BitConverter.ToInt32(receiveBytes, offset)); offset += BYTEOFFSET4;
            oIndex.ReservedF2 = Swap32(BitConverter.ToInt32(receiveBytes, offset)); offset += BYTEOFFSET4;
            offset += BYTEOFFSET2;

            oIndex.Hour = Swap16(BitConverter.ToInt16(receiveBytes, offset)); offset += BYTEOFFSET2;
            oIndex.Minute = Swap16(BitConverter.ToInt16(receiveBytes, offset)); offset += BYTEOFFSET2;
            oIndex.Second = Swap16(BitConverter.ToInt16(receiveBytes, offset)); offset += BYTEOFFSET2;
            oIndex.Millisecond = Swap16(BitConverter.ToInt16(receiveBytes, offset)); offset += BYTEOFFSET2;
            oIndex.ReservedF4 = Swap16(BitConverter.ToInt16(receiveBytes, offset)); offset += BYTEOFFSET2;
            oIndex.ReservedF5 = Swap16(BitConverter.ToInt16(receiveBytes, offset)); offset += BYTEOFFSET2;
            oIndex.NoOfRec = Swap16(BitConverter.ToInt16(receiveBytes, offset)); offset += BYTEOFFSET2;

            oIndexMain.MessType = oIndex.MessType;
            oIndexMain.NoOfRec = oIndex.NoOfRec;

            if (oIndex.NoOfRec > NoOfRecords2012_2011)
            {
                oIndex.NoOfRec = NoOfRecords2012_2011;
            }

                for (short rec = 0; rec < oIndex.NoOfRec; rec++)
                {
                //IdicesDetails oIndex.ind[rec] = new IdicesDetails();
                //IdicesDetails oIdicesDetails = new IdicesDetails();
                oIndex.ind[rec].IndexCode = Swap32(BitConverter.ToInt32(receiveBytes, offset)); offset += BYTEOFFSET4;
                oIndex.ind[rec].IndexHigh = Swap32(BitConverter.ToInt32(receiveBytes, offset)); offset += BYTEOFFSET4;
                oIndex.ind[rec].IndexLow = Swap32(BitConverter.ToInt32(receiveBytes, offset)); offset += BYTEOFFSET4;
                oIndex.ind[rec].IndexOpen = Swap32(BitConverter.ToInt32(receiveBytes, offset)); offset += BYTEOFFSET4;
                oIndex.ind[rec].PreviousIndexClose = Swap32(BitConverter.ToInt32(receiveBytes, offset)); offset += BYTEOFFSET4;
                oIndex.ind[rec].IndexValue = Swap32(BitConverter.ToInt32(receiveBytes, offset)); offset += BYTEOFFSET4;
                oIndex.ind[rec].IndexId = Encoding.UTF8.GetString(receiveBytes, offset, BYTEOFFSET7);
                    offset += BYTEOFFSET7;
                    offset += BYTEOFFSET1;
                    offset += BYTEOFFSET1;
                    offset += BYTEOFFSET1;
                    offset += BYTEOFFSET2;
                oIndex.ind[rec].ReservedF10 = Swap16(BitConverter.ToInt16(receiveBytes, offset)); offset += BYTEOFFSET2;
                oIndex.ind[rec].ReservedF11 = Swap16(BitConverter.ToInt16(receiveBytes, offset)); offset += BYTEOFFSET2;

                //if (BroadcastMasterMemory.objIndicesDetailsConDict.ContainsKey(oIndex.ind[rec].IndexCode))
                //{
                //    BroadcastMasterMemory.objIndicesDetailsConDict[oIndex.ind[rec].IndexCode] = oIndex.ind[rec];
                //}
                //else
                //{
                //    BroadcastMasterMemory.objIndicesDetailsConDict.TryAdd(oIndex.ind[rec].IndexCode, oIndex.ind[rec]);
                //}

                //if (SubscribeIndicesMemory.objSubscribeIndex.ContainsKey(oIdicesDetails.IndexCode))
                //{
                //    SubscribeIndicesMemory.SubscribeIndiceQueue.Enqueue(oIdicesDetails);
                //}
                
                oIndexMain.indMain[rec].IndexCode = oIndex.ind[rec].IndexCode;
                oIndexMain.indMain[rec].IndexHigh = oIndex.ind[rec].IndexHigh;
                oIndexMain.indMain[rec].IndexLow = oIndex.ind[rec].IndexLow;
                oIndexMain.indMain[rec].IndexOpen = oIndex.ind[rec].IndexOpen;
                oIndexMain.indMain[rec].PreviousIndexClose = oIndex.ind[rec].PreviousIndexClose;
                oIndexMain.indMain[rec].IndexValue = oIndex.ind[rec].IndexValue;
                oIndexMain.indMain[rec].IndexId = oIndex.ind[rec].IndexId;
                //string[] seprator =a.Split('\');
                //oIndexMain.indMain[rec].IndexId = 
                //using (StreamWriter writer = new StreamWriter(@"C:\Users\tcs.prafulla\Desktop\Tws New\IML1.txt", false))
                //{
                //    foreach (ScipMaster line in objMastertxtDic.Values)
                //        writer.WriteLine(line.ScripCode + "|" + line.ExchangeCode + "|" + line.ScripId + "|" + line.ScripName + "|" +
                //            line.PartitionId + "|" + line.InstrumentType + "|" + line.GroupName + "|" + line.ScripType
                //            + "|" + line.FaceValue + "|" + line.MarketLot + "|" +
                //            line.TickSize + "|" + line.BseExclusive + "|" + line.Status + "|" + line.ExDivDate + "|" +
                //            line.NoDelEndDate + "|" + line.NoDelStartDate + "|" + line.NewTickSize
                //              + "|" + line.IsinCode + "|" + line.CallAuctionIndicator + "|" + line.BcStartDate + "|" +
                //            line.ExBonusDate + "|" + line.ExRightDate + "|" + line.Filler + "|" + line.Filler2
                //            + "|" + line.BcEndDate + "|" + line.Filler3 + "|"
                //            );
                //}
                if (SubscribeIndicesMemory.SubscribeIndiceDict.Keys.Contains(oIndex.ind[rec].IndexCode))
                {
                    SubscribeIndicesMemory.SubscribeIndiceDict[oIndex.ind[rec].IndexCode] = oIndexMain;
                }
                else {
                    SubscribeIndicesMemory.SubscribeIndiceDict.TryAdd(oIndex.ind[rec].IndexCode, oIndexMain);
                }
            }


            if (Segment == 0)
            {
                if (IndicesTick != null)
                {
                    IndicesTick();
                }
            }


        }

        private void MarketPictureBcast(byte[] receiveBytes, int offset,int Segment)
        {
            short HH_s, MM_s, SS_s, SSS_s;
            short Session_s, NoOfRecs_s;

            offset = 0;
            offset += BYTEOFFSET4;
            offset += BYTEOFFSET4;
            offset += BYTEOFFSET4;
            offset += BYTEOFFSET2;

            HH_s = Swap16(BitConverter.ToInt16(receiveBytes, offset)); offset += BYTEOFFSET2;
            MM_s = Swap16(BitConverter.ToInt16(receiveBytes, offset)); offset += BYTEOFFSET2;
            SS_s = Swap16(BitConverter.ToInt16(receiveBytes, offset)); offset += BYTEOFFSET2;
            SSS_s = Swap16(BitConverter.ToInt16(receiveBytes, offset)); offset += BYTEOFFSET2;

            offset += BYTEOFFSET2;
            Session_s = Swap16(BitConverter.ToInt16(receiveBytes, offset)); offset += BYTEOFFSET2;
            NoOfRecs_s = Swap16(BitConverter.ToInt16(receiveBytes, offset)); offset += BYTEOFFSET2;

            for (short rec = 0; rec < NoOfRecs_s; rec++)
            {
                ScripDetails s = new ScripDetails();

                s.ScripCode_l = Swap32(BitConverter.ToInt32(receiveBytes, offset)); offset += BYTEOFFSET4;

             //   if (s.ScripCode_l >= 999999)
              //      return;

                s.NoOfTrades_l = Swap32(BitConverter.ToInt32(receiveBytes, offset)); offset += BYTEOFFSET4;
                s.TradedVolume_l = Swap32(BitConverter.ToInt32(receiveBytes, offset)); offset += BYTEOFFSET4;
                s.TradedValue_l = Swap32(BitConverter.ToInt32(receiveBytes, offset)); offset += BYTEOFFSET4;

                s.Unit_c = Convert.ToChar(0 + receiveBytes[offset]);

                offset += BYTEOFFSET1;
                offset += BYTEOFFSET1;
                offset += BYTEOFFSET1;
                offset += BYTEOFFSET1;

                s.MarketType_s = Swap16(BitConverter.ToInt16(receiveBytes, offset)); offset += BYTEOFFSET2;
                s.SessionNo_s = Swap16(BitConverter.ToInt16(receiveBytes, offset)); offset += BYTEOFFSET2;

                s.LTP_HH_s = receiveBytes[offset]; offset += BYTEOFFSET1;
                s.LTP_MM_s = receiveBytes[offset]; offset += BYTEOFFSET1;
                s.LTP_SS_s = receiveBytes[offset]; offset += BYTEOFFSET1;

                //  LTP_SSS_s   Milli sec
                offset += BYTEOFFSET1;
                offset += BYTEOFFSET1;
                offset += BYTEOFFSET1;

                offset += BYTEOFFSET1;
                offset += BYTEOFFSET1;

                offset += BYTEOFFSET2; // Not Processsed    Filler_s1;
                offset += BYTEOFFSET2; // Not Processsed    No_of_PricePoints_s;
                offset += BYTEOFFSET8; // Not Processsed    ReserveField_ll;

                s.CloseRate_l = Swap32(BitConverter.ToInt32(receiveBytes, offset)); offset += BYTEOFFSET4;
                s.LastTradeQty_l = Swap32(BitConverter.ToInt32(receiveBytes, offset)); offset += BYTEOFFSET4;
                s.LastTradeRate_l = Swap32(BitConverter.ToInt32(receiveBytes, offset)); offset += BYTEOFFSET4;


                s.OpenRate_l = DecryptInt32(s.LastTradeRate_l, receiveBytes, ref offset);
                s.PrevClosePrice_l = DecryptInt32(s.LastTradeRate_l, receiveBytes, ref offset);
                s.HighRate_l = DecryptInt32(s.LastTradeRate_l, receiveBytes, ref offset);
                s.LowRate_l = DecryptInt32(s.LastTradeRate_l, receiveBytes, ref offset);
                s.BRP = DecryptInt32(s.LastTradeRate_l, receiveBytes, ref offset);

                s.IndicativeEqPrice_l = DecryptInt32(s.LastTradeRate_l, receiveBytes, ref offset);
                s.IndicativeEqQuantity_l = DecryptInt32(s.LastTradeRate_l, receiveBytes, ref offset);

                s.TotBuyQty_l = DecryptInt32(s.LastTradeQty_l, receiveBytes, ref offset);
                s.TotSellQty_l = DecryptInt32(s.LastTradeQty_l, receiveBytes, ref offset);
                s.LowerCktLmt_l = DecryptInt32(s.LastTradeRate_l, receiveBytes, ref offset);
                s.UpperCktLmt_l = DecryptInt32(s.LastTradeRate_l, receiveBytes, ref offset);
                s.WtAvgRate_l = DecryptInt32(s.LastTradeRate_l, receiveBytes, ref offset);
                s.Segment = Segment;
                s.HasBroadcastCome = true;
                for (int i = 0; i < 5; i++)
                {
                    short conv = Swap16(BitConverter.ToInt16(receiveBytes, offset));
                    if (conv != 32766)
                    {
                        if (i == 0)
                        {
                            s.Det[i].BuyRate_l = DecryptInt32(s.LastTradeRate_l, receiveBytes, ref offset);
                            s.Det[i].BuyQty_l = DecryptInt32(s.LastTradeQty_l, receiveBytes, ref offset);
                            s.Det[i].NoOfBidBuy_l = DecryptInt32(s.LastTradeQty_l, receiveBytes, ref offset);
                            s.Det[i].FillerBuy_l = DecryptInt32(s.LastTradeQty_l, receiveBytes, ref offset);
                        }
                        else
                        {
                            s.Det[i].BuyRate_l = DecryptInt32(s.Det[i - 1].BuyRate_l, receiveBytes, ref offset);
                            s.Det[i].BuyQty_l = DecryptInt32(s.Det[i - 1].BuyQty_l, receiveBytes, ref offset);
                            s.Det[i].NoOfBidBuy_l = DecryptInt32(s.Det[i - 1].NoOfBidBuy_l, receiveBytes, ref offset);
                            s.Det[i].FillerBuy_l = DecryptInt32(s.Det[i - 1].FillerBuy_l, receiveBytes, ref offset);
                        }
                    }
                    else
                    {
                        offset += BYTEOFFSET2;
                        //for (int j = i; j < 5; j++)
                        //{
                        //    s.Det[j].BuyRate_l = 0; s.Det[j].BuyQty_l = 0;
                        //    s.Det[j].NoOfBidBuy_l = 0; s.Det[j].FillerBuy_l = 0;
                        //}
                        break;
                    }
                }

                for (int i = 0; i < 5; i++)
                {
                    short conv;
                    conv = Swap16(BitConverter.ToInt16(receiveBytes, offset));
                    if (conv != -32766)
                    {
                        if (i == 0)
                        {
                            s.Det[i].SellRate_l = DecryptInt32(s.LastTradeRate_l, receiveBytes, ref offset);
                            s.Det[i].SellQty_l = DecryptInt32(s.LastTradeQty_l, receiveBytes, ref offset);
                            s.Det[i].NoOfBidSell_l = DecryptInt32(s.LastTradeQty_l, receiveBytes, ref offset);
                            s.Det[i].FillerSell_l = DecryptInt32(s.LastTradeQty_l, receiveBytes, ref offset);
                        }
                        else
                        {
                            s.Det[i].SellRate_l = DecryptInt32(s.Det[i - 1].SellRate_l, receiveBytes, ref offset);
                            s.Det[i].SellQty_l = DecryptInt32(s.Det[i - 1].SellQty_l, receiveBytes, ref offset);
                            s.Det[i].NoOfBidSell_l = DecryptInt32(s.Det[i - 1].NoOfBidSell_l, receiveBytes, ref offset);
                            s.Det[i].FillerSell_l = DecryptInt32(s.Det[i - 1].FillerSell_l, receiveBytes, ref offset);
                        }
                    }
                    else
                    {
                        offset += BYTEOFFSET2;
                        //for (int j = i; j < 5; j++)
                        //{
                        //    s.Det[j].SellRate_l = 0; s.Det[j].SellQty_l = 0;
                        //    s.Det[j].NoOfBidSell_l = 0; s.Det[j].FillerSell_l = 0;
                        //}
                        break;
                    }
                }
                if (BroadcastMasterMemory.objScripDetailsConDict.ContainsKey(s.ScripCode_l))
                {
                    BroadcastMasterMemory.objScripDetailsConDict[s.ScripCode_l] = s;
                }
                else
                {
                    BroadcastMasterMemory.objScripDetailsConDict.TryAdd(s.ScripCode_l, s);
                }

                if (SubscribeScripMemory.objMasterSubscribeScrip.ContainsKey(s.ScripCode_l))
                {
                    SubscribeScripMemory.SubscribeScripQueue.Enqueue(s);
                }

            }
        }

        public BroadcastListener()
        {
            this.listening = false;

            //BroadCastIp = IPAddress.Parse("226.1.0.1");
            //BroadCastPort = 12401;

            this.BroadCastIp = IPAddress.Parse("227.0.0.21");
            this.BroadCastPort = 12996;
            var host = Dns.GetHostEntry(Dns.GetHostName());
            this.InterFaceIp = host.AddressList[1];

            UdpListner = new UdpClient();
        }

        public BroadcastListener(string BcastIp, int BcastPort, string InterFaceIp)
        {
            this.listening = false;

            this.BroadCastIp = IPAddress.Parse(BcastIp);
            this.BroadCastPort = BcastPort;
            this.InterFaceIp = IPAddress.Parse(InterFaceIp);

            UdpListner = new UdpClient();
        }

        public BroadcastListener(string BcastIp, int BcastPort, long UserId, string LoginKey, string SockCompressionType, int TimeOut)
        {
            this.UserId = UserId;
            this.LoginKey = LoginKey;
            this.SockCompressionType = SockCompressionType;
            this.TimeOut = TimeOut;

            this.TCPConnectMessage = "CONNECT" + "|" + this.UserId + "|" + this.LoginKey + "|TCP|" + this.SockCompressionType + "|||Y";

            this.BroadCastIp = IPAddress.Parse(BcastIp);
            this.BroadCastPort = BcastPort;
        }

        ~BroadcastListener()
        {
            StopListener();
        }

        public void OpenCloseMBP(string lstrMessage)
        {
            this.TCPMBPMessage = lstrMessage;
            Send(TCPMBPMessage);
        }


        public void StartListener(short SockType,int Segment)
        {
            switch(SockType)
            {
                case 1:
                    {
                        if (!this.listening)
                        {
                            _timerData = new System.Timers.Timer(1);
                            _timerData.AutoReset = true;
                            _timerData.Enabled = true;

                            m_ListeningThread = new Thread(() =>ListenForUDPPackages(Segment));
                            this.listening = true;
                            m_ListeningThread.Start();
                        }
                    }
                    break;

                case 0:
                    {
                        if (!this.listening)
                        {
                            _timerData = new System.Timers.Timer(1);
                            _timerData.AutoReset = true;
                            _timerData.Enabled = true;

                            m_ListeningThread = new Thread(ListenForTCPPackages);
                            this.listening = true;
                            m_ListeningThread.Start();
                        }
                    }
                    break;
            }
        }

        public void PauseListener(short StartThreadKey)
        {
            if (StartThreadKey == 1)
            {
                _pauseEvent.Reset();
            }
            else if (StartThreadKey == 2)
            {
                _pauseEvent.Set();
            }
        }

        public void StopListener()
        {
            _pauseEvent.Set();
            this.listening = false;

            _timerData.Enabled = false;
            _timerData.Stop();
            _timerData.Close();

            m_ListeningThread.Join();
        }

        public void ListenForUDPPackages(int  Segment)
        {
            try
            {
                UdpListner.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                UdpListner.JoinMulticastGroup(BroadCastIp);
                UdpListner.Client.Bind(new IPEndPoint(InterFaceIp, BroadCastPort));
            }
            catch (SocketException ec)
            {
                //do nothing
            }

            if (UdpListner != null)
            {
                IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, 0);

                ReceivedDataEvent += new BcastEventHandler(ReceivedInBytesMethod);

                try
                {
                    while (this.listening)
                    {
                        _pauseEvent.WaitOne(Timeout.Infinite);

                        byte[] bytes = UdpListner.Receive(ref groupEP);

                        ReceivedInBytesMethod(bytes,"" ,Segment);

                        Datacounter++;

                        // int msg = Swap32(BitConverter.ToInt32(bytes, 0));

                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                finally
                {
                    //UdpListner.Close();
                }
            }
        }

        public void Send(string pMessage)
        {
            if (pMessage == null || pMessage.Trim().Length == 0)
            {
                pMessage = "\n";
            }
            else
            {
                pMessage = pMessage.Trim();
            }

            if (tcpClient != null)
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
        }

        public void ListenForTCPPackages()
        {
            try
            {
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
                tcpClient = new TcpClient();
                tcpClient.ReceiveTimeout = TimeOut;
                tcpClient.SendTimeout = TimeOut;
                tcpClient.NoDelay = true;
                tcpClient.LingerState = new System.Net.Sockets.LingerOption(false, 0);

                tcpClient.Connect(BroadCastIp, BroadCastPort);
                tcpStream = tcpClient.GetStream();

                //if (!TCPConnectMessage.Substring(TCPConnectMessage.Length - 1).Equals("\n"))
                //    TCPConnectMessage = TCPConnectMessage + "\n";

                Send(TCPConnectMessage);
                

                this.listening = true;
                // send(TCPClientData);
            }
            catch (SocketException)
            {
                //do nothing
            }

            if (tcpClient != null)
            {
                ReceivedDataEvent += new BcastEventHandler(ReceivedInBytesMethod);

                try
                {
                    int mintPoolFrequency = 10;
                    System.DateTime lLastPacketSendOrReceiveTime;
                    lLastPacketSendOrReceiveTime = System.DateTime.Now;


                    while (this.listening)
                    {
                        _pauseEvent.WaitOne(Timeout.Infinite);

                        byte[] bytes = new byte[80000];
                        int lNumberOfBytesReceived = 0;

                        if (tcpStream != null && tcpStream.DataAvailable == false)
                        {
                            if ((System.DateTime.Now.Ticks - lLastPacketSendOrReceiveTime.Ticks) > (mintPoolFrequency * 10000000))
                            {
                               // Send(TCPMBPMessage);
                               // "OPEN|634|1957834255-02154100151152|546|"

                                Send("\n");

                                lLastPacketSendOrReceiveTime = System.DateTime.Now;
                            }
                            Thread.Sleep(10);
                        }
                        else if (tcpStream != null && tcpStream.DataAvailable == true)
                        {
                            lNumberOfBytesReceived = tcpStream.Read(bytes, 0, bytes.Length);

                            string mTCPMessage = "";
                            mTCPMessage += Encoding.ASCII.GetString(bytes, 0, lNumberOfBytesReceived);
                            mTCPMessage = mTCPMessage.Replace("\n", "/z");

                            ReceivedInBytesMethod(bytes, lNumberOfBytesReceived);

                            Datacounter++;
                        }

                        if(lNumberOfBytesReceived > 0)
                            lLastPacketSendOrReceiveTime = System.DateTime.Now;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                finally
                {
                    //UdpListner.Close();
                }
            }
        }

        void ReceivedInBytesMethod(byte[] receiveBytes,string empty,int Segment)
        {
            int offset = 0, MsgType = 0;
            if (receiveBytes == null)
                return;

            MsgType = Swap32(BitConverter.ToInt32(receiveBytes, offset));

            switch (MsgType)
            {
                case MKTPIC_EQT:
                     MarketPictureBcast(receiveBytes, offset,Segment);
                    break;

                case SENSEXBCAST: 
				case ALLINDEXBCAST:
                    UpdateIndexTicker(receiveBytes, offset, Segment);
                    break;

                case OPENPRICEBCAST:
                case CLOSPRICEBCAST:                                   
                   UpdateClosePrice(receiveBytes, offset);
                    break;

                case OIRBCAST:

                    UpdateOIPrice(receiveBytes, offset, Segment);
                   
                   
                    break;

                case RBIBCAST:
                    //UpdateRBI(receiveBytes, offset);
                    break;

                case VARBCAST:
                    UpdateVarPrice(receiveBytes, offset, Segment);
                    break;

                case NEWSBCAST:
                    UpdateNewsPrice(receiveBytes, offset, Segment);
                    break;

                case SESSIONBCAST:
                case AUCTIONSESSION:
                    UpdateSessionBcast(receiveBytes, offset, Segment);
                    break;
            }
        }

        void ReceivedInBytesMethod(byte[] receiveBytes, int NumOfBytes)
        {
            int offset = 0, MsgType = 0;
            if (receiveBytes == null)
                return;

            string mTCPMessage = "";
            mTCPMessage += Encoding.ASCII.GetString(receiveBytes, 0, NumOfBytes);
            mTCPMessage = mTCPMessage.Replace("\n", "/z");

            string lstrSocketData = "";
            lstrSocketData = mTCPMessage;

            int lintFoundAt = 0;
            lintFoundAt = lstrSocketData.IndexOf("/z");

            string lstrOneCompleteData = "";
            while (lintFoundAt >= 0)
            {
                lstrOneCompleteData = lstrSocketData.Substring(0, lintFoundAt);

                if (lstrOneCompleteData.Trim().Length > 0)
                {
                    char[] lDelimiter;
                    string[] lRecords;
                    int lCount;
                    lDelimiter = new char[1];
                    lDelimiter[0] = '~';// RECORD_SEPARATOR;
                                        //First split the records using the RECORD_SEPARATOR
                    lRecords = lstrOneCompleteData.Split(lDelimiter);
                    lDelimiter[0] = '|';// FIELD_SEPARATOR;
                                        //The length of the lRecords array will be the number of records in the data.
                                        //Allocate String Arrays to accomodate all the records.
                    int lreclenght = lRecords.Length;

                    string[][] records;
                    records = new string[lreclenght][];

                    Parallel.For(0, lreclenght, (i) =>
                    {
                        records[i] = lRecords[i].Split(lDelimiter);
                    });

                    switch(records[0][0])
                    {
                        case "60":
                            {
                                try
                                {
                                    ScripDetails s = new ScripDetails();

                                    if (!string.IsNullOrEmpty(records[0][8]))
                                        s.ScripCode_l = Convert.ToInt32(records[0][8]);

                                    if (s.ScripCode_l >= 999999)
                                        break;

                                    if (!string.IsNullOrEmpty(records[0][13])) s.TradedVolume_l = Convert.ToInt32(records[0][13]);
                                    if (!string.IsNullOrEmpty(records[0][14])) s.LastTradeRate_l = (int)(Convert.ToDecimal(records[0][14]) * 100);
                                    if (!string.IsNullOrEmpty(records[0][18])) s.LastTradeQty_l = Convert.ToInt32(records[0][18]);
                                    if (!string.IsNullOrEmpty(records[0][20])) s.WtAvgRate_l = (int)(Convert.ToDecimal(records[0][20]) * 100);
                                    if (!string.IsNullOrEmpty(records[0][51])) s.TotBuyQty_l = Convert.ToInt32(records[0][51]);
                                    if (!string.IsNullOrEmpty(records[0][52])) s.TotSellQty_l = Convert.ToInt32(records[0][52]);
                                    if (!string.IsNullOrEmpty(records[0][53])) s.CloseRate_l = (int)(Convert.ToDecimal(records[0][53]) * 100);
                                    if (!string.IsNullOrEmpty(records[0][54])) s.OpenRate_l = (int)(Convert.ToDecimal(records[0][54]) * 100);
                                    if (!string.IsNullOrEmpty(records[0][55])) s.HighRate_l = (int)(Convert.ToDecimal(records[0][55]) * 100);
                                    if (!string.IsNullOrEmpty(records[0][56])) s.LowRate_l = (int)(Convert.ToDecimal(records[0][56]) * 100);
                                    if (!string.IsNullOrEmpty(records[0][59])) s.TradedValue_l = (int)(Convert.ToDecimal(records[0][59]) * 100);
                                    if (!string.IsNullOrEmpty(records[0][60])) s.NoOfTrades_l = Convert.ToInt32(records[0][60]);
                                    if (!string.IsNullOrEmpty(records[0][61])) s.UpperCktLmt_l = (int)(Convert.ToDecimal(records[0][61]) * 100);
                                    if (!string.IsNullOrEmpty(records[0][62])) s.LowerCktLmt_l = (int)(Convert.ToDecimal(records[0][62]) * 100);

                                    if (!string.IsNullOrEmpty(records[0][21])) s.Det[0].BuyQty_l = Convert.ToInt32(records[0][21]);
                                    if (!string.IsNullOrEmpty(records[0][22])) s.Det[0].BuyRate_l = (int)(Convert.ToDecimal(records[0][22]) * 100);
                                    if (!string.IsNullOrEmpty(records[0][23])) s.Det[0].NoOfBidBuy_l = Convert.ToInt32(records[0][23]);

                                    if (!string.IsNullOrEmpty(records[0][24])) s.Det[1].BuyQty_l = Convert.ToInt32(records[0][24]);
                                    if (!string.IsNullOrEmpty(records[0][25])) s.Det[1].BuyRate_l = (int)(Convert.ToDecimal(records[0][25]) * 100);
                                    if (!string.IsNullOrEmpty(records[0][26])) s.Det[1].NoOfBidBuy_l = Convert.ToInt32(records[0][26]);

                                    if (!string.IsNullOrEmpty(records[0][27])) s.Det[2].BuyQty_l = Convert.ToInt32(records[0][27]);
                                    if (!string.IsNullOrEmpty(records[0][28])) s.Det[2].BuyRate_l = (int)(Convert.ToDecimal(records[0][28]) * 100);
                                    if (!string.IsNullOrEmpty(records[0][29])) s.Det[2].NoOfBidBuy_l = Convert.ToInt32(records[0][29]);

                                    if (!string.IsNullOrEmpty(records[0][30])) s.Det[3].BuyQty_l = Convert.ToInt32(records[0][30]);
                                    if (!string.IsNullOrEmpty(records[0][31])) s.Det[3].BuyRate_l = (int)(Convert.ToDecimal(records[0][31]) * 100);
                                    if (!string.IsNullOrEmpty(records[0][32])) s.Det[3].NoOfBidBuy_l = Convert.ToInt32(records[0][32]);

                                    if (!string.IsNullOrEmpty(records[0][33])) s.Det[4].BuyQty_l = Convert.ToInt32(records[0][33]);
                                    if (!string.IsNullOrEmpty(records[0][34])) s.Det[4].BuyRate_l = (int)(Convert.ToDecimal(records[0][34]) * 100);
                                    if (!string.IsNullOrEmpty(records[0][35])) s.Det[4].NoOfBidBuy_l = Convert.ToInt32(records[0][35]);

                                    
                                    if (!string.IsNullOrEmpty(records[0][36])) s.Det[0].SellQty_l = Convert.ToInt32(records[0][36]);
                                    if (!string.IsNullOrEmpty(records[0][37])) s.Det[0].SellRate_l = (int)(Convert.ToDecimal(records[0][37]) * 100);
                                    if (!string.IsNullOrEmpty(records[0][38])) s.Det[0].NoOfBidSell_l = Convert.ToInt32(records[0][38]);

                                    if (!string.IsNullOrEmpty(records[0][39])) s.Det[1].SellQty_l = Convert.ToInt32(records[0][39]);
                                    if (!string.IsNullOrEmpty(records[0][40])) s.Det[1].SellRate_l = (int)(Convert.ToDecimal(records[0][40]) * 100);
                                    if (!string.IsNullOrEmpty(records[0][41])) s.Det[1].NoOfBidSell_l = Convert.ToInt32(records[0][41]);

                                    if (!string.IsNullOrEmpty(records[0][42])) s.Det[2].SellQty_l = Convert.ToInt32(records[0][42]);
                                    if (!string.IsNullOrEmpty(records[0][43])) s.Det[2].SellRate_l = (int)(Convert.ToDecimal(records[0][43]) * 100);
                                    if (!string.IsNullOrEmpty(records[0][44])) s.Det[2].NoOfBidSell_l = Convert.ToInt32(records[0][44]);

                                    if (!string.IsNullOrEmpty(records[0][45])) s.Det[3].SellQty_l = Convert.ToInt32(records[0][45]);
                                    if (!string.IsNullOrEmpty(records[0][46])) s.Det[3].SellRate_l = (int)(Convert.ToDecimal(records[0][46]) * 100);
                                    if (!string.IsNullOrEmpty(records[0][47])) s.Det[3].NoOfBidSell_l = Convert.ToInt32(records[0][47]);

                                    if (!string.IsNullOrEmpty(records[0][48])) s.Det[4].SellQty_l = Convert.ToInt32(records[0][48]);
                                    if (!string.IsNullOrEmpty(records[0][49])) s.Det[4].SellRate_l = (int)(Convert.ToDecimal(records[0][49]) * 100);
                                    if (!string.IsNullOrEmpty(records[0][50])) s.Det[4].NoOfBidSell_l = Convert.ToInt32(records[0][50]);
                                    
                                    //s.LTP_HH_s = 
                                    //s.LTP_MM_s = 
                                    //s.LTP_SS_s =  
                                    //s.MarketType_s = 
                                    //s.SessionNo_s =                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     
                                    //s.ClosePrice_l = 
                                    //s.ReserveField_l = 
                                    //s.IndicativeEqPrice_l = 
                                    //s.IndicativeEqQuantity_l = 
                                    //s.Unit_c = 

                                    if (BroadcastMasterMemory.objScripDetailsConDict.ContainsKey(s.ScripCode_l))
                                    {
                                        BroadcastMasterMemory.objScripDetailsConDict[s.ScripCode_l] = s;
                                    }
                                    else
                                    {
                                        BroadcastMasterMemory.objScripDetailsConDict.TryAdd(s.ScripCode_l, s);
                                    }

                                    if (SubscribeScripMemory.objMasterSubscribeScrip.ContainsKey(s.ScripCode_l))
                                    {
                                        SubscribeScripMemory.SubscribeScripQueue.Enqueue(s);
                                    }
                                }
                                catch (Exception e)
                                {
                                    throw e;
                                }
                            }
                            Datacounter60++;
                            break;
                        case "61":
                            {
                                try
                                {
                                    ScripDetails s = new ScripDetails();

                                    if (!string.IsNullOrEmpty(records[0][6]))
                                        s.ScripCode_l = Convert.ToInt32(records[0][6]);

                                    if (s.ScripCode_l >= 999999)
                                        break;

                                    if (!string.IsNullOrEmpty(records[0][7])) s.TradedVolume_l = Convert.ToInt32(records[0][7]);
                                    if (!string.IsNullOrEmpty(records[0][8])) s.LastTradeRate_l = (int)(Convert.ToDecimal(records[0][8]) * 100);
                                    if (!string.IsNullOrEmpty(records[0][12])) s.LastTradeQty_l = Convert.ToInt32(records[0][12]);
                                    if (!string.IsNullOrEmpty(records[0][14])) s.WtAvgRate_l = (int)(Convert.ToDecimal(records[0][14]) * 100);
                                    if (!string.IsNullOrEmpty(records[0][17])) s.TotBuyQty_l = Convert.ToInt32(records[0][17]);
                                    if (!string.IsNullOrEmpty(records[0][20])) s.TotSellQty_l = Convert.ToInt32(records[0][20]);
                                    if (!string.IsNullOrEmpty(records[0][21])) s.CloseRate_l = (int)(Convert.ToDecimal(records[0][21]) * 100);
                                    if (!string.IsNullOrEmpty(records[0][22])) s.OpenRate_l = (int)(Convert.ToDecimal(records[0][22]) * 100);
                                    if (!string.IsNullOrEmpty(records[0][23])) s.HighRate_l = (int)(Convert.ToDecimal(records[0][23]) * 100);
                                    if (!string.IsNullOrEmpty(records[0][24])) s.LowRate_l = (int)(Convert.ToDecimal(records[0][24]) * 100);
                                    if (!string.IsNullOrEmpty(records[0][27])) s.TradedValue_l = (int)(Convert.ToDecimal(records[0][27]) * 100);
                                    if (!string.IsNullOrEmpty(records[0][28])) s.NoOfTrades_l = Convert.ToInt32(records[0][28]);
                                    if (!string.IsNullOrEmpty(records[0][29])) s.UpperCktLmt_l = (int)(Convert.ToDecimal(records[0][29]) * 100);
                                    if (!string.IsNullOrEmpty(records[0][30])) s.LowerCktLmt_l = (int)(Convert.ToDecimal(records[0][30]) * 100);

                                    if (!string.IsNullOrEmpty(records[0][15])) s.Det[0].BuyQty_l = Convert.ToInt32(records[0][15]);
                                    if (!string.IsNullOrEmpty(records[0][16])) s.Det[0].BuyRate_l = (int)(Convert.ToDecimal(records[0][16]) * 100);
                                    if (!string.IsNullOrEmpty(records[0][18])) s.Det[0].SellQty_l = Convert.ToInt32(records[0][18]);
                                    if (!string.IsNullOrEmpty(records[0][19])) s.Det[0].SellRate_l = (int)(Convert.ToDecimal(records[0][19]) * 100);

                                    //s.LTP_HH_s = 
                                    //s.LTP_MM_s = 
                                    //s.LTP_SS_s =  
                                    //s.MarketType_s = 
                                    //s.SessionNo_s =                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     
                                    //s.ClosePrice_l = 
                                    //s.ReserveField_l = 
                                    //s.IndicativeEqPrice_l = 
                                    //s.IndicativeEqQuantity_l = 
                                    //s.Unit_c = 

                                    if (BroadcastMasterMemory.objScripDetailsConDict.ContainsKey(s.ScripCode_l))
                                    {
                                        BroadcastMasterMemory.objScripDetailsConDict[s.ScripCode_l] = s;
                                    }
                                    else
                                    {
                                        BroadcastMasterMemory.objScripDetailsConDict.TryAdd(s.ScripCode_l, s);
                                    }

                                    if (SubscribeScripMemory.objMasterSubscribeScrip.ContainsKey(s.ScripCode_l))
                                    {
                                        SubscribeScripMemory.SubscribeScripQueue.Enqueue(s);
                                    }
                                }
                                catch(Exception e)
                                {
                                    throw e;
                                }
                            }
                            Datacounter61++;
                            break;
                        case "69":
                            Datacounter69++;
                            break;
                        case "72":
                            IdicesDetailsMain oIdicesDetailsMain = new IdicesDetailsMain();


                            //Datacounter72++;
                            break;

                    }
                }
                lstrSocketData = lstrSocketData.Substring(lintFoundAt + 2);
                lintFoundAt = lstrSocketData.IndexOf("/z");
            }

                


        }
    }

    public class BestFive
    {
        public int BuyRate_l;
        public int BuyQty_l;
        public int NoOfBidBuy_l;
        public int FillerBuy_l;

        public int SellRate_l;
        public int SellQty_l;
        public int NoOfBidSell_l;
        public int FillerSell_l;
    }

    public class ScripDetails
    {
        public int ScripCode_l;
        public int LastTradeRate_l;
        public int OpenRate_l;
        public int CloseRate_l;
        public int HighRate_l;
        public int LowRate_l;
        public int TotBuyQty_l;
        public int TotSellQty_l;
        public int WtAvgRate_l;

        public int LastTradeQty_l;
        public int TradedVolume_l;
        public int TradedValue_l;
        public int LowerCktLmt_l;
        public int UpperCktLmt_l;
        public int NoOfTrades_l;
        public int IndicativeEqPrice_l;
        public int IndicativeEqQuantity_l;
        public long OI;
        public long ChangeInOI;
        public long OIValue;

        public BestFive[] Det = new BestFive[5];

        public int PrevClosePrice_l;
        public int BRP;

        public short MarketType_s;
        public short SessionNo_s;

        public short LTP_HH_s;
        public short LTP_MM_s;
        public short LTP_SS_s;
        public short LTP_SSS_s;

        public char Unit_c;

        public int Segment;
        public bool HasBroadcastCome = false;
        public ScripDetails()
        {
            for (short i = 0; i < 5; i++)
            {
                Det[i] = new BestFive();
            }
        }
    }

    public struct BestFive_st
    {
        public Int32 BuyRateL;
        public Int32 BuyQtyL;
        public Int32 NoOfBidBuyL;
        public Int32 FillerBuyL;

        public Int32 SellRateL;
        public Int32 SellQtyL;
        public Int32 NoOfBidSellL;
        public Int32 FillerSellL;
    }
    // TWS structure memory
    public class ScripDetails2
    {
        public int ScriptCode = 0;
        public Int32 lastTradeRateL = 0;
        public Int32 openRateL = 0;
        public Int32 closeRateL = 0;
        public Int32 highRateL = 0;
        public Int32 lowRateL = 0;
        public Int32 totBuyQtyL = 0;
        public Int32 totSellQtyL = 0;
        public Int32 wtAvgRateL = 0;

        public Int32 lastTradeQtyL;
        public Int32 TrdVolume;
        public Int32 TrdValue;
        public Int32 LowerCtLmt;
        public Int32 UprCtLmt;
        public Int32 NoOfTrades;
        public Int32 IndicateEqPrice;
        public Int32 IndicateEqQty;

        public List<BestFive> listBestFive = new List<BestFive>(5);

        //buysellObj.Unit_c = Unit_c.ToString();
        //buysellObj.LastTradeTime;

        public int index = 0;
        public Int32 BuyRateL = 0;
        public Int32 BuyQtyL = 0;
        
        //  public string scripId = "";
        public Int32 NoOfBidBuyL = 0;
        public Int32 FillerBuyL = 0;
        public Int32 SellRateL = 0;
        public Int32 SellQtyL = 0;
        public Int32 NoOfBidSellL = 0;
        public Int32 FillerSellL = 0;
        
        //    public string startTime="";
        //    public string endTime="";

        public float ChangePercentage;
        
        public Int32 Yield;
        public Int32 OI;
        public DateTime FiftyTwoHighDate;
        public DateTime FiftyTwoLowDate;
        public float FiftyTwoHigh;
        public float FiftyTwoLow;
        public string LastTradeTime = "         ";
        public string Unit_c = " ";
    }

    public class SessionMain
    {
        public int MessType;
        public int ReservedF1;
        public int ReservedF2;
        public ushort ReservedF3;
        public short Hour;
        public short Minute;
        public short Second;
        public short Millisecond;
        public short ProductID;
        public short ReservedField4;
        public short Filler;
        public short MarketType;
        public short SessionNumber;
        public int ReservedField5;
        public char StartEndFlag;
        public char ReservedField6;
        public byte[] ReservedF7 = new byte[2];
    }

    public class NewsMain
    {
        public int MessType;
        public int ReservedF1;
        public int ReservedF2;
        public ushort ReservedF3;
        public short Hour;
        public short Minute;
        public short Second;
        public short Millisecond;
        public short ReservedF4;
        public short ReservedF5;
        public short ReservedF6;
        public short NewsCategory;
        public short ReservedF7;
        public int NewsID;
        public string NewsHeadline;
        public short Reserved8;
        public short ReservedF9;        
        public byte[] ReservedF10 = new byte[2];


    }

    public class Index
    {
        public int MessType;
        public int ReservedF1;
        public int ReservedF2;
        public ushort ReservedF3;
        public short Hour;
        public short Minute;
        public short Second;
        public short Millisecond;
        public short ReservedF4;
        public short ReservedF5;
        public short NoOfRec;
        public IdicesDetails[] ind = new IdicesDetails[24];

        public Index()
        {
            for (short i = 0; i < 24; i++)
                ind[i] = new IdicesDetails();
        }
    }

    public class ClosePriceMain
    {
        public int MessType;
        public int ReservedF1;
        public int ReservedF2;
        public ushort ReservedF3;
        public short Hour;
        public short Minute;
        public short Second;
        public short Millisecond;
        public short ReservedF4;
        public short ReservedF5;
        public short NoOfRec;
        public ClosePriceDetails[] CloseMain = new ClosePriceDetails[80];

        public ClosePriceMain()
        {
            for (short i = 0; i < 80; i++)
                CloseMain[i] = new ClosePriceDetails();
        }
    }

    public class VarMain
    {
        public int MessType;
        public int ReservedF1;
        public int ReservedF2;
        public ushort ReservedF3;
        public short Hour;
        public short Minute;
        public short Second;
        public short Millisecond;
        public short ReservedF4;
        public short ReservedF5;
        public short NoOfRec;

        public VarMainDetails[] VarMainDetailsObj = new VarMainDetails[40];

        public VarMain()
        {
            for (short i = 0; i < 40; i++)
                VarMainDetailsObj[i] = new VarMainDetails();
        }
    }

    public class VarMainDetails
    {
        public int InstrumentCode;
        public int IMPercentage;
        public int ELMPercentage;
        public int ReservedField1;
        public short ReservedField2;
        public short ReservedField3;
        public short ReservedField4;
        public char Identifier;
        public short ReservedField5;

    }

    public class RBIMain
    {
        public int MessType;
        public int ReservedF1;
        public int ReservedF2;
        public ushort ReservedF3;
        public short Hour;
        public short Minute;
        public short Second;
        public short Millisecond;
        public short ReservedF4;
        public short ReservedF5;
        public short NoOfRec;

        public RBIMainDetails[] RBIMainDetailsObj = new RBIMainDetails[12];

        public RBIMain()
        {
            for (short i = 0; i < 12; i++)
                RBIMainDetailsObj[i] = new RBIMainDetails();
        }
    }

    public class OIMain
    {
        public int MessType;
        public int ReservedF1;
        public int ReservedF2;
        public ushort ReservedF3;
        public short Hour;
        public short Minute;
        public short Second;
        public short Millisecond;
        public short ReservedF4;
        public short ReservedF5;
        public short NoOfRec;
        public int Segment;

        public OIMainDetails[] ObjOIMainDetails = new OIMainDetails[26];

        public OIMain()
        {
            for (short i = 0; i < 26; i++)
                ObjOIMainDetails[i] = new OIMainDetails();
        }
    }

    public class RBIMainDetails
    {
        public int UnderlyingAssetId;
        public int RBIRate;
        public short ReservedField1;
        public short ReservedField2;  
        public string Date ;
        public char Filler;
          
    }

    public class OIMainDetails
    {
        public int InstrumentID;
        public int OpenInterestQuantity;
        public long OpenInterestValue;
        public int OpenInterestChange;
        public long TotalTradedValue;
        public byte[] ReservedF1 = new byte[4];
        public int ReservedField2;
        public short ReservedField3;
        public short ReservedField4;
        public char ReservedField5;
        public char ReservedField6;
        public byte[] ReservedF7 = new byte[2];

    }

    public class ClosePriceDetails
    {
        public int InstrumentCode;
        public int Price;
        public char ReservedField1;
        public char Traded;
        public char PrecisionIndicator;
        public char ReservedField2;

    }

    public class IdicesDetails
    {
        public int IndexCode;
        public int IndexHigh;
        public int IndexLow;
        public int IndexOpen;
        public int PreviousIndexClose;
        public int IndexValue;
        public string IndexId;
        public char ReservedF6;
        public char ReservedF7;
        public char ReservedF8;
        public byte[] ReservedF9 = new byte[2];
        public short ReservedF10;
        public short ReservedF11;
    }

    public class IndexMain
    {
        public int MessType;
        public short NoOfRec;
        public IdicesDetailsMain[] indMain = new IdicesDetailsMain[24];

        public IndexMain()
        {
            for (short i = 0; i < 24; i++)
                indMain[i] = new IdicesDetailsMain();
        }
    }

    public class IdicesDetailsMain
    {
        public int IndexCode;
        public int IndexHigh;
        public int IndexLow;
        public int IndexOpen;
        public int PreviousIndexClose;
        public int IndexValue;
        public string IndexId;
    }

    public class IdicesDetailsMainBow
    {
        public int MessType;
        public int IndexCode;
        public int IndexHigh;
        public int IndexLow;
        public int IndexOpen;
        public int PreviousIndexClose;
        public int IndexValue;
        public string IndexId;
    }
}

namespace BroadcastMaster
{
    public static class BroadcastMasterMemory
    {
        public static ConcurrentDictionary<int, ScripDetails> objScripDetailsConDict = new ConcurrentDictionary<int, ScripDetails>();
        public static ConcurrentDictionary<int, IdicesDetails> objIndicesDetailsConDict = new ConcurrentDictionary<int, IdicesDetails>();
   
    }
}

namespace SubscribeList
{
    public class SubscribeScrip
    {
        public int ScripCode_l;
        public short UpdateFlag_s;
    }

    public class SubscribeIndex
    {

        public short UpdateFlag_s;
    }
    public static class SubscribeScripMemory
    {
        public static ConcurrentDictionary<int, SubscribeScrip> objMasterSubscribeScrip = new ConcurrentDictionary<int, SubscribeScrip>();    
        public static Queue<ScripDetails> SubscribeScripQueue = new Queue<ScripDetails>();
    }

    public static class SubscribeIndicesMemory
    {
        public static ConcurrentDictionary<int, IndexMain> SubscribeIndiceDict = new ConcurrentDictionary<int, IndexMain>();      
       // public static Queue<IndexMain> SubscribeIndiceQueue = new Queue<IndexMain>();
    }

    public static class VarMemory
    {
        public static ConcurrentDictionary<int, VarMain> SubscribeVarMemoryDict = new ConcurrentDictionary<int, VarMain>();
         //   public static Queue<VarMain> SubscribeVarMemoryQueue = new Queue<VarMain>();
    }


    //public static class RBIMemory
    //{
    //    public static ConcurrentDictionary<int, SubscribeIndex> objRBIMemoryIndex = new ConcurrentDictionary<int, SubscribeIndex>();
    //    public static Queue<RBIMain> SubscribeRBIMemoryQueue = new Queue<RBIMain>();
    //}


    public static class OIMemory
    {
        public static ConcurrentDictionary<int, OIMain> SubscribeOIMemoryDict = new ConcurrentDictionary<int, OIMain>();
      //  public static Queue<OIMain> SubscribeOIMemoryQueue = new Queue<OIMain>();
      

       
    }

    public static class NewsMemory
    {
        public static ConcurrentDictionary<int, NewsMain> SubscribeNewsMemoryDict = new ConcurrentDictionary<int, NewsMain>();
       // public static Queue<NewsMain> SubscribeNewsMemoryQueue = new Queue<NewsMain>();
    }

    public static class SessionMemory
    {
        public static ConcurrentDictionary<int, SessionMain> SubscribeSessionMemoryDict = new ConcurrentDictionary<int, SessionMain>();
        //public static Queue<SessionMain> SubscribeSessionMemoryQueue = new Queue<SessionMain>();
    }

}


