
using CommonFrontEnd.ViewModel.Order;
using System;
using CommonFrontEnd.Model;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CommonFrontEnd.Common;
using System.Collections.ObjectModel;
using System.Threading;
using System.ComponentModel;
using System.Globalization;
using CommonFrontEnd.Controller;
using SubscribeList;
using CommonFrontEnd.Constants;
using System.Collections;
using CommonFrontEnd.ViewModel.Touchline;
using static CommonFrontEnd.Common.Enumerations;
using CommonFrontEnd.Global;
using CommonFrontEnd.Processor.Order;
using CommonFrontEnd.Model.Order;
using BroadcastMaster;
using CommonFrontEnd.SharedMemories;
using CommonFrontEnd.View;
using CommonFrontEnd.Model.Trade;

namespace CommonFrontEnd.ViewModel
{
    public partial class BestFiveVM
    {

#if BOW
        public BestFiveVM()
        {
            OrderEntryUC_VM.OnScripIDOrCodeChange += UpdateValues;
        }
#endif
        static string ScripID = String.Empty;
        static string ScripName = String.Empty;
        private static string _TitleBestFive;
        static int ScripCode;
        static int DecimalPoint;
        public static Action<int> OnScripChangeuserControl;
        static int PreviousSessionNumber = -1;

        static int PreviousMarketType = -1;
        static string SessionType = "";
        static Enumerations.Segment Segment;
        static int HeaderScripCode;
        public static string TitleBestFive
        {
            get { return _TitleBestFive; }
            set
            {
                _TitleBestFive = value;
                NotifyStaticPropertyChanged("TitleBestFive");
            }
        }
        private static ObservableCollection<BestFiveModel> _otherDetails;
        public static ObservableCollection<BestFiveModel> OtherDetails
        {
            get { return _otherDetails; }
            set
            {
                _otherDetails = value;

            }
        }

        private static ObservableCollection<BestFiveModel> _BstFiveCollection;
        public static ObservableCollection<BestFiveModel> BstFiveCollection
        {
            get { return _BstFiveCollection; }
            set { _BstFiveCollection = value; }
        }

        private static int _TotalBuyQty;

        public static int TotalBuyQty
        {
            get { return _TotalBuyQty; }
            set
            {
                _TotalBuyQty = value;
                NotifyStaticPropertyChanged("TotalBuyQty");
            }
        }


        private static string _DeciVisiEq;

        public static string DeciVisiEq
        {
            get { return _DeciVisiEq; }
            set
            {
                _DeciVisiEq = value;
                NotifyStaticPropertyChanged("DeciVisiEq");
            }
        }

        private static string _DeciVisiDev;

        public static string DeciVisiDev
        {
            get { return _DeciVisiDev; }
            set
            {
                _DeciVisiDev = value;
                NotifyStaticPropertyChanged("DeciVisiDev");
            }
        }


        private static int _TotalSellQty;

        public static int TotalSellQty
        {
            get { return _TotalSellQty; }
            set
            {
                _TotalSellQty = value;
                NotifyStaticPropertyChanged("TotalSellQty");
            }
        }
        private static string _StringFormat;

        public static string StringFormat
        {
            get { return _StringFormat; }
            set
            {
                _StringFormat = value;
                NotifyStaticPropertyChanged("StringFormat");
            }
        }
        private static string _TitleTWSBestFiveWindow;

        public static string TitleTWSBestFiveWindow
        {
            get { return _TitleTWSBestFiveWindow; }
            set { _TitleTWSBestFiveWindow = value; NotifyStaticPropertyChanged("TitleTWSBestFiveWindow"); }
        }
        private static string _BuyPosition;

        public static string BuyPosition
        {
            get { return _BuyPosition; }
            set { _BuyPosition = value; NotifyStaticPropertyChanged("BuyPosition"); }
        }

        private static string _SellPosition;

        public static string SellPosition
        {
            get { return _SellPosition; }
            set { _SellPosition = value; NotifyStaticPropertyChanged("SellPosition"); }
        }

        private static string _AvgBuyPosition;

        public static string AvgBuyPosition
        {
            get { return _AvgBuyPosition; }
            set { _AvgBuyPosition = value; NotifyStaticPropertyChanged("AvgBuyPosition"); }
        }

        private static string _AvgSellPosition;

        public static string AvgSellPosition
        {
            get { return _AvgSellPosition; }
            set { _AvgSellPosition = value; NotifyStaticPropertyChanged("AvgSellPosition"); }
        }
        static SynchronizationContext uiContext = SynchronizationContext.Current;
        public static void UpdateTitle(int ScripCode)
        {
            //TODO TBD2017
            if (MasterSharedMemory.objMastertxtDictBaseBSE.ContainsKey(ScripCode))
            {
                ScripID = MasterSharedMemory.objMastertxtDictBaseBSE[ScripCode].ScripId;
                ScripName = MasterSharedMemory.objMastertxtDictBaseBSE[ScripCode].ScripName;
            }
            else if (MasterSharedMemory.objMasterDerivativeDictBaseBSE.ContainsKey(ScripCode))
            {
                ScripID = MasterSharedMemory.objMasterDerivativeDictBaseBSE[ScripCode].ScripId;
                ScripName = MasterSharedMemory.objMasterDerivativeDictBaseBSE[ScripCode].InstrumentName;
            }
            else if (MasterSharedMemory.objMasterCurrencyDictBaseBSE.ContainsKey(ScripCode))
            {
                ScripID = MasterSharedMemory.objMasterCurrencyDictBaseBSE[ScripCode].ScripId;
                ScripName = MasterSharedMemory.objMasterCurrencyDictBaseBSE[ScripCode].InstrumentName;
            }

            if (ScripCode != 0)
            {
                HeaderScripCode = ScripCode;
                TitleBestFive = "Details --" + ScripID + "                                                  " + "[" + ScripCode + ": " + ScripName + "]";
            }
            else
                TitleBestFive = string.Empty;
            //string.Format("Details --")
            TitleTWSBestFiveWindow = "BestFiveWindow     [" + ScripCode + ":" + ScripID + "]       " + SessionType;
        }

        public static void UpdateBestWindow(ScripDetails objScripDetails, bool SendData)
        {
            try
            {
                BstFiveCollection = new ObservableCollection<BestFiveModel>();
                OtherDetails = new ObservableCollection<BestFiveModel>();
                if (objScripDetails != null)
                {
                    int sessionNumber = -1;
                    int MarketType = -1;
                    ScripCode = objScripDetails.ScriptCode_BseToken_NseToken;
                    ScripID = objScripDetails.ScripID;
                    int SegmentCode = CommonFunctions.GetSegmentFromScripCode(ScripCode);
                    if (SegmentCode == 1)
                    {
                        Segment = Enumerations.Segment.Equity;
                        DecimalPoint = CommonFunctions.GetDecimal(ScripCode, Enumerations.Exchange.BSE, Enumerations.Segment.Equity);
                    }
                    if (SegmentCode == 2)
                    {
                        Segment = Enumerations.Segment.Derivative;
                        DecimalPoint = CommonFunctions.GetDecimal(ScripCode, Enumerations.Exchange.BSE, Enumerations.Segment.Derivative);

                    }
                    if (SegmentCode == 3)
                    {
                        Segment = Enumerations.Segment.Currency;
                        DecimalPoint = CommonFunctions.GetDecimal(ScripCode, Enumerations.Exchange.BSE, Enumerations.Segment.Currency);
                    }



                    ScripID = CommonFunctions.GetScripId(ScripCode, Enumerations.Exchange.BSE, Segment);
                    OtherDetails = new ObservableCollection<BestFiveModel>();
                    BstFiveCollection = new ObservableCollection<BestFiveModel>();
                    BestFiveModel bstfivedata = new BestFiveModel();

                    if (DecimalPoint == 4)
                    {

                        StringFormat = ".0000;.0000;#";
                    }
                    else if (DecimalPoint == 2)
                    {

                        StringFormat = ".00;.00;#";
                    }


                    //Assign Other Details : Apoorva Sharma
                    bstfivedata.NoOfTrds = objScripDetails.NoOfTrades;

                    if (objScripDetails.NoOfTrades > 0)
                    {

                        bstfivedata.LastTradePrice = CommonFunctions.GetValueInDecimal(objScripDetails.lastTradeRateL, DecimalPoint) == "0" ? "" : CommonFunctions.GetValueInDecimal(objScripDetails.lastTradeRateL, DecimalPoint);

#if TWS
                        bstfivedata.LastTradeTime = objScripDetails.LastTradeTime;
#elif BOW
                        if (objScripDetails.LastTradeTime != "" &&( objScripDetails.LastTradeTime.Contains("AM") || objScripDetails.LastTradeTime.Contains("PM")))
                        {
                            bstfivedata.LastTradeTime = DateTime.ParseExact(objScripDetails.LastTradeTime, "hh:mm:ss tt", CultureInfo.InvariantCulture).ToString("HH:mm:ss");
                        }
                        else
                        {
                            bstfivedata.LastTradeTime = DateTime.ParseExact(objScripDetails.LastTradeTime, "hh:mm:ss", CultureInfo.InvariantCulture).ToString("HH:mm:ss");
                        }
#endif
                        bstfivedata.LastTradeQuantity = objScripDetails.lastTradeQtyL;
                    }
                    else
                        bstfivedata.LastTradeTime = " ";

                    bstfivedata.WtAvgPrice = CommonFunctions.GetValueInDecimal(objScripDetails.wtAvgRateL, DecimalPoint) == "0" ? "" : CommonFunctions.GetValueInDecimal(objScripDetails.wtAvgRateL, DecimalPoint);
                    bstfivedata.TradeVolume = (objScripDetails.TrdVolume).ToString() == "0" ? "" : (objScripDetails.TrdVolume).ToString();
                    if (objScripDetails.Unit_c == 'C')
                        bstfivedata.TradeValue = string.Format("{0} {1}", CommonFunctions.DisplayInDecimalFormatTouch<int>(objScripDetails.TrdValue, 2), "C");

                    else if (objScripDetails.Unit_c == 'L')
                        bstfivedata.TradeValue = string.Format("{0} {1}", CommonFunctions.DisplayInDecimalFormatTouch<int>(objScripDetails.TrdValue, 2), "L");

                    else
                        bstfivedata.TradeValue = CommonFunctions.DisplayInDecimalFormatTouch<int>(objScripDetails.TrdValue, 2) == "0" ? bstfivedata.TradeValue = string.Empty : CommonFunctions.DisplayInDecimalFormatTouch<int>(objScripDetails.TrdValue, DecimalPoint);
                    bstfivedata.OpenRate = CommonFunctions.GetValueInDecimal(objScripDetails.openRateL, DecimalPoint) == "0" ? "" : CommonFunctions.GetValueInDecimal(objScripDetails.openRateL, DecimalPoint);
                    bstfivedata.HighRate = CommonFunctions.GetValueInDecimal(objScripDetails.highRateL, DecimalPoint) == "0" ? "" : CommonFunctions.GetValueInDecimal(objScripDetails.highRateL, DecimalPoint);

                    bstfivedata.LowRate = CommonFunctions.GetValueInDecimal(objScripDetails.lowRateL, DecimalPoint) == "0" ? "" : CommonFunctions.GetValueInDecimal(objScripDetails.lowRateL, DecimalPoint);

                    bstfivedata.BRP = objScripDetails.BRP / Math.Pow(10, DecimalPoint);
                    if (objScripDetails.closeRateL == 0)
                    {
                        if (objScripDetails.PrevcloseRateL == 0)
                        {

                            if (DecimalPoint == 4)
                                bstfivedata.CloseRate = String.Format("{0:0.0000}", Convert.ToDecimal(MasterSharedMemory.objDicDP.Where(x => x.Key == ScripCode).Select(x => x.Value.PreviousClosePrice).FirstOrDefault() / Math.Pow(10, DecimalPoint))).ToString();
                            else if (DecimalPoint == 2)
                                bstfivedata.CloseRate = String.Format("{0:0.00}", Convert.ToDecimal(MasterSharedMemory.objDicDP.Where(x => x.Key == ScripCode).Select(x => x.Value.PreviousClosePrice).FirstOrDefault() / Math.Pow(10, DecimalPoint))).ToString();

                        }
                        else
                        {
                            if (DecimalPoint == 4)
                                bstfivedata.CloseRate = String.Format("{0:0.0000}", Convert.ToDecimal(objScripDetails.PrevcloseRateL / Math.Pow(10, DecimalPoint))).ToString();
                            else if (DecimalPoint == 2)
                                bstfivedata.CloseRate = String.Format("{0:0.00}", Convert.ToDecimal(objScripDetails.PrevcloseRateL / Math.Pow(10, DecimalPoint))).ToString();

                        }
                        if (bstfivedata.CloseRate == "0" || bstfivedata.CloseRate == "0.00" || bstfivedata.CloseRate == "0.0000")
                            bstfivedata.CloseRate = string.Empty;
                    }
                    else
                    {
                        bstfivedata.CloseRate = CommonFunctions.GetValueInDecimal(objScripDetails.closeRateL, DecimalPoint) == "0" ? "" : CommonFunctions.GetValueInDecimal(objScripDetails.closeRateL, DecimalPoint);
                    }

                    if (objScripDetails.LowerCtLmt == 0)
                    {
                        bstfivedata.LowerCtLmt = CommonFunctions.GetValueInDecimal(MasterSharedMemory.objDicDP.Where(x => x.Key == ScripCode).Select(x => x.Value.LowerCircuitPrice).FirstOrDefault(), DecimalPoint) == "0" ? "" : CommonFunctions.GetValueInDecimal(MasterSharedMemory.objDicDP.Where(x => x.Key == ScripCode).Select(x => x.Value.LowerCircuitPrice).FirstOrDefault(), DecimalPoint);
                    }
                    else
                    {
                        bstfivedata.LowerCtLmt = CommonFunctions.GetValueInDecimal(objScripDetails.LowerCtLmt, DecimalPoint) == "0" ? "" : CommonFunctions.GetValueInDecimal(objScripDetails.LowerCtLmt, DecimalPoint);
                    }
                    if (bstfivedata.LowerCtLmt == "0" || bstfivedata.LowerCtLmt == "0.00" || bstfivedata.LowerCtLmt == "0.0000")
                        bstfivedata.LowerCtLmt = string.Empty;
                    if (objScripDetails.UprCtLmt == 0)
                    {
                        bstfivedata.UpperCtLmt = CommonFunctions.GetValueInDecimal(MasterSharedMemory.objDicDP.Where(x => x.Key == ScripCode).Select(x => x.Value.UpperCircuitePrice).FirstOrDefault(), DecimalPoint) == "0" ? "" : CommonFunctions.GetValueInDecimal(MasterSharedMemory.objDicDP.Where(x => x.Key == ScripCode).Select(x => x.Value.UpperCircuitePrice).FirstOrDefault(), DecimalPoint);
                    }
                    else
                    {
                        bstfivedata.UpperCtLmt = CommonFunctions.GetValueInDecimal(objScripDetails.UprCtLmt, DecimalPoint) == "0" ? "" : CommonFunctions.GetValueInDecimal(objScripDetails.UprCtLmt, DecimalPoint);
                    }
                    if (bstfivedata.UpperCtLmt == "0" || bstfivedata.UpperCtLmt == "0.00" || bstfivedata.UpperCtLmt == "0.0000")
                        bstfivedata.UpperCtLmt = string.Empty;

                    bstfivedata.IndEqPrice = CommonFunctions.GetValueInDecimal(objScripDetails.IndicateEqPrice, DecimalPoint) == "0" ? "" : CommonFunctions.GetValueInDecimal(objScripDetails.IndicateEqPrice, DecimalPoint);
                    if (bstfivedata.IndEqPrice == "")
                    {
                        bstfivedata.IndEqQty = 0;
                    }
                    else
                    {
                        bstfivedata.IndEqQty = objScripDetails.IndicateEqQty;
                    }

                    if (bstfivedata.IndEqPrice == "0" || bstfivedata.IndEqPrice == "0.00" || bstfivedata.IndEqPrice == "0.0000")
                        bstfivedata.IndEqPrice = string.Empty;
                    //Assign 52Weeks High/Low Price and Date 
                    if (MasterSharedMemory.objDicDP.Where(x => x.Key == ScripCode).Select(x => x.Value).FirstOrDefault() != null)
                    {
                        bstfivedata.FiftyTwoHigh = CommonFunctions.GetValueInDecimal(MasterSharedMemory.objDicDP[ScripCode].WeeksHighprice, DecimalPoint) == "0" ? "" : CommonFunctions.GetValueInDecimal(MasterSharedMemory.objDicDP[ScripCode].WeeksHighprice, DecimalPoint);
                        bstfivedata.FiftyTwoLow = CommonFunctions.GetValueInDecimal(MasterSharedMemory.objDicDP[ScripCode].WeeksLowprice, DecimalPoint) == "0" ? "" : CommonFunctions.GetValueInDecimal(MasterSharedMemory.objDicDP[ScripCode].WeeksLowprice, DecimalPoint);
                        if (!string.IsNullOrEmpty(MasterSharedMemory.objDicDP[ScripCode].Dateof52weeksHighprice))
                            bstfivedata.FiftyTwoHighDate = DateTime.ParseExact(MasterSharedMemory.objDicDP[ScripCode].Dateof52weeksHighprice, "ddMMyyyy", CultureInfo.InvariantCulture).ToString("dd-MM-yyyy");

                        if (!string.IsNullOrEmpty(MasterSharedMemory.objDicDP[ScripCode].Dateof52weeksLowprice))
                            bstfivedata.FiftyTwoLowDate = DateTime.ParseExact(MasterSharedMemory.objDicDP[ScripCode].Dateof52weeksLowprice, "ddMMyyyy", CultureInfo.InvariantCulture).ToString("dd-MM-yyyy");
                    }
                    if (bstfivedata.FiftyTwoLow == "0" || bstfivedata.FiftyTwoLow == "0.00" || bstfivedata.FiftyTwoLow == "0.0000")
                        bstfivedata.FiftyTwoLow = string.Empty;
                    if (objScripDetails.PrevcloseRateL != 0 && objScripDetails.lastTradeRateL != 0)
                    {
                        if (Math.Round((objScripDetails.lastTradeRateL - objScripDetails.PrevcloseRateL) * 100.0 / objScripDetails.PrevcloseRateL, 2) > 0)
                            bstfivedata.ChangePercentage = string.Format("{0}{1} {2}", "+", Math.Round(((objScripDetails.lastTradeRateL - objScripDetails.PrevcloseRateL) * 100.0 / objScripDetails.PrevcloseRateL), 2).ToString("0.00"), "%");
                        else
                            bstfivedata.ChangePercentage = string.Format("{0} {1}", Math.Round(((objScripDetails.lastTradeRateL - objScripDetails.PrevcloseRateL) * 100.0 / objScripDetails.PrevcloseRateL), 2).ToString("0.00"), "%");
                    }
                    bstfivedata.PremDisc = CommonFunctions.GetValueInDecimal((objScripDetails.lastTradeRateL - objScripDetails.closeRateL), DecimalPoint) == "0" ? "" : CommonFunctions.GetValueInDecimal((objScripDetails.lastTradeRateL - objScripDetails.closeRateL), DecimalPoint);
                    if (bstfivedata.PremDisc == "0" || bstfivedata.PremDisc == "0.00" || bstfivedata.PremDisc == "0.0000")
                        bstfivedata.PremDisc = string.Empty;
                    TotalBuyQty = objScripDetails.totBuyQtyL;
                    TotalSellQty = objScripDetails.totSellQtyL;
                    sessionNumber = objScripDetails.SessionNo;
                    MarketType = objScripDetails.MarketType;
                    FindSession(sessionNumber, MarketType, Segment);
                    AssignTitle();
                    //Using Synchronization Context for adding items to the Collection
                    uiContext.Send(x => OtherDetails.Add(bstfivedata), null);

                    //Delegating Below Operation to UI(Main) thread
                    //App.Current.Dispatcher.Invoke((Action)delegate
                    //{
                    // OtherDetails.Add(bstfivedata);
                    //});


                    if (objScripDetails.listBestFive != null)
                    {
                        BstFiveCollection = new ObservableCollection<BestFiveModel>();
                        //Assign Best Five Details 
                        for (int i = 0; i < objScripDetails.listBestFive.Count; i++)
                        {

                            bstfivedata = new BestFiveModel();
                            bstfivedata.NoofBidBuy = objScripDetails.listBestFive[i].NoOfBidBuyL;
                            bstfivedata.BuyQualtity = objScripDetails.listBestFive[i].BuyQtyL;
                            bstfivedata.BuyRate = CommonFunctions.GetValueInDecimal(objScripDetails.listBestFive[i].BuyRateL, DecimalPoint) == "0" ? "" : CommonFunctions.GetValueInDecimal(objScripDetails.listBestFive[i].BuyRateL, DecimalPoint);
                            if (bstfivedata.BuyRate == "0" || bstfivedata.BuyRate == "0.00" || bstfivedata.BuyRate == "0.0000")
                                bstfivedata.BuyRate = string.Empty;
                            bstfivedata.OfferRate = CommonFunctions.GetValueInDecimal(objScripDetails.listBestFive[i].SellRateL, DecimalPoint) == "0" ? "" : CommonFunctions.GetValueInDecimal(objScripDetails.listBestFive[i].SellRateL, DecimalPoint);
                            if (bstfivedata.OfferRate == "0" || bstfivedata.OfferRate == "0.00" || bstfivedata.OfferRate == "0.0000")
                                bstfivedata.OfferRate = string.Empty;
                            bstfivedata.OfferQty = objScripDetails.listBestFive[i].SellQtyL;
                            bstfivedata.NoOfSellOrder = objScripDetails.listBestFive[i].NoOfBidSellL;
                            //Delegating Below Operation to UI (Main) thread
                            //App.Current.Dispatcher.Invoke((Action)delegate
                            //{
                            //  BstFiveCollection.Add(bstfivedata);
                            //});

                            //Using Synchronization Context for adding items to the Collection
                            uiContext.Send(x => BstFiveCollection.Add(bstfivedata), null);

                        }

                        //Informs UserControl to Update the ScripCode and ScripID dropdown.
                        if (SendData)
                            OnScripChangeuserControl(ScripCode);
                        //Messenger.Default.Send(ScripCode);
                    }

                    else if (SendData)
                    {
                        OnScripChangeuserControl(ScripCode);
                    }
                }
                else
                {
                    BstFiveCollection = new ObservableCollection<BestFiveModel>();
                    NotifyStaticPropertyChanged("BstFiveCollection");
                    OtherDetails = new ObservableCollection<BestFiveModel>();
                    NotifyStaticPropertyChanged("OtherDetails");
                    TotalBuyQty = 0;
                    TotalSellQty = 0;
                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
                return;
                // MessageBox.Show("Exception :" + ex + ex.InnerException + " " + "Error in Opening BestFive Window");
            }

            finally
            {
                NotifyStaticPropertyChanged("BstFiveCollection");
                NotifyStaticPropertyChanged("OtherDetails");
            }
        }
        public static void FindSession(int SessionNo, int MarketType, Enumerations.Segment segment)
        {
            if (Enumerations.Segment.Equity == segment || Enumerations.Segment.Debt == segment)
            {
                switch (SessionNo)
                {
                    case 0:
                        if (MarketType == 0)
                        {
                            if (PreviousMarketType == -1 && PreviousSessionNumber == -1)
                            {
                                //store previous session in session variable
                                SessionType = "LOGON";
                            }
                            else
                            {

                                SessionType = "Random End of Normal Call auction Order Entry Session";

                            }
                            PreviousMarketType = 0;
                            PreviousSessionNumber = 0;
                        }
                        break;
                    case 1:
                        if (MarketType == 0)
                        {
                            //  objSessionModel.SessionType = "Pre-Open Call auction, SPOS Order Entry Session start";
                            SessionType = "Normal Call auction Order Entry Session start";
                            PreviousMarketType = 0;
                            PreviousSessionNumber = 1;
                        }
                        if (MarketType == 20)
                        {

                            SessionType = "PCAS session Order Entry Session(Entry/End)";
                            //  objSessionModel.SessionType = "PCAS session 2 Order Entry Session[Freeze Session]";

                            PreviousMarketType = 20;
                            PreviousSessionNumber = 1;
                        }

                        break;
                    case 2:
                        if (MarketType == 0)
                        {
                            // objSessionModel.SessionType = "End of Matching Session of Pre - Open Call auction .The End of matching session will be communicated by session number 2 instead of 0.";
                            SessionType = "End of Matching Session of Normal Call auction";
                        }
                        if (MarketType == 20)
                        {

                            SessionType = "End of Matching Session of PCAS session";

                        }
                        PreviousMarketType = 20;
                        PreviousSessionNumber = 0;
                        break;
                    case 3:
                        SessionType = "Continuous Session";
                        PreviousMarketType = 0;
                        PreviousSessionNumber = 3;
                        break;
                    case 4:
                        SessionType = "Closing";
                        PreviousMarketType = 0;
                        PreviousSessionNumber = 4;
                        break;
                    case 5:
                        SessionType = "Post Closing session";
                        PreviousMarketType = 0;
                        PreviousSessionNumber = 5;
                        break;
                    case 6:
                        SessionType = "End of day";
                        PreviousMarketType = 0;
                        PreviousSessionNumber = 6;
                        break;
                    case 7:
                        SessionType = "Member Query Session";
                        PreviousMarketType = 0;
                        PreviousSessionNumber = 7;
                        break;
                    case 10:
                        SessionType = "Random End of SPOS Order Entry Session[Freeze Session]";
                        PreviousMarketType = 0;
                        PreviousSessionNumber = 10;
                        break;
                    case 12:
                        SessionType = "End of Matching Session of SPOS";
                        PreviousMarketType = 0;
                        PreviousSessionNumber = 12;
                        break;
                    case 13:
                        SessionType = "Continuous Session for SPOS";
                        PreviousMarketType = 0;
                        PreviousSessionNumber = 13;
                        break;
                }
            }
            else if (Enumerations.Segment.Derivative == segment || Enumerations.Segment.Currency == segment)
            {
                switch (SessionNo)
                {
                    case 3:
                        SessionType = "Continuous Session";
                        PreviousMarketType = 0;
                        PreviousSessionNumber = 3;
                        break;
                    default:
                        SessionType = " ";
                        break;
                }
            }
            else
                SessionType = "";
        }
        public static void MemberQueryBF()
        {
            Segment = (Enumerations.Segment)Enum.Parse(typeof(Enumerations.Segment), OrderEntryUC_VM.SelectedSegment);
            BroadcastReceiver.ScripDetails objScripDetails = BroadcastMaster.BroadcastMasterMemory.objScripDetailsConDict.Values.Where(x => x.ScripCode_l == OrderEntryUC_VM.SelectedScripCode).FirstOrDefault();
            if (objScripDetails != null)
            {
                if (!objScripDetails.HasBroadcastCome)
                {

                    QUERYPCASMKTINFOREQ objQUERYPCASMKTINFOREQ = CreateMemberQueryRequest();
                    objQUERYPCASMKTINFOREQ.ScripCodes_la = HeaderScripCode;
                    MemberQueryRequestProcessor objMemberQueryRequestProcessor = new MemberQueryRequestProcessor(new MemberQuery());
                    objMemberQueryRequestProcessor.ProcessRequest(objQUERYPCASMKTINFOREQ);
                }
            }
            else
            {
                QUERYPCASMKTINFOREQ objQUERYPCASMKTINFOREQ = CreateMemberQueryRequest();
                objQUERYPCASMKTINFOREQ.ScripCodes_la = HeaderScripCode;
                MemberQueryRequestProcessor objMemberQueryRequestProcessor = new MemberQueryRequestProcessor(new MemberQuery());
                objMemberQueryRequestProcessor.ProcessRequest(objQUERYPCASMKTINFOREQ);
            }
        }
        public static QUERYPCASMKTINFOREQ CreateMemberQueryRequest()
        {
            QUERYPCASMKTINFOREQ objQUERYPCASMKTINFOREQ = new QUERYPCASMKTINFOREQ();

            if (Segment == Enumerations.Segment.Equity)
            {

                objQUERYPCASMKTINFOREQ.NoOfScrips_s = 1;
                objQUERYPCASMKTINFOREQ.MessageTag = 601;
            }
            if (Segment == Enumerations.Segment.Derivative)
            {

                objQUERYPCASMKTINFOREQ.NoOfScrips_s = 2;
                objQUERYPCASMKTINFOREQ.MessageTag = 602;
            }
            if (Segment == Enumerations.Segment.Currency)
            {

                objQUERYPCASMKTINFOREQ.NoOfScrips_s = 3;
                objQUERYPCASMKTINFOREQ.MessageTag = 603;
            }
            objQUERYPCASMKTINFOREQ.ScripCodes_la = ScripCode;
            return objQUERYPCASMKTINFOREQ;
        }
#if BOW
        public static void UpdateValues(long scripCode)
        {
            ScripDetails objScripDetails = new ScripDetails();
            string lstrKeysTEM = MarketWatchConstants.objScripDetails.Where(x => x.ScriptCode_BseToken_NseToken == scripCode).Select(x => x.SecurityKey_MW_EMT_Token).FirstOrDefault();
            if (lstrKeysTEM != null)
            {
                string pstrSegment = MarketWatchConstants.objScripDetails.Where(x => x.ScriptCode_BseToken_NseToken == scripCode).Select(x => x.Segment_Market).FirstOrDefault();
                string pstrExchange = MarketWatchConstants.objScripDetails.Where(x => x.ScriptCode_BseToken_NseToken == scripCode).Select(x => x.Exchange_Source).FirstOrDefault();
                string glngCMToken = MarketWatchVM.GetTokenFromSelectedLegKey(lstrKeysTEM);
                string exchange = "";
                if (pstrExchange == "1")
                {
                    exchange = "BSE";
                }
                MarketWatchConstants.lstrTokenString = pstrSegment + "||" + glngCMToken + "||" + exchange;
                string lstrReturn = GetDataByCallingServlet(pstrSegment, pstrExchange, glngCMToken, MarketWatchConstants.lstrTokenString);
                //  RecordSplitter lobjRecordHelper = new RecordSplitter(lstrReturn);
                ArrayList lobjArgs = new ArrayList(2);
                if (lstrReturn.Substring(0, 1) == BowConstants.SUCCESS_FLAG)
                {

                    // IAsyncResult IAsyncResult = default(IAsyncResult);
                    lock (lobjArgs)
                    {

                        RecordSplitter lobjRecordHelper = new RecordSplitter(lstrReturn);
                        lobjArgs.Add(lobjRecordHelper);
                        lobjArgs.Add(true);
                    }
                }
                OpenMBPMessages(pstrExchange, MarketWatchConstants.lstrTokenString);

                if (lobjArgs.Count != 0) {
                    objScripDetails = MarketWatchVM.UpdateData(lobjArgs, MarketWatchConstants.lstrTokenString);
                }
                
                UpdateTitle(Convert.ToInt32(scripCode));
                UpdateBestWindow(objScripDetails, false);

            }


        }

        public static void OpenMBPMessages(string exchange, string pstrTokenString = "")
        {
            try
            {
                System.Collections.ArrayList lstrParameters = new System.Collections.ArrayList();
                System.Collections.ArrayList lstrValues = new System.Collections.ArrayList();
                string lstrMessage = null;

                lstrMessage = GetMessageString(exchange, true, pstrTokenString);
                SettingsManager.SendMessagesOverSocket(ref lstrMessage);
            }
            catch (Exception e)
            {
            }
        }

        public static string GetMessageString(string exchange, bool pblnOpen = true, string pstrTokenString = "")
        {
            return GetMBPMessageString(exchange, Enumerations.MBPMessageCalledFrom.MBP, pblnOpen, pstrTokenString);
        }

        public static void OnBroadCastRecieved(int scipcode, ScripDetails objScripDetails)
        {
            if (ScripCode == scipcode)
                UpdateBestWindow(objScripDetails, false);            
            
        }

        public static string GetMBPMessageString(string exchange, MBPMessageCalledFrom pobjCalledFrom, bool pblnOpen = true, string pstrTokenString = "")
        {

            string lstrMBPTokenToOpen1 = "";
            string lstrMBPTokenToOpen2 = "";
            string lstrMBPTokenToOpen = "";
            string lstrMarketId = null;
            string lstrToken = null;
            string lstrExchange = null;
            string[] lstrTempToOpen = null;
            string[] lstrDualMBPToken = null;
            string lstrMessage = "";

            if (pstrTokenString.Trim().Length > 0)
            {
                lstrDualMBPToken = pstrTokenString.Split('~');
                lstrTempToOpen = lstrDualMBPToken[0].Split('|');
                lstrMarketId = lstrTempToOpen[0];
                lstrToken = lstrTempToOpen[2];
                lstrExchange = exchange;
                // send exchange id
                lstrMBPTokenToOpen1 = lstrToken + "^" + lstrExchange + "^" + lstrMarketId;
                lstrMBPTokenToOpen = lstrMBPTokenToOpen1;
                //: for Dual

            }

            if (string.IsNullOrWhiteSpace(lstrMBPTokenToOpen) == false)
            {
                if (pblnOpen == true)
                {
                    lstrMessage = "OPENMBP|";
                }
                else
                {
                    lstrMessage = "CLOSEMBP|";
                }

                return lstrMessage + UtilityConnParameters.GetInstance.UserId + "|" + UtilityConnParameters.GetInstance.LoginKeyValue + "|" + lstrMBPTokenToOpen;
            }
            else
            {
                Infrastructure.Logger.WriteLog("Some how MBPTokenString  is blank in GetMessageString Function");
                return "";
            }
        }

        private static string GetDataByCallingServlet(string pstrSegment, string pstrExchange, string pstrToken, string pstrOldMBPToken)
        {
            string lstrReturn = null;
            //: Call the servlet 
            string[] larrParameterName = new string[7];
            string[] larrParameterValue = new string[7];

            larrParameterName[0] = "marketbypricesegment";
            larrParameterValue[0] = pstrSegment;
            larrParameterName[1] = "marketbypricesource";
            larrParameterValue[1] = pstrExchange;
            // exchange id
            larrParameterName[2] = "marketbypricetoken";
            larrParameterValue[2] = pstrToken;
            larrParameterName[3] = "CloseMBP";
            larrParameterValue[3] = pstrOldMBPToken;
            //: Vaibhav : Added for HTTP Broadcast mode
            if (MarketWatchConstants.gblnConnectOnHTTP == true)
            {
                larrParameterName[4] = MarketWatchConstants.MODE_STRING;
                larrParameterValue[4] = MarketWatchConstants.PROXY_STRING;
            }
            try
            {
                //gfrmActiveMarketWatch.MarketWatchHelper.getMarketWatchControl.RePaintRow(gfrmActiveMarketWatch.MarketWatchHelper.getMarketWatchControl.CurRow)
                //: A callback will be made to the RefreshForm Function
                //: lstrResult will return blank value
                //if (Globals.GETInstance.gblnDirectBroadcastConfigured)
                //{
                //    gfrmActiveMarketWatch.MarketWatchHelper.PaintRowState(gfrmActiveMarketWatch.MarketWatchHelper.getCurrentlySelectedLegsKey());
                //}
                //HTTPRequestHelper.HTTPHelper.ResponseReturned lobjDelegate = new HTTPRequestHelper.HTTPHelper.ResponseReturned(RefreshForm);
                //HTTPHlpr.ResponseReturned lobjDelegate = new HTTPHlpr.ResponseReturned(RefreshForm);
                lstrReturn = SettingsManager.GetDataFromServer("GetMarketByPrice", larrParameterName, larrParameterValue, false, null);

                return lstrReturn;
            }
            catch (Exception ex)
            {
                return lstrReturn;
            }
        }
#endif
        #region StaticNotifyPropertyChangedEvent
        public static event EventHandler<PropertyChangedEventArgs> StaticPropertyChanged
                 = delegate { };
        private static void NotifyStaticPropertyChanged(string propertyName)
        {
            StaticPropertyChanged(null, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
        #region NotifyPropertyChangedEvent
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(String propertyName = "")
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;

            if (handler != null)
            {

                var e = new PropertyChangedEventArgs(propertyName);

                this.PropertyChanged(this, e);

            }

        }
        #endregion
    }
    public partial class BestFiveVM
    {
        private static void AssignTitle()
        {

            if (MasterSharedMemory.objMastertxtDictBaseBSE.ContainsKey(ScripCode))
            {
                ScripID = MasterSharedMemory.objMastertxtDictBaseBSE.Where(x => x.Value.ScripCode == ScripCode).Select(x => x.Value.ScripId).FirstOrDefault();
                ScripName = MasterSharedMemory.objMastertxtDictBaseBSE.Where(x => x.Value.ScripCode == ScripCode).Select(x => x.Value.ScripName).FirstOrDefault();
            }
            else if (MasterSharedMemory.objMasterDerivativeDictBaseBSE.ContainsKey(ScripCode))
            {
                ScripID = MasterSharedMemory.objMasterDerivativeDictBaseBSE.Where(x => x.Value.ContractTokenNum == ScripCode).Select(x => x.Value.ScripId).FirstOrDefault();
                ScripName = MasterSharedMemory.objMasterDerivativeDictBaseBSE.Where(x => x.Value.ContractTokenNum == ScripCode).Select(x => x.Value.InstrumentName).FirstOrDefault();
            }

            else if (MasterSharedMemory.objMasterCurrencyDictBaseBSE.ContainsKey(ScripCode))
            {
                ScripID = MasterSharedMemory.objMasterCurrencyDictBaseBSE.Where(x => x.Value.ContractTokenNum == ScripCode).Select(x => x.Value.ScripId).FirstOrDefault();
                ScripName = MasterSharedMemory.objMasterCurrencyDictBaseBSE.Where(x => x.Value.ContractTokenNum == ScripCode).Select(x => x.Value.InstrumentName).FirstOrDefault();
            }
            TitleTWSBestFiveWindow = "BestFiveWindow     [" + ScripCode + ":" + ScripID + "]       " + SessionType;

        }
#if TWS
        private int _WidthWindow;

        public int WidthWindow
        {
            get { return _WidthWindow; }
            set
            {
                _WidthWindow = value;
                NotifyPropertyChanged("WidthWindow");
            }
        }


        private int _HeightWindow;

        public int HeightWindow
        {
            get { return _HeightWindow; }
            set
            {
                _HeightWindow = value;
                NotifyPropertyChanged("HeightWindow");
            }
        }

        private static string _NetPositionNetQty = "0";

        public static string NetPositionNetQty
        {
            get { return _NetPositionNetQty; }
            set { _NetPositionNetQty = value; NotifyStaticPropertyChanged("NetPositionNetQty"); }
        }

        private static string _NetPositionNetValue = "0";

        public static string NetPositionNetValue
        {
            get { return _NetPositionNetValue; }
            set { _NetPositionNetValue = value; NotifyStaticPropertyChanged("NetPositionNetValue"); }
        }

        private RelayCommand _ShrinkWindow;

        public RelayCommand ShrinkWindow
        {
            get
            {
                return _ShrinkWindow ?? (_ShrinkWindow = new RelayCommand(
                    (Obj) => ShrinkWindow_Click()
                    ));
            }
        }
        private RelayCommand _Escape_ShortCut;

        public RelayCommand Escape_ShortCut
        {
            get
            {
                return _Escape_ShortCut ?? (_Escape_ShortCut = new RelayCommand(
                    (object e) => EscapeUsingUserControl(e)
                        ));
            }
        }



        /// <summary>
        ///MarketPicture Functionality :Apoorva Sharma 3-02-2018
        /// </summary>
        public BestFiveVM()
        {
            DeciVisiDev = "Hidden";
            DeciVisiEq = "Visible";
            //    if(OrderEntryUC_VM.SelectedSegment != "")
            Segment = (Enumerations.Segment)Enum.Parse(typeof(Enumerations.Segment), OrderEntryUC_VM.SelectedSegment);
            //if (Processor.UMSProcessor.IntimateOrderEntryNetPosition == null)
            //{
                Processor.UMSProcessor.IntimateOrderEntryNetPosition += FetchNetPositionByScripCode;
            //}
        }

        public static void Initialize()
        {
            //   BroadcastController.OnMessageTransmitted += objBroadCastProcessor_OnBroadCastRecieved;
            OrderEntryUC_VM.OnScripIDOrCodeChange += UpdateValues;
            OrderEntryUC_VM.OnScripIDOrCodeChange += FetchNetPositionByScripCode;
        }
        public void ShrinkWindow_Click()
        {
            WidthWindow = 300;
            HeightWindow = 300;
            NotifyPropertyChanged("WidthWindow");
            NotifyPropertyChanged("HeightWindow");
            //TODO: Resize Window Size accordingly
        }
        public static void objBroadCastProcessor_OnBroadCastRecieved(Model.ScripDetails objScripDetails)
        {
            if (ScripCode == objScripDetails.ScriptCode_BseToken_NseToken)
            {
                //ScripDetails objScripDetails = BroadcastMasterMemory.objScripDetailsConDict.Where(x => x.Key == ScripCode).Select(x => x.Value).FirstOrDefault();
                //  BroadcastReceiver.ScripDetails Br = BroadcastMaster.BroadcastMasterMemory.objScripDetailsConDict.Where(x => x.Key == objScripDetails.ScriptCode_BseToken_NseToken).Select(x => x.Value).FirstOrDefault();
                //  ScripDetails objScripDetails = new ScripDetails();
                //  objScripDetails = MainWindowVM.UpdateScripDataFromMemory(Br);
                //   objScripDetails.ScriptCode_BseToken_NseToken = ScripCode;

                UpdateBestWindow(objScripDetails, false);

            }
        }
        public static void UpdateValues(long scripCode)
        {

            ScripCode = Convert.ToInt32(scripCode);
            //ScripDetails objScripDetails = BroadcastMasterMemory.objScripDetailsConDict.Where(x => x.Key == scripCode).Select(x => x.Value).FirstOrDefault() == null ? new ScripDetails() : BroadcastMasterMemory.objScripDetailsConDict.Where(x => x.Key == scripCode).Select(x => x.Value).FirstOrDefault();
            BroadcastReceiver.ScripDetails Br = BroadcastMaster.BroadcastMasterMemory.objScripDetailsConDict.Where(x => x.Key == scripCode).Select(x => x.Value).FirstOrDefault() == null ? new BroadcastReceiver.ScripDetails() : BroadcastMaster.BroadcastMasterMemory.objScripDetailsConDict.Where(x => x.Key == scripCode).Select(x => x.Value).FirstOrDefault();
            ScripDetails objScripDetails = new ScripDetails();
            objScripDetails = MainWindowVM.UpdateScripDataFromMemory(Br);
            objScripDetails.ScriptCode_BseToken_NseToken = ScripCode;
            SubscribeList.SubscribeScrip s = new SubscribeScrip();
            s.ScripCode_l = objScripDetails.ScriptCode_BseToken_NseToken;
            s.UpdateFlag_s = 1;
            if (SubscribeScripMemory.objMasterSubscribeScrip.ContainsKey(s.ScripCode_l))
            { }
            else
                SubscribeScripMemory.objMasterSubscribeScrip.TryAdd(s.ScripCode_l, s);

            UpdateTitle(Convert.ToInt32(scripCode));
            UpdateBestWindow(objScripDetails, false);
        }
        private void EscapeUsingUserControl(object e)
        {
            BestFiveMarketPicture mainwindow = System.Windows.Application.Current.Windows.OfType<BestFiveMarketPicture>().FirstOrDefault();

            mainwindow.Hide();
        }

#endif
        public static void FetchNetPositionByScripCode(long ScripCode)
        {
            UpdateScripNetPosition(Convert.ToString(ScripCode), MemoryManager.NetPositionSWDemoDict.Where(x => ((CommonFrontEnd.Model.Trade.NetPosition)x.Value).ScripCode == ScripCode).ToList());
        }
        public static void UpdateScripNetPosition(string strScripCode, List<KeyValuePair<string, object>> Obj)
        {

            try
            {
                int index = 0;
                long longScripCode = Convert.ToInt64(strScripCode);
                if ((Obj != null && Obj.Count() > 0) &&
                    (ScripCode == Convert.ToInt64(strScripCode) || ScripCode == Convert.ToInt64(strScripCode)))
                {


                    var results = Obj.AsParallel().GroupBy(p => ((NetPosition)p.Value).ScripCode,
                                  p => p.Value,
                                  (key, g) => new
                                  {
                                      scripCode = key,
                                      scripData = g.ToList()
                                  }
                                 );
                    foreach (var item in results)
                    {
                        var intBuyPosition = item.scripData.Cast<NetPosition>().Sum(x => x.BuyQty);
                        if (intBuyPosition == 0)
                        {
                            BuyPosition = string.Empty;
                        }
                        else
                        {
                            BuyPosition = intBuyPosition.ToString();
                        }

                        //oScripWisePositionModel.SellQty = item.scripData.Cast<NetPosition>().Sum(x => x.SellQty);
                        var intSellPosition = item.scripData.Cast<NetPosition>().Sum(x => x.SellQty);
                        if (intSellPosition == 0)
                        {
                            SellPosition = string.Empty;
                        }
                        else
                        {
                            SellPosition = intSellPosition.ToString();
                        }

                        var segment = CommonFunctions.GetSegmentID(longScripCode);
                        var multiplier = CommonFunctions.GetQuantityMultiplier(longScripCode, "BSE", segment);
                        var totalBuyVal = item.scripData.Cast<NetPosition>().Sum(x => x.BuyValue);
                        var totalBuyQty = intBuyPosition * multiplier;


                        var decimalPoint = CommonFunctions.GetDecimal(Convert.ToInt32(strScripCode), "BSE", segment);
                        var AvgBuyRate = Convert.ToDouble(CommonFrontEnd.Common.CommonFunctions.DisplayInDecimalFormat(totalBuyVal, totalBuyQty, decimalPoint));//item.scripData.Cast<NetPosition>().Sum(x => x.AvgBuyRate);

                        var totalSellVal = item.scripData.Cast<NetPosition>().Sum(x => x.SellValue);
                        var totalSellQty = intSellPosition * multiplier;

                        var AvgSellRate = Convert.ToDouble(CommonFrontEnd.Common.CommonFunctions.DisplayInDecimalFormat(totalSellVal, totalSellQty, decimalPoint));//item.scripData.Cast<NetPosition>().Sum(x => x.SellValue) / oScripWisePositionModel.SellQty;//item.scripData.Cast<NetPosition>().Sum(x => x.AvgSellRate);

                        var NetQty = item.scripData.Cast<NetPosition>().Sum(x => x.NetQty);
                        var NetValue = Convert.ToInt64((totalBuyVal - totalSellVal) / Math.Pow(10, UtilityApplicationDetails.GetInstance.MaxDecimalPoint));

                        if (decimalPoint == 4)
                        {
                            if (AvgBuyRate == 0)
                            {
                                AvgBuyPosition = string.Empty;
                            }
                            else
                            {
                                AvgBuyPosition = string.Format("{0:0.0000}", (AvgBuyRate / (Math.Pow(10, UtilityApplicationDetails.GetInstance.MaxDecimalPoint))));
                            }

                            if (AvgSellRate == 0)
                            {
                                AvgSellPosition = string.Empty;
                            }
                            else
                            {
                                AvgSellPosition = string.Format("{0:0.0000}", (AvgSellRate / (Math.Pow(10, UtilityApplicationDetails.GetInstance.MaxDecimalPoint))));
                            }

                            if (NetValue == 0 || NetQty == 0)
                            {
                                NetPositionNetValue = string.Empty;
                            }
                            else
                            {
                                NetPositionNetValue = string.Format("{0:0.0000}", (Convert.ToDecimal(NetValue) / Convert.ToDecimal(NetQty)) / multiplier);
                            }

                            if (NetQty == 0)
                            {
                                NetPositionNetQty = string.Empty;
                            }
                            else
                            {
                                NetPositionNetQty = Convert.ToString(NetQty);
                            }

                        }
                        else//Decimal 2 places code- Gaurav Jadhav 24/4/2018
                        {
                            if (AvgBuyRate == 0)
                            {
                                AvgBuyPosition = string.Empty;
                            }
                            else
                            {
                                AvgBuyPosition = string.Format("{0:0.00}", (AvgBuyRate / (Math.Pow(10, UtilityApplicationDetails.GetInstance.MaxDecimalPoint))));
                            }

                            if (AvgSellRate == 0)
                            {
                                AvgSellPosition = string.Empty;
                            }
                            else
                            {
                                AvgSellPosition = string.Format("{0:0.00}", (AvgSellRate / (Math.Pow(10, UtilityApplicationDetails.GetInstance.MaxDecimalPoint))));
                            }

                            if (NetValue == 0 || NetQty == 0)
                            {
                                NetPositionNetValue = string.Empty;
                            }
                            else
                            {
                                NetPositionNetValue = string.Format("{0:0.00}", (Convert.ToDecimal(NetValue) / Convert.ToDecimal(NetQty)) / multiplier);
                            }

                            if (NetQty == 0)
                            {
                                NetPositionNetQty = string.Empty;
                            }
                            else
                            {
                                NetPositionNetQty = Convert.ToString(NetQty);
                            }

                        }
                    }
                }
                else//clear fields
                {
                    BuyPosition = string.Empty;
                    SellPosition = string.Empty;
                    AvgBuyPosition = string.Empty;
                    AvgSellPosition = string.Empty;
                    NetPositionNetValue = string.Empty;
                    NetPositionNetQty = string.Empty;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

}
