using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using CommonFrontEnd.Common;
using CommonFrontEnd.Model.Trade;
using CommonFrontEnd.ViewModel.Trade;
using System.ComponentModel;
using CommonFrontEnd.Processor;
using CommonFrontEnd.Global;

namespace CommonFrontEnd.SharedMemories
{
#if TWS
    public class NetPositionMemory
    {

        #region Events



        #endregion

        public static ObservableCollection<ClientWisePositionModel> NetPositionCWDataCollection { get; set; }
        public static ObservableCollection<ScripWisePositionModel> NetPositionSWDataCollection { get; set; }
        public static ObservableCollection<ScripWiseDetailPositionModel> NetPositionSWCWDataCollection { get; set; }
        public static ObservableCollection<CWSWDetailPositionModel> NetPositionCWSWDataCollection { get; set; }
        //public static ObservableCollection<SaudasUMSModel> TraderTradeDataCollection { get; set; }
        public static ObservableCollection<TradeUMS> TraderTradeDataCollection { get; set; }
        //public static Action<ObservableCollection<SaudasUMSModel>> AD2TRDataUpdation;


        #region Properties
        private static object ObjectCW = new object();
        private static object ObjectSW = new object();
        private static object ObjectCWSW = new object();
        private static object ObjectSWCW = new object();
        private static object ObjectTrade = new object();

        #endregion
        public static void Initialize()
        {
            NetPositionCWDataCollection = new ObservableCollection<ClientWisePositionModel>();
            NetPositionSWDataCollection = new ObservableCollection<ScripWisePositionModel>();
            NetPositionSWCWDataCollection = new ObservableCollection<ScripWiseDetailPositionModel>();
            NetPositionCWSWDataCollection = new ObservableCollection<CWSWDetailPositionModel>();
            //TraderTradeDataCollection = new ObservableCollection<TradeUMS>();


            //CommonFrontEnd.Processor.UMSProcessor.OnTradeReceivedCW += UpdateClientNetPosition;
            //CommonFrontEnd.Processor.UMSProcessor.OnTradeReceivedSW += UpdateScripNetPosition;
            //CommonFrontEnd.Processor.UMSProcessor.OnTradeReceivedSWD += UpdateScripNetPositionDetail;
            //CommonFrontEnd.Processor.UMSProcessor.OnTradeReceivedCWSWD += UpdateCWSWDNetPosition;
            //CommonFrontEnd.Processor.UMSProcessor.OnTradeReceivedTrader += UpdateTraderTradeData;


            BindingOperations.EnableCollectionSynchronization(NetPositionCWDataCollection, ObjectCW);
            BindingOperations.EnableCollectionSynchronization(NetPositionSWDataCollection, ObjectSW);
            BindingOperations.EnableCollectionSynchronization(NetPositionSWCWDataCollection, ObjectSWCW);
            BindingOperations.EnableCollectionSynchronization(NetPositionCWSWDataCollection, ObjectCWSW);
            //BindingOperations.EnableCollectionSynchronization(TraderTradeDataCollection, ObjectTrade);

        }

        public static void UpdateClientNetPosition(string ClientId, List<KeyValuePair<string, object>> Obj)
        {
            int index = 0;
            if (UtilityLoginDetails.GETInstance.Role.ToLower() == "trader")
            {
                try
                {
                    var results = Obj.GroupBy(p => ((NetPosition)p.Value).ClientId,
                                    p => p.Value,
                                    (key, g) => new
                                    {
                                        clientID = key,
                                        clientData = g.ToList()
                                    }
                                   );

                    foreach (var item in results)
                    {
                        NetPosition oNetPosition = new NetPosition();
                        oNetPosition = (NetPosition)item.clientData.FirstOrDefault();

                        var segment = CommonFunctions.GetSegmentID(oNetPosition.ScripCode);
                        var decimalPoint = CommonFunctions.GetDecimal(Convert.ToInt32(oNetPosition.ScripCode), "BSE", segment);
                        var multiplier = CommonFunctions.GetQuantityMultiplier(oNetPosition.ScripCode, "BSE", segment);

                        ClientWisePositionModel oClientWisePositionModel = new ClientWisePositionModel();
                        oClientWisePositionModel.TraderId = oNetPosition.TraderId;
                        oClientWisePositionModel.ClientId = oNetPosition.ClientId;
                        oClientWisePositionModel.ClientType = oNetPosition.ClientType;
                        //oClientWisePositionModel.NetValue = oNetPosition.NetValue;
                        oClientWisePositionModel.GrossPurchase = item.clientData.Sum(x => ((NetPosition)x).GrossPurchase);

                        oClientWisePositionModel.GrossSell = item.clientData.Sum(x => ((NetPosition)x).GrossSell);
                        oClientWisePositionModel.NetValue = Convert.ToInt64((oClientWisePositionModel.GrossPurchase) - (oClientWisePositionModel.GrossSell));
                        {
                            oClientWisePositionModel.GrossPurchaseString = string.Format("{0:0.00}", (oClientWisePositionModel.GrossPurchase / (Math.Pow(10, UtilityApplicationDetails.GetInstance.MaxDecimalPoint))));
                            oClientWisePositionModel.GrossSellString = string.Format("{0:0.00}", (oClientWisePositionModel.GrossSell / (Math.Pow(10, UtilityApplicationDetails.GetInstance.MaxDecimalPoint))));
                        }

                        var NetPL = item.clientData.Cast<NetPosition>().Sum(x => x.NetPL);
                        oClientWisePositionModel.NetPL = Convert.ToInt64(NetPL / (Math.Pow(10, UtilityApplicationDetails.GetInstance.MaxDecimalPoint)));
                        oClientWisePositionModel.NetValue = Convert.ToInt64(oClientWisePositionModel.NetValue / (Math.Pow(10, UtilityApplicationDetails.GetInstance.MaxDecimalPoint)));
                        var RealPL = item.clientData.Cast<NetPosition>().Sum(x => x.RealPL);
                        oClientWisePositionModel.RealPL = Convert.ToInt64(RealPL / (Math.Pow(10, UtilityApplicationDetails.GetInstance.MaxDecimalPoint)));
                        var UnRealPL = item.clientData.Cast<NetPosition>().Sum(x => x.UnRealPL);
                        oClientWisePositionModel.UnRealPL = Convert.ToInt64(UnRealPL / (Math.Pow(10, UtilityApplicationDetails.GetInstance.MaxDecimalPoint)));

                        App.Current.Dispatcher.BeginInvoke((Action)delegate ()
                          {
                              if (NetPositionMemory.NetPositionCWDataCollection != null && NetPositionMemory.NetPositionCWDataCollection.Count > 0)
                              {
                                  if (NetPositionMemory.NetPositionCWDataCollection.Any(x => x.ClientId == oNetPosition.ClientId))
                                  {
                                      index = NetPositionMemory.NetPositionCWDataCollection.IndexOf(NetPositionMemory.NetPositionCWDataCollection.Where(x => x.ClientId == oNetPosition.ClientId).FirstOrDefault());
                                      NetPositionMemory.NetPositionCWDataCollection[index] = oClientWisePositionModel;
                                  }
                                  else
                                  {
                                      NetPositionMemory.NetPositionCWDataCollection.Add(oClientWisePositionModel);
                                  }
                              }
                              else
                              {
                                  NetPositionMemory.NetPositionCWDataCollection.Add(oClientWisePositionModel);
                              }
                          });

                    }

                }
                catch (Exception ex)
                {
                    ExceptionUtility.LogError(ex);
                }
            }
            else if (UtilityLoginDetails.GETInstance.Role.ToLower() == "admin")
            {
                try
                {
                    var results = Obj.GroupBy(p => ((NetPosition)p.Value).TraderId,
                                    p => p.Value,
                                    (key, g) => new
                                    {
                                        clientID = key,
                                        clientData = g.ToList()
                                    }
                                   );

                    foreach (var item in results)
                    {
                        NetPosition oNetPosition = new NetPosition();
                        oNetPosition = (NetPosition)item.clientData.FirstOrDefault();

                        var segment = CommonFunctions.GetSegmentID(oNetPosition.ScripCode);
                        var decimalPoint = CommonFunctions.GetDecimal(Convert.ToInt32(oNetPosition.ScripCode), "BSE", segment);
                        var multiplier = CommonFunctions.GetQuantityMultiplier(oNetPosition.ScripCode, "BSE", segment);

                        ClientWisePositionModel oClientWisePositionModel = new ClientWisePositionModel();
                        oClientWisePositionModel.TraderId = oNetPosition.TraderId;
                        oClientWisePositionModel.TraderIdString = string.Format("Trd Id-{0}", oNetPosition.TraderId);

                        oClientWisePositionModel.ClientId = oNetPosition.ClientId;
                        oClientWisePositionModel.ClientType = oNetPosition.ClientType;
                        //oClientWisePositionModel.NetValue = oNetPosition.NetValue;
                        oClientWisePositionModel.GrossPurchase = item.clientData.Sum(x => ((NetPosition)x).GrossPurchase);

                        oClientWisePositionModel.GrossSell = item.clientData.Sum(x => ((NetPosition)x).GrossSell);
                        oClientWisePositionModel.NetValue = Convert.ToInt64((oClientWisePositionModel.GrossPurchase) - (oClientWisePositionModel.GrossSell));
                        {
                            oClientWisePositionModel.GrossPurchaseString = string.Format("{0:0.00}", (oClientWisePositionModel.GrossPurchase / (Math.Pow(10, UtilityApplicationDetails.GetInstance.MaxDecimalPoint))));
                            oClientWisePositionModel.GrossSellString = string.Format("{0:0.00}", (oClientWisePositionModel.GrossSell / (Math.Pow(10, UtilityApplicationDetails.GetInstance.MaxDecimalPoint))));
                        }

                        var NetPL = item.clientData.Cast<NetPosition>().Sum(x => x.NetPL);
                        oClientWisePositionModel.NetPL = Convert.ToInt64(NetPL / (Math.Pow(10, UtilityApplicationDetails.GetInstance.MaxDecimalPoint)));
                        oClientWisePositionModel.NetValue = Convert.ToInt64(oClientWisePositionModel.NetValue / (Math.Pow(10, UtilityApplicationDetails.GetInstance.MaxDecimalPoint)));
                        var RealPL = item.clientData.Cast<NetPosition>().Sum(x => x.RealPL);
                        oClientWisePositionModel.RealPL = Convert.ToInt64(RealPL / (Math.Pow(10, UtilityApplicationDetails.GetInstance.MaxDecimalPoint)));
                        var UnRealPL = item.clientData.Cast<NetPosition>().Sum(x => x.UnRealPL);
                        oClientWisePositionModel.UnRealPL = Convert.ToInt64(UnRealPL / (Math.Pow(10, UtilityApplicationDetails.GetInstance.MaxDecimalPoint)));

                        App.Current.Dispatcher.BeginInvoke((Action)delegate ()
                        {
                            if (NetPositionMemory.NetPositionCWDataCollection != null && NetPositionMemory.NetPositionCWDataCollection.Count > 0)
                            {
                                if (NetPositionMemory.NetPositionCWDataCollection.Any(x => x.TraderId == oNetPosition.TraderId))
                                {
                                    index = NetPositionMemory.NetPositionCWDataCollection.IndexOf(NetPositionMemory.NetPositionCWDataCollection.Where(x => x.TraderId == oNetPosition.TraderId).FirstOrDefault());
                                    NetPositionMemory.NetPositionCWDataCollection[index] = oClientWisePositionModel;
                                }
                                else
                                {
                                    NetPositionMemory.NetPositionCWDataCollection.Add(oClientWisePositionModel);
                                }
                            }
                            else
                            {
                                NetPositionMemory.NetPositionCWDataCollection.Add(oClientWisePositionModel);
                            }
                        });

                    }

                }
                catch (Exception ex)
                {
                    ExceptionUtility.LogError(ex);
                }
            }
            #region Admin Commented


            //if (UtilityLoginDetails.GETInstance.Role.ToLower() == "admin")
            //{
            //    try
            //    {
            //        var results = Obj.GroupBy(p => ((NetPosition)p.Value).TraderId,
            //                        p => p.Value,
            //                        (key, g) => new
            //                        {
            //                            clientID = key,
            //                            clientData = g.ToList()
            //                        }
            //                       );

            //        foreach (var item in results)
            //        {
            //            NetPosition oNetPosition = new NetPosition();
            //            oNetPosition = (NetPosition)item.clientData.FirstOrDefault();

            //            var segment = CommonFunctions.GetSegmentID(oNetPosition.ScripCode);
            //            var decimalPoint = CommonFunctions.GetDecimal(Convert.ToInt32(oNetPosition.ScripCode), "BSE", segment);
            //            var multiplier = CommonFunctions.GetQuantityMultiplier(oNetPosition.ScripCode, "BSE", segment);

            //            ClientWisePositionModel oClientWisePositionModel = new ClientWisePositionModel();
            //            oClientWisePositionModel.TraderId = oNetPosition.TraderId;
            //            oClientWisePositionModel.ClientId = oNetPosition.ClientId;
            //            oClientWisePositionModel.ClientType = oNetPosition.ClientType;
            //            oClientWisePositionModel.NetValue = oNetPosition.NetValue;
            //            oClientWisePositionModel.GrossPurchase = item.clientData.Sum(x => ((NetPosition)x).GrossPurchase);

            //            oClientWisePositionModel.GrossSell = item.clientData.Sum(x => ((NetPosition)x).GrossSell);

            //            //if (decimalPoint == 4)
            //            //{
            //            //    oClientWisePositionModel.GrossPurchaseString = string.Format("{0:0.0000}", (oClientWisePositionModel.GrossPurchase / (Math.Pow(10, decimalPoint))));
            //            //    oClientWisePositionModel.GrossSellString = string.Format("{0:0.0000}", (oClientWisePositionModel.GrossSell / (Math.Pow(10, decimalPoint))));
            //            //}
            //            //else
            //            {
            //                oClientWisePositionModel.GrossPurchaseString = string.Format("{0:0.00}", (oClientWisePositionModel.GrossPurchase / (Math.Pow(10, 2))));
            //                oClientWisePositionModel.GrossSellString = string.Format("{0:0.00}", (oClientWisePositionModel.GrossSell / (Math.Pow(10, 2))));
            //            }
            //            oClientWisePositionModel.NetPL = item.clientData.Cast<NetPosition>().Sum(x => x.NetPL);
            //            oClientWisePositionModel.NetPLIn2Long = Convert.ToInt64(oClientWisePositionModel.NetPL / (Math.Pow(10, 2)));
            //            oClientWisePositionModel.NetValue = Convert.ToInt64(oClientWisePositionModel.NetValue / (Math.Pow(10, 2)));
            //            //oClientWisePositionModel.NetValue = item.clientData.Cast<NetPosition>().Sum(x => x.NetValue);
            //            //oClientWisePositionModel.NetValueIn2Long = Convert.ToInt64(oClientWisePositionModel.NetValue / (Math.Pow(10, 2)));
            //            oClientWisePositionModel.RealPL = item.clientData.Cast<NetPosition>().Sum(x => x.RealPL);
            //            oClientWisePositionModel.RealPLIn2Long = Convert.ToInt64(oClientWisePositionModel.RealPL / (Math.Pow(10, 2)));
            //            oClientWisePositionModel.UnRealPL = item.clientData.Cast<NetPosition>().Sum(x => x.UnRealPL);
            //            oClientWisePositionModel.UnRealPLIn2Long = Convert.ToInt64(oClientWisePositionModel.UnRealPL / (Math.Pow(10, 2)));
            //            //MemoryManager.NetRealPL = MemoryManager.NetRealPL + oClientWisePositionModel.RealPL;
            //            //MemoryManager.NetUnRealPL = MemoryManager.NetUnRealPL + oClientWisePositionModel.UnRealPL;
            //            //MemoryManager.NetPL = MemoryManager.NetPL + oClientWisePositionModel.NetPL;

            //            App.Current.Dispatcher.BeginInvoke((Action)delegate ()
            //            {
            //                if (NetPositionMemory.NetPositionCWDataCollection != null && NetPositionMemory.NetPositionCWDataCollection.Count > 0)
            //                {
            //                    if (NetPositionMemory.NetPositionCWDataCollection.Any(x => x.TraderId == oNetPosition.TraderId))
            //                    {
            //                        index = NetPositionMemory.NetPositionCWDataCollection.IndexOf(NetPositionMemory.NetPositionCWDataCollection.Where(x => x.TraderId == oNetPosition.TraderId).FirstOrDefault());
            //                        NetPositionMemory.NetPositionCWDataCollection[index] = oClientWisePositionModel;
            //                    }
            //                    else
            //                    {
            //                        NetPositionMemory.NetPositionCWDataCollection.Add(oClientWisePositionModel);
            //                    }
            //                }
            //                else
            //                {
            //                    NetPositionMemory.NetPositionCWDataCollection.Add(oClientWisePositionModel);
            //                }
            //            }, System.Windows.Threading.DispatcherPriority.Send);

            //        }

            //    }
            //    catch (Exception)
            //    {

            //        throw;
            //    }

            //}
            #endregion
        }



        public static void UpdateScripNetPosition(string scripId, List<KeyValuePair<string, object>> Obj)
        {
            int index = 0;
            try
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
                    NetPosition oNetPosition = new NetPosition();
                    oNetPosition = (NetPosition)item.scripData.FirstOrDefault();

                    ScripWisePositionModel oScripWisePositionModel = new ScripWisePositionModel();
                    oScripWisePositionModel.ClientId = oNetPosition.ClientId;

                    oScripWisePositionModel.ScripCode = oNetPosition.ScripCode;
                    oScripWisePositionModel.ScripName = oNetPosition.ScripName;
                    oScripWisePositionModel.ScripID = oNetPosition.ScripId;
                    oScripWisePositionModel.ISINNum = oNetPosition.ISINNum;

                    oScripWisePositionModel.TraderId = oNetPosition.TraderId;
                    oScripWisePositionModel.BuyQty = item.scripData.Cast<NetPosition>().Sum(x => x.BuyQty);
                    oScripWisePositionModel.SellQty = item.scripData.Cast<NetPosition>().Sum(x => x.SellQty);
                    var segment = CommonFunctions.GetSegmentID(oNetPosition.ScripCode);
                    var multiplier = CommonFunctions.GetQuantityMultiplier(oScripWisePositionModel.ScripCode, "BSE", segment);
                    var totalBuyVal = item.scripData.Cast<NetPosition>().Sum(x => x.BuyValue);
                    var totalBuyQty = oScripWisePositionModel.BuyQty * multiplier;


                    var decimalPoint = CommonFunctions.GetDecimal(Convert.ToInt32(oNetPosition.ScripCode), "BSE", segment);
                    oScripWisePositionModel.AvgBuyRate = Convert.ToDouble(CommonFrontEnd.Common.CommonFunctions.DisplayInDecimalFormat(totalBuyVal, totalBuyQty, decimalPoint));//item.scripData.Cast<NetPosition>().Sum(x => x.AvgBuyRate);
                                                                                                                                                                                //oScripWisePositionModel.AvgBuyRateIn2Decimal = oScripWisePositionModel.AvgBuyRate / (Math.Pow(10, 2));
                                                                                                                                                                                //oScripWisePositionModel.AvgBuyRateIn4Decimal = oScripWisePositionModel.AvgBuyRate / (Math.Pow(10, 4));


                    //oScripWisePositionModel.AvgBuyRateString
                    //oScripWisePositionModel.AvgBuyRate = totalBuyVal / totalBuyQty;

                    //var AvgBuyRate = oScripWisePositionModel.AvgBuyRate / (Math.Pow(10, 4));
                    //oScripWisePositionModel.AvgBuyRate = AvgBuyRate;
                    //oScripWisePositionModel.AvgBuyRateString = string.Format("{0}.{1}", AvgBuyRate, "0000");

                    var totalSellVal = item.scripData.Cast<NetPosition>().Sum(x => x.SellValue);
                    var totalSellQty = oScripWisePositionModel.SellQty * multiplier;

                    oScripWisePositionModel.AvgSellRate = Convert.ToDouble(CommonFrontEnd.Common.CommonFunctions.DisplayInDecimalFormat(totalSellVal, totalSellQty, decimalPoint));//item.scripData.Cast<NetPosition>().Sum(x => x.SellValue) / oScripWisePositionModel.SellQty;//item.scripData.Cast<NetPosition>().Sum(x => x.AvgSellRate);
                    if (decimalPoint == 4)
                    {
                        oScripWisePositionModel.AvgBuyRateString = string.Format("{0:0.0000}", (oScripWisePositionModel.AvgBuyRate / (Math.Pow(10, UtilityApplicationDetails.GetInstance.MaxDecimalPoint))));
                        oScripWisePositionModel.AvgSellRateString = string.Format("{0:0.0000}", (oScripWisePositionModel.AvgSellRate / (Math.Pow(10, UtilityApplicationDetails.GetInstance.MaxDecimalPoint))));
                    }
                    else
                    {
                        oScripWisePositionModel.AvgBuyRateString = string.Format("{0:0.00}", (oScripWisePositionModel.AvgBuyRate / (Math.Pow(10, UtilityApplicationDetails.GetInstance.MaxDecimalPoint))));
                        oScripWisePositionModel.AvgSellRateString = string.Format("{0:0.00}", (oScripWisePositionModel.AvgSellRate / (Math.Pow(10, UtilityApplicationDetails.GetInstance.MaxDecimalPoint))));
                    }
                    //default 4 decimal fields. added by Gaurav 14/5/2018
                    oScripWisePositionModel.AvgBuyRateString4DecimalCheck = string.Format("{0:0.0000}", (oScripWisePositionModel.AvgBuyRate / (Math.Pow(10, UtilityApplicationDetails.GetInstance.MaxDecimalPoint))));
                    oScripWisePositionModel.AvgSellRateString4DecimalCheck = string.Format("{0:0.0000}", (oScripWisePositionModel.AvgSellRate / (Math.Pow(10, UtilityApplicationDetails.GetInstance.MaxDecimalPoint))));

                    //oScripWisePositionModel.AvgSellRateIn2Decimal = oScripWisePositionModel.AvgSellRate / (Math.Pow(10, 2));
                    //oScripWisePositionModel.AvgSellRateIn4Decimal = oScripWisePositionModel.AvgSellRate / (Math.Pow(10, 4));

                    //if (segment == Enumerations.Segment.Equity.ToString())
                    //{
                    //    oScripWisePositionModel.AvgBuyRateString = string.Format("{0:0.00}", (oScripWisePositionModel.AvgBuyRate / (Math.Pow(10, 2))));
                    //    oScripWisePositionModel.AvgSellRateString = string.Format("{0:0.00}", (oScripWisePositionModel.AvgSellRate / (Math.Pow(10, 2))));
                    //}
                    //else if (segment == Enumerations.Segment.Derivative.ToString())
                    //{
                    //    oScripWisePositionModel.AvgBuyRateString = string.Format("{0:0.00}", (oScripWisePositionModel.AvgBuyRate / (Math.Pow(10, 2))));
                    //    oScripWisePositionModel.AvgSellRateString = string.Format("{0:0.00}", (oScripWisePositionModel.AvgSellRate / (Math.Pow(10, 2))));
                    //}
                    //else if (segment == Enumerations.Segment.Currency.ToString())
                    //{
                    //    oScripWisePositionModel.AvgBuyRateString = string.Format("{0:0.0000}", (oScripWisePositionModel.AvgBuyRate / (Math.Pow(10, 4))));
                    //    oScripWisePositionModel.AvgSellRateString = string.Format("{0:0.0000}", oScripWisePositionModel.AvgSellRate / (Math.Pow(10, 4))));
                    //}
                    //MemoryManager.TotalGrossBuyVal = MemoryManager.TotalGrossBuyVal + (oScripWisePositionModel.AvgBuyRate * oScripWisePositionModel.BuyQty);
                    //MemoryManager.TotalGrossSellVal = item.scripData.Cast<NetPosition>().Sum(x => x.GrossSell);//MemoryManager.TotalGrossSellVal + (oScripWisePositionModel.AvgSellRate * oScripWisePositionModel.SellQty);
                    //MemoryManager.TotalNetVal = MemoryManager.TotalGrossBuyVal - MemoryManager.TotalGrossSellVal;
                    //MemoryManager.TotalGrossVal = MemoryManager.TotalGrossBuyVal + MemoryManager.TotalGrossSellVal;

                    //var AvgSellRate = item.scripData.Cast<NetPosition>().Sum(x => x.AvgSellRate);
                    // var SellQty = item.scripData.Cast<NetPosition>().Sum(x => x.SellQty);
                    //MemoryManager.TotalGrossSellVal = (AvgSellRate * SellQty) + MemoryManager.TotalGrossSellVal;

                    oScripWisePositionModel.NetQty = item.scripData.Cast<NetPosition>().Sum(x => x.NetQty);
                    oScripWisePositionModel.NetValue = Convert.ToInt64((totalBuyVal - totalSellVal) / Math.Pow(10, 4));
                    // oScripWisePositionModel.NetValue = item.scripData.Cast<NetPosition>().Sum(x => x.NetValue);
                    //oScripWisePositionModel.NetValueIn2Long = oScripWisePositionModel.NetValue;//Convert.ToInt64(oScripWisePositionModel.NetValue / (Math.Pow(10, 2)));
                    //oScripWisePositionModel.NetValueIn2Long = oScripWisePositionModel.NetValue;//Convert.ToInt64(oScripWisePositionModel.NetValue / (Math.Pow(10, 2)));

                    if ((oScripWisePositionModel.NetQty) != 0)
                    {
                        //oScripWisePositionModel.BEP = Convert.ToInt32((oScripWisePositionModel.NetValue) / (oScripWisePositionModel.NetQty));
                        //oScripWisePositionModel.BEPIn2Long = Convert.ToInt32((oScripWisePositionModel.NetValue*10000) / (oScripWisePositionModel.NetQty* multiplier));
                        oScripWisePositionModel.BEP = Convert.ToInt64((oScripWisePositionModel.NetValue * 10000) / (oScripWisePositionModel.NetQty * multiplier));
                        //oScripWisePositionModel.BEPIn2Long = Convert.ToInt32(oScripWisePositionModel.BEP / (Math.Pow(10, 2)));//Convert.ToInt32((oScripWisePositionModel.NetValue) / (oScripWisePositionModel.NetQty));
                        if (decimalPoint == 4)
                            //oScripWisePositionModel.BEPString = string.Format("{0:0.0000}", (oScripWisePositionModel.BEPIn2Long / (Math.Pow(10, 4))));
                            oScripWisePositionModel.BEPString = string.Format("{0:0.0000}", (oScripWisePositionModel.BEP / (Math.Pow(10, 4))));
                        else
                            //oScripWisePositionModel.BEPString = string.Format("{0:0.00}", (oScripWisePositionModel.BEPIn2Long / (Math.Pow(10, 4))));
                            oScripWisePositionModel.BEPString = string.Format("{0:0.00}", (oScripWisePositionModel.BEP / (Math.Pow(10, 4))));
                    }
                    App.Current.Dispatcher.BeginInvoke((Action)delegate ()
                         {
                             if (NetPositionMemory.NetPositionSWDataCollection != null && NetPositionMemory.NetPositionSWDataCollection.Count > 0)
                             {
                                 if (NetPositionMemory.NetPositionSWDataCollection.Any(x => x.ScripCode == Convert.ToInt32(oNetPosition.ScripCode)))
                                 {
                                     index = NetPositionMemory.NetPositionSWDataCollection.IndexOf(NetPositionMemory.NetPositionSWDataCollection.Where(x => x.ScripCode == Convert.ToInt64(oNetPosition.ScripCode)).FirstOrDefault());
                                     NetPositionMemory.NetPositionSWDataCollection[index] = oScripWisePositionModel;
                                 }
                                 else
                                 {
                                     NetPositionMemory.NetPositionSWDataCollection.Add(oScripWisePositionModel);
                                 }
                             }
                             else
                             {
                                 NetPositionMemory.NetPositionSWDataCollection.Add(oScripWisePositionModel);
                             }
                         });
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void UpdateScripNetPositionDetail(string scripId, List<KeyValuePair<string, object>> Obj)
        {
            int index = 0;
            if (UtilityLoginDetails.GETInstance.Role.ToLower() == "trader")
            {
                try
                {
                    var results = Obj.AsParallel().GroupBy(p => ((NetPosition)p.Value).ClientId,
                                     p => p.Value,
                                     (key, g) => new
                                     {
                                         ClientId = key,
                                         scripData = g.ToList()
                                     }
                                    );
                    foreach (var item in results)
                    {
                        NetPosition oNetPosition = new NetPosition();
                        oNetPosition = (NetPosition)item.scripData.FirstOrDefault();


                        ScripWiseDetailPositionModel oScripWiseDetailPositionModel = new ScripWiseDetailPositionModel();
                        oScripWiseDetailPositionModel.TraderId = oNetPosition.TraderId;
                        oScripWiseDetailPositionModel.ClientID = oNetPosition.ClientId;

                        oScripWiseDetailPositionModel.ScripCode = oNetPosition.ScripCode;

                        var segment = CommonFunctions.GetSegmentID(oNetPosition.ScripCode);
                        var multiplier = CommonFunctions.GetQuantityMultiplier(oScripWiseDetailPositionModel.ScripCode, "BSE", segment);
                        var decimalPoint = CommonFunctions.GetDecimal(Convert.ToInt32(oNetPosition.ScripCode), "BSE", segment);

                        oScripWiseDetailPositionModel.BuyQty = item.scripData.Cast<NetPosition>().Sum(x => x.BuyQty);
                        oScripWiseDetailPositionModel.SellQty = item.scripData.Cast<NetPosition>().Sum(x => x.SellQty);
                        oScripWiseDetailPositionModel.AvgBuyRate = item.scripData.Cast<NetPosition>().Sum(x => x.AvgBuyRate);

                        oScripWiseDetailPositionModel.AvgSellRate = item.scripData.Cast<NetPosition>().Sum(x => x.AvgSellRate);

                        var totalBuyVal = item.scripData.Cast<NetPosition>().Sum(x => x.BuyValue);
                        var totalSellVal = item.scripData.Cast<NetPosition>().Sum(x => x.SellValue);
                        if (decimalPoint == 4)
                        {
                            oScripWiseDetailPositionModel.AvgBuyRateString = string.Format("{0:0.0000}", (oScripWiseDetailPositionModel.AvgBuyRate / (Math.Pow(10, UtilityApplicationDetails.GetInstance.MaxDecimalPoint))));
                            oScripWiseDetailPositionModel.AvgSellRateString = string.Format("{0:0.0000}", (oScripWiseDetailPositionModel.AvgSellRate / (Math.Pow(10, UtilityApplicationDetails.GetInstance.MaxDecimalPoint))));
                        }
                        else
                        {
                            oScripWiseDetailPositionModel.AvgBuyRateString = string.Format("{0:0.00}", (oScripWiseDetailPositionModel.AvgBuyRate / (Math.Pow(10, UtilityApplicationDetails.GetInstance.MaxDecimalPoint))));
                            oScripWiseDetailPositionModel.AvgSellRateString = string.Format("{0:0.00}", (oScripWiseDetailPositionModel.AvgSellRate / (Math.Pow(10, UtilityApplicationDetails.GetInstance.MaxDecimalPoint))));
                        }
                        //default 4 decimal check. added by Gaurav 14/5/2018
                        oScripWiseDetailPositionModel.AvgBuyRateString4DecimalCheck = string.Format("{0:0.0000}", (oScripWiseDetailPositionModel.AvgBuyRate / (Math.Pow(10, UtilityApplicationDetails.GetInstance.MaxDecimalPoint))));
                        oScripWiseDetailPositionModel.AvgSellRateString4DecimalCheck = string.Format("{0:0.0000}", (oScripWiseDetailPositionModel.AvgSellRate / (Math.Pow(10, UtilityApplicationDetails.GetInstance.MaxDecimalPoint))));

                        oScripWiseDetailPositionModel.NetQty = item.scripData.Cast<NetPosition>().Sum(x => x.NetQty);
                        oScripWiseDetailPositionModel.NetValue = Convert.ToInt64((totalBuyVal - totalSellVal) / Math.Pow(10, UtilityApplicationDetails.GetInstance.MaxDecimalPoint));
                        oScripWiseDetailPositionModel.ClientType = oNetPosition.ClientType;

                        var NetPL = item.scripData.Cast<NetPosition>().Sum(x => x.NetPL);
                        oScripWiseDetailPositionModel.NetPL = Convert.ToInt64(NetPL / (Math.Pow(10, UtilityApplicationDetails.GetInstance.MaxDecimalPoint)));//item.scripData.Cast<NetPosition>().Sum(x => x.NetPL);
                        var RealPL = item.scripData.Cast<NetPosition>().Sum(x => x.RealPL);
                        oScripWiseDetailPositionModel.RealPL = Convert.ToInt64(RealPL / (Math.Pow(10, UtilityApplicationDetails.GetInstance.MaxDecimalPoint))); //item.scripData.Cast<NetPosition>().Sum(x => x.RealPL);

                        var UnRealPL = item.scripData.Cast<NetPosition>().Sum(x => x.UnRealPL);
                        oScripWiseDetailPositionModel.UnRealPL = Convert.ToInt64(UnRealPL / (Math.Pow(10, UtilityApplicationDetails.GetInstance.MaxDecimalPoint))); //item.scripData.Cast<NetPosition>().Sum(x => x.UnRealPL);

                        if ((oScripWiseDetailPositionModel.NetQty) != 0)
                        {
                            oScripWiseDetailPositionModel.BEP = Convert.ToInt64((oScripWiseDetailPositionModel.NetValue * 10000) / (oScripWiseDetailPositionModel.NetQty * multiplier));

                            if (decimalPoint == 4)
                            {
                                oScripWiseDetailPositionModel.BEPString = string.Format("{0:0.0000}", (oScripWiseDetailPositionModel.BEP / (Math.Pow(10, UtilityApplicationDetails.GetInstance.MaxDecimalPoint))));
                            }
                            else
                            {
                                oScripWiseDetailPositionModel.BEPString = string.Format("{0:0.00}", (oScripWiseDetailPositionModel.BEP / (Math.Pow(10, UtilityApplicationDetails.GetInstance.MaxDecimalPoint))));
                            }
                        }
                        App.Current.Dispatcher.BeginInvoke((Action)delegate ()
                             {
                                 if (NetPositionMemory.NetPositionSWCWDataCollection != null && NetPositionMemory.NetPositionSWCWDataCollection.Count > 0)
                                 {
                                     if (NetPositionMemory.NetPositionSWCWDataCollection.Any(x => (x.ClientID == oNetPosition.ClientId && x.ScripCode == Convert.ToInt64(scripId))))
                                     {
                                         index = NetPositionMemory.NetPositionSWCWDataCollection.IndexOf(NetPositionMemory.NetPositionSWCWDataCollection.Where(x => (x.ClientID == oNetPosition.ClientId && x.ScripCode == Convert.ToInt64(scripId))).FirstOrDefault());
                                         if (index != -1)
                                         {
                                             NetPositionMemory.NetPositionSWCWDataCollection[index] = oScripWiseDetailPositionModel;
                                         }
                                     }
                                     else
                                     {
                                         NetPositionMemory.NetPositionSWCWDataCollection.Add(oScripWiseDetailPositionModel);
                                     }
                                 }
                                 else
                                 {
                                     NetPositionMemory.NetPositionSWCWDataCollection.Add(oScripWiseDetailPositionModel);
                                 }
                                 if (UMSProcessor.OnTradeSWCWOnlineReceived != null)
                                     UMSProcessor.OnTradeSWCWOnlineReceived(oScripWiseDetailPositionModel.TraderId);
                             });
                    }
                }
                catch (Exception ex)
                {
                    ExceptionUtility.LogError(ex);
                }
                finally
                {


                }
            }
            else if (UtilityLoginDetails.GETInstance.Role.ToLower() == "admin")
            {
                try
                {
                    var results = Obj.AsParallel().GroupBy(p => ((NetPosition)p.Value).TraderId,
                                     p => p.Value,
                                     (key, g) => new
                                     {
                                         ClientId = key,
                                         scripData = g.ToList()
                                     }
                                    );
                    foreach (var item in results)
                    {
                        NetPosition oNetPosition = new NetPosition();
                        oNetPosition = (NetPosition)item.scripData.FirstOrDefault();


                        ScripWiseDetailPositionModel oScripWiseDetailPositionModel = new ScripWiseDetailPositionModel();
                        oScripWiseDetailPositionModel.TraderId = String.Format("Trd Id-{0}", oNetPosition.TraderId);
                        oScripWiseDetailPositionModel.ClientID = oNetPosition.ClientId;
                        oScripWiseDetailPositionModel.ScripCode = oNetPosition.ScripCode;

                        var segment = CommonFunctions.GetSegmentID(oNetPosition.ScripCode);
                        var multiplier = CommonFunctions.GetQuantityMultiplier(oScripWiseDetailPositionModel.ScripCode, "BSE", segment);
                        var decimalPoint = CommonFunctions.GetDecimal(Convert.ToInt32(oNetPosition.ScripCode), "BSE", segment);

                        oScripWiseDetailPositionModel.BuyQty = item.scripData.Cast<NetPosition>().Sum(x => x.BuyQty);
                        oScripWiseDetailPositionModel.SellQty = item.scripData.Cast<NetPosition>().Sum(x => x.SellQty);
                        oScripWiseDetailPositionModel.AvgBuyRate = item.scripData.Cast<NetPosition>().Sum(x => x.AvgBuyRate);

                        oScripWiseDetailPositionModel.AvgSellRate = item.scripData.Cast<NetPosition>().Sum(x => x.AvgSellRate);

                        var totalBuyVal = item.scripData.Cast<NetPosition>().Sum(x => x.BuyValue);
                        var totalSellVal = item.scripData.Cast<NetPosition>().Sum(x => x.SellValue);
                        if (decimalPoint == 4)
                        {
                            oScripWiseDetailPositionModel.AvgBuyRateString = string.Format("{0:0.0000}", (oScripWiseDetailPositionModel.AvgBuyRate / (Math.Pow(10, UtilityApplicationDetails.GetInstance.MaxDecimalPoint))));
                            oScripWiseDetailPositionModel.AvgSellRateString = string.Format("{0:0.0000}", (oScripWiseDetailPositionModel.AvgSellRate / (Math.Pow(10, UtilityApplicationDetails.GetInstance.MaxDecimalPoint))));
                        }
                        else
                        {
                            oScripWiseDetailPositionModel.AvgBuyRateString = string.Format("{0:0.00}", (oScripWiseDetailPositionModel.AvgBuyRate / (Math.Pow(10, UtilityApplicationDetails.GetInstance.MaxDecimalPoint))));
                            oScripWiseDetailPositionModel.AvgSellRateString = string.Format("{0:0.00}", (oScripWiseDetailPositionModel.AvgSellRate / (Math.Pow(10, UtilityApplicationDetails.GetInstance.MaxDecimalPoint))));
                        }
                        //default 4 decimal check. added by Gaurav 14/5/2018
                        oScripWiseDetailPositionModel.AvgBuyRateString4DecimalCheck = string.Format("{0:0.0000}", (oScripWiseDetailPositionModel.AvgBuyRate / (Math.Pow(10, UtilityApplicationDetails.GetInstance.MaxDecimalPoint))));
                        oScripWiseDetailPositionModel.AvgSellRateString4DecimalCheck = string.Format("{0:0.0000}", (oScripWiseDetailPositionModel.AvgSellRate / (Math.Pow(10, UtilityApplicationDetails.GetInstance.MaxDecimalPoint))));

                        oScripWiseDetailPositionModel.NetQty = item.scripData.Cast<NetPosition>().Sum(x => x.NetQty);
                        oScripWiseDetailPositionModel.NetValue = Convert.ToInt64((totalBuyVal - totalSellVal) / Math.Pow(10, UtilityApplicationDetails.GetInstance.MaxDecimalPoint));
                        oScripWiseDetailPositionModel.ClientType = oNetPosition.ClientType;

                        var NetPL = item.scripData.Cast<NetPosition>().Sum(x => x.NetPL);
                        oScripWiseDetailPositionModel.NetPL = Convert.ToInt64(NetPL / (Math.Pow(10, UtilityApplicationDetails.GetInstance.MaxDecimalPoint)));//item.scripData.Cast<NetPosition>().Sum(x => x.NetPL);
                        var RealPL = item.scripData.Cast<NetPosition>().Sum(x => x.RealPL);
                        oScripWiseDetailPositionModel.RealPL = Convert.ToInt64(RealPL / (Math.Pow(10, UtilityApplicationDetails.GetInstance.MaxDecimalPoint))); //item.scripData.Cast<NetPosition>().Sum(x => x.RealPL);

                        var UnRealPL = item.scripData.Cast<NetPosition>().Sum(x => x.UnRealPL);
                        oScripWiseDetailPositionModel.UnRealPL = Convert.ToInt64(UnRealPL / (Math.Pow(10, UtilityApplicationDetails.GetInstance.MaxDecimalPoint))); //item.scripData.Cast<NetPosition>().Sum(x => x.UnRealPL);

                        if ((oScripWiseDetailPositionModel.NetQty) != 0)
                        {
                            oScripWiseDetailPositionModel.BEP = Convert.ToInt64((oScripWiseDetailPositionModel.NetValue * 10000) / (oScripWiseDetailPositionModel.NetQty * multiplier));

                            if (decimalPoint == 4)
                            {
                                oScripWiseDetailPositionModel.BEPString = string.Format("{0:0.0000}", (oScripWiseDetailPositionModel.BEP / (Math.Pow(10, UtilityApplicationDetails.GetInstance.MaxDecimalPoint))));
                            }
                            else
                            {
                                oScripWiseDetailPositionModel.BEPString = string.Format("{0:0.00}", (oScripWiseDetailPositionModel.BEP / (Math.Pow(10, UtilityApplicationDetails.GetInstance.MaxDecimalPoint))));
                            }
                        }
                        App.Current.Dispatcher.BeginInvoke((Action)delegate ()
                        {
                            if (NetPositionMemory.NetPositionSWCWDataCollection != null && NetPositionMemory.NetPositionSWCWDataCollection.Count > 0)
                            {
                                if (NetPositionMemory.NetPositionSWCWDataCollection.Any(x => (x.TraderId == oNetPosition.TraderId && x.ScripCode == Convert.ToInt64(scripId))))
                                {
                                    index = NetPositionMemory.NetPositionSWCWDataCollection.IndexOf(NetPositionMemory.NetPositionSWCWDataCollection.Where(x => (x.TraderId == oNetPosition.TraderId && x.ScripCode == Convert.ToInt64(scripId))).FirstOrDefault());
                                    if (index != -1)
                                    {
                                        NetPositionMemory.NetPositionSWCWDataCollection[index] = oScripWiseDetailPositionModel;
                                    }
                                }
                                else
                                {
                                    NetPositionMemory.NetPositionSWCWDataCollection.Add(oScripWiseDetailPositionModel);
                                }
                            }
                            else
                            {
                                NetPositionMemory.NetPositionSWCWDataCollection.Add(oScripWiseDetailPositionModel);
                            }
                            if (UMSProcessor.OnTradeSWCWOnlineReceived != null)
                                UMSProcessor.OnTradeSWCWOnlineReceived(oScripWiseDetailPositionModel.TraderId);
                        });
                    }
                }
                catch (Exception ex)
                {
                    ExceptionUtility.LogError(ex);
                }
                finally
                {


                }
            }
            #region Admin Commented


            //if (UtilityLoginDetails.GETInstance.Role.ToLower() == "admin")
            //{
            //    try
            //    {
            //        var results = Obj.AsParallel().GroupBy(p => ((NetPosition)p.Value).TraderId,
            //                         p => p.Value,
            //                         (key, g) => new
            //                         {
            //                             ClientId = key,
            //                             scripData = g.ToList()
            //                         }
            //                        );
            //        foreach (var item in results)
            //        {
            //            NetPosition oNetPosition = new NetPosition();
            //            oNetPosition = (NetPosition)item.scripData.FirstOrDefault();

            //            ScripWiseDetailPositionModel oScripWiseDetailPositionModel = new ScripWiseDetailPositionModel();
            //            oScripWiseDetailPositionModel.TraderId = oNetPosition.TraderId;
            //            oScripWiseDetailPositionModel.ClientID = oNetPosition.ClientId;
            //            oScripWiseDetailPositionModel.ScripCode = oNetPosition.ScripCode;
            //            oScripWiseDetailPositionModel.BuyQty = item.scripData.Cast<NetPosition>().Sum(x => x.BuyQty);
            //            oScripWiseDetailPositionModel.SellQty = item.scripData.Cast<NetPosition>().Sum(x => x.SellQty);
            //            oScripWiseDetailPositionModel.AvgBuyRate = item.scripData.Cast<NetPosition>().Sum(x => x.AvgBuyRate);
            //            oScripWiseDetailPositionModel.AvgBuyRateIn2Decimal = oScripWiseDetailPositionModel.AvgBuyRate / (Math.Pow(10, 2));//item.scripData.Cast<NetPosition>().Sum(x => x.AvgBuyRate);
            //            oScripWiseDetailPositionModel.AvgBuyRateIn4Decimal = oScripWiseDetailPositionModel.AvgBuyRate / (Math.Pow(10, 4));//item.scripData.Cast<NetPosition>().Sum(x => x.AvgBuyRate);
            //            oScripWiseDetailPositionModel.AvgSellRate = item.scripData.Cast<NetPosition>().Sum(x => x.AvgSellRate);
            //            oScripWiseDetailPositionModel.AvgSellRateIn2Decimal = oScripWiseDetailPositionModel.AvgSellRate / (Math.Pow(10, 2));//item.scripData.Cast<NetPosition>().Sum(x => x.AvgSellRate);
            //            oScripWiseDetailPositionModel.AvgSellRateIn4Decimal = oScripWiseDetailPositionModel.AvgSellRate / (Math.Pow(10, 4));//item.scripData.Cast<NetPosition>().Sum(x => x.AvgSellRate);
            //            oScripWiseDetailPositionModel.NetQty = item.scripData.Cast<NetPosition>().Sum(x => x.NetQty);
            //            oScripWiseDetailPositionModel.NetValue = item.scripData.Cast<NetPosition>().Sum(x => x.NetValue);
            //            oScripWiseDetailPositionModel.NetValueIn2Long = Convert.ToInt64(oScripWiseDetailPositionModel.NetValue / (Math.Pow(10, 2)));//item.scripData.Cast<NetPosition>().Sum(x => x.NetValue);
            //            oScripWiseDetailPositionModel.ClientType = oNetPosition.ClientType;
            //            oScripWiseDetailPositionModel.NetPL = item.scripData.Cast<NetPosition>().Sum(x => x.NetPL);
            //            oScripWiseDetailPositionModel.NetPLIn2Long = Convert.ToInt64(oScripWiseDetailPositionModel.NetPL / (Math.Pow(10, 2))); //item.scripData.Cast<NetPosition>().Sum(x => x.NetPL);
            //            oScripWiseDetailPositionModel.RealPL = item.scripData.Cast<NetPosition>().Sum(x => x.RealPL);
            //            oScripWiseDetailPositionModel.RealPLIn2Long = Convert.ToInt64(oScripWiseDetailPositionModel.RealPL / (Math.Pow(10, 2)));//item.scripData.Cast<NetPosition>().Sum(x => x.RealPL);
            //            oScripWiseDetailPositionModel.UnRealPL = item.scripData.Cast<NetPosition>().Sum(x => x.UnRealPL);
            //            oScripWiseDetailPositionModel.UnRealPLIn2Long = Convert.ToInt64(oScripWiseDetailPositionModel.UnRealPL / (Math.Pow(10, 2)));//item.scripData.Cast<NetPosition>().Sum(x => x.UnRealPL);
            //            if ((oScripWiseDetailPositionModel.NetQty) != 0)
            //            {
            //                oScripWiseDetailPositionModel.BEP = Convert.ToInt32((oScripWiseDetailPositionModel.NetValue) / (oScripWiseDetailPositionModel.NetQty));
            //                //oScripWiseDetailPositionModel.BEPIn2Long = Convert.ToInt32(oScripWiseDetailPositionModel.BEP / (Math.Pow(10, 2)));
            //            }
            //            App.Current.Dispatcher.BeginInvoke((Action)delegate ()
            //            {
            //                if (NetPositionMemory.NetPositionSWCWDataCollection != null && NetPositionMemory.NetPositionSWCWDataCollection.Count > 0)
            //                {
            //                    if (NetPositionMemory.NetPositionSWCWDataCollection.Any(x => (x.TraderId == oNetPosition.TraderId && x.ScripCode == Convert.ToInt64(scripId))))
            //                    {
            //                        index = NetPositionMemory.NetPositionSWCWDataCollection.IndexOf(NetPositionMemory.NetPositionSWCWDataCollection.Where(x => (x.TraderId == oNetPosition.TraderId && x.ScripCode == Convert.ToInt64(scripId))).FirstOrDefault());
            //                        if (index != -1)
            //                        {
            //                            NetPositionMemory.NetPositionSWCWDataCollection[index] = oScripWiseDetailPositionModel;
            //                        }
            //                    }
            //                    else
            //                    {
            //                        NetPositionMemory.NetPositionSWCWDataCollection.Add(oScripWiseDetailPositionModel);
            //                    }
            //                }
            //                else
            //                {
            //                    NetPositionMemory.NetPositionSWCWDataCollection.Add(oScripWiseDetailPositionModel);
            //                }
            //                if (UMSProcessor.OnTradeSWCWOnlineReceived != null)
            //                    UMSProcessor.OnTradeSWCWOnlineReceived(oScripWiseDetailPositionModel.TraderId);
            //            });
            //        }
            //    }
            //    catch (Exception)
            //    {

            //        throw;
            //    }
            //    finally
            //    {


            //    }
            //}
            #endregion
        }

        public static void UpdateCWSWDNetPosition(string clientId, List<KeyValuePair<string, object>> Obj)
        {

            int index = 0;
            if (UtilityLoginDetails.GETInstance.Role.ToLower() == "trader")
            {
                try
                {
                    var results = Obj.AsParallel().GroupBy(p => ((NetPosition)p.Value).ScripName,
                                         p => p.Value,
                                         (key, g) => new
                                         {
                                             ScripName = key,
                                             ScripData = g.ToList()
                                         }
                                        );
                    foreach (var item in results)
                    {
                        NetPosition oNetPosition = new NetPosition();
                        oNetPosition = (NetPosition)item.ScripData.FirstOrDefault();

                        CWSWDetailPositionModel oCWSWDetailPositionModel = new CWSWDetailPositionModel();
                        oCWSWDetailPositionModel.TraderId = oNetPosition.TraderId;
                        oCWSWDetailPositionModel.ClientID = oNetPosition.ClientId;
                        oCWSWDetailPositionModel.ScripCode = oNetPosition.ScripCode;
                        oCWSWDetailPositionModel.ClientType = oNetPosition.ClientType;

                        var segment = CommonFunctions.GetSegmentID(oCWSWDetailPositionModel.ScripCode);
                        var multiplier = CommonFunctions.GetQuantityMultiplier(oCWSWDetailPositionModel.ScripCode, "BSE", segment);
                        var decimalPoint = CommonFunctions.GetDecimal(Convert.ToInt32(oCWSWDetailPositionModel.ScripCode), "BSE", segment);

                        oCWSWDetailPositionModel.ScripID = oNetPosition.ScripId;
                        oCWSWDetailPositionModel.ScripName = oNetPosition.ScripName;
                        oCWSWDetailPositionModel.ISINNum = oNetPosition.ISINNum;
                        oCWSWDetailPositionModel.BuyQty = item.ScripData.Cast<NetPosition>().Sum(x => x.BuyQty);
                        oCWSWDetailPositionModel.SellQty = item.ScripData.Cast<NetPosition>().Sum(x => x.SellQty);
                        oCWSWDetailPositionModel.AvgBuyRate = item.ScripData.Cast<NetPosition>().Sum(x => x.AvgBuyRate);

                        oCWSWDetailPositionModel.AvgSellRate = item.ScripData.Cast<NetPosition>().Sum(x => x.AvgSellRate);

                        if (decimalPoint == 4)
                        {
                            oCWSWDetailPositionModel.AvgBuyRateString = string.Format("{0:0.0000}", (oCWSWDetailPositionModel.AvgBuyRate / (Math.Pow(10, UtilityApplicationDetails.GetInstance.MaxDecimalPoint))));
                            oCWSWDetailPositionModel.AvgSellRateString = string.Format("{0:0.0000}", (oCWSWDetailPositionModel.AvgSellRate / (Math.Pow(10, UtilityApplicationDetails.GetInstance.MaxDecimalPoint))));
                        }
                        else
                        {
                            oCWSWDetailPositionModel.AvgBuyRateString = string.Format("{0:0.00}", (oCWSWDetailPositionModel.AvgBuyRate / (Math.Pow(10, UtilityApplicationDetails.GetInstance.MaxDecimalPoint))));
                            oCWSWDetailPositionModel.AvgSellRateString = string.Format("{0:0.00}", (oCWSWDetailPositionModel.AvgSellRate / (Math.Pow(10, UtilityApplicationDetails.GetInstance.MaxDecimalPoint))));
                        }
                        //default 4 decimal check. added by Gaurav 14/5/2018
                        oCWSWDetailPositionModel.AvgBuyRateString4DecimalCheck = string.Format("{0:0.0000}", (oCWSWDetailPositionModel.AvgBuyRate / (Math.Pow(10, UtilityApplicationDetails.GetInstance.MaxDecimalPoint))));
                        oCWSWDetailPositionModel.AvgSellRateString4DecimalCheck = string.Format("{0:0.0000}", (oCWSWDetailPositionModel.AvgSellRate / (Math.Pow(10, UtilityApplicationDetails.GetInstance.MaxDecimalPoint))));

                        oCWSWDetailPositionModel.NetQty = item.ScripData.Cast<NetPosition>().Sum(x => x.NetQty);

                        var NetValue = item.ScripData.Cast<NetPosition>().Sum(x => x.NetValue);
                        oCWSWDetailPositionModel.NetValue = Convert.ToInt64(NetValue / (Math.Pow(10, UtilityApplicationDetails.GetInstance.MaxDecimalPoint)));//item.ScripData.Cast<NetPosition>().Sum(x => x.NetValue);


                        var NetPL = item.ScripData.Cast<NetPosition>().Sum(x => x.NetPL);
                        oCWSWDetailPositionModel.NetPL = Convert.ToInt64(NetPL / (Math.Pow(10, UtilityApplicationDetails.GetInstance.MaxDecimalPoint)));//item.ScripData.Cast<NetPosition>().Sum(x => x.NetPL);


                        var RealPL = item.ScripData.Cast<NetPosition>().Sum(x => x.RealPL);
                        oCWSWDetailPositionModel.RealPL = Convert.ToInt64(RealPL / (Math.Pow(10, UtilityApplicationDetails.GetInstance.MaxDecimalPoint)));//item.ScripData.Cast<NetPosition>().Sum(x => x.RealPL);



                        var UnRealPL = item.ScripData.Cast<NetPosition>().Sum(x => x.UnRealPL);
                        oCWSWDetailPositionModel.UnRealPL = Convert.ToInt64(UnRealPL / (Math.Pow(10, UtilityApplicationDetails.GetInstance.MaxDecimalPoint)));//item.ScripData.Cast<NetPosition>().Sum(x => x.UnRealPL);


                        MemoryManager.TotalNetRealPL = item.ScripData.Cast<NetPosition>().Sum(x => x.RealPL);
                        MemoryManager.TotalNetUnRealPL = item.ScripData.Cast<NetPosition>().Sum(x => x.UnRealPL);
                        MemoryManager.TotalNetPL = MemoryManager.TotalNetRealPL + MemoryManager.TotalNetUnRealPL;

                        if (UMSProcessor.OnTradeCWSWReceived != null)
                            UMSProcessor.OnTradeCWSWReceived();

                        if ((oCWSWDetailPositionModel.NetQty) != 0)
                        {
                            oCWSWDetailPositionModel.BEP = Convert.ToInt64((oCWSWDetailPositionModel.NetValue) / (oCWSWDetailPositionModel.NetQty * multiplier));
                        }

                        App.Current.Dispatcher.BeginInvoke((Action)delegate ()
                             {
                                 if (NetPositionMemory.NetPositionCWSWDataCollection != null && NetPositionMemory.NetPositionCWSWDataCollection.Count > 0)
                                 {
                                     if (NetPositionMemory.NetPositionCWSWDataCollection.Any(x => (x.ClientID == oNetPosition.ClientId && x.ScripName == oNetPosition.ScripName)))
                                     {
                                         index = NetPositionMemory.NetPositionCWSWDataCollection.IndexOf(NetPositionMemory.NetPositionCWSWDataCollection.Where(x => (x.ClientID == oNetPosition.ClientId && x.ScripName == oNetPosition.ScripName)).FirstOrDefault());
                                         if (index != -1)
                                         {
                                             NetPositionMemory.NetPositionCWSWDataCollection[index] = oCWSWDetailPositionModel;
                                         }
                                     }
                                     else
                                     {
                                         NetPositionMemory.NetPositionCWSWDataCollection.Add(oCWSWDetailPositionModel);
                                     }
                                 }
                                 else
                                 {
                                     NetPositionMemory.NetPositionCWSWDataCollection.Add(oCWSWDetailPositionModel);
                                 }
                                 if (UMSProcessor.OnTradeCWSWOnlineReceived != null)
                                     UMSProcessor.OnTradeCWSWOnlineReceived(oCWSWDetailPositionModel.ScripCode);
                             });

                    }
                }
                catch (Exception ex)
                {
                    ExceptionUtility.LogError(ex);
                }
                finally
                {

                }
            }
            else if (UtilityLoginDetails.GETInstance.Role.ToLower() == "admin")
            {
                try
                {
                    var results = Obj.AsParallel().GroupBy(p => ((NetPosition)p.Value).TraderId + "_" + ((NetPosition)p.Value).ScripCode,
                                         p => p.Value,
                                         (key, g) => new
                                         {
                                             ScripName = key,
                                             ScripData = g.ToList()
                                         }
                                        );
                    foreach (var item in results)
                    {
                        NetPosition oNetPosition = new NetPosition();
                        oNetPosition = (NetPosition)item.ScripData.FirstOrDefault();

                        CWSWDetailPositionModel oCWSWDetailPositionModel = new CWSWDetailPositionModel();
                        oCWSWDetailPositionModel.TraderId = oNetPosition.TraderId;
                        oCWSWDetailPositionModel.ClientID = oNetPosition.ClientId;
                        oCWSWDetailPositionModel.ScripCode = oNetPosition.ScripCode;
                        oCWSWDetailPositionModel.ClientType = oNetPosition.ClientType;

                        var segment = CommonFunctions.GetSegmentID(oCWSWDetailPositionModel.ScripCode);
                        var multiplier = CommonFunctions.GetQuantityMultiplier(oCWSWDetailPositionModel.ScripCode, "BSE", segment);
                        var decimalPoint = CommonFunctions.GetDecimal(Convert.ToInt32(oCWSWDetailPositionModel.ScripCode), "BSE", segment);

                        oCWSWDetailPositionModel.ScripID = oNetPosition.ScripId;
                        oCWSWDetailPositionModel.ScripName = oNetPosition.ScripName;
                        oCWSWDetailPositionModel.ISINNum = oNetPosition.ISINNum;
                        oCWSWDetailPositionModel.BuyQty = item.ScripData.Cast<NetPosition>().Sum(x => x.BuyQty);
                        oCWSWDetailPositionModel.SellQty = item.ScripData.Cast<NetPosition>().Sum(x => x.SellQty);
                        oCWSWDetailPositionModel.AvgBuyRate = item.ScripData.Cast<NetPosition>().Sum(x => x.AvgBuyRate);

                        oCWSWDetailPositionModel.AvgSellRate = item.ScripData.Cast<NetPosition>().Sum(x => x.AvgSellRate);

                        if (decimalPoint == 4)
                        {
                            oCWSWDetailPositionModel.AvgBuyRateString = string.Format("{0:0.0000}", (oCWSWDetailPositionModel.AvgBuyRate / (Math.Pow(10, UtilityApplicationDetails.GetInstance.MaxDecimalPoint))));
                            oCWSWDetailPositionModel.AvgSellRateString = string.Format("{0:0.0000}", (oCWSWDetailPositionModel.AvgSellRate / (Math.Pow(10, UtilityApplicationDetails.GetInstance.MaxDecimalPoint))));
                        }
                        else
                        {
                            oCWSWDetailPositionModel.AvgBuyRateString = string.Format("{0:0.00}", (oCWSWDetailPositionModel.AvgBuyRate / (Math.Pow(10, UtilityApplicationDetails.GetInstance.MaxDecimalPoint))));
                            oCWSWDetailPositionModel.AvgSellRateString = string.Format("{0:0.00}", (oCWSWDetailPositionModel.AvgSellRate / (Math.Pow(10, UtilityApplicationDetails.GetInstance.MaxDecimalPoint))));
                        }
                        //default 4 decimal check. added by Gaurav 14/5/2018
                        oCWSWDetailPositionModel.AvgBuyRateString4DecimalCheck = string.Format("{0:0.0000}", (oCWSWDetailPositionModel.AvgBuyRate / (Math.Pow(10, UtilityApplicationDetails.GetInstance.MaxDecimalPoint))));
                        oCWSWDetailPositionModel.AvgSellRateString4DecimalCheck = string.Format("{0:0.0000}", (oCWSWDetailPositionModel.AvgSellRate / (Math.Pow(10, UtilityApplicationDetails.GetInstance.MaxDecimalPoint))));

                        oCWSWDetailPositionModel.NetQty = item.ScripData.Cast<NetPosition>().Sum(x => x.NetQty);

                        var NetValue = item.ScripData.Cast<NetPosition>().Sum(x => x.NetValue);
                        oCWSWDetailPositionModel.NetValue = Convert.ToInt64(NetValue / (Math.Pow(10, UtilityApplicationDetails.GetInstance.MaxDecimalPoint)));//item.ScripData.Cast<NetPosition>().Sum(x => x.NetValue);


                        var NetPL = item.ScripData.Cast<NetPosition>().Sum(x => x.NetPL);
                        oCWSWDetailPositionModel.NetPL = Convert.ToInt64(NetPL / (Math.Pow(10, UtilityApplicationDetails.GetInstance.MaxDecimalPoint)));//item.ScripData.Cast<NetPosition>().Sum(x => x.NetPL);


                        var RealPL = item.ScripData.Cast<NetPosition>().Sum(x => x.RealPL);
                        oCWSWDetailPositionModel.RealPL = Convert.ToInt64(RealPL / (Math.Pow(10, UtilityApplicationDetails.GetInstance.MaxDecimalPoint)));//item.ScripData.Cast<NetPosition>().Sum(x => x.RealPL);



                        var UnRealPL = item.ScripData.Cast<NetPosition>().Sum(x => x.UnRealPL);
                        oCWSWDetailPositionModel.UnRealPL = Convert.ToInt64(UnRealPL / (Math.Pow(10, UtilityApplicationDetails.GetInstance.MaxDecimalPoint)));//item.ScripData.Cast<NetPosition>().Sum(x => x.UnRealPL);


                        MemoryManager.TotalNetRealPL = item.ScripData.Cast<NetPosition>().Sum(x => x.RealPL);
                        MemoryManager.TotalNetUnRealPL = item.ScripData.Cast<NetPosition>().Sum(x => x.UnRealPL);
                        MemoryManager.TotalNetPL = MemoryManager.TotalNetRealPL + MemoryManager.TotalNetUnRealPL;

                        if (UMSProcessor.OnTradeCWSWReceived != null)
                            UMSProcessor.OnTradeCWSWReceived();

                        if ((oCWSWDetailPositionModel.NetQty) != 0)
                        {
                            oCWSWDetailPositionModel.BEP = Convert.ToInt64((oCWSWDetailPositionModel.NetValue) / (oCWSWDetailPositionModel.NetQty * multiplier));
                        }

                        App.Current.Dispatcher.BeginInvoke((Action)delegate ()
                        {
                            if (NetPositionMemory.NetPositionCWSWDataCollection != null && NetPositionMemory.NetPositionCWSWDataCollection.Count > 0)
                            {
                                if (NetPositionMemory.NetPositionCWSWDataCollection.Any(x => (x.TraderId == oNetPosition.TraderId && x.ScripName == oNetPosition.ScripName)))
                                {
                                    index = NetPositionMemory.NetPositionCWSWDataCollection.IndexOf(NetPositionMemory.NetPositionCWSWDataCollection.Where(x => (x.TraderId == oNetPosition.TraderId && x.ScripName == oNetPosition.ScripName)).FirstOrDefault());
                                    if (index != -1)
                                    {
                                        NetPositionMemory.NetPositionCWSWDataCollection[index] = oCWSWDetailPositionModel;
                                    }
                                }
                                else
                                {
                                    NetPositionMemory.NetPositionCWSWDataCollection.Add(oCWSWDetailPositionModel);
                                }
                            }
                            else
                            {
                                NetPositionMemory.NetPositionCWSWDataCollection.Add(oCWSWDetailPositionModel);
                            }
                            if (UMSProcessor.OnTradeCWSWOnlineReceived != null)
                                UMSProcessor.OnTradeCWSWOnlineReceived(oCWSWDetailPositionModel.ScripCode);
                        });

                    }
                }
                catch (Exception ex)
                {
                    ExceptionUtility.LogError(ex);
                }
                finally
                {

                }
            }
            #region Admin Commented   
            //else if (UtilityLoginDetails.GETInstance.Role.ToLower() == "admin")
            //{
            //    try
            //    {
            //        var results = Obj.AsParallel().GroupBy(p => (((NetPosition)p.Value).TraderId + "_" +
            //        ((NetPosition)p.Value).ScripCode),
            //                             p => p.Value,
            //                             (key, g) => new
            //                             {
            //                                 ScripName = key,
            //                                 ScripData = g.ToList()
            //                             }
            //                            );
            //        foreach (var item in results)
            //        {
            //            NetPosition oNetPosition = new NetPosition();
            //            oNetPosition = (NetPosition)item.ScripData.FirstOrDefault();

            //            CWSWDetailPositionModel oCWSWDetailPositionModel = new CWSWDetailPositionModel();
            //            oCWSWDetailPositionModel.TraderId = oNetPosition.TraderId;
            //            oCWSWDetailPositionModel.ClientID = oNetPosition.ClientId;
            //            oCWSWDetailPositionModel.ScripCode = oNetPosition.ScripCode;
            //            oCWSWDetailPositionModel.ScripID = oNetPosition.ScripId;
            //            oCWSWDetailPositionModel.ScripName = oNetPosition.ScripName;
            //            oCWSWDetailPositionModel.ISINNum = oNetPosition.ISINNum;
            //            oCWSWDetailPositionModel.BuyQty = item.ScripData.Cast<NetPosition>().Sum(x => x.BuyQty);
            //            oCWSWDetailPositionModel.SellQty = item.ScripData.Cast<NetPosition>().Sum(x => x.SellQty);
            //            oCWSWDetailPositionModel.AvgBuyRate = item.ScripData.Cast<NetPosition>().Sum(x => x.AvgBuyRate);
            //            oCWSWDetailPositionModel.AvgBuyRateIn2Decimal = oCWSWDetailPositionModel.AvgBuyRate / (Math.Pow(10, 2));//item.ScripData.Cast<NetPosition>().Sum(x => x.AvgBuyRate);
            //            oCWSWDetailPositionModel.AvgBuyRateIn4Decimal = oCWSWDetailPositionModel.AvgBuyRate / (Math.Pow(10, 4));//item.ScripData.Cast<NetPosition>().Sum(x => x.AvgBuyRate);
            //            oCWSWDetailPositionModel.AvgSellRate = item.ScripData.Cast<NetPosition>().Sum(x => x.AvgSellRate);
            //            oCWSWDetailPositionModel.AvgSellRateIn2Decimal = oCWSWDetailPositionModel.AvgSellRate / (Math.Pow(10, 2));//item.ScripData.Cast<NetPosition>().Sum(x => x.AvgSellRate);
            //            oCWSWDetailPositionModel.AvgSellRateIn4Decimal = oCWSWDetailPositionModel.AvgSellRate / (Math.Pow(10, 4));//item.ScripData.Cast<NetPosition>().Sum(x => x.AvgSellRate);
            //            oCWSWDetailPositionModel.NetQty = item.ScripData.Cast<NetPosition>().Sum(x => x.NetQty);
            //            oCWSWDetailPositionModel.NetValue = item.ScripData.Cast<NetPosition>().Sum(x => x.NetValue);
            //            oCWSWDetailPositionModel.NetValueIn2long = Convert.ToInt64(oCWSWDetailPositionModel.NetValue / (Math.Pow(10, 2)));
            //            oCWSWDetailPositionModel.ClientType = oNetPosition.ClientType;
            //            oCWSWDetailPositionModel.NetPL = item.ScripData.Cast<NetPosition>().Sum(x => x.NetPL);
            //            oCWSWDetailPositionModel.NetPLIn2Long = Convert.ToInt64(oCWSWDetailPositionModel.NetPL / (Math.Pow(10, 2)));//item.ScripData.Cast<NetPosition>().Sum(x => x.NetPL);
            //            oCWSWDetailPositionModel.RealPL = item.ScripData.Cast<NetPosition>().Sum(x => x.RealPL);
            //            oCWSWDetailPositionModel.RealPLIn2Long = Convert.ToInt64(oCWSWDetailPositionModel.RealPL / (Math.Pow(10, 2)));//item.ScripData.Cast<NetPosition>().Sum(x => x.RealPL);
            //            oCWSWDetailPositionModel.UnRealPL = item.ScripData.Cast<NetPosition>().Sum(x => x.UnRealPL);
            //            oCWSWDetailPositionModel.UnRealPLIn2Long = Convert.ToInt64(oCWSWDetailPositionModel.UnRealPL / (Math.Pow(10, 2)));//item.ScripData.Cast<NetPosition>().Sum(x => x.UnRealPL);

            //            //MemoryManager.TotalNetRealPL = oCWSWDetailPositionModel.RealPL;
            //            //MemoryManager.TotalNetUnRealPL = oCWSWDetailPositionModel.UnRealPL;
            //            //MemoryManager.TotalNetPL = oCWSWDetailPositionModel.NetPL;

            //            if (UMSProcessor.OnTradeCWSWReceived != null)
            //                UMSProcessor.OnTradeCWSWReceived();

            //            if ((oCWSWDetailPositionModel.NetQty) != 0)
            //            {
            //                oCWSWDetailPositionModel.BEP = Convert.ToInt32((oCWSWDetailPositionModel.NetValue) / (oCWSWDetailPositionModel.NetQty));
            //                //oCWSWDetailPositionModel.BEPIn2Long = Convert.ToInt32(oCWSWDetailPositionModel.BEP / (Math.Pow(10, 2)));
            //            }

            //            App.Current.Dispatcher.BeginInvoke((Action)delegate ()
            //            {
            //                if (NetPositionMemory.NetPositionCWSWDataCollection != null && NetPositionMemory.NetPositionCWSWDataCollection.Count > 0)
            //                {
            //                    if (NetPositionMemory.NetPositionCWSWDataCollection.Any(x => (x.TraderId == oNetPosition.TraderId && x.ScripName == oNetPosition.ScripName)))
            //                    {
            //                        index = NetPositionMemory.NetPositionCWSWDataCollection.IndexOf(NetPositionMemory.NetPositionCWSWDataCollection.Where(x => (x.TraderId == oNetPosition.TraderId && x.ScripName == oNetPosition.ScripName)).FirstOrDefault());
            //                        if (index != -1)
            //                        {
            //                            NetPositionMemory.NetPositionCWSWDataCollection[index] = oCWSWDetailPositionModel;
            //                        }
            //                    }
            //                    else
            //                    {
            //                        NetPositionMemory.NetPositionCWSWDataCollection.Add(oCWSWDetailPositionModel);
            //                    }
            //                }
            //                else
            //                {
            //                    NetPositionMemory.NetPositionCWSWDataCollection.Add(oCWSWDetailPositionModel);
            //                }
            //                if (UMSProcessor.OnTradeCWSWOnlineReceived != null)
            //                    UMSProcessor.OnTradeCWSWOnlineReceived(oCWSWDetailPositionModel.ScripCode);
            //            });
            //        }
            //    }
            //    catch (Exception)
            //    {

            //        throw;
            //    }
            //    finally
            //    {

            //    }
            //}
            #endregion
        }



        public static void UpdateTraderTradeData(string traderId, object obj)
        {

            TradeUMS objumsmodel = obj as TradeUMS;

            App.Current.Dispatcher.BeginInvoke((Action)delegate ()
            {
                NetPositionMemory.TraderTradeDataCollection.Add(objumsmodel);
            });
            //SaudasUMSModel oSaudasUMSModel = new SaudasUMSModel();

            ////oSaudasUMSModel.MemberID = Convert.ToInt16(App.MemberId);

            ////oSaudasUMSModel.BSFlag = Enum.GetName(typeof(Enumerations.SideShort), objumsmodel.Side);

            //oSaudasUMSModel.Segment = objumsmodel.Market;

            //oSaudasUMSModel.OrdType = Enum.GetName(typeof(Enumerations.OrderTypeShort), objumsmodel.OrdType);

            ////  oSaudasUMSModel.Client = objumsmodel.FreeText1.Trim();

            //// oSaudasUMSModel.ClientType = Enum.GetName(typeof(Enumerations.AccountType), objumsmodel.AccountType);

            //oSaudasUMSModel.CPCd = objumsmodel.CPCode;

            //oSaudasUMSModel.DealCode = objumsmodel.TradeID;

            //oSaudasUMSModel.LocationId = objumsmodel.SenderLocationID;

            //oSaudasUMSModel.OrderId = objumsmodel.OrderID;

            //oSaudasUMSModel.Qty = objumsmodel.LastQty;

            //// oSaudasUMSModel.Rate = CommonFunctions.DisplayInDecimalFormat(objumsmodel.LastPx, 100000000d, 2);//paise Convert.ToDecimal(objumsmodel.LastPx / 100000000d);

            //oSaudasUMSModel.Rate1 = objumsmodel.LastPx / 1000000;//rupees

            //oSaudasUMSModel.ScripCode = objumsmodel.SecurityID;

            //// oSaudasUMSModel.ScripName = CommonFrontEnd.Common.CommonFunctions.GetScripName(objumsmodel.SecurityID);

            //// oSaudasUMSModel.ScripGroup = CommonFrontEnd.Common.CommonFunctions.GetGroupName(objumsmodel.SecurityID);//MasterSharedMemory.objMastertxtDic.Where(x => x.Value.ScripCode == (objumsmodel.SecurityID)).Select(x => x.Value.GroupName).FirstOrDefault();

            ////oSaudasUMSModel.SettlNo = CommonFrontEnd.Common.CommonFunctions.GetSettlNo(objumsmodel.SecurityID);
            ////if(MasterSharedMemory.listSetlMas.objSetlmas.Field1 == DR)
            ////var date= 
            //oSaudasUMSModel.SettlNo = MasterSharedMemory.listSetlMas.Where(x => x.Field1 == "DR" && Convert.ToDateTime(x.Field2).ToString("dd/MM/yyyy") == System.DateTime.Now.ToString("dd/MM/yyyy")).Select(x => x.Fy).FirstOrDefault().Split('/');

            //oSaudasUMSModel.SettlNoComb = MasterSharedMemory.listSetlMas.Where(x => x.Field1 == "DR" && Convert.ToDateTime(x.Field2).ToString("dd/MM/yyyy") == System.DateTime.Now.ToString("dd/MM/yyyy")).Select(x => x.Fy).FirstOrDefault();

            ////foreach (var item in MasterSharedMemory.listSetlMas.Where(x=>x.Field1=="DR"))
            ////{
            ////    if (string.Format("{0:dd/MM/yyyy}", item.Field2) == System.DateTime.Now.ToString("dd/MM/yyyy"))
            ////    {
            ////        oSaudasUMSModel.SettlNo = item.Fy;
            ////    }
            ////}


            //oSaudasUMSModel.Status = objumsmodel.FreeText3;

            //// oSaudasUMSModel.ISIN = CommonFrontEnd.Common.CommonFunctions.GetISIN(objumsmodel.SecurityID);//MasterSharedMemory.objMastertxtDic.Where(x => x.Value.ScripCode == (objumsmodel.SecurityID)).Select(x => x.Value.IsinCode).FirstOrDefault();

            //if (App.Role != null && App.Role.ToLower() == "trader")
            //{
            //    oSaudasUMSModel.Time = String.Format("{0:00}:{1:00}:{2:00}", objumsmodel.Hour, objumsmodel.Minute, objumsmodel.Second);//CommonFrontEnd.Common.Converter.UnixTimeStampToDateTime(objumsmodel.TransactTime).ToString();
            //}
            //else
            //{
            //    oSaudasUMSModel.Time = String.Format("{0:0000}-{1:00}-{2:00}:{3:00}:{4:00}:{5:00}", objumsmodel.Year, objumsmodel.Month, objumsmodel.Day, objumsmodel.Hour, objumsmodel.Minute, objumsmodel.Second);//CommonFrontEnd.Common.Converter.UnixTimeStampToDateTime(objumsmodel.TransactTime).ToString();
            //}

            //oSaudasUMSModel.Date = String.Format("{0:0000}-{1:00}-{2:00}", objumsmodel.Year, objumsmodel.Month, objumsmodel.Day);

            //oSaudasUMSModel.TimeOnly = String.Format("{0:00}:{1:00}:{2:00}", objumsmodel.Hour, objumsmodel.Minute, objumsmodel.Second);

            //oSaudasUMSModel.TradeId = objumsmodel.SideTradeID;
            //// oSaudasUMSModel.TraderId = Convert.ToInt32(objumsmodel.RootPartyIDSessionID.ToString().Remove(0, 6));

            //oSaudasUMSModel.OrderTimeStamp = "00:00:00";

            //oSaudasUMSModel.OrderTimeStamp1 = "";

            //oSaudasUMSModel.PendingQty = objumsmodel.LeavesQty;

            //if (objumsmodel.UnderlyingDirtyPrice == long.MinValue || objumsmodel.UnderlyingDirtyPrice == long.MaxValue)
            //{
            //    oSaudasUMSModel.DirtyPrice = 0;
            //}
            //else
            //{
            //    oSaudasUMSModel.DirtyPrice = objumsmodel.UnderlyingDirtyPrice;
            //}
            ////if (objumsmodel.Yield == long.MinValue || objumsmodel.Yield == long.MaxValue)
            ////{
            //oSaudasUMSModel.Yield = 0;
            ////}
            //else
            //{
            //    oSaudasUMSModel.Yield = objumsmodel.Yield;
            //}

            // oSaudasUMSModel.ElaspedTime = ElapsedTime;

            // Title = "Admin trade View " + TradeViewDataCollection.Count.ToString();

            //MemoryManager.NetPositionScripsDict.TryAdd(oSaudasUMSModel.CPCd + "_" + oSaudasUMSModel.ScripCode, ProcessNetPosition(objumsmodel));

            //ProcessNetPosition(objumsmodel);

            //TraderTradeDataCollection.Insert(0, oSaudasUMSModel);

            // if (AD2TRDataUpdation != null)
            //   AD2TRDataUpdation(oSaudasUMSModel); 
        }

        public async Task UpdateClientNetPositionAsync(string ClientId, List<KeyValuePair<string, object>> Obj)
        {
            int index = 0;
            try
            {

                await Task.Run(() =>
                   {
                       var results = Obj.GroupBy(p => ((NetPosition)p.Value).ClientId,
                                       p => p.Value,
                                       (key, g) => new
                                       {
                                           clientID = key,
                                           clientData = g.ToList()
                                       }
                                      );

                       foreach (var item in results)
                       {
                           NetPosition oNetPosition = new NetPosition();
                           oNetPosition = (NetPosition)item.clientData.FirstOrDefault();

                           ClientWisePositionModel oClientWisePositionModel = new ClientWisePositionModel();
                           oClientWisePositionModel.TraderId = oNetPosition.TraderId;
                           oClientWisePositionModel.ClientId = oNetPosition.ClientId;
                           oClientWisePositionModel.ClientType = oNetPosition.ClientType;
                           oClientWisePositionModel.GrossPurchase = item.clientData.Sum(x => ((NetPosition)x).GrossPurchase);
                           oClientWisePositionModel.GrossSell = item.clientData.Sum(x => ((NetPosition)x).GrossSell);
                           oClientWisePositionModel.NetPL = item.clientData.Cast<NetPosition>().Sum(x => x.NetPL);
                           oClientWisePositionModel.NetValue = item.clientData.Cast<NetPosition>().Sum(x => x.NetValue);
                           oClientWisePositionModel.RealPL = item.clientData.Cast<NetPosition>().Sum(x => x.RealPL);
                           oClientWisePositionModel.UnRealPL = item.clientData.Cast<NetPosition>().Sum(x => x.UnRealPL);

                           // App.Current.Dispatcher.BeginInvoke((Action)delegate()
                           //   {
                           if (NetPositionMemory.NetPositionCWDataCollection != null && NetPositionMemory.NetPositionCWDataCollection.Count > 0)
                           {
                               if (NetPositionMemory.NetPositionCWDataCollection.Any(x => x.ClientId == ClientId))
                               {
                                   index = NetPositionMemory.NetPositionCWDataCollection.IndexOf(NetPositionMemory.NetPositionCWDataCollection.Where(x => x.ClientId == ClientId).FirstOrDefault());
                                   NetPositionMemory.NetPositionCWDataCollection[index] = oClientWisePositionModel;
                               }
                               else
                               {
                                   NetPositionMemory.NetPositionCWDataCollection.Add(oClientWisePositionModel);
                               }
                           }
                           else
                           {
                               NetPositionMemory.NetPositionCWDataCollection.Add(oClientWisePositionModel);
                           }
                           //    });

                       }
                   });
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static void UpdateScripNetPositionCopy(string scripId, List<KeyValuePair<string, object>> Obj)
        {
            int index = 0;
            try
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
                    NetPosition oNetPosition = new NetPosition();
                    oNetPosition = (NetPosition)item.scripData.FirstOrDefault();

                    ScripWisePositionModel oScripWisePositionModel = new ScripWisePositionModel();
                    oScripWisePositionModel.ClientId = oNetPosition.ClientId;

                    oScripWisePositionModel.ScripCode = oNetPosition.ScripCode;
                    oScripWisePositionModel.ScripName = oNetPosition.ScripName;
                    oScripWisePositionModel.ScripID = oNetPosition.ScripId;
                    oScripWisePositionModel.ISINNum = oNetPosition.ISINNum;

                    oScripWisePositionModel.TraderId = oNetPosition.TraderId;
                    oScripWisePositionModel.BuyQty = item.scripData.Cast<NetPosition>().Sum(x => x.BuyQty);
                    oScripWisePositionModel.SellQty = item.scripData.Cast<NetPosition>().Sum(x => x.SellQty);

                    var totalBuyVal = item.scripData.Cast<NetPosition>().Sum(x => x.BuyValue);
                    var totalBuyQty = oScripWisePositionModel.BuyQty;

                    oScripWisePositionModel.AvgBuyRate = Convert.ToDouble(CommonFrontEnd.Common.CommonFunctions.DisplayInDecimalFormat(totalBuyVal, totalBuyQty, 2));//item.scripData.Cast<NetPosition>().Sum(x => x.AvgBuyRate);

                    var totalSellVal = item.scripData.Cast<NetPosition>().Sum(x => x.SellValue);
                    var totalSellQty = oScripWisePositionModel.SellQty;

                    oScripWisePositionModel.AvgSellRate = Convert.ToDouble(CommonFrontEnd.Common.CommonFunctions.DisplayInDecimalFormat(totalSellVal, totalSellQty, 2));//item.scripData.Cast<NetPosition>().Sum(x => x.SellValue) / oScripWisePositionModel.SellQty;//item.scripData.Cast<NetPosition>().Sum(x => x.AvgSellRate);

                    oScripWisePositionModel.NetQty = item.scripData.Cast<NetPosition>().Sum(x => x.NetQty);
                    oScripWisePositionModel.NetValue = item.scripData.Cast<NetPosition>().Sum(x => x.NetValue);
                    if ((oScripWisePositionModel.NetQty) != 0)
                    {
                        oScripWisePositionModel.BEP = Convert.ToInt32((oScripWisePositionModel.NetValue) / (oScripWisePositionModel.NetQty));
                        //uncommented later 16/3/2018 
                        //oScripWisePositionModel.BEPIn2Long = Convert.ToInt32(oScripWisePositionModel.BEP / (Math.Pow(10, 2)));//Convert.ToInt32((oScripWisePositionModel.NetValue) / (oScripWisePositionModel.NetQty));
                    }
                    App.Current.Dispatcher.BeginInvoke((Action)delegate ()
                    {
                        if (NetPositionMemory.NetPositionSWDataCollection != null && NetPositionMemory.NetPositionSWDataCollection.Count > 0)
                        {
                            if (NetPositionMemory.NetPositionSWDataCollection.Any(x => x.ScripCode == Convert.ToInt32(scripId)))
                            {
                                index = NetPositionMemory.NetPositionSWDataCollection.IndexOf(NetPositionMemory.NetPositionSWDataCollection.Where(x => x.ScripCode == Convert.ToInt64(scripId)).FirstOrDefault());
                                NetPositionMemory.NetPositionSWDataCollection[index] = oScripWisePositionModel;
                            }
                            else
                            {
                                NetPositionMemory.NetPositionSWDataCollection.Add(oScripWisePositionModel);
                            }
                        }
                        else
                        {
                            NetPositionMemory.NetPositionSWDataCollection.Add(oScripWisePositionModel);
                        }
                    });
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
#endif
}
