using CommonFrontEnd.Common;
using CommonFrontEnd.CSVReader;
using CommonFrontEnd.Global;
using CommonFrontEnd.Model;
using CommonFrontEnd.Processor;
using CommonFrontEnd.SharedMemories;
using SubscribeList;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Controls;
using CommonFrontEnd.Constants;
using static CommonFrontEnd.Common.Enumerations;
using System.Collections;
using System.Threading;
using CommonFrontEnd.View;
using CommonFrontEnd.HTTPHlper;
using CommonFrontEnd.ViewModel.Order;
using CommonFrontEnd.View.Settings;
using System.Data.SQLite;
using CommonFrontEnd.Model.Profiling;
using System.Globalization;
using CommonFrontEnd.View.Order;
using System.Windows.Data;
using CommonFrontEnd.View.Touchline;
using System.Diagnostics;
using CommonFrontEnd.ViewModel.Profiling;
using System.Windows.Media;
using System.Drawing;
using CommonFrontEnd.View.UserControls;
using System.Windows.Controls.Primitives;
using System.Timers;
using CommonFrontEnd.Model.Order;
using CommonFrontEnd.Processor.Order;
using static CommonFrontEnd.SharedMemories.DataAccessLayer;

namespace CommonFrontEnd.ViewModel.Touchline
{
    public partial class MarketWatchVM
    {

        static List<string> objProfileNames = new List<string>();
        public static SQLiteDataReader oSQLiteDataReader = null;
        private static ConcurrentBag<int> objListOfVisibleRecordsList;
        public delegate void ShowVisibleRecordsEventHandler(ConcurrentBag<int> objListOfVisibleRecords);
        public static event ShowVisibleRecordsEventHandler OnScrollUpdateVisibleItemsOnly;
        private static ReadFromCSV ObjCommon = new ReadFromCSV();
        public string TabName = "Default";
        public static int DecimalPnt { get; set; }
        private bool Predefined_Flag = true;
        public bool UserDefined_Flag = true;
        static List<int> objScripCodes = new List<int>();
        private static int SelectedScripCode = 0;
        bool Loaded = false;
        public static int index;
        private string _LeftPosition = "71";
        BestFiveMarketPicture mainwindow;
        public DataAccessLayer oDataAccessLayer;
        public static DataAccessLayer oDataAccessLayer1;

        System.Timers.Timer SrchTime;

        private string _searchString;
        public string searchString
        {
            get { return _searchString; }
            set { _searchString = value; }
        }

        private int _searchStringCnt;
        public int searchStringCnt
        {
            get { return _searchStringCnt; }
            set { _searchStringCnt = value; }
        }
        private static string _StringFormat;

        public static string StringFormat
        {
            get { return _StringFormat; }
            set
            {
                _StringFormat = value;
                NotifyStaticPropertyChanged("StringFormat");
            }
        }
        private static string _DeciVisiEq;

        public static string DeciVisiEq
        {
            get { return _DeciVisiEq; }
            set
            {
                _DeciVisiEq = value;
                NotifyStaticPropertyChanged("DeciVisiEq");
            }
        }
        private static int _StrikePriceL;

        public static int StrikePriceL
        {
            get { return _StrikePriceL; }
            set
            {
                _StrikePriceL = value;
                NotifyStaticPropertyChanged("StrikePriceL");
            }
        }

        private static string _DeciVisiDev;

        public static string DeciVisiDev
        {
            get { return _DeciVisiDev; }
            set
            {
                _DeciVisiDev = value;
                NotifyStaticPropertyChanged("DeciVisiDev");
            }
        }
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

        private string _Width = "1232";

        public string ClassicWidth
        {
            get { return _Width; }
            set { _Width = value; NotifyPropertyChanged("ClassicWidth"); }
        }


        private string _Height = "729";

        public string ClassicHeight
        {
            get { return _Height; }
            set { _Height = value; NotifyPropertyChanged("ClassicHeight"); }
        }

        /// <summary>
        /// 
        /// </summary>
        private static ObservableCollection<MarketWatchModel> objTouchlineDataCollection;
        public static ObservableCollection<MarketWatchModel> ObjTouchlineDataCollection
        {
            get { return objTouchlineDataCollection; }
            set { objTouchlineDataCollection = value; NotifyStaticPropertyChanged("ObjTouchlineDataCollection"); }
        }

        public class TlineListData
        {
            public int ScripCode;
            public int index;
        };

        public static LinkedList<TlineListData> TouchLineList;

        private static int TLStart, TLEnd;

        private static List<MarketWatchModel> _SearchTemplist;
        public static List<MarketWatchModel> SearchTemplist
        {
            get { return _SearchTemplist; }
            set { _SearchTemplist = value; }
        }

        private static ObservableCollection<MarketWatchModel> _indicesDataCollection;
        public static ObservableCollection<MarketWatchModel> IndicesDataCollection
        {
            get { return _indicesDataCollection; }
            set { _indicesDataCollection = value; NotifyStaticPropertyChanged("IndicesDataCollection"); }
        }

        private static List<string> marketMoversCombo;
        public static List<string> MarketMoversCombo
        {
            get { return marketMoversCombo; }
            set { marketMoversCombo = value; NotifyStaticPropertyChanged("MarketMoversCombo"); }
        }

        private static ObservableCollection<string> marketsCombo;
        public static ObservableCollection<string> MarketsCombo
        {
            get { return marketsCombo; }
            set { marketsCombo = value; NotifyStaticPropertyChanged("MarketsCombo"); }
        }

        private static List<AllIndicesModel> _MarketList;

        public static List<AllIndicesModel> MarketList
        {
            get { return _MarketList; }
            set { _MarketList = value; }
        }


        private static ObservableCollection<string> _AddSubMenuProfiles;
        public static ObservableCollection<string> AddSubMenuProfiles
        {
            get { return _AddSubMenuProfiles; }
            set { _AddSubMenuProfiles = value; NotifyStaticPropertyChanged("AddSubMenuProfiles"); }
        }

        private static List<string> groupCombo;
        public static List<string> GroupCombo
        {
            get { return groupCombo; }
            set { groupCombo = value; NotifyStaticPropertyChanged("GroupCombo"); }
        }

        private static ObservableCollection<string> _ScripProfilingCombo;
        public static ObservableCollection<string> ScripProfilingCombo
        {
            get { return _ScripProfilingCombo; }
            set { _ScripProfilingCombo = value; NotifyStaticPropertyChanged("ScripProfilingCombo"); }
        }

        private static string _ScripProfComboSelectedItem;

        public static string ScripProfComboSelectedItem
        {
            get { return _ScripProfComboSelectedItem; }
            set
            {
                _ScripProfComboSelectedItem = value;
                NotifyStaticPropertyChanged("ScripProfComboSelectedItem");
            }
        }
        private static string _MarketsComboSelectedItem;

        public static string MarketsComboSelectedItem
        {
            get { return _MarketsComboSelectedItem; }
            set
            {
                _MarketsComboSelectedItem = value;
                NotifyStaticPropertyChanged("MarketsComboSelectedItem");
                // UpdateDataGrid();
                //OnChangeOfMarkets();
            }
        }

        private string _FilterVisibility = "Visible";
        public string FilterVisibility
        {
            get { return _FilterVisibility; }
            set { _FilterVisibility = value; NotifyPropertyChanged("FilterVisibility"); }
        }


        private static MarketWatchModel _SelectedItem;
        public static MarketWatchModel SelectedItem
        {
            get { return _SelectedItem; }
            set
            {
                _SelectedItem = value;
                NotifyStaticPropertyChanged("SelectedItem");
                if (value != null)
                {
                    SelectedScripCode = value.Scriptcode1;
                    UtilityLoginDetails.GETInstance.SelectedTouchLineScripID = value.ScriptId1;
                    UtilityLoginDetails.GETInstance.SelectedTouchLineScripCode = value.Scriptcode1;
                    if (MemoryManager.InvokeWindowOnScripCodeSelection != null)
                        MemoryManager.InvokeWindowOnScripCodeSelection(Convert.ToString(value.Scriptcode1), UtilityOrderDetails.GETInstance.CurrentOrderEntry.ToString());
                }
            }
        }

        private RelayCommand _KeyUp;
        public RelayCommand KeyUp
        {
            get
            {
                return _KeyUp ?? (_KeyUp = new RelayCommand(
                    (object e) => DataGrid_KeyUp(e)));
            }
        }

        private RelayCommand _Drop;
        public RelayCommand Drop
        {
            get
            {
                return _Drop ?? (_Drop = new RelayCommand(
                    (object e) => DataGrid_Drop(e)));
            }
        }

        private RelayCommand _TabSelectionChanged_Event;
        public RelayCommand TabSelectionChanged_Event
        {
            get
            {
                return _TabSelectionChanged_Event ?? (_TabSelectionChanged_Event = new RelayCommand(
                    (object e) => UpdateGrid_tabSlection(e)
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

        private RelayCommand _SelectionChanged_UserDefined;
        public RelayCommand SelectionChanged_UserDefined
        {
            get
            {
                return _SelectionChanged_UserDefined ?? (_SelectionChanged_UserDefined = new RelayCommand(
                    (object e) => UpdateDataGrid_UserDefined(e)
                        ));
            }
        }


        private RelayCommand _DataGridDoubleClick;

        public RelayCommand DataGridDoubleClick
        {
            get
            {
                return _DataGridDoubleClick ?? (_DataGridDoubleClick = new RelayCommand(
                    (object e) => DataGrid_DoubleClick()));
            }
        }

        private RelayCommand _Open_Col_Profile;
        public RelayCommand Open_Col_Profile
        {
            get
            {
                return _Open_Col_Profile ?? (_Open_Col_Profile = new RelayCommand(
                    (object e) => Open_Column_Profile()
                        ));
            }
        }

        private RelayCommand _Open_OrderEntry;
        public RelayCommand Open_OrderEntry
        {
            get
            {
                return _Open_OrderEntry ?? (_Open_OrderEntry = new RelayCommand(
                    (object e) => Open_SwiftOrderEntry(e)
                        ));

            }
        }

        private RelayCommand _FontSelect;

        public RelayCommand FontSelect
        {
            get
            {
                return _FontSelect ?? (_FontSelect = new RelayCommand(
                    (object e) => FontSelection(e)
                        ));
            }
        }
        private RelayCommand _scripProfile;
        public RelayCommand scripProfile
        {
            get
            {
                return _scripProfile ?? (_scripProfile = new RelayCommand(
                    (object e) => openProfile((int)Enumerations.ScripHelpTab.ScripTab)
                        ));
            }
        }
        private RelayCommand _exportText;
        public RelayCommand exportText
        {
            get
            {
                return _exportText ?? (_exportText = new RelayCommand(
                    (object e) => exportToText()
                        ));
            }
        }
        private RelayCommand _colorProfile;
        public RelayCommand colorProfile
        {
            get
            {
                return _colorProfile ?? (_colorProfile = new RelayCommand(
                    (object e) => openProfile((int)Enumerations.ScripHelpTab.ColorTab)
                        ));
            }
        }
        private RelayCommand _defaultTab;
        public RelayCommand defaultTab
        {
            get
            {
                return _defaultTab ?? (_defaultTab = new RelayCommand(
                    (object e) =>
                    {
                        System.Windows.Application.Current.Windows.OfType<MarketWatch>().FirstOrDefault().SnPSensexTab.SelectedIndex = 0;
                    }
                        ));
            }
        }

        private RelayCommand _ExportExcel;
        public RelayCommand ExportExcel
        {
            get
            {
                return _ExportExcel ?? (_ExportExcel = new RelayCommand(
                    (object e) => ExecuteMyCommand()
                        ));
            }
        }

        private RelayCommand _GridLines_visible;
        public RelayCommand GridLines_visible
        {
            get
            {
                return _GridLines_visible ?? (_GridLines_visible = new RelayCommand(
                    (object e) => GrdlineVisible_Click()
                        ));
            }
        }

        ////private RelayCommand _Window_Closing;
        ////public RelayCommand Window_Closing
        ////{
        ////    get
        ////    {
        ////        return _Window_Closing ?? (_Window_Closing = new RelayCommand(
        ////            (object e) => Window_Closing_Event()
        ////                ));
        ////    }
        ////}

        private RelayCommand _Insert_Blank;
        public RelayCommand Insert_Blank
        {
            get
            {
                return _Insert_Blank ?? (_Insert_Blank = new RelayCommand(
                    (object e) => Insert_Blank_row()
                        ));
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

        private RelayCommand _ShortCut_Enter;

        public RelayCommand ShortCut_Enter
        {
            get
            {
                return _ShortCut_Enter ?? (_ShortCut_Enter = new RelayCommand((object e) => EnterUsingUserControl(e)));
            }
        }

        private RelayCommand _Delete_Scrips;
        public RelayCommand Delete_Scrips
        {
            get
            {
                return _Delete_Scrips ?? (_Delete_Scrips = new RelayCommand(
                    (object e) => Delete_Scrips_row()
                        ));
            }
        }

        private RelayCommand _OnClickOfProfile;

        public RelayCommand OnClickOfProfile
        {
            get
            {
                return _OnClickOfProfile ?? (_OnClickOfProfile = new RelayCommand(
                    (object e) => OnSubmenuSelection(e)
                        ));
            }
        }

        private RelayCommand _ShortCut_EnterScrips;

        public RelayCommand ShortCut_EnterScrips
        {
            get
            {
                return _ShortCut_EnterScrips ?? (_ShortCut_EnterScrips = new RelayCommand(
                    (object e) => AddScripsUsingUserControl()
                        ));
            }
        }


        private RelayCommand _AddMenuColProfile;

        public RelayCommand AddMenuColProfile
        {
            get
            {
                return _AddMenuColProfile ?? (_AddMenuColProfile = new RelayCommand(
                    (object e) => Change_Column_Profile()
                        ));
            }
        }

        private RelayCommand _DeleteTabItems;

        public RelayCommand DeleteTabItems
        {
            get { return _DeleteTabItems ?? (_DeleteTabItems = new RelayCommand((object e) => DeleteTabItems_Click(e))); }

        }


        private RelayCommand _BtnHideShowFilter;

        public RelayCommand BtnHideShowFilter
        {
            get
            {
                return _BtnHideShowFilter ?? (_BtnHideShowFilter = new RelayCommand((object e) =>
                {

                    if (FilterVisibility == "Visible")
                        FilterVisibility = "Collapsed";
                    else
                        FilterVisibility = "Visible";
                }));
            }

        }

        private RelayCommand _ClearFilterButton;

        public RelayCommand ClearFilterButton
        {
            get
            {
                return _ClearFilterButton ?? (_ClearFilterButton = new RelayCommand((object e) =>
                {
                    txtScripID = string.Empty;
                    txtScripCode = string.Empty;
                }));
            }
        }

        private RelayCommand _HideShowFilter;

        public RelayCommand HideShowFilter
        {
            get
            {
                return _HideShowFilter ?? (_HideShowFilter = new RelayCommand((object e) =>
                {

                    ToggleButton tglbtn = e as ToggleButton;
                    tglbtn.IsChecked = true;
                    FilterVisibility = "Collapsed";
                }));
            }

        }

        private RelayCommand _ShowFilter;

        public RelayCommand ShowFilter
        {
            get
            {
                return _ShowFilter ?? (_ShowFilter = new RelayCommand((object e) =>
                {

                    ToggleButton tglbtn = e as ToggleButton;
                    tglbtn.IsChecked = false;
                    FilterVisibility = "Visible";

                }));
            }

        }

        private RelayCommand _OnClickOfSaveProfile;

        public RelayCommand OnClickOfSaveProfile
        {
            get
            {
                return _OnClickOfSaveProfile ?? (_OnClickOfSaveProfile = new RelayCommand(
                    (object e) => OnClickOfSaveProfileSelection(e)
                        ));
            }
        }

        private string _txtScripID = string.Empty;

        public string txtScripID
        {
            get { return _txtScripID; }
            set { _txtScripID = value; ScripIdTxtChange_Click(); NotifyPropertyChanged("txtScripID"); }
        }

        private string _txtScripCode = string.Empty;

        public string txtScripCode
        {
            get { return _txtScripCode; }
            set { _txtScripCode = value; ScripIdTxtChange_Click(); NotifyPropertyChanged("txtScripCode"); }
        }

        private string _GrdlineVisible;

        public string GrdlineVisible
        {
            get { return _GrdlineVisible; }
            set { _GrdlineVisible = value; NotifyPropertyChanged("GrdlineVisible"); }
        }

        private string _GridLinesVisibility = "Disable GridLines";

        public string GridLinesVisibility
        {
            get { return _GridLinesVisibility; }
            set { _GridLinesVisibility = value; NotifyPropertyChanged("GridLinesVisibility"); }
        }

        private static int? LastData = null;

        private static string _CurrentTabSlected;

        public static string CurrentTabSlected
        {
            get
            {
                if (System.Windows.Application.Current.Windows.OfType<MarketWatch>().FirstOrDefault().SnPSensexTab.SelectedItem != null)
                {
                    string tabName = ((HeaderedContentControl)System.Windows.Application.Current.Windows.OfType<MarketWatch>().FirstOrDefault().SnPSensexTab.SelectedItem).Header.ToString();

                    if (tabName != "Default")
                        tabName = ((TabCloseButton)((HeaderedContentControl)System.Windows.Application.Current.Windows.OfType<MarketWatch>().FirstOrDefault().SnPSensexTab.SelectedItem).Header).TabLblTitle.Content.ToString();

                    //return ((HeaderedContentControl)System.Windows.Application.Current.Windows.OfType<MarketWatch>().FirstOrDefault().SnPSensexTab.SelectedItem).Header.ToString();
                    return tabName;
                }
                else
                    return string.Empty;
            }
            //set { _CurrentTabSlected = value; NotifyStaticPropertyChanged("CurrentTabSlected"); }
        }

        private static string _TitleTouchLine;

        public static string TitleTouchLine
        {
            get { return _TitleTouchLine; }
            set { _TitleTouchLine = value; NotifyStaticPropertyChanged("TitleTouchLine"); }
        }

        public static string SelectedProfilename;

        static bool OnSubMenuSelection = false;
        //private RelayCommand _Change_Profile;
        //public RelayCommand Change_Profile
        //{
        //    get
        //    {
        //        return _Change_Profile ?? (_Change_Profile = new RelayCommand(
        //            (object e) => Change_Column_Profile()
        //                ));
        //    }
        //}

        #region StaticNotifyPropertyChangedEvent
        public static event EventHandler<PropertyChangedEventArgs> StaticPropertyChanged
                 = delegate { };
        private static void NotifyStaticPropertyChanged(string propertyName)
        {
            StaticPropertyChanged(null, new PropertyChangedEventArgs(propertyName));
        }
        #endregion


        #region NotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(String propertyName = "")
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
                //var e = new PropertyChangedEventArgs(propertyName);
                //this.PropertyChanged(this, e);
            }

        }

        #endregion
        public MarketWatchVM()
        {
            oDataAccessLayer = new DataAccessLayer();
            oDataAccessLayer.Connect((int)DataAccessLayer.ConnectionDB.Masters);
            oDataAccessLayer1 = new DataAccessLayer();
            oDataAccessLayer1.Connect((int)DataAccessLayer.ConnectionDB.Masters);

            if (CommonFrontEnd.SharedMemories.ConfigurationMasterMemory.ConfigurationDict.ContainsKey("WindowsPosition"))
            {
                BoltAppSettingsWindowsPosition oBoltAppSettingsWindowsPosition = new BoltAppSettingsWindowsPosition();
                oBoltAppSettingsWindowsPosition = (BoltAppSettingsWindowsPosition)ConfigurationMasterMemory.ConfigurationDict["WindowsPosition"];
                if (oBoltAppSettingsWindowsPosition != null && oBoltAppSettingsWindowsPosition.ClassicTouchLine != null && oBoltAppSettingsWindowsPosition.ClassicTouchLine.WNDPOSITION != null)
                {
                    ClassicHeight = oBoltAppSettingsWindowsPosition.ClassicTouchLine.WNDPOSITION.Down.ToString();
                    TopPosition = oBoltAppSettingsWindowsPosition.ClassicTouchLine.WNDPOSITION.Top.ToString();
                    LeftPosition = oBoltAppSettingsWindowsPosition.ClassicTouchLine.WNDPOSITION.Left.ToString();
                    ClassicWidth = oBoltAppSettingsWindowsPosition.ClassicTouchLine.WNDPOSITION.Right.ToString();
                }
            }
            //Change_Column_Profile();
#if TWS
            CookTouchLineData(null);

            UpdatingColumnProfile("", false);

#endif
            //  BroadCastProcessor.ScripCodesConfigured(objScripCodes);
            TitleTouchLine = "TouchLine - " + CurrentTabSlected + "-" + ObjTouchlineDataCollection.Count;
            ScripProfilingVM.OnSave += ScripProfilingVM_OnSave;

            searchString = "";
            searchStringCnt = 0;
            SrchTime = new System.Timers.Timer();
            SrchTime.Interval = 700;
            SrchTime.Elapsed += SrchTime_Elapsed;
        }

        private void ScripProfilingVM_OnSave()
        {
            string TempSelectionOfDropDown = ScripProfComboSelectedItem;
            RefreshUserDefinedDropDowns();
            ScripProfComboSelectedItem = TempSelectionOfDropDown;
        }


        private void OnClickOfSaveProfileSelection(object e)
        {
            MarketWatchModel mwmodel = new MarketWatchModel();
            StreamWriter sw = null;
            try
            {

                if (ScripProfComboSelectedItem != "Userdefined Marketwatch")
                {
                    string path = masterPath.ToString() + ScripProfComboSelectedItem + ".csv";
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                        using (sw = File.CreateText(path))
                        {
                            foreach (MarketWatchModel line in ObjTouchlineDataCollection)
                            {
                                if (string.IsNullOrEmpty(line.ScriptId1))
                                {
                                    sw.WriteLine(string.Empty + "," + string.Empty);
                                }
                                else
                                {
                                    long scripCode = CommonFunctions.GetScripCodeFromScripID(line.ScriptId1);
                                    sw.WriteLine(line.ScriptId1 + "," + scripCode);
                                }
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
                System.Windows.MessageBox.Show("Error while saving file " + ex);
            }
            finally
            {
                if (sw != null)
                {
                    //sw.Flush();
                    sw.Close();
                }


                //Intimate TouchLine to Read the saved File
                // RefreshTouchLine(true);
            }
        }

        private void Change_Column_Profile()
        {
            AddSubMenuProfiles = new ObservableCollection<string>();
            AddSubMenuProfiles.Add("DEFAULT");
            try
            {

                oDataAccessLayer.OpenConnection((int)DataAccessLayer.ConnectionDB.Masters);


#if TWS
                string strQuery = @"SELECT distinct(ProfileName) FROM USER_DEFINED_PROFILE where MemberID=" + "'" + UtilityLoginDetails.GETInstance.MemberId.ToString() + "' AND TraderID =" + "'" + UtilityLoginDetails.GETInstance.TraderId.ToString() + "' AND ScreenName='Touchline'";
#elif BOW
                string strQuery = @"SELECT distinct(ProfileName) FROM USER_DEFINED_PROFILE where MemberID=" + "'" + UtilityLoginDetails.GETInstance.UserLoginId.ToString() + "' AND TraderID = 'NA' AND ScreenName='Touchline'";
#endif
                SQLiteDataReader oSQLiteDataReader = oDataAccessLayer.ExecuteDataReader((int)ConnectionDB.Masters, strQuery, System.Data.CommandType.Text, null);

                while (oSQLiteDataReader.Read())
                {
                    Model.Profiling.ColumnProfilingModel cpm = new Model.Profiling.ColumnProfilingModel();

                    //Profile Name
                    cpm.ColProfile = oSQLiteDataReader["ProfileName"]?.ToString().Trim();


                    // DataGrid objDataGrid = System.Windows.Application.Current.Windows.OfType<MainWindow>().FirstOrDefault().dg;
                    //  List<MenuItem> obj = System.Windows.Application.Current.Windows.OfType<MainWindow>().FirstOrDefault().ContextMenu.Items;

                    AddSubMenuProfiles.Add(cpm.ColProfile.ToString());


                }
                AddSubMenuProfiles.Distinct();


            }
            catch (Exception e)
            {
                //ExceptionUtility.LogError(e);
            }
            finally
            {
                oDataAccessLayer.CloseConnection((int)DataAccessLayer.ConnectionDB.Masters);

            }
        }

        private void Open_Column_Profile()
        {
            ProfileSettings SettingsWindow = System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault();

            if (SettingsWindow != null)
            {
                // SettingsWindow.Activate();
                SettingsWindow.Focus();
                SettingsWindow.MainTabControl.SelectedIndex = 5;
                //ColumnProfilingVM cw = new ColumnProfilingVM();
                //cw.SelectedWindow = Enumerations.WindowName.Touchline.ToString();
                SettingsWindow.Show();
                // ColumnProfilingVM.SelectedWindow = Enumerations.WindowName.Touchline.ToString();
                //  ColumnProfilingVM cw = new ColumnProfilingVM();
                // cw.SelectedWindow = Enumerations.WindowName.Touchline.ToString();
            }
            else
            {
                //SettingsWindow = null;
                SettingsWindow = new ProfileSettings();
                SettingsWindow.ResizeMode = System.Windows.ResizeMode.NoResize;
                SettingsWindow.Owner = System.Windows.Application.Current.MainWindow;
                SettingsWindow.Activate();
                SettingsWindow.MainTabControl.SelectedIndex = 5;
                //SettingsWindow.ColumnProfiling.
                //ColumnProfilingVM cw = new ColumnProfilingVM();
                //cw.SelectedWindow = Enumerations.WindowName.Touchline.ToString();

                SettingsWindow.Show();
                var cw = SettingsWindow.ColumnProfiling.Content;
                //   ((CommonFrontEnd.ViewModel.Profiling.ColumnProfilingVM)(((UserControl)cw).DataContext)).SelectedWindow = Enumerations.WindowName.Touchline.ToString();
                //ColumnProfilingVM cw = new ColumnProfilingVM();
                // cw.SelectedWindow = Enumerations.WindowName.Touchline.ToString();
                //ColumnProfilingVM cw = new ColumnProfilingVM();
                //cw.SelectedWindow=
            }
        }

        public static void Initialize()
        {
            objTouchlineDataCollection = new ObservableCollection<MarketWatchModel>();
            TouchLineList = new LinkedList<TlineListData>();
            SearchTemplist = new List<MarketWatchModel>();
            IndicesDataCollection = new ObservableCollection<MarketWatchModel>();
            MarketMoversCombo = new List<string>();
            MarketsCombo = new ObservableCollection<string>();
            MarketList = new List<AllIndicesModel>();
            GroupCombo = new List<string>();
            ScripProfilingCombo = new ObservableCollection<string>();
            PopulatingDropDowns();
            PopulatingScripProfilingDropDown();

            PopulateScripInsertControl();
            //Add Scripcodes to observable collection
            //Messenger.Default.Register<long>(this, UpdateValues);
            objListOfVisibleRecordsList = new ConcurrentBag<int>();
            //BindingOperations.EnableCollectionSynchronization(objTouchlineDataCollection, ObjectCW);
            OnScrollUpdateVisibleItemsOnly += new ShowVisibleRecordsEventHandler(broadcastReciever_OnScrollUpdateVisibleItemsOnly);
            ObjCommon.ReadVariables();
            ScripInsertUC_VM.ChangeScripCode += OrderEntryVMScripCodeChange;

        }

        private static void OrderEntryVMScripCodeChange(long obj)
        {
            //var value = (int)obj;
            SelectedItem.Scriptcode1 = Convert.ToInt32(obj);
            MarketWatchVM vmObj = new MarketWatchVM();
            vmObj.DataGrid_DoubleClick();
        }

        private static void PopulatingScripProfilingDropDown()
        {
            try
            {
                ScripProfilingCombo.Clear();
                ScripProfilingCombo.Add("-Userdefined Marketwatch-");
#if TWS
                // string masterPath = @"C:\test";
                string filePattern = "*.csv";

                var coFiles = from fullFilename
                     in Directory.EnumerateFiles(masterPath.ToString(), filePattern)
                              select Path.GetFileNameWithoutExtension(fullFilename);
                foreach (var item in coFiles)
                    ScripProfilingCombo.Add(item);
#elif BOW

                foreach (MarketWatchHelper item in UtilityLoginDetails.GETInstance.gobjMarketWatchColl.objMarketWatchObj)
                    ScripProfilingCombo.Add(item.Name);
#endif



                ScripProfComboSelectedItem = ScripProfilingCombo[0];
                //NotifyStaticPropertyChanged("ScripProfilingCombo");
                //NotifyStaticPropertyChanged("ScripProfComboSelectedItem");
            }
            catch (Exception e)
            {
                ExceptionUtility.LogError(e);
                throw;
            }

        }

        public static void objBroadCastProcessor_OnBroadCastRecievedNew(ScripDetails objScripDetails)
        {
            try
            {
                int scripCode = objScripDetails.ScriptCode_BseToken_NseToken;
                //ScripDetails objScripDetails = BroadcastMasterMemory.objScripDetailsConDict.Where(x => x.Key == scripCode).Select(x => x.Value).FirstOrDefault();
                if (objScripDetails != null && ObjTouchlineDataCollection.Count > 0)
                {
                    MainWindowVM.TLMutex.WaitOne();
                    List<int> list = new List<int>();

                    list = TouchLineList.Where(i => i.ScripCode == scripCode).Select(x => x.index).ToList();
                    // list = ObjTouchlineDataCollection.Where(i => i.Scriptcode1 == scripCode).Select(x => ObjTouchlineDataCollection.IndexOf(x)).ToList();

                    for (int i = 0; i < list.Count; i++)
                    {
                        if (list[i] > -1 && list[i] < ObjTouchlineDataCollection.Count)
                            UpdateGrid(list[i], objScripDetails);
                    }
                    MainWindowVM.TLMutex.ReleaseMutex();
                }
                // BestFiveVM.objBroadCastProcessor_OnBroadCastRecieved(scripCode);
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
                return;
            }
        }

        private static void broadcastReciever_OnScrollUpdateVisibleItemsOnly(ConcurrentBag<int> objListOfVisibleRecords)
        {
            objListOfVisibleRecordsList = new ConcurrentBag<int>();
            objListOfVisibleRecordsList = objListOfVisibleRecords;
            ParallelOptions parallelOptions = new ParallelOptions
            {
                MaxDegreeOfParallelism = 10
            };
            Parallel.ForEach<MarketWatchModel>(ObjTouchlineDataCollection, parallelOptions, delegate (MarketWatchModel item)
            {
                if (item != null)
                {
                    if (!objListOfVisibleRecordsList.Contains(item.Scriptcode1))
                    {
                        item.IsVisible = true;
                    }
                    else
                    {
                        item.IsVisible = false;
                    }
                }
            });
        }

        private static void Clear_Fields(MarketWatchModel item)
        {
            item.BRP = 0;
            item.BuyQualtity1 = 0;
            item.BuyRate1 = "";
            item.ChangePercentage = "";
            item.CloseRate = "";
            item.CloseRateL = "";
            item.CorpActionValue = "";
            //item.Counter;
            item.CtValue = "";
            item.CtVolume = 0;
            //item.Exchange_Source;
            item.FiftyTwoHigh = "";
            //item.FiftyTwoHighBColor;
            item.FiftyTwoHighDate = "";
            item.FiftyTwoLow = "";
            //item.FiftyTwoLowBColor;
            item.FiftyTwoLowDate = "";
            //item.ForegroundBuyRate;
            //item.ForegroundColorBuyQuantity;
            //item.ForegroundLTP;
            //item.ForegroundSellQ;
            //item.ForegroundSellR;
            item.HighRate = "";
            item.HighRateL = "";
            item.IndEqPrice = "";
            item.IndEqQty = 0;
            //item.Index;
            //item.IsVisible;
            item.LastTradeTime = "";
            item.LowerCtLmt = "";
            item.LowRate = "";
            item.LowRateL = "";
            item.LTP1 = "";
            item.LTQ1 = 0;
            item.NoofBidBuy1 = 0;
            item.NoOfBidSell1 = 0;
            item.NoOfTrds = 0;
            item.OI = 0;
            item.OIChange = 0;
            item.OIValue = "";
            item.OpenRate = "";
            item.OpenRateL = "";
            item.PremDisc = "";
            item.PrevBuyQualtity1 = "";
            item.PrevBuyRate1 = "";
            item.PrevLTP1 = "";
            item.PrevLTPScripId = "";
            item.PrevSellQuantity1 = "";
            item.PrevSellRate1 = "";
            //item.Scriptcode1;
            //item.ScriptId1;
            //item.Segment_Market;
            item.SellQuantity1 = 0;
            item.SellRate1 = "";
            //item.StartTime;
            //item.StartTime1;
            item.StrikePriceL = 0;
            item.TotBuyQtyL = 0;
            item.TotSellQtyL = 0;
            item.UpperCtLmt = "";
            item.WtAvgRateL = "";
            item.Yield = 0;
        }

        private static void Initialize_Fields(ScripDetails objscrip, ref MarketWatchModel item, int DecimalPoint, int ScripCode)
        {
            try
            {
                if (objscrip != null)
                {
                    item.ScriptId1 = objscrip.ScripID;
                    item.BuyQualtity1 = objscrip.BuyQtyL;
                    item.BuyRate1 = (objscrip.BuyRateL / Math.Pow(10, DecimalPnt)).ToString();
                    item.SellQuantity1 = objscrip.SellQtyL;
                    item.SellRate1 = (objscrip.SellRateL / Math.Pow(10, DecimalPnt)).ToString();
                    item.LTP1 = (objscrip.lastTradeRateL / Math.Pow(10, DecimalPnt)).ToString(); ;
                    item.LTQ1 = objscrip.lastTradeQtyL;
                    item.NoofBidBuy1 = objscrip.NoOfBidBuyL;
                    item.NoOfBidSell1 = objscrip.NoOfBidSellL;
                    item.OpenRateL = (objscrip.openRateL / Math.Pow(10, DecimalPnt)).ToString();
                    item.BRP = objscrip.BRP / Math.Pow(10, DecimalPnt);
                    item.CloseRateL = (objscrip.closeRateL / Math.Pow(10, DecimalPnt)).ToString();
                    item.HighRateL = (objscrip.highRateL / Math.Pow(10, DecimalPnt)).ToString();
                    item.LowRateL = (objscrip.lowRateL / Math.Pow(10, DecimalPnt)).ToString();
                    item.TotBuyQtyL = objscrip.totBuyQtyL;
                    item.TotSellQtyL = objscrip.totSellQtyL;
                    item.WtAvgRateL = (objscrip.wtAvgRateL / Math.Pow(10, DecimalPnt)).ToString();
                    item.CtValue = (objscrip.TrdValue / Math.Pow(10, DecimalPnt)).ToString();
                    item.CtVolume = objscrip.TrdVolume;
                    item.UpperCtLmt = objscrip.UprCtLmt.ToString(); ;
                    item.LowerCtLmt = objscrip.LowerCtLmt.ToString(); ;
                    item.CloseRateL = objscrip.closeRateL.ToString(); ;
                    item.FiftyTwoHigh = objscrip.FiftyTwoHigh.ToString(); ;
                    item.FiftyTwoLow = objscrip.FiftyTwoLow.ToString();




                }
                else
                {
                    item.BuyQualtity1 = 0;
                    item.BuyRate1 = "0";
                    item.SellQuantity1 = 0;
                    item.SellRate1 = "0";
                    item.LTP1 = "0";
                    item.LTQ1 = 0;
                    item.NoofBidBuy1 = 0;
                    item.NoOfBidSell1 = 0;
                    item.OpenRateL = "0";
                    item.CloseRateL = "0";
                    item.BRP = 0;
                    item.HighRateL = "0";
                    item.LowRateL = "0";
                    item.TotBuyQtyL = 0;
                    item.TotSellQtyL = 0;
                    item.WtAvgRateL = "0";
                    item.CtVolume = 0;
                    item.UpperCtLmt = "0";
                    item.LowerCtLmt = "0";
                    item.CloseRateL = "0";

                }
                item.IsVisible = true;
                //if (MasterSharedMemory.objDicDP.ContainsKey(ScripCode))
                //{
                //    item.FiftyTwoHigh = MasterSharedMemory.objDicDP[ScripCode].WeeksHighprice / Math.Pow(10, DecimalPnt);
                //    item.FiftyTwoLow = MasterSharedMemory.objDicDP[ScripCode].WeeksLowprice / Math.Pow(10, DecimalPnt);
                //}
                //else
                {
                    item.FiftyTwoHigh = "0";
                    item.FiftyTwoLow = "0";
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private static void UpdateGrid(int indexthis, ScripDetails _BstFive)
        {
            //if (this.ObjTouchlineDataCollection[index].IsVisible)
            //{
            //Code For COunters
            // int c;
            // CountUpdatedperView++;
            try
            {
                String Segment = CommonFunctions.GetSegmentID(ObjTouchlineDataCollection[indexthis].Scriptcode1);
                index = indexthis;
                int Scripcode_New = ObjTouchlineDataCollection[indexthis].Scriptcode1;
#if TWS
                DecimalPnt = CommonFunctions.GetDecimal(ObjTouchlineDataCollection[indexthis].Scriptcode1, "BSE", Segment);//BSE EQUITY
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
#endif
                int num = ObjTouchlineDataCollection[indexthis].BuyQualtity1;
                double num2;
                if (ObjTouchlineDataCollection[indexthis].BuyRate1 != "")
                    num2 = Convert.ToDouble(ObjTouchlineDataCollection[indexthis].BuyRate1);
                else
                    num2 = 0;
                int num3 = ObjTouchlineDataCollection[indexthis].SellQuantity1;
                double num4;
                if (ObjTouchlineDataCollection[indexthis].SellRate1 != "")
                    num4 = Convert.ToDouble(ObjTouchlineDataCollection[indexthis].SellRate1);
                else
                    num4 = 0;

                double ltpPrevious;
                if (ObjTouchlineDataCollection[indexthis].LTP1 != "")
                    ltpPrevious = Convert.ToDouble(ObjTouchlineDataCollection[indexthis].LTP1);
                else
                    ltpPrevious = 0;

                //Code For COunters
                //c=++ this.ObjTouchlineDataCollection[index].Counter;
                // WriteLog objWriteLog=new WriteLog();
                // objWriteLog.ScriptCode=ObjTouchlineDataCollection[index].ScriptId1;
                // objWriteLog.startTime=_BstFive.startTime;
                // objWriteLog.Count = c;
                // objWriteLog.BuyQtyL = _BstFive.BuyQtyL;
                // objWriteLog.BuyRateL = _BstFive.BuyRateL;
                // objWriteLog.SellQtyL = _BstFive.SellRateL;
                // objWriteLog.SellRateL = _BstFive.SellRateL;
                // objWriteLog.lastTradeRateL = _BstFive.lastTradeRateL;
                // objWriteLog.NoOfBidBuyL = _BstFive.NoOfBidBuyL;
                // objWriteLog.NoOfBidSellL = _BstFive.NoOfBidSellL;
                // objWriteLog.openRateL = _BstFive.openRateL;
                // objWriteLog.closeRateL = _BstFive.closeRateL;
                //_listWriteinFile.TryAdd(objWriteLog.Count, objWriteLog);
                //if (calculatetime)
                //{
                //    this.ObjTouchlineDataCollection[index].StartTime = _BstFive.startTime;
                //    calculatetime = false;
                //}
                //if (calculatetime1)
                //{
                //    this.ObjTouchlineDataCollection[index].StartTime1 = _BstFive.startTime;
                //    calculatetime1 = false;
                //      using (StreamWriter writer = new StreamWriter(@"E:\Deepshikha\IML.csv", false))
                //    {
                //        foreach (WriteLog line in _listWriteinFile.Values)
                //            writer.WriteLine(line.ScriptCode + ": " + line.startTime + ": " + line.Count + ": " + line.BuyQtyL + ": " +
                //                line.BuyRateL + ": " + line.SellQtyL + ": " + line.SellRateL
                //                + line.lastTradeRateL + ": " + line.NoOfBidBuyL + ": " +
                //                line.NoOfBidSellL + ": " + line.openRateL + ": " + line.closeRateL);
                //    }
                //}

                ObjTouchlineDataCollection[indexthis].BuyQualtity1 = _BstFive.BuyQtyL;
                ObjTouchlineDataCollection[indexthis].BuyRate1 = (_BstFive.BuyRateL / Math.Pow(10, DecimalPnt)).ToString(StringFormat) == "0" ? "" : (_BstFive.BuyRateL / Math.Pow(10, DecimalPnt)).ToString(StringFormat);

                objTouchlineDataCollection[indexthis].SellQuantity1 = _BstFive.SellQtyL;
                objTouchlineDataCollection[indexthis].SellRate1 = (_BstFive.SellRateL / Math.Pow(10, DecimalPnt)).ToString(StringFormat) == "0" ? "" : (_BstFive.SellRateL / Math.Pow(10, DecimalPnt)).ToString(StringFormat);
                ObjTouchlineDataCollection[indexthis].LTP1 = (_BstFive.lastTradeRateL / Math.Pow(10, DecimalPnt)).ToString(StringFormat) == "0" ? "" : (_BstFive.lastTradeRateL / Math.Pow(10, DecimalPnt)).ToString(StringFormat);
                objTouchlineDataCollection[indexthis].LTQ1 = _BstFive.lastTradeQtyL;
                objTouchlineDataCollection[indexthis].IndEqPrice = (_BstFive.IndicateEqPrice / Math.Pow(10, DecimalPnt)).ToString(StringFormat) == "0" ? "" : (_BstFive.IndicateEqPrice / Math.Pow(10, DecimalPnt)).ToString(StringFormat);
                objTouchlineDataCollection[indexthis].IndEqQty = _BstFive.IndicateEqQty;
                objTouchlineDataCollection[indexthis].NoOfTrds = _BstFive.NoOfTrades;
                objTouchlineDataCollection[indexthis].StrikePriceL = _BstFive.BuyRateL;
                //  objTouchlineDataCollection[indexthis].to
#if BOW
            if (_BstFive.LastTradeTime != "" &&( _BstFive.LastTradeTime.Contains("AM") || _BstFive.LastTradeTime.Contains("PM")))
            {
                objTouchlineDataCollection[index].LastTradeTime = DateTime.ParseExact(_BstFive.LastTradeTime, "hh:mm:ss tt", CultureInfo.InvariantCulture).ToString("HH:mm:ss");
            }
            else
            {
                objTouchlineDataCollection[index].LastTradeTime = DateTime.ParseExact(_BstFive.LastTradeTime, "hh:mm:ss", CultureInfo.InvariantCulture).ToString("HH:mm:ss");
            }
#elif TWS
                objTouchlineDataCollection[indexthis].LastTradeTime = _BstFive.LastTradeTime;
#endif
                objTouchlineDataCollection[indexthis].PrevLTPScripId = "Black";
                if (_BstFive.PrevcloseRateL != 0 && _BstFive.lastTradeRateL != 0)
                {
                    if (Math.Round((_BstFive.lastTradeRateL - _BstFive.PrevcloseRateL) * 100.0 / _BstFive.PrevcloseRateL, 2) > 0)
                    {
                        objTouchlineDataCollection[indexthis].PrevLTPScripId = "Blue";
                        _BstFive.ChangePercentage = string.Format("{0}{1} {2}", "+", Math.Round(((_BstFive.lastTradeRateL - _BstFive.PrevcloseRateL) * 100.0 / _BstFive.PrevcloseRateL), 2).ToString("0.00"), "%");
                    }
                    else
                    {
                        objTouchlineDataCollection[indexthis].PrevLTPScripId = "Red";
                        _BstFive.ChangePercentage = string.Format("{0} {1}", Math.Round(((_BstFive.lastTradeRateL - _BstFive.PrevcloseRateL) * 100.0 / _BstFive.PrevcloseRateL), 2).ToString("0.00"), "%");
                    }
                }
                objTouchlineDataCollection[indexthis].ChangePercentage = _BstFive.ChangePercentage;
                /*if (objTouchlineDataCollection[indexthis].ChangePercentage != null)
                {
                    if (objTouchlineDataCollection[indexthis].ChangePercentage.StartsWith("+"))
                    {
                        objTouchlineDataCollection[indexthis].PrevLTPScripId = "Blue";
                    }
                    else if (objTouchlineDataCollection[indexthis].ChangePercentage.StartsWith("-"))
                    {
                        objTouchlineDataCollection[indexthis].PrevLTPScripId = "Red";
                    }
                    else
                    {
                        objTouchlineDataCollection[indexthis].PrevLTPScripId = "Black";
                    }
                }
                else
                    objTouchlineDataCollection[indexthis].PrevLTPScripId = "Black";*/

                objTouchlineDataCollection[indexthis].PremDisc = ((_BstFive.lastTradeRateL - _BstFive.closeRateL) / Math.Pow(10, DecimalPnt)).ToString(StringFormat) == "0" ? "" : ((_BstFive.lastTradeRateL - _BstFive.closeRateL) / Math.Pow(10, DecimalPnt)).ToString(StringFormat);
                //ObjTouchlineDataCollection[index].NoofBidBuy1 = _BstFive.NoOfBidBuyL;
                //ObjTouchlineDataCollection[index].NoOfBidSell1 = _BstFive.NoOfBidSellL;
                ObjTouchlineDataCollection[indexthis].OpenRateL = (_BstFive.openRateL / Math.Pow(10, DecimalPnt)).ToString(StringFormat) == "0" ? "" : (_BstFive.openRateL / Math.Pow(10, DecimalPnt)).ToString(StringFormat);
                ObjTouchlineDataCollection[indexthis].CloseRateL = (_BstFive.closeRateL / Math.Pow(10, DecimalPnt)).ToString(StringFormat) == "0" ? "" : (_BstFive.closeRateL / Math.Pow(10, DecimalPnt)).ToString(StringFormat);

                if (_BstFive.closeRateL == 0)
                {
                    if (_BstFive.PrevcloseRateL == 0)
                    {
                        ObjTouchlineDataCollection[indexthis].CloseRateL = (MasterSharedMemory.objDicDP.Where(x => x.Key == ObjTouchlineDataCollection[indexthis].Scriptcode1).Select(x => x.Value.PreviousClosePrice).FirstOrDefault() / Math.Pow(10, DecimalPnt)).ToString(StringFormat) == "0" ? "" : (_BstFive.lastTradeRateL / Math.Pow(10, DecimalPnt)).ToString(StringFormat);
                    }
                    else
                    {
                        ObjTouchlineDataCollection[indexthis].CloseRateL = (_BstFive.PrevcloseRateL / Math.Pow(10, DecimalPnt)).ToString(StringFormat) == "0" ? "" : (_BstFive.PrevcloseRateL / Math.Pow(10, DecimalPnt)).ToString(StringFormat);
                    }
                }
                //  bstfivedata.CloseRate = MasterSharedMemory.objDicDP.Where(x => x.Key == ScripCode).Select(x => x.Value.PreviousClosePrice).FirstOrDefault() / Math.Pow(10, DecimalPoint);
                else
                    ObjTouchlineDataCollection[indexthis].CloseRateL = (_BstFive.closeRateL / Math.Pow(10, DecimalPnt)).ToString(StringFormat) == "0" ? "" : (_BstFive.closeRateL / Math.Pow(10, DecimalPnt)).ToString(StringFormat);

                ObjTouchlineDataCollection[indexthis].HighRateL = (_BstFive.highRateL / Math.Pow(10, DecimalPnt)).ToString(StringFormat) == "0" ? "" : (_BstFive.highRateL / Math.Pow(10, DecimalPnt)).ToString(StringFormat);
                ObjTouchlineDataCollection[indexthis].LowRateL = (_BstFive.lowRateL / Math.Pow(10, DecimalPnt)).ToString(StringFormat) == "0" ? "" : (_BstFive.lowRateL / Math.Pow(10, DecimalPnt)).ToString(StringFormat);
                ObjTouchlineDataCollection[indexthis].UpperCtLmt = (_BstFive.UprCtLmt / Math.Pow(10, DecimalPnt)).ToString(StringFormat) == "0" ? "" : (_BstFive.UprCtLmt / Math.Pow(10, DecimalPnt)).ToString(StringFormat);
                objTouchlineDataCollection[indexthis].LowerCtLmt = (_BstFive.LowerCtLmt / Math.Pow(10, DecimalPnt)).ToString(StringFormat) == "0" ? "" : (_BstFive.LowerCtLmt / Math.Pow(10, DecimalPnt)).ToString(StringFormat);
                ObjTouchlineDataCollection[indexthis].TotBuyQtyL = _BstFive.totBuyQtyL;
                ObjTouchlineDataCollection[indexthis].TotSellQtyL = _BstFive.totSellQtyL;
                ObjTouchlineDataCollection[indexthis].WtAvgRateL = (_BstFive.wtAvgRateL / Math.Pow(10, DecimalPnt)).ToString(StringFormat) == "0" ? "" : (_BstFive.wtAvgRateL / Math.Pow(10, DecimalPnt)).ToString(StringFormat); ;
                ObjTouchlineDataCollection[indexthis].CtVolume = _BstFive.TrdVolume;
                ObjTouchlineDataCollection[indexthis].CtValue = (_BstFive.TrdValue / Math.Pow(10, DecimalPnt)).ToString() + "  " + _BstFive.Unit_c;
                ObjTouchlineDataCollection[indexthis].OI = _BstFive.OI;
                ObjTouchlineDataCollection[indexthis].NoOfTrds = _BstFive.NoOfTrades;



                double LTP;
                if (ObjTouchlineDataCollection[indexthis].LTP1 != "")
                    LTP = Convert.ToDouble(ObjTouchlineDataCollection[indexthis].LTP1);
                else
                    LTP = 0;
                //   ObjTouchlineDataCollection.ListChanged += ObjTouchlineDataCollection_ListChanged;
                //if (!this.ObjTouchlineDataCollection[index].FirstTickExecuted)
                //{
                //    this.ObjTouchlineDataCollection[index].FirstTickExecuted = true;
                //}

                if (ObjTouchlineDataCollection[indexthis].BuyQualtity1 != 0 && num != 0)
                {
                    if (num < ObjTouchlineDataCollection[indexthis].BuyQualtity1)
                    {
                        ObjTouchlineDataCollection[indexthis].PrevBuyQualtity1 = UtilityLoginDetails.GETInstance.UpTrendColorGlobal;
                        ObjTouchlineDataCollection[indexthis].ForegroundColorBuyQuantity = "White";
                    }
                    else if (num > ObjTouchlineDataCollection[indexthis].BuyQualtity1)
                    {
                        ObjTouchlineDataCollection[indexthis].PrevBuyQualtity1 = UtilityLoginDetails.GETInstance.DownTrendColorGlobal;
                        ObjTouchlineDataCollection[indexthis].ForegroundColorBuyQuantity = "Black";
                    }

                    else if (num == ObjTouchlineDataCollection[indexthis].BuyQualtity1)
                    {
                        ObjTouchlineDataCollection[indexthis].ForegroundColorBuyQuantity = "Black";
                        if (indexthis % 2 != 0)
                        {
                            ObjTouchlineDataCollection[indexthis].PrevBuyQualtity1 = "Transparent";
                        }
                        else
                        {
                            ObjTouchlineDataCollection[indexthis].PrevBuyQualtity1 = "White";
                        }
                    }
                }
                else
                {

                    ObjTouchlineDataCollection[indexthis].ForegroundColorBuyQuantity = "Black";
                    if (indexthis % 2 != 0)
                    {
                        ObjTouchlineDataCollection[indexthis].PrevBuyQualtity1 = "Transparent";
                    }
                    else
                    {
                        ObjTouchlineDataCollection[indexthis].PrevBuyQualtity1 = "White";
                    }
                }
                if (ObjTouchlineDataCollection[indexthis].BuyRate1 != "")
                {
                    if (Convert.ToDouble(ObjTouchlineDataCollection[indexthis].BuyRate1) != 0 && num2 != 0)
                    {
                        if (num2 < Convert.ToDouble(ObjTouchlineDataCollection[indexthis].BuyRate1))
                        {
                            ObjTouchlineDataCollection[indexthis].PrevBuyRate1 = UtilityLoginDetails.GETInstance.UpTrendColorGlobal;
                            ObjTouchlineDataCollection[indexthis].ForegroundBuyRate = "White";
                        }
                        else if (num2 > Convert.ToDouble(ObjTouchlineDataCollection[indexthis].BuyRate1))
                        {
                            ObjTouchlineDataCollection[indexthis].PrevBuyRate1 = UtilityLoginDetails.GETInstance.DownTrendColorGlobal;
                            ObjTouchlineDataCollection[indexthis].ForegroundBuyRate = "Black";
                        }
                        else if (num2 == Convert.ToDouble(ObjTouchlineDataCollection[indexthis].BuyRate1))
                        {
                            ObjTouchlineDataCollection[indexthis].ForegroundBuyRate = "Black";
                            if (indexthis % 2 != 0)
                            {
                                ObjTouchlineDataCollection[indexthis].PrevBuyRate1 = "Transparent";
                            }
                            else
                            {
                                ObjTouchlineDataCollection[indexthis].PrevBuyRate1 = "White";
                            }
                        }
                    }
                    else
                    {
                        ObjTouchlineDataCollection[indexthis].ForegroundBuyRate = "Black";
                        if (indexthis % 2 != 0)
                        {
                            ObjTouchlineDataCollection[indexthis].PrevBuyRate1 = "Transparent";
                        }
                        else
                        {
                            ObjTouchlineDataCollection[indexthis].PrevBuyRate1 = "White";
                        }
                    }
                }
                else
                {
                    ObjTouchlineDataCollection[indexthis].ForegroundBuyRate = "Black";
                    if (indexthis % 2 != 0)
                    {
                        ObjTouchlineDataCollection[indexthis].PrevBuyRate1 = "Transparent";
                    }
                    else
                    {
                        ObjTouchlineDataCollection[indexthis].PrevBuyRate1 = "White";
                    }
                }

                if (ObjTouchlineDataCollection[indexthis].SellQuantity1 != 0 && num3 != 0)
                {
                    if (num3 < ObjTouchlineDataCollection[indexthis].SellQuantity1)
                    {
                        ObjTouchlineDataCollection[indexthis].PrevSellQuantity1 = UtilityLoginDetails.GETInstance.UpTrendColorGlobal;
                        ObjTouchlineDataCollection[indexthis].ForegroundSellQ = "White";
                    }
                    else if (num3 > ObjTouchlineDataCollection[indexthis].SellQuantity1)
                    {
                        ObjTouchlineDataCollection[indexthis].PrevSellQuantity1 = UtilityLoginDetails.GETInstance.DownTrendColorGlobal;
                        ObjTouchlineDataCollection[indexthis].ForegroundSellQ = "Black";
                    }
                    else if (num3 == ObjTouchlineDataCollection[indexthis].SellQuantity1)
                    {
                        ObjTouchlineDataCollection[indexthis].ForegroundSellQ = "Black";
                        if (indexthis % 2 != 0)
                        {
                            ObjTouchlineDataCollection[indexthis].PrevSellQuantity1 = "Transparent";
                        }
                        else
                        {
                            ObjTouchlineDataCollection[indexthis].PrevSellQuantity1 = "White";
                        }
                    }
                }
                else
                {
                    ObjTouchlineDataCollection[indexthis].ForegroundSellQ = "Black";
                    if (indexthis % 2 != 0)
                    {
                        ObjTouchlineDataCollection[indexthis].PrevSellQuantity1 = "Transparent";
                    }
                    else
                    {
                        ObjTouchlineDataCollection[indexthis].PrevSellQuantity1 = "White";
                    }
                }
                if (ObjTouchlineDataCollection[indexthis].SellRate1 != "")
                {
                    if (Convert.ToDouble(ObjTouchlineDataCollection[indexthis].SellRate1) != 0 && num4 != 0)
                    {
                        if (num4 < Convert.ToDouble(ObjTouchlineDataCollection[indexthis].SellRate1))
                        {
                            ObjTouchlineDataCollection[indexthis].PrevSellRate1 = UtilityLoginDetails.GETInstance.UpTrendColorGlobal;
                            ObjTouchlineDataCollection[indexthis].ForegroundSellR = "White";
                        }
                        else if (num4 > Convert.ToDouble(ObjTouchlineDataCollection[indexthis].SellRate1))
                        {
                            ObjTouchlineDataCollection[indexthis].PrevSellRate1 = UtilityLoginDetails.GETInstance.DownTrendColorGlobal;
                            ObjTouchlineDataCollection[indexthis].ForegroundSellR = "Black";
                        }
                        else if (num4 == Convert.ToDouble(ObjTouchlineDataCollection[indexthis].SellRate1))
                        {
                            ObjTouchlineDataCollection[indexthis].ForegroundSellR = "Black";
                            if (indexthis % 2 != 0)
                            {
                                ObjTouchlineDataCollection[indexthis].PrevSellRate1 = "Transparent";
                            }
                            else
                            {
                                ObjTouchlineDataCollection[indexthis].PrevSellRate1 = "White";
                            }
                        }
                    }
                    else
                    {
                        ObjTouchlineDataCollection[indexthis].ForegroundSellR = "Black";
                        if (indexthis % 2 != 0)
                        {
                            ObjTouchlineDataCollection[indexthis].PrevSellRate1 = "Transparent";
                        }
                        else
                        {
                            ObjTouchlineDataCollection[indexthis].PrevSellRate1 = "White";
                        }
                    }
                }
                else
                {
                    ObjTouchlineDataCollection[indexthis].ForegroundSellR = "Black";
                    if (indexthis % 2 != 0)
                    {
                        ObjTouchlineDataCollection[indexthis].PrevSellRate1 = "Transparent";
                    }
                    else
                    {
                        ObjTouchlineDataCollection[indexthis].PrevSellRate1 = "White";
                    }
                }


                if (ObjTouchlineDataCollection[indexthis].LTP1 != "")
                {
                    if (ltpPrevious == 0 && Convert.ToDouble(ObjTouchlineDataCollection[indexthis].LTP1) != 0)
                    {

                        if (Convert.ToDouble(ObjTouchlineDataCollection[indexthis].LTP1) > Convert.ToDouble(ObjTouchlineDataCollection[indexthis].CloseRateL))
                        {
                            ObjTouchlineDataCollection[indexthis].PrevLTP1 = UtilityLoginDetails.GETInstance.UpTrendColorGlobal;
                            ObjTouchlineDataCollection[indexthis].ForegroundLTP = "White";
                        }
                        else if (Convert.ToDouble(ObjTouchlineDataCollection[indexthis].LTP1) < Convert.ToDouble(ObjTouchlineDataCollection[indexthis].CloseRateL))
                        {
                            ObjTouchlineDataCollection[indexthis].PrevLTP1 = UtilityLoginDetails.GETInstance.DownTrendColorGlobal;
                            ObjTouchlineDataCollection[indexthis].ForegroundLTP = "Black";
                        }
                        else
                        {
                            ObjTouchlineDataCollection[indexthis].ForegroundLTP = "Black";
                            ObjTouchlineDataCollection[indexthis].PrevLTP1 = "White";
                        }
                    }
                    else if (Convert.ToDouble(ObjTouchlineDataCollection[indexthis].LTP1) != 0 && ltpPrevious != 0)
                    {
                        if (ltpPrevious < Convert.ToDouble(ObjTouchlineDataCollection[indexthis].LTP1))
                        {
                            ObjTouchlineDataCollection[indexthis].PrevLTP1 = UtilityLoginDetails.GETInstance.UpTrendColorGlobal;
                            ObjTouchlineDataCollection[indexthis].ForegroundLTP = "White";
                        }
                        else if (ltpPrevious > Convert.ToDouble(ObjTouchlineDataCollection[indexthis].LTP1))
                        {
                            ObjTouchlineDataCollection[indexthis].PrevLTP1 = UtilityLoginDetails.GETInstance.DownTrendColorGlobal;
                            ObjTouchlineDataCollection[indexthis].ForegroundLTP = "Black";
                        }
                        else if (ltpPrevious == Convert.ToDouble(ObjTouchlineDataCollection[indexthis].LTP1))
                        {
                            ObjTouchlineDataCollection[indexthis].ForegroundLTP = "Black";
                            if (indexthis % 2 != 0)
                            {
                                ObjTouchlineDataCollection[indexthis].PrevLTP1 = "Transparent";
                            }
                            else
                            {
                                ObjTouchlineDataCollection[indexthis].PrevLTP1 = "White";
                            }
                        }
                    }
                }
                else
                {
                    ObjTouchlineDataCollection[indexthis].ForegroundLTP = "Black";
                    if (indexthis % 2 != 0)
                    {
                        ObjTouchlineDataCollection[indexthis].PrevLTP1 = "Transparent";
                    }
                    else
                    {
                        ObjTouchlineDataCollection[indexthis].PrevLTP1 = "White";
                    }
                }

                /*
                if (LTP != 0 && ObjTouchlineDataCollection[index].CloseRateL != 0)
                {
                    if (LTP > ObjTouchlineDataCollection[index].CloseRateL)
                    {
                        ObjTouchlineDataCollection[index].PrevLTP1 = App.UpTrendColorGlobal;
                        ObjTouchlineDataCollection[index].ForegroundLTP = "White";
                    }
                    else if (LTP < ObjTouchlineDataCollection[index].CloseRateL)
                    {
                        ObjTouchlineDataCollection[index].PrevLTP1 = App.DownTrendColorGlobal;
                        ObjTouchlineDataCollection[index].ForegroundLTP = "White";
                    }
                    else if (LTP == ObjTouchlineDataCollection[index].CloseRateL)
                    {
                        ObjTouchlineDataCollection[index].ForegroundLTP = "White";
                        if (index % 2 != 0)
                        {
                            ObjTouchlineDataCollection[index].PrevLTP1 = "#FF333972";
                        }
                        else
                        {
                            ObjTouchlineDataCollection[index].PrevLTP1 = "#FF333972";
                        }
                    }
                }
                else
                {
                    ObjTouchlineDataCollection[index].ForegroundLTP = "White";
                    if (index % 2 != 0)
                    {
                        ObjTouchlineDataCollection[index].PrevLTP1 = "#FF333972";
                    }
                    else
                    {
                        ObjTouchlineDataCollection[index].PrevLTP1 = "#FF333972";
                    }

                }*/
                if (MasterSharedMemory.objDicDP.Where(x => x.Key == Scripcode_New).Select(x => x.Value).FirstOrDefault() != null)
                {
                    _BstFive.FiftyTwoHigh = CommonFunctions.GetValueInDecimal(MasterSharedMemory.objDicDP[Scripcode_New].WeeksHighprice, DecimalPnt) == "0" ? "" : CommonFunctions.GetValueInDecimal(MasterSharedMemory.objDicDP[Scripcode_New].WeeksHighprice, DecimalPnt);
                    _BstFive.FiftyTwoLow = CommonFunctions.GetValueInDecimal(MasterSharedMemory.objDicDP[Scripcode_New].WeeksLowprice, DecimalPnt) == "0" ? "" : CommonFunctions.GetValueInDecimal(MasterSharedMemory.objDicDP[Scripcode_New].WeeksLowprice, DecimalPnt);
                    if (!string.IsNullOrEmpty(MasterSharedMemory.objDicDP[Scripcode_New].Dateof52weeksHighprice))
                        _BstFive.FiftyTwoHighDate = DateTime.ParseExact(MasterSharedMemory.objDicDP[Scripcode_New].Dateof52weeksHighprice, "ddMMyyyy", CultureInfo.InvariantCulture).ToString("dd-MM-yyyy");

                    if (!string.IsNullOrEmpty(MasterSharedMemory.objDicDP[Scripcode_New].Dateof52weeksLowprice))
                        _BstFive.FiftyTwoLowDate = DateTime.ParseExact(MasterSharedMemory.objDicDP[Scripcode_New].Dateof52weeksLowprice, "ddMMyyyy", CultureInfo.InvariantCulture).ToString("dd-MM-yyyy");
                }

                if (_BstFive.FiftyTwoHigh != "")
                {
                    if (LTP != 0 && Convert.ToDouble(_BstFive.FiftyTwoHigh) != 0)
                    {
                        if (LTP > Convert.ToDouble(_BstFive.FiftyTwoHigh))
                        {
                            // ObjTouchlineDataCollection[index].FiftyTwoHighBColor = App.UpTrendColorGlobal;
                            ObjTouchlineDataCollection[indexthis].FiftyTwoHigh = LTP.ToString(StringFormat) == "0" ? "" : (LTP).ToString(StringFormat);
                            MasterSharedMemory.objDicDP[ObjTouchlineDataCollection[indexthis].Scriptcode1].WeeksHighprice = _BstFive.lastTradeRateL;
                            MasterSharedMemory.objDicDP[ObjTouchlineDataCollection[indexthis].Scriptcode1].Dateof52weeksHighprice = CommonFunctions.GetDate().ToString("ddMMyyyy");//DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy"), "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("ddMMyyyy");

                        }
                        else
                        {
                            ObjTouchlineDataCollection[indexthis].FiftyTwoHigh = (_BstFive.FiftyTwoHigh).ToString() == "0" ? "" : (_BstFive.FiftyTwoHigh).ToString();
                        }
                    }
                    else
                        ObjTouchlineDataCollection[indexthis].FiftyTwoHigh = _BstFive.FiftyTwoHigh.ToString() == "0" ? "" : (_BstFive.FiftyTwoHigh).ToString();
                }
                else
                {
                    ObjTouchlineDataCollection[indexthis].FiftyTwoHigh = _BstFive.FiftyTwoHigh.ToString() == "0" ? "" : (_BstFive.FiftyTwoHigh).ToString();
                }

                if (_BstFive.FiftyTwoLow != "")
                {
                    if (LTP != 0 && Convert.ToDouble(_BstFive.FiftyTwoLow) != 0)
                    {
                        if (LTP < Convert.ToDouble(_BstFive.FiftyTwoLow))
                        {
                            //ObjTouchlineDataCollection[index].FiftyTwoLowBColor = App.DownTrendColorGlobal;
                            ObjTouchlineDataCollection[indexthis].FiftyTwoLow = LTP.ToString(StringFormat) == "0" ? "" : (LTP).ToString(StringFormat);
                            MasterSharedMemory.objDicDP[ObjTouchlineDataCollection[indexthis].Scriptcode1].WeeksLowprice = _BstFive.lastTradeRateL;
                            MasterSharedMemory.objDicDP[ObjTouchlineDataCollection[indexthis].Scriptcode1].Dateof52weeksLowprice = CommonFunctions.GetDate().ToString("ddMMyyyy");//DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy"), "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("ddMMyyyy");

                        }
                        else
                        {
                            ObjTouchlineDataCollection[indexthis].FiftyTwoLow = _BstFive.FiftyTwoLow.ToString() == "0" ? "" : (_BstFive.FiftyTwoLow).ToString();

                        }
                    }
                    else
                        ObjTouchlineDataCollection[indexthis].FiftyTwoLow = _BstFive.FiftyTwoLow.ToString() == "0" ? "" : (_BstFive.FiftyTwoLow).ToString();
                }
                else
                {
                    ObjTouchlineDataCollection[indexthis].FiftyTwoLow = _BstFive.FiftyTwoLow.ToString() == "0" ? "" : (_BstFive.FiftyTwoLow).ToString();
                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
                System.Windows.MessageBox.Show("Error in displaying indices!");
                return;
            }

            //}
            //    OIMainDetails[] objOiMainD= OIMemory.SubscribeOIMemoryDict.Select(x => x.Value.ObjOIMainDetails).FirstOrDefault();
            //  objOiMainD.Where(x=>x.InstrumentID)


        }
        public static void UPdateOI(ObservableCollection<OpenInterestModel> obj)
        {

            foreach (MarketWatchModel _objMarketwatchGridObject in ObjTouchlineDataCollection)
            {

                if (ObjTouchlineDataCollection != null && ObjTouchlineDataCollection.Count != 0)
                {
                    OpenInterestModel objOIMainDe = obj.Where(x => x.SeriesCode == _objMarketwatchGridObject.Scriptcode1).FirstOrDefault();
                    if (objOIMainDe != null)
                    {
                        ObjTouchlineDataCollection[index].OIChange = objOIMainDe.ChangeInOI;
                        ObjTouchlineDataCollection[index].OIValue = objOIMainDe.OIValue == "0" ? "" : (objOIMainDe.OIValue);
                        ObjTouchlineDataCollection[index].OI = objOIMainDe.OI;
                    }
                }

            }
        }
        private void UpdateGrid_tabSlection(object Parameter)
        {
            if (Parameter != null)
            {

                try
                {
                    int keysWithMatchingValues = 0;
                    int count = 0;

                    int PrevTLCount = 0;

                    var param = (Tuple<object, object>)Parameter;
                    System.Windows.Controls.DataGrid MainDataGrid = param.Item1 as System.Windows.Controls.DataGrid;
                    System.Windows.Controls.TabControl maintabcontrol = param.Item2 as System.Windows.Controls.TabControl;
                    if (maintabcontrol.SelectedItem != null)
                    {
                        foreach (DataGridColumn Colm in MainDataGrid.Columns)
                        {
                            if (Colm.SortDirection != null)
                            {
                                Colm.SortDirection = null;
                                break;
                            }
                        }
                        ResultSort._SortMode = 0;

                        if (maintabcontrol.SelectedIndex == 0)
                            TabName = ((HeaderedContentControl)maintabcontrol.SelectedItem).Header.ToString();
                        else
                            TabName = ((TabCloseButton)((System.Windows.Controls.HeaderedContentControl)maintabcontrol.SelectedItem).Header).TabLblTitle.Content.ToString();
                        UserDetails.SelectedTab = TabName;
                        PrevTLCount = objTouchlineDataCollection.Count;
                        objTouchlineDataCollection.Clear();
                        if (maintabcontrol.SelectedIndex == 0)
                        {
                            if (TabName != null)
                            {
#if TWS
                                CookTouchLineData(null);
#elif BOW
                                CookTouchLineData();
#endif
                                TabName = ((HeaderedContentControl)maintabcontrol.SelectedItem).Header.ToString();

                                if (PrevTLCount == objTouchlineDataCollection.Count)
                                    DataGrid_SuscribeList();

                                return;
                            }


                        }

                        //keysWithMatchingValues = MasterSharedMemory.objMasterDicSyb.Where(p => p.Value.IndexID.ToUpper().Equals(((System.Windows.Controls.HeaderedContentControl)(maintabcontrol.SelectedItem)).Header)).Select(p => p.Key).FirstOrDefault();
                        keysWithMatchingValues = MasterSharedMemory.objMasterDicSyb.Where(p => p.Value.IndexID.ToUpper().Equals(TabName)).Select(p => p.Key).FirstOrDefault();
                        if (((System.Windows.FrameworkElement)maintabcontrol.SelectedItem).Name == "Userdefined")
                        {
                            //if (keysWithMatchingValues != 0)
                            {
#if TWS
                                CookTouchLineData(TabName);
#elif BOW
                            CookTouchLineData();
#endif
                            }
                        }
                        else
                        {
                            //SubscribeScripMemory.objMasterSubscribeScrip.Clear();

                            foreach (var item in MasterSharedMemory.MainListSybas.Where(x => x.IndexCodeSub == keysWithMatchingValues))
                            {
                                SubscribeList.SubscribeScrip s = new SubscribeScrip();
                                int scripCode = item.ScripCodeSyb;

                                if ((scripCode != 0) && (!CommonFunctions.ValidScripOrNot(scripCode)))
                                    continue;

                                MarketWatchModel sc = new MarketWatchModel();
                                ScripDetails objscrip = new ScripDetails();
                                sc.Scriptcode1 = scripCode;
                                sc.ScriptId1 = CommonFunctions.GetScripId(scripCode, Enumerations.Exchange.BSE, Enumerations.Segment.Equity);//BSE EQUITY
                                sc.Index = count++;
                                sc.IsVisible = true;
                                DecimalPnt = CommonFunctions.GetDecimal(sc.Scriptcode1, Enumerations.Exchange.BSE, Enumerations.Segment.Equity);

                                //objscrip = BroadcastMasterMemory.objScripDetailsConDict.Where(x => x.Key == scripCode).Select(x => x.Value).FirstOrDefault();

#if TWS
                                //s.ScripCode_l = scripCode;
                                //s.UpdateFlag_s = 1;
                                //if (SubscribeScripMemory.objMasterSubscribeScrip.ContainsKey(s.ScripCode_l))
                                //{ }
                                //else
                                //    SubscribeScripMemory.objMasterSubscribeScrip.TryAdd(s.ScripCode_l, s);
                                //BroadcastReceiver.ScripDetails Br = BroadcastMaster.BroadcastMasterMemory.objScripDetailsConDict.Where(x => x.Key == scripCode).Select(x => x.Value).FirstOrDefault();
                                //objscrip = MainWindowVM.UpdateScripDataFromMemory(Br);

#elif BOW
                                SocketConnection.MessageArrivedEventArgs lobjMessageArrivedEventArgs = default(SocketConnection.MessageArrivedEventArgs);
                                lobjMessageArrivedEventArgs = null;
                                MarketWatchConstants.mobjSync_BroadcastMsgQueue.TryDequeue(out lobjMessageArrivedEventArgs);

                                string[] larrstrCol = lobjMessageArrivedEventArgs.MessageString.Split('|');
                                objscrip = MainWindowVM.UpdateScripDataFromMemory(larrstrCol);
#endif
                                //objscrip.ScripID = sc.ScriptId1;
                                //objscrip.ScriptCode_BseToken_NseToken = scripCode;
                                //Initialize_Fields(objscrip, ref sc, DecimalPnt, sc.Scriptcode1);

                                // ScripCodeList.Add(scripCode);
                                objTouchlineDataCollection.Add(sc);
                                //objBroadCastProcessor_OnBroadCastRecievedNew(objscrip);
                            }
                        }

                        if (PrevTLCount == objTouchlineDataCollection.Count)
                            DataGrid_SuscribeList();

                        ScripProfilingVM.LoadMapping();
                        UpdatingColumnProfile(TabName, false);

                    }

                }
                catch (Exception ex)
                {
                    ExceptionUtility.LogError(ex);
                    System.Windows.MessageBox.Show("Error while showing indices!");
                    return;
                }

                finally
                {
                    TitleTouchLine = "TouchLine - " + CurrentTabSlected + "-" + ObjTouchlineDataCollection.Count;
                    SearchTemplist = ObjTouchlineDataCollection.Cast<MarketWatchModel>().ToList();
                    //BroadCastProcessor.objScripCodeFromSettingsLst = ScripCodeList;
                    //BroadCastProcessor.OnBroadCastRecieved += objBroadCastProcessor_OnBroadCastRecieved;

                }
            }
        }

        private void UpdateDataGrid(object parameter)
        {
            if (parameter != null)
            {
                try
                {
                    var param = (Tuple<object, object>)parameter;
                    System.Windows.Controls.DataGrid MainDataGrid = param.Item1 as System.Windows.Controls.DataGrid;
                    System.Windows.Controls.TabControl maintabcontrol = param.Item2 as System.Windows.Controls.TabControl;
                    TabName = ((HeaderedContentControl)maintabcontrol.SelectedItem).Header.ToString();

                    if (MarketsComboSelectedItem == MarketsCombo[0])
                        return;

                    if (Predefined_Flag)
                    {
                        UserDefined_Flag = false;
                        ScripProfComboSelectedItem = ScripProfilingCombo[0];
                    }

                    //foreach (TabItem item in maintabcontrol.Items)
                    //{
                    //    if (item.Header.ToString() == MarketsComboSelectedItem)
                    //    {
                    //        item.IsSelected = true;
                    //        TitleTouchLine = "TouchLine - " + CurrentTabSlected + "-" + ObjTouchlineDataCollection.Count;
                    //        return;
                    //    }
                    //}

                    for (int i = 0; i < maintabcontrol.Items.Count; i++)
                    {
                        TabItem tabItem = maintabcontrol.ItemContainerGenerator.Items[i] as TabItem;
                        string tabName;

                        if (tabItem.Header.ToString() == "Default")
                            tabName = tabItem.Header.ToString();
                        else
                            tabName = ((TabCloseButton)tabItem.Header).TabLblTitle.Content.ToString();

                        if (tabName == MarketsComboSelectedItem && tabItem.Name == "Predefined")
                        {
                            tabItem.IsSelected = true;
                            TitleTouchLine = "TouchLine - " + CurrentTabSlected + "-" + ObjTouchlineDataCollection.Count;
                            return;
                        }
                    }

                    TabCloseButton tabCloseButton = new TabCloseButton();
                    tabCloseButton.TabLblTitle.Content = MarketsComboSelectedItem;
                    tabCloseButton.TabBtnClose.Click += new RoutedEventHandler(TabCloseButton_VM.TabBtnClose_Click);

                    TabItem tabitem = new TabItem();
                    //tabitem.Header = MarketsComboSelectedItem;
                    tabitem.Header = tabCloseButton;


                    tabitem.FontFamily = new System.Windows.Media.FontFamily("Verdana");
                    tabitem.FontWeight = FontWeights.Bold;
                    maintabcontrol.Items.Add(tabitem);
                    tabitem.Name = "Predefined";
                    tabitem.IsSelected = true;
                    TitleTouchLine = "TouchLine - " + CurrentTabSlected + "-" + ObjTouchlineDataCollection.Count;
                }
                catch (Exception ex)
                {
                    ExceptionUtility.LogError(ex);
                    System.Windows.MessageBox.Show("Error while showing indices!");
                }

                finally
                {
                    UserDefined_Flag = true;
                    //BroadCastProcessor.OnBroadCastRecieved += objBroadCastProcessor_OnBroadCastRecieved;
                }
            }
        }


        private void UpdateDataGrid_UserDefined(object parameter)
        {

            if (parameter != null && ScripProfilingCombo.Count > 0)
            {
                try
                {
                    var param = (Tuple<object, object>)parameter;
                    System.Windows.Controls.DataGrid MainDataGrid = param.Item1 as System.Windows.Controls.DataGrid;
                    System.Windows.Controls.TabControl maintabcontrol = param.Item2 as System.Windows.Controls.TabControl;

                    if (ScripProfComboSelectedItem == ScripProfilingCombo[0])
                        return;

                    if (UserDefined_Flag)
                    {
                        Predefined_Flag = false;
                        MarketsComboSelectedItem = MarketsCombo[0];
                    }
                    //foreach (TabItem item in maintabcontrol.Items)
                    //{
                    //    if (item.Header.ToString() == ScripProfComboSelectedItem)
                    //    {
                    //        item.IsSelected = true;
                    //        TitleTouchLine = "TouchLine - " + CurrentTabSlected + "-" + ObjTouchlineDataCollection.Count;
                    //        return;
                    //    }
                    //}

                    for (int i = 0; i < maintabcontrol.Items.Count; i++)
                    {
                        TabItem tabItem = maintabcontrol.ItemContainerGenerator.Items[i] as TabItem;
                        string tabName;

                        if (tabItem.Header.ToString() == "Default")
                            tabName = tabItem.Header.ToString();
                        else
                            tabName = ((TabCloseButton)tabItem.Header).TabLblTitle.Content.ToString();

                        if (tabName == ScripProfComboSelectedItem && tabItem.Name == "Userdefined")
                        {
                            tabItem.IsSelected = true;
                            TitleTouchLine = "TouchLine - " + CurrentTabSlected + "-" + ObjTouchlineDataCollection.Count;
                            return;
                        }
                    }

                    TabCloseButton tabCloseButton = new TabCloseButton();
                    tabCloseButton.TabLblTitle.Content = ScripProfComboSelectedItem;
                    tabCloseButton.TabBtnClose.Click += new RoutedEventHandler(TabCloseButton_VM.TabBtnClose_Click);


                    TabItem tabitem = new TabItem();
                    //tabitem.Header = ScripProfComboSelectedItem;
                    tabitem.Header = tabCloseButton;

                    tabitem.FontFamily = new System.Windows.Media.FontFamily("Verdana");
                    tabitem.FontWeight = FontWeights.Bold;
                    maintabcontrol.Items.Add(tabitem);
                    tabitem.Name = "Userdefined";
                    tabitem.IsSelected = true;
#if BOW
                   CookTouchLineData();
#endif
                    TitleTouchLine = "TouchLine - " + CurrentTabSlected + "-" + ObjTouchlineDataCollection.Count;
                }
                catch (Exception ex)
                {
                    ExceptionUtility.LogError(ex);
                    System.Windows.MessageBox.Show("Error while Adding ScripProfilingScrips!", "Add Scrips", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    Predefined_Flag = true;
                    // BroadCastProcessor.OnBroadCastRecieved += objBroadCastProcessor_OnBroadCastRecieved;
                }
            }
        }

        private void DataGrid_DoubleClick()
        {
            if (SelectedItem != null)
            {
                if (SelectedItem.Scriptcode1 <= 0)
                    return;

                CommonFunctions.CallBestFiveUsingScripCode(Convert.ToInt32(SelectedItem.Scriptcode1));
            }
            else
            {
                if (SelectedScripCode <= 0)
                    return;

                CommonFunctions.CallBestFiveUsingScripCode(SelectedScripCode);
            }

            BestFiveVM.MemberQueryBF();
            //BestFiveVM.UpdateTitle(SelectedItem.Scriptcode1);

            /*
                        if (SelectedItem != null && SelectedItem.Scriptcode1 != 0)
                        {
                            string ScripID = string.Empty;

                            mainwindow = System.Windows.Application.Current.Windows.OfType<BestFiveMarketPicture>().FirstOrDefault();
                            if (mainwindow == null)
                                mainwindow = new BestFiveMarketPicture();
                            //ScripDetails objScripDetails = BroadcastMasterMemory.objScripDetailsConDict.Where(x => x.Key == SelectedItem.Scriptcode1).Select(x => x.Value).FirstOrDefault();
                            //if (objScripDetails == null)
                            //{
                            //    objScripDetails = new ScripDetails();
                            //    objScripDetails.ScriptCode = SelectedItem.Scriptcode1;
                            //}
                            ScripDetails objScripDetails = new ScripDetails();

            #if TWS
                            BroadcastReceiver.ScripDetails Br = BroadcastMaster.BroadcastMasterMemory.objScripDetailsConDict.Where(x => x.Key == SelectedItem.Scriptcode1).Select(x => x.Value).FirstOrDefault();
                            objScripDetails = MainWindowVM.UpdateScripDataFromMemory(Br);
                            objScripDetails.ScriptCode_BseToken_NseToken = SelectedItem.Scriptcode1;

            #elif BOW
                            string lstrKeysTEM = MarketWatchConstants.objScripDetails.Where(x => x.ScriptCode_BseToken_NseToken == SelectedScripCode).Select(x => x.SecurityKey_MW_EMT_Token).FirstOrDefault();
                            string pstrSegment= MarketWatchConstants.objScripDetails.Where(x => x.ScriptCode_BseToken_NseToken == SelectedScripCode).Select(x => x.Segment_Market).FirstOrDefault();
                            string pstrExchange = MarketWatchConstants.objScripDetails.Where(x => x.ScriptCode_BseToken_NseToken == SelectedScripCode).Select(x => x.Exchange_Source).FirstOrDefault();
                           string glngCMToken= GetTokenFromSelectedLegKey(lstrKeysTEM);
                            string exchange="";
                            if(pstrExchange=="1")
                            {
                                exchange = "BSE";
                            }
                            MarketWatchConstants.lstrTokenString = pstrSegment + "||" + glngCMToken + "||" + exchange;
                            string lstrReturn = GetDataByCallingServlet(pstrSegment, pstrExchange, glngCMToken, MarketWatchConstants.lstrTokenString);
                            //  RecordSplitter lobjRecordHelper = new RecordSplitter(lstrReturn);
                            ArrayList lobjArgs = new ArrayList(2);
                            if (lstrReturn.Substring(0, 1) == BowConstants.SUCCESS_FLAG)
                            {

                               // IAsyncResult IAsyncResult = default(IAsyncResult);
                                lock (lobjArgs)
                                {

                                    RecordSplitter lobjRecordHelper = new RecordSplitter(lstrReturn);
                                    lobjArgs.Add(lobjRecordHelper);
                                    lobjArgs.Add(true);
                                }
                            }
                            OpenMBPMessages(pstrExchange, MarketWatchConstants.lstrTokenString);
                            if (lobjArgs.Count != 0)
                            {
                                objScripDetails = UpdateData(lobjArgs, MarketWatchConstants.lstrTokenString);
                            }
                            else
                            {
                                objScripDetails.ScriptCode_BseToken_NseToken = SelectedItem.Scriptcode1;
                            }
            #endif


                            // bstfivewindow.Activate();
                            objBroadCastProcessor_OnBroadCastRecievedNew(objScripDetails);
                     //      ScripInsertUC_VM vm = new ScripInsertUC_VM();
                   // vm.SelectScripCodeFromTouchline(SelectedItem.Scriptcode1);
                            BestFiveVM.UpdateTitle(SelectedItem.Scriptcode1);
                            BestFiveVM.UpdateBestWindow(objScripDetails, true);
                            // HourlyStatisticsVM.UpdateTitleHourlyStatistics(SelectedItem.Scriptcode1, true);
                            //mainwindow.Activate();
                            mainwindow.Topmost = true;
                            mainwindow.Topmost = false;
                            mainwindow.Focus();
                            mainwindow.Show();
                            Loaded = true;
                        }
            */
        }

        private void Open_SwiftOrderEntry(object e)
        {
            SwiftOrderEntry sw = Application.Current.Windows.OfType<SwiftOrderEntry>().FirstOrDefault();
            if (sw == null)
            {
                sw = new SwiftOrderEntry();
                var dataContext = sw.DataContext as OrderEntryVM;
                if (e.ToString() == "B")
                {
                    dataContext.BuyVisible = "Visible";
                    dataContext.SellVisible = "Hidden";
                    dataContext.WindowColour = "#023199";
                    dataContext.BuySellInd = Enumerations.Order.BuySellFlag.B.ToString();
                    dataContext.HeaderTitle = "SwiftOrderEntry BUY";
                }
                else if (e.ToString() == "S")
                {
                    dataContext.SellVisible = "Visible";
                    dataContext.BuyVisible = "Hidden";
                    //WindowColour = "#ffadb0";
                    dataContext.WindowColour = "#960f00";
                    dataContext.BuySellInd = Enumerations.Order.BuySellFlag.S.ToString();
                    dataContext.HeaderTitle = "SwiftOrderEntry SELL";
                }
                dataContext.Selected_EXCH = SelectedItem.Exchange_Source;
                dataContext.ScripSelectedSegment = SelectedItem.Segment_Market;
                dataContext.PopulateDataForMW(SelectedItem);
                sw.Show();
                sw.Activate();
                sw.Focus();

            }
            // var dataContext = sw.DataContext as OrderEntryVM;
            // dataContext.ScripSelectedCode = SelectedScripCode;
            // dataContext.PopulateDataForMW(SelectedItem);
        }

        private void FontSelection(object e)
        {
            System.Windows.Controls.DataGrid dg = e as System.Windows.Controls.DataGrid;
            System.Windows.Forms.FontDialog fd = new System.Windows.Forms.FontDialog();
            fd.ShowEffects = false;
            if (fd.ShowDialog() != System.Windows.Forms.DialogResult.Cancel)
            {
                dg.FontFamily = new System.Windows.Media.FontFamily(fd.Font.Name);
                dg.FontSize = fd.Font.Size * 96.0 / 72.0;
                dg.FontWeight = fd.Font.Bold ? FontWeights.Bold : FontWeights.Regular;
                dg.FontStyle = fd.Font.Italic ? FontStyles.Italic : FontStyles.Normal;
                //dg.UpdateLayout();
                foreach (DataGridColumn c in dg.Columns)
                {
                    c.Width = 0;
                    dg.UpdateLayout();
                    //c.Width = new DataGridLength(1, DataGridLengthUnitType.Auto);
                    c.Width = new DataGridLength(1.0, DataGridLengthUnitType.SizeToCells);
                }


            }
            fd.Dispose();
        }

        private void ScripIdTxtChange_Click()
        {
            try
            {
                // Collection which will take your ObservableCollection
                // var _itemSourceList = new CollectionViewSource() { Source = SearchTemplist.OrderBy(x => x.ScriptId1) };
                var _itemSourceList = new CollectionViewSource() { Source = SearchTemplist };
                // ICollectionView the View/UI part 
                ICollectionView Itemlist = _itemSourceList.View;
                // executeFilterAction(new Action(() =>
                //{
                if (!string.IsNullOrEmpty(txtScripID) && !string.IsNullOrEmpty(txtScripCode))
                {
                    var yourCostumFilter = new Predicate<object>(item => ((MarketWatchModel)item).ScriptId1.ToLower().StartsWith(txtScripID.Trim().ToLower()) && ((MarketWatchModel)item).Scriptcode1.ToString().StartsWith(txtScripCode.Trim()));
                    Itemlist.Filter = yourCostumFilter;
                }
                else if (string.IsNullOrEmpty(txtScripCode))
                {
                    var yourCostumFilter = new Predicate<object>(item => ((MarketWatchModel)item).ScriptId1.ToLower().StartsWith(txtScripID.Trim().ToLower()));
                    Itemlist.Filter = yourCostumFilter;
                }
                else if (string.IsNullOrEmpty(txtScripID))
                {
                    var yourCostumFilter = new Predicate<object>(item => ((MarketWatchModel)item).Scriptcode1.ToString().StartsWith(txtScripCode.Trim()));
                    Itemlist.Filter = yourCostumFilter;
                }
                //now we add our Filter

                var l = Itemlist.Cast<MarketWatchModel>().ToList();
                ObjTouchlineDataCollection = new ObservableCollection<MarketWatchModel>(l);
                // }));
            }
            catch (Exception)
            {
                return;
            }

            finally
            {
                if (ObjTouchlineDataCollection.Count == SearchTemplist.Count && string.IsNullOrEmpty(txtScripCode) && string.IsNullOrEmpty(txtScripID))
                    TitleTouchLine = "TouchLine - " + CurrentTabSlected + " - " + ObjTouchlineDataCollection.Count;
                else
                    TitleTouchLine = "TouchLine - " + CurrentTabSlected + " - " + ObjTouchlineDataCollection.Count + " of " + SearchTemplist.Count;

                //NotifyStaticPropertyChanged("ObjTouchlineDataCollection");
            }
        }

        public void ExecuteMyCommand()
        {
            if (ObjTouchlineDataCollection.Count == 0)
            {
                MessageBox.Show("No Records to save");
            }
            StreamWriter sw = null;
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.DefaultExt = ".csv";
            dlg.Filter = "CSV Files (.csv)|*.csv";
            Nullable<bool> res = dlg.ShowDialog();
            if (res == true)
            {
                try
                {
                    List<DataGridColumn> objColumnHeader = new List<DataGridColumn>();

                    objColumnHeader = System.Windows.Application.Current.Windows.OfType<MarketWatch>().FirstOrDefault().dataGridView1.Columns.ToList();
                    sw = new StreamWriter(dlg.FileName, false, Encoding.UTF8);
                    try
                    {
                        sw.WriteLine("DATE : " + DateTime.Today.ToString("dd-MM-yyyy") + "," + "TIME: " + DateTime.Now.ToString("HH:mm:ss") + "," + "INDEX = " + SharedMemories.BroadcastMasterMemory.objIndexDetailsConDict[UtilityLoginDetails.GETInstance.SensexIndexCode].IndexValue / Math.Pow(10, 2));
                        //for (int j = 0; j < objColumnHeader.Count; j++)
                        //{
                        //    if (objColumnHeader[j].Visibility == Visibility.Visible)
                        //        sw.Write(objColumnHeader[j].Header.ToString() + ",");
                        //}
                        foreach (var item in objColumnHeader)
                        {
                            if (item.Visibility == Visibility.Visible)
                                sw.Write(item.Header.ToString() + " , ");
                        }
                        sw.WriteLine();

                        foreach (var item in ObjTouchlineDataCollection)
                        {
                            //for (int j = 0; j < objColumnHeader.Count; j++)
                            //{
                            //    if (objColumnHeader[j].Visibility == Visibility.Visible)
                            //        sw.Write(item.getValueFromHeader(objColumnHeader[j].Header.ToString()) + ",");
                            //}
                            foreach (var item1 in objColumnHeader)
                            {
                                if (item1.Visibility == Visibility.Visible)
                                    sw.Write(item.getValueFromHeader(item1.Header.ToString()) + " , ");
                            }
                            sw.WriteLine();
                        }

                    }
                    catch (Exception ex)
                    {
                        ExceptionUtility.LogError(ex);
                        System.Windows.MessageBox.Show("Error in Exporting data to Excel", "Export Data to Excel", MessageBoxButton.OK, MessageBoxImage.Error);
                        //MessageBox.Show(ex.Message);
                    }

                    System.Windows.MessageBox.Show("Successfully exported data to Excel Sheet at: " + dlg.FileName.ToString(), "Export Data to Excel", MessageBoxButton.OK, MessageBoxImage.None);
                }
                catch (Exception e)
                {
                    ExceptionUtility.LogError(e);
                    System.Windows.MessageBox.Show("Error in Exporting data to Excel", "Export Data to Excel", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    if (sw != null)
                    {
                        sw.Flush();
                        sw.Close();
                    }
                }
            }
            else { return; }

        }


        private void openProfile(int tabValue)
        {
            ProfileSettings SettingsWindow = System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault();

            if (SettingsWindow != null)
            {
                // SettingsWindow.Activate();
                SettingsWindow.Focus();
                SettingsWindow.MainTabControl.SelectedIndex = tabValue;
                SettingsWindow.Show();
            }
            else
            {
                //SettingsWindow = null;
                SettingsWindow = new ProfileSettings();
                SettingsWindow.ResizeMode = System.Windows.ResizeMode.NoResize;
                SettingsWindow.Owner = System.Windows.Application.Current.MainWindow;
                SettingsWindow.MainTabControl.SelectedIndex = tabValue;
                SettingsWindow.Show();
            }

        }

        public void exportToText()
        {
            if (ObjTouchlineDataCollection.Count == 0)
            {
                MessageBox.Show("No Records to save");
            }
            StreamWriter sw = null;
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.DefaultExt = ".txt";
            dlg.Filter = "Text Files (.txt)|*.txt";
            Nullable<bool> res = dlg.ShowDialog();
            if (res == true)
            {
                try
                {
                    List<DataGridColumn> objColumnHeader = new List<DataGridColumn>();
                    objColumnHeader = System.Windows.Application.Current.Windows.OfType<MarketWatch>().FirstOrDefault().dataGridView1.Columns.ToList();
                    sw = new StreamWriter(dlg.FileName, false, Encoding.UTF8);
                    try
                    {
                        sw.WriteLine("DATE : " + DateTime.Today.ToString("dd-MM-yyyy") + " | " + "TIME: " + DateTime.Now.ToString("HH:mm:ss") + " | " + "INDEX = " + SharedMemories.BroadcastMasterMemory.objIndexDetailsConDict[UtilityLoginDetails.GETInstance.SensexIndexCode].IndexValue / Math.Pow(10, 2));
                        foreach (var item in objColumnHeader)
                        {
                            if (item.Visibility == Visibility.Visible)
                                sw.Write(item.Header.ToString() + " | ");
                        }

                        //for (int j = 0; j < objColumnHeader.Count; j++)
                        //{
                        //    if (objColumnHeader[j].Visibility == Visibility.Visible)
                        //        sw.Write(objColumnHeader[j].Header.ToString() + " | ");
                        //}
                        sw.WriteLine();

                        foreach (var item in ObjTouchlineDataCollection)
                        {
                            //for (int j = 0; j < objColumnHeader.Count; j++)
                            //{
                            //    if (objColumnHeader[j].Visibility == Visibility.Visible)
                            //        sw.Write(item.getValueFromHeader(objColumnHeader[j].Header.ToString()) + " | ");
                            //}
                            foreach (var item1 in objColumnHeader)
                            {
                                if (item1.Visibility == Visibility.Visible)
                                    sw.Write(item.getValueFromHeader(item1.Header.ToString()) + " | ");
                            }
                            sw.WriteLine("");

                        }

                    }
                    catch (Exception ex)
                    {
                        ExceptionUtility.LogError(ex);
                        System.Windows.MessageBox.Show("Error in Exporting data to Text File", "Export Data to Text", MessageBoxButton.OK, MessageBoxImage.Error);
                        //MessageBox.Show(ex.Message);
                    }

                    System.Windows.MessageBox.Show("Successfully exported data to Text File at: " + dlg.FileName.ToString(), "Export Data to Text", MessageBoxButton.OK, MessageBoxImage.None);
                }
                catch (Exception e)
                {
                    ExceptionUtility.LogError(e);
                    System.Windows.MessageBox.Show("Error in Exporting data to Text File", "Export Data to Text", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    if (sw != null)
                    {
                        sw.Flush();
                        sw.Close();
                    }
                }
            }
            else { return; }

        }



        private void GrdlineVisible_Click()
        {
            if (GridLinesVisibility == "Disable GridLines")
            {
                GrdlineVisible = "None";
                GridLinesVisibility = "Enable GridLines";
            }
            else
            {
                GrdlineVisible = "All";
                GridLinesVisibility = "Disable GridLines";
            }
        }


        private void Delete_Scrips_row()
        {
            if (SelectedItem != null)
            {
                List<MarketWatchModel> SelectedRows = new List<MarketWatchModel>();
                MessageBoxResult result;

                foreach (MarketWatchModel item in System.Windows.Application.Current.Windows.OfType<MarketWatch>().FirstOrDefault().dataGridView1.SelectedItems)
                {
                    SelectedRows.Add(item);
                }




                //foreach (MarketWatchModel item in System.Windows.Application.Current.Windows.OfType<MarketWatch>().FirstOrDefault().dataGridView1.SelectedItems)
                //{
                //    SelectedRows.Add(item);
                //}
                if (MarketsCombo.Contains(CurrentTabSlected))
                    System.Windows.MessageBox.Show("Scrip Deletion Funtionality Not Available", "Information", MessageBoxButton.OK, MessageBoxImage.Error);

                else
                {
                    if (SelectedRows.All(x => x.Scriptcode1 == 0))
                    {
                        result = System.Windows.MessageBox.Show("Do you want to delete the Blank Row?", "Delete...",
                                   MessageBoxButton.YesNo, MessageBoxImage.Question);
                        //if (response == MessageBoxResult.Yes)
                        //{

                        //    foreach (var item in SelectedRows)
                        //    {
                        //        ObjTouchlineDataCollection.Remove(item);
                        //    }
                        //    //ObjTouchlineDataCollection.Remove(ObjTouchlineDataCollection.FirstOrDefault(x => x.Scriptcode1 == SelectedItem.Scriptcode1));
                        //}
                    }
                    else if (SelectedRows.Any(x => x.Scriptcode1 == 0))
                    {
                        result = System.Windows.MessageBox.Show("Do you want to delete the Blank Row and Scrips?", "Delete...",
                                    MessageBoxButton.YesNo, MessageBoxImage.Question);
                    }
                    else
                    {
                        result = System.Windows.MessageBox.Show("Do you really want to Delete the scrips?", "Delete...",
                                       MessageBoxButton.YesNo, MessageBoxImage.Question);

                    }

                    if (result == MessageBoxResult.Yes)
                    {
                        foreach (var item in SelectedRows)
                        {
                            ObjTouchlineDataCollection.Remove(item);
                        }
                        //System.Windows.Application.Current.Windows.OfType<ClassicTouchLine>().FirstOrDefault().dataGridView1.Focus();
                    }
                    TitleTouchLine = "TouchLine - " + CurrentTabSlected + "-" + ObjTouchlineDataCollection.Count;
                }

                SearchTemplist = ObjTouchlineDataCollection.Cast<MarketWatchModel>().ToList();
                //else if (response == MessageBoxResult.No)
                //{
                //    //TODO:
                //}
            }
        }

        private void EnterUsingUserControl(object e)
        {
            DataGrid_DoubleClick();
        }

        private void EscapeUsingUserControl(object e)
        {
            MarketWatch mainwindow = System.Windows.Application.Current.Windows.OfType<MarketWatch>().FirstOrDefault();
            //ClassicTouchLine mainwindow = e as ClassicTouchLine;
            mainwindow.Hide();
        }

        private void Insert_Blank_row()
        {
            if (MarketsCombo.Contains(CurrentTabSlected))
                System.Windows.MessageBox.Show("Blank Row Addition  Funtionality Not Available", "Information", MessageBoxButton.OK, MessageBoxImage.Error);

            else
            {
                MarketWatchModel objtouch = new MarketWatchModel();
                objtouch.Index = ObjTouchlineDataCollection.Count;
                DataGrid_ViewUpdate(objtouch, "ScripInsert");
            }
            /*
            else if (SelectedItem != null)
            {
                LastData = ObjTouchlineDataCollection.IndexOf(ObjTouchlineDataCollection.FirstOrDefault(i => i.Scriptcode1 == SelectedItem.Scriptcode1));
                if (LastData != null)
                    ObjTouchlineDataCollection.Insert(Convert.ToInt32(LastData) + 1, new MarketWatchModel());
            }
            */
        }

        private void AddScripsUsingUserControl()
        {
            if (MarketsCombo.Contains(CurrentTabSlected))
                System.Windows.MessageBox.Show("Scrip Addition Funtionality Not Available", "Information", MessageBoxButton.OK, MessageBoxImage.Error);

            else
            {

                ScripInsertUC_VM.TouchLineInsert = true;
                UserControlWindow.Owner = System.Windows.Application.Current.MainWindow;
                UserControlWindow.ShowDialog();

            }
            //else
            //UserControlWindow.Activate();
            //UserControlLoaded = true;
            //UserControlScrips = new ScripInsertUC();
            //UserControlScrips.BringIntoView();

        }


        public void OnSubmenuSelection(object sender)
        {
            RoutedEventArgs obj = sender as RoutedEventArgs;
            SelectedProfilename = ((HeaderedItemsControl)obj.OriginalSource).Header.ToString();
            ColumnProfilingVM.FetchColumnsFromMemory(SelectedProfilename, "Touchline");
            ObservableCollection<ColumnProfilingModel> objCP = ColumnProfilingVM.FetchedColumnsFromMemory;
            List<string> objColumnName = new List<string>();
            foreach (string ProfileName in objCP.Select(x => x.ColProfile))
            {
                if (SelectedProfilename == ProfileName)
                {
                    objColumnName.Add(objCP.Select(x => x.ColumnName).FirstOrDefault());
                }
            }
            OnSubMenuSelection = true;
            UpdatingColumnProfile(TabName, OnSubMenuSelection);

        }

        private void SrchTime_Elapsed(object sender, ElapsedEventArgs e)
        {
            //SrchTime.Enabled = false;
            //searchString = string.Empty;
            //searchStringCnt = 0;


            App.Current.Dispatcher.BeginInvoke((Action)delegate ()
            {
                if (objTouchlineDataCollection != null && objTouchlineDataCollection.Count > 0)
                {
                    MarketWatch mainwindow = System.Windows.Application.Current.Windows.OfType<MarketWatch>().FirstOrDefault();
                    int cnt = mainwindow.dataGridView1.Items.Count;

                    int si = mainwindow.dataGridView1.SelectedIndex + 1;
                    if (si < 0 || si >= cnt)
                        si = 0;

                    for (int t = 0, s = si; /* s<cnt */ t < cnt; s++, t++)
                    {
                        if (s == cnt)
                            s = 0;

                        object o = mainwindow.dataGridView1.Items[s];
                        string st = ((CommonFrontEnd.Model.MarketWatchModel)o).ScriptId1;

                        int scnt = searchStringCnt > st.Length ? st.Length : searchStringCnt;
                        string fnd = st.Substring(0, scnt);

                        if (fnd == searchString)
                        {
                            mainwindow.dataGridView1.CurrentItem = o;
                            mainwindow.dataGridView1.ScrollIntoView(o);
                            mainwindow.dataGridView1.SelectedItem = o;
                            break;
                        }
                    }

                    searchString = string.Empty;
                    searchStringCnt = 0;
                }
            });

            SrchTime.Enabled = false;
        }


        private void DataGrid_KeyUp(object sender)
        {
            KeyEventArgs e = sender as KeyEventArgs;

            string txt = new KeyConverter().ConvertToString(e.Key);
            if (txt.ToString().ToList().Any(x => !System.Char.IsLetterOrDigit(x)))
                return;

            if (e.Key == Key.Escape || e.Key == Key.Enter)
            {
                searchString = "";
                searchStringCnt = 0;
            }
            else //if ((e.Key >= Key.A && e.Key <= Key.Z) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || (e.Key >= Key.Z && e.Key <= Key.Z))
            {
                searchString += e.Key;
                searchStringCnt++;

                if (SrchTime == null)
                {
                    SrchTime = new System.Timers.Timer();
                    SrchTime.Interval = 700;
                    SrchTime.Enabled = true;
                    SrchTime.Elapsed += SrchTime_Elapsed;
                }
                else if (SrchTime.Enabled == false)
                {
                    SrchTime.Enabled = true;
                    SrchTime.Interval = 700;
                }

                /*
                if (Option == 2)
                {
                    MarketWatchModel item = objTouchlineDataCollection.OrderBy(x => x.ScriptId1).FirstOrDefault(x => x.ScriptId1.StartsWith(searchString));
                    if (item != null)
                    {
                        MarketWatch mainwindow = System.Windows.Application.Current.Windows.OfType<MarketWatch>().FirstOrDefault();

                        mainwindow.dataGridView1.CurrentItem = item;
                        mainwindow.dataGridView1.ScrollIntoView(item);
                        mainwindow.dataGridView1.SelectedItem = item;
                    }
                }
                else if (Option == 3)
                {
                    //if (searchStringCnt > 2)
                    {
                        MarketWatch mainwindow = System.Windows.Application.Current.Windows.OfType<MarketWatch>().FirstOrDefault();
                        int cnt = mainwindow.dataGridView1.Items.Count;

                        int si = mainwindow.dataGridView1.SelectedIndex + 1;
                        if (si < 0 || si >= cnt)
                            si = 0;

                        for (int t = 0, s = si;  t < cnt; s++, t++)
                        {
                            if (s == cnt)
                                s = 0;

                            object o = mainwindow.dataGridView1.Items[s];
                            string st = ((CommonFrontEnd.Model.MarketWatchModel)o).ScriptId1;

                            int scnt = searchStringCnt > st.Length ? st.Length : searchStringCnt;
                            string fnd = st.Substring(0, scnt);


                            if (fnd == searchString)
                            {
                                mainwindow.dataGridView1.CurrentItem = o;
                                mainwindow.dataGridView1.ScrollIntoView(o);
                                mainwindow.dataGridView1.SelectedItem = o;
                                break;
                            }
                        }
                    }
                }
                else if (Option == 4)
                {
                    MarketWatch mainwindow = System.Windows.Application.Current.Windows.OfType<MarketWatch>().FirstOrDefault();
                    int cnt = mainwindow.dataGridView1.Items.Count;
                   
                    for (int s = 0; s < cnt; s++)
                    {
                        object o = mainwindow.dataGridView1.Items[s];
                        string st = ((CommonFrontEnd.Model.MarketWatchModel)o).ScriptId1;
                        int scnt = searchStringCnt > st.Length ? st.Length : searchStringCnt;
                        string fnd = st.Substring(0, scnt);

                        if (fnd == searchString)
                        {
                            mainwindow.dataGridView1.CurrentItem = o;
                            mainwindow.dataGridView1.ScrollIntoView(o);
                            mainwindow.dataGridView1.SelectedItem = o;
                            break;
                        }
                    }
                }
                */
            }
        }

        public static void DataGrid_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {

            double d = e.VerticalOffset;

            if (e.VerticalChange != 0 || e.ViewportHeightChange != 0 || (e.VerticalChange == 0 && e.ViewportHeightChange == 0 && e.HorizontalChange == 0 && e.ViewportWidthChange == 0))
            {
                SubscribeScripMemory.objMasterSubscribeScrip.Clear();
                TouchLineList.Clear();
                int start = Convert.ToInt32(e.VerticalOffset), end = Convert.ToInt32(e.ViewportHeight);

                TLStart = start;
                TLEnd = end;

                DataGrid_SuscribeList();

                e.Handled = true;
            }
        }

        public static void DataGrid_PreviewPageUpDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.PageDown:
                case Key.PageUp:
                    {
                        e.Handled = false;

                        MarketWatch MktWt = System.Windows.Application.Current.Windows.OfType<MarketWatch>().FirstOrDefault();
                        List<MarketWatchModel> OldRows = new List<MarketWatchModel>();
                        OldRows = MktWt.dataGridView1.ItemContainerGenerator.Items.Cast<MarketWatchModel>().ToList();

                        int CurrIndx = ((System.Windows.Controls.Primitives.Selector)sender).SelectedIndex;
                        int TotalCount = MktWt.dataGridView1.Items.Count;
                        if (CurrIndx < 0 || CurrIndx > TotalCount)
                            break;

                        double GridHeight = MktWt.dataGridView1.ActualHeight;
                        DataGridRow rowItem = (DataGridRow)MktWt.dataGridView1.ItemContainerGenerator.ContainerFromIndex(CurrIndx);
                        if (rowItem == null)
                        {
                            for (int i = 0; i < TotalCount; i++)
                            {
                                rowItem = (DataGridRow)MktWt.dataGridView1.ItemContainerGenerator.ContainerFromIndex(i);
                                if (rowItem != null)
                                    break;
                            }
                        }
                        double RowHeight = rowItem != null ? rowItem.ActualHeight : 21;
                        int PageCount = Convert.ToInt32(Math.Round((GridHeight - RowHeight) / RowHeight));

                        if (e.Key == Key.PageUp)
                        {
                            for (int i = CurrIndx; i >= 0 && i >= CurrIndx - PageCount; i--)
                                CommonFunctions.TouchLineScripQueryList.Add(OldRows[i].Scriptcode1);
                        }
                        else if (e.Key == Key.PageDown)
                        {
                            for (int i = CurrIndx; i < CurrIndx + PageCount && i < TotalCount; i++)
                                CommonFunctions.TouchLineScripQueryList.Add(OldRows[i].Scriptcode1);
                        }

                        while (CommonFunctions.TouchLineScripQueryList.Count > 0)
                        {
                            bool retVal = CommonFunctions.MarketPicQuery(CommonFunctions.TouchLineScripQueryList[0]);
                            CommonFunctions.TouchLineScripQueryList.RemoveAt(0);
                            if (retVal == false)
                                continue;
                            else
                                break;
                        }
                    }
                    break;
            }
        }

        private static void DataGrid_SuscribeList()
        {
            MarketWatch MktWt = System.Windows.Application.Current.Windows.OfType<MarketWatch>().FirstOrDefault();
            SubscribeScripMemory.objMasterSubscribeScrip.Clear();
            TouchLineList.Clear();
            for (int i = TLStart; i <= TLStart + TLEnd; i++)
            {
                if (i == MktWt.dataGridView1.Items.Count)
                    continue;

                TlineListData t = new TlineListData();
                t.ScripCode = ((CommonFrontEnd.Model.MarketWatchModel)(MktWt.dataGridView1.ItemContainerGenerator.Items[i])).Scriptcode1;
                t.index = i;
                TouchLineList.AddLast(t);

                if (!SubscribeScripMemory.objMasterSubscribeScrip.ContainsKey(t.ScripCode))
                {
                    SubscribeList.SubscribeScrip s = new SubscribeScrip();
                    s.ScripCode_l = t.ScripCode;
                    s.UpdateFlag_s = 1;
                    SubscribeScripMemory.objMasterSubscribeScrip.TryAdd(t.ScripCode, s);
                }

                {
                    //ScripDetails objscrip = new ScripDetails();
                    ScripDetails objscrip;
                    //BroadcastReceiver.ScripDetails Br = BroadcastMaster.BroadcastMasterMemory.objScripDetailsConDict.Where(x => x.Key == t.ScripCode).Select(x => x.Value).FirstOrDefault();
                    //objscrip = MainWindowVM.UpdateScripDataFromMemory(Br);

                    objscrip = MainWindowVM.UpdateScripDataFromMemory(t.ScripCode);
                    if (objscrip == null)
                    {
                        //MarketWatchModel m = ObjTouchlineDataCollection[i];
                        Clear_Fields(ObjTouchlineDataCollection[i]);
                        //Initialize_Fields(null, ref m, 2, t.ScripCode);
                        continue;
                    }

                    objscrip.ScripID = CommonFunctions.GetScripId(t.ScripCode, "BSE");
                    if (objscrip.ScripID == string.Empty)
                        continue;

                    if (i > -1 && i < ObjTouchlineDataCollection.Count)
                        UpdateGrid(i, objscrip);
                }
            }
            if (!SubscribeScripMemory.objMasterSubscribeScrip.ContainsKey((Convert.ToInt32(OrderEntryUC_VM.SelectedScripCode))))
            {
                SubscribeList.SubscribeScrip s = new SubscribeScrip();
                s.ScripCode_l = Convert.ToInt32(OrderEntryUC_VM.SelectedScripCode);
                s.UpdateFlag_s = 1;
                SubscribeScripMemory.objMasterSubscribeScrip.TryAdd(s.ScripCode_l, s);
            }

            if (!SubscribeScripMemory.objMasterSubscribeScrip.ContainsKey((Convert.ToInt32(NormalOrderEntryVM.OrderEntryScripCode))))
            {
                SubscribeList.SubscribeScrip s = new SubscribeScrip();
                s.ScripCode_l = Convert.ToInt32(NormalOrderEntryVM.OrderEntryScripCode);
                s.UpdateFlag_s = 1;
                SubscribeScripMemory.objMasterSubscribeScrip.TryAdd(s.ScripCode_l, s);
            }
            TouchLineList.OrderBy(x => x.ScripCode);
        }

        private static void DataGrid_ViewUpdate(object sender, string EventName)
        {
            MarketWatch MktWt = System.Windows.Application.Current.Windows.OfType<MarketWatch>().FirstOrDefault();
            List<MarketWatchModel> OldRows = new List<MarketWatchModel>();
            OldRows = MktWt.dataGridView1.ItemContainerGenerator.Items.Cast<MarketWatchModel>().ToList();

            if (EventName == "DragDrop")
            {
                DragEventArgs e = sender as DragEventArgs;

                string DragScrip = ((CommonFrontEnd.Model.MarketWatchModel)(((System.Windows.Controls.Primitives.MultiSelector)e.Source).SelectedItem)).ScriptId1;
                int DragScripIndx = (((System.Windows.Controls.Primitives.MultiSelector)e.Source)).SelectedIndex;

                if (DragScripIndx < 0 || DragScripIndx > ObjTouchlineDataCollection.Count)
                    return;

                var DropElement = (UIElement)e.OriginalSource;
                var DropMktWtch = ((System.Windows.FrameworkElement)DropElement).DataContext as MarketWatchModel;
                MktWt.dataGridView1.SelectedItem = DropMktWtch;
                int DropScripIndx = MktWt.dataGridView1.SelectedIndex;

                if (DropScripIndx < 0 || DropScripIndx > ObjTouchlineDataCollection.Count)
                    return;

                var tmp = OldRows[DragScripIndx];
                OldRows.RemoveAt(DragScripIndx);
                OldRows.Insert(DropScripIndx, tmp);
            }
            else if (EventName == "ScripInsert")
            {
                MarketWatchModel Newitem = sender as MarketWatchModel;

                int InsertScripIndx = MktWt.dataGridView1.SelectedIndex;

                if (InsertScripIndx < 0)
                    InsertScripIndx = ObjTouchlineDataCollection.Count;
                else
                    InsertScripIndx++;

                OldRows.Insert(InsertScripIndx, Newitem);
            }

            DataGrid objDataGrid = null;
            objDataGrid = System.Windows.Application.Current.Windows.OfType<MarketWatch>().FirstOrDefault().dataGridView1;
            ResultSort._SortMode = 0;
            objDataGrid.CanUserSortColumns = false;
            if (objDataGrid.CurrentColumn != null)
                objDataGrid.CurrentColumn.SortDirection = null;

            ObjTouchlineDataCollection.Clear();
            int NewIndex = 0;
            foreach (var Rowitem in OldRows)
            {
                MarketWatchModel Newitem = new MarketWatchModel();
                Newitem = Rowitem;
                Newitem.Index = NewIndex++;
                ObjTouchlineDataCollection.Insert(Newitem.Index, Newitem);
            }

            objDataGrid.CanUserSortColumns = true;

            DataGrid_SuscribeList();
        }

        private void DataGrid_Drop(object sender)
        {
            DragEventArgs e = sender as DragEventArgs;
            //int j = ((System.Windows.Controls.ItemsControl)e.Source).Items.CurrentPosition;
            {
                if (SelectedItem != null)
                {
                    int cnt = ((System.Windows.Controls.Primitives.MultiSelector)e.Source).SelectedItems.Count;
                    if (cnt == 1)
                    {
                        string elTxt = e.OriginalSource.ToString();
                        if (elTxt == "System.Windows.Controls.TextBlock" || elTxt == "System.Windows.Controls.Border")
                        {
                            DataGrid_ViewUpdate(e, "DragDrop");
                        }
                    }
                }
            }
        }

        public static void DataGrid_CustomSorting(DataGridColumn column)
        {
            IComparer comparer = null;
            ListSortDirection direction = (column.SortDirection != ListSortDirection.Ascending) ? ListSortDirection.Ascending : ListSortDirection.Descending;

            //set the sort order on the column
            column.SortDirection = direction;
            string Column = column.SortMemberPath;
            int SortMode = 0;

            if (direction == ListSortDirection.Descending)
                SortMode = 2;
            else if (direction == ListSortDirection.Ascending)
                SortMode = 1;
            else
                SortMode = 0;

            MarketWatch MktWt = System.Windows.Application.Current.Windows.OfType<MarketWatch>().FirstOrDefault();
            List<MarketWatchModel> OldRows = new List<MarketWatchModel>();
            OldRows = MktWt.dataGridView1.ItemContainerGenerator.Items.Cast<MarketWatchModel>().ToList();

            List<MarketWatchModel> NewRows = new List<MarketWatchModel>();

            List<MarketWatchModel> SortedRows = new List<MarketWatchModel>();

            int i = 0, j = 0;
            for (i = j; i < OldRows.Count; i++)
            {
                ObjTouchlineDataCollection.Clear();
                int NewIndex = 0;
                bool blankFlag = false;
                for (j = i; j < OldRows.Count; j++)
                {
                    if (OldRows[j].Scriptcode1 == 0)
                    {
                        blankFlag = true;
                        break;
                    }

                    MarketWatchModel Newitem = new MarketWatchModel();
                    Newitem = OldRows[j];
                    Newitem.Index = NewIndex++;
                    ObjTouchlineDataCollection.Insert(Newitem.Index, Newitem);
                }

                MktWt.dataGridView1.CanUserSortColumns = true;
                ListCollectionView lcv = (ListCollectionView)CollectionViewSource.GetDefaultView(MktWt.dataGridView1.ItemsSource);
                comparer = new ResultSort(SortMode, Column);
                lcv.CustomSort = comparer;

                ResultSort._SortMode = 0;
                MktWt.dataGridView1.CanUserSortColumns = false;

                if (blankFlag == true)
                {
                    MarketWatchModel Newitem = new MarketWatchModel();
                    Newitem.Index = NewIndex++;
                    ObjTouchlineDataCollection.Insert(Newitem.Index, Newitem);
                }

                NewRows = MktWt.dataGridView1.ItemContainerGenerator.Items.Cast<MarketWatchModel>().ToList();
                SortedRows = SortedRows.Concat(NewRows).ToList();
                i = j;
            }

            ResultSort._SortMode = 0;
            ObjTouchlineDataCollection.Clear();
            int NewIndex2 = 0;
            foreach (var Rowitem in SortedRows)
            {
                MarketWatchModel Newitem = new MarketWatchModel();
                Newitem = Rowitem;
                Newitem.Index = NewIndex2++;
                ObjTouchlineDataCollection.Insert(Newitem.Index, Newitem);
            }
            MktWt.dataGridView1.CanUserSortColumns = true;

            if (TLStart > 0)
                MktWt.dataGridView1.ScrollIntoView(ObjTouchlineDataCollection[0]);
            else
                DataGrid_SuscribeList();
        }

        public static void DataGrid_Sorting(object sender, DataGridSortingEventArgs e)
        {
            DataGridColumn column = e.Column;
            e.Handled = true;

            DataGrid_CustomSorting(column);
        }
    }

    public class ResultSort : IComparer
    {
        public static int _SortMode;
        private string _Column;
        public ResultSort(int SortMode, string Column)
        {
            _SortMode = SortMode;
            _Column = Column;
        }
        public int Compare(object x, object y)
        {
            if (_SortMode == 0)
                return -1;

            switch (_Column)
            {
                case "ScriptId1":
                    return (_SortMode == 1 ? 1 : -1) * (((CommonFrontEnd.Model.MarketWatchModel)x).ScriptId1).ToString().CompareTo((((CommonFrontEnd.Model.MarketWatchModel)y).ScriptId1).ToString());
                    break;

                case "Scriptcode1":
                    return (_SortMode == 1 ? 1 : -1) * (((CommonFrontEnd.Model.MarketWatchModel)x).Scriptcode1).CompareTo((((CommonFrontEnd.Model.MarketWatchModel)y).Scriptcode1));
                    break;

                    //case "BuyQualtity1":
                    //    return (_SortMode == 1 ? 1 : -1) * (((CommonFrontEnd.Model.MarketWatchModel)x).BuyQualtity1).CompareTo((((CommonFrontEnd.Model.MarketWatchModel)y).BuyQualtity1));
                    //    break;

                    //case "SellQuantity1":
                    //    return (_SortMode == 1 ? 1 : -1) * (((CommonFrontEnd.Model.MarketWatchModel)x).SellQuantity1).CompareTo((((CommonFrontEnd.Model.MarketWatchModel)y).SellQuantity1));
                    //    break;

                    //case "BuyRate1":
                    //    return (_SortMode == 1 ? 1 : -1) * (((CommonFrontEnd.Model.MarketWatchModel)x).BuyRate1).CompareTo((((CommonFrontEnd.Model.MarketWatchModel)y).BuyRate1));
                    //    break;

                    //case "SellRate1":
                    //    return (_SortMode == 1 ? 1 : -1) * (((CommonFrontEnd.Model.MarketWatchModel)x).SellRate1).ToString().CompareTo((((CommonFrontEnd.Model.MarketWatchModel)y).SellRate1).ToString());
                    //    break;
                    /*
                    case "OpenRateL":
                        return (_SortMode == 1 ? 1 : -1) * Convert.ToDecimal(((CommonFrontEnd.Model.MarketWatchModel)x).OpenRateL).CompareTo(Convert.ToDecimal(((CommonFrontEnd.Model.MarketWatchModel)y).OpenRateL));
                        break;
                        
                    case "CloseRateL":
                        return (_SortMode == 1 ? 1 : -1) * Convert.ToDecimal(((CommonFrontEnd.Model.MarketWatchModel)x).CloseRateL).CompareTo(Convert.ToDecimal(((CommonFrontEnd.Model.MarketWatchModel)y).CloseRateL));
                        break;

                    case "LTP1":
                        return (_SortMode == 1 ? 1 : -1) * Convert.ToDecimal(((CommonFrontEnd.Model.MarketWatchModel)x).LTP1).CompareTo(Convert.ToDecimal(((CommonFrontEnd.Model.MarketWatchModel)y).LTP1));
                        break;

                    case "HighRateL":
                        return (_SortMode == 1 ? 1 : -1) * Convert.ToDecimal(((CommonFrontEnd.Model.MarketWatchModel)x).HighRateL).CompareTo(Convert.ToDecimal(((CommonFrontEnd.Model.MarketWatchModel)y).HighRateL));
                        break;

                    case "LowRateL":
                        return (_SortMode == 1 ? 1 : -1) * Convert.ToDecimal(((CommonFrontEnd.Model.MarketWatchModel)x).LowRateL).CompareTo(Convert.ToDecimal(((CommonFrontEnd.Model.MarketWatchModel)y).LowRateL));
                        break;

                    case "FiftyTwoHigh":
                        return (_SortMode == 1 ? 1 : -1) * Convert.ToDecimal(((CommonFrontEnd.Model.MarketWatchModel)x).FiftyTwoHigh).CompareTo(Convert.ToDecimal(((CommonFrontEnd.Model.MarketWatchModel)y).FiftyTwoHigh));
                        break;

                    case "FiftyTwoLow":
                        return (_SortMode == 1 ? 1 : -1) * Convert.ToDecimal(((CommonFrontEnd.Model.MarketWatchModel)x).FiftyTwoLow).CompareTo(Convert.ToDecimal(((CommonFrontEnd.Model.MarketWatchModel)y).FiftyTwoLow));
                        break;
                        /*
                    case "CorpActionValue":
                        return (_SortMode == 1 ? 1 : -1) * (((CommonFrontEnd.Model.MarketWatchModel)x).CorpActionValue).ToString().CompareTo((((CommonFrontEnd.Model.MarketWatchModel)y).CorpActionValue).ToString());
                        break;

                    case "CtVolume":
                        return (_SortMode == 1 ? 1 : -1) * (((CommonFrontEnd.Model.MarketWatchModel)x).CtVolume).ToString().CompareTo((((CommonFrontEnd.Model.MarketWatchModel)y).CtVolume).ToString());
                        break;
/*
                    //case "Prem/Disc":
                    //    return (_SortMode == 1 ? 1 : -1) * (((CommonFrontEnd.Model.MarketWatchModel)x).Prem / Disc).ToString().CompareTo((((CommonFrontEnd.Model.MarketWatchModel)y).Prem / Disc).ToString());
                    //    break;

                    case "ChangePercentage":
                        return (_SortMode == 1 ? 1 : -1) * (((CommonFrontEnd.Model.MarketWatchModel)x).ChangePercentage).ToString().CompareTo((((CommonFrontEnd.Model.MarketWatchModel)y).ChangePercentage).ToString());
                        break;

                    case "LTQ1":
                        return (_SortMode == 1 ? 1 : -1) * (((CommonFrontEnd.Model.MarketWatchModel)x).LTQ1).ToString().CompareTo((((CommonFrontEnd.Model.MarketWatchModel)y).LTQ1).ToString());
                        break;

                    case "OI":
                        return (_SortMode == 1 ? 1 : -1) * (((CommonFrontEnd.Model.MarketWatchModel)x).OI).ToString().CompareTo((((CommonFrontEnd.Model.MarketWatchModel)y).OI).ToString());
                        break;

                    //case "OI":
                    //    return (_SortMode == 1 ? 1 : -1) * (((CommonFrontEnd.Model.MarketWatchModel)x).OI).ToString().CompareTo((((CommonFrontEnd.Model.MarketWatchModel)y).OI).ToString());
                    //    break;

                    //case "OI":
                    //    return (_SortMode == 1 ? 1 : -1) * (((CommonFrontEnd.Model.MarketWatchModel)x).OI).ToString().CompareTo((((CommonFrontEnd.Model.MarketWatchModel)y).OI).ToString());
                    //    break;

                    //case "IndPrice":
                    //    return (_SortMode == 1 ? 1 : -1) * (((CommonFrontEnd.Model.MarketWatchModel)x).IndPrice).ToString().CompareTo((((CommonFrontEnd.Model.MarketWatchModel)y).IndPrice).ToString());
                    //    break;

                    //case "IndQty":
                    //    return (_SortMode == 1 ? 1 : -1) * (((CommonFrontEnd.Model.MarketWatchModel)x).IndQty).ToString().CompareTo((((CommonFrontEnd.Model.MarketWatchModel)y).IndQty).ToString());
                    //    break;

                    case "LowerCtLmt":
                        return (_SortMode == 1 ? 1 : -1) * (((CommonFrontEnd.Model.MarketWatchModel)x).LowerCtLmt).ToString().CompareTo((((CommonFrontEnd.Model.MarketWatchModel)y).LowerCtLmt).ToString());
                        break;

                    case "UpperCtLmt":
                        return (_SortMode == 1 ? 1 : -1) * (((CommonFrontEnd.Model.MarketWatchModel)x).UpperCtLmt).ToString().CompareTo((((CommonFrontEnd.Model.MarketWatchModel)y).UpperCtLmt).ToString());
                        break;

                    //case "Trades":
                    //    return (_SortMode == 1 ? 1 : -1) * (((CommonFrontEnd.Model.MarketWatchModel)x).Trades).ToString().CompareTo((((CommonFrontEnd.Model.MarketWatchModel)y).Trades).ToString());
                    //    break;

                    case "WtAvgRateL":
                        return (_SortMode == 1 ? 1 : -1) * (((CommonFrontEnd.Model.MarketWatchModel)x).WtAvgRateL).ToString().CompareTo((((CommonFrontEnd.Model.MarketWatchModel)y).WtAvgRateL).ToString());
                        break;

                    //case "BDRate":
                    //    return (_SortMode == 1 ? 1 : -1) * (((CommonFrontEnd.Model.MarketWatchModel)x).BDRate).ToString().CompareTo((((CommonFrontEnd.Model.MarketWatchModel)y).BDRate).ToString());
                    //    break;

                    //case "SDRate":
                    //    return (_SortMode == 1 ? 1 : -1) * (((CommonFrontEnd.Model.MarketWatchModel)x).SDRate).ToString().CompareTo((((CommonFrontEnd.Model.MarketWatchModel)y).SDRate).ToString());
                    //    break;

                    //case "AI":
                    //    return (_SortMode == 1 ? 1 : -1) * (((CommonFrontEnd.Model.MarketWatchModel)x).AI).ToString().CompareTo((((CommonFrontEnd.Model.MarketWatchModel)y).AI).ToString());
                    //    break;

                    case "BRP":
                        return (_SortMode == 1 ? 1 : -1) * (((CommonFrontEnd.Model.MarketWatchModel)x).BRP).ToString().CompareTo((((CommonFrontEnd.Model.MarketWatchModel)y).BRP).ToString());
                        break;

                    case "TotBuyQtyL":
                        return (_SortMode == 1 ? 1 : -1) * (((CommonFrontEnd.Model.MarketWatchModel)x).TotBuyQtyL).ToString().CompareTo((((CommonFrontEnd.Model.MarketWatchModel)y).TotBuyQtyL).ToString());
                        break;

                    case "TotSellQtyL":
                        return (_SortMode == 1 ? 1 : -1) * (((CommonFrontEnd.Model.MarketWatchModel)x).TotSellQtyL).ToString().CompareTo((((CommonFrontEnd.Model.MarketWatchModel)y).TotSellQtyL).ToString());
                        break;

                    case "CtValue":
                        return (_SortMode == 1 ? 1 : -1) * (((CommonFrontEnd.Model.MarketWatchModel)x).CtValue).ToString().CompareTo((((CommonFrontEnd.Model.MarketWatchModel)y).CtValue).ToString());
                        break;

                    case "StrikePriceL":
                        return (_SortMode == 1 ? 1 : -1) * (((CommonFrontEnd.Model.MarketWatchModel)x).StrikePriceL).ToString().CompareTo((((CommonFrontEnd.Model.MarketWatchModel)y).StrikePriceL).ToString());
                        break;

                    //case "ExpiryDateL":
                    //    return (_SortMode == 1 ? 1 : -1) * (((CommonFrontEnd.Model.MarketWatchModel)x).ExpiryDateL).ToString().CompareTo((((CommonFrontEnd.Model.MarketWatchModel)y).ExpiryDateL).ToString());
                    //    break;

                    //case "TotTradedValL":
                    //    return (_SortMode == 1 ? 1 : -1) * (((CommonFrontEnd.Model.MarketWatchModel)x).TotTradedValL).ToString().CompareTo((((CommonFrontEnd.Model.MarketWatchModel)y).TotTradedValL).ToString());
                    //    break;

                    //case "ChangeL":
                    //    return (_SortMode == 1 ? 1 : -1) * (((CommonFrontEnd.Model.MarketWatchModel)x).ChangeL).ToString().CompareTo((((CommonFrontEnd.Model.MarketWatchModel)y).ChangeL).ToString());
                    //    break;

                    //case "BDRateL":
                    //    return (_SortMode == 1 ? 1 : -1) * (((CommonFrontEnd.Model.MarketWatchModel)x).BDRateL).ToString().CompareTo((((CommonFrontEnd.Model.MarketWatchModel)y).BDRateL).ToString());
                    //    break;

                    //case "SDRateL":
                    //    return (_SortMode == 1 ? 1 : -1) * (((CommonFrontEnd.Model.MarketWatchModel)x).SDRateL).ToString().CompareTo((((CommonFrontEnd.Model.MarketWatchModel)y).SDRateL).ToString());
                    //    break;
                    
                    //case "BRPL":
                    //    return (_SortMode == 1 ? 1 : -1) * (((CommonFrontEnd.Model.MarketWatchModel)x).BRPL).ToString().CompareTo((((CommonFrontEnd.Model.MarketWatchModel)y).BRPL).ToString());
                    //    break;

                    //case "DTExpL":
                    //    return (_SortMode == 1 ? 1 : -1) * (((CommonFrontEnd.Model.MarketWatchModel)x).DTExpL).ToString().CompareTo((((CommonFrontEnd.Model.MarketWatchModel)y).DTExpL).ToString());
                    //    break;

                    //case "LstUpdtTimeL":
                    //    return (_SortMode == 1 ? 1 : -1) * (((CommonFrontEnd.Model.MarketWatchModel)x).LstUpdtTimeL).ToString().CompareTo((((CommonFrontEnd.Model.MarketWatchModel)y).LstUpdtTimeL).ToString());
                    //    break;

                    //case "NoOfTradesL":
                    //    return (_SortMode == 1 ? 1 : -1) * (((CommonFrontEnd.Model.MarketWatchModel)x).NoOfTradesL).ToString().CompareTo((((CommonFrontEnd.Model.MarketWatchModel)y).NoOfTradesL).ToString());
                    //    break;
                    */


            }

            return 0;
        }
    }


#if TWS

    public partial class MarketWatchVM : INotifyPropertyChanged
    {

        static DirectoryInfo masterPath = new DirectoryInfo(Path.GetFullPath(Path.Combine(System.Environment.CurrentDirectory, @"Profile/MarketWatch/")));

        static Window UserControlWindow = new Window();

        private static string _GroupComboSelectedItem;

        public static string GroupComboSelectedItem
        {
            get { return _GroupComboSelectedItem; }
            set
            {
                _GroupComboSelectedItem = value;
                NotifyStaticPropertyChanged("GroupComboSelectedItem");
                //OnChangeOfMarketMovers();
            }
        }

        public static void CookTouchLineData(string name)
        {
            List<int> objScripCodeFromSettingsSendLst = new List<int>();
            //int ScripCode = 0;
            string[] strArray = null;
            try
            {
                if (name == null)
                {
                    strArray = ObjCommon.ReaDFromCSV();
                }
                else
                {
                    string UserDefinedPath = Path.Combine(masterPath.ToString(), name);
                    if (File.Exists(UserDefinedPath + ".csv"))
                        strArray = File.ReadAllLines(UserDefinedPath + ".csv");

                    else
                        return;

                }
                //SubscribeScripMemory.objMasterSubscribeScrip.Clear();

                //strArray.GetLength(0);
                for (int i = 0, j = 0; i < strArray.GetLength(0); i++)
                {
                    SubscribeList.SubscribeScrip s = new SubscribeScrip();
                    string[] strArray2 = strArray[i].Split(new char[] { ',' });

                    MarketWatchModel item = new MarketWatchModel();
                    ScripDetails objscrip = new ScripDetails();


                    if (!string.IsNullOrEmpty(strArray2[0]) && !string.IsNullOrEmpty(strArray2[1]))
                    {
                        if (strArray2.Length == 2)
                            item.Scriptcode1 = Convert.ToInt32(strArray2[1]);
                        else
                            item.Scriptcode1 = Convert.ToInt32(CommonFunctions.GetScripCode(strArray2[0], Enumerations.Exchange.BSE, Enumerations.Segment.Equity));//BSE EQUITY
                    }
                    else
                        item.Scriptcode1 = 0;





                    //if (ObjTouchlineDataCollection != null && ObjTouchlineDataCollection.Count != 0)
                    //{
                    //    MarketWatchModel obj = ObjTouchlineDataCollection.Where(x => x.Scriptcode1 == item.Scriptcode1).FirstOrDefault();
                    //    if (obj != null)
                    //        continue;
                    //}

                    if ((item.Scriptcode1 != 0) && (!CommonFunctions.ValidScripOrNot(item.Scriptcode1)) && (!string.IsNullOrEmpty(strArray2[0])))
                        continue;


                    item.Index = j;
                    j++;



                    //DecimalPnt = CommonFunctions.GetDecimal(item.Scriptcode1, Enumerations.Exchange.BSE, Enumerations.Segment.Equity);//BSE EQUITY
                    //DecimalPoint =  CommonFunctions.GetDecimal(item.Scriptcode1),
                    //objscrip = BroadcastMasterMemory.objScripDetailsConDict.Where(x => x.Key == item.Scriptcode1).Select(x => x.Value).FirstOrDefault();                    
                    //s.ScripCode_l = item.Scriptcode1;
                    //s.UpdateFlag_s = 1;
                    //if (SubscribeScripMemory.objMasterSubscribeScrip.ContainsKey(s.ScripCode_l))
                    //{ }
                    //else
                    //    SubscribeScripMemory.objMasterSubscribeScrip.TryAdd(s.ScripCode_l, s);
                    //BroadcastReceiver.ScripDetails Br = BroadcastMaster.BroadcastMasterMemory.objScripDetailsConDict.Where(x => x.Key == item.Scriptcode1).Select(x => x.Value).FirstOrDefault();
                    ////   if (Br != null)
                    //{
                    //    objscrip = MainWindowVM.UpdateScripDataFromMemory(Br);
                    //    objscrip.ScriptCode_BseToken_NseToken = item.Scriptcode1;
                    //    Initialize_Fields(objscrip, ref item, DecimalPnt, item.Scriptcode1);
                    if (!string.IsNullOrEmpty(strArray2[0]))
                        item.ScriptId1 = strArray2[0];
                    else
                        item.ScriptId1 = string.Empty;
                    ObjTouchlineDataCollection.Add(item);
                    //    objBroadCastProcessor_OnBroadCastRecievedNew(objscrip);
                    //}
                }

            }
            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
            }
            SearchTemplist = ObjTouchlineDataCollection.Cast<MarketWatchModel>().ToList();
            // return objScripCodeFromSettingsSendLst;
        }

        private static void UpdatingColumnProfile(string TabName, bool OnSubMenuSelection)
        {

            //ScripProfilingVM.LoadMapping();
            string ProfileName = "";
            if (ProfileName == "")
            {
                string ProfileNameRecieved = "";
                string strQuery;
                string NewTabName;
                NewTabName = TabName + ".csv";
                if (TabName != "")
                {

                    try
                    {
                        // MasterSharedMemory.oDataAccessLayer.Connect((int)DataAccessLayer.ConnectionDB.Masters);
                        // MasterSharedMemory.oDataAccessLayer.OpenConnection((int)DataAccessLayer.ConnectionDB.Masters);
                        oDataAccessLayer1.OpenConnection((int)DataAccessLayer.ConnectionDB.Masters);
                        // oDataAccessLayer1.OpenConnection((int)DataAccessLayer.ConnectionDB.Masters);
                        //ProfileNameRecieved = ScripProfilingVM.ScripColMapping.Where(y => y.MarketWatch == TabName).Select(x => x.ProfileName).FirstOrDefault();
                        strQuery = @"SELECT ProfileName FROM SCRIP_COL_PROFILE_MAPPING where MemberID=" + "'" + UtilityLoginDetails.GETInstance.MemberId.ToString() + "' AND TraderID =" + "'" + UtilityLoginDetails.GETInstance.TraderId.ToString() + "' AND MarketWatch =" + "'" + NewTabName + "'";
                        oSQLiteDataReader = oDataAccessLayer1.ExecuteDataReader((int)ConnectionDB.Masters, strQuery, System.Data.CommandType.Text, null);// .ExecuteDataReader(strQuery, System.Data.CommandType.Text, null);
                        ProfileNameRecieved = "DEFAULT";
                        while (oSQLiteDataReader.Read())
                        {
                            ProfileNameRecieved = oSQLiteDataReader["ProfileName"]?.ToString().Trim();
                        }

                    }
                    catch (Exception ex)
                    {
                        ExceptionUtility.LogError(ex);
                    }
                    finally
                    {
                        //MasterSharedMemory.oDataAccessLayer.CloseConnection((int)DataAccessLayer.ConnectionDB.Masters);
                        oDataAccessLayer1.CloseConnection((int)DataAccessLayer.ConnectionDB.Masters);
                    }
                }
                //       List<string> objListOfDefaultColumn = new List<string>();

                //  DataGrid objDataGrid = System.Windows.Application.Current.Windows.OfType<MarketWatch>().FirstOrDefault().dataGridView1;
                //  DataGrid objDataGrid = null;
                //  objDataGrid = System.Windows.Application.Current.Windows.OfType<MarketWatch>().FirstOrDefault().dataGridView1; 

                if ((ProfileNameRecieved == null || TabName == "") && OnSubMenuSelection == false)
                {
                    /* foreach (DataGridColumn Colm in objDataGrid.Columns)
                     {
                         Colm.Visibility = Visibility.Hidden;
                     }

                     foreach (var item in MasterSharedMemory.objTouchlineCollection.Where(x => x.ScreenName == "Touchline" && (x.DefaultColumns == 2 || x.DefaultColumns == 1)).ToList())
                     {
                         string ColumnName = item.FieldName.Split('(')[1].Split(')')[0];
                         objListOfDefaultColumn.Add(ColumnName);

                         DataGridColumn objDataGridColumn = objDataGrid.Columns.Where(x => x.Header.ToString() == ColumnName).FirstOrDefault();

                         ObservableCollection<DataGridColumn> objDataGridColumns = objDataGrid.Columns;

                         if (objDataGridColumn != null)
                         {
                             objDataGridColumn.Visibility = Visibility.Visible;
                         }
                         //  objDataGrid.Columns.Where(x => x.Header.ToString() == ColumnName).FirstOrDefault().
                     }*/
                    ProfileNameRecieved = "DEFAULT";
                    GetColumnsRelatedToThisProfile(ProfileNameRecieved);

                }
                else
                {
                    if (!OnSubMenuSelection)
                    {
                        // GetColumn(TabName);
                        // for (int i = 0; i < objProfileNames.Count; i++)
                        {
                            //GetColumnsRelatedToThisProfile(objProfileNames[i]);
                            GetColumnsRelatedToThisProfile(ProfileNameRecieved);
                        }
                    }
                    else
                    {
                        GetColumnsRelatedToThisProfile(SelectedProfilename);
                    }

                }
            }
        }
        private static void GetColumnsRelatedToThisProfile(string ProfileNm)
        {
            //  FetchedColumnsFromMemory = new ObservableCollection<ColumnProfilingModel>();
            try
            {
                string strQuery = string.Empty;
                oDataAccessLayer1.OpenConnection((int)DataAccessLayer.ConnectionDB.Masters);
                //MasterSharedMemory.oDataAccessLayer.Connect((int)DataAccessLayer.ConnectionDB.Masters);
                // MasterSharedMemory.oDataAccessLayer.OpenConnection((int)DataAccessLayer.ConnectionDB.Masters);
                // oDataAccessLayer1.OpenConnection((int)DataAccessLayer.ConnectionDB.Masters);

                DataGrid objDataGrid = null;
                objDataGrid = System.Windows.Application.Current.Windows.OfType<MarketWatch>().FirstOrDefault().dataGridView1;

                if (ProfileNm == "DEFAULT")
                {
                    strQuery = @"SELECT distinct(FieldName) FROM TOUCHLINE where ((FEColumns='1' OR FEColumns='3') AND (DefaultColumns=='1' OR DefaultColumns=='2'))";
                }
                else
                    strQuery = @"SELECT distinct(ColumnName) FROM USER_DEFINED_PROFILE where MemberID=" + "'" + UtilityLoginDetails.GETInstance.MemberId.ToString() + "' AND TraderID =" + "'" + UtilityLoginDetails.GETInstance.TraderId.ToString() + "' AND ScreenName='Touchline' AND ProfileName =" + "'" + ProfileNm + "'";

                oSQLiteDataReader = oDataAccessLayer1.ExecuteDataReader((int)ConnectionDB.Masters, strQuery, System.Data.CommandType.Text, null);

                ObservableCollection<DataGridColumn> objDataGridColumns = objDataGrid.Columns;
                foreach (var item in objDataGridColumns)
                {
                    item.Visibility = Visibility.Hidden;
                }

                while (oSQLiteDataReader.Read())
                {
                    string ColumnProfile = string.Empty;
                    if (ProfileNm == "DEFAULT")
                        ColumnProfile = oSQLiteDataReader["FieldName"]?.ToString().Trim();
                    else
                        ColumnProfile = oSQLiteDataReader["ColumnName"]?.ToString().Trim();
                    string ColumnName = ColumnProfile.Split('(')[1].Split(')')[0];

                    DataGridColumn objDataGridColumn = objDataGrid.Columns.Where(x => x.Header.ToString() == ColumnName).FirstOrDefault();

                    if (objDataGridColumn != null)
                    {
                        objDataGridColumn.Visibility = Visibility.Visible;
                    }

                }
                OnSubMenuSelection = false;
            }
            catch (Exception e)
            {
                //ExceptionUtility.LogError(e);
            }
            finally
            {
                // MasterSharedMemory.oDataAccessLayer.CloseConnection((int)DataAccessLayer.ConnectionDB.Masters);
                oDataAccessLayer1.CloseConnection((int)DataAccessLayer.ConnectionDB.Masters);

            }
        }
        private static void GetColumn(string TabName)
        {
            try
            {
                //MasterSharedMemory.oDataAccessLayer.Connect((int)DataAccessLayer.ConnectionDB.Masters);
                //MasterSharedMemory.oDataAccessLayer.OpenConnection((int)DataAccessLayer.ConnectionDB.Masters);
                oDataAccessLayer1.OpenConnection((int)DataAccessLayer.ConnectionDB.Masters);

                string strQuery = @"SELECT ProfileName FROM SCRIP_COL_PROFILE_MAPPING where MarketWatch=" + "'" + TabName + "'";

                oSQLiteDataReader = oDataAccessLayer1.ExecuteDataReader((int)ConnectionDB.Masters, strQuery, System.Data.CommandType.Text, null);

                while (oSQLiteDataReader.Read())
                {

                    string ColumnProfile = oSQLiteDataReader["ProfileName"]?.ToString().Trim();
                    objProfileNames.Add(ColumnProfile);
                }
            }
            catch (Exception e)
            {
                //ExceptionUtility.LogError(e);
            }
            finally
            {
                //  MasterSharedMemory.oDataAccessLayer.CloseConnection((int)DataAccessLayer.ConnectionDB.Masters);
                oDataAccessLayer1.CloseConnection((int)DataAccessLayer.ConnectionDB.Masters);
            }
        }
        private static void PopulateScripInsertControl()
        {
            //UserControlWindow = new Window();
            UserControlWindow.Title = "Add Scrips";
            //ScripInsertUC oScripInsertUC = System.Windows.Application.Current.Windows.OfType<ScripInsertUC>().FirstOrDefault();
            //if (oScripInsertUC != null)
            //    UserControlWindow.Content = oScripInsertUC;
            //else
            UserControlWindow.Content = new ScripInsertUC();
            UserControlWindow.Height = 78;
            UserControlWindow.Width = 780;
            UserControlWindow.ResizeMode = ResizeMode.NoResize;
            UserControlWindow.WindowStyle = WindowStyle.None;
            UserControlWindow.AllowsTransparency = false;
            UserControlWindow.BorderThickness = new Thickness(1);
            UserControlWindow.BorderBrush = System.Windows.Media.Brushes.Black;
            UserControlWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            //if (ScripInsertUC_VM.OnScripIDOrCodeChange == null)
            //   ScripInsertUC_VM.OnScripIDOrCodeChange += BestFiveVM.UpdateValues;
            UserControlWindow.ShowInTaskbar = false;
            //UserControlWindow.Closing += UserControlWindow_Closing;
            ScripInsertUC_VM.OnScripAddTouchLine += AddNewScrip;
        }
        private static void UserControlWindow_Closing(object sender, CancelEventArgs e)
        {
            if (ScripInsertUC_VM.OnScripIDOrCodeChange == null)
            {
                ScripInsertUC_VM.OnScripIDOrCodeChange += BestFiveVM.UpdateValues;
                //ScripInsertUC_VM.OnScripIDOrCodeChange += BestFiveVM.FetchNetPositionByScripCode;
            }
        }
        private static void AddNewScrip(long scripcode)
        {
            try
            {
                int recscripcode = Convert.ToInt32(scripcode);
                LastData = null;
                //  ScripDetails objScripDetails = BroadcastMasterMemory.objScripDetailsConDict.Where(x => x.Key == recscripcode).Select(x => x.Value).FirstOrDefault();

                if (ObjTouchlineDataCollection != null && ObjTouchlineDataCollection.Count != 0)
                {
                    MarketWatchModel obj = ObjTouchlineDataCollection.Where(x => x.Scriptcode1 == scripcode).FirstOrDefault();
                    if (obj != null)
                    {
                        System.Windows.MessageBox.Show("Scrip Already Present", "Add Scrips", MessageBoxButton.OK, MessageBoxImage.None);

                        if (UserControlWindow != null && UserControlWindow.IsActive)
                        {
                            UserControlWindow.Hide();
                            ScripInsertUC_VM.TouchLineInsert = false;
                        }
                        return;
                    }
                }

                MarketWatchModel objtouch = new MarketWatchModel();
                objtouch.Scriptcode1 = Convert.ToInt32(recscripcode);
                objtouch.ScriptId1 = CommonFrontEnd.Common.CommonFunctions.GetScripId(recscripcode, "BSE", CommonFunctions.GetSegmentID(recscripcode)).Trim();//MasterSharedMemory.objMastertxtDic.Where(x => x.Key == recscripcode).Select(x => x.Value.ScripId).FirstOrDefault();
                objtouch.Index = ObjTouchlineDataCollection.Count;

                DataGrid_ViewUpdate(objtouch, "ScripInsert");

                int Segm = CommonFunctions.GetSegmentFromScripCode(recscripcode);
                if (Segm == 1)
                    DecimalPnt = CommonFunctions.GetDecimal(recscripcode, Enumerations.Exchange.BSE, Enumerations.Segment.Equity);
                else if (Segm == 2)
                    DecimalPnt = CommonFunctions.GetDecimal(recscripcode, Enumerations.Exchange.BSE, Enumerations.Segment.Derivative);
                else if (Segm == 3)
                    DecimalPnt = CommonFunctions.GetDecimal(recscripcode, Enumerations.Exchange.BSE, Enumerations.Segment.Currency);
                else
                    DecimalPnt = 2;

                /*
                if (ObjTouchlineDataCollection.Count > 0)
                {
                    if (SelectedItem != null)
                        LastData = ObjTouchlineDataCollection.IndexOf(SelectedItem); //ObjTouchlineDataCollection.IndexOf(ObjTouchlineDataCollection.FirstOrDefault(i => i.Scriptcode1 == SelectedItem.Scriptcode1));
                    else
                        LastData = ObjTouchlineDataCollection.IndexOf(ObjTouchlineDataCollection.Last());
                }

                if (LastData != null)
                    ObjTouchlineDataCollection.Insert(Convert.ToInt32(LastData) + 1, objtouch);
                else
                    ObjTouchlineDataCollection.Add(objtouch);
                */
                SubscribeList.SubscribeScrip s = new SubscribeScrip();
                s.ScripCode_l = recscripcode;
                s.UpdateFlag_s = 1;
                if (SubscribeScripMemory.objMasterSubscribeScrip.ContainsKey(recscripcode))
                { }
                else
                    SubscribeScripMemory.objMasterSubscribeScrip.TryAdd(recscripcode, s);

                System.Windows.MessageBox.Show("Successfully added Scrip in TouchLine", "Add Scrips", MessageBoxButton.OK, MessageBoxImage.None);

                if (UserControlWindow != null && UserControlWindow.IsActive)
                {
                    //if (ScripInsertUC_VM.OnScripIDOrCodeChange == null)
                    //    ScripInsertUC_VM.OnScripIDOrCodeChange += BestFiveVM.UpdateValues;
                    UserControlWindow.Hide();
                    ScripInsertUC_VM.TouchLineInsert = false;
                }

                SearchTemplist = ObjTouchlineDataCollection.Cast<MarketWatchModel>().ToList();
                TitleTouchLine = "TouchLine - " + CurrentTabSlected + "-" + ObjTouchlineDataCollection.Count;

            }


            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
                System.Windows.MessageBox.Show("Error while Adding Scrips", "Add Scrips", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private static void PopulatingDropDowns()
        {
            oDataAccessLayer1 = new DataAccessLayer();
            oDataAccessLayer1.Connect((int)DataAccessLayer.ConnectionDB.Masters);
            try
            {
                MarketsCombo.Add("-Pre_Defined-");

                //MasterSharedMemory.oDataAccessLayer.Connect((int)DataAccessLayer.ConnectionDB.Masters);
                //MasterSharedMemory.oDataAccessLayer.OpenConnection((int)DataAccessLayer.ConnectionDB.Masters);
                oDataAccessLayer1.OpenConnection((int)DataAccessLayer.ConnectionDB.Masters);

                //MasterSharedMemory.oDataAccessLayer.Connect();
                //MasterSharedMemory.oDataAccessLayer.OpenConnection();

                string str = "SELECT * FROM BSE_SNPINDICES_CFE";//"SELECT Distinct(ExistingShortName) FROM BSE_SNPINDICES_CFE";
                SQLiteDataReader oSQLiteDataReader = oDataAccessLayer1.ExecuteDataReader((int)ConnectionDB.Masters, str, System.Data.CommandType.Text, null);
                while (oSQLiteDataReader.Read())
                {
                    AllIndicesModel omodel = new AllIndicesModel();

                    if (oSQLiteDataReader["ExistingShortName"] != string.Empty)
                    {
                        MarketsCombo.Add(oSQLiteDataReader["ExistingShortName"]?.ToString().Trim());
                        omodel.IndexName = oSQLiteDataReader["ExistingShortName"]?.ToString().Trim();
                    }

                    if (oSQLiteDataReader["IndexCode"] != string.Empty)
                        omodel.IndexCode = Convert.ToInt32(oSQLiteDataReader["IndexCode"]);

                    MarketList.Add(omodel);
                }
                MarketsComboSelectedItem = MarketsCombo[0];

            }
            catch (Exception e) { ExceptionUtility.LogError(e); }
            finally
            {
                // MasterSharedMemory.oDataAccessLayer.CloseConnection((int)DataAccessLayer.ConnectionDB.Masters);
                oDataAccessLayer1.CloseConnection((int)DataAccessLayer.ConnectionDB.Masters);

            }

        }
        private static void RefreshUserDefinedDropDowns()
        {
            //ScripProfilingCombo = new List<string>();
            PopulatingScripProfilingDropDown();
        }

        public static bool DeleteTabItems_Click(object e, int n)
        {
            TabItem t = e as TabItem;
            string SelTabName = ((TabCloseButton)t.Header).TabLblTitle.Content.ToString();
            string SelTabType = t.Name;
            TabControl maintab = System.Windows.Application.Current.Windows.OfType<MarketWatch>().FirstOrDefault().SnPSensexTab;

            if (maintab.Items.Count > 1 && n != 0)
            {
                var response = System.Windows.MessageBox.Show("Do you want to close the " + SelTabName + " tab?", "Close tab",
                                           MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (response == MessageBoxResult.Yes)
                {
                    Button btn = ((TabCloseButton)t.Header).TabBtnClose as Button;
                    btn.Click -= TabCloseButton_VM.TabBtnClose_Click;
                    maintab.Items.RemoveAt(n);

                    if (SelTabType == "Userdefined" && ScripProfComboSelectedItem == SelTabName)
                        ScripProfComboSelectedItem = ScripProfilingCombo[0];
                    else if (SelTabType == "Predefined" && MarketsComboSelectedItem == SelTabName)
                        MarketsComboSelectedItem = MarketsCombo[0];

                    return true;
                }
            }
            else
            {
                System.Windows.MessageBox.Show("You Cannot Close Default tab", "Default Tab", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            return false;
        }

        public static bool DeleteTabItems_Click(object e)
        {
            System.Windows.Controls.TabControl maintab = e as System.Windows.Controls.TabControl;
            string SelTabName = ((MarketWatchVM)maintab.DataContext).TabName.ToString();
            string SelTabType = ((FrameworkElement)maintab.SelectedItem).Name;
            // ((System.Windows.FrameworkElement)maintab.SelectedItem).Name;


            if (maintab.Items.Count > 1 && maintab.SelectedIndex != 0)
            {
                var response = System.Windows.MessageBox.Show("Do you want to close the " + SelTabName + " tab ?", "Close tab",
                                           MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (response == MessageBoxResult.Yes)
                {
                    Button btn = ((TabCloseButton)((HeaderedContentControl)maintab.SelectedItem).Header).TabBtnClose as Button;
                    btn.Click -= TabCloseButton_VM.TabBtnClose_Click;

                    //int PrevIndex = maintab.SelectedIndex;
                    maintab.Items.Remove(maintab.SelectedItem);

                    if (SelTabType == "Userdefined" && ScripProfComboSelectedItem == SelTabName)
                        ScripProfComboSelectedItem = ScripProfilingCombo[0];
                    else if (SelTabType == "Predefined" && MarketsComboSelectedItem == SelTabName)
                        MarketsComboSelectedItem = MarketsCombo[0];

                    return true;
                }
            }
            else
            {
                System.Windows.MessageBox.Show("You Cannot Close Default tab", "Default Tab", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            return false;
        }

        internal static void OpenMarketFromIndexWindow(int IndexCode)
        {
            string IndexName = MarketList.Where(x => x.IndexCode == IndexCode).Select(x => x.IndexName).FirstOrDefault();
            if (!string.IsNullOrEmpty(IndexName))
            {
                if (MarketsCombo.Contains(IndexName))
                {
                    MarketsComboSelectedItem = IndexName;
                }
            }
        }

    }
#elif BOW


    public partial class MarketWatchVM
    {
    #region Properties
        static Thread gobjBroadCastMessagePumpThread;
        static Thread gobjManageMessageThread;
        static MarketWatchHelper mobjMarketWatchHelper;
        static RecordSplitter lobjRecordHelper;

        
        static int mintSortableColumn = -1;
        static MWColumn[] lobjMWColumns = null;


        static string mstrID;
        static string mstrUSID;
        static string mstrName;
        static string mstrSegmentId;
        static string mstrFormat;
        static string mstrCreatedBy;
        static string mstrCreatedAt;
        static string mstrLastUpdatedBy;
        static string mstrLastUpdatedAt;
        static string mstrFIELD1;
        static string mstrFIELD2;
        static string mstrFIELD3;
        static string mstrFIELD4;
        static string mstrROWSTATE;
        static string mstrType;
        static string mstrAdmin;
        static string mstrNoOfLegs;
        static string mstrSearchColumn = "SYMBOL";
        //static string mstrSortType;
        static SortType mstrSortType;
        static string mstrSortFrequency;
        static ArrayList mMarketList = new ArrayList();
        static ArrayList mExchangeList = new ArrayList();
        static ArrayList mNameList = new ArrayList();

        string lstrKey = null;
    #endregion Properties

        public static void PopulatingDropDowns()
        {
            MarketsCombo.Add("-" + MarketWatchModel.Markets.Pre_Defined.ToString() + "-");
        }

        public static void CookTouchLineData()
        {
            int previousSelected;
            if (MarketWatchConstants.SelectedID != 0)
            {
                previousSelected = MarketWatchConstants.SelectedID;
                SettingsManager.SendCloseBroadcastMsg(previousSelected);

            }
            MarketWatchConstants.SelectedID = UtilityLoginDetails.GETInstance.gobjMarketWatchColl.objMarketWatchObj.Where(obj => obj.Name == ScripProfComboSelectedItem).Select(obj => obj.Id).FirstOrDefault();
           LoadMW(MarketWatchConstants.SelectedID);

        }

        static void LoadMW(int pintMarketWatchId)
        {
            string lstrURL = null;
            string lstrReturn;

            lstrURL = MarketWatchConstants.URL_LIST_ARBITRAGE_MARKET_WATCH;

            System.Collections.ArrayList lstrValues = new System.Collections.ArrayList();
            System.Collections.ArrayList lstrParameters = new System.Collections.ArrayList();
            lstrParameters.Add("marketwatchusid");
            lstrValues.Add(UtilityLoginDetails.GETInstance.UserLoginId);
            lstrParameters.Add("MWID");
            lstrValues.Add(pintMarketWatchId.ToString());
            //: Vaibhav added for HTTPBroadcast
            if (MarketWatchConstants.gblnConnectOnHTTP == true)
            {
                lstrParameters.Add(MarketWatchConstants.MODE_STRING);
                lstrValues.Add(MarketWatchConstants.PROXY_STRING);
            }
            //: Append LoginKey and Thick cleint
            lstrParameters.Add(UtilityConnParameters.GetInstance.LoginKeyName);
            lstrParameters.Add(UtilityConnParameters.GetInstance.LoginKeyValue);
            lstrValues.Add(UtilityConnParameters.GetInstance.ThickClientParameter);
            lstrValues.Add(UtilityConnParameters.GetInstance.ThickClientValue);


            lstrReturn = SettingsManager.GetDataFromServer(lstrURL, (string[])lstrParameters.ToArray(typeof(string)), (string[])lstrValues.ToArray(typeof(string)));
            lobjRecordHelper = new RecordSplitter(lstrReturn);
            if (lobjRecordHelper.getField(0, 0) == BowConstants.SUCCESS_FLAG)
            {
                mobjMarketWatchHelper = new MarketWatchHelper(lstrReturn);
            }
            int lintReocrdPointer = 0;
            FillMarketWatchData(ref lintReocrdPointer);
          
            SettingsManager.SendOpenBroadcastMsg(pintMarketWatchId);

            //gobjManageMessageThread = new Thread(SettingsManager.ManageAsynchronousBroadcast);
            //gobjManageMessageThread.Name = "Manage InComming Messages";
            //gobjManageMessageThread.Start();

            //gobjBroadCastMessagePumpThread = new Thread(SettingsManager.BroadcastMessagePump);
            //gobjBroadCastMessagePumpThread.Name = "BroadcastMessagePump";
            //gobjBroadCastMessagePumpThread.Priority = ThreadPriority.AboveNormal;
            //gobjBroadCastMessagePumpThread.Start();
                     


        }

        public static void FillMarketWatchData(ref int lintReocrdPointer)
        {
           
            lintReocrdPointer += 1;

            mstrID = lobjRecordHelper.getField(lintReocrdPointer, (int)FIELDS.ID);
            mstrUSID = lobjRecordHelper.getField(lintReocrdPointer, (int)FIELDS.USID);
            mstrName = lobjRecordHelper.getField(lintReocrdPointer, (int)FIELDS.Name);
            mstrSegmentId = lobjRecordHelper.getField(lintReocrdPointer, (int)FIELDS.SegmentId);
            mstrFormat = lobjRecordHelper.getField(lintReocrdPointer, (int)FIELDS.Format);
            mstrCreatedBy = lobjRecordHelper.getField(lintReocrdPointer, (int)FIELDS.CreatedBy);
            mstrCreatedAt = lobjRecordHelper.getField(lintReocrdPointer, (int)FIELDS.CreatedAt);
            mstrLastUpdatedBy = lobjRecordHelper.getField(lintReocrdPointer, (int)FIELDS.LastUpdatedBy);
            mstrLastUpdatedAt = lobjRecordHelper.getField(lintReocrdPointer, (int)FIELDS.LastUpdatedAt);
            mstrFIELD1 = lobjRecordHelper.getField(lintReocrdPointer, (int)FIELDS.FIELD1);
            mstrFIELD2 = lobjRecordHelper.getField(lintReocrdPointer, (int)FIELDS.FIELD2);
            mstrFIELD3 = lobjRecordHelper.getField(lintReocrdPointer, (int)FIELDS.FIELD3);
            mstrFIELD4 = lobjRecordHelper.getField(lintReocrdPointer, (int)FIELDS.FIELD4);
            mstrROWSTATE = lobjRecordHelper.getField(lintReocrdPointer, (int)FIELDS.ROWSTATE);
            mstrType = lobjRecordHelper.getField(lintReocrdPointer, (int)FIELDS.Type);
            mstrAdmin = lobjRecordHelper.getField(lintReocrdPointer, (int)FIELDS.Admin);
            mstrNoOfLegs = lobjRecordHelper.getField(lintReocrdPointer, (int)FIELDS.NoOfLegs);
           // mstrSortType.= lobjRecordHelper.getField(lintReocrdPointer, (int)FIELDS.SortType);
            mstrSortFrequency = lobjRecordHelper.getField(lintReocrdPointer,(int) FIELDS.SortFrequency);

           
         

            string[] mstrLegsDetailArray = mstrFIELD4.Split(',');
            for (Int16 i = 0; i <= mstrLegsDetailArray.Length - 1; i++)
            {
                string[] mstrLegDetailArray = mstrLegsDetailArray[i].Split('^');
                if (mstrLegDetailArray.Length >= 3)
                {
                    mNameList.Add(mstrLegDetailArray[0]);
                    mExchangeList.Add(mstrLegDetailArray[1]);
                    mMarketList.Add(mstrLegDetailArray[2]);
                }
            }

            lintReocrdPointer += 1;

            FillColumns(ref lintReocrdPointer);
            List<int> objListOfScripCodes = new List<int>();


            if (lobjRecordHelper.numberOfRecords() > lintReocrdPointer)
            {
                string blank = "";
                FillDatahash_BC_And_Attribute(ref lintReocrdPointer, ref blank);
            }
            if (lobjRecordHelper.numberOfRecords() > lintReocrdPointer)
            {
                FillRows(ref lintReocrdPointer);
            }

        }

        public static bool FillColumns(ref int lintReocrdPointer)
        {

            try
            {
                int lintNumberOfColumn = Convert.ToInt32(lobjRecordHelper.getField(lintReocrdPointer, 2));

                //lobjMWColumns = mobjMarketWatchControl.MW_Columns_Array
                // ERROR: Not supported in C#: ReDimStatement

                lintReocrdPointer += 1;
                lobjMWColumns = new MWColumn[lintNumberOfColumn];
                for (int lcount = 0; lcount <= lintNumberOfColumn - 1; lcount++)
                {
                    string[] lstrCol = lobjRecordHelper.getRecord(lintReocrdPointer);
                    //: Patch : 
                    if (lstrCol[(int)MWColumn.MWCOLS_FIELDS.PositionOnScreen].Trim().Length > 0)
                    {
                        if (Convert.ToInt32(lstrCol[(int)MWColumn.MWCOLS_FIELDS.PositionOnScreen]) != lcount)
                        {
                            lstrCol[(int)MWColumn.MWCOLS_FIELDS.PositionOnScreen] = lcount.ToString();
                        }
                    }
                    //:

                    lobjMWColumns[lcount] = new MWColumn(lstrCol, ref mNameList);
                    //if (lobjMWColumns[lcount].MintSortDirection > 0)
                    //{
                    //    mintSortableColumn = lobjMWColumns[lcount].MintPosition;
                    //}
                    lintReocrdPointer += 1;
                }
                //mobjMarketWatchControl.MW_Columns_Array = lobjMWColumns;
                return true;
            }
            catch (Exception ex)
            {
                // throw new Exception(ex.Message + Constants.vbCrLf + "Error in FillColumns: Error is ", ex);
                return false;
            }
            return true;

        }

        public static void FillDatahash_BC_And_Attribute(ref int pintReocrdPointer, ref string pstrBCMessageArrived_OR_ReturnKeys)
        {
            try
            {

                int lintNoOfBroadCast = 0;
                string[] larrstrCol = null;
                ArrayList lobjMWRefList = new ArrayList();

                bool lblnSendKey = false;
                string lsterRecordSpliter = "";
                bool lblnBroadcastMsgRecd = false;
                //MessageArrived in frmMarketWatch
                if (pintReocrdPointer == -1)
                {
                    pintReocrdPointer = 0;
                    lobjRecordHelper = new RecordSplitter(pstrBCMessageArrived_OR_ReturnKeys);
                    //for finding script lobjUtilityScript = New Utilities.UtilitiesHelper.UtilityScript
                    lintNoOfBroadCast = pintReocrdPointer + 1;
                    lblnBroadcastMsgRecd = true;
                    //FillMarketWatchData & ADDROW()
                }
                else
                {
                    lintNoOfBroadCast = Convert.ToInt32(lobjRecordHelper.getField(pintReocrdPointer, 2));
                    pintReocrdPointer += 1;
                    //ReDim mBroadcast(lintCountOfRows_BroadCast) 'in our case 3BC (relianceEq,fut1,fut2) relianceEq is there in both rows so it will not send two times
                    lobjRecordHelper = lobjRecordHelper;
                    lblnSendKey = true;
                }
                MarketWatchModel item;
                ScripDetails objscrip = new ScripDetails();
                
                List<int> objScripCode = new List<int>();
                SubscribeList.SubscribeScrip s = new SubscribeScrip();
                for (Int16 lcount = 0; lcount <= lintNoOfBroadCast - 1; lcount++)
                {
                    item = new MarketWatchModel();
                    //If Not IsNothing(pstrBCMessageArrived_OR_ReturnKeys) Then
                    larrstrCol = lobjRecordHelper.getRecord(pintReocrdPointer);
                    //  objscrip = MainWindowVM.UpdateScripDataFromMemory(larrstrCol);

                    if (larrstrCol[48] == "NSE" || larrstrCol[48] == "NCDEX" || larrstrCol[48] == "MCX")
                        item.Scriptcode1 = Convert.ToInt32(larrstrCol[5]);
                    if (larrstrCol[48] == "BSE")
                        item.Scriptcode1 = Convert.ToInt32(larrstrCol[6]);
                  

                    s.ScripCode_l = item.Scriptcode1;  
                    objscrip= MainWindowVM.UpdateScripDataFromMemory(larrstrCol);
                    DecimalPnt = CommonFunctions.GetDecimal(s.ScripCode_l, Enumerations.Exchange.BSE, Enumerations.Segment.Equity);
                    Initialize_Fields(objscrip, ref item, DecimalPnt, item.Scriptcode1);
                    if (larrstrCol[48] == "NCDEX")
                    {
                        if (MasterSharedMemory.objNCDEXMasterDict.Values.Any(x => x.NCDToken == item.Scriptcode1))
                            ObjTouchlineDataCollection.Add(item);
                    }
                    if (larrstrCol[48] == "MCX")
                    {
                        if (MasterSharedMemory.objMCXMasterDict.Values.Any(x => x.MCToken == item.Scriptcode1))
                            ObjTouchlineDataCollection.Add(item);
                    }
                    if (larrstrCol[48] == "NSE")
                    {
                        if(MasterSharedMemory.objMastertxtDictBaseNSE.Values.Any(x=>x.ScripCode==item.Scriptcode1))
                          ObjTouchlineDataCollection.Add(item);
                    }
                    if (larrstrCol[48] == "BSE")
                    {
                        if (MasterSharedMemory.objMastertxtDictBaseBSE.Values.Any(x => x.ScripCode == item.Scriptcode1))
                            ObjTouchlineDataCollection.Add(item);
                    }

                   
                     objBroadCastProcessor_OnBroadCastRecievedNew(objscrip);
                    
                    pintReocrdPointer += 1;

                }



            }
            catch (Exception ex)
            {
                CommonFrontEnd.Infrastructure.Logger.WriteLog("Error FillDatahash_BC_And_Attribute " + ex.StackTrace);
                //Throw New Exception(ex.Message & vbCrLf & "Error in FillDatahash : Error is ", ex)

            }
        }

        private static void FillRows(ref int pintReocrdPointer)
        {
            try
            {
                int lintNoOfRows_inMW = Convert.ToInt32(lobjRecordHelper.getField(pintReocrdPointer, 2));

                string lstrkeys = "";
                string lstrRecordSepretor = "";
                pintReocrdPointer += 1;
                if (lobjRecordHelper.numberOfRecords() - pintReocrdPointer - lintNoOfRows_inMW < 0)
                {
                    lintNoOfRows_inMW = lobjRecordHelper.numberOfRecords() - pintReocrdPointer;
                }

                for (int lRowCount = 0; lRowCount <= lintNoOfRows_inMW - 1; lRowCount++)
                {
                    if (lobjRecordHelper.numberOfRecords() > pintReocrdPointer)
                    {
                        string[] larrstrRow = lobjRecordHelper.getRecord(pintReocrdPointer);
                        string KeysTEM = larrstrRow[(int)MWROW_FIELDS.Token] + "^" + larrstrRow[(int)MWROW_FIELDS.Exchange] + "^" + larrstrRow[(int)MWROW_FIELDS.Market];
                        objScripCodes.Add(Convert.ToInt32(larrstrRow[(int)MWROW_FIELDS.Token]));
                        pintReocrdPointer += 1;
                        lstrkeys = lstrkeys + lstrRecordSepretor + KeysTEM;
                        lstrRecordSepretor = "~";
                    }
                    else
                    {
                        //  Infrastructure.Logger.WriteLog("Less row data send by server against explisitly declared in the header. ---->" + mobjRecordHelper.GetActualData);
                        break; // TODO: might not be correct. Was : Exit For
                    }
                }
                Globals.GETInstance.MW_Rows_Array = lstrkeys;
                //  return MW_Rows_Array;
            }
            catch (Exception ex)
            {
                // throw new Exception(ex.Message + Constants.vbCrLf + "Error in FillRows: Error is ", ex);

            }
        }

        public void MessageArrived(string pstrMessage)
        {
            //SyncLock Me
            string[] lobjMWRefList = null;
            try
            {
                if ((mobjMarketWatchHelper != null))
                {
                    int pointer = -1;
                    FillDatahash_BC_And_Attribute(ref pointer, ref pstrMessage);
                    //if ((lobjMWRefList != null) && mobjMarketWatchHelper.MW_Formula_Columns.Count > 0)
                    //{
                    //    EvaluateFormulasFromBroadcast(lobjMWRefList);

                    //}
                }
            }
            catch (Exception ex)
            {
                CommonFrontEnd.Infrastructure.Logger.WriteLog("Error frmMultilegMW MessageArrived" + ex.StackTrace);
                CommonFrontEnd.Infrastructure.Logger.WriteLog("Error frmMultilegMW MessageArrived" + ex.Message);
            }

            //End SyncLock
        }

        //: had to convert to string as when using reflection only string paramerters are passed.
        private string GetDataByCallingServlet(string pstrSegment, string pstrExchange, string pstrToken, string pstrOldMBPToken)
        {
            string lstrReturn=null;
            //: Call the servlet 
            string[] larrParameterName = new string[7];
            string[] larrParameterValue = new string[7];

            larrParameterName[0] = "marketbypricesegment";
            larrParameterValue[0] = pstrSegment;
            larrParameterName[1] = "marketbypricesource";
            larrParameterValue[1] = pstrExchange;
            // exchange id
            larrParameterName[2] = "marketbypricetoken";
            larrParameterValue[2] = pstrToken;
            larrParameterName[3] = "CloseMBP";
            larrParameterValue[3] = pstrOldMBPToken;
            //: Vaibhav : Added for HTTP Broadcast mode
            if (MarketWatchConstants.gblnConnectOnHTTP == true)
            {
                larrParameterName[4] = MarketWatchConstants.MODE_STRING;
                larrParameterValue[4] = MarketWatchConstants.PROXY_STRING;
            }
            try
            {
                //gfrmActiveMarketWatch.MarketWatchHelper.getMarketWatchControl.RePaintRow(gfrmActiveMarketWatch.MarketWatchHelper.getMarketWatchControl.CurRow)
                //: A callback will be made to the RefreshForm Function
                //: lstrResult will return blank value
                //if (Globals.GETInstance.gblnDirectBroadcastConfigured)
                //{
                //    gfrmActiveMarketWatch.MarketWatchHelper.PaintRowState(gfrmActiveMarketWatch.MarketWatchHelper.getCurrentlySelectedLegsKey());
                //}
                //HTTPRequestHelper.HTTPHelper.ResponseReturned lobjDelegate = new HTTPRequestHelper.HTTPHelper.ResponseReturned(RefreshForm);
                //HTTPHlpr.ResponseReturned lobjDelegate = new HTTPHlpr.ResponseReturned(RefreshForm);
                lstrReturn = SettingsManager.GetDataFromServer("GetMarketByPrice", larrParameterName, larrParameterValue, false, null);
       
                return lstrReturn;
            }
            catch (Exception ex)
            {
                return lstrReturn;
            }
        }
        public static string GetTokenFromSelectedLegKey(string pstrKey)
        {
            string[] lstrValues = null;
            lstrValues = pstrKey.Split('^');
            string lstrToken = "";
            if (!string.IsNullOrEmpty(lstrValues[0]))
            {
                lstrToken = lstrValues[0];
            }
            return lstrToken;
        }

        public static Model.ScripDetails UpdateData(ArrayList pobjArraylist,string lstrTokenString)
        {
            //
            //   If pstrMBPmsg.Substring(0, 2) = "60" Then

            //    if(pobjArraylist[0].=="60")
            //{
            //    pobjArraylist[0] = "0|~0|1|54~" + pobjArraylist[0];
            //}
           
          RecordSplitter pobjRH =(RecordSplitter) pobjArraylist[0];
            bool pblnIsServletCall =(bool) pobjArraylist[1];
            string lstrExchange = null;

            int lintRecord = 0;
            int lintDisplacement = 0;
            if (pblnIsServletCall == true)
            {
                lintRecord = MarketWatchConstants.RECORD_POSITION_SERVLETCALL;
                lintDisplacement = MarketWatchConstants.DISPLACEMENT_SERVLETCALL;
            }
            else
            {
                lintRecord = MarketWatchConstants.RECORD_POSITION_UPDATE;
                lintDisplacement = MarketWatchConstants.DISPLACEMENT_UPDATE;
            }

                // lstrExchange = pobjRH.getField(lintRecord,Convert.ToInt32(Exchange));

                //: 1st Row

                {
                    string CommonToken = pobjRH.getField(lintRecord, (int)Enumerations.MktByPrice.CommonToken).Trim();
                    Model.ScripDetails s = MarketWatchConstants.objScripDetails.Where(p => p.CommonToken == CommonToken).FirstOrDefault();
                    if (s != null)
                    {

                    }
                    else
                    {
                        s = new Model.ScripDetails();
                    }
                    s.listBestFive = new List<BestFive>();
                    BestFive objBestFive;
                    string lstrValue = pobjRH.getField(lintRecord, (int)Enumerations.MktByPrice.NumberOfOrders1 - lintDisplacement).Trim();

                    objBestFive = new BestFive();
                    objBestFive.BuyQtyL = Convert.ToInt32(pobjRH.getField(lintRecord, (int)MktByPrice.Quantity1 - lintDisplacement).Trim());
                    objBestFive.BuyRateL = Convert.ToInt32(Convert.ToDouble(pobjRH.getField(lintRecord, (int)MktByPrice.Price1 - lintDisplacement).Trim()) * Math.Pow(10, DecimalPnt));

                    objBestFive.SellRateL = Convert.ToInt32(Convert.ToDouble(pobjRH.getField(lintRecord, (int)MktByPrice.Price6 - lintDisplacement).Trim()) * Math.Pow(10, DecimalPnt));

                    objBestFive.SellQtyL = Convert.ToInt32(pobjRH.getField(lintRecord, (int)MktByPrice.Quantity6 - lintDisplacement).Trim());
                    objBestFive.NoOfBidSellL = Convert.ToInt32(pobjRH.getField(lintRecord, (int)MktByPrice.NumberOfOrders6 - lintDisplacement).Trim());
                    objBestFive.NoOfBidBuyL = Convert.ToInt32(pobjRH.getField(lintRecord, (int)MktByPrice.NumberOfOrders1 - lintDisplacement).Trim());
                    s.listBestFive.Add(objBestFive);
                    s.NoOfBidBuyL = objBestFive.NoOfBidBuyL;
                    s.NoOfBidSellL = objBestFive.NoOfBidSellL;

                    //: 2nd Row
                    objBestFive = new BestFive();
                    objBestFive.BuyQtyL = Convert.ToInt32(pobjRH.getField(lintRecord, (int)MktByPrice.Quantity2 - lintDisplacement).Trim());
                    objBestFive.BuyRateL = Convert.ToInt32(Convert.ToDouble(pobjRH.getField(lintRecord, (int)MktByPrice.Price2 - lintDisplacement).Trim()) * Math.Pow(10, DecimalPnt));

                    objBestFive.SellRateL = Convert.ToInt32(Convert.ToDouble(pobjRH.getField(lintRecord, (int)MktByPrice.Price7 - lintDisplacement).Trim()) * Math.Pow(10, DecimalPnt));

                    objBestFive.SellQtyL = Convert.ToInt32(pobjRH.getField(lintRecord, (int)MktByPrice.Quantity7 - lintDisplacement).Trim());
                    objBestFive.NoOfBidSellL = Convert.ToInt32(pobjRH.getField(lintRecord, (int)MktByPrice.NumberOfOrders7 - lintDisplacement).Trim());
                    objBestFive.NoOfBidBuyL = Convert.ToInt32(pobjRH.getField(lintRecord, (int)MktByPrice.NumberOfOrders2 - lintDisplacement).Trim());

                    s.listBestFive.Add(objBestFive);

                    //: 3rd Row
                    objBestFive = new BestFive();
                    objBestFive.BuyQtyL = Convert.ToInt32(pobjRH.getField(lintRecord, (int)MktByPrice.Quantity3 - lintDisplacement).Trim());
                    objBestFive.BuyRateL = Convert.ToInt32(Convert.ToDouble(pobjRH.getField(lintRecord, (int)MktByPrice.Price3 - lintDisplacement).Trim()) * Math.Pow(10, DecimalPnt));

                    objBestFive.SellRateL = Convert.ToInt32(Convert.ToDouble(pobjRH.getField(lintRecord, (int)MktByPrice.Price8 - lintDisplacement).Trim()) * Math.Pow(10, DecimalPnt));

                    objBestFive.SellQtyL = Convert.ToInt32(pobjRH.getField(lintRecord, (int)MktByPrice.Quantity8 - lintDisplacement).Trim());
                    objBestFive.NoOfBidSellL = Convert.ToInt32(pobjRH.getField(lintRecord, (int)MktByPrice.NumberOfOrders8 - lintDisplacement).Trim());
                    objBestFive.NoOfBidBuyL = Convert.ToInt32(pobjRH.getField(lintRecord, (int)MktByPrice.NumberOfOrders3 - lintDisplacement).Trim());

                    s.listBestFive.Add(objBestFive);
                    //: 4th Row
                    objBestFive = new BestFive();
                    objBestFive.BuyQtyL = Convert.ToInt32(pobjRH.getField(lintRecord, (int)MktByPrice.Quantity4 - lintDisplacement).Trim());
                    objBestFive.BuyRateL = Convert.ToInt32(Convert.ToDouble(pobjRH.getField(lintRecord, (int)MktByPrice.Price4 - lintDisplacement).Trim()) * Math.Pow(10, DecimalPnt));

                    objBestFive.SellRateL = Convert.ToInt32(Convert.ToDouble(pobjRH.getField(lintRecord, (int)MktByPrice.Price9 - lintDisplacement).Trim()) * Math.Pow(10, DecimalPnt));

                    objBestFive.SellQtyL = Convert.ToInt32(pobjRH.getField(lintRecord, (int)MktByPrice.Quantity9 - lintDisplacement).Trim());
                    objBestFive.NoOfBidSellL = Convert.ToInt32(pobjRH.getField(lintRecord, (int)MktByPrice.NumberOfOrders9 - lintDisplacement).Trim());
                    objBestFive.NoOfBidBuyL = Convert.ToInt32(pobjRH.getField(lintRecord, (int)MktByPrice.NumberOfOrders4 - lintDisplacement).Trim());
                    s.listBestFive.Add(objBestFive);
                    //: 5th Row
                    objBestFive = new BestFive();
                    objBestFive.BuyQtyL = Convert.ToInt32(pobjRH.getField(lintRecord, (int)MktByPrice.Quantity5 - lintDisplacement).Trim());
                    objBestFive.BuyRateL = Convert.ToInt32(Convert.ToDouble(pobjRH.getField(lintRecord, (int)MktByPrice.Price5 - lintDisplacement).Trim()) * Math.Pow(10, DecimalPnt));

                    objBestFive.SellRateL = Convert.ToInt32(Convert.ToDouble(pobjRH.getField(lintRecord, (int)MktByPrice.Price10 - lintDisplacement).Trim()) * Math.Pow(10, DecimalPnt));

                    objBestFive.SellQtyL = Convert.ToInt32(pobjRH.getField(lintRecord, (int)MktByPrice.Quantity10 - lintDisplacement).Trim());
                    objBestFive.NoOfBidSellL = Convert.ToInt32(pobjRH.getField(lintRecord, (int)MktByPrice.NumberOfOrders10 - lintDisplacement).Trim());
                    objBestFive.NoOfBidBuyL = Convert.ToInt32(pobjRH.getField(lintRecord, (int)MktByPrice.NumberOfOrders5 - lintDisplacement).Trim());


                    s.listBestFive.Add(objBestFive);
                    //==========================
                    // ADDED BY UPPILI KRISHNAN
                    //==========================
                    //: 6th ROW TOTALS
                    s.totBuyQtyL = Convert.ToInt32(pobjRH.getField(lintRecord, (int)MktByPrice.TotalBuyQty - lintDisplacement).Trim());

                    s.totSellQtyL = Convert.ToInt32(pobjRH.getField(lintRecord, (int)MktByPrice.TotalSellQty - lintDisplacement).Trim());


                    //if ((mobjBSEScripDtls != null))
                    //{
                    //    if (mobjBSEScripDtls.ExchangeId == Constants.General.EX_MCX_VALUE)
                    //    {
                    s.LastUpdatedTime = pobjRH.getField(lintRecord, (int)MktByPrice.LastUpdateTime - lintDisplacement).Trim();

                    //    }

                    s.DailyPriceRange = Convert.ToString(pobjRH.getField(lintRecord, (int)MktByPrice.DailyPriceRange - lintDisplacement));
                    s.CA = pobjRH.getField(lintRecord, (int)MktByPrice.CA - lintDisplacement).Trim();
                    //if (!string.IsNullOrEmpty(lstrValue))
                    //{
                    //    lstrValue = (lstrValue.IndexOf("null") < 1 ? lstrValue : 0);
                    //    if (Information.IsNumeric(lstrValue) && Convert.ToDouble(lstrValue) == 0 && pblnAllowZero == false)
                    //    {
                    //        if (gblnDirectBroadcastConfigured == false)
                    //        {
                    //            lblBSECADisp.Text = "";
                    //        }
                    //    }
                    //    else
                    //    {
                    //        lblBSECADisp.Text = lstrValue;
                    //    }
                    //}
                    string IndicateEqPriceQuantity = pobjRH.getField(lintRecord, (int)MktByPrice.IndicativeQtyPrice - lintDisplacement).Trim();
                    string[] priceq = new string[2];
                    priceq = IndicateEqPriceQuantity.Split('@');
                    //s.IndicateEqPrice = Convert.ToInt32(Convert.ToDouble(priceq[0]));
                    //s.IndicateEqQty = Convert.ToInt32(Convert.ToDouble(priceq[1]));

                    return s;
                
            }

      

    }

        public void OpenMBPMessages(string exchange,string pstrTokenString = "")
        {
            try
            {
                System.Collections.ArrayList lstrParameters = new System.Collections.ArrayList();
                System.Collections.ArrayList lstrValues = new System.Collections.ArrayList();
                string lstrMessage = null;

                lstrMessage = GetMessageString(exchange,true, pstrTokenString);
                SettingsManager.SendMessagesOverSocket(ref lstrMessage);
            }
            catch (Exception e)
            {
            }
        }

        public string GetMessageString(string exchange,bool pblnOpen = true, string pstrTokenString = "")
        {
            return GetMBPMessageString(exchange,Enumerations.MBPMessageCalledFrom.MBP, pblnOpen, pstrTokenString);
        }



        public string GetMBPMessageString(string exchange,MBPMessageCalledFrom pobjCalledFrom, bool pblnOpen = true, string pstrTokenString = "")
        {

            string lstrMBPTokenToOpen1 = "";
            string lstrMBPTokenToOpen2 = "";
            string lstrMBPTokenToOpen = "";
            string lstrMarketId = null;
            string lstrToken = null;
            string lstrExchange = null;
            string[] lstrTempToOpen = null;
            string[] lstrDualMBPToken = null;
            string lstrMessage = "";

            if (pstrTokenString.Trim().Length > 0)
            {
                lstrDualMBPToken = pstrTokenString.Split('~');
                lstrTempToOpen = lstrDualMBPToken[0].Split('|');
                lstrMarketId = lstrTempToOpen[0];
                lstrToken = lstrTempToOpen[2];
                lstrExchange = exchange;
                // send exchange id
                lstrMBPTokenToOpen1 = lstrToken + "^" + lstrExchange + "^" + lstrMarketId;
                lstrMBPTokenToOpen = lstrMBPTokenToOpen1;
                //: for Dual
               
            }

            if (string.IsNullOrWhiteSpace(lstrMBPTokenToOpen) == false)
            {
                if (pblnOpen == true)
                {
                    lstrMessage = "OPENMBP|";
                }
                else
                {
                    lstrMessage = "CLOSEMBP|";
                }

                return lstrMessage + UtilityConnParameters.GetInstance.UserId + "|" + UtilityConnParameters.GetInstance.LoginKeyValue + "|" + lstrMBPTokenToOpen;
            }
            else
            {
                Infrastructure.Logger.WriteLog("Some how MBPTokenString  is blank in GetMessageString Function");
                return "";
            }
        }

    }
#endif


}
