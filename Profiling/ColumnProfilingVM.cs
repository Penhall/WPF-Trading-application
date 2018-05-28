using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonFrontEnd.Model.Profiling;
using System.Collections.ObjectModel;
using static CommonFrontEnd.Common.Enumerations;
using CommonFrontEnd.Common;
using CommonFrontEnd.SharedMemories;
using System.Data.SQLite;
using static CommonFrontEnd.Model.Profiling.ColumnProfilingModel;
using System.Windows.Data;
using GongSolutions.Wpf.DragDrop;
using System.Windows;
using System.Collections;
using System.Windows.Input;
using System.Text.RegularExpressions;
using CommonFrontEnd.Global;
using static CommonFrontEnd.SharedMemories.DataAccessLayer;

namespace CommonFrontEnd.ViewModel.Profiling
{
    public class ColumnProfilingVM : INotifyPropertyChanged, IDropTarget
    {
        public static Dictionary<long, ScripMasterBase> objUserDefineColProfileDict;

        #region Database fields
        public static DataAccessLayer oDataAccessLayer;
        public static SQLiteDataReader oSQLiteDataReader = null;
        public string str = string.Empty;
        public static string query = string.Empty;
        public static bool AllSelected_set = true;
        public int res = 0;
        public int chk = 0;
        #endregion

        #region list,Collection
        private BindingList<string> _cmbWindowName;
        public BindingList<string> cmbWindowName
        {
            get { return _cmbWindowName; }
            set
            {
                _cmbWindowName = value;
                NotifyPropertyChanged("cmbWindowName");
            }
        }

        private ObservableCollection<string> _cmbTempCreatedProfileNameCollection = new ObservableCollection<string>();
        public ObservableCollection<string> cmbTempCreatedProfileNameCollection
        {
            get { return _cmbTempCreatedProfileNameCollection; }
            set
            {
                _cmbTempCreatedProfileNameCollection = value;
                NotifyPropertyChanged("cmbTempCreatedProfileNameCollection");
            }
        }

        private ObservableCollection<string> _cmbCreatedProfileNameCollection = new ObservableCollection<string>();
        public ObservableCollection<string> cmbCreatedProfileNameCollection
        {
            get { return _cmbCreatedProfileNameCollection; }
            set
            {
                _cmbCreatedProfileNameCollection = value;
                NotifyPropertyChanged("cmbCreatedProfileNameCollection");
            }
        }

        private static ObservableCollection<TouchlineWindow> _lstDisplayColumns;
        public static ObservableCollection<TouchlineWindow> lstDisplayColumns
        {
            get { return _lstDisplayColumns; }
            set
            {
                _lstDisplayColumns = value;
                NotifyStaticPropertyChanged("lstDisplayColumns");
            }
        }


        private ObservableCollection<TouchlineWindow> _DuplicateCheckMemory;
        public ObservableCollection<TouchlineWindow> DuplicateCheckMemory
        {
            get { return _DuplicateCheckMemory; }
            set
            {
                _DuplicateCheckMemory = value;
                NotifyPropertyChanged("DuplicateCheckMemory");
            }
        }


        private ObservableCollection<TouchlineWindow> _lstCopyUpdateDisplayColumns = new ObservableCollection<TouchlineWindow>();
        public ObservableCollection<TouchlineWindow> lstCopyUpdateDisplayColumns
        {
            get { return _lstCopyUpdateDisplayColumns; }
            set
            {
                _lstCopyUpdateDisplayColumns = value;
                NotifyPropertyChanged("lstCopyUpdateDisplayColumns");
            }
        }

        private ObservableCollection<ColumnProfilingModel> _SaveSelectedColumnsInMemory;
        public ObservableCollection<ColumnProfilingModel> SaveSelectedColumnsInMemory
        {
            get { return _SaveSelectedColumnsInMemory; }
            set
            {
                _SaveSelectedColumnsInMemory = value;
                NotifyPropertyChanged("SaveSelectedColumnsInMemory");
            }
        }

        private static ObservableCollection<ColumnProfilingModel> _FetchedColumnsFromMemory;
        public static ObservableCollection<ColumnProfilingModel> FetchedColumnsFromMemory
        {
            get { return _FetchedColumnsFromMemory; }
            set
            {
                _FetchedColumnsFromMemory = value;
                NotifyStaticPropertyChanged("FetchedColumnsFromMemory");
            }
        }

        private static ObservableCollection<ColumnProfilingModel> _TempSaveSelectedColumnsInMemory;
        public static ObservableCollection<ColumnProfilingModel> TempSaveSelectedColumnsInMemory
        {
            get { return _TempSaveSelectedColumnsInMemory; }
            set
            {
                _TempSaveSelectedColumnsInMemory = value;
                NotifyStaticPropertyChanged("TempSaveSelectedColumnsInMemory");
            }
        }

        private static ObservableCollection<TouchlineWindow> _lstTempDisplayColumns;
        public static ObservableCollection<TouchlineWindow> lstTempDisplayColumns
        {
            get { return _lstTempDisplayColumns; }
            set
            {
                _lstTempDisplayColumns = value;
                NotifyStaticPropertyChanged("lstTempDisplayColumns");
            }
        }

        //private ObservableCollection<TouchlineWindow> _ExchangeDefaultColumns;
        //public ObservableCollection<TouchlineWindow> ExchangeDefaultColumns
        //{
        //    get { return _ExchangeDefaultColumns; }
        //    set
        //    {
        //        _ExchangeDefaultColumns = value;
        //        NotifyPropertyChanged("ExchangeDefaultColumns");
        //    }
        //}

        private static ObservableCollection<TouchlineWindow> _lstCopyTempDisplayColumns = new ObservableCollection<TouchlineWindow>();
        public static ObservableCollection<TouchlineWindow> lstCopyTempDisplayColumns
        {
            get { return _lstCopyTempDisplayColumns; }
            set
            {
                _lstCopyTempDisplayColumns = value;
                NotifyStaticPropertyChanged("lstCopyTempDisplayColumns");
            }
        }

        private ObservableCollection<TouchlineWindow> _lstUpdatedTempDisplayColumns = new ObservableCollection<TouchlineWindow>();
        public ObservableCollection<TouchlineWindow> lstUpdatedTempDisplayColumns
        {
            get { return _lstUpdatedTempDisplayColumns; }
            set
            {
                _lstUpdatedTempDisplayColumns = value;
                NotifyPropertyChanged("lstUpdatedTempDisplayColumns");
            }
        }
        #endregion

        #region Relay Commands
        private RelayCommand _RCreateCommand;
        public RelayCommand RCreateCommand
        {
            get
            {
                return _RCreateCommand ?? (_RCreateCommand = new RelayCommand(
                    (object e1) => OnChangeOfCreateButton()));
            }
        }

        private RelayCommand _RUpdateCommand;
        public RelayCommand RUpdateCommand
        {
            get
            {
                return _RUpdateCommand ?? (_RUpdateCommand = new RelayCommand(
                    (object e1) => OnChangeOfUpdateButton()));
            }
        }

        private RelayCommand _RDeleteCommand;
        public RelayCommand RDeleteCommand
        {
            get
            {
                return _RDeleteCommand ?? (_RDeleteCommand = new RelayCommand(
                    (object e1) => OnChangeOfDeleteButton()));
            }
        }


        private RelayCommand _SaveColumns;
        public RelayCommand SaveColumns
        {
            get
            {
                chk = 1;
                return _SaveColumns ?? (_SaveColumns = new RelayCommand(
                    (object e1) => OnClickOfSaveButton()));
            }
        }

        private RelayCommand _SaveAsColumns;
        public RelayCommand SaveAsColumns
        {
            get
            {
                chk = 2;
                return _SaveAsColumns ?? (_SaveAsColumns = new RelayCommand(
                    (object e1) => OnClickOfSaveAsButton()));
            }
        }

        private RelayCommand _DeleteColumns;
        public RelayCommand DeleteColumns
        {
            get
            {
                return _DeleteColumns ?? (_DeleteColumns = new RelayCommand(
                    (object e1) => OnClickOfDeleteButton()));
            }
        }

        private RelayCommand<System.Windows.Input.KeyEventArgs> _KeyDown_Event;

        public RelayCommand<System.Windows.Input.KeyEventArgs> KeyDown_Event
        {
            get
            {
                return _KeyDown_Event ?? (_KeyDown_Event = new RelayCommand<System.Windows.Input.KeyEventArgs>(KeyDown_Event_Click));
            }
        }
        #endregion

        #region Properties
        private string _IsSaveVisibleCreate;
        public string IsSaveVisibleCreate
        {
            get { return _IsSaveVisibleCreate; }
            set
            {
                _IsSaveVisibleCreate = value;
                NotifyPropertyChanged("IsSaveVisibleCreate");
            }
        }

        private string _IsSaveAsVisibleCreate;
        public string IsSaveAsVisibleCreate
        {
            get { return _IsSaveAsVisibleCreate; }
            set
            {
                _IsSaveAsVisibleCreate = value;
                NotifyPropertyChanged("IsSaveAsVisibleCreate");
            }
        }

        private static string _TouchlineVisibility;
        public static string TouchlineVisibility
        {
            get { return _TouchlineVisibility; }
            set
            {
                _TouchlineVisibility = value;
                NotifyStaticPropertyChanged("TouchlineVisibility");
            }
        }

        private static string _SelectedWindow;
        public static string SelectedWindow
        {
            get { return _SelectedWindow; }
            set
            {
                _SelectedWindow = value;
                NotifyStaticPropertyChanged("SelectedWindow");
                OnChangeOfWindowSelection();
            }
        }

        private static string _SelectedColProfile;
        public static string SelectedColProfile
        {
            get { return _SelectedColProfile; }
            set
            {
                _SelectedColProfile = value;
                NotifyStaticPropertyChanged("SelectedColProfile");
                OnChangeOfColumnProfileSelection();
            }
        }

        private static string _txtProfileName;
        public static string txtProfileName
        {
            get { return _txtProfileName; }
            set
            {
                _txtProfileName = value;
                NotifyStaticPropertyChanged("txtProfileName");
            }
        }

        private static bool _chkDefaultProfile;
        public static bool chkDefaultProfile
        {
            get { return _chkDefaultProfile; }
            set
            {
                _chkDefaultProfile = value;
                NotifyStaticPropertyChanged("chkDefaultProfile");
            }
        }

        private static bool _isCreateChecked;
        public static bool isCreateChecked
        {
            get { return _isCreateChecked; }
            set
            {
                _isCreateChecked = value;
                NotifyStaticPropertyChanged("isCreateChecked");
            }
        }

        private static bool _isUpdateChecked;
        public static bool isUpdateChecked
        {
            get { return _isUpdateChecked; }
            set
            {
                _isUpdateChecked = value;
                NotifyStaticPropertyChanged("isUpdateChecked");
            }
        }

        private static bool _isDeleteChecked;
        public static bool isDeleteChecked
        {
            get { return _isDeleteChecked; }
            set
            {
                _isDeleteChecked = value;
                NotifyStaticPropertyChanged("isDeleteChecked");
            }
        }

        private static bool _chkShowSelected;
        public static bool chkShowSelected
        {
            get { return _chkShowSelected; }
            set
            {
                _chkShowSelected = value;
                NotifyStaticPropertyChanged("chkShowSelected");
                OnChnageOfCheckboxSelection();
            }
        }

        private static bool _chkShowDeselected;
        public static bool chkShowDeselected
        {
            get { return _chkShowDeselected; }
            set
            {
                _chkShowDeselected = value;
                NotifyStaticPropertyChanged("chkShowDeselected");
                OnChnageOfCheckboxSelection();
            }
        }

        private static bool _chkShowExchangeCol;
        public static bool chkShowExchangeCol
        {
            get { return _chkShowExchangeCol; }
            set
            {
                _chkShowExchangeCol = value;
                NotifyStaticPropertyChanged("chkShowExchangeCol");
                OnChnageOfCheckboxSelection();
            }
        }

        private static bool _rdbShowAll;
        public static bool rdbShowAll
        {
            get { return _rdbShowAll; }
            set
            {
                _rdbShowAll = value;
                NotifyStaticPropertyChanged("rdbShowAll");
                OnChnageOfCheckboxSelection();
            }
        }

        private bool _IsSaveEnabled;
        public bool IsSaveEnabled
        {
            get { return _IsSaveEnabled; }
            set
            {
                _IsSaveEnabled = value;
                NotifyPropertyChanged("IsSaveEnabled");
            }
        }

        private static bool _IsSaveAsEnabled;
        public static bool IsSaveAsEnabled
        {
            get { return _IsSaveAsEnabled; }
            set
            {
                _IsSaveAsEnabled = value;
                NotifyStaticPropertyChanged("IsSaveAsEnabled");
            }
        }

        private bool _IsDeleteEnabled;
        public bool IsDeleteEnabled
        {
            get { return _IsDeleteEnabled; }
            set
            {
                _IsDeleteEnabled = value;
                NotifyPropertyChanged("IsDeleteEnabled");
            }
        }

        private static bool _isShowAllEnabled;
        public static bool isShowAllEnabled
        {
            get { return _isShowAllEnabled; }
            set
            {
                _isShowAllEnabled = value;
                NotifyStaticPropertyChanged("isShowAllEnabled");
            }
        }

        private static bool _isShowDeselectedEnabled;
        public static bool isShowDeselectedEnabled
        {
            get { return _isShowDeselectedEnabled; }
            set
            {
                _isShowDeselectedEnabled = value;
                NotifyStaticPropertyChanged("isShowDeselectedEnabled");
            }
        }

        private static bool _isShowExchangeColEnabled;
        public static bool isShowExchangeColEnabled
        {
            get { return _isShowExchangeColEnabled; }
            set
            {
                _isShowExchangeColEnabled = value;
                NotifyStaticPropertyChanged("isShowExchangeColEnabled");
            }
        }

        private static bool _isShowSelectedEnabled;
        public static bool isShowSelectedEnabled
        {
            get { return _isShowSelectedEnabled; }
            set
            {
                _isShowSelectedEnabled = value;
                NotifyStaticPropertyChanged("isShowSelectedEnabled");
            }
        }

        private bool _IsProfileNameEnabled;
        public bool IsProfileNameEnabled
        {
            get { return _IsProfileNameEnabled; }
            set
            {
                _IsProfileNameEnabled = value;
                NotifyPropertyChanged("IsProfileNameEnabled");
            }
        }

        private bool _IsDefaultProfileEnabled;
        public bool IsDefaultProfileEnabled
        {
            get { return _IsDefaultProfileEnabled; }
            set
            {
                _IsDefaultProfileEnabled = value;
                NotifyPropertyChanged("IsDefaultProfileEnabled");
            }
        }

        private static bool _chkSelectAll;
        public static bool chkSelectAll
        {
            get { return _chkSelectAll; }
            set
            {
                _chkSelectAll = value;

#if TWS || BOW
                if (isCreateChecked || isUpdateChecked)
                {
                    foreach (TouchlineWindow oTouchlineProfModel in lstDisplayColumns)
                    {
                        oTouchlineProfModel.PropertyChanged -= OnElementPropertyChanged;
                    }

                    if (AllSelected_set)
                        lstDisplayColumns.Where(x => x.IsLstColumnEnabled == true && x.DefaultColumns == 0).ToList().ForEach(x => x.LstColumnIsChecked = value);

                    foreach (TouchlineWindow oTouchLineWindow in lstDisplayColumns)
                    {
                        oTouchLineWindow.PropertyChanged += OnElementPropertyChanged;
                    }
                    AllSelected_set = true;
                    NotifyStaticPropertyChanged("chkSelectAll");
                }
                else if (isDeleteChecked)
                {
                    foreach (TouchlineWindow oTouchlineProfModel in lstDisplayColumns)
                    {
                        oTouchlineProfModel.PropertyChanged -= OnElementPropertyChanged;
                    }

                    if (AllSelected_set)
                        lstDisplayColumns.ToList().ForEach(x => x.LstColumnIsChecked = value);

                    foreach (TouchlineWindow oTouchLineWindow in lstDisplayColumns)
                    {
                        oTouchLineWindow.PropertyChanged += OnElementPropertyChanged;
                    }
                    AllSelected_set = true;
                    NotifyStaticPropertyChanged("chkSelectAll");
                }
#endif
            }
        }

        private bool _cmbTouchlineProfileName;
        public bool cmbTouchlineProfileName
        {
            get { return _cmbTouchlineProfileName; }
            set
            {
                _cmbTouchlineProfileName = value;
                NotifyPropertyChanged("cmbTouchlineProfileName");
            }
        }

        private static string _txtSearchScrip = string.Empty;

        public static string txtSearchScrip
        {
            get { return _txtSearchScrip; }
            set
            {
                _txtSearchScrip = value;
#if TWS || BOW
                ScripIdTxtChange_Click();
                NotifyStaticPropertyChanged("txtSearchScrip");
#endif
            }
        }

        #endregion

        public ColumnProfilingVM()
        {
#if TWS || BOW
            //if (oDataAccessLayer != null)
            oDataAccessLayer = new DataAccessLayer();
            oDataAccessLayer.Connect((int)DataAccessLayer.ConnectionDB.Masters);

            isCreateChecked = true;
            cmbTouchlineProfileName = false;
            rdbShowAll = true;
            chkDefaultProfile = false;
            TouchlineVisibility = "Hidden";
            PopulateWindowName();
            cmbCreatedProfileNameCollection.Add(WindowName.Exchange_Default_Profile.ToString().Replace("_", " "));
            SelectedColProfile = cmbCreatedProfileNameCollection[0];
            IsDeleteEnabled = false;
            IsSaveAsEnabled = false;
            IsSaveEnabled = true;
            isShowAllEnabled = true;
            isShowDeselectedEnabled = true;
            isShowExchangeColEnabled = true;
            isShowSelectedEnabled = true;
            IsSaveVisibleCreate = "Visible";
            IsSaveAsVisibleCreate = "Hidden";
            ChkRadioButtons();
            //  CreationOfDefaultProfile();
            IsDefaultProfileEnabled = true;
            IsProfileNameEnabled = true;
#endif
        }

        private void CreationOfDefaultProfile()
        {
            PopulateColumns();
            oDataAccessLayer.Connect((int)DataAccessLayer.ConnectionDB.Masters);
            oDataAccessLayer.OpenConnection((int)DataAccessLayer.ConnectionDB.Masters);
            try
            {
                foreach (var item in lstDisplayColumns)
                {
#if TWS
                    str = @"INSERT INTO USER_DEFINED_PROFILE(MemberID,TraderID,ScreenName,ColumnName,ProfileName,ColPriorityVal,DefProfile)
                                 VALUES( " + "'" + UtilityLoginDetails.GETInstance.MemberId + "'," + "'" + UtilityLoginDetails.GETInstance.TraderId + "'," + "'" + SelectedWindow + "'," +
                                              "'" + item.FieldName + "'," + "'DEFAULT'," + "'" + item.DefaultColumns + "'," + "'True');";
#elif BOW
                    str = @"INSERT INTO USER_DEFINED_PROFILE(MemberID,TraderID,ScreenName,ColumnName,ProfileName,ColPriorityVal,DefProfile)
                                 VALUES( " + "'" + UtilityLoginDetails.GETInstance.UserLoginId+ "'," + "'NA'," + "'" + SelectedWindow + "'," +
                                              "'" + item.FieldName + "'," + "'Default'," + "'" + item.DefaultColumns + "'," + "'True');";
#endif
                    int result = oDataAccessLayer.ExecuteNonQuery((int)ConnectionDB.Masters,str, System.Data.CommandType.Text, null);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error While Creating Default Profile", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            finally
            {
                oDataAccessLayer.CloseConnection((int)DataAccessLayer.ConnectionDB.Masters);
                System.Data.SQLite.SQLiteConnection.ClearAllPools();
            }
        }

        private static void OnChangeOfColumnProfileSelection()
        {
            chkSelectAll = false;
            if (chkShowDeselected == true || chkShowSelected == true)
            {
                PopulateColumnsWhenFileNameSelected();
                rdbShowAll = true;
            }
            TempSaveSelectedColumnsInMemory = new ObservableCollection<ColumnProfilingModel>();
            if (SelectedColProfile != WindowName.Exchange_Default_Profile.ToString().Replace("_", " ") || (String.IsNullOrEmpty(SelectedColProfile)))
                FetchColumnsFromMemory(SelectedColProfile, SelectedWindow);

            //TempSaveSelectedColumnsInMemory = SaveSelectedColumnsInMemory.Where(x => x.ColProfile == SelectedColProfile && x.ScreenName==SelectedWindow).ToList();
            if (SelectedColProfile == WindowName.Exchange_Default_Profile.ToString().Replace("_", " ") || (String.IsNullOrEmpty(SelectedColProfile)))
            {
                PopulateColumns();
                if (isUpdateChecked == true)
                {
                    if (SelectedColProfile == WindowName.Exchange_Default_Profile.ToString().Replace("_", " "))
                        txtProfileName = "DEFAULT";
                }
            }
            else
            {
                if (FetchedColumnsFromMemory.Count > 0)
                {
                    foreach (var item in FetchedColumnsFromMemory.Where(x => (x.ColProfile == SelectedColProfile) && (x.ScreenName == SelectedWindow)))
                    {
                        ColumnProfilingModel cpm = new ColumnProfilingModel();
                        cpm.ScreenName = item.ScreenName;
                        cpm.ColumnName = item.ColumnName;
                        cpm.ColPriorityValue = item.ColPriorityValue;
                        cpm.DefaultProfile = item.DefaultProfile;
                        //if (item.DefaultProfile == true)//default profile checkbox will get checked
                        //    chkDefaultProfile = true;
                        TempSaveSelectedColumnsInMemory.Add(cpm);
                    }

                    txtProfileName = FetchedColumnsFromMemory.Where(x => x.ColProfile == SelectedColProfile && (x.ScreenName == SelectedWindow)).Select(x => x.ColProfile).FirstOrDefault().ToString();
                    bool val = Convert.ToBoolean(FetchedColumnsFromMemory.Where(x => x.ColProfile == SelectedColProfile && x.DefaultProfile == true && (x.ScreenName == SelectedWindow)).Select(x => x.DefaultProfile).FirstOrDefault().ToString());
                    if (val == true)
                        chkDefaultProfile = true;
                    else
                        chkDefaultProfile = false;

                    lstDisplayColumns = new ObservableCollection<TouchlineWindow>();
                    foreach (var item in lstTempDisplayColumns.Where(x => x.ScreenName == SelectedWindow))
                    {
                        TouchlineWindow cpm = new TouchlineWindow();
                        if (TempSaveSelectedColumnsInMemory.Any(p => p.ColumnName == item.FieldName))
                        {
                            cpm.DefaultColumns = item.DefaultColumns;
                            cpm.FieldName = item.FieldName;
                            cpm.ScreenName = item.ScreenName;
                            cpm.FileName = txtProfileName.ToUpper();
                            if (cpm.DefaultColumns == 0)
                            {
                                cpm.LstColumnIsChecked = true;
                                cpm.IsLstColumnEnabled = true;
                            }
                            else if (cpm.DefaultColumns == 1)
                            {
                                cpm.LstColumnIsChecked = true;
                                cpm.IsLstColumnEnabled = true;
                            }
                            else if (cpm.DefaultColumns == 2)
                            {
                                cpm.LstColumnIsChecked = true;
                                cpm.IsLstColumnEnabled = false;
                            }
                        }
                        else
                        {
                            cpm.DefaultColumns = item.DefaultColumns;
                            cpm.FieldName = item.FieldName;
                            cpm.ScreenName = item.ScreenName;
                            cpm.FileName = txtProfileName.ToUpper();
                            if (cpm.DefaultColumns == 0)
                            {
                                cpm.LstColumnIsChecked = false;
                                cpm.IsLstColumnEnabled = true;
                            }
                            else if (cpm.DefaultColumns == 1)
                            {
                                cpm.LstColumnIsChecked = false;
                                cpm.IsLstColumnEnabled = true;
                            }
                            else if (cpm.DefaultColumns == 2)
                            {
                                cpm.LstColumnIsChecked = true;
                                cpm.IsLstColumnEnabled = false;
                            }
                        }
                        lstDisplayColumns.Add(cpm);
                    }
                }
                else
                {
                    MessageBox.Show("Create Profile before Loading", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                if (SelectedColProfile == "DEFAULT")
                {
                    IsSaveAsEnabled = false;
                }
            }
            //lstCopyTempDisplayColumns.Clear();
            lstTempDisplayColumns = lstDisplayColumns;
            lstCopyTempDisplayColumns = lstDisplayColumns;
            //rdbShowAll = true;
        }

        public static void FetchColumnsFromMemory(string SelectedColProfile, string SelectedWindow)
        {
            FetchedColumnsFromMemory = new ObservableCollection<ColumnProfilingModel>();
            try
            {
                if (oDataAccessLayer == null)
                    oDataAccessLayer = new DataAccessLayer();
                oDataAccessLayer.Connect((int)DataAccessLayer.ConnectionDB.Masters);
                oDataAccessLayer.OpenConnection((int)DataAccessLayer.ConnectionDB.Masters);

#if TWS
                string strQuery = @"SELECT * FROM USER_DEFINED_PROFILE where (MemberID=" + "'" + UtilityLoginDetails.GETInstance.MemberId.ToString() + "' AND TraderID =" + "'" + UtilityLoginDetails.GETInstance.TraderId.ToString() + "' ) AND (ProfileName=" + "'" + SelectedColProfile + "' AND ScreenName=" + "'" + SelectedWindow + "');";
#elif BOW
                string strQuery = @"SELECT * FROM USER_DEFINED_PROFILE where ( MemberID=" + "'" + UtilityLoginDetails.GETInstance.UserLoginId.ToString() + "' AND TraderID = 'NA" + "' ) AND (ProfileName=" + "'" + SelectedColProfile + "' AND ScreenName=" + "'" + SelectedWindow + "');";
#endif
                oSQLiteDataReader = oDataAccessLayer.ExecuteDataReader((int)ConnectionDB.Masters,strQuery, System.Data.CommandType.Text, null);

                while (oSQLiteDataReader.Read())
                {
                    ColumnProfilingModel cpm = new ColumnProfilingModel();

                    //Profile Name
                    cpm.ColProfile = oSQLiteDataReader["ProfileName"]?.ToString().Trim();

                    //Column Name
                    cpm.ColumnName = oSQLiteDataReader["ColumnName"]?.ToString().Trim();

                    //Screen Name
                    cpm.ScreenName = oSQLiteDataReader["ScreenName"]?.ToString().Trim();

                    //Col Priority Value
                    cpm.ColPriorityValue = Convert.ToInt16(oSQLiteDataReader["ColPriorityVal"]?.ToString().Trim());

                    //Default Profile 
                    cpm.DefaultProfile = Convert.ToBoolean(oSQLiteDataReader["DefProfile"]?.ToString().Trim());

                    FetchedColumnsFromMemory.Add(cpm);
                }
            }
            catch (Exception e)
            {
                //ExceptionUtility.LogError(e);
            }
            finally
            {
                oDataAccessLayer.CloseConnection((int)DataAccessLayer.ConnectionDB.Masters);
                System.Data.SQLite.SQLiteConnection.ClearAllPools();
            }
        }

        private void AddDefaultOptionInColProfile()
        {
            ColumnProfilingModel cpm = new ColumnProfilingModel();
            cpm.ColProfile = WindowName.Exchange_Default_Profile.ToString().Replace("_", " ");
            // cmbCreatedProfileNameCollection.Add(cpm);
            SelectedColProfile = cmbCreatedProfileNameCollection[0].ToString();
        }

        /// <summary>
        ///   This function stores the selected columns in memory and update Columns list
        /// </summary>
        private void OnClickOfSaveButton()
        {
            SaveSelectedColumnsInMemory = new ObservableCollection<ColumnProfilingModel>();
            if (isCreateChecked || isUpdateChecked)
            {
                if (!string.IsNullOrEmpty(txtProfileName))
                {
                    if (chkShowDeselected == true)
                    {
                        checkDeSelectedCheck();
                        updatelist();
                        lstDisplayColumns = lstCopyUpdateDisplayColumns;
                    }

                    LoadFileNameForCheckingDuplication();

                    if (DuplicateCheckMemory.Count > 0)
                    {
                        if (DuplicateCheckMemory.Any(x => x.FieldName.ToUpper() == txtProfileName.ToUpper() && (x.ScreenName == SelectedWindow)))
                        {
                            MessageBox.Show("Duplicate File Name", "Change File Name", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                    }
                    if (txtProfileName.ToUpper() == "DEFAULT")
                    {
                        MessageBox.Show("File Name Cannot be DEFAULT", "Change File Name", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    foreach (var item in lstDisplayColumns.Where(x => x.LstColumnIsChecked == true))
                    {
                        ColumnProfilingModel cpm = new ColumnProfilingModel();
#if TWS
                        cpm.MemberID = UtilityLoginDetails.GETInstance.MemberId.ToString();
                        cpm.TraderID = UtilityLoginDetails.GETInstance.TraderId.ToString();
#elif BOW
                        cpm.MemberID = UtilityLoginDetails.GETInstance.UserLoginId.ToString();
                        cpm.TraderID = "NA";
#endif
                        cpm.FileName = txtProfileName.ToUpper();
                        cpm.ColumnName = item.FieldName;
                        cpm.ScreenName = SelectedWindow;
                        cpm.ColPriorityValue = item.DefaultColumns;
                        if (chkDefaultProfile == true)
                            cpm.DefaultProfile = true;
                        else
                            cpm.DefaultProfile = false;
                        SaveSelectedColumnsInMemory.Add(cpm);
                        res = InsertSelectedColumnsInUserProfile(cpm);
                    }

                    if (chkDefaultProfile == true)
                    {
                        int res = UpdateDefaultProfile(txtProfileName);
                    }

                    if (res > 0)
                    {
                        //  ScripProfilingVM.cmbProfileName.Add(txtProfileName);
                        chkDefaultProfile = false;
                    }
                    else
                    {
                        MessageBox.Show("Error While Saving Profile", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Enter Profile Name", "Input Missing", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                txtProfileName = string.Empty;
                isCreateChecked = true;
                PopulateColumns();
                rdbShowAll = true;
                MessageBox.Show("Profile Saved Successfully", "Save Successfull", MessageBoxButton.OK, MessageBoxImage.Information);
                if (SelectedWindow != WindowName.Touchline.ToString())
                {
                    PopulateColumns();
                }
            }
            //chk = 0;
            //TempSaveSelectedColumnsInMemory = SaveSelectedColumnsInMemory;
        }

        private void LoadFileNameForCheckingDuplication()
        {
            DuplicateCheckMemory = new ObservableCollection<TouchlineWindow>();
            try
            {
                oDataAccessLayer.Connect((int)DataAccessLayer.ConnectionDB.Masters);
                oDataAccessLayer.OpenConnection((int)DataAccessLayer.ConnectionDB.Masters);

#if TWS
                string strQuery = @"SELECT distinct(ProfileName) FROM USER_DEFINED_PROFILE where MemberID=" + "'" + UtilityLoginDetails.GETInstance.MemberId.ToString() + "' AND TraderID =" + "'" + UtilityLoginDetails.GETInstance.TraderId.ToString() + "'AND ScreenName=" + "'" + SelectedWindow + "';";
#elif BOW
                string strQuery = @"SELECT distinct(ProfileName) FROM USER_DEFINED_PROFILE where MemberID=" + "'" + UtilityLoginDetails.GETInstance.UserLoginId.ToString() + "' AND TraderID = 'NA' AND ScreenName=" + "'" + SelectedWindow + "';";
#endif
                oSQLiteDataReader = oDataAccessLayer.ExecuteDataReader((int)ConnectionDB.Masters,strQuery, System.Data.CommandType.Text, null);

                while (oSQLiteDataReader.Read())
                {
                    TouchlineWindow cpm = new TouchlineWindow();

                    //Profile Name
                    cpm.FieldName = oSQLiteDataReader["ProfileName"]?.ToString().Trim();
                    //cpm.ScreenName= oSQLiteDataReader["ScreenName"]?.ToString().Trim();
                    cpm.ScreenName = SelectedWindow;
                    cpm.IsLstColumnEnabled = true;
                    DuplicateCheckMemory.Add(cpm);
                }
            }
            catch (Exception e)
            {
                //ExceptionUtility.LogError(e);
            }
            finally
            {
                oDataAccessLayer.CloseConnection((int)DataAccessLayer.ConnectionDB.Masters);
                System.Data.SQLite.SQLiteConnection.ClearAllPools();
            }
        }

        private void OnClickOfSaveAsButton()
        {
            SaveSelectedColumnsInMemory = new ObservableCollection<ColumnProfilingModel>();
            if (isUpdateChecked)
            {
                if (!string.IsNullOrEmpty(txtProfileName))
                {
                    DeleteColumnsBeforeUpdating();
                    if (chkShowDeselected == true || chkShowSelected == true)
                    {
                        checkDeSelectedCheck();
                        updatelist();
                        //lstDisplayColumns.Clear();
                        lstDisplayColumns = lstCopyUpdateDisplayColumns;
                    }

                    if (txtProfileName.ToUpper() != SelectedColProfile.ToUpper() && chk == 1)
                    {
                        MessageBox.Show("Click on Save As button to create new profile", "Incorrect Selection", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    foreach (var item in lstDisplayColumns.Where(x => x.LstColumnIsChecked == true))
                    {
                        ColumnProfilingModel cpm = new ColumnProfilingModel();
#if TWS
                        cpm.MemberID = UtilityLoginDetails.GETInstance.MemberId.ToString();
                        cpm.TraderID = UtilityLoginDetails.GETInstance.TraderId.ToString();
#elif BOW
                        cpm.MemberID = UtilityLoginDetails.GETInstance.UserLoginId.ToString();
                        cpm.TraderID = "NA";
#endif
                        cpm.FileName = txtProfileName;
                        cpm.ColumnName = item.FieldName;
                        cpm.ScreenName = SelectedWindow;
                        cpm.ColPriorityValue = item.DefaultColumns;
                        if (chkDefaultProfile == true)
                            cpm.DefaultProfile = true;
                        else
                            cpm.DefaultProfile = false;
                        SaveSelectedColumnsInMemory.Add(cpm);
                        res = UpdateSelectedColumnsInUserProfile(cpm);
                    }

                    if (chkDefaultProfile == true)
                    {
                        int res = UpdateDefaultProfile(txtProfileName);
                    }

                    if (res > 0)
                    {
                        //  ScripProfilingVM.cmbProfileName.Add(txtProfileName);
                        chkDefaultProfile = false;
                    }
                    else
                    {
                        MessageBox.Show("Error While Updating Profile", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Enter Profile Name", "Input Missing", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                txtProfileName = string.Empty;
                isCreateChecked = true;
                rdbShowAll = true;
                MessageBox.Show("Profile Updated Successfully", "Update Successfull", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            //chk = 0;
        }

        private void updatelist()
        {
            if (chkShowDeselected == true)
            {
                lstCopyUpdateDisplayColumns.Clear();
                foreach (var item in lstCopyTempDisplayColumns.Where(x => x.ScreenName == SelectedWindow))
                {
                    TouchlineWindow cpm = new TouchlineWindow();
                    if (lstUpdatedTempDisplayColumns.Any(p => p.FieldName == item.FieldName))
                    {
                        cpm.DefaultColumns = item.DefaultColumns;
                        cpm.FieldName = item.FieldName;
                        cpm.ScreenName = item.ScreenName;
                        if (cpm.DefaultColumns == 0)
                        {
                            cpm.LstColumnIsChecked = true;
                            cpm.IsLstColumnEnabled = true;
                            cpm.DefaultColumns = 0;
                        }
                        else if (cpm.DefaultColumns == 1)
                        {
                            cpm.LstColumnIsChecked = true;
                            cpm.IsLstColumnEnabled = true;
                            cpm.DefaultColumns = 1;
                            cpm.ScreenName = SelectedWindow;
                        }
                        else if (cpm.DefaultColumns == 2)
                        {
                            cpm.LstColumnIsChecked = true;
                            cpm.IsLstColumnEnabled = false;
                            cpm.DefaultColumns = 2;
                        }
                    }
                    else
                    {
                        cpm.DefaultColumns = item.DefaultColumns;
                        cpm.FieldName = item.FieldName;

                        if (cpm.DefaultColumns == 0)
                        {
                            if (item.LstColumnIsChecked == true)
                            {
                                cpm.LstColumnIsChecked = true;
                                cpm.IsLstColumnEnabled = true;
                                cpm.DefaultColumns = 0;
                            }
                            else
                            {
                                cpm.LstColumnIsChecked = false;
                                cpm.IsLstColumnEnabled = true;
                                cpm.DefaultColumns = 0;
                            }

                        }
                        else if (cpm.DefaultColumns == 1)
                        {
                            cpm.LstColumnIsChecked = true;
                            cpm.IsLstColumnEnabled = true;
                            cpm.DefaultColumns = 1;
                        }
                        else if (cpm.DefaultColumns == 2)
                        {
                            cpm.LstColumnIsChecked = true;
                            cpm.IsLstColumnEnabled = false;
                            cpm.DefaultColumns = 2;
                        }
                    }

                    lstCopyUpdateDisplayColumns.Add(cpm);
                }
            }
            else if (chkShowSelected == true)
            {
                lstCopyUpdateDisplayColumns.Clear();
                foreach (var item in lstCopyTempDisplayColumns)
                {
                    TouchlineWindow cpm = new TouchlineWindow();
                    if (lstUpdatedTempDisplayColumns.Any(p => p.FieldName == item.FieldName))
                    {
                        cpm.DefaultColumns = item.DefaultColumns;
                        cpm.FieldName = item.FieldName;
                        cpm.ScreenName = item.ScreenName;
                        if (cpm.DefaultColumns == 0)
                        {
                            cpm.LstColumnIsChecked = false;
                            cpm.IsLstColumnEnabled = true;
                            cpm.DefaultColumns = 0;
                        }
                        else if (cpm.DefaultColumns == 1)
                        {
                            cpm.LstColumnIsChecked = false;
                            cpm.IsLstColumnEnabled = true;
                            cpm.DefaultColumns = 1;
                        }
                        else if (cpm.DefaultColumns == 2)
                        {
                            cpm.LstColumnIsChecked = true;
                            cpm.IsLstColumnEnabled = false;
                            cpm.DefaultColumns = 2;
                        }
                    }
                    else
                    {
                        cpm.DefaultColumns = item.DefaultColumns;
                        cpm.FieldName = item.FieldName;
                        cpm.ScreenName = item.ScreenName;
                        if (cpm.DefaultColumns == 0)
                        {
                            if (item.LstColumnIsChecked == true)
                            {
                                cpm.LstColumnIsChecked = true;
                                cpm.IsLstColumnEnabled = true;
                                cpm.DefaultColumns = 0;
                            }
                            else
                            {
                                cpm.LstColumnIsChecked = false;
                                cpm.IsLstColumnEnabled = true;
                                cpm.DefaultColumns = 0;
                            }

                        }
                        else if (cpm.DefaultColumns == 1)
                        {
                            cpm.LstColumnIsChecked = true;
                            cpm.IsLstColumnEnabled = true;
                            cpm.DefaultColumns = 1;
                        }
                        else if (cpm.DefaultColumns == 2)
                        {
                            cpm.LstColumnIsChecked = true;
                            cpm.IsLstColumnEnabled = false;
                            cpm.DefaultColumns = 2;
                        }
                    }
                    lstCopyUpdateDisplayColumns.Add(cpm);
                }
            }
        }

        private void checkDeSelectedCheck()
        {
            if (chkShowDeselected == true)
            {
                lstUpdatedTempDisplayColumns.Clear();
                foreach (var item in lstDisplayColumns.Where(x => (x.LstColumnIsChecked == true) && (x.ScreenName == SelectedWindow)))
                {
                    TouchlineWindow objTouchline = new TouchlineWindow();
                    objTouchline.FieldName = item.FieldName;
                    objTouchline.ScreenName = item.ScreenName;
                    if (item.DefaultColumns == 0)
                    {
                        if (item.LstColumnIsChecked == true)
                        {
                            objTouchline.LstColumnIsChecked = true;
                            objTouchline.IsLstColumnEnabled = true;
                            objTouchline.DefaultColumns = 0;
                        }
                        else
                        {
                            objTouchline.LstColumnIsChecked = false;
                            objTouchline.IsLstColumnEnabled = true;
                            objTouchline.DefaultColumns = 0;
                        }
                        //objTouchline.ScreenName = SelectedWindow;
                    }
                    else if (item.DefaultColumns == 1)
                    {
                        objTouchline.LstColumnIsChecked = true;
                        objTouchline.IsLstColumnEnabled = true;
                        objTouchline.DefaultColumns = 1;
                        //objTouchline.ScreenName = SelectedWindow;
                    }
                    else if (item.DefaultColumns == 2)
                    {
                        objTouchline.LstColumnIsChecked = true;
                        objTouchline.IsLstColumnEnabled = false;
                        objTouchline.DefaultColumns = 2;
                        //objTouchline.ScreenName = SelectedWindow;
                    }
                    lstUpdatedTempDisplayColumns.Add(objTouchline);
                }
            }
            else if (chkShowSelected == true)
            {
                lstUpdatedTempDisplayColumns.Clear();
                foreach (var item in lstDisplayColumns.Where(x => (x.LstColumnIsChecked == false) && (x.ScreenName == SelectedWindow)))
                {
                    TouchlineWindow objTouchline = new TouchlineWindow();
                    objTouchline.FieldName = item.FieldName;
                    if (item.DefaultColumns == 0)
                    {
                        objTouchline.LstColumnIsChecked = false;
                        objTouchline.IsLstColumnEnabled = true;
                        objTouchline.DefaultColumns = 0;
                        objTouchline.ScreenName = SelectedWindow;
                    }
                    else if (item.DefaultColumns == 1)
                    {
                        objTouchline.LstColumnIsChecked = false;
                        objTouchline.IsLstColumnEnabled = true;
                        objTouchline.DefaultColumns = 1;
                        objTouchline.ScreenName = SelectedWindow;
                    }
                    else if (item.DefaultColumns == 2)
                    {
                        objTouchline.LstColumnIsChecked = true;
                        objTouchline.IsLstColumnEnabled = false;
                        objTouchline.DefaultColumns = 2;
                        objTouchline.ScreenName = SelectedWindow;
                    }
                    lstUpdatedTempDisplayColumns.Add(objTouchline);
                }
            }
        }

        private void OnClickOfDeleteButton()
        {
            int count1, count2;
            count1 = lstTempDisplayColumns.Count;

            List<string> DeleteFileName = lstTempDisplayColumns.Where(x => x.LstColumnIsChecked == true).Select(y => y.FieldName).ToList();
            int result = 0;
            oDataAccessLayer.Connect((int)DataAccessLayer.ConnectionDB.Masters);
            oDataAccessLayer.OpenConnection((int)DataAccessLayer.ConnectionDB.Masters);
            try
            {
#if TWS
                for (int i = 0; i < DeleteFileName.Count; i++)
                {

                    str = @"DELETE FROM USER_DEFINED_PROFILE WHERE MemberID = '" + UtilityLoginDetails.GETInstance.MemberId + "' AND TraderID = ' " + UtilityLoginDetails.GETInstance.TraderId + "' AND ProfileName = '" + DeleteFileName[i] + "'AND ScreenName='" + SelectedWindow + "';";
#elif BOW
                    str = @"DELETE FROM USER_DEFINED_PROFILE WHERE MemberID = '" + UtilityLoginDetails.GETInstance.UserLoginId + "' AND TraderID = 'NA' AND ProfileName = '" + DeleteFileName[i] + "'AND ScreenName=+'"+SelectedWindow+"';";
#endif
                    result = oDataAccessLayer.ExecuteNonQuery((int)ConnectionDB.Masters,str, System.Data.CommandType.Text, null);


                    var index = lstDisplayColumns.IndexOf(lstDisplayColumns.Where(x => x.FieldName == DeleteFileName[i].ToString()).FirstOrDefault());
                    lstDisplayColumns.RemoveAt(index);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error While Deleting Profile", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            finally
            {
                oDataAccessLayer.CloseConnection((int)DataAccessLayer.ConnectionDB.Masters);
                System.Data.SQLite.SQLiteConnection.ClearAllPools();
            }

            count2 = lstTempDisplayColumns.Count;
            if (count1 - count2 > 0)
            {
                var lastCount = count1 - count2;
                MessageBox.Show(lastCount + " Profiles Deleted", "Deleted", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

        }


        private int UpdateDefaultProfile(string fileName)
        {
            try
            {
                oDataAccessLayer.Connect((int)DataAccessLayer.ConnectionDB.Masters);
                oDataAccessLayer.OpenConnection((int)DataAccessLayer.ConnectionDB.Masters);

#if TWS
                str = @"UPDATE USER_DEFINED_PROFILE SET  DefProfile = 'false' where ProfileName!= " + "'" + fileName.ToUpper() +
                        "' AND ( MemberID =" + "'" + UtilityLoginDetails.GETInstance.MemberId + "' AND TraderID=" + "'" + UtilityLoginDetails.GETInstance.TraderId + "') AND ScreenName=" + "'" + SelectedWindow + "';";
#elif BOW
                str = @"UPDATE USER_DEFINED_PROFILE SET  DefProfile = 'false' where ProfileName!= " + "'" + fileName +
                       "' AND ( MemberID =" + "'" + UtilityLoginDetails.GETInstance.UserLoginId.ToString() + "' AND TraderID='NA') AND ScreenName=" + "'" + SelectedWindow + "';";
#endif

                int result = oDataAccessLayer.ExecuteNonQuery((int)ConnectionDB.Masters,str, System.Data.CommandType.Text, null);
                return result;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error While Making Profile Default", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return 0;
            }
            finally
            {
                oDataAccessLayer.CloseConnection((int)DataAccessLayer.ConnectionDB.Masters);
                System.Data.SQLite.SQLiteConnection.ClearAllPools();
            }
        }


        private int InsertSelectedColumnsInUserProfile(ColumnProfilingModel cpmodel)
        {
            //oDataAccessLayer.Dispose();
            //oDataAccessLayer.DisposeSQLite();
            try
            {
                oDataAccessLayer.Connect((int)DataAccessLayer.ConnectionDB.Masters);
                oDataAccessLayer.OpenConnection((int)DataAccessLayer.ConnectionDB.Masters);

                str = @"INSERT INTO USER_DEFINED_PROFILE(MemberID,TraderID,ScreenName,ColumnName,ProfileName,ColPriorityVal,DefProfile)
                                 VALUES( " + "'" + cpmodel.MemberID + "'," + "'" + cpmodel.TraderID + "'," + "'" + cpmodel.ScreenName + "'," +
                                          "'" + cpmodel.ColumnName + "'," + "'" + cpmodel.FileName.ToUpper() + "'," + "'" + cpmodel.ColPriorityValue + "'," + "'" + cpmodel.DefaultProfile + "');";
                int result = oDataAccessLayer.ExecuteNonQuery((int)ConnectionDB.Masters,str, System.Data.CommandType.Text, null);
                return result;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error While Saving Profile", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                ExceptionUtility.LogError(ex);
                return 0;
            }
            finally
            {
                oDataAccessLayer.CloseConnection((int)DataAccessLayer.ConnectionDB.Masters);
                System.Data.SQLite.SQLiteConnection.ClearAllPools();
            }
        }

        private int UpdateSelectedColumnsInUserProfile(ColumnProfilingModel cpmodel)
        {
            try
            {
                oDataAccessLayer.Connect((int)DataAccessLayer.ConnectionDB.Masters);
                oDataAccessLayer.OpenConnection((int)DataAccessLayer.ConnectionDB.Masters);


                str = @"INSERT INTO USER_DEFINED_PROFILE (MemberID,TraderID,ScreenName,ColumnName,ProfileName,ColPriorityVal,DefProfile)
                       VALUES (" + "'" + cpmodel.MemberID + "'," + "'" + cpmodel.TraderID + "'," + "'" + cpmodel.ScreenName + "'," + "'" + cpmodel.ColumnName + "',"
                       + "'" + cpmodel.FileName.ToUpper() + "'," + "'" + cpmodel.ColPriorityValue + "'," + "'" + cpmodel.DefaultProfile + "');";

                int result = oDataAccessLayer.ExecuteNonQuery((int)ConnectionDB.Masters,str, System.Data.CommandType.Text, null);
                return result;
                //str = @"UPDATE USER_DEFINED_PROFILE SET MemberID = '" + cpmodel.MemberID + "',TraderID = '" + cpmodel.TraderID + "',ScreenName = '"
                //    + cpmodel.ScreenName + "',ColumnName = '" + cpmodel.ColumnName + "',ProfileName = '" + cpmodel.FileName + "',ColPriorityVal = '" + cpmodel.ColPriorityValue
                //    + "',DefProfile = '" + cpmodel.DefaultProfile + "' WHERE MemberID = '" + UtilityLoginDetails.GETInstance.MemberId
                //    + "' AND TraderID = '" + UtilityLoginDetails.GETInstance.TraderId + "' AND ScreenName = '" + SelectedWindow + "' AND ProfileName = '" + txtProfileName + "'";

                //str = @"UPDATE USER_DEFINED_PROFILE SET MemberID = '" + cpmodel.MemberID + "',TraderID = '" + cpmodel.TraderID + "',ScreenName = '"
                //    + cpmodel.ScreenName + "',ColumnName = '" + cpmodel.ColumnName + "',ProfileName = '" + cpmodel.FileName + "',ColPriorityVal = '" + cpmodel.ColPriorityValue
                //    + "',DefProfile = '" + cpmodel.DefaultProfile + "' WHERE MemberID = '" + UtilityLoginDetails.GETInstance.UserLoginId
                //    + "' AND TraderID = 'NA' AND ScreenName = '" + SelectedWindow + "' AND ProfileName = '" + txtProfileName + "'";

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error While Saving Profile", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return 0;
            }
            finally
            {
                oDataAccessLayer.CloseConnection((int)DataAccessLayer.ConnectionDB.Masters);
                System.Data.SQLite.SQLiteConnection.ClearAllPools();
            }
        }

        private void DeleteColumnsBeforeUpdating()
        {
            int result = 0;
            try
            {
                oDataAccessLayer.Connect((int)DataAccessLayer.ConnectionDB.Masters);
                oDataAccessLayer.OpenConnection((int)DataAccessLayer.ConnectionDB.Masters);

#if TWS
                str = @"DELETE FROM USER_DEFINED_PROFILE WHERE MemberID = '" + UtilityLoginDetails.GETInstance.MemberId + "' AND TraderID = ' " + UtilityLoginDetails.GETInstance.TraderId + "' AND ProfileName = '" + txtProfileName + "'AND ScreenName=+'" + SelectedWindow + "';";
#elif BOW
                str = @"DELETE FROM USER_DEFINED_PROFILE WHERE MemberID = '" + UtilityLoginDetails.GETInstance.UserLoginId + "' AND TraderID = 'NA' AND ProfileName = '" + txtProfileName + "'AND ScreenName=+'"+SelectedWindow+"';";
#endif
                result = oDataAccessLayer.ExecuteNonQuery((int)ConnectionDB.Masters,str, System.Data.CommandType.Text, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error While Updating Profile", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                oDataAccessLayer.CloseConnection((int)DataAccessLayer.ConnectionDB.Masters);
                System.Data.SQLite.SQLiteConnection.ClearAllPools();
            }
        }

        private static void OnChnageOfCheckboxSelection()
        {
#if TWS || BOW
            lstDisplayColumns = new ObservableCollection<TouchlineWindow>();
            lstTempDisplayColumns = new ObservableCollection<TouchlineWindow>();
            //if (SelectedWindow == WindowName.Touchline.ToString())
            //{
            #region Touchline
            if (isCreateChecked || isUpdateChecked)
            {
                ChkRadioButtons();

                if (chkShowSelected == true)
                {
                    lstDisplayColumns = new ObservableCollection<TouchlineWindow>();
                    txtSearchScrip = string.Empty;
                    //chkShowSelected = true;
                    foreach (var item in lstCopyTempDisplayColumns.Where(x => ((x.DefaultColumns == 1) || (x.DefaultColumns == 2) || (x.DefaultColumns == 0)) && (x.ScreenName == SelectedWindow)))
                    {
                        if (item.LstColumnIsChecked == true)
                        {
                            TouchlineWindow objTouchline = new TouchlineWindow();
                            objTouchline.FieldName = item.FieldName;
                            if (item.DefaultColumns == 1)
                            {
                                objTouchline.LstColumnIsChecked = true;
                                objTouchline.IsLstColumnEnabled = true;
                                objTouchline.DefaultColumns = 1;
                                objTouchline.ScreenName = SelectedWindow;
                            }
                            else if (item.DefaultColumns == 2)
                            {
                                objTouchline.LstColumnIsChecked = true;
                                objTouchline.IsLstColumnEnabled = false;
                                objTouchline.DefaultColumns = 2;
                                objTouchline.ScreenName = SelectedWindow;
                            }
                            else if (item.DefaultColumns == 0)
                            {
                                if (item.LstColumnIsChecked == true)
                                {
                                    objTouchline.LstColumnIsChecked = true;
                                    objTouchline.IsLstColumnEnabled = true;
                                    objTouchline.DefaultColumns = 0;
                                    objTouchline.ScreenName = SelectedWindow;
                                }
                                else
                                {
                                    objTouchline.LstColumnIsChecked = false;
                                    objTouchline.IsLstColumnEnabled = false;
                                    objTouchline.DefaultColumns = 0;
                                    objTouchline.ScreenName = SelectedWindow;
                                }
                            }
                            lstDisplayColumns.Add(objTouchline);
                        }
                    }
                }
                else if (chkShowDeselected == true)
                {
                    lstDisplayColumns = new ObservableCollection<TouchlineWindow>();
                    txtSearchScrip = string.Empty;
                    //chkShowDeselected = true;
                    foreach (var item in lstCopyTempDisplayColumns.Where(x => (x.DefaultColumns == 0 || x.DefaultColumns == 1) && (x.ScreenName == SelectedWindow)))
                    {
                        if (item.LstColumnIsChecked == false)
                        {
                            TouchlineWindow objTouchline = new TouchlineWindow();
                            objTouchline.FieldName = item.FieldName;
                            objTouchline.LstColumnIsChecked = false;
                            objTouchline.IsLstColumnEnabled = true;
                            objTouchline.ScreenName = SelectedWindow;
                            if (item.DefaultColumns == 1)
                                objTouchline.DefaultColumns = 1;
                            else if (item.DefaultColumns == 0)
                                objTouchline.DefaultColumns = 0;
                            lstDisplayColumns.Add(objTouchline);
                        }
                    }

                }
                else if (rdbShowAll == true)
                {
                    lstDisplayColumns = new ObservableCollection<TouchlineWindow>();
                    txtSearchScrip = string.Empty;
                    //rdbShowAll = true;
                    if (!isUpdateChecked)
                    {
                        foreach (var item in lstCopyTempDisplayColumns.Where(x => x.ScreenName == SelectedWindow))
                        {
                            TouchlineWindow objTouchline = new TouchlineWindow();
                            objTouchline.FieldName = item.FieldName;
                            if (item.DefaultColumns == 0)
                            {
                                objTouchline.LstColumnIsChecked = false;
                                objTouchline.IsLstColumnEnabled = true;
                                objTouchline.DefaultColumns = 0;
                                objTouchline.ScreenName = SelectedWindow;
                            }
                            else if (item.DefaultColumns == 1)
                            {
                                objTouchline.LstColumnIsChecked = true;
                                objTouchline.IsLstColumnEnabled = true;
                                objTouchline.DefaultColumns = 1;
                                objTouchline.ScreenName = SelectedWindow;
                            }
                            else if (item.DefaultColumns == 2)
                            {
                                objTouchline.LstColumnIsChecked = true;
                                objTouchline.IsLstColumnEnabled = false;
                                objTouchline.DefaultColumns = 2;
                                objTouchline.ScreenName = SelectedWindow;
                            }
                            lstDisplayColumns.Add(objTouchline);
                        }
                    }
                    else
                    {
                        if (SelectedColProfile == WindowName.Exchange_Default_Profile.ToString().Replace("_", " ") || SelectedColProfile == "DEFAULT")
                        {
                            foreach (var item in lstCopyTempDisplayColumns.Where(x => x.ScreenName == SelectedWindow))
                            {
                                TouchlineWindow objTouchline = new TouchlineWindow();
                                objTouchline.FieldName = item.FieldName;
                                if (item.DefaultColumns == 0)
                                {
                                    objTouchline.LstColumnIsChecked = false;
                                    objTouchline.IsLstColumnEnabled = true;
                                    objTouchline.DefaultColumns = 0;
                                    objTouchline.ScreenName = SelectedWindow;
                                }
                                else if (item.DefaultColumns == 1)
                                {
                                    objTouchline.LstColumnIsChecked = true;
                                    objTouchline.IsLstColumnEnabled = true;
                                    objTouchline.DefaultColumns = 1;
                                    objTouchline.ScreenName = SelectedWindow;
                                }
                                else if (item.DefaultColumns == 2)
                                {
                                    objTouchline.LstColumnIsChecked = true;
                                    objTouchline.IsLstColumnEnabled = false;
                                    objTouchline.DefaultColumns = 2;
                                    objTouchline.ScreenName = SelectedWindow;
                                }
                                lstDisplayColumns.Add(objTouchline);
                            }
                        }
                        else
                        {
                            foreach (var item in lstCopyTempDisplayColumns.Where(x => x.ScreenName == SelectedWindow && x.FileName == SelectedColProfile))
                            {
                                TouchlineWindow objTouchline = new TouchlineWindow();
                                objTouchline.FieldName = item.FieldName;
                                if (item.DefaultColumns == 0)
                                {
                                    if (item.LstColumnIsChecked == true)
                                    {
                                        objTouchline.LstColumnIsChecked = true;
                                        objTouchline.IsLstColumnEnabled = true;
                                        objTouchline.DefaultColumns = 0;
                                        objTouchline.ScreenName = SelectedWindow;
                                    }
                                    else if (item.LstColumnIsChecked == false)
                                    {
                                        objTouchline.LstColumnIsChecked = false;
                                        objTouchline.IsLstColumnEnabled = true;
                                        objTouchline.DefaultColumns = 0;
                                        objTouchline.ScreenName = SelectedWindow;
                                    }
                                }
                                else if (item.DefaultColumns == 1)
                                {
                                    if (item.LstColumnIsChecked == true)
                                    {
                                        objTouchline.LstColumnIsChecked = true;
                                        objTouchline.IsLstColumnEnabled = true;
                                        objTouchline.DefaultColumns = 1;
                                        objTouchline.ScreenName = SelectedWindow;
                                    }
                                    else if (item.LstColumnIsChecked == false)
                                    {
                                        objTouchline.LstColumnIsChecked = false;
                                        objTouchline.IsLstColumnEnabled = true;
                                        objTouchline.DefaultColumns = 1;
                                        objTouchline.ScreenName = SelectedWindow;
                                    }
                                }
                                else if (item.DefaultColumns == 2)
                                {
                                    if (item.LstColumnIsChecked == true)
                                    {
                                        objTouchline.LstColumnIsChecked = true;
                                        objTouchline.IsLstColumnEnabled = false;
                                        objTouchline.DefaultColumns = 2;
                                        objTouchline.ScreenName = SelectedWindow;
                                    }

                                }
                                lstDisplayColumns.Add(objTouchline);
                            }
                        }
                    }
                }

                else if (chkShowExchangeCol == true)
                {
                    // chkShowExchangeCol = true;
                    lstDisplayColumns = new ObservableCollection<TouchlineWindow>();
                    foreach (var item in lstCopyTempDisplayColumns.Where(x => (x.DefaultColumns == 2) && (x.ScreenName == SelectedWindow)))
                    {
                        TouchlineWindow objTouchline = new TouchlineWindow();
                        objTouchline.FieldName = item.FieldName;
                        objTouchline.LstColumnIsChecked = true;
                        objTouchline.IsLstColumnEnabled = false;
                        objTouchline.ScreenName = SelectedWindow;
                        lstDisplayColumns.Add(objTouchline);
                    }
                }
                #endregion
            }
            else if (isDeleteChecked)
            {
                ChkRadioButtons();
            }
            //}
            lstTempDisplayColumns = lstDisplayColumns;
        }

        private static void ScripIdTxtChange_Click()
        {
            try
            {
                // bool validate = Validate();
                //if (validate)
                //{
                var _itemSourceList = new CollectionViewSource() { Source = lstTempDisplayColumns };
                // ICollectionView the View/UI part 
                ICollectionView Itemlist = _itemSourceList.View;

                if (!string.IsNullOrEmpty(txtSearchScrip))
                {
                    var yourCostumFilter = new Predicate<object>(item => ((TouchlineWindow)item).FieldName.ToLower().StartsWith(txtSearchScrip.Trim().ToLower()));
                    Itemlist.Filter = yourCostumFilter;
                }
                else if (string.IsNullOrEmpty(txtSearchScrip))
                {
                    Itemlist.Filter = o =>
                    {
                        TouchlineWindow p = o as TouchlineWindow;
                        return !string.IsNullOrEmpty(p.FieldName);
                    };
                }

                //now we add our Filter
                var l = Itemlist.Cast<TouchlineWindow>().ToList();
                lstDisplayColumns = new ObservableCollection<TouchlineWindow>(l);
                // }
                //else
                //{
                //    MessageBox.Show("Enter only Alphabets", "Incorrect Input", MessageBoxButton.OK, MessageBoxImage.Information);
                //    return;
                //}
            }
            catch (Exception ex)
            {
                return;
            }

            finally
            {
                //if (ObjTouchlineDataCollection.Count == SearchTemplist.Count && string.IsNullOrEmpty(txtScripCode) && string.IsNullOrEmpty(txtScripID))
                //    TitleTouchLine = "TouchLine - " + CurrentTabSlected + " - " + ObjTouchlineDataCollection.Count;
                //else
                //    TitleTouchLine = "TouchLine - " + CurrentTabSlected + " - " + ObjTouchlineDataCollection.Count + " of " + SearchTemplist.Count;

                //NotifyStaticPropertyChanged("ObjTouchlineDataCollection");
            }
#endif
        }

        private bool Validate()
        {
            var val = txtSearchScrip;

            if (val.Length > 0)
            {
                if (!char.IsDigit(Convert.ToChar(val)) || !char.IsNumber(Convert.ToChar(val)))
                {
                    return true;
                }
                else
                    return false;
            }
            else
                return true;
        }

        private static void PopulateColumns()
        {
            lstDisplayColumns = new ObservableCollection<TouchlineWindow>();
            foreach (var item in MasterSharedMemory.objTouchlineCollection.Where(x => x.ScreenName == SelectedWindow).ToList())
            {
                TouchlineWindow objTouchline = new TouchlineWindow();
                objTouchline.FieldName = item.FieldName;
                objTouchline.ScreenName = item.ScreenName;
                if (item.DefaultColumns == 0)
                {
                    objTouchline.LstColumnIsChecked = false;
                    objTouchline.IsLstColumnEnabled = true;
                    objTouchline.DefaultColumns = 0;
                }
                else if (item.DefaultColumns == 1)
                {
                    objTouchline.LstColumnIsChecked = true;
                    objTouchline.IsLstColumnEnabled = true;
                    objTouchline.DefaultColumns = 1;
                }
                else if (item.DefaultColumns == 2)
                {
                    objTouchline.LstColumnIsChecked = true;
                    objTouchline.IsLstColumnEnabled = false;
                    objTouchline.DefaultColumns = 2;
                }
                lstDisplayColumns.Add(objTouchline);
            }
            lstTempDisplayColumns = lstDisplayColumns;
            lstCopyTempDisplayColumns = lstDisplayColumns;

            foreach (TouchlineWindow oTouchLineWindow in lstDisplayColumns)
            {
                oTouchLineWindow.PropertyChanged -= OnElementPropertyChanged;
            }
        }

        private static void PopulateColumnsWhenFileNameSelected()
        {
            lstDisplayColumns = new ObservableCollection<TouchlineWindow>();
            foreach (var item in MasterSharedMemory.objTouchlineCollection.Where(x => x.ScreenName == SelectedWindow).ToList())
            {
                TouchlineWindow objTouchline = new TouchlineWindow();
                objTouchline.FieldName = item.FieldName;
                objTouchline.ScreenName = item.ScreenName;
                if (item.DefaultColumns == 0)
                {
                    objTouchline.LstColumnIsChecked = false;
                    objTouchline.IsLstColumnEnabled = true;
                    objTouchline.DefaultColumns = 0;
                    objTouchline.FileName = SelectedColProfile;
                }
                else if (item.DefaultColumns == 1)
                {
                    objTouchline.LstColumnIsChecked = true;
                    objTouchline.IsLstColumnEnabled = true;
                    objTouchline.DefaultColumns = 1;
                    objTouchline.FileName = SelectedColProfile;
                }
                else if (item.DefaultColumns == 2)
                {
                    objTouchline.LstColumnIsChecked = true;
                    objTouchline.IsLstColumnEnabled = false;
                    objTouchline.DefaultColumns = 2;
                    objTouchline.FileName = SelectedColProfile;
                }
                lstDisplayColumns.Add(objTouchline);
            }
            lstTempDisplayColumns = lstDisplayColumns;
            lstCopyTempDisplayColumns = lstDisplayColumns;

            foreach (TouchlineWindow oTouchLineWindow in lstDisplayColumns)
            {
                oTouchLineWindow.PropertyChanged -= OnElementPropertyChanged;
            }
        }

        private static void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "LstColumnIsChecked")
            {
                if (lstDisplayColumns.All(el => el.LstColumnIsChecked))
                {
                    AllSelected_set = false;
                    chkSelectAll = true;
                }
                if (lstDisplayColumns.Any(el => !el.LstColumnIsChecked))
                {
                    AllSelected_set = false;
                    chkSelectAll = false;
                }
            }
        }

        private void KeyDown_Event_Click(System.Windows.Input.KeyEventArgs e)
        {

        }
        private void PopulateWindowName()
        {
#if TWS || BOW
            cmbWindowName = new BindingList<String>();

            //SelectedWindow = WindowName.Trade.ToString();
            //cmbWindowName.Add(WindowName.Touchline.ToString());
            //cmbWindowName.Add(WindowName.Trade.ToString());
            try
            {
                oDataAccessLayer.Connect((int)DataAccessLayer.ConnectionDB.Masters);
                oDataAccessLayer.OpenConnection((int)DataAccessLayer.ConnectionDB.Masters);
                string strQuery = @"SELECT ScreenName FROM CFE_WINDOW_LIST";
                SQLiteDataReader oSQLiteDataReader = oDataAccessLayer.ExecuteDataReader((int)ConnectionDB.Masters,strQuery, System.Data.CommandType.Text, null);

                while (oSQLiteDataReader.Read())
                {
                    if (!String.IsNullOrEmpty(oSQLiteDataReader["ScreenName"]?.ToString().Trim()))
                        cmbWindowName.Add(oSQLiteDataReader["ScreenName"]?.ToString().Trim());
                }
                //cmbWindowName.Sort();
                SelectedWindow = cmbWindowName[0];
            }
            catch (Exception e)
            {
                //ExceptionUtility.LogError(e);
            }
            finally
            {
                oDataAccessLayer.CloseConnection((int)DataAccessLayer.ConnectionDB.Masters);
                System.Data.SQLite.SQLiteConnection.ClearAllPools();
            }
#endif
        }

        private static void OnChangeOfWindowSelection()
        {
#if TWS || BOW
            if (SelectedWindow == WindowName.Touchline.ToString())
            {
                TouchlineVisibility = "Visible";
                PopulateColumns();
                isCreateChecked = true;
                isUpdateChecked = false;
                isDeleteChecked = false;
                chkSelectAll = false;
            }
            else
            {
                TouchlineVisibility = "Visible";
                //TouchlineVisibility = "Hidden";
            }
            if (SelectedWindow == WindowName.Pending_Order.ToString().Replace("_", " "))
            {
                // TouchlineVisibility = "Hidden";
                TouchlineVisibility = "Visible";
                PopulateColumns();
                isCreateChecked = true;
                isUpdateChecked = false;
                isDeleteChecked = false;
                chkSelectAll = false;
            }
#endif
        }


        private void OnChangeOfDeleteButton()
        {
            SelectedColProfile = WindowName.Exchange_Default_Profile.ToString().Replace("_", " ");
            cmbTouchlineProfileName = false;
            IsSaveEnabled = false;
            IsSaveAsVisibleCreate = "Hidden";
            IsSaveVisibleCreate = "Visible";
            IsSaveAsEnabled = false;
            IsDeleteEnabled = true;
            lstDisplayColumns = new ObservableCollection<TouchlineWindow>();
            ChkRadioButtons();
            IsDefaultProfileEnabled = false;
            IsProfileNameEnabled = false;
            txtProfileName = string.Empty;
            chkSelectAll = false;
            try
            {
                oDataAccessLayer.Connect((int)DataAccessLayer.ConnectionDB.Masters);
                oDataAccessLayer.OpenConnection((int)DataAccessLayer.ConnectionDB.Masters);

#if TWS
                string strQuery = @"SELECT distinct(ProfileName) FROM USER_DEFINED_PROFILE where MemberID=" + "'" + UtilityLoginDetails.GETInstance.MemberId.ToString() + "' AND TraderID =" + "'" + UtilityLoginDetails.GETInstance.TraderId.ToString() + "' AND ScreenName='" + SelectedWindow + "';";
#elif BOW
                string strQuery = @"SELECT distinct(ProfileName) FROM USER_DEFINED_PROFILE where MemberID=" + "'" + UtilityLoginDetails.GETInstance.UserLoginId.ToString() + "' AND TraderID = 'NA' AND ScreenName=+'" + SelectedWindow + "';";
#endif
                oSQLiteDataReader = oDataAccessLayer.ExecuteDataReader((int)ConnectionDB.Masters,strQuery, System.Data.CommandType.Text, null);

                while (oSQLiteDataReader.Read())
                {
                    TouchlineWindow cpm = new TouchlineWindow();

                    //Profile Name
                    cpm.FieldName = oSQLiteDataReader["ProfileName"]?.ToString().Trim();
                    if (cpm.FieldName.Equals("DEFAULT"))
                        cpm.IsLstColumnEnabled = false;
                    else
                        cpm.IsLstColumnEnabled = true;
                    lstDisplayColumns.Add(cpm);
                }
                lstDisplayColumns.Distinct();

                if (lstDisplayColumns.Contains(lstDisplayColumns.Where(i => i.FieldName == "DEFAULT").FirstOrDefault()))
                    lstDisplayColumns.Remove(lstDisplayColumns.Where(i => i.FieldName == "DEFAULT").Single());
                lstTempDisplayColumns = lstDisplayColumns;


                foreach (TouchlineWindow oTouchLineWindow in lstDisplayColumns)
                {
                    oTouchLineWindow.PropertyChanged += OnElementPropertyChanged;
                }

            }
            catch (Exception e)
            {
                //ExceptionUtility.LogError(e);
            }
            finally
            {
                oDataAccessLayer.CloseConnection((int)DataAccessLayer.ConnectionDB.Masters);
                System.Data.SQLite.SQLiteConnection.ClearAllPools();
            }
        }

        private void OnChangeOfUpdateButton()
        {

            cmbTouchlineProfileName = true;
            cmbCreatedProfileNameCollection.Clear();
            cmbCreatedProfileNameCollection.Add(WindowName.Exchange_Default_Profile.ToString().Replace("_", " "));
            SelectedColProfile = cmbCreatedProfileNameCollection[0];
            IsSaveEnabled = true;
            IsSaveAsEnabled = true;
            IsDeleteEnabled = false;
            IsSaveVisibleCreate = "Hidden";
            IsSaveAsVisibleCreate = "Visible";
            rdbShowAll = true;
            ChkRadioButtons();
            IsDefaultProfileEnabled = true;
            IsProfileNameEnabled = true;
            chkSelectAll = false;
            try
            {
                oDataAccessLayer.Connect((int)DataAccessLayer.ConnectionDB.Masters);
                oDataAccessLayer.OpenConnection((int)DataAccessLayer.ConnectionDB.Masters);
#if TWS
                string strQuery = @"SELECT distinct(ProfileName) FROM USER_DEFINED_PROFILE where MemberID=" + "'" + UtilityLoginDetails.GETInstance.MemberId.ToString() + "' AND TraderID =" + "'" + UtilityLoginDetails.GETInstance.TraderId.ToString() + "' AND ScreenName=+'" + SelectedWindow + "';";
#elif BOW
                string strQuery = @"SELECT distinct(ProfileName) FROM USER_DEFINED_PROFILE where MemberID=" + "'" + UtilityLoginDetails.GETInstance.UserLoginId.ToString() + "' AND TraderID = 'NA' AND ScreenName=+'"+SelectedWindow+"';";
#endif
                oSQLiteDataReader = oDataAccessLayer.ExecuteDataReader((int)ConnectionDB.Masters,strQuery, System.Data.CommandType.Text, null);

                while (oSQLiteDataReader.Read())
                {
                    ColumnProfilingModel cpm = new ColumnProfilingModel();

                    //Profile Name
                    cpm.ColProfile = oSQLiteDataReader["ProfileName"]?.ToString().Trim();

                    cmbCreatedProfileNameCollection.Add(cpm.ColProfile.ToString());
                }
                cmbCreatedProfileNameCollection.Distinct();

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

        private static void ChkRadioButtons()
        {
            if (isUpdateChecked)
            {

                isShowAllEnabled = true;
                isShowDeselectedEnabled = true;
                isShowExchangeColEnabled = true;
                isShowSelectedEnabled = true;
            }
            else if (isCreateChecked || isDeleteChecked)
            {

                isShowAllEnabled = false;
                isShowDeselectedEnabled = false;
                isShowExchangeColEnabled = false;
                isShowSelectedEnabled = false;
            }
        }

        private void OnChangeOfCreateButton()
        {
            cmbTouchlineProfileName = false;
            SelectedColProfile = WindowName.Exchange_Default_Profile.ToString().Replace("_", " ");
            txtProfileName = String.Empty;
            IsSaveEnabled = true;
            IsSaveAsEnabled = false;
            IsSaveAsVisibleCreate = "Hidden";
            IsSaveVisibleCreate = "Visible";
            IsDeleteEnabled = false;
            ChkRadioButtons();
            IsDefaultProfileEnabled = true;
            IsProfileNameEnabled = true;
            txtProfileName = string.Empty;
            chkSelectAll = false;
        }

        //private void GenericTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        //{
        //    e.Handled = !IsTextAllowed(e.Text, @"[^a-zA-Z]");
        //}

        //private static bool IsTextAllowed(string Text, string AllowedRegex)
        //{
        //    try
        //    {
        //        var regex = new Regex(AllowedRegex);
        //        return !regex.IsMatch(Text);
        //    }
        //    catch
        //    {
        //        return true;
        //    }
        //}

        //use of dll
        #region Drag and Drop 
        void IDropTarget.DragOver(DropInfo dropInfo)
        {
            if (dropInfo.Data is TouchlineWindow)
            {
                dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
                dropInfo.Effects = DragDropEffects.Move;
            }
        }

        void IDropTarget.Drop(DropInfo dropInfo)
        {

            TouchlineWindow tldata = (TouchlineWindow)dropInfo.Data;
            ((IList)dropInfo.DragInfo.SourceCollection).Remove(tldata);
        }
        #endregion

        #region Notify Properties
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(String propertyName = "")
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
}
