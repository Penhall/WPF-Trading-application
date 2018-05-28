using CommonFrontEnd.Common;
using CommonFrontEnd.Model.Order;
using CommonFrontEnd.Processor.Order;
using CommonFrontEnd.SharedMemories;
using CommonFrontEnd.View.Order;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static CommonFrontEnd.Common.Enumerations;

namespace CommonFrontEnd.ViewModel.Order
{
    public partial class ReturnedStopLossOrderVM : BaseViewModel
    {
        static ReturnedStopLossOrder mWindow = null;

        private static ObservableCollection<ReturnedStopLossOrderModel> _ReturnedStopLossOrderCollection = new ObservableCollection<ReturnedStopLossOrderModel>();

        public static ObservableCollection<ReturnedStopLossOrderModel> ReturnedStopLossOrderCollection
        {
            get { return _ReturnedStopLossOrderCollection; }
            set { _ReturnedStopLossOrderCollection = value; NotifyStaticPropertyChanged(nameof(ReturnedStopLossOrderCollection)); }
        }

        private RelayCommand _CloseWindowsOnEscape;

        public RelayCommand CloseWindowsOnEscape
        {
            get
            {
                return _CloseWindowsOnEscape ?? (_CloseWindowsOnEscape = new RelayCommand(
                    (object e) => { mWindow?.Close(); }
                        ));
            }
        }
        private RelayCommand _ExportExcel;
        public RelayCommand ExportExcel
        {
            get
            {
                return _ExportExcel ?? (_ExportExcel = new RelayCommand((object e) => ExecuteMyCommand()));
            }
        }


        private void ExecuteMyCommand()

        {
            if (ReturnedStopLossOrderCollection.Count == 0)
            {
                MessageBox.Show("No Stop Loss Orders to Save");
                return;
            }
            StreamWriter writer = null;
            SaveFileDialog dlg = new SaveFileDialog();
            //dlg.FileName = "HourlyStatistics_" + SelectedScripCode;
            dlg.DefaultExt = ".csv";
            dlg.Filter = "CSV Files (.csv)|*.csv";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                writer = new StreamWriter(dlg.FileName, false, Encoding.UTF8);
                try
                {
                    writer.Write("BuySell, SCode, ScripID, TotalQty, RevQty , LmtRate,TrgRate, ClientID, Time, OrdID, ClientType, CPCode");
                    writer.Write(writer.NewLine);

                    foreach (var dr in ReturnedStopLossOrderCollection)
                    {

                        writer.Write(dr.BuySell + "," + dr.SCode + "," + dr.ScripID + "," + dr.TotalQty + "," + dr.RevQty + "," + Convert.ToDouble(dr.LimitRate) * 100 + "," + dr.TriggertRate + "," + dr.ClientID + "," +
                            dr.Time + "," + dr.OrdID + "," + dr.ClientType + ",");

                        writer.Write(writer.NewLine);
                    }

                    System.Windows.MessageBox.Show("Returned StopLoss Orders Saved in file :" + dlg.FileName.ToString(), "Message", MessageBoxButton.OK);
                }
                catch (Exception e)
                {
                    ExceptionUtility.LogError(e);
                    System.Windows.MessageBox.Show("Error in Exporting data in CSV Format");
                }
                finally
                {
                    if (writer != null)
                    {
                        writer.Flush();
                        writer.Close();
                    }
                }
            }

            else
            {
                return;
            }
        }

        public ReturnedStopLossOrderVM()
        {
            OrderProcessor.OnOrderResponseReceived += UpdateRetStopLoss;
            //OrderProcessor.OrderResponseReceived += ReadDataFromOrderMemory;
            //Pickdata();
            ReadDataFromOrderMemory(OrderExecutionStatus.RstopExist.ToString());
            mWindow = System.Windows.Application.Current.Windows.OfType<ReturnedStopLossOrder>().FirstOrDefault();
        }

        private void UpdateRetStopLoss(object orderModel, string status)
        {
            if (!string.IsNullOrEmpty(status) && status == Enumerations.OrderExecutionStatus.RstopExist.ToString())
            {
                if (orderModel != null)
                {
                    if (orderModel.GetType().Name == "OrderModel")
                    {
                        OrderModel oOrderModel = new OrderModel();
                        oOrderModel = orderModel as OrderModel;
                        AssignData(oOrderModel);
                    }
                }
            }
        }

        private void ReadDataFromOrderMemory(string status)
        {
            if (status == OrderExecutionStatus.RstopExist.ToString())
            {
                ReturnedStopLossOrderCollection.Clear();
                if (MemoryManager.OrderDictionary != null && MemoryManager.OrderDictionary.Count > 0)
                {
                    foreach (OrderModel item in MemoryManager.OrderDictionary.Values.Where(x => (x.InternalOrderStatus == OrderExecutionStatus.RstopExist.ToString())))
                    {

                        //OrderModel oOrderModel = new OrderModel();
                        AssignData(item);
                    }
                    //foreach (OrderModel item in MemoryManager.OrderDictionary.Values.Where(x => x.InternalOrderStatus == Enumerations.OrderExecutionStatus.OrderCancelled_Return.ToString()))
                    //{
                    //    AssignData(item);
                    //}
                }
            }
        }

        private static void AssignData(OrderModel item)
        {
            if (item.InternalOrderStatus == Enumerations.OrderExecutionStatus.RstopExist.ToString())
            {
                if (new[] { "A", "U" }.Any(x => x == item.OrderAction))
                {
                    ReturnedStopLossOrderModel objReturnedOrderModel = new ReturnedStopLossOrderModel();

                    objReturnedOrderModel.BuySell = item.BuySellIndicator;
                    objReturnedOrderModel.SCode = item.ScripCode;
                    var Segment = CommonFunctions.GetSegmentID(objReturnedOrderModel.SCode);
                    int DecimalPoint = CommonFunctions.GetDecimal(Convert.ToInt32(objReturnedOrderModel.SCode), "BSE", Segment);
                    objReturnedOrderModel.ScripID = item.Symbol;
                    objReturnedOrderModel.TotalQty = item.PendingQuantity;
                    objReturnedOrderModel.RevQty = item.RevealQty;
                    objReturnedOrderModel.ClientType = item.ClientType;

                    objReturnedOrderModel.LimitRate = (item.Price / Math.Pow(10, DecimalPoint)).ToString();
                    if (Segment == Enumerations.Segment.Currency.ToString())
                    {
                        objReturnedOrderModel.LimitRateString = string.Format("{0}.{1}", objReturnedOrderModel.LimitRate, "0000");
                    }
                    else
                    {
                        objReturnedOrderModel.LimitRateString = string.Format("{0}.{1}", objReturnedOrderModel.LimitRate, "00");
                    }
                    objReturnedOrderModel.TriggertRate = (item.TriggerPrice / Math.Pow(10, DecimalPoint)).ToString();
                    if (Segment == Enumerations.Segment.Currency.ToString())
                    {
                        objReturnedOrderModel.TriggertRateString = string.Format("{0}.{1}", objReturnedOrderModel.TriggertRate, "0000");
                    }
                    else
                    {
                        objReturnedOrderModel.TriggertRateString = string.Format("{0}.{1}", objReturnedOrderModel.TriggertRate, "00");
                    }
                    objReturnedOrderModel.ClientID = item.ClientId;
                    objReturnedOrderModel.Time = Convert.ToDateTime(item.Time);
                    objReturnedOrderModel.OrdID = item.OrderId + item.OrderType;
                    objReturnedOrderModel.ClientType = item.ClientType;
                    objReturnedOrderModel.OrderKey = string.Format("{0}_{1}", item.ScripCode, item.OrderId);
                    objReturnedOrderModel.SegmentId = item.SegmentFlag;

                    if (ReturnedStopLossOrderCollection != null && ReturnedStopLossOrderCollection.Count > 0)
                    {
                        if (ReturnedStopLossOrderCollection.Any(x => x.OrderKey == item.OrderKey))
                        {
                            int index = ReturnedStopLossOrderCollection.IndexOf(ReturnedStopLossOrderCollection.Where(x => x.OrderKey == item.OrderKey).FirstOrDefault());
                            ReturnedStopLossOrderCollection[index] = objReturnedOrderModel;
                        }
                        else
                        {
                            ReturnedStopLossOrderCollection.Add(objReturnedOrderModel);
                        }
                    }
                    else
                    {
                        ReturnedStopLossOrderCollection?.Add(objReturnedOrderModel);
                    }
                }
                else if (item.OrderAction == "D")
                {
                    if (ReturnedStopLossOrderCollection != null && ReturnedStopLossOrderCollection.Count > 0)
                    {
                        if (ReturnedStopLossOrderCollection.Any(x => x.OrderKey == item.OrderKey))
                        {
                            int index = ReturnedStopLossOrderCollection.IndexOf(ReturnedStopLossOrderCollection.Where(x => x.OrderKey == item.OrderKey).FirstOrDefault());
                            if (index != -1)
                            {
                                ReturnedStopLossOrderCollection.RemoveAt(index);
                            }
                        }
                    }
                }
            }

            else
            {
                if (ReturnedStopLossOrderCollection != null && ReturnedStopLossOrderCollection.Count > 0)
                {
                    if (ReturnedStopLossOrderCollection.Any(x => x.OrderKey == item.OrderKey))
                    {
                        int index = ReturnedStopLossOrderCollection.IndexOf(ReturnedStopLossOrderCollection.Where(x => x.OrderKey == item.OrderKey).FirstOrDefault());
                        if (index != -1)
                        {
                            ReturnedStopLossOrderCollection.RemoveAt(index);
                        }
                    }
                }
            }


            //ReturnedStopLossOrderCollection.Add(objReturnedOrderModel);
        }

    }

    public partial class ReturnedStopLossOrderVM : BaseViewModel
    {
#if TWS

#endif
    }


    public partial class ReturnedStopLossOrderVM : BaseViewModel
    {
#if BOW

#endif
    }
}
