using CommonFrontEnd.Common;
using CommonFrontEnd.View;
using CommonFrontEnd.Model.Order;
using CommonFrontEnd.SharedMemories;
using CommonFrontEnd.View.UserControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using static CommonFrontEnd.Model.Order.OrderModel;
using CommonFrontEnd.Global;
using System.Diagnostics;
using System.Collections.ObjectModel;
using CommonFrontEnd.Model;
using CommonFrontEnd.Processor.Order;
using System.Globalization;
using CommonFrontEnd.ViewModel.Touchline;
using System.Windows.Input;
using System.Timers;

namespace CommonFrontEnd.ViewModel.Order
{
    public partial class ScripInsertUC_VM : INotifyPropertyChanged
    {
        /// <summary>
        /// This control is used in best five and order entry windows : Apoorva Sharma 
        /// </summary>
        #region Properties
        long num;
        string AppendedChar = string.Empty;
        int TimeResetCounter = 0;
        private static bool _TouchlineInsert = false;

        public static bool TouchLineInsert
        {
            get { return _TouchlineInsert; }
            set { _TouchlineInsert = value; }
        }


        private static List<string> _SegmentLst;
        public static List<string> SegmentLst
        {
            get { return _SegmentLst; }
            set { _SegmentLst = value; NotifyStaticPropertyChanged("SegmentLst"); }
        }

        private static string _SelectedSegment;
        public static string SelectedSegment
        {
            get { return _SelectedSegment; }
            set
            {
                if (value != null && _SelectedSegment != value)
                {
                    _SelectedSegment = value;
                    NotifyStaticPropertyChanged("SelectedSegment");
                    HideShowControls();
                    PopulatingDropDowns();
                }
                else if (value == null)
                    _SelectedSegment = value;
            }
        }

        private static string _SelectedScripId;
        public static string SelectedScripId
        {
            get { return _SelectedScripId; }
            set
            {
                if (value != null && _SelectedScripId != value)
                {
                    _SelectedScripId = value;
                    NotifyStaticPropertyChanged("SelectedScripId");
                    OnChange_ScripId();
                }
                else if (value == null)
                    _SelectedScripId = value;
            }
        }

        private static long? _SelectedScripCode;
        public static long? SelectedScripCode
        {
            get { return _SelectedScripCode; }
            set
            {
                if (value != null && _SelectedScripCode != value)
                {
                    _SelectedScripCode = value;
                    NotifyStaticPropertyChanged("SelectedScripCode");
                    OnChange_ScripCode();
                }
                else if (value == null)
                    _SelectedScripCode = value;
            }
        }

        private static ObservableCollection<string> _IntrTypeLst;
        public static ObservableCollection<string> IntrTypeLst
        {
            get { return _IntrTypeLst; }
            set { _IntrTypeLst = value; NotifyStaticPropertyChanged("IntrTypeLst"); }
        }

        private static string _IntrTypeSelected;
        public static string IntrTypeSelected
        {
            get { return _IntrTypeSelected; }
            set
            {
                if (value != null && _IntrTypeSelected != value)
                {
                    _IntrTypeSelected = value;
                    NotifyStaticPropertyChanged("IntrTypeSelected");
                    OnChange_IntrType();
                }
                else if (value == null)
                    _IntrTypeSelected = value;
            }
        }

        private static ObservableCollection<string> _AssetList;
        public static ObservableCollection<string> AssetList
        {
            get { return _AssetList; }
            set { _AssetList = value; NotifyStaticPropertyChanged("AssetList"); }
        }

        private static string _AssetSelected;
        public static string AssetSelected
        {
            get { return _AssetSelected; }
            set
            {
                if (value != null && _AssetSelected != value)
                {
                    _AssetSelected = value;
                    NotifyStaticPropertyChanged("AssetSelected");
                    OnChange_Asset();
                }
                else if (value == null)
                    _AssetSelected = value;
            }
        }

        private static ObservableCollection<string> _ExpDateLst;
        public static ObservableCollection<string> ExpDateLst
        {
            get { return _ExpDateLst; }
            set { _ExpDateLst = value; NotifyStaticPropertyChanged("ExpDateLst"); }
        }

        private static string _ExpDateSelected;
        public static string ExpDateSelected
        {
            get { return _ExpDateSelected; }
            set
            {
                if (value != null && _ExpDateSelected != value)
                {
                    _ExpDateSelected = value;
                    NotifyStaticPropertyChanged("ExpDateSelected");
                    OnChange_Expiry();
                }
                else if (value == null)
                    _ExpDateSelected = value;
            }
        }

        private static ObservableCollection<string> _StkPrcLst;
        public static ObservableCollection<string> StkPrcLst
        {
            get { return _StkPrcLst; }
            set { _StkPrcLst = value; NotifyStaticPropertyChanged("StkPrcLst"); }
        }

        private static string _StkPriceSelected;
        public static string StkPriceSelected
        {
            get { return _StkPriceSelected; }
            set
            {
                if (value != null && _StkPriceSelected != value)
                {
                    _StkPriceSelected = value;
                    NotifyStaticPropertyChanged("StkPriceSelected");
                    OnChange_StkPrice();
                }
                else if (value == null)
                    _StkPriceSelected = value;
            }
        }

        private static ObservableCollection<string> _InstrumentNameColl;
        public static ObservableCollection<string> InstrumentNameColl
        {
            get { return _InstrumentNameColl; }
            set { _InstrumentNameColl = value; NotifyStaticPropertyChanged("InstrumentNameColl"); }
        }

        private static string _InstrNameSelected;
        public static string InstrNameSelected
        {
            get { return _InstrNameSelected; }
            set
            {
                if (value != null && _InstrNameSelected != value)
                {
                    _InstrNameSelected = value;
                    NotifyStaticPropertyChanged("InstrNameSelected");
                    OnChange_InstrName();
                }
                else if (value == null)
                    _InstrNameSelected = value;
            }
        }

        public static string _SelectedName;
        public static string SelectedName
        {
            get { return _SelectedName; }
            set
            {
                if (value != null && _SelectedName != value)
                {
                    _SelectedName = value;
                    NotifyStaticPropertyChanged("SelectedName");
                }
                else if (value == null)
                    _SelectedName = value;
            }
        }





        private static string _Selected_EXCH;
        public static string Selected_EXCH
        {
            get { return _Selected_EXCH; }
            set
            {
                _Selected_EXCH = value;
                NotifyStaticPropertyChanged("Selected_EXCH");

                //  PopulatingScripProfileSegment();
            }
        }
        private static int _DecimalPoint;
        public static int DecimalPoint
        {
            get { return _DecimalPoint; }
            set
            {
                _DecimalPoint = value;
                NotifyStaticPropertyChanged("DecimalPoint");

            }
        }
        private bool _isFiveLacSelected = false;

        public bool isFiveLacSelected
        {
            get { return _isFiveLacSelected; }
            set { _isFiveLacSelected = value; NotifyPropertyChanged(nameof(isFiveLacSelected)); }
        }


        long stkprice = 0;
        Timer TimerCombo = new Timer();
        private string _corpEnability;

        public string corpEnability
        {
            get { return _corpEnability; }
            set { _corpEnability = value; NotifyPropertyChanged("corpEnability"); }
        }
        public static Action<long> OnScripIDOrCodeChange;

        public static Action<long> ChangeScripCode;

        public static Action<long> OnScripAddTouchLine;

        private static bool flag = true;
        private static bool changeScripId = true;

        

        





        private static string selectedNameDebt;
        private static string _SelectedScripIdDebt;
        private List<string> typesOfDerivateSegmentLst;
        public List<string> TypesOfDerivateSegmentLst
        {
            get { return typesOfDerivateSegmentLst; }
            set { typesOfDerivateSegmentLst = value; NotifyPropertyChanged("TypesOfDerivateSegmentLst"); }
        }

        

        

        private static List<long> _ScripCodeSelectedLst;
        public static List<long> ScripCodeSelectedLst
        {
            get { return _ScripCodeSelectedLst; }
            set { _ScripCodeSelectedLst = value; NotifyStaticPropertyChanged("ScripCodeSelectedLst"); }
        }



        private static long _code;
        public static long code
        {
            get { return _code; }
            set { _code = value; NotifyStaticPropertyChanged("code"); }
        }

        private static List<Scrip_Code_ID> scripEquityDetails;
        public static List<Scrip_Code_ID> ScripEquityDetails
        {
            get { return scripEquityDetails; }
            set { scripEquityDetails = value; NotifyStaticPropertyChanged("ScripEquityDetails"); }
        }

        private string selectedTypesOfDerivateSegment;
        public string SelectedTypesOfDerivateSegment
        {
            get { return selectedTypesOfDerivateSegment; }
            set { selectedTypesOfDerivateSegment = value; NotifyPropertyChanged("SelectedTypesOfDerivateSegment"); }
        }
        private static string _ddlEquityVisible;

        public static string ddlEquityVisible
        {
            get { return _ddlEquityVisible; }
            set { _ddlEquityVisible = value; NotifyStaticPropertyChanged("ddlEquityVisible"); }
        }



        

       

        private string _ScripIdSelected;
        public string ScripIdSelected
        {
            get { return _ScripIdSelected; }
            set { _ScripIdSelected = value; NotifyPropertyChanged("ScripIdSelected"); }
        }
        


        
        

        private static string _EquityVis;
        public static string EquityVis
        {
            get { return _EquityVis; }
            set
            {
                _EquityVis = value; NotifyStaticPropertyChanged("EquityVis");

            }
        }
        private static string _DerivativeVis;
        public static string DerivativeVis
        {
            get { return _DerivativeVis; }
            set { _DerivativeVis = value; NotifyStaticPropertyChanged("DerivativeVis"); }
        }

        private static string _FutureVis;
        public static string FutureVis
        {
            get { return _FutureVis; }
            set { _FutureVis = value; NotifyStaticPropertyChanged("FutureVis"); }
        }
        private static string _CallVis;
        public static string CallVis
        {
            get { return _CallVis; }
            set { _CallVis = value; NotifyStaticPropertyChanged("CallVis"); }
        }
        private static string _ddlDebtVisible;

        public static string ddlDebtVisible
        {
            get { return _ddlDebtVisible; }
            set { _ddlDebtVisible = value; NotifyStaticPropertyChanged("ddlDebtVisible"); }
        }

        public static string SelectedScripIdDebt
        {
            get { return _SelectedScripIdDebt; }
            set
            {
                if (value != null && _SelectedScripIdDebt != value)
                {
                    _SelectedScripIdDebt = value;
                    NotifyStaticPropertyChanged("SelectedScripIdDebt");
                    OnChangeOfScripIdDebt();
                }
                else if (value == null)
                {
                    _SelectedScripIdDebt = value;
                }
            }
        }
        private void EnterUsingUserControl()
        {
            if (TouchLineInsert)
            {
                if (SelectedScripCode != null)
                    OnScripAddTouchLine((long)SelectedScripCode);
                //else if (SelectedScripCode != null && ddlEquityVisible == "Visible")
                //    OnScripAddTouchLine((long)SelectedScripCode);
                //else if (SelectedScripCodeDebt != null && ddlDebtVisible == "Visible")
                //    OnScripAddTouchLine((long)SelectedScripCodeDebt);
            }
        }
        private static void OnChangeOfScripIdDebt()
        {
            if (changeScripId)
            {
                //Scrip_Code_ID objScrip_Code_ID = ScripEquityDetails.Where(x => x.ScripId == SelectedScripId).FirstOrDefault();
                flag = false;
                if (SelectedScripIdDebt != null)
                {
                    SelectedScripCodeDebt = CommonFunctions.GetScripCode(SelectedScripIdDebt, Enumerations.Exchange.BSE, Enumerations.Segment.Equity);//BSE Equity
                    SelectedNameDebt = MasterSharedMemory.objMastertxtDictBaseBSE[(long)SelectedScripCodeDebt].ScripName;//TODO TBD2017
                }
                else
                {
                    SelectedNameDebt = string.Empty;
                    SelectedScripCodeDebt = null;
                }
                //if (SelectedScripCode != null)
                //    Messenger.Default.Send(SelectedScripCode);
                //if (objScrip_Code_ID != null)
                //{
                //    SelectedName = objScrip_Code_ID.ScripName;
                //    SelectedScripCode = objScrip_Code_ID.ScripCode;
                //}
                // ScripCodeLst.Add(objScrip_Code_ID.ScripCode);
            }
            changeScripId = true;

            if (SelectedScripCodeDebt != null && OnScripIDOrCodeChange != null)
                OnScripIDOrCodeChange((long)SelectedScripCodeDebt);
            //else if (OnScripIDOrCodeChange != null)
            //    OnScripIDOrCodeChange(0);
            // Messenger.Default.Send(SelectedScripCode);
        }

        public static string SelectedNameDebt
        {
            get { return selectedNameDebt; }
            set { selectedNameDebt = value; NotifyStaticPropertyChanged("SelectedNameDebt"); }
        }
        private static string _VisibilityEquityScripCode = "Visible";

        public static string VisibilityEquityScripCode
        {
            get { return _VisibilityEquityScripCode; }
            set { _VisibilityEquityScripCode = value; NotifyStaticPropertyChanged("VisibilityEquityScripCode"); }
        }
        private static string _VisibilityDebtScripCode = "Hidden";

        public static string VisibilityDebtScripCode
        {
            get { return _VisibilityDebtScripCode; }
            set { _VisibilityDebtScripCode = value; NotifyStaticPropertyChanged("VisibilityDebtScripCode"); }
        }
        private static List<Scrip_Code_ID> scripDebtDetails;
        public static List<Scrip_Code_ID> ScripDebtDetails
        {
            get { return scripDebtDetails; }
            set { scripDebtDetails = value; NotifyStaticPropertyChanged("ScripDebtDetails"); }
        }
        private List<Scrip_Code_ID> _ScripCodeDebtLst;

        public List<Scrip_Code_ID> ScripCodeDebtLst
        {
            get { return _ScripCodeDebtLst; }
            set { _ScripCodeDebtLst = value; NotifyPropertyChanged("ScripCodeDebtLst"); }
        }
        private List<Scrip_Code_ID> _ScripCodeLst;

        public List<Scrip_Code_ID> ScripCodeLst
        {
            get { return _ScripCodeLst; }
            set { _ScripCodeLst = value; NotifyPropertyChanged("ScripCodeLst"); }
        }
        private static List<string> typesOfCurrencySegmentLst;
        public static List<string> TypesOfCurrencySegmentLst
        {
            get { return typesOfCurrencySegmentLst; }
            set { typesOfCurrencySegmentLst = value; NotifyStaticPropertyChanged("TypesOfCurrencySegmentLst"); }
        }

        private static string selectedTypesOfCurrencySegment;
        public static string SelectedTypesOfCurrencySegment
        {
            get { return selectedTypesOfCurrencySegment; }
            set { selectedTypesOfCurrencySegment = value; NotifyStaticPropertyChanged("SelectedTypesOfCurrencySegment"); }
        }
        private static string _IndexVisibility;

        public static string IndexVisibility
        {
            get { return _IndexVisibility; }
            set { _IndexVisibility = value; NotifyStaticPropertyChanged("IndexVisibility"); }

        }
        private static string _VisibiltyCropAct;

        public static string VisibiltyCropAct
        {
            get { return _VisibiltyCropAct; }
            set
            {
                _VisibiltyCropAct = value;
                NotifyStaticPropertyChanged("VisibiltyCropAct");
            }
        }
        #endregion
        #region Memories

        
       

        


        #endregion

        public static Action<long?> OnSelectedScripCodeChange;
        /// <summary>
        /// Notifying Static Property Change event
        /// </summary>
        public static event EventHandler<PropertyChangedEventArgs> StaticPropertyChanged
                = delegate { };
        public static void NotifyStaticPropertyChanged(string propertyName)
        {
            StaticPropertyChanged(null, new PropertyChangedEventArgs(propertyName));
        }
        #region RelayCommands
        private RelayCommand<TextCompositionEventArgs> _EqFiveLachCalculator;

        public RelayCommand<TextCompositionEventArgs> EqFiveLachCalculator
        {
            get { return _EqFiveLachCalculator ?? (_EqFiveLachCalculator = new RelayCommand<TextCompositionEventArgs>(EqFiveLachCalculatorCheck)); }
        }



        private RelayCommand _Window_Loaded;

        public RelayCommand Window_Loaded
        {
            get { return _Window_Loaded ?? (_Window_Loaded = new RelayCommand((object e) => Window_Loaded_Event(e))); }

        }

        private RelayCommand _ShortCut_Enter;

        public RelayCommand ShortCut_Enter
        {
            get { return _ShortCut_Enter ?? (_ShortCut_Enter = new RelayCommand((object e) => EnterUsingUserControl())); }

        }
        private static string _VisibilityEquityScripName;

        public static string VisibilityEquityScripName
        {
            get { return _VisibilityEquityScripName; }
            set { _VisibilityEquityScripName = value; NotifyStaticPropertyChanged("VisibilityEquityScripName"); }
        }
        private static string _VisibilityDebtScripName;

        public static string VisibilityDebtScripName
        {
            get { return _VisibilityDebtScripName; }
            set { _VisibilityDebtScripName = value; NotifyStaticPropertyChanged("VisibilityDebtScripName"); }
        }
        private static long? _SelectedScripCodeDebt;
        public static long? SelectedScripCodeDebt
        {
            get { return _SelectedScripCodeDebt; }
            set
            {
                if (_SelectedScripCodeDebt != value)
                {
                    _SelectedScripCodeDebt = value;
                    NotifyStaticPropertyChanged("SelectedScripCodeDebt");
                    OnChangeOfScripCodeDebt();
                }
            }
        }

        private RelayCommand _DispalyHourlyStatistics;

        public RelayCommand DispalyHourlyStatistics
        {
            get { return _DispalyHourlyStatistics ?? (_DispalyHourlyStatistics = new RelayCommand((object e) => OpenHourlyStatisticsWindow(e))); }

        }

        private RelayCommand _MemberQuery;

        public RelayCommand MemberQuery
        {
            get { return _MemberQuery ?? (_MemberQuery = new RelayCommand((object e) => BestFiveVM.MemberQueryBF())); }

        }

        private RelayCommand _ShortCut_Escape;

        public RelayCommand ShortCut_Escape
        {
            get { return _ShortCut_Escape ?? (_ShortCut_Escape = new RelayCommand((object e) => EscapeUsingUserControl(e))); }

        }

        private RelayCommand _Info;

        public RelayCommand Info
        {
            get { return _Info ?? (_Info = new RelayCommand((object e) => InfoWindow())); }

        }


        private RelayCommand _ChartWindow;

        public RelayCommand ChartWindow
        {
            get { return _ChartWindow ?? (_ChartWindow = new RelayCommand((object e) => ChartWindowOpen())); }

        }


        private RelayCommand _SelectedScripCodeChanged;

        public RelayCommand SelectedScripCodeChanged
        {
            get { return _SelectedScripCodeChanged ?? (_SelectedScripCodeChanged = new RelayCommand((object e) => OnChangeOfScripCode())); }

        }

        private RelayCommand _SelectedScripIdChanged;

        public RelayCommand SelectedScripIdChanged
        {
            get { return _SelectedScripIdChanged ?? (_SelectedScripIdChanged = new RelayCommand((object e) => OnChangeOfScripCode())); }

        }

        #endregion

        #region Constructor
        public ScripInsertUC_VM()
        {

                FillEqutyDebtData();

			TimerCombo = new Timer();
            TimerCombo.Elapsed += TimerCombo_Elapsed;
            TimerCombo.Interval = 2000;
        }
        #endregion

        #region methods
        private void TimerCombo_Elapsed(object sender, ElapsedEventArgs e)
        {
            AppendedChar = string.Empty;
            TimeResetCounter = 0;
        }
        private void OnChangeOfScripCodeSelected(object e)
        {
            throw new NotImplementedException();
        }
        private void SelectScripCode(int scriptCode)
        {

            if (MasterSharedMemory.objMastertxtDictBaseBSE.ContainsKey(scriptCode) == true && MasterSharedMemory.objMastertxtDictBaseBSE[scriptCode].InstrumentType == 'E')
            {
                MasterSharedMemory.g_Segment = Enumerations.Order.ScripSegment.Equity.ToString();
                MasterSharedMemory.g_ScripId = MasterSharedMemory.objMastertxtDictBaseBSE[(long)scriptCode].ScripId;
                MasterSharedMemory.g_ScripName = MasterSharedMemory.objMastertxtDictBaseBSE[(long)scriptCode].ScripName;
                MasterSharedMemory.g_ScripCode = scriptCode;

                SelectedSegment = MasterSharedMemory.g_Segment;
                SelectedScripId = MasterSharedMemory.g_ScripId;
                SelectedName = MasterSharedMemory.g_ScripName;
                SelectedScripCode = MasterSharedMemory.g_ScripCode;

                //NotifyStaticPropertyChanged("SelectedScripId");
                //NotifyStaticPropertyChanged("SelectedScripCode");
            }

            else if (MasterSharedMemory.objMastertxtDictBaseBSE.ContainsKey(scriptCode) == true && (MasterSharedMemory.objMastertxtDictBaseBSE[scriptCode].InstrumentType == 'D' || MasterSharedMemory.objMastertxtDictBaseBSE[scriptCode].InstrumentType == 'C'))
            {
                MasterSharedMemory.g_Segment = Enumerations.Order.ScripSegment.Debt.ToString();
                MasterSharedMemory.g_ScripId = MasterSharedMemory.objMastertxtDictBaseBSE[(long)scriptCode].ScripId;
                MasterSharedMemory.g_ScripName = MasterSharedMemory.objMastertxtDictBaseBSE[(long)scriptCode].ScripName;
                MasterSharedMemory.g_ScripCode = scriptCode;

                SelectedSegment = MasterSharedMemory.g_Segment;
                SelectedScripId = MasterSharedMemory.g_ScripId;
                SelectedName = MasterSharedMemory.g_ScripName;
                SelectedScripCode = MasterSharedMemory.g_ScripCode;

                //NotifyStaticPropertyChanged("SelectedScripIdDebt");
                //NotifyStaticPropertyChanged("SelectedScripCodeDebt");
            }
            else if (MasterSharedMemory.objMasterDerivativeDictBaseBSE.ContainsKey(scriptCode) == true)
            {
                MasterSharedMemory.g_Segment = Enumerations.Order.ScripSegment.Derivative.ToString();
                MasterSharedMemory.g_ScripId = MasterSharedMemory.objMasterDerivativeDictBaseBSE[(long)scriptCode].ScripId;
                MasterSharedMemory.g_ScripName = MasterSharedMemory.objMasterDerivativeDictBaseBSE[(long)scriptCode].InstrumentName;
                MasterSharedMemory.g_ScripCode = scriptCode;

                MasterSharedMemory.g_Asset = MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Cast<DerivativeMasterBase>().Where(x => x.ContractTokenNum == scriptCode).Select(x => x.UnderlyingAsset).FirstOrDefault().ToString();
                string s = MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Cast<DerivativeMasterBase>().Where(x => x.ContractTokenNum == scriptCode).Select(x => x.ExpiryDate).FirstOrDefault().ToString();
                MasterSharedMemory.g_Expiry = DateTime.ParseExact(s, "dd-MMM-yyyy", CultureInfo.InstalledUICulture).ToString("dd-MM-yyyy");
                //MasterSharedMemory.g_Strike = MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Cast<DerivativeMasterBase>().Where(x => x.ContractTokenNum == scriptCode).Select(x => x.StrikePrice).FirstOrDefault().ToString();
                long strk = MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Cast<DerivativeMasterBase>().Where(x => x.ContractTokenNum == scriptCode).Select(x => x.StrikePrice).FirstOrDefault();
                MasterSharedMemory.g_Strike = Convert.ToDecimal(CommonFunctions.DisplayInDecimalFormatTouch(strk, 2)).ToString();

                string Instrument = MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Cast<DerivativeMasterBase>().Where(x => x.ContractTokenNum == scriptCode).Select(x => x.InstrumentType).FirstOrDefault().ToString();
                if (Instrument == "IF" || Instrument == "SF")
                    MasterSharedMemory.g_InstrType = Enumerations.Order.InstrumentType.Future.ToString();
                else
                {
                    int Strategy = MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Cast<DerivativeMasterBase>().Where(x => x.ContractTokenNum == scriptCode).Select(x => x.StrategyID).FirstOrDefault();
                    if (Strategy == 15)
                        MasterSharedMemory.g_InstrType = Enumerations.Order.InstrumentType.PairOption.ToString();
                    else
                    {
                        string optiontype = MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Cast<DerivativeMasterBase>().Where(x => x.ContractTokenNum == scriptCode).Select(x => x.OptionType).FirstOrDefault().ToString();
                        if (optiontype == "CE")
                            MasterSharedMemory.g_InstrType = Enumerations.Order.InstrumentType.Call.ToString();
                        if (optiontype == "PE")
                            MasterSharedMemory.g_InstrType = Enumerations.Order.InstrumentType.Put.ToString();
                    }
                }

                SelectedSegment = MasterSharedMemory.g_Segment;
                IntrTypeSelected = MasterSharedMemory.g_InstrType;
                AssetSelected = MasterSharedMemory.g_Asset;
                ExpDateSelected = MasterSharedMemory.g_Expiry;

                if (MasterSharedMemory.g_InstrType != Enumerations.Order.InstrumentType.Future.ToString())
                    StkPriceSelected = MasterSharedMemory.g_Strike;
                else
                    MasterSharedMemory.g_Strike = null;

                InstrNameSelected = MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Cast<DerivativeMasterBase>().Where(x => x.ContractTokenNum == scriptCode).Select(x => x.InstrumentName).FirstOrDefault().ToString();
            }
            else if (MasterSharedMemory.objMasterCurrencyDictBaseBSE.ContainsKey(scriptCode) == true)
            {
                MasterSharedMemory.g_Segment = Enumerations.Order.ScripSegment.Currency.ToString();
                MasterSharedMemory.g_ScripId = MasterSharedMemory.objMasterCurrencyDictBaseBSE[(long)scriptCode].ScripId;
                MasterSharedMemory.g_ScripName = MasterSharedMemory.objMasterCurrencyDictBaseBSE[(long)scriptCode].InstrumentName;
                MasterSharedMemory.g_ScripCode = scriptCode;

                MasterSharedMemory.g_Asset = MasterSharedMemory.objMasterCurrencyDictBaseBSE.Values.Cast<CurrencyMasterBase>().Where(x => x.ContractTokenNum == scriptCode).Select(x => x.UnderlyingAsset).FirstOrDefault().ToString();
                string s = MasterSharedMemory.objMasterCurrencyDictBaseBSE.Values.Cast<CurrencyMasterBase>().Where(x => x.ContractTokenNum == scriptCode).Select(x => x.ExpiryDate).FirstOrDefault().ToString();
                MasterSharedMemory.g_Expiry = DateTime.ParseExact(s, "dd-MMM-yyyy", CultureInfo.InstalledUICulture).ToString("dd-MM-yyyy");
                //MasterSharedMemory.g_Strike = MasterSharedMemory.objMasterCurrencyDictBaseBSE.Values.Cast<CurrencyMasterBase>().Where(x => x.ContractTokenNum == scriptCode).Select(x => x.StrikePrice).FirstOrDefault().ToString();
                long strk = MasterSharedMemory.objMasterCurrencyDictBaseBSE.Values.Cast<CurrencyMasterBase>().Where(x => x.ContractTokenNum == scriptCode).Select(x => x.StrikePrice).FirstOrDefault();
                MasterSharedMemory.g_Strike = Convert.ToDecimal(CommonFunctions.DisplayInDecimalFormatTouch(strk, 4)).ToString();

                string Instrument = MasterSharedMemory.objMasterCurrencyDictBaseBSE.Values.Cast<CurrencyMasterBase>().Where(x => x.ContractTokenNum == scriptCode).Select(x => x.InstrumentType).FirstOrDefault().ToString();
                if (Instrument == "FUTIRD" || Instrument == "FUTCUR" || Instrument == "FUTIRT")
                    MasterSharedMemory.g_InstrType = Enumerations.Order.InstrumentType.Future.ToString();
                else
                {
                    int Strategy = MasterSharedMemory.objMasterCurrencyDictBaseBSE.Values.Cast<CurrencyMasterBase>().Where(x => x.ContractTokenNum == scriptCode).Select(x => x.StrategyID).FirstOrDefault();
                    if (Strategy == 15)
                        MasterSharedMemory.g_InstrType = Enumerations.Order.InstrumentType.PairOption.ToString();
                    else if (Strategy == 30)
                        MasterSharedMemory.g_InstrType = Enumerations.Order.InstrumentType.Straddle.ToString();
                    else
                    {
                        string optiontype = MasterSharedMemory.objMasterCurrencyDictBaseBSE.Values.Cast<CurrencyMasterBase>().Where(x => x.ContractTokenNum == scriptCode).Select(x => x.OptionType).FirstOrDefault().ToString();
                        if (optiontype == "CE")
                            MasterSharedMemory.g_InstrType = Enumerations.Order.InstrumentType.Call.ToString();
                        if (optiontype == "PE")
                            MasterSharedMemory.g_InstrType = Enumerations.Order.InstrumentType.Put.ToString();
                    }
                }

                SelectedSegment = MasterSharedMemory.g_Segment;
                IntrTypeSelected = MasterSharedMemory.g_InstrType;
                AssetSelected = MasterSharedMemory.g_Asset;
                ExpDateSelected = MasterSharedMemory.g_Expiry;

                if (MasterSharedMemory.g_InstrType != Enumerations.Order.InstrumentType.Future.ToString())
                    StkPriceSelected = MasterSharedMemory.g_Strike;
                else
                    MasterSharedMemory.g_Strike = null;

                InstrNameSelected = MasterSharedMemory.objMasterCurrencyDictBaseBSE.Values.Cast<CurrencyMasterBase>().Where(x => x.ContractTokenNum == scriptCode).Select(x => x.InstrumentName).FirstOrDefault().ToString();
            }
            //On the Selection of NSE Exchange
            else if (MasterSharedMemory.objMastertxtDictBaseNSE.ContainsKey(scriptCode) == true && MasterSharedMemory.objMastertxtDictBaseNSE[scriptCode].InstrumentType == 'E')
            {
                SelectedSegment = Enumerations.Order.ScripSegment.Equity.ToString();
                SelectedScripCode = scriptCode;
            }
            else
            {
                // SelectedScripCode = scriptCode;
            }

            MasterSharedMemory.g_Segment = null;
            MasterSharedMemory.g_ScripId = null;
            MasterSharedMemory.g_ScripName = null;
            MasterSharedMemory.g_ScripCode = 0;
            MasterSharedMemory.g_InstrType = null;
            MasterSharedMemory.g_Asset = null;
            MasterSharedMemory.g_Expiry = null;
            MasterSharedMemory.g_Strike = null;
        }
        
        
        private static void OnChangeOfScripCodeDebt()
        {
            if (flag)
            {
                changeScripId = false;
                if (SelectedScripCodeDebt != null)
                {//TODO TBD2017
                    SelectedNameDebt = MasterSharedMemory.objMastertxtDictBaseBSE[(long)SelectedScripCodeDebt].ScripName;
                    SelectedScripIdDebt = MasterSharedMemory.objMastertxtDictBaseBSE[(long)SelectedScripCodeDebt].ScripId;
                }
                else
                {
                    SelectedNameDebt = string.Empty;
                    SelectedScripIdDebt = null;
                }
            }
            flag = true;
        }
        private bool CheckIfNumber(string text)
        {
            int i = 0;
            bool result = int.TryParse(text, out i);
            return result;
        }
        
        private void ControlleEnabllity()
        {
            corpEnability = "False";
        }

        private void FillListData()
        {
            //Debt
            ScripDebtDetails = new List<Scrip_Code_ID>();
            //Dictionary<long, ScipMaster> objDebtDetailsDic = new Dictionary<long, ScipMaster>();
            ScripCodeDebtLst = new List<Scrip_Code_ID>();
            // ScripEquityDetails = MasterSharedMemory.ScripCommonEquityDetails.OrderBy(x => x.ScripId).ToList();
            //objDebtDetailsDic = MasterSharedMemory.objMastertxtDic.Where(x => x.Value.SegmentFlag == 'D' || x.Value.SegmentFlag == 'C').OrderBy(y => y.Value.ScripId).ToDictionary(x => x.Key, x => x.Value);
            // objDebtDetailsDic = MasterSharedMemory.objMastertxtDic.Where(x => x.Value.SegmentFlag == 'C').OrderBy(y => y.Value.ScripId).ToDictionary(x => x.Key, x => x.Value);

            //TODO TBD2017
            foreach (var item in MasterSharedMemory.objMastertxtDictBaseBSE.Where(x => x.Value.InstrumentType == 'D' || x.Value.InstrumentType == 'C').OrderBy(y => y.Value.ScripId).ToDictionary(x => x.Key, x => x.Value))
            {
                Scrip_Code_ID scrip = new Scrip_Code_ID();
                scrip.ScripCode = item.Value.ScripCode;
                scrip.ScripId = item.Value.ScripId;
                scrip.ScripName = item.Value.ScripName;

                ScripDebtDetails.Add(scrip);
                ScripCodeDebtLst.Add(scrip);
            }

            ScripDebtDetails.OrderBy(x => x.ScripId);
            if (ScripDebtDetails != null && ScripDebtDetails.Count != 0)
            {
                SelectedScripIdDebt = ScripDebtDetails[0].ScripId;
                SelectedScripCodeDebt = ScripDebtDetails[0].ScripCode;
                SelectedNameDebt = ScripDebtDetails[0].ScripName;

            }

            //Equity
            ScripEquityDetails = new List<Scrip_Code_ID>();
            // Dictionary<long, ScipMaster> objEquityDetailsDic = new Dictionary<long, ScipMaster>();
            ScripCodeLst = new List<Scrip_Code_ID>();
            //ScripEquityDetails.Clear();
            //ScripCodeLst.Clear();
            // ScripEquityDetails = MasterSharedMemory.ScripCommonEquityDetails.OrderBy(x => x.ScripId).ToList();
            // objEquityDetailsDic = MasterSharedMemory.objMastertxtDic.Where(x => x.Value.SegmentFlag == 'E').OrderBy(y => y.Value.ScripId).ToDictionary(x => x.Key, x => x.Value);
            //NotifyPropertyChanged("objEquityDetailsDic");

            //TODO TBD2017
            foreach (var item in MasterSharedMemory.objMastertxtDictBaseBSE.Where(x => x.Value.InstrumentType == 'E').OrderBy(y => y.Value.ScripId).ToDictionary(x => x.Key, x => x.Value))
            {
                Scrip_Code_ID scrip = new Scrip_Code_ID();
                scrip.ScripCode = item.Value.ScripCode;
                scrip.ScripId = item.Value.ScripId;
                scrip.ScripName = item.Value.ScripName;

                ScripEquityDetails.Add(scrip);
                ScripCodeLst.Add(scrip);
            }

            scripEquityDetails.OrderBy(x => x.ScripId);
            if (ScripEquityDetails != null && scripEquityDetails.Count != 0)
            {
                SelectedScripId = ScripEquityDetails[0].ScripId;
                SelectedScripCode = ScripEquityDetails[0].ScripCode;
                SelectedName = ScripEquityDetails[0].ScripName;
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
                    if (SelectedSegment.ToUpper() == Enumerations.Order.ScripSegment.Equity.ToString().ToUpper() && !string.IsNullOrEmpty(AppendedChar) && ScripEquityDetails != null)
                    {
                        if (AppendedChar.Length < 100000)
                        {

                            num = Convert.ToInt64(AppendedChar.Trim()) + 500000;
                            e.Handled = true;
                           
                                int index = ScripEquityDetails.IndexOf(ScripEquityDetails.Where(x=>x.ScripCode==num).FirstOrDefault());
                                if(index != -1)
                                SelectedScripCode = ScripEquityDetails[index].ScripCode;
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
        }
        public void PopulatingSegDropDowns()
        {
            SegmentLst = new List<string>();
            SegmentLst.Add(Enumerations.Order.ScripSegment.Equity.ToString());
            SegmentLst.Add(Enumerations.Order.ScripSegment.Derivative.ToString());
            SegmentLst.Add(Enumerations.Order.ScripSegment.Debt.ToString());
            SegmentLst.Add(Enumerations.Order.ScripSegment.Currency.ToString());
            SelectedSegment = Enumerations.Order.ScripSegment.Equity.ToString();
        }



        private void FillEqutyDebtData()
        {
            SegmentLst = new List<string>();
            SegmentLst.Add(Enumerations.Order.ScripSegment.Equity.ToString());
            SegmentLst.Add(Enumerations.Order.ScripSegment.Derivative.ToString());
            SegmentLst.Add(Enumerations.Order.ScripSegment.Debt.ToString());
            SegmentLst.Add(Enumerations.Order.ScripSegment.Currency.ToString());

            ScripEquityDetails = new List<Scrip_Code_ID>();
            ScripDebtDetails = new List<Scrip_Code_ID>();

            foreach (var item in MasterSharedMemory.objMastertxtDictBaseBSE.OrderBy(x => x.Value.ScripId).ToDictionary(x => x.Key, x => x.Value))
            {
                Scrip_Code_ID scrip = new Scrip_Code_ID();
                scrip.ScripCode = item.Value.ScripCode;
                scrip.ScripId = item.Value.ScripId;
                scrip.ScripName = item.Value.ScripName;

                if (item.Value.InstrumentType == 'D' || item.Value.InstrumentType == 'C')
                    ScripDebtDetails.Add(scrip);
                else
                    ScripEquityDetails.Add(scrip);
            }

            scripEquityDetails.OrderBy(x => x.ScripId);
            ScripDebtDetails.OrderBy(x => x.ScripId);

            if (ScripEquityDetails != null && scripEquityDetails.Count != 0)
            {
                SelectedSegment = Enumerations.Order.ScripSegment.Equity.ToString();
                SelectedScripId = ScripEquityDetails[0].ScripId;
                SelectedScripCode = ScripEquityDetails[0].ScripCode;
                SelectedName = ScripEquityDetails[0].ScripName;
            }
            
            
            //if (ScripDebtDetails != null && ScripDebtDetails.Count != 0)
            //{
            //    SelectedScripIdDebt = ScripDebtDetails[0].ScripId;
            //    SelectedScripCodeDebt = ScripDebtDetails[0].ScripCode;
            //    SelectedNameDebt = ScripDebtDetails[0].ScripName;
            //}
        }

        private static void HideShowControls()
        {
            try
            {
                if (SelectedSegment == Enumerations.Order.ScripSegment.Equity.ToString() || SelectedSegment == Enumerations.Order.ScripSegment.Debt.ToString())
                {
                    EquityVis = "Visible";
                    DerivativeVis = "Hidden";
                    FutureVis = "Hidden";
                    CallVis = "Hidden";
                }
                else if (SelectedSegment == Enumerations.Order.ScripSegment.Derivative.ToString() || SelectedSegment == Enumerations.Order.ScripSegment.Currency.ToString())
                {
                    EquityVis = "Hidden";
                    DerivativeVis = "Visible";
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static void PopulatingDropDowns()
        {
            try
            {
                if (SelectedSegment == Enumerations.Order.ScripSegment.Equity.ToString())
                {
                    SelectedScripId = ScripEquityDetails[0].ScripId;
                    //SelectedScripCode = ScripEquityDetails[0].ScripCode;

                    IndexVisibility = "Hidden";
                    VisibiltyCropAct = "Visible";

                    ddlEquityVisible = "Visible";
                    ddlDebtVisible = "Hidden";

                    VisibilityEquityScripCode = "Visible";
                    VisibilityDebtScripCode = "Hidden";
                    VisibilityEquityScripName = "Visible";
                    VisibilityDebtScripName = "Hidden";
                }
                else if (SelectedSegment == Enumerations.Order.ScripSegment.Debt.ToString())
                {
                    SelectedScripId = ScripDebtDetails[0].ScripId;
                    //SelectedScripCode = ScripDebtDetails[0].ScripCode;

                    IndexVisibility = "Hidden";
                    VisibiltyCropAct = "Visible";

                    ddlEquityVisible = "Hidden";
                    ddlDebtVisible = "Visible";
                    
                    VisibilityEquityScripCode = "Hidden";
                    VisibilityDebtScripCode = "Visible";
                    VisibilityEquityScripName = "Hidden";
                    VisibilityDebtScripName = "Visible";
                }
                else if (SelectedSegment == Enumerations.Order.ScripSegment.Derivative.ToString())
                {
                    if(IntrTypeLst == null)
                        IntrTypeLst = new ObservableCollection<String>();
                    IntrTypeLst.Clear();

                    IntrTypeLst.Add(Enumerations.Order.InstrumentType.Call.ToString());
                    IntrTypeLst.Add(Enumerations.Order.InstrumentType.Put.ToString());
                    IntrTypeLst.Add(Enumerations.Order.InstrumentType.Future.ToString());
                    IntrTypeLst.Add(Enumerations.Order.InstrumentType.PairOption.ToString());

                    IntrTypeSelected = null;
                    IntrTypeSelected = MasterSharedMemory.g_InstrType != null ? MasterSharedMemory.g_InstrType : IntrTypeLst[0];
                }
                else if (SelectedSegment == Enumerations.Order.ScripSegment.Currency.ToString())
                {
                    if (IntrTypeLst == null)
                        IntrTypeLst = new ObservableCollection<String>();
                    IntrTypeLst.Clear();

                    IntrTypeLst.Add(Enumerations.Order.InstrumentType.Call.ToString());
                    IntrTypeLst.Add(Enumerations.Order.InstrumentType.Put.ToString());
                    IntrTypeLst.Add(Enumerations.Order.InstrumentType.Future.ToString());
                    IntrTypeLst.Add(Enumerations.Order.InstrumentType.PairOption.ToString());
                    IntrTypeLst.Add(Enumerations.Order.InstrumentType.Straddle.ToString());

                    IntrTypeSelected = null;
                    IntrTypeSelected = MasterSharedMemory.g_InstrType != null ? MasterSharedMemory.g_InstrType : IntrTypeLst[0];
                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
            }
        }

        private static void OnChangeOfScripId()
        {
            return;
            if (changeScripId)
            {
                //Scrip_Code_ID objScrip_Code_ID = ScripEquityDetails.Where(x => x.ScripId == SelectedScripId).FirstOrDefault();
                flag = false;
                if (SelectedScripId != null)
                {
                    SelectedScripCode = CommonFunctions.GetScripCode(SelectedScripId, Enumerations.Exchange.BSE, Enumerations.Segment.Equity);//BSE Equity
                    SelectedName = MasterSharedMemory.objMastertxtDictBaseBSE[(long)SelectedScripCode].ScripName;//TODO TBD2017
                }
                else
                {
                    SelectedName = string.Empty;
                    SelectedScripCode = null;
                }
                OnSelectedScripCodeChange?.Invoke(SelectedScripCode);
                NormalOrderEntryVM.GetInstance.UpdateScripDetails(SelectedScripCode);

            }
            changeScripId = true;

            if (SelectedScripCode != null && OnScripIDOrCodeChange != null)
                OnScripIDOrCodeChange((long)SelectedScripCode);
            //else if (OnScripIDOrCodeChange == null)
            //    OnScripIDOrCodeChange(0);
        }
        private static void OnChange_ScripId()
        {
            if (SelectedScripId != null)
            {
                //SelectedScripCode = CommonFunctions.GetScripCode(SelectedScripId, Enumerations.Exchange.BSE, Enumerations.Segment.Equity);
                //SelectedName = MasterSharedMemory.objMastertxtDictBaseBSE[(long)SelectedScripCode].ScripName;

                if (SelectedSegment == "Equity" || SelectedSegment == "Debt")
                {
                    SelectedScripCode = CommonFunctions.GetScripCode(SelectedScripId, Enumerations.Exchange.BSE, Enumerations.Segment.Equity);
                    SelectedName = MasterSharedMemory.objMastertxtDictBaseBSE[(long)SelectedScripCode].ScripName;
                }
                else if (SelectedSegment == "Derivative")
                {
                    SelectedScripCode = CommonFunctions.GetScripCode(SelectedScripId, Enumerations.Exchange.BSE, Enumerations.Segment.Derivative);
                    SelectedName = MasterSharedMemory.objMasterDerivativeDictBaseBSE[(long)SelectedScripCode].InstrumentName;
                }
                else if (SelectedSegment == "Currency")
                {
                    SelectedScripCode = CommonFunctions.GetScripCode(SelectedScripId, Enumerations.Exchange.BSE, Enumerations.Segment.Currency);
                    SelectedName = MasterSharedMemory.objMasterCurrencyDictBaseBSE[(long)SelectedScripCode].InstrumentName;
                }
            }
            //NormalOrderEntryVM.GetInstance.UpdateScripDetails(SelectedScripCode);
            //if (SelectedScripCode != null && OnScripIDOrCodeChange != null)
            //    OnScripIDOrCodeChange((long)SelectedScripCode);
        }

        private static void OnChangeOfScripCode()
        {
            return;
            if (flag)
            {
                //Scrip_Code_ID objScrip_Code_ID = ScripEquityDetails.Where(x => x.ScripCode == SelectedScripCode).FirstOrDefault();
                changeScripId = false;
                if (SelectedScripCode != null)
                {                           //TODO TBD2017
                    if ((SelectedSegment == "Equity" || SelectedSegment == "Debt") && MasterSharedMemory.objMastertxtDictBaseBSE.ContainsKey((long)SelectedScripCode))
                    {
                        if (MasterSharedMemory.objMastertxtDictBaseBSE.ContainsKey((long)SelectedScripCode))
                        {
                            SelectedName = MasterSharedMemory.objMastertxtDictBaseBSE[(long)SelectedScripCode].ScripName;
                            SelectedScripId = MasterSharedMemory.objMastertxtDictBaseBSE[(long)SelectedScripCode].ScripId;
                        }
                    }
                    if ((SelectedSegment == "Derivative") && MasterSharedMemory.objMasterDerivativeDictBaseBSE.ContainsKey((long)SelectedScripCode))
                    {
                        if (MasterSharedMemory.objMasterDerivativeDictBaseBSE.ContainsKey((long)SelectedScripCode))
                        {
                            SelectedName = MasterSharedMemory.objMasterDerivativeDictBaseBSE[(long)SelectedScripCode].InstrumentName;
                            SelectedScripId = MasterSharedMemory.objMasterDerivativeDictBaseBSE[(long)SelectedScripCode].ScripId;
                        }
                    }
                    if ((SelectedSegment == "Currency") && MasterSharedMemory.objMasterCurrencyDictBaseBSE.ContainsKey((long)SelectedScripCode))
                    {
                        if (MasterSharedMemory.objMasterCurrencyDictBaseBSE.ContainsKey((long)SelectedScripCode))
                        {
                            SelectedName = MasterSharedMemory.objMasterCurrencyDictBaseBSE[(long)SelectedScripCode].InstrumentName;
                            SelectedScripId = MasterSharedMemory.objMasterCurrencyDictBaseBSE[(long)SelectedScripCode].ScripId;
                        }
                    }
                }
                else
                {
                    SelectedName = string.Empty;
                    SelectedScripId = null;
                }

                OnSelectedScripCodeChange?.Invoke(SelectedScripCode);
                NormalOrderEntryVM.GetInstance.UpdateScripDetails(SelectedScripCode);

            }
            flag = true;
            //Messenger.Default.Send(SelectedScripCode);
        }
        private static void OnChange_ScripCode()
        {
            if (SelectedScripCode != null)
            {
                if ((SelectedSegment == "Equity" || SelectedSegment == "Debt") && MasterSharedMemory.objMastertxtDictBaseBSE.ContainsKey((long)SelectedScripCode))
                {
                    SelectedScripId = MasterSharedMemory.objMastertxtDictBaseBSE[(long)SelectedScripCode].ScripId;
                    SelectedName = MasterSharedMemory.objMastertxtDictBaseBSE[(long)SelectedScripCode].ScripName;
                }
                else if ((SelectedSegment == "Derivative") && MasterSharedMemory.objMasterDerivativeDictBaseBSE.ContainsKey((long)SelectedScripCode))
                {
                    SelectedScripId = MasterSharedMemory.objMasterDerivativeDictBaseBSE[(long)SelectedScripCode].ScripId;
                    SelectedName = MasterSharedMemory.objMasterDerivativeDictBaseBSE[(long)SelectedScripCode].InstrumentName;
                }
                else if ((SelectedSegment == "Currency") && MasterSharedMemory.objMasterCurrencyDictBaseBSE.ContainsKey((long)SelectedScripCode))
                {
                    SelectedScripId = MasterSharedMemory.objMasterCurrencyDictBaseBSE[(long)SelectedScripCode].ScripId;
                    SelectedName = MasterSharedMemory.objMasterCurrencyDictBaseBSE[(long)SelectedScripCode].InstrumentName;
                }
            }
            //NormalOrderEntryVM.GetInstance.UpdateScripDetails(SelectedScripCode);
            if (SelectedScripCode != null && OnScripIDOrCodeChange != null)
                OnScripIDOrCodeChange((long)SelectedScripCode);

            //OnSelectedScripCodeChange?.Invoke(SelectedScripCode);
            //NormalOrderEntryVM.GetInstance.UpdateScripDetails(SelectedScripCode);

            //MarketWatchVM.SelectedItem = SelectedScripCode;
            //NormalOrderEntryVM.GetInstance.PopulateOrderEntryWindow(string strScripCode, string status);
            //if (MemoryManager.InvokeWindowOnScripCodeSelection != null)
            //    MemoryManager.InvokeWindowOnScripCodeSelection(Convert.ToString(SelectedScripCode), UtilityOrderDetails.GETInstance.CurrentOrderEntry.ToString());
        }

        private static void PopulatingUnderlyingAsset()
        {
            return;
            try
            {
                ////for BSE Exchange
                //if (Selected_EXCH == Enumerations.Order.Exchanges.BSE.ToString())
                //{
                //for derivative market DerivativeMasterBase> 
                if (SelectedSegment == Enumerations.Order.ScripSegment.Derivative.ToString())
                {
                    if (IntrTypeSelected == Enumerations.Order.InstrumentType.PairOption.ToString())
                    {

                        //AssetList = new ObservableCollection<string>();
                        //foreach (var item in MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Cast<DerivativeMasterBase>().Where(x => x.InstrumentType == "IO" || x.InstrumentType == "SO").Select(x => x.UnderlyingAsset).Distinct())
                        // {
                        // AssetList.Add(item);
                        // }
                        // if (AssetList.Count() > 0 && AssetList != null)
                        // {
                        // AssetSelected = AssetList[0];
                        // }

                    }
                    else if (IntrTypeSelected == Enumerations.Order.InstrumentType.Future.ToString())
                    {
                        FutureVis = "Visible";
                        CallVis = "Hidden";
                        AssetList = new ObservableCollection<string>();
                        foreach (var item in MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Cast<DerivativeMasterBase>().Where(x => x.InstrumentType == "IF" || x.InstrumentType == "SF").Select(x => x.UnderlyingAsset).Distinct())
                        {
                            AssetList.Add(item);
                        }
                        if (AssetList.Count() > 0 && AssetList != null)
                        {
                            AssetSelected = null;
                            AssetSelected = MasterSharedMemory.g_Asset != null ? MasterSharedMemory.g_Asset : AssetList[0];
                        }
                    }
                    else if (IntrTypeSelected == Enumerations.Order.InstrumentType.Call.ToString())
                    {
                        FutureVis = "Hidden";
                        CallVis = "Visible";
                        AssetList = new ObservableCollection<string>();

                        foreach (var item in MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Cast<DerivativeMasterBase>().Where(x => x.OptionType == "CE").Select(x => x.UnderlyingAsset).Distinct())
                        {
                            AssetList.Add(item);
                        }
                        if (AssetList.Count() > 0 && AssetList != null)
                        {
                            AssetSelected = null;
                            AssetSelected = MasterSharedMemory.g_Asset != null ? MasterSharedMemory.g_Asset : AssetList[0];
                        }
                    }
                    else if (IntrTypeSelected == Enumerations.Order.InstrumentType.Put.ToString())
                    {
                        FutureVis = "Hidden";
                        CallVis = "Visible";
                        AssetList = new ObservableCollection<string>();

                        foreach (var item in MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Cast<DerivativeMasterBase>().Where(x => x.OptionType == "PE").Select(x => x.UnderlyingAsset).Distinct())
                        {
                            AssetList.Add(item);
                        }
                        if (AssetList.Count() > 0 && AssetList != null)
                        {
                            AssetSelected = null;
                            AssetSelected = MasterSharedMemory.g_Asset != null ? MasterSharedMemory.g_Asset : AssetList[0];
                        }
                    }
                }
                if (SelectedSegment == Enumerations.Order.ScripSegment.Currency.ToString())
                {
                    if (IntrTypeSelected == Enumerations.Order.InstrumentType.PairOption.ToString())
                    {

                        //AssetList = new ObservableCollection<string>();
                        //foreach (var item in MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Cast<DerivativeMasterBase>().Where(x => x.InstrumentType == "IO" || x.InstrumentType == "SO").Select(x => x.UnderlyingAsset).Distinct())
                        // {
                        // AssetList.Add(item);
                        // }
                        // if (AssetList.Count() > 0 && AssetList != null)
                        // {
                        // AssetSelected = AssetList[0]; 
                        // }

                    }
                    else if (IntrTypeSelected == Enumerations.Order.InstrumentType.Future.ToString())
                    {
                        FutureVis = "Visible";
                        CallVis = "Hidden";
                        AssetList = new ObservableCollection<string>();
                        foreach (var item in MasterSharedMemory.objMasterCurrencyDictBaseBSE.Values.Cast<CurrencyMasterBase>().Where(x => x.InstrumentType == "FUTIRD" || x.InstrumentType == "FUTCUR" || x.InstrumentType == "FUTIRT").Select(x => x.UnderlyingAsset).Distinct())
                        {
                            AssetList.Add(item);
                        }
                        if (AssetList.Count() > 0 && AssetList != null)
                        {
                            AssetSelected = null;
                            AssetSelected = MasterSharedMemory.g_Asset != null ? MasterSharedMemory.g_Asset : AssetList[0];
                        }
                    }
                    else if (IntrTypeSelected == Enumerations.Order.InstrumentType.Call.ToString())
                    {
                        FutureVis = "Hidden";
                        CallVis = "Visible";
                        AssetList = new ObservableCollection<string>();

                        foreach (var item in MasterSharedMemory.objMasterCurrencyDictBaseBSE.Values.Cast<CurrencyMasterBase>().Where(x => x.OptionType == "CE").Select(x => x.UnderlyingAsset).Distinct())
                        {
                            AssetList.Add(item);
                        }
                        if (AssetList.Count() > 0 && AssetList != null)
                        {
                            AssetSelected = null;
                            AssetSelected = MasterSharedMemory.g_Asset != null ? MasterSharedMemory.g_Asset : AssetList[0];
                        }
                    }
                    else if (IntrTypeSelected == Enumerations.Order.InstrumentType.Put.ToString())
                    {
                        FutureVis = "Hidden";
                        CallVis = "Visible";
                        AssetList = new ObservableCollection<string>();

                        foreach (var item in MasterSharedMemory.objMasterCurrencyDictBaseBSE.Values.Cast<CurrencyMasterBase>().Where(x => x.OptionType == "PE").Select(x => x.UnderlyingAsset).Distinct())
                        {
                            AssetList.Add(item);
                        }
                        if (AssetList.Count() > 0 && AssetList != null)
                        {
                            AssetSelected = null;
                            AssetSelected = MasterSharedMemory.g_Asset != null ? MasterSharedMemory.g_Asset : AssetList[0];
                        }
                    }
                }

                //}
            }
            catch (Exception ex)
            {

                ExceptionUtility.LogError(ex);
            }
        }
        private static void OnChange_IntrType()
        {
            try
            {
                if (SelectedSegment == Enumerations.Order.ScripSegment.Derivative.ToString())
                {
                    if (AssetList == null)
                        AssetList = new ObservableCollection<string>();
                    AssetList.Clear();

                    if (IntrTypeSelected == Enumerations.Order.InstrumentType.Call.ToString())
                    {
                        FutureVis = "Hidden";
                        CallVis = "Visible";

                        foreach (var item in MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Cast<DerivativeMasterBase>().Where(x => x.OptionType == "CE" && x.StrategyID != 15).Select(x => x.UnderlyingAsset).Distinct())
                            AssetList.Add(item);
                    }
                    else if (IntrTypeSelected == Enumerations.Order.InstrumentType.Put.ToString())
                    {
                        FutureVis = "Hidden";
                        CallVis = "Visible";

                        foreach (var item in MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Cast<DerivativeMasterBase>().Where(x => x.OptionType == "PE" && x.StrategyID != 15).Select(x => x.UnderlyingAsset).Distinct())
                            AssetList.Add(item);
                    }
                    else if (IntrTypeSelected == Enumerations.Order.InstrumentType.Future.ToString())
                    {
                        FutureVis = "Visible";
                        CallVis = "Hidden";

                        foreach (var item in MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Cast<DerivativeMasterBase>().Where(x => x.InstrumentType == "IF" || x.InstrumentType == "SF").Select(x => x.UnderlyingAsset).Distinct())
                            AssetList.Add(item);
                    }
                    else if (IntrTypeSelected == Enumerations.Order.InstrumentType.PairOption.ToString())
                    {
                        FutureVis = "Hidden";
                        CallVis = "Visible";

                        foreach (var item in MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Cast<DerivativeMasterBase>().Where(x => x.StrategyID == 15).Select(x => x.UnderlyingAsset).Distinct())
                            AssetList.Add(item);

                    }

                    if (AssetList.Count() > 0 && AssetList != null)
                    {
                        AssetSelected = null;
                        AssetSelected = MasterSharedMemory.g_Asset != null ? MasterSharedMemory.g_Asset : AssetList[0];
                    }
                }
                else if (SelectedSegment == Enumerations.Order.ScripSegment.Currency.ToString())
                {
                    if (AssetList == null)
                        AssetList = new ObservableCollection<string>();
                    AssetList.Clear();

                    if (IntrTypeSelected == Enumerations.Order.InstrumentType.Call.ToString())
                    {
                        FutureVis = "Hidden";
                        CallVis = "Visible";

                        foreach (var item in MasterSharedMemory.objMasterCurrencyDictBaseBSE.Values.Cast<CurrencyMasterBase>().Where(x => x.OptionType == "CE" && (x.StrategyID != 15 || x.StrategyID != 30)).Select(x => x.UnderlyingAsset).Distinct())
                            AssetList.Add(item);
                    }
                    else if (IntrTypeSelected == Enumerations.Order.InstrumentType.Put.ToString())
                    {
                        FutureVis = "Hidden";
                        CallVis = "Visible";

                        foreach (var item in MasterSharedMemory.objMasterCurrencyDictBaseBSE.Values.Cast<CurrencyMasterBase>().Where(x => x.OptionType == "PE" && (x.StrategyID != 15 || x.StrategyID != 30)).Select(x => x.UnderlyingAsset).Distinct())
                            AssetList.Add(item);
                    }
                    else if (IntrTypeSelected == Enumerations.Order.InstrumentType.Future.ToString())
                    {
                        FutureVis = "Visible";
                        CallVis = "Hidden";

                        foreach (var item in MasterSharedMemory.objMasterCurrencyDictBaseBSE.Values.Cast<CurrencyMasterBase>().Where(x => x.InstrumentType == "FUTIRD" || x.InstrumentType == "FUTCUR" || x.InstrumentType == "FUTIRT").Select(x => x.UnderlyingAsset).Distinct())
                            AssetList.Add(item);
                    }
                    else if (IntrTypeSelected == Enumerations.Order.InstrumentType.PairOption.ToString())
                    {
                        FutureVis = "Hidden";
                        CallVis = "Visible";

                        foreach (var item in MasterSharedMemory.objMasterCurrencyDictBaseBSE.Values.Cast<CurrencyMasterBase>().Where(x => x.StrategyID == 15).Select(x => x.UnderlyingAsset).Distinct())
                            AssetList.Add(item);

                    }
                    else if (IntrTypeSelected == Enumerations.Order.InstrumentType.Straddle.ToString())
                    {
                        FutureVis = "Hidden";
                        CallVis = "Visible";

                        foreach (var item in MasterSharedMemory.objMasterCurrencyDictBaseBSE.Values.Cast<CurrencyMasterBase>().Where(x => x.StrategyID == 28).Select(x => x.UnderlyingAsset).Distinct())
                            AssetList.Add(item);
                    }

                    if (AssetList.Count() > 0 && AssetList != null)
                    {
                        AssetSelected = null;
                        AssetSelected = MasterSharedMemory.g_Asset != null ? MasterSharedMemory.g_Asset : AssetList[0];
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
            }
        }

        private static void OnChangeOfAsset()
        {
            return;
            try
            {

                //if (Selected_EXCH == Enumerations.Order.Exchanges.BSE.ToString())
                //{ //BSE DERIVATIVE
                if (SelectedSegment == Enumerations.Order.ScripSegment.Derivative.ToString())
                {
                    if (AssetList.Count() > 0 && AssetList != null)
                    {
                        ExpDateLst = new ObservableCollection<string>();
                        if (IntrTypeSelected == Enumerations.Order.InstrumentType.Future.ToString())
                        {
                            //x => DateTime.ParseExact(x.ExpiryDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture).ToString("yy-MMM-dd")
                            foreach (var item in MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Cast<DerivativeMasterBase>().OrderByDescending(y => y.ExpiryDate).Where(x => x.UnderlyingAsset == AssetSelected && (x.InstrumentType == "SF" || x.InstrumentType == "IF")).Select(x => DateTime.ParseExact(x.ExpiryDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd")).Distinct())
                            {
                                ExpDateLst.Add(item);
                            }
                        }
                        else if (IntrTypeSelected == Enumerations.Order.InstrumentType.Call.ToString())
                        {
                            foreach (var item in MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Cast<DerivativeMasterBase>().OrderByDescending(y => y.ExpiryDate).Where(x => x.UnderlyingAsset == AssetSelected && x.OptionType == "CE").Select(x => DateTime.ParseExact(x.ExpiryDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd")).Distinct())
                            {
                                ExpDateLst.Add(item);
                            }
                        }
                        else if (IntrTypeSelected == Enumerations.Order.InstrumentType.Put.ToString())
                        {

                            foreach (var item in MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Cast<DerivativeMasterBase>().OrderByDescending(y => y.ExpiryDate).Where(x => x.UnderlyingAsset == AssetSelected && x.OptionType == "PE").Select(x => DateTime.ParseExact(x.ExpiryDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd")).Distinct())
                            {
                                ExpDateLst.Add(item);
                            }
                        }
                        ExpDateLst = new ObservableCollection<string>(ExpDateLst.OrderBy(p => p));
                        var sortedDatelst = DisplayDateInComboBox(ExpDateLst);
                        ExpDateLst.Clear();
                        ExpDateLst = sortedDatelst;
                        //ExpDateLst = new ObservableCollection<string>(ExpDateLst.OrderBy(p => p));
                        //var sortedDatelst = DisplayDateInComboBoxddMMMyyyy(ExpDateLst);
                        //ExpDateLst.Clear();
                        //ExpDateLst = sortedDatelst;

                        if (ExpDateLst.Count > 0 && ExpDateLst != null)
                        {
                            ExpDateSelected = null;
                            ExpDateSelected = MasterSharedMemory.g_Expiry != null ? MasterSharedMemory.g_Expiry : ExpDateLst[0];//.ToUpper();
                        }
                        else if (ExpDateSelected == null)
                        {
                            InstrNameSelected = null;
                            StkPriceSelected = string.Empty;
                        }
                    }
                }
                //BSE CURRENCY 
                if (SelectedSegment == Enumerations.Order.ScripSegment.Currency.ToString())
                {

                    if (AssetList.Count() > 0 && AssetList != null)
                    {
                        ExpDateLst = new ObservableCollection<string>();
                        if (IntrTypeSelected == Enumerations.Order.InstrumentType.Future.ToString())
                        {
                            foreach (var item in MasterSharedMemory.objMasterCurrencyDictBaseBSE.Values.Cast<CurrencyMasterBase>().OrderByDescending(y => y.ExpiryDate).Where(x => x.UnderlyingAsset == AssetSelected && (x.InstrumentType == "FUTIRD" || x.InstrumentType == "FUTCUR" || x.InstrumentType == "FUTIRT")).Select(x => DateTime.ParseExact(x.ExpiryDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd")).Distinct())
                            {
                                ExpDateLst.Add(item);
                            }
                        }
                        else if (IntrTypeSelected == Enumerations.Order.InstrumentType.Call.ToString())
                        {
                            foreach (var item in MasterSharedMemory.objMasterCurrencyDictBaseBSE.Values.Cast<CurrencyMasterBase>().OrderByDescending(y => y.ExpiryDate).Where(x => x.UnderlyingAsset == AssetSelected && x.OptionType == "CE").Select(x => DateTime.ParseExact(x.ExpiryDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd")).Distinct())
                            {
                                ExpDateLst.Add(item);
                            }
                        }
                        else if (IntrTypeSelected == Enumerations.Order.InstrumentType.Put.ToString())
                        {
                            foreach (var item in MasterSharedMemory.objMasterCurrencyDictBaseBSE.Values.Cast<CurrencyMasterBase>().OrderByDescending(y => y.ExpiryDate).Where(x => x.UnderlyingAsset == AssetSelected && x.OptionType == "PE").Select(x => DateTime.ParseExact(x.ExpiryDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd")).Distinct())
                            {
                                ExpDateLst.Add(item);
                            }
                        }

                        ExpDateLst = new ObservableCollection<string>(ExpDateLst.OrderBy(p => p));
                        var sortedDatelst = DisplayDateInComboBox(ExpDateLst);
                        ExpDateLst.Clear();
                        ExpDateLst = sortedDatelst;

                        if (ExpDateLst.Count > 0 && ExpDateLst != null)
                        {
                            ExpDateSelected = null;
                            ExpDateSelected = MasterSharedMemory.g_Expiry != null ? MasterSharedMemory.g_Expiry : ExpDateLst[0];
                            //                            NotifyStaticPropertyChanged("ExpDateSelected");
                        }
                        else if (ExpDateSelected == null)
                        {
                            InstrNameSelected = null;
                            StkPriceSelected = string.Empty;
                        }
                    }
                }

                // }
            }
            catch (Exception ex)
            {

                ExceptionUtility.LogError(ex);
            }
        }
        private static void OnChange_Asset()
        {
            try
            {
                if (SelectedSegment == Enumerations.Order.ScripSegment.Derivative.ToString())
                {
                    if (ExpDateLst == null)
                        ExpDateLst = new ObservableCollection<string>();
                    ExpDateLst.Clear();

                    if (AssetList.Count() > 0 && AssetList != null)
                    {
                        if (IntrTypeSelected == Enumerations.Order.InstrumentType.Call.ToString())
                        {
                            foreach (var item in MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Cast<DerivativeMasterBase>().OrderByDescending(y => y.ExpiryDate).Where(x => x.UnderlyingAsset == AssetSelected && x.OptionType == "CE").Select(x => DateTime.ParseExact(x.ExpiryDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd")).Distinct())
                                ExpDateLst.Add(item);
                        }
                        else if (IntrTypeSelected == Enumerations.Order.InstrumentType.Put.ToString())
                        {
                            foreach (var item in MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Cast<DerivativeMasterBase>().OrderByDescending(y => y.ExpiryDate).Where(x => x.UnderlyingAsset == AssetSelected && x.OptionType == "PE").Select(x => DateTime.ParseExact(x.ExpiryDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd")).Distinct())
                                ExpDateLst.Add(item);
                        }
                        else if (IntrTypeSelected == Enumerations.Order.InstrumentType.Future.ToString())
                        {
                            foreach (var item in MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Cast<DerivativeMasterBase>().OrderByDescending(y => y.ExpiryDate).Where(x => x.UnderlyingAsset == AssetSelected && (x.InstrumentType == "SF" || x.InstrumentType == "IF")).Select(x => DateTime.ParseExact(x.ExpiryDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd")).Distinct())
                                ExpDateLst.Add(item);
                        }
                        else if (IntrTypeSelected == Enumerations.Order.InstrumentType.PairOption.ToString())
                        {
                            foreach (var item in MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Cast<DerivativeMasterBase>().OrderByDescending(y => y.ExpiryDate).Where(x => x.UnderlyingAsset == AssetSelected && x.StrategyID == 15).Select(x => DateTime.ParseExact(x.ExpiryDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd")).Distinct())
                                ExpDateLst.Add(item);
                        }

                        ExpDateLst = new ObservableCollection<string>(ExpDateLst.OrderBy(p => p));
                        var sortedDatelst = DisplayDateInComboBox(ExpDateLst);
                        ExpDateLst.Clear();
                        ExpDateLst = sortedDatelst;
                      
                        if (ExpDateLst.Count > 0 && ExpDateLst != null)
                        {
                            ExpDateSelected = null;
                            ExpDateSelected = MasterSharedMemory.g_Expiry != null ? MasterSharedMemory.g_Expiry : ExpDateLst[0];//.ToUpper();
                        }
                    }
                }
                if (SelectedSegment == Enumerations.Order.ScripSegment.Currency.ToString())
                {
                    if (ExpDateLst == null)
                        ExpDateLst = new ObservableCollection<string>();
                    ExpDateLst.Clear();

                    if (AssetList.Count() > 0 && AssetList != null)
                    {
                        if (IntrTypeSelected == Enumerations.Order.InstrumentType.Call.ToString())
                        {
                            foreach (var item in MasterSharedMemory.objMasterCurrencyDictBaseBSE.Values.Cast<CurrencyMasterBase>().OrderByDescending(y => y.ExpiryDate).Where(x => x.UnderlyingAsset == AssetSelected && x.OptionType == "CE").Select(x => DateTime.ParseExact(x.ExpiryDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd")).Distinct())
                                ExpDateLst.Add(item);
                        }
                        else if (IntrTypeSelected == Enumerations.Order.InstrumentType.Put.ToString())
                        {
                            foreach (var item in MasterSharedMemory.objMasterCurrencyDictBaseBSE.Values.Cast<CurrencyMasterBase>().OrderByDescending(y => y.ExpiryDate).Where(x => x.UnderlyingAsset == AssetSelected && x.OptionType == "PE").Select(x => DateTime.ParseExact(x.ExpiryDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd")).Distinct())
                                ExpDateLst.Add(item);
                        }
                        else if (IntrTypeSelected == Enumerations.Order.InstrumentType.Future.ToString())
                        {
                            foreach (var item in MasterSharedMemory.objMasterCurrencyDictBaseBSE.Values.Cast<CurrencyMasterBase>().OrderByDescending(y => y.ExpiryDate).Where(x => x.UnderlyingAsset == AssetSelected && (x.InstrumentType == "FUTIRD" || x.InstrumentType == "FUTCUR" || x.InstrumentType == "FUTIRT")).Select(x => DateTime.ParseExact(x.ExpiryDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd")).Distinct())
                                ExpDateLst.Add(item);
                        }
                        else if (IntrTypeSelected == Enumerations.Order.InstrumentType.PairOption.ToString())
                        {
                            foreach (var item in MasterSharedMemory.objMasterCurrencyDictBaseBSE.Values.Cast<CurrencyMasterBase>().OrderByDescending(y => y.ExpiryDate).Where(x => x.UnderlyingAsset == AssetSelected && x.StrategyID == 15).Select(x => DateTime.ParseExact(x.ExpiryDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd")).Distinct())
                                ExpDateLst.Add(item);
                        }
                        else if (IntrTypeSelected == Enumerations.Order.InstrumentType.Straddle.ToString())
                        {
                            foreach (var item in MasterSharedMemory.objMasterCurrencyDictBaseBSE.Values.Cast<CurrencyMasterBase>().OrderByDescending(y => y.ExpiryDate).Where(x => x.UnderlyingAsset == AssetSelected && x.StrategyID == 28).Select(x => DateTime.ParseExact(x.ExpiryDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd")).Distinct())
                                ExpDateLst.Add(item);
                        }

                        ExpDateLst = new ObservableCollection<string>(ExpDateLst.OrderBy(p => p));
                        var sortedDatelst = DisplayDateInComboBox(ExpDateLst);
                        ExpDateLst.Clear();
                        ExpDateLst = sortedDatelst;

                        if (ExpDateLst.Count > 0 && ExpDateLst != null)
                        {
                            ExpDateSelected = null;
                            ExpDateSelected = MasterSharedMemory.g_Expiry != null ? MasterSharedMemory.g_Expiry : ExpDateLst[0];
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                ExceptionUtility.LogError(ex);
            }
        }

        private static void OnExpiryChange()
        {
            return;
            try
            {
                //BSE DERIVATIVE
                if (SelectedSegment == Enumerations.Order.ScripSegment.Derivative.ToString())
                {
                    if (ExpDateLst.Count() > 0 && ExpDateLst != null)
                    {
                        InstrumentNameColl = new ObservableCollection<string>();
                        if (IntrTypeSelected == Enumerations.Order.InstrumentType.Future.ToString())
                        {
                            foreach (var item in MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Cast<DerivativeMasterBase>().Where(x => x.UnderlyingAsset == AssetSelected && (x.InstrumentType == "SF" || x.InstrumentType == "IF") && x.ExpiryDate == DateTime.ParseExact(ExpDateSelected, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy").ToUpper()).Select(x => x.InstrumentName).Distinct())
                            {
                                InstrumentNameColl.Add(item);
                            }
                        }
                        else if (IntrTypeSelected == Enumerations.Order.InstrumentType.Call.ToString())
                        {
                            StkPrcLst = new ObservableCollection<string>();

                            foreach (var item in MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Cast<DerivativeMasterBase>().OrderByDescending(y => y.StrikePrice).Where(x => x.UnderlyingAsset == AssetSelected && x.OptionType == "CE" && x.ExpiryDate == DateTime.ParseExact(ExpDateSelected, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy").ToUpper() && x.StrategyID == -1).Select(x => x.StrikePrice))
                            {
                                StkPrcLst.Add(Convert.ToString((Convert.ToDecimal(CommonFunctions.DisplayInDecimalFormatTouch(item, DecimalPoint)))));
                            }
                            if (StkPrcLst.Count > 0 && StkPrcLst != null)
                            {
                                StkPriceSelected = null;
                                StkPriceSelected = MasterSharedMemory.g_Strike != null ? MasterSharedMemory.g_Strike : StkPrcLst[0];
                            }
                            foreach (var item in MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Cast<DerivativeMasterBase>().Where(x => x.UnderlyingAsset == AssetSelected && x.ExpiryDate == DateTime.ParseExact(ExpDateSelected, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy").ToUpper() && x.OptionType == "CE" && x.StrikePrice == Convert.ToInt64(Convert.ToDouble(StkPriceSelected) * Math.Pow(10, x.Precision)) && x.StrategyID == -1).Select(x => x.InstrumentName))
                            {
                                InstrumentNameColl.Add(item);
                            }
                        }
                        else if (IntrTypeSelected == Enumerations.Order.InstrumentType.Put.ToString())
                        {
                            StkPrcLst = new ObservableCollection<string>();

                            foreach (var item in MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Cast<DerivativeMasterBase>().OrderByDescending(y => y.StrikePrice).Where(x => x.UnderlyingAsset == AssetSelected && x.OptionType == "PE" && x.ExpiryDate == DateTime.ParseExact(ExpDateSelected, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy").ToUpper() && x.StrategyID == -1).Select(x => x.StrikePrice))
                            {
                                StkPrcLst.Add(Convert.ToString((Convert.ToDecimal(CommonFunctions.DisplayInDecimalFormatTouch(item, DecimalPoint)))));
                            }
                            if (StkPrcLst.Count > 0 && StkPrcLst != null)
                            {
                                StkPriceSelected = null;
                                StkPriceSelected = MasterSharedMemory.g_Strike != null ? MasterSharedMemory.g_Strike : StkPrcLst[0];
                                ScripCodeSelectedLst = MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Cast<DerivativeMasterBase>().Where(x => x.InstrumentName == InstrNameSelected).Select(x => x.ContractTokenNum).ToList();
                            }
                            foreach (var item in MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Cast<DerivativeMasterBase>().Where(x => x.UnderlyingAsset == AssetSelected && x.ExpiryDate == DateTime.ParseExact(ExpDateSelected, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy").ToUpper() && x.OptionType == "PE" && x.StrikePrice == Convert.ToInt64(Convert.ToDouble(StkPriceSelected) * Math.Pow(10, x.Precision)) && x.StrategyID == -1).Select(x => x.InstrumentName))
                            {
                                InstrumentNameColl.Add(item);
                            }
                        }
                        if (InstrumentNameColl.Count > 0 && InstrumentNameColl != null)
                        {
                            InstrNameSelected = null;
                            InstrNameSelected = MasterSharedMemory.g_ScripName != null ? MasterSharedMemory.g_ScripName : InstrumentNameColl[0];
                            ScripCodeSelectedLst = MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Cast<DerivativeMasterBase>().Where(x => x.InstrumentName == InstrNameSelected).Select(x => x.ContractTokenNum).ToList();
                            code = MasterSharedMemory.g_ScripCode != 0 ? MasterSharedMemory.g_ScripCode : ScripCodeSelectedLst[0];
                            BestFiveVM.UpdateTitle((int)code);
                        }

                    }
                    if (ExpDateLst.Count() == 0 || ExpDateLst == null)
                    {
                        StkPriceSelected = string.Empty;
                        StkPrcLst = new ObservableCollection<string>();
                        InstrumentNameColl = new ObservableCollection<string>();
                        InstrNameSelected = string.Empty;
                        ScripCodeSelectedLst = new List<long>();
                        SelectedScripCode = null;

                    }
                    if (code != null && OnScripIDOrCodeChange != null)
                        OnScripIDOrCodeChange((long)code);
                    //else if (OnScripIDOrCodeChange == null)
                    //    OnScripIDOrCodeChange(0);
                }
                //BSE CURRENCY
                if (SelectedSegment == Enumerations.Order.ScripSegment.Currency.ToString())
                {
                    if (ExpDateLst.Count() > 0 && ExpDateLst != null)
                    {
                        InstrumentNameColl = new ObservableCollection<string>();
                        if (IntrTypeSelected == Enumerations.Order.InstrumentType.Future.ToString())
                        {
                            foreach (var item in MasterSharedMemory.objMasterCurrencyDictBaseBSE.Values.Cast<CurrencyMasterBase>().Where(x => x.UnderlyingAsset == AssetSelected && (x.InstrumentType == "FUTIRD" || x.InstrumentType == "FUTCUR" || x.InstrumentType == "FUTIRT") && x.ExpiryDate == DateTime.ParseExact(ExpDateSelected, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy").ToUpper()).Select(x => x.InstrumentName).Distinct())
                            {
                                InstrumentNameColl.Add(item);
                            }
                        }

                        else if (IntrTypeSelected == Enumerations.Order.InstrumentType.Call.ToString())
                        {
                            StkPrcLst = new ObservableCollection<string>();

                            foreach (var item in MasterSharedMemory.objMasterCurrencyDictBaseBSE.Values.Cast<CurrencyMasterBase>().OrderByDescending(y => y.StrikePrice).Where(x => x.UnderlyingAsset == AssetSelected && x.OptionType == "CE" && x.ExpiryDate == DateTime.ParseExact(ExpDateSelected, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy").ToUpper() && x.StrategyID == -1).Select(x => x.StrikePrice))
                            {
                                StkPrcLst.Add(Convert.ToString((Convert.ToDecimal(CommonFunctions.DisplayInDecimalFormatTouch(item, DecimalPoint)))));
                            }
                            if (StkPrcLst.Count > 0 && StkPrcLst != null)
                            {
                                StkPriceSelected = null;
                                StkPriceSelected = MasterSharedMemory.g_Strike != null ? MasterSharedMemory.g_Strike : StkPrcLst[0];
                            }
                            foreach (var item in MasterSharedMemory.objMasterCurrencyDictBaseBSE.Values.Cast<CurrencyMasterBase>().Where(x => x.UnderlyingAsset == AssetSelected && x.ExpiryDate == DateTime.ParseExact(ExpDateSelected, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy").ToUpper() && x.OptionType == "CE" && x.StrikePrice == Convert.ToInt64(Convert.ToDouble(StkPriceSelected) * Math.Pow(10, x.Precision)) && x.StrategyID == -1).Select(x => x.InstrumentName).Distinct())
                            {
                                InstrumentNameColl.Add(item);
                            }
                        }
                        else if (IntrTypeSelected == Enumerations.Order.InstrumentType.Put.ToString())
                        {
                            StkPrcLst = new ObservableCollection<string>();

                            foreach (var item in MasterSharedMemory.objMasterCurrencyDictBaseBSE.Values.Cast<CurrencyMasterBase>().OrderByDescending(y => y.StrikePrice).Where(x => x.UnderlyingAsset == AssetSelected && x.OptionType == "PE" && x.ExpiryDate == DateTime.ParseExact(ExpDateSelected, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy").ToUpper() && x.StrategyID == -1).Select(x => x.StrikePrice))
                            {
                                StkPrcLst.Add(Convert.ToString((Convert.ToDecimal(CommonFunctions.DisplayInDecimalFormatTouch(item, DecimalPoint)))));
                            }

                            if (StkPrcLst.Count > 0 && StkPrcLst != null)
                            {
                                StkPriceSelected = null;
                                StkPriceSelected = MasterSharedMemory.g_Strike != null ? MasterSharedMemory.g_Strike : StkPrcLst[0];
                            }
                            foreach (var item in MasterSharedMemory.objMasterCurrencyDictBaseBSE.Values.Cast<CurrencyMasterBase>().Where(x => (x.UnderlyingAsset == AssetSelected) && (x.ExpiryDate == DateTime.ParseExact(ExpDateSelected, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy").ToUpper()) && (x.OptionType == "PE") && (x.StrikePrice == Convert.ToInt64(Convert.ToDouble(StkPriceSelected) * Math.Pow(10, x.Precision))) && x.StrategyID == -1).Select(x => x.InstrumentName).Distinct())
                            {
                                InstrumentNameColl.Add(item);
                            }
                        }

                        if (InstrumentNameColl.Count > 0 && InstrumentNameColl != null)
                        {
                            InstrNameSelected = null;
                            InstrNameSelected = MasterSharedMemory.g_ScripName != null ? MasterSharedMemory.g_ScripName : InstrumentNameColl[0];
                            ScripCodeSelectedLst = MasterSharedMemory.objMasterCurrencyDictBaseBSE.Values.Cast<CurrencyMasterBase>().Where(x => x.InstrumentName == InstrNameSelected).Select(x => x.ContractTokenNum).ToList();

                            //SelectedScripCode = ScripCodeSelectedLst[0];
                            long code = MasterSharedMemory.g_ScripCode != 0 ? MasterSharedMemory.g_ScripCode : ScripCodeSelectedLst[0];
                            BestFiveVM.UpdateTitle((int)code);
                        }
                    }
                    if (ExpDateLst.Count() == 0 || ExpDateLst == null)
                    {
                        StkPriceSelected = string.Empty;
                        StkPrcLst = new ObservableCollection<string>();
                        InstrumentNameColl = new ObservableCollection<string>();
                        InstrNameSelected = string.Empty;
                        ScripCodeSelectedLst = new List<long>();
                        SelectedScripCode = null;
                    }
                    if (code != null && OnScripIDOrCodeChange != null)
                        OnScripIDOrCodeChange((long)code);
                    //else if (OnScripIDOrCodeChange == null)
                    //    OnScripIDOrCodeChange(0);
                }
            }
            catch (Exception ex)
            {

                ExceptionUtility.LogError(ex);
            }
        }
        private static void OnChange_Expiry()
        {
            try
            {
                if (SelectedSegment == Enumerations.Order.ScripSegment.Derivative.ToString())
                {
                    if (ExpDateLst.Count() > 0 && ExpDateLst != null)
                    {
                        if (IntrTypeSelected == Enumerations.Order.InstrumentType.Call.ToString())
                        {
                            if (StkPrcLst == null)
                                StkPrcLst = new ObservableCollection<string>();
                            StkPrcLst.Clear();

                            foreach (var item in MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Cast<DerivativeMasterBase>().OrderByDescending(y => y.StrikePrice).Where(x => x.UnderlyingAsset == AssetSelected && x.OptionType == "CE" && x.ExpiryDate == DateTime.ParseExact(ExpDateSelected, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy").ToUpper() && x.StrategyID == -1).Select(x => x.StrikePrice))
                                StkPrcLst.Add(Convert.ToString((Convert.ToDecimal(CommonFunctions.DisplayInDecimalFormatTouch(item, 2)))));

                            if (StkPrcLst.Count > 0 && StkPrcLst != null)
                            {
                                StkPriceSelected = null;
                                StkPriceSelected = MasterSharedMemory.g_Strike != null ? MasterSharedMemory.g_Strike : StkPrcLst[0];
                            }

                            //foreach (var item in MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Cast<DerivativeMasterBase>().Where(x => x.UnderlyingAsset == AssetSelected && x.ExpiryDate == DateTime.ParseExact(ExpDateSelected, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy").ToUpper() && x.OptionType == "CE" && x.StrikePrice == Convert.ToInt64(Convert.ToDouble(StkPriceSelected) * Math.Pow(10, x.Precision)) && x.StrategyID == -1).Select(x => x.InstrumentName))
                            //    InstrumentNameColl.Add(item);
                        }
                        else if (IntrTypeSelected == Enumerations.Order.InstrumentType.Put.ToString())
                        {
                            if (StkPrcLst == null)
                                StkPrcLst = new ObservableCollection<string>();
                            StkPrcLst.Clear();

                            foreach (var item in MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Cast<DerivativeMasterBase>().OrderByDescending(y => y.StrikePrice).Where(x => x.UnderlyingAsset == AssetSelected && x.OptionType == "PE" && x.ExpiryDate == DateTime.ParseExact(ExpDateSelected, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy").ToUpper() && x.StrategyID == -1).Select(x => x.StrikePrice))
                                StkPrcLst.Add(Convert.ToString((Convert.ToDecimal(CommonFunctions.DisplayInDecimalFormatTouch(item, 2)))));

                            if (StkPrcLst.Count > 0 && StkPrcLst != null)
                            {
                                StkPriceSelected = null;
                                StkPriceSelected = MasterSharedMemory.g_Strike != null ? MasterSharedMemory.g_Strike : StkPrcLst[0];
                            }

                            //foreach (var item in MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Cast<DerivativeMasterBase>().Where(x => x.UnderlyingAsset == AssetSelected && x.ExpiryDate == DateTime.ParseExact(ExpDateSelected, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy").ToUpper() && x.OptionType == "PE" && x.StrikePrice == Convert.ToInt64(Convert.ToDouble(StkPriceSelected) * Math.Pow(10, x.Precision)) && x.StrategyID == -1).Select(x => x.InstrumentName))
                            //    InstrumentNameColl.Add(item);
                        }
                        else if (IntrTypeSelected == Enumerations.Order.InstrumentType.Future.ToString())
                        {
                            if (InstrumentNameColl == null)
                                InstrumentNameColl = new ObservableCollection<string>();
                            InstrumentNameColl.Clear();

                            foreach (var item in MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Cast<DerivativeMasterBase>().Where(x => x.UnderlyingAsset == AssetSelected && (x.InstrumentType == "SF" || x.InstrumentType == "IF") && x.ExpiryDate == DateTime.ParseExact(ExpDateSelected, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy").ToUpper()).Select(x => x.InstrumentName).Distinct())
                                InstrumentNameColl.Add(item);

                            if (InstrumentNameColl.Count > 0 && InstrumentNameColl != null)
                            {
                                InstrNameSelected = null;
                                InstrNameSelected = MasterSharedMemory.g_ScripName != null ? MasterSharedMemory.g_ScripName : InstrumentNameColl[0];

                                //ScripCodeSelectedLst = MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Cast<DerivativeMasterBase>().Where(x => x.InstrumentName == InstrNameSelected).Select(x => x.ContractTokenNum).ToList();
                                //code = MasterSharedMemory.g_ScripCode != 0 ? MasterSharedMemory.g_ScripCode : ScripCodeSelectedLst[0];
                                //BestFiveVM.UpdateTitle((int)code);

                                //if (code != null && OnScripIDOrCodeChange != null)
                                //    OnScripIDOrCodeChange((long)code);
                            }
                        }
                        else if (IntrTypeSelected == Enumerations.Order.InstrumentType.PairOption.ToString())
                        {
                            if (StkPrcLst == null)
                                StkPrcLst = new ObservableCollection<string>();
                            StkPrcLst.Clear();

                            foreach (var item in MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Cast<DerivativeMasterBase>().OrderByDescending(y => y.StrikePrice).Where(x => x.UnderlyingAsset == AssetSelected && x.ExpiryDate == DateTime.ParseExact(ExpDateSelected, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy").ToUpper() && x.StrategyID == 15).Select(x => x.StrikePrice))
                                StkPrcLst.Add(Convert.ToString((Convert.ToDecimal(CommonFunctions.DisplayInDecimalFormatTouch(item, 2)))));

                            if (StkPrcLst.Count > 0 && StkPrcLst != null)
                            {
                                StkPriceSelected = null;
                                StkPriceSelected = MasterSharedMemory.g_Strike != null ? MasterSharedMemory.g_Strike : StkPrcLst[0];
                            }
                        }
                    }
                }
                if (SelectedSegment == Enumerations.Order.ScripSegment.Currency.ToString())
                {
                    if (ExpDateLst.Count() > 0 && ExpDateLst != null)
                    {
                        if (IntrTypeSelected == Enumerations.Order.InstrumentType.Call.ToString())
                        {
                            if (StkPrcLst == null)
                                StkPrcLst = new ObservableCollection<string>();
                            StkPrcLst.Clear();

                            foreach (var item in MasterSharedMemory.objMasterCurrencyDictBaseBSE.Values.Cast<CurrencyMasterBase>().OrderByDescending(y => y.StrikePrice).Where(x => x.UnderlyingAsset == AssetSelected && x.OptionType == "CE" && x.ExpiryDate == DateTime.ParseExact(ExpDateSelected, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy").ToUpper() && x.StrategyID == -1).Select(x => x.StrikePrice))
                                StkPrcLst.Add(Convert.ToString((Convert.ToDecimal(CommonFunctions.DisplayInDecimalFormatTouch(item, 4)))));

                            if (StkPrcLst.Count > 0 && StkPrcLst != null)
                            {
                                StkPriceSelected = null;
                                StkPriceSelected = MasterSharedMemory.g_Strike != null ? MasterSharedMemory.g_Strike : StkPrcLst[0];
                            }

                            //foreach (var item in MasterSharedMemory.objMasterCurrencyDictBaseBSE.Values.Cast<CurrencyMasterBase>().Where(x => x.UnderlyingAsset == AssetSelected && x.ExpiryDate == DateTime.ParseExact(ExpDateSelected, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy").ToUpper() && x.OptionType == "CE" && x.StrikePrice == Convert.ToInt64(Convert.ToDouble(StkPriceSelected) * Math.Pow(10, x.Precision)) && x.StrategyID == -1).Select(x => x.InstrumentName).Distinct())
                            //    InstrumentNameColl.Add(item);
                        }
                        else if (IntrTypeSelected == Enumerations.Order.InstrumentType.Put.ToString())
                        {
                            if (StkPrcLst == null)
                                StkPrcLst = new ObservableCollection<string>();
                            StkPrcLst.Clear();

                            foreach (var item in MasterSharedMemory.objMasterCurrencyDictBaseBSE.Values.Cast<CurrencyMasterBase>().OrderByDescending(y => y.StrikePrice).Where(x => x.UnderlyingAsset == AssetSelected && x.OptionType == "PE" && x.ExpiryDate == DateTime.ParseExact(ExpDateSelected, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy").ToUpper() && x.StrategyID == -1).Select(x => x.StrikePrice))
                                StkPrcLst.Add(Convert.ToString((Convert.ToDecimal(CommonFunctions.DisplayInDecimalFormatTouch(item, 4)))));

                            if (StkPrcLst.Count > 0 && StkPrcLst != null)
                            {
                                StkPriceSelected = null;
                                StkPriceSelected = MasterSharedMemory.g_Strike != null ? MasterSharedMemory.g_Strike : StkPrcLst[0];
                            }

                            //foreach (var item in MasterSharedMemory.objMasterCurrencyDictBaseBSE.Values.Cast<CurrencyMasterBase>().Where(x => (x.UnderlyingAsset == AssetSelected) && (x.ExpiryDate == DateTime.ParseExact(ExpDateSelected, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy").ToUpper()) && (x.OptionType == "PE") && (x.StrikePrice == Convert.ToInt64(Convert.ToDouble(StkPriceSelected) * Math.Pow(10, x.Precision))) && x.StrategyID == -1).Select(x => x.InstrumentName).Distinct())
                            //    InstrumentNameColl.Add(item);
                        }
                        else if (IntrTypeSelected == Enumerations.Order.InstrumentType.Future.ToString())
                        {
                            if (InstrumentNameColl == null)
                                InstrumentNameColl = new ObservableCollection<string>();
                            InstrumentNameColl.Clear();

                            foreach (var item in MasterSharedMemory.objMasterCurrencyDictBaseBSE.Values.Cast<CurrencyMasterBase>().Where(x => x.UnderlyingAsset == AssetSelected && (x.InstrumentType == "FUTIRD" || x.InstrumentType == "FUTCUR" || x.InstrumentType == "FUTIRT") && x.ExpiryDate == DateTime.ParseExact(ExpDateSelected, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy").ToUpper()).Select(x => x.InstrumentName).Distinct())
                                InstrumentNameColl.Add(item);

                            if (InstrumentNameColl.Count > 0 && InstrumentNameColl != null)
                            {
                                InstrNameSelected = null;
                                InstrNameSelected = MasterSharedMemory.g_ScripName != null ? MasterSharedMemory.g_ScripName : InstrumentNameColl[0];

                                //ScripCodeSelectedLst = MasterSharedMemory.objMasterCurrencyDictBaseBSE.Values.Cast<CurrencyMasterBase>().Where(x => x.InstrumentName == InstrNameSelected).Select(x => x.ContractTokenNum).ToList();
                                //long code = MasterSharedMemory.g_ScripCode != 0 ? MasterSharedMemory.g_ScripCode : ScripCodeSelectedLst[0];
                                //BestFiveVM.UpdateTitle((int)code);

                                //if (code != null && OnScripIDOrCodeChange != null)
                                //    OnScripIDOrCodeChange((long)code);
                            }
                        }
                        else if (IntrTypeSelected == Enumerations.Order.InstrumentType.PairOption.ToString())
                        {
                            if (StkPrcLst == null)
                                StkPrcLst = new ObservableCollection<string>();
                            StkPrcLst.Clear();

                            foreach (var item in MasterSharedMemory.objMasterCurrencyDictBaseBSE.Values.Cast<CurrencyMasterBase>().OrderByDescending(y => y.StrikePrice).Where(x => x.UnderlyingAsset == AssetSelected && x.ExpiryDate == DateTime.ParseExact(ExpDateSelected, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy").ToUpper() && x.StrategyID == 15).Select(x => x.StrikePrice))
                                StkPrcLst.Add(Convert.ToString((Convert.ToDecimal(CommonFunctions.DisplayInDecimalFormatTouch(item, 4)))));

                            if (StkPrcLst.Count > 0 && StkPrcLst != null)
                            {
                                StkPriceSelected = null;
                                StkPriceSelected = MasterSharedMemory.g_Strike != null ? MasterSharedMemory.g_Strike : StkPrcLst[0];
                            }
                        }
                        else if (IntrTypeSelected == Enumerations.Order.InstrumentType.Straddle.ToString())
                        {
                            if (StkPrcLst == null)
                                StkPrcLst = new ObservableCollection<string>();
                            StkPrcLst.Clear();

                            foreach (var item in MasterSharedMemory.objMasterCurrencyDictBaseBSE.Values.Cast<CurrencyMasterBase>().OrderByDescending(y => y.StrikePrice).Where(x => x.UnderlyingAsset == AssetSelected && x.ExpiryDate == DateTime.ParseExact(ExpDateSelected, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy").ToUpper() && x.StrategyID == 28).Select(x => x.StrikePrice))
                                StkPrcLst.Add(Convert.ToString((Convert.ToDecimal(CommonFunctions.DisplayInDecimalFormatTouch(item, 4)))));

                            if (StkPrcLst.Count > 0 && StkPrcLst != null)
                            {
                                StkPriceSelected = null;
                                StkPriceSelected = MasterSharedMemory.g_Strike != null ? MasterSharedMemory.g_Strike : StkPrcLst[0];
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

        private static void OnChangeOfAssetOfStkPrice()
        {
            return;
            try
            {
                //BSE DERIVATIVES

                if (SelectedSegment == Enumerations.Order.ScripSegment.Derivative.ToString())
                {
                    if (StkPrcLst.Count() > 0 && StkPrcLst != null)
                    {
                        //  long StkPrice = Convert.ToInt64(StkPriceSelected);
                        InstrumentNameColl = new ObservableCollection<string>();
                        if (IntrTypeSelected == Enumerations.Order.InstrumentType.Call.ToString())
                        {
                            foreach (var item in MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Cast<DerivativeMasterBase>().Where(x => x.UnderlyingAsset == AssetSelected && x.ExpiryDate == DateTime.ParseExact(ExpDateSelected, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy").ToUpper() && x.OptionType == "CE" && x.StrikePrice == Convert.ToInt64(Convert.ToDouble(StkPriceSelected) * Math.Pow(10, x.Precision)) && x.StrategyID == -1).Select(x => x.InstrumentName).Distinct())
                            {
                                InstrumentNameColl.Add(item);
                            }
                        }

                        else if (IntrTypeSelected == Enumerations.Order.InstrumentType.Put.ToString())
                        {
                            foreach (var item in MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Cast<DerivativeMasterBase>().Where(x => x.UnderlyingAsset == AssetSelected && x.ExpiryDate == DateTime.ParseExact(ExpDateSelected, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy").ToUpper() && x.OptionType == "PE" && x.StrikePrice == Convert.ToInt64(Convert.ToDouble(StkPriceSelected) * Math.Pow(10, x.Precision)) && x.StrategyID == -1).Select(x => x.InstrumentName))
                            {
                                InstrumentNameColl.Add(item);
                            }
                        }

                        if (InstrumentNameColl.Count > 0 && InstrumentNameColl != null)
                        {
                            InstrNameSelected = null;
                            InstrNameSelected = MasterSharedMemory.g_ScripName != null ? MasterSharedMemory.g_ScripName : InstrumentNameColl[0];
                            ScripCodeSelectedLst = MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Cast<DerivativeMasterBase>().Where(x => x.InstrumentName == InstrNameSelected).Select(x => x.ContractTokenNum).ToList();
                            code = MasterSharedMemory.g_ScripCode != 0 ? MasterSharedMemory.g_ScripCode : ScripCodeSelectedLst[0];
                            BestFiveVM.UpdateTitle((int)code);
                        }
                        else
                            InstrNameSelected = string.Empty;
                    }
                    if (code != null && OnScripIDOrCodeChange != null)
                        OnScripIDOrCodeChange((long)code);
                    //else if (OnScripIDOrCodeChange == null)
                    //    OnScripIDOrCodeChange(0);
                }
                //BSE CURRENCY
                if (SelectedSegment == Enumerations.Order.ScripSegment.Currency.ToString())
                {
                    if (StkPrcLst != null && StkPrcLst?.Count() > 0)
                    {
                        InstrumentNameColl = new ObservableCollection<string>();

                        if (IntrTypeSelected == Enumerations.Order.InstrumentType.Call.ToString())
                        {
                            foreach (var item in MasterSharedMemory.objMasterCurrencyDictBaseBSE.Values.Cast<CurrencyMasterBase>().Where(x => x.UnderlyingAsset == AssetSelected && x.ExpiryDate == DateTime.ParseExact(ExpDateSelected, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy").ToUpper() && x.OptionType == "CE" && x.StrikePrice == Convert.ToInt64(Convert.ToDouble(StkPriceSelected) * Math.Pow(10, x.Precision)) && x.StrategyID == -1).Select(x => x.InstrumentName))
                            {
                                InstrumentNameColl.Add(item);
                            }
                        }
                        else if (IntrTypeSelected == Enumerations.Order.InstrumentType.Put.ToString())
                        {
                            foreach (var item in MasterSharedMemory.objMasterCurrencyDictBaseBSE.Values.Cast<CurrencyMasterBase>().Where(x => x.UnderlyingAsset == AssetSelected && x.ExpiryDate == DateTime.ParseExact(ExpDateSelected, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy").ToUpper() && x.OptionType == "PE" && x.StrikePrice == Convert.ToInt64(Convert.ToDouble(StkPriceSelected) * Math.Pow(10, x.Precision)) && x.StrategyID == -1).Select(x => x.InstrumentName))
                            {
                                InstrumentNameColl.Add(item);
                            }
                        }
                        if (InstrumentNameColl.Count > 0 && InstrumentNameColl != null)
                        {
                            InstrNameSelected = null;
                            InstrNameSelected = MasterSharedMemory.g_ScripName != null ? MasterSharedMemory.g_ScripName : InstrumentNameColl[0];
                            ScripCodeSelectedLst = MasterSharedMemory.objMasterCurrencyDictBaseBSE.Values.Cast<CurrencyMasterBase>().Where(x => x.InstrumentName == InstrNameSelected).Select(x => x.ContractTokenNum).ToList();
                            code = MasterSharedMemory.g_ScripCode != 0 ? MasterSharedMemory.g_ScripCode : ScripCodeSelectedLst[0];
                            BestFiveVM.UpdateTitle((int)code);
                        }
                        else
                            InstrNameSelected = string.Empty;
                    }
                    if (code != null && OnScripIDOrCodeChange != null)
                        OnScripIDOrCodeChange((long)code);
                    //else if (OnScripIDOrCodeChange == null)
                    //    OnScripIDOrCodeChange(0);
                }
            }

            catch (Exception ex)
            {

                ExceptionUtility.LogError(ex);
            }
        }
        private static void OnChange_StkPrice()
        {
            try
            {
                if (SelectedSegment == Enumerations.Order.ScripSegment.Derivative.ToString())
                {
                    if(InstrumentNameColl == null)
                        InstrumentNameColl = new ObservableCollection<string>();
                    InstrumentNameColl.Clear();

                    if (StkPrcLst.Count() > 0 && StkPrcLst != null)
                    {
                        if (IntrTypeSelected == Enumerations.Order.InstrumentType.Call.ToString())
                        {
                            foreach (var item in MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Cast<DerivativeMasterBase>().Where(x => x.UnderlyingAsset == AssetSelected && x.ExpiryDate == DateTime.ParseExact(ExpDateSelected, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy").ToUpper() && x.OptionType == "CE" && x.StrikePrice == Convert.ToInt64(Convert.ToDouble(StkPriceSelected) * Math.Pow(10, x.Precision)) && x.StrategyID == -1).Select(x => x.InstrumentName).Distinct())
                                InstrumentNameColl.Add(item);
                        }
                        else if (IntrTypeSelected == Enumerations.Order.InstrumentType.Put.ToString())
                        {
                            foreach (var item in MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Cast<DerivativeMasterBase>().Where(x => x.UnderlyingAsset == AssetSelected && x.ExpiryDate == DateTime.ParseExact(ExpDateSelected, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy").ToUpper() && x.OptionType == "PE" && x.StrikePrice == Convert.ToInt64(Convert.ToDouble(StkPriceSelected) * Math.Pow(10, x.Precision)) && x.StrategyID == -1).Select(x => x.InstrumentName))
                                InstrumentNameColl.Add(item);
                        }
                        else if (IntrTypeSelected == Enumerations.Order.InstrumentType.PairOption.ToString())
                        {
                            foreach (var item in MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Cast<DerivativeMasterBase>().Where(x => x.UnderlyingAsset == AssetSelected && x.ExpiryDate == DateTime.ParseExact(ExpDateSelected, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy").ToUpper() && x.StrikePrice == Convert.ToInt64(Convert.ToDouble(StkPriceSelected) * Math.Pow(10, x.Precision)) && x.StrategyID == 15).Select(x => x.InstrumentName).Distinct())
                                InstrumentNameColl.Add(item);
                        }

                        if (InstrumentNameColl.Count > 0 && InstrumentNameColl != null)
                        {
                            InstrNameSelected = null;
                            InstrNameSelected = MasterSharedMemory.g_ScripName != null ? MasterSharedMemory.g_ScripName : InstrumentNameColl[0];
                            //ScripCodeSelectedLst = MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Cast<DerivativeMasterBase>().Where(x => x.InstrumentName == InstrNameSelected).Select(x => x.ContractTokenNum).ToList();
                            //code = MasterSharedMemory.g_ScripCode != 0 ? MasterSharedMemory.g_ScripCode : ScripCodeSelectedLst[0];
                            //BestFiveVM.UpdateTitle((int)code);

                            //if (code != null && OnScripIDOrCodeChange != null)
                            //    OnScripIDOrCodeChange((long)code);
                        }
                    }
                }
                //BSE CURRENCY
                if (SelectedSegment == Enumerations.Order.ScripSegment.Currency.ToString())
                {
                    if (InstrumentNameColl == null)
                        InstrumentNameColl = new ObservableCollection<string>();
                    InstrumentNameColl.Clear();

                    if (StkPrcLst != null && StkPrcLst?.Count() > 0)
                    {
                        if (IntrTypeSelected == Enumerations.Order.InstrumentType.Call.ToString())
                        {
                            foreach (var item in MasterSharedMemory.objMasterCurrencyDictBaseBSE.Values.Cast<CurrencyMasterBase>().Where(x => x.UnderlyingAsset == AssetSelected && x.ExpiryDate == DateTime.ParseExact(ExpDateSelected, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy").ToUpper() && x.OptionType == "CE" && x.StrikePrice == Convert.ToInt64(Convert.ToDouble(StkPriceSelected) * Math.Pow(10, x.Precision)) && x.StrategyID == -1).Select(x => x.InstrumentName))
                                InstrumentNameColl.Add(item);
                        }
                        else if (IntrTypeSelected == Enumerations.Order.InstrumentType.Put.ToString())
                        {
                            foreach (var item in MasterSharedMemory.objMasterCurrencyDictBaseBSE.Values.Cast<CurrencyMasterBase>().Where(x => x.UnderlyingAsset == AssetSelected && x.ExpiryDate == DateTime.ParseExact(ExpDateSelected, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy").ToUpper() && x.OptionType == "PE" && x.StrikePrice == Convert.ToInt64(Convert.ToDouble(StkPriceSelected) * Math.Pow(10, x.Precision)) && x.StrategyID == -1).Select(x => x.InstrumentName))
                                InstrumentNameColl.Add(item);
                        }
                        else if (IntrTypeSelected == Enumerations.Order.InstrumentType.PairOption.ToString())
                        {
                            foreach (var item in MasterSharedMemory.objMasterCurrencyDictBaseBSE.Values.Cast<CurrencyMasterBase>().Where(x => x.UnderlyingAsset == AssetSelected && x.ExpiryDate == DateTime.ParseExact(ExpDateSelected, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy").ToUpper() && x.StrikePrice == Convert.ToInt64(Convert.ToDouble(StkPriceSelected) * Math.Pow(10, x.Precision)) && x.StrategyID == 15).Select(x => x.InstrumentName))
                                InstrumentNameColl.Add(item);
                        }
                        else if (IntrTypeSelected == Enumerations.Order.InstrumentType.Straddle.ToString())
                        {
                            foreach (var item in MasterSharedMemory.objMasterCurrencyDictBaseBSE.Values.Cast<CurrencyMasterBase>().Where(x => x.UnderlyingAsset == AssetSelected && x.ExpiryDate == DateTime.ParseExact(ExpDateSelected, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy").ToUpper() && x.StrikePrice == Convert.ToInt64(Convert.ToDouble(StkPriceSelected) * Math.Pow(10, x.Precision)) && x.StrategyID == 28).Select(x => x.InstrumentName))
                                InstrumentNameColl.Add(item);
                        }
                        if (InstrumentNameColl.Count > 0 && InstrumentNameColl != null)
                        {
                            InstrNameSelected = null;
                            InstrNameSelected = MasterSharedMemory.g_ScripName != null ? MasterSharedMemory.g_ScripName : InstrumentNameColl[0];
                            //ScripCodeSelectedLst = MasterSharedMemory.objMasterCurrencyDictBaseBSE.Values.Cast<CurrencyMasterBase>().Where(x => x.InstrumentName == InstrNameSelected).Select(x => x.ContractTokenNum).ToList();
                            //code = MasterSharedMemory.g_ScripCode != 0 ? MasterSharedMemory.g_ScripCode : ScripCodeSelectedLst[0];
                            //BestFiveVM.UpdateTitle((int)code);

                            //if (code != null && OnScripIDOrCodeChange != null)
                            //    OnScripIDOrCodeChange((long)code);
                        }
                    }
                }
            }

            catch (Exception ex)
            {

                ExceptionUtility.LogError(ex);
            }
        }

        private static void OnChangeOfInstrName()
        {
            return;
            if (SelectedSegment == Enumerations.Order.ScripSegment.Derivative.ToString())
            {
                if (InstrumentNameColl.Count > 0 && InstrumentNameColl != null)
                {
                    //InstrNameSelected = InstrumentNameColl[0];
                    ScripCodeSelectedLst = MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Cast<DerivativeMasterBase>().Where(x => x.InstrumentName == InstrNameSelected).Select(x => x.ContractTokenNum).ToList();
                    code = MasterSharedMemory.g_ScripCode != 0 ? MasterSharedMemory.g_ScripCode : ScripCodeSelectedLst[0];
                    BestFiveVM.UpdateTitle((int)code);
                }

                //InstrNameSelected = string.Empty;

            }
            if (SelectedSegment == Enumerations.Order.ScripSegment.Currency.ToString())
            {
                if (InstrumentNameColl.Count > 0 && InstrumentNameColl != null)
                {
                    //  InstrNameSelected = InstrumentNameColl[0];
                    ScripCodeSelectedLst = MasterSharedMemory.objMasterCurrencyDictBaseBSE.Values.Cast<CurrencyMasterBase>().Where(x => x.InstrumentName == InstrNameSelected).Select(x => x.ContractTokenNum).ToList();
                    code = MasterSharedMemory.g_ScripCode != 0 ? MasterSharedMemory.g_ScripCode : ScripCodeSelectedLst[0];
                    BestFiveVM.UpdateTitle((int)code);
                }

                //InstrNameSelected = string.Empty;
            }

            changeScripId = true;
            SelectedScripCode = code;
            if (SelectedScripCode != null && OnScripIDOrCodeChange != null)
                OnScripIDOrCodeChange((long)SelectedScripCode);

        }
        private static void OnChange_InstrName()
        {
            if (SelectedSegment == Enumerations.Order.ScripSegment.Derivative.ToString())
            {
                if (InstrumentNameColl.Count > 0 && InstrumentNameColl != null)
                {
                    ScripCodeSelectedLst = MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Cast<DerivativeMasterBase>().Where(x => x.InstrumentName == InstrNameSelected).Select(x => x.ContractTokenNum).ToList();
                    code = MasterSharedMemory.g_ScripCode != 0 ? MasterSharedMemory.g_ScripCode : ScripCodeSelectedLst[0];
                }
            }
            if (SelectedSegment == Enumerations.Order.ScripSegment.Currency.ToString())
            {
                if (InstrumentNameColl.Count > 0 && InstrumentNameColl != null)
                {
                    ScripCodeSelectedLst = MasterSharedMemory.objMasterCurrencyDictBaseBSE.Values.Cast<CurrencyMasterBase>().Where(x => x.InstrumentName == InstrNameSelected).Select(x => x.ContractTokenNum).ToList();
                    code = MasterSharedMemory.g_ScripCode != 0 ? MasterSharedMemory.g_ScripCode : ScripCodeSelectedLst[0];
                }
            }
            SelectedScripCode = code;
            //if (SelectedScripCode != null && OnScripIDOrCodeChange != null)
            //    OnScripIDOrCodeChange((long)SelectedScripCode);
        }

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
        private void Window_Loaded_Event(object e)
        {
            ScripInsertUC usrcntrls = e as ScripInsertUC;
            usrcntrls.ddlScripIdEquity.Focus();
            //var targetWindow = Window.GetWindow(usrcntrls);
            //targetWindow.Focus();
        }

        public void SelectScripCodeFromTouchline(int scriptcode1)
        {
            SelectScripCode(scriptcode1);
        }

        private void OpenHourlyStatisticsWindow(object e)
        {
            try
            {
                HourlyStatistics objHS = System.Windows.Application.Current.Windows.OfType<HourlyStatistics>().FirstOrDefault();
                if (objHS != null)
                {
                    if (SelectedScripCode != null)
                    {
                        HourlyStatisticsVM.UpdateTitleHourlyStatistics(Convert.ToInt32(SelectedScripCode), true);
                    }
                    objHS.Show();
                    objHS.Focus();
                }
                else
                {
                    objHS = new HourlyStatistics();
                    objHS.Owner = Application.Current.MainWindow;
                    objHS.Activate();
                    if (SelectedScripCode != null)
                    {
                        HourlyStatisticsVM.UpdateTitleHourlyStatistics(Convert.ToInt32(SelectedScripCode), true);
                    }
                    objHS.Show();

                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
            }
        }

        private void InfoWindow()
        {
            try
            {
                string url = "http://10.1.101.125:3000/twsreports/StockReach.aspx?sc=" + SelectedScripCode + "&memid=" + UtilityLoginDetails.GETInstance.MemberId + "&trdid=" + UtilityLoginDetails.GETInstance.TraderId;
                ProcessStartInfo sInfo = new ProcessStartInfo(url);
                Process.Start(sInfo);

            }
            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
            }
        }

        private void ChartWindowOpen()
        {
            try
            {
                string url = "http://www.bseindia.com/charting/BSETechnicalCharts.aspx?SYMBOL=" + SelectedScripCode;
                ProcessStartInfo sInfo = new ProcessStartInfo(url);
                Process.Start(sInfo);

            }
            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
            }
        }


        
        
        
        

        private static ObservableCollection<string> DisplayDateInComboBox(ObservableCollection<string> ExpDate)
        {
            ObservableCollection<string> sortedList = new ObservableCollection<string>();
            foreach (var item in ExpDate)//.Select(x => (DateTime.ParseExact(item., "dd-MMM-yyyy", CultureInfo.InvariantCulture).ToString("yy-MM-dd"))).Distinct())
                if (!(item == " All" || item == "All"))
                    sortedList.Add(DateTime.ParseExact(item, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("dd-MM-yyyy"));
                else
                    sortedList.Add(item);
            return sortedList;
        }


        private static ObservableCollection<string> DisplayDateInComboBoxddMMMyyyy(ObservableCollection<string> ExpDate)
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
                    sortedList.Add(DateTime.ParseExact(item.ToUpper(), "dd-MM-yy", CultureInfo.InvariantCulture).ToString("dd-MM-yyyy"));
                else
                    sortedList.Add(item.ToUpper());
            return sortedList;
        }


        

        

        

        private void EscapeUsingUserControl(object e)
        {
            UserControl usrcntrl = e as UserControl;
            var targetWindow = Window.GetWindow(usrcntrl);
            TouchLineInsert = false;
            if (OnScripIDOrCodeChange == null)
            {
                OnScripIDOrCodeChange += BestFiveVM.UpdateValues;
                //OnScripIDOrCodeChange += BestFiveVM.FetchNetPositionByScripCode;
            }
            targetWindow.Hide();
            //  Application.Current.MainWindow.Close();
            //UserControl o = e as UserControl;
            //(ScripInsertUC. as StackPanel).Children.Remove(o);
            // Window oentryusercontrol = e as Window;
            //oentryusercontrol.
        }
        //public static void  UpdateOrderUC(ScripDetails objScripDetails)
        //  {
        //      try
        //      {
        //          if (objScripDetails != null)
        //          {
        //              if (objScripDetails.Segment_Market == "0")
        //              {
        //                  Selected_EXCH = Enumerations.Segment.Equity.ToString();

        //              }

        //                  if (objScripDetails.Segment_Market == "0")
        //                  {
        //                      Selected_EXCH = Enumerations.Segment.Derivative.ToString();
        //                      SelectedScripCode = objScripDetails.ScriptCode_BseToken_NseToken;
        //                      AssetSelected = MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Cast<DerivativeMasterBase>().Where(x => x.ContractTokenNum == SelectedScripCode).Select(x => x.UnderlyingAsset).ToString();
        //                      IntrTypeSelected = MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Cast<DerivativeMasterBase>().Where(x => x.ContractTokenNum == SelectedScripCode).Select(x => x.InstrumentType).ToString();
        //                      ExpDateSelected = MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Cast<DerivativeMasterBase>().Where(x => x.ContractTokenNum == SelectedScripCode).Select(x => x.ExpiryDate).ToString();
        //                      StkPriceSelected = MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Cast<DerivativeMasterBase>().Where(x => x.ContractTokenNum == SelectedScripCode).Select(x => x.StrikePrice).ToString();
        //                      InstrNameSelected = MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Cast<DerivativeMasterBase>().Where(x => x.ContractTokenNum == SelectedScripCode).Select(x => x.InstrumentName).ToString();
        //                  }
        //                  if (objScripDetails.Segment_Market == "2")
        //                  {
        //                      Selected_EXCH = Enumerations.Segment.Equity.ToString();
        //                      SelectedScripCode = objScripDetails.ScriptCode_BseToken_NseToken;
        //                      AssetSelected = MasterSharedMemory.objMasterCurrencyDictBaseBSE.Values.Cast<CurrencyMasterBase>().Where(x => x.ContractTokenNum == SelectedScripCode).Select(x => x.UnderlyingAsset).ToString();
        //                      IntrTypeSelected = MasterSharedMemory.objMasterCurrencyDictBaseBSE.Values.Cast<CurrencyMasterBase>().Where(x => x.ContractTokenNum == SelectedScripCode).Select(x => x.InstrumentType).ToString();
        //                      ExpDateSelected = MasterSharedMemory.objMasterCurrencyDictBaseBSE.Values.Cast<CurrencyMasterBase>().Where(x => x.ContractTokenNum == SelectedScripCode).Select(x => x.DisplayExpiryDate).ToString();
        //                      StkPriceSelected = MasterSharedMemory.objMasterCurrencyDictBaseBSE.Values.Cast<CurrencyMasterBase>().Where(x => x.ContractTokenNum == SelectedScripCode).Select(x => x.StrikePrice).ToString();
        //                      InstrNameSelected = MasterSharedMemory.objMasterCurrencyDictBaseBSE.Values.Cast<CurrencyMasterBase>().Where(x => x.ContractTokenNum == SelectedScripCode).Select(x => x.InstrumentName).ToString();
        //                  }
        //              }

        //      }
        //      catch (Exception ex)
        //      {
        //          ExceptionUtility.LogError(ex);

        //      }
        //  }
        #endregion


        //public event PropertyChangedEventHandler PropertyChanged;
        //private void NotifyPropertyChanged(String propertyName = "")
        //{
        //    PropertyChangedEventHandler handler = this.PropertyChanged;
        //    if (handler != null)
        //    {
        //        var e = new PropertyChangedEventArgs(propertyName);
        //        this.PropertyChanged(this, e);
        //    }

        //}
        //private void Window_Loaded_Event(object e)
        //{
        //    ScripInsertUC usrcntrls = e as ScripInsertUC;
        //    //usrcntrls.ddlScripIdEquity.Focus();
        //    //var targetWindow = Window.GetWindow(usrcntrls);
        //    //targetWindow.Focus();
        //}

    }
#if TWS


    public partial class ScripInsertUC_VM
    {



    }
#endif
}
