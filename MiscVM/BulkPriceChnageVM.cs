using CommonFrontEnd.Common;
using CommonFrontEnd.Model.Order;
using CommonFrontEnd.Processor.Order;
using CommonFrontEnd.SharedMemories;
using CommonFrontEnd.View;
using CommonFrontEnd.ViewModel.Order;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace CommonFrontEnd.ViewModel
{
    public class BulkPriceChnageVM : BaseViewModel
    {
        #region SingleInstance

        private static BulkPriceChnageVM mobjInstance;
        public static BulkPriceChnageVM GETInstance
        {
            get
            {
                if (mobjInstance == null)
                {
                    mobjInstance = new BulkPriceChnageVM();
                }
                return mobjInstance;
            }
        }

        #endregion
        #region Properties
        static View.BulkPriceChnage mWindow = null;
        Thread DeleteThread = null;
        string Validate_Message = string.Empty;
        public static AutoResetEvent BulkChnageOrderEvent = new AutoResetEvent(false);
        private string _txtNewPrice;

        public string txtNewPrice
        {
            get { return _txtNewPrice; }
            set { _txtNewPrice = value; NotifyPropertyChanged(nameof(txtNewPrice)); }
        }
        private string _OCOTrgRatetxt;

        public string OCOTrgRatetxt
        {
            get { return _OCOTrgRatetxt; }
            set { _OCOTrgRatetxt = value; NotifyPropertyChanged(nameof(OCOTrgRatetxt)); }
        }


        private string _txtReply;

        public string txtReply
        {
            get { return _txtReply; }
            set { _txtReply = value; NotifyPropertyChanged(nameof(txtReply)); }
        }

        private string _titleBulkPrice;

        public string titleBulkPrice
        {
            get { return _titleBulkPrice; }
            set { _titleBulkPrice = value; NotifyPropertyChanged(nameof(titleBulkPrice)); }
        }


        #endregion

        #region RelayCommond

        private RelayCommand _btnSubmit;

        public RelayCommand btnSubmit
        {
            get
            {
                return _btnSubmit ?? (_btnSubmit = new RelayCommand(
                    (object e) => btnSubmitClick(e)
                        ));
            }
        }

        private RelayCommand _btnRemove;

        public RelayCommand btnRemove
        {
            get
            {
                return _btnRemove ?? (_btnRemove = new RelayCommand(
                    (object e) => btnRemoveClick(e)
                        ));
            }
        }

        

        private RelayCommand _BulkChangeWindowClosing;

        public RelayCommand BulkChangeWindowClosing
        {
            get
            {
                return _BulkChangeWindowClosing ?? (_BulkChangeWindowClosing = new RelayCommand(
                    (object e) => CloseWindow(e)
                        ));
            }
        }




        #endregion

        #region EventsAndDelagates
        public Action<string> TogglePersonalWindow;
        #endregion

        #region Memories


        private ObservableCollection<Model.Order.PendingOrder> _selecteBulkScripList;

        public ObservableCollection<Model.Order.PendingOrder> selecteBulkScripList
        {
            get { return _selecteBulkScripList; }
            set
            {
                _selecteBulkScripList = value; NotifyPropertyChanged(nameof(selecteBulkScripList));
            }
        }

        private List<Model.Order.PendingOrder> _SelectItemList = new List<Model.Order.PendingOrder>();

        public List<Model.Order.PendingOrder> SelectItemList
        {
            get { return _SelectItemList; }
            set
            {
                _SelectItemList = value;
                NotifyPropertyChanged(nameof(SelectItemList));
            }
        }

       


        #endregion

        #region constructor
        public BulkPriceChnageVM()
        {
            selecteBulkScripList = new ObservableCollection<Model.Order.PendingOrder>();
            populateGrid();
            mWindow = System.Windows.Application.Current.Windows.OfType<View.BulkPriceChnage>().FirstOrDefault();
            OrderProcessor.OnOrderResponse += ReadOrderResponse;
            OrderProcessor.OnOrderResponseReceived += UpdateBulkMemory;
            //DisplayBulkScripList();
        }



        #endregion

        #region Methods
        public void populateGrid()
        {
            selecteBulkScripList = new ObservableCollection<PendingOrder>();
            foreach (var item in PendingOrderClassicVM.GETInstance.SelectedValue)
            {
                insertData(item);
            }
        }
        public void CloseWindow(object e)
        {
            BulkPriceChnage oBulkPriceChnage = System.Windows.Application.Current.Windows.OfType<BulkPriceChnage>().FirstOrDefault();
            if (oBulkPriceChnage != null)
            {
               // oBulkPriceChnage.Hide();
                ViewModel.Order.PendingOrderClassicVM.bulkChangeWindowOpen = false;
                ViewModel.Order.PendingOrderClassicVM.GETInstance.ShowPendingWindowButtons();
                oBulkPriceChnage?.Close();
            }
           
            //BulkPriceChnage oBulkPriceChnage = System.Windows.Application.Current.Windows.OfType<BulkPriceChnage>().FirstOrDefault();
            //if (oBulkPriceChnage != null)
            //{
            //    oBulkPriceChnage.Hide();
            //    PendingOrderClassicVM.bulkChangeWindowOpen = false;
            //    PendingOrderClassicVM.GETInstance.ShowPendingWindowButtons();
            //}
        }
        private void btnSubmitClick(object e)
        {
            try
            {
                DeleteThread = new Thread(SubmitOrder);
                DeleteThread.Start();

            }
            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
            }

        }

        private void btnRemoveClick(object e)
        {
            try
            {
                if (SelectItemList != null && SelectItemList.Count != 0)
                {
                    List<PendingOrder> tempselectedOrderRow = new List<PendingOrder>();

                    foreach (PendingOrder item in System.Windows.Application.Current.Windows.OfType<BulkPriceChnage>().FirstOrDefault().dataGrid.SelectedItems)
                    {
                        tempselectedOrderRow.Add(item);
                    }
                    //tempselectedIndicesRow = SelectedIndicesRow;
                        foreach (PendingOrder item in tempselectedOrderRow)
                        {
                            selecteBulkScripList.Remove(item);
                        if(selecteBulkScripList.Count == 0)
                        {
                            CloseWindow(null);
                        }
                        titleBulkPrice = string.Format("Bulk Change : Count {0}", selecteBulkScripList.Count);
                    }
                    
                }
                else { MessageBox.Show("Please Select the Index to Remove", "Remove Index", MessageBoxButton.OK, MessageBoxImage.Warning); }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
            }

        }
           
        private void SubmitOrder()
        {
            int count = selecteBulkScripList.Count;
            try
            {
                //if (SelectedValue != null)
                for (int index = 0; index < count; index++)
                {
                    PendingOrder oPendingOrder = new PendingOrder();
                    oPendingOrder = (PendingOrder)selecteBulkScripList[index];
                    //bool validate = ValidateUserInputs(oPendingOrder.SCode, oPendingOrder.TotalQty, oPendingOrder.RevQty, oPendingOrder.Rate, oPendingOrder.ClientID, oPendingOrder.ProtectionPercent);
                    //if (!validate)
                    //{
                    //    return;
                    //}
                    //foreach (var item in SelectedValue)
                    //{
#if BOW
                    //OrderProcessor.GetOrderDataFromOrderID(SelectedValue.FirstOrDefault());                  
                    OrderProcessor.GetOrderDataFromOrderID(SelectedValue.FirstOrDefault());
#elif TWS
                    //OrderProcessor.GetOrderDataByMessageTag(SelectedValue.FirstOrDefault());
                    OrderRequestProcessor oOrderRequestProcessor = new OrderRequestProcessor(new ModifyOrder());
                    OrderModel oOrderModel;
                    string key = Convert.ToString(oPendingOrder.SCode) + "_" + oPendingOrder.OrdID;
                    MemoryManager.OrderDictionary.TryGetValue(key, out oOrderModel);


                    if (oOrderModel != null)
                    {
                        OrderModel oTempOrderModel = new OrderModel();
                        var DecimalPoint = CommonFunctions.GetDecimal(System.Convert.ToInt32(oOrderModel.ScripCode), "BSE", CommonFunctions.GetSegmentID(oOrderModel.ScripCode));
                        bool validate = true;
                        if (string.IsNullOrEmpty(txtNewPrice) && string.IsNullOrEmpty(OCOTrgRatetxt))
                        {
                            txtReply = "Enter Either of the rates";
                            return;
                        }

                        if (!string.IsNullOrEmpty(txtNewPrice))
                        {
                            if (!string.IsNullOrEmpty(OCOTrgRatetxt))
                            {
                                validate = ValidateUserInputs(oPendingOrder.SCode, oPendingOrder.TotalQty, oPendingOrder.RevQty, txtNewPrice, oPendingOrder.ClientID, oPendingOrder.ProtectionPercent, OCOTrgRatetxt, oOrderModel, ref Validate_Message);
                            }
                            else
                            {
                                validate = ValidateUserInputs(oPendingOrder.SCode, oPendingOrder.TotalQty, oPendingOrder.RevQty, txtNewPrice, oPendingOrder.ClientID, oPendingOrder.ProtectionPercent, oPendingOrder.OCOTrgRate, oOrderModel, ref Validate_Message);
                            }

                            //validate = ValidateUserInputs(oPendingOrder.SCode, oPendingOrder.TotalQty, oPendingOrder.RevQty, txtNewPrice, oPendingOrder.ClientID, oPendingOrder.ProtectionPercent, OCOTrgRatetxt, oOrderModel, ref Validate_Message);
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(OCOTrgRatetxt))
                            {
                                validate = ValidateUserInputs(oPendingOrder.SCode, oPendingOrder.TotalQty, oPendingOrder.RevQty, oPendingOrder.Rate, oPendingOrder.ClientID, oPendingOrder.ProtectionPercent, OCOTrgRatetxt, oOrderModel, ref Validate_Message);
                            }
                            else
                            {
                                validate = ValidateUserInputs(oPendingOrder.SCode, oPendingOrder.TotalQty, oPendingOrder.RevQty, oPendingOrder.Rate, oPendingOrder.ClientID, oPendingOrder.ProtectionPercent, oPendingOrder.OCOTrgRate, oOrderModel, ref Validate_Message);
                            }
                            //validate = ValidateUserInputs(oPendingOrder.SCode, oPendingOrder.TotalQty, oPendingOrder.RevQty, oPendingOrder.Rate, oPendingOrder.ClientID, oPendingOrder.ProtectionPercent, OCOTrgRatetxt, oOrderModel, ref Validate_Message);
                        }

                      

                        if (!validate)
                        {
                            txtReply = Validate_Message;
                            return;
                        }

                        oTempOrderModel.OrderAction = "U";
                        oTempOrderModel.OriginalQty = oOrderModel.OriginalQty;
                        oTempOrderModel.RevealQty = oOrderModel.RevealQty;
                        oTempOrderModel.OrderType = oOrderModel.OrderType;
                        //Convert Price in Long and send to processing
                        if (oTempOrderModel.OrderType == "G")//oTempOrderModel.OrderType == "STOPLOSSMKT")
                        {
                            oTempOrderModel.Price = 0;
                        }
                        else//for limit
                        {
                            if (!string.IsNullOrEmpty(txtNewPrice))
                                oTempOrderModel.Price = Convert.ToInt64(Convert.ToDouble(txtNewPrice) * Math.Pow(10, DecimalPoint));//Convert.ToInt32(Ratetxt);
                            else
                                oTempOrderModel.Price = Convert.ToInt64(Convert.ToDouble(oPendingOrder.Rate) * Math.Pow(10, DecimalPoint));

                            //oTempOrderModel.Price = Convert.ToInt64(Convert.ToDouble(txtNewPrice) * Math.Pow(10, DecimalPoint));
                        }
                        oTempOrderModel.Exchange = Enumerations.Order.Exchanges.BSE.ToString();
                        oTempOrderModel.ScreenId = (int)Enumerations.WindowName.Pending_Order;
                        oTempOrderModel.OrderRetentionStatus = oOrderModel.OrderRetentionStatus;
                        oTempOrderModel.ClientId = oOrderModel.ClientId;
                        oTempOrderModel.OrderType = oOrderModel.OrderType;
                        if (oTempOrderModel.OrderType == "G")
                        {
                            oTempOrderModel.ProtectionPercentage = Convert.ToString(Convert.ToInt32(Convert.ToDecimal(oOrderModel.ProtectionPercentage) * 100));
                        }
                        else
                        {
                            oTempOrderModel.ProtectionPercentage = "0";
                        }
                        oTempOrderModel.OrderId = oOrderModel.OrderId;
                        oTempOrderModel.BuySellIndicator = oOrderModel.BuySellIndicator;
                        oTempOrderModel.OrderKey = key;
                        oTempOrderModel.ScripCode = oOrderModel.ScripCode;
                        oTempOrderModel.Segment = CommonFunctions.GetSegmentID(oOrderModel.ScripCode);
                        oTempOrderModel.SegmentFlag = CommonFunctions.SegmentFlag(oTempOrderModel.Segment); //(int)Enum.Parse(typeof(Enumerations.Order.ScripSegment), oTempOrderModel.Segment);
                        oTempOrderModel.ClientType = oOrderModel.ClientType;
                        oTempOrderModel.ParticipantCode = oOrderModel.ParticipantCode;
                        oTempOrderModel.Symbol = oOrderModel.Symbol;
                        if (oOrderModel.IsOCOOrder == true)
                        {
                            oTempOrderModel.IsOCOOrder = true;
                            if (!string.IsNullOrEmpty(OCOTrgRatetxt))
                                oTempOrderModel.TriggerPrice = Convert.ToInt64(Convert.ToDouble(OCOTrgRatetxt) * Math.Pow(10, DecimalPoint));
                            else
                                oTempOrderModel.TriggerPrice = Convert.ToInt64(Convert.ToDouble(oPendingOrder.OCOTrgRate) * Math.Pow(10, DecimalPoint));
                        }

                        oTempOrderModel.BuySellIndicator = oOrderModel.BuySellIndicator;
                        oTempOrderModel.MarketLot = oOrderModel.MarketLot;
                        oTempOrderModel.TickSize = oOrderModel.TickSize;
                        oTempOrderModel.Group = oOrderModel.Group;
                        oOrderRequestProcessor.ProcessRequest(oTempOrderModel);
                    }
                    else
                    {
                        //MessageBox.Show("Can't find order");
                        txtReply = "Can't find order";
                    }
                    BulkChnageOrderEvent.WaitOne();

                }
#endif

            }
            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
            }
        }

        private bool ValidateUserInputs(long ScripCode, int TotalQty, int RevQty, string Rate, string ClientID, string MktPT,string OCOTrgRatetxt,OrderModel omodel,ref string Validate_Message)
        {
            try
            {
                //Quantity Validation
                Validate_Message = Validations.ValidateVolume(TotalQty, 9999999999L, 1, 0, false);
                if (Validate_Message.Trim().ToUpper() != "Success".ToUpper())
                {
                    Validate_Message = "Quantity : " + Validate_Message;
                    return false;
                }

                //Total Qty should be Multiple of Mkt Lot
                var MarketLot = Convert.ToInt64(CommonFunctions.GetMarketLot(ScripCode));

                if (MarketLot != 0)
                {
                    if (TotalQty % MarketLot != 0)
                    {
                        Validate_Message = "Quantity should be a multiple of Mkt lot[" + MarketLot + "]";
                        return false;
                    }
                }

                else
                {
                    Validate_Message = "Market Lot is Zero";
                    return false;
                }

                //Validate Reveal Qty
                Validate_Message = Validations.ValidateRevlQty(RevQty, 9999999999L, 1, 0, false);
                if (Validate_Message.Trim().ToUpper() != "Success".ToUpper())
                {
                    Validate_Message = "RevealQty : " + Validate_Message;
                    return false;
                }

                // Reveal Qty volume should not be greater than actual volume
                if (RevQty > TotalQty)
                {
                    Validate_Message = "Disclosed quantity can't be greater than actual quantity";
                    return false;
                }

                // if Reveal Qty is zero then send actual quantity as reveal qty
                if (RevQty == 0)
                {
                    RevQty = TotalQty;
                }

                //Reveal Qty should be Greater than or Equal to 10% of Total QTY
                if (RevQty < (TotalQty * 10) / 100)
                {
                    Validate_Message = "Reveal quantity should > or =  to 10 % of total quantity";
                    return false;
                }

                //Reveal Qty should be Multiple of Mkt Lot
                if (RevQty % MarketLot != 0)
                {
                    Validate_Message = " Reveal quantity is Not a Multiple of Mkt Lot[" + MarketLot + "]";
                    return false;
                }

                //if Rate is zero
                if (Rate == "0" || string.IsNullOrEmpty(Rate))
                {
                    Validate_Message = "Rate Entered is Empty";
                    return false;
                }



                //Check Number after decimal point
                var Segment = Common.CommonFunctions.GetSegmentID(ScripCode);
                var DecimalPoint = Common.CommonFunctions.GetDecimal(Convert.ToInt32(ScripCode), "BSE", Segment);
                var rate = Convert.ToString(Rate);
                if (rate.Contains(".") && rate.Substring(rate.IndexOf(".") + 1).Length > DecimalPoint)
                {
                    Validate_Message = "Rate More than " + DecimalPoint + " decimal places!";
                    return false;
                }

                //Total Rate should be Multiple of Tick Size
                var TickSize = CommonFunctions.GetTickSize(ScripCode);
                if (!string.IsNullOrEmpty(TickSize))
                {
                    if (Convert.ToDouble(TickSize) != 0)
                    {
                        if (Convert.ToInt64(Convert.ToDouble(rate) * Math.Pow(10, DecimalPoint)) % Convert.ToInt64(Convert.ToDouble(TickSize)) != 0)
                        {
                            Validate_Message = "Rate should be a multiple of TickSize[" + TickSize + "]";
                            return false;
                        }
                    }
                    else
                    {
                        Validate_Message = "Tick size is Zero.";
                        return false;
                    }
                }

                else
                {
                    Validate_Message = "Tick size is Zero.";
                    return false;
                }

                if (!String.IsNullOrEmpty(MktPT))
                {
                    int i;
                    if (int.TryParse(MktPT, out i))

                    {
                        if (!(i >= 1 && i <= 99))
                        {
                            Validate_Message = "MProt % can not be greater than 99.00 %";

                            return false;

                        }

                    }

                    else if (!Regex.IsMatch(MktPT, @"^\d{1,2}(\.\d{1})?$"))
                    {
                        Validate_Message = "MProt % more than 1 decimal places!";
                        return false;
                    }
                }


                if (omodel.OrderType.ToUpper() == "P" || (omodel.OrderType.ToUpper() == "L" && omodel.IsOCOOrder))
                {
                    if (string.IsNullOrEmpty(OCOTrgRatetxt) || OCOTrgRatetxt == "0")
                    {
                        Validate_Message = "Please enter Trg Rate";

                        return false;
                    }

                    //Check Trigger Price is Not a number
                    if (!Regex.IsMatch(OCOTrgRatetxt, @"^[1-9]\d*(\.\d+)?$"))
                    {
                        //if (rate == "0" || rate == string.Empty)
                        //    Validate_Message = "Please provide Rate";
                        Validate_Message = "Illegal Characters in Trigger Rate";

                        return false;
                    }

                    //Check Number after decimal point
                    if (OCOTrgRatetxt.Contains(".") && OCOTrgRatetxt.Substring(OCOTrgRatetxt.IndexOf(".") + 1).Length > DecimalPoint)
                    {
                        Validate_Message = "Trigger Rate More than" + DecimalPoint + "decimal places!";
                        return false;
                    }

                    long llngTriggerPrice = Convert.ToInt64(Convert.ToDouble(OCOTrgRatetxt) * Math.Pow(10, DecimalPoint));

                    if (llngTriggerPrice > 0)
                    {
                        if (Convert.ToDouble(OCOTrgRatetxt) > 0)
                        {
                            long llngPrice = Convert.ToInt64(Convert.ToDouble(rate) * Math.Pow(10, DecimalPoint));
                            if (llngPrice != 0)
                            {
                                if (omodel.BuySellIndicator.ToUpper() == Enumerations.Side.Buy.ToString().ToUpper())
                                {
                                    if ((llngTriggerPrice > llngPrice))
                                    {
                                        Validate_Message = "Stop Price should be less than the Limit Price.";
                                        //pstrError = lstrMessage;
                                        //pintPropertyId = omodel.TriggerPrice;
                                        return false;
                                    }
                                }
                                else
                                {
                                    if ((llngTriggerPrice < llngPrice))
                                    {
                                        Validate_Message = "Stop Price should be greater than the Limit Price.";
                                        //pstrError = lstrMessage;
                                        return false;
                                    }
                                }
                            }
                        }
                        else
                        {
                            Validate_Message = "Please enter a valid Limit Price and Stop Price";
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
                txtReply = "Error In Validation of order";
                return false;
            }

            return true;

        }


        private void insertData(Model.Order.PendingOrder item)
        {
            try
            {
                Model.Order.PendingOrder objPendinOrder = new Model.Order.PendingOrder();
                objPendinOrder.BuySell = item.BuySell;
                objPendinOrder.TotalQty = item.TotalQty;
                objPendinOrder.RevQty = item.RevQty;
                objPendinOrder.SCode = item.SCode;
                objPendinOrder.ScripID = item.ScripID;
                objPendinOrder.Rate = item.Rate;
                objPendinOrder.ClientID = item.ClientID;
                objPendinOrder.Time = item.Time;
                objPendinOrder.OrdID = item.OrdID;
                objPendinOrder.ClientType = item.ClientType;
                objPendinOrder.RetainTill = item.RetainTill;
                objPendinOrder.CPCode = item.CPCode;
                objPendinOrder.OCOTrgRate = item.OCOTrgRate;
                objPendinOrder.Yield = item.Yield;
                objPendinOrder.DirtyPrice = item.DirtyPrice;
                objPendinOrder.OrderKey = item.OrderKey;
                objPendinOrder.OCOTrgRate = item.OCOTrgRate;
                selecteBulkScripList.Add(objPendinOrder);
                titleBulkPrice = string.Format("Bulk Change : Count {0}", selecteBulkScripList.Count);
            }
            catch (Exception e)
            {
                ExceptionUtility.LogError(e);
            }

        }

        private void UpdateBulkMemory(object orderModel, string status)
        {
            try
            {
                if (!string.IsNullOrEmpty(status) && status == Enumerations.OrderExecutionStatus.Exits.ToString())
                {
                    if (orderModel.GetType().Name == "OrderModel")
                    {
                        OrderModel oOrderModel = new OrderModel();
                        oOrderModel = orderModel as OrderModel;
                        if (oOrderModel != null)
                        {
                            if (new[] { "U" }.Any(x => x == oOrderModel.OrderAction))
                            {
                                Model.Order.PendingOrder oPendingOrder = new Model.Order.PendingOrder();
                                oPendingOrder = PendingOrderClassicVM.GETInstance.CreatePendingOrder(oOrderModel);

                                if (selecteBulkScripList != null && selecteBulkScripList.Count > 0)
                                {
                                    if (selecteBulkScripList.Any(x => x.OrderKey == oOrderModel.OrderKey))
                                    {
                                        int index = selecteBulkScripList.IndexOf(selecteBulkScripList.Where(x => x.OrderKey == oOrderModel.OrderKey).FirstOrDefault());
                                        if (index != -1)
                                        {
                                            selecteBulkScripList[index] = oPendingOrder;
                                        }

                                    }

                                }
                            }

                            else if (oOrderModel.OrderAction == "D")
                            {
                                if (selecteBulkScripList != null && selecteBulkScripList.Count > 0)
                                {
                                    if (selecteBulkScripList.Any(x => x.OrderKey == oOrderModel.OrderKey))
                                    {
                                        int index = selecteBulkScripList.IndexOf(selecteBulkScripList.Where(x => x.OrderKey == oOrderModel.OrderKey).FirstOrDefault());
                                        if (index != -1)
                                        {
                                            selecteBulkScripList.RemoveAt(index);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void ReadOrderResponse(object oModel, string status)
        {
            try
            {
                if (!string.IsNullOrEmpty(status) && status == Enumerations.WindowName.Pending_OE.ToString())
                {
                    if (oModel != null)
                    {
                        if (oModel.GetType().Name == "OrderModel")
                        {
                            OrderModel oOrderModel = new OrderModel();
                            oOrderModel = oModel as OrderModel;
                            txtReply = oOrderModel.ReplyText.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                ExceptionUtility.LogError(ex);
            }
        }

        #endregion


    }
}
