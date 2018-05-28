using CommonFrontEnd.Common;
using CommonFrontEnd.Model;
using CommonFrontEnd.SharedMemories;
using CommonFrontEnd.View;
using CommonFrontEnd.ViewModel.Touchline;
using SubscribeList;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CommonFrontEnd.ViewModel
{
    public class OpenInterestVM : INotifyPropertyChanged
    {
        #region memory
        private List<string> _listExchange;

        public List<string> listExchange
        {
            get { return _listExchange; }
            set
            {
                _listExchange = value;
                NotifyPropertyChanged(nameof(listExchange));
            }
        }
        
            private List<string> _listFO;

        public List<string> listFO
        {
            get { return _listFO; }
            set
            {
                _listFO = value;
                NotifyPropertyChanged(nameof(listFO));
            }
        }
        
              private static BindingList<string> _listCallPut;

        public static BindingList<string> listCallPut
        {
            get { return _listCallPut; }
            set
            {
                _listCallPut = value;
                NotifyStaticPropertyChanged(nameof(listCallPut));
            }
        }

        private static ObservableCollection<string> _listSegment;

        public static ObservableCollection<string> listSegment
        {
            get { return _listSegment; }
            set { _listSegment = value; NotifyStaticPropertyChanged(nameof(listSegment)); }
        }

        private static ObservableCollection<string> _listCurrIdxSdk;

        public static ObservableCollection<string> listCurrIdxSdk
        {
            get { return _listCurrIdxSdk; }
            set { _listCurrIdxSdk = value; NotifyStaticPropertyChanged(nameof(listCurrIdxSdk)); }
        }
        
             private static ObservableCollection<string> _listAsset;

        public static ObservableCollection<string> listAsset
        {
            get { return _listAsset; }
            set { _listAsset = value; NotifyStaticPropertyChanged(nameof(listAsset)); }
        }
        private static ObservableCollection<OpenInterestModel> _ObjOpenInterstCollection;

        public static ObservableCollection<OpenInterestModel> ObjOpenInterstCollection
        {
            get { return _ObjOpenInterstCollection; }
            set { _ObjOpenInterstCollection = value; NotifyStaticPropertyChanged(nameof(ObjOpenInterstCollection)); }
        }
        
            private static ObservableCollection<OpenInterestModel> _OIDataCollection= new ObservableCollection<OpenInterestModel>();

        public static ObservableCollection<OpenInterestModel> OIDataCollection
        {
            get { return _OIDataCollection; }
            set { _OIDataCollection = value; NotifyStaticPropertyChanged(nameof(OIDataCollection)); }
        }

        #endregion

        #region properties


        public string OIVisibility
        {
            get
            {
                return _OIVisibility;
            }

            set
            {
                _OIVisibility = value;
                NotifyPropertyChanged(nameof(OIVisibility));
            }
        }

        public string ChangeOIQTYVisibility
        {
            get
            {
                return _ChangeOIQTYVisibility;
            }

            set
            {
                _ChangeOIQTYVisibility = value;
                NotifyPropertyChanged(nameof(ChangeOIQTYVisibility));
            }
        }
        public bool OIQTYVisibility
        {
            get
            {
                return _OIQTYVisibility;
            }

            set
            {
                _OIQTYVisibility = value;
                NotifyPropertyChanged(nameof(OIQTYVisibility));
            }
        }

        public string ChangeOIVisibility
        {
            get
            {
                return _ChangeOIVisibility;
            }

            set
            {
                _ChangeOIVisibility = value;
                NotifyPropertyChanged(nameof(ChangeOIVisibility));
            }
        }

        private bool _OIQTYVisibility = true;

        private string _ChangeOIQTYVisibility = "Hidden";
        private string _ChangeOIVisibility = "Visible";
        private string _OIVisibility = "Visible";
        private static string _cmbSelectedExchange;


        public static string cmbSelectedExchange
        {
            get { return _cmbSelectedExchange; }
            set
            {
                _cmbSelectedExchange = value;
                NotifyStaticPropertyChanged(nameof(cmbSelectedExchange));
                populateSegment();
                
                //PopulateCurrIdxData();
            }
        }

        private static string _cmbSelectedSegment;

        public static string cmbSelectedSegment
        {
            get { return _cmbSelectedSegment; }
            set
            {
                _cmbSelectedSegment = value;
                NotifyStaticPropertyChanged(nameof(cmbSelectedSegment));
                //populateSegment();
                PopulateCurrIdxData();
                
            }
        }


        private static string _cmbSelectedlistCurrIdxSdk;

        public static string cmbSelectedlistCurrIdxSdk
        {
            get { return _cmbSelectedlistCurrIdxSdk; }
            set
            { _cmbSelectedlistCurrIdxSdk = value;
                NotifyStaticPropertyChanged(nameof(cmbSelectedlistCurrIdxSdk));
              PopulateAsset();
            }
            
        }
        
            private static string _cmbSelectedlistAsset;

        public static string cmbSelectedlistAsset
        {
            get { return _cmbSelectedlistAsset; }
            set
            {
                _cmbSelectedlistAsset = value;
                NotifyStaticPropertyChanged(nameof(cmbSelectedlistAsset));
                
                if(value != null)
                    DataGridPopulation();
            }

        }
        
              private static string _cmbSelectedlistFO;

        public static string cmbSelectedlistFO
        {
            get { return _cmbSelectedlistFO; }
            set
            {
                _cmbSelectedlistFO = value;
                NotifyStaticPropertyChanged(nameof(cmbSelectedlistFO));
                PopulateCallPut();
                
            }

        }
        
             private static string _cmbSelectedCallPut;

        public static string cmbSelectedCallPut
        {
            get { return _cmbSelectedCallPut; }
            set
            {
                _cmbSelectedCallPut = value;
                NotifyStaticPropertyChanged(nameof(cmbSelectedCallPut));

                if (value != null)
                    DataGridPopulation();
            }

        }
        

             private static bool _isEnableCP;

        public static bool isEnableCP
        {
            get { return _isEnableCP; }
            set
            {
                _isEnableCP = value;
                NotifyStaticPropertyChanged(nameof(isEnableCP));

            }

        }
        #endregion

  
        #region relayCommand
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

        private RelayCommand _OIUpdateCommand;
        public RelayCommand OIUpdateCommand
        {
            get
            {
                return _OIUpdateCommand ?? (_OIUpdateCommand = new RelayCommand(
                    (object e1) => OnChangetoOIButton()));
            }
        }
        private RelayCommand _QtyUpdateCommand;
        public RelayCommand QtyUpdateCommand
        {
            get
            {
                return _QtyUpdateCommand ?? (_QtyUpdateCommand = new RelayCommand(
                    (object e1) => OnChangetoQTYButton()));
            }
        }


        private RelayCommand _FetchFreshOI;
        public RelayCommand FetchFreshOI
        {
            get
            {
                return _FetchFreshOI ?? (_FetchFreshOI = new RelayCommand(
                    (object e) => Listener_OITick()
                        ));
            }
        }



        #endregion

        #region constructor
        public OpenInterestVM()
        {

            //   ObjOpenInterstCollection = new ObservableCollection<OpenInterestModel>();
       //   OIDataCollection = new ObservableCollection<OpenInterestModel>();
            PopulateData();
            PopulateFO();
            if (ObjOpenInterstCollection != null)
            {
                for (int i = 0; i < ObjOpenInterstCollection.Count; i++)
                {
                    Model.ScripDetails objScripDetails = new Model.ScripDetails();
                    if (BroadcastMasterMemory.objScripDetailsConDict != null && BroadcastMasterMemory.objScripDetailsConDict.Count > 0)
                    {
                        objScripDetails = BroadcastMasterMemory.objScripDetailsConDict.Values.Where(x => x.ScriptCode_BseToken_NseToken == ObjOpenInterstCollection[i].SeriesCode).FirstOrDefault();
                        if (objScripDetails != null)
                        {
                            ObjOpenInterstCollection[i].DayTTV = objScripDetails.TrdValue;
                        }
                    }
                }
            }

            
        }

        #endregion

        #region method

        public static void Listener_OITick()
        {
            try
            {

                if (OIMemory.SubscribeOIMemoryDict.Count > 0 && Globals.IsOiWindowOPen == true)
                {
                    objOIBroadcastProcessor_OnBroadCastRecievedNew();
                    //HandlingInCaseOfCurrency();
                    MarketWatchVM.UPdateOI(OpenInterestVM.ObjOpenInterstCollection);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally { }
        }

        public static void objOIBroadcastProcessor_OnBroadCastRecievedNew()
        {
            try
            {
                DataGridPopulation();
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
            }
        }

        private void EscapeUsingUserControl(object e)
        {
            Globals.IsOiWindowOPen = false;
           OpenInterest oOpenInterest = e as OpenInterest;
            oOpenInterest.Hide();
        }
        /// <summary>
        /// Populate Exchange Data
        /// </summary>
        private void PopulateData()
        {
            listExchange = new List<string>();
            cmbSelectedExchange = Common.Enumerations.Exchange.BSE.ToString();
            listExchange.Add(Common.Enumerations.Exchange.BSE.ToString());

            //TODO Prafulla 21Nov2017 openInterest
#if BOW
            listExchange.Add(Common.Enumerations.Exchange.BSEINX.ToString());
            listExchange.Add(Common.Enumerations.Exchange.NSE.ToString());
            listExchange.Add(Common.Enumerations.Exchange.USE.ToString());

#endif
        }
        /// <summary>
        /// Populate Segment Data
        /// </summary>
        private static void populateSegment()
        {
            try
            {
                if (cmbSelectedExchange == Common.Enumerations.Exchange.BSE.ToString())
                {
                    listSegment = new ObservableCollection<string>();
                    listSegment.Add(Common.Enumerations.Segment.Derivative.ToString());
                    listSegment.Add(Common.Enumerations.Segment.Currency.ToString());
                    cmbSelectedSegment = Common.Enumerations.Segment.Derivative.ToString();
                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
            }

        }

        /// <summary>
        /// Populate Curr / Index/ Stock enum
        /// </summary>
        private static void PopulateCurrIdxData()
        {
            try
            {

                if (cmbSelectedExchange == Common.Enumerations.Exchange.BSE.ToString() && cmbSelectedSegment == Common.Enumerations.Segment.Derivative.ToString())
                {
                    listCurrIdxSdk = new ObservableCollection<string>();
                    listCurrIdxSdk.Add(Common.Enumerations.CurIdxStk.ALL.ToString());
                    //listCurrIdxSdk.Add(Common.Enumerations.CurIdxStk.CURR.ToString());
                    listCurrIdxSdk.Add(Common.Enumerations.CurIdxStk.INDEX.ToString());
                    listCurrIdxSdk.Add(Common.Enumerations.CurIdxStk.STOCK.ToString());
                    cmbSelectedlistCurrIdxSdk = Common.Enumerations.CurIdxStk.ALL.ToString();
                }
                else if (cmbSelectedExchange == Common.Enumerations.Exchange.BSE.ToString() && cmbSelectedSegment == Common.Enumerations.Segment.Currency.ToString())
                {
                    listCurrIdxSdk = new ObservableCollection<string>();
                    listCurrIdxSdk.Add(Common.Enumerations.CurIdxStk.ALL.ToString());
                    listCurrIdxSdk.Add(Common.Enumerations.CurIdxStk.CURR.ToString());

                    cmbSelectedlistCurrIdxSdk = Common.Enumerations.CurIdxStk.ALL.ToString();

                }
#if BOW
                else if (cmbSelectedExchange == Common.Enumerations.Exchange.BSEINX.ToString())
                {
                    listCurrIdxSdk = new ObservableCollection<string>();
                    cmbSelectedlistCurrIdxSdk = Common.Enumerations.CurIdxStk.STOCK.ToString();
                    listCurrIdxSdk.Add(Common.Enumerations.CurIdxStk.STOCK.ToString());
                    listCurrIdxSdk.Add(Common.Enumerations.CurIdxStk.CURR.ToString());
                    listCurrIdxSdk.Add(Common.Enumerations.CurIdxStk.INDEX.ToString());
                }
#endif

            }
            catch (Exception e)
            {
                ExceptionUtility.LogError(e);
            }


        }
        /// <summary>
        /// Populate Asset List
        /// </summary>
        private static void PopulateAsset()
        {
            try
            {
                #region BSE
                //for BSE Exchange
                if (cmbSelectedExchange == Enumerations.Order.Exchanges.BSE.ToString())
                {
                    //for derivative market DerivativeMasterBase> 
                    if (cmbSelectedSegment == Enumerations.Order.ScripSegment.Derivative.ToString())
                    {
                        if (cmbSelectedlistCurrIdxSdk == Common.Enumerations.CurIdxStk.ALL.ToString())
                        {

                            listAsset = new ObservableCollection<string>();
                            listAsset.Add("ALL");
                            foreach (var item in MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Cast<DerivativeMasterBase>().Select(x => x.UnderlyingAsset).Distinct())
                            {
                                listAsset.Add(item);
                            }
                            if (listAsset.Count > 0 && listAsset != null)
                            {
                                cmbSelectedlistAsset = listAsset[0];


                            }

                        }
                        if (cmbSelectedlistCurrIdxSdk == Common.Enumerations.CurIdxStk.INDEX.ToString())
                        {

                            listAsset = new ObservableCollection<string>();
                            listAsset.Add("ALL");
                            foreach (var item in MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Cast<DerivativeMasterBase>().Where(x => x.InstrumentType == "IO" || x.InstrumentType == "IF").Select(x => x.UnderlyingAsset).Distinct())
                            {
                                listAsset.Add(item);
                            }
                            if (listAsset.Count > 0 && listAsset != null)
                            {
                                cmbSelectedlistAsset = listAsset[0];
                            }
                        }
                        if (cmbSelectedlistCurrIdxSdk == Common.Enumerations.CurIdxStk.STOCK.ToString())
                        {

                            listAsset = new ObservableCollection<string>();
                            listAsset.Add("ALL");
                            foreach (var item in MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Cast<DerivativeMasterBase>().Where(x => x.InstrumentType == "SO" || x.InstrumentType == "SF").Select(x => x.UnderlyingAsset).Distinct())
                            {
                                listAsset.Add(item);
                            }
                            if (listAsset.Count > 0 && listAsset != null)
                            {
                                cmbSelectedlistAsset = listAsset[0];
                            }
                        }
                        //foreach (var item in MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Cast<DerivativeMasterBase>().Where(x => x.InstrumentType == "IF"&& x.UnderlyingAsset==cmbSelectedlistAsset).Select(x => x.scri).Distinct())
                        //{
                        //    cmbSelectedlistAsset.Add(item);
                        //}
            
                    }
                  //  BSE CURRENCY objMasterCurrencyDictBaseBSE CurrencyMasterBase

                        if (cmbSelectedSegment == Enumerations.Order.ScripSegment.Currency.ToString())
                    {
                        if (cmbSelectedlistCurrIdxSdk == Common.Enumerations.CurIdxStk.ALL.ToString())
                        {

                            listAsset = new ObservableCollection<string>();
                            listAsset.Add("ALL");
                            foreach (var item in MasterSharedMemory.objMasterCurrencyDictBaseBSE.Values.Cast<CurrencyMasterBase>().Select(x => x.UnderlyingAsset).Distinct())
                            {
                                listAsset.Add(item);
                            }
                            if (listAsset.Count > 0 && listAsset != null)
                            {
                                cmbSelectedlistAsset = listAsset[0];


                            }

                        }
                        if (cmbSelectedlistCurrIdxSdk == Common.Enumerations.CurIdxStk.CURR.ToString())
                        {
                            listAsset = new ObservableCollection<string>();
                            listAsset.Add("ALL");

                            foreach (var item in MasterSharedMemory.objMasterCurrencyDictBaseBSE.Values.Cast<CurrencyMasterBase>().Where(x => x.InstrumentType == "FUTCUR" || x.InstrumentType == "FUTIRT" || x.InstrumentType == "OPTCUR" || x.InstrumentType == "FUTIRD").Select(x => x.UnderlyingAsset).Distinct())
                            {
                                listAsset.Add(item);
                            }
                           
                            if (listAsset.Count > 0 && listAsset != null)
                            {
                                cmbSelectedlistAsset = listAsset[0];
                            }
                        }
                     
                    }
             
                    #endregion
                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
                throw;
            }
        }
        /// <summary>
        /// Populate Future/Option list
        /// </summary>
        private void PopulateFO()
        {
            try
            {
                #region BSE
                //for BSE Exchange
                if (cmbSelectedExchange == Enumerations.Order.Exchanges.BSE.ToString())
                {
                    listFO = new List<string>();
                    listFO.Add(Common.Enumerations.FutureOption.All.ToString());
                    listFO.Add(Common.Enumerations.FutureOption. Future.ToString());
                    listFO.Add(Common.Enumerations.FutureOption.Option.ToString());
                     cmbSelectedlistFO = Common.Enumerations.FutureOption.All.ToString();


                }

                #endregion
              

            }
            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
              
            }
        }
        /// <summary>
        /// Populate Call Put list
        /// </summary>
        private static void PopulateCallPut()
        {
            if (cmbSelectedExchange == Enumerations.Order.Exchanges.BSE.ToString() && cmbSelectedlistFO == Common.Enumerations.FutureOption.Option.ToString())
            {
                isEnableCP = true;
                listCallPut = new BindingList<string>();
                listCallPut.Add(Common.Enumerations.CallPut.All.ToString());
                listCallPut.Add(Common.Enumerations.CallPut.Call.ToString());
                listCallPut.Add(Common.Enumerations.CallPut.Put.ToString());
                cmbSelectedCallPut = Common.Enumerations.CallPut.All.ToString();
               
            }
          if(cmbSelectedlistFO == Common.Enumerations.FutureOption.All.ToString()|| cmbSelectedlistFO == Common.Enumerations.FutureOption.Future.ToString())
            {
                isEnableCP = false;
                //listCallPut = new List<string>();
                //listFO.Add(Common.Enumerations.CallPut.All.ToString());
                cmbSelectedCallPut = Common.Enumerations.CallPut.All.ToString();
               
            }
        }
        /// <summary>
        /// Data Filtration on basis of filters
        /// </summary>
        internal static void DataGridPopulation()
        {
            BroadcastReceiver.OIMainDetails Br;

            ObjOpenInterstCollection.Clear();
            foreach (var item in OIMemory.SubscribeOIMemoryDict.Values.ToList())
            {
                Br = item;

                if (Br != null)
                {
                    int MarketLot = 1;
                    long Multiplier = 1;

                    if (Br.Segment == (int)Enumerations.Segment.Currency && MasterSharedMemory.objMasterCurrencyDictBaseBSE != null &&
                        MasterSharedMemory.objMasterCurrencyDictBaseBSE.Count != 0 &&
                        MasterSharedMemory.objMasterCurrencyDictBaseBSE.ContainsKey(Br.InstrumentID))
                    {
                        MarketLot = MasterSharedMemory.objMasterCurrencyDictBaseBSE[Br.InstrumentID].MinimumLotSize;
                        Multiplier = MasterSharedMemory.objMasterCurrencyDictBaseBSE[Br.InstrumentID].QuantityMultiplier;
                    }
                    else if (Br.Segment == (int)Enumerations.Segment.Derivative && MasterSharedMemory.objMasterDerivativeDictBaseBSE != null &&
                        MasterSharedMemory.objMasterDerivativeDictBaseBSE.Count != 0 &&
                        MasterSharedMemory.objMasterDerivativeDictBaseBSE.ContainsKey(Br.InstrumentID))
                    {
                        MarketLot = MasterSharedMemory.objMasterDerivativeDictBaseBSE[Br.InstrumentID].MinimumLotSize;
                        Multiplier = MasterSharedMemory.objMasterDerivativeDictBaseBSE[Br.InstrumentID].QuantityMultiplier;
                    }
                    else
                        continue;

                    OpenInterestModel objOpenInterestModel = new OpenInterestModel();
                    objOpenInterestModel.SeriesCode = Br.InstrumentID;
                    objOpenInterestModel.Segment = Br.Segment;

                    if (Br.Segment == (int)Enumerations.Segment.Currency)
                    {
                        objOpenInterestModel.SeriesName = MasterSharedMemory.objMasterCurrencyDictBaseBSE[Br.InstrumentID].InstrumentName;
                        objOpenInterestModel.SeriesID = MasterSharedMemory.objMasterCurrencyDictBaseBSE[Br.InstrumentID].ScripId;
                        objOpenInterestModel.Asset = MasterSharedMemory.objMasterCurrencyDictBaseBSE[Br.InstrumentID].UnderlyingAsset;
                        objOpenInterestModel.InstrumentType = MasterSharedMemory.objMasterCurrencyDictBaseBSE[Br.InstrumentID].InstrumentType;
                        objOpenInterestModel.CallPutFlag = MasterSharedMemory.objMasterCurrencyDictBaseBSE[Br.InstrumentID].OptionType;
                    }
                    else
                    {
                        objOpenInterestModel.SeriesName = MasterSharedMemory.objMasterDerivativeDictBaseBSE[Br.InstrumentID].InstrumentName;
                        objOpenInterestModel.SeriesID = MasterSharedMemory.objMasterDerivativeDictBaseBSE[Br.InstrumentID].ScripId;
                        objOpenInterestModel.Asset = MasterSharedMemory.objMasterDerivativeDictBaseBSE[Br.InstrumentID].UnderlyingAsset;
                        objOpenInterestModel.InstrumentType = MasterSharedMemory.objMasterDerivativeDictBaseBSE[Br.InstrumentID].InstrumentType;
                        objOpenInterestModel.CallPutFlag = MasterSharedMemory.objMasterDerivativeDictBaseBSE[Br.InstrumentID].OptionType;
                    }

                    objOpenInterestModel.OI = Br.OpenInterestQuantity;
                    objOpenInterestModel.ChangeInOI = Br.OpenInterestChange;
                    objOpenInterestModel.OIValue = string.Format("{0:0.00}", Convert.ToDouble(Br.OpenInterestValue) / 10000000);
                    objOpenInterestModel.DayTTV = Br.TotalTradedValue;


                    if (cmbSelectedExchange != Enumerations.Order.Exchanges.BSE.ToString())
                        continue;

                    if (cmbSelectedSegment == Enumerations.Order.ScripSegment.Derivative.ToString())
                    {
                        if (objOpenInterestModel.Segment != (int)Enumerations.Segment.Derivative)
                            continue;

                        if (cmbSelectedlistCurrIdxSdk != "ALL")
                        {
                            if (cmbSelectedlistCurrIdxSdk == "INDEX")
                            {
                                if (objOpenInterestModel.InstrumentType != "IO" && objOpenInterestModel.InstrumentType != "IF")
                                    continue;
                            }
                            else if (cmbSelectedlistCurrIdxSdk == "STOCK")
                            {
                                if (objOpenInterestModel.InstrumentType != "SO" && objOpenInterestModel.InstrumentType != "SF")
                                    continue;
                            }
                        }
                    }
                    else if (cmbSelectedSegment == Enumerations.Order.ScripSegment.Currency.ToString())
                    {
                        if (objOpenInterestModel.Segment != (int)Enumerations.Segment.Currency)
                            continue;

                        if (cmbSelectedlistCurrIdxSdk != "ALL")
                        {
                            if (cmbSelectedlistCurrIdxSdk == "CURR")
                            {
                                if (objOpenInterestModel.InstrumentType != "FUTCUR" && objOpenInterestModel.InstrumentType != "FUTIRD" &&
                                    objOpenInterestModel.InstrumentType != "FUTIRT" && objOpenInterestModel.InstrumentType != "OPTCUR")
                                    continue;
                            }
                        }
                    }
                    else
                        continue;

                    if (cmbSelectedlistAsset != "ALL")
                    {
                        if (cmbSelectedlistAsset != objOpenInterestModel.Asset)
                            continue;
                    }

                    if (cmbSelectedlistFO != "All")
                    {
                        if (cmbSelectedlistFO == "Future")
                        {
                            if (objOpenInterestModel.CallPutFlag != null)
                                continue;
                        }
                        else if (cmbSelectedlistFO == "Option")
                        {
                            if (cmbSelectedCallPut != "All")
                            {
                                if (cmbSelectedCallPut == "Call" && objOpenInterestModel.CallPutFlag != "CE")
                                    continue;
                                else if (cmbSelectedCallPut == "Put" && objOpenInterestModel.CallPutFlag != "PE")
                                    continue;
                            }
                            else
                            {
                                if (objOpenInterestModel.CallPutFlag == null)
                                    continue;
                            }
                        }
                    }

                    lock ((ObjOpenInterstCollection as ICollection).SyncRoot)
                        ObjOpenInterstCollection.Add(objOpenInterestModel);
                }
            }
        }



        public void OnChangetoQTYButton()
        {


            int MarketLot = 1;
            long Multiplier = 1;
            if (ObjOpenInterstCollection != null && ObjOpenInterstCollection.Count > 0)
            {
                for (int i = 0; i < ObjOpenInterstCollection.Count; i++)
                {
                    //if (ObjOpenInterstCollection[i].Segment == 1)
                    //{
                    //    MarketLot = MasterSharedMemory.objMastertxtDictBaseBSE[ObjOpenInterstCollection[i].SeriesCode].MarketLot;
                    //}
                    if (ObjOpenInterstCollection[i].Segment == 2 && MasterSharedMemory.objMasterDerivativeDictBaseBSE != null &&
                        MasterSharedMemory.objMasterDerivativeDictBaseBSE.ContainsKey(ObjOpenInterstCollection[i].SeriesCode)
                        && MasterSharedMemory.objMasterDerivativeDictBaseBSE.Count != 0)
                    {

                        MarketLot = MasterSharedMemory.objMasterDerivativeDictBaseBSE[ObjOpenInterstCollection[i].SeriesCode].MinimumLotSize;
                        Multiplier = MasterSharedMemory.objMasterDerivativeDictBaseBSE[ObjOpenInterstCollection[i].SeriesCode].QuantityMultiplier;
                    }
                    if (ObjOpenInterstCollection[i].Segment == 3 &&
                         MasterSharedMemory.objMasterCurrencyDictBaseBSE != null &&
                        MasterSharedMemory.objMasterCurrencyDictBaseBSE.ContainsKey(ObjOpenInterstCollection[i].SeriesCode) &&
                        MasterSharedMemory.objMasterCurrencyDictBaseBSE.Count != 0)
                    {
                        MarketLot = MasterSharedMemory.objMasterCurrencyDictBaseBSE[ObjOpenInterstCollection[i].SeriesCode].MinimumLotSize;
                        Multiplier = MasterSharedMemory.objMasterCurrencyDictBaseBSE[ObjOpenInterstCollection[i].SeriesCode].QuantityMultiplier;
                    }
                    ObjOpenInterstCollection[i].OIContract = ObjOpenInterstCollection[i].OI;
                    ObjOpenInterstCollection[i].OI = ObjOpenInterstCollection[i].OI * MarketLot * Multiplier;
                    ObjOpenInterstCollection[i].ChangeInOIContract = ObjOpenInterstCollection[i].ChangeInOI;
                    ObjOpenInterstCollection[i].ChangeInOI = ObjOpenInterstCollection[i].ChangeInOI * MarketLot * Multiplier;

                }

            }
        }

        public void OnChangetoOIButton()
        {
            if (ObjOpenInterstCollection != null && ObjOpenInterstCollection.Count > 0)
            {
                for (int i = 0; i < ObjOpenInterstCollection.Count; i++)
                {

                    ObjOpenInterstCollection[i].OI = ObjOpenInterstCollection[i].OIContract;

                    ObjOpenInterstCollection[i].ChangeInOI = ObjOpenInterstCollection[i].ChangeInOIContract;

                }
            }
        }

        #endregion




        #region NotifyPropertyChangedEvent
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

        public static event EventHandler<PropertyChangedEventArgs> StaticPropertyChanged
                = delegate { };
        public static void NotifyStaticPropertyChanged(string propertyName)
        {
            StaticPropertyChanged(null, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

    }
}
