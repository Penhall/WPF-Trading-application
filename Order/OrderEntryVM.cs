using CommonFrontEnd.Common;
using CommonFrontEnd.Global;
using CommonFrontEnd.Model;
using CommonFrontEnd.Model.Order;
using CommonFrontEnd.Processor.Order;
using CommonFrontEnd.SharedMemories;
using CommonFrontEnd.View.Order;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static CommonFrontEnd.Common.Enumerations;
using static CommonFrontEnd.SharedMemories.MasterSharedMemory;
using CommonFrontEnd.Model.Trade;

namespace CommonFrontEnd.ViewModel.Order
{
    public class OrderEntryVM : BaseViewModel
    {
        #region RelayCommands

        private RelayCommand _myLocationChanged;

        public RelayCommand myLocationChanged
        {
            get
            {
                return _myLocationChanged ?? (_myLocationChanged = new RelayCommand(
                    (object e) => SwiftOE_LocationChanged(e)));

            }
        }

        private void SwiftOE_LocationChanged(object e)
        {
            if (CommonFrontEnd.SharedMemories.ConfigurationMasterMemory.ConfigurationDict.ContainsKey("WindowsPosition"))
            {
                BoltAppSettingsWindowsPosition oBoltAppSettingsWindowsPosition = (BoltAppSettingsWindowsPosition)CommonFrontEnd.SharedMemories.ConfigurationMasterMemory.ConfigurationDict["WindowsPosition"];
                if (oBoltAppSettingsWindowsPosition != null && oBoltAppSettingsWindowsPosition.SOrderEntry != null && oBoltAppSettingsWindowsPosition.SOrderEntry.WNDPOSITION != null)
                {
                    oBoltAppSettingsWindowsPosition.SOrderEntry.WNDPOSITION.Left = Convert.ToInt32(LeftPosition);
                    oBoltAppSettingsWindowsPosition.SOrderEntry.WNDPOSITION.Top = Convert.ToInt32(TopPosition);
                    oBoltAppSettingsWindowsPosition.SOrderEntry.WNDPOSITION.Right = Convert.ToInt32(Width);
                    oBoltAppSettingsWindowsPosition.SOrderEntry.WNDPOSITION.Down = Convert.ToInt32(Height);
                }
                //CommonFrontEnd.SharedMemories.SaveConfiguration.SaveUserConfiguration(@"D:\TWS_DotNetNewStructure\TWS_DOTNETT\CommonFrontEnd\bin\Debug\xml\Users\AppSettings\920200000.xml", "WindowsPosition");
                CommonFrontEnd.SharedMemories.SaveConfiguration.SaveUserConfiguration(CommonFrontEnd.SharedMemories.SettingsManager.AppSettingsXmlPath, "WindowsPosition");
            }
        }

        private RelayCommand _SwiftOrderEntryClosing;

        public RelayCommand SwiftOrderEntryClosing
        {
            get { return _SwiftOrderEntryClosing ?? (_SwiftOrderEntryClosing = new RelayCommand((object e) => SwiftOrderEntryClosing_Closing(e))); }
        }

        private void SwiftOrderEntryClosing_Closing(object e)
        {
            SwiftOE_LocationChanged(e);
        }

        private RelayCommand<KeyEventArgs> _PeviewDown;

        public RelayCommand<KeyEventArgs> PeviewDown
        {
            get
            {
                return _PeviewDown ?? (_PeviewDown = new RelayCommand<KeyEventArgs>(OnPreviewKeyDown));
            }
            set { _PeviewDown = value; }
        }

        private RelayCommand<KeyEventArgs> _PreviewDown2;

        public RelayCommand<KeyEventArgs> PreviewDown2
        {
            get
            {
                return _PreviewDown2 ?? (_PreviewDown2 = new RelayCommand<KeyEventArgs>(OnPreviewKeyDown2));
            }
            set { _PreviewDown2 = value; }
        }



        private RelayCommand _BuyWindow;

        public RelayCommand BuyWindow
        {
            get { return _BuyWindow ?? (_BuyWindow = new RelayCommand((object e) => BuySellWindow(e))); }
        }

        private RelayCommand _HideTitle;

        public RelayCommand HideTitle
        {
            get { return _HideTitle ?? (_HideTitle = new RelayCommand((object e) => HideTitleBar())); }
        }

        private RelayCommand _ShowTitle;

        public RelayCommand ShowTitle
        {
            get { return _ShowTitle ?? (_ShowTitle = new RelayCommand((object e) => ShowTitleBar())); }
        }
        private RelayCommand _SubmitButton;
        public RelayCommand SubmitButton
        {
            get { return _SubmitButton ?? (_SubmitButton = new RelayCommand((object e) => SubmitButton_Click())); }
        }
        private RelayCommand _Modify_Click;
        public RelayCommand Modify_Click
        {
            get { return _Modify_Click ?? (_Modify_Click = new RelayCommand((object e) => Modify_Click_Button())); }
        }


        private RelayCommand _NormalOrderEntry;
        public RelayCommand NormalOrderEntry
        {
            get { return _NormalOrderEntry ?? (_NormalOrderEntry = new RelayCommand((object e) => NormalOrderEntry_Window(e))); }
        }



        private void Modify_Click_Button()
        {
            //throw new NotImplementedException();
        }

        #endregion

        #region properties

        long globalScripcode = 0;

        private string _LeftPosition = "345";

        public string LeftPosition
        {
            get { return _LeftPosition; }
            set { _LeftPosition = value; NotifyPropertyChanged("LeftPosition"); }
        }

        private string _TopPosition = "200";

        public string TopPosition
        {
            get { return _TopPosition; }
            set { _TopPosition = value; NotifyPropertyChanged("TopPosition"); }
        }

        private string _Width = "985.684";

        public string Width
        {
            get { return _Width; }
            set { _Width = value; NotifyPropertyChanged("Width"); }
        }


        private string _Height = "125.582";

        public string Height
        {
            get { return _Height; }
            set { _Height = value; NotifyPropertyChanged("Height"); }
        }


        OrderModel omodel = new OrderModel();

        string ScripID = String.Empty;
        string ScripName = String.Empty;
        internal string BuySellInd = String.Empty;
        private bool scripIdChange = true;
        string Validate_Message = string.Empty;
        private bool flag = true;
        int sum = 0;

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
        private uint _messageTag;

        public uint MessageTag
        {
            get { return _messageTag; }
            set
            {
                _messageTag = value;
                NotifyPropertyChanged("MessageTag");
            }
        }
        private List<long> _ScripCodeDrv;
        public List<long> ScripCodeDrv
        {
            get { return _ScripCodeDrv; }
            set { _ScripCodeDrv = value; NotifyPropertyChanged("ScripCodeDrv"); }
        }

        private List<long> _Token;
        public List<long> Token
        {
            get { return _Token; }
            set { _Token = value; NotifyPropertyChanged("Token"); }
        }
        private BindingList<string> _ScripSegmentLst;
        public BindingList<string> ScripSegmentLst
        {
            get { return _ScripSegmentLst; }
            set { _ScripSegmentLst = value; NotifyPropertyChanged("ScripSegmentLst"); }
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
        private ObservableCollection<string> _InstrumentNameColl;
        public ObservableCollection<string> InstrumentNameColl
        {
            get { return _InstrumentNameColl; }
            set { _InstrumentNameColl = value; NotifyPropertyChanged("InstrumentNameColl"); }

        }
        private string _InstrNameSelected;
        public string InstrNameSelected
        {
            get { return _InstrNameSelected; }
            set
            {
                _InstrNameSelected = value;
                NotifyPropertyChanged("InstrNameSelected");
                OnChangeOfInstrName();

            }
        }

        private void OnChangeOfInstrName()
        {
            if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Derivative.ToString())
            {
                if (!string.IsNullOrEmpty(InstrNameSelected))
                    ScripSelectedCode = MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Where(x => x.InstrumentName == InstrNameSelected).Select(x => x.ContractTokenNum).FirstOrDefault();
            }
            else if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Currency.ToString())
            {
                if (!string.IsNullOrEmpty(InstrNameSelected))
                    ScripSelectedCode = MasterSharedMemory.objMasterCurrencyDictBaseBSE.Values.Where(x => x.InstrumentName == InstrNameSelected).Select(x => x.ContractTokenNum).FirstOrDefault();
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
                OnChangeOfStkPrc();

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
        private string _qty;
        public string qty
        {
            get { return _qty; }
            set
            {
                _qty = value;
                NotifyPropertyChanged("qty");

            }
        }
        private bool _intracheck;
        public bool intracheck
        {
            get { return _intracheck; }
            set
            {
                _intracheck = value;
                NotifyPropertyChanged("intracheck");

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
        private string _DefaultFieldFocus;
        public string DefaultFieldFocus
        {
            get { return _DefaultFieldFocus; }
            set
            {
                _DefaultFieldFocus = value;
                NotifyPropertyChanged("DefaultFieldFocus");

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
        private string _Remarks;
        public string Remarks
        {
            get { return _Remarks; }
            set
            {
                _Remarks = value;
                NotifyPropertyChanged("Remarks");

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

        

        private string _ShortClientSelected;
        public string ShortClientSelected
        {
            get { return _ShortClientSelected; }
            set
            {
                _ShortClientSelected = value;
                NotifyPropertyChanged("ShortClientSelected");
                OnCheckClientTypeChange();
            }
        }
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
        private string _IntrTypeSelected;
        public string IntrTypeSelected
        {
            get { return _IntrTypeSelected; }
            set
            {
                _IntrTypeSelected = value;
                NotifyPropertyChanged("IntrTypeSelected");
                PopulatingUnderlyingAsset();
            }
        }
        private string _CallPutSelected;
        public string CallPutSelected
        {
            get { return _CallPutSelected; }
            set
            {
                _CallPutSelected = value;
                NotifyPropertyChanged("CallPutSelected");
                PopulatingExpDate();
            }
        }
        private string _UnderAssetSelected;
        public string UnderAssetSelected
        {
            get { return _UnderAssetSelected; }
            set
            {
                _UnderAssetSelected = value;
                NotifyPropertyChanged("UnderAssetSelected");
                OnChangeOfUnderAsset();
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
                PopulatingExpDate();
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
                EnableDisable();
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
        private string _ScripSelectedSegment;
        public string ScripSelectedSegment
        {
            get { return _ScripSelectedSegment; }
            set
            {
                _ScripSelectedSegment = value;
                NotifyPropertyChanged("ScripSelectedSegment");
                EnableDisable();

                // if (_ScripSymSelected == Enumerations.Order.ScripSegment.Derivative.ToString())
                //PopulatingInstType();
            }
        }
        private string _Selected_EXCH;
        public string Selected_EXCH
        {
            get { return _Selected_EXCH; }
            set
            {
                _Selected_EXCH = value;
                NotifyPropertyChanged("Selected_EXCH");
                // EnableDisable();
                PopulatingScripProfileSegment();
            }
        }
        private long _ScripSelectedCode;
        public long ScripSelectedCode
        {
            get { return _ScripSelectedCode; }
            set
            {
                _ScripSelectedCode = value;
                NotifyPropertyChanged("ScripSelectedCode");
                OnChangeOfScripCode();

            }
        }
        private string _WindowColour;
        public string WindowColour
        {
            get { return _WindowColour; }
            set
            {
                _WindowColour = value;
                NotifyPropertyChanged("WindowColour");
                EnableDisable();
            }
        }
        private string _EquityBoundVis;
        public string EquityBoundVis
        {
            get { return _EquityBoundVis; }
            set { _EquityBoundVis = value; NotifyPropertyChanged("EquityBoundVis"); }
        }


        private string _DerivativeBoundVis;
        public string DerivativeBoundVis
        {
            get { return _DerivativeBoundVis; }
            set { _DerivativeBoundVis = value; NotifyPropertyChanged("DerivativeBoundVis"); }
        }
        private string _CpVisibility;
        public string CpVisibility
        {
            get { return _CpVisibility; }
            set { _CpVisibility = value; NotifyPropertyChanged("CpVisibility"); }
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
        private bool _currencyAssetEnable;
        public bool CurrencyAssetEnable
        {
            get { return _currencyAssetEnable; }
            set { _currencyAssetEnable = value; NotifyPropertyChanged("CurrencyAssetEnable"); }
        }
        private bool _CallPutEnable;
        public bool CallPutEnable
        {
            get { return _CallPutEnable; }
            set { _CallPutEnable = value; NotifyPropertyChanged("CallPutEnable"); }
        }
        private bool _slbEnabled;
        public bool slbEnabled
        {
            get { return _slbEnabled; }
            set { _slbEnabled = value; NotifyPropertyChanged("slbEnabled"); }
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
        private string _isHideTitle;
        public string isHideTitle
        {
            get { return _isHideTitle; }
            set { _isHideTitle = value; NotifyPropertyChanged("isHideTitle"); }
        }
        private string _ModifyVisible;

        public string ModifyVisible
        {
            get { return _ModifyVisible; }
            set { _ModifyVisible = value; NotifyPropertyChanged(nameof(ModifyVisible)); }
        }

        #endregion

        #region Constructor
        public OrderEntryVM()
        {
            if (CommonFrontEnd.SharedMemories.ConfigurationMasterMemory.ConfigurationDict.ContainsKey("WindowsPosition"))
            {
                CommonFrontEnd.Model.BoltAppSettingsWindowsPosition oBoltAppSettingsWindowsPosition = new Model.BoltAppSettingsWindowsPosition();
                oBoltAppSettingsWindowsPosition = (CommonFrontEnd.Model.BoltAppSettingsWindowsPosition)CommonFrontEnd.SharedMemories.ConfigurationMasterMemory.ConfigurationDict["WindowsPosition"];
                if (oBoltAppSettingsWindowsPosition != null && oBoltAppSettingsWindowsPosition.SOrderEntry != null && oBoltAppSettingsWindowsPosition.SOrderEntry.WNDPOSITION != null)
                {
                    Height = oBoltAppSettingsWindowsPosition.SOrderEntry.WNDPOSITION.Down.ToString();
                    TopPosition = oBoltAppSettingsWindowsPosition.SOrderEntry.WNDPOSITION.Top.ToString();
                    LeftPosition = oBoltAppSettingsWindowsPosition.SOrderEntry.WNDPOSITION.Left.ToString();
                    Width = oBoltAppSettingsWindowsPosition.SOrderEntry.WNDPOSITION.Right.ToString();
                }
            }

            ScripSegmentLst = new BindingList<string>();
            Exchange = new List<string>();
            OrderTypeList = new List<string>();
            DefaultOrderProfileSettings();
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
            CurrencyAssetEnable = false;
            clientinputEnabled = true;
            TrgEnabled = false;
            CpVisibility = "Hidden";
            ModifyVisible = "Hidden";
            //SellVisible = "Hidden";
            //BuyVisible = "Visible";
            //WindowColour = "#023199";
            //BuySellInd = Enumerations.Order.BuySellFlag.BUY.ToString();
            //HeaderTitle = string.Format("OrderEntry BUY   GrossBuyLimit - [{0}]   GrossSellLimit - [{1}]", UtilityLimitDetails.GrossBuyLimit, UtilityLimitDetails.GrossSellLimit);
            //BuySellWindow(e);
            isHideTitle = "SingleBorderWindow";
            //    HeaderTitle = "SwiftOrderEntry BUY";
            intracheck = false;
            slbEnabled = false;
            DecimalPoint = 2;

            //if (Selected_EXCH == Enumerations.Order.Exchanges.BSE.ToString())
            //{
            //    if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Equity.ToString())
            //    {

            //        if (MasterSharedMemory.objMastertxtDictBaseBSE != null && MasterSharedMemory.objMastertxtDictBaseBSE.Count > 0)
            //            DecimalPoint = MasterSharedMemory.objMastertxtDictBaseBSE.Where(x => x.Key == ScripSelectedCode).Select(x => x.Value.Precision).FirstOrDefault();
            //    }
            //    if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Derivative.ToString())
            //    {

            //        if (MasterSharedMemory.objMastertxtDictBaseBSE != null && MasterSharedMemory.objMastertxtDictBaseBSE.Count > 0)
            //            DecimalPoint = MasterSharedMemory.objMastertxtDictBaseBSE.Where(x => x.Key == ScripSelectedCode).Select(x => x.Value.Precision).FirstOrDefault();
            //    }
            //}
            //if (Selected_EXCH == Enumerations.Order.Exchanges.NSE.ToString())
            //{
            //    if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Equity.ToString())
            //    {
            //        if (MasterSharedMemory.objMastertxtDictBaseNSE != null && MasterSharedMemory.objMastertxtDictBaseNSE.Count > 0)
            //            DecimalPoint = MasterSharedMemory.objMastertxtDictBaseNSE.Where(x => x.Key == ScripSelectedCode).Select(x => x.Value.Precision).FirstOrDefault();
            //    }
            //}

            //TODO code for client id client name from client profiling 
            // qty = "QTY";
            // rate = "RATE";
            // revQty = "REV QTY";
            //trgPrice = "TRG Price";
            // MktPT = "MP%";
            //Remarks = "REMARKS";
            //  CLientIdinput = "CLIENT ID";******
            //ShortClientSelected = "Short Client";
            //#0022ff
            //ScripNameSelected = "ScripName";
            //if (MasterSharedMemory.objMastertxtDictBase != null && MasterSharedMemory.objMastertxtDictBase.Count > 0)
            //  DecimalPoint = MasterSharedMemory.objMastertxtDictBase.Where(x => x.Key == ScripSelectedCode).Select(x => x.Value.Precision).FirstOrDefault();





            //if (ScripSelectedSegment =='Equity')
            //{
            //    objScipMaster.InstrumentType = 0;
            //    objScipMaster.DecimalPoint = 2;
            //}
            //else if (objScipMaster.SegmentFlag == 'D')
            //{
            //    objScipMaster.InstrumentType = 1;
            //    objScipMaster.DecimalPoint = 2;
            //}
            //else if (objScipMaster.SegmentFlag == 'C')
            //{
            //    objScipMaster.InstrumentType = 2;
            //    objScipMaster.DecimalPoint = 4;
            //}
        }
        #endregion

        #region Methods

        private void NormalOrderEntry_Window(object e)
        {
            if (UtilityLoginDetails.GETInstance.IsLoggedIN)
            {
                if (!(UtilityLoginDetails.GETInstance.Role == Role.Admin.ToString()))
                {
                    e = "BUY";
                    SwiftOrderEntry objswift = Application.Current.Windows.OfType<SwiftOrderEntry>().FirstOrDefault();
                    NormalOrderEntry objnormal = Application.Current.Windows.OfType<NormalOrderEntry>().FirstOrDefault();

                    if (objnormal != null)
                    {
                        if (objnormal.WindowState == WindowState.Minimized)
                            objnormal.WindowState = WindowState.Normal;

                        ((NormalOrderEntryVM)objnormal.DataContext).BuySellWindow(e);

                        objnormal.Focus();
                        objnormal.Show();
                        UtilityOrderDetails.GETInstance.CurrentOrderEntry = Enumerations.OrderEntryWindow.Normal.ToString();
                        objswift.Close(); // closing the existing window

                    }
                    else
                    {
                        objnormal = new NormalOrderEntry();
                        objnormal.Owner = System.Windows.Application.Current.MainWindow;
                        //objswift.CmbExcangeType.Focus();

                        ((NormalOrderEntryVM)objnormal.DataContext).BuySellWindow(e);

                        objnormal.Activate();
                        objnormal.Show();
                        UtilityOrderDetails.GETInstance.CurrentOrderEntry = Enumerations.OrderEntryWindow.Normal.ToString();
                        objswift.Close(); // closing the existing window
                    }


                }
            }
        }

        private void PopulatingScripName()
        {
            try
            {

                //for BSE Exchange
                if (Selected_EXCH == Enumerations.Order.Exchanges.BSE.ToString())
                {
                    //for equity market
                    if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Equity.ToString())
                    {
                        ScripNameLst = new List<String>();
                        //if (MasterSharedMemory.objMastertxtDictBase != null)
                        //  ScripNameLst = MasterSharedMemory.objMastertxtDictBase.Values.Cast<ScripMasterBase>().Select(x => x.ScripName).ToList();
                        if (MasterSharedMemory.objMastertxtDictBaseBSE != null)//BSE Equity
                            ScripNameLst = MasterSharedMemory.objMastertxtDictBaseBSE.Values.Cast<ScripMasterBase>().Select(x => x.ScripName).ToList();
                        if (ScripNameLst.Count() > 0)
                            ScripNameSelected = ScripNameLst[0];
                    }
                }

                //for NSE Exchhnge
                if (Selected_EXCH == Enumerations.Order.Exchanges.NSE.ToString())
                {
                    //for equity market
                    if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Equity.ToString())
                    {
                        ScripNameLst = new List<String>();

                        if (MasterSharedMemory.objMastertxtDictBaseNSE != null)//NSE Equity
                            ScripNameLst = MasterSharedMemory.objMastertxtDictBaseNSE.Values.Cast<ScripMasterBase>().Select(x => x.ScripName).ToList();
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
                        //if (MasterSharedMemory.objMastertxtDictBase != null)
                        //    ScripCodeLst = MasterSharedMemory.objMastertxtDictBase.Values.Cast<ScripMasterBase>().Select(x => x.ScripCode).ToList();
                        if (MasterSharedMemory.objMastertxtDictBaseBSE != null)
                            ScripCodeLst = MasterSharedMemory.objMastertxtDictBaseBSE.Values.Cast<ScripMasterBase>().Select(x => x.ScripCode).ToList();

                        if (ScripCodeLst.Count() > 0)
                            ScripSelectedCode = ScripCodeLst[0];
                    }
                    //for institution type for derivative

                    else
                    {
                        if (ScripCodeLst != null)
                        {
                            ScripCodeLst.Clear();
                            //  ScripSelectedCode = 0;
                        }
                    }
                }

                //for NSE Exchange
                if (Selected_EXCH == Enumerations.Order.Exchanges.NSE.ToString())
                {
                    //for equity market
                    if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Equity.ToString())
                    {
                        ScripCodeLst = new List<long>();

                        if (MasterSharedMemory.objMastertxtDictBaseNSE != null)
                            ScripCodeLst = MasterSharedMemory.objMastertxtDictBaseNSE.Values.Cast<ScripMasterBase>().Select(x => x.ScripCode).ToList();

                        if (ScripCodeLst.Count() > 0)
                            ScripSelectedCode = ScripCodeLst[0];
                    }
                    else
                    {
                        if (ScripCodeLst != null)
                        {
                            ScripCodeLst.Clear();
                            //  ScripSelectedCode = 0;
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
                            ScripSymLst = MasterSharedMemory.objMastertxtDictBaseBSE.Values.Cast<ScripMasterBase>().Select(x => x.ScripId).ToList();
                        if (ScripSymLst.Count() > 0)
                            ScripSymSelected = ScripSymLst[0];
                    }
                }
                //for NSE Exchange
                if (Selected_EXCH == Enumerations.Order.Exchanges.NSE.ToString())
                {
                    //for equity market
                    if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Equity.ToString())
                    {
                        // if (MasterSharedMemory.objMastertxtDictBase != null)
                        if (MasterSharedMemory.objMastertxtDictBaseNSE != null)
                            // ScripSymLst = MasterSharedMemory.objMastertxtDictBase.Values.Cast<ScripMasterBase>().Where(x => (x.InstrumentType == 'E')).GroupBy(x => x.ScripId).Select(x => x.FirstOrDefault().ScripId).ToList();
                            //   ScripSymLst = MasterSharedMemory.objMastertxtDictBase.Values.Cast<ScripMasterBase>().Select(x => x.ScripId).ToList();
                            ScripSymLst = MasterSharedMemory.objMastertxtDictBaseNSE.Values.Cast<ScripMasterBase>().Select(x => x.ScripId).ToList();
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

        private void PopulatingScripProfileSegment()
        {
            try
            {

                //ScripSelectedSegment = Enumerations.Order.ScripSegment.Equity.ToString();
                //ScripSegmentLst.Add(Enumerations.Order.ScripSegment.Equity.ToString());
                //ScripSegmentLst.Add(Enumerations.Order.ScripSegment.Derivative.ToString());
                //ScripSegmentLst.Add(Enumerations.Order.ScripSegment.Debt.ToString());
                //ScripSegmentLst.Add(Enumerations.Order.ScripSegment.Currency.ToString());
                //ScripSegmentLst.Add(Enumerations.Order.ScripSegment.Commodities.ToString());
                //ScripSegmentLst.Add(Enumerations.Order.ScripSegment.Itp.ToString());

                if (Selected_EXCH == Enumerations.Order.Exchanges.BSE.ToString())
                {
                    ScripSegmentLst = new BindingList<string>();

                    ScripSelectedSegment = Enumerations.Order.ScripSegment.Equity.ToString();
                    ScripSegmentLst.Add(Enumerations.Order.ScripSegment.Equity.ToString());
                    ScripSegmentLst.Add(Enumerations.Order.ScripSegment.Derivative.ToString());
                    ScripSegmentLst.Add(Enumerations.Order.ScripSegment.Debt.ToString());
                    ScripSegmentLst.Add(Enumerations.Order.ScripSegment.Currency.ToString());
                    //ScripSegmentLst.Add(Enumerations.Order.ScripSegment.Commodities.ToString());
                    //ScripSegmentLst.Add(Enumerations.Order.ScripSegment.ITP.ToString());
                    //ScripSegmentLst.Add(Enumerations.Order.ScripSegment.SLB.ToString());
                }

                else if (Selected_EXCH == Enumerations.Order.Exchanges.NSE.ToString())
                {
                    ScripSegmentLst = new BindingList<string>();

                    ScripSelectedSegment = Enumerations.Order.ScripSegment.Equity.ToString();
                    ScripSegmentLst.Add(Enumerations.Order.ScripSegment.Equity.ToString());
                    ScripSegmentLst.Add(Enumerations.Order.ScripSegment.Derivative.ToString());
                    ScripSegmentLst.Add(Enumerations.Order.ScripSegment.Currency.ToString());
                }
                else if (Selected_EXCH == Enumerations.Order.Exchanges.NCDEX.ToString())
                {
                    ScripSegmentLst = new BindingList<string>();
                    ScripSelectedSegment = Enumerations.Order.ScripSegment.Commodities.ToString();
                    ScripSegmentLst.Add(Enumerations.Order.ScripSegment.Commodities.ToString());

                }
                else if (Selected_EXCH == Enumerations.Order.Exchanges.MCX.ToString())
                {
                    ScripSegmentLst = new BindingList<string>();
                    ScripSelectedSegment = Enumerations.Order.ScripSegment.Commodities.ToString();
                    ScripSegmentLst.Add(Enumerations.Order.ScripSegment.Currency.ToString());
                    ScripSegmentLst.Add(Enumerations.Order.ScripSegment.Commodities.ToString());

                }

            }
            catch (Exception ex)
            {

                ExceptionUtility.LogError(ex);
            }

        }


        #region Derivative Methods
        private void PopulatingInstType()
        {
            try
            {
                CallPutLSt = new List<string>();

                #region BSE
                //for BSE Exchange
                if (Selected_EXCH == Enumerations.Order.Exchanges.BSE.ToString())
                {
                    //for derivative market DerivativeMasterBase> 
                    if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Derivative.ToString())
                    {
                        DecimalPoint = 2;
                        IntrTypeLst = new List<String>();

                        if (MasterSharedMemory.objMasterDerivativeDictBaseBSE != null)//BSE Derivative
                            IntrTypeLst = MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Cast<DerivativeMasterBase>().Select(x => x.InstrumentType).Distinct().ToList();
                        if (IntrTypeLst.Count() > 0)
                            IntrTypeSelected = IntrTypeLst[0];
                        if (MasterSharedMemory.objMasterDerivativeDictBaseBSE != null)//BSE Derivative
                            CallPutLSt = MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Cast<DerivativeMasterBase>().Select(x => x.OptionType).Distinct().ToList();
                        if (CallPutLSt.Count() > 0)
                            CallPutSelected = CallPutLSt[0];
                        //CallPutSelected = Enumerations.Order.CallPut.Call.ToString();
                        //CallPutLSt.Add(Enumerations.Order.CallPut.Call.ToString());
                        //CallPutLSt.Add(Enumerations.Order.CallPut.Put.ToString());

                    }
                    //Debt WholesaleDebtMarket objMasterWholesaleDebtMarket
                    if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Debt.ToString())
                    {
                        IntrTypeLst = new List<String>();
                        CallPutEnable = false;
                        if (MasterSharedMemory.objMasterWholesaleDebtMarket != null)//BSE debt
                            IntrTypeLst = MasterSharedMemory.objMasterWholesaleDebtMarket.Values.Cast<WholesaleDebtMarket>().Select(x => x.WDMInstrumentName).Distinct().ToList();
                        if (IntrTypeLst.Count() > 0)
                            IntrTypeSelected = IntrTypeLst[0];
                        if (MasterSharedMemory.objMasterWholesaleDebtMarket != null)//BSE debt
                            CallPutLSt = MasterSharedMemory.objMasterWholesaleDebtMarket.Values.Cast<WholesaleDebtMarket>().Select(x => x.WDMOptionType).Distinct().ToList();
                        if (CallPutLSt.Count() > 0)
                            CallPutSelected = CallPutLSt[0];
                    }
                    //for BSE Currency
                    if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Currency.ToString())
                    {
                        DecimalPoint = 4;
                        IntrTypeLst = new List<String>();

                        if (MasterSharedMemory.objMasterCurrencyDictBaseBSE != null)//BSE Currency
                        {
                            IntrTypeLst = MasterSharedMemory.objMasterCurrencyDictBaseBSE.Values.Cast<CurrencyMasterBase>().Select(x => x.InstrumentType).Distinct().ToList();
                            IntrTypeLst.Remove(null);
                        }
                        if (IntrTypeLst.Count() > 0)
                            IntrTypeSelected = IntrTypeLst[0];
                        if (MasterSharedMemory.objMasterCurrencyDictBaseBSE != null)//BSE Currency
                            CallPutLSt = MasterSharedMemory.objMasterCurrencyDictBaseBSE.Values.Cast<CurrencyMasterBase>().Select(x => x.OptionType).Distinct().ToList();
                        if (CallPutLSt.Count() > 0)
                            CallPutSelected = CallPutLSt[0];
                    }
                    //itp
                    if (ScripSelectedSegment == Enumerations.Order.ScripSegment.ITP.ToString())
                    {
                        IntrTypeLst = new List<String>();
                        CallPutLSt.Clear();
                        CallPutEnable = false;
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
                        CallPutEnable = false;
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
                            CallPutSelected = CallPutLSt[0];
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
                            CallPutSelected = CallPutLSt[0];
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
                            CallPutSelected = CallPutLSt[0];


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
                            CallPutSelected = CallPutLSt[0];


                    }
                }
                #endregion

            }
            catch (Exception ex)
            {

                ExceptionUtility.LogError(ex);
            }


        }
        private void PopulatingUnderlyingAsset()
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
                        if (IntrTypeSelected == "SO" || IntrTypeSelected == "IO")
                        {
                            CallPutEnable = true;
                            UnderLyingAssetLst = new ObservableCollection<string>();
                            ExpDateLst = new ObservableCollection<string>();
                            foreach (var item in MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Cast<DerivativeMasterBase>().Where(x => x.InstrumentType == IntrTypeSelected).Select(x => x.UnderlyingAsset).Distinct())
                            {
                                UnderLyingAssetLst.Add(item);
                            }
                            if (UnderLyingAssetLst.Count() > 0 && UnderLyingAssetLst != null)
                            {
                                UnderAssetSelected = UnderLyingAssetLst[0];


                                //foreach (var item in MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Cast<DerivativeMasterBase>().OrderByDescending(y => y.ExpiryDate).Where(x => x.UnderlyingAsset == UnderAssetSelected && x.OptionType==CallPutSelected).Select(x => x.ExpiryDate).Distinct())
                                //{
                                //    ExpDateLst.Add(item);
                                //}
                                //if (ExpDateLst.Count > 0 && ExpDateLst != null)
                                //{
                                //    ExpDateSelected = ExpDateLst[0];
                                //}


                            }

                        }
                        if (IntrTypeSelected == "SF" || IntrTypeSelected == "IF")
                        {
                            CallPutEnable = false;
                            UnderLyingAssetLst = new ObservableCollection<string>();
                            ExpDateLst = new ObservableCollection<string>();
                            InstrumentNameColl = new ObservableCollection<string>();
                            foreach (var item in MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Cast<DerivativeMasterBase>().Where(x => x.InstrumentType == IntrTypeSelected).Select(x => x.UnderlyingAsset).Distinct())
                            {
                                UnderLyingAssetLst.Add(item);
                            }
                            if (UnderLyingAssetLst.Count() > 0 && UnderLyingAssetLst != null)
                            {
                                UnderAssetSelected = UnderLyingAssetLst[0];

                                //code redundancy minimization, get a review
                                //foreach (var item in MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Cast<DerivativeMasterBase>().OrderByDescending(y => y.ExpiryDate).Where(x => x.UnderlyingAsset == UnderAssetSelected).Select(x => x.ExpiryDate).Distinct())
                                //{
                                //    ExpDateLst.Add(item);
                                //}
                                //if (ExpDateLst.Count > 0 && ExpDateLst != null)
                                //{
                                //    ExpDateSelected = ExpDateLst[0];
                                //}

                                //code redundancy minimization, get a review(separate methods created
                                //foreach (var item in MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Cast<DerivativeMasterBase>().Where(x=>x.UnderlyingAsset==UnderAssetSelected && x.ExpiryDate==ExpDateSelected).Select(x => x.InstrumentName))
                                //{
                                //    InstrumentNameColl.Add(item);
                                //}
                                //if (InstrumentNameColl.Count > 0 && InstrumentNameColl != null)
                                //{
                                //    InstrNameSelected = InstrumentNameColl[0];
                                //}

                            }
                        }
                    }
                    //BSE CURRENCY objMasterCurrencyDictBaseBSE CurrencyMasterBase

                    if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Currency.ToString())
                    {
                        UnderLyingAssetLst = new ObservableCollection<string>();
                        if (IntrTypeSelected == "OPTCUR")
                        {
                            CallPutEnable = true;
                            UnderLyingAssetLst = new ObservableCollection<string>();
                            ExpDateLst = new ObservableCollection<string>();
                            foreach (var item in MasterSharedMemory.objMasterCurrencyDictBaseBSE.Values.Cast<CurrencyMasterBase>().Where(x => x.InstrumentType == IntrTypeSelected).Select(x => x.UnderlyingAsset).Distinct())
                            {
                                UnderLyingAssetLst.Add(item);
                            }
                            if (UnderLyingAssetLst.Count() > 0 && UnderLyingAssetLst != null)
                            {
                                UnderAssetSelected = UnderLyingAssetLst[0];


                            }



                        }

                        if (IntrTypeSelected == "FUTIRD" || IntrTypeSelected == "FUTCUR" || IntrTypeSelected == "FUTIRT")
                        {
                            CallPutEnable = false;
                            UnderLyingAssetLst = new ObservableCollection<string>();
                            ExpDateLst = new ObservableCollection<string>();
                            InstrumentNameColl = new ObservableCollection<string>();
                            foreach (var item in MasterSharedMemory.objMasterCurrencyDictBaseBSE.Values.Cast<CurrencyMasterBase>().Where(x => x.InstrumentType == IntrTypeSelected).Select(x => x.UnderlyingAsset).Distinct())
                            {
                                UnderLyingAssetLst.Add(item);
                            }
                            if (UnderLyingAssetLst.Count() > 0 && UnderLyingAssetLst != null)
                            {
                                UnderAssetSelected = UnderLyingAssetLst[0];


                            }
                        }
                    }
                    //Debt WholesaleDebtMarket objMasterWholesaleDebtMarket
                    if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Debt.ToString())
                    {
                        UnderLyingAssetLst = new ObservableCollection<string>();
                        ExpDateLst = new ObservableCollection<string>();
                        foreach (var item in MasterSharedMemory.objMasterWholesaleDebtMarket.Values.Cast<WholesaleDebtMarket>().Where(x => x.WDMInstrumentName == IntrTypeSelected).Select(x => x.WDMSymbol).Distinct())
                        {
                            UnderLyingAssetLst.Add(item);
                        }
                        if (UnderLyingAssetLst.Count() > 0 && UnderLyingAssetLst != null)
                        {
                            UnderAssetSelected = UnderLyingAssetLst[0];
                        }
                    }

                    //BSE itp ITPMaster objITPMasterDict

                    if (ScripSelectedSegment == Enumerations.Order.ScripSegment.ITP.ToString())
                    {
                        UnderLyingAssetLst = new ObservableCollection<string>();
                        ExpDateLst = new ObservableCollection<string>();
                        foreach (var item in MasterSharedMemory.objITPMasterDict.Values.Cast<ITPMaster>().Where(x => x.ITPInstrumenName == IntrTypeSelected).Select(x => x.ITPScripID).Distinct())
                        {
                            UnderLyingAssetLst.Add(item);
                        }
                        if (UnderLyingAssetLst.Count() > 0 && UnderLyingAssetLst != null)
                        {
                            UnderAssetSelected = UnderLyingAssetLst[0];
                        }
                    }

                    // SLB SLBMaster objSLBMasterDict
                    if (ScripSelectedSegment == Enumerations.Order.ScripSegment.SLB.ToString())
                    {
                        UnderLyingAssetLst = new ObservableCollection<string>();
                        ExpDateLst = new ObservableCollection<string>();
                        foreach (var item in MasterSharedMemory.objSLBMasterDict.Values.Cast<SLBMaster>().Where(x => x.SCInstrumentName == IntrTypeSelected).Select(x => x.ScripID_ShortName).Distinct())
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

                #region NSE
                //NSE Derivative
                //for NSE Exchange
                if (Selected_EXCH == Enumerations.Order.Exchanges.NSE.ToString())
                {
                    //for derivative market DerivativeMasterBase> 
                    if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Derivative.ToString())
                    {
                        if (IntrTypeSelected == "OPTIDX" || IntrTypeSelected == "OPTSTK")
                        {
                            CallPutEnable = true;
                            UnderLyingAssetLst = new ObservableCollection<string>();
                            ExpDateLst = new ObservableCollection<string>();
                            foreach (var item in MasterSharedMemory.objMasterDerivativeDictBaseNSE.Values.Cast<DerivativeMasterBase>().Where(x => x.InstrumentType == IntrTypeSelected).Select(x => x.UnderlyingAsset).Distinct())
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
                            CallPutEnable = false;
                            UnderLyingAssetLst = new ObservableCollection<string>();
                            ExpDateLst = new ObservableCollection<string>();
                            InstrumentNameColl = new ObservableCollection<string>();
                            foreach (var item in MasterSharedMemory.objMasterDerivativeDictBaseNSE.Values.Cast<DerivativeMasterBase>().Where(x => x.InstrumentType == IntrTypeSelected).Select(x => x.UnderlyingAsset).Distinct())
                            {
                                UnderLyingAssetLst.Add(item);
                            }
                            if (UnderLyingAssetLst.Count() > 0 && UnderLyingAssetLst != null)
                            {
                                UnderAssetSelected = UnderLyingAssetLst[0];


                            }
                        }
                    }
                    //NSE CURRENCY objMasterCurrencyDictBaseNSE CurrencyMasterBase

                    if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Currency.ToString())
                    {
                        if (IntrTypeSelected == "OPTCUR")
                        {
                            CallPutEnable = true;
                            UnderLyingAssetLst = new ObservableCollection<string>();
                            ExpDateLst = new ObservableCollection<string>();
                            foreach (var item in MasterSharedMemory.objMasterCurrencyDictBaseNSE.Values.Cast<CurrencyMasterBase>().Where(x => x.InstrumentType == IntrTypeSelected).Select(x => x.UnderlyingAsset).Distinct())
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
                            CallPutEnable = false;
                            UnderLyingAssetLst = new ObservableCollection<string>();
                            ExpDateLst = new ObservableCollection<string>();
                            InstrumentNameColl = new ObservableCollection<string>();
                            foreach (var item in MasterSharedMemory.objMasterCurrencyDictBaseNSE.Values.Cast<CurrencyMasterBase>().Where(x => x.InstrumentType == IntrTypeSelected).Select(x => x.UnderlyingAsset).Distinct())
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

                #region NCDEX
                //NCDEX
                if (Selected_EXCH == Enumerations.Order.Exchanges.NCDEX.ToString())
                {
                    //Commodities NCDEXMaster objNCDEXMasterDict
                    if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Commodities.ToString())
                    {

                        if (IntrTypeSelected == "COMDTY" || IntrTypeSelected == "FUTCOM" || IntrTypeSelected == "OPTFUT")
                            CallPutEnable = false;
                        UnderLyingAssetLst = new ObservableCollection<string>();
                        ExpDateLst = new ObservableCollection<string>();
                        InstrumentNameColl = new ObservableCollection<string>();
                        foreach (var item in MasterSharedMemory.objNCDEXMasterDict.Values.Cast<NCDEXMaster>().Where(x => x.NCDInstrumentName == IntrTypeSelected).Select(x => x.NCDSymbol).Distinct())
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

                #region MCX
                if (Selected_EXCH == Enumerations.Order.Exchanges.MCX.ToString())
                {
                    //Commodities MCXMaster> objMCXMasterDict
                    if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Commodities.ToString())
                    {
                        if (IntrTypeSelected == "OPTFUT")
                        {
                            CallPutEnable = true;
                            UnderLyingAssetLst = new ObservableCollection<string>();
                            ExpDateLst = new ObservableCollection<string>();
                            foreach (var item in MasterSharedMemory.objMCXMasterDict.Values.Cast<MCXMaster>().Where(x => x.MCInstrumentName == IntrTypeSelected).Select(x => x.MCContractCode).Distinct())
                            {
                                UnderLyingAssetLst.Add(item);
                            }
                            if (UnderLyingAssetLst.Count() > 0 && UnderLyingAssetLst != null)
                            {
                                UnderAssetSelected = UnderLyingAssetLst[0];


                            }
                        }
                        if (IntrTypeSelected == "AUCSO" || IntrTypeSelected == "FUTCOM")
                        {
                            CallPutEnable = false;
                            UnderLyingAssetLst = new ObservableCollection<string>();
                            ExpDateLst = new ObservableCollection<string>();
                            InstrumentNameColl = new ObservableCollection<string>();
                            foreach (var item in MasterSharedMemory.objMCXMasterDict.Values.Cast<MCXMaster>().Where(x => x.MCInstrumentName == IntrTypeSelected).Select(x => x.MCContractCode).Distinct())
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
        private void OnChangeOfUnderAsset()
        {
            try
            {

                if (Selected_EXCH == Enumerations.Order.Exchanges.BSE.ToString())
                { //BSE DERIVATIVE
                    if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Derivative.ToString())
                    {
                        if (UnderLyingAssetLst.Count() > 0 && UnderLyingAssetLst != null)
                        {
                            ExpDateLst = new ObservableCollection<string>();
                            if (CallPutEnable == false)
                            {
                                foreach (var item in MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Cast<DerivativeMasterBase>().OrderByDescending(y => y.ExpiryDate).Where(x => x.UnderlyingAsset == UnderAssetSelected && x.InstrumentType == IntrTypeSelected).Select(x => x.ExpiryDate).Distinct())
                                {
                                    ExpDateLst.Add(item);
                                }
                            }
                            else if (CallPutEnable == true)
                            {
                                foreach (var item in MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Cast<DerivativeMasterBase>().OrderByDescending(y => y.ExpiryDate).Where(x => x.UnderlyingAsset == UnderAssetSelected && x.InstrumentType == IntrTypeSelected && x.OptionType == CallPutSelected).Select(x => x.ExpiryDate).Distinct())
                                {
                                    ExpDateLst.Add(item);
                                }
                            }
                            if (ExpDateLst.Count > 0 && ExpDateLst != null)
                            {
                                ExpDateSelected = ExpDateLst[0];
                            }
                        }
                    }
                    //BSE CURRENCY 
                    if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Currency.ToString())
                    {

                        if (UnderLyingAssetLst.Count() > 0 && UnderLyingAssetLst != null)
                        {
                            ExpDateLst = new ObservableCollection<string>();
                            if (CallPutEnable == false)
                            {
                                foreach (var item in MasterSharedMemory.objMasterCurrencyDictBaseBSE.Values.Cast<CurrencyMasterBase>().OrderByDescending(y => y.ExpiryDate).Where(x => x.UnderlyingAsset == UnderAssetSelected && x.InstrumentType == IntrTypeSelected && x.StrategyID == -1 && x.ComplexInstrumentType == 1).Select(x => x.ExpiryDate).Distinct())
                                {
                                    ExpDateLst.Add(item);
                                }
                            }
                            else if (CallPutEnable == true)
                            {
                                foreach (var item in MasterSharedMemory.objMasterCurrencyDictBaseBSE.Values.Cast<CurrencyMasterBase>().OrderByDescending(y => y.ExpiryDate).Where(x => x.UnderlyingAsset == UnderAssetSelected && x.InstrumentType == IntrTypeSelected && x.OptionType == CallPutSelected).Select(x => x.ExpiryDate).Distinct())
                                {
                                    ExpDateLst.Add(item);
                                }

                            }
                            if (ExpDateLst.Count > 0 && ExpDateLst != null)
                            {
                                ExpDateSelected = ExpDateLst[0];
                            }
                            else if (ExpDateSelected == null)
                            {
                                InstrNameSelected = null;
                                StkPriceSelected = string.Empty;
                            }
                        }

                    }
                    //Debt WholesaleDebtMarket objMasterWholesaleDebtMarket
                    if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Debt.ToString())
                    {
                        if (UnderLyingAssetLst.Count() > 0 && UnderLyingAssetLst != null)
                        {
                            ExpDateLst = new ObservableCollection<string>();

                            foreach (var item in MasterSharedMemory.objMasterWholesaleDebtMarket.Values.Cast<WholesaleDebtMarket>().OrderByDescending(y => y.WDMExpiryDate).Where(x => x.WDMSymbol == UnderAssetSelected && x.WDMInstrumentName == IntrTypeSelected).Select(x => x.WDMDisplayExpiryDate).Distinct())
                            {
                                ExpDateLst.Add(item);
                            }

                            if (ExpDateLst.Count > 0 && ExpDateLst != null)
                            {
                                ExpDateSelected = ExpDateLst[0];
                            }
                            else if (ExpDateSelected == null)
                            {
                                InstrNameSelected = null;
                            }
                        }
                    }

                    //BSE itp ITPMaster objITPMasterDict
                    if (ScripSelectedSegment == Enumerations.Order.ScripSegment.ITP.ToString())
                    {
                        if (UnderLyingAssetLst.Count() > 0 && UnderLyingAssetLst != null)
                        {
                            ExpDateLst = new ObservableCollection<string>();

                            foreach (var item in MasterSharedMemory.objITPMasterDict.Values.Cast<ITPMaster>().OrderByDescending(y => y.ITPDisplayExpiryDate).Where(x => x.ITPScripID == UnderAssetSelected && x.ITPInstrumenName == IntrTypeSelected).Select(x => x.ITPDisplayExpiryDate).Distinct())
                            {
                                ExpDateLst.Add(item);
                            }

                            if (ExpDateLst.Count > 0 && ExpDateLst != null)
                            {
                                ExpDateSelected = ExpDateLst[0];
                            }
                            else if (ExpDateSelected == null)
                            {
                                InstrNameSelected = null;
                            }
                        }
                    }
                    // SLB SLBMaster objSLBMasterDict
                    if (ScripSelectedSegment == Enumerations.Order.ScripSegment.SLB.ToString())
                    {
                        if (UnderLyingAssetLst.Count() > 0 && UnderLyingAssetLst != null)
                        {
                            ExpDateLst = new ObservableCollection<string>();

                            foreach (var item in MasterSharedMemory.objSLBMasterDict.Values.Cast<SLBMaster>().OrderByDescending(y => y.SCDisplayExpiryDate).Where(x => x.ScripID_ShortName == UnderAssetSelected && x.SCInstrumentName == IntrTypeSelected).Select(x => x.SCDisplayExpiryDate).Distinct())
                            {
                                ExpDateLst.Add(item);
                            }

                            if (ExpDateLst.Count > 0 && ExpDateLst != null)
                            {
                                ExpDateSelected = ExpDateLst[0];
                            }
                            else if (ExpDateSelected == null)
                            {
                                InstrNameSelected = null;
                            }
                        }
                    }
                }
                // NSE DERIVATIVE

                if (Selected_EXCH == Enumerations.Order.Exchanges.NSE.ToString())
                {
                    if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Derivative.ToString())
                    {
                        if (UnderLyingAssetLst.Count() > 0 && UnderLyingAssetLst != null)
                        {
                            ExpDateLst = new ObservableCollection<string>();
                            if (CallPutEnable == false)
                            {
                                foreach (var item in MasterSharedMemory.objMasterDerivativeDictBaseNSE.Values.Cast<DerivativeMasterBase>().OrderByDescending(y => y.ExpiryDate).Where(x => x.UnderlyingAsset == UnderAssetSelected && x.InstrumentType == IntrTypeSelected).Select(x => x.ExpiryDate).Distinct())
                                {
                                    ExpDateLst.Add(item);
                                }

                            }
                            else if (CallPutEnable == true)
                            {
                                foreach (var item in MasterSharedMemory.objMasterDerivativeDictBaseNSE.Values.Cast<DerivativeMasterBase>().OrderByDescending(y => y.ExpiryDate).Where(x => x.UnderlyingAsset == UnderAssetSelected && x.InstrumentType == IntrTypeSelected && x.OptionType == CallPutSelected).Select(x => x.ExpiryDate).Distinct())
                                {
                                    ExpDateLst.Add(item);
                                }
                            }
                            if (ExpDateLst.Count > 0 && ExpDateLst != null)
                            {
                                ExpDateSelected = ExpDateLst[0];
                            }
                            else if (ExpDateSelected == null)
                            {
                                InstrNameSelected = null;
                            }
                        }

                    }
                    //NSE CURRENCY 
                    if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Currency.ToString())
                    {
                        if (UnderLyingAssetLst.Count() > 0 && UnderLyingAssetLst != null)
                        {
                            ExpDateLst = new ObservableCollection<string>();
                            if (CallPutEnable == false)
                            {
                                foreach (var item in MasterSharedMemory.objMasterCurrencyDictBaseNSE.Values.Cast<CurrencyMasterBase>().OrderByDescending(y => y.ExpiryDate).Where(x => x.UnderlyingAsset == UnderAssetSelected && x.InstrumentType == IntrTypeSelected).Select(x => x.ExpiryDate).Distinct())
                                {
                                    ExpDateLst.Add(item);
                                }
                            }
                            else if (CallPutEnable == true)
                            {
                                foreach (var item in MasterSharedMemory.objMasterCurrencyDictBaseNSE.Values.Cast<CurrencyMasterBase>().OrderByDescending(y => y.ExpiryDate).Where(x => x.UnderlyingAsset == UnderAssetSelected && x.InstrumentType == IntrTypeSelected && x.OptionType == CallPutSelected).Select(x => x.ExpiryDate).Distinct())
                                {
                                    ExpDateLst.Add(item);
                                }
                            }
                            if (ExpDateLst.Count > 0 && ExpDateLst != null)
                            {
                                ExpDateSelected = ExpDateLst[0];
                            }
                        }
                    }
                }
                //NCDEX
                if (Selected_EXCH == Enumerations.Order.Exchanges.NCDEX.ToString())
                {
                    //Commodities 
                    if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Commodities.ToString())
                    {
                        ExpDateLst = new ObservableCollection<string>();
                        if (CallPutEnable == false)
                        {
                            foreach (var item in MasterSharedMemory.objNCDEXMasterDict.Values.Cast<NCDEXMaster>().OrderByDescending(y => y.NCDDisplayExpiryDate).Where(x => x.NCDSymbol == UnderAssetSelected && x.NCDInstrumentName == IntrTypeSelected).Select(x => x.NCDDisplayExpiryDate).Distinct())
                            {
                                ExpDateLst.Add(item);
                            }
                        }
                        else if (CallPutEnable == true)
                        {
                            foreach (var item in MasterSharedMemory.objNCDEXMasterDict.Values.Cast<NCDEXMaster>().OrderByDescending(y => y.NCDDisplayExpiryDate).Where(x => x.NCDSymbol == UnderAssetSelected && x.NCDInstrumentName == IntrTypeSelected && x.NCDOptionType == CallPutSelected).Select(x => x.NCDDisplayExpiryDate).Distinct())
                            {
                                ExpDateLst.Add(item);
                            }
                        }
                        if (ExpDateLst.Count > 0 && ExpDateLst != null)
                        {
                            ExpDateSelected = ExpDateLst[0];
                        }
                        else if (ExpDateSelected == null)
                        {
                            InstrNameSelected = null;
                        }
                    }
                }
                #region MCX
                //mcx
                if (Selected_EXCH == Enumerations.Order.Exchanges.MCX.ToString())
                {
                    //Commodities
                    //Commodities 
                    if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Commodities.ToString())
                    {
                        ExpDateLst = new ObservableCollection<string>();
                        if (CallPutEnable == false)
                        {
                            foreach (var item in MasterSharedMemory.objMCXMasterDict.Values.Cast<MCXMaster>().OrderByDescending(y => y.MCExpiryDate).Where(x => x.MCContractCode == UnderAssetSelected && x.MCInstrumentName == IntrTypeSelected).Select(x => x.MCDisplayExpiryDate).Distinct())
                            {
                                ExpDateLst.Add(item);
                            }
                        }
                        else if (CallPutEnable == true)
                        {
                            foreach (var item in MasterSharedMemory.objMCXMasterDict.Values.Cast<MCXMaster>().OrderByDescending(y => y.MCExpiryDate).Where(x => x.MCContractCode == UnderAssetSelected && x.MCInstrumentName == IntrTypeSelected && x.MCOptionType == CallPutSelected).Select(x => x.MCDisplayExpiryDate).Distinct())
                            {
                                ExpDateLst.Add(item);
                            }
                        }
                        if (ExpDateLst.Count > 0 && ExpDateLst != null)
                        {
                            ExpDateSelected = ExpDateLst[0];
                        }
                        else if (ExpDateSelected == null)
                        {
                            InstrNameSelected = null;
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
        private void PopulatingExpDate()
        {
            try
            {
                //BSE 

                if (Selected_EXCH == Enumerations.Order.Exchanges.BSE.ToString())
                {
                    //BSE DERIVATIVE
                    if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Derivative.ToString())
                    {
                        if (ExpDateLst.Count() > 0 && ExpDateLst != null)
                        {
                            InstrumentNameColl = new ObservableCollection<string>();
                            if (CallPutEnable == false)
                            {
                                foreach (var item in MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Cast<DerivativeMasterBase>().Where(x => x.UnderlyingAsset == UnderAssetSelected && x.InstrumentType == IntrTypeSelected && x.ExpiryDate == ExpDateSelected && x.StrategyID == -1).Select(x => x.InstrumentName).Distinct())
                                {
                                    InstrumentNameColl.Add(item);
                                }
                            }
                            else if (CallPutEnable == true)
                            {
                                StkPrcLst = new ObservableCollection<string>();

                                foreach (var item in MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Cast<DerivativeMasterBase>().OrderByDescending(y => y.StrikePrice).Where(x => x.UnderlyingAsset == UnderAssetSelected && x.InstrumentType == IntrTypeSelected && x.ExpiryDate == ExpDateSelected && x.OptionType == CallPutSelected).Select(x => x.StrikePrice))
                                {
                                    //StkPrcLst.Add(item);
                                    StkPrcLst.Add(Convert.ToString(Math.Truncate(Convert.ToDecimal(CommonFunctions.DisplayInDecimalFormatTouch(item, DecimalPoint)))));
                                }
                                if (StkPrcLst.Count > 0 && StkPrcLst != null)
                                {
                                    StkPriceSelected = StkPrcLst[0];
                                }
                                foreach (var item in MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Cast<DerivativeMasterBase>().Where(x => x.UnderlyingAsset == UnderAssetSelected && x.InstrumentType == IntrTypeSelected && x.ExpiryDate == ExpDateSelected && x.OptionType == CallPutSelected && x.StrikePrice == Convert.ToInt64(StkPriceSelected) * 100).Select(x => x.InstrumentName))
                                {
                                    InstrumentNameColl.Add(item);
                                }
                            }
                            if (InstrumentNameColl.Count > 0 && InstrumentNameColl != null)
                            {
                                InstrNameSelected = InstrumentNameColl[0];
                                Token = MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Cast<DerivativeMasterBase>().Where(x => x.InstrumentName == InstrNameSelected).Select(x => x.BowId).ToList();
                                ScripCodeDrv = MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Cast<DerivativeMasterBase>().Where(x => x.InstrumentName == InstrNameSelected).Select(x => x.ContractTokenNum).ToList();
                                //    DecimalPoint = 2;

                                //     ScripCodeDrv = MasterSharedMemory.objMasterDerivativeDictBaseBSE.Where(x => x.Value.InstrumentName.ToLower() == InstrNameSelected.ToLower()).Select(x => x.Key).FirstOrDefault();

                            }
                        }
                        if (ExpDateLst.Count() == 0 || ExpDateLst == null)
                        {
                            StkPriceSelected = string.Empty;
                            StkPrcLst = new ObservableCollection<string>();
                            InstrumentNameColl = new ObservableCollection<string>();
                            InstrNameSelected = string.Empty;

                        }
                    }
                    //BSE CURRENCY
                    if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Currency.ToString())
                    {
                        if (ExpDateLst.Count() > 0 && ExpDateLst != null)
                        {
                            InstrumentNameColl = new ObservableCollection<string>();
                            if (CallPutEnable == false)
                            {
                                foreach (var item in MasterSharedMemory.objMasterCurrencyDictBaseBSE.Values.Cast<CurrencyMasterBase>().Where(x => x.UnderlyingAsset == UnderAssetSelected && x.InstrumentType == IntrTypeSelected && x.ExpiryDate == ExpDateSelected).Select(x => x.InstrumentName).Distinct())
                                {
                                    InstrumentNameColl.Add(item);
                                }
                            }
                            else if (CallPutEnable == true)
                            {
                                StkPrcLst = new ObservableCollection<string>();

                                foreach (var item in MasterSharedMemory.objMasterCurrencyDictBaseBSE.Values.Cast<CurrencyMasterBase>().OrderByDescending(y => y.StrikePrice).Where(x => x.UnderlyingAsset == UnderAssetSelected && x.InstrumentType == IntrTypeSelected && x.ExpiryDate == ExpDateSelected && x.OptionType == CallPutSelected).Select(x => x.StrikePrice))
                                {
                                    // StkPrcLst.Add(item);
                                    //StkPrcLst.Add(Convert.ToString(Math.Truncate(Convert.ToDecimal(CommonFunctions.DisplayInDecimalFormatTouch(item, 2)))));
                                    StkPrcLst.Add(String.Format("{0:0.0000}", Convert.ToDecimal(CommonFunctions.DisplayInDecimalFormatTouch(item, DecimalPoint))).ToString());
                                }
                                if (StkPrcLst.Count > 0 && StkPrcLst != null)
                                {
                                    StkPriceSelected = StkPrcLst[0];

                                }
                                foreach (var item in MasterSharedMemory.objMasterCurrencyDictBaseBSE.Values.Cast<CurrencyMasterBase>().Where(x => x.UnderlyingAsset == UnderAssetSelected && x.InstrumentType == IntrTypeSelected && x.ExpiryDate == ExpDateSelected && x.OptionType == CallPutSelected && x.StrikePrice == Convert.ToInt64(StkPriceSelected) * 10000).Select(x => x.InstrumentName))
                                {
                                    InstrumentNameColl.Add(item);
                                }
                            }
                            if (InstrumentNameColl.Count > 0 && InstrumentNameColl != null)
                            {
                                InstrNameSelected = InstrumentNameColl[0];
                                Token = MasterSharedMemory.objMasterCurrencyDictBaseBSE.Values.Cast<CurrencyMasterBase>().Where(x => x.InstrumentName == InstrNameSelected).Select(x => x.BowId).ToList();
                                ScripCodeDrv = MasterSharedMemory.objMasterCurrencyDictBaseBSE.Values.Cast<CurrencyMasterBase>().Where(x => x.InstrumentName == InstrNameSelected).Select(x => x.ContractTokenNum).ToList();

                            }
                        }
                        if (ExpDateLst.Count() == 0 || ExpDateLst == null)
                        {
                            StkPriceSelected = string.Empty;
                            StkPrcLst = new ObservableCollection<string>();
                            InstrumentNameColl = new ObservableCollection<string>();
                            InstrNameSelected = string.Empty;
                        }

                    }
                    //Debt WholesaleDebtMarket objMasterWholesaleDebtMarket
                    if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Debt.ToString())
                    {
                        if (ExpDateLst.Count() > 0 && ExpDateLst != null)
                        {
                            InstrumentNameColl = new ObservableCollection<string>();
                            if (CallPutEnable == false)
                            {
                                foreach (var item in MasterSharedMemory.objMasterWholesaleDebtMarket.Values.Cast<WholesaleDebtMarket>().Where(x => x.WDMSymbol == UnderAssetSelected && x.WDMInstrumentName == IntrTypeSelected && x.WDMDisplayExpiryDate == ExpDateSelected).Select(x => x.WDMSymbol).Distinct())
                                {
                                    InstrumentNameColl.Add(item);
                                }
                            }


                            if (InstrumentNameColl.Count > 0 && InstrumentNameColl != null)
                            {
                                InstrNameSelected = InstrumentNameColl[0];
                                Token = MasterSharedMemory.objMasterWholesaleDebtMarket.Values.Cast<WholesaleDebtMarket>().Where(x => x.WDMSymbol == InstrNameSelected).Select(x => x.WDMID).ToList();
                                ScripCodeDrv = MasterSharedMemory.objMasterWholesaleDebtMarket.Values.Cast<WholesaleDebtMarket>().Where(x => x.WDMSymbol == InstrNameSelected).Select(x => x.WDMScriptCode).ToList();


                            }
                            StkPrcLst.Clear();
                            StkPriceSelected = string.Empty;
                        }
                    }
                    //BSE itp ITPMaster objITPMasterDict
                    if (ScripSelectedSegment == Enumerations.Order.ScripSegment.ITP.ToString())
                    {
                        if (ExpDateLst.Count() > 0 && ExpDateLst != null)
                        {
                            InstrumentNameColl = new ObservableCollection<string>();
                            if (CallPutEnable == false)
                            {
                                foreach (var item in MasterSharedMemory.objITPMasterDict.Values.Cast<ITPMaster>().Where(x => x.ITPScripID == UnderAssetSelected && x.ITPInstrumenName == IntrTypeSelected && x.ITPDisplayExpiryDate == ExpDateSelected).Select(x => x.ITPScripName).Distinct())
                                {
                                    InstrumentNameColl.Add(item);
                                }
                                StkPrcLst.Clear();
                                StkPriceSelected = string.Empty;

                            }


                            if (InstrumentNameColl.Count > 0 && InstrumentNameColl != null)
                            {

                                InstrNameSelected = InstrumentNameColl[0];
                                Token = MasterSharedMemory.objITPMasterDict.Values.Cast<ITPMaster>().Where(x => x.ITPScripID == InstrNameSelected).Select(x => x.ITPID).ToList();

                                // ScripCodeDrv =(Convert.ToInt64( MasterSharedMemory.objITPMasterDict.Values.Cast<ITPMaster>().Where(x => x.ITPInstrumenName == InstrNameSelected).Select(x => x.ITPScripCode))).ToList();
                                //todo : convert the scripcode
                            }

                        }
                    }
                    // SLB SLBMaster objSLBMasterDict
                    if (ScripSelectedSegment == Enumerations.Order.ScripSegment.SLB.ToString())
                    {
                        if (ExpDateLst.Count() > 0 && ExpDateLst != null)
                        {
                            InstrumentNameColl = new ObservableCollection<string>();
                            if (CallPutEnable == false)
                            {
                                foreach (var item in MasterSharedMemory.objSLBMasterDict.Values.Cast<SLBMaster>().Where(x => x.ScripID_ShortName == UnderAssetSelected && x.SCInstrumentName == IntrTypeSelected && x.SCDisplayExpiryDate == ExpDateSelected).Select(x => x.SCScripId).Distinct())
                                {
                                    InstrumentNameColl.Add(item);
                                }
                            }
                            StkPrcLst.Clear();
                            StkPriceSelected = string.Empty;

                            if (InstrumentNameColl.Count > 0 && InstrumentNameColl != null)
                            {
                                InstrNameSelected = InstrumentNameColl[0];
                                //      Token = MasterSharedMemory.objSLBMasterDict.Values.Cast<SLBMaster>().Where(x => x.ScripID_ShortName == InstrNameSelected).Select(x => x.SCID).ToList();
                                //     ScripCodeDrv = MasterSharedMemory.objSLBMasterDict.Values.Cast<SLBMaster>().Where(x => x.SCScripId == InstrNameSelected).Select(x => x.SCScripCode).ToList();
                                //todo : convert the scripcode
                            }

                        }
                    }
                }
                //NSE DERIVATIVE 
                if (Selected_EXCH == Enumerations.Order.Exchanges.NSE.ToString())
                {
                    if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Derivative.ToString())
                    {
                        if (ExpDateLst.Count() > 0 && ExpDateLst != null)
                        {
                            InstrumentNameColl = new ObservableCollection<string>();
                            if (CallPutEnable == false)
                            {
                                foreach (var item in MasterSharedMemory.objMasterDerivativeDictBaseNSE.Values.Cast<DerivativeMasterBase>().Where(x => x.UnderlyingAsset == UnderAssetSelected && x.InstrumentType == IntrTypeSelected && x.ExpiryDate == ExpDateSelected).Select(x => x.InstrumentName).Distinct())
                                {
                                    InstrumentNameColl.Add(item);
                                }
                            }
                            else if (CallPutEnable == true)
                            {
                                StkPrcLst = new ObservableCollection<string>();

                                foreach (var item in MasterSharedMemory.objMasterDerivativeDictBaseNSE.Values.Cast<DerivativeMasterBase>().OrderByDescending(y => y.StrikePrice).Where(x => x.UnderlyingAsset == UnderAssetSelected && x.InstrumentType == IntrTypeSelected && x.ExpiryDate == ExpDateSelected && x.OptionType == CallPutSelected).Select(x => x.StrikePrice))
                                {
                                    //StkPrcLst.Add(item);
                                    StkPrcLst.Add(Convert.ToString(Math.Truncate(Convert.ToDecimal(CommonFunctions.DisplayInDecimalFormatTouch(item, DecimalPoint)))));
                                }
                                if (StkPrcLst.Count > 0 && StkPrcLst != null)
                                {
                                    StkPriceSelected = StkPrcLst[0];
                                }
                                foreach (var item in MasterSharedMemory.objMasterDerivativeDictBaseNSE.Values.Cast<DerivativeMasterBase>().Where(x => x.UnderlyingAsset == UnderAssetSelected && x.InstrumentType == IntrTypeSelected && x.ExpiryDate == ExpDateSelected && x.OptionType == CallPutSelected && x.StrikePrice == Convert.ToInt64(StkPriceSelected) * 100).Select(x => x.InstrumentName))
                                {
                                    InstrumentNameColl.Add(item);
                                }
                            }
                            if (InstrumentNameColl.Count > 0 && InstrumentNameColl != null)
                            {
                                InstrNameSelected = InstrumentNameColl[0];
                                Token = MasterSharedMemory.objMasterDerivativeDictBaseNSE.Values.Cast<DerivativeMasterBase>().Where(x => x.InstrumentName == InstrNameSelected).Select(x => x.BowId).ToList();
                                ScripCodeDrv = MasterSharedMemory.objMasterDerivativeDictBaseNSE.Values.Cast<DerivativeMasterBase>().Where(x => x.InstrumentName == InstrNameSelected).Select(x => x.ContractTokenNum).ToList();

                            }
                        }
                        if (ExpDateLst.Count() == 0 || ExpDateLst == null)
                        {
                            StkPriceSelected = string.Empty;
                            StkPrcLst = new ObservableCollection<string>();
                            InstrumentNameColl = new ObservableCollection<string>();
                            InstrNameSelected = string.Empty;
                        }

                    }
                    //NSE CURRENCY
                    if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Currency.ToString())
                    {
                        if (ExpDateLst.Count() > 0 && ExpDateLst != null)
                        {

                            InstrumentNameColl = new ObservableCollection<string>();
                            if (CallPutEnable == false)
                            {
                                foreach (var item in MasterSharedMemory.objMasterCurrencyDictBaseNSE.Values.Cast<CurrencyMasterBase>().Where(x => x.UnderlyingAsset == UnderAssetSelected && x.InstrumentType == IntrTypeSelected && x.ExpiryDate == ExpDateSelected).Select(x => x.InstrumentName).Distinct())
                                {
                                    InstrumentNameColl.Add(item);
                                }
                                StkPrcLst.Clear();
                                StkPriceSelected = String.Empty;
                            }
                            else if (CallPutEnable == true)
                            {
                                StkPrcLst = new ObservableCollection<string>();

                                foreach (var item in MasterSharedMemory.objMasterCurrencyDictBaseNSE.Values.Cast<CurrencyMasterBase>().OrderByDescending(y => y.StrikePrice).Where(x => x.UnderlyingAsset == UnderAssetSelected && x.InstrumentType == IntrTypeSelected && x.ExpiryDate == ExpDateSelected && x.OptionType == CallPutSelected).Select(x => x.StrikePrice))
                                {
                                    // StkPrcLst.Add(item);
                                    StkPrcLst.Add(Convert.ToString(Math.Truncate(Convert.ToDecimal(CommonFunctions.DisplayInDecimalFormatTouch(item, DecimalPoint)))));
                                }
                                if (StkPrcLst.Count > 0 && StkPrcLst != null)
                                {
                                    StkPriceSelected = StkPrcLst[0];
                                }
                                foreach (var item in MasterSharedMemory.objMasterCurrencyDictBaseNSE.Values.Cast<CurrencyMasterBase>().Where(x => x.UnderlyingAsset == UnderAssetSelected && x.InstrumentType == IntrTypeSelected && x.ExpiryDate == ExpDateSelected && x.OptionType == CallPutSelected && x.StrikePrice == Convert.ToInt64(StkPriceSelected) * 10000).Select(x => x.InstrumentName))
                                {
                                    InstrumentNameColl.Add(item);
                                }
                            }
                            if (InstrumentNameColl.Count > 0 && InstrumentNameColl != null)
                            {
                                InstrNameSelected = InstrumentNameColl[0];
                                Token = MasterSharedMemory.objMasterCurrencyDictBaseNSE.Values.Cast<CurrencyMasterBase>().Where(x => x.InstrumentName == InstrNameSelected).Select(x => x.BowId).ToList();
                                ScripCodeDrv = MasterSharedMemory.objMasterCurrencyDictBaseNSE.Values.Cast<CurrencyMasterBase>().Where(x => x.InstrumentName == InstrNameSelected).Select(x => x.ContractTokenNum).ToList();
                                DecimalPoint = 4;
                            }
                        }
                        if (ExpDateLst.Count() == 0 || ExpDateLst == null)
                        {
                            StkPriceSelected = string.Empty;
                            StkPrcLst = new ObservableCollection<string>();
                            InstrumentNameColl = new ObservableCollection<string>();
                            InstrNameSelected = string.Empty;
                        }
                    }

                }
                //NCDEX COmmodity 

                if (Selected_EXCH == Enumerations.Order.Exchanges.NCDEX.ToString())
                {
                    if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Commodities.ToString())
                    {
                        if (ExpDateLst.Count() > 0 && ExpDateLst != null)
                        {
                            InstrumentNameColl = new ObservableCollection<string>();
                            if (CallPutEnable == false)
                            {   //commented as no stkprice given in db
                                //foreach (var item in MasterSharedMemory.objNCDEXMasterDict.Values.Cast<NCDEXMaster>().Where(x => x.NCDSymbol == UnderAssetSelected && x.NCDInstrumentName == IntrTypeSelected && x.NCDDisplayExpiryDate == ExpDateSelected).Select(x => x.NCDName).Distinct())
                                //{
                                //    InstrumentNameColl.Add(item);
                                //}
                                foreach (var item in MasterSharedMemory.objNCDEXMasterDict.Values.Cast<NCDEXMaster>().Where(x => x.NCDSymbol == UnderAssetSelected && x.NCDInstrumentName == IntrTypeSelected && x.NCDDisplayExpiryDate == ExpDateSelected).Select(x => x.NCDName).Distinct())
                                {
                                    InstrumentNameColl.Add(item);
                                }

                            }
                            else if (CallPutEnable == true)
                            {
                                StkPrcLst = new ObservableCollection<string>();

                                foreach (var item in MasterSharedMemory.objNCDEXMasterDict.Values.Cast<NCDEXMaster>().OrderByDescending(y => y.NCDExpiryDate).Where(x => x.NCDSymbol == UnderAssetSelected && x.NCDInstrumentName == IntrTypeSelected && x.NCDDisplayExpiryDate == ExpDateSelected && x.NCDOptionType == CallPutSelected).Select(x => x.NCDExpiryDate))
                                {
                                    //StkPrcLst.Add(item);
                                    StkPrcLst.Add(Convert.ToString(Math.Truncate(Convert.ToDecimal(CommonFunctions.DisplayInDecimalFormatTouch(item, DecimalPoint)))));
                                }
                                if (StkPrcLst.Count > 0 && StkPrcLst != null)
                                {
                                    StkPriceSelected = StkPrcLst[0];
                                }
                                foreach (var item in MasterSharedMemory.objNCDEXMasterDict.Values.Cast<NCDEXMaster>().Where(x => x.NCDSymbol == UnderAssetSelected && x.NCDInstrumentName == IntrTypeSelected && x.NCDDisplayExpiryDate == ExpDateSelected && x.NCDOptionType == CallPutSelected && x.NCDExpiryDate == Convert.ToInt64(StkPriceSelected)).Select(x => x.NCDName))
                                {
                                    InstrumentNameColl.Add(item);
                                }
                            }
                            if (InstrumentNameColl.Count > 0 && InstrumentNameColl != null)
                            {
                                InstrNameSelected = InstrumentNameColl[0];
                                Token = MasterSharedMemory.objNCDEXMasterDict.Values.Cast<NCDEXMaster>().Where(x => x.NCDName == InstrNameSelected).Select(x => x.NCDID).ToList();
                                ScripCodeDrv = MasterSharedMemory.objNCDEXMasterDict.Values.Cast<NCDEXMaster>().Where(x => x.NCDName == InstrNameSelected).Select(x => x.NCDToken).ToList();
                                DecimalPoint = 4;
                            }
                        }
                    }
                }
                //MCX
                if (Selected_EXCH == Enumerations.Order.Exchanges.MCX.ToString())
                {
                    //Commodities MCXMaster> objMCXMasterDict
                    //Commodities 
                    if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Commodities.ToString())
                    {
                        if (ExpDateLst.Count() > 0 && ExpDateLst != null)
                        {
                            InstrumentNameColl = new ObservableCollection<string>();
                            if (CallPutEnable == false)
                            {
                                //no name in db
                                //foreach (var item in MasterSharedMemory.objMCXMasterDict.Values.Cast<MCXMaster>().Where(x => x.MCContractCode == UnderAssetSelected && x.MCInstrumentName == IntrTypeSelected && x.MCDisplayExpiryDate == ExpDateSelected).Select(x => x.NCDName).Distinct())
                                //{
                                //    InstrumentNameColl.Add(item);
                                //}
                                InstrNameSelected = null;

                            }
                            else if (CallPutEnable == true)
                            {
                                StkPrcLst = new ObservableCollection<string>();
                                StkPrcLst = null;
                                if (StkPrcLst.Count > 0 && StkPrcLst != null)
                                {
                                    StkPriceSelected = StkPrcLst[0];
                                }

                                //foreach (var item in MasterSharedMemory.objMCXMasterDict.Values.Cast<MCXMaster>().Where(x => x.MCContractCode == UnderAssetSelected && x.MCInstrumentName == IntrTypeSelected && x.MCDisplayExpiryDate == ExpDateSelected && x.MCOptionType == CallPutSelected && x.MCStrikePrice == StkPriceSelected).Select(x => x.NCDName))
                                //{
                                //    InstrumentNameColl.Add(item);
                                //}
                            }

                            //if (InstrumentNameColl.Count > 0 && InstrumentNameColl != null)
                            //{
                            //    InstrNameSelected = InstrumentNameColl[0];

                            //    ScripCodeDrv = MasterSharedMemory.objMCXMasterDict.Values.Cast<MCXMaster>().Where(x => x.NCDName == InstrNameSelected).Select(x => x.NCDToken).ToList();
                            //    DecimalPoint = 4;
                            //}

                        }
                    }
                }

            }
            catch (Exception ex)
            {

                ExceptionUtility.LogError(ex);
            }

        }
        private void OnChangeOfStkPrc()
        {
            try
            {
                //BSE DERIVATIVES
                if (Selected_EXCH == Enumerations.Order.Exchanges.BSE.ToString())
                {
                    if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Derivative.ToString())
                    {
                        if (StkPrcLst.Count() > 0 && StkPrcLst != null)
                        {
                            long StkPrice = Convert.ToInt64(StkPriceSelected);
                            InstrumentNameColl = new ObservableCollection<string>();
                            foreach (var item in MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Cast<DerivativeMasterBase>().Where(x => x.UnderlyingAsset == UnderAssetSelected && x.InstrumentType == IntrTypeSelected && x.ExpiryDate == ExpDateSelected && x.OptionType == CallPutSelected && x.StrikePrice == StkPrice).Select(x => x.InstrumentName))
                            {
                                InstrumentNameColl.Add(item);
                            }
                            if (InstrumentNameColl.Count > 0 && InstrumentNameColl != null)
                            {
                                InstrNameSelected = InstrumentNameColl[0];
                            }
                            else
                                InstrNameSelected = string.Empty;
                        }
                    }
                    //BSE CURRENCY
                    if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Currency.ToString())
                    {
                        if (StkPrcLst != null && StkPrcLst?.Count() > 0)
                        {
                            InstrumentNameColl = new ObservableCollection<string>();
                            foreach (var item in MasterSharedMemory.objMasterCurrencyDictBaseBSE.Values.Cast<CurrencyMasterBase>().Where(x => x.UnderlyingAsset == UnderAssetSelected && x.InstrumentType == IntrTypeSelected && x.ExpiryDate == ExpDateSelected && x.OptionType == CallPutSelected && x.StrikePrice == Convert.ToInt64(StkPriceSelected)).Select(x => x.InstrumentName))
                            {
                                InstrumentNameColl.Add(item);
                            }
                            if (InstrumentNameColl.Count > 0 && InstrumentNameColl != null)
                            {
                                InstrNameSelected = InstrumentNameColl[0];
                            }
                        }
                    }
                    //Debt WholesaleDebtMarket objMasterWholesaleDebtMarket
                    if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Debt.ToString())
                    {
                        if (StkPrcLst.Count() > 0 && StkPrcLst != null)
                        {
                            InstrumentNameColl = new ObservableCollection<string>();
                            foreach (var item in MasterSharedMemory.objMasterWholesaleDebtMarket.Values.Cast<WholesaleDebtMarket>().Where(x => x.WDMSymbol == UnderAssetSelected && x.WDMInstrumentName == IntrTypeSelected && x.WDMDisplayExpiryDate == ExpDateSelected && x.WDMOptionType == CallPutSelected && x.WDMStrikePrice == Convert.ToInt64(StkPriceSelected)).Select(x => x.WDMSymbol))
                            {
                                InstrumentNameColl.Add(item);
                            }
                            if (InstrumentNameColl.Count > 0 && InstrumentNameColl != null)
                            {
                                InstrNameSelected = InstrumentNameColl[0];
                            }
                            else
                                InstrNameSelected = string.Empty;

                        }
                    }
                }
                //NSE DERIVATIVES
                if (Selected_EXCH == Enumerations.Order.Exchanges.NSE.ToString())
                {

                    if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Derivative.ToString())
                    {

                        if (StkPrcLst.Count() > 0 && StkPrcLst != null)
                        {
                            InstrumentNameColl = new ObservableCollection<string>();
                            foreach (var item in MasterSharedMemory.objMasterDerivativeDictBaseNSE.Values.Cast<DerivativeMasterBase>().Where(x => x.UnderlyingAsset == UnderAssetSelected && x.InstrumentType == IntrTypeSelected && x.ExpiryDate == ExpDateSelected && x.OptionType == CallPutSelected && x.StrikePrice == Convert.ToInt64(StkPriceSelected)).Select(x => x.InstrumentName))
                            {
                                InstrumentNameColl.Add(item);
                            }
                            if (InstrumentNameColl.Count > 0 && InstrumentNameColl != null)
                            {
                                InstrNameSelected = InstrumentNameColl[0];

                            }
                        }
                    }
                    //NSE CURRENCY
                    if (Selected_EXCH == Enumerations.Order.ScripSegment.Currency.ToString())
                    {
                        if (StkPrcLst.Count() > 0 && StkPrcLst != null)
                        {
                            InstrumentNameColl = new ObservableCollection<string>();
                            foreach (var item in MasterSharedMemory.objMasterCurrencyDictBaseNSE.Values.Cast<CurrencyMasterBase>().Where(x => x.UnderlyingAsset == UnderAssetSelected && x.InstrumentType == IntrTypeSelected && x.ExpiryDate == ExpDateSelected && x.OptionType == CallPutSelected && x.StrikePrice == Convert.ToInt64(StkPriceSelected)).Select(x => x.InstrumentName))
                            {
                                InstrumentNameColl.Add(item);
                            }
                            if (InstrumentNameColl.Count > 0 && InstrumentNameColl != null)
                            {
                                InstrNameSelected = InstrumentNameColl[0];
                            }
                        }
                    }
                }
                //NCDEX Commodity NCDEXMaster objNCDEXMasterDict
                if (Selected_EXCH == Enumerations.Order.Exchanges.MCX.ToString())
                {
                    if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Currency.ToString())
                    {
                        if (StkPrcLst.Count() > 0 && StkPrcLst != null)
                        {
                            InstrumentNameColl = new ObservableCollection<string>();
                            foreach (var item in MasterSharedMemory.objNCDEXMasterDict.Values.Cast<NCDEXMaster>().Where(x => x.NCDSymbol == UnderAssetSelected && x.NCDInstrumentName == IntrTypeSelected && x.NCDDisplayExpiryDate == ExpDateSelected && x.NCDOptionType == CallPutSelected && x.NCDStrikePrice == Convert.ToInt64(StkPriceSelected)).Select(x => x.NCDName))
                            {
                                InstrumentNameColl.Add(item);
                            }
                            if (InstrumentNameColl.Count > 0 && InstrumentNameColl != null)
                            {
                                InstrNameSelected = InstrumentNameColl[0];
                            }
                        }
                    }
                }
                //MCX
                if (Selected_EXCH == Enumerations.Order.Exchanges.MCX.ToString())
                {
                    //no instrument name in db yet
                }
            }
            catch (Exception ex)
            {

                ExceptionUtility.LogError(ex);
            }
        }
        #endregion


        private void OnChangeOfExchange()
        {
            Exchange = new List<string>();

            //if (MasterSharedMemory.objMastertxtDic != null)
            //    Exchange = MasterSharedMemory.objMastertxtDic.Values.Cast<ScipMaster>().GroupBy(x => x.ExchangeCode).Select(x => x.FirstOrDefault().ExchangeCode).ToList();
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

        private void EnableDisable()
        {
            try
            {
                if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Equity.ToString())
                {
                    EquityBoundVis = "Visible";  // To show Equity OE
                    DerivativeBoundVis = "Hidden"; //To Hide Derivative OE
                }
                else if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Derivative.ToString() || ScripSelectedSegment == Enumerations.Order.ScripSegment.Currency.ToString() || ScripSelectedSegment == Enumerations.Order.ScripSegment.Debt.ToString() || ScripSelectedSegment == Enumerations.Order.ScripSegment.Commodities.ToString() || ScripSelectedSegment == Enumerations.Order.ScripSegment.ITP.ToString() || ScripSelectedSegment == Enumerations.Order.ScripSegment.SLB.ToString())
                {
                    EquityBoundVis = "Hidden";  // To hide EQ OE
                    DerivativeBoundVis = "Visible"; // To show Derivative OE
                }
                if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Equity.ToString())
                {
                    CurrencyAssetEnable = false;
                    CpVisibility = "Hidden";
                }
                else
                {
                    CurrencyAssetEnable = true;
                    CpVisibility = "Visible";
                }


                if (clienttypeselected == Enumerations.Order.ClientTypes.OWN.ToString())
                {
                    clientinputEnabled = false;
                    ShortClientSelected = "OWN";
                }
                else if (clienttypeselected == Enumerations.Order.ClientTypes.INST.ToString() || clienttypeselected == Enumerations.Order.ClientTypes.CLIENT.ToString() || clienttypeselected == Enumerations.Order.ClientTypes.SPLCLI.ToString())
                {
                    clientinputEnabled = true;
                    if (ShortClientSelected.Equals("OWN"))
                        ShortClientSelected = String.Empty;
                    if (!(ShortClientSelected.Equals(String.Empty)) && (ShortClientSelected.Equals("OWN")))
                    {
                        ShortClientSelected = ShortClientSelected;
                    }
                }
                //else if (clienttypeselected == Enumerations.Order.ClientTypes.CLIENT.ToString() || clienttypeselected == Enumerations.Order.ClientTypes.SPLCLI.ToString() || clienttypeselected == Enumerations.Order.ClientTypes.INST.ToString())
                //{

                //    clientinputEnabled = true;
                //}

                if (OrderTypeSelected == Enumerations.Order.OrderTypes.STOPLOSS.ToString())
                {
                    TrgEnabled = true;
                }
#if TWS
                if (OrderTypeSelected == Enumerations.Order.OrderTypes.STOPLOSS.ToString() || OrderTypeSelected == "STOPLOSSMKT" || OrderTypeSelected == Enumerations.Order.OrderTypes.OCO.ToString())
                {
                    TrgEnabled = true;
                }
                if (OrderTypeSelected == "STOPLOSSMKT")
                    MktPrtEnabled = true;
#endif

                if (OrderTypeSelected == Enumerations.Order.OrderTypes.LIMIT.ToString() || OrderTypeSelected == Enumerations.Order.OrderTypes.MARKET.ToString())
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
                //if (Selected_EXCH != Enumerations.Order.Exchanges.BSE.ToString() && Selected_EXCH != Enumerations.Order.Exchanges.NSE.ToString())
                //{
                //    ScripCodeLst = null;
                //    ScripNameLst = null;
                //    ScripSymLst = null;
                //}
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
                            ScripSelectedCode = globalScripcode;
                        }

                    }
                    else if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Derivative.ToString())
                    {

                        if (UtilityOrderDetails.GETInstance.GlobalScripSelectedCode != 0)
                        {
                            globalScripcode = UtilityOrderDetails.GETInstance.GlobalScripSelectedCode;
                        }

                        PopulatingInstType();
                        PopulatingUnderlyingAsset();

                        if (globalScripcode != 0)
                        {
                            ScripSelectedCode = globalScripcode;
                        }
                    }
                    else if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Debt.ToString())
                    {

                        if (UtilityOrderDetails.GETInstance.GlobalScripSelectedCode != 0)
                        {
                            globalScripcode = UtilityOrderDetails.GETInstance.GlobalScripSelectedCode;
                        }

                        PopulatingInstType();
                        PopulatingUnderlyingAsset();

                        if (globalScripcode != 0)
                        {
                            ScripSelectedCode = globalScripcode;
                        }

                    }
                    else if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Currency.ToString())
                    {

                        if (UtilityOrderDetails.GETInstance.GlobalScripSelectedCode != 0)
                        {
                            globalScripcode = UtilityOrderDetails.GETInstance.GlobalScripSelectedCode;
                        }

                        PopulatingInstType();
                        PopulatingUnderlyingAsset();

                        if (globalScripcode != 0)
                        {
                            ScripSelectedCode = globalScripcode;
                        }
                    }
                    else if (ScripSelectedSegment == Enumerations.Order.ScripSegment.ITP.ToString())
                    {

                        PopulatingInstType();
                        PopulatingUnderlyingAsset();
                    }
                    else if (ScripSelectedSegment == Enumerations.Order.ScripSegment.SLB.ToString())
                    {

                        PopulatingInstType();
                        PopulatingUnderlyingAsset();
                    }
                }


                else if (Selected_EXCH == Enumerations.Order.Exchanges.NSE.ToString())
                {
                    if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Equity.ToString())
                    {

                        PopulateScripCode();
                        PopulatingScripName();
                        PopulatingScripSym();

                    }

                    else if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Derivative.ToString())
                    {
                        PopulatingInstType();
                        PopulatingUnderlyingAsset();


                    }
                    else if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Currency.ToString())
                    {
                        PopulatingInstType();
                        PopulatingUnderlyingAsset();


                    }

                }
                else if (Selected_EXCH == Enumerations.Order.Exchanges.NCDEX.ToString())
                {
                    // if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Commodities.ToString())
                    //{

                    PopulatingInstType();
                    PopulatingUnderlyingAsset();
                    //  }
                }
                else if (Selected_EXCH == Enumerations.Order.Exchanges.MCX.ToString())
                {
                    PopulatingInstType();
                    PopulatingUnderlyingAsset();
                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
            }

        }

        private void HideTitleBar()
        {
            isHideTitle = "None";
        }

        private void ShowTitleBar()
        {
            isHideTitle = "SingleBorderWindow";
        }

        public void BuySellWindow(object e)
        {

            if (e.ToString().Trim().ToUpper() == "BUY")
            {
                BuyVisible = "Visible";
                SellVisible = "Hidden";
                WindowColour = "#023199";
                BuySellInd = Enumerations.Order.BuySellFlag.B.ToString();
                HeaderTitle = string.Format("OrderEntry BUY   GrossBuyLimit - [{0:0.00} L]   GrossSellLimit - [{1:0.00} L]", Limit.g_Limit[CommonFunctions.GetSegment(ScripSelectedSegment)].AvailGrossBuyLimit / 100000, Limit.g_Limit[CommonFunctions.GetSegment(ScripSelectedSegment)].AvailGrossSellLimit / 100000);
            }
            else if (e.ToString().Trim().ToUpper() == "SELL")
            {
                SellVisible = "Visible";
                BuyVisible = "Hidden";
                //WindowColour = "#ffadb0";
                WindowColour = "#960f00";
                BuySellInd = Enumerations.Order.BuySellFlag.S.ToString();
                HeaderTitle = string.Format("OrderEntry SELL   GrossBuyLimit - [{0:0.00} L]   GrossSellLimit - [{1:0.00} L]", Limit.g_Limit[CommonFunctions.GetSegment(ScripSelectedSegment)].AvailGrossBuyLimit / 100000, Limit.g_Limit[CommonFunctions.GetSegment(ScripSelectedSegment)].AvailGrossSellLimit / 100000);
            }
            ClearAll();


        }

        public void SetScripCodeandID()
        {
            if (UtilityOrderDetails.GETInstance.GlobalScripSelectedCode != 0)
            {
                ScripSelectedCode = UtilityOrderDetails.GETInstance.GlobalScripSelectedCode;
            }

        }

        public void PassByReturnOrder(long scripCode, string Key)
        {
            if (MemoryManager.OrderDictionary.ContainsKey(Key))
            {
                int DecimalPoint = CommonFunctions.GetDecimal(Convert.ToInt32(scripCode), "BSE", CommonFunctions.GetSegmentID(scripCode));
                ScripSelectedSegment = CommonFunctions.GetSegmentID(scripCode);
                Selected_EXCH = Enumerations.Order.Exchanges.BSE.ToString();
                //IntrTypeSelected = scripCode.ToString().Trim();
                ScripSelectedCode = scripCode;
                qty = MemoryManager.OrderDictionary[Key].PendingQuantity.ToString();
                rate = (MemoryManager.OrderDictionary[Key].Price / Math.Pow(10, DecimalPoint)).ToString();
                ShortClientSelected = MemoryManager.OrderDictionary[Key].ClientId;
                clienttypeselected = MemoryManager.OrderDictionary[Key].ClientType;
                RetTypeSelected = MemoryManager.OrderDictionary[Key].OrderRetentionStatus;
                revQty = MemoryManager.OrderDictionary[Key].RevealQty.ToString();
                if (MemoryManager.OrderDictionary[Key].OrderType == "L")
                {
                    OrderTypeSelected = Enumerations.Order.OrderTypes.LIMIT.ToString();
                }
                else if (MemoryManager.OrderDictionary[Key].OrderType == "G")
                {
                    OrderTypeSelected = Enumerations.Order.OrderTypes.MARKET.ToString();
                    MktPT = MemoryManager.OrderDictionary[Key].ProtectionPercentage;
                }
                else if (MemoryManager.OrderDictionary[Key].OrderType == "P")
                {
                    OrderTypeSelected = Enumerations.Order.OrderTypes.STOPLOSS.ToString();
                    trgPrice = (MemoryManager.OrderDictionary[Key].TriggerPrice / Math.Pow(10, DecimalPoint)).ToString();
                }
            }

            else
            {
                //TODO: 
            }

        }

        internal void PassByPassByNetPositionScripWiseDetails(long scripCode, ScripWiseDetailPositionModel selectEntireRow)
        {
            int DecimalPoint = CommonFunctions.GetDecimal(Convert.ToInt32(scripCode), "BSE", CommonFunctions.GetSegmentID(scripCode));
            ScripSelectedSegment = CommonFunctions.GetSegmentID(scripCode);
            Selected_EXCH = Enumerations.Order.Exchanges.BSE.ToString();
            //IntrTypeSelected = scripCode.ToString().Trim();
            ScripSelectedCode = scripCode;
            if (selectEntireRow.NetQty < 0)
                qty = Convert.ToString(selectEntireRow.NetQty * -1);
            else
                qty = Convert.ToString(selectEntireRow.NetQty);
            ShortClientSelected = selectEntireRow.ClientID;
            clienttypeselected = selectEntireRow.ClientType;
        }

        internal void PassByNetPositionClientWiseDetails(long scripCode, CWSWDetailPositionModel selectEntireRow)
        {
            int DecimalPoint = CommonFunctions.GetDecimal(Convert.ToInt32(scripCode), "BSE", CommonFunctions.GetSegmentID(scripCode));
            ScripSelectedSegment = CommonFunctions.GetSegmentID(scripCode);
            Selected_EXCH = Enumerations.Order.Exchanges.BSE.ToString();
            //IntrTypeSelected = scripCode.ToString().Trim();
            ScripSelectedCode = scripCode;
            if (selectEntireRow.NetQty < 0)
                qty = Convert.ToString(selectEntireRow.NetQty * -1);
            else
                qty = Convert.ToString(selectEntireRow.NetQty);
            ShortClientSelected = selectEntireRow.ClientID;
            clienttypeselected = selectEntireRow.ClientType;
        }

        public void PopulateOrderType()
        {
#if BOW

            OrderTypeSelected = Enumerations.Order.OrderTypes.LIMIT.ToString();
            OrderTypeList.Add(Enumerations.Order.OrderTypes.LIMIT.ToString());
            OrderTypeList.Add(Enumerations.Order.OrderTypes.MARKET.ToString());
            OrderTypeList.Add(Enumerations.Order.OrderTypes.STOPLOSS.ToString());
            OrderTypeList.Add(Enumerations.Order.OrderTypes.OCO.ToString());

#endif

#if TWS
            //OrderTypeSelected = Enumerations.Order.OrderTypes.LIMIT.ToString();
            //OrderTypeList.Add(Enumerations.Order.OrderTypes.LIMIT.ToString());
            //OrderTypeList.Add(Enumerations.Order.OrderTypes.MARKET.ToString());
            //OrderTypeList.Add(Enumerations.Order.OrderTypes.STOPLOSS.ToString());
            // OrderTypeList.Add(Enumerations.Order.OrderTypes.STOPLOSSMKT.ToString());
            //    OrderTypeList.Add(Enumerations.Order.OrderTypes.OCO.ToString());
            // OrderTypeList.Add(Enumerations.Order.OrderTypes.BOC.ToString());
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
                ClientIDinputlst = new List<string>();
                ClientIDinputlst.Add("Client ID");
                ShortClientSelected = ClientIDinputlst[0];

                if (objClientMasterDict != null && objClientMasterDict.Count > 0)
                    ClientIDinputlst = MasterSharedMemory.objClientMasterDict.Select(x => x.Value.ShortClientId).ToList();

                if (ClientIDinputlst.Count() > 0)
                {
                    ShortClientSelected = ClientIDinputlst[0];
                    ClientName = MasterSharedMemory.objClientMasterDict.Where(x => x.Value.ShortClientId == ShortClientSelected).Select(y => y.Value.FirstName).First().ToString();
                    //foreach (var item in MasterSharedMemory.objClientMasterLst)
                    //{
                    //    if (item.ShortClientId== ShortClientSelected)
                    //    {
                    //        ClientName = item.FirstName;
                    //    }
                    //}
                }
                else
                {
                    ShortClientSelected = "CLIENT ID";
                }



                //foreach (var item in MasterSharedMemory.objClientMasterLst)
                //{
                //    ClientIDinputlst.Add(item.ShortClientId);
                //}
                // //if (MasterSharedMemory.objClientMasterLst != null)
                //// ScripNameLst = MasterSharedMemory.
                // if (ScripNameLst.Count() > 0)
                //     ScripNameSelected = ScripNameLst[0];
                // if (CLientIdinput != null)
                //     ShortClientSelected = CLientIdinput;
                // else
                //{
                //      //TODO get clientid values from client profile
                //      //ClientIDinputlst.Add(" from client profile");
                //      if (clienttypeselected == Enumerations.Order.ClientTypes.Own.ToString())
                //          ShortClientSelected = "Own";
                //      else
                //          ShortClientSelected = string.Empty;
                //  }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
            }

        }

        private void OnCheckClientTypeChange()
        {
            try
            {
                if (ClientIDinputlst.Contains(ShortClientSelected))
                {
                    var count = MasterSharedMemory.objClientMasterDict.Values.Where(x => x.ShortClientId == ShortClientSelected).Count();
                    if (count > 0)
                    {
                        ClientName = MasterSharedMemory.objClientMasterDict.Values.Where(x => x.ShortClientId == ShortClientSelected).FirstOrDefault().FirstName;
                    }
                    else
                    {
                        ClientName = "Client Name";
                    }


                }
                else
                    ClientName = "Client Name";
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
            //ShortClientSelected = String.Empty;
            Clienttypelst.Add(Enumerations.Order.ClientTypes.CLIENT.ToString());
            Clienttypelst.Add(Enumerations.Order.ClientTypes.SPLCLI.ToString());
            Clienttypelst.Add(Enumerations.Order.ClientTypes.INST.ToString());
            Clienttypelst.Add(Enumerations.Order.ClientTypes.OWN.ToString());

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

                                MarketLot = MasterSharedMemory.objMastertxtDictBaseBSE.Where(x => x.Key == ScripSelectedCode).Select(x => x.Value.MarketLot).FirstOrDefault();
                                TickSize = MasterSharedMemory.objMastertxtDictBaseBSE.Where(x => x.Key == ScripSelectedCode).Select(x => x.Value.TickSize).FirstOrDefault().ToString();
                                Group = CommonFunctions.GetGroupName(ScripSelectedCode, Enumerations.Exchange.BSE, Enumerations.Segment.Equity);
                                Series = "EQ";
                            }
                        }
                        //DERIVATIVE
                        if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Derivative.ToString())
                        {
                            if (MasterSharedMemory.objMastertxtDictBaseBSE != null)
                            {

                                MarketLot = MasterSharedMemory.objMastertxtDictBaseBSE.Where(x => x.Key == ScripSelectedCode).Select(x => x.Value.MarketLot).FirstOrDefault();
                                TickSize = MasterSharedMemory.objMastertxtDictBaseBSE.Where(x => x.Key == ScripSelectedCode).Select(x => x.Value.TickSize).FirstOrDefault().ToString();
                                Group = CommonFunctions.GetGroupName(ScripSelectedCode, Enumerations.Exchange.BSE, Enumerations.Segment.Derivative);
                                Series = "EQ";
                            }
                        }
                        //bse currency
                        if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Currency.ToString())
                        {
                            if (MasterSharedMemory.objMasterCurrencyDictBaseBSE != null)
                            {

                                MarketLot = MasterSharedMemory.objMasterCurrencyDictBaseBSE.Where(x => x.Key == ScripSelectedCode).Select(x => x.Value.MinimumLotSize).FirstOrDefault();
                                TickSize = MasterSharedMemory.objMasterCurrencyDictBaseBSE.Where(x => x.Key == ScripSelectedCode).Select(x => x.Value.TickSize).FirstOrDefault().ToString();
                                Group = CommonFunctions.GetGroupName(ScripSelectedCode, Enumerations.Exchange.BSE, Enumerations.Segment.Derivative);
                                Series = "EQ";
                            }
                        }
                        //Debt WholesaleDebtMarket objMasterWholesaleDebtMarket
                        if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Debt.ToString())
                        {
                            if (MasterSharedMemory.objMasterWholesaleDebtMarket != null)
                            {

                                MarketLot = MasterSharedMemory.objMasterWholesaleDebtMarket.Where(x => x.Key == ScripSelectedCode).Select(x => x.Value.WDMMarketLot).FirstOrDefault();
                                TickSize = MasterSharedMemory.objMasterWholesaleDebtMarket.Where(x => x.Key == ScripSelectedCode).Select(x => x.Value.WDMTickSize).FirstOrDefault().ToString();
                                Group = CommonFunctions.GetGroupName(ScripSelectedCode, Enumerations.Exchange.BSE, Enumerations.Segment.Debt);
                                Series = "EQ";
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

        /// <summary>
        /// Assign local(Field) values to <see cref="omodel"/>
        /// </summary>
        private void ModelCreation()

        {
            omodel = new OrderModel();
            DecimalPoint = CommonFunctions.GetDecimal(Convert.ToInt32(ScripSelectedCode), Selected_EXCH, ScripSelectedSegment);

            //if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Equity.ToString())
            //{
#if BOW
            omodel.Segment = ScripSelectedSegment;
            omodel.Exchange = Selected_EXCH;
            omodel.ScripCode = ScripSelectedCode;
            omodel.ScripName = ScripNameSelected;
            omodel.Symbol = ScripSymSelected;
            omodel.Series = Series;
            omodel.MarketLot = MarketLot;
            omodel.TickSize = TickSize;
            omodel.Group = Group;
            omodel.Token = objMastertxtDictBaseBSE.Where(x => x.Key == ScripSelectedCode).Select(x => x.Value.BowTokenID).FirstOrDefault();
            ModifyVisible = "Hidden";
            if (omodel.Mode == Convert.ToInt32(Enumerations.Order.Modes.Add))
                omodel.DisclosedVolumeRemaining = Convert.ToInt32(revQty);
            omodel.DisclosedVolumeRemaining = Convert.ToInt32(revQty);
            omodel.BuySellIndicator = BuySellInd;
            if (!string.IsNullOrEmpty(qty))
                omodel.Quantity = Convert.ToInt32(qty);
            if (!string.IsNullOrEmpty(revQty))
                omodel.RevealQty = Convert.ToInt32(revQty);
            omodel.OrderRetentionStatus = RetTypeSelected;
            omodel.OrderType = OrderTypeSelected;
            omodel.Price = Convert.ToInt64(rate);
            if (omodel.OrderType == Enumerations.Order.OrderTypes.STOPLOSS.ToString())
            {
                omodel.TriggerPrice = Convert.ToInt64(trgPrice);
            }
            if (omodel.OrderType == Enumerations.Order.OrderTypes.MARKET.ToString())
            {
                omodel.ProtectionPercentage = MktPT;
            }
            omodel.ClientType = clienttypeselected;
            omodel.ClientId = ShortClientSelected.ToUpper();
            omodel.OrderRemarks = Remarks;
            omodel.Delivery = intracheck;
            
            
#elif TWS
            omodel.Exchange = Selected_EXCH; //1- BSE, 2-BOW
            omodel.Segment = ScripSelectedSegment; //1 - Equity, 2 - Derv., 4.Curr
            omodel.SegmentFlag = CommonFunctions.SegmentFlag(omodel.Segment); //(int)Enum.Parse(typeof(Enumerations.Order.ScripSegment), ScripSelectedSegment);
            omodel.ClientType = clienttypeselected.ToUpper();
            //omodel.ClientId = ShortClientSelected.ToUpper();//Client Id
            if (omodel.ClientType == Enumerations.Order.ClientTypes.CLIENT.ToString() || omodel.ClientType == Enumerations.Order.ClientTypes.INST.ToString() || omodel.ClientType == Enumerations.Order.ClientTypes.SPLCLI.ToString())
            {
                omodel.ClientId = ShortClientSelected.ToUpper();//Client Id
            }
            else if (omodel.ClientType == Enumerations.Order.ClientTypes.OWN.ToString())
            {
                omodel.ClientId = Enumerations.Order.ClientTypes.OWN.ToString();//in case of own
            }
            omodel.ScripCode = ScripSelectedCode;//532976;//500325;
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
            omodel.Symbol = ScripSymSelected;
            omodel.OrderRemarks = Remarks;

            if (OrderTypeSelected.ToUpper() == Enumerations.Order.OrderTypes.LIMIT.ToString().ToUpper())
            {
                omodel.OrderType = "L";
            }
            else if (OrderTypeSelected.ToUpper() == Enumerations.Order.OrderTypes.OCO.ToString().ToUpper())
            {
                omodel.OrderType = "L";
                omodel.IsOCOOrder = true;
            }
            else if (OrderTypeSelected.ToUpper() == Enumerations.Order.OrderTypes.MARKET.ToString().ToUpper())
            {
                omodel.OrderType = "G";
            }
            else if (OrderTypeSelected.ToUpper() == Enumerations.Order.OrderTypes.BLOCKDEAL.ToString().ToUpper())
            {
                omodel.OrderType = "K";
            }
            else if (OrderTypeSelected.ToUpper() == Enumerations.Order.OrderTypes.STOPLOSS.ToString().ToUpper())
            {
                omodel.OrderType = "P";
            }
            else if (OrderTypeSelected.ToUpper() == "STOPLOSSMKT")//Enumerations.Order.OrderTypes.STOPLOSSMKT.ToString().ToUpper())
            {
                omodel.OrderType = "P";
            }
            //omodel.OrderType = OrderTypeSelected;

            if (omodel.OrderType == "G" || omodel.OrderType == "P")
            {
                omodel.ProtectionPercentage = Convert.ToString(MktPT);//MarketProtection
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
            omodel.ScreenId = (int)Enumerations.WindowName.Swift_OE;//"SwiftOrder01";

            omodel.MarketLot = Convert.ToInt32(CommonFunctions.GetMarketLot((long)ScripSelectedCode));// MarketLot;
            omodel.TickSize = CommonFunctions.GetTickSize((long)ScripSelectedCode);//TickSize;
            omodel.Group = CommonFunctions.GetGroupName((long)ScripSelectedCode, "BSE", ScripSelectedSegment); //Group;
            omodel.ExecInst = Enumerations.Order.ExecInst.PersistentOrder.ToString();
            omodel.ParticipantCode = "";
            omodel.FreeText3 = "fdf";
            omodel.Filler_c = "fdf";
            omodel.PartitionID = Convert.ToUInt16(CommonFunctions.GetPartitionId((long)ScripSelectedCode, "BSE", ScripSelectedSegment));
            omodel.MarketSegmentID = CommonFunctions.GetProductId((long)ScripSelectedCode, "BSE", ScripSelectedSegment);
            //}
            if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Derivative.ToString() || ScripSelectedSegment == Enumerations.Order.ScripSegment.Currency.ToString() || ScripSelectedSegment == Enumerations.Order.ScripSegment.Debt.ToString() || ScripSelectedSegment == Enumerations.Order.ScripSegment.Commodities.ToString() || ScripSelectedSegment == Enumerations.Order.ScripSegment.SLB.ToString() || ScripSelectedSegment == Enumerations.Order.ScripSegment.ITP.ToString())
            {

                omodel.ScripName = InstrNameSelected;
                omodel.ScripCode = ScripCodeDrv[0];
                omodel.Symbol = ScripSymSelected;//UnderAssetSelected;
            }

#endif
            if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Derivative.ToString() || ScripSelectedSegment == Enumerations.Order.ScripSegment.Currency.ToString() || ScripSelectedSegment == Enumerations.Order.ScripSegment.Debt.ToString() || ScripSelectedSegment == Enumerations.Order.ScripSegment.Commodities.ToString() || ScripSelectedSegment == Enumerations.Order.ScripSegment.SLB.ToString() || ScripSelectedSegment == Enumerations.Order.ScripSegment.ITP.ToString())
            {
                omodel.ScripName = InstrNameSelected;
                omodel.ScripCode = ScripCodeDrv[0];
                omodel.Symbol = ScripSymSelected;//UnderAssetSelected;
            }
        }




        #region commented
        //omodel.ExpiryDate = ExpDateSelected;
        //omodel.OptionType = CallPutSelected;
        //omodel.StrikePrice = StkPriceSelected; //strikeprice
        //
        //omodel.ScripName = InstrNameSelected;
        //omodel.ScripCode = ScripCodeDrv[0];
        //omodel.MarketLot = MarketLot;
        //omodel.TickSize = TickSize;
        //omodel.Group = Group;
        ////For Bse
        //if (Selected_EXCH == Enumerations.Order.Exchanges.BSE.ToString())
        //{
        //    //for equity market
        //    if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Equity.ToString())
        //    {
        //        if (MasterSharedMemory.objMastertxtDictBaseBSE != null)
        //        {
        //            omodel.MarketLot = MarketLot;
        //            omodel.MarketLot = MasterSharedMemory.objMastertxtDictBaseBSE.Where(x => x.Key == ScripSelectedCode).Select(x => x.Value.MarketLot).FirstOrDefault();
        //            omodel.TickSize = MasterSharedMemory.objMastertxtDictBaseBSE.Where(x => x.Key == ScripSelectedCode).Select(x => x.Value.TickSize).FirstOrDefault().ToString();
        //            omodel.Group = CommonFunctions.GetGroupName(ScripSelectedCode, Enumerations.Exchange.BSE, Enumerations.Segment.Equity);
        //            omodel.Series = "EQ";
        //        }
        //    }
        //}
        ////For NSE
        ////for NSE Exchange
        //if (Selected_EXCH == Enumerations.Order.Exchanges.NSE.ToString())
        //{
        //    //for equity market
        //    if (ScripSelectedSegment == Enumerations.Order.ScripSegment.Equity.ToString())
        //    {
        //        if (MasterSharedMemory.objMastertxtDictBaseNSE != null)
        //        {
        //            omodel.MarketLot = MasterSharedMemory.objMastertxtDictBaseNSE.Where(x => x.Key == ScripSelectedCode).Select(x => x.Value.MarketLot).FirstOrDefault();
        //            omodel.TickSize = MasterSharedMemory.objMastertxtDictBaseNSE.Where(x => x.Key == ScripSelectedCode).Select(x => x.Value.TickSize).FirstOrDefault().ToString();
        //            omodel.Group = CommonFunctions.GetGroupName(ScripSelectedCode, Enumerations.Exchange.NSE, Enumerations.Segment.Equity);
        //            omodel.Series = "EQ";
        //        }
        //    }
        //}
        #endregion

        /// <summary>
        /// To change selected scripname and ScripCode upon selection of ScripSymbol.
        /// </summary>
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
                            ScripSelectedCode = CommonFunctions.GetScripCodeFromScripID(ScripSymSelected, "BSE", ScripSelectedSegment);//BSE EQUITY
                            if (ScripSelectedCode != 0)
                                ScripNameSelected = MasterSharedMemory.objMastertxtDictBaseBSE[(long)ScripSelectedCode].ScripName;
                            else
                                ScripSelectedCode = 0;
                        }
                    }
                    else if (Selected_EXCH == Enumerations.Exchange.NSE.ToString())
                    {
                        ScripSelectedCode = CommonFunctions.GetScripCodeFromScripID(ScripSymSelected, "BSE", ScripSelectedSegment);//NSE EQUITY
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

        }

        /// <summary>
        /// To change selected scripname and ScripSymbol  upon selection of ScripCode.
        /// </summary>
        private void OnChangeOfScripCode()
        {
            UtilityOrderDetails.GETInstance.GlobalScripSelectedCode = ScripSelectedCode;
            if (flag)
            {
                scripIdChange = false;
                if (ScripSelectedCode != '0')
                {
                    //ScripSymSelected = CommonFunctions.GetScripId((long)ScripSelectedCode);
                    //ScripNameSelected = MasterSharedMemory.objMastertxtDictBase[(long)ScripSelectedCode].ScripName;
                    if (Selected_EXCH == Enumerations.Exchange.BSE.ToString())
                    {
                        ScripSymSelected = CommonFunctions.GetScripId((long)ScripSelectedCode, "BSE", ScripSelectedSegment);//BSE Equity
                        if (ScripSymSelected != null && ScripSymSelected != string.Empty && ScripSymSelected != "")
                        {
                            ScripNameSelected = CommonFunctions.GetScripName((long)ScripSelectedCode, "BSE", ScripSelectedSegment); //MasterSharedMemory.objMastertxtDictBaseBSE[(long)ScripSelectedCode].ScripName;

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
        }

        /// <summary>
        /// To get Scripcode corresponding to the selected scrip symbol
        /// </summary>
        /// <param name="scripName"></param>
        /// <returns></returns>
        //public static long GetScripCode(string scripName)
        //{
        //    long scripCode = 0;

        //    if (MasterSharedMemory.objMastertxtDictBase != null && MasterSharedMemory.objMastertxtDictBase.Count > 0)
        //    {
        //        scripCode = MasterSharedMemory.objMastertxtDictBase.FirstOrDefault(x => x.Value.ScripName.ToLower() == scripName.Trim().ToLower()).Key;
        //    }
        //    return scripCode;
        //}
        ///// <summary>
        ///// To get ScripName corresponding to the selected scrip code
        ///// </summary>
        ///// <param name="scripCode"></param>
        ///// <returns></returns>
        //public string GetScripName(long scripCode)
        //{
        //    string scripName = string.Empty;

        //    if (MasterSharedMemory.objMastertxtDictBase != null && MasterSharedMemory.objMastertxtDictBase.Count > 0)
        //    {
        //        if (MasterSharedMemory.objMastertxtDictBase.ContainsKey(scripCode))
        //        {
        //            scripName = MasterSharedMemory.objMastertxtDictBase[scripCode].ScripName;
        //        }
        //    }

        //    return scripName;
        //}
        ///// <summary>
        ///// To change scripcode on scrip name selection.Not a functionality as per tws
        ///// </summary>
        //        private void OnChangeOfScripName()
        //        {
        //            if (flag1)
        //            {
        //                scripIdChange = false;
        //                flag = false;
        //                if (ScripNameSelected != null)
        //                {
        //                    ScripSelectedCode = GetScripNameCode(ScripNameSelected);
        //                    ScripSymSelected = GetScripName((long)ScripSelectedCode);

        //                }
        //                else
        //                {
        //                    ScripSymSelected = null;
        //                    ScripSymSelected = string.Empty;
        //                }
        //            }
        //            flag = true;
        //        }
        ///// <summary>
        /////TODO : to get scripcode for selected scripname
        ///// </summary>
        ///// <param name="scripname"></param>
        ///// <returns></returns>
        //public long GetScripNameCode(string scripname)
        //{
        //    long scripcode = 0;

        //    if (MasterSharedMemory.objMasterScripCdScripIdMapp != null && MasterSharedMemory.objMasterScripCdScripIdMapp.Count > 0)
        //    {
        //        scripcode = MasterSharedMemory.objMasterScripCdScripIdMapp.FirstOrDefault(x => x.Value.ToLower() == scripname.Trim().ToLower()).Key;

        //    }

        //    return scripcode;
        //}

        private void ClearAll()
        {
            qty = String.Empty;
            rate = String.Empty;
            revQty = String.Empty;
            MktPT = String.Empty;
            ShortClientSelected = String.Empty;
            Remarks = String.Empty;
            intracheck = false;
        }

        internal void HideShowControlOnModifyClick(OrderModel oOrderModel)
        {
            SellVisible = "Hidden";
            BuyVisible = "Hidden";
            ModifyVisible = "Visible";
            object e = oOrderModel.BuySellIndicator;
            BuySellWindow(e);
        }

        internal void DefaultOrderProfileSettings()
        {
            revQty = MainWindowVM.parserOS.GetSetting("GENERAL OS", "RevQty");
            MktPT = MainWindowVM.parserOS.GetSetting("GENERAL OS", "MarketProtection");
            DefaultFieldFocus = MainWindowVM.parserOS.GetSetting("SWIFT OS", "SelectedDefaultFocusForSwiftOE");


        }

        /// <summary>
        /// Form the OrderModel by assigning local values <see cref="ModelCreation"/> and calling ValdiateOrder. Called on Click of Buy/Sell button
        /// </summary>
        private void SubmitButton_Click()
        {

            OrderRequestProcessor oOrderRequestProcessor = null;

            ModelCreation();
#if TWS
            omodel.OrderAction = Enumerations.Order.Modes.A.ToString();
#elif BOW
            omodel.Mode = Convert.ToInt32(Enumerations.Order.Modes.Add);
#endif
            bool validate = Validations.ValidateOrder(rate, trgPrice, omodel, ref Validate_Message, DecimalPoint);

            PostUIValidationTWSProcessOrder(ref omodel);
            //Assign Price 

            if (!validate)
            {
                ErrorPopUpWindow win = System.Windows.Application.Current.Windows.OfType<ErrorPopUpWindow>().FirstOrDefault();
                SwiftOrderEntry swiftwin = System.Windows.Application.Current.Windows.OfType<SwiftOrderEntry>().FirstOrDefault();
                win = new ErrorPopUpWindow();
                ErrorPopUpVm.msg = Validate_Message;
                win.Owner = swiftwin;
                win.Show();


                //Open the PopUpNotificationScreen with appropriate message P.S yet to decide 
                //  SwiftOrderEntry swiftwin = new SwiftOrderEntry();
                // ErrorPopUpWindow win = new View.Order.ErrorPopUpWindow();
                return;

            }
            else
            {
                //Convert Price in Long and send to processing
                if (omodel.OrderType == "G" || omodel.OrderType == "P")
                {
                    omodel.Price = 0;
                }
                else
                {
                    omodel.Price = Convert.ToInt64(Convert.ToDouble(rate) * Math.Pow(10, DecimalPoint));
                }
                if (omodel.OrderType == "G")// stoploss mkt
                {
                    var protectionPercent = Convert.ToInt16(Convert.ToDecimal(omodel.ProtectionPercentage) * 100);
                    omodel.ProtectionPercentage = Convert.ToString(protectionPercent);
                }

                if (!string.IsNullOrEmpty(trgPrice))
                    omodel.TriggerPrice = Convert.ToInt64(Convert.ToDouble(trgPrice) * Math.Pow(10, DecimalPoint));
                oOrderRequestProcessor = new OrderRequestProcessor(new AddOrder());
                oOrderRequestProcessor.ProcessRequest(omodel);

                //WindowColour = "#00802b";
                // HeaderTitle = "Order Placed Successfully";
            }

        }
        #region Commented Normal or swift Order Entry should not have modify button
        //        private void Modify_Click_Button()
        //        {
        //            Validate_Message = string.Empty;
        //            ModelCreation();
        //            bool validate = Validations.ValidateOrder(rate, trgPrice, omodel, ref Validate_Message, DecimalPoint);

        //            if (!validate)
        //            {
        //                ErrorPopUpWindow win = new View.Order.ErrorPopUpWindow();
        //                ErrorPopUpVm.msg = Validate_Message;
        //                win.Show();
        //                return;

        //            }
        //            else
        //            {
        //                //Convert Price in Long and send to processing
        //                omodel.Price = Convert.ToInt64(Convert.ToDouble(rate) * Math.Pow(10, DecimalPoint));
        //#if BOW
        //                omodel.Mode = Convert.ToInt32(Enumerations.Order.Modes.Edit);
        //#elif TWS
        //                omodel.OrderAction = Enumerations.Order.Modes.U.ToString();
        //                omodel.Mode = Convert.ToInt32(Enumerations.Order.Modes.U);
        //                omodel.ScripCode = MemoryManager.OrderDictionary[MessageTag].ScripCode;
        //                omodel.ScripName = MemoryManager.OrderDictionary[MessageTag].ScripName;
        //                omodel.OrderId = MemoryManager.OrderDictionary[MessageTag].OrderId;
        //                omodel.OrderNumber = MemoryManager.OrderDictionary[MessageTag].OrderNumber;
        //                omodel.MessageTag = MessageTag;
        //#endif
        //                if (!string.IsNullOrEmpty(trgPrice))
        //                {

        //                    omodel.TriggerPrice = Convert.ToInt64(Convert.ToDouble(trgPrice) * Math.Pow(10, DecimalPoint));

        //                }
        //                //Processor.Order.OrderProcessor.ProcessOrderObject(omodel);
        //                OrderRequestProcessor oOrderRequestProcessor = new OrderRequestProcessor(new ModifyOrder());
        //                oOrderRequestProcessor.ProcessRequest(omodel);
        //            }
        //        }
        #endregion


        private void PostUIValidationTWSProcessOrder(ref OrderModel oOrderModel)
        {
            try
            {
                if (oOrderModel != null)
                {
                    if (omodel.OrderType == Enumerations.Order.OrderTypes.MARKET.ToString() || omodel.OrderType == "STOPLOSSMKT")
                    {
                        if (!string.IsNullOrEmpty(MktPT))
                        {
                            var marketProtectionPercent = Convert.ToDecimal(MktPT) * 100;
                            omodel.ProtectionPercentage = Convert.ToString(marketProtectionPercent);//MarketProtection
                        }
                        else
                        {
                            //MessageBox.Show("Please Enter Market Protection");
                        }

                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void OnPreviewKeyDown(KeyEventArgs e)
        {
            try
            {
                SwiftOrderEntry swiftwin = System.Windows.Application.Current.Windows.OfType<SwiftOrderEntry>().FirstOrDefault();

                string txtqty = swiftwin.textBox3.Text; //qty textbox

                //string txtt = string.Empty;
                int maxLimit = 999999999;
                int minLimit = 0;

                if (e.Key == Key.Down || e.Key == Key.Up)
                {
                    //for qty textbox
                    if (txtqty != string.Empty)
                    {
                        int value = Convert.ToInt32(txtqty);
                        if (e.Key == Key.Down)
                        {
                            if (value >= minLimit)//>0
                            {
                                sum = value - MarketLot;
                                if (sum < 0)
                                    sum = MarketLot;
                                qty = sum.ToString();
                            }
                        }


                        else if (e.Key == Key.Up)
                        {
                            if (value <= maxLimit)
                            {
                                sum = MarketLot + value;
                                if (sum > maxLimit)
                                    sum = maxLimit;
                                qty = sum.ToString();
                            }
                        }
                    }
                    else
                    {
                        sum = MarketLot;
                        qty = sum.ToString();
                    }


                }
                //
                //        if (e.Key == Key.Down)
                //{
                //    if (txt!= string.Empty && Convert.ToInt32(txt) < maxLimit)
                //    {  
                //        sum =  Convert.ToInt32(txt) - MarketLot;
                //        if (sum < 0)
                //            sum = MarketLot;
                //     //txtt = sum.ToString();
                //     //   qty = txtt;
                //    }
                //    else 
                //    {

                //        sum =Convert.ToInt32(MarketLot);
                //        if (sum < 0)
                //            sum = MarketLot;
                //    txtt = sum.ToString();
                //        // txt = txtt;
                //        qty = txtt;
                //    }
                //}
                //else if (e.Key == Key.Up)
                //{
                //    if (txt != string.Empty)
                //    {
                //        sum = MarketLot + Convert.ToInt32(txt);
                //        if (sum < 0)
                //            sum = MarketLot;
                //        txtt = sum.ToString();
                //        qty = txtt;
                //    }
                //    else
                //    {

                //            sum = sum + Convert.ToInt32(MarketLot);
                //        if (sum < 0)
                //            sum = MarketLot;
                //        txtt = sum.ToString();
                //        qty = txtt;
                //    }
                //}
                if (e.Key == Key.Space)
                    e.Handled = true;
            }
            catch (Exception ex)
            {

                ExceptionUtility.LogError(ex);
            }

        }

        private void OnPreviewKeyDown3(KeyEventArgs e)
        {
            try
            {
                SwiftOrderEntry swiftwin = System.Windows.Application.Current.Windows.OfType<SwiftOrderEntry>().FirstOrDefault();

                string txtqty = swiftwin.textBox3.Text; //qty textbox

                //string txtt = string.Empty;
                int maxLimit = 999999999;
                int minLimit = 0;

                if (e.Key == Key.Down || e.Key == Key.Up)
                {
                    //for qty textbox
                    if (txtqty != string.Empty)
                    {
                        int value = Convert.ToInt32(txtqty);
                        if (e.Key == Key.Down)
                        {
                            if (value >= minLimit)//>0
                            {
                                sum = value - MarketLot;
                                if (sum < 0)
                                    sum = MarketLot;
                                qty = sum.ToString();
                            }
                        }


                        else if (e.Key == Key.Up)
                        {
                            if (value <= maxLimit)
                            {
                                sum = MarketLot + value;
                                if (sum > maxLimit)
                                    sum = maxLimit;
                                qty = sum.ToString();
                            }
                        }
                    }
                    else
                    {
                        sum = MarketLot;
                        qty = sum.ToString();
                    }


                }

                if (e.Key == Key.Space)
                    e.Handled = true;
            }
            catch (Exception ex)
            {

                ExceptionUtility.LogError(ex);
            }

        }

        private void OnPreviewKeyDown2(KeyEventArgs e)
        {
            try
            {
                SwiftOrderEntry swiftwin = System.Windows.Application.Current.Windows.OfType<SwiftOrderEntry>().FirstOrDefault();
                string txtrate = swiftwin.textBox4.Text;//rate textbox
                int maxLimit = 999999999;
                int minLimit = 0;
                double ticksize = Convert.ToDouble(TickSize);
                double sum = 0;
                //for rate textbox
                if (txtrate != string.Empty)
                {

                    double value = Convert.ToDouble(txtrate);
                    if (e.Key == Key.Down)
                    {
                        if (value >= minLimit)//>0
                        {
                            sum = value - ticksize;
                            if (sum <= 0)
                                sum = ticksize;
                            rate = sum.ToString();
                        }
                    }
                    else if (e.Key == Key.Up)
                    {
                        if (value <= maxLimit)
                        {
                            sum = ticksize + value;
                            if (sum > maxLimit)
                                sum = maxLimit;
                            rate = sum.ToString();
                        }
                    }

                    else if (e.Key == Key.Space)
                        e.Handled = true;
                }
                else
                {
                    if (e.Key == Key.Down || e.Key == Key.Up)
                    {
                        sum = ticksize;
                        rate = sum.ToString();
                    }
                }

            }
            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
            }
        }

        internal void PopulateDataForMW(MarketWatchModel ScripDetails)
        {
            try
            {


                ScripSelectedCode = ScripDetails.Scriptcode1;
                //  ScripSymSelected = ScripDetails.ScriptId1;

                if (Selected_EXCH == Common.Enumerations.Order.Exchanges.BSE.ToString())
                {
                    if (ScripSelectedSegment == Common.Enumerations.Order.ScripSegment.Equity.ToString())
                    {


                    }
                    if (ScripSelectedSegment == Common.Enumerations.Order.ScripSegment.Derivative.ToString())
                    {
                        if (MasterSharedMemory.objMasterDerivativeDictBaseBSE.ContainsKey(ScripSelectedCode))
                        {
                            if (MasterSharedMemory.objMasterDerivativeDictBaseBSE != null)//BSE Derivative
                            {
                                CallPutSelected = MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Cast<DerivativeMasterBase>().Where(x => x.ContractTokenNum == ScripSelectedCode).Select(x => x.OptionType).Distinct().ToString();
                                if (CallPutSelected != null)
                                {
                                    StkPriceSelected = MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Cast<DerivativeMasterBase>().Where(x => x.ContractTokenNum == ScripSelectedCode).Select(x => x.StrikePrice).Distinct().ToString();
                                    UnderAssetSelected = MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Cast<DerivativeMasterBase>().Where(x => x.ContractTokenNum == ScripSelectedCode).Select(x => x.UnderlyingAsset).Distinct().ToString();
                                    IntrTypeSelected = MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Cast<DerivativeMasterBase>().Where(x => x.ContractTokenNum == ScripSelectedCode).Select(x => x.InstrumentType).Distinct().ToString();


                                    //foreach (var item in MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Cast<DerivativeMasterBase>().Where(x => x.ContractTokenNum == ScripSelectedCode && x.OptionType == CallPutSelected).Select(x => x.UnderlyingAsset).Distinct())
                                    // {
                                    //     UnderLyingAssetLst.Add(item);
                                    // }
                                    // if (UnderLyingAssetLst.Count > 0 || UnderLyingAssetLst != null)
                                    //     UnderAssetSelected = UnderLyingAssetLst[0];






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
        }

        internal void PopulateOrderEntryForModification(OrderModel oOrderModel)
        {
            try
            {

                if (omodel != null)
                {
                    //OrderModel oOrderModel = new OrderModel();
                    //oOrderModel = oomodel;
                    int decimal_point = CommonFunctions.GetDecimal(Convert.ToInt32(oOrderModel.ScripCode), oOrderModel.Exchange, oOrderModel.Segment);
                    Selected_EXCH = oOrderModel.Exchange;
                    ScripSelectedSegment = oOrderModel.Segment;//Enum.GetName(typeof(Enumerations.Order.ScripSegment), Convert.ToInt32(lobjRecordHelper.getFieldByName(BowConstants.ROW_DATA_START, BowOrderBean.f_Market)));
                    ScripSelectedCode = oOrderModel.ScripCode;// ScripCodeLst.SingleOrDefault(x => x == Convert.ToInt64(lobjRecordHelper.getFieldByName(BowConstants.ROW_DATA_START, BowOrderBean.f_BseToken))); 
                    qty = oOrderModel.OriginalQty.ToString();//lobjRecordHelper.getFieldByName(BowConstants.ROW_DATA_START, BowOrderBean.f_Volume);
                    rate = Convert.ToString(Convert.ToDouble(oOrderModel.Price) / Math.Pow(10, decimal_point));
                    trgPrice = Convert.ToString(Convert.ToDouble(oOrderModel.TriggerPrice) / Math.Pow(10, decimal_point));  // omodel.Price.ToString();//lobjRecordHelper.getFieldByName(BowConstants.ROW_DATA_START, BowOrderBean.f_Price);
                    revQty = oOrderModel.RevealQty.ToString();//lobjRecordHelper.getFieldByName(BowConstants.ROW_DATA_START, BowOrderBean.f_DisclosedVolume);
                    clienttypeselected = oOrderModel.ClientType; //lobjRecordHelper.getFieldByName(BowConstants.ROW_DATA_START, BowOrderBean.f_IntuitionalClient);
                    intracheck = oOrderModel.Delivery;
                    OrderTypeSelected = oOrderModel.OrderType;
                    MessageTag = oOrderModel.MessageTag;
                    ScripSelectedCode = oOrderModel.ScripCode;
                    ShortClientSelected = oOrderModel.ClientId;
                    Remarks = oOrderModel.OrderRemarks;
                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
            }

        }

#if BOW
        internal void ShowValuesForModification(OrderModel oomodel)
        {
            try
            {
                if (omodel != null)
                {
                    omodel = new OrderModel();
                    omodel = oomodel;
                    int decimal_point = CommonFunctions.GetDecimal(Convert.ToInt32(omodel.ScripCode), omodel.Exchange, omodel.Segment);
                    Selected_EXCH = omodel.Exchange;
                    ScripSelectedSegment = omodel.Segment;//Enum.GetName(typeof(Enumerations.Order.ScripSegment), Convert.ToInt32(lobjRecordHelper.getFieldByName(BowConstants.ROW_DATA_START, BowOrderBean.f_Market)));
                    ScripSelectedCode = omodel.ScripCode;// ScripCodeLst.SingleOrDefault(x => x == Convert.ToInt64(lobjRecordHelper.getFieldByName(BowConstants.ROW_DATA_START, BowOrderBean.f_BseToken))); 
                    qty = omodel.Quantity.ToString();//lobjRecordHelper.getFieldByName(BowConstants.ROW_DATA_START, BowOrderBean.f_Volume);
                    rate = Convert.ToString(Convert.ToDouble(omodel.Price) / Math.Pow(10, decimal_point));
                    trgPrice = Convert.ToString(Convert.ToDouble(omodel.TriggerPrice) / Math.Pow(10, decimal_point));  // omodel.Price.ToString();//lobjRecordHelper.getFieldByName(BowConstants.ROW_DATA_START, BowOrderBean.f_Price);
                    revQty = omodel.RevealQty.ToString();//lobjRecordHelper.getFieldByName(BowConstants.ROW_DATA_START, BowOrderBean.f_DisclosedVolume);
                    clienttypeselected = omodel.ClientType; //lobjRecordHelper.getFieldByName(BowConstants.ROW_DATA_START, BowOrderBean.f_IntuitionalClient);
                    intracheck = omodel.Delivery;
                    OrderTypeSelected = omodel.OrderType;
                    SellVisible = "Hidden";
                    BuyVisible = "Hidden";
                    ModifyVisible = "Visible";
                    object e = omodel.BuySellIndicator;
                    BuySellWindow(e);
                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
            }

        }
#endif

    }
    #endregion
}

