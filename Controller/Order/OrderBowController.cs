using CommonFrontEnd.HTTPHlper;
using System;
using System.Linq;
using CommonFrontEnd.Constants;
using CommonFrontEnd.Common;
using CommonFrontEnd.Global;
using CommonFrontEnd.Model.Order;
using CommonFrontEnd.GetDataForStock;
using CommonFrontEnd.SharedMemories;
using System.Windows;
using static CommonFrontEnd.Common.Enumerations.Order;
using CommonFrontEnd.Processor.Order;
//using CommonFrontEnd.Controller.Trade;
using System.Text;
using CommonFrontEnd.Utility;

namespace CommonFrontEnd.Controller.Order
{
    //#if BOW --> Commented as errors in BOW debug
    public class OrderBowController
    {
#if BOW
        #region Properties

        static UtilityLoginDetails objUtilityLoginDetails = UtilityLoginDetails.GETInstance;
        static UtilityConnParameters gobjUtilityConnParameters = UtilityConnParameters.GetInstance;
        //static internal ConcurrentQueue<OrderModel> OrderQueue;

        //private HTTPHlper.HTTPHlper.ResponseReturned mobjMessageArrivedDelegate;
        public static HTTPHlper.HTTPHlpr.ResponseReturned MessageArrivedDelegate = new HTTPHlpr.ResponseReturned(HTTPOrderResponseArrived);
        public static event OrderSentEventHandler OrderSent;
        public delegate void OrderSentEventHandler(string pstrMessage, int pstrBuySellIndicator);
        //OnScrollUpdateVisibleItemsOnly += new ShowVisibleRecordsEventHandler(broadcastReciever_OnScrollUpdateVisibleItemsOnly);
        //HTTPHlper.HTTPHlpr.ResponseReturned mobjMessageArrivedDelegate = new HTTPHlpr.ResponseReturned()

        // mobjMessageArrivedDelegate = New HTTPRequestHelper.HTTPHelper.ResponseReturned(AddressOf OrderResponseArrived)
        #endregion


        #region "Order over TCP"
        //:Fixed header feilds
        private static string mstrMsgCode = "          ";
        private static string mstrTblName = "Orders                        ";
        private static string mstrAddOrdercd = "2000      ";
        private static string mstrEditOrdercd = "2040      ";
        private static string mstrDirection = "1";
        private static string mstrIntToken = "0         ";
        private static string mstrISINNo = "                              ";
        private static string mstrInstName = "          ";
        private static string mstrExpDate = "0000000000";
        private static string mstrStkPrc = "00000000000000000000";
        private static string mstrOptType = "0         ";
        private static string mstrMsgType = " ";
        private static string mstrFiller = "  ";
        private static string mstrToken2 = "0         ";
        private static string mstrMsgOrgTime = "0         ";
        private static string mstrPurpose = "P";
        #endregion

        #region Methods
        public static void SendOrderDetailsBow(OrderModel omodel)
        {
            try
            {
                bool lblnSendOverTcp = false;
                if (omodel != null)
                {
                    //int DecimalPoint = CommonFunctions.GetDecimal(Convert.ToInt32(omodel.ScripCode), Enumerations.Exchange.BSE, (Enumerations.Segment)Enum.Parse(typeof(Enumerations.Segment), omodel.Segment));
                    int DecimalPoint = CommonFunctions.GetDecimal(Convert.ToInt32(omodel.ScripCode), omodel.Exchange, omodel.Segment);

                    if ((omodel.Exchange == Exchanges.BSE.ToString() || omodel.Exchange == Exchanges.NSE.ToString()) && omodel.Segment == ScripSegment.Equity.ToString())
                    {
                        if (omodel.Mode == Convert.ToInt32(Modes.Add))
                        {
                            if (omodel.Exchange == Exchanges.BSE.ToString()
                            && omodel.Segment == ScripSegment.Equity.ToString() && App.SendOrdOverTCP == true)
                            {
                                //if (pblnAutoQuoteOrder == false)
                                //{
                                //    mintOrderEntryState = ORDER_ENTRY_STATE.OrderEntered;
                                //}

                                //TODO: Uncomment below line and resolve error in  sendOrderOverTCP method
                                //lblnSendOverTcp = sendOrderOverTCP(omodel,DecimalPoint);
                            }
                            else
                            {
                                //if (pblnAutoQuoteOrder == false)
                                //{
                                //    mintOrderEntryState = ORDER_ENTRY_STATE.OrderEntered;
                                //}

                                GetDataForStocks.GetDataFromServer(BowServletsConstants.URL_ADD_ORDER, GetNames(omodel), GetValues(omodel, DecimalPoint), false, MessageArrivedDelegate);
                                //if (pblnIsSOROrder)
                                //{
                                //    GetDataFromServer(Constants.OrderConstants.URL_ADD_SOR_ORDER, GetNames, GetValues, false, mobjMessageArrivedDelegate);
                                //}
                                //else
                                //{
                                //    GetDataFromServer(Constants.OrderConstants.URL_ADD_ORDER, GetNames, GetValues, false, mobjMessageArrivedDelegate);
                                //}
                            }
                        }
                        else if (omodel.Mode == Convert.ToInt32(Enumerations.Order.Modes.Edit))
                        {

                            if (omodel.Exchange == Enumerations.Order.Exchanges.BSE.ToString()
                            && omodel.Segment == Enumerations.Order.ScripSegment.Equity.ToString() && App.SendOrdOverTCP == true)
                            {
                                //TODO: Uncomment below line and resolve error in  sendOrderOverTCP method
                                // lblnSendOverTcp = sendOrderOverTCP(omodel, DecimalPoint);
                            }
                            else
                            {
                                GetDataForStocks.GetDataFromServer(BowServletsConstants.URL_EDIT_ORDER, GetNames(omodel), GetValues(omodel, DecimalPoint), false, MessageArrivedDelegate);
                            }
                        }
                    }

                    else
                    {

                        if (omodel.Mode == Convert.ToInt32(Modes.Add))
                        {
                            if (App.SendOrdOverTCP)
                            {
                                if (omodel.Exchange == Exchanges.BSE.ToString() && (omodel.Segment == ScripSegment.Derivative.ToString() || omodel.Segment == ScripSegment.Currency.ToString() || omodel.Segment == ScripSegment.Commodities.ToString()))
                                {
                                    //TODO: TCP connection and Send order Over TCP
                                    //lblnSendOverTcp = SetValuesForOrderDtls()
                                }

                            }
                            else
                            {
                                GetDataForStocks.GetDataFromServer(BowServletsConstants.URL_ADD_ORDER, GetNames_Structure_2(omodel), GetValues_Structure_2(omodel, DecimalPoint), false, MessageArrivedDelegate);
                            }
                        }
                        else if (omodel.Mode == Convert.ToInt32(Enumerations.Order.Modes.Edit))
                        {
                            //TODO: For Derivatives and others
                            if (App.SendOrdOverTCP)
                            {
                                if (omodel.Exchange == Exchanges.BSE.ToString() && (omodel.Segment == ScripSegment.Derivative.ToString() || omodel.Segment == ScripSegment.Currency.ToString() || omodel.Segment == ScripSegment.Commodities.ToString()))
                                {
                                    //TODO: TCP connection and Send order Over TCP
                                    //lblnSendOverTcp = SetValuesForOrderDtls()
                                }

                            }
                            else
                            {
                                GetDataForStocks.GetDataFromServer(BowServletsConstants.URL_EDIT_ORDER, GetNames_Structure_2(omodel), GetValues_Structure_2(omodel, DecimalPoint), false, MessageArrivedDelegate);
                            }
                        }
                    }
                }
                else
                {
                    throw new Exception("Error while sending Bow request");
                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
            }


        }

        private static string[] GetNames(OrderModel omodel)
        {
            string[] lstrNames = new string[Convert.ToInt32(Enumerations.Order.CMBuySellParameters.NO_OF_PARAMETERS) + 3];
            lstrNames[Convert.ToInt32(CMBuySellParameters.Token)] = BowOrderBean.f_Token;
            lstrNames[Convert.ToInt32(CMBuySellParameters.NseToken)] = BowOrderBean.f_NseToken;
            lstrNames[Convert.ToInt32(CMBuySellParameters.BseToken)] = BowOrderBean.f_BseToken;
            lstrNames[Convert.ToInt32(CMBuySellParameters.Destination)] = BowOrderBean.f_Destination;
            lstrNames[Convert.ToInt32(CMBuySellParameters.Market)] = BowOrderBean.f_Market;
            lstrNames[Convert.ToInt32(CMBuySellParameters.Symbol)] = BowOrderBean.f_Symbol;
            lstrNames[Convert.ToInt32(CMBuySellParameters.Series)] = BowOrderBean.f_Series;
            lstrNames[Convert.ToInt32(CMBuySellParameters.Volume)] = BowOrderBean.f_Volume;
            lstrNames[Convert.ToInt32(CMBuySellParameters.Price)] = BowOrderBean.f_Price;
            lstrNames[Convert.ToInt32(CMBuySellParameters.TriggerPrice)] = BowOrderBean.f_TriggerPrice;
            lstrNames[Convert.ToInt32(CMBuySellParameters.DisclosedVolume)] = BowOrderBean.f_DisclosedVolume;
            lstrNames[Convert.ToInt32(CMBuySellParameters.BackOfficeId)] = BowOrderBean.f_BackOfficeId;
            lstrNames[Convert.ToInt32(CMBuySellParameters.Delivery)] = BowOrderBean.f_Field3;
            lstrNames[Convert.ToInt32(CMBuySellParameters.BuySellIndicator)] = BowOrderBean.f_BuySellIndicator;
            lstrNames[Convert.ToInt32(CMBuySellParameters.AlphaChar)] = BowOrderBean.f_AlphaChar;
            lstrNames[Convert.ToInt32(CMBuySellParameters.ActualApproverId)] = BowOrderBean.f_ActualApproverId;
            lstrNames[Convert.ToInt32(CMBuySellParameters.VolumeRemaining)] = BowOrderBean.f_VolumeRemaining;
            lstrNames[Convert.ToInt32(CMBuySellParameters.DisclosedVolumeRemaining)] = BowOrderBean.f_DisclosedVolumeRemaining;
            lstrNames[Convert.ToInt32(CMBuySellParameters.AdminUsId)] = BowOrderBean.f_AdminUsId;
            lstrNames[Convert.ToInt32(CMBuySellParameters.BatchOrder)] = BowOrderBean.f_Status;
            lstrNames[Convert.ToInt32(CMBuySellParameters.ProClientIndicator)] = BowOrderBean.f_ProClientIndicator;
            lstrNames[Convert.ToInt32(CMBuySellParameters.ParticipantCode)] = BowOrderBean.f_ParticipantType;
            lstrNames[Convert.ToInt32(CMBuySellParameters.IntuitionalClient)] = BowOrderBean.f_IntuitionalClient;
            lstrNames[Convert.ToInt32(CMBuySellParameters.GoodTillDate)] = BowOrderBean.f_GoodTillDate;
            lstrNames[Convert.ToInt32(CMBuySellParameters.BookType)] = BowOrderBean.f_BookType;
            lstrNames[Convert.ToInt32(CMBuySellParameters.BlockDeal)] = BowOrderBean.f_BlockDeal;
            lstrNames[Convert.ToInt32(CMBuySellParameters.Reason)] = BowOrderBean.f_Reason;


            if (omodel.Mode == Convert.ToInt32(Modes.Edit))
            {
                lstrNames[Convert.ToInt32(CMBuySellParameters.IOC)] = BowServletsConstants.PARAMORDERIOC;

                lstrNames[Convert.ToInt32(CMBuySellParameters.TransactionCode)] = Constants.BowOrderBean.f_TransactionCode;
                lstrNames[Convert.ToInt32(CMBuySellParameters.OrderId)] = Constants.BowOrderBean.f_Id;
                lstrNames[Convert.ToInt32(CMBuySellParameters.OrderNumber)] = Constants.BowOrderBean.f_OrderNumber;
                lstrNames[Convert.ToInt32(CMBuySellParameters.RowState)] = BowOrderBean.f_RowState;
                lstrNames[Convert.ToInt32(CMBuySellParameters.Source)] = BowOrderBean.f_Source;
                lstrNames[Convert.ToInt32(CMBuySellParameters.UserId)] = "USID";
                lstrNames[Convert.ToInt32(CMBuySellParameters.LoginId)] = "USLOGINID";
            }
            else
            {
                lstrNames[Convert.ToInt32(CMBuySellParameters.IOC)] = BowServletsConstants.PARAMORDERIOC;
                lstrNames[Convert.ToInt32(CMBuySellParameters.TransactionCode)] = "";
                lstrNames[Convert.ToInt32(CMBuySellParameters.OrderId)] = "";
                lstrNames[Convert.ToInt32(CMBuySellParameters.OrderNumber)] = "";
                lstrNames[Convert.ToInt32(CMBuySellParameters.RowState)] = "";
                lstrNames[Convert.ToInt32(CMBuySellParameters.Source)] = "";
                lstrNames[Convert.ToInt32(CMBuySellParameters.UserId)] = "";
                lstrNames[Convert.ToInt32(CMBuySellParameters.LoginId)] = "";
            }
            lstrNames[Convert.ToInt32(CMBuySellParameters.Segment)] = BowOrderBean.f_Segment;
            lstrNames[Convert.ToInt32(CMBuySellParameters.Group)] = BowOrderBean.f_Group;
            lstrNames[Convert.ToInt32(CMBuySellParameters.ExcelFlag)] = BowOrderBean.f_ApproverRemarks;
            lstrNames[Convert.ToInt32(CMBuySellParameters.SolicitorPeriod)] = BowOrderBean.f_SolicitorPeriod;
            return lstrNames;
        }

        private static string[] GetValues(OrderModel omodel, int DecimalPoint)
        {
            string[] lstrValues = new string[Convert.ToInt32(Enumerations.Order.CMBuySellParameters.NO_OF_PARAMETERS) + 3];
            int lintExchangeID = (int)Enum.Parse(typeof(Enumerations.Order.Exchanges), omodel.Exchange); //mobjBusinessLogic.EMS.GetExchangeId(this.Exchange);
            int lintMarketID = (int)Enum.Parse(typeof(Enumerations.Order.ScripSegment), omodel.Segment);//mobjBusinessLogic.EMS.GetMarketId(this.Market);

            if (omodel != null)
            {
                //Need to remove hard coded values
                lstrValues[Convert.ToInt32(Enumerations.Order.CMBuySellParameters.Token)] = omodel.Token.ToString();//SecurityInfo.Id;
                if (lintExchangeID == 1)//Constants.General.EX_BSE_VALUE)
                {
                    lstrValues[Convert.ToInt32(Enumerations.Order.CMBuySellParameters.NseToken)] = "-1";
                    lstrValues[Convert.ToInt32(Enumerations.Order.CMBuySellParameters.BseToken)] = omodel.ScripCode.ToString();
                }
                else
                {
                    lstrValues[Convert.ToInt32(Enumerations.Order.CMBuySellParameters.NseToken)] = omodel.ScripCode.ToString();
                    lstrValues[Convert.ToInt32(Enumerations.Order.CMBuySellParameters.BseToken)] = "-1";
                }
            }
            lstrValues[Convert.ToInt32(Enumerations.Order.CMBuySellParameters.Destination)] = lintExchangeID.ToString();
            lstrValues[Convert.ToInt32(Enumerations.Order.CMBuySellParameters.Market)] = lintMarketID.ToString();
            if ((omodel.BuySellIndicator.ToUpper() == Enumerations.Order.BuySellFlag.BUY.ToString().ToUpper()))
            {
                lstrValues[Convert.ToInt32(Enumerations.Order.CMBuySellParameters.BuySellIndicator)] = BowConstants.BUY_VALUE;
            }
            else if (omodel.BuySellIndicator.ToUpper() == Enumerations.Order.BuySellFlag.SELL.ToString().ToUpper())
            {
                lstrValues[Convert.ToInt32(Enumerations.Order.CMBuySellParameters.BuySellIndicator)] = BowConstants.SELL_VALUE;
            }

            lstrValues[Convert.ToInt32(Enumerations.Order.CMBuySellParameters.Symbol)] = omodel.Symbol;
            lstrValues[Convert.ToInt32(Enumerations.Order.CMBuySellParameters.Series)] = omodel.Series;

            lstrValues[Convert.ToInt32(Enumerations.Order.CMBuySellParameters.Volume)] = omodel.Quantity.ToString();
            lstrValues[Convert.ToInt32(Enumerations.Order.CMBuySellParameters.VolumeRemaining)] = omodel.VolumeRemaining.ToString();

            //If String.IsNullOrEmpty(Me.DisclosedVolume) AndAlso Me.Exchange = Constants.General.EX_BSE _
            //        AndAlso Me.Market = Constants.General.MKT_EQUITY Then
            //    lstrValues(Convert.ToInt32(Enumerations.CMBuySellParameters.DisclosedVolume) = Me.Volume
            //    lstrValues(Convert.ToInt32(Enumerations.CMBuySellParameters.DisclosedVolumeRemaining) = Me.Volume
            //Else
            lstrValues[Convert.ToInt32(Enumerations.Order.CMBuySellParameters.DisclosedVolume)] = omodel.RevealQty.ToString();
            lstrValues[Convert.ToInt32(Enumerations.Order.CMBuySellParameters.DisclosedVolumeRemaining)] = omodel.DisclosedVolumeRemaining.ToString();
            //End If
            lstrValues[Convert.ToInt32(Enumerations.Order.CMBuySellParameters.Price)] = Convert.ToString(Convert.ToDouble(omodel.Price) / Math.Pow(10, DecimalPoint));
            lstrValues[Convert.ToInt32(Enumerations.Order.CMBuySellParameters.TriggerPrice)] = Convert.ToString(Convert.ToDouble(omodel.TriggerPrice) / Math.Pow(10, DecimalPoint));//omodel.TriggerPrice.ToString();

            //Need to remove hard coded values
            lstrValues[Convert.ToInt32(Enumerations.Order.CMBuySellParameters.BackOfficeId)] = objUtilityLoginDetails.UserBackOfficeId;//omodel.BackOfficeId;


            if (omodel.BlockDeal == true)
            {
                lstrValues[Convert.ToInt32(Enumerations.Order.CMBuySellParameters.BlockDeal)] = BowConstants.YESNO_Y;
            }
            else
            {
                lstrValues[Convert.ToInt32(Enumerations.Order.CMBuySellParameters.BlockDeal)] = BowConstants.YESNO_N;
            }


            //: ProOrder
            //: in Bse the ProClient check box is not displayed, instead the cboBank combobox is displayed
            //: When user selects OWN then its a pro order
            if (omodel.ProClientIndicator == true || lintExchangeID == BowConstants.EX_BSE_VALUE && omodel.ClientType.ToUpper() == BowConstants.OWN.ToUpper())
            {
                lstrValues[Convert.ToInt32(Enumerations.Order.CMBuySellParameters.BackOfficeId)] = "BROKER";
                lstrValues[Convert.ToInt32(Enumerations.Order.CMBuySellParameters.ProClientIndicator)] = "2";
            }
            else
            {
                lstrValues[Convert.ToInt32(Enumerations.Order.CMBuySellParameters.ProClientIndicator)] = "1";
            }

            lstrValues[Convert.ToInt32(Enumerations.Order.CMBuySellParameters.IntuitionalClient)] = omodel.ClientType;
            lstrValues[Convert.ToInt32(Enumerations.Order.CMBuySellParameters.ParticipantCode)] = omodel.ParticipantCode;
            if (omodel.Delivery == true)
            {
                lstrValues[Convert.ToInt32(Enumerations.Order.CMBuySellParameters.Delivery)] = BowConstants.YESNO_Y;
            }
            else
            {
                lstrValues[Convert.ToInt32(Enumerations.Order.CMBuySellParameters.Delivery)] = BowConstants.YESNO_N;
            }
            lstrValues[Convert.ToInt32(Enumerations.Order.CMBuySellParameters.AlphaChar)] = omodel.ScripCode.ToString().Substring(0, 2);//omodel.AlphaChar;

            //Need to remove hard coded values
            lstrValues[Convert.ToInt32(Enumerations.Order.CMBuySellParameters.ActualApproverId)] = objUtilityLoginDetails.UserId.ToString();//omodel.ActualApproverId;
            lstrValues[Convert.ToInt32(Enumerations.Order.CMBuySellParameters.AdminUsId)] = objUtilityLoginDetails.UserId.ToString();//omodel.AdminUsId;

            if (omodel.BatchOrder == true)
            {
                lstrValues[Convert.ToInt32(Enumerations.Order.CMBuySellParameters.BatchOrder)] = "1";
            }
            else
            {
                lstrValues[Convert.ToInt32(Enumerations.Order.CMBuySellParameters.BatchOrder)] = "";
            }
            //: for NSE GTD = 0 and for BSE GTD=1
            if (lintExchangeID == BowConstants.EX_NSE_VALUE)
            {
                lstrValues[Convert.ToInt32(Enumerations.Order.CMBuySellParameters.GoodTillDate)] = "0";

            }
            else
            {
                lstrValues[Convert.ToInt32(Enumerations.Order.CMBuySellParameters.GoodTillDate)] = GetRetentionTypeId(omodel.OrderRetentionStatus.ToUpper());
                //'If Me.OrderRetentionStatus.ToUpper = Constants.General.EOSESS.ToUpper Then
                //'    lstrValues(Convert.ToInt32(Enumerations.CMBuySellParameters.GoodTillDate) = Constants.General.EOSESS_VALUE
                //'ElseIf Me.OrderRetentionStatus.ToUpper = Constants.General.EOTODY.ToUpper Then
                //'    lstrValues(Convert.ToInt32(Enumerations.CMBuySellParameters.GoodTillDate) = Constants.General.EOTODY_VALUE
                //'ElseIf Me.OrderRetentionStatus.ToUpper = Constants.General.EOSTLM.ToUpper Then
                //'    lstrValues(Convert.ToInt32(Enumerations.CMBuySellParameters.GoodTillDate) = Constants.General.EOSTLM_VALUE
                //'End If
            }
            if (omodel.Mode == Convert.ToInt32(Enumerations.Order.Modes.Edit))//MODES.Edit)
            {
                lstrValues[Convert.ToInt32(Enumerations.Order.CMBuySellParameters.Reason)] = "";

                if (omodel.OrderRetentionStatus == "IOC")
                {
                    lstrValues[Convert.ToInt32(Enumerations.Order.CMBuySellParameters.IOC)] = "1";
                }
                else
                {
                    lstrValues[Convert.ToInt32(Enumerations.Order.CMBuySellParameters.IOC)] = "0";
                }

                lstrValues[Convert.ToInt32(Enumerations.Order.CMBuySellParameters.TransactionCode)] = omodel.TransactionCode;
                lstrValues[Convert.ToInt32(Enumerations.Order.CMBuySellParameters.OrderId)] = omodel.OrderId;
                lstrValues[Convert.ToInt32(Enumerations.Order.CMBuySellParameters.OrderNumber)] = omodel.OrderNumber;
                lstrValues[Convert.ToInt32(Enumerations.Order.CMBuySellParameters.Source)] = omodel.Source;
                lstrValues[Convert.ToInt32(Enumerations.Order.CMBuySellParameters.RowState)] = omodel.RowState;
                lstrValues[Convert.ToInt32(Enumerations.Order.CMBuySellParameters.UserId)] = omodel.UserId;
                lstrValues[Convert.ToInt32(Enumerations.Order.CMBuySellParameters.LoginId)] = omodel.LoginId;
            }
            else
            {
                lstrValues[Convert.ToInt32(Enumerations.Order.CMBuySellParameters.Reason)] = omodel.Reason + "^";
                if (omodel.OrderRetentionStatus == "IOC")
                {
                    lstrValues[Convert.ToInt32(Enumerations.Order.CMBuySellParameters.IOC)] = "1";
                }
                else
                {
                    lstrValues[Convert.ToInt32(Enumerations.Order.CMBuySellParameters.IOC)] = "0";
                }
                lstrValues[Convert.ToInt32(Enumerations.Order.CMBuySellParameters.TransactionCode)] = "";
                lstrValues[Convert.ToInt32(Enumerations.Order.CMBuySellParameters.OrderId)] = "";
                lstrValues[Convert.ToInt32(Enumerations.Order.CMBuySellParameters.OrderNumber)] = "";
                lstrValues[Convert.ToInt32(Enumerations.Order.CMBuySellParameters.Source)] = "";
                lstrValues[Convert.ToInt32(Enumerations.Order.CMBuySellParameters.RowState)] = "";
                lstrValues[Convert.ToInt32(Enumerations.Order.CMBuySellParameters.UserId)] = "";
                lstrValues[Convert.ToInt32(Enumerations.Order.CMBuySellParameters.LoginId)] = "";
            }
            lstrValues[Convert.ToInt32(Enumerations.Order.CMBuySellParameters.BookType)] = GetBookTypeId(omodel.OrderType);
            lstrValues[Convert.ToInt32(Enumerations.Order.CMBuySellParameters.Segment)] = "1";//omodel.SegmentValue;
            if (lintExchangeID == BowConstants.EX_BSE_VALUE)
            {
                lstrValues[Convert.ToInt32(Enumerations.Order.CMBuySellParameters.Group)] = omodel.Group;//((BSESecurity)omodel).Group;
            }
            else
            {
                lstrValues[Convert.ToInt32(Enumerations.Order.CMBuySellParameters.Group)] = "";
            }
            lstrValues[Convert.ToInt32(Enumerations.Order.CMBuySellParameters.ExcelFlag)] = omodel.ExcelFlag;
            if (lintExchangeID == BowConstants.EX_BSE_VALUE && lintMarketID == BowConstants.MKT_EQUITY_VALUE && (omodel.Price.ToString().Trim().Length == 0 || Convert.ToDecimal(omodel.Price) == 0) && omodel.OrderType == BowConstants.ORDER_TYPE_RL)
            {
                //: SolicitorPeriod
                if (string.IsNullOrWhiteSpace(omodel.TriggerPrice.ToString()) == false)
                {
                    lstrValues[Convert.ToInt32(Enumerations.Order.CMBuySellParameters.SolicitorPeriod)] = (omodel.TriggerPrice * 100000000).ToString();
                }
                //else if (omodel.IsDefaultOrdSettings == true && Information.IsNumeric(omodel.SecurityInfo.defaultProtPer) && omodel.SecurityInfo.defaultProtPer > 0)
                //{
                //    lstrValues[Convert.ToInt32(Enumerations.CMBuySellParameters.SolicitorPeriod)] = omodel.SecurityInfo.defaultProtPer * 100000000;
                //}
                //else if (glngDefaultProtectionPercentage > 0)
                //{
                //    lstrValues[Convert.ToInt32(Enumerations.CMBuySellParameters.SolicitorPeriod)] = glngDefaultProtectionPercentage * 100000000;
                //}
                else
                {
                    lstrValues[Convert.ToInt32(Enumerations.Order.CMBuySellParameters.SolicitorPeriod)] = "";
                }
            }
            else
            {
                lstrValues[Convert.ToInt32(Enumerations.Order.CMBuySellParameters.SolicitorPeriod)] = "";
            }

            return lstrValues;
        }

        private static string[] GetNames_Structure_2(OrderModel omodel)
        {
            string[] lstrNames = new string[40];
            int ExchangeID = (int)Enum.Parse(typeof(Exchanges), omodel.Exchange);
            //lstrNames.Append(BowOrderBean.f_Token)
            lstrNames[Convert.ToInt32(FNOBuySellParameters.Token)] = BowOrderBean.f_Token;
            lstrNames[Convert.ToInt32(FNOBuySellParameters.NseToken)] = BowOrderBean.f_NseToken;
            lstrNames[Convert.ToInt32(FNOBuySellParameters.Destination)] = BowOrderBean.f_Destination;
            lstrNames[Convert.ToInt32(FNOBuySellParameters.Market)] = BowOrderBean.f_Market;
            lstrNames[Convert.ToInt32(FNOBuySellParameters.Symbol)] = BowOrderBean.f_Symbol;
            lstrNames[Convert.ToInt32(FNOBuySellParameters.Name)] = BowOrderBean.f_Name;
            lstrNames[Convert.ToInt32(FNOBuySellParameters.InstrumentType)] = BowOrderBean.f_InstrumentType;
            lstrNames[Convert.ToInt32(FNOBuySellParameters.ExpiryDate)] = BowOrderBean.f_ExpiryDate;
            lstrNames[Convert.ToInt32(FNOBuySellParameters.StrikePrice)] = BowOrderBean.f_StrikePrice;
            lstrNames[Convert.ToInt32(FNOBuySellParameters.OptionType)] = BowOrderBean.f_OptionType;
            lstrNames[Convert.ToInt32(FNOBuySellParameters.Volume)] = BowOrderBean.f_Volume;
            lstrNames[Convert.ToInt32(FNOBuySellParameters.Price)] = BowOrderBean.f_Price;
            lstrNames[Convert.ToInt32(FNOBuySellParameters.TriggerPrice)] = BowOrderBean.f_TriggerPrice;
            lstrNames[Convert.ToInt32(FNOBuySellParameters.VolumeRemaining)] = BowOrderBean.f_VolumeRemaining;
            lstrNames[Convert.ToInt32(FNOBuySellParameters.BuySellIndicator)] = BowOrderBean.f_BuySellIndicator;
            lstrNames[Convert.ToInt32(FNOBuySellParameters.BackOfficeId)] = BowOrderBean.f_BackOfficeId;
            lstrNames[Convert.ToInt32(FNOBuySellParameters.Reason)] = BowOrderBean.f_Reason;
            lstrNames[Convert.ToInt32(FNOBuySellParameters.DisclosedVolume)] = BowOrderBean.f_DisclosedVolume;
            if (ExchangeID == BowConstants.EX_NSE_VALUE)
            {
                lstrNames[Convert.ToInt32(FNOBuySellParameters.GoodTillDate)] = BowOrderBean.f_GoodTillDate;
                lstrNames[Convert.ToInt32(FNOBuySellParameters.IOC)] = BowServletsConstants.PARAMORDERIOC;
            }
            else if (ExchangeID == BowConstants.EX_NCDEX_VALUE || ExchangeID == BowConstants.EX_NMCE_VALUE || ExchangeID == BowConstants.EX_DGCX_VALUE ||
                ExchangeID == BowConstants.EX_MCX_VALUE || ExchangeID == BowConstants.EX_BSE_VALUE
                || ExchangeID == BowConstants.EX_USE_VALUE)
            {
                lstrNames[Convert.ToInt32(FNOBuySellParameters.GoodTillDate)] = BowOrderBean.f_GoodTillDate;
                lstrNames[Convert.ToInt32(FNOBuySellParameters.GoodTillCancel)] = BowServletsConstants.PARAMORDERGTC;
                lstrNames[Convert.ToInt32(FNOBuySellParameters.IOC)] = BowServletsConstants.PARAMORDERIOC;
            }

            lstrNames[Convert.ToInt32(FNOBuySellParameters.ProClientIndicator)] = BowOrderBean.f_ProClientIndicator;
            lstrNames[Convert.ToInt32(FNOBuySellParameters.ParticipantCode)] = BowOrderBean.f_Settlor;
            if (ExchangeID == BowConstants.EX_NMCE_VALUE || ExchangeID == BowConstants.EX_BSE_VALUE || ExchangeID == BowConstants.EX_USE_VALUE)
            {
                lstrNames[Convert.ToInt32(FNOBuySellParameters.ClientType)] = BowOrderBean.f_IntuitionalClient;
                lstrNames[Convert.ToInt32(FNOBuySellParameters.OrderType)] = BowOrderBean.f_Field1;
            }
            else if (ExchangeID == BowConstants.EX_MCX_VALUE || ExchangeID == BowConstants.EX_DGCX_VALUE)
            {
                lstrNames[Convert.ToInt32(FNOBuySellParameters.ClientType)] = BowOrderBean.f_IntuitionalClient;
                lstrNames[Convert.ToInt32(FNOBuySellParameters.OrderType)] = BowOrderBean.f_Field1;
                lstrNames[Convert.ToInt32(FNOBuySellParameters.UserRemarks)] = BowOrderBean.f_Remarks;
            }

            if (omodel.Mode == Convert.ToInt32(Modes.Edit))
            {
                lstrNames[Convert.ToInt32(FNOBuySellParameters.TransactionCode)] = BowOrderBean.f_TransactionCode;
                lstrNames[Convert.ToInt32(FNOBuySellParameters.OrderId)] = BowOrderBean.f_Id;
                lstrNames[Convert.ToInt32(FNOBuySellParameters.OrderNumber)] = BowOrderBean.f_OrderNumber;
                lstrNames[Convert.ToInt32(FNOBuySellParameters.RowState)] = BowOrderBean.f_RowState;
                lstrNames[Convert.ToInt32(FNOBuySellParameters.Source)] = BowOrderBean.f_Source;
                lstrNames[Convert.ToInt32(FNOBuySellParameters.Settlor)] = BowOrderBean.f_Settlor;
                lstrNames[Convert.ToInt32(FNOBuySellParameters.UserId)] = BowOrderBean.f_Id;
                lstrNames[Convert.ToInt32(FNOBuySellParameters.LoginId)] = "USLOGINID";
            }
            else
            {
                lstrNames[Convert.ToInt32(FNOBuySellParameters.TransactionCode)] = "";
                lstrNames[Convert.ToInt32(FNOBuySellParameters.OrderId)] = "";
                lstrNames[Convert.ToInt32(FNOBuySellParameters.OrderNumber)] = BowOrderBean.f_OrderNumber;
                lstrNames[Convert.ToInt32(FNOBuySellParameters.RowState)] = "";
                lstrNames[Convert.ToInt32(FNOBuySellParameters.Source)] = "";
                lstrNames[Convert.ToInt32(FNOBuySellParameters.Settlor)] = "";
                lstrNames[Convert.ToInt32(FNOBuySellParameters.UserId)] = "";
                lstrNames[Convert.ToInt32(FNOBuySellParameters.LoginId)] = "";
            }

            lstrNames[Convert.ToInt32(FNOBuySellParameters.ExcelFlag)] = BowOrderBean.f_ApproverRemarks;
            lstrNames[Convert.ToInt32(FNOBuySellParameters.SolicitorPeriod)] = BowOrderBean.f_SolicitorPeriod;
            if (ExchangeID == BowConstants.EX_DGCX_VALUE && ExchangeID == BowConstants.MKT_COMMODITIES_VALUE)
            {
                lstrNames[Convert.ToInt32(FNOBuySellParameters.Series)] = BowOrderBean.f_Series;
            }

            if (ExchangeID == BowConstants.EX_BSE_VALUE && (int)Enum.Parse(typeof(ScripSegment), omodel.Segment) == BowConstants.MKT_SLB_VALUE)
            {
                lstrNames[Convert.ToInt32(FNOBuySellParameters.BseToken)] = BowOrderBean.f_BseToken;
            }

            lstrNames[Convert.ToInt32(FNOBuySellParameters.Delivery)] = BowOrderBean.f_Field3;
            return lstrNames;
        }

        private static string[] GetValues_Structure_2(OrderModel omodel, int DecimalPoint)
        {
            string[] lstrValues = new string[40];
            int lintExchangeID = (int)Enum.Parse(typeof(Exchanges), omodel.Exchange);
            int lintMarketID = (int)Enum.Parse(typeof(ScripSegment), omodel.Segment);

            if (omodel != null)
            {
                lstrValues[Convert.ToInt32(FNOBuySellParameters.Token)] = omodel.Token.ToString();
                lstrValues[Convert.ToInt32(FNOBuySellParameters.NseToken)] = omodel.ScripCode.ToString();
            }

            lstrValues[Convert.ToInt32(FNOBuySellParameters.Destination)] = lintExchangeID.ToString();
            lstrValues[Convert.ToInt32(FNOBuySellParameters.Market)] = lintMarketID.ToString();


            if (lintExchangeID == BowConstants.EX_DGCX_VALUE)
            {
                lstrValues[Convert.ToInt32(FNOBuySellParameters.Symbol)] = omodel.ScripCode.ToString();
            }
            else
            {
                lstrValues[Convert.ToInt32(FNOBuySellParameters.Symbol)] = omodel.Symbol;
            }

            lstrValues[Convert.ToInt32(FNOBuySellParameters.Name)] = omodel.ScripName;
            lstrValues[Convert.ToInt32(FNOBuySellParameters.InstrumentType)] = omodel.InstrumentName;
            lstrValues[Convert.ToInt32(FNOBuySellParameters.DisclosedVolume)] = omodel.RevealQty.ToString();
            //if (gblnLDBAll == true || gblnLDBSelf == true)
            //{
            //    if (omodel.UsedForBulkOrders == false)
            //    {
            //        lstrValues[Convert.ToInt32(FNOBuySellParameters.ExpiryDate)] = gobjUtilitySQLScript.GetLongExpiryDateForScript(lintExchangeID, omodel.ExpiryDate, lintMarketID);
            //    }
            //    else
            //    {
            //        lstrValues[Convert.ToInt32(FNOBuySellParameters.ExpiryDate)] = omodel.ExpiryDate;
            //    }

            //    lstrValues[Convert.ToInt64(FNOBuySellParameters.StrikePrice)] = omodel.StrikePrice;//UtilityScript.GetActualStrikePrice(omodel.StrikePrice, lintMarketID);
            //}
            //else
            //{
            if (omodel.UsedForBulkOrders == false)
            {
                lstrValues[Convert.ToInt32(FNOBuySellParameters.ExpiryDate)] = omodel.ExpiryDate;//mobjBusinessLogic.ScriptDetails.GetLongExpiryDateForScript(lintExchangeID, omodel.ExpiryDate, lintMarketID);
            }
            else
            {
                lstrValues[Convert.ToInt32(FNOBuySellParameters.ExpiryDate)] = omodel.ExpiryDate;
            }

            lstrValues[Convert.ToInt32(FNOBuySellParameters.StrikePrice)] = omodel.StrikePrice;//UtilityScript.GetActualStrikePrice(omodel.StrikePrice, lintMarketID);                
            //}

            lstrValues[Convert.ToInt32(FNOBuySellParameters.OptionType)] = omodel.OptionType;
            lstrValues[Convert.ToInt32(FNOBuySellParameters.Volume)] = omodel.Quantity.ToString();
            lstrValues[Convert.ToInt32(FNOBuySellParameters.VolumeRemaining)] = omodel.VolumeRemaining.ToString();
            lstrValues[Convert.ToInt32(FNOBuySellParameters.Price)] = omodel.Price.ToString();
            lstrValues[Convert.ToInt32(FNOBuySellParameters.TriggerPrice)] = omodel.TriggerPrice.ToString();
            if (lintExchangeID != BowConstants.EX_NSE_VALUE)
            {
                lstrValues[Convert.ToInt32(FNOBuySellParameters.GoodTillDate)] = omodel.OrderRetentionStatus;
                lstrValues[Convert.ToInt32(FNOBuySellParameters.GoodTillCancel)] = omodel.GoodTillCancel;
            }

            if ((omodel.BuySellIndicator.ToUpper() == BuySellFlag.BUY.ToString()))
            {
                lstrValues[Convert.ToInt32(FNOBuySellParameters.BuySellIndicator)] = BowConstants.BUY_VALUE;
            }
            else if (omodel.BuySellIndicator.ToUpper().Trim() == BuySellFlag.SELL.ToString())
            {
                lstrValues[Convert.ToInt32(FNOBuySellParameters.BuySellIndicator)] = BowConstants.SELL_VALUE;
            }

            lstrValues[Convert.ToInt32(FNOBuySellParameters.BackOfficeId)] = omodel.BackOfficeId;
            if (lintExchangeID == BowConstants.EX_NCDEX_VALUE || lintExchangeID == BowConstants.EX_NMCE_VALUE || lintExchangeID == BowConstants.EX_MCX_VALUE || lintExchangeID == BowConstants.EX_DGCX_VALUE || lintExchangeID == BowConstants.EX_BSE_VALUE || lintExchangeID == BowConstants.EX_USE_VALUE)
            {
                lstrValues[Convert.ToInt32(FNOBuySellParameters.GoodTillDate)] = "0";
                if (lintMarketID != BowConstants.MKT_CURRENCY_VALUE)
                {
                    lstrValues[Convert.ToInt32(FNOBuySellParameters.GoodTillCancel)] = "0";
                }

                if (omodel.GoodTillCancel.Trim() == "")
                {
                }
                else if (omodel.GoodTillCancel.Trim().ToUpper() == BowConstants.GOOD_TILL_DAYS.ToUpper())
                {
                    lstrValues[Convert.ToInt32(FNOBuySellParameters.GoodTillDate)] = omodel.GoodTillDate.Trim();
                    lstrValues[Convert.ToInt32(FNOBuySellParameters.GoodTillCancel)] = "0";
                }
                else if (omodel.GoodTillCancel.Trim().ToUpper() == BowConstants.GOOD_TILL_DATE.ToUpper())
                {
                    lstrValues[Convert.ToInt32(FNOBuySellParameters.GoodTillDate)] = omodel.GoodTillDate.Trim();
                    lstrValues[Convert.ToInt32(FNOBuySellParameters.GoodTillCancel)] = "0";
                }
                else if (omodel.GoodTillCancel.Trim().ToUpper() == BowConstants.GOOD_TILL_CANCEL.ToUpper())
                {
                    if (lintExchangeID == BowConstants.EX_NMCE_VALUE || lintExchangeID == BowConstants.EX_MCX_VALUE || lintExchangeID == BowConstants.EX_DGCX_VALUE || lintExchangeID == BowConstants.EX_BSE_VALUE || lintExchangeID == BowConstants.EX_USE_VALUE)
                    {
                        lstrValues[Convert.ToInt32(FNOBuySellParameters.GoodTillDate)] = "-1";
                        lstrValues[Convert.ToInt32(FNOBuySellParameters.GoodTillCancel)] = "1";
                    }
                    else
                    {
                        lstrValues[Convert.ToInt32(FNOBuySellParameters.GoodTillDate)] = "0";
                        lstrValues[Convert.ToInt32(FNOBuySellParameters.GoodTillCancel)] = "1";
                    }
                }
                else if (omodel.GoodTillCancel.Trim().ToUpper() == BowConstants.GOOD_FOR_A_DAY.ToUpper())
                {
                    if (lintExchangeID == BowConstants.EX_DGCX_VALUE)
                    {
                        lstrValues[Convert.ToInt32(FNOBuySellParameters.GoodTillDate)] = "2";
                    }
                    else
                    {
                        lstrValues[Convert.ToInt32(FNOBuySellParameters.GoodTillDate)] = omodel.GoodTillDate.Trim();
                    }

                    lstrValues[Convert.ToInt32(FNOBuySellParameters.GoodTillCancel)] = "0";
                }
                else if (omodel.GoodTillCancel.Trim().ToUpper() == RetType.EOS.ToString())
                {
                    lstrValues[Convert.ToInt32(FNOBuySellParameters.GoodTillDate)] = "1";
                    lstrValues[Convert.ToInt32(FNOBuySellParameters.GoodTillCancel)] = "0";
                }
                else if (omodel.GoodTillCancel.Trim().ToUpper() == BowConstants.CANCEL_ON_LOGOUT.ToUpper())
                {
                    lstrValues[Convert.ToInt32(FNOBuySellParameters.GoodTillDate)] = "-9";
                    lstrValues[Convert.ToInt32(FNOBuySellParameters.GoodTillCancel)] = "0";
                }
            }

            lstrValues[Convert.ToInt32(FNOBuySellParameters.IOC)] = "0";
            if (omodel.GoodTillCancel.Trim().ToUpper() == BowServletsConstants.PARAMORDERIOC)
            {
                lstrValues[Convert.ToInt32(FNOBuySellParameters.IOC)] = "1";
            }

            if (omodel.Exchange == Exchanges.DGCX.ToString())
            {
                if (omodel.GoodTillCancel.Trim().ToUpper() == BowServletsConstants.PARAMORDERIOC)
                {
                    lstrValues[Convert.ToInt32(FNOBuySellParameters.IOC)] = "0";
                }
                else
                {
                    lstrValues[Convert.ToInt32(FNOBuySellParameters.IOC)] = "1";
                }
            }

            if (omodel.Exchange == Exchanges.BSE.ToString() && omodel.Segment != ScripSegment.Currency.ToString())
            {
                if (omodel.ImmediateOrCancel == true)
                {
                    lstrValues[Convert.ToInt32(FNOBuySellParameters.IOC)] = "1";
                }
                else
                {
                    lstrValues[Convert.ToInt32(FNOBuySellParameters.IOC)] = "0";
                }
            }

            if (omodel.Delivery)
            {
                lstrValues[Convert.ToInt32(FNOBuySellParameters.Delivery)] = "Y";
            }
            else
            {
                lstrValues[Convert.ToInt32(FNOBuySellParameters.Delivery)] = "N";
            }

            if (omodel.ProClientIndicator == true)
            {
                lstrValues[Convert.ToInt32(FNOBuySellParameters.ProClientIndicator)] = "2";
                lstrValues[Convert.ToInt32(FNOBuySellParameters.BackOfficeId)] = "BROKER";
                lstrValues[Convert.ToInt32(FNOBuySellParameters.ClientType)] = "PRO";
            }
            else
            {
                lstrValues[Convert.ToInt32(FNOBuySellParameters.ProClientIndicator)] = "1";
                lstrValues[Convert.ToInt32(FNOBuySellParameters.ClientType)] = omodel.ClientType;
            }

            if (lintExchangeID == BowConstants.EX_NMCE_VALUE || lintExchangeID == BowConstants.EX_BSE_VALUE || lintExchangeID == BowConstants.EX_USE_VALUE)
            {
                lstrValues[Convert.ToInt32(FNOBuySellParameters.OrderType)] = omodel.OrderType;
            }
            else if (lintExchangeID == BowConstants.EX_MCX_VALUE || lintExchangeID == BowConstants.EX_DGCX_VALUE)
            {
                lstrValues[Convert.ToInt32(FNOBuySellParameters.OrderType)] = omodel.OrderType;
                lstrValues[Convert.ToInt32(FNOBuySellParameters.UserRemarks)] = omodel.UserRemarks;
                lstrValues[Convert.ToInt32(FNOBuySellParameters.ParticipantCode)] = omodel.ParticipantCode;
            }

            if (omodel.Mode == Convert.ToInt32(Modes.Edit))
            {
                lstrValues[Convert.ToInt32(FNOBuySellParameters.Reason)] = "";
                lstrValues[Convert.ToInt32(FNOBuySellParameters.TransactionCode)] = omodel.TransactionCode;
                lstrValues[Convert.ToInt32(FNOBuySellParameters.OrderId)] = omodel.OrderId;
                lstrValues[Convert.ToInt32(FNOBuySellParameters.OrderNumber)] = omodel.OrderNumber;
                lstrValues[Convert.ToInt32(FNOBuySellParameters.Source)] = omodel.Source;
                lstrValues[Convert.ToInt32(FNOBuySellParameters.RowState)] = omodel.RowState;
                lstrValues[Convert.ToInt32(FNOBuySellParameters.Settlor)] = omodel.Settlor;
                lstrValues[Convert.ToInt32(FNOBuySellParameters.UserId)] = omodel.UserId;
                lstrValues[Convert.ToInt32(FNOBuySellParameters.LoginId)] = omodel.LoginId;
            }
            else
            {
                lstrValues[Convert.ToInt32(FNOBuySellParameters.Reason)] = omodel.UserRemarks + "^";
                lstrValues[Convert.ToInt32(FNOBuySellParameters.TransactionCode)] = "";
                lstrValues[Convert.ToInt32(FNOBuySellParameters.OrderId)] = "";
                if (omodel.OrderNumber != "" && !string.IsNullOrEmpty(omodel.OrderNumber))
                {
                    lstrValues[Convert.ToInt32(FNOBuySellParameters.OrderNumber)] = omodel.OrderNumber;
                }
                else
                {
                    lstrValues[Convert.ToInt32(FNOBuySellParameters.OrderNumber)] = "";
                }

                lstrValues[Convert.ToInt32(FNOBuySellParameters.Source)] = "";
                lstrValues[Convert.ToInt32(FNOBuySellParameters.RowState)] = "";
                lstrValues[Convert.ToInt32(FNOBuySellParameters.Settlor)] = "";
                lstrValues[Convert.ToInt32(FNOBuySellParameters.UserId)] = "";
                lstrValues[Convert.ToInt32(FNOBuySellParameters.LoginId)] = "";
            }

            lstrValues[Convert.ToInt32(FNOBuySellParameters.ExcelFlag)] = omodel.ExcelFlag;
            if (lintExchangeID == BowConstants.EX_BSE_VALUE && (lintMarketID == BowConstants.MKT_DERIVATIVE_VALUE || lintMarketID == BowConstants.MKT_CURRENCY_VALUE) &&
                omodel.OrderType == BowConstants.ORDER_TYPE_PF_CONV_VALUE)
            {
                //TODO: resolve this 
                //if (this.ContractInfo.IsDefaultOrdSettings == true && IsNumeric(this.ContractInfo.defaultProtPer) && omodel.ContractInfo.defaultProtPer > 0)
                //{
                //    lstrValues[Convert.ToInt32(FNOBuySellParameters.SolicitorPeriod)] = this.ContractInfo.defaultProtPer * 100000000;
                //}
                //else if (glngDefaultProtectionPercentage > 0)
                //{
                //    lstrValues[Convert.ToInt32(FNOBuySellParameters.SolicitorPeriod)] = glngDefaultProtectionPercentage * 100000000;
                //}
                //else
                //{
                //    lstrValues[Convert.ToInt32(FNOBuySellParameters.SolicitorPeriod)] = "";
                //}
                //TODO: resolve this
            }
            else
            {
                lstrValues[Convert.ToInt32(FNOBuySellParameters.SolicitorPeriod)] = "";
            }

            if (lintExchangeID == BowConstants.EX_DGCX_VALUE && lintMarketID == BowConstants.MKT_COMMODITIES_VALUE)
            {
                lstrValues[Convert.ToInt32(FNOBuySellParameters.Series)] = "*D";
            }

            if (lintExchangeID == BowConstants.EX_BSE_VALUE && lintMarketID == BowConstants.MKT_SLB_VALUE)
            {
                lstrValues[Convert.ToInt32(FNOBuySellParameters.BseToken)] = omodel.RollOver;
            }

            return lstrValues;
        }

        public static string GetRetentionTypeId(string pstrRetentionTypeString)
        {
            string lstrTemp = null;
            switch (pstrRetentionTypeString.Trim().ToUpper())
            {
                case "EOS":
                    lstrTemp = BowConstants.EOSESS_VALUE;
                    break;
                case "EOSTLM":
                    lstrTemp = BowConstants.EOSTLM_VALUE;
                    break;
                case "EOTODY":
                    lstrTemp = BowConstants.EOTODY_VALUE;
                    break;
                default:
                    lstrTemp = "";
                    break;
            }
            return lstrTemp;
        }

        public static string GetBookTypeId(string pstrBookType)
        {
            string lstrTemp = null;
            switch (pstrBookType.Trim().ToUpper())
            {
                case "LIMIT":
                    lstrTemp = BowConstants.ORDER_TYPE_RL_VALUE;
                    break;
                case "Stoploss":
                    lstrTemp = BowConstants.ORDER_TYPE_SL_VALUE;
                    break;
                case "RollOver":
                    lstrTemp = BowConstants.ORDER_TYPE_RollOver_VALUE;
                    break;
                case "OCO":
                    lstrTemp = BowConstants.ORDER_TYPE_OCO_VALUE;
                    break;
                default:
                    lstrTemp = "";
                    break;
            }
            return lstrTemp;
        }

        private static void HTTPOrderResponseArrived(string pstrResponse)
        {
            RecordSplitter lobjResponseHelper = new RecordSplitter(pstrResponse);
            if (lobjResponseHelper.getField(0, 0) != BowConstants.SUCCESS_FLAG)
            {
                if (lobjResponseHelper.getField(0, 1).Trim().ToUpper().IndexOf("BUY") > 0)
                {
                    OrderSent?.Invoke(lobjResponseHelper.getField(0, 1), 1);
                }
                else
                {
                    OrderSent?.Invoke(lobjResponseHelper.getField(0, 1), 2);
                }

                MessageBox.Show(lobjResponseHelper.getField(0, 0) + lobjResponseHelper.getField(0, 1));
            }

        }

        internal static OrderModel ProcessOrderQueue(RecordSplitter pobjRecordHelper)
        {
            //TODO: Split the record and append it in order model 
            OrderModel omodel = new OrderModel();
            try
            {
                if (pobjRecordHelper != null)
                {
                    bool lblnISSendExcel = false;
                    string MessageIdentifier = pobjRecordHelper.getField(0, 0).Trim();
                    string ExchangeId = string.Empty;
                    string SegmentID = string.Empty;
                    string InstrumentType = string.Empty;
                    string IntraDayFlag = string.Empty;
                    string Price = string.Empty;
                    string BookTypeFlag = string.Empty;
                    string ProClient = string.Empty;
                    int Strike_Price = 0;

                    omodel.TransactionCode = pobjRecordHelper.getField(0, 1).Trim();
                    if (MessageIdentifier == MessageIdentifiers.MESSAGE_ORDER_STREAM)
                    {

                        if (pobjRecordHelper.getField(0, Convert.ToInt32(OrderConfirmation.OEBookType)).Trim() == BowConstants.ORDER_TYPE_ODDLOTGrab_VALUE)
                        {
                            return omodel;
                        }

                        if (pobjRecordHelper.getField(0, Convert.ToInt32(OrderConfirmation.OEBookType)).Trim() == BowConstants.ORDER_TYPE_ODDLOTGrab_VALUE
                            || pobjRecordHelper.getField(0, Convert.ToInt32(OrderConfirmation.OEBookType)).Trim() == BowConstants.ORDER_TYPE_ODDLOT_VALUE)
                        {
                            //TODO: Need to write Logic for ODD Lot
                            //if ((mobjfrmoddlot != null) && mobjfrmoddlot.IsDisposed == false)
                            //{
                            //    mobjfrmoddlot.RefreshOddLotDepth();
                            //}
                        }


                        omodel.OrderId = pobjRecordHelper.getField(0, Convert.ToInt32(OrderConfirmation.OEID)).Trim();
                        omodel.OrderNumber = pobjRecordHelper.getField(0, Convert.ToInt32(OrderConfirmation.OEOrderNumber)).Trim();
                        omodel.UserId = pobjRecordHelper.getField(0, Convert.ToInt32(OrderConfirmation.OEUsId)).Trim();

                        ExchangeId = pobjRecordHelper.getField(0, Convert.ToInt32(OrderConfirmation.OEExchange)).Trim();
                        omodel.Exchange = Enum.GetName(typeof(Exchanges), Convert.ToInt32(ExchangeId));
                        //pobjRecordHelper.setField(0, Convert.ToInt32(OrderConfirmation.OEExchange), Enum.GetName(typeof(Exchanges), ExchangeId));

                        SegmentID = pobjRecordHelper.getField(0, Convert.ToInt32(OrderConfirmation.OEMarket)).Trim();
                        InstrumentType = pobjRecordHelper.getField(0, Convert.ToInt32(OrderConfirmation.Size) + 1).Trim();
                        omodel.Symbol = pobjRecordHelper.getField(0, Convert.ToInt32(OrderConfirmation.Size + 2)).Trim();
                        omodel.OrderRemarks = pobjRecordHelper.getField(0, Convert.ToInt32(OrderConfirmation.OERemarks)).Trim();
                        omodel.Reason = pobjRecordHelper.getField(0, Convert.ToInt32(OrderConfirmation.OEReason)).Trim();

                        omodel.Quantity = Convert.ToInt32(pobjRecordHelper.getField(0, Convert.ToInt32(OrderConfirmation.OEVolume)).Trim());
                        omodel.RevealQty = Convert.ToInt32(pobjRecordHelper.getField(0, Convert.ToInt32(OrderConfirmation.OEDisclosedVolume)).Trim());
                        omodel.ClientType = pobjRecordHelper.getField(0, 73).Trim();
                        omodel.Time = pobjRecordHelper.getField(0, Convert.ToInt32(OrderConfirmation.OECreatedAt)).Trim();
                        if (ExchangeId.Trim() == BowConstants.EX_USE_VALUE.ToString().Trim() && GetSegmentIDBasedonInstrument(InstrumentType) == Convert.ToInt32(BowConstants.SGT_FUTURES_VALUE))
                        {
                            // : By Passing the Split Spread Trade
                            if (omodel.Symbol.Trim().Length < 11 && Convert.ToInt64(omodel.OrderNumber) < 0)
                            {
                                return omodel;
                            }
                        }
                        IntraDayFlag = pobjRecordHelper.getField(0, Convert.ToInt32(OrderConfirmation.Size + 17));
                        if (Convert.ToInt32(SegmentID) != BowConstants.MKT_EQUITY_VALUE)
                        {
                            omodel.Segment = Enum.GetName(typeof(ScripSegment), GetSegmentIDBasedonInstrument(InstrumentType, IntraDayFlag));
                            //pobjRecordHelper.setField(0, Convert.ToInt32(OrderConfirmation.OEMarket), Enum.Parse(typeof(Enumerations.Order), GetSegmentIDBasedonInstrument(InstrumentType, IntraDayFlag).ToString()).ToString());
                        }
                        else
                        {
                            //pobjRecordHelper.setField(0, Convert.ToInt32(OrderConfirmation.OEMarket), "Equity");
                            omodel.Segment = Enum.GetName(typeof(ScripSegment), Convert.ToInt32(SegmentID));
                        }
                        omodel.Token = Convert.ToInt32(pobjRecordHelper.getField(0, Convert.ToInt32(OrderConfirmation.OEToken)).Trim());
                        omodel.Price = Convert.ToInt64(pobjRecordHelper.getField(0, Convert.ToInt32(OrderConfirmation.OEPrice)).Trim());



                        if (omodel.Exchange == Enumerations.Order.Exchanges.BSE.ToString())
                        {
                            if (MasterSharedMemory.objMastertxtDictBaseBSE != null)
                            {

                                if (omodel.Token != 0)
                                {
                                    omodel.ScripCode = MasterSharedMemory.objMastertxtDictBaseBSE.Where(x => x.Value.BowTokenID == omodel.Token).Select(x => x.Value.ScripCode).FirstOrDefault();
                                    omodel.ScripName = MasterSharedMemory.objMastertxtDictBaseBSE.Where(x => x.Value.BowTokenID == omodel.Token).Select(x => x.Value.ScripName).FirstOrDefault();
                                }
                            }

                        }
                        //For NSE
                        //for NSE Exchange
                        if (omodel.Exchange == Enumerations.Order.Exchanges.NSE.ToString())
                        {
                            if (omodel.Token != 0)
                            {
                                omodel.ScripCode = MasterSharedMemory.objMastertxtDictBaseNSE.Where(x => x.Value.BowTokenID == omodel.Token).Select(x => x.Value.ScripCode).FirstOrDefault();
                                omodel.ScripName = MasterSharedMemory.objMastertxtDictBaseNSE.Where(x => x.Value.BowTokenID == omodel.Token).Select(x => x.Value.ScripName).FirstOrDefault();
                            }
                        }

                        //if (omodel.Price != 0)
                        //{
                        //    if (MarketId == BowConstants.MKT_CURRENCY_VALUE.ToString())
                        //    {
                        //        pobjRecordHelper.setField(0, Convert.ToInt32(OrderConfirmation.OEPrice), string.Format("{0}", (omodel.Price / 10000), 4));
                        //    }
                        //    else
                        //    {
                        //        pobjRecordHelper.setField(0, Convert.ToInt32(OrderConfirmation.OEPrice), string.Format("{0}", (omodel.Price / 100), 2));
                        //    }
                        //}

                        // omodel.BuySellIndicator = Enum.GetName(typeof(Exchanges), pobjRecordHelper.getField(0, Convert.ToInt32(OrderConfirmation.Size + 7)).Trim());
                        // IF MARKET IS MUTUAL FUND THEN VOLUME AND RE VOLUME ARE DIVIDE BY 10000 AND PRICE DIVIDED BY 1000000

                        if (pobjRecordHelper.getField(0, Convert.ToInt32(OrderConfirmation.Size + 7)).Trim() == "1")
                        {
                            if (SegmentID == BowConstants.MKT_MF_VALUE.ToString())
                            {
                                //TODO: Write Logic for Mutual Fund
                                //decimal ldecValue = Convert.ToDecimal(pobjRecordHelper.getField(0, Convert.ToInt32(OrderConfirmation.Size + 11)).Trim()) / 10000;
                                //pobjRecordHelper.setField(0, Convert.ToInt32(OrderConfirmation.Size + 11), ldecValue.ToString());
                                //pobjRecordHelper.setField(0, Convert.ToInt32(OrderConfirmation.OEPrice), String.Format("{0}", (omodel.Price / 1000000), 4));
                            }
                            omodel.BuySellIndicator = BuySellFlag.BUY.ToString();
                        }
                        else
                        {
                            if (SegmentID == BowConstants.MKT_MF_VALUE.ToString())
                            {
                                //TODO: Write Logic for Mutual Fund
                                //    decimal lstrVolume = Convert.ToDecimal(pobjRecordHelper.getField(0, Convert.ToInt32(OrderConfirmation.OEVolume)).Trim()) / 10000;
                                //    decimal lstrVolumeRemaining = Convert.ToDecimal(pobjRecordHelper.getField(0, Convert.ToInt32(OrderConfirmation.OEVolumeRemaining)).Trim()) / 10000;
                                //    pobjRecordHelper.setField(0, Convert.ToInt32(OrderConfirmation.OEVolume), lstrVolume.ToString());
                                //    pobjRecordHelper.setField(0, Convert.ToInt32(OrderConfirmation.OEVolumeRemaining), lstrVolumeRemaining.ToString());
                            }
                            omodel.BuySellIndicator = BuySellFlag.SELL.ToString();
                        }
                        BookTypeFlag = pobjRecordHelper.getField(0, Convert.ToInt32(OrderConfirmation.OEBookType)).Trim();
                        if (BookTypeFlag == "1")
                        {
                            omodel.OrderType = "Normal";
                            // pobjRecordHelper.setField(0, Convert.ToInt32(OrderConfirmation.OEBookType), "Normal");
                        }
                        else if (BookTypeFlag == "3")
                        {
                            omodel.OrderType = "SL";
                            //  pobjRecordHelper.setField(0, Convert.ToInt32(OrderConfirmation.OEBookType), "SL");
                        }
                        else if (BookTypeFlag == "8")
                        {

                            if (ExchangeId == BowConstants.EX_BSE_VALUE.ToString() & SegmentID == BowConstants.MKT_SLB_VALUE.ToString())
                            {
                                omodel.OrderType = "Normal";
                                //pobjRecordHelper.setField(0, Convert.ToInt32(OrderConfirmation.OEBookType), "Normal");
                            }
                            else
                            {
                                omodel.OrderType = "OCO";
                                //pobjRecordHelper.setField(0, Convert.ToInt32(OrderConfirmation.OEBookType), "OCO");
                            }

                        }
                        else if (BookTypeFlag == "9")
                        {
                            omodel.OrderType = "Block Deal";
                            //  pobjRecordHelper.setField(0, Convert.ToInt32(OrderConfirmation.OEBookType), "Block Deal");
                        }
                        else if (BookTypeFlag == "15")
                        {
                            omodel.OrderType = "Odd Lot";
                            //pobjRecordHelper.setField(0, Convert.ToInt32(OrderConfirmation.OEBookType), "Odd Lot");
                        }
                        else if (BookTypeFlag == "16")
                        {
                            omodel.OrderType = "Odd Lot Grab";
                            // pobjRecordHelper.setField(0, Convert.ToInt32(OrderConfirmation.OEBookType), "Odd Lot Grab");
                        }
                        if (SegmentID == BowConstants.MKT_OFS_VALUE.ToString())
                        {
                            if (BookTypeFlag == "2")
                            {
                                omodel.OrderType = "100% Upfront";
                                //pobjRecordHelper.setField(0, Convert.ToInt32(OrderConfirmation.OEBookType), "100% Upfront");
                            }
                            else if (BookTypeFlag == "1")
                            {
                                omodel.OrderType = "0%";
                                //pobjRecordHelper.setField(0, Convert.ToInt32(OrderConfirmation.OEBookType), "0%");
                            }
                        }

                        omodel.TriggerPrice = Convert.ToInt64(pobjRecordHelper.getField(0, Convert.ToInt32(OrderConfirmation.OETriggerPrice)));
                        //if (omodel.TriggerPrice > 0)
                        //{
                        //    if (MarketId == BowConstants.MKT_CURRENCY_VALUE.ToString())
                        //    {
                        //        pobjRecordHelper.setField(0, Convert.ToInt32(OrderConfirmation.OETriggerPrice), String.Format("{0}", (omodel.TriggerPrice / 10000), 4));
                        //    }
                        //    else
                        //    {
                        //        pobjRecordHelper.setField(0, Convert.ToInt32(OrderConfirmation.OETriggerPrice), String.Format("{0}", (omodel.TriggerPrice / 100), 2));
                        //    }
                        //}
                        //else
                        //{
                        //    pobjRecordHelper.setField(0, Convert.ToInt32(OrderConfirmation.OETriggerPrice), "0.00");
                        //}
                        ProClient = pobjRecordHelper.getField(0, Convert.ToInt32(OrderConfirmation.OEProClientIndicator)).Trim();
                        if (ProClient == "1")
                        {
                            omodel.ProClientIndicator = false;
                            //pobjRecordHelper.setField(0, Convert.ToInt32(OrderConfirmation.OEProClientIndicator), "Cli");
                        }
                        else
                        {
                            omodel.ProClientIndicator = true;
                            //pobjRecordHelper.setField(0, Convert.ToInt32(OrderConfirmation.OEProClientIndicator), "Pro");
                        }
                        omodel.StrikePrice = pobjRecordHelper.getField(0, Convert.ToInt32(Convert.ToInt32(OrderConfirmation.Size + 5))).Trim();
                        if ((omodel.StrikePrice != null) && int.TryParse(omodel.StrikePrice, out Strike_Price) == true && Convert.ToInt32(omodel.StrikePrice) == -1)
                        {
                            omodel.StrikePrice = "0.00";
                            // pobjRecordHelper.setField(0, Convert.ToInt32(Convert.ToInt32(OrderConfirmation.Size + 5)), "0.00");
                        }
                        if ((IntraDayFlag != null) && IntraDayFlag.Trim().Length > 0)
                        {
                            if (IntraDayFlag.Trim().ToUpper() == "N")
                            {
                                omodel.Delivery = false;
                                //pobjRecordHelper.setField(0, Convert.ToInt32(Convert.ToInt32(OrderConfirmation.Size)) + 17, "Intra");
                            }
                            else
                            {
                                omodel.Delivery = true;
                                //pobjRecordHelper.setField(0, Convert.ToInt32(Convert.ToInt32(OrderConfirmation.Size)) + 17, "CNC");
                            }
                        }
                        SegmentID = pobjRecordHelper.getField(0, Convert.ToInt32(Convert.ToInt32(OrderConfirmation.Size + 18)));
                        if ((SegmentID != null) && SegmentID.Trim().Length > 0)
                        {
                            omodel.Segment = Enum.GetName(typeof(ScripSegment), Convert.ToInt32(SegmentID));
                            //pobjRecordHelper.setField(0, Convert.ToInt32(OrderConfirmation.Size) + 18, Enum.GetName(typeof(Exchanges), MarketId));
                        }


                        if ((omodel.TransactionCode == StockConstants.ORDER_ENTRY_CONFIRMATION.ToString() || omodel.TransactionCode == StockConstants.ORDER_MODIFICATION_CONFIRMATION.ToString() || omodel.TransactionCode == StockConstants.ORDER_CANCELLATION_CONFIRMATION.ToString() || omodel.TransactionCode == StockConstants.ORDER_PRICE_CONFIRMATION.ToString()))
                        {

                            if (omodel.TransactionCode == StockConstants.ORDER_ENTRY_CONFIRMATION.ToString()
                                || omodel.TransactionCode == StockConstants.ORDER_MODIFICATION_CONFIRMATION.ToString()
                                || omodel.TransactionCode == StockConstants.ORDER_PRICE_CONFIRMATION.ToString())
                            {
                                omodel.InternalOrderStatus = Enumerations.OrderExecutionStatus.Exits.ToString();
                                OrderProcessor.AddInOrderMemory(Convert.ToInt64(omodel.OrderRemarks), omodel);
                                // GetOrderList(MarketId, omodel.OrderNumber, lstrUserId, lstrExchangeId, lstrToken, "", omodel.TransactionCode, pobjRecordHelper);
                                // RefreshBooksInMaster("O", lstrUserId, lstrToken, lstrExchangeId, lstrMarketId, lstrOrderNumber, "", omodel.TransactionCode, pobjRecordHelper);
                                // if (gblnAutoQuoteEntry == true && omodel.TransactionCode == Constants.StockConstants.ORDER_ENTRY_CONFIRMATION)
                                // {
                                // GetStrategyList(pobjRecordHelper);
                                //  }

                            }
                            else if (omodel.TransactionCode == StockConstants.ORDER_CANCELLATION_CONFIRMATION.ToString())
                            {
                                omodel.InternalOrderStatus = Enumerations.OrderExecutionStatus.Deleted.ToString();
                                OrderProcessor.AddInOrderMemory(Convert.ToInt64(omodel.OrderRemarks), omodel);
                                ////: As order is removed from the list based on th orderId
                                //pobjRecordHelper.setField(0, Convert.ToInt32(OrderConfirmation.OEID), "");
                                //GetOrderList(lstrMarketId, lstrOrderNumber, lstrUserId, lstrExchangeId, lstrToken, "", omodel.TransactionCode, pobjRecordHelper);
                                //RefreshBooksInMaster("O", lstrUserId, lstrToken, lstrExchangeId, lstrMarketId, lstrOrderNumber, "", omodel.TransactionCode, pobjRecordHelper);
                                //// : Logic for remove cancel order that already cancel in exchange but comes in servlet response
                                //if (!string.IsNullOrEmpty(lstrOERemarks))
                                //{
                                //    if ((glstRemarksColl != null))
                                //    {
                                //        if (!glstRemarksColl.Contains(lstrOERemarks))
                                //        {
                                //            glstRemarksColl.Add(lstrOERemarks);
                                //        }
                                //    }
                                //    else
                                //    {
                                //        glstRemarksColl = new List<string>();
                                //        glstRemarksColl.Add(lstrOERemarks);
                                //    }
                                //}

                                //TODO: Show cancelled order accordingly
                            }
                            //if (Is_ExcelOpen() == true && gblnOpenExcelOrderBook == true)
                            //{
                            //    if (omodel.TransactionCode == Constants.StockConstants.ORDER_ENTRY_CONFIRMATION || omodel.TransactionCode == Constants.StockConstants.ORDER_MODIFICATION_CONFIRMATION || omodel.TransactionCode == Constants.StockConstants.ORDER_PRICE_CONFIRMATION)
                            //    {
                            //        if ((gobjExcelOrderColl != null))
                            //        {
                            //            if (gobjExcelOrderColl.ContainsKey(lstrOrderNumber))
                            //            {
                            //                gobjExcelOrderColl.Item(lstrOrderNumber) = lstrOEID;
                            //            }
                            //            else
                            //            {
                            //                gobjExcelOrderColl.Add(lstrOrderNumber, lstrOEID);
                            //            }
                            //        }
                            //    }
                            //    else if (omodel.TransactionCode == Constants.StockConstants.ORDER_CANCELLATION_CONFIRMATION)
                            //    {
                            //        gobjExcelOrderColl.Remove(lstrOrderNumber);
                            //    }
                            //    SocketConnection.MessageArrivedEventArgs lobjMessageEvent = new SocketConnection.MessageArrivedEventArgs(pobjRecordHelper.GetActualData().ToString());
                            //    if ((gobjSync_ExcelAndChartMsgQueue != null))
                            //    {
                            //        lblnISSendExcel = true;
                            //        gobjSync_ExcelAndChartMsgQueue.Enqueue(lobjMessageEvent);
                            //    }
                            //}
                            //else
                            //{
                            //    gobjExcelOrderColl.Clear();
                            //}
                        }
                        //End If

                        if (omodel.TransactionCode != StockConstants.ORDER_ENTRY_REQUEST.ToString() && omodel.TransactionCode != StockConstants.ORDER_MODIFICATION_REQUEST.ToString()
                            && omodel.TransactionCode != StockConstants.ORDER_CANCELLATION_REQUEST.ToString() && omodel.TransactionCode != StockConstants.ORDER_MIRAGE.ToString())
                        {
                            //RefreshActivityLog(pobjRecordHelper, omodel.TransactionCode);
                        }

                    }

                    else if (MessageIdentifier == MessageIdentifiers.MESSAGE_TRADE_STREAM)
                    {
                        //Send Trade Related Messages to TradeBow Controller
                        //TradeBowController.ProcessBowTrade(pobjRecordHelper);
                    }
                }
            }

            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
            }

            return omodel;

        }


        public static int GetSegmentIDBasedonInstrument(string pstrInstrumentType, string pstrIntrDelvFlag = "N")
        {
            //:/*Start Changes for Getting EMS from SERVER */
            int lstrValue = 0;
            try
            {
                if ((pstrInstrumentType != null))
                {
                    if (pstrInstrumentType.Trim().Length == 0)
                    {
                        //: Since InstrumentType is Blank hence the Segment Ought to be an Equity Segment
                        lstrValue = 1;
                    }
                    else if ((pstrInstrumentType.Trim().ToUpper() == BowConstants.INSTRUMENT_TYPE_FUTURE_INDEX.ToUpper()) || (pstrInstrumentType.Trim().ToUpper() == BowConstants.INSTRUMENT_TYPE_FUTURE_STOCK.ToUpper()) || (pstrInstrumentType.Trim().ToUpper() == BowConstants.INSTRUMENT_TYPE_FUTURE_INT.ToUpper()) || (pstrInstrumentType.Trim().ToUpper() == BowConstants.INSTRUMENT_TYPE_FUTURE_CURRENCY.ToUpper()) || (pstrInstrumentType.Trim().ToUpper() == BowConstants.INSTRUMENT_TYPE_FUTURE_IRD.ToUpper()) || (pstrInstrumentType.Trim().ToUpper() == BowConstants.INSTRUMENT_TYPE_FUTURE_IRT.ToUpper()))
                    {
                        lstrValue = Convert.ToInt32(BowConstants.SGT_FUTURES_VALUE);

                    }
                    else if ((pstrInstrumentType.Trim().ToUpper() == BowConstants.INSTRUMENT_TYPE_SLB_FUTURE.ToUpper()))
                    {
                        //lstrValue = 2
                        lstrValue = Convert.ToInt32(BowConstants.SGT_FUTURES_VALUE);
                    }
                    else if ((pstrInstrumentType.Trim().ToUpper() == BowConstants.INSTRUMENT_TYPE_OPTION_INDEX.ToUpper()) || (pstrInstrumentType.Trim().ToUpper() == BowConstants.INSTRUMENT_TYPE_OPTION_STOCK.ToUpper()) || (pstrInstrumentType.Trim().ToUpper() == BowConstants.INSTRUMENT_TYPE_OPTION_CURRENCY) || (pstrInstrumentType.Trim().ToUpper() == "OPTINT".ToUpper()))
                    {
                        //lstrValue = 3
                        lstrValue = Convert.ToInt32(BowConstants.SGT_OPTIONS_VALUE);
                    }
                    else if ((pstrInstrumentType.Trim().ToUpper() == BowConstants.INSTRUMENT_TYPE_COMMODITY_CASH.ToUpper() || pstrInstrumentType.Trim().ToUpper() == "COM"))
                    {
                        lstrValue = Convert.ToInt32(BowConstants.SGT_COMM_CASH_VALUE);
                    }
                    else if ((pstrInstrumentType.Trim().ToUpper() == BowConstants.INSTRUMENT_TYPE_COMMODITY_FUTURE.ToUpper()))
                    {
                        lstrValue = Convert.ToInt32(BowConstants.SGT_COMM_FUTURES_VALUE);
                    }
                    else if ((pstrInstrumentType.Trim().ToUpper() == BowConstants.INSTRUMENT_TYPE_COMMODITY_OPTION.ToUpper()))
                    {
                        lstrValue = Convert.ToInt32(BowConstants.SGT_COMM_OPTIONS_VALUE);
                    }
                    else if (pstrInstrumentType.Trim().ToUpper() == BowConstants.INSTRUMENT_TYPE_WDM)
                    {
                        lstrValue = Convert.ToInt32(BowConstants.SGT_FUTURES_VALUE);
                    }
                    else if (pstrInstrumentType.Trim().ToUpper() == BowConstants.INSTRUMENT_TYPE_OFS)
                    {
                        lstrValue = Convert.ToInt32(BowConstants.SGT_EQUITY_VALUE);
                    }
                    else if (pstrInstrumentType.Trim().ToUpper() == BowConstants.INSTRUMENT_TYPE_ITP)
                    {
                        lstrValue = Convert.ToInt32(BowConstants.SGT_EQUITY_VALUE);
                    }
                    else
                    {
                        lstrValue = 0;
                    }

                    if (pstrIntrDelvFlag == "Y")
                    {
                        if (lstrValue == Convert.ToInt32(BowConstants.SGT_FUTURES_VALUE))
                        {
                            lstrValue = Convert.ToInt32(BowConstants.SGT_FUTURES_DELIVERY_VALUE);
                        }
                        else if (lstrValue == Convert.ToInt32(BowConstants.SGT_OPTIONS_VALUE))
                        {
                            lstrValue = Convert.ToInt32(BowConstants.SGT_OPTIONS_DELIVERY_VALUE);
                        }
                        else if (lstrValue == Convert.ToInt32(BowConstants.SGT_COMM_CASH_VALUE))
                        {
                            lstrValue = Convert.ToInt32(BowConstants.SGT_COMM_CASH_DELIVERY_VALUE);
                        }
                        else if (lstrValue == Convert.ToInt32(BowConstants.SGT_COMM_FUTURES_VALUE))
                        {
                            lstrValue = Convert.ToInt32(BowConstants.SGT_COMM_FUTURES_DELIVERY_VALUE);
                        }
                        else if (lstrValue == Convert.ToInt32(BowConstants.SGT_COMM_OPTIONS_VALUE))
                        {
                            lstrValue = Convert.ToInt32(BowConstants.SGT_COMM_OPTIONS_DELIVERY_VALUE);
                        }
                    }

                    return lstrValue;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error in getting Segment id based on Instrument", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            return 0;
            //:/*END Changes for Getting EMS from SERVER */
        }

        //private static bool sendOrderOverTCP(OrderModel omodel, int decimalPnt)
        //{
        //    try
        //    {
        //        //Sample
        //        //AddOrder|1468094178-01018075|OEToken=288&OENSEToken=-fr1&OEBSEToken=500410&OEDestination=1&OEMarket=1&OESymbol=ACC&OESeries=EQ&OEVolume=1&OEPrice=1004.60&OETriggerPrice=&OEDisclosedVolume=&USBackOfficeId=BROKER&OEFIELD3=N&OEBuySellIndicator=1&OEAlphaChar=50&OEActualApproverId=9&OEVolumeRemaining=&OEDisclosedVolumeRemaining=&OEAdminUSID=&OEStatus=&OEProClientIndicator=2&OEParticipantType=BROKER&OEIntuitionalClient=CLIENT&OEGoodTillDate=2&IOC=0&OEBookType=1&OESegment=1&OEGroup=A&LoginKey=1468094178-01018075&Thick Client=Y&compressed=N

        //        System.Text.StringBuilder lstrOrderMessage = new System.Text.StringBuilder();


        //        //:Header Start
        //        lstrOrderMessage.Append(mstrMsgCode);
        //        lstrOrderMessage.Append(mstrTblName);
        //        if (omodel.Mode == Convert.ToInt32(Modes.Edit))
        //        {
        //            lstrOrderMessage.Append(mstrEditOrdercd);
        //        }
        //        else
        //        {
        //            lstrOrderMessage.Append(mstrAddOrdercd);
        //        }
        //        lstrOrderMessage.Append(mstrDirection);
        //        lstrOrderMessage.Append(appendBlankSpaces(omodel.Token.ToString(), 10));
        //        lstrOrderMessage.Append(mstrISINNo);
        //        lstrOrderMessage.Append(appendBlankSpaces(omodel.ScripName.Trim(), 10));
        //        lstrOrderMessage.Append(appendBlankSpaces(omodel.Symbol.Trim(), 20));
        //        lstrOrderMessage.Append(omodel.Series.Trim());
        //        string lstrLongExpDt = null;
        //        string lstrAcualStkPrc = null;
        //        lstrLongExpDt = "";
        //        lstrOrderMessage.Append(appendBlankSpaces(lstrLongExpDt, 10));
        //        lstrAcualStkPrc = "";

        //        lstrOrderMessage.Append(appendBlankSpaces(lstrAcualStkPrc, 20));
        //        lstrOrderMessage.Append(appendBlankSpaces("", 10));
        //        lstrOrderMessage.Append(mstrMsgType);
        //        lstrOrderMessage.Append((int)Enum.Parse(typeof(ScripSegment),omodel.Segment));
        //        lstrOrderMessage.Append("1");
        //        lstrOrderMessage.Append(mstrFiller);
        //        int ExchangeID = (int)Enum.Parse(typeof(Exchanges), omodel.Exchange);
        //        if (omodel.Mode == Convert.ToInt32(Modes.Add))
        //        {
        //            lstrOrderMessage.Append(ExchangeID.ToString());
        //        }
        //        else
        //        {
        //            lstrOrderMessage.Append(Source.Trim);
        //        }
        //        lstrOrderMessage.Append(ExchangeID.ToString());
        //        if (ExchangeID == BowConstants.EX_BSE_VALUE)
        //        {
        //            lstrOrderMessage.Append(appendBlankSpaces("-1", 10));
        //            lstrOrderMessage.Append(appendBlankSpaces(omodel.ScripCode.ToString(), 10));
        //        }
        //        else
        //        {
        //            lstrOrderMessage.Append(appendBlankSpaces(omodel.ScripCode.ToString(), 10));
        //            lstrOrderMessage.Append(appendBlankSpaces("-1", 10));
        //        }
        //        lstrOrderMessage.Append(mstrMsgOrgTime);
        //        lstrOrderMessage.Append(mstrPurpose);
        //        //: Header End

        //        //:Values
        //        if (omodel.Mode == Convert.ToInt32(Modes.Edit))
        //        {
        //            lstrOrderMessage.Append(omodel.OrderId + "|");
        //            //id
        //        }
        //        else
        //        {
        //            lstrOrderMessage.Append("|");
        //            //id
        //        }

        //        lstrOrderMessage.Append("0|");
        //        //timestamp
        //        lstrOrderMessage.Append("0|");
        //        //logtime
        //        lstrOrderMessage.Append("|");
        //        //alphachar
        //        //Transaction Cd
        //        if (omodel.Mode == Convert.ToInt32(Modes.Add))
        //        {
        //            lstrOrderMessage.Append("2000|");
        //        }
        //        else
        //        {
        //            lstrOrderMessage.Append("2040|");
        //        }
        //        lstrOrderMessage.Append("0|0|0|");
        //        lstrOrderMessage.Append(omodel.UserId + "|");
        //        //participant type
        //        if (ExchangeID == BowConstants.EX_MCX_VALUE || ExchangeID == BowConstants.EX_DGCX_VALUE)
        //        {
        //            lstrOrderMessage.Append(omodel.ParticipantCode + "|");
        //        }
        //        else
        //        {
        //            lstrOrderMessage.Append("S|");
        //        }

        //        //submit stat
        //        lstrOrderMessage.Append("1|");
        //        if (omodel.Mode == Convert.ToInt32(Modes.Add))
        //        {
        //            lstrOrderMessage.Append("0|");
        //        }
        //        else
        //        {
        //            lstrOrderMessage.Append(omodel.OrderNumber + "|");
        //        }
        //        lstrOrderMessage.Append(frmFastOrderEntryPanel.GetOrderTypeIdContract(omodel.OrderType) + "|");
        //        if (omodel.BuySellIndicator.ToUpper() == BuySellFlag.BUY.ToString())
        //        {
        //            lstrOrderMessage.Append(BowConstants.BUY_VALUE + "|");
        //        }
        //        else
        //        {
        //            lstrOrderMessage.Append(BowConstants.SELL_VALUE + "|");
        //        }
        //        lstrOrderMessage.Append(omodel.Quantity + "|");
        //        lstrOrderMessage.Append(omodel.VolumeRemaining + "|");
        //        //:dis qty & remaiing DQ
        //        lstrOrderMessage.Append(omodel.RevealQty + "|0|");
        //        //:min Vol & vol filled 2day
        //        lstrOrderMessage.Append("0|0|");
        //        lstrOrderMessage.Append(omodel.Price + "|");
        //        if (omodel.TriggerPrice == 0)
        //        {
        //            lstrOrderMessage.Append("0|");
        //        }
        //        else
        //        {
        //            lstrOrderMessage.Append(omodel.TriggerPrice + "|");
        //        }
        //        //:flag
        //        lstrOrderMessage.Append("0|");
        //        //:broker,trader,branch id
        //        lstrOrderMessage.Append("0|0|0|");

        //        if (omodel.Mode == Convert.ToInt32(Modes.Edit))
        //        {
        //            lstrOrderMessage.Append(omodel.OrderRemarks + "|");
        //        }
        //        else
        //        {
        //            lstrOrderMessage.Append("0|");
        //        }

        //        //:end dt time & lst Modify tm
        //        lstrOrderMessage.Append("0|0|");
        //        if (omodel.ProClientIndicator == true)
        //        {
        //            lstrOrderMessage.Append("BROKER");
        //        }
        //        else
        //        {
        //            lstrOrderMessage.Append(omodel.BackOfficeId);
        //        }
        //        //:CompetitorPeriod 
        //        lstrOrderMessage.Append("|0|");
        //        if (ExchangeID == BowConstants.EX_BSE_VALUE && MarketID == BowConstants.MKT_EQUITY_VALUE && (this.Price.Trim.Length == 0 || Convert.ToDecimal(this.Price) == 0) && this.OrderType == BowConstants.ORDER_TYPE_RL)
        //        {
        //            //: SolicitorPeriod
        //            if (string.IsNullOrWhiteSpace(omodel.TriggerPrice) == false)
        //            {
        //                lstrOrderMessage.Append(TriggerPrice * 100000000);
        //            }
        //            else if (this.SecurityInfo.IsDefaultOrdSettings == true && Information.IsNumeric(this.SecurityInfo.defaultProtPer) && this.SecurityInfo.defaultProtPer > 0)
        //            {
        //                lstrOrderMessage.Append(this.SecurityInfo.defaultProtPer * 100000000);
        //            }
        //            else if (glngDefaultProtectionPercentage > 0)
        //            {
        //                lstrOrderMessage.Append(glngDefaultProtectionPercentage * 100000000);
        //            }
        //            else
        //            {
        //                lstrOrderMessage.Append("");
        //            }
        //        }
        //        else
        //        {
        //            lstrOrderMessage.Append("");
        //        }
        //        // :modified cancelledby
        //        lstrOrderMessage.Append("|T|");
        //        //:reason cd , auction no , counterpartybroker,suspended secuirty
        //        lstrOrderMessage.Append("0|0|||");
        //        //: for NSE GTD = 0 and for BSE GTD=1
        //        //lstrOrderMessage.Append("" & "|") REM : Currently sending it as blank for GFD.
        //        if (GetExchangeId(omodel.Exchange) == BowConstants.EX_NSE_VALUE)
        //        {
        //            lstrOrderMessage.Append("0" + "|");
        //        }
        //        else
        //        {
        //            lstrOrderMessage.Append(CashOrderEntry.GetRetentionTypeId(this.OrderRetentionStatus.ToUpper) + "|");
        //        }
        //        if (Mode == MODES.Add)
        //        {
        //            lstrOrderMessage.Append("0|");
        //        }
        //        else
        //        {
        //            lstrOrderMessage.Append("" + "|");
        //            //lstrOrderMessage.Append(Settlor & "|")
        //        }
        //        if (ProClientIndicator == true)
        //        {
        //            lstrOrderMessage.Append("2|");
        //        }
        //        else
        //        {
        //            lstrOrderMessage.Append("1|");
        //        }
        //        // :Settlement period
        //        lstrOrderMessage.Append("0|");
        //        // :Calevel            
        //        if (InstitutionalClient == BowConstants.INSTITUTION)
        //        {
        //            lstrOrderMessage.Append("90|");
        //        }
        //        else if (InstitutionalClient == BowConstants.SPLCLI)
        //        {
        //            lstrOrderMessage.Append("40|");
        //        }
        //        else
        //        {
        //            lstrOrderMessage.Append("30|");
        //        }

        //        // :openclose,coveruncover,giveupflag,purpose
        //        lstrOrderMessage.Append("|||0|");
        //        lstrOrderMessage.Append(AdminUsId + "|");
        //        if (omodel.Mode == Convert.ToInt32(Modes.Edit))
        //        {
        //            lstrOrderMessage.Append("|");
        //        }
        //        else
        //        {
        //            lstrOrderMessage.Append(omodel.OrderRemarks + "^|");
        //        }

        //        //:Status,ExpectedApproverId,ActualApproverId,ApproverRemarks,OldVolume,OldPrice
        //        lstrOrderMessage.Append("0|0|||0|0|");
        //        //:CreatedBy,CreatedAt,LastUpdatedBy,LastUpdatedAt
        //        lstrOrderMessage.Append(gobjUtilityLoginDetails.UserId + "|" + DateTime.Now + "|||");
        //        //:Field1
        //        lstrOrderMessage.Append(frmFastOrderEntryPanel.GetOrderTypeIdContract(OrderType) + "|");
        //        //:Field2,
        //        lstrOrderMessage.Append("|");
        //        //:Field3 for intraday or delv
        //        if (omodel.Delivery == true)
        //        {
        //            lstrOrderMessage.Append("Y|");
        //        }
        //        else
        //        {
        //            lstrOrderMessage.Append("N|");
        //        }
        //        //:Field4,
        //        lstrOrderMessage.Append("|");
        //        //REM:Field2,Field3,Field4
        //        //lstrOrderMessage.Append("|||")

        //        if (omodel.Mode == Convert.ToInt32(Modes.Edit))
        //        {
        //            lstrOrderMessage.Append(RowState + "|");
        //        }
        //        else
        //        {
        //            lstrOrderMessage.Append("0|");
        //        }

        //        //: IOC
        //        string lstrIOC = "0";
        //        if (omodel.OrderType.Trim().ToUpper() == BowServletsConstants.PARAMORDERIOC)
        //        {
        //            lstrIOC = "1";
        //        }
        //        if (ExchangeID == BowConstants.EX_DGCX_VALUE)
        //        {
        //            if (omodel.OrderType.Trim().ToUpper() == BowServletsConstants.PARAMORDERIOC)
        //            {
        //                lstrIOC = "0";
        //            }
        //            else
        //            {
        //                lstrIOC = "1";
        //            }
        //        }
        //        else if (ExchangeID == BowConstants.EX_BSE_VALUE && ExchangeID != BowConstants.MKT_CURRENCY_VALUE)
        //        {
        //            if (omodel.ImmediateOrCancel == true)
        //            {
        //                lstrIOC = "1";
        //            }
        //            else
        //            {
        //                lstrIOC = "0";
        //            }
        //        }
        //        lstrOrderMessage.Append(lstrIOC + "|");
        //        // : AutionIDForTCP
        //        lstrOrderMessage.Append("|");
        //        // : BlockDealForTCP
        //        if (this.BlockDeal == true)
        //        {
        //            lstrOrderMessage.Append("Y|");
        //        }
        //        else
        //        {
        //            lstrOrderMessage.Append("N|");
        //        }
        //        //: Good Till Cancel
        //        lstrOrderMessage.Append("0" + "|");
        //        lstrOrderMessage.Append(LTP + "|");
        //        lstrOrderMessage.Append(gobjUtilityLoginDetails.LoginKeyValue);


        //        if ((gobjInteractiveSocket != null) && gblnOrdOverTCP == true && gobjInteractiveSocket.ConnectedToServer)
        //        {
        //            gobjInteractiveSocket.send(lstrOrderMessage.ToString, true);
        //            lstrOrderMessage.Length = 0;
        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}

        public static string appendBlankSpaces(string pstrValue, int pintLenReq)
        {
            if (pstrValue.Length < pintLenReq)
            {
                while (pstrValue.Length < pintLenReq)
                {
                    pstrValue = pstrValue + " ";
                }
                return pstrValue;
            }
            else
            {
                return pstrValue.Substring(0, pintLenReq);
            }
        }

        #endregion
#endif
    }
}
