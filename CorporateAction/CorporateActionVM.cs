

using CommonFrontEnd.Common;
using CommonFrontEnd.Global;
using CommonFrontEnd.Model;
using CommonFrontEnd.Model.CorporateAction;
using CommonFrontEnd.SharedMemories;
using CommonFrontEnd.View.CorporateAction;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SQLite;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Threading;
using static CommonFrontEnd.SharedMemories.DataAccessLayer;

namespace CommonFrontEnd.ViewModel.CorporateAction
{
    class CorporateActionVM : INotifyPropertyChanged
    {
        public SQLiteDataReader oSQLiteDataReader = null;
        public string strQuery = string.Empty;
        public string CurrentDate = string.Empty;
        public string prevSettlementDate = string.Empty;
        public string nextSettlementDate = string.Empty;
        public string TempNextSettlementDate1 = string.Empty;
        public string TempNextSettlementDate2 = string.Empty;
        public string settlementNo = string.Empty;
        public string finalDate = string.Empty;
        private readonly object lockObject = new object();
        public Dispatcher CallingThreadDispatcher { get; set; }
        public DateTime CDate;
        public int res = 0;
        public int cnt = 0;
        public DateTime EDate, PDate;
        public string pastDate = string.Empty;
        public string result = string.Empty;
        public DataAccessLayer oDataAccessLayer;
        //public static Dictionary<string, CorporateActionModel> objCorpActBSE;

        #region Collections
        private ObservableCollection<CorporateActionModel> _objCorporateActionDataCollection;
        public ObservableCollection<CorporateActionModel> ObjCorporateActionDataCollection
        {
            get { return _objCorporateActionDataCollection; }
            set
            {
                _objCorporateActionDataCollection = value;
            }
        }

        private ObservableCollection<CorporateActionModel> _TempCorporateActionDataCollection;
        public ObservableCollection<CorporateActionModel> TempCorporateActionDataCollection
        {
            get { return _TempCorporateActionDataCollection; }
            set
            {
                _TempCorporateActionDataCollection = value;
            }
        }
        #endregion

        #region Properties
        private List<string> _corpSegment;
        public List<string> corpSegment
        {
            get { return _corpSegment; }
            set { _corpSegment = value; }
        }

        private string _corpSegmentSelected;
        public string corpSegmentSelected
        {
            get { return _corpSegmentSelected; }
            set
            {
                _corpSegmentSelected = value;
                OnChangeOfCorpSegmentSelected();
            }
        }

        private List<string> _CorpPurpose;
        public List<string> CorpPurpose
        {
            get { return _CorpPurpose; }
            set { _CorpPurpose = value; }
        }

        private string _CorpPurposeSelected;
        public string CorpPurposeSelected
        {
            get { return _CorpPurposeSelected; }
            set
            {
                _CorpPurposeSelected = value;
                NotifyPropertyChanged("CorpPurposeSelected");
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

        private string _Grp = string.Empty;
        public string Grp
        {
            get { return _Grp; }
            set
            {
                _Grp = value;
                NotifyPropertyChanged("Grp");
                ScripIdTxtChange_Click();
            }
        }

        private int _LeftPosition = 71;

        public int LeftPosition
        {
            get { return _LeftPosition; }
            set { _LeftPosition = value; NotifyPropertyChanged("LeftPosition"); }
        }

        private int _TopPosition = 17;

        public int TopPosition
        {
            get { return _TopPosition; }
            set { _TopPosition = value; NotifyPropertyChanged("TopPosition"); }
        }

        private int _Width = 732;

        public int CorpActWidth
        {
            get { return _Width; }
            set { _Width = value; NotifyPropertyChanged("CorpActWidth"); }
        }


        private int _Height = 574;

        public int CorpActHeight
        {
            get { return _Height; }
            set { _Height = value; NotifyPropertyChanged("CorpActHeight"); }
        }

        private List<SharedMemories.CorporateAction> _ScripList;
        public List<SharedMemories.CorporateAction> ScripList
        {
            get { return _ScripList; }
            set { _ScripList = value; }
        }

        private string _TitleCorporate;
        public string TitleCorporate
        {
            get { return _TitleCorporate; }
            set { _TitleCorporate = value; NotifyPropertyChanged("TitleCorporate"); }
        }

        private bool _IsPurposeEnable;
        public bool IsPurposeEnable
        {
            get { return _IsPurposeEnable; }
            set { _IsPurposeEnable = value; NotifyPropertyChanged("IsPurposeEnable"); }
        }

        private bool _IsPastDatesChk;
        public bool IsPastDatesChk
        {
            get { return _IsPastDatesChk; }
            set
            {
                _IsPastDatesChk = value;
                OnChangeOfChk();
            }
        }

        #endregion

        #region Dictionary and Relay Commands

        private Dictionary<string, CorporateActionModel> _TempobjCorpActBSE;

        public Dictionary<string, CorporateActionModel> TempobjCorpActBSE
        {
            get { return _TempobjCorpActBSE; }
            set { _TempobjCorpActBSE = value; }
        }


        private Dictionary<string, CorporateActionModel> _objCorpActBSE;

        public Dictionary<string, CorporateActionModel> objCorpActBSE
        {
            get { return _objCorpActBSE; }
            set { _objCorpActBSE = value; }
        }


        private Dictionary<string, CorporateActionModel> _objExDatePresentDateCorpActBSE;

        public Dictionary<string, CorporateActionModel> objExDatePresentDateCorpActBSE
        {
            get { return _objExDatePresentDateCorpActBSE; }
            set { _objExDatePresentDateCorpActBSE = value; }
        }

        private Dictionary<string, CorporateActionModel> _objPastExDateCorpActBSE;

        public Dictionary<string, CorporateActionModel> objPastExDateCorpActBSE
        {
            get { return _objPastExDateCorpActBSE; }
            set { _objPastExDateCorpActBSE = value; }
        }

        private Dictionary<string, CorporateActionModel> _objCumExDateCorpActBSE;

        public Dictionary<string, CorporateActionModel> objCumExDateCorpActBSE
        {
            get { return _objCumExDateCorpActBSE; }
            set { _objCumExDateCorpActBSE = value; }
        }

        private RelayCommand _SearchButtonClick;
        public RelayCommand SearchButtonClick
        {
            get
            {

                return _SearchButtonClick ?? (_SearchButtonClick = new RelayCommand(
                    (object e) => PopulatingCAGrid()
                        ));
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

        #endregion

        public CorporateActionVM()
        {
            oDataAccessLayer = new DataAccessLayer();
            oDataAccessLayer.Connect((int)DataAccessLayer.ConnectionDB.Masters);
            if (CommonFrontEnd.SharedMemories.ConfigurationMasterMemory.ConfigurationDict.ContainsKey("WindowsPosition"))
            {
                CommonFrontEnd.Model.BoltAppSettingsWindowsPosition oBoltAppSettingsWindowsPosition = new Model.BoltAppSettingsWindowsPosition();
                oBoltAppSettingsWindowsPosition = (CommonFrontEnd.Model.BoltAppSettingsWindowsPosition)CommonFrontEnd.SharedMemories.ConfigurationMasterMemory.ConfigurationDict["WindowsPosition"];
                if (oBoltAppSettingsWindowsPosition != null && oBoltAppSettingsWindowsPosition.ScrpHelpCorpAction != null && oBoltAppSettingsWindowsPosition.ScrpHelpCorpAction.WNDPOSITION != null)
                {
                    CorpActHeight = oBoltAppSettingsWindowsPosition.ScrpHelpCorpAction.WNDPOSITION.Down;
                    TopPosition = oBoltAppSettingsWindowsPosition.ScrpHelpCorpAction.WNDPOSITION.Top;
                    LeftPosition = oBoltAppSettingsWindowsPosition.ScrpHelpCorpAction.WNDPOSITION.Left;
                    CorpActWidth = oBoltAppSettingsWindowsPosition.ScrpHelpCorpAction.WNDPOSITION.Right;
                }
            }

            //TODO Open Corporate Action before login
            try
            {

                oDataAccessLayer.OpenConnection((int)DataAccessLayer.ConnectionDB.Masters);
                IsPurposeEnable = true;
                ObjCorporateActionDataCollection = new ObservableCollection<CorporateActionModel>();

                // CDate = CommonFunctions.GetDate();//after successfull login
                //TODO add date before login and after login

                //Cannot use Memory Manager for now as it has been commented till date 08/01/2018
                #region changes with Memory Manager
                //if (MemoryManager.CDate != null) 01-01-0001
                //    CurrentDate = MemoryManager.CDate.ToString("dd-MM-yyyy");
                //else
                //{
                //    MemoryManager.CDate = DateTime.Now.Date;
                //    CurrentDate = MemoryManager.CDate.ToString("dd-MM-yyyy");
                //}

                CurrentDate = System.DateTime.Today.ToString("dd-MM-yyyy");
                pastDate = System.DateTime.Today.AddDays(-10).ToString("dd-MM-yyyy");
                //pastDate = MemoryManager.CDate.AddDays(-10).ToString("dd-MM-yyyy");
                #endregion

                #region Changes without Memory Manager
                //if (CDate != null)
                //    CurrentDate = CDate.ToString("dd-MM-yyyy");
                //else
                //{
                //    CDate = DateTime.Now.Date;
                //    CurrentDate = CDate.ToString("dd-MM-yyyy");
                //}

                //pastDate = CDate.AddDays(-10).ToString("dd-MM-yyyy");
                #endregion

                DateTime exDate;
                corpSegment = new List<string>();//static data
                CorpPurpose = new List<string>();//static data
                ScripList = new List<SharedMemories.CorporateAction>();
                objCorpActBSE = new Dictionary<string, CorporateActionModel>();
                TempobjCorpActBSE = new Dictionary<string, CorporateActionModel>();
                TempCorporateActionDataCollection = new ObservableCollection<CorporateActionModel>();
                objExDatePresentDateCorpActBSE = new Dictionary<string, CorporateActionModel>();
                objPastExDateCorpActBSE = new Dictionary<string, CorporateActionModel>();
                objCumExDateCorpActBSE = new Dictionary<string, CorporateActionModel>();

                #region CAMainMemory
                PopulateCAMainMemory();
                #endregion

                #region Past Days Memory
                PopulatePastMemory();
                #endregion

                #region Present Ex Date Memory
                PopulateExDatePresentDateMemory();
                #endregion

                #region Store Cum Date Memory
                ReadSettlementMaster();
                if (!string.IsNullOrEmpty(finalDate))
                {
                    PopulateTodayCumDateMemory();
                }
                #endregion

                PopulatingCorporateActionSegment();
                PopulatingCorporateActionPurpose();
                populatingGrid();
                TitleCorporate = "Detailed Corporate Action - Count: " + ObjCorporateActionDataCollection.Count;
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
            }
            finally
            {
                oDataAccessLayer.CloseConnection((int)DataAccessLayer.ConnectionDB.Masters);
            }
        }


        private void EscapeUsingUserControl(object e)
        {
            View.CorporateAction.CorporateAction ca = e as View.CorporateAction.CorporateAction;
            ca?.Hide();
            Windows_CorporarteActionLocationChanged(e);

        }

        private void Windows_CorporarteActionLocationChanged(object e)
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

        private void ScripIdTxtChange_Click()
        {
            try
            {
                // Collection which will take your ObservableCollection
                var _itemSourceList = new CollectionViewSource() { Source = TempCorporateActionDataCollection.OrderBy(x => x.scripCode) };

                // ICollectionView the View/UI part 
                ICollectionView Itemlist = _itemSourceList.View;
                //   executeFilterAction(new Action(() =>
                //   {
                if (!string.IsNullOrEmpty(ScripId.Trim()) && (!string.IsNullOrEmpty(ScripCode.Trim()) && (!string.IsNullOrEmpty(Grp.Trim()))))
                {
                    var yourCostumFilter = new Predicate<object>(item => ((CorporateActionModel)item).ScripID.Trim().ToLower().StartsWith(ScripId.Trim().ToLower()) && ((CorporateActionModel)item).scripCode.ToString().StartsWith(ScripCode.Trim()) && ((CorporateActionModel)item).Group.Trim().ToLower().StartsWith(Grp.Trim().ToLower()));
                    Itemlist.Filter = yourCostumFilter;
                }
                else if (string.IsNullOrEmpty(ScripCode.Trim()))
                {
                    var yourCostumFilter = new Predicate<object>(item => ((CorporateActionModel)item).ScripID.Trim().ToLower().StartsWith(ScripId.Trim().ToLower()) && ((CorporateActionModel)item).Group.Trim().ToLower().StartsWith(Grp.Trim().ToLower()));
                    Itemlist.Filter = yourCostumFilter;
                }
                else if (string.IsNullOrEmpty(ScripId.Trim()))
                {
                    var yourCostumFilter = new Predicate<object>(item => ((CorporateActionModel)item).scripCode.ToString().StartsWith(ScripCode.Trim()) && ((CorporateActionModel)item).Group.Trim().ToLower().StartsWith(Grp.Trim().ToLower()));
                    Itemlist.Filter = yourCostumFilter;
                }
                else if (string.IsNullOrEmpty(Grp.Trim()))
                {
                    var yourCostumFilter = new Predicate<object>(item => ((CorporateActionModel)item).ScripID.Trim().ToLower().StartsWith(ScripId.Trim().ToLower()) && ((CorporateActionModel)item).scripCode.ToString().StartsWith(ScripCode.Trim()));
                    Itemlist.Filter = yourCostumFilter;
                }
                else if (!string.IsNullOrEmpty(Grp.Trim()))
                {
                    var yourCostumFilter = new Predicate<object>(item => ((CorporateActionModel)item).Group.Trim().ToLower().StartsWith(Grp.Trim()));
                    Itemlist.Filter = yourCostumFilter;
                }
                //now we add our Filter

                var l = Itemlist.Cast<CorporateActionModel>().ToList();

                ObjCorporateActionDataCollection = new ObservableCollection<CorporateActionModel>(l);
                NotifyPropertyChanged("ObjCorporateActionDataCollection");
                //l.Clear();

                // NotifyStaticPropertyChanged("ObjTouchlineDataCollection");
                //  }));

            }
            catch (Exception)
            {
                return;
            }
            finally
            {
                if (ObjCorporateActionDataCollection.Count == TempCorporateActionDataCollection.Count && string.IsNullOrEmpty(ScripId) && string.IsNullOrEmpty(ScripCode))
                    TitleCorporate = string.Format("Detailed Corporate Action - Count: {0}", ObjCorporateActionDataCollection.Count);
                else
                    TitleCorporate = string.Format("Detailed Corporate Action - Count: {0} of {1}", ObjCorporateActionDataCollection.Count, TempCorporateActionDataCollection.Count);
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

        private void ClearFilterButton_Click()
        {
            ScripId = string.Empty;
            ScripCode = string.Empty;
            Grp = string.Empty;
        }

        private void OnChangeOfCorpSegmentSelected()
        {
            ClearFilterButton_Click();
        }

        private void ReadSettlementMaster()
        {
            int counter_index = 0;
            var length = MasterSharedMemory.listSetlMas.Count;
            for (int index = 0; index < length; index++)
            {
                int res = CommonFunctions.CompareDate(MasterSharedMemory.listSetlMas[index].Field3, CurrentDate);
                if (res == 0)
                {
                    if (MasterSharedMemory.listSetlMas[index].Field1 == "DR")
                    {
                        settlementNo = MasterSharedMemory.listSetlMas[index].Field2;
                        counter_index = index;
                    }
                }
            }

            if (!string.IsNullOrEmpty(settlementNo))
            {
                if (counter_index != 0)
                {
                    nextSettlementDate = MasterSharedMemory.listSetlMas.Where(x => (x.Field2 == settlementNo) && (x.Field1.Equals("RA"))).Select(x => x.Field3).FirstOrDefault();
                    string s1 = settlementNo.Substring(0, 3);
                    string s2 = settlementNo.Substring(3, 5);
                    int prevDate = Convert.ToInt32(s1) - 1;
                    string prev = prevDate.ToString("000");
                    string prevSttlementNo = string.Concat(prev, s2);//string.Concat(Convert.ToString(prev, s2));
                    if (!string.IsNullOrEmpty(prevSttlementNo))
                        prevSettlementDate = MasterSharedMemory.listSetlMas.Where(x => (x.Field2 == prevSttlementNo) && (x.Field1.Equals("RA"))).Select(x => x.Field3).FirstOrDefault();

                    res = CommonFunctions.CompareDate(prevSettlementDate, nextSettlementDate);
                    if (res == -1 || res == 3 || res == 6 || res == 0)//added 0 becoz prev settlement date and next settlemnet date was same for feb month for date 20
                        finalDate = prevSettlementDate;

                }
                else
                {
                    //TempNextSettlementDate1 = MasterSharedMemory.listSetlMas.Where(x => (x.Field2 == settlementNo) && (x.Field1.Equals("RA"))).Select(x => x.Field3).FirstOrDefault();
                    //res = CommonFunctions.CompareDate(TempNextSettlementDate1, CurrentDate);
                    //if (res == 1 || res == 2 || res == 5)
                    //    nextSettlementDate = TempNextSettlementDate1;

                    string s1 = settlementNo.Substring(0, 3);
                    string s2 = settlementNo.Substring(3, 5);
                    int nextDate = Convert.ToInt32(s1) + 1;
                    string nextSttlementNo = string.Concat(Convert.ToString(nextDate), s2);

                    if (!string.IsNullOrEmpty(nextSttlementNo))
                        finalDate = MasterSharedMemory.listSetlMas.Where(x => (x.Field2 == nextSttlementNo) && (x.Field1.Equals("DR"))).Select(x => x.Field3).FirstOrDefault();
                    //if (!string.IsNullOrEmpty(nextSttlementNo))
                    //    TempNextSettlementDate2 = MasterSharedMemory.listSetlMas.Where(x => (x.Field2 == nextSttlementNo) && (x.Field1.Equals("DR"))).Select(x => x.Field3).FirstOrDefault();


                    //res = CommonFunctions.CompareDate(TempNextSettlementDate1, TempNextSettlementDate2);
                    //if (res == 1 || res == 2 || res == 5)
                    //{
                    //    int res1 = CommonFunctions.CompareDate(TempNextSettlementDate2, CurrentDate);
                    //    if (res == 1 || res == 2 || res == 5)
                    //        finalDate = TempNextSettlementDate2;
                    //}
                }
            }
            else
            {
                MessageBox.Show("Download latest Settlement Master", "Warning");
                return;
            }
        }

        #region Cum Date Memory
        private void PopulateTodayCumDateMemory()
        {
            //nextSettlementDate = MasterSharedMemory.listSetlMas.Where(x=>(CommonFunctions.CompareDate(x.Field3,CurrentDate))==0).Select(y=>y.)
            oDataAccessLayer.OpenConnection((int)DataAccessLayer.ConnectionDB.Masters);//TBD store only those records whose diff is only 10days
            try
            {
                strQuery = @"SELECT * FROM BSE_EQ_CORPACT_CFE where ((ScripCode BETWEEN 500000 AND 600000) OR (ScripCode>=700000));";

                oSQLiteDataReader = oDataAccessLayer.ExecuteDataReader((int)ConnectionDB.Masters,strQuery, System.Data.CommandType.Text, null);

                while (oSQLiteDataReader.Read())
                {
                    CorporateActionModel objCorpAct = new CorporateActionModel();

                    var ScripCode = oSQLiteDataReader["ScripCode"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(ScripCode))
                        objCorpAct.scripCode = Convert.ToInt64(ScripCode);

                    var BookClosureFrom = oSQLiteDataReader["BookClosureFrom"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(BookClosureFrom))
                        objCorpAct.bookClosureFrom = Convert.ToDateTime(BookClosureFrom).ToString("dd-MM-yyyy");

                    var BookClosureTo = oSQLiteDataReader["BookClosureTo"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(BookClosureTo))
                        objCorpAct.bookClosureTo = Convert.ToDateTime(BookClosureTo).ToString("dd-MM-yyyy");

                    var PurposeOrEvent = oSQLiteDataReader["PurposeOrEvent"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(PurposeOrEvent))
                        objCorpAct.purposeOrEvent = PurposeOrEvent;

                    var BcOrRdFlag = oSQLiteDataReader["BcOrRdFlag"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(BcOrRdFlag))
                        objCorpAct.bcOrRdFlag = BcOrRdFlag;

                    var ExDate = oSQLiteDataReader["ExDate"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(ExDate))
                        objCorpAct.exDate = Convert.ToDateTime(ExDate).ToString("yyyy-MM-dd");
                    #region Change
                    //objCorpAct.exDate = Convert.ToDateTime(ExDate).ToString("dd-MM-yyyy");
                    #endregion
                    if (BcOrRdFlag.Equals("RD") && string.IsNullOrEmpty(ExDate) && !string.IsNullOrEmpty(BookClosureFrom))
                    {
                        objCorpAct.exDate = Convert.ToDateTime(BookClosureFrom).ToString("yyyy-MM-dd");
                        objCorpAct.recordDate = Convert.ToDateTime(BookClosureFrom).ToString("yyyy-MM-dd");
                    }
                    if (BcOrRdFlag.Equals("RD") && !string.IsNullOrEmpty(ExDate))
                        objCorpAct.recordDate = Convert.ToDateTime(BookClosureFrom).ToString("yyyy-MM-dd");
                    if (BcOrRdFlag.Equals("BC") && string.IsNullOrEmpty(ExDate) && !string.IsNullOrEmpty(BookClosureFrom))
                        objCorpAct.exDate = Convert.ToDateTime(BookClosureFrom).ToString("yyyy-MM-dd");

                    var ScripName = oSQLiteDataReader["ScripName"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(ScripName))
                        objCorpAct.scripName = ScripName;

                    var ApplicableFor = oSQLiteDataReader["ApplicableFor"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(ApplicableFor))
                        objCorpAct.applicableFor = ApplicableFor;

                    var NDStartSetlNumber = oSQLiteDataReader["NDStartSetlNumber"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(NDStartSetlNumber))
                        objCorpAct.NDStartSetlNumber = NDStartSetlNumber;

                    var NDEndSetlNumber = oSQLiteDataReader["NDEndSetlNumber"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(NDEndSetlNumber))
                        objCorpAct.NDEndSetlNumber = NDEndSetlNumber;

                    var DeliverySettlement = oSQLiteDataReader["DeliverySettlement"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(DeliverySettlement))
                        objCorpAct.deliverySettlement = DeliverySettlement;

                    var NdStartDate = oSQLiteDataReader["NdStartDate"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(NdStartDate))
                        objCorpAct.NdStartDate = Convert.ToDateTime(NdStartDate).ToString("dd-MM-yyyy");

                    var NdEndDate = oSQLiteDataReader["NdEndDate"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(NdEndDate))
                        objCorpAct.ndEndDate = Convert.ToDateTime(NdEndDate).ToString("dd-MM-yyyy"); ;

                    var NdOrNDAndExFlag = oSQLiteDataReader["NdOrNDAndExFlag"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(NdOrNDAndExFlag))
                        objCorpAct.NdOrNDAndExFlag = NdOrNDAndExFlag;

                    var grp = CommonFunctions.GetGroupName(objCorpAct.scripCode, Enumerations.Exchange.BSE, Enumerations.Segment.Equity);
                    if (!string.IsNullOrEmpty(grp))
                        objCorpAct.Group = grp;
                    else
                        objCorpAct.Group = "NA";

                    var scripId = CommonFunctions.GetScripId(objCorpAct.scripCode, Enumerations.Exchange.BSE, Enumerations.Segment.Equity);
                    if (!string.IsNullOrEmpty(scripId))
                        objCorpAct.ScripID = scripId;
                    else
                        objCorpAct.ScripID = "NA";

                    var key = string.Format("{0}_{1}", objCorpAct.scripCode, objCorpAct.bcOrRdFlag);
                    if (!objCumExDateCorpActBSE.ContainsKey(key))
                    {
                        res = CommonFunctions.CompareDateExDate(objCorpAct.exDate, finalDate);
                        if (res == 0)
                            objCumExDateCorpActBSE.Add(key, objCorpAct);
                    }

                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
            }
            finally
            {
                oDataAccessLayer.CloseConnection((int)DataAccessLayer.ConnectionDB.Masters);
            }
            objCumExDateCorpActBSE = objCumExDateCorpActBSE.OrderBy(x => x.Value.exDate).ToDictionary(x => x.Key, x => x.Value);
        }
        #endregion

        private void OnChangeOfChk()
        {
            ObjCorporateActionDataCollection.Clear();
            if (IsPastDatesChk)
            {
                IsPurposeEnable = false;
                CorpPurposeSelected = CorpActModel.Purpose.All.ToString();
                if (corpSegmentSelected == CorpActModel.Segment.Equity.ToString())
                {
                    //ObjCorporateActionDataCollection.Clear();
                    ObjCorporateActionDataCollection = new ObservableCollection<CorporateActionModel>();
                    foreach (var item in objPastExDateCorpActBSE.Values)//objCorpActBSE.Values.Where(x =>(!x.Group.Equals(null)) && (!x.Group.Equals(string.Empty)) && (!x.Group.Contains("F,G,FC,GC"))))
                    {
                        if (item != null)
                        {
                            if (!string.IsNullOrEmpty(item.Group) && !new[] { "F", "G", "FC", "GC" }.Any(x => x == item.Group.Trim()))
                            {
                                CorporateActionModel objCorpAct = new CorporateActionModel();
                                if (item.bcOrRdFlag == "BC")
                                {
                                    objCorpAct.ScripID = item.ScripID;
                                    objCorpAct.scripCode = item.scripCode;
                                    objCorpAct.Group = item.Group;
                                    objCorpAct.purposeOrEvent = item.purposeOrEvent;
                                    objCorpAct.exDate = DateTime.ParseExact(item?.exDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("dd-MM-yyyy");//item.exDate;
                                    objCorpAct.exDateSort = item.exDate;
                                    objCorpAct.bookClosureFrom = item.bookClosureFrom;
                                    objCorpAct.bookClosureTo = item.bookClosureTo;
                                    objCorpAct.NdStartDate = item.NdStartDate;
                                    objCorpAct.ndEndDate = item.ndEndDate;
                                }
                                else if (item.bcOrRdFlag == "RD")
                                {
                                    objCorpAct.ScripID = item.ScripID;
                                    objCorpAct.scripCode = item.scripCode;
                                    objCorpAct.Group = item.Group;
                                    objCorpAct.purposeOrEvent = item.purposeOrEvent;
                                    objCorpAct.exDate = DateTime.ParseExact(item?.exDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("dd-MM-yyyy");//item.exDate;
                                    objCorpAct.exDateSort = item.exDate;
                                    objCorpAct.recordDate = DateTime.ParseExact(item?.recordDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("dd-MM-yyyy");//item.exDate;
                                    objCorpAct.recordDateSort = item.recordDate;
                                    objCorpAct.NdStartDate = item.NdStartDate;
                                    objCorpAct.ndEndDate = item.ndEndDate;
                                }
                                ObjCorporateActionDataCollection.Add(objCorpAct);
                            }

                        }
                    }

                    ObjCorporateActionDataCollection.OrderBy(x => x.exDate);
                    TitleCorporate = "Detailed Corporate Action - Count: " + ObjCorporateActionDataCollection.Count;
                    TempCorporateActionDataCollection = ObjCorporateActionDataCollection;
                    NotifyPropertyChanged("ObjCorporateActionDataCollection");
                }
                else if (corpSegmentSelected == CorpActModel.Segment.Fixed_Income.ToString().Replace("_", " "))
                {
                    //ObjCorporateActionDataCollection.Clear();
                    ObjCorporateActionDataCollection = new ObservableCollection<CorporateActionModel>();
                    foreach (var item in objPastExDateCorpActBSE.Values)//objCorpActBSE.Values.Where(x =>(!x.Group.Equals(null)) && (!x.Group.Equals(string.Empty)) && (!x.Group.Contains("F,G,FC,GC"))))
                    {
                        if (item != null)
                        {
                            if (!string.IsNullOrEmpty(item.Group) && new[] { "F", "G", "FC", "GC" }.Any(x => x == item.Group.Trim()))
                            {
                                CorporateActionModel objCorpAct = new CorporateActionModel();
                                if (item.bcOrRdFlag == "BC")
                                {
                                    objCorpAct.ScripID = item.ScripID;
                                    objCorpAct.scripCode = item.scripCode;
                                    objCorpAct.Group = item.Group;
                                    objCorpAct.purposeOrEvent = item.purposeOrEvent;
                                    objCorpAct.exDate = DateTime.ParseExact(item?.exDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("dd-MM-yyyy");//item.exDate;
                                    objCorpAct.exDateSort = item.exDate;
                                    objCorpAct.bookClosureFrom = item.bookClosureFrom;
                                    objCorpAct.bookClosureTo = item.bookClosureTo;
                                    objCorpAct.NdStartDate = item.NdStartDate;
                                    objCorpAct.ndEndDate = item.ndEndDate;
                                }
                                else if (item.bcOrRdFlag == "RD")
                                {
                                    objCorpAct.ScripID = item.ScripID;
                                    objCorpAct.scripCode = item.scripCode;
                                    objCorpAct.Group = item.Group;
                                    objCorpAct.purposeOrEvent = item.purposeOrEvent;
                                    objCorpAct.exDate = DateTime.ParseExact(item?.exDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("dd-MM-yyyy");//item.exDate;
                                    objCorpAct.exDateSort = item.exDate;
                                    objCorpAct.recordDate = DateTime.ParseExact(item?.recordDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("dd-MM-yyyy");//item.exDate;
                                    objCorpAct.recordDateSort = item.recordDate;
                                    objCorpAct.NdStartDate = item.NdStartDate;
                                    objCorpAct.ndEndDate = item.ndEndDate;
                                }
                                ObjCorporateActionDataCollection.Add(objCorpAct);
                            }
                        }
                    }
                    ObjCorporateActionDataCollection.OrderBy(x => x.exDate);
                    TitleCorporate = "Detailed Corporate Action - Count: " + ObjCorporateActionDataCollection.Count;
                    TempCorporateActionDataCollection = ObjCorporateActionDataCollection;
                    NotifyPropertyChanged("ObjCorporateActionDataCollection");
                }
                else
                {
                    //ObjCorporateActionDataCollection.Clear();
                    ObjCorporateActionDataCollection = new ObservableCollection<CorporateActionModel>();
                    foreach (var item in objPastExDateCorpActBSE.Values)//objCorpActBSE.Values.Where(x =>(!x.Group.Equals(null)) && (!x.Group.Equals(string.Empty)) && (!x.Group.Contains("F,G,FC,GC"))))
                    {
                        if (item != null)
                        {
                            if (!string.IsNullOrEmpty(item.Group))
                            {
                                CorporateActionModel objCorpAct = new CorporateActionModel();
                                if (item.bcOrRdFlag == "BC")
                                {
                                    objCorpAct.ScripID = item.ScripID;
                                    objCorpAct.scripCode = item.scripCode;
                                    objCorpAct.Group = item.Group;
                                    objCorpAct.purposeOrEvent = item.purposeOrEvent;
                                    objCorpAct.exDate = DateTime.ParseExact(item?.exDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("dd-MM-yyyy");//item.exDate;
                                    objCorpAct.exDateSort = item.exDate;
                                    objCorpAct.bookClosureFrom = item.bookClosureFrom;
                                    objCorpAct.bookClosureTo = item.bookClosureTo;
                                    objCorpAct.NdStartDate = item.NdStartDate;
                                    objCorpAct.ndEndDate = item.ndEndDate;
                                }
                                else if (item.bcOrRdFlag == "RD")
                                {
                                    objCorpAct.ScripID = item.ScripID;
                                    objCorpAct.scripCode = item.scripCode;
                                    objCorpAct.Group = item.Group;
                                    objCorpAct.purposeOrEvent = item.purposeOrEvent;
                                    objCorpAct.exDate = DateTime.ParseExact(item?.exDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("dd-MM-yyyy");//item.exDate;
                                    objCorpAct.exDateSort = item.exDate;
                                    objCorpAct.recordDate = DateTime.ParseExact(item?.recordDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("dd-MM-yyyy");//item.exDate;
                                    objCorpAct.recordDateSort = item.recordDate;
                                    objCorpAct.NdStartDate = item.NdStartDate;
                                    objCorpAct.ndEndDate = item.ndEndDate;
                                }
                                ObjCorporateActionDataCollection.Add(objCorpAct);
                            }
                        }
                    }
                    ObjCorporateActionDataCollection.OrderBy(x => x.exDate);
                    TitleCorporate = "Detailed Corporate Action - Count: " + ObjCorporateActionDataCollection.Count;
                    TempCorporateActionDataCollection = ObjCorporateActionDataCollection;
                    NotifyPropertyChanged("ObjCorporateActionDataCollection");
                }
            }
            else
            {
                IsPurposeEnable = true;
                ObjCorporateActionDataCollection.Clear();
                corpSegmentSelected = CorpActModel.Segment.All.ToString();
                CorpPurposeSelected = CorpActModel.Purpose.All.ToString();
                populatingGrid();
            }
        }

        #region PastMemory
        private void PopulatePastMemory()
        {
            int line = 1, cnt = 1;
            objPastExDateCorpActBSE = new Dictionary<string, CorporateActionModel>();
            //TBD store only those records whose diff is only 10days
            try
            {
                oDataAccessLayer.OpenConnection((int)DataAccessLayer.ConnectionDB.Masters);
                strQuery = @"SELECT * FROM BSE_EQ_CORPACT_CFE where ((ScripCode BETWEEN 500000 AND 600000) OR (ScripCode>=700000))  order by ExDate ASC;";

                oSQLiteDataReader = oDataAccessLayer.ExecuteDataReader((int)ConnectionDB.Masters,strQuery, System.Data.CommandType.Text, null);
                //int cnt = 1;

                while (oSQLiteDataReader.Read())
                {

                    CorporateActionModel objCorpAct = new CorporateActionModel();

                    var ScripCode = oSQLiteDataReader["ScripCode"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(ScripCode))
                        objCorpAct.scripCode = Convert.ToInt64(ScripCode);

                    var BookClosureFrom = oSQLiteDataReader["BookClosureFrom"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(BookClosureFrom))
                        objCorpAct.bookClosureFrom = Convert.ToDateTime(BookClosureFrom).ToString("dd-MM-yyyy");

                    var BookClosureTo = oSQLiteDataReader["BookClosureTo"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(BookClosureTo))
                        objCorpAct.bookClosureTo = Convert.ToDateTime(BookClosureTo).ToString("dd-MM-yyyy");

                    var PurposeOrEvent = oSQLiteDataReader["PurposeOrEvent"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(PurposeOrEvent))
                        objCorpAct.purposeOrEvent = PurposeOrEvent;

                    var BcOrRdFlag = oSQLiteDataReader["BcOrRdFlag"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(BcOrRdFlag))
                        objCorpAct.bcOrRdFlag = BcOrRdFlag;

                    var ExDate = oSQLiteDataReader["ExDate"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(ExDate))
                        objCorpAct.exDate = Convert.ToDateTime(ExDate).ToString("yyyy-MM-dd");
                    #region Change
                    //objCorpAct.exDate = Convert.ToDateTime(ExDate).ToString("dd-MM-yyyy");
                    #endregion
                    if (BcOrRdFlag.Equals("RD") && string.IsNullOrEmpty(ExDate) && !string.IsNullOrEmpty(BookClosureFrom))
                    {
                        objCorpAct.exDate = Convert.ToDateTime(BookClosureFrom).ToString("yyyy-MM-dd");
                        objCorpAct.recordDate = Convert.ToDateTime(BookClosureFrom).ToString("yyyy-MM-dd");
                    }
                    if (BcOrRdFlag.Equals("RD") && !string.IsNullOrEmpty(ExDate))
                        objCorpAct.recordDate = Convert.ToDateTime(BookClosureFrom).ToString("yyyy-MM-dd");
                    if (BcOrRdFlag.Equals("BC") && string.IsNullOrEmpty(ExDate) && !string.IsNullOrEmpty(BookClosureFrom))
                        objCorpAct.exDate = Convert.ToDateTime(BookClosureFrom).ToString("yyyy-MM-dd");

                    var ScripName = oSQLiteDataReader["ScripName"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(ScripName))
                        objCorpAct.scripName = ScripName;

                    var ApplicableFor = oSQLiteDataReader["ApplicableFor"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(ApplicableFor))
                        objCorpAct.applicableFor = ApplicableFor;

                    var NDStartSetlNumber = oSQLiteDataReader["NDStartSetlNumber"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(NDStartSetlNumber))
                        objCorpAct.NDStartSetlNumber = NDStartSetlNumber;

                    var NDEndSetlNumber = oSQLiteDataReader["NDEndSetlNumber"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(NDEndSetlNumber))
                        objCorpAct.NDEndSetlNumber = NDEndSetlNumber;

                    var DeliverySettlement = oSQLiteDataReader["DeliverySettlement"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(DeliverySettlement))
                        objCorpAct.deliverySettlement = DeliverySettlement;

                    var NdStartDate = oSQLiteDataReader["NdStartDate"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(NdStartDate))
                        objCorpAct.NdStartDate = Convert.ToDateTime(NdStartDate).ToString("dd-MM-yyyy");

                    var NdEndDate = oSQLiteDataReader["NdEndDate"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(NdEndDate))
                        objCorpAct.ndEndDate = Convert.ToDateTime(NdEndDate).ToString("dd-MM-yyyy");

                    var NdOrNDAndExFlag = oSQLiteDataReader["NdOrNDAndExFlag"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(NdOrNDAndExFlag))
                        objCorpAct.NdOrNDAndExFlag = NdOrNDAndExFlag;

                    var grp = CommonFunctions.GetGroupName(objCorpAct.scripCode, Enumerations.Exchange.BSE, Enumerations.Segment.Equity);
                    if (!string.IsNullOrEmpty(grp))
                        objCorpAct.Group = grp;
                    else
                        objCorpAct.Group = "NA";

                    var scripId = CommonFunctions.GetScripId(objCorpAct.scripCode, Enumerations.Exchange.BSE, Enumerations.Segment.Equity);
                    if (!string.IsNullOrEmpty(scripId))
                        objCorpAct.ScripID = scripId;
                    else
                        objCorpAct.ScripID = "NA";

                    var key = string.Format("{0}_{1}", objCorpAct.scripCode, objCorpAct.bcOrRdFlag);

                    if (!objPastExDateCorpActBSE.ContainsKey(key))
                    {
                        if (!(string.IsNullOrEmpty(objCorpAct.exDate) && string.IsNullOrEmpty(objCorpAct.bookClosureFrom) && string.IsNullOrEmpty(objCorpAct.bookClosureTo)))
                        {
                            string expirydate = DateTime.ParseExact(objCorpAct.exDate, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture).ToString("dd-MM-yyyy");
                            var expiryDate1 = DateTime.ParseExact(expirydate, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                            var CurrentDate1 = DateTime.ParseExact(CurrentDate, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                            if (((CurrentDate1 - expiryDate1).TotalDays >= 0) && ((CurrentDate1 - expiryDate1).TotalDays <= 10))
                            {
                                objPastExDateCorpActBSE.Add(key, objCorpAct);
                                line++;
                            }
                            cnt++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Error"+line+" "+cnt);
                ExceptionUtility.LogError(ex);
            }
            finally
            {
                oDataAccessLayer.CloseConnection((int)DataAccessLayer.ConnectionDB.Masters);
            }
            objPastExDateCorpActBSE = objPastExDateCorpActBSE.OrderBy(x => x.Value.exDate).ToDictionary(x => x.Key, x => x.Value);
        }
        #endregion
        private void PopulateCAMainMemory()
        {

            #region change
            oDataAccessLayer.OpenConnection((int)DataAccessLayer.ConnectionDB.Masters);
            try
            {
                string strQuery = @"SELECT * FROM BSE_EQ_CORPACT_CFE where ((ScripCode BETWEEN 500000 AND 600000) OR (ScripCode>=700000))  order by ExDate ASC;";

                SQLiteDataReader oSQLiteDataReader = oDataAccessLayer.ExecuteDataReader((int)ConnectionDB.Masters,strQuery, System.Data.CommandType.Text, null);

                while (oSQLiteDataReader.Read())
                {
                    CorporateActionModel objCorpAct = new CorporateActionModel();

                    var ScripCode = oSQLiteDataReader["ScripCode"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(ScripCode))
                        objCorpAct.scripCode = Convert.ToInt64(ScripCode);

                    var BookClosureFrom = oSQLiteDataReader["BookClosureFrom"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(BookClosureFrom))
                        objCorpAct.bookClosureFrom = Convert.ToDateTime(BookClosureFrom).ToString("dd-MM-yyyy");
                    //objCorpAct.bookClosureFrom = DateTime.ParseExact(BookClosureFrom, @"M/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString("dd-MM-yyyy");

                    var BookClosureTo = oSQLiteDataReader["BookClosureTo"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(BookClosureTo))
                        objCorpAct.bookClosureTo = Convert.ToDateTime(BookClosureTo).ToString("dd-MM-yyyy");
                    //objCorpAct.bookClosureTo = DateTime.ParseExact(BookClosureTo, @"M/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString("dd-MM-yyyy");

                    var PurposeOrEvent = oSQLiteDataReader["PurposeOrEvent"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(PurposeOrEvent))
                        objCorpAct.purposeOrEvent = PurposeOrEvent;

                    var BcOrRdFlag = oSQLiteDataReader["BcOrRdFlag"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(BcOrRdFlag))
                        objCorpAct.bcOrRdFlag = BcOrRdFlag;

                    var ExDate = oSQLiteDataReader["ExDate"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(ExDate))
                        objCorpAct.exDate = Convert.ToDateTime(ExDate).ToString("yyyy-MM-dd");
                    #region Change Check
                    //objCorpAct.exDate = Convert.ToDateTime(ExDate).ToString("dd-MM-yyyy");
                    #endregion
                    //objCorpAct.exDate = DateTime.ParseExact(ExDate, @"M/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString("dd-MM-yyyy");
                    if (BcOrRdFlag.Equals("RD") && string.IsNullOrEmpty(ExDate) && !string.IsNullOrEmpty(BookClosureFrom))
                    {

                        objCorpAct.exDate = Convert.ToDateTime(BookClosureFrom).ToString("yyyy-MM-dd");
                        objCorpAct.recordDate = Convert.ToDateTime(BookClosureFrom).ToString("yyyy-MM-dd");
                        #region Change Check
                        //objCorpAct.exDate = Convert.ToDateTime(BookClosureFrom).ToString("dd-MM-yyyy");
                        //objCorpAct.recordDate = Convert.ToDateTime(BookClosureFrom).ToString("dd-MM-yyyy");
                        #endregion
                        //objCorpAct.exDate = DateTime.ParseExact(BookClosureFrom, @"M/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString("dd-MM-yyyy"); ;
                        //objCorpAct.recordDate = DateTime.ParseExact(BookClosureFrom, @"M/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString("dd-MM-yyyy");
                    }
                    if (BcOrRdFlag.Equals("RD") && !string.IsNullOrEmpty(ExDate))
                        objCorpAct.recordDate = Convert.ToDateTime(BookClosureFrom).ToString("yyyy-MM-dd");
                    #region Change Check
                    //objCorpAct.recordDate = Convert.ToDateTime(BookClosureFrom).ToString("dd-MM-yyyy");
                    #endregion
                    // objCorpAct.recordDate = DateTime.ParseExact(BookClosureFrom, @"M/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString("dd-MM-yyyy");
                    if (BcOrRdFlag.Equals("BC") && string.IsNullOrEmpty(ExDate) && !string.IsNullOrEmpty(BookClosureFrom))
                        objCorpAct.exDate = Convert.ToDateTime(BookClosureFrom).ToString("yyyy-MM-dd");
                    #region Change Check
                    //objCorpAct.exDate = Convert.ToDateTime(BookClosureFrom).ToString("dd-MM-yyyy");
                    #endregion
                    //objCorpAct.exDate = DateTime.ParseExact(BookClosureFrom, @"M/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString("dd-MM-yyyy");

                    var ScripName = oSQLiteDataReader["ScripName"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(ScripName))
                        objCorpAct.scripName = ScripName;

                    var ApplicableFor = oSQLiteDataReader["ApplicableFor"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(ApplicableFor))
                        objCorpAct.applicableFor = ApplicableFor;

                    var NDStartSetlNumber = oSQLiteDataReader["NDStartSetlNumber"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(NDStartSetlNumber))
                        objCorpAct.NDStartSetlNumber = NDStartSetlNumber;

                    var NDEndSetlNumber = oSQLiteDataReader["NDEndSetlNumber"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(NDEndSetlNumber))
                        objCorpAct.NDEndSetlNumber = NDEndSetlNumber;

                    var DeliverySettlement = oSQLiteDataReader["DeliverySettlement"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(DeliverySettlement))
                        objCorpAct.deliverySettlement = DeliverySettlement;

                    var NdStartDate = oSQLiteDataReader["NdStartDate"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(NdStartDate))
                        objCorpAct.NdStartDate = Convert.ToDateTime(NdStartDate).ToString("dd-MM-yyyy");
                    //objCorpAct.NdStartDate = DateTime.ParseExact(NdStartDate, @"M/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString("dd-MM-yyyy");

                    var NdEndDate = oSQLiteDataReader["NdEndDate"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(NdEndDate))
                        objCorpAct.ndEndDate = Convert.ToDateTime(NdEndDate).ToString("dd-MM-yyyy");
                    //objCorpAct.ndEndDate = DateTime.ParseExact(NdEndDate, @"M/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString("dd-MM-yyyy");

                    var NdOrNDAndExFlag = oSQLiteDataReader["NdOrNDAndExFlag"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(NdOrNDAndExFlag))
                        objCorpAct.NdOrNDAndExFlag = NdOrNDAndExFlag;

                    var grp = CommonFunctions.GetGroupName(objCorpAct.scripCode, Enumerations.Exchange.BSE, Enumerations.Segment.Equity);
                    if (!string.IsNullOrEmpty(grp))
                        objCorpAct.Group = grp;
                    else
                        objCorpAct.Group = "NA";

                    var scripId = CommonFunctions.GetScripId(objCorpAct.scripCode, Enumerations.Exchange.BSE, Enumerations.Segment.Equity);
                    if (!string.IsNullOrEmpty(scripId))
                        objCorpAct.ScripID = scripId;
                    else
                        objCorpAct.ScripID = "NA";

                    var key = string.Format("{0}_{1}", objCorpAct.scripCode, objCorpAct.bcOrRdFlag);
                    if (!objCorpActBSE.ContainsKey(key))
                    {
                        int res = CommonFunctions.CompareDate(objCorpAct.exDate, CurrentDate);
                        if (res == 0 || res == 1 || res == 2 || res == 5)
                            objCorpActBSE.Add(key, objCorpAct);

                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
            }
            finally
            {
                oDataAccessLayer.CloseConnection((int)DataAccessLayer.ConnectionDB.Masters);
            }
            #endregion
            objCorpActBSE = objCorpActBSE.OrderBy(x => x.Value.exDate).ToDictionary(x => x.Key, x => x.Value);
        }

        private void PopulateExDatePresentDateMemory()
        {
            oDataAccessLayer.OpenConnection((int)DataAccessLayer.ConnectionDB.Masters);
            try
            {
                strQuery = @"SELECT * FROM BSE_EQ_CORPACT_CFE where ((ScripCode BETWEEN 500000 AND 600000) OR (ScripCode>=700000))  order by ExDate ASC;";

                oSQLiteDataReader = oDataAccessLayer.ExecuteDataReader((int)ConnectionDB.Masters,strQuery, System.Data.CommandType.Text, null);

                while (oSQLiteDataReader.Read())
                {
                    CorporateActionModel objCorpAct = new CorporateActionModel();

                    var ScripCode = oSQLiteDataReader["ScripCode"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(ScripCode))
                        objCorpAct.scripCode = Convert.ToInt64(ScripCode);

                    var BookClosureFrom = oSQLiteDataReader["BookClosureFrom"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(BookClosureFrom))
                        objCorpAct.bookClosureFrom = Convert.ToDateTime(BookClosureFrom).ToString("dd-MM-yyyy");

                    var BookClosureTo = oSQLiteDataReader["BookClosureTo"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(BookClosureTo))
                        objCorpAct.bookClosureTo = Convert.ToDateTime(BookClosureTo).ToString("dd-MM-yyyy");

                    var PurposeOrEvent = oSQLiteDataReader["PurposeOrEvent"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(PurposeOrEvent))
                        objCorpAct.purposeOrEvent = PurposeOrEvent;

                    var BcOrRdFlag = oSQLiteDataReader["BcOrRdFlag"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(BcOrRdFlag))
                        objCorpAct.bcOrRdFlag = BcOrRdFlag;

                    var ExDate = oSQLiteDataReader["ExDate"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(ExDate))
                        objCorpAct.exDate = Convert.ToDateTime(ExDate).ToString("yyyy-MM-dd");
                    #region Change
                    //objCorpAct.exDate = Convert.ToDateTime(ExDate).ToString("dd-MM-yyyy");
                    #endregion
                    if (BcOrRdFlag.Equals("RD") && string.IsNullOrEmpty(ExDate) && !string.IsNullOrEmpty(BookClosureFrom))
                    {
                        objCorpAct.exDate = Convert.ToDateTime(BookClosureFrom).ToString("yyyy-MM-dd");
                        objCorpAct.recordDate = Convert.ToDateTime(BookClosureFrom).ToString("yyyy-MM-dd");
                    }
                    if (BcOrRdFlag.Equals("RD") && !string.IsNullOrEmpty(ExDate))
                        objCorpAct.recordDate = Convert.ToDateTime(BookClosureFrom).ToString("yyyy-MM-dd");
                    if (BcOrRdFlag.Equals("BC") && string.IsNullOrEmpty(ExDate) && !string.IsNullOrEmpty(BookClosureFrom))
                        objCorpAct.exDate = Convert.ToDateTime(BookClosureFrom).ToString("yyyy-MM-dd");

                    var ScripName = oSQLiteDataReader["ScripName"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(ScripName))
                        objCorpAct.scripName = ScripName;

                    var ApplicableFor = oSQLiteDataReader["ApplicableFor"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(ApplicableFor))
                        objCorpAct.applicableFor = ApplicableFor;

                    var NDStartSetlNumber = oSQLiteDataReader["NDStartSetlNumber"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(NDStartSetlNumber))
                        objCorpAct.NDStartSetlNumber = NDStartSetlNumber;

                    var NDEndSetlNumber = oSQLiteDataReader["NDEndSetlNumber"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(NDEndSetlNumber))
                        objCorpAct.NDEndSetlNumber = NDEndSetlNumber;

                    var DeliverySettlement = oSQLiteDataReader["DeliverySettlement"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(DeliverySettlement))
                        objCorpAct.deliverySettlement = DeliverySettlement;

                    var NdStartDate = oSQLiteDataReader["NdStartDate"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(NdStartDate))
                        objCorpAct.NdStartDate = Convert.ToDateTime(NdStartDate).ToString("dd-MM-yyyy");

                    var NdEndDate = oSQLiteDataReader["NdEndDate"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(NdEndDate))
                        objCorpAct.ndEndDate = Convert.ToDateTime(NdEndDate).ToString("dd-MM-yyyy"); ;

                    var NdOrNDAndExFlag = oSQLiteDataReader["NdOrNDAndExFlag"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(NdOrNDAndExFlag))
                        objCorpAct.NdOrNDAndExFlag = NdOrNDAndExFlag;

                    var grp = CommonFunctions.GetGroupName(objCorpAct.scripCode, Enumerations.Exchange.BSE, Enumerations.Segment.Equity);
                    if (!string.IsNullOrEmpty(grp))
                        objCorpAct.Group = grp;
                    else
                        objCorpAct.Group = "NA";

                    var scripId = CommonFunctions.GetScripId(objCorpAct.scripCode, Enumerations.Exchange.BSE, Enumerations.Segment.Equity);
                    if (!string.IsNullOrEmpty(scripId))
                        objCorpAct.ScripID = scripId;
                    else
                        objCorpAct.ScripID = "NA";

                    var key = string.Format("{0}_{1}", objCorpAct.scripCode, objCorpAct.bcOrRdFlag);
                    if (!objExDatePresentDateCorpActBSE.ContainsKey(key))
                    {
                        res = CommonFunctions.CompareDateExDate(objCorpAct.exDate, CurrentDate);
                        if (res == 0)
                            objExDatePresentDateCorpActBSE.Add(key, objCorpAct);
                    }
                }
                objExDatePresentDateCorpActBSE = objExDatePresentDateCorpActBSE.OrderBy(x => x.Value.exDate).ToDictionary(x => x.Key, x => x.Value);
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
            }
            finally
            {
                oDataAccessLayer.CloseConnection((int)DataAccessLayer.ConnectionDB.Masters);
            }
        }

        private void PopulatingCorporateActionPurpose()
        {
            CorpPurposeSelected = CorpActModel.Purpose.All.ToString();
            CorpPurpose.Add(CorpActModel.Purpose.All.ToString());
            CorpPurpose.Add(CorpActModel.Purpose.Fut_Dividend.ToString().Replace("_", " "));
            CorpPurpose.Add(CorpActModel.Purpose.Fut_Bonus.ToString().Replace("_", " "));
            CorpPurpose.Add(CorpActModel.Purpose.Fut_Interest_Payment.ToString().Replace("_", " "));
            CorpPurpose.Add(CorpActModel.Purpose.Fut_Stock_Split.ToString().Replace("_", " "));
            CorpPurpose.Add(CorpActModel.Purpose.Today_Cum_Date.ToString().Replace("_", " "));
            CorpPurpose.Add(CorpActModel.Purpose.Today_Ex_Date.ToString().Replace("_", " "));
        }

        private void PopulatingCorporateActionSegment()
        {
            corpSegmentSelected = CorpActModel.Segment.All.ToString();
            corpSegment.Add(CorpActModel.Segment.All.ToString());
            corpSegment.Add(CorpActModel.Segment.Equity.ToString());
            corpSegment.Add(CorpActModel.Segment.Fixed_Income.ToString().Replace("_", " "));
        }

        private void populatingGrid()
        {
            try
            {
                //ObjCorporateActionDataCollection.Clear();
                ObjCorporateActionDataCollection = new ObservableCollection<CorporateActionModel>();
                foreach (var item in objCorpActBSE.Values)
                {
                    CorporateActionModel objCorpAct = new CorporateActionModel();
                    if (item.bcOrRdFlag == "BC")
                    {
                        objCorpAct.ScripID = item.ScripID;
                        objCorpAct.scripCode = item.scripCode;
                        objCorpAct.Group = item.Group;
                        objCorpAct.purposeOrEvent = item.purposeOrEvent;
                        objCorpAct.exDate = DateTime.ParseExact(item?.exDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("dd-MM-yyyy");//item.exDate;
                        objCorpAct.exDateSort = item.exDate;
                        objCorpAct.bookClosureFrom = item.bookClosureFrom;
                        objCorpAct.bookClosureTo = item.bookClosureTo;
                        objCorpAct.NdStartDate = item.NdStartDate;
                        objCorpAct.ndEndDate = item.ndEndDate;
                        objCorpAct.bcOrRdFlag = item.bcOrRdFlag;
                    }
                    else if (item.bcOrRdFlag == "RD")
                    {
                        objCorpAct.ScripID = item.ScripID;
                        objCorpAct.scripCode = item.scripCode;
                        objCorpAct.Group = item.Group;
                        objCorpAct.purposeOrEvent = item.purposeOrEvent;
                        objCorpAct.exDate = DateTime.ParseExact(item?.exDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("dd-MM-yyyy");//item.exDate;
                        objCorpAct.exDateSort = item.exDate;
                        objCorpAct.recordDate = DateTime.ParseExact(item?.recordDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("dd-MM-yyyy");//item.recordDate;
                        objCorpAct.recordDateSort = item.recordDate;
                        objCorpAct.NdStartDate = item.NdStartDate;
                        objCorpAct.ndEndDate = item.ndEndDate;
                        objCorpAct.bcOrRdFlag = item.bcOrRdFlag;
                    }
                    ObjCorporateActionDataCollection.Add(objCorpAct);
                }
                // ObjCorporateActionDataCollection.OrderBy(x => x.exDate);
                ObjCorporateActionDataCollection = new ObservableCollection<CorporateActionModel>(ObjCorporateActionDataCollection.OrderBy(p => p.exDateSort));
                #region Sort
                //var sortedDatelst1 = ObjCorporateActionDataCollection.OrderBy(x => x.exDate).ToList();
                /*    var sortedDatelst = DisplayDateInSortedOrder(ObjCorporateActionDataCollection);
                    ObjCorporateActionDataCollection.Clear();
                    ObjCorporateActionDataCollection = (ObservableCollection<CorporateActionModel>)sortedDatelst;// (ObservableCollection<CorporateActionModel>)*/
                #endregion

                TitleCorporate = "Detailed Corporate Action - Count: " + ObjCorporateActionDataCollection.Count;
                TempCorporateActionDataCollection = ObjCorporateActionDataCollection;
                NotifyPropertyChanged("ObjCorporateActionDataCollection");
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
            }
        }

        private void PopulatingCAGrid()
        {
            if (UtilityLoginDetails.GETInstance.IsLoggedIN)
            {
                if (corpSegmentSelected == CorpActModel.Segment.All.ToString())
                {
                    #region All
                    string segment = "All";
                    if (IsPastDatesChk == true)
                    {
                        OnChangeOfChk();
                    }
                    else
                    {
                        if (CorpPurposeSelected == CorpActModel.Purpose.Fut_Dividend.ToString().Replace("_", " "))
                            PurposeFilter(segment, CorpActModel.Purpose.Fut_Dividend.ToString().Replace("_", " "));
                        else if (CorpPurposeSelected == CorpActModel.Purpose.Fut_Bonus.ToString().Replace("_", " "))
                            PurposeFilter(segment, CorpActModel.Purpose.Fut_Bonus.ToString().Replace("_", " "));
                        else if (CorpPurposeSelected == CorpActModel.Purpose.Fut_Interest_Payment.ToString().Replace("_", " "))
                            PurposeFilter(segment, CorpActModel.Purpose.Fut_Interest_Payment.ToString().Replace("_", " "));
                        else if (CorpPurposeSelected == CorpActModel.Purpose.Fut_Stock_Split.ToString().Replace("_", " "))
                            PurposeFilter(segment, CorpActModel.Purpose.Fut_Stock_Split.ToString().Replace("_", " "));
                        else if (CorpPurposeSelected == CorpActModel.Purpose.Today_Cum_Date.ToString().Replace("_", " "))
                            PurposeFilter(segment, CorpActModel.Purpose.Today_Cum_Date.ToString().Replace("_", " "));
                        else if (CorpPurposeSelected == CorpActModel.Purpose.Today_Ex_Date.ToString().Replace("_", " "))
                            PurposeFilter(segment, CorpActModel.Purpose.Today_Ex_Date.ToString().Replace("_", " "));
                        else
                            PurposeFilter(segment, CorpActModel.Purpose.All.ToString());
                    }

                    #endregion
                }
                else if (corpSegmentSelected == CorpActModel.Segment.Equity.ToString())
                {
                    if (IsPastDatesChk == true)
                    {
                        OnChangeOfChk();
                    }
                    else
                    {
                        string segment = "E"; ;
                        if (CorpPurposeSelected == CorpActModel.Purpose.Fut_Dividend.ToString().Replace("_", " "))
                            PurposeFilter(segment, CorpActModel.Purpose.Fut_Dividend.ToString().Replace("_", " "));
                        else if (CorpPurposeSelected == CorpActModel.Purpose.Fut_Bonus.ToString().Replace("_", " "))
                            PurposeFilter(segment, CorpActModel.Purpose.Fut_Bonus.ToString().Replace("_", " "));
                        else if (CorpPurposeSelected == CorpActModel.Purpose.Fut_Interest_Payment.ToString().Replace("_", " "))
                            PurposeFilter(segment, CorpActModel.Purpose.Fut_Interest_Payment.ToString().Replace("_", " "));
                        else if (CorpPurposeSelected == CorpActModel.Purpose.Fut_Stock_Split.ToString().Replace("_", " "))
                            PurposeFilter(segment, CorpActModel.Purpose.Fut_Stock_Split.ToString().Replace("_", " "));
                        else if (CorpPurposeSelected == CorpActModel.Purpose.Today_Cum_Date.ToString().Replace("_", " "))
                            PurposeFilter(segment, CorpActModel.Purpose.Today_Cum_Date.ToString().Replace("_", " "));
                        else if (CorpPurposeSelected == CorpActModel.Purpose.Today_Ex_Date.ToString().Replace("_", " "))
                            PurposeFilter(segment, CorpActModel.Purpose.Today_Ex_Date.ToString().Replace("_", " "));
                        else
                            PurposeFilter(segment, CorpActModel.Purpose.All.ToString());
                    }

                }
                else if (corpSegmentSelected == CorpActModel.Segment.Fixed_Income.ToString().Replace("_", " "))
                {
                    if (IsPastDatesChk == true)
                    {
                        OnChangeOfChk();
                    }
                    else
                    {
                        string segment = "D";
                        if (CorpPurposeSelected == CorpActModel.Purpose.Fut_Dividend.ToString().Replace("_", " "))
                            PurposeFilter(segment, CorpActModel.Purpose.Fut_Dividend.ToString().Replace("_", " "));
                        else if (CorpPurposeSelected == CorpActModel.Purpose.Fut_Bonus.ToString().Replace("_", " "))
                            PurposeFilter(segment, CorpActModel.Purpose.Fut_Bonus.ToString().Replace("_", " "));
                        else if (CorpPurposeSelected == CorpActModel.Purpose.Fut_Interest_Payment.ToString().Replace("_", " "))
                            PurposeFilter(segment, CorpActModel.Purpose.Fut_Interest_Payment.ToString().Replace("_", " "));
                        else if (CorpPurposeSelected == CorpActModel.Purpose.Fut_Stock_Split.ToString().Replace("_", " "))
                            PurposeFilter(segment, CorpActModel.Purpose.Fut_Stock_Split.ToString().Replace("_", " "));
                        else if (CorpPurposeSelected == CorpActModel.Purpose.Today_Cum_Date.ToString().Replace("_", " "))
                            PurposeFilter(segment, CorpActModel.Purpose.Today_Cum_Date.ToString().Replace("_", " "));
                        else if (CorpPurposeSelected == CorpActModel.Purpose.Today_Ex_Date.ToString().Replace("_", " "))
                            PurposeFilter(segment, CorpActModel.Purpose.Today_Ex_Date.ToString().Replace("_", " "));
                        else
                            PurposeFilter(segment, CorpActModel.Purpose.All.ToString());
                    }
                }
            }
            else
            {
                MessageBox.Show("Please login to view Purpose Related Reports", "Corporate Action - Warning", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
        }

        private void PurposeFilter(string segment, string Purpose)
        {
            switch (Purpose)
            {
                case "Fut Dividend":
                    {
                        if (segment == "E")
                        {
                            ObjCorporateActionDataCollection.Clear();
                            foreach (var item in objCorpActBSE.Values)//objCorpActBSE.Values.Where(x =>(!x.Group.Equals(null)) && (!x.Group.Equals(string.Empty)) && (!x.Group.Contains("F,G,FC,GC"))))
                            {
                                if (item != null)
                                {
                                    if (!string.IsNullOrEmpty(item.Group) && !new[] { "F", "G", "FC", "GC" }.Any(x => x == item.Group.Trim()))
                                    {
                                        CorporateActionModel objCorpAct = new CorporateActionModel();
                                        if (item.purposeOrEvent.Contains("DP"))
                                        {
                                            if (item.bcOrRdFlag == "BC")
                                            {
                                                objCorpAct.ScripID = item.ScripID;
                                                objCorpAct.scripCode = item.scripCode;
                                                objCorpAct.Group = item.Group;
                                                objCorpAct.purposeOrEvent = item.purposeOrEvent;
                                                // objCorpAct.exDate = item.exDate;
                                                objCorpAct.exDate = DateTime.ParseExact(item?.exDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("dd-MM-yyyy");//item.exDate;
                                                objCorpAct.exDateSort = item.exDate;
                                                objCorpAct.bookClosureFrom = item.bookClosureFrom;
                                                objCorpAct.bookClosureTo = item.bookClosureTo;
                                                objCorpAct.NdStartDate = item.NdStartDate;
                                                objCorpAct.ndEndDate = item.ndEndDate;
                                                objCorpAct.bcOrRdFlag = item.bcOrRdFlag;
                                            }
                                            else if (item.bcOrRdFlag == "RD")
                                            {
                                                objCorpAct.ScripID = item.ScripID;
                                                objCorpAct.scripCode = item.scripCode;
                                                objCorpAct.Group = item.Group;
                                                objCorpAct.purposeOrEvent = item.purposeOrEvent;
                                                // objCorpAct.exDate = item.exDate;
                                                objCorpAct.exDate = DateTime.ParseExact(item?.exDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("dd-MM-yyyy");//item.exDate;
                                                objCorpAct.exDateSort = item.exDate;
                                                objCorpAct.recordDate = DateTime.ParseExact(item?.recordDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("dd-MM-yyyy");//item.recordDate;
                                                objCorpAct.recordDateSort = item.recordDate;
                                                objCorpAct.NdStartDate = item.NdStartDate;
                                                objCorpAct.ndEndDate = item.ndEndDate;
                                                objCorpAct.bcOrRdFlag = item.bcOrRdFlag;
                                            }
                                            ObjCorporateActionDataCollection.Add(objCorpAct);
                                        }
                                    }
                                }
                            }
                            ObjCorporateActionDataCollection.OrderBy(x => x.exDate);
                            TitleCorporate = "Detailed Corporate Action - Count: " + ObjCorporateActionDataCollection.Count;
                            TempCorporateActionDataCollection = ObjCorporateActionDataCollection;
                        }
                        else if (segment == "D")
                        {
                            ObjCorporateActionDataCollection.Clear();
                            foreach (var item in objCorpActBSE.Values)//objCorpActBSE.Values.Where(x =>(!x.Group.Equals(null)) && (!x.Group.Equals(string.Empty)) && (!x.Group.Contains("F,G,FC,GC"))))
                            {
                                if (item != null)
                                {
                                    if (!string.IsNullOrEmpty(item.Group) && new[] { "F", "G", "FC", "GC" }.Any(x => x == item.Group.Trim()))
                                    {
                                        CorporateActionModel objCorpAct = new CorporateActionModel();
                                        if (item.purposeOrEvent.Contains("DP"))
                                        {
                                            if (item.bcOrRdFlag == "BC")
                                            {
                                                objCorpAct.ScripID = item.ScripID;
                                                objCorpAct.scripCode = item.scripCode;
                                                objCorpAct.Group = item.Group;
                                                objCorpAct.purposeOrEvent = item.purposeOrEvent;
                                                //objCorpAct.exDate = item.exDate;
                                                objCorpAct.exDate = DateTime.ParseExact(item?.exDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("dd-MM-yyyy");//item.exDate;
                                                objCorpAct.exDateSort = item.exDate;
                                                objCorpAct.bookClosureFrom = item.bookClosureFrom;
                                                objCorpAct.bookClosureTo = item.bookClosureTo;
                                                objCorpAct.NdStartDate = item.NdStartDate;
                                                objCorpAct.ndEndDate = item.ndEndDate;
                                                objCorpAct.bcOrRdFlag = item.bcOrRdFlag;
                                            }
                                            else if (item.bcOrRdFlag == "RD")
                                            {
                                                objCorpAct.ScripID = item.ScripID;
                                                objCorpAct.scripCode = item.scripCode;
                                                objCorpAct.Group = item.Group;
                                                objCorpAct.purposeOrEvent = item.purposeOrEvent;
                                                //objCorpAct.exDate = item.exDate;
                                                objCorpAct.exDate = DateTime.ParseExact(item?.exDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("dd-MM-yyyy");//item.exDate;
                                                objCorpAct.exDateSort = item.exDate;
                                                objCorpAct.recordDate = DateTime.ParseExact(item?.recordDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("dd-MM-yyyy");//item.recordDate;
                                                objCorpAct.recordDateSort = item.recordDate;
                                                objCorpAct.NdStartDate = item.NdStartDate;
                                                objCorpAct.ndEndDate = item.ndEndDate;
                                                objCorpAct.bcOrRdFlag = item.bcOrRdFlag;
                                            }
                                            ObjCorporateActionDataCollection.Add(objCorpAct);
                                        }
                                    }
                                }
                            }
                            ObjCorporateActionDataCollection.OrderBy(x => x.exDate);
                            TitleCorporate = "Detailed Corporate Action - Count: " + ObjCorporateActionDataCollection.Count;
                            TempCorporateActionDataCollection = ObjCorporateActionDataCollection;
                        }
                        else if (segment == "All")
                        {
                            ObjCorporateActionDataCollection.Clear();
                            foreach (var item in objCorpActBSE.Values)//objCorpActBSE.Values.Where(x =>(!x.Group.Equals(null)) && (!x.Group.Equals(string.Empty)) && (!x.Group.Contains("F,G,FC,GC"))))
                            {
                                if (item != null)
                                {
                                    CorporateActionModel objCorpAct = new CorporateActionModel();
                                    if (item.purposeOrEvent.Contains("DP"))
                                    {
                                        if (item.bcOrRdFlag == "BC")
                                        {
                                            objCorpAct.ScripID = item.ScripID;
                                            objCorpAct.scripCode = item.scripCode;
                                            objCorpAct.Group = item.Group;
                                            objCorpAct.purposeOrEvent = item.purposeOrEvent;
                                            // objCorpAct.exDate = item.exDate;
                                            objCorpAct.exDate = DateTime.ParseExact(item?.exDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("dd-MM-yyyy");//item.exDate;
                                            objCorpAct.exDateSort = item.exDate;
                                            objCorpAct.bookClosureFrom = item.bookClosureFrom;
                                            objCorpAct.bookClosureTo = item.bookClosureTo;
                                            objCorpAct.NdStartDate = item.NdStartDate;
                                            objCorpAct.ndEndDate = item.ndEndDate;
                                            objCorpAct.bcOrRdFlag = item.bcOrRdFlag;
                                        }
                                        else if (item.bcOrRdFlag == "RD")
                                        {
                                            objCorpAct.ScripID = item.ScripID;
                                            objCorpAct.scripCode = item.scripCode;
                                            objCorpAct.Group = item.Group;
                                            objCorpAct.purposeOrEvent = item.purposeOrEvent;
                                            //objCorpAct.exDate = item.exDate;
                                            objCorpAct.exDate = DateTime.ParseExact(item?.exDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("dd-MM-yyyy");//item.exDate;
                                            objCorpAct.exDateSort = item.exDate;
                                            objCorpAct.recordDate = DateTime.ParseExact(item?.recordDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("dd-MM-yyyy");//item.recordDate;
                                            objCorpAct.recordDateSort = item.recordDate;
                                            objCorpAct.NdStartDate = item.NdStartDate;
                                            objCorpAct.ndEndDate = item.ndEndDate;
                                            objCorpAct.bcOrRdFlag = item.bcOrRdFlag;
                                        }
                                        ObjCorporateActionDataCollection.Add(objCorpAct);
                                    }
                                }
                            }
                            ObjCorporateActionDataCollection.OrderBy(x => x.exDate);
                            TitleCorporate = "Detailed Corporate Action - Count: " + ObjCorporateActionDataCollection.Count;
                            TempCorporateActionDataCollection = ObjCorporateActionDataCollection;
                        }
                    }
                    break;

                case "Fut Bonus":
                    {
                        if (segment == "E")
                        {
                            ObjCorporateActionDataCollection.Clear();
                            foreach (var item in objCorpActBSE.Values)//objCorpActBSE.Values.Where(x =>(!x.Group.Equals(null)) && (!x.Group.Equals(string.Empty)) && (!x.Group.Contains("F,G,FC,GC"))))
                            {
                                if (item != null)
                                {
                                    if (!string.IsNullOrEmpty(item.Group) && !new[] { "F", "G", "FC", "GC" }.Any(x => x == item.Group.Trim()))
                                    {
                                        CorporateActionModel objCorpAct = new CorporateActionModel();
                                        if (item.purposeOrEvent.Contains("BN"))
                                        {
                                            if (item.bcOrRdFlag == "BC")
                                            {
                                                objCorpAct.ScripID = item.ScripID;
                                                objCorpAct.scripCode = item.scripCode;
                                                objCorpAct.Group = item.Group;
                                                objCorpAct.purposeOrEvent = item.purposeOrEvent;
                                                // objCorpAct.exDate = item.exDate;
                                                objCorpAct.exDate = DateTime.ParseExact(item?.exDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("dd-MM-yyyy");//item.exDate;
                                                objCorpAct.exDateSort = item.exDate;
                                                objCorpAct.bookClosureFrom = item.bookClosureFrom;
                                                objCorpAct.bookClosureTo = item.bookClosureTo;
                                                objCorpAct.NdStartDate = item.NdStartDate;
                                                objCorpAct.bcOrRdFlag = item.bcOrRdFlag;
                                                objCorpAct.ndEndDate = item.ndEndDate;
                                            }
                                            else if (item.bcOrRdFlag == "RD")
                                            {
                                                objCorpAct.ScripID = item.ScripID;
                                                objCorpAct.scripCode = item.scripCode;
                                                objCorpAct.Group = item.Group;
                                                objCorpAct.purposeOrEvent = item.purposeOrEvent;
                                                // objCorpAct.exDate = item.exDate;
                                                objCorpAct.exDate = DateTime.ParseExact(item?.exDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("dd-MM-yyyy");//item.exDate;
                                                objCorpAct.exDateSort = item.exDate;
                                                objCorpAct.recordDate = DateTime.ParseExact(item?.recordDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("dd-MM-yyyy");//item.recordDate;
                                                objCorpAct.recordDateSort = item.recordDate;
                                                objCorpAct.NdStartDate = item.NdStartDate;
                                                objCorpAct.ndEndDate = item.ndEndDate;
                                                objCorpAct.bcOrRdFlag = item.bcOrRdFlag;
                                            }
                                            ObjCorporateActionDataCollection.Add(objCorpAct);
                                        }
                                    }
                                }
                            }
                            ObjCorporateActionDataCollection.OrderBy(x => x.exDate);
                            TitleCorporate = "Detailed Corporate Action - Count: " + ObjCorporateActionDataCollection.Count;
                            TempCorporateActionDataCollection = ObjCorporateActionDataCollection;
                        }
                        else if (segment == "D")
                        {
                            ObjCorporateActionDataCollection.Clear();
                            foreach (var item in objCorpActBSE.Values)//objCorpActBSE.Values.Where(x =>(!x.Group.Equals(null)) && (!x.Group.Equals(string.Empty)) && (!x.Group.Contains("F,G,FC,GC"))))
                            {
                                if (item != null)
                                {
                                    if (!string.IsNullOrEmpty(item.Group) && new[] { "F", "G", "FC", "GC" }.Any(x => x == item.Group.Trim()))
                                    {
                                        CorporateActionModel objCorpAct = new CorporateActionModel();
                                        if (item.purposeOrEvent.Contains("BN"))
                                        {
                                            if (item.bcOrRdFlag == "BC")
                                            {
                                                objCorpAct.ScripID = item.ScripID;
                                                objCorpAct.scripCode = item.scripCode;
                                                objCorpAct.Group = item.Group;
                                                objCorpAct.purposeOrEvent = item.purposeOrEvent;
                                                // objCorpAct.exDate = item.exDate;
                                                objCorpAct.exDate = DateTime.ParseExact(item?.exDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("dd-MM-yyyy");//item.exDate;
                                                objCorpAct.exDateSort = item.exDate;
                                                objCorpAct.bookClosureFrom = item.bookClosureFrom;
                                                objCorpAct.bookClosureTo = item.bookClosureTo;
                                                objCorpAct.NdStartDate = item.NdStartDate;
                                                objCorpAct.ndEndDate = item.ndEndDate;
                                                objCorpAct.bcOrRdFlag = item.bcOrRdFlag;
                                            }
                                            else if (item.bcOrRdFlag == "RD")
                                            {
                                                objCorpAct.ScripID = item.ScripID;
                                                objCorpAct.scripCode = item.scripCode;
                                                objCorpAct.Group = item.Group;
                                                objCorpAct.purposeOrEvent = item.purposeOrEvent;
                                                // objCorpAct.exDate = item.exDate;
                                                objCorpAct.exDate = DateTime.ParseExact(item?.exDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("dd-MM-yyyy");//item.exDate;
                                                objCorpAct.exDateSort = item.exDate;
                                                objCorpAct.recordDate = DateTime.ParseExact(item?.recordDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("dd-MM-yyyy");// item.recordDate;
                                                objCorpAct.recordDateSort = item.recordDate;
                                                objCorpAct.NdStartDate = item.NdStartDate;
                                                objCorpAct.ndEndDate = item.ndEndDate;
                                                objCorpAct.bcOrRdFlag = item.bcOrRdFlag;
                                            }
                                            ObjCorporateActionDataCollection.Add(objCorpAct);
                                        }
                                    }
                                }
                            }
                            ObjCorporateActionDataCollection.OrderBy(x => x.exDate);
                            TitleCorporate = "Detailed Corporate Action - Count: " + ObjCorporateActionDataCollection.Count;
                            TempCorporateActionDataCollection = ObjCorporateActionDataCollection;
                        }
                        else if (segment == "All")
                        {
                            ObjCorporateActionDataCollection.Clear();
                            foreach (var item in objCorpActBSE.Values)//objCorpActBSE.Values.Where(x =>(!x.Group.Equals(null)) && (!x.Group.Equals(string.Empty)) && (!x.Group.Contains("F,G,FC,GC"))))
                            {
                                if (item != null)
                                {
                                    CorporateActionModel objCorpAct = new CorporateActionModel();
                                    if (item.purposeOrEvent.Contains("BN"))
                                    {
                                        if (item.bcOrRdFlag == "BC")
                                        {
                                            objCorpAct.ScripID = item.ScripID;
                                            objCorpAct.scripCode = item.scripCode;
                                            objCorpAct.Group = item.Group;
                                            objCorpAct.purposeOrEvent = item.purposeOrEvent;
                                            //objCorpAct.exDate = item.exDate;
                                            objCorpAct.exDate = DateTime.ParseExact(item?.exDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("dd-MM-yyyy");//item.exDate;
                                            objCorpAct.exDateSort = item.exDate;
                                            objCorpAct.bookClosureFrom = item.bookClosureFrom;
                                            objCorpAct.bookClosureTo = item.bookClosureTo;
                                            objCorpAct.NdStartDate = item.NdStartDate;
                                            objCorpAct.ndEndDate = item.ndEndDate;
                                            objCorpAct.bcOrRdFlag = item.bcOrRdFlag;
                                        }
                                        else if (item.bcOrRdFlag == "RD")
                                        {
                                            objCorpAct.ScripID = item.ScripID;
                                            objCorpAct.scripCode = item.scripCode;
                                            objCorpAct.Group = item.Group;
                                            objCorpAct.purposeOrEvent = item.purposeOrEvent;
                                            // objCorpAct.exDate = item.exDate;
                                            objCorpAct.exDate = DateTime.ParseExact(item?.exDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("dd-MM-yyyy");//item.exDate;
                                            objCorpAct.exDateSort = item.exDate;
                                            objCorpAct.recordDate = DateTime.ParseExact(item?.recordDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("dd-MM-yyyy");//item.recordDate;
                                            objCorpAct.recordDateSort = item.recordDate;
                                            objCorpAct.NdStartDate = item.NdStartDate;
                                            objCorpAct.ndEndDate = item.ndEndDate;
                                            objCorpAct.bcOrRdFlag = item.bcOrRdFlag;
                                        }
                                        ObjCorporateActionDataCollection.Add(objCorpAct);
                                    }
                                }
                            }
                            ObjCorporateActionDataCollection.OrderBy(x => x.exDate);
                            TitleCorporate = "Detailed Corporate Action - Count: " + ObjCorporateActionDataCollection.Count;
                            TempCorporateActionDataCollection = ObjCorporateActionDataCollection;
                        }
                    }
                    break;

                case "Fut Interest Payment":
                    {
                        if (segment == "E")
                        {
                            ObjCorporateActionDataCollection.Clear();
                            foreach (var item in objCorpActBSE.Values)//objCorpActBSE.Values.Where(x =>(!x.Group.Equals(null)) && (!x.Group.Equals(string.Empty)) && (!x.Group.Contains("F,G,FC,GC"))))
                            {
                                if (item != null)
                                {
                                    if (!string.IsNullOrEmpty(item.Group) && !new[] { "F", "G", "FC", "GC" }.Any(x => x == item.Group.Trim()))
                                    {
                                        CorporateActionModel objCorpAct = new CorporateActionModel();
                                        if (item.purposeOrEvent.Contains("IPB"))
                                        {
                                            if (item.bcOrRdFlag == "BC")
                                            {
                                                objCorpAct.ScripID = item.ScripID;
                                                objCorpAct.scripCode = item.scripCode;
                                                objCorpAct.Group = item.Group;
                                                objCorpAct.purposeOrEvent = item.purposeOrEvent;
                                                //objCorpAct.exDate = item.exDate;
                                                objCorpAct.exDate = DateTime.ParseExact(item?.exDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("dd-MM-yyyy");//item.exDate;
                                                objCorpAct.exDateSort = item.exDate;
                                                objCorpAct.bookClosureFrom = item.bookClosureFrom;
                                                objCorpAct.bookClosureTo = item.bookClosureTo;
                                                objCorpAct.NdStartDate = item.NdStartDate;
                                                objCorpAct.ndEndDate = item.ndEndDate;
                                                objCorpAct.bcOrRdFlag = item.bcOrRdFlag;
                                            }
                                            else if (item.bcOrRdFlag == "RD")
                                            {
                                                objCorpAct.ScripID = item.ScripID;
                                                objCorpAct.scripCode = item.scripCode;
                                                objCorpAct.Group = item.Group;
                                                objCorpAct.purposeOrEvent = item.purposeOrEvent;
                                                //objCorpAct.exDate = item.exDate;
                                                objCorpAct.exDate = DateTime.ParseExact(item?.exDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("dd-MM-yyyy");//item.exDate;
                                                objCorpAct.exDateSort = item.exDate;
                                                objCorpAct.recordDate = DateTime.ParseExact(item?.recordDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("dd-MM-yyyy");//item.recordDate;
                                                objCorpAct.recordDateSort = item.recordDate;
                                                objCorpAct.NdStartDate = item.NdStartDate;
                                                objCorpAct.ndEndDate = item.ndEndDate;
                                                objCorpAct.bcOrRdFlag = item.bcOrRdFlag;
                                            }
                                            ObjCorporateActionDataCollection.Add(objCorpAct);
                                        }
                                    }
                                }
                            }
                            ObjCorporateActionDataCollection.OrderBy(x => x.exDate);
                            TitleCorporate = "Detailed Corporate Action - Count: " + ObjCorporateActionDataCollection.Count;
                            TempCorporateActionDataCollection = ObjCorporateActionDataCollection;
                        }
                        else if (segment == "D")
                        {
                            ObjCorporateActionDataCollection.Clear();
                            foreach (var item in objCorpActBSE.Values)//objCorpActBSE.Values.Where(x =>(!x.Group.Equals(null)) && (!x.Group.Equals(string.Empty)) && (!x.Group.Contains("F,G,FC,GC"))))
                            {
                                if (item != null)
                                {
                                    if (!string.IsNullOrEmpty(item.Group) && new[] { "F", "G", "FC", "GC" }.Any(x => x == item.Group.Trim()))
                                    {
                                        CorporateActionModel objCorpAct = new CorporateActionModel();
                                        if (item.purposeOrEvent.Contains("IPB"))
                                        {
                                            if (item.bcOrRdFlag == "BC")
                                            {
                                                objCorpAct.ScripID = item.ScripID;
                                                objCorpAct.scripCode = item.scripCode;
                                                objCorpAct.Group = item.Group;
                                                objCorpAct.purposeOrEvent = item.purposeOrEvent;
                                                // objCorpAct.exDate = item.exDate;
                                                objCorpAct.exDate = DateTime.ParseExact(item?.exDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("dd-MM-yyyy");//item.exDate;
                                                objCorpAct.exDateSort = item.exDate;
                                                objCorpAct.bookClosureFrom = item.bookClosureFrom;
                                                objCorpAct.bookClosureTo = item.bookClosureTo;
                                                objCorpAct.NdStartDate = item.NdStartDate;
                                                objCorpAct.ndEndDate = item.ndEndDate;
                                                objCorpAct.bcOrRdFlag = item.bcOrRdFlag;
                                            }
                                            else if (item.bcOrRdFlag == "RD")
                                            {
                                                objCorpAct.ScripID = item.ScripID;
                                                objCorpAct.scripCode = item.scripCode;
                                                objCorpAct.Group = item.Group;
                                                objCorpAct.purposeOrEvent = item.purposeOrEvent;
                                                // objCorpAct.exDate = item.exDate;
                                                objCorpAct.exDate = DateTime.ParseExact(item?.exDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("dd-MM-yyyy");//item.exDate;
                                                objCorpAct.exDateSort = item.exDate;
                                                objCorpAct.recordDate = DateTime.ParseExact(item?.recordDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("dd-MM-yyyy");//item.recordDate;
                                                objCorpAct.recordDateSort = item.recordDate;
                                                objCorpAct.NdStartDate = item.NdStartDate;
                                                objCorpAct.ndEndDate = item.ndEndDate;
                                                objCorpAct.bcOrRdFlag = item.bcOrRdFlag;
                                            }
                                            ObjCorporateActionDataCollection.Add(objCorpAct);
                                        }
                                    }
                                }
                            }
                            ObjCorporateActionDataCollection.OrderBy(x => x.exDate);
                            TitleCorporate = "Detailed Corporate Action - Count: " + ObjCorporateActionDataCollection.Count;
                            TempCorporateActionDataCollection = ObjCorporateActionDataCollection;
                        }
                        else if (segment == "All")
                        {
                            ObjCorporateActionDataCollection.Clear();
                            foreach (var item in objCorpActBSE.Values)//objCorpActBSE.Values.Where(x =>(!x.Group.Equals(null)) && (!x.Group.Equals(string.Empty)) && (!x.Group.Contains("F,G,FC,GC"))))
                            {
                                if (item != null)
                                {
                                    CorporateActionModel objCorpAct = new CorporateActionModel();
                                    if (item.purposeOrEvent.Contains("IPB"))
                                    {
                                        if (item.bcOrRdFlag == "BC")
                                        {
                                            objCorpAct.ScripID = item.ScripID;
                                            objCorpAct.scripCode = item.scripCode;
                                            objCorpAct.Group = item.Group;
                                            objCorpAct.purposeOrEvent = item.purposeOrEvent;
                                            //objCorpAct.exDate = item.exDate;
                                            objCorpAct.exDate = DateTime.ParseExact(item?.exDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("dd-MM-yyyy");//item.exDate;
                                            objCorpAct.exDateSort = item.exDate;
                                            objCorpAct.bookClosureFrom = item.bookClosureFrom;
                                            objCorpAct.bookClosureTo = item.bookClosureTo;
                                            objCorpAct.NdStartDate = item.NdStartDate;
                                            objCorpAct.ndEndDate = item.ndEndDate;
                                            objCorpAct.bcOrRdFlag = item.bcOrRdFlag;
                                        }
                                        else if (item.bcOrRdFlag == "RD")
                                        {
                                            objCorpAct.ScripID = item.ScripID;
                                            objCorpAct.scripCode = item.scripCode;
                                            objCorpAct.Group = item.Group;
                                            objCorpAct.purposeOrEvent = item.purposeOrEvent;
                                            //objCorpAct.exDate = item.exDate;
                                            objCorpAct.exDate = DateTime.ParseExact(item?.exDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("dd-MM-yyyy");//item.exDate;
                                            objCorpAct.exDateSort = item.exDate;
                                            objCorpAct.recordDate = DateTime.ParseExact(item?.recordDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("dd-MM-yyyy");//item.recordDate;
                                            objCorpAct.recordDateSort = item.recordDate;
                                            objCorpAct.NdStartDate = item.NdStartDate;
                                            objCorpAct.ndEndDate = item.ndEndDate;
                                            objCorpAct.bcOrRdFlag = item.bcOrRdFlag;
                                        }
                                        ObjCorporateActionDataCollection.Add(objCorpAct);
                                    }
                                }
                            }
                            ObjCorporateActionDataCollection.OrderBy(x => x.exDate);
                            TitleCorporate = "Detailed Corporate Action - Count: " + ObjCorporateActionDataCollection.Count;
                            TempCorporateActionDataCollection = ObjCorporateActionDataCollection;
                        }
                    }
                    break;

                case "Fut Stock Split":
                    {
                        if (segment == "E")
                        {
                            ObjCorporateActionDataCollection.Clear();
                            foreach (var item in objCorpActBSE.Values)//objCorpActBSE.Values.Where(x =>(!x.Group.Equals(null)) && (!x.Group.Equals(string.Empty)) && (!x.Group.Contains("F,G,FC,GC"))))
                            {
                                if (item != null)
                                {
                                    if (!string.IsNullOrEmpty(item.Group) && !new[] { "F", "G", "FC", "GC" }.Any(x => x == item.Group.Trim()))
                                    {
                                        CorporateActionModel objCorpAct = new CorporateActionModel();
                                        if (item.purposeOrEvent.Contains("SS"))
                                        {
                                            if (item.bcOrRdFlag == "BC")
                                            {
                                                objCorpAct.ScripID = item.ScripID;
                                                objCorpAct.scripCode = item.scripCode;
                                                objCorpAct.Group = item.Group;
                                                objCorpAct.purposeOrEvent = item.purposeOrEvent;
                                                // objCorpAct.exDate = item.exDate;
                                                objCorpAct.exDate = DateTime.ParseExact(item?.exDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("dd-MM-yyyy");//item.exDate;
                                                objCorpAct.exDateSort = item.exDate;
                                                objCorpAct.bookClosureFrom = item.bookClosureFrom;
                                                objCorpAct.bookClosureTo = item.bookClosureTo;
                                                objCorpAct.NdStartDate = item.NdStartDate;
                                                objCorpAct.ndEndDate = item.ndEndDate;
                                                objCorpAct.bcOrRdFlag = item.bcOrRdFlag;
                                            }
                                            else if (item.bcOrRdFlag == "RD")
                                            {
                                                objCorpAct.ScripID = item.ScripID;
                                                objCorpAct.scripCode = item.scripCode;
                                                objCorpAct.Group = item.Group;
                                                objCorpAct.purposeOrEvent = item.purposeOrEvent;
                                                // objCorpAct.exDate = item.exDate;
                                                objCorpAct.exDate = DateTime.ParseExact(item?.exDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("dd-MM-yyyy");//item.exDate;
                                                objCorpAct.exDateSort = item.exDate;
                                                objCorpAct.recordDate = DateTime.ParseExact(item?.recordDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("dd-MM-yyyy");// item.recordDate;
                                                objCorpAct.recordDateSort = item.recordDate;
                                                objCorpAct.NdStartDate = item.NdStartDate;
                                                objCorpAct.ndEndDate = item.ndEndDate;
                                                objCorpAct.bcOrRdFlag = item.bcOrRdFlag;
                                            }
                                            ObjCorporateActionDataCollection.Add(objCorpAct);
                                        }
                                    }
                                }
                            }
                            ObjCorporateActionDataCollection.OrderBy(x => x.exDate);
                            TitleCorporate = "Detailed Corporate Action - Count: " + ObjCorporateActionDataCollection.Count;
                            TempCorporateActionDataCollection = ObjCorporateActionDataCollection;
                        }
                        else if (segment == "D")
                        {
                            ObjCorporateActionDataCollection.Clear();
                            foreach (var item in objCorpActBSE.Values)//objCorpActBSE.Values.Where(x =>(!x.Group.Equals(null)) && (!x.Group.Equals(string.Empty)) && (!x.Group.Contains("F,G,FC,GC"))))
                            {
                                if (item != null)
                                {
                                    if (!string.IsNullOrEmpty(item.Group) && new[] { "F", "G", "FC", "GC" }.Any(x => x == item.Group.Trim()))
                                    {
                                        CorporateActionModel objCorpAct = new CorporateActionModel();
                                        if (item.purposeOrEvent.Contains("SS"))
                                        {
                                            if (item.bcOrRdFlag == "BC")
                                            {
                                                objCorpAct.ScripID = item.ScripID;
                                                objCorpAct.scripCode = item.scripCode;
                                                objCorpAct.Group = item.Group;
                                                objCorpAct.purposeOrEvent = item.purposeOrEvent;
                                                //objCorpAct.exDate = item.exDate;
                                                objCorpAct.exDate = DateTime.ParseExact(item?.exDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("dd-MM-yyyy");//item.exDate;
                                                objCorpAct.exDateSort = item.exDate;
                                                objCorpAct.bookClosureFrom = item.bookClosureFrom;
                                                objCorpAct.bookClosureTo = item.bookClosureTo;
                                                objCorpAct.NdStartDate = item.NdStartDate;
                                                objCorpAct.ndEndDate = item.ndEndDate;
                                                objCorpAct.bcOrRdFlag = item.bcOrRdFlag;
                                            }
                                            else if (item.bcOrRdFlag == "RD")
                                            {
                                                objCorpAct.ScripID = item.ScripID;
                                                objCorpAct.scripCode = item.scripCode;
                                                objCorpAct.Group = item.Group;
                                                objCorpAct.purposeOrEvent = item.purposeOrEvent;
                                                // objCorpAct.exDate = item.exDate;
                                                objCorpAct.exDate = DateTime.ParseExact(item?.exDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("dd-MM-yyyy");//item.exDate;
                                                objCorpAct.exDateSort = item.exDate;
                                                objCorpAct.recordDate = DateTime.ParseExact(item?.recordDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("dd-MM-yyyy");//item.recordDate;
                                                objCorpAct.recordDateSort = item.recordDate;
                                                objCorpAct.NdStartDate = item.NdStartDate;
                                                objCorpAct.ndEndDate = item.ndEndDate;
                                                objCorpAct.bcOrRdFlag = item.bcOrRdFlag;
                                            }
                                            ObjCorporateActionDataCollection.Add(objCorpAct);
                                        }
                                    }
                                }
                            }
                            ObjCorporateActionDataCollection.OrderBy(x => x.exDate);
                            TitleCorporate = "Detailed Corporate Action - Count: " + ObjCorporateActionDataCollection.Count;
                            TempCorporateActionDataCollection = ObjCorporateActionDataCollection;
                        }
                        else if (segment == "All")
                        {
                            ObjCorporateActionDataCollection.Clear();
                            foreach (var item in objCorpActBSE.Values)//objCorpActBSE.Values.Where(x =>(!x.Group.Equals(null)) && (!x.Group.Equals(string.Empty)) && (!x.Group.Contains("F,G,FC,GC"))))
                            {
                                if (item != null)
                                {
                                    CorporateActionModel objCorpAct = new CorporateActionModel();
                                    if (item.purposeOrEvent.Contains("SS"))
                                    {
                                        if (item.bcOrRdFlag == "BC")
                                        {
                                            objCorpAct.ScripID = item.ScripID;
                                            objCorpAct.scripCode = item.scripCode;
                                            objCorpAct.Group = item.Group;
                                            objCorpAct.purposeOrEvent = item.purposeOrEvent;
                                            //objCorpAct.exDate = item.exDate;
                                            objCorpAct.exDate = DateTime.ParseExact(item?.exDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("dd-MM-yyyy");//item.exDate;
                                            objCorpAct.exDateSort = item.exDate;
                                            objCorpAct.bookClosureFrom = item.bookClosureFrom;
                                            objCorpAct.bookClosureTo = item.bookClosureTo;
                                            objCorpAct.NdStartDate = item.NdStartDate;
                                            objCorpAct.ndEndDate = item.ndEndDate;
                                            objCorpAct.bcOrRdFlag = item.bcOrRdFlag;
                                        }
                                        else if (item.bcOrRdFlag == "RD")
                                        {
                                            objCorpAct.ScripID = item.ScripID;
                                            objCorpAct.scripCode = item.scripCode;
                                            objCorpAct.Group = item.Group;
                                            objCorpAct.purposeOrEvent = item.purposeOrEvent;
                                            // objCorpAct.exDate = item.exDate;
                                            objCorpAct.exDate = DateTime.ParseExact(item?.exDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("dd-MM-yyyy");//item.exDate;
                                            objCorpAct.exDateSort = item.exDate;
                                            objCorpAct.recordDate = DateTime.ParseExact(item?.recordDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("dd-MM-yyyy");//item.recordDate;
                                            objCorpAct.recordDateSort = item.recordDate;
                                            objCorpAct.NdStartDate = item.NdStartDate;
                                            objCorpAct.ndEndDate = item.ndEndDate;
                                            objCorpAct.bcOrRdFlag = item.bcOrRdFlag;
                                        }
                                        ObjCorporateActionDataCollection.Add(objCorpAct);
                                    }
                                }
                            }
                            ObjCorporateActionDataCollection.OrderBy(x => x.exDate);
                            TitleCorporate = "Detailed Corporate Action - Count: " + ObjCorporateActionDataCollection.Count;
                            TempCorporateActionDataCollection = ObjCorporateActionDataCollection;
                        }
                    }
                    break;

                case "Today Cum Date":
                    {
                        if (segment == "E")
                        {
                            ObjCorporateActionDataCollection.Clear();
                            foreach (var item in objCumExDateCorpActBSE.Values)//objCorpActBSE.Values.Where(x =>(!x.Group.Equals(null)) && (!x.Group.Equals(string.Empty)) && (!x.Group.Contains("F,G,FC,GC"))))
                            {
                                if (item != null)
                                {
                                    if (!string.IsNullOrEmpty(item.Group) && !new[] { "F", "G", "FC", "GC" }.Any(x => x == item.Group.Trim()))
                                    {
                                        CorporateActionModel objCorpAct = new CorporateActionModel();
                                        if (item.bcOrRdFlag == "BC")
                                        {
                                            objCorpAct.ScripID = item.ScripID;
                                            objCorpAct.scripCode = item.scripCode;
                                            objCorpAct.Group = item.Group;
                                            objCorpAct.purposeOrEvent = item.purposeOrEvent;
                                            //objCorpAct.exDate = item.exDate;
                                            objCorpAct.exDate = DateTime.ParseExact(item?.exDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("dd-MM-yyyy");//item.exDate;
                                            objCorpAct.exDateSort = item.exDate;
                                            objCorpAct.bookClosureFrom = item.bookClosureFrom;
                                            objCorpAct.bookClosureTo = item.bookClosureTo;
                                            objCorpAct.NdStartDate = item.NdStartDate;
                                            objCorpAct.ndEndDate = item.ndEndDate;
                                            objCorpAct.bcOrRdFlag = item.bcOrRdFlag;
                                        }
                                        else if (item.bcOrRdFlag == "RD")
                                        {
                                            objCorpAct.ScripID = item.ScripID;
                                            objCorpAct.scripCode = item.scripCode;
                                            objCorpAct.Group = item.Group;
                                            objCorpAct.purposeOrEvent = item.purposeOrEvent;
                                            // objCorpAct.exDate = item.exDate;
                                            objCorpAct.exDate = DateTime.ParseExact(item?.exDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("dd-MM-yyyy");//item.exDate;
                                            objCorpAct.exDateSort = item.exDate;
                                            objCorpAct.recordDate = DateTime.ParseExact(item?.recordDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("dd-MM-yyyy");//item.recordDate;
                                            objCorpAct.recordDateSort = item.recordDate;
                                            objCorpAct.NdStartDate = item.NdStartDate;
                                            objCorpAct.ndEndDate = item.ndEndDate;
                                            objCorpAct.bcOrRdFlag = item.bcOrRdFlag;
                                        }
                                        ObjCorporateActionDataCollection.Add(objCorpAct);
                                    }
                                }
                            }
                            ObjCorporateActionDataCollection.OrderBy(x => x.exDate);
                            TitleCorporate = "Detailed Corporate Action - Count: " + ObjCorporateActionDataCollection.Count;
                            TempCorporateActionDataCollection = ObjCorporateActionDataCollection;
                        }
                        else if (segment == "D")
                        {
                            ObjCorporateActionDataCollection.Clear();
                            foreach (var item in objCumExDateCorpActBSE.Values)//objCorpActBSE.Values.Where(x =>(!x.Group.Equals(null)) && (!x.Group.Equals(string.Empty)) && (!x.Group.Contains("F,G,FC,GC"))))
                            {
                                if (item != null)
                                {
                                    if (!string.IsNullOrEmpty(item.Group) && new[] { "F", "G", "FC", "GC" }.Any(x => x == item.Group.Trim()))
                                    {
                                        CorporateActionModel objCorpAct = new CorporateActionModel();
                                        if (item.bcOrRdFlag == "BC")
                                        {
                                            objCorpAct.ScripID = item.ScripID;
                                            objCorpAct.scripCode = item.scripCode;
                                            objCorpAct.Group = item.Group;
                                            objCorpAct.purposeOrEvent = item.purposeOrEvent;
                                            //objCorpAct.exDate = item.exDate;
                                            objCorpAct.exDate = DateTime.ParseExact(item?.exDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("dd-MM-yyyy");//item.exDate;
                                            objCorpAct.exDateSort = item.exDate;
                                            objCorpAct.bookClosureFrom = item.bookClosureFrom;
                                            objCorpAct.bookClosureTo = item.bookClosureTo;
                                            objCorpAct.NdStartDate = item.NdStartDate;
                                            objCorpAct.ndEndDate = item.ndEndDate;
                                            objCorpAct.bcOrRdFlag = item.bcOrRdFlag;
                                        }
                                        else if (item.bcOrRdFlag == "RD")
                                        {
                                            objCorpAct.ScripID = item.ScripID;
                                            objCorpAct.scripCode = item.scripCode;
                                            objCorpAct.Group = item.Group;
                                            objCorpAct.purposeOrEvent = item.purposeOrEvent;
                                            //objCorpAct.exDate = item.exDate;
                                            objCorpAct.exDate = DateTime.ParseExact(item?.exDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("dd-MM-yyyy");//item.exDate;
                                            objCorpAct.exDateSort = item.exDate;
                                            objCorpAct.recordDate = DateTime.ParseExact(item?.recordDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("dd-MM-yyyy");//item.recordDate;
                                            objCorpAct.recordDateSort = item.recordDate;
                                            objCorpAct.NdStartDate = item.NdStartDate;
                                            objCorpAct.ndEndDate = item.ndEndDate;
                                            objCorpAct.bcOrRdFlag = item.bcOrRdFlag;
                                        }
                                        ObjCorporateActionDataCollection.Add(objCorpAct);
                                    }
                                }
                            }
                            ObjCorporateActionDataCollection.OrderBy(x => x.exDate);
                            TitleCorporate = "Detailed Corporate Action - Count: " + ObjCorporateActionDataCollection.Count;
                            TempCorporateActionDataCollection = ObjCorporateActionDataCollection;
                        }
                        else if (segment == "All")
                        {
                            ObjCorporateActionDataCollection.Clear();
                            foreach (var item in objCumExDateCorpActBSE.Values)//objCorpActBSE.Values.Where(x =>(!x.Group.Equals(null)) && (!x.Group.Equals(string.Empty)) && (!x.Group.Contains("F,G,FC,GC"))))
                            {
                                if (item != null)
                                {
                                    CorporateActionModel objCorpAct = new CorporateActionModel();
                                    if (item.bcOrRdFlag == "BC")
                                    {
                                        objCorpAct.ScripID = item.ScripID;
                                        objCorpAct.scripCode = item.scripCode;
                                        objCorpAct.Group = item.Group;
                                        objCorpAct.purposeOrEvent = item.purposeOrEvent;
                                        // objCorpAct.exDate = item.exDate;
                                        objCorpAct.exDate = DateTime.ParseExact(item?.exDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("dd-MM-yyyy");//item.exDate;
                                        objCorpAct.exDateSort = item.exDate;
                                        objCorpAct.bookClosureFrom = item.bookClosureFrom;
                                        objCorpAct.bookClosureTo = item.bookClosureTo;
                                        objCorpAct.NdStartDate = item.NdStartDate;
                                        objCorpAct.ndEndDate = item.ndEndDate;
                                        objCorpAct.bcOrRdFlag = item.bcOrRdFlag;
                                    }
                                    else if (item.bcOrRdFlag == "RD")
                                    {
                                        objCorpAct.ScripID = item.ScripID;
                                        objCorpAct.scripCode = item.scripCode;
                                        objCorpAct.Group = item.Group;
                                        objCorpAct.purposeOrEvent = item.purposeOrEvent;
                                        // objCorpAct.exDate = item.exDate;
                                        objCorpAct.exDate = DateTime.ParseExact(item?.exDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("dd-MM-yyyy");//item.exDate;
                                        objCorpAct.exDateSort = item.exDate;
                                        objCorpAct.recordDate = DateTime.ParseExact(item?.recordDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("dd-MM-yyyy");//item.recordDate;
                                        objCorpAct.recordDateSort = item.recordDate;
                                        objCorpAct.NdStartDate = item.NdStartDate;
                                        objCorpAct.ndEndDate = item.ndEndDate;
                                        objCorpAct.bcOrRdFlag = item.bcOrRdFlag;
                                    }
                                    ObjCorporateActionDataCollection.Add(objCorpAct);
                                }
                            }
                            ObjCorporateActionDataCollection.OrderBy(x => x.exDate);
                            TitleCorporate = "Detailed Corporate Action - Count: " + ObjCorporateActionDataCollection.Count;
                            TempCorporateActionDataCollection = ObjCorporateActionDataCollection;
                        }
                    }
                    break;
                case "Today Ex Date":
                    {
                        if (segment == "E")
                        {
                            ObjCorporateActionDataCollection.Clear();
                            foreach (var item in objExDatePresentDateCorpActBSE.Values)//objCorpActBSE.Values.Where(x =>(!x.Group.Equals(null)) && (!x.Group.Equals(string.Empty)) && (!x.Group.Contains("F,G,FC,GC"))))
                            {
                                if (item != null)
                                {
                                    if (!string.IsNullOrEmpty(item.Group) && !new[] { "F", "G", "FC", "GC" }.Any(x => x == item.Group.Trim()))
                                    {
                                        CorporateActionModel objCorpAct = new CorporateActionModel();
                                        if (item.bcOrRdFlag == "BC")
                                        {
                                            objCorpAct.ScripID = item.ScripID;
                                            objCorpAct.scripCode = item.scripCode;
                                            objCorpAct.Group = item.Group;
                                            objCorpAct.purposeOrEvent = item.purposeOrEvent;
                                            // objCorpAct.exDate = item.exDate;
                                            objCorpAct.exDate = DateTime.ParseExact(item?.exDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("dd-MM-yyyy");//item.exDate;
                                            objCorpAct.exDateSort = item.exDate;
                                            objCorpAct.bookClosureFrom = item.bookClosureFrom;
                                            objCorpAct.bookClosureTo = item.bookClosureTo;
                                            objCorpAct.NdStartDate = item.NdStartDate;
                                            objCorpAct.ndEndDate = item.ndEndDate;
                                            objCorpAct.bcOrRdFlag = item.bcOrRdFlag;
                                        }
                                        else if (item.bcOrRdFlag == "RD")
                                        {
                                            objCorpAct.ScripID = item.ScripID;
                                            objCorpAct.scripCode = item.scripCode;
                                            objCorpAct.Group = item.Group;
                                            objCorpAct.purposeOrEvent = item.purposeOrEvent;
                                            //objCorpAct.exDate = item.exDate;
                                            objCorpAct.exDate = DateTime.ParseExact(item?.exDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("dd-MM-yyyy");//item.exDate;
                                            objCorpAct.exDateSort = item.exDate;
                                            objCorpAct.recordDate = DateTime.ParseExact(item?.recordDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("dd-MM-yyyy");// item.recordDate;
                                            objCorpAct.recordDateSort = item.recordDate;
                                            objCorpAct.NdStartDate = item.NdStartDate;
                                            objCorpAct.ndEndDate = item.ndEndDate;
                                            objCorpAct.bcOrRdFlag = item.bcOrRdFlag;
                                        }
                                        ObjCorporateActionDataCollection.Add(objCorpAct);
                                    }
                                }
                            }
                            ObjCorporateActionDataCollection.OrderBy(x => x.exDate);
                            TitleCorporate = "Detailed Corporate Action - Count: " + ObjCorporateActionDataCollection.Count;
                            TempCorporateActionDataCollection = ObjCorporateActionDataCollection;
                        }
                        else if (segment == "D")
                        {
                            ObjCorporateActionDataCollection.Clear();
                            foreach (var item in objExDatePresentDateCorpActBSE.Values)//objCorpActBSE.Values.Where(x =>(!x.Group.Equals(null)) && (!x.Group.Equals(string.Empty)) && (!x.Group.Contains("F,G,FC,GC"))))
                            {
                                if (item != null)
                                {
                                    if (!string.IsNullOrEmpty(item.Group) && new[] { "F", "G", "FC", "GC" }.Any(x => x == item.Group.Trim()))
                                    {
                                        CorporateActionModel objCorpAct = new CorporateActionModel();
                                        if (item.bcOrRdFlag == "BC")
                                        {
                                            objCorpAct.ScripID = item.ScripID;
                                            objCorpAct.scripCode = item.scripCode;
                                            objCorpAct.Group = item.Group;
                                            objCorpAct.purposeOrEvent = item.purposeOrEvent;
                                            //objCorpAct.exDate = item.exDate;
                                            objCorpAct.exDate = DateTime.ParseExact(item?.exDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("dd-MM-yyyy");//item.exDate;
                                            objCorpAct.exDateSort = item.exDate;
                                            objCorpAct.bookClosureFrom = item.bookClosureFrom;
                                            objCorpAct.bookClosureTo = item.bookClosureTo;
                                            objCorpAct.NdStartDate = item.NdStartDate;
                                            objCorpAct.ndEndDate = item.ndEndDate;
                                            objCorpAct.bcOrRdFlag = item.bcOrRdFlag;
                                        }
                                        else if (item.bcOrRdFlag == "RD")
                                        {
                                            objCorpAct.ScripID = item.ScripID;
                                            objCorpAct.scripCode = item.scripCode;
                                            objCorpAct.Group = item.Group;
                                            objCorpAct.purposeOrEvent = item.purposeOrEvent;
                                            // objCorpAct.exDate = item.exDate;
                                            objCorpAct.exDate = DateTime.ParseExact(item?.exDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("dd-MM-yyyy");//item.exDate;
                                            objCorpAct.exDateSort = item.exDate;
                                            objCorpAct.recordDate = DateTime.ParseExact(item?.recordDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("dd-MM-yyyy");//item.recordDate;
                                            objCorpAct.recordDateSort = item.recordDate;
                                            objCorpAct.NdStartDate = item.NdStartDate;
                                            objCorpAct.ndEndDate = item.ndEndDate;
                                            objCorpAct.bcOrRdFlag = item.bcOrRdFlag;
                                        }
                                        ObjCorporateActionDataCollection.Add(objCorpAct);
                                    }
                                }
                            }
                            ObjCorporateActionDataCollection.OrderBy(x => x.exDate);
                            TitleCorporate = "Detailed Corporate Action - Count: " + ObjCorporateActionDataCollection.Count;
                            TempCorporateActionDataCollection = ObjCorporateActionDataCollection;
                        }
                        else if (segment == "All")
                        {
                            ObjCorporateActionDataCollection.Clear();
                            foreach (var item in objExDatePresentDateCorpActBSE.Values)//objCorpActBSE.Values.Where(x =>(!x.Group.Equals(null)) && (!x.Group.Equals(string.Empty)) && (!x.Group.Contains("F,G,FC,GC"))))
                            {
                                if (item != null)
                                {
                                    CorporateActionModel objCorpAct = new CorporateActionModel();
                                    if (item.bcOrRdFlag == "BC")
                                    {
                                        objCorpAct.ScripID = item.ScripID;
                                        objCorpAct.scripCode = item.scripCode;
                                        objCorpAct.Group = item.Group;
                                        objCorpAct.purposeOrEvent = item.purposeOrEvent;
                                        //objCorpAct.exDate = item.exDate;
                                        objCorpAct.exDate = DateTime.ParseExact(item?.exDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("dd-MM-yyyy");//item.exDate;
                                        objCorpAct.exDateSort = item.exDate;
                                        objCorpAct.bookClosureFrom = item.bookClosureFrom;
                                        objCorpAct.bookClosureTo = item.bookClosureTo;
                                        objCorpAct.NdStartDate = item.NdStartDate;
                                        objCorpAct.ndEndDate = item.ndEndDate;
                                        objCorpAct.bcOrRdFlag = item.bcOrRdFlag;
                                    }
                                    else if (item.bcOrRdFlag == "RD")
                                    {
                                        objCorpAct.ScripID = item.ScripID;
                                        objCorpAct.scripCode = item.scripCode;
                                        objCorpAct.Group = item.Group;
                                        objCorpAct.purposeOrEvent = item.purposeOrEvent;
                                        // objCorpAct.exDate = item.exDate;
                                        objCorpAct.exDate = DateTime.ParseExact(item?.exDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("dd-MM-yyyy");//item.exDate;
                                        objCorpAct.exDateSort = item.exDate;
                                        objCorpAct.recordDate = DateTime.ParseExact(item?.recordDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("dd-MM-yyyy");//item.recordDate;
                                        objCorpAct.recordDateSort = item.recordDate;
                                        objCorpAct.NdStartDate = item.NdStartDate;
                                        objCorpAct.ndEndDate = item.ndEndDate;
                                        objCorpAct.bcOrRdFlag = item.bcOrRdFlag;
                                    }
                                    ObjCorporateActionDataCollection.Add(objCorpAct);
                                }
                            }
                            ObjCorporateActionDataCollection.OrderBy(x => x.exDate);
                            TitleCorporate = "Detailed Corporate Action - Count: " + ObjCorporateActionDataCollection.Count;
                            TempCorporateActionDataCollection = ObjCorporateActionDataCollection;
                        }
                    }
                    break;

                case "All":
                    {
                        if (segment == "E")
                        {
                            ObjCorporateActionDataCollection.Clear();
                            foreach (var item in objCorpActBSE.Values)//objCorpActBSE.Values.Where(x =>(!x.Group.Equals(null)) && (!x.Group.Equals(string.Empty)) && (!x.Group.Contains("F,G,FC,GC"))))
                            {
                                if (item != null)
                                {
                                    if (!string.IsNullOrEmpty(item.Group) && !new[] { "F", "G", "FC", "GC" }.Any(x => x == item.Group.Trim()))
                                    {
                                        CorporateActionModel objCorpAct = new CorporateActionModel();
                                        if (item.bcOrRdFlag == "BC")
                                        {
                                            objCorpAct.ScripID = item.ScripID;
                                            objCorpAct.scripCode = item.scripCode;
                                            objCorpAct.Group = item.Group;
                                            objCorpAct.purposeOrEvent = item.purposeOrEvent;
                                            // objCorpAct.exDate = item.exDate;
                                            objCorpAct.exDate = DateTime.ParseExact(item?.exDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("dd-MM-yyyy");//item.exDate;
                                            objCorpAct.exDateSort = item.exDate;
                                            objCorpAct.bookClosureFrom = item.bookClosureFrom;
                                            objCorpAct.bookClosureTo = item.bookClosureTo;
                                            objCorpAct.NdStartDate = item.NdStartDate;
                                            objCorpAct.ndEndDate = item.ndEndDate;
                                            objCorpAct.bcOrRdFlag = item.bcOrRdFlag;
                                        }
                                        else if (item.bcOrRdFlag == "RD")
                                        {
                                            objCorpAct.ScripID = item.ScripID;
                                            objCorpAct.scripCode = item.scripCode;
                                            objCorpAct.Group = item.Group;
                                            objCorpAct.purposeOrEvent = item.purposeOrEvent;
                                            // objCorpAct.exDate = item.exDate;
                                            objCorpAct.exDate = DateTime.ParseExact(item?.exDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("dd-MM-yyyy");//item.exDate;
                                            objCorpAct.exDateSort = item.exDate;
                                            objCorpAct.recordDate = DateTime.ParseExact(item?.recordDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("dd-MM-yyyy");//item.recordDate;
                                            objCorpAct.recordDateSort = item.recordDate;
                                            objCorpAct.NdStartDate = item.NdStartDate;
                                            objCorpAct.ndEndDate = item.ndEndDate;
                                            objCorpAct.bcOrRdFlag = item.bcOrRdFlag;
                                        }
                                        ObjCorporateActionDataCollection.Add(objCorpAct);
                                    }
                                }
                            }
                            ObjCorporateActionDataCollection.OrderBy(x => x.exDate);
                            TitleCorporate = "Detailed Corporate Action - Count: " + ObjCorporateActionDataCollection.Count;
                            TempCorporateActionDataCollection = ObjCorporateActionDataCollection;
                        }
                        else if (segment == "D")
                        {
                            ObjCorporateActionDataCollection.Clear();
                            foreach (var item in objCorpActBSE.Values)//objCorpActBSE.Values.Where(x =>(!x.Group.Equals(null)) && (!x.Group.Equals(string.Empty)) && (!x.Group.Contains("F,G,FC,GC"))))
                            {
                                if (item != null)
                                {
                                    if (!string.IsNullOrEmpty(item.Group) && new[] { "F", "G", "FC", "GC" }.Any(x => x == item.Group.Trim()))
                                    {
                                        CorporateActionModel objCorpAct = new CorporateActionModel();

                                        if (item.bcOrRdFlag == "BC")
                                        {
                                            objCorpAct.ScripID = item.ScripID;
                                            objCorpAct.scripCode = item.scripCode;
                                            objCorpAct.Group = item.Group;
                                            objCorpAct.purposeOrEvent = item.purposeOrEvent;
                                            // objCorpAct.exDate = item.exDate;
                                            objCorpAct.exDate = DateTime.ParseExact(item?.exDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("dd-MM-yyyy");//item.exDate;
                                            objCorpAct.exDateSort = item.exDate;
                                            objCorpAct.bookClosureFrom = item.bookClosureFrom;
                                            objCorpAct.bookClosureTo = item.bookClosureTo;
                                            objCorpAct.NdStartDate = item.NdStartDate;
                                            objCorpAct.ndEndDate = item.ndEndDate;
                                            objCorpAct.bcOrRdFlag = item.bcOrRdFlag;
                                        }
                                        else if (item.bcOrRdFlag == "RD")
                                        {
                                            objCorpAct.ScripID = item.ScripID;
                                            objCorpAct.scripCode = item.scripCode;
                                            objCorpAct.Group = item.Group;
                                            objCorpAct.purposeOrEvent = item.purposeOrEvent;
                                            //objCorpAct.exDate = item.exDate;
                                            objCorpAct.exDate = DateTime.ParseExact(item?.exDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("dd-MM-yyyy");//item.exDate;
                                            objCorpAct.exDateSort = item.exDate;
                                            objCorpAct.recordDate = DateTime.ParseExact(item?.recordDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("dd-MM-yyyy");//item.recordDate;
                                            objCorpAct.recordDateSort = item.recordDate;
                                            objCorpAct.NdStartDate = item.NdStartDate;
                                            objCorpAct.ndEndDate = item.ndEndDate;
                                            objCorpAct.bcOrRdFlag = item.bcOrRdFlag;
                                        }
                                        ObjCorporateActionDataCollection.Add(objCorpAct);
                                    }
                                }
                            }
                            ObjCorporateActionDataCollection.OrderBy(x => x.exDate);
                            TitleCorporate = "Detailed Corporate Action - Count: " + ObjCorporateActionDataCollection.Count;
                            TempCorporateActionDataCollection = ObjCorporateActionDataCollection;
                        }
                        else if (segment == "All")
                        {
                            ObjCorporateActionDataCollection.Clear();
                            try
                            {
                                foreach (var item in objCorpActBSE.Values)
                                {
                                    CorporateActionModel objCorpAct = new CorporateActionModel();
                                    if (item.bcOrRdFlag == "BC")
                                    {
                                        objCorpAct.ScripID = item.ScripID;
                                        objCorpAct.scripCode = item.scripCode;
                                        objCorpAct.Group = item.Group;
                                        objCorpAct.purposeOrEvent = item.purposeOrEvent;
                                        //objCorpAct.exDate = item.exDate;
                                        objCorpAct.exDate = DateTime.ParseExact(item?.exDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("dd-MM-yyyy");//item.exDate;
                                        objCorpAct.exDateSort = item.exDate;
                                        objCorpAct.bookClosureFrom = item.bookClosureFrom;
                                        objCorpAct.bookClosureTo = item.bookClosureTo;
                                        objCorpAct.NdStartDate = item.NdStartDate;
                                        objCorpAct.ndEndDate = item.ndEndDate;
                                        objCorpAct.bcOrRdFlag = item.bcOrRdFlag;
                                    }
                                    else if (item.bcOrRdFlag == "RD")
                                    {
                                        objCorpAct.ScripID = item.ScripID;
                                        objCorpAct.scripCode = item.scripCode;
                                        objCorpAct.Group = item.Group;
                                        objCorpAct.purposeOrEvent = item.purposeOrEvent;
                                        //objCorpAct.exDate = item.exDate;
                                        objCorpAct.exDate = DateTime.ParseExact(item?.exDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("dd-MM-yyyy");//item.exDate;
                                        objCorpAct.exDateSort = item.exDate;
                                        objCorpAct.recordDate = DateTime.ParseExact(item?.recordDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("dd-MM-yyyy");//item.recordDate;
                                        objCorpAct.recordDateSort = item.recordDate;
                                        objCorpAct.NdStartDate = item.NdStartDate;
                                        objCorpAct.ndEndDate = item.ndEndDate;
                                        objCorpAct.bcOrRdFlag = item.bcOrRdFlag;
                                    }
                                    ObjCorporateActionDataCollection.Add(objCorpAct);
                                }
                                ObjCorporateActionDataCollection.OrderBy(x => x.exDate);
                                TitleCorporate = "Detailed Corporate Action - Count: " + ObjCorporateActionDataCollection.Count;
                                TempCorporateActionDataCollection = ObjCorporateActionDataCollection;
                            }
                            catch (Exception ex)
                            {
                                ExceptionUtility.LogError(ex);
                            }
                        }
                    }
                    break;
            }
            //ObjCorporateActionDataCollection.OrderBy(x => x.exDate);
            #region Sort
            ObjCorporateActionDataCollection = new ObservableCollection<CorporateActionModel>(ObjCorporateActionDataCollection.OrderBy(p => p.exDateSort));
            //var sortedDatelst1 = ObjCorporateActionDataCollection.OrderBy(x => x.exDate).ToList();
            //var sortedDatelst = DisplayDateInSortedOrder(ObjCorporateActionDataCollection);
            //ObjCorporateActionDataCollection.Clear();
            //ObjCorporateActionDataCollection = (ObservableCollection<CorporateActionModel>)sortedDatelst;//(ObservableCollection<CorporateActionModel>)
            #endregion
            NotifyPropertyChanged("ObjCorporateActionDataCollection");
        }

        private object DisplayDateInSortedOrder(ObservableCollection<CorporateActionModel> sortedDatelst1)
        {
            int count = 0;
            ObservableCollection<CorporateActionModel> sortedList = new ObservableCollection<CorporateActionModel>();
            // ObservableCollection<CorporateActionModel> orderedList = new ObservableCollection<CorporateActionModel>();
            try
            {
                foreach (var item in sortedDatelst1)//.OrderBy(x => x.exDate) .Select(x => (DateTime.ParseExact(item., "dd-MMM-yyyy", CultureInfo.InvariantCulture).ToString("yy-MM-dd"))).Distinct())
                {
                    CorporateActionModel objCorpAct = new CorporateActionModel();
                    if (item.bcOrRdFlag == "BC")
                    {
                        objCorpAct.ScripID = item.ScripID;
                        objCorpAct.scripCode = item.scripCode;
                        objCorpAct.Group = item.Group;
                        objCorpAct.purposeOrEvent = item.purposeOrEvent;
                        objCorpAct.exDate = DateTime.ParseExact(item?.exDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("dd-MM-yyyy");
                        objCorpAct.bookClosureFrom = item.bookClosureFrom;
                        objCorpAct.bookClosureTo = item.bookClosureTo;
                        objCorpAct.NdStartDate = item.NdStartDate;
                        objCorpAct.ndEndDate = item.ndEndDate;
                        objCorpAct.bcOrRdFlag = item.bcOrRdFlag;

                    }
                    else if (item.bcOrRdFlag == "RD")
                    {
                        objCorpAct.ScripID = item.ScripID;
                        objCorpAct.scripCode = item.scripCode;
                        objCorpAct.Group = item.Group;
                        objCorpAct.purposeOrEvent = item.purposeOrEvent;
                        objCorpAct.exDate = DateTime.ParseExact(item?.exDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("dd-MM-yyyy");
                        objCorpAct.recordDate = DateTime.ParseExact(item.recordDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("dd-MM-yyyy");//item.recordDate;
                        objCorpAct.NdStartDate = item.NdStartDate;
                        objCorpAct.ndEndDate = item.ndEndDate;
                        objCorpAct.bcOrRdFlag = item.bcOrRdFlag;
                    }
                    sortedList.Add(objCorpAct);
                    count++;
                }
            }
            catch (Exception ex)
            {
                // MessageBox.Show("error in record" + count);
            }

            //foreach (var item in sortedList.OrderBy(x=>x.exDate))
            //{
            //    orderedList.Add(item);
            //}
            //orderedList = orderedList.OrderBy(x=>x.exDate).ToList();
            //orderedList = sortedList.Cast<CorporateActionModel>().OrderBy(x=>x.exDate).ToList();//sortedList.OrderBy(x => x.exDate);
            //return orderedList;
            //ObservableCollection<CorporateActionModel> orderedList = new ObservableCollection<CorporateActionModel>(sortedList);
            return sortedList;
        }

        //private ObservableCollection<CorporateActionModel> DisplayDateInSortedOrder(ObservableCollection<CorporateActionModel> ExpDate)
        //{
        //    ObservableCollection<CorporateActionModel> sortedList = new ObservableCollection<CorporateActionModel>();
        //    // foreach (var item in ExpDate)//.Select(x => (DateTime.ParseExact(item., "dd-MMM-yyyy", CultureInfo.InvariantCulture).ToString("yy-MM-dd"))).Distinct())
        //    //if (!(item == " All" || item == "All"))
        //    //    sortedList.Add(DateTime.ParseExact(item, "yy-MM-dd", CultureInfo.InvariantCulture).ToString("dd-MM-yy"));
        //    //else
        //    //    sortedList.Add(item);
        //    return sortedList;
        //}


        private ObservableCollection<CorporateActionModel> DisplayDateInComboBoxddMMMyyyy(ObservableCollection<CorporateActionModel> ExpDate)
        {
            ObservableCollection<CorporateActionModel> sortedList = new ObservableCollection<CorporateActionModel>();
            //ObservableCollection<string> sortedList1 = new ObservableCollection<string>();
            //foreach (var item in ExpDate)//.Select(x => (DateTime.ParseExact(item., "dd-MMM-yyyy", CultureInfo.InvariantCulture).ToString("yy-MM-dd"))).Distinct())
            //    if (!(item == " All" || item == "All"))
            //        sortedList.Add(DateTime.ParseExact(item, "yy-MMM-dd", CultureInfo.InvariantCulture).ToString("yy-MM-dd"));
            //    else
            //        sortedList.Add(item);
            //sortedList = new ObservableCollection<string>(sortedList.OrderBy(p => p));
            //sortedList1 = DisplayDateInComboBox(sortedList);
            //sortedList.Clear();
            //foreach (var item in sortedList1)//.Select(x => (DateTime.ParseExact(item., "dd-MMM-yyyy", CultureInfo.InvariantCulture).ToString("yy-MM-dd"))).Distinct())
            //    if (!(item == " All" || item == "All"))
            //        sortedList.Add(DateTime.ParseExact(item, "dd-MM-yy", CultureInfo.InvariantCulture).ToString("dd-MM-yy"));
            //    else
            //        sortedList.Add(item);
            return sortedList;
        }

        #region Detail Corporate Action Window
        // Open full purpose name window on button click
        private RelayCommand _DisplayFullName;
        public RelayCommand DisplayFullName
        {
            get
            {
                return _DisplayFullName ?? (_DisplayFullName = new RelayCommand(
                    (object e) => OpenPurposeFullName()
                        ));
            }
        }

        private void OpenPurposeFullName()
        {
            DetailedCorporateAction dCorpAct = new DetailedCorporateAction();
            dCorpAct.Show();
            dCorpAct.Focus();
        }
        #endregion

        private RelayCommand _myLocationChanged;

        public RelayCommand myLocationChanged
        {
            get
            {
                return _myLocationChanged ?? (_myLocationChanged = new RelayCommand(
                    (object e) => Windows_CorporateActionLocationChanged(e)));

            }
        }

        private void Windows_CorporateActionLocationChanged(object e)
        {
            if (CommonFrontEnd.SharedMemories.ConfigurationMasterMemory.ConfigurationDict.ContainsKey("WindowsPosition"))
            {
                BoltAppSettingsWindowsPosition oBoltAppSettingsWindowsPosition = (BoltAppSettingsWindowsPosition)CommonFrontEnd.SharedMemories.ConfigurationMasterMemory.ConfigurationDict["WindowsPosition"];
                if (oBoltAppSettingsWindowsPosition != null && oBoltAppSettingsWindowsPosition.ScrpHelpCorpAction != null && oBoltAppSettingsWindowsPosition.ScrpHelpCorpAction.WNDPOSITION != null)
                {
                    oBoltAppSettingsWindowsPosition.ScrpHelpCorpAction.WNDPOSITION.Left = Convert.ToInt32(LeftPosition);
                    oBoltAppSettingsWindowsPosition.ScrpHelpCorpAction.WNDPOSITION.Top = Convert.ToInt32(TopPosition);
                    oBoltAppSettingsWindowsPosition.ScrpHelpCorpAction.WNDPOSITION.Right = Convert.ToInt32(CorpActWidth);
                    oBoltAppSettingsWindowsPosition.ScrpHelpCorpAction.WNDPOSITION.Down = Convert.ToInt32(CorpActHeight);
                }
                //CommonFrontEnd.SharedMemories.SaveConfiguration.SaveUserConfiguration(@"D:\TWS_DotNetNewStructure\TWS_DOTNETT\CommonFrontEnd\bin\Debug\xml\Users\AppSettings\920200000.xml", "WindowsPosition");
                CommonFrontEnd.SharedMemories.SaveConfiguration.SaveUserConfiguration(CommonFrontEnd.SharedMemories.SettingsManager.AppSettingsXmlPath, "WindowsPosition");
            }

        }


        private RelayCommand _CorpActClosing;

        public RelayCommand CorpActClosing
        {
            get { return _CorpActClosing ?? (_CorpActClosing = new RelayCommand((object e) => CorpActClosing_Closing(e))); }
        }

        private void CorpActClosing_Closing(object e)
        {
            Windows_CorporateActionLocationChanged(e);
            //if (CommonFrontEnd.Processor.UMSProcessor.OnTradeReceivedCW != null)
            //CommonFrontEnd.Processor.UMSProcessor.OnTradeReceivedCW -= NetPositionMemory.UpdateClientNetPosition;

            //if (UMSProcessor.OnTradeCWReceived != null)
            //    UMSProcessor.OnTradeCWReceived -= UpdateHeader;

            //NetPositionClientWise oNetPositionClientWise = System.Windows.Application.Current.Windows.OfType<NetPositionClientWise>().FirstOrDefault();
            //if (oNetPositionClientWise != null)
            //{
            //    if (CommonFrontEnd.SharedMemories.ConfigurationMasterMemory.ConfigurationDict.ContainsKey("WindowsPosition"))
            //    {
            //        BoltAppSettingsWindowsPosition oBoltAppSettingsWindowsPosition = (BoltAppSettingsWindowsPosition)CommonFrontEnd.SharedMemories.ConfigurationMasterMemory.ConfigurationDict["WindowsPosition"];
            //        if (oBoltAppSettingsWindowsPosition != null && oBoltAppSettingsWindowsPosition.NETPOSITIONCLIENTWISE != null && oBoltAppSettingsWindowsPosition.NETPOSITIONCLIENTWISE.WNDPOSITION != null)
            //        {
            //            oBoltAppSettingsWindowsPosition.NETPOSITIONCLIENTWISE.WNDPOSITION.Left = Convert.ToInt32(LeftPosition);
            //            oBoltAppSettingsWindowsPosition.NETPOSITIONCLIENTWISE.WNDPOSITION.Top = Convert.ToInt32(TopPosition);
            //            oBoltAppSettingsWindowsPosition.NETPOSITIONCLIENTWISE.WNDPOSITION.Right = Convert.ToInt32(Width);
            //            oBoltAppSettingsWindowsPosition.NETPOSITIONCLIENTWISE.WNDPOSITION.Down = Convert.ToInt32(Height);
            //        }
            //        //CommonFrontEnd.SharedMemories.SaveConfiguration.SaveUserConfiguration(@"D:\TWS_DotNetNewStructure\TWS_DOTNETT\CommonFrontEnd\bin\Debug\xml\Users\AppSettings\920200000.xml", "WindowsPosition");
            //        CommonFrontEnd.SharedMemories.SaveConfiguration.SaveUserConfiguration(CommonFrontEnd.SharedMemories.SettingsManager.AppSettingsXmlPath, "WindowsPosition");
            //    }
            //    //  oNetPositionClientWise.Hide();
            //}

        }

        #region NotifyProperty
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
}
