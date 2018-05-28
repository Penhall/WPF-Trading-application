using CommonFrontEnd.Common;
using CommonFrontEnd.Global;
using CommonFrontEnd.Model.Order;
using CommonFrontEnd.Processor.Order;
using CommonFrontEnd.SharedMemories;
using CommonFrontEnd.View.Order;
using CommonFrontEnd.ViewModel.Profiling;
using CommonFrontEnd.ViewModel.Trade;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using static CommonFrontEnd.Common.Enumerations;

namespace CommonFrontEnd.ViewModel.Order
{
    public partial class ReturnedOrderVM : BaseViewModel
    {
        #region Memory 
        static ReturnedOrders mWindow = null;

        private static ObservableCollection<ReturnedOrderModel> _ReturnedOrderCollection = new ObservableCollection<ReturnedOrderModel>();

        public static ObservableCollection<ReturnedOrderModel> ReturnedOrderCollection
        {
            get { return _ReturnedOrderCollection; }
            set { _ReturnedOrderCollection = value; NotifyStaticPropertyChanged(nameof(ReturnedOrderCollection)); }
        }

        private ObservableCollection<string> _FilterReasonList;

        public ObservableCollection<string> FilterReasonList
        {
            get { return _FilterReasonList; }
            set { _FilterReasonList = value; NotifyPropertyChanged(nameof(FilterReasonList)); }
        }
        #endregion

        #region Property
        private bool _chkReasonFilter;

        public bool chkReasonFilter
        {
            get { return _chkReasonFilter; }
            set { _chkReasonFilter = value; NotifyPropertyChanged(nameof(chkReasonFilter)); checkFilterResonEnability(); }
        }

        private bool _chkTimeFilter;

        public bool chkTimeFilter
        {
            get { return _chkTimeFilter; }
            set { _chkTimeFilter = value; NotifyPropertyChanged(nameof(chkTimeFilter)); checkFilterTimeEnability(); }
        }

        private bool _cmbReason;

        public bool cmbReason
        {
            get { return _cmbReason; }
            set { _cmbReason = value; NotifyPropertyChanged(nameof(cmbReason)); }
        }

        private bool _tpEnability;

        public bool tpEnability
        {
            get { return _tpEnability; }
            set { _tpEnability = value; NotifyPropertyChanged(nameof(tpEnability)); }
        }

        private bool _btnShowAllEnability = false;

        public bool btnShowAllEnability
        {
            get { return _btnShowAllEnability; }
            set { _btnShowAllEnability = value; NotifyPropertyChanged(nameof(btnShowAllEnability)); }
        }

        private string _txtReply = string.Empty;

        public string txtReply
        {
            get { return _txtReply; }
            set { _txtReply = value; NotifyPropertyChanged(nameof(txtReply)); }
        }


        private string _selectedReason;

        public string selectedReason
        {
            get { return _selectedReason; }
            set { _selectedReason = value; NotifyPropertyChanged(nameof(selectedReason)); }
        }

        private DateTime _starttime;

        public DateTime starttime
        {
            get { return _starttime; }
            set { _starttime = value; NotifyPropertyChanged(nameof(starttime)); }
        }

        private DateTime _stoptime;

        public DateTime stoptime
        {
            get { return _stoptime; }
            set { _stoptime = value; NotifyPropertyChanged(nameof(stoptime)); }
        }

        private string _txtClientID;

        public string txtClientID
        {
            get { return _txtClientID; }
            set { _txtClientID = value; NotifyPropertyChanged(nameof(txtClientID)); }
        }


        public static string SMTPIP { get; private set; }
        public static string LiveSettings { get; private set; }
        public static bool IsPing { get; private set; }
        public static string GmailEmailID { get; private set; }
        public static string GmailPassword { get; private set; }
        public static string LiveFromEmail { get; private set; }
        public static string LiveSMTPIP { get; private set; }
        public static string LiveSMTPPort { get; private set; }
        public static string LiveSMTPPassword { get; private set; }
        public static string LiveSMTPEmailID { get; private set; }

        public int PortStatus = 2;
        int SerialNumber = 1;
        int serialNumber = 1;
        private ReturnedOrderModel _selectEntireRow;

        public ReturnedOrderModel selectEntireRow
        {
            get { return _selectEntireRow; }
            set { _selectEntireRow = value; NotifyPropertyChanged(nameof(selectEntireRow)); }
        }


        #endregion

        #region RelayCommand
        private RelayCommand _btnFilterGrid;

        public RelayCommand btnFilterGrid
        {
            get { return _btnFilterGrid ?? (_btnFilterGrid = new RelayCommand((object e) => populateGridDataFilter(chkReasonFilter, chkTimeFilter))); }
        }

        private RelayCommand _btnShowALL;

        public RelayCommand btnShowALL
        {
            get { return _btnShowALL ?? (_btnShowALL = new RelayCommand((object e) => ReadAllDataFromOrderMemory(Enumerations.OrderExecutionStatus.Return.ToString()))); }
        }

        private RelayCommand _EMailClick;

        public RelayCommand EMailClick
        {
            get { return _EMailClick ?? (_EMailClick = new RelayCommand((object e) => EMailClick_CLick())); }
        }

        private RelayCommand _SaveImageClick;

        public RelayCommand SaveImageClick
        {
            get { return _SaveImageClick ?? (_SaveImageClick = new RelayCommand((object e) => SaveImageClick_Click())); }
        }

        private RelayCommand _btnReSubmit;

        public RelayCommand btnReSubmit
        {
            get { return _btnReSubmit ?? (_btnReSubmit = new RelayCommand((object e) => OnReSubmitClick())); }
        }

        private RelayCommand _btnBatchResubmit;

        public RelayCommand btnBatchResubmit
        {
            get { return _btnBatchResubmit ?? (_btnBatchResubmit = new RelayCommand((object e) => OnBatchReSubmitClick())); }
        }

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


        public ReturnedOrderVM()
        {
            FilterReasonList = new ObservableCollection<string>();
            PopulatedReason();
            //   OrderProcessor.OrderResponseReceived += OnlineResponseReceived;
            OrderProcessor.OnOrderResponseReceived += UpdateReturnOrder;
            //OrderProcessor.OrderResponseReceived += ReadDataFromOrderMemory;
            mWindow = System.Windows.Application.Current.Windows.OfType<ReturnedOrders>().FirstOrDefault();
            ReadDataFromOrderMemory(Enumerations.OrderExecutionStatus.Return.ToString());
        }

        private void UpdateReturnOrder(object oModel, string status)
        {
            if (status == OrderExecutionStatus.Return.ToString())
            {
                if (oModel != null)
                {
                    if (oModel.GetType().Name == "OrderModel")
                    {
                        OrderModel orderModel = new OrderModel();
                        orderModel = oModel as OrderModel;
                        AssignData(orderModel);
                    }
                }
            }
        }



        #region Methods
        private void PopulatedReason()
        {
            FilterReasonList = new ObservableCollection<string>();
            selectedReason = "-Select-";
            FilterReasonList.Add(selectedReason);
            FilterReasonList.Add(Common.Enumerations.ReturnedOrderReason.EOSSESS.ToString());
            FilterReasonList.Add(Common.Enumerations.ReturnedOrderReason.PCAS.ToString());
            FilterReasonList.Add(Common.Enumerations.ReturnedOrderReason.RRM.ToString());
            FilterReasonList.Add(Common.Enumerations.ReturnedOrderReason.SPOS.ToString());
            FilterReasonList.Add(Common.Enumerations.ReturnedOrderReason.MASSCANCELL.ToString());
            FilterReasonList.Add(Common.Enumerations.ReturnedOrderReason.OTHER.ToString());
        }
        private void checkFilterResonEnability()
        {
            if (chkReasonFilter == true)
            {
                cmbReason = true;
            }
            else
            {
                cmbReason = false;
            }
        }
        private void checkFilterTimeEnability()
        {
            if (chkTimeFilter == true)
            {
                tpEnability = true;
            }
            else
            {
                tpEnability = false;
                ReadDataFromOrderMemory(Enumerations.OrderExecutionStatus.Return.ToString());
                txtReply = string.Empty;
            }
        }
        private void ReadAllDataFromOrderMemory(string status)
        {
            chkReasonFilter = false;
            chkTimeFilter = false;
            if (status == OrderExecutionStatus.Return.ToString())
            {
                ReturnedOrderCollection?.Clear();
                if (MemoryManager.OrderDictionary != null && MemoryManager.OrderDictionary.Count > 0)
                {
                    ReturnedOrderCollection?.Clear();
                    foreach (OrderModel item in MemoryManager.OrderDictionary.Values.Where(x => (x.InternalOrderStatus == OrderExecutionStatus.Return.ToString() || x.InternalOrderStatus == Enumerations.OrderExecutionStatus.OrderCancelled_Return.ToString())))
                    {
                        int count = AssignData(item);
                        txtReply = count + " Record/s Found.";
                    }
                }
            }
            btnShowAllEnability = false;
            //if (mWindow != null)
            //{
            //    mWindow?.dataGrid?.UnselectAll();
            //}
        }
        private void ReadDataFromOrderMemory(string status)
        {
            if (status == OrderExecutionStatus.Return.ToString())
            {
                if (MemoryManager.OrderDictionary != null && MemoryManager.OrderDictionary.Count > 0)
                {
                    ReturnedOrderCollection?.Clear();
                    foreach (OrderModel item in MemoryManager.OrderDictionary.Values.Where(x => (x.InternalOrderStatus == OrderExecutionStatus.Return.ToString() || x.InternalOrderStatus == Enumerations.OrderExecutionStatus.OrderCancelled_Return.ToString())))
                    {

                        if (chkTimeFilter == true && chkReasonFilter == false)
                        {
                            populateGridDataFilter(chkReasonFilter, chkTimeFilter);
                        }
                        else if (chkReasonFilter == true && chkTimeFilter == false)
                        {
                            populateGridDataFilter(chkReasonFilter, chkTimeFilter);
                        }
                        else if (chkTimeFilter == true && chkReasonFilter == true)
                        {
                            populateGridDataFilter(chkReasonFilter, chkTimeFilter);
                        }
                        else
                        {
                            int count = AssignData(item);
                            txtReply = count + " Record/s Found.";
                            if (count == 0)
                            {
                                txtReply = "Record Not Found.";
                            }

                        }
                    }
                    //foreach (OrderModel item in MemoryManager.OrderDictionary.Values.Where(x => x.InternalOrderStatus == OrderExecutionStatus.OrderCancelled_Return.ToString()))
                    //{
                    //    AssignData(item);
                    //}
                }
            }
        }
        //private void populateGridData()
        //{
        //    ReturnedOrderCollection?.Clear();
        //    foreach (OrderModel item in MemoryManager.OrderDictionary.Values.Where(x => x.InternalOrderStatus == OrderExecutionStatus.Return.ToString()))
        //    {
        //        btnShowAllEnability = false;
        //        AssignData(item);
        //    }
        //}
        private static int AssignData(OrderModel item)
        {
            if (item.InternalOrderStatus == OrderExecutionStatus.Return.ToString())
            {
                if (new[] { "A", "U" }.Any(x => x == item.OrderAction))
                {
                    AddInReturnMemory(item);
                }
                else if (item.OrderAction == "D")
                {
                    RemoveFromReturnMemory(item);
                }
            }
            else
            {
                RemoveFromReturnMemory(item);
            }

            return ReturnedOrderCollection.Count;
        }

        private static void RemoveFromReturnMemory(OrderModel item)
        {
            if (ReturnedOrderCollection != null && ReturnedOrderCollection.Count > 0)
            {
                if (ReturnedOrderCollection.Any(x => x.OrderKey == item.OrderKey))
                {
                    int index = ReturnedOrderCollection.IndexOf(ReturnedOrderCollection.Where(x => x.OrderKey == item.OrderKey).FirstOrDefault());
                    if (index != -1)
                    {
                        ReturnedOrderCollection.RemoveAt(index);
                    }
                }
            }
        }

        private static void AddInReturnMemory(OrderModel item)
        {
            ReturnedOrderModel objReturnedOrderModel = new ReturnedOrderModel();
            string Segment_Name = CommonFunctions.GetSegmentID(item.ScripCode);
            int Decimal_pnt = CommonFunctions.GetDecimal(System.Convert.ToInt32(item.ScripCode), "BSE", Segment_Name);
            objReturnedOrderModel.BuySell = item.BuySellIndicator;
            objReturnedOrderModel.TotalQty = item.PendingQuantity;
            objReturnedOrderModel.RevQty = item.RevealQty;
            objReturnedOrderModel.SCode = item.ScripCode;
            objReturnedOrderModel.ScripID = item.Symbol;
            objReturnedOrderModel.Rate = CommonFunctions.GetValueInDecimal(item.Price, Decimal_pnt);

            //objReturnedOrderModel.Rate = (item.Price / Math.Pow(10, Decimal_pnt)).ToString();
            //if (Segment_Name == Enumerations.Segment.Currency.ToString())
            //{
            //    objReturnedOrderModel.RateString = string.Format("{0:0000}", objReturnedOrderModel.Rate);
            //}
            //else
            //{
            //    objReturnedOrderModel.RateString = string.Format("{0:0.00}", objReturnedOrderModel.Rate);
            //}
            objReturnedOrderModel.ClientID = item.ClientId;
            objReturnedOrderModel.Time = Convert.ToDateTime(item.Time);
            objReturnedOrderModel.OnlyOrderID = item.OrderId;
            objReturnedOrderModel.OrderType = item.OrderType;
            objReturnedOrderModel.OrdID = item.OrderId + item.OrderType;
            objReturnedOrderModel.ClientType = item.ClientType;
            objReturnedOrderModel.RetainTill = item.OrderRetentionStatus;
            string Filter = string.Empty;
            objReturnedOrderModel.ReturnReason = item.Reason;
            if (string.IsNullOrEmpty(item.Reason))
            {
                objReturnedOrderModel.ReturnReason = "OTHER";
                //selectedReason = Filter;
            }
            objReturnedOrderModel.CPCode = item.ParticipantCode;
            //objReturnedOrderModel.OCOTrgRate= item
            objReturnedOrderModel.Yield = item.Yield;
            //objReturnedOrderModel.DirtyPrice = item.D
            //ReturnedOrderCollection.Add(objReturnedOrderModel);
            objReturnedOrderModel.OrderKey = string.Format("{0}_{1}", item.ScripCode, item.OrderId);
            objReturnedOrderModel.SegmentID = item.SegmentFlag;
            if (ReturnedOrderCollection != null && ReturnedOrderCollection.Count > 0)
            {
                if (ReturnedOrderCollection.Any(x => x.OrderKey == item.OrderKey))
                {
                    int index = ReturnedOrderCollection.IndexOf(ReturnedOrderCollection.Where(x => x.OrderKey == item.OrderKey).FirstOrDefault());
                    ReturnedOrderCollection[index] = objReturnedOrderModel;


                }
                else
                {
                    ReturnedOrderCollection.Add(objReturnedOrderModel);
                }
            }
            else
            {
                ReturnedOrderCollection?.Add(objReturnedOrderModel);
            }
        }

        private void populateGridDataFilter(bool byReson, bool byTime)
        {

            try
            {


                if (byReson == true && byTime == false)
                {
                    btnShowAllEnability = true;
                    txtReply = string.Empty;
                    int count = 0;
                    ReturnedOrderCollection?.Clear();
                    if (MemoryManager.OrderDictionary.Values.Where(x => x.InternalOrderStatus == OrderExecutionStatus.Return.ToString()).Count() == 0)
                    {
                        txtReply = "No Records Found";
                    }
                    foreach (OrderModel item in MemoryManager.OrderDictionary.Values.Where(x => x.InternalOrderStatus == OrderExecutionStatus.Return.ToString()))
                    {
                        View.Order.ReturnedOrders oReturnedOrdersWindow = System.Windows.Application.Current.Windows.OfType<View.Order.ReturnedOrders>().FirstOrDefault();
                        //string starttime_LocTemp = oReturnedOrdersWindow.StartTimeText.Text;
                        //string stoptime_locTemp = oReturnedOrdersWindow.StopTimeText.Text;
                        // bool validate = Validate(starttime_LocTemp, stoptime_locTemp);
                        //if (!validate)
                        // {
                        //  return;
                        // }

                        // DateTime starttime_Loc = Convert.ToDateTime(starttime_LocTemp);
                        // DateTime stoptime_loc = Convert.ToDateTime(stoptime_locTemp);
                        // DateTime temp = Convert.ToDateTime(item.Time.Trim());


                        if (selectedReason == "-Select-")
                        {
                            txtReply = "Please Select The Valid Reason.";
                        }
                        else if (item.FilterType == selectedReason)
                        {
                            btnShowAllEnability = true;
                            count = AssignData(item);
                            if (count > 0)
                                txtReply = count + " Record/s Found.";
                        }
                        else
                        {
                            if (count == 0)
                            {
                                txtReply = "Record Not Found.";
                            }
                        }


                    }
                }
                else if (byReson == false && byTime == true)
                {
                    btnShowAllEnability = true;
                    txtReply = string.Empty;
                    int count = 0;
                    ReturnedOrderCollection?.Clear();

                    if (MemoryManager.OrderDictionary.Values.Where(x => x.InternalOrderStatus == OrderExecutionStatus.Return.ToString()).Count() == 0)
                    {
                        txtReply = "No Records Found";
                    }
                    foreach (OrderModel item in MemoryManager.OrderDictionary.Values.Where(x => x.InternalOrderStatus == OrderExecutionStatus.Return.ToString()))
                    {
                        View.Order.ReturnedOrders oReturnedOrdersWindow = System.Windows.Application.Current.Windows.OfType<View.Order.ReturnedOrders>().FirstOrDefault();
                        string starttime_LocTemp = oReturnedOrdersWindow.StartTimeText.Text;
                        string stoptime_locTemp = oReturnedOrdersWindow.StopTimeText.Text;
                        bool validate = Validate(starttime_LocTemp, stoptime_locTemp);
                        if (!validate)
                        {
                            return;
                        }

                        DateTime starttime_Loc = Convert.ToDateTime(starttime_LocTemp);
                        DateTime stoptime_loc = Convert.ToDateTime(stoptime_locTemp);
                        DateTime temp = Convert.ToDateTime(item.Time.Trim());

                        if (temp <= stoptime_loc && temp >= starttime_Loc)
                        {
                            btnShowAllEnability = true;
                            count = AssignData(item);
                            if (count > 0)
                                txtReply = count + " Record/s Found.";
                        }
                        else
                        {
                            if (count == 0)
                            {
                                txtReply = "Record Not Found.";
                            }
                        }
                    }
                }
                else if (byReson == true && byTime == true)
                {
                    btnShowAllEnability = true;
                    txtReply = string.Empty;
                    int count = 0;
                    ReturnedOrderCollection?.Clear();
                    if (MemoryManager.OrderDictionary.Values.Where(x => x.InternalOrderStatus == OrderExecutionStatus.Return.ToString()).Count() == 0)
                    {
                        txtReply = "No Records Found";
                    }
                    foreach (OrderModel item in MemoryManager.OrderDictionary.Values.Where(x => x.InternalOrderStatus == OrderExecutionStatus.Return.ToString()))
                    {
                        View.Order.ReturnedOrders oReturnedOrdersWindow = System.Windows.Application.Current.Windows.OfType<View.Order.ReturnedOrders>().FirstOrDefault();
                        string starttime_LocTemp = oReturnedOrdersWindow.StartTimeText.Text;
                        string stoptime_locTemp = oReturnedOrdersWindow.StopTimeText.Text;
                        bool validate = Validate(starttime_LocTemp, stoptime_locTemp);
                        if (!validate)
                        {
                            return;
                        }

                        DateTime starttime_Loc = Convert.ToDateTime(starttime_LocTemp);
                        DateTime stoptime_loc = Convert.ToDateTime(stoptime_locTemp);
                        DateTime temp = Convert.ToDateTime(item.Time.Trim());

                        if ((item.FilterType == selectedReason) && (temp <= stoptime_loc && temp >= starttime_Loc))
                        {
                            btnShowAllEnability = true;
                            count = AssignData(item);
                            if (count > 0)
                                txtReply = count + " Record/s Found.";
                            // else
                            // txtReply = "No Records Found Under the given time";

                        }
                        else
                        {
                            if (count == 0)
                            {
                                txtReply = "Record Not Found.";
                            }
                        }

                    }
                }
                else
                {
                    btnShowAllEnability = false;
                    System.Windows.MessageBox.Show("Please select a filter option and try again", "Alert", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
            }
        }

        public bool Validate(string startTime, string stoptime)
        {
            bool value = false;
            //Regex rgx = new Regex(@"^(2[0-3]|[01]?[0-9]):([0-5]?[0-9]):([0-5]?[0-9])$");
            if (startTime.ToString() == string.Empty)
            {
                txtReply = "Please Fill Start Time";
                //value = false;
                return false;
            }
            else if (stoptime.ToString() == string.Empty)
            {
                txtReply = "Please Fill End Time";
                //value = false;
                return false;
            }
            //if (!rgx.IsMatch(startTime))
            //{
            //    txtReply = "Start Time should be right Format.";
            //    value = false;
            //}
            //if (!rgx.IsMatch(stoptime))
            //{
            //    txtReply = "End Time should be right Format.";
            //    value = false;
            //}
            DateTime StartDate = Convert.ToDateTime(startTime);
            DateTime EndDate = Convert.ToDateTime(stoptime);
            if (EndDate <= StartDate)
            {
                txtReply = "End Date should be greater than Start Date";
                //value = false;
                return false;
            }
            if (chkReasonFilter == true)
            {
                if (selectedReason == "-Select-")
                {
                    txtReply = "Please Select The valid Reason";
                    //value = false;
                    return false;
                }
            }
            //if(value != false )
            //{
            //    value = true;
            //    txtReply = string.Empty;
            //}
            //txtReply = string.Empty;
            return true;
        }


        private void SaveImageClick_Click()
        {
            try
            {
                if (!string.IsNullOrEmpty(txtClientID))
                {
                    ConvertCSVtoDataTableForWhatApp(1);
                }
                else
                {
                    txtReply = "Provide Valid Client";
                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
            }
        }

        private void EMailClick_CLick()
        {
            try
            {
                if (!string.IsNullOrEmpty(txtClientID))
                {
                    SMTPIP = ReadEmailDetailFromINI();


                    if (LiveSettings == "2") //i.e SMTP, not direct connection 
                    {
                        IsPing = EmailProfilingVM.PingValidation(SMTPIP); //PORT and IP address checks are built   

                        if (IsPing == true && PortStatus == 1)
                        {

                            //foreach (string s in csv_file_path)
                            //{
                            ConvertCSVtoDataTable(1); //for Return Order
                                                      //}
                                                      //Intimation to TWS that all the files are processed  
                        }
                        else
                        {
                            //foreach (string s in csv_file_path)
                            //{
                            //    File.Delete(s);
                            //}
                            txtReply = "Either SMTP IP address ping is not reachable or port is not open";
                            return;
                        }
                        //Win32.PostMessage(CommonUtil.GhTwsWnd, AM_CMSNETPOSSAVE, 1, 0);

                    }
                    else if (LiveSettings == "1")  //direct connection
                    {

                        //foreach (string s in csv_file_path)
                        //{
                        ConvertCSVtoDataTable(1); //for Return Order
                                                  //}
                                                  //Win32.PostMessage(CommonUtil.GhTwsWnd, AM_CMSNETPOSSAVE, 1, 0); //Intimation to TWS that all the files are processed  

                    }
                }
                else
                {
                    txtReply = "Provide Valid Client";
                }

            }
            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
            }


            //throw new NotImplementedException();
        }


        public void ConvertCSVtoDataTable(int flag)
        {
            //MessageBox.Show("1", "information");
            string email, GenTime, ClientID, SettlementNumber = "";
            Boolean bln_validfile = true;
            StringBuilder SB = new StringBuilder();

            DateTime serverTime = DateTime.Now;
            DataTable dt = new DataTable();

            #region NEw 
            string[] headers = { "Client", "Scrip Id", "Scrip Code", "B/S", "Total Qty", "Rev Qty", "Rate", "Order Id", "Client Type", "Retention", "Reason Of Return" };
            foreach (string header in headers)
            {
                dt.Columns.Add(header);
            }

            ReturnedOrderModel oreturnedOrderModel;
            oreturnedOrderModel = ReturnedOrderCollection.ToList().Where(x => x.ClientID.Trim().ToUpper() == txtClientID.Trim().ToUpper()).FirstOrDefault();
            //int BuyQty = 0;
            //double BavgRate = 0;
            //int Sqty = 0;
            //double SAvgRate = 0;
            //int NetQty = 0;
            //double NetValue = 0;
            //string ISINNo = string.Empty;
            //string settlementnumber = string.Empty;

            if (oreturnedOrderModel != null)
            {
                bln_validfile = true;
                //BuyQty = oreturnedOrderModel.B;
                //BavgRate = oreturnedOrderModel.AvgBuyRateIn2Decimal;
                //Sqty = oreturnedOrderModel.SellQty;
                //SAvgRate = oreturnedOrderModel.AvgSellRateIn2Decimal;
                //NetQty = oreturnedOrderModel.NetQty;
                //NetValue = oreturnedOrderModel.NetValue;
                //ISINNo = oreturnedOrderModel.ISINNum;
                //settlementnumber = oreturnedOrderModel.GETInstance.SettlementNo;
            }
            else
            {
                bln_validfile = false;
            }

            string[] rows1 = { txtClientID, oreturnedOrderModel.ScripID, oreturnedOrderModel.SCode.ToString(), oreturnedOrderModel.BuySell,
                                oreturnedOrderModel.TotalQty.ToString(),oreturnedOrderModel.RevQty.ToString(),oreturnedOrderModel.Rate.ToString(),
                                oreturnedOrderModel.OrdID,oreturnedOrderModel.ClientType,oreturnedOrderModel.RetainTill,oreturnedOrderModel.ReturnReason};
            DataRow dr = dt.NewRow();

            string[] rows = { "", "", "", "", "", "", "", "", "", GmailEmailID, DateTime.Now.ToString("ddMMyy") };

            for (int i = 0; i < headers.Length; i++)
            {
                dr[i] = rows[i];
            }
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            for (int i = 0; i < headers.Length; i++)
            {
                dr[i] = rows1[i];
            }

            dt.Rows.Add(dr);

            #endregion

            //using (StreamReader sr = new StreamReader(strFilePath))
            //{
            //    string line;
            //    while ((line = sr.ReadLine()) != null)
            //    {
            //        string[] headers = line.Split(',');
            //        if ((headers.Length == 10 && flag == 0) || (headers.Length == 11 && flag == 1))
            //        {
            //            bln_validfile = true;
            //            foreach (string header in headers)
            //            { dt.Columns.Add(header); }

            //            while (!sr.EndOfStream)
            //            {
            //                string[] rows = sr.ReadLine().Split(',');
            //                DataRow dr = dt.NewRow();
            //                for (int i = 0; i < headers.Length; i++)
            //                {
            //                    dr[i] = rows[i];
            //                }
            //                dt.Rows.Add(dr);
            //            }
            //        }
            //        else
            //        { bln_validfile = false; }
            //        break;
            //    }

            //    if (line == null)
            //    { bln_validfile = false; }
            //}
            if (bln_validfile)
            {
                ClientID = dt.Rows[1].ItemArray[0].ToString();


                email = dt.Rows[0].ItemArray[9].ToString();
                GenTime = dt.Rows[0].ItemArray[10].ToString();

                ReturnOrderMessageBodyFormation(dt, ClientID, SB);


                SB.Append("<font-family:calibri>Generated at: " + serverTime + "(IST)" + "</font>");
                if (flag == 0) //Net Position
                {
                    SB.Append("</b></br><font-family:calibri;text-align:right>Equity Settlement Number: " + SettlementNumber + "</font>");
                }
                SB.Append("<p><font-family:calibri>This communication is for your internal purpose only. In case you find any mismatch/discrepancy in transaction(s), please inform to your trading member only. This email is nonbinding document/ communication and meant for your perusal only. You agree not to refer this communication in any legal proceedings.</font></p>");
                SB.Append("<br><b><font-family:calibri>Thanks & Regards,</font></b></br>");
                SB.Append("<br><font-family:calibri>(This is a System Generated mail)</font></br>");
                SB.Append("<font-family:calibri>BSE: World’s fastest Exchange with a speed of 6 microseconds</font>");

                if (email != "NA")
                {
                    if (LiveSettings == "1") //1 is direct internet connection
                    {
                        EmailFormatForDirectInternetConnection(email, ClientID, flag, SB);
                    }

                    else if (LiveSettings == "2") //SMTP connection
                    {
                        EmailFormatForSMTPConnection(email, ClientID, flag, SB);
                    }

                }
                else
                {
                    //File.Delete(strFilePath); //In case of NA, delete the file from folder.   
                }

            }
            else
            {
                // File.Delete(strFilePath); //In case of junk file, delete the file from folder.
            }

        }

        public void ConvertCSVtoDataTableForWhatApp(int flag)
        {
            SerialNumber = 1;
            serialNumber = 1;
            string ClientID, SettlementNumber = "";
            Boolean bln_validfile = true;
            StringBuilder SBColumn0;
            StringBuilder SBColumn1;
            StringBuilder SBColumn2;
            StringBuilder SBColumn3;
            StringBuilder SBColumn4;

            int PageNo;
            int TotalPages = 1;

            DateTime serverTime = DateTime.Now;
            DataTable dt = new DataTable();
            #region NEw 
            string[] headers = { "Client", "Scrip Id", "Scrip Code", "B/S", "Total Qty", "Rev Qty", "Rate", "Order Id", "Client Type", "Retention", "Reason Of Return" };
            foreach (string header in headers)
            {
                dt.Columns.Add(header);
            }

            ReturnedOrderModel oreturnedOrderModel;
            oreturnedOrderModel = ReturnedOrderCollection.ToList().Where(x => x.ClientID.Trim().ToUpper() == txtClientID.Trim().ToUpper()).FirstOrDefault();
            //int BuyQty = 0;
            //double BavgRate = 0;
            //int Sqty = 0;
            //double SAvgRate = 0;
            //int NetQty = 0;
            //double NetValue = 0;
            //string ISINNo = string.Empty;
            //string settlementnumber = string.Empty;

            if (oreturnedOrderModel != null)
            {
                bln_validfile = true;
                //BuyQty = objCWSWDetailPositionModel.BuyQty;
                //BavgRate = objCWSWDetailPositionModel.AvgBuyRateIn2Decimal;
                //Sqty = objCWSWDetailPositionModel.SellQty;
                //SAvgRate = objCWSWDetailPositionModel.AvgSellRateIn2Decimal;
                //NetQty = objCWSWDetailPositionModel.NetQty;
                //NetValue = objCWSWDetailPositionModel.NetValue;
                //ISINNo = objCWSWDetailPositionModel.ISINNum;
                //settlementnumber = UtilityLoginDetails.GETInstance.SettlementNo;
            }
            else
            {
                bln_validfile = false;
            }

            string[] rows1 = { txtClientID, oreturnedOrderModel.ScripID, oreturnedOrderModel.SCode.ToString(), oreturnedOrderModel.BuySell,
                                oreturnedOrderModel.TotalQty.ToString(),oreturnedOrderModel.RevQty.ToString(),oreturnedOrderModel.Rate.ToString(),
                                oreturnedOrderModel.OrdID,oreturnedOrderModel.ClientType,oreturnedOrderModel.RetainTill,oreturnedOrderModel.ReturnReason};
            DataRow dr = dt.NewRow();

            string[] rows = { "", "", "", "", "", "", "", "", "", GmailEmailID, DateTime.Now.ToString("ddMMyy") };

            for (int i = 0; i < headers.Length; i++)
            {
                dr[i] = rows[i];
            }
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            for (int i = 0; i < headers.Length; i++)
            {
                dr[i] = rows1[i];
            }

            dt.Rows.Add(dr);

            #endregion


            if (bln_validfile)
            {
                ClientID = dt.Rows[1].ItemArray[0].ToString();
                if (flag == 0) //Net Position
                {
                    SettlementNumber = dt.Rows[0].ItemArray[7].ToString();
                    if (SettlementNumber == "NA")
                    {
                        SettlementNumber = "";
                    }
                }
                try
                {
                    int NoOfPages = dt.Rows.Count / 20;
                    int remainder = dt.Rows.Count / 20;
                    dt.Rows[0].Delete();
                    if (NoOfPages >= 1 && remainder != 0)
                    {



                        DataTable[] splittedtables = dt.AsEnumerable()
                                                       .Select((row, index) => new { row, index })
                                                       .GroupBy(x => x.index / 20)  // integer division, the fractional part is truncated
                                                       .Select(g => g.Select(x => x.row).CopyToDataTable())
                                                       .ToArray();
                        TotalPages = splittedtables.Length;
                        for (int i = 0; i < splittedtables.Length; i++)
                        {
                            SBColumn0 = new StringBuilder();
                            SBColumn1 = new StringBuilder();
                            SBColumn2 = new StringBuilder();
                            SBColumn3 = new StringBuilder();
                            SBColumn4 = new StringBuilder();
                            PageNo = i + 1;
                            int count = splittedtables[i].Rows.Count;

                            ReturnOrderForImageCreation(splittedtables[i], ClientID, SBColumn0, SBColumn1, SBColumn2, SBColumn3, SBColumn4, PageNo);


                            CreatingAnImageFromText(count, flag, ClientID, SettlementNumber, SBColumn0, SBColumn1, SBColumn2, SBColumn3, SBColumn4, serverTime, PageNo, TotalPages);

                            //File.Delete(strFilePath);
                        }
                    }
                    else
                    {
                        SBColumn0 = new StringBuilder();
                        SBColumn1 = new StringBuilder();
                        SBColumn2 = new StringBuilder();
                        SBColumn3 = new StringBuilder();
                        SBColumn4 = new StringBuilder();
                        PageNo = 1;


                        ReturnOrderForImageCreation(dt, ClientID, SBColumn0, SBColumn1, SBColumn2, SBColumn3, SBColumn4, PageNo);



                        CreatingAnImageFromText(dt.Rows.Count, flag, ClientID, SettlementNumber, SBColumn0, SBColumn1, SBColumn2, SBColumn3, SBColumn4, serverTime, PageNo, TotalPages);

                    }
                    string MessageInCMW;
                    if (flag == 0)
                    {
                        MessageInCMW = "Net Position";
                    }
                    else
                    {
                        MessageInCMW = "Return Order";
                    }
                    //CmwListDetails cmwErrorSuccess = new CmwListDetails
                    //{
                    //    BoltTime = DateTime.Now.ToString("HH:mm:ss"),
                    //    MsgCategory = Constants.MsgCat.Other,
                    //    MsgPrefix = Constants.MsgPrefix.Other,
                    //    MsgDetail = MessageInCMW + " Image created successfully at '..\\Bolt\\Bolt\\Image' for client:" + ClientID
                    //};
                    //AddRecordGrid(cmwErrorSuccess, 0);
                    // File.Delete(strFilePath);
                }
                catch (Exception ex)
                {
                    // File.Delete(strFilePath); //after sending the mail, delete the file from folder.  
                }

            }

        }

        private void EmailFormatForDirectInternetConnection(string email, string ClientID, int flag, StringBuilder SB)
        {
            // MessageBox.Show("inside  ConvertCSVtoDataTable LiveSettings == 1", "information");
            //MailMessage msg = new MailMessage(GmailEmailID, "ajay.sengar@bseindia.com,cmcbolt@bseindia.com");
            MailMessage msg = new MailMessage(GmailEmailID, email);
            if (flag == 0)
                msg.Subject = "Netposition Summary (Client ID - " + ClientID + ") ";
            else
                msg.Subject = "Unexecuted Order Summary (Client ID - " + ClientID + ") ";

            msg.Body = SB.ToString();
            msg.BodyEncoding = Encoding.UTF8;
            msg.IsBodyHtml = true;

            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            // System.Net.NetworkCredential basicCredential = new System.Net.NetworkCredential(GmailEmailID, GmailPassword);
            client.Credentials = new System.Net.NetworkCredential(GmailEmailID, GmailPassword);
            client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
            //client.Credentials = basicCredential;
            try
            {
                client.Send(msg);
                txtReply = "Email sent successfully to client:" + ClientID;
            }
            catch (IOException ex)
            {
                txtReply = "Unable To connect to the port as it is not open";
            }
            catch (SocketException ex)
            {
                txtReply = "An attempt was made to access a socket in a way forbidden by its access permissions";
            }

            catch (Exception ex)
            {
                bool exceptioncaught = false;

                if (ex.ToString().Contains("The remote name could not be resolved: 'smtp.gmail.com'"))
                {
                    txtReply = "Please check your internet connection";
                    exceptioncaught = true;
                }

                if (ex.ToString().Contains("The SMTP server requires a secure connection or the client was not authenticated."))
                {
                    txtReply = "The  gmail SMTP server requires a secure connection or the client was not authenticated.";
                    exceptioncaught = true;

                }
                if (!exceptioncaught)
                {
                    txtReply = "Email sending unsuccessful to:" + ClientID;
                }
            }
            finally
            {

            }
        }

        private void EmailFormatForSMTPConnection(string email, string ClientID, int flag, StringBuilder SB)
        {

            // MessageBox.Show("inside  ConvertCSVtoDataTable LiveSettings == 2", "information");
            //MailMessage msg = new MailMessage(LiveFromEmail, "ajay.sengar@bseindia.com,cmcbolt@bseindia.com");
            MailMessage msg = new MailMessage(LiveFromEmail, email);
            //msg.Subject = "Netposition Summary (Client ID - ) ";
            if (flag == 0)
                msg.Subject = "Netposition Summary (Client ID - " + ClientID + ") ";
            else
                msg.Subject = "Unexecuted Order Summary (Client ID - " + ClientID + ") ";


            msg.Body = SB.ToString();
            msg.BodyEncoding = Encoding.UTF8;
            msg.IsBodyHtml = true;
            SmtpClient client = new SmtpClient(LiveSMTPIP, Convert.ToInt16(LiveSMTPPort));
            client.UseDefaultCredentials = false;
            System.Net.NetworkCredential basicCredential = new System.Net.NetworkCredential(LiveSMTPEmailID, LiveSMTPPassword);
            client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
            client.Credentials = basicCredential;
            try
            {
                client.Send(msg);
                txtReply = "Email sent successfully to client: " + ClientID;
            }
            catch (Exception ex)
            {
                txtReply = "Email sending unsuccessfull: " + ClientID;
            }
        }

        private void ReturnOrderMessageBodyFormation(DataTable dt, string ClientID, StringBuilder SB)
        {
            dt.Rows[0].Delete();
            SB.Append("<p><font-family:calibri>Dear Investor (Client ID- " + ClientID + ")," + "</font></p>");
            SB.Append("<p><font-family:calibri>Your unexecuted order summary is given below- </font></p>");
            SB.Append("<table>");
            SB.Append("<tr><td style=\"text-align:center;border-bottom:1pt solid green;border-right:1pt solid green;border-left:1pt solid green;border-top:1pt solid green;background-color:#b5dbe8;font-family:calibri\"><b>Scrip Id</b></td><td style=\"text-align:center;border-bottom:1pt solid green;border-right:1pt solid green;border-left:1pt solid green;border-top:1pt solid green;background-color:#b5dbe8;font-family:calibri\"><b>Scrip Code</b></td><td style=\"text-align:center;border-bottom:1pt solid green;border-right:1pt solid green;border-left:1pt solid green;border-top:1pt solid green;background-color:#b5dbe8;font-family:calibri\"><b>BuySell</b></td><td style=\"text-align:center;border-bottom:1pt solid green;border-right:1pt solid green;border-left:1pt solid green;border-top:1pt solid green;background-color:#b5dbe8;font-family:calibri\"><b>Total Qty</b></td><td style=\"text-align:center;border-bottom:1pt solid green;border-right:1pt solid green;border-left:1pt solid green;border-top:1pt solid green;background-color:#b5dbe8;font-family:calibri\"><b>Rev Qty</b></td><td style=\"text-align:center;border-bottom:1pt solid green;border-right:1pt solid green;border-left:1pt solid green;border-top:1pt solid green;background-color:#b5dbe8;font-family:calibri\"><b>Rate</b></td><td style=\"text-align:center;border-bottom:1pt solid green;border-right:1pt solid green;border-left:1pt solid green;border-top:1pt solid green;background-color:#b5dbe8;font-family:calibri\"><b>Order ID</b></td><td style=\"text-align:center;border-bottom:1pt solid green;border-right:1pt solid green;border-left:1pt solid green;border-top:1pt solid green;background-color:#b5dbe8;font-family:calibri\"><b>Client Type</b></td><td style=\"text-align:center;border-bottom:1pt solid green;border-right:1pt solid green;border-left:1pt solid green;border-top:1pt solid green;background-color:#b5dbe8;font-family:calibri\"><b>Retention</b></td><td style=\"text-align:center;border-bottom:1pt solid green;border-right:1pt solid green;border-left:1pt solid green;border-top:1pt solid green;background-color:#b5dbe8;font-family:calibri\"><b>Reason Of Return</b></td></tr>");

            foreach (DataRow dr in dt.Rows)
            {
                SB.Append("<tr><td style=\"text-align:center;border-bottom:1pt solid green;border-right:1pt solid green;border-left:1pt solidgreen;font-family:calibri\">" + dr["Scrip Id"].ToString() + "</td><td style=\"text-align:center;border-bottom:1pt solid green;border-right:1pt solid green;border-left:1pt solid green;font-family:calibri\">" + dr["Scrip Code"].ToString() + "</td><td style=\"text-align:center;border-bottom:1pt solid green;border-right:1pt solid green;border-left:1pt solid green;font-family:calibri\">" + dr["B/S"].ToString() + "</td><td style=\"text-align:center;border-bottom:1pt solid green;border-right:1pt solid green;border-left:1pt solid green;font-family:calibri\">" + dr["Total Qty"].ToString() + "</td><td style=\"text-align:center;border-bottom:1pt solid green;border-right:1pt solid green;border-left:1pt solid green;font-family:calibri\">" + dr["Rev Qty"].ToString() + "</td><td style=\"text-align:center;border-bottom:1pt solid green;border-right:1pt solid green;border-left:1pt solid green;font-family:calibri\">" + dr["Rate"].ToString() + "</td><td style=\"text-align:center;border-bottom:1pt solid green;border-right:1pt solid green;border-left:1pt solid green;font-family:calibri\">" + dr["Order Id"].ToString() + "</td><td style=\"text-align:center;border-bottom:1pt solid green;border-right:1pt solid green;border-left:1pt solid green;font-family:calibri\">" + dr["Client Type"].ToString() + "</td><td style=\"text-align:center;border-bottom:1pt solid green;border-right:1pt solid green;border-left:1pt solid green;font-family:calibri\">" + dr["Retention"].ToString() + "</td><td style=\"text-align:center;border-bottom:1pt solid green;border-right:1pt solid green;border-left:1pt solid green;font-family:calibri\">" + dr["Reason Of Return"].ToString() + "</td></tr>");
            }
            SB.Append("</table>");
        }

        public static string ReadEmailDetailFromINI()
        {
            SMTPIP = MainWindowVM.parser.GetSetting("SMTP EMAIL PROFILING", "TestSMTPServerIP");
            //For Test SMTP : End
            //For Live SMTP: Start
            LiveSMTPIP = MainWindowVM.parser.GetSetting("SMTP EMAIL PROFILING", "LiveSMTPServerIP");
            LiveSMTPPort = MainWindowVM.parser.GetSetting("SMTP EMAIL PROFILING", "LiveSMTPPort");
            LiveSMTPEmailID = MainWindowVM.parser.GetSetting("SMTP EMAIL PROFILING", "LiveSMTPEmailID");
            LiveSMTPPassword = MainWindowVM.parser.GetSetting("SMTP EMAIL PROFILING", "LiveSMTPPassword");
            if (!string.IsNullOrEmpty(LiveSMTPPassword))
            {
                string LiveDecrypted = EmailProfilingVM.EncryptDecrypt(LiveSMTPPassword);
                LiveSMTPPassword = LiveDecrypted;
            }
            LiveFromEmail = MainWindowVM.parser.GetSetting("SMTP EMAIL PROFILING", "LiveFromEmailID");
            LiveSettings = MainWindowVM.parser.GetSetting("SMTP EMAIL PROFILING", "LiveSettings");
            //MessageBox.Show(LiveSettings, "Information");
            //For Live SMTP: End
            //For Direct Internent : Start
            GmailEmailID = MainWindowVM.parser.GetSetting("SMTP EMAIL PROFILING", "GmailID");
            GmailPassword = MainWindowVM.parser.GetSetting("SMTP EMAIL PROFILING", "Password");
            if (!string.IsNullOrEmpty(GmailPassword))
            {
                string GmailDecrypted = EmailProfilingVM.EncryptDecrypt(GmailPassword);
                GmailPassword = GmailDecrypted;
            }
            //For Direct Internent : End


            return SMTPIP;

        }

        private void ReturnOrderForImageCreation(DataTable dt, string ClientID, StringBuilder SBColumn0,
          StringBuilder SBColumn1, StringBuilder SBColumn2, StringBuilder SBColumn3, StringBuilder SBColumn4, int PageNo)
        {
            SBColumn0.Append("Sn" + System.Environment.NewLine);
            SBColumn1.Append("Scrip Id" + System.Environment.NewLine);
            SBColumn2.Append("B/S" + System.Environment.NewLine);
            SBColumn3.Append("Qty" + System.Environment.NewLine);
            SBColumn4.Append("Rate" + System.Environment.NewLine);


            foreach (DataRow dr in dt.Rows)
            {
                if (dr.RowState != DataRowState.Deleted)
                {
                    SBColumn0.Append(SerialNumber + System.Environment.NewLine);
                    SBColumn1.Append(dr["Scrip Id"].ToString() + System.Environment.NewLine);
                    SBColumn2.Append(dr["B/S"].ToString() + System.Environment.NewLine);
                    if (dr["Total Qty"].ToString() == "")
                    {
                        dr["Total Qty"] = "0";
                    }
                    SBColumn3.Append(dr["Total Qty"].ToString() + System.Environment.NewLine);
                    if (dr["Total Qty"] != "0")
                    {
                        SBColumn4.Append(dr["Rate"].ToString() + System.Environment.NewLine);
                    }
                    else
                    {
                        SBColumn4.Append("--" + System.Environment.NewLine);
                    }

                    SerialNumber++;
                }
            }
            //for (int i = 0; i < 2; i++)
            //{
            //    SBColumn0.Append(serialNumber + Environment.NewLine);
            //    SBColumn1.Append("USDINR16JUNE63.50000CE" + Environment.NewLine);
            //    SBColumn2.Append("B" + Environment.NewLine);

            //    SBColumn3.Append("88888888888" + Environment.NewLine);
            //    SBColumn4.Append("88888888888" + Environment.NewLine);
            //    SerialNumber++;
            //}

        }

        private void CreatingAnImageFromText(int Count, int flag, string ClientID, string SettlementNumber, StringBuilder SBColumn0, StringBuilder SBColumn1,
   StringBuilder SBColumn2, StringBuilder SBColumn3, StringBuilder SBColumn4, DateTime serverTime, int PageNo, int TotalPages)
        {
            int y1 = 50;
            for (int i = 0; i < Count; i++)
            {
                y1 = y1 + 18;
            }
            int FinalHeight = y1 + 18;
            //12-font
            //Bitmap bitmap = new Bitmap(630, FinalHeight);
            Bitmap bitmap = new Bitmap(440, FinalHeight);
            // Bitmap bitmap = new Bitmap(600, 300);

            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                Font font = new Font("Arial", 11, System.Drawing.FontStyle.Bold);
                graphics.FillRectangle(new SolidBrush(Color.LightSkyBlue), 0, 0, bitmap.Width, bitmap.Height);
                graphics.FillRectangle(new SolidBrush(Color.LightBlue), 0, 30, bitmap.Width, bitmap.Height);
                int y2 = 46;

                for (int i = 0; i < Count; i++)
                {
                    if (i % 2 == 0)
                    {
                        graphics.FillRectangle(new SolidBrush(Color.MintCream), 0, y2, bitmap.Width, bitmap.Height);
                    }
                    else
                    {
                        graphics.FillRectangle(new SolidBrush(Color.LightGray), 0, y2, bitmap.Width, bitmap.Height);
                    }
                    y2 = y2 + 17;
                }
                graphics.FillRectangle(new SolidBrush(Color.LightSkyBlue), 0, y2, bitmap.Width, bitmap.Height);

                StringFormat stringFormat = new StringFormat();
                stringFormat.Alignment = StringAlignment.Far;
                StringFormat stringFormat1 = new StringFormat();
                stringFormat1.Alignment = StringAlignment.Center;
                Font font1 = new Font("Arial", 15, System.Drawing.FontStyle.Bold);
                if (flag == 0)
                {
                    graphics.DrawString("                   NET POSITION for " + ClientID, font1, new SolidBrush(Color.Black), 0, 0);
                }
                else
                {
                    graphics.DrawString("       RETURN ORDER for " + ClientID + " (End Of Day)", font1, new SolidBrush(Color.Black), 0, 0);
                }
                graphics.DrawString(SBColumn0.ToString(), font, new SolidBrush(Color.Black), 0, 30);
                graphics.DrawString(SBColumn1.ToString(), font, new SolidBrush(Color.Black), 27, 30);
                if (flag == 0)
                {
                    graphics.DrawString(SBColumn2.ToString(), font, new SolidBrush(Color.Black), 329, 30, stringFormat);
                    graphics.DrawString(SBColumn3.ToString(), font, new SolidBrush(Color.Black), 430, 30, stringFormat);
                }
                else
                {
                    graphics.DrawString(SBColumn2.ToString(), font, new SolidBrush(Color.Black), 220, 30);
                    graphics.DrawString(SBColumn3.ToString(), font, new SolidBrush(Color.Black), 329, 30, stringFormat);
                    graphics.DrawString(SBColumn4.ToString(), font, new SolidBrush(Color.Black), 430, 30, stringFormat);
                }

                if (flag == 0)
                {
                    graphics.DrawString("Eq.Set: " + SettlementNumber + "   Date: " + serverTime + "   Image:" + PageNo + "/" + TotalPages, font, new SolidBrush(Color.Black), 0, y1);
                }
                else
                {
                    graphics.DrawString("Date: " + serverTime + "                           Image:" + PageNo + "/" + TotalPages, font, new SolidBrush(Color.Black), 0, y1);
                }
                graphics.Flush();
                font.Dispose();
                graphics.Dispose();
            }


            bool FileCreated = System.IO.Directory.Exists(Directory.GetCurrentDirectory() + "\\Image\\" + DateTime.Now.ToString("ddMMyyyy"));
            //  bool FileCreated = System.IO.Directory.Exists("C:\\BitmapImages");
            string NPorRO;
            if (flag == 0)
            {
                NPorRO = "_NP_";
            }
            else
            {
                NPorRO = "_RO_";
            }
            if (!FileCreated)
            {
                System.IO.Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\Image\\" + DateTime.Now.ToString("ddMMyyyy"));
                //Array file = Directory.GetFiles(Directory.GetCurrentDirectory() + "\\Image\\" + DateTime.Now.ToString("ddMMyyyy"));
                //foreach (string s in file)
                //{
                //    if(s.Contains(ClientID + NPorRO+1))
                //    {
                //    File.Delete(s);
                //    }
                //}
                bitmap.Save(Directory.GetCurrentDirectory() + "\\Image\\" + DateTime.Now.ToString("ddMMyyyy") + "\\" + ClientID + NPorRO + PageNo + ".Jpeg", ImageFormat.Jpeg);
                bitmap.Dispose();
            }
            else
            {
                bitmap.Save(Directory.GetCurrentDirectory() + "\\Image\\" + DateTime.Now.ToString("ddMMyyyy") + "\\" + ClientID + NPorRO + PageNo + ".Jpeg", ImageFormat.Jpeg);
                bitmap.Dispose();
            }
            System.Windows.MessageBox.Show("Image Created: " + Directory.GetCurrentDirectory() + "\\Image\\", "Image", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }


        private void OnReSubmitClick()
        {
            if (UtilityOrderDetails.GETInstance.CurrentOrderEntry.ToLower() == Enumerations.OrderEntryWindow.Normal.ToString().ToLower())
            {
                #region for Normal Order Entry
                if (selectEntireRow != null)
                {
                    txtReply = string.Empty;
                    NormalOrderEntry objNormal = System.Windows.Application.Current.Windows.OfType<NormalOrderEntry>().FirstOrDefault();
                    var key = string.Format("{0}_{1}", selectEntireRow.SCode, selectEntireRow.OnlyOrderID);

                    if (objNormal != null)
                    {
                        if (selectEntireRow.BuySell == "S")
                        {
                            //((OrderEntryVM)objNormal.DataContext).BuySellWindow("SELL");
                            if (objNormal.WindowState == WindowState.Minimized)
                                objNormal.WindowState = WindowState.Normal;

                            if (((NormalOrderEntryVM)objNormal.DataContext).WindowColour != "#FFB3A7")
                                ((NormalOrderEntryVM)objNormal.DataContext).BuySellWindow("SELL");

                        }
                        if (selectEntireRow.BuySell == "B")
                        {
                            if (objNormal.WindowState == WindowState.Minimized)
                                objNormal.WindowState = WindowState.Normal;

                            if (((NormalOrderEntryVM)objNormal.DataContext).WindowColour != "#89C4F4")
                                ((NormalOrderEntryVM)objNormal.DataContext).BuySellWindow("BUY");


                        }
                        ((NormalOrderEntryVM)objNormal.DataContext).PassByNormalReturnOrder(selectEntireRow.SCode, key);
                        objNormal.Focus();
                        objNormal.Show();
                    }
                    else
                    {
                        objNormal = new NormalOrderEntry();
                        objNormal.Owner = System.Windows.Application.Current.MainWindow;
                        //objswift.CmbExcangeType.Focus();

                        if (selectEntireRow.BuySell == "S")
                        {
                            //((OrderEntryVM)objNormal.DataContext).BuySellWindow("SELL");
                            if (((NormalOrderEntryVM)objNormal.DataContext).WindowColour != "#FFB3A7")
                                ((NormalOrderEntryVM)objNormal.DataContext).BuySellWindow("SELL");
                        }
                        if (selectEntireRow.BuySell == "B")
                        {
                            // ((OrderEntryVM)objNormal.DataContext).BuySellWindow("BUY");
                            if (((NormalOrderEntryVM)objNormal.DataContext).WindowColour != "#89C4F4")
                                ((NormalOrderEntryVM)objNormal.DataContext).BuySellWindow("BUY");
                        }

                        objNormal.Activate();
                        objNormal.Show();
                        ((NormalOrderEntryVM)objNormal.DataContext).PassByNormalReturnOrder(selectEntireRow.SCode, key);
                    }
                }
                else
                {
                    txtReply = "Please Select order to Resubmit";
                }
                #endregion
            }
            else if (UtilityOrderDetails.GETInstance.CurrentOrderEntry.ToLower() == Enumerations.OrderEntryWindow.Swift.ToString().ToLower())
            {
                #region for Swift Order Entry
                if (selectEntireRow != null)
                {
                    txtReply = string.Empty;
                    SwiftOrderEntry objswift = System.Windows.Application.Current.Windows.OfType<SwiftOrderEntry>().FirstOrDefault();
                    var key = string.Format("{0}_{1}", selectEntireRow.SCode, selectEntireRow.OnlyOrderID);

                    if (objswift != null)
                    {
                        if (selectEntireRow.BuySell == "S")
                        {
                            ((OrderEntryVM)objswift.DataContext).BuySellWindow("SELL");

                        }
                        if (selectEntireRow.BuySell == "B")
                        {
                            ((OrderEntryVM)objswift.DataContext).BuySellWindow("BUY");
                        }
                        ((OrderEntryVM)objswift.DataContext).PassByReturnOrder(selectEntireRow.SCode, key);
                        objswift.Focus();
                        objswift.Show();
                    }
                    else
                    {
                        objswift = new SwiftOrderEntry();
                        objswift.Owner = System.Windows.Application.Current.MainWindow;
                        //objswift.CmbExcangeType.Focus();

                        if (selectEntireRow.BuySell == "S")
                        {
                            ((OrderEntryVM)objswift.DataContext).BuySellWindow("SELL");
                        }
                        if (selectEntireRow.BuySell == "B")
                        {
                            ((OrderEntryVM)objswift.DataContext).BuySellWindow("BUY");
                        }

                        objswift.Activate();
                        objswift.Show();
                        ((OrderEntryVM)objswift.DataContext).PassByReturnOrder(selectEntireRow.SCode, key);
                    }
                }
                else
                {
                    txtReply = "Please Select order to Resubmit";
                }
                #endregion
            }
        }

        private void OnBatchReSubmitClick()
        {
            try
            {
                if (ReturnedOrderCollection.Count == 0)
                {
                    System.Windows.MessageBox.Show("No Returned Orders to save.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                txtReply = string.Empty;
                SaveFileDialog objFileDialogBatchResub = new SaveFileDialog();
                objFileDialogBatchResub.InitialDirectory = Path.Combine(Path.GetDirectoryName(Path.GetFullPath(Path.Combine(System.Environment.CurrentDirectory, @"User/"))));
                if (!Directory.Exists(objFileDialogBatchResub.InitialDirectory))
                    Directory.CreateDirectory(objFileDialogBatchResub.InitialDirectory);

                //objFileDialogBatchResub.Title = "Browse CSV Files";
                objFileDialogBatchResub.DefaultExt = "csv";
                string Filter = "CSV files (*.csv)|*.csv";
                objFileDialogBatchResub.Filter = Filter;
                const string header = "Buy/Sell, Qty,Rev.Qty,Scrip Code,Rate,Short/Client ID,Retention Status,Client Type,Order Type,CP Code";
                StreamWriter writer = null;
                //objFileDialogBatchResub.ShowDialog();
                if (objFileDialogBatchResub.ShowDialog() == DialogResult.OK)
                {
                    Filter = objFileDialogBatchResub.FileName;

                    writer = new StreamWriter(Filter, false, Encoding.UTF8);

                    writer.WriteLine(header);
                    foreach (var item in ReturnedOrderCollection)
                    {
                        string Segment = CommonFunctions.GetSegmentID(item.SCode);
                        int DecimalPoint = CommonFunctions.GetDecimal(Convert.ToInt32(item.SCode), "BSE", Segment);
                        string Rate = !string.IsNullOrEmpty(item.Rate) ? Convert.ToString(Convert.ToDouble(item.Rate) * Math.Pow(10, DecimalPoint)) : string.Empty;
                        if (Segment == Enumerations.Segment.Equity.ToString())
                            writer.WriteLine($"{item.BuySell}, {item.TotalQty}, {item.RevQty}, {item.SCode}, {Rate}, {item.ClientID}, {item.RetainTill},{item.ClientType},{item.OrderType},{item.CPCode}");
                        else if (Segment == Enumerations.Segment.Derivative.ToString())
                            writer.WriteLine($"{item.BuySell}, {item.TotalQty}, {item.RevQty}, {item.SCode}, {Rate}, {item.ClientID}, {item.RetainTill},{item.ClientType},{item.OrderType},{item.CPCode}");
                        else if (Segment == Enumerations.Segment.Currency.ToString())
                            writer.WriteLine($"{item.BuySell}, {item.TotalQty}, {item.RevQty}, {item.SCode}, {Rate}, {item.ClientID}, {item.RetainTill},{item.ClientType},{item.OrderType},{item.CPCode}");
                    }
                    writer.Close();

                    txtReply = "File Saved Successfully";
                }
            }
            catch (Exception e)
            {
                ExceptionUtility.LogError(e);
            }

        }

        private void ExecuteMyCommand()
        {
            StreamWriter writer = null;

            if (Global.UtilityTradeDetails.GetInstance.SelectedID == -1)
            {
                if (ReturnedOrderCollection.Count == 0)
                {
                    System.Windows.MessageBox.Show("No Return Order Present to Save","Information", MessageBoxButton.OK,MessageBoxImage.Information);
                    return;
                }
                else
                {
                    SaveFileDialog dlg = new SaveFileDialog();
                    dlg.InitialDirectory = Path.Combine(Path.GetDirectoryName(Path.GetFullPath(Path.Combine(System.Environment.CurrentDirectory, @"User/"))));
                    if (!Directory.Exists(dlg.InitialDirectory))
                        Directory.CreateDirectory(dlg.InitialDirectory);

                    //objFileDialogBatchResub.Title = "Browse CSV Files";
                    dlg.DefaultExt = "csv";
                    string Filter = "CSV files (*.csv)|*.csv";
                    dlg.Filter = Filter;
                    Nullable<bool> result = (dlg.ShowDialog() == DialogResult.OK);
                    if (result == true)
                    {
                        try
                        {
                            writer = new StreamWriter(dlg.FileName, false, Encoding.UTF8);


                            //writer.Write(dr.BuySell + "," + dr.TotalQty + "," + dr.RevQty + "," + dr.SCode + "," + dr.ScripID + "," + Rate + "," + dr.ClientID + "," +
                            //    dr.Time + "," + dr.OrdID + "," + dr.ClientType + "," + dr.RetainTill + "," + dr.ReturnReason + "," + dr.CPCode + "," + dr.OCOTrgRate + "," + dr.Yield + "," + dr.DirtyPrice);

                            writer.Write("Buy/Sell, TotalQty, RevQty, SCode, ScripID, Rate, ClientID, Time, OrdID, ClientType, RetainTill,Reason of Return, CPCode, OCOTrgRate, Yield, DirtyPrice");
                            writer.Write(writer.NewLine);
                            foreach (var dr in ReturnedOrderCollection)
                            {
                                int DecimalPoint = CommonFunctions.GetDecimal(Convert.ToInt32(dr.SCode), "BSE", CommonFunctions.GetSegmentID(dr.SCode));
                                string Rate = !string.IsNullOrEmpty(dr.Rate) ? (Convert.ToDouble(dr.Rate) * Math.Pow(10, DecimalPoint)).ToString() : string.Empty;
                                writer.Write(dr.BuySell + "," + dr.TotalQty + "," + dr.RevQty + "," + dr.SCode + "," + dr.ScripID + "," + Rate + "," + dr.ClientID + "," +
                                    dr.Time + "," + dr.OrdID + "," + dr.ClientType + "," + dr.RetainTill + "," + dr.ReturnReason + "," + dr.CPCode + "," + dr.OCOTrgRate + "," + dr.Yield + "," + dr.DirtyPrice);

                                writer.Write(writer.NewLine);
                            }
                            System.Windows.MessageBox.Show("All Returned Orders Saved in file :" + dlg.FileName.ToString(), "Information", MessageBoxButton.OK, MessageBoxImage.Warning);

                        }
                        catch (IOException oioExption)
                        {
                            ExceptionUtility.LogError(oioExption);
                            System.Windows.MessageBox.Show("The File is open in another application. close it and try agian.", "Information", MessageBoxButton.OK, MessageBoxImage.Warning);

                        }
                        catch (Exception e)
                        {
                            ExceptionUtility.LogError(e);
                            System.Windows.MessageBox.Show("Error in Exporting data in CSV Format", " Error!", MessageBoxButton.OK, MessageBoxImage.Error);
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
                }
            }
            else if (Global.UtilityTradeDetails.GetInstance.SelectedID == 1)
            {
                var i = ReturnedOrderCollection.Where(a => a.SegmentID == 1).Count();
                if (i == 0)
                {
                    System.Windows.MessageBox.Show("No Equity Orders to Save.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    SaveFileDialog dlg = new SaveFileDialog();
                    dlg.InitialDirectory = Path.Combine(Path.GetDirectoryName(Path.GetFullPath(Path.Combine(System.Environment.CurrentDirectory, @"User/"))));
                    if (!Directory.Exists(dlg.InitialDirectory))
                        Directory.CreateDirectory(dlg.InitialDirectory);

                    //objFileDialogBatchResub.Title = "Browse CSV Files";
                    dlg.DefaultExt = "csv";
                    string Filter = "CSV files (*.csv)|*.csv";
                    dlg.Filter = Filter;
                    Nullable<bool> result = (dlg.ShowDialog() == DialogResult.OK);
                    if (result == true)
                    {
                        try
                        {
                            writer = new StreamWriter(dlg.FileName, false, Encoding.UTF8);

                          
                            writer.Write("Buy/Sell, TotalQty, RevQty, SCode, ScripID, Rate, ClientID, Time, OrdID, ClientType, RetainTill,Reason of Return, CPCode, OCOTrgRate, Yield, DirtyPrice");
                            writer.Write(writer.NewLine);
                            foreach (var dr in ReturnedOrderCollection)
                            {
                                if (dr.SegmentID == Global.UtilityTradeDetails.GetInstance.SelectedID)
                                {
                                    int DecimalPoint = CommonFunctions.GetDecimal(Convert.ToInt32(dr.SCode), "BSE", CommonFunctions.GetSegmentID(dr.SCode));
                                    string Rate = !string.IsNullOrEmpty(dr.Rate) ? (Convert.ToDouble(dr.Rate) * Math.Pow(10, DecimalPoint)).ToString() : string.Empty;

                                    writer.Write(dr.BuySell + "," + dr.TotalQty + "," + dr.RevQty + "," + dr.SCode + "," + dr.ScripID + "," + Rate + "," + dr.ClientID + "," +
                                        dr.Time + "," + dr.OrdID + "," + dr.ClientType + "," + dr.RetainTill + "," + dr.ReturnReason + "," + dr.CPCode + "," + dr.OCOTrgRate + "," + dr.Yield + "," + dr.DirtyPrice);

                                    writer.Write(writer.NewLine);
                                }
                            }
                            System.Windows.MessageBox.Show("All Returned Equity Orders Saved in file :" + dlg.FileName.ToString(), "Information", MessageBoxButton.OK, MessageBoxImage.Warning);

                        }
                        catch (IOException oioExption)
                        {
                            ExceptionUtility.LogError(oioExption);
                            System.Windows.MessageBox.Show("The File is open in another application. close it and try agian.", "Information", MessageBoxButton.OK, MessageBoxImage.Warning);

                        }
                        catch (Exception e)
                        {
                            ExceptionUtility.LogError(e);
                            System.Windows.MessageBox.Show("Error in Exporting data in CSV Format", " Error!", MessageBoxButton.OK, MessageBoxImage.Error);
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
                }
            }

            else if (Global.UtilityTradeDetails.GetInstance.SelectedID == 2)
            {
                var i = ReturnedOrderCollection.Where(a => a.SegmentID == 2).Count();
                if (i == 0)
                {
                    System.Windows.MessageBox.Show("No Derivative Returned  Orders to Save.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    SaveFileDialog dlg = new SaveFileDialog();
                    dlg.InitialDirectory = Path.Combine(Path.GetDirectoryName(Path.GetFullPath(Path.Combine(System.Environment.CurrentDirectory, @"User/"))));
                    if (!Directory.Exists(dlg.InitialDirectory))
                        Directory.CreateDirectory(dlg.InitialDirectory);

                    //objFileDialogBatchResub.Title = "Browse CSV Files";
                    dlg.DefaultExt = "csv";
                    string Filter = "CSV files (*.csv)|*.csv";
                    dlg.Filter = Filter;
                    Nullable<bool> result = (dlg.ShowDialog() == DialogResult.OK);
                    if (result == true)
                    {
                        try
                        {
                            writer = new StreamWriter(dlg.FileName, false, Encoding.UTF8);
                            

                            writer.Write("Buy/Sell, TotalQty, RevQty, SCode, ScripID, Rate, ClientID, Time, OrdID, ClientType, RetainTill,Reason of Return, CPCode, OCOTrgRate, Yield, DirtyPrice");
                            writer.Write(writer.NewLine);
                            foreach (var dr in ReturnedOrderCollection)
                            {
                                if (dr.SegmentID == Global.UtilityTradeDetails.GetInstance.SelectedID)
                                {
                                    int DecimalPoint = CommonFunctions.GetDecimal(Convert.ToInt32(dr.SCode), "BSE", CommonFunctions.GetSegmentID(dr.SCode));
                                    string Rate = !string.IsNullOrEmpty(dr.Rate) ? (Convert.ToDouble(dr.Rate) * Math.Pow(10, DecimalPoint)).ToString() : string.Empty;

                                    writer.Write(dr.BuySell + "," + dr.TotalQty + "," + dr.RevQty + "," + dr.SCode + "," + dr.ScripID + "," + Rate + "," + dr.ClientID + "," +
                                        dr.Time + "," + dr.OrdID + "," + dr.ClientType + "," + dr.RetainTill + "," + dr.ReturnReason + "," + dr.CPCode + "," + dr.OCOTrgRate + "," + dr.Yield + "," + dr.DirtyPrice);

                                    writer.Write(writer.NewLine);
                                }
                            }
                            System.Windows.MessageBox.Show("All Derivatives Equity Orders Saved in file :" + dlg.FileName.ToString(), "Message", MessageBoxButton.OK, MessageBoxImage.Information);

                        }
                        catch (IOException oioExption)
                        {
                            ExceptionUtility.LogError(oioExption);
                            System.Windows.MessageBox.Show("The File is open in another application. close it and try agian.", "Information", MessageBoxButton.OK, MessageBoxImage.Warning);

                        }
                        catch (Exception e)
                        {
                            ExceptionUtility.LogError(e);
                            System.Windows.MessageBox.Show("Error in Exporting data in CSV Format", " Error!", MessageBoxButton.OK, MessageBoxImage.Error);
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

                }
            }


            else if (Global.UtilityTradeDetails.GetInstance.SelectedID == 3)
            {
                var i = ReturnedOrderCollection.Where(a => a.SegmentID == 3).Count();
                if (i == 0)
                {
                    System.Windows.MessageBox.Show("No Currency Returned Orders to Save.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    SaveFileDialog dlg = new SaveFileDialog();
                    dlg.InitialDirectory = Path.Combine(Path.GetDirectoryName(Path.GetFullPath(Path.Combine(System.Environment.CurrentDirectory, @"User/"))));
                    if (!Directory.Exists(dlg.InitialDirectory))
                        Directory.CreateDirectory(dlg.InitialDirectory);

                    //objFileDialogBatchResub.Title = "Browse CSV Files";
                    dlg.DefaultExt = "csv";
                    string Filter = "CSV files (*.csv)|*.csv";
                    dlg.Filter = Filter;
                    Nullable<bool> result = (dlg.ShowDialog() == DialogResult.OK);
                    if (result == true)
                    {
                        try
                        {
                            writer = new StreamWriter(dlg.FileName, false, Encoding.UTF8);

                            writer.Write("Buy/Sell, TotalQty, RevQty, SCode, ScripID, Rate, ClientID, Time, OrdID, ClientType, RetainTill,Reason of Return, CPCode, OCOTrgRate, Yield, DirtyPrice");
                            writer.Write(writer.NewLine);
                            foreach (var dr in ReturnedOrderCollection)
                            {
                                if (dr.SegmentID == Global.UtilityTradeDetails.GetInstance.SelectedID)
                                {
                                    int DecimalPoint = CommonFunctions.GetDecimal(Convert.ToInt32(dr.SCode), "BSE", CommonFunctions.GetSegmentID(dr.SCode));
                                    string Rate = !string.IsNullOrEmpty(dr.Rate) ? (Convert.ToDouble(dr.Rate) * Math.Pow(10, DecimalPoint)).ToString() : string.Empty;

                                    writer.Write(dr.BuySell + "," + dr.TotalQty + "," + dr.RevQty + "," + dr.SCode + "," + dr.ScripID + "," + Rate + "," + dr.ClientID + "," +
                                        dr.Time + "," + dr.OrdID + "," + dr.ClientType + "," + dr.RetainTill + "," + dr.ReturnReason + "," + dr.CPCode + "," + dr.OCOTrgRate + "," + dr.Yield + "," + dr.DirtyPrice);

                                    writer.Write(writer.NewLine);
                                }
                            }
                            System.Windows.MessageBox.Show("Currency Returned Orders Saved in file :" + dlg.FileName.ToString(), "Message", MessageBoxButton.OK, MessageBoxImage.Information);

                        }
                        catch (IOException oioExption)
                        {
                            ExceptionUtility.LogError(oioExption);
                            System.Windows.MessageBox.Show("The File is open in another application. close it and try agian.", "Information", MessageBoxButton.OK, MessageBoxImage.Warning);

                        }
                        catch (Exception e)
                        {
                            ExceptionUtility.LogError(e);
                            System.Windows.MessageBox.Show("Error in Exporting data in CSV Format", " Error!", MessageBoxButton.OK, MessageBoxImage.Error);
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
                }
            }


        }


        private void CloseWindowsOnEscape_Click()
        {
            mWindow?.Close();
        }

        #endregion


    }

    public partial class ReturnedOrderVM : BaseViewModel
    {
#if TWS

#endif
    }


    public partial class ReturnedOrderVM : BaseViewModel
    {
#if BOW

#endif
    }

}
