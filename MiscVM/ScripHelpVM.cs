using CommonFrontEnd.Common;
using CommonFrontEnd.Model;
using CommonFrontEnd.SharedMemories;
using CommonFrontEnd.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Forms;
using static CommonFrontEnd.Common.Enumerations;
using static CommonFrontEnd.SharedMemories.DataAccessLayer;

namespace CommonFrontEnd.ViewModel
{
    partial class ScripHelpVM : BaseViewModel
    {
        DirectoryInfo directory2 = new DirectoryInfo(Path.GetFullPath(Path.Combine(System.Environment.CurrentDirectory,@"User")));
        #region properties
        // public static bool flag = false;
        // public static Action<bool> flagChange;
        public static DataAccessLayer oDataAccessLayer;


        private string _LeftPosition = "71";

        public string LeftPosition
        {
            get { return _LeftPosition; }
            set { _LeftPosition = value; NotifyPropertyChanged("LeftPosition"); }
        }

        private string _TopPosition = "17";

        public string TopPosition
        {
            get { return _TopPosition; }
            set { _TopPosition = value; NotifyPropertyChanged("TopPosition"); }
        }

        private string _Width;

        public string CorpActWidth
        {
            get { return _Width; }
            set { _Width = value; NotifyPropertyChanged("CorpActWidth"); }
        }


        private string _Height = "300";

        public string CorpActHeight
        {
            get { return _Height; }
            set { _Height = value; NotifyPropertyChanged("CorpActHeight"); }
        }

        private string _selectedExchange;

        public string selectedExchange
        {
            get { return _selectedExchange; }
            set
            {
                _selectedExchange = value;
                NotifyPropertyChanged(nameof(selectedExchange));


                populateSegment();
                //if (flagChange != null)
                //{
                //    flagChange(flag = true);
                //}

            }
        }

        private static List<ScripHelpModel> _SearchEquityTemplist;
        public static List<ScripHelpModel> SearchEquityTemplist
        {
            get { return _SearchEquityTemplist; }
            set { _SearchEquityTemplist = value; }
        }

        private static List<ScripHelpBSEDerivativeCO> _SearchDerCOTemplist;
        public static List<ScripHelpBSEDerivativeCO> SearchDerCOTemplist
        {
            get { return _SearchDerCOTemplist; }
            set { _SearchDerCOTemplist = value; }
        }

        private static List<ScripHelpBSEDerivativeSPD> _SearchDerSPDTemplist;
        public static List<ScripHelpBSEDerivativeSPD> SearchDerSPDTemplist
        {
            get { return _SearchDerSPDTemplist; }
            set { _SearchDerSPDTemplist = value; }
        }

        private static List<ScripHelpBSECurrencyCO> _SearchCurCOTemplist;
        public static List<ScripHelpBSECurrencyCO> SearchCurCOTemplist
        {
            get { return _SearchCurCOTemplist; }
            set { _SearchCurCOTemplist = value; }
        }

        private static List<ScripHelpBSECurrencySPD> _SearchCurSPDTemplist;
        public static List<ScripHelpBSECurrencySPD> SearchCurSPDTemplist
        {
            get { return _SearchCurSPDTemplist; }
            set { _SearchCurSPDTemplist = value; }
        }

        private static List<ScripHelpModel> _SearchDebtTemplist;
        public static List<ScripHelpModel> SearchDebtTemplist
        {
            get { return _SearchDebtTemplist; }
            set { _SearchDebtTemplist = value; }
        }


        private static string _TitleScripHelp = "Scrip Help";

        public static string TitleScripHelp
        {
            get { return _TitleScripHelp; }
            set { _TitleScripHelp = value; NotifyStaticPropertyChanged("TitleScripHelp"); }
        }
        //TODO: Added Filter count on scrip help. Prafulla 22/05/2018 

        private string _selectedSegment;

        public string selectedSegment
        {
            get { return _selectedSegment; }
            set
            {
                _selectedSegment = value; NotifyPropertyChanged(nameof(selectedSegment));

                ControleVisibility();
                populateGridData();
                //if (flagChange != null)
                //{
                //    flagChange(flag = true);
                //}
                //populateGridData();
            }
        }



        private string _selectedCOSPD;

        public string selectedCOSPD
        {
            get { return _selectedCOSPD; }
            set
            {
                _selectedCOSPD = value; NotifyPropertyChanged(nameof(selectedCOSPD));
                //if (flagChange != null)
                //{
                //    flagChange(flag = true);
                //}
            }
        }


        private bool _cochecked = true;

        public bool cochecked
        {
            get { return _cochecked; }
            set
            {
                _cochecked = value; NotifyPropertyChanged(nameof(cochecked));
                //if (flagChange != null)
                //{
                //    flagChange(flag = true);
                //}
            }
        }

        private bool _spdchecked;

        public bool spdchecked
        {
            get { return _spdchecked; }
            set
            {
                _spdchecked = value; NotifyPropertyChanged(nameof(spdchecked));
                //if (flagChange != null)
                //{
                //    flagChange(flag = true);
                //}
            }
        }

        private string _CoSpdVisibility;

        public string CoSpdVisibility
        {
            get { return _CoSpdVisibility; }
            set { _CoSpdVisibility = value; NotifyPropertyChanged(nameof(CoSpdVisibility)); }
        }

        private string _ISINCodeVisibility;

        public string ISINCodeVisibility
        {
            get { return _ISINCodeVisibility; }
            set { _ISINCodeVisibility = value; NotifyPropertyChanged(nameof(ISINCodeVisibility)); }
        }

        private List<string> _ExchangeData;

        public List<string> ExchangeData
        {
            get { return _ExchangeData; }
            set { _ExchangeData = value; NotifyPropertyChanged(nameof(ExchangeData)); }
        }

        private ObservableCollection<string> _SegmentData;

        public ObservableCollection<string> SegmentData
        {
            get { return _SegmentData; }
            set { _SegmentData = value; NotifyPropertyChanged(nameof(SegmentData)); }
        }

        private List<string> _COSPDData;

        public List<string> COSPDData
        {
            get { return _COSPDData; }
            set { _COSPDData = value; NotifyPropertyChanged(nameof(COSPDData)); }
        }

        private string _VisibilityBSEEquityGrid;

        public string VisibilityBSEEquityGrid
        {
            get { return _VisibilityBSEEquityGrid; }
            set { _VisibilityBSEEquityGrid = value; NotifyPropertyChanged(nameof(VisibilityBSEEquityGrid)); }
        }

        private string _VisibilityBSEDerivativeCOGrid;

        public string VisibilityBSEDerivativeCOGrid
        {
            get { return _VisibilityBSEDerivativeCOGrid; }
            set { _VisibilityBSEDerivativeCOGrid = value; NotifyPropertyChanged(nameof(VisibilityBSEDerivativeCOGrid)); }
        }

        private string _VisibilityBSEDerivativeSPDGrid;

        public string VisibilityBSEDerivativeSPDGrid
        {
            get { return _VisibilityBSEDerivativeSPDGrid; }
            set { _VisibilityBSEDerivativeSPDGrid = value; NotifyPropertyChanged(nameof(VisibilityBSEDerivativeSPDGrid)); }
        }
        private string _VisibilityBSECurrencyCOGrid;

        public string VisibilityBSECurrencyCOGrid
        {
            get { return _VisibilityBSECurrencyCOGrid; }
            set { _VisibilityBSECurrencyCOGrid = value; NotifyPropertyChanged(nameof(VisibilityBSECurrencyCOGrid)); }
        }

        private string _VisibilityBSECurrencySPDGrid;

        public string VisibilityBSECurrencySPDGrid
        {
            get { return _VisibilityBSECurrencySPDGrid; }
            set { _VisibilityBSECurrencySPDGrid = value; NotifyPropertyChanged(nameof(VisibilityBSECurrencySPDGrid)); }
        }

        private string _VisibilityBSEDebtGrid;

        public string VisibilityBSEDebtGrid
        {
            get { return _VisibilityBSEDebtGrid; }
            set { _VisibilityBSEDebtGrid = value; NotifyPropertyChanged(nameof(VisibilityBSEDebtGrid)); }
        }

        private string _VisibilityNSEEquityGrid;

        public string VisibilityNSEEquityGrid
        {
            get { return _VisibilityNSEEquityGrid; }
            set { _VisibilityNSEEquityGrid = value; NotifyPropertyChanged(nameof(VisibilityNSEEquityGrid)); }
        }
        private string _VisibilityNSEDerivativeCOGrid;

        public string VisibilityNSEDerivativeCOGrid
        {
            get { return _VisibilityNSEDerivativeCOGrid; }
            set { _VisibilityNSEDerivativeCOGrid = value; NotifyPropertyChanged(nameof(VisibilityNSEDerivativeCOGrid)); }
        }


        private string _VisibilityNSECurrencyCOGrid;

        public string VisibilityNSECurrencyCOGrid
        {
            get { return _VisibilityNSECurrencyCOGrid; }
            set { _VisibilityNSECurrencyCOGrid = value; NotifyPropertyChanged(nameof(VisibilityNSECurrencyCOGrid)); }
        }

        private string _VisibilityNCDEXCommoditiesGrid;

        public string VisibilityNCDEXCommoditiesGrid
        {
            get { return _VisibilityNCDEXCommoditiesGrid; }
            set { _VisibilityNCDEXCommoditiesGrid = value; NotifyPropertyChanged(nameof(VisibilityNCDEXCommoditiesGrid)); }
        }

        private string _VisibilityMCXCommoditiesGrid;

        public string VisibilityMCXCommoditiesGrid
        {
            get { return _VisibilityMCXCommoditiesGrid; }
            set { _VisibilityMCXCommoditiesGrid = value; NotifyPropertyChanged(nameof(VisibilityMCXCommoditiesGrid)); }
        }

        private string _txtScripID = string.Empty;

        public string txtScripID
        {
            get { return _txtScripID; }
            set
            {
                _txtScripID = value; NotifyPropertyChanged("txtScripID");
                ScripTxtChange_Click();
            }
        }



        private string _txtScripCode = string.Empty;

        public string txtScripCode
        {
            get { return _txtScripCode; }
            set
            {
                _txtScripCode = value; NotifyPropertyChanged("txtScripCode");
                ScripTxtChange_Click();
            }
        }

        private string _txtISINCode = string.Empty;

        public string txtISINCode
        {
            get { return _txtISINCode; }
            set
            {
                _txtISINCode = value; NotifyPropertyChanged("txtISINCode");
                ScripTxtChange_Click();
            }
        }



        #region LocalMemory
        //ObservableCollection<ScripHelpModel> objObservableCollection = new ObservableCollection<ScripHelpModel>();
        private ObservableCollection<ScripHelpModel> _ObjEquityDataCollection = new ObservableCollection<ScripHelpModel>();

        public ObservableCollection<ScripHelpModel> ObjEquityDataCollection
        {
            get { return _ObjEquityDataCollection; }
            set { _ObjEquityDataCollection = value; /*NotifyStaticPropertyChanged(nameof(ObjIndicesCollection));*/ }
        }

        private ObservableCollection<ScripHelpBSEDerivativeCO> _ObjBSEDerivativeCOCollection = new ObservableCollection<ScripHelpBSEDerivativeCO>();

        public ObservableCollection<ScripHelpBSEDerivativeCO> ObjBSEDerivativeCOCollection
        {
            get { return _ObjBSEDerivativeCOCollection; }
            set { _ObjBSEDerivativeCOCollection = value; /*NotifyStaticPropertyChanged(nameof(ObjIndicesCollection));*/ }
        }
        private ObservableCollection<ScripHelpBSEDerivativeSPD> _ObjBSEDerivativeSPDCollection = new ObservableCollection<ScripHelpBSEDerivativeSPD>();

        public ObservableCollection<ScripHelpBSEDerivativeSPD> ObjBSEDerivativeSPDCollection
        {
            get { return _ObjBSEDerivativeSPDCollection; }
            set { _ObjBSEDerivativeSPDCollection = value; /*NotifyStaticPropertyChanged(nameof(ObjIndicesCollection));*/ }
        }

        private ObservableCollection<ScripHelpBSECurrencyCO> _ObjBSECurrencyCOCollection = new ObservableCollection<ScripHelpBSECurrencyCO>();

        public ObservableCollection<ScripHelpBSECurrencyCO> ObjBSECurrencyCOCollection
        {
            get { return _ObjBSECurrencyCOCollection; }
            set { _ObjBSECurrencyCOCollection = value; /*NotifyStaticPropertyChanged(nameof(ObjIndicesCollection));*/ }
        }
        private ObservableCollection<ScripHelpBSECurrencySPD> _ObjBSECurrencySPDCollection = new ObservableCollection<ScripHelpBSECurrencySPD>();

        public ObservableCollection<ScripHelpBSECurrencySPD> ObjBSECurrencySPDCollection
        {
            get { return _ObjBSECurrencySPDCollection; }
            set { _ObjBSECurrencySPDCollection = value; /*NotifyStaticPropertyChanged(nameof(ObjIndicesCollection));*/ }
        }

        private ObservableCollection<ScripHelpModel> _ObjDebtDataCollection = new ObservableCollection<ScripHelpModel>();

        public ObservableCollection<ScripHelpModel> ObjDebtDataCollection
        {
            get { return _ObjDebtDataCollection; }
            set { _ObjDebtDataCollection = value; /*NotifyStaticPropertyChanged(nameof(ObjIndicesCollection));*/ }
        }



        //private RelayCommand _ScripHelpClose;
        //public RelayCommand ScripHelpClose
        //{
        //    get
        //    {
        //        return _ScripHelpClose ?? (_ScripHelpClose = new RelayCommand((object e) => ScripHelpClose_Click()));
        //    }
        //}

        private RelayCommand _btnSearch;

        public RelayCommand btnSearch
        {
            get { return _btnSearch ?? (_btnSearch = new RelayCommand((object e) => populateGridData())); }
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

        private RelayCommand _btnSaveinCSV;

        public RelayCommand btnSaveinCSV
        {
            get
            {
                return _btnSaveinCSV ?? (_btnSaveinCSV = new RelayCommand(
                    (object e) => SaveinCSV(e)
                    ));
            }
        }




        //private void ScripHelpClosing_Closing(object e)
        //{
        //    Windows_ScripHelpLocationChanged(e);
        //}

        private RelayCommand _myLocationChanged;

        public RelayCommand myLocationChanged
        {
            get
            {
                return _myLocationChanged ?? (_myLocationChanged = new RelayCommand(
                    (object e) => Windows_ScripHelpLocationChanged(e)));

            }
        }


        private void Windows_ScripHelpLocationChanged(object e)
        {
            if (CommonFrontEnd.SharedMemories.ConfigurationMasterMemory.ConfigurationDict.ContainsKey("WindowsPosition"))
            {
                BoltAppSettingsWindowsPosition oBoltAppSettingsWindowsPosition = (BoltAppSettingsWindowsPosition)CommonFrontEnd.SharedMemories.ConfigurationMasterMemory.ConfigurationDict["WindowsPosition"];
                if (oBoltAppSettingsWindowsPosition != null && oBoltAppSettingsWindowsPosition.ScripHelp != null && oBoltAppSettingsWindowsPosition.ScripHelp.WNDPOSITION != null)
                {
                    oBoltAppSettingsWindowsPosition.ScripHelp.WNDPOSITION.Left = Convert.ToInt32(LeftPosition);
                    oBoltAppSettingsWindowsPosition.ScripHelp.WNDPOSITION.Top = Convert.ToInt32(TopPosition);
                    oBoltAppSettingsWindowsPosition.ScripHelp.WNDPOSITION.Right = Convert.ToInt32(CorpActWidth);
                    oBoltAppSettingsWindowsPosition.ScripHelp.WNDPOSITION.Down = Convert.ToInt32(CorpActHeight);
                }
                //CommonFrontEnd.SharedMemories.SaveConfiguration.SaveUserConfiguration(@"D:\TWS_DotNetNewStructure\TWS_DOTNETT\CommonFrontEnd\bin\Debug\xml\Users\AppSettings\920200000.xml", "WindowsPosition");
                CommonFrontEnd.SharedMemories.SaveConfiguration.SaveUserConfiguration(CommonFrontEnd.SharedMemories.SettingsManager.AppSettingsXmlPath, "WindowsPosition");
            }

        }


#if BOW
        private ObservableCollection<ScripHelpNSEEquity> _ObjNSEEquityDataCollection = new ObservableCollection<ScripHelpNSEEquity>();

        public ObservableCollection<ScripHelpNSEEquity> ObjNSEEquityDataCollection
        {
            get { return _ObjNSEEquityDataCollection; }
            set { _ObjNSEEquityDataCollection = value; /*NotifyStaticPropertyChanged(nameof(ObjIndicesCollection));*/ }
        }

        private ObservableCollection<ScripHelpNSEDerivativeCO> _ObjNSEDerivativeCOCollection = new ObservableCollection<ScripHelpNSEDerivativeCO>();

        public ObservableCollection<ScripHelpNSEDerivativeCO> ObjNSEDerivativeCOCollection
        {
            get { return _ObjNSEDerivativeCOCollection; }
            set { _ObjNSEDerivativeCOCollection = value; /*NotifyStaticPropertyChanged(nameof(ObjIndicesCollection));*/ }
        }
        private ObservableCollection<ScripHelpNSECurrencyCO> _ObjNSECurrencyCOCollection = new ObservableCollection<ScripHelpNSECurrencyCO>();

        public ObservableCollection<ScripHelpNSECurrencyCO> ObjNSECurrencyCOCollection
        {
            get { return _ObjNSECurrencyCOCollection; }
            set { _ObjNSECurrencyCOCollection = value; /*NotifyStaticPropertyChanged(nameof(ObjIndicesCollection));*/ }
        }
        private ObservableCollection<ScripHelpNCDEXCommodities> _ObjNCDEXCommoditiesCollection = new ObservableCollection<ScripHelpNCDEXCommodities>();

        public ObservableCollection<ScripHelpNCDEXCommodities> ObjNCDEXCommoditiesCollection
        {
            get { return _ObjNCDEXCommoditiesCollection; }
            set { _ObjNCDEXCommoditiesCollection = value; /*NotifyStaticPropertyChanged(nameof(ObjIndicesCollection));*/ }
        }

        private ObservableCollection<ScripHelpMCXCommodities> _ObjMCXCommoditiesCollection = new ObservableCollection<ScripHelpMCXCommodities>();

        public ObservableCollection<ScripHelpMCXCommodities> ObjMCXCommoditiesCollection
        {
            get { return _ObjMCXCommoditiesCollection; }
            set { _ObjMCXCommoditiesCollection = value; /*NotifyStaticPropertyChanged(nameof(ObjIndicesCollection));*/ }
        }

#endif
        #endregion

        #endregion

        #region constructor


        public ScripHelpVM()
        {
            if (CommonFrontEnd.SharedMemories.ConfigurationMasterMemory.ConfigurationDict.ContainsKey("WindowsPosition"))
            {
                CommonFrontEnd.Model.BoltAppSettingsWindowsPosition oBoltAppSettingsWindowsPosition = new Model.BoltAppSettingsWindowsPosition();
                oBoltAppSettingsWindowsPosition = (CommonFrontEnd.Model.BoltAppSettingsWindowsPosition)CommonFrontEnd.SharedMemories.ConfigurationMasterMemory.ConfigurationDict["WindowsPosition"];
                if (oBoltAppSettingsWindowsPosition != null && oBoltAppSettingsWindowsPosition.ScripHelp != null && oBoltAppSettingsWindowsPosition.ScripHelp.WNDPOSITION != null)
                {
                    CorpActHeight = oBoltAppSettingsWindowsPosition.ScripHelp.WNDPOSITION.Down.ToString();
                    TopPosition = oBoltAppSettingsWindowsPosition.ScripHelp.WNDPOSITION.Top.ToString();
                    LeftPosition = oBoltAppSettingsWindowsPosition.ScripHelp.WNDPOSITION.Left.ToString();
                    CorpActWidth = oBoltAppSettingsWindowsPosition.ScripHelp.WNDPOSITION.Right.ToString();
                }
            }

            oDataAccessLayer = new DataAccessLayer();
            SegmentData = new ObservableCollection<string>();
            SearchEquityTemplist = new List<ScripHelpModel>();
            SearchDebtTemplist = new List<ScripHelpModel>();
            SearchDerCOTemplist = new List<ScripHelpBSEDerivativeCO>();
            //SearchDerSPDTemplist = new List<ScripHelpBSEDerivativeSPD>();
            SearchCurCOTemplist = new List<ScripHelpBSECurrencyCO>();
            ExchangeData = new List<string>();
            oDataAccessLayer.Connect((int)DataAccessLayer.ConnectionDB.Masters);
            populateExchangeData();
            //DefaultPopulateEquityBSE();

            //flagChange += populateGridData;
        }



        #endregion

        #region Methods
        private void EscapeUsingUserControl(object e)
        {
            ScripHelp oScripHelp = e as ScripHelp;
            oScripHelp?.Hide();
            Windows_ScripHelpLocationChanged(e);

        }
        private ScripHelpModel _SelectedItem;
        public ScripHelpModel SelectedItem
        {
            get { return _SelectedItem; }
            set { _SelectedItem = value; NotifyPropertyChanged(nameof(SelectedItem)); }
        }
        //private RelayCommand _DataGridDoubleClick;
        //public RelayCommand DataGridDoubleClick
        //{
        //    get
        //    {
        //        return _DataGridDoubleClick ?? (_DataGridDoubleClick = new RelayCommand(
        //            (object e) => DataGrid_DoubleClick()));
        //    }
        //}
        
        private ScripHelpBSEDerivativeCO _SelectedItemDerivateCO;
        public ScripHelpBSEDerivativeCO SelectedItemDerivateCO
        { 
            get { return _SelectedItemDerivateCO; }
            set { _SelectedItemDerivateCO = value; NotifyPropertyChanged(nameof(SelectedItemDerivateCO)); }
        }

        private void DataGrid_DoubleClickDerivateCO()
        {
            //if (selectedSegment == Common.Enumerations.Segment.Equity.ToString())
            //{
                ScripInfo2 obj1 = System.Windows.Application.Current.Windows.OfType<ScripInfo2>().FirstOrDefault();
                if (obj1 != null)
                {
                    obj1.Show();
                    obj1.Focus();
                }
                else
                {
                    obj1 = new ScripInfo2();
                    obj1.Activate();
                    obj1.Show();
                }

                ScripInfo2VM.GetInstance.SetDataDerivative(SelectedItemDerivateCO);
                OnClickGrid();

            //}
        }
       
        private ScripHelpBSECurrencyCO _SelectedItemCurrency;
        public ScripHelpBSECurrencyCO SelectedItemCurrency
        {
            get { return _SelectedItemCurrency; }
            set { _SelectedItemCurrency = value; NotifyPropertyChanged(nameof(SelectedItemCurrency)); }
        }
        private void DataGrid_DoubleClickCurrency()
        {
            //if (selectedSegment == Common.Enumerations.Segment.Equity.ToString())
            //{
            ScripInfo2 obj1 = System.Windows.Application.Current.Windows.OfType<ScripInfo2>().FirstOrDefault();
            if (obj1 != null)
            {
                obj1.Show();
                obj1.Focus();
            }
            else
            {
                obj1 = new ScripInfo2();
                obj1.Activate();
                obj1.Show();
            }

            ScripInfo2VM.GetInstance.SetDataCurrency(SelectedItemCurrency);
            OnClickGrid();


            //}
        }
        private ScripHelpModel _SelectedItemDebt;
        public ScripHelpModel SelectedItemDebt
        {
            get { return _SelectedItemDebt; }
            set { _SelectedItemDebt = value; NotifyPropertyChanged(nameof(SelectedItemDebt)); }
        }
        private void DataGrid_DoubleClickDebt()
        {
            //if (selectedSegment == Common.Enumerations.Segment.Debt.ToString())
            //{
                ScripInfo obj = System.Windows.Application.Current.Windows.OfType<ScripInfo>().FirstOrDefault();
                if (obj != null)
                {
                    obj.Show();
                    obj.Focus();
                }
                else
                {
                    obj = new ScripInfo();
                    obj.Activate();
                    obj.Show();
                }

                ScripInfoVM.GetInstance.SetDebtData(SelectedItemDebt);



            //}
        }
        private void DataGrid_DoubleClickEquity()
        {
            // if (selectedSegment == Common.Enumerations.Segment.Equity.ToString())
            // {
            ScripInfo obj = System.Windows.Application.Current.Windows.OfType<ScripInfo>().FirstOrDefault();
            if (obj != null)
            {
                obj.Show();
                obj.Focus();
            }
            else
            {
                obj = new ScripInfo();
                obj.Activate();
                obj.Show();
            }

            ScripInfoVM.GetInstance.SetDataEquity(SelectedItem);
            


            //}
        }
        
        private ScripHelpBSEDerivativeCO _SelectedItemDerivateSPD;
        public ScripHelpBSEDerivativeCO SelectedItemDerivateSPD
        {
            get { return _SelectedItemDerivateSPD; }
            set { _SelectedItemDerivateSPD = value; NotifyPropertyChanged(nameof(SelectedItemDerivateSPD)); }
        }
        private void DataGrid_DoubleClickDerivateSPD()
        {
            // if (selectedSegment == Common.Enumerations.Segment.Equity.ToString())
            // {
            ScripInfo2 obj = System.Windows.Application.Current.Windows.OfType<ScripInfo2>().FirstOrDefault();
            if (obj != null)
            {
                obj.Show();
                obj.Focus();
            }
            else
            {
                obj = new ScripInfo2();
                obj.Activate();
                obj.Show();
            }

            ScripInfo2VM.GetInstance.SetDataDerivateSPD(SelectedItemDerivateSPD);
            OnClickGrid();

            //}
        }
        
        private ScripHelpBSECurrencyCO _SelectedItemCurrencySPD;
        public ScripHelpBSECurrencyCO SelectedItemCurrencySPD
        {
            get { return _SelectedItemCurrencySPD; }
            set { _SelectedItemCurrencySPD = value; NotifyPropertyChanged(nameof(SelectedItemCurrencySPD)); }
        }
        private void DataGrid_DoubleClickCurrencySPD()
        {
            // if (selectedSegment == Common.Enumerations.Segment.Equity.ToString())
            // {
            ScripInfo2 obj = System.Windows.Application.Current.Windows.OfType<ScripInfo2>().FirstOrDefault();
            if (obj != null)
            {
                obj.Show();
                obj.Focus();
            }
            else
            {
                obj = new ScripInfo2();
                obj.Activate();
                obj.Show();
            }

            ScripInfo2VM.GetInstance.SetDataCurrencySPD(SelectedItemCurrencySPD);
            OnClickGrid();


            //}
        }
        private void OnClickGrid()
        {
            if (selectedSegment == Common.Enumerations.Segment.Derivative.ToString() && cochecked == true)
            {
                ScripInfo2VM.GetInstance.SPDVisible = "Hidden";

            }
            else if (selectedSegment == Common.Enumerations.Segment.Derivative.ToString() && spdchecked == true)
            {
                ScripInfo2VM.GetInstance.SPDVisible = "Visible";
            }
            else if (selectedSegment == Common.Enumerations.Segment.Currency.ToString() && cochecked == true)

            {
                ScripInfo2VM.GetInstance.SPDVisible = "Hidden";
            }
            else if (selectedSegment == Common.Enumerations.Segment.Currency.ToString() && spdchecked == true)
            {
                ScripInfo2VM.GetInstance.SPDVisible = "Visible";
            }

        }
        private void SaveinCSV(object e)
        {
            try
            {
                //txtReply = string.Empty;
                SaveFileDialog objSaveinCSV = new SaveFileDialog();
                objSaveinCSV.InitialDirectory = Path.Combine(Path.GetDirectoryName(Path.GetFullPath(Path.Combine(System.Environment.CurrentDirectory, @"User/"))));
                if (!Directory.Exists(objSaveinCSV.InitialDirectory))
                    Directory.CreateDirectory(objSaveinCSV.InitialDirectory);

                //objFileDialogBatchResub.Title = "Browse CSV Files";
                objSaveinCSV.DefaultExt = "csv";
                string Filter = "CSV files (*.csv)|*.csv";
                objSaveinCSV.Filter = Filter;

                if (selectedSegment == Common.Enumerations.Segment.Equity.ToString())
                {
                    const string header = "SCID, ScripCode, ExchangeCode, ScripId, ScripName, PartitionId, ProductId, InstrumentType, GroupName, ScripType, FaceValue, MarketLot, TickSize, BseExclusive, Status, ExDivDate, NoDelEndDate, NoDelStartDate, NewTickSize, IsinCode, CallAuctionIndicator, BcStartDate, ExBonusDate, ExRightDate, BcEndDate";
                    StreamWriter writer = null;

                    //objFileDialogBatchResub.ShowDialog();
                    if (objSaveinCSV.ShowDialog() == DialogResult.OK)
                    {
                        Filter = objSaveinCSV.FileName;

                        writer = new StreamWriter(Filter, false, Encoding.UTF8);

                        writer.WriteLine(header);

                        foreach (var item in ObjEquityDataCollection)
                        {
                            // string Rate;

                            // Rate = Convert.ToString(Convert.ToDouble(item.rate) * Math.Pow(10, DecimalPoint));
                            // if (seg == Segment.Equity.ToString())
                            writer.WriteLine($"{item.SCID}, {item.ScripCode}, {item.ExchangeCode}, {item.ScripId}, {item.ScripName}, {item.PartitionId}, {item.ProductId}, {item.InstrumentType},{item.GroupName},{item.ScripType},{item.FaceValue},{item.MarketLot},{item.TickSize},{item.BseExclusive},{item.Status},{item.ExDivDate},{item.NoDelEndDate},{item.NoDelStartDate},{item.NewTickSize},{item.IsinCode},{item.CallAuctionIndicator},{item.BcStartDate},{item.ExBonusDate},{item.ExRightDate},{item.BcEndDate}");

                        }
                    }
                    writer.Close();
                }
                else if (selectedSegment == Common.Enumerations.Segment.Derivative.ToString() && cochecked == true)
                {

                    const string header1 = "ContractTokenNum, ScripID, AssestTokenNum, InstrumentType, AssetCd, UnderlyingAsset, ExpiryDate, StrikePrice, OptionType, Precision, PartitionID, ProductID, CapacityGroupID, MinimumLotSize, TickSize, InstrumentName, QuantityMultiplier, UnderlyingMarket, ContractType, ProductCode, BasePrice, DeleteFlag";
                    
                    StreamWriter writer1 = null;
                    if (objSaveinCSV.ShowDialog() == DialogResult.OK)
                    {
                        Filter = objSaveinCSV.FileName;

                        writer1 = new StreamWriter(Filter, false, Encoding.UTF8);

                        writer1.WriteLine(header1);

                        foreach (var item in ObjBSEDerivativeCOCollection)
                        {

                            writer1.WriteLine($"{item.ContractTokenNum }, {item.ScripID}, {item.AssestTokenNum}, {item.InstrumentType}, {item.AssetCd}, {item.UnderlyingAsset}, {item.ExpiryDate}, {item.StrikePrice},{item.OptionType},{item.Precision},{item.PartitionID},{item.ProductID},{item.CapacityGroupID},{item.MinimumLotSize},{item.TickSize},{item.InstrumentName},{item.QuantityMultiplier},{item.UnderlyingMarket},{item.ContractType},{item.ProductCode},{item.BasePrice},{item.DeleteFlag}");
                        }
                    }
                    writer1.Close();
                }
                else if (selectedSegment == Common.Enumerations.Segment.Derivative.ToString() && spdchecked == true)
                {

                    const string header3 = "ContractTokenNum, ScripID, AssestTokenNum, InstrumentType, AssetCd, UnderlyingAsset, ExpiryDate, StrikePrice, OptionType, Precision, PartitionID, ProductID, CapacityGroupID, MinimumLotSize, TickSize, InstrumentName, QuantityMultiplier, UnderlyingMarket, ContractType, ProductCode, BasePrice, DeleteFlag, ContractTokenNum_Leg1, ContractTokenNum_Leg2, NTAScripCode, StrategyID, NoofLegsinStrategy, Eligibility";

                    
                    StreamWriter writer1 = null;
                    if (objSaveinCSV.ShowDialog() == DialogResult.OK)
                    {
                        Filter = objSaveinCSV.FileName;

                        writer1 = new StreamWriter(Filter, false, Encoding.UTF8);

                        writer1.WriteLine(header3);

                        foreach (var item in ObjBSEDerivativeCOCollection)
                        {

                            writer1.WriteLine($"{item.ContractTokenNum }, {item.ScripID}, {item.AssestTokenNum}, {item.InstrumentType}, {item.AssetCd}, {item.UnderlyingAsset}, {item.ExpiryDate}, {item.StrikePrice},{item.OptionType},{item.Precision},{item.PartitionID},{item.ProductID},{item.CapacityGroupID},{item.MinimumLotSize},{item.TickSize},{item.InstrumentName},{item.QuantityMultiplier},{item.UnderlyingMarket},{item.ContractType},{item.ProductCode},{item.BasePrice},{item.DeleteFlag},{item.ContractTokenNum_Leg1},{item.ContractTokenNum_Leg2},{item.NTAScripCode},{item.StrategyID},{item.NoofLegsinStrategy},{item.Eligibility}");
                        }
                    }
                    writer1.Close();
                }
                else if (selectedSegment == Common.Enumerations.Segment.Currency.ToString() && cochecked == true)
                {

                    const string header4 = "ContractTokenNum, ScripID, AssestTokenNum, InstrumentType, AssetCD, ExpiryDate, StrikePrice, OptionType, Precision, PartitionID, ProductID, CapacityGroupID, MinimumLotSize, TickSize, InstrumentName, QuantityMultiplier, UnderlyingMarket, ContractType, BasePrice, DeleteFlag";
                    
                    StreamWriter writer1 = null;
                    if (objSaveinCSV.ShowDialog() == DialogResult.OK)
                    {
                        Filter = objSaveinCSV.FileName;

                        writer1 = new StreamWriter(Filter, false, Encoding.UTF8);

                        writer1.WriteLine(header4);

                        foreach (var item in ObjBSECurrencyCOCollection)
                        {
                            writer1.WriteLine($"{item.ContractTokenNum }, {item.ScripID}, {item.AssestTokenNum}, {item.InstrumentType}, {item.AssetCD}, {item.ExpiryDate}, {item.StrikePrice},{item.OptionType},{item.Precision},{item.PartitionID},{item.ProductID},{item.CapacityGroupID},{item.MinimumLotSize},{item.TickSize},{item.InstrumentName},{item.QuantityMultiplier},{item.UnderlyingMarket},{item.ContractType},{item.BasePrice},{item.DeleteFlag}");
                        }
                        writer1.Close();
                    }
                }
                else if (selectedSegment == Common.Enumerations.Segment.Currency.ToString() && spdchecked == true)
                {

                    const string header4 = "ContractTokenNum, ScripID, AssestTokenNum, InstrumentType, AssetCD, ExpiryDate, StrikePrice, OptionType, Precision, PartitionID, ProductID, CapacityGroupID, MinimumLotSize, TickSize, InstrumentName, QuantityMultiplier, UnderlyingMarket, ContractType, BasePrice, DeleteFlag, ContractTokenNum_Leg1, ContractTokenNum_Leg2, NTAScripCode, StrategyID, NoofLegsinStrategy, Eligibility";

                    
                    StreamWriter writer1 = null;
                    if (objSaveinCSV.ShowDialog() == DialogResult.OK)
                    {
                        Filter = objSaveinCSV.FileName;

                        writer1 = new StreamWriter(Filter, false, Encoding.UTF8);

                        writer1.WriteLine(header4);

                        foreach (var item in ObjBSECurrencyCOCollection)
                        {
                            writer1.WriteLine($"{item.ContractTokenNum}, {item.ScripID},{item.AssestTokenNum},{item.InstrumentType},{item.AssetCD},{item.ExpiryDate},{item.StrikePrice},{item.OptionType},{item.Precision},{item.PartitionID},{item.ProductID},{item.CapacityGroupID},{item.MinimumLotSize},{item.TickSize},{item.InstrumentName},{item.QuantityMultiplier},{item.UnderlyingMarket},{item.ContractType},{item.BasePrice},{item.DeleteFlag}, {item.ContractTokenNum_Leg1}, {item.ContractTokenNum_Leg2}, {item.NTAScripCode}, {item.StrategyID}, {item.NoofLegsinStrategy}, {item.Eligibility}");
                        }
                        writer1.Close();
                    }
                }
                else if (selectedSegment == Common.Enumerations.Segment.Debt.ToString())
                {

                    const string header5 = "SCID, ScripCode, ExchangeCode, ScripId, ScripName, PartitionId, InstrumentType, GroupName, ScripType, FaceValue, MarketLot, TickSize, BseExclusive, Status, ExDivDate, NoDelEndDate, NoDelStartDate, NewTickSize, IsinCode, CallAuctionIndicator, BcStartDate, ExBonusDate, ExRightDate, BcEndDate, SCSequenceId";
                    StreamWriter writer3 = null;
                    if (objSaveinCSV.ShowDialog() == DialogResult.OK)
                    {
                        Filter = objSaveinCSV.FileName;

                        writer3 = new StreamWriter(Filter, false, Encoding.UTF8);

                        writer3.WriteLine(header5);
                        foreach (var item in ObjDebtDataCollection)
                        {
                            writer3.WriteLine($"{item.SCID}, {item.ScripCode}, {item.ExchangeCode}, {item.ScripId}, {item.ScripName}, {item.PartitionId}, {item.InstrumentType},{item.GroupName},{item.ScripType},{item.FaceValue}, {item.MarketLot},{item.TickSize},{item.BseExclusive},{item.Status}, {item.ExDivDate},{item.NoDelEndDate},{item.NoDelStartDate},{item.NewTickSize},{item.IsinCode},{item.CallAuctionIndicator},{item.BcStartDate},{item.ExBonusDate},{item.ExRightDate},{item.BcEndDate},{item.SCSequenceId}");
                        }
                        writer3.Close();
                    }

                }
                System.Windows.MessageBox.Show("File Saved Successfully in"+ directory2,  "Information");
            }
            //writer.Close();

            // 


            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
            }


        }

        /// <summary>
        /// populateExchangeData
        /// </summary>
        private void populateExchangeData()
        {
            ExchangeData.Clear();

            ExchangeData.Add(Common.Enumerations.Exchange.BSE.ToString());
#if BOW
            ExchangeData.Add(Common.Enumerations.Exchange.NSE.ToString());
            ExchangeData.Add(Common.Enumerations.Exchange.NCDEX.ToString());
            ExchangeData.Add(Common.Enumerations.Exchange.MCX.ToString());
#endif
            selectedExchange = Common.Enumerations.Exchange.BSE.ToString();
        }

        private void populateSegment()
        {

            SegmentData.Clear();
            if (selectedExchange == Common.Enumerations.Exchange.BSE.ToString())
            {
                SegmentData.Add(Common.Enumerations.Segment.Equity.ToString());
                SegmentData.Add(Common.Enumerations.Segment.Derivative.ToString());
                SegmentData.Add(Common.Enumerations.Segment.Currency.ToString());
                SegmentData.Add(Common.Enumerations.Segment.Debt.ToString());
                selectedSegment = Common.Enumerations.Segment.Equity.ToString();
            }
#if BOW
            else if (selectedExchange == Common.Enumerations.Exchange.NSE.ToString())
            {
                SegmentData.Add(Common.Enumerations.Segment.Equity.ToString());
                SegmentData.Add(Common.Enumerations.Segment.Derivative.ToString());
                SegmentData.Add(Common.Enumerations.Segment.Currency.ToString());
                selectedSegment = Common.Enumerations.Segment.Equity.ToString();
            }
            else if (selectedExchange == Common.Enumerations.Exchange.NCDEX.ToString())
            {
                SegmentData.Add(Common.Enumerations.Segment.Commodities.ToString());
                selectedSegment = Common.Enumerations.Segment.Commodities.ToString();
            }
            else if (selectedExchange == Common.Enumerations.Exchange.MCX.ToString())
            {
                SegmentData.Add(Common.Enumerations.Segment.Commodities.ToString());
                selectedSegment = Common.Enumerations.Segment.Commodities.ToString();
            }
#endif
        }

        /// <summary>
        /// Hide Or Visible controls on the change of Segment or Exchange
        /// </summary>
        private void ControleVisibility()
        {
            if (selectedExchange == Common.Enumerations.Exchange.BSE.ToString() && selectedSegment == Common.Enumerations.Segment.Equity.ToString())
            {
                VisibilityBSEEquityGrid = "Visible";
                VisibilityBSEDerivativeCOGrid = "Hidden";
                VisibilityBSEDerivativeSPDGrid = "Hidden";
                VisibilityBSECurrencyCOGrid = "Hidden";
                VisibilityBSECurrencySPDGrid = "Hidden";
                VisibilityBSEDebtGrid = "Hidden";
                VisibilityNSEEquityGrid = "Hidden";
                VisibilityNSEDerivativeCOGrid = "Hidden";
                VisibilityNSECurrencyCOGrid = "Hidden";
                VisibilityNCDEXCommoditiesGrid = "Hidden";
                VisibilityMCXCommoditiesGrid = "Hidden";
                ISINCodeVisibility = "Visible";
            }
            else if (selectedExchange == Common.Enumerations.Exchange.BSE.ToString() && selectedSegment == Common.Enumerations.Segment.Derivative.ToString() && cochecked == true)
            {
                VisibilityBSEEquityGrid = "Hidden";
                VisibilityBSEDerivativeCOGrid = "Visible";
                VisibilityBSEDerivativeSPDGrid = "Hidden";
                VisibilityBSECurrencyCOGrid = "Hidden";
                VisibilityBSECurrencySPDGrid = "Hidden";
                VisibilityBSEDebtGrid = "Hidden";
                VisibilityNSEEquityGrid = "Hidden";
                VisibilityNSEDerivativeCOGrid = "Hidden";
                VisibilityNSECurrencyCOGrid = "Hidden";
                VisibilityNCDEXCommoditiesGrid = "Hidden";
                VisibilityMCXCommoditiesGrid = "Hidden";
                ISINCodeVisibility = "Hidden";
            }
            else if (selectedExchange == Common.Enumerations.Exchange.BSE.ToString() && selectedSegment == Common.Enumerations.Segment.Derivative.ToString() && spdchecked == true)
            {
                VisibilityBSEEquityGrid = "Hidden";
                VisibilityBSEDerivativeCOGrid = "Hidden";
                VisibilityBSEDerivativeSPDGrid = "Visible";
                VisibilityBSECurrencyCOGrid = "Hidden";
                VisibilityBSECurrencySPDGrid = "Hidden";
                VisibilityBSEDebtGrid = "Hidden";
                VisibilityNSEEquityGrid = "Hidden";
                VisibilityNSEDerivativeCOGrid = "Hidden";
                VisibilityNSECurrencyCOGrid = "Hidden";
                VisibilityNCDEXCommoditiesGrid = "Hidden";
                VisibilityMCXCommoditiesGrid = "Hidden";
                ISINCodeVisibility = "Hidden";
            }
            else if (selectedExchange == Common.Enumerations.Exchange.BSE.ToString() && selectedSegment == Common.Enumerations.Segment.Currency.ToString() && cochecked == true)
            {
                VisibilityBSEEquityGrid = "Hidden";
                VisibilityBSEDerivativeCOGrid = "Hidden";
                VisibilityBSEDerivativeSPDGrid = "Hidden";
                VisibilityBSECurrencyCOGrid = "Visible";
                VisibilityBSECurrencySPDGrid = "Hidden";
                VisibilityBSEDebtGrid = "Hidden";
                VisibilityNSEEquityGrid = "Hidden";
                VisibilityNSEDerivativeCOGrid = "Hidden";
                VisibilityNSECurrencyCOGrid = "Hidden";
                VisibilityNCDEXCommoditiesGrid = "Hidden";
                VisibilityMCXCommoditiesGrid = "Hidden";
                ISINCodeVisibility = "Hidden";
            }
            else if (selectedExchange == Common.Enumerations.Exchange.BSE.ToString() && selectedSegment == Common.Enumerations.Segment.Currency.ToString() && spdchecked == true)
            {
                VisibilityBSEEquityGrid = "Hidden";
                VisibilityBSEDerivativeCOGrid = "Hidden";
                VisibilityBSEDerivativeSPDGrid = "Hidden";
                VisibilityBSECurrencyCOGrid = "Hidden";
                VisibilityBSECurrencySPDGrid = "Visible";
                VisibilityBSEDebtGrid = "Hidden";
                VisibilityNSEEquityGrid = "Hidden";
                VisibilityNSEDerivativeCOGrid = "Hidden";
                VisibilityNSECurrencyCOGrid = "Hidden";
                VisibilityNCDEXCommoditiesGrid = "Hidden";
                VisibilityMCXCommoditiesGrid = "Hidden";
                ISINCodeVisibility = "Hidden";
            }
            else if (selectedExchange == Common.Enumerations.Exchange.BSE.ToString() && selectedSegment == Common.Enumerations.Segment.Debt.ToString())
            {
                VisibilityBSEEquityGrid = "Hidden";
                VisibilityBSEDerivativeCOGrid = "Hidden";
                VisibilityBSEDerivativeSPDGrid = "Hidden";
                VisibilityBSECurrencyCOGrid = "Hidden";
                VisibilityBSECurrencySPDGrid = "Hidden";
                VisibilityBSEDebtGrid = "Visible";
                VisibilityNSEEquityGrid = "Hidden";
                VisibilityNSEDerivativeCOGrid = "Hidden";
                VisibilityNSECurrencyCOGrid = "Hidden";
                VisibilityNCDEXCommoditiesGrid = "Hidden";
                VisibilityMCXCommoditiesGrid = "Hidden";
                ISINCodeVisibility = "Visible";

            }
            else if (selectedExchange == Common.Enumerations.Exchange.NSE.ToString() && selectedSegment == Common.Enumerations.Segment.Equity.ToString())
            {
                VisibilityBSEEquityGrid = "Hidden";
                VisibilityBSEDerivativeCOGrid = "Hidden";
                VisibilityBSEDerivativeSPDGrid = "Hidden";
                VisibilityBSECurrencyCOGrid = "Hidden";
                VisibilityBSECurrencySPDGrid = "Hidden";
                VisibilityBSEDebtGrid = "Hidden";
                VisibilityNSEEquityGrid = "Visible";
                VisibilityNSEDerivativeCOGrid = "Hidden";
                VisibilityNSECurrencyCOGrid = "Hidden";
                VisibilityNCDEXCommoditiesGrid = "Hidden";
                VisibilityMCXCommoditiesGrid = "Hidden";
            }
            else if (selectedExchange == Common.Enumerations.Exchange.NSE.ToString() && selectedSegment == Common.Enumerations.Segment.Derivative.ToString() && cochecked == true)
            {
                VisibilityBSEEquityGrid = "Hidden";
                VisibilityBSEDerivativeCOGrid = "Hidden";
                VisibilityBSEDerivativeSPDGrid = "Hidden";
                VisibilityBSECurrencyCOGrid = "Hidden";
                VisibilityBSECurrencySPDGrid = "Hidden";
                VisibilityBSEDebtGrid = "Hidden";
                VisibilityNSEEquityGrid = "Hidden";
                VisibilityNSEDerivativeCOGrid = "Visible";
                VisibilityNSECurrencyCOGrid = "Hidden";
                VisibilityNCDEXCommoditiesGrid = "Hidden";
                VisibilityMCXCommoditiesGrid = "Hidden";
            }
            else if (selectedExchange == Common.Enumerations.Exchange.NSE.ToString() && selectedSegment == Common.Enumerations.Segment.Currency.ToString() && cochecked == true)
            {
                VisibilityBSEEquityGrid = "Hidden";
                VisibilityBSEDerivativeCOGrid = "Hidden";
                VisibilityBSEDerivativeSPDGrid = "Hidden";
                VisibilityBSECurrencyCOGrid = "Hidden";
                VisibilityBSECurrencySPDGrid = "Hidden";
                VisibilityBSEDebtGrid = "Hidden";
                VisibilityNSEEquityGrid = "Hidden";
                VisibilityNSEDerivativeCOGrid = "Hidden";
                VisibilityNSECurrencyCOGrid = "Visible";
                VisibilityNCDEXCommoditiesGrid = "Hidden";
                VisibilityMCXCommoditiesGrid = "Hidden";
            }
            else if (selectedExchange == Common.Enumerations.Exchange.NCDEX.ToString() && selectedSegment == Common.Enumerations.Segment.Commodities.ToString())
            {
                VisibilityBSEEquityGrid = "Hidden";
                VisibilityBSEDerivativeCOGrid = "Hidden";
                VisibilityBSEDerivativeSPDGrid = "Hidden";
                VisibilityBSECurrencyCOGrid = "Hidden";
                VisibilityBSECurrencySPDGrid = "Hidden";
                VisibilityBSEDebtGrid = "Hidden";
                VisibilityNSEEquityGrid = "Hidden";
                VisibilityNSEDerivativeCOGrid = "Hidden";
                VisibilityNSECurrencyCOGrid = "Hidden";
                VisibilityNCDEXCommoditiesGrid = "Visible";
                VisibilityMCXCommoditiesGrid = "Hidden";
            }
            else if (selectedExchange == Common.Enumerations.Exchange.MCX.ToString() && selectedSegment == Common.Enumerations.Segment.Commodities.ToString())
            {
                VisibilityBSEEquityGrid = "Hidden";
                VisibilityBSEDerivativeCOGrid = "Hidden";
                VisibilityBSEDerivativeSPDGrid = "Hidden";
                VisibilityBSECurrencyCOGrid = "Hidden";
                VisibilityBSECurrencySPDGrid = "Hidden";
                VisibilityBSEDebtGrid = "Hidden";
                VisibilityNSEEquityGrid = "Hidden";
                VisibilityNSEDerivativeCOGrid = "Hidden";
                VisibilityNSECurrencyCOGrid = "Hidden";
                VisibilityNCDEXCommoditiesGrid = "Hidden";
                VisibilityMCXCommoditiesGrid = "Visible";
            }
            else
            {
                VisibilityBSEEquityGrid = "Hidden";
                VisibilityBSEDerivativeCOGrid = "Hidden";
                VisibilityBSEDerivativeSPDGrid = "Hidden";
                VisibilityBSECurrencyCOGrid = "Hidden";
                VisibilityBSECurrencySPDGrid = "Hidden";
                VisibilityBSEDebtGrid = "Hidden";
                VisibilityNSEEquityGrid = "Hidden";
                VisibilityNSEDerivativeCOGrid = "Hidden";
                VisibilityNSECurrencyCOGrid = "Hidden";
                VisibilityNCDEXCommoditiesGrid = "Hidden";
                VisibilityMCXCommoditiesGrid = "Hidden";
            }
            if (selectedExchange == Common.Enumerations.Exchange.BSE.ToString() && (selectedSegment == Common.Enumerations.Segment.Derivative.ToString() || selectedSegment == Common.Enumerations.Segment.Currency.ToString()))
            {
                CoSpdVisibility = "Visible";
            }
            else
            {
                CoSpdVisibility = "Hidden";
            }
        }

        /// <summary>
        /// By Default BSE Equity Master Load, When Scrip Help Window will open
        /// </summary>
        private void DefaultPopulateEquityBSE()
        {
            try
            {
                oDataAccessLayer.OpenConnection((int)DataAccessLayer.ConnectionDB.Masters);
                VisibilityBSEEquityGrid = "Visible";
                VisibilityBSEDerivativeCOGrid = "Hidden";
                VisibilityBSEDerivativeSPDGrid = "Hidden";
                VisibilityBSECurrencyCOGrid = "Hidden";
                VisibilityBSECurrencySPDGrid = "Hidden";
                VisibilityBSEDebtGrid = "Hidden";
                VisibilityNSEEquityGrid = "Hidden";
                VisibilityNSEDerivativeCOGrid = "Hidden";
                VisibilityNSECurrencyCOGrid = "Hidden";
                VisibilityNCDEXCommoditiesGrid = "Hidden";
                ISINCodeVisibility = "Visible";
                string strQuery = @"SELECT SCID, ScripCode, ExchangeCode, ScripId, ScripName, PartitionId, InstrumentType, GroupName, ScripType, FaceValue, MarketLot, TickSize, BseExclusive, Status, ExDivDate, NoDelEndDate, NoDelStartDate, NewTickSize, IsinCode, CallAuctionIndicator, BcStartDate, ExBonusDate, ExRightDate,  Filler, Filler2_GSM, BcEndDate, Filler3, SCSequenceId  FROM BSE_SECURITIES_CFE Where InstrumentType = 'E';";
                ObjEquityDataCollection = new ObservableCollection<ScripHelpModel>();
                SQLiteDataReader oSQLiteDataReader = oDataAccessLayer.ExecuteDataReader((int)ConnectionDB.Masters,strQuery, System.Data.CommandType.Text, null);

                while (oSQLiteDataReader.Read())
                {
                    ScripHelpModel objScripHelpModel = new ScripHelpModel();
                    objScripHelpModel.SCID = Convert.ToInt64(oSQLiteDataReader["SCID"]);
                    objScripHelpModel.ScripCode = Convert.ToInt32(oSQLiteDataReader["ScripCode"]);
                    objScripHelpModel.ExchangeCode = oSQLiteDataReader["ExchangeCode"].ToString();
                    objScripHelpModel.ScripId = oSQLiteDataReader["ScripId"].ToString();
                    objScripHelpModel.ScripName = oSQLiteDataReader["ScripName"].ToString();
                    //objScripHelpModel.PartitionId = oSQLiteDataReader["PartitionId"].ToString();
                    string[] arr = oSQLiteDataReader["PartitionId"].ToString().Split('-');
                    objScripHelpModel.PartitionId = arr[0];
                    objScripHelpModel.ProductId = arr[1];
                    objScripHelpModel.InstrumentType = Convert.ToChar(oSQLiteDataReader["InstrumentType"]);
                    objScripHelpModel.GroupName = oSQLiteDataReader["GroupName"].ToString();
                    objScripHelpModel.ScripType = Convert.ToChar(oSQLiteDataReader["ScripType"]);
                    objScripHelpModel.FaceValue = Convert.ToInt32(oSQLiteDataReader["FaceValue"]) / 1000;
                    objScripHelpModel.MarketLot = Convert.ToInt32(oSQLiteDataReader["MarketLot"]);
                    objScripHelpModel.TickSize = Convert.ToDouble(oSQLiteDataReader["TickSize"]);
                    objScripHelpModel.BseExclusive = Convert.ToChar(oSQLiteDataReader["BseExclusive"]);
                    objScripHelpModel.Status = Convert.ToChar(oSQLiteDataReader["Status"]);
                    objScripHelpModel.ExDivDate = oSQLiteDataReader["ExDivDate"].ToString();
                    objScripHelpModel.NoDelEndDate = oSQLiteDataReader["NoDelEndDate"].ToString();
                    objScripHelpModel.NoDelStartDate = oSQLiteDataReader["NoDelStartDate"].ToString();
                    objScripHelpModel.NewTickSize = oSQLiteDataReader["NewTickSize"].ToString();
                    objScripHelpModel.IsinCode = oSQLiteDataReader["IsinCode"].ToString();
                    objScripHelpModel.CallAuctionIndicator = Convert.ToInt32(oSQLiteDataReader["CallAuctionIndicator"]);
                    objScripHelpModel.BcStartDate = oSQLiteDataReader["BcStartDate"].ToString();
                    objScripHelpModel.ExBonusDate = oSQLiteDataReader["ExBonusDate"].ToString();
                    objScripHelpModel.ExRightDate = oSQLiteDataReader["ExRightDate"].ToString();
                    objScripHelpModel.Filler2_GSM = Convert.ToInt32(oSQLiteDataReader["Filler2_GSM"]);
                    objScripHelpModel.BcEndDate = oSQLiteDataReader["BcEndDate"].ToString();
                    objScripHelpModel.SCSequenceId = Convert.ToInt64(oSQLiteDataReader["SCSequenceId"]);

                    ObjEquityDataCollection.Add(objScripHelpModel);

                }
            }
            catch (Exception ex) { }
            finally
            {
                oDataAccessLayer.CloseConnection((int)DataAccessLayer.ConnectionDB.Masters);
            }
        }

        /// <summary>
        /// Populate Grid Data Segment Wise and Exchange Wise
        /// </summary>
        private void populateGridData()
        {
            try
            {
                oDataAccessLayer.OpenConnection((int)DataAccessLayer.ConnectionDB.Masters);
                if (selectedExchange == Common.Enumerations.Exchange.BSE.ToString() && selectedSegment == Common.Enumerations.Segment.Equity.ToString())
                {
                    VisibilityBSEEquityGrid = "Visible";
                    VisibilityBSEDerivativeCOGrid = "Hidden";
                    VisibilityBSEDerivativeSPDGrid = "Hidden";
                    VisibilityBSECurrencyCOGrid = "Hidden";
                    VisibilityBSECurrencySPDGrid = "Hidden";
                    VisibilityBSEDebtGrid = "Hidden";
                    VisibilityNSEEquityGrid = "Hidden";
                    VisibilityNSEDerivativeCOGrid = "Hidden";
                    VisibilityNSECurrencyCOGrid = "Hidden";
                    VisibilityNCDEXCommoditiesGrid = "Hidden";
                    VisibilityMCXCommoditiesGrid = "Hidden";
                    ISINCodeVisibility = "Visible";
                    string strQuery = @"SELECT SCID, ScripCode, ExchangeCode, ScripId, ScripName, PartitionId, InstrumentType, GroupName, ScripType, FaceValue, MarketLot, TickSize, BseExclusive, Status, ExDivDate, NoDelEndDate, NoDelStartDate, NewTickSize, IsinCode, CallAuctionIndicator, BcStartDate, ExBonusDate, ExRightDate,  Filler, Filler2_GSM, BcEndDate, Filler3, SCSequenceId  FROM BSE_SECURITIES_CFE Where InstrumentType = 'E';";
                    ObjEquityDataCollection = new ObservableCollection<ScripHelpModel>();
                    SQLiteDataReader oSQLiteDataReader = oDataAccessLayer.ExecuteDataReader((int)ConnectionDB.Masters,strQuery, System.Data.CommandType.Text, null);
                    while (oSQLiteDataReader.Read())
                    {
                        ScripHelpModel objScripHelpModel = new ScripHelpModel();
                        objScripHelpModel.SCID = Convert.ToInt64(oSQLiteDataReader["SCID"]);
                        objScripHelpModel.ScripCode = Convert.ToInt32(oSQLiteDataReader["ScripCode"]);
                        objScripHelpModel.ExchangeCode = oSQLiteDataReader["ExchangeCode"].ToString();
                        objScripHelpModel.ScripId = oSQLiteDataReader["ScripId"].ToString();
                        objScripHelpModel.ScripName = oSQLiteDataReader["ScripName"].ToString();
                        string[] arr = oSQLiteDataReader["PartitionId"].ToString().Split('-');
                        objScripHelpModel.PartitionId = arr[0];
                        objScripHelpModel.ProductId = arr[1];
                        objScripHelpModel.InstrumentType = Convert.ToChar(oSQLiteDataReader["InstrumentType"]);
                        objScripHelpModel.GroupName = oSQLiteDataReader["GroupName"].ToString();
                        objScripHelpModel.ScripType = Convert.ToChar(oSQLiteDataReader["ScripType"]);
                        objScripHelpModel.FaceValue = Convert.ToDecimal(Convert.ToInt32(oSQLiteDataReader["FaceValue"]) / 1000);
                        objScripHelpModel.MarketLot = Convert.ToInt32(oSQLiteDataReader["MarketLot"]);
                        objScripHelpModel.TickSize = Convert.ToDouble(oSQLiteDataReader["TickSize"]);
                        objScripHelpModel.BseExclusive = Convert.ToChar(oSQLiteDataReader["BseExclusive"]);
                        objScripHelpModel.Status = Convert.ToChar(oSQLiteDataReader["Status"]);
                        objScripHelpModel.ExDivDate = oSQLiteDataReader["ExDivDate"].ToString();
                        objScripHelpModel.NoDelEndDate = oSQLiteDataReader["NoDelEndDate"].ToString();
                        objScripHelpModel.NoDelStartDate = oSQLiteDataReader["NoDelStartDate"].ToString();
                        objScripHelpModel.NewTickSize = oSQLiteDataReader["NewTickSize"].ToString();
                        objScripHelpModel.IsinCode = oSQLiteDataReader["IsinCode"].ToString();
                        objScripHelpModel.CallAuctionIndicator = Convert.ToInt32(oSQLiteDataReader["CallAuctionIndicator"]);
                        objScripHelpModel.BcStartDate = oSQLiteDataReader["BcStartDate"].ToString();
                        objScripHelpModel.ExBonusDate = oSQLiteDataReader["ExBonusDate"].ToString();
                        objScripHelpModel.ExRightDate = oSQLiteDataReader["ExRightDate"].ToString();
                        objScripHelpModel.Filler2_GSM = Convert.ToInt32(oSQLiteDataReader["Filler2_GSM"]);
                        objScripHelpModel.BcEndDate = oSQLiteDataReader["BcEndDate"].ToString();
                        objScripHelpModel.SCSequenceId = Convert.ToInt64(oSQLiteDataReader["SCSequenceId"]);

                        ObjEquityDataCollection.Add(objScripHelpModel);
                    }

                    SearchEquityTemplist = ObjEquityDataCollection.Cast<ScripHelpModel>().ToList();
                    NotifyPropertyChanged(nameof(ObjEquityDataCollection));
                }
                else if (selectedExchange == Common.Enumerations.Exchange.BSE.ToString() && selectedSegment == Common.Enumerations.Segment.Derivative.ToString() && cochecked == true)
                {
                    VisibilityBSEEquityGrid = "Hidden";
                    VisibilityBSEDerivativeCOGrid = "Visible";
                    VisibilityBSEDerivativeSPDGrid = "Hidden";
                    VisibilityBSECurrencyCOGrid = "Hidden";
                    VisibilityBSECurrencySPDGrid = "Hidden";
                    VisibilityBSEDebtGrid = "Hidden";
                    VisibilityNSEEquityGrid = "Hidden";
                    VisibilityNSEDerivativeCOGrid = "Hidden";
                    VisibilityNSECurrencyCOGrid = "Hidden";
                    VisibilityNCDEXCommoditiesGrid = "Hidden";
                    VisibilityMCXCommoditiesGrid = "Hidden";
                    ISINCodeVisibility = "Hidden";
                    double precision = 0;

                    string strQuery = @"SELECT BOWID, ContractTokenNum, AssestTokenNum, InstrumentType, AssetCd, UnderlyingAsset, ExpiryDate,
                                        StrikePrice, OptionType, Precision, PartitionID, ProductID, CapacityGroupID, ScripID, MinimumLotSize, TickSize,
                                        InstrumentName, QuantityMultiplier, UnderlyingMarket, ContractType, ProductCode, BasePrice, DeleteFlag
                                        FROM BSE_DERIVATIVE_CO_CFE where ComplexInstrumentType!=2 and ComplexInstrumentType!=5;";

                    //                  string strQuery = @"SELECT BOWID,
                    //     ContractTokenNum,
                    //     AssestTokenNum,
                    //     InstrumentType,
                    //     AssetCd,
                    //     UnderlyingAsset,
                    //     ExpiryDate,
                    //     StrikePrice,
                    //     OptionType,
                    //     Precision,
                    //     PartitionID,
                    //     ProductID,
                    //     CapacityGroupID,
                    //     MinimumLotSize,
                    //     TickSize,
                    //     InstrumentName,
                    //     QuantityMultiplier,
                    //     UnderlyingMarket,
                    //     ContractType,
                    //     ProductCode,
                    //     BasePrice,
                    //     DeleteFlag,
                    //     ScripID,
                    //     StrategyID,
                    //     ComplexInstrumentType
                    //FROM BSE_DERIVATIVE_CO_CFE where ComplexInstrumentType!=2 and ComplexInstrumentType!=5;";
                    //ObjBSEDerivativeCOCollection = new ObservableCollection<ScripHelpBSEDerivativeCO>();
                    //ObjBSEDerivativeCOCollection = new ObservableCollection<ScripHelpBSEDerivativeCO>();
                    ObjBSEDerivativeCOCollection = new ObservableCollection<ScripHelpBSEDerivativeCO>();
                    SQLiteDataReader oSQLiteDataReader = oDataAccessLayer.ExecuteDataReader((int)ConnectionDB.Masters,strQuery, System.Data.CommandType.Text, null);
                    try
                    {
                        while (oSQLiteDataReader.Read())
                        {
                            try
                            {
                                ScripHelpBSEDerivativeCO objScripHelpBSEDerivativeCO = new ScripHelpBSEDerivativeCO();
                                objScripHelpBSEDerivativeCO.BOWID = Convert.ToInt64(oSQLiteDataReader["BOWID"]);
                                objScripHelpBSEDerivativeCO.ContractTokenNum = Convert.ToInt32(oSQLiteDataReader["ContractTokenNum"]);
                                objScripHelpBSEDerivativeCO.AssestTokenNum = Convert.ToInt32(oSQLiteDataReader["AssestTokenNum"]);
                                objScripHelpBSEDerivativeCO.AssetCd = oSQLiteDataReader["AssetCd"].ToString();
                                objScripHelpBSEDerivativeCO.UnderlyingAsset = oSQLiteDataReader["UnderlyingAsset"].ToString();
                                objScripHelpBSEDerivativeCO.ExpiryDate = oSQLiteDataReader["ExpiryDate"].ToString();
                                objScripHelpBSEDerivativeCO.OptionType = oSQLiteDataReader["OptionType"].ToString();
                                objScripHelpBSEDerivativeCO.Precision = Convert.ToInt32(oSQLiteDataReader["Precision"]);
                                precision = DivideByPrecision(objScripHelpBSEDerivativeCO.Precision);
                                objScripHelpBSEDerivativeCO.StrikePrice = Convert.ToDecimal(Convert.ToInt32(oSQLiteDataReader["StrikePrice"]) / precision);
                                objScripHelpBSEDerivativeCO.PartitionID = Convert.ToInt32(oSQLiteDataReader["PartitionID"]);
                                objScripHelpBSEDerivativeCO.ProductID = Convert.ToInt32(oSQLiteDataReader["ProductID"]);
                                objScripHelpBSEDerivativeCO.CapacityGroupID = Convert.ToInt32(oSQLiteDataReader["CapacityGroupID"]);
                                objScripHelpBSEDerivativeCO.MinimumLotSize = Convert.ToInt32(oSQLiteDataReader["MinimumLotSize"]);
                                objScripHelpBSEDerivativeCO.TickSize = Convert.ToDecimal(Convert.ToInt32(oSQLiteDataReader["TickSize"]) / precision);
                                objScripHelpBSEDerivativeCO.InstrumentName = oSQLiteDataReader["InstrumentName"].ToString();
                                objScripHelpBSEDerivativeCO.QuantityMultiplier = Convert.ToInt32(oSQLiteDataReader["QuantityMultiplier"]);
                                objScripHelpBSEDerivativeCO.UnderlyingMarket = Convert.ToInt32(oSQLiteDataReader["UnderlyingMarket"]);
                                objScripHelpBSEDerivativeCO.ContractType = Convert.ToString(oSQLiteDataReader["ContractType"]);
                                objScripHelpBSEDerivativeCO.ProductCode = oSQLiteDataReader["ProductCode"].ToString();
                                objScripHelpBSEDerivativeCO.BasePrice = Convert.ToDecimal(Convert.ToInt32(oSQLiteDataReader["BasePrice"]) / precision);
                                objScripHelpBSEDerivativeCO.DeleteFlag = Convert.ToChar(oSQLiteDataReader["DeleteFlag"]);
                                objScripHelpBSEDerivativeCO.ScripID = Convert.ToString(oSQLiteDataReader["ScripID"]);

                                ObjBSEDerivativeCOCollection.Add(objScripHelpBSEDerivativeCO);
                            }
                            catch (Exception e)
                            {

                                throw e;
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtility.LogError(ex);
                    }

                    finally
                    {
                        SearchDerCOTemplist = ObjBSEDerivativeCOCollection.Cast<ScripHelpBSEDerivativeCO>().ToList();
                        NotifyPropertyChanged(nameof(ObjBSEDerivativeCOCollection));
                    }
                }
                else if (selectedExchange == Common.Enumerations.Exchange.BSE.ToString() && selectedSegment == Common.Enumerations.Segment.Derivative.ToString() && spdchecked == true)
                {
                    VisibilityBSEEquityGrid = "Hidden";
                    VisibilityBSEDerivativeCOGrid = "Hidden";
                    VisibilityBSEDerivativeSPDGrid = "Visible";
                    VisibilityBSECurrencyCOGrid = "Hidden";
                    VisibilityBSECurrencySPDGrid = "Hidden";
                    VisibilityBSEDebtGrid = "Hidden";
                    VisibilityNSEEquityGrid = "Hidden";
                    VisibilityNSEDerivativeCOGrid = "Hidden";
                    VisibilityNSECurrencyCOGrid = "Hidden";
                    VisibilityNCDEXCommoditiesGrid = "Hidden";
                    VisibilityMCXCommoditiesGrid = "Hidden";
                    ISINCodeVisibility = "Hidden";
                    double precision = 0;

                    //ObjBSEDerivativeSPDCollection = new ObservableCollection<ScripHelpBSEDerivativeSPD>();
                    //string strQuery = @"SELECT ContractTokenNum, ContractTokenNum_Leg1, ContractTokenNum_Leg2, NTAScripCode, StrategyID,
                    //                    NoofLegsinStrategy, Eligibility, ComplexInstrumentType FROM BSE_DERIVATIVE_SPD_CFE;";
                    //SQLiteDataReader oSQLiteDataReader = oDataAccessLayer.ExecuteDataReader((int)ConnectionDB.Masters,strQuery, System.Data.CommandType.Text, null);



                    //try
                    //{
                    //    while (oSQLiteDataReader.Read())
                    //    {
                    //        ScripHelpBSEDerivativeSPD objScripHelpBSEDerivativeSPD = new ScripHelpBSEDerivativeSPD();
                    //        objScripHelpBSEDerivativeSPD.ContractTokenNum = Convert.ToInt32(oSQLiteDataReader["ContractTokenNum"]);
                    //        objScripHelpBSEDerivativeSPD.ContractTokenNum_Leg1 = Convert.ToInt32(oSQLiteDataReader["ContractTokenNum_Leg1"]);
                    //        objScripHelpBSEDerivativeSPD.ContractTokenNum_Leg2 = Convert.ToInt32(oSQLiteDataReader["ContractTokenNum_Leg2"]);
                    //        objScripHelpBSEDerivativeSPD.ScripCode = Convert.ToInt64(oSQLiteDataReader["NTAScripCode"]);
                    //        objScripHelpBSEDerivativeSPD.StrategyID = Convert.ToInt32(oSQLiteDataReader["StrategyID"]);
                    //        objScripHelpBSEDerivativeSPD.NoofLegsinStrategy = Convert.ToInt32(oSQLiteDataReader["NoofLegsinStrategy"]);
                    //        objScripHelpBSEDerivativeSPD.Eligibility = Convert.ToChar(oSQLiteDataReader["Eligibility"]);
                    //        objScripHelpBSEDerivativeSPD.ComplexInstrumentType = oSQLiteDataReader["ComplexInstrumentType"].ToString();
                    //        ObjBSEDerivativeSPDCollection.Add(objScripHelpBSEDerivativeSPD);
                    //    }

                    //}
                    //catch (Exception ex)
                    //{

                    //    ExceptionUtility.LogError(ex);
                    //}

                    //finally
                    //{     
                    //    SearchDerSPDTemplist = ObjBSEDerivativeSPDCollection.Cast<ScripHelpBSEDerivativeSPD>().ToList();
                    //}

                    string strQuery = @"SELECT BOWID, A.ContractTokenNum, AssestTokenNum, InstrumentType, AssetCd, UnderlyingAsset, ExpiryDate,
                                        StrikePrice, OptionType, Precision, PartitionID, ProductID, CapacityGroupID, MinimumLotSize, TickSize,
                                        InstrumentName, QuantityMultiplier, UnderlyingMarket, ContractType, ProductCode, BasePrice, DeleteFlag
                                        ,ScripID, ContractTokenNum_Leg1, ContractTokenNum_Leg2, NTAScripCode, B.StrategyID, NoofLegsinStrategy, Eligibility
                                        FROM BSE_DERIVATIVE_CO_CFE A, BSE_DERIVATIVE_SPD_CFE B where A.ContractTokenNum==B.ContractTokenNum;";
                    //ObjBSEDerivativeCOCollection = new ObservableCollection<ScripHelpBSEDerivativeCO>();
                    //ObjBSEDerivativeCOCollection = new ObservableCollection<ScripHelpBSEDerivativeCO>();
                    ObjBSEDerivativeCOCollection = new ObservableCollection<ScripHelpBSEDerivativeCO>();
                    SQLiteDataReader oSQLiteDataReader = oDataAccessLayer.ExecuteDataReader((int)ConnectionDB.Masters,strQuery, System.Data.CommandType.Text, null);
                    try
                    {
                        while (oSQLiteDataReader.Read())
                        {
                            try
                            {
                                ScripHelpBSEDerivativeCO objScripHelpBSEDerivativeCO = new ScripHelpBSEDerivativeCO();
                                objScripHelpBSEDerivativeCO.BOWID = Convert.ToInt64(oSQLiteDataReader["BOWID"]);
                                objScripHelpBSEDerivativeCO.ContractTokenNum = Convert.ToInt32(oSQLiteDataReader["ContractTokenNum"]);
                                objScripHelpBSEDerivativeCO.AssestTokenNum = Convert.ToInt32(oSQLiteDataReader["AssestTokenNum"]);
                                objScripHelpBSEDerivativeCO.AssetCd = oSQLiteDataReader["AssetCd"].ToString();
                                objScripHelpBSEDerivativeCO.UnderlyingAsset = oSQLiteDataReader["UnderlyingAsset"].ToString();
                                objScripHelpBSEDerivativeCO.ExpiryDate = oSQLiteDataReader["ExpiryDate"].ToString();
                                objScripHelpBSEDerivativeCO.OptionType = oSQLiteDataReader["OptionType"].ToString();
                                objScripHelpBSEDerivativeCO.Precision = Convert.ToInt32(oSQLiteDataReader["Precision"]);
                                precision = DivideByPrecision(objScripHelpBSEDerivativeCO.Precision);
                                objScripHelpBSEDerivativeCO.StrikePrice = Convert.ToDecimal(Convert.ToInt32(oSQLiteDataReader["StrikePrice"]) / precision);
                                objScripHelpBSEDerivativeCO.PartitionID = Convert.ToInt32(oSQLiteDataReader["PartitionID"]);
                                objScripHelpBSEDerivativeCO.ProductID = Convert.ToInt32(oSQLiteDataReader["ProductID"]);
                                objScripHelpBSEDerivativeCO.CapacityGroupID = Convert.ToInt32(oSQLiteDataReader["CapacityGroupID"]);
                                objScripHelpBSEDerivativeCO.MinimumLotSize = Convert.ToInt32(oSQLiteDataReader["MinimumLotSize"]);
                                objScripHelpBSEDerivativeCO.TickSize = Convert.ToDecimal(Convert.ToInt32(oSQLiteDataReader["TickSize"]) / precision);
                                objScripHelpBSEDerivativeCO.InstrumentName = oSQLiteDataReader["InstrumentName"].ToString();
                                objScripHelpBSEDerivativeCO.QuantityMultiplier = Convert.ToInt32(oSQLiteDataReader["QuantityMultiplier"]);
                                objScripHelpBSEDerivativeCO.UnderlyingMarket = Convert.ToInt32(oSQLiteDataReader["UnderlyingMarket"]);
                                objScripHelpBSEDerivativeCO.ContractType = Convert.ToString(oSQLiteDataReader["ContractType"]);
                                objScripHelpBSEDerivativeCO.ProductCode = oSQLiteDataReader["ProductCode"].ToString();
                                objScripHelpBSEDerivativeCO.BasePrice = Convert.ToDecimal(Convert.ToInt32(oSQLiteDataReader["BasePrice"]) / precision);
                                objScripHelpBSEDerivativeCO.DeleteFlag = Convert.ToChar(oSQLiteDataReader["DeleteFlag"]);
                                objScripHelpBSEDerivativeCO.ScripID = Convert.ToString(oSQLiteDataReader["ScripID"]);

                                //SPD Fields
                                objScripHelpBSEDerivativeCO.ContractTokenNum_Leg1 = Convert.ToInt32(oSQLiteDataReader["ContractTokenNum_Leg1"]);
                                objScripHelpBSEDerivativeCO.ContractTokenNum_Leg2 = Convert.ToInt32(oSQLiteDataReader["ContractTokenNum_Leg2"]);
                                objScripHelpBSEDerivativeCO.NTAScripCode = Convert.ToInt64(oSQLiteDataReader["NTAScripCode"]);
                                objScripHelpBSEDerivativeCO.StrategyID = Convert.ToInt32(oSQLiteDataReader["StrategyID"]);
                                objScripHelpBSEDerivativeCO.NoofLegsinStrategy = Convert.ToInt32(oSQLiteDataReader["NoofLegsinStrategy"]);
                                objScripHelpBSEDerivativeCO.Eligibility = Convert.ToString(oSQLiteDataReader["Eligibility"]);
                                //SPD Fields

                                ObjBSEDerivativeCOCollection.Add(objScripHelpBSEDerivativeCO);
                            }
                            catch (Exception e)
                            {

                                throw e;
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtility.LogError(ex);
                    }

                    finally
                    {
                        SearchDerCOTemplist = ObjBSEDerivativeCOCollection.Cast<ScripHelpBSEDerivativeCO>().ToList();
                        NotifyPropertyChanged(nameof(ObjBSEDerivativeCOCollection));
                    }


                }
                else if (selectedExchange == Common.Enumerations.Exchange.BSE.ToString() && selectedSegment == Common.Enumerations.Segment.Currency.ToString() && cochecked == true)
                {
                    VisibilityBSEEquityGrid = "Hidden";
                    VisibilityBSEDerivativeCOGrid = "Hidden";
                    VisibilityBSEDerivativeSPDGrid = "Hidden";
                    VisibilityBSECurrencyCOGrid = "Visible";
                    VisibilityBSECurrencySPDGrid = "Hidden";
                    VisibilityBSEDebtGrid = "Hidden";
                    VisibilityNSEEquityGrid = "Hidden";
                    VisibilityNSEDerivativeCOGrid = "Hidden";
                    VisibilityNSECurrencyCOGrid = "Hidden";
                    VisibilityNCDEXCommoditiesGrid = "Hidden";
                    VisibilityMCXCommoditiesGrid = "Hidden";
                    ISINCodeVisibility = "Hidden";
                    double precision = 0;
                    // ObjEquityDataCollection = new ObservableCollection<ScripHelpModel>();
                    string strQuery = @"SELECT BOWID, ContractTokenNum, AssestTokenNum, InstrumentType, AssetCD, ExpiryDate, StrikePrice, OptionType, Precision, PartitionID,
                                        ProductID, CapacityGroupID, MinimumLotSize, TickSize, InstrumentName, QuantityMultiplier, UnderlyingMarket,
                                        ContractType, BasePrice, ScripID, DeleteFlag FROM BSE_CURRENCY_CO_CFE where ComplexInstrumentType!=2 and ComplexInstrumentType!=5;";

                    try
                    {
                        ObjBSECurrencyCOCollection = new ObservableCollection<ScripHelpBSECurrencyCO>();
                        SQLiteDataReader oSQLiteDataReader = oDataAccessLayer.ExecuteDataReader((int)ConnectionDB.Masters,strQuery, System.Data.CommandType.Text, null);
                        while (oSQLiteDataReader.Read())
                        {
                            ScripHelpBSECurrencyCO objScripHelpBSECurrencyCO = new ScripHelpBSECurrencyCO();
                            objScripHelpBSECurrencyCO.BOWID = Convert.ToInt32(oSQLiteDataReader["BOWID"]);
                            objScripHelpBSECurrencyCO.ContractTokenNum = Convert.ToInt32(oSQLiteDataReader["ContractTokenNum"]);
                            objScripHelpBSECurrencyCO.AssestTokenNum = Convert.ToInt32(oSQLiteDataReader["AssestTokenNum"]);
                            objScripHelpBSECurrencyCO.InstrumentType = oSQLiteDataReader["InstrumentType"].ToString();
                            objScripHelpBSECurrencyCO.AssetCD = oSQLiteDataReader["AssetCd"].ToString();
                            objScripHelpBSECurrencyCO.Precision = Convert.ToChar(oSQLiteDataReader["Precision"]);
                            precision = DivideByPrecision(objScripHelpBSECurrencyCO.Precision);
                            objScripHelpBSECurrencyCO.ExpiryDate = oSQLiteDataReader["ExpiryDate"].ToString();
                            objScripHelpBSECurrencyCO.StrikePrice = $"{Convert.ToInt32(oSQLiteDataReader["StrikePrice"]) / Math.Pow(10, 7):0.0000}";
                            objScripHelpBSECurrencyCO.OptionType = oSQLiteDataReader["OptionType"].ToString();
                            objScripHelpBSECurrencyCO.PartitionID = Convert.ToInt32(oSQLiteDataReader["PartitionID"]);
                            objScripHelpBSECurrencyCO.ProductID = Convert.ToInt32(oSQLiteDataReader["ProductID"]);
                            objScripHelpBSECurrencyCO.CapacityGroupID = Convert.ToInt32(oSQLiteDataReader["CapacityGroupID"]);
                            objScripHelpBSECurrencyCO.MinimumLotSize = Convert.ToInt32(oSQLiteDataReader["MinimumLotSize"]);
                            objScripHelpBSECurrencyCO.TickSize = Convert.ToInt32(oSQLiteDataReader["TickSize"]) / Math.Pow(10, 7);
                            objScripHelpBSECurrencyCO.InstrumentName = oSQLiteDataReader["InstrumentName"].ToString();
                            objScripHelpBSECurrencyCO.QuantityMultiplier = Convert.ToInt32(oSQLiteDataReader["QuantityMultiplier"]);
                            objScripHelpBSECurrencyCO.UnderlyingMarket = Convert.ToInt32(oSQLiteDataReader["UnderlyingMarket"]);
                            objScripHelpBSECurrencyCO.ContractType = Convert.ToChar(oSQLiteDataReader["ContractType"]);
                            objScripHelpBSECurrencyCO.BasePrice = Convert.ToInt32(oSQLiteDataReader["BasePrice"]) / Math.Pow(10, 7);
                            objScripHelpBSECurrencyCO.DeleteFlag = Convert.ToChar(oSQLiteDataReader["DeleteFlag"]);
                            objScripHelpBSECurrencyCO.ScripID = Convert.ToString(oSQLiteDataReader["ScripID"]);
                            ObjBSECurrencyCOCollection.Add(objScripHelpBSECurrencyCO);
                        }
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtility.LogError(ex);
                    }

                    finally
                    {
                        SearchCurCOTemplist = ObjBSECurrencyCOCollection.Cast<ScripHelpBSECurrencyCO>().ToList();
                        NotifyPropertyChanged(nameof(ObjBSECurrencyCOCollection));
                    }




                }
                else if (selectedExchange == Common.Enumerations.Exchange.BSE.ToString() && selectedSegment == Common.Enumerations.Segment.Currency.ToString() && spdchecked == true)
                {
                    VisibilityBSEEquityGrid = "Hidden";
                    VisibilityBSEDerivativeCOGrid = "Hidden";
                    VisibilityBSEDerivativeSPDGrid = "Hidden";
                    VisibilityBSECurrencyCOGrid = "Hidden";
                    VisibilityBSECurrencySPDGrid = "Visible";
                    VisibilityBSEDebtGrid = "Hidden";
                    VisibilityNSEEquityGrid = "Hidden";
                    VisibilityNSEDerivativeCOGrid = "Hidden";
                    VisibilityNSECurrencyCOGrid = "Hidden";
                    VisibilityNCDEXCommoditiesGrid = "Hidden";
                    VisibilityMCXCommoditiesGrid = "Hidden";
                    double precision = 0;
                    ISINCodeVisibility = "Hidden";
                    //ObjEquityDataCollection = new ObservableCollection<ScripHelpModel>();
                    //string strQuery = @"SELECT ContractTokenNum, ContractTokenNum_Leg1, ContractTokenNum_Leg2, NTAScripCode, StrategyID, NoofLegsinStrategy,
                    //                    Eligibility, ComplexInstrumentType FROM BSE_CURRENCY_SPD_CFE;";
                    //SQLiteDataReader oSQLiteDataReader = oDataAccessLayer.ExecuteDataReader((int)ConnectionDB.Masters,strQuery, System.Data.CommandType.Text, null);
                    //while (oSQLiteDataReader.Read())
                    //{
                    //    ScripHelpBSECurrencySPD objScripHelpBSECurrencySPD = new ScripHelpBSECurrencySPD();
                    //    objScripHelpBSECurrencySPD.ContractTokenNum = Convert.ToInt32(oSQLiteDataReader["ContractTokenNum"]);
                    //    objScripHelpBSECurrencySPD.ContractTokenNum_Leg1 = Convert.ToInt32(oSQLiteDataReader["ContractTokenNum_Leg1"]);
                    //    objScripHelpBSECurrencySPD.ContractTokenNum_Leg2 = Convert.ToInt32(oSQLiteDataReader["ContractTokenNum_Leg2"]);
                    //    objScripHelpBSECurrencySPD.ScripCode = Convert.ToInt64(oSQLiteDataReader["NTAScripCode"]);
                    //    objScripHelpBSECurrencySPD.StrategyID = Convert.ToInt32(oSQLiteDataReader["StrategyID"]);
                    //    objScripHelpBSECurrencySPD.NoofLegsinStrategy = Convert.ToInt32(oSQLiteDataReader["NoofLegsinStrategy"]);
                    //    objScripHelpBSECurrencySPD.Eligibility = Convert.ToChar(oSQLiteDataReader["Eligibility"]);
                    //    objScripHelpBSECurrencySPD.ComplexInstrumentType = oSQLiteDataReader["ComplexInstrumentType"].ToString();
                    //    ObjBSECurrencySPDCollection.Add(objScripHelpBSECurrencySPD);
                    //}

                    //SearchCurSPDTemplist = ObjBSECurrencySPDCollection.Cast<ScripHelpBSECurrencySPD>().ToList();


                    string strQuery = @"SELECT BOWID, A.ContractTokenNum, AssestTokenNum, InstrumentType, AssetCD, ExpiryDate, StrikePrice, OptionType, Precision, PartitionID,
                                        ProductID, CapacityGroupID, MinimumLotSize, TickSize, InstrumentName, QuantityMultiplier, UnderlyingMarket,
                                        ContractType, BasePrice, DeleteFlag, ScripID,ContractTokenNum_Leg1,ContractTokenNum_Leg2,NTAScripCode,B.StrategyID,NoofLegsinStrategy,Eligibility
                                        FROM BSE_CURRENCY_CO_CFE A, BSE_CURRENCY_SPD_CFE B where A.ContractTokenNum==B.ContractTokenNum;";

                    try
                    {
                        ObjBSECurrencyCOCollection = new ObservableCollection<ScripHelpBSECurrencyCO>();
                        SQLiteDataReader oSQLiteDataReader = oDataAccessLayer.ExecuteDataReader((int)ConnectionDB.Masters,strQuery, System.Data.CommandType.Text, null);
                        while (oSQLiteDataReader.Read())
                        {
                            ScripHelpBSECurrencyCO objScripHelpBSECurrencyCO = new ScripHelpBSECurrencyCO();
                            objScripHelpBSECurrencyCO.BOWID = Convert.ToInt32(oSQLiteDataReader["BOWID"]);
                            objScripHelpBSECurrencyCO.ContractTokenNum = Convert.ToInt32(oSQLiteDataReader["ContractTokenNum"]);
                            objScripHelpBSECurrencyCO.AssestTokenNum = Convert.ToInt32(oSQLiteDataReader["AssestTokenNum"]);
                            objScripHelpBSECurrencyCO.InstrumentType = oSQLiteDataReader["InstrumentType"].ToString();
                            objScripHelpBSECurrencyCO.AssetCD = oSQLiteDataReader["AssetCd"].ToString();
                            objScripHelpBSECurrencyCO.Precision = Convert.ToChar(oSQLiteDataReader["Precision"]);
                            precision = DivideByPrecision(objScripHelpBSECurrencyCO.Precision);
                            objScripHelpBSECurrencyCO.ExpiryDate = oSQLiteDataReader["ExpiryDate"].ToString();
                            objScripHelpBSECurrencyCO.StrikePrice = $"{Convert.ToInt32(oSQLiteDataReader["StrikePrice"]) / Math.Pow(10, 7):0.0000}";
                            objScripHelpBSECurrencyCO.OptionType = oSQLiteDataReader["OptionType"].ToString();
                            objScripHelpBSECurrencyCO.PartitionID = Convert.ToInt32(oSQLiteDataReader["PartitionID"]);
                            objScripHelpBSECurrencyCO.ProductID = Convert.ToInt32(oSQLiteDataReader["ProductID"]);
                            objScripHelpBSECurrencyCO.CapacityGroupID = Convert.ToInt32(oSQLiteDataReader["CapacityGroupID"]);
                            objScripHelpBSECurrencyCO.MinimumLotSize = Convert.ToInt32(oSQLiteDataReader["MinimumLotSize"]);
                            objScripHelpBSECurrencyCO.TickSize = Convert.ToInt32(oSQLiteDataReader["TickSize"]) / Math.Pow(10, 7);
                            objScripHelpBSECurrencyCO.InstrumentName = oSQLiteDataReader["InstrumentName"].ToString();
                            objScripHelpBSECurrencyCO.QuantityMultiplier = Convert.ToInt32(oSQLiteDataReader["QuantityMultiplier"]);
                            objScripHelpBSECurrencyCO.UnderlyingMarket = Convert.ToInt32(oSQLiteDataReader["UnderlyingMarket"]);
                            objScripHelpBSECurrencyCO.ContractType = Convert.ToChar(oSQLiteDataReader["ContractType"]);
                            objScripHelpBSECurrencyCO.BasePrice = Convert.ToInt32(oSQLiteDataReader["BasePrice"]) / Math.Pow(10, 7);
                            objScripHelpBSECurrencyCO.DeleteFlag = Convert.ToChar(oSQLiteDataReader["DeleteFlag"]);
                            objScripHelpBSECurrencyCO.ScripID = Convert.ToString(oSQLiteDataReader["ScripID"]);

                            //SPD Fields
                            objScripHelpBSECurrencyCO.ContractTokenNum_Leg1 = Convert.ToInt32(oSQLiteDataReader["ContractTokenNum_Leg1"]);
                            objScripHelpBSECurrencyCO.ContractTokenNum_Leg2 = Convert.ToInt32(oSQLiteDataReader["ContractTokenNum_Leg2"]);
                            objScripHelpBSECurrencyCO.NTAScripCode = Convert.ToInt64(oSQLiteDataReader["NTAScripCode"]);
                            objScripHelpBSECurrencyCO.StrategyID = Convert.ToInt32(oSQLiteDataReader["StrategyID"]);
                            objScripHelpBSECurrencyCO.NoofLegsinStrategy = Convert.ToInt32(oSQLiteDataReader["NoofLegsinStrategy"]);
                            objScripHelpBSECurrencyCO.Eligibility = Convert.ToString(oSQLiteDataReader["Eligibility"]);
                            //SPD Fields

                            ObjBSECurrencyCOCollection.Add(objScripHelpBSECurrencyCO);
                        }
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtility.LogError(ex);
                    }

                    finally
                    {
                        SearchCurCOTemplist = ObjBSECurrencyCOCollection.Cast<ScripHelpBSECurrencyCO>().ToList();
                        NotifyPropertyChanged(nameof(ObjBSECurrencyCOCollection));
                    }

                }
                else if (selectedExchange == Common.Enumerations.Exchange.BSE.ToString() && selectedSegment == Common.Enumerations.Segment.Debt.ToString())
                {
                    VisibilityBSEEquityGrid = "Hidden";
                    VisibilityBSEDerivativeCOGrid = "Hidden";
                    VisibilityBSEDerivativeSPDGrid = "Hidden";
                    VisibilityBSECurrencyCOGrid = "Hidden";
                    VisibilityBSECurrencySPDGrid = "Hidden";
                    VisibilityBSEDebtGrid = "Visible";
                    VisibilityNSEEquityGrid = "Hidden";
                    VisibilityNSEDerivativeCOGrid = "Hidden";
                    VisibilityNSECurrencyCOGrid = "Hidden";
                    VisibilityNCDEXCommoditiesGrid = "Hidden";
                    VisibilityMCXCommoditiesGrid = "Hidden";
                    ISINCodeVisibility = "Visible";

                    string strQuery = @"SELECT SCID, ScripCode, ExchangeCode, ScripId, ScripName, PartitionId, InstrumentType, GroupName, ScripType, FaceValue, MarketLot, TickSize, BseExclusive, Status, ExDivDate, NoDelEndDate, NoDelStartDate, NewTickSize, IsinCode, CallAuctionIndicator, BcStartDate, ExBonusDate, ExRightDate,  Filler, Filler2_GSM, BcEndDate, Filler3, SCSequenceId  FROM BSE_SECURITIES_CFE Where InstrumentType = 'D' OR InstrumentType = 'C';";
                    try
                    {
                        SQLiteDataReader oSQLiteDataReader = oDataAccessLayer.ExecuteDataReader((int)ConnectionDB.Masters,strQuery, System.Data.CommandType.Text, null);
                        ObjDebtDataCollection = new ObservableCollection<ScripHelpModel>();
                        while (oSQLiteDataReader.Read())
                        {
                            ScripHelpModel objScripHelpModelDebt = new ScripHelpModel();
                            objScripHelpModelDebt.SCID = Convert.ToInt64(oSQLiteDataReader["SCID"]);
                            objScripHelpModelDebt.ScripCode = Convert.ToInt32(oSQLiteDataReader["ScripCode"]);
                            objScripHelpModelDebt.ExchangeCode = oSQLiteDataReader["ExchangeCode"].ToString();
                            objScripHelpModelDebt.ScripId = oSQLiteDataReader["ScripId"].ToString();
                            objScripHelpModelDebt.ScripName = oSQLiteDataReader["ScripName"].ToString();
                            // objScripHelpModelDebt.PartitionId = oSQLiteDataReader["PartitionId"].ToString();
                            string[] arr = oSQLiteDataReader["PartitionId"].ToString().Split('-');
                            objScripHelpModelDebt.PartitionId = arr[0];
                            objScripHelpModelDebt.ProductId = arr[1];
                            objScripHelpModelDebt.InstrumentType = Convert.ToChar(oSQLiteDataReader["InstrumentType"]);
                            objScripHelpModelDebt.GroupName = oSQLiteDataReader["GroupName"].ToString();
                            objScripHelpModelDebt.ScripType = Convert.ToChar(oSQLiteDataReader["ScripType"]);
                            objScripHelpModelDebt.FaceValue = Convert.ToInt64(oSQLiteDataReader["FaceValue"]) / 100;
                            objScripHelpModelDebt.MarketLot = Convert.ToInt32(oSQLiteDataReader["MarketLot"]);
                            objScripHelpModelDebt.TickSize = Convert.ToDouble(oSQLiteDataReader["TickSize"]);
                            objScripHelpModelDebt.BseExclusive = Convert.ToChar(oSQLiteDataReader["BseExclusive"]);
                            objScripHelpModelDebt.Status = Convert.ToChar(oSQLiteDataReader["Status"]);
                            objScripHelpModelDebt.ExDivDate = oSQLiteDataReader["ExDivDate"].ToString();
                            objScripHelpModelDebt.NoDelEndDate = oSQLiteDataReader["NoDelEndDate"].ToString();
                            objScripHelpModelDebt.NoDelStartDate = oSQLiteDataReader["NoDelStartDate"].ToString();
                            objScripHelpModelDebt.NewTickSize = oSQLiteDataReader["NewTickSize"].ToString();
                            objScripHelpModelDebt.IsinCode = oSQLiteDataReader["IsinCode"].ToString();
                            objScripHelpModelDebt.CallAuctionIndicator = Convert.ToInt32(oSQLiteDataReader["CallAuctionIndicator"]);
                            objScripHelpModelDebt.BcStartDate = oSQLiteDataReader["BcStartDate"].ToString();
                            objScripHelpModelDebt.ExBonusDate = oSQLiteDataReader["ExBonusDate"].ToString();
                            objScripHelpModelDebt.ExRightDate = oSQLiteDataReader["ExRightDate"].ToString();
                            objScripHelpModelDebt.Filler2_GSM = Convert.ToInt32(oSQLiteDataReader["Filler2_GSM"]);
                            objScripHelpModelDebt.BcEndDate = oSQLiteDataReader["BcEndDate"].ToString();
                            objScripHelpModelDebt.SCSequenceId = Convert.ToInt64(oSQLiteDataReader["SCSequenceId"]);

                            ObjDebtDataCollection.Add(objScripHelpModelDebt);
                        }
                    }
                    catch (Exception ex)
                    {

                        ExceptionUtility.LogError(ex);
                    }
                    finally
                    {
                        SearchDebtTemplist = ObjDebtDataCollection.Cast<ScripHelpModel>().ToList();
                        NotifyPropertyChanged(nameof(ObjDebtDataCollection));
                    }
                }
#if BOW
                else if (selectedExchange == Common.Enumerations.Exchange.NSE.ToString() && selectedSegment == Common.Enumerations.Segment.Equity.ToString())
                {
                    VisibilityBSEEquityGrid = "Hidden";
                    VisibilityBSEDerivativeCOGrid = "Hidden";
                    VisibilityBSEDerivativeSPDGrid = "Hidden";
                    VisibilityBSECurrencyCOGrid = "Hidden";
                    VisibilityBSECurrencySPDGrid = "Hidden";
                    VisibilityBSEDebtGrid = "Hidden";
                    VisibilityNSEEquityGrid = "Visible";
                    VisibilityNSEDerivativeCOGrid = "Hidden";
                    VisibilityNSECurrencyCOGrid = "Hidden";
                    VisibilityNCDEXCommoditiesGrid = "Hidden";
                    VisibilityMCXCommoditiesGrid = "Hidden";
                    string strQuery = @"SELECT SCID, ScripCode, ExchangeCode, ScripId, ScripName, GroupName, FaceValue, MarketLot, TickSize,
                                        Status, IsinCode, CallAuctionIndicator, SCSequenceId FROM NSE_SECURITIES_CFE;";

                    SQLiteDataReader oSQLiteDataReader = oDataAccessLayer.ExecuteDataReader((int)ConnectionDB.Masters,strQuery, System.Data.CommandType.Text, null);
                    while (oSQLiteDataReader.Read())
                    {
                        ScripHelpNSEEquity objScripHelpNSEEquity = new ScripHelpNSEEquity();
                        objScripHelpNSEEquity.SCID = Convert.ToInt64(oSQLiteDataReader["SCID"]);
                        objScripHelpNSEEquity.ScripCode = Convert.ToInt32(oSQLiteDataReader["ScripCode"]);
                        objScripHelpNSEEquity.ExchangeCode = oSQLiteDataReader["ExchangeCode"].ToString();
                        objScripHelpNSEEquity.ScripId = oSQLiteDataReader["ScripId"].ToString();
                        objScripHelpNSEEquity.ScripName = oSQLiteDataReader["ScripName"].ToString();
                        objScripHelpNSEEquity.GroupName = oSQLiteDataReader["GroupName"].ToString();
                        objScripHelpNSEEquity.FaceValue = Convert.ToInt32(oSQLiteDataReader["FaceValue"]);
                        objScripHelpNSEEquity.MarketLot = Convert.ToInt32(oSQLiteDataReader["MarketLot"]);
                        objScripHelpNSEEquity.TickSize = Convert.ToDouble(oSQLiteDataReader["MarketLot"]);
                        objScripHelpNSEEquity.Status = Convert.ToChar(oSQLiteDataReader["Status"]);
                        objScripHelpNSEEquity.IsinCode = oSQLiteDataReader["IsinCode"].ToString();
                        objScripHelpNSEEquity.CallAuctionIndicator = Convert.ToInt32(oSQLiteDataReader["CallAuctionIndicator"]);
                        objScripHelpNSEEquity.SCSequenceId = Convert.ToInt64(oSQLiteDataReader["SCSequenceId"]);

                        ObjNSEEquityDataCollection.Add(objScripHelpNSEEquity);
                    }
                }
                else if (selectedExchange == Common.Enumerations.Exchange.NSE.ToString() && selectedSegment == Common.Enumerations.Segment.Derivative.ToString() && cochecked == true)
                {
                    VisibilityBSEEquityGrid = "Hidden";
                    VisibilityBSEDerivativeCOGrid = "Hidden";
                    VisibilityBSEDerivativeSPDGrid = "Hidden";
                    VisibilityBSECurrencyCOGrid = "Hidden";
                    VisibilityBSECurrencySPDGrid = "Hidden";
                    VisibilityBSEDebtGrid = "Hidden";
                    VisibilityNSEEquityGrid = "Hidden";
                    VisibilityNSEDerivativeCOGrid = "Hidden";
                    VisibilityNSEDerivativeCOGrid = "Visible";
                    VisibilityNSECurrencyCOGrid = "Hidden";
                    VisibilityNCDEXCommoditiesGrid = "Hidden";
                    VisibilityMCXCommoditiesGrid = "Hidden";
                    string strQuery = @"SELECT BOWID, ContractTokenNum, AssestTokenNum, InstrumentType, UnderlyingAsset, ExpiryDate, StrikePrice,
                                    OptionType, Precision, MinimumLotSize, TickSize, InstrumentName, QuantityMultiplier, DeleteFlag, InBannedPeriod
                                    FROM NSE_DERIVATIVE_CO_CFE";
                    SQLiteDataReader oSQLiteDataReader = oDataAccessLayer.ExecuteDataReader((int)ConnectionDB.Masters,strQuery, System.Data.CommandType.Text, null);
                    while (oSQLiteDataReader.Read())
                    {
                        ScripHelpNSEDerivativeCO objScripHelpNSEDerivativeCO = new ScripHelpNSEDerivativeCO();
                        objScripHelpNSEDerivativeCO.BOWID = Convert.ToInt64(oSQLiteDataReader["BOWID"]);
                        objScripHelpNSEDerivativeCO.ContractTokenNum = Convert.ToInt32(oSQLiteDataReader["ContractTokenNum"]);
                        objScripHelpNSEDerivativeCO.AssestTokenNum = Convert.ToInt32(oSQLiteDataReader["AssestTokenNum"]);
                        objScripHelpNSEDerivativeCO.InstrumentType = oSQLiteDataReader["InstrumentType"].ToString();
                        objScripHelpNSEDerivativeCO.UnderlyingAsset = oSQLiteDataReader["UnderlyingAsset"].ToString();
                        objScripHelpNSEDerivativeCO.ExpiryDate = oSQLiteDataReader["ExpiryDate"].ToString();
                        objScripHelpNSEDerivativeCO.StrikePrice = Convert.ToInt32(oSQLiteDataReader["StrikePrice"]);
                        objScripHelpNSEDerivativeCO.OptionType = oSQLiteDataReader["OptionType"].ToString();
                        objScripHelpNSEDerivativeCO.Precision = oSQLiteDataReader["Precision"].ToString();
                        //objScripHelpNSEDerivativeCO.PartitionID = Convert.ToInt32(oSQLiteDataReader["PartitionID"]);
                        //objScripHelpNSEDerivativeCO.ProductID = Convert.ToInt32(oSQLiteDataReader["ProductID"]);
                        //objScripHelpNSEDerivativeCO.CapacityGroupID = Convert.ToInt32(oSQLiteDataReader["CapacityGroupID"]);
                        objScripHelpNSEDerivativeCO.MinimumLotSize = Convert.ToInt32(oSQLiteDataReader["MinimumLotSize"]);
                        objScripHelpNSEDerivativeCO.TickSize = Convert.ToInt32(oSQLiteDataReader["TickSize"]);
                        objScripHelpNSEDerivativeCO.InstrumentName = oSQLiteDataReader["InstrumentName"].ToString();
                        objScripHelpNSEDerivativeCO.QuantityMultiplier = Convert.ToInt32(oSQLiteDataReader["QuantityMultiplier"]);
                        objScripHelpNSEDerivativeCO.DeleteFlag = oSQLiteDataReader["DeleteFlag"].ToString();
                        objScripHelpNSEDerivativeCO.InBannedPeriod = oSQLiteDataReader["InBannedPeriod"].ToString();
                        ObjNSEDerivativeCOCollection.Add(objScripHelpNSEDerivativeCO);
                    }
                }
                else if (selectedExchange == Common.Enumerations.Exchange.NSE.ToString() && selectedSegment == Common.Enumerations.Segment.Currency.ToString() && cochecked == true)
                {
                    VisibilityBSEEquityGrid = "Hidden";
                    VisibilityBSEDerivativeCOGrid = "Hidden";
                    VisibilityBSEDerivativeSPDGrid = "Hidden";
                    VisibilityBSECurrencyCOGrid = "Hidden";
                    VisibilityBSECurrencySPDGrid = "Hidden";
                    VisibilityBSEDebtGrid = "Hidden";
                    VisibilityNSEEquityGrid = "Hidden";
                    VisibilityNSEDerivativeCOGrid = "Hidden";
                    VisibilityNSECurrencyCOGrid = "Visible";
                    VisibilityNCDEXCommoditiesGrid = "Hidden";
                    VisibilityMCXCommoditiesGrid = "Hidden";
                    string strQuery = @"SELECT BOWID, ContractTokenNum, AssestTokenNum, InstrumentType, UnderlyingAsset, ExpiryDate, StrikePrice,
                                    OptionType, Precision, MinimumLotSize, TickSize, InstrumentName, QuantityMultiplier, DeleteFlag, InBannedPeriod
                                    FROM NSE_CURRENCY_CO_CFE";
                    SQLiteDataReader oSQLiteDataReader = oDataAccessLayer.ExecuteDataReader((int)ConnectionDB.Masters,strQuery, System.Data.CommandType.Text, null);
                    while (oSQLiteDataReader.Read())
                    {
                        ScripHelpNSECurrencyCO objScripHelpNSECurrencyCO = new ScripHelpNSECurrencyCO();
                        objScripHelpNSECurrencyCO.BOWID = Convert.ToInt64(oSQLiteDataReader["BOWID"]);
                        objScripHelpNSECurrencyCO.ContractTokenNum = Convert.ToInt32(oSQLiteDataReader["ContractTokenNum"]);
                        objScripHelpNSECurrencyCO.AssestTokenNum = Convert.ToInt32(oSQLiteDataReader["AssestTokenNum"]);
                        objScripHelpNSECurrencyCO.InstrumentType = oSQLiteDataReader["InstrumentType"].ToString();
                        objScripHelpNSECurrencyCO.UnderlyingAsset = oSQLiteDataReader["UnderlyingAsset"].ToString();
                        objScripHelpNSECurrencyCO.ExpiryDate = oSQLiteDataReader["ExpiryDate"].ToString();
                        objScripHelpNSECurrencyCO.StrikePrice = Convert.ToInt32(oSQLiteDataReader["StrikePrice"]);
                        objScripHelpNSECurrencyCO.OptionType = oSQLiteDataReader["OptionType"].ToString();
                        objScripHelpNSECurrencyCO.Precision = oSQLiteDataReader["Precision"].ToString();
                        //objScripHelpNSECurrencyCOCO.PartitionID = Convert.ToInt32(oSQLiteDataReader["PartitionID"]);
                        //objScripHelpNSECurrencyCOCO.ProductID = Convert.ToInt32(oSQLiteDataReader["ProductID"]);
                        //objScripHelpNSECurrencyCOCO.CapacityGroupID = Convert.ToInt32(oSQLiteDataReader["CapacityGroupID"]);
                        objScripHelpNSECurrencyCO.MinimumLotSize = Convert.ToInt32(oSQLiteDataReader["MinimumLotSize"]);
                        objScripHelpNSECurrencyCO.TickSize = Convert.ToInt32(oSQLiteDataReader["TickSize"]);
                        objScripHelpNSECurrencyCO.InstrumentName = oSQLiteDataReader["InstrumentName"].ToString();
                        objScripHelpNSECurrencyCO.QuantityMultiplier = Convert.ToInt32(oSQLiteDataReader["QuantityMultiplier"]);
                        objScripHelpNSECurrencyCO.DeleteFlag = oSQLiteDataReader["DeleteFlag"].ToString();
                        objScripHelpNSECurrencyCO.InBannedPeriod = oSQLiteDataReader["InBannedPeriod"].ToString();
                        ObjNSECurrencyCOCollection.Add(objScripHelpNSECurrencyCO);
                    }
                }
                else if (selectedExchange == Common.Enumerations.Exchange.NCDEX.ToString() && selectedSegment == Common.Enumerations.Segment.Commodities.ToString())
                {
                    VisibilityBSEEquityGrid = "Hidden";
                    VisibilityBSEDerivativeCOGrid = "Hidden";
                    VisibilityBSEDerivativeSPDGrid = "Hidden";
                    VisibilityBSECurrencyCOGrid = "Hidden";
                    VisibilityBSECurrencySPDGrid = "Hidden";
                    VisibilityBSEDebtGrid = "Hidden";
                    VisibilityNSEEquityGrid = "Hidden";
                    VisibilityNSEDerivativeCOGrid = "Hidden";
                    VisibilityNSECurrencyCOGrid = "Hidden";
                    VisibilityNCDEXCommoditiesGrid = "Visible";
                    VisibilityMCXCommoditiesGrid = "Hidden";
                    string strQuery = @"SELECT NCDID, NCDToken, NCDInstrumentName, NCDSymbol, NCDExpiryDate, NCDDisplayExpiryDate, NCDStrikePrice,
                        NCDOptionType, NCDBoardLotQuantity, NCDTickSize, NCDName, NCDExerciseStartDate, NCDExerciseEndDate, NCDPriceNumerator,
                        NCDPriceDenominator, NCDPriceUnit, NCDQuantityUnit, NCDDeliveryUnit, NCDDeliveryLotQuantity, NCDSequenceId, NCDTokenString
                        FROM NCDEX_COMMODITY_CFE;";
                    SQLiteDataReader oSQLiteDataReader = oDataAccessLayer.ExecuteDataReader((int)ConnectionDB.Masters,strQuery, System.Data.CommandType.Text, null);
                    while (oSQLiteDataReader.Read())
                    {
                        ScripHelpNCDEXCommodities objScripHelpNCDEXCommodities = new ScripHelpNCDEXCommodities();
                        objScripHelpNCDEXCommodities.NCDID = Convert.ToInt64(oSQLiteDataReader["NCDID"]);
                        objScripHelpNCDEXCommodities.NCDToken = Convert.ToInt64(oSQLiteDataReader["NCDToken"]);
                        objScripHelpNCDEXCommodities.NCDInstrumentName = oSQLiteDataReader["NCDInstrumentName"].ToString();
                        objScripHelpNCDEXCommodities.NCDSymbol = oSQLiteDataReader["NCDSymbol"].ToString();
                        objScripHelpNCDEXCommodities.NCDExpiryDate = Convert.ToInt64(oSQLiteDataReader["NCDExpiryDate"]);
                        objScripHelpNCDEXCommodities.NCDDisplayExpiryDate = oSQLiteDataReader["NCDDisplayExpiryDate"].ToString();
                        objScripHelpNCDEXCommodities.NCDStrikePrice = Convert.ToInt64(oSQLiteDataReader["NCDStrikePrice"]);
                        objScripHelpNCDEXCommodities.NCDOptionType = oSQLiteDataReader["NCDOptionType"].ToString();
                        objScripHelpNCDEXCommodities.NCDBoardLotQuantity = Convert.ToInt64(oSQLiteDataReader["NCDBoardLotQuantity"]);
                        objScripHelpNCDEXCommodities.NCDTickSize = Convert.ToInt64(oSQLiteDataReader["NCDTickSize"]);
                        objScripHelpNCDEXCommodities.NCDName = oSQLiteDataReader["NCDName"].ToString();
                        objScripHelpNCDEXCommodities.NCDExerciseStartDate = Convert.ToInt64(oSQLiteDataReader["NCDExerciseStartDate"]);
                        objScripHelpNCDEXCommodities.NCDExerciseEndDate = Convert.ToInt64(oSQLiteDataReader["NCDExerciseEndDate"]);
                        objScripHelpNCDEXCommodities.NCDPriceNumerator = Convert.ToInt64(oSQLiteDataReader["NCDPriceNumerator"]);
                        objScripHelpNCDEXCommodities.NCDPriceDenominator = Convert.ToInt64(oSQLiteDataReader["NCDPriceDenominator"]);
                        objScripHelpNCDEXCommodities.NCDPriceUnit = oSQLiteDataReader["NCDPriceUnit"].ToString();
                        objScripHelpNCDEXCommodities.NCDQuantityUnit = oSQLiteDataReader["NCDQuantityUnit"].ToString();
                        objScripHelpNCDEXCommodities.NCDDeliveryUnit = oSQLiteDataReader["NCDDeliveryUnit"].ToString();
                        objScripHelpNCDEXCommodities.NCDDeliveryLotQuantity = Convert.ToInt64(oSQLiteDataReader["NCDDeliveryLotQuantity"]);
                        objScripHelpNCDEXCommodities.NCDSequenceId = Convert.ToInt64(oSQLiteDataReader["NCDSequenceId"]);
                        objScripHelpNCDEXCommodities.NCDTokenString = oSQLiteDataReader["NCDTokenString"].ToString();
                        ObjNCDEXCommoditiesCollection.Add(objScripHelpNCDEXCommodities);
                    }
                }
                else if (selectedExchange == Common.Enumerations.Exchange.MCX.ToString() && selectedSegment == Common.Enumerations.Segment.Commodities.ToString())
                {
                    VisibilityBSEEquityGrid = "Hidden";
                    VisibilityBSEDerivativeCOGrid = "Hidden";
                    VisibilityBSEDerivativeSPDGrid = "Hidden";
                    VisibilityBSECurrencyCOGrid = "Hidden";
                    VisibilityBSECurrencySPDGrid = "Hidden";
                    VisibilityBSEDebtGrid = "Hidden";
                    VisibilityNSEEquityGrid = "Hidden";
                    VisibilityNSEDerivativeCOGrid = "Hidden";
                    VisibilityNSECurrencyCOGrid = "Hidden";
                    VisibilityNCDEXCommoditiesGrid = "Hidden";
                    VisibilityMCXCommoditiesGrid = "Visible";
                    string strQuery = @"SELECT MCID, MCSequenceId, MCInstrumentName, MCToken, MCUnderlyingAsset, MCContractCode, MCStrikePrice,
       MCOptionType, MCExpiryDate, MCDisplayExpiryDate, MCContractDescription, MCQuotationUnit, MCQuotationMetric, MCBoardLot, MCPriceTick,
       MCPQFactor, MCTradingUnit, MCGeneralNumerator, MCGeneralDenominator, MCField3 FROM MCX_COMMODITY_CFE;";

                    SQLiteDataReader oSQLiteDataReader = oDataAccessLayer.ExecuteDataReader((int)ConnectionDB.Masters,strQuery, System.Data.CommandType.Text, null);
                    while (oSQLiteDataReader.Read())
                    {
                        ScripHelpMCXCommodities objScripHelpMCXCommodities = new ScripHelpMCXCommodities();
                        objScripHelpMCXCommodities.MCID = Convert.ToInt64(oSQLiteDataReader["MCID"]);
                        objScripHelpMCXCommodities.MCSequenceId = Convert.ToInt64(oSQLiteDataReader["MCSequenceId"]);
                        objScripHelpMCXCommodities.MCInstrumentName = oSQLiteDataReader["MCInstrumentName"].ToString();
                        objScripHelpMCXCommodities.MCToken = Convert.ToInt64(oSQLiteDataReader["MCToken"]);
                        objScripHelpMCXCommodities.MCUnderlyingAsset = Convert.ToInt64(oSQLiteDataReader["MCUnderlyingAsset"]);
                        objScripHelpMCXCommodities.MCContractCode = oSQLiteDataReader["MCContractCode"].ToString();
                        objScripHelpMCXCommodities.MCStrikePrice = Convert.ToInt64(oSQLiteDataReader["MCStrikePrice"]);
                        objScripHelpMCXCommodities.MCOptionType = oSQLiteDataReader["MCOptionType"].ToString();
                        objScripHelpMCXCommodities.MCExpiryDate = Convert.ToInt64(oSQLiteDataReader["MCExpiryDate"]);
                        objScripHelpMCXCommodities.MCDisplayExpiryDate = oSQLiteDataReader["MCDisplayExpiryDate"].ToString();
                        objScripHelpMCXCommodities.MCContractDescription = oSQLiteDataReader["MCContractDescription"].ToString();
                        objScripHelpMCXCommodities.MCQuotationUnit = Convert.ToInt64(oSQLiteDataReader["MCQuotationUnit"]);
                        objScripHelpMCXCommodities.MCQuotationMetric = oSQLiteDataReader["MCQuotationMetric"].ToString();
                        objScripHelpMCXCommodities.MCBoardLot = Convert.ToInt64(oSQLiteDataReader["MCBoardLot"]);
                        objScripHelpMCXCommodities.MCPriceTick = Convert.ToInt64(oSQLiteDataReader["MCPriceTick"]);
                        objScripHelpMCXCommodities.MCPQFactor = oSQLiteDataReader["MCPQFactor"].ToString();
                        objScripHelpMCXCommodities.MCTradingUnit = oSQLiteDataReader["MCTradingUnit"].ToString();
                        objScripHelpMCXCommodities.MCGeneralNumerator = Convert.ToDecimal(oSQLiteDataReader["MCGeneralNumerator"]);
                        objScripHelpMCXCommodities.MCGeneralDenominator = Convert.ToDecimal(oSQLiteDataReader["MCGeneralDenominator"]);
                        objScripHelpMCXCommodities.MCField3 = oSQLiteDataReader["MCField3"].ToString();
                        ObjMCXCommoditiesCollection.Add(objScripHelpMCXCommodities);
                    }
                }

#endif

            }
            catch (Exception ex)
            {
                //ExceptionUtility.LogError(ex);
            }
            finally
            {
                oDataAccessLayer.CloseConnection((int)DataAccessLayer.ConnectionDB.Masters);
            }

        }

        //private void ScripHelpClose_Click()
        //{
        //    try
        //    {
        //        ScripHelp ScripHelpWindow = System.Windows.Application.Current.Windows.OfType<ScripHelp>().FirstOrDefault();
        //        ScripHelpWindow.Hide();
        //    }
        //    catch (Exception ex) { }


        //}


        public double DivideByPrecision(int prec)
        {
            double Num = 0;
            Num = Math.Pow(10, prec);
            return Num;
        }

        private void ScripTxtChange_Click()
        {
            try
            {
                if (selectedSegment == Common.Enumerations.Segment.Equity.ToString())
                {
                    List<ScripHelpModel> l = ApplyFilter();
                    ObjEquityDataCollection = new ObservableCollection<ScripHelpModel>(l);
                    NotifyPropertyChanged(nameof(ObjEquityDataCollection));
                }
                else if (selectedSegment == Common.Enumerations.Segment.Derivative.ToString() && cochecked == true)
                {
                    List<ScripHelpBSEDerivativeCO> l1 = ApplyFilter1();
                    ObjBSEDerivativeCOCollection = new ObservableCollection<ScripHelpBSEDerivativeCO>(l1);
                    NotifyPropertyChanged(nameof(ObjBSEDerivativeCOCollection));
                }


                else if (selectedSegment == Common.Enumerations.Segment.Derivative.ToString() && spdchecked == true)
                {
                    List<ScripHelpBSEDerivativeCO> l2 = ApplyFilter2();
                    ObjBSEDerivativeCOCollection = new ObservableCollection<ScripHelpBSEDerivativeCO>(l2);
                    NotifyPropertyChanged(nameof(ObjBSEDerivativeCOCollection));
                }

                else if (selectedSegment == Common.Enumerations.Segment.Currency.ToString() && cochecked == true)
                {
                    List<ScripHelpBSECurrencyCO> l3 = ApplyFilter3();
                    ObjBSECurrencyCOCollection = new ObservableCollection<ScripHelpBSECurrencyCO>(l3);
                    NotifyPropertyChanged(nameof(ObjBSECurrencyCOCollection));

                }

                else if (selectedSegment == Common.Enumerations.Segment.Currency.ToString() && spdchecked == true)
                {
                    List<ScripHelpBSECurrencyCO> l4 = ApplyFilter4();
                    ObjBSECurrencyCOCollection = new ObservableCollection<ScripHelpBSECurrencyCO>(l4);
                    NotifyPropertyChanged(nameof(ObjBSECurrencyCOCollection));

                }

                else if (selectedSegment == Common.Enumerations.Segment.Debt.ToString())
                {
                    List<ScripHelpModel> l5 = ApplyFilter5();
                    ObjDebtDataCollection = new ObservableCollection<ScripHelpModel>(l5);
                    NotifyPropertyChanged(nameof(ObjDebtDataCollection));
                }

            }
            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
                return;
            }


        }

        private List<ScripHelpModel> ApplyFilter()
        {
            // Collection which will take your ObservableCollection
            // var _itemSourceList = new CollectionViewSource() { Source = SearchTemplist.OrderBy(x => x.ScriptId1) };
            var _itemSourceList = new CollectionViewSource() { Source = SearchEquityTemplist };
            // ICollectionView the View/UI part 
            ICollectionView Itemlist = _itemSourceList.View;
            // executeFilterAction(new Action(() =>
            //{
            if (!string.IsNullOrEmpty(txtScripID) && !string.IsNullOrEmpty(txtScripCode) && string.IsNullOrEmpty(txtISINCode))
            {
                var yourCostumFilter = new Predicate<object>(item => ((ScripHelpModel)item).ScripId.ToLower().StartsWith(txtScripID.Trim().ToLower()) && ((ScripHelpModel)item).ScripCode.ToString().StartsWith(txtScripCode.Trim()) && ((ScripHelpModel)item).IsinCode.ToString().StartsWith(txtISINCode.Trim()));
                Itemlist.Filter = yourCostumFilter;
            }
            else if (string.IsNullOrEmpty(txtScripCode) && string.IsNullOrEmpty(txtISINCode))
            {
                var yourCostumFilter = new Predicate<object>(item => ((ScripHelpModel)item).ScripId.ToLower().StartsWith(txtScripID.Trim().ToLower()));
                Itemlist.Filter = yourCostumFilter;
            }
            else if (string.IsNullOrEmpty(txtScripID) && string.IsNullOrEmpty(txtISINCode))
            {
                var yourCostumFilter = new Predicate<object>(item => ((ScripHelpModel)item).ScripCode.ToString().StartsWith(txtScripCode.Trim()));
                Itemlist.Filter = yourCostumFilter;
            }
            else if (string.IsNullOrEmpty(txtScripID) && string.IsNullOrEmpty(txtScripCode))
            {
                var yourCostumFilter = new Predicate<object>(item => ((ScripHelpModel)item).IsinCode.ToString().StartsWith(txtISINCode.Trim()));
                Itemlist.Filter = yourCostumFilter;
            }

            //now we add our Filter

            var l = Itemlist.Cast<ScripHelpModel>().ToList();
            return l;
        }

        private List<ScripHelpBSEDerivativeCO> ApplyFilter1()
        {
            // Collection which will take your ObservableCollection
            // var _itemSourceList = new CollectionViewSource() { Source = SearchTemplist.OrderBy(x => x.ScriptId1) };
            var _itemSourceList = new CollectionViewSource() { Source = SearchDerCOTemplist };
            // ICollectionView the View/UI part 
            ICollectionView Itemlist = _itemSourceList.View;
            // executeFilterAction(new Action(() =>
            //{
            if (!string.IsNullOrEmpty(txtScripID) && !string.IsNullOrEmpty(txtScripCode))
            {
                var yourCostumFilter = new Predicate<object>(item => ((ScripHelpBSEDerivativeCO)item).ScripID.ToLower().StartsWith(txtScripID.Trim().ToLower()) && ((ScripHelpBSEDerivativeCO)item).ContractTokenNum.ToString().StartsWith(txtScripCode.Trim()));
                Itemlist.Filter = yourCostumFilter;
            }
            else if (string.IsNullOrEmpty(txtScripCode))
            {
                var yourCostumFilter = new Predicate<object>(item => ((ScripHelpBSEDerivativeCO)item).ScripID.ToLower().StartsWith(txtScripID.Trim().ToLower()));
                Itemlist.Filter = yourCostumFilter;
            }
            else if (string.IsNullOrEmpty(txtScripID))
            {
                var yourCostumFilter = new Predicate<object>(item => ((ScripHelpBSEDerivativeCO)item).ContractTokenNum.ToString().StartsWith(txtScripCode.Trim()));
                Itemlist.Filter = yourCostumFilter;
            }
            //now we add our Filter

            var l1 = Itemlist.Cast<ScripHelpBSEDerivativeCO>().ToList();
            return l1;
        }

        private List<ScripHelpBSEDerivativeCO> ApplyFilter2()
        {
            // Collection which will take your ObservableCollection
            // var _itemSourceList = new CollectionViewSource() { Source = SearchTemplist.OrderBy(x => x.ScriptId1) };
            var _itemSourceList = new CollectionViewSource() { Source = SearchDerCOTemplist };
            // ICollectionView the View/UI part 
            ICollectionView Itemlist = _itemSourceList.View;
            // executeFilterAction(new Action(() =>
            //{
            if (!string.IsNullOrEmpty(txtScripID) && !string.IsNullOrEmpty(txtScripCode))
            {
                var yourCostumFilter = new Predicate<object>(item => ((ScripHelpBSEDerivativeCO)item).ScripID.ToString().ToLower().Trim().StartsWith(txtScripID.Trim().ToLower()) && ((ScripHelpBSEDerivativeCO)item).ContractTokenNum.ToString().StartsWith(txtScripCode.Trim()));
                Itemlist.Filter = yourCostumFilter;
            }
            else if (string.IsNullOrEmpty(txtScripCode))
            {
                var yourCostumFilter = new Predicate<object>(item => ((ScripHelpBSEDerivativeCO)item).ScripID.ToString().ToLower().Trim().StartsWith(txtScripID.Trim().ToLower()));
                Itemlist.Filter = yourCostumFilter;
            }
            else if (string.IsNullOrEmpty(txtScripID))
            {
                var yourCostumFilter = new Predicate<object>(item => ((ScripHelpBSEDerivativeCO)item).ContractTokenNum.ToString().Trim().StartsWith(txtScripCode.Trim()));
                Itemlist.Filter = yourCostumFilter;
            }
            //now we add our Filter

            var l2 = Itemlist.Cast<ScripHelpBSEDerivativeCO>().ToList();
            return l2;
        }

        private List<ScripHelpBSECurrencyCO> ApplyFilter3()
        {
            // Collection which will take your ObservableCollection
            // var _itemSourceList = new CollectionViewSource() { Source = SearchTemplist.OrderBy(x => x.ScriptId1) };
            var _itemSourceList = new CollectionViewSource() { Source = SearchCurCOTemplist };
            // ICollectionView the View/UI part 
            ICollectionView Itemlist = _itemSourceList.View;
            // executeFilterAction(new Action(() =>
            //{
            if (!string.IsNullOrEmpty(txtScripID) && !string.IsNullOrEmpty(txtScripCode))
            {
                var yourCostumFilter = new Predicate<object>(item => ((ScripHelpBSECurrencyCO)item).ScripID.ToString().ToLower().Trim().StartsWith(txtScripID.Trim().ToLower()) && ((ScripHelpBSECurrencyCO)item).ContractTokenNum.ToString().StartsWith(txtScripCode.Trim()));
                Itemlist.Filter = yourCostumFilter;
            }
            else if (string.IsNullOrEmpty(txtScripCode))
            {
                var yourCostumFilter = new Predicate<object>(item => ((ScripHelpBSECurrencyCO)item).ScripID.ToString().ToLower().Trim().StartsWith(txtScripID.Trim().ToLower()));
                Itemlist.Filter = yourCostumFilter;
            }
            else if (string.IsNullOrEmpty(txtScripID))
            {
                var yourCostumFilter = new Predicate<object>(item => ((ScripHelpBSECurrencyCO)item).ContractTokenNum.ToString().Trim().StartsWith(txtScripCode.Trim()));
                Itemlist.Filter = yourCostumFilter;
            }
            //now we add our Filter

            var l3 = Itemlist.Cast<ScripHelpBSECurrencyCO>().ToList();
            return l3;
        }

        private List<ScripHelpBSECurrencyCO> ApplyFilter4()
        {
            // Collection which will take your ObservableCollection
            // var _itemSourceList = new CollectionViewSource() { Source = SearchTemplist.OrderBy(x => x.ScriptId1) };
            var _itemSourceList = new CollectionViewSource() { Source = SearchCurCOTemplist };
            // ICollectionView the View/UI part 
            ICollectionView Itemlist = _itemSourceList.View;
            // executeFilterAction(new Action(() =>
            //{
            if (!string.IsNullOrEmpty(txtScripID) && !string.IsNullOrEmpty(txtScripCode))
            {
                var yourCostumFilter = new Predicate<object>(item => ((ScripHelpBSECurrencyCO)item).ScripID.ToString().ToLower().Trim().StartsWith(txtScripID.Trim().ToLower()) && ((ScripHelpBSECurrencyCO)item).ContractTokenNum.ToString().StartsWith(txtScripCode.Trim()));
                Itemlist.Filter = yourCostumFilter;
            }
            else if (string.IsNullOrEmpty(txtScripCode))
            {
                var yourCostumFilter = new Predicate<object>(item => ((ScripHelpBSECurrencyCO)item).ScripID.ToString().ToLower().Trim().StartsWith(txtScripID.Trim().ToLower()));
                Itemlist.Filter = yourCostumFilter;
            }
            else if (string.IsNullOrEmpty(txtScripID))
            {
                var yourCostumFilter = new Predicate<object>(item => ((ScripHelpBSECurrencyCO)item).ContractTokenNum.ToString().Trim().StartsWith(txtScripCode.Trim()));
                Itemlist.Filter = yourCostumFilter;
            }
            //now we add our Filter

            var l4 = Itemlist.Cast<ScripHelpBSECurrencyCO>().ToList();
            return l4;
        }

        private List<ScripHelpModel> ApplyFilter5()
        {
            // Collection which will take your ObservableCollection
            // var _itemSourceList = new CollectionViewSource() { Source = SearchTemplist.OrderBy(x => x.ScriptId1) };
            var _itemSourceList = new CollectionViewSource() { Source = SearchDebtTemplist };
            // ICollectionView the View/UI part 
            ICollectionView Itemlist = _itemSourceList.View;
            // executeFilterAction(new Action(() =>
            //{
            if (!string.IsNullOrEmpty(txtScripID) && !string.IsNullOrEmpty(txtScripCode) && string.IsNullOrEmpty(txtISINCode))
            {
                var yourCostumFilter = new Predicate<object>(item => ((ScripHelpModel)item).ScripId.ToLower().StartsWith(txtScripID.Trim().ToLower()) && ((ScripHelpModel)item).ScripCode.ToString().StartsWith(txtScripCode.Trim()) && ((ScripHelpModel)item).IsinCode.ToString().StartsWith(txtISINCode.Trim()));
                Itemlist.Filter = yourCostumFilter;
            }
            else if (string.IsNullOrEmpty(txtScripCode) && string.IsNullOrEmpty(txtISINCode))
            {
                var yourCostumFilter = new Predicate<object>(item => ((ScripHelpModel)item).ScripId.ToLower().StartsWith(txtScripID.Trim().ToLower()));
                Itemlist.Filter = yourCostumFilter;
            }
            else if (string.IsNullOrEmpty(txtScripID) && string.IsNullOrEmpty(txtISINCode))
            {
                var yourCostumFilter = new Predicate<object>(item => ((ScripHelpModel)item).ScripCode.ToString().StartsWith(txtScripCode.Trim()));
                Itemlist.Filter = yourCostumFilter;
            }
            else if (string.IsNullOrEmpty(txtScripID) && string.IsNullOrEmpty(txtScripCode))
            {
                var yourCostumFilter = new Predicate<object>(item => ((ScripHelpModel)item).IsinCode.ToString().StartsWith(txtISINCode.Trim()));
                Itemlist.Filter = yourCostumFilter;
            }
            //-------------------------------------------
            //if (!string.IsNullOrEmpty(txtScripID) && !string.IsNullOrEmpty(txtScripCode))
            //{
            //    var yourCostumFilter = new Predicate<object>(item => ((ScripHelpModel)item).ScripId.ToString().ToLower().Trim().StartsWith(txtScripID.Trim().ToLower()) && ((ScripHelpModel)item).ScripCode.ToString().StartsWith(txtScripCode.Trim()));
            //    Itemlist.Filter = yourCostumFilter;
            //}
            //else if (string.IsNullOrEmpty(txtScripCode))
            //{
            //    var yourCostumFilter = new Predicate<object>(item => ((ScripHelpModel)item).ScripId.ToString().ToLower().Trim().StartsWith(txtScripID.Trim().ToLower()));
            //    Itemlist.Filter = yourCostumFilter;
            //}
            //else if (string.IsNullOrEmpty(txtScripID))
            //{
            //    var yourCostumFilter = new Predicate<object>(item => ((ScripHelpModel)item).ScripCode.ToString().Trim().StartsWith(txtScripCode.Trim()));
            //    Itemlist.Filter = yourCostumFilter;
            //}
            //---------------------------
            //now we add our Filter

            var l5 = Itemlist.Cast<ScripHelpModel>().ToList();
            return l5;
        }
        #endregion

        //#if TWS
        //    partial class ScripHelpVM
        //    {

        //    }
        //#elif BOW
        //    partial class ScripHelpVM
        //    {

        //    }
        //    //#endif
    }
}