using CommonFrontEnd.Common;
using CommonFrontEnd.Model.Order;
using CommonFrontEnd.Processor.Order;
using CommonFrontEnd.SharedMemories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows;
using static CommonFrontEnd.SharedMemories.MemoryManager;

namespace CommonFrontEnd.ViewModel.Order
{

    public partial class PendingOrderVM : BaseViewModel
    {
        #region Properties

        static SynchronizationContext uiContext;

        public static int Decimal_Point { get; set; }

        private static ObservableCollection<PendingOrder> _ObjPendingOrderCollection;

        public static ObservableCollection<PendingOrder> ObjPendingOrderCollection
        {
            get { return _ObjPendingOrderCollection; }
            set { _ObjPendingOrderCollection = value; NotifyStaticPropertyChanged(nameof(ObjPendingOrderCollection)); }
        }
        #endregion

        #region Relay Commands
        private RelayCommand _Modify_Click;

        public RelayCommand Modify_Click
        {
            get { return _Modify_Click ?? (_Modify_Click = new RelayCommand((object e) => Modify_ClickButton())); }

        }

        private RelayCommand _Cancel_Click;

        public RelayCommand Cancel_Click
        {
            get { return _Cancel_Click ?? (_Cancel_Click = new RelayCommand((object e) => Cancel_ClickButton())); }

        }

        private RelayCommand _Cancel_All_Click;

        public RelayCommand Cancel_All_Click
        {
            get { return _Cancel_All_Click ?? (_Cancel_All_Click = new RelayCommand((object e) => Cancel_All_ClickButton())); }

        }

        private List<PendingOrder> _selectedModels = new List<PendingOrder>();

        public List<PendingOrder> SelectedValue
        {
            get { return _selectedModels; }
            set
            {
                _selectedModels = value;
                NotifyPropertyChanged(nameof(SelectedValue));
            }
        }

        #endregion

        #region Constructor
        public PendingOrderVM()
        {
            ObjPendingOrderCollection = new ObservableCollection<PendingOrder>();
            uiContext = SynchronizationContext.Current;
            //OrderProcessor.OrderResponseReceived += ReadDataFromOrderMemory;
            //ReadDataFromOrderMemory();

        }

        #endregion

        #region Methods
        ///// <summary>
        ///// Add/Update Response in Pending Window
        ///// </summary>
        ///// <param name="obj"></param>
        //public static void UpdateOrderResponse(OrderModel obj)
        //{
        //   if (obj != null && ObjPendingOrderCollection != null)
        //    {
        //        PendingOrder oPendingOrder = new PendingOrder();
        //        Decimal_Point = CommonFunctions.GetDecimal(Convert.ToInt32(obj.ScripCode), obj.Exchange, obj.Segment);
        //        oPendingOrder.BuySell = obj.BuySellIndicator;
        //        oPendingOrder.TotalQty = obj.Quantity;
        //        oPendingOrder.RevQty = obj.RevealQty;
        //        oPendingOrder.SCode = obj.ScripCode;
        //        oPendingOrder.ScripID = obj.Symbol;
        //        oPendingOrder.Rate = Convert.ToInt64(Convert.ToDouble(obj.Price) / Math.Pow(10, Decimal_Point));
        //        oPendingOrder.ClientID = obj.ClientId;
        //        oPendingOrder.OrdID = obj.OrderNumber;
        //        oPendingOrder.ClientType = obj.ClientType;
        //        oPendingOrder.CPCode = obj.ParticipantCode;
        //        oPendingOrder.OrderStatus = obj.OrdStatus;
        //        oPendingOrder.Yield = obj.Yield;
        //        uiContext?.Send(x => ObjPendingOrderCollection?.Add(oPendingOrder), null);
        //        MessageBox.Show(obj.Reason);
        //    }
        //}


        /// <summary>
        /// Reads from Order Memory whose status is Exits
        /// </summary>
        private void ReadDataFromOrderMemory()
        {
            uiContext?.Send(x => ObjPendingOrderCollection?.Clear(), null);
            if (OrderDictionary != null && OrderDictionary.Count > 0)
            {
                foreach (OrderModel item in OrderDictionary.Values.Where(x => x.InternalOrderStatus == Enumerations.OrderExecutionStatus.Exits.ToString()))
                {
                    PendingOrder oPendingOrder = new PendingOrder();
                    // OrderModel obj = item;
                    Decimal_Point = CommonFunctions.GetDecimal(Convert.ToInt32(item.ScripCode), "BSE", CommonFunctions.GetSegmentID(item.ScripCode));
                    //if (item.BuySellIndicator == Enumerations.Order.BuySellFlag.BUY.ToString())
                    //{
                    //    oPendingOrder.BuySell = Enumerations.SideShort.B.ToString();
                    //}
                    //else if (item.BuySellIndicator == Enumerations.Order.BuySellFlag.SELL.ToString())
                    //{
                    //    oPendingOrder.BuySell = Enumerations.SideShort.S.ToString();
                    //}
                    item.Segment = CommonFunctions.GetSegmentID(item.ScripCode);
                    oPendingOrder.BuySell = item.BuySellIndicator;
                    oPendingOrder.TotalQty = item.OriginalQty;
                    oPendingOrder.RevQty = item.RevealQty;
                    oPendingOrder.SCode = item.ScripCode;
                    oPendingOrder.ScripID = item.Symbol;
                    oPendingOrder.Segment = item.Segment;
                    oPendingOrder.Time = Convert.ToDateTime(item.Time);
#if BOW
                    oPendingOrder.Rate = Convert.ToDouble(Convert.ToDouble(item.Price) / Math.Pow(10, Decimal_Point));
                    oPendingOrder.PendingQty = item.PendingQuantity;
                    oPendingOrder.DirtyPrice = Convert.ToString(item.Price);
#elif TWS
                    oPendingOrder.OrderType = item.OrderType?.ToString();


                    oPendingOrder.Rate = Convert.ToString(Convert.ToDouble(item.Price) / Math.Pow(10, Decimal_Point));
                    if (oPendingOrder.Segment == Enumerations.Order.ScripSegment.Debt.ToString())
                    {
                        if (item.Price == long.MinValue || item.Price == long.MaxValue)
                        {
                            oPendingOrder.DirtyPrice = "";
                        }
                        else
                        {
                            oPendingOrder.DirtyPrice = Convert.ToString(item.Price);
                        }
                        if (item.Yield == long.MinValue || item.Yield == long.MaxValue)
                        {
                            oPendingOrder.Yield = 0;
                        }
                        else
                        {
                            oPendingOrder.Yield = item.Yield;
                        }
                    }
                    oPendingOrder.RetainTill = item.OrderRetentionStatus?.ToString();
#endif
                    oPendingOrder.ClientID = item.ClientId;
                    oPendingOrder.OrdID = item.OrderId;
                    oPendingOrder.OrdNumber = item.OrderNumber;
                    oPendingOrder.ClientType = item.ClientType;
                    oPendingOrder.CPCode = item.ParticipantCode;
                    oPendingOrder.OrderStatus = item.OrdStatus;

#if BOW
                    oPendingOrder.Yield = item.Yield;
                    oPendingOrder.Time = item.Time;
#elif TWS


                    //TODO After data Correction from IML Gaurav 22NOV2017
                    //var dateTime = new DateTime(item.Year, item.Month, item.Day, item.Minute, item.Second, item.Msecond);
                    //oPendingOrder.Time = Convert.ToString(dateTime);
#endif
                    oPendingOrder.OEREmarks = item.OrderRemarks;
                    oPendingOrder.MessageTag = item.MessageTag;
                    string key = string.Format("{0}_{1}", item.ScripCode, item.OrderId);
                    oPendingOrder.OrderKey = key;
                    uiContext?.Send(x => ObjPendingOrderCollection?.Add(oPendingOrder), null);
                    //ObjPendingOrderCollection.Add(oPendingOrder);
                }
            }
        }

        /// <summary>
        /// Callled when Cancel All button is clicked. Deletes all pending orders
        /// </summary>
        private void Cancel_All_ClickButton()
        {
            try
            {
                if (ObjPendingOrderCollection != null && ObjPendingOrderCollection.Count > 0)
                {
                    if (MessageBox.Show("Do you really want to delete all the pending orders?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {
                        foreach (PendingOrder item in ObjPendingOrderCollection)
                        {
#if BOW
                            OrderProcessor.SendDeleteOrderRequest(OrderDictionary.Select(x => x.Value).Where(x => x.OrderRemarks == item.OEREmarks).FirstOrDefault());
#elif TWS
                            // OrderProcessor.SendDeleteOrderRequest(item.MessageTag);
                            OrderRequestProcessor oOrderRequestProcessor = new OrderRequestProcessor(new DeleteOrder());
                            OrderModel oOrderModel = new OrderModel();
                            MemoryManager.OrderDictionary.TryGetValue(item.OrderKey, out oOrderModel);
                            if (oOrderModel != null)
                            {
                                oOrderRequestProcessor.ProcessRequest(oOrderModel);
                            }
                            else
                            {
                                MessageBox.Show("Can't find order");
                            }
#endif
                        }
                    }
                }

                else
                {
                    MessageBox.Show("No pending orders to delete", "Delete Orders", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
            }
        }

        /// <summary>
        /// Called when Cancel button is clicked. Deletes selected pending order
        /// </summary>
        private void Cancel_ClickButton()
        {

            try
            {
                //if (SelectedValue != null)
                if (SelectedValue != null && SelectedValue.Count > 0)
                {
                    if (MessageBox.Show("Do you really want to delete this Order?", "Delete Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {
                        foreach (var item in SelectedValue)
                        {
#if BOW
                            OrderProcessor.SendDeleteOrderRequest(OrderDictionary.Select(x => x.Value).Where(x => x.OrderRemarks == item.OEREmarks).FirstOrDefault());
#elif TWS
                            //OrderProcessor.SendDeleteOrderRequest(item.MessageTag);
                            OrderRequestProcessor oOrderRequestProcessor = new OrderRequestProcessor(new DeleteOrder());
                            OrderModel oOrderModel = new OrderModel();
                            MemoryManager.OrderDictionary.TryGetValue(item.OrderKey, out oOrderModel);
                            if (oOrderModel != null)
                            {
                                oOrderRequestProcessor.ProcessRequest(oOrderModel);
                            }
                            else
                            {
                                MessageBox.Show("Can't find order");
                            }
#endif

                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please Select the Order to Cancel", "Cancel Order", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
            }
        }

        /// <summary>
        /// Called When Modify Button is clicked. Pending order can be modified
        /// </summary>
        private void Modify_ClickButton()
        {

            try
            {
                //if (SelectedValue != null)
                if (SelectedValue != null && SelectedValue.Count > 0)
                {
#if BOW
                    //OrderProcessor.GetOrderDataFromOrderID(SelectedValue.FirstOrDefault());                  
                    OrderProcessor.GetOrderDataFromOrderID(SelectedValue.FirstOrDefault());
#elif TWS
                    OrderProcessor.GetOrderDataByMessageTag(SelectedValue.FirstOrDefault());
#endif
                }
                else
                {
                    MessageBox.Show("Please Select the Order to Modify", "Modify Order", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
            }
        }
        //#endregion
        #endregion
    }

    public partial class PendingOrderVM : BaseViewModel
    {

    }

}



