using CommonFrontEnd.Common;
using CommonFrontEnd.Global;
using CommonFrontEnd.Model.Admin;
using CommonFrontEnd.Model.CorporateAction;
using CommonFrontEnd.Model.Trade;
using CommonFrontEnd.ViewModel.Trade;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SQLite;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static CommonFrontEnd.SharedMemories.DataAccessLayer;

namespace CommonFrontEnd.SharedMemories
{
    public class NetQtyLimit
    {
        public long BuyLimit;
        public long AvailBuyLimit;
        public long SellLimit;
        public long AvailSellLimit;

        public NetQtyLimit()
        {
            BuyLimit = 0;
            AvailBuyLimit = 0;
            SellLimit = 0;
            AvailSellLimit = 0;
        }
    }

    public class GroupLimit
    {
        public long BuyLimit;
        public double AvailBuyLimit;
        public long SellLimit;
        public double AvailSellLimit;

        public GroupLimit()
        {
            BuyLimit = -1;
            AvailBuyLimit = 0;
            SellLimit = -1;
            AvailSellLimit = 0;
        }
    }

    public class Limits
    {
        public long GrossBuyLimit;
        public double AvailGrossBuyLimit;
        public long GrossSellLimit;
        public double AvailGrossSellLimit;

        public long NetValue;
        public double CurrNetValue;
        public double BuyValue;
        public double SellValue;

        public bool UnrestGrpLimit;
        public ConcurrentDictionary<string, GroupLimit> g_GroupLimit;

        public bool UnrestNetQtyLimit;
        public ConcurrentDictionary<long, NetQtyLimit> g_NetQtyLimit;

        
        public Limits()
        {
            GrossBuyLimit = 0;
            AvailGrossBuyLimit = 0;
            GrossSellLimit = 0;
            AvailGrossSellLimit = 0;

            NetValue = 0;
            CurrNetValue = 0;
            BuyValue = 0;
            SellValue = 0;

            UnrestGrpLimit = false;
            UnrestNetQtyLimit = false;
        }
    }
    
    
    public static class Limit
    {
        public static string[] ExchangeList = new string[]
        {
            "BSE EQUITY", "BSE EQT-DERIVATIVE", "BSE CUR-DERIVATIVE"
        };

        public static string[] GroupList = new string[]
        {
            "A", "B", "T"
        };

        public enum ExchangeNum
        {
            BSE_EQT, BSE_EDRV, BSE_CDRV
            //, BSE_COMM
            //, NSE_EQT
        };

        public static Limits[] g_Limit;
        // public static GroupLimit[] g_GroupLimit;
        // public static Dictionary <long, NetQtyLimit>[] g_NetQtyLimit;

        public static string[] SpreadLegList = new string[]
        {
            "Parent Scrip", "Leg1", "Leg2"
        };

        public enum SpreadLeg
        {
            LEG0, LEG1, LEG2
        };

        public static int ORDDNLD = 0, ORDAUD = 1, TRDDNLD = 3, TRDONLINE = 4, CNVTLIMIT = 5, IOCACTION = 6, REJECTED = 7, LIMITVIOLATE = 8;
    }

    public class SpreadLegInfo
    {
        public long ScripCodeLeg;
        public long PrevPricePaisa;
        public long CurrPricePaisa;
        public string BuySellIndicator;
        public string ScripIdLeg;

        public SpreadLegInfo()
        {
            ScripCodeLeg = 0;
            PrevPricePaisa = 0;
            CurrPricePaisa = 0;
            BuySellIndicator = "";
            ScripIdLeg = "";
        }
    }


    public static class MasterSharedMemory
    {
        public static string g_Segment;
        public static string g_ScripId;
        public static string g_ScripName;
        public static long g_ScripCode;

        public static string g_InstrType;
        public static string g_Asset;
        public static string g_Expiry;
        public static string g_Strike;


        public static string Edate;
        public static string Ddate;
        public static string Cdate;
        static string currentDir = Environment.CurrentDirectory;
        public static Dictionary<long, string> objExclusiveScripDataCollection;
        public static Dictionary<long, ScripMasterBase> objMastertxtDictBaseBSE;//scripmaster base BSE
        public static Dictionary<long, ScripMasterBase> objMastertxtDictBaseNSE;//scripmaster base NSE
        public static Dictionary<long, DerivativeMasterBase> objMasterDerivativeDictBaseBSE;//derivative master base BSE
        public static Dictionary<long, DerivativeMaster> objMasterDerivativeSpreadDictBaseBSE;//derivative master base BSE
        public static Dictionary<long, DerivativeMasterBase> objMasterDerivativeDictBaseNSE;//derivative master base NSE
        public static Dictionary<long, CurrencyMasterBase> objMasterCurrencyDictBaseBSE;//derivative master base BSE
        public static Dictionary<long, CurrencyMaster> objMasterCurrencySpreadDictBaseBSE;//derivative master base BSE
        public static Dictionary<long, CurrencyMasterBase> objMasterCurrencyDictBaseNSE;//derivative master base NSE
        public static Dictionary<long, WholesaleDebtMarket> objMasterWholesaleDebtMarket;// Wholesale Debt Market
        public static Dictionary<long, ScripMasterDebtInfo> objScripMasterDebtInfo;// Wholesale Debt Market
        public static Dictionary<int, SLBMaster> objSLBMasterDict;//SLB Master
        public static Dictionary<int, ITPMaster> objITPMasterDict;//ITP Master
        public static Dictionary<long, MCXMaster> objMCXMasterDict;//MCX Master
        public static Dictionary<long, NCDEXMaster> objNCDEXMasterDict;//NCDEX Master

        public static Dictionary<int, ScripMasterSybas> objMasterDicSyb;//SYSBAS sub
        public static List<ScripMasterSybas> MainListSybas;//SYSBAS main
        public static Dictionary<int, DP> objDicDP;//DP File
        public static Dictionary<long, ScripMasterSpnIndices> objSpnIndicesDic;//only for long name of indices        
        public static List<SetlMas> listSetlMas;//Settlement master
        public static DataAccessLayer oDataAccessLayer;
        public static ConcurrentDictionary<string, ClientMaster> objClientMasterDict;
        public static List<TouchlineWindow> objTouchlineCollection;
        public static ObservableCollection<TouchlineWindow> ExchangeDefaultColumns;
        public static ConcurrentDictionary<string, GroupWiseLimitsModel> GroupWiseLimitDict = null;
        public static Dictionary<string, CPCodeDerivative> objCPCodeDerivativeDict;
        public static Dictionary<string, CPCodeCurrency> objCPCodeCurrencyDict;
        public static Dictionary<string, CorporateActionModel> objCorpActBSE;
       // public static Dictionary<int, TraderEntitlementModel>TraderEntitlementDict; //Admin TraderEntitlement
        public static List<string> lines = null;

        static MasterSharedMemory()
        {
            oDataAccessLayer = new DataAccessLayer();
            oDataAccessLayer.Connect((int)DataAccessLayer.ConnectionDB.Masters);
            //oDataAccessLayer.ConnectTraderEntitlement();
        }


        internal static void ReadDPSetlMaster()
        {
            ScripMasterDebtInfo_read();
            SetlMas_txt();
            DP_file();
        }

        public static void ReadAllMasters()
        {
            MainListSybas = new List<ScripMasterSybas>();
            listSetlMas = new List<SetlMas>();

            //Admin
            //TraderEntitlementDict = new Dictionary<int, TraderEntitlementModel>();
            //ReadTraderEntitlementDB();

            //Securities
            objMastertxtDictBaseBSE = new Dictionary<long, ScripMasterBase>();//BSE
            objMastertxtDictBaseNSE = new Dictionary<long, ScripMasterBase>();//NSE

            //Derivative
            objMasterDerivativeDictBaseBSE = new Dictionary<long, DerivativeMasterBase>();//BSE
            objMasterDerivativeSpreadDictBaseBSE = new Dictionary<long, DerivativeMaster>();//BSE
            objMasterDerivativeDictBaseNSE = new Dictionary<long, DerivativeMasterBase>();//NSE

            //Currency
            objMasterCurrencyDictBaseBSE = new Dictionary<long, CurrencyMasterBase>();//BSE
            objMasterCurrencySpreadDictBaseBSE = new Dictionary<long, CurrencyMaster>();//BSE
            objMasterCurrencyDictBaseNSE = new Dictionary<long, CurrencyMasterBase>();//NSE

            //Corporate Action
            objCorpActBSE = new Dictionary<string, CorporateActionModel>();
            //WholeSaleDebt Market
            objMasterWholesaleDebtMarket = new Dictionary<long, WholesaleDebtMarket>();

            //Scrip Master Debt Info
            objScripMasterDebtInfo = new Dictionary<long, ScripMasterDebtInfo>();

            //SLB Market
            objSLBMasterDict = new Dictionary<int, SLBMaster>();

            //ITP Market
            objITPMasterDict = new Dictionary<int, ITPMaster>();

            //Commodity
            objMCXMasterDict = new Dictionary<long, MCXMaster>();//MCX
            objNCDEXMasterDict = new Dictionary<long, NCDEXMaster>();//NCDEX


            //CPCodeFilesRead
            objCPCodeDerivativeDict = new Dictionary<string, CPCodeDerivative>();//Derivative EQD_PARTICIPANT
            objCPCodeCurrencyDict = new Dictionary<string, CPCodeCurrency>();//Currency BFX_PARTICIPANT
            //Read Securities
            ReadScripMasterBSE();//BSE
            ReadScripMasterNSE();//NSE

            //Read Derivatives
            ReadDerivativeMasterBSE();//BSE
            ReadDerivativeSpreadMasterBSE();
            ReadDerivativeMasterNSE();//NSE

            //Read Currencies
            ReadCurrencyMasterBSE();//BSE
            ReadCurrencyMasterSpreadBSE();//BSE
            ReadCurrencyMasterNSE();//NSE


            // PopulateCAMainMemory();
            //Read Wholesale debt market
            ReadWholeSaleDebtMarket();
            //Read SLB Market
            ReadSLBMaster();
            //Read ITP Market
            ReadITPMaster();

            //Read Commodity
            ReadMCXMaster();//MCX
            ReadNCDEXMaster();//NCDEX
                              //CorpAct_txt();

            //Client Master
         //   Read_CPCodeDerivative();


            //Client Master
            ReadClientMasters_txt();

            //Touchline Db
            ReadExchangeColumnProfile();

            // InsertDefaultProfileInUserTable();

            objMasterDicSyb = new Dictionary<int, ScripMasterSybas>();
            ScripMasterSybas_csv();

            objDicDP = new Dictionary<int, DP>();
            DP_file();

            objSpnIndicesDic = new Dictionary<long, ScripMasterSpnIndices>();
            SpnIndices_mas();

            //Read after login refer ReadDPSetlMaster()
            //SetlMas_txt();
            ReadDerivativeCPCode();
            ReadCurrencyCPCode();
            //GroupWise fetching for limits
            ReadScripGroup();

            ReadCorporateActionBSE();

        }


        private static void ReadCurrencyMasterSpreadBSE()
        {
            try
            {
                oDataAccessLayer.Connect((int)DataAccessLayer.ConnectionDB.Masters);
                oDataAccessLayer.OpenConnection((int)DataAccessLayer.ConnectionDB.Masters);

                string strQuery = @"SELECT ContractTokenNum,ContractTokenNum_Leg1,ContractTokenNum_Leg2,NTAScripCode,StrategyID,NoofLegsinStrategy,
                                    Eligibility,ComplexInstrumentType FROM BSE_CURRENCY_SPD_CFE;";

                SQLiteDataReader oSQLiteDataReader = oDataAccessLayer.ExecuteDataReader((int)ConnectionDB.Masters,strQuery, System.Data.CommandType.Text, null);

                while (oSQLiteDataReader.Read())
                {
                    CurrencyMaster objCurrMaster = new CurrencyMaster();

                    //ContractTokenNum
                    var ContractTokenNum = oSQLiteDataReader["ContractTokenNum"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(ContractTokenNum))
                        objCurrMaster.ContractTokenNum = Convert.ToInt64(ContractTokenNum);

                    //ContractTokenNum_Leg1
                    var ContractTokenNum_Leg1 = oSQLiteDataReader["ContractTokenNum_Leg1"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(ContractTokenNum_Leg1))
                        objCurrMaster.ContractTokenNum_Leg1 = Convert.ToInt32(ContractTokenNum_Leg1);

                    //ContractTokenNum_Leg2
                    var ContractTokenNum_Leg2 = oSQLiteDataReader["ContractTokenNum_Leg2"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(ContractTokenNum_Leg2))
                        objCurrMaster.ContractTokenNum_Leg2 = Convert.ToInt32(ContractTokenNum_Leg2);

                    //NTAScripCode
                    var NTAScripCode = oSQLiteDataReader["NTAScripCode"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(NTAScripCode))
                        objCurrMaster.NTAScripCode = Convert.ToInt64(NTAScripCode);

                    //StrategyID
                    var StrategyID = oSQLiteDataReader["StrategyID"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(StrategyID))
                        objCurrMaster.StrategyID = Convert.ToInt32(StrategyID);

                    //NoofLegsinStrategy
                    var NoofLegsinStrategy = oSQLiteDataReader["NoofLegsinStrategy"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(NoofLegsinStrategy))
                        objCurrMaster.NoofLegsinStrategy = Convert.ToInt32(NoofLegsinStrategy);

                    //Eligibility
                    var Eligibility = oSQLiteDataReader["Eligibility"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(Eligibility))
                        objCurrMaster.Eligibility = Convert.ToChar(Eligibility);

                    //ComplexInstrumentType
                    var ComplexInstrumentType = oSQLiteDataReader["ComplexInstrumentType"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(ComplexInstrumentType))
                        objCurrMaster.ComplexInstrumentType = Convert.ToInt32(ComplexInstrumentType);

                    if (!objMasterCurrencySpreadDictBaseBSE.ContainsKey(objCurrMaster.ContractTokenNum))
                    {
                        objMasterCurrencySpreadDictBaseBSE.Add(objCurrMaster.ContractTokenNum, objCurrMaster);
                    }
                }
                objMasterCurrencySpreadDictBaseBSE = objMasterCurrencySpreadDictBaseBSE.OrderBy(x => x.Value.ContractTokenNum).ToDictionary(x => x.Key, x => x.Value);
            }
            catch (Exception ex)
            {

            }
            finally
            {
                oDataAccessLayer.CloseConnection((int)DataAccessLayer.ConnectionDB.Masters);
                System.Data.SQLite.SQLiteConnection.ClearAllPools();
            }
        }


        private static void ReadCorporateActionBSE()
        {
            DataAccessLayer oDataAccessLayer = new DataAccessLayer();
            oDataAccessLayer.OpenConnection((int)DataAccessLayer.ConnectionDB.Masters);
            try
            {
                string strQuery = @"SELECT * FROM BSE_EQ_CORPACT_CFE order by ExDate ASC;";//where ((ScripCode BETWEEN 500000 AND 600000) OR (ScripCode>=700000))

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
                    {
                        objCorpAct.exDate = Convert.ToDateTime(ExDate).ToString("dd-MM-yyyy");
                        objCorpAct.bcrduniqueness = 1;
                    }
                    #region Change Check
                    //objCorpAct.exDate = Convert.ToDateTime(ExDate).ToString("yyyy-MM-dd");
                    #endregion

                    //objCorpAct.exDate = DateTime.ParseExact(ExDate, @"M/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString("dd-MM-yyyy");
                    if (BcOrRdFlag.Equals("RD") && string.IsNullOrEmpty(ExDate) && !string.IsNullOrEmpty(BookClosureFrom))
                    {
                        #region Change Check
                        //objCorpAct.exDate = Convert.ToDateTime(BookClosureFrom).ToString("yyyy-MM-dd");
                        //objCorpAct.recordDate = Convert.ToDateTime(BookClosureFrom).ToString("yyyy-MM-dd");
                        #endregion
                        objCorpAct.exDate = Convert.ToDateTime(BookClosureFrom).ToString("dd-MM-yyyy");
                        objCorpAct.recordDate = Convert.ToDateTime(BookClosureFrom).ToString("dd-MM-yyyy");
                        objCorpAct.bcrduniqueness = 2;
                        //objCorpAct.exDate = DateTime.ParseExact(BookClosureFrom, @"M/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString("dd-MM-yyyy"); ;
                        //objCorpAct.recordDate = DateTime.ParseExact(BookClosureFrom, @"M/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString("dd-MM-yyyy");
                    }
                    if (BcOrRdFlag.Equals("RD") && !string.IsNullOrEmpty(ExDate))
                    {
                        objCorpAct.recordDate = Convert.ToDateTime(BookClosureFrom).ToString("dd-MM-yyyy");
                        objCorpAct.bcrduniqueness = 2;
                    }
                    #region Change Check
                    //objCorpAct.recordDate = Convert.ToDateTime(BookClosureFrom).ToString("yyyy-MM-dd");
                    #endregion


                    // objCorpAct.recordDate = DateTime.ParseExact(BookClosureFrom, @"M/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString("dd-MM-yyyy");
                    if (BcOrRdFlag.Equals("BC") && string.IsNullOrEmpty(ExDate) && !string.IsNullOrEmpty(BookClosureFrom))
                    {
                        objCorpAct.exDate = Convert.ToDateTime(BookClosureFrom).ToString("dd-MM-yyyy");
                        objCorpAct.bcrduniqueness = 1;
                    }
                    #region Change Check
                    //objCorpAct.exDate = Convert.ToDateTime(BookClosureFrom).ToString("yyyy-MM-dd");
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
            
            objCorpActBSE = objCorpActBSE.OrderBy(x => x.Value.exDate).ToDictionary(x => x.Key, x => x.Value);
        }


        private static void ReadDerivativeSpreadMasterBSE()
        {
            try
            {
                oDataAccessLayer.Connect((int)DataAccessLayer.ConnectionDB.Masters);
                oDataAccessLayer.OpenConnection((int)DataAccessLayer.ConnectionDB.Masters);

                string strQuery = @"SELECT ContractTokenNum,ContractTokenNum_Leg1,ContractTokenNum_Leg2,NTAScripCode,StrategyID,NoofLegsinStrategy,Eligibility,
                                    ComplexInstrumentType FROM BSE_DERIVATIVE_SPD_CFE;";

                SQLiteDataReader oSQLiteDataReader = oDataAccessLayer.ExecuteDataReader((int)ConnectionDB.Masters,strQuery, System.Data.CommandType.Text, null);


                while (oSQLiteDataReader.Read())
                {
                    DerivativeMaster objDervMaster = new DerivativeMaster();

                    //ContractTokenNum
                    var ContractTokenNum = oSQLiteDataReader["ContractTokenNum"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(ContractTokenNum))
                        objDervMaster.ContractTokenNum = Convert.ToInt64(ContractTokenNum);

                    //ContractTokenNum_Leg1
                    var ContractTokenNum_Leg1 = oSQLiteDataReader["ContractTokenNum_Leg1"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(ContractTokenNum_Leg1))
                        objDervMaster.ContractTokenNum_Leg1 = Convert.ToInt32(ContractTokenNum_Leg1);

                    //ContractTokenNum_Leg2
                    var ContractTokenNum_Leg2 = oSQLiteDataReader["ContractTokenNum_Leg2"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(ContractTokenNum_Leg2))
                        objDervMaster.ContractTokenNum_Leg2 = Convert.ToInt32(ContractTokenNum_Leg2);

                    //NTAScripCode
                    var NTAScripCode = oSQLiteDataReader["NTAScripCode"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(NTAScripCode))
                        objDervMaster.NTAScripCode = Convert.ToInt64(NTAScripCode);

                    //StrategyID
                    var StrategyID = oSQLiteDataReader["StrategyID"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(StrategyID))
                        objDervMaster.StrategyID = Convert.ToInt32(StrategyID);

                    //NoofLegsinStrategy
                    var NoofLegsinStrategy = oSQLiteDataReader["NoofLegsinStrategy"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(NoofLegsinStrategy))
                        objDervMaster.NoofLegsinStrategy = Convert.ToInt32(NoofLegsinStrategy);

                    //Eligibility
                    var Eligibility = oSQLiteDataReader["Eligibility"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(Eligibility))
                        objDervMaster.Eligibility = Convert.ToChar(Eligibility);

                    //ComplexInstrumentType
                    var ComplexInstrumentType = oSQLiteDataReader["ComplexInstrumentType"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(ComplexInstrumentType))
                        objDervMaster.ComplexInstrumentType = Convert.ToInt32(ComplexInstrumentType);

                    if (!objMasterDerivativeSpreadDictBaseBSE.ContainsKey(objDervMaster.ContractTokenNum))
                    {
                        objMasterDerivativeSpreadDictBaseBSE.Add(objDervMaster.ContractTokenNum, objDervMaster);
                    }
                }
                objMasterDerivativeSpreadDictBaseBSE = objMasterDerivativeSpreadDictBaseBSE.OrderBy(x => x.Value.ContractTokenNum).ToDictionary(x => x.Key, x => x.Value);
            }
            catch (Exception ex)
            {

            }
            finally
            {
                oDataAccessLayer.CloseConnection((int)DataAccessLayer.ConnectionDB.Masters);
                System.Data.SQLite.SQLiteConnection.ClearAllPools();
            }
        }

        private static void ReadScripGroup()
        {
            try
            {
                //GroupWiseLimitsVM.GroupWiseLimitsCollection = new ObservableCollection<GroupWiseLimitsModel>();
                GroupWiseLimitDict = new ConcurrentDictionary<string, GroupWiseLimitsModel>();

                oDataAccessLayer.Connect((int)DataAccessLayer.ConnectionDB.Masters);
                oDataAccessLayer.OpenConnection((int)DataAccessLayer.ConnectionDB.Masters);
                // ScripGrp = new List<string>();
                //SelectedScripGrp = ScripProfilingModel.FutType._All.ToString().Replace("_", " ");
                string str = "SELECT Distinct(GroupName) FROM BSE_SECURITIES_CFE;";
                SQLiteDataReader oSQLiteDataReader = oDataAccessLayer.ExecuteDataReader((int)ConnectionDB.Masters,str, System.Data.CommandType.Text, null);
                while (oSQLiteDataReader.Read())
                {
                    GroupWiseLimitsModel objgwlmodel = new GroupWiseLimitsModel();


                    if (oSQLiteDataReader["GroupName"] != string.Empty)
                        objgwlmodel.Group = oSQLiteDataReader["GroupName"]?.ToString().Trim();

                    objgwlmodel.BuyValue = -1;
                    objgwlmodel.SellValue = -1;
                    objgwlmodel.AvlBuy = -1 * 100000;
                    objgwlmodel.AvlSell = -1 * 100000;

                    //    GroupWiseLimitsVM.GroupWiseLimitsCollection.Add(objgwlmodel);
                    GroupWiseLimitDict.TryAdd(objgwlmodel.Group, objgwlmodel);

                }

                if (!GroupWiseLimitDict.Keys.Contains("DF"))
                {
                    GroupWiseLimitsModel objgwlmodel = new GroupWiseLimitsModel();
                    objgwlmodel.Group = "DF";
                    objgwlmodel.BuyValue = -1;
                    objgwlmodel.SellValue = -1;
                    objgwlmodel.AvlBuy = -1 * 100000;
                    objgwlmodel.AvlSell = -1 * 100000;
                    GroupWiseLimitDict.TryAdd(objgwlmodel.Group, objgwlmodel);
                }

                if (!GroupWiseLimitDict.Keys.Contains("CD"))
                {
                    GroupWiseLimitsModel objgwlmodel = new GroupWiseLimitsModel();
                    objgwlmodel.Group = "CD";
                    objgwlmodel.BuyValue = -1;
                    objgwlmodel.SellValue = -1;
                    objgwlmodel.AvlBuy = -1 * 100000;
                    objgwlmodel.AvlSell = -1 * 100000;
                    GroupWiseLimitDict.TryAdd(objgwlmodel.Group, objgwlmodel);
                }

                // GroupWiseLimitsVM.GroupWiseLimitsCollection.OrderBy(x => x.Group);





            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                oDataAccessLayer.CloseConnection((int)DataAccessLayer.ConnectionDB.Masters);
                System.Data.SQLite.SQLiteConnection.ClearAllPools();
            }
        }


        //private static void ReadTraderEntitlementDB()
        //{
        //    try
        //    {
        //        oDataAccessLayer.OpenConnection((int)DataAccessLayer.ConnectionDB.Masters);

        //        //string strQuery = @"SELECT ScripCode,ScripId,ScripName,GroupName,FaceValue,MarketLot,TickSize,Filler2_GSM,InstrumentType,SCID
        //        //                    FROM BSE_SECURITIES_CFE";


        //        string strQuery = @"SELECT TraderID,ClientViewOnTraderTerminal,OrderPlacementOutsideClientMaster,ViewClientsOnLevel,FourLakhSeries,SixLakhSeries,Filler,AuctionBlock,BlockDealBlock,OddLotBlock,InstitutionalTradingBlock,NotApplicable
        //                            FROM TRADER_RIGHTS";



        //        SQLiteDataReader oSQLiteDataReader = oDataAccessLayer.ExecuteDataReader((int)ConnectionDB.Masters,strQuery, System.Data.CommandType.Text, null);

        //        while (oSQLiteDataReader.Read())
        //        {
        //            TraderEntitlementModel oTraderEntitlementModel = new TraderEntitlementModel();

        //            //MarketLot
        //            var TraderID = oSQLiteDataReader["TraderID"]?.ToString().Trim();
        //            if (!string.IsNullOrEmpty(TraderId))
        //                oTraderEntitlementModel.TraderId = Convert.ToInt32(TraderId);

        //            //GroupName
        //            objScripMaster.GroupName = oSQLiteDataReader["GroupName"]?.ToString().Trim();

        //            //FaceValue in paise
        //            var FaceValue = oSQLiteDataReader["FaceValue"]?.ToString().Trim();
        //            if (!string.IsNullOrEmpty(FaceValue))
        //                objScripMaster.FaceValue = Convert.ToInt64(FaceValue);

        //            //Filler2_GSM flag
        //            var Filler2_GSM = oSQLiteDataReader["Filler2_GSM"]?.ToString().Trim();
        //            if (!string.IsNullOrEmpty(Filler2_GSM))
        //                objScripMaster.Filler2_GSM = Convert.ToInt32(Filler2_GSM);

        //            //TickSize
        //            var TickSize = oSQLiteDataReader["TickSize"]?.ToString().Trim();
        //            if (!string.IsNullOrEmpty(TickSize))
        //                objScripMaster.TickSize = Convert.ToDouble(TickSize);

        //            //ScripCode
        //            var ScripCode = oSQLiteDataReader["ScripCode"]?.ToString().Trim();
        //            if (!string.IsNullOrEmpty(ScripCode))
        //                objScripMaster.ScripCode = Convert.ToInt64(ScripCode);

        //            //InstrumentType
        //            var InstrumentType = oSQLiteDataReader["InstrumentType"].ToString().Trim();
        //            if (!string.IsNullOrEmpty(InstrumentType))
        //                objScripMaster.InstrumentType = Convert.ToChar(InstrumentType.Truncate(1));

        //            //ScripId
        //            objScripMaster.ScripId = oSQLiteDataReader["ScripId"]?.ToString().Trim();

        //            //ScripName
        //            objScripMaster.ScripName = oSQLiteDataReader["ScripName"]?.ToString().Trim();

        //            //BOWID
        //            var SCID = oSQLiteDataReader["SCID"]?.ToString().Trim();
        //            if (!string.IsNullOrEmpty(SCID))
        //                objScripMaster.BowTokenID = Convert.ToInt32(SCID);

        //            //IMPercentage from Bcast
        //            objScripMaster.IMPercentage = 0.0m;

        //            //EMPercentage from Bcast
        //            objScripMaster.EMPercentage = 0.0m;

        //            objScripMaster.Precision = 2;
        //            //if (!objMastertxtDictBase.ContainsKey(objScripMaster.ScripCode))
        //            if (!objMastertxtDictBaseBSE.ContainsKey(823460))
        //            {
        //                //   objMastertxtDictBase.Add(objScripMaster.ScripCode, objScripMaster);
        //                objMastertxtDictBaseBSE.Add(objScripMaster.ScripCode, objScripMaster);
        //            }

        //            #region Commented
        //            //var BcEndDate = oSQLiteDataReader["BcEndDate"]?.ToString().Trim();
        //            //if (!string.IsNullOrEmpty(BcEndDate))
        //            //    objScipMaster.BcEndDate = BcEndDate;

        //            //var BcStartDate = oSQLiteDataReader["BcStartDate"]?.ToString().Trim();
        //            //if (!string.IsNullOrEmpty(BcStartDate))
        //            //    objScipMaster.BcStartDate = BcStartDate;

        //            //var BseExclusive = oSQLiteDataReader["BseExclusive"]?.ToString().Trim();
        //            //if (!string.IsNullOrEmpty(BseExclusive))
        //            //    objScipMaster.BseExclusive = Convert.ToChar(BseExclusive);

        //            //var CallAuctionIndicator = oSQLiteDataReader["CallAuctionIndicator"]?.ToString().Trim();
        //            //if (!string.IsNullOrEmpty(CallAuctionIndicator))
        //            //    objScipMaster.CallAuctionIndicator = Convert.ToInt32(CallAuctionIndicator);

        //            //var ExBonusDate = oSQLiteDataReader["ExBonusDate"]?.ToString().Trim();
        //            //if (!string.IsNullOrEmpty(ExBonusDate))
        //            //    objScipMaster.ExBonusDate = ExBonusDate;

        //            //objScipMaster.ExchangeCode = oSQLiteDataReader["ExchangeCode"]?.ToString().Trim();

        //            //var ExDivDate = oSQLiteDataReader["ExDivDate"]?.ToString().Trim();
        //            //if (!string.IsNullOrEmpty(ExDivDate))
        //            //    objScipMaster.ExDivDate = ExDivDate;

        //            //objScipMaster.ExpiryDate =;
        //            //var ExRightDate = oSQLiteDataReader["ExRightDate"]?.ToString().Trim();
        //            //if (!string.IsNullOrEmpty(ExRightDate))
        //            //    objScipMaster.ExRightDate = ExRightDate;

        //            //var FaceValue = oSQLiteDataReader["FaceValue"]?.ToString().Trim();
        //            //if (!string.IsNullOrEmpty(FaceValue))
        //            //    objScipMaster.FaceValue = Convert.ToInt64(FaceValue);

        //            //objScipMaster.Filler = oSQLiteDataReader["Filler"]?.ToString().Trim();

        //            //var Filler2 = oSQLiteDataReader["Filler2_GSM"]?.ToString().Trim();
        //            //if (!string.IsNullOrEmpty(Filler2))
        //            //    objScipMaster.Filler2 = Convert.ToInt32(Filler2);

        //            //var Filler3 = oSQLiteDataReader["Filler3"]?.ToString().Trim();
        //            //if (!string.IsNullOrEmpty(Filler3))
        //            //    objScipMaster.Filler3 = Convert.ToInt32(Filler3);

        //            //objScipMaster.GroupName = oSQLiteDataReader["GroupName"]?.ToString().Trim();
        //            //objScipMaster.IsinCode = oSQLiteDataReader["IsinCode"]?.ToString().Trim();

        //            //var MarketLot = oSQLiteDataReader["MarketLot"]?.ToString().Trim();
        //            //if (!string.IsNullOrEmpty(MarketLot))
        //            //    objScipMaster.MarketLot = Convert.ToInt32(MarketLot);
        //            //objScipMaster.MinimumLotSize =;
        //            //objScipMaster.NewTickSize = oSQLiteDataReader["NewTickSize"]?.ToString().Trim();

        //            //var NoDelEndDate = oSQLiteDataReader["NoDelEndDate"]?.ToString().Trim();
        //            //if (!string.IsNullOrEmpty(NoDelEndDate))
        //            //    objScipMaster.NoDelEndDate = NoDelEndDate;

        //            //var NoDelStartDate = oSQLiteDataReader["NoDelStartDate"]?.ToString().Trim();
        //            //if (!string.IsNullOrEmpty(NoDelStartDate))
        //            //    objScipMaster.NoDelStartDate = NoDelStartDate;

        //            //objScipMaster.PartitionId = oSQLiteDataReader["PartitionId"]?.ToString().Trim();

        //            //var ScripCode = oSQLiteDataReader["ScripCode"]?.ToString().Trim();
        //            //if (!string.IsNullOrEmpty(ScripCode))
        //            //    objScipMaster.ScripCode = Convert.ToInt64(ScripCode);

        //            //objScipMaster.ScripId = oSQLiteDataReader["ScripId"]?.ToString().Trim();
        //            //objScipMaster.ScripName = oSQLiteDataReader["ScripName"]?.ToString().Trim();

        //            //var ScripType = oSQLiteDataReader["ScripType"]?.ToString().Trim();
        //            //if (!string.IsNullOrEmpty(ScripType))
        //            //    objScipMaster.ScripType = Convert.ToChar(ScripType);

        //            //var SegmentFlag = oSQLiteDataReader["InstrumentType"]?.ToString().Trim();
        //            //if (!string.IsNullOrEmpty(SegmentFlag))
        //            //{
        //            //    objScipMaster.SegmentFlag = Convert.ToChar(SegmentFlag);

        //            //    if (objScipMaster.SegmentFlag == 'E')
        //            //    {
        //            //        objScipMaster.InstrumentType = 0;
        //            //        objScipMaster.DecimalPoint = 2;
        //            //    }
        //            //    else if (objScipMaster.SegmentFlag == 'D')
        //            //    {
        //            //        objScipMaster.InstrumentType = 1;
        //            //        objScipMaster.DecimalPoint = 2;
        //            //    }
        //            //    else if (objScipMaster.SegmentFlag == 'C')
        //            //    {
        //            //        objScipMaster.InstrumentType = 2;
        //            //        objScipMaster.DecimalPoint = 4;
        //            //    }
        //            //}
        //            //var Status = oSQLiteDataReader["Status"]?.ToString().Trim();
        //            //if (!string.IsNullOrEmpty(Status))
        //            //    objScipMaster.Status = Convert.ToChar(Status);
        //            //objScipMaster.strikePrice =;
        //            //objScipMaster.StrikePrice =;
        //            //var TickSize = oSQLiteDataReader["TickSize"]?.ToString().Trim();
        //            //if (!string.IsNullOrEmpty(TickSize))
        //            //    objScipMaster.TickSize = Convert.ToDouble(TickSize);


        //            //if (!objMastertxtDic.ContainsKey(objScipMaster.ScripCode))
        //            //{
        //            //    objMastertxtDic.Add(objScipMaster.ScripCode, objScipMaster);
        //            //}
        //            //if (!objscripCodeDict.ContainsKey(Convert.ToInt32(objScipMaster.ScripCode)))//TBD Gaurav Jadhav 11/07/2017. Need to discuss with MJ
        //            //{
        //            //    objscripCodeDict.Add(Convert.ToInt32(objScipMaster.ScripCode), objScipMaster.ScripId);
        //            //}

        //            #endregion

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //ExceptionUtility.LogError(ex);
        //    }
        //    finally
        //    {
        //        oDataAccessLayer.CloseConnection((int)DataAccessLayer.ConnectionDB.Masters);
        //    }
        //}
        private static void ReadDerivativeCPCode()
        {
            int cnt = 0, line = 1;
            //lines = File.ReadAllLines(Path.GetFullPath(Path.Combine(System.Environment.CurrentDirectory, @"Master files/BSEDATA.ini"))).ToList<string>();
            try
            {
                string mastercsv = GetLatestCoFile("EQD_PARTICIPANT*.TXT", 27);
                if (mastercsv == string.Empty)
                {
                    System.Windows.MessageBox.Show("Please download EQD_PARTICIPANT Master file.");
                    System.Windows.MessageBox.Show("Proceeding with back date data.", "Important Message");
                    return;
                }
                else
                {
                    if (!string.IsNullOrEmpty(mastercsv))
                    {
                        string[] strScrips = File.ReadAllLines(mastercsv);
                        string[] strScripLine = strScrips[0].Split('|');
                        //int NoOfConts = Convert.ToInt32(strScripLine[2]);
                        for (int i = 1; i < strScrips.Count(); i++)
                        {
                            try
                            {
                                CPCodeDerivative objCPCodeDerivative = new CPCodeDerivative();
                                string[] strScrip = strScrips[i].Split('|');
                                strScrip = ConvertDatatoFormat(strScrip);
                                objCPCodeDerivative.ParticipantId = strScrip[0];
                                objCPCodeDerivative.ParticipantName = strScrip[1];
                                objCPCodeDerivative.ParticipantStatus = Convert.ToChar(strScrip[2]);
                                objCPCodeDerivative.DeleteFlag = Convert.ToChar(strScrip[3]);
                                objCPCodeDerivative.InfoDate = strScrip[4];
                                
                                if (!objCPCodeDerivativeDict.ContainsKey(objCPCodeDerivative.ParticipantId))
                                {
                                    objCPCodeDerivativeDict.Add(objCPCodeDerivative.ParticipantId, objCPCodeDerivative);
                                    line++;
                                }

                            }
                            catch (Exception)
                            {
                                System.Windows.MessageBox.Show("Error while reading EQD_Participant at line number: " + line, "Important Message");
                                if (MessageBox.Show("Do you want to close the exe", "Important Message", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes)
                                    Process.GetCurrentProcess().Kill();
                                else
                                    return;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
                throw;
            }
        }

        private static void ReadCurrencyCPCode()
        {
            int cnt = 0, line = 1;
            //lines = File.ReadAllLines(Path.GetFullPath(Path.Combine(System.Environment.CurrentDirectory, @"Master files/BSEDATA.ini"))).ToList<string>();
            try
            {
                string mastercsv = GetLatestCoFile("BFX_PARTICIPANT*.TXT", 27);
                if (mastercsv == string.Empty)
                {
                    System.Windows.MessageBox.Show("Please download BFX_PARTICIPANT Master file.");
                    System.Windows.MessageBox.Show("Proceeding with back date data.", "Important Message");
                    return;
                }
                else
                {
                    if (!string.IsNullOrEmpty(mastercsv))
                    {
                        string[] strScrips = File.ReadAllLines(mastercsv);
                        string[] strScripLine = strScrips[0].Split('|');
                        //int NoOfConts = Convert.ToInt32(strScripLine[2]);
                        for (int i = 1; i < strScrips.Count(); i++)
                        {
                            try
                            {
                                CPCodeCurrency objCPCodeCurrency = new CPCodeCurrency();
                                string[] strScrip = strScrips[i].Split('|');
                                strScrip = ConvertDatatoFormat(strScrip);
                                objCPCodeCurrency.ParticipantId = strScrip[0];
                                objCPCodeCurrency.ParticipantName = strScrip[1];
                                objCPCodeCurrency.ParticipantStatus = Convert.ToChar(strScrip[2]);
                                objCPCodeCurrency.DeleteFlag = Convert.ToChar(strScrip[3]);
                                objCPCodeCurrency.InfoDate = strScrip[4];

                                if (!objCPCodeCurrencyDict.ContainsKey(objCPCodeCurrency.ParticipantId))
                                {
                                    objCPCodeCurrencyDict.Add(objCPCodeCurrency.ParticipantId, objCPCodeCurrency);
                                    line++;
                                }

                            }
                            catch (Exception)
                            {
                                System.Windows.MessageBox.Show("Error while reading EQD_Participant at line number: " + line, "Important Message");
                                if (MessageBox.Show("Do you want to close the exe", "Important Message", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes)
                                    Process.GetCurrentProcess().Kill();
                                else
                                    return;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
                throw;
            }
        }

        private static void InsertDefaultProfileInUserTable()
        {
            ExchangeDefaultColumns = new ObservableCollection<TouchlineWindow>();

            //            foreach (var item in objTouchlineCollection)
            //            {
            //                try
            //                {
            //                    MasterSharedMemory.oDataAccessLayer.OpenConnection((int)DataAccessLayer.ConnectionDB.Masters);

            //#if TWS
            //                   string str = @"INSERT INTO USER_DEFINED_PROFILE(MemberID,TraderID,ScreenName,ColumnName,ProfileName,ColPriorityVal,DefProfile)
            //                                 VALUES( " + "'" + UtilityLoginDetails.GETInstance.MemberId + "'," + "'" + UtilityLoginDetails.GETInstance.TraderId + "'," + "'" + item.ScreenName + "'," +
            //                                              "'" + item.FieldName + "'," + "'DEFAULT'," + "'" + item.DefaultColumns + "'," + "'True');";
            //#elif BOW
            //                    str = @"INSERT INTO USER_DEFINED_PROFILE(MemberID,TraderID,ScreenName,ColumnName,ProfileName,ColPriorityVal,DefProfile)
            //                                 VALUES( " + "'" + UtilityLoginDetails.GETInstance.UserLoginId+ "'," + "'NA'," + "'" + item.ScreenName + "'," +
            //                                              "'" + item.FieldName + "'," + "'Default'," + "'" + item.DefaultColumns + "'," + "'True');";
            //#endif
            //                    int result = MasterSharedMemory.oDataAccessLayer.ExecuteNonQuery((int)ConnectionDB.Masters,str, System.Data.CommandType.Text, null);
            //                }
            //                catch (Exception ex)
            //                {
            //                    //MessageBox.Show("Error While Creating Default Profile", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            //                    return;
            //                }
            //                finally
            //                {
            //                    MasterSharedMemory.oDataAccessLayer.CloseConnection((int)DataAccessLayer.ConnectionDB.Masters);
            //                }
            //            }

        }

        /// <summary>
        /// Reads scrip masters for BSE
        /// </summary>
        //private static void ReadScripMasterForSCRIP_txt()
        private static void ReadScripMasterBSE()
        {
            try
            {
                oDataAccessLayer.OpenConnection((int)DataAccessLayer.ConnectionDB.Masters);
                string strQuery = @"SELECT ScripCode,ScripId,ScripName,GroupName,FaceValue,MarketLot,TickSize,Filler2_GSM,InstrumentType,SCID,PartitionId,IsinCode
                                    FROM BSE_SECURITIES_CFE ORDER BY ScripId ASC";

                SQLiteDataReader oSQLiteDataReader = oDataAccessLayer.ExecuteDataReader((int)ConnectionDB.Masters,strQuery, System.Data.CommandType.Text, null);

                while (oSQLiteDataReader.Read())
                {
                    ScripMasterBase objScripMaster = new ScripMasterBase();

                    //Partition ID
                    var partition = oSQLiteDataReader["PartitionId"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(partition))
                    {
                        if (partition.Contains("-"))
                        {
                            var arrData = partition.Split('-');
                            if (arrData.Length > 0)
                            {
                                objScripMaster.PartitionId = arrData[0];
                                objScripMaster.MarketSegmentID = arrData[1];
                            }
                        }
                    }


                    //MarketLot
                    var MarketLot = oSQLiteDataReader["MarketLot"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(MarketLot))
                        objScripMaster.MarketLot = Convert.ToInt32(MarketLot);

                    //GroupName
                    objScripMaster.GroupName = oSQLiteDataReader["GroupName"]?.ToString().Trim();

                    //FaceValue in paise
                    var FaceValue = oSQLiteDataReader["FaceValue"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(FaceValue))
                        objScripMaster.FaceValue = Convert.ToInt64(FaceValue);

                    //Filler2_GSM flag
                    var Filler2_GSM = oSQLiteDataReader["Filler2_GSM"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(Filler2_GSM))
                        objScripMaster.Filler2_GSM = Convert.ToInt32(Filler2_GSM);

                    //TickSize
                    var TickSize = oSQLiteDataReader["TickSize"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(TickSize))
                        objScripMaster.TickSize = Convert.ToDouble(TickSize);

                    //ScripCode
                    var ScripCode = oSQLiteDataReader["ScripCode"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(ScripCode))
                        objScripMaster.ScripCode = Convert.ToInt64(ScripCode);

                    //InstrumentType
                    

                    var InstrumentType = oSQLiteDataReader["InstrumentType"].ToString().Trim();
                    if (!string.IsNullOrEmpty(InstrumentType))
                    {
                        objScripMaster.InstrumentType = Convert.ToChar(InstrumentType.Truncate(1));

                        //objScripMaster.Precision = objScripMaster.InstrumentType == 'C' ? 4 : 2;

                        if (objScripMaster.InstrumentType == 'C')
                        {
                            objScripMaster.Precision = 4;
                            objScripMaster.TickSize *= 10000;
                        }
                        else
                        {
                            objScripMaster.Precision = 2;
                            objScripMaster.TickSize *= 100;
                        }

                    }

                    //ScripId
                    objScripMaster.ScripId = oSQLiteDataReader["ScripId"]?.ToString().Trim();

                    //ScripName
                    objScripMaster.ScripName = oSQLiteDataReader["ScripName"]?.ToString().Trim();

                    //BOWID
                    var SCID = oSQLiteDataReader["SCID"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(SCID))
                        objScripMaster.BowTokenID = Convert.ToInt32(SCID);

                    //IMPercentage from Bcast
                    objScripMaster.IMPercentage = 0.0m;

                    //EMPercentage from Bcast
                    objScripMaster.EMPercentage = 0.0m;



                    //Segment For Equity
                    objScripMaster.Segment = 1;

                    //IsinCode. Added by Gaurav 16/5/2018
                    var IsinCode = oSQLiteDataReader["IsinCode"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(IsinCode))
                    {
                        objScripMaster.IsinCode = IsinCode;
                    }
                    else
                    {
                        objScripMaster.IsinCode = "NA";
                    }

                    if (!objMastertxtDictBaseBSE.ContainsKey(objScripMaster.ScripCode))
                    {
                        objMastertxtDictBaseBSE.Add(objScripMaster.ScripCode, objScripMaster);
                    }

                    #region Commented
                    //var BcEndDate = oSQLiteDataReader["BcEndDate"]?.ToString().Trim();
                    //if (!string.IsNullOrEmpty(BcEndDate))
                    //    objScipMaster.BcEndDate = BcEndDate;

                    //var BcStartDate = oSQLiteDataReader["BcStartDate"]?.ToString().Trim();
                    //if (!string.IsNullOrEmpty(BcStartDate))
                    //    objScipMaster.BcStartDate = BcStartDate;

                    //var BseExclusive = oSQLiteDataReader["BseExclusive"]?.ToString().Trim();
                    //if (!string.IsNullOrEmpty(BseExclusive))
                    //    objScipMaster.BseExclusive = Convert.ToChar(BseExclusive);

                    //var CallAuctionIndicator = oSQLiteDataReader["CallAuctionIndicator"]?.ToString().Trim();
                    //if (!string.IsNullOrEmpty(CallAuctionIndicator))
                    //    objScipMaster.CallAuctionIndicator = Convert.ToInt32(CallAuctionIndicator);

                    //var ExBonusDate = oSQLiteDataReader["ExBonusDate"]?.ToString().Trim();
                    //if (!string.IsNullOrEmpty(ExBonusDate))
                    //    objScipMaster.ExBonusDate = ExBonusDate;

                    //objScipMaster.ExchangeCode = oSQLiteDataReader["ExchangeCode"]?.ToString().Trim();

                    //var ExDivDate = oSQLiteDataReader["ExDivDate"]?.ToString().Trim();
                    //if (!string.IsNullOrEmpty(ExDivDate))
                    //    objScipMaster.ExDivDate = ExDivDate;

                    //objScipMaster.ExpiryDate =;
                    //var ExRightDate = oSQLiteDataReader["ExRightDate"]?.ToString().Trim();
                    //if (!string.IsNullOrEmpty(ExRightDate))
                    //    objScipMaster.ExRightDate = ExRightDate;

                    //var FaceValue = oSQLiteDataReader["FaceValue"]?.ToString().Trim();
                    //if (!string.IsNullOrEmpty(FaceValue))
                    //    objScipMaster.FaceValue = Convert.ToInt64(FaceValue);

                    //objScipMaster.Filler = oSQLiteDataReader["Filler"]?.ToString().Trim();

                    //var Filler2 = oSQLiteDataReader["Filler2_GSM"]?.ToString().Trim();
                    //if (!string.IsNullOrEmpty(Filler2))
                    //    objScipMaster.Filler2 = Convert.ToInt32(Filler2);

                    //var Filler3 = oSQLiteDataReader["Filler3"]?.ToString().Trim();
                    //if (!string.IsNullOrEmpty(Filler3))
                    //    objScipMaster.Filler3 = Convert.ToInt32(Filler3);

                    //objScipMaster.GroupName = oSQLiteDataReader["GroupName"]?.ToString().Trim();
                    //objScipMaster.IsinCode = oSQLiteDataReader["IsinCode"]?.ToString().Trim();

                    //var MarketLot = oSQLiteDataReader["MarketLot"]?.ToString().Trim();
                    //if (!string.IsNullOrEmpty(MarketLot))
                    //    objScipMaster.MarketLot = Convert.ToInt32(MarketLot);
                    //objScipMaster.MinimumLotSize =;
                    //objScipMaster.NewTickSize = oSQLiteDataReader["NewTickSize"]?.ToString().Trim();

                    //var NoDelEndDate = oSQLiteDataReader["NoDelEndDate"]?.ToString().Trim();
                    //if (!string.IsNullOrEmpty(NoDelEndDate))
                    //    objScipMaster.NoDelEndDate = NoDelEndDate;

                    //var NoDelStartDate = oSQLiteDataReader["NoDelStartDate"]?.ToString().Trim();
                    //if (!string.IsNullOrEmpty(NoDelStartDate))
                    //    objScipMaster.NoDelStartDate = NoDelStartDate;

                    //objScipMaster.PartitionId = oSQLiteDataReader["PartitionId"]?.ToString().Trim();

                    //var ScripCode = oSQLiteDataReader["ScripCode"]?.ToString().Trim();
                    //if (!string.IsNullOrEmpty(ScripCode))
                    //    objScipMaster.ScripCode = Convert.ToInt64(ScripCode);

                    //objScipMaster.ScripId = oSQLiteDataReader["ScripId"]?.ToString().Trim();
                    //objScipMaster.ScripName = oSQLiteDataReader["ScripName"]?.ToString().Trim();

                    //var ScripType = oSQLiteDataReader["ScripType"]?.ToString().Trim();
                    //if (!string.IsNullOrEmpty(ScripType))
                    //    objScipMaster.ScripType = Convert.ToChar(ScripType);

                    //var SegmentFlag = oSQLiteDataReader["InstrumentType"]?.ToString().Trim();
                    //if (!string.IsNullOrEmpty(SegmentFlag))
                    //{
                    //    objScipMaster.SegmentFlag = Convert.ToChar(SegmentFlag);

                    //    if (objScipMaster.SegmentFlag == 'E')
                    //    {
                    //        objScipMaster.InstrumentType = 0;
                    //        objScipMaster.DecimalPoint = 2;
                    //    }
                    //    else if (objScipMaster.SegmentFlag == 'D')
                    //    {
                    //        objScipMaster.InstrumentType = 1;
                    //        objScipMaster.DecimalPoint = 2;
                    //    }
                    //    else if (objScipMaster.SegmentFlag == 'C')
                    //    {
                    //        objScipMaster.InstrumentType = 2;
                    //        objScipMaster.DecimalPoint = 4;
                    //    }
                    //}
                    //var Status = oSQLiteDataReader["Status"]?.ToString().Trim();
                    //if (!string.IsNullOrEmpty(Status))
                    //    objScipMaster.Status = Convert.ToChar(Status);
                    //objScipMaster.strikePrice =;
                    //objScipMaster.StrikePrice =;
                    //var TickSize = oSQLiteDataReader["TickSize"]?.ToString().Trim();
                    //if (!string.IsNullOrEmpty(TickSize))
                    //    objScipMaster.TickSize = Convert.ToDouble(TickSize);


                    //if (!objMastertxtDic.ContainsKey(objScipMaster.ScripCode))
                    //{
                    //    objMastertxtDic.Add(objScipMaster.ScripCode, objScipMaster);
                    //}
                    //if (!objscripCodeDict.ContainsKey(Convert.ToInt32(objScipMaster.ScripCode)))//TBD Gaurav Jadhav 11/07/2017. Need to discuss with MJ
                    //{
                    //    objscripCodeDict.Add(Convert.ToInt32(objScipMaster.ScripCode), objScipMaster.ScripId);
                    //}

                    #endregion

                }
                objMastertxtDictBaseBSE = objMastertxtDictBaseBSE.OrderBy(x => x.Value.ScripId).ToDictionary(x => x.Key, x => x.Value);
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

        private static void ReadClientMasters_txt()
        {
            objClientMasterDict = new ConcurrentDictionary<string, ClientMaster>();
            try
            {

                oDataAccessLayer.OpenConnection((int)DataAccessLayer.ConnectionDB.Masters);

                string strQuery = @"SELECT FirstName,
       MiddleName,
       LastName,
       ClientType,
       ClientStatus,
       MobileNum,
       Email,
       ShortClientId,
       CompleteClientId,
       CPCodeDerivative,
       CPCodeCurrency,
       OwnerLoginId,
       UserLoginId,
       Password,
       MobilePwd,
       TransactionPwd,
       Salutation,
       PanCardNum
  FROM CLIENT_MASTER;";


                SQLiteDataReader oSQLiteDataReader = oDataAccessLayer.ExecuteDataReader((int)ConnectionDB.Masters,strQuery, System.Data.CommandType.Text, null);

                while (oSQLiteDataReader.Read())
                {
                    ClientMaster objClientMasters = new ClientMaster();

                    //FirstName
                    var FirstName = oSQLiteDataReader["FirstName"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(FirstName))
                        objClientMasters.FirstName = FirstName;

                    //LastName
                    var LastName = oSQLiteDataReader["LastName"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(LastName))
                        objClientMasters.LastName = LastName;

                    //Type
                    var ClientType = oSQLiteDataReader["ClientType"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(ClientType))
                        objClientMasters.ClientType = ClientType;

                    //Status
                    var ClientStatus = oSQLiteDataReader["ClientStatus"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(ClientStatus))
                        objClientMasters.ClientStatus = ClientStatus;

                    //MobileNum
                    var MobileNumber = oSQLiteDataReader["MobileNum"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(MobileNumber))
                        objClientMasters.MobileNumber = MobileNumber;

                    //EmailID
                    var EmailID = oSQLiteDataReader["Email"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(EmailID))
                        objClientMasters.EmailID = EmailID;

                    //ShortClientId
                    var ShortClientId = oSQLiteDataReader["ShortClientId"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(ShortClientId))
                        objClientMasters.ShortClientId = ShortClientId;

                    //CompleteClientId
                    var CompleteClientId = oSQLiteDataReader["CompleteClientId"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(CompleteClientId))
                        objClientMasters.CompleteClientId = CompleteClientId;

                    //CPCodeDerivative
                    var CPCodeDerivative = oSQLiteDataReader["CPCodeDerivative"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(CPCodeDerivative))
                        objClientMasters.CPCodeDerivative = CPCodeDerivative;

                    //CPCodeCurrency
                    var CPCodeCurrency = oSQLiteDataReader["CPCodeCurrency"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(CPCodeCurrency))
                        objClientMasters.CPCodeCurrency = CPCodeCurrency;

                    ////CPCodeCurrency
                    //var SerialNo = oSQLiteDataReader["SerialNo"]?.ToString().Trim();
                    //if (!string.IsNullOrEmpty(SerialNo))
                    //    objClientMasters.SerialNo = SerialNo;

                    var matchkeyClientMaster = string.Format("{0}_{1}", CompleteClientId, ClientType);

                    objClientMasterDict.TryAdd(matchkeyClientMaster, objClientMasters);

                    //var SerialNo = string.Format("{0}_{1}", CompleteClientId, ClientType);

                }

                ////MobilePwd
                //var Salutation = oSQLiteDataReader["Salutation"]?.ToString().Trim();
                //if (!string.IsNullOrEmpty(Salutation))
                //    objClientMasters.Salutation = Salutation;

                //// PanCardNum
                //var PanCardNum = oSQLiteDataReader["PanCardNum"]?.ToString().Trim();
                //if (!string.IsNullOrEmpty(PanCardNum))
                //    objClientMasters.PanCardNum = PanCardNum;

                //objClientMasterLst.Add(objClientMasters);

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
        /// <summary>
        /// Reads scrip masters for NSE
        /// </summary>
        private static void ReadScripMasterNSE()
        {
            try
            {
                oDataAccessLayer.OpenConnection((int)DataAccessLayer.ConnectionDB.Masters);

                string strQuery = @"Select ScripId,ScripCode,ScripName,SCID,MarketLot,GroupName,FaceValue,TickSize,'0' as 'Filler2_GSM', 'E' as 'InstrumentType'
                                    FROM NSE_SECURITIES_CFE ORDER BY ScripId ASC";

                SQLiteDataReader oSQLiteDataReader = oDataAccessLayer.ExecuteDataReader((int)ConnectionDB.Masters,strQuery, System.Data.CommandType.Text, null);

                while (oSQLiteDataReader.Read())
                {
                    ScripMasterBase objScripMaster = new ScripMasterBase();

                    //MarketLot
                    var MarketLot = oSQLiteDataReader["MarketLot"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(MarketLot))
                        objScripMaster.MarketLot = Convert.ToInt32(MarketLot);

                    //GroupName
                    objScripMaster.GroupName = oSQLiteDataReader["GroupName"]?.ToString().Trim();

                    //FaceValue in paise
                    var FaceValue = oSQLiteDataReader["FaceValue"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(FaceValue))
                        objScripMaster.FaceValue = Convert.ToInt64(FaceValue);

                    //Filler2_GSM flag
                    var Filler2_GSM = oSQLiteDataReader["Filler2_GSM"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(Filler2_GSM))
                        objScripMaster.Filler2_GSM = Convert.ToInt32(Filler2_GSM);

                    //TickSize
                    var TickSize = oSQLiteDataReader["TickSize"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(TickSize))
                        objScripMaster.TickSize = Convert.ToDouble(TickSize);

                    //ScripCode
                    var ScripCode = oSQLiteDataReader["ScripCode"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(ScripCode))
                        objScripMaster.ScripCode = Convert.ToInt64(ScripCode);

                    //InstrumentType
                    var InstrumentType = oSQLiteDataReader["InstrumentType"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(InstrumentType))
                        objScripMaster.InstrumentType = Convert.ToChar(InstrumentType.Truncate(1));

                    //ScripId
                    objScripMaster.ScripId = oSQLiteDataReader["ScripId"]?.ToString().Trim();

                    //ScripName
                    objScripMaster.ScripName = oSQLiteDataReader["ScripName"]?.ToString().Trim();

                    //IMPercentage from Bcast
                    objScripMaster.IMPercentage = 0.0m;

                    //EMPercentage from Bcast
                    objScripMaster.EMPercentage = 0.0m;

                    objScripMaster.Precision = 2;

                    //BOWID
                    var SCID = oSQLiteDataReader["SCID"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(SCID))
                        objScripMaster.BowTokenID = Convert.ToInt32(SCID);

                    if (!objMastertxtDictBaseNSE.ContainsKey(objScripMaster.ScripCode))
                    {
                        objMastertxtDictBaseNSE.Add(objScripMaster.ScripCode, objScripMaster);
                    }

                }
                objMastertxtDictBaseNSE = objMastertxtDictBaseNSE.OrderBy(x => x.Value.ScripId).ToDictionary(x => x.Key, x => x.Value);
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

        /// <summary>
        /// Reads Derivative masters for BSE
        /// </summary>
        private static void ReadDerivativeMasterBSE()
        {
            try
            {
                oDataAccessLayer.OpenConnection((int)DataAccessLayer.ConnectionDB.Masters);

                string strQuery = @"SELECT BOWID,
       ContractTokenNum,
       AssestTokenNum,
       InstrumentType,
       AssetCd,
       UnderlyingAsset,
       ExpiryDate,
       StrikePrice,
       OptionType,
       Precision,
       PartitionID,
       ProductID,
       CapacityGroupID,
       MinimumLotSize,
       TickSize,
       InstrumentName,
       QuantityMultiplier,
       UnderlyingMarket,
       ContractType,
       ProductCode,
       BasePrice,
       DeleteFlag,
       ScripID,
       StrategyID,
       ComplexInstrumentType
  FROM BSE_DERIVATIVE_CO_CFE ORDER BY ScripID ASC;
";
                SQLiteDataReader oSQLiteDataReader = oDataAccessLayer.ExecuteDataReader((int)ConnectionDB.Masters,strQuery, System.Data.CommandType.Text, null);

                while (oSQLiteDataReader.Read())
                {
                    DerivativeMasterBase objDerivativeMasterBase = new DerivativeMasterBase();

                    //BOWID
                    var BOWID = oSQLiteDataReader["BOWID"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(BOWID))
                        objDerivativeMasterBase.BowId = Convert.ToInt32(BOWID);

                    //ContractTokenNum
                    var ContractTokenNum = oSQLiteDataReader["ContractTokenNum"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(ContractTokenNum))
                        objDerivativeMasterBase.ContractTokenNum = Convert.ToInt64(ContractTokenNum);

                    //AssestTokenNum
                    var AssestTokenNum = oSQLiteDataReader["AssestTokenNum"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(AssestTokenNum))
                        objDerivativeMasterBase.AssestTokenNum = Convert.ToInt64(AssestTokenNum);

                    //InstrumentType
                    var InstrumentType = oSQLiteDataReader["InstrumentType"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(InstrumentType))
                        objDerivativeMasterBase.InstrumentType = InstrumentType;

                    //UnderlyingAsset
                    var UnderlyingAsset = oSQLiteDataReader["UnderlyingAsset"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(UnderlyingAsset))
                        objDerivativeMasterBase.UnderlyingAsset = UnderlyingAsset;

                    //ExpiryDate
                    var ExpiryDate = oSQLiteDataReader["ExpiryDate"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(ExpiryDate))
                    {
                        objDerivativeMasterBase.ExpiryDate = ExpiryDate;
                        //DisplayExpiryDate
                        objDerivativeMasterBase.DisplayExpiryDate = string.Format("{0:dd-MM-yyyy}", Convert.ToDateTime(ExpiryDate).Date);
                    }

                    //StrikePrice
                    var StrikePrice = oSQLiteDataReader["StrikePrice"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(StrikePrice))
                        objDerivativeMasterBase.StrikePrice = Convert.ToInt64(StrikePrice);

                    //OptionType
                    var OptionType = oSQLiteDataReader["OptionType"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(OptionType))
                        objDerivativeMasterBase.OptionType = OptionType;

                    //Precision
                    var Precision = oSQLiteDataReader["Precision"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(Precision))
                        objDerivativeMasterBase.Precision = Convert.ToInt32(Precision);

                    //MarketLot OR MinimumLotSize
                    var MinimumLotSize = oSQLiteDataReader["MinimumLotSize"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(MinimumLotSize))
                        objDerivativeMasterBase.MinimumLotSize = Convert.ToInt32(MinimumLotSize);//MarketLot
                    //QuantityMultiplier
                    var QuantityMultiplier = oSQLiteDataReader["QuantityMultiplier"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(QuantityMultiplier))
                        objDerivativeMasterBase.QuantityMultiplier = Convert.ToInt32(QuantityMultiplier);// QuantityMultiplier

                    //TickSize
                    var TickSize = oSQLiteDataReader["TickSize"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(TickSize))
                        objDerivativeMasterBase.TickSize = Convert.ToInt32(TickSize);

                    //InstrumentName
                    var InstrumentName = oSQLiteDataReader["InstrumentName"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(InstrumentName))
                        objDerivativeMasterBase.InstrumentName = InstrumentName;

                    //ContractType
                    var ContractType = oSQLiteDataReader["ContractType"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(ContractType))
                        objDerivativeMasterBase.ContractType = ContractType;

                    //AssetCode
                    var AssetCd = oSQLiteDataReader["AssetCd"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(AssetCd))
                        objDerivativeMasterBase.AssetCode = AssetCd;

                    //PartitionID
                    var PartitionID = oSQLiteDataReader["PartitionID"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(PartitionID))
                        objDerivativeMasterBase.PartitionID = Convert.ToInt32(PartitionID);

                    //ProductID
                    var ProductID = oSQLiteDataReader["ProductID"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(ProductID))
                        objDerivativeMasterBase.ProductID = Convert.ToInt32(ProductID);

                    //CapacityGroupID
                    var CapacityGroupID = oSQLiteDataReader["CapacityGroupID"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(CapacityGroupID))
                        objDerivativeMasterBase.CapacityGroupID = Convert.ToInt32(CapacityGroupID);

                    //UnderlyingMarket
                    var UnderlyingMarket = oSQLiteDataReader["UnderlyingMarket"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(UnderlyingMarket))
                        objDerivativeMasterBase.UnderlyingMarket = Convert.ToInt32(UnderlyingMarket);

                    //ProductCode
                    var ProductCode = oSQLiteDataReader["ProductCode"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(ProductCode))
                        objDerivativeMasterBase.ProductCode = ProductCode;

                    //BasePrice
                    var BasePrice = oSQLiteDataReader["BasePrice"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(BasePrice))
                        objDerivativeMasterBase.BasePrice = Convert.ToInt32(BasePrice);

                    //DeleteFlag
                    var DeleteFlag = oSQLiteDataReader["DeleteFlag"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(DeleteFlag))
                        objDerivativeMasterBase.DeleteFlag = Convert.ToChar(DeleteFlag);

                    //ScripID or series code
                    var ScripID = oSQLiteDataReader["ScripID"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(ScripID))
                        objDerivativeMasterBase.ScripId = ScripID;

                    //StrategyID
                    var StrategyID = oSQLiteDataReader["StrategyID"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(StrategyID))
                        objDerivativeMasterBase.StrategyID = Convert.ToInt32(StrategyID);

                    //ComplexInstrumentType
                    var ComplexInstrumentType = oSQLiteDataReader["ComplexInstrumentType"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(ComplexInstrumentType))
                        objDerivativeMasterBase.ComplexInstrumentType = Convert.ToInt32(ComplexInstrumentType);

                    //scrip group
                    objDerivativeMasterBase.DerScripGroup = "DF";

                    //Segment
                    objDerivativeMasterBase.Segment = 2;

                    //Precision
                    objDerivativeMasterBase.Precision = 2;

                    if (!objMasterDerivativeDictBaseBSE.ContainsKey(objDerivativeMasterBase.ContractTokenNum))
                    {
                        objMasterDerivativeDictBaseBSE.Add(objDerivativeMasterBase.ContractTokenNum, objDerivativeMasterBase);
                    }
                }
                objMasterDerivativeDictBaseBSE = objMasterDerivativeDictBaseBSE.OrderBy(x => x.Value.ScripId).ToDictionary(x => x.Key, x => x.Value);
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

        /// <summary>
        /// Reads Derivative masters for NSE
        /// </summary>
        private static void ReadDerivativeMasterNSE()
        {
            try
            {
                oDataAccessLayer.OpenConnection((int)DataAccessLayer.ConnectionDB.Masters);

                string strQuery = @"SELECT BOWID, ContractTokenNum, AssestTokenNum, InstrumentType, UnderlyingAsset, ExpiryDate, StrikePrice,
                                    OptionType, Precision, MinimumLotSize, TickSize, InstrumentName, 'NA' as 'ContractType'
                                    FROM NSE_DERIVATIVE_CO_CFE ";

                SQLiteDataReader oSQLiteDataReader = oDataAccessLayer.ExecuteDataReader((int)ConnectionDB.Masters,strQuery, System.Data.CommandType.Text, null);

                while (oSQLiteDataReader.Read())
                {
                    DerivativeMasterBase objDerivativeMasterBase = new DerivativeMasterBase();

                    //BOWID
                    var BOWID = oSQLiteDataReader["BOWID"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(BOWID))
                        objDerivativeMasterBase.BowId = Convert.ToInt32(BOWID);

                    //ContractTokenNum
                    var ContractTokenNum = oSQLiteDataReader["ContractTokenNum"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(ContractTokenNum))
                        objDerivativeMasterBase.ContractTokenNum = Convert.ToInt64(ContractTokenNum);

                    //AssestTokenNum
                    var AssestTokenNum = oSQLiteDataReader["AssestTokenNum"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(AssestTokenNum))
                        objDerivativeMasterBase.AssestTokenNum = Convert.ToInt64(AssestTokenNum);

                    //InstrumentType
                    var InstrumentType = oSQLiteDataReader["InstrumentType"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(InstrumentType))
                        objDerivativeMasterBase.InstrumentType = InstrumentType;

                    //UnderlyingAsset
                    var UnderlyingAsset = oSQLiteDataReader["UnderlyingAsset"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(UnderlyingAsset))
                        objDerivativeMasterBase.UnderlyingAsset = UnderlyingAsset;

                    //ExpiryDate
                    var ExpiryDate = oSQLiteDataReader["ExpiryDate"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(ExpiryDate))
                        objDerivativeMasterBase.ExpiryDate = ExpiryDate;

                    //StrikePrice
                    var StrikePrice = oSQLiteDataReader["StrikePrice"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(StrikePrice))
                        objDerivativeMasterBase.StrikePrice = Convert.ToInt64(StrikePrice);

                    //OptionType
                    var OptionType = oSQLiteDataReader["OptionType"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(OptionType))
                        objDerivativeMasterBase.OptionType = OptionType;

                    //Precision
                    var Precision = oSQLiteDataReader["Precision"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(Precision))
                        objDerivativeMasterBase.Precision = Convert.ToInt32(Precision);

                    //MarketLot OR MinimumLotSize
                    var MinimumLotSize = oSQLiteDataReader["MinimumLotSize"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(MinimumLotSize))
                        objDerivativeMasterBase.MinimumLotSize = Convert.ToInt32(MinimumLotSize);//MarketLot

                    //TickSize
                    var TickSize = oSQLiteDataReader["TickSize"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(TickSize))
                        objDerivativeMasterBase.TickSize = Convert.ToInt32(TickSize);

                    //InstrumentName
                    var InstrumentName = oSQLiteDataReader["InstrumentName"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(InstrumentName))
                        objDerivativeMasterBase.InstrumentName = InstrumentName;

                    //ContractType
                    var ContractType = oSQLiteDataReader["ContractType"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(ContractType))
                        objDerivativeMasterBase.ContractType = ContractType;

                    objDerivativeMasterBase.DerScripGroup = "DF";
                    if (!objMasterDerivativeDictBaseNSE.ContainsKey(objDerivativeMasterBase.ContractTokenNum))
                    {
                        objMasterDerivativeDictBaseNSE.Add(objDerivativeMasterBase.ContractTokenNum, objDerivativeMasterBase);
                    }

                }
                objMasterDerivativeDictBaseNSE = objMasterDerivativeDictBaseNSE.OrderBy(x => x.Value.ScripId).ToDictionary(x => x.Key, x => x.Value);
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


        /// <summary>
        /// Reads Currency masters for BSE
        /// </summary>
        private static void ReadCurrencyMasterBSE()
        {
            try
            {
                oDataAccessLayer.OpenConnection((int)DataAccessLayer.ConnectionDB.Masters);

                //string strQuery = @"SELECT BOWID, ContractTokenNum, AssestTokenNum, InstrumentType, AssetCD, ExpiryDate, StrikePrice, OptionType,
                //                    Precision, MinimumLotSize, TickSize, InstrumentName, ContractType 
                //                    FROM BSE_CURRENCY_CO_CFE";

                string strQuery = @"SELECT BOWID,
       ContractTokenNum,
       AssestTokenNum,
       InstrumentType,
       AssetCD,
       ExpiryDate,
       StrikePrice,
       OptionType,
       Precision,
       PartitionID,
       ProductID,
       CapacityGroupID,
       MinimumLotSize,
       TickSize,
       InstrumentName,
       QuantityMultiplier,
       UnderlyingMarket,
       ContractType,
       BasePrice,
       DeleteFlag,
       ScripID,
       StrategyID,
       ComplexInstrumentType
  FROM BSE_CURRENCY_CO_CFE ORDER BY ScripID ASC;
";
                SQLiteDataReader oSQLiteDataReader = oDataAccessLayer.ExecuteDataReader((int)ConnectionDB.Masters,strQuery, System.Data.CommandType.Text, null);

                while (oSQLiteDataReader.Read())
                {
                    CurrencyMasterBase objCurrencyMasterBase = new CurrencyMasterBase();

                    //BOWID
                    var BOWID = oSQLiteDataReader["BOWID"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(BOWID))
                        objCurrencyMasterBase.BowId = Convert.ToInt32(BOWID);

                    //ContractTokenNum
                    var ContractTokenNum = oSQLiteDataReader["ContractTokenNum"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(ContractTokenNum))
                        objCurrencyMasterBase.ContractTokenNum = Convert.ToInt64(ContractTokenNum);

                    //AssestTokenNum
                    var AssestTokenNum = oSQLiteDataReader["AssestTokenNum"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(AssestTokenNum))
                        objCurrencyMasterBase.AssestTokenNum = Convert.ToInt64(AssestTokenNum);

                    //InstrumentType
                    var InstrumentType = oSQLiteDataReader["InstrumentType"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(InstrumentType))
                        objCurrencyMasterBase.InstrumentType = InstrumentType;

                    //UnderlyingAsset OR AssetCode
                    var AssetCD = oSQLiteDataReader["AssetCD"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(AssetCD))
                        objCurrencyMasterBase.UnderlyingAsset = AssetCD;

                    //ExpiryDate
                    var ExpiryDate = oSQLiteDataReader["ExpiryDate"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(ExpiryDate))
                    {
                        objCurrencyMasterBase.ExpiryDate = ExpiryDate;
                        //DisplayExpiryDate
                        objCurrencyMasterBase.DisplayExpiryDate = string.Format("{0:dd-MM-yyyy}", Convert.ToDateTime(ExpiryDate).Date);
                    }


                    //StrikePrice
                    var StrikePrice = oSQLiteDataReader["StrikePrice"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(StrikePrice))
                        objCurrencyMasterBase.StrikePrice = Convert.ToInt64(StrikePrice);
                    objCurrencyMasterBase.StrikePrice = objCurrencyMasterBase.StrikePrice > 0 ? objCurrencyMasterBase.StrikePrice / 1000 : objCurrencyMasterBase.StrikePrice;

                    //OptionType
                    var OptionType = oSQLiteDataReader["OptionType"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(OptionType))
                        objCurrencyMasterBase.OptionType = OptionType;

                    //Precision
                    var Precision = oSQLiteDataReader["Precision"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(Precision))
                        objCurrencyMasterBase.Precision = Convert.ToInt32(Precision);

                    //MarketLot OR MinimumLotSize
                    var MinimumLotSize = oSQLiteDataReader["MinimumLotSize"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(MinimumLotSize))
                        objCurrencyMasterBase.MinimumLotSize = Convert.ToInt32(MinimumLotSize);

                    //TickSize
                    var TickSize = oSQLiteDataReader["TickSize"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(TickSize))
                        objCurrencyMasterBase.TickSize = Convert.ToInt32(TickSize) / 1000;

                    //InstrumentName
                    var InstrumentName = oSQLiteDataReader["InstrumentName"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(InstrumentName))
                        objCurrencyMasterBase.InstrumentName = InstrumentName;

                    //ContractType
                    var ContractType = oSQLiteDataReader["ContractType"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(ContractType))
                        objCurrencyMasterBase.ContractType = ContractType;

                    //PartitionID
                    var PartitionID = oSQLiteDataReader["PartitionID"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(PartitionID))
                        objCurrencyMasterBase.PartitionID = Convert.ToInt32(PartitionID);

                    //ProductID
                    var ProductID = oSQLiteDataReader["ProductID"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(ProductID))
                        objCurrencyMasterBase.ProductID = Convert.ToInt32(ProductID);

                    //CapacityGroupID
                    var CapacityGroupID = oSQLiteDataReader["CapacityGroupID"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(CapacityGroupID))
                        objCurrencyMasterBase.CapacityGroupID = Convert.ToInt32(CapacityGroupID);

                    //QuantityMultiplier
                    var QuantityMultiplier = oSQLiteDataReader["QuantityMultiplier"]?.ToString().Trim();//MarketLot
                    if (!string.IsNullOrEmpty(QuantityMultiplier))
                        objCurrencyMasterBase.QuantityMultiplier = Convert.ToInt64(QuantityMultiplier);

                    //UnderlyingMarket
                    var UnderlyingMarket = oSQLiteDataReader["UnderlyingMarket"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(UnderlyingMarket))
                        objCurrencyMasterBase.UnderlyingMarket = Convert.ToInt32(UnderlyingMarket);

                    //BasePrice
                    var BasePrice = oSQLiteDataReader["BasePrice"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(BasePrice))
                        objCurrencyMasterBase.BasePrice = Convert.ToInt32(BasePrice);

                    //DeleteFlag
                    var DeleteFlag = oSQLiteDataReader["DeleteFlag"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(DeleteFlag))
                        objCurrencyMasterBase.DeleteFlag = Convert.ToChar(DeleteFlag);

                    //ScripID
                    var ScripID = oSQLiteDataReader["ScripID"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(ScripID))
                        objCurrencyMasterBase.ScripId = ScripID;

                    //StrategyID
                    var StrategyID = oSQLiteDataReader["StrategyID"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(StrategyID))
                        objCurrencyMasterBase.StrategyID = Convert.ToInt32(StrategyID);

                    //ComplexInstrumentType
                    var ComplexInstrumentType = oSQLiteDataReader["ComplexInstrumentType"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(ComplexInstrumentType))
                        objCurrencyMasterBase.ComplexInstrumentType = Convert.ToInt32(ComplexInstrumentType);

                    //Scrip Group
                    objCurrencyMasterBase.CurrScripGroup = "CD";

                    //Segment
                    objCurrencyMasterBase.Segment = 3;

                    //Precision
                    objCurrencyMasterBase.Precision = 4;

                    if (!objMasterCurrencyDictBaseBSE.ContainsKey(objCurrencyMasterBase.ContractTokenNum))
                    {
                        objMasterCurrencyDictBaseBSE.Add(objCurrencyMasterBase.ContractTokenNum, objCurrencyMasterBase);
                    }
                }
                objMasterCurrencyDictBaseBSE = objMasterCurrencyDictBaseBSE.OrderBy(x => x.Value.ScripId).ToDictionary(x => x.Key, x => x.Value);
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

        /// <summary>
        /// Reads Currency masters for NSE
        /// </summary>
        private static void ReadCurrencyMasterNSE()
        {
            try
            {
                oDataAccessLayer.OpenConnection((int)DataAccessLayer.ConnectionDB.Masters);

                string strQuery = @"SELECT BOWID, ContractTokenNum, AssestTokenNum, InstrumentType, UnderlyingAsset, ExpiryDate,
                                    StrikePrice, OptionType, Precision, MinimumLotSize, TickSize, InstrumentName, QuantityMultiplier
                                    FROM NSE_CURRENCY_CO_CFE;";

                SQLiteDataReader oSQLiteDataReader = oDataAccessLayer.ExecuteDataReader((int)ConnectionDB.Masters,strQuery, System.Data.CommandType.Text, null);

                while (oSQLiteDataReader.Read())
                {
                    CurrencyMasterBase objCurrencyMasterBase = new CurrencyMasterBase();

                    //BOWID
                    var BOWID = oSQLiteDataReader["BOWID"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(BOWID))
                        objCurrencyMasterBase.BowId = Convert.ToInt32(BOWID);

                    //ContractTokenNum
                    var ContractTokenNum = oSQLiteDataReader["ContractTokenNum"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(ContractTokenNum))
                        objCurrencyMasterBase.ContractTokenNum = Convert.ToInt64(ContractTokenNum);

                    //AssestTokenNum
                    var AssestTokenNum = oSQLiteDataReader["AssestTokenNum"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(AssestTokenNum))
                        objCurrencyMasterBase.AssestTokenNum = Convert.ToInt64(AssestTokenNum);

                    //InstrumentType
                    var InstrumentType = oSQLiteDataReader["InstrumentType"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(InstrumentType))
                        objCurrencyMasterBase.InstrumentType = InstrumentType;

                    //UnderlyingAsset OR AssetCode
                    var UnderlyingAsset = oSQLiteDataReader["UnderlyingAsset"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(UnderlyingAsset))
                        objCurrencyMasterBase.UnderlyingAsset = UnderlyingAsset;

                    //ExpiryDate
                    var ExpiryDate = oSQLiteDataReader["ExpiryDate"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(ExpiryDate))
                        objCurrencyMasterBase.ExpiryDate = ExpiryDate;

                    //StrikePrice
                    var StrikePrice = oSQLiteDataReader["StrikePrice"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(StrikePrice))
                        objCurrencyMasterBase.StrikePrice = Convert.ToInt64(StrikePrice);

                    //OptionType
                    var OptionType = oSQLiteDataReader["OptionType"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(OptionType))
                        objCurrencyMasterBase.OptionType = OptionType;

                    //Precision
                    var Precision = oSQLiteDataReader["Precision"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(Precision))
                        objCurrencyMasterBase.Precision = Convert.ToInt32(Precision);

                    //MarketLot OR MinimumLotSize
                    var MinimumLotSize = oSQLiteDataReader["MinimumLotSize"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(MinimumLotSize))
                        objCurrencyMasterBase.MinimumLotSize = Convert.ToInt32(MinimumLotSize);//MarketLot

                    //TickSize
                    var TickSize = oSQLiteDataReader["TickSize"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(TickSize))
                        objCurrencyMasterBase.TickSize = Convert.ToInt32(TickSize);

                    //InstrumentName
                    var InstrumentName = oSQLiteDataReader["InstrumentName"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(InstrumentName))
                        objCurrencyMasterBase.InstrumentName = InstrumentName;

                    //QuantityMultiplier
                    var QuantityMultiplier = oSQLiteDataReader["QuantityMultiplier"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(QuantityMultiplier))
                        objCurrencyMasterBase.QuantityMultiplier = Convert.ToInt64(QuantityMultiplier);

                    if (!objMasterCurrencyDictBaseNSE.ContainsKey(objCurrencyMasterBase.ContractTokenNum))
                    {
                        objMasterCurrencyDictBaseNSE.Add(objCurrencyMasterBase.ContractTokenNum, objCurrencyMasterBase);
                    }
                }
                objMasterCurrencyDictBaseNSE = objMasterCurrencyDictBaseNSE.OrderBy(x => x.Value.ScripId).ToDictionary(x => x.Key, x => x.Value);
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

        /// <summary>
        /// Reads Wholesale Debt Market
        /// </summary>
        private static void ReadWholeSaleDebtMarket()
        {
            try
            {
                oDataAccessLayer.OpenConnection((int)DataAccessLayer.ConnectionDB.Masters);

                string strQuery = @"SELECT WDMID, WDMScriptCode, WDMSegment, WDMSettlementType, WDMListingCatagory, WDMInstrumentStandard, WDMBondCategory,
                                    WDMGSecType, WDMISIN, WDMSymbol, WDMInstrumentName, WDMLongExpiryDate, WDMDisplayExpiryDate, WDMStrikePrice, WDMOptionType, WDMMarketLot, WDMFaceValue, WDMIssueDate,
                                    WDMCoupon, WDMIPPeriod, WDMIPDuration, WDMPrevIPDate, WDMNextIPDate, WDMAccruedIntrest, WDMLastTradingDate, WDMExpiryDate, WDMTickSize, WDMActiveSuspendFlag,
                                    WDMShutPeriodStartDate, WDMShutPeriodEndDate, WDMRecordDate, WDMFirstCallDate, WDMFirstPutDate, WDMCrisilRating, WDMCareRating, WDMIcraRating, WDMFitchRating
                                    FROM BSE_DEBTMARKET_CFE ORDER BY WDMSymbol ASC";

                SQLiteDataReader oSQLiteDataReader = oDataAccessLayer.ExecuteDataReader((int)ConnectionDB.Masters,strQuery, System.Data.CommandType.Text, null);

                while (oSQLiteDataReader.Read())
                {
                    WholesaleDebtMarket objWholesaleDebtMarket = new WholesaleDebtMarket();

                    //WDMID 
                    var WDMID = oSQLiteDataReader["WDMID"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(WDMID))
                        objWholesaleDebtMarket.WDMID = Convert.ToInt64(WDMID);
                    //WDMScriptCode 
                    var WDMScriptCode = oSQLiteDataReader["WDMScriptCode"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(WDMScriptCode))
                        objWholesaleDebtMarket.WDMScriptCode = Convert.ToInt64(WDMScriptCode);
                    //WDMSegment 
                    var WDMSegment = oSQLiteDataReader["WDMSegment"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(WDMSegment))
                        objWholesaleDebtMarket.WDMSegment = WDMSegment;
                    //WDMSettlementType 
                    var WDMSettlementType = oSQLiteDataReader["WDMSettlementType"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(WDMSettlementType))
                        objWholesaleDebtMarket.WDMSettlementType = Convert.ToInt64(WDMSettlementType);
                    //WDMListingCatagory 
                    var WDMListingCatagory = oSQLiteDataReader["WDMListingCatagory"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(WDMListingCatagory))
                        objWholesaleDebtMarket.WDMListingCatagory = Convert.ToInt64(WDMListingCatagory);
                    //WDMInstrumentStandard 
                    var WDMInstrumentStandard = oSQLiteDataReader["WDMInstrumentStandard"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(WDMInstrumentStandard))
                        objWholesaleDebtMarket.WDMInstrumentStandard = WDMInstrumentStandard;
                    //WDMBondCategory 
                    var WDMBondCategory = oSQLiteDataReader["WDMBondCategory"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(WDMBondCategory))
                        objWholesaleDebtMarket.WDMBondCategory = WDMBondCategory;
                    //WDMGSecType 
                    var WDMGSecType = oSQLiteDataReader["WDMGSecType"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(WDMGSecType))
                        objWholesaleDebtMarket.WDMGSecType = WDMGSecType;
                    //WDMISIN 
                    var WDMISIN = oSQLiteDataReader["WDMISIN"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(WDMISIN))
                        objWholesaleDebtMarket.WDMISIN = WDMISIN;
                    //WDMSymbol 
                    var WDMSymbol = oSQLiteDataReader["WDMSymbol"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(WDMSymbol))
                        objWholesaleDebtMarket.WDMSymbol = WDMSymbol;
                    //WDMInstrumentName 
                    var WDMInstrumentName = oSQLiteDataReader["WDMInstrumentName"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(WDMInstrumentName))
                        objWholesaleDebtMarket.WDMInstrumentName = WDMInstrumentName;
                    //WDMLongExpiryDate 
                    var WDMLongExpiryDate = oSQLiteDataReader["WDMLongExpiryDate"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(WDMLongExpiryDate))
                        objWholesaleDebtMarket.WDMLongExpiryDate = Convert.ToInt64(WDMLongExpiryDate);
                    //WDMDisplayExpiryDate 
                    var WDMDisplayExpiryDate = oSQLiteDataReader["WDMDisplayExpiryDate"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(WDMDisplayExpiryDate))
                        objWholesaleDebtMarket.WDMDisplayExpiryDate = WDMDisplayExpiryDate;
                    //WDMStrikePrice 
                    var WDMStrikePrice = oSQLiteDataReader["WDMStrikePrice"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(WDMStrikePrice))
                        objWholesaleDebtMarket.WDMStrikePrice = Convert.ToInt32(WDMStrikePrice);
                    //WDMOptionType 
                    var WDMOptionType = oSQLiteDataReader["WDMOptionType"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(WDMOptionType))
                        objWholesaleDebtMarket.WDMOptionType = WDMOptionType;
                    //WDMMarketLot 
                    var WDMMarketLot = oSQLiteDataReader["WDMMarketLot"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(WDMMarketLot))
                        objWholesaleDebtMarket.WDMMarketLot = Convert.ToInt32(WDMMarketLot);
                    //WDMFaceValue 
                    var WDMFaceValue = oSQLiteDataReader["WDMFaceValue"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(WDMFaceValue))
                        objWholesaleDebtMarket.WDMFaceValue = Convert.ToDecimal(WDMFaceValue);
                    //WDMIssueDate 
                    var WDMIssueDate = oSQLiteDataReader["WDMIssueDate"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(WDMIssueDate))
                        objWholesaleDebtMarket.WDMIssueDate = WDMIssueDate;
                    //WDMCoupon 
                    var WDMCoupon = oSQLiteDataReader["WDMCoupon"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(WDMCoupon))
                        objWholesaleDebtMarket.WDMCoupon = Convert.ToDecimal(WDMCoupon);
                    //WDMIPPeriod 
                    var WDMIPPeriod = oSQLiteDataReader["WDMIPPeriod"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(WDMIPPeriod))
                        objWholesaleDebtMarket.WDMIPPeriod = WDMIPPeriod;
                    //WDMIPDuration 
                    var WDMIPDuration = oSQLiteDataReader["WDMIPDuration"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(WDMIPDuration))
                        objWholesaleDebtMarket.WDMIPDuration = WDMIPDuration;
                    //WDMPrevIPDate 
                    var WDMPrevIPDate = oSQLiteDataReader["WDMPrevIPDate"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(WDMPrevIPDate))
                        objWholesaleDebtMarket.WDMPrevIPDate = WDMPrevIPDate;
                    //WDMNextIPDate 
                    var WDMNextIPDate = oSQLiteDataReader["WDMNextIPDate"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(WDMNextIPDate))
                        objWholesaleDebtMarket.WDMNextIPDate = WDMNextIPDate;
                    //WDMAccruedIntrest 
                    var WDMAccruedIntrest = oSQLiteDataReader["WDMAccruedIntrest"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(WDMAccruedIntrest))
                        objWholesaleDebtMarket.WDMAccruedIntrest = Convert.ToDecimal(WDMAccruedIntrest);
                    //WDMLastTradingDate 
                    var WDMLastTradingDate = oSQLiteDataReader["WDMLastTradingDate"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(WDMLastTradingDate))
                        objWholesaleDebtMarket.WDMLastTradingDate = WDMLastTradingDate;
                    //WDMExpiryDate 
                    var WDMExpiryDate = oSQLiteDataReader["WDMExpiryDate"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(WDMExpiryDate))
                        objWholesaleDebtMarket.WDMExpiryDate = WDMExpiryDate;
                    //WDMTickSize 
                    var WDMTickSize = oSQLiteDataReader["WDMTickSize"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(WDMTickSize))
                        objWholesaleDebtMarket.WDMTickSize = Convert.ToDecimal(WDMTickSize);
                    //WDMActiveSuspendFlag 
                    var WDMActiveSuspendFlag = oSQLiteDataReader["WDMActiveSuspendFlag"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(WDMActiveSuspendFlag))
                        objWholesaleDebtMarket.WDMActiveSuspendFlag = WDMActiveSuspendFlag;
                    //WDMShutPeriodStartDate 
                    var WDMShutPeriodStartDate = oSQLiteDataReader["WDMShutPeriodStartDate"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(WDMShutPeriodStartDate))
                        objWholesaleDebtMarket.WDMShutPeriodStartDate = WDMShutPeriodStartDate;
                    //WDMShutPeriodEndDate 
                    var WDMShutPeriodEndDate = oSQLiteDataReader["WDMShutPeriodEndDate"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(WDMShutPeriodEndDate))
                        objWholesaleDebtMarket.WDMShutPeriodEndDate = WDMShutPeriodEndDate;
                    //WDMRecordDate 
                    var WDMRecordDate = oSQLiteDataReader["WDMRecordDate"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(WDMRecordDate))
                        objWholesaleDebtMarket.WDMRecordDate = WDMRecordDate;
                    //WDMFirstCallDate 
                    var WDMFirstCallDate = oSQLiteDataReader["WDMFirstCallDate"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(WDMFirstCallDate))
                        objWholesaleDebtMarket.WDMFirstCallDate = WDMFirstCallDate;
                    //WDMFirstPutDate 
                    var WDMFirstPutDate = oSQLiteDataReader["WDMFirstPutDate"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(WDMFirstPutDate))
                        objWholesaleDebtMarket.WDMFirstPutDate = WDMFirstPutDate;
                    //WDMCrisilRating 
                    var WDMCrisilRating = oSQLiteDataReader["WDMCrisilRating"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(WDMCrisilRating))
                        objWholesaleDebtMarket.WDMCrisilRating = WDMCrisilRating;
                    //WDMCareRating 
                    var WDMCareRating = oSQLiteDataReader["WDMCareRating"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(WDMCareRating))
                        objWholesaleDebtMarket.WDMCareRating = WDMCareRating;
                    //WDMIcraRating 
                    var WDMIcraRating = oSQLiteDataReader["WDMIcraRating"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(WDMIcraRating))
                        objWholesaleDebtMarket.WDMIcraRating = WDMIcraRating;
                    //WDMFitchRating 
                    var WDMFitchRating = oSQLiteDataReader["WDMFitchRating"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(WDMFitchRating))
                        objWholesaleDebtMarket.WDMFitchRating = WDMFitchRating;

                    if (!objMasterWholesaleDebtMarket.ContainsKey(objWholesaleDebtMarket.WDMScriptCode))
                    {
                        objMasterWholesaleDebtMarket.Add(objWholesaleDebtMarket.WDMScriptCode, objWholesaleDebtMarket);
                    }
                }
                objMasterWholesaleDebtMarket = objMasterWholesaleDebtMarket.OrderBy(x => x.Value.WDMSymbol).ToDictionary(x => x.Key, x => x.Value);
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

        /// <summary>
        /// Read SLB master
        /// </summary>
        private static void ReadSLBMaster()
        {
            try
            {


                oDataAccessLayer.OpenConnection((int)DataAccessLayer.ConnectionDB.Masters);

                string strQuery = @"SELECT A.SCID,SCScripCode,SCScripId,SCISIN,SCProductId,B.ScripId as ScripID_ShortName,SCProductCode,SCScripStatus,SCMarketLot,SCNoOfDays,
                                    SCStartDate,SCLastTrade_Date,SCExpiryDate,SCContractMonth,SCRowstate,SCInstrumentName,SCLongExpiryDate,SCDisplayExpiryDate,SCStrikePrice,
                                    SCOptionType,SCTickSize,SCRollOver,SCFiller1 FROM BSE_SLB_CFE A inner join BSE_SECURITIES_CFE B on A.SCProductId == B.ScripCode ";

                SQLiteDataReader oSQLiteDataReader = oDataAccessLayer.ExecuteDataReader((int)ConnectionDB.Masters,strQuery, System.Data.CommandType.Text, null);

                while (oSQLiteDataReader.Read())
                {
                    SLBMaster objSLBMaster = new SLBMaster();

                    //SCID 
                    var SCID = oSQLiteDataReader["SCID"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(SCID))
                        objSLBMaster.SCID = Convert.ToInt32(SCID);
                    //SCScripCode 
                    var SCScripCode = oSQLiteDataReader["SCScripCode"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(SCScripCode))
                        objSLBMaster.SCScripCode = Convert.ToInt32(SCScripCode);
                    //SCScripId 
                    var SCScripId = oSQLiteDataReader["SCScripId"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(SCScripId))
                        objSLBMaster.SCScripId = SCScripId;
                    //ScripID_ShortName
                    var ScripID_ShortName = oSQLiteDataReader["ScripID_ShortName"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(ScripID_ShortName))
                        objSLBMaster.ScripID_ShortName = ScripID_ShortName;
                    //SCISIN 
                    var SCISIN = oSQLiteDataReader["SCISIN"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(SCISIN))
                        objSLBMaster.SCISIN = SCISIN;
                    //SCProductId 
                    var SCProductId = oSQLiteDataReader["SCProductId"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(SCProductId))
                        objSLBMaster.SCProductId = Convert.ToInt32(SCProductId);
                    //SCProductCode 
                    var SCProductCode = oSQLiteDataReader["SCProductCode"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(SCProductCode))
                        objSLBMaster.SCProductCode = SCProductCode;
                    //SCScripStatus 
                    var SCScripStatus = oSQLiteDataReader["SCScripStatus"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(SCScripStatus))
                        objSLBMaster.SCScripStatus = SCScripStatus;
                    //SCMarketLot 
                    var SCMarketLot = oSQLiteDataReader["SCMarketLot"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(SCMarketLot))
                        objSLBMaster.SCMarketLot = Convert.ToInt32(SCMarketLot);
                    //SCNoOfDays 
                    var SCNoOfDays = oSQLiteDataReader["SCNoOfDays"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(SCNoOfDays))
                        objSLBMaster.SCNoOfDays = Convert.ToInt32(SCNoOfDays);
                    //SCStartDate 
                    var SCStartDate = oSQLiteDataReader["SCStartDate"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(SCStartDate))
                        objSLBMaster.SCStartDate = SCStartDate;
                    //SCLastTrade_Date 
                    var SCLastTrade_Date = oSQLiteDataReader["SCLastTrade_Date"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(SCLastTrade_Date))
                        objSLBMaster.SCLastTrade_Date = SCLastTrade_Date;
                    //SCExpiryDate 
                    var SCExpiryDate = oSQLiteDataReader["SCExpiryDate"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(SCExpiryDate))
                        objSLBMaster.SCExpiryDate = SCExpiryDate;
                    //SCContractMonth 
                    var SCContractMonth = oSQLiteDataReader["SCContractMonth"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(SCContractMonth))
                        objSLBMaster.SCContractMonth = SCContractMonth;
                    //SCRowstate 
                    var SCRowstate = oSQLiteDataReader["SCRowstate"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(SCRowstate))
                        objSLBMaster.SCRowstate = Convert.ToInt32(SCRowstate);
                    //SCInstrumentName 
                    var SCInstrumentName = oSQLiteDataReader["SCInstrumentName"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(SCInstrumentName))
                        objSLBMaster.SCInstrumentName = SCInstrumentName;
                    //SCLongExpiryDate 
                    var SCLongExpiryDate = oSQLiteDataReader["SCLongExpiryDate"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(SCLongExpiryDate))
                        objSLBMaster.SCLongExpiryDate = Convert.ToInt64(SCLongExpiryDate);
                    //SCDisplayExpiryDate 
                    var SCDisplayExpiryDate = oSQLiteDataReader["SCDisplayExpiryDate"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(SCDisplayExpiryDate))
                        objSLBMaster.SCDisplayExpiryDate = SCDisplayExpiryDate;
                    //SCStrikePrice 
                    var SCStrikePrice = oSQLiteDataReader["SCStrikePrice"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(SCStrikePrice))
                        objSLBMaster.SCStrikePrice = Convert.ToInt32(SCStrikePrice);
                    //SCOptionType 
                    var SCOptionType = oSQLiteDataReader["SCOptionType"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(SCOptionType))
                        objSLBMaster.SCOptionType = SCOptionType;
                    //SCTickSize 
                    var SCTickSize = oSQLiteDataReader["SCTickSize"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(SCTickSize))
                        objSLBMaster.SCTickSize = Convert.ToInt32(SCTickSize);
                    //SCRollOver 
                    var SCRollOver = oSQLiteDataReader["SCRollOver"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(SCRollOver))
                        objSLBMaster.SCRollOver = SCRollOver;
                    //SCFiller1 
                    var SCFiller1 = oSQLiteDataReader["SCFiller1"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(SCFiller1))
                        objSLBMaster.SCFiller1 = SCFiller1;



                    if (!objSLBMasterDict.ContainsKey(objSLBMaster.SCScripCode))
                    {
                        objSLBMasterDict.Add(objSLBMaster.SCScripCode, objSLBMaster);
                    }
                }
                objSLBMasterDict = objSLBMasterDict.OrderBy(x => x.Value.SCInstrumentName).ToDictionary(x => x.Key, x => x.Value);
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                oDataAccessLayer.CloseConnection((int)DataAccessLayer.ConnectionDB.Masters);
            }
        }

        /// <summary>
        /// Reads ITP master
        /// </summary>
        private static void ReadITPMaster()
        {
            try
            {
                oDataAccessLayer.OpenConnection((int)DataAccessLayer.ConnectionDB.Masters);

                string strQuery = @"SELECT ITPID,       ITPScripCode,       ITPScripID,       ITPScripName,       ITPScripISIN,       ITPGroupName,
                                ITPCompanyCode,       ITPInstrumenName,       ITPExpiryDate,       ITPDisplayExpiryDate,       ITPStrikePrice,       ITPOptionType,
                                ITPMarketLot,       ITPTickSize,       ITPSequenceID  
                                FROM BSE_ITP_CFE";

                SQLiteDataReader oSQLiteDataReader = oDataAccessLayer.ExecuteDataReader((int)ConnectionDB.Masters,strQuery, System.Data.CommandType.Text, null);

                while (oSQLiteDataReader.Read())
                {
                    ITPMaster objITPMaster = new ITPMaster();

                    //ITPID 
                    var ITPID = oSQLiteDataReader["ITPID"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(ITPID))
                        objITPMaster.ITPID = Convert.ToInt64(ITPID);
                    //ITPScripCode 
                    var ITPScripCode = oSQLiteDataReader["ITPScripCode"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(ITPScripCode))
                        objITPMaster.ITPScripCode = Convert.ToInt32(ITPScripCode);
                    //ITPScripID 
                    var ITPScripID = oSQLiteDataReader["ITPScripID"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(ITPScripID))
                        objITPMaster.ITPScripID = ITPScripID;
                    //ITPScripName 
                    var ITPScripName = oSQLiteDataReader["ITPScripName"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(ITPScripName))
                        objITPMaster.ITPScripName = ITPScripName;
                    //ITPScripISIN 
                    var ITPScripISIN = oSQLiteDataReader["ITPScripISIN"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(ITPScripISIN))
                        objITPMaster.ITPScripISIN = ITPScripISIN;
                    //ITPGroupName 
                    var ITPGroupName = oSQLiteDataReader["ITPGroupName"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(ITPGroupName))
                        objITPMaster.ITPGroupName = ITPGroupName;
                    //ITPCompanyCode 
                    var ITPCompanyCode = oSQLiteDataReader["ITPCompanyCode"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(ITPCompanyCode))
                        objITPMaster.ITPCompanyCode = Convert.ToInt64(ITPCompanyCode);
                    //ITPInstrumenName 
                    var ITPInstrumenName = oSQLiteDataReader["ITPInstrumenName"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(ITPInstrumenName))
                        objITPMaster.ITPInstrumenName = ITPInstrumenName;
                    //ITPExpiryDate 
                    var ITPExpiryDate = oSQLiteDataReader["ITPExpiryDate"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(ITPExpiryDate))
                        objITPMaster.ITPExpiryDate = Convert.ToInt64(ITPExpiryDate);
                    //ITPDisplayExpiryDate 
                    var ITPDisplayExpiryDate = oSQLiteDataReader["ITPDisplayExpiryDate"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(ITPDisplayExpiryDate))
                        objITPMaster.ITPDisplayExpiryDate = ITPDisplayExpiryDate;
                    //ITPStrikePrice 
                    var ITPStrikePrice = oSQLiteDataReader["ITPStrikePrice"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(ITPStrikePrice))
                        objITPMaster.ITPStrikePrice = Convert.ToInt32(ITPStrikePrice);
                    //ITPOptionType 
                    var ITPOptionType = oSQLiteDataReader["ITPOptionType"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(ITPOptionType))
                        objITPMaster.ITPOptionType = ITPOptionType;
                    //ITPMarketLot 
                    var ITPMarketLot = oSQLiteDataReader["ITPMarketLot"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(ITPMarketLot))
                        objITPMaster.ITPMarketLot = Convert.ToInt32(ITPMarketLot);
                    //ITPTickSize 
                    var ITPTickSize = oSQLiteDataReader["ITPTickSize"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(ITPTickSize))
                        objITPMaster.ITPTickSize = Convert.ToInt32(ITPTickSize);
                    //ITPSequenceID 
                    var ITPSequenceID = oSQLiteDataReader["ITPSequenceID"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(ITPSequenceID))
                        objITPMaster.ITPSequenceID = Convert.ToInt32(ITPSequenceID);


                    if (!objITPMasterDict.ContainsKey(objITPMaster.ITPScripCode))
                    {
                        objITPMasterDict.Add(objITPMaster.ITPScripCode, objITPMaster);
                    }
                }
                objITPMasterDict = objITPMasterDict.OrderBy(x => x.Value.ITPInstrumenName).ToDictionary(x => x.Key, x => x.Value);
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

        /// <summary>
        /// Reads MCX master
        /// </summary>
        private static void ReadMCXMaster()
        {
            try
            {
                oDataAccessLayer.OpenConnection((int)DataAccessLayer.ConnectionDB.Masters);

                string strQuery = @"SELECT MCID,       MCSequenceId,       MCInstrumentName,       MCToken,       MCUnderlyingAsset,       MCContractCode,
       MCStrikePrice,       MCOptionType,       MCExpiryDate,       MCDisplayExpiryDate,       MCContractDescription,       MCQuotationUnit,
       MCQuotationMetric,       MCBoardLot,       MCPriceTick,       MCPQFactor,       MCTradingUnit,       MCGeneralNumerator,       MCGeneralDenominator,
       MCField3  FROM MCX_COMMODITY_CFE";

                SQLiteDataReader oSQLiteDataReader = oDataAccessLayer.ExecuteDataReader((int)ConnectionDB.Masters,strQuery, System.Data.CommandType.Text, null);

                while (oSQLiteDataReader.Read())
                {
                    MCXMaster objMCXMaster = new MCXMaster();

                    //MCID 
                    var MCID = oSQLiteDataReader["MCID"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(MCID))
                        objMCXMaster.MCID = Convert.ToInt64(MCID);
                    //MCSequenceId 
                    var MCSequenceId = oSQLiteDataReader["MCSequenceId"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(MCSequenceId))
                        objMCXMaster.MCSequenceId = Convert.ToInt64(MCSequenceId);
                    //MCInstrumentName 
                    var MCInstrumentName = oSQLiteDataReader["MCInstrumentName"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(MCInstrumentName))
                        objMCXMaster.MCInstrumentName = MCInstrumentName;
                    //MCToken 
                    var MCToken = oSQLiteDataReader["MCToken"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(MCToken))
                        objMCXMaster.MCToken = Convert.ToInt64(MCToken);
                    //MCUnderlyingAsset 
                    var MCUnderlyingAsset = oSQLiteDataReader["MCUnderlyingAsset"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(MCUnderlyingAsset))
                        objMCXMaster.MCUnderlyingAsset = Convert.ToInt64(MCUnderlyingAsset);
                    //MCContractCode 
                    var MCContractCode = oSQLiteDataReader["MCContractCode"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(MCContractCode))
                        objMCXMaster.MCContractCode = MCContractCode;
                    //MCStrikePrice 
                    var MCStrikePrice = oSQLiteDataReader["MCStrikePrice"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(MCStrikePrice))
                        objMCXMaster.MCStrikePrice = Convert.ToInt64(MCStrikePrice);
                    //MCOptionType 
                    var MCOptionType = oSQLiteDataReader["MCOptionType"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(MCOptionType))
                        objMCXMaster.MCOptionType = MCOptionType;
                    //MCExpiryDate 
                    var MCExpiryDate = oSQLiteDataReader["MCExpiryDate"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(MCExpiryDate))
                        objMCXMaster.MCExpiryDate = Convert.ToInt64(MCExpiryDate);
                    //MCDisplayExpiryDate 
                    var MCDisplayExpiryDate = oSQLiteDataReader["MCDisplayExpiryDate"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(MCDisplayExpiryDate))
                        objMCXMaster.MCDisplayExpiryDate = MCDisplayExpiryDate;
                    //MCContractDescription 
                    var MCContractDescription = oSQLiteDataReader["MCContractDescription"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(MCContractDescription))
                        objMCXMaster.MCContractDescription = MCContractDescription;
                    //MCQuotationUnit 
                    var MCQuotationUnit = oSQLiteDataReader["MCQuotationUnit"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(MCQuotationUnit))
                        objMCXMaster.MCQuotationUnit = Convert.ToInt64(MCQuotationUnit);
                    //MCQuotationMetric 
                    var MCQuotationMetric = oSQLiteDataReader["MCQuotationMetric"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(MCQuotationMetric))
                        objMCXMaster.MCQuotationMetric = MCQuotationMetric;
                    //MCBoardLot 
                    var MCBoardLot = oSQLiteDataReader["MCBoardLot"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(MCBoardLot))
                        objMCXMaster.MCBoardLot = Convert.ToInt64(MCBoardLot);
                    //MCPriceTick 
                    var MCPriceTick = oSQLiteDataReader["MCPriceTick"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(MCPriceTick))
                        objMCXMaster.MCPriceTick = Convert.ToInt64(MCPriceTick);
                    //MCPQFactor 
                    var MCPQFactor = oSQLiteDataReader["MCPQFactor"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(MCPQFactor))
                        objMCXMaster.MCPQFactor = MCPQFactor;
                    //MCTradingUnit 
                    var MCTradingUnit = oSQLiteDataReader["MCTradingUnit"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(MCTradingUnit))
                        objMCXMaster.MCTradingUnit = MCTradingUnit;
                    //MCGeneralNumerator 
                    var MCGeneralNumerator = oSQLiteDataReader["MCGeneralNumerator"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(MCGeneralNumerator))
                        objMCXMaster.MCGeneralNumerator = Convert.ToDecimal(MCGeneralNumerator);
                    //MCGeneralDenominator 
                    var MCGeneralDenominator = oSQLiteDataReader["MCGeneralDenominator"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(MCGeneralDenominator))
                        objMCXMaster.MCGeneralDenominator = Convert.ToDecimal(MCGeneralDenominator);
                    //MCField3 
                    var MCField3 = oSQLiteDataReader["MCField3"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(MCField3))
                        objMCXMaster.MCField3 = MCField3;


                    if (!objMCXMasterDict.ContainsKey(objMCXMaster.MCToken))
                    {
                        objMCXMasterDict.Add(objMCXMaster.MCToken, objMCXMaster);
                    }
                }
                objMCXMasterDict = objMCXMasterDict.OrderBy(x => x.Value.MCToken).ToDictionary(x => x.Key, x => x.Value);
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

        /// <summary>
        /// Read NCDEX Master
        /// </summary>
        private static void ReadNCDEXMaster()
        {
            try
            {
                oDataAccessLayer.OpenConnection((int)DataAccessLayer.ConnectionDB.Masters);

                string strQuery = @"SELECT NCDID,       NCDToken,       NCDInstrumentName,       NCDSymbol,       NCDExpiryDate,       NCDDisplayExpiryDate,
       NCDStrikePrice,       NCDOptionType,       NCDBoardLotQuantity,       NCDTickSize,       NCDName,       NCDExerciseStartDate,       NCDExerciseEndDate,       NCDPriceNumerator,
       NCDPriceDenominator,       NCDPriceUnit,       NCDQuantityUnit,       NCDDeliveryUnit,       NCDDeliveryLotQuantity,       NCDSequenceId,       NCDTokenString
  FROM NCDEX_COMMODITY_CFE";

                SQLiteDataReader oSQLiteDataReader = oDataAccessLayer.ExecuteDataReader((int)ConnectionDB.Masters,strQuery, System.Data.CommandType.Text, null);

                while (oSQLiteDataReader.Read())
                {
                    NCDEXMaster objNCDEXMaster = new NCDEXMaster();

                    //NCDID 
                    var NCDID = oSQLiteDataReader["NCDID"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(NCDID))
                        objNCDEXMaster.NCDID = Convert.ToInt64(NCDID);
                    //NCDToken 
                    var NCDToken = oSQLiteDataReader["NCDToken"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(NCDToken))
                        objNCDEXMaster.NCDToken = Convert.ToInt64(NCDToken);
                    //NCDInstrumentName 
                    var NCDInstrumentName = oSQLiteDataReader["NCDInstrumentName"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(NCDInstrumentName))
                        objNCDEXMaster.NCDInstrumentName = NCDInstrumentName;
                    //NCDSymbol 
                    var NCDSymbol = oSQLiteDataReader["NCDSymbol"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(NCDSymbol))
                        objNCDEXMaster.NCDSymbol = NCDSymbol;
                    //NCDExpiryDate 
                    var NCDExpiryDate = oSQLiteDataReader["NCDExpiryDate"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(NCDExpiryDate))
                        objNCDEXMaster.NCDExpiryDate = Convert.ToInt64(NCDExpiryDate);
                    //NCDDisplayExpiryDate 
                    var NCDDisplayExpiryDate = oSQLiteDataReader["NCDDisplayExpiryDate"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(NCDDisplayExpiryDate))
                        objNCDEXMaster.NCDDisplayExpiryDate = NCDDisplayExpiryDate;
                    //NCDStrikePrice 
                    var NCDStrikePrice = oSQLiteDataReader["NCDStrikePrice"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(NCDStrikePrice))
                        objNCDEXMaster.NCDStrikePrice = Convert.ToInt64(NCDStrikePrice);
                    //NCDOptionType 
                    var NCDOptionType = oSQLiteDataReader["NCDOptionType"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(NCDOptionType))
                        objNCDEXMaster.NCDOptionType = NCDOptionType;
                    //NCDBoardLotQuantity 
                    var NCDBoardLotQuantity = oSQLiteDataReader["NCDBoardLotQuantity"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(NCDBoardLotQuantity))
                        objNCDEXMaster.NCDBoardLotQuantity = Convert.ToInt64(NCDBoardLotQuantity);
                    //NCDTickSize 
                    var NCDTickSize = oSQLiteDataReader["NCDTickSize"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(NCDTickSize))
                        objNCDEXMaster.NCDTickSize = Convert.ToInt64(NCDTickSize);
                    //NCDName 
                    var NCDName = oSQLiteDataReader["NCDName"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(NCDName))
                        objNCDEXMaster.NCDName = NCDName;
                    //NCDExerciseStartDate 
                    var NCDExerciseStartDate = oSQLiteDataReader["NCDExerciseStartDate"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(NCDExerciseStartDate))
                        objNCDEXMaster.NCDExerciseStartDate = Convert.ToInt64(NCDExerciseStartDate);
                    //NCDExerciseEndDate 
                    var NCDExerciseEndDate = oSQLiteDataReader["NCDExerciseEndDate"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(NCDExerciseEndDate))
                        objNCDEXMaster.NCDExerciseEndDate = Convert.ToInt64(NCDExerciseEndDate);
                    //NCDPriceNumerator 
                    var NCDPriceNumerator = oSQLiteDataReader["NCDPriceNumerator"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(NCDPriceNumerator))
                        objNCDEXMaster.NCDPriceNumerator = Convert.ToInt64(NCDPriceNumerator);
                    //NCDPriceDenominator 
                    var NCDPriceDenominator = oSQLiteDataReader["NCDPriceDenominator"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(NCDPriceDenominator))
                        objNCDEXMaster.NCDPriceDenominator = Convert.ToInt64(NCDPriceDenominator);
                    //NCDPriceUnit 
                    var NCDPriceUnit = oSQLiteDataReader["NCDPriceUnit"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(NCDPriceUnit))
                        objNCDEXMaster.NCDPriceUnit = NCDPriceUnit;
                    //NCDQuantityUnit 
                    var NCDQuantityUnit = oSQLiteDataReader["NCDQuantityUnit"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(NCDQuantityUnit))
                        objNCDEXMaster.NCDQuantityUnit = NCDQuantityUnit;
                    //NCDDeliveryUnit 
                    var NCDDeliveryUnit = oSQLiteDataReader["NCDDeliveryUnit"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(NCDDeliveryUnit))
                        objNCDEXMaster.NCDDeliveryUnit = NCDDeliveryUnit;
                    //NCDDeliveryLotQuantity 
                    var NCDDeliveryLotQuantity = oSQLiteDataReader["NCDDeliveryLotQuantity"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(NCDDeliveryLotQuantity))
                        objNCDEXMaster.NCDDeliveryLotQuantity = Convert.ToInt64(NCDDeliveryLotQuantity);
                    //NCDSequenceId 
                    var NCDSequenceId = oSQLiteDataReader["NCDSequenceId"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(NCDSequenceId))
                        objNCDEXMaster.NCDSequenceId = Convert.ToInt64(NCDSequenceId);
                    //NCDTokenString 
                    var NCDTokenString = oSQLiteDataReader["NCDTokenString"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(NCDTokenString))
                        objNCDEXMaster.NCDTokenString = NCDTokenString;


                    if (!objNCDEXMasterDict.ContainsKey(objNCDEXMaster.NCDToken))
                    {
                        objNCDEXMasterDict.Add(objNCDEXMaster.NCDToken, objNCDEXMaster);
                    }
                }
                objNCDEXMasterDict = objNCDEXMasterDict.OrderBy(x => x.Value.NCDToken).ToDictionary(x => x.Key, x => x.Value);
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

        private static void SetlMas_txt()
        {
           oDataAccessLayer.OpenConnection((int)DataAccessLayer.ConnectionDB.Masters);
            try
            {

                string strQuery = @"SELECT * FROM BSE_EQ_SETLMAS_CFE;";
                SQLiteDataReader oSQLiteDataReader = oDataAccessLayer.ExecuteDataReader((int)ConnectionDB.Masters,strQuery, System.Data.CommandType.Text, null);
                while (oSQLiteDataReader.Read())
                {
                    SetlMas objSetlMas = new SetlMas();
                    var field1 = oSQLiteDataReader["Field1"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(field1))
                        objSetlMas.Field1 = field1.ToString();

                    var field2 = oSQLiteDataReader["Field2"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(field2))
                        objSetlMas.Field2 = field2.ToString();

                    var field3 = oSQLiteDataReader["Field3"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(field3))
                        objSetlMas.Field3 = Convert.ToDateTime(field3).ToString("dd-MM-yyyy");

                    var field4 = oSQLiteDataReader["Field4"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(field4))
                        objSetlMas.Field4 = Convert.ToDateTime(field4).ToString("dd-MM-yyyy");

                    var field5 = oSQLiteDataReader["Field5"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(field5))
                        objSetlMas.Field5 = Convert.ToDateTime(field5).ToString("dd-MM-yyyy");

                    var field6 = oSQLiteDataReader["Field6"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(field6))
                        objSetlMas.Field6 = Convert.ToDateTime(field6).ToString("dd-MM-yyyy");

                    var field7 = oSQLiteDataReader["Field7"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(field7))
                        objSetlMas.Field7 = Convert.ToDateTime(field7).ToString("dd-MM-yyyy");

                    var field8 = oSQLiteDataReader["Field8"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(field8))
                        objSetlMas.Field8 = Convert.ToDateTime(field8).ToString("dd-MM-yyyy");

                    var field9 = oSQLiteDataReader["Field9"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(field9))
                        objSetlMas.Field9 = Convert.ToDateTime(field9).ToString("dd-MM-yyyy");

                    listSetlMas.Add(objSetlMas);
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Error in MasterSharedMemory SetlMas_txt()");
                ExceptionUtility.LogError(ex);
            }
            finally
            {
                oDataAccessLayer.CloseConnection((int)DataAccessLayer.ConnectionDB.Masters);
            }
        }

        private static void ScripMasterSybas_csv()
        {
            try
            {
                oDataAccessLayer.OpenConnection((int)DataAccessLayer.ConnectionDB.Masters);

                string strQuery = @"SELECT IndexCode,IndexID,NoofConstituents FROM BSE_SYSBASMAIN_CFE";
                SQLiteDataReader oSQLiteDataReader = oDataAccessLayer.ExecuteDataReader((int)ConnectionDB.Masters,strQuery, System.Data.CommandType.Text, null);

                while (oSQLiteDataReader.Read())
                {
                    ScripMasterSybas objScripMasterSys = new ScripMasterSybas();


                    var IndexCode = oSQLiteDataReader["IndexCode"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(IndexCode))
                        objScripMasterSys.IndexCode = Convert.ToInt32(IndexCode);

                    objScripMasterSys.IndexID = oSQLiteDataReader["IndexID"]?.ToString();

                    var NoofConstituents = oSQLiteDataReader["NoofConstituents"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(NoofConstituents))
                        objScripMasterSys.NoOfConstituents = Convert.ToInt32(NoofConstituents);

                    if (!objMasterDicSyb.ContainsKey(objScripMasterSys.IndexCode))
                    {
                        objMasterDicSyb.Add(objScripMasterSys.IndexCode, objScripMasterSys);
                    }
                }

                strQuery = "SELECT IndexCode,ScripCode,NoofShares FROM BSE_SYSBASSUB_CFE";
                oSQLiteDataReader = oDataAccessLayer.ExecuteDataReader((int)ConnectionDB.Masters,strQuery, System.Data.CommandType.Text, null);

                while (oSQLiteDataReader.Read())
                {
                    ScripMasterSybas objScripMasterSys = new ScripMasterSybas();
                    var IndexCode = oSQLiteDataReader["IndexCode"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(IndexCode))
                        objScripMasterSys.IndexCodeSub = Convert.ToInt32(IndexCode);

                    var ScripCode = oSQLiteDataReader["ScripCode"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(ScripCode))
                        objScripMasterSys.ScripCodeSyb = Convert.ToInt32(ScripCode);

                    var NoofShares = oSQLiteDataReader["NoofShares"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(NoofShares))
                        objScripMasterSys.NoOfShares = Convert.ToInt64(NoofShares);
                    MainListSybas.Add(objScripMasterSys);
                }
                //objMasterDicSyb = objMasterDicSyb.OrderBy(x => x.Value.IndexID).ToDictionary(x => x.Key, x => x.Value);
                //objMasterDicSyb = objMasterDicSyb.OrderBy(x => x.Value.IndexID).ToDictionary(x => x.Key, x => x.Value);
            }

            catch (Exception ex1)
            {
                //  ExceptionUtility.LogError(ex1);
            }
            finally
            {
                oDataAccessLayer.CloseConnection((int)DataAccessLayer.ConnectionDB.Masters);
            }
        }


        #region CPCodeDerivative

        public class CPCodeDerivative
        {
            private string _ParticipantId;

            public string ParticipantId
            {
                get { return _ParticipantId; }
                set { _ParticipantId = value;  }
            }

            private string _ParticipantName;

            public string ParticipantName
            {
                get { return _ParticipantName; }
                set { _ParticipantName = value; }
            }

            private char _ParticipantStatus;

            public char ParticipantStatus
            {
                get { return _ParticipantStatus; }
                set { _ParticipantStatus = value; }
            }

            private char _DeleteFlag;

            public char DeleteFlag
            {
                get { return _DeleteFlag; }
                set { _DeleteFlag = value; }
            }

            private string _InfoDate;

            public string InfoDate
            {
                get { return _InfoDate; }
                set { _InfoDate = value; }
            }


        }
        #endregion

        #region CPCodeCurrency


        public class CPCodeCurrency
        {
            private string _ParticipantId;

            public string ParticipantId
            {
                get { return _ParticipantId; }
                set { _ParticipantId = value; }
            }

            private string _ParticipantName;

            public string ParticipantName
            {
                get { return _ParticipantName; }
                set { _ParticipantName = value; }
            }

            private char _ParticipantStatus;

            public char ParticipantStatus
            {
                get { return _ParticipantStatus; }
                set { _ParticipantStatus = value; }
            }

            private char _DeleteFlag;

            public char DeleteFlag
            {
                get { return _DeleteFlag; }
                set { _DeleteFlag = value; }
            }

            private string _InfoDate;

            public string InfoDate
            {
                get { return _InfoDate; }
                set { _InfoDate = value; }
            }
        }
        #endregion


        #region ClientMaster
        public class ClientMaster
        {
            private string _SerialNo; public string SerialNo { get { return _SerialNo; } set { _SerialNo = value; } }
            private string _FirstName; public string FirstName { get { return _FirstName; } set { _FirstName = value; } }
            //private string _MiddleName; public string MiddleName { get { return _MiddleName; } set { _MiddleName = value; } }
            private string _LastName; public string LastName { get { return _LastName; } set { _LastName = value; } }
            private string _ClientType; public string ClientType { get { return _ClientType; } set { _ClientType = value; } }
            private string _ClientStatus; public string ClientStatus { get { return _ClientStatus; } set { _ClientStatus = value; } }
            private string _MobileNumber; public string MobileNumber { get { return _MobileNumber; } set { _MobileNumber = value; } }
            private string _EmailID; public string EmailID { get { return _EmailID; } set { _EmailID = value; } }
            private string _ShortClientId; public string ShortClientId { get { return _ShortClientId; } set { _ShortClientId = value; } }
            private string _CompleteClientId; public string CompleteClientId { get { return _CompleteClientId; } set { _CompleteClientId = value; } }
            private string _CPCodeDerivative; public string CPCodeDerivative { get { return _CPCodeDerivative; } set { _CPCodeDerivative = value; } }
            private string _CPCodeCurrency; public string CPCodeCurrency { get { return _CPCodeCurrency; } set { _CPCodeCurrency = value; } }
           
        }
        #endregion

        public static void ScripMasterDebtInfo_read()
        {
            try
            {
                oDataAccessLayer.OpenConnection((int)DataAccessLayer.ConnectionDB.Masters);
                string strQuery = @"SELECT 
                                    ScripCodeDebt, ISINNumber, TypeOfOwnership, NatureOfBusiness, IssuePrice, FaceValue, RedemptionValue,
                                    RedemptionRecordDate, RedemptionExDate, AllotmentDate, MaturityConversionDate, IssueSize,
                                    CreditAgencyandRating1, CreditAgencyandRating2, CreditAgencyandRating3, TypeofCoupon, CouponRateINPerc, CouponType,
                                    CouponFrequency, DayCountConvention, LastIntPaymentDate, NextIntPaymentDate, NextIntPaymentRec, NextIntPaymentEx,
                                    StepUpStepDown, PutOptionFlagandDate, PutOptionFlagRecordDate, PutOptionFlagExDate, CallOptionFlagandDate, CallOptionFlagRecordDate,
                                    CallOptionFlagExDate, ExchangeListed, Seniority, Redemption, ModeofIssue, SecurityStatus, WhetherTaxFree, InfraCategory,
                                    Guaranteed, Convertibility, ConvertibilityRecordDate, ConvertibilityExDate, Registrar, DebentureTrustee, ComplianceCompanySecretary,
                                    Arranger, Remarks, AccruedInterest, TradeDate, SettelementDate
                                    FROM BSE_DEBTINFO_CFE";
                SQLiteDataReader oSQLiteDataReader = oDataAccessLayer.ExecuteDataReader((int)ConnectionDB.Masters,strQuery, System.Data.CommandType.Text, null);
                while (oSQLiteDataReader.Read())
                {
                    ScripMasterDebtInfo objScipMaster1 = new ScripMasterDebtInfo();
                    objScipMaster1.ScripCodeDebt = Convert.ToInt32(oSQLiteDataReader["ScripCodeDebt"]);
                    objScipMaster1.ISINNumber = oSQLiteDataReader["ISINNumber"]?.ToString().Trim();
                    objScipMaster1.TypeOfOwnership = oSQLiteDataReader["TypeOfOwnership"]?.ToString().Trim();
                    objScipMaster1.NatureOfBusiness = oSQLiteDataReader["NatureOfBusiness"]?.ToString().Trim();

                    var IssuePrice = oSQLiteDataReader["IssuePrice"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(IssuePrice))
                        objScipMaster1.IssuePrice = Convert.ToInt64(IssuePrice);

                    var FaceValue = oSQLiteDataReader["FaceValue"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(FaceValue))
                        objScipMaster1.FaceValue = Convert.ToInt64(FaceValue);

                    var RedemptionValue = oSQLiteDataReader["RedemptionValue"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(RedemptionValue))
                        objScipMaster1.RedemptionValue = Convert.ToInt64(RedemptionValue);

                    objScipMaster1.RedemptionRecordDate = oSQLiteDataReader["RedemptionRecordDate"]?.ToString().Trim();
                    objScipMaster1.RedemptionExDate = oSQLiteDataReader["RedemptionExDate"]?.ToString().Trim();
                    objScipMaster1.AllotmentDateDDMMYYYY = oSQLiteDataReader["AllotmentDate"]?.ToString().Trim();
                    objScipMaster1.MaturityConversionDateDDMMYYYY = oSQLiteDataReader["MaturityConversionDate"]?.ToString().Trim();

                    var IssueSize = oSQLiteDataReader["IssueSize"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(IssueSize))
                        objScipMaster1.IssueSize = Convert.ToUInt64(IssueSize);

                    objScipMaster1.CreditAgencyandRating1 = oSQLiteDataReader["CreditAgencyandRating1"]?.ToString().Trim();
                    objScipMaster1.CreditAgencyandRating2 = oSQLiteDataReader["CreditAgencyandRating2"]?.ToString().Trim();
                    objScipMaster1.CreditAgencyandRating3 = oSQLiteDataReader["CreditAgencyandRating3"]?.ToString().Trim();
                    objScipMaster1.TypeofCoupon = oSQLiteDataReader["TypeofCoupon"]?.ToString().Trim();
                    objScipMaster1.CouponRateINPerc = oSQLiteDataReader["CouponRateINPerc"]?.ToString().Trim();
                    objScipMaster1.CouponType = oSQLiteDataReader["CouponType"]?.ToString().Trim();
                    objScipMaster1.CouponFrequency = oSQLiteDataReader["CouponFrequency"]?.ToString().Trim();
                    objScipMaster1.DayCountConvention = oSQLiteDataReader["DayCountConvention"]?.ToString().Trim();
                    objScipMaster1.LastIntPaymentDate = oSQLiteDataReader["LastIntPaymentDate"]?.ToString().Trim();
                    objScipMaster1.NextIntPaymentDate = oSQLiteDataReader["NextIntPaymentDate"]?.ToString().Trim();
                    objScipMaster1.NextIntPaymentRec = oSQLiteDataReader["NextIntPaymentRec"]?.ToString().Trim();
                    objScipMaster1.NextIntPaymentEx = oSQLiteDataReader["NextIntPaymentEx"]?.ToString().Trim();
                    objScipMaster1.StepUpStepDown = oSQLiteDataReader["StepUpStepDown"]?.ToString().Trim();
                    objScipMaster1.PutOptionFlagandDate = oSQLiteDataReader["PutOptionFlagandDate"]?.ToString().Trim();
                    objScipMaster1.PutOptionFlagRecordDate = oSQLiteDataReader["PutOptionFlagRecordDate"]?.ToString().Trim();
                    objScipMaster1.PutOptionFlagExDate = oSQLiteDataReader["PutOptionFlagExDate"]?.ToString().Trim();
                    objScipMaster1.CallOptionFlagandDate = oSQLiteDataReader["CallOptionFlagandDate"]?.ToString().Trim();
                    objScipMaster1.CallOptionFlagRecordDate = oSQLiteDataReader["CallOptionFlagRecordDate"]?.ToString().Trim();
                    objScipMaster1.CallOptionFlagExDate = oSQLiteDataReader["CallOptionFlagExDate"]?.ToString().Trim();
                    objScipMaster1.ExchangeListed = oSQLiteDataReader["ExchangeListed"]?.ToString().Trim();
                    objScipMaster1.Seniority = oSQLiteDataReader["Seniority"]?.ToString().Trim();
                    objScipMaster1.Redemption = oSQLiteDataReader["Redemption"]?.ToString().Trim();
                    objScipMaster1.ModeofIssue = oSQLiteDataReader["ModeofIssue"]?.ToString().Trim();
                    objScipMaster1.SecurityStatus = oSQLiteDataReader["SecurityStatus"]?.ToString().Trim();
                    objScipMaster1.WhetherTaxFree = oSQLiteDataReader["WhetherTaxFree"]?.ToString().Trim();
                    objScipMaster1.InfraCategory = oSQLiteDataReader["InfraCategory"]?.ToString().Trim();
                    objScipMaster1.Guaranteed = oSQLiteDataReader["Guaranteed"]?.ToString().Trim();
                    objScipMaster1.Convertibility = oSQLiteDataReader["Convertibility"]?.ToString().Trim();
                    objScipMaster1.ConvertibilityRecordDate = oSQLiteDataReader["ConvertibilityRecordDate"]?.ToString().Trim();
                    objScipMaster1.ConvertibilityExDate = oSQLiteDataReader["ConvertibilityExDate"]?.ToString().Trim();
                    objScipMaster1.Registrar = oSQLiteDataReader["Registrar"]?.ToString().Trim();
                    objScipMaster1.DebentureTrustee = oSQLiteDataReader["DebentureTrustee"]?.ToString().Trim();
                    objScipMaster1.ComplianceCompanySecretary = oSQLiteDataReader["ComplianceCompanySecretary"]?.ToString().Trim();
                    objScipMaster1.Arranger = oSQLiteDataReader["Arranger"]?.ToString().Trim();
                    objScipMaster1.Remarks = oSQLiteDataReader["Remarks"]?.ToString().Trim();
                    objScipMaster1.AccruedInterest = oSQLiteDataReader["AccruedInterest"]?.ToString().Trim();
                    objScipMaster1.TradeDate = oSQLiteDataReader["TradeDate"]?.ToString().Trim();
                    objScipMaster1.SettelementDate = oSQLiteDataReader["SettelementDate"]?.ToString().Trim();

                    //TODO TBD2017
                    if (!objScripMasterDebtInfo.ContainsKey(objScipMaster1.ScripCodeDebt))
                    {
                        objScripMasterDebtInfo.Add(objScipMaster1.ScripCodeDebt, objScipMaster1);
                    }
                }
                objScripMasterDebtInfo = objScripMasterDebtInfo.OrderBy(x => x.Value.ScripCodeDebt).ToDictionary(x => x.Key, x => x.Value);
            }
            catch (Exception e)
            {
                ExceptionUtility.LogError(e);
            }
            finally
            {
                oDataAccessLayer.CloseConnection((int)DataAccessLayer.ConnectionDB.Masters);
            }
        }

        //Reading Exchange Column Profile
        private static void ReadExchangeColumnProfile()
        {
            try
            {
                objTouchlineCollection = new List<TouchlineWindow>();
                oDataAccessLayer.OpenConnection((int)DataAccessLayer.ConnectionDB.Masters);

# if TWS
                string strQuery = @"SELECT ScreenName,FieldName,DefaultColumns,FEColumns FROM TOUCHLINE where FEColumns='1' OR FEColumns='3'";
#elif BOW
                 string strQuery = @"SELECT ScreenName,FieldName,DefaultColumns,FEColumns FROM TOUCHLINE where FEColumns='1' OR FEColumns='2'";
#endif

                SQLiteDataReader oSQLiteDataReader = oDataAccessLayer.ExecuteDataReader((int)ConnectionDB.Masters,strQuery, System.Data.CommandType.Text, null);

                while (oSQLiteDataReader.Read())
                {
                    TouchlineWindow objTouchlineWindow = new TouchlineWindow();

                    //Screen Name
                    objTouchlineWindow.ScreenName = oSQLiteDataReader["ScreenName"]?.ToString().Trim();

                    //Field Name
                    objTouchlineWindow.FieldName = oSQLiteDataReader["FieldName"]?.ToString().Trim();

                    //Default Columns 
                    objTouchlineWindow.DefaultColumns = Convert.ToInt16(oSQLiteDataReader["DefaultColumns"]?.ToString().Trim());

                    //FE Columns
                    //1- Columns Common for both FE
                    //2 - Only BOW Specific Col
                    //3- Only TWS Specific Col 
                    objTouchlineWindow.FEColumns = Convert.ToInt16(oSQLiteDataReader["FEColumns"]?.ToString().Trim());

                    objTouchlineCollection.Add(objTouchlineWindow);
                }
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

        private static string[] ConvertDatatoFormat(string[] a)
        {
            for (int i = 0; i < a.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(a[i]))
                {
                    a[i] = null;
                }
            }
            return a;
        }

        public static void DP_file()
        {
            try
            {
                oDataAccessLayer.OpenConnection((int)DataAccessLayer.ConnectionDB.Masters);

                string strQuery = @"SELECT * FROM BSE_EQ_DP_CFE;";
                SQLiteDataReader oSQLiteDataReader = oDataAccessLayer.ExecuteDataReader((int)ConnectionDB.Masters,strQuery, System.Data.CommandType.Text, null);
                while (oSQLiteDataReader.Read())
                {
                    DP objDP = new DP();

                    //ScripCode
                    var ScripCode = oSQLiteDataReader["ScripCode"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(ScripCode))
                        objDP.SecurityCode = Convert.ToInt32(ScripCode);

                    //CircuitRelaxFlag
                    var CircuitRelaxFlag = oSQLiteDataReader["CircuitRelaxFlag"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(CircuitRelaxFlag))
                        objDP.CircuitRelaxFlag = Convert.ToChar(CircuitRelaxFlag);

                    //PreviousClosePrice
                    var PreviousClosePrice = oSQLiteDataReader["PreviousClosePrice"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(PreviousClosePrice))
                        objDP.PreviousClosePrice = Convert.ToInt64(PreviousClosePrice);

                    //LowerCircuit
                    var LowerCircuit = oSQLiteDataReader["LowerCircuit"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(LowerCircuit))
                        objDP.LowerCircuit = Convert.ToInt32(LowerCircuit);

                    //UpperCircuit
                    var UpperCircuit = oSQLiteDataReader["UpperCircuit"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(UpperCircuit))
                        objDP.UpperCircuit = Convert.ToInt32(UpperCircuit);

                    //LowerCircuitPrice
                    var LowerCircuitPrice = oSQLiteDataReader["LowerCircuitPrice"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(LowerCircuitPrice))
                        objDP.LowerCircuitPrice = Convert.ToInt64(LowerCircuitPrice);

                    //UpperCircuitePrice
                    var UpperCircuitePrice = oSQLiteDataReader["UpperCircuitePrice"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(UpperCircuitePrice))
                        objDP.UpperCircuitePrice = Convert.ToInt64(UpperCircuitePrice);

                    //WeeksHighprice
                    var WeeksHighprice = oSQLiteDataReader["WeeksHighprice"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(WeeksHighprice))
                        objDP.WeeksHighprice = Convert.ToInt32(WeeksHighprice);

                    //WeeksLowprice
                    var WeeksLowprice = oSQLiteDataReader["WeeksLowprice"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(WeeksLowprice))
                        objDP.WeeksLowprice = Convert.ToInt32(WeeksLowprice);

                    //Dateof52weeksHighprice
                    var Dateof52weeksHighprice = oSQLiteDataReader["Dateof52weeksHighprice"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(Dateof52weeksHighprice))
                        if (Dateof52weeksHighprice.Length == 7)
                            Dateof52weeksHighprice = Dateof52weeksHighprice.PadLeft(8, '0');
                    objDP.Dateof52weeksHighprice = Dateof52weeksHighprice;

                    //Dateof52weeksLowprice
                    var Dateof52weeksLowprice = oSQLiteDataReader["Dateof52weeksLowprice"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(Dateof52weeksLowprice))
                        if (Dateof52weeksLowprice.Length == 7)
                            Dateof52weeksLowprice = Dateof52weeksLowprice.PadLeft(8, '0');
                    objDP.Dateof52weeksLowprice = Dateof52weeksLowprice;


                    //LastTradeDate
                    var LastTradeDate = oSQLiteDataReader["LastTradeDate"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(LastTradeDate))
                        if (LastTradeDate.Length == 7)
                            LastTradeDate = LastTradeDate.PadLeft(8, '0');
                    objDP.LastTradeDate = LastTradeDate;

                    //DecimalLocator
                    var DecimalLocator = oSQLiteDataReader["DecimalLocator"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(DecimalLocator))
                        objDP.DecimalLocator = Convert.ToInt32(DecimalLocator);

                    //Filler2
                    var Filler2 = oSQLiteDataReader["Filler2"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(Filler2))
                        objDP.Filler2 = Convert.ToInt32(Filler2);

                    //Filler3
                    var Filler3 = oSQLiteDataReader["Filler3"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(Filler3))
                        objDP.Filler3 = Convert.ToInt32(Filler3);

                    //Filler4
                    var Filler4 = oSQLiteDataReader["Filler4"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(Filler4))
                        objDP.Filler4 = Convert.ToInt32(Filler4);

                    if (!objDicDP.ContainsKey(objDP.SecurityCode))
                    {
                        objDicDP.Add(objDP.SecurityCode, objDP);
                    }
                }
                objDicDP = objDicDP.OrderBy(x => x.Value.SecurityCode).ToDictionary(x => x.Key, x => x.Value);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Error in MasterSharedMemory DP File");
                ExceptionUtility.LogError(ex);
            }
            finally
            {
                oDataAccessLayer.CloseConnection((int)DataAccessLayer.ConnectionDB.Masters);
            }
        }

        private static void SpnIndices_mas()
        {
            try
            {
                oDataAccessLayer.OpenConnection((int)DataAccessLayer.ConnectionDB.Masters);
                string strQuery = @"SELECT  IndexCode, ExistingShortName, RebrandedLongName 
                                            FROM BSE_SNPINDICES_CFE;";
                SQLiteDataReader oSQLiteDataReader = oDataAccessLayer.ExecuteDataReader((int)ConnectionDB.Masters,strQuery, System.Data.CommandType.Text, null);

                while (oSQLiteDataReader.Read())
                {
                    ScripMasterSpnIndices objSpnInd = new ScripMasterSpnIndices();

                    var IndexCode = oSQLiteDataReader["IndexCode"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(IndexCode))
                        objSpnInd.IndexCode = Convert.ToInt64(IndexCode);

                    objSpnInd.ExistingShortName_ca = oSQLiteDataReader["ExistingShortName"]?.ToString().Trim();
                    objSpnInd.RebrandedLongName_ca = oSQLiteDataReader["RebrandedLongName"]?.ToString().Trim();

                    if (!objSpnIndicesDic.ContainsKey(objSpnInd.IndexCode))
                    {
                        objSpnIndicesDic.Add(objSpnInd.IndexCode, objSpnInd);
                    }
                }
                objSpnIndicesDic = objSpnIndicesDic.OrderBy(x => x.Value.IndexCode).ToDictionary(x => x.Key, x => x.Value);
            }
            catch (Exception exs)
            {
                //ExceptionUtility.LogError(exs);
            }
            finally
            {
                oDataAccessLayer.CloseConnection((int)DataAccessLayer.ConnectionDB.Masters);
            }
        }

        private static string GetLatestCoFile(string filePattern, int length)
        {
            string File1Path = string.Empty;
            string File2Path = string.Empty;
            string File2 = string.Empty;
            string File1 = string.Empty;

            DirectoryInfo masterPath = new DirectoryInfo(
         Path.GetFullPath(Path.Combine(currentDir, @"Master files/")));
            try
            {
                string[] coFiles = Directory.GetFiles(masterPath.ToString(), filePattern);


                if (coFiles.Length > 0)
                {
                    coFiles = Directory.GetFiles(masterPath.ToString(), filePattern);
                    int i = 0;
                    File1 = Path.GetFileName(coFiles[i]);
                    while (File1 != null && File1.Length != length)
                    {
                        i++;
                        if (i < coFiles.Length)
                            File1 = coFiles[i];
                    }

                    i++;
                    File1Path = masterPath + File1;

                    while (i < coFiles.Length)
                    {
                        File1Path = masterPath + File1;

                        string CompareFile = Path.GetFileName(coFiles[i]);
                        string ComparePath = masterPath + CompareFile;

                        if (CompareFile != string.Empty && CompareFile.Length != length)
                            continue;
                        if (CompareDates(File1, CompareFile, filePattern.IndexOf('*')) == 1) // File 1 > CompareFile
                        {
                            if (File2 != string.Empty)
                            {
                                if (CompareDates(File2, CompareFile, filePattern.IndexOf('*')) == 0) // File 2 < CompareFile
                                {
                                    File2 = CompareFile;
                                    File2Path = ComparePath;
                                }
                            }
                            else
                            {
                                File2 = CompareFile;
                                File2Path = ComparePath;
                            }
                        }
                        else if (CompareDates(File1, CompareFile, filePattern.IndexOf('*')) == 0) // File 1 < CompareFile
                        {
                            File2 = File1;
                            File2Path = File1Path;

                            File1 = CompareFile;
                            File1Path = ComparePath;
                        }
                        i++;
                    }

                    int index = 0;

                    while (index < coFiles.Length)
                    {
                        if (!(coFiles[index] == File1Path || coFiles[index] == File2Path))
                        {
                            File.Delete(coFiles[index]);
                        }
                        index++;
                    }
                }
                else
                {
                    return string.Format("{0}", string.Empty);
                }
            }
            catch (Exception ex)
            {

                // ExceptionUtility.LogError(ex);
            }
            return masterPath + File1;
        }

        private static int CompareDates(string tempFile1, string tempFile2, int index)
        {
            int result = 1;
            DateTime dt1;
            DateTime dt2;
            if (tempFile1.Substring(0, 8) == "SETLMAS_")
            {
                dt1 = new DateTime(Convert.ToInt32(tempFile1.Substring(index + 2, 2)), Convert.ToInt32(tempFile1.Substring(index, 2)), 01);

                dt2 = new DateTime(Convert.ToInt32(tempFile2.Substring(index + 2, 2)), Convert.ToInt32(tempFile2.Substring(index, 2)), 01);
            }
            else
            {

                dt1 = new DateTime(Convert.ToInt32(tempFile1.Substring(index + 4, 2)), Convert.ToInt32(tempFile1.Substring(index + 2, 2)),
                                           Convert.ToInt32(tempFile1.Substring(index, 2)));
                dt2 = new DateTime(Convert.ToInt32(tempFile2.Substring(index + 4, 2)), Convert.ToInt32(tempFile2.Substring(index + 2, 2)),
                                           Convert.ToInt32(tempFile2.Substring(index, 2)));

            }
            if (dt1 > dt2)
            {
                result = 1;
            }
            else if (dt1 < dt2)
            {
                result = 0;
            }
            return result;
        }

    }

    public class ScripMasterBase
    {
        //Added by Gaurav Jadhav 7/3/2018
        //Partition ID
        private string _partitionId; public string PartitionId { get { return _partitionId; } set { _partitionId = value; } }

        //Added by Gaurav Jadhav 7/3/2018
        //Market Segment ID
        private string _marketSegmentID; public string MarketSegmentID { get { return _marketSegmentID; } set { _marketSegmentID = value; } }

        //Quantity multiplier. default 1
        private int _quantityMultiplier = 1; public int QuantityMultiplier { get { return _quantityMultiplier; } set { _quantityMultiplier = value; } }

        private int _Segment;

        public int Segment
        {
            get { return _Segment; }
            set { _Segment = value; }
        }


        //MarketLot
        private Int32 _marketLot = 0;
        public Int32 MarketLot
        {
            get { return _marketLot; }
            set { _marketLot = value; }
        }

        //ScripGroup
        private string _groupName = string.Empty;
        public string GroupName
        {
            get { return _groupName; }
            set { _groupName = value; }
        }

        //FaceValue
        private long _faceValue = 0;
        public long FaceValue
        {
            get { return _faceValue; }
            set { _faceValue = value; }
        }

        //TickSize
        private Double _tickSize = 0;
        public Double TickSize
        {
            get { return _tickSize; }
            set { _tickSize = value; }
        }

        //GSMFlag
        private int _filler2_GSM = 0;
        public int Filler2_GSM
        {
            get { return _filler2_GSM; }
            set { _filler2_GSM = value; }
        }

        //VAR/IM%
        private decimal _iMPercentage;
        public decimal IMPercentage
        {
            get { return _iMPercentage; }
            set { _iMPercentage = value; }
        }

        //VAR/EM%
        private decimal _eMPercentage;
        public decimal EMPercentage
        {
            get { return _eMPercentage; }
            set { _eMPercentage = value; }
        }

        //ScripName
        private string _scripName = string.Empty;
        public string ScripName
        {
            get { return _scripName; }
            set { _scripName = value; }
        }

        //ScripId/Symbol
        private string _scripId = string.Empty;
        public string ScripId
        {
            get { return _scripId; }
            set { _scripId = value; }
        }

        //ScripCode
        private long _scripCode = 0;
        public long ScripCode
        {
            get { return _scripCode; }
            set { _scripCode = value; }
        }

        //Instrument type
        private char _InstrumentType;
        public char InstrumentType
        {
            get { return _InstrumentType; }
            set { _InstrumentType = value; }
        }

        //precision for Eq = 2
        private int _precision = 0;
        public int Precision
        {
            get { return _precision; }
            set { _precision = value; }
        }

        private int _bowtokenid;

        public int BowTokenID
        {
            get { return _bowtokenid; }
            set { _bowtokenid = value; }
        }

        private string _IsinCode;

        public string IsinCode
        {
            get { return _IsinCode; }
            set { _IsinCode = value; }
        }

    }

    public class ScripMaster : ScripMasterBase
    {
        //SCRIP_txt:12
        // 26 Property
        private int _sCID;
        public int SCID
        {
            get { return _sCID; }
            set { _sCID = value; }
        }

        private string _exchangeCode = string.Empty;
        public string ExchangeCode
        {
            get { return _exchangeCode; }
            set { _exchangeCode = value; }
        }

        private string _partitionId = string.Empty;
        public string PartitionId
        {
            get { return _partitionId; }
            set { _partitionId = value; }
        }

        private char _scripType = ' ';
        public char ScripType
        {
            get { return _scripType; }
            set { _scripType = value; }
        }


        private Char _bseExclusive = ' ';
        public Char BseExclusive
        {
            get { return _bseExclusive; }
            set { _bseExclusive = value; }
        }

        private Char _status = ' ';
        public Char Status
        {
            get { return _status; }
            set { _status = value; }
        }

        private string _exDivDate = string.Empty;
        public string ExDivDate
        {
            get { return _exDivDate; }
            set { _exDivDate = value; }
        }


        private string _noDelEndDate = string.Empty;
        public string NoDelEndDate
        {
            get { return _noDelEndDate; }
            set { _noDelEndDate = value; }
        }

        private string _noDelStartDate = string.Empty;
        public string NoDelStartDate
        {
            get { return _noDelStartDate; }
            set { _noDelStartDate = value; }
        }

        private string _newTickSize = string.Empty;
        public string NewTickSize
        {
            get { return _newTickSize; }
            set { _newTickSize = value; }
        }


        private string _isinCode = string.Empty;
        public string IsinCode
        {
            get { return _isinCode; }
            set { _isinCode = value; }
        }

        private Int32 _callAuctionIndicator = 0;
        public Int32 CallAuctionIndicator
        {
            get { return _callAuctionIndicator; }
            set { _callAuctionIndicator = value; }
        }

        private string _bcStartDate = string.Empty;
        public string BcStartDate
        {
            get { return _bcStartDate; }
            set { _bcStartDate = value; }
        }

        private string _exBonusDate = string.Empty;
        public string ExBonusDate
        {
            get { return _exBonusDate; }
            set { _exBonusDate = value; }
        }

        private string _exRightDate = string.Empty;
        public string ExRightDate
        {
            get { return _exRightDate; }
            set { _exRightDate = value; }
        }

        private string _filler = string.Empty;
        public string Filler
        {
            get { return _filler; }
            set { _filler = value; }
        }



        private string _bcEndDate = string.Empty;
        public string BcEndDate
        {
            get { return _bcEndDate; }
            set { _bcEndDate = value; }
        }

        private Int32 _filler3 = 0;
        public Int32 Filler3
        {
            get { return _filler3; }
            set { _filler3 = value; }
        }




        //Not required - TBD
        private int _sCSequenceId;

        public int SCSequenceId
        {
            get { return _sCSequenceId; }
            set { _sCSequenceId = value; }
        }

    }

    public class DerivativeMasterBase
    {
        //Segment
        private int _Segment;

        public int Segment
        {
            get { return _Segment; }
            set { _Segment = value; }
        }


        //ScripGroup
        private string _groupName = "DF";
        public string GroupName
        {
            get { return _groupName; }
            set { _groupName = value; }
        }

        private long _bowId;
        public long BowId
        {
            get { return _bowId; }
            set { _bowId = value; }
        }
        private long _contractTokenNum;
        public long ContractTokenNum
        {
            get { return _contractTokenNum; }
            set { _contractTokenNum = value; }
        }
        private long _assestTokenNum;
        public long AssestTokenNum
        {
            get { return _assestTokenNum; }
            set { _assestTokenNum = value; }
        }
        private string _instrumentType;
        public string InstrumentType
        {
            get { return _instrumentType; }
            set { _instrumentType = value; }
        }
        private string _scripId;
        public string ScripId
        {
            get { return _scripId; }
            set { _scripId = value; }
        }//for Series Code
        private string _underlyingAsset;
        public string UnderlyingAsset
        {
            get { return _underlyingAsset; }
            set { _underlyingAsset = value; }
        }
        private string _expiryDate;
        public string ExpiryDate
        {
            get { return _expiryDate; }
            set { _expiryDate = value; }
        }
        //Expiry Date in dd/MM/yyyy format
        private string _displayExpiryDate;
        public string DisplayExpiryDate
        {
            get { return _displayExpiryDate; }
            set { _displayExpiryDate = value; }
        }
        private long _strikePrice;
        public long StrikePrice
        {
            get { return _strikePrice; }
            set { _strikePrice = value; }
        }
        private string _optionType;
        public string OptionType
        {
            get { return _optionType; }
            set { _optionType = value; }
        }
        //precision for der = 2
        private int _precision = 0;
        public int Precision
        {
            get { return _precision; }
            set { _precision = value; }
        }
        private int _minimumLotSize;
        public int MinimumLotSize
        {
            get { return _minimumLotSize; }
            set { _minimumLotSize = value; }
        }//MarketLot

        private int _QuantityMultiplier;
        public int QuantityMultiplier
        {
            get { return _QuantityMultiplier; }
            set { _QuantityMultiplier = value; }
        }//MarketLot
        private int _tickSize;
        public int TickSize
        {
            get { return _tickSize; }
            set { _tickSize = value; }
        }
        private string _instrumentName;
        public string InstrumentName
        {
            get { return _instrumentName; }
            set { _instrumentName = value; }
        }
        private string _contractType;
        public string ContractType
        {
            get { return _contractType; }
            set { _contractType = value; }
        }
        private string _scripGroup;
        public string ScripGroup
        {
            get { return _scripGroup; }
            set { _scripGroup = value; }
        }
        private int _strategyID;
        public int StrategyID
        {
            get { return _strategyID; }
            set { _strategyID = value; }
        }

        private int _complexInstrumentType;
        public int ComplexInstrumentType
        {
            get { return _complexInstrumentType; }
            set { _complexInstrumentType = value; }
        }

        private string _assetCode = string.Empty;
        public string AssetCode
        {
            get { return _assetCode; }
            set { _assetCode = value; }
        }
        private int _partitionID;
        public int PartitionID
        {
            get { return _partitionID; }
            set { _partitionID = value; }
        }

        private int _productID;
        public int ProductID
        {
            get { return _productID; }
            set { _productID = value; }
        }
        private int _capacityGroupID;
        public int CapacityGroupID
        {
            get { return _capacityGroupID; }
            set { _capacityGroupID = value; }
        }
        private int _basePrice;
        public int BasePrice
        {
            get { return _basePrice; }
            set { _basePrice = value; }
        }
        private char _deleteFlag;
        public char DeleteFlag
        {
            get { return _deleteFlag; }
            set { _deleteFlag = value; }
        }
        private int _underlyingMarket = 0;
        public int UnderlyingMarket
        {
            get { return _underlyingMarket; }
            set { _underlyingMarket = value; }
        }

        private string _productCode = string.Empty;
        public string ProductCode
        {
            get { return _productCode; }
            set { _productCode = value; }
        }

        private string _DerScripGroup;

        public string DerScripGroup
        {
            get { return _DerScripGroup; }
            set { _DerScripGroup = value; }
        }
    }

    public class DerivativeMaster : DerivativeMasterBase
    {
        //CO Properties



        private int _quantityMultiplier;
        public int QuantityMultiplier
        {
            get { return _quantityMultiplier; }
            set { _quantityMultiplier = value; }
        }
        private int _underlyingMarket;
        public int UnderlyingMarket
        {
            get { return _underlyingMarket; }
            set { _underlyingMarket = value; }
        }
        private string _productCode;
        public string ProductCode
        {
            get { return _productCode; }
            set { _productCode = value; }
        }


        private string _CurrScripGroup;

        public string CurrScripGroup
        {
            get { return _CurrScripGroup; }
            set { _CurrScripGroup = value; }
        }


        //Spread Properties

        private int _contractTokenNum_Leg1;
        public int ContractTokenNum_Leg1
        {
            get { return _contractTokenNum_Leg1; }
            set { _contractTokenNum_Leg1 = value; }
        }
        private int _contractTokenNum_Leg2;
        public int ContractTokenNum_Leg2
        {
            get { return _contractTokenNum_Leg2; }
            set { _contractTokenNum_Leg2 = value; }
        }
        private long _NTAscripCode;
        public long NTAScripCode
        {
            get { return _NTAscripCode; }
            set { _NTAscripCode = value; }
        }
        private int _strategyID;
        public int StrategyID
        {
            get { return _strategyID; }
            set { _strategyID = value; }
        }
        private int _noofLegsinStrategy;
        public int NoofLegsinStrategy
        {
            get { return _noofLegsinStrategy; }
            set { _noofLegsinStrategy = value; }
        }
        private char _eligibility;
        public char Eligibility
        {
            get { return _eligibility; }
            set { _eligibility = value; }
        }
        private int _complexInstrumentType;
        public int ComplexInstrumentType
        {
            get { return _complexInstrumentType; }
            set { _complexInstrumentType = value; }
        }

        //NSE CO
        private string _inBannedPeriod;
        public string InBannedPeriod
        {
            get { return _inBannedPeriod; }
            set { _inBannedPeriod = value; }
        }

    }

    public class CurrencyMasterBase
    {
        private int _Segment;

        public int Segment
        {
            get { return _Segment; }
            set { _Segment = value; }
        }

        //ScripGroup
        private string _groupName = "CD";
        public string GroupName
        {
            get { return _groupName; }
            set { _groupName = value; }
        }

        private long _bowId;
        public long BowId
        {
            get { return _bowId; }
            set { _bowId = value; }
        }
        private long _contractTokenNum;
        public long ContractTokenNum
        {
            get { return _contractTokenNum; }
            set { _contractTokenNum = value; }
        }
        private long _assestTokenNum;
        public long AssestTokenNum
        {
            get { return _assestTokenNum; }
            set { _assestTokenNum = value; }
        }
        private string _instrumentType;
        public string InstrumentType
        {
            get { return _instrumentType; }
            set { _instrumentType = value; }
        }
        private string _scripId;
        public string ScripId
        {
            get { return _scripId; }
            set { _scripId = value; }
        }//for Series Code
        private string _underlyingAsset;
        public string UnderlyingAsset
        {
            get { return _underlyingAsset; }
            set { _underlyingAsset = value; }
        }
        private string _expiryDate;
        public string ExpiryDate
        {
            get { return _expiryDate; }
            set { _expiryDate = value; }
        }

        //Expiry Date in dd/MM/yyyy format
        private string _displayExpiryDate;
        public string DisplayExpiryDate
        {
            get { return _displayExpiryDate; }
            set { _displayExpiryDate = value; }
        }
        private long _strikePrice;
        public long StrikePrice
        {
            get { return _strikePrice; }
            set { _strikePrice = value; }
        }
        private string _optionType;
        public string OptionType
        {
            get { return _optionType; }
            set { _optionType = value; }
        }
        //precision for cur = 4
        private int _precision = 0;
        public int Precision
        {
            get { return _precision; }
            set { _precision = value; }
        }
        private int _minimumLotSize;
        public int MinimumLotSize
        {
            get { return _minimumLotSize; }
            set { _minimumLotSize = value; }
        }//MarketLot
        private int _tickSize;
        public int TickSize
        {
            get { return _tickSize; }
            set { _tickSize = value; }
        }
        private string _instrumentName;
        public string InstrumentName
        {
            get { return _instrumentName; }
            set { _instrumentName = value; }
        }
        private string _contractType;
        public string ContractType
        {
            get { return _contractType; }
            set { _contractType = value; }
        }
        private string _scripGroup = "CD";
        public string ScripGroup
        {
            get { return _scripGroup; }
            set { _scripGroup = value; }
        }
        private long _quantityMultiplier;
        public long QuantityMultiplier
        {
            get { return _quantityMultiplier; }
            set { _quantityMultiplier = value; }
        }
        private int _strategyID;
        public int StrategyID
        {
            get { return _strategyID; }
            set { _strategyID = value; }
        }

        private int _complexInstrumentType;
        public int ComplexInstrumentType
        {
            get { return _complexInstrumentType; }
            set { _complexInstrumentType = value; }
        }

        private int _partitionID;
        public int PartitionID
        {
            get { return _partitionID; }
            set { _partitionID = value; }
        }

        private int _productID;
        public int ProductID
        {
            get { return _productID; }
            set { _productID = value; }
        }


        private int _capacityGroupID;
        public int CapacityGroupID
        {
            get { return _capacityGroupID; }
            set { _capacityGroupID = value; }
        }

        private int _underlyingMarket;
        public int UnderlyingMarket
        {
            get { return _underlyingMarket; }
            set { _underlyingMarket = value; }
        }

        private int _basePrice;
        public int BasePrice
        {
            get { return _basePrice; }
            set { _basePrice = value; }
        }
        private char _deleteFlag;
        public char DeleteFlag
        {
            get { return _deleteFlag; }
            set { _deleteFlag = value; }
        }

        private string _CurrScripGroup;

        public string CurrScripGroup
        {
            get { return _CurrScripGroup; }
            set { _CurrScripGroup = value; }
        }


    }

    public class CurrencyMaster
    {
        //CO Properties
        private int _quantityMultiplier;
        public int QuantityMultiplier
        {
            get { return _quantityMultiplier; }
            set { _quantityMultiplier = value; }
        }

        //Spread Properties
        private int _contractTokenNum_Leg1;
        public int ContractTokenNum_Leg1
        {
            get { return _contractTokenNum_Leg1; }
            set { _contractTokenNum_Leg1 = value; }
        }
        private int _contractTokenNum_Leg2;
        public int ContractTokenNum_Leg2
        {
            get { return _contractTokenNum_Leg2; }
            set { _contractTokenNum_Leg2 = value; }
        }
        private long _NTAscripCode;
        public long NTAScripCode
        {
            get { return _NTAscripCode; }
            set { _NTAscripCode = value; }
        }

        private int _noofLegsinStrategy;
        public int NoofLegsinStrategy
        {
            get { return _noofLegsinStrategy; }
            set { _noofLegsinStrategy = value; }
        }
        private char _eligibility;
        public char Eligibility
        {
            get { return _eligibility; }
            set { _eligibility = value; }
        }

        private long _contractTokenNum;
        public long ContractTokenNum
        {
            get { return _contractTokenNum; }
            set { _contractTokenNum = value; }
        }

        private int _strategyID;
        public int StrategyID
        {
            get { return _strategyID; }
            set { _strategyID = value; }
        }

        private int _complexInstrumentType;
        public int ComplexInstrumentType
        {
            get { return _complexInstrumentType; }
            set { _complexInstrumentType = value; }
        }

        //NSE CO
        private string _inBannedPeriod;
        public string InBannedPeriod
        {
            get { return _inBannedPeriod; }
            set { _inBannedPeriod = value; }
        }

    }

    public class WholesaleDebtMarket
    {
        private long _wDMID;
        public long WDMID
        {
            get { return _wDMID; }
            set { _wDMID = value; }
        }
        private long _wDMScriptCode;
        public long WDMScriptCode
        {
            get { return _wDMScriptCode; }
            set { _wDMScriptCode = value; }
        }
        private string _wDMSegment;
        public string WDMSegment
        {
            get { return _wDMSegment; }
            set { _wDMSegment = value; }
        }
        private long _wDMSettlementType;
        public long WDMSettlementType
        {
            get { return _wDMSettlementType; }
            set { _wDMSettlementType = value; }
        }
        private long _wDMListingCatagory;
        public long WDMListingCatagory
        {
            get { return _wDMListingCatagory; }
            set { _wDMListingCatagory = value; }
        }
        private string _wDMInstrumentStandard;
        public string WDMInstrumentStandard
        {
            get { return _wDMInstrumentStandard; }
            set { _wDMInstrumentStandard = value; }
        }
        private string _wDMBondCategory;
        public string WDMBondCategory
        {
            get { return _wDMBondCategory; }
            set { _wDMBondCategory = value; }
        }
        private string _wDMGSecType;
        public string WDMGSecType
        {
            get { return _wDMGSecType; }
            set { _wDMGSecType = value; }
        }
        private string _wDMISIN;
        public string WDMISIN
        {
            get { return _wDMISIN; }
            set { _wDMISIN = value; }
        }
        private string _wDMSymbol; public string WDMSymbol { get { return _wDMSymbol; } set { _wDMSymbol = value; } }
        private string _wDMInstrumentName; public string WDMInstrumentName { get { return _wDMInstrumentName; } set { _wDMInstrumentName = value; } }
        private long _wDMLongExpiryDate; public long WDMLongExpiryDate { get { return _wDMLongExpiryDate; } set { _wDMLongExpiryDate = value; } }
        private string _wDMDisplayExpiryDate; public string WDMDisplayExpiryDate { get { return _wDMDisplayExpiryDate; } set { _wDMDisplayExpiryDate = value; } }
        private int _wDMStrikePrice; public int WDMStrikePrice { get { return _wDMStrikePrice; } set { _wDMStrikePrice = value; } }
        private string _wDMOptionType; public string WDMOptionType { get { return _wDMOptionType; } set { _wDMOptionType = value; } }
        private int _wDMMarketLot; public int WDMMarketLot { get { return _wDMMarketLot; } set { _wDMMarketLot = value; } }
        private decimal _wDMFaceValue; public decimal WDMFaceValue { get { return _wDMFaceValue; } set { _wDMFaceValue = value; } }
        private string _wDMIssueDate; public string WDMIssueDate { get { return _wDMIssueDate; } set { _wDMIssueDate = value; } }
        private decimal _wDMCoupon; public decimal WDMCoupon { get { return _wDMCoupon; } set { _wDMCoupon = value; } }
        private string _wDMIPPeriod; public string WDMIPPeriod { get { return _wDMIPPeriod; } set { _wDMIPPeriod = value; } }
        private string _wDMIPDuration; public string WDMIPDuration { get { return _wDMIPDuration; } set { _wDMIPDuration = value; } }
        private string _wDMPrevIPDate; public string WDMPrevIPDate { get { return _wDMPrevIPDate; } set { _wDMPrevIPDate = value; } }
        private string _wDMNextIPDate; public string WDMNextIPDate { get { return _wDMNextIPDate; } set { _wDMNextIPDate = value; } }
        private decimal _wDMAccruedIntrest; public decimal WDMAccruedIntrest { get { return _wDMAccruedIntrest; } set { _wDMAccruedIntrest = value; } }
        private string _wDMLastTradingDate; public string WDMLastTradingDate { get { return _wDMLastTradingDate; } set { _wDMLastTradingDate = value; } }
        private string _wDMExpiryDate; public string WDMExpiryDate { get { return _wDMExpiryDate; } set { _wDMExpiryDate = value; } }
        private decimal _wDMTickSize; public decimal WDMTickSize { get { return _wDMTickSize; } set { _wDMTickSize = value; } }
        private string _wDMActiveSuspendFlag; public string WDMActiveSuspendFlag { get { return _wDMActiveSuspendFlag; } set { _wDMActiveSuspendFlag = value; } }
        private string _wDMShutPeriodStartDate; public string WDMShutPeriodStartDate { get { return _wDMShutPeriodStartDate; } set { _wDMShutPeriodStartDate = value; } }
        private string _wDMShutPeriodEndDate; public string WDMShutPeriodEndDate { get { return _wDMShutPeriodEndDate; } set { _wDMShutPeriodEndDate = value; } }
        private string _wDMRecordDate; public string WDMRecordDate { get { return _wDMRecordDate; } set { _wDMRecordDate = value; } }
        private string _wDMFirstCallDate; public string WDMFirstCallDate { get { return _wDMFirstCallDate; } set { _wDMFirstCallDate = value; } }
        private string _wDMFirstPutDate; public string WDMFirstPutDate { get { return _wDMFirstPutDate; } set { _wDMFirstPutDate = value; } }
        private string _wDMCrisilRating; public string WDMCrisilRating { get { return _wDMCrisilRating; } set { _wDMCrisilRating = value; } }
        private string _wDMCareRating; public string WDMCareRating { get { return _wDMCareRating; } set { _wDMCareRating = value; } }
        private string _wDMIcraRating; public string WDMIcraRating { get { return _wDMIcraRating; } set { _wDMIcraRating = value; } }
        private string _wDMFitchRating; public string WDMFitchRating { get { return _wDMFitchRating; } set { _wDMFitchRating = value; } }

    }

    public class SLBMaster
    {
        private int _sCID;
        public int SCID
        {
            get { return _sCID; }
            set { _sCID = value; }
        }
        private int _sCScripCode;
        public int SCScripCode
        {
            get { return _sCScripCode; }
            set { _sCScripCode = value; }
        }
        private string _sCScripId;
        public string SCScripId
        {
            get { return _sCScripId; }
            set { _sCScripId = value; }
        }
        private string _scripID_ShortName;

        public string ScripID_ShortName
        {
            get { return _scripID_ShortName; }
            set { _scripID_ShortName = value; }
        }

        private string _sCISIN;
        public string SCISIN
        {
            get { return _sCISIN; }
            set { _sCISIN = value; }
        }
        private int _sCProductId;
        public int SCProductId
        {
            get { return _sCProductId; }
            set { _sCProductId = value; }
        }
        private string _sCProductCode;
        public string SCProductCode
        {
            get { return _sCProductCode; }
            set { _sCProductCode = value; }
        }
        private string _sCScripStatus;
        public string SCScripStatus
        {
            get { return _sCScripStatus; }
            set { _sCScripStatus = value; }
        }
        private int _sCMarketLot;
        public int SCMarketLot
        {
            get { return _sCMarketLot; }
            set { _sCMarketLot = value; }
        }
        private int _sCNoOfDays;
        public int SCNoOfDays
        {
            get { return _sCNoOfDays; }
            set { _sCNoOfDays = value; }
        }
        private string _sCStartDate;
        public string SCStartDate
        {
            get { return _sCStartDate; }
            set { _sCStartDate = value; }
        }
        private string _sCLastTrade_Date;
        public string SCLastTrade_Date
        {
            get { return _sCLastTrade_Date; }
            set { _sCLastTrade_Date = value; }
        }
        private string _sCExpiryDate;
        public string SCExpiryDate
        {
            get { return _sCExpiryDate; }
            set { _sCExpiryDate = value; }
        }
        private string _sCContractMonth;
        public string SCContractMonth
        {
            get { return _sCContractMonth; }
            set { _sCContractMonth = value; }
        }
        private int _sCRowstate;
        public int SCRowstate
        {
            get { return _sCRowstate; }
            set { _sCRowstate = value; }
        }
        private string _sCInstrumentName;
        public string SCInstrumentName
        {
            get { return _sCInstrumentName; }
            set { _sCInstrumentName = value; }
        }
        private long _sCLongExpiryDate;
        public long SCLongExpiryDate
        {
            get { return _sCLongExpiryDate; }
            set { _sCLongExpiryDate = value; }
        }
        private string _sCDisplayExpiryDate;
        public string SCDisplayExpiryDate
        {
            get { return _sCDisplayExpiryDate; }
            set { _sCDisplayExpiryDate = value; }
        }
        private int _sCStrikePrice;
        public int SCStrikePrice
        {
            get { return _sCStrikePrice; }
            set { _sCStrikePrice = value; }
        }
        private string _sCOptionType;
        public string SCOptionType
        {
            get { return _sCOptionType; }
            set { _sCOptionType = value; }
        }
        private int _sCTickSize;
        public int SCTickSize
        {
            get { return _sCTickSize; }
            set { _sCTickSize = value; }
        }
        private string _sCRollOver;
        public string SCRollOver
        {
            get { return _sCRollOver; }
            set { _sCRollOver = value; }
        }
        private string _sCFiller1;
        public string SCFiller1
        {
            get { return _sCFiller1; }
            set { _sCFiller1 = value; }
        }

    }

    public class ITPMaster
    {
        private long _iTPID; public long ITPID { get { return _iTPID; } set { _iTPID = value; } }
        private int _iTPScripCode; public int ITPScripCode { get { return _iTPScripCode; } set { _iTPScripCode = value; } }
        private string _iTPScripID; public string ITPScripID { get { return _iTPScripID; } set { _iTPScripID = value; } }
        private string _iTPScripName; public string ITPScripName { get { return _iTPScripName; } set { _iTPScripName = value; } }
        private string _iTPScripISIN; public string ITPScripISIN { get { return _iTPScripISIN; } set { _iTPScripISIN = value; } }
        private string _iTPGroupName; public string ITPGroupName { get { return _iTPGroupName; } set { _iTPGroupName = value; } }
        private long _iTPCompanyCode; public long ITPCompanyCode { get { return _iTPCompanyCode; } set { _iTPCompanyCode = value; } }
        private string _iTPInstrumenName; public string ITPInstrumenName { get { return _iTPInstrumenName; } set { _iTPInstrumenName = value; } }
        private long _iTPExpiryDate; public long ITPExpiryDate { get { return _iTPExpiryDate; } set { _iTPExpiryDate = value; } }
        private string _iTPDisplayExpiryDate; public string ITPDisplayExpiryDate { get { return _iTPDisplayExpiryDate; } set { _iTPDisplayExpiryDate = value; } }
        private int _iTPStrikePrice; public int ITPStrikePrice { get { return _iTPStrikePrice; } set { _iTPStrikePrice = value; } }
        private string _iTPOptionType; public string ITPOptionType { get { return _iTPOptionType; } set { _iTPOptionType = value; } }
        private int _iTPMarketLot; public int ITPMarketLot { get { return _iTPMarketLot; } set { _iTPMarketLot = value; } }
        private int _iTPTickSize; public int ITPTickSize { get { return _iTPTickSize; } set { _iTPTickSize = value; } }
        private int _iTPSequenceID; public int ITPSequenceID { get { return _iTPSequenceID; } set { _iTPSequenceID = value; } }

    }

    public class MCXMaster
    {
        private long _mCID; public long MCID { get { return _mCID; } set { _mCID = value; } }
        private long _mCSequenceId; public long MCSequenceId { get { return _mCSequenceId; } set { _mCSequenceId = value; } }
        private string _mCInstrumentName; public string MCInstrumentName { get { return _mCInstrumentName; } set { _mCInstrumentName = value; } }
        private long _mCToken; public long MCToken { get { return _mCToken; } set { _mCToken = value; } }
        private long _mCUnderlyingAsset; public long MCUnderlyingAsset { get { return _mCUnderlyingAsset; } set { _mCUnderlyingAsset = value; } }
        private string _mCContractCode; public string MCContractCode { get { return _mCContractCode; } set { _mCContractCode = value; } }
        private long _mCStrikePrice; public long MCStrikePrice { get { return _mCStrikePrice; } set { _mCStrikePrice = value; } }
        private string _mCOptionType; public string MCOptionType { get { return _mCOptionType; } set { _mCOptionType = value; } }
        private long _mCExpiryDate; public long MCExpiryDate { get { return _mCExpiryDate; } set { _mCExpiryDate = value; } }
        private string _mCDisplayExpiryDate; public string MCDisplayExpiryDate { get { return _mCDisplayExpiryDate; } set { _mCDisplayExpiryDate = value; } }
        private string _mCContractDescription; public string MCContractDescription { get { return _mCContractDescription; } set { _mCContractDescription = value; } }
        private long _mCQuotationUnit; public long MCQuotationUnit { get { return _mCQuotationUnit; } set { _mCQuotationUnit = value; } }
        private string _mCQuotationMetric; public string MCQuotationMetric { get { return _mCQuotationMetric; } set { _mCQuotationMetric = value; } }
        private long _mCBoardLot; public long MCBoardLot { get { return _mCBoardLot; } set { _mCBoardLot = value; } }
        private long _mCPriceTick; public long MCPriceTick { get { return _mCPriceTick; } set { _mCPriceTick = value; } }
        private string _mCPQFactor; public string MCPQFactor { get { return _mCPQFactor; } set { _mCPQFactor = value; } }
        private string _mCTradingUnit; public string MCTradingUnit { get { return _mCTradingUnit; } set { _mCTradingUnit = value; } }
        private decimal _mCGeneralNumerator; public decimal MCGeneralNumerator { get { return _mCGeneralNumerator; } set { _mCGeneralNumerator = value; } }
        private decimal _mCGeneralDenominator; public decimal MCGeneralDenominator { get { return _mCGeneralDenominator; } set { _mCGeneralDenominator = value; } }
        private string _mCField3; public string MCField3 { get { return _mCField3; } set { _mCField3 = value; } }

    }

    public class NCDEXMaster
    {
        private long _nCDID; public long NCDID { get { return _nCDID; } set { _nCDID = value; } }
        private long _nCDToken; public long NCDToken { get { return _nCDToken; } set { _nCDToken = value; } }
        private string _nCDInstrumentName; public string NCDInstrumentName { get { return _nCDInstrumentName; } set { _nCDInstrumentName = value; } }
        private string _nCDSymbol; public string NCDSymbol { get { return _nCDSymbol; } set { _nCDSymbol = value; } }
        private long _nCDExpiryDate; public long NCDExpiryDate { get { return _nCDExpiryDate; } set { _nCDExpiryDate = value; } }
        private string _nCDDisplayExpiryDate; public string NCDDisplayExpiryDate { get { return _nCDDisplayExpiryDate; } set { _nCDDisplayExpiryDate = value; } }
        private long _nCDStrikePrice; public long NCDStrikePrice { get { return _nCDStrikePrice; } set { _nCDStrikePrice = value; } }
        private string _nCDOptionType; public string NCDOptionType { get { return _nCDOptionType; } set { _nCDOptionType = value; } }
        private long _nCDBoardLotQuantity; public long NCDBoardLotQuantity { get { return _nCDBoardLotQuantity; } set { _nCDBoardLotQuantity = value; } }
        private long _nCDTickSize; public long NCDTickSize { get { return _nCDTickSize; } set { _nCDTickSize = value; } }
        private string _nCDName; public string NCDName { get { return _nCDName; } set { _nCDName = value; } }
        private long _nCDExerciseStartDate; public long NCDExerciseStartDate { get { return _nCDExerciseStartDate; } set { _nCDExerciseStartDate = value; } }
        private long _nCDExerciseEndDate; public long NCDExerciseEndDate { get { return _nCDExerciseEndDate; } set { _nCDExerciseEndDate = value; } }
        private long _nCDPriceNumerator; public long NCDPriceNumerator { get { return _nCDPriceNumerator; } set { _nCDPriceNumerator = value; } }
        private long _nCDPriceDenominator; public long NCDPriceDenominator { get { return _nCDPriceDenominator; } set { _nCDPriceDenominator = value; } }
        private string _nCDPriceUnit; public string NCDPriceUnit { get { return _nCDPriceUnit; } set { _nCDPriceUnit = value; } }
        private string _nCDQuantityUnit; public string NCDQuantityUnit { get { return _nCDQuantityUnit; } set { _nCDQuantityUnit = value; } }
        private string _nCDDeliveryUnit; public string NCDDeliveryUnit { get { return _nCDDeliveryUnit; } set { _nCDDeliveryUnit = value; } }
        private long _nCDDeliveryLotQuantity; public long NCDDeliveryLotQuantity { get { return _nCDDeliveryLotQuantity; } set { _nCDDeliveryLotQuantity = value; } }
        private long _nCDSequenceId; public long NCDSequenceId { get { return _nCDSequenceId; } set { _nCDSequenceId = value; } }
        private string _nCDTokenString; public string NCDTokenString { get { return _nCDTokenString; } set { _nCDTokenString = value; } }

    }

    public class ScripMasterSybas
    {
        //SYSBAS
        // 3-property Main-Structure, 3-Property Sub-Structure

        private Int32 _indexCode = 0;
        public Int32 IndexCode
        {
            get { return _indexCode; }
            set { _indexCode = value; }
        }

        private string _indexID = string.Empty;
        public string IndexID
        {
            get { return _indexID; }
            set { _indexID = value; }
        }

        private Int32 _noOfConstituents = 0;
        public Int32 NoOfConstituents
        {
            get { return _noOfConstituents; }
            set { _noOfConstituents = value; }
        }

        private Int32 _indexCodeSub = 0;
        public Int32 IndexCodeSub
        {
            get { return _indexCodeSub; }
            set { _indexCodeSub = value; }
        }

        private Int32 _scripCodeSyb = 0;
        public Int32 ScripCodeSyb
        {
            get { return _scripCodeSyb; }
            set { _scripCodeSyb = value; }
        }

        private Int64 _noOfShares = 0;
        public Int64 NoOfShares
        {
            get { return _noOfShares; }
            set { _noOfShares = value; }
        }
    }

    public class ScripMasterDebtInfo
    {
        //DEBTINFO_31052016
        private Int32 _scripCodeDebt = 0;
        public Int32 ScripCodeDebt
        {
            get { return _scripCodeDebt; }
            set { _scripCodeDebt = value; }
        }

        private string _isinNumber = string.Empty;
        public string ISINNumber
        {
            get { return _isinNumber; }
            set { _isinNumber = value; }
        }


        private string _typeOfOwnership = string.Empty;
        public string TypeOfOwnership
        {
            get { return _typeOfOwnership; }
            set { _typeOfOwnership = value; }
        }

        private string _natureOfBusiness = string.Empty;
        public string NatureOfBusiness
        {
            get { return _natureOfBusiness; }
            set { _natureOfBusiness = value; }
        }

        private Int64 _issuePrice = 0;
        public Int64 IssuePrice
        {
            get { return _issuePrice; }
            set { _issuePrice = value; }
        }

        private Int64 _faceValue = 0;
        public Int64 FaceValue
        {
            get { return _faceValue; }
            set { _faceValue = value; }
        }

        private Int64 _redemptionValue = 0;
        public Int64 RedemptionValue
        {
            get { return _redemptionValue; }
            set { _redemptionValue = value; }
        }

        private string _redemptionRecordDate = string.Empty;
        public string RedemptionRecordDate
        {
            get { return _redemptionRecordDate; }
            set { _redemptionRecordDate = value; }
        }

        private string _redemptionExDate = string.Empty;
        public string RedemptionExDate
        {
            get { return _redemptionExDate; }
            set { _redemptionExDate = value; }
        }

        private string _allotmentDateDDMMYYYY = string.Empty;
        public string AllotmentDateDDMMYYYY
        {
            get { return _allotmentDateDDMMYYYY; }
            set { _allotmentDateDDMMYYYY = value; }
        }

        private string _maturityConversionDateDDMMYYYY = string.Empty;
        public string MaturityConversionDateDDMMYYYY
        {
            get { return _maturityConversionDateDDMMYYYY; }
            set { _maturityConversionDateDDMMYYYY = value; }
        }

        private ulong _issueSize = 0;
        public ulong IssueSize
        {
            get { return _issueSize; }
            set { _issueSize = value; }
        }

        private string _creditAgencyandRating1 = string.Empty;
        public string CreditAgencyandRating1
        {
            get { return _creditAgencyandRating1; }
            set { _creditAgencyandRating1 = value; }
        }

        private string _creditAgencyandRating2 = string.Empty;
        public string CreditAgencyandRating2
        {
            get { return _creditAgencyandRating2; }
            set { _creditAgencyandRating2 = value; }
        }

        private string _creditAgencyandRating3 = string.Empty;
        public string CreditAgencyandRating3
        {
            get { return _creditAgencyandRating3; }
            set { _creditAgencyandRating3 = value; }
        }

        private string _typeofCoupon = string.Empty;
        public string TypeofCoupon
        {
            get { return _typeofCoupon; }
            set { _typeofCoupon = value; }
        }

        private string _couponRateInPerc = string.Empty;
        public string CouponRateINPerc
        {
            get { return _couponRateInPerc; }
            set { _couponRateInPerc = value; }
        }

        private string _couponType = string.Empty;
        public string CouponType
        {
            get { return _couponType; }
            set { _couponType = value; }
        }

        private string _couponFrequency = string.Empty;
        public string CouponFrequency
        {
            get { return _couponFrequency; }
            set { _couponFrequency = value; }
        }

        private string _dayCountConvention = string.Empty;
        public string DayCountConvention
        {
            get { return _dayCountConvention; }
            set { _dayCountConvention = value; }
        }

        private string _lastIntPaymentDate = string.Empty;
        public string LastIntPaymentDate
        {
            get { return _lastIntPaymentDate; }
            set { _lastIntPaymentDate = value; }
        }

        private string _nextIntPaymentDate = string.Empty;
        public string NextIntPaymentDate
        {
            get { return _nextIntPaymentDate; }
            set { _nextIntPaymentDate = value; }
        }

        private string _nextIntPaymentRec = string.Empty;
        public string NextIntPaymentRec
        {
            get { return _nextIntPaymentRec; }
            set { _nextIntPaymentRec = value; }
        }

        private string _nextIntPaymentEx = string.Empty;
        public string NextIntPaymentEx
        {
            get { return _nextIntPaymentEx; }
            set { _nextIntPaymentEx = value; }
        }

        private string _stepUpStepDown = string.Empty;
        public string StepUpStepDown
        {
            get { return _stepUpStepDown; }
            set { _stepUpStepDown = value; }
        }

        private string _putOptionFlagandDate = string.Empty;
        public string PutOptionFlagandDate
        {
            get { return _putOptionFlagandDate; }
            set { _putOptionFlagandDate = value; }
        }

        private string _putOptionFlagRecordDate = string.Empty;
        public string PutOptionFlagRecordDate
        {
            get { return _putOptionFlagRecordDate; }
            set { _putOptionFlagRecordDate = value; }
        }

        private string _putOptionFlagExDate = string.Empty;
        public string PutOptionFlagExDate
        {
            get { return _putOptionFlagExDate; }
            set { _putOptionFlagExDate = value; }
        }

        private string _callOptionFlagandDate = string.Empty;
        public string CallOptionFlagandDate
        {
            get { return _callOptionFlagandDate; }
            set { _callOptionFlagandDate = value; }
        }

        private string _callOptionFlagRecordDate = string.Empty;
        public string CallOptionFlagRecordDate
        {
            get { return _callOptionFlagRecordDate; }
            set { _callOptionFlagRecordDate = value; }
        }

        private string _callOptionFlagExDate = string.Empty;
        public string CallOptionFlagExDate
        {
            get { return _callOptionFlagExDate; }
            set { _callOptionFlagExDate = value; }
        }

        private string _exchangeListed = string.Empty;
        public string ExchangeListed
        {

            get { return _exchangeListed; }
            set { _exchangeListed = value; }
        }

        private string _seniority = string.Empty;
        public string Seniority
        {
            get { return _seniority; }
            set { _seniority = value; }
        }

        private string _redemption = string.Empty;
        public string Redemption
        {
            get { return _redemption; }
            set { _redemption = value; }
        }

        private string _modeofIssue = string.Empty;
        public string ModeofIssue
        {
            get { return _modeofIssue; }
            set { _modeofIssue = value; }
        }

        private string _securityStatus = string.Empty;
        public string SecurityStatus
        {
            get { return _securityStatus; }
            set { _securityStatus = value; }
        }

        private string _whetherTaxFree = string.Empty;
        public string WhetherTaxFree
        {
            get { return _whetherTaxFree; }
            set { _whetherTaxFree = value; }
        }

        private string _infraCategory = string.Empty;
        public string InfraCategory
        {
            get { return _infraCategory; }
            set { _infraCategory = value; }
        }

        private string _guaranteed = string.Empty;
        public string Guaranteed
        {
            get { return _guaranteed; }
            set { _guaranteed = value; }
        }

        private string _convertibility = string.Empty;
        public string Convertibility
        {
            get { return _convertibility; }
            set { _convertibility = value; }
        }

        private string _convertibilityRecordDate = string.Empty;
        public string ConvertibilityRecordDate
        {
            get { return _convertibilityRecordDate; }
            set { _convertibilityRecordDate = value; }
        }

        private string _convertibilityExDate = string.Empty;
        public string ConvertibilityExDate
        {
            get { return _convertibilityExDate; }
            set { _convertibilityExDate = value; }
        }

        private string _registrar = string.Empty;
        public string Registrar
        {
            get { return _registrar; }
            set { _registrar = value; }
        }

        private string _debentureTrustee = string.Empty;
        public string DebentureTrustee
        {
            get { return _debentureTrustee; }
            set { _debentureTrustee = value; }
        }

        private string _complianceCompanySecretary = string.Empty;
        public string ComplianceCompanySecretary
        {
            get { return _complianceCompanySecretary; }
            set { _complianceCompanySecretary = value; }
        }

        private string _arranger = string.Empty;
        public string Arranger
        {
            get { return _arranger; }
            set { _arranger = value; }
        }

        private string _remarks = string.Empty;
        public string Remarks
        {
            get { return _remarks; }
            set { _remarks = value; }
        }

        private string _accruedInterest = string.Empty;
        public string AccruedInterest
        {
            get { return _accruedInterest; }
            set { _accruedInterest = value; }
        }

        private string _tradeDate = string.Empty;
        public string TradeDate
        {
            get { return _tradeDate; }
            set { _tradeDate = value; }
        }

        private string _settelementDate = string.Empty;
        public string SettelementDate
        {
            get { return _settelementDate; }
            set { _settelementDate = value; }
        }


    }

    public class DP
    {
        //DPDDMMYY
        private Int32 _securityCode = 0;
        public Int32 SecurityCode
        {
            get { return _securityCode; }
            set { _securityCode = value; }
        }

        private char _circuitRelaxFlag = ' ';
        public char CircuitRelaxFlag
        {
            get { return _circuitRelaxFlag; }
            set { _circuitRelaxFlag = value; }
        }

        private Int64 _previousClosePrice = 0;
        public Int64 PreviousClosePrice
        {
            get { return _previousClosePrice; }
            set { _previousClosePrice = value; }
        }

        private Int32 _lowerCircuit = 0;
        public Int32 LowerCircuit
        {
            get { return _lowerCircuit; }
            set { _lowerCircuit = value; }
        }

        private Int32 _upperCircuit = 0;
        public Int32 UpperCircuit
        {
            get { return _upperCircuit; }
            set { _upperCircuit = value; }
        }

        private Int64 _lowerCircuitPrice = 0;
        public Int64 LowerCircuitPrice
        {
            get { return _lowerCircuitPrice; }
            set { _lowerCircuitPrice = value; }
        }

        private Int64 _upperCircuitPrice = 0;
        public Int64 UpperCircuitePrice
        {
            get { return _upperCircuitPrice; }
            set { _upperCircuitPrice = value; }
        }

        private Int32 _52weeksHighprice = 0;
        public Int32 WeeksHighprice
        {
            get { return _52weeksHighprice; }
            set { _52weeksHighprice = value; }
        }

        private Int32 _52weeksLowprice = 0;
        public Int32 WeeksLowprice
        {
            get { return _52weeksLowprice; }
            set { _52weeksLowprice = value; }
        }

        private string _Dateof52weeksHighprice = string.Empty;
        public string Dateof52weeksHighprice
        {
            get { return _Dateof52weeksHighprice; }
            set { _Dateof52weeksHighprice = value; }
        }

        private string _dateof52weeksLowprice = string.Empty;
        public string Dateof52weeksLowprice
        {
            get { return _dateof52weeksLowprice; }
            set { _dateof52weeksLowprice = value; }
        }

        private string _lastTradeDate = string.Empty;
        public string LastTradeDate
        {
            get { return _lastTradeDate; }
            set { _lastTradeDate = value; }
        }

        private Int32 _decimalLocator = 0;
        public Int32 DecimalLocator
        {
            get { return _decimalLocator; }
            set { _decimalLocator = value; }
        }

        private Int32 _filler2 = 0;
        public Int32 Filler2
        {
            get { return _filler2; }
            set { _filler2 = value; }
        }

        private Int32 _filler3 = 0;
        public Int32 Filler3
        {
            get { return _filler3; }
            set { _filler3 = value; }
        }

        private Int32 _filler4 = 0;
        public Int32 Filler4
        {
            get { return _filler4; }
            set { _filler4 = value; }
        }

    }

    public class ScripMasterSpnIndices
    {
        private long _indexCode;
        public long IndexCode
        {
            get { return _indexCode; }
            set { _indexCode = value; }
        }

        private string _existingShortName_ca;
        public string ExistingShortName_ca
        {
            get { return _existingShortName_ca; }
            set { _existingShortName_ca = value; }
        }

        private string _rebrandedLongName_ca;
        public string RebrandedLongName_ca
        {
            get { return _rebrandedLongName_ca; }
            set { _rebrandedLongName_ca = value; }
        }
    }

    public class SetlMas
    {
        private string _field1;
        public string Field1
        {
            get { return _field1; }
            set { _field1 = value; }
        }

        private string _fy;
        public string Fy
        {
            get { return _fy; }
            set { _fy = value; }
        }

        private string _field2;
        public string Field2
        {
            get { return _field2; }
            set { _field2 = value; }
        }

        private string _field3;
        public string Field3
        {
            get { return _field3; }
            set { _field3 = value; }
        }

        private string _field4;
        public string Field4
        {
            get { return _field4; }
            set { _field4 = value; }
        }
        private string _field5;
        public string Field5
        {
            get { return _field5; }
            set { _field5 = value; }
        }

        private string _field6;
        public string Field6
        {
            get { return _field6; }
            set { _field6 = value; }
        }

        private string _field7;
        public string Field7
        {
            get { return _field7; }
            set { _field7 = value; }
        }
        private string _field8;
        public string Field8
        {
            get { return _field8; }
            set { _field8 = value; }
        }

        private string _field9;

        public string Field9
        {
            get { return _field9; }
            set { _field9 = value; }
        }


    }

    public class CorporateAction
    {
        private long _scripCode;
        public long scripCode
        {
            get { return _scripCode; }
            set { _scripCode = value; }
        }

        private string _scripID;
        public string ScripID
        {
            get { return _scripID; }
            set { _scripID = value; }
        }

        private string _bookClosureFrom;
        public string bookClosureFrom
        {
            get { return _bookClosureFrom; }
            set { _bookClosureFrom = value; }
        }

        private string _bookClosureTo;
        public string bookClosureTo
        {
            get { return _bookClosureTo; }
            set { _bookClosureTo = value; }
        }

        private string _purposeOrEvent;
        public string purposeOrEvent
        {
            get { return _purposeOrEvent; }
            set { _purposeOrEvent = value; }
        }

        private string _exDate;
        public string exDate
        {
            get { return _exDate; }
            set { _exDate = value; }
        }

        private string _scripName;
        public string scripName
        {
            get { return _scripName; }
            set { _scripName = value; }
        }

        private string _bcOrRdFlag;
        public string bcOrRdFlag
        {
            get { return _bcOrRdFlag; }
            set { _bcOrRdFlag = value; }
        }

        private string _applicableFor;
        public string applicableFor
        {
            get { return _applicableFor; }
            set { _applicableFor = value; }
        }

        private string _NDStartSetlNumber;
        public string NDStartSetlNumber
        {
            get { return _NDStartSetlNumber; }
            set { _NDStartSetlNumber = value; }
        }

        private string _NDEndSetlNumber;
        public string NDEndSetlNumber
        {
            get { return _NDEndSetlNumber; }
            set { _NDEndSetlNumber = value; }
        }

        private string _deliverySettlement;
        public string deliverySettlement
        {
            get { return _deliverySettlement; }
            set { _deliverySettlement = value; }
        }

        private string _NdStartDate;
        public string NdStartDate
        {
            get { return _NdStartDate; }
            set { _NdStartDate = value; }
        }

        private string _ndEndDate;
        public string ndEndDate
        {
            get { return _ndEndDate; }
            set { _ndEndDate = value; }
        }

        private string _NdOrNDAndExFlag;
        public string NdOrNDAndExFlag
        {
            get { return _NdOrNDAndExFlag; }
            set { _NdOrNDAndExFlag = value; }
        }
        private string _ExDateD;
        public string ExDateD
        {
            get { return _ExDateD; }
            set { _ExDateD = value; }
        }

        private string _ExDateD1;
        public string ExDateD1
        {
            get { return _ExDateD1; }
            set { _ExDateD1 = value; }
        }

        private string _RecordDate;
        public string RecordDate
        {
            get { return _RecordDate; }
            set { _RecordDate = value; }
        }

        private string _ExDateB;

        public string ExDateB
        {
            get { return _ExDateB; }
            set { _ExDateB = value; }
        }

        private string _ExDateR;
        public string ExDateR
        {
            get { return _ExDateR; }
            set { _ExDateR = value; }
        }

        private string _Rating1;
        public string Rating1
        {
            get { return _Rating1; }
            set { _Rating1 = value; }
        }

        private string _Rating;
        public string Rating
        {
            get { return _Rating; }
            set { _Rating = value; }
        }

        private string _BCStartDate;
        public string BCStartDate
        {
            get { return _BCStartDate; }
            set { _BCStartDate = value; }
        }

        private string _BCEndDate;
        public string BCEndDate
        {
            get { return _BCEndDate; }
            set { _BCEndDate = value; }
        }

        private string _NDSDate;
        public string NDSDate
        {
            get { return _NDSDate; }
            set { _NDSDate = value; }
        }

        private string _NDEDate;
        public string NDEDate
        {
            get { return _NDEDate; }
            set { _NDEDate = value; }
        }
    }

    public class TouchlineWindow : INotifyPropertyChanged
    {
        private string _ScreenName;
        public string ScreenName
        {
            get { return _ScreenName; }
            set { _ScreenName = value; }
        }

        private string _FieldName;
        public string FieldName
        {
            get { return _FieldName; }
            set { _FieldName = value; }
        }

        private Int16 _DefaultColumns;
        public Int16 DefaultColumns
        {
            get { return _DefaultColumns; }
            set { _DefaultColumns = value; }
        }

        private Int16 _FEColumns;
        public Int16 FEColumns
        {
            get { return _FEColumns; }
            set { _FEColumns = value; }
        }

        private bool _LstColumnIsChecked;
        public bool LstColumnIsChecked
        {
            get { return _LstColumnIsChecked; }
            set
            {
                _LstColumnIsChecked = value;
                NotifyPropertyChanged("LstColumnIsChecked");
            }
        }

        private bool _IsLstColumnEnabled;
        public bool IsLstColumnEnabled
        {
            get { return _IsLstColumnEnabled; }
            set
            {
                _IsLstColumnEnabled = value;
                NotifyPropertyChanged("IsLstColumnEnabled");
            }
        }

        private string _FileName;
        public string FileName
        {
            get { return _FileName; }
            set
            {
                _FileName = value;
                NotifyPropertyChanged("FileName");
            }
        }

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
    }

    public static class StringExt
    {
        public static string Truncate(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }
    }

}
