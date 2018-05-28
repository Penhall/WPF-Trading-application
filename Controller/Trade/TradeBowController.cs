using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonFrontEnd.Common;
using CommonFrontEnd.Model.Trade;
using CommonFrontEnd.Constants;
using CommonFrontEnd.Controller.Order;
using CommonFrontEnd.Processor.Trade;

namespace CommonFrontEnd.Controller.Trade
{
    public class TradeBowController
    {
#if BOW
        #region Methods
        /// <summary>
        /// Process Trade Messages received from BOW(Msg Code - 76) and Add in Trade Memory
        /// </summary>
        /// <param name="pobjRecordHelper"></param>
        internal static void ProcessBowTrade(RecordSplitter pobjRecordHelper)
        {
            string ExchangeId = string.Empty;
            string SegmentID = string.Empty;
            string InstrumentType = string.Empty;
            string IntraDayFlag = string.Empty;
            string Price = string.Empty;
            string BookTypeFlag = string.Empty;
            string ProClient = string.Empty;
            string StrikePrice = string.Empty;

            TradeBowModel tmodel = new TradeBowModel();
            tmodel.TransactionCode = Convert.ToInt32(pobjRecordHelper.getField(0, Convert.ToInt32(Enumerations.Trade.TRTransactionCode)));
            tmodel.OrderNumber = pobjRecordHelper.getField(0, Convert.ToInt32(Enumerations.Trade.TROrderNumber)).Trim();
            tmodel.TradeNumber = pobjRecordHelper.getField(0, Convert.ToInt32(Enumerations.Trade.TRNumber)).Trim();
            tmodel.TradeID = pobjRecordHelper.getField(0, Convert.ToInt32(Enumerations.Trade.TRID)).Trim();
            tmodel.UserID = pobjRecordHelper.getField(0, Convert.ToInt32(Enumerations.Trade.TRNewUsId)).Trim();
            ExchangeId = pobjRecordHelper.getField(0, Convert.ToInt32(Enumerations.Trade.TRExchange)).Trim();
            tmodel.Exchange = Enum.GetName(typeof(Enumerations.Order.Exchanges), Convert.ToInt32(ExchangeId));

            //pobjRecordHelper.setField(0, LDB.Trade.TradesConfirmation.TRExchange, gobjUtilityEMS.GetExchangeString(lstrExchangeId));
            SegmentID = pobjRecordHelper.getField(0, Convert.ToInt32(Enumerations.Trade.TRMarket)).Trim();
            tmodel.Market = Enum.GetName(typeof(Enumerations.Order.ScripSegment), Convert.ToInt32(SegmentID));

            InstrumentType = pobjRecordHelper.getField(0, Convert.ToInt32(Enumerations.Trade.Size + 1)).Trim();
            if (string.IsNullOrWhiteSpace(pobjRecordHelper.getField(0, Convert.ToInt32(Enumerations.Trade.Size + 8))))
            {
                IntraDayFlag = "N";
            }
            else
            {
                IntraDayFlag = pobjRecordHelper.getField(0, Convert.ToInt32(Enumerations.Trade.Size + 8)).Trim();
            }

            //pobjRecordHelper.setField(0, LDB.Trade.TradesConfirmation.TRMarket, gobjUtilityEMS.GetSegmentString(UtilityEMS.GetSegmentIDBasedonInstrument(lstrInstrumentType, lstrIntraDayFlag)));
            tmodel.Token = pobjRecordHelper.getField(0, Convert.ToInt32(Enumerations.Trade.TRToken)).Trim();
            tmodel.Symbol = pobjRecordHelper.getField(0, Convert.ToInt32(Enumerations.Trade.Size + 2)).Trim();
            tmodel.Scripcode = CommonFunctions.GetScripCodeFromScripID(tmodel.Symbol, tmodel.Exchange, tmodel.Market);
            tmodel.ScripName = CommonFunctions.GetScripName(tmodel.Scripcode, tmodel.Exchange, tmodel.Market);
            tmodel.Price = Convert.ToInt64(pobjRecordHelper.getField(0, Convert.ToInt32(Enumerations.Trade.TRPrice)).Trim());

            tmodel.TotalValue = pobjRecordHelper.getField(0, Convert.ToInt32(Enumerations.Trade.Size + 9)).Trim();
            tmodel.Remarks = Convert.ToInt64(pobjRecordHelper.getField(0, Convert.ToInt32(Enumerations.Trade.TRFIELD4)).Trim().Remove(0, 2));
            tmodel.Volume = Convert.ToInt32(pobjRecordHelper.getField(0, Convert.ToInt32(Enumerations.Trade.TRVolume)).Trim());
            tmodel.RemainingVolume = Convert.ToInt32(pobjRecordHelper.getField(0, Convert.ToInt32(Enumerations.Trade.TRRemainingVolume)).Trim());
            tmodel.AdminUSId = pobjRecordHelper.getField(0, Convert.ToInt32(Enumerations.Trade.TRAdminUSId)).Trim();
            tmodel.AdminUSBackOfficeId = pobjRecordHelper.getField(0, Convert.ToInt32(Enumerations.Trade.TRAdminUSBackOfficeId)).Trim();


            //if (tmodel.Price != 0)
            //{
            //    if (SegmentID == BowConstants.MKT_CURRENCY_VALUE.ToString())
            //    {
            //        //TODO: Currency Related Price
            //        //pobjRecordHelper.setField(0, LDB.Trade.TradesConfirmation.TRPrice, FormatNumber((lstrPrice / 10000), 4));
            //        //TODO: Currency Related Price
            //    }
            //    else
            //    {
            //        //pobjRecordHelper.setField(0, LDB.Trade.TradesConfirmation.TRPrice, FormatNumber((lstrPrice / 100), 2));
            //    }
            //}

            string lstrTime = pobjRecordHelper.getField(0, Convert.ToInt32(Enumerations.Trade.TRLogTime)).Trim();
            if (lstrTime != null && Convert.ToDouble(lstrTime) > 0)
            {
                if (ExchangeId == BowConstants.EX_USE_VALUE.ToString() || ExchangeId == BowConstants.EX_BSE_VALUE.ToString() || ExchangeId == BowConstants.EX_MCX_VALUE.ToString()
                    || ExchangeId == BowConstants.EX_NCDEX_VALUE.ToString())
                {
                    tmodel.LogTime = new DateTime(1970, 1, 1).AddSeconds(1512041570).ToString();//LocalDbUtility.ConvertLongToStringDate(lstrTime, true);
                    //pobjRecordHelper.setField(0, Convert.ToInt32(Enumerations.Trade.TRLogTime), LocalDbUtility.ConvertLongToStringDate(lstrTime, true));
                }
                else
                {
                    tmodel.LogTime = new DateTime(1980, 1, 1).AddSeconds(1512041570).ToString();
                    //pobjRecordHelper.setField(0, Convert.ToInt32(Enumerations.Trade.TRLogTime), LocalDbUtility.ConvertLongToStringDate(lstrTime, false));
                }
            }

            if (IntraDayFlag == "Y")
            {
                tmodel.IntraDayFlag = "CNC";
                //pobjRecordHelper.setField(0, Convert.ToInt32(Enumerations.Trade.Size + 8), "CNC");
            }
            else
            {
                tmodel.IntraDayFlag = "Intra";
                //pobjRecordHelper.setField(0, (int)LDB.Trade.TradesConfirmation.Size + 8, "Intra");
            }

            IntraDayFlag = pobjRecordHelper.getField(0, Convert.ToInt32(Enumerations.Trade.Size + 7)).Trim();
            if (IntraDayFlag == "1")
            {
                tmodel.BuySellIndicator = Enumerations.Order.BuySellFlag.BUY.ToString();
                // pobjRecordHelper.setField(0, (int)LDB.Trade.TradesConfirmation.Size + 7, "Buy");
            }
            else
            {
                tmodel.BuySellIndicator = Enumerations.Order.BuySellFlag.SELL.ToString();
                // pobjRecordHelper.setField(0, (int)LDB.Trade.TradesConfirmation.Size + 7, "Sell");
            }

            StrikePrice = pobjRecordHelper.getField(0, Convert.ToInt32(Enumerations.Trade.Size + 5)).Trim();
            long StrikePrice_1 = 0;
            if (StrikePrice != null && long.TryParse(StrikePrice, out StrikePrice_1) == true && Convert.ToInt32(StrikePrice) == -1)
            {
                tmodel.StrikePrice = 0;
                //pobjRecordHelper.setField(0, (int)LDB.Trade.TradesConfirmation.Size + 5, "0.00");
            }

            if (tmodel.TransactionCode.Equals(StockConstants.TRADE_CONFIRMATION) || tmodel.TransactionCode.Equals(StockConstants.ON_STOP_NOTIFICATION) ||
                 tmodel.TransactionCode.Equals(StockConstants.TRADE_MODIFICATION_CONFIRMATION) || tmodel.TransactionCode.Equals(StockConstants.TRADE_INTRADAY_TO_DELIVERY) ||
                 tmodel.TransactionCode.Equals("5445"))
            {
                if (tmodel.TransactionCode.Equals(StockConstants.TRADE_CONFIRMATION) || tmodel.TransactionCode.Equals(StockConstants.ON_STOP_NOTIFICATION) || tmodel.TransactionCode.Equals("5445"))
                {
                    if (tmodel.TransactionCode.Equals(StockConstants.ON_STOP_NOTIFICATION))
                    {
                        if (tmodel.Symbol.Length > 10 && ExchangeId.Equals(BowConstants.EX_USE_VALUE.ToString()) && OrderBowController.GetSegmentIDBasedonInstrument(InstrumentType).Equals(BowConstants.SGT_FUTURES_VALUE.ToString()))
                        {
                            if (Convert.ToInt64(tmodel.OrderNumber) > 0)
                            {
                                // GetOrderList(lstrMarketId, lstrOrderNumber, lstrUserId, lstrExchangeId, lstrToken, lstrTradeNumber, lstrTransactionCode, pobjRecordHelper);
                                // RefreshBooksInMaster("O", lstrUserId, lstrToken, lstrExchangeId, lstrMarketId, lstrOrderNumber, "", lstrTransactionCode, pobjRecordHelper);
                                //RefreshActivityLog(pobjRecordHelper, lstrTransactionCode);
                                return;
                            }
                        }

                        if (ExchangeId.Equals(BowConstants.EX_BSE_VALUE) && SegmentID.Equals(BowConstants.MKT_SLB_VALUE))
                        {
                            //GetSLBTradeList(lstrTradeNumber, lstrUserId, lstrExchangeId, lstrToken, lstrOrderNumber, pobjRecordHelper);
                        }
                        else
                        {
                            //GetTradeList(lstrTradeNumber, lstrUserId, lstrExchangeId, lstrToken, lstrOrderNumber, pobjRecordHelper);
                            //RefreshBooksInMaster("T", lstrUserId, lstrToken, lstrExchangeId, lstrMarketId, lstrOrderNumber, lstrTradeNumber, lstrTransactionCode, pobjRecordHelper);
                        }
                    }

                    //GetOrderList(lstrMarketId, lstrOrderNumber, lstrUserId, lstrExchangeId, lstrToken, lstrTradeNumber, lstrTransactionCode, pobjRecordHelper);
                    // RefreshBooksInMaster("O", lstrUserId, lstrToken, lstrExchangeId, lstrMarketId, lstrOrderNumber, "", lstrTransactionCode, pobjRecordHelper);
                    if (tmodel.TransactionCode.Equals(StockConstants.TRADE_CONFIRMATION))
                    {

                        TradeBowProcessor.ProcessAddTrade(tmodel);
                        // GetNetPosition_TradeNetWise(lstrUserId, lstrToken, lstrExchangeId, pobjRecordHelper);
                        //  if (gblnAutoQuoteEntry == true)
                        //  {
                        //    GetStrategyList(pobjRecordHelper);
                        //  }

                        // RefreshMarginSummary(pobjRecordHelper);
                    }
                }
                else if (tmodel.TransactionCode.Equals(StockConstants.TRADE_MODIFICATION_CONFIRMATION) || tmodel.TransactionCode.Equals(StockConstants.TRADE_INTRADAY_TO_DELIVERY))
                {
                    if (ExchangeId.Equals(BowConstants.EX_USE_VALUE.ToString()) && OrderBowController.GetSegmentIDBasedonInstrument(InstrumentType).Equals(BowConstants.SGT_FUTURES_VALUE.ToString()))
                    {
                        if (!(tmodel.Symbol.Length > 10 && Convert.ToInt64(tmodel.OrderNumber) > 0))
                        {
                            //GetTradeList(lstrTradeNumber, lstrUserId, lstrExchangeId, lstrToken, lstrOrderNumber, pobjRecordHelper);
                            // RefreshBooksInMaster("T", lstrUserId, lstrToken, lstrExchangeId, lstrMarketId, lstrOrderNumber, lstrTradeNumber, lstrTransactionCode, pobjRecordHelper);
                        }
                    }
                    else
                    {
                        if (ExchangeId.Equals(BowConstants.EX_BSE_VALUE.ToString()) && SegmentID.Equals(BowConstants.MKT_SLB_VALUE.ToString()))
                        {
                            //GetSLBTradeList(lstrTradeNumber, lstrUserId, lstrExchangeId, lstrToken, lstrOrderNumber, pobjRecordHelper);
                        }
                        else
                        {
                            // GetTradeList(lstrTradeNumber, lstrUserId, lstrExchangeId, lstrToken, lstrOrderNumber, pobjRecordHelper);
                            //RefreshBooksInMaster("T", lstrUserId, lstrToken, lstrExchangeId, lstrMarketId, lstrOrderNumber, lstrTradeNumber, lstrTransactionCode, pobjRecordHelper);
                        }
                    }
                }

                //if (Is_ExcelOpen() == true && gblnOpenExcelTradeBook == true)
                //{
                //    SocketConnection.MessageArrivedEventArgs lobjMessageEvent = new SocketConnection.MessageArrivedEventArgs(pobjRecordHelper.GetActualData().ToString());
                //    if (!gobjSync_ExcelAndChartMsgQueue == null)
                //    {
                //        lblnISSendExcel = true;
                //        gobjSync_ExcelAndChartMsgQueue.Enqueue(lobjMessageEvent);
                //    }
                //}
            }

            //RefreshActivityLog(pobjRecordHelper, lstrTransactionCode);

        }

        #endregion
#endif
    }
}
