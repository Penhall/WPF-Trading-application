using CommonFrontEnd.Common;
using CommonFrontEnd.Global;
using CommonFrontEnd.View.Profiling;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using static CommonFrontEnd.Common.Enumerations;

namespace CommonFrontEnd.ViewModel.Profiling
{
    public partial class OrderProfilingVM : INotifyPropertyChanged
    {
        #region Properties
        public static Action<string> OnChangeOfMarketProtection;
        public static Action<string> OnChangeOfTouchlineDataonOE;
        public static Action<bool> OnChangeOf5LCheck;

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
                //PopulateRetType();

            }
        }

        private List<string> _RetType;
        public List<string> RetType
        {
            get { return _RetType; }
            set { _RetType = value; NotifyPropertyChanged("RetType"); }
        }

        private string _SelectedRetType;
        public string SelectedRetType
        {
            get { return _SelectedRetType; }
            set
            {
                _SelectedRetType = value;
                NotifyPropertyChanged("SelectedRetType");
                //onChangeOfRetType();

            }
        }



        //private List<string> _DefaultFocus;
        //public List<string> DefaultFocus
        //{
        //    get { return _DefaultFocus; }
        //    set { _DefaultFocus = value; NotifyStaticPropertyChanged("DefaultFocus"); }
        //}

        //private string _SelectedDefaultFocus;
        //public string SelectedDefaultFocus
        //{
        //    get { return _SelectedDefaultFocus; }
        //    set
        //    {
        //        _SelectedDefaultFocus = value;
        //        NotifyStaticPropertyChanged("SelectedDefaultFocus");

        //    }
        //}
        private List<string> _DefOrderEntry;
        public List<string> DefOrderEntry
        {
            get { return _DefOrderEntry; }
            set { _DefOrderEntry = value; NotifyPropertyChanged("DefOrderEntry"); }
        }

        private string _SelectedOrderEntry;
        public string SelectedOrderEntry
        {
            get { return _SelectedOrderEntry; }
            set
            {
                _SelectedOrderEntry = value;
                NotifyPropertyChanged("SelectedOrderEntry");
            }
        }

        private string _SelectedKeyStrokeForMultileggedOE;
        public string SelectedKeyStrokeForMultileggedOE
        {
            get { return _SelectedKeyStrokeForMultileggedOE; }
            set
            {
                _SelectedKeyStrokeForMultileggedOE = value;
                NotifyPropertyChanged("SelectedKeyStrokeForMultileggedOE");
            }
        }

        private string _SelectedKeyStrokeForSwiftOE;
        public string SelectedKeyStrokeForSwiftOE
        {
            get { return _SelectedKeyStrokeForSwiftOE; }
            set
            {
                _SelectedKeyStrokeForSwiftOE = value;
                NotifyPropertyChanged("SelectedKeyStrokeForSwiftOE");
            }
        }

        private string _SelectedTouchlineData;
        public string SelectedTouchlineData
        {
            get { return _SelectedTouchlineData; }
            set
            {
                _SelectedTouchlineData = value;
                NotifyPropertyChanged("SelectedTouchlineData");
            }
        }

        private List<string> _DefaultFocusforNormalOE;
        public List<string> DefaultFocusforNormalOE
        {
            get { return _DefaultFocusforNormalOE; }
            set { _DefaultFocusforNormalOE = value; NotifyPropertyChanged("DefaultFocusforNormalOE"); }
        }

        private string _SelectedDefaultFocusforNormalOE;

        public string SelectedDefaultFocusforNormalOE
        {
            get { return _SelectedDefaultFocusforNormalOE; }
            set { _SelectedDefaultFocusforNormalOE = value; NotifyPropertyChanged("SelectedDefaultFocusforNormalOE"); }
        }

        private List<string> _DefaultFocusForSwiftOE;

        public List<string> DefaultFocusForSwiftOE
        {
            get { return _DefaultFocusForSwiftOE; }
            set { _DefaultFocusForSwiftOE = value; NotifyPropertyChanged("DefaultFocusForSwiftOE"); }
        }

        private string _SelectedDefaultFocusForSwiftOE;

        public string SelectedDefaultFocusForSwiftOE
        {
            get { return _SelectedDefaultFocusForSwiftOE; }
            set { _SelectedDefaultFocusForSwiftOE = value; NotifyPropertyChanged("SelectedDefaultFocusForSwiftOE"); }
        }

        private List<string> _DefaultFocusforMultileggedOE;

        public List<string> DefaultFocusforMultileggedOE
        {
            get { return _DefaultFocusforMultileggedOE; }
            set { _DefaultFocusforMultileggedOE = value; NotifyPropertyChanged("DefaultFocusforMultileggedOE"); }
        }

        private string _SelectedDefaultFocusforMultileggedOE;

        public string SelectedDefaultFocusforMultileggedOE
        {
            get { return _SelectedDefaultFocusforMultileggedOE; }
            set { _SelectedDefaultFocusforMultileggedOE = value; NotifyPropertyChanged("SelectedDefaultFocusforMultileggedOE"); }
        }

        private List<string> _OrdEntrySpecificSettings;
        public List<string> OrdEntrySpecificSettings
        {
            get { return _OrdEntrySpecificSettings; }
            set { _OrdEntrySpecificSettings = value; NotifyPropertyChanged("OrdEntrySpecificSettings"); }
        }

        private string _SelectedOrdEntrySpecificSettings;
        public string SelectedOrdEntrySpecificSettings
        {
            get { return _SelectedOrdEntrySpecificSettings; }
            set
            {
                _SelectedOrdEntrySpecificSettings = value;
                NotifyPropertyChanged("SelectedOrdEntrySpecificSettings");
                OnChangeOrdEntrySpecificSettings();
            }
        }



        private string _TwsVisibility;

        public string TwsVisibility
        {
            get { return _TwsVisibility; }
            set { _TwsVisibility = value; NotifyPropertyChanged("TwsVisibility"); }
        }

        private string _BowVisibility;

        public string BowVisibility
        {
            get { return _BowVisibility; }
            set { _BowVisibility = value; NotifyPropertyChanged("BowVisibility"); }
        }

        private string _NormalOEVisibility;

        public string NormalOEVisibility
        {
            get { return _NormalOEVisibility; }
            set
            {
                _NormalOEVisibility = value; NotifyPropertyChanged("NormalOEVisibility");
                // PopulateDefaultFocusforNormalOE();
            }

        }

        private string _SwiftOEVisibility;

        public string SwiftOEVisibility
        {
            get { return _SwiftOEVisibility; }
            set
            {
                _SwiftOEVisibility = value; NotifyPropertyChanged("SwiftOEVisibility");
                // PopulateDefaultFocusforNormalOE();
            }
        }

        private string _MultiLeggedOEVisibility;

        public string MultiLeggedOEVisibility
        {
            get { return _MultiLeggedOEVisibility; }
            set
            {
                _MultiLeggedOEVisibility = value; NotifyPropertyChanged("MultiLeggedOEVisibility");
                //PopulateDefaultFocus();
            }
        }

        private string _MarketProtection;

        public string MarketProtection
        {
            get { return _MarketProtection; }
            set { _MarketProtection = UtilityOrderDetails.GETInstance.MktProtection = value; NotifyPropertyChanged("MarketProtection");  }

        }

        private string _RevQty;

        public string RevQty
        {
            get { return _RevQty; }
            set
            {
                _RevQty = value;
                if (string.IsNullOrEmpty(value))
                {
                    UtilityOrderDetails.GETInstance.RevlQtyPercentage = "100";
                }
                else
                {
                    UtilityOrderDetails.GETInstance.RevlQtyPercentage = value;
                }
                NotifyPropertyChanged("RevQty");
            }

        }

        private string _EqMaxOrderQty;

        public string EqMaxOrderQty
        {
            get { return _EqMaxOrderQty; }
            set { _EqMaxOrderQty = value; NotifyPropertyChanged("EqMaxOrderQty"); }

        }

        private string _EqMaxOrderValue;

        public string EqMaxOrderValue
        {
            get { return _EqMaxOrderValue; }
            set { _EqMaxOrderValue = value; NotifyPropertyChanged("EqMaxOrderValue"); }

        }

        private string _EqMinOrderQty;

        public string EqMinOrderQty
        {
            get { return _EqMinOrderQty; }
            set { _EqMinOrderQty = value; NotifyPropertyChanged("EqMinOrderQty"); }
        }

        private string _DerMaxOrderQty;

        public string DerMaxOrderQty
        {
            get { return _DerMaxOrderQty; }
            set { _DerMaxOrderQty = value; NotifyPropertyChanged("DerMaxOrderQty"); }

        }

        private string _DerMaxOrderValue;

        public string DerMaxOrderValue
        {
            get { return _DerMaxOrderValue; }
            set { _DerMaxOrderValue = value; NotifyPropertyChanged("DerMaxOrderValue"); }

        }

        private string _DerMinOrderQty;

        public string DerMinOrderQty
        {
            get { return _DerMinOrderQty; }
            set { _DerMinOrderQty = value; NotifyPropertyChanged("DerMinOrderQty"); }
        }

        private string _CurrencyMaxOrderQty;

        public string CurrencyMaxOrderQty
        {
            get { return _CurrencyMaxOrderQty; }
            set { _CurrencyMaxOrderQty = value; NotifyPropertyChanged("CurrencyMaxOrderQty"); }

        }

        private string _CurrencyMaxOrderValue;

        public string CurrencyMaxOrderValue
        {
            get { return _CurrencyMaxOrderValue; }
            set { _CurrencyMaxOrderValue = value; NotifyPropertyChanged("CurrencyMaxOrderValue"); }

        }

        private string _CurrencyMinOrderQty;

        public string CurrencyMinOrderQty
        {
            get { return _CurrencyMinOrderQty; }
            set { _CurrencyMinOrderQty = value; NotifyPropertyChanged("CurrencyMinOrderQty"); }
        }

        private string _CommodityMaxOrderQty;

        public string CommodityMaxOrderQty
        {
            get { return _CommodityMaxOrderQty; }
            set { _CommodityMaxOrderQty = value; NotifyPropertyChanged("CommodityMaxOrderQty"); }

        }

        private string _CommodityMaxOrderValue;

        public string CommodityMaxOrderValue
        {
            get { return _CommodityMaxOrderValue; }
            set { _CommodityMaxOrderValue = value; NotifyPropertyChanged("CommodityMaxOrderValue"); }

        }

        private string _CommodityMinOrderQty;

        public string CommodityMinOrderQty
        {
            get { return _CommodityMinOrderQty; }
            set { _CommodityMinOrderQty = value; NotifyPropertyChanged("CommodityMinOrderQty"); }
        }

        private string _OtherMaxOrderQty;

        public string OtherMaxOrderQty
        {
            get { return _OtherMaxOrderQty; }
            set { _OtherMaxOrderQty = value; NotifyPropertyChanged("OtherMaxOrderQty"); }

        }

        private string _OtherMaxOrderValue;

        public string OtherMaxOrderValue
        {
            get { return _OtherMaxOrderValue; }
            set { _OtherMaxOrderValue = value; NotifyPropertyChanged("OtherMaxOrderValue"); }

        }

        private string _OtherMinOrderQty;

        public string OtherMinOrderQty
        {
            get { return _OtherMinOrderQty; }
            set { _OtherMinOrderQty = value; NotifyPropertyChanged("OtherMinOrderQty"); }
        }


        private string _Defaultprice;

        public string Defaultprice
        {
            get { return _Defaultprice; }
            set { _Defaultprice = value; NotifyPropertyChanged("Defaultprice"); }
        }



        private string _CtrEnable;

        public string CtrEnable
        {
            get { return _CtrEnable; }
            set
            {
                _CtrEnable = value; NotifyPropertyChanged("CtrEnable");
                if (true)
                {
                    WarningEnabled = true;
                    NotAllowedEnabled = true;
                    AllowedEnabled = true;
                }


            }
        }



        private List<string> _KeyStrokeForMultileggedOE;

        public List<string> KeyStrokeForMultileggedOE
        {
            get { return _KeyStrokeForMultileggedOE; }
            set { _KeyStrokeForMultileggedOE = value; NotifyPropertyChanged("KeyStrokeForMultileggedOE"); }
        }


        private string _BtnModifyEnable;

        public string BtnModifyEnable
        {
            get { return _BtnModifyEnable; }
            set { _BtnModifyEnable = value; NotifyPropertyChanged("BtnModifyEnable"); }
        }


        private List<string> _KeyStrokeForSwiftOE;

        public List<string> KeyStrokeForSwiftOE
        {
            get { return _KeyStrokeForSwiftOE; }
            set { _KeyStrokeForSwiftOE = value; NotifyPropertyChanged("KeyStrokeForSwiftOE"); }
        }

        private List<string> _TouchlineData;

        public List<string> TouchlineData
        {
            get { return _TouchlineData; }
            set { _TouchlineData = value; NotifyPropertyChanged("TouchlineData"); }
        }

        private string _OrdEntrySpecificSettingsEnabled;

        public string OrdEntrySpecificSettingsEnabled
        {
            get { return _OrdEntrySpecificSettingsEnabled; }
            set { _OrdEntrySpecificSettingsEnabled = value; NotifyPropertyChanged("OrdEntrySpecificSettingsEnabled"); }
        }


        private string _BtnCancelEnable;

        public string BtnCancelEnable
        {
            get { return _BtnCancelEnable; }
            set { _BtnCancelEnable = value; NotifyPropertyChanged(nameof(BtnCancelEnable)); }
        }


        private string _BtnSaveEnable;

        public string BtnSaveEnable
        {
            get { return _BtnSaveEnable; }
            set { _BtnSaveEnable = value; NotifyPropertyChanged(nameof(BtnSaveEnable)); }
        }

        private string _DefaultPrice;

        public string DefaultPrice
        {
            get { return _DefaultPrice; }
            set { _DefaultPrice = value; NotifyPropertyChanged("DefaultPrice"); }
        }


        private bool _WarningChecked;

        public bool WarningChecked
        {
            get { return _WarningChecked; }
            set
            {
                _WarningChecked = value;
                NotifyPropertyChanged("WarningChecked");
                // OnSelectionChangedofWarning();
                //if (true)
                //{
                //    //NotAllowedChecked = false;
                //    NotAllowedChecked = false;
                //}



            }
        }

        private bool _WarningEnabled;

        public bool WarningEnabled
        {
            get { return _WarningEnabled; }
            set
            {
                _WarningEnabled = value;
                NotifyPropertyChanged("WarningEnabled");



            }
        }

        private bool _NotAllowedChecked;

        public bool NotAllowedChecked
        {
            get { return _NotAllowedChecked; }
            set
            {
                _NotAllowedChecked = value;

                NotifyPropertyChanged("NotAllowedChecked");
                // OnSelectionChangedofWarning();
                //if (true)
                //{
                //    WarningChecked = false;

                //}
            }
        }

        private bool _NotAllowedEnabled;

        public bool NotAllowedEnabled
        {
            get { return _NotAllowedEnabled; }
            set
            {
                _NotAllowedEnabled = value;
                NotifyPropertyChanged("NotAllowedEnabled");
            }
        }

        private bool _AllowedChecked;

        public bool AllowedChecked
        {
            get { return _AllowedChecked; }
            set
            {
                _AllowedChecked = value;

                NotifyPropertyChanged("AllowedChecked");
                // OnSelectionChangedofWarning();
                //if (true)
                //{
                //    WarningChecked = false;

                //}
            }
        }

        private bool _AllowedEnabled;

        public bool AllowedEnabled
        {
            get { return _AllowedEnabled; }
            set
            {
                _AllowedEnabled = value;
                NotifyPropertyChanged("AllowedEnabled");
            }
        }

        private bool _LSeries1Checked;

        public bool LSeries1Checked
        {
            get { return _LSeries1Checked; }
            set
            {
                _LSeries1Checked = value;
                NotifyPropertyChanged("LSeries1Checked");
            }
        }




        private bool _T2TGroup1Checked;

        public bool T2TGroup1Checked
        {
            get { return _T2TGroup1Checked; }
            set
            {
                _T2TGroup1Checked = value;
                NotifyPropertyChanged("T2TGroup1Checked");
            }
        }



        private bool _PGroup1Checked;

        public bool PGroup1Checked
        {
            get { return _PGroup1Checked; }
            set
            {
                _PGroup1Checked = value;
                NotifyPropertyChanged("PGroup1Checked");
            }
        }



        private bool _FandG1Checked;

        public bool FandG1Checked
        {
            get { return _FandG1Checked; }
            set
            {
                _FandG1Checked = value;
                NotifyPropertyChanged("FandG1Checked");
            }
        }

        private bool _Suspended1Checked;

        public bool Suspended1Checked
        {
            get { return _Suspended1Checked; }
            set
            {
                _Suspended1Checked = value;
                NotifyPropertyChanged("Suspended1Checked");
            }
        }


        private bool _OtherChecked;

        public bool OtherChecked
        {
            get { return _OtherChecked; }
            set
            {
                _OtherChecked = value;
                NotifyPropertyChanged("OtherChecked");
            }
        }



        private bool _L1SeriesChecked;

        public bool L1SeriesChecked
        {
            get { return _L1SeriesChecked; }
            set
            {
                _L1SeriesChecked = value;
                NotifyPropertyChanged("L1SeriesChecked");
            }
        }


        private bool _T2TGroupChecked;

        public bool T2TGroupChecked
        {
            get { return _T2TGroupChecked; }
            set
            {
                _T2TGroupChecked = value;
                NotifyPropertyChanged("T2TGroupChecked");
            }
        }

        private bool _LSeriesChecked;

        public bool LSeriesChecked
        {
            get { return _LSeriesChecked; }
            set
            {
                _LSeriesChecked = value;
                NotifyPropertyChanged("LSeriesChecked");
            }
        }


        private bool _PGroupChecked;

        public bool PGroupChecked
        {
            get { return _PGroupChecked; }
            set
            {
                _PGroupChecked = value;
                NotifyPropertyChanged("PGroupChecked");
            }
        }


        private bool _FandGChecked;

        public bool FandGChecked
        {
            get { return _FandGChecked; }
            set
            {
                _FandGChecked = value;
                NotifyPropertyChanged("FandGChecked");
            }
        }

        private bool _SuspendedChecked;

        public bool SuspendedChecked
        {
            get { return _SuspendedChecked; }
            set
            {
                _SuspendedChecked = value;
                NotifyPropertyChanged("SuspendedChecked");
            }
        }



        private bool _GSMChecked;

        public bool GSMChecked
        {
            get { return _GSMChecked; }
            set
            {
                _GSMChecked = value;
                NotifyPropertyChanged("GSMChecked");
            }
        }



        private bool _SSandSTChecked;
        public bool SSandSTChecked
        {
            get { return _SSandSTChecked; }
            set
            {
                _SSandSTChecked = value;
                NotifyPropertyChanged("SSandSTChecked");
            }
        }



        //private bool _KeyStrokeForMultilegChecked;

        //public bool KeyStrokeForMultilegChecked
        //{
        //    get { return _KeyStrokeForMultilegChecked; }
        //    set
        //    {
        //        _KeyStrokeForMultilegChecked = value;
        //        NotifyPropertyChanged("KeyStrokeForMultilegChecked");
        //    }
        //}

        //private static bool _Default5LEnabled;

        //public static bool Default5LEnabled
        //{
        //    get { return _Default5LEnabled; }
        //    set
        //    {
        //        _Default5LEnabled = value;
        //        NotifyStaticPropertyChanged("Default5LEnabled");
        //    }
        //}

        private bool _Default5LChecked;

        public bool Default5LChecked
        {
            get { return _Default5LChecked; }
            set
            {
                _Default5LChecked = value;
                NotifyPropertyChanged("Default5LChecked");
            }
        }






        #endregion

        #region RelayCommands


        private RelayCommand _btnSave_Click;

        public RelayCommand btnSave_Click
        {
            get
            {
                return _btnSave_Click ?? (_btnSave_Click = new RelayCommand(
                    (object e) => SaveChanges(e)));



            }

        }

        private RelayCommand _btnModify_Click;

        public RelayCommand btnModify_Click
        {
            get
            {
                return _btnModify_Click ?? (_btnModify_Click = new RelayCommand(
                    (object e) => ModifyChanges(e)));

                // OrdEntrySpecificSettingsEnabled = Boolean.TrueString();
            }

        }


        private RelayCommand _btnCancel_Click;

        public RelayCommand btnCancel_Click
        {
            get
            {
                return _btnCancel_Click ?? (_btnCancel_Click = new RelayCommand(
                    (object e) => CancelChanges(e)));

            }

        }



        private RelayCommand _TxtRevQty_TextChanged;

        public RelayCommand TxtRevQty_TextChanged
        {
            get
            {
                return _TxtRevQty_TextChanged ?? (_TxtRevQty_TextChanged = new RelayCommand((object e) => TxtRevQty_TextChangedevent(e)));
            }

        }


        private RelayCommand _TxtEqMaxOrderQty_TextChanged;

        public RelayCommand TxtEqMaxOrderQty_TextChanged
        {
            get
            {
                return _TxtEqMaxOrderQty_TextChanged ?? (_TxtEqMaxOrderQty_TextChanged = new RelayCommand((object e) => TxtEqMaxOrderQty_TextChangedevent(e)));
            }

        }



        private RelayCommand _TxtEqMaxOrderValue_TextChanged;

        public RelayCommand TxtEqMaxOrderValue_TextChanged
        {
            get
            {
                return _TxtEqMaxOrderValue_TextChanged ?? (_TxtEqMaxOrderValue_TextChanged = new RelayCommand((object e) => TxtEqMaxOrderValue_TextChangedevent(e)));
            }

        }



        private RelayCommand _TxtEqMinOrderQty_TextChanged;

        public RelayCommand TxtEqMinOrderQty_TextChanged
        {
            get
            {
                return _TxtEqMinOrderQty_TextChanged ?? (_TxtEqMinOrderQty_TextChanged = new RelayCommand((object e) => TxtEqMinOrderQty_TextChangedevent(e)));
            }

        }



        private RelayCommand _TxtDerMaxOrderQty_TextChanged;

        public RelayCommand TxtDerMaxOrderQty_TextChanged
        {
            get
            {
                return _TxtDerMaxOrderQty_TextChanged ?? (_TxtDerMaxOrderQty_TextChanged = new RelayCommand((object e) => TxtDerMaxOrderQty_TextChangedevent(e)));
            }

        }



        private RelayCommand _TxtDerMaxOrderValue_TextChanged;

        public RelayCommand TxtDerMaxOrderValue_TextChanged
        {
            get
            {
                return _TxtDerMaxOrderValue_TextChanged ?? (_TxtDerMaxOrderValue_TextChanged = new RelayCommand((object e) => TxtDerMaxOrderValue_TextChangedevent(e)));
            }

        }



        private RelayCommand _TxtDerMinOrderQty_TextChanged;

        public RelayCommand TxtDerMinOrderQty_TextChanged
        {
            get
            {
                return _TxtDerMinOrderQty_TextChanged ?? (_TxtDerMinOrderQty_TextChanged = new RelayCommand((object e) => TxtDerMinOrderQty_TextChangedevent(e)));
            }

        }



        private RelayCommand _TxtCurrencyMaxOrderQty_TextChanged;

        public RelayCommand TxtCurrencyMaxOrderQty_TextChanged
        {
            get
            {
                return _TxtCurrencyMaxOrderQty_TextChanged ?? (_TxtCurrencyMaxOrderQty_TextChanged = new RelayCommand((object e) => TxtCurrencyMaxOrderQty_TextChangedevent(e)));
            }

        }



        private RelayCommand _TxtCurrencyMaxOrderValue_TextChanged;

        public RelayCommand TxtCurrencyMaxOrderValue_TextChanged
        {
            get
            {
                return _TxtCurrencyMaxOrderValue_TextChanged ?? (_TxtCurrencyMaxOrderValue_TextChanged = new RelayCommand((object e) => TxtCurrencyMaxOrderValue_TextChangedevent(e)));
            }

        }



        private RelayCommand _TxtCurrencyMinOrderQty_TextChanged;

        public RelayCommand TxtCurrencyMinOrderQty_TextChanged
        {
            get
            {
                return _TxtCurrencyMinOrderQty_TextChanged ?? (_TxtCurrencyMinOrderQty_TextChanged = new RelayCommand((object e) => TxtCurrencyMinOrderQty_TextChangedevent(e)));
            }

        }



        private RelayCommand _TxtCommodityMaxOrderQty_TextChanged;

        public RelayCommand TxtCommodityMaxOrderQty_TextChanged
        {
            get
            {
                return _TxtCommodityMaxOrderQty_TextChanged ?? (_TxtCommodityMaxOrderQty_TextChanged = new RelayCommand((object e) => TxtCommodityMaxOrderQty_TextChangedevent(e)));
            }

        }



        private RelayCommand _TxtCommodityMaxOrderValue_TextChanged;

        public RelayCommand TxtCommodityMaxOrderValue_TextChanged
        {
            get
            {
                return _TxtCommodityMaxOrderValue_TextChanged ?? (_TxtCommodityMaxOrderValue_TextChanged = new RelayCommand((object e) => TxtCommodityMaxOrderValue_TextChangedevent(e)));
            }

        }


        private RelayCommand _TxtCommodityMinOrderQty_TextChanged;

        public RelayCommand TxtCommodityMinOrderQty_TextChanged
        {
            get
            {
                return _TxtCommodityMinOrderQty_TextChanged ?? (_TxtCommodityMinOrderQty_TextChanged = new RelayCommand((object e) => TxtCommodityMinOrderQty_TextChangedevent(e)));
            }

        }


        private RelayCommand _TxtOtherMaxOrderQty_TextChanged;

        public RelayCommand TxtOtherMaxOrderQty_TextChanged
        {
            get
            {
                return _TxtOtherMaxOrderQty_TextChanged ?? (_TxtOtherMaxOrderQty_TextChanged = new RelayCommand((object e) => TxtOtherMaxOrderQty_TextChangedevent(e)));
            }

        }



        private RelayCommand _TxtOtherMaxOrderValue_TextChanged;

        public RelayCommand TxtOtherMaxOrderValue_TextChanged
        {
            get
            {
                return _TxtOtherMaxOrderValue_TextChanged ?? (_TxtOtherMaxOrderValue_TextChanged = new RelayCommand((object e) => TxtOtherMaxOrderValue_TextChangedevent(e)));
            }

        }



        private RelayCommand _TxtOtherMinOrderQty_TextChanged;

        public RelayCommand TxtOtherMinOrderQty_TextChanged
        {
            get
            {
                return _TxtOtherMinOrderQty_TextChanged ?? (_TxtOtherMinOrderQty_TextChanged = new RelayCommand((object e) => TxtOtherMinOrderQty_TextChangedevent(e)));
            }

        }


        private RelayCommand _TxtMarketProtection_TextChanged;

        public RelayCommand TxtMarketProtection_TextChanged
        {
            get
            {
                return _TxtMarketProtection_TextChanged ?? (_TxtMarketProtection_TextChanged = new RelayCommand((object e) => TxtMarketProtection_TextChangedevent(e)));
            }

        }


        private RelayCommand _TxtDefaultPrice_TextChanged;

        public RelayCommand TxtDefaultPrice_TextChanged
        {
            get
            {
                return _TxtDefaultPrice_TextChanged ?? (_TxtDefaultPrice_TextChanged = new RelayCommand((object e) => TxtDefaultPrice_TextChangedevent(e)));
            }

        }

        public string Validate_Message { get; private set; }



        //private RelayCommand _orderprofilingloaded;

        //public RelayCommand orderprofilingloaded
        //{
        //    get
        //    {
        //        return _orderprofilingloaded ?? (_orderprofilingloaded = new RelayCommand((object e) => OrderProfilingLoaded_Load(e)));

        //    }

        //}




        #endregion





        //private static void onChangeOfRetType()
        //{
        //    MainWindowVM.parserOS.AddSetting("Order Setting", "SelectedRetType", SelectedRetType);
        //    MainWindowVM.parserOS.SaveSettings(MainWindowVM.OrderSettingsINIPath.ToString());
        //}


        #region Constructor

        public OrderProfilingVM()
        {



            Visibility();
            Default5LChecked = false;
            PopulatingExchangeDropDown();
            PopulateRetType();
            PopulateDefOrderEntry();
            TouchlineDataFormat();
            PopulateOrdEntrySpecificSettings();
            PopulateDefaultFocusforNormalOE();
            PopulateDefaultFocusForSwiftOE();
            PopulateDefaultFocusForMultileggedOE();
            PopulateKeyStrokeForSwiftOE();
            PopulateKeyStrokeForMultileggedOE();
            OrderProfilingLoaded_Load();
            //AssignUserInputs();


        }

        #endregion



        #region Methods

        //private void BindOrderSettingsData()
        //{
        //  //  IniParser parser1 = new IniParser(MainWindowVM.OrderSettingsINIPath.ToString());
        //   //lstExchange = parser1.GetSetting("")
        //    RevQty = MainWindowVM.parser1.GetSetting("RSC", "RevQty");

        //}

        //EquityIP2Prod = parser1.GetSetting("RSC", "EQTHOST2");
        //EquityPort2Prod = parser1.GetSetting("RSC", "EQTPORT2");
        //EquityIP3Prod = parser1.GetSetting("RSC", "EQTHOST3");
        //EquityPort3Prod = parser1.GetSetting("RSC", "EQTPORT3");
        //EquityIP4Prod = parser1.GetSetting("RSC", "EQTHOST4");
        //EquityPort4Prod = parser1.GetSetting("RSC", "EQTPORT4");
        //EquityIP1BCastProd = parser1.GetSetting("RSC", "EQTMultiCastAdd");
        //EquityPort1BCastProd = parser1.GetSetting("RSC", "EQTUDPPort");
        //EquityIP2BCastProd = parser1.GetSetting("RSC", "EQTMultiCastAdd2");
        //EquityPort2BCastProd = parser1.GetSetting("RSC", "EQTUDPPort2");

        private void Visibility()
        {
#if TWS
            TwsVisibility = "Visible";
            BowVisibility = "Hidden";
            BtnModifyEnable = Boolean.TrueString;
            BtnSaveEnable = Boolean.FalseString;
            BtnCancelEnable = Boolean.FalseString;
            CtrEnable = Boolean.FalseString;
            OrdEntrySpecificSettingsEnabled = Boolean.FalseString;

            // TWSNEWVisibility = "Hidden";

#elif BOW
            BowVisibility = "Visible";
            TwsVisibility = "Hidden";
            BtnModifyEnable = Boolean.TrueString;
            BtnSaveEnable = Boolean.FalseString;
            BtnCancelEnable = Boolean.FalseString;
            CtrEnable = Boolean.FalseString;
            OrdEntrySpecificSettingsEnabled = Boolean.FalseString;

            // BOWNEWVisibility = "Hidden";

#endif
        }

        //   private void CheckInputValidations()



        private void SaveChanges(object e)
        {
            //if (!String.IsNullOrEmpty(RevQty))
            //{
            //    int i;
            //    if (int.TryParse(RevQty, out i))
            //    {
            //        if (!(i >= 10 && i <= 100))
            //        {
            //            //BtnSaveEnable = Boolean.TrueString;
            //            MessageBox.Show("Invalid Revealed Quantity %.It should have value in between 10.00 % to 100.00 %");
            //            BtnSaveEnable = Boolean.TrueString;
            //            return;
            //            // return;
            //        }
            //        //else
            //        //{

            //        //}
            //    }
            //}

            //if (!String.IsNullOrEmpty(MarketProtection))
            //{

            //   // if (!Regex.IsMatch(MarketProtection, @"^[0-9]+\.?[0-9]{1}$") )
            //       // (?<=^| )\d + (\.\d +)?(?=$| )| (?<=^| )\.\d + (?=$| )
            //    if (!Regex.IsMatch(MarketProtection, @" ?<=^| )\d + (\.\d +)?(?=$| )| (?<=^| )\.\d + (?=$|") )
            //        //(?<=^| )\d + (\.\d +)?(?=$| )| (?<=^| )\.\d + (?=$| )
            //    {
            //        // BtnSaveEnable = Boolean.TrueString;
            //        MessageBox.Show("MProt % more than 1 decimal places!","!Warning!", MessageBoxButton.OK, MessageBoxImage.Error);
            //        BtnSaveEnable = Boolean.TrueString;
            //        return;
            //    }

            ////else if(i >= 1 && i <= 99)
            ////    {

            ////    }
            //    else
            //    {
            //        int i;
            //        if (int.TryParse(MarketProtection, out i))
            //        {
            //            if (!(i >= 1 && i <= 99))
            //            {
            //                //BtnSaveEnable = Boolean.TrueString;
            //                MessageBox.Show("MProt % can not be greater than 99.00 %", "!Warning!", MessageBoxButton.OK, MessageBoxImage.Error);
            //                BtnSaveEnable = Boolean.TrueString;
            //                return;
            //                // return;
            //            }

            //        }
            //    }
            //}

            //else
            //{
            //    BtnSaveEnable = Boolean.FalseString;
            //    // return true;
            //}

            CheckUserInputs();

            //onChangeOfRetType();
        }


        public void CheckUserInputs()
        {
            if (!String.IsNullOrEmpty(RevQty))
            {
                int i;
                if (int.TryParse(RevQty, out i))
                {
                    if (!(i >= 10 && i <= 100))
                    {
                        //BtnSaveEnable = Boolean.TrueString;
                        MessageBox.Show("Invalid Revealed Quantity %.It should have value in between 10.00 % to 100.00 %", "!Warning!", MessageBoxButton.OK, MessageBoxImage.Error);
                        BtnSaveEnable = Boolean.TrueString;
                        CtrEnable = Boolean.TrueString;
                        return;
                        // return;
                    }
                    else
                    {
                        MainWindowVM.parserOS.AddSetting("GENERAL OS", "RevQty", RevQty);
                    }
                }
            }

            if (!String.IsNullOrEmpty(MarketProtection))
            {
                int i;
                if (int.TryParse(MarketProtection, out i))
                {
                    if (!(i >= 1 && i <= 99))
                    {
                        MessageBox.Show("MProt % can not be greater than 99.00 %");
                        BtnSaveEnable = Boolean.TrueString;
                        CtrEnable = Boolean.TrueString;
                        return;

                    }

                }



                //if (!Regex.IsMatch(MarketProtection, @"^[0 - 9]\d{ 0,9} (\.\d{ 1,3})?%?$"))
                //if (!Regex.IsMatch(MarketProtection, @" ?<=^| )\d + (\.\d +)?(?=$| )| (?<=^| )\.\d + (?=$|"))
                else if (!Regex.IsMatch(MarketProtection, @"^\d{1,2}(\.\d{1})?$"))
                {
                    // BtnSaveEnable = Boolean.TrueString;
                    MessageBox.Show("MProt % more than 1 decimal places!", "!Warning!", MessageBoxButton.OK, MessageBoxImage.Error);
                    BtnSaveEnable = Boolean.TrueString;
                    CtrEnable = Boolean.TrueString;
                    return;
                }



                MainWindowVM.parserOS.AddSetting("GENERAL OS", "MarketProtection", MarketProtection);

                MainWindowVM.parserOS.AddSetting("GENERAL OS", "SelectedExchange", SelectedExchange);
                MainWindowVM.parserOS.AddSetting("GENERAL OS", "SelectedRetType", SelectedRetType);
                MainWindowVM.parserOS.AddSetting("GENERAL OS", "SelectedOrderEntry", SelectedOrderEntry);
                MainWindowVM.parserOS.AddSetting("GENERAL OS", "SelectedOrdEntrySpecificSettings", SelectedOrdEntrySpecificSettings);
                MainWindowVM.parserOS.AddSetting("GENERAL OS", "WarningChecked", WarningChecked.ToString());
                MainWindowVM.parserOS.AddSetting("GENERAL OS", "NotAllowedChecked", NotAllowedChecked.ToString());
                MainWindowVM.parserOS.AddSetting("GENERAL OS", "AllowedChecked", AllowedChecked.ToString());
                MainWindowVM.parserOS.AddSetting("SINGLE ORDER LIMIT", "EqMaxOrderQty", EqMaxOrderQty);
                MainWindowVM.parserOS.AddSetting("SINGLE ORDER LIMIT", "EqMaxOrderValue", EqMaxOrderValue);
                MainWindowVM.parserOS.AddSetting("SINGLE ORDER LIMIT", "EqMinOrderQty", EqMinOrderQty);
                MainWindowVM.parserOS.AddSetting("SINGLE ORDER LIMIT", "DerMaxOrderQty", DerMaxOrderQty);
                MainWindowVM.parserOS.AddSetting("SINGLE ORDER LIMIT", "DerMaxOrderValue", DerMaxOrderValue);
                MainWindowVM.parserOS.AddSetting("SINGLE ORDER LIMIT", "DerMinOrderQty", DerMinOrderQty);
                MainWindowVM.parserOS.AddSetting("SINGLE ORDER LIMIT", "CurrencyMaxOrderQty", CurrencyMaxOrderQty);
                MainWindowVM.parserOS.AddSetting("SINGLE ORDER LIMIT", "CurrencyMaxOrderValue", CurrencyMaxOrderValue);
                MainWindowVM.parserOS.AddSetting("SINGLE ORDER LIMIT", "CurrencyMinOrderQty", CurrencyMinOrderQty);
                MainWindowVM.parserOS.AddSetting("SINGLE ORDER LIMIT", "CommodityMaxOrderQty", CommodityMaxOrderQty);
                MainWindowVM.parserOS.AddSetting("SINGLE ORDER LIMIT", "CommodityMaxOrderValue", CommodityMaxOrderValue);
                MainWindowVM.parserOS.AddSetting("SINGLE ORDER LIMIT", "CommodityMinOrderQty", CommodityMinOrderQty);
                MainWindowVM.parserOS.AddSetting("SINGLE ORDER LIMIT", "OtherMaxOrderQty", OtherMaxOrderQty);
                MainWindowVM.parserOS.AddSetting("SINGLE ORDER LIMIT", "OtherMaxOrderValue", OtherMaxOrderValue);
                MainWindowVM.parserOS.AddSetting("SINGLE ORDER LIMIT", "OtherMinOrderQty", OtherMinOrderQty);

                MainWindowVM.parserOS.AddSetting("PROMPT MESSAGES ON", "LSeries1Checked", LSeries1Checked.ToString());
                MainWindowVM.parserOS.AddSetting("PROMPT MESSAGES ON", "T2TGroup1Checked", T2TGroup1Checked.ToString());
                MainWindowVM.parserOS.AddSetting("PROMPT MESSAGES ON", "PGroup1Checked", PGroup1Checked.ToString());
                MainWindowVM.parserOS.AddSetting("PROMPT MESSAGES ON", "FandG1Checked", FandG1Checked.ToString());
                MainWindowVM.parserOS.AddSetting("PROMPT MESSAGES ON", "Suspended1Checked", Suspended1Checked.ToString());
                MainWindowVM.parserOS.AddSetting("PROMPT MESSAGES ON", "OtherChecked", OtherChecked.ToString());
                MainWindowVM.parserOS.AddSetting("SELF BLOCKED SCRIPS OR GROUPS", "T2TGroupChecked", T2TGroupChecked.ToString());
                MainWindowVM.parserOS.AddSetting("SELF BLOCKED SCRIPS OR GROUPS", "FandGChecked", FandGChecked.ToString());
                MainWindowVM.parserOS.AddSetting("SELF BLOCKED SCRIPS OR GROUPS", "SuspendedChecked", SuspendedChecked.ToString());
                MainWindowVM.parserOS.AddSetting("SELF BLOCKED SCRIPS OR GROUPS", "GSMChecked", GSMChecked.ToString());
                MainWindowVM.parserOS.AddSetting("SELF BLOCKED SCRIPS OR GROUPS", "SSandSTChecked", SSandSTChecked.ToString());
                MainWindowVM.parserOS.AddSetting("SELF BLOCKED SCRIPS OR GROUPS", "PGroupChecked", PGroupChecked.ToString());
                MainWindowVM.parserOS.AddSetting("SELF BLOCKED SCRIPS OR GROUPS", "LSeriesChecked", LSeriesChecked.ToString());
                MainWindowVM.parserOS.AddSetting("NORMAL OS", "Default5LChecked", Default5LChecked.ToString());
                MainWindowVM.parserOS.AddSetting("NORMAL OS", "TouchlineDataFormat", SelectedTouchlineData?.ToString());
                MainWindowVM.parserOS.AddSetting("NORMAL OS", "SelectedDefaultFocusforNormalOE", SelectedDefaultFocusforNormalOE);
                MainWindowVM.parserOS.AddSetting("NORMAL OS", "DefaultPrice", DefaultPrice);
                MainWindowVM.parserOS.AddSetting("SWIFT OS", "SelectedDefaultFocusForSwiftOE", SelectedDefaultFocusForSwiftOE);
                MainWindowVM.parserOS.AddSetting("MULTILEGGED OS", "SelectedDefaultFocusforMultileggedOE", SelectedDefaultFocusforMultileggedOE);
                MainWindowVM.parserOS.AddSetting("MULTILEGGED OS", "SelectedKeyStrokeForMultileggedOE", SelectedKeyStrokeForMultileggedOE);
                MainWindowVM.parserOS.AddSetting("SWIFT OS", "SelectedKeyStrokeForSwiftOE", SelectedKeyStrokeForSwiftOE);
                MainWindowVM.parserOS.SaveSettings(MainWindowVM.OrderSettingsINIPath.ToString());
                BtnSaveEnable = Boolean.FalseString;
                BtnModifyEnable = Boolean.TrueString;
                CtrEnable = Boolean.FalseString;
                BtnCancelEnable = Boolean.TrueString;
                //BtnSaveEnable = Boolean.FalseString;
                OrdEntrySpecificSettingsEnabled = Boolean.FalseString;
                MessageBox.Show("Order Entry Profile Setting Applied Successfully.", "Info", MessageBoxButton.OK);
                UtilityOrderDetails.GETInstance.EQTYMaxQty = EqMaxOrderQty;
                UtilityOrderDetails.GETInstance.EQTYMinQty = EqMinOrderQty;
                UtilityOrderDetails.GETInstance.EQTYMaxOrderValue = EqMaxOrderValue;

                UtilityOrderDetails.GETInstance.DERVMaxQty = DerMaxOrderQty;
                UtilityOrderDetails.GETInstance.DERVMinQty = DerMinOrderQty;
                UtilityOrderDetails.GETInstance.DERVMaxOrderValue = DerMaxOrderValue;

                UtilityOrderDetails.GETInstance.CURRMaxQty = CurrencyMaxOrderQty;
                UtilityOrderDetails.GETInstance.CURRMinQty = CurrencyMinOrderQty;
                UtilityOrderDetails.GETInstance.CURRMaxOrderValue = CurrencyMaxOrderValue;
                if (WarningChecked) {
                    UtilityOrderDetails.GETInstance.clientIdAllowed = 'W';
                }
                if (NotAllowedChecked) { UtilityOrderDetails.GETInstance.clientIdAllowed = 'N'; }
                if (AllowedChecked) { UtilityOrderDetails.GETInstance.clientIdAllowed = 'A'; }
                //UtilityOrderDetails.GETInstance.

                CommonFrontEnd.Global.UtilityOrderDetails.GETInstance.SelectedTouchlineData = SelectedTouchlineData;
                CommonFrontEnd.Global.UtilityOrderDetails.GETInstance.Default5LChecked = Default5LChecked;

                OnChangeOfMarketProtection?.Invoke(MarketProtection);
                OnChangeOfTouchlineDataonOE?.Invoke(SelectedTouchlineData);
                OnChangeOf5LCheck?.Invoke(Default5LChecked);
            }
        }
        private void ModifyChanges(object e)
        {
            BtnModifyEnable = Boolean.FalseString;
            BtnSaveEnable = Boolean.TrueString;
            BtnCancelEnable = Boolean.TrueString;
            OrdEntrySpecificSettingsEnabled = Boolean.TrueString;

            if (CtrEnable == Boolean.TrueString)
            {
                CtrEnable = Boolean.FalseString;
            }
            else { CtrEnable = Boolean.TrueString; }
        }



        private void CancelChanges(object e)
        {

            if (CtrEnable == Boolean.TrueString)
            {
                CtrEnable = Boolean.FalseString;
                BtnModifyEnable = Boolean.TrueString;
                OrderProfilingLoaded_Load();
                BtnSaveEnable = Boolean.FalseString;
                BtnCancelEnable = Boolean.FalseString;
                OrdEntrySpecificSettingsEnabled = Boolean.FalseString;
            }

            else
            {
                CtrEnable = Boolean.FalseString;
                BtnModifyEnable = Boolean.TrueString;
                OrderProfilingLoaded_Load();
                BtnSaveEnable = Boolean.FalseString;
                BtnCancelEnable = Boolean.FalseString;
            }

        }


        private void PopulateRetType()
        {

            RetType = new List<string>();
            if (string.IsNullOrEmpty(SelectedRetType))
                SelectedRetType = (Enumerations.Order.RetType.EOS.ToString());

            {
                //    RetType.Add(Enumerations.Order.RetType.EOS.ToString());
                //    RetType.Add(Enumerations.Order.RetType.EOSTLM.ToString());
                //    RetType.Add(Enumerations.Order.RetType.EOTODY.ToString());

                //}
                //RetType.Add(Enumerations.Order.RetType.EOSTLM.ToString());
                //RetType.Add(Enumerations.Order.RetType.EOTODY.ToString());

                RetType.Add(Enumerations.Order.RetType.EOS.ToString());
                RetType.Add(Enumerations.Order.RetType.IOC.ToString());
                RetType.Add(Enumerations.Order.RetType.EOD.ToString());

            }

        }
        private void PopulatingExchangeDropDown()
        {
            lstExchange = new List<string>();
            if (string.IsNullOrEmpty(SelectedExchange))
                SelectedExchange = Model.Profiling.ScripProfilingModel.Exchanges.BSE.ToString();
            // lstExchange.Add(Model.Profiling.ScripProfilingModel.Exchanges.BSE.ToString());
#if BOW

            {
                lstExchange.Add(Model.Profiling.ScripProfilingModel.Exchanges.BSE.ToString());

                lstExchange.Add(Model.Profiling.ScripProfilingModel.Exchanges.NSE.ToString());

                lstExchange.Add(Model.Profiling.ScripProfilingModel.Exchanges.MCX.ToString());
            }
#elif TWS

            lstExchange.Add(Model.Profiling.ScripProfilingModel.Exchanges.BSE.ToString());
#endif

        }

        private void PopulateKeyStrokeForSwiftOE()
        {
            KeyStrokeForSwiftOE = new List<string>();
            if (string.IsNullOrEmpty(SelectedOrderEntry))
                SelectedKeyStrokeForSwiftOE = Enumerations.KeystrokeForOE.Enter.ToString();

            {
                KeyStrokeForSwiftOE.Add(Enumerations.KeystrokeForOE.Enter.ToString());
                KeyStrokeForSwiftOE.Add(Enumerations.KeystrokeForOE.EnterControl.ToString());
            }
        }

        private void PopulateKeyStrokeForMultileggedOE()
        {
            KeyStrokeForMultileggedOE = new List<string>();
            if (string.IsNullOrEmpty(SelectedOrderEntry))
                SelectedKeyStrokeForMultileggedOE = Enumerations.KeystrokeForOE.Enter.ToString();

            {
                KeyStrokeForMultileggedOE.Add(Enumerations.KeystrokeForOE.Enter.ToString());
                KeyStrokeForMultileggedOE.Add(Enumerations.KeystrokeForOE.EnterControl.ToString());
            }
        }

        private void PopulateDefOrderEntry()
        {
            DefOrderEntry = new List<string>();
            if (string.IsNullOrEmpty(SelectedOrderEntry))
                SelectedOrderEntry = Enumerations.DefOrderEntry.Normal.ToString();

#if TWS
            {
                DefOrderEntry.Add(Enumerations.DefOrderEntry.Fast.ToString());
                DefOrderEntry.Add(Enumerations.DefOrderEntry.Normal.ToString());
                DefOrderEntry.Add(Enumerations.DefOrderEntry.Swift.ToString());
                // DefOrderEntry.Add(Enumerations.DefOrderEntry.Multilegged.ToString());

            }
#elif BOW
            //  DefOrderEntry.Add(Enumerations.DefOrderEntry.Fast.ToString());
            {
                 DefOrderEntry.Add(Enumerations.DefOrderEntry.Normal.ToString());
                DefOrderEntry.Add(Enumerations.DefOrderEntry.Swift.ToString());
                DefOrderEntry.Add(Enumerations.DefOrderEntry.Multilegged.ToString());
               
            }
#endif


        }

        private void TouchlineDataFormat()
        {
            TouchlineData = new List<string>();
            if (string.IsNullOrEmpty(SelectedOrderEntry))
                SelectedTouchlineData = "B:[BQ@BR]//S:[SQ@SR]".ToString();

            {
                TouchlineData.Add("B:[BQ@BR]//S:[SQ@SR]".ToString());
                TouchlineData.Add("B:[BQ/BR//SR/SQ]:S".ToString());
                TouchlineData.Add("B//S:[BR//SR:BQ//SQ]".ToString());

            }
        }

        private void PopulateDefaultFocusforNormalOE()
        {
            DefaultFocusforNormalOE = new List<string>();
            if (string.IsNullOrEmpty(SelectedDefaultFocusforNormalOE))
                SelectedDefaultFocusforNormalOE = Enumerations.DefaultFocusforNormalOE.ScripId.ToString();

            //SelectedDefaultFocus = Enumerations.DefaultFocus.ScripId.ToString();
#if TWS
            {// DefOrderEntry.Add(Enumerations.DefOrderEntry.Fast.ToString());
                DefaultFocusforNormalOE.Add(Enumerations.DefaultFocusforNormalOE.ScripId.ToString());
                DefaultFocusforNormalOE.Add(Enumerations.DefaultFocusforNormalOE.ScripCode.ToString());
                DefaultFocusforNormalOE.Add(Enumerations.DefaultFocusforNormalOE.OrderQty.ToString());
                DefaultFocusforNormalOE.Add(Enumerations.DefaultFocusforNormalOE.Rate.ToString());
            }
#elif BOW
            {
                DefaultFocusforNormalOE.Add(Enumerations.DefaultFocusforNormalOE.ScripId.ToString());
                DefaultFocusforNormalOE.Add(Enumerations.DefaultFocusforNormalOE.ScripCode.ToString());
                DefaultFocusforNormalOE.Add(Enumerations.DefaultFocusforNormalOE.OrderQty.ToString());
                DefaultFocusforNormalOE.Add(Enumerations.DefaultFocusforNormalOE.Rate.ToString());
            }
#endif

        }

        private void PopulateDefaultFocusForSwiftOE()
        {
            DefaultFocusForSwiftOE = new List<string>();
            if (string.IsNullOrEmpty(SelectedDefaultFocusForSwiftOE))
                SelectedDefaultFocusForSwiftOE = Enumerations.DefaultFocusForSwiftOE.ScripId.ToString();

#if TWS
            { // DefOrderEntry.Add(Enumerations.DefOrderEntry.Fast.ToString());
                DefaultFocusForSwiftOE.Add(Enumerations.DefaultFocusForSwiftOE.ScripId.ToString());
                DefaultFocusForSwiftOE.Add(Enumerations.DefaultFocusForSwiftOE.ScripCode.ToString());
                DefaultFocusForSwiftOE.Add(Enumerations.DefaultFocusForSwiftOE.OrderQty.ToString());
                DefaultFocusForSwiftOE.Add(Enumerations.DefaultFocusForSwiftOE.Rate.ToString());
            }
#elif BOW
            { 
                DefaultFocusForSwiftOE.Add(Enumerations.DefaultFocusForSwiftOE.ScripId.ToString());
            DefaultFocusForSwiftOE.Add(Enumerations.DefaultFocusForSwiftOE.ScripCode.ToString());
            DefaultFocusForSwiftOE.Add(Enumerations.DefaultFocusForSwiftOE.OrderQty.ToString());
            DefaultFocusForSwiftOE.Add(Enumerations.DefaultFocusForSwiftOE.Rate.ToString());
            }
#endif

        }


        private void PopulateDefaultFocusForMultileggedOE()
        {
            DefaultFocusforMultileggedOE = new List<string>();
            if (string.IsNullOrEmpty(SelectedDefaultFocusforMultileggedOE))
                SelectedDefaultFocusforMultileggedOE = Enumerations.DefaultFocusForMultilegOE.Segment.ToString();

            //SelectedDefaultFocus = Enumerations.DefaultFocus.ScripId.ToString();

            // DefOrderEntry.Add(Enumerations.DefOrderEntry.Fast.ToString());
            {
                DefaultFocusforMultileggedOE.Add(DefaultFocusForMultilegOE.Segment.ToString());
                DefaultFocusforMultileggedOE.Add(DefaultFocusForMultilegOE.Legs.ToString());
                DefaultFocusforMultileggedOE.Add(DefaultFocusForMultilegOE.BuyorSell.ToString());
            }
            // DefaultFocusforMultileggedOE.Add(Enumerations.DefaultFocusForMultilegOE.Rate.ToString());


            //DefaultFocusforMultileggedOE.Add(Enumerations.DefaultFocusForMultilegOE.Segment.ToString());
            //  DefaultFocusforMultileggedOE.Add(Enumerations.DefaultFocusForMultilegOE.Legs.ToString());
            //  DefaultFocusforMultileggedOE.Add(Enumerations.DefaultFocusForMultilegOE.BuyorSell.ToString());


        }


        private void PopulateOrdEntrySpecificSettings()
        {
            OrdEntrySpecificSettings = new List<string>();
            if (string.IsNullOrEmpty(SelectedOrdEntrySpecificSettings))
                SelectedOrdEntrySpecificSettings = Enumerations.DefOrderEntry.Normal.ToString();
            //NormalOEVisibility = "Visible";
            //SwiftOEVisibility = "Hidden";
            //SelectedOrdEntrySpecificSettings = Enumerations.DefOrderEntry.Normal.ToString();
#if TWS
            // OrdEntrySpecificSettings.Add(Enumerations.DefOrderEntry.Fast.ToString());
            OrdEntrySpecificSettings.Add(Enumerations.DefOrderEntry.Normal.ToString());
            OrdEntrySpecificSettings.Add(Enumerations.DefOrderEntry.Swift.ToString());
            //  OrdEntrySpecificSettings.Add(Enumerations.DefOrderEntry.Multilegged.ToString());


            if (SelectedOrdEntrySpecificSettings == "Normal")
            {
                NormalOEVisibility = "Visible";
                SwiftOEVisibility = "Hidden";
                MultiLeggedOEVisibility = "Hidden";
            }
            else if (SelectedOrdEntrySpecificSettings == "Swift")
            {
                NormalOEVisibility = "Hidden";
                SwiftOEVisibility = "Visible";
                MultiLeggedOEVisibility = "Hidden";
            }
            //else if (SelectedOrdEntrySpecificSettings == "Multilegged")
            //{
            //    NormalOEVisibility = "Hidden";
            //    SwiftOEVisibility = "Hidden";
            //    MultiLeggedOEVisibility = "Visible";
            //}

#elif BOW
            // OrdEntrySpecificSettings.Add(Enumerations.DefOrderEntry.Fast.ToString());
            OrdEntrySpecificSettings.Add(Enumerations.DefOrderEntry.Multilegged.ToString());
            OrdEntrySpecificSettings.Add(Enumerations.DefOrderEntry.Normal.ToString());
            OrdEntrySpecificSettings.Add(Enumerations.DefOrderEntry.Swift.ToString());

            if (SelectedOrdEntrySpecificSettings == "Normal")
            {
                NormalOEVisibility = "Visible";
                SwiftOEVisibility = "Hidden";
                MultiLeggedOEVisibility = "Hidden";
            }

            else if (SelectedOrdEntrySpecificSettings == "Swift")
            {
                NormalOEVisibility = "Hidden";
                SwiftOEVisibility = "Visible";
                MultiLeggedOEVisibility = "Hidden";
            }
            else if (SelectedOrdEntrySpecificSettings == "Multilegged")
            {
                NormalOEVisibility = "Hidden";
                SwiftOEVisibility = "Hidden";
                MultiLeggedOEVisibility = "Visible";
            }
#endif

        }




        private void OrderProfilingLoaded_Load()
        {
            {
                RevQty = MainWindowVM.parserOS.GetSetting("GENERAL OS", "RevQty");
                MarketProtection = MainWindowVM.parserOS.GetSetting("GENERAL OS", "MarketProtection");
                SelectedRetType = MainWindowVM.parserOS.GetSetting("GENERAL OS", "SelectedRetType");
                SelectedExchange = MainWindowVM.parserOS.GetSetting("GENERAL OS", "SelectedExchange");
                SelectedOrderEntry = MainWindowVM.parserOS.GetSetting("GENERAL OS", "SelectedOrderEntry");
                WarningChecked = Convert.ToBoolean(MainWindowVM.parserOS.GetSetting("GENERAL OS", "WarningChecked"));
                NotAllowedChecked = Convert.ToBoolean(MainWindowVM.parserOS.GetSetting("GENERAL OS", "NotAllowedChecked"));
                AllowedChecked = Convert.ToBoolean(MainWindowVM.parserOS.GetSetting("GENERAL OS", "AllowedChecked"));
               
                if (WarningChecked == false && NotAllowedChecked == false && AllowedChecked == false)
                {
                    WarningChecked = false;
                    NotAllowedChecked = false;
                    AllowedChecked = true;
                }

                SelectedOrdEntrySpecificSettings = MainWindowVM.parserOS.GetSetting("GENERAL OS", "SelectedOrdEntrySpecificSettings");
                EqMaxOrderQty = MainWindowVM.parserOS.GetSetting("SINGLE ORDER LIMIT", "EqMaxOrderQty");
                EqMaxOrderValue = MainWindowVM.parserOS.GetSetting("SINGLE ORDER LIMIT", "EqMaxOrderValue");
                EqMinOrderQty = MainWindowVM.parserOS.GetSetting("SINGLE ORDER LIMIT", "EqMinOrderQty");
                DerMaxOrderQty = MainWindowVM.parserOS.GetSetting("SINGLE ORDER LIMIT", "DerMaxOrderQty");
                DerMaxOrderValue = MainWindowVM.parserOS.GetSetting("SINGLE ORDER LIMIT", "DerMaxOrderValue");
                DerMinOrderQty = MainWindowVM.parserOS.GetSetting("SINGLE ORDER LIMIT", "DerMinOrderQty");
                CurrencyMaxOrderQty = MainWindowVM.parserOS.GetSetting("SINGLE ORDER LIMIT", "CurrencyMaxOrderQty");
                CurrencyMaxOrderValue = MainWindowVM.parserOS.GetSetting("SINGLE ORDER LIMIT", "CurrencyMaxOrderValue");
                CurrencyMinOrderQty = MainWindowVM.parserOS.GetSetting("SINGLE ORDER LIMIT", "CurrencyMinOrderQty");
                CommodityMaxOrderQty = MainWindowVM.parserOS.GetSetting("SINGLE ORDER LIMIT", "CommodityMaxOrderQty");
                CommodityMaxOrderValue = MainWindowVM.parserOS.GetSetting("SINGLE ORDER LIMIT", "CommodityMaxOrderValue");
                CommodityMinOrderQty = MainWindowVM.parserOS.GetSetting("SINGLE ORDER LIMIT", "CommodityMinOrderQty");
                OtherMaxOrderQty = MainWindowVM.parserOS.GetSetting("SINGLE ORDER LIMIT", "OtherMaxOrderQty");
                OtherMaxOrderValue = MainWindowVM.parserOS.GetSetting("SINGLE ORDER LIMIT", "OtherMaxOrderValue");
                OtherMinOrderQty = MainWindowVM.parserOS.GetSetting("SINGLE ORDER LIMIT", "OtherMinOrderQty");
                LSeries1Checked = Convert.ToBoolean(MainWindowVM.parserOS.GetSetting("PROMPT MESSAGES ON", "LSeries1Checked"));
                T2TGroup1Checked = Convert.ToBoolean(MainWindowVM.parserOS.GetSetting("PROMPT MESSAGES ON", "T2TGroup1Checked"));
                PGroup1Checked = Convert.ToBoolean(MainWindowVM.parserOS.GetSetting("PROMPT MESSAGES ON", "PGroup1Checked"));
                FandG1Checked = Convert.ToBoolean(MainWindowVM.parserOS.GetSetting("PROMPT MESSAGES ON", "FandG1Checked"));
                Suspended1Checked = Convert.ToBoolean(MainWindowVM.parserOS.GetSetting("PROMPT MESSAGES ON", "Suspended1Checked"));
                OtherChecked = Convert.ToBoolean(MainWindowVM.parserOS.GetSetting("PROMPT MESSAGES ON", "OtherChecked"));
                FandGChecked = Convert.ToBoolean(MainWindowVM.parserOS.GetSetting("SELF BLOCKED SCRIPS OR GROUPS", "FandChecked"));
                SuspendedChecked = Convert.ToBoolean(MainWindowVM.parserOS.GetSetting("SELF BLOCKED SCRIPS OR GROUPS", "SuspendedChecked"));
                GSMChecked = Convert.ToBoolean(MainWindowVM.parserOS.GetSetting("SELF BLOCKED SCRIPS OR GROUPS", "GSMChecked"));
                SSandSTChecked = Convert.ToBoolean(MainWindowVM.parserOS.GetSetting("SELF BLOCKED SCRIPS OR GROUPS", "SSandSTChecked"));
                T2TGroupChecked = Convert.ToBoolean(MainWindowVM.parserOS.GetSetting("SELF BLOCKED SCRIPS OR GROUPS", "T2TGroupChecked"));
                PGroupChecked = Convert.ToBoolean(MainWindowVM.parserOS.GetSetting("SELF BLOCKED SCRIPS OR GROUPS", "PGroupChecked"));
                LSeriesChecked = Convert.ToBoolean(MainWindowVM.parserOS.GetSetting("SELF BLOCKED SCRIPS OR GROUPS", "LSeriesChecked"));
                Default5LChecked = Convert.ToBoolean(MainWindowVM.parserOS.GetSetting("NORMAL OS", "Default5LChecked"));
                SelectedTouchlineData = MainWindowVM.parserOS.GetSetting("NORMAL OS", "TouchlineDataFormat");
                SelectedDefaultFocusforNormalOE = MainWindowVM.parserOS.GetSetting("NORMAL OS", "SelectedDefaultFocusforNormalOE");
                DefaultPrice = MainWindowVM.parserOS.GetSetting("NORMAL OS", "DefaultPrice");
                //KeyStrokeForSwiftChecked = Convert.ToBoolean(MainWindowVM.parserOS.GetSetting("SWIFT OS", "KeyStrokeForSwiftChecked"));
                SelectedDefaultFocusForSwiftOE = MainWindowVM.parserOS.GetSetting("SWIFT OS", "SelectedDefaultFocusForSwiftOE");
                //KeyStrokeForMultilegChecked = Convert.ToBoolean(MainWindowVM.parserOS.GetSetting("MULTILEGGED OS", "KeyStrokeForMultilegChecked"));
                SelectedDefaultFocusforMultileggedOE = MainWindowVM.parserOS.GetSetting("MULTILEGGED OS", "SelectedDefaultFocusforMultileggedOE");
                SelectedKeyStrokeForMultileggedOE = MainWindowVM.parserOS.GetSetting("MULTILEGGED OS", "SelectedKeyStrokeForMultileggedOE");
                SelectedKeyStrokeForSwiftOE = MainWindowVM.parserOS.GetSetting("SWIFT OS", "SelectedKeyStrokeForSwiftOE");
            }
        }



        #endregion


        //     #endregion
        //#region Input Validation

        //private void TxtRevQty_TextChangedevent(object e)
        //{
        //    if (RevQty != null)
        //    {

        //        RevQty = Regex.Replace(RevQty.Trim(), @"^[0-9] + (\.([0-9]{1,2})?)?$", "");


        //    }


        //}

        //private void TxtRevQty_TextChangedevent(object sender, TextCompositionEventArgs e)

        //    {
        //        e.Handled = !IsValid(((RevQty.ToString())sender).Text + e.Text);
        //    }
        //     public static bool IsValid(string str)
        //{
        //    int i;
        //    return int.TryParse(str, out i) && i >= 10 && i <= 100;
        //}




        //private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        //{
        //    e.Handled = !IsValid(((TextBox)sender).Text + e.Text);
        //}

        //public static bool IsValid(string str)
        //{
        //    int i;
        //    return int.TryParse(str, out i) && i >= 5 && i <= 9999;
        //}

        private void TxtRevQty_TextChangedevent(object e)
        {
            //bool validate = ValidateRevQty();
            //if (!validate)
            //{
            //    return;
            //}

            //if (RevQty != null)
            //{
            //    int i;

            //    if(i >= 10 && i <= 100)
            //    {
            //        return;
            //    }
            //}

            // if (RevQty != null)
            //{
            //     int i;
            //     if (int.TryParse(RevQty, out i))
            //     {
            //         if (i >= 10 && i <= 100)
            //             return;
            //     }
            // }
            // MessageBox.Show("invalid input");

        }

        //private bool ValidateRevQty()
        //{
        //    if (RevQty != null)
        //    {
        //        int i;
        //        if (int.TryParse(RevQty, out i))
        //        {
        //            if (i >= 10 && i <= 100)
        //                return true;
        //        }
        //        else
        //        {
        //            MessageBox.Show("Invalid Revealed Quantity %.It should have value in between 10.00 % to 100.00 %");
        //            return false;
        //        }
        //    }

        //}

        private void TxtEqMaxOrderQty_TextChangedevent(object e)
        {
            if (EqMaxOrderQty != null)
            {
                EqMaxOrderQty = Regex.Replace(EqMaxOrderQty, "[^0-9]+", "");
            }


        }

        private void TxtEqMaxOrderValue_TextChangedevent(object e)
        {
            if (EqMaxOrderValue != null)
            {
                EqMaxOrderValue = Regex.Replace(EqMaxOrderValue, "[^0-9]+", "");
            }
        }

        private void TxtEqMinOrderQty_TextChangedevent(object e)
        {
            if (EqMinOrderQty != null)
            {
                EqMinOrderQty = Regex.Replace(EqMinOrderQty, "[^0-9]+", "");
            }
        }

        private void TxtDerMaxOrderQty_TextChangedevent(object e)
        {
            if (DerMaxOrderQty != null)
            {
                DerMaxOrderQty = Regex.Replace(DerMaxOrderQty, "[^0-9]+", "");
            }
        }

        private void TxtDerMaxOrderValue_TextChangedevent(object e)
        {
            if (DerMaxOrderValue != null)
            {
                DerMaxOrderValue = Regex.Replace(DerMaxOrderValue, "[^0-9]+", "");
            }
        }

        private void TxtDerMinOrderQty_TextChangedevent(object e)
        {
            if (DerMinOrderQty != null)
            {
                DerMinOrderQty = Regex.Replace(DerMinOrderQty, "[^0-9]+", "");
            }

        }


        private void TxtCurrencyMaxOrderQty_TextChangedevent(object e)
        {
            if (CurrencyMaxOrderQty != null)
            {
                CurrencyMaxOrderQty = Regex.Replace(CurrencyMaxOrderQty, "[^0-9]+", "");
            }
        }


        private void TxtCurrencyMaxOrderValue_TextChangedevent(object e)
        {
            if (CurrencyMaxOrderValue != null)
            {
                CurrencyMaxOrderValue = Regex.Replace(CurrencyMaxOrderValue, "[^0-9]+", "");
            }
        }


        private void TxtCurrencyMinOrderQty_TextChangedevent(object e)
        {
            if (CurrencyMinOrderQty != null)
            {
                CurrencyMinOrderQty = Regex.Replace(CurrencyMinOrderQty, "[^0-9]+", "");
            }
        }

        private void TxtCommodityMaxOrderQty_TextChangedevent(object e)
        {
            if (CommodityMaxOrderQty != null)
            {
                CommodityMaxOrderQty = Regex.Replace(CommodityMaxOrderQty, "[^0-9]+", "");
            }
        }

        private void TxtCommodityMaxOrderValue_TextChangedevent(object e)
        {
            if (CommodityMaxOrderValue != null)
            {
                CommodityMaxOrderValue = Regex.Replace(CommodityMaxOrderValue, "[^0-9]+", "");
            }
        }

        private void TxtCommodityMinOrderQty_TextChangedevent(object e)
        {
            if (CommodityMinOrderQty != null)
            {
                CommodityMinOrderQty = Regex.Replace(CommodityMinOrderQty, "[^0-9]+", "");
            }
        }

        private void TxtOtherMaxOrderQty_TextChangedevent(object e)
        {
            if (OtherMaxOrderQty != null)
            {
                OtherMaxOrderQty = Regex.Replace(OtherMaxOrderQty, "[^0-9]+", "");
            }
        }

        private void TxtOtherMaxOrderValue_TextChangedevent(object e)
        {
            if (OtherMaxOrderValue != null)
            {
                OtherMaxOrderValue = Regex.Replace(OtherMaxOrderValue, "[^0-9]+", "");
            }
        }

        private void TxtOtherMinOrderQty_TextChangedevent(object e)
        {
            if (OtherMinOrderQty != null)
            {
                OtherMinOrderQty = Regex.Replace(OtherMinOrderQty, "[^0-9]+", "");
            }
        }

        private void TxtMarketProtection_TextChangedevent(object e)
        {
            //  Validations.ValidateVolume(Convert.long(MarketProtection), 999999999L, 1, 0, false);
            if (MarketProtection != null)
            {
                // MarketProtection = Regex.Replace(MarketProtection, @"^-{0,1}\d+\.{0,1}\d*$", "");
                // MarketProtection = Regex.Replace(MarketProtection, (" \d+(\.\d{1,2})?");
                // MarketProtection = Regex.Replace(MarketProtection.Trim(), @"^[0-9] + (\.([0-9]{1,2})?)?$", "");

            }
        }

        private void TxtDefaultPrice_TextChangedevent(object e)
        {
            // Validations.ValidateVolume(123, 999999999L, 1, 0, false);
            if (DefaultPrice != null)
            {
                DefaultPrice = Regex.Replace(DefaultPrice, "[^0-9]+", "");

            }
        }






        #region NotifyPropertyChangedEvent

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

        #region StaticNotifyPropertyChangedEvent
        public static event EventHandler<PropertyChangedEventArgs> StaticPropertyChanged
                 = delegate { };
        private static void NotifyStaticPropertyChanged(string propertyName)
        {
            StaticPropertyChanged(null, new PropertyChangedEventArgs(propertyName));
        }
        #endregion


    }



    public partial class OrderProfilingVM
    {
#if TWS

        private void OnChangeOrdEntrySpecificSettings()
        {

            if (SelectedOrdEntrySpecificSettings == "Normal")
            {
                NormalOEVisibility = "Visible";
                SwiftOEVisibility = "Hidden";
                MultiLeggedOEVisibility = "Hidden";
            }
            else if (SelectedOrdEntrySpecificSettings == "Swift")
            {

                NormalOEVisibility = "Hidden";
                SwiftOEVisibility = "Visible";
                MultiLeggedOEVisibility = "Hidden";
            }
            else if (SelectedOrdEntrySpecificSettings == "Multilegged")
            {

                NormalOEVisibility = "Hidden";
                SwiftOEVisibility = "Hidden";
                MultiLeggedOEVisibility = "Visible";
            }
        }
#endif
    }



    public partial class OrderProfilingVM
    {
#if BOW
           private void OnChangeOrdEntrySpecificSettings()
        {
            
            if (SelectedOrdEntrySpecificSettings == "Normal")
            {
                NormalOEVisibility = "Visible";
                SwiftOEVisibility = "Hidden";
                MultiLeggedOEVisibility = "Hidden";
            }
            else if (SelectedOrdEntrySpecificSettings == "Swift")
            {
                
                NormalOEVisibility = "Hidden";
                SwiftOEVisibility = "Visible";
                MultiLeggedOEVisibility = "Hidden";
            }
            else if (SelectedOrdEntrySpecificSettings == "Multilegged")
            {
               
                NormalOEVisibility = "Hidden";
               SwiftOEVisibility = "Hidden";
              MultiLeggedOEVisibility = "Visible";
            }
        }

#endif
    }

}


