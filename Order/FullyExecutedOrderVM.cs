using CommonFrontEnd.Common;
using CommonFrontEnd.Global;
using CommonFrontEnd.Model.Order;
using CommonFrontEnd.Model.Trade;
using CommonFrontEnd.Processor.Order;
using CommonFrontEnd.SharedMemories;
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

    public partial class FullyExecutedOrderVM : BaseViewModel
    {
        #region LocalMemory
        static View.Order.FullyExecutedOrder mWindow = null;

        private static ObservableCollection<FullyExecutedModel> _FullyExecutedOrderCollection = new ObservableCollection<FullyExecutedModel>();

        public static ObservableCollection<FullyExecutedModel> FullyExecutedOrderCollection
        {
            get { return _FullyExecutedOrderCollection; }
            set { _FullyExecutedOrderCollection = value; NotifyStaticPropertyChanged(nameof(FullyExecutedOrderCollection)); }
        }

        private static string _setColor;

        public static string setColor
        {
            get { return _setColor; }
            set { _setColor = value; NotifyStaticPropertyChanged(nameof(setColor)); }
        }
        #endregion

        #region RelayCommand
        private RelayCommand _CloseWindowsOnEscape;

        public RelayCommand CloseWindowsOnEscape
        {
            get
            {
                return _CloseWindowsOnEscape ?? (_CloseWindowsOnEscape = new RelayCommand(
                    (object e) => CloseWindowsOnEscape_Click()
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
        #endregion

            #region Constructor
        public FullyExecutedOrderVM()
        {
            OrderProcessor.OnOrderResponseReceived += UpdateFullyOrderMemory;
            //OrderProcessor.OrderResponseReceived += ReadDataFromOrderMemory;
            mWindow = System.Windows.Application.Current.Windows.OfType<View.Order.FullyExecutedOrder>().FirstOrDefault();
            //Pickdata();
            ReadDataFromOrderMemory(OrderExecutionStatus.Executed.ToString());

        }


        #endregion

        #region methods

        private void UpdateFullyOrderMemory(object orderModel, string status)
        {
            if (!string.IsNullOrEmpty(status) && status == Enumerations.OrderExecutionStatus.Executed.ToString())
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

        public void ReadDataFromOrderMemory(string status)
        {
            if (status == OrderExecutionStatus.Executed.ToString())
            {
                if (MemoryManager.OrderDictionary != null && MemoryManager.OrderDictionary.Count > 0)
                {
                    foreach (OrderModel item in MemoryManager.OrderDictionary.Values.Where(x => (x.InternalOrderStatus == Enumerations.OrderExecutionStatus.Executed.ToString() || x.InternalOrderStatus == Enumerations.OrderExecutionStatus.OrderCancelled_Return.ToString())))
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
            //OrderModel objOrderModel = null;
            //try
            //{

            //    if (MemoryManager.OrderDictionary.ContainsKey(Key))
            //    {
            //        objOrderModel = new OrderModel();
            //        objOrderModel = MemoryManager.OrderDictionary[Key];
            //        if (objOrderModel != null)
            //        {
            //            AssignData(objOrderModel);
            //        }
            //    }
            //    else
            //    {
            //        MessageBox.Show("Order not found in order memory");
            //    }

            //}
            //catch (Exception ex)
            //{
            //    ExceptionUtility.LogError(ex);
            //}

        }

        //public void Pickdata()
        //{
        //    try
        //    {
        //        foreach (var item in MemoryManager.OrderDictionary.Values.Where(x => x.InternalOrderStatus == OrderExecutionStatus.Executed.ToString()).ToList())
        //        {
        //            if (item != null)
        //            {
        //                AssignData(item);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionUtility.LogError(ex);
        //    }

        //}

        private static void AssignData(OrderModel item)
        {
            if (item.InternalOrderStatus == Enumerations.OrderExecutionStatus.Executed.ToString() || item.InternalOrderStatus == Enumerations.OrderExecutionStatus.OrderCancelled_Return.ToString())
            {
                //if (new[] { "A", "U" }.Any(x => x == item.OrderAction))
                //{
                //    AddInFullyExecutedMemory(item);
                //}
                //else if (item.OrderAction == "D")
                //{
                //    RemoveFromFullyExecutedMemory(item);
                //}
                 AddInFullyExecutedMemory(item);
            }
            else
            {
                RemoveFromFullyExecutedMemory(item);
            }


            //if (!FullyExecutedOrderCollection.Any(x => x.OrdID == objFullyExecutedModel.OrdID))
            //FullyExecutedOrderCollection.Add(objFullyExecutedModel);
        }

        private static void RemoveFromFullyExecutedMemory(OrderModel item)
        {
            if (FullyExecutedOrderCollection != null && FullyExecutedOrderCollection.Count > 0)
            {
                if (FullyExecutedOrderCollection.Any(x => x.OrderKey == item.OrderKey))
                {
                    int index = FullyExecutedOrderCollection.IndexOf(FullyExecutedOrderCollection.Where(x => x.OrderKey == item.OrderKey).FirstOrDefault());
                    if (index != -1)
                    {
                        FullyExecutedOrderCollection.RemoveAt(index);
                    }
                }
            }
        }

        private static void AddInFullyExecutedMemory(OrderModel item)
        {
            FullyExecutedModel objFullyExecutedModel = new FullyExecutedModel();
            objFullyExecutedModel.BuySell = item.BuySellIndicator;

            objFullyExecutedModel.ClientID = item.ClientId;
            objFullyExecutedModel.ClientType = item.ClientType;
            objFullyExecutedModel.OrdID = item.OrderId;
            objFullyExecutedModel.OrdType = item.OrderType;
            objFullyExecutedModel.Qty = item.TradedQty;
            objFullyExecutedModel.ScripID = item.Symbol;
            objFullyExecutedModel.ScripCode = item.ScripCode.ToString();
            objFullyExecutedModel.ScripGroup = item.Group.ToString();
            string Segment_Name = CommonFunctions.GetSegmentID(item.ScripCode);
            int Decimal_pnt = CommonFunctions.GetDecimal(System.Convert.ToInt32(item.ScripCode), "BSE", Segment_Name);
            objFullyExecutedModel.Time = Convert.ToDateTime(item.Time);
            if (objFullyExecutedModel.Qty > 0)
            {
                if (Decimal_pnt == 2)
                    objFullyExecutedModel.AvgRate = string.Format("{0:0.00}", (item.NetOrderValue / objFullyExecutedModel.Qty) / Math.Pow(10, Decimal_pnt));
                else if (Decimal_pnt == 4)
                    objFullyExecutedModel.AvgRate = string.Format("{0:0.0000}", (item.NetOrderValue / objFullyExecutedModel.Qty) / Math.Pow(10, Decimal_pnt));
                else
                    objFullyExecutedModel.AvgRate = string.Format("{0:0.00}", (item.NetOrderValue / objFullyExecutedModel.Qty) / Math.Pow(10, Decimal_pnt));
            }

            objFullyExecutedModel.OrderKey = string.Format("{0}_{1}", item.ScripCode, item.OrderId);
            objFullyExecutedModel.SegmentID = item.SegmentFlag;

            if (FullyExecutedOrderCollection != null && FullyExecutedOrderCollection.Count > 0)
            {
                if (FullyExecutedOrderCollection.Any(x => x.OrderKey == item.OrderKey))
                {
                    int index = FullyExecutedOrderCollection.IndexOf(FullyExecutedOrderCollection.Where(x => x.OrderKey == item.OrderKey).FirstOrDefault());
                    FullyExecutedOrderCollection[index] = objFullyExecutedModel;
                }
                else
                {
                    FullyExecutedOrderCollection.Add(objFullyExecutedModel);
                }
            }
            else
            {
                FullyExecutedOrderCollection?.Add(objFullyExecutedModel);
            }
        }

        private void ExecuteMyCommand()
        
            {
            if (FullyExecutedOrderCollection.Count == 0)
            {
                MessageBox.Show("No Orders to Save","Information", MessageBoxButton.OK,MessageBoxImage.Information);
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
                try
                {
                    writer = new StreamWriter(dlg.FileName, false, Encoding.UTF8);

                    writer.Write("Buy/Sell, ScripID, Qty, AvgRate, Scrip_Code,Scrip_Group, ClientID, Time, OrdID, ClientType,Average DirtyPrice");
                    writer.Write(writer.NewLine);

                    if (Global.UtilityTradeDetails.GetInstance.SelectedID == 1)
                    {
                        foreach (var dr in FullyExecutedOrderCollection)
                        {
                            if (dr.SegmentID == 1)
                            {
                                writer.Write(dr.BuySell + "," + dr.ScripID + "," + dr.Qty + "," + dr.AvgRate +","+dr.ScripCode+ "," +dr.ScripGroup+","+ dr.ClientID + "," + dr.Time.ToString() + "," + dr.OrdID + "," + dr.ClientType + "," + dr.AvgDirtyPrice);
                                writer.Write(writer.NewLine);
                            }

                        }
                        System.Windows.MessageBox.Show("Fully Executed Equity Orders Saved in file :" + dlg.FileName.ToString(), "Message", MessageBoxButton.OK,MessageBoxImage.Information);

                    }
                    else if (Global.UtilityTradeDetails.GetInstance.SelectedID == 2)
                    {
                        foreach (var dr in FullyExecutedOrderCollection)
                        {
                            if (dr.SegmentID == 2)
                            {
                                writer.Write(dr.BuySell + "," + dr.ScripID + "," + dr.Qty + "," + dr.AvgRate + "," + dr.ScripCode + "," + dr.ScripGroup + ","  + dr.ClientID + "," + dr.Time.ToString() + "," + dr.OrdID + "," + dr.ClientType + "," + dr.AvgDirtyPrice);
                                writer.Write(writer.NewLine);
                            }

                        }
                        System.Windows.MessageBox.Show("Fully Executed Derivatives Orders Saved in file :" + dlg.FileName.ToString(), "Message", MessageBoxButton.OK,MessageBoxImage.Information);

                    }
                    else if (Global.UtilityTradeDetails.GetInstance.SelectedID == -1)
                    {
                        foreach (var dr in FullyExecutedOrderCollection)
                        {
                            writer.Write(dr.BuySell + "," + dr.ScripID + "," + dr.Qty + "," + dr.AvgRate + "," + dr.ScripCode + "," + dr.ScripGroup + ","  + dr.ClientID + "," + dr.Time.ToString() + "," + dr.OrdID + "," + dr.ClientType + "," + dr.AvgDirtyPrice);
                            writer.Write(writer.NewLine);

                        }
                        System.Windows.MessageBox.Show("All Fully Executed Orders Saved in file :" + dlg.FileName.ToString(), "Message", MessageBoxButton.OK,MessageBoxImage.Information);

                    }
                    else if (Global.UtilityTradeDetails.GetInstance.SelectedID == 3)
                    {
                        foreach (var dr in FullyExecutedOrderCollection)
                        {
                            if (dr.SegmentID == 3)
                            {
                                writer.Write(dr.BuySell + "," + dr.ScripID + "," + dr.Qty + "," + dr.AvgRate + "," + dr.ScripCode + "," + dr.ScripGroup + ","  + dr.ClientID + "," + dr.Time.ToString() + "," + dr.OrdID + "," + dr.ClientType + "," + dr.AvgDirtyPrice);
                                writer.Write(writer.NewLine);
                            }

                        }
                        System.Windows.MessageBox.Show("Fully Executed Currency Orders Saved in file :" + dlg.FileName.ToString(), "Message", MessageBoxButton.OK);

                    }

                }
                catch (IOException io)
                {
                    ExceptionUtility.LogError(io);
                    MessageBox.Show("The File is being used by another process. Close the file and try again."," Error!",MessageBoxButton.OK,MessageBoxImage.Error); 
                }
                catch (Exception e)
                {
                    ExceptionUtility.LogError(e);
                    System.Windows.MessageBox.Show("Error in Exporting data in CSV Format"," Error!",MessageBoxButton.OK,MessageBoxImage.Error);
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


        private void CloseWindowsOnEscape_Click()
        {
            mWindow?.Close();
        }

        #endregion

    }
}
