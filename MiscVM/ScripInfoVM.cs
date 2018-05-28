using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonFrontEnd.Model;
using CommonFrontEnd.SharedMemories;
using SubscribeList;
using CommonFrontEnd.Common;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using CommonFrontEnd.View;
using System.Diagnostics;
using CommonFrontEnd.Global;
using CommonFrontEnd.Model.CorporateAction;

namespace CommonFrontEnd.ViewModel
{
    class ScripInfoVM : INotifyPropertyChanged
    {

        #region Properties
        static ScripInfo sWindow = null;
        private string _ScripID;
        public string ScripID
        {
            get { return _ScripID; }
            set { _ScripID = value; NotifyPropertyChanged(nameof(ScripID)); }
        }

        private string _Scode;
        public string Scode
        {
            get { return _Scode; }
            set { _Scode = value; NotifyPropertyChanged(nameof(Scode)); }
        }

        private string _ISIN;
        public string ISIN
        {
            get { return _ISIN; }
            set { _ISIN = value; NotifyPropertyChanged(nameof(ISIN)); }
        }

        private string _SName;
        public string SName
        {
            get { return _SName; }
            set { _SName = value; NotifyPropertyChanged(nameof(SName)); }
        }

        private string _FWeekHP;
        public string FWeekHP
        {
            get { return _FWeekHP; }
            set { _FWeekHP = value; NotifyPropertyChanged(nameof(FWeekHP)); }
        }

        private string _MktLotEq;
        public string MktLotEq
        {
            get { return _MktLotEq; }
            set { _MktLotEq = value; NotifyPropertyChanged(nameof(MktLotEq)); }
        }

        private string _TicketSizeEqt;
        public string TicketSizeEqt
        {
            get { return _TicketSizeEqt; }
            set { _TicketSizeEqt = value; NotifyPropertyChanged(nameof(TicketSizeEqt)); }
        }

        private string _FaceValue;
        public string FaceValue
        {
            get { return _FaceValue; }
            set { _FaceValue = value; NotifyPropertyChanged(nameof(FaceValue)); }
        }

        private string _FWeekLP;
        public string FWeekLP
        {
            get { return _FWeekLP; }
            set { _FWeekLP = value; NotifyPropertyChanged(nameof(FWeekLP)); }
        }

        private string _BseExc;
        public string BseExc
        {
            get { return _BseExc; }
            set { _BseExc = value; NotifyPropertyChanged(nameof(BseExc)); }
        }

        private string _ScrpGrp;
        public string ScrpGrp
        {
            get { return _ScrpGrp; }
            set { _ScrpGrp = value; NotifyPropertyChanged(nameof(ScrpGrp)); }
        }

        private string _GSM;
        public string GSM
        {
            get { return _GSM; }
            set { _GSM = value; NotifyPropertyChanged(nameof(GSM)); }
        }

        private string _Status;
        public string Status
        {
            get { return _Status; }
            set { _Status = value; NotifyPropertyChanged(nameof(Status)); }
        }

        private string _SettOptn;
        public string SettOptn
        {
            get { return _SettOptn; }
            set { _SettOptn = value; NotifyPropertyChanged(nameof(SettOptn)); }
        }

        private string _VARIM;
        public string VARIM
        {
            get { return _VARIM; }
            set { _VARIM = value; NotifyPropertyChanged(nameof(VARIM)); }
        }

        private string _VAREM;
        public string VAREM
        {
            get { return _VAREM; }
            set { _VAREM = value; NotifyPropertyChanged(nameof(VAREM)); }
        }

        private string _PartnID;
        public string PartnID
        {
            get { return _PartnID; }
            set { _PartnID = value; NotifyPropertyChanged(nameof(PartnID)); }
        }

        private string _ProdID;
        public string ProdID
        {
            get { return _ProdID; }
            set { _ProdID = value; NotifyPropertyChanged(nameof(ProdID)); }
        }

        private string _Purpose;
        public string Purpose
        {
            get { return _Purpose; }
            set { _Purpose = value; NotifyPropertyChanged(nameof(Purpose)); }
        }

        private string _ExDate;
        public string ExDate
        {
            get { return _ExDate; }
            set { _ExDate = value; NotifyPropertyChanged(nameof(ExDate)); }
        }

        private string _BCStartDate;
        public string BCStartDate
        {
            get { return _BCStartDate; }
            set { _BCStartDate = value; NotifyPropertyChanged(nameof(BCStartDate)); }
        }

        private string _BCEndDate;
        public string BCEndDate
        {
            get { return _BCEndDate; }
            set { _BCEndDate = value; NotifyPropertyChanged(nameof(BCEndDate)); }
        }

        private string _RecordDate;
        public string RecordDate
        {
            get { return _RecordDate; }
            set { _RecordDate = value; NotifyPropertyChanged(nameof(RecordDate)); }
        }

        private string _NDStartDate;
        public string NDStartDate
        {
            get { return _NDStartDate; }
            set { _NDStartDate = value; NotifyPropertyChanged(nameof(NDStartDate)); }
        }

        private string _NDEndDate;
        public string NDEndDate
        {
            get { return _NDEndDate; }
            set { _NDEndDate = value; NotifyPropertyChanged(nameof(NDEndDate)); }
        }

        private string _OnIndices;
        public string OnIndices
        {
            get { return _OnIndices; }
            set { _OnIndices = value; NotifyPropertyChanged(nameof(OnIndices)); }
        }

        private ObservableCollection<string> _IndicesMemory;
        public ObservableCollection<string> IndicesMemory
        {
            get { return _IndicesMemory; }
            set { _IndicesMemory = value; }
        }

        private ObservableCollection<CorporateActionModel> _CAInfoGrid;
        public ObservableCollection<CorporateActionModel> CAInfoGrid
        {
            get { return _CAInfoGrid; }
            set { _CAInfoGrid = value; NotifyPropertyChanged("CAInfoGrid"); }
        }
        

        private RelayCommand _AllCorpClick;
        public RelayCommand AllCorpClick
        {
            get { return _AllCorpClick ?? (_AllCorpClick = new RelayCommand((object e) => AllCorp())); }

        }
        private RelayCommand _CloseWindowsOnEscape;
        public RelayCommand CloseWindowsOnEscape
        {
            get
            {
                return _CloseWindowsOnEscape ?? (_CloseWindowsOnEscape = new RelayCommand(
                   (object e) => { sWindow?.Close(); }
                       ));
            }
        }

        private RelayCommand _Info;
        public RelayCommand Info
        {
            get { return _Info ?? (_Info = new RelayCommand((object e) => InfoWindow())); }

        }

        private RelayCommand _CloseOnEscape;

        public RelayCommand CloseOnEscape
        {
            get
            {
                return _CloseOnEscape ?? (_CloseOnEscape = new RelayCommand(
                    (object e) => CloseOnEscape_Click()
                        ));
            }
        }






        #endregion
        private void CloseOnEscape_Click()
        {
            sWindow?.Close();
        }

        private static ScripInfoVM _getInstance;

        public static ScripInfoVM GetInstance
        {
            get
            {
                if (_getInstance == null)
                {
                    _getInstance = new ScripInfoVM();
                }
                return _getInstance;
            }

        }
        private void AllCorp()
        {
            View.CorporateAction.CorporateAction obj = new View.CorporateAction.CorporateAction();
            if (obj != null)
            {
                obj.Show();
                obj.Focus();
            }
            else
            {

                obj = new View.CorporateAction.CorporateAction();
                obj.Activate();
                obj.Show();
            }
        }
        public ScripInfoVM()
        {
            IndicesMemory = new ObservableCollection<string>();
            CAInfoGrid = new ObservableCollection<CorporateActionModel>();
            sWindow = System.Windows.Application.Current.Windows.OfType<ScripInfo>().FirstOrDefault();
        }


        
        private void InfoWindow()
        {
            try
            {
                string url = "http://10.1.101.125:3000/twsreports/StockReach.aspx?sc=" + Scode + "&memid=" + UtilityLoginDetails.GETInstance.MemberId + "&trdid=" + UtilityLoginDetails.GETInstance.TraderId;
                ProcessStartInfo sInfo = new ProcessStartInfo(url);
                Process.Start(sInfo);

            }
            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
            }
        }


        private void PopulateIndices(int scripcode)
        {
            DataAccessLayer oDataAccessLayer1 = new DataAccessLayer();
            oDataAccessLayer1.Connect((int)DataAccessLayer.ConnectionDB.Masters);
            try
            {

                oDataAccessLayer1.OpenConnection((int)DataAccessLayer.ConnectionDB.Masters);
                IndicesMemory.Clear();
                string str = "SELECT ExistingShortName from BSE_SYSBASSUB_CFE A, BSE_SNPINDICES_CFE B where A.IndexCode=B.IndexCode and A.ScripCode=" + scripcode;
                SQLiteDataReader oSQLiteDataReader = oDataAccessLayer1.ExecuteDataReader((int)DataAccessLayer.ConnectionDB.Masters, str, System.Data.CommandType.Text, null);
                while (oSQLiteDataReader.Read())
                {
                    if (oSQLiteDataReader["ExistingShortName"] != string.Empty)
                        IndicesMemory.Add(oSQLiteDataReader["ExistingShortName"]?.ToString().Trim());
                }
                
            }
            catch (Exception e) { ExceptionUtility.LogError(e); }
            finally
            {
                
                oDataAccessLayer1.CloseConnection((int)DataAccessLayer.ConnectionDB.Masters);
            }

        }

        internal void SetDataEquity(ScripHelpModel selectedItem)
        {
            try
            {
               
                ScripID = selectedItem.ScripId;
                Scode = Convert.ToString(selectedItem.ScripCode);
                int DecimalPnt = CommonFunctions.GetDecimal((int)selectedItem.ScripCode, "BSE", CommonFunctions.GetSegmentID((long)selectedItem.ScripCode));
                ISIN = Convert.ToString(selectedItem.IsinCode);
                SName = selectedItem.ScripName;
                MktLotEq = Convert.ToString(selectedItem.MarketLot);
                TicketSizeEqt = Convert.ToString(selectedItem.TickSize);
                FaceValue = Convert.ToString(selectedItem.FaceValue);
                BseExc = Convert.ToString(selectedItem.BseExclusive);
                ScrpGrp = selectedItem.GroupName;
                Status = Convert.ToString(selectedItem.Status);
                PartnID = selectedItem.PartitionId;
                ProdID = selectedItem.ProductId;
                GSM = MasterSharedMemory.objMastertxtDictBaseBSE.Where(x => x.Key == selectedItem.ScripCode).Select(x => x.Value.Filler2_GSM).FirstOrDefault().ToString();
                if (VarMemory.SubscribeVarMemoryDict != null)
                {
                    if (VarMemory.SubscribeVarMemoryDict.ContainsKey((int)selectedItem.ScripCode))
                    {
                        VAREM = string.Format("{0:0.00}", Convert.ToDouble(VarMemory.SubscribeVarMemoryDict[(int)selectedItem.ScripCode].ELMPercentage) / 100);

                        VARIM = string.Format("{0:0.00}", Convert.ToDouble(VarMemory.SubscribeVarMemoryDict[(int)selectedItem.ScripCode].IMPercentage) / 100);

                    }
                    else
                    {
                        VAREM = string.Empty;
                        VARIM = string.Empty;
                    }

                }
                if (MasterSharedMemory.objDicDP.ContainsKey((int)selectedItem.ScripCode))
                {
                    FWeekHP = CommonFunctions.GetValueInDecimal(MasterSharedMemory.objDicDP[(int)selectedItem.ScripCode].WeeksHighprice, DecimalPnt);
                    FWeekLP = CommonFunctions.GetValueInDecimal(MasterSharedMemory.objDicDP[(int)selectedItem.ScripCode].WeeksLowprice, DecimalPnt);
                }

                PopulateIndices((int)selectedItem.ScripCode);

                CAInfoGrid = new ObservableCollection<CorporateActionModel>();
                foreach (var item in MasterSharedMemory.objCorpActBSE.Where(x => x.Value.scripCode == selectedItem.ScripCode))
                {
                    CorporateActionModel objCorpAct = new CorporateActionModel();
                    if (item.Value.bcOrRdFlag == "BC" && item.Value.bcrduniqueness==1)
                    {

                        objCorpAct.purposeOrEvent = item.Value.purposeOrEvent;
                        objCorpAct.exDate = item.Value.exDate;
                        objCorpAct.bookClosureFrom = item.Value.bookClosureFrom;
                        objCorpAct.bookClosureTo = item.Value.bookClosureTo;
                        objCorpAct.NdStartDate = item.Value.NdStartDate;
                        objCorpAct.ndEndDate = item.Value.ndEndDate;
                        objCorpAct.bcrduniqueness= item.Value.bcrduniqueness;
                    }
                    else if (item.Value.bcOrRdFlag == "RD" && item.Value.bcrduniqueness == 2)
                    {
                        objCorpAct.purposeOrEvent = item.Value.purposeOrEvent;
                        objCorpAct.exDate = item.Value.exDate;
                        objCorpAct.recordDate = item.Value.recordDate;
                        objCorpAct.NdStartDate = item.Value.NdStartDate;
                        objCorpAct.ndEndDate = item.Value.ndEndDate;
                        objCorpAct.bcrduniqueness = item.Value.bcrduniqueness;
                    }
                    CAInfoGrid.Add(objCorpAct);
                }
                
            }
            catch (Exception e)
            { ExceptionUtility.LogError(e); }
            }
          
    

        internal void SetDebtData(ScripHelpModel selectedItemDebt)
        {
            try
            {
              
                ScripID = selectedItemDebt.ScripId;
                Scode = Convert.ToString(selectedItemDebt.ScripCode);
                int DecimalPnt = CommonFunctions.GetDecimal((int)selectedItemDebt.ScripCode, "BSE", CommonFunctions.GetSegmentID((long)selectedItemDebt.ScripCode));
                ISIN = Convert.ToString(selectedItemDebt.IsinCode);
                SName = selectedItemDebt.ScripName;
                MktLotEq = Convert.ToString(selectedItemDebt.MarketLot);
                TicketSizeEqt = Convert.ToString(selectedItemDebt.TickSize);
                FaceValue = Convert.ToString(selectedItemDebt.FaceValue);
                BseExc = Convert.ToString(selectedItemDebt.BseExclusive);
                ScrpGrp = selectedItemDebt.GroupName;
                Status = Convert.ToString(selectedItemDebt.Status);
                PartnID = selectedItemDebt.PartitionId;
                ProdID = selectedItemDebt.ProductId;
                GSM = MasterSharedMemory.objMastertxtDictBaseBSE.Where(x => x.Key == selectedItemDebt.ScripCode).Select(x => x.Value.Filler2_GSM).FirstOrDefault().ToString();
                if (VarMemory.SubscribeVarMemoryDict != null)
                {
                    if (VarMemory.SubscribeVarMemoryDict.ContainsKey((int)selectedItemDebt.ScripCode))
                    {
                        VAREM = string.Format("{0:0.00}", Convert.ToDouble(VarMemory.SubscribeVarMemoryDict[(int)selectedItemDebt.ScripCode].ELMPercentage) / 100);

                        VARIM = string.Format("{0:0.00}", Convert.ToDouble(VarMemory.SubscribeVarMemoryDict[(int)selectedItemDebt.ScripCode].IMPercentage) / 100);

                    }
                    else
                    {
                        VAREM = string.Empty;
                        VARIM = string.Empty;
                    }

                }
                if (MasterSharedMemory.objDicDP.ContainsKey((int)selectedItemDebt.ScripCode))
                {
                    FWeekHP = CommonFunctions.GetValueInDecimal(MasterSharedMemory.objDicDP[(int)selectedItemDebt.ScripCode].WeeksHighprice, DecimalPnt);
                    FWeekLP = CommonFunctions.GetValueInDecimal(MasterSharedMemory.objDicDP[(int)selectedItemDebt.ScripCode].WeeksLowprice, DecimalPnt);
                }

                PopulateIndices((int)selectedItemDebt.ScripCode);

                CAInfoGrid = new ObservableCollection<CorporateActionModel>();
                foreach (var item in MasterSharedMemory.objCorpActBSE.Where(x => x.Value.scripCode == selectedItemDebt.ScripCode))
                {
                    CorporateActionModel objCorpAct = new CorporateActionModel();
                    if (item.Value.bcOrRdFlag == "BC" && item.Value.bcrduniqueness == 1)
                    {

                        objCorpAct.purposeOrEvent = item.Value.purposeOrEvent;
                        objCorpAct.exDate = item.Value.exDate;
                        objCorpAct.bookClosureFrom = item.Value.bookClosureFrom;
                        objCorpAct.bookClosureTo = item.Value.bookClosureTo;
                        objCorpAct.NdStartDate = item.Value.NdStartDate;
                        objCorpAct.ndEndDate = item.Value.ndEndDate;
                        objCorpAct.bcrduniqueness = item.Value.bcrduniqueness;
                    }
                    else if (item.Value.bcOrRdFlag == "RD" && item.Value.bcrduniqueness == 2)
                    {
                        objCorpAct.purposeOrEvent = item.Value.purposeOrEvent;
                        objCorpAct.exDate = item.Value.exDate;
                        objCorpAct.recordDate = item.Value.recordDate;
                        objCorpAct.NdStartDate = item.Value.NdStartDate;
                        objCorpAct.ndEndDate = item.Value.ndEndDate;
                        objCorpAct.bcrduniqueness = item.Value.bcrduniqueness;
                    }
                    CAInfoGrid.Add(objCorpAct);
                }
               
            }

            catch (Exception e)
            { ExceptionUtility.LogError(e); }
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
