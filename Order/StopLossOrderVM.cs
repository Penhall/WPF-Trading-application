using CommonFrontEnd.Common;
using CommonFrontEnd.Global;
using CommonFrontEnd.Model.Order;
using CommonFrontEnd.Processor.Order;
using CommonFrontEnd.SharedMemories;
using CommonFrontEnd.View;
using CommonFrontEnd.View.Order;
using CommonFrontEnd.View.UserControls;
using CommonFrontEnd.ViewModel;
using CommonFrontEnd.ViewModel.Profiling;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using static CommonFrontEnd.Common.Enumerations;

namespace CommonFrontEnd.ViewModel.Order
{
    public partial class StopLossOrderVM : BaseViewModel
    {
        #region Memmory
        private static ObservableCollection<StopLossModel> _StopLossOrderCollection = new ObservableCollection<StopLossModel>();

        public static ObservableCollection<StopLossModel> StopLossOrderCollection
        {
            get { return _StopLossOrderCollection; }
            set { _StopLossOrderCollection = value; NotifyStaticPropertyChanged(nameof(StopLossOrderCollection)); }
        }

        private ObservableCollection<string> _cmbUpdateList;

        public ObservableCollection<string> cmbUpdateList
        {
            get { return _cmbUpdateList; }
            set { _cmbUpdateList = value; NotifyPropertyChanged(nameof(cmbUpdateList)); }
        }

        private ObservableCollection<string> _cmbOrderTypeList;

        public ObservableCollection<string> cmbOrderTypeList
        {
            get { return _cmbOrderTypeList; }
            set { _cmbOrderTypeList = value; NotifyPropertyChanged(nameof(cmbOrderTypeList)); }
        }
        private ObservableCollection<string> _BuySellList;

        public ObservableCollection<string> BuySellList
        {
            get { return _BuySellList; }
            set { _BuySellList = value; NotifyPropertyChanged(nameof(BuySellList)); }
        }


        private ObservableCollection<string> _cmbClientTypeList;

        public ObservableCollection<string> cmbClientTypeList
        {
            get { return _cmbClientTypeList; }
            set { _cmbClientTypeList = value; NotifyPropertyChanged(nameof(cmbClientTypeList)); }
        }

        private ObservableCollection<string> _cmbRetainTillList;

        public ObservableCollection<string> cmbRetainTillList
        {
            get { return _cmbRetainTillList; }
            set { _cmbRetainTillList = value; NotifyPropertyChanged(nameof(cmbRetainTillList)); }
        }

        private StopLossModel _selectEntireRow;

        public StopLossModel selectEntireRow
        {
            get { return _selectEntireRow; }
            set
            {
                _selectEntireRow = value; NotifyPropertyChanged(nameof(selectEntireRow));
                AssignDataToForm();
            }
        }


        public void AssignDataToForm()
        {
            try
            {
                if (selectEntireRow != null)
                {

                    textBuySell = selectEntireRow.BuySell;
                    selectedUpdateStatus = "MODIFY";
                    if (selectEntireRow.LimitRate.StartsWith("M"))
                    {
                        selectedOrderType = "SL-Market";

                        txtLmtRate = "0";
                    }
                    else
                    {
                        selectedOrderType = "SL-Limit";
                        txtLmtRate = selectEntireRow.LimitRate;
                    }
                    textScripCodeOrId = Convert.ToString(selectEntireRow.SCode);

                    //txtMarketProtection = UtilityOrderDetails.GETInstance.MktProtection == null ? "1.0" : UtilityOrderDetails.GETInstance.MktProtection;
                    if (selectEntireRow.LimitRate.StartsWith("M("))
                    {
                        // txtMarketProtection = selectEntireRow.LimitRate.Substring(2,4);
                        string[] stringArr = selectEntireRow.LimitRate.Split('(');
                        string[] stringArr2 = stringArr[1].Split('%');
                        txtMarketProtection = stringArr2[0];
                    }
                    else
                    {
                        txtMarketProtection = UtilityOrderDetails.GETInstance.MktProtection == null ? "1.0" : UtilityOrderDetails.GETInstance.MktProtection;
                    }
                    textTotlQty = Convert.ToString(selectEntireRow.TotalQty);
                    textRevQty = Convert.ToString(selectEntireRow.RevQty);

                    txtTrgRate = selectEntireRow.TriggertRate;
                    // txtTrgRate = Convert.ToInt64(selectEntireRow.TriggertRate);
                    txtShortClientId = selectEntireRow.ClientID;
                    SelectedClientType = selectEntireRow.ClientType;
                    txtCPCode = selectEntireRow.CPCode;
                    txtReply = string.Empty;
                    int segment = CommonFunctions.GetSegmentFromScripCode(selectEntireRow.SCode);
                    switch (segment)
                    {
                        case 1:
                            if (Limit.g_Limit != null)
                                titleStopLossOrderEntry = string.Format("Stop Loss Order Entry EQX - Buy Limit: [{0:0.00} L], Sell Limit: [{1:0.00} L]", Limit.g_Limit[CommonFunctions.GetSegment(601)].AvailGrossBuyLimit / 100000, Limit.g_Limit[CommonFunctions.GetSegment(601)].AvailGrossSellLimit / 100000);
                            break;
                        case 2:
                            if (Limit.g_Limit != null)
                                titleStopLossOrderEntry = string.Format("Stop Loss Order Entry EDX - Buy Limit: [{0:0.00} L], Sell Limit: [{1:0.00} L]", Limit.g_Limit[CommonFunctions.GetSegment(602)].AvailGrossBuyLimit / 100000, Limit.g_Limit[CommonFunctions.GetSegment(602)].AvailGrossSellLimit / 100000);
                            break;
                        case 3:
                            if (Limit.g_Limit != null)
                                titleStopLossOrderEntry = string.Format("Stop Loss Order Entry CDX - Buy Limit: [{0:0.00} L], Sell Limit: [{1:0.00} L]", Limit.g_Limit[CommonFunctions.GetSegment(603)].AvailGrossBuyLimit / 100000, Limit.g_Limit[CommonFunctions.GetSegment(603)].AvailGrossSellLimit / 100000);
                            break;
                        default:
                            titleStopLossOrderEntry = string.Format("Stop Loss Order Entry");
                            break;
                    }

                }
            }
            catch (Exception e)
            {
                ExceptionUtility.LogError(e);
                throw;
            }

        }


        #endregion

        #region Properties
        static StopLossOrderEntry mWindow = null;
        public static AutoResetEvent StopLossEvent = new AutoResetEvent(false);
        Thread StoplossBackGroundThread = null;

        private string _textBuySell;

        public string textBuySell
        {
            get { return _textBuySell; }
            set { _textBuySell = value; NotifyPropertyChanged(nameof(textBuySell)); }
        }

        private string _titleStopLossOrderEntry = string.Format("Stop Loss Order Entry EQX - Buy Limit: [{0:0.00} L], Sell Limit: [{1:0.00} L]", Limit.g_Limit[CommonFunctions.GetSegment(601)].AvailGrossBuyLimit / 100000, Limit.g_Limit[CommonFunctions.GetSegment(601)].AvailGrossSellLimit / 100000);

        public string titleStopLossOrderEntry
        {
            get { return _titleStopLossOrderEntry; }
            set { _titleStopLossOrderEntry = value; NotifyPropertyChanged(nameof(titleStopLossOrderEntry)); }
        }


        private string _selectedUpdateStatus;

        public string selectedUpdateStatus
        {
            get { return _selectedUpdateStatus; }
            set { _selectedUpdateStatus = value; NotifyPropertyChanged(nameof(selectedUpdateStatus)); PopulateOrderType(); }
        }

        private int _DecimalPoint;
        public int DecimalPoint
        {
            get { return _DecimalPoint; }
            set
            {
                _DecimalPoint = value;
                NotifyPropertyChanged("DecimalPoint");

            }
        }


        private string _selectedOrderType;

        public string selectedOrderType
        {
            get { return _selectedOrderType; }
            set { _selectedOrderType = value; NotifyPropertyChanged(nameof(selectedOrderType)); OnOrderTypeEnable(); }
        }


        private string _textScripCodeOrId;

        public string textScripCodeOrId
        {
            get { return _textScripCodeOrId; }
            set { _textScripCodeOrId = value; NotifyPropertyChanged(nameof(textScripCodeOrId)); }
        }

        private string _textTotlQty;

        public string textTotlQty
        {
            get { return _textTotlQty; }
            set { _textTotlQty = value; NotifyPropertyChanged(nameof(textTotlQty)); }
        }

        private string _textRevQty;

        public string textRevQty
        {
            get { return _textRevQty; }
            set { _textRevQty = value; NotifyPropertyChanged(nameof(textRevQty)); }
        }


        private string _SelectedClientType;

        public string SelectedClientType
        {
            get { return _SelectedClientType; }
            set { _SelectedClientType = value; NotifyPropertyChanged(nameof(SelectedClientType)); OnSelectionClientType(); }
        }



        private string _txtLmtRate;

        public string txtLmtRate
        {
            get { return _txtLmtRate; }
            set { _txtLmtRate = value; NotifyPropertyChanged(nameof(txtLmtRate)); }

        }

        private string _txtMarketProtection;

        public string txtMarketProtection
        {
            get { return _txtMarketProtection; }
            set { _txtMarketProtection = value; NotifyPropertyChanged(nameof(txtMarketProtection)); }
        }
        OrderModel omodel = new OrderModel();

        private string _txtTrgRate;

        public string txtTrgRate
        {
            get { return _txtTrgRate; }
            set { _txtTrgRate = value; NotifyPropertyChanged(nameof(txtTrgRate)); }
        }

        private string _txtShortClientId;

        public string txtShortClientId
        {
            get { return _txtShortClientId; }
            set { _txtShortClientId = value; NotifyPropertyChanged(nameof(txtShortClientId)); }
        }

        private string _txtCPCode = string.Empty;

        public string txtCPCode
        {
            get { return _txtCPCode; }
            set { _txtCPCode = value; NotifyPropertyChanged(nameof(txtCPCode)); }
        }
        private string _selectedRetainTill;

        public string selectedRetainTill
        {
            get { return _selectedRetainTill; }
            set { _selectedRetainTill = value; NotifyPropertyChanged(nameof(selectedRetainTill)); }
        }

        private bool _isEnableLmtRate;

        public bool isEnableLmtRate
        {
            get { return _isEnableLmtRate; }
            set { _isEnableLmtRate = value; NotifyPropertyChanged(nameof(isEnableLmtRate)); }
        }

        private bool _isEnableTrgRate;

        public bool isEnableTrgRate
        {
            get { return _isEnableTrgRate; }
            set { _isEnableTrgRate = value; NotifyPropertyChanged(nameof(isEnableTrgRate)); }
        }



        private bool _isEnableScripCodeORID;

        public bool isEnableScripCodeORID
        {
            get { return _isEnableScripCodeORID; }
            set { _isEnableScripCodeORID = value; NotifyPropertyChanged(nameof(isEnableScripCodeORID)); }
        }

        private bool _isEnableBuySell;

        public bool isEnableBuySell
        {
            get { return _isEnableBuySell; }
            set { _isEnableBuySell = value; NotifyPropertyChanged(nameof(isEnableBuySell)); }
        }

        private bool _isEnableCPCode;

        public bool isEnableCPCode
        {
            get { return _isEnableCPCode; }
            set { _isEnableCPCode = value; NotifyPropertyChanged(nameof(isEnableCPCode)); }
        }

        private bool _isEnableShortClientID;

        public bool isEnableShortClientID
        {
            get { return _isEnableShortClientID; }
            set { _isEnableShortClientID = value; NotifyPropertyChanged(nameof(isEnableShortClientID)); }
        }



        string Validate_Message = string.Empty;

        private string _txtReply;

        public string txtReply
        {
            get { return _txtReply; }
            set { _txtReply = value; NotifyPropertyChanged(nameof(txtReply)); }
        }

        private List<StopLossModel> _SelectedItemLists = new List<StopLossModel>();

        public List<StopLossModel> SelectedItemLists
        {
            get { return _SelectedItemLists; }
            set { _SelectedItemLists = value; NotifyPropertyChanged(nameof(SelectedItemLists)); }
        }

        private string _btnSelectAllContent;

        public string btnSelectAllContent
        {
            get { return _btnSelectAllContent; }
            set { _btnSelectAllContent = value; NotifyPropertyChanged(nameof(btnSelectAllContent)); }
        }

        private ObservableCollection<string> _btnActionList;

        public ObservableCollection<string> btnActionList
        {
            get { return _btnActionList; }
            set { _btnActionList = value; NotifyPropertyChanged(nameof(btnActionList)); }
        }

        private string _SelectedAction;

        public string SelectedAction
        {
            get { return _SelectedAction; }
            set { _SelectedAction = value; NotifyPropertyChanged(nameof(SelectedAction)); }
        }

        //private Brush _DataGridBgColor;

        //public Brush DataGridBgColor
        //{
        //    get { return _DataGridBgColor; }
        //    set { _DataGridBgColor = value; NotifyPropertyChanged(nameof(DataGridBgColor)); }
        //}

        private Brush _BuyForegroundColor;

        public Brush BuyForegroundColor
        {
            get { return _BuyForegroundColor; }
            set { _BuyForegroundColor = value; NotifyPropertyChanged(nameof(BuyForegroundColor)); }
        }

        private Brush _SellForegroundColor;

        public Brush SellForegroundColor
        {
            get { return _SellForegroundColor; }
            set { _SellForegroundColor = value; NotifyPropertyChanged(nameof(SellForegroundColor)); }
        }

        #endregion

        #region RelayCommand
        private RelayCommand _txtBuySell_TextChanged;

        public RelayCommand txtBuySell_TextChanged
        {
            get
            {
                return _txtBuySell_TextChanged ?? (_txtBuySell_TextChanged = new RelayCommand((object e) => BuySellTextValidation(e)));
            }

        }

        private RelayCommand _txtTotlQty_TextChanged;

        public RelayCommand txtTotlQty_TextChanged
        {
            get { return _txtTotlQty_TextChanged ?? (_txtTotlQty_TextChanged = new RelayCommand((object e) => TotalQtyValidation(e))); }
        }

        private RelayCommand _txtRevQty_TextChanged;

        public RelayCommand txtRevQty_TextChanged
        {
            get { return _txtRevQty_TextChanged ?? (_txtRevQty_TextChanged = new RelayCommand((object e) => textRevQtyValidation(e))); }
        }




        private RelayCommand _btnSubmit;

        public RelayCommand btnSubmit
        {
            get
            {
                return _btnSubmit ?? (_btnSubmit = new RelayCommand(
                    (object e) => btnSubmitClick(e)));
            }

        }

        private RelayCommand _DataGridLostFocus;

        public RelayCommand DataGridLostFocus
        {
            get
            {
                return _DataGridLostFocus ?? (_DataGridLostFocus = new RelayCommand(
                    (object e) => AssignDataToForm()));
            }

        }

        private RelayCommand _btnSelectAll;

        public RelayCommand btnSelectAll
        {
            get
            {
                return _btnSelectAll ?? (_btnSelectAll = new RelayCommand(
                    (object e) => btnSelectAll_Click()));
            }

        }

        private RelayCommand _btnApply;

        public RelayCommand btnApply
        {
            get
            {
                return _btnApply ?? (_btnApply = new RelayCommand(
                    (object e) => btnApply_Click()));
            }

        }

        private RelayCommand _btnRefreshClick;

        public RelayCommand btnRefreshClick
        {
            get
            {
                return _btnRefreshClick ?? (_btnRefreshClick = new RelayCommand(
                   (object e) => btnRefreshClickAction()));
            }
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

        public BrushConverter objBrushConvertor { get; private set; }


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
        public StopLossOrderVM()
        {
            txtMarketProtection = UtilityOrderDetails.GETInstance.MktProtection == null ? "1.0" : UtilityOrderDetails.GETInstance.MktProtection;
            UpdateOrderStatus();
            //PopulateOrderType();
            PopulateBuySell();
            PopulateClintType();
            PopulateRetainTill();
            //OrderProcessor.OnOrderResponse += CheckOrderResponce;
            OrderProcessor.OnOrderResponseReceived += UpdateStopLossOrder;
            //OrderProcessor.OrderResponseReceived += ReadDataFromOrderMemory;
            mWindow = System.Windows.Application.Current.Windows.OfType<StopLossOrderEntry>().FirstOrDefault();
            OrderProcessor.OnOrderResponse += StopLossOrderReceived;
            OrderProfilingVM.OnChangeOfMarketProtection += delegate (string MarketProt) { txtMarketProtection = MarketProt; };

            ReadDataFromOrderMemory(Enumerations.OrderExecutionStatus.StopExist.ToString());

            objBrushConvertor = new BrushConverter();
            ColourProfilingVM.OnColorSettingChange += SetColor;
            SetColor();


        }

        private void SetColor()
        {
            //DataGridBgColor = MainWindowVM.ParserDefaultCPR.GetSetting("PENDING", "PendingBackGround") != null ? objBrushConvertor.ConvertFromString(MainWindowVM.ParserDefaultCPR.GetSetting("PENDING", "PendingBackGround")) as SolidColorBrush : null;
            BuyForegroundColor = MainWindowVM.ParserDefaultCPR.GetSetting("PENDING", "PendingBuyStoploss") != null ? objBrushConvertor.ConvertFromString(MainWindowVM.ParserDefaultCPR.GetSetting("PENDING", "PendingBuyStoploss")) as SolidColorBrush : null;
            SellForegroundColor = MainWindowVM.ParserDefaultCPR.GetSetting("PENDING", "PendingSellStoploss") != null ? objBrushConvertor.ConvertFromString(MainWindowVM.ParserDefaultCPR.GetSetting("PENDING", "PendingSellStoploss")) as SolidColorBrush : null;

        }


        private void UpdateStopLossOrder(object oModel, string status)
        {

            try
            {
                if (!string.IsNullOrEmpty(status) && status == Enumerations.OrderExecutionStatus.StopExist.ToString())
                {
                    if (oModel != null)
                    {
                        if (mWindow != null)
                            mWindow?.dataGrid?.UnselectAll();

                        if (oModel.GetType().Name == "OrderModel")
                        {
                            OrderModel oOrderModel = new OrderModel();
                            oOrderModel = oModel as OrderModel;
                            AssignData(oOrderModel);
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
                //  FilterData();
            }

        }

        private void OnSelectionClientType()
        {
            if (SelectedClientType == Enumerations.Order.ClientTypes.CLIENT.ToString())
            {
                isEnableCPCode = false;
                isEnableShortClientID = true;
                txtCPCode = string.Empty;
                if (txtShortClientId?.ToString().Trim().ToUpper() == "OWN")
                {
                    txtShortClientId = string.Empty;
                }
            }
            else if (SelectedClientType == Enumerations.Order.ClientTypes.INST.ToString())
            {
                isEnableCPCode = true;
                isEnableShortClientID = true;
                if (txtShortClientId?.ToString().Trim().ToUpper() == "OWN")
                {
                    txtShortClientId = string.Empty;
                }
            }
            else if (SelectedClientType == Enumerations.Order.ClientTypes.OWN.ToString())
            {
                isEnableCPCode = false;
                txtShortClientId = "OWN";
                isEnableShortClientID = false;
                txtCPCode = string.Empty;

            }
            else if (SelectedClientType == Enumerations.Order.ClientTypes.SPLCLI.ToString())
            {
                isEnableCPCode = true;
                isEnableShortClientID = true;
                if (txtShortClientId?.ToString().Trim().ToUpper() == "OWN")
                {
                    txtShortClientId = string.Empty;
                }
            }
        }
        private void StopLossOrderReceived(object oModel, string status)
        {
            try
            {
                if (status == Enumerations.WindowName.Stoploss_OE.ToString())
                {
                    if (oModel != null)
                    {
                        if (oModel.GetType().Name == "OrderModel")//1025
                        {
                            OrderModel oOrderModel = new OrderModel();
                            oOrderModel = oModel as OrderModel;
                            if (oOrderModel != null)
                            {
                                if (oOrderModel.OrderType == "P" || oOrderModel.OrderType == "L" || oOrderModel.OptionType == "G")
                                {
                                    txtReply = oOrderModel.ReplyText;
                                    if (oOrderModel.ReplyCode == 0)
                                    {
                                        ClearFields();
                                        //clear fields
                                    }
                                }
                            }
                        }
                        else if (oModel.GetType().Name == "ESLTriggerUMS")
                        {
                            ClearFields();
                            //clear fields
                        }
                    }
                }
                if (omodel.IsStopLossOrder == true)
                {
                    // ClearFields();
                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
            }
        }

        private void ClearFields()
        {
            selectedUpdateStatus = "ADD";
            textBuySell = Enumerations.Order.BuySellFlag.B.ToString();
            textScripCodeOrId = string.Empty;
            textTotlQty = string.Empty;
            textRevQty = string.Empty;
            txtLmtRate = string.Empty;
            txtTrgRate = string.Empty;
            txtShortClientId = string.Empty;
            SelectedClientType = Enumerations.Order.ClientTypes.CLIENT.ToString();
            txtCPCode = string.Empty;
            selectedRetainTill = Enumerations.Order.RetType.EOD.ToString();
            txtMarketProtection = UtilityOrderDetails.GETInstance.MktProtection == null ? "1.0" : UtilityOrderDetails.GETInstance.MktProtection;
            selectEntireRow = null;
            // txtReply = string.Empty;

        }
        //private void CheckOrderResponce(object oModel, string status)
        //{

        //    try
        //    {
        //        if (!string.IsNullOrEmpty(status) && status == Enumerations.WindowName.Stoploss_OE.ToString())
        //        {
        //            if (oModel != null)
        //            {
        //                if (oModel.GetType().Name == "OrderModel")
        //                {
        //                    OrderModel oOrderModel = new OrderModel();
        //                    oOrderModel = oModel as OrderModel;
        //                    if (oOrderModel.ReplyCode == 0)
        //                    {
        //                        ClearFields();
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        ExceptionUtility.LogError(ex);
        //    }

        //}
        private void CloseWindowsOnEscape_Click()
        {
            mWindow?.Close();
        }
        private void ReadDataFromOrderMemory(string status)
        {
            if (status == OrderExecutionStatus.StopExist.ToString())
            {
                if (MemoryManager.OrderDictionary != null && MemoryManager.OrderDictionary.Count > 0)
                {
                    StopLossOrderCollection?.Clear();
                    foreach (OrderModel item in MemoryManager.OrderDictionary.Values.Where(x => (x.InternalOrderStatus == OrderExecutionStatus.StopExist.ToString())))
                    {
                        // ClearFields();
                        AssignData(item);

                    }
                    //StopLossOrderCollection = new ObservableCollection<Model.Order.StopLossModel>(StopLossOrderCollection.OrderByDescending(i => i.CurrentTime));  //ObjPendingOrderCollection.OrderByDescending(x => x.CurrentTime);
                    //StopLossOrderCollection.OrderBy(x => x.CurrentTime);
                }
            }
        }

        private void AssignData(OrderModel item)
        {
            if (item.InternalOrderStatus == Enumerations.OrderExecutionStatus.StopExist.ToString())
            {
                if (new[] { "A", "U" }.Any(x => x == item.OrderAction))
                {
                    AddInStopLossMemory(item);

                }
                else if (item.OrderAction == "D")
                {
                    RemoveFromStoplossMemory(item);
                }
            }

            else
            {
                RemoveFromStoplossMemory(item);
            }
        }

        private static void RemoveFromStoplossMemory(OrderModel item)
        {
            if (StopLossOrderCollection != null && StopLossOrderCollection.Count > 0)
            {
                if (StopLossOrderCollection.Any(x => x.OrderKey == item.OrderKey))
                {
                    int index = StopLossOrderCollection.IndexOf(StopLossOrderCollection.Where(x => x.OrderKey == item.OrderKey).FirstOrDefault());
                    if (index != -1)
                    {
                        StopLossOrderCollection.RemoveAt(index);
                    }
                }

            }
        }

        private static void AddInStopLossMemory(OrderModel item)
        {
            StopLossModel objStopLossModel = new StopLossModel();
            objStopLossModel.BuySell = item.BuySellIndicator;
            objStopLossModel.ClientID = item.ClientId;
            objStopLossModel.ClientType = item.ClientType;
            objStopLossModel.CPCode = item.ParticipantCode;
            objStopLossModel.TotalQty = item.OriginalQty;
            objStopLossModel.RevQty = item.RevealQty;
            objStopLossModel.Time = Convert.ToDateTime(item.Time);
            objStopLossModel.OrderKey = item.OrderKey;
            objStopLossModel.SCode = item.ScripCode;
            objStopLossModel.ScripID = item.Symbol;
            objStopLossModel.RetationTillStatus = item.OrderRetentionStatus;
            string Segment_Name = CommonFunctions.GetSegmentID(objStopLossModel.SCode);
            int Decimal_pnt = CommonFunctions.GetDecimal(System.Convert.ToInt32(objStopLossModel.SCode), "BSE", Segment_Name);
            if (item.Price == 0)
            {
                objStopLossModel.LimitRate = item.ProtectionPercentage != null ? $"{"M("}{(Convert.ToDouble(item.ProtectionPercentage) / Math.Pow(10, Decimal_pnt)):0.00}{"%)"}" : $"{"M("}{UtilityOrderDetails.GETInstance.MktProtection:0.00}{"%)"}";//string.Format("M({0:0.00}%)", txtMarketProtection);
                //objStopLossModel.LimitRate = //UtilityOrderDetails.GETInstance.MktProtection == null ? $"{"M("}{"1.0":0.00}{"%)"}" : $"{"M("}{UtilityOrderDetails.GETInstance.MktProtection:0.00}{"%)"}";
                objStopLossModel.TriggertRate = CommonFunctions.GetValueInDecimal(item.TriggerPrice, Decimal_pnt);//string.Format("{0:0.00}", item.TriggerPrice / Math.Pow(10, Decimal_pnt));
            }
            else
            {
                objStopLossModel.LimitRate = CommonFunctions.GetValueInDecimal(item.Price, Decimal_pnt);
                objStopLossModel.TriggertRate = CommonFunctions.GetValueInDecimal(item.TriggerPrice, Decimal_pnt);
            }

            objStopLossModel.OrderType = item.OrderType;
            objStopLossModel.OnlyOrdID = item.OrderId;
            objStopLossModel.OrdID = item.OrderId + item.OrderType;
            objStopLossModel.CurrentTime = Convert.ToDateTime(item.Time);//DateTime.ParseExact(item.Time, "HH:mm:ss", CultureInfo.InvariantCulture);
                                                                         // objStopLossModel.MarketProtection = item.ProtectionPercentage;
                                                                         //StopLossOrderCollection.Add(objStopLossModel);
            objStopLossModel.SegmentID = item.SegmentFlag;

            if (StopLossOrderCollection != null && StopLossOrderCollection.Count > 0)
            {
                if (StopLossOrderCollection.Any(x => x.OrderKey == item.OrderKey))
                {
                    int index = StopLossOrderCollection.IndexOf(StopLossOrderCollection.Where(x => x.OrderKey == item.OrderKey).FirstOrDefault());
                    StopLossOrderCollection[index] = objStopLossModel;


                }
                else
                {
                    StopLossOrderCollection.Add(objStopLossModel);
                }
            }
            else
            {
                StopLossOrderCollection?.Add(objStopLossModel);
            }
        }




        #endregion

        #region ControlsV Validation
        private void BuySellTextValidation(object e)
        {
            textBuySell = Regex.Replace(textBuySell, "[^a-zA-Z]+", "").ToUpper();
        }

        private void TotalQtyValidation(object e)
        {
            textTotlQty = Regex.Replace(textTotlQty, "[^0-9]+", "");
        }

        private void textRevQtyValidation(object e)
        {
            textRevQty = Regex.Replace(textRevQty, "[^0-9]+", "");
        }


        private void OnOrderTypeEnable()
        {
            if (selectedOrderType == "SL-Limit")
            {
                isEnableLmtRate = true;
                isEnableTrgRate = true;
            }


            else if (selectedOrderType == "SL-Market")
            {
                isEnableLmtRate = false;
                isEnableTrgRate = true;
                txtLmtRate = "0";
            }
            else if (selectedOrderType == "R-Limit")
            {
                isEnableLmtRate = true;
                isEnableTrgRate = false;
                txtTrgRate = "0";
            }
            else if (selectedOrderType == "R-Market")
            {
                isEnableLmtRate = false;
                isEnableTrgRate = false;
                txtLmtRate = "0";
                txtTrgRate = "0";

            }


        }
        //private void onChangeUpdateStatus()
        //{
        //    if (selectedUpdateStatus == "ADD")
        //    {
        //        isEnableBuySell = true;
        //        isEnableScripCodeORID = true;
        //    }
        //    else
        //    {
        //        isEnableBuySell = false;
        //        isEnableScripCodeORID = false;
        //        PopulateModifyOrderType();
        //    }
        //}
        #endregion

        #region Method
        private void UpdateOrderStatus()
        {
            cmbUpdateList = new ObservableCollection<string>();
            cmbUpdateList.Add("ADD");
            cmbUpdateList.Add("MODIFY");
            selectedUpdateStatus = cmbUpdateList[0];

            btnSelectAllContent = "Select All";

            btnActionList = new ObservableCollection<string>();
            btnActionList.Add("Delete");
            btnActionList.Add("SL-Market");
            btnActionList.Add("R-Market");
            SelectedAction = btnActionList[0];
        }

        private void PopulateOrderType()
        {
            if (selectedUpdateStatus == "ADD")
            {
                cmbOrderTypeList = new ObservableCollection<string>();
                cmbOrderTypeList.Add("SL-Limit");
                cmbOrderTypeList.Add("SL-Market");
                selectedOrderType = cmbOrderTypeList[0];

                isEnableBuySell = true;
                isEnableScripCodeORID = true;
            }
            else if (selectedUpdateStatus == "MODIFY")
            {
                cmbOrderTypeList = new ObservableCollection<string>();
                cmbOrderTypeList.Add("SL-Limit");
                cmbOrderTypeList.Add("SL-Market");
                cmbOrderTypeList.Add("R-Limit");
                cmbOrderTypeList.Add("R-Market");
                selectedOrderType = cmbOrderTypeList[0];

                isEnableBuySell = false;
                isEnableScripCodeORID = false;
            }

        }

        private void ExecuteMyCommand()

        {
            if (StopLossOrderCollection.Count == 0)
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
                try
                {
                    writer = new StreamWriter(dlg.FileName, false, Encoding.UTF8);

                    writer.Write("BuySell, SCode, ScripID, TotalQty, RevQty , LmtRate,TrgRate, Order_Type, Retention_Status,ClientID, Time, OrdID, ClientType, CPCode");
                    writer.Write(writer.NewLine);

                    foreach (var dr in StopLossOrderCollection)
                    {

                        writer.Write(dr.BuySell + "," + dr.SCode + "," + dr.ScripID + "," + dr.TotalQty + "," + dr.RevQty + "," + Convert.ToDouble(dr.LimitRate) * 100 + "," + dr.TriggertRate+","+dr.OrderType+","+dr.RetationTillStatus+"," + dr.ClientID + "," +
                            dr.Time + "," + dr.OrdID + "," + dr.ClientType + "," + dr.CPCode);

                        writer.Write(writer.NewLine);
                    }

                    System.Windows.MessageBox.Show("StopLoss Orders Saved in file :" + dlg.FileName.ToString(), "Message", MessageBoxButton.OK);
                }
                catch (IOException io)
                {
                    ExceptionUtility.LogError(io);
                    MessageBox.Show("The File is already open in an application. please close it and try again", " Error!", MessageBoxButton.OK, MessageBoxImage.Error);
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


        private void PopulateModifyOrderType()
        {

        }

        private void PopulateBuySell()
        {
            BuySellList = new ObservableCollection<string>();
            BuySellList.Add(Enumerations.Order.BuySellFlag.B.ToString());
            BuySellList.Add(Enumerations.Order.BuySellFlag.S.ToString());
            textBuySell = BuySellList[0];
        }

        private void PopulateClintType()
        {
            cmbClientTypeList = new ObservableCollection<string>();
            cmbClientTypeList.Add(Enumerations.Order.ClientTypes.CLIENT.ToString());
            cmbClientTypeList.Add(Enumerations.Order.ClientTypes.INST.ToString());
            cmbClientTypeList.Add(Enumerations.Order.ClientTypes.OWN.ToString());
            cmbClientTypeList.Add(Enumerations.Order.ClientTypes.SPLCLI.ToString());
            SelectedClientType = cmbClientTypeList[0];
        }

        private void PopulateRetainTill()
        {
            cmbRetainTillList = new ObservableCollection<string>();
            cmbRetainTillList.Add(Enumerations.Order.RetType.EOD.ToString());
            cmbRetainTillList.Add(Enumerations.Order.RetType.EOS.ToString());
            cmbRetainTillList.Add(Enumerations.Order.RetType.IOC.ToString());
            selectedRetainTill = cmbRetainTillList[0];
        }
        private void btnRefreshClickAction()
        {
            if (!Validator(textScripCodeOrId))
            {
                return;
            }
            int i = 0;
            long SCode = 0;
            string Segment_Name = string.Empty;
            bool result = int.TryParse(textScripCodeOrId, out i);
            if (result == false)
            {
                SCode = CommonFunctions.GetScripCodeFromScripID(textScripCodeOrId);
                Segment_Name = CommonFunctions.GetSegmentID(SCode);
                CommonFunctions.CallBestFiveUsingScripCode((int)SCode);

            }
            else
            {
                Segment_Name = CommonFunctions.GetSegmentID(Convert.ToInt64(textScripCodeOrId));
                SCode = Convert.ToInt64(textScripCodeOrId);
            }
            CommonFunctions.CallBestFiveUsingScripCode((int)SCode);

        }

        private void ShowBestFive()
        {
            BestFiveMarketPicture oLoginScreen = System.Windows.Application.Current.Windows.OfType<BestFiveMarketPicture>().FirstOrDefault();

            if (oLoginScreen != null)
            {
                oLoginScreen.Activate();
                oLoginScreen.Show();
            }
            else
            {
                oLoginScreen = new BestFiveMarketPicture();
                oLoginScreen.Activate();
                oLoginScreen.Owner = System.Windows.Application.Current.MainWindow;
                oLoginScreen.Activate();
                oLoginScreen.Show();
            }
        }

        private bool Validator(string scripCodeOrId)
        {
            int i = 0;
            string Segment_Name = string.Empty;
            if (string.IsNullOrEmpty(textScripCodeOrId))
            {
                txtReply = "INVALID SCRIPID";
                return false;
            }

            bool result = int.TryParse(textScripCodeOrId, out i);

            if (result == false)
            {
                long SCode = CommonFunctions.GetScripCodeFromScripID(textScripCodeOrId);
                if (SCode == 0)
                {
                    txtReply = "INVALID SCRIPID";
                    return false;
                }
                else
                {
                    Segment_Name = CommonFunctions.GetSegmentID(SCode);
                    string ScripID = textScripCodeOrId;
                }


            }
            else
            {
                string ScripID = CommonFunctions.GetScripId(Convert.ToInt64(textScripCodeOrId), "BSE");
                if (string.IsNullOrEmpty(ScripID))
                {
                    txtReply = "INVALID SCRIP CODE";
                    return false;
                }
                else
                {
                    Segment_Name = CommonFunctions.GetSegmentID(Convert.ToInt64(textScripCodeOrId));
                    long SCode = CommonFunctions.GetScripCodeFromScripID(ScripID);
                }

            }


            return true;
        }

        private void btnSubmitClick(object e)
        {

            if (!ValidateOrder())
            {
                return;
            }

            //objStopLossModel.TotalQty = Convert.ToInt32(textTotlQty);
            //objStopLossModel.RevQty = Convert.ToInt32(textRevQty);
            //if (selectedOrderType == "SL-Limit")
            //{
            //    if (Segment_Name == Enumerations.Segment.Currency.ToString())
            //        objStopLossModel.LimitRate = string.Format("{0:0.0000}", txtLmtRate / Math.Pow(10, Decimal_pnt));
            //    else
            //        objStopLossModel.LimitRate = string.Format("{0:0.00}", txtLmtRate / Math.Pow(10, Decimal_pnt));
            //}
            //else if (selectedOrderType == "SL-Market")
            //{
            //    objStopLossModel.LimitRate = $"{"M("}{txtMarketProtection:0.00}{"%)"}"; //string.Format("M({0:0.00}%)", txtMarketProtection);
            //}

            OrderRequestProcessor oOrderRequestProcessor = null;
            ModelCreation();
            int DecimalPoint = CommonFunctions.GetDecimal(System.Convert.ToInt32(omodel.ScripCode), "BSE", omodel.Segment);
            if (selectedUpdateStatus == "ADD")
                omodel.OrderAction = Enumerations.Order.Modes.A.ToString();
            else
            {
                omodel.OrderAction = Enumerations.Order.Modes.U.ToString();
                if (selectEntireRow.OnlyOrdID == null)
                {
                    txtReply = "ORDER NOT SELECTED";
                    return;
                }
                else
                {
                    omodel.OrderId = selectEntireRow.OnlyOrdID;
                    omodel.OrderKey = selectEntireRow.OrderKey;
                }
                //omodel.OrderKey = string.Format()
            }

            bool validate = Validations.ValidateOrder(Convert.ToString(txtLmtRate), Convert.ToString(txtTrgRate), omodel, ref Validate_Message, DecimalPoint, true);
            if (!validate)
            {
                txtReply = Validate_Message;
                return;
            }
            else
            {
                if (selectedOrderType.ToUpper() == "SL-MARKET")
                {
                    omodel.Price = 0;
                }
                else
                {
                    omodel.Price = Convert.ToInt64(Convert.ToDouble(txtLmtRate) * Math.Pow(10, DecimalPoint));
                }
                if (selectedOrderType.ToUpper() == "SL-MARKET")
                {
                    var protectionPercent = Convert.ToInt32(Convert.ToDecimal(txtMarketProtection) * 100);
                    omodel.ProtectionPercentage = Convert.ToString(protectionPercent);
                }

                if (!string.IsNullOrEmpty(txtTrgRate))
                    omodel.TriggerPrice = Convert.ToInt64(Convert.ToDouble(txtTrgRate) * Math.Pow(10, DecimalPoint));
                if (selectedUpdateStatus == "ADD")
                    oOrderRequestProcessor = new OrderRequestProcessor(new AddOrder());
                else if (selectedUpdateStatus == "MODIFY")
                    oOrderRequestProcessor = new OrderRequestProcessor(new ModifyOrder());
                oOrderRequestProcessor.ProcessRequest(omodel);



            }
            //if(omodel.OrderType == "L")
            //{
            //    StopLossOrderEntry obj = new StopLossOrderEntry();
            //}
        }

        private void ModelCreation()
        {
            omodel = new OrderModel();
            string Segment_Name = string.Empty;

            int i = 0;
            bool result = int.TryParse(textScripCodeOrId, out i);
            if (result == false)
            {
                omodel.ScripCode = CommonFunctions.GetScripCodeFromScripID(textScripCodeOrId);
                Segment_Name = CommonFunctions.GetSegmentID(omodel.ScripCode);
                omodel.Symbol = textScripCodeOrId.Trim();
            }
            else
            {
                omodel.Symbol = CommonFunctions.GetScripId(Convert.ToInt64(textScripCodeOrId), "BSE");
                omodel.ScripCode = Convert.ToInt64(textScripCodeOrId);
                Segment_Name = CommonFunctions.GetSegmentID(omodel.ScripCode);
                //objStopLossModel.ScripID = CommonFunctions.GetScripId(Convert.ToInt64());
            }
            DecimalPoint = CommonFunctions.GetDecimal(Convert.ToInt32(omodel.ScripCode), "BSE", Segment_Name);
            omodel.Exchange = "BSE"; //1- BSE, 2-BOW
            omodel.Segment = CommonFunctions.GetSegmentID(omodel.ScripCode); //1 - Equity, 2 - Derv., 4.Curr
            omodel.SegmentFlag = CommonFunctions.SegmentFlag(omodel.Segment); //(int)Enum.Parse(typeof(Enumerations.Order.ScripSegment), Segment_Name);
            omodel.ClientType = SelectedClientType.ToUpper();
            if (omodel.ClientType == Enumerations.Order.ClientTypes.CLIENT.ToString() || omodel.ClientType == Enumerations.Order.ClientTypes.INST.ToString() || omodel.ClientType == Enumerations.Order.ClientTypes.SPLCLI.ToString())
            {
                omodel.ClientId = txtShortClientId.ToUpper();//Client Id
            }
            else if (omodel.ClientType == Enumerations.Order.ClientTypes.OWN.ToString())
            {
                omodel.ClientId = Enumerations.Order.ClientTypes.OWN.ToString();//in case of own
            }
            omodel.SenderLocationID = UtilityLoginDetails.GETInstance.SenderLocationId;//UtilityLoginDetails.GETInstance.SenderLocationId;
            if (textBuySell.ToUpper() == Enumerations.Order.BuySellFlag.B.ToString())
            {
                omodel.BuySellIndicator = "B";
            }
            else if (textBuySell.ToUpper() == Enumerations.Order.BuySellFlag.S.ToString())
            {
                omodel.BuySellIndicator = "S";
            }
            omodel.OrderRetentionStatus = selectedRetainTill;//0-IOC/1-SES/2-DAY

            omodel.OrderRemarks = "SL Order";
            if (selectedOrderType == "SL-Limit")
            {
                omodel.OrderType = "P";
            }
            else if (selectedOrderType == "SL-Market")
            {
                omodel.OrderType = "P";
            }
            else if (selectedOrderType == "R-Limit")
            {
                omodel.OrderType = "L";
            }
            else if (selectedOrderType == "R-Market")
            {
                omodel.OrderType = "G";
            }



            if (!string.IsNullOrEmpty(textRevQty))
                omodel.RevealQty = Convert.ToInt32(textRevQty);//Reveal QTY

            if (!string.IsNullOrEmpty(textTotlQty))
            {
                omodel.OriginalQty = Convert.ToInt32(textTotlQty);
                //pending qty should be equal to orginal qty while adding order. Gaurav Jadhav 23/2/2018
                omodel.PendingQuantity = omodel.OriginalQty;
            }

            omodel.ScreenId = (int)Enumerations.WindowName.Stoploss_OE;
            omodel.MarketLot = Convert.ToInt32(CommonFunctions.GetMarketLot(omodel.ScripCode));
            omodel.TickSize = CommonFunctions.GetTickSize(omodel.ScripCode);
            omodel.Group = CommonFunctions.GetGroupName(omodel.ScripCode, "BSE", Segment_Name);
            omodel.ExecInst = Enumerations.Order.ExecInst.PersistentOrder.ToString();
            omodel.ParticipantCode = txtCPCode;
            //omodel.ParticipantCode = string.Empty;
            // omodel.ProtectionPercentage = string.Empty;
            omodel.FreeText3 = "fdf";
            omodel.Filler_c = "fdf";
            omodel.IsStopLossOrder = true;
        }

        private bool ValidateOrder()
        {
            int i = 0;
            string Segment_Name = string.Empty;
            bool result = int.TryParse(textScripCodeOrId, out i);
            if (result == false)
            {
                long SCode = CommonFunctions.GetScripCodeFromScripID(textScripCodeOrId);
                if (SCode == 0)
                {
                    txtReply = "INVALID SCRIPID";
                    return false;
                }
                else
                {
                    Segment_Name = CommonFunctions.GetSegmentID(SCode);
                    string ScripID = textScripCodeOrId;
                }


            }
            else
            {
                string ScripID = CommonFunctions.GetScripId(Convert.ToInt64(textScripCodeOrId), "BSE");
                if (string.IsNullOrEmpty(ScripID))
                {
                    txtReply = "INVALID SCRIP CODE";
                    return false;
                }
                else
                {
                    Segment_Name = CommonFunctions.GetSegmentID(Convert.ToInt64(textScripCodeOrId));
                    long SCode = CommonFunctions.GetScripCodeFromScripID(ScripID);
                }

            }

            if (Segment_Name == Enumerations.Segment.Equity.ToString() || Segment_Name == Enumerations.Segment.Debt.ToString())
            {
                if (!string.IsNullOrEmpty(txtCPCode))
                {
                    txtReply = "CP Code should be blank for Equity/Debt Scrip";
                    txtCPCode = string.Empty;
                    return false;
                }
            }

            if (string.IsNullOrEmpty(txtShortClientId))
            {
                txtReply = "PLEASE ENTER SHORT CLIENT";
                return false;
            }

            if (selectedOrderType.ToUpper() == "SL-MARKET")
            {
                if (string.IsNullOrEmpty(txtMarketProtection))
                {
                    txtReply = "PLEASE FILL MARKET PROTECTION FIELD.";
                    return false;
                }
                //var protectionPercent = Convert.ToInt32(Convert.ToDecimal(txtMarketProtection) * 100).ToString();
                var protectionPercent = Convert.ToInt32(Convert.ToDecimal(txtMarketProtection) * 100);
                if (!string.IsNullOrEmpty(txtMarketProtection))
                {
                    if (!Regex.IsMatch(txtMarketProtection.Trim(), @"^[1-9]\d*(\.\d+)?$"))
                    {
                        if (txtMarketProtection == "0" || txtMarketProtection == string.Empty)
                            txtReply = "Please Provide Market Protection";

                        //Check Number after 2 decimal point
                        if (txtMarketProtection.Contains(".") && txtMarketProtection.Substring(txtMarketProtection.IndexOf(".") + 1).Length > 2)
                            txtReply = "Market Protection More than 2 decimal places!";

                        else
                            txtReply = "Invalid Market Protection";

                        return false;
                    }
                    //MarketProtection can't be greater than  99.99%

                    else if (protectionPercent > 9999)
                    {
                        txtReply = "MarketProtection can't be greater than  99.99%";
                        return false;
                    }
                }
            }
            if (selectedUpdateStatus == "MODIFY" && selectEntireRow == null)
            {
                txtReply = "No order Selected For Updation";
                return false;
            }

            if (selectedOrderType.ToUpper() == "SL-LIMIT")
            {
                if ((string.IsNullOrEmpty(txtLmtRate)) && (Convert.ToDecimal(txtLmtRate) <= 0))
                {
                    txtReply = "INVALID LIMIT RATE";
                    return false;
                }

                if (textBuySell == Enumerations.Order.BuySellFlag.B.ToString())
                {
                    if (Convert.ToDecimal(txtLmtRate) < Convert.ToDecimal(txtTrgRate))
                    {
                        txtReply = "LIMIT PRICE MUST BE GREATER THAN STOP PRICE";
                        return false;
                    }
                }
                else
                {
                    if (Convert.ToDecimal(txtLmtRate) > Convert.ToDecimal(txtTrgRate))
                    {
                        txtReply = "LIMIT PRICE MUST BE SMALLER THAN STOP PRICE";
                        return false;
                    }
                }
            }

            if (selectedOrderType.ToUpper() == "R-Limit")
            {
                if ((string.IsNullOrEmpty(txtLmtRate)) && (Convert.ToDecimal(txtLmtRate) <= 0))
                {
                    txtReply = "INVALID LIMIT RATE";
                    return false;
                }
            }

            if ((string.IsNullOrEmpty(txtTrgRate)) && (Convert.ToDecimal(txtTrgRate) <= 0))
            {
                txtReply = "INVALID TRIGGER RATE";
                return false;
            }
            if (selectedUpdateStatus == "MODIFY")
            {
                if (SelectedClientType != selectEntireRow.ClientType)
                {
                    txtReply = "Client type can't be modified";
                    return false;
                }
            }
            return true;
        }

        private void btnSelectAll_Click()
        {
            StopLossOrderEntry mWindow = System.Windows.Application.Current.Windows.OfType<StopLossOrderEntry>().FirstOrDefault();

            if (btnSelectAllContent == "Select All")
            {
                SelectedItemLists = StopLossOrderCollection.ToList();
                if (mWindow != null)
                {
                    mWindow.dataGrid.SelectAll();
                    mWindow.dataGrid.Focus();
                }

                txtReply = SelectedItemLists.Count + " Order(s) Selected";
                btnSelectAllContent = "Deselect All";
            }
            else if (btnSelectAllContent == "Deselect All")
            {
                txtReply = SelectedItemLists.Count + " Order(s) Deselected";
                SelectedItemLists.Clear();
                if (mWindow != null)
                {
                    mWindow.dataGrid.UnselectAll();
                    mWindow.dataGrid.Focus();
                }
                btnSelectAllContent = "Select All";
            }
        }

        private void btnApply_Click()
        {
            if (SelectedAction == "Delete")
            {
                StoplossBackGroundThread = new Thread(SendDeleteRequest);
                StoplossBackGroundThread.Start();
            }
            else if (SelectedAction == "SL-Market")
            {
                StoplossBackGroundThread = new Thread(UpdatetoSlMarket);
                StoplossBackGroundThread.Start();
            }
            else if (SelectedAction == "R-Market")
            {
                StoplossBackGroundThread = new Thread(UpdateToRMarket);
                StoplossBackGroundThread.Start();
            }

        }

        private void UpdateToRMarket()
        {
            foreach (var item in SelectedItemLists.ToList())
            {
                SendUpdateRequest(item, SelectedAction);
                StopLossEvent.WaitOne();
            }
        }

        private void UpdatetoSlMarket()
        {
            foreach (var item in SelectedItemLists.ToList())
            {
                SendUpdateRequest(item, SelectedAction);
                StopLossEvent.WaitOne();
            }
        }

        private void SendDeleteRequest()
        {
            foreach (var item in SelectedItemLists.ToList())
            {
                DeleteOrder(item);
                StopLossEvent.WaitOne();
            }
        }

        private void DeleteOrder(StopLossModel item)
        {
            OrderRequestProcessor oOrderRequestProcessor = new OrderRequestProcessor(new DeleteOrder());
            OrderModel oOrderModel = new OrderModel();
            string key = string.Format("{0}_{1}", item.SCode, item.OnlyOrdID);
            MemoryManager.OrderDictionary.TryGetValue(key, out oOrderModel);
            if (oOrderModel != null)
            {
                oOrderModel.OrderAction = "D";
                oOrderModel.SegmentFlag = CommonFunctions.SegmentFlag(oOrderModel.Segment);
                oOrderRequestProcessor.ProcessRequest(oOrderModel);
            }
            else
            {
                txtReply = "Can't find order";
            }
        }

        private void SendUpdateRequest(StopLossModel item, string selectedAction)
        {
            omodel = new OrderModel();
            OrderRequestProcessor oOrderRequestProcessor = null;
            //omodel.ScripCode = item.SCode;
            //omodel.Symbol = item.ScripID;
            //omodel.Exchange = Exchange.BSE.ToString();

            //omodel.Segment = CommonFunctions.GetSegmentID(omodel.ScripCode);
            //omodel.SegmentFlag = (int)Enum.Parse(typeof(Enumerations.Order.ScripSegment), omodel.Segment);
            //int DecimalPoint = CommonFunctions.GetDecimal(System.Convert.ToInt32(omodel.ScripCode), "BSE", omodel.Segment);
            //omodel.ClientType = item.ClientType;
            //if (omodel.ClientType == Enumerations.Order.ClientTypes.OWN.ToString())
            //{
            //    omodel.ClientId = Enumerations.Order.ClientTypes.OWN.ToString();//in case of own
            //}
            //else {
            //    omodel.ClientId = item.ClientID;
            //}

            //omodel.SenderLocationID = UtilityLoginDetails.GETInstance.SenderLocationId;
            //omodel.BuySellIndicator = item.BuySell;
            //omodel.OrderRetentionStatus = item.RetationTillStatus;
            //omodel.OrderRemarks = "SL Order";
            //if (SelectedAction == "SL-Market")
            //{
            //    omodel.OrderType = "P";
            //}
            //else if (SelectedAction == "R-Market")
            //{
            //    omodel.OrderType = "G";
            //}
            //    omodel.RevealQty = item.RevQty;
            //omodel.OriginalQty = item.TotalQty;
            //omodel.PendingQuantity = omodel.OriginalQty;
            //omodel.ScreenId = (int)Enumerations.WindowName.Stoploss_OE;
            //omodel.MarketLot = Convert.ToInt32(CommonFunctions.GetMarketLot(omodel.ScripCode));
            //omodel.TickSize = CommonFunctions.GetTickSize(omodel.ScripCode);
            //omodel.Group = CommonFunctions.GetGroupName(omodel.ScripCode, "BSE", omodel.Segment);
            //omodel.ExecInst = Enumerations.Order.ExecInst.PersistentOrder.ToString();
            //// omodel.ParticipantCode = txtCPCode;
            //omodel.ParticipantCode = string.Empty;
            //omodel.ProtectionPercentage = UtilityOrderDetails.GETInstance.MktProtection;
            //omodel.FreeText3 = "fdf";
            //omodel.Filler_c = "fdf";
            //omodel.PartitionID = Convert.ToUInt16(CommonFunctions.GetPartitionId((long)omodel.ScripCode, "BSE", omodel.Segment));
            //omodel.MarketSegmentID = CommonFunctions.GetProductId((long)omodel.ScripCode, "BSE", omodel.Segment);
            //omodel.IsStopLossOrder = true;
            //omodel.OrderAction = Enumerations.Order.Modes.U.ToString();
            //omodel.OrderId = item.OrdID;
            //omodel.OrderKey = item.OrderKey;
            //omodel.Price = 0;
            //omodel.ProtectionPercentage = item.
            //omodel.TriggerPrice = Convert.ToInt64(Convert.ToDouble(item.TriggertRate) * Math.Pow(10, DecimalPoint));

            if (MemoryManager.OrderDictionary.ContainsKey(item.OrderKey))
            {
                omodel = MemoryManager.OrderDictionary[item.OrderKey];
                omodel.OrderAction = Enumerations.Order.Modes.U.ToString();
                if (SelectedAction == "SL-Market")
                {
                    omodel.OrderType = "P";
                    omodel.Price = 0;
                    omodel.ProtectionPercentage = Convert.ToInt32(Convert.ToDecimal(txtMarketProtection) * 100).ToString();
                    omodel.SegmentFlag = CommonFunctions.SegmentFlag(omodel.Segment);
                }
                else if (SelectedAction == "R-Market")
                {
                    omodel.OrderType = "G";
                    omodel.Price = 0;
                    omodel.TriggerPrice = 0;
                    omodel.ProtectionPercentage = Convert.ToInt32(Convert.ToDecimal(txtMarketProtection) * 100).ToString();
                    omodel.SegmentFlag = CommonFunctions.SegmentFlag(omodel.Segment);
                }
                omodel.IsStopLossOrder = true;
                oOrderRequestProcessor = new OrderRequestProcessor(new ModifyOrder());
                oOrderRequestProcessor.ProcessRequest(omodel);
            }
            else
            {
                txtReply = "Order Not Found!";
            }
        }
        #endregion
    }


    public partial class StopLossOrderVM : BaseViewModel
    {
#if TWS

#endif
    }


    public partial class StopLossOrderVM : BaseViewModel
    {
#if BOW

#endif
    }

}
