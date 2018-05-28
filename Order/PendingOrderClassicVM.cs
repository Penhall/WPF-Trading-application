using CommonFrontEnd.Model.Order;
using CommonFrontEnd.Processor.Order;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CommonFrontEnd.SharedMemories;
using static CommonFrontEnd.SharedMemories.MemoryManager;
using CommonFrontEnd.Common;
using System.Windows;
using System.IO;
using Microsoft.Win32;
using CommonFrontEnd.Common.DataGridHelperClasses;
using System.Windows.Controls;
using CommonFrontEnd.View.Order;
using System.ComponentModel;
using System.Windows.Data;
using CommonFrontEnd.View;
using System.Data;
using CommonFrontEnd.Global;
using System.Globalization;
using System.Windows.Media;
using CommonFrontEnd.ViewModel.Profiling;
using System.Text.RegularExpressions;

namespace CommonFrontEnd.ViewModel.Order
{
    public partial class PendingOrderClassicVM : BaseViewModel
    {

        #region Properties

        //static SynchronizationContext uiContext;
        string Validate_Message = string.Empty;
        public static bool bulkChangeWindowOpen = false;
        public CollectionViewSource viewSource = null;
        static View.Order.PendingOrderClassic mWindow = null;
        Thread DeleteThread = null;
        public static AutoResetEvent PendingOrderDeleteEvent = new AutoResetEvent(false);
        public BrushConverter objBrushConvertor { get; set; }

        public static int Decimal_Point { get; set; }

        private bool _btnEnable = true;

        public bool btnEnable
        {
            get { return _btnEnable; }
            set { _btnEnable = value; NotifyPropertyChanged(nameof(btnEnable)); }
        }


        private ObservableCollection<Model.Order.PendingOrder> _ObjPendingOrderCollection;

        public ObservableCollection<Model.Order.PendingOrder> ObjPendingOrderCollection
        {
            get { return _ObjPendingOrderCollection; }
            set { _ObjPendingOrderCollection = value; NotifyPropertyChanged(nameof(ObjPendingOrderCollection)); }
        }

        //private ObservableCollection<Model.Order.PendingOrder> _tempObjPendingOrderCollection;

        //public ObservableCollection<Model.Order.PendingOrder> TempObjPendingOrderCollection
        //{
        //    get { return _tempObjPendingOrderCollection; }
        //    set { _tempObjPendingOrderCollection = value; NotifyPropertyChanged(nameof(TempObjPendingOrderCollection)); }
        //}



        private int _TotalQtytxt;

        public int TotalQtytxt
        {
            get { return _TotalQtytxt; }
            set { _TotalQtytxt = value; NotifyPropertyChanged(nameof(TotalQtytxt)); }
        }

        private int _RevQtytxt;

        public int RevQtytxt
        {
            get { return _RevQtytxt; }
            set { _RevQtytxt = value; NotifyPropertyChanged(nameof(RevQtytxt)); }
        }

        private string _Ratetxt;

        public string Ratetxt
        {
            get { return _Ratetxt; }
            set { _Ratetxt = value; NotifyPropertyChanged(nameof(Ratetxt)); }
        }

        private string _ClientIDtxt;

        public string ClientIDtxt
        {
            get { return _ClientIDtxt; }
            set { _ClientIDtxt = value; NotifyPropertyChanged(nameof(ClientIDtxt)); }
        }

        private string _MktPTtxt;
        public string MktPTtxt
        {
            get { return _MktPTtxt; }
            set
            {
                _MktPTtxt = value;
                NotifyPropertyChanged("MktPTtxt");

            }
        }

        private string _SelectedRetainType;

        public string SelectedRetainType
        {
            get { return _SelectedRetainType; }
            set
            {
                _SelectedRetainType = value;
                NotifyPropertyChanged("SelectedRetainType");
            }
        }

        private List<string> _RetainType;

        public List<string> RetainType
        {
            get { return _RetainType; }
            set { _RetainType = value; NotifyPropertyChanged(nameof(RetainType)); }
        }

        private string _SeletedOrderType;

        public string SeletedOrderType
        {
            get { return _SeletedOrderType; }
            set
            {
                _SeletedOrderType = value;
                NotifyPropertyChanged("SeletedOrderType");
                if (SeletedOrderType == "L")
                {
                    MktProtEnabled = false;
                }
                else if (SeletedOrderType == "G")
                {
                    MktProtEnabled = true;
                }

            }

        }

        private List<string> _OrderType;

        public List<string> OrderType
        {
            get { return _OrderType; }
            set { _OrderType = value; NotifyPropertyChanged(nameof(OrderType)); }
        }


        private bool _CheckedOrderType;

        public bool CheckedOrderType
        {
            get { return _CheckedOrderType; }
            set
            {
                _CheckedOrderType = value; NotifyPropertyChanged(nameof(CheckedOrderType));
                if (value == true)
                {
                    EnableOrderType = true;
                }
                else
                {
                    if (OrderType != null)
                    {
                        SeletedOrderType = "L";
                    }
                    EnableOrderType = false;
                }
            }

        }


        private bool _EnableOrderType;

        public bool EnableOrderType
        {
            get { return _EnableOrderType; }
            set
            {
                _EnableOrderType = value; NotifyPropertyChanged(nameof(EnableOrderType));

            }
        }


        private bool _MktProtEnabled;

        public bool MktProtEnabled
        {
            get { return _MktProtEnabled; }
            set { _MktProtEnabled = value; NotifyPropertyChanged(nameof(MktProtEnabled)); }
        }


        private string _OCOTrgRatetxt;

        public string OCOTrgRatetxt
        {
            get { return _OCOTrgRatetxt; }
            set { _OCOTrgRatetxt = value; NotifyPropertyChanged(nameof(OCOTrgRatetxt)); }
        }

        private string _Reply;

        public string Reply
        {
            get { return _Reply; }
            set { _Reply = value; NotifyPropertyChanged(nameof(Reply)); }
        }


        private string _SelectAllVisible;

        public string SelectAllVisible
        {
            get { return _SelectAllVisible; }
            set { _SelectAllVisible = value; NotifyPropertyChanged(nameof(SelectAllVisible)); }
        }


        private string _DeSelectAllVisible;

        public string DeSelectAllVisible
        {
            get { return _DeSelectAllVisible; }
            set { _DeSelectAllVisible = value; NotifyPropertyChanged(nameof(DeSelectAllVisible)); }
        }

        private string _BuySellFiltertxt;

        public string BuySellFiltertxt
        {
            get { return _BuySellFiltertxt; }
            set { _BuySellFiltertxt = value; NotifyPropertyChanged(nameof(BuySellFiltertxt)); OnChangeOfBuySellFiltertxt(); }
        }


        private string _ScripCodeFilterTxt;

        public string ScripCodeFilterTxt
        {
            get { return _ScripCodeFilterTxt; }
            set { _ScripCodeFilterTxt = value; NotifyPropertyChanged(nameof(ScripCodeFilterTxt)); onChangeOfScripCodeFilterTxt(); }
        }



        private string _ClientIDFilterTxt;

        public string ClientIDFilterTxt
        {
            get { return _ClientIDFilterTxt; }
            set { _ClientIDFilterTxt = value; NotifyPropertyChanged(nameof(ClientIDFilterTxt)); onChangeOfClientIDFilterTxt(); }
        }

        private bool _TouchLineIsChecked;

        public bool TouchLineIsChecked
        {
            get { return _TouchLineIsChecked; }
            set { _TouchLineIsChecked = value; NotifyPropertyChanged(nameof(TouchLineIsChecked)); onChangeOfTouchLineIsChecked(); }
        }


        private bool _FiveLakhChk;

        public bool FiveLakhChk
        {
            get { return _FiveLakhChk; }
            set { _FiveLakhChk = value; NotifyPropertyChanged(nameof(FiveLakhChk)); onChangeOfFiveLakhChk(); }
        }

        private Model.Order.PendingOrder _SelectedItem;

        public Model.Order.PendingOrder SelectedItem
        {
            get { return _SelectedItem; }
            set { _SelectedItem = value; NotifyPropertyChanged(nameof(SelectedItem)); UpdateDataGridSelectedItem(null); }
        }

        private string _btnSelectAllContent = "Select All";

        public string btnSelectAllContent
        {
            get { return _btnSelectAllContent; }
            set { _btnSelectAllContent = value; NotifyPropertyChanged(nameof(btnSelectAllContent)); }
        }

        private string _txtYTM;

        public string txtYTM
        {
            get { return _txtYTM; }
            set { _txtYTM = value; NotifyPropertyChanged(nameof(txtYTM)); }
        }

        private string _Txtcpcode;

        public string Txtcpcode
        {
            get { return _Txtcpcode; }
            set { _Txtcpcode = value; NotifyPropertyChanged(nameof(Txtcpcode)); }
        }

        private string _txtSCripCodeScripID;

        public string txtSCripCodeScripID
        {
            get { return _txtSCripCodeScripID; }
            set { _txtSCripCodeScripID = value; NotifyPropertyChanged(nameof(txtSCripCodeScripID)); onChangeOfScripCodeTextBox(); }
        }

        private string _SelectedScripID;

        public string SelectedScripID
        {
            get { return _SelectedScripID; }
            set { _SelectedScripID = value; NotifyPropertyChanged(nameof(SelectedScripID)); }
        }

        private int _SelectedTotBuyQuantity;

        public int SelectedTotBuyQuantity
        {
            get { return _SelectedTotBuyQuantity; }
            set { _SelectedTotBuyQuantity = value; NotifyPropertyChanged(nameof(SelectedTotBuyQuantity)); }
        }

        private int _SelectedTotSellQuantity;

        public int SelectedTotSellQuantity
        {
            get { return _SelectedTotSellQuantity; }
            set { _SelectedTotSellQuantity = value; NotifyPropertyChanged(nameof(SelectedTotSellQuantity)); }
        }

        private string _SelectedAsset;

        public string SelectedAsset
        {
            get { return _SelectedAsset; }
            set { _SelectedAsset = value; NotifyPropertyChanged(nameof(SelectedAsset)); }
        }

        private string _selectedTouchLineScripID;

        public string SelectedTouchLineScripID
        {
            get { return _selectedTouchLineScripID; }
            set { _selectedTouchLineScripID = value; onChangeOfTouchLineIsChecked(); }
        }
        private Brush _DataGridBgColor;

        public Brush DataGridBgColor
        {
            get { return _DataGridBgColor; }
            set { _DataGridBgColor = value; NotifyPropertyChanged(nameof(DataGridBgColor)); }
        }

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

        private static List<Model.Order.PendingOrder> _SearchTemplist;
        public static List<Model.Order.PendingOrder> SearchTemplist
        {
            get { return _SearchTemplist; }
            set { _SearchTemplist = value; }
        }

        #endregion


        #region RelayCommand

        private RelayCommand _Delete_Click;

        public RelayCommand Delete_Click
        {
            get { return _Delete_Click ?? (_Delete_Click = new RelayCommand((object e) => Delete_Click_ClickButton())); }

        }


        //private RelayCommand _Cancel_All_Click;

        //public RelayCommand Cancel_All_Click
        //{
        //    get { return _Cancel_All_Click ?? (_Cancel_All_Click = new RelayCommand((object e) => Delete_All_ClickButton())); }

        //}


        private RelayCommand _Modify_Click;

        public RelayCommand Modify_Click
        {
            get { return _Modify_Click ?? (_Modify_Click = new RelayCommand((object e) => Modify_ClickButton())); }

        }


        private List<Model.Order.PendingOrder> _SelectedValue = new List<Model.Order.PendingOrder>();

        public List<Model.Order.PendingOrder> SelectedValue
        {
            get { return _SelectedValue; }
            set
            {
                _SelectedValue = value;
                NotifyPropertyChanged(nameof(SelectedValue));
                //UpdateDataGrid(null);
            }
        }


        private RelayCommand _SelectionChanged;

        public RelayCommand SelectionChanged
        {
            get
            {
                return _SelectionChanged ?? (_SelectionChanged = new RelayCommand(
                    (object e) => UpdateDataGrid(e)
                        ));
            }
        }


        private RelayCommand _SaveButton;

        public RelayCommand SaveButton
        {
            get { return _SaveButton ?? (_SaveButton = new RelayCommand((object e) => SaveButton_ClickButton())); }

        }


        private RelayCommand _btnSelectAll_Click;

        public RelayCommand btnSelectAll_Click
        {
            get
            {
                return _btnSelectAll_Click ?? (_btnSelectAll_Click = new RelayCommand(
                    (object e) => btnSelectAll_Click_Click(e, null)
                        ));
            }
        }



        private RelayCommand _btnDeselectAll_Click;

        public RelayCommand btnDeselectAll_Click
        {
            get
            {
                return _btnDeselectAll_Click ?? (_btnDeselectAll_Click = new RelayCommand(
                    (object e) => btnDeselectAll_Click_Click(e, null)
                        ));
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


        private static PendingOrderClassicVM mobjInstance;
        public static PendingOrderClassicVM GETInstance
        {
            get
            {
                if (mobjInstance == null)
                {
                    mobjInstance = new PendingOrderClassicVM();
                }
                return mobjInstance;
            }
        }

        private RelayCommand _OpenBulkChng;

        public RelayCommand OpenBulkChng
        {
            get
            {
                return _OpenBulkChng ?? (_OpenBulkChng = new RelayCommand(
                    (object e) => OpenBulkChng_Click()
                        ));
            }
        }



        #endregion


        #region Constructor
        public PendingOrderClassicVM()
        {

            ObjPendingOrderCollection = new ObservableCollection<Model.Order.PendingOrder>();
            //TempObjPendingOrderCollection = new ObservableCollection<Model.Order.PendingOrder>();
            SearchTemplist = new List<Model.Order.PendingOrder>();

            // uiContext = SynchronizationContext.Current;
            Visibility();

            foreach (var order in MemoryManager.OrderDictionary.Values.ToList())
            {
                if (order.InternalOrderStatus == Enumerations.OrderExecutionStatus.Exits.ToString())
                {
                    UpdatePendingOrderMemory(order, Enumerations.OrderExecutionStatus.Exits.ToString());
                }
            }
            MktPTtxt = UtilityOrderDetails.GETInstance.MktProtection == null ? "1.0" : UtilityOrderDetails.GETInstance.MktProtection;
            //OrderProcessor.OrderResponseReceived += ReadDataFromOrderMemory;
            OrderProcessor.OnOrderResponse += ReadOrderResponse;
            OrderProcessor.OnOrderResponseReceived += UpdatePendingOrderMemory;
            //ReadDataFromOrderMemory(Enumerations.OrderExecutionStatus.Exits.ToString());

            mWindow = System.Windows.Application.Current.Windows.OfType<View.Order.PendingOrderClassic>().FirstOrDefault();
            viewSource = ((System.Windows.Data.CollectionViewSource)(mWindow.FindResource("MyItemsViewSource")));
            objBrushConvertor = new BrushConverter();
            OrderProfilingVM.OnChangeOfMarketProtection += delegate (string MarketProt) { MktPTtxt = MarketProt; };
            ColourProfilingVM.OnColorSettingChange += SetColor;

            SetColor();
        }

        private void SetColor()
        {
            DataGridBgColor = MainWindowVM.ParserDefaultCPR.GetSetting("PENDING", "PendingBackGround") != null ? objBrushConvertor.ConvertFromString(MainWindowVM.ParserDefaultCPR.GetSetting("PENDING", "PendingBackGround")) as SolidColorBrush : null;
            BuyForegroundColor = MainWindowVM.ParserDefaultCPR.GetSetting("PENDING", "PendingBuyOrder") != null ? objBrushConvertor.ConvertFromString(MainWindowVM.ParserDefaultCPR.GetSetting("PENDING", "PendingBuyOrder")) as SolidColorBrush : null;
            SellForegroundColor = MainWindowVM.ParserDefaultCPR.GetSetting("PENDING", "PendingSellOrder") != null ? objBrushConvertor.ConvertFromString(MainWindowVM.ParserDefaultCPR.GetSetting("PENDING", "PendingSellOrder")) as SolidColorBrush : null;

        }

        public Model.Order.PendingOrder CreatePendingOrder(OrderModel oOrderModel)
        {
            Model.Order.PendingOrder oPendingOrder = null;
            try
            {
                if (oOrderModel != null)
                {
                    oPendingOrder = new Model.Order.PendingOrder();
                    oOrderModel.Segment = CommonFunctions.GetSegmentID(oOrderModel.ScripCode);

                    Decimal_Point = CommonFunctions.GetDecimal(Convert.ToInt32(oOrderModel.ScripCode), "BSE", oOrderModel.Segment);

                    oPendingOrder.Segment = oOrderModel.Segment;
                    oPendingOrder.BuySell = oOrderModel.BuySellIndicator;
                    oPendingOrder.TotalQty = oOrderModel.PendingQuantity;
                    oPendingOrder.RevQty = oOrderModel.RevealQty;
                    oPendingOrder.SCode = oOrderModel.ScripCode;
                    oPendingOrder.ScripID = oOrderModel.Symbol;
                    oPendingOrder.ScripGroup = oOrderModel.Group;
                    oPendingOrder.Time = Convert.ToDateTime(oOrderModel.Time);

                    oPendingOrder.OrderType = oOrderModel.OrderType?.ToString();
                    oPendingOrder.Rate = CommonFunctions.GetValueInDecimal(oOrderModel.Price, Decimal_Point);

                    if (oPendingOrder.Segment == Enumerations.Order.ScripSegment.Debt.ToString())
                    {
                        if (oOrderModel.Price == long.MinValue || oOrderModel.Price == long.MaxValue)
                        {
                            oPendingOrder.DirtyPrice = "";
                        }
                        else
                        {
                            oPendingOrder.DirtyPrice = Convert.ToString(oOrderModel.Price);
                        }
                        if (oOrderModel.Yield == long.MinValue || oOrderModel.Yield == long.MaxValue)
                        {
                            oPendingOrder.Yield = 0;
                        }
                        else
                        {
                            oPendingOrder.Yield = oOrderModel.Yield;
                        }
                    }

                    oPendingOrder.RetainTill = oOrderModel.OrderRetentionStatus?.ToString();
                    if (oOrderModel.IsOCOOrder)
                    {
                        oPendingOrder.OCOTrgRate = CommonFunctions.GetValueInDecimal(oOrderModel.TriggerPrice, Decimal_Point);
                    }
                    oPendingOrder.ClientID = oOrderModel.ClientId;
                    oPendingOrder.OrdID = oOrderModel.OrderId;
                    oPendingOrder.OrdNumber = oOrderModel.OrderNumber;
                    oPendingOrder.ClientType = oOrderModel.ClientType;
                    oPendingOrder.CPCode = oOrderModel.ParticipantCode;
                    oPendingOrder.OrderStatus = oOrderModel.OrdStatus;
                    string key = string.Format("{0}_{1}", oOrderModel.ScripCode, oOrderModel.OrderId);
                    oPendingOrder.OrderKey = key;

                    oPendingOrder.OEREmarks = oOrderModel.OrderRemarks;
                    oPendingOrder.MessageTag = oOrderModel.MessageTag;
                    oPendingOrder.SegmentID = oOrderModel.SegmentFlag;

                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
            }
            return oPendingOrder;
        }
        public bool AddUpdateOrderToCollection(OrderModel oOrderModel)
        {
            bool result = false;
            try
            {
                Model.Order.PendingOrder oPendingOrder = new Model.Order.PendingOrder();
                oPendingOrder = CreatePendingOrder(oOrderModel);
                App.Current.Dispatcher.BeginInvoke((Action)delegate ()
                {
                    if (ObjPendingOrderCollection != null && ObjPendingOrderCollection.Count > 0)
                    {
                        if (ObjPendingOrderCollection.Any(x => x.OrderKey == oOrderModel.OrderKey))
                        {
                            int index = ObjPendingOrderCollection.IndexOf(ObjPendingOrderCollection.Where(x => x.OrderKey == oOrderModel.OrderKey).FirstOrDefault());
                            if (index != -1)
                            {
                                ObjPendingOrderCollection[index] = oPendingOrder;
                            }

                            //int filterindex = TempObjPendingOrderCollection.IndexOf(TempObjPendingOrderCollection.Where(x => x.OrderKey == oOrderModel.OrderKey).FirstOrDefault());
                            //if (filterindex != -1)
                            //{
                            //    TempObjPendingOrderCollection[filterindex] = oPendingOrder;
                            //}
                        }
                        else
                        {
                            ObjPendingOrderCollection.Add(oPendingOrder);
                            //TempObjPendingOrderCollection.Add(oPendingOrder);// ObjPendingOrderCollection;
                        }
                        if (bulkChangeWindowOpen == true)
                        {
                            if (BulkPriceChnageVM.GETInstance.selecteBulkScripList.Any(x => x.OrderKey == oOrderModel.OrderKey))
                            {
                                int index = BulkPriceChnageVM.GETInstance.selecteBulkScripList.IndexOf(BulkPriceChnageVM.GETInstance.selecteBulkScripList.Where(x => x.OrderKey == oOrderModel.OrderKey).FirstOrDefault());
                                if (index != -1)
                                {
                                    BulkPriceChnageVM.GETInstance.selecteBulkScripList[index] = oPendingOrder;
                                }

                            }
                        }
                    }
                    else
                    {
                        ObjPendingOrderCollection?.Add(oPendingOrder);
                        //TempObjPendingOrderCollection?.Add(oPendingOrder); //ObjPendingOrderCollection;
                    }
                });
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
                ExceptionUtility.LogError(ex);
            }
            return result;
        }
        public bool RemoveOrderFromCollection(OrderModel oOrderModel)
        {
            bool result = false;
            try
            {
                App.Current.Dispatcher.BeginInvoke((Action)delegate ()
                {
                    if (ObjPendingOrderCollection != null && ObjPendingOrderCollection.Count > 0)
                    {
                        if (ObjPendingOrderCollection.Any(x => x.OrderKey == oOrderModel.OrderKey))
                        {
                            int index = ObjPendingOrderCollection.IndexOf(ObjPendingOrderCollection.Where(x => x.OrderKey == oOrderModel.OrderKey).FirstOrDefault());
                            if (index != -1)
                            {
                                ObjPendingOrderCollection.RemoveAt(index);
                            }
                        }
                        if (bulkChangeWindowOpen == true)
                        {
                            if (BulkPriceChnageVM.GETInstance.selecteBulkScripList.Any(x => x.OrderKey == oOrderModel.OrderKey))
                            {
                                int index = BulkPriceChnageVM.GETInstance.selecteBulkScripList.IndexOf(BulkPriceChnageVM.GETInstance.selecteBulkScripList.Where(x => x.OrderKey == oOrderModel.OrderKey).FirstOrDefault());
                                if (index != -1)
                                {
                                    BulkPriceChnageVM.GETInstance.selecteBulkScripList.RemoveAt(index);
                                    BulkPriceChnageVM.GETInstance.titleBulkPrice = string.Format("Bulk Change : Count {0}", BulkPriceChnageVM.GETInstance.selecteBulkScripList.Count);
                                    if(BulkPriceChnageVM.GETInstance.selecteBulkScripList.Count == 0)
                                    {
                                        BulkPriceChnageVM.GETInstance.CloseWindow(null);
                                    }
                                }
                            }
                        }
                    }
                });
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
                ExceptionUtility.LogError(ex);
            }
            return result;
        }
        private void UpdatePendingOrderMemory(object orderModel, string status)
        {
            try
            {
                if (!string.IsNullOrEmpty(status) && status == Enumerations.OrderExecutionStatus.Exits.ToString())
                {
                    if (orderModel != null)
                    {
                        //deselect grid 
                        if (mWindow != null)
                            mWindow?.dataGrid?.UnselectAll();


                        if (orderModel.GetType().Name == "OrderModel")
                        {
                            OrderModel oOrderModel = new OrderModel();
                            oOrderModel = orderModel as OrderModel;
                            if (oOrderModel != null)
                            {
                                // AddInFilterMemory(oOrderModel);

                                if (new[] { "A", "U" }.Any(x => x == oOrderModel.OrderAction))
                                {
                                    //If SCripCOde txt box is not Empty 
                                    if (!string.IsNullOrEmpty(ScripCodeFilterTxt))
                                    {
                                        if (CheckIfNumber(ScripCodeFilterTxt))
                                        {
                                            if (oOrderModel.ScripCode == Convert.ToInt32(ScripCodeFilterTxt))
                                            {
                                                if (!string.IsNullOrEmpty(ClientIDFilterTxt))
                                                {
                                                    if (oOrderModel.ClientId.Trim().ToUpper() == ClientIDFilterTxt.Trim().ToUpper())
                                                    {
                                                        if (!string.IsNullOrEmpty(BuySellFiltertxt))
                                                        {
                                                            if (oOrderModel.BuySellIndicator.Trim().ToUpper() == BuySellFiltertxt.Trim().ToUpper())
                                                            {
                                                                AddInMemory(oOrderModel);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            AddInMemory(oOrderModel);
                                                        }
                                                    }
                                                }
                                                else if (!string.IsNullOrEmpty(BuySellFiltertxt))
                                                {
                                                    if (oOrderModel.BuySellIndicator.Trim().ToUpper() == BuySellFiltertxt.Trim().ToUpper())
                                                    {
                                                        AddInMemory(oOrderModel);
                                                    }
                                                }
                                                else
                                                {
                                                    AddInMemory(oOrderModel);
                                                }

                                            }

                                        }
                                        else
                                        {
                                            if (oOrderModel.Symbol.Trim().ToUpper() == ScripCodeFilterTxt.Trim().ToUpper())
                                            {
                                                if (!string.IsNullOrEmpty(ClientIDFilterTxt))
                                                {
                                                    if (oOrderModel.ClientId.Trim().ToUpper() == ClientIDFilterTxt.Trim().ToUpper())
                                                    {
                                                        if (!string.IsNullOrEmpty(BuySellFiltertxt))
                                                        {
                                                            if (oOrderModel.BuySellIndicator.Trim().ToUpper() == BuySellFiltertxt.Trim().ToUpper())
                                                            {
                                                                AddInMemory(oOrderModel);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            AddInMemory(oOrderModel);
                                                        }
                                                    }
                                                }
                                                else if (!string.IsNullOrEmpty(BuySellFiltertxt))
                                                {
                                                    if (oOrderModel.BuySellIndicator.Trim().ToUpper() == BuySellFiltertxt.Trim().ToUpper())
                                                    {
                                                        AddInMemory(oOrderModel);
                                                    }
                                                }
                                                else
                                                {
                                                    AddInMemory(oOrderModel);
                                                }

                                            }
                                        }
                                    }


                                    //ClientID
                                    else if (!string.IsNullOrEmpty(ClientIDFilterTxt))
                                    {
                                        if (oOrderModel.ClientId.Trim().ToUpper() == ClientIDFilterTxt.Trim().ToUpper())
                                        {
                                            if (!string.IsNullOrEmpty(BuySellFiltertxt))
                                            {
                                                if (oOrderModel.BuySellIndicator.Trim().ToUpper() == BuySellFiltertxt.Trim().ToUpper())
                                                {
                                                    AddInMemory(oOrderModel);
                                                }
                                            }
                                            else
                                            {
                                                AddInMemory(oOrderModel);
                                            }
                                            //AddInMemory(oOrderModel);
                                        }
                                    }

                                    else if (!string.IsNullOrEmpty(BuySellFiltertxt))
                                    {
                                        if (oOrderModel.BuySellIndicator.Trim().ToUpper() == BuySellFiltertxt.Trim().ToUpper())
                                        {
                                            AddInMemory(oOrderModel);
                                        }
                                    }

                                    else
                                    {
                                        AddInMemory(oOrderModel);
                                    }
                                }

                                else if (oOrderModel.OrderAction == "D")
                                {
                                    RemoveOrderFromCollection(oOrderModel);
                                }
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
                //FilterData();
                //ICollectionView view = CollectionViewSource.GetDefaultView(mWindow?.dataGrid?.ItemsSource);
                //if (view != null)
                //{
                //    view.SortDescriptions.Clear();
                //    view.SortDescriptions.Add(new SortDescription("Time", ListSortDirection.Descending));
                //    view.Refresh();
                //}
            }
        }

        private void AddInMemory(OrderModel oOrderModel)
        {
            if (oOrderModel.InternalOrderStatus == Enumerations.OrderExecutionStatus.Exits.ToString())
            {
                if (new[] { "A", "U" }.Any(x => x == oOrderModel.OrderAction))
                {
                    AddUpdateOrderToCollection(oOrderModel);
                }
                else if (oOrderModel.OrderAction == "D")
                {
                    RemoveOrderFromCollection(oOrderModel);
                }
            }
            else if (oOrderModel.InternalOrderStatus == Enumerations.OrderExecutionStatus.Executed.ToString()
                || oOrderModel.InternalOrderStatus == Enumerations.OrderExecutionStatus.Deleted.ToString()
                || oOrderModel.InternalOrderStatus == Enumerations.OrderExecutionStatus.Return.ToString())
            {
                RemoveOrderFromCollection(oOrderModel);
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
                            if (oOrderModel.ReplyCode == 0)
                            {
                                ClearAllFields();
                            }
                            Reply = oOrderModel.ReplyText.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                ExceptionUtility.LogError(ex);
            }
        }

        public void ClearAllFields()
        {
            TotalQtytxt = 0;
            RevQtytxt = 0;
            Ratetxt = string.Empty;
            ClientIDtxt = string.Empty;
            MktPTtxt = UtilityOrderDetails.GETInstance.MktProtection == null ? "1.0" : UtilityOrderDetails.GETInstance.MktProtection;
            txtYTM = string.Empty;
            OCOTrgRatetxt = string.Empty;
            Txtcpcode = string.Empty;
            SelectedScripID = string.Empty;
            SelectedTotBuyQuantity = 0;
            SelectedTotSellQuantity = 0;
            SelectedAsset = string.Empty;
            Reply = string.Empty;
        }

        private void OpenBulkChng_Click()
        {
            bool Validate = CkeckValidation();
            if (Validate)
            {
                //hidePendingWindowButtons();
                BulkPriceChnage oBulkPriceChnage = System.Windows.Application.Current.Windows.OfType<BulkPriceChnage>().FirstOrDefault();
                if (oBulkPriceChnage != null)
                {
                    if (oBulkPriceChnage.WindowState == WindowState.Minimized)
                        oBulkPriceChnage.WindowState = WindowState.Normal;
                    oBulkPriceChnage.Focus();
                    //oBulkPriceChnage.Activate();
                    BulkPriceChnageVM.GETInstance.populateGrid();
                    oBulkPriceChnage.Show();
                    //BulkPriceChnageVM.GETInstance.TogglePersonalWindow("Open");

                    hidePendingWindowButtons();
                    bulkChangeWindowOpen = true;

                }
                else
                {
                    oBulkPriceChnage = new BulkPriceChnage();
                    oBulkPriceChnage.Owner = System.Windows.Application.Current.MainWindow;
                    oBulkPriceChnage.Activate();
                    oBulkPriceChnage.Show();
                    hidePendingWindowButtons();
                    bulkChangeWindowOpen = true;
                }
            }

        }

        public void hidePendingWindowButtons()
        {
            btnEnable = false;
        }
        public void ShowPendingWindowButtons()
        {
            btnEnable = true;
        }

        private bool CkeckValidation()
        {


            if (SelectedValue.Count <= 1)
            {
                Reply = "Bulk Change is applicable only for multiple orders of same scrip";
                return false;
            }
            else if (SelectedValue.Count > 1)
            {
                string temp = SelectedValue[0].ScripID;

                foreach (var item in SelectedValue)
                {
                    if (temp != item.ScripID)
                    {
                        Reply = "Bulk Change is applicable only for multiple orders of same scrip";
                        return false;
                    }
                }
                return true;

            }
            else
            {
                Reply = string.Empty;
                return true;
            }
        }
        #endregion

        private void Visibility()
        {
            PopulateRetainType();
            PopulateOrderType();
            EnableOrderType = false;
            SelectAllVisible = "Visible";
            DeSelectAllVisible = "Hidden";
            //MktProtEnabled = false;
        }

        private void PopulateRetainType()
        {
            RetainType = new List<string>();
            RetainType.Add(Enumerations.Order.RetType.EOD.ToString());
            RetainType.Add(Enumerations.Order.RetType.EOS.ToString());
            RetainType.Add(Enumerations.Order.RetType.IOC.ToString());

            if (string.IsNullOrEmpty(SelectedRetainType))
                SelectedRetainType = Enumerations.Order.RetType.EOD.ToString();

        }

        private void PopulateOrderType()
        {
            OrderType = new List<string>();
            OrderType.Add("L");
            OrderType.Add("G");

            if (string.IsNullOrEmpty(SeletedOrderType))
            {
                SeletedOrderType = "L";
            }
        }

        private void UpdateDataGrid(object e)
        {
            if (SelectedValue != null && SelectedValue.Count > 0)
            {
                Reply = string.Empty;
                foreach (var item in SelectedValue)
                {
                    TotalQtytxt = item.TotalQty;
                    if (item.RevQty > item.TotalQty)
                    {
                        RevQtytxt = item.TotalQty;
                    }
                    else
                    {
                        RevQtytxt = item.RevQty;
                    }

                    Ratetxt = item.Rate;
                    ClientIDtxt = item.ClientID;
                    SelectedRetainType = item.RetainTill;
                    Txtcpcode = item.CPCode;
                    return;
                }


            }

            else
            {
                //TotalQtytxt = 0;
                //RevQtytxt = 0;
                //Ratetxt = string.Empty;
                //ClientIDtxt = string.Empty;
            }

        }

        private void UpdateDataGridSelectedItem(object e)
        {
            if (SelectedItem != null)
            {
                Reply = string.Empty;
                SelectedTotBuyQuantity = 0;
                SelectedTotSellQuantity = 0;
                //foreach (var item in SelectedItem)
                // {
                TotalQtytxt = SelectedItem.TotalQty;
                if (SelectedItem.RevQty > SelectedItem.TotalQty)
                {
                    RevQtytxt = SelectedItem.TotalQty;
                }
                else
                {
                    RevQtytxt = SelectedItem.RevQty;
                }

                Ratetxt = SelectedItem.Rate;
                ClientIDtxt = SelectedItem.ClientID;
                SelectedRetainType = SelectedItem.RetainTill;
                Txtcpcode = SelectedItem.CPCode;

                SelectedScripID = SelectedItem.ScripID?.Trim();
                foreach (var item in ObjPendingOrderCollection)
                {
                    if (item.SCode == SelectedItem.SCode)
                    {
                        if (item.BuySell.Trim().ToUpper() == "B")
                            SelectedTotBuyQuantity += item.TotalQty;
                        else if (item.BuySell.Trim().ToUpper() == "S")
                            SelectedTotSellQuantity += item.TotalQty;
                    }
                }
                SelectedAsset = CommonFunctions.GetUnderLyingAssetCode(SelectedItem.SCode, "BSE", CommonFunctions.GetSegmentID(SelectedItem.SCode));
                SeletedOrderType = SelectedItem.OrderType;
                OCOTrgRatetxt = SelectedItem.OCOTrgRate;

                // }
            }
        }

        public bool ValidateUserInputs(int TotalQty, int RevQty, string Rate, string ClientID, string MktPT,string OcoTriggerRate, OrderModel omodel, ref string Validate_Message)
        {
            try
            {
                //Quantity Validation
                Validate_Message = Validations.ValidateVolume(TotalQty, 9999999999L, 1, 0, false);
                if (Validate_Message.Trim().ToUpper() != "Success".ToUpper())
                {
                    Reply = "Quantity : " + Validate_Message;
                    return false;
                }

                //Total Qty should be Multiple of Mkt Lot
                var MarketLot = Convert.ToInt64(CommonFunctions.GetMarketLot(SelectedValue.Last().SCode));

                if (MarketLot != 0)
                {
                    if (TotalQty % MarketLot != 0)
                    {
                        Reply = "Quantity should be a multiple of Mkt lot[" + MarketLot + "]";
                        return false;
                    }
                }

                else
                {
                    Reply = "Market Lot is Zero";
                    return false;
                }

                //Validate Reveal Qty
                Validate_Message = Validations.ValidateRevlQty(RevQty, 9999999999L, 1, 0, false);
                if (Validate_Message.Trim().ToUpper() != "Success".ToUpper())
                {
                    Reply = "RevealQty : " + Validate_Message;
                    return false;
                }

                // Reveal Qty volume should not be greater than actual volume
                if (RevQty > TotalQty)
                {
                    Reply = "Disclosed quantity can't be greater than actual quantity";
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
                    Reply = "Reveal quantity should > or =  to 10 % of total quantity";
                    return false;
                }

                //Reveal Qty should be Multiple of Mkt Lot
                if (RevQty % MarketLot != 0)
                {
                    Reply = " Reveal quantity is Not a Multiple of Mkt Lot[" + MarketLot + "]";
                    return false;
                }

                //if Rate is zero
                if (Rate == "0")
                {
                    Reply = "Rate Entered is Empty";
                    return false;
                }



                //Check Number after decimal point
                var Segment = Common.CommonFunctions.GetSegmentID(SelectedValue.Last().SCode);
                var DecimalPoint = Common.CommonFunctions.GetDecimal(Convert.ToInt32(SelectedValue.Last().SCode), "BSE", Segment);
                var rate = Convert.ToString(Rate);
                if (rate.Contains(".") && rate.Substring(rate.IndexOf(".") + 1).Length > DecimalPoint)
                {
                    Reply = "Rate More than " + DecimalPoint + " decimal places!";
                    return false;
                }

                //Total Rate should be Multiple of Tick Size
                var TickSize = CommonFunctions.GetTickSize(SelectedValue.Last().SCode);
                if (!string.IsNullOrEmpty(TickSize))
                {
                    if (Convert.ToDouble(TickSize) != 0)
                    {
                        if (Convert.ToInt64(Convert.ToDouble(rate) * Math.Pow(10, DecimalPoint)) % Convert.ToInt64(Convert.ToDouble(TickSize)) != 0)
                        {
                            Reply = "Rate should be a multiple of TickSize[" + TickSize + "]";
                            return false;
                        }
                    }
                    else
                    {
                        Reply = "Tick size is Zero.";
                        return false;
                    }
                }

                else
                {
                    Reply = "Tick size is Zero.";
                    return false;
                }

                if (!String.IsNullOrEmpty(MktPT))
                {
                    int i;
                    if (int.TryParse(MktPT, out i))

                    {
                        if (!(i >= 1 && i <= 99))
                        {
                            Reply = "MProt % can not be greater than 99.00 %";

                            return false;

                        }

                    }

                    else if (!Regex.IsMatch(MktPT, @"^\d{1,2}(\.\d{1})?$"))
                    {
                        Reply = "MProt % more than 1 decimal places!";
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
                Reply = "Error In Validation of order";
                return false;
            }

            return true;

        }

        private void btnSelectAll_Click_Click(object e, object p)
        {
            //DataGrid objDataGrid = System.Windows.Application.Current.Windows.OfType<PendingOrderClassic>().FirstOrDefault().dataGrid;
            //objDataGrid.SelectAll();
            //SelectAllVisible = "Hidden";
            //DeSelectAllVisible = "Visible";
            // Reply = ObjPendingOrderCollection + " Order(s) selected";


            if (btnSelectAllContent == "Select All")
            {
                SelectedValue = ObjPendingOrderCollection.ToList();
                if (mWindow != null)
                {
                    mWindow.dataGrid.SelectAll();
                    mWindow.dataGrid.Focus();
                }

                Reply = SelectedValue.Count + " Order(s) Selected";
                btnSelectAllContent = "Deselect All";
            }
            else if (btnSelectAllContent == "Deselect All")
            {
                Reply = SelectedValue.Count + " Order(s) Deselected";
                SelectedValue.Clear();
                if (mWindow != null)
                {
                    mWindow.dataGrid.UnselectAll();
                    mWindow.dataGrid.Focus();
                }
                btnSelectAllContent = "Select All";
            }
        }

        private void btnDeselectAll_Click_Click(object e, object p)
        {
            DataGrid objDataGrid = System.Windows.Application.Current.Windows.OfType<PendingOrderClassic>().FirstOrDefault().dataGrid;
            objDataGrid.UnselectAllCells();
        }

        /// <summary>
        /// Reads from Order Memory whose status is Exits
        /// </summary>
        private void ReadDataFromOrderMemory(string status)
        {
            if (status == Enumerations.OrderExecutionStatus.Exits.ToString())
            {
                ObjPendingOrderCollection?.Clear();
                //TempObjPendingOrderCollection?.Clear();
                if (OrderDictionary != null && OrderDictionary.Count > 0)
                {
                    foreach (OrderModel item in OrderDictionary.Values.Where(x => x.InternalOrderStatus == Enumerations.OrderExecutionStatus.Exits.ToString()))
                    {
                        Model.Order.PendingOrder oPendingOrder = new Model.Order.PendingOrder();
                        Decimal_Point = CommonFunctions.GetDecimal(Convert.ToInt32(item.ScripCode), "BSE", CommonFunctions.GetSegmentID(item.ScripCode));
                        oPendingOrder.BuySell = item.BuySellIndicator;
                        //if (item.BuySellIndicator == Enumerations.Order.BuySellFlag.BUY.ToString())
                        //{
                        //    oPendingOrder.BuySell = Enumerations.SideShort.B.ToString();
                        //}
                        //else if (item.BuySellIndicator == Enumerations.Order.BuySellFlag.SELL.ToString())
                        //{
                        //    oPendingOrder.BuySell = Enumerations.SideShort.S.ToString();
                        //}

                        item.Segment = CommonFunctions.GetSegmentID(item.ScripCode);
                        oPendingOrder.Segment = item.Segment;

                        oPendingOrder.TotalQty = item.PendingQuantity;
                        oPendingOrder.RevQty = item.RevealQty;
                        oPendingOrder.SCode = item.ScripCode;
                        oPendingOrder.ScripID = item.Symbol;
                        oPendingOrder.Time = Convert.ToDateTime(item.Time);
#if BOW
                    oPendingOrder.Rate = Convert.ToDouble(Convert.ToDouble(item.Price) / Math.Pow(10, Decimal_Point));
                    oPendingOrder.PendingQty = item.PendingQuantity;
                    oPendingOrder.DirtyPrice = Convert.ToString(item.Price);
#elif TWS
                        oPendingOrder.OrderType = item.OrderType?.ToString();
                        //if (Decimal_Point == 2)

                        //    oPendingOrder.Rate = Convert.ToDouble(Convert.ToDouble(item.Price) / Math.Pow(10, Decimal_Point));

                        if (Decimal_Point == 4)
                            oPendingOrder.Rate = string.Format("{0:0.0000}", item.Price / Math.Pow(10, Decimal_Point));
                        else if (Decimal_Point == 2)
                            oPendingOrder.Rate = string.Format("{0:0.00}", item.Price / Math.Pow(10, Decimal_Point));
                        else
                            oPendingOrder.Rate = string.Format("{0:0.00}", item.Price / Math.Pow(10, Decimal_Point));

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
                        if (item.IsOCOOrder)
                        {
                            oPendingOrder.OCOTrgRate = string.Format("{0:0.0000}", item.TriggerPrice / Math.Pow(10, Decimal_Point));
                        }
                        oPendingOrder.OrdID = item.OrderId;
                        oPendingOrder.OrdNumber = item.OrderNumber;
                        oPendingOrder.ClientType = item.ClientType;
                        oPendingOrder.CPCode = item.ParticipantCode;
                        oPendingOrder.OrderStatus = item.OrdStatus;
                        string key = string.Format("{0}_{1}", item.ScripCode, item.OrderId);
                        oPendingOrder.OrderKey = key;
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
                        ObjPendingOrderCollection?.Add(oPendingOrder);
                        //TempObjPendingOrderCollection = ObjPendingOrderCollection;
                    }
                }
                FilterData();
                ICollectionView view = CollectionViewSource.GetDefaultView(mWindow?.dataGrid?.ItemsSource);
                if (view != null)
                {
                    view.SortDescriptions.Clear();
                    view.SortDescriptions.Add(new SortDescription("Time", ListSortDirection.Descending));
                    view.Refresh();
                }
                //ObjPendingOrderCollection = new ObservableCollection<Model.Order.PendingOrder>(ObjPendingOrderCollection.OrderByDescending(i => i.Time));  //ObjPendingOrderCollection.OrderByDescending(x => x.CurrentTime);
                //mWindow.dataGrid.UpdateLayout();
            }
        }


        /// <summary>
        /// Called when delete button is clicked. Deletes selected pending order
        /// </summary>
        private void Delete_Click_ClickButton()
        {
            try
            {
                DeleteThread = new Thread(DeleteOnBackGroundThread);
                DeleteThread.Start();


                //                #region Delete all Pending Orders
                //                if (ObjPendingOrderCollection != null && ObjPendingOrderCollection.Count > 0)
                //                {
                //                    if (MessageBox.Show("Do you really want to delete all the pending orders?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                //                    {
                //                        foreach (Model.Order.PendingOrder item in ObjPendingOrderCollection)
                //                        {
                //#if BOW
                //                            OrderProcessor.SendDeleteOrderRequest(OrderDictionary.Select(x => x.Value).Where(x => x.OrderRemarks == item.OEREmarks).FirstOrDefault());
                //#elif TWS
                //                            // OrderProcessor.SendDeleteOrderRequest(item.MessageTag);
                //                            OrderRequestProcessor oOrderRequestProcessor = new OrderRequestProcessor(new DeleteOrder());
                //                            OrderModel oOrderModel = new OrderModel();
                //                            MemoryManager.OrderDictionary.TryGetValue(item.OrderKey, out oOrderModel);
                //                            if (oOrderModel != null)
                //                            {
                //                                oOrderModel.OrderAction = "D";
                //                                oOrderRequestProcessor.ProcessRequest(oOrderModel);
                //                            }
                //                            else
                //                            {
                //                                Reply = "Can't find order";
                //                            }
                //#endif
                //                        }
                //                    }
                //                }

                //                else
                //                {
                //                    //MessageBox.Show("No pending orders to delete", "Delete Orders", MessageBoxButton.OK, MessageBoxImage.Warning);
                //                    Reply = "No pending orders to delete";
                //                }
                //                #endregion
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
            }

        }

        private void DeleteOnBackGroundThread()
        {
            #region Delete Selected Pending Orders

            if (!string.IsNullOrEmpty(txtSCripCodeScripID))
            {
                if (MessageBox.Show("Do you really want to delete Order(s)?", "Delete Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    bool Number = CheckIfNumber(txtSCripCodeScripID);
                    if (Number)
                    {
                        foreach (var item in ObjPendingOrderCollection.Where(x => x.SCode == Convert.ToInt64(txtSCripCodeScripID.Trim())).ToList())
                        {
                            //Set to False
                            DeleteOrder(item);
                            PendingOrderDeleteEvent.WaitOne();
                        }
                    }
                    else
                    {
                        foreach (var item in ObjPendingOrderCollection.Where(x => x.ScripID.Trim().ToUpper() == txtSCripCodeScripID.Trim().ToUpper()).ToList())
                        {
                            //Set to False
                            DeleteOrder(item);
                            PendingOrderDeleteEvent.WaitOne();
                        }
                    }
                }
            }
            else
            {
                if (SelectedValue != null && SelectedValue.Count > 0)
                {
                    if (MessageBox.Show("Do you really want to delete Selected Order(s)?", "Delete Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {
                        foreach (var item in SelectedValue.ToList())
                        {
#if BOW
                            OrderProcessor.SendDeleteOrderRequest(OrderDictionary.Select(x => x.Value).Where(x => x.OrderRemarks == item.OEREmarks).FirstOrDefault());
#elif TWS
                            //OrderProcessor.SendDeleteOrderRequest(item.MessageTag);
                            //OrderRequestProcessor oOrderRequestProcessor = new OrderRequestProcessor(new DeleteOrder());
                            //OrderModel oOrderModel = new OrderModel();
                            //MemoryManager.OrderDictionary.TryGetValue(item.OrderKey, out oOrderModel);
                            //if (oOrderModel != null)
                            //{
                            //    oOrderModel.OrderAction = "D";
                            //    oOrderRequestProcessor.ProcessRequest(oOrderModel);
                            //    //Reply = "Selected
                            //}
                            //else
                            //{
                            //    Reply = "Can't find order";
                            //}

                            //Set to False
                            DeleteOrder(item);
                            PendingOrderDeleteEvent.WaitOne();
#endif

                        }
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    //MessageBox.Show("Please Select the Order to Delete", "Delete Order", MessageBoxButton.OK, MessageBoxImage.Warning);
                    Reply = "Please Select the Order to Delete";
                }
            }
            #endregion
        }


        /// <summary>
        /// Callled when Cancel All button is clicked. Deletes all pending orders
        /// </summary>
        //        private void Delete_All_ClickButton()
        //        {
        //            try
        //            {
        //                if (ObjPendingOrderCollection != null && ObjPendingOrderCollection.Count > 0)
        //                {
        //                    if (MessageBox.Show("Do you really want to delete all the pending orders?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
        //                    {
        //                        foreach (Model.Order.PendingOrder item in ObjPendingOrderCollection)
        //                        {
        //#if BOW
        //                            OrderProcessor.SendDeleteOrderRequest(OrderDictionary.Select(x => x.Value).Where(x => x.OrderRemarks == item.OEREmarks).FirstOrDefault());
        //#elif TWS
        //                            // OrderProcessor.SendDeleteOrderRequest(item.MessageTag);
        //                            OrderRequestProcessor oOrderRequestProcessor = new OrderRequestProcessor(new DeleteOrder());
        //                            OrderModel oOrderModel = new OrderModel();
        //                            MemoryManager.OrderDictionary.TryGetValue(item.OrderKey, out oOrderModel);
        //                            if (oOrderModel != null)
        //                            {
        //                                oOrderModel.OrderAction = "D";
        //                                oOrderRequestProcessor.ProcessRequest(oOrderModel);
        //                            }
        //                            else
        //                            {
        //                                Reply = "Can't find order";
        //                            }
        //#endif
        //                        }
        //                    }
        //                }

        //                else
        //                {
        //                    //MessageBox.Show("No pending orders to delete", "Delete Orders", MessageBoxButton.OK, MessageBoxImage.Warning);
        //                    Reply = "No pending orders to delete";
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                ExceptionUtility.LogError(ex);
        //            }
        //        }


        /// <summary>
        /// Called When Change Button is clicked. Pending order can be modified
        /// All pending then Client ID and Client Type can be changed for that particular Scrip.
        /// If trader quantity is greater than zero then Client ID and Client Type can be changed for that particular Scrip
        /// User is allowed to change Qty, Rate, Retention, Order Type and market protection
        /// </summary>
        private void Modify_ClickButton()
        {

            try
            {
                //if (SelectedValue != null)
                if (SelectedValue != null && SelectedValue.Count > 0)
                {
                                   
                    //foreach (var item in SelectedValue)
                    //{
#if BOW
                    //OrderProcessor.GetOrderDataFromOrderID(SelectedValue.FirstOrDefault());                  
                    OrderProcessor.GetOrderDataFromOrderID(SelectedValue.FirstOrDefault());
#elif TWS
                    //OrderProcessor.GetOrderDataByMessageTag(SelectedValue.FirstOrDefault());
                    OrderRequestProcessor oOrderRequestProcessor = new OrderRequestProcessor(new ModifyOrder());
                    OrderModel oOrderModel;
                    string key = SelectedValue.Last().OrderKey;
                    MemoryManager.OrderDictionary.TryGetValue(key, out oOrderModel);

                    if (oOrderModel != null)
                    {
                        bool validate = ValidateUserInputs(TotalQtytxt, RevQtytxt, Ratetxt, ClientIDtxt, MktPTtxt, OCOTrgRatetxt, oOrderModel, ref Validate_Message);
                        if (!validate)
                        {
                            Reply = Validate_Message;
                            return;
                        }
                        OrderModel oTempOrderModel = new OrderModel();
                        var DecimalPoint = CommonFunctions.GetDecimal(System.Convert.ToInt32(oOrderModel.ScripCode), "BSE", CommonFunctions.GetSegmentID(oOrderModel.ScripCode));
                        oTempOrderModel.OrderAction = "U";
                        oTempOrderModel.OriginalQty = TotalQtytxt;
                        //oTempOrderModel.PendingQuantity = oTempOrderModel.OriginalQty;
                        //oTempOrderModel.PendingQuantity = oOrderModel.PendingQuantity;
                        oTempOrderModel.RevealQty = RevQtytxt;
                        oTempOrderModel.Price = Convert.ToInt64(Convert.ToDouble(Ratetxt) * Math.Pow(10, DecimalPoint));//Convert.ToInt32(Ratetxt);
                        oTempOrderModel.OrderType = SeletedOrderType;
                        //Convert Price in Long and send to processing
                        if (oTempOrderModel.OrderType == "G")//oTempOrderModel.OrderType == "STOPLOSSMKT")
                        {
                            oTempOrderModel.Price = 0;
                        }
                        else//for limit
                        {
                            oTempOrderModel.Price = Convert.ToInt64(Convert.ToDouble(Ratetxt) * Math.Pow(10, DecimalPoint));
                        }
                        oTempOrderModel.Exchange = Enumerations.Order.Exchanges.BSE.ToString();
                        oTempOrderModel.ScreenId = (int)Enumerations.WindowName.Pending_Order;
                        oTempOrderModel.OrderRetentionStatus = SelectedRetainType;
                        oTempOrderModel.ClientId = ClientIDtxt;
                        oTempOrderModel.OrderType = SeletedOrderType;
                        if (oTempOrderModel.OrderType == "G")
                        {
                            oTempOrderModel.ProtectionPercentage = Convert.ToString(Convert.ToInt32(Convert.ToDecimal(MktPTtxt) * 100));
                        }
                        else
                        {
                            oTempOrderModel.ProtectionPercentage = "0";
                        }
                        oTempOrderModel.OrderId = oOrderModel.OrderId;
                        oTempOrderModel.BuySellIndicator = oOrderModel.BuySellIndicator;
                        oTempOrderModel.OrderKey = key;
                        oTempOrderModel.ScripCode = SelectedValue.Last().SCode;
                        oTempOrderModel.Segment = CommonFunctions.GetSegmentID(oTempOrderModel.ScripCode);
                        oTempOrderModel.SegmentFlag = CommonFunctions.SegmentFlag(oTempOrderModel.Segment); //(int)Enum.Parse(typeof(Enumerations.Order.ScripSegment), oTempOrderModel.Segment);
                        oTempOrderModel.ClientType = SelectedValue.Last().ClientType;
                        oTempOrderModel.ParticipantCode = SelectedValue.Last().CPCode;
                        oTempOrderModel.Symbol = oOrderModel.Symbol;
                        if (oOrderModel.IsOCOOrder)
                        {
                            oTempOrderModel.TriggerPrice = Convert.ToInt64(Convert.ToDouble(OCOTrgRatetxt) * Math.Pow(10, DecimalPoint));
                        }
                        if (oTempOrderModel.OrderType == "L" && oTempOrderModel.TriggerPrice > 0)
                        {
                            oTempOrderModel.IsOCOOrder = true;
                        }
                        //oTempOrderModel.BuySellIndicator = oOrderModel.BuySellIndicator;
                        oTempOrderModel.MarketLot = oOrderModel.MarketLot;
                        oTempOrderModel.TickSize = oOrderModel.TickSize;
                        oTempOrderModel.Group = oOrderModel.Group;
                        oOrderRequestProcessor.ProcessRequest(oTempOrderModel);
                    }
                    else
                    {
                        //MessageBox.Show("Can't find order");
                        Reply = "Can't find order";
                    }
#endif
                }
                //}
                else
                {
                    // MessageBox.Show("Please Select the Order to Modify", "Modify Order", MessageBoxButton.OK, MessageBoxImage.Warning);
                    Reply = "Please Select the Order to Modify";
                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
            }
        }

        public void Validation()
        {
            if (!String.IsNullOrEmpty(MktPTtxt))
            {
                int i;
                if (int.TryParse(MktPTtxt, out i))

                {
                    if (!(i >= 1 && i <= 99))
                    {
                        Reply = "MProt % can not be greater than 99.00 %";

                        return;

                    }

                }



                //if (!Regex.IsMatch(MarketProtection, @"^[0 - 9]\d{ 0,9} (\.\d{ 1,3})?%?$"))
                //if (!Regex.IsMatch(MarketProtection, @" ?<=^| )\d + (\.\d +)?(?=$| )| (?<=^| )\.\d + (?=$|"))
                else if (!Regex.IsMatch(MktPTtxt, @"^\d{1,2}(\.\d{1})?$"))
                {
                    // BtnSaveEnable = Boolean.TrueString;
                    Reply = "MProt % more than 1 decimal places!";

                    return;
                }
            }

        }
        private void SaveButton_ClickButton()
        {
            if (ObjPendingOrderCollection.Count == 0)
            {
                MessageBox.Show("No Pending Orders to Save","!Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
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
                writer = new StreamWriter(dlg.FileName, true, Encoding.UTF8);
                try
                {
                    writer.Write("BuySell, TotalQty, RevQty, SCode, Rate, ClientID, RetainTill, ClientType, OrderType, ScripID, CPCode, ScripGroup");
                    writer.Write(writer.NewLine);

                    foreach (var dr in ObjPendingOrderCollection)
                    {
                        writer.Write(dr.BuySell + "," + dr.TotalQty + "," + dr.RevQty + "," + dr.SCode + "," + Convert.ToDouble(dr.Rate) * 100 + "," + dr.ClientID + "," + dr.RetainTill + "," + dr.ClientType + "," + dr.OrderType + "," + dr.ScripID + "," + dr.CPCode + "," + dr.ScripGroup
                           /* + dr.OrdID + "," + dr.OCOTrgRate + "," + dr.Yield + "," + dr.DirtyPrice*/);

                        writer.Write(writer.NewLine);
                    }

                    System.Windows.MessageBox.Show("Pending Orders Saved in file :" + dlg.FileName.ToString(), "Message", MessageBoxButton.OK);
                }
                catch (Exception e)
                {
                    ExceptionUtility.LogError(e);
                    System.Windows.MessageBox.Show("Error in Exporting data in CSV Format","!Error!",MessageBoxButton.OK,MessageBoxImage.Error);
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

        private void OnChangeOfBuySellFiltertxt()
        {
            try
            {
                FilterData();
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
            }
        }

        private bool CheckIfNumber(string strtoCheck)
        {
            int i = 0;
            bool result = int.TryParse(strtoCheck, out i);
            return result;
        }

        private void onChangeOfScripCodeFilterTxt()
        {
            FilterData();
        }

        private void onChangeOfClientIDFilterTxt()
        {
            FilterData();
        }

        private void onChangeOfTouchLineIsChecked()
        {
            if (TouchLineIsChecked)
            {
                if (!string.IsNullOrEmpty(UtilityLoginDetails.GETInstance.SelectedTouchLineScripID))
                {
                    ScripCodeFilterTxt = UtilityLoginDetails.GETInstance.SelectedTouchLineScripID.Trim();
                }
            }
            else
            {
                ScripCodeFilterTxt = string.Empty;
            }
        }
        private void onChangeOfFiveLakhChk()
        {
            FilterData();
        }

        private void onChangeOfScripCodeTextBox()
        {
            try
            {


            }
            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
            }
        }

        private void DeleteOrder(Model.Order.PendingOrder item)
        {
            OrderRequestProcessor oOrderRequestProcessor = new OrderRequestProcessor(new DeleteOrder());
            OrderModel oOrderModel = new OrderModel();
            OrderDictionary.TryGetValue(item.OrderKey, out oOrderModel);
            if (oOrderModel != null)
            {
                oOrderModel.OrderAction = "D";
                oOrderRequestProcessor.ProcessRequest(oOrderModel);
            }
            else
            {
                Reply = "Can't find order";
            }
        }

        private void FilterData()
        {
            try
            {

                bool Number = CheckIfNumber(ScripCodeFilterTxt);
                if (Number)
                {
                    long ScripCode = 0;
                    if (FiveLakhChk)
                    {
                        if (Convert.ToInt64(ScripCodeFilterTxt) < 100000)
                            ScripCode = Convert.ToInt64(ScripCodeFilterTxt) + 500000;
                        else
                            ScripCode = Convert.ToInt64(ScripCodeFilterTxt);
                    }
                    else
                        ScripCode = Convert.ToInt64(ScripCodeFilterTxt);


                    ObjPendingOrderCollection = new ObservableCollection<Model.Order.PendingOrder>();

                    var ListofFiltersData = OrderDictionary.Values.Where(x => (!string.IsNullOrEmpty(BuySellFiltertxt) ? BuySellFiltertxt.Trim().ToUpper() == x.BuySellIndicator.Trim().ToUpper() : true)
            && (!string.IsNullOrEmpty(ClientIDFilterTxt) ? x.ClientId.Trim().ToUpper().StartsWith(ClientIDFilterTxt.Trim().ToUpper()) : true)
            && (!string.IsNullOrEmpty(ScripCodeFilterTxt) ? x.ScripCode.ToString().Trim().ToUpper().StartsWith(ScripCode.ToString().Trim().ToUpper()) : true)
            && (FiveLakhChk ? (ScripCode >= 500000 && ScripCode <= 599999) : true)).ToList();

                    foreach (var item in ListofFiltersData.Where(x => x.InternalOrderStatus == Enumerations.OrderExecutionStatus.Exits.ToString()).ToList())
                    {
                        Model.Order.PendingOrder oPendingOrder = new Model.Order.PendingOrder();
                        oPendingOrder = CreatePendingOrder(item);
                        ObjPendingOrderCollection.Add(oPendingOrder);
                    }

                    //        ObjPendingOrderCollection = new ObservableCollection<Model.Order.PendingOrder>(TempObjPendingOrderCollection.Where(
                    //x => (!string.IsNullOrEmpty(BuySellFiltertxt) ? BuySellFiltertxt.Trim().ToUpper() == x.BuySell.Trim().ToUpper() : true)
                    //&& (!string.IsNullOrEmpty(ClientIDFilterTxt) ? x.ClientID.Trim().ToUpper().StartsWith(ClientIDFilterTxt.Trim().ToUpper()) : true)
                    //&& (!string.IsNullOrEmpty(ScripCodeFilterTxt) ? x.SCode.ToString().Trim().ToUpper().StartsWith(ScripCode.ToString().Trim().ToUpper()) : true)
                    //&& (FiveLakhChk ? (ScripCode >= 500000 && ScripCode <= 599999) : true)));

                }
                else
                {
                    //ObjPendingOrderCollection = new ObservableCollection<Model.Order.PendingOrder>(TempObjPendingOrderCollection.Where(
                    //x => (!string.IsNullOrEmpty(BuySellFiltertxt) ? BuySellFiltertxt.Trim().ToUpper() == x.BuySell.Trim().ToUpper() : true)
                    //&& (!string.IsNullOrEmpty(ClientIDFilterTxt) ? x.ClientID.Trim().ToUpper().StartsWith(ClientIDFilterTxt.Trim().ToUpper()) : true)
                    //&& (!string.IsNullOrEmpty(ScripCodeFilterTxt) ? x.ScripID.ToString().Trim().ToUpper().StartsWith(ScripCodeFilterTxt.Trim().ToUpper()) : true)));

                    ObjPendingOrderCollection = new ObservableCollection<Model.Order.PendingOrder>();

                    var ListofFiltersData = OrderDictionary.Values.Where(x => (!string.IsNullOrEmpty(BuySellFiltertxt) ? BuySellFiltertxt.Trim().ToUpper() == x.BuySellIndicator.Trim().ToUpper() : true)
                   && (!string.IsNullOrEmpty(ClientIDFilterTxt) ? x.ClientId.Trim().ToUpper().StartsWith(ClientIDFilterTxt.Trim().ToUpper()) : true)
                   && (!string.IsNullOrEmpty(ScripCodeFilterTxt) ? x.Symbol.ToString().Trim().ToUpper().StartsWith(ScripCodeFilterTxt.Trim().ToUpper()) : true)).ToList();

                    foreach (var item in ListofFiltersData.Where(x => x.InternalOrderStatus == Enumerations.OrderExecutionStatus.Exits.ToString()).ToList())
                    {
                        Model.Order.PendingOrder oPendingOrder = new Model.Order.PendingOrder();
                        oPendingOrder = CreatePendingOrder(item);
                        ObjPendingOrderCollection.Add(oPendingOrder);
                    }


                }

            }
            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
            }
        }

        private void CloseWindowsOnEscape_Click()
        {
            mWindow?.Close();
        }

    }

    public partial class PendingOrderClassicVM : BaseViewModel
    {
#if TWS

#endif
    }

    public partial class PendingOrderClassicVM : BaseViewModel
    {
#if BOW

#endif
    }

}
