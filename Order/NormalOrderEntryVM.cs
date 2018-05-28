using BroadcastReceiver;
using CommonFrontEnd.Common;
using CommonFrontEnd.Global;
using CommonFrontEnd.Model;
using CommonFrontEnd.Model.Order;
using CommonFrontEnd.Model.Trade;
using CommonFrontEnd.Processor.Order;
using CommonFrontEnd.SharedMemories;
using CommonFrontEnd.View;
using CommonFrontEnd.View.Order;
using CommonFrontEnd.ViewModel.Profiling;
using SubscribeList;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static BroadcastReceiver.BroadcastListener;
using static CommonFrontEnd.Common.Enumerations;

namespace CommonFrontEnd.ViewModel.Order
{
    class NormalOrderEntryVM : BaseViewModel
    {
        #region Relay Commands

        private RelayCommand _BuyWindow;
        public RelayCommand BuyWindow
        {
            get { return _BuyWindow ?? (_BuyWindow = new RelayCommand((object e) => BuySellWindow(e))); }
        }

        private RelayCommand _SubmitButton;
        public RelayCommand LimitSubmitButton
        {
            get { return _SubmitButton ?? (_SubmitButton = new RelayCommand((object e) => LimitSubmitButton_Click())); }
        }

        private RelayCommand _RefreshBtnClick;
        public RelayCommand RefreshBtnClick
        {
            get { return _RefreshBtnClick ?? (_RefreshBtnClick = new RelayCommand((object e) => RefreshBtnClick_Click())); }
        }

        private RelayCommand _MarketSubmitButton;

        public RelayCommand MarketSubmitButton
        {
            get { return _MarketSubmitButton ?? (_MarketSubmitButton = new RelayCommand((object e) => MarketSubmitButton_Click())); }
        }


        private RelayCommand _BlockDealSubmitButton;

        public RelayCommand BlockDealSubmitButton
        {
            get { return _BlockDealSubmitButton ?? (_BlockDealSubmitButton = new RelayCommand((object e) => BlockDealSubmitButton_Click())); }
        }


        private RelayCommand _SwiftOrderEntry;

        public RelayCommand SwiftOrderEntry
        {
            get { return _SwiftOrderEntry ?? (_SwiftOrderEntry = new RelayCommand((object e) => SwiftOrderEntry_Window(e))); }
        }


        //private RelayCommand _ScripSelectedSegmentChange;

        //public RelayCommand ScripSelectedSegmentChange
        //{
        //    get { return _ScripSelectedSegmentChange ?? (_ScripSelectedSegmentChange = new RelayCommand((object e) => PopulatingUnderlyingAsset(e))); }
        //}



        private RelayCommand _ScripSegmentSelectionChanged;

        public RelayCommand ScripSegmentSelectionChanged
        {
            get { return _ScripSegmentSelectionChanged ?? (_ScripSegmentSelectionChanged = new RelayCommand((object e) => EnableDisable())); }
        }

        private RelayCommand _InstrTypeSelectionChange;

        public RelayCommand InstrTypeSelectionChange
        {
            get { return _InstrTypeSelectionChange ?? (_InstrTypeSelectionChange = new RelayCommand((object e) => PopulatingUnderlyingAsset(e))); }
        }


        private RelayCommand _UnderAssetSelectionChange;

        public RelayCommand UnderAssetSelectionChange
        {
            get { return _UnderAssetSelectionChange ?? (_UnderAssetSelectionChange = new RelayCommand((object e) => OnChangeOfUnderAsset(e))); }
        }


        private RelayCommand _ExpDateSelectionChange;

        public RelayCommand ExpDateSelectionChange
        {
            get { return _ExpDateSelectionChange ?? (_ExpDateSelectionChange = new RelayCommand((object e) => PopulatingExpDate(e))); }
        }


        private RelayCommand _StkPriceSelectionChange;

        public RelayCommand StkPriceSelectionChange
        {
            get { return _StkPriceSelectionChange ?? (_StkPriceSelectionChange = new RelayCommand((object e) => OnChangeOfStkPrc(e))); }
        }


        private RelayCommand _ScripCodeSelectionChange;

        public RelayCommand ScripCodeSelectionChange
        {
            get { return _ScripCodeSelectionChange ?? (_ScripCodeSelectionChange = new RelayCommand((object e) => OnChangeOfScripCodeSelected(e))); }
        }


        private RelayCommand _ScripIDSelectionChange;

        public RelayCommand ScripIDSelectionChange
        {
            get { return _ScripIDSelectionChange ?? (_ScripIDSelectionChange = new RelayCommand((object e) => OnChangeOfScripIDSelected(e))); }
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


        private RelayCommand _ShortClientIDTextChanged;

        public RelayCommand ShortClientIDTextChanged
        {
            get { return _ShortClientIDTextChanged ?? (_ShortClientIDTextChanged = new RelayCommand((object e) => PopulatingShortClientID_SegmentWise(e))); }
        }


        private RelayCommand _ShortClientIDSelectionChange;

        public RelayCommand ShortClientIDSelectionChange
        {
            get { return _ShortClientIDSelectionChange ?? (_ShortClientIDSelectionChange = new RelayCommand((object e) => OnCheckClientTypeChange())); }
        }

        //Equity Short Client ID Check uncheck event. Added by Gaurav Jadhav. 22/3/2018
        private RelayCommand _ShortClientIDEquityCheckCMD;

        public RelayCommand ShortClientIDEquityCheckCMD
        {
            get { return _ShortClientIDEquityCheckCMD ?? (_ShortClientIDEquityCheckCMD = new RelayCommand((object e) => OnShortClientIDCheckUncheck(e))); }
        }

        //Debt Short Client ID Check uncheck event. Added by Gaurav Jadhav. 22/3/2018
        private RelayCommand _ShortClientIDDebtCheckCMD;

        public RelayCommand ShortClientIDDebtCheckCMD
        {
            get { return _ShortClientIDDebtCheckCMD ?? (_ShortClientIDDebtCheckCMD = new RelayCommand((object e) => OnShortClientIDCheckUncheck(e))); }
        }

        //Derivative Short Client ID Check uncheck event. Added by Gaurav Jadhav. 22/3/2018
        private RelayCommand _ShortClientIDDERCheckCMD;

        public RelayCommand ShortClientIDDERCheckCMD
        {
            get { return _ShortClientIDDERCheckCMD ?? (_ShortClientIDDERCheckCMD = new RelayCommand((object e) => OnShortClientIDCheckUncheck(e))); }
        }

        //Currency Short Client ID Check uncheck event. Added by Gaurav Jadhav. 22/3/2018
        private RelayCommand _ShortClientIDCURCheckCMD;

        public RelayCommand ShortClientIDCURCheckCMD
        {
            get { return _ShortClientIDCURCheckCMD ?? (_ShortClientIDCURCheckCMD = new RelayCommand((object e) => OnShortClientIDCheckUncheck(e))); }
        }
        //private RelayCommand _NormalOrderEntryClosing;

        //public RelayCommand NormalOrderEntryClosing
        //{
        //    get { return _NormalOrderEntryClosing ?? (_NormalOrderEntryClosing = new RelayCommand((object e) => NormalOrderEntryClosing_Closing(e))); }
        //}


        private RelayCommand<TextCompositionEventArgs> _EqFiveLachCalculator;

        public RelayCommand<TextCompositionEventArgs> EqFiveLachCalculator
        {
            get { return _EqFiveLachCalculator ?? (_EqFiveLachCalculator = new RelayCommand<TextCompositionEventArgs>(EqFiveLachCalculatorCheck)); }
        }



        private RelayCommand _QtyLostFocus;

        public RelayCommand QtyLostFocus
        {
            get { return _QtyLostFocus ?? (_QtyLostFocus = new RelayCommand((object e) => QtyLostFocusValidation(e))); }
        }


        private RelayCommand _LoadTabIndex;

        public RelayCommand LoadTabIndex
        {
            get { return _LoadTabIndex ?? (_LoadTabIndex = new RelayCommand((object e) => SetTabindex(e))); }
        }

        private RelayCommand _searchButtonClick;

        public RelayCommand searchButtonClick
        {
            get { return _searchButtonClick ?? (_searchButtonClick = new RelayCommand((object e) => searchButtonClick_execute())); }
        }

        #endregion

        #region properties
        long num;
        string AppendedChar = string.Empty;
        int TimeResetCounter = 0;
        long globalScripcode = 0;
        Stopwatch sw = null;
        Timer TimerCombo = new Timer();
        int ChangeCounter = 0;
        static View.Order.NormalOrderEntry mWindow = null;
        //string globalScripID = string.Empty;

        OrderModel omodel = new OrderModel();
        string ShortIntrType = string.Empty;
        string ScripID = String.Empty;
        string ScripName = String.Empty;

        internal string _BuySellInd;
        internal string BuySellInd
        {
            get { return _BuySellInd; }
            set
            {
                _BuySellInd = value; NotifyPropertyChanged("BuySellInd");

            }
        }
        private bool _isFiveLacSelected = false;

        public bool isFiveLacSelected
        {
            get { return _isFiveLacSelected; }
            set { _isFiveLacSelected = value; NotifyPropertyChanged(nameof(isFiveLacSelected)); }
        }

        private bool scripIdChange = true;
        string Validate_Message = string.Empty;
        private bool flag = true;
        private bool ClientIDFlag = true;
        private bool ChangeShortClient = true;
        private BindingList<string> _ScripSegmentLst;
        public BindingList<string> ScripSegmentLst
        {
            get { return _ScripSegmentLst; }
            set { _ScripSegmentLst = value; NotifyPropertyChanged("ScripSegmentLst"); }
        }

        private List<string> _Exchange;
        public List<string> Exchange
        {
            get { return _Exchange; }
            set
            {
                _Exchange = value;
                NotifyPropertyChanged(nameof(Exchange));
            }
        }

        private List<long> _ScripCodeLst;
        public List<long> ScripCodeLst
        {
            get { return _ScripCodeLst; }
            set { _ScripCodeLst = value; NotifyPropertyChanged("ScripCodeLst"); }
        }

        private List<string> _OrderTypeList;
        public List<string> OrderTypeList
        {
            get { return _OrderTypeList; }
            set
            {
                _OrderTypeList = value;
                NotifyPropertyChanged("OrderTypeList");
            }
        }

        private List<string> _ScripSymLst;
        public List<string> ScripSymLst
        {
            get { return _ScripSymLst; }
            set { _ScripSymLst = value; NotifyPropertyChanged("ScripSymLst"); }
        }

        private List<string> _ScripNameLst;
        public List<string> ScripNameLst
        {
            get { return _ScripNameLst; }
            set { _ScripNameLst = value; NotifyPropertyChanged("ScripNameLst"); }

        }

        private string _Selected_EXCH;
        public string Selected_EXCH
        {
            get { return _Selected_EXCH; }
            set
            {
                _Selected_EXCH = value;
                NotifyPropertyChanged("Selected_EXCH");
                EnableDisable();
                PopulatingScripProfileSegment();
            }
        }

        public static long OrderEntryScripCode;

        private string _ScripSelectedSegment;
        public string ScripSelectedSegment
        {
            get { return _ScripSelectedSegment; }
            set
            {
                _ScripSelectedSegment = value;
                NotifyPropertyChanged("ScripSelectedSegment");
                //EnableDisable();
            }
        }

        private long _ScripSelectedCode;
        public long ScripSelectedCode
        {
            get { return _ScripSelectedCode; }
            set
            {
                OrderEntryScripCode = value;
                CorpActionData = CommonFunctions.GetCorpAction(ScripSelectedCode);
                _ScripSelectedCode = value;
                NotifyPropertyChanged("ScripSelectedCode");
                OnChangeOfScripCode();
                FetchNetPositionByScripCode(value);
                ErrorMsg = CommonFunctions.DisplaySpreadLeg(OrderEntryScripCode);
            }
        }


        private string _ScripNameSelected;
        public string ScripNameSelected
        {
            get { return _ScripNameSelected; }
            set
            {
                _ScripNameSelected = value;
                NotifyPropertyChanged("ScripNameSelected");
                GetFieldData();
            }
        }



        private string _ScripSymSelected;
        public string ScripSymSelected
        {
            get { return _ScripSymSelected; }
            set
            {
                _ScripSymSelected = value;
                NotifyPropertyChanged("ScripSymSelected");
                OnChangeOfScripSym();
            }
        }

        private string _OrderTypeSelected;
        public string OrderTypeSelected
        {
            get { return _OrderTypeSelected; }
            set
            {
                _OrderTypeSelected = value;
                NotifyPropertyChanged("OrderTypeSelected");
                EnableDisable();
            }
        }

        private string _clienttypeselected;
        public string clienttypeselected
        {
            get { return _clienttypeselected; }
            set
            {
                _clienttypeselected = value;
                NotifyPropertyChanged("clienttypeselected");
                OnChangeofClientType();
                EnableDisableClientIdCPCode();
            }
        }

        private string _ClientName;
        public string ClientName
        {
            get { return _ClientName; }
            set
            {
                _ClientName = value;
                NotifyPropertyChanged("ClientName");

            }
        }

        private bool _clientinputEnabled;
        public bool clientinputEnabled
        {
            get { return _clientinputEnabled; }
            set { _clientinputEnabled = value; NotifyPropertyChanged("clientinputEnabled"); }
        }

        private bool _TrgEnabled;
        public bool TrgEnabled
        {
            get { return _TrgEnabled; }
            set { _TrgEnabled = value; NotifyPropertyChanged("TrgEnabled"); }
        }

        private bool _MktPrtEnabled;
        public bool MktPrtEnabled
        {
            get { return _MktPrtEnabled; }
            set { _MktPrtEnabled = value; NotifyPropertyChanged("MktPrtEnabled"); }
        }

        //private string _ShortClientSelected;
        //public string ShortClientSelected
        //{
        //    get { return _ShortClientSelected; }
        //    set
        //    {
        //        _ShortClientSelected = value;
        //        NotifyPropertyChanged("ShortClientSelected");
        //        //OnCheckClientTypeChange();
        //    }
        //}

        private string _RetTypeSelected;
        public string RetTypeSelected
        {
            get { return _RetTypeSelected; }
            set
            {
                _RetTypeSelected = value;
                NotifyPropertyChanged("RetTypeSelected");

            }
        }

        private List<string> _RetType;
        public List<string> RetType
        {
            get { return _RetType; }
            set { _RetType = value; NotifyPropertyChanged("RetType"); }
        }

        private List<string> _Clienttypelst;
        public List<string> Clienttypelst
        {

            get { return _Clienttypelst; }
            set { _Clienttypelst = value; NotifyPropertyChanged("Clienttypelst"); }


        }

        private List<string> _ClientIDinputlst;
        public List<string> ClientIDinputlst
        {

            get { return _ClientIDinputlst; }
            set { _ClientIDinputlst = value; NotifyPropertyChanged("ClientIDinputlst"); }


        }

        private bool _currencyAssetEnable;
        public bool CurrencyAssetEnable
        {
            get { return _currencyAssetEnable; }
            set { _currencyAssetEnable = value; NotifyPropertyChanged("CurrencyAssetEnable"); }
        }

        private string _PerlotVisibilty;
        public string PerlotVisibilty
        {
            get { return _PerlotVisibilty; }
            set { _PerlotVisibilty = value; NotifyPropertyChanged("PerlotVisibilty"); }
        }

        private string _MktLotVisibilty;
        public string MktLotVisibilty
        {
            get { return _MktLotVisibilty; }
            set { _MktLotVisibilty = value; NotifyPropertyChanged("MktLotVisibilty"); }
        }

        private string _FaceValueVisibility;
        public string FaceValueVisibility
        {
            get { return _FaceValueVisibility; }
            set { _FaceValueVisibility = value; NotifyPropertyChanged("FaceValueVisibility"); }
        }

        private string _OpenIntVisibility;
        public string OpenIntVisibility
        {
            get { return _OpenIntVisibility; }
            set { _OpenIntVisibility = value; NotifyPropertyChanged("OpenIntVisibility"); }
        }


        private string _BlockDealVisibility;
        public string BlockDealVisibility
        {
            get { return _BlockDealVisibility; }
            set { _BlockDealVisibility = value; NotifyPropertyChanged("BlockDealVisibility"); }
        }

        private string _OrderSlicingVisibility;
        public string OrderSlicingVisibility
        {
            get { return _OrderSlicingVisibility; }
            set { _OrderSlicingVisibility = value; NotifyPropertyChanged("OrderSlicingVisibility"); }
        }

        private string _BondCalcVisibility;
        public string BondCalcVisibility
        {
            get { return _BondCalcVisibility; }
            set { _BondCalcVisibility = value; NotifyPropertyChanged("BondCalcVisibility"); }
        }

        private string _DervChainVisibility;
        public string DervChainVisibility
        {
            get { return _DervChainVisibility; }
            set { _DervChainVisibility = value; NotifyPropertyChanged("DervChainVisibility"); }
        }


        private string _ClientIdVisibility;

        public string ClientIdVisibility
        {
            get { return _ClientIdVisibility; }
            set { _ClientIdVisibility = value; NotifyPropertyChanged(nameof(ClientIdVisibility)); }
        }



        private string _CPCodeVisibility;

        public string CPCodeVisibility
        {
            get { return _CPCodeVisibility; }
            set { _CPCodeVisibility = value; NotifyPropertyChanged(nameof(CPCodeVisibility)); }
        }

        private string _SellVisible;
        public string SellVisible
        {
            get { return _SellVisible; }
            set { _SellVisible = value; NotifyPropertyChanged("SellVisible"); }
        }

        private string _BuyVisible;
        public string BuyVisible
        {
            get { return _BuyVisible; }
            set { _BuyVisible = value; NotifyPropertyChanged("BuyVisible"); }
        }

        private string _WindowColour;
        public string WindowColour
        {
            get { return _WindowColour; }
            set
            {
                _WindowColour = value;
                NotifyPropertyChanged("WindowColour");
            }
        }

        private string _HeaderTitle;
        public string HeaderTitle
        {
            get { return _HeaderTitle; }
            set
            {
                _HeaderTitle = value;
                NotifyPropertyChanged("HeaderTitle");

            }
        }

        private string _AssetValue;

        public string AssetValue
        {
            get { return _AssetValue; }
            set { _AssetValue = value; NotifyPropertyChanged("AssetValue"); }
        }

        private string _qty;
        public string qty
        {
            get { return _qty; }
            set
            {
                //if (!string.IsNullOrEmpty(value) && Convert.ToInt32(value) % MarketLot == 0 && ScripSelectedSegment != Segment.Currency.ToString() && Convert.ToInt32(value) != 0)
                //{
                //    value = Convert.ToInt32(value).ToString();
                //    _qty = revQty = value;
                //}
                //else if (ScripSelectedSegment?.ToLower() == Enumerations.Order.ScripSegment.Currency.ToString().ToLower() && !string.IsNullOrEmpty(value) && Convert.ToInt32(value) != 0)
                //{
                //    value = Convert.ToInt32(value).ToString();
                //    _qty = revQty = value;
                //}
                //else
                //{
                //    revQty = string.Empty;
                //}
                _qty = value;

                NotifyPropertyChanged("qty");
                if (!string.IsNullOrEmpty(value) && Convert.ToInt32(value) % MarketLot == 0 && ScripSelectedSegment != Segment.Currency.ToString() && Convert.ToInt32(value) != 0)
                {
                    revQty = CommonFunctions.CalculateRevQty(MarketLot, Convert.ToInt64(value)).ToString();
                }
                else if (ScripSelectedSegment?.ToLower() == Enumerations.Order.ScripSegment.Currency.ToString().ToLower() && !string.IsNullOrEmpty(value) && Convert.ToInt32(value) != 0)
                {
                    value = Convert.ToInt32(value).ToString();
                    revQty = CommonFunctions.CalculateRevQty(1, Convert.ToInt64(value)).ToString();
                    //_qty = revQty = value;
                }
                else
                {
                    revQty = string.Empty;
                }
                //OnChangeOfQuantity();
            }
        }


        private string _rate;
        public string rate
        {
            get { return _rate; }
            set
            {
                _rate = value;
                NotifyPropertyChanged("rate");

            }
        }

        private string _revQty;
        public string revQty
        {
            get { return _revQty; }
            set
            {
                _revQty = value;
                NotifyPropertyChanged("revQty");

            }
        }

        private string _trgPrice;
        public string trgPrice
        {
            get { return _trgPrice; }
            set
            {
                _trgPrice = value;
                NotifyPropertyChanged("trgPrice");

            }
        }

        private string _MktPT;
        public string MktPT
        {
            get { return _MktPT; }
            set
            {
                _MktPT = value;
                NotifyPropertyChanged("MktPT");

            }
        }

        private int _MarketLot;
        public int MarketLot
        {
            get { return _MarketLot; }
            set
            {
                _MarketLot = value;
                NotifyPropertyChanged("MarketLot");

            }
        }

        private string _TickSize;
        public string TickSize
        {
            get { return _TickSize; }
            set
            {
                _TickSize = value;
                NotifyPropertyChanged("TickSize");

            }
        }

        private string _Group;
        public string Group
        {
            get { return _Group; }
            set
            {
                _Group = value;
                NotifyPropertyChanged("Group");

            }
        }

        private string _Series;
        public string Series
        {
            get { return _Series; }
            set
            {
                _Series = value;
                NotifyPropertyChanged("Series");

            }
        }

        private long _FaceValue;
        public long FaceValue
        {
            get { return _FaceValue; }
            set
            {
                _FaceValue = value;
                NotifyPropertyChanged("FaceValue");
            }
        }

        private string _Gsm;
        public string Gsm
        {
            get { return _Gsm; }
            set
            {
                _Gsm = value;
                NotifyPropertyChanged("Gsm");

            }
        }

        private string _VarIM;
        public string VarIM
        {
            get { return _VarIM; }
            set
            {
                _VarIM = value;
                NotifyPropertyChanged("VarIM");

            }
        }

        private string _VarEM;
        public string VarEM
        {
            get { return _VarEM; }
            set
            {
                _VarEM = value;
                NotifyPropertyChanged("VarEM");

            }
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

        private string _ErrorMsg;
        public string ErrorMsg
        {
            get { return _ErrorMsg; }
            set
            {
                _ErrorMsg = value;
                NotifyPropertyChanged("ErrorMsg");

            }
        }

        private bool _BuyChecked;
        public bool BuyChecked
        {
            get { return _BuyChecked; }
            set
            {
                _BuyChecked = value;
                NotifyPropertyChanged("BuyChecked");
            }
        }

        private bool _SellChecked;
        public bool SellChecked
        {
            get { return _SellChecked; }
            set
            {
                _SellChecked = value;
                NotifyPropertyChanged("SellChecked");
            }
        }

        private bool _RevChck;
        public bool RevChck
        {
            get { return _RevChck; }
            set
            {
                _RevChck = value;
                NotifyPropertyChanged("RevChck");
                RevQtyEnability();
            }
        }

        private bool _RevQtyEnbl;
        public bool RevQtyEnbl
        {
            get { return _RevQtyEnbl; }
            set
            {
                _RevQtyEnbl = value;
                NotifyPropertyChanged("RevQtyEnbl");
            }
        }

        private bool _TrgRateEnbl;
        public bool TrgRateEnbl
        {
            get { return _TrgRateEnbl; }
            set
            {
                _TrgRateEnbl = value;
                NotifyPropertyChanged("TrgRateEnbl");

            }
        }

        private bool _OcoChecked;
        public bool OcoChecked
        {
            get { return _OcoChecked; }
            set
            {
                _OcoChecked = value;
                NotifyPropertyChanged("OcoChecked");
                OCOChkEnability();
            }
        }

        private bool _MktbtnEnability;
        public bool MktbtnEnability
        {
            get { return _MktbtnEnability; }
            set { _MktbtnEnability = value; NotifyPropertyChanged(nameof(MktbtnEnability)); }
        }

        private bool _BlockDealbtnEnability;
        public bool BlockDealbtnEnability
        {
            get { return _BlockDealbtnEnability; }
            set { _BlockDealbtnEnability = value; NotifyPropertyChanged(nameof(BlockDealbtnEnability)); }
        }

        private string _DerivStackVisi;
        public string DerivStackVisi
        {
            get { return _DerivStackVisi; }
            set { _DerivStackVisi = value; NotifyPropertyChanged("DerivStackVisi"); }
        }

        private string _EQStackVisi;
        public string EQStackVisi
        {
            get { return _EQStackVisi; }
            set { _EQStackVisi = value; NotifyPropertyChanged("EQStackVisi"); }
        }

        private List<string> _IntrTypeLst;
        public List<string> IntrTypeLst
        {
            get { return _IntrTypeLst; }
            set { _IntrTypeLst = value; NotifyPropertyChanged("IntrTypeLst"); }

        }

        private List<string> _CallPutLSt;
        public List<string> CallPutLSt
        {
            get { return _CallPutLSt; }
            set { _CallPutLSt = value; NotifyPropertyChanged("CallPutLSt"); }

        }

        private ObservableCollection<string> _UnderLyingAssetLst;
        public ObservableCollection<string> UnderLyingAssetLst
        {
            get { return _UnderLyingAssetLst; }
            set { _UnderLyingAssetLst = value; NotifyPropertyChanged("UnderLyingAssetLst"); }

        }

        private ObservableCollection<string> _ExpDateLst;
        public ObservableCollection<string> ExpDateLst
        {
            get { return _ExpDateLst; }
            set { _ExpDateLst = value; NotifyPropertyChanged("ExpDateLst"); }

        }

        private ObservableCollection<string> _StkPrcLst;
        public ObservableCollection<string> StkPrcLst
        {
            get { return _StkPrcLst; }
            set { _StkPrcLst = value; NotifyPropertyChanged("StkPrcLst"); }

        }

        private ObservableCollection<string> _ScripIDColl;
        public ObservableCollection<string> ScripIDColl
        {
            get { return _ScripIDColl; }
            set { _ScripIDColl = value; NotifyPropertyChanged("ScripIDColl"); }

        }

        private string _ScripIDSelected;
        public string ScripIDSelected
        {
            get { return _ScripIDSelected; }
            set
            {
                _ScripIDSelected = value;
                NotifyPropertyChanged("ScripIDSelected");
                //ErrorMsg = CommonFunctions.DisplaySpreadLeg(OrderEntryScripCode);
            }
        }

        private string _StkPriceSelected;
        public string StkPriceSelected
        {
            get { return _StkPriceSelected; }
            set
            {
                _StkPriceSelected = value;
                NotifyPropertyChanged("StkPriceSelected");
            }
        }

        private string _IntrTypeSelected;
        public string IntrTypeSelected
        {
            get { return _IntrTypeSelected; }
            set
            {
                _IntrTypeSelected = value;
                NotifyPropertyChanged("IntrTypeSelected");
            }
        }

        private string _SubIntrTypeSelected;
        public string SubIntrTypeSelected
        {
            get { return _SubIntrTypeSelected; }
            set
            {
                _SubIntrTypeSelected = value;
                NotifyPropertyChanged("SubIntrTypeSelected");
            }
        }

        private int _StrategyID;
        public int StrategyID
        {
            get { return _StrategyID; }
            set
            {
                _StrategyID = value;
                NotifyPropertyChanged("StrategyID");
            }
        }

        //private string _CallPutSelected;
        //public string CallPutSelected
        //{
        //    get { return _CallPutSelected; }
        //    set
        //    {
        //        _CallPutSelected = value;
        //        NotifyPropertyChanged("CallPutSelected");
        //        PopulatingExpDate();
        //    }
        //}

        private string _UnderAssetSelected;
        public string UnderAssetSelected
        {
            get { return _UnderAssetSelected; }
            set
            {
                _UnderAssetSelected = value;
                NotifyPropertyChanged("UnderAssetSelected");
            }
        }

        private string _ExpDateSelected;
        public string ExpDateSelected
        {
            get { return _ExpDateSelected; }
            set
            {
                _ExpDateSelected = value;
                NotifyPropertyChanged("ExpDateSelected");
            }
        }

        private bool _StrikePriceEnable;
        public bool StrikePriceEnable
        {
            get { return _StrikePriceEnable; }
            set { _StrikePriceEnable = value; NotifyPropertyChanged("StrikePriceEnable"); }
        }

        private List<long> _ScripCodeList;
        public List<long> ScripCodeList
        {
            get { return _ScripCodeList; }
            set { _ScripCodeList = value; NotifyPropertyChanged("ScripCodeList"); }
        }

        private long _ScripCodeSelected;
        public long ScripCodeSelected
        {
            get { return _ScripCodeSelected; }
            set { OrderEntryScripCode = value; _ScripCodeSelected = value; NotifyPropertyChanged("ScripCodeSelected"); ErrorMsg = CommonFunctions.DisplaySpreadLeg(OrderEntryScripCode); }
        }


        private string _InstrNameSelected;
        public string InstrNameSelected
        {
            get { return _InstrNameSelected; }
            set
            {
                _InstrNameSelected = value;
                NotifyPropertyChanged("InstrNameSelected");
                GetFieldData();
            }
        }

        private List<long> _Token;
        public List<long> Token
        {
            get { return _Token; }
            set { _Token = value; NotifyPropertyChanged("Token"); }
        }

        private string _TouchlineValue;
        public string TouchlineValue
        {
            get { return _TouchlineValue; }
            set { _TouchlineValue = value; NotifyPropertyChanged(nameof(TouchlineValue)); }
        }

        private static NormalOrderEntryVM _getinstance;
        public static NormalOrderEntryVM GetInstance
        {
            get
            {
                if (_getinstance == null)
                {
                    _getinstance = new NormalOrderEntryVM();
                }
                return _getinstance;
            }
        }

        //private string _ShortClientIDSelectionCheck;

        //public string ShortClientIDSelectionCheck
        //{
        //    get { return _ShortClientIDSelectionCheck; }
        //    set { _ShortClientIDSelectionCheck = value; NotifyPropertyChanged(nameof(ShortClientIDSelectionCheck)); }
        //}


        //Equity Short Client ID CheckBox Checked
        private string _ShortClientIDEquityCheck;

        public string ShortClientIDEquityCheck
        {
            get { return _ShortClientIDEquityCheck; }
            set { _ShortClientIDEquityCheck = value; NotifyPropertyChanged(nameof(ShortClientIDEquityCheck)); }
        }

        //Debt Short Client ID CheckBox Checked
        private string _ShortClientIDDebtCheck;

        public string ShortClientIDDebtCheck
        {
            get { return _ShortClientIDDebtCheck; }
            set { _ShortClientIDDebtCheck = value; NotifyPropertyChanged(nameof(ShortClientIDDebtCheck)); }
        }

        //Derivative Short Client ID CheckBox Checked
        private string _ShortClientIDDerCheck;

        public string ShortClientIDDerCheck
        {
            get { return _ShortClientIDDerCheck; }
            set { _ShortClientIDDerCheck = value; NotifyPropertyChanged(nameof(ShortClientIDDerCheck)); }
        }

        //Currency Short Client ID CheckBox Checked
        private string _ShortClientIDCurCheck;

        public string ShortClientIDCurCheck
        {
            get { return _ShortClientIDCurCheck; }
            set { _ShortClientIDCurCheck = value; NotifyPropertyChanged(nameof(ShortClientIDCurCheck)); }
        }
        private string _ShortClientSelectedText;
        public string ShortClientSelectedText
        {
            get { return _ShortClientSelectedText; }
            set
            {
                _ShortClientSelectedText = value;
                NotifyPropertyChanged("ShortClientSelectedText");
                OnChangeofShortClientTxt();
                AssignShortClientTxtChange();
            }
        }


        public bool ChangedFromScripCode = false;


        private string _isEQvisible;

        public string IsEQvisible
        {
            get { return _isEQvisible; }
            set { _isEQvisible = value; NotifyPropertyChanged("IsEQvisible"); }
        }

        private string _isDERvisible;

        public string IsDERvisible
        {
            get { return _isDERvisible; }
            set { _isDERvisible = value; NotifyPropertyChanged("IsDERvisible"); }
        }

        private string _isDebtvisible;

        public string IsDebtvisible
        {
            get { return _isDebtvisible; }
            set { _isDebtvisible = value; NotifyPropertyChanged("IsDebtvisible"); }
        }

        private string _isCurrencyvisible;

        public string IsCurrencyvisible
        {
            get { return _isCurrencyvisible; }
            set { _isCurrencyvisible = value; NotifyPropertyChanged("IsCurrencyvisible"); }
        }

        private string _NetPositionNetQty;

        public string NetPositionNetQty
        {
            get { return _NetPositionNetQty; }
            set { _NetPositionNetQty = value; NotifyPropertyChanged("NetPositionNetQty"); }
        }

        private string _NetPositionNetValue;

        public string NetPositionNetValue
        {
            get { return _NetPositionNetValue; }
            set { _NetPositionNetValue = value; NotifyPropertyChanged("NetPositionNetValue"); }
        }

        private string _BuyPosition;

        public string BuyPosition
        {
            get { return _BuyPosition; }
            set { _BuyPosition = value; NotifyPropertyChanged("BuyPosition"); }
        }

        private string _SellPosition;

        public string SellPosition
        {
            get { return _SellPosition; }
            set { _SellPosition = value; NotifyPropertyChanged("SellPosition"); }
        }

        private string _AvgBuyPosition;

        public string AvgBuyPosition
        {
            get { return _AvgBuyPosition; }
            set { _AvgBuyPosition = value; NotifyPropertyChanged("AvgBuyPosition"); }
        }

        private string _AvgSellPosition;

        public string AvgSellPosition
        {
            get { return _AvgSellPosition; }
            set { _AvgSellPosition = value; NotifyPropertyChanged("AvgSellPosition"); }
        }

        private string _DefaultFocusControlName;

        public string DefaultFocusControlName
        {
            get { return _DefaultFocusControlName; }
            set { _DefaultFocusControlName = value; NotifyPropertyChanged("DefaultFocusControlName"); }
        }

        private string _CorpActionData;

        public string CorpActionData
        {
            get { return _CorpActionData; }
            set { _CorpActionData = value; NotifyPropertyChanged("CorpActionData"); }
        }

        private string _InstrumentName;

        public string InstrumentName
        {
            get { return _InstrumentName; }
            set { _InstrumentName = value; NotifyPropertyChanged("InstrumentName"); }
        }

        private bool _IsCurDerScripCodeFocus;

        public bool IsCurDerScripCodeFocus
        {
            get { return _IsCurDerScripCodeFocus; }
            set { _IsCurDerScripCodeFocus = value; NotifyPropertyChanged("IsCurDerScripCodeFocus"); }
        }

        private bool _IsEquityDebtScripCodeFocus;

        public bool IsEquityDebtScripCodeFocus
        {
            get { return _IsEquityDebtScripCodeFocus; }
            set { _IsEquityDebtScripCodeFocus = value; NotifyPropertyChanged("IsEquityDebtScripCodeFocus"); }
        }

        private bool _IsCurDerScripIdFocus;

        public bool IsCurDerScripIdFocus
        {
            get { return _IsCurDerScripIdFocus; }
            set { _IsCurDerScripIdFocus = value; NotifyPropertyChanged("IsCurDerScripIdFocus"); }
        }

        private bool _IsEquityDebtScripIdFocus;

        public bool IsEquityDebtScripIdFocus
        {
            get { return _IsEquityDebtScripIdFocus; }
            set { _IsEquityDebtScripIdFocus = value; NotifyPropertyChanged("IsEquityDebtScripIdFocus"); }
        }

        private bool _IsOrderRateFocus;

        public bool IsOrderRateFocus
        {
            get { return _IsOrderRateFocus; }
            set { _IsOrderRateFocus = value; NotifyPropertyChanged("IsOrderRateFocus"); }
        }

        private bool _IsOrderQtyFocus;

        public bool IsOrderQtyFocus
        {
            get { return _IsOrderQtyFocus; }
            set { _IsOrderQtyFocus = value; NotifyPropertyChanged("IsOrderQtyFocus"); }
        }

        private bool _IsClientIDCpCdReadOnly = true;

        public bool IsClientIDCpCdEnabled
        {
            get { return _IsClientIDCpCdReadOnly; }
            set { _IsClientIDCpCdReadOnly = value; NotifyPropertyChanged("IsClientIDCpCdEnabled"); }
        }

        private long _AssetTokenNumber;

        public long AssetTokenNumber
        {
            get { return _AssetTokenNumber; }
            set { _AssetTokenNumber = value; }
        }

        #endregion

        #region Action Events

        public static Action<Model.ScripDetails> OETouchlineValue;
        //public static Action<VarMain> OEVarValues;

        #endregion

        #region Constructor
        public NormalOrderEntryVM()
        {
            if (MemoryManager.InvokeWindowOnScripCodeSelection == null)
            {
                MemoryManager.InvokeWindowOnScripCodeSelection += PopulateOrderEntryWindow;
            }

            ScripSegmentLst = new BindingList<string>();
            IntrTypeLst = new List<string>();

            UnderLyingAssetLst = new ObservableCollection<string>();
            ExpDateLst = new ObservableCollection<string>();
            ScripIDColl = new ObservableCollection<string>();
            ScripCodeList = new List<long>();
            MktPT = UtilityOrderDetails.GETInstance.MktProtection;
            Exchange = new List<string>();
            OrderTypeList = new List<string>();
            OnChangeOfExchange();
            PopulatingScripProfileSegment();
            PopulateScripCode();
            PopulatingScripName();
            PopulatingScripSym();
            GetFieldData();
            PopulateOrderType();
            PopulateRetType();
            PopulateClientIdInput();
            PopulateCLientType();
            EnableDisable();
            OcoChecked = false;
            OETouchlineValue += UpdateTouchlineValue;
            OrderProcessor.OnOrderResponse += NormalOrderReceived;

            mWindow = System.Windows.Application.Current.Windows.OfType<View.Order.NormalOrderEntry>().FirstOrDefault();

            if (OrderEntryUC_VM.OnSelectedScripCodeChange == null)
                OrderEntryUC_VM.OnSelectedScripCodeChange += UpdateScripDetails;
            if (Processor.UMSProcessor.IntimateOrderEntryNetPosition == null)
            {
                Processor.UMSProcessor.IntimateOrderEntryNetPosition += FetchNetPositionByScripCode;
            }

            ClientProfilingVM.OnClientProfilingChange += PopulateClientIdInputInstant;
            if (ClientProfilingVM.OnClientProfilingChange != null)//call only once. Gaurav Jadhav 25/4/2018
            {
                ClientProfilingVM.OnClientProfilingChange?.Invoke();
            }
            OrderProfilingVM.OnChangeOfMarketProtection += delegate (string MarketProt) { MktPT = MarketProt; };
            OrderProfilingVM.OnChangeOf5LCheck += Toggle5LCheck;
            OrderProfilingVM.OnChangeOfTouchlineDataonOE += ToggleTouchlineData;
            TimerCombo = new Timer();
            TimerCombo.Elapsed += TimerCombo_Elapsed;
            TimerCombo.Interval = 2000;
            AssignDefaultFocusStart(null);
            MainWindowVM.indicesBroadCast += objIndicesBroadCastProcessor_OnBroadCastRecievedNewBow;

        }

        private void ToggleTouchlineData(string obj)
        {
            UpdateTouchlineValue(null);
        }

        private void Toggle5LCheck(bool obj)
        {
            isFiveLacSelected = UtilityOrderDetails.GETInstance.Default5LChecked;
        }

        internal void objIndicesBroadCastProcessor_OnBroadCastRecievedNewBow(CommonFrontEnd.Model.IdicesDetailsMain obj)
        {
            try
            {
                AssetTokenNumber = CommonFunctions.GetAssetTokenNumber(ScripCodeSelected, ScripSelectedSegment, IntrTypeSelected);
                if (ScripSelectedSegment == "Derivative")
                {
                    if (obj.IndexCode == AssetTokenNumber)
                    {
                        AssetValue = CommonFunctions.GetLTPBCast(Convert.ToInt32(AssetTokenNumber), IntrTypeSelected);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
            }
        }
        /// <summary>
        /// Only Visible controls
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="depObj"></param>
        /// <returns></returns>
        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < System.Windows.Media.VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = System.Windows.Media.VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }


        #endregion
        public void AssignDefaultFocusStart(object State)
        {
            if (State == null)//should execute code if state is null. dont execute in Buy/Sell state 
            {
                //default focus as per OE Settings- Gaurav Jadhav 25/4/2018
                if (!string.IsNullOrEmpty(UtilityOrderDetails.GETInstance.DefaultFocusOESettings))
                {
                    DefaultFocusControlName = UtilityOrderDetails.GETInstance.DefaultFocusOESettings;
                }
                else
                {
                    DefaultFocusControlName = "ScripId";
                }
                if (ScripSelectedSegment == "Equity")
                {
                    if (DefaultFocusControlName == "ScripId")
                    {

                        mWindow?.comboBox.Focus();
                        //mWindow?.comboBox.IsSelectionBoxHighlighted = true;
                        IsEquityDebtScripIdFocus = true;
                    }
                    else if (DefaultFocusControlName == "ScripCode")
                    {
                        mWindow?.comboBox1.Focus();
                        IsEquityDebtScripCodeFocus = true;
                    }
                    else if (DefaultFocusControlName == "OrderQty")
                    {
                        mWindow?.TxtTotalLot.Focus();
                        IsOrderQtyFocus = true;
                    }
                    else if (DefaultFocusControlName == "Rate")
                    {
                        mWindow?.TxtRate.Focus();
                        IsOrderRateFocus = true;
                    }
                }
                else if (ScripSelectedSegment == "Debt")
                {
                    if (DefaultFocusControlName == "ScripId")
                    {
                        mWindow?.comboBox.Focus();
                        IsEquityDebtScripIdFocus = true;
                    }
                    else if (DefaultFocusControlName == "ScripCode")
                    {
                        mWindow?.comboBox1.Focus();
                        IsEquityDebtScripCodeFocus = true;
                    }
                    else if (DefaultFocusControlName == "OrderQty")
                    {
                        mWindow?.TxtTotalLot.Focus();
                        IsOrderQtyFocus = true;
                    }
                    else if (DefaultFocusControlName == "Rate")
                    {
                        mWindow?.TxtRate.Focus();
                        IsOrderRateFocus = true;
                    }
                }
                else if (ScripSelectedSegment == "Derivative")
                {
                    if (DefaultFocusControlName == "ScripId")
                    {
                        mWindow?.DerivInstr.Focus();
                        IsCurDerScripIdFocus = true;
                    }
                    else if (DefaultFocusControlName == "ScripCode")
                    {
                        mWindow?.derivcode.Focus();
                        IsCurDerScripCodeFocus = true;
                    }
                    else if (DefaultFocusControlName == "OrderQty")
                    {
                        mWindow?.TxtTotalLot.Focus();
                        IsOrderQtyFocus = true;
                    }
                    else if (DefaultFocusControlName == "Rate")
                    {
                        mWindow?.TxtRate.Focus();
                        IsOrderRateFocus = true;
                    }
                }
                else if (ScripSelectedSegment == "Currency")
                {
                    if (DefaultFocusControlName == "ScripId")
                    {
                        mWindow?.DerivInstr.Focus();
                        IsCurDerScripIdFocus = true;
                    }
                    else if (DefaultFocusControlName == "ScripCode")
                    {
                        mWindow?.derivcode.Focus();
                        IsCurDerScripCodeFocus = true;
                    }
                    else if (DefaultFocusControlName == "OrderQty")
                    {
                        mWindow?.TxtTotalLot.Focus();
                        IsOrderQtyFocus = true;
                    }
                    else if (DefaultFocusControlName == "Rate")
                    {
                        mWindow?.TxtRate.Focus();
                        IsOrderRateFocus = true;
                    }
                }
                else//eq scrip id default
                {
                    if (DefaultFocusControlName == "ScripId")
                    {
                        mWindow?.comboBox.Focus();
                        IsEquityDebtScripIdFocus = true;
                    }
                }
            }
        }

        private void SetTabindex(object e)
        {

            mWindow = System.Windows.Application.Current.Windows.OfType<View.Order.NormalOrderEntry>().FirstOrDefault();

            //Equity
            #region EquityTabIndex
            if (ScripSelectedSegment == "Equity")
            {
                foreach (ComboBox cb in FindVisualChildren<ComboBox>(mWindow))
                {
                    if (cb.Name == "comboBox")
                    {
                        cb.IsTabStop = true;
                        cb.TabIndex = 1;
                    }
                    if (cb.Name == "comboBox1")
                    {
                        cb.IsTabStop = true;
                        cb.TabIndex = 2;
                    }
                    if (cb.Name == "Ret")
                    {
                        cb.IsTabStop = true;
                        cb.TabIndex = 9;
                    }
                    if (cb.Name == "drpdwnShortClient")
                    {
                        cb.IsTabStop = true;
                        cb.TabIndex = 11;
                    }
                    if (cb.Name == "drpdwnClientType")
                    {
                        cb.IsTabStop = true;
                        cb.TabIndex = 12;
                    }
                    if (cb.Name == "ddlSegmentType")
                    {
                        cb.IsTabStop = true;
                        cb.TabIndex = 24;
                    }
                    //ddlSegmentType
                    //drpdwnClientType
                    //comboBox1
                    // do something with tb here
                }
                foreach (CheckBox checkbox in FindVisualChildren<CheckBox>(mWindow))
                {
                    if (checkbox.Name == "cbEquityShortClientId")
                    {
                        checkbox.IsTabStop = true;
                        checkbox.TabIndex = 10;
                    }
                    if (checkbox.Name == "ocoCheck")
                    {
                        checkbox.IsTabStop = true;
                        checkbox.TabIndex = 14;
                    }
                    if (checkbox.Name == "ChkEnblRevQty")
                    {
                        checkbox.IsTabStop = true;
                        checkbox.TabIndex = 21;
                    }
                    if (checkbox.Name == "chkBox5L")
                    {
                        checkbox.IsTabStop = true;
                        checkbox.TabIndex = 22;
                    }
                    //chkBox5L
                }
                foreach (GroupBox gb in FindVisualChildren<GroupBox>(mWindow))
                {
                    if (gb.Name == "groupBox1")
                    {
                        gb.IsTabStop = true;
                        gb.TabIndex = 3;
                    }
                }
                foreach (RadioButton rb in FindVisualChildren<RadioButton>(mWindow))
                {
                    if (rb.Name == "rdoBuy")
                    {
                        rb.IsTabStop = true;
                        rb.TabIndex = 4;
                    }
                    if (rb.Name == "rdoSell")
                    {
                        rb.IsTabStop = true;
                        rb.TabIndex = 5;
                    }
                }
                foreach (ClickSelectTextBox oClickselectTextBox in FindVisualChildren<ClickSelectTextBox>(mWindow))
                {
                    if (oClickselectTextBox.Name == "TxtTotalLot")
                    {
                        oClickselectTextBox.IsTabStop = true;
                        oClickselectTextBox.TabIndex = 6;
                    }
                    if (oClickselectTextBox.Name == "TxtRevealedQty")
                    {
                        oClickselectTextBox.IsTabStop = true;
                        oClickselectTextBox.TabIndex = 7;
                    }
                    if (oClickselectTextBox.Name == "TxtRate")
                    {
                        oClickselectTextBox.IsTabStop = true;
                        oClickselectTextBox.TabIndex = 8;
                    }
                    if (oClickselectTextBox.Name == "textBox2")
                    {
                        oClickselectTextBox.IsTabStop = true;
                        oClickselectTextBox.TabIndex = 13;
                    }
                    if (oClickselectTextBox.Name == "txtOCOTriggerRate")
                    {
                        oClickselectTextBox.IsTabStop = true;
                        oClickselectTextBox.TabIndex = 15;
                    }
                    if (oClickselectTextBox.Name == "TxtMktProfitPercentage")
                    {
                        oClickselectTextBox.IsTabStop = true;
                        oClickselectTextBox.TabIndex = 20;
                    }

                }
                foreach (Button btn in FindVisualChildren<Button>(mWindow))
                {
                    if (btn.Name == "btnRefresh")
                    {
                        btn.IsTabStop = true;
                        btn.TabIndex = 16;
                    }
                    if (btn.Name == "btnLimit")
                    {
                        btn.IsTabStop = true;
                        btn.TabIndex = 17;
                    }
                    if (btn.Name == "BtnMarket")
                    {
                        btn.IsTabStop = true;
                        btn.TabIndex = 18;
                    }
                    if (btn.Name == "BtnBlockDeal")
                    {
                        btn.IsTabStop = true;
                        btn.TabIndex = 19;
                    }
                    //btnSearchCorpAction
                    if (btn.Name == "btnSearchCorpAction")
                    {
                        btn.IsTabStop = true;
                        btn.TabIndex = 23;
                    }
                    if (btn.Name == "OrderSliceCalc")
                    {
                        btn.IsTabStop = true;
                        btn.TabIndex = 25;
                    }
                    //OrderSliceCalc

                }
                //ClickSelectTextBox

            }
            #endregion
            #region Derivative
            else if (ScripSelectedSegment == "Derivative")
            {
                foreach (ComboBox cb in FindVisualChildren<ComboBox>(mWindow))
                {
                    if (cb.Name == "DerivInstr")
                    {
                        cb.IsTabStop = true;
                        cb.TabIndex = 1;
                    }
                    if (cb.Name == "derivcode")
                    {
                        cb.IsTabStop = true;
                        cb.TabIndex = 2;
                    }
                    if (cb.Name == "Ret")
                    {
                        cb.IsTabStop = true;
                        cb.TabIndex = 9;
                    }
                    if (cb.Name == "drpdwnShortClient")
                    {
                        cb.IsTabStop = true;
                        cb.TabIndex = 11;
                    }
                    if (cb.Name == "drpdwnClientType")
                    {
                        cb.IsTabStop = true;
                        cb.TabIndex = 12;
                    }
                    if (cb.Name == "ddlSegmentType")
                    {
                        cb.IsTabStop = true;
                        cb.TabIndex = 24;
                    }
                    if (cb.Name == "comboBox2")
                    {
                        cb.IsTabStop = true;
                        cb.TabIndex = 25;
                    }
                    if (cb.Name == "comboBox3")
                    {
                        cb.IsTabStop = true;
                        cb.TabIndex = 26;
                    }
                    if (cb.Name == "comboBox4")
                    {
                        cb.IsTabStop = true;
                        cb.TabIndex = 27;
                    }
                    if (cb.Name == "comboBox5")
                    {
                        cb.IsTabStop = true;
                        cb.TabIndex = 28;
                    }
                }
                foreach (CheckBox checkbox in FindVisualChildren<CheckBox>(mWindow))
                {
                    if (checkbox.Name == "cbDerShortClientId")
                    {
                        checkbox.IsTabStop = true;
                        checkbox.TabIndex = 10;
                    }
                    if (checkbox.Name == "ocoCheck")
                    {
                        checkbox.IsTabStop = true;
                        checkbox.TabIndex = 14;
                    }
                    if (checkbox.Name == "ChkEnblRevQty")
                    {
                        checkbox.IsTabStop = true;
                        checkbox.TabIndex = 21;
                    }
                    if (checkbox.Name == "chkBox5L")
                    {
                        checkbox.IsTabStop = true;
                        checkbox.TabIndex = 22;
                    }
                    //chkBox5L
                }
                foreach (GroupBox gb in FindVisualChildren<GroupBox>(mWindow))
                {
                    if (gb.Name == "groupBox1")
                    {
                        gb.IsTabStop = true;
                        gb.TabIndex = 3;
                    }
                }
                foreach (RadioButton rb in FindVisualChildren<RadioButton>(mWindow))
                {
                    if (rb.Name == "rdoBuy")
                    {
                        rb.IsTabStop = true;
                        rb.TabIndex = 4;
                    }
                    if (rb.Name == "rdoSell")
                    {
                        rb.IsTabStop = true;
                        rb.TabIndex = 5;
                    }
                }
                foreach (ClickSelectTextBox oClickselectTextBox in FindVisualChildren<ClickSelectTextBox>(mWindow))
                {
                    if (oClickselectTextBox.Name == "TxtTotalLot")
                    {
                        oClickselectTextBox.IsTabStop = true;
                        oClickselectTextBox.TabIndex = 6;
                    }
                    if (oClickselectTextBox.Name == "TxtRevealedQty")
                    {
                        oClickselectTextBox.IsTabStop = true;
                        oClickselectTextBox.TabIndex = 7;
                    }
                    if (oClickselectTextBox.Name == "TxtRate")
                    {
                        oClickselectTextBox.IsTabStop = true;
                        oClickselectTextBox.TabIndex = 8;
                    }
                    if (oClickselectTextBox.Name == "textBox2")
                    {
                        oClickselectTextBox.IsTabStop = true;
                        oClickselectTextBox.TabIndex = 13;
                    }
                    if (oClickselectTextBox.Name == "txtOCOTriggerRate")
                    {
                        oClickselectTextBox.IsTabStop = true;
                        oClickselectTextBox.TabIndex = 15;
                    }
                    if (oClickselectTextBox.Name == "TxtMktProfitPercentage")
                    {
                        oClickselectTextBox.IsTabStop = true;
                        oClickselectTextBox.TabIndex = 20;
                    }

                }
                foreach (Button btn in FindVisualChildren<Button>(mWindow))
                {
                    if (btn.Name == "btnRefresh")
                    {
                        btn.IsTabStop = true;
                        btn.TabIndex = 16;
                    }
                    if (btn.Name == "btnLimit")
                    {
                        btn.IsTabStop = true;
                        btn.TabIndex = 17;
                    }
                    if (btn.Name == "BtnMarket")
                    {
                        btn.IsTabStop = true;
                        btn.TabIndex = 18;
                    }
                    if (btn.Name == "DerChain")
                    {
                        btn.IsTabStop = true;
                        btn.TabIndex = 19;
                    }
                    //btnSearchCorpAction
                    if (btn.Name == "btnSearchCorpAction")
                    {
                        btn.IsTabStop = true;
                        btn.TabIndex = 23;
                    }
                }
            }
            #endregion
            #region Currency
            else if (ScripSelectedSegment == "Currency")
            {
                foreach (ComboBox cb in FindVisualChildren<ComboBox>(mWindow))
                {
                    if (cb.Name == "DerivInstr")
                    {
                        cb.IsTabStop = true;
                        cb.TabIndex = 1;
                    }
                    if (cb.Name == "derivcode")
                    {
                        cb.IsTabStop = true;
                        cb.TabIndex = 2;
                    }
                    if (cb.Name == "Ret")
                    {
                        cb.IsTabStop = true;
                        cb.TabIndex = 9;
                    }
                    if (cb.Name == "drpdwnShortClient")
                    {
                        cb.IsTabStop = true;
                        cb.TabIndex = 11;
                    }
                    if (cb.Name == "drpdwnClientType")
                    {
                        cb.IsTabStop = true;
                        cb.TabIndex = 12;
                    }
                    if (cb.Name == "ddlSegmentType")
                    {
                        cb.IsTabStop = true;
                        cb.TabIndex = 23;
                    }
                    if (cb.Name == "comboBox2")
                    {
                        cb.IsTabStop = true;
                        cb.TabIndex = 24;
                    }
                    if (cb.Name == "comboBox3")
                    {
                        cb.IsTabStop = true;
                        cb.TabIndex = 25;
                    }
                    if (cb.Name == "comboBox4")
                    {
                        cb.IsTabStop = true;
                        cb.TabIndex = 26;
                    }
                    if (cb.Name == "comboBox5")
                    {
                        cb.IsTabStop = true;
                        cb.TabIndex = 27;
                    }
                }
                foreach (CheckBox checkbox in FindVisualChildren<CheckBox>(mWindow))
                {
                    if (checkbox.Name == "cbCurShortClientId")
                    {
                        checkbox.IsTabStop = true;
                        checkbox.TabIndex = 10;
                    }
                    if (checkbox.Name == "ocoCheck")
                    {
                        checkbox.IsTabStop = true;
                        checkbox.TabIndex = 14;
                    }
                    if (checkbox.Name == "ChkEnblRevQty")
                    {
                        checkbox.IsTabStop = true;
                        checkbox.TabIndex = 20;
                    }
                    if (checkbox.Name == "chkBox5L")
                    {
                        checkbox.IsTabStop = true;
                        checkbox.TabIndex = 21;
                    }
                    //chkBox5L
                }
                foreach (GroupBox gb in FindVisualChildren<GroupBox>(mWindow))
                {
                    if (gb.Name == "groupBox1")
                    {
                        gb.IsTabStop = true;
                        gb.TabIndex = 3;
                    }
                }
                foreach (RadioButton rb in FindVisualChildren<RadioButton>(mWindow))
                {
                    if (rb.Name == "rdoBuy")
                    {
                        rb.IsTabStop = true;
                        rb.TabIndex = 4;
                    }
                    if (rb.Name == "rdoSell")
                    {
                        rb.IsTabStop = true;
                        rb.TabIndex = 5;
                    }
                }
                foreach (ClickSelectTextBox oClickselectTextBox in FindVisualChildren<ClickSelectTextBox>(mWindow))
                {
                    if (oClickselectTextBox.Name == "TxtTotalLot")
                    {
                        oClickselectTextBox.IsTabStop = true;
                        oClickselectTextBox.TabIndex = 6;
                    }
                    if (oClickselectTextBox.Name == "TxtRevealedQty")
                    {
                        oClickselectTextBox.IsTabStop = true;
                        oClickselectTextBox.TabIndex = 7;
                    }
                    if (oClickselectTextBox.Name == "TxtRate")
                    {
                        oClickselectTextBox.IsTabStop = true;
                        oClickselectTextBox.TabIndex = 8;
                    }
                    if (oClickselectTextBox.Name == "textBox2")
                    {
                        oClickselectTextBox.IsTabStop = true;
                        oClickselectTextBox.TabIndex = 13;
                    }
                    if (oClickselectTextBox.Name == "txtOCOTriggerRate")
                    {
                        oClickselectTextBox.IsTabStop = true;
                        oClickselectTextBox.TabIndex = 15;
                    }
                    if (oClickselectTextBox.Name == "TxtMktProfitPercentage")
                    {
                        oClickselectTextBox.IsTabStop = true;
                        oClickselectTextBox.TabIndex = 19;
                    }

                }
                foreach (Button btn in FindVisualChildren<Button>(mWindow))
                {
                    if (btn.Name == "btnRefresh")
                    {
                        btn.IsTabStop = true;
                        btn.TabIndex = 16;
                    }
                    if (btn.Name == "btnLimit")
                    {
                        btn.IsTabStop = true;
                        btn.TabIndex = 17;
                    }
                    if (btn.Name == "BtnMarket")
                    {
                        btn.IsTabStop = true;
                        btn.TabIndex = 18;
                    }

                    //btnSearchCorpAction
                    if (btn.Name == "btnSearchCorpAction")
                    {
                        btn.IsTabStop = true;
                        btn.TabIndex = 22;
                    }
                }
            }
            #endregion
            else if (ScripSelectedSegment == "Debt")
            {
                foreach (ComboBox cb in FindVisualChildren<ComboBox>(mWindow))
                {
                    if (cb.Name == "comboBox")
                    {
                        cb.IsTabStop = true;
                        cb.TabIndex = 1;
                    }
                    if (cb.Name == "comboBox1")
                    {
                        cb.IsTabStop = true;
                        cb.TabIndex = 2;
                    }
                    if (cb.Name == "Ret")
                    {
                        cb.IsTabStop = true;
                        cb.TabIndex = 9;
                    }
                    if (cb.Name == "drpdwnShortClient")
                    {
                        cb.IsTabStop = true;
                        cb.TabIndex = 11;
                    }
                    if (cb.Name == "drpdwnClientType")
                    {
                        cb.IsTabStop = true;
                        cb.TabIndex = 12;
                    }
                    if (cb.Name == "ddlSegmentType")
                    {
                        cb.IsTabStop = true;
                        cb.TabIndex = 24;
                    }
                    //ddlSegmentType
                    //drpdwnClientType
                    //comboBox1
                    // do something with tb here
                }
                foreach (CheckBox checkbox in FindVisualChildren<CheckBox>(mWindow))
                {
                    if (checkbox.Name == "cbDebtShortClientId")
                    {
                        checkbox.IsTabStop = true;
                        checkbox.TabIndex = 10;
                    }
                    if (checkbox.Name == "ocoCheck")
                    {
                        checkbox.IsTabStop = true;
                        checkbox.TabIndex = 14;
                    }
                    if (checkbox.Name == "ChkEnblRevQty")
                    {
                        checkbox.IsTabStop = true;
                        checkbox.TabIndex = 21;
                    }
                    if (checkbox.Name == "chkBox5L")
                    {
                        checkbox.IsTabStop = true;
                        checkbox.TabIndex = 22;
                    }
                    //chkBox5L
                }
                foreach (GroupBox gb in FindVisualChildren<GroupBox>(mWindow))
                {
                    if (gb.Name == "groupBox1")
                    {
                        gb.IsTabStop = true;
                        gb.TabIndex = 3;
                    }
                }
                foreach (RadioButton rb in FindVisualChildren<RadioButton>(mWindow))
                {
                    if (rb.Name == "rdoBuy")
                    {
                        rb.IsTabStop = true;
                        rb.TabIndex = 4;
                    }
                    if (rb.Name == "rdoSell")
                    {
                        rb.IsTabStop = true;
                        rb.TabIndex = 5;
                    }
                }
                foreach (ClickSelectTextBox oClickselectTextBox in FindVisualChildren<ClickSelectTextBox>(mWindow))
                {
                    if (oClickselectTextBox.Name == "TxtTotalLot")
                    {
                        oClickselectTextBox.IsTabStop = true;
                        oClickselectTextBox.TabIndex = 6;
                    }
                    if (oClickselectTextBox.Name == "TxtRevealedQty")
                    {
                        oClickselectTextBox.IsTabStop = true;
                        oClickselectTextBox.TabIndex = 7;
                    }
                    if (oClickselectTextBox.Name == "TxtRate")
                    {
                        oClickselectTextBox.IsTabStop = true;
                        oClickselectTextBox.TabIndex = 8;
                    }
                    if (oClickselectTextBox.Name == "textBox2")
                    {
                        oClickselectTextBox.IsTabStop = true;
                        oClickselectTextBox.TabIndex = 13;
                    }
                    if (oClickselectTextBox.Name == "txtOCOTriggerRate")
                    {
                        oClickselectTextBox.IsTabStop = true;
                        oClickselectTextBox.TabIndex = 15;
                    }
                    if (oClickselectTextBox.Name == "TxtMktProfitPercentage")
                    {
                        oClickselectTextBox.IsTabStop = true;
                        oClickselectTextBox.TabIndex = 20;
                    }

                }
                foreach (Button btn in FindVisualChildren<Button>(mWindow))
                {
                    if (btn.Name == "btnRefresh")
                    {
                        btn.IsTabStop = true;
                        btn.TabIndex = 16;
                    }
                    if (btn.Name == "btnLimit")
                    {
                        btn.IsTabStop = true;
                        btn.TabIndex = 17;
                    }
                    if (btn.Name == "BtnMarket")
                    {
                        btn.IsTabStop = true;
                        btn.TabIndex = 18;
                    }
                    if (btn.Name == "BtnBlockDeal")
                    {
                        btn.IsTabStop = true;
                        btn.TabIndex = 19;
                    }
                    if (btn.Name == "btnSearchCorpAction")
                    {
                        btn.IsTabStop = true;
                        btn.TabIndex = 23;
                    }
                    if (btn.Name == "btnSearchCorpAction")
                    {
                        btn.IsTabStop = true;
                        btn.TabIndex = 23;
                    }
                    //BtnBondCalc
                    if (btn.Name == "BtnBondCalc")
                    {
                        btn.IsTabStop = true;
                        btn.TabIndex = 25;
                    }
                    //OrderSliceCalc

                }
            }
            if (mWindow != null)
            {
                KeyboardNavigation.SetTabNavigation(mWindow, KeyboardNavigationMode.Local);
            }
        }
        private void NormalOrderReceived(object oModel, string status)
        {
            try
            {
                if (!string.IsNullOrEmpty(status) && status == Enumerations.WindowName.Normal_OE.ToString())
                {
                    if (oModel != null)
                    {
                        if (oModel.GetType().Name == "OrderModel")
                        {
                            OrderModel oOrderModel = new OrderModel();
                            oOrderModel = oModel as OrderModel;
                            if (oOrderModel.ReplyCode == 0)
                            {
                                ClearDataAfterOrderPlacement(oOrderModel);
                            }
                            ErrorMsg = oOrderModel.ReplyText?.ToString();

                        }
                    }
                }
            }
            catch (Exception ex)
            {

                ExceptionUtility.LogError(ex);
            }
        }

        private void ClearDataAfterOrderPlacement(OrderModel oOrderModel)
        {
            qty = String.Empty;
            rate = String.Empty;
            revQty = String.Empty;
            //if (ShortClientIDDerCheck != "Checked" || ShortClientIDCurCheck != "Checked" || ShortClientIDEquityCheck != "Checked" || ShortClientIDDebtCheck != "Checked")
            //{
            //    ShortClientSelectedText = String.Empty;
            //}

            if (oOrderModel.Segment == Enumerations.Order.ScripSegment.Equity.ToString())
            {
                if (ShortClientIDEquityCheck == Boolean.FalseString)
                    ShortClientSelectedText = String.Empty;
            }
            else if (oOrderModel.Segment == Enumerations.Order.ScripSegment.Debt.ToString())
            {
                if (ShortClientIDDebtCheck == Boolean.FalseString)
                    ShortClientSelectedText = String.Empty;
            }
            else if (oOrderModel.Segment == Enumerations.Order.ScripSegment.Derivative.ToString())
            {
                if (ShortClientIDDerCheck == Boolean.FalseString)
                    ShortClientSelectedText = String.Empty;
            }
            else if (oOrderModel.Segment == Enumerations.Order.ScripSegment.Currency.ToString())
            {
                if (ShortClientIDCurCheck == Boolean.FalseString)
                    ShortClientSelectedText = String.Empty;
            }
            //MktPT = String.Empty;
            //ShortClientSelectedText = String.Empty;
            //Need to be confirmed from Aakash.
            //RevChck = false;
            //OcoChecked = false;

            //newly added 25/4/2018 Gaurav Jadhav
            clienttypeselected = "CLIENT";
        }


        /// <summary>
        /// Display Normal Order Entry Window info after Scrip Click/Select from Touchline
        /// </summary>
        /// <param name="strScripCode"></param>
        /// <param name="status"></param>
        private void PopulateOrderEntryWindow(string strScripCode, string status)
        {
            long ScripCode = 0;
            //UtilityOrderDetails.GETInstance.GlobalScripSelectedCodeDerv = 0;
            //globalScripcode = 0;
            try
            {
                if (!string.IsNullOrEmpty(strScripCode))
                {
                    ScripCode = Convert.ToInt64(strScripCode);
                    var Segment = CommonFunctions.GetSegmentID(ScripCode);
                    ScripSelectedSegment = Segment;
                    //EnableDisable();

                    if (ScripSelectedSegment?.ToLower() == Enumerations.Order.ScripSegment.Equity.ToString().ToLower())
                    {
                        ScripSelectedCode = ScripCode;
                    }

                    else if (ScripSelectedSegment?.ToLower() == Enumerations.Order.ScripSegment.Debt.ToString().ToLower())
                    {
                        ScripSelectedCode = ScripCode;
                    }

                    else if (ScripSelectedSegment?.ToLower() == Enumerations.Order.ScripSegment.Derivative.ToString().ToLower())
                    {
                        // ScripCodeSelected = ScripCode;
                        ShortIntrType = CommonFunctions.GetInstrumentType(ScripCode, Selected_EXCH, ScripSelectedSegment);
                        SubIntrTypeSelected = CommonFunctions.GetOptionType(ScripCode, Selected_EXCH, ScripSelectedSegment);
                        StrategyID = CommonFunctions.GetStrategyID(ScripCode, Selected_EXCH, ScripSelectedSegment);

                        if (ShortIntrType == "IF")
                        {
                            SubIntrTypeSelected = string.Empty;
                            StrikePriceEnable = false;
                            IntrTypeSelected = Enumerations.Order.DervInstrumentType.FutIndex.ToString();
                        }
                        if (ShortIntrType == "SF")
                        {
                            SubIntrTypeSelected = string.Empty;
                            StrikePriceEnable = false;
                            IntrTypeSelected = Enumerations.Order.DervInstrumentType.FutStock.ToString();
                        }
                        else if (ShortIntrType == "IO")
                        {
                            if (SubIntrTypeSelected == "CE")
                            {
                                StrikePriceEnable = true;
                                IntrTypeSelected = Enumerations.Order.DervInstrumentType.CallIndex.ToString();
                            }
                            else if (SubIntrTypeSelected == "PE")
                            {
                                StrikePriceEnable = true;
                                IntrTypeSelected = Enumerations.Order.DervInstrumentType.PutIndex.ToString();
                            }

                        }
                        else if (ShortIntrType == "SO")
                        {
                            if (SubIntrTypeSelected == "CE")
                            {
                                StrikePriceEnable = true;
                                IntrTypeSelected = Enumerations.Order.DervInstrumentType.CallStock.ToString();
                            }
                            else if (SubIntrTypeSelected == "PE")
                            {
                                StrikePriceEnable = true;
                                IntrTypeSelected = Enumerations.Order.DervInstrumentType.PutStock.ToString();
                            }
                        }

                        if (StrategyID == 15)
                        {
                            StrikePriceEnable = true;
                            IntrTypeSelected = Enumerations.Order.DervInstrumentType.PairOption.ToString();
                        }

                        UnderAssetSelected = CommonFunctions.GetUnderLyingAssetCode(ScripCode, Selected_EXCH, ScripSelectedSegment);
                        ExpDateSelected = CommonFunctions.GetExpiryDate(ScripCode, Selected_EXCH, ScripSelectedSegment);
                        ScripIDSelected = CommonFunctions.GetScripId(ScripCode, Selected_EXCH, ScripSelectedSegment);
                        InstrNameSelected = CommonFunctions.GetScripName(ScripCode, Selected_EXCH, ScripSelectedSegment);
                        StkPriceSelected = CommonFunctions.GetStrikePrice(ScripCode, Selected_EXCH, ScripSelectedSegment);
                        ScripCodeSelected = ScripCode;
                    }

                    else if (ScripSelectedSegment?.ToLower() == Enumerations.Order.ScripSegment.Currency.ToString().ToLower())
                    {

                        ShortIntrType = CommonFunctions.GetInstrumentType(ScripCode, Selected_EXCH, ScripSelectedSegment);
                        SubIntrTypeSelected = CommonFunctions.GetOptionType(ScripCode, Selected_EXCH, ScripSelectedSegment);
                        StrategyID = CommonFunctions.GetStrategyID(ScripCode, Selected_EXCH, ScripSelectedSegment);

                        if (ShortIntrType != "OPTCUR")
                        {
                            //ShortIntrType = "OPTCUR";
                            SubIntrTypeSelected = string.Empty;
                            StrikePriceEnable = false;
                            IntrTypeSelected = Enumerations.Order.CurrInstrumentType.Future.ToString();
                        }
                        else if (ShortIntrType == "OPTCUR")
                        {
                            //ShortIntrType = "OPTCUR";
                            if (SubIntrTypeSelected == "CE")
                            {
                                StrikePriceEnable = true;
                                IntrTypeSelected = Enumerations.Order.CurrInstrumentType.Call.ToString();
                            }
                            else if (SubIntrTypeSelected == "PE")
                            {
                                StrikePriceEnable = true;
                                IntrTypeSelected = Enumerations.Order.CurrInstrumentType.Put.ToString();
                            }

                        }


                        if (StrategyID == 15)
                        {
                            StrikePriceEnable = true;
                            IntrTypeSelected = Enumerations.Order.CurrInstrumentType.PairOption.ToString();
                        }
                        else if (StrategyID == 28)
                        {
                            StrikePriceEnable = true;
                            IntrTypeSelected = Enumerations.Order.CurrInstrumentType.Straddle.ToString();
                        }

                        UnderAssetSelected = CommonFunctions.GetUnderLyingAssetCode(ScripCode, Selected_EXCH, ScripSelectedSegment);
                        ExpDateSelected = CommonFunctions.GetExpiryDate(ScripCode, Selected_EXCH, ScripSelectedSegment);
                        ScripIDSelected = CommonFunctions.GetScripId(ScripCode, Selected_EXCH, ScripSelectedSegment);
                        InstrNameSelected = CommonFunctions.GetScripName(ScripCode, Selected_EXCH, ScripSelectedSegment);
                        StkPriceSelected = CommonFunctions.GetStrikePrice(ScripCode, Selected_EXCH, ScripSelectedSegment);
                        ScripCodeSelected = ScripCode;
                        SetGlobalScripCodeSelected(Convert.ToString(ScripCode), ScripSelectedSegment);
                    }
                }
            }
            catch (Exception ex)
            {

                ExceptionUtility.LogError(ex);
            }
        }

        private void PopulateClientIdInputInstant()
        {
            ClientIDinputlst = new List<string>();

            if (MasterSharedMemory.objClientMasterDict != null && MasterSharedMemory.objClientMasterDict.Count > 0)
                ClientIDinputlst = MasterSharedMemory.objClientMasterDict.OrderBy(x => x.Value.ShortClientId).Select(x => x.Value.ShortClientId).ToList();

        }
        /// <summary>
        /// Assign scripcode to global variable
        /// </summary>
        /// <param name="ScripCode"></param>
        /// <returns></returns>
        private bool SetGlobalScripCodeSelected(string ScripCode, string segment)
        {
            bool result = false;
            if (!string.IsNullOrEmpty(ScripCode))
            {
                try
                {
                    if (segment == Enumerations.Order.ScripSegment.Equity.ToString() || segment == Enumerations.Order.ScripSegment.Debt.ToString())
                        UtilityOrderDetails.GETInstance.GlobalScripSelectedCode = Convert.ToInt64(ScripCode);
                    else if (segment == Enumerations.Order.ScripSegment.Derivative.ToString() || segment == Enumerations.Order.ScripSegment.Currency.ToString())
                        UtilityOrderDetails.GETInstance.GlobalScripSelectedCodeDerv = Convert.ToInt64(ScripCode);

                    result = true;
                }
                catch (Exception ex)
                {
                    ExceptionUtility.LogError(ex);
                    result = false;
                }
            }
            else
            {
                result = false;
            }
            return result;
        }
        //private void NormalOrderEntryClosing_Closing(object e)
        //{
        //    UtilityOrderDetails.GETInstance.GlobalScripSelectedCode = ScripSelectedCode;
        //    UtilityOrderDetails.GETInstance.GlobalScripSelectedCodeDerv = ScripCodeSelected;
        //}

        /// <summary>
        /// Display Normal Order Entry Window info after order placement from Returned Order.
        /// </summary>
        /// <param name="scripCode"></param>
        /// <param name="Key"></param>
        public void PassByNormalReturnOrder(long scripCode, string Key)
        {
            if (MemoryManager.OrderDictionary.ContainsKey(Key))
            {
                int DecimalPoint = CommonFunctions.GetDecimal(Convert.ToInt32(scripCode), "BSE", CommonFunctions.GetSegmentID(scripCode));
                ScripSelectedSegment = CommonFunctions.GetSegmentID(scripCode);
                //Selected_EXCH = Enumerations.Order.Exchanges.BSE.ToString();
                //IntrTypeSelected = scripCode.ToString().Trim();
                ScripSelectedCode = scripCode;
                qty = MemoryManager.OrderDictionary[Key].PendingQuantity.ToString();
                rate = (MemoryManager.OrderDictionary[Key].Price / Math.Pow(10, DecimalPoint)).ToString();
                clienttypeselected = MemoryManager.OrderDictionary[Key].ClientType;
                RetTypeSelected = MemoryManager.OrderDictionary[Key].OrderRetentionStatus;
                ShortClientSelectedText = MemoryManager.OrderDictionary[Key].ClientId;
                revQty = MemoryManager.OrderDictionary[Key].RevealQty.ToString();
                //if (MemoryManager.OrderDictionary[Key].OrderType == "L")
                //{
                //    OrderTypeSelected = Enumerations.Order.OrderTypes.LIMIT.ToString();
                //}
                if (MemoryManager.OrderDictionary[Key].OrderType == "G")
                {
                    // OrderTypeSelected = Enumerations.Order.OrderTypes.MARKET.ToString();
                    if (DecimalPoint == 2)
                    {
                        MktPT = Convert.ToString(Convert.ToInt64(MemoryManager.OrderDictionary[Key].ProtectionPercentage) / 100);
                    }
                    else if (DecimalPoint == 4)
                    {
                        MktPT = Convert.ToString(Convert.ToInt64(MemoryManager.OrderDictionary[Key].ProtectionPercentage) / 10000);
                    }
                }
                else if (MemoryManager.OrderDictionary[Key].OrderType == "P")
                {
                    //OrderTypeSelected = Enumerations.Order.OrderTypes.STOPLOSS.ToString();
                    trgPrice = (MemoryManager.OrderDictionary[Key].TriggerPrice / Math.Pow(10, DecimalPoint)).ToString();
                }
            }

            else
            {
                //TODO: 
            }
        }


        internal void PassByNetPositionScripWiseDetails(long scripCode, ScripWiseDetailPositionModel selectEntireRow)
        {
            int DecimalPoint = CommonFunctions.GetDecimal(Convert.ToInt32(scripCode), "BSE", CommonFunctions.GetSegmentID(scripCode));
            ScripSelectedSegment = CommonFunctions.GetSegmentID(scripCode);
            //Selected_EXCH = Enumerations.Order.Exchanges.BSE.ToString();
            //IntrTypeSelected = scripCode.ToString().Trim();
            ScripSelectedCode = scripCode;
            if (selectEntireRow.NetQty < 0)
                qty = Convert.ToString(selectEntireRow.NetQty * -1);
            else
                qty = Convert.ToString(selectEntireRow.NetQty);
            clienttypeselected = selectEntireRow.ClientType;
            ShortClientSelectedText = selectEntireRow.ClientID;
        }
        internal void PassByNetPositionClientWiseDetails(long scripCode, CWSWDetailPositionModel selectEntireRow)
        {
            int DecimalPoint = CommonFunctions.GetDecimal(Convert.ToInt32(scripCode), "BSE", CommonFunctions.GetSegmentID(scripCode));
            ScripSelectedSegment = CommonFunctions.GetSegmentID(scripCode);
            //Selected_EXCH = Enumerations.Order.Exchanges.BSE.ToString();
            //IntrTypeSelected = scripCode.ToString().Trim();
            ScripSelectedCode = scripCode;
            if (selectEntireRow.NetQty < 0)
                qty = Convert.ToString(selectEntireRow.NetQty * -1);
            else
                qty = Convert.ToString(selectEntireRow.NetQty);
            clienttypeselected = selectEntireRow.ClientType;
            ShortClientSelectedText = selectEntireRow.ClientID;
        }
        private void SwiftOrderEntry_Window(object e)
        {
            if (UtilityLoginDetails.GETInstance.IsLoggedIN)
            {
                if (!(UtilityLoginDetails.GETInstance.Role == Role.Admin.ToString()))
                {
                    if (SellChecked)
                    {
                        e = "SELL";
                    }
                    else
                    {
                        e = "BUY";
                    }
                    SwiftOrderEntry objswift = Application.Current.Windows.OfType<SwiftOrderEntry>().FirstOrDefault();
                    NormalOrderEntry objnormal = Application.Current.Windows.OfType<NormalOrderEntry>().FirstOrDefault();

                    if (objswift != null)
                    {
                        if (objswift.WindowState == WindowState.Minimized)
                            objswift.WindowState = WindowState.Normal;

                        ((OrderEntryVM)objswift.DataContext).BuySellWindow(e);

                        objswift.Focus();
                        objswift.Show();
                        UtilityOrderDetails.GETInstance.CurrentOrderEntry = Enumerations.OrderEntryWindow.Swift.ToString();
                        objnormal.Close(); // closing the existing window

                    }
                    else
                    {
                        objswift = new SwiftOrderEntry();
                        objswift.Owner = System.Windows.Application.Current.MainWindow;
                        //objswift.CmbExcangeType.Focus();

                        ((OrderEntryVM)objswift.DataContext).BuySellWindow(e);

                        objswift.Activate();
                        objswift.Show();
                        UtilityOrderDetails.GETInstance.CurrentOrderEntry = Enumerations.OrderEntryWindow.Swift.ToString();
                        objnormal.Close(); // closing the existing window
                    }


                }
            }
        }

        private void EqFiveLachCalculatorCheck(TextCompositionEventArgs e)
        {
            if (isFiveLacSelected == true)
            {
                bool Number = CheckIfNumber(e.Text);
                if (Number)
                {
                    TimeResetCounter++;
                    if (TimeResetCounter == 1)
                    {
                        TimerCombo.Enabled = true;
                    }



                    //num = e.Text;
                    //int number = Convert.ToInt32(num);
                    //int scripcode = 500000 +number;
                    //if (sw.ElapsedMilliseconds <= 2000)
                    //{
                    AppendedChar = AppendedChar + e.Text;
                    if (ScripSelectedSegment.ToUpper() == Enumerations.Order.ScripSegment.Equity.ToString().ToUpper() && !string.IsNullOrEmpty(AppendedChar))
                    {
                        if (AppendedChar.Length < 100000)
                        {

                            num = Convert.ToInt64(AppendedChar.Trim()) + 500000;
                            e.Handled = true;
                            if (ScripCodeLst.Contains(num))
                            {
                                int index = ScripCodeLst.IndexOf(num);
                                ScripSelectedCode = ScripCodeLst[index];
                            }
                        }
                    }
                    //}
                    //else
                    //{
                    //    AppendedChar = string.Empty;
                    //    TimeResetCounter = 0;
                    //}

                }
                else
                {
                    AppendedChar = string.Empty;
                }

            }
            //System.Windows.Forms.MessageBox.Show(e.Text);
        }

        private bool CheckIfNumber(string text)
        {
            int i = 0;
            bool result = int.TryParse(text, out i);
            return result;
        }

        private void TimerCombo_Elapsed(object sender, ElapsedEventArgs e)
        {
            AppendedChar = string.Empty;
            TimeResetCounter = 0;
        }

        //private void SetTimeronAndOff()
        //{
        //    if (isFiveLacSelected)
        //    {

        //    }
        //}


        private void RefreshBtnClick_Click()
        {
            if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Equity.ToString() || ScripSelectedSegment == Enumerations.Order.ScripSegment.Debt.ToString())
            {
                CommonFunctions.CallBestFiveUsingScripCode(Convert.ToInt32(ScripSelectedCode));
            }
            else if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Derivative.ToString() || ScripSelectedSegment == Enumerations.Order.ScripSegment.Currency.ToString())
            {
                CommonFunctions.CallBestFiveUsingScripCode(Convert.ToInt32(ScripCodeSelected));
            }

            BestFiveVM.MemberQueryBF();
            //BestFiveMarketPicture oLoginScreen = System.Windows.Application.Current.Windows.OfType<BestFiveMarketPicture>().FirstOrDefault();

            //if (oLoginScreen != null)
            //{
            //    oLoginScreen.Activate();
            //    oLoginScreen.Show();
            //}
            //else
            //{
            //    oLoginScreen = new BestFiveMarketPicture();
            //    oLoginScreen.Activate();
            //    oLoginScreen.Owner = System.Windows.Application.Current.MainWindow;
            //    oLoginScreen.Activate();
            //    oLoginScreen.Show();
            //}
        }
        private void searchButtonClick_execute()
        {
            //scripNameSearch scripNameSearch = new scripNameSearch();
            //scripNameSearch.Activate();
            //scripNameSearch.Show();

            scripNameSearch scripNameSearch = System.Windows.Application.Current.Windows.OfType<scripNameSearch>().FirstOrDefault();
            if (scripNameSearch == null)
            {
                scripNameSearch = new scripNameSearch();
                scripNameSearch.Activate();
                scripNameSearch.Show();
            }
            else
            {
                scripNameSearch.Focus();
                scripNameSearch.Show();
            }


        }

        private void UpdateTouchlineValue(Model.ScripDetails obj)
        {
            if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Equity.ToString() || ScripSelectedSegment == Enumerations.Order.ScripSegment.Debt.ToString())
            {
                DecimalPoint = Common.CommonFunctions.GetDecimal(Convert.ToInt32(ScripSelectedCode), Selected_EXCH, ScripSelectedSegment);
                BroadcastReceiver.ScripDetails Br = BroadcastMaster.BroadcastMasterMemory.objScripDetailsConDict.Where(x => x.Key == ScripSelectedCode).Select(x => x.Value).FirstOrDefault() == null ? new BroadcastReceiver.ScripDetails() : BroadcastMaster.BroadcastMasterMemory.objScripDetailsConDict.Where(x => x.Key == ScripSelectedCode).Select(x => x.Value).FirstOrDefault();
                Model.ScripDetails objScripDetails = new Model.ScripDetails();
                objScripDetails = MainWindowVM.UpdateScripDataFromMemory(Br);

                if (objScripDetails != null)
                {
                    if (objScripDetails.listBestFive != null && objScripDetails.listBestFive.Count() > 0)
                    {
                        if (objScripDetails.listBestFive[0].BuyQtyL == 0 && objScripDetails.listBestFive[0].SellQtyL == 0)
                        {
                            TouchlineValue = string.Empty;
                        }
                        else if (objScripDetails.listBestFive[0].BuyQtyL == 0)
                        {
                            TouchlineValue = "// S : " + objScripDetails.listBestFive[0].SellQtyL + " @ " + string.Format("{0:0.00}", (objScripDetails.listBestFive[0].SellRateL / Math.Pow(10, DecimalPoint)));
                        }
                        else if (objScripDetails.listBestFive[0].SellQtyL == 0)
                        {
                            TouchlineValue = "B : " + objScripDetails.listBestFive[0].BuyQtyL + " @ " + string.Format("{0:0.00}", (objScripDetails.listBestFive[0].BuyRateL / Math.Pow(10, DecimalPoint)));
                        }
                        else
                        {
                            if (CommonFrontEnd.Global.UtilityOrderDetails.GETInstance.SelectedTouchlineData == "B:[BQ@BR]//S:[SQ@SR]")
                            {
                                TouchlineValue = "B : " + objScripDetails.listBestFive[0].BuyQtyL + " @ " + string.Format("{0:0.00}", (objScripDetails.listBestFive[0].BuyRateL / Math.Pow(10, DecimalPoint))) + " // S : " + objScripDetails.listBestFive[0].SellQtyL + " @ " + string.Format("{0:0.00}", (objScripDetails.listBestFive[0].SellRateL / Math.Pow(10, DecimalPoint)));
                            }
                            else if (CommonFrontEnd.Global.UtilityOrderDetails.GETInstance.SelectedTouchlineData == "B:[BQ/BR//SR/SQ]:S")

                            {
                                TouchlineValue = "B: " + objScripDetails.listBestFive[0].BuyQtyL + " / " + string.Format("{0:0.00}", (objScripDetails.listBestFive[0].BuyRateL / Math.Pow(10, DecimalPoint))) + "//" + string.Format("{0:0.00}", (objScripDetails.listBestFive[0].SellRateL / Math.Pow(10, DecimalPoint))) + " / " + objScripDetails.listBestFive[0].SellQtyL + " :S";
                            }
                            else if (CommonFrontEnd.Global.UtilityOrderDetails.GETInstance.SelectedTouchlineData == "B//S:[BR//SR:BQ//SQ]")

                            {
                                TouchlineValue = "B // S : " + string.Format("{0:0.00}", (objScripDetails.listBestFive[0].BuyRateL / Math.Pow(10, DecimalPoint))) + " // " + string.Format("{0:0.00}", (objScripDetails.listBestFive[0].SellRateL / Math.Pow(10, DecimalPoint))) + " :  " + objScripDetails.listBestFive[0].BuyQtyL + " // " + objScripDetails.listBestFive[0].SellQtyL;
                            }

                        }
                    }
                    else
                    {
                        TouchlineValue = string.Empty;
                    }
                }
            }
            else if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Derivative.ToString())
            {
                DecimalPoint = Common.CommonFunctions.GetDecimal(Convert.ToInt32(ScripCodeSelected), Selected_EXCH, ScripSelectedSegment);
                BroadcastReceiver.ScripDetails Br = BroadcastMaster.BroadcastMasterMemory.objScripDetailsConDict.Where(x => x.Key == ScripCodeSelected).Select(x => x.Value).FirstOrDefault() == null ? new BroadcastReceiver.ScripDetails() : BroadcastMaster.BroadcastMasterMemory.objScripDetailsConDict.Where(x => x.Key == ScripCodeSelected).Select(x => x.Value).FirstOrDefault();
                Model.ScripDetails objScripDetails = new Model.ScripDetails();
                objScripDetails = MainWindowVM.UpdateScripDataFromMemory(Br);

                if (objScripDetails != null)
                {
                    if (objScripDetails.listBestFive != null && objScripDetails.listBestFive.Count() > 0)
                    {
                        if (objScripDetails.listBestFive[0].BuyQtyL == 0 && objScripDetails.listBestFive[0].SellQtyL == 0)
                        {
                            TouchlineValue = string.Empty;
                        }
                        else if (objScripDetails.listBestFive[0].BuyQtyL == 0)
                        {
                            TouchlineValue = "// S : " + objScripDetails.listBestFive[0].SellQtyL + "@" + string.Format("{0:0.00}", (objScripDetails.listBestFive[0].SellRateL / Math.Pow(10, DecimalPoint)));
                        }
                        else if (objScripDetails.listBestFive[0].SellQtyL == 0)
                        {
                            TouchlineValue = "B : " + objScripDetails.listBestFive[0].BuyQtyL + "@" + string.Format("{0:0.00}", (objScripDetails.listBestFive[0].BuyRateL / Math.Pow(10, DecimalPoint)));
                        }
                        else
                        {
                            //TouchlineValue = "B : " + objScripDetails.listBestFive[0].BuyQtyL + "@" + string.Format("{0:0.00}", (objScripDetails.listBestFive[0].BuyRateL / Math.Pow(10, DecimalPoint))) + "// S : " + objScripDetails.listBestFive[0].SellQtyL + "@" + string.Format("{0:0.00}", (objScripDetails.listBestFive[0].SellRateL / Math.Pow(10, DecimalPoint)));
                            if (CommonFrontEnd.Global.UtilityOrderDetails.GETInstance.SelectedTouchlineData == "B:[BQ@BR]//S:[SQ@SR]")
                            {
                                TouchlineValue = "B : " + objScripDetails.listBestFive[0].BuyQtyL + " @ " + string.Format("{0:0.00}", (objScripDetails.listBestFive[0].BuyRateL / Math.Pow(10, DecimalPoint))) + " // S : " + objScripDetails.listBestFive[0].SellQtyL + " @ " + string.Format("{0:0.00}", (objScripDetails.listBestFive[0].SellRateL / Math.Pow(10, DecimalPoint)));
                            }
                            else if (CommonFrontEnd.Global.UtilityOrderDetails.GETInstance.SelectedTouchlineData == "B:[BQ/BR//SR/SQ]:S")

                            {
                                TouchlineValue = "B: " + objScripDetails.listBestFive[0].BuyQtyL + " / " + string.Format("{0:0.00}", (objScripDetails.listBestFive[0].BuyRateL / Math.Pow(10, DecimalPoint))) + "//" + string.Format("{0:0.00}", (objScripDetails.listBestFive[0].SellRateL / Math.Pow(10, DecimalPoint))) + " / " + objScripDetails.listBestFive[0].SellQtyL + " :S";
                            }
                            else if (CommonFrontEnd.Global.UtilityOrderDetails.GETInstance.SelectedTouchlineData == "B//S:[BR//SR:BQ//SQ]")

                            {
                                TouchlineValue = "B // S : " + string.Format("{0:0.00}", (objScripDetails.listBestFive[0].BuyRateL / Math.Pow(10, DecimalPoint))) + " // " + string.Format("{0:0.00}", (objScripDetails.listBestFive[0].SellRateL / Math.Pow(10, DecimalPoint))) + " :  " + objScripDetails.listBestFive[0].BuyQtyL + " // " + objScripDetails.listBestFive[0].SellQtyL;
                            }
                        }
                    }
                    else
                    {
                        TouchlineValue = string.Empty;
                    }
                }
            }
            else if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Currency.ToString())
            {
                DecimalPoint = Common.CommonFunctions.GetDecimal(Convert.ToInt32(ScripCodeSelected), Selected_EXCH, ScripSelectedSegment);
                BroadcastReceiver.ScripDetails Br = BroadcastMaster.BroadcastMasterMemory.objScripDetailsConDict.Where(x => x.Key == ScripCodeSelected).Select(x => x.Value).FirstOrDefault() == null ? new BroadcastReceiver.ScripDetails() : BroadcastMaster.BroadcastMasterMemory.objScripDetailsConDict.Where(x => x.Key == ScripCodeSelected).Select(x => x.Value).FirstOrDefault();
                Model.ScripDetails objScripDetails = new Model.ScripDetails();
                objScripDetails = MainWindowVM.UpdateScripDataFromMemory(Br);

                if (objScripDetails != null)
                {
                    if (objScripDetails.listBestFive != null && objScripDetails.listBestFive.Count() > 0)
                    {
                        if (objScripDetails.listBestFive[0].BuyQtyL == 0 && objScripDetails.listBestFive[0].SellQtyL == 0)
                        {
                            TouchlineValue = string.Empty;
                        }
                        else if (objScripDetails.listBestFive[0].BuyQtyL == 0)
                        {
                            TouchlineValue = "// S : " + objScripDetails.listBestFive[0].SellQtyL + "@" + string.Format("{0:0.0000}", (objScripDetails.listBestFive[0].SellRateL / Math.Pow(10, DecimalPoint)));
                        }
                        else if (objScripDetails.listBestFive[0].SellQtyL == 0)
                        {
                            TouchlineValue = "B : " + objScripDetails.listBestFive[0].BuyQtyL + "@" + string.Format("{0:0.0000}", (objScripDetails.listBestFive[0].BuyRateL / Math.Pow(10, DecimalPoint)));
                        }
                        else
                        {
                            //TouchlineValue = "B : " + objScripDetails.listBestFive[0].BuyQtyL + "@" + string.Format("{0:0.0000}", (objScripDetails.listBestFive[0].BuyRateL / Math.Pow(10, DecimalPoint))) + "// S : " + objScripDetails.listBestFive[0].SellQtyL + "@" + string.Format("{0:0.0000}", (objScripDetails.listBestFive[0].SellRateL / Math.Pow(10, DecimalPoint)));
                            if (CommonFrontEnd.Global.UtilityOrderDetails.GETInstance.SelectedTouchlineData == "B:[BQ@BR]//S:[SQ@SR]")
                            {
                                TouchlineValue = "B : " + objScripDetails.listBestFive[0].BuyQtyL + " @ " + string.Format("{0:0.0000}", (objScripDetails.listBestFive[0].BuyRateL / Math.Pow(10, DecimalPoint))) + " // S : " + objScripDetails.listBestFive[0].SellQtyL + " @ " + string.Format("{0:0.0000}", (objScripDetails.listBestFive[0].SellRateL / Math.Pow(10, DecimalPoint)));
                            }
                            else if (CommonFrontEnd.Global.UtilityOrderDetails.GETInstance.SelectedTouchlineData == "B:[BQ/BR//SR/SQ]:S")

                            {
                                TouchlineValue = "B: " + objScripDetails.listBestFive[0].BuyQtyL + " / " + string.Format("{0:0.0000}", (objScripDetails.listBestFive[0].BuyRateL / Math.Pow(10, DecimalPoint))) + "//" + string.Format("{0:0.0000}", (objScripDetails.listBestFive[0].SellRateL / Math.Pow(10, DecimalPoint))) + " / " + objScripDetails.listBestFive[0].SellQtyL + " :S";
                            }
                            else if (CommonFrontEnd.Global.UtilityOrderDetails.GETInstance.SelectedTouchlineData == "B//S:[BR//SR:BQ//SQ]")

                            {
                                TouchlineValue = "B // S : " + string.Format("{0:0.0000}", (objScripDetails.listBestFive[0].BuyRateL / Math.Pow(10, DecimalPoint))) + " // " + string.Format("{0:0.0000}", (objScripDetails.listBestFive[0].SellRateL / Math.Pow(10, DecimalPoint))) + " :  " + objScripDetails.listBestFive[0].BuyQtyL + " // " + objScripDetails.listBestFive[0].SellQtyL;
                            }
                        }
                    }
                    else
                    {
                        TouchlineValue = string.Empty;
                    }
                }
            }
        }

        private void SubscribetoBcastMemory(int ScripCode)
        {
            if (!SubscribeScripMemory.objMasterSubscribeScrip.ContainsKey(ScripCode))
            {
                SubscribeList.SubscribeScrip s = new SubscribeScrip();
                s.ScripCode_l = ScripCode;
                s.UpdateFlag_s = 1;
                SubscribeScripMemory.objMasterSubscribeScrip.TryAdd(ScripCode, s);
            }

            if (BroadcastMaster.BroadcastMasterMemory.objScripDetailsConDict.ContainsKey(ScripCode))
            {
                BroadcastReceiver.ScripDetails objScripDetails = new BroadcastReceiver.ScripDetails();
                objScripDetails = BroadcastMaster.BroadcastMasterMemory.objScripDetailsConDict[ScripCode];
                DecimalPoint = Common.CommonFunctions.GetDecimal(ScripCode, Selected_EXCH, ScripSelectedSegment);

                if (objScripDetails != null)
                {
                    if (objScripDetails.Det != null && objScripDetails.Det.Count() > 0)
                    {
                        if (objScripDetails.Det[0].BuyQty_l == 0 && objScripDetails.Det[0].SellQty_l == 0)
                        {
                            TouchlineValue = string.Empty;
                        }
                        else if (objScripDetails.Det[0].SellQty_l == 0)
                        {
                            TouchlineValue = "B : " + objScripDetails.Det[0].BuyQty_l + "@" + string.Format("{0:0.00}", (objScripDetails.Det[0].BuyRate_l / Math.Pow(10, DecimalPoint)));
                        }
                        else if (objScripDetails.Det[0].BuyQty_l == 0)
                        {
                            TouchlineValue = "// S : " + objScripDetails.Det[0].SellQty_l + "@" + string.Format("{0:0.00}", (objScripDetails.Det[0].SellRate_l / Math.Pow(10, DecimalPoint)));
                        }
                        else
                            TouchlineValue = "B : " + objScripDetails.Det[0].BuyQty_l + "@" + string.Format("{0:0.00}", (objScripDetails.Det[0].BuyRate_l / Math.Pow(10, DecimalPoint))) + "// S : " + objScripDetails.Det[0].SellQty_l + "@" + string.Format("{0:0.00}", (objScripDetails.Det[0].SellRate_l / Math.Pow(10, DecimalPoint)));
                    }
                    else
                    {
                        TouchlineValue = string.Empty;
                    }
                }
            }
            else
            {
                TouchlineValue = string.Empty;
            }

        }

        private void UpdateVarValue(VarMain obj)
        {
            try
            {
                if (obj != null)
                {
                    for (int i = 0; i < obj.NoOfRec; i++)
                    {
                        //List<int> list = new List<int>();

                        //if (obj.VarMainDetailsObj[i].Identifier == 'E')
                        //{
                        //    ScripSymSelected = CommonFunctions.GetScripName(obj.VarMainDetailsObj[i].InstrumentCode, Enumerations.Exchange.BSE, Enumerations.Segment.Equity);
                        //    ScripSelectedCode = obj.VarMainDetailsObj[i].InstrumentCode;
                        //    flag = true;
                        //}
                        //else
                        //{
                        //    ScripSymSelected = CommonFunctions.GetScripName(obj.VarMainDetailsObj[i].InstrumentCode, Enumerations.Exchange.BSE, Enumerations.Segment.Derivative);
                        //    ScripSelectedCode = obj.VarMainDetailsObj[i].InstrumentCode;
                        //    flag = true;
                        //}

                        //if (obj.VarMainDetailsObj[i].IMPercentage != 0)
                        //    VarIM = obj.VarMainDetailsObj[i].IMPercentage / 100;
                        //if (obj.VarMainDetailsObj[i].ELMPercentage != 0)
                        //    VarEM = obj.VarMainDetailsObj[i].ELMPercentage / 100;
                        // VarPercentageBroadcastVM.ObjVarPercentageBroadcastCollection[list[0]].Identifier = obj.VarMainDetailsObj[i].Identifier;

                        if (obj.VarMainDetailsObj[i].InstrumentCode == ScripSelectedCode)
                        {
                            //VarIM = obj.VarMainDetailsObj[i].IMPercentage / 100;
                            //VarEM = obj.VarMainDetailsObj[i].ELMPercentage / 100;
                        }

                    }

                }
            }

            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
            }

        }

        public void UpdateScripDetails(long? selectedScripCode)
        {
            if (selectedScripCode != 0 && selectedScripCode != null)
            {
                if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Equity.ToString() || ScripSelectedSegment == Enumerations.Order.ScripSegment.Debt.ToString())
                {
                    if (ScripCodeLst.Contains((long)selectedScripCode))
                        ScripSelectedCode = selectedScripCode.GetValueOrDefault();

                }
                else if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Derivative.ToString() || ScripSelectedSegment == Enumerations.Order.ScripSegment.Currency.ToString())
                {
                    if (ScripCodeList.Contains((long)selectedScripCode))
                        ScripCodeSelected = selectedScripCode.GetValueOrDefault();
                }
            }
            //else
            //{
            //    ScripSelectedCode = 0;
            //}

            flag = true;
        }

        private void EnableDisable()
        {
            SetTabindex(null);
            try
            {
                if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Equity.ToString())
                {
                    EQStackVisi = "Visible";  // To show Equity OE
                    DerivStackVisi = "Hidden"; //To Hide Derivative OE
                    CurrencyAssetEnable = false;
                    MktLotVisibilty = "Visible";
                    PerlotVisibilty = "Hidden";
                    FaceValueVisibility = "Visible";
                    OpenIntVisibility = "Hidden";
                    BlockDealVisibility = "Visible";
                    //OrderSlicingVisibility = "Visible";
                    OrderSlicingVisibility = "Hidden";
                    BondCalcVisibility = "Collapsed";
                    DervChainVisibility = "Collapsed";
                    ClientIdVisibility = "Visible";
                    CPCodeVisibility = "Hidden";
                    IsEQvisible = "Visible";
                    IsDERvisible = "Collapsed";
                    IsDebtvisible = "Collapsed";
                    IsCurrencyvisible = "Collapsed";
                    RetTypeSelected = Enumerations.Order.RetType.EOD.ToString();
                    ClearRequiredFields();
                    if (Limit.g_Limit != null && Limit.g_Limit.Length > 0)
                        HeaderTitle = string.Format("Order Entry EQX - Buy Limit: [{0:0.00} L], Sell Limit: [{1:0.00} L]", Limit.g_Limit[CommonFunctions.GetSegment(ScripSelectedSegment)].AvailGrossBuyLimit / 100000, Limit.g_Limit[CommonFunctions.GetSegment(ScripSelectedSegment)].AvailGrossSellLimit / 100000);
                }

                else if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Debt.ToString())
                {
                    EQStackVisi = "Visible";  // To show Equity OE
                    DerivStackVisi = "Hidden"; //To Hide Derivative OE
                    CurrencyAssetEnable = false;
                    MktLotVisibilty = "Visible";
                    PerlotVisibilty = "Hidden";
                    FaceValueVisibility = "Visible";
                    OpenIntVisibility = "Hidden";
                    BlockDealVisibility = "Visible";
                    //OrderSlicingVisibility = "Collapsed";
                    OrderSlicingVisibility = "Hidden";
                    BondCalcVisibility = "Visible";
                    DervChainVisibility = "Collapsed";
                    ClientIdVisibility = "Visible";
                    CPCodeVisibility = "Hidden";
                    IsEQvisible = "Collapsed";
                    IsDERvisible = "Collapsed";
                    IsDebtvisible = "Visible";
                    IsCurrencyvisible = "Collapsed";
                    RetTypeSelected = Enumerations.Order.RetType.EOD.ToString();
                    ClearRequiredFields();
                    if (Limit.g_Limit != null && Limit.g_Limit.Length > 0)
                        HeaderTitle = string.Format("Order Entry EQX - Buy Limit: [{0:0.00} L], Sell Limit: [{1:0.00} L]", Limit.g_Limit[CommonFunctions.GetSegment(ScripSelectedSegment)].AvailGrossBuyLimit / 100000, Limit.g_Limit[CommonFunctions.GetSegment(ScripSelectedSegment)].AvailGrossSellLimit / 100000);
                }

                else if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Derivative.ToString())
                {
                    ChangeCounter = 0;
                    EQStackVisi = "Hidden";  // To hide EQ OE
                    DerivStackVisi = "Visible"; // To show Derivative OE
                    PerlotVisibilty = "Hidden";
                    MktLotVisibilty = "Visible";
                    FaceValueVisibility = "Hidden";
                    OpenIntVisibility = "Visible";
                    CurrencyAssetEnable = true;
                    BlockDealVisibility = "Collapsed";
                    //OrderSlicingVisibility = "Collapsed";
                    OrderSlicingVisibility = "Hidden";
                    BondCalcVisibility = "Collapsed";
                    DervChainVisibility = "Visible";
                    ClientIdVisibility = "Hidden";
                    CPCodeVisibility = "Visible";
                    IsEQvisible = "Collapsed";
                    IsDERvisible = "Visible";
                    IsDebtvisible = "Collapsed";
                    IsCurrencyvisible = "Collapsed";
                    RetTypeSelected = Enumerations.Order.RetType.EOS.ToString();
                    ClearRequiredFields();
                    if (Limit.g_Limit != null && Limit.g_Limit.Length > 0)
                        HeaderTitle = string.Format("Order Entry EDX - Buy Limit: [{0:0.00} L], Sell Limit: [{1:0.00} L]", Limit.g_Limit[CommonFunctions.GetSegment(ScripSelectedSegment)].AvailGrossBuyLimit / 100000, Limit.g_Limit[CommonFunctions.GetSegment(ScripSelectedSegment)].AvailGrossSellLimit / 100000);
                }

                else if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Currency.ToString())
                {
                    ChangeCounter = 0;
                    EQStackVisi = "Hidden";  // To hide EQ OE
                    DerivStackVisi = "Visible"; // To show Derivative OE
                    MktLotVisibilty = "Hidden";
                    PerlotVisibilty = "Visible";
                    FaceValueVisibility = "Hidden";
                    OpenIntVisibility = "Visible";
                    CurrencyAssetEnable = true;
                    BlockDealVisibility = "Collapsed";
                    //OrderSlicingVisibility = "Collapsed";
                    OrderSlicingVisibility = "Hidden";
                    BondCalcVisibility = "Collapsed";
                    DervChainVisibility = "Collapsed";
                    ClientIdVisibility = "Hidden";
                    CPCodeVisibility = "Visible";
                    IsEQvisible = "Collapsed";
                    IsDERvisible = "Collapsed";
                    IsDebtvisible = "Collapsed";
                    IsCurrencyvisible = "Visible";
                    RetTypeSelected = Enumerations.Order.RetType.EOD.ToString();
                    ClearRequiredFields();
                    if (Limit.g_Limit != null && Limit.g_Limit.Length > 0)
                        HeaderTitle = string.Format("Order Entry CDX - Buy Limit: [{0:0.00} L], Sell Limit: [{1:0.00} L]", Limit.g_Limit[CommonFunctions.GetSegment(ScripSelectedSegment)].AvailGrossBuyLimit / 100000, Limit.g_Limit[CommonFunctions.GetSegment(ScripSelectedSegment)].AvailGrossSellLimit / 100000);
                }


                if (OrderTypeSelected == Enumerations.Order.OrderTypes.STOPLOSS.ToString())
                {
                    TrgEnabled = true;
                }
#if TWS
                if (OrderTypeSelected == Enumerations.Order.OrderTypes.STOPLOSS.ToString() || OrderTypeSelected == "STOPLOSSMKT")
                {
                    TrgEnabled = true;
                }
                if (OrderTypeSelected == "STOPLOSSMKT")
                    MktPrtEnabled = true;
#endif

                if (OrderTypeSelected == Enumerations.Order.OrderTypes.LIMIT.ToString() || OrderTypeSelected == Enumerations.Order.OrderTypes.OCO.ToString() || OrderTypeSelected == Enumerations.Order.OrderTypes.MARKET.ToString())
                {
                    TrgEnabled = true;
                }

                if (OrderTypeSelected == Enumerations.Order.OrderTypes.LIMIT.ToString() || OrderTypeSelected == Enumerations.Order.OrderTypes.OCO.ToString() || OrderTypeSelected == Enumerations.Order.OrderTypes.MARKET.ToString())
                {
                    TrgEnabled = false;
                }
                if (OrderTypeSelected == Enumerations.Order.OrderTypes.MARKET.ToString())
                {
                    MktPrtEnabled = true;
                }
                else if (OrderTypeSelected == Enumerations.Order.OrderTypes.LIMIT.ToString() || OrderTypeSelected == Enumerations.Order.OrderTypes.OCO.ToString() || OrderTypeSelected == Enumerations.Order.OrderTypes.STOPLOSS.ToString())
                {
                    MktPrtEnabled = false;
                }

                if (Selected_EXCH == Enumerations.Order.Exchanges.BSE.ToString())
                {
                    if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Equity.ToString())
                    {
                        if (UtilityOrderDetails.GETInstance.GlobalScripSelectedCode != 0)
                        {
                            globalScripcode = UtilityOrderDetails.GETInstance.GlobalScripSelectedCode;
                        }

                        PopulateScripCode();
                        PopulatingScripName();
                        PopulatingScripSym();

                        if (globalScripcode != 0)
                        {
                            if (ScripCodeLst.Contains(globalScripcode))
                                ScripSelectedCode = globalScripcode;
                        }

                        if (ShortClientIDEquityCheck?.ToLower() == Boolean.TrueString.ToLower())
                        {
                            ClientIDinputlst.Clear();
                            ShortClientSelectedText = UtilityOrderDetails.GETInstance.EqtShortClientID;
                        }


                    }
                    else if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Debt.ToString())
                    {
                        if (UtilityOrderDetails.GETInstance.GlobalScripSelectedCode != 0)
                        {
                            globalScripcode = UtilityOrderDetails.GETInstance.GlobalScripSelectedCode;
                        }

                        PopulateScripCode();
                        PopulatingScripName();
                        PopulatingScripSym();

                        if (globalScripcode != 0)
                        {
                            if (ScripCodeLst.Contains(globalScripcode))
                                ScripSelectedCode = globalScripcode;
                        }


                        if (ShortClientIDDebtCheck?.ToLower() == Boolean.TrueString.ToLower())
                        {
                            ClientIDinputlst.Clear();
                            ShortClientSelectedText = UtilityOrderDetails.GETInstance.DebtShortClientID;
                        }

                    }
                    else if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Derivative.ToString())
                    {
                        PopulatingInstType();


                        if (ShortClientIDDerCheck?.ToLower() == Boolean.TrueString.ToLower())
                        {
                            ClientIDinputlst.Clear();
                            ShortClientSelectedText = UtilityOrderDetails.GETInstance.DervShortClientID;
                        }
                    }
                    else if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Currency.ToString())
                    {

                        PopulatingInstType();

                        if (ShortClientIDCurCheck?.ToLower() == Boolean.TrueString.ToLower())
                        {
                            ClientIDinputlst.Clear();
                            ShortClientSelectedText = UtilityOrderDetails.GETInstance.CurrShortClientID;
                        }
                    }

                }
                //clienttypeselected = Enumerations.Order.ClientTypes.CLIENT.ToString();
                AssignClientTypeFromGlobal();
                ErrorMsg = string.Empty;
            }

            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
            }

        }

        private void ClearRequiredFields()
        {
            qty = string.Empty;
            revQty = string.Empty;
            rate = string.Empty;

        }

        private void OCOChkEnability()
        {
            if (OcoChecked == true)
            {
                TrgRateEnbl = true;
                MktbtnEnability = false;
                BlockDealbtnEnability = false;
            }
            else
            {
                TrgRateEnbl = false;
                trgPrice = string.Empty;
                MktbtnEnability = true;
                BlockDealbtnEnability = true;
            }
        }

        private void RevQtyEnability()
        {
            if (RevChck == true)
            {
                RevQtyEnbl = true;
            }
            else
            {
                RevQtyEnbl = false;
                if (!string.IsNullOrEmpty(qty))
                {
                    revQty = CommonFunctions.CalculateRevQty(MarketLot, Convert.ToInt64(qty)).ToString();
                }
            }
        }

        //private void OnChangeOfQuantity()
        //{
        //    if (RevChck == true)
        //        RevQtyEnbl = true;
        //    else
        //    {
        //        if (Convert.ToInt32(qty) % MarketLot == 0)
        //        {
        //            RevQtyEnbl = false;
        //            revQty = qty;
        //        }
        //    }
        //}

        private void PopulatingScripProfileSegment()
        {
            try
            {


                if (Selected_EXCH == Enumerations.Order.Exchanges.BSE.ToString())
                {
                    ScripSegmentLst = new BindingList<string>();

                    ScripSelectedSegment = Enumerations.Order.ScripSegment.Equity.ToString();
                    ScripSegmentLst.Add(Enumerations.Order.ScripSegment.Equity.ToString());
                    ScripSegmentLst.Add(Enumerations.Order.ScripSegment.Derivative.ToString());
                    ScripSegmentLst.Add(Enumerations.Order.ScripSegment.Debt.ToString());
                    ScripSegmentLst.Add(Enumerations.Order.ScripSegment.Currency.ToString());
                }



            }
            catch (Exception ex)
            {

                ExceptionUtility.LogError(ex);
            }

        }

        private void PopulateScripCode()
        {
            try
            {
                //for BSE Exchange
                if (Selected_EXCH == Enumerations.Order.Exchanges.BSE.ToString())
                {
                    //for equity market
                    if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Equity.ToString())
                    {
                        ScripCodeLst = new List<long>();
                        if (MasterSharedMemory.objMastertxtDictBaseBSE != null)
                            ScripCodeLst = MasterSharedMemory.objMastertxtDictBaseBSE.Values.Cast<ScripMasterBase>().Where(x => x.InstrumentType == 'E').Select(x => x.ScripCode).ToList();

                        if (ScripCodeLst.Count() > 0)
                            ScripSelectedCode = ScripCodeLst[0];
                    }
                    else if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Debt.ToString())
                    {
                        ScripCodeLst = new List<long>();
                        if (MasterSharedMemory.objMastertxtDictBaseBSE != null)
                            ScripCodeLst = MasterSharedMemory.objMastertxtDictBaseBSE.Values.Cast<ScripMasterBase>().Where(x => x.InstrumentType != 'E').Select(x => x.ScripCode).ToList();

                        if (ScripCodeLst.Count() > 0)
                            ScripSelectedCode = ScripCodeLst[0];
                    }

                    else
                    {
                        if (ScripCodeLst != null)
                        {
                            ScripCodeLst.Clear();
                        }
                    }
                }


            }
            catch (Exception ex)
            {

                ExceptionUtility.LogError(ex);
            }


        }

        private void PopulatingScripSym()
        {
            try
            {
                ScripSymLst = new List<string>();
                //for BSE Exchange
                if (Selected_EXCH == Enumerations.Order.Exchanges.BSE.ToString())
                {
                    //for equity market
                    if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Equity.ToString())
                    {
                        // if (MasterSharedMemory.objMastertxtDictBase != null)
                        if (MasterSharedMemory.objMastertxtDictBaseBSE != null)
                            // ScripSymLst = MasterSharedMemory.objMastertxtDictBase.Values.Cast<ScripMasterBase>().Where(x => (x.InstrumentType == 'E')).GroupBy(x => x.ScripId).Select(x => x.FirstOrDefault().ScripId).ToList();
                            //   ScripSymLst = MasterSharedMemory.objMastertxtDictBase.Values.Cast<ScripMasterBase>().Select(x => x.ScripId).ToList();
                            ScripSymLst = MasterSharedMemory.objMastertxtDictBaseBSE.Values.Cast<ScripMasterBase>().Where(x => x.InstrumentType == 'E').Select(x => x.ScripId).ToList();
                        if (ScripSymLst.Count() > 0)
                            ScripSymSelected = ScripSymLst[0];
                    }
                    else if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Debt.ToString())
                    {
                        // if (MasterSharedMemory.objMastertxtDictBase != null)
                        if (MasterSharedMemory.objMastertxtDictBaseBSE != null)
                            // ScripSymLst = MasterSharedMemory.objMastertxtDictBase.Values.Cast<ScripMasterBase>().Where(x => (x.InstrumentType == 'E')).GroupBy(x => x.ScripId).Select(x => x.FirstOrDefault().ScripId).ToList();
                            //   ScripSymLst = MasterSharedMemory.objMastertxtDictBase.Values.Cast<ScripMasterBase>().Select(x => x.ScripId).ToList();
                            ScripSymLst = MasterSharedMemory.objMastertxtDictBaseBSE.Values.Cast<ScripMasterBase>().Where(x => x.InstrumentType != 'E').Select(x => x.ScripId).ToList();
                        if (ScripSymLst.Count() > 0)
                            ScripSymSelected = ScripSymLst[0];
                    }
                }

            }
            catch (Exception ex)
            {

                ExceptionUtility.LogError(ex);
            }


        }

        private void PopulatingScripName()
        {
            try
            {
                //for BSE Exchange
                if (Selected_EXCH == Enumerations.Order.Exchanges.BSE.ToString())
                {
                    if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Equity.ToString())
                    {
                        ScripNameLst = new List<String>();
                        if (MasterSharedMemory.objMastertxtDictBaseBSE != null)//BSE Equity
                            ScripNameLst = MasterSharedMemory.objMastertxtDictBaseBSE.Values.Cast<ScripMasterBase>().Where(x => x.InstrumentType == 'E').Select(x => x.ScripName).ToList();
                        if (ScripNameLst.Count() > 0)
                            ScripNameSelected = ScripNameLst[0];
                    }
                    else if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Debt.ToString())
                    {
                        ScripNameLst = new List<String>();
                        if (MasterSharedMemory.objMastertxtDictBaseBSE != null)//BSE Equity
                            ScripNameLst = MasterSharedMemory.objMastertxtDictBaseBSE.Values.Cast<ScripMasterBase>().Where(x => x.InstrumentType != 'E').Select(x => x.ScripName).ToList();
                        if (ScripNameLst.Count() > 0)
                            ScripNameSelected = ScripNameLst[0];
                    }
                }
            }

            catch (Exception ex)
            {

                ExceptionUtility.LogError(ex);
            }

        }

        private void OnChangeOfExchange()
        {
            Exchange = new List<string>();
#if TWS
            Selected_EXCH = MainWindowVM.parserOS.GetSetting("GENERAL OS", "SelectedExchange");
            if (Selected_EXCH == null)
                Selected_EXCH = Enumerations.Order.Exchanges.BSE.ToString();
            Exchange.Add(Enumerations.Order.Exchanges.BSE.ToString());
#elif BOW
            Selected_EXCH = Enumerations.Order.Exchanges.BSE.ToString();
            Exchange.Add(Enumerations.Order.Exchanges.BSE.ToString());
            Exchange.Add(Enumerations.Order.Exchanges.NSE.ToString());
            Exchange.Add(Enumerations.Order.Exchanges.MCX.ToString());
            Exchange.Add(Enumerations.Order.Exchanges.NCDEX.ToString());
#endif
        }

        private void OnChangeOfScripCode()
        {
            UtilityOrderDetails.GETInstance.GlobalScripSelectedCode = ScripSelectedCode;

            CorpActionData = CommonFunctions.GetCorpAction(ScripSelectedCode);
            if (flag)
            {
                scripIdChange = false;
                if (ScripSelectedCode != 0)
                {
                    SubscribetoBcastMemory((int)ScripSelectedCode);
                    //ScripSymSelected = CommonFunctions.GetScripId((long)ScripSelectedCode);
                    //ScripNameSelected = MasterSharedMemory.objMastertxtDictBase[(long)ScripSelectedCode].ScripName;
                    if (Selected_EXCH == Enumerations.Exchange.BSE.ToString())
                    {
                        ScripSymSelected = CommonFunctions.GetScripId((long)ScripSelectedCode, Enumerations.Exchange.BSE, Enumerations.Segment.Equity);//BSE Equity
                        if (ScripSymSelected != null && ScripSymSelected != string.Empty && ScripSymSelected != "")
                        {
                            ScripNameSelected = MasterSharedMemory.objMastertxtDictBaseBSE[(long)ScripSelectedCode].ScripName;

                        }
                        else
                        {
                            ScripSymSelected = string.Empty;
                        }
                    }
                    else if (Selected_EXCH == Enumerations.Exchange.NSE.ToString())
                    {
                        ScripSymSelected = CommonFunctions.GetScripId((long)ScripSelectedCode, Enumerations.Exchange.NSE, Enumerations.Segment.Equity);//NSE Equity
                        if (ScripSymSelected != null && ScripSymSelected != string.Empty && ScripSymSelected != "")
                            ScripNameSelected = MasterSharedMemory.objMastertxtDictBaseNSE[(long)ScripSelectedCode].ScripName;
                        else
                            ScripSymSelected = string.Empty;
                    }

                }
                else
                {
                    ScripSymSelected = string.Empty;
                    ScripNameSelected = string.Empty;
                }
            }
            flag = true;
            //ErrorMsg = string.Empty;
        }

        private void OnChangeOfScripSym()
        {
            //if (scripIdChange)
            if (scripIdChange)
            {
                flag = false;
                if (ScripSymSelected != null && ScripSymSelected != "")
                {
                    //ScripSelectedCode = CommonFunctions.GetScripCode(ScripSymSelected);
                    //ScripNameSelected = MasterSharedMemory.objMastertxtDictBase[(long)ScripSelectedCode].ScripName;
                    if (Selected_EXCH == Enumerations.Exchange.BSE.ToString())
                    {
                        if (Selected_EXCH == Enumerations.Exchange.BSE.ToString())
                        {
                            ScripSelectedCode = CommonFunctions.GetScripCode(ScripSymSelected, Enumerations.Exchange.BSE, Enumerations.Segment.Equity);//BSE EQUITY
                            if (ScripSelectedCode != 0)
                            {
                                SubscribetoBcastMemory((int)ScripSelectedCode);
                                ScripNameSelected = MasterSharedMemory.objMastertxtDictBaseBSE[(long)ScripSelectedCode].ScripName;
                            }
                            else
                                ScripSelectedCode = 0;
                        }
                    }
                    else if (Selected_EXCH == Enumerations.Exchange.NSE.ToString())
                    {
                        ScripSelectedCode = CommonFunctions.GetScripCode(ScripSymSelected, Enumerations.Exchange.NSE, Enumerations.Segment.Equity);//NSE EQUITY
                        if (ScripSelectedCode != 0)
                            ScripNameSelected = MasterSharedMemory.objMastertxtDictBaseNSE[(long)ScripSelectedCode].ScripName;
                        else
                            ScripSelectedCode = 0;
                    }
                }
                else
                {
                    // ScripSelectedCode = null;
                    ScripNameSelected = string.Empty;
                    ScripSelectedCode = 0;
                }
            }
            scripIdChange = true;
            //ErrorMsg = string.Empty;

        }

        private void OnChangeofClientType()
        {
            if (ClientIDFlag)
            {
                ChangeShortClient = false;
                ClientName = string.Empty;
                if (clienttypeselected == Enumerations.Order.ClientTypes.OWN.ToString())
                {
                    clientinputEnabled = false;
                    ShortClientSelectedText = Enumerations.Order.ClientTypes.OWN.ToString();
                }
                else
                {
                    clientinputEnabled = true;
                    AssignShortClientTxtFromGlobal();
                    ChangeShortClient = false;
                    //AssignShortClientTxtChange();
                    //ShortClientSelectedText = string.Empty;
                }
                AssignClientTypeTxtChange();
                OnCheckClientTypeChange();
            }
            ClientIDFlag = true;
        }
        private void EnableDisableClientIdCPCode()
        {
            if (new[] { "Equity", "Debt" }.Any(x => x == ScripSelectedSegment))
            {
                IsClientIDCpCdEnabled = true;
            }
            else if (new[] { "Derivative", "Currency" }.Any(x => x == ScripSelectedSegment))
            {
                if (clienttypeselected == "SPLCLI" || clienttypeselected == "INST")
                {
                    IsClientIDCpCdEnabled = false;
                }
                else
                {
                    IsClientIDCpCdEnabled = true;
                }
            }

        }
        private void OnCheckClientTypeChange()
        {
            try
            {
                if (ChangeShortClient)
                {
                    ClientIDFlag = false;
                    if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Equity.ToString() || ScripSelectedSegment == Enumerations.Order.ScripSegment.Debt.ToString())
                    {
                        if (ClientIDinputlst != null && ClientIDinputlst.Contains(ShortClientSelectedText))
                        {
                            var count = MasterSharedMemory.objClientMasterDict.Values.Where(x => x.ShortClientId == ShortClientSelectedText).Count();
                            if (count > 0)
                            {
                                ClientName = MasterSharedMemory.objClientMasterDict.Values.Where(x => x.ShortClientId == ShortClientSelectedText).FirstOrDefault().CompleteClientId;
                                clienttypeselected = MasterSharedMemory.objClientMasterDict.Values.Where(x => x.ShortClientId == ShortClientSelectedText).FirstOrDefault().ClientType;
                            }
                            else if (ShortClientSelectedText == Enumerations.Order.ClientTypes.OWN.ToString())
                            {
                                clienttypeselected = Enumerations.Order.ClientTypes.OWN.ToString();
                                ClientName = string.Empty;
                            }
                            else
                            {
                                ClientName = string.Empty;
                            }
                        }
                        else
                        {
                            clienttypeselected = Enumerations.Order.ClientTypes.CLIENT.ToString();
                            ClientName = string.Empty;
                        }

                    }
                    else if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Derivative.ToString())
                    {
                        if (ClientIDinputlst.Contains(ShortClientSelectedText))
                        {
                            var count = MasterSharedMemory.objClientMasterDict.Values.Where(x => x.ShortClientId == ShortClientSelectedText).Count();
                            if (count > 0)
                            {
                                ClientName = MasterSharedMemory.objClientMasterDict.Values.Where(x => x.ShortClientId == ShortClientSelectedText).FirstOrDefault().CPCodeDerivative;
                                clienttypeselected = MasterSharedMemory.objClientMasterDict.Values.Where(x => x.ShortClientId == ShortClientSelectedText).FirstOrDefault().ClientType;
                            }
                            else if (ShortClientSelectedText == Enumerations.Order.ClientTypes.OWN.ToString())
                            {
                                clienttypeselected = Enumerations.Order.ClientTypes.OWN.ToString();
                                ClientName = string.Empty;
                            }
                            else
                            {
                                ClientName = string.Empty;
                            }
                        }
                        else
                        {
                            clienttypeselected = Enumerations.Order.ClientTypes.CLIENT.ToString();
                            ClientName = string.Empty;
                        }

                    }
                    else if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Currency.ToString())
                    {
                        if (ClientIDinputlst.Contains(ShortClientSelectedText))
                        {
                            var count = MasterSharedMemory.objClientMasterDict.Values.Where(x => x.ShortClientId == ShortClientSelectedText).Count();
                            if (count > 0)
                            {
                                ClientName = MasterSharedMemory.objClientMasterDict.Values.Where(x => x.ShortClientId == ShortClientSelectedText).FirstOrDefault().CPCodeCurrency;
                                clienttypeselected = MasterSharedMemory.objClientMasterDict.Values.Where(x => x.ShortClientId == ShortClientSelectedText).FirstOrDefault().ClientType;
                            }
                            else if (ShortClientSelectedText == Enumerations.Order.ClientTypes.OWN.ToString())
                            {
                                clienttypeselected = Enumerations.Order.ClientTypes.OWN.ToString();
                                ClientName = string.Empty;
                            }
                            else
                            {
                                ClientName = string.Empty;
                            }
                        }
                        else if (ShortClientSelectedText == Enumerations.Order.ClientTypes.OWN.ToString())
                        {
                            clienttypeselected = Enumerations.Order.ClientTypes.OWN.ToString();
                            ClientName = string.Empty;
                        }
                        else
                        {
                            clienttypeselected = Enumerations.Order.ClientTypes.CLIENT.ToString();
                            ClientName = string.Empty;
                        }

                    }

                    ErrorMsg = string.Empty;
                }
                ChangeShortClient = true;
            }
            catch (Exception ex)
            {

                ExceptionUtility.LogError(ex);
            }
        }

        public void PopulateOrderType()
        {
#if BOW

            OrderTypeSelected = Enumerations.Order.OrderTypes.LIMIT.ToString();
            OrderTypeList.Add(Enumerations.Order.OrderTypes.LIMIT.ToString());
            OrderTypeList.Add(Enumerations.Order.OrderTypes.MARKET.ToString());
            OrderTypeList.Add(Enumerations.Order.OrderTypes.STOPLOSS.ToString());
            OrderTypeList.Add(Enumerations.Order.OrderTypes.OCO.ToString());

            //   OrderTypeList.Add(Enumerations.Order.OrderTypes.STOPLOSS.ToString());
#endif

#if TWS
            var OrderTypes = Enum.GetNames(typeof(Enumerations.Order.OrderTypes));
            foreach (var item in OrderTypes)
            {
                OrderTypeList.Add(item);
            }
            OrderTypeSelected = OrderTypeList[0];
#endif

        }

        private void PopulateRetType()
        {
            RetType = new List<string>();
            RetTypeSelected = MainWindowVM.parserOS.GetSetting("GENERAL OS", "SelectedRetType");
            if (RetTypeSelected == null)
                RetTypeSelected = Enumerations.Order.RetType.EOS.ToString();

#if TWS
            RetType.Add(Enumerations.Order.RetType.EOS.ToString());
            RetType.Add(Enumerations.Order.RetType.EOD.ToString());
            RetType.Add(Enumerations.Order.RetType.IOC.ToString());
#elif BOW
            RetType.Add(Enumerations.Order.RetType.EOTODY.ToString());
            RetType.Add(Enumerations.Order.RetType.EOSTLM.ToString());
            RetType.Add(Enumerations.Order.RetType.EOS.ToString());

#endif

        }

        private void PopulateClientIdInput()
        {
            try
            {
                //if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Equity.ToString() || ScripSelectedSegment == Enumerations.Order.ScripSegment.Debt.ToString())
                //{
                ClientIDinputlst = new List<string>();

                if (MasterSharedMemory.objClientMasterDict != null && MasterSharedMemory.objClientMasterDict.Count > 0)
                    ClientIDinputlst = MasterSharedMemory.objClientMasterDict.OrderBy(x => x.Value.ShortClientId).Select(x => x.Value.ShortClientId).ToList();

                //ClientIDinputlst.Add(Enumerations.Order.ClientTypes.OWN.ToString());

                //if (ClientIDinputlst.Count() > 0)
                //{
                //    //ShortClientSelected = ClientIDinputlst[0];
                //    ClientName = MasterSharedMemory.objClientMasterDict.Where(x => x.Value.ShortClientId == ShortClientSelected).Select(y => y.Value.FirstName).First().ToString();
                //}
                //else
                //{
                //    ShortClientSelected = string.Empty;
                //}
                //}
                //else if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Derivative.ToString())
                //{
                //    ClientIDinputlst.Clear();

                //    if (MasterSharedMemory.objClientMasterDict != null && MasterSharedMemory.objClientMasterDict.Count > 0)
                //        ClientIDinputlst = MasterSharedMemory.objClientMasterDict.Select(x => x.Value.ShortClientId).ToList();

                //    //if (ClientIDinputlst.Count() > 0)
                //    //{
                //    //    //ShortClientSelected = ClientIDinputlst[0];
                //    //    ClientName = MasterSharedMemory.objClientMasterDict.Where(x => x.Value.ShortClientId == ShortClientSelected).Select(y => y.Value.CPCodeDerivative).First().ToString();
                //    //}
                //    //else
                //    //{
                //    //    ShortClientSelected = string.Empty;
                //    //}
                //}
                //else if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Currency.ToString())
                //{
                //    ClientIDinputlst.Clear();

                //    if (MasterSharedMemory.objClientMasterDict != null && MasterSharedMemory.objClientMasterDict.Count > 0)
                //        ClientIDinputlst = MasterSharedMemory.objClientMasterDict.Select(x => x.Value.ShortClientId).ToList();

                //    //if (ClientIDinputlst.Count() > 0)
                //    //{
                //    //    //ShortClientSelected = ClientIDinputlst[0];
                //    //    ClientName = MasterSharedMemory.objClientMasterDict.Where(x => x.Value.ShortClientId == ShortClientSelected).Select(y => y.Value.CPCodeDerivative).First().ToString();
                //    //}
                //    //else
                //    //{
                //    //    ShortClientSelected = string.Empty;
                //    //}
                //}



            }
            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
            }

        }

        private void PopulateCLientType()
        {
            Clienttypelst = new List<string>();
            clienttypeselected = Enumerations.Order.ClientTypes.CLIENT.ToString();
            //ShortClientSelectedText = String.Empty;
            Clienttypelst.Add(Enumerations.Order.ClientTypes.CLIENT.ToString());
            Clienttypelst.Add(Enumerations.Order.ClientTypes.SPLCLI.ToString());
            Clienttypelst.Add(Enumerations.Order.ClientTypes.INST.ToString());
            Clienttypelst.Add(Enumerations.Order.ClientTypes.OWN.ToString());

        }

        public void BuySellWindow(object e)
        {

            if (e.ToString().Trim().ToUpper() == "BUY")
            {
                BuyChecked = true;
                SellChecked = false;
                BuyVisible = "Visible";
                SellVisible = "Hidden";
                WindowColour = "#c8e3f7";
                BuySellInd = Enumerations.Order.BuySellFlag.B.ToString();
                if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Equity.ToString())
                {
                    HeaderTitle = string.Format("Order Entry EQX - Buy Limit: [{0:0.00} L], Sell Limit: [{1:0.00} L]", Limit.g_Limit[CommonFunctions.GetSegment(ScripSelectedSegment)].AvailGrossBuyLimit / 100000, Limit.g_Limit[CommonFunctions.GetSegment(ScripSelectedSegment)].AvailGrossSellLimit / 100000);
                }
                else if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Derivative.ToString())
                {
                    HeaderTitle = string.Format("Order Entry EDX - Buy Limit: [{0:0.00} L], Sell Limit: [{1:0.00} L]", Limit.g_Limit[CommonFunctions.GetSegment(ScripSelectedSegment)].AvailGrossBuyLimit / 100000, Limit.g_Limit[CommonFunctions.GetSegment(ScripSelectedSegment)].AvailGrossSellLimit / 100000);
                }
                else if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Debt.ToString())
                {
                    HeaderTitle = string.Format("Order Entry EQX - Buy Limit: [{0:0.00} L], Sell Limit: [{1:0.00} L]", Limit.g_Limit[CommonFunctions.GetSegment(ScripSelectedSegment)].AvailGrossBuyLimit / 100000, Limit.g_Limit[CommonFunctions.GetSegment(ScripSelectedSegment)].AvailGrossSellLimit / 100000);
                }
                else if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Currency.ToString())
                {
                    HeaderTitle = string.Format("Order Entry CDX - Buy Limit: [{0:0.00} L], Sell Limit: [{1:0.00} L]", Limit.g_Limit[CommonFunctions.GetSegment(ScripSelectedSegment)].AvailGrossBuyLimit / 100000, Limit.g_Limit[CommonFunctions.GetSegment(ScripSelectedSegment)].AvailGrossSellLimit / 100000);
                }
            }
            else if (e.ToString().Trim().ToUpper() == "SELL")
            {
                SellChecked = true;
                BuyChecked = false;
                SellVisible = "Visible";
                BuyVisible = "Hidden";
                WindowColour = "#ffd9d9";
                BuySellInd = Enumerations.Order.BuySellFlag.S.ToString();
                if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Equity.ToString())
                {
                    HeaderTitle = string.Format("Order Entry EQX - Buy Limit: [{0:0.00} L], Sell Limit: [{1:0.00} L]", Limit.g_Limit[CommonFunctions.GetSegment(ScripSelectedSegment)].AvailGrossBuyLimit / 100000, Limit.g_Limit[CommonFunctions.GetSegment(ScripSelectedSegment)].AvailGrossSellLimit / 100000);
                }
                else if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Derivative.ToString())
                {
                    HeaderTitle = string.Format("Order Entry EDX - Buy Limit: [{0:0.00} L], Sell Limit: [{1:0.00} L]", Limit.g_Limit[CommonFunctions.GetSegment(ScripSelectedSegment)].AvailGrossBuyLimit / 100000, Limit.g_Limit[CommonFunctions.GetSegment(ScripSelectedSegment)].AvailGrossSellLimit / 100000);
                }
                else if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Debt.ToString())
                {
                    HeaderTitle = string.Format("Order Entry EQX - Buy Limit: [{0:0.00} L], Sell Limit: [{1:0.00} L]", Limit.g_Limit[CommonFunctions.GetSegment(ScripSelectedSegment)].AvailGrossBuyLimit / 100000, Limit.g_Limit[CommonFunctions.GetSegment(ScripSelectedSegment)].AvailGrossSellLimit / 100000);
                }
                else if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Currency.ToString())
                {
                    HeaderTitle = string.Format("Order Entry CDX - Buy Limit: [{0:0.00} L], Sell Limit: [{1:0.00} L]", Limit.g_Limit[CommonFunctions.GetSegment(ScripSelectedSegment)].AvailGrossBuyLimit / 100000, Limit.g_Limit[CommonFunctions.GetSegment(ScripSelectedSegment)].AvailGrossSellLimit / 100000);
                }
            }
            ClearAll();
        }

        public void SetScripCodeandID()
        {
            if (UtilityOrderDetails.GETInstance.GlobalScripSelectedCode != 0 && (ScripSelectedSegment == Enumerations.Order.ScripSegment.Equity.ToString() || ScripSelectedSegment == Enumerations.Order.ScripSegment.Debt.ToString()))
            {
                ScripSelectedCode = UtilityOrderDetails.GETInstance.GlobalScripSelectedCode;
            }
            else if (UtilityOrderDetails.GETInstance.GlobalScripSelectedCodeDerv != 0 && (ScripSelectedSegment == Enumerations.Order.ScripSegment.Derivative.ToString() || ScripSelectedSegment == Enumerations.Order.ScripSegment.Currency.ToString()))
            {
                ScripCodeSelected = UtilityOrderDetails.GETInstance.GlobalScripSelectedCodeDerv;
            }
        }

        private void ClearAll()
        {
            // qty = String.Empty;
            //rate = String.Empty;
            //revQty = String.Empty;
            // MktPT = String.Empty;
            // ShortClientSelected = String.Empty;
            //RevChck = false;
            // OcoChecked = false;
        }

        private void GetFieldData()
        {
            try

            {
                //For Bse
                if (Selected_EXCH == Enumerations.Order.Exchanges.BSE.ToString())
                {
                    //For Bse
                    if (Selected_EXCH == Enumerations.Order.Exchanges.BSE.ToString())
                    {
                        //for equity market
                        if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Equity.ToString())
                        {
                            if (MasterSharedMemory.objMastertxtDictBaseBSE != null)
                            {
                                DecimalPoint = CommonFunctions.GetDecimal(Convert.ToInt32(ScripSelectedCode), Selected_EXCH, ScripSelectedSegment);
                                MarketLot = MasterSharedMemory.objMastertxtDictBaseBSE.Where(x => x.Key == ScripSelectedCode).Select(x => x.Value.MarketLot).FirstOrDefault();
                                string TickSizetemp = MasterSharedMemory.objMastertxtDictBaseBSE.Where(x => x.Key == ScripSelectedCode).Select(x => x.Value.TickSize).FirstOrDefault().ToString();
                                //TickSize = (Convert.ToDouble(TickSizetemp) / Math.Pow(10, DecimalPoint)).ToString();
                                if (DecimalPoint == 4)
                                    TickSize = string.Format("{0:0.0000}", (Convert.ToDouble(TickSizetemp) / Math.Pow(10, DecimalPoint)));
                                else
                                    TickSize = string.Format("{0:0.00}", (Convert.ToDouble(TickSizetemp) / Math.Pow(10, DecimalPoint)));
                                Group = CommonFunctions.GetGroupName(ScripSelectedCode, Enumerations.Exchange.BSE, Enumerations.Segment.Equity);
                                Series = "EQ";
                                long FaceValueTemp = MasterSharedMemory.objMastertxtDictBaseBSE.Where(x => x.Key == ScripSelectedCode).Select(x => x.Value.FaceValue).FirstOrDefault();
                                FaceValue = Convert.ToInt32(FaceValueTemp / Math.Pow(10, DecimalPoint));
                                Gsm = MasterSharedMemory.objMastertxtDictBaseBSE.Where(x => x.Key == ScripSelectedCode).Select(x => x.Value.Filler2_GSM).FirstOrDefault() != 100 ? MasterSharedMemory.objMastertxtDictBaseBSE.Where(x => x.Key == ScripSelectedCode).Select(x => x.Value.Filler2_GSM).FirstOrDefault().ToString() : string.Empty;
                            }
                            //if (VarMemory.SubscribeVarMemoryDict != null)
                            //{
                            //    try
                            //    {
                            //        VarEM = VarMemory.SubscribeVarMemoryDict.Where(x => x.Key == ScripSelectedCode).Select(x => x.Value.VarMainDetailsObj.Select(y => y.ELMPercentage).FirstOrDefault()).FirstOrDefault();
                            //        VarIM = VarMemory.SubscribeVarMemoryDict.Where(x => x.Key == ScripSelectedCode).Select(x => x.Value.VarMainDetailsObj.Select(y => y.IMPercentage).FirstOrDefault()).FirstOrDefault();
                            //    }
                            //    catch (Exception ex)
                            //    {

                            //        ExceptionUtility.LogError(ex);
                            //    }

                            //}
                            try
                            {
                                if (VarMemory.SubscribeVarMemoryDict != null)
                                {
                                    if (VarMemory.SubscribeVarMemoryDict.ContainsKey((int)ScripSelectedCode))
                                    {   //foreach (var item in VarMemory.SubscribeVarMemoryDict.Values)
                                        //{
                                        VarEM = string.Format("{0:0.00}", Convert.ToDouble(VarMemory.SubscribeVarMemoryDict[(int)ScripSelectedCode].ELMPercentage) / 100);
                                        //VarEM = VarMemory.SubscribeVarMemoryDict.Values.VarMainDetailsObj.;

                                        VarIM = string.Format("{0:0.00}", Convert.ToDouble(VarMemory.SubscribeVarMemoryDict[(int)ScripSelectedCode].IMPercentage) / 100);

                                    }
                                    else
                                    {
                                        VarEM = string.Empty;
                                        VarIM = string.Empty;
                                    }
                                    //}


                                    //var obj = VarMemory.SubscribeVarMemoryDict.Where(x => x.Key == ScripSelectedCode).Select(x => x.Value.VarMainDetailsObj).FirstOrDefault();
                                    //VarMain obj = new VarMain();
                                    //for (int i = 0; i < VarMemory.SubscribeVarMemoryDict.Count; i++)
                                    //{

                                    //    //if (VarMemory.SubscribeVarMemoryDict.Values.Contains(VarMainDetailsObj[i].))
                                    //    //{
                                    //    //    VarIM = VarMemory.SubscribeVarMemoryDict.VarMainDetailsObj[i].IMPercentage / 100;
                                    //    //    VarEM = VarMemory.SubscribeVarMemoryDict.VarMainDetailsObj[i].ELMPercentage / 100;
                                    //    //}
                                    //    if (VarMemory.SubscribeVarMemoryDict.VarMainDetailsObj[i].instr)
                                    //    {
                                    //        VarIM = VarMemory.SubscribeVarMemoryDict.VarMainDetailsObj[i].IMPercentage / 100;
                                    //        VarEM = VarMemory.SubscribeVarMemoryDict.VarMainDetailsObj[i].ELMPercentage / 100;
                                    //    }

                                    //    //if (obj[i].)
                                    //    //{

                                    //    //}
                                    //}

                                }
                            }

                            catch (Exception ex)
                            {
                                ExceptionUtility.LogError(ex);
                            }
                        }
                        //DERIVATIVE
                        if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Derivative.ToString())
                        {
                            if (MasterSharedMemory.objMasterDerivativeDictBaseBSE != null)
                            {
                                DecimalPoint = CommonFunctions.GetDecimal(Convert.ToInt32(ScripCodeSelected), Selected_EXCH, ScripSelectedSegment);
                                MarketLot = MasterSharedMemory.objMasterDerivativeDictBaseBSE.Where(x => x.Key == ScripCodeSelected).Select(x => x.Value.MinimumLotSize).FirstOrDefault();
                                string TickSizetemp = MasterSharedMemory.objMasterDerivativeDictBaseBSE.Where(x => x.Key == ScripCodeSelected).Select(x => x.Value.TickSize).FirstOrDefault().ToString();
                                //TickSize = (Convert.ToDouble(TickSizetemp) / Math.Pow(10, DecimalPoint)).ToString();
                                if (DecimalPoint == 4)
                                    TickSize = string.Format("{0:0.0000}", (Convert.ToDouble(TickSizetemp) / Math.Pow(10, DecimalPoint)));
                                else
                                    TickSize = string.Format("{0:0.00}", (Convert.ToDouble(TickSizetemp) / Math.Pow(10, DecimalPoint)));
                                Group = CommonFunctions.GetGroupName(ScripCodeSelected, Enumerations.Exchange.BSE, Enumerations.Segment.Derivative);
                                Gsm = string.Empty;
                                FaceValue = 0; // TODO: Get it from Broadcast
                                //Series = "EQ";
                            }
                        }
                        //bse currency
                        if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Currency.ToString())
                        {
                            if (MasterSharedMemory.objMasterCurrencyDictBaseBSE != null)
                            {
                                DecimalPoint = CommonFunctions.GetDecimal(Convert.ToInt32(ScripCodeSelected), Selected_EXCH, ScripSelectedSegment);
                                MarketLot = Convert.ToInt32(MasterSharedMemory.objMasterCurrencyDictBaseBSE.Where(x => x.Key == ScripCodeSelected).Select(x => x.Value.QuantityMultiplier).FirstOrDefault());
                                string TickSizetemp = MasterSharedMemory.objMasterCurrencyDictBaseBSE.Where(x => x.Key == ScripCodeSelected).Select(x => x.Value.TickSize).FirstOrDefault().ToString();
                                //TickSize = (Convert.ToDouble(TickSizetemp) / Math.Pow(10, DecimalPoint)).ToString();
                                if (DecimalPoint == 4)
                                    TickSize = string.Format("{0:0.0000}", (Convert.ToDouble(TickSizetemp) / Math.Pow(10, DecimalPoint)));
                                else
                                    TickSize = string.Format("{0:0.00}", (Convert.ToDouble(TickSizetemp) / Math.Pow(10, DecimalPoint)));

                                Group = "CD";
                                Gsm = string.Empty;
                                FaceValue = 0; // TODO: Get it from Broadcast
                                //Series = "EQ";
                            }
                        }
                        //bse Debt 
                        if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Debt.ToString())
                        {
                            if (MasterSharedMemory.objMastertxtDictBaseBSE != null)
                            {
                                DecimalPoint = CommonFunctions.GetDecimal(Convert.ToInt32(ScripSelectedCode), Selected_EXCH, ScripSelectedSegment);
                                MarketLot = MasterSharedMemory.objMastertxtDictBaseBSE.Where(x => x.Key == ScripSelectedCode).Select(x => x.Value.MarketLot).FirstOrDefault();
                                string TickSizetemp = MasterSharedMemory.objMastertxtDictBaseBSE.Where(x => x.Key == ScripSelectedCode).Select(x => x.Value.TickSize).FirstOrDefault().ToString();
                                //TickSize = (Convert.ToDouble(TickSizetemp) / Math.Pow(10, DecimalPoint)).ToString();
                                if (DecimalPoint == 4)
                                    TickSize = string.Format("{0:0.0000}", (Convert.ToDouble(TickSizetemp) / Math.Pow(10, DecimalPoint)));
                                else
                                    TickSize = string.Format("{0:0.00}", (Convert.ToDouble(TickSizetemp) / Math.Pow(10, DecimalPoint)));
                                Group = CommonFunctions.GetGroupName(ScripSelectedCode, Enumerations.Exchange.BSE, Enumerations.Segment.Equity);
                                Series = "EQ";
                                long FaceValueTemp = MasterSharedMemory.objMastertxtDictBaseBSE.Where(x => x.Key == ScripSelectedCode).Select(x => x.Value.FaceValue).FirstOrDefault();
                                FaceValue = Convert.ToInt32(FaceValueTemp / Math.Pow(10, DecimalPoint));
                                Gsm = MasterSharedMemory.objMastertxtDictBaseBSE.Where(x => x.Key == ScripSelectedCode).Select(x => x.Value.Filler2_GSM).FirstOrDefault() != 100 ? MasterSharedMemory.objMastertxtDictBaseBSE.Where(x => x.Key == ScripSelectedCode).Select(x => x.Value.Filler2_GSM).FirstOrDefault().ToString() : string.Empty;

                            }
                        }
                    }
                    //For NSE
                    //for NSE Exchange
                    if (Selected_EXCH == Enumerations.Order.Exchanges.NSE.ToString())
                    {
                        //for equity market
                        if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Equity.ToString())
                        {
                            if (MasterSharedMemory.objMastertxtDictBaseNSE != null)
                            {
                                MarketLot = MasterSharedMemory.objMastertxtDictBaseNSE.Where(x => x.Key == ScripSelectedCode).Select(x => x.Value.MarketLot).FirstOrDefault();
                                TickSize = MasterSharedMemory.objMastertxtDictBaseNSE.Where(x => x.Key == ScripSelectedCode).Select(x => x.Value.TickSize).FirstOrDefault().ToString();
                                Group = CommonFunctions.GetGroupName(ScripSelectedCode, Enumerations.Exchange.NSE, Enumerations.Segment.Equity);

                            }
                        }
                        //DERIVATIVE
                        if (Selected_EXCH == Enumerations.Order.Exchanges.NSE.ToString())
                        {
                            //for equity market
                            if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Derivative.ToString())
                            {
                                if (MasterSharedMemory.objMastertxtDictBaseNSE != null)
                                {
                                    MarketLot = MasterSharedMemory.objMastertxtDictBaseNSE.Where(x => x.Key == ScripSelectedCode).Select(x => x.Value.MarketLot).FirstOrDefault();
                                    TickSize = MasterSharedMemory.objMastertxtDictBaseNSE.Where(x => x.Key == ScripSelectedCode).Select(x => x.Value.TickSize).FirstOrDefault().ToString();
                                    Group = CommonFunctions.GetGroupName(ScripSelectedCode, Enumerations.Exchange.NSE, Enumerations.Segment.Derivative);


                                }
                            }
                            //currency nse
                            if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Derivative.ToString())
                            {
                                if (MasterSharedMemory.objMasterCurrencyDictBaseNSE != null)
                                {
                                    MarketLot = MasterSharedMemory.objMasterCurrencyDictBaseNSE.Where(x => x.Key == ScripSelectedCode).Select(x => x.Value.MinimumLotSize).FirstOrDefault();
                                    TickSize = MasterSharedMemory.objMasterCurrencyDictBaseNSE.Where(x => x.Key == ScripSelectedCode).Select(x => x.Value.TickSize).FirstOrDefault().ToString();
                                    Group = CommonFunctions.GetGroupName(ScripSelectedCode, Enumerations.Exchange.NSE, Enumerations.Segment.Currency);


                                }
                            }
                        }
                    }
                    //ncdex
                    if (Selected_EXCH == Enumerations.Order.Exchanges.NCDEX.ToString())
                    {
                        //for commodity market
                        if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Commodities.ToString())
                        {
                            if (MasterSharedMemory.objNCDEXMasterDict != null)
                            {
                                MarketLot = Convert.ToInt32(MasterSharedMemory.objNCDEXMasterDict.Where(x => x.Key == ScripSelectedCode).Select(x => x.Value.NCDDeliveryLotQuantity).FirstOrDefault());
                                TickSize = MasterSharedMemory.objNCDEXMasterDict.Where(x => x.Key == ScripSelectedCode).Select(x => x.Value.NCDTickSize).FirstOrDefault().ToString();
                                Group = CommonFunctions.GetGroupName(ScripSelectedCode, Enumerations.Exchange.NCDEX, Enumerations.Segment.Commodities);

                            }
                        }

                    }
                    //mcx
                    if (Selected_EXCH == Enumerations.Order.Exchanges.MCX.ToString())
                    {
                        //for commodity market  MCXMaster
                        if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Commodities.ToString())
                        {
                            if (MasterSharedMemory.objMCXMasterDict != null)
                            {
                                MarketLot = Convert.ToInt32(MasterSharedMemory.objMCXMasterDict.Where(x => x.Key == ScripSelectedCode).Select(x => x.Value.MCBoardLot).FirstOrDefault());
                                TickSize = MasterSharedMemory.objMCXMasterDict.Where(x => x.Key == ScripSelectedCode).Select(x => x.Value.MCPriceTick).FirstOrDefault().ToString();
                                Group = CommonFunctions.GetGroupName(ScripSelectedCode, Enumerations.Exchange.MCX, Enumerations.Segment.Commodities);
                                //field required by Manoj, usage yet to be confirmed
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {

                ExceptionUtility.LogError(ex);
            }

        }

        private void ModelCreation(string OrderType)
        {
            omodel = new OrderModel();
            if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Equity.ToString() || ScripSelectedSegment == Enumerations.Order.ScripSegment.Debt.ToString())
            {
                if (ScripSelectedCode == 0 || string.IsNullOrEmpty(ScripSymSelected))
                {
                    ErrorMsg = "ScripCode/ScripID is Invalid";
                    return;
                }

                DecimalPoint = CommonFunctions.GetDecimal(Convert.ToInt32(ScripSelectedCode), Selected_EXCH, ScripSelectedSegment);
                omodel.ScripCode = ScripSelectedCode;//532976;//500325;
                omodel.Symbol = ScripSymSelected;

            }

            else if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Derivative.ToString() || ScripSelectedSegment == Enumerations.Order.ScripSegment.Currency.ToString())
            {
                if (ScripCodeSelected == 0 || string.IsNullOrEmpty(ScripIDSelected))
                {
                    ErrorMsg = "ScripCode/ScripID is Invalid";
                    return;
                }

                DecimalPoint = CommonFunctions.GetDecimal(Convert.ToInt32(ScripCodeSelected), Selected_EXCH, ScripSelectedSegment);
                //omodel.ScripCode = ScripCodeSelected;
                //omodel.Symbol = ScripIDSelected;
            }

#if TWS
            omodel.Exchange = Selected_EXCH; //1- BSE, 2-BOW
            omodel.Segment = ScripSelectedSegment; //1 - Equity, 2 - Derv., 4.Curr
            omodel.SegmentFlag = CommonFunctions.SegmentFlag(omodel.Segment); //(int)Enum.Parse(typeof(Enumerations.Order.ScripSegment), ScripSelectedSegment);
            omodel.ClientType = clienttypeselected.ToUpper();
            //omodel.ClientId = ShortClientSelected.ToUpper();//Client Id
            if (omodel.ClientType == Enumerations.Order.ClientTypes.CLIENT.ToString() || omodel.ClientType == Enumerations.Order.ClientTypes.INST.ToString() || omodel.ClientType == Enumerations.Order.ClientTypes.SPLCLI.ToString())
            {
                omodel.ClientId = string.IsNullOrEmpty(ClientName) ? ShortClientSelectedText?.ToUpper() : ClientName;//Client Id
            }
            else if (omodel.ClientType == Enumerations.Order.ClientTypes.OWN.ToString())
            {
                omodel.ClientId = Enumerations.Order.ClientTypes.OWN.ToString();//in case of own
            }

            omodel.SenderLocationID = UtilityLoginDetails.GETInstance.SenderLocationId;//UtilityLoginDetails.GETInstance.SenderLocationId;
            if (BuySellInd.ToUpper() == Enumerations.Order.BuySellFlag.B.ToString())
            {
                omodel.BuySellIndicator = "B";
            }
            else if (BuySellInd.ToUpper() == Enumerations.Order.BuySellFlag.S.ToString())
            {
                omodel.BuySellIndicator = "S";
            }
            // omodel.BuySellIndicator = BuySellInd;//1-Buy/2-Sell as per BOW Buy is 0, sell is 1
            omodel.OrderRetentionStatus = RetTypeSelected;//0-IOC/1-SES/2-DAY

            //omodel.OrderRemarks = Remarks;

            omodel.OrderType = OrderType;


            if (omodel.OrderType == "G")// /MarketProtection
            {
                var protectionPercent = Convert.ToInt16(Convert.ToDecimal(MktPT) * 100);
                omodel.ProtectionPercentage = Convert.ToString(protectionPercent);
            }

            if (!string.IsNullOrEmpty(revQty))
                omodel.RevealQty = Convert.ToInt32(revQty);//Reveal QTY

            if (!string.IsNullOrEmpty(qty))
            {
                omodel.OriginalQty = Convert.ToInt32(qty);
                //pending qty should be equal to orginal qty while adding order. Gaurav Jadhav 23/2/2018
                omodel.PendingQuantity = omodel.OriginalQty;
            }

            //omodel.OriginalQty = omodel.OriginalQty;
            //omodel.RevealQty = omodel.RevealQty;
            omodel.ScreenId = (int)Enumerations.WindowName.Normal_OE;//"SwiftOrder01";

            if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Equity.ToString() || ScripSelectedSegment == Enumerations.Order.ScripSegment.Debt.ToString())
            {
                omodel.MarketLot = Convert.ToInt32(CommonFunctions.GetMarketLot((long)ScripSelectedCode));// MarketLot;
                omodel.TickSize = CommonFunctions.GetTickSize((long)ScripSelectedCode);//TickSize;
                omodel.Group = CommonFunctions.GetGroupName((long)ScripSelectedCode, "BSE", ScripSelectedSegment); //Group;
                omodel.PartitionID = Convert.ToUInt16(CommonFunctions.GetPartitionId((long)ScripSelectedCode, "BSE", ScripSelectedSegment));
                omodel.MarketSegmentID = CommonFunctions.GetProductId((long)ScripSelectedCode, "BSE", ScripSelectedSegment);
            }
            else if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Derivative.ToString() || ScripSelectedSegment == Enumerations.Order.ScripSegment.Currency.ToString())
            {
                omodel.MarketLot = Convert.ToInt32(CommonFunctions.GetMarketLot((long)ScripCodeSelected));// MarketLot;
                omodel.TickSize = CommonFunctions.GetTickSize((long)ScripCodeSelected);//TickSize;
                omodel.Group = CommonFunctions.GetGroupName((long)ScripCodeSelected, "BSE", ScripSelectedSegment); //Group;
                omodel.PartitionID = Convert.ToUInt16(CommonFunctions.GetPartitionId((long)ScripCodeSelected, "BSE", ScripSelectedSegment));
                omodel.MarketSegmentID = CommonFunctions.GetProductId((long)ScripCodeSelected, "BSE", ScripSelectedSegment);
            }


            omodel.ExecInst = Enumerations.Order.ExecInst.PersistentOrder.ToString();
            omodel.ParticipantCode = "";
            omodel.FreeText3 = "fdf";
            omodel.Filler_c = "fdf";

            //}
            if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Derivative.ToString() || ScripSelectedSegment == Enumerations.Order.ScripSegment.Currency.ToString())
            {

                omodel.ScripName = InstrNameSelected;
                omodel.ScripCode = ScripCodeSelected;
                omodel.Symbol = ScripIDSelected;//UnderAssetSelected;
            }

#endif
            //if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Derivative.ToString() || ScripSelectedSegment == Enumerations.Order.ScripSegment.Currency.ToString() || ScripSelectedSegment == Enumerations.Order.ScripSegment.Debt.ToString() || ScripSelectedSegment == Enumerations.Order.ScripSegment.Commodities.ToString() || ScripSelectedSegment == Enumerations.Order.ScripSegment.SLB.ToString() || ScripSelectedSegment == Enumerations.Order.ScripSegment.ITP.ToString())
            //{
            //    omodel.ScripName = ScripIDSelected;
            //    omodel.ScripCode = ScripCodeList[0];
            //    omodel.Symbol = UnderAssetSelected;
            //}
        }

        private void LimitSubmitButton_Click()
        {
            ModelCreation("L");
#if TWS
            omodel.OrderAction = Enumerations.Order.Modes.A.ToString();

#elif BOW
            omodel.Mode = Convert.ToInt32(Enumerations.Order.Modes.Add);
#endif
            if (omodel.OrderType == "L" && OcoChecked == true)
            {
                omodel.IsOCOOrder = true;
            }
            if (!string.IsNullOrEmpty(rate))
            {
                decimal rateInRs = Convert.ToDecimal(rate);
                rate = Convert.ToString(rateInRs);
            }
            bool validate = Validations.ValidateOrder(rate, trgPrice, omodel, ref Validate_Message, DecimalPoint);

            if (!validate)
            {
                ErrorMsg = Validate_Message;

                return;

            }

            bool GorupValidation = GroupAndGSMValidation();
            if (!GorupValidation)
            {
                //clear the fields
                return;
            }

            bool ValidateOrderSetting = OrderSettingValidationForQtyAndValue(omodel);

            if (!ValidateOrderSetting)
            {
                return;
            }
            else
            {
                //Convert Price in Long and send to processing
                omodel.Price = Convert.ToInt64(Convert.ToDouble(rate) * Math.Pow(10, DecimalPoint));

                if (!string.IsNullOrEmpty(trgPrice))
                    omodel.TriggerPrice = Convert.ToInt64(Convert.ToDouble(trgPrice) * Math.Pow(10, DecimalPoint));


                //Processor.Order.OrderProcessor.ProcessOrderObject(omodel);
                OrderRequestProcessor oOrderRequestProcessor = new OrderRequestProcessor(new AddOrder());
                oOrderRequestProcessor.ProcessRequest(omodel);
                MktPT = UtilityOrderDetails.GETInstance.MktProtection;
            }

        }

        private bool GroupAndGSMValidation()
        {
            if (ScripSelectedSegment?.ToLower() == Enumerations.Order.ScripSegment.Equity.ToString().ToLower() || ScripSelectedSegment?.ToLower() == Enumerations.Order.ScripSegment.Debt.ToString().ToLower())
            {
                if (Gsm != "100" && !string.IsNullOrEmpty(Gsm))
                {
                    if (Group.Trim().ToUpper() == "Z")
                    {
                        if (Gsm == "0")
                        {
                            if (MessageBox.Show("Please note that the scrip is in 'Z' group and under Graded Surveillance Measure (GSM). Trade would be settled on 'Trade to Trade' basis.\nFor more Info refer notice no. 20170223-44.\nDo you wish to continue?", "Warning", MessageBoxButton.YesNo) == MessageBoxResult.No)
                                return false;
                        }
                        else
                        {
                            if (MessageBox.Show(string.Format("Please note that the scrip is in 'Z' group and under Graded Surveillance Measure (GSM Stage {0}). Trade would be settled on 'Trade to Trade' basis. Additional deposit shall be applicable for buyer (Stage 2 onwards).\nFor more Info refer notice no. 20170223-44.\nDo you wish to continue?", Gsm), "Warning", MessageBoxButton.YesNo) == MessageBoxResult.No)
                                return false;
                        }

                    }
                    else if (Group.Trim().ToUpper() == "SS")
                    {
                        if (MessageBox.Show(string.Format("Scrip is in 'SS' Group & in 'S+' Framework under Stage {0}. Additional Weekly and Monthly Price Bands with transaction charges of 1% of transaction value applicable. \nDo you wish to continue?", Gsm), "Warning", MessageBoxButton.YesNo) == MessageBoxResult.No)
                            return false;
                    }
                    else if (Group.Trim().ToUpper() == "ST")
                    {
                        switch (Gsm)
                        {
                            case "0":
                            case "1":
                                if (MessageBox.Show(string.Format("Scrip is in 'ST' Group & in 'S+' Framework under Stage {0}. Additional Weekly and Monthly Price Bands with transaction charges of 1% of transaction value applicable & in Trade to Trade. \nDo you wish to continue?", Gsm), "Warning", MessageBoxButton.YesNo) == MessageBoxResult.No)
                                    return false;
                                break;
                            case "2":
                                if (MessageBox.Show(string.Format("Scrip is in 'ST' Group & in 'S+' Framework under Stage {0}. Additional Weekly and Monthly Price Bands with transaction charges of 1% of transaction value, ASD of 200% of buy value applicable & in Trade to Trade. \nDo you wish to continue?", Gsm), "Warning", MessageBoxButton.YesNo) == MessageBoxResult.No)
                                    return false;
                                break;
                        }
                    }
                    else if (Group.Trim().ToUpper().Contains("T")) //T Group
                    {
                        if (Gsm == "0")
                        {
                            if (MessageBox.Show(string.Format("Please note that the scrip is in {0} group and under Graded Surveillance Measure (GSM).  Trade would be settled on 'Trade to Trade' basis.\nFor more Info refer notice no. 20170223-44.\nDo you wish to continue?", Group.Trim().ToUpper()), "Warning", MessageBoxButton.YesNo) == MessageBoxResult.No)
                                return false;
                        }
                        else
                        {
                            if (MessageBox.Show(string.Format("Please note that the scrip is in {0} group and under Graded Surveillance Measure (GSM Stage {1}). Trade would be settled on 'Trade to Trade' basis. Additional deposit shall be applicable for buyer (Stage 2 onwards).\nFor more Info refer notice no. 20170223-44.\nDo you wish to continue?", Group.Trim().ToUpper(), Gsm), "Warning", MessageBoxButton.YesNo) == MessageBoxResult.No)
                                return false;
                        }
                    }
                    else
                    {
                        if (Gsm == "0")
                        {
                            if (MessageBox.Show("Please note that the Scrip under Graded Surveillance Measure (GSM).\nFor more Info refer notice no. 20170223-44.\nDo you wish to continue?", "Warning", MessageBoxButton.YesNo) == MessageBoxResult.No)
                                return false;
                        }
                        else
                        {
                            if (MessageBox.Show(string.Format("Please note that the Scrip under Graded Surveillance Measure (Stage {0}). Trade would be settled on 'Trade to Trade' basis. Additional deposit shall be applicable for buyer (Stage 2 onwards).\nFor more Info refer notice no. 20170223-44.\nDo you wish to continue?", Gsm), "Warning", MessageBoxButton.YesNo) == MessageBoxResult.No)
                                return false;
                        }

                    }
                }
                else if (Group.Trim().ToUpper() == "Z")
                {
                    if (MessageBox.Show("Please note that the Scrip is in 'Z' GROUP and Trades would be settled on\n'Trade to Trade' basis\nDo you wish to continue?", "Warning", MessageBoxButton.YesNo) == MessageBoxResult.No)
                        return false;
                }

                else if (Group.Contains("T"))
                {
                    if (MessageBox.Show(string.Format("Please note that the Scrip is in '{0}' GROUP and Trades would be settled on\n'Trade to Trade' basis\nDo you wish to continue?", Group), "Warning", MessageBoxButton.YesNo) == MessageBoxResult.No)
                        return false;
                }
                else if (Group.Contains("GC") || Group.Contains("FC"))
                {
                    MessageBox.Show(string.Format("Please note that the Scrip is in '{0}' GROUP and Orders are not allowed for this group?", Group), "Warning", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }

            }

            return true;
        }

        private void MarketSubmitButton_Click()
        {
            ModelCreation("G");
#if TWS
            omodel.OrderAction = Enumerations.Order.Modes.A.ToString();


#elif BOW
            omodel.Mode = Convert.ToInt32(Enumerations.Order.Modes.Add);
#endif
            if (!string.IsNullOrEmpty(rate))
            {
                decimal rateInRs = Convert.ToDecimal(rate);
                rate = Convert.ToString(rateInRs);
            }
            bool validate = Validations.ValidateOrder(rate, trgPrice, omodel, ref Validate_Message, DecimalPoint);

            if (!validate)
            {
                ErrorMsg = Validate_Message;

                return;

            }

            bool GorupValidation = GroupAndGSMValidation();
            if (!GorupValidation)
            {
                //clear the fields
                return;
            }
            bool ValidateOrderSetting = OrderSettingValidationForQtyAndValue(omodel);

            if (!ValidateOrderSetting)
            {
                return;
            }
            else
            {
                //Convert Price in Long and send to processing
                //omodel.Price = Convert.ToInt64(Convert.ToDouble(rate) * Math.Pow(10, DecimalPoint));
                omodel.Price = 0;


                if (!string.IsNullOrEmpty(trgPrice))
                    omodel.TriggerPrice = Convert.ToInt64(Convert.ToDouble(trgPrice) * Math.Pow(10, DecimalPoint));


                //Processor.Order.OrderProcessor.ProcessOrderObject(omodel);
                OrderRequestProcessor oOrderRequestProcessor = new OrderRequestProcessor(new AddOrder());
                oOrderRequestProcessor.ProcessRequest(omodel);
                MktPT = UtilityOrderDetails.GETInstance.MktProtection;
            }

        }

        private void BlockDealSubmitButton_Click()
        {
            ModelCreation("K");
#if TWS
            omodel.OrderAction = Enumerations.Order.Modes.A.ToString();
#elif BOW
            omodel.Mode = Convert.ToInt32(Enumerations.Order.Modes.Add);
#endif
            bool validate = Validations.ValidateOrder(rate, trgPrice, omodel, ref Validate_Message, DecimalPoint);

            if (!validate)
            {
                ErrorMsg = Validate_Message;

                return;

            }

            bool ValidateOrderSetting = OrderSettingValidationForQtyAndValue(omodel);

            if (!ValidateOrderSetting)
            {
                return;
            }
            else
            {
                //Convert Price in Long and send to processing
                omodel.Price = Convert.ToInt64(Convert.ToDouble(rate) * Math.Pow(10, DecimalPoint));

                if (!string.IsNullOrEmpty(trgPrice))
                    omodel.TriggerPrice = Convert.ToInt64(Convert.ToDouble(trgPrice) * Math.Pow(10, DecimalPoint));
                //Processor.Order.OrderProcessor.ProcessOrderObject(omodel);
                OrderRequestProcessor oOrderRequestProcessor = new OrderRequestProcessor(new AddOrder());
                oOrderRequestProcessor.ProcessRequest(omodel);
                MktPT = UtilityOrderDetails.GETInstance.MktProtection;
                //ErrorMsg = "Order Placed Successfully";
                //ack needed 
                //WindowColour = "#00802b";
                // HeaderTitle = "Order Placed Successfully";
                //SuccessfulMsgControl win = System.Windows.Application.Current.Windows.OfType<SuccessfulMsgControl>().FirstOrDefault();
                //SwiftOrderEntry swiftwin = System.Windows.Application.Current.Windows.OfType<SwiftOrderEntry>().FirstOrDefault();
                //win = new SuccessfulMsgControl();
                //win.Owner = swiftwin;
                //win.Show();
            }
        }

        private void CloseWindowsOnEscape_Click()
        {
            mWindow = System.Windows.Application.Current.Windows.OfType<View.Order.NormalOrderEntry>().FirstOrDefault();
            mWindow?.Close();
            //if (MemoryManager.InvokeWindowOnScripCodeSelection != null)
            //{
            //    MemoryManager.InvokeWindowOnScripCodeSelection -= PopulateOrderEntryWindow;
            //}
        }

        private void PopulatingShortClientID_SegmentWise(object e)
        {
            if (e != null)
            {
                System.Windows.Controls.ComboBox cbShortClientId = new System.Windows.Controls.ComboBox();
                cbShortClientId = e as System.Windows.Controls.ComboBox;
                //if (ShortClientIDSelectionCheck?.ToLower() == Boolean.TrueString.ToLower())
                //{
                //    if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Equity.ToString())
                //    {
                //        UtilityOrderDetails.GETInstance.EqtShortClientID = Convert.ToString(cbShortClientId.SelectedValue);
                //    }
                //    else if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Debt.ToString())
                //    {
                //        UtilityOrderDetails.GETInstance.DebtShortClientID = Convert.ToString(cbShortClientId.SelectedValue);
                //    }
                //    else if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Derivative.ToString())
                //    {
                //        UtilityOrderDetails.GETInstance.DervShortClientID = Convert.ToString(cbShortClientId.SelectedValue);
                //    }
                //    else if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Currency.ToString())
                //    {
                //        UtilityOrderDetails.GETInstance.CurrShortClientID = Convert.ToString(cbShortClientId.SelectedValue);
                //    }
                //}
            }
        }
        /// <summary>
        /// On Segment Change. Assign ClientType from global if available
        /// </summary>
        private void AssignClientTypeFromGlobal()
        {
            if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Equity.ToString())
            {
                if (UtilityOrderDetails.GETInstance.EQClientType == Enumerations.Order.ClientTypes.CLIENT.ToString())
                {
                    clienttypeselected = Enumerations.Order.ClientTypes.CLIENT.ToString();
                }
                else if (UtilityOrderDetails.GETInstance.EQClientType == Enumerations.Order.ClientTypes.INST.ToString())
                {
                    clienttypeselected = Enumerations.Order.ClientTypes.INST.ToString();
                }
                else if (UtilityOrderDetails.GETInstance.EQClientType == Enumerations.Order.ClientTypes.OWN.ToString())
                {
                    clienttypeselected = Enumerations.Order.ClientTypes.OWN.ToString();
                }
                else if (UtilityOrderDetails.GETInstance.EQClientType == Enumerations.Order.ClientTypes.SPLCLI.ToString())
                {
                    clienttypeselected = Enumerations.Order.ClientTypes.SPLCLI.ToString();
                }
                else
                {
                    clienttypeselected = Enumerations.Order.ClientTypes.CLIENT.ToString();
                }
            }
            else if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Derivative.ToString())
            {
                if (UtilityOrderDetails.GETInstance.DERClientType == Enumerations.Order.ClientTypes.CLIENT.ToString())
                {
                    clienttypeselected = Enumerations.Order.ClientTypes.CLIENT.ToString();
                }
                else if (UtilityOrderDetails.GETInstance.DERClientType == Enumerations.Order.ClientTypes.INST.ToString())
                {
                    clienttypeselected = Enumerations.Order.ClientTypes.INST.ToString();
                }
                else if (UtilityOrderDetails.GETInstance.DERClientType == Enumerations.Order.ClientTypes.OWN.ToString())
                {
                    clienttypeselected = Enumerations.Order.ClientTypes.OWN.ToString();
                }
                else if (UtilityOrderDetails.GETInstance.DERClientType == Enumerations.Order.ClientTypes.SPLCLI.ToString())
                {
                    clienttypeselected = Enumerations.Order.ClientTypes.SPLCLI.ToString();
                }
                else
                {
                    clienttypeselected = Enumerations.Order.ClientTypes.CLIENT.ToString();
                }
            }
            else if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Currency.ToString())
            {
                if (UtilityOrderDetails.GETInstance.CURClientType == Enumerations.Order.ClientTypes.CLIENT.ToString())
                {
                    clienttypeselected = Enumerations.Order.ClientTypes.CLIENT.ToString();
                }
                else if (UtilityOrderDetails.GETInstance.CURClientType == Enumerations.Order.ClientTypes.INST.ToString())
                {
                    clienttypeselected = Enumerations.Order.ClientTypes.INST.ToString();
                }
                else if (UtilityOrderDetails.GETInstance.CURClientType == Enumerations.Order.ClientTypes.OWN.ToString())
                {
                    clienttypeselected = Enumerations.Order.ClientTypes.OWN.ToString();
                }
                else if (UtilityOrderDetails.GETInstance.CURClientType == Enumerations.Order.ClientTypes.SPLCLI.ToString())
                {
                    clienttypeselected = Enumerations.Order.ClientTypes.SPLCLI.ToString();
                }
                else
                {
                    clienttypeselected = Enumerations.Order.ClientTypes.CLIENT.ToString();
                }
            }
            else if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Debt.ToString())
            {
                if (UtilityOrderDetails.GETInstance.DEBTClientType == Enumerations.Order.ClientTypes.CLIENT.ToString())
                {
                    clienttypeselected = Enumerations.Order.ClientTypes.CLIENT.ToString();
                }
                else if (UtilityOrderDetails.GETInstance.DEBTClientType == Enumerations.Order.ClientTypes.INST.ToString())
                {
                    clienttypeselected = Enumerations.Order.ClientTypes.INST.ToString();
                }
                else if (UtilityOrderDetails.GETInstance.DEBTClientType == Enumerations.Order.ClientTypes.OWN.ToString())
                {
                    clienttypeselected = Enumerations.Order.ClientTypes.OWN.ToString();
                }
                else if (UtilityOrderDetails.GETInstance.DEBTClientType == Enumerations.Order.ClientTypes.SPLCLI.ToString())
                {
                    clienttypeselected = Enumerations.Order.ClientTypes.SPLCLI.ToString();
                }
                else
                {
                    clienttypeselected = Enumerations.Order.ClientTypes.CLIENT.ToString();
                }
            }
        }
        /// <summary>
        ///On Client Type Change. Manage client type along with  short client for each segment.
        /// </summary>
        private void AssignClientTypeTxtChange()
        {
            if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Equity.ToString())
            {
                if (clienttypeselected == Enumerations.Order.ClientTypes.CLIENT.ToString())
                {
                    UtilityOrderDetails.GETInstance.EQClientType = Enumerations.Order.ClientTypes.CLIENT.ToString();
                }
                else if (clienttypeselected == Enumerations.Order.ClientTypes.INST.ToString())
                {
                    UtilityOrderDetails.GETInstance.EQClientType = Enumerations.Order.ClientTypes.INST.ToString();
                }
                else if (clienttypeselected == Enumerations.Order.ClientTypes.OWN.ToString())
                {
                    UtilityOrderDetails.GETInstance.EQClientType = Enumerations.Order.ClientTypes.OWN.ToString();
                }
                else if (clienttypeselected == Enumerations.Order.ClientTypes.SPLCLI.ToString())
                {
                    UtilityOrderDetails.GETInstance.EQClientType = Enumerations.Order.ClientTypes.SPLCLI.ToString();
                }
            }
            else if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Derivative.ToString())
            {
                if (clienttypeselected == Enumerations.Order.ClientTypes.CLIENT.ToString())
                {
                    UtilityOrderDetails.GETInstance.DERClientType = Enumerations.Order.ClientTypes.CLIENT.ToString();
                }
                else if (clienttypeselected == Enumerations.Order.ClientTypes.INST.ToString())
                {
                    UtilityOrderDetails.GETInstance.DERClientType = Enumerations.Order.ClientTypes.INST.ToString();
                }
                else if (clienttypeselected == Enumerations.Order.ClientTypes.OWN.ToString())
                {
                    UtilityOrderDetails.GETInstance.DERClientType = Enumerations.Order.ClientTypes.OWN.ToString();
                }
                else if (clienttypeselected == Enumerations.Order.ClientTypes.SPLCLI.ToString())
                {
                    UtilityOrderDetails.GETInstance.DERClientType = Enumerations.Order.ClientTypes.SPLCLI.ToString();
                }
            }
            else if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Currency.ToString())
            {
                if (clienttypeselected == Enumerations.Order.ClientTypes.CLIENT.ToString())
                {
                    UtilityOrderDetails.GETInstance.CURClientType = Enumerations.Order.ClientTypes.CLIENT.ToString();
                }
                else if (clienttypeselected == Enumerations.Order.ClientTypes.INST.ToString())
                {
                    UtilityOrderDetails.GETInstance.CURClientType = Enumerations.Order.ClientTypes.INST.ToString();
                }
                else if (clienttypeselected == Enumerations.Order.ClientTypes.OWN.ToString())
                {
                    UtilityOrderDetails.GETInstance.CURClientType = Enumerations.Order.ClientTypes.OWN.ToString();
                }
                else if (clienttypeselected == Enumerations.Order.ClientTypes.SPLCLI.ToString())
                {
                    UtilityOrderDetails.GETInstance.CURClientType = Enumerations.Order.ClientTypes.SPLCLI.ToString();
                }
            }
            else if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Debt.ToString())
            {
                if (clienttypeselected == Enumerations.Order.ClientTypes.CLIENT.ToString())
                {
                    UtilityOrderDetails.GETInstance.DEBTClientType = Enumerations.Order.ClientTypes.CLIENT.ToString();
                }
                else if (clienttypeselected == Enumerations.Order.ClientTypes.INST.ToString())
                {
                    UtilityOrderDetails.GETInstance.DEBTClientType = Enumerations.Order.ClientTypes.INST.ToString();
                }
                else if (clienttypeselected == Enumerations.Order.ClientTypes.OWN.ToString())
                {
                    UtilityOrderDetails.GETInstance.DEBTClientType = Enumerations.Order.ClientTypes.OWN.ToString();
                }
                else if (clienttypeselected == Enumerations.Order.ClientTypes.SPLCLI.ToString())
                {
                    UtilityOrderDetails.GETInstance.DEBTClientType = Enumerations.Order.ClientTypes.SPLCLI.ToString();
                }
            }
        }
        /// <summary>
        /// Assign ShortClientTxt change to global variable. Added by Gaurav Jadhav 22/3/2018
        /// </summary>
        private void AssignShortClientTxtChange()
        {
            if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Equity.ToString())
            {
                if (ShortClientIDEquityCheck == bool.TrueString || UtilityOrderDetails.GETInstance.IsEqtShortClientIDChecked == "1")
                {
                    //UtilityOrderDetails.GETInstance.IsEqtShortClientIDChecked = "1";
                    UtilityOrderDetails.GETInstance.EqtShortClientID = ShortClientSelectedText;
                }
                else
                {
                    //UtilityOrderDetails.GETInstance.IsEqtShortClientIDChecked = "0";
                    UtilityOrderDetails.GETInstance.EqtShortClientID = string.Empty;
                }

            }
            else if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Debt.ToString())
            {
                if (ShortClientIDDebtCheck == bool.TrueString || UtilityOrderDetails.GETInstance.IsDebtShortClientIDChecked == "1")
                {
                    //UtilityOrderDetails.GETInstance.IsDebtShortClientIDChecked = "1";
                    UtilityOrderDetails.GETInstance.DebtShortClientID = ShortClientSelectedText;
                }
                else
                {
                    //UtilityOrderDetails.GETInstance.IsDebtShortClientIDChecked = "0";
                    UtilityOrderDetails.GETInstance.DebtShortClientID = string.Empty;
                }
            }
            else if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Derivative.ToString())
            {
                if (ShortClientIDDerCheck == bool.TrueString || UtilityOrderDetails.GETInstance.IsDervShortClientIDChecked == "1")
                {
                    //UtilityOrderDetails.GETInstance.IsDervShortClientIDChecked = "1";
                    UtilityOrderDetails.GETInstance.DervShortClientID = ShortClientSelectedText;
                }
                else
                {
                    //UtilityOrderDetails.GETInstance.IsDervShortClientIDChecked = "0";
                    UtilityOrderDetails.GETInstance.DervShortClientID = string.Empty;
                }
            }

            else if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Currency.ToString())
            {
                if (ShortClientIDCurCheck == bool.TrueString || UtilityOrderDetails.GETInstance.IsCurrShortClientIDChecked == "1")
                {
                    //UtilityOrderDetails.GETInstance.IsCurrShortClientIDChecked = "1";
                    UtilityOrderDetails.GETInstance.CurrShortClientID = ShortClientSelectedText;
                }
                else
                {
                    //UtilityOrderDetails.GETInstance.IsCurrShortClientIDChecked = "0";
                    UtilityOrderDetails.GETInstance.CurrShortClientID = string.Empty;
                }
            }

        }
        /// <summary>
        /// Check uncheck Short Client Checkbox based on Global flag
        /// </summary>
        /// <param name="segment"></param>
        /// <param name="checkUncheckFlag"></param>
        private void ShortClientIDCheck(string segment, string checkUncheckFlag)
        {
            if (segment == Enumerations.Order.ScripSegment.Equity.ToString())
            {
                if (checkUncheckFlag == "1")
                    ShortClientIDEquityCheck = Boolean.TrueString;
                else
                    ShortClientIDEquityCheck = Boolean.FalseString;
            }
            else if (segment == Enumerations.Order.ScripSegment.Derivative.ToString())
            {
                if (checkUncheckFlag == "1")
                    ShortClientIDDerCheck = Boolean.TrueString;
                else
                    ShortClientIDDerCheck = Boolean.FalseString;
            }
            else if (segment == Enumerations.Order.ScripSegment.Debt.ToString())
            {
                if (checkUncheckFlag == "1")
                    ShortClientIDDebtCheck = Boolean.TrueString;
                else
                    ShortClientIDDebtCheck = Boolean.FalseString;
            }
            else if (segment == Enumerations.Order.ScripSegment.Currency.ToString())
            {
                if (checkUncheckFlag == "1")
                    ShortClientIDCurCheck = Boolean.TrueString;
                else
                    ShortClientIDCurCheck = Boolean.FalseString;
            }
        }
        private void AssignShortClientTxtFromGlobal()
        {
            if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Equity.ToString())
            {
                if (ShortClientIDEquityCheck == bool.TrueString || UtilityOrderDetails.GETInstance.IsEqtShortClientIDChecked == "1")
                {
                    //UtilityOrderDetails.GETInstance.IsEqtShortClientIDChecked = "1";
                    ShortClientSelectedText = UtilityOrderDetails.GETInstance.EqtShortClientID;
                }
                else
                {
                    //    ShortClientSelectedText = string.Empty;
                }
                ShortClientIDCheck(ScripSelectedSegment, UtilityOrderDetails.GETInstance.IsEqtShortClientIDChecked);
            }
            else if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Debt.ToString())
            {
                if (ShortClientIDDebtCheck == bool.TrueString || UtilityOrderDetails.GETInstance.IsDebtShortClientIDChecked == "1")
                {
                    //UtilityOrderDetails.GETInstance.IsDebtShortClientIDChecked = "1";
                    ShortClientSelectedText = UtilityOrderDetails.GETInstance.DebtShortClientID;
                }
                else
                {
                    //   ShortClientSelectedText = string.Empty;
                }
                ShortClientIDCheck(ScripSelectedSegment, UtilityOrderDetails.GETInstance.IsDebtShortClientIDChecked);
            }
            else if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Derivative.ToString())
            {
                if (ShortClientIDDerCheck == bool.TrueString || UtilityOrderDetails.GETInstance.IsDervShortClientIDChecked == "1")
                {
                    //UtilityOrderDetails.GETInstance.IsDervShortClientIDChecked = "1";
                    ShortClientSelectedText = UtilityOrderDetails.GETInstance.DervShortClientID;
                }
                else
                {
                    //   ShortClientSelectedText = string.Empty;
                }
                ShortClientIDCheck(ScripSelectedSegment, UtilityOrderDetails.GETInstance.IsDervShortClientIDChecked);
            }

            else if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Currency.ToString())
            {
                if (ShortClientIDCurCheck == bool.TrueString || UtilityOrderDetails.GETInstance.IsCurrShortClientIDChecked == "1")
                {
                    //UtilityOrderDetails.GETInstance.IsCurrShortClientIDChecked = "1";
                    ShortClientSelectedText = UtilityOrderDetails.GETInstance.CurrShortClientID;
                }
                else
                {
                    //   ShortClientSelectedText = string.Empty;
                }
                ShortClientIDCheck(ScripSelectedSegment, UtilityOrderDetails.GETInstance.IsCurrShortClientIDChecked);
            }

            //default
            if (ShortClientSelectedText == Enumerations.Order.ClientTypes.OWN.ToString() &&
                clienttypeselected == Enumerations.Order.ClientTypes.CLIENT.ToString())
            {
                ShortClientSelectedText = string.Empty;
                if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Equity.ToString())
                    UtilityOrderDetails.GETInstance.EqtShortClientID = string.Empty;

                else if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Debt.ToString())
                    UtilityOrderDetails.GETInstance.DebtShortClientID = string.Empty;

                else if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Derivative.ToString())
                    UtilityOrderDetails.GETInstance.DervShortClientID = string.Empty;

                else if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Currency.ToString())
                    UtilityOrderDetails.GETInstance.CurrShortClientID = string.Empty;
            }

        }
        private void OnChangeofShortClientTxt()
        {

            try
            {
                if (ChangeShortClient)
                {
                    ClientIDFlag = false;
                    if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Equity.ToString() || ScripSelectedSegment == Enumerations.Order.ScripSegment.Debt.ToString())
                    {
                        if (ClientIDinputlst.Contains(ShortClientSelectedText))
                        {
                            var count = MasterSharedMemory.objClientMasterDict.Values.Where(x => x.ShortClientId == ShortClientSelectedText || x.CompleteClientId == ShortClientSelectedText).Count();
                            if (count > 0)
                            {
                                ClientName = MasterSharedMemory.objClientMasterDict.Values.Where(x => x.ShortClientId == ShortClientSelectedText || x.CompleteClientId == ShortClientSelectedText).FirstOrDefault().CompleteClientId;
                                clienttypeselected = MasterSharedMemory.objClientMasterDict.Values.Where(x => x.ShortClientId == ShortClientSelectedText).FirstOrDefault().ClientType;
                            }
                            else if (ShortClientSelectedText == Enumerations.Order.ClientTypes.OWN.ToString())
                            {
                                clienttypeselected = Enumerations.Order.ClientTypes.OWN.ToString();
                                ClientName = string.Empty;
                                clientinputEnabled = false;
                            }
                            else
                            {
                                ClientName = string.Empty;
                            }
                        }
                        else if (ShortClientSelectedText == Enumerations.Order.ClientTypes.OWN.ToString())
                        {
                            clienttypeselected = Enumerations.Order.ClientTypes.OWN.ToString();
                            ClientName = string.Empty;
                            clientinputEnabled = false;
                        }
                        else
                        {
                            //clienttypeselected = Enumerations.Order.ClientTypes.CLIENT.ToString();
                            ClientName = string.Empty;
                        }

                    }
                    else if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Derivative.ToString())
                    {
                        if (ClientIDinputlst.Contains(ShortClientSelectedText))
                        {
                            var count = MasterSharedMemory.objClientMasterDict.Values.Where(x => x.ShortClientId == ShortClientSelectedText).Count();
                            if (count > 0)
                            {
                                ClientName = MasterSharedMemory.objClientMasterDict.Values.Where(x => x.ShortClientId == ShortClientSelectedText).FirstOrDefault().CPCodeDerivative;
                                clienttypeselected = MasterSharedMemory.objClientMasterDict.Values.Where(x => x.ShortClientId == ShortClientSelectedText).FirstOrDefault().ClientType;
                            }
                            else if (ShortClientSelectedText == Enumerations.Order.ClientTypes.OWN.ToString())
                            {
                                clienttypeselected = Enumerations.Order.ClientTypes.OWN.ToString();
                                ClientName = string.Empty;
                                clientinputEnabled = false;
                            }
                            else
                            {
                                ClientName = string.Empty;
                            }
                        }
                        else if (ShortClientSelectedText == Enumerations.Order.ClientTypes.OWN.ToString())
                        {
                            clienttypeselected = Enumerations.Order.ClientTypes.OWN.ToString();
                            ClientName = string.Empty;
                            clientinputEnabled = false;
                        }
                        else
                        {
                            clienttypeselected = Enumerations.Order.ClientTypes.CLIENT.ToString();
                            ClientName = string.Empty;
                        }
                    }
                    else if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Currency.ToString())
                    {
                        if (ClientIDinputlst.Contains(ShortClientSelectedText))
                        {
                            var count = MasterSharedMemory.objClientMasterDict.Values.Where(x => x.ShortClientId == ShortClientSelectedText).Count();
                            if (count > 0)
                            {
                                ClientName = MasterSharedMemory.objClientMasterDict.Values.Where(x => x.ShortClientId == ShortClientSelectedText).FirstOrDefault().CPCodeCurrency;
                                clienttypeselected = MasterSharedMemory.objClientMasterDict.Values.Where(x => x.ShortClientId == ShortClientSelectedText).FirstOrDefault().ClientType;
                            }
                            else if (ShortClientSelectedText == Enumerations.Order.ClientTypes.OWN.ToString())
                            {
                                clienttypeselected = Enumerations.Order.ClientTypes.OWN.ToString();
                                ClientName = string.Empty;
                                clientinputEnabled = false;
                            }
                            else
                            {
                                ClientName = string.Empty;
                            }
                        }
                        else if (ShortClientSelectedText == Enumerations.Order.ClientTypes.OWN.ToString())
                        {
                            clienttypeselected = Enumerations.Order.ClientTypes.OWN.ToString();
                            ClientName = string.Empty;
                            clientinputEnabled = false;
                        }
                        else
                        {
                            clienttypeselected = Enumerations.Order.ClientTypes.CLIENT.ToString();
                            ClientName = string.Empty;
                        }
                    }

                    ErrorMsg = string.Empty;
                }
                ChangeShortClient = true;
            }
            catch (Exception ex)
            {

                ExceptionUtility.LogError(ex);
            }
        }
        public void UpdateScripNetPosition(string strScripCode, List<KeyValuePair<string, object>> Obj)
        {

            try
            {
                int index = 0;
                long longScripCode = Convert.ToInt64(strScripCode);
                if ((Obj != null && Obj.Count() > 0) &&
                    (ScripSelectedCode == Convert.ToInt64(strScripCode) || ScripCodeSelected == Convert.ToInt64(strScripCode)))
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
                        var intBuyPosition = item.scripData.Cast<NetPosition>().Sum(x => x.BuyQty);
                        if (intBuyPosition == 0)
                        {
                            BuyPosition = string.Empty;
                        }
                        else
                        {
                            BuyPosition = intBuyPosition.ToString();
                        }

                        //oScripWisePositionModel.SellQty = item.scripData.Cast<NetPosition>().Sum(x => x.SellQty);
                        var intSellPosition = item.scripData.Cast<NetPosition>().Sum(x => x.SellQty);
                        if (intSellPosition == 0)
                        {
                            SellPosition = string.Empty;
                        }
                        else
                        {
                            SellPosition = intSellPosition.ToString();
                        }

                        var segment = CommonFunctions.GetSegmentID(longScripCode);
                        var multiplier = CommonFunctions.GetQuantityMultiplier(longScripCode, "BSE", segment);
                        var totalBuyVal = item.scripData.Cast<NetPosition>().Sum(x => x.BuyValue);
                        var totalBuyQty = intBuyPosition * multiplier;


                        var decimalPoint = CommonFunctions.GetDecimal(Convert.ToInt32(strScripCode), "BSE", segment);
                        var AvgBuyRate = Convert.ToDouble(CommonFrontEnd.Common.CommonFunctions.DisplayInDecimalFormat(totalBuyVal, totalBuyQty, decimalPoint));//item.scripData.Cast<NetPosition>().Sum(x => x.AvgBuyRate);

                        var totalSellVal = item.scripData.Cast<NetPosition>().Sum(x => x.SellValue);
                        var totalSellQty = intSellPosition * multiplier;

                        var AvgSellRate = Convert.ToDouble(CommonFrontEnd.Common.CommonFunctions.DisplayInDecimalFormat(totalSellVal, totalSellQty, decimalPoint));//item.scripData.Cast<NetPosition>().Sum(x => x.SellValue) / oScripWisePositionModel.SellQty;//item.scripData.Cast<NetPosition>().Sum(x => x.AvgSellRate);

                        var NetQty = item.scripData.Cast<NetPosition>().Sum(x => x.NetQty);
                        var NetValue = Convert.ToInt64((totalBuyVal - totalSellVal) / Math.Pow(10, UtilityApplicationDetails.GetInstance.MaxDecimalPoint));

                        if (decimalPoint == 4)
                        {
                            if (AvgBuyRate == 0)
                            {
                                AvgBuyPosition = string.Empty;
                            }
                            else
                            {
                                AvgBuyPosition = string.Format("{0:0.0000}", (AvgBuyRate / (Math.Pow(10, UtilityApplicationDetails.GetInstance.MaxDecimalPoint))));
                            }

                            if (AvgSellRate == 0)
                            {
                                AvgSellPosition = string.Empty;
                            }
                            else
                            {
                                AvgSellPosition = string.Format("{0:0.0000}", (AvgSellRate / (Math.Pow(10, UtilityApplicationDetails.GetInstance.MaxDecimalPoint))));
                            }

                            if (NetValue == 0 || NetQty == 0)
                            {
                                NetPositionNetValue = string.Empty;
                            }
                            else
                            {
                                NetPositionNetValue = string.Format("{0:0.0000}", (Convert.ToDecimal(NetValue) / Convert.ToDecimal(NetQty)) / multiplier);
                            }

                            if (NetQty == 0)
                            {
                                NetPositionNetQty = string.Empty;
                            }
                            else
                            {
                                NetPositionNetQty = Convert.ToString(NetQty);
                            }

                        }
                        else//Decimal 2 places code- Gaurav Jadhav 24/4/2018
                        {
                            if (AvgBuyRate == 0)
                            {
                                AvgBuyPosition = string.Empty;
                            }
                            else
                            {
                                AvgBuyPosition = string.Format("{0:0.00}", (AvgBuyRate / (Math.Pow(10, UtilityApplicationDetails.GetInstance.MaxDecimalPoint))));
                            }

                            if (AvgSellRate == 0)
                            {
                                AvgSellPosition = string.Empty;
                            }
                            else
                            {
                                AvgSellPosition = string.Format("{0:0.00}", (AvgSellRate / (Math.Pow(10, UtilityApplicationDetails.GetInstance.MaxDecimalPoint))));
                            }

                            if (NetValue == 0 || NetQty == 0)
                            {
                                NetPositionNetValue = string.Empty;
                            }
                            else
                            {
                                NetPositionNetValue = string.Format("{0:0.00}", (Convert.ToDecimal(NetValue) / Convert.ToDecimal(NetQty)) / multiplier);
                            }

                            if (NetQty == 0)
                            {
                                NetPositionNetQty = string.Empty;
                            }
                            else
                            {
                                NetPositionNetQty = Convert.ToString(NetQty);
                            }

                        }
                    }
                }
                else//clear fields
                {
                    BuyPosition = string.Empty;
                    SellPosition = string.Empty;
                    AvgBuyPosition = string.Empty;
                    AvgSellPosition = string.Empty;
                    NetPositionNetValue = string.Empty;
                    NetPositionNetQty = string.Empty;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void FetchNetPositionByScripCode(long ScripCode)
        {
            UpdateScripNetPosition(Convert.ToString(ScripCode), MemoryManager.NetPositionSWDemoDict.Where(x => ((CommonFrontEnd.Model.Trade.NetPosition)x.Value).ScripCode == ScripCode).ToList());

            #region Commented


            //if (ScripSelectedCode == ScripCode)
            //{
            //    var decimalPoint = CommonFunctions.GetDecimal(Convert.ToInt32(ScripCode), "BSE", ScripSelectedSegment);

            //    var tempNetPositionNetQty = MemoryManager.NetPositionSWDemoDict.Where(x => ((CommonFrontEnd.Model.Trade.NetPosition)x.Value).ScripCode == ScripCode).ToList().Sum(x => ((NetPosition)x.Value).NetQty);
            //    var JusttempNetPositionNetValue = Math.Abs(MemoryManager.NetPositionSWDemoDict.Where(x => ((NetPosition)x.Value).ScripCode == ScripCode).ToList().Sum(x => ((NetPosition)x.Value).NetValue));
            //    long tempNetPositionNetValue = 0;
            //    if (tempNetPositionNetQty != 0)
            //    {
            //        tempNetPositionNetValue = Math.Abs((JusttempNetPositionNetValue / tempNetPositionNetQty));
            //        NetPositionNetQty = Convert.ToString(tempNetPositionNetQty);
            //    }
            //    else
            //        NetPositionNetQty = string.Empty;

            //    var buyQty = MemoryManager.NetPositionSWDemoDict.Where(x => ((CommonFrontEnd.Model.Trade.NetPosition)x.Value).ScripCode == ScripCode).ToList().Sum(x => ((NetPosition)x.Value).BuyQty);
            //    var sellQty = MemoryManager.NetPositionSWDemoDict.Where(x => ((CommonFrontEnd.Model.Trade.NetPosition)x.Value).ScripCode == ScripCode).ToList().Sum(x => ((NetPosition)x.Value).SellQty);
            //    var buyAvgRate = MemoryManager.NetPositionSWDemoDict.Where(x => ((CommonFrontEnd.Model.Trade.NetPosition)x.Value).ScripCode == ScripCode).ToList().Sum(x => ((NetPosition)x.Value).AvgBuyRate);
            //    var sellAvgRate = MemoryManager.NetPositionSWDemoDict.Where(x => ((CommonFrontEnd.Model.Trade.NetPosition)x.Value).ScripCode == ScripCode).ToList().Sum(x => ((NetPosition)x.Value).AvgSellRate);

            //    if (buyQty > 0)
            //    {
            //        BuyPosition = Convert.ToString(buyQty);
            //        //AvgBuyPosition = Convert.ToString(buyAvgRate);
            //    }
            //    else
            //    {
            //        BuyPosition = string.Empty;
            //    }
            //    if (sellQty > 0)
            //    {
            //        SellPosition = Convert.ToString(sellQty);
            //        //AvgSellPosition = Convert.ToString(sellAvgRate);
            //    }
            //    else
            //    {
            //        SellPosition = string.Empty;
            //    }
            //    if (decimalPoint == 4)
            //    {
            //        if (tempNetPositionNetValue > 0)
            //            NetPositionNetValue = string.Format("{0:0.0000}", (tempNetPositionNetValue / (Math.Pow(10, UtilityApplicationDetails.GetInstance.MaxDecimalPoint))));
            //        else
            //            NetPositionNetValue = string.Empty;
            //        if (buyAvgRate > 0)
            //            AvgBuyPosition = string.Format("{0:0.0000}", (buyAvgRate / (Math.Pow(10, UtilityApplicationDetails.GetInstance.MaxDecimalPoint))));
            //        else
            //            AvgBuyPosition = string.Empty;
            //        if (sellAvgRate > 0)
            //            AvgSellPosition = string.Format("{0:0.0000}", (sellAvgRate / (Math.Pow(10, UtilityApplicationDetails.GetInstance.MaxDecimalPoint))));
            //        else
            //            AvgSellPosition = string.Empty;
            //    }
            //    else
            //    {
            //        if (tempNetPositionNetValue > 0)
            //            NetPositionNetValue = string.Format("{0:0.00}", (tempNetPositionNetValue / (Math.Pow(10, UtilityApplicationDetails.GetInstance.MaxDecimalPoint))));
            //        else
            //            NetPositionNetValue = string.Empty;
            //        if (buyAvgRate > 0)
            //            AvgBuyPosition = string.Format("{0:0.00}", (buyAvgRate / (Math.Pow(10, UtilityApplicationDetails.GetInstance.MaxDecimalPoint))));
            //        else
            //            AvgBuyPosition = string.Empty;
            //        if (sellAvgRate > 0)
            //            AvgSellPosition = string.Format("{0:0.00}", (sellAvgRate / (Math.Pow(10, UtilityApplicationDetails.GetInstance.MaxDecimalPoint))));
            //        else
            //            AvgSellPosition = string.Empty;
            //    }

            //}
            #endregion

        }
        private bool OrderSettingValidationForQtyAndValue(OrderModel omodel)
        {

            #region Equity and Debt OrderSetting Validation

            if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Equity.ToString() || ScripSelectedSegment == Enumerations.Order.ScripSegment.Debt.ToString())
            {
                if (!string.IsNullOrEmpty(UtilityOrderDetails.GETInstance.EQTYMaxQty))
                {
                    if (omodel.OriginalQty >= Convert.ToInt32(UtilityOrderDetails.GETInstance.EQTYMaxQty))
                    {
                        MessageBoxResult res = MessageBox.Show("Current Order Qty = " + omodel.OriginalQty + "\nWarning Maximum Limit = " + UtilityOrderDetails.GETInstance.EQTYMaxQty + "\nDo you wish to continue?", "ORDER WARNING!", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (res == MessageBoxResult.Yes)
                        {
                        }
                        else
                        {
                            return false;
                        }
                    }
                }

                if (!string.IsNullOrEmpty(UtilityOrderDetails.GETInstance.EQTYMinQty))
                {
                    if (omodel.OriginalQty <= Convert.ToInt32(UtilityOrderDetails.GETInstance.EQTYMinQty))
                    {
                        MessageBoxResult res = MessageBox.Show("Current Order Qty = " + omodel.OriginalQty + "\nWarning Minimum Limit = " + UtilityOrderDetails.GETInstance.EQTYMinQty + "\nDo you wish to continue?", "ORDER WARNING!", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (res == MessageBoxResult.Yes)
                        {
                        }
                        else
                        {
                            return false;
                        }
                    }
                }


                if (!string.IsNullOrEmpty(UtilityOrderDetails.GETInstance.EQTYMaxOrderValue))
                {
                    double value = 0;
                    if (!string.IsNullOrEmpty(rate))
                    {
                        value = omodel.OriginalQty * Convert.ToDouble(rate);
                    }
                    else
                    {
                        if (BroadcastMaster.BroadcastMasterMemory.objScripDetailsConDict.ContainsKey((int)omodel.ScripCode))
                        {
                            BroadcastReceiver.ScripDetails objScripDetails = new BroadcastReceiver.ScripDetails();
                            objScripDetails = BroadcastMaster.BroadcastMasterMemory.objScripDetailsConDict[(int)omodel.ScripCode];
                            DecimalPoint = Common.CommonFunctions.GetDecimal((int)omodel.ScripCode, Selected_EXCH, ScripSelectedSegment);

                            if (objScripDetails != null)
                            {
                                if (objScripDetails.Det != null && objScripDetails.Det.Count() > 0)
                                {
                                    if (omodel.BuySellIndicator == Enumerations.Order.BuySellFlag.B.ToString())
                                    {
                                        if (objScripDetails.Det[0].SellRate_l / Math.Pow(10, DecimalPoint) != 0)
                                        {
                                            value = omodel.OriginalQty * objScripDetails.Det[0].SellRate_l / Math.Pow(10, DecimalPoint);
                                        }
                                        else
                                        {
                                            int CurrentLTP = BroadcastMaster.BroadcastMasterMemory.objScripDetailsConDict[(int)omodel.ScripCode].LastTradeRate_l;
                                            if (CurrentLTP != 0)
                                            {
                                                value = omodel.OriginalQty * CurrentLTP;
                                            }
                                            else
                                            {
                                                int closePrice = BroadcastMaster.BroadcastMasterMemory.objScripDetailsConDict[(int)omodel.ScripCode].CloseRate_l;
                                                if (closePrice != 0)
                                                {
                                                    value = omodel.OriginalQty * closePrice;
                                                }
                                                else
                                                {
                                                    int prevclose = BroadcastMaster.BroadcastMasterMemory.objScripDetailsConDict[(int)omodel.ScripCode].PrevClosePrice_l;
                                                    if (prevclose != 0)
                                                    {
                                                        value = omodel.OriginalQty * prevclose;
                                                    }
                                                    else
                                                    {
                                                        ErrorMsg = "Click on Refresh Button for latest values";
                                                    }
                                                }
                                            }
                                        }

                                    }

                                    else if (omodel.BuySellIndicator == Enumerations.Order.BuySellFlag.S.ToString())
                                    {
                                        if (objScripDetails.Det[0].BuyRate_l / Math.Pow(10, DecimalPoint) != 0)
                                        {
                                            value = omodel.OriginalQty * objScripDetails.Det[0].BuyRate_l / Math.Pow(10, DecimalPoint);
                                        }
                                        else
                                        {
                                            int CurrentLTP = BroadcastMaster.BroadcastMasterMemory.objScripDetailsConDict[(int)omodel.ScripCode].LastTradeRate_l;
                                            if (CurrentLTP != 0)
                                            {
                                                value = omodel.OriginalQty * CurrentLTP;
                                            }
                                            else
                                            {
                                                int closePrice = BroadcastMaster.BroadcastMasterMemory.objScripDetailsConDict[(int)omodel.ScripCode].CloseRate_l;
                                                if (closePrice != 0)
                                                {
                                                    value = omodel.OriginalQty * closePrice;
                                                }
                                                else
                                                {
                                                    int prevclose = BroadcastMaster.BroadcastMasterMemory.objScripDetailsConDict[(int)omodel.ScripCode].PrevClosePrice_l;
                                                    if (prevclose != 0)
                                                    {
                                                        value = omodel.OriginalQty * prevclose;
                                                    }
                                                    else
                                                    {
                                                        ErrorMsg = "Click on Refresh Button for latest values";
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    ErrorMsg = "Click on Refresh Button for latest values";
                                }
                            }
                        }
                        else
                        {
                            ErrorMsg = "Click on Refresh Button for latest values";
                        }

                    }

                    if (value >= Convert.ToDouble(UtilityOrderDetails.GETInstance.EQTYMaxOrderValue))
                    {
                        MessageBoxResult res = MessageBox.Show(string.Format("Current Order Value = {0:0.00} \nWarning Limit = {1:0.00} \nDo you wish to continue?", value, Convert.ToDouble(UtilityOrderDetails.GETInstance.EQTYMaxOrderValue)), "ORDER WARNING!", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (res == MessageBoxResult.Yes)
                        {
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            #endregion

            #region Derivative OrderSetting Validation

            else if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Derivative.ToString())
            {
                if (!string.IsNullOrEmpty(UtilityOrderDetails.GETInstance.DERVMaxQty))
                {
                    if (omodel.OriginalQty >= Convert.ToInt32(UtilityOrderDetails.GETInstance.DERVMaxQty))
                    {
                        MessageBoxResult res = MessageBox.Show("Current Order Qty = " + omodel.OriginalQty + "\nWarning Maximum Limit = " + UtilityOrderDetails.GETInstance.DERVMaxQty + "\nDo you wish to continue?", "ORDER WARNING!", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (res == MessageBoxResult.Yes)
                        {
                        }
                        else
                        {
                            return false;
                        }
                    }
                }

                if (!string.IsNullOrEmpty(UtilityOrderDetails.GETInstance.DERVMinQty))
                {
                    if (omodel.OriginalQty <= Convert.ToInt32(UtilityOrderDetails.GETInstance.DERVMinQty))
                    {
                        MessageBoxResult res = MessageBox.Show("Current Order Qty = " + omodel.OriginalQty + "\nWarning Minimum Limit = " + UtilityOrderDetails.GETInstance.DERVMinQty + "\nDo you wish to continue?", "ORDER WARNING!", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (res == MessageBoxResult.Yes)
                        {
                        }
                        else
                        {
                            return false;
                        }
                    }
                }

                if (!string.IsNullOrEmpty(UtilityOrderDetails.GETInstance.DERVMaxOrderValue))
                {
                    double value = 0;
                    if (!string.IsNullOrEmpty(rate))
                    {
                        value = omodel.OriginalQty * Convert.ToDouble(rate);
                    }
                    else
                    {
                        if (BroadcastMaster.BroadcastMasterMemory.objScripDetailsConDict.ContainsKey((int)omodel.ScripCode))
                        {
                            BroadcastReceiver.ScripDetails objScripDetails = new BroadcastReceiver.ScripDetails();
                            objScripDetails = BroadcastMaster.BroadcastMasterMemory.objScripDetailsConDict[(int)omodel.ScripCode];
                            DecimalPoint = Common.CommonFunctions.GetDecimal((int)omodel.ScripCode, Selected_EXCH, ScripSelectedSegment);

                            if (objScripDetails != null)
                            {
                                if (objScripDetails.Det != null && objScripDetails.Det.Count() > 0)
                                {
                                    if (omodel.BuySellIndicator == Enumerations.Order.BuySellFlag.B.ToString())
                                    {
                                        if (objScripDetails.Det[0].SellRate_l / Math.Pow(10, DecimalPoint) != 0)
                                        {
                                            value = omodel.OriginalQty * objScripDetails.Det[0].SellRate_l / Math.Pow(10, DecimalPoint);
                                        }
                                        else
                                        {
                                            int CurrentLTP = BroadcastMaster.BroadcastMasterMemory.objScripDetailsConDict[(int)omodel.ScripCode].LastTradeRate_l;
                                            if (CurrentLTP != 0)
                                            {
                                                value = omodel.OriginalQty * CurrentLTP;
                                            }
                                            else
                                            {
                                                int closePrice = BroadcastMaster.BroadcastMasterMemory.objScripDetailsConDict[(int)omodel.ScripCode].CloseRate_l;
                                                if (closePrice != 0)
                                                {
                                                    value = omodel.OriginalQty * closePrice;
                                                }
                                                else
                                                {
                                                    int prevclose = BroadcastMaster.BroadcastMasterMemory.objScripDetailsConDict[(int)omodel.ScripCode].PrevClosePrice_l;
                                                    if (prevclose != 0)
                                                    {
                                                        value = omodel.OriginalQty * prevclose;
                                                    }
                                                    else
                                                    {
                                                        ErrorMsg = "Click on Refresh Button for latest values";
                                                    }
                                                }
                                            }
                                        }

                                    }

                                    else if (omodel.BuySellIndicator == Enumerations.Order.BuySellFlag.S.ToString())
                                    {
                                        if (objScripDetails.Det[0].BuyRate_l / Math.Pow(10, DecimalPoint) != 0)
                                        {
                                            value = omodel.OriginalQty * objScripDetails.Det[0].BuyRate_l / Math.Pow(10, DecimalPoint);
                                        }
                                        else
                                        {
                                            int CurrentLTP = BroadcastMaster.BroadcastMasterMemory.objScripDetailsConDict[(int)omodel.ScripCode].LastTradeRate_l;
                                            if (CurrentLTP != 0)
                                            {
                                                value = omodel.OriginalQty * CurrentLTP;
                                            }
                                            else
                                            {
                                                int closePrice = BroadcastMaster.BroadcastMasterMemory.objScripDetailsConDict[(int)omodel.ScripCode].CloseRate_l;
                                                if (closePrice != 0)
                                                {
                                                    value = omodel.OriginalQty * closePrice;
                                                }
                                                else
                                                {
                                                    int prevclose = BroadcastMaster.BroadcastMasterMemory.objScripDetailsConDict[(int)omodel.ScripCode].PrevClosePrice_l;
                                                    if (prevclose != 0)
                                                    {
                                                        value = omodel.OriginalQty * prevclose;
                                                    }
                                                    else
                                                    {
                                                        ErrorMsg = "Click on Refresh Button for latest values";
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    ErrorMsg = "Click on Refresh Button for latest values";
                                }
                            }
                        }
                        else
                        {
                            ErrorMsg = "Click on Refresh Button for latest values";
                        }

                    }

                    if (value >= Convert.ToDouble(UtilityOrderDetails.GETInstance.DERVMaxOrderValue))
                    {
                        MessageBoxResult res = MessageBox.Show(string.Format("Current Order Value = {0:0.00} \nWarning Limit = {1:0.00} \nDo you wish to continue?", value, Convert.ToDouble(UtilityOrderDetails.GETInstance.DERVMaxOrderValue)), "ORDER WARNING!", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (res == MessageBoxResult.Yes)
                        {
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            #endregion

            #region Currency OrderSetting Validation

            else if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Currency.ToString())
            {
                if (!string.IsNullOrEmpty(UtilityOrderDetails.GETInstance.CURRMaxQty))
                {
                    if (omodel.OriginalQty >= Convert.ToInt32(UtilityOrderDetails.GETInstance.CURRMaxQty))
                    {
                        MessageBoxResult res = MessageBox.Show("Current Order Qty = " + omodel.OriginalQty + "\nWarning Maximum Limit = " + UtilityOrderDetails.GETInstance.CURRMaxQty + "\nDo you wish to continue?", "ORDER WARNING!", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (res == MessageBoxResult.Yes)
                        {
                        }
                        else
                        {
                            return false;
                        }
                    }
                }

                if (!string.IsNullOrEmpty(UtilityOrderDetails.GETInstance.CURRMinQty))
                {
                    if (omodel.OriginalQty <= Convert.ToInt32(UtilityOrderDetails.GETInstance.CURRMinQty))
                    {
                        MessageBoxResult res = MessageBox.Show("Current Order Qty = " + omodel.OriginalQty + "\nWarning Minimum Limit = " + UtilityOrderDetails.GETInstance.CURRMinQty + "\nDo you wish to continue?", "ORDER WARNING!", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (res == MessageBoxResult.Yes)
                        {
                        }
                        else
                        {
                            return false;
                        }
                    }
                }

                if (!string.IsNullOrEmpty(UtilityOrderDetails.GETInstance.CURRMaxOrderValue))
                {
                    double value = 0;
                    if (!string.IsNullOrEmpty(rate))
                    {
                        value = omodel.OriginalQty * Convert.ToDouble(rate);
                    }
                    else
                    {
                        if (BroadcastMaster.BroadcastMasterMemory.objScripDetailsConDict.ContainsKey((int)omodel.ScripCode))
                        {
                            BroadcastReceiver.ScripDetails objScripDetails = new BroadcastReceiver.ScripDetails();
                            objScripDetails = BroadcastMaster.BroadcastMasterMemory.objScripDetailsConDict[(int)omodel.ScripCode];
                            DecimalPoint = Common.CommonFunctions.GetDecimal((int)omodel.ScripCode, Selected_EXCH, ScripSelectedSegment);

                            if (objScripDetails != null)
                            {
                                if (objScripDetails.Det != null && objScripDetails.Det.Count() > 0)
                                {
                                    if (omodel.BuySellIndicator == Enumerations.Order.BuySellFlag.B.ToString())
                                    {
                                        if (objScripDetails.Det[0].SellRate_l / Math.Pow(10, DecimalPoint) != 0)
                                        {
                                            value = omodel.OriginalQty * objScripDetails.Det[0].SellRate_l / Math.Pow(10, DecimalPoint);
                                        }
                                        else
                                        {
                                            int CurrentLTP = BroadcastMaster.BroadcastMasterMemory.objScripDetailsConDict[(int)omodel.ScripCode].LastTradeRate_l;
                                            if (CurrentLTP != 0)
                                            {
                                                value = omodel.OriginalQty * CurrentLTP;
                                            }
                                            else
                                            {
                                                int closePrice = BroadcastMaster.BroadcastMasterMemory.objScripDetailsConDict[(int)omodel.ScripCode].CloseRate_l;
                                                if (closePrice != 0)
                                                {
                                                    value = omodel.OriginalQty * closePrice;
                                                }
                                                else
                                                {
                                                    int prevclose = BroadcastMaster.BroadcastMasterMemory.objScripDetailsConDict[(int)omodel.ScripCode].PrevClosePrice_l;
                                                    if (prevclose != 0)
                                                    {
                                                        value = omodel.OriginalQty * prevclose;
                                                    }
                                                    else
                                                    {
                                                        ErrorMsg = "Click on Refresh Button for latest values";
                                                    }
                                                }
                                            }
                                        }

                                    }

                                    else if (omodel.BuySellIndicator == Enumerations.Order.BuySellFlag.S.ToString())
                                    {
                                        if (objScripDetails.Det[0].BuyRate_l / Math.Pow(10, DecimalPoint) != 0)
                                        {
                                            value = omodel.OriginalQty * objScripDetails.Det[0].BuyRate_l / Math.Pow(10, DecimalPoint);
                                        }
                                        else
                                        {
                                            int CurrentLTP = BroadcastMaster.BroadcastMasterMemory.objScripDetailsConDict[(int)omodel.ScripCode].LastTradeRate_l;
                                            if (CurrentLTP != 0)
                                            {
                                                value = omodel.OriginalQty * CurrentLTP;
                                            }
                                            else
                                            {
                                                int closePrice = BroadcastMaster.BroadcastMasterMemory.objScripDetailsConDict[(int)omodel.ScripCode].CloseRate_l;
                                                if (closePrice != 0)
                                                {
                                                    value = omodel.OriginalQty * closePrice;
                                                }
                                                else
                                                {
                                                    int prevclose = BroadcastMaster.BroadcastMasterMemory.objScripDetailsConDict[(int)omodel.ScripCode].PrevClosePrice_l;
                                                    if (prevclose != 0)
                                                    {
                                                        value = omodel.OriginalQty * prevclose;
                                                    }
                                                    else
                                                    {
                                                        ErrorMsg = "Click on Refresh Button for latest values";
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    ErrorMsg = "Click on Refresh Button for latest values";
                                }
                            }
                        }
                        else
                        {
                            ErrorMsg = "Click on Refresh Button for latest values";
                        }

                    }

                    if (value >= Convert.ToDouble(UtilityOrderDetails.GETInstance.CURRMaxOrderValue))
                    {
                        MessageBoxResult res = MessageBox.Show(string.Format("Current Order Value = {0:0.00} \nWarning Limit = {1:0.00} \nDo you wish to continue?", value, Convert.ToDouble(UtilityOrderDetails.GETInstance.CURRMaxOrderValue)), "ORDER WARNING!", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (res == MessageBoxResult.Yes)
                        {
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            #endregion

            if (ClientIDinputlst.Contains(ShortClientSelectedText) || MasterSharedMemory.objClientMasterDict.Select(x => x.Value.CompleteClientId).ToList().Contains(ShortClientSelectedText))
            {
                return true;
            }
            else
            {
                if (UtilityOrderDetails.GETInstance.clientIdAllowed == 'N')
                {
                    MessageBox.Show("Client code : " + ShortClientSelectedText + " is not present in Client List.\n Please Add the Client ID.", "WARNING!", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
                if (UtilityOrderDetails.GETInstance.clientIdAllowed == 'A') { return true; }

                if (UtilityOrderDetails.GETInstance.clientIdAllowed == 'W')
                {
                    MessageBoxResult result = MessageBox.Show("Client code : " + ShortClientSelectedText + " is not present in Client List.\n Do you wish to continue ?", "Confirmation", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                    if (result == MessageBoxResult.OK) return true;
                    if (result == MessageBoxResult.Cancel) return false;
                }

            }

            return true;
        }

        private void OnShortClientIDCheckUncheck(object e)
        {
            //throw new NotImplementedException();
            if (e?.ToString() == "EQ")
            {
                if (ShortClientIDEquityCheck == bool.TrueString)
                {
                    UtilityOrderDetails.GETInstance.IsEqtShortClientIDChecked = "1";
                    UtilityOrderDetails.GETInstance.EqtShortClientID = ShortClientSelectedText;
                }
                else
                {
                    UtilityOrderDetails.GETInstance.IsEqtShortClientIDChecked = "0";
                    UtilityOrderDetails.GETInstance.EqtShortClientID = string.Empty;
                }
            }
            else if (e?.ToString() == "DEBT")
            {
                if (ShortClientIDDebtCheck == bool.TrueString)
                {
                    UtilityOrderDetails.GETInstance.IsDebtShortClientIDChecked = "1";
                    UtilityOrderDetails.GETInstance.DebtShortClientID = ShortClientSelectedText;
                }
                else
                {
                    UtilityOrderDetails.GETInstance.IsDebtShortClientIDChecked = "0";
                    UtilityOrderDetails.GETInstance.DebtShortClientID = string.Empty;
                }
            }
            else if (e?.ToString() == "DER")
            {
                if (ShortClientIDDerCheck == bool.TrueString)
                {
                    UtilityOrderDetails.GETInstance.IsDervShortClientIDChecked = "1";
                    UtilityOrderDetails.GETInstance.DervShortClientID = ShortClientSelectedText;
                }
                else
                {
                    UtilityOrderDetails.GETInstance.IsDervShortClientIDChecked = "0";
                    UtilityOrderDetails.GETInstance.DervShortClientID = string.Empty;
                }
            }
            else if (e?.ToString() == "CUR")
            {
                if (ShortClientIDCurCheck == bool.TrueString)
                {
                    UtilityOrderDetails.GETInstance.IsCurrShortClientIDChecked = "1";
                    UtilityOrderDetails.GETInstance.CurrShortClientID = ShortClientSelectedText;
                }
                else
                {
                    UtilityOrderDetails.GETInstance.IsCurrShortClientIDChecked = "0";
                    UtilityOrderDetails.GETInstance.CurrShortClientID = string.Empty;
                }
            }

        }
        private void QtyLostFocusValidation(object e)
        {
            if (revQty == string.Empty)
            {
                ErrorMsg = "Total quantity not multiple of mkt lot";
            }
            else
            {
                ErrorMsg = string.Empty;
            }
        }

        #region Derivative And Currency Methods

        private void PopulatingInstType()
        {
            try
            {

                #region BSE

                //for BSE Exchange
                if (Selected_EXCH == Enumerations.Order.Exchanges.BSE.ToString())
                {
                    //for derivative market DerivativeMasterBase> 
                    if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Derivative.ToString())
                    {
                        IntrTypeLst = new List<string>();

                        IntrTypeLst.Add(Enumerations.Order.DervInstrumentType.FutIndex.ToString());
                        IntrTypeLst.Add(Enumerations.Order.DervInstrumentType.FutStock.ToString());
                        IntrTypeLst.Add(Enumerations.Order.DervInstrumentType.CallIndex.ToString());
                        IntrTypeLst.Add(Enumerations.Order.DervInstrumentType.CallStock.ToString());
                        IntrTypeLst.Add(Enumerations.Order.DervInstrumentType.PutIndex.ToString());
                        IntrTypeLst.Add(Enumerations.Order.DervInstrumentType.PutStock.ToString());
                        IntrTypeLst.Add(Enumerations.Order.DervInstrumentType.PairOption.ToString());

                        IntrTypeSelected = IntrTypeLst[0];
                        NotifyPropertyChanged(nameof(IntrTypeLst));

                    }

                    //for BSE Currency
                    if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Currency.ToString())
                    {
                        IntrTypeLst = new List<string>();

                        IntrTypeLst.Add(Enumerations.Order.CurrInstrumentType.Future.ToString());
                        IntrTypeLst.Add(Enumerations.Order.CurrInstrumentType.Call.ToString());
                        IntrTypeLst.Add(Enumerations.Order.CurrInstrumentType.Put.ToString());
                        IntrTypeLst.Add(Enumerations.Order.CurrInstrumentType.PairOption.ToString());
                        IntrTypeLst.Add(Enumerations.Order.CurrInstrumentType.Straddle.ToString());

                        IntrTypeSelected = IntrTypeLst[0];
                        NotifyPropertyChanged(nameof(IntrTypeLst));
                    }

                    //itp
                    if (ScripSelectedSegment == Enumerations.Order.ScripSegment.ITP.ToString())
                    {
                        IntrTypeLst = new List<String>();
                        CallPutLSt.Clear();
                        StrikePriceEnable = false;
                        if (MasterSharedMemory.objITPMasterDict != null)
                            IntrTypeLst = MasterSharedMemory.objITPMasterDict.Values.Cast<ITPMaster>().Select(x => x.ITPInstrumenName).Distinct().ToList();
                        if (IntrTypeLst.Count() > 0)
                            IntrTypeSelected = IntrTypeLst[0];
                        //TODO option type to be hidden
                    }
                    // SLB SLBMaster objSLBMasterDict
                    if (ScripSelectedSegment == Enumerations.Order.ScripSegment.SLB.ToString())
                    {
                        IntrTypeLst = new List<String>();
                        CallPutLSt.Clear();
                        StrikePriceEnable = false;
                        if (MasterSharedMemory.objITPMasterDict != null)
                            IntrTypeLst = MasterSharedMemory.objSLBMasterDict.Values.Cast<SLBMaster>().Select(x => x.SCInstrumentName).Distinct().ToList();
                        if (IntrTypeLst.Count() > 0)
                            IntrTypeSelected = IntrTypeLst[0];
                        //TODO option type to be hidden
                    }


                }

                #endregion

                #region NSE
                //for NSE
                if (Selected_EXCH == Enumerations.Order.Exchanges.NSE.ToString())
                {
                    //for derivative market DerivativeMasterBase> 
                    if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Derivative.ToString())
                    {
                        DecimalPoint = 2;
                        IntrTypeLst = new List<String>();

                        if (MasterSharedMemory.objMasterDerivativeDictBaseNSE != null)//NSE Derivative
                        {
                            IntrTypeLst = MasterSharedMemory.objMasterDerivativeDictBaseNSE.Values.Cast<DerivativeMasterBase>().Select(x => x.InstrumentType).Distinct().ToList();
                            IntrTypeLst.Remove(null);
                        }
                        if (IntrTypeLst.Count() > 0)
                            IntrTypeSelected = IntrTypeLst[0];
                        if (MasterSharedMemory.objMasterDerivativeDictBaseNSE != null)//NSE Derivative
                            CallPutLSt = MasterSharedMemory.objMasterDerivativeDictBaseNSE.Values.Cast<DerivativeMasterBase>().Select(x => x.OptionType).Distinct().ToList();
                        if (CallPutLSt.Count() > 0)
                            SubIntrTypeSelected = CallPutLSt[0];
                        //CallPutSelected = Enumerations.Order.CallPut.Call.ToString();
                        //CallPutLSt.Add(Enumerations.Order.CallPut.Call.ToString());
                        //CallPutLSt.Add(Enumerations.Order.CallPut.Put.ToString());

                    }


                    //for NSE Currency
                    if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Currency.ToString())
                    {
                        DecimalPoint = 4;
                        IntrTypeLst = new List<String>();

                        if (MasterSharedMemory.objMasterCurrencyDictBaseNSE != null)//NSE Currency
                        {
                            IntrTypeLst = MasterSharedMemory.objMasterCurrencyDictBaseNSE.Values.Cast<CurrencyMasterBase>().Select(x => x.InstrumentType).Distinct().ToList();
                            IntrTypeLst.Remove(null);
                        }
                        if (IntrTypeLst.Count() > 0)
                            IntrTypeSelected = IntrTypeLst[0];
                        if (MasterSharedMemory.objMasterCurrencyDictBaseNSE != null)//NSE Currency
                        {
                            CallPutLSt = MasterSharedMemory.objMasterCurrencyDictBaseNSE.Values.Cast<CurrencyMasterBase>().Select(x => x.OptionType).Distinct().ToList();
                            CallPutLSt.Remove("XX");
                        }
                        if (CallPutLSt.Count() > 0)
                            SubIntrTypeSelected = CallPutLSt[0];
                    }
                }
                #endregion

                #region NCDEX
                //NCDEX
                if (Selected_EXCH == Enumerations.Order.Exchanges.NCDEX.ToString())
                {
                    //Commodities NCDEXMaster objNCDEXMasterDict
                    if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Commodities.ToString())
                    {
                        IntrTypeLst = new List<String>();

                        if (MasterSharedMemory.objNCDEXMasterDict != null)//
                            IntrTypeLst = MasterSharedMemory.objNCDEXMasterDict.Values.Cast<NCDEXMaster>().Select(x => x.NCDInstrumentName).Distinct().ToList();
                        if (IntrTypeLst.Count() > 0)
                            IntrTypeSelected = IntrTypeLst[0];

                        if (MasterSharedMemory.objMasterDerivativeDictBaseNSE != null)
                            CallPutLSt = MasterSharedMemory.objNCDEXMasterDict.Values.Cast<NCDEXMaster>().Select(x => x.NCDOptionType).Distinct().ToList();
                        if (CallPutLSt.Count() > 0)
                            SubIntrTypeSelected = CallPutLSt[0];


                    }
                }

                #endregion

                #region MCX
                if (Selected_EXCH == Enumerations.Order.Exchanges.MCX.ToString())
                {
                    //Commodities  
                    if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Commodities.ToString())
                    {
                        IntrTypeLst = new List<String>();

                        if (MasterSharedMemory.objMCXMasterDict != null)//
                            IntrTypeLst = MasterSharedMemory.objMCXMasterDict.Values.Cast<MCXMaster>().Select(x => x.MCInstrumentName).Distinct().ToList();
                        if (IntrTypeLst.Count() > 0)
                            IntrTypeSelected = IntrTypeLst[0];

                        if (MasterSharedMemory.objMCXMasterDict != null)
                        {
                            CallPutLSt = MasterSharedMemory.objMCXMasterDict.Values.Cast<MCXMaster>().Select(x => x.MCOptionType).Distinct().ToList();
                            CallPutLSt.Remove("XX");
                        }
                        if (CallPutLSt.Count() > 0)
                            SubIntrTypeSelected = CallPutLSt[0];


                    }
                }
                #endregion

            }
            catch (Exception ex)
            {

                ExceptionUtility.LogError(ex);
            }


        }

        private void PopulatingUnderlyingAsset(object e)
        {
            try
            {

                #region BSE
                //for BSE Exchange
                if (Selected_EXCH == Enumerations.Order.Exchanges.BSE.ToString())
                {
                    #region Derivative
                    //for derivative 
                    if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Derivative.ToString())
                    {
                        if (IntrTypeSelected == Enumerations.Order.DervInstrumentType.FutIndex.ToString())
                        {
                            ShortIntrType = "IF";
                            SubIntrTypeSelected = string.Empty;
                            StrikePriceEnable = false;
                        }
                        else if (IntrTypeSelected == Enumerations.Order.DervInstrumentType.FutStock.ToString())
                        {
                            ShortIntrType = "SF";
                            SubIntrTypeSelected = string.Empty;
                            StrikePriceEnable = false;
                        }
                        else if (IntrTypeSelected == Enumerations.Order.DervInstrumentType.CallIndex.ToString())
                        {
                            ShortIntrType = "IO";
                            SubIntrTypeSelected = "CE";
                            StrikePriceEnable = true;
                        }
                        else if (IntrTypeSelected == Enumerations.Order.DervInstrumentType.CallStock.ToString())
                        {
                            ShortIntrType = "SO";
                            SubIntrTypeSelected = "CE";
                            StrikePriceEnable = true;
                        }
                        else if (IntrTypeSelected == Enumerations.Order.DervInstrumentType.PutIndex.ToString())
                        {
                            ShortIntrType = "IO";
                            SubIntrTypeSelected = "PE";
                            StrikePriceEnable = true;
                        }
                        else if (IntrTypeSelected == Enumerations.Order.DervInstrumentType.PutStock.ToString())
                        {
                            ShortIntrType = "SO";
                            SubIntrTypeSelected = "PE";
                            StrikePriceEnable = true;
                        }
                        else if (IntrTypeSelected == Enumerations.Order.DervInstrumentType.PairOption.ToString())
                        {
                            StrategyID = 15;
                            StrikePriceEnable = true;
                        }



                        if (!(IntrTypeSelected == Enumerations.Order.DervInstrumentType.PairOption.ToString()))
                        {

                            ScripIDColl = new ObservableCollection<string>();
                            foreach (var item in MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Cast<DerivativeMasterBase>().Where(x => x.InstrumentType == ShortIntrType && !string.IsNullOrEmpty(x.ScripId) && x.ContractTokenNum != 0).Select(x => x.ScripId).Distinct())
                            {
                                ScripIDColl.Add(item);
                            }
                            if (ScripIDColl.Count() > 0 && ScripIDColl != null)
                            {
                                ScripIDSelected = ScripIDColl[0];
                            }

                            ScripCodeList = new List<long>();
                            foreach (var item in MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Cast<DerivativeMasterBase>().Where(x => x.InstrumentType == ShortIntrType && !string.IsNullOrEmpty(x.ScripId) && x.ContractTokenNum != 0).Select(x => x.ContractTokenNum).Distinct())
                            {
                                ScripCodeList.Add(item);
                            }
                            if (ScripCodeList.Count() > 0 && ScripCodeList != null)
                            {
                                ScripCodeSelected = ScripCodeList[0];
                            }

                            UnderLyingAssetLst = new ObservableCollection<string>();
                            foreach (var item in MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Cast<DerivativeMasterBase>().OrderBy(y => y.UnderlyingAsset).Where(x => x.InstrumentType == ShortIntrType).Select(x => x.UnderlyingAsset).Distinct())
                            {
                                UnderLyingAssetLst.Add(item);
                            }
                            if (UnderLyingAssetLst.Count() > 0 && UnderLyingAssetLst != null)
                            {
                                UnderAssetSelected = UnderLyingAssetLst[0];
                            }
                        }
                        else if (IntrTypeSelected == Enumerations.Order.DervInstrumentType.PairOption.ToString())
                        {

                            ScripIDColl = new ObservableCollection<string>();
                            foreach (var item in MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Cast<DerivativeMasterBase>().Where(x => x.StrategyID == StrategyID && !string.IsNullOrEmpty(x.ScripId) && x.ContractTokenNum != 0).Select(x => x.ScripId).Distinct())
                            {
                                ScripIDColl.Add(item);
                            }
                            if (ScripIDColl.Count() > 0 && ScripIDColl != null)
                            {
                                ScripIDSelected = ScripIDColl[0];
                            }

                            ScripCodeList = new List<long>();
                            foreach (var item in MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Cast<DerivativeMasterBase>().Where(x => x.StrategyID == StrategyID && !string.IsNullOrEmpty(x.ScripId) && x.ContractTokenNum != 0).Select(x => x.ContractTokenNum).Distinct())
                            {
                                ScripCodeList.Add(item);
                            }
                            if (ScripCodeList.Count() > 0 && ScripCodeList != null)
                            {
                                ScripCodeSelected = ScripCodeList[0];
                            }

                            UnderLyingAssetLst = new ObservableCollection<string>();
                            foreach (var item in MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Cast<DerivativeMasterBase>().OrderBy(y => y.UnderlyingAsset).Where(x => x.StrategyID == StrategyID).Select(x => x.UnderlyingAsset).Distinct())
                            {
                                UnderLyingAssetLst.Add(item);
                            }
                            if (UnderLyingAssetLst.Count() > 0 && UnderLyingAssetLst != null)
                            {
                                UnderAssetSelected = UnderLyingAssetLst[0];
                            }
                        }




                    }
                    #endregion

                    #region Currency
                    //BSE CURRENCY 

                    if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Currency.ToString())
                    {
                        if (IntrTypeSelected == Enumerations.Order.CurrInstrumentType.Future.ToString())
                        {
                            ShortIntrType = "OPTCUR";
                            SubIntrTypeSelected = string.Empty;
                            StrikePriceEnable = false;
                        }
                        else if (IntrTypeSelected == Enumerations.Order.CurrInstrumentType.Call.ToString())
                        {
                            ShortIntrType = "OPTCUR";
                            SubIntrTypeSelected = "CE";
                            StrikePriceEnable = true;
                        }
                        else if (IntrTypeSelected == Enumerations.Order.CurrInstrumentType.Put.ToString())
                        {
                            ShortIntrType = "OPTCUR";
                            SubIntrTypeSelected = "PE";
                            StrikePriceEnable = true;
                        }
                        else if (IntrTypeSelected == Enumerations.Order.CurrInstrumentType.PairOption.ToString())
                        {
                            StrategyID = 15;
                            StrikePriceEnable = true;
                        }
                        else if (IntrTypeSelected == Enumerations.Order.CurrInstrumentType.Straddle.ToString())
                        {
                            StrategyID = 28;
                            StrikePriceEnable = true;
                        }


                        if (IntrTypeSelected == Enumerations.Order.CurrInstrumentType.Future.ToString())
                        {
                            ScripIDColl = new ObservableCollection<string>();
                            foreach (var item in MasterSharedMemory.objMasterCurrencyDictBaseBSE.Values.Cast<CurrencyMasterBase>().Where(x => x.InstrumentType != ShortIntrType && !string.IsNullOrEmpty(x.ScripId) && x.ContractTokenNum != 0).Select(x => x.ScripId).Distinct())
                            {
                                ScripIDColl.Add(item);
                            }
                            if (ScripIDColl.Count() > 0 && ScripIDColl != null)
                            {
                                ScripIDSelected = ScripIDColl[0];
                            }

                            ScripCodeList = new List<long>();
                            foreach (var item in MasterSharedMemory.objMasterCurrencyDictBaseBSE.Values.Cast<CurrencyMasterBase>().Where(x => x.InstrumentType != ShortIntrType && !string.IsNullOrEmpty(x.ScripId) && x.ContractTokenNum != 0).Select(x => x.ContractTokenNum).Distinct())
                            {
                                ScripCodeList.Add(item);
                            }
                            if (ScripCodeList.Count() > 0 && ScripCodeList != null)
                            {
                                ScripCodeSelected = ScripCodeList[0];
                            }

                            UnderLyingAssetLst = new ObservableCollection<string>();
                            foreach (var item in MasterSharedMemory.objMasterCurrencyDictBaseBSE.Values.Cast<CurrencyMasterBase>().OrderByDescending(y => y.UnderlyingAsset).Where(x => x.InstrumentType != ShortIntrType).Select(x => x.UnderlyingAsset).Distinct())
                            {
                                UnderLyingAssetLst.Add(item);
                            }
                            if (UnderLyingAssetLst.Count() > 0 && UnderLyingAssetLst != null)
                            {
                                UnderAssetSelected = UnderLyingAssetLst[0];
                            }
                        }
                        else if (IntrTypeSelected == Enumerations.Order.CurrInstrumentType.Call.ToString() || IntrTypeSelected == Enumerations.Order.CurrInstrumentType.Put.ToString())
                        {

                            ScripIDColl = new ObservableCollection<string>();
                            foreach (var item in MasterSharedMemory.objMasterCurrencyDictBaseBSE.Values.Cast<CurrencyMasterBase>().Where(x => x.InstrumentType == ShortIntrType && !string.IsNullOrEmpty(x.ScripId) && x.ContractTokenNum != 0).Select(x => x.ScripId).Distinct())
                            {
                                ScripIDColl.Add(item);
                            }
                            if (ScripIDColl.Count() > 0 && ScripIDColl != null)
                            {
                                ScripIDSelected = ScripIDColl[0];
                            }

                            ScripCodeList = new List<long>();
                            foreach (var item in MasterSharedMemory.objMasterCurrencyDictBaseBSE.Values.Cast<CurrencyMasterBase>().Where(x => x.InstrumentType == ShortIntrType && !string.IsNullOrEmpty(x.ScripId) && x.ContractTokenNum != 0).Select(x => x.ContractTokenNum).Distinct())
                            {
                                ScripCodeList.Add(item);
                            }
                            if (ScripCodeList.Count() > 0 && ScripCodeList != null)
                            {
                                ScripCodeSelected = ScripCodeList[0];
                            }

                            UnderLyingAssetLst = new ObservableCollection<string>();
                            foreach (var item in MasterSharedMemory.objMasterCurrencyDictBaseBSE.Values.Cast<CurrencyMasterBase>().OrderByDescending(y => y.UnderlyingAsset).Where(x => x.InstrumentType == ShortIntrType).Select(x => x.UnderlyingAsset).Distinct())
                            {
                                UnderLyingAssetLst.Add(item);
                            }
                            if (UnderLyingAssetLst.Count() > 0 && UnderLyingAssetLst != null)
                            {
                                UnderAssetSelected = UnderLyingAssetLst[0];
                            }
                        }
                        else if (IntrTypeSelected == Enumerations.Order.CurrInstrumentType.PairOption.ToString() || IntrTypeSelected == Enumerations.Order.CurrInstrumentType.Straddle.ToString())
                        {

                            ScripIDColl = new ObservableCollection<string>();
                            foreach (var item in MasterSharedMemory.objMasterCurrencyDictBaseBSE.Values.Cast<CurrencyMasterBase>().Where(x => x.StrategyID == StrategyID && !string.IsNullOrEmpty(x.ScripId) && x.ContractTokenNum != 0).Select(x => x.ScripId).Distinct())
                            {
                                ScripIDColl.Add(item);
                            }
                            if (ScripIDColl.Count() > 0 && ScripIDColl != null)
                            {
                                ScripIDSelected = ScripIDColl[0];
                            }

                            ScripCodeList = new List<long>();
                            foreach (var item in MasterSharedMemory.objMasterCurrencyDictBaseBSE.Values.Cast<CurrencyMasterBase>().Where(x => x.StrategyID == StrategyID && !string.IsNullOrEmpty(x.ScripId) && x.ContractTokenNum != 0).Select(x => x.ContractTokenNum).Distinct())
                            {
                                ScripCodeList.Add(item);
                            }
                            if (ScripCodeList.Count() > 0 && ScripCodeList != null)
                            {
                                ScripCodeSelected = ScripCodeList[0];
                            }

                            UnderLyingAssetLst = new ObservableCollection<string>();
                            foreach (var item in MasterSharedMemory.objMasterCurrencyDictBaseBSE.Values.Cast<CurrencyMasterBase>().OrderByDescending(y => y.UnderlyingAsset).Where(x => x.StrategyID == StrategyID).Select(x => x.UnderlyingAsset).Distinct())
                            {
                                UnderLyingAssetLst.Add(item);
                            }
                            if (UnderLyingAssetLst.Count() > 0 && UnderLyingAssetLst != null)
                            {
                                UnderAssetSelected = UnderLyingAssetLst[0];
                            }
                        }
                    }
                    #endregion


                }

                #endregion

                #region NSE
                //NSE Derivative
                //for NSE Exchange
                if (Selected_EXCH == Enumerations.Order.Exchanges.NSE.ToString())
                {
                    //for derivative market 
                    if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Derivative.ToString())
                    {
                        if (IntrTypeSelected == "OPTIDX" || IntrTypeSelected == "OPTSTK")
                        {
                            StrikePriceEnable = true;
                            UnderLyingAssetLst = new ObservableCollection<string>();
                            ExpDateLst = new ObservableCollection<string>();
                            foreach (var item in MasterSharedMemory.objMasterDerivativeDictBaseNSE.Values.Cast<DerivativeMasterBase>().Where(x => x.InstrumentType == ShortIntrType).Select(x => x.UnderlyingAsset).Distinct())
                            {
                                UnderLyingAssetLst.Add(item);
                            }
                            if (UnderLyingAssetLst.Count() > 0 && UnderLyingAssetLst != null)
                            {
                                UnderAssetSelected = UnderLyingAssetLst[0];

                            }

                        }
                        if (IntrTypeSelected == "FUTIDX" || IntrTypeSelected == "FUTSTK" || IntrTypeSelected == "FUTIVX")
                        {
                            StrikePriceEnable = false;
                            UnderLyingAssetLst = new ObservableCollection<string>();
                            ExpDateLst = new ObservableCollection<string>();
                            ScripIDColl = new ObservableCollection<string>();
                            foreach (var item in MasterSharedMemory.objMasterDerivativeDictBaseNSE.Values.Cast<DerivativeMasterBase>().Where(x => x.InstrumentType == ShortIntrType).Select(x => x.UnderlyingAsset).Distinct())
                            {
                                UnderLyingAssetLst.Add(item);
                            }
                            if (UnderLyingAssetLst.Count() > 0 && UnderLyingAssetLst != null)
                            {
                                UnderAssetSelected = UnderLyingAssetLst[0];


                            }
                        }
                    }
                    //NSE CURRENCY 

                    if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Currency.ToString())
                    {
                        if (IntrTypeSelected == "OPTCUR")
                        {
                            StrikePriceEnable = true;
                            UnderLyingAssetLst = new ObservableCollection<string>();
                            ExpDateLst = new ObservableCollection<string>();
                            foreach (var item in MasterSharedMemory.objMasterCurrencyDictBaseNSE.Values.Cast<CurrencyMasterBase>().Where(x => x.InstrumentType == ShortIntrType).Select(x => x.UnderlyingAsset).Distinct())
                            {
                                UnderLyingAssetLst.Add(item);
                            }
                            if (UnderLyingAssetLst.Count() > 0 && UnderLyingAssetLst != null)
                            {
                                UnderAssetSelected = UnderLyingAssetLst[0];


                            }

                        }
                        if (IntrTypeSelected == "UNDCUR" || IntrTypeSelected == "INDEX" || IntrTypeSelected == "UNDIRC" || IntrTypeSelected == "UNDIRD" || IntrTypeSelected == "UNDIRT" || IntrTypeSelected == "FUTIRD" || IntrTypeSelected == "FUTCUR" || IntrTypeSelected == "FUTIRT")
                        {
                            StrikePriceEnable = false;
                            UnderLyingAssetLst = new ObservableCollection<string>();
                            ExpDateLst = new ObservableCollection<string>();
                            ScripIDColl = new ObservableCollection<string>();
                            foreach (var item in MasterSharedMemory.objMasterCurrencyDictBaseNSE.Values.Cast<CurrencyMasterBase>().Where(x => x.InstrumentType == ShortIntrType).Select(x => x.UnderlyingAsset).Distinct())
                            {
                                UnderLyingAssetLst.Add(item);
                            }
                            if (UnderLyingAssetLst.Count() > 0 && UnderLyingAssetLst != null)
                            {
                                UnderAssetSelected = UnderLyingAssetLst[0];


                            }
                        }
                    }
                }
                #endregion

            }

            catch (Exception ex)
            {

                ExceptionUtility.LogError(ex);
            }
        }

        private void OnChangeOfUnderAsset(object e)
        {
            try
            {
                if (!ChangedFromScripCode || ChangeCounter == 0)
                {
                    if (Selected_EXCH == Enumerations.Order.Exchanges.BSE.ToString())
                    {
                        #region Derivative
                        //BSE DERIVATIVE
                        if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Derivative.ToString())
                        {
                            if (!(IntrTypeSelected == Enumerations.Order.DervInstrumentType.PairOption.ToString()))
                            {
                                if (UnderLyingAssetLst.Count() > 0 && UnderLyingAssetLst != null)
                                {
                                    ExpDateLst = new ObservableCollection<string>();
                                    if (StrikePriceEnable == false)
                                    {
                                        List<DateTime> temp = new List<DateTime>();
                                        foreach (var item in MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Cast<DerivativeMasterBase>().Where(x => x.UnderlyingAsset == UnderAssetSelected && x.InstrumentType == ShortIntrType).Select(x => x.ExpiryDate).Distinct())
                                        {
                                            var convertedDate = Convert.ToDateTime(item);
                                            temp.Add(convertedDate.Date);
                                        }
                                        foreach (var dateMonthWise in temp.OrderBy(x => x.Month).OrderBy(y => y.Year).OrderBy(z => z.Date))//default ascending
                                        {
                                            var date = string.Format("{0:dd-MM-yyyy}", dateMonthWise);
                                            ExpDateLst.Add(date);
                                        }

                                        ScripIDSelected = MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Where(x => x.UnderlyingAsset == UnderAssetSelected && x.InstrumentType == ShortIntrType).Select(x => x.ScripId).FirstOrDefault();
                                    }
                                    else if (StrikePriceEnable == true)
                                    {
                                        List<DateTime> temp = new List<DateTime>();
                                        foreach (var item in MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Cast<DerivativeMasterBase>().Where(x => x.UnderlyingAsset == UnderAssetSelected && x.InstrumentType == ShortIntrType && x.OptionType == SubIntrTypeSelected).Select(x => x.ExpiryDate).Distinct())
                                        {
                                            var convertedDate = Convert.ToDateTime(item);
                                            temp.Add(convertedDate.Date);
                                        }
                                        foreach (var dateMonthWise in temp.OrderBy(x => x.Month).OrderBy(y => y.Year).OrderBy(z => z.Date))//default ascending
                                        {
                                            var date = string.Format("{0:dd-MM-yyyy}", dateMonthWise);
                                            ExpDateLst.Add(date);
                                        }

                                        ScripIDSelected = MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Where(x => x.UnderlyingAsset == UnderAssetSelected && x.InstrumentType == ShortIntrType && x.OptionType == SubIntrTypeSelected).Select(x => x.ScripId).FirstOrDefault();
                                    }
                                    ChangeCounter = 0;
                                    if (ExpDateLst.Count > 0 && ExpDateLst != null)
                                    {
                                        ExpDateSelected = ExpDateLst[0];
                                    }

                                }

                            }
                            else if (IntrTypeSelected == Enumerations.Order.DervInstrumentType.PairOption.ToString())
                            {
                                if (UnderLyingAssetLst.Count() > 0 && UnderLyingAssetLst != null)
                                {
                                    ExpDateLst = new ObservableCollection<string>();
                                    if (StrikePriceEnable == true)
                                    {
                                        List<DateTime> temp = new List<DateTime>();
                                        foreach (var item in MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Cast<DerivativeMasterBase>().Where(x => x.UnderlyingAsset == UnderAssetSelected && x.StrategyID == StrategyID).Select(x => x.ExpiryDate).Distinct())
                                        {
                                            var convertedDate = Convert.ToDateTime(item);
                                            temp.Add(convertedDate.Date);
                                        }
                                        foreach (var dateMonthWise in temp.OrderBy(x => x.Month).OrderBy(y => y.Year).OrderBy(z => z.Date))//default ascending
                                        {
                                            var date = string.Format("{0:dd-MM-yyyy}", dateMonthWise);
                                            ExpDateLst.Add(date);
                                        }
                                        ScripIDSelected = MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Where(x => x.UnderlyingAsset == UnderAssetSelected && x.StrategyID == StrategyID).Select(x => x.ScripId).FirstOrDefault();
                                    }
                                    ChangeCounter = 0;

                                    if (ExpDateLst.Count > 0 && ExpDateLst != null)
                                    {
                                        ExpDateSelected = ExpDateLst[0];
                                    }

                                }
                            }

                        }
                        #endregion

                        #region Currency
                        //BSE CURRENCY 
                        if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Currency.ToString())
                        {
                            if (IntrTypeSelected == Enumerations.Order.CurrInstrumentType.Future.ToString())
                            {
                                if (UnderLyingAssetLst.Count() > 0 && UnderLyingAssetLst != null)
                                {
                                    ExpDateLst = new ObservableCollection<string>();
                                    if (StrikePriceEnable == false)
                                    {
                                        List<DateTime> temp = new List<DateTime>();
                                        foreach (var item in MasterSharedMemory.objMasterCurrencyDictBaseBSE.Values.Cast<CurrencyMasterBase>().Where(x => x.UnderlyingAsset == UnderAssetSelected && x.InstrumentType != ShortIntrType).Select(x => x.ExpiryDate).Distinct())
                                        {
                                            var convertedDate = Convert.ToDateTime(item);
                                            temp.Add(convertedDate.Date);
                                        }
                                        foreach (var dateMonthWise in temp.OrderBy(x => x.Month).OrderBy(y => y.Year).OrderBy(z => z.Date))//default ascending
                                        {
                                            var date = string.Format("{0:dd-MM-yyyy}", dateMonthWise);
                                            ExpDateLst.Add(date);
                                        }

                                        ScripIDSelected = MasterSharedMemory.objMasterCurrencyDictBaseBSE.Values.Where(x => x.UnderlyingAsset == UnderAssetSelected && x.InstrumentType == ShortIntrType).Select(x => x.ScripId).FirstOrDefault();
                                    }
                                    ChangeCounter = 0;

                                    if (ExpDateLst.Count > 0 && ExpDateLst != null)
                                    {
                                        ExpDateSelected = ExpDateLst[0];
                                    }

                                }
                            }

                            else if (IntrTypeSelected == Enumerations.Order.CurrInstrumentType.Call.ToString() || IntrTypeSelected == Enumerations.Order.CurrInstrumentType.Put.ToString())
                            {
                                if (UnderLyingAssetLst.Count() > 0 && UnderLyingAssetLst != null)
                                {
                                    ExpDateLst = new ObservableCollection<string>();
                                    if (StrikePriceEnable == false)
                                    {
                                        List<DateTime> temp = new List<DateTime>();
                                        foreach (var item in MasterSharedMemory.objMasterCurrencyDictBaseBSE.Values.Cast<CurrencyMasterBase>().Where(x => x.UnderlyingAsset == UnderAssetSelected && x.InstrumentType == ShortIntrType).Select(x => x.ExpiryDate).Distinct())
                                        {
                                            var convertedDate = Convert.ToDateTime(item);
                                            temp.Add(convertedDate.Date);
                                        }
                                        foreach (var dateMonthWise in temp.OrderBy(x => x.Month).OrderBy(y => y.Year).OrderBy(z => z.Date))//default ascending
                                        {
                                            var date = string.Format("{0:dd-MM-yyyy}", dateMonthWise);
                                            ExpDateLst.Add(date);
                                        }

                                        ScripIDSelected = MasterSharedMemory.objMasterCurrencyDictBaseBSE.Values.Where(x => x.UnderlyingAsset == UnderAssetSelected && x.InstrumentType == ShortIntrType).Select(x => x.ScripId).FirstOrDefault();
                                    }
                                    else if (StrikePriceEnable == true)
                                    {
                                        List<DateTime> temp = new List<DateTime>();
                                        foreach (var item in MasterSharedMemory.objMasterCurrencyDictBaseBSE.Values.Cast<CurrencyMasterBase>().Where(x => x.UnderlyingAsset == UnderAssetSelected && x.InstrumentType == ShortIntrType && x.OptionType == SubIntrTypeSelected).Select(x => x.ExpiryDate).Distinct())
                                        {
                                            var convertedDate = Convert.ToDateTime(item);
                                            temp.Add(convertedDate.Date);
                                        }
                                        foreach (var dateMonthWise in temp.OrderBy(x => x.Month).OrderBy(y => y.Year).OrderBy(z => z.Date))//default ascending
                                        {
                                            var date = string.Format("{0:dd-MM-yyyy}", dateMonthWise);
                                            ExpDateLst.Add(date);
                                        }

                                        ScripIDSelected = MasterSharedMemory.objMasterCurrencyDictBaseBSE.Values.Where(x => x.UnderlyingAsset == UnderAssetSelected && x.InstrumentType == ShortIntrType && x.OptionType == SubIntrTypeSelected).Select(x => x.ScripId).FirstOrDefault();

                                    }

                                    ChangeCounter = 0;

                                    if (ExpDateLst.Count > 0 && ExpDateLst != null)
                                    {
                                        ExpDateSelected = ExpDateLst[0];
                                    }

                                }

                            }
                            else if (IntrTypeSelected == Enumerations.Order.CurrInstrumentType.PairOption.ToString() || IntrTypeSelected == Enumerations.Order.CurrInstrumentType.Straddle.ToString())
                            {
                                if (UnderLyingAssetLst.Count() > 0 && UnderLyingAssetLst != null)
                                {
                                    ExpDateLst = new ObservableCollection<string>();
                                    if (StrikePriceEnable == true)
                                    {
                                        List<DateTime> temp = new List<DateTime>();
                                        foreach (var item in MasterSharedMemory.objMasterCurrencyDictBaseBSE.Values.Cast<CurrencyMasterBase>().Where(x => x.UnderlyingAsset == UnderAssetSelected && x.StrategyID == StrategyID).Select(x => x.ExpiryDate).Distinct())
                                        {
                                            var convertedDate = Convert.ToDateTime(item);
                                            temp.Add(convertedDate.Date);
                                        }
                                        foreach (var dateMonthWise in temp.OrderBy(x => x.Month).OrderBy(y => y.Year).OrderBy(z => z.Date))//default ascending
                                        {
                                            var date = string.Format("{0:dd-MM-yyyy}", dateMonthWise);
                                            ExpDateLst.Add(date);
                                        }

                                        ScripIDSelected = MasterSharedMemory.objMasterCurrencyDictBaseBSE.Values.Where(x => x.UnderlyingAsset == UnderAssetSelected && x.StrategyID == StrategyID).Select(x => x.ScripId).FirstOrDefault();

                                    }
                                    ChangeCounter = 0;
                                    if (ExpDateLst.Count > 0 && ExpDateLst != null)
                                    {
                                        ExpDateSelected = ExpDateLst[0];
                                    }

                                }
                            }

                        }

                        #endregion

                    }
                }


            }
            catch (Exception ex)
            {

                ExceptionUtility.LogError(ex);
            }

        }

        private void PopulatingExpDate(object e)
        {
            try
            {
                //BSE 
                if (!ChangedFromScripCode || ChangeCounter == 0)
                {
                    if (Selected_EXCH == Enumerations.Order.Exchanges.BSE.ToString())
                    {
                        #region Derivative
                        //BSE DERIVATIVE
                        if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Derivative.ToString())
                        {
                            if (!(IntrTypeSelected == Enumerations.Order.DervInstrumentType.PairOption.ToString()))
                            {
                                if (ExpDateLst.Count() > 0 && ExpDateLst != null)
                                {
                                    //ExpDateSelected = string.Format("{0:dd-mmm-yyyy}", ExpDateSelected); 
                                    string TempExpiryDate = DateTime.ParseExact(ExpDateSelected, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy"); //$"{ Convert.ToDateTime(ExpDateSelected):dd-MMM-yyyy}";
                                    //ExpDateSelected = TempExpiryDate.ToUpper();

                                    if (StrikePriceEnable == true)
                                    {
                                        ScripIDSelected = MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Where(x => x.UnderlyingAsset == UnderAssetSelected && x.InstrumentType == ShortIntrType && x.OptionType == SubIntrTypeSelected && x.ExpiryDate.Trim().ToUpper() == TempExpiryDate.Trim().ToUpper()).Select(x => x.ScripId).FirstOrDefault();

                                        StkPrcLst = new ObservableCollection<string>();
                                        foreach (var item in MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Cast<DerivativeMasterBase>().Where(x => x.UnderlyingAsset == UnderAssetSelected && x.InstrumentType == ShortIntrType && x.ExpiryDate.Trim().ToUpper() == TempExpiryDate.Trim().ToUpper() && x.OptionType == SubIntrTypeSelected).Select(x => x.StrikePrice))
                                        {
                                            //StkPrcLst.Add(item);
                                            StkPrcLst.Add(Convert.ToString(Math.Truncate(Convert.ToDecimal(CommonFunctions.DisplayInDecimalFormatTouch(item, DecimalPoint)))));
                                        }
                                        if (StkPrcLst.Count > 0 && StkPrcLst != null)
                                        {
                                            StkPriceSelected = StkPrcLst[0];
                                        }
                                    }
                                    else if (StrikePriceEnable == false)
                                    {
                                        ScripIDSelected = MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Where(x => x.UnderlyingAsset == UnderAssetSelected && x.InstrumentType == ShortIntrType && x.ExpiryDate.Trim().ToUpper() == TempExpiryDate.Trim().ToUpper()).Select(x => x.ScripId).FirstOrDefault();
                                    }
                                }
                            }
                            else if (IntrTypeSelected == Enumerations.Order.DervInstrumentType.PairOption.ToString())
                            {
                                if (ExpDateLst.Count() > 0 && ExpDateLst != null)
                                {
                                    string TempExpiryDate = DateTime.ParseExact(ExpDateSelected, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy");//$"{ Convert.ToDateTime(ExpDateSelected):dd-MMM-yyyy}";
                                    //ExpDateSelected = TempExpiryDate.ToUpper();

                                    if (StrikePriceEnable == true)
                                    {
                                        ScripIDSelected = MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Where(x => x.UnderlyingAsset == UnderAssetSelected && x.StrategyID == StrategyID && x.ExpiryDate.Trim().ToUpper() == TempExpiryDate.Trim().ToUpper()).Select(x => x.ScripId).FirstOrDefault();

                                        StkPrcLst = new ObservableCollection<string>();
                                        foreach (var item in MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Cast<DerivativeMasterBase>().Where(x => x.UnderlyingAsset == UnderAssetSelected && x.StrategyID == StrategyID && x.ExpiryDate.Trim().ToUpper() == TempExpiryDate.Trim().ToUpper()).Select(x => x.StrikePrice))
                                        {
                                            //StkPrcLst.Add(item);
                                            StkPrcLst.Add(Convert.ToString(Math.Truncate(Convert.ToDecimal(CommonFunctions.DisplayInDecimalFormatTouch(item, DecimalPoint)))));
                                        }
                                        if (StkPrcLst.Count > 0 && StkPrcLst != null)
                                        {
                                            StkPriceSelected = StkPrcLst[0];
                                        }
                                    }
                                }
                            }



                            if (ExpDateLst.Count() == 0 || ExpDateLst == null)
                            {
                                StkPriceSelected = string.Empty;
                                StkPrcLst = new ObservableCollection<string>();
                            }
                        }

                        #endregion

                        #region Currency
                        //BSE CURRENCY
                        if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Currency.ToString())
                        {
                            if (IntrTypeSelected == Enumerations.Order.CurrInstrumentType.Future.ToString())
                            {
                                if (ExpDateLst.Count() > 0 && ExpDateLst != null)
                                {
                                    string TempExpiryDate = DateTime.ParseExact(ExpDateSelected, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy"); //$"{ Convert.ToDateTime(ExpDateSelected):dd-MMM-yyyy}";
                                    //ExpDateSelected = TempExpiryDate.ToUpper();

                                    if (StrikePriceEnable == false)
                                    {
                                        ScripIDSelected = MasterSharedMemory.objMasterCurrencyDictBaseBSE.Values.Where(x => x.UnderlyingAsset == UnderAssetSelected && x.InstrumentType != ShortIntrType && x.ExpiryDate == TempExpiryDate.ToUpper()).Select(x => x.ScripId).FirstOrDefault();
                                    }
                                }
                            }
                            else if (IntrTypeSelected == Enumerations.Order.CurrInstrumentType.Call.ToString() || IntrTypeSelected == Enumerations.Order.CurrInstrumentType.Put.ToString())
                            {
                                if (ExpDateLst.Count() > 0 && ExpDateLst != null)
                                {
                                    string TempExpiryDate = DateTime.ParseExact(ExpDateSelected, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy"); //$"{ Convert.ToDateTime(ExpDateSelected):dd-MMM-yyyy}";
                                    //var ExpDateSelect = TempExpiryDate.ToUpper();

                                    if (StrikePriceEnable == true)
                                    {
                                        ScripIDSelected = MasterSharedMemory.objMasterCurrencyDictBaseBSE.Values.Where(x => x.UnderlyingAsset == UnderAssetSelected && x.InstrumentType == ShortIntrType && x.OptionType == SubIntrTypeSelected && x.ExpiryDate.Trim().ToUpper() == TempExpiryDate.ToUpper()).Select(x => x.ScripId).FirstOrDefault();

                                        StkPrcLst = new ObservableCollection<string>();
                                        foreach (var item in MasterSharedMemory.objMasterCurrencyDictBaseBSE.Values.Cast<CurrencyMasterBase>().Where(x => x.UnderlyingAsset == UnderAssetSelected && x.InstrumentType == ShortIntrType && x.ExpiryDate == TempExpiryDate.Trim().ToUpper() && x.OptionType == SubIntrTypeSelected).Select(x => x.StrikePrice))
                                        {
                                            //StkPrcLst.Add(item);
                                            StkPrcLst.Add(String.Format("{0:0.0000}", Convert.ToDecimal(CommonFunctions.DisplayInDecimalFormatTouch(item, DecimalPoint))).ToString());
                                            //StkPrcLst.Add(Convert.ToString(Math.Truncate(Convert.ToDecimal(CommonFunctions.DisplayInDecimalFormatTouch(item, DecimalPoint)))));
                                            //StkPrcLst.Add(String.Format("{0:0.0000}", Convert.ToString(Math.Truncate(Convert.ToDecimal(CommonFunctions.DisplayInDecimalFormatTouch(item, DecimalPoint))))).ToString());
                                        }
                                        if (StkPrcLst.Count > 0 && StkPrcLst != null)
                                        {
                                            StkPriceSelected = StkPrcLst[0];
                                        }
                                    }
                                    else if (StrikePriceEnable == false)
                                    {
                                        ScripIDSelected = MasterSharedMemory.objMasterCurrencyDictBaseBSE.Values.Where(x => x.UnderlyingAsset == UnderAssetSelected && x.InstrumentType == ShortIntrType && x.ExpiryDate == ExpDateSelected).Select(x => x.ScripId).FirstOrDefault();
                                    }
                                }
                            }
                            else if (IntrTypeSelected == Enumerations.Order.CurrInstrumentType.PairOption.ToString() || IntrTypeSelected == Enumerations.Order.CurrInstrumentType.Straddle.ToString())
                            {
                                if (ExpDateLst.Count() > 0 && ExpDateLst != null)
                                {
                                    string TempExpiryDate = DateTime.ParseExact(ExpDateSelected, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy"); //$"{ Convert.ToDateTime(ExpDateSelected):dd-MMM-yyyy}";
                                    //var ExpDateSelect = TempExpiryDate.ToUpper();

                                    if (StrikePriceEnable == true)
                                    {
                                        ScripIDSelected = MasterSharedMemory.objMasterCurrencyDictBaseBSE.Values.Where(x => x.UnderlyingAsset == UnderAssetSelected && x.StrategyID == StrategyID && x.ExpiryDate.Trim().ToUpper() == TempExpiryDate.Trim().ToUpper()).Select(x => x.ScripId).FirstOrDefault();

                                        StkPrcLst = new ObservableCollection<string>();
                                        foreach (var item in MasterSharedMemory.objMasterCurrencyDictBaseBSE.Values.Cast<CurrencyMasterBase>().Where(x => x.UnderlyingAsset == UnderAssetSelected && x.StrategyID == StrategyID && x.ExpiryDate.Trim().ToUpper() == TempExpiryDate.Trim().ToUpper()).Select(x => x.StrikePrice))
                                        {
                                            //StkPrcLst.Add(item);
                                            StkPrcLst.Add(String.Format("{0:0.0000}", Convert.ToDecimal(CommonFunctions.DisplayInDecimalFormatTouch(item, DecimalPoint))).ToString());
                                            //StkPrcLst.Add(Convert.ToString(Math.Truncate(Convert.ToDecimal(CommonFunctions.DisplayInDecimalFormatTouch(item, DecimalPoint)))));
                                        }
                                        if (StkPrcLst.Count > 0 && StkPrcLst != null)
                                        {
                                            StkPriceSelected = StkPrcLst[0];
                                        }
                                    }
                                }
                            }

                            if (ExpDateLst.Count() == 0 || ExpDateLst == null)
                            {
                                StkPriceSelected = string.Empty;
                                StkPrcLst = new ObservableCollection<string>();
                            }

                        }

                        #endregion

                    }
                }


            }
            catch (Exception ex)
            {

                ExceptionUtility.LogError(ex);
            }

        }

        private void OnChangeOfStkPrc(object e)
        {
            try
            {
                string TempExpiryDate = string.Empty;
                if (!ChangedFromScripCode || ChangeCounter == 0)
                {
                    if (!string.IsNullOrEmpty(ExpDateSelected))
                    {
                        TempExpiryDate = DateTime.ParseExact(ExpDateSelected, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy");//$"{ Convert.ToDateTime(ExpDateSelected):dd-MMM-yyyy}";
                    }
                    //BSE DERIVATIVES
                    if (Selected_EXCH == Enumerations.Order.Exchanges.BSE.ToString())
                    {
                        #region Derivative
                        if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Derivative.ToString())
                        {
                            if (!(IntrTypeSelected == Enumerations.Order.DervInstrumentType.PairOption.ToString()))
                            {
                                if (StkPrcLst.Count() > 0 && StkPrcLst != null)
                                {
                                    long StkPriceComp = Convert.ToInt64(Convert.ToDouble(StkPriceSelected) * Math.Pow(10, DecimalPoint));
                                    if (ScripIDColl.Count > 0 && ScripIDColl != null)
                                    {
                                        ScripIDSelected = MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Where(x => x.UnderlyingAsset == UnderAssetSelected && x.InstrumentType == ShortIntrType && x.OptionType == SubIntrTypeSelected && x.ExpiryDate.Trim().ToUpper() == TempExpiryDate.ToUpper().Trim() && x.StrikePrice == StkPriceComp).Select(x => x.ScripId).FirstOrDefault();
                                    }
                                    else
                                        ScripIDSelected = string.Empty;
                                }
                            }
                            else if (IntrTypeSelected == Enumerations.Order.DervInstrumentType.PairOption.ToString())
                            {
                                if (StkPrcLst.Count() > 0 && StkPrcLst != null)
                                {
                                    long StkPriceComp = Convert.ToInt64(Convert.ToDouble(StkPriceSelected) * Math.Pow(10, DecimalPoint));
                                    if (ScripIDColl.Count > 0 && ScripIDColl != null)
                                    {
                                        ScripIDSelected = MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Where(x => x.UnderlyingAsset == UnderAssetSelected && x.StrategyID == StrategyID && x.ExpiryDate.Trim().ToUpper() == TempExpiryDate.ToUpper().Trim() && x.StrikePrice == StkPriceComp).Select(x => x.ScripId).FirstOrDefault();
                                    }
                                    else
                                        ScripIDSelected = string.Empty;
                                }
                            }
                        }

                        #endregion

                        #region Currency
                        //BSE CURRENCY
                        if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Currency.ToString())
                        {
                            if (IntrTypeSelected == Enumerations.Order.CurrInstrumentType.Future.ToString() || IntrTypeSelected == Enumerations.Order.CurrInstrumentType.Call.ToString() || IntrTypeSelected == Enumerations.Order.CurrInstrumentType.Put.ToString())
                            {
                                if (StkPrcLst.Count() > 0 && StkPrcLst != null)
                                {
                                    long StkPriceComp = Convert.ToInt64(Convert.ToDouble(StkPriceSelected) * Math.Pow(10, DecimalPoint));
                                    if (ScripIDColl.Count > 0 && ScripIDColl != null)
                                    {
                                        ScripIDSelected = MasterSharedMemory.objMasterCurrencyDictBaseBSE.Values.Where(x => x.UnderlyingAsset == UnderAssetSelected && x.InstrumentType == ShortIntrType && x.OptionType == SubIntrTypeSelected && x.ExpiryDate.Trim().ToUpper() == TempExpiryDate.ToUpper().Trim() && x.StrikePrice == StkPriceComp).Select(x => x.ScripId).FirstOrDefault();
                                    }
                                    else
                                        ScripIDSelected = string.Empty;
                                }
                            }
                            else if (IntrTypeSelected == Enumerations.Order.CurrInstrumentType.PairOption.ToString() || IntrTypeSelected == Enumerations.Order.CurrInstrumentType.Straddle.ToString())
                            {
                                if (StkPrcLst.Count() > 0 && StkPrcLst != null)
                                {
                                    long StkPriceComp = Convert.ToInt64(Convert.ToDouble(StkPriceSelected) * Math.Pow(10, DecimalPoint));
                                    if (ScripIDColl.Count > 0 && ScripIDColl != null)
                                    {
                                        ScripIDSelected = MasterSharedMemory.objMasterCurrencyDictBaseBSE.Values.Where(x => x.UnderlyingAsset == UnderAssetSelected && x.StrategyID == StrategyID && x.ExpiryDate.Trim().ToUpper() == TempExpiryDate.ToUpper().Trim() && x.StrikePrice == StkPriceComp).Select(x => x.ScripId).FirstOrDefault();
                                    }
                                    else
                                        ScripIDSelected = string.Empty;
                                }
                            }
                        }
                        #endregion

                    }
                }


            }
            catch (Exception ex)
            {

                ExceptionUtility.LogError(ex);
            }
        }

        public void objBroadCastProcessor_OnBroadCastRecievedNew(Model.ScripDetails objScripDetails)
        {
            try
            {
                //int scripCode = objScripDetails.ScriptCode_BseToken_NseToken;
                ////ScripDetails objScripDetails = BroadcastMasterMemory.objScripDetailsConDict.Where(x => x.Key == scripCode).Select(x => x.Value).FirstOrDefault();
                //if (objScripDetails != null )
                //{
                //    AssetTokenNumber = objScripDetails.lastTradeRateL;
                //}
                // BestFiveVM.objBroadCastProcessor_OnBroadCastRecieved(scripCode);
                if (AssetTokenNumber == objScripDetails.ScriptCode_BseToken_NseToken)
                {
                    string StringFormat = string.Empty;
                    String Segment = CommonFunctions.GetSegmentID(objScripDetails.ScriptCode_BseToken_NseToken);

                    int DecimalPnt = CommonFunctions.GetDecimal(objScripDetails.ScriptCode_BseToken_NseToken, "BSE", Segment);//BSE EQUITY
                    if (DecimalPnt == 4)
                    {
                        //DeciVisiDev = "Hidden";
                        //DeciVisiEq = "Visible";
                        StringFormat = ".0000;.0000;#";
                    }
                    else if (DecimalPnt == 2)
                    {
                        //DeciVisiDev = "Visible";
                        //DeciVisiEq = "Hidden";
                        StringFormat = ".00;.00;#";
                    }
                    AssetValue = (objScripDetails.lastTradeRateL / Math.Pow(10, DecimalPnt)).ToString(StringFormat) == "0" ? "" : (objScripDetails.lastTradeRateL / Math.Pow(10, DecimalPnt)).ToString(StringFormat);
                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
                return;
            }
        }
        private void OnChangeOfScripCodeSelected(object e)
        {
            CorpActionData = CommonFunctions.GetCorpAction(ScripCodeSelected);
            AssetTokenNumber = CommonFunctions.GetAssetTokenNumber(ScripCodeSelected, ScripSelectedSegment, IntrTypeSelected);
            if (ScripSelectedSegment == "Derivative")
            {
                //if ((IntrTypeSelected == "FutStock" || IntrTypeSelected == "CallStock" || IntrTypeSelected == "PutStock"))
                //{
                AssetValue = CommonFunctions.GetLTPBCast(Convert.ToInt32(AssetTokenNumber), IntrTypeSelected);
                //}
                //else if (IntrTypeSelected == "FutIndex" || IntrTypeSelected == "CallIndex" || IntrTypeSelected == "PutIndex")
                // {

                // }
            }

            if (ScripCodeSelected != 0)
            {
                FetchNetPositionByScripCode(ScripCodeSelected);
                UtilityOrderDetails.GETInstance.GlobalScripSelectedCodeDerv = ScripCodeSelected;

                if (Selected_EXCH == Enumerations.Exchange.BSE.ToString())
                {
                    ScripIDSelected = CommonFunctions.GetScripId((long)ScripCodeSelected, Selected_EXCH, ScripSelectedSegment);//BSE Derivative

                    if (ScripIDSelected != null && ScripIDSelected != string.Empty && ScripIDSelected != "")
                    {
                        InstrumentName = CommonFunctions.GetScripName((long)ScripCodeSelected, Selected_EXCH, ScripSelectedSegment);
                    }
                    else
                    {
                        ScripIDSelected = string.Empty;
                    }
                }
                else if (Selected_EXCH == Enumerations.Exchange.NSE.ToString())
                {
                    ScripIDSelected = CommonFunctions.GetScripId((long)ScripCodeSelected, Selected_EXCH, ScripSelectedSegment);//NSE Derivative
                    if (ScripIDSelected != null && ScripIDSelected != string.Empty && ScripIDSelected != "")
                        InstrNameSelected = MasterSharedMemory.objMasterDerivativeDictBaseNSE[(long)ScripCodeSelected].InstrumentName;
                    else
                        ScripIDSelected = string.Empty;
                }

            }
            else
            {
                ScripIDSelected = string.Empty;
                InstrNameSelected = string.Empty;
            }

            //ErrorMsg = string.Empty;
        }

        private void OnChangeOfScripIDSelected(object e)
        {

            if (ScripIDSelected != null && ScripIDSelected != "")
            {
                if (Selected_EXCH == Enumerations.Exchange.BSE.ToString())
                {
                    ScripCodeSelected = CommonFunctions.GetScripCodeFromScripID(ScripIDSelected, Selected_EXCH, ScripSelectedSegment);
                    if (ScripCodeSelected != 0)
                    {
                        //Subscribe to BroadCast
                        SubscribetoBcastMemory((int)ScripCodeSelected);

                        if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Derivative.ToString())
                        {
                            InstrNameSelected = MasterSharedMemory.objMasterDerivativeDictBaseBSE[(long)ScripCodeSelected].InstrumentName;
                            ChangedFromScripCode = true;
                            UnderAssetSelected = MasterSharedMemory.objMasterDerivativeDictBaseBSE[(long)ScripCodeSelected].UnderlyingAsset;
                            foreach (var item in ExpDateLst)
                            {
                                if (DateTime.ParseExact(item, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("dd-MM-yyyy").Equals(Convert.ToDateTime(MasterSharedMemory.objMasterDerivativeDictBaseBSE[(long)ScripCodeSelected].ExpiryDate).ToString("dd-MM-yyyy")))
                                {
                                    ExpDateSelected = item;

                                    string TempExpiryDate = DateTime.ParseExact(ExpDateSelected, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy"); // $"{ Convert.ToDateTime(ExpDateSelected):dd-MMM-yyyy}";
                                    StkPrcLst = new ObservableCollection<string>();

                                    if (!(IntrTypeSelected == Enumerations.Order.DervInstrumentType.PairOption.ToString()))
                                    {
                                        foreach (var item1 in MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Cast<DerivativeMasterBase>().Where(x => x.UnderlyingAsset == UnderAssetSelected && x.InstrumentType == ShortIntrType && x.ExpiryDate.Trim().ToUpper() == TempExpiryDate.Trim().ToUpper() && x.OptionType == SubIntrTypeSelected).Select(x => x.StrikePrice))
                                        {
                                            StkPrcLst.Add(Convert.ToString(Math.Truncate(Convert.ToDecimal(CommonFunctions.DisplayInDecimalFormatTouch(item1, DecimalPoint)))));
                                        }
                                    }
                                    else
                                    {
                                        foreach (var item1 in MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Cast<DerivativeMasterBase>().Where(x => x.UnderlyingAsset == UnderAssetSelected && x.StrategyID == StrategyID && x.ExpiryDate.Trim().ToUpper() == TempExpiryDate.Trim().ToUpper()).Select(x => x.StrikePrice))
                                        {
                                            //StkPrcLst.Add(item);
                                            StkPrcLst.Add(Convert.ToString(Math.Truncate(Convert.ToDecimal(CommonFunctions.DisplayInDecimalFormatTouch(item1, DecimalPoint)))));
                                        }
                                    }
                                    if (StkPrcLst.Count > 0 && StkPrcLst != null)
                                    {
                                        foreach (var item2 in StkPrcLst)
                                        {
                                            if (Convert.ToDouble(item2) * Math.Pow(10, DecimalPoint) == Convert.ToDouble(MasterSharedMemory.objMasterDerivativeDictBaseBSE[(long)ScripCodeSelected].StrikePrice))
                                                StkPriceSelected = item2;
                                        }
                                    }

                                }
                            }

                            ChangedFromScripCode = false;
                            ChangeCounter = 1;

                        }
                        else if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Currency.ToString())
                        {
                            InstrNameSelected = MasterSharedMemory.objMasterCurrencyDictBaseBSE[(long)ScripCodeSelected].InstrumentName;
                            ChangedFromScripCode = true;
                            UnderAssetSelected = MasterSharedMemory.objMasterCurrencyDictBaseBSE[(long)ScripCodeSelected].UnderlyingAsset;
                            //ExpDateSelected = MasterSharedMemory.objMasterCurrencyDictBaseBSE[(long)ScripCodeSelected].ExpiryDate;
                            foreach (var item in ExpDateLst)
                            {
                                if (DateTime.ParseExact(item, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("dd-MM-yyyy").Equals(Convert.ToDateTime(MasterSharedMemory.objMasterCurrencyDictBaseBSE[(long)ScripCodeSelected].ExpiryDate).ToString("dd-MM-yyyy")))
                                    ExpDateSelected = item;
                                //if (Convert.ToDateTime(item).Equals(Convert.ToDateTime(MasterSharedMemory.objMasterCurrencyDictBaseBSE[(long)ScripCodeSelected].ExpiryDate)))
                                //    ExpDateSelected = item;
                            }
                            if (StkPrcLst != null)
                                foreach (var item in StkPrcLst)
                                {
                                    if (Convert.ToDouble(item) * Math.Pow(10, DecimalPoint) == Convert.ToDouble(MasterSharedMemory.objMasterCurrencyDictBaseBSE[(long)ScripCodeSelected].StrikePrice))
                                        StkPriceSelected = item;
                                    //StkPrcLst.Add(Convert.ToString(Math.Truncate(Convert.ToDecimal(CommonFunctions.DisplayInDecimalFormatTouch(item, DecimalPoint)))));
                                }
                            ChangedFromScripCode = false;
                            ChangeCounter = 1;
                        }

                    }

                    else
                        ScripCodeSelected = 0;
                }

                else if (Selected_EXCH == Enumerations.Exchange.NSE.ToString())
                {
                    ScripCodeSelected = CommonFunctions.GetScripCodeFromScripID(ScripSymSelected, Selected_EXCH, ScripSelectedSegment);//NSE derivative
                    if (ScripCodeSelected != 0)
                        InstrNameSelected = MasterSharedMemory.objMasterDerivativeDictBaseNSE[(long)ScripCodeSelected].InstrumentName;
                    else
                        ScripCodeSelected = 0;
                }
            }
            else
            {
                InstrNameSelected = string.Empty;
                ScripCodeSelected = 0;
            }

            //ErrorMsg = string.Empty;

        }

        #endregion

    }
}
