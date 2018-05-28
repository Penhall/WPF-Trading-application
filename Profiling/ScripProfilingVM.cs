using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.IO;
using Microsoft.Win32;
using System.Threading;
using System.Text;
using System.Windows.Data;
using System.Windows.Threading;
using System.Data.SQLite;
using CommonFrontEnd.SharedMemories;
using CommonFrontEnd.Common;
using CommonFrontEnd.Model.Profiling;
using static CommonFrontEnd.Model.Profiling.ScripProfilingModel;
using static CommonFrontEnd.Common.Enumerations;
using CommonFrontEnd.Global;
using System.Globalization;
using CommonFrontEnd.ViewModel.Touchline;
using static CommonFrontEnd.SharedMemories.DataAccessLayer;
using CommonFrontEnd.View.Profiling;

namespace CommonFrontEnd.ViewModel
{
    public class ScripProfilingVM : INotifyPropertyChanged
    {
        SynchronizationContext uiContext = SynchronizationContext.Current;
        DirectoryInfo CsvFilesPath = new DirectoryInfo(Path.GetFullPath(Path.Combine(System.Environment.CurrentDirectory, @"Profile/MarketWatch/")));
        //public static Action<bool> FileSaved;
        public string DerProdSelected;
        public string CurrProdSelected;
        public string FutSelected;
        public int CalSelected;
        private int? LastData = null;
        string expDateSel = String.Empty;
        string optionTyp = String.Empty;
        char contractTyp;
        int strikePrice = 0;
        public static Action<bool> RefreshTouchLine;
        private bool flag = false;
        private bool IndicesFlag = false;
        private readonly object lockObject = new object();
        public bool isMasterCheckboxToBeUnchecked = false;
        public Dispatcher CallingThreadDispatcher { get; set; }
        public bool AllSelected_set = true;
        public bool AllSelectedForGrid2_Set = true;
        public static string str = string.Empty;
        public string query = string.Empty;
        public SQLiteDataReader oSQLiteDataReader = null;
        public DataAccessLayer oDataAccessLayer;
        public static DataAccessLayer oDataAccessLayer1;
        public delegate void OnNewProfileAddedHandler();
        public static event OnNewProfileAddedHandler OnSave;

        // EventHandler CheckBoxChecked;
        #region Properties

        private List<string> _ScripProfilesegmentLst;
        public List<string> ScripSegmentLst
        {
            get { return _ScripProfilesegmentLst; }
            set { _ScripProfilesegmentLst = value; NotifyPropertyChanged("ScripSegmentLst"); }
        }

        private List<string> _lstExchange;
        public List<string> lstExchange
        {
            get { return _lstExchange; }
            set { _lstExchange = value; NotifyPropertyChanged("lstExchange"); }
        }

        private string _SelectedExchange;
        public string SelectedExchange
        {
            get { return _SelectedExchange; }
            set
            {
                _SelectedExchange = value;
                NotifyPropertyChanged("SelectedExchange");
                OnChangeofExchnage();
            }
        }

        private string _DerivativeAsset;
        public string DerivativeAsset
        {
            get { return _DerivativeAsset; }
            set
            {
                _DerivativeAsset = value;
                NotifyPropertyChanged("DerivativeAsset");
            }
        }

        private static string _cmbProfileSelected;
        public static string cmbProfileSelected
        {
            get { return _cmbProfileSelected; }
            set
            {
                _cmbProfileSelected = value;
                NotifyStaticPropertyChanged("cmbProfileSelected");
            }
        }

        private ScripProfModel _SelectBlankRow;
        public ScripProfModel SelectBlankRow
        {
            get { return _SelectBlankRow; }
            set
            {
                _SelectBlankRow = value;
                NotifyPropertyChanged("SelectBlankRow");
            }
        }

        private string _ScripProfileselectedSegment;
        public string ScripSelectedSegment
        {
            get { return _ScripProfileselectedSegment; }
            set
            {
                _ScripProfileselectedSegment = value;
                NotifyPropertyChanged("ScripSelectedSegment");
                onChangeOfScripSegment();
            }
        }

        private string _ScripId = string.Empty;
        public string ScripId
        {
            get { return _ScripId; }
            set
            {
                _ScripId = value;
                NotifyPropertyChanged("ScripId");
                ScripIdTxtChange_Click();
            }
        }

        private string _ScripCode = string.Empty;
        public string ScripCode
        {
            get { return _ScripCode; }
            set
            {
                _ScripCode = value;
                NotifyPropertyChanged("ScripCode");
                ScripIdTxtChange_Click();
            }
        }

        private string _CurrStrikePriceSelected;
        public string CurrStrikePriceSelected
        {
            get { return _CurrStrikePriceSelected; }
            set
            {
                _CurrStrikePriceSelected = value;
                NotifyPropertyChanged("CurrStrikePriceSelected");
            }
        }

        private string _SelectedDerExpDate;
        public string SelectedDerExpDate
        {
            get { return _SelectedDerExpDate; }
            set { _SelectedDerExpDate = value; NotifyPropertyChanged("SelectedDerExpDate"); }
        }

        private string _NewSelectedDerExpDate;
        public string NewSelectedDerExpDate
        {
            get { return _NewSelectedDerExpDate; }
            set { _NewSelectedDerExpDate = value; NotifyPropertyChanged("NewSelectedDerExpDate"); }
        }

        private string _selectedScripGrp;
        public string SelectedScripGrp
        {
            get { return _selectedScripGrp; }
            set
            {
                _selectedScripGrp = value;
                NotifyPropertyChanged("SelectedScripGrp");
                // PopulatingEquityGrid();
            }
        }

        private List<int> _DerStrikePrice;
        public List<int> DerStrikePrice
        {
            get { return _DerStrikePrice; }
            set { _DerStrikePrice = value; NotifyPropertyChanged("DerStrikePrice"); }
        }

        private string _DerStrikePriceSelected;
        public string DerStrikePriceSelected
        {
            get { return _DerStrikePriceSelected; }
            set
            {
                _DerStrikePriceSelected = value;
                NotifyPropertyChanged("DerStrikePriceSelected");
                //onChangeOfScripSegment();
            }
        }

        private List<string> _DerivativeProdType;
        public List<string> DerivativePType
        {
            get { return _DerivativeProdType; }
            set { _DerivativeProdType = value; NotifyPropertyChanged("DerivativePType"); }
        }

        private string _prodTypeDerivativeSelected;
        public string ProdTypeDerivativeSelected
        {
            get { return _prodTypeDerivativeSelected; }
            set
            {
                _prodTypeDerivativeSelected = value;
                NotifyPropertyChanged("ProdTypeDerivativeSelected");
                onChangeOfDerivativeProductType();
            }
        }

        private List<string> _CurrencyProdType;
        public List<string> CurrencyPType
        {
            get { return _CurrencyProdType; }
            set { _CurrencyProdType = value; NotifyPropertyChanged("CurrencyPType"); }
        }

        private string _prodTypeCurrencySelected;
        public string ProdTypeCurrencySelected
        {
            get { return _prodTypeCurrencySelected; }
            set
            {
                _prodTypeCurrencySelected = value;
                NotifyPropertyChanged("ProdTypeCurrencySelected");
                OnChangeOfProdTypeCurrency();
            }
        }

        private string _ExpiryDate;
        public string ExpiryDate
        {
            get { return _ExpiryDate; }
            set
            {
                _ExpiryDate = value;
                NotifyPropertyChanged("ExpiryDate");
            }
        }

        private Double _StrikePrice;
        public Double StrikePrice
        {
            get { return _StrikePrice; }
            set
            {
                _StrikePrice = value;
                NotifyPropertyChanged("StrikePrice");
            }
        }

        private List<string> _CurrStrikePrice;
        public List<string> CurrStrikePrice
        {
            get { return _CurrStrikePrice; }
            set { _CurrStrikePrice = value; NotifyPropertyChanged("CurrStrikePrice"); }
        }

        private Double _strikePriceSelected;
        public Double StrikePriceSelected
        {
            get { return _strikePriceSelected; }
            set
            {
                _strikePriceSelected = value;
                NotifyPropertyChanged("StrikePriceSelected");
            }
        }
        private List<string> _OpType;
        public List<string> OptionType
        {
            get { return _OpType; }
            set { _OpType = value; NotifyPropertyChanged("OptionType"); }
        }

        private string _SelectedOpType;
        public string SelectedOptionType
        {
            get { return _SelectedOpType; }
            set
            {
                _SelectedOpType = value;
                NotifyPropertyChanged("SelectedOptionType");
                OnChangeOfOptionType();
            }
        }

        private List<string> _futType;
        public List<string> FutureType
        {
            get { return _futType; }
            set { _futType = value; NotifyPropertyChanged("FutureType"); }
        }

        private string _SelectedFType;
        public string SelectedFutType
        {
            get { return _SelectedFType; }
            set
            {
                _SelectedFType = value;
                NotifyPropertyChanged("SelectedFutType");
                OnChnageOfFutureType();
            }
        }

        private List<string> _scripSet;
        public List<string> ScripSet
        {
            get { return _scripSet; }
            set { _scripSet = value; NotifyPropertyChanged("ScripSet"); }
        }

        private string _scripsetSelected;
        public string SelectedScripSet
        {
            get { return _scripsetSelected; }
            set
            {
                _scripsetSelected = value;
                NotifyPropertyChanged("SelectedScripSet");
                OnChangeOfScripSet1();
            }
        }

        private ObservableCollection<string> _expDateLst;
        public ObservableCollection<string> ExpDateLst
        {
            get { return _expDateLst; }
            set { _expDateLst = value; NotifyPropertyChanged("ExpDateLst"); }
        }

        private ObservableCollection<int> _LocalexpDateLst;
        public ObservableCollection<int> LocalExpDateLst
        {
            get { return _LocalexpDateLst; }
            set { _LocalexpDateLst = value; NotifyPropertyChanged("LocalExpDateLst"); }
        }

        private string _SelectedCurrExpDate;
        public string SelectedCurrExpDate
        {
            get { return _SelectedCurrExpDate; }
            set { _SelectedCurrExpDate = value; NotifyPropertyChanged("SelectedCurrExpDate"); }
        }

        private ObservableCollection<string> _currexpDateLst;
        public ObservableCollection<string> CurrExpDateLst
        {
            get { return _currexpDateLst; }
            set { _currexpDateLst = value; NotifyPropertyChanged("CurrExpDateLst"); }
        }

        private List<string> _exPrd;
        public List<string> ExPrd
        {
            get { return _exPrd; }
            set { _exPrd = value; NotifyPropertyChanged("ExPrd"); }
        }

        private List<string> _striekPriceLst;
        public List<string> striekPriceLst
        {
            get { return _striekPriceLst; }
            set { _striekPriceLst = value; NotifyPropertyChanged("striekPriceLst"); }
        }

        private string _SelectedexPrd;
        public string SelectedExPrd
        {
            get { return _SelectedexPrd; }
            set
            {
                _SelectedexPrd = value;
                NotifyPropertyChanged("SelectedExPrd");
                //OnChangeOfExpPrd();
                OnChangeOfExpPrd();
            }
        }

        private List<string> _scripGrp;
        public List<string> ScripGrp
        {
            get { return _scripGrp; }
            set { _scripGrp = value; NotifyPropertyChanged("ScripGrp"); }
        }
        private ObservableCollection<string> _derAsset;
        public ObservableCollection<string> DerAsset
        {
            get { return _derAsset; }
            set { _derAsset = value; NotifyPropertyChanged("DerAsset"); }
        }

        private string _SelectedDerAsset;
        public string SelectedDerAsset
        {
            get { return _SelectedDerAsset; }
            set
            {
                _SelectedDerAsset = value; NotifyPropertyChanged("SelectedDerAsset");
                //OnChangeOfDerAsset();
            }
        }

        private ObservableCollection<string> _currAsset;
        public ObservableCollection<string> CurrAsset
        {
            get { return _currAsset; }
            set { _currAsset = value; NotifyPropertyChanged("CurrAsset"); }
        }

        private string _SelectedCurrAsset;
        public string SelectedCurrAsset
        {
            get { return _SelectedCurrAsset; }
            set
            {
                _SelectedCurrAsset = value;
                NotifyPropertyChanged("SelectedCurrAsset");
                OnChangeOfCurrencyAssets();
            }
        }

        private BindingList<string> _TempDerStrikePrice;
        public BindingList<string> TempDerStrikePrice
        {
            get { return _TempDerStrikePrice; }
            set { _TempDerStrikePrice = value; NotifyPropertyChanged("TempDerStrikePrice"); }
        }


        private string _lblCountContent;
        public string lblCountContent
        {
            get { return _lblCountContent; }
            set { _lblCountContent = value; NotifyPropertyChanged("lblCountContent"); }
        }

        private string _IsColProfileVisible;
        public string IsColProfileVisible
        {
            get { return _IsColProfileVisible; }
            set { _IsColProfileVisible = value; NotifyPropertyChanged("IsColProfileVisible"); }
        }

        private List<string> _indicesSet;
        public List<string> IndicesSet
        {
            get { return _indicesSet; }
            set { _indicesSet = value; NotifyPropertyChanged("IndicesSet"); }
        }

        private string _SelectedIndicesSet;
        public string SelectedIndicesSet
        {
            get { return _SelectedIndicesSet; }
            set
            {
                _SelectedIndicesSet = value;
                NotifyPropertyChanged("SelectedIndicesSet");
                onchangeofIndiceSet1();
            }
        }

        private string _futuretypeVisible;
        public string FuturetypeVisible
        {
            get { return _futuretypeVisible; }
            set { _futuretypeVisible = value; NotifyPropertyChanged("FuturetypeVisible"); }
        }

        private bool _Is4L6LEnabled;
        public bool Is4L6LEnabled
        {
            get { return _Is4L6LEnabled; }
            set { _Is4L6LEnabled = value; NotifyPropertyChanged("Is4L6LEnabled"); }
        }


        private bool _isCmbProfileEnable;
        public bool isCmbProfileEnable
        {
            get { return _isCmbProfileEnable; }
            set { _isCmbProfileEnable = value; NotifyPropertyChanged("isCmbProfileEnable"); }
        }

        private bool _IsSearchEnable;
        public bool IsSearchEnable
        {
            get { return _IsSearchEnable; }
            set { _IsSearchEnable = value; NotifyPropertyChanged("IsSearchEnable"); }
        }

        private bool _CurrStrikePriceEnable;
        public bool CurrStrikePriceEnable
        {
            get { return _CurrStrikePriceEnable; }
            set { _CurrStrikePriceEnable = value; NotifyPropertyChanged("CurrStrikePriceEnable"); }
        }

        private bool _currExpDateEnable;
        public bool CurrExpDateEnable
        {
            get { return _currExpDateEnable; }
            set { _currExpDateEnable = value; NotifyPropertyChanged("CurrExpDateEnable"); }
        }

        private bool _derExpDateEnable;
        public bool DerExpDateEnable
        {
            get { return _derExpDateEnable; }
            set { _derExpDateEnable = value; NotifyPropertyChanged("DerExpDateEnable"); }
        }

        private bool _exPrdEnable;
        public bool ExPrdEnable
        {
            get { return _exPrdEnable; }
            set { _exPrdEnable = value; NotifyPropertyChanged("ExPrdEnable"); }
        }

        private bool _optTypeEnable;
        public bool OptTypeEnable
        {
            get { return _optTypeEnable; }
            set { _optTypeEnable = value; NotifyPropertyChanged("OptTypeEnable"); }
        }

        private bool _IsBlankRowEnabled;
        public bool IsBlankRowEnabled
        {
            get { return _IsBlankRowEnabled; }
            set { _IsBlankRowEnabled = value; NotifyPropertyChanged("IsBlankRowEnabled"); }
        }

        private bool _enabledFutureType;
        public bool EnabledFutureType
        {
            get { return _enabledFutureType; }
            set { _enabledFutureType = value; NotifyPropertyChanged("EnabledFutureType"); }
        }

        private bool _scrpGrpEnable;
        public bool ScrpGrpEnable
        {
            get { return _scrpGrpEnable; }
            set { _scrpGrpEnable = value; NotifyPropertyChanged("ScrpGrpEnable"); }
        }

        private bool _derivativeAssetEnable;
        public bool DerivativeAssetEnable
        {
            get { return _derivativeAssetEnable; }
            set { _derivativeAssetEnable = value; NotifyPropertyChanged("DerivativeAssetEnable"); }
        }

        private bool _currencyAssetEnable;
        public bool CurrencyAssetEnable
        {
            get { return _currencyAssetEnable; }
            set { _currencyAssetEnable = value; NotifyPropertyChanged("CurrencyAssetEnable"); }
        }

        private bool _IsSaveColProfileEnabled;
        public bool IsSaveColProfileEnabled
        {
            get { return _IsSaveColProfileEnabled; }
            set { _IsSaveColProfileEnabled = value; NotifyPropertyChanged("IsSaveColProfileEnabled"); }
        }

        private ScripProfModel _AddFromGrid;
        public ScripProfModel AddFromGrid
        {
            get { return _AddFromGrid; }
            set
            {
                _AddFromGrid = value;
                NotifyPropertyChanged("AddFromGrid");
            }
        }

        private ScripProfModel _RemFromGrid;
        public ScripProfModel RemFromGrid
        {
            get { return _RemFromGrid; }
            set
            {
                _RemFromGrid = value;
                NotifyPropertyChanged("RemFromGrid");
            }
        }

        private string _optTypeLabelVisible;
        public string OptTypeLabelVisible
        {
            get { return _optTypeLabelVisible; }
            set { _optTypeLabelVisible = value; NotifyPropertyChanged("OptTypeLabelVisible"); }
        }

        private string _futTypeLabelVisible;
        public string FutTypeLabelVisible
        {
            get { return _futTypeLabelVisible; }
            set { _futTypeLabelVisible = value; NotifyPropertyChanged("FutTypeLabelVisible"); }
        }

        private string _exPrdVisible;
        public string ExPrdVisible
        {
            get { return _exPrdVisible; }
            set { _exPrdVisible = value; NotifyPropertyChanged("ExPrdVisible"); }
        }

        private string _currencyAssestVisible;
        public string CurrencyAssestVisible
        {
            get { return _currencyAssestVisible; }
            set { _currencyAssestVisible = value; NotifyPropertyChanged("CurrencyAssestVisible"); }
        }

        private string _optionTypeVisible;
        public string OptionTypeVisible
        {
            get { return _optionTypeVisible; }
            set { _optionTypeVisible = value; NotifyPropertyChanged("OptionTypeVisible"); }
        }

        private string _derivativeAssestVisible;
        public string DerivativeAssestVisible
        {
            get { return _derivativeAssestVisible; }
            set { _derivativeAssestVisible = value; NotifyPropertyChanged("DerivativeAssestVisible"); }
        }

        private bool _IsEnabledAsset;
        public bool IsEnabledAsset
        {
            get { return _IsEnabledAsset; }
            set { _IsEnabledAsset = value; NotifyPropertyChanged("IsEnabledAsset"); }
        }

        private string _IsProfileTxtVisible;
        public string IsProfileTxtVisible
        {
            get { return _IsProfileTxtVisible; }
            set { _IsProfileTxtVisible = value; NotifyPropertyChanged("IsProfileTxtVisible"); }
        }

        private static object _SelectedItem;
        public static object SelectedItem
        {
            get { return _SelectedItem; }
            set
            {
                _SelectedItem = value;
            }
        }

        private bool _MoneynessEnabled;
        public bool MoneynessEnabled
        {
            get { return _MoneynessEnabled; }
            set { _MoneynessEnabled = value; NotifyPropertyChanged("MoneynessEnabled"); }
        }

        private bool _IsEnabledDer;
        public bool IsEnabledProductTypeDerivative
        {
            get { return _IsEnabledDer; }
            set { _IsEnabledDer = value; NotifyPropertyChanged("IsEnabledProductTypeDerivative"); }
        }

        private bool _DerStrikePriceEnable;
        public bool DerStrikePriceEnable
        {
            get { return _DerStrikePriceEnable; }
            set { _DerStrikePriceEnable = value; NotifyPropertyChanged("DerStrikePriceEnable"); }
        }

        private bool _IsEnabledcurr;
        public bool IsEnabledProductTypeCurrency
        {
            get { return _IsEnabledcurr; }
            set { _IsEnabledcurr = value; NotifyPropertyChanged("IsEnabledProductTypeCurrency"); }
        }

        private bool _IsScripProfileEnable;
        public bool IsScripProfileEnable
        {
            get { return _IsScripProfileEnable; }
            set { _IsScripProfileEnable = value; NotifyPropertyChanged("IsScripProfileEnable"); }
        }

        private bool _IsCreateChecked;
        public bool IsCreateChecked
        {
            get { return _IsCreateChecked; }
            set { _IsCreateChecked = value; NotifyPropertyChanged("IsCreateChecked"); }
        }

        private bool _IsUpdateChecked;
        public bool IsUpdateChecked
        {
            get { return _IsUpdateChecked; }
            set { _IsUpdateChecked = value; NotifyPropertyChanged("IsUpdateChecked"); }
        }

        private bool _IsDeleteChecked;
        public bool IsDeleteChecked
        {
            get { return _IsDeleteChecked; }
            set { _IsDeleteChecked = value; NotifyPropertyChanged("IsDeleteChecked"); }
        }

        private bool _IsSaveAsEnabled;
        public bool IsSaveAsEnabled
        {
            get { return _IsSaveAsEnabled; }
            set { _IsSaveAsEnabled = value; NotifyPropertyChanged("IsSaveAsEnabled"); }
        }

        private bool _Is4L6LEnabledGrpBox;
        public bool Is4L6LEnabledGrpBox
        {
            get { return _Is4L6LEnabledGrpBox; }
            set { _Is4L6LEnabledGrpBox = value; NotifyPropertyChanged("Is4L6LEnabledGrpBox"); }
        }

        private bool _Is4LEnabledGrpBox;
        public bool Is4LEnabledGrpBox
        {
            get { return _Is4LEnabledGrpBox; }
            set { _Is4LEnabledGrpBox = value; NotifyPropertyChanged("Is4LEnabledGrpBox"); }
        }

        private bool _Is6LEnabledGrpBox;
        public bool Is6LEnabledGrpBox
        {
            get { return _Is6LEnabledGrpBox; }
            set { _Is6LEnabledGrpBox = value; NotifyPropertyChanged("Is6LEnabledGrpBox"); }
        }

        private bool _IsSegmentEnable;
        public bool IsSegmentEnable
        {
            get { return _IsSegmentEnable; }
            set { _IsSegmentEnable = value; NotifyPropertyChanged("IsSegmentEnable"); }
        }

        private string _Isvisibleder;
        public string IsvisibleProductTypeDerivative
        {
            get { return _Isvisibleder; }
            set { _Isvisibleder = value; NotifyPropertyChanged("IsvisibleProductTypeDerivative"); }
        }

        private string _Isvisiblecurr;
        public string IsvisibleProductTypeCurrency
        {
            get { return _Isvisiblecurr; }
            set { _Isvisiblecurr = value; NotifyPropertyChanged("IsvisibleProductTypeCurrency"); }
        }

        private string _CurrExpDateVisible;
        public string CurrExpDateVisible
        {
            get { return _CurrExpDateVisible; }
            set { _CurrExpDateVisible = value; NotifyPropertyChanged("CurrExpDateVisible"); }
        }

        private string _DerExpDateVisible;
        public string DerExpDateVisible
        {
            get { return _DerExpDateVisible; }
            set { _DerExpDateVisible = value; NotifyPropertyChanged("DerExpDateVisible"); }
        }
        private string _CurrStrikePriceVisible;
        public string CurrStrikePriceVisible
        {
            get { return _CurrStrikePriceVisible; }
            set { _CurrStrikePriceVisible = value; NotifyPropertyChanged("CurrStrikePriceVisible"); }
        }

        private string _DerStrikePriceVisible;
        public string DerStrikePriceVisible
        {
            get { return _DerStrikePriceVisible; }
            set { _DerStrikePriceVisible = value; NotifyPropertyChanged("DerStrikePriceVisible"); }
        }

        private string _IsDeselectVisible;
        public string IsDeselectVisible
        {
            get { return _IsDeselectVisible; }
            set
            {
                _IsDeselectVisible = value; NotifyPropertyChanged("IsDeselectVisible");
                // OnChangeofDeleteButton();
            }
        }

        private string _IsSelectAllVisible;
        public string IsSelectAllVisible
        {
            get { return _IsSelectAllVisible; }
            set { _IsSelectAllVisible = value; NotifyPropertyChanged("IsSelectAllVisible"); }
        }

        private string _IsProfileComboVisible;
        public string IsProfileComboVisible
        {
            get { return _IsProfileComboVisible; }
            set { _IsProfileComboVisible = value; }
        }


        private bool _IsSaveEnabled;
        public bool IsSaveEnabled
        {
            get { return _IsSaveEnabled; }
            set { _IsSaveEnabled = value; NotifyPropertyChanged("IsSaveEnabled"); }
        }

        private string _IsSaveVisible;
        public string IsSaveVisible
        {
            get { return _IsSaveVisible; }
            set { _IsSaveVisible = value; NotifyPropertyChanged("IsSaveVisible"); }
        }

        private string _IsSaveAsVisible;
        public string IsSaveAsVisible
        {
            get { return _IsSaveAsVisible; }
            set { _IsSaveAsVisible = value; NotifyPropertyChanged("IsSaveAsVisible"); }
        }


        private ObservableCollection<string> _FileCombo;

        public ObservableCollection<string> FileCombo
        {
            get { return _FileCombo; }
            set { _FileCombo = value; NotifyPropertyChanged("FileCombo"); }
        }

        private bool _AllSelectedScrips;
        public bool AllSelectedScrips
        {
            get { return _AllSelectedScrips; }
            set
            {
                _AllSelectedScrips = value;
                ObjEquityDataCollection.ToList().ForEach(x => x.IsSelected1 = value);
                NotifyPropertyChanged("AllSelectedScrips");
            }
        }

        private bool _Is4LSelected;
        public bool Is4LSelected
        {
            get { return _Is4LSelected; }
            set
            {
                _Is4LSelected = value;
                //_Is5LSelected = value;
                if (value)
                {
                    ObjEquityDataCollection = new ObservableCollection<ScripProfModel>(TempEquityDataCollection.Where(t => t.ScripCode > 400000 && t.ScripCode < 500000));
                    lblCountContent = ObjEquityDataCollection.Count + " Scrips";
                }
                else
                {
                    ObjEquityDataCollection = TempEquityDataCollection;
                    lblCountContent = ObjEquityDataCollection.Count + " Scrips";
                }

                NotifyPropertyChanged("ObjEquityDataCollection");
                NotifyPropertyChanged("Is4LSelected");
            }
        }

        private bool _Is6LChecked;
        public bool Is6LChecked
        {
            get { return _Is6LChecked; }
            set
            {
                _Is6LChecked = value;
            }
        }

        private bool _Is4LChecked;
        public bool Is4LChecked
        {
            get { return _Is4LChecked; }
            set
            {
                _Is4LChecked = value;
            }
        }

        private bool _Is6LSelected;
        public bool Is6LSelected
        {
            get { return _Is6LSelected; }
            set
            {
                _Is6LSelected = value;
                if (value)
                {
                    ObjEquityDataCollection = new ObservableCollection<ScripProfModel>(TempEquityDataCollection.Where(t => t.ScripCode > 600000 && t.ScripCode < 700000));
                    lblCountContent = ObjEquityDataCollection.Count + " Scrips";
                }
                else
                {
                    ObjEquityDataCollection = TempEquityDataCollection;
                    lblCountContent = ObjEquityDataCollection.Count + " Scrips";
                }
                NotifyPropertyChanged("ObjEquityDataCollection");
                NotifyPropertyChanged("Is6LSelected");
            }
        }

        private bool _AllSelected;
        public bool AllSelected
        {
            get { return _AllSelected; }
            set
            {
                _AllSelected = value;
                foreach (ScripProfModel oScripProfModel in ScripProfileSPRList)
                {
                    oScripProfModel.PropertyChanged -= OnElementPropertyChanged;
                }
                if (AllSelected_set)
                    ScripProfileSPRList.ToList().ForEach(x => x.IsSelected = value);
                foreach (ScripProfModel oScripProfModel in ScripProfileSPRList)
                {
                    oScripProfModel.PropertyChanged += OnElementPropertyChanged;
                }
                AllSelected_set = true;
                NotifyPropertyChanged("AllSelected");
            }
        }

        private bool _AllSelectedForGrid2;
        public bool AllSelectedForGrid2
        {
            get { return _AllSelectedForGrid2; }
            set
            {
                _AllSelectedForGrid2 = value;

                foreach (ScripProfModel oScripProfModel in selectedList)
                {
                    oScripProfModel.PropertyChanged -= OnElementPropertyChanged1;
                }
                if (AllSelectedForGrid2_Set)
                    selectedList.ToList().ForEach(x => x.IsSelected1 = value);

                foreach (ScripProfModel oScripProfModel in selectedList)
                {
                    oScripProfModel.PropertyChanged += OnElementPropertyChanged1;
                }
                AllSelectedForGrid2_Set = true;
                NotifyPropertyChanged("AllSelectedForGrid2");
            }
        }

        private string _ReplyText;
        public string ReplyText
        {
            get { return _ReplyText; }
            set
            { _ReplyText = value; NotifyPropertyChanged("ReplyText"); }
        }

        private string _SelectedScripProfileSPR;
        public string SelectedScripProfileSPR
        {
            get { return _SelectedScripProfileSPR; }
            set
            {
                _SelectedScripProfileSPR = value;
                NotifyPropertyChanged("SelectedScripProfileSPR");
            }
        }

        private string _PopulateFileNameGrid;
        public string PopulateFileNameGrid
        {
            get { return _PopulateFileNameGrid; }
            set
            { _PopulateFileNameGrid = value; NotifyPropertyChanged("PopulateFileNameGrid"); }
        }

        private string _PopulateGrid2;
        public string PopulateGrid2
        {
            get { return _PopulateGrid2; }
            set
            { _PopulateGrid2 = value; NotifyPropertyChanged("PopulateGrid2"); }
        }

        private bool _IsAddEnable;

        public bool IsAddEnable
        {
            get { return _IsAddEnable; }
            set
            {
                _IsAddEnable = value;
                NotifyPropertyChanged("IsAddEnable");
            }
        }

        #endregion

        #region Collections
        private ObservableCollection<ScripProfModel> _objEquityDataCollection = new ObservableCollection<ScripProfModel>();
        public ObservableCollection<ScripProfModel> ObjEquityDataCollection
        {
            get { return _objEquityDataCollection; }
            set
            {
                _objEquityDataCollection = value;
                NotifyPropertyChanged("ObjEquityDataCollection");
            }
        }

        private ObservableCollection<ColumnProfilingModel> _SaveColProfileCollection = new ObservableCollection<ColumnProfilingModel>();
        public ObservableCollection<ColumnProfilingModel> SaveColProfileCollection
        {
            get { return _SaveColProfileCollection; }
            set
            {
                _SaveColProfileCollection = value;
                NotifyPropertyChanged("SaveColProfileCollection");
            }
        }

        private static ObservableCollection<string> _cmbProfileName = new ObservableCollection<string>();
        public static ObservableCollection<string> cmbProfileName
        {
            get { return _cmbProfileName; }
            set
            {
                _cmbProfileName = value;
                NotifyStaticPropertyChanged("cmbProfileName");
            }
        }

        private ObservableCollection<ScripProfModel> _tempEquityDataCollection;
        public ObservableCollection<ScripProfModel> TempEquityDataCollection
        {
            get { return _tempEquityDataCollection; }
            set
            {
                _tempEquityDataCollection = value;
            }
        }

        private static ObservableCollection<ScripProfModel> _DemoDataCollection;
        public static ObservableCollection<ScripProfModel> DemoDataCollection
        {
            get { return _DemoDataCollection; }
            set
            {
                _DemoDataCollection = value;
            }
        }
        private ObservableCollection<ScripMasterDebtInfo> _objDebtDataCollection;
        public ObservableCollection<ScripMasterDebtInfo> ObjDebtDataCollection
        {
            get { return _objDebtDataCollection; }
            set
            {
                _objDebtDataCollection = value;
            }
        }
        //TODO TBD2017
        private ObservableCollection<ScripProfModel> _objDerivativeDataCollection;
        public ObservableCollection<ScripProfModel> ObjDerivativeDataCollection
        {
            get { return _objDerivativeDataCollection; }
            set
            {
                _objDerivativeDataCollection = value;
            }
        }
        //TODO TBD2017
        private ObservableCollection<ScripProfModel> _objCurrencyDataCollection;
        public ObservableCollection<ScripProfModel> ObjCurrencyDataCollection
        {
            get { return _objCurrencyDataCollection; }
            set
            {
                _objCurrencyDataCollection = value;
            }
        }
        private ObservableCollection<ScripProfModel> _selectedList = new ObservableCollection<ScripProfModel>();
        public ObservableCollection<ScripProfModel> selectedList
        {
            get { return _selectedList; }
            set
            {
                _selectedList = value;
            }
        }

        private ObservableCollection<ScripProfModel> _tempSelectedList;
        public ObservableCollection<ScripProfModel> tempSelectedList
        {
            get { return _tempSelectedList; }
            set
            {
                _tempSelectedList = value;
            }
        }
        //TODO TBD2017
        private Dictionary<int, ScripProfModel> _currencyScripList;
        public Dictionary<int, ScripProfModel> currencyScripList
        {
            get { return _currencyScripList; }
            set { _currencyScripList = value; }
        }
        private ObservableCollection<ScripProfModel> _ScripProfileSPRList;
        public ObservableCollection<ScripProfModel> ScripProfileSPRList
        {
            get { return _ScripProfileSPRList; }
            set
            { _ScripProfileSPRList = value; NotifyPropertyChanged("ScripProfileSPRList"); }
        }

        private ObservableCollection<ScripMaster> _objSPRDataCollection;
        public ObservableCollection<ScripMaster> ObjSPRDataCollection
        {
            get { return _objSPRDataCollection; }
            set
            {
                _objSPRDataCollection = value;
            }
        }


        private static ObservableCollection<ScripColProfileMapping> _ScripColMapping;
        public static ObservableCollection<ScripColProfileMapping> ScripColMapping
        {
            get { return _ScripColMapping; }
            set
            {
                _ScripColMapping = value;
            }
        }
        #endregion

        # region RelayCommands
        private RelayCommand _SearchButtonClick;
        public RelayCommand SearchButtonClick
        {
            get
            {

                return _SearchButtonClick ?? (_SearchButtonClick = new RelayCommand(
                    (object e) => PopulatingEquity1Grid()
                        ));
            }
        }

        private RelayCommand _DefualtButtonClick;
        public RelayCommand DefualtButtonClick
        {
            get
            {

                return _DefualtButtonClick ?? (_DefualtButtonClick = new RelayCommand(
                    (object e) => ResetValues()
                        ));
            }
        }


        private RelayCommand _selectGridridRow;
        public RelayCommand SelectGridridRow
        {
            get
            {
                return _selectGridridRow ?? (_selectGridridRow = new RelayCommand(
                    (object e1) => PopulatingEquityGrid2(e1)));
            }
        }


        private RelayCommand _selectAllGridridRow;
        public RelayCommand selectAllGridridRow
        {
            get
            {
                return _selectAllGridridRow ?? (_selectAllGridridRow = new RelayCommand(
                    (object e1) => PopulatingEquityGrid3(e1)));
            }
        }

        private RelayCommand _DeselectAllGridridRow;
        public RelayCommand DeselectAllGridridRow
        {
            get
            {
                return _DeselectAllGridridRow ?? (_DeselectAllGridridRow = new RelayCommand(
                    (object e1) => PopulatingEquityGrid4(e1)));
            }
        }

        private RelayCommand _SaveSprFile;
        public RelayCommand SaveSprFile
        {
            get
            {
                return _SaveSprFile ?? (_SaveSprFile = new RelayCommand(
                    (object e1) => SaveIntoSprFile(e1)));
            }
        }

        private RelayCommand _CreateCSVFile;
        public RelayCommand CreateCSVFile
        {
            get
            {
                return _CreateCSVFile ?? (_CreateCSVFile = new RelayCommand(
                    (object e1) => CreateCSVIntoFile()));
            }
        }

        private RelayCommand _RemoveFromGrid;
        public RelayCommand RemoveFromGrid
        {
            get
            {
                return _RemoveFromGrid ?? (_RemoveFromGrid = new RelayCommand(
                    (object e1) => RemovingFromGrid(e1)));
            }
        }

        private RelayCommand _RUpdateCommand;
        public RelayCommand RUpdateCommand
        {
            get
            {
                return _RUpdateCommand ?? (_RUpdateCommand = new RelayCommand(
                    (object e1) => PopulatingUpdateButtonGrid()));
            }
        }


        private RelayCommand _CheckColumns;
        public RelayCommand CheckColumns
        {
            get
            {
                return _CheckColumns ?? (_CheckColumns = new RelayCommand(
                    (object e1) => CheckAllColumns(e1)));
            }
        }

        private RelayCommand _RDeleteCommand;
        public RelayCommand RDeleteCommand
        {
            get
            {
                return _RDeleteCommand ?? (_RDeleteCommand = new RelayCommand(
                    (object e1) => PopulatingDeleteButtonGrid(e1)));
            }
        }

        private RelayCommand _AddBlankRow;
        public RelayCommand AddBlankRow
        {
            get
            {
                return _AddBlankRow ?? (_AddBlankRow = new RelayCommand(
                    (object e1) => AditionOfBlankRow(e1)));
            }
        }

        private RelayCommand _CheckAllFile;
        public RelayCommand CheckAllFile
        {
            get
            {
                return _CheckAllFile ?? (_CheckAllFile = new RelayCommand(
                    (object e1) => SelctionOfAllDataGridRow(e1)));
            }
        }

        private RelayCommand _FileNameComboBoxChanged;
        public RelayCommand FileNameComboBoxChanged
        {
            get
            {
                return _FileNameComboBoxChanged ?? (_FileNameComboBoxChanged = new RelayCommand(
                    (object e1) => OnChangeOfSelectedScripProfileSPR(e1)));
            }
        }

        private RelayCommand _DerAssetComboBoxChanged;
        public RelayCommand DerAssetComboBoxChanged
        {
            get
            {
                return _DerAssetComboBoxChanged ?? (_DerAssetComboBoxChanged = new RelayCommand(
                    (object e1) => OnChangeOfDerSelectedAssest()));
            }
        }

        private RelayCommand _ClearFilterButton;
        public RelayCommand ClearFilterButton
        {
            get
            {
                return _ClearFilterButton ?? (_ClearFilterButton = new RelayCommand((object e) => ClearFilterButton_Click()));
            }
        }

        private RelayCommand _btnSaveColumnProfile;
        public RelayCommand btnSaveColumnProfile
        {
            get
            {
                return _btnSaveColumnProfile ?? (_btnSaveColumnProfile = new RelayCommand((object e) => SaveColumnProfileButton_Click()));
            }
        }

        #endregion


        private void SaveColumnProfileButton_Click()
        {
# if TWS
            if (SelectedScripProfileSPR != WindowName.Exchange_Default_Profile.ToString().Replace("_", " ") || SelectedScripProfileSPR == "Select")
            {
                SaveColProfileCollection.Clear();
                LoadMapping();
                try
                {
                    ColumnProfilingModel cpmodel = new ColumnProfilingModel();
                    cpmodel.MemberID = UtilityLoginDetails.GETInstance.MemberId.ToString();
                    cpmodel.TraderID = UtilityLoginDetails.GETInstance.TraderId.ToString();
                    cpmodel.FileName = SelectedScripProfileSPR;
                    cpmodel.ColProfile = cmbProfileSelected;
                    SaveColProfileCollection.Add(cpmodel);

                    if (cmbProfileSelected == WindowName.Exchange_Default_Profile.ToString().Replace("_", " ") || cmbProfileSelected == "DEFAULT" || cmbProfileSelected == "Default")
                    {
                        MessageBox.Show("Select another Profile Name", "Incorrect Selection", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                    if (SaveColProfileCollection.Count > 0)
                    {
                        if (ScripColMapping.Count > 0)
                        {
                            if (ScripColMapping.Any(p => p.MarketWatch == SelectedScripProfileSPR))
                            {
                                int res = UpdateExistingScripColProfile();
                                if (res > 0)
                                {
                                    MessageBox.Show("Column Profile Saved Successfully", "Save Profile", MessageBoxButton.OK, MessageBoxImage.Information);
                                    return;
                                }
                            }
                            else
                            {
                                UpdateScripColumnProfileTable(cpmodel);
                                MessageBox.Show("Column Profile Saved Successfully", "Save Profile", MessageBoxButton.OK, MessageBoxImage.Information);

                                return;
                            }
                        }
                        else
                        {
                            UpdateScripColumnProfileTable(cpmodel);
                            MessageBox.Show("Column Profile Saved Successfully", "Save Profile", MessageBoxButton.OK, MessageBoxImage.Information);

                            return;
                        }
                    }
                }
                catch (Exception ex)
                {
                    return;
                }
            }

            else if (SelectedScripProfileSPR == WindowName.Exchange_Default_Profile.ToString().Replace("_", " ") || cmbProfileSelected == WindowName.Exchange_Default_Profile.ToString().Replace("_", " ") || SelectedScripProfileSPR == "Select")
            {
                MessageBox.Show("Select FileName or Profile Name", "Incorrect Selection", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
#elif  BOW
#endif
        }

        private int UpdateExistingScripColProfile()
        {
            int result = 0;
            try
            {
                oDataAccessLayer.Connect((int)DataAccessLayer.ConnectionDB.Masters);
                oDataAccessLayer.OpenConnection((int)DataAccessLayer.ConnectionDB.Masters);

                str = @"UPDATE SCRIP_COL_PROFILE_MAPPING
                    SET MemberID = '" + UtilityLoginDetails.GETInstance.MemberId + "',TraderID = '" + UtilityLoginDetails.GETInstance.TraderId + "',MarketWatch = '" + SelectedScripProfileSPR + "',ProfileName = '" + cmbProfileSelected
                            + "' WHERE MemberID = '" + UtilityLoginDetails.GETInstance.MemberId + "' AND TraderID = '" + UtilityLoginDetails.GETInstance.TraderId + "' AND MarketWatch = '" + SelectedScripProfileSPR + "';";
                result = oDataAccessLayer.ExecuteNonQuery((int)ConnectionDB.Masters,str, System.Data.CommandType.Text, null);

            }
            catch (Exception e)
            {
            }
            finally
            {
               oDataAccessLayer.CloseConnection((int)DataAccessLayer.ConnectionDB.Masters);
                System.Data.SQLite.SQLiteConnection.ClearAllPools();
            }

            return result;
        }

        public static void LoadMapping()
        {
            try
            {
                ScripColMapping = new ObservableCollection<ScripColProfileMapping>();
                oDataAccessLayer1 = new DataAccessLayer();
                oDataAccessLayer1.Connect((int)DataAccessLayer.ConnectionDB.Masters);
                oDataAccessLayer1.OpenConnection((int)DataAccessLayer.ConnectionDB.Masters);
                //MasterSharedMemory.oDataAccessLayer.Connect((int)DataAccessLayer.ConnectionDB.Masters);
                //MasterSharedMemory.oDataAccessLayer.OpenConnection((int)DataAccessLayer.ConnectionDB.Masters);
                str = @"SELECT MemberID,TraderID,MarketWatch,ProfileName FROM SCRIP_COL_PROFILE_MAPPING WHERE MemberID='" + UtilityLoginDetails.GETInstance.MemberId + "' AND TraderID='" + UtilityLoginDetails.GETInstance.TraderId + "';";

                SQLiteDataReader oSQLiteDataReader = oDataAccessLayer1.ExecuteDataReader((int)ConnectionDB.Masters,str, System.Data.CommandType.Text, null);
                while (oSQLiteDataReader.Read())
                {

                    ScripColProfileMapping objScripColProfileMapping = new ScripColProfileMapping();
                    if (oSQLiteDataReader["MemberID"] != string.Empty)
                        objScripColProfileMapping.MemberID = oSQLiteDataReader["MemberID"]?.ToString().Trim();

                    if (oSQLiteDataReader["TraderID"] != string.Empty)
                        objScripColProfileMapping.TraderID = oSQLiteDataReader["TraderID"]?.ToString().Trim();

                    if (oSQLiteDataReader["MarketWatch"] != string.Empty)
                        objScripColProfileMapping.MarketWatch = oSQLiteDataReader["MarketWatch"]?.ToString().Trim();

                    if (oSQLiteDataReader["ProfileName"] != string.Empty)
                        objScripColProfileMapping.ProfileName = oSQLiteDataReader["ProfileName"]?.ToString().Trim();

                    ScripColMapping.Add(objScripColProfileMapping);

                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error while reading Mappping File");
                return;
            }
            finally
            {
               // MasterSharedMemory.oDataAccessLayer.CloseConnection((int)DataAccessLayer.ConnectionDB.Masters);
                oDataAccessLayer1.CloseConnection((int)DataAccessLayer.ConnectionDB.Masters);
            }
        }

        private void UpdateScripColumnProfileTable(ColumnProfilingModel cpmodel)
        {
            try
            {
                oDataAccessLayer.Connect((int)DataAccessLayer.ConnectionDB.Masters);
                oDataAccessLayer.OpenConnection((int)DataAccessLayer.ConnectionDB.Masters);

                str = @"INSERT INTO SCRIP_COL_PROFILE_MAPPING (MemberID,TraderID,MarketWatch,ProfileName)
                                      VALUES ( " + "'" + cpmodel.MemberID + "'," + "'" + cpmodel.TraderID + "'," +
                                          "'" + cpmodel.FileName + "'," + "'" + cpmodel.ColProfile + "');";
               oDataAccessLayer.ExecuteNonQuery((int)ConnectionDB.Masters,str, System.Data.CommandType.Text, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error While Saving Data", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            finally
            {
                oDataAccessLayer.CloseConnection((int)DataAccessLayer.ConnectionDB.Masters);
                System.Data.SQLite.SQLiteConnection.ClearAllPools();
            }
        }

        private void OnChangeofExchnage()
        {
            PopulateSegmentDropDown();
            PopulateProductType();
            PopulateAsset();
        }

        private void PopulateStrikePrice()
        {

            if (SelectedExchange == ScripProfilingModel.Exchanges.MCX.ToString())
            {
                try
                {
                    oDataAccessLayer.Connect((int)DataAccessLayer.ConnectionDB.Masters);
                    oDataAccessLayer.OpenConnection((int)DataAccessLayer.ConnectionDB.Masters);
                    TempDerStrikePrice = new BindingList<string>();
                    TempDerStrikePrice.Add(ScripProfilingModel.FutType._All.ToString().Replace("_", " "));

                    str = @"SELECT distinct(MCStrikePrice) FROM MCXContracts;";
                    SQLiteDataReader oSQLiteDataReader = oDataAccessLayer.ExecuteDataReader((int)ConnectionDB.Masters,str, System.Data.CommandType.Text, null);
                    while (oSQLiteDataReader.Read())
                    {
                        if (oSQLiteDataReader["MCStrikePrice"] != string.Empty)
                            TempDerStrikePrice.Add(oSQLiteDataReader["MCStrikePrice"]?.ToString().Trim());
                    }
                    DerStrikePriceSelected = TempDerStrikePrice[0];
                    SelectedDerAsset = DerAsset[0];
                    TempDerStrikePrice.Remove(string.Empty);
                }
                catch (Exception e)
                {
                }
                finally
                {
                  oDataAccessLayer.CloseConnection((int)DataAccessLayer.ConnectionDB.Masters);
                    System.Data.SQLite.SQLiteConnection.ClearAllPools();
                }
            }
        }

        private void PopulateAsset()
        {
            if (SelectedExchange == ScripProfilingModel.Exchanges.MCX.ToString())
            {

                try
                {
                    oDataAccessLayer.Connect((int)DataAccessLayer.ConnectionDB.Masters);
                    oDataAccessLayer.OpenConnection((int)DataAccessLayer.ConnectionDB.Masters);
                    DerAsset = new ObservableCollection<string>();
                    DerAsset.Add(ScripProfilingModel.CurrencyProdType.All.ToString());

                    str = @"SELECT distinct(MCContractCode) FROM MCXContracts;";
                    SQLiteDataReader oSQLiteDataReader = oDataAccessLayer.ExecuteDataReader((int)ConnectionDB.Masters,str, System.Data.CommandType.Text, null);
                    while (oSQLiteDataReader.Read())
                    {
                        if (oSQLiteDataReader["MCContractCode"] != string.Empty)
                            DerAsset.Add(oSQLiteDataReader["MCContractCode"]?.ToString().Trim());
                    }
                    SelectedDerAsset = DerAsset[0];
                    DerAsset.Remove(string.Empty);
                }
                catch (Exception ex)
                {
                    ExceptionUtility.LogError(ex);
                }
                finally
                {
                   oDataAccessLayer.CloseConnection((int)DataAccessLayer.ConnectionDB.Masters);
                    System.Data.SQLite.SQLiteConnection.ClearAllPools();
                }
            }
        }

        private void PopulateProductType()
        {

            if (SelectedExchange == ScripProfilingModel.Exchanges.MCX.ToString())
            {
                try
                {
                    oDataAccessLayer.Connect((int)DataAccessLayer.ConnectionDB.Masters);
                   oDataAccessLayer.OpenConnection((int)DataAccessLayer.ConnectionDB.Masters);
                    DerivativePType = new List<string>();
                    DerivativePType.Add(ScripProfilingModel.FutType._All.ToString().Replace("_", " "));
                    str = @"SELECT distinct(MCInstrumentName) FROM MCXContracts;";
                    SQLiteDataReader oSQLiteDataReader = oDataAccessLayer.ExecuteDataReader((int)ConnectionDB.Masters,str, System.Data.CommandType.Text, null);
                    while (oSQLiteDataReader.Read())
                    {
                        if (oSQLiteDataReader["MCInstrumentName"] != string.Empty)
                            DerivativePType.Add(oSQLiteDataReader["MCInstrumentName"]?.ToString().Trim());
                    }
                    ProdTypeDerivativeSelected = DerivativePType[0];
                    DerivativePType.Sort();
                    DerivativePType.Remove(string.Empty);
                }

                catch (Exception ex)
                {
                    ExceptionUtility.LogError(ex);
                }
                finally
                {
                    oDataAccessLayer.CloseConnection((int)DataAccessLayer.ConnectionDB.Masters);
                    System.Data.SQLite.SQLiteConnection.ClearAllPools();
                }
            }
        }

        private void ClearFilterButton_Click()
        {
            if (ScripId != string.Empty || ScripCode != string.Empty)
            {
                ScripId = string.Empty;
                ScripCode = string.Empty;
            }
        }

        public static void CheckBox_UnCheckedSelectAll(object sender, RoutedEventArgs e)
        {
            CheckBox chk = sender as CheckBox;
            chk.IsChecked = false;
        }

        private void ScripIdTxtChange_Click()
        {
            try
            {
                // Collection which will take your ObservableCollection
                var _itemSourceList = new CollectionViewSource() { Source = TempEquityDataCollection.OrderBy(x => x.ScripId) };

                // ICollectionView the View/UI part 
                ICollectionView Itemlist = _itemSourceList.View;
                //   executeFilterAction(new Action(() =>
                //   {
                if (!string.IsNullOrEmpty(ScripId.Trim()) && !string.IsNullOrEmpty(ScripCode.Trim()))
                {
                    var yourCostumFilter = new Predicate<object>(item => ((ScripProfModel)item).ScripId.Trim().ToLower().StartsWith(ScripId.Trim().ToLower()) && ((ScripProfModel)item).ScripCode.ToString().StartsWith(ScripCode.Trim()));
                    Itemlist.Filter = yourCostumFilter;
                }
                else if (string.IsNullOrEmpty(ScripCode.Trim()))
                {
                    var yourCostumFilter = new Predicate<object>(item => ((ScripProfModel)item).ScripId.Trim().ToLower().StartsWith(ScripId.Trim().ToLower()));
                    Itemlist.Filter = yourCostumFilter;
                }
                else if (string.IsNullOrEmpty(ScripId.Trim()))
                {
                    var yourCostumFilter = new Predicate<object>(item => ((ScripProfModel)item).ScripCode.ToString().StartsWith(ScripCode.Trim()));
                    Itemlist.Filter = yourCostumFilter;
                }
                //now we add our Filter

                var l = Itemlist.Cast<ScripProfModel>().ToList();

                ObjEquityDataCollection = new ObservableCollection<ScripProfModel>(l);
                //l.Clear();
            }
            catch (Exception)
            {
                return;
            }
            finally
            {
                if (ObjEquityDataCollection.Count == TempEquityDataCollection.Count && string.IsNullOrEmpty(ScripId) && string.IsNullOrEmpty(ScripCode))
                    lblCountContent = string.Format("{0}", ObjEquityDataCollection.Count);
                else
                    lblCountContent = string.Format("{0} of {1}", ObjEquityDataCollection.Count, TempEquityDataCollection.Count);
            }
        }

        private void executeFilterAction(Action action)
        {
            BackgroundWorker worker = new BackgroundWorker();

            worker.DoWork += delegate (object sender, DoWorkEventArgs e)
            {
                lock (lockObject)
                {
                    if (this.CallingThreadDispatcher != null && !this.CallingThreadDispatcher.CheckAccess())
                    {
                        this.CallingThreadDispatcher.Invoke
                            (
                                new Action(() =>
                                {
                                    action.Invoke();
                                })
                            );
                    }
                    else
                    {
                        action.Invoke();
                    }
                }
            };

            worker.RunWorkerCompleted += delegate (object sender, RunWorkerCompletedEventArgs e)
            {
                if (e.Error != null)
                {
                    //FilteringError?.Invoke(sender, e);
                }
            };

            worker.RunWorkerAsync();
        }
        void OnChecked(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OnChangeOfDerSelectedAssest()
        {
#if (TWS)
            #region TWS
            {
                List<int> DerStrikePrice = new List<int>();
                if (SelectedDerAsset == " All")
                {
                    ExpDateLst.Clear();
                    ExpDateLst.Add(ScripProfilingModel.FutType._All.ToString().Replace("_", " "));
                    foreach (var item in MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Where(x => x.InstrumentType == "IF" || x.InstrumentType == "IO" && x.OptionType == " " && ((DateTime.Parse(x.ExpiryDate) >= DateTime.Today))).OrderBy(x => x.ExpiryDate).Select(x => (DateTime.ParseExact(x.ExpiryDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture).ToString("yy-MM-dd"))).Distinct())
                    {
                        ExpDateLst.Add(item);
                    }

                    ExpDateLst = new ObservableCollection<string>(ExpDateLst.OrderBy(p => p));
                    var sortedDatelst = DisplayDateInComboBox(ExpDateLst);
                    ExpDateLst.Clear();
                    ExpDateLst = sortedDatelst;

                    if (ExpDateLst != null)
                        SelectedDerExpDate = ExpDateLst[0];

                    TempDerStrikePrice.Clear();
                    TempDerStrikePrice.Add(ScripProfilingModel.FutType._All.ToString().Replace("_", " "));

                    foreach (var item in MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Where(x => (x.InstrumentType == "SO" || x.InstrumentType == "IO") && (x.OptionType == "CE" || x.OptionType == "PE")).Select(x => x.StrikePrice).Distinct())
                    {
                        TempDerStrikePrice.Add(Convert.ToString(Math.Truncate(Convert.ToDecimal(CommonFunctions.DisplayInDecimalFormatTouch(item, 2)))));
                    }

                    //TempDerStrikePrice.sort();
                    DerStrikePriceSelected = " All";
                    DerStrikePriceEnable = false;
                }
                else if (SelectedDerAsset != null)
                {
                    ExpDateLst.Clear();
                    ExpDateLst.Add(ScripProfilingModel.FutType._All.ToString().Replace("_", " "));
                    ObservableCollection<string> lst = new ObservableCollection<string>();
                    List<int> reversed = new List<int>();
                    //foreach (var item in MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Where(x => x.UnderlyingAsset == SelectedDerAsset).OrderBy(x => x.ExpiryDate).Select(x => (DateTime.ParseExact(x.ExpiryDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture).ToString("dd-MM-yy"))).Distinct())
                    foreach (var item in MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Where(x => x.UnderlyingAsset == SelectedDerAsset).OrderBy(x => x.ExpiryDate).Select(x => (DateTime.ParseExact(x.ExpiryDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture).ToString("yy-MM-dd"))).Distinct())
                    {
                        ExpDateLst.Add(item);
                    }
                    ExpDateLst = new ObservableCollection<string>(ExpDateLst.OrderBy(p => p));
                    var sortedDatelst = DisplayDateInComboBox(ExpDateLst);
                    ExpDateLst.Clear();
                    ExpDateLst = sortedDatelst;
                    //#region Date Sorting
                    //for (int i = 1; i <= ExpDateLst.Count; i++)
                    //{
                    //    for (int j = i + 1; j < ExpDateLst.Count; j++)
                    //    {
                    //        int res = CommonFunctions.CompareDateddMMYY(ExpDateLst[i], ExpDateLst[j]);
                    //        if (res == 1 || res == 2|| res == 5||res==0)
                    //        {
                    //            string temp = ExpDateLst[j];
                    //            ExpDateLst[j] = ExpDateLst[i];
                    //            ExpDateLst[i] = temp;
                    //        }
                    //    }
                    //}
                    //#endregion

                    if (ExpDateLst != null)
                        SelectedDerExpDate = ExpDateLst[0];

                    TempDerStrikePrice.Clear();
                    TempDerStrikePrice.Add(ScripProfilingModel.FutType._All.ToString().Replace("_", " "));

                    foreach (var item in MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Where(x => x.UnderlyingAsset == SelectedDerAsset && (x.InstrumentType == "IO" || x.InstrumentType == "SO") && (x.OptionType == "PE" || x.OptionType == "CE")).Select(x => x.StrikePrice).Distinct())
                    {
                        TempDerStrikePrice.Add(Convert.ToString(Math.Truncate(Convert.ToDecimal(CommonFunctions.DisplayInDecimalFormatTouch(item, 2)))));
                    }
                    //TempDerStrikePrice.OrderBy(x=>x.);
                    DerStrikePriceSelected = TempDerStrikePrice[0];

                    if (ProdTypeDerivativeSelected == ScripProfilingModel.DerivativeProdType.Index_Option.ToString().Replace("_", " ") && SelectedExPrd != " All" || (ProdTypeDerivativeSelected == ScripProfilingModel.DerivativeProdType.Stock_Option.ToString().Replace("_", " ") && SelectedExPrd != " All"))
                    {
                        DerStrikePriceEnable = true;
                    }
                    else if (SelectedExPrd == " All")
                    {
                        DerStrikePriceEnable = false;
                    }
                }

            }
            #endregion
#elif BOW
            MasterSharedMemory.oDataAccessLayer.OpenConnection((int)DataAccessLayer.ConnectionDB.Masters);
            if (SelectedDerAsset == "All")
            {
            #region MCX Expiry Date
                ExpDateLst = new ObservableCollection<string>();
                ExpDateLst.Clear();
                ExpDateLst.Add(ScripProfilingModel.ExpPrd.All.ToString());

                LocalExpDateLst = new ObservableCollection<string>();
                str = @"SELECT distinct(MCDisplayExpiryDate) FROM MCXContracts;";
                oSQLiteDataReader = MasterSharedMemory.oDataAccessLayer.ExecuteDataReader((int)ConnectionDB.Masters,str, System.Data.CommandType.Text, null);
                while (oSQLiteDataReader.Read())
                {
                    if (oSQLiteDataReader["MCDisplayExpiryDate"] != string.Empty)
                        LocalExpDateLst.Add(oSQLiteDataReader["MCDisplayExpiryDate"]?.ToString().Trim());
                }
                LocalExpDateLst.Remove(string.Empty);

                foreach (var item in LocalExpDateLst)
                {
                    string date = DateTime.Parse(item).ToString("dd/MM/yy");
                    ExpDateLst.Add(date);
                }
                if (ExpDateLst != null)
                    SelectedDerExpDate = ExpDateLst[0];
                int negativePrice = 0;
                foreach (var item in ExpDateLst.Distinct())
                {
                    if (item.Equals(-1))
                        negativePrice++;
                }
                if (negativePrice == 1)
                {
                    DerStrikePriceEnable = false;
                    ExpDateLst.Add(ScripProfilingModel.ScripSet._Select.ToString().Replace("_", " "));
                    SelectedDerExpDate = ScripProfilingModel.ScripSet._Select.ToString().Replace("_", " ");
                }
            #endregion

            #region MCX Strike Price
                TempDerStrikePrice = new BindingList<String>();
                TempDerStrikePrice.Add(ScripProfilingModel.ExpPrd.All.ToString());

                str = "SELECT distinct(MCStrikePrice) FROM MCXContracts;";
                oSQLiteDataReader = MasterSharedMemory.oDataAccessLayer.ExecuteDataReader((int)ConnectionDB.Masters,str, System.Data.CommandType.Text, null);
                while (oSQLiteDataReader.Read())
                {
                    if (oSQLiteDataReader["MCStrikePrice"] != string.Empty)
                        TempDerStrikePrice.Add(oSQLiteDataReader["MCStrikePrice"]?.ToString().Trim());
                }
                TempDerStrikePrice.Remove(string.Empty);
                DerStrikePriceSelected = TempDerStrikePrice[0];
            #endregion

            }
            else if (SelectedDerAsset != "All" && SelectedDerAsset != null)
            {
            #region  MCX Expiry Date
                ExpDateLst = new ObservableCollection<string>();
                ExpDateLst.Clear();
                ExpDateLst.Add(ScripProfilingModel.ExpPrd.All.ToString());

                LocalExpDateLst = new ObservableCollection<string>();
                str = "SELECT distinct(MCDisplayExpiryDate) FROM MCXContracts where MCContractCode=" + "'" + SelectedDerAsset + "';";
                oSQLiteDataReader = MasterSharedMemory.oDataAccessLayer.ExecuteDataReader((int)ConnectionDB.Masters,str, System.Data.CommandType.Text, null);
                while (oSQLiteDataReader.Read())
                {
                    if (oSQLiteDataReader["MCDisplayExpiryDate"] != string.Empty)
                        LocalExpDateLst.Add(oSQLiteDataReader["MCDisplayExpiryDate"]?.ToString().Trim());
                }
                LocalExpDateLst.Remove(string.Empty);

                foreach (var item in LocalExpDateLst)
                {
                    string date = DateTime.Parse(item).ToString("dd/MM/yy");
                    ExpDateLst.Add(date);
                }
                if (ExpDateLst != null)
                    SelectedDerExpDate = ExpDateLst[0];
            #endregion

            #region  MCX StrikePrice
                TempDerStrikePrice = new BindingList<String>();
                TempDerStrikePrice.Clear();
                TempDerStrikePrice.Add(ScripProfilingModel.ExpPrd.All.ToString());

                str = "SELECT distinct(MCStrikePrice) FROM MCXContracts where MCContractCode=" + "'" + SelectedDerAsset + "';";
                oSQLiteDataReader = MasterSharedMemory.oDataAccessLayer.ExecuteDataReader((int)ConnectionDB.Masters,str, System.Data.CommandType.Text, null);
                while (oSQLiteDataReader.Read())
                {
                    if (oSQLiteDataReader["MCStrikePrice"] != string.Empty)
                        TempDerStrikePrice.Add(oSQLiteDataReader["MCStrikePrice"]?.ToString().Trim());
                }
                TempDerStrikePrice.Remove(string.Empty);
                DerStrikePriceSelected = TempDerStrikePrice[0];

                int negativePrice = 0;
                foreach (var item in TempDerStrikePrice.Distinct())
                {
                    if (item.Equals("-1"))
                        negativePrice++;
                }
                if (negativePrice == 1)
                {
                    DerStrikePriceEnable = false;
                    TempDerStrikePrice.Add(ScripProfilingModel.ScripSet._Select.ToString().Replace("_", " "));
                    DerStrikePriceSelected = ScripProfilingModel.ScripSet._Select.ToString().Replace("_", " ");
                }
            #endregion
            }
#endif
        }

        private ObservableCollection<string> DisplayDateInComboBox(ObservableCollection<string> ExpDate)
        {
            ObservableCollection<string> sortedList = new ObservableCollection<string>();
            foreach (var item in ExpDate)//.Select(x => (DateTime.ParseExact(item., "dd-MMM-yyyy", CultureInfo.InvariantCulture).ToString("yy-MM-dd"))).Distinct())
                if (!(item == " All" || item == "All"))
                    sortedList.Add(DateTime.ParseExact(item, "yy-MM-dd", CultureInfo.InvariantCulture).ToString("dd-MM-yy"));
                else
                    sortedList.Add(item);
            return sortedList;
        }


        private ObservableCollection<string> DisplayDateInComboBoxddMMMyyyy(ObservableCollection<string> ExpDate)
        {
            ObservableCollection<string> sortedList = new ObservableCollection<string>();
            ObservableCollection<string> sortedList1 = new ObservableCollection<string>();
            foreach (var item in ExpDate)//.Select(x => (DateTime.ParseExact(item., "dd-MMM-yyyy", CultureInfo.InvariantCulture).ToString("yy-MM-dd"))).Distinct())
                if (!(item == " All" || item == "All"))
                    sortedList.Add(DateTime.ParseExact(item, "yy-MMM-dd", CultureInfo.InvariantCulture).ToString("yy-MM-dd"));
                else
                    sortedList.Add(item);
            sortedList = new ObservableCollection<string>(sortedList.OrderBy(p => p));
            sortedList1 = DisplayDateInComboBox(sortedList);
            sortedList.Clear();
            foreach (var item in sortedList1)//.Select(x => (DateTime.ParseExact(item., "dd-MMM-yyyy", CultureInfo.InvariantCulture).ToString("yy-MM-dd"))).Distinct())
                if (!(item == " All" || item == "All"))
                    sortedList.Add(DateTime.ParseExact(item, "dd-MM-yy", CultureInfo.InvariantCulture).ToString("dd-MM-yy"));
                else
                    sortedList.Add(item);
            return sortedList;
        }

        private void SelctionOfAllDataGridRow(object e)
        {
            DataGrid dg = e as DataGrid;
            foreach (ScripProfModel c in dg.ItemsSource)
            {
                c.IsSelected = true;
            }
            dg.SelectAll();
        }

        private void OnChnageOfFutureType()
        {
            if (SelectedFutType == "Normal Fut")
                FutSelected = null;
            else
                CalSelected = 5;
        }

        private void onchangeofIndiceSet1()//Scrip ID and Grp Name need to fetched from Common Functions
        {

            if (flag)
            {
                try
                {
                    oDataAccessLayer.Connect((int)DataAccessLayer.ConnectionDB.Masters);
                   // oDataAccessLayer.OpenConnection((int)DataAccessLayer.ConnectionDB.Masters);
                    IndicesFlag = false;
                    IsSearchEnable = false;
                    ResetValuesForScripAndIndicesSet();
                    ObjEquityDataCollection.Clear();
                    if (ScripSet != null && ScripSet.Count > 0)
                        SelectedScripSet = ScripSet[0];
                    oDataAccessLayer.OpenConnection((int)DataAccessLayer.ConnectionDB.Masters);
#if TWS
                    #region TWS

                    if (SelectedIndicesSet == ScripProfilingModel.ScripSet._Select.ToString().Replace("_", " "))
                    {
                        str = @"SELECT * FROM BSE_SECURITIES_CFE where InstrumentType = 'E' AND ((ScripCode  BETWEEN '100000' AND '200000') 
                        OR (ScripCode BETWEEN '500000' AND '600000') OR (ScripCode >= 700000));";
                        ReadEquity(str);
                    }
                    else
                    {
                        str = "SELECT IndexCode FROM BSE_SNPINDICES_CFE where ExistingShortName ='" + SelectedIndicesSet + "'";
                        var keysWithMatchingValues = oDataAccessLayer.ExecuteScalar((int)ConnectionDB.Masters,str, System.Data.CommandType.Text, null);

                        str = "SELECT * FROM BSE_SYSBASSUB_CFE where IndexCode = '" + keysWithMatchingValues + "'";
                        SQLiteDataReader oSQLiteDataReader = oDataAccessLayer.ExecuteDataReader((int)ConnectionDB.Masters,str, System.Data.CommandType.Text, null);
                        while (oSQLiteDataReader.Read())
                        {
                            long scripCode = Convert.ToInt64(oSQLiteDataReader["ScripCode"]);
                            ScripProfModel objsc = new ScripProfModel();
                            objsc.ScripId = CommonFunctions.GetScripId(scripCode, Enumerations.Exchange.BSE, Enumerations.Segment.Equity);//BSE EQUITY
                            objsc.ScripCode = scripCode;
                            objsc.GroupName = CommonFunctions.GetGroupName(scripCode, Enumerations.Exchange.BSE, Enumerations.Segment.Equity);//BSE EQUITY
                            ObjEquityDataCollection.Add(objsc);
                        }
                    }
                    lblCountContent = ObjEquityDataCollection.Count + " Scrips";
                    ObjEquityDataCollection.GroupBy(x => x.ScripId);
                    TempEquityDataCollection = ObjEquityDataCollection;
                    Is4L6LEnabledGrpBox = false;
                    Is4LEnabledGrpBox = false;
                    Is6LEnabledGrpBox = false;
                    #endregion
#elif BOW
#endif
                }

            catch (Exception e)
                {

                }
                finally
                {
                    oDataAccessLayer.CloseConnection((int)DataAccessLayer.ConnectionDB.Masters);
                    System.Data.SQLite.SQLiteConnection.ClearAllPools();
                   
                }
            }
            flag = true;
        }


        public static IEnumerable<object> GetValues<T>(IEnumerable<T> items, string propertyName)
        {
            Type type = typeof(T);
            var prop = type.GetProperty(propertyName);
            foreach (var item in items)
                yield return prop.GetValue(item, null);
        }

        private void CreateCSVIntoFile()
        {
            isCmbProfileEnable = false;
            AllSelected = false;
            AllSelectedForGrid2 = false;
            ReplyText = string.Empty;
            IsBlankRowEnabled = true;
            IsAddEnable = true;
            PopulateGrid2 = "Visible";
            PopulateFileNameGrid = "Hidden";
            IsSaveAsEnabled = false;
            IsSaveAsVisible = "Hidden";
            IsSaveEnabled = true;
            IsSaveVisible = "Visible";
            ReplyText = string.Empty;
            SelectedScripProfileSPR = "Select";
            IsScripProfileEnable = false;
            IsColProfileVisible = "Visible";
            IsSaveColProfileEnabled = false;
            selectedList.Clear();
            //cmbProfileName.Add(WindowName.Exchange_Default_Profile.ToString().Replace("_", " "));
            cmbProfileName.Add("DEFAULT");
            cmbProfileSelected = cmbProfileName[0];
        }

        private void PopulatingUpdateButtonGrid()
        {
            FileCombo.Clear();
            ReadScripFiles();
            isCmbProfileEnable = true;
            AllSelected = false;
            AllSelectedForGrid2 = false;
            IsBlankRowEnabled = true;
            IsAddEnable = true;
            IsScripProfileEnable = true;
            PopulateFileNameGrid = "Hidden";
            PopulateGrid2 = "Visible";
            IsSaveVisible = "Hidden";
            IsSaveEnabled = false;
            //IsSaveVisible = "Hidden";
            IsSaveAsVisible = "Visible";
            IsSaveAsEnabled = true;
            SelectedScripProfileSPR = FileCombo[0];
            ReplyText = string.Empty;
            IsColProfileVisible = "Visible";
            IsSaveColProfileEnabled = true;
            selectedList.Clear();
            ReadColProfiles();
            cmbProfileSelected = cmbProfileName[0];
            #region comment
            //DataGrid datagrid = e as DataGrid;

            //MessageBoxButton
            //if (selectedList.Count > 0)
            //{
            //    MessageBoxButton buttons = MessageBoxButton.YesNo;
            //    MessageBoxResult result = MessageBox.Show("Cancel Operation", "Do you want to continue", buttons);

            //    if (result == MessageBoxResult.Yes)
            //    {
            //        IsDeleteChecked = false;
            //        IsCreateChecked = false;
            //        IsUpdateChecked = true;
            //        IsSaveAsEnabled = true;
            //        PopulateFileNameGrid = "Hidden";
            //        PopulateGrid2 = "Visible";
            //        IsSaveVisible = "Hidden";
            //        IsSaveEnabled = false;
            //        IsSaveAsVisible = "Visible";
            //        // IsScripProfileEnable = true;
            //        ReplyText = string.Empty;
            //        selectedList.Clear();
            //    }
            //    else if (result == MessageBoxResult.No)
            //    {
            //        IsCreateChecked = true;
            //        return;
            //    }



            //IsCreateChecked = true;
            //else if(result == MessageBoxResult.No)
            //{
            //    IsCreateChecked = true;
            //    PopulateGrid2 = "Visible";
            //    NotifyPropertyChanged("selectedList");
            //}
            // }
            #endregion
        }

        private void ReadColProfiles()
        {
            cmbProfileName = new ObservableCollection<string>();
            try
            {
                oDataAccessLayer.Connect((int)DataAccessLayer.ConnectionDB.Masters);
                oDataAccessLayer.OpenConnection((int)DataAccessLayer.ConnectionDB.Masters);
#if TWS
                string strQuery = @"SELECT distinct(ProfileName) FROM USER_DEFINED_PROFILE where MemberID=" + "'" + UtilityLoginDetails.GETInstance.MemberId.ToString() + "' AND TraderID =" + "'" + UtilityLoginDetails.GETInstance.TraderId.ToString() + "' AND ScreenName='Touchline';";
#elif BOW
                string strQuery = @"SELECT distinct(ProfileName) FROM USER_DEFINED_PROFILE where MemberID=" + "'" + UtilityLoginDetails.GETInstance.UserLoginId.ToString() + "' AND TraderID = 'NA' AND ScreenName='Touchline';";
#endif
                oSQLiteDataReader = oDataAccessLayer.ExecuteDataReader((int)ConnectionDB.Masters,strQuery, System.Data.CommandType.Text, null);

                while (oSQLiteDataReader.Read())
                {
                    ColumnProfilingModel cpm = new ColumnProfilingModel();

                    //Profile Name
                    cpm.ColProfile = oSQLiteDataReader["ProfileName"]?.ToString().Trim();

                    cmbProfileName.Add(cpm.ColProfile.ToString());
                }
                //cmbProfileName.Add(WindowName.Exchange_Default_Profile.ToString().Replace("_", " "));
                cmbProfileName.Add("DEFAULT");
                cmbProfileName.Distinct();
                //SelectedScripProfileSPR = WindowName.Exchange_Default_Profile.ToString().Replace("_", " ");
            }
            catch (Exception e)
            {
                ExceptionUtility.LogError(e);
            }
            finally
            {
               oDataAccessLayer.CloseConnection((int)DataAccessLayer.ConnectionDB.Masters);
                System.Data.SQLite.SQLiteConnection.ClearAllPools();
            }
        }

        private void OnChangeOfScripSet1()
        {
            try
            {
                oDataAccessLayer.Connect((int)DataAccessLayer.ConnectionDB.Masters);
               // oDataAccessLayer.OpenConnection((int)DataAccessLayer.ConnectionDB.Masters);
#if TWS
                if (IndicesFlag)
                {
                    flag = false;
                    ObjEquityDataCollection.Clear();
                    if (IndicesSet != null && IndicesSet.Count > 0)
                        SelectedIndicesSet = IndicesSet[0];
                    IsSearchEnable = false;
                    ResetValuesForScripAndIndicesSet();
                    ObjEquityDataCollection.Clear();
                    oDataAccessLayer.OpenConnection((int)DataAccessLayer.ConnectionDB.Masters);
                    string strQuery = "SELECT * FROM BSE_SECURITIES_CFE  ";

                    switch (SelectedScripSet)
                    {
                        case "ReList Scrips":
                            strQuery += " where " + ScripMasterEnum.CallAuctionIndicator.ToString() + " = '2';";
                            break;
                        case "PreOpen Scrips":
                            strQuery += "where " + ScripMasterEnum.CallAuctionIndicator.ToString() + " = '90';";
                            break;
                        case "IPO Scrips":
                            strQuery += "where " + ScripMasterEnum.CallAuctionIndicator.ToString() + " = '1';";
                            break;
                        case "PCAS Scrips":
                            strQuery += "where " + ScripMasterEnum.CallAuctionIndicator.ToString() + " = '3';";
                            break;
                        case "Proposed Scrips":
                            strQuery += " where Status = 'I';";
                            break;
                        case "SPOS Scrips":
                            strQuery += "where " + ScripMasterEnum.CallAuctionIndicator.ToString() + "= '1' OR " + ScripMasterEnum.CallAuctionIndicator.ToString() + " = '2';";
                            break;
                        case "BSE Exclusive":
                            strQuery += "where " + ScripMasterEnum.BseExclusive.ToString() + " = 'Y';";
                            break;
                        case "All Currency Spreads":
                            {
                                ObjEquityDataCollection = new ObservableCollection<ScripProfModel>();
                                foreach (var item in MasterSharedMemory.objMasterCurrencyDictBaseBSE.Where(x => x.Value.StrategyID == 0))
                                {
                                    ScripProfModel currsm = new ScripProfModel();
                                    currsm.GroupName = item.Value.CurrScripGroup;
                                    currsm.ScripId = item.Value.InstrumentName;
                                    currsm.ScripCode = item.Value.ContractTokenNum;
                                    currsm.ExpiryDate = MasterSharedMemory.objMasterCurrencyDictBaseBSE.Where(x => x.Value.ContractTokenNum == item.Value.ContractTokenNum).Select(x => x.Value.ExpiryDate).FirstOrDefault();
                                    ObjEquityDataCollection.Add(currsm);
                                }
                                ObjEquityDataCollection.GroupBy(x => x.ScripId);
                                TempEquityDataCollection = ObjEquityDataCollection;
                                lblCountContent = ObjEquityDataCollection.Count + " Scrips";
                            }
                            break;
                        case "All Derivative Spreads":
                            {
                                ObjEquityDataCollection = new ObservableCollection<ScripProfModel>();
                                foreach (var item in MasterSharedMemory.objMasterDerivativeDictBaseBSE.Where(x => x.Value.StrategyID == 0))
                                {
                                    ScripProfModel dersm = new ScripProfModel();
                                    dersm.GroupName = item.Value.DerScripGroup;
                                    dersm.ScripId = item.Value.InstrumentName;
                                    dersm.ScripCode = item.Value.ContractTokenNum;
                                    dersm.ExpiryDate = MasterSharedMemory.objMasterDerivativeDictBaseBSE.Where(x => x.Value.ContractTokenNum == item.Value.ContractTokenNum).Select(x => x.Value.ExpiryDate).FirstOrDefault();
                                    ObjEquityDataCollection.Add(dersm);
                                }
                                ObjEquityDataCollection.GroupBy(x => x.ScripId);
                                TempEquityDataCollection = ObjEquityDataCollection;
                                lblCountContent = ObjEquityDataCollection.Count + " Scrips";
                            }
                            break;
                        case "Derivative Scrips":
                            {
                                strQuery = "Select distinct(b.ScripCode),b.ScripId,b.GroupName from BSE_DERIVATIVE_CO_CFE a, BSE_SECURITIES_CFE b where AssestTokenNum>1000 and a.AssestTokenNum=b.ScripCode ORDER BY b.ScripId ASC;";
                            }
                            //strQuery += "where " + ScripMasterEnum.BseExclusive.ToString() + " = 'N';";

                            break;
                        case "Only 6L Series":
                            strQuery += "where ScripCode BETWEEN '600000' AND '700000';";
                            break;
                        case "Only 4L Series":
                            strQuery += "where ScripCode BETWEEN '400000' AND '500000';";
                            break;
                        case "GSM Scrips":
                            strQuery += " where Filler2_GSM!='100';";
                            break;
                        default:
                            strQuery += " where InstrumentType = 'E' AND ((ScripCode  BETWEEN '100000' AND '200000') OR (ScripCode BETWEEN '500000' AND '600000') OR (ScripCode >= 700000));";
                            break;
                    }

                    if (!(SelectedScripSet.Equals("All Currency Spreads") || SelectedScripSet.Equals("All Derivative Spreads")))
                        ReadEquity(strQuery);
                    ObjEquityDataCollection.OrderBy(x => x.ScripId);
                    TempEquityDataCollection = ObjEquityDataCollection;
                    lblCountContent = ObjEquityDataCollection.Count + " Scrips";
                }
                IndicesFlag = true;
                Is4L6LEnabledGrpBox = false;
                Is4LEnabledGrpBox = false;
                Is6LEnabledGrpBox = false;
            }
            catch (Exception)
            {
            }
            finally
            {
                oDataAccessLayer.CloseConnection((int)DataAccessLayer.ConnectionDB.Masters);
                System.Data.SQLite.SQLiteConnection.ClearAllPools();
            }
         
#endif
        }

        private void ReadEquity(string strQuery)
        {
            SQLiteDataReader oSQLiteDataReader = oDataAccessLayer.ExecuteDataReader((int)ConnectionDB.Masters,strQuery, System.Data.CommandType.Text, null);
            while (oSQLiteDataReader.Read())
            {
                ScripProfModel objsc = new ScripProfModel();
                objsc.ScripId = oSQLiteDataReader["ScripId"]?.ToString().Trim();
                objsc.ScripCode = Convert.ToInt64(oSQLiteDataReader["ScripCode"]);
                objsc.GroupName = oSQLiteDataReader["GroupName"]?.ToString().Trim();
                ObjEquityDataCollection.Add(objsc);
            }
        }

        private void OnChangeOfScripSet()
        {
            try
            {
                oDataAccessLayer.Connect((int)DataAccessLayer.ConnectionDB.Masters);
               oDataAccessLayer.OpenConnection((int)DataAccessLayer.ConnectionDB.Masters);


                if (IndicesFlag)
                {
                    flag = false;
                    ObjEquityDataCollection.Clear();
                    if (IndicesSet != null && IndicesSet.Count > 0)
                        SelectedIndicesSet = IndicesSet[0];
                    IsSearchEnable = false;
                    if (SelectedScripSet == ScripProfilingModel.ScripSet.ReList_Scrips.ToString().Replace("_", " "))
                    {
                        ObjEquityDataCollection.Clear();

                        string strQuery = "SELECT * FROM SCRIPMASTER where CallAuctionIndicator=2;";

                        SQLiteDataReader oSQLiteDataReader = oDataAccessLayer.ExecuteDataReader((int)ConnectionDB.Masters,strQuery, System.Data.CommandType.Text, null);

                        while (oSQLiteDataReader.Read())//reader is forward only and read only
                        {
                            ScripProfModel objsc = new ScripProfModel();
                            objsc.ScripId = oSQLiteDataReader["ScripId"]?.ToString().Trim();
                            objsc.ScripCode = Convert.ToInt64(oSQLiteDataReader["ScripCode"]);
                            objsc.GroupName = oSQLiteDataReader["GroupName"]?.ToString().Trim();
                            ObjEquityDataCollection.Add(objsc);
                        }
                        ObjEquityDataCollection.OrderBy(x => x.ScripId);
                        TempEquityDataCollection = ObjEquityDataCollection;
                        lblCountContent = ObjEquityDataCollection.Count + " Scrips";
                        //oDataAccessLayer.CloseConnection((int)DataAccessLayer.ConnectionDB.Masters);
                    }
                    else if (SelectedScripSet == ScripProfilingModel.ScripSet.PreOpen_Scrips.ToString().Replace("_", " "))
                    {
                        ObjEquityDataCollection.Clear();
                        //foreach (var item in MasterSharedMemory.objMastertxtDictBase)
                        //{
                        //    if (item.Value.CallAuctionIndicator == 90)
                        //    {
                        //        ScripProfModel objsc = new ScripProfModel();
                        //        objsc.ScripId = item.Value.ScripId;
                        //        objsc.ScripCode = item.Value.ScripCode;
                        //        objsc.GroupName = item.Value.GroupName;
                        //        ObjEquityDataCollection.Add(objsc);
                        //    }
                        //}
                        //ObjEquityDataCollection.OrderBy(x => x.ScripId);
                        //TempEquityDataCollection = ObjEquityDataCollection;
                        lblCountContent = ObjEquityDataCollection.Count + " Scrips";
                    }
                    else if (SelectedScripSet == ScripProfilingModel.ScripSet.IPO_Scrips.ToString().Replace("_", " "))
                    {
                        ObjEquityDataCollection.Clear(); //TODO TBD2017
                                                         //foreach (var item in MasterSharedMemory.objMastertxtDict)
                                                         //{
                                                         //    if (item.Value.CallAuctionIndicator == 1)
                                                         //    {
                                                         //        ScripProfModel objsc = new ScripProfModel();
                                                         //        objsc.ScripId = item.Value.ScripId;
                                                         //        objsc.ScripCode = item.Value.ScripCode;
                                                         //        objsc.GroupName = item.Value.GroupName;
                                                         //        ObjEquityDataCollection.Add(objsc);
                                                         //    }
                                                         //}
                                                         //ObjEquityDataCollection.OrderBy(x => x.ScripId);
                                                         //TempEquityDataCollection = ObjEquityDataCollection;
                        lblCountContent = ObjEquityDataCollection.Count + " Scrips";
                    }
                    else if (SelectedScripSet == ScripProfilingModel.ScripSet.PCAS_Scrips.ToString().Replace("_", " "))
                    {
                        ObjEquityDataCollection.Clear(); //TODO TBD2017
                                                         //foreach (var item in MasterSharedMemory.objMastertxtDict)
                                                         //{
                                                         //    if (item.Value.CallAuctionIndicator == 3)
                                                         //    {
                                                         //        ScripProfModel objsc = new ScripProfModel();
                                                         //        objsc.ScripId = item.Value.ScripId;
                                                         //        objsc.ScripCode = item.Value.ScripCode;
                                                         //        objsc.GroupName = item.Value.GroupName;
                                                         //        ObjEquityDataCollection.Add(objsc);
                                                         //    }
                                                         //}
                                                         //ObjEquityDataCollection.OrderBy(x => x.ScripId);
                                                         //TempEquityDataCollection = ObjEquityDataCollection;
                        lblCountContent = ObjEquityDataCollection.Count + " Scrips";
                    }
                    else if (SelectedScripSet == ScripProfilingModel.ScripSet.Proposed_Scrips.ToString().Replace("_", " "))
                    {
                        ObjEquityDataCollection.Clear(); //TODO TBD2017
                                                         //foreach (var item in MasterSharedMemory.objMastertxtDict)
                                                         //{
                                                         //    if (item.Value.Status == 'I')
                                                         //    {
                                                         //        ScripProfModel objsc = new ScripProfModel();
                                                         //        objsc.ScripId = item.Value.ScripId;
                                                         //        objsc.ScripCode = item.Value.ScripCode;
                                                         //        objsc.GroupName = item.Value.GroupName;
                                                         //        ObjEquityDataCollection.Add(objsc);
                                                         //    }
                                                         //}
                                                         //ObjEquityDataCollection.OrderBy(x => x.ScripId);
                                                         //TempEquityDataCollection = ObjEquityDataCollection;
                        lblCountContent = ObjEquityDataCollection.Count + " Scrips";
                    }
                    else if (SelectedScripSet == ScripProfilingModel.ScripSet.SPOS_Scrips.ToString().Replace("_", " "))
                    {
                        ObjEquityDataCollection.Clear(); //TODO TBD2017
                                                         //foreach (var item in MasterSharedMemory.objMastertxtDict)
                                                         //{
                                                         //    if (item.Value.CallAuctionIndicator == 1 || item.Value.CallAuctionIndicator == 2)
                                                         //    {
                                                         //        ScripProfModel objsc = new ScripProfModel();
                                                         //        objsc.ScripId = item.Value.ScripId;
                                                         //        objsc.ScripCode = item.Value.ScripCode;
                                                         //        objsc.GroupName = item.Value.GroupName;
                                                         //        ObjEquityDataCollection.Add(objsc);
                                                         //    }
                                                         //}
                                                         //ObjEquityDataCollection.OrderBy(x => x.ScripId);
                                                         //TempEquityDataCollection = ObjEquityDataCollection;
                        lblCountContent = ObjEquityDataCollection.Count + " Scrips";
                    }
                    else if (SelectedScripSet == ScripProfilingModel.ScripSet.BSE_Exclusive.ToString().Replace("_", " "))
                    {
                        ObjEquityDataCollection.Clear(); //TODO TBD2017
                                                         //foreach (var item in MasterSharedMemory.objMastertxtDict)
                                                         //{
                                                         //    if (item.Value.BseExclusive == 'Y')
                                                         //    {
                                                         //        ScripProfModel objsc = new ScripProfModel();
                                                         //        objsc.ScripId = item.Value.ScripId;
                                                         //        objsc.ScripCode = item.Value.ScripCode;
                                                         //        objsc.GroupName = item.Value.GroupName;
                                                         //        ObjEquityDataCollection.Add(objsc);
                                                         //    }
                                                         //}
                                                         //ObjEquityDataCollection.OrderBy(x => x.ScripId);
                                                         //TempEquityDataCollection = ObjEquityDataCollection;
                        lblCountContent = ObjEquityDataCollection.Count + " Scrips";
                    }
                    else if (SelectedScripSet == ScripProfilingModel.ScripSet.All_Currency_Spreads.ToString().Replace("_", " "))
                    {
                        ObjEquityDataCollection.Clear();
                        //foreach (var item in CurrencyDerivativeMemory.objCurrencyCommon)
                        //{
                        //    ScripProfModel currsm = new ScripProfModel();
                        //    currsm.GroupName = item.Value.CurrScripGroup;
                        //    currsm.ScripId = item.Value.InstrumentName;
                        //    currsm.ScripCode = item.Value.ScripCode;
                        //    currsm.ExpiryDate = CurrencyDerivativeMemory.objCurrencyCommon.Where(x => x.Value.ScripCode == item.Value.ScripCode).Select(x => x.Value.ExpiryDate).FirstOrDefault();
                        //    ObjEquityDataCollection.Add(currsm);
                        //}
                        //ObjEquityDataCollection.GroupBy(x => x.ScripId);
                        TempEquityDataCollection = ObjEquityDataCollection;
                        lblCountContent = ObjEquityDataCollection.Count + " Scrips";
                    }
                    else if (SelectedScripSet == ScripProfilingModel.ScripSet.All_Derivative_Spreads.ToString().Replace("_", " "))
                    {
                        ObjEquityDataCollection.Clear();
                        //foreach (var item in CurrencyDerivativeMemory.objDerivativeCommon)
                        //{
                        //    ScripProfModel dersm = new ScripProfModel();
                        //    dersm.GroupName = item.Value.DerScripGroup;
                        //    dersm.ScripId = item.Value.InstrumentName;
                        //    dersm.ScripCode = item.Value.ScripCode;
                        //    dersm.ExpiryDate = CurrencyDerivativeMemory.objCurrencyCommon.Where(x => x.Value.ScripCode == item.Value.ScripCode).Select(x => x.Value.ExpiryDate).FirstOrDefault();
                        //    ObjEquityDataCollection.Add(dersm);
                        //}
                        //ObjEquityDataCollection.GroupBy(x => x.ScripId);
                        TempEquityDataCollection = ObjEquityDataCollection;
                        lblCountContent = ObjEquityDataCollection.Count + " Scrips";
                    }
                    else if (SelectedScripSet == ScripProfilingModel.ScripSet.Derivative_Scrips.ToString().Replace("_", " "))
                    {
                        ObjEquityDataCollection.Clear();
                        //foreach (var item in CurrencyDerivativeMemory.objDerivativeCommon)
                        //{
                        //    if (item.Value.ProductType == "SF")
                        //    {
                        //        ScripProfModel dersm = new ScripProfModel();
                        //        dersm.GroupName = item.Value.DerScripGroup;
                        //        dersm.ScripId = item.Value.InstrumentName;
                        //        dersm.ScripCode = item.Value._ParentScripCode;
                        //        dersm.ExpiryDate = CurrencyDerivativeMemory.objCurrencyCommon.Where(x => x.Value.ScripCode == item.Value._ParentScripCode).Select(x => x.Value.ExpiryDate).FirstOrDefault();
                        //        if (ObjEquityDataCollection.Any(p => p.ScripCode == dersm.ScripCode) == false)
                        //            ObjEquityDataCollection.Add(dersm);
                        //    }
                        //}
                        //ObjEquityDataCollection.GroupBy(x => x.ScripId);
                        TempEquityDataCollection = ObjEquityDataCollection;
                        lblCountContent = ObjEquityDataCollection.Count + " Scrips";
                    }
                    else if (SelectedScripSet == ScripProfilingModel.ScripSet.Only_6L_Series.ToString().Replace("_", " "))
                    {
                        ObjEquityDataCollection.Clear();
                        //foreach (var item in MasterSharedMemory.objMastertxtDic)
                        //{
                        //    if (item.Value.ScripCode >= 600000 && item.Value.ScripCode < 700000)
                        //    {
                        //        ScripProfModel eqsm = new ScripProfModel();
                        //        eqsm.GroupName = item.Value.GroupName;
                        //        eqsm.ScripId = item.Value.ScripId;
                        //        eqsm.ScripCode = item.Value.ScripCode;
                        //        ObjEquityDataCollection.Add(eqsm);
                        //    }
                        //}
                        //ObjEquityDataCollection.GroupBy(x => x.ScripId);
                        TempEquityDataCollection = ObjEquityDataCollection;
                        lblCountContent = ObjEquityDataCollection.Count + " Scrips";
                    }
                    else if (SelectedScripSet == ScripProfilingModel.ScripSet.Only_4L_Series.ToString().Replace("_", " "))
                    {
                        ObjEquityDataCollection.Clear();
                        //foreach (var item in MasterSharedMemory.objMastertxtDic)
                        //{
                        //    if (item.Value.ScripCode >= 400000 && item.Value.ScripCode < 500000)
                        //    {
                        //        ScripProfModel eqsm = new ScripProfModel();
                        //        eqsm.GroupName = item.Value.GroupName;
                        //        eqsm.ScripId = item.Value.ScripId;
                        //        eqsm.ScripCode = item.Value.ScripCode;
                        //        ObjEquityDataCollection.Add(eqsm);
                        //    }
                        //}
                        //ObjEquityDataCollection.GroupBy(x => x.ScripId);
                        TempEquityDataCollection = ObjEquityDataCollection;
                        lblCountContent = ObjEquityDataCollection.Count + " Scrips";
                    }
                    else
                    {
                        ObjEquityDataCollection.Clear();
                        Dictionary<long, ScripMaster> objEquityDetailsDic = new Dictionary<long, ScripMaster>(); //TODO TBD2017
                                                                                                                 //if (MasterSharedMemory.objMastertxtDict != null)
                                                                                                                 //{
                                                                                                                 //    objEquityDetailsDic = MasterSharedMemory.objMastertxtDict.Where(x => (x.Value.InstrumentType == 'E') && ((x.Value.ScripCode >= 100000 && x.Value.ScripCode < 200000) || (x.Value.ScripCode >= 500000 && x.Value.ScripCode < 600000) || (x.Value.ScripCode >= 700000))).OrderBy(y => y.Value.ScripId).ToDictionary(x => x.Key, x => x.Value);
                                                                                                                 //}
                        foreach (var item in objEquityDetailsDic)
                        {
                            ScripProfModel scripmasterEquity = new ScripProfModel();
                            scripmasterEquity.ScripId = item.Value.ScripId;
                            scripmasterEquity.ScripCode = item.Value.ScripCode;
                            scripmasterEquity.GroupName = item.Value.GroupName;
                            ObjEquityDataCollection.Add(scripmasterEquity);
                        }
                        ObjEquityDataCollection.GroupBy(x => x.ScripId);
                        TempEquityDataCollection = ObjEquityDataCollection;
                        lblCountContent = ObjEquityDataCollection.Count + " Scrips";
                    }
                }
                IndicesFlag = true;
            }
            catch (Exception ex)
            {
            }
            finally
            {
                oDataAccessLayer.CloseConnection((int)DataAccessLayer.ConnectionDB.Masters);
                System.Data.SQLite.SQLiteConnection.ClearAllPools();
            }
        }//for BOW

        private void AditionOfBlankRow(object sender)
        {
            DataGrid dg = sender as DataGrid;
            
            if (SelectBlankRow != null)
            {
                if (SelectBlankRow.ScripCode != 0)
                    LastData = selectedList.IndexOf(selectedList.FirstOrDefault(i => i.ScripCode == SelectBlankRow.ScripCode));
                else
                {
                    LastData = dg.SelectedIndex;
                }
                   // LastData = selectedList.IndexOf(selectedList.Last(i => i.ScripCode == SelectBlankRow.ScripCode));
                selectedList.Insert(Convert.ToInt32(LastData) + 1, new ScripProfModel());
                ReplyText = "Blank Row Added/ Total Scrips Populated: " + selectedList.Count();

                foreach (ScripProfModel oScripProfModel in selectedList)
                {
                    oScripProfModel.PropertyChanged += OnElementPropertyChanged1;
                }
            }
            else
            {
                MessageBox.Show("Select Scrip/row before adding blank row", "Incorrect Data", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
        }

        private void OnChangeOfOptionType()
        {
            if (SelectedOptionType == "Call")
                optionTyp = "CE";
            else if (SelectedOptionType == "Put")
            {
                optionTyp = "PE";
            }
        }

        private void PopulatingDeleteButtonGrid(object e)
        {
            ReplyText = String.Empty;
            ScripProfileSPRList.Clear();
            isCmbProfileEnable = false;
            AllSelected = false;
            AllSelectedForGrid2 = false;
            IsBlankRowEnabled = false;
            IsSaveAsVisible = "Hidden";
            IsSaveVisible = "Visible";
            PopulateFileNameGrid = "Visible";
            PopulateGrid2 = "Hidden";
            SelectedScripProfileSPR = "Select";
            IsScripProfileEnable = false;
            IsSaveEnabled = false;
            IsSaveAsEnabled = false;
            IsColProfileVisible = "Visible";
            IsSaveColProfileEnabled = false;
            cmbProfileName.Add("DEFAULT");
            cmbProfileSelected = cmbProfileName[0];

            string masterPath = CsvFilesPath.ToString();
            string filePattern = "*.csv";

            var coFiles = from fullFilename
                       in Directory.EnumerateFiles(masterPath, filePattern)
                          select Path.GetFileName(fullFilename);

            foreach (var item in coFiles)
            {
                ScripProfModel scripProfModel = new ScripProfModel();
                scripProfModel.FileName = item;
                ScripProfileSPRList.Add(scripProfModel);
            }

            foreach (ScripProfModel oScripProfModel in ScripProfileSPRList)
            {
                oScripProfModel.PropertyChanged += OnElementPropertyChanged;
            }
            NotifyPropertyChanged("FileCombo");
        }

        private void CheckAllColumns(object e)
        {
            DataGrid datagrid = e as DataGrid;
            if (datagrid.ItemsSource != null)
            {
                datagrid.SelectAll();
                datagrid.Focus();
            }
        }

        private void OnChangeOfSelectedScripProfileSPR(object e)
        {
            NotifyPropertyChanged("FileCombo");
            selectedList.Clear();
            DataGrid datagrid = e as DataGrid;
            IsSaveAsEnabled = true;
            IsSaveAsVisible = "Visible";
            IsSaveEnabled = false;
            IsSaveVisible = "Hidden";
            selectedList.Clear();
            string filename = SelectedScripProfileSPR;
            if (SelectedScripProfileSPR == "Select")
            {
                selectedList.Clear();
            }
            else if (!File.Exists(CsvFilesPath.ToString() + filename))
            {
                ReplyText = "File is Not Present";
            }
            else
            {
                if (selectedList.Count >= 0)
                {
                    string[] lines = File.ReadAllLines(CsvFilesPath.ToString() + filename);

                    try
                    {
                        for (int i = 0; i < lines.GetLength(0); i++)
                        {
                            string grp = string.Empty;
                            string[] strArray2 = lines[i].Split(new char[] { ',' });
                            ScripProfModel sm = new ScripProfModel();

                            if (!string.IsNullOrEmpty(strArray2[0]))
                                sm.ScripId = strArray2[0];
                            else
                                sm.ScripId = string.Empty;
                            if (!string.IsNullOrEmpty(strArray2[1]))
                                sm.ScripCode = Convert.ToInt64(strArray2[1]);
                            else
                                sm.ScripCode = Convert.ToInt64(Convert.ToString(0));

                            if (selectedList != null && selectedList.Count != 0)
                            {
                                if (sm.ScripCode != 0)
                                {
                                    ScripProfModel obj = selectedList.Where(x => x.ScripCode == sm.ScripCode).FirstOrDefault();

                                    if (obj != null)
                                        continue;
                                }
                            }

                            if ((sm.ScripCode != 0) && (!CommonFunctions.ValidScripOrNot(sm.ScripCode)))
                                continue;

                            if (!String.IsNullOrEmpty(CommonFunctions.GetGroupName(sm.ScripCode, Exchange.BSE, Segment.Equity)))
                                grp = CommonFunctions.GetGroupName(sm.ScripCode, Exchange.BSE, Segment.Equity);
                            else if (!String.IsNullOrEmpty(CommonFunctions.GetGroupName(sm.ScripCode, Exchange.BSE, Segment.Derivative)))
                                grp = CommonFunctions.GetGroupName(sm.ScripCode, Exchange.BSE, Segment.Derivative);
                            else if (!String.IsNullOrEmpty(CommonFunctions.GetGroupName(sm.ScripCode, Exchange.BSE, Segment.Currency)))
                                grp = CommonFunctions.GetGroupName(sm.ScripCode, Exchange.BSE, Segment.Currency);
                            else
                                grp = string.Empty;

                            sm.GroupName = grp;
                            selectedList.Add(sm);
                        }
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtility.LogError(ex);
                        MessageBox.Show("File is not in correct format");
                        return;
                    }


                    foreach (ScripProfModel oScripProfModel in selectedList)
                    {
                        oScripProfModel.PropertyChanged += OnElementPropertyChanged1;
                    }
                    datagrid.CanUserAddRows = false;
                    datagrid.IsReadOnly = true;
                    ReplyText = "Total Scrips Populated: " + selectedList.Count;
                }
                NotifyPropertyChanged("FileCombo");
            }
            IsScripProfileEnable = true;
            datagrid.IsReadOnly = false;
        }

        private void SaveIntoSprFile(object e)
        {
            if (selectedList.Count <= 0 || PopulateFileNameGrid == "Visible")
            {
                MessageBox.Show("Add Scrips Before Saving File", "Incorrect Data", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            else
            {
                ScripProfModel scripprofile = new ScripProfModel();
                StreamWriter writer = null;
                try
                {
                    IsSaveAsEnabled = true;
                    RadioButton rd = e as RadioButton;
                    SaveFileDialog dlg = new SaveFileDialog();
                    if (IsCreateChecked || IsUpdateChecked)
                    {
                        if (IsCreateChecked)
                        {
                            dlg.FileName = "Market Watch"; // Default file name
                        }
                        else
                        {
                            dlg.FileName = SelectedScripProfileSPR; // Default file name
                        }
                        //dlg.FileName = "Market Watch"; // Default file name
                        dlg.DefaultExt = ".csv"; // Default file extension
                        dlg.Filter = "CSV Files (.csv)|*.csv"; // Filter files by extension
                        string StartupPath = CsvFilesPath.ToString();
                        dlg.InitialDirectory = Path.Combine(Path.GetDirectoryName(StartupPath));
                        // Show save file dialog box
                        Nullable<bool> result = dlg.ShowDialog();
                        // Process save file dialog box results
                        if (result == true)
                        {
                            while (Path.GetDirectoryName(dlg.FileName) != Path.Combine(Path.GetDirectoryName(StartupPath)))
                            {
                                MessageBox.Show("Please save CSV in the default path <Profile/MarketWatch>", "Scrip Profiling – Save File", MessageBoxButton.OK, MessageBoxImage.Information);
                                dlg.ShowDialog();
                                return;
                            }

                            // Save document
                            if (!File.Exists(CsvFilesPath.ToString() + dlg.SafeFileName))
                            {
                                writer = new StreamWriter(dlg.FileName, true, Encoding.UTF8);
                                {
                                    foreach (ScripProfModel line in selectedList)
                                    {
                                        if (!string.IsNullOrEmpty(line.ScripId)|| line.ScripCode!=0)
                                        {
                                            int segmentID = CommonFunctions.GetSegmentFromScripCode(line.ScripCode);
                                            string segment = CommonFunctions.GetSegmentName(segmentID);
                                            var grp = CommonFunctions.GetGroupName(line.ScripCode, "BSE", segment);
                                            writer.WriteLine(line.ScripId + "," + line.ScripCode);
                                        }
                                        else
                                        writer.WriteLine(string.Empty + "," + string.Empty);
                                    }
                                        
                                }
                                scripprofile.FileName = dlg.SafeFileName;
                                ScripProfileSPRList.Add(scripprofile);
                                FileCombo.Add(dlg.SafeFileName);
                                FileCombo.Distinct().ToList();
                            }
                            else
                            {
                                File.Delete(CsvFilesPath.ToString() + dlg.SafeFileName);
                                writer = new StreamWriter(dlg.FileName, true, Encoding.UTF8);
                                {
                                    foreach (ScripProfModel line in selectedList)
                                    {
                                        if (!string.IsNullOrEmpty(line.ScripId) || line.ScripCode != 0)
                                        {
                                            int segmentID = CommonFunctions.GetSegmentFromScripCode(line.ScripCode);
                                            string segment = CommonFunctions.GetSegmentName(segmentID-1);
                                            var grp = CommonFunctions.GetGroupName(line.ScripCode, "BSE", segment);
                                            writer.WriteLine(line.ScripId + "," + line.ScripCode );
                                        }
                                        else
                                            writer.WriteLine(string.Empty + "," + string.Empty);
                                    }
                                    //foreach (ScripProfModel line in selectedList)
                                    //    writer.WriteLine(line.ScripId + "," + line.ScripCode);
                                }
                                scripprofile.FileName = dlg.SafeFileName;
                                ScripProfileSPRList.Add(scripprofile);
                                FileCombo.Distinct();
                            }
                        }
                        else if (result == false)
                        {
                            return;
                        }
                    }
                    selectedList.Clear();
                    FileCombo.Distinct();
                    ReplyText = "File Saved Successfully in " + dlg.FileName;
                    if (OnSave != null)
                    {
                        OnSave();
                    }

                    IsScripProfileEnable = false;
                    IsProfileComboVisible = "Visible";
                    SelectedScripProfileSPR = "Select";
                    NotifyPropertyChanged("FileCombo");
                }
                catch (Exception ex)
                {
                    ExceptionUtility.LogError(ex);
                    MessageBox.Show("Error while saving file " + ex);
                }
                finally
                {
                    if (writer != null)
                    {
                        writer.Flush();
                        writer.Close();
                    }
                    NotifyPropertyChanged("selectedList");
                    NotifyPropertyChanged("FileCombo");

                    //Intimate TouchLine to Read the saved File
                    // RefreshTouchLine(true);
                }
            }
        }

        private void PopulatingEquityGrid3(object e)
        {
            DataGrid datagrid = e as DataGrid;
            datagrid.CanUserAddRows = false;
            datagrid.IsReadOnly = true;
            datagrid.SelectAll();
            datagrid.Focus();
            IsDeselectVisible = "Visible";
            IsSelectAllVisible = "Hidden";
            datagrid.IsReadOnly = false;
        }

        private void PopulatingEquityGrid4(object e)
        {
            IsSelectAllVisible = "Hidden";
            DataGrid datagrid = e as DataGrid;
            datagrid.UnselectAll();
            datagrid.Focus();
            IsSelectAllVisible = "Visible";
            IsDeselectVisible = "Hidden";
        }

        private void RemovingFromGrid(object e)
        {
            try
            {
                CheckBox chkbx = e as CheckBox;
                ReplyText = string.Empty;
                int cnt = 0;
                int count = 0;
                cnt = ScripProfileSPRList.Count;
                count = selectedList.Count;

                if (PopulateFileNameGrid == "Visible")
                {
                    if (ScripProfileSPRList.Where(x => x.IsSelected).Count() > 0)
                    {
                        if (MessageBox.Show("Do you want to Delete files ?", "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {
                            //FileCombo.Remove(ScripProfileSPRList.Where(x => x.IsSelected).Select(y => y.FileName).FirstOrDefault().ToString());
                            List<string> DeleteFileName = ScripProfileSPRList.Where(x => x.IsSelected).Select(y => y.FileName).ToList();
                            for (int i = 0; i < DeleteFileName.Count; i++)
                            {
                                File.Delete(CsvFilesPath.ToString() + DeleteFileName[i]);
                            }

                            List<string> RemoveFileName = ScripProfileSPRList.Where(x => x.IsSelected).Select(y => y.FileName).ToList();
                            for (int i = 0; i < RemoveFileName.Count; i++)
                            {
                                FileCombo.Remove(RemoveFileName[i]);
                            }
                            if (FileCombo.Count == 0)
                            {
                                ScripProfileSPRList.Clear();
                            }
                        }
                        else
                            return;
                        if (ScripProfileSPRList.Count == 0)
                            AllSelected = false;
                    }
                    else
                    {
                        MessageBox.Show("Select files for deleting", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                    ScripProfileSPRList.Where(l => l.IsSelected == true).ToList().All(i => ScripProfileSPRList.Remove(i));
                    cnt = cnt - ScripProfileSPRList.Count;
                    ReplyText = cnt + " Files Deleted";
                    NotifyPropertyChanged("FileCombo");
                }
                else if (PopulateGrid2 == "Visible")
                {
                    if (IsCreateChecked || IsUpdateChecked)
                    {
                        if (selectedList.Count == 0)
                        {
                            MessageBox.Show("No Scrips added to delete", "Incorrect Data", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }

                    if (selectedList.Where(x => x.IsSelected1).Count() > 0)
                    {
                        if (SelectBlankRow == null || SelectBlankRow.ScripCode != 0)
                        {
                            if (MessageBox.Show("Do you to  want Delete Scrips ?", "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                            {
                                selectedList.Where(l => l.IsSelected1 == true).ToList().All(i => selectedList.Remove(i));
                            }
                            else
                                return;
                            count = count - selectedList.Count;
                            if (count > 0)
                                ReplyText = count + " Scrips Deleted";
                        }
                        else
                        {
                            if (MessageBox.Show("Do you want to Delete Blank Row ?", "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                            {
                                selectedList.Where(l => l.IsSelected1 == true).ToList().All(i => selectedList.Remove(i));
                            }
                            else
                                return;
                            count = count - selectedList.Count;
                            if (count > 0)
                                ReplyText = count + " Blank Row Deleted";
                        }

                        if (selectedList.Count == 0)
                        {
                            AllSelectedForGrid2 = false;
                        }

                    }
                    else
                    {
                        MessageBox.Show("Select Scrips for deleting", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                    NotifyPropertyChanged("selectedList");
                    NotifyPropertyChanged("FileCombo");
                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
            }
            finally
            {
                //Intimate TouchLine to Read the saved File
                //RefreshTouchLine(true);
            }
        }

        private void PopulatingEquityGrid2(object e)
        {
            IsAddEnable = false;
            ReplyText = String.Empty;
            DataGrid datgrid = e as DataGrid;
            int count = 0;
            int cnt = 0;

            IList list = SelectedItem as IList;

            if (SelectedItem != null)
            {
                try
                {
                    List<ScripProfModel> SelectedItemsList = list.Cast<ScripProfModel>().ToList();
                    List<long> scripCodes = new List<long>();
                    scripCodes = selectedList.Select(x => x.ScripCode).ToList();
                    if (PopulateGrid2 == "Visible")
                    {
                        StringBuilder sb = null;
                        for (int i = 0; i < SelectedItemsList.Count; i++)
                        {
                            if (!(selectedList.Contains(SelectedItemsList[i])))
                            {
                                if (SelectBlankRow != null)
                                {
                                    LastData = selectedList.IndexOf(selectedList.FirstOrDefault(y => y.ScripCode == SelectBlankRow.ScripCode));
                                    selectedList.Insert(Convert.ToInt32(LastData + 1) - 1, SelectedItemsList[i]);
                                    count++;
                                    foreach (ScripProfModel oScripProfModel in selectedList)
                                    {
                                        oScripProfModel.PropertyChanged += OnElementPropertyChanged1;
                                    }
                                }
                                else
                                {
                                    selectedList.Add(SelectedItemsList[i]);
                                    count++;
                                }
                            }
                            else if (selectedList.Contains(SelectedItemsList[i]))
                            {
                                cnt++;
                            }
                        }

                        if (count > 0)
                        {
                            sb = new StringBuilder();
                            sb.Append(count);
                            sb.Append(" Scrips Added /");
                        }
                        if (cnt > 0)
                        {
                            sb = new StringBuilder();
                            sb.Append(cnt);
                            sb.Append(" Duplicate Scrip /");
                        }
                        if (cnt > 0 && count > 0)
                        {
                            sb = new StringBuilder();
                            sb.Append(cnt);
                            sb.Append(" Scrips Already Present / ");
                            sb.Append(count);
                            sb.Append(" Scrips Added /");
                        }
                        ReplyText = sb.ToString();
                        NotifyPropertyChanged("ReplyText");
                        if (IsSaveAsEnabled == false && IsUpdateChecked == true)
                        {
                            MessageBox.Show("Load file data before adding scrips  ", "Incorrect Selection", MessageBoxButton.OK, MessageBoxImage.Information);
                            IsAddEnable = true;
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Select Create or Update Button", "Incorrect Selection", MessageBoxButton.OK, MessageBoxImage.Information);
                        IsAddEnable = true;
                        return;
                    }

                    selectedList = new ObservableCollection<ScripProfModel>(selectedList.Distinct());
                    if (selectedList.Count == 0)
                    {
                        MessageBox.Show("No Scrips Selected");
                    }
                    int countBlank = ObjEquityDataCollection.Where(y => y.ScripCode == 0).Count();
                    if (AddFromGrid.ScripCode == 0 || countBlank > 0)
                    {
                        IsAddEnable = true;
                    }

                    list.Clear();
                    SelectedItemsList.Clear();
                    ReplyText += " Total Scrips Populated: " + selectedList.Count;
                    NotifyPropertyChanged("selectedList");
                    IsAddEnable = true;
                    IsDeselectVisible = "Hidden";
                    IsSelectAllVisible = "Visible";
                }
                catch (Exception)
                {
                    IsAddEnable = true;
                }
            }
            else
            {
                MessageBox.Show("Select scrip before adding ", "Incorrect Selection", MessageBoxButton.OK, MessageBoxImage.Information);
                IsAddEnable = true;
                return;
            }
        }

        public ScripProfilingVM()
        {
            oDataAccessLayer = new DataAccessLayer();
            oDataAccessLayer.Connect((int)DataAccessLayer.ConnectionDB.Masters);
            oDataAccessLayer1 = new DataAccessLayer();
            oDataAccessLayer1.Connect((int)DataAccessLayer.ConnectionDB.Masters);
            PopulatingExchangeDropDown();
#if TWS
            PopulatingExchangeDropDown();
            cmbProfileName = new ObservableCollection<string>();
            ScripSegmentLst = new List<string>();
            DerivativePType = new List<string>();
            CurrencyPType = new List<string>();
            FutureType = new List<string>();
            OptionType = new List<string>();
            ScripSet = new List<string>();
            ExPrd = new List<string>();
            CurrExpDateLst = new ObservableCollection<string>();
            TempDerStrikePrice = new BindingList<string>();
            ScripProfileSPRList = new ObservableCollection<ScripProfModel>();
            ObjEquityDataCollection = new ObservableCollection<ScripProfModel>();
            ObjDebtDataCollection = new ObservableCollection<ScripMasterDebtInfo>();
            ObjCurrencyDataCollection = new ObservableCollection<ScripProfModel>();//TODO TBD2017
            selectedList = new ObservableCollection<ScripProfModel>();
            ObjSPRDataCollection = new ObservableCollection<ScripMaster>();
            FileCombo = new ObservableCollection<string>();
            currencyScripList = new Dictionary<int, ScripProfModel>();////TODO TBD2017
            PopulatingExchangeDropDown();
            PopulatingScripProfileDropDowns();
            PopulatingColumnProfileDropDown();
            PopulatingScripGroupDropDowns();
            PopulatingDerivativeProdTypeDropDowns();
            PopulatingCurrencyProdTypeDropDowns();
            PopulatingOptionDropDowns();
            PopulatingFutureDropDowns();
            PopulatingScripSetDropDowns();
            PopulatingIndicesSetDropDowns();
            PopulatingExpiryDateDropDown();
            PopulatingCurrencyExpiryDateDropDown();
            PopulatingExpiryPrdDropDown();
            PopulatingCurrencyAssetDropDown();
            PopulatingEquity1Grid();
            CheckingVisiblity();
            isCmbProfileEnable = false;
            IsSearchEnable = true;
            IsEnabledProductTypeDerivative = false;
            IsEnabledProductTypeCurrency = false;
            IsvisibleProductTypeCurrency = "Hidden";
            IsvisibleProductTypeDerivative = "Visible";
            DerivativeAssetEnable = false;
            DerivativeAssestVisible = "Visible";
            CurrencyAssetEnable = false;
            CurrencyAssestVisible = "Hidden";
            ScrpGrpEnable = true;
            SelectedExPrd = " All";
            CurrStrikePriceEnable = false;
            DerStrikePriceEnable = false;
            DerStrikePriceVisible = "Visible";
            TempDerStrikePrice.Add(ScripProfilingModel.FutType._All.ToString().Replace("_", " "));
            DerStrikePriceSelected = TempDerStrikePrice[0];
            CurrStrikePriceVisible = "Hidden";
            CurrStrikePriceEnable = false;
            IsCreateChecked = true;
            IsSaveAsEnabled = false;
            IsSaveAsVisible = "Hidden";
            IsSaveEnabled = true;
            IsSaveVisible = "Visible";
            IsProfileComboVisible = "Hidden";
            PopulateFileNameGrid = "Hidden";
            PopulateGrid2 = "Visible";
            IsProfileComboVisible = "Visible";
            IsScripProfileEnable = false;
            IsBlankRowEnabled = true;
            IsAddEnable = true;
            IsColProfileVisible = "Visible";
            IsSaveColProfileEnabled = false;
            Is4L6LEnabledGrpBox = true;
            Is4LEnabledGrpBox = true;
            Is6LEnabledGrpBox = true;
            IsSegmentEnable = true;
            FileCombo.Add(ScripProfilingModel.ScripSet._Select.ToString().Replace("_", " "));
            SelectedScripProfileSPR = FileCombo[0];
            ScripProfileSPRList.Clear();

#elif BOW
            DerivativePType = new List<string>();
            CurrencyPType = new List<string>();
            FutureType = new List<string>();
            OptionType = new List<string>();
            ScripSet = new List<string>();
            ExPrd = new List<string>();
            selectedList = new ObservableCollection<ScripProfModel>();
            ObjSPRDataCollection = new ObservableCollection<ScripMaster>();
            FileCombo = new ObservableCollection<string>();
            IsSearchEnable = true;
            IsCreateChecked = true;
            IsSaveAsEnabled = false;
            IsSaveEnabled = true;
            IsSaveVisible = "Visible";
            IsProfileComboVisible = "Hidden";
            PopulateFileNameGrid = "Hidden";
            PopulateGrid2 = "Visible";
            IsProfileComboVisible = "Visible";
            IsScripProfileEnable = false;
            IsBlankRowEnabled = true;
            IsAddEnable = true;
            //ScripProfileSPRList.Clear();
            //VisibilityOfBowControls();
            // PopulateSegmentDropDown();
#endif
        }

        private static void PopulatingColumnProfileDropDown()
        {
#if TWS || BOW
            //cmbProfileName.Add(WindowName.Exchange_Default_Profile.ToString().Replace("_", " "));
            cmbProfileName.Add("DEFAULT");
            cmbProfileSelected = cmbProfileName[0];
#endif
        }

        private void PopulateSegmentDropDown()
        {
            if (SelectedExchange == ScripProfilingModel.Exchanges.MCX.ToString())
            {
                ScripSegmentLst = new List<string>();
                ScripSegmentLst.Add(ScripProfilingModel.ScripSegment.All.ToString());
                ScripSelectedSegment = ScripSegmentLst[0];
            }
        }

        private void VisibilityOfBowControls()
        {
            if (SelectedExchange == ScripProfilingModel.Exchanges.MCX.ToString())
            {
                ScrpGrpEnable = false;
                ExPrdEnable = false;
                IsEnabledProductTypeCurrency = false;
                IsvisibleProductTypeCurrency = "Hidden";
                IsvisibleProductTypeDerivative = "Visible";
                IsEnabledProductTypeDerivative = true;
                CurrencyAssestVisible = "Hidden";
                CurrencyAssetEnable = false;
                DerivativeAssestVisible = "Visible";
                DerivativeAssetEnable = true;
                CurrExpDateVisible = "Hidden";
                CurrExpDateEnable = false;
                DerExpDateEnable = true;
                DerExpDateVisible = "Visible";
                CurrStrikePriceEnable = false;
                CurrStrikePriceVisible = "Hidden";
                DerStrikePriceVisible = "Visible";
                DerStrikePriceEnable = true;
            }
        }

        private void PopulatingExchangeDropDown()
        {
            lstExchange = new List<string>();
#if BOW
            lstExchange.Add(ScripProfilingModel.Exchanges.NSE.ToString());
            lstExchange.Add(ScripProfilingModel.Exchanges.BSE.ToString());
            lstExchange.Add(ScripProfilingModel.Exchanges.MCX.ToString());

#elif TWS
            lstExchange.Add(ScripProfilingModel.Exchanges.BSE.ToString());
#endif
            SelectedExchange = ScripProfilingModel.Exchanges.BSE.ToString();
        }

        private void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //int SelectedCount = Convert.ToInt32(ScripProfileSPRList.All(el => el.IsSelected == true).ToString());
            //int count = Convert.ToInt32(selectedList.All(el => el.IsSelected == true).ToString());
            if (e.PropertyName == "IsSelected")
            {
                if (ScripProfileSPRList.All(el => el.IsSelected))
                {
                    AllSelected_set = false;
                    AllSelected = true;
                }
                if (ScripProfileSPRList.Any(el => !el.IsSelected))
                {
                    AllSelected_set = false;
                    AllSelected = false;
                }
            }
        }

        private void OnElementPropertyChanged1(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsSelected1")
            {
                if (selectedList.All(el => el.IsSelected1))
                {
                    AllSelectedForGrid2_Set = false;
                    AllSelectedForGrid2 = true;
                }
                if (selectedList.Any(el => !el.IsSelected1))
                {
                    AllSelectedForGrid2_Set = false;
                    AllSelectedForGrid2 = false;
                }
            }
        }

        private void ReadScripFiles()
        {
            try
            {
                string masterPath = CsvFilesPath.ToString();
                string filePattern = "*.csv";

                var coFiles = from fullFilename
                     in Directory.EnumerateFiles(masterPath, filePattern)
                              select Path.GetFileName(fullFilename);
                FileCombo.Add("Select");
                foreach (var item in coFiles)
                {
                    FileCombo.Add(item);
                    SelectedScripProfileSPR = FileCombo[0];
                }
                FileCombo.Distinct();
                // ReplyText = FileCombo.Count + " Scrips Populated";
            }
            catch (Exception e)
            { ExceptionUtility.LogError(e); }
        }

        private void CheckingVisiblity()
        {
            if (IsCreateChecked == true)
            {
                IsScripProfileEnable = false;
            }
        }

        private void ResetValues()
        {
            ScripSelectedSegment = ScripProfilingModel.ScripSegment.Equity.ToString();
            Is4L6LEnabled = true;
            SelectedScripGrp = ScripProfilingModel.FutType._All.ToString().Replace("_", " ");
            ProdTypeDerivativeSelected = ScripProfilingModel.DerivativeProdType._All.ToString().Replace("_", " ");
            ProdTypeCurrencySelected = ScripProfilingModel.CurrencyProdType.All.ToString();
            SelectedDerAsset = ScripProfilingModel.FutType._All.ToString().Replace("_", " ");
            SelectedCurrExpDate = ScripProfilingModel.FutType._All.ToString().Replace("_", " ");
            SelectedDerExpDate = ScripProfilingModel.FutType._All.ToString().Replace("_", " ");
            SelectedExPrd = ScripProfilingModel.ExpPrd.All.ToString();
            DerStrikePriceSelected = ScripProfilingModel.FutType._All.ToString().Replace("_", " ");
            FutTypeLabelVisible = "Hidden";
            OptTypeLabelVisible = "Visible";
            SelectedScripSet = ScripProfilingModel.ScripSet._Select.ToString().Replace("_", " ");
            SelectedIndicesSet = IndicesSet[0];
            //EnableDisable();
            Is4LChecked = false;
            Is6LChecked = false;
            IsSegmentEnable = true;
            ScrpGrpEnable = true;
            IsSearchEnable = true;
            Is4L6LEnabledGrpBox = true;
            Is4LEnabledGrpBox = true;
            Is6LEnabledGrpBox = true;
            //   PopulatingEquity1Grid();///for bolt rename it to PopulatingEquity1Grid()** for bow  fn would be PopulatingEquityGrid()
        }

        private void ResetValuesForScripAndIndicesSet()
        {
            ScripSelectedSegment = ScripProfilingModel.ScripSegment.Equity.ToString();
            //Is4L6LEnabled = true;
            SelectedScripGrp = ScripProfilingModel.FutType._All.ToString().Replace("_", " ");
            ProdTypeDerivativeSelected = ScripProfilingModel.DerivativeProdType._All.ToString().Replace("_", " ");
            ProdTypeCurrencySelected = ScripProfilingModel.CurrencyProdType.All.ToString();
            SelectedDerAsset = ScripProfilingModel.FutType._All.ToString().Replace("_", " ");
            SelectedCurrExpDate = ScripProfilingModel.FutType._All.ToString().Replace("_", " ");
            SelectedDerExpDate = ScripProfilingModel.FutType._All.ToString().Replace("_", " ");
            SelectedExPrd = ScripProfilingModel.ExpPrd.All.ToString();
            DerStrikePriceSelected = ScripProfilingModel.FutType._All.ToString().Replace("_", " ");
            FutTypeLabelVisible = "Hidden";
            OptTypeLabelVisible = "Visible";
            //EnableDisable();
            ScrpGrpEnable = false;
            IsSearchEnable = false;
            IsSegmentEnable = false;
        }

        private void OnChangeOfExpPrd()
        {
#if TWS
            if (SelectedExPrd == ScripProfilingModel.ExpPrd.Half_Yearly.ToString().Replace("_", " "))
                contractTyp = 'H';
            else if (SelectedExPrd == ScripProfilingModel.ExpPrd.Monthly.ToString())
                contractTyp = 'M';
            else if (SelectedExPrd == ScripProfilingModel.ExpPrd.Quaterly.ToString())
                contractTyp = 'Q';
            else if (SelectedExPrd == ScripProfilingModel.ExpPrd.Weekly.ToString())
                contractTyp = 'W';

            if (ScripSelectedSegment == ScripProfilingModel.ScripSegment.Derivative.ToString())
                if (ProdTypeDerivativeSelected == ScripProfilingModel.DerivativeProdType.Index_Option.ToString().Replace("_", " "))
                {
                    ExpDateLst.Clear();
                    ExpDateLst.Add(ScripProfilingModel.FutType._All.ToString().Replace("_", " "));
                    foreach (var item in MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Where(x => x.InstrumentType == "IO" && (x.OptionType == "PE" || x.OptionType == "CE") && ((DateTime.Parse(x.ExpiryDate) >= DateTime.Today)) && x.ContractType == contractTyp.ToString()).OrderBy(x => x.ExpiryDate).Select(x => DateTime.ParseExact(x.ExpiryDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture).ToString("yy-MMM-dd")).Distinct())
                    {
                        ExpDateLst.Add(item);
                    }

                    ExpDateLst = new ObservableCollection<string>(ExpDateLst.OrderBy(p => p));
                    var sortedDatelst = DisplayDateInComboBoxddMMMyyyy(ExpDateLst);
                    ExpDateLst.Clear();
                    ExpDateLst = sortedDatelst;

                    if (ExpDateLst != null)
                        SelectedDerExpDate = ExpDateLst[0];
                }
                else if (ProdTypeDerivativeSelected == ScripProfilingModel.DerivativeProdType.Stock_Option.ToString().Replace("_", " "))
                {
                    ExpDateLst.Clear();
                    ExpDateLst.Add(ScripProfilingModel.FutType._All.ToString().Replace("_", " "));
                    foreach (var item in MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Where(x => x.InstrumentType == "SO" || x.InstrumentType == "SF" && (x.OptionType == "PE" || x.OptionType == "CE") && ((DateTime.Parse(x.ExpiryDate) >= DateTime.Today)) && x.ContractType == contractTyp.ToString()).OrderBy(x => x.ExpiryDate).Select(x => DateTime.ParseExact(x.ExpiryDate, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("yy-MMM-dd")).Distinct())
                    {
                        ExpDateLst.Add(item);
                    }

                    ExpDateLst = new ObservableCollection<string>(ExpDateLst.OrderBy(p => p));
                    var sortedDatelst = DisplayDateInComboBoxddMMMyyyy(ExpDateLst);
                    ExpDateLst.Clear();
                    ExpDateLst = sortedDatelst;

                    if (ExpDateLst != null)
                        SelectedDerExpDate = ExpDateLst[0];
                }

#endif
        }

        private void PopulatingEquity1Grid()
        {
#if TWS
            IsSearchEnable = false;
            try
            {
                oDataAccessLayer.Connect((int)DataAccessLayer.ConnectionDB.Masters);
               oDataAccessLayer.OpenConnection((int)DataAccessLayer.ConnectionDB.Masters);
                ObjEquityDataCollection.Clear();
                Dictionary<long, ScripMasterBase> objDebtDetailsDic = new Dictionary<long, ScripMasterBase>();
                Dictionary<long, ScripMasterBase> objEquityDetailsDic = new Dictionary<long, ScripMasterBase>();
                Dictionary<long, DerivativeMasterBase> objDerivativeDetailsDic = new Dictionary<long, DerivativeMasterBase>();
                //Dictionary<Int64, DerivativeMaster> objDerivativeSpreadDetailsDic = new Dictionary<long, DerivativeMaster>();
                Dictionary<long, CurrencyMasterBase> objCurrencyDetailsDic = new Dictionary<long, CurrencyMasterBase>();
                //Dictionary<Int64, DerivativeMaster> objCurrencySpreadDetailsDic = new Dictionary<long, DerivativeMaster>();

                if (MasterSharedMemory.objMastertxtDictBaseBSE != null)
                {
                    objDebtDetailsDic = MasterSharedMemory.objMastertxtDictBaseBSE.Where(x => ((x.Value.InstrumentType == 'C' || x.Value.InstrumentType == 'D') && ((x.Value.ScripCode >= 100000 && x.Value.ScripCode < 200000) || (x.Value.ScripCode >= 500000 && x.Value.ScripCode < 600000) || (x.Value.ScripCode >= 700000)))).OrderBy(y => y.Value.ScripId).ToDictionary(x => x.Key, x => x.Value);
                    objEquityDetailsDic = MasterSharedMemory.objMastertxtDictBaseBSE.Where(x => (x.Value.InstrumentType == 'E') && ((x.Value.ScripCode >= 100000 && x.Value.ScripCode < 200000) || (x.Value.ScripCode >= 500000 && x.Value.ScripCode < 600000) || (x.Value.ScripCode >= 700000))).OrderBy(y => y.Value.ScripId).ToDictionary(x => x.Key, x => x.Value);
                }

                if (MasterSharedMemory.objMasterDerivativeDictBaseBSE != null)
                {
                    //objDerivativeDetailsDic = MasterSharedMemory.objMasterDicEqd_Co.Where(x => x.Value.ProductType == "IF" || x.Value.ProductType == "IO" || x.Value.ProductType == "SF" || x.Value.ProductType == "SO").ToDictionary(x => x.Key, x => x.Value);
                    objDerivativeDetailsDic = MasterSharedMemory.objMasterDerivativeDictBaseBSE.Where(x => x.Value.DerScripGroup == "DF").ToDictionary(x => x.Key, x => x.Value);
                }
                //if (CurrencyDerivativeMemory.objDerivativeCommon != null)
                //{
                //    //objDerivativeSpreadDetailsDic = MasterSharedMemory.objMasterDicEqdSpd_Co.Where(x => x.Value.ComplexInstrumentType == 2).ToDictionary(x => x.Key, x => x.Value);
                //    objDerivativeSpreadDetailsDic = CurrencyDerivativeMemory.objDerivativeCommon.Where(x => x.Value.DerScripGroup == "DF").ToDictionary(x => x.Key, x => x.Value);
                //}
                if (MasterSharedMemory.objMasterCurrencyDictBaseBSE != null)
                {
                    objCurrencyDetailsDic = MasterSharedMemory.objMasterCurrencyDictBaseBSE.Where(x => x.Value.CurrScripGroup == "CD").ToDictionary(x => x.Key, x => x.Value);
                }
                //if (CurrencyDerivativeMemory.objCurrencyCommon != null)
                //{
                //    objCurrencySpreadDetailsDic = CurrencyDerivativeMemory.objCurrencyCommon.Where(x => x.Value.CurrScripGroup == "CD").ToDictionary(x => x.Key, x => x.Value);
                //}
                if (ScripSelectedSegment == ScripProfilingModel.ScripSegment.Equity.ToString())
                {
                    string strQuery = "SELECT * FROM BSE_SECURITIES_CFE where InstrumentType = 'E' ";//exclude 4Land 6L series
                    char segment = 'E';
                    ObjEquityDataCollection.Clear();

                    if (SelectedScripGrp == ScripProfilingModel.FutType._All.ToString().Replace("_", " "))
                    {
                        str = Check4L6LScripsCheckedForAll(strQuery, segment);
                    }
                    else
                    {
                        str = Check4L6LScripsCheckedForSelectedItem(strQuery, segment);
                    }
                    ReadEquity(str);
                    lblCountContent = ObjEquityDataCollection.Count + " - Scrips";
                }
                else if (ScripSelectedSegment == ScripProfilingModel.ScripSegment.Debt.ToString())
                {
                    string strQuery = "SELECT * FROM BSE_SECURITIES_CFE where (( InstrumentType = 'C' ";
                    char segment = 'C';

                    if (SelectedScripGrp == ScripProfilingModel.FutType._All.ToString().Replace("_", " "))
                    {
                        str = Check4L6LScripsCheckedForAll(strQuery, segment);
                    }
                    else
                    {
                        str = Check4L6LScripsCheckedForSelectedItem(strQuery, segment);
                    }
                    ReadEquity(str);
                    lblCountContent = ObjEquityDataCollection.Count + " - Scrips";
                }
                else if (ScripSelectedSegment == ScripProfilingModel.ScripSegment.Currency.ToString())
                {
                    if (ProdTypeCurrencySelected == ScripProfilingModel.CurrencyProdType.Straddle.ToString())
                    {
                        try
                        {
                            Dictionary<long, CurrencyMasterBase> TempCurrencyDetailsDic = new Dictionary<long, CurrencyMasterBase>();
                            TempCurrencyDetailsDic = objCurrencyDetailsDic.Where(x => x.Value.StrategyID == 28 && x.Value.ComplexInstrumentType == 2).ToDictionary(x => x.Key, y => y.Value);
                            if (SelectedCurrAsset != " All")
                            {
                                TempCurrencyDetailsDic = TempCurrencyDetailsDic.Where(x => x.Value.UnderlyingAsset == SelectedCurrAsset).ToDictionary(x => x.Key, y => y.Value);
                            }
                            foreach (var item in TempCurrencyDetailsDic)
                            {
                                ScripProfModel currSpread = new ScripProfModel();
                                currSpread.ScripId = item.Value.InstrumentName;
                                currSpread.ScripCode = item.Value.ContractTokenNum;
                                currSpread.GroupName = "CD";
                                currSpread.ExpiryDate = item.Value.ExpiryDate;
                                currSpread.StrikePrice = String.Format("{0:0.0000}", Convert.ToDecimal(CommonFunctions.DisplayInDecimalFormatTouch(item.Value.StrikePrice, 7))).ToString();
                                if (currSpread.StrikePrice == "0.0000")
                                    currSpread.StrikePrice = string.Empty;
                                ObjEquityDataCollection.Add(currSpread);
                            }
                            lblCountContent = ObjEquityDataCollection.Count + " Scrips";
                        }
                        catch (Exception)
                        {
                            IsSearchEnable = true;
                        }
                    }
                    else if (ProdTypeCurrencySelected == ScripProfilingModel.CurrencyProdType.Pair_Option.ToString().Replace("_", " "))
                    {
                        try
                        {
                            Dictionary<long, CurrencyMasterBase> TempCurrencyDetailsDic = new Dictionary<long, CurrencyMasterBase>();
                            TempCurrencyDetailsDic = objCurrencyDetailsDic.Where(x => x.Value.StrategyID == 15 && x.Value.ComplexInstrumentType == 2).ToDictionary(x => x.Key, y => y.Value);
                            if (SelectedCurrAsset != " All")
                            {
                                TempCurrencyDetailsDic = TempCurrencyDetailsDic.Where(x => x.Value.UnderlyingAsset == SelectedCurrAsset).ToDictionary(x => x.Key, y => y.Value);
                            }
                            foreach (var item in TempCurrencyDetailsDic)
                            {
                                ScripProfModel currSpread = new ScripProfModel();
                                currSpread.ScripId = item.Value.InstrumentName;
                                currSpread.ScripCode = item.Value.ContractTokenNum;
                                currSpread.GroupName = "CD";
                                currSpread.ExpiryDate = item.Value.ExpiryDate;
                                currSpread.StrikePrice = CommonFunctions.DisplayInDecimalFormatTouch(item.Value.StrikePrice, 4);
                                if (currSpread.StrikePrice == "0.0000")
                                    currSpread.StrikePrice = string.Empty;
                                ObjEquityDataCollection.Add(currSpread);
                            }
                            lblCountContent = ObjEquityDataCollection.Count + " Scrips";
                        }
                        catch (Exception)
                        {
                            IsSearchEnable = true;
                        }
                    }
                    else if (ProdTypeCurrencySelected == ScripProfilingModel.CurrencyProdType.Future.ToString())
                    {
                        try
                        {
                            Dictionary<long, CurrencyMasterBase> TempCurrencyDetailsDic = new Dictionary<long, CurrencyMasterBase>();
                            TempCurrencyDetailsDic = objCurrencyDetailsDic.Where(x => x.Value.InstrumentType == "FUTIRD" || x.Value.InstrumentType == "FUTCUR" || x.Value.InstrumentType == "FUTIRT").ToDictionary(x => x.Key, y => y.Value);
                            if (SelectedCurrAsset != " All")
                            {
                                TempCurrencyDetailsDic = TempCurrencyDetailsDic.Where(x => x.Value.UnderlyingAsset == SelectedCurrAsset).ToDictionary(x => x.Key, y => y.Value);
                            }
                            if (SelectedFutType != " All")
                            {
                                if (SelectedFutType == "Normal Fut")
                                {
                                    TempCurrencyDetailsDic = TempCurrencyDetailsDic.Where(x => x.Value.OptionType == FutSelected && x.Value.StrategyID == -1).ToDictionary(x => x.Key, x => x.Value);
                                }
                                else
                                {
                                    TempCurrencyDetailsDic = TempCurrencyDetailsDic.Where(x => x.Value.ComplexInstrumentType == CalSelected).ToDictionary(x => x.Key, x => x.Value);
                                }
                            }
                            if (SelectedCurrExpDate != " All")
                            {
                                if (NewSelectedDerExpDate == SelectedCurrExpDate)
                                {
                                    TempCurrencyDetailsDic = TempCurrencyDetailsDic.Where(x => DateTime.ParseExact(x.Value.ExpiryDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture).ToString("dd-MMM-yy").ToLower() == SelectedCurrExpDate.ToLower()).ToDictionary(x => x.Key, x => x.Value);
                                }
                                else
                                {
                                    SelectedCurrExpDate = DateTime.ParseExact(SelectedCurrExpDate, "dd-MM-yy", CultureInfo.InvariantCulture).ToString("dd-MMM-yy");
                                    TempCurrencyDetailsDic = TempCurrencyDetailsDic.Where(x => DateTime.ParseExact(x.Value.ExpiryDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture).ToString("dd-MMM-yy").ToLower() == SelectedCurrExpDate.ToLower()).ToDictionary(x => x.Key, x => x.Value);
                                    NewSelectedDerExpDate = SelectedCurrExpDate;
                                }
                            }
                            if (CurrStrikePriceSelected != " All")
                            {
                                TempCurrencyDetailsDic = TempCurrencyDetailsDic.Where(x => x.Value.StrikePrice == Convert.ToInt32(CurrStrikePriceSelected)).ToDictionary(x => x.Key, x => x.Value);
                            }
                            foreach (var item in TempCurrencyDetailsDic)
                            {
                                ScripProfModel currSpread = new ScripProfModel();
                                currSpread.ScripId = item.Value.InstrumentName;
                                currSpread.ScripCode = item.Value.ContractTokenNum;
                                currSpread.GroupName = "CD";
                                currSpread.ExpiryDate = DateTime.ParseExact(item.Value.ExpiryDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture).ToString("dd-MM-yy");
                                ObjEquityDataCollection.Add(currSpread);
                            }
                            lblCountContent = ObjEquityDataCollection.Count + " Scrips";
                        }
                        catch (Exception)
                        {
                            IsSearchEnable = true;
                        }
                    }
                    else if (ProdTypeCurrencySelected == ScripProfilingModel.CurrencyProdType.Option.ToString())
                    {
                        try
                        {
                            Dictionary<long, CurrencyMasterBase> TempCurrencyDetailsDic = new Dictionary<long, CurrencyMasterBase>();
                            TempCurrencyDetailsDic = objCurrencyDetailsDic.Where(x => x.Value.InstrumentType == "OPTCUR" && x.Value.StrategyID == -1).ToDictionary(x => x.Key, y => y.Value);
                            if (SelectedCurrAsset != " All")
                            {
                                TempCurrencyDetailsDic = TempCurrencyDetailsDic.Where(x => x.Value.UnderlyingAsset == SelectedCurrAsset).ToDictionary(x => x.Key, y => y.Value);
                            }
                            if (SelectedOptionType != " All")
                            {
                                TempCurrencyDetailsDic = TempCurrencyDetailsDic.Where(x => x.Value.OptionType == optionTyp).ToDictionary(x => x.Key, x => x.Value);
                            }
                            if (SelectedCurrExpDate != " All")
                            {
                                if (NewSelectedDerExpDate == SelectedCurrExpDate)
                                {
                                    TempCurrencyDetailsDic = TempCurrencyDetailsDic.Where(x => DateTime.ParseExact(x.Value.ExpiryDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture).ToString("dd-MMM-yy").ToLower() == SelectedCurrExpDate.ToLower()).Distinct().ToDictionary(x => x.Key, x => x.Value);
                                }
                                else
                                {
                                    SelectedCurrExpDate = DateTime.ParseExact(SelectedCurrExpDate, "dd-MM-yy", CultureInfo.InvariantCulture).ToString("dd-MMM-yy");
                                    TempCurrencyDetailsDic = TempCurrencyDetailsDic.Where(x => DateTime.ParseExact(x.Value.ExpiryDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture).ToString("dd-MMM-yy").ToLower() == SelectedCurrExpDate.ToLower()).Distinct().ToDictionary(x => x.Key, x => x.Value);
                                    NewSelectedDerExpDate = SelectedCurrExpDate;
                                }
                            }
                            if (CurrStrikePriceSelected != " All" && SelectedCurrAsset != " All")
                            {
                                double SelStrikePrice = Convert.ToDouble(CurrStrikePriceSelected) * Math.Pow(10, 7);
                                TempCurrencyDetailsDic = TempCurrencyDetailsDic.Where(x => x.Value.StrikePrice == SelStrikePrice).ToDictionary(x => x.Key, x => x.Value);
                            }

                            foreach (var item in TempCurrencyDetailsDic)
                            {
                                ScripProfModel currSpread = new ScripProfModel();
                                currSpread.ScripId = item.Value.InstrumentName;
                                currSpread.ScripCode = item.Value.ContractTokenNum;
                                currSpread.GroupName = "CD";
                                currSpread.ExpiryDate = DateTime.ParseExact(item.Value.ExpiryDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture).ToString("dd-MM-yy");
                                currSpread.StrikePrice = String.Format("{0:0.0000}", Convert.ToDecimal(CommonFunctions.DisplayInDecimalFormatTouch(item.Value.StrikePrice, 7))).ToString();
                                if (currSpread.StrikePrice == "0.0000")
                                    currSpread.StrikePrice = string.Empty;
                                ObjEquityDataCollection.Add(currSpread);
                            }
                            lblCountContent = ObjEquityDataCollection.Count + " Scrips";
                        }
                        catch (Exception)
                        {
                            IsSearchEnable = true;
                        }
                    }
                    else if (ProdTypeCurrencySelected == ScripProfilingModel.OptType.All.ToString().Replace("_", " "))
                    {
                        try
                        {
                            Dictionary<long, CurrencyMasterBase> TempCurrencyDetailsDic = new Dictionary<long, CurrencyMasterBase>();
                            TempCurrencyDetailsDic = objCurrencyDetailsDic.Where(x => x.Value.InstrumentType == "FUTIRD" || x.Value.InstrumentType == "FUTCUR" || x.Value.InstrumentType == "FUTIRT" || x.Value.InstrumentType == "OPTCUR").OrderBy(x => x.Value.UnderlyingAsset).ToDictionary(x => x.Key, y => y.Value);
                            if (SelectedCurrAsset != " All")
                            {
                                TempCurrencyDetailsDic = TempCurrencyDetailsDic.Where(x => x.Value.UnderlyingAsset == SelectedCurrAsset).OrderBy(x => x.Value.UnderlyingAsset).ToDictionary(x => x.Key, y => y.Value);
                            }
                            foreach (var item in TempCurrencyDetailsDic)
                            {
                                ScripProfModel currSpread = new ScripProfModel();
                                currSpread.ScripId = item.Value.InstrumentName;
                                currSpread.ScripCode = item.Value.ContractTokenNum;
                                currSpread.GroupName = "CD";
                                currSpread.ExpiryDate = DateTime.ParseExact(item.Value.ExpiryDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture).ToString("dd-MM-yy");// item.Value.ExpiryDate;
                                currSpread.StrikePrice = String.Format("{0:0.0000}", Convert.ToDecimal(CommonFunctions.DisplayInDecimalFormatTouch(item.Value.StrikePrice, 7))).ToString();
                                if (currSpread.StrikePrice == "0.0000")
                                    currSpread.StrikePrice = string.Empty;
                                ObjEquityDataCollection.Add(currSpread);
                            }
                            lblCountContent = ObjEquityDataCollection.Count + " Scrips";
                        }
                        catch (Exception)
                        {
                            IsSearchEnable = true;
                        }
                    }
                    else
                    {
                        try
                        {
                            Dictionary<long, CurrencyMasterBase> TempCurrencyDetailsDic = new Dictionary<long, CurrencyMasterBase>();
                            TempCurrencyDetailsDic = objCurrencyDetailsDic.Where(x => x.Value.CurrScripGroup == "CD").ToDictionary(x => x.Key, y => y.Value);
                            foreach (var item in TempCurrencyDetailsDic)
                            {
                                ScripProfModel currSpread = new ScripProfModel();
                                currSpread.ScripId = item.Value.InstrumentName;
                                currSpread.ScripCode = item.Value.ContractTokenNum;
                                currSpread.GroupName = "CD";
                                currSpread.ExpiryDate = DateTime.ParseExact(item.Value.ExpiryDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture).ToString("dd-MM-yy"); //item.Value.ExpiryDate;
                                currSpread.StrikePrice = String.Format("{0:0.0000}", Convert.ToDecimal(CommonFunctions.DisplayInDecimalFormatTouch(item.Value.StrikePrice, 7))).ToString();
                                if (currSpread.StrikePrice == "0.0000")
                                    currSpread.StrikePrice = string.Empty;
                                ObjEquityDataCollection.Add(currSpread);
                            }
                            ObjEquityDataCollection.Remove(ObjEquityDataCollection.Where(x => x.StrikePrice == "0").SingleOrDefault());
                            lblCountContent = ObjEquityDataCollection.Count + " Scrips";
                        }
                        catch (Exception)
                        {
                            IsSearchEnable = true;
                        }
                    }
                }
                else if (ScripSelectedSegment == ScripProfilingModel.ScripSegment.Derivative.ToString())
                {
                    string pTypeDerivativeSelected = String.Empty;
                    string DerivativeAssetSel = String.Empty;
                    string futureSel = String.Empty;
                    Dictionary<long, DerivativeMasterBase> TempDerivativeDetailsDict = new Dictionary<long, DerivativeMasterBase>();
                    if (ProdTypeDerivativeSelected != " All")
                    {
                        if (DerProdSelected == "IF" || DerProdSelected == "SF")
                        {
                            try
                            {
                                TempDerivativeDetailsDict = objDerivativeDetailsDic.Where(x => x.Value.InstrumentType == DerProdSelected).ToDictionary(x => x.Key, x => x.Value);
                                if (SelectedDerAsset != " All")
                                {
                                    TempDerivativeDetailsDict = TempDerivativeDetailsDict.Where(x => x.Value.UnderlyingAsset == SelectedDerAsset).ToDictionary(x => x.Key, x => x.Value);
                                }
                                if (SelectedFutType != " All")
                                {
                                    if (SelectedFutType == "Normal Fut")
                                    {
                                        TempDerivativeDetailsDict = TempDerivativeDetailsDict.Where(x => x.Value.OptionType == FutSelected && x.Value.StrategyID == -1).ToDictionary(x => x.Key, x => x.Value);
                                    }
                                    else
                                    {
                                        TempDerivativeDetailsDict = TempDerivativeDetailsDict.Where(x => x.Value.ComplexInstrumentType == CalSelected).ToDictionary(x => x.Key, x => x.Value);
                                    }
                                }
                                if (SelectedDerExpDate != " All")
                                {
                                    if (NewSelectedDerExpDate == SelectedDerExpDate)
                                    {
                                        TempDerivativeDetailsDict = TempDerivativeDetailsDict.Where(x => DateTime.ParseExact(x.Value.ExpiryDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture).ToString("dd-MM-yy").ToLower() == SelectedDerExpDate.ToLower()).ToDictionary(x => x.Key, x => x.Value);//dd-MMM-yyyy
                                    }
                                    else
                                    {
                                        SelectedDerExpDate = DateTime.ParseExact(SelectedDerExpDate, "dd-MM-yy", CultureInfo.InvariantCulture).ToString("dd-MM-yy");
                                        TempDerivativeDetailsDict = TempDerivativeDetailsDict.Where(x => DateTime.ParseExact(x.Value.ExpiryDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture).ToString("dd-MM-yy").ToLower() == SelectedDerExpDate.ToLower()).ToDictionary(x => x.Key, x => x.Value);
                                        NewSelectedDerExpDate = SelectedDerExpDate;
                                    }

                                }
                                foreach (var item in TempDerivativeDetailsDict)
                                {
                                    ScripProfModel sc = new ScripProfModel();
                                    sc.ScripId = item.Value.InstrumentName;
                                    sc.ScripCode = item.Value.ContractTokenNum;
                                    sc.GroupName = "DF";
                                    sc.ExpiryDate = DateTime.ParseExact(item.Value.ExpiryDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture).ToString("dd-MM-yy");
                                    ObjEquityDataCollection.Add(sc);
                                }
                                lblCountContent = ObjEquityDataCollection.Count + " Scrips";
                            }
                            catch (Exception ex)
                            {
                                IsSearchEnable = true;
                            }
                        }
                        else if (DerProdSelected == "IO" || DerProdSelected == "SO")
                        {
                            try
                            {

                                TempDerivativeDetailsDict = objDerivativeDetailsDic.Where(x => x.Value.InstrumentType == DerProdSelected).ToDictionary(x => x.Key, x => x.Value);
                                if (SelectedDerAsset != " All")
                                {
                                    TempDerivativeDetailsDict = TempDerivativeDetailsDict.Where(x => x.Value.UnderlyingAsset == SelectedDerAsset).ToDictionary(x => x.Key, x => x.Value);
                                }
                                if (SelectedOptionType != " All")
                                {
                                    TempDerivativeDetailsDict = TempDerivativeDetailsDict.Where(x => x.Value.OptionType == optionTyp).ToDictionary(x => x.Key, x => x.Value);
                                }
                                if (SelectedExPrd != " All")
                                {
                                    TempDerivativeDetailsDict = TempDerivativeDetailsDict.Where(x => x.Value.ContractType == contractTyp.ToString()).ToDictionary(x => x.Key, x => x.Value);
                                }
                                if (SelectedDerExpDate != " All")
                                {
                                    if (NewSelectedDerExpDate == SelectedDerExpDate)
                                    {
                                        TempDerivativeDetailsDict = TempDerivativeDetailsDict.Where(x => DateTime.ParseExact(x.Value.ExpiryDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture).ToString("dd-MMM-yy").ToLower() == SelectedDerExpDate.ToLower()).ToDictionary(x => x.Key, x => x.Value);
                                    }
                                    else
                                    {
                                        SelectedDerExpDate = DateTime.ParseExact(SelectedDerExpDate, "dd-MM-yy", CultureInfo.InvariantCulture).ToString("dd-MMM-yy");
                                        TempDerivativeDetailsDict = TempDerivativeDetailsDict.Where(x => DateTime.ParseExact(x.Value.ExpiryDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture).ToString("dd-MMM-yy").ToLower() == SelectedDerExpDate.ToLower()).ToDictionary(x => x.Key, x => x.Value);
                                        NewSelectedDerExpDate = SelectedDerExpDate;
                                    }

                                }
                                if (DerStrikePriceSelected != " All")
                                {
                                    TempDerivativeDetailsDict = TempDerivativeDetailsDict.Where(x => x.Value.StrikePrice == Convert.ToInt32(DerStrikePriceSelected) * 100).ToDictionary(x => x.Key, x => x.Value);
                                }

                                foreach (var item in TempDerivativeDetailsDict)
                                {
                                    ScripProfModel sc = new ScripProfModel();
                                    sc.ScripId = item.Value.InstrumentName;
                                    sc.ScripCode = item.Value.ContractTokenNum;
                                    sc.GroupName = "DF";
                                    sc.ExpiryDate = DateTime.ParseExact(item.Value.ExpiryDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture).ToString("dd-MM-yy");
                                    sc.StrikePrice = CommonFunctions.DisplayInDecimalFormatTouch(item.Value.StrikePrice, 2);
                                    if (sc.StrikePrice == "0.00" || sc.StrikePrice == "0")
                                        sc.StrikePrice = string.Empty;
                                    ObjEquityDataCollection.Add(sc);
                                }
                                lblCountContent = ObjEquityDataCollection.Count + " Scrips";
                            }
                            catch (Exception)
                            {
                                IsSearchEnable = true;
                            }
                        }
                        else if (DerProdSelected == "PO" || ProdTypeDerivativeSelected == ScripProfilingModel.DerivativeProdType.Pair_Option.ToString().Replace("_", " "))
                        {
                            try
                            {
                                TempDerivativeDetailsDict = objDerivativeDetailsDic.Where(x => x.Value.ComplexInstrumentType == 2).ToDictionary(x => x.Key, x => x.Value);
                                if (SelectedDerAsset != " All")
                                {
                                    TempDerivativeDetailsDict = TempDerivativeDetailsDict.Where(x => x.Value.UnderlyingAsset == SelectedDerAsset).ToDictionary(x => x.Key, x => x.Value);
                                }
                                foreach (var item in TempDerivativeDetailsDict)
                                {
                                    ScripProfModel sc = new ScripProfModel();
                                    sc.ScripId = item.Value.InstrumentName;
                                    sc.ScripCode = item.Value.ContractTokenNum;
                                    sc.GroupName = "DF";
                                    sc.ExpiryDate = DateTime.ParseExact(item.Value.ExpiryDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture).ToString("dd-MM-yy");
                                    sc.StrikePrice = CommonFunctions.DisplayInDecimalFormatTouch(item.Value.StrikePrice, 2);
                                    if (sc.StrikePrice == "0.00" || sc.StrikePrice == "0")
                                        sc.StrikePrice = string.Empty;
                                    ObjEquityDataCollection.Add(sc);
                                }
                                lblCountContent = ObjEquityDataCollection.Count + " Scrips";
                            }
                            catch (Exception)
                            {
                                IsSearchEnable = true;
                            }
                        }
                    }
                    else if (ProdTypeDerivativeSelected == " All")
                    {
                        try
                        {
                            TempDerivativeDetailsDict = objDerivativeDetailsDic.Where(x => x.Value.InstrumentType == "IF" || x.Value.InstrumentType == "IO" || x.Value.InstrumentType == "SF" || x.Value.InstrumentType == "SO").ToDictionary(x => x.Key, x => x.Value);
                            if (SelectedDerAsset != " All")
                            {
                                TempDerivativeDetailsDict = TempDerivativeDetailsDict.Where(x => x.Value.AssetCode == SelectedDerAsset).ToDictionary(x => x.Key, x => x.Value);
                            }
                            foreach (var item in TempDerivativeDetailsDict)
                            {
                                ScripProfModel sc = new ScripProfModel();
                                sc.ScripId = item.Value.InstrumentName;
                                sc.ScripCode = item.Value.ContractTokenNum;
                                sc.GroupName = "DF";
                                sc.ExpiryDate = DateTime.ParseExact(item.Value.ExpiryDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture).ToString("dd-MM-yy");
                                sc.StrikePrice = CommonFunctions.DisplayInDecimalFormatTouch(item.Value.StrikePrice, 2);
                                if (sc.StrikePrice == "0.00" || sc.StrikePrice == "0")
                                    sc.StrikePrice = string.Empty;
                                ObjEquityDataCollection.Add(sc);
                            }
                            lblCountContent = ObjEquityDataCollection.Count + " Scrips";
                        }
                        catch (Exception)
                        {
                            IsSearchEnable = true;
                        }
                    }
                }
                else if (ScripSelectedSegment == ScripProfilingModel.ScripSegment.All.ToString())
                {
                    foreach (var item in objEquityDetailsDic)
                    {
                        ScripProfModel scripmasterEquity = new ScripProfModel();
                        scripmasterEquity.ScripId = item.Value.ScripId;
                        scripmasterEquity.ScripCode = item.Value.ScripCode;
                        scripmasterEquity.GroupName = item.Value.GroupName;
                        ObjEquityDataCollection.Add(scripmasterEquity);
                    }
                    foreach (var item in objDebtDetailsDic)
                    {
                        ScripProfModel scripmasterEquity = new ScripProfModel();
                        scripmasterEquity.ScripId = item.Value.ScripId;
                        scripmasterEquity.ScripCode = item.Value.ScripCode;
                        scripmasterEquity.GroupName = item.Value.GroupName;
                        ObjEquityDataCollection.Add(scripmasterEquity);
                    }
                    foreach (var item in objCurrencyDetailsDic)
                    {
                        ScripProfModel scbfx = new ScripProfModel();
                        scbfx.ScripId = item.Value.InstrumentName;
                        scbfx.ScripCode = item.Value.ContractTokenNum;
                        scbfx.GroupName = item.Value.CurrScripGroup;
                        scbfx.ExpiryDate = DateTime.ParseExact(item.Value.ExpiryDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture).ToString("dd-MM-yy"); //item.Value.ExpiryDate;
                        scbfx.StrikePrice = String.Format("{0:0.0000}", Convert.ToDecimal(CommonFunctions.DisplayInDecimalFormatTouch(item.Value.StrikePrice, 7))).ToString();
                        if (scbfx.StrikePrice == "0.0000")
                            scbfx.StrikePrice = string.Empty;
                        ObjEquityDataCollection.Remove(ObjEquityDataCollection.Where(x => (x.StrikePrice == "0")).SingleOrDefault());
                    }
                    foreach (var item in objCurrencyDetailsDic.Where(x => x.Value.StrategyID == 28))
                    {
                        ScripProfModel scbfxSpd = new ScripProfModel();
                        scbfxSpd.ScripId = item.Value.InstrumentName;
                        scbfxSpd.ScripCode = item.Value.ContractTokenNum;
                        scbfxSpd.GroupName = item.Value.CurrScripGroup;
                        scbfxSpd.ExpiryDate = item.Value.ExpiryDate;
                        ObjEquityDataCollection.Add(scbfxSpd);
                    }
                    foreach (var item in objDerivativeDetailsDic)
                    {
                        ScripProfModel scpder = new ScripProfModel();
                        scpder.ScripId = item.Value.InstrumentName;
                        scpder.ScripCode = item.Value.ContractTokenNum;
                        scpder.GroupName = item.Value.DerScripGroup;
                        scpder.ExpiryDate = DateTime.ParseExact(item.Value.ExpiryDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture).ToString("dd-MM-yy");// item.Value.ExpiryDate;
                        scpder.StrikePrice = CommonFunctions.DisplayInDecimalFormatTouch(item.Value.StrikePrice, 2);
                        if (scpder.StrikePrice == "0.00" || scpder.StrikePrice == "0")
                            scpder.StrikePrice = string.Empty;
                        ObjEquityDataCollection.Add(scpder);
                    }
                    foreach (var item in objDerivativeDetailsDic.Where(x => x.Value.StrategyID == 28))
                    {
                        ScripProfModel scpderSpd = new ScripProfModel();
                        scpderSpd.ScripId = item.Value.InstrumentName;
                        scpderSpd.ScripCode = item.Value.ContractTokenNum;
                        scpderSpd.GroupName = item.Value.DerScripGroup;
                        scpderSpd.ExpiryDate = DateTime.ParseExact(item.Value.ExpiryDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture).ToString("dd-MM-yy");//item.Value.ExpiryDate;
                        ObjEquityDataCollection.Add(scpderSpd);
                    }

                    //#region Date Sorting
                    //String temp = String.Empty;
                    //for (int i = 1; i <= ObjEquityDataCollection.Count; i++)
                    //{
                    //    for (int j = i + 1; j < ObjEquityDataCollection.Count; j++)
                    //    {
                    //        int res = CommonFunctions.CompareDateddMMYY(ObjEquityDataCollection[i].ToString(), ObjEquityDataCollection[j].ToString());
                    //        if (res == 1 || res == 2 || res == 5 || res == 0)
                    //        {
                    //            ObjEquityDataCollection[i].temp = ObjEquityDataCollection[j].ExpiryDate;
                    //            ObjEquityDataCollection[j].ExpiryDate = ObjEquityDataCollection[i].ExpiryDate;
                    //            ObjEquityDataCollection[i].ExpiryDate = ObjEquityDataCollection[i].temp;
                    //        }
                    //    }
                    //}
                    //#endregion

                    lblCountContent = ObjEquityDataCollection.Count + " Scrips";
                }
                ObjEquityDataCollection.GroupBy(x => x.ScripId);
                TempEquityDataCollection = ObjEquityDataCollection;
                DemoDataCollection = TempEquityDataCollection;
                IsSearchEnable = true;
            }
            catch (Exception e) { ExceptionUtility.LogError(e); }
            finally
            {
                oDataAccessLayer.CloseConnection((int)DataAccessLayer.ConnectionDB.Masters);
                System.Data.SQLite.SQLiteConnection.ClearAllPools();
            }
#endif
        }

        private string Check4L6LScripsCheckedForSelectedItem(string strQuery, char segment)
        {
#if TWS
            #region Adding Scrips of Series 4L and 6L
            if (Is6LChecked == true && Is4LChecked == true)
            {
                if (segment == 'E')
                {
                    strQuery += " AND ((ScripCode BETWEEN '100000' AND '200000') OR (ScripCode >= '500000')) AND " + ScripMasterEnum.GroupName + " = '" + SelectedScripGrp + "'" + ";";
                }
                else
                {
                    strQuery += "AND " + ScripMasterEnum.GroupName + " = '" + SelectedScripGrp + "')) AND ((ScripCode BETWEEN '100000' AND '200000') OR ScripCode >= '400000') OR ((InstrumentType = 'D'  AND " + ScripMasterEnum.GroupName + " = '" + SelectedScripGrp + "') AND (((ScripCode BETWEEN '100000' AND '200000') OR ScripCode >= '400000')));";
                }
            }
            else if (Is6LChecked == true)
            {
                if (segment == 'E')
                {
                    strQuery += " AND ((ScripCode BETWEEN '100000' AND '200000') OR (ScripCode >= '500000')) AND " + ScripMasterEnum.GroupName + " = '" + SelectedScripGrp + "'" + ";";
                }
                else
                {
                    strQuery += "  AND " + ScripMasterEnum.GroupName + " = '" + SelectedScripGrp + "')) AND ((ScripCode BETWEEN '100000' AND '200000') OR (ScripCode >= '500000')) OR ((InstrumentType = 'D' AND " + ScripMasterEnum.GroupName + " = '" + SelectedScripGrp + "') AND ((ScripCode BETWEEN '100000' AND '200000') OR (ScripCode >= '500000')));";
                }
            }
            else if (Is4LChecked == true)
            {
                if (segment == 'E')
                {
                    strQuery += " AND ((ScripCode BETWEEN '100000' AND '200000') OR (ScripCode BETWEEN '400000' AND '600000') OR (ScripCode >= '700000')) AND " + ScripMasterEnum.GroupName + " = '" + SelectedScripGrp + "'" + ";";
                }
                else
                {
                    strQuery += " AND " + ScripMasterEnum.GroupName + " = '" + SelectedScripGrp + "')) AND ((ScripCode BETWEEN '100000' AND '200000') OR (ScripCode >= '400000')) OR ((InstrumentType = 'D' AND " + ScripMasterEnum.GroupName + " = '" + SelectedScripGrp + "') AND ((ScripCode BETWEEN '100000' AND '200000') OR (ScripCode >= '400000')));";
                }
            }
            else
            {
                if (segment == 'E')
                {
                    strQuery += " AND ((ScripCode BETWEEN '100000' AND '200000') OR (ScripCode BETWEEN '500000' AND '600000') OR (ScripCode >= '700000')) AND " + ScripMasterEnum.GroupName + " = '" + SelectedScripGrp + "'" + ";";
                }
                else
                {
                    strQuery += " OR InstrumentType = 'D') AND (" + ScripMasterEnum.GroupName + " = '" + SelectedScripGrp + "')) AND ((ScripCode BETWEEN '100000' AND '200000') OR (ScripCode BETWEEN '500000' AND '600000') OR (ScripCode >= '700000'));";
                }
            }

            #endregion
#endif
            return strQuery;
        }

        private string Check4L6LScripsCheckedForAll(string strQuery, char segment)
        {
#if TWS
            #region Adding Scrips of Series 4L and 6L for All Case
            if (Is6LChecked == true && Is4LChecked == true)
            {
                if (segment == 'E')
                {
                    strQuery += " AND ((ScripCode BETWEEN '100000' AND '200000') OR (ScripCode >= '500000'));";
                }
                else
                {
                    strQuery += " )) AND ((ScripCode BETWEEN '100000' AND '200000') OR ScripCode >= '400000') OR (InstrumentType = 'D' AND ((ScripCode BETWEEN '100000' AND '200000') OR ScripCode >= '400000'));";
                }
            }
            else if (Is6LChecked == true)
            {
                if (segment == 'E')
                {
                    strQuery += " AND ((ScripCode BETWEEN '100000' AND '200000') OR (ScripCode >= '500000'));";
                }
                else
                {
                    strQuery += " )) AND ((ScripCode BETWEEN '100000' AND '200000') OR ScripCode >= '500000') OR (InstrumentType = 'D' AND ((ScripCode BETWEEN '100000' AND '200000') OR (ScripCode >= '500000')));";
                }
            }
            else if (Is4LChecked == true)
            {
                if (segment == 'E')
                {
                    strQuery += " AND ((ScripCode BETWEEN '100000' AND '200000') OR (ScripCode BETWEEN '400000' AND '600000') OR (ScripCode >= '700000'));";
                }
                else
                {
                    strQuery += " )) AND ((ScripCode BETWEEN '100000' AND '200000') OR (ScripCode >= '400000')) OR (InstrumentType = 'D' AND ((ScripCode BETWEEN '100000' AND '200000') OR (ScripCode >= '400000')));";
                }
            }
            else
            {
                if (segment == 'E')
                {
                    strQuery += " AND ((ScripCode BETWEEN '100000' AND '200000') OR (ScripCode BETWEEN '500000' AND '600000') OR (ScripCode >= '700000'));";
                }
                else
                {
                    strQuery += " OR InstrumentType = 'D')) AND ((ScripCode BETWEEN '100000' AND '200000') OR (ScripCode BETWEEN '500000' AND '600000') OR (ScripCode >= '700000'));";
                }
            }

            #endregion
#endif
            return strQuery;
        }

        private void PopulatingEquityGrid()//for bow
        {
            IsSearchEnable = false;
            try
            {
                ObjEquityDataCollection.Clear();
                Dictionary<long, ScripMaster> objDebtDetailsDic = new Dictionary<long, ScripMaster>();
                Dictionary<long, ScripMaster> objEquityDetailsDic = new Dictionary<long, ScripMaster>();
                Dictionary<long, DerivativeMaster> objDerivativeDetailsDic = new Dictionary<long, DerivativeMaster>();
                Dictionary<Int64, DerivativeMaster> objDerivativeSpreadDetailsDic = new Dictionary<long, DerivativeMaster>();
                Dictionary<long, CurrencyMasterBase> objCurrencyDetailsDic = new Dictionary<long, CurrencyMasterBase>();
                Dictionary<Int64, DerivativeMaster> objCurrencySpreadDetailsDic = new Dictionary<long, DerivativeMaster>();
                // TODO TBD2017
                //if (MasterSharedMemory.objMastertxtDict != null)
                //{
                //    objDebtDetailsDic = MasterSharedMemory.objMastertxtDict.Where(x => ((x.Value.InstrumentType == 'C' || x.Value.InstrumentType == 'D') && ((x.Value.ScripCode >= 100000 && x.Value.ScripCode < 200000) || (x.Value.ScripCode >= 500000 && x.Value.ScripCode < 600000) || (x.Value.ScripCode >= 700000)))).OrderBy(y => y.Value.ScripId).ToDictionary(x => x.Key, x => x.Value);
                //    objEquityDetailsDic = MasterSharedMemory.objMastertxtDict.Where(x => (x.Value.InstrumentType == 'E') && ((x.Value.ScripCode >= 100000 && x.Value.ScripCode < 200000) || (x.Value.ScripCode >= 500000 && x.Value.ScripCode < 600000) || (x.Value.ScripCode >= 700000))).OrderBy(y => y.Value.ScripId).ToDictionary(x => x.Key, x => x.Value);
                //}
                //if(MasterSharedMemory.objMasterCurrencyDictBaseBSE != null)
                //{
                //    //objDerivativeDetailsDic = MasterSharedMemory.objMasterDicEqd_Co.Where(x => x.Value.ProductType == "IF" || x.Value.ProductType == "IO" || x.Value.ProductType == "SF" || x.Value.ProductType == "SO").ToDictionary(x => x.Key, x => x.Value);
                //    objDerivativeDetailsDic = CurrencyDerivativeMemory.objDerivativeCommon.Where(x => x.Value.DerScripGroup == "DF").ToDictionary(x => x.Key, x => x.Value);
                //}
                //if (CurrencyDerivativeMemory.objDerivativeCommon != null)
                //{
                //    //objDerivativeSpreadDetailsDic = MasterSharedMemory.objMasterDicEqdSpd_Co.Where(x => x.Value.ComplexInstrumentType == 2).ToDictionary(x => x.Key, x => x.Value);
                //    objDerivativeSpreadDetailsDic = CurrencyDerivativeMemory.objDerivativeCommon.Where(x => x.Value.DerScripGroup == "DF").ToDictionary(x => x.Key, x => x.Value);
                //}
                if (MasterSharedMemory.objMasterCurrencyDictBaseBSE != null)
                {
                    objCurrencyDetailsDic = MasterSharedMemory.objMasterCurrencyDictBaseBSE.Where(x => x.Value.ScripGroup == "CD").ToDictionary(x => x.Key, x => x.Value);
                }
                //if (CurrencyDerivativeMemory.objCurrencyCommon != null)
                //{
                //    objCurrencySpreadDetailsDic = CurrencyDerivativeMemory.objCurrencyCommon.Where(x => x.Value.CurrScripGroup == "CD").ToDictionary(x => x.Key, x => x.Value);
                //}
                if (ScripSelectedSegment == ScripProfilingModel.ScripSegment.Equity.ToString())
                {
                    ObjEquityDataCollection.Clear();
                    #region Adding Scrips of Series 4L and 6L
                    if (Is6LChecked == true && Is4LChecked == true)
                    { //TODO TBD2017
                      //objEquityDetailsDic = MasterSharedMemory.objMastertxtDic.Where(x => (x.Value.SegmentFlag == 'C' && (x.Value.ScripCode >= 400000 && x.Value.ScripCode < 800000)) || (x.Value.SegmentFlag == 'E' && ((x.Value.ScripCode >= 100000 && x.Value.ScripCode < 200000) || (x.Value.ScripCode >= 500000)))).OrderBy(y => y.Value.ScripId).ToDictionary(x => x.Key, x => x.Value);
                      // objEquityDetailsDic = MasterSharedMemory.objMastertxtDict.Where(x => (x.Value.InstrumentType == 'E' && ((x.Value.ScripCode >= 100000 && x.Value.ScripCode < 200000) || (x.Value.ScripCode >= 500000)))).OrderBy(y => y.Value.ScripId).ToDictionary(x => x.Key, x => x.Value);
                    }
                    else if (Is6LChecked == true)
                    { //TODO TBD2017
                      // objEquityDetailsDic = MasterSharedMemory.objMastertxtDict.Where(x => (x.Value.InstrumentType == 'E') && ((x.Value.ScripCode >= 100000 && x.Value.ScripCode < 200000) || (x.Value.ScripCode >= 500000))).OrderBy(y => y.Value.ScripId).ToDictionary(x => x.Key, x => x.Value);
                    }
                    else if (Is4LChecked == true)
                    { //TODO TBD2017
                      //  objEquityDetailsDic = MasterSharedMemory.objMastertxtDict.Where(x => (x.Value.InstrumentType == 'E' && ((x.Value.ScripCode >= 100000 && x.Value.ScripCode < 200000) || (x.Value.ScripCode >= 400000 && x.Value.ScripCode < 600000) || (x.Value.ScripCode >= 700000)))).OrderBy(y => y.Value.ScripId).ToDictionary(x => x.Key, x => x.Value);
                    }
                    #endregion

                    if (SelectedScripGrp == ScripProfilingModel.FutType._All.ToString().Replace("_", " "))
                    {
                        foreach (var item in objEquityDetailsDic)
                        {
                            ScripProfModel scripmasterEquity = new ScripProfModel();
                            scripmasterEquity.ScripId = item.Value.ScripId;
                            scripmasterEquity.ScripCode = item.Value.ScripCode;
                            scripmasterEquity.GroupName = item.Value.GroupName;
                            ObjEquityDataCollection.Add(scripmasterEquity);
                        }
                    }
                    else
                    {
                        Dictionary<long, ScripMaster> TempDict = new Dictionary<long, ScripMaster>();
                        TempDict = objEquityDetailsDic.Where(x => x.Value.GroupName == SelectedScripGrp).ToDictionary(x => x.Key, x => x.Value);

                        foreach (var item in TempDict)
                        {
                            ScripProfModel scripmasterEquity = new ScripProfModel();
                            scripmasterEquity.ScripId = item.Value.ScripId;
                            scripmasterEquity.ScripCode = item.Value.ScripCode;
                            scripmasterEquity.GroupName = item.Value.GroupName;
                            ObjEquityDataCollection.Add(scripmasterEquity);
                        }
                    }
                    lblCountContent = ObjEquityDataCollection.Count + " - Scrips";
                }
                else if (ScripSelectedSegment == ScripProfilingModel.ScripSegment.Debt.ToString())
                {
                    #region Adding Scrips of Series 4L and 6L
                    if (Is6LChecked == true && Is4LChecked == true)
                    { //TODO TBD2017
                      // objDebtDetailsDic = MasterSharedMemory.objMastertxtDict.Where(x => (x.Value.InstrumentType == 'C' && ((x.Value.ScripCode >= 100000 && x.Value.ScripCode < 200000) || x.Value.ScripCode >= 400000) || (x.Value.InstrumentType == 'D' && (((x.Value.ScripCode >= 100000 && x.Value.ScripCode < 200000) || x.Value.ScripCode >= 400000))))).OrderBy(y => y.Value.ScripId).ToDictionary(x => x.Key, x => x.Value);
                    }
                    else if (Is6LChecked == true)
                    { //TODO TBD2017
                        //objDebtDetailsDic = MasterSharedMemory.objMastertxtDic.Where(x => (x.Value.SegmentFlag == 'C' || x.Value.SegmentFlag == 'D') && ((x.Value.ScripCode >= 100000 && x.Value.ScripCode < 200000) || (x.Value.ScripCode >= 500000))).OrderBy(y => y.Value.ScripId).ToDictionary(x => x.Key, x => x.Value);
                        //objDebtDetailsDic = MasterSharedMemory.objMastertxtDict.Where(x => (x.Value.InstrumentType == 'C' && ((x.Value.ScripCode >= 100000 && x.Value.ScripCode < 200000) || x.Value.ScripCode >= 500000) || (x.Value.InstrumentType == 'D' && ((x.Value.ScripCode >= 100000 && x.Value.ScripCode < 200000) || (x.Value.ScripCode >= 500000))))).OrderBy(y => y.Value.ScripId).ToDictionary(x => x.Key, x => x.Value);
                    }
                    else if (Is4LChecked == true)
                    { //TODO TBD2017
                      // objDebtDetailsDic = MasterSharedMemory.objMastertxtDict.Where(x => (x.Value.InstrumentType == 'C' && ((x.Value.ScripCode >= 100000 && x.Value.ScripCode < 200000) || x.Value.ScripCode >= 400000)) || (x.Value.InstrumentType == 'D' && ((x.Value.ScripCode >= 100000 && x.Value.ScripCode < 200000) || (x.Value.ScripCode >= 400000)))).OrderBy(y => y.Value.ScripId).ToDictionary(x => x.Key, x => x.Value);
                    }
                    #endregion
                    if (SelectedScripGrp == ScripProfilingModel.FutType._All.ToString().Replace("_", " "))
                    {
                        foreach (var item in objDebtDetailsDic)
                        {
                            ScripProfModel scripmasterEquity = new ScripProfModel();
                            scripmasterEquity.ScripId = item.Value.ScripId;
                            scripmasterEquity.ScripCode = item.Value.ScripCode;
                            scripmasterEquity.GroupName = item.Value.GroupName;
                            ObjEquityDataCollection.Add(scripmasterEquity);
                        }
                        lblCountContent = ObjEquityDataCollection.Count + " Scrips";
                    }
                    else
                    {
                        Dictionary<long, ScripMaster> TempDict = new Dictionary<long, ScripMaster>();
                        TempDict = objDebtDetailsDic.Where(x => x.Value.GroupName == SelectedScripGrp).ToDictionary(x => x.Key, x => x.Value);
                        foreach (var item in TempDict)
                        {
                            ScripProfModel scripmasterEquity = new ScripProfModel();
                            scripmasterEquity.ScripId = item.Value.ScripId;
                            scripmasterEquity.ScripCode = item.Value.ScripCode;
                            scripmasterEquity.GroupName = item.Value.GroupName;
                            ObjEquityDataCollection.Add(scripmasterEquity);
                        }
                        lblCountContent = ObjEquityDataCollection.Count + " Scrips";
                    }
                }
                else if (ScripSelectedSegment == ScripProfilingModel.ScripSegment.Currency.ToString())
                {
                    if (ProdTypeCurrencySelected == ScripProfilingModel.CurrencyProdType.Straddle.ToString())
                    {
                        try
                        {
                            //Dictionary<long, DerivativeMaster> TempCurrencyDetailsDic = new Dictionary<long, DerivativeMaster>();
                            //TempCurrencyDetailsDic = objCurrencyDetailsDic.Where(x => x.Value.StrategyID == 28 && x.Value.ComplexInstrumentType == 2).ToDictionary(x => x.Key, y => y.Value);
                            //if (SelectedCurrAsset != " All")
                            //{
                            //    TempCurrencyDetailsDic = TempCurrencyDetailsDic.Where(x => x.Value.AssetCode == SelectedCurrAsset).ToDictionary(x => x.Key, y => y.Value);
                            //}
                            //foreach (var item in TempCurrencyDetailsDic)
                            //{
                            //    ScripProfModel currSpread = new ScripProfModel();
                            //    currSpread.ScripId = item.Value.InstrumentName;
                            //    currSpread.ScripCode = item.Value.SeriesIDofleg1;
                            //    currSpread.GroupName = "CD";
                            //    currSpread.ExpiryDate = item.Value.ExpiryDate;
                            //    currSpread.StrikePrice = String.Format("{0:0.0000}", Convert.ToDecimal(CommonFunctions.DisplayInDecimalFormatTouch(item.Value.StrikePrice, 7))).ToString();

                            //    ObjEquityDataCollection.Add(currSpread);
                            //}
                            ////ObjEquityDataCollection.RemoveAt(0);
                            //lblCountContent = ObjEquityDataCollection.Count + " Scrips";
                        }
                        catch (Exception)
                        {
                            IsSearchEnable = true;
                        }
                    }
                    else if (ProdTypeCurrencySelected == ScripProfilingModel.CurrencyProdType.Pair_Option.ToString().Replace("_", " "))
                    {
                        try
                        {
                            //Dictionary<long, DerivativeMaster> TempCurrencyDetailsDic = new Dictionary<long, DerivativeMaster>();
                            //TempCurrencyDetailsDic = objCurrencyDetailsDic.Where(x => x.Value.StrategyID == 15 && x.Value.ComplexInstrumentType == 2).ToDictionary(x => x.Key, y => y.Value);
                            //if (SelectedCurrAsset != " All")
                            //{
                            //    TempCurrencyDetailsDic = TempCurrencyDetailsDic.Where(x => x.Value.AssetCode == SelectedCurrAsset).ToDictionary(x => x.Key, y => y.Value);
                            //}
                            //foreach (var item in TempCurrencyDetailsDic)
                            //{
                            //    ScripProfModel currSpread = new ScripProfModel();
                            //    currSpread.ScripId = item.Value.InstrumentName;
                            //    currSpread.ScripCode = item.Value.SeriesIDofleg1;
                            //    currSpread.GroupName = "CD";
                            //    currSpread.ExpiryDate = item.Value.ExpiryDate;
                            //    currSpread.StrikePrice = CommonFunctions.DisplayInDecimalFormatTouch(item.Value.StrikePrice, 4);
                            //    ObjEquityDataCollection.Add(currSpread);
                            //}
                            lblCountContent = ObjEquityDataCollection.Count + " Scrips";
                        }
                        catch (Exception)
                        {
                            IsSearchEnable = true;
                        }
                    }
                    else if (ProdTypeCurrencySelected == ScripProfilingModel.CurrencyProdType.Future.ToString())
                    {
                        try
                        {
                            //Dictionary<long, DerivativeMaster> TempCurrencyDetailsDic = new Dictionary<long, DerivativeMaster>();
                            //TempCurrencyDetailsDic = objCurrencyDetailsDic.Where(x => x.Value.ProductType == "FUTIRD" || x.Value.ProductType == "FUTCUR" || x.Value.ProductType == "FUTIRT").ToDictionary(x => x.Key, y => y.Value);
                            //if (SelectedCurrAsset != " All")
                            //{
                            //    TempCurrencyDetailsDic = TempCurrencyDetailsDic.Where(x => x.Value.AssetCode == SelectedCurrAsset).ToDictionary(x => x.Key, y => y.Value);
                            //}
                            //if (SelectedFutType != " All")
                            //{
                            //    if (SelectedFutType == "Normal Fut")
                            //    {
                            //        TempCurrencyDetailsDic = TempCurrencyDetailsDic.Where(x => x.Value.OptionType == FutSelected).ToDictionary(x => x.Key, x => x.Value);
                            //    }
                            //    else
                            //    {
                            //        TempCurrencyDetailsDic = TempCurrencyDetailsDic.Where(x => x.Value.ComplexInstrumentType == CalSelected).ToDictionary(x => x.Key, x => x.Value);
                            //    }
                            //}
                            //if (SelectedCurrExpDate != " All")
                            //{
                            //    if (NewSelectedDerExpDate == SelectedCurrExpDate)
                            //    {
                            //        TempCurrencyDetailsDic = TempCurrencyDetailsDic.Where(x => DateTime.ParseExact(x.Value.ExpiryDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture).ToString("dd-MMM-yy").ToLower() == SelectedCurrExpDate.ToLower()).ToDictionary(x => x.Key, x => x.Value);
                            //    }
                            //    else
                            //    {
                            //        SelectedCurrExpDate = DateTime.ParseExact(SelectedCurrExpDate, "dd-MM-yy", CultureInfo.InvariantCulture).ToString("dd-MMM-yy");
                            //        TempCurrencyDetailsDic = TempCurrencyDetailsDic.Where(x => DateTime.ParseExact(x.Value.ExpiryDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture).ToString("dd-MMM-yy").ToLower() == SelectedCurrExpDate.ToLower()).ToDictionary(x => x.Key, x => x.Value);
                            //        NewSelectedDerExpDate = SelectedCurrExpDate;
                            //    }
                            //}
                            //if (CurrStrikePriceSelected != " All")
                            //{
                            //    TempCurrencyDetailsDic = TempCurrencyDetailsDic.Where(x => x.Value.StrikePrice == Convert.ToInt32(CurrStrikePriceSelected)).ToDictionary(x => x.Key, x => x.Value);
                            //}
                            //foreach (var item in TempCurrencyDetailsDic)
                            //{
                            //    ScripProfModel currSpread = new ScripProfModel();
                            //    currSpread.ScripId = item.Value.InstrumentName;
                            //    currSpread.ScripCode = item.Value.ScripCode;
                            //    currSpread.GroupName = "CD";
                            //    currSpread.ExpiryDate = item.Value.ExpiryDate;
                            //    ObjEquityDataCollection.Add(currSpread);
                            //}
                            lblCountContent = ObjEquityDataCollection.Count + " Scrips";
                        }
                        catch (Exception)
                        {
                            IsSearchEnable = true;
                        }
                    }
                    else if (ProdTypeCurrencySelected == ScripProfilingModel.CurrencyProdType.Option.ToString())
                    {
                        //ObjEquityDataCollection.Clear();
                        try
                        {

                            //Dictionary<long, DerivativeMaster> TempCurrencyDetailsDic = new Dictionary<long, DerivativeMaster>();
                            //TempCurrencyDetailsDic = objCurrencyDetailsDic.Where(x => x.Value.ProductType == "OPTCUR" && x.Value.UnderlyingId == 600).ToDictionary(x => x.Key, y => y.Value);
                            //if (SelectedCurrAsset != " All")
                            //{
                            //    TempCurrencyDetailsDic = TempCurrencyDetailsDic.Where(x => x.Value.AssetCode == SelectedCurrAsset).ToDictionary(x => x.Key, y => y.Value);
                            //}
                            //if (SelectedOptionType != " All")
                            //{
                            //    TempCurrencyDetailsDic = TempCurrencyDetailsDic.Where(x => x.Value.OptionType == optionTyp).ToDictionary(x => x.Key, x => x.Value);
                            //}
                            //if (SelectedCurrExpDate != " All")
                            //{
                            //    if (NewSelectedDerExpDate == SelectedCurrExpDate)
                            //    {
                            //        TempCurrencyDetailsDic = TempCurrencyDetailsDic.Where(x => DateTime.ParseExact(x.Value.ExpiryDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture).ToString("dd-MMM-yy").ToLower() == SelectedCurrExpDate.ToLower()).Distinct().ToDictionary(x => x.Key, x => x.Value);
                            //    }
                            //    else
                            //    {
                            //        SelectedCurrExpDate = DateTime.ParseExact(SelectedCurrExpDate, "dd-MM-yy", CultureInfo.InvariantCulture).ToString("dd-MMM-yy");
                            //        TempCurrencyDetailsDic = TempCurrencyDetailsDic.Where(x => DateTime.ParseExact(x.Value.ExpiryDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture).ToString("dd-MMM-yy").ToLower() == SelectedCurrExpDate.ToLower()).Distinct().ToDictionary(x => x.Key, x => x.Value);
                            //        NewSelectedDerExpDate = SelectedCurrExpDate;
                            //    }
                            //}
                            //if (CurrStrikePriceSelected != " All" && SelectedCurrAsset != " All")
                            //{
                            //    double SelStrikePrice = Convert.ToDouble(CurrStrikePriceSelected) * Math.Pow(10, 7);
                            //    TempCurrencyDetailsDic = TempCurrencyDetailsDic.Where(x => x.Value.StrikePrice == SelStrikePrice).ToDictionary(x => x.Key, x => x.Value);
                            //}

                            //foreach (var item in TempCurrencyDetailsDic)
                            //{
                            //    ScripProfModel currSpread = new ScripProfModel();
                            //    currSpread.ScripId = item.Value.InstrumentName;
                            //    currSpread.ScripCode = item.Value.ScripCode;
                            //    currSpread.GroupName = "CD";
                            //    currSpread.ExpiryDate = item.Value.ExpiryDate;
                            //    currSpread.StrikePrice = String.Format("{0:0.0000}", Convert.ToDecimal(CommonFunctions.DisplayInDecimalFormatTouch(item.Value.StrikePrice, 7))).ToString();
                            //    ObjEquityDataCollection.Add(currSpread);
                            //}
                            //ObjEquityDataCollection.RemoveAt(0);
                            lblCountContent = ObjEquityDataCollection.Count + " Scrips";
                        }
                        catch (Exception)
                        {
                            IsSearchEnable = true;
                        }
                    }
                    else if (ProdTypeCurrencySelected == ScripProfilingModel.OptType.All.ToString().Replace("_", " "))
                    {
                        try
                        {
                            //Dictionary<long, DerivativeMaster> TempCurrencyDetailsDic = new Dictionary<long, DerivativeMaster>();
                            //TempCurrencyDetailsDic = objCurrencyDetailsDic.Where(x => x.Value.ProductType == "FUTIRD" || x.Value.ProductType == "FUTCUR" || x.Value.ProductType == "FUTIRT" || x.Value.ProductType == "OPTCUR").OrderBy(x => x.Value.AssetCode).ToDictionary(x => x.Key, y => y.Value);
                            //if (SelectedCurrAsset != " All")
                            //{
                            //    TempCurrencyDetailsDic = TempCurrencyDetailsDic.Where(x => x.Value.AssetCode == SelectedCurrAsset).OrderBy(x => x.Value.AssetCode).ToDictionary(x => x.Key, y => y.Value);
                            //}
                            //foreach (var item in TempCurrencyDetailsDic)
                            //{
                            //    ScripProfModel currSpread = new ScripProfModel();
                            //    currSpread.ScripId = item.Value.InstrumentName;
                            //    currSpread.ScripCode = item.Value.ScripCode;
                            //    currSpread.GroupName = "CD";
                            //    currSpread.ExpiryDate = item.Value.ExpiryDate;
                            //    currSpread.StrikePrice = String.Format("{0:0.0000}", Convert.ToDecimal(CommonFunctions.DisplayInDecimalFormatTouch(item.Value.StrikePrice, 7))).ToString();
                            //    ObjEquityDataCollection.Add(currSpread);
                            //}
                            //ObjEquityDataCollection.RemoveAt(0);
                            lblCountContent = ObjEquityDataCollection.Count + " Scrips";
                        }
                        catch (Exception)
                        {
                            IsSearchEnable = true;
                        }
                    }
                    else
                    {
                        try
                        {
                            //    Dictionary<long, DerivativeMaster> TempCurrencyDetailsDic = new Dictionary<long, DerivativeMaster>();
                            //    TempCurrencyDetailsDic = objCurrencyDetailsDic.Where(x => x.Value.CurrScripGroup == "CD").ToDictionary(x => x.Key, y => y.Value);
                            //    //TempCurrencyDetailsDic = objCurrencyDetailsDic.Where(x => x.Value.ProductType == "FUTIRD" || x.Value.ProductType == "FUTCUR" || x.Value.ProductType == "FUTIRT" || x.Value.ProductType == "OPTCUR").ToDictionary(x => x.Key, y => y.Value);
                            //    //if (SelectedCurrAsset != " All")
                            //    //{
                            //    //    TempCurrencyDetailsDic = TempCurrencyDetailsDic.Where(x => x.Value.AssetCode == SelectedCurrAsset).OrderBy(x=>x.Value.AssetCode).ToDictionary(x => x.Key, y => y.Value);
                            //    //}
                            //    foreach (var item in TempCurrencyDetailsDic)
                            //    {
                            //        ScripProfModel currSpread = new ScripProfModel();
                            //        currSpread.ScripId = item.Value.InstrumentName;
                            //        currSpread.ScripCode = item.Value.ScripCode;
                            //        currSpread.GroupName = "CD";
                            //        currSpread.ExpiryDate = item.Value.ExpiryDate;
                            //        currSpread.StrikePrice = String.Format("{0:0.0000}", Convert.ToDecimal(CommonFunctions.DisplayInDecimalFormatTouch(item.Value.StrikePrice, 7))).ToString();
                            //        ObjEquityDataCollection.Add(currSpread);
                            //    }
                            //    ObjEquityDataCollection.Remove(ObjEquityDataCollection.Where(x => x.StrikePrice == "0").SingleOrDefault());
                            lblCountContent = ObjEquityDataCollection.Count + " Scrips";
                        }
                        catch (Exception)
                        {
                            IsSearchEnable = true;
                        }
                    }
                }
                else if (ScripSelectedSegment == ScripProfilingModel.ScripSegment.Derivative.ToString())
                {
                    string pTypeDerivativeSelected = String.Empty;
                    string DerivativeAssetSel = String.Empty;
                    string futureSel = String.Empty;
                    //Dictionary<long, DerivativeMaster> TempDerivativeDetailsDict = new Dictionary<long, DerivativeMaster>();
                    //if (ProdTypeDerivativeSelected != " All")
                    //{
                    //    if (DerProdSelected == "IF" || DerProdSelected == "SF")
                    //    {
                    //        try
                    //        {
                    //            //var countProdType = CommonFrontEnd.SharedMemories.CurrencyDerivativeMemory.objDerivativeCommon.Values.Count(x => x.ProductType == "IF");
                    //            TempDerivativeDetailsDict = objDerivativeDetailsDic.Where(x => x.Value.ProductType == DerProdSelected).ToDictionary(x => x.Key, x => x.Value);
                    //            if (SelectedDerAsset != " All")
                    //            {
                    //                TempDerivativeDetailsDict = TempDerivativeDetailsDict.Where(x => x.Value.AssetCode == SelectedDerAsset).ToDictionary(x => x.Key, x => x.Value);
                    //            }
                    //            if (SelectedFutType != " All")
                    //            {
                    //                if (SelectedFutType == "Normal Fut")
                    //                {
                    //                    TempDerivativeDetailsDict = TempDerivativeDetailsDict.Where(x => x.Value.OptionType == FutSelected).ToDictionary(x => x.Key, x => x.Value);
                    //                }
                    //                else
                    //                {
                    //                    TempDerivativeDetailsDict = TempDerivativeDetailsDict.Where(x => x.Value.ComplexInstrumentType == CalSelected).ToDictionary(x => x.Key, x => x.Value);
                    //                }
                    //            }
                    //            if (SelectedDerExpDate != " All")
                    //            {
                    //                if (NewSelectedDerExpDate == SelectedDerExpDate)
                    //                {
                    //                    TempDerivativeDetailsDict = TempDerivativeDetailsDict.Where(x => DateTime.ParseExact(x.Value.ExpiryDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture).ToString("dd-MMM-yy").ToLower() == SelectedDerExpDate.ToLower()).ToDictionary(x => x.Key, x => x.Value);
                    //                }
                    //                else
                    //                {
                    //                    SelectedDerExpDate = DateTime.ParseExact(SelectedDerExpDate, "dd-MM-yy", CultureInfo.InvariantCulture).ToString("dd-MMM-yy");
                    //                    TempDerivativeDetailsDict = TempDerivativeDetailsDict.Where(x => DateTime.ParseExact(x.Value.ExpiryDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture).ToString("dd-MMM-yy").ToLower() == SelectedDerExpDate.ToLower()).ToDictionary(x => x.Key, x => x.Value);
                    //                    NewSelectedDerExpDate = SelectedDerExpDate;
                    //                }

                    //            }
                    //            foreach (var item in TempDerivativeDetailsDict)
                    //            {
                    //                ScripProfModel sc = new ScripProfModel();
                    //                sc.ScripId = item.Value.SeriesCode;
                    //                sc.ScripCode = item.Value.ScripCode;
                    //                sc.GroupName = "DF";
                    //                sc.ExpiryDate = DateTime.ParseExact(item.Value.ExpiryDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture).ToString("dd-MM-yy");
                    //                ObjEquityDataCollection.Add(sc);
                    //            }
                    //    lblCountContent = ObjEquityDataCollection.Count + " Scrips";
                    //}
                    //catch (Exception ex)
                    //{
                    //    IsSearchEnable = true;
                    //}
                }
                else if (DerProdSelected == "IO" || DerProdSelected == "SO")
                {
                    try
                    {

                        //TempDerivativeDetailsDict = objDerivativeDetailsDic.Where(x => x.Value.ProductType == DerProdSelected).ToDictionary(x => x.Key, x => x.Value);
                        //if (SelectedDerAsset != " All")
                        //{
                        //    TempDerivativeDetailsDict = TempDerivativeDetailsDict.Where(x => x.Value.AssetCode == SelectedDerAsset).ToDictionary(x => x.Key, x => x.Value);
                        //}
                        //if (SelectedOptionType != " All")
                        //{
                        //    TempDerivativeDetailsDict = TempDerivativeDetailsDict.Where(x => x.Value.OptionType == optionTyp).ToDictionary(x => x.Key, x => x.Value);
                        //}
                        //if (SelectedExPrd != " All")
                        //{
                        //    TempDerivativeDetailsDict = TempDerivativeDetailsDict.Where(x => x.Value.ContractType.Equals(contractTyp)).ToDictionary(x => x.Key, x => x.Value);
                        //}
                        //if (SelectedDerExpDate != " All")
                        //{
                        //    if (NewSelectedDerExpDate == SelectedDerExpDate)
                        //    {
                        //        TempDerivativeDetailsDict = TempDerivativeDetailsDict.Where(x => DateTime.ParseExact(x.Value.ExpiryDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture).ToString("dd-MMM-yy").ToLower() == SelectedDerExpDate.ToLower()).ToDictionary(x => x.Key, x => x.Value);
                        //    }
                        //    else
                        //    {
                        //        SelectedDerExpDate = DateTime.ParseExact(SelectedDerExpDate, "dd-MM-yy", CultureInfo.InvariantCulture).ToString("dd-MMM-yy");
                        //        TempDerivativeDetailsDict = TempDerivativeDetailsDict.Where(x => DateTime.ParseExact(x.Value.ExpiryDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture).ToString("dd-MMM-yy").ToLower() == SelectedDerExpDate.ToLower()).ToDictionary(x => x.Key, x => x.Value);
                        //        NewSelectedDerExpDate = SelectedDerExpDate;
                        //    }

                        //}
                        //if (DerStrikePriceSelected != " All")
                        //{
                        //    TempDerivativeDetailsDict = TempDerivativeDetailsDict.Where(x => x.Value.StrikePrice == Convert.ToInt32(DerStrikePriceSelected) * 100).ToDictionary(x => x.Key, x => x.Value);
                        //}

                        //foreach (var item in TempDerivativeDetailsDict)
                        //{
                        //    ScripProfModel sc = new ScripProfModel();
                        //    sc.ScripId = item.Value.SeriesCode;
                        //    sc.ScripCode = item.Value.ScripCode;
                        //    sc.GroupName = "DF";
                        //    sc.ExpiryDate = DateTime.ParseExact(item.Value.ExpiryDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture).ToString("dd-MM-yy");
                        //    sc.StrikePrice = CommonFunctions.DisplayInDecimalFormatTouch(item.Value.StrikePrice, 2);
                        //    ObjEquityDataCollection.Add(sc);
                        //}
                        lblCountContent = ObjEquityDataCollection.Count + " Scrips";
                    }
                    catch (Exception)
                    {
                        IsSearchEnable = true;
                    }
                }
                else if (DerProdSelected == "PO" || ProdTypeDerivativeSelected == ScripProfilingModel.DerivativeProdType.Pair_Option.ToString().Replace("_", " "))
                {
                    try
                    {
                        //TempDerivativeDetailsDict = objDerivativeDetailsDic.Where(x => x.Value.ComplexInstrumentType == 2).ToDictionary(x => x.Key, x => x.Value);
                        //if (SelectedDerAsset != " All")
                        //{
                        //    TempDerivativeDetailsDict = TempDerivativeDetailsDict.Where(x => x.Value.AssetCode == SelectedDerAsset).ToDictionary(x => x.Key, x => x.Value);
                        //}
                        //foreach (var item in TempDerivativeDetailsDict)
                        //{
                        //    ScripProfModel sc = new ScripProfModel();
                        //    sc.ScripId = item.Value.InstrumentName;
                        //    sc.ScripCode = item.Value.ScripCode;
                        //    sc.GroupName = "DF";
                        //    sc.ExpiryDate = DateTime.ParseExact(item.Value.ExpiryDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture).ToString("dd-MM-yy");
                        //    sc.StrikePrice = CommonFunctions.DisplayInDecimalFormatTouch(item.Value.StrikePrice, 2);
                        //    ObjEquityDataCollection.Add(sc);
                        //}
                        lblCountContent = ObjEquityDataCollection.Count + " Scrips";
                    }
                    catch (Exception)
                    {
                        IsSearchEnable = true;
                    }
                }
                else if (ProdTypeDerivativeSelected == " All")
                {
                    try
                    {
                        //TempDerivativeDetailsDict = objDerivativeDetailsDic.Where(x => x.Value.ProductType == "IF" || x.Value.ProductType == "IO" || x.Value.ProductType == "SF" || x.Value.ProductType == "SO").ToDictionary(x => x.Key, x => x.Value);
                        //if (SelectedDerAsset != " All")
                        //{
                        //    TempDerivativeDetailsDict = TempDerivativeDetailsDict.Where(x => x.Value.AssetCode == SelectedDerAsset).ToDictionary(x => x.Key, x => x.Value);
                        //}
                        //foreach (var item in TempDerivativeDetailsDict)
                        //{
                        //    ScripProfModel sc = new ScripProfModel();
                        //    sc.ScripId = item.Value.SeriesCode;
                        //    sc.ScripCode = item.Value.ScripCode;
                        //    sc.GroupName = "DF";
                        //    sc.ExpiryDate = DateTime.ParseExact(item.Value.ExpiryDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture).ToString("dd-MM-yy");
                        //    sc.StrikePrice = CommonFunctions.DisplayInDecimalFormatTouch(item.Value.StrikePrice, 2);
                        //    ObjEquityDataCollection.Add(sc);
                        //}
                        lblCountContent = ObjEquityDataCollection.Count + " Scrips";
                    }
                    catch (Exception)
                    {
                        IsSearchEnable = true;
                    }
                }
                else if (ScripSelectedSegment == ScripProfilingModel.ScripSegment.All.ToString())
                {
                    foreach (var item in objEquityDetailsDic)
                    {
                        ScripProfModel scripmasterEquity = new ScripProfModel();
                        scripmasterEquity.ScripId = item.Value.ScripId;
                        scripmasterEquity.ScripCode = item.Value.ScripCode;
                        scripmasterEquity.GroupName = item.Value.GroupName;
                        ObjEquityDataCollection.Add(scripmasterEquity);
                    }
                    foreach (var item in objDebtDetailsDic)
                    {
                        ScripProfModel scripmasterEquity = new ScripProfModel();
                        scripmasterEquity.ScripId = item.Value.ScripId;
                        scripmasterEquity.ScripCode = item.Value.ScripCode;
                        scripmasterEquity.GroupName = item.Value.GroupName;
                        ObjEquityDataCollection.Add(scripmasterEquity);
                    }
                    //foreach (var item in objCurrencyDetailsDic)
                    //{
                    //    ScripProfModel scbfx = new ScripProfModel();
                    //    scbfx.ScripId = item.Value.InstrumentName;
                    //    scbfx.ScripCode = item.Value.ScripCode;
                    //    scbfx.GroupName = item.Value.CurrScripGroup;
                    //    scbfx.ExpiryDate = item.Value.ExpiryDate;
                    //    scbfx.StrikePrice = String.Format("{0:0.0000}", Convert.ToDecimal(CommonFunctions.DisplayInDecimalFormatTouch(item.Value.StrikePrice, 7))).ToString();
                    //    //if (item.Value.StrikePrice == 0)
                    //    //{
                    //    //    ObjEquityDataCollection.RemoveAt(0);
                    //    //}
                    //    ObjEquityDataCollection.Remove(ObjEquityDataCollection.Where(x => (x.StrikePrice == "0")).SingleOrDefault());
                    //}
                    //foreach (var item in objCurrencySpreadDetailsDic)
                    //{
                    //    ScripProfModel scbfxSpd = new ScripProfModel();
                    //    scbfxSpd.ScripId = item.Value.InstrumentName;
                    //    scbfxSpd.ScripCode = item.Value.ScripCode;
                    //    scbfxSpd.GroupName = item.Value.CurrScripGroup;
                    //    scbfxSpd.ExpiryDate = item.Value.ExpiryDate;
                    //    ObjEquityDataCollection.Add(scbfxSpd);
                    //}
                    //foreach (var item in objDerivativeDetailsDic)
                    //{
                    //    ScripProfModel scpder = new ScripProfModel();
                    //    scpder.ScripId = item.Value.SeriesCode;
                    //    scpder.ScripCode = item.Value.ScripCode;
                    //    scpder.GroupName = item.Value.DerScripGroup;
                    //    scpder.ExpiryDate = item.Value.ExpiryDate;
                    //    scpder.StrikePrice = CommonFunctions.DisplayInDecimalFormatTouch(item.Value.StrikePrice, 2);
                    //    ObjEquityDataCollection.Add(scpder);
                    //}
                    //foreach (var item in objDerivativeSpreadDetailsDic)
                    //{
                    //    ScripProfModel scpderSpd = new ScripProfModel();
                    //    scpderSpd.ScripId = item.Value.SeriesCode;
                    //    scpderSpd.ScripCode = item.Value.ScripCode;
                    //    scpderSpd.GroupName = item.Value.DerScripGroup;
                    //    scpderSpd.ExpiryDate = item.Value.ExpiryDate;
                    //    ObjEquityDataCollection.Add(scpderSpd);
                    //}
                    lblCountContent = ObjEquityDataCollection.Count + " Scrips";
                }
                ObjEquityDataCollection.GroupBy(x => x.ScripId);
                TempEquityDataCollection = ObjEquityDataCollection;
                DemoDataCollection = TempEquityDataCollection;
                IsSearchEnable = true;
            }
            catch (Exception e)
            {
                ExceptionUtility.LogError(e);
            }
        }

        private void PopulatingDerivativeProdTypeDropDowns()
        {
#if TWS
            ProdTypeDerivativeSelected = ScripProfilingModel.FutType._All.ToString().Replace("_", " ");
            DerivativePType.Add(ScripProfilingModel.FutType._All.ToString().Replace("_", " "));
            DerivativePType.Add(ScripProfilingModel.DerivativeProdType.Index_Future.ToString().Replace("_", " "));
            DerivativePType.Add(ScripProfilingModel.DerivativeProdType.Index_Option.ToString().Replace("_", " "));
            DerivativePType.Add(ScripProfilingModel.DerivativeProdType.Stock_Future.ToString().Replace("_", " "));
            DerivativePType.Add(ScripProfilingModel.DerivativeProdType.Stock_Option.ToString().Replace("_", " "));
            DerivativePType.Add(ScripProfilingModel.DerivativeProdType.Pair_Option.ToString().Replace("_", " "));
#endif
        }

        private void PopulatingScripSetDropDowns()
        {
#if TWS
            SelectedScripSet = ScripProfilingModel.ScripSet._Select.ToString().Replace("_", " ");
            ScripSet.Add(ScripProfilingModel.ScripSet._Select.ToString().Replace("_", " "));
            ScripSet.Add(ScripProfilingModel.ScripSet.PreOpen_Scrips.ToString().Replace("_", " "));
            ScripSet.Add(ScripProfilingModel.ScripSet.All_Currency_Spreads.ToString().Replace("_", " "));
            ScripSet.Add(ScripProfilingModel.ScripSet.All_Derivative_Spreads.ToString().Replace("_", " "));
            ScripSet.Add(ScripProfilingModel.ScripSet.IPO_Scrips.ToString().Replace("_", " "));
            ScripSet.Add(ScripProfilingModel.ScripSet.ReList_Scrips.ToString().Replace("_", " "));
            ScripSet.Add(ScripProfilingModel.ScripSet.PCAS_Scrips.ToString().Replace("_", " "));
            ScripSet.Add(ScripProfilingModel.ScripSet.SPOS_Scrips.ToString().Replace("_", " "));
            ScripSet.Add(ScripProfilingModel.ScripSet.Proposed_Scrips.ToString().Replace("_", " "));
            ScripSet.Add(ScripProfilingModel.ScripSet.BSE_Exclusive.ToString().Replace("_", " "));
            ScripSet.Add(ScripProfilingModel.ScripSet.Derivative_Scrips.ToString().Replace("_", " "));
            ScripSet.Add(ScripProfilingModel.ScripSet.Only_6L_Series.ToString().Replace("_", " "));
            ScripSet.Add(ScripProfilingModel.ScripSet.Only_4L_Series.ToString().Replace("_", " "));
            ScripSet.Add(ScripProfilingModel.ScripSet.GSM_Scrips.ToString().Replace("_", " "));
#endif
        }

        private void PopulatingScripProfileDropDowns()
        {
#if TWS
            ScripSelectedSegment = ScripProfilingModel.ScripSegment.Equity.ToString();
            ScripSegmentLst.Add(ScripProfilingModel.ScripSegment.All.ToString());
            ScripSegmentLst.Add(ScripProfilingModel.ScripSegment.Equity.ToString());
            ScripSegmentLst.Add(ScripProfilingModel.ScripSegment.Derivative.ToString());
            ScripSegmentLst.Add(ScripProfilingModel.ScripSegment.Debt.ToString());
            ScripSegmentLst.Add(ScripProfilingModel.ScripSegment.Currency.ToString());
#endif
        }

        private void PopulatingCurrencyProdTypeDropDowns()
        {
#if TWS
            ProdTypeCurrencySelected = ScripProfilingModel.CurrencyProdType.All.ToString();
            CurrencyPType.Add(ScripProfilingModel.CurrencyProdType.All.ToString());
            CurrencyPType.Add(ScripProfilingModel.CurrencyProdType.Future.ToString());
            CurrencyPType.Add(ScripProfilingModel.CurrencyProdType.Option.ToString());
            CurrencyPType.Add(ScripProfilingModel.CurrencyProdType.Pair_Option.ToString().Replace("_", " "));
            CurrencyPType.Add(ScripProfilingModel.CurrencyProdType.Straddle.ToString());
#endif
        }
        private void PopulatingOptionDropDowns()
        {
#if TWS
            SelectedOptionType = ScripProfilingModel.FutType._All.ToString().Replace("_", " ");
            OptionType.Add(ScripProfilingModel.FutType._All.ToString().Replace("_", " "));
            OptionType.Add(ScripProfilingModel.OptType.Call.ToString());
            OptionType.Add(ScripProfilingModel.OptType.Put.ToString());
#endif
        }

        private void PopulatingFutureDropDowns()
        {
#if TWS
            SelectedFutType = ScripProfilingModel.FutType._All.ToString().Replace("_", " ");
            FutureType.Add(ScripProfilingModel.FutType._All.ToString().Replace("_", " "));
            FutureType.Add(ScripProfilingModel.FutType.Normal_Fut.ToString().Replace("_", " "));
            FutureType.Add(ScripProfilingModel.FutType.Cal_Spread.ToString().Replace("_", " "));
#endif
        }

        private void PopulatingScripGroupDropDowns()
        {
#if TWS
            try
            {
                oDataAccessLayer.Connect((int)DataAccessLayer.ConnectionDB.Masters);
                oDataAccessLayer.OpenConnection((int)DataAccessLayer.ConnectionDB.Masters);
                ScripGrp = new List<string>();
                SelectedScripGrp = ScripProfilingModel.FutType._All.ToString().Replace("_", " ");
                str = "SELECT Distinct(GroupName) FROM BSE_SECURITIES_CFE;";
                SQLiteDataReader oSQLiteDataReader = oDataAccessLayer.ExecuteDataReader((int)ConnectionDB.Masters,str, System.Data.CommandType.Text, null);
                while (oSQLiteDataReader.Read())
                {
                    if (oSQLiteDataReader["GroupName"] != string.Empty)
                        ScripGrp.Add(oSQLiteDataReader["GroupName"]?.ToString().Trim());
                }
                ScripGrp.Add(ScripProfilingModel.FutType._All.ToString().Replace("_", " "));
                ScripGrp.Sort();
                ScripGrp.Remove(string.Empty);

            }
            catch (Exception e) { ExceptionUtility.LogError(e); }
            finally
            {
               oDataAccessLayer.CloseConnection((int)DataAccessLayer.ConnectionDB.Masters);
                System.Data.SQLite.SQLiteConnection.ClearAllPools();
            }
#endif
        }

        private void PopulatingCurrencyExpiryDateDropDown()
        {
#if TWS
            try
            {
                CurrExpDateLst = new ObservableCollection<string>();
                CurrExpDateLst.Add(ScripProfilingModel.ScripSegment.All.ToString());
                SelectedCurrExpDate = ScripProfilingModel.FutType._All.ToString().Replace("_", " ");
                if (MasterSharedMemory.objMasterCurrencyDictBaseBSE != null)
                {
                    foreach (var item in MasterSharedMemory.objMasterCurrencyDictBaseBSE.Values.Cast<CurrencyMasterBase>().GroupBy(x => x.ExpiryDate).Select(x => DateTime.ParseExact(x.FirstOrDefault().ExpiryDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture).ToString("dd-MM-yy")))
                    {
                        CurrExpDateLst.Add(item);
                    }
                }
            }
            catch (Exception e) { ExceptionUtility.LogError(e); }
#endif
        }

        private void PopulatingIndicesSetDropDowns()
        {

            try
            {
                IndicesSet = new List<string>();
                SelectedIndicesSet = ScripProfilingModel.ScripSet._Select.ToString().Replace("_", " ");

                oDataAccessLayer.Connect((int)DataAccessLayer.ConnectionDB.Masters);
                oDataAccessLayer.OpenConnection((int)DataAccessLayer.ConnectionDB.Masters);
                str = "SELECT Distinct(ExistingShortName) FROM BSE_SNPINDICES_CFE";
                SQLiteDataReader oSQLiteDataReader = oDataAccessLayer.ExecuteDataReader((int)ConnectionDB.Masters,str, System.Data.CommandType.Text, null);
                while (oSQLiteDataReader.Read())
                {
                    if (oSQLiteDataReader["ExistingShortName"] != string.Empty)
                        IndicesSet.Add(oSQLiteDataReader["ExistingShortName"]?.ToString().Trim());
                }

                //if (MasterSharedMemory.objSpnIndicesDic != null)
                //    IndicesSet = MasterSharedMemory.objSpnIndicesDic.Values.Cast<ScripMasterSpnIndices>().GroupBy(x => x.ExistingShortName_ca).Select(x => x.FirstOrDefault().ExistingShortName_ca).ToList();
                IndicesSet.Add(ScripProfilingModel.ScripSet._Select.ToString().Replace("_", " "));
                IndicesSet.Sort();
            }
            catch (Exception e) { ExceptionUtility.LogError(e); }
            finally
            {

               oDataAccessLayer.CloseConnection((int)DataAccessLayer.ConnectionDB.Masters);
                System.Data.SQLite.SQLiteConnection.ClearAllPools();
            }
        }

        private void EnableDisable()
        {
#if TWS

            SelectedDerAsset = " All";
            DerivativeAssetEnable = false;
            DerivativeAssestVisible = "Visible";
            CurrencyAssetEnable = false;
            CurrencyAssestVisible = "Hidden";
            SelectedScripGrp = " All";
            ProdTypeDerivativeSelected = ScripProfilingModel.DerivativeProdType._All.ToString().Replace("_", " ");
            ProdTypeCurrencySelected = ScripProfilingModel.CurrencyProdType.All.ToString();
            IsEnabledProductTypeDerivative = false;
            IsvisibleProductTypeDerivative = "Visible";
            IsEnabledProductTypeCurrency = false;
            IsvisibleProductTypeCurrency = "Hidden";
            EnabledFutureType = false;
            OptTypeEnable = false;
            CurrStrikePriceEnable = false;
            CurrExpDateEnable = false;
            SelectedDerExpDate = " All";
            DerExpDateEnable = false;
            SelectedExPrd = "All";
            ExPrdEnable = false;
            OptTypeLabelVisible = "Visible";
            MoneynessEnabled = false;
            DerStrikePriceEnable = false;
            DerStrikePriceVisible = "Visible";
            CurrStrikePriceVisible = "Hidden";
            IsDeselectVisible = "Hidden";
            IsSegmentEnable = true;
            ScrpGrpEnable = true;
            Is4L6LEnabledGrpBox = true;
            Is4LEnabledGrpBox = true;
            Is6LEnabledGrpBox = true;
#endif
        }

        private void PopulatingExpiryDateDropDown()
        {
#if TWS
            try
            {
                ExpDateLst = new ObservableCollection<string>();
                LocalExpDateLst = new ObservableCollection<int>();
                ExpDateLst.Clear();
                ExpDateLst.Add(ScripProfilingModel.FutType._All.ToString().Replace("_", " "));
                SelectedDerExpDate = ScripProfilingModel.FutType._All.ToString().Replace("_", " ");
                if (MasterSharedMemory.objMasterDerivativeDictBaseBSE != null)
                {
                    foreach (var item in MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Cast<DerivativeMasterBase>().GroupBy(x => x.ExpiryDate).Select(x => x.FirstOrDefault().ExpiryDate))
                    {
                        ExpDateLst.Add(item);
                    }


                }
            }
            catch (Exception e) { ExceptionUtility.LogError(e); }
#endif
        }

        private void PopulatingCurrencyAssetDropDown()
        {
#if TWS
            try
            {
                CurrAsset = new ObservableCollection<string>();
                CurrAsset.Add(ScripProfilingModel.DerivativeProdType._All.ToString().Replace("_", " "));
                SelectedCurrAsset = ScripProfilingModel.DerivativeProdType._All.ToString().Replace("_", " ");
                if (MasterSharedMemory.objMasterCurrencyDictBaseBSE != null)
                {
                    foreach (var item in MasterSharedMemory.objMasterCurrencyDictBaseBSE.Values.Cast<CurrencyMasterBase>().GroupBy(x => x.UnderlyingAsset).Select(x => x.FirstOrDefault().UnderlyingAsset))
                    {
                        CurrAsset.Add(item);
                    }
                }
            }
            catch (Exception e) { ExceptionUtility.LogError(e); }
#endif
        }

        private void PopulatingExpiryPrdDropDown()
        {
#if TWS
            SelectedExPrd = ScripProfilingModel.ExpPrd.Monthly.ToString();
            ExPrd.Add(ScripProfilingModel.FutType._All.ToString().Replace("_", " "));
            ExPrd.Add(ScripProfilingModel.ExpPrd.Weekly.ToString());
            ExPrd.Add(ScripProfilingModel.ExpPrd.Monthly.ToString());
            ExPrd.Add(ScripProfilingModel.ExpPrd.Quaterly.ToString());
            ExPrd.Add(ScripProfilingModel.ExpPrd.Half_Yearly.ToString().Replace("_", " "));
#endif
        }

        private void onChangeOfScripSegment()
        {
#if TWS
            try
            {
                if (ScripSelectedSegment == ScripProfilingModel.ScripSegment.Derivative.ToString())
                {
                    ProdTypeDerivativeSelected = ScripProfilingModel.DerivativeProdType._All.ToString().Replace("_", " ");
                    ProdTypeCurrencySelected = ScripProfilingModel.CurrencyProdType.All.ToString();
                    IsSegmentEnable = true;
                    IsEnabledProductTypeDerivative = true;
                    IsvisibleProductTypeDerivative = "Visible";
                    IsEnabledProductTypeCurrency = false;
                    IsvisibleProductTypeCurrency = "Hidden";
                    DerivativeAssetEnable = true;
                    DerivativeAssestVisible = "Visible";
                    CurrencyAssetEnable = false;
                    CurrencyAssestVisible = "Hidden";
                    SelectedScripGrp = " All";
                    ScrpGrpEnable = false;
                    EnabledFutureType = false;
                    OptTypeEnable = false;
                    CurrStrikePriceEnable = false;
                    CurrStrikePriceVisible = "Hidden";
                    CurrExpDateEnable = false;
                    DerExpDateEnable = false;
                    SelectedExPrd = " All";
                    ExPrdEnable = false;
                    OptTypeLabelVisible = "Visible";
                    DerStrikePriceEnable = false;
                    DerStrikePriceVisible = "Visible";
                    Is4L6LEnabledGrpBox = false;
                    Is4LEnabledGrpBox = false;
                    Is6LEnabledGrpBox = false;
                }
                else if (ScripSelectedSegment == ScripProfilingModel.ScripSegment.Currency.ToString())
                {
                    ProdTypeDerivativeSelected = ScripProfilingModel.DerivativeProdType._All.ToString().Replace("_", " ");
                    ProdTypeCurrencySelected = ScripProfilingModel.CurrencyProdType.All.ToString();
                    IsSegmentEnable = true;
                    IsEnabledProductTypeCurrency = true;
                    IsvisibleProductTypeCurrency = "Visible";
                    IsEnabledProductTypeDerivative = false;
                    IsvisibleProductTypeDerivative = "Hidden";
                    DerivativeAssetEnable = false;
                    DerivativeAssestVisible = "Hidden";
                    CurrencyAssetEnable = true;
                    CurrencyAssestVisible = "Visible";
                    ScrpGrpEnable = false;
                    EnabledFutureType = false;
                    OptTypeEnable = false;
                    CurrStrikePriceEnable = false;
                    CurrExpDateEnable = false;
                    DerExpDateEnable = false;
                    ExPrdEnable = false;
                    OptTypeLabelVisible = "Visible";
                    FutTypeLabelVisible = "Hidden";
                    DerStrikePriceEnable = false;
                    DerStrikePriceVisible = "Visible";
                    CurrStrikePriceVisible = "Hidden";
                    CurrStrikePriceEnable = false;
                    DerStrikePriceSelected = TempDerStrikePrice[0];
                    Is4L6LEnabledGrpBox = false;
                    Is4LEnabledGrpBox = false;
                    Is6LEnabledGrpBox = false;
                }
                else
                {
                    EnableDisable();
                    if (ScripSelectedSegment == ScripProfilingModel.ScripSegment.All.ToString())
                    {
                        Is4L6LEnabledGrpBox = false;
                        ScrpGrpEnable = false;
                    }
                }
                PopulatingEquity1Grid();
            }
            catch (Exception e) { ExceptionUtility.LogError(e); }
#endif
        }

        private void OnChangeOfProdTypeCurrency()
        {
#if TWS
            try
            {
                if (ProdTypeCurrencySelected == ScripProfilingModel.CurrencyProdType.Future.ToString())
                {
                    EnabledFutureType = true;
                    FuturetypeVisible = "Visible";
                    OptTypeEnable = false;
                    OptionTypeVisible = "Hidden";
                    CurrExpDateEnable = true;
                    FutTypeLabelVisible = "Visible";
                    OptTypeLabelVisible = "Hidden";
                    DerExpDateEnable = false;
                    DerExpDateVisible = "Hidden";
                    CurrExpDateVisible = "Visible";
                    CurrExpDateEnable = true;
                    if (CurrStrikePrice != null)
                        CurrStrikePriceSelected = CurrStrikePrice[0];
                }
                else if (ProdTypeCurrencySelected == ScripProfilingModel.CurrencyProdType.Option.ToString())
                {
                    EnabledFutureType = false;
                    FuturetypeVisible = "Hidden";
                    OptTypeEnable = true;
                    OptionTypeVisible = "Visible";
                    CurrExpDateVisible = "Visible";
                    CurrExpDateEnable = true;
                    OptTypeLabelVisible = "Visible";
                    FutTypeLabelVisible = "Hidden";
                    CurrStrikePriceEnable = true;
                    CurrStrikePriceVisible = "Visible";
                    DerStrikePriceEnable = false;
                    DerStrikePriceVisible = "Hidden";
                    DerStrikePriceSelected = TempDerStrikePrice[0];
                }
                else if (ProdTypeCurrencySelected == ScripProfilingModel.CurrencyProdType.Pair_Option.ToString().Replace("_", " ") || ProdTypeCurrencySelected == ScripProfilingModel.CurrencyProdType.Straddle.ToString() || ProdTypeCurrencySelected == ScripProfilingModel.CurrencyProdType.All.ToString())
                {
                    EnabledFutureType = false;
                    FuturetypeVisible = "Hidden";
                    OptTypeEnable = false;
                    OptionTypeVisible = "Visible";
                    CurrExpDateEnable = false;
                    OptTypeLabelVisible = "Visible";
                    FutTypeLabelVisible = "Hidden";
                    CurrStrikePriceEnable = false;
                    if (CurrStrikePrice != null && CurrStrikePrice.Count > 0)
                        CurrStrikePriceSelected = CurrStrikePrice[0];
                }

                if (ProdTypeCurrencySelected == ScripProfilingModel.CurrencyProdType.Option.ToString())
                {
                    CurrAsset = new ObservableCollection<string>();
                    CurrAsset.Add(ScripProfilingModel.DerivativeProdType._All.ToString().Replace("_", " "));
                    foreach (var item in MasterSharedMemory.objMasterCurrencyDictBaseBSE.Values.Cast<CurrencyMasterBase>().Where(x => x.InstrumentType == "OPTCUR").OrderBy(x => x.UnderlyingAsset).Select(x => x.UnderlyingAsset).Distinct())
                    {
                        CurrAsset.Add(item);
                    }
                    SelectedCurrAsset = ScripProfilingModel.DerivativeProdType._All.ToString().Replace("_", " ");



                    CurrExpDateLst.Clear();
                    CurrExpDateLst.Add(ScripProfilingModel.FutType._All.ToString().Replace("_", " "));
                    foreach (var item in MasterSharedMemory.objMasterCurrencyDictBaseBSE.Values.Where(x => x.InstrumentType == "OPTCUR" && ((DateTime.Parse(x.ExpiryDate) >= DateTime.Today))).OrderBy(x => x.ExpiryDate).Select(x => (DateTime.ParseExact(x.ExpiryDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture).ToString("yy-MM-dd"))).Distinct())
                    {
                        CurrExpDateLst.Add(item);
                    }

                    CurrExpDateLst = new ObservableCollection<string>(CurrExpDateLst.OrderBy(p => p));
                    var sortedDatelst = DisplayDateInComboBox(CurrExpDateLst);
                    CurrExpDateLst.Clear();
                    CurrExpDateLst = sortedDatelst;

                    SelectedCurrExpDate = CurrExpDateLst[0];
                }
                else if (ProdTypeCurrencySelected == ScripProfilingModel.CurrencyProdType.Future.ToString())
                {
                    CurrAsset = new ObservableCollection<string>();
                    CurrAsset.Add(ScripProfilingModel.DerivativeProdType._All.ToString().Replace("_", " "));
                    foreach (var item in MasterSharedMemory.objMasterCurrencyDictBaseBSE.Values.Cast<CurrencyMasterBase>().Where(x => x.InstrumentType == "FUTCUR" || x.InstrumentType == "FUTIRT" || x.InstrumentType == "FUTIRD").OrderBy(x => x.UnderlyingAsset).Select(x => x.UnderlyingAsset).Distinct())
                    {
                        CurrAsset.Add(item);
                    }
                    SelectedCurrAsset = ScripProfilingModel.DerivativeProdType._All.ToString().Replace("_", " ");
                }
                else if (ProdTypeCurrencySelected == ScripProfilingModel.CurrencyProdType.Straddle.ToString())
                {
                    CurrAsset = new ObservableCollection<string>();
                    CurrAsset.Add(ScripProfilingModel.DerivativeProdType._All.ToString().Replace("_", " "));
                    foreach (var item in MasterSharedMemory.objMasterCurrencyDictBaseBSE.Values.Cast<CurrencyMasterBase>().Where(x => x.StrategyID == 15 && x.ComplexInstrumentType == 2).OrderBy(x => x.UnderlyingAsset).Select(x => x.UnderlyingAsset).Distinct())
                    {
                        CurrAsset.Add(item);
                    }

                    SelectedCurrAsset = ScripProfilingModel.DerivativeProdType._All.ToString().Replace("_", " ");
                }
                else if (ProdTypeCurrencySelected == ScripProfilingModel.CurrencyProdType.Pair_Option.ToString().Replace("_", " "))
                {
                    CurrAsset = new ObservableCollection<string>();
                    CurrAsset.Add(ScripProfilingModel.DerivativeProdType._All.ToString().Replace("_", " "));
                    foreach (var item in MasterSharedMemory.objMasterCurrencyDictBaseBSE.Values.Cast<CurrencyMasterBase>().Where(x => x.StrategyID == 28 && x.ComplexInstrumentType == 2).OrderBy(x => x.UnderlyingAsset).Select(x => x.UnderlyingAsset).Distinct())
                    {
                        CurrAsset.Add(item);
                    }
                    SelectedCurrAsset = ScripProfilingModel.DerivativeProdType._All.ToString().Replace("_", " ");
                }
            }
            catch (Exception e) { ExceptionUtility.LogError(e); }
#endif
        }

        private void OnChangeOfCurrencyAssets()
        {
#if TWS
            try
            {
                if (SelectedCurrAsset == " All" && SelectedCurrAsset != null)
                {
                    CurrExpDateLst.Clear();
                    CurrExpDateLst.Add(ScripProfilingModel.FutType._All.ToString().Replace("_", " "));
                    foreach (var item in MasterSharedMemory.objMasterCurrencyDictBaseBSE.Values.Where(x => x.InstrumentType == "FUTCUR" || x.InstrumentType == "FUTIRD" && x.InstrumentType == "FUTIRT" && ((DateTime.Parse(x.ExpiryDate) >= DateTime.Today))).OrderBy(x => x.ExpiryDate).Select(x => (DateTime.ParseExact(x.ExpiryDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture).ToString("yy-MM-dd"))).Distinct())
                    {
                        CurrExpDateLst.Add(item);
                    }

                    CurrExpDateLst = new ObservableCollection<string>(CurrExpDateLst.OrderBy(p => p));
                    var sortedDatelst = DisplayDateInComboBox(CurrExpDateLst);
                    CurrExpDateLst.Clear();
                    CurrExpDateLst = sortedDatelst;

                    SelectedCurrExpDate = CurrExpDateLst[0];
                }
                else
                {
                    CurrExpDateLst.Clear();
                    CurrExpDateLst.Add(ScripProfilingModel.FutType._All.ToString().Replace("_", " "));
                    foreach (var item in MasterSharedMemory.objMasterCurrencyDictBaseBSE.Values.Where(x => x.UnderlyingAsset == SelectedCurrAsset).OrderBy(x => x.ExpiryDate).Select(x => (DateTime.ParseExact(x.ExpiryDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture).ToString("yy-MM-dd"))).Distinct())
                    {
                        CurrExpDateLst.Add(item);
                    }

                    CurrExpDateLst = new ObservableCollection<string>(CurrExpDateLst.OrderBy(p => p));
                    var sortedDatelst = DisplayDateInComboBox(CurrExpDateLst);
                    CurrExpDateLst.Clear();
                    CurrExpDateLst = sortedDatelst;

                    SelectedCurrExpDate = CurrExpDateLst[0];
                }

                if ((SelectedCurrAsset != " All" && SelectedCurrAsset != null) && (ProdTypeCurrencySelected == "Option"))
                {
                    List<long> tempCurrStrikePrice = new List<long>();
                    CurrStrikePrice = new List<string>();
                    tempCurrStrikePrice = MasterSharedMemory.objMasterCurrencyDictBaseBSE.Values.Where(x => x.UnderlyingAsset == SelectedCurrAsset).Select(x => x.StrikePrice).Distinct().ToList();
                    for (int i = 0; i < tempCurrStrikePrice.Count; i++)
                    {
                        CurrStrikePrice.Add(String.Format("{0:0.0000}", Convert.ToDecimal(CommonFunctions.DisplayInDecimalFormatTouch(tempCurrStrikePrice[i], 7))).ToString());
                    }
                    CurrStrikePrice.Add(ScripProfilingModel.FutType._All.ToString().Replace("_", " "));
                    CurrStrikePrice.RemoveAt(0);
                    CurrStrikePrice.Sort();
                    CurrStrikePriceSelected = CurrStrikePrice[0];
                    CurrStrikePriceVisible = "Visible";
                    CurrStrikePriceEnable = true;
                    DerStrikePriceVisible = "Hidden";
                }
                else
                {
                    CurrStrikePriceEnable = false;
                }
            }
            catch (Exception e) { ExceptionUtility.LogError(e); }
#endif
        }

        private void onChangeOfDerivativeProductType()
        {
#if TWS
            #region TWS`
            try
            {
                if (ProdTypeDerivativeSelected == ScripProfilingModel.DerivativeProdType.Index_Future.ToString().Replace("_", " ") || ProdTypeDerivativeSelected == ScripProfilingModel.DerivativeProdType.Stock_Future.ToString().Replace("_", " "))
                {
                    EnabledFutureType = true;
                    DerExpDateVisible = "Visible";
                    CurrExpDateVisible = "Hidden";
                    OptTypeEnable = false;
                    FuturetypeVisible = "Visible";
                    DerExpDateEnable = true;
                    FutTypeLabelVisible = "Visible";
                    OptTypeLabelVisible = "Hidden";
                    DerStrikePriceEnable = false;
                    CurrStrikePriceEnable = false;
                    CurrStrikePriceVisible = "Hidden";
                    ExPrdEnable = false;

                }
                else if (ProdTypeDerivativeSelected == ScripProfilingModel.DerivativeProdType.Index_Option.ToString().Replace("_", " ") || ProdTypeDerivativeSelected == ScripProfilingModel.DerivativeProdType.Stock_Option.ToString().Replace("_", " "))
                {
                    OptTypeEnable = true;
                    ExPrdEnable = true;
                    DerExpDateVisible = "Visible";
                    CurrExpDateVisible = "Hidden";
                    DerExpDateEnable = true;
                    EnabledFutureType = false;
                    OptionTypeVisible = "Visible";
                    OptTypeLabelVisible = "Visible";
                    FutTypeLabelVisible = "Hidden";
                    FuturetypeVisible = "Hidden";
                    DerStrikePriceEnable = false;
                    CurrStrikePriceEnable = false;
                    CurrStrikePriceVisible = "Hidden";
                    DerStrikePriceVisible = "Visible";
                    SelectedExPrd = "Monthly";
                }
                else
                {
                    ScrpGrpEnable = false;
                    OptTypeEnable = false;
                    ExPrdEnable = false;
                    DerExpDateEnable = false;
                    OptTypeLabelVisible = "Visible";
                    FutTypeLabelVisible = "Hidden";
                    DerStrikePriceEnable = false;
                    CurrStrikePriceEnable = false;
                    CurrStrikePriceVisible = "Hidden";
                }

                if (ProdTypeDerivativeSelected == ScripProfilingModel.DerivativeProdType.Index_Future.ToString().Replace("_", " "))
                {
                    DerProdSelected = "IF";
                    DerAsset = new ObservableCollection<string>();
                    DerAsset.Add(ScripProfilingModel.FutType._All.ToString().Replace("_", " "));

                    foreach (var item in MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Cast<DerivativeMasterBase>().Where(x => x.InstrumentType == "IF" && x.OptionType == null).OrderBy(x => x.UnderlyingAsset).Select(x => x.UnderlyingAsset).Distinct())//changed assetcode to underlyingasset
                    {
                        DerAsset.Add(item);
                    }
                    SelectedDerAsset = ScripProfilingModel.FutType._All.ToString().Replace("_", " ");
                    ExpDateLst.Clear();
                    foreach (var item in MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Where(x => x.InstrumentType == "IF" || x.InstrumentType == "IO" && x.OptionType == " " && ((DateTime.Parse(x.ExpiryDate) >= DateTime.Today))).OrderBy(x => x.ExpiryDate).Select(x => DateTime.ParseExact(x.ExpiryDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture).ToString("yy-MMM-dd")).Distinct())
                    {
                        ExpDateLst.Add(item);
                    }

                    ExpDateLst = new ObservableCollection<string>(ExpDateLst.OrderBy(p => p));
                    var sortedDatelst = DisplayDateInComboBoxddMMMyyyy(ExpDateLst);
                    ExpDateLst.Clear();
                    ExpDateLst = sortedDatelst;

                    if (ExpDateLst != null)
                        SelectedDerExpDate = ExpDateLst[0];
                    IsSearchEnable = true;
                }
                else if (ProdTypeDerivativeSelected == ScripProfilingModel.DerivativeProdType.Index_Option.ToString().Replace("_", " "))
                {
                    DerProdSelected = "IO";
                    DerAsset = new ObservableCollection<string>();
                    DerAsset.Add(ScripProfilingModel.FutType._All.ToString().Replace("_", " "));
                    foreach (var item in MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Cast<DerivativeMasterBase>().Where(x => x.InstrumentType == "IO" && (x.OptionType == "PE" || x.OptionType == "CE")).OrderBy(x => x.UnderlyingAsset).Select(x => x.UnderlyingAsset).Distinct())
                    {
                        DerAsset.Add(item);
                    }
                    SelectedDerAsset = ScripProfilingModel.FutType._All.ToString().Replace("_", " ");

                    ExpDateLst.Clear();
                    ExpDateLst.Add(ScripProfilingModel.FutType._All.ToString().Replace("_", " "));
                    foreach (var item in MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Where(x => x.InstrumentType == "IO" && (x.OptionType == "PE" || x.OptionType == "CE") && ((DateTime.Parse(x.ExpiryDate) >= DateTime.Today)) && x.ContractType == contractTyp.ToString()).OrderBy(x => x.ExpiryDate).Select(x => DateTime.ParseExact(x.ExpiryDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture).ToString("yy-MMM-dd")).Distinct())
                    {
                        ExpDateLst.Add(item);
                    }

                    ExpDateLst = new ObservableCollection<string>(ExpDateLst.OrderBy(p => p));
                    var sortedDatelst = DisplayDateInComboBoxddMMMyyyy(ExpDateLst);
                    ExpDateLst.Clear();
                    ExpDateLst = sortedDatelst;

                    if (ExpDateLst != null)
                        SelectedDerExpDate = ExpDateLst[0];
                    IsSearchEnable = true;
                }
                else if (ProdTypeDerivativeSelected == ScripProfilingModel.DerivativeProdType.Stock_Future.ToString().Replace("_", " "))
                {
                    DerProdSelected = "SF";
                    DerAsset = new ObservableCollection<string>();
                    DerAsset.Add(ScripProfilingModel.FutType._All.ToString().Replace("_", " "));
                    foreach (var item in MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Cast<DerivativeMasterBase>().Where(x => x.InstrumentType == "SF").OrderBy(x => x.UnderlyingAsset).Select(x => x.UnderlyingAsset).Distinct())
                    {
                        DerAsset.Add(item);
                    }
                    SelectedDerAsset = ScripProfilingModel.FutType._All.ToString().Replace("_", " ");

                    ExpDateLst.Clear();
                    ExpDateLst.Add(ScripProfilingModel.FutType._All.ToString().Replace("_", " "));
                    foreach (var item in MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Where(x => x.InstrumentType == "SF" || x.InstrumentType == "SO" && x.OptionType == " " && ((DateTime.Parse(x.ExpiryDate) >= DateTime.Today))).OrderBy(x => x.ExpiryDate).Select(x => DateTime.ParseExact(x.ExpiryDate, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("yy-MMM-dd")).Distinct())
                    {
                        ExpDateLst.Add(item);
                    }

                    ExpDateLst = new ObservableCollection<string>(ExpDateLst.OrderBy(p => p));
                    var sortedDatelst = DisplayDateInComboBoxddMMMyyyy(ExpDateLst);
                    ExpDateLst.Clear();
                    ExpDateLst = sortedDatelst;

                    if (ExpDateLst != null)
                        SelectedDerExpDate = ExpDateLst[0];
                    IsSearchEnable = true;
                }
                else if (ProdTypeDerivativeSelected == ScripProfilingModel.DerivativeProdType.Stock_Option.ToString().Replace("_", " "))
                {
                    DerProdSelected = "SO";
                    DerAsset = new ObservableCollection<string>();
                    DerAsset.Add(ScripProfilingModel.FutType._All.ToString().Replace("_", " "));
                    foreach (var item in MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Cast<DerivativeMasterBase>().Where(x => x.InstrumentType == "SO").OrderBy(x => x.UnderlyingAsset).Select(x => x.UnderlyingAsset).Distinct())
                    {
                        DerAsset.Add(item);
                    }

                    SelectedDerAsset = ScripProfilingModel.FutType._All.ToString().Replace("_", " ");
                    ExpDateLst.Clear();
                    ExpDateLst.Add(ScripProfilingModel.FutType._All.ToString().Replace("_", " "));
                    foreach (var item in MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Where(x => x.InstrumentType == "SO" || x.InstrumentType == "SF" && (x.OptionType == "PE" || x.OptionType == "CE") && ((DateTime.Parse(x.ExpiryDate) >= DateTime.Today)) && x.ContractType == contractTyp.ToString()).OrderBy(x => x.ExpiryDate).Select(x => DateTime.ParseExact(x.ExpiryDate, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("yy-MMM-dd")).Distinct())
                    {
                        ExpDateLst.Add(item);
                    }

                    ExpDateLst = new ObservableCollection<string>(ExpDateLst.OrderBy(p => p));
                    var sortedDatelst = DisplayDateInComboBoxddMMMyyyy(ExpDateLst);
                    ExpDateLst.Clear();
                    ExpDateLst = sortedDatelst;

                    if (ExpDateLst != null)
                        SelectedDerExpDate = ExpDateLst[0];
                    IsSearchEnable = true;
                }
                else if (ProdTypeDerivativeSelected == ScripProfilingModel.DerivativeProdType.Pair_Option.ToString().Replace("_", " "))
                {
                    DerProdSelected = "PO";
                    DerAsset = new ObservableCollection<string>();
                    DerAsset.Add(ScripProfilingModel.FutType._All.ToString().Replace("_", " "));
                    foreach (var item in MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Cast<DerivativeMasterBase>().Where(x => x.ComplexInstrumentType == 2).OrderBy(x => x.UnderlyingAsset).Select(x => x.UnderlyingAsset).Distinct())
                    {
                        DerAsset.Add(item);
                    }

                    SelectedDerAsset = ScripProfilingModel.FutType._All.ToString().Replace("_", " ");
                    IsSearchEnable = true;
                }
                else if (ProdTypeDerivativeSelected == ScripProfilingModel.FutType._All.ToString().Replace("_", " "))
                {
                    DerAsset = new ObservableCollection<string>();
                    DerAsset.Add(ScripProfilingModel.FutType._All.ToString().Replace("_", " "));
                    foreach (var item in MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Cast<DerivativeMasterBase>().Where(x => x.InstrumentType == "IO" || x.InstrumentType == "IF" || x.InstrumentType == "SO" || x.InstrumentType == "SF").OrderBy(x => x.UnderlyingAsset).Select(x => x.UnderlyingAsset).Distinct())
                    {
                        DerAsset.Add(item);
                    }
                    SelectedDerAsset = ScripProfilingModel.FutType._All.ToString().Replace("_", " ");
                    IsSearchEnable = true;
                }
            }
            catch (Exception e) { ExceptionUtility.LogError(e); }
            #endregion
#elif BOW
            #region BOW
            try
            {
                if (SelectedExchange == ScripProfilingModel.Exchanges.MCX.ToString())
                {
                    ScrpGrpEnable = false;
                    ExPrdEnable = false;
                    IsEnabledProductTypeCurrency = false;
                    IsvisibleProductTypeCurrency = "Hidden";
                    IsvisibleProductTypeDerivative = "Visible";
                    IsEnabledProductTypeDerivative = true;
                    CurrencyAssestVisible = "Hidden";
                    CurrencyAssetEnable = false;
                    DerivativeAssestVisible = "Visible";
                    DerivativeAssetEnable = true;
                    CurrExpDateVisible = "Hidden";
                    CurrExpDateEnable = false;
                    DerExpDateEnable = true;
                    DerExpDateVisible = "Visible";
                    CurrStrikePriceEnable = false;
                    CurrStrikePriceVisible = "Hidden";
                    DerStrikePriceVisible = "Visible";
                    DerStrikePriceEnable = true;
                }

                MasterSharedMemory.oDataAccessLayer.OpenConnection((int)DataAccessLayer.ConnectionDB.Masters);
                if (SelectedExchange == ScripProfilingModel.Exchanges.MCX.ToString() && ProdTypeDerivativeSelected == "AUCSO")
                {
            #region Asset
                    DerAsset = new ObservableCollection<string>();
                    DerAsset.Clear();
                    DerAsset.Add(ScripProfilingModel.ExpPrd.All.ToString());
                    str = "SELECT distinct(MCContractCode) FROM MCXContracts where MCInstrumentName=" + "'" + ProdTypeDerivativeSelected + "';";

                    SQLiteDataReader oSQLiteDataReader = MasterSharedMemory.oDataAccessLayer.ExecuteDataReader((int)ConnectionDB.Masters,str, System.Data.CommandType.Text, null);
                    while (oSQLiteDataReader.Read())
                    {
                        if (oSQLiteDataReader["MCContractCode"] != string.Empty)
                            DerAsset.Add(oSQLiteDataReader["MCContractCode"]?.ToString().Trim());
                    }
                    if (DerAsset != null)
                        SelectedDerAsset = DerAsset[0];
                    DerAsset.Remove(string.Empty);
            #endregion

            #region StrikePrice
                    TempDerStrikePrice = new BindingList<String>();
                    TempDerStrikePrice.Clear();
                    TempDerStrikePrice.Add(ScripProfilingModel.ExpPrd.All.ToString());

                    str = "SELECT distinct(MCStrikePrice) FROM MCXContracts where MCInstrumentName=" + "'" + ProdTypeDerivativeSelected + "';";
                    oSQLiteDataReader = MasterSharedMemory.oDataAccessLayer.ExecuteDataReader((int)ConnectionDB.Masters,str, System.Data.CommandType.Text, null);
                    while (oSQLiteDataReader.Read())
                    {
                        if (oSQLiteDataReader["MCStrikePrice"] != string.Empty)
                            TempDerStrikePrice.Add(oSQLiteDataReader["MCStrikePrice"]?.ToString().Trim());
                    }
                    TempDerStrikePrice.Remove(string.Empty);
                    DerStrikePriceSelected = TempDerStrikePrice[0];
            #endregion

            #region Expiry Date
                    ExpDateLst = new ObservableCollection<string>();
                    ExpDateLst.Clear();
                    ExpDateLst.Add(ScripProfilingModel.ExpPrd.All.ToString());

                    LocalExpDateLst = new ObservableCollection<string>();
                    str = "SELECT distinct(MCDisplayExpiryDate) FROM MCXContracts where MCInstrumentName=" + "'" + ProdTypeDerivativeSelected + "';";
                    oSQLiteDataReader = MasterSharedMemory.oDataAccessLayer.ExecuteDataReader((int)ConnectionDB.Masters,str, System.Data.CommandType.Text, null);
                    while (oSQLiteDataReader.Read())
                    {
                        if (oSQLiteDataReader["MCDisplayExpiryDate"] != string.Empty)
                            LocalExpDateLst.Add(oSQLiteDataReader["MCDisplayExpiryDate"]?.ToString().Trim());
                    }
                    LocalExpDateLst.Remove(string.Empty);
                    foreach (var item in LocalExpDateLst)
                    {
                        string date = DateTime.Parse(item).ToString("dd/MM/yy");
                        ExpDateLst.Add(date);
                    }
                    if (ExpDateLst != null)
                        SelectedDerExpDate = ExpDateLst[0];
            #endregion
                }
                else if (SelectedExchange == ScripProfilingModel.Exchanges.MCX.ToString() && ProdTypeDerivativeSelected == "FUTCOM")
                {
            #region Asset
                    DerAsset = new ObservableCollection<string>();
                    DerAsset.Clear();
                    DerAsset.Add(ScripProfilingModel.ExpPrd.All.ToString());
                    str = "SELECT distinct(MCContractCode) FROM MCXContracts where MCInstrumentName=" + "'" + ProdTypeDerivativeSelected + "';";

                    SQLiteDataReader oSQLiteDataReader = MasterSharedMemory.oDataAccessLayer.ExecuteDataReader((int)ConnectionDB.Masters,str, System.Data.CommandType.Text, null);
                    while (oSQLiteDataReader.Read())
                    {
                        if (oSQLiteDataReader["MCContractCode"] != string.Empty)
                            DerAsset.Add(oSQLiteDataReader["MCContractCode"]?.ToString().Trim());
                    }
                    if (DerAsset != null)
                        SelectedDerAsset = DerAsset[0];
                    DerAsset.Remove(string.Empty);
            #endregion

            #region StrikePrice
                    TempDerStrikePrice = new BindingList<String>();
                    TempDerStrikePrice.Clear();
                    TempDerStrikePrice.Add(ScripProfilingModel.ExpPrd.All.ToString());

                    str = "SELECT distinct(MCStrikePrice) FROM MCXContracts where MCInstrumentName=" + "'" + ProdTypeDerivativeSelected + "';";
                    oSQLiteDataReader = MasterSharedMemory.oDataAccessLayer.ExecuteDataReader((int)ConnectionDB.Masters,str, System.Data.CommandType.Text, null);
                    while (oSQLiteDataReader.Read())
                    {
                        if (oSQLiteDataReader["MCStrikePrice"] != string.Empty)
                            TempDerStrikePrice.Add(oSQLiteDataReader["MCStrikePrice"]?.ToString().Trim());
                    }
                    TempDerStrikePrice.Remove(string.Empty);
                    DerStrikePriceSelected = TempDerStrikePrice[0];
            #endregion

            #region Expiry Date
                    ExpDateLst = new ObservableCollection<string>();
                    ExpDateLst.Clear();
                    ExpDateLst.Add(ScripProfilingModel.ExpPrd.All.ToString());

                    LocalExpDateLst = new ObservableCollection<string>();
                    str = "SELECT distinct(MCDisplayExpiryDate) FROM MCXContracts where MCInstrumentName=" + "'" + ProdTypeDerivativeSelected + "';";
                    oSQLiteDataReader = MasterSharedMemory.oDataAccessLayer.ExecuteDataReader((int)ConnectionDB.Masters,str, System.Data.CommandType.Text, null);
                    while (oSQLiteDataReader.Read())
                    {
                        if (oSQLiteDataReader["MCDisplayExpiryDate"] != string.Empty)
                            LocalExpDateLst.Add(oSQLiteDataReader["MCDisplayExpiryDate"]?.ToString().Trim());
                    }
                    LocalExpDateLst.Remove(string.Empty);
                    foreach (var item in LocalExpDateLst)
                    {
                        string date = DateTime.Parse(item).ToString("dd/MM/yy");
                        ExpDateLst.Add(date);
                    }
                    if (ExpDateLst != null)
                        SelectedDerExpDate = ExpDateLst[0];
            #endregion
                }
                else if (SelectedExchange == ScripProfilingModel.Exchanges.MCX.ToString() && ProdTypeDerivativeSelected == "OPTFUT")
                {
            #region Asset
                    DerAsset = new ObservableCollection<string>();
                    DerAsset.Clear();
                    DerAsset.Add(ScripProfilingModel.ExpPrd.All.ToString());
                    str = "SELECT distinct(MCContractCode) FROM MCXContracts where MCInstrumentName=" + "'" + ProdTypeDerivativeSelected + "';";

                    SQLiteDataReader oSQLiteDataReader = MasterSharedMemory.oDataAccessLayer.ExecuteDataReader((int)ConnectionDB.Masters,str, System.Data.CommandType.Text, null);
                    while (oSQLiteDataReader.Read())
                    {
                        if (oSQLiteDataReader["MCContractCode"] != string.Empty)
                            DerAsset.Add(oSQLiteDataReader["MCContractCode"]?.ToString().Trim());
                    }
                    if (DerAsset != null)
                        SelectedDerAsset = DerAsset[0];
                    DerAsset.Remove(string.Empty);
            #endregion

            #region StrikePrice
                    TempDerStrikePrice = new BindingList<String>();
                    TempDerStrikePrice.Clear();
                    TempDerStrikePrice.Add(ScripProfilingModel.ExpPrd.All.ToString());

                    str = "SELECT distinct(MCStrikePrice) FROM MCXContracts where MCInstrumentName=" + "'" + ProdTypeDerivativeSelected + "';";
                    oSQLiteDataReader = MasterSharedMemory.oDataAccessLayer.ExecuteDataReader((int)ConnectionDB.Masters,str, System.Data.CommandType.Text, null);
                    while (oSQLiteDataReader.Read())
                    {
                        if (oSQLiteDataReader["MCStrikePrice"] != string.Empty)
                            TempDerStrikePrice.Add(oSQLiteDataReader["MCStrikePrice"]?.ToString().Trim());
                    }
                    TempDerStrikePrice.Remove(string.Empty);
                    DerStrikePriceSelected = TempDerStrikePrice[0];
            #endregion


            #region Expiry Date
                    ExpDateLst = new ObservableCollection<string>();
                    ExpDateLst.Clear();
                    ExpDateLst.Add(ScripProfilingModel.ExpPrd.All.ToString());

                    LocalExpDateLst = new ObservableCollection<string>();
                    str = "SELECT distinct(MCDisplayExpiryDate) FROM MCXContracts where MCInstrumentName=" + "'" + ProdTypeDerivativeSelected + "';";
                    oSQLiteDataReader = MasterSharedMemory.oDataAccessLayer.ExecuteDataReader((int)ConnectionDB.Masters,str, System.Data.CommandType.Text, null);
                    while (oSQLiteDataReader.Read())
                    {
                        if (oSQLiteDataReader["MCDisplayExpiryDate"] != string.Empty)
                            LocalExpDateLst.Add(oSQLiteDataReader["MCDisplayExpiryDate"]?.ToString().Trim());
                    }
                    LocalExpDateLst.Remove(string.Empty);
                    foreach (var item in LocalExpDateLst)
                    {
                        string date = DateTime.Parse(item).ToString("dd/MM/yy");
                        ExpDateLst.Add(date);
                    }
                    if (ExpDateLst != null)
                        SelectedDerExpDate = ExpDateLst[0];
            #endregion
                }
                MasterSharedMemory.oDataAccessLayer.CloseConnection((int)DataAccessLayer.ConnectionDB.Masters);
            }
            catch (Exception e)
            {
                ExceptionUtility.LogError(e);
            }
            #endregion
#endif
        }

        #region StaticNotifyPropertyChangedEvent
        public static event EventHandler<PropertyChangedEventArgs> StaticPropertyChanged
                 = delegate { };
        private static void NotifyStaticPropertyChanged(string propertyName)
        {
            StaticPropertyChanged(null, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region NotifyPropertyChange
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName = "")
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                this.PropertyChanged(this, e);
            }
        }
        #endregion
    }

    public class ListBoxSelectedItemsBehavior : Behavior<System.Windows.Controls.DataGrid>
    {
        protected override void OnAttached()
        {
            AssociatedObject.SelectionChanged += AssociatedObjectSelectionChanged;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.SelectionChanged -= AssociatedObjectSelectionChanged;
        }

        void AssociatedObjectSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedItems = new List<ScripMaster>();
            SelectedItems = AssociatedObject.SelectedItems;
        }

        public static readonly DependencyProperty SelectedItemsProperty =
            DependencyProperty.Register("SelectedItems", typeof(IEnumerable), typeof(ListBoxSelectedItemsBehavior),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public object SelectedItems
        {
            get { return (object)GetValue(SelectedItemsProperty); }
            set
            {
                SetValue(SelectedItemsProperty, value);
                SetSelectedItems();
            }
        }

        private void SetSelectedItems()
        {
            ScripProfilingVM.SelectedItem = SelectedItems;
        }
    }
}


