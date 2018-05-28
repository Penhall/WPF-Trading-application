using CommonFrontEnd.Common;
using CommonFrontEnd.Global;
using CommonFrontEnd.Model.Order;
using CommonFrontEnd.Processor.Order;
using CommonFrontEnd.SharedMemories;
using CommonFrontEnd.View.Order;
using CommonFrontEnd.ViewModel.Profiling;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace CommonFrontEnd.ViewModel.Order
{
    public class BatchOrderVM : BaseViewModel
    {
        SynchronizationContext uiContext = SynchronizationContext.Current;
        DirectoryInfo CsvFilesPath = new DirectoryInfo(Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"Profile/")));
        public string winName = String.Empty;
        public static string previousPath = string.Empty;
        static View.Order.BatchOrder batch = null;
        public static AutoResetEvent batchOrderAutoReset = new AutoResetEvent(false);
        public static List<object> BatchOrderQueue = new List<object>();
        public int DecimalPoint = 0;
        public static int OrderMaxLimit = 1000000000;
        public static double MAXRATELIMIT = 21400000.00;
        public static double MAXNEGATIVERATELIMIT = -21400000.00;
        public static int ChangedRecords = 0, ErrorRecords=0,IgnoredRecords=0;

        #region SingleInstance
        private static BatchOrderVM _GetInstance;

        public static BatchOrderVM GetInstance
        {
            get
            {
                if (_GetInstance == null)
                {
                    _GetInstance = new BatchOrderVM();
                }
                return _GetInstance;
            }
        }

        #endregion

        #region RelayCommand
        private RelayCommand _OnClickOfChangePrice;
        public RelayCommand OnClickOfChangePrice
        {
            get
            {
                return _OnClickOfChangePrice ?? (_OnClickOfChangePrice = new RelayCommand(
                    (object e1) => OnChangeOfBulkPrice(e1)));
            }
        }

        private RelayCommand _FastOrderCommand;
        public RelayCommand FastOrderCommand
        {
            get
            {
                return _FastOrderCommand ?? (_FastOrderCommand = new RelayCommand(
                    (object e1) => OnEntryOfFastOrderEntryFields()));
            }
        }

        private RelayCommand _BtnLoadFile;
        public RelayCommand BtnLoadFile
        {
            get
            {
                return _BtnLoadFile ?? (_BtnLoadFile = new RelayCommand(
                    (object e1) => OnClickOfLoadButton()));
            }
        }

        private RelayCommand _btnSubmit;
        public RelayCommand btnSubmit
        {
            get
            {
                return _btnSubmit ?? (_btnSubmit = new RelayCommand(
                    (object e1) => OnClickOfSubmitButton()));
            }
        }


        private RelayCommand _BtnSelectAll;
        public RelayCommand BtnSelectAll
        {
            get
            {
                return _BtnSelectAll ?? (_BtnSelectAll = new RelayCommand(
                    (object e) => OnClickOfSelectAllButton(e)));
            }
        }


        private RelayCommand _BtnRemove;
        public RelayCommand BtnRemove
        {
            get
            {
                return _BtnRemove ?? (_BtnRemove = new RelayCommand(
                    (object e1) => OnClickOfRemoveButton()));
            }
        }



        private RelayCommand _BtnRemoveAll;
        public RelayCommand BtnRemoveAll
        {
            get
            {
                return _BtnRemoveAll ?? (_BtnRemoveAll = new RelayCommand(
                    (object e1) => OnClickOfRemoveAllButton()));
            }
        }


        private RelayCommand _BtnChange;
        public RelayCommand BtnChange
        {
            get
            {
                return _BtnChange ?? (_BtnChange = new RelayCommand(
                    (object e1) => OnClickOfChangeButton()));
            }
        }

        private RelayCommand _BtnSave;
        public RelayCommand BtnSave
        {
            get
            {
                return _BtnSave ?? (_BtnSave = new RelayCommand(
                    (object e1) => OnClickOfSaveButton()));
            }
        }

        private RelayCommand _BtnStop;
        public RelayCommand BtnStop
        {
            get
            {
                return _BtnStop ?? (_BtnStop = new RelayCommand(
                    (object e1) => OnClickOfStopButton()));
            }
        }

        private RelayCommand _BtnBondCalc;
        public RelayCommand BtnBondCalc
        {
            get
            {
                return _BtnBondCalc ?? (_BtnBondCalc = new RelayCommand(
                    (object e1) => OnClickOfBondCalcButton()));
            }
        }

        private RelayCommand _ShortCut_Escape;

        public RelayCommand ShortCut_Escape
        {
            get
            {
                return _ShortCut_Escape ?? (_ShortCut_Escape = new RelayCommand(
                    (object e) => EscapeUsingUserControl(e)
                        ));
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

        private RelayCommand _txtMktProt_TextChanged;

        public RelayCommand txtMktProt_TextChanged
        {
            get { return _txtMktProt_TextChanged ?? (_txtMktProt_TextChanged = new RelayCommand((object e) => TotalMarketProtectionValidation(e))); }
        }

        #endregion

        #region List and Collections
        private string _BulkScripCodeID;
        public string BulkScripCodeID
        {
            get { return _BulkScripCodeID; }
            set
            {
                _BulkScripCodeID = value;
                NotifyPropertyChanged("BulkScripCodeID");
            }
        }

        private string _BulkNewPrice;
        public string BulkNewPrice
        {
            get { return _BulkNewPrice; }
            set
            {
                _BulkNewPrice = value;
                NotifyPropertyChanged("BulkNewPrice");
            }
        }

        private string _BulkNewTrgPrice;
        public string BulkNewTrgPrice
        {
            get { return _BulkNewTrgPrice; }
            set
            {
                _BulkNewTrgPrice = value;
                NotifyPropertyChanged("BulkNewTrgPrice");
            }
        }

        private string _LoadMarketProtection;
        public string LoadMarketProtection
        {
            get { return _LoadMarketProtection; }
            set
            {
                _LoadMarketProtection = value;
                NotifyPropertyChanged("LoadMarketProtection");
            }
        }

        private string _ModCPCode;
        public string ModCPCode
        {
            get { return _ModCPCode; }
            set
            {
                _ModCPCode = value;
                NotifyPropertyChanged(nameof(ModCPCode));
            }
        }

        private bool _IsSubmitEnabled;
        public bool IsSubmitEnabled
        {
            get { return _IsSubmitEnabled; }
            set
            {
                _IsSubmitEnabled = value;
                NotifyPropertyChanged(nameof(IsSubmitEnabled));
            }
        }

        private string _txtSS;
        public string txtSS
        {
            get { return _txtSS; }
            set
            {
                _txtSS = value;
                NotifyPropertyChanged(nameof(txtSS));
            }
        }

        private string _txtMM;
        public string txtMM
        {
            get { return _txtMM; }
            set
            {
                _txtMM = value;
                NotifyPropertyChanged(nameof(txtMM));
            }
        }


        private string _txtHH;
        public string txtHH
        {
            get { return _txtHH; }
            set
            {
                _txtHH = value;
                NotifyPropertyChanged(nameof(txtHH));
            }
        }


        private bool _IsSetTimeEnabled;
        public bool IsSetTimeEnabled
        {
            get { return _IsSetTimeEnabled; }
            set
            {
                _IsSetTimeEnabled = value;
                NotifyPropertyChanged(nameof(IsSetTimeEnabled));
            }
        }

        private bool _IsTxtSSEnabled;
        public bool IsTxtSSEnabled
        {
            get { return _IsTxtSSEnabled; }
            set
            {
                _IsTxtSSEnabled = value;
                NotifyPropertyChanged(nameof(IsTxtSSEnabled));
            }
        }

        private bool _IsTxtMMEnabled;
        public bool IsTxtMMEnabled
        {
            get { return _IsTxtMMEnabled; }
            set
            {
                _IsTxtMMEnabled = value;
                NotifyPropertyChanged(nameof(IsTxtMMEnabled));
            }
        }


        private bool _IsTxtHHEnabled;
        public bool IsTxtHHEnabled
        {
            get { return _IsTxtHHEnabled; }
            set
            {
                _IsTxtHHEnabled = value;
                NotifyPropertyChanged(nameof(IsTxtHHEnabled));
            }
        }


        private bool _IsAutoSubmitChecked;
        public bool IsAutoSubmitChecked
        {
            get { return _IsAutoSubmitChecked; }
            set
            {
                _IsAutoSubmitChecked = value;
                NotifyPropertyChanged(nameof(IsAutoSubmitChecked));
                CheckPropertiesOnCheckOfAutoSubmitCheckBox();
            }
        }

        private bool _IsBulkChangePriceEnabled;
        public bool IsBulkChangePriceEnabled
        {
            get { return _IsBulkChangePriceEnabled; }
            set
            {
                _IsBulkChangePriceEnabled = value;
                NotifyPropertyChanged(nameof(IsBulkChangePriceEnabled));
            }
        }


        private bool _IsBulkNewTrgPriceEnabled;
        public bool IsBulkNewTrgPriceEnabled
        {
            get { return _IsBulkNewTrgPriceEnabled; }
            set
            {
                _IsBulkNewTrgPriceEnabled = value;
                NotifyPropertyChanged(nameof(IsBulkNewTrgPriceEnabled));
            }
        }


        private bool _IsBulkNewPriceEnabled;
        public bool IsBulkNewPriceEnabled
        {
            get { return _IsBulkNewPriceEnabled; }
            set
            {
                _IsBulkNewPriceEnabled = value;
                NotifyPropertyChanged(nameof(IsBulkNewPriceEnabled));
            }
        }


        private bool _IsBulkScipCodeIDEnabled;
        public bool IsBulkScipCodeIDEnabled
        {
            get { return _IsBulkScipCodeIDEnabled; }
            set
            {
                _IsBulkScipCodeIDEnabled = value;
                NotifyPropertyChanged(nameof(IsBulkScipCodeIDEnabled));
            }
        }


        private bool _IsBulkPriceChkd;
        public bool IsBulkPriceChkd
        {
            get { return _IsBulkPriceChkd; }
            set
            {
                _IsBulkPriceChkd = value;
                NotifyPropertyChanged(nameof(IsBulkPriceChkd));
                CheckPropertiesonClickOfCheckbox();
            }
        }


        private bool _IsFOMktProtEnabled;
        public bool IsFOMktProtEnabled
        {
            get { return _IsFOMktProtEnabled; }
            set { _IsFOMktProtEnabled = value; NotifyPropertyChanged(nameof(IsFOMktProtEnabled)); }
        }

        private string _FOMktProt = "1.0";
        public string FOMktProt
        {
            get { return _FOMktProt; }
            set { _FOMktProt = value; NotifyPropertyChanged(nameof(FOMktProt)); }
        }

        private string _BatchOrderTitleCount;
        public string BatchOrderTitleCount
        {
            get { return _BatchOrderTitleCount; }
            set { _BatchOrderTitleCount = value; NotifyPropertyChanged(nameof(BatchOrderTitleCount)); }
        }

        private string _txtReply;

        public string txtReply
        {
            get { return _txtReply; }
            set { _txtReply = value; NotifyPropertyChanged(nameof(txtReply)); }
        }
        private string _FOEntryString;//=string.Empty;

        public string FOEntryString
        {
            get { return _FOEntryString; }
            set
            {
                _FOEntryString = value;
                NotifyPropertyChanged(nameof(FOEntryString));
            }
        }

        private bool _IsFECPCodeEnable;

        public bool IsFECPCodeEnable
        {
            get { return _IsFECPCodeEnable; }
            set
            {
                _IsFECPCodeEnable = value;
                NotifyPropertyChanged(nameof(IsFECPCodeEnable));
            }
        }


        private bool _IsModCPCodeEnabled;
        public bool IsModCPCodeEnabled
        {
            get { return _IsModCPCodeEnabled; }
            set
            {
                _IsModCPCodeEnabled = value;
                NotifyPropertyChanged(nameof(IsModCPCodeEnabled));
            }
        }

        private bool _IsTrgRateEnabled;
        public bool IsTrgRateEnabled
        {
            get { return _IsTrgRateEnabled; }
            set
            {
                _IsTrgRateEnabled = value;
                NotifyPropertyChanged(nameof(IsTrgRateEnabled));
            }
        }

        private bool _IsMktEnabled;
        public bool IsMktEnabled
        {
            get { return _IsMktEnabled; }
            set
            {
                _IsMktEnabled = value;
                NotifyPropertyChanged(nameof(IsMktEnabled));
            }
        }

        private string _ModMktProt;
        public string ModMktProt
        {
            get { return _ModMktProt; }
            set
            {
                _ModMktProt = value;
                NotifyPropertyChanged(nameof(ModMktProt));
            }
        }

        private string _TotBuyQty;
        public string TotBuyQty
        {
            get { return _TotBuyQty; }
            set
            {
                _TotBuyQty = value;
                NotifyPropertyChanged(nameof(TotBuyQty));
            }
        }

        private string _TotSellQty;
        public string TotSellQty
        {
            get { return _TotSellQty; }
            set
            {
                _TotSellQty = value;
                NotifyPropertyChanged(nameof(TotSellQty));
            }
        }

        private string _ModBSFlag;
        public string ModBSFlag
        {
            get { return _ModBSFlag; }
            set
            {
                _ModBSFlag = value;
                NotifyPropertyChanged(nameof(ModBSFlag));
            }
        }

        private string _ModTotQty;
        public string ModTotQty
        {
            get { return _ModTotQty; }
            set
            {
                _ModTotQty = value;
                NotifyPropertyChanged(nameof(ModTotQty));
            }
        }

        private string _ModRevealQty;
        public string ModRevealQty
        {
            get { return _ModRevealQty; }
            set
            {
                _ModRevealQty = value;
                NotifyPropertyChanged(nameof(ModRevealQty));
            }
        }

        private string _ModScripID;
        public string ModScripID
        {
            get { return _ModScripID; }
            set
            {
                _ModScripID = value;
                NotifyPropertyChanged(nameof(ModScripID));
            }
        }

        private string _ModTrgRate;
        public string ModTrgRate
        {
            get { return _ModTrgRate; }
            set
            {
                _ModTrgRate = value;
                NotifyPropertyChanged(nameof(ModTrgRate));
            }
        }


        private string _ModRate;
        public string ModRate
        {
            get { return _ModRate; }
            set
            {
                _ModRate = value;
                NotifyPropertyChanged(nameof(ModRate));
            }
        }

        private string _ModClientID;
        public string ModClientID
        {
            get { return _ModClientID; }
            set
            {
                _ModClientID = value;
                NotifyPropertyChanged(nameof(ModClientID));
            }
        }

        private string _ModClientTypeSelected;
        public string ModClientTypeSelected
        {
            get { return _ModClientTypeSelected; }
            set
            {
                _ModClientTypeSelected = value;
                NotifyPropertyChanged(nameof(ModClientTypeSelected));
                OnChangeOfModClientType();
            }
        }

        private string _ViewReply;
        public string ViewReply
        {
            get { return _ViewReply; }
            set
            {
                _ViewReply = value;
                NotifyPropertyChanged(nameof(ViewReply));
            }
        }

        private string _ModRetainSelected;
        public string ModRetainSelected
        {
            get { return _ModRetainSelected; }
            set
            {
                _ModRetainSelected = value;
                NotifyPropertyChanged(nameof(ModRetainSelected));
            }
        }

        private string _ModOrdTypeSelected;
        public string ModOrdTypeSelected
        {
            get { return _ModOrdTypeSelected; }
            set
            {
                _ModOrdTypeSelected = value;
                NotifyPropertyChanged(nameof(ModOrdTypeSelected));
            }
        }

        private string _FEClientSelected;
        public string FEClientSelected
        {
            get { return _FEClientSelected; }
            set
            {
                _FEClientSelected = value;
                NotifyPropertyChanged(nameof(FEClientSelected));
                OnChangeOfFastEntryRetain();
            }
        }

        private string _FERetainSelected;
        public string FERetainSelected
        {
            get { return _FERetainSelected; }
            set
            {
                _FERetainSelected = value;
                NotifyPropertyChanged(nameof(FERetainSelected));
            }
        }

        private string _FEOrderTypeSelected;
        public string FEOrderTypeSelected
        {
            get { return _FEOrderTypeSelected; }
            set
            {
                _FEOrderTypeSelected = value;
                NotifyPropertyChanged(nameof(FEOrderTypeSelected));
                OnChangeOfFEOrderType();
            }
        }

        private bool _IsModClientIDEnable;
        public bool IsModClientIDEnable
        {
            get { return _IsModClientIDEnable; }
            set
            {
                _IsModClientIDEnable = value;
                NotifyPropertyChanged(nameof(IsModClientIDEnable));
            }
        }

        private string _IsFETrgRateVisible;
        public string IsFETrgRateVisible
        {
            get { return _IsFETrgRateVisible; }
            set
            {
                _IsFETrgRateVisible = value;
                NotifyPropertyChanged(nameof(IsFETrgRateVisible));
            }
        }

        private string _btnSelectAllContent = "Select All";

        public string btnSelectAllContent
        {
            get { return _btnSelectAllContent; }
            set { _btnSelectAllContent = value; NotifyPropertyChanged(nameof(btnSelectAllContent)); }
        }


        private List<string> _OrdTypeLst;
        public List<string> OrdTypeLst
        {
            get { return _OrdTypeLst; }
            set
            {
                _OrdTypeLst = value;
                NotifyPropertyChanged("OrdTypeLst");
            }
        }

        private List<string> _RetainLst;
        public List<string> RetainLst
        {
            get { return _RetainLst; }
            set
            {
                _RetainLst = value;
                NotifyPropertyChanged("RetainLst");
            }
        }

        private List<string> _ClientTypeLst;
        public List<string> ClientTypeLst
        {
            get { return _ClientTypeLst; }
            set
            {
                _ClientTypeLst = value;
                NotifyPropertyChanged("ClientTypeLst");
            }
        }

        private List<OrderModel> _ViewSuccessCollection;
        public List<OrderModel> ViewSuccessCollection
        {
            get { return _ViewSuccessCollection; }
            set
            {
                _ViewSuccessCollection = value;
                NotifyPropertyChanged("ViewSuccessCollection");
            }
        }

        private ObservableCollection<OrderModel> _BatchOrderCollection;
        public ObservableCollection<OrderModel> BatchOrderCollection
        {
            get { return _BatchOrderCollection; }
            set
            {
                _BatchOrderCollection = value;
                NotifyPropertyChanged("BatchOrderCollection");
            }
        }

        private OrderModel _BatchGridSelectedItem;
        public OrderModel BatchGridSelectedItem
        {
            get { return _BatchGridSelectedItem; }
            set
            {
                _BatchGridSelectedItem = value;
                NotifyPropertyChanged("BatchGridSelectedItem");
            }
        }

        private ObservableCollection<OrderModel> _CopyOfBatchOrderCollection;
        public ObservableCollection<OrderModel> CopyOfBatchOrderCollection
        {
            get { return _CopyOfBatchOrderCollection; }
            set
            {
                _CopyOfBatchOrderCollection = value;
                NotifyPropertyChanged("CopyOfBatchOrderCollection");
            }
        }

        private List<Model.Order.OrderModel> _SelectedValue = new List<Model.Order.OrderModel>();

        public List<Model.Order.OrderModel> SelectedValue
        {
            get { return _SelectedValue; }
            set
            {
                _SelectedValue = value;
                NotifyPropertyChanged(nameof(SelectedValue));
            }
        }

        #endregion

        public BatchOrderVM()
        {
            winName = Enumerations.WindowName.Batch_Order.ToString().Replace("_", " ");
            batch = System.Windows.Application.Current.Windows.OfType<View.Order.BatchOrder>().FirstOrDefault();
            IsModCPCodeEnabled = false;
            IsTrgRateEnabled = false;
            IsMktEnabled = false;
            BatchOrderCollection = new ObservableCollection<OrderModel>();
            ViewSuccessCollection = new List<OrderModel>();
            PopulatingOrderType();
            PopulatingClientType();
            PopulatingRetainList();
            FOMktProt = "1.0";
            BatchOrderTitleCount = "Batch Submission - Orders (Total Count: 0)";
            IsFOMktProtEnabled = false;
            IsModClientIDEnable = true;
            IsBulkPriceChkd = false;
            IsBulkScipCodeIDEnabled = false;
            IsBulkNewPriceEnabled = false;
            IsBulkNewTrgPriceEnabled = false;
            IsBulkChangePriceEnabled = false;
            IsAutoSubmitChecked = false;
            IsTxtHHEnabled = false;
            IsTxtMMEnabled = false;
            IsTxtSSEnabled = false;
            IsSetTimeEnabled = false;
            IsFETrgRateVisible = "Hidden";
            FOMktProt = UtilityOrderDetails.GETInstance.MktProtection == null ? "1.0" : UtilityOrderDetails.GETInstance.MktProtection;
            ModMktProt = UtilityOrderDetails.GETInstance.MktProtection == null ? "1.0" : UtilityOrderDetails.GETInstance.MktProtection;
            OrderProfilingVM.OnChangeOfMarketProtection += delegate (string MarketProt) { FOMktProt = MarketProt; ModMktProt = MarketProt; };
            OrderProcessor.OnOrderResponseReceived += ReadDataFromOrderMemory;
            if (UtilityLoginDetails.GETInstance.IsLoggedIN)
                IsSubmitEnabled = true;
            else
                IsSubmitEnabled = false;
        }

        private void CheckPropertiesOnCheckOfAutoSubmitCheckBox()
        {
            if (IsAutoSubmitChecked)
            {
                IsTxtHHEnabled = true;
                IsTxtMMEnabled = true;
                IsTxtSSEnabled = true;
                IsSetTimeEnabled = true;
            }
            else
            {
                IsTxtHHEnabled = false;
                IsTxtMMEnabled = false;
                IsTxtSSEnabled = false;
                IsSetTimeEnabled = false;
            }
        }

        private void CheckPropertiesonClickOfCheckbox()
        {
            if (IsBulkPriceChkd)
            {
                IsBulkScipCodeIDEnabled = true;
                IsBulkNewPriceEnabled = true;
                IsBulkNewTrgPriceEnabled = true;
                IsBulkChangePriceEnabled = true;
            }
            else
            {
                IsBulkScipCodeIDEnabled = false;
                IsBulkNewPriceEnabled = false;
                IsBulkNewTrgPriceEnabled = false;
                IsBulkChangePriceEnabled = false;
            }
        }

        private void OnChangeOfFastEntryRetain()
        {
            if (FEClientSelected == Enumerations.Order.ClientTypes.CLIENT.ToString() || FEClientSelected == Enumerations.Order.ClientTypes.OWN.ToString())
            {
                IsFECPCodeEnable = false;
            }
            else
            {
                IsFECPCodeEnable = true;
            }
        }

        private void OnChangeOfModClientType()
        {
            if (BatchGridSelectedItem != null)
            {
                if (ModClientTypeSelected == Enumerations.Order.ClientTypes.OWN.ToString())
                {
                    ModClientID = "OWN";
                    IsModClientIDEnable = false;
                }
                else
                {
                    IsModClientIDEnable = true;
                    if (ModClientID == "OWN")
                        ModClientID = string.Empty;
                }

                if ((BatchGridSelectedItem.Segment == "Currency" || BatchGridSelectedItem.Segment == "Derivative") && (ModClientTypeSelected == Enumerations.Order.ClientTypes.INST.ToString() || ModClientTypeSelected == Enumerations.Order.ClientTypes.SPLCLI.ToString()))
                {
                    IsModCPCodeEnabled = true;
                }
                else
                    IsModCPCodeEnabled = false;
            }
        }


        private void OnChangeOfFEOrderType()
        {
            if (FEOrderTypeSelected == Enumerations.Order.OrderTypesBatch.MKT_G.ToString().Replace("_", "-") || FEOrderTypeSelected == Enumerations.Order.OrderTypesBatch.SLMKT_P.ToString().Replace("_", "-") || FEOrderTypeSelected == Enumerations.Order.OrderTypesBatch.OCO_L.ToString().Replace("_", "-"))
                IsFOMktProtEnabled = true;
            else /*if (FEOrderTypeSelected == Enumerations.Order.OrderTypesBatch.LIMIT_L.ToString().Replace("_", "-")||)*/
                IsFOMktProtEnabled = false;

            if (FEOrderTypeSelected == Enumerations.Order.OrderTypesBatch.SLMKT_P.ToString().Replace("_", "-") || FEOrderTypeSelected == Enumerations.Order.OrderTypesBatch.SL_P.ToString().Replace("_", "-") || FEOrderTypeSelected == Enumerations.Order.OrderTypesBatch.OCO_L.ToString().Replace("_", "-"))
                IsFETrgRateVisible = "Visible";
            else
                IsFETrgRateVisible = "Hidden";
        }

        private void OnEntryOfFastOrderEntryFields()
        {
            ViewReply = string.Empty;
            long z = 0, count = 0;
            int segmentFlag = 0, decimalPoint = 0;
            string Segment_Name = string.Empty;
            string part = string.Empty;
            string[] tokens = FOEntryString.Split(null);
            int tokenLength = tokens.Length;
            if ((tokenLength == 6 && (FEOrderTypeSelected == Enumerations.Order.OrderTypesBatch.LIMIT_L.ToString().Replace("_", "-") || FEOrderTypeSelected == Enumerations.Order.OrderTypesBatch.ODDL_O.ToString().Replace("_", "-")
                || FEOrderTypeSelected == Enumerations.Order.OrderTypesBatch.MKT_G.ToString().Replace("_", "-") || FEOrderTypeSelected == Enumerations.Order.OrderTypesBatch.BDEAL_K.ToString().Replace("_", "-"))) ||
                tokenLength == 7 && (FEOrderTypeSelected == Enumerations.Order.OrderTypesBatch.OCO_L.ToString().Replace("_", "-") || FEOrderTypeSelected == Enumerations.Order.OrderTypesBatch.SL_P.ToString().Replace("_", "-")
                || FEOrderTypeSelected == Enumerations.Order.OrderTypesBatch.SLMKT_P.ToString().Replace("_", "-")))
            {
                OrderModel omodel = new OrderModel();

                #region Buy/Sell Flag
                if (tokens[0] == "+")
                {
                    omodel.BuySellIndicator = "B";
                }
                else if (tokens[0] == "-")
                {
                    omodel.BuySellIndicator = "S";
                }

                if (!(tokens[0] == "B" || tokens[0] == "S" || tokens[0] == "+" || tokens[0] == "-" || tokens[0].Length > 1))
                {
                    ViewReply = "Invalid Bought/Sold Flag";
                    return;
                }
                else
                    omodel.BuySellIndicator = tokens[0];
                #endregion

                #region Total Quantity
                bool result = CommonFunctions.OnlyNumeric(tokens[1]);
                if (result)
                {
                    if (Convert.ToInt32(tokens[1]) < 0 || Convert.ToInt32(tokens[1]) == 0)
                    {
                        ViewReply = "Invalid Total Quantity - To Small";
                        return;
                    }
                    else if (Convert.ToInt32(tokens[1]) > OrderMaxLimit)
                    {
                        ViewReply = "Invalid Total Quantity - To Large";
                        return;
                    }
                    else
                    {
                        omodel.OriginalQty = Convert.ToInt32(tokens[1]);
                    }

                }
                else
                {
                    ViewReply = "Invalid Total Quantity";
                    return;
                }
                #endregion

                #region Reaveal Quantity
                // double num = 0.01 * Convert.ToInt32(tokens[1]);
                bool res = CommonFunctions.OnlyNumeric(tokens[2]);
                if (res)
                {
                    if (Convert.ToInt32(tokens[2]) < 0 || Convert.ToInt32(tokens[2]) == 0)
                    {
                        ViewReply = "Invalid Total Quantity - To Small";
                        return;
                    }
                    else if (Convert.ToInt32(tokens[2]) > OrderMaxLimit)
                    {
                        ViewReply = "Invalid Total Quantity - To Large";
                        return;
                    }
                    else if (Convert.ToInt32(tokens[2]) > Convert.ToInt32(tokens[1]))
                    {
                        ViewReply = "Reaveal Quantity should not be greater than Total Quantity";
                        return;
                    }
                    //else if (Convert.ToDouble(tokens[1]) > num)
                    //{
                    //    //TO be done by praful
                    //}
                    else
                    {
                        omodel.RevealQty = Convert.ToInt32(tokens[2]);
                    }

                }
                else
                {
                    ViewReply = "Invalid Total Quantity";
                    return;
                }

                #endregion

                #region Scrip Code / Scrip ID
                bool convres = long.TryParse(tokens[3], out z);
                if (convres)
                {
                    omodel.Symbol = CommonFunctions.GetScripId(Convert.ToInt64(tokens[3].Trim()), "BSE");
                    omodel.ScripCode = Convert.ToInt64(tokens[3].Trim());
                    Segment_Name = CommonFunctions.GetSegmentID(omodel.ScripCode);
                    omodel.ScripCodeID = CommonFunctions.GetScripId(Convert.ToInt64(tokens[3].Trim()), "BSE");
                }
                else
                {
                    omodel.ScripCode = CommonFunctions.GetScripCodeFromScripID(tokens[3].Trim());
                    Segment_Name = CommonFunctions.GetSegmentID(omodel.ScripCode);
                    omodel.Symbol = tokens[3].Trim();
                    omodel.ScripCodeID = tokens[3].Trim();
                }

                decimalPoint = CommonFunctions.GetDecimal((int)omodel.ScripCode, "BSE", Segment_Name);

                if (Segment_Name == "Derivative")
                {
                    segmentFlag = MasterSharedMemory.objMasterDerivativeDictBaseBSE.Where(x => x.Value.ContractTokenNum == omodel.ScripCode).Select(x => x.Value.StrategyID).FirstOrDefault();
                }
                else if (Segment_Name == "Currency")
                {
                    segmentFlag = MasterSharedMemory.objMasterDerivativeDictBaseBSE.Where(x => x.Value.ContractTokenNum == omodel.ScripCode).Select(x => x.Value.StrategyID).FirstOrDefault();
                }
                #endregion

                #region Rate
                ValidateMarketProtection(FOMktProt);

                //string input = "12356";
                //bool valid = Regex.IsMatch(input, @"^[-+]?\d+\.?\d*$")); // returns true;

                //string rate = checkRate(tokens[4]);

                //if (!(System.Text.RegularExpressions.Regex.IsMatch(rate, @"[^0-9]|.|-")))
                //{
                //    ViewReply = "Invalid Rate";
                //    return;
                //}

                count = tokens[4].Count(c => c == '.');
                if (count > 1)
                {
                    ViewReply = "Invalid Rate";
                }

                if (count == 1)
                {
                    part = tokens[4].Substring(tokens[4].IndexOf('.'), (tokens[4].Length - tokens[4].IndexOf('.')));

                    if (part.Length > 3) // For Eqt, Deri, Debt (other than Fc,GC)
                        ViewReply = "";
                    else if (part.Length > 5) // For Curr, Debt (FC,GC)
                        ViewReply = "";
                }

                count = tokens[4].Count(c => c == '-');
                if (count > 1)
                {
                    ViewReply = "Invalid Rate";
                }

                if (FEOrderTypeSelected == Enumerations.Order.OrderTypesBatch.LIMIT_L.ToString().Replace("_", "-") || FEOrderTypeSelected == Enumerations.Order.OrderTypesBatch.BDEAL_K.ToString().Replace("_", "-") ||
                    FEOrderTypeSelected == Enumerations.Order.OrderTypesBatch.SL_P.ToString().Replace("_", "-") || FEOrderTypeSelected == Enumerations.Order.OrderTypesBatch.ODDL_O.ToString().Replace("_", "-"))
                {
                    if (segmentFlag != -1)
                    {
                        if (Convert.ToDouble(tokens[4]) < 0.0)
                        {
                            ViewReply = "Invalid Rate - too small";
                        }

                        if (Convert.ToDouble(tokens[4]) > MAXRATELIMIT)
                        {
                            ViewReply = "Invalid Rate - too large";
                        }

                        if (Convert.ToDouble(tokens[4]) == 0.0)
                        {
                            ViewReply = "Invalid Rate - Zero allowed only for Market Orders";
                        }
                    }
                    else
                    {
                        if (Math.Pow(Convert.ToDouble(tokens[4]), decimalPoint) > MAXRATELIMIT)
                        {
                            ViewReply = "Invalid Rate - too large";
                        }

                        if (Math.Pow(Convert.ToDouble(tokens[4]), decimalPoint) < MAXNEGATIVERATELIMIT)
                        {
                            ViewReply = "Invalid Rate - too small";
                        }
                    }

                }
                count = tokens[4].Count(c => c == '.');
                if (count == 1)
                    omodel.Price = Convert.ToInt64(Convert.ToDouble(Convert.ToDecimal(tokens[4])) * Math.Pow(10, decimalPoint));
                else
                    omodel.Price = Convert.ToInt64(Convert.ToDouble(Convert.ToDecimal(tokens[4])) * Math.Pow(10, decimalPoint));
                omodel.PriceS = tokens[4];
                #endregion

                #region S/Client
                omodel.ClientId = tokens[5].Trim();
                #endregion

                #region Trigger Rate
                if (tokenLength == 7 && (FEOrderTypeSelected == Enumerations.Order.OrderTypesBatch.OCO_L.ToString().Replace("_", "-") || FEOrderTypeSelected == Enumerations.Order.OrderTypesBatch.SL_P.ToString().Replace("_", "-")
                  || FEOrderTypeSelected == Enumerations.Order.OrderTypesBatch.SLMKT_P.ToString().Replace("_", "-")))
                {
                    omodel.TriggerPriceS = tokens[6];
                    if (ValidateTriggerPrice(tokens[6], omodel.BuySellIndicator, tokens[4]))
                    {
                        count = tokens[6].Count(c => c == '.');
                        if (count == 1)
                            omodel.TriggerPrice = Convert.ToInt64(Convert.ToDouble(Convert.ToDecimal(tokens[6])) * Math.Pow(10, decimalPoint));
                        else
                            omodel.TriggerPrice = Convert.ToInt64(Convert.ToDouble(Convert.ToDecimal(tokens[6])) * Math.Pow(10, decimalPoint));
                    }
                    else
                    {
                        return;
                    }
                    // omodel.TriggerPrice = Convert.ToInt64(tokens[6]);
                }
                else
                {
                    omodel.TriggerPrice = 0;
                    //return;
                }
                #endregion

                //Need to check all the fields after validations
                #region additional fields
                omodel.ClientType = FEClientSelected;
                omodel.Segment = Segment_Name;
                omodel.SenderLocationID = UtilityLoginDetails.GETInstance.SenderLocationId;
                omodel.OrderRemarks = "NA";
                //  omodel.Price = BatchGridSelectedItem.Price; Price need to be filled in rate

                var protectionPercent = Convert.ToInt16(Convert.ToDecimal(FOMktProt) * 100);
                omodel.ProtectionPercentage = Convert.ToString(protectionPercent);
                // omodel.ProtectionPercentage = FOMktProt;
                omodel.PendingQuantity = omodel.OriginalQty;
                omodel.ScreenId = (int)Enumerations.WindowName.Batch_Order;
                //omodel.CurrentScreenId = BatchGridSelectedItem.CurrentScreenId;
                omodel.MarketLot = Convert.ToInt32(CommonFunctions.GetMarketLot(omodel.ScripCode));//Convert.ToInt16(Math.Floor(Convert.ToDouble(FOMktProt)));
                omodel.TickSize = CommonFunctions.GetTickSize(omodel.ScripCode);
                omodel.Group = CommonFunctions.GetGroupName(omodel.ScripCode, "BSE", Segment_Name);
                omodel.ExecInst = Enumerations.Order.ExecInst.PersistentOrder.ToString();
                omodel.ParticipantCode = BatchGridSelectedItem?.ParticipantCode;
                omodel.SegmentFlag = CommonFunctions.SegmentFlag(Segment_Name);
                omodel.FreeText3 = "abc";
                omodel.Filler_c = "abc";
                omodel.PartitionID = Convert.ToUInt16(CommonFunctions.GetPartitionId(omodel.ScripCode, "BSE", omodel.Segment));//BatchOrder.PartitionID;
                omodel.MarketSegmentID = CommonFunctions.GetProductId(omodel.ScripCode, "BSE", omodel.Segment);//BatchOrder.MarketSegmentID;
                omodel.ScripName = CommonFunctions.GetScripName(omodel.ScripCode, "BSE", Segment_Name);
                omodel.MessageTag = GetOrderMessageTag();
                omodel.ParticipantCode = "";
                omodel.OrderFomLoadButton = false;
                int Batch_decimalPoint = CommonFunctions.GetDecimal(Convert.ToInt32(omodel.ScripCode), "BSE", Segment_Name);
                omodel.BatchKey = string.Format("{0}_{1}", omodel.ScripCode, omodel.MessageTag); //Need to confirm

                if (Batch_decimalPoint == 2)
                {
                    omodel.PriceS = String.Format("{0:0.00}", Convert.ToDecimal(tokens[4])).ToString();
                }
                else
                {
                    omodel.PriceS = String.Format("{0:0.0000}", Convert.ToDecimal(tokens[4])).ToString();
                }

                if (tokenLength == 7 && (FEOrderTypeSelected == Enumerations.Order.OrderTypesBatch.OCO_L.ToString().Replace("_", "-") || FEOrderTypeSelected == Enumerations.Order.OrderTypesBatch.SL_P.ToString().Replace("_", "-")
                  || FEOrderTypeSelected == Enumerations.Order.OrderTypesBatch.SLMKT_P.ToString().Replace("_", "-")))
                {
                    if (Batch_decimalPoint == 2)
                    {
                        omodel.TriggerPriceS = String.Format("{0:0.00}", Convert.ToDecimal(tokens[6])).ToString();
                    }
                    else
                    {
                        omodel.TriggerPriceS = String.Format("{0:0.0000}", Convert.ToDecimal(tokens[6])).ToString();
                    }
                }


                if (FERetainSelected == Enumerations.Order.RetType.EOD.ToString())
                    omodel.OrderRetentionStatus = "EOTODY";
                else if (FERetainSelected == Enumerations.Order.RetType.EOS.ToString())
                    omodel.OrderRetentionStatus = "EOSESS";
                else if (FERetainSelected == Enumerations.Order.RetType.IOC.ToString())
                    omodel.OrderRetentionStatus = "IOC";

                if (FEOrderTypeSelected == Enumerations.Order.OrderTypesBatch.LIMIT_L.ToString().Replace("_", "-"))
                    omodel.OrderType = "L";
                else if (FEOrderTypeSelected == Enumerations.Order.OrderTypesBatch.MKT_G.ToString().Replace("_", "-"))
                    omodel.OrderType = "G";
                else if (FEOrderTypeSelected == Enumerations.Order.OrderTypesBatch.SLMKT_P.ToString().Replace("_", "-"))
                    omodel.OrderType = "P";
                else if (FEOrderTypeSelected == Enumerations.Order.OrderTypesBatch.SL_P.ToString().Replace("_", "-"))
                    omodel.OrderType = "P";
                else if (FEOrderTypeSelected == Enumerations.Order.OrderTypesBatch.BDEAL_K.ToString().Replace("_", "-"))
                    omodel.OrderType = "K";
                else if (FEOrderTypeSelected == Enumerations.Order.OrderTypesBatch.ODDL_O.ToString().Replace("_", "-"))
                    omodel.OrderType = "O";
                else if (FEOrderTypeSelected == Enumerations.Order.OrderTypesBatch.OCO_L.ToString().Replace("_", "-"))
                    omodel.OrderType = "L";

                if (omodel.OrderType == "L")
                {
                    if (omodel.TriggerPrice == 0)
                    {
                        omodel.IsOCOOrder = false;
                    }
                    else if (omodel.TriggerPrice > 0)
                    {
                        omodel.IsOCOOrder = true;
                    }
                }

                if (omodel.OrderType == "L")
                {
                    //if (omodel.IsOCOOrder)//OCO order
                    //{
                    //    //oOrderModel.OrderType = Enumerations.Order.OrderTypes.OCO.ToString();
                    //    System.Windows.MessageBox.Show("OCO order placement not allowed");
                    //    return;
                    //}
                }
                else if (omodel.OrderType == "P")
                {
                    if (omodel.TriggerPrice == 0)
                    {
                        ViewReply = "Enter Trigger Price for STOPLOSS";
                        return;
                    }
                    else
                    {
                        if (omodel.Price == 0)
                            omodel.OrderType = "P"; //Enumerations.Order.OrderTypes.STOPLOSSMKT.ToString();
                        else
                            omodel.OrderType = "P";// Enumerations.Order.OrderTypes.STOPLOSS.ToString();
                    }
                }
                #endregion
                BatchOrderCollection.Add(omodel);
                TotBuyQty = BatchOrderCollection.Where(y => y.BuySellIndicator == "B").Sum<OrderModel>(x => x.OriginalQty).ToString();
                TotSellQty = BatchOrderCollection.Where(y => y.BuySellIndicator == "S").Sum<OrderModel>(x => x.OriginalQty).ToString();
                BatchOrderTitleCount = "Batch Submission - Orders (Total Count : " + BatchOrderCollection.Count + ")";
                FOEntryString = string.Empty;
            }
            else
            {
                if (((tokenLength > 6) && (FEOrderTypeSelected == Enumerations.Order.OrderTypesBatch.LIMIT_L.ToString().Replace("_", "-") || FEOrderTypeSelected == Enumerations.Order.OrderTypesBatch.ODDL_O.ToString().Replace("_", "-")
                || FEOrderTypeSelected == Enumerations.Order.OrderTypesBatch.MKT_G.ToString().Replace("_", "-") || FEOrderTypeSelected == Enumerations.Order.OrderTypesBatch.BDEAL_K.ToString().Replace("_", "-"))))
                    ViewReply = "Invalid number of Parameters";
                else if (tokenLength > 7 && (FEOrderTypeSelected == Enumerations.Order.OrderTypesBatch.OCO_L.ToString().Replace("_", "-") || FEOrderTypeSelected == Enumerations.Order.OrderTypesBatch.SL_P.ToString().Replace("_", "-")
                || FEOrderTypeSelected == Enumerations.Order.OrderTypesBatch.SLMKT_P.ToString().Replace("_", "-")))
                    ViewReply = "Invalid number of Parameters";
                else
                    ViewReply = "Enter All the fields";
            }
        }

        private bool ValidateTriggerPrice(string TriggerValue, string flag, string LimitValue)
        {
            string orderType = string.Empty;
            if (FEOrderTypeSelected == Enumerations.Order.OrderTypesBatch.SL_P.ToString().Replace("_", "-") || orderType == "SL-P")
            {

                if ((string.IsNullOrEmpty(LimitValue)) && (Convert.ToDecimal(LimitValue) <= 0))
                {
                    ViewReply = "INVALID LIMIT RATE";
                    return false;
                }

                if (flag == Enumerations.Order.BuySellFlag.B.ToString())
                {
                    if (Convert.ToDecimal(LimitValue) < Convert.ToDecimal(TriggerValue))
                    {
                        ViewReply = "LIMIT PRICE MUST BE GREATER THAN STOP PRICE";
                        return false;
                    }
                }
                else
                {
                    if (Convert.ToDecimal(LimitValue) > Convert.ToDecimal(TriggerValue))
                    {
                        ViewReply = "LIMIT PRICE MUST BE SMALLER THAN STOP PRICE";
                        return false;
                    }
                }
            }
            return true;
        }

        private bool ValidateTriggerPriceForOCOAndStopLoss(string TriggerValue, string flag, string LimitValue, string orderType)
        {
            //string orderType = string.Empty;
            if (FEOrderTypeSelected == Enumerations.Order.OrderTypesBatch.SL_P.ToString().Replace("_", "-") || orderType == "P")
            {

                if ((string.IsNullOrEmpty(LimitValue)) && (Convert.ToDecimal(LimitValue) <= 0))
                {
                    ViewReply = "INVALID LIMIT RATE";
                    return false;
                }

                if (flag == Enumerations.Order.BuySellFlag.B.ToString())
                {
                    if (Convert.ToDecimal(LimitValue) < Convert.ToDecimal(TriggerValue))
                    {
                        ViewReply = "LIMIT PRICE MUST BE GREATER THAN STOP PRICE";
                        return false;
                    }
                }
                else
                {
                    if (Convert.ToDecimal(LimitValue) > Convert.ToDecimal(TriggerValue))
                    {
                        ViewReply = "LIMIT PRICE MUST BE SMALLER THAN STOP PRICE";
                        return false;
                    }
                }
            }
            else if (orderType == "L" && Convert.ToUInt32(TriggerValue) > 0)
            {
                if ((string.IsNullOrEmpty(LimitValue)) && (Convert.ToDecimal(LimitValue) <= 0))
                {
                    ViewReply = "INVALID LIMIT RATE";
                    return false;
                }

                if (flag == Enumerations.Order.BuySellFlag.B.ToString())
                {
                    if (Convert.ToDecimal(LimitValue) > Convert.ToDecimal(TriggerValue))
                    {
                        ViewReply = "LIMIT PRICE MUST BE SMALLER THAN STOP PRICE";
                        return false;
                    }
                }
                else
                {
                    if (Convert.ToDecimal(LimitValue) < Convert.ToDecimal(TriggerValue))
                    {
                        ViewReply = "LIMIT PRICE MUST BE GREATER THAN STOP PRICE";
                        return false;
                    }
                }
            }
            return true;
        }

        private string checkRate(string v)
        {
            if (!(System.Text.RegularExpressions.Regex.IsMatch(v, @"[^0-9.]")))
            {
                v += ".00";
            }
            return v;
        }

        private void ValidateMarketProtection(string value)
        {
            int count = 0;
            if (string.IsNullOrEmpty(value))
            {
                ViewReply = "MarketProtection : Cannot be blank";
            }

            count = value.Count(c => c == '.');
            if (count > 1)
            {
                ViewReply = "MarketProtection : Invalid Market Protection";
            }

            count = value.ToString().Split('.').Count() > 1
                   ? value.ToString().Split('.').ToList().ElementAt(1).Length
                   : 0;
            if (count > 2)
            {
                ViewReply = "MarketProtection more than 2 decimal places not allowed";
            }

            var protectionPercent = Convert.ToInt32(Convert.ToDecimal(value) * 100);
            if (protectionPercent > 9999)
            {
                ViewReply = "MarketProtection can't be greater than  99.99%";
            }
        }

        private void TotalMarketProtectionValidation(object e)
        {
            FOMktProt = Regex.Replace(FOMktProt, "[^0-9.]+", "");
        }

        private void UpdateDataGrid(object e)
        {
            int cnt = 0;
            if (BatchGridSelectedItem != null && SelectedValue.Count == 1)
            {
                ViewReply = SelectedValue.Count + " Order(s) Selected";
                ModBSFlag = BatchGridSelectedItem.BuySellIndicator;
                ModTotQty = BatchGridSelectedItem.OriginalQty.ToString();
                ModRevealQty = BatchGridSelectedItem.RevealQty.ToString();
                //ModScripID = BatchGridSelectedItem.ScripCode.ToString();
                ModScripID = BatchGridSelectedItem.ScripCodeID.ToString();
                ModRate = BatchGridSelectedItem.PriceS;
                ModTrgRate = BatchGridSelectedItem.TriggerPriceS;

                int Batch_decimalPoint = CommonFunctions.GetDecimal(Convert.ToInt32(BatchGridSelectedItem.ScripCode), "BSE", BatchGridSelectedItem.Segment);

                // ModClientID = BatchGridSelectedItem.ClientId;
                if (BatchGridSelectedItem.OrderType == "L" || BatchGridSelectedItem.OrderType == "O" || BatchGridSelectedItem.OrderType == "K")
                {
                    // Limit, BlockDeal, OddLot
                    if (BatchGridSelectedItem.TriggerPrice == 0)//Trigger Price need to be chnaged from 0 to deafault value of int (Sr Pooja) -2147483648
                    {
                        ModOrdTypeSelected = Enumerations.Order.OrderTypesBatch.LIMIT_L.ToString().Replace("_", "-");
                        IsMktEnabled = false;
                    }
                    else // OCO
                    {
                        ModOrdTypeSelected = Enumerations.Order.OrderTypesBatch.OCO_L.ToString().Replace("_", "-");
                        IsMktEnabled = true;
                        IsTrgRateEnabled = true;
                    }
                }
                else if (BatchGridSelectedItem.OrderType == "G")
                { // Market
                    ModOrdTypeSelected = Enumerations.Order.OrderTypesBatch.MKT_G.ToString().Replace("_", "-");
                    IsMktEnabled = true;
                    ModMktProt = UtilityOrderDetails.GETInstance.MktProtection == null ? "1.0" : UtilityOrderDetails.GETInstance.MktProtection;//"1.0";//Need to take from Order Settings
                    if (Batch_decimalPoint == 2)
                        ModRate = "0.00";
                    else
                        ModRate = "0.0000";
                }
                else if (BatchGridSelectedItem.OrderType == "P")
                {
                    // Stoploss, stoploss market
                    IsMktEnabled = true;
                    IsTrgRateEnabled = true;
                    ModMktProt = UtilityOrderDetails.GETInstance.MktProtection == null ? "1.0" : UtilityOrderDetails.GETInstance.MktProtection;//"1.0";//Need to take from Order Settings

                    if (BatchGridSelectedItem.Price == 0) // Market Stoploss
                    {
                        ModOrdTypeSelected = Enumerations.Order.OrderTypesBatch.SLMKT_P.ToString().Replace("_", "-");
                    }
                    else
                    {
                        ModOrdTypeSelected = Enumerations.Order.OrderTypesBatch.SL_P.ToString().Replace("_", "-");
                    }
                }

                if (BatchGridSelectedItem.ClientType.ToUpper() == Enumerations.Order.ClientTypes.CLIENT.ToString())
                    ModClientTypeSelected = Enumerations.Order.ClientTypes.CLIENT.ToString();
                else if (BatchGridSelectedItem.ClientType.ToUpper() == Enumerations.Order.ClientTypes.INST.ToString())
                    ModClientTypeSelected = Enumerations.Order.ClientTypes.INST.ToString();
                else if (BatchGridSelectedItem.ClientType.ToUpper() == Enumerations.Order.ClientTypes.OWN.ToString())
                    ModClientTypeSelected = Enumerations.Order.ClientTypes.OWN.ToString();
                else if (BatchGridSelectedItem.ClientType.ToUpper() == Enumerations.Order.ClientTypes.SPLCLI.ToString())
                    ModClientTypeSelected = Enumerations.Order.ClientTypes.SPLCLI.ToString();

                if (BatchGridSelectedItem.OrderRetentionStatus == "EOTODY")
                    ModRetainSelected = Enumerations.Order.RetType.EOD.ToString();
                else if (BatchGridSelectedItem.OrderRetentionStatus == "EOSESS")
                    ModRetainSelected = Enumerations.Order.RetType.EOS.ToString();
                else if (BatchGridSelectedItem.OrderRetentionStatus == "IOC")
                    ModRetainSelected = Enumerations.Order.RetType.IOC.ToString();

                ModClientID = BatchGridSelectedItem.ClientId;
                //  NotifyPropertyChanged("ModClientID");

                #region Bulk Price Change
                if (IsBulkPriceChkd)
                {
                    BulkScripCodeID = BatchGridSelectedItem.ScripCodeID;
                    BulkNewPrice = string.Empty;
                    BulkNewTrgPrice = string.Empty;
                }
                #endregion
            }
            else
            {
                ViewReply = SelectedValue.Count + " Order selected";
                ModBSFlag = string.Empty;
                ModTotQty = string.Empty;
                ModRevealQty = string.Empty;
                ModScripID = string.Empty;
                ModRate = string.Empty;
                ModClientID = string.Empty;
                ModTrgRate = string.Empty;
            }
        }

        private void PopulatingRetainList()
        {
            RetainLst = new List<string>();
            RetainLst.Add(Enumerations.Order.RetType.EOD.ToString());
            RetainLst.Add(Enumerations.Order.RetType.EOS.ToString());
            RetainLst.Add(Enumerations.Order.RetType.IOC.ToString());
            ModRetainSelected = RetainLst[0];
            FERetainSelected = RetainLst[0];
        }

        private void PopulatingClientType()
        {
            ClientTypeLst = new List<string>();
            ClientTypeLst.Add(Enumerations.Order.ClientTypes.CLIENT.ToString());
            ClientTypeLst.Add(Enumerations.Order.ClientTypes.INST.ToString());
            ClientTypeLst.Add(Enumerations.Order.ClientTypes.OWN.ToString());
            ClientTypeLst.Add(Enumerations.Order.ClientTypes.SPLCLI.ToString());
            ModClientTypeSelected = ClientTypeLst[0];
            FEClientSelected = ClientTypeLst[0];
        }

        private void PopulatingOrderType()
        {
            OrdTypeLst = new List<string>();
            OrdTypeLst.Add(Enumerations.Order.OrderTypesBatch.LIMIT_L.ToString().Replace("_", "-"));
            OrdTypeLst.Add(Enumerations.Order.OrderTypesBatch.MKT_G.ToString().Replace("_", "-"));
            OrdTypeLst.Add(Enumerations.Order.OrderTypesBatch.OCO_L.ToString().Replace("_", "-"));
            OrdTypeLst.Add(Enumerations.Order.OrderTypesBatch.SL_P.ToString().Replace("_", "-"));
            OrdTypeLst.Add(Enumerations.Order.OrderTypesBatch.SLMKT_P.ToString().Replace("_", "-"));
            OrdTypeLst.Add(Enumerations.Order.OrderTypesBatch.BDEAL_K.ToString().Replace("_", "-"));
            OrdTypeLst.Add(Enumerations.Order.OrderTypesBatch.ODDL_O.ToString().Replace("_", "-"));
            FEOrderTypeSelected = OrdTypeLst[0];
            ModOrdTypeSelected = OrdTypeLst[0];
        }

        private void OnClickOfLoadButton()
        {
            //BatchOrderCollection = new ObservableCollection<OrderModel>();
            ViewSuccessCollection = new List<OrderModel>();
            string fileName;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            //string StartupPath = CsvFilesPath.ToString();
            //openFileDialog1.InitialDirectory = Path.Combine(Path.GetDirectoryName(StartupPath));

            openFileDialog1.InitialDirectory = previousPath;

            openFileDialog1.Title = "Browse CSV Files";

            openFileDialog1.DefaultExt = "csv";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.CheckFileExists = true;
            // openFileDialog1.Filter = "CSV Files(*.CSV) | *.CSV | DOT(*.txt) | *.txt ";//"CSV files (*.csv)|*.csv";
            openFileDialog1.Filter = "CSV files (*.csv)|*.csv";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                previousPath = Path.GetDirectoryName(openFileDialog1.FileName);
                BatchOrderCollection = new ObservableCollection<OrderModel>();
                ViewSuccessCollection = new List<OrderModel>();
                fileName = openFileDialog1.FileName;
                string[] strScrips = File.ReadAllLines(fileName);

                ViewSuccessCollection = CommonFunctions.ReadCSVFile(winName, strScrips).ToList();
                int i = 1;
                int segment;
                foreach (var item in ViewSuccessCollection)
                {
#if TWS
                    int cnt = 0, z = 0, error = 0;
                    string Segment_Name = string.Empty;
                    OrderModel csv = new OrderModel();
                    csv.Exchange = Enumerations.Order.Exchanges.BSE.ToString();
                    csv.BuySellIndicator = item.BuySellIndicator;
                    csv.OriginalQty = item.OriginalQty;
                    csv.PendingQuantity = item.OriginalQty;
                    csv.RevealQty = item.RevealQty;


                    bool result = int.TryParse(item.ScripCodeID, out z);
                    if (result == false)
                    {
                        csv.ScripCode = CommonFunctions.GetScripCodeFromScripID(item.ScripCodeID);
                        Segment_Name = CommonFunctions.GetSegmentID(csv.ScripCode);
                        csv.Symbol = item.ScripCodeID.Trim();
                    }
                    else
                    {
                        csv.Symbol = CommonFunctions.GetScripId(Convert.ToInt64(item.ScripCodeID), "BSE");
                        csv.ScripCode = Convert.ToInt64(item.ScripCodeID);
                        Segment_Name = CommonFunctions.GetSegmentID(csv.ScripCode);
                    }
                    csv.ScripCodeID = csv.Symbol;
                    // csv.ScripCode = item.ScripCode;
                    csv.ScreenId = (int)Enumerations.WindowName.Batch_Order;
                    csv.ExecInst = Enumerations.Order.ExecInst.PersistentOrder.ToString();
                    csv.ParticipantCode = "";
                    csv.FreeText3 = "fdf";
                    csv.Filler_c = "fdf";
                    segment = CommonFunctions.GetSegmentFromScripCode(csv.ScripCode);
                    string ExchangeSegment = string.Empty;

                    if (segment == 1)
                    {
                        item.Segment = "EQX";
                        ExchangeSegment = Enumerations.Segment.Equity.ToString();
                    }
                    else if (segment == 2)
                    {
                        item.Segment = "EDX";
                        ExchangeSegment = Enumerations.Segment.Derivative.ToString();
                    }
                    else if (segment == 3)
                    {
                        item.Segment = "CDX";
                        ExchangeSegment = Enumerations.Segment.Currency.ToString();
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("Invalid Scrip Code", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        error += 1;
                        // return;
                    }


                    if (item.Segment == "EQX") //Equity
                    {
                        csv.Segment = Enumerations.Order.ScripSegment.Equity.ToString();
                        csv.SegmentFlag = CommonFunctions.SegmentFlag(csv.Segment); //(int)Enum.Parse(typeof(Enumerations.Order.ScripSegment), Enumerations.Order.ScripSegment.Equity.ToString());
                        csv.Symbol = CommonFunctions.GetScripId(csv.ScripCode, Enumerations.Exchange.BSE, Enumerations.Segment.Equity);
                        csv.MarketLot = MasterSharedMemory.objMastertxtDictBaseBSE.Values.Where(x => x.ScripCode == csv.ScripCode).Select(x => x.MarketLot).First();
                        csv.TickSize = MasterSharedMemory.objMastertxtDictBaseBSE.Values.Where(x => x.ScripCode == csv.ScripCode).Select(x => x.TickSize).First().ToString();
                        csv.Group = CommonFunctions.GetGroupName(csv.ScripCode, Enumerations.Exchange.BSE, Enumerations.Segment.Equity);
                        DecimalPoint = CommonFunctions.GetDecimal(Convert.ToInt32(csv.ScripCode), Enumerations.Exchange.BSE, Enumerations.Segment.Equity);
                        i++;
                    }
                    else if (item.Segment == "EDX") //Derivative
                    {
                        csv.Segment = Enumerations.Order.ScripSegment.Derivative.ToString();
                        csv.SegmentFlag = CommonFunctions.SegmentFlag(csv.Segment); //(int)Enum.Parse(typeof(Enumerations.Order.ScripSegment), Enumerations.Order.ScripSegment.Derivative.ToString());
                        csv.Symbol = CommonFunctions.GetScripId(csv.ScripCode, Enumerations.Exchange.BSE, Enumerations.Segment.Derivative);
                        try
                        {
                            csv.MarketLot = MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Where(x => x.ContractTokenNum == csv.ScripCode).Select(x => x.MinimumLotSize).First();
                            // csv.TickSize = MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Where(x => x.ContractTokenNum == csv.ScripCode).Select(x => x.TickSize).First().ToString();
                            csv.TickSize = MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Where(x => x.ContractTokenNum == csv.ScripCode).Select(x => x.TickSize).First().ToString();
                            DecimalPoint = CommonFunctions.GetDecimal(Convert.ToInt32(csv.ScripCode), Enumerations.Exchange.BSE, Enumerations.Segment.Derivative);
                            i++;
                            //csv.Group = CommonFunctions.GetGroupName(item.ScripCode, Enumerations.Exchange.BSE, Enumerations.Segment.Derivative);
                        }
                        catch (Exception ex)
                        {
                            System.Windows.MessageBox.Show(i + " position scrip is not of Derivative Segment");
                            cnt++;
                            i++;
                        }
                    }
                    else if (item.Segment == "CDX") //Currency
                    {
                        csv.Segment = Enumerations.Order.ScripSegment.Currency.ToString();
                        csv.SegmentFlag = CommonFunctions.SegmentFlag(csv.Segment); //(int)Enum.Parse(typeof(Enumerations.Order.ScripSegment), Enumerations.Order.ScripSegment.Currency.ToString());
                        csv.Symbol = CommonFunctions.GetScripId(csv.ScripCode, Enumerations.Exchange.BSE, Enumerations.Segment.Currency);
                        try
                        {
                            csv.MarketLot = MasterSharedMemory.objMasterCurrencyDictBaseBSE.Values.Where(x => x.ContractTokenNum == csv.ScripCode).Select(x => x.MinimumLotSize).First();
                            // csv.TickSize = MasterSharedMemory.objMasterCurrencyDictBaseBSE.Values.Where(x => x.ContractTokenNum == csv.ScripCode).Select(x => x.TickSize).First().ToString();
                            csv.TickSize = MasterSharedMemory.objMasterCurrencyDictBaseBSE.Values.Where(x => x.ContractTokenNum == csv.ScripCode).Select(x => x.TickSize).First().ToString();
                            DecimalPoint = CommonFunctions.GetDecimal(Convert.ToInt32(csv.ScripCode), Enumerations.Exchange.BSE, Enumerations.Segment.Currency);
                            i++;
                            //Derivative Group fetch from memory
                            //csv.Group = CommonFunctions.GetGroupName(item.ScripCode, Enumerations.Exchange.BSE, Enumerations.Segment.Currency);
                        }
                        catch (Exception ex)
                        {
                            System.Windows.MessageBox.Show(i + " position scrip is not of Currency Segment");
                            cnt++;
                            i++;
                        }
                    }

                    #region Fields value not stored yet
                    //decimalpoint
                    //Remarks
                    #endregion


                    if (DecimalPoint == 2)
                    {
                        csv.PriceS = String.Format("{0:0.00}", (item.Price / Math.Pow(10, DecimalPoint))).ToString();
                        //Convert.ToInt64(Convert.ToDouble(item.Price / Math.Pow(10, DecimalPoint)));
                        csv.Price = item.Price;
                        csv.TriggerPriceS = String.Format("{0:0.00}", (item.TriggerPrice / Math.Pow(10, DecimalPoint))).ToString();
                        csv.TriggerPrice = item.TriggerPrice;
                    }
                    else
                    {
                        csv.PriceS = String.Format("{0:0.0000}", ((item.Price / Math.Pow(10, DecimalPoint)))).ToString();
                        //csv.PriceS = Convert.ToInt64(item.Price / Math.Pow(10, DecimalPoint));
                        csv.Price = item.Price;
                        csv.TriggerPriceS = String.Format("{0:0.00}", (item.TriggerPrice / Math.Pow(10, DecimalPoint))).ToString();
                        csv.TriggerPrice = item.TriggerPrice;
                    }
                    csv.ClientType = item.ClientType;

                    if (csv.ClientType == "OWN")
                        csv.ClientId = "OWN";
                    else
                        csv.ClientId = item.ClientId;

                    csv.OrderType = item.OrderType;
                    if (item.OrderType == "G")
                    {
                        LoadMarketProtection = UtilityOrderDetails.GETInstance.MktProtection == null ? "1.0" : UtilityOrderDetails.GETInstance.MktProtection;
                        var protectionPercent = Convert.ToInt32(Convert.ToDecimal(LoadMarketProtection) * 100);
                        csv.ProtectionPercentage = Convert.ToString(protectionPercent);
                        // csv.ProtectionPercentage = UtilityOrderDetails.GETInstance.MktProtection == null ? "1.0" : UtilityOrderDetails.GETInstance.MktProtection;// Convert.ToString(1);
                        csv.Price = 0;
                        csv.PriceS = item.ProtectionPercentage != null ? $"{"M("}{("1.0"):0.00}{"%)"}" : $"{"M("}{UtilityOrderDetails.GETInstance.MktProtection:0.00}{"%)"}";// "M ( " + UtilityOrderDetails.GETInstance.MktProtection == null ? "1.0" : UtilityOrderDetails.GETInstance.MktProtection + " % )";
                    }
                    else
                        csv.ProtectionPercentage = (UtilityOrderDetails.GETInstance.MktProtection == null ? "1.0" : UtilityOrderDetails.GETInstance.MktProtection).ToString();//Convert.ToString(1);
                    //if (item.OrderType == "G")
                    //    csv.ProtectionPercentage = Convert.ToString(1);
                    //else
                    //    csv.ProtectionPercentage = Convert.ToString(1);

                    csv.ParticipantCode = item.ParticipantCode;

                    csv.OrderRetentionStatus = item.OrderRetentionStatus;// check that retention should be EOD,IOC,EOS
                    //csv.TriggerPrice = item.TriggerPrice;

                    csv.SenderLocationID = UtilityLoginDetails.GETInstance.SenderLocationId;
                    csv.MessageTag = GetOrderMessageTag();
                    if (cnt == 0 && error == 0)
                    {
                        csv.BatchKey = string.Format("{0}_{1}", csv.ScripCode, csv.MessageTag);
                        BatchOrderCollection.Add(csv);
                        //i++;
                    }
                    csv.OrderFomLoadButton = item.OrderFomLoadButton;
#endif
                }
                TotBuyQty = BatchOrderCollection.Where(y => y.BuySellIndicator == "B").Sum<OrderModel>(x => x.OriginalQty).ToString();
                TotSellQty = BatchOrderCollection.Where(y => y.BuySellIndicator == "S").Sum<OrderModel>(x => x.OriginalQty).ToString();
                BatchOrderTitleCount = "Batch Submission - Orders (Total Count : " + BatchOrderCollection.Count + ")";
            }
            else
            {
                return;
            }
        }

        public uint GetOrderMessageTag()
        {
            //if(UtilityLoginDetails.GETInstance.MaxMessageTag?)
            //MemoryManager.MessageTag = MemoryManager.MessageTag + 1; 
            UtilityLoginDetails.GETInstance.MaxMessageTag = UtilityLoginDetails.GETInstance.MaxMessageTag + 1;
            uint result = System.Convert.ToUInt32(UtilityLoginDetails.GETInstance.MaxMessageTag);
            return result;
        }

        private void OnClickOfSelectAllButton(object e1)
        {
            if (BatchOrderCollection?.Count > 0)
            {
                if (btnSelectAllContent == "Select All")
                {
                    SelectedValue = BatchOrderCollection.ToList();
                    if (batch != null)
                    {
                        batch.BatchGrid.SelectAll();
                        batch.BatchGrid.Focus();
                        //batch.BatchGrid.Focus();
                    }
                    ViewReply = SelectedValue.Count + " Order(s) Selected";
                    btnSelectAllContent = "Deselect All";
                }
                else if (btnSelectAllContent == "Deselect All")
                {
                    ViewReply = SelectedValue.Count + " Order(s) Deselected";
                    SelectedValue.Clear();
                    if (batch != null)
                    {
                        batch.BatchGrid.UnselectAll();
                        batch.BatchGrid.Focus();
                    }
                    btnSelectAllContent = "Select All";
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("No orders to select", "Warning", MessageBoxButtons.OK);
                return;
            }
        }

        private void OnClickOfRemoveButton()
        {
            try
            {
                if (SelectedValue?.Count > 0)
                {
                    if (System.Windows.MessageBox.Show("Do you want to Delete Order ?", "Confirmation", System.Windows.MessageBoxButton.YesNo) == System.Windows.MessageBoxResult.Yes)
                    {
                        for (int i = 0; i < BatchOrderCollection.Count; i++)
                        {
                            if (BatchGridSelectedItem != null)
                            {
                                int index = BatchOrderCollection.IndexOf(BatchOrderCollection.Where(x => x.BatchKey == BatchGridSelectedItem.BatchKey).FirstOrDefault());
                                //int index = BatchOrderCollection.IndexOf(BatchOrderCollection.Where(x => x.ScripCode == BatchGridSelectedItem.ScripCode).FirstOrDefault());
                                BatchOrderCollection.RemoveAt(index);
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
                TotBuyQty = BatchOrderCollection.Where(y => y.BuySellIndicator == "B").Sum<OrderModel>(x => x.OriginalQty).ToString();
                TotSellQty = BatchOrderCollection.Where(y => y.BuySellIndicator == "S").Sum<OrderModel>(x => x.OriginalQty).ToString();
                BatchOrderTitleCount = "Batch Submission - Orders (Total Count : " + BatchOrderCollection.Count + ")";
                ViewReply = string.Empty;
            }
        }

        private void OnClickOfRemoveAllButton()
        {
            if (BatchOrderCollection?.Count > 0)
            {
                if (System.Windows.MessageBox.Show("Do you want to Delete Orders ?", "Confirmation", System.Windows.MessageBoxButton.YesNo) == System.Windows.MessageBoxResult.Yes)
                {
                    BatchOrderCollection.Clear();
                    BatchOrderQueue.Clear();
                    ViewSuccessCollection.Clear();
                    CommonFunctions.SuccessCollection.Clear();
                }
                TotBuyQty = BatchOrderCollection?.Where(y => y.BuySellIndicator == "B").Sum<OrderModel>(x => x.OriginalQty).ToString();
                TotSellQty = BatchOrderCollection?.Where(y => y.BuySellIndicator == "S").Sum<OrderModel>(x => x.OriginalQty).ToString();
                BatchOrderTitleCount = "Batch Submission - Orders (Total Count : " + BatchOrderCollection.Count + ")";
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("No orders to Remove", "Warning", MessageBoxButtons.OK);
                return;
            }
        }

        private bool ValidateUserInputs(int TotalQty, int RevQty, string Rate, string ClientID, string MktPT, string OrdType)
        {
            try
            {
                //Quantity Validation
                var Validate_Message = Validations.ValidateVolume(TotalQty, 9999999999L, 1, 0, false);
                if (Validate_Message.Trim().ToUpper() != "Success".ToUpper())
                {
                    ViewReply = "Quantity : " + Validate_Message;
                    return false;
                }

                //Total Qty should be Multiple of Mkt Lot
                var MarketLot = Convert.ToInt64(CommonFunctions.GetMarketLot(SelectedValue.Last().ScripCode));

                if (MarketLot != 0)
                {
                    if (TotalQty % MarketLot != 0)
                    {
                        ViewReply = "Quantity should be a multiple of Mkt lot[" + MarketLot + "]";
                        return false;
                    }
                }

                else
                {
                    ViewReply = "Market Lot is Zero";
                    return false;
                }

                //Validate Reveal Qty
                Validate_Message = Validations.ValidateRevlQty(RevQty, 9999999999L, 1, 0, false);
                if (Validate_Message.Trim().ToUpper() != "Success".ToUpper())
                {
                    ViewReply = "RevealQty : " + Validate_Message;
                    return false;
                }

                // Reveal Qty volume should not be greater than actual volume
                if (RevQty > TotalQty)
                {
                    ViewReply = "Disclosed quantity can't be greater than actual quantity";
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
                    ViewReply = "Reveal quantity should > or =  to 10 % of total quantity";
                    return false;
                }

                //Reveal Qty should be Multiple of Mkt Lot
                if (RevQty % MarketLot != 0)
                {
                    ViewReply = " Reveal quantity is Not a Multiple of Mkt Lot[" + MarketLot + "]";
                    return false;
                }

                //if Rate is zero
                if (!OrdType.Equals("G"))
                {
                    if (Rate == "0")
                    {
                        ViewReply = "Rate Entered is Empty";
                        return false;
                    }
                }

                //Check Number after decimal point
                var Segment = Common.CommonFunctions.GetSegmentID(SelectedValue.Last().ScripCode);
                var DecimalPoint = Common.CommonFunctions.GetDecimal(Convert.ToInt32(SelectedValue.Last().ScripCode), "BSE", Segment);
                var rate = string.Empty;
                if (OrdType != "G")
                {
                    rate = Convert.ToString(Rate);
                    if (rate.Contains(".") && rate.Substring(rate.IndexOf(".") + 1).Length > DecimalPoint)
                    {
                        ViewReply = "Rate More than " + DecimalPoint + " decimal places!";
                        return false;
                    }

                    //Total Rate should be Multiple of Tick Size
                    var TickSize = CommonFunctions.GetTickSize(SelectedValue.Last().ScripCode);
                    if (!string.IsNullOrEmpty(TickSize))
                    {
                        if (Convert.ToDouble(TickSize) != 0)
                        {
                            if (Convert.ToInt64(Convert.ToDouble(rate) * Math.Pow(10, DecimalPoint)) % Convert.ToInt64(Convert.ToDouble(TickSize)) != 0)
                            {
                                ViewReply = "Rate should be a multiple of TickSize[" + TickSize + "]";
                                return false;
                            }
                        }
                        else
                        {
                            ViewReply = "Tick size is Zero.";
                            return false;
                        }
                    }

                    else
                    {
                        ViewReply = "Tick size is Zero.";
                        return false;
                    }
                }
                //TODO : MarketProtection validation
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
                ViewReply = "Error In Validation of order";
                return false;
            }
            return true;
        }


        private void OnClickOfChangeButton()
        {
            //Add MKTProt cannot be greater the 99
            //Trigger Price Remaining
            bool validate;
            if (SelectedValue != null && SelectedValue.Count > 0)
            {
                if (BatchGridSelectedItem.OrderType == "G")
                {
                    validate = ValidateUserInputs(Convert.ToInt32(ModTotQty), Convert.ToInt32(ModRevealQty), ModRate, ModClientID, ModMktProt, BatchGridSelectedItem.OrderType);
                }
                else
                {
                    // ModMktProt = "1.0";
                    validate = ValidateUserInputs(Convert.ToInt32(ModTotQty), Convert.ToInt32(ModRevealQty), ModRate, ModClientID, ModMktProt, BatchGridSelectedItem.OrderType);
                }

                if (!validate)
                {
                    return;
                }

                if (BatchGridSelectedItem != null)
                {
                    if (batch != null)
                    {
                        int Index = batch.BatchGrid.SelectedIndex;
                        OrderModel obj = new OrderModel();
                        obj.BuySellIndicator = ModBSFlag.ToUpper();
                        obj.OriginalQty = Convert.ToInt32(ModTotQty);
                        obj.RevealQty = Convert.ToInt32(ModRevealQty);
                        obj.ScripCodeID = ModScripID;
                        obj.ScripCode = BatchGridSelectedItem.ScripCode;
                        obj.PriceS = ModRate;
                        if (string.IsNullOrEmpty(ModTrgRate))
                        {
                            obj.TriggerPriceS = string.Empty;
                            obj.TriggerPrice = 0;
                        }
                        else
                        {
                            obj.TriggerPriceS = ModTrgRate;
                            // obj.TriggerPrice =Convert.ToInt64(ModTrgRate);
                        }

                        obj.ClientId = ModClientID;

                        if (BatchGridSelectedItem.ClientType == Enumerations.Order.ClientTypes.CLIENT.ToString())
                            obj.ClientType = ModClientTypeSelected;
                        else if (BatchGridSelectedItem.ClientType == Enumerations.Order.ClientTypes.INST.ToString())
                            obj.ClientType = ModClientTypeSelected;
                        else if (BatchGridSelectedItem.ClientType == Enumerations.Order.ClientTypes.OWN.ToString())
                            obj.ClientType = ModClientTypeSelected;
                        else
                            obj.ClientType = ModClientTypeSelected;

                        // obj.ClientType = BatchGridSelectedItem.ClientType;

                        if (ModRetainSelected == Enumerations.Order.RetType.EOD.ToString())
                        {
                            obj.OrderRetentionStatus = "EOTODY";
                        }
                        else if (ModRetainSelected == Enumerations.Order.RetType.EOS.ToString())
                        {
                            obj.OrderRetentionStatus = "EOSESS";
                        }
                        else
                        {
                            obj.OrderRetentionStatus = "IOC";
                        }

                        if (ModOrdTypeSelected == Enumerations.Order.OrderTypesBatch.LIMIT_L.ToString().Replace("_", "-") || (ModOrdTypeSelected == Enumerations.Order.OrderTypesBatch.OCO_L.ToString().Replace("_", "-")))
                        {
                            ModOrdTypeSelected = "L";
                        }
                        else if (ModOrdTypeSelected == Enumerations.Order.OrderTypesBatch.MKT_G.ToString().Replace("_", "-"))
                        {
                            ModOrdTypeSelected = "G";
                        }
                        else if (ModOrdTypeSelected == Enumerations.Order.OrderTypesBatch.SLMKT_P.ToString().Replace("_", "-") || ModOrdTypeSelected == Enumerations.Order.OrderTypesBatch.SL_P.ToString().Replace("_", "-"))
                        {
                            ModOrdTypeSelected = "P";
                        }
                        else if ((ModOrdTypeSelected == Enumerations.Order.OrderTypesBatch.BDEAL_K.ToString().Replace("_", "-")))
                        {
                            ModOrdTypeSelected = "K";
                        }
                        else
                        {
                            ModOrdTypeSelected = "O";
                        }
                        obj.OrderType = ModOrdTypeSelected;
                        //  Enumerations.Order.OrderTypesBatch.OCO_L.ToString().Replace("_", "-"));
                        #region additional fields
                        obj.Segment = CommonFunctions.GetSegmentID(BatchGridSelectedItem.ScripCode);
                        obj.SenderLocationID = UtilityLoginDetails.GETInstance.SenderLocationId;
                        obj.Symbol = BatchGridSelectedItem.Symbol;
                        obj.OrderRemarks = BatchGridSelectedItem.OrderRemarks;
                        // if (ModOrdTypeSelected != "G")

                        // var protectionPercent = Convert.ToInt32(Convert.ToDecimal(BatchGridSelectedItem.ProtectionPercentage) * 100);
                        // obj.ProtectionPercentage = Convert.ToString(protectionPercent);
                        obj.ProtectionPercentage = BatchGridSelectedItem.ProtectionPercentage;

                        obj.PendingQuantity = BatchGridSelectedItem.PendingQuantity;
                        obj.ScreenId = BatchGridSelectedItem.ScreenId;
                        obj.CurrentScreenId = BatchGridSelectedItem.CurrentScreenId;
                        obj.MarketLot = BatchGridSelectedItem.MarketLot;
                        obj.TickSize = BatchGridSelectedItem.TickSize;
                        obj.Group = BatchGridSelectedItem.Group;
                        obj.ExecInst = BatchGridSelectedItem.ExecInst;
                        obj.ParticipantCode = BatchGridSelectedItem.ParticipantCode;
                        obj.SegmentFlag = BatchGridSelectedItem.SegmentFlag;
                        obj.FreeText3 = BatchGridSelectedItem.FreeText3;
                        obj.Filler_c = BatchGridSelectedItem.Filler_c;
                        obj.PartitionID = Convert.ToUInt16(CommonFunctions.GetPartitionId(BatchGridSelectedItem.ScripCode, "BSE", BatchGridSelectedItem.Segment));//BatchOrder.PartitionID;
                        obj.MarketSegmentID = CommonFunctions.GetProductId(BatchGridSelectedItem.ScripCode, "BSE", BatchGridSelectedItem.Segment);//BatchOrder.MarketSegmentID;
                        obj.ScripName = BatchGridSelectedItem.ScripName;
                        int Batch_decimalPoint = CommonFunctions.GetDecimal(Convert.ToInt32(BatchGridSelectedItem.ScripCode), "BSE", BatchGridSelectedItem.Segment);
                        obj.BatchKey = BatchGridSelectedItem.BatchKey;
                        obj.Price = Convert.ToInt64(Convert.ToDouble(ModRate) * Math.Pow(10, Batch_decimalPoint));
                        obj.TriggerPrice = Convert.ToInt64(Convert.ToDouble(ModTrgRate) * Math.Pow(10, Batch_decimalPoint));//BatchGridSelectedItem.TriggerPrice;

                        if (Batch_decimalPoint == 2)
                        {
                            obj.PriceS = String.Format("{0:0.00}", Convert.ToDecimal(ModRate)).ToString();
                            obj.TriggerPriceS = String.Format("{0:0.00}", Convert.ToDecimal(ModTrgRate)).ToString();
                        }
                        else
                        {
                            obj.PriceS = String.Format("{0:0.0000}", Convert.ToDecimal(ModRate)).ToString();
                            obj.TriggerPriceS = String.Format("{0:0.0000}", Convert.ToDecimal(ModTrgRate)).ToString();
                        }

                        if (obj.OrderType == "L")
                        {
                            if (obj.TriggerPrice == 0)
                            {
                                obj.IsOCOOrder = false;
                            }
                            else if (obj.TriggerPrice > 0)
                            {
                                obj.IsOCOOrder = true;
                            }
                        }

                        if (obj.OrderType == "L")
                        {
                            //if (obj.IsOCOOrder)//OCO order
                            //{
                            //    //oOrderModel.OrderType = Enumerations.Order.OrderTypes.OCO.ToString();
                            //    System.Windows.MessageBox.Show("OCO order placement not allowed");
                            //    return;
                            //}
                        }
                        else if (obj.OrderType == "P")
                        {
                            if (obj.TriggerPrice == 0)
                            {
                                System.Windows.MessageBox.Show("Enter Trigger Price for STOPLOSS");
                                return;
                            }
                            else
                            {
                                if (obj.Price == 0)
                                    // obj.OrderType = Enumerations.Order.OrderTypes.STOPLOSSMKT.ToString();
                                    obj.OrderType = "P";
                                else
                                    //  obj.OrderType = Enumerations.Order.OrderTypes.STOPLOSS.ToString();
                                    obj.OrderType = "P";
                            }
                        }
                        else if (obj.OrderType == "G")
                        {
                            obj.PriceS = BatchGridSelectedItem.ProtectionPercentage != null ? $"{"M("}{("1.0"):0.00}{"%)"}" : $"{"M("}{UtilityOrderDetails.GETInstance.MktProtection:0.00}{"%)"}";
                            obj.Price = 0;
                        }

                        obj.ClientId = ModClientID;

                        ModBSFlag = string.Empty;
                        //  Enumerations.Order.OrderTypesBatch.SL_P.ToString().Replace("_", "-"));
                        ModTotQty = string.Empty;
                        // Enumerations.Order.OrderTypesBatch.SLMKT_P.ToString().Replace("_", "-"))
                        ModRevealQty = string.Empty;
                        //  Enumerations.Order.OrderTypesBatch.BDEAL_K.ToString().Replace("_", "-"))
                        ModScripID = string.Empty;
                        // Enumerations.Order.OrderTypesBatch.ODDL_O.ToString().Replace("_", "-"));
                        ModRate = string.Empty;
                        ModClientID = string.Empty;
                        ModTrgRate = string.Empty;
                        #endregion
                        BatchOrderCollection[Index] = obj;
                    }
                }
                else
                {
                    ViewReply = "Can't find order";
                }
                TotBuyQty = BatchOrderCollection.Where(y => y.BuySellIndicator == "B").Sum<OrderModel>(x => x.OriginalQty).ToString();
                TotSellQty = BatchOrderCollection.Where(y => y.BuySellIndicator == "S").Sum<OrderModel>(x => x.OriginalQty).ToString();
            }
            else
            {
                ViewReply = "Please Select the Order to Modify";
            }
        }


        private void OnClickOfSaveButton()
        {
            if (BatchOrderCollection.Count > 0)
            {
                StreamWriter writer = null;
                SaveFileDialog dlg = new SaveFileDialog();
                dlg.DefaultExt = ".csv";
                dlg.Filter = "CSV Files(*.csv) | *.csv | DOT(*.txt*) | *.txt ";//"csv(*.csv) | *.csv;| *.DOT | (*.doc)";//"CSV (*.CSV;*.DOT;)|*.CSV;*.DOT)";//"CSV Files (.csv)|*.csv";
                // Nullable<bool> result = dlg.ShowDialog();
                if (dlg.ShowDialog() == DialogResult.OK)
                {

                    try
                    {
                        if (!File.Exists(Path.GetFullPath(dlg.FileName)))
                        {
                            writer = new StreamWriter(dlg.FileName, true, Encoding.UTF8);
                            writer.Write("Buy/Sell,Qty,Rev.Qty,Scrip Code,Rate,Short/Client ID,Retention Status,Client Type,Order Type,CP Code,TrgRate");//, CP Code Remaining                      
                            writer.Write(writer.NewLine);

                            foreach (var dr in BatchOrderCollection)
                            {
                                writer.Write(dr.BuySellIndicator + "," + dr.OriginalQty + "," + dr.RevealQty + "," + dr.ScripCode + "," + Convert.ToDouble(dr.PriceS) + "," + dr.ClientId + "," +
                                    dr.OrderRetentionStatus + "," + dr.ClientType + "," + dr.OrderType + "," + dr.ParticipantCode + "," + Convert.ToDouble(dr.TriggerPriceS));

                                writer.Write(writer.NewLine);
                            }
                        }
                        else
                        {
                            File.Delete(Path.GetFullPath(dlg.FileName));
                            writer = new StreamWriter(dlg.FileName, true, Encoding.UTF8);
                            writer.Write("Buy/Sell,Qty,Rev.Qty,Scrip Code,Rate,Short/Client ID,Retention Status,Client Type,Order Type,CP Code,TrgRate");//, CP Code Remaining                      
                            writer.Write(writer.NewLine);

                            foreach (var dr in BatchOrderCollection)
                            {
                                writer.Write(dr.BuySellIndicator + "," + dr.OriginalQty + "," + dr.RevealQty + "," + dr.ScripCode + "," + Convert.ToDouble(dr.PriceS) + "," + dr.ClientId + "," +
                                    dr.OrderRetentionStatus + "," + dr.ClientType + "," + dr.OrderType + "," + dr.ParticipantCode + "," + Convert.ToDouble(dr.TriggerPriceS));

                                writer.Write(writer.NewLine);
                            }
                        }
                        ViewReply = BatchOrderCollection.Count + " Order(s) Saved in file :" + dlg.FileName.ToString();
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
            else
            {
                System.Windows.MessageBox.Show("No Orders found to be saved");
            }
        }

        private void OnClickOfStopButton()
        {

        }

        private void OnClickOfBondCalcButton()
        {

        }

        private void OnChangeOfBulkPrice(object sender)
        {
            CopyOfBatchOrderCollection = BatchOrderCollection;
            DataGrid dg = sender as DataGrid;
            if (!string.IsNullOrEmpty(BulkNewPrice) && (!string.IsNullOrEmpty(BulkScripCodeID)))
            {
                try
                {
                    string Segment_Name = string.Empty;
                    string Symbol = string.Empty;
                    int z = 0, Batch_decimalPoint = 0;
                    long ScripCode = 0;
                    bool result = int.TryParse(BulkScripCodeID, out z);
                    if (result == false)
                    {
                        ScripCode = CommonFunctions.GetScripCodeFromScripID(BulkScripCodeID);
                        Segment_Name = CommonFunctions.GetSegmentID(ScripCode);
                        Symbol = BulkScripCodeID.Trim();
                    }
                    else
                    {
                        Symbol = CommonFunctions.GetScripId(Convert.ToInt64(BulkScripCodeID), "BSE");
                        ScripCode = Convert.ToInt64(BulkScripCodeID);
                        Segment_Name = CommonFunctions.GetSegmentID(ScripCode);
                    }

                    if (!string.IsNullOrEmpty(Segment_Name))
                        Batch_decimalPoint = CommonFunctions.GetDecimal(Convert.ToInt32(ScripCode), "BSE", Segment_Name);
                    else
                        ViewReply = "Invalid Scrip Code/ID";

                    if (CopyOfBatchOrderCollection.Count > 0)
                    {
                        foreach (var item in CopyOfBatchOrderCollection)
                        {
                            if (item.ScripCodeID == BulkScripCodeID)
                            {
                                if ((item.OrderType == "L" && item.TriggerPrice == 0) || item.OrderType == "K" || item.OrderType == "O")
                                {
                                    item.Price = Convert.ToInt64(Convert.ToDouble(Convert.ToDecimal(BulkNewPrice)) * Math.Pow(10, Batch_decimalPoint));
                                    if (Batch_decimalPoint == 2)
                                    {
                                        item.PriceS = String.Format("{0:0.00}", Convert.ToDecimal(BulkNewPrice)).ToString();
                                        ChangedRecords++;
                                    }
                                    else
                                    {
                                        item.PriceS = String.Format("{0:0.0000}", Convert.ToDecimal(BulkNewPrice)).ToString();
                                        ChangedRecords++;
                                    }
                                }
                                else
                                {
                                  //  IgnoredRecords++;
                                    if (!string.IsNullOrEmpty(BulkNewPrice) && (!string.IsNullOrEmpty(BulkScripCodeID)) && (!string.IsNullOrEmpty(BulkNewTrgPrice)))
                                    {
                                        if (ValidateTriggerPriceForOCOAndStopLoss(BulkNewTrgPrice, item.BuySellIndicator, BulkNewPrice, item.OrderType))
                                        {
                                            item.Price = Convert.ToInt64(Convert.ToDouble(Convert.ToDecimal(BulkNewPrice)) * Math.Pow(10, Batch_decimalPoint));
                                            item.TriggerPrice = Convert.ToInt64(Convert.ToDouble(Convert.ToDecimal(BulkNewTrgPrice)) * Math.Pow(10, Batch_decimalPoint));
                                            if (Batch_decimalPoint == 2)
                                            {
                                                item.PriceS = String.Format("{0:0.00}", Convert.ToDecimal(BulkNewPrice)).ToString();
                                                item.TriggerPriceS = String.Format("{0:0.00}", Convert.ToDecimal(BulkNewTrgPrice)).ToString();
                                                ChangedRecords++;
                                            }
                                            else
                                            {
                                                item.PriceS = String.Format("{0:0.0000}", Convert.ToDecimal(BulkNewPrice)).ToString();
                                                item.TriggerPriceS = String.Format("{0:0.0000}", Convert.ToDecimal(BulkNewTrgPrice)).ToString();
                                                ChangedRecords++;
                                            }
                                        }
                                        else
                                        {
                                            ErrorRecords++;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        ViewReply = "Scrip Not available for update";
                        return;
                    }
                    BatchOrderCollection = new ObservableCollection<OrderModel>();
                    BatchOrderCollection = CopyOfBatchOrderCollection;
                    ViewReply = "Changed Records: " + ChangedRecords + "; Error Records: " + ErrorRecords; //+"; Ignored Records:"+IgnoredRecords;
                    NotifyPropertyChanged("BatchOrderCollection");
                }
                catch (Exception e)
                {
                    ExceptionUtility.LogError(e);
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Enter New Price and ScripCode/ID ", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void EscapeUsingUserControl(object e)
        {
            View.Order.BatchOrder ca = e as View.Order.BatchOrder;
            ca?.Hide();
        }

        private void ReadDataFromOrderMemory(object Order, string status)
        {
            if (status == Enumerations.OrderExecutionStatus.Batch.ToString())
            {
                //uiContext?.Send(x => BatchOrderCollection?.Clear(), null);
                if (BatchOrderQueue != null && BatchOrderQueue.Count > 0)
                {
                    foreach (OrderModel item in BatchOrderQueue.Where(x => ((OrderModel)x).InternalOrderStatus == Enumerations.OrderExecutionStatus.Batch.ToString()))
                    {
                        OrderModel oBatchOrderModel = new OrderModel();
                        oBatchOrderModel = item;

                        if (oBatchOrderModel.ReplyCode == 0)
                        {
                            int index = BatchOrderCollection.IndexOf(BatchOrderCollection.Where(x => x.BatchKey == oBatchOrderModel.BatchKey).FirstOrDefault());
                            // int index1 = CommonFunctions.SuccessCollection.IndexOf(BatchOrderCollection.Where(x => x.BatchKey == oBatchOrderModel.BatchKey).FirstOrDefault());
                            if (index != -1)
                            {
                                uiContext?.Send(x => BatchOrderCollection.RemoveAt(index), null);
                                //uiContext?.Send(x => CommonFunctions.SuccessCollection.RemoveAt(index1), null);
                            }
                            //uiContext?.Send(x => BatchOrderCollection?.Add(oBatchOrderModel), null);
                        }
                        else
                        {
                            int index = BatchOrderCollection.IndexOf(BatchOrderCollection.Where(x => x.BatchKey == oBatchOrderModel.BatchKey).FirstOrDefault());
                            if (index != -1)
                            {
                                uiContext?.Send(x =>
                                {
                                    if (BatchOrderCollection != null && BatchOrderCollection[index] != null)
                                    {
                                        if (BatchOrderQueue != null && BatchOrderQueue.Count > 0)
                                        {
                                            if (BatchOrderQueue.Count >= BatchOrderCollection[index].BatchOrderQueueIndex)
                                            {
                                                if (BatchOrderQueue[BatchOrderCollection[index].BatchOrderQueueIndex] != null)
                                                    BatchOrderCollection[index].ReplyText = ((OrderModel)BatchOrderQueue[BatchOrderCollection[index].BatchOrderQueueIndex]).ReplyText;
                                            }

                                        }
                                        //BatchOrderCollection[index].Reason = "Failed";
                                        //NotifyPropertyChanged("BatchOrderCollection");
                                    }
                                }, null);
                            }
                        }

                    }
                }
            }
        }

        private void OnClickOfSubmitButton()
        {
            CopyOfBatchOrderCollection = new ObservableCollection<OrderModel>();
            bool result = false;
            string scripName = string.Empty;
            string Validate_Error = string.Empty;
            string ChangedOrderTyp = string.Empty;
            string Price = string.Empty;
            string TriggerPrice = string.Empty;
            int Batch_decimalPoint;
            OrderRequestProcessor oOrderRequestProcessor = null;

            BatchOrderQueue?.Clear();

            var length = SelectedValue.Count;

            for (int i = 0; i < length; i++)
            {
                SelectedValue[i].BatchOrderQueueIndex = i;
                BatchOrderQueue.Add(SelectedValue[i]);
            }

            #region Comment

            //while (BatchOrderQueue.Count > 0)
            //{
            //   OrderModel oOrderModel = (OrderModel)BatchOrderQueue.Dequeue();
            //    oOrderRequestProcessor = new OrderRequestProcessor(new AddOrder());
            //    oOrderRequestProcessor.ProcessRequest(oOrderModel);
            //    batchOrderAutoReset.WaitOne();
            //}


            //  foreach (var item in BatchOrderCollection)
            //            {
            //                //Need to work on order Type Check
            //#if TWS
            //                if (item.OrderType == "L")
            //                {
            //                    if (item.TriggerPrice == 0)
            //                    {
            //                        item.OrderType = Enumerations.Order.OrderTypes.LIMIT.ToString();
            //                        item.ExecInst = Enumerations.Order.ExecInst.PersistentOrder.ToString();
            //                        item.OrderRemarks = "Order";
            //                        item.SenderLocationID = UtilityLoginDetails.GETInstance.SenderLocationId;
            //                        item.Filler_c = "abc";//string.Empty;
            //                        item.FreeText3 = "abc";//string.Empty;
            //                        item.MarketSegmentID = "0";
            //                    }
            //                    else
            //                    {
            //                        item.OrderType = Enumerations.Order.OrderTypes.OCO.ToString();
            //                        MessageBox.Show("OCO order placement not allowed");
            //                        break;
            //                    }
            //                }
            //                //else if (item.OrderType == "G")
            //                //    item.OrderType = Enumerations.Order.OrderTypes.MARKET.ToString();
            //                //else if (item.OrderType == "K")
            //                //    item.OrderType = Enumerations.Order.OrderTypes.BLOCKDEAL.ToString();
            //                else if (item.OrderType == "P")
            //                {
            //                    if (item.TriggerPrice == 0)
            //                    {
            //                        MessageBox.Show("Enter Trigger Price for STOPLOSS");
            //                        break;
            //                    }
            //                    else
            //                    {
            //                        if (item.Price == 0)
            //                            item.OrderType = Enumerations.Order.OrderTypes.STOPLOSSMKT.ToString();
            //                        else
            //                            item.OrderType = Enumerations.Order.OrderTypes.STOPLOSS.ToString();
            //                    }
            //                }

            //                //if (item.BuySellIndicator == "B")
            //                //    item.BuySellIndicator = "BUY";
            //                //else if (item.BuySellIndicator == "S")
            //                //    item.BuySellIndicator = "SELL";

            //                int Decimal = CommonFunctions.GetDecimal(Convert.ToInt32(item.ScripCode), item.Exchange, item.Segment);
            //                string Price1 = Convert.ToString(Convert.ToDecimal(item.Price / Math.Pow(10, Decimal)));
            //                string TriggerPrice1 = Convert.ToString(Convert.ToDecimal(item.TriggerPrice / Math.Pow(10, Decimal)));
            //                if (Decimal == 2)
            //                {
            //                    Price = String.Format("{0:0.00}", Convert.ToInt32(Price1));
            //                    TriggerPrice = String.Format("{0:0.00}", Convert.ToInt32(TriggerPrice1));
            //                }
            //                else
            //                {
            //                    Price = String.Format("{0:0.0000}", Convert.ToInt32(Price1));
            //                    TriggerPrice = String.Format("{0:0.0000}", Convert.ToInt32(TriggerPrice1));
            //                }

            //                result = Validations.ValidateOrder(Price, TriggerPrice, item, ref Validate_Error, Decimal);
            //                if (result == true)
            //                {
            //                    MessageBox.Show("Order validated successfully");
            //                    //Convert Price in Long and send to processing
            //                    item.Price = Convert.ToInt64(Convert.ToDouble(Price) * Math.Pow(10, Decimal));

            //                    if (!string.IsNullOrEmpty(TriggerPrice))
            //                        item.TriggerPrice = Convert.ToInt64(Convert.ToDouble(TriggerPrice) * Math.Pow(10, Decimal));

            //                    //addition of order action field
            //                    item.OrderAction = Enumerations.Order.Modes.A.ToString();
            //                    //Processor.Order.OrderProcessor.ProcessOrderObject(item);
            //                    //OrderRequestProcessor oOrderRequestProcessor = new OrderRequestProcessor(new AddOrder());
            //                    //oOrderRequestProcessor.ProcessRequest(item);

            #endregion

            if (BatchOrderQueue.Count > 0)
            {
                try
                {
                    Task.Factory.StartNew(() =>
                    {
                        int count = BatchOrderQueue.Count;
                        for (int i = 0; i < count; i++)
                        {
                            OrderModel oOrderModel = new OrderModel();
                            OrderModel BatchOrder = (OrderModel)BatchOrderQueue[i];
                            oOrderModel.BatchOrderQueueIndex = BatchOrder.BatchOrderQueueIndex;
                            oOrderModel.BuySellIndicator = BatchOrder.BuySellIndicator;
                            oOrderModel.Exchange = BatchOrder.Exchange;
                            oOrderModel.Segment = BatchOrder.Segment;
                            oOrderModel.ClientId = BatchOrder.ClientId;
                            oOrderModel.ClientType = BatchOrder.ClientType;
                            oOrderModel.ScripCode = BatchOrder.ScripCode;
                            oOrderModel.SenderLocationID = UtilityLoginDetails.GETInstance.SenderLocationId;
                            oOrderModel.OrderRetentionStatus = BatchOrder.OrderRetentionStatus;
                            oOrderModel.Symbol = BatchOrder.Symbol;
                            oOrderModel.OrderRemarks = BatchOrder.OrderRemarks;
                            oOrderModel.Price = BatchOrder.Price;
                            oOrderModel.TriggerPrice = BatchOrder.TriggerPrice;
                            oOrderModel.OrderType = BatchOrder.OrderType;
                            var protectionPercent = Convert.ToInt16(Convert.ToDecimal(BatchOrder.ProtectionPercentage));
                            oOrderModel.ProtectionPercentage = Convert.ToString(protectionPercent);
                            //oOrderModel.ProtectionPercentage = BatchOrder.ProtectionPercentage;
                            oOrderModel.RevealQty = BatchOrder.RevealQty;
                            oOrderModel.OriginalQty = BatchOrder.OriginalQty;
                            oOrderModel.PendingQuantity = BatchOrder.PendingQuantity;
                            oOrderModel.ScreenId = BatchOrder.ScreenId;
                            oOrderModel.CurrentScreenId = BatchOrder.CurrentScreenId;
                            oOrderModel.MarketLot = BatchOrder.MarketLot;
                            oOrderModel.TickSize = BatchOrder.TickSize;
                            oOrderModel.Group = BatchOrder.Group;
                            oOrderModel.ExecInst = BatchOrder.ExecInst;
                            oOrderModel.ParticipantCode = BatchOrder.ParticipantCode;
                            oOrderModel.SegmentFlag = BatchOrder.SegmentFlag;
                            oOrderModel.FreeText3 = BatchOrder.FreeText3;
                            oOrderModel.Filler_c = BatchOrder.Filler_c;
                            oOrderModel.PartitionID = Convert.ToUInt16(CommonFunctions.GetPartitionId(BatchOrder.ScripCode, "BSE", BatchOrder.Segment));//BatchOrder.PartitionID;
                            oOrderModel.MarketSegmentID = CommonFunctions.GetProductId(BatchOrder.ScripCode, "BSE", BatchOrder.Segment);//BatchOrder.MarketSegmentID;
                            oOrderModel.ScripName = BatchOrder.ScripName;
                            //oOrderModel.OrderFomLoadButton = BatchOrder.OrderFomLoadButton;
                            Batch_decimalPoint = CommonFunctions.GetDecimal(Convert.ToInt32(BatchOrder.ScripCode), "BSE", BatchOrder.Segment);
                            oOrderModel.BatchKey = BatchOrder.BatchKey;

                            if (oOrderModel.OrderType == "L")
                            {
                                if (oOrderModel.TriggerPrice == 0)
                                {
                                    oOrderModel.IsOCOOrder = false;
                                }
                                else if (oOrderModel.TriggerPrice > 0)
                                {
                                    oOrderModel.IsOCOOrder = true;
                                }
                            }

                            if (oOrderModel.OrderType == "L")
                            {
                                //    if (oOrderModel.IsOCOOrder)//OCO order
                                //    {
                                //        //oOrderModel.OrderType = Enumerations.Order.OrderTypes.OCO.ToString();
                                //        System.Windows.MessageBox.Show("OCO order placement not allowed");
                                //        return;
                                //    }
                            }
                            //else if (item.OrderType == "G")
                            //    item.OrderType = Enumerations.Order.OrderTypes.MARKET.ToString();
                            //else if (item.OrderType == "K")
                            //    item.OrderType = Enumerations.Order.OrderTypes.BLOCKDEAL.ToString();
                            else if (oOrderModel.OrderType == "P")
                            {
                                if (oOrderModel.TriggerPrice == 0)
                                {
                                    System.Windows.MessageBox.Show("Enter Trigger Price for STOPLOSS");
                                    break;
                                }
                                else
                                {
                                    if (oOrderModel.Price == 0)
                                        oOrderModel.OrderType = "P";//Enumerations.Order.OrderTypes.STOPLOSSMKT.ToString();
                                    else
                                        oOrderModel.OrderType = "P";// Enumerations.Order.OrderTypes.STOPLOSS.ToString();
                                }
                            }

                            //if (item.BuySellIndicator == "B")
                            //    item.BuySellIndicator = "BUY";
                            //else if (item.BuySellIndicator == "S")
                            //    item.BuySellIndicator = "SELL";

                            int Decimal = CommonFunctions.GetDecimal(Convert.ToInt32(oOrderModel.ScripCode), "BSE", oOrderModel.Segment);
                            if (BatchOrder.OrderFomLoadButton)
                            {
                                string Price1 = Convert.ToString(Convert.ToDecimal(oOrderModel.Price / Math.Pow(10, Decimal)));
                                string TriggerPrice1 = Convert.ToString(Convert.ToDecimal(oOrderModel.TriggerPrice / Math.Pow(10, Decimal)));
                                if (Decimal == 2)
                                {
                                    Price = Convert.ToString(Convert.ToDouble(Price1) * Math.Pow(10, Decimal));
                                    Price = String.Format("{0:0.00}", Convert.ToInt32(Price));
                                    TriggerPrice = Convert.ToString(Convert.ToDouble(TriggerPrice1) * Math.Pow(10, Decimal));
                                    TriggerPrice = String.Format("{0:0.00}", Convert.ToInt32(TriggerPrice));
                                    //oOrderModel.Price= System.Convert.ToInt64(BatchOrder.Price);
                                    //oOrderModel.TriggerPrice = System.Convert.ToInt64(BatchOrder.TriggerPrice);
                                }
                                else
                                {
                                    Price = Convert.ToString(Convert.ToDouble(Price1) * Math.Pow(10, Decimal));
                                    Price = String.Format("{0:0.0000}", Convert.ToInt32(Price));
                                    TriggerPrice = Convert.ToString(Convert.ToDouble(TriggerPrice1) * Math.Pow(10, Decimal));
                                    TriggerPrice = String.Format("{0:0.0000}", Convert.ToInt32(TriggerPrice));
                                    //oOrderModel.Price = System.Convert.ToInt64(BatchOrder.Price);
                                    //oOrderModel.TriggerPrice = System.Convert.ToInt64(BatchOrder.TriggerPrice);
                                }
                            }
                            else
                            {
                                if (Decimal == 2)
                                {
                                    Price = String.Format("{0:0.00}", Convert.ToInt32(BatchOrder.Price));
                                    TriggerPrice = String.Format("{0:0.00}", Convert.ToInt32(BatchOrder.TriggerPrice));
                                    //Price = Convert.ToString(Convert.ToDouble(BatchOrder.Price) * Math.Pow(10, Decimal));
                                    //Price = String.Format("{0:0.00}", Convert.ToInt32(Price));
                                    //TriggerPrice = Convert.ToString(Convert.ToDouble(BatchOrder.TriggerPrice) * Math.Pow(10, Decimal));
                                    //TriggerPrice = String.Format("{0:0.00}", Convert.ToInt32(TriggerPrice));
                                    oOrderModel.Price = BatchOrder.Price;//System.Convert.ToInt64(Convert.ToDouble(BatchOrder.Price) / Math.Pow(10, Decimal));
                                    oOrderModel.TriggerPrice = BatchOrder.TriggerPrice;//System.Convert.ToInt64(Convert.ToDouble(BatchOrder.TriggerPrice) * Math.Pow(10, Decimal));
                                }
                                else
                                {
                                    //Price = Convert.ToString(Convert.ToDouble(BatchOrder.Price) * Math.Pow(10, Decimal));
                                    Price = String.Format("{0:0.0000}", Convert.ToInt32(BatchOrder.Price));
                                    //TriggerPrice = Convert.ToString(Convert.ToDouble(BatchOrder.TriggerPrice) * Math.Pow(10, Decimal));
                                    TriggerPrice = String.Format("{0:0.0000}", Convert.ToInt32(BatchOrder.TriggerPrice));
                                    oOrderModel.Price = BatchOrder.Price;// System.Convert.ToInt64(Convert.ToDouble(BatchOrder.Price) * Math.Pow(10, Decimal));
                                    oOrderModel.TriggerPrice = BatchOrder.TriggerPrice;//System.Convert.ToInt64(Convert.ToDouble(BatchOrder.TriggerPrice) * Math.Pow(10, Decimal));
                                }
                            }

                            if (oOrderModel.OrderType == "P" || oOrderModel.OrderType == "G")
                                result = Validations.ValidateOrder(Price, TriggerPrice, oOrderModel, ref Validate_Error, Decimal, true);
                            else
                                result = Validations.ValidateOrder(Price, TriggerPrice, oOrderModel, ref Validate_Error, Decimal);
                            if (result == true)
                            {
                                //MessageBox.Show("Order validated successfully");
                                //Convert Price in Long and send to processing
                                //oOrderModel.Price = Convert.ToInt64(Convert.ToDouble(Price) * Math.Pow(10, Decimal));

                                //if (!string.IsNullOrEmpty(TriggerPrice))
                                //    oOrderModel.TriggerPrice = Convert.ToInt64(Convert.ToDouble(TriggerPrice) * Math.Pow(10, Decimal));

                                //addition of order action field
                                oOrderModel.OrderAction = Enumerations.Order.Modes.A.ToString();
                                if (oOrderModel.OrderRetentionStatus == "EOTODY")
                                {
                                    oOrderModel.OrderRetentionStatus = "EOD";
                                }
                                else if (oOrderModel.OrderRetentionStatus == "EOSESS")
                                {
                                    oOrderModel.OrderRetentionStatus = "EOS";
                                }
                                oOrderModel.IsBatchOrder = true;
                                oOrderRequestProcessor = new OrderRequestProcessor(new AddOrder());
                                oOrderRequestProcessor.ProcessRequest(oOrderModel);
                                batchOrderAutoReset.WaitOne();
                                // System.Windows.MessageBox.Show("1");
                                App.Current.Dispatcher.BeginInvoke((Action)delegate ()
                                {
                                    TotBuyQty = BatchOrderCollection.Where(y => y.BuySellIndicator == "B").Sum<OrderModel>(x => x.OriginalQty).ToString();
                                    BatchOrderTitleCount = "Batch Submission - Orders (Total Count : " + BatchOrderCollection.Count + ")";
                                });

                                //System.Windows.MessageBox.Show("2");
                                TotSellQty = BatchOrderCollection.Where(y => y.BuySellIndicator == "S").Sum<OrderModel>(x => x.OriginalQty).ToString();
                                // System.Windows.MessageBox.Show("3");

                            }
                            //else
                            //    MessageBox.Show("Order validation failed");
                        }
                    });
                }
                catch (Exception ex)
                {
                    ExceptionUtility.LogError(ex);
                }

            }
            else
            {
                //  MessageBox.Show("Select Order before submitting","Invalid Selection",MessageBoxButtons.OK);
                ViewReply = "Select Order before Submitting";
            }
        }

        //private bool Validator(string scripCodeOrId)
        //{
        //    int i = 0;
        //    string Segment_Name = string.Empty;
        //    if (string.IsNullOrEmpty(textScripCodeOrId))
        //    {
        //        txtReply = "INVALID SCRIPID";
        //        return false;
        //    }

        //    bool result = int.TryParse(textScripCodeOrId, out i);

        //    if (result == false)
        //    {
        //        long SCode = CommonFunctions.GetScripCodeFromScripID(textScripCodeOrId);
        //        if (SCode == 0)
        //        {
        //            txtReply = "INVALID SCRIPID";
        //            return false;
        //        }
        //        else
        //        {
        //            Segment_Name = CommonFunctions.GetSegmentID(SCode);
        //            string ScripID = textScripCodeOrId;
        //        }


        //    }
        //    else
        //    {
        //        string ScripID = CommonFunctions.GetScripId(Convert.ToInt64(textScripCodeOrId), "BSE");
        //        if (string.IsNullOrEmpty(ScripID))
        //        {
        //            txtReply = "INVALID SCRIP CODE";
        //            return false;
        //        }
        //        else
        //        {
        //            Segment_Name = CommonFunctions.GetSegmentID(Convert.ToInt64(textScripCodeOrId));
        //            long SCode = CommonFunctions.GetScripCodeFromScripID(ScripID);
        //        }

        //    }


        //    return true;
        //}
    }
}

