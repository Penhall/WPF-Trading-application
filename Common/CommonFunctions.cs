using BroadcastMaster;
using CommonFrontEnd.Global;
using CommonFrontEnd.Common;
using CommonFrontEnd.SharedMemories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Collections.ObjectModel;
using CommonFrontEnd.View.Order;
using CommonFrontEnd.Model.Order;
using CommonFrontEnd.Model;
using static CommonFrontEnd.Common.Enumerations;
using CommonFrontEnd.View;
using CommonFrontEnd.ViewModel.Order;
using CommonFrontEnd.Model.ETIMessageStructure;
using static CommonFrontEnd.Common.Enumerations.Trade;
using static CommonFrontEnd.SharedMemories.Limit;
using System.Collections.Concurrent;
using System.Windows;
using CommonFrontEnd.Model.Trade;
using System.IO;
using CommonFrontEnd.Processor.Order;
using CommonFrontEnd.ViewModel;
using static CommonFrontEnd.SharedMemories.DataAccessLayer;

namespace CommonFrontEnd.Common
{

    public class CommonFunctions
    {
        public static bool headerflag;

        public static string fileName;

        public static StreamWriter writer;

        public static List<long> TouchLineScripQueryList = new List<long>();
        public static bool MarketPicQuery(long ScripCode)
        {
            int SegNo = Common.CommonFunctions.GetSegmentFromScripCode(ScripCode);
            if (SegNo == 0)
                return false;

            BroadcastReceiver.ScripDetails objScripDetails = BroadcastMaster.BroadcastMasterMemory.objScripDetailsConDict.Values.Where(x => x.ScripCode_l == ScripCode).FirstOrDefault();
            if (objScripDetails != null && objScripDetails.HasBroadcastCome == true)
                return false;

            QUERYPCASMKTINFOREQ objQUERYPCASMKTINFOREQ = new QUERYPCASMKTINFOREQ();
            objQUERYPCASMKTINFOREQ.MessageTag = 600 + SegNo;
            objQUERYPCASMKTINFOREQ.NoOfScrips_s = SegNo;
            objQUERYPCASMKTINFOREQ.ScripCodes_la = (int)ScripCode;
            MemberQueryRequestProcessor objMemberQueryRequestProcessor = new MemberQueryRequestProcessor(new MemberQuery());
            objMemberQueryRequestProcessor.ProcessRequest(objQUERYPCASMKTINFOREQ);
            return true;
        }

        public static int GetSegment(int SegNo)
        {
            int Seg = -1;
            if (SegNo == (int)AdminTradeRequestMsgTag.EQTYSEGINDICATOR)
                Seg = 0;
            else if (SegNo == (int)AdminTradeRequestMsgTag.DERISEGINDICATOR)
                Seg = 1;
            else if (SegNo == (int)AdminTradeRequestMsgTag.CURRSEGINDICATOR)
                Seg = 2;

            return (Seg);
        }

        public static int GetSegment(string SegName)
        {
            int Seg = 0;
            if (SegName == Segment.Equity.ToString() || SegName == Segment.Debt.ToString())
                Seg = 0;
            else if (SegName == Segment.Derivative.ToString())
                Seg = 1;
            else if (SegName == Segment.Currency.ToString())
                Seg = 2;

            return (Seg);
        }

        public static string GetSegmentName(int Seg)
        {
            string SegName = "";
            if (Seg == (int)Segment.Equity)
                SegName = "Equity";
            else if (Seg == (int)Segment.Derivative)
                SegName = "Derivative";
            else if (Seg == (int)Segment.Currency)
                SegName = "Currency";

            return (SegName);
        }

        public static bool GetBitValue(int Val, int ChkBitPos)
        {
            bool hasBit = false;
            hasBit = (Val & ChkBitPos) == ChkBitPos;
            return (!hasBit);
        }

        public static long GetPrice(long ScripCode, string BuySellIndicator)
        {
            long Rate = 0;
            BroadcastReceiver.ScripDetails Br = BroadcastMaster.BroadcastMasterMemory.objScripDetailsConDict.Where(x => x.Key == ScripCode).Select(x => x.Value).FirstOrDefault() == null ? new BroadcastReceiver.ScripDetails() : BroadcastMaster.BroadcastMasterMemory.objScripDetailsConDict.Where(x => x.Key == ScripCode).Select(x => x.Value).FirstOrDefault();
            if (Br != null)
            {
                if (BuySellIndicator == "B" && Br.Det[0] != null)
                    Rate = Br.Det[0].SellRate_l;
                else if (BuySellIndicator == "S" && Br.Det[0] != null)
                    Rate = Br.Det[0].BuyRate_l;

                if (Rate <= 0)
                    Rate = Br.LastTradeRate_l;

                if (Rate <= 0)
                    Rate = Br.CloseRate_l;

                if (Rate <= 0)
                    Rate = Br.PrevClosePrice_l;
            }
            return Rate;
        }

        public static long GetPrice(long ScripCode, string BuySellIndicator, bool SpreadFlag)
        {
            long Rate = 0;
            BroadcastReceiver.ScripDetails Br = BroadcastMaster.BroadcastMasterMemory.objScripDetailsConDict.Where(x => x.Key == ScripCode).Select(x => x.Value).FirstOrDefault() == null ? new BroadcastReceiver.ScripDetails() : BroadcastMaster.BroadcastMasterMemory.objScripDetailsConDict.Where(x => x.Key == ScripCode).Select(x => x.Value).FirstOrDefault();
            if (Br != null)
            {
                if (SpreadFlag == false)
                {
                    if (BuySellIndicator == "B" && Br.Det[0] != null)
                        Rate = Br.Det[0].SellRate_l;
                    else if (BuySellIndicator == "S" && Br.Det[0] != null)
                        Rate = Br.Det[0].BuyRate_l;

                    if (Rate <= 0)
                        Rate = Br.LastTradeRate_l;
                }

                if (Rate <= 0)
                    Rate = Br.CloseRate_l;

                if (Rate <= 0)
                    Rate = Br.PrevClosePrice_l;
            }
            return Rate;
        }

        public static long GetStrikePrice(long ScripCode, int Seg)
        {
            long StrikePrice = 0;

            if (Seg == (int)Segment.Derivative)
            {
                if (MasterSharedMemory.objMasterDerivativeDictBaseBSE.ContainsKey(ScripCode))
                    StrikePrice = MasterSharedMemory.objMasterDerivativeDictBaseBSE[ScripCode].StrikePrice;
            }
            else if (Seg == (int)Segment.Currency)
            {
                if (MasterSharedMemory.objMasterCurrencyDictBaseBSE.ContainsKey(ScripCode))
                    StrikePrice = MasterSharedMemory.objMasterCurrencyDictBaseBSE[ScripCode].StrikePrice;
            }

            return StrikePrice;
        }

        public static int GetComplexInstrumentType(long ScripCode, int Seg)
        {
            int InstrumentType = 1;

            if (Seg == (int)Segment.Derivative)
            {
                if (MasterSharedMemory.objMasterDerivativeDictBaseBSE.ContainsKey(ScripCode))
                    InstrumentType = MasterSharedMemory.objMasterDerivativeDictBaseBSE[ScripCode].ComplexInstrumentType;
            }
            else if (Seg == (int)Segment.Currency)
            {
                if (MasterSharedMemory.objMasterCurrencyDictBaseBSE.ContainsKey(ScripCode))
                    InstrumentType = MasterSharedMemory.objMasterCurrencyDictBaseBSE[ScripCode].ComplexInstrumentType;
            }
            return InstrumentType;
        }

        public static string GetOptionType(long ScripCode, int Seg)
        {
            string OptionType = null;

            if (Seg == (int)Segment.Derivative)
            {
                if (MasterSharedMemory.objMasterDerivativeDictBaseBSE.ContainsKey(ScripCode))
                    OptionType = MasterSharedMemory.objMasterDerivativeDictBaseBSE[ScripCode].OptionType;
            }
            else if (Seg == (int)Segment.Currency)
            {
                if (MasterSharedMemory.objMasterCurrencyDictBaseBSE.ContainsKey(ScripCode))
                    OptionType = MasterSharedMemory.objMasterCurrencyDictBaseBSE[ScripCode].OptionType;
            }
            return OptionType;
        }

        public static void GetSpreadLegDetails(long ScripCode, int Seg, string BuySellIndicator, string OrderAction, ref SpreadLegInfo[] SpreadDet)
        {
            SpreadDet[(int)SpreadLeg.LEG0].ScripCodeLeg = ScripCode;
            if (Seg == (int)Segment.Derivative)
            {
                if (MasterSharedMemory.objMasterDerivativeSpreadDictBaseBSE.ContainsKey(ScripCode))
                {
                    SpreadDet[(int)SpreadLeg.LEG1].ScripCodeLeg = MasterSharedMemory.objMasterDerivativeSpreadDictBaseBSE[ScripCode].ContractTokenNum_Leg1;
                    SpreadDet[(int)SpreadLeg.LEG2].ScripCodeLeg = MasterSharedMemory.objMasterDerivativeSpreadDictBaseBSE[ScripCode].ContractTokenNum_Leg2;

                    SpreadDet[(int)SpreadLeg.LEG1].ScripIdLeg = MasterSharedMemory.objMasterDerivativeDictBaseBSE[SpreadDet[(int)SpreadLeg.LEG1].ScripCodeLeg].ScripId;
                    SpreadDet[(int)SpreadLeg.LEG2].ScripIdLeg = MasterSharedMemory.objMasterDerivativeDictBaseBSE[SpreadDet[(int)SpreadLeg.LEG2].ScripCodeLeg].ScripId;
                }
            }
            else if (Seg == (int)Segment.Currency)
            {
                if (MasterSharedMemory.objMasterCurrencySpreadDictBaseBSE.ContainsKey(ScripCode))
                {
                    SpreadDet[(int)SpreadLeg.LEG1].ScripCodeLeg = MasterSharedMemory.objMasterCurrencySpreadDictBaseBSE[ScripCode].ContractTokenNum_Leg1;
                    SpreadDet[(int)SpreadLeg.LEG2].ScripCodeLeg = MasterSharedMemory.objMasterCurrencySpreadDictBaseBSE[ScripCode].ContractTokenNum_Leg2;

                    SpreadDet[(int)SpreadLeg.LEG1].ScripIdLeg = MasterSharedMemory.objMasterCurrencyDictBaseBSE[SpreadDet[(int)SpreadLeg.LEG1].ScripCodeLeg].ScripId;
                    SpreadDet[(int)SpreadLeg.LEG2].ScripIdLeg = MasterSharedMemory.objMasterCurrencyDictBaseBSE[SpreadDet[(int)SpreadLeg.LEG2].ScripCodeLeg].ScripId;
                }
            }

            SpreadDet[(int)SpreadLeg.LEG0].BuySellIndicator = BuySellIndicator;
            SpreadDet[(int)SpreadLeg.LEG1].BuySellIndicator = GetBuySellForSpread(BuySellIndicator, GetStrategyId(ScripCode, Seg), 1);
            SpreadDet[(int)SpreadLeg.LEG2].BuySellIndicator = GetBuySellForSpread(BuySellIndicator, GetStrategyId(ScripCode, Seg), 2);

            SpreadDet[(int)SpreadLeg.LEG0].PrevPricePaisa = 0;
            SpreadDet[(int)SpreadLeg.LEG0].CurrPricePaisa = 0;
            if (OrderAction == "A" || OrderAction == "D")
            {
                SpreadDet[(int)SpreadLeg.LEG1].PrevPricePaisa = 0;
                SpreadDet[(int)SpreadLeg.LEG1].CurrPricePaisa = GetPrice(SpreadDet[(int)SpreadLeg.LEG1].ScripCodeLeg, SpreadDet[(int)SpreadLeg.LEG1].BuySellIndicator, true);

                SpreadDet[(int)SpreadLeg.LEG2].PrevPricePaisa = 0;
                SpreadDet[(int)SpreadLeg.LEG2].CurrPricePaisa = GetPrice(SpreadDet[(int)SpreadLeg.LEG2].ScripCodeLeg, SpreadDet[(int)SpreadLeg.LEG2].BuySellIndicator, true);
            }
            else if (OrderAction == "U" || OrderAction == "CNVTLIMIT" || OrderAction == "IOCACTION" || OrderAction == "" || OrderAction == null)
            {
                SpreadDet[(int)SpreadLeg.LEG1].PrevPricePaisa = GetPrice(SpreadDet[(int)SpreadLeg.LEG1].ScripCodeLeg, SpreadDet[(int)SpreadLeg.LEG1].BuySellIndicator, true);
                SpreadDet[(int)SpreadLeg.LEG1].CurrPricePaisa = GetPrice(SpreadDet[(int)SpreadLeg.LEG1].ScripCodeLeg, SpreadDet[(int)SpreadLeg.LEG1].BuySellIndicator, true);

                SpreadDet[(int)SpreadLeg.LEG2].PrevPricePaisa = GetPrice(SpreadDet[(int)SpreadLeg.LEG2].ScripCodeLeg, SpreadDet[(int)SpreadLeg.LEG2].BuySellIndicator, true);
                SpreadDet[(int)SpreadLeg.LEG2].CurrPricePaisa = GetPrice(SpreadDet[(int)SpreadLeg.LEG2].ScripCodeLeg, SpreadDet[(int)SpreadLeg.LEG2].BuySellIndicator, true);
            }
            else if (OrderAction == "TRDDNLD" || OrderAction == "TRDONLINE")
            {
                SpreadDet[(int)SpreadLeg.LEG1].PrevPricePaisa = 0;
                SpreadDet[(int)SpreadLeg.LEG1].CurrPricePaisa = 0;

                SpreadDet[(int)SpreadLeg.LEG2].PrevPricePaisa = 0;
                SpreadDet[(int)SpreadLeg.LEG2].CurrPricePaisa = 0;

            }
        }

        public static int GetStrategyId(long ScripCode, int Seg)
        {
            int StrategyId = -1;

            if (Seg == (int)Segment.Derivative)
            {
                if (MasterSharedMemory.objMasterDerivativeDictBaseBSE.ContainsKey(ScripCode))
                    StrategyId = MasterSharedMemory.objMasterDerivativeDictBaseBSE[ScripCode].StrategyID;
            }
            else if (Seg == (int)Segment.Currency)
            {
                if (MasterSharedMemory.objMasterCurrencyDictBaseBSE.ContainsKey(ScripCode))
                    StrategyId = MasterSharedMemory.objMasterCurrencyDictBaseBSE[ScripCode].StrategyID;
            }
            return StrategyId;
        }

        public static string GetBuySellForSpread(string BuySellIndicator, int StrategyId, int Leg)
        {
            if (Leg == 1)
            {
                if (StrategyId == 0)
                {
                    if (BuySellIndicator == "B")
                        BuySellIndicator = "S";
                    else
                        BuySellIndicator = "B";
                }
                else if (StrategyId == 15)
                {
                    if (BuySellIndicator == "B")
                        BuySellIndicator = "B";
                    else
                        BuySellIndicator = "S";
                }
                else if (StrategyId == 28)
                {
                    if (BuySellIndicator == "B")
                        BuySellIndicator = "B";
                    else
                        BuySellIndicator = "S";
                }
            }
            else if (Leg == 2)
            {
                if (StrategyId == 0)
                {
                    if (BuySellIndicator == "B")
                        BuySellIndicator = "B";
                    else
                        BuySellIndicator = "S";
                }
                else if (StrategyId == 15)
                {
                    if (BuySellIndicator == "B")
                        BuySellIndicator = "S";
                    else
                        BuySellIndicator = "B";
                }
                else if (StrategyId == 28)
                {
                    if (BuySellIndicator == "B")
                        BuySellIndicator = "B";
                    else
                        BuySellIndicator = "S";
                }
            }
            return BuySellIndicator;
        }

        public static void InitializeLimitMemory()
        {
            int TotSeg = Limit.ExchangeList.Length;

            g_Limit = new Limits[TotSeg];

            for (int n = 0; n < TotSeg; n++)
            {
                g_Limit[n] = new Limits();
                g_Limit[n].g_NetQtyLimit = new ConcurrentDictionary<long, NetQtyLimit>();
                g_Limit[n].g_GroupLimit = new ConcurrentDictionary<string, GroupLimit>();
            }


            if (MasterSharedMemory.GroupWiseLimitDict != null)
            {
                GroupLimit obj = null;
                foreach (var item in MasterSharedMemory.GroupWiseLimitDict)
                {
                    obj = new GroupLimit();
                    g_Limit[(int)ExchangeNum.BSE_EQT].g_GroupLimit.TryAdd(item.Key.Trim().ToUpper(), obj);
                }
                obj = new GroupLimit();
                g_Limit[(int)ExchangeNum.BSE_EDRV].g_GroupLimit.TryAdd("DF", obj);
                obj = new GroupLimit();
                g_Limit[(int)ExchangeNum.BSE_CDRV].g_GroupLimit.TryAdd("CD", obj);
            }


            if (MasterSharedMemory.objMastertxtDictBaseBSE != null)
            {
                foreach (var item in MasterSharedMemory.objMastertxtDictBaseBSE)
                {
                    NetQtyLimit obj = new NetQtyLimit();
                    g_Limit[(int)ExchangeNum.BSE_EQT].g_NetQtyLimit.TryAdd(item.Key, obj);
                }
            }

            if (MasterSharedMemory.objMasterDerivativeDictBaseBSE != null)
            {
                foreach (var item in MasterSharedMemory.objMasterDerivativeDictBaseBSE)
                {
                    NetQtyLimit obj = new NetQtyLimit();
                    g_Limit[(int)ExchangeNum.BSE_EDRV].g_NetQtyLimit.TryAdd(item.Key, obj);
                }
            }

            if (MasterSharedMemory.objMasterCurrencyDictBaseBSE != null)
            {
                foreach (var item in MasterSharedMemory.objMasterCurrencyDictBaseBSE)
                {
                    NetQtyLimit obj = new NetQtyLimit();
                    g_Limit[(int)ExchangeNum.BSE_CDRV].g_NetQtyLimit.TryAdd(item.Key, obj);
                }
            }
        }

        public static void PopulateLimitMemory(uint Message, object Obj)
        {
            switch (Message)
            {
                case 22001:
                    {
                        ETradeLimitOnlineUMS TrdLimit = new ETradeLimitOnlineUMS();
                        TrdLimit = Obj as ETradeLimitOnlineUMS;
                        int Seg = GetSegment((int)TrdLimit.MsgTag);
                        long PrevGrossBuyLimit = 0, PrevGrossSellLimit = 0;
                        long PrevBuyLimit = 0, PrevSellLimit = 0;
                        long PrevNetValue = 0;

                        PrevGrossBuyLimit = g_Limit[Seg].GrossBuyLimit;
                        PrevGrossSellLimit = g_Limit[Seg].GrossSellLimit;
                        PrevNetValue = g_Limit[Seg].NetValue;
                        g_Limit[Seg].GrossBuyLimit = TrdLimit.GrossLimitBuy * 100000;
                        g_Limit[Seg].GrossSellLimit = TrdLimit.GrossLimitSell * 100000;
                        g_Limit[Seg].NetValue = TrdLimit.NetValue * 100000;

                        g_Limit[Seg].AvailGrossBuyLimit += (g_Limit[Seg].GrossBuyLimit - PrevGrossBuyLimit);
                        g_Limit[Seg].AvailGrossSellLimit += (g_Limit[Seg].GrossSellLimit - PrevGrossSellLimit);

                        //g_Limit[Seg].CurrNetValue += (g_Limit[Seg].NetValue - PrevNetValue);

                        g_Limit[Seg].UnrestNetQtyLimit = GetBitValue(TrdLimit.NXActionId, (1 << 29));
                        g_Limit[Seg].UnrestGrpLimit = GetBitValue(TrdLimit.NXActionId, (1 << 30));

                        foreach (var item in g_Limit[Seg].g_NetQtyLimit)
                        {
                            PrevBuyLimit = g_Limit[Seg].g_NetQtyLimit[item.Key].BuyLimit;
                            PrevSellLimit = g_Limit[Seg].g_NetQtyLimit[item.Key].SellLimit;
                            g_Limit[Seg].g_NetQtyLimit[item.Key].BuyLimit = TrdLimit.DefaultBuy;
                            g_Limit[Seg].g_NetQtyLimit[item.Key].SellLimit = TrdLimit.DefaultSell;

                            g_Limit[Seg].g_NetQtyLimit[item.Key].AvailBuyLimit += (g_Limit[Seg].g_NetQtyLimit[item.Key].BuyLimit - PrevBuyLimit);
                            g_Limit[Seg].g_NetQtyLimit[item.Key].AvailSellLimit += (g_Limit[Seg].g_NetQtyLimit[item.Key].SellLimit - PrevSellLimit);
                        }
                    }
                    break;

                case 22002:
                    {
                        ETradeLimitReply TrdLimit = new ETradeLimitReply();
                        TrdLimit = Obj as ETradeLimitReply;
                        int Seg = GetSegment((int)TrdLimit.MsgTag);
                        long PrevGrossBuyLimit = 0, PrevGrossSellLimit = 0;
                        long PrevBuyLimit = 0, PrevSellLimit = 0;
                        long PrevNetValue = 0;

                        PrevGrossBuyLimit = g_Limit[Seg].GrossBuyLimit;
                        PrevGrossSellLimit = g_Limit[Seg].GrossSellLimit;
                        PrevNetValue = g_Limit[Seg].NetValue;
                        g_Limit[Seg].GrossBuyLimit = TrdLimit.GrossLimitBuy * 100000;
                        g_Limit[Seg].GrossSellLimit = TrdLimit.GrossLimitSell * 100000;
                        g_Limit[Seg].NetValue = TrdLimit.NetValue * 100000;

                        g_Limit[Seg].AvailGrossBuyLimit += (g_Limit[Seg].GrossBuyLimit - PrevGrossBuyLimit);
                        g_Limit[Seg].AvailGrossSellLimit += (g_Limit[Seg].GrossSellLimit - PrevGrossSellLimit);

                        //g_Limit[Seg].CurrNetValue  += (g_Limit[Seg].NetValue - PrevNetValue);

                        //g_Limit[Seg].AvailGrossBuyLimit += g_Limit[Seg].GrossBuyLimit;
                        //g_Limit[Seg].AvailGrossSellLimit += g_Limit[Seg].GrossSellLimit;

                        g_Limit[Seg].UnrestNetQtyLimit = GetBitValue(TrdLimit.NXActionId, (1 << 29));
                        g_Limit[Seg].UnrestGrpLimit = GetBitValue(TrdLimit.NXActionId, (1 << 30));

                        foreach (var item in g_Limit[Seg].g_NetQtyLimit)
                        {
                            PrevBuyLimit = g_Limit[Seg].g_NetQtyLimit[item.Key].BuyLimit;
                            PrevSellLimit = g_Limit[Seg].g_NetQtyLimit[item.Key].SellLimit;
                            g_Limit[Seg].g_NetQtyLimit[item.Key].BuyLimit = TrdLimit.DefaultBuy;
                            g_Limit[Seg].g_NetQtyLimit[item.Key].SellLimit = TrdLimit.DefaultSell;

                            g_Limit[Seg].g_NetQtyLimit[item.Key].AvailBuyLimit += (g_Limit[Seg].g_NetQtyLimit[item.Key].BuyLimit - PrevBuyLimit);
                            g_Limit[Seg].g_NetQtyLimit[item.Key].AvailSellLimit += (g_Limit[Seg].g_NetQtyLimit[item.Key].SellLimit - PrevSellLimit);

                            //g_Limit[Seg].g_NetQtyLimit[item.Key].AvailBuyLimit += g_Limit[Seg].g_NetQtyLimit[item.Key].BuyLimit;
                            //g_Limit[Seg].g_NetQtyLimit[item.Key].AvailSellLimit += g_Limit[Seg].g_NetQtyLimit[item.Key].SellLimit;
                        }
                    }
                    break;

                case 22012:
                    {
                        ETradeGWLimitReply GrpLimit = new ETradeGWLimitReply();
                        GrpLimit = Obj as ETradeGWLimitReply;
                        int Seg = GetSegment((int)GrpLimit.MsgTag);
                        long PrevBuyLimit = 0, PrevSellLimit = 0, CurrBuyLimit = 0, CurrSellLimit = 0;

                        for (int n = 0; n < GrpLimit.NoOfRecs; n++)
                        {
                            EGroupWiseLimitDet GrpDet = new EGroupWiseLimitDet();
                            GrpDet = (EGroupWiseLimitDet)GrpLimit.lstTraderGwLimitGrp[n];
                            string GrpName = GrpDet.GroupName.Trim().ToUpper();

                            if (g_Limit[Seg].g_GroupLimit.ContainsKey(GrpName))
                            {
                                PrevBuyLimit = g_Limit[Seg].g_GroupLimit[GrpName].BuyLimit == -1 ? 0 : g_Limit[Seg].g_GroupLimit[GrpName].BuyLimit * 100000;
                                PrevSellLimit = g_Limit[Seg].g_GroupLimit[GrpName].SellLimit == -1 ? 0 : g_Limit[Seg].g_GroupLimit[GrpName].SellLimit * 100000;

                                CurrBuyLimit = GrpDet.BuyValue == -1 ? 0 : GrpDet.BuyValue * 100000;
                                CurrSellLimit = GrpDet.SellValue == -1 ? 0 : GrpDet.SellValue * 100000;

                                g_Limit[Seg].g_GroupLimit[GrpName].AvailBuyLimit += (CurrBuyLimit - PrevBuyLimit);
                                g_Limit[Seg].g_GroupLimit[GrpName].AvailSellLimit += (CurrSellLimit - PrevSellLimit);

                                g_Limit[Seg].g_GroupLimit[GrpName].BuyLimit = GrpDet.BuyValue;
                                g_Limit[Seg].g_GroupLimit[GrpName].SellLimit = GrpDet.SellValue;

                                //PrevBuyLimit = g_Limit[Seg].g_GroupLimit[GrpName].BuyLimit * 100000;
                                //PrevSellLimit = g_Limit[Seg].g_GroupLimit[GrpName].SellLimit * 100000;
                                //g_Limit[Seg].g_GroupLimit[GrpName].BuyLimit = GrpDet.BuyValue * 100000;
                                //g_Limit[Seg].g_GroupLimit[GrpName].SellLimit = GrpDet.SellValue * 100000;

                                //g_Limit[Seg].g_GroupLimit[GrpName].AvailBuyLimit += (g_Limit[Seg].g_GroupLimit[GrpName].BuyLimit - PrevBuyLimit);
                                //g_Limit[Seg].g_GroupLimit[GrpName].AvailSellLimit += (g_Limit[Seg].g_GroupLimit[GrpName].SellLimit - PrevSellLimit);

                                //g_Limit[Seg].g_GroupLimit[GrpName].BuyLimit = GrpDet.BuyValue;
                                //g_Limit[Seg].g_GroupLimit[GrpName].SellLimit = GrpDet.SellValue;

                                UpdateGroupLimitScreen(GrpName, g_Limit[Seg], true, Seg);
                            }
                        }
                    }
                    break;

                case 22016:
                    {
                        ETradeGWLimitOnlineUMS GrpLimit = new ETradeGWLimitOnlineUMS();
                        GrpLimit = Obj as ETradeGWLimitOnlineUMS;
                        int Seg = GetSegment((int)GrpLimit.MsgTag);
                        long PrevBuyLimit = 0, PrevSellLimit = 0, CurrBuyLimit = 0, CurrSellLimit = 0;

                        for (int n = 0; n < GrpLimit.NoOfRecs; n++)
                        {
                            EGroupWiseLimitDet GrpDet = new EGroupWiseLimitDet();
                            GrpDet = (EGroupWiseLimitDet)GrpLimit.lstTraderGwLimitGrp[n];
                            string GrpName = GrpDet.GroupName.Trim().ToUpper();

                            if (g_Limit[Seg].g_GroupLimit.ContainsKey(GrpName))
                            {
                                PrevBuyLimit = g_Limit[Seg].g_GroupLimit[GrpName].BuyLimit == -1 ? 0 : g_Limit[Seg].g_GroupLimit[GrpName].BuyLimit * 100000;
                                PrevSellLimit = g_Limit[Seg].g_GroupLimit[GrpName].SellLimit == -1 ? 0 : g_Limit[Seg].g_GroupLimit[GrpName].SellLimit * 100000;

                                CurrBuyLimit = GrpDet.BuyValue == -1 ? 0 : GrpDet.BuyValue * 100000;
                                CurrSellLimit = GrpDet.SellValue == -1 ? 0 : GrpDet.SellValue * 100000;

                                g_Limit[Seg].g_GroupLimit[GrpName].AvailBuyLimit += (CurrBuyLimit - PrevBuyLimit);
                                g_Limit[Seg].g_GroupLimit[GrpName].AvailSellLimit += (CurrSellLimit - PrevSellLimit);

                                g_Limit[Seg].g_GroupLimit[GrpName].BuyLimit = GrpDet.BuyValue;
                                g_Limit[Seg].g_GroupLimit[GrpName].SellLimit = GrpDet.SellValue;

                                //PrevBuyLimit = g_Limit[Seg].g_GroupLimit[GrpName].BuyLimit * 100000;
                                //PrevSellLimit = g_Limit[Seg].g_GroupLimit[GrpName].SellLimit * 100000;
                                //g_Limit[Seg].g_GroupLimit[GrpName].BuyLimit = GrpDet.BuyValue * 100000;
                                //g_Limit[Seg].g_GroupLimit[GrpName].SellLimit = GrpDet.SellValue * 100000;

                                //g_Limit[Seg].g_GroupLimit[GrpName].AvailBuyLimit += (g_Limit[Seg].g_GroupLimit[GrpName].BuyLimit - PrevBuyLimit);
                                //g_Limit[Seg].g_GroupLimit[GrpName].AvailSellLimit += (g_Limit[Seg].g_GroupLimit[GrpName].SellLimit - PrevSellLimit);

                                //g_Limit[Seg].g_GroupLimit[GrpName].BuyLimit = GrpDet.BuyValue;
                                //g_Limit[Seg].g_GroupLimit[GrpName].SellLimit = GrpDet.SellValue;

                                UpdateGroupLimitScreen(GrpName, g_Limit[Seg], true, Seg);
                            }
                        }
                    }
                    break;
            }

        }


        public static void UpdateGroupLimitScreen(string Group, Limits SegLimit, bool UpdateFlag, int segmentFlag)
        {
            //GroupLimit grp =  SegLimit.g_GroupLimit[Group];
            GroupWiseLimitsModel gmodfel = new GroupWiseLimitsModel();

            if (UpdateFlag)
            {
                MasterSharedMemory.GroupWiseLimitDict[Group].BuyValue = SegLimit.g_GroupLimit[Group].BuyLimit;
                MasterSharedMemory.GroupWiseLimitDict[Group].SellValue = SegLimit.g_GroupLimit[Group].SellLimit;
                MasterSharedMemory.GroupWiseLimitDict[Group].AvlBuy = SegLimit.g_GroupLimit[Group].AvailBuyLimit;
                MasterSharedMemory.GroupWiseLimitDict[Group].AvlSell = SegLimit.g_GroupLimit[Group].AvailSellLimit;
            }

            gmodfel.Group = Group;
            gmodfel.BuyValue = SegLimit.g_GroupLimit[Group].BuyLimit;
            gmodfel.SellValue = SegLimit.g_GroupLimit[Group].SellLimit;

            if (gmodfel.BuyValue == -1)
                gmodfel.AvlBuy = -1 * 100000;
            else
                gmodfel.AvlBuy = SegLimit.g_GroupLimit[Group].AvailBuyLimit;

            if (gmodfel.SellValue == -1)
                gmodfel.AvlSell = -1 * 100000;
            else
                gmodfel.AvlSell = SegLimit.g_GroupLimit[Group].AvailSellLimit;

            MasterSharedMemory.GroupWiseLimitDict.TryUpdate(gmodfel.Group, gmodfel, MasterSharedMemory.GroupWiseLimitDict[gmodfel.Group]);

            if (MemoryManager.OnGroupwiseLimitReceive != null && UpdateFlag)
                MemoryManager.OnGroupwiseLimitReceive.Invoke(gmodfel);
            //var segment = "Equity";

            if (segmentFlag == 0)
                NormalOrderEntryVM.GetInstance.HeaderTitle = string.Format("Order Entry EQX - Buy Limit: [{0:0.00} L], Sell Limit: [{1:0.00} L]", SegLimit.AvailGrossBuyLimit / 100000, SegLimit.AvailGrossSellLimit / 100000);
            else if (segmentFlag == 1)
                NormalOrderEntryVM.GetInstance.HeaderTitle = string.Format("Order Entry EDX - Buy Limit: [{0:0.00} L], Sell Limit: [{1:0.00} L]", SegLimit.AvailGrossBuyLimit / 100000, SegLimit.AvailGrossSellLimit / 100000);
            else if (segmentFlag == 2)
                NormalOrderEntryVM.GetInstance.HeaderTitle = string.Format("Order Entry CDX - Buy Limit: [{0:0.00} L], Sell Limit: [{1:0.00} L]", SegLimit.AvailGrossBuyLimit / 100000, SegLimit.AvailGrossSellLimit / 100000);
        }

        public static string DisplaySpreadLeg(long ScripCode)
        {
            string returnStr = string.Empty;

            string SegmentName = GetSegmentID(ScripCode);

            if(!(SegmentName == Segment.Derivative.ToString() || SegmentName == Segment.Currency.ToString()))
                return returnStr;
            int Seg = GetSegment(SegmentName);
            int SpreadType = GetComplexInstrumentType(ScripCode, Seg);
            if (SpreadType == 1)
                return returnStr;

            string BuySellIndicator = NormalOrderEntryVM.GetInstance.BuySellInd;
            SpreadLegInfo[] SpreadDet = new SpreadLegInfo[SpreadLegList.Length];
            for (int n = 0; n < SpreadLegList.Length; n++)
                SpreadDet[n] = new SpreadLegInfo();

            GetSpreadLegDetails(ScripCode, Seg, BuySellIndicator, "A", ref SpreadDet);
            returnStr = SpreadDet[(int)SpreadLeg.LEG1].BuySellIndicator + ":" + SpreadDet[(int)SpreadLeg.LEG1].ScripIdLeg + ", ";
            returnStr += SpreadDet[(int)SpreadLeg.LEG2].BuySellIndicator + ":" + SpreadDet[(int)SpreadLeg.LEG2].ScripIdLeg;

            return returnStr;
        }

        public static string DisplaySpreadLeg(string SegmentName, long ScripCode, string BuySellIndicator)
        {
            string returnStr = string.Empty;

            int Seg = GetSegment(SegmentName);
            int SpreadType = GetComplexInstrumentType(ScripCode, Seg);
            if(SpreadType == 1)
                return returnStr;

            SpreadLegInfo[] SpreadDet = new SpreadLegInfo[SpreadLegList.Length];
            for (int n = 0; n < SpreadLegList.Length; n++)
                SpreadDet[n] = new SpreadLegInfo();

            GetSpreadLegDetails(ScripCode, Seg, BuySellIndicator, "A", ref SpreadDet);

            returnStr = SpreadDet[(int)SpreadLeg.LEG1].BuySellIndicator + ":" + SpreadDet[(int)SpreadLeg.LEG1].ScripIdLeg + ", ";
            returnStr += SpreadDet[(int)SpreadLeg.LEG2].BuySellIndicator + ":" + SpreadDet[(int)SpreadLeg.LEG2].ScripIdLeg;
            
            return returnStr;
        }

        public static bool CalculateGrossValue(int Seg, int Type, object Obj)
        {
            switch (Seg)
            {
                case 10:
                    {
                        string OrderAction = "", BuySellIndicator = "", Group = "";
                        long ScripCode = 0;
                        int MktLot, QtyMult;
                        int PrevQty = 0, CurrQty = 0;
                        long PrevPricePaisa, CurrPricePaisa;
                        double PrevPrice, CurrPrice;
                        double PrevValue, CurrValue;
                        bool ValidationFlag = false;

                        int Deci = 2;

                        MktLot = 1;
                        QtyMult = 1;

                        PrevValue = CurrValue = 0;

                        if (Type == 3)
                        {
                            TradeUMS Ord = new TradeUMS();
                            Ord = Obj as TradeUMS;

                            OrderAction = "TRDDNLD";
                            BuySellIndicator = Ord.BSFlag;
                            ScripCode = Ord.ScripCode;
                            QtyMult = (int)GetQuantityMultiplier(ScripCode, "BSE", GetSegmentName(Seg));
                            Deci = GetDecimal((int)ScripCode, "BSE", GetSegmentName(Seg));
                            Group = Ord.ScripGroup;
                            PrevPricePaisa = 0;
                            CurrPricePaisa = Ord.Rate;

                            PrevQty = 0 * MktLot * QtyMult;
                            PrevPrice = PrevPricePaisa / Math.Pow(10, Deci);
                            PrevValue = PrevPrice * PrevQty;

                            CurrQty = Ord.LastQty * MktLot * QtyMult;
                            CurrPrice = CurrPricePaisa / Math.Pow(10, Deci);
                            CurrValue = CurrPrice * CurrQty;
                        }
                        else if (Type == 4)
                        {
                            TradeUMS Ord = new TradeUMS();
                            Ord = Obj as TradeUMS;

                            OrderAction = "TRDONLINE";
                            BuySellIndicator = Ord.BSFlag;
                            ScripCode = Ord.ScripCode;
                            QtyMult = (int)GetQuantityMultiplier(ScripCode, "BSE", GetSegmentName(Seg));
                            Deci = GetDecimal((int)ScripCode, "BSE", GetSegmentName(Seg));
                            //Group = MasterSharedMemory.objMastertxtDictBaseBSE[ScripCode].GroupName;
                            Group = Ord.ScripGroup;
                            PrevPricePaisa = 0;
                            CurrPricePaisa = Ord.Rate;

                            PrevQty = 0 * MktLot * QtyMult;
                            PrevPrice = PrevPricePaisa / Math.Pow(10, Deci);
                            PrevValue = PrevPrice * PrevQty;

                            CurrQty = Ord.LastQty * MktLot * QtyMult;
                            CurrPrice = CurrPricePaisa / Math.Pow(10, Deci);
                            CurrValue = CurrPrice * CurrQty;
                        }
                        else if (Type == 5)
                        {
                            EMarketOrderUMS Ord = new EMarketOrderUMS();
                            Ord = Obj as EMarketOrderUMS;

                            OrderAction = "CNVTLIMIT";
                            ScripCode = Ord.ScripCode;
                            QtyMult = (int)GetQuantityMultiplier(ScripCode, "BSE", GetSegmentName(Seg));
                            Deci = GetDecimal((int)ScripCode, "BSE", GetSegmentName(Seg));
                            Group = GetGroupName(ScripCode, "BSE", GetSegmentName(Seg));

                            string key = string.Format("{0}_{1}", Ord.ScripCode, Ord.OrderId);
                            if (SharedMemories.MemoryManager.OrderDictionary.ContainsKey(key))
                            {
                                OrderModel OrdModel = SharedMemories.MemoryManager.OrderDictionary[key];
                                if (OrdModel != null)
                                {
                                    BuySellIndicator = OrdModel.BuySellIndicator;

                                    //PrevPricePaisa = OrdModel.Price;
                                    PrevPricePaisa = GetPrice(ScripCode, BuySellIndicator);
                                    CurrPricePaisa = Ord.CnvtdRate;

                                    PrevQty = OrdModel.PendingQuantity * MktLot * QtyMult;
                                    PrevPrice = PrevPricePaisa / Math.Pow(10, Deci);
                                    PrevValue = PrevPrice * PrevQty;

                                    CurrQty = OrdModel.OriginalQty * MktLot * QtyMult;
                                    CurrPrice = CurrPricePaisa / Math.Pow(10, Deci);
                                    CurrValue = CurrPrice * CurrQty;

                                    ValidationFlag = false;
                                }
                            }
                        }
                        else if (Type == 6)
                        {
                            EIOCCancelOrderUMS Ord = new EIOCCancelOrderUMS();
                            Ord = Obj as EIOCCancelOrderUMS;

                            OrderAction = "IOCACTION";
                            ScripCode = Ord.ScripCode;
                            QtyMult = (int)GetQuantityMultiplier(ScripCode, "BSE", GetSegmentName(Seg));
                            Deci = GetDecimal((int)ScripCode, "BSE", GetSegmentName(Seg));
                            Group = GetGroupName(ScripCode, "BSE", GetSegmentName(Seg));

                            string key = string.Format("{0}_{1}", Ord.ScripCode, Ord.OrderId);
                            if (SharedMemories.MemoryManager.OrderDictionary.ContainsKey(key))
                            {
                                OrderModel OrdModel = SharedMemories.MemoryManager.OrderDictionary[key];
                                if (OrdModel != null)
                                {
                                    BuySellIndicator = OrdModel.BuySellIndicator;

                                    PrevPricePaisa = 0;
                                    CurrPricePaisa = OrdModel.Price;

                                    if (OrdModel.OrderType == "G")
                                        CurrPricePaisa = GetPrice(ScripCode, BuySellIndicator);

                                    PrevQty = 0 * MktLot * QtyMult;
                                    PrevPrice = PrevPricePaisa / Math.Pow(10, Deci);
                                    PrevValue = PrevPrice * PrevQty;

                                    CurrQty = -(Ord.KilledQty * MktLot * QtyMult);
                                    CurrPrice = CurrPricePaisa / Math.Pow(10, Deci);
                                    CurrValue = CurrPrice * CurrQty;

                                    ValidationFlag = false;
                                }
                            }
                        }
                        else
                        {
                            OrderModel Ord = new OrderModel();
                            Ord = Obj as OrderModel;

                            OrderAction = Ord.OrderAction;
                            BuySellIndicator = Ord.BuySellIndicator;
                            ScripCode = Ord.ScripCode;
                            QtyMult = (int)GetQuantityMultiplier(ScripCode, "BSE", GetSegmentName(Seg));
                            Deci = GetDecimal((int)ScripCode, "BSE", GetSegmentName(Seg));
                            Group = Ord.Group;
                            if (Group == null)
                                Group = GetGroupName(ScripCode, "BSE", GetSegmentName(Seg));

                            if (OrderAction == "A")
                            {
                                PrevPricePaisa = 0;
                                CurrPricePaisa = Ord.Price;
                                if (Ord.OrderType == "G")
                                    CurrPricePaisa = GetPrice(ScripCode, BuySellIndicator);

                                PrevQty = 0 * MktLot * QtyMult;
                                PrevPrice = PrevPricePaisa / Math.Pow(10, Deci);
                                PrevValue = PrevPrice * PrevQty;

                                CurrQty = Ord.OriginalQty * MktLot * QtyMult;
                                CurrPrice = CurrPricePaisa / Math.Pow(10, Deci);
                                CurrValue = CurrPrice * CurrQty;

                                ValidationFlag = true;
                            }
                            else if (OrderAction == "U")
                            {
                                OrderModel tOrd = new OrderModel();
                                tOrd = MemoryManager.OrderDictionaryBackupMemory[Ord.OrderKey];

                                //MemoryManager.OrderDictionary.TryGetValue(Ord.OrderKey, out tOrd);

                                PrevPricePaisa = tOrd.Price;
                                CurrPricePaisa = Ord.Price;

                                if (tOrd.OrderType == "G")
                                    PrevPricePaisa = GetPrice(ScripCode, BuySellIndicator);

                                if (Ord.OrderType == "G")
                                    CurrPricePaisa = GetPrice(ScripCode, BuySellIndicator);

                                if (Type == 7)
                                {
                                    PrevQty = tOrd.PendingQuantity;
                                    CurrQty = Ord.OriginalQty + tOrd.PendingQuantity;
                                }
                                else
                                {
                                    PrevQty = tOrd.PendingQuantity;
                                    CurrQty = Ord.OriginalQty;
                                }


                                PrevQty = PrevQty * MktLot * QtyMult;
                                PrevPrice = PrevPricePaisa / Math.Pow(10, Deci);
                                PrevValue = PrevPrice * PrevQty;

                                CurrQty = CurrQty * MktLot * QtyMult;
                                CurrPrice = CurrPricePaisa / Math.Pow(10, Deci);
                                CurrValue = CurrPrice * CurrQty;

                                ValidationFlag = true;
                            }
                            else if (OrderAction == "D")
                            {
                                PrevPricePaisa = 0;
                                CurrPricePaisa = Ord.Price;

                                if (Ord.OrderType == "G")
                                    CurrPricePaisa = GetPrice(ScripCode, BuySellIndicator);

                                PrevQty = 0 * MktLot * QtyMult;
                                PrevPrice = PrevPricePaisa / Math.Pow(10, Deci);
                                PrevValue = PrevPrice * PrevQty;

                                CurrQty = -(Ord.PendingQuantity * MktLot * QtyMult);
                                CurrPrice = CurrPricePaisa / Math.Pow(10, Deci);
                                CurrValue = CurrPrice * CurrQty;
                            }
                            else
                            {
                                PrevPricePaisa = 0;
                                CurrPricePaisa = Ord.Price;

                                if (Ord.OrderType == "G")
                                    CurrPricePaisa = GetPrice(ScripCode, BuySellIndicator);

                                PrevQty = 0 * MktLot * QtyMult;
                                PrevPrice = PrevPricePaisa / Math.Pow(10, Deci);
                                PrevValue = PrevPrice * PrevQty;

                                CurrQty = Ord.PendingQuantity * MktLot * QtyMult;
                                CurrPrice = CurrPricePaisa / Math.Pow(10, Deci);
                                CurrValue = CurrPrice * CurrQty;
                            }
                        }

                        if (Type == 7)
                        {
                            ValidationFlag = false;

                            OrderAction = "REJECTED";

                        }



                        if (BuySellIndicator == "B")
                        {
                            if (ValidationFlag == true)
                            {
                                if (g_Limit[Seg].AvailGrossBuyLimit < (CurrValue - PrevValue))
                                {
                                    MessageBox.Show("Gross Buy Limit Exceeded", "Capital", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
                                    return false;
                                }

                                if (g_Limit[Seg].UnrestNetQtyLimit && g_Limit[Seg].g_NetQtyLimit[ScripCode].BuyLimit > -1 && g_Limit[Seg].g_NetQtyLimit[ScripCode].AvailBuyLimit < (CurrQty - PrevQty))
                                {
                                    MessageBox.Show("Net Quantity Buy Limit Exceeded", "Capital", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
                                    return false;
                                }

                                if (g_Limit[Seg].UnrestGrpLimit && g_Limit[Seg].g_GroupLimit[Group].BuyLimit > -1 && g_Limit[Seg].g_GroupLimit[Group].AvailBuyLimit < (CurrValue - PrevValue))
                                {
                                    MessageBox.Show("Group Buy Limit Exceeded", "Capital", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
                                    return false;
                                }

                                if (g_Limit[Seg].NetValue > -1 && g_Limit[Seg].NetValue < ((g_Limit[Seg].BuyValue + (CurrValue - PrevValue)) + g_Limit[Seg].CurrNetValue))
                                {
                                    MessageBox.Show("Net Value Limit Exceeded", "Capital", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
                                    return false;
                                }
                            }


                            if (OrderAction == "TRDDNLD")
                            {
                                g_Limit[Seg].AvailGrossBuyLimit -= (CurrValue - PrevValue);

                                g_Limit[Seg].g_NetQtyLimit[ScripCode].AvailBuyLimit -= (CurrQty - PrevQty);

                                g_Limit[Seg].g_GroupLimit[Group].AvailBuyLimit -= (CurrValue - PrevValue);

                                g_Limit[Seg].g_NetQtyLimit[ScripCode].AvailSellLimit += (CurrQty - PrevQty);

                                g_Limit[Seg].CurrNetValue += (CurrValue - PrevValue);
                            }
                            else if (OrderAction == "TRDONLINE")
                            {
                                g_Limit[Seg].g_NetQtyLimit[ScripCode].AvailSellLimit += (CurrQty - PrevQty);

                                g_Limit[Seg].BuyValue -= (CurrValue - PrevValue);

                                g_Limit[Seg].CurrNetValue += (CurrValue - PrevValue);
                            }
                            else if (OrderAction == "CNVTLIMIT")
                            {
                                g_Limit[Seg].AvailGrossBuyLimit -= (CurrValue - PrevValue);

                                //if (g_Limit[Seg].UnrestGrpLimit)
                                g_Limit[Seg].g_GroupLimit[Group].AvailBuyLimit -= (CurrValue - PrevValue);

                                g_Limit[Seg].BuyValue += (CurrValue - PrevValue);
                            }
                            else if (OrderAction == "REJECTED")
                            {
                                g_Limit[Seg].AvailGrossBuyLimit += (CurrValue - PrevValue);

                                //if (g_Limit[Seg].UnrestNetQtyLimit && g_Limit[Seg].g_NetQtyLimit[Ord.ScripCode].BuyLimit > -1)
                                g_Limit[Seg].g_NetQtyLimit[ScripCode].AvailBuyLimit += (CurrQty - PrevQty);

                                //if (g_Limit[Seg].UnrestGrpLimit)
                                g_Limit[Seg].g_GroupLimit[Group].AvailBuyLimit += (CurrValue - PrevValue);

                                g_Limit[Seg].BuyValue -= (CurrValue - PrevValue);

                            }
                            else
                            {
                                g_Limit[Seg].AvailGrossBuyLimit -= (CurrValue - PrevValue);

                                //if (g_Limit[Seg].UnrestNetQtyLimit && g_Limit[Seg].g_NetQtyLimit[Ord.ScripCode].BuyLimit > -1)
                                g_Limit[Seg].g_NetQtyLimit[ScripCode].AvailBuyLimit -= (CurrQty - PrevQty);

                                //if (g_Limit[Seg].UnrestGrpLimit)
                                g_Limit[Seg].g_GroupLimit[Group].AvailBuyLimit -= (CurrValue - PrevValue);

                                g_Limit[Seg].BuyValue += (CurrValue - PrevValue);
                            }
                        }
                        else if (BuySellIndicator == "S")
                        {
                            if (ValidationFlag == true)
                            {
                                if (g_Limit[Seg].AvailGrossSellLimit < (CurrValue - PrevValue))
                                {
                                    MessageBox.Show("Gross Sell Limit Exceeded", "Capital", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
                                    return false;
                                }

                                if (g_Limit[Seg].UnrestNetQtyLimit && g_Limit[Seg].g_NetQtyLimit[ScripCode].SellLimit > -1 && g_Limit[Seg].g_NetQtyLimit[ScripCode].AvailSellLimit < (CurrQty - PrevQty))
                                {
                                    MessageBox.Show("Net Quantity Sell Limit Exceeded", "Capital", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
                                    return false;
                                }

                                if (g_Limit[Seg].UnrestGrpLimit && g_Limit[Seg].g_GroupLimit[Group].SellLimit > -1 && g_Limit[Seg].g_GroupLimit[Group].AvailSellLimit < (CurrValue - PrevValue))
                                {
                                    MessageBox.Show("Group Sell Limit Exceeded", "Capital", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
                                    return false;
                                }

                                if (g_Limit[Seg].NetValue > -1 && g_Limit[Seg].NetValue < ((g_Limit[Seg].SellValue + (CurrValue - PrevValue)) - g_Limit[Seg].CurrNetValue))
                                {
                                    MessageBox.Show("Net Value Limit Exceeded", "Capital", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
                                    return false;
                                }
                            }

                            if (OrderAction == "TRDDNLD")
                            {
                                g_Limit[Seg].AvailGrossSellLimit -= (CurrValue - PrevValue);

                                g_Limit[Seg].g_NetQtyLimit[ScripCode].AvailSellLimit -= (CurrQty - PrevQty);

                                g_Limit[Seg].g_GroupLimit[Group].AvailSellLimit -= (CurrValue - PrevValue);

                                g_Limit[Seg].g_NetQtyLimit[ScripCode].AvailBuyLimit += (CurrQty - PrevQty);

                                g_Limit[Seg].CurrNetValue -= (CurrValue - PrevValue);
                            }
                            else if (OrderAction == "TRDONLINE")
                            {
                                g_Limit[Seg].g_NetQtyLimit[ScripCode].AvailBuyLimit += (CurrQty - PrevQty);

                                g_Limit[Seg].SellValue -= (CurrValue - PrevValue);

                                g_Limit[Seg].CurrNetValue -= (CurrValue - PrevValue);
                            }
                            else if (OrderAction == "CNVTLIMIT")
                            {
                                g_Limit[Seg].AvailGrossSellLimit -= (CurrValue - PrevValue);

                                //if (g_Limit[Seg].UnrestGrpLimit)
                                g_Limit[Seg].g_GroupLimit[Group].AvailSellLimit -= (CurrValue - PrevValue);

                                g_Limit[Seg].SellValue += (CurrValue - PrevValue);
                            }
                            else if (OrderAction == "REJECTED")
                            {
                                g_Limit[Seg].AvailGrossSellLimit += (CurrValue - PrevValue);

                                //if (g_Limit[Seg].UnrestNetQtyLimit && g_Limit[Seg].g_NetQtyLimit[Ord.ScripCode].SellLimit > -1)
                                g_Limit[Seg].g_NetQtyLimit[ScripCode].AvailSellLimit += (CurrQty - PrevQty);

                                //if (g_Limit[Seg].UnrestGrpLimit)
                                g_Limit[Seg].g_GroupLimit[Group].AvailSellLimit += (CurrValue - PrevValue);

                                g_Limit[Seg].SellValue -= (CurrValue - PrevValue);

                            }
                            else
                            {
                                g_Limit[Seg].AvailGrossSellLimit -= (CurrValue - PrevValue);

                                //if (g_Limit[Seg].UnrestNetQtyLimit && g_Limit[Seg].g_NetQtyLimit[Ord.ScripCode].SellLimit > -1)
                                g_Limit[Seg].g_NetQtyLimit[ScripCode].AvailSellLimit -= (CurrQty - PrevQty);

                                //if (g_Limit[Seg].UnrestGrpLimit)
                                g_Limit[Seg].g_GroupLimit[Group].AvailSellLimit -= (CurrValue - PrevValue);

                                g_Limit[Seg].SellValue += (CurrValue - PrevValue);
                            }
                        }
                    }
                    break;

                case 0:
                case 1:
                case 2:
                    {
                        string OrderAction = "", BuySellIndicator = "", Group = "";
                        long ScripCode = 0;
                        int MktLot, QtyMult;
                        int PrevQty = 0, CurrQty = 0;
                        long PrevPricePaisa, CurrPricePaisa;
                        double PrevPrice, CurrPrice;
                        double PrevValue, CurrValue;
                        bool ValidationFlag = false;

                        long StrikePrice;
                        int SpreadType, StrategyId = -1;
                        string OptionType = "";

                        bool ErrorPopUp = false;

                        SpreadLegInfo[] SpreadDet = new SpreadLegInfo[SpreadLegList.Length];
                        for (int n = 0; n < SpreadLegList.Length; n++)
                            SpreadDet[n] = new SpreadLegInfo();

                        int Deci = 2;

                        for (int Leg = 0; Leg < SpreadLegList.Length; Leg++)
                        {
                            MktLot = 1;
                            QtyMult = 1;

                            PrevValue = CurrValue = 0;

                            if (Type == TRDDNLD)
                            {
                                TradeUMS Ord = new TradeUMS();
                                Ord = Obj as TradeUMS;

                                OrderAction = "TRDDNLD";
                                BuySellIndicator = Ord.BSFlag;
                                ScripCode = Ord.ScripCode;
                                QtyMult = (int)GetQuantityMultiplier(ScripCode, "BSE", GetSegmentName(Seg));
                                Deci = GetDecimal((int)ScripCode, "BSE", GetSegmentName(Seg));
                                Group = Ord.ScripGroup;

                                StrikePrice = GetStrikePrice(ScripCode, Seg);
                                SpreadType = GetComplexInstrumentType(ScripCode, Seg);
                                OptionType = GetOptionType(ScripCode, Seg);
                                StrategyId = GetStrategyId(ScripCode, Seg);
                                if (StrategyId == -1)
                                    Leg = SpreadLegList.Length;

                                PrevPricePaisa = 0;
                                CurrPricePaisa = Ord.Rate;

                                PrevQty = 0;
                                CurrQty = Ord.LastQty;

                                if (SpreadType == 1) //Normal
                                {
                                }
                                else //Spread
                                {
                                    if (Leg == (int)SpreadLeg.LEG0)
                                        GetSpreadLegDetails(ScripCode, Seg, BuySellIndicator, OrderAction, ref SpreadDet);

                                    PrevPricePaisa = SpreadDet[Leg].PrevPricePaisa;
                                    CurrPricePaisa = SpreadDet[Leg].CurrPricePaisa;
                                    ScripCode = SpreadDet[Leg].ScripCodeLeg;
                                    BuySellIndicator = SpreadDet[Leg].BuySellIndicator;

                                    Leg = SpreadLegList.Length;
                                }

                                PrevQty = PrevQty * MktLot * QtyMult;
                                PrevPrice = PrevPricePaisa / Math.Pow(10, Deci);
                                PrevValue = PrevPrice * PrevQty;

                                CurrQty = CurrQty * MktLot * QtyMult;
                                CurrPrice = CurrPricePaisa / Math.Pow(10, Deci);
                                CurrValue = CurrPrice * CurrQty;
                            }
                            else if (Type == TRDONLINE)
                            {
                                TradeUMS Ord = new TradeUMS();
                                Ord = Obj as TradeUMS;

                                OrderAction = "TRDONLINE";
                                BuySellIndicator = Ord.BSFlag;
                                ScripCode = Ord.ScripCode;
                                QtyMult = (int)GetQuantityMultiplier(ScripCode, "BSE", GetSegmentName(Seg));
                                Deci = GetDecimal((int)ScripCode, "BSE", GetSegmentName(Seg));
                                //Group = MasterSharedMemory.objMastertxtDictBaseBSE[ScripCode].GroupName;
                                Group = Ord.ScripGroup;

                                StrikePrice = GetStrikePrice(ScripCode, Seg);
                                SpreadType = GetComplexInstrumentType(ScripCode, Seg);
                                OptionType = GetOptionType(ScripCode, Seg);
                                StrategyId = GetStrategyId(ScripCode, Seg);
                                if (StrategyId == -1)
                                    Leg = SpreadLegList.Length;

                                PrevPricePaisa = 0;
                                CurrPricePaisa = Ord.Rate;

                                PrevQty = 0;
                                CurrQty = Ord.LastQty;

                                if (SpreadType == 1) //Normal
                                {
                                }
                                else //Spread
                                {
                                    if (Leg == (int)SpreadLeg.LEG0)
                                        GetSpreadLegDetails(ScripCode, Seg, BuySellIndicator, OrderAction, ref SpreadDet);

                                    PrevPricePaisa = SpreadDet[Leg].PrevPricePaisa;
                                    CurrPricePaisa = SpreadDet[Leg].CurrPricePaisa;
                                    ScripCode = SpreadDet[Leg].ScripCodeLeg;
                                    BuySellIndicator = SpreadDet[Leg].BuySellIndicator;

                                    Leg = SpreadLegList.Length;
                                }

                                if (BuySellIndicator == "S" && (OptionType == "CE" || OptionType == "PE"))
                                    PrevPricePaisa = 0;

                                if (BuySellIndicator == "S" && (OptionType == "CE" || OptionType == "PE"))
                                    CurrPricePaisa = StrikePrice - CurrPricePaisa;

                                PrevQty = PrevQty * MktLot * QtyMult;
                                PrevPrice = PrevPricePaisa / Math.Pow(10, Deci);
                                PrevValue = PrevPrice * PrevQty;

                                CurrQty = CurrQty * MktLot * QtyMult;
                                CurrPrice = CurrPricePaisa / Math.Pow(10, Deci);
                                CurrValue = CurrPrice * CurrQty;
                            }
                            else if (Type == CNVTLIMIT)
                            {
                                EMarketOrderUMS Ord = new EMarketOrderUMS();
                                Ord = Obj as EMarketOrderUMS;

                                OrderAction = "CNVTLIMIT";
                                ScripCode = Ord.ScripCode;
                                QtyMult = (int)GetQuantityMultiplier(ScripCode, "BSE", GetSegmentName(Seg));
                                Deci = GetDecimal((int)ScripCode, "BSE", GetSegmentName(Seg));
                                Group = GetGroupName(ScripCode, "BSE", GetSegmentName(Seg));

                                StrikePrice = GetStrikePrice(ScripCode, Seg);
                                SpreadType = GetComplexInstrumentType(ScripCode, Seg);
                                OptionType = GetOptionType(ScripCode, Seg);
                                StrategyId = GetStrategyId(ScripCode, Seg);
                                if (StrategyId == -1)
                                    Leg = SpreadLegList.Length;

                                string key = string.Format("{0}_{1}", Ord.ScripCode, Ord.OrderId);
                                if (SharedMemories.MemoryManager.OrderDictionary.ContainsKey(key))
                                {
                                    OrderModel OrdModel = SharedMemories.MemoryManager.OrderDictionary[key];
                                    if (OrdModel != null)
                                    {
                                        BuySellIndicator = OrdModel.BuySellIndicator;

                                        //PrevPricePaisa = OrdModel.Price;
                                        PrevPricePaisa = GetPrice(ScripCode, BuySellIndicator);
                                        CurrPricePaisa = Ord.CnvtdRate;

                                        PrevQty = OrdModel.PendingQuantity;
                                        CurrQty = OrdModel.OriginalQty;

                                        if (SpreadType == 1) //Normal
                                        {
                                        }
                                        else //Spread
                                        {
                                            if (Leg == (int)SpreadLeg.LEG0)
                                                GetSpreadLegDetails(ScripCode, Seg, BuySellIndicator, OrderAction, ref SpreadDet);

                                            PrevPricePaisa = SpreadDet[Leg].PrevPricePaisa;
                                            CurrPricePaisa = SpreadDet[Leg].CurrPricePaisa;
                                            ScripCode = SpreadDet[Leg].ScripCodeLeg;
                                            BuySellIndicator = SpreadDet[Leg].BuySellIndicator;
                                        }

                                        if (BuySellIndicator == "S" && (OptionType == "CE" || OptionType == "PE"))
                                            PrevPricePaisa = 0;

                                        if (BuySellIndicator == "S" && (OptionType == "CE" || OptionType == "PE"))
                                            CurrPricePaisa = StrikePrice - CurrPricePaisa;

                                        PrevQty = PrevQty * MktLot * QtyMult;
                                        PrevPrice = PrevPricePaisa / Math.Pow(10, Deci);
                                        PrevValue = PrevPrice * PrevQty;

                                        CurrQty = CurrQty * MktLot * QtyMult;
                                        CurrPrice = CurrPricePaisa / Math.Pow(10, Deci);
                                        CurrValue = CurrPrice * CurrQty;

                                        ValidationFlag = false;
                                    }
                                }
                            }
                            else if (Type == IOCACTION)
                            {
                                EIOCCancelOrderUMS Ord = new EIOCCancelOrderUMS();
                                Ord = Obj as EIOCCancelOrderUMS;

                                OrderAction = "IOCACTION";
                                ScripCode = Ord.ScripCode;
                                QtyMult = (int)GetQuantityMultiplier(ScripCode, "BSE", GetSegmentName(Seg));
                                Deci = GetDecimal((int)ScripCode, "BSE", GetSegmentName(Seg));
                                Group = GetGroupName(ScripCode, "BSE", GetSegmentName(Seg));

                                StrikePrice = GetStrikePrice(ScripCode, Seg);
                                SpreadType = GetComplexInstrumentType(ScripCode, Seg);
                                OptionType = GetOptionType(ScripCode, Seg);
                                StrategyId = GetStrategyId(ScripCode, Seg);
                                if (StrategyId == -1)
                                    Leg = SpreadLegList.Length;

                                string key = string.Format("{0}_{1}", Ord.ScripCode, Ord.OrderId);
                                if (SharedMemories.MemoryManager.OrderDictionary.ContainsKey(key))
                                {
                                    OrderModel OrdModel = SharedMemories.MemoryManager.OrderDictionary[key];
                                    if (OrdModel != null)
                                    {
                                        BuySellIndicator = OrdModel.BuySellIndicator;

                                        PrevPricePaisa = 0;
                                        CurrPricePaisa = OrdModel.Price;

                                        if (OrdModel.OrderType == "G")
                                            CurrPricePaisa = GetPrice(ScripCode, BuySellIndicator);

                                        PrevQty = 0;
                                        CurrQty = Ord.KilledQty;

                                        if (SpreadType == 1) //Normal
                                        {
                                        }
                                        else //Spread
                                        {
                                            if (Leg == (int)SpreadLeg.LEG0)
                                                GetSpreadLegDetails(ScripCode, Seg, BuySellIndicator, OrderAction, ref SpreadDet);

                                            PrevPricePaisa = SpreadDet[Leg].PrevPricePaisa;
                                            CurrPricePaisa = SpreadDet[Leg].CurrPricePaisa;
                                            ScripCode = SpreadDet[Leg].ScripCodeLeg;
                                            BuySellIndicator = SpreadDet[Leg].BuySellIndicator;
                                        }

                                        if (BuySellIndicator == "S" && (OptionType == "CE" || OptionType == "PE"))
                                            PrevPricePaisa = 0;

                                        if (BuySellIndicator == "S" && (OptionType == "CE" || OptionType == "PE"))
                                            CurrPricePaisa = StrikePrice - CurrPricePaisa;

                                        PrevQty = PrevQty * MktLot * QtyMult;
                                        PrevPrice = PrevPricePaisa / Math.Pow(10, Deci);
                                        PrevValue = PrevPrice * PrevQty;

                                        CurrQty = -(CurrQty * MktLot * QtyMult);
                                        CurrPrice = CurrPricePaisa / Math.Pow(10, Deci);
                                        CurrValue = CurrPrice * CurrQty;

                                        ValidationFlag = false;
                                    }
                                }
                            }
                            else
                            {
                                OrderModel Ord = new OrderModel();
                                Ord = Obj as OrderModel;

                                OrderAction = Ord.OrderAction;
                                if (Type == REJECTED || Type == ORDDNLD)
                                    OrderAction = null;

                                BuySellIndicator = Ord.BuySellIndicator;
                                ScripCode = Ord.ScripCode;
                                QtyMult = (int)GetQuantityMultiplier(ScripCode, "BSE", GetSegmentName(Seg));
                                Deci = GetDecimal((int)ScripCode, "BSE", GetSegmentName(Seg));
                                Group = Ord.Group;
                                if (Group == null)
                                    Group = GetGroupName(ScripCode, "BSE", GetSegmentName(Seg));

                                StrikePrice = GetStrikePrice(ScripCode, Seg);
                                SpreadType = GetComplexInstrumentType(ScripCode, Seg);
                                OptionType = GetOptionType(ScripCode, Seg);
                                StrategyId = GetStrategyId(ScripCode, Seg);
                                if (StrategyId == -1)
                                    Leg = SpreadLegList.Length;

                                if (OrderAction == "A")
                                {
                                    PrevPricePaisa = 0;
                                    CurrPricePaisa = Ord.Price;

                                    if (Ord.OrderType == "G")
                                        CurrPricePaisa = GetPrice(ScripCode, BuySellIndicator);
                                    else if (Ord.OrderType == "P")
                                        CurrPricePaisa = Ord.TriggerPrice;

                                    PrevQty = 0;
                                    CurrQty = Ord.OriginalQty;

                                    if (SpreadType == 1) //Normal
                                    {
                                    }
                                    else //Spread
                                    {
                                        if (Leg == (int)SpreadLeg.LEG0)
                                            GetSpreadLegDetails(ScripCode, Seg, BuySellIndicator, OrderAction, ref SpreadDet);

                                        if (SpreadDet[(int)SpreadLeg.LEG1].CurrPricePaisa == 0)
                                        {
                                            if (Type == ORDAUD)
                                            {
                                                string ReplyMsg = "Open Best 5 Window for " + SpreadDet[(int)SpreadLeg.LEG1].ScripIdLeg + "/" + SpreadDet[(int)SpreadLeg.LEG1].ScripCodeLeg;
                                                if (Ord.IsBatchOrder == true)
                                                {
                                                    Ord.ReplyText = ReplyMsg;
                                                    ((OrderModel)BatchOrderVM.BatchOrderQueue[Ord.BatchOrderQueueIndex]).ReplyText = ReplyMsg;
                                                }
                                                else
                                                    MessageBox.Show(ReplyMsg, "Message", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
                                            }
                                            ValidationFlag = false;
                                            ErrorPopUp = true;
                                            return false;
                                        }

                                        if (SpreadDet[(int)SpreadLeg.LEG2].CurrPricePaisa == 0)
                                        {
                                            if (Type == ORDAUD)
                                            {
                                                string ReplyMsg = "Open Best 5 Window for " + SpreadDet[(int)SpreadLeg.LEG2].ScripIdLeg + " / " + SpreadDet[(int)SpreadLeg.LEG2].ScripCodeLeg;
                                                if (Ord.IsBatchOrder == true)
                                                {
                                                    Ord.ReplyText = ReplyMsg;
                                                    ((OrderModel)BatchOrderVM.BatchOrderQueue[Ord.BatchOrderQueueIndex]).ReplyText = ReplyMsg;
                                                }
                                                else
                                                    MessageBox.Show(ReplyMsg, "Message", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
                                            }
                                            ValidationFlag = false;
                                            ErrorPopUp = true;
                                            return false;
                                        }

                                        PrevPricePaisa = SpreadDet[Leg].PrevPricePaisa;
                                        CurrPricePaisa = SpreadDet[Leg].CurrPricePaisa;
                                        ScripCode = SpreadDet[Leg].ScripCodeLeg;
                                        BuySellIndicator = SpreadDet[Leg].BuySellIndicator;
                                    }

                                    if (BuySellIndicator == "S" && (OptionType == "CE" || OptionType == "PE"))
                                        PrevPricePaisa = 0;

                                    if (BuySellIndicator == "S" && (OptionType == "CE" || OptionType == "PE"))
                                        CurrPricePaisa = StrikePrice - CurrPricePaisa;

                                    PrevQty = PrevQty * MktLot * QtyMult;
                                    PrevPrice = PrevPricePaisa / Math.Pow(10, Deci);
                                    PrevValue = PrevPrice * PrevQty;

                                    CurrQty = CurrQty * MktLot * QtyMult;
                                    CurrPrice = CurrPricePaisa / Math.Pow(10, Deci);
                                    CurrValue = CurrPrice * CurrQty;

                                    ValidationFlag = true;
                                }
                                else if (OrderAction == "U")
                                {
                                    OrderModel tOrd = new OrderModel();
                                    OrderModel tOrdIncr = new OrderModel();

                                    if (Type == REJECTED)
                                    {
                                        tOrd = MemoryManager.OrderDictionaryBackupMemory[Ord.OrderKey];

                                        if (MemoryManager.DummyOrderDictionary.ContainsKey(Ord.MessageTag))
                                            MemoryManager.DummyOrderDictionary.TryGetValue(Ord.MessageTag, out tOrdIncr);
                                    }
                                    else if (Type == LIMITVIOLATE)
                                        MemoryManager.OrderDictionary.TryGetValue(Ord.OrderKey, out tOrd);
                                    else
                                        MemoryManager.OrderDictionary.TryGetValue(Ord.OrderKey, out tOrd);

                                    PrevPricePaisa = tOrd.Price;
                                    CurrPricePaisa = Ord.Price;

                                    if (tOrd.OrderType == "G")
                                        PrevPricePaisa = GetPrice(ScripCode, BuySellIndicator);

                                    if (Ord.OrderType == "G")
                                        CurrPricePaisa = GetPrice(ScripCode, BuySellIndicator);

                                    if (Type == REJECTED)
                                    {
                                        PrevQty = tOrd.PendingQuantity;
                                        CurrQty = Ord.OriginalQty + tOrd.PendingQuantity;
                                    }
                                    else
                                    {
                                        PrevQty = tOrd.PendingQuantity;
                                        CurrQty = Ord.OriginalQty;
                                    }

                                    if (SpreadType == 1) //Normal
                                    {
                                    }
                                    else //Spread
                                    {
                                        if (Leg == (int)SpreadLeg.LEG0)
                                            GetSpreadLegDetails(ScripCode, Seg, BuySellIndicator, OrderAction, ref SpreadDet);

                                        if (SpreadDet[(int)SpreadLeg.LEG1].CurrPricePaisa == 0)
                                        {
                                            if (Type == ORDAUD)
                                            {
                                                string ReplyMsg = "Open Best 5 Window for " + SpreadDet[(int)SpreadLeg.LEG1].ScripIdLeg + "/" + SpreadDet[(int)SpreadLeg.LEG1].ScripCodeLeg;
                                                if (Ord.IsBatchOrder == true)
                                                {
                                                    Ord.ReplyText = ReplyMsg;
                                                    ((OrderModel)BatchOrderVM.BatchOrderQueue[Ord.BatchOrderQueueIndex]).ReplyText = ReplyMsg;
                                                }
                                                else
                                                    MessageBox.Show(ReplyMsg, "Message", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
                                            }
                                            ValidationFlag = false;
                                            ErrorPopUp = true;
                                            return false;
                                        }

                                        if (SpreadDet[(int)SpreadLeg.LEG2].CurrPricePaisa == 0)
                                        {
                                            if (Type == ORDAUD)
                                            {
                                                string ReplyMsg = "Open Best 5 Window for " + SpreadDet[(int)SpreadLeg.LEG2].ScripIdLeg + " / " + SpreadDet[(int)SpreadLeg.LEG2].ScripCodeLeg;
                                                if (Ord.IsBatchOrder == true)
                                                {
                                                    Ord.ReplyText = ReplyMsg;
                                                    ((OrderModel)BatchOrderVM.BatchOrderQueue[Ord.BatchOrderQueueIndex]).ReplyText = ReplyMsg;
                                                }
                                                else
                                                    MessageBox.Show(ReplyMsg, "Message", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
                                            }
                                            ValidationFlag = false;
                                            ErrorPopUp = true;
                                            return false;
                                        }

                                        PrevPricePaisa = SpreadDet[Leg].PrevPricePaisa;
                                        CurrPricePaisa = SpreadDet[Leg].CurrPricePaisa;
                                        ScripCode = SpreadDet[Leg].ScripCodeLeg;
                                        BuySellIndicator = SpreadDet[Leg].BuySellIndicator;
                                    }

                                    if (BuySellIndicator == "S" && (OptionType == "CE" || OptionType == "PE"))
                                        PrevPricePaisa = StrikePrice - PrevPricePaisa;

                                    if (BuySellIndicator == "S" && (OptionType == "CE" || OptionType == "PE"))
                                        CurrPricePaisa = StrikePrice - CurrPricePaisa;

                                    PrevQty = PrevQty * MktLot * QtyMult;
                                    PrevPrice = PrevPricePaisa / Math.Pow(10, Deci);
                                    PrevValue = PrevPrice * PrevQty;

                                    CurrQty = CurrQty * MktLot * QtyMult;
                                    CurrPrice = CurrPricePaisa / Math.Pow(10, Deci);
                                    CurrValue = CurrPrice * CurrQty;

                                    ValidationFlag = true;
                                }
                                else if (OrderAction == "D")
                                {
                                    PrevPricePaisa = 0;
                                    CurrPricePaisa = Ord.Price;

                                    if (Ord.OrderType == "G")
                                        CurrPricePaisa = GetPrice(ScripCode, BuySellIndicator);
                                    else if (Ord.OrderType == "P")
                                        CurrPricePaisa = Ord.TriggerPrice;

                                    PrevQty = 0;
                                    CurrQty = Ord.PendingQuantity;

                                    if (SpreadType == 1) //Normal
                                    {
                                    }
                                    else //Spread
                                    {
                                        if (Leg == (int)SpreadLeg.LEG0)
                                            GetSpreadLegDetails(ScripCode, Seg, BuySellIndicator, OrderAction, ref SpreadDet);

                                        if (SpreadDet[(int)SpreadLeg.LEG1].CurrPricePaisa == 0)
                                        {
                                            if (Type == ORDAUD)
                                            {
                                                string ReplyMsg = "Open Best 5 Window for " + SpreadDet[(int)SpreadLeg.LEG1].ScripIdLeg + "/" + SpreadDet[(int)SpreadLeg.LEG1].ScripCodeLeg;
                                                if (Ord.IsBatchOrder == true)
                                                {
                                                    Ord.ReplyText = ReplyMsg;
                                                    ((OrderModel)BatchOrderVM.BatchOrderQueue[Ord.BatchOrderQueueIndex]).ReplyText = ReplyMsg;
                                                }
                                                else
                                                    MessageBox.Show(ReplyMsg, "Message", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
                                            }
                                            ValidationFlag = false;
                                            ErrorPopUp = true;
                                            return false;
                                        }

                                        if (SpreadDet[(int)SpreadLeg.LEG2].CurrPricePaisa == 0)
                                        {
                                            if (Type == ORDAUD)
                                            {
                                                string ReplyMsg = "Open Best 5 Window for " + SpreadDet[(int)SpreadLeg.LEG2].ScripIdLeg + " / " + SpreadDet[(int)SpreadLeg.LEG2].ScripCodeLeg;
                                                if (Ord.IsBatchOrder == true)
                                                {
                                                    Ord.ReplyText = ReplyMsg;
                                                    ((OrderModel)BatchOrderVM.BatchOrderQueue[Ord.BatchOrderQueueIndex]).ReplyText = ReplyMsg;
                                                }
                                                else
                                                    MessageBox.Show(ReplyMsg, "Message", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
                                            }
                                            ValidationFlag = false;
                                            ErrorPopUp = true;
                                            return false;
                                        }

                                        PrevPricePaisa = SpreadDet[Leg].PrevPricePaisa;
                                        CurrPricePaisa = SpreadDet[Leg].CurrPricePaisa;
                                        ScripCode = SpreadDet[Leg].ScripCodeLeg;
                                        BuySellIndicator = SpreadDet[Leg].BuySellIndicator;
                                    }

                                    if (BuySellIndicator == "S" && (OptionType == "CE" || OptionType == "PE"))
                                        PrevPricePaisa = 0;

                                    if (BuySellIndicator == "S" && (OptionType == "CE" || OptionType == "PE"))
                                        CurrPricePaisa = StrikePrice - CurrPricePaisa;

                                    PrevQty = PrevQty * MktLot * QtyMult;
                                    PrevPrice = PrevPricePaisa / Math.Pow(10, Deci);
                                    PrevValue = PrevPrice * PrevQty;

                                    CurrQty = -(CurrQty * MktLot * QtyMult);
                                    CurrPrice = CurrPricePaisa / Math.Pow(10, Deci);
                                    CurrValue = CurrPrice * CurrQty;
                                }
                                else
                                {
                                    PrevPricePaisa = 0;
                                    CurrPricePaisa = Ord.Price;

                                    if (Ord.OrderType == "G")
                                        CurrPricePaisa = GetPrice(ScripCode, BuySellIndicator);
                                    else if (Ord.OrderType == "P")
                                        CurrPricePaisa = Ord.TriggerPrice;

                                    PrevQty = 0;
                                    CurrQty = Ord.PendingQuantity;

                                    if (SpreadType == 1) //Normal
                                    {
                                    }
                                    else //Spread
                                    {
                                        if (Leg == (int)SpreadLeg.LEG0)
                                            GetSpreadLegDetails(ScripCode, Seg, BuySellIndicator, OrderAction, ref SpreadDet);

                                        PrevPricePaisa = SpreadDet[Leg].PrevPricePaisa;
                                        CurrPricePaisa = SpreadDet[Leg].CurrPricePaisa;
                                        ScripCode = SpreadDet[Leg].ScripCodeLeg;
                                        BuySellIndicator = SpreadDet[Leg].BuySellIndicator;
                                    }

                                    if (BuySellIndicator == "S" && (OptionType == "CE" || OptionType == "PE"))
                                        PrevPricePaisa = 0;

                                    if (BuySellIndicator == "S" && (OptionType == "CE" || OptionType == "PE"))
                                        CurrPricePaisa = StrikePrice - CurrPricePaisa;

                                    PrevQty = PrevQty * MktLot * QtyMult;
                                    PrevPrice = PrevPricePaisa / Math.Pow(10, Deci);
                                    PrevValue = PrevPrice * PrevQty;

                                    CurrQty = CurrQty * MktLot * QtyMult;
                                    CurrPrice = CurrPricePaisa / Math.Pow(10, Deci);
                                    CurrValue = CurrPrice * CurrQty;
                                }
                            }

                            if (Type == REJECTED || Type == LIMITVIOLATE)
                            {
                                ValidationFlag = false;

                                OrderAction = "REJECTED";

                            }



                            if (BuySellIndicator == "B")
                            {
                                if (ValidationFlag == true)
                                {
                                    if (ErrorPopUp == false && g_Limit[Seg].AvailGrossBuyLimit < (CurrValue - PrevValue))
                                    {
                                        MessageBox.Show("Gross Buy Limit Exceeded", "Capital", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
                                        ErrorPopUp = true;
                                    }

                                    if (ErrorPopUp == false && g_Limit[Seg].UnrestNetQtyLimit && g_Limit[Seg].g_NetQtyLimit[ScripCode].BuyLimit > -1 && g_Limit[Seg].g_NetQtyLimit[ScripCode].AvailBuyLimit < (CurrQty - PrevQty))
                                    {
                                        MessageBox.Show("Net Quantity Buy Limit Exceeded", "Capital", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
                                        ErrorPopUp = true;
                                    }

                                    if (ErrorPopUp == false && g_Limit[Seg].UnrestGrpLimit && g_Limit[Seg].g_GroupLimit[Group].BuyLimit > -1 && g_Limit[Seg].g_GroupLimit[Group].AvailBuyLimit < (CurrValue - PrevValue))
                                    {
                                        MessageBox.Show("Group Buy Limit Exceeded", "Capital", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
                                        ErrorPopUp = true;
                                    }

                                    if (ErrorPopUp == false && g_Limit[Seg].NetValue > -1 && g_Limit[Seg].NetValue < ((g_Limit[Seg].BuyValue + (CurrValue - PrevValue)) + g_Limit[Seg].CurrNetValue))
                                    {
                                        MessageBox.Show("Net Value Limit Exceeded", "Capital", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
                                        ErrorPopUp = true;
                                    }
                                }


                                if (OrderAction == "TRDDNLD")
                                {
                                    g_Limit[Seg].AvailGrossBuyLimit -= (CurrValue - PrevValue);

                                    g_Limit[Seg].g_NetQtyLimit[ScripCode].AvailBuyLimit -= (CurrQty - PrevQty);

                                    g_Limit[Seg].g_GroupLimit[Group].AvailBuyLimit -= (CurrValue - PrevValue);

                                    g_Limit[Seg].g_NetQtyLimit[ScripCode].AvailSellLimit += (CurrQty - PrevQty);

                                    g_Limit[Seg].CurrNetValue += (CurrValue - PrevValue);
                                }
                                else if (OrderAction == "TRDONLINE")
                                {
                                    g_Limit[Seg].g_NetQtyLimit[ScripCode].AvailSellLimit += (CurrQty - PrevQty);

                                    g_Limit[Seg].BuyValue -= (CurrValue - PrevValue);

                                    g_Limit[Seg].CurrNetValue += (CurrValue - PrevValue);
                                }
                                else if (OrderAction == "CNVTLIMIT")
                                {
                                    g_Limit[Seg].AvailGrossBuyLimit -= (CurrValue - PrevValue);

                                    //if (g_Limit[Seg].UnrestGrpLimit)
                                    g_Limit[Seg].g_GroupLimit[Group].AvailBuyLimit -= (CurrValue - PrevValue);

                                    g_Limit[Seg].BuyValue += (CurrValue - PrevValue);
                                }
                                else if (OrderAction == "REJECTED")
                                {
                                    g_Limit[Seg].AvailGrossBuyLimit += (CurrValue - PrevValue);

                                    //if (g_Limit[Seg].UnrestNetQtyLimit && g_Limit[Seg].g_NetQtyLimit[Ord.ScripCode].BuyLimit > -1)
                                    g_Limit[Seg].g_NetQtyLimit[ScripCode].AvailBuyLimit += (CurrQty - PrevQty);

                                    //if (g_Limit[Seg].UnrestGrpLimit)
                                    g_Limit[Seg].g_GroupLimit[Group].AvailBuyLimit += (CurrValue - PrevValue);

                                    g_Limit[Seg].BuyValue -= (CurrValue - PrevValue);

                                }
                                else
                                {
                                    g_Limit[Seg].AvailGrossBuyLimit -= (CurrValue - PrevValue);

                                    //if (g_Limit[Seg].UnrestNetQtyLimit && g_Limit[Seg].g_NetQtyLimit[Ord.ScripCode].BuyLimit > -1)
                                    g_Limit[Seg].g_NetQtyLimit[ScripCode].AvailBuyLimit -= (CurrQty - PrevQty);

                                    //if (g_Limit[Seg].UnrestGrpLimit)
                                    g_Limit[Seg].g_GroupLimit[Group].AvailBuyLimit -= (CurrValue - PrevValue);

                                    g_Limit[Seg].BuyValue += (CurrValue - PrevValue);
                                }
                            }
                            else if (BuySellIndicator == "S")
                            {
                                if (ValidationFlag == true)
                                {
                                    if (ErrorPopUp == false && g_Limit[Seg].AvailGrossSellLimit < (CurrValue - PrevValue))
                                    {
                                        MessageBox.Show("Gross Sell Limit Exceeded", "Capital", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
                                        ErrorPopUp = true;
                                    }

                                    if (ErrorPopUp == false && g_Limit[Seg].UnrestNetQtyLimit && g_Limit[Seg].g_NetQtyLimit[ScripCode].SellLimit > -1 && g_Limit[Seg].g_NetQtyLimit[ScripCode].AvailSellLimit < (CurrQty - PrevQty))
                                    {
                                        MessageBox.Show("Net Quantity Sell Limit Exceeded", "Capital", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
                                        ErrorPopUp = true;
                                    }

                                    if (ErrorPopUp == false && g_Limit[Seg].UnrestGrpLimit && g_Limit[Seg].g_GroupLimit[Group].SellLimit > -1 && g_Limit[Seg].g_GroupLimit[Group].AvailSellLimit < (CurrValue - PrevValue))
                                    {
                                        MessageBox.Show("Group Sell Limit Exceeded", "Capital", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
                                        ErrorPopUp = true;
                                    }

                                    if (ErrorPopUp == false && g_Limit[Seg].NetValue > -1 && g_Limit[Seg].NetValue < ((g_Limit[Seg].SellValue + (CurrValue - PrevValue)) - g_Limit[Seg].CurrNetValue))
                                    {
                                        MessageBox.Show("Net Value Limit Exceeded", "Capital", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
                                        ErrorPopUp = true;
                                    }
                                }

                                if (OrderAction == "TRDDNLD")
                                {
                                    g_Limit[Seg].AvailGrossSellLimit -= (CurrValue - PrevValue);

                                    g_Limit[Seg].g_NetQtyLimit[ScripCode].AvailSellLimit -= (CurrQty - PrevQty);

                                    g_Limit[Seg].g_GroupLimit[Group].AvailSellLimit -= (CurrValue - PrevValue);

                                    g_Limit[Seg].g_NetQtyLimit[ScripCode].AvailBuyLimit += (CurrQty - PrevQty);

                                    g_Limit[Seg].CurrNetValue -= (CurrValue - PrevValue);
                                }
                                else if (OrderAction == "TRDONLINE")
                                {
                                    g_Limit[Seg].g_NetQtyLimit[ScripCode].AvailBuyLimit += (CurrQty - PrevQty);

                                    g_Limit[Seg].SellValue -= (CurrValue - PrevValue);

                                    g_Limit[Seg].CurrNetValue -= (CurrValue - PrevValue);
                                }
                                else if (OrderAction == "CNVTLIMIT")
                                {
                                    g_Limit[Seg].AvailGrossSellLimit -= (CurrValue - PrevValue);

                                    //if (g_Limit[Seg].UnrestGrpLimit)
                                    g_Limit[Seg].g_GroupLimit[Group].AvailSellLimit -= (CurrValue - PrevValue);

                                    g_Limit[Seg].SellValue += (CurrValue - PrevValue);
                                }
                                else if (OrderAction == "REJECTED")
                                {
                                    g_Limit[Seg].AvailGrossSellLimit += (CurrValue - PrevValue);

                                    //if (g_Limit[Seg].UnrestNetQtyLimit && g_Limit[Seg].g_NetQtyLimit[Ord.ScripCode].SellLimit > -1)
                                    g_Limit[Seg].g_NetQtyLimit[ScripCode].AvailSellLimit += (CurrQty - PrevQty);

                                    //if (g_Limit[Seg].UnrestGrpLimit)
                                    g_Limit[Seg].g_GroupLimit[Group].AvailSellLimit += (CurrValue - PrevValue);

                                    g_Limit[Seg].SellValue -= (CurrValue - PrevValue);

                                }
                                else
                                {
                                    g_Limit[Seg].AvailGrossSellLimit -= (CurrValue - PrevValue);

                                    //if (g_Limit[Seg].UnrestNetQtyLimit && g_Limit[Seg].g_NetQtyLimit[Ord.ScripCode].SellLimit > -1)
                                    g_Limit[Seg].g_NetQtyLimit[ScripCode].AvailSellLimit -= (CurrQty - PrevQty);

                                    //if (g_Limit[Seg].UnrestGrpLimit)
                                    g_Limit[Seg].g_GroupLimit[Group].AvailSellLimit -= (CurrValue - PrevValue);

                                    g_Limit[Seg].SellValue += (CurrValue - PrevValue);
                                }
                            }

                            if ((BuySellIndicator == "B") || (BuySellIndicator == "S"))
                                UpdateGroupLimitScreen(Group, g_Limit[Seg], false, Seg);
                        }
                        if (ErrorPopUp == true)
                            return false;
                    }
                    break;
            }

            return true;
        }


        public static bool ValidateLimit(string Exchange, int Mode, int Type, string Segment, object Obj)
        {
            string Route = Exchange + Mode.ToString();
            switch (Route)
            {
                case "BSE0":
                    {
                        int Seg = GetSegment(Segment);

                        if (!CalculateGrossValue(Seg, Type, Obj))
                        {
                            CalculateGrossValue(Seg, LIMITVIOLATE, Obj);
                            return false;
                        }
                    }
                    break;

            }
            return true;
        }

        public int index = 0;
        public static int ErrorCount = 0;
        public static int WarningCount = 0;

        #region LocalMemory
        //private static ObservableCollection<CommonFieldsForReadingCSV> _SuccessCollection = new ObservableCollection<CommonFieldsForReadingCSV>();
        //public static ObservableCollection<CommonFieldsForReadingCSV> SuccessCollection
        //{
        //    get { return _SuccessCollection; }
        //    set
        //    {
        //        _SuccessCollection = value;
        //        //  NotifyPropertyChanged("SuccessCollection");
        //    }
        //}

        private static ObservableCollection<OrderModel> _SuccessCollection = new ObservableCollection<OrderModel>();
        public static ObservableCollection<OrderModel> SuccessCollection
        {
            get { return _SuccessCollection; }
            set
            {
                _SuccessCollection = value;
                //  NotifyPropertyChanged("SuccessCollection");
            }
        }

        private static ObservableCollection<string> _ErrorCollection = new ObservableCollection<string>();
        public static ObservableCollection<string> ErrorCollection
        {
            get { return _ErrorCollection; }
            set
            {
                _ErrorCollection = value;
                //  NotifyPropertyChanged("SuccessCollection");
            }
        }
        #endregion

        public CommonFunctions()
        {
            oDataAccessLayer = new DataAccessLayer();
            oDataAccessLayer.Connect((int)DataAccessLayer.ConnectionDB.Masters);


        }
        public static DataAccessLayer oDataAccessLayer;
        /// <summary>
        /// Get LTP From Broadcast
        /// Gaurav 31/01/2017
        /// </summary>
        /// <param name="Scripcode">Scripcode for which LTP needs to be return</param>
        /// <returns>Returns LTP, Type: Double</returns>
        public static string GetLTPBCast(int Scripcode, string IntrTypeSelected)
        {
            //TODO: getEquity scrip code from asset code
            string LTPPrice = string.Empty;
            string segmentName = string.Empty;
            try
            {

                if ((IntrTypeSelected == "FutStock" || IntrTypeSelected == "CallStock" || IntrTypeSelected == "PutStock"))
                {
                    int segment = CommonFunctions.GetSegmentFromScripCode(Scripcode);
                    if (segment == 1)
                    {
                        segmentName = "Equity";
                    }
                    else if (segment == 2)
                    {
                        segmentName = "Derivative";
                    }
                    else if (segment == 3)
                    {
                        segmentName = "Currency";
                    }
                    int DecimalPoint = CommonFunctions.GetDecimal(Scripcode, "BSE", segmentName);
                    BroadcastReceiver.ScripDetails Br = BroadcastMaster.BroadcastMasterMemory.objScripDetailsConDict.Where(x => x.Key == Scripcode).Select(x => x.Value).FirstOrDefault() == null ? new BroadcastReceiver.ScripDetails() : BroadcastMaster.BroadcastMasterMemory.objScripDetailsConDict.Where(x => x.Key == Scripcode).Select(x => x.Value).FirstOrDefault();
                    ScripDetails objScripDetails = new ScripDetails();
                    objScripDetails = MainWindowVM.UpdateScripDataFromMemory(Br);
                    objScripDetails.ScriptCode_BseToken_NseToken = Scripcode;
                    if (objScripDetails.NoOfTrades > 0)
                    {
                        if (DecimalPoint == 4)
                        {
                            LTPPrice = string.Format("{0:0.0000}", CommonFunctions.GetValueInDecimal(objScripDetails.lastTradeRateL, DecimalPoint) == "0" ? "" : CommonFunctions.GetValueInDecimal(objScripDetails.lastTradeRateL, DecimalPoint));
                        }
                        else
                        {
                            LTPPrice = string.Format("{0:0.00}", CommonFunctions.GetValueInDecimal(objScripDetails.lastTradeRateL, DecimalPoint) == "0" ? "" : CommonFunctions.GetValueInDecimal(objScripDetails.lastTradeRateL, DecimalPoint));
                        }
                        //LTPPrice = CommonFunctions.GetValueInDecimal(objScripDetails.lastTradeRateL, DecimalPoint) == "0" ? "" : CommonFunctions.GetValueInDecimal(objScripDetails.lastTradeRateL, DecimalPoint);
                    }

                }
                else if (IntrTypeSelected == "FutIndex" || IntrTypeSelected == "CallIndex" || IntrTypeSelected == "PutIndex")
                {
                    if (SharedMemories.BroadcastMasterMemory.objIndexDetailsConDict.ContainsKey(Scripcode))
                        LTPPrice = string.Format("{0:0.00}", SharedMemories.BroadcastMasterMemory.objIndexDetailsConDict[Scripcode].IndexValue / Math.Pow(10, 2));
                    //LTPPrice = Convert.ToString(SharedMemories.BroadcastMasterMemory.objIndexDetailsConDict[Scripcode].IndexValue / Math.Pow(10, 2));
                }

            }
            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
            }
            return LTPPrice;
        }

        public static string GetLTPBCast(int Scripcode)
        {
            string LTP = "0";
            try
            {
                if (Scripcode > 0)
                {
                    var segment = CommonFunctions.GetSegmentID(Scripcode);
                    var decimalpoint = CommonFunctions.GetDecimal(Scripcode, "BSE", segment);
                    if (BroadcastMaster.BroadcastMasterMemory.objScripDetailsConDict.ContainsKey(Scripcode))
                    {
                        if (decimalpoint == 4)
                        {
                            LTP = string.Format("0:0.00", (BroadcastMaster.BroadcastMasterMemory.objScripDetailsConDict[Scripcode].LastTradeRate_l) / Math.Pow(10, decimalpoint));
                        }
                        else
                        {
                            LTP = string.Format("0:0.0000", (BroadcastMaster.BroadcastMasterMemory.objScripDetailsConDict[Scripcode].LastTradeRate_l) / Math.Pow(10, decimalpoint));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
            }
            return LTP;
        }

        /// <summary>
        /// Get Asset token Number
        /// </summary>
        /// <param name="ContractTokenNumber"></param>
        /// <param name="segment"></param>
        /// <param name="InstrumentType"></param>
        /// <returns></returns>
        public static long GetAssetTokenNumber(long ContractTokenNumber, string segment, string InstrumentType)
        {
            long strAssetTokenNumber = -1;
            try
            {
                if (segment == "Derivative" && (InstrumentType == "FutStock" || InstrumentType == "CallStock" || InstrumentType == "PutStock"))
                {
                    var data = MasterSharedMemory.objMasterDerivativeDictBaseBSE[ContractTokenNumber];
                    if (data != null)
                    {
                        strAssetTokenNumber = MasterSharedMemory.objMasterDerivativeDictBaseBSE[ContractTokenNumber].AssestTokenNum;
                    }
                }
                else if (segment == "Derivative" && (InstrumentType == "FutIndex" || InstrumentType == "CallIndex" || InstrumentType == "PutIndex"))
                {
                    var data = MasterSharedMemory.objMasterDerivativeDictBaseBSE[ContractTokenNumber];
                    if (data != null)
                    {
                        strAssetTokenNumber = MasterSharedMemory.objMasterDerivativeDictBaseBSE[ContractTokenNumber].AssestTokenNum;
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
            }
            return strAssetTokenNumber;
        }

        /// <summary>
        /// Compare Date
        /// </summary>
        /// <param name="CompareDate1">Date in Format dd-MM-yyyy</param>
        /// <param name="CompareDate2">Date in Format dd-MM-yyyy</param>
        /// <returns></returns>
        /// 
        public static int CompareDate(string CompareDate1, string CompareDate2)
        {
            int result = 0;
            try
            {
                //Common Date Format "dd-MM-yyyy"
                int lengthDayOrMonth = 2;
                int lengthYear = 4;
                string CompareDate1Year = string.Empty;
                string CompareDate2Year = string.Empty;
                string CompareDate1Month = string.Empty;
                string CompareDate2Month = string.Empty;
                string CompareDate1Day = string.Empty;
                string CompareDate2Day = string.Empty;

                //storing Date1 day month year
                if (!string.IsNullOrEmpty(CompareDate1))
                {
                    if (lengthDayOrMonth == CompareDate1.Substring(0, 2).Length)
                        CompareDate1Day = CompareDate1.Substring(0, 2);
                    if (lengthDayOrMonth == CompareDate1.Substring(3, 2).Length)
                        CompareDate1Month = CompareDate1.Substring(3, 2);
                    if (lengthYear == CompareDate1.Substring(6, 4).Length)
                        CompareDate1Year = CompareDate1.Substring(6, 4);
                }

                //stroing Date2 day month year
                if (!string.IsNullOrEmpty(CompareDate2))
                {
                    if (lengthDayOrMonth == CompareDate2.Substring(0, 2).Length)
                        CompareDate2Day = CompareDate2.Substring(0, 2);
                    if (lengthDayOrMonth == CompareDate2.Substring(3, 2).Length)
                        CompareDate2Month = CompareDate2.Substring(3, 2);
                    if (lengthYear == CompareDate2.Substring(6, 4).Length)
                        CompareDate2Year = CompareDate2.Substring(6, 4);
                }

                //Date Comparison logic
                if (string.Compare(CompareDate1Year, CompareDate2Year) == 0) // Same Year
                {
                    if (string.Compare(CompareDate1Month, CompareDate2Month) == 0) // Same Month
                    {
                        if (string.Compare(CompareDate1Day, CompareDate2Day) == 0) // Same Day
                            result = 0;
                        else if (string.Compare(CompareDate1Day, CompareDate2Day) > 0) // Date2 Day smaller
                            result = 1;
                        else if (string.Compare(CompareDate1Day, CompareDate2Day) < 0) // Date2 Day greater
                            result = -1;

                    }
                    else if (string.Compare(CompareDate1Month, CompareDate2Month) > 0) // Date2 Month smaller
                        result = 2;
                    else if (string.Compare(CompareDate1Month, CompareDate2Month) < 0) // Date2 Month greater
                        result = 3;

                }
                else if (string.Compare(CompareDate1Year, CompareDate2Year) > 0) // Date2 Year smaller
                    result = 5;
                else if (string.Compare(CompareDate1Year, CompareDate2Year) < 0) // Date2 Year greater
                    result = 6;

            }
            catch (Exception ex)
            { }
            return result;
        }


        public static int CompareDateExDate(string CompareDate1, string CompareDate2)
        {
            int result = 0;
            try
            {
                //Common Date Format "dd-MM-yyyy"
                int lengthDayOrMonth = 2;
                int lengthYear = 4;
                string CompareDate1Year = string.Empty;
                string CompareDate2Year = string.Empty;
                string CompareDate1Month = string.Empty;
                string CompareDate2Month = string.Empty;
                string CompareDate1Day = string.Empty;
                string CompareDate2Day = string.Empty;

                //storing Date1 day month year
                if (!string.IsNullOrEmpty(CompareDate1))
                {
                    if (lengthDayOrMonth == CompareDate1.Substring(8, 2).Length)
                        CompareDate1Day = CompareDate1.Substring(8, 2);
                    if (lengthDayOrMonth == CompareDate1.Substring(5, 2).Length)
                        CompareDate1Month = CompareDate1.Substring(5, 2);
                    if (lengthYear == CompareDate1.Substring(0, 4).Length)
                        CompareDate1Year = CompareDate1.Substring(0, 4);
                }

                //stroing Date2 day month year
                if (!string.IsNullOrEmpty(CompareDate2))
                {
                    if (lengthDayOrMonth == CompareDate2.Substring(0, 2).Length)
                        CompareDate2Day = CompareDate2.Substring(0, 2);
                    if (lengthDayOrMonth == CompareDate2.Substring(3, 2).Length)
                        CompareDate2Month = CompareDate2.Substring(3, 2);
                    if (lengthYear == CompareDate2.Substring(6, 4).Length)
                        CompareDate2Year = CompareDate2.Substring(6, 4);
                }

                //Date Comparison logic
                if (string.Compare(CompareDate1Year, CompareDate2Year) == 0) // Same Year
                {
                    if (string.Compare(CompareDate1Month, CompareDate2Month) == 0) // Same Month
                    {
                        if (string.Compare(CompareDate1Day, CompareDate2Day) == 0) // Same Day
                            result = 0;
                        else if (string.Compare(CompareDate1Day, CompareDate2Day) > 0) // Date2 Day smaller
                            result = 1;
                        else if (string.Compare(CompareDate1Day, CompareDate2Day) < 0) // Date2 Day greater
                            result = -1;

                    }
                    else if (string.Compare(CompareDate1Month, CompareDate2Month) > 0) // Date2 Month smaller
                        result = 2;
                    else if (string.Compare(CompareDate1Month, CompareDate2Month) < 0) // Date2 Month greater
                        result = 3;

                }
                else if (string.Compare(CompareDate1Year, CompareDate2Year) > 0) // Date2 Year smaller
                    result = 5;
                else if (string.Compare(CompareDate1Year, CompareDate2Year) < 0) // Date2 Year greater
                    result = 6;

            }
            catch (Exception ex)
            { }
            return result;
        }

        #region Commented Date Comparison
        //public static int CompareDateddMMYY(string CompareDate1, string CompareDate2)
        //{
        //    int result = 0;
        //    try
        //    {
        //        //Common Date Format "dd-MM-yyyy"
        //        int lengthDayOrMonth = 2;
        //        int lengthYear = 4;
        //        string CompareDate1Year = string.Empty;
        //        string CompareDate2Year = string.Empty;
        //        string CompareDate1Month = string.Empty;
        //        string CompareDate2Month = string.Empty;
        //        string CompareDate1Day = string.Empty;
        //        string CompareDate2Day = string.Empty;

        //        //storing Date1 day month year
        //        if (!string.IsNullOrEmpty(CompareDate1))
        //        {
        //            if (lengthDayOrMonth == CompareDate1.Substring(0, 2).Length)
        //                CompareDate1Day = CompareDate1.Substring(0, 2);
        //            if (lengthDayOrMonth == CompareDate1.Substring(3, 2).Length)
        //                CompareDate1Month = CompareDate1.Substring(3, 2);
        //            if (lengthYear == CompareDate1.Substring(6, 2).Length)
        //                CompareDate1Year = CompareDate1.Substring(6, 2);
        //        }

        //        //stroing Date2 day month year
        //        if (!string.IsNullOrEmpty(CompareDate2))
        //        {
        //            if (lengthDayOrMonth == CompareDate2.Substring(0, 2).Length)
        //                CompareDate2Day = CompareDate2.Substring(0, 2);
        //            if (lengthDayOrMonth == CompareDate2.Substring(3, 2).Length)
        //                CompareDate2Month = CompareDate2.Substring(3, 2);
        //            if (lengthYear == CompareDate2.Substring(6, 2).Length)
        //                CompareDate2Year = CompareDate2.Substring(6, 2);
        //        }

        //        //Date Comparison logic
        //        if (string.Compare(CompareDate1Year, CompareDate2Year) == 0) // Same Year
        //        {
        //            if (string.Compare(CompareDate1Month, CompareDate2Month) == 0) // Same Month
        //            {
        //                if (string.Compare(CompareDate1Day, CompareDate2Day) == 0) // Same Day
        //                    result = 0;
        //                else if (string.Compare(CompareDate1Day, CompareDate2Day) > 0) // Date2 Day smaller
        //                    result = 1;
        //                else if (string.Compare(CompareDate1Day, CompareDate2Day) < 0) // Date2 Day greater
        //                    result = -1;

        //            }
        //            else if (string.Compare(CompareDate1Month, CompareDate2Month) > 0) // Date2 Month smaller
        //                result = 2;
        //            else if (string.Compare(CompareDate1Month, CompareDate2Month) < 0) // Date2 Month greater
        //                result = 3;

        //        }
        //        else if (string.Compare(CompareDate1Year, CompareDate2Year) > 0) // Date2 Year smaller
        //            result = 5;
        //        else if (string.Compare(CompareDate1Year, CompareDate2Year) < 0) // Date2 Year greater
        //            result = 6;

        //    }
        //    catch (Exception ex)
        //    { }
        //    return result;
        //}
        #endregion

        /// <summary>
        /// Can fetch segment against scripcode
        /// </summary>
        /// <param name="scripCode"></param>
        /// <returns>segment</returns>
        /// 1-Equity 2-Derivative 3-Currency
        public static int GetSegmentFromScripCode(long scripCode)
        {
            int segmentNo = 0;
            //Equity
            if (MasterSharedMemory.objMastertxtDictBaseBSE.Any(p => p.Value.ScripCode == scripCode))
            {
                segmentNo = 1;
            }
            else if (MasterSharedMemory.objMasterDerivativeDictBaseBSE.Any(p => p.Value.ContractTokenNum == scripCode))
            {
                segmentNo = 2;
            }
            else if (MasterSharedMemory.objMasterCurrencyDictBaseBSE.Any(p => p.Value.ContractTokenNum == scripCode))
            {
                segmentNo = 3;
            }
            return segmentNo;

        }

        public static bool OnlyNumeric(string text)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(text, @"^[0-9]"))
                return true;
            else
                return false;
        }

        public static bool OnlyNumericNDecimal(string text)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(text, @"^[1-9]\d*(\.\d+)?$"))
                return true;
            else
                return false;
        }

        public static bool OnlyString(string text)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(text, @"^[a-zA-Z]+$"))
                return true;
            else
                return false;
        }

        public static void setLoginStatus(int SegmentValue, bool value)
        {
            ViewModel.MainWindowVM.objLogOnStatus[SegmentValue].isLoggedIn = value;
        }

        public static bool StringNSpecial(string text)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(text, @"^[a-zA-Z._^%$#!~@,-]+$"))
                return true;
            else
                return false;
        }


        /// <summary>
        ///  Read CSV File after spiltting and validae data
        /// </summary>
        /// <param name="WindowName"></param>
        /// <param name="Filedata"></param>
        /// <returns>Collection</returns>
        /// ConvertDatatoFormat is use to assign empty string for null/No value in file
        public static IEnumerable<OrderModel> ReadCSVFile(string WindowName, string[] Filedata)
        {
            switch (WindowName)
            {
                case "Batch Order":
                    {
                        #region TWS
                        ErrorCollection = new ObservableCollection<string>();
                        //SuccessCollection = new ObservableCollection<OrderModel>();
                        for (int i = 1; i < Filedata.Length; i++)
                        {
#if TWS
                            if (String.IsNullOrEmpty(Filedata[i]))
                            {
                                continue;
                            }

                            ErrorCount = 0;
                            StringBuilder sb = new StringBuilder();
                            try
                            {
                                OrderModel omodel = new OrderModel();
                                string[] strScrip = Filedata[i].Split(',');
                                strScrip = ConvertDatatoFormat(strScrip);

                                #region Changes For BOW
                                ////Segment
                                //if (!string.IsNullOrEmpty(strScrip[0].Trim()))
                                //{
                                //    //field validation
                                //    omodel.Segment = strScrip[0].Trim();
                                //}
                                //else
                                //{
                                //    sb.AppendLine(i + ": Segment should not be empty");
                                //    ErrorCount++;
                                //    // break;
                                //}

                                ////Buy/Sell Flag
                                //if (!string.IsNullOrEmpty(strScrip[1].Trim()))
                                //{
                                //    //field validation
                                //    omodel.BuySellIndicator = strScrip[1].Trim();
                                //}
                                //else
                                //{
                                //    sb.AppendLine(i + ": Buy/Sell Flag should not be empty");
                                //    ErrorCount++;
                                //    // break;
                                //}

                                ////Quantity
                                //if (!string.IsNullOrEmpty(strScrip[2].Trim()))
                                //{
                                //    //field validation
                                //    omodel.OriginalQty = Convert.ToInt32(strScrip[2]);
                                //}
                                //else
                                //{
                                //    sb.AppendLine(i + ": Quantity should not be empty");
                                //    ErrorCount++;
                                //    //break;
                                //}

                                ////Reaveal Quantity
                                //if (!string.IsNullOrEmpty(strScrip[3].Trim()))
                                //{
                                //    //field validation
                                //    omodel.RevealQty = Convert.ToInt32(strScrip[3]);
                                //}
                                //else
                                //{
                                //    sb.AppendLine(i + ": Reveal Quantity should not be empty");
                                //    ErrorCount++;
                                //    // break;
                                //}

                                ////ScripName
                                //if (!string.IsNullOrEmpty(strScrip[4].Trim()))
                                //{
                                //    //field validation
                                //    omodel.ScripCode = Convert.ToInt64(strScrip[4].Trim());
                                //}
                                //else
                                //{
                                //    sb.AppendLine(i + ": ScripName should not be empty");
                                //    ErrorCount++;
                                //    // break;
                                //}

                                ////Price(Rate)
                                //if (!string.IsNullOrEmpty(strScrip[5].Trim()))
                                //{
                                //    //field validation
                                //    omodel.Price = Convert.ToInt64(strScrip[5]);
                                //}
                                //else
                                //{
                                //    sb.AppendLine(i + ": Price should not be empty");
                                //    ErrorCount++;
                                //    // break;
                                //}

                                ////ClientID
                                //if (!string.IsNullOrEmpty(strScrip[6].Trim()))
                                //{
                                //    //field validation
                                //    omodel.ClientId = strScrip[6].Trim();
                                //}
                                //else
                                //{
                                //    sb.AppendLine(i + ": ClientId should not be empty");
                                //    ErrorCount++;
                                //    // break;
                                //}

                                ////Retention
                                //if (!string.IsNullOrEmpty(strScrip[7].Trim()))
                                //{
                                //    //field validation
                                //    omodel.OrderRetentionStatus = strScrip[7].Trim();
                                //}
                                //else
                                //{
                                //    sb.AppendLine(i + ": Retention should not be empty");
                                //    ErrorCount++;
                                //    //  break;
                                //}

                                ////ClientType 
                                //if (!string.IsNullOrEmpty(strScrip[8].Trim()))
                                //{
                                //    //field validation
                                //    omodel.ClientType = strScrip[8].Trim();
                                //}
                                //else
                                //{
                                //    sb.AppendLine(i + ": ClientType should not be empty");
                                //    ErrorCount++;
                                //    // break;
                                //}

                                ////OrderType 
                                //if (!string.IsNullOrEmpty(strScrip[9].Trim()))
                                //{
                                //    //field validation
                                //    omodel.OrderType = strScrip[9].Trim();
                                //}
                                //else
                                //{
                                //    sb.AppendLine(i + ": OrderType should not be empty");
                                //    ErrorCount++;
                                //    // break;
                                //}

                                ////CPCode
                                //if (!string.IsNullOrEmpty(strScrip[10].Trim()) || string.IsNullOrEmpty(strScrip[10].Trim()))
                                //{
                                //    //field validation
                                //    omodel.ParticipantCode = strScrip[10].Trim();
                                //}
                                //else
                                //{
                                //    sb.AppendLine(i + ": Error in CP Code");
                                //    ErrorCount++;
                                //    // break;
                                //}

                                ////TrgRate
                                //if (!string.IsNullOrEmpty(strScrip[11].Trim()))
                                //{
                                //    //field validation
                                //    omodel.TriggerPrice = Convert.ToInt64(strScrip[11].Trim());
                                //}
                                //else
                                //{
                                //    sb.AppendLine(i + ": TriggerPrice should not be empty");
                                //    ErrorCount++;
                                //    // break;
                                //}
                                #endregion

                                omodel.OrderFomLoadButton = true;

                                //Buy/Sell Flag
                                if (!string.IsNullOrEmpty(strScrip[0].Trim()))
                                {
                                    //field validation
                                    omodel.BuySellIndicator = strScrip[0].Trim();
                                }
                                else
                                {
                                    sb.AppendLine(i + ": Buy/Sell Flag should not be empty");
                                    ErrorCount++;
                                    // break;
                                }

                                //Quantity
                                if (!string.IsNullOrEmpty(strScrip[1].Trim()))
                                {
                                    //field validation
                                    omodel.OriginalQty = Convert.ToInt32(strScrip[1]);
                                }
                                else
                                {
                                    sb.AppendLine(i + ": Quantity should not be empty");
                                    ErrorCount++;
                                    //break;
                                }

                                //Reaveal Quantity
                                if (!string.IsNullOrEmpty(strScrip[2].Trim()))
                                {
                                    //field validation
                                    omodel.RevealQty = Convert.ToInt32(strScrip[2]);
                                }
                                else
                                {
                                    sb.AppendLine(i + ": Reveal Quantity should not be empty");
                                    ErrorCount++;
                                    // break;
                                }

                                //ScripName
                                if (!string.IsNullOrEmpty(strScrip[3].Trim()))
                                {
                                    //field validation
                                    // omodel.ScripCode = Convert.ToInt64(strScrip[3].Trim());
                                    omodel.ScripCodeID = strScrip[3].Trim();
                                }
                                else
                                {
                                    sb.AppendLine(i + ": ScripName should not be empty");
                                    ErrorCount++;
                                    // break;
                                }

                                //Price(Rate)
                                if (!string.IsNullOrEmpty(strScrip[4].Trim()))
                                {
                                    //field validation
                                    omodel.Price = Convert.ToInt64(strScrip[4]);
                                }
                                else
                                {
                                    sb.AppendLine(i + ": Price should not be empty");
                                    ErrorCount++;
                                    // break;
                                }

                                //ClientID
                                if (!string.IsNullOrEmpty(strScrip[5].Trim()))
                                {
                                    //field validation
                                    omodel.ClientId = strScrip[5].Trim();
                                }
                                else
                                {
                                    sb.AppendLine(i + ": ClientId should not be empty");
                                    ErrorCount++;
                                    // break;
                                }

                                //Retention
                                if (!string.IsNullOrEmpty(strScrip[6].Trim()))
                                {
                                    //field validation
                                    omodel.OrderRetentionStatus = strScrip[6].Trim();
                                }
                                else
                                {
                                    sb.AppendLine(i + ": Retention should not be empty");
                                    ErrorCount++;
                                    //  break;
                                }

                                //ClientType 
                                if (!string.IsNullOrEmpty(strScrip[7].Trim()))
                                {
                                    //field validation
                                    omodel.ClientType = strScrip[7].Trim();
                                }
                                else
                                {
                                    sb.AppendLine(i + ": ClientType should not be empty");
                                    ErrorCount++;
                                    // break;
                                }

                                //OrderType 
                                if (!string.IsNullOrEmpty(strScrip[8].Trim()))
                                {
                                    //field validation
                                    omodel.OrderType = strScrip[8].Trim();
                                }
                                else
                                {
                                    sb.AppendLine(i + ": OrderType should not be empty");
                                    ErrorCount++;
                                    // break;
                                }

                                //CPCode
                                if (!string.IsNullOrEmpty(strScrip[9].Trim()) || string.IsNullOrEmpty(strScrip[9].Trim()))
                                {
                                    //field validation
                                    omodel.ParticipantCode = strScrip[9].Trim();
                                }
                                else
                                {
                                    sb.AppendLine(i + ": Error in CP Code");
                                    ErrorCount++;
                                    // break;
                                }

                                //TrgRate
                                if (!string.IsNullOrEmpty(strScrip[10].Trim()))
                                {
                                    //field validation
                                    omodel.TriggerPrice = Convert.ToInt64(strScrip[10].Trim());
                                }
                                else
                                {
                                    sb.AppendLine(i + ": TriggerPrice should not be empty");
                                    ErrorCount++;
                                    // break;
                                }
                                //omodel.MessageTag = GetOrderMessageTag();
                                #region Comment
                                //CommonFieldsForReadingCSV csfields = new CommonFieldsForReadingCSV();
                                //string[] strScrip = Filedata[i].Split(',');
                                //strScrip = ConvertDatatoFormat(strScrip);

                                //#region B/S Flag 
                                //if (Convert.ToString(strScrip[0].Trim()).Length == 1)
                                //{
                                //    if (OnlyString(strScrip[0]))
                                //    {
                                //        if ((Convert.ToString(strScrip[0].Trim()).Equals('B')) || (Convert.ToString(strScrip[0].Trim()).Equals('S')))
                                //        {
                                //            csfields.BSFlag = Convert.ToChar(strScrip[0].Trim());
                                //        }
                                //        else
                                //        {
                                //            csfields.BSFlag = Convert.ToChar(strScrip[0].Trim());
                                //            sb.AppendLine(i + ": Buy/Sell Field should contain B/S");
                                //            WarningCount++;
                                //        }
                                //    }
                                //    else
                                //    {
                                //        sb.AppendLine(i + ": Buy/Sell Field should contain letters");
                                //        ErrorCount++;
                                //        break;
                                //    }
                                //}
                                //else
                                //{
                                //    sb.AppendLine(i + ": Error in Buy/Sell Field value should be B/S");
                                //    ErrorCount++;
                                //    break;
                                //}
                                //#endregion

                                //#region Quantity
                                //if (OnlyNumeric(Convert.ToString(strScrip[1])))
                                //{
                                //    if (Convert.ToInt32(strScrip[1]) <= 1000000000)//need to check the limit
                                //    {
                                //        csfields.TotQty = Convert.ToInt32(strScrip[1]);
                                //    }
                                //    else
                                //    {
                                //        sb.AppendLine(i + ": Quantity out of limit");
                                //        ErrorCount++;
                                //        break;
                                //    }
                                //}
                                //else
                                //{
                                //    sb.AppendLine(i + ": Quantity should be numeric");
                                //    ErrorCount++;
                                //    break;
                                //}
                                //#endregion

                                //#region Reveal Quantity
                                //if (OnlyNumeric(Convert.ToString(strScrip[2])))
                                //{
                                //    if (Convert.ToInt32(strScrip[1]) <= 1000000000)//need to check the limit
                                //    {
                                //        csfields.RevQty = Convert.ToInt32(strScrip[2]);
                                //    }
                                //    else
                                //    {
                                //        sb.AppendLine(i + ": Reveal Quantity out of limit");
                                //        ErrorCount++;
                                //        break;
                                //    }
                                //}
                                //else
                                //{
                                //    sb.AppendLine(i + ": Reveal Quantity should be numeric");
                                //    ErrorCount++;
                                //    break;
                                //}
                                //#endregion


                                //if (OnlyNumeric(Convert.ToString(strScrip[3])))
                                //{
                                //    csfields.ScripCode = Convert.ToInt64(strScrip[3]);
                                //}
                                //else
                                //{
                                //    sb.AppendLine(i + ": Scrip Code should be numeric");
                                //}

                                //if (OnlyNumericNDecimal(Convert.ToString(strScrip[4])))
                                //{
                                //    csfields.Rate = Convert.ToInt64(strScrip[4]);
                                //}
                                //else
                                //{
                                //    sb.AppendLine(i + ": Rate should be numeric/in paise");
                                //}

                                //if (StringNSpecial(Convert.ToString(strScrip[5])))
                                //{
                                //    csfields.ClientID = strScrip[5].Trim();
                                //}
                                //else
                                //{
                                //    sb.AppendLine(i + ": Enter client ID properly");
                                //}

                                //if (OnlyString(Convert.ToString(strScrip[6])))
                                //{
                                //    csfields.Retention = strScrip[6].Trim();
                                //}
                                //else
                                //{
                                //    sb.AppendLine(i + ": Retention should be in string ");
                                //}

                                //if (OnlyString(Convert.ToString(strScrip[7])))
                                //{
                                //    csfields.ClientType = strScrip[7].Trim();
                                //}
                                //else
                                //{
                                //    sb.AppendLine(i + ": Type shuold be of length 1/ enter only string ");
                                //}

                                //if (OnlyString(Convert.ToString(strScrip[8])) && Convert.ToString(strScrip[7].Trim()).Length == 1)
                                //{
                                //    csfields.OrderType = strScrip[8].Trim();
                                //}
                                //else
                                //{
                                //    sb.AppendLine(i + ": Enter only characters");
                                //}

                                //if (OnlyNumeric(Convert.ToString(strScrip[9])))
                                //{
                                //    csfields.CPCode = strScrip[9].Trim();
                                //}
                                //else
                                //{
                                //    sb.AppendLine(i + ": CP Code should be numeric ");
                                //}

                                //if (OnlyNumeric(Convert.ToString(strScrip[10])))
                                //{
                                //    csfields.TrgRate = Convert.ToInt64(strScrip[10]);
                                //}
                                //else
                                //{
                                //    sb.AppendLine(i + ": Trg Rate should be numeric/in paise");
                                //}

                                //csfields.TrgRate = Convert.ToInt64(strScrip[10]);
                                #endregion
                                if (ErrorCount == 0)
                                {
                                    //omodel.BatchKey = string.Format("{0}_{1}", omodel.ScripCode, omodel.MessageTag);
                                    SuccessCollection.Add(omodel);
                                }
                                else
                                {
                                    ErrorCollection.Add(sb.ToString());
                                }

                                BatchErrorWindowVM.TotalCount++;
                                if (ErrorCount > 0)
                                {
                                    BatchErrorWindow oBatchOrderScreen = System.Windows.Application.Current.Windows.OfType<BatchErrorWindow>().FirstOrDefault();

                                    if (oBatchOrderScreen != null)
                                    {
                                        oBatchOrderScreen.Activate();
                                        oBatchOrderScreen.Show();
                                    }
                                    else
                                    {
                                        oBatchOrderScreen = new BatchErrorWindow();
                                        oBatchOrderScreen.Activate();
                                        oBatchOrderScreen.Owner = System.Windows.Application.Current.MainWindow;
                                        oBatchOrderScreen.Activate();
                                        oBatchOrderScreen.Show();
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                ExceptionUtility.LogError(ex);
                            }
#endif
                        }
                        return SuccessCollection;
                        break;
                    }
                default:
                    return SuccessCollection;
                    break;

            }
            #endregion
        }

        /// <summary>
        /// Assign empty string to No value field in File
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        private static string[] ConvertDatatoFormat(string[] a)
        {
            for (int i = 0; i < a.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(a[i]))
                {
                    a[i] = String.Empty;
                }
            }
            return a;
        }

        public static bool ValidScripOrNot(long ScripCode)
        {
            if (MasterSharedMemory.objMastertxtDictBaseBSE != null && MasterSharedMemory.objMastertxtDictBaseBSE.Count > 0 && MasterSharedMemory.objMastertxtDictBaseBSE.ContainsKey(ScripCode))
                return (true);

            if (MasterSharedMemory.objMasterDerivativeDictBaseBSE != null && MasterSharedMemory.objMasterDerivativeDictBaseBSE.Count > 0 && MasterSharedMemory.objMasterDerivativeDictBaseBSE.ContainsKey(ScripCode))
                return (true);

            if (MasterSharedMemory.objMasterCurrencyDictBaseBSE != null && MasterSharedMemory.objMasterCurrencyDictBaseBSE.Count > 0 && MasterSharedMemory.objMasterCurrencyDictBaseBSE.ContainsKey(ScripCode))
                return (true);

            return (false);
        }

        public static string GetGroupName(long scripCode, Enumerations.Exchange exchangeFlag, Enumerations.Segment segment)
        {
            string groupName = string.Empty;
            //BSE
            if (Enumerations.Exchange.BSE == exchangeFlag)
            {
                //BSE Equity
                if (Enumerations.Segment.Equity == segment || Enumerations.Segment.Debt == segment)
                {
                    if (MasterSharedMemory.objMastertxtDictBaseBSE != null && MasterSharedMemory.objMastertxtDictBaseBSE.Count > 0)
                    {
                        if (MasterSharedMemory.objMastertxtDictBaseBSE.ContainsKey(scripCode))
                        {
                            groupName = MasterSharedMemory.objMastertxtDictBaseBSE[scripCode].GroupName;
                        }
                    }
                }
                //BSE Derivative
                else if (Enumerations.Segment.Derivative == segment)
                {
                    if (MasterSharedMemory.objMasterDerivativeDictBaseBSE != null && MasterSharedMemory.objMasterDerivativeDictBaseBSE.Count > 0)
                    {
                        if (MasterSharedMemory.objMasterDerivativeDictBaseBSE.ContainsKey(scripCode))
                        {
                            groupName = MasterSharedMemory.objMasterDerivativeDictBaseBSE[scripCode].DerScripGroup;
                        }
                    }
                }
                //BSE Currency
                else if (Enumerations.Segment.Currency == segment)
                {
                    if (MasterSharedMemory.objMasterCurrencyDictBaseBSE != null && MasterSharedMemory.objMasterCurrencyDictBaseBSE.Count > 0)
                    {
                        if (MasterSharedMemory.objMasterCurrencyDictBaseBSE.ContainsKey(scripCode))
                        {
                            groupName = MasterSharedMemory.objMasterCurrencyDictBaseBSE[scripCode].GroupName;
                        }
                    }

                    //groupName = MasterSharedMemory.objMasterCurrencyDictBaseBSE[scripCode].GroupName;
                }
                //BSE Currency
                else if (Enumerations.Segment.Debt == segment)
                {
                }
            }



#if BOW
            else if (Enumerations.Exchange.NSE == exchangeFlag)
            {
                //NSE Equity
                if (Enumerations.Segment.Equity == segment)
                {
                    if (MasterSharedMemory.objMastertxtDictBaseNSE != null && MasterSharedMemory.objMastertxtDictBaseNSE.Count > 0)
                    {
                        if (MasterSharedMemory.objMastertxtDictBaseNSE.ContainsKey(scripCode))
                        {
                            groupName = MasterSharedMemory.objMastertxtDictBaseNSE[scripCode].GroupName;
                        }
                    }
                }
                //NSE Derivative
                else if (Enumerations.Segment.Derivative == segment)
                {

                }
                //NSE Currency
                else if (Enumerations.Segment.Currency == segment)
                {
                }
            }
#endif
            return groupName;
        }


        public static string GetGroupName(long scripCode, string exchangeFlag, string segment)
        {
            string groupName = string.Empty;
            //BSE
            if (Enumerations.Exchange.BSE.ToString() == exchangeFlag)
            {
                //BSE Equity
                if (Enumerations.Segment.Equity.ToString() == segment || Enumerations.Segment.Debt.ToString() == segment)
                {
                    if (MasterSharedMemory.objMastertxtDictBaseBSE != null && MasterSharedMemory.objMastertxtDictBaseBSE.Count > 0)
                    {
                        if (MasterSharedMemory.objMastertxtDictBaseBSE.ContainsKey(scripCode))
                        {
                            groupName = MasterSharedMemory.objMastertxtDictBaseBSE[scripCode].GroupName;
                        }
                    }
                }
                //BSE Derivative
                else if (Enumerations.Segment.Derivative.ToString() == segment)
                {//

                    groupName = "DF";//MasterSharedMemory.objMasterDerivativeDictBaseBSE[scripCode].GroupName;
                    return groupName;
                }
                //BSE Currency
                else if (Enumerations.Segment.Currency.ToString() == segment)
                {
                    groupName = "CD";//MasterSharedMemory.objMasterCurrencyDictBaseBSE[scripCode].GroupName;//MasterSharedMemory.objMasterDerivativeDictBaseBSE[scripCode].GroupName;
                    return groupName;
                }
                //BSE Currency
                //else if (Enumerations.Segment.Debt.ToString() == segment)
                //{
                //}
            }
            return groupName;
        }

        public static string GetISIN(long scripCode, string exchange, string segment)
        {
            string ISIN = string.Empty;
            try
            {

                //BSE
                if (exchange == "BSE")
                {
                    //BSE Equity
                    if (segment == Enumerations.Order.ScripSegment.Equity.ToString())
                    {
                        if (MasterSharedMemory.objMastertxtDictBaseBSE != null && MasterSharedMemory.objMastertxtDictBaseBSE.Count > 0)
                        {
                            if (MasterSharedMemory.objMastertxtDictBaseBSE.ContainsKey(scripCode))
                            {
                                ISIN = MasterSharedMemory.objMastertxtDictBaseBSE[scripCode].IsinCode;
                            }
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
            }
            //NSE
#if BOW
            else if (Enumerations.Exchange.NSE == exchangeFlag)
            {
                //NSE Equity
                if (Enumerations.Segment.Equity == segment)
                {
                    if (MasterSharedMemory.objMastertxtDictBaseNSE != null && MasterSharedMemory.objMastertxtDictBaseNSE.Count > 0)
                    {
                        if (MasterSharedMemory.objMastertxtDictBaseNSE.ContainsKey(scripCode))
                        {
                            string strQuery = String.Format("SELECT IsinCode  FROM NSE_SECURITIES_CFE WHERE ScripCode={0}", scripCode);

                            SQLiteDataReader oSQLiteDataReader = oDataAccessLayer.ExecuteDataReader((int)ConnectionDB.Masters,strQuery, System.Data.CommandType.Text, null);
                            while (oSQLiteDataReader.Read())
                            {
                                //ISIN
                                var IsinCode = oSQLiteDataReader["IsinCode"]?.ToString().Trim();
                                if (!string.IsNullOrEmpty(IsinCode))
                                    ISIN = IsinCode; //MasterSharedMemory.objMastertxtDictBaseNSE[scripCode].IsinCode;
                            }
                        }
                    }
                }
                //NSE Derivative
                else if (Enumerations.Segment.Derivative == segment)
                {
                }
                //NSE Currency
                else if (Enumerations.Segment.Currency == segment)
                {
                }
            }
#endif
            return ISIN;
        }
        public static string GetScripName(long scripCode, Enumerations.Exchange exchangeFlag, Enumerations.Segment segment)
        {
            string scripName = string.Empty;
            //BSE
            if (Enumerations.Exchange.BSE == exchangeFlag)
            {
                //BSE Equity
                if (Enumerations.Segment.Equity == segment || Enumerations.Segment.Debt == segment)
                {
                    if (MasterSharedMemory.objMastertxtDictBaseBSE != null && MasterSharedMemory.objMastertxtDictBaseBSE.Count > 0)
                    {
                        if (MasterSharedMemory.objMastertxtDictBaseBSE.ContainsKey(scripCode))
                        {
                            scripName = MasterSharedMemory.objMastertxtDictBaseBSE[scripCode].ScripName;
                        }
                    }
                }
                //BSE Derivative
                else if (Enumerations.Segment.Derivative == segment)
                {
                    if (MasterSharedMemory.objMasterDerivativeDictBaseBSE != null && MasterSharedMemory.objMasterDerivativeDictBaseBSE.Count > 0)
                    {
                        if (MasterSharedMemory.objMasterDerivativeDictBaseBSE.ContainsKey(scripCode))
                        {
                            scripName = MasterSharedMemory.objMasterDerivativeDictBaseBSE[scripCode].InstrumentName;
                        }
                    }
                }
                //BSE Currency
                else if (Enumerations.Segment.Currency == segment)
                {
                    if (MasterSharedMemory.objMasterCurrencyDictBaseBSE != null && MasterSharedMemory.objMasterCurrencyDictBaseBSE.Count > 0)
                    {
                        if (MasterSharedMemory.objMasterCurrencyDictBaseBSE.ContainsKey(scripCode))
                        {
                            scripName = MasterSharedMemory.objMasterCurrencyDictBaseBSE[scripCode].InstrumentName;
                        }
                    }
                }
            }

            //NSE
#if BOW
            else if (Enumerations.Exchange.NSE == exchangeFlag)
            {
                //NSE Equity
                if (Enumerations.Segment.Equity == segment)
                {
                    if (MasterSharedMemory.objMastertxtDictBaseNSE != null && MasterSharedMemory.objMastertxtDictBaseNSE.Count > 0)
                    {
                        if (MasterSharedMemory.objMastertxtDictBaseNSE.ContainsKey(scripCode))
                        {
                            scripName = MasterSharedMemory.objMastertxtDictBaseNSE[scripCode].ScripName;
                        }
                    }
                }
                //NSE Derivative
                else if (Enumerations.Segment.Derivative == segment)
                {
                }
                //NSE Currency
                else if (Enumerations.Segment.Currency == segment)
                {
                }
            }
#endif
            return scripName;
        }


        //#region Commented GetSettlNo(long scripCode)


        //public static string GetSettlNo(long scripCode)
        //{
        //    string SettlNo = string.Empty;

        //    if (MasterSharedMemory.listSetlMas != null && MasterSharedMemory.listSetlMas.Count > 0)
        //    {
        //        //if (MasterSharedMemory.listSetlMas.Any(x => x.Field1 == "DR"))
        //        //{
        //        //    SettlNo = MasterSharedMemory.listSetlMas[scripCode];
        //        //}
        //        if (MasterSharedMemory.listSetlMas.Where(x => x.Field1 == "DR" && Convert.ToDateTime(x.Field2).ToString("dd/MM/yyyy") == System.DateTime.Now.ToString("dd/MM/yyyy")).Select(x => x.Fy).FirstOrDefault())
        //        {
        //            SettlNo = MasterSharedMemory.listSetlMas[scripCode];
        //        }
        //    }

        //    //oSaudasUMSModel.SettlNoComb = MasterSharedMemory.listSetlMas.Where(x => x.Field1 == "DR" && Convert.ToDateTime(x.Field2).ToString("dd/MM/yyyy") == System.DateTime.Now.ToString("dd/MM/yyyy")).Select(x => x.Fy).FirstOrDefault();

        //    ////foreach (var item in MasterSharedMemory.listSetlMas.Where(x=>x.Field1=="DR"))
        //    ////{
        //    ////    if (string.Format("{0:dd/MM/yyyy}", item.Field2) == System.DateTime.Now.ToString("dd/MM/yyyy"))
        //    ////    {
        //    ////        oSaudasUMSModel.SettlNo = item.Fy;
        //    ////    }
        //    ////}

        //    return SettlNo;
        //}
        //#endregion

        public static long GetScripCode(string scripName, Enumerations.Exchange exchangeFlag, Enumerations.Segment segment)
        {
            long scripCode = 0;
            //BSE
            if (Enumerations.Exchange.BSE == exchangeFlag)
            {
                if (Enumerations.Segment.Equity == segment || Enumerations.Segment.Debt == segment)
                {
                    if (MasterSharedMemory.objMastertxtDictBaseBSE != null && MasterSharedMemory.objMastertxtDictBaseBSE.Count > 0)
                    {
                        scripCode = MasterSharedMemory.objMastertxtDictBaseBSE.Where(x => x.Value.ScripId?.ToLower() == scripName.ToLower()).Select(x => x.Key).FirstOrDefault();
                    }
                }
                //BSE Derivative
                else if (Enumerations.Segment.Derivative == segment)
                {
                    if (MasterSharedMemory.objMasterDerivativeDictBaseBSE != null && MasterSharedMemory.objMasterDerivativeDictBaseBSE.Count > 0)
                    {
                        //if (MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Select(x => x.ScripId).FirstOrDefault() != null)
                        //{
                        scripCode = MasterSharedMemory.objMasterDerivativeDictBaseBSE.Where(x => x.Value.ScripId?.ToLower() == scripName.ToLower()).Select(x => x.Key).FirstOrDefault();
                        //}

                    }
                }
                //BSE Currency
                else if (Enumerations.Segment.Currency == segment)
                {
                    if (MasterSharedMemory.objMasterCurrencyDictBaseBSE != null && MasterSharedMemory.objMasterCurrencyDictBaseBSE.Count > 0)
                    {
                        scripCode = MasterSharedMemory.objMasterCurrencyDictBaseBSE.Where(x => x.Value.ScripId?.ToLower() == scripName.ToLower()).Select(x => x.Key).FirstOrDefault();
                    }
                }
            }
            //NSE
#if BOW
            else if (Enumerations.Exchange.NSE == exchangeFlag)
            {
                if (Enumerations.Segment.Equity == segment)
                {
                    if (MasterSharedMemory.objMastertxtDictBaseNSE != null && MasterSharedMemory.objMastertxtDictBaseNSE.Count > 0)
                    {
                        scripCode = MasterSharedMemory.objMastertxtDictBaseNSE.Where(x => x.Value.ScripId.ToLower() == scripName.ToLower()).Select(x => x.Key).FirstOrDefault();
                    }
                }
                //NSE Derivative
                else if (Enumerations.Segment.Derivative == segment)
                {
                }
                //NSE Currency
                else if (Enumerations.Segment.Currency == segment)
                {
                }
            }
#endif
            return scripCode;
        }

        /// <summary>
        /// Gets ScripId For Exchange BSE/NSE and Segment EQ/DER/CUR 
        /// </summary>
        /// <param name="scripCode"></param>
        /// <returns></returns>
        public static string GetScripId(long scripCode, Enumerations.Exchange exchangeFlag, Enumerations.Segment segment)
        {
            string scripId = string.Empty;

            //BSE
            if (Enumerations.Exchange.BSE == exchangeFlag)
            {
                //BSE Equity
                if (Enumerations.Segment.Equity == segment || Enumerations.Segment.Debt == segment)
                {
                    if (MasterSharedMemory.objMastertxtDictBaseBSE != null && MasterSharedMemory.objMastertxtDictBaseBSE.Count > 0)
                    {
                        if (MasterSharedMemory.objMastertxtDictBaseBSE.ContainsKey(scripCode))
                        {
                            scripId = MasterSharedMemory.objMastertxtDictBaseBSE[scripCode].ScripId;
                        }
                    }
                }
                //BSE Derivative
                else if (Enumerations.Segment.Derivative == segment)
                {
                    if (MasterSharedMemory.objMasterDerivativeDictBaseBSE != null && MasterSharedMemory.objMasterDerivativeDictBaseBSE.Count > 0)
                    {
                        if (MasterSharedMemory.objMasterDerivativeDictBaseBSE.ContainsKey(scripCode))
                        {
                            scripId = MasterSharedMemory.objMasterDerivativeDictBaseBSE[scripCode].ScripId;
                        }
                    }
                }
                //BSE Currency
                else if (Enumerations.Segment.Currency == segment)
                {
                    if (MasterSharedMemory.objMasterCurrencyDictBaseBSE != null && MasterSharedMemory.objMasterCurrencyDictBaseBSE.Count > 0)
                    {
                        if (MasterSharedMemory.objMasterCurrencyDictBaseBSE.ContainsKey(scripCode))
                        {
                            scripId = MasterSharedMemory.objMasterCurrencyDictBaseBSE[scripCode].ScripId;
                        }
                    }
                }
            }

            //NSE
#if BOW
            else if (Enumerations.Exchange.NSE == exchangeFlag)
            {
                //NSE Equity
                if (Enumerations.Segment.Equity == segment)
                {
                    if (MasterSharedMemory.objMastertxtDictBaseNSE != null && MasterSharedMemory.objMastertxtDictBaseNSE.Count > 0)
                    {
                        if (MasterSharedMemory.objMastertxtDictBaseNSE.ContainsKey(scripCode))
                        {
                            scripId = MasterSharedMemory.objMastertxtDictBaseNSE[scripCode].ScripId;
                        }
                    }
                }
                //NSE Derivative
                else if (Enumerations.Segment.Derivative == segment)
                {
                }
                //NSE Currency
                else if (Enumerations.Segment.Currency == segment)
                {
                }
            }
#endif
            return scripId;
        }

        public static string GetScripId(long scripCode, string exchangeFlag, string segment)
        {
            string scripId = string.Empty;

            //BSE
            if (Enumerations.Exchange.BSE.ToString() == exchangeFlag)
            {
                //BSE Equity
                if (Enumerations.Segment.Equity.ToString() == segment || Enumerations.Segment.Debt.ToString() == segment)
                {
                    if (MasterSharedMemory.objMastertxtDictBaseBSE != null && MasterSharedMemory.objMastertxtDictBaseBSE.Count > 0)
                    {
                        if (MasterSharedMemory.objMastertxtDictBaseBSE.ContainsKey(scripCode))
                        {
                            scripId = MasterSharedMemory.objMastertxtDictBaseBSE[scripCode].ScripId;
                        }
                    }
                }
                //BSE Derivative
                else if (Enumerations.Segment.Derivative.ToString() == segment)
                {
                    if (MasterSharedMemory.objMasterDerivativeDictBaseBSE != null && MasterSharedMemory.objMasterDerivativeDictBaseBSE.Count > 0)
                    {
                        if (MasterSharedMemory.objMasterDerivativeDictBaseBSE.ContainsKey(scripCode))
                        {
                            scripId = MasterSharedMemory.objMasterDerivativeDictBaseBSE[scripCode].ScripId;
                        }
                    }
                }
                //BSE Currency
                else if (Enumerations.Segment.Currency.ToString() == segment)
                {
                    if (MasterSharedMemory.objMasterCurrencyDictBaseBSE != null && MasterSharedMemory.objMasterCurrencyDictBaseBSE.Count > 0)
                    {
                        if (MasterSharedMemory.objMasterCurrencyDictBaseBSE.ContainsKey(scripCode))
                        {
                            scripId = MasterSharedMemory.objMasterCurrencyDictBaseBSE[scripCode].ScripId;
                        }
                    }
                }
            }

            //NSE
#if BOW
            else if (Enumerations.Exchange.NSE.ToString() == exchangeFlag)
            {
                //NSE Equity
                if (Enumerations.Segment.Equity.ToString() == segment)
                {
                    if (MasterSharedMemory.objMastertxtDictBaseNSE != null && MasterSharedMemory.objMastertxtDictBaseNSE.Count > 0)
                    {
                        if (MasterSharedMemory.objMastertxtDictBaseNSE.ContainsKey(scripCode))
                        {
                            scripId = MasterSharedMemory.objMastertxtDictBaseNSE[scripCode].ScripId;
                        }
                    }
                }
                //NSE Derivative
                else if (Enumerations.Segment.Derivative.ToString() == segment)
                {
                }
                //NSE Currency
                else if (Enumerations.Segment.Currency.ToString() == segment)
                {
                }
            }
#endif
            return scripId;
        }


        #region Decimal Conversion
        public static string DisplayInDecimalFormat<T, M>(T Value, M divisor, int decimalPlaces) where T : struct
        {
            //return Math.Round((Value / divisor), decimalPlaces);
            double temp = 0d;
            string result = string.Empty;
            string fmt = string.Empty;

            switch (decimalPlaces)
            {
                case 1:
                    fmt = ".0";
                    break;
                case 2:
                    fmt = ".00";
                    break;
                case 3:
                    fmt = ".000";
                    break;
                case 4:
                    fmt = ".0000";
                    break;
                case 5:
                    fmt = ".00000";
                    break;
                case 6:
                    fmt = ".000000";
                    break;
                case 7:
                    fmt = ".0000000";
                    break;
                case 8:
                    fmt = ".00000000";
                    break;
                default:
                    break;
            }

            if (Convert.ToDouble(divisor) != 0d)
            {
                temp = Convert.ToDouble(Value) / Convert.ToDouble(divisor);
            }

            result = temp.ToString(fmt, CultureInfo.InvariantCulture);
            return result;
        }

        /// <summary>
        /// No Operation Performed, String is return as per decimal
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Value"></param>
        /// <param name="decimalPlaces"></param>
        /// <returns></returns>
        public static string DisplayInDecimalFormat<T>(T Value, int decimalPlaces) where T : struct
        {
            if (Convert.ToInt64(Value) == 0)
            {
                return decimal.Zero.ToString();
            }

            string result = string.Empty;
            double temp = 0d;
            string fmt = string.Empty;

            switch (decimalPlaces)
            {
                case 1:
                    fmt = ".0";

                    break;
                case 2:
                    fmt = ".00";

                    break;
                case 3:
                    fmt = ".000";

                    break;
                case 4:
                    fmt = ".0000";

                    break;
                case 5:
                    fmt = ".00000";

                    break;
                case 6:
                    fmt = ".000000";

                    break;
                case 7:
                    fmt = ".0000000";

                    break;
                case 8:
                    fmt = ".00000000";

                    break;
                default:
                    break;
            }


            temp = Convert.ToDouble(Value);

            result = temp.ToString(fmt, CultureInfo.InvariantCulture);
            return result;
        }
        #endregion

        /// <summary>
        /// Get DecimalPoint/Precision for Exchange and segment. Modified by Gaurav Jadhav 08/09/2017
        /// </summary>
        /// <param name="scripcode">Any valid scripcode</param>
        /// <param name="exchangeFlag">BSE/NSE</param>
        /// <param name="segment">EQ/DER/CUR</param>
        /// <returns></returns>
        public static int GetDecimal(int scripcode, Enumerations.Exchange exchangeFlag, Enumerations.Segment segment)
        {
            int dec = 0;

            //BSE
            if (Enumerations.Exchange.BSE == exchangeFlag)
            {
                //BSE Equity
                if (Enumerations.Segment.Equity == segment || Enumerations.Segment.Debt == segment)
                {
                    if (scripcode != 0 && scripcode > 0)
                    {
                        if (MasterSharedMemory.objMastertxtDictBaseBSE != null && MasterSharedMemory.objMastertxtDictBaseBSE.ContainsKey(scripcode))
                        {
                            dec = MasterSharedMemory.objMastertxtDictBaseBSE[scripcode].Precision;//added by Gaurav  20/04/2017 15.52
                        }
                    }
                }
                //BSE Derivative
                else if (Enumerations.Segment.Derivative == segment)
                {
                    if (scripcode != 0 && scripcode > 0)
                    {
                        if (MasterSharedMemory.objMasterDerivativeDictBaseBSE != null && MasterSharedMemory.objMasterDerivativeDictBaseBSE.ContainsKey(scripcode))
                        {
                            dec = MasterSharedMemory.objMasterDerivativeDictBaseBSE[scripcode].Precision;//added by Gaurav  20/04/2017 15.52
                        }
                    }

                }
                //BSE Currency
                else if (Enumerations.Segment.Currency == segment)
                {
                    if (scripcode != 0 && scripcode > 0)
                    {
                        if (MasterSharedMemory.objMasterCurrencyDictBaseBSE != null && MasterSharedMemory.objMasterCurrencyDictBaseBSE.ContainsKey(scripcode))
                        {
                            dec = MasterSharedMemory.objMasterCurrencyDictBaseBSE[scripcode].Precision;//added by Gaurav  20/04/2017 15.52
                        }
                    }
                }
            }

            //NSE
#if BOW
            else if (Enumerations.Exchange.NSE == exchangeFlag)
            {
                //NSE Equity
                if (Enumerations.Segment.Equity == segment)
                {
                    if (scripcode != 0 && scripcode > 0)
                    {
                        if (MasterSharedMemory.objMastertxtDictBaseNSE != null && MasterSharedMemory.objMastertxtDictBaseNSE.ContainsKey(scripcode))
                        {
                            dec = MasterSharedMemory.objMastertxtDictBaseNSE[scripcode].Precision;//added by Gaurav  20/04/2017 15.52
                        }
                    }
                }
                //NSE Derivative
                else if (Enumerations.Segment.Derivative == segment)
                {
                    if (scripcode != 0 && scripcode > 0)
                    {
                        if (MasterSharedMemory.objMasterDerivativeDictBaseNSE != null && MasterSharedMemory.objMasterDerivativeDictBaseNSE.ContainsKey(scripcode))
                        {
                            dec = MasterSharedMemory.objMasterDerivativeDictBaseNSE[scripcode].Precision;//added by Gaurav  20/04/2017 15.52
                        }
                    }
                }
                //NSE Currency
                else if (Enumerations.Segment.Currency == segment)
                {
                    if (scripcode != 0 && scripcode > 0)
                    {
                        if (MasterSharedMemory.objMasterCurrencyDictBaseNSE != null && MasterSharedMemory.objMasterCurrencyDictBaseNSE.ContainsKey(scripcode))
                        {
                            dec = MasterSharedMemory.objMasterCurrencyDictBaseNSE[scripcode].Precision;//added by Gaurav  20/04/2017 15.52
                        }
                    }
                }
            }
#endif
            return dec;
        }

        public static int GetDecimal(int scripcode, string exchangeFlag, string segment)
        {
            int dec = 0;

            //BSE
            if (Enumerations.Exchange.BSE.ToString() == exchangeFlag)
            {
                //BSE Equity
                if (Enumerations.Segment.Equity.ToString() == segment)
                {
                    if (scripcode != 0 && scripcode > 0)
                    {
                        if (MasterSharedMemory.objMastertxtDictBaseBSE != null && MasterSharedMemory.objMastertxtDictBaseBSE.ContainsKey(scripcode))
                        {
                            dec = MasterSharedMemory.objMastertxtDictBaseBSE[scripcode].Precision;//added by Gaurav  20/04/2017 15.52
                        }
                    }
                }
                //BSE Derivative
                else if (Enumerations.Segment.Derivative.ToString() == segment)
                {
                    if (scripcode != 0 && scripcode > 0)
                    {
                        if (MasterSharedMemory.objMasterDerivativeDictBaseBSE != null && MasterSharedMemory.objMasterDerivativeDictBaseBSE.ContainsKey(scripcode))
                        {
                            dec = MasterSharedMemory.objMasterDerivativeDictBaseBSE[scripcode].Precision;//added by Gaurav  20/04/2017 15.52
                        }
                    }
                }
                //BSE Currency
                else if (Enumerations.Segment.Currency.ToString() == segment)
                {
                    if (scripcode != 0 && scripcode > 0)
                    {
                        if (MasterSharedMemory.objMasterCurrencyDictBaseBSE != null && MasterSharedMemory.objMasterCurrencyDictBaseBSE.ContainsKey(scripcode))
                        {
                            dec = MasterSharedMemory.objMasterCurrencyDictBaseBSE[scripcode].Precision;//added by Gaurav  20/04/2017 15.52
                        }
                    }
                }
                //BSE Debt
                else if (Enumerations.Segment.Debt.ToString() == segment)
                {
                    if (scripcode != 0 && scripcode > 0)
                    {
                        if (MasterSharedMemory.objMastertxtDictBaseBSE != null && MasterSharedMemory.objMastertxtDictBaseBSE.ContainsKey(scripcode))
                        {
                            dec = MasterSharedMemory.objMastertxtDictBaseBSE[scripcode].Precision;//added by Gaurav  20/04/2017 15.52
                        }
                    }
                }
            }




            //NSE
#if BOW
            else if (Enumerations.Exchange.NSE.ToString() == exchangeFlag)
            {
                //NSE Equity
                if (Enumerations.Segment.Equity.ToString() == segment)
                {
                    if (scripcode != 0 && scripcode > 0)
                    {
                        if (MasterSharedMemory.objMastertxtDictBaseNSE != null && MasterSharedMemory.objMastertxtDictBaseNSE.ContainsKey(scripcode))
                        {
                            dec = MasterSharedMemory.objMastertxtDictBaseNSE[scripcode].Precision;//added by Gaurav  20/04/2017 15.52
                        }
                    }
                }
                //NSE Derivative
                else if (Enumerations.Segment.Derivative.ToString() == segment)
                {
                }
                //NSE Currency
                else if (Enumerations.Segment.Currency.ToString() == segment)
                {
                }
            }
#endif
            return dec;
        }

        /// <summary>
        /// Partition ID. Added by Gaurav Jadhav 9/3/2018 
        /// </summary>
        /// <param name="scripcode"></param>
        /// <param name="exchangeFlag"></param>
        /// <param name="segment"></param>
        /// <returns></returns>
        public static string GetPartitionId(long scripcode, string exchangeFlag, string segment)
        {
            string result = string.Empty;

            //BSE
            if (Enumerations.Exchange.BSE.ToString() == exchangeFlag)
            {
                //BSE Equity
                if (Enumerations.Order.ScripSegment.Equity.ToString() == segment || Enumerations.Order.ScripSegment.Debt.ToString() == segment)
                {
                    if (scripcode != 0 && scripcode > 0)
                    {
                        if (MasterSharedMemory.objMastertxtDictBaseBSE != null && MasterSharedMemory.objMastertxtDictBaseBSE.ContainsKey(scripcode))
                        {
                            result = MasterSharedMemory.objMastertxtDictBaseBSE[scripcode].PartitionId;//added by Gaurav  20/04/2017 15.52
                            return result;
                        }
                    }
                }
                //BSE Derivative
                else if (Enumerations.Order.ScripSegment.Derivative.ToString() == segment)
                {
                    if (scripcode != 0 && scripcode > 0)
                    {
                        if (MasterSharedMemory.objMasterDerivativeDictBaseBSE != null && MasterSharedMemory.objMasterDerivativeDictBaseBSE.ContainsKey(scripcode))
                        {
                            result = Convert.ToString(MasterSharedMemory.objMasterDerivativeDictBaseBSE[scripcode].PartitionID);//added by Gaurav  20/04/2017 15.52
                            return result;
                        }
                    }
                }
                //BSE Currency
                else if (Enumerations.Order.ScripSegment.Currency.ToString() == segment)
                {
                    if (scripcode != 0 && scripcode > 0)
                    {
                        if (MasterSharedMemory.objMasterCurrencyDictBaseBSE != null && MasterSharedMemory.objMasterCurrencyDictBaseBSE.ContainsKey(scripcode))
                        {
                            result = Convert.ToString(MasterSharedMemory.objMasterCurrencyDictBaseBSE[scripcode].PartitionID);//added by Gaurav  20/04/2017 15.52
                            return result;
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// MarketSegmentId OR ProductID.  Added by Gaurav Jadhav 9/3/2018
        /// </summary>
        /// <param name="scripcode"></param>
        /// <param name="exchangeFlag"></param>
        /// <param name="segment"></param>
        /// <returns></returns>
        public static string GetProductId(long scripcode, string exchangeFlag, string segment)
        {
            string result = string.Empty;

            //BSE
            if (Enumerations.Exchange.BSE.ToString() == exchangeFlag)
            {
                //BSE Equity
                if (Enumerations.Order.ScripSegment.Equity.ToString() == segment || Enumerations.Order.ScripSegment.Debt.ToString() == segment)
                {
                    if (scripcode != 0 && scripcode > 0)
                    {
                        if (MasterSharedMemory.objMastertxtDictBaseBSE != null && MasterSharedMemory.objMastertxtDictBaseBSE.ContainsKey(scripcode))
                        {
                            result = MasterSharedMemory.objMastertxtDictBaseBSE[scripcode].MarketSegmentID;//added by Gaurav  20/04/2017 15.52
                            return result;
                        }
                    }
                }
                //BSE Derivative
                else if (Enumerations.Order.ScripSegment.Derivative.ToString() == segment)
                {
                    if (scripcode != 0 && scripcode > 0)
                    {
                        if (MasterSharedMemory.objMasterDerivativeDictBaseBSE != null && MasterSharedMemory.objMasterDerivativeDictBaseBSE.ContainsKey(scripcode))
                        {
                            result = Convert.ToString(MasterSharedMemory.objMasterDerivativeDictBaseBSE[scripcode].ProductID);//added by Gaurav  20/04/2017 15.52
                            return result;
                        }
                    }
                }
                //BSE Currency
                else if (Enumerations.Order.ScripSegment.Currency.ToString() == segment)
                {
                    if (scripcode != 0 && scripcode > 0)
                    {
                        if (MasterSharedMemory.objMasterCurrencyDictBaseBSE != null && MasterSharedMemory.objMasterCurrencyDictBaseBSE.ContainsKey(scripcode))
                        {
                            result = Convert.ToString(MasterSharedMemory.objMasterCurrencyDictBaseBSE[scripcode].ProductID);//added by Gaurav  20/04/2017 15.52
                            return result;
                        }
                    }
                }
            }

            return result;
        }

        public static string GetSegmentID(long scripcode)
        {
            if (scripcode != 0 && scripcode > 0)
            {
                if (MasterSharedMemory.objMastertxtDictBaseBSE != null && MasterSharedMemory.objMastertxtDictBaseBSE.ContainsKey(scripcode))
                {
                    char InstrumentType = MasterSharedMemory.objMastertxtDictBaseBSE.Where(x => x.Key == scripcode).Select(x => x.Value.InstrumentType).FirstOrDefault();
                    if (InstrumentType == 'E')
                    {
                        return Segment.Equity.ToString();
                    }
                    else
                    {
                        return Segment.Debt.ToString();
                    }

                }
                else if (MasterSharedMemory.objMasterDerivativeDictBaseBSE != null && MasterSharedMemory.objMasterDerivativeDictBaseBSE.ContainsKey(scripcode))
                {
                    return Segment.Derivative.ToString();
                }
                else if (MasterSharedMemory.objMasterCurrencyDictBaseBSE != null && MasterSharedMemory.objMasterCurrencyDictBaseBSE.ContainsKey(scripcode))
                {
                    return Segment.Currency.ToString();
                }
            }
            return string.Empty;
        }

        public static long GetQuantityMultiplier(long ScripCode, string exchange, string segment)
        {
            long result = -1;
            if (ScripCode != 0 && ScripCode > 0)
            {
                if (exchange == Enumerations.Exchange.BSE.ToString())
                {
                    if (segment == Enumerations.Segment.Equity.ToString() || segment == Enumerations.Segment.Debt.ToString())
                    {
                        if (MasterSharedMemory.objMastertxtDictBaseBSE != null && MasterSharedMemory.objMastertxtDictBaseBSE.ContainsKey(ScripCode))
                        {
                            return MasterSharedMemory.objMastertxtDictBaseBSE[ScripCode].QuantityMultiplier;
                        }
                    }
                    else if (segment == Enumerations.Segment.Derivative.ToString())
                    {
                        if (MasterSharedMemory.objMasterDerivativeDictBaseBSE != null && MasterSharedMemory.objMasterDerivativeDictBaseBSE.ContainsKey(ScripCode))
                        {
                            return MasterSharedMemory.objMasterDerivativeDictBaseBSE[ScripCode].QuantityMultiplier;
                        }
                    }
                    else if (segment == Enumerations.Segment.Currency.ToString())
                    {
                        if (MasterSharedMemory.objMasterCurrencyDictBaseBSE != null && MasterSharedMemory.objMasterCurrencyDictBaseBSE.ContainsKey(ScripCode))
                        {
                            return MasterSharedMemory.objMasterCurrencyDictBaseBSE[ScripCode].QuantityMultiplier;
                        }
                    }
                }
            }
            return result;
        }
        public static string GetReasonReturnedOrder(string ReasonCode, uint messageType, out string type)
        {
            string message = string.Empty;
            type = string.Empty;
            try
            {
                string key1 = string.Format("{0}_{1}", ReasonCode, "OTHER");
                string key2 = string.Format("{0}_{1}", ReasonCode, "SPOS");
                string key3 = string.Format("{0}_{1}", ReasonCode, "EOSSESS");
                string key4 = string.Format("{0}_{1}", ReasonCode, "RRM");
                string key5 = string.Format("{0}_{1}", ReasonCode, "MASSCANCELL");
                List<string> keyArr = new List<string>();
                keyArr.Add(key1);
                keyArr.Add(key2);
                keyArr.Add(key3);
                keyArr.Add(key4);
                keyArr.Add(key5);
                foreach (var localKey in keyArr)
                {
                    if (MemoryManager.ReturnedOrderDictionaryFilter.ContainsKey(localKey))
                    {
                        if (MemoryManager.ReturnedOrderDictionaryFilter[localKey].MessageList.ContainsKey(messageType))
                        {
                            message = MemoryManager.ReturnedOrderDictionaryFilter[localKey].MessageList[messageType];
                            type = localKey.Split('_')[1];
                            break;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw;
            }
            return message;
        }

        public static string GetTickSize(long scripcode)
        {
            if (scripcode != 0 && scripcode > 0)
            {
                if (MasterSharedMemory.objMastertxtDictBaseBSE != null && MasterSharedMemory.objMastertxtDictBaseBSE.ContainsKey(scripcode))
                {
                    return MasterSharedMemory.objMastertxtDictBaseBSE[scripcode].TickSize.ToString();
                }
                else if (MasterSharedMemory.objMasterDerivativeDictBaseBSE != null && MasterSharedMemory.objMasterDerivativeDictBaseBSE.ContainsKey(scripcode))
                {
                    return MasterSharedMemory.objMasterDerivativeDictBaseBSE[scripcode].TickSize.ToString();
                }
                else if (MasterSharedMemory.objMasterCurrencyDictBaseBSE != null && MasterSharedMemory.objMasterCurrencyDictBaseBSE.ContainsKey(scripcode))
                {
                    return MasterSharedMemory.objMasterCurrencyDictBaseBSE[scripcode].TickSize.ToString();
                }
            }
            return string.Empty;
        }

        public static string GetMarketLot(long scripcode)
        {
            if (scripcode != 0 && scripcode > 0)
            {
                if (MasterSharedMemory.objMastertxtDictBaseBSE != null && MasterSharedMemory.objMastertxtDictBaseBSE.ContainsKey(scripcode))
                {
                    return MasterSharedMemory.objMastertxtDictBaseBSE[scripcode].MarketLot.ToString();
                }
                else if (MasterSharedMemory.objMasterDerivativeDictBaseBSE != null && MasterSharedMemory.objMasterDerivativeDictBaseBSE.ContainsKey(scripcode))
                {
                    return MasterSharedMemory.objMasterDerivativeDictBaseBSE[scripcode].MinimumLotSize.ToString();
                }
                else if (MasterSharedMemory.objMasterCurrencyDictBaseBSE != null && MasterSharedMemory.objMasterCurrencyDictBaseBSE.ContainsKey(scripcode))
                {
                    return MasterSharedMemory.objMasterCurrencyDictBaseBSE[scripcode].MinimumLotSize.ToString();
                }
            }
            return string.Empty;
        }
        public static string GetMarketLot(long scripcode, string exchange, string segment)
        {
            if (scripcode != 0 && scripcode > 0)
            {
                if (exchange == Enumerations.Exchange.BSE.ToString())
                {
                    if (segment == Enumerations.Segment.Equity.ToString() || segment == Enumerations.Segment.Debt.ToString())
                    {
                        if (MasterSharedMemory.objMastertxtDictBaseBSE != null && MasterSharedMemory.objMastertxtDictBaseBSE.ContainsKey(scripcode))
                        {
                            return MasterSharedMemory.objMastertxtDictBaseBSE[scripcode].MarketLot.ToString();
                        }
                    }
                    else if (segment == Enumerations.Segment.Derivative.ToString())
                    {
                        if (MasterSharedMemory.objMasterDerivativeDictBaseBSE != null && MasterSharedMemory.objMasterDerivativeDictBaseBSE.ContainsKey(scripcode))
                        {
                            return MasterSharedMemory.objMasterDerivativeDictBaseBSE[scripcode].MinimumLotSize.ToString();
                        }
                    }
                    else if (segment == Enumerations.Segment.Currency.ToString())
                    {
                        if (MasterSharedMemory.objMasterCurrencyDictBaseBSE != null && MasterSharedMemory.objMasterCurrencyDictBaseBSE.ContainsKey(scripcode))
                        {
                            return MasterSharedMemory.objMasterCurrencyDictBaseBSE[scripcode].MinimumLotSize.ToString();
                        }
                    }
                }
                else if (exchange == Enumerations.Exchange.NSE.ToString())
                {

                }
            }
            return string.Empty;
        }
        public static string DisplayInDecimalFormatTouch<T>(T Value, int decimalPlaces) where T : struct
        {
            if (Convert.ToInt64(Value) == 0)
            {
                return "0";
            }
            //return Math.Round((Value / divisor), decimalPlaces);
            string result = string.Empty;
            double temp = 0d;
            string fmt = string.Empty;
            int divisor = 0;

            switch (decimalPlaces)
            {
                case 1:
                    fmt = ".0";
                    divisor = 10;
                    break;
                case 2:
                    fmt = ".00";
                    divisor = 100;
                    break;
                case 3:
                    fmt = ".000";
                    divisor = 1000;
                    break;
                case 4:
                    fmt = ".0000";
                    divisor = 10000;
                    break;
                case 5:
                    fmt = ".00000";
                    divisor = 100000;
                    break;
                case 6:
                    fmt = ".000000";
                    divisor = 1000000;
                    break;
                case 7:
                    fmt = ".0000000";
                    divisor = 10000000;
                    break;
                case 8:
                    fmt = ".00000000";
                    divisor = 100000000;
                    break;
                default:
                    break;
            }

            if (Convert.ToDouble(divisor) != 0d)
            {
                temp = Convert.ToDouble(Value) / Convert.ToDouble(divisor);
            }

            result = temp.ToString(fmt, CultureInfo.InvariantCulture);
            return result;
        }
        public static string DisplayInDecimalFormatTouch1<T>(T Value, int decimalPlaces) where T : struct
        {
            if (Convert.ToInt64(Value) == 0)
            {
                return "0";
            }
            //return Math.Round((Value / divisor), decimalPlaces);
            string result = string.Empty;
            double temp = 0d;
            string fmt = string.Empty;
            int divisor = 0;

            switch (decimalPlaces)
            {
                case 1:
                    fmt = "0.0";
                    divisor = 10;
                    break;
                case 2:
                    fmt = "0.00";
                    divisor = 100;
                    break;
                case 3:
                    fmt = "0.000";
                    divisor = 1000;
                    break;
                case 4:
                    fmt = "0.0000";
                    divisor = 10000;
                    break;
                case 5:
                    fmt = "0.00000";
                    divisor = 100000;
                    break;
                case 6:
                    fmt = "0.000000";
                    divisor = 1000000;
                    break;
                case 7:
                    fmt = "0.0000000";
                    divisor = 10000000;
                    break;
                case 8:
                    fmt = "0.00000000";
                    divisor = 100000000;
                    break;
                default:
                    break;
            }

            if (Convert.ToDouble(divisor) != 0d)
            {
                temp = Convert.ToDouble(Value) / Convert.ToDouble(divisor);
            }

            result = temp.ToString(fmt, CultureInfo.InvariantCulture);
            return result;
        }
        public static string DisplayInDecimalFormatTouch2<T>(T Value, long decimalPlaces) where T : struct
        {
            if (Convert.ToInt64(Value) == 0)
            {
                return "0";
            }
            //return Math.Round((Value / divisor), decimalPlaces);
            string result = string.Empty;
            double temp = 0d;
            string fmt = string.Empty;
            int divisor = 0;

            switch (decimalPlaces)
            {
                case 1:
                    fmt = "0.0";
                    divisor = 10;
                    break;
                case 2:
                    fmt = "0.00";
                    divisor = 100;
                    break;
                case 3:
                    fmt = "0.000";
                    divisor = 1000;
                    break;
                case 4:
                    fmt = "0.0000";
                    divisor = 10000;
                    break;
                case 5:
                    fmt = "0.00000";
                    divisor = 100000;
                    break;
                case 6:
                    fmt = "0.000000";
                    divisor = 1000000;
                    break;
                case 7:
                    fmt = "0.0000000";
                    divisor = 10000000;
                    break;
                case 8:
                    fmt = "0.00000000";
                    divisor = 100000000;
                    break;
                default:
                    break;
            }

            if (Convert.ToDouble(divisor) != 0d)
            {
                temp = Convert.ToDouble(Value) / Convert.ToDouble(divisor);
            }

            result = temp.ToString(fmt, CultureInfo.InvariantCulture);
            return result;
        }
        /// <summary>
        /// Custom logic to Round off paise
        /// </summary>
        /// <param name="TmpPaisa_float">TmpPaisa_float of type float</param>
        /// <param name="TmpSIndx">TmpSIndx is scripcode</param>
        /// <returns></returns>
        //internal static long RoundOffPaisa_Float_New(float TmpPaisa_float, int TmpSIndx) //MHB24
        //{
        //    long TmpPaisa_long = 0;
        //    bool NegativeRate = false;
        //    long ratemultiply = 10000;
        //    try
        //    {
        //        int decimalPnt = GetDecimal(TmpSIndx);

        //        if (TmpSIndx != -1)
        //            ratemultiply = Convert.ToInt64(Math.Pow(10, Convert.ToDouble(decimalPnt)));

        //        if (TmpPaisa_float < 0)
        //        {
        //            NegativeRate = true;
        //            TmpPaisa_float = -(TmpPaisa_float);
        //        }

        //        if (TmpSIndx != -1)
        //        {
        //            switch (decimalPnt)
        //            {
        //                case 2:
        //                    TmpPaisa_long = (long)((TmpPaisa_float + 0.005) * ratemultiply);
        //                    break;

        //                default:
        //                case 4:
        //                    TmpPaisa_long = (long)((TmpPaisa_float + 0.00005) * ratemultiply);
        //                    break;
        //            }
        //        }
        //        else
        //            TmpPaisa_long = (long)((TmpPaisa_float + 0.00005) * ratemultiply);

        //        if (NegativeRate == true)
        //            TmpPaisa_long = -(TmpPaisa_long);
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionUtility.LogError(ex);
        //    }
        //    return (TmpPaisa_long);
        //}
        /// <summary>
        /// Custom logic to Round off paise
        /// </summary>
        /// <param name="TmpPaisa_double">TmpPaisa_double of type double</param>
        /// <param name="TmpSIndx">TmpSIndx is scripcode</param>
        /// <returns></returns>
        //internal static long RoundOffPaisa_Double_New(double TmpPaisa_double, int TmpSIndx) //MHB24
        //{
        //    long TmpPaisa_int64 = 0;
        //    bool NegativeRate = false;
        //    long ratemultiply = 10000;

        //    try
        //    {
        //        int decimalPnt = GetDecimal(TmpSIndx);

        //        if (TmpSIndx != -1)
        //            ratemultiply = Convert.ToInt64(Math.Pow(10, Convert.ToDouble(decimalPnt)));

        //        if (TmpPaisa_double < 0)
        //        {
        //            NegativeRate = true;
        //            TmpPaisa_double = -(TmpPaisa_double);
        //        }

        //        if (TmpSIndx != -1)
        //        {
        //            switch (decimalPnt)
        //            {
        //                case 2:
        //                    TmpPaisa_int64 = (long)((TmpPaisa_double + 0.005) * ratemultiply);
        //                    break;

        //                default:
        //                case 4:
        //                    TmpPaisa_int64 = (long)((TmpPaisa_double + 0.00005) * ratemultiply);
        //                    break;
        //            }
        //        }
        //        else
        //            TmpPaisa_int64 = (long)((TmpPaisa_double + 0.00005) * ratemultiply);

        //        if (NegativeRate == true)
        //            TmpPaisa_int64 = -(TmpPaisa_int64);


        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionUtility.LogError(ex);
        //    }
        //    return (TmpPaisa_int64);
        //}

        public static DateTime GetDate()
        {
            if (UtilityLoginDetails.GETInstance.TodaysDateTime != DateTime.MinValue || UtilityLoginDetails.GETInstance.TodaysDateTime != DateTime.MaxValue)
                return UtilityLoginDetails.GETInstance.TodaysDateTime;
            else
                return DateTime.Now;
        }

        public static long GetScripCodeFromScripID(string ScripID, string exchangeFlag, string segment)
        {
            long scripCode = 0;
            //BSE
            if (Enumerations.Exchange.BSE.ToString() == exchangeFlag)
            {
                //BSE Equity
                if (Enumerations.Segment.Equity.ToString() == segment || Enumerations.Segment.Debt.ToString() == segment)
                {
                    if (MasterSharedMemory.objMastertxtDictBaseBSE != null && MasterSharedMemory.objMastertxtDictBaseBSE.Count > 0)
                    {
                        if (MasterSharedMemory.objMastertxtDictBaseBSE.Values.Any(x => x.ScripId.ToUpper().Trim().Equals(ScripID.ToUpper().Trim())))
                        {
                            scripCode = MasterSharedMemory.objMastertxtDictBaseBSE.Where(x => x.Value.ScripId.ToUpper().Trim().Equals(ScripID.ToUpper().Trim())).Select(x => x.Key).FirstOrDefault();
                        }
                    }
                }
                //BSE Derivative
                else if (Enumerations.Segment.Derivative.ToString() == segment)
                {
                    if (MasterSharedMemory.objMasterDerivativeDictBaseBSE != null && MasterSharedMemory.objMasterDerivativeDictBaseBSE.Count > 0)
                    {
                        //if (MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Select(x => x.ScripId).FirstOrDefault() != null)
                        //{
                        scripCode = MasterSharedMemory.objMasterDerivativeDictBaseBSE.Where(x => x.Value.ScripId?.ToLower() == ScripID.ToLower()).Select(x => x.Key).FirstOrDefault();
                        //}

                    }
                }
                //BSE Currency
                else if (Enumerations.Segment.Currency.ToString() == segment)
                {
                    if (MasterSharedMemory.objMasterCurrencyDictBaseBSE != null && MasterSharedMemory.objMasterCurrencyDictBaseBSE.Count > 0)
                    {
                        scripCode = MasterSharedMemory.objMasterCurrencyDictBaseBSE.Where(x => x.Value.ScripId?.ToLower() == ScripID.ToLower()).Select(x => x.Key).FirstOrDefault();
                    }
                }
            }

            //NSE
#if BOW
            else if (Enumerations.Exchange.NSE.ToString() == exchangeFlag)
            {
                //NSE Equity
                if (Enumerations.Segment.Equity.ToString() == segment)
                {
                    if (MasterSharedMemory.objMastertxtDictBaseNSE != null && MasterSharedMemory.objMastertxtDictBaseNSE.Count > 0)
                    {
                        if (MasterSharedMemory.objMastertxtDictBaseNSE.Values.Any(x => x.ScripId.ToUpper().Trim().Equals(ScripID.ToUpper().Trim())))
                        {
                            scripCode = MasterSharedMemory.objMastertxtDictBaseNSE.Where(x => x.Value.ScripId.ToUpper().Trim().Equals(ScripID.ToUpper().Trim())).Select(x => x.Key).FirstOrDefault();
                        }
                    }
                }
                //NSE Derivative
                else if (Enumerations.Segment.Derivative.ToString() == segment)
                {
                }
                //NSE Currency
                else if (Enumerations.Segment.Currency.ToString() == segment)
                {
                }
            }
#endif
            return scripCode;
        }
        public static string GetCorpAction(long scripCode)
        {
            string result = string.Empty;
            try
            {
                if (scripCode != -1)
                {
                    if (MemoryManager.CorpActForOE != null && MemoryManager.CorpActForOE.Count > 0 && MemoryManager.CorpActForOE.ContainsKey(scripCode))
                    {
                        result = MemoryManager.CorpActForOE[scripCode];
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
            }
            return result;
        }
        public static string GetScripName(long scripCode, string exchangeFlag, string segment)
        {
            string scripName = string.Empty;
            //BSE
            if (Enumerations.Exchange.BSE.ToString() == exchangeFlag)
            {
                //BSE Equity
                if (Enumerations.Segment.Equity.ToString() == segment || Enumerations.Segment.Debt.ToString() == segment)
                {
                    if (MasterSharedMemory.objMastertxtDictBaseBSE != null && MasterSharedMemory.objMastertxtDictBaseBSE.Count > 0)
                    {
                        if (MasterSharedMemory.objMastertxtDictBaseBSE.ContainsKey(scripCode))
                        {
                            scripName = MasterSharedMemory.objMastertxtDictBaseBSE[scripCode].ScripName;
                        }
                    }
                }
                //BSE Derivative
                else if (Enumerations.Segment.Derivative.ToString() == segment)
                {
                    if (MasterSharedMemory.objMasterDerivativeDictBaseBSE != null && MasterSharedMemory.objMasterDerivativeDictBaseBSE.Count > 0)
                    {
                        if (MasterSharedMemory.objMasterDerivativeDictBaseBSE.ContainsKey(scripCode))
                        {
                            scripName = MasterSharedMemory.objMasterDerivativeDictBaseBSE[scripCode].InstrumentName;
                        }
                    }
                }
                //BSE Currency
                else if (Enumerations.Segment.Currency.ToString() == segment)
                {
                    if (MasterSharedMemory.objMasterCurrencyDictBaseBSE != null && MasterSharedMemory.objMasterCurrencyDictBaseBSE.Count > 0)
                    {
                        if (MasterSharedMemory.objMasterCurrencyDictBaseBSE.ContainsKey(scripCode))
                        {
                            scripName = MasterSharedMemory.objMasterCurrencyDictBaseBSE[scripCode].InstrumentName;
                        }
                    }
                }
            }

            //NSE
#if BOW
            else if (Enumerations.Exchange.NSE.ToString() == exchangeFlag)
            {
                //NSE Equity
                if (Enumerations.Segment.Equity.ToString() == segment)
                {
                    if (MasterSharedMemory.objMastertxtDictBaseNSE != null && MasterSharedMemory.objMastertxtDictBaseNSE.Count > 0)
                    {
                        if (MasterSharedMemory.objMastertxtDictBaseNSE.ContainsKey(scripCode))
                        {
                            scripName = MasterSharedMemory.objMastertxtDictBaseNSE[scripCode].ScripName;
                        }
                    }
                }
                //NSE Derivative
                else if (Enumerations.Segment.Derivative.ToString() == segment)
                {
                }
                //NSE Currency
                else if (Enumerations.Segment.Currency.ToString() == segment)
                {
                }
            }
#endif
            return scripName;
        }
        public static List<string> PopulateIndices()
        {
            List<string> IndicesSet = new List<string>();
            oDataAccessLayer.Connect((int)DataAccessLayer.ConnectionDB.Masters);
            try
            {
                oDataAccessLayer.OpenConnection((int)DataAccessLayer.ConnectionDB.Masters);
                string str = "SELECT Distinct(ExistingShortName) FROM BSE_SNPINDICES_CFE";
                SQLiteDataReader oSQLiteDataReader = oDataAccessLayer.ExecuteDataReader((int)ConnectionDB.Masters, str, System.Data.CommandType.Text, null);
                while (oSQLiteDataReader.Read())
                {
                    if (oSQLiteDataReader["ExistingShortName"] != string.Empty)
                        IndicesSet.Add(oSQLiteDataReader["ExistingShortName"]?.ToString().Trim());
                }

                //if (MasterSharedMemory.objSpnIndicesDic != null)
                //    IndicesSet = MasterSharedMemory.objSpnIndicesDic.Values.Cast<ScripMasterSpnIndices>().GroupBy(x => x.ExistingShortName_ca).Select(x => x.FirstOrDefault().ExistingShortName_ca).ToList();
                return IndicesSet;
            }
            catch (Exception e) { ExceptionUtility.LogError(e); return IndicesSet; }
            finally
            {
                oDataAccessLayer.CloseConnection((int)DataAccessLayer.ConnectionDB.Masters);
                System.Data.SQLite.SQLiteConnection.ClearAllPools();
            }
        }

        public static long GetScripCodeFromScripID(string ScripID)
        {
            long ScripCode = 0;
            if (!string.IsNullOrEmpty(ScripID))
            {
                ScripCode = MasterSharedMemory.objMastertxtDictBaseBSE.Values.Where(x => x.ScripId != null && x.ScripId.Trim().ToUpper().Equals(ScripID.Trim().ToUpper())).Select(x => x.ScripCode).FirstOrDefault();
                if (ScripCode != 0)
                    return ScripCode;

                ScripCode = MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Where(x => x.ScripId != null && x.ScripId.Trim().ToUpper().Equals(ScripID.Trim().ToUpper())).Select(x => x.ContractTokenNum).FirstOrDefault();

                if (ScripCode != 0)
                    return ScripCode;

                ScripCode = MasterSharedMemory.objMasterCurrencyDictBaseBSE.Values.Where(x => x.ScripId != null && x.ScripId.Trim().ToUpper().Equals(ScripID.Trim().ToUpper())).Select(x => x.ContractTokenNum).FirstOrDefault();

                if (ScripCode != 0)
                    return ScripCode;

                //else if (MasterSharedMemory.objMasterDerivativeDictBaseBSE != null && MasterSharedMemory.objMasterDerivativeDictBaseBSE.ContainsKey(scripcode))
                //{
                //    return Segment.Derivative.ToString();
                //}
                //else if (MasterSharedMemory.objMasterCurrencyDictBaseBSE != null && MasterSharedMemory.objMasterCurrencyDictBaseBSE.ContainsKey(scripcode))
                //{
                //    //return Segment.Currency.ToString();
                //}
            }
            return ScripCode;
        }

        public static string GetScripId(long scripCode, string exchangeFlag)
        {
            string scripId = string.Empty;

            if (scripCode != 0)
            {
                //BSE
                if (Enumerations.Exchange.BSE.ToString() == exchangeFlag)
                {
                    if (MasterSharedMemory.objMastertxtDictBaseBSE != null && MasterSharedMemory.objMastertxtDictBaseBSE.ContainsKey(scripCode))
                    {
                        return MasterSharedMemory.objMastertxtDictBaseBSE[scripCode].ScripId;
                    }
                    else if (MasterSharedMemory.objMasterDerivativeDictBaseBSE != null && MasterSharedMemory.objMasterDerivativeDictBaseBSE.ContainsKey(scripCode))
                    {
                        return MasterSharedMemory.objMasterDerivativeDictBaseBSE[scripCode].ScripId;
                    }
                    else if (MasterSharedMemory.objMasterCurrencyDictBaseBSE != null && MasterSharedMemory.objMasterCurrencyDictBaseBSE.ContainsKey(scripCode))
                    {
                        return MasterSharedMemory.objMasterCurrencyDictBaseBSE[scripCode].ScripId;
                    }

                }
            }
            return scripId;
        }

        public static string GetUnderLyingAssetCode(long ScripCode, string exchangeFlag, string SegmentFlag)
        {
            string UnderLyingAssetCode = string.Empty;
            if (ScripCode != 0)
            {
                if (exchangeFlag == Exchange.BSE.ToString())
                {
                    if (SegmentFlag == Segment.Equity.ToString() || SegmentFlag == Segment.Debt.ToString())
                    {
                        UnderLyingAssetCode = string.Empty;
                    }
                    else if (SegmentFlag == Segment.Derivative.ToString())
                    {
                        if (MasterSharedMemory.objMasterDerivativeDictBaseBSE.ContainsKey(ScripCode))
                        {
                            UnderLyingAssetCode = MasterSharedMemory.objMasterDerivativeDictBaseBSE[ScripCode].UnderlyingAsset;
                        }
                    }

                    else if (SegmentFlag == Segment.Currency.ToString())
                    {
                        if (MasterSharedMemory.objMasterCurrencyDictBaseBSE.ContainsKey(ScripCode))
                        {
                            UnderLyingAssetCode = MasterSharedMemory.objMasterCurrencyDictBaseBSE[ScripCode].UnderlyingAsset;
                        }
                    }
                }
            }
            return UnderLyingAssetCode;
        }

        public static void CallBestFiveUsingScripCode(int ScripCode)
        {
            if (ScripCode <= 0)
                return;

            BestFiveMarketPicture objBestFive = System.Windows.Application.Current.Windows.OfType<BestFiveMarketPicture>().FirstOrDefault();

            if (ScripCode != 0)
            {
                if (objBestFive != null)
                {
                    ((OrderEntryUC_VM)objBestFive.OrderEntryUC.DataContext).SelectScripCodeFromTouchline(ScripCode);
                    objBestFive.Show();
                    objBestFive.Focus();
                }
                else
                {
                    objBestFive = new BestFiveMarketPicture();
                    objBestFive.Activate();
                    objBestFive.Owner = System.Windows.Application.Current.MainWindow;
                    ((OrderEntryUC_VM)objBestFive.OrderEntryUC.DataContext).SelectScripCodeFromTouchline(ScripCode);
                    objBestFive.Show();
                }
            }

            else
            {
                if (objBestFive != null)
                {
                    objBestFive.Show();
                    objBestFive.Focus();
                }
                else
                {
                    objBestFive = new BestFiveMarketPicture();
                    objBestFive.Activate();
                    objBestFive.Owner = System.Windows.Application.Current.MainWindow;
                    objBestFive.Show();
                }
            }
        }
        /// <summary>
        /// For Der and Currency
        /// </summary>
        /// <param name="scripcode"></param>
        /// <param name="exchangeFlag"></param>
        /// <param name="segment"></param>
        /// <returns></returns>
        public static string GetInstrumentType(long scripcode, string exchangeFlag, string segment)
        {
            string result = string.Empty;

            //BSE
            if (Enumerations.Exchange.BSE.ToString() == exchangeFlag)
            {
                //BSE Equity
                if (Enumerations.Order.ScripSegment.Equity.ToString() == segment || Enumerations.Order.ScripSegment.Debt.ToString() == segment)
                {
                    return result;
                }
                //BSE Derivative
                else if (Enumerations.Order.ScripSegment.Derivative.ToString() == segment)
                {
                    if (scripcode != 0 && scripcode > 0)
                    {
                        if (MasterSharedMemory.objMasterDerivativeDictBaseBSE != null && MasterSharedMemory.objMasterDerivativeDictBaseBSE.ContainsKey(scripcode))
                        {
                            result = MasterSharedMemory.objMasterDerivativeDictBaseBSE[scripcode].InstrumentType;//added by Gaurav  20/04/2017 15.52
                            return result;
                        }
                    }
                }
                //BSE Currency
                else if (Enumerations.Order.ScripSegment.Currency.ToString() == segment)
                {
                    if (scripcode != 0 && scripcode > 0)
                    {
                        if (MasterSharedMemory.objMasterCurrencyDictBaseBSE != null && MasterSharedMemory.objMasterCurrencyDictBaseBSE.ContainsKey(scripcode))
                        {
                            result = MasterSharedMemory.objMasterCurrencyDictBaseBSE[scripcode].InstrumentType;//added by Gaurav  20/04/2017 15.52
                            return result;
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// For Call and Put Option.
        /// </summary>
        /// <param name="ScripCode"></param>
        /// <param name="exchangeFlag"></param>
        /// <param name="SegmentFlag"></param>
        /// <returns></returns>
        public static string GetOptionType(long scripcode, string exchangeFlag, string segment)
        {
            string result = string.Empty;

            //BSE
            if (Enumerations.Exchange.BSE.ToString() == exchangeFlag)
            {
                //BSE Equity
                if (Enumerations.Order.ScripSegment.Equity.ToString() == segment || Enumerations.Order.ScripSegment.Debt.ToString() == segment)
                {
                    return result;
                }
                //BSE Derivative
                else if (Enumerations.Order.ScripSegment.Derivative.ToString() == segment)
                {
                    if (scripcode != 0 && scripcode > 0)
                    {
                        if (MasterSharedMemory.objMasterDerivativeDictBaseBSE != null && MasterSharedMemory.objMasterDerivativeDictBaseBSE.ContainsKey(scripcode))
                        {
                            result = MasterSharedMemory.objMasterDerivativeDictBaseBSE[scripcode].OptionType;//added by Gaurav  20/04/2017 15.52
                            return result;
                        }
                    }
                }
                //BSE Currency
                else if (Enumerations.Order.ScripSegment.Currency.ToString() == segment)
                {
                    if (scripcode != 0 && scripcode > 0)
                    {
                        if (MasterSharedMemory.objMasterCurrencyDictBaseBSE != null && MasterSharedMemory.objMasterCurrencyDictBaseBSE.ContainsKey(scripcode))
                        {
                            result = MasterSharedMemory.objMasterCurrencyDictBaseBSE[scripcode].OptionType;//added by Gaurav  20/04/2017 15.52
                            return result;
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// For Pair and Straddle. Der and cur
        /// </summary>
        /// <param name="scripcode"></param>
        /// <param name="exchangeFlag"></param>
        /// <param name="segment"></param>
        /// <returns></returns>
        public static int GetStrategyID(long scripcode, string exchangeFlag, string segment)
        {
            int result = 0;

            //BSE
            if (Enumerations.Exchange.BSE.ToString() == exchangeFlag)
            {
                //BSE Equity
                if (Enumerations.Order.ScripSegment.Equity.ToString() == segment || Enumerations.Order.ScripSegment.Debt.ToString() == segment)
                {
                    return result;
                }
                //BSE Derivative
                else if (Enumerations.Order.ScripSegment.Derivative.ToString() == segment)
                {
                    if (scripcode != 0 && scripcode > 0)
                    {
                        if (MasterSharedMemory.objMasterDerivativeDictBaseBSE != null && MasterSharedMemory.objMasterDerivativeDictBaseBSE.ContainsKey(scripcode))
                        {
                            result = MasterSharedMemory.objMasterDerivativeDictBaseBSE[scripcode].StrategyID;//added by Gaurav  20/04/2017 15.52
                            return result;
                        }
                    }
                }
                //BSE Currency
                else if (Enumerations.Order.ScripSegment.Currency.ToString() == segment)
                {
                    if (scripcode != 0 && scripcode > 0)
                    {
                        if (MasterSharedMemory.objMasterCurrencyDictBaseBSE != null && MasterSharedMemory.objMasterCurrencyDictBaseBSE.ContainsKey(scripcode))
                        {
                            result = MasterSharedMemory.objMasterCurrencyDictBaseBSE[scripcode].StrategyID;//added by Gaurav  20/04/2017 15.52
                            return result;
                        }
                    }
                }
            }

            return result;
        }

        public static string GetExpiryDate(long scripcode, string exchangeFlag, string segment)
        {
            string result = string.Empty;

            //BSE
            if (Enumerations.Exchange.BSE.ToString() == exchangeFlag)
            {
                //BSE Equity
                if (Enumerations.Order.ScripSegment.Equity.ToString() == segment || Enumerations.Order.ScripSegment.Debt.ToString() == segment)
                {
                    return result;
                }
                //BSE Derivative
                else if (Enumerations.Order.ScripSegment.Derivative.ToString() == segment)
                {
                    if (scripcode != 0 && scripcode > 0)
                    {
                        if (MasterSharedMemory.objMasterDerivativeDictBaseBSE != null && MasterSharedMemory.objMasterDerivativeDictBaseBSE.ContainsKey(scripcode))
                        {
                            result = MasterSharedMemory.objMasterDerivativeDictBaseBSE[scripcode].DisplayExpiryDate;//added by Gaurav  20/04/2017 15.52
                            return result;
                        }
                    }
                }
                //BSE Currency
                else if (Enumerations.Order.ScripSegment.Currency.ToString() == segment)
                {
                    if (scripcode != 0 && scripcode > 0)
                    {
                        if (MasterSharedMemory.objMasterCurrencyDictBaseBSE != null && MasterSharedMemory.objMasterCurrencyDictBaseBSE.ContainsKey(scripcode))
                        {
                            result = MasterSharedMemory.objMasterCurrencyDictBaseBSE[scripcode].DisplayExpiryDate;//added by Gaurav  20/04/2017 15.52
                            return result;
                        }
                    }
                }
            }

            return result;
        }

        public static string GetStrikePrice(long scripcode, string exchangeFlag, string segment)
        {
            string result = string.Empty;

            //BSE
            if (Enumerations.Exchange.BSE.ToString() == exchangeFlag)
            {
                //BSE Equity
                if (Enumerations.Order.ScripSegment.Equity.ToString() == segment || Enumerations.Order.ScripSegment.Debt.ToString() == segment)
                {
                    return result;
                }
                //BSE Derivative
                else if (Enumerations.Order.ScripSegment.Derivative.ToString() == segment)
                {
                    if (scripcode != 0 && scripcode > 0)
                    {
                        if (MasterSharedMemory.objMasterDerivativeDictBaseBSE != null && MasterSharedMemory.objMasterDerivativeDictBaseBSE.ContainsKey(scripcode))
                        {
                            result = Convert.ToString(MasterSharedMemory.objMasterDerivativeDictBaseBSE[scripcode].StrikePrice);//added by Gaurav  20/04/2017 15.52
                            return result;
                        }
                    }
                }
                //BSE Currency
                else if (Enumerations.Order.ScripSegment.Currency.ToString() == segment)
                {
                    if (scripcode != 0 && scripcode > 0)
                    {
                        if (MasterSharedMemory.objMasterCurrencyDictBaseBSE != null && MasterSharedMemory.objMasterCurrencyDictBaseBSE.ContainsKey(scripcode))
                        {
                            result = Convert.ToString(MasterSharedMemory.objMasterCurrencyDictBaseBSE[scripcode].StrikePrice);//added by Gaurav  20/04/2017 15.52
                            return result;
                        }
                    }
                }
            }

            return result;
        }

        public static int SegmentFlag(string Segment)
        {
            if (Order.ScripSegment.Equity.ToString().Trim().ToUpper() == Segment.Trim().ToUpper() || Order.ScripSegment.Debt.ToString().Trim().ToUpper() == Segment.Trim().ToUpper())
            {
                return 1;
            }
            else if (Order.ScripSegment.Derivative.ToString().Trim().ToUpper() == Segment.Trim().ToUpper())
            {
                return 2;
            }
            else if (Order.ScripSegment.Currency.ToString().Trim().ToUpper() == Segment.Trim().ToUpper())
            {
                return 3;
            }
            return 0;
        }

        public static void LogOrderTrade(OrderModel oOrderModel, TradeUMS oTradeUMS, int flag)
        {


            //string fileName = "";

            if (!headerflag)
            {

                DirectoryInfo LogPath = new DirectoryInfo(
Path.GetFullPath(Path.Combine(UtilityApplicationDetails.GetInstance.CurrentDirectory, @"LogFiles/" + System.DateTime.Now.ToString("dd-MM-yyyy") + "TradeOrder.txt")));

                if (!Directory.Exists(UtilityApplicationDetails.GetInstance.CurrentDirectory + "/LogFiles"))
                    Directory.CreateDirectory(UtilityApplicationDetails.GetInstance.CurrentDirectory + "/LogFiles");
                fileName = LogPath.ToString();
                writer = new StreamWriter(fileName, true, Encoding.UTF8);
                writer.Write("TypeOT", "BuySell, PendingQty,TradedQty, RevQty, SCode, ScripID, Rate, ClientID, Time, OrdID, ClientType, RetainTill");
                writer.Write(writer.NewLine);
                headerflag = true;

            }
            if (flag == 1)
            {

                //writer = new StreamWriter(fileName, true, Encoding.UTF8);
                writer.Write("Order" + "," + oOrderModel.BuySellIndicator + "," + oOrderModel.PendingQuantity + "," + oOrderModel.TradedQty + "," + oOrderModel.RevealQty + "," + oOrderModel.ScripCode + "," + oOrderModel.Symbol + "," + oOrderModel.Price + "," + oOrderModel.ClientId + "," +
                        oOrderModel.Time + "," + oOrderModel.OrderId + "," + oOrderModel.ClientType + "," + oOrderModel.OrderRetentionStatus);

                writer.Write(writer.NewLine);
            }
            else if (flag == 2)
            {

                //writer = new StreamWriter(fileName, true, Encoding.UTF8);
                writer.Write("Trade" + "," + oTradeUMS.BSFlag + "," + oTradeUMS.LastQty + "," + oTradeUMS.LastQty + "," + oTradeUMS.LastQty + "," + oTradeUMS.ScripCode + "," + oTradeUMS.ScripID + "," + oTradeUMS.LastPx + "," + oTradeUMS.Client + "," +
                    oTradeUMS.TimeOnly + "," + oTradeUMS.OrderID + "," + oTradeUMS.ClientType + "," + "EOS");

                writer.Write(writer.NewLine);
            }
            else if (flag == 3)
            {
                //writer = new StreamWriter(fileName, true, Encoding.UTF8);
                writer.Write("MissingTrade" + "," + oTradeUMS.BSFlag + "," + oTradeUMS.LastQty + "," + oTradeUMS.LastQty + "," + oTradeUMS.LastQty + "," + oTradeUMS.ScripCode + "," + oTradeUMS.ScripID + "," + oTradeUMS.LastPx + "," + oTradeUMS.Client + "," +
                    oTradeUMS.TimeOnly + "," + oTradeUMS.OrderID + "," + oTradeUMS.ClientType + "," + "EOS");

                writer.Write(writer.NewLine);
            }
            if (writer != null)
            {
                writer.Flush();
                // writer.Close();
            }
        }


        internal static string GetValueInDecimal(long price, int decimal_Point)
        {
            string ValueInDecimalFormat = string.Empty;

            if (decimal_Point == 4)
                ValueInDecimalFormat = string.Format("{0:0.0000}", Convert.ToDecimal(price / Math.Pow(10, decimal_Point))).ToString();

            else if (decimal_Point == 2)
                ValueInDecimalFormat = string.Format("{0:0.00}", Convert.ToDecimal(price / Math.Pow(10, decimal_Point))).ToString();

            else
                ValueInDecimalFormat = string.Format("{0:0.00}", Convert.ToDecimal(price / Math.Pow(10, decimal_Point))).ToString();

            if (ValueInDecimalFormat == "0" || ValueInDecimalFormat == "0.00" || ValueInDecimalFormat == "0.0000")
                return string.Empty;

            return ValueInDecimalFormat;
        }
        public static long CalculateRevQty(long MarketLot, long qty)
        {
            double prof_rev_qty;
            double rev_qty_percent;
            double calc_rev_qty;
            long result_rev_qty;
            long temp_rev_qty;
            long fraction_l;
            prof_rev_qty = Convert.ToDouble(UtilityOrderDetails.GETInstance.RevlQtyPercentage);
            rev_qty_percent = (prof_rev_qty) / 100; /* In percent */
            calc_rev_qty = (qty) * (rev_qty_percent);
            //if (UtilityOrderDetails.GETInstance.RevlQtyPercentage.Contains("."))
            //{
            //    prof_rev_qty = UtilityOrderDetails.GETInstance.RevlQtyPercentage;
            //    rev_qty_percent = Convert.ToDecimal(prof_rev_qty) / 100; /* In percent */
            //}
            //else
            //{
            //    rev_qty_percent = Convert.ToDecimal(UtilityOrderDetails.GETInstance.RevlQtyPercentage) / 100;
            //}

            //string calc_rev_qtyString = string.Format("{0:0.00}", (qty * rev_qty_percent));
            //calc_rev_qty = Convert.ToDecimal(calc_rev_qtyString);
            if (calc_rev_qty < MarketLot)         /* If Calc. Qty less than Market Lot then Set 'Resultant Rev.Qty' = Market Lot */
            {
                result_rev_qty = MarketLot;
                return (result_rev_qty);
            }
            else
            {
                string calc_rev_qtyInString = string.Format("{0:0.00}", calc_rev_qty);
                string[] arr = calc_rev_qtyInString.Split('.');
                double interger_part_f = Convert.ToDouble(arr[0]);
                double fraction_part_f = Convert.ToDouble(arr[1]);
                if (fraction_part_f == 0)
                {
                    temp_rev_qty = (long)interger_part_f;
                    if ((temp_rev_qty % MarketLot) == 0) /* If Multiple of Market Lot */
                    {
                        result_rev_qty = temp_rev_qty;

                        return (result_rev_qty);
                    }
                    else /* If Not  Multiple of Market Lot */
                    {
                        fraction_l = (long)((calc_rev_qty) / (MarketLot));

                        temp_rev_qty = (fraction_l + 1) * (MarketLot); /* Increamental Market Lot */

                        if (temp_rev_qty > UtilityOrderDetails.GETInstance.MAXQTYLIMIT)
                        {
                            result_rev_qty = (fraction_l) * (MarketLot);
                        }
                        else
                        {
                            result_rev_qty = temp_rev_qty;
                        }

                        return (result_rev_qty);
                    }
                }
                else
                {
                    fraction_l = (long)((calc_rev_qty) / (MarketLot));

                    temp_rev_qty = (fraction_l + 1) * (MarketLot); /* Increamental Market Lot */
                    if (temp_rev_qty > UtilityOrderDetails.GETInstance.MAXQTYLIMIT)
                    {
                        result_rev_qty = (fraction_l) * (MarketLot);
                    }
                    else
                    {
                        result_rev_qty = temp_rev_qty;
                    }

                    return (result_rev_qty);
                }
            }

        }
    }
    #region ExtensionMethods


    /// <summary>
    /// Truncates the string as per Max length specified in Input Parameter
    /// <maxLength>Max length for input string</maxLength>
    /// Added by Gaurav Jadhav 06/07/2017
    /// </summary>
    public static class StringExt
    {
        public static string Truncate(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }
    }
    #endregion
}