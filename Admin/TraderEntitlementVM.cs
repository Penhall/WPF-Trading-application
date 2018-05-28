using CommonFrontEnd.Common;
using CommonFrontEnd.Model.Admin;
using CommonFrontEnd.SharedMemories;
using CommonFrontEnd.View.Admin;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static CommonFrontEnd.SharedMemories.DataAccessLayer;

namespace CommonFrontEnd.ViewModel.Admin
{
    class TraderEntitlementVM : BaseViewModel
    {

        #region Properties
        private string str = string.Empty;
        public static DataAccessLayer oDataAccessLayer = null;
        TraderEntitlementModel oTEModel = new TraderEntitlementModel();

        private ObservableCollection<TraderEntitlementModel> _TraderEntitlementCollection = new ObservableCollection<TraderEntitlementModel>();
        public ObservableCollection<TraderEntitlementModel> TraderEntitlementCollection
        {
            get { return _TraderEntitlementCollection; }
            set
            {
                _TraderEntitlementCollection = value;
                NotifyPropertyChanged("TraderEntitlementCollection");

            }
        }

        private ObservableCollection<TraderEntitlementModel> _FrmTraderEntitlementCollection = new ObservableCollection<TraderEntitlementModel>();
        public ObservableCollection<TraderEntitlementModel> FrmTraderEntitlementCollection
        {
            get { return _FrmTraderEntitlementCollection; }
            set
            {
                _FrmTraderEntitlementCollection = value;
                NotifyPropertyChanged("FrmTraderEntitlementCollection");

            }
        }

        private ObservableCollection<TraderEntitlementModel> _NewTraderEntitlementCollection = new ObservableCollection<TraderEntitlementModel>();
             public ObservableCollection<TraderEntitlementModel> NewTraderEntitlementCollection
        {
            get { return _NewTraderEntitlementCollection; }
            set
            {
                _NewTraderEntitlementCollection = value;
                NotifyPropertyChanged("NewTraderEntitlementCollection");
                FilterPopulation();
            }
        }

    
        private ObservableCollection<TraderEntitlementModel> _CopyFrmTraderEntitlementCollection = new ObservableCollection<TraderEntitlementModel>();
        public ObservableCollection<TraderEntitlementModel> CopyFrmTraderEntitlementCollection
        {
            get { return _CopyFrmTraderEntitlementCollection; }
            set
            {
                _CopyFrmTraderEntitlementCollection = value;
                NotifyPropertyChanged("CopyFrmTraderEntitlementCollection");
                FilterPopulation();
            }
        }
        private List<string> _TraderADDUpdate;

        public List<string> TraderADDUpdate
        {
            get { return _TraderADDUpdate; }
            set
            {
                _TraderADDUpdate = value;
                NotifyPropertyChanged("TraderADDUpdate");
            }
        }



        private List<string> _TraderIDList;

        public List<string> TraderIDList
        {
            get { return _TraderIDList; }
            set
            {
                _TraderIDList = value;
                NotifyPropertyChanged("TraderIDList");
            }
        }
        private List<string> _GroupList;

        public List<string> GroupList
        {
            get { return _GroupList; }
            set
            {
                _GroupList = value;
                NotifyPropertyChanged("GroupList");
            }
        }
        private List<string> _BranchList;

        public List<string> BranchList
        {
            get { return _BranchList; }
            set
            {
                _BranchList = value;
                NotifyPropertyChanged("BranchList");
            }
        }
        private List<string> _ClientviewonTraderList;

        public List<string> ClientviewonTraderList
        {
            get { return _ClientviewonTraderList; }
            set
            {
                _ClientviewonTraderList = value;
                NotifyPropertyChanged("ClientviewonTraderlist");
            }
        }

              private List<string> _ViewClientOnLevelList;

        public List<string> ViewClientOnLevelList
        {
            get { return _ViewClientOnLevelList; }
            set
            {
                _ViewClientOnLevelList = value;
                NotifyPropertyChanged("ViewClientOnLevelList");
            }
        }

        private List<string> _OrderPlacementOutClientMasterList;

        public List<string> OrderPlacementOutClientMasterList
        {
            get { return _OrderPlacementOutClientMasterList; }
            set
            {
                _OrderPlacementOutClientMasterList = value;
                NotifyPropertyChanged("OrderPlacementOutClientMasterList");
            }
        }

        private TraderEntitlementModel _TESelectedItem;
        public TraderEntitlementModel TESelectedItem
        {
            get { return _TESelectedItem; }
            set
            {
                _TESelectedItem = value;
                NotifyPropertyChanged("TESelectedItem");
            }
        }

        private string _SelectedTraderADDUpdate;

        public string SelectedTraderADDUpdate
        {
            get { return _SelectedTraderADDUpdate; }
            set
            {
                _SelectedTraderADDUpdate = value;
                NotifyPropertyChanged("SelectedTraderADDUpdate");
                ModifyAddVisibilty();
            }
        }

        private long _RecordCount;
        public long RecordCount
        {
            get { return _RecordCount; }
            set
            {
                _RecordCount = value;
                NotifyPropertyChanged("RecordCount");

            }
        }
        private String _VBranch;
        public String VBranch
        {
            get { return _VBranch; }
            set
            {
                _VBranch = value;
                NotifyPropertyChanged("VBranch");

            }
        }


        private string _TraderFilter;
        public string TraderFilter
        {
            get { return _TraderFilter; }
            set
            {
                _TraderFilter = value;
                NotifyPropertyChanged("TraderFilter");

            }
        }

        private string _GroupFilter;
        public string GroupFilter
        {
            get { return _GroupFilter; }
            set
            {
                _GroupFilter = value;
                NotifyPropertyChanged("GroupFilter");

            }
        }

        private string _BranchFilter;
        public string BranchFilter
        {
            get { return _BranchFilter; }
            set
            {
                _BranchFilter = value;
                NotifyPropertyChanged("BranchFilter");

            }
        }

        private String _VGroup;
        public String VGroup
        {
            get { return _VGroup; }
            set
            {
                _VGroup = value;
                NotifyPropertyChanged("VGroup");

            }
        }
        private string _VTraderId;
        public string VTraderId
        {
            get { return _VTraderId; }
            set
            {
                _VTraderId = value;
                NotifyPropertyChanged("VTraderId");

            }
        }

            private string _Replytxt;
        public string Replytxt
        {
            get { return _Replytxt; }
            set
            {
                _Replytxt = value;
                NotifyPropertyChanged("Replytxt");

            }
        }
        private string _ClientViewOnTrader;
        public string ClientViewOnTrader
        {
            get { return _ClientViewOnTrader; }
            set
            {
                _ClientViewOnTrader = value;
                NotifyPropertyChanged("ClientViewOnTrader");

            }
        }

            private string _OrderPlacementOutClientMaster;
        public string OrderPlacementOutClientMaster
        {
            get { return _OrderPlacementOutClientMaster; }
            set
            {
                _OrderPlacementOutClientMaster = value;
                NotifyPropertyChanged("OrderPlacementOutClientMaster");

            }
        }

                  private string _ViewClientOnLevel;
        public string ViewClientOnLevel
        {
            get { return _ViewClientOnLevel; }
            set
            {
                _ViewClientOnLevel = value;
                NotifyPropertyChanged("ViewClientOnLevel");

            }
        }

                private bool _I4LBlock;
        public bool I4LBlock
        {
            get { return _I4LBlock; }
            set
            {
                _I4LBlock = value;
                NotifyPropertyChanged("I4LBlock");

            }
        }

        private bool _I6LBlock;
        public bool I6LBlock
        {
            get { return _I6LBlock; }
            set
            {
                _I6LBlock = value;
                NotifyPropertyChanged("I6LBlock");

            }
        }


            private bool _AuctionBlock;
        public bool AuctionBlock
        {
            get { return _AuctionBlock; }
            set
            {
                _AuctionBlock = value;
                NotifyPropertyChanged("AuctionBlock");

            }
        }
        private bool _BlockDealBlock;
        public bool BlockDealBlock
        {
            get { return _BlockDealBlock; }
            set
            {
                _BlockDealBlock = value;
                NotifyPropertyChanged("BlockDealBlock");

            }
        }


 private bool _OddLotBlock;
        public bool OddLotBlock
        {
            get { return _OddLotBlock; }
            set
            {
                _OddLotBlock = value;
                NotifyPropertyChanged("OddLotBlock");

            }
        }


            private bool _InstitutionalTradingBlock;
        public bool InstitutionalTradingBlock
        {
            get { return _InstitutionalTradingBlock; }
            set
            {
                _InstitutionalTradingBlock = value;
                NotifyPropertyChanged("InstitutionalTradingBlock");

            }
        }

        private bool _Revert;
        public bool Revert
        {
            get { return _Revert; }
            set
            {
                _Revert = value;
                NotifyPropertyChanged("Revert");

            }
        }

     private bool _NotApplicable;
        public bool NotApplicable
        {
            get { return _NotApplicable; }
            set
            {
                _NotApplicable = value;
                NotifyPropertyChanged("NotApplicable");

            }
        }


        private string _ADDVis;
        public string ADDVis
        {
            get { return _ADDVis; }
            set
            {
                _ADDVis = value;
                NotifyPropertyChanged("ADDVis");

            }
        }

        private string _ModifyVis;
        public string ModifyVis
        {
            get { return _ModifyVis; }
            set
            {
                _ModifyVis = value;
                NotifyPropertyChanged("ModifyVis");

            }
        }

        private String _FrmTraderId;
        public String FrmTraderId
        {
            get { return _FrmTraderId; }
            set
            {
                _FrmTraderId = value;
                NotifyPropertyChanged("FrmTraderId");

            }
        }
        private String _ToTraderId;
        public String ToTraderId
        {
            get { return _ToTraderId; }
            set
            {
                _ToTraderId = value;
                NotifyPropertyChanged("ToTraderId");

            }
        }

        #endregion


        #region RelayCommand

        private RelayCommand _TraderEntitlementBack;

        public RelayCommand TraderEntitlementBack
        {
            get
            {
                return _TraderEntitlementBack ?? (_TraderEntitlementBack = new RelayCommand((object e) => TraderEntitlementBack_Click(e)));

            }
        }

        private RelayCommand _Submit;


        public RelayCommand Submit
        {
            get
            {
                return _Submit ?? (_Submit = new RelayCommand(
                    (object e) => ModelCreation()
                        ));
            }
        }
        private RelayCommand _OnModify;

        public RelayCommand OnModify
        {
            get
            {
                return _OnModify ?? (_OnModify = new RelayCommand(
                    (object e) => ModifyGrid()
                        ));
            }
        }
        private RelayCommand _CopyTrdrInfo;

        public RelayCommand CopyTrdrInfo
        {
            get
            {
                return _CopyTrdrInfo ?? (_CopyTrdrInfo = new RelayCommand(
                    (object e) => CopyInfoTrader()
                        ));
            }
        }



        private RelayCommand _Filter;
        public RelayCommand Filter
        {
            get
            {
                return _Filter ?? (_Filter = new RelayCommand(
                    (object e) => DataFilteration()
                        ));
                }
        }
        private RelayCommand _TraderEntitlementNext;

        public RelayCommand TraderEntitlementNext
                {
            get
            {
                return _TraderEntitlementNext ?? (_TraderEntitlementNext = new RelayCommand((object e) => TraderEntitlementNext_Click(e)));
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


        public bool ModifyFlag { get; private set; }
       



        #endregion
      /// <summary>
      /// Trader Entitlement functionality : added by Apoorva Sharma 10/05/2018
      /// </summary>
        public TraderEntitlementVM()
        {
            try
            {
                oDataAccessLayer = new DataAccessLayer();
                oDataAccessLayer.Connect((int)DataAccessLayer.ConnectionDB.TraderEntitlement);
                PopulateData();
                TraderADDUpdate = new List<string>();
                PopulateAddModify();
                PopulateClientonTrader();
                PopulateViewClientOnLevel();
                PopulateOrderPlacementRights();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        private void TraderEntitlementNext_Click(object e)
        {
            try

            {
                TraderEntitlementMenuVM.ClientMasterWindow_Click(e);

            }
            catch (Exception)
            {

                throw;
            }
        }

        public static void TraderEntitlementBack_Click(object e)
        {
            try
            {
                TraderEntitlementMenuVM.ClientMasterWindow_Click(e);
                TraderEntitlement oTraderEntitlement = System.Windows.Application.Current.Windows.OfType<TraderEntitlement>().FirstOrDefault();
                TraderEntitlementMenu oTraderEntitlementMenu = System.Windows.Application.Current.Windows.OfType<TraderEntitlementMenu>().FirstOrDefault();
                if (oTraderEntitlementMenu != null)
                {
                    oTraderEntitlementMenu.Show();
                    oTraderEntitlementMenu.Focus();
                    oTraderEntitlement.Hide();
                }
                else
                {
                    oTraderEntitlementMenu = new TraderEntitlementMenu();
                    oTraderEntitlementMenu.Owner = Application.Current.MainWindow;
                    oTraderEntitlementMenu.Activate();
                    oTraderEntitlementMenu.Show();
                    oTraderEntitlement.Hide();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        //private void TraderEntitlementNext_Click(object e)
        //{
        //    TraderEntitlementMenuVM.ClientMasterWindow_Click(e);
        //}
        private void PopulateAddModify()
        {
            TraderADDUpdate.Clear();
            TraderADDUpdate.Add("Add");
            TraderADDUpdate.Add("Modify");
            SelectedTraderADDUpdate = "Add";

        }

        private void ModifyAddVisibilty()
        {
            if (SelectedTraderADDUpdate == "Add")
            {
                ModifyVis = "Hidden";
                ADDVis = "Visible";
            }
            if (SelectedTraderADDUpdate == "Modify")
            {
                ModifyVis = "Visible";
                ADDVis = "Hidden";

            }
        }
        private void PopulateClientonTrader()
        {
            ClientviewonTraderList = new List<string>();
            ClientviewonTraderList.Add("Only Admin Client");
            ClientviewonTraderList.Add("Admin + Trader Profiled");
            ClientviewonTraderList.Add("Only Trader Profiled");
            ClientViewOnTrader = ClientviewonTraderList[0];
        }
        private void PopulateViewClientOnLevel()
        {
            ViewClientOnLevelList = new List<string>();
            ViewClientOnLevelList.Add("All Clients");
            ViewClientOnLevelList.Add("Only Mapped Clients");
            ViewClientOnLevel = ViewClientOnLevelList[0];
        }


             private void PopulateOrderPlacementRights()
        {
            OrderPlacementOutClientMasterList = new List<string>();
            OrderPlacementOutClientMasterList.Add("Warning");
            OrderPlacementOutClientMasterList.Add("Not Allowed");
            OrderPlacementOutClientMasterList.Add("No Restriction");
            OrderPlacementOutClientMaster = OrderPlacementOutClientMasterList[0];
        }
        private void PopulateData()
        {
            try
            {
                oDataAccessLayer.OpenConnection((int)DataAccessLayer.ConnectionDB.TraderEntitlement);

                string strQuery = @"SELECT Branch,TraderGroup,TraderId,ClientViewOnTraderTerminal,OrderPlacementOutClientMaster,ViewClientOnLevel,I4LBlock,I6LBlock,Filler,AuctionBlock,BlockDealBlock,OddLotBlock,InstitutionalTradingBlock, NotApplicable,RevertFromTrader FROM TRADER_ENTITLEMENT";

                SQLiteDataReader oSQLiteDataReader = oDataAccessLayer.ExecuteDataReader((int)ConnectionDB.TraderEntitlement,strQuery, System.Data.CommandType.Text, null);
                while (oSQLiteDataReader.Read())
                {
                    TraderEntitlementModel oTraderEntitlementModel = new TraderEntitlementModel();
                    oTraderEntitlementModel.Branch = oSQLiteDataReader["Branch"].ToString();
                    oTraderEntitlementModel.TraderGroup = oSQLiteDataReader["TraderGroup"].ToString();
                    oTraderEntitlementModel.TraderId = Convert.ToString(oSQLiteDataReader["TraderId"]);
                    oTraderEntitlementModel.TraderIdSort = Convert.ToInt32(oTraderEntitlementModel.TraderId);
                   
                    oTraderEntitlementModel.ClientView = Convert.ToString(oSQLiteDataReader["ClientViewOnTraderTerminal"]);
                    oTraderEntitlementModel.OrderRights = oSQLiteDataReader["OrderPlacementOutClientMaster"].ToString();
                    oTraderEntitlementModel.ClientViewAT = oSQLiteDataReader["ViewClientOnLevel"].ToString();
                    oTraderEntitlementModel.FourLBlock = Convert.ToBoolean(oSQLiteDataReader["I4LBlock"]);
                    oTraderEntitlementModel.SixLBlock = Convert.ToBoolean(oSQLiteDataReader["I6LBlock"]);
                    oTraderEntitlementModel.AuctionBlock = Convert.ToBoolean(oSQLiteDataReader["AuctionBlock"]);
                    oTraderEntitlementModel.BlockDealBlock = Convert.ToBoolean(oSQLiteDataReader["BlockDealBlock"]);
                    oTraderEntitlementModel.ODDlotBlock = Convert.ToBoolean(oSQLiteDataReader["OddLotBlock"]);
                    oTraderEntitlementModel.InstTradingBlock = Convert.ToBoolean(oSQLiteDataReader["InstitutionalTradingBlock"]);
                    oTraderEntitlementModel.NotApplicable = Convert.ToBoolean(oSQLiteDataReader["NotApplicable"].ToString());

                    TraderEntitlementCollection.Add(oTraderEntitlementModel);
                }
               TraderEntitlementCollection = new ObservableCollection<TraderEntitlementModel>(TraderEntitlementCollection.OrderBy(i => i.TraderIdSort));
                RecordCount = TraderEntitlementCollection.Count;
                NewTraderEntitlementCollection = TraderEntitlementCollection;
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
            }
            finally
            {
                oDataAccessLayer.CloseConnection((int)DataAccessLayer.ConnectionDB.TraderEntitlement);
            }
        }
        private void Reset()
        {
            TraderFilter = "All";
            GroupFilter = "All";
            BranchFilter = "All";
            TraderEntitlementCollection = NewTraderEntitlementCollection;

        }
        /// <summary>
        /// Data Grid Filters: Added By Apoorva Sharma 7/05/2018
        /// </summary>
        private void FilterPopulation()
        {
            TraderIDList = new List<string>();
            GroupList = new List<string>();
            BranchList = new List<string>();
            if (NewTraderEntitlementCollection != null)
            {

                TraderIDList = NewTraderEntitlementCollection.Select(x => x.TraderId.ToString()).Distinct().ToList();

                TraderIDList.Insert(0, "All");
             
                TraderFilter = TraderIDList[0];

                GroupList = NewTraderEntitlementCollection.Select(x => x.TraderGroup).Distinct().ToList();
                GroupList.Insert(0, "All");
           
                GroupFilter = GroupList[0];

                BranchList = NewTraderEntitlementCollection.Select(x => x.Branch).Distinct().ToList();
                BranchList.Insert(0, "All");
            
                BranchFilter = BranchList[0];
            }
        }
        private void DataFilteration()
        {

            if (NewTraderEntitlementCollection.Count > 0)
            {
                TraderEntitlementCollection = new ObservableCollection<TraderEntitlementModel>();



                foreach (var item in NewTraderEntitlementCollection)
                {


                    if (TraderFilter == "All" || GroupFilter == "All" || BranchFilter == "All")
                    {
                        if (TraderFilter == "All" && GroupFilter == "All" && BranchFilter == "All")
                        {
                            TraderEntitlementCollection = NewTraderEntitlementCollection;
                        }
                        else if (TraderFilter == "All" && GroupFilter == "All" && BranchFilter != "All")
                        {
                            if (item.Branch == BranchFilter)
                                TraderEntitlementCollection.Add(item);
                        }
                        else if (TraderFilter == "All" && GroupFilter != "All" && BranchFilter == "All")
                        {
                            if (item.TraderGroup == GroupFilter)
                                TraderEntitlementCollection.Add(item);
                        }
                        else if (TraderFilter != "All" && GroupFilter == "All" && BranchFilter == "All")
                        {
                            if (item.TraderId.ToString() == TraderFilter)
                                TraderEntitlementCollection.Add(item);
                        }
                        else if (TraderFilter == "All" && GroupFilter != "All" && BranchFilter != "All")
                        {
                            if (item.Branch == BranchFilter && item.TraderGroup == GroupFilter)
                                TraderEntitlementCollection.Add(item);
                        }
                        else if (TraderFilter != "All" && GroupFilter != "All" && BranchFilter == "All")
                        {
                            if (item.TraderId.ToString() == TraderFilter && item.TraderGroup == GroupFilter)
                                TraderEntitlementCollection.Add(item);
                        }
                        else if (TraderFilter != "All" && GroupFilter == "All" && BranchFilter != "All")
                        {
                            if (item.TraderId.ToString() == TraderFilter && item.Branch == BranchFilter)
                                TraderEntitlementCollection.Add(item);
                        }
                    }

                    else if ((TraderFilter != "All" && GroupFilter != "All" && BranchFilter != "All") || (TraderFilter != "Unassigned" && GroupFilter != "Unassigned" && BranchFilter != "Unassigned"))
                    {
                        if (item.TraderId.ToString() == TraderFilter && item.TraderGroup == GroupFilter && item.Branch == BranchFilter)
                            TraderEntitlementCollection.Add(item);


                    }
                   
                }
            }
            RecordCount = TraderEntitlementCollection.Count;
        }

        private void UpdateDataGrid(object e)
        {
            if (TESelectedItem != null)
            {
                VBranch = TESelectedItem.Branch;
                VTraderId = TESelectedItem.TraderId;
                VGroup = TESelectedItem.TraderGroup;
                ClientViewOnTrader = TESelectedItem.ClientView;
                ViewClientOnLevel = TESelectedItem.ClientViewAT;
                AuctionBlock = TESelectedItem.AuctionBlock;
                I4LBlock = TESelectedItem.FourLBlock;
                I6LBlock = TESelectedItem.SixLBlock;
                InstitutionalTradingBlock = TESelectedItem.InstTradingBlock;
                OddLotBlock = TESelectedItem.ODDlotBlock;
                OrderPlacementOutClientMaster = TESelectedItem.OrderRights;
                BlockDealBlock = TESelectedItem.BlockDealBlock;
                NotApplicable = TESelectedItem.NotApplicable;
                Revert = TESelectedItem.Revert;
            }
        }
        /// <summary>
        /// Addition of new trader info :Added By Apoorva Sharma 7/05/2018
        /// </summary>
        private void ModelCreation()
        {
            try
            {
                Reset();
                bool NullCheckValue = NullCheck();
                if (NullCheckValue)
                {
                TraderEntitlementModel oTEModel = new TraderEntitlementModel();
                    oTEModel.Branch = VBranch?.ToString();
                    oTEModel.TraderId = VTraderId?.ToString();
                    oTEModel.TraderIdSort = Convert.ToInt32(oTEModel.TraderId);
                    oTEModel.TraderGroup = VGroup?.ToString();
                    oTEModel.ClientView = ClientViewOnTrader?.ToString();
                    oTEModel.ClientViewAT = ViewClientOnLevel?.ToString();
                oTEModel.AuctionBlock = AuctionBlock;
                oTEModel.FourLBlock = I4LBlock;
                oTEModel.SixLBlock = I6LBlock;
                oTEModel.InstTradingBlock = InstitutionalTradingBlock;
                oTEModel.ODDlotBlock = OddLotBlock;
                    oTEModel.OrderRights = OrderPlacementOutClientMaster.ToString(); ;
                oTEModel.BlockDealBlock = BlockDealBlock;
                oTEModel.NotApplicable = NotApplicable;
                    oTEModel.Revert = Revert;
                //check short traderid duplication case
                    if (SelectedTraderADDUpdate == "Add")
                    {
               if (TraderEntitlementCollection.Count > 0)
                {
                            if (TraderEntitlementCollection.Any(x => x.TraderId == VTraderId))
                    {
                                Replytxt = "The TraderId Already exist";
                                return;
                            }
                            else
                        {
                                ModifyFlag = false;
                                TraderEntitlementCollection.Add(oTEModel);

                                oDataAccessLayer.OpenConnection((int)DataAccessLayer.ConnectionDB.TraderEntitlement);

                                str = @"INSERT INTO TRADER_ENTITLEMENT(Branch, TraderGroup,
                                   TraderId,
                                   ClientViewOnTraderTerminal,
                                   OrderPlacementOutClientMaster,
                                   ViewClientOnLevel,
                                   I4LBlock,
                                   I6LBlock,
                                   Filler,
                                   AuctionBlock,
                                   BlockDealBlock,
                                   OddLotBlock,
                                   InstitutionalTradingBlock,
                                   NotApplicable,
                                   RevertFromTrader) VALUES( " + "'" + oTEModel.Branch + "'," + "'" + oTEModel.TraderGroup + "'," + "'" + oTEModel.TraderId + "'," + "'" + oTEModel.ClientView + "'," + "'" + oTEModel.OrderRights + "'," + "'" + oTEModel.ClientViewAT + "'," + "'" + oTEModel.FourLBlock + "'," + "'" + oTEModel.SixLBlock + "'," + "'" + "NA" + "'," + "'" + oTEModel.AuctionBlock + "'," + "'" + oTEModel.BlockDealBlock + "'," + "'" + oTEModel.ODDlotBlock + "'," + "'" + oTEModel.InstTradingBlock + "'," + "'" + oTEModel.NotApplicable + "'," + "'" + oTEModel.Revert + "'); ";

                                int result = oDataAccessLayer.ExecuteNonQuery((int)ConnectionDB.TraderEntitlement,str, System.Data.CommandType.Text, null);
                                Replytxt = "Trader added successfully";
                        }
                    }


                }
                    else if (SelectedTraderADDUpdate == "Modify")
                    {
                        ModifyGrid();
                    }
               //else if(TraderEntitlementCollection.Count==0)
                    // TraderEntitlementCollection.Add(oTEModel);
                    TraderEntitlementCollection = new ObservableCollection<TraderEntitlementModel>(TraderEntitlementCollection.OrderBy(i => i.TraderIdSort));
                    NewTraderEntitlementCollection = TraderEntitlementCollection;
                RecordCount = TraderEntitlementCollection.Count;
                //MasterSharedMemory.oDataAccessLayer.OpenConnection((int)DataAccessLayer.ConnectionDB.Masters);
                //int result = MasterSharedMemory.oDataAccessLayer.ExecuteNonQuery((int)ConnectionDB.Mastersstr, System.Data.CommandType.Text, null);
                //TODO:  insert and update in db

                }
            }

            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
            }
            finally
            {

                oDataAccessLayer.CloseConnection((int)DataAccessLayer.ConnectionDB.TraderEntitlement);
            }

        }

        private bool NullCheck()
        {
            try
            {
                //if (String.IsNullOrEmpty(VBranch))
                //{
                //    Replytxt = "Enter  Branch";
                //    return false;
                //}
                if (String.IsNullOrEmpty(VTraderId))
                {
                    Replytxt = "Enter TraderId";
                    return false;
                }
                if (String.IsNullOrEmpty(ClientViewOnTrader))
                {
                    Replytxt = "Enter ClientViewOnTrader";
                    return false;

                }
                if (String.IsNullOrEmpty(ViewClientOnLevel))
                {
                    Replytxt = "Enter  ViewClientOnLevel";
                    return false;
                }

                if (String.IsNullOrEmpty(OrderPlacementOutClientMaster))
                {
                    Replytxt = "Enter OrderPlacementOutClientMaster";
                    return false;
                }

            }
            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
            }
            return true;
        }
        /// <summary>
        /// Addition of existing trader info : Added By Apoorva Sharma 7/05/2018
        /// </summary>
        public void ModifyGrid()
        {
            try
            {
                Reset();
                bool NullCheckValue = NullCheck();
                if (NullCheckValue)
                {
                    TraderEntitlementModel oTEModel = new TraderEntitlementModel();

                    int index = TraderEntitlementCollection.IndexOf(TraderEntitlementCollection.Where(x => x.TraderId == VTraderId).FirstOrDefault());
                    if (TraderEntitlementCollection.Any(x => x.TraderId == VTraderId))
                    {
                        TraderEntitlementCollection.RemoveAt(index);

                    }
                    else
                    {
                        //    MessageBox.Show("Trader not found in Database", "!Warning!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                    oTEModel.Branch = VBranch.ToString();
                    oTEModel.TraderId = VTraderId.ToString();
                    oTEModel.TraderIdSort = Convert.ToInt32(oTEModel.TraderId);
                    oTEModel.TraderGroup = VGroup.ToString();
                    oTEModel.ClientView = ClientViewOnTrader.ToString();
                    oTEModel.ClientViewAT = ViewClientOnLevel.ToString();
                    oTEModel.AuctionBlock = AuctionBlock;
                    oTEModel.FourLBlock = I4LBlock;
                    oTEModel.SixLBlock = I6LBlock;
                    oTEModel.InstTradingBlock = InstitutionalTradingBlock;
                    oTEModel.ODDlotBlock = OddLotBlock;
                    oTEModel.OrderRights = OrderPlacementOutClientMaster.ToString();
                    oTEModel.BlockDealBlock = BlockDealBlock;
                    oTEModel.NotApplicable = NotApplicable;
                    oTEModel.Revert = Revert;

                    TraderEntitlementCollection.Add(oTEModel);
                    TraderEntitlementCollection = new ObservableCollection<TraderEntitlementModel>(TraderEntitlementCollection.OrderBy(i => i.TraderIdSort));
                    NewTraderEntitlementCollection = TraderEntitlementCollection;
                    RecordCount = TraderEntitlementCollection.Count();

                    oDataAccessLayer.OpenConnection((int)DataAccessLayer.ConnectionDB.TraderEntitlement);
                    str = @"UPDATE TRADER_ENTITLEMENT SET Branch = '" + VBranch + "', " +
         "TraderGroup = '" + VGroup + "', " +
         "TraderId = '" + VTraderId + "', " +
         "ClientViewOnTraderTerminal = '" + ClientViewOnTrader + "' ," +
         "OrderPlacementOutClientMaster = '" + OrderPlacementOutClientMaster + "', " +
         "ViewClientOnLevel = '" + ViewClientOnLevel + "' ," +
         "I4LBlock = '" + I4LBlock + "' ," +
         "I6LBlock = '" + I6LBlock + "' ," +
         "Filler = '" + "NA" + "' ," +
         "AuctionBlock = '" + AuctionBlock + "' ," +
         "BlockDealBlock = '" + BlockDealBlock + "' ," +
         "OddLotBlock = '" + OddLotBlock + "' ," +
         "InstitutionalTradingBlock = '" + InstitutionalTradingBlock + "' ," +
         "NotApplicable = '" + NotApplicable + "' ," +
         "RevertFromTrader = '" + Revert + "'" +
         "WHERE TraderId = '" + VTraderId + "'";
                    int result = oDataAccessLayer.ExecuteNonQuery((int)ConnectionDB.TraderEntitlement,str, System.Data.CommandType.Text, null);
                    Replytxt = "Trader Modified";
                }
            }
            catch (Exception ex)
            {

                ExceptionUtility.LogError(ex);
            }
            finally
            {
                oDataAccessLayer.CloseConnection((int)DataAccessLayer.ConnectionDB.TraderEntitlement);
        }

    }
        /// <summary>
        /// mirroring desired trader info to other traders : Added by Apoorva Sharma 8/05/2018
        /// </summary>
        private void CopyInfoTrader()
        {
            FrmTraderEntitlementCollection = new ObservableCollection<TraderEntitlementModel>();
          CopyFrmTraderEntitlementCollection = new ObservableCollection<TraderEntitlementModel>();
            CopyFrmTraderEntitlementCollection = TraderEntitlementCollection;
            try
            {
                if (FrmTraderId != null)
                {
                    string[] tokens = ToTraderId.Split(',');
                    int tokenLength = tokens.Length;

                    for (int i = 0; i < tokenLength; i++)
                    {
                        int index = CopyFrmTraderEntitlementCollection.IndexOf(CopyFrmTraderEntitlementCollection.Where(x => x.TraderId == tokens[i]).FirstOrDefault());
                        if (CopyFrmTraderEntitlementCollection.Any(x => x.TraderId == tokens[i]))
                        {
                            CopyFrmTraderEntitlementCollection.RemoveAt(index);
                        }
                    }


                    foreach (var item in CopyFrmTraderEntitlementCollection.Where(x => x.TraderId == FrmTraderId))
                    {
                        FrmTraderEntitlementCollection.Add(item);
                    }
                    // FrmTraderEntitlementCollection.Add(TraderEntitlementCollection.Where(x => x.TraderId == FrmTraderId).FirstOrDefault());


                    for (int i = 0; i < tokenLength; i++)
                    {

                        TraderEntitlementModel oTEModel = new TraderEntitlementModel();
                        oTEModel.Branch = FrmTraderEntitlementCollection[0].Branch.ToString();
                        oTEModel.TraderId = tokens[i].ToString();
                        oTEModel.TraderIdSort = Convert.ToInt32(oTEModel.TraderId);
                        oTEModel.TraderGroup = FrmTraderEntitlementCollection[0].TraderGroup.ToString();
                        oTEModel.ClientView = FrmTraderEntitlementCollection[0].ClientView.ToString();
                        oTEModel.ClientViewAT = FrmTraderEntitlementCollection[0].ClientViewAT.ToString();
                        oTEModel.AuctionBlock = FrmTraderEntitlementCollection[0].AuctionBlock;
                        oTEModel.FourLBlock = FrmTraderEntitlementCollection[0].FourLBlock;
                        oTEModel.SixLBlock = FrmTraderEntitlementCollection[0].SixLBlock;
                        oTEModel.InstTradingBlock = FrmTraderEntitlementCollection[0].InstTradingBlock;
                        oTEModel.ODDlotBlock = FrmTraderEntitlementCollection[0].ODDlotBlock;
                        oTEModel.OrderRights = FrmTraderEntitlementCollection[0].OrderRights.ToString();
                        oTEModel.BlockDealBlock = FrmTraderEntitlementCollection[0].BlockDealBlock;
                        oTEModel.NotApplicable = FrmTraderEntitlementCollection[0].NotApplicable;
                        oTEModel.Revert = FrmTraderEntitlementCollection[0].Revert;
                        CopyFrmTraderEntitlementCollection.Add(oTEModel);

                    }

                    CopyFrmTraderEntitlementCollection = new ObservableCollection<TraderEntitlementModel>(CopyFrmTraderEntitlementCollection.OrderBy(i => i.TraderIdSort));
                    
                   int res= InsertCopyTrdrInfo(CopyFrmTraderEntitlementCollection);
                    if (res > 0)
                    {
                        Replytxt = "Trader added successfully";
                        TraderEntitlementCollection = CopyFrmTraderEntitlementCollection;
                      
                    }
                    else
                        Replytxt = "Error while saving the trader infromation";
                    RecordCount = TraderEntitlementCollection.Count();
                    NewTraderEntitlementCollection = TraderEntitlementCollection;

                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
            }
            finally
            {
                oDataAccessLayer.CloseConnection((int)DataAccessLayer.ConnectionDB.TraderEntitlement);
            }
        }
        /// <summary>
        /// Inserting mirrored data into dB : Added by Apoorva Sharma 8/05/2018
        /// </summary>
        /// <param name="CopyFrmTraderEntitlementCollection"></param>
        /// <returns></returns>
        private int InsertCopyTrdrInfo(ObservableCollection<TraderEntitlementModel> CopyFrmTraderEntitlementCollection)
        {
            int result=0;
            try
            { 

                oDataAccessLayer.OpenConnection((int)DataAccessLayer.ConnectionDB.TraderEntitlement);
                str = @"DELETE FROM TRADER_ENTITLEMENT";
                result = oDataAccessLayer.ExecuteNonQuery((int)ConnectionDB.TraderEntitlement,str, System.Data.CommandType.Text, null);
                foreach (var item in CopyFrmTraderEntitlementCollection)
                { 
                    str = @"INSERT INTO TRADER_ENTITLEMENT(Branch, TraderGroup,
                                   TraderId,
                                   ClientViewOnTraderTerminal,
                                   OrderPlacementOutClientMaster,
                                   ViewClientOnLevel,
                                   I4LBlock,
                                   I6LBlock,
                                   Filler,
                                   AuctionBlock,
                                   BlockDealBlock,
                                   OddLotBlock,
                                   InstitutionalTradingBlock,
                                   NotApplicable,
                                   RevertFromTrader) VALUES( " + "'" + item.Branch + "'," + "'" + item.TraderGroup + "'," + "'" + item.TraderId + "'," + "'" + item.ClientView + "'," + "'" + item.OrderRights + "'," + "'" + item.ClientViewAT + "'," + "'" + item.FourLBlock + "'," + "'" + item.SixLBlock + "'," + "'" + "NA" + "'," + "'" + item.AuctionBlock + "'," + "'" + item.BlockDealBlock + "'," + "'" + item.ODDlotBlock + "'," + "'" + item.InstTradingBlock + "'," + "'" + item.NotApplicable + "'," + "'" + item.Revert + "'); ";

                    result = oDataAccessLayer.ExecuteNonQuery((int)ConnectionDB.TraderEntitlement,str, System.Data.CommandType.Text, null);
                }
                
                
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
            }
            finally
            {
                oDataAccessLayer.CloseConnection((int)DataAccessLayer.ConnectionDB.TraderEntitlement);
            }
            //return result;
            return result;
        }
    }


}
