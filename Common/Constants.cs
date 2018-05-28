using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonFrontEnd.Common
{
    public class Constants
    {
#if BOW
        public static string NewLineReplacementCharacter = "  N1e2w3L4i5n6eC7h8a9r0  ";
        public static int ADD_MODE = -1;
        public static string SUCCESS_FLAG = "0";
        public static string CUSTOM_DATE_FORMAT = "dd-MMM-yyyy";
        public static int ROW_COLUMN_NAME = 2;
        public static int ROW_DATA_START = 3;
        public static int AMOUNT_FACTOR = 10000;
        public static int PERCENTAGE_FACTOR = 10000;
        public static int Currency_Divisor = 10000;
        public static string AMOUNT_TAG = "Amount";
        public static string AMOUNT_OPTIONAL_TAG = "OptionalAmount";
        public static string PERCENTAGE_TAG = "Percentage";
        public static string NUMERIC_TAG = "Numeric";
        public static string ALL = "All";
        //SEGMENTS
        public static string SGT_EQUITY = "Equity";
        public static string SGT_FUTURES = "Futures";
        public static string SGT_OPTIONS = "Options";
        public static string SGT_COMM_CASH = "Commodities-Cash";
        public static string SGT_COMM_FUTURES = "Commodities-Futures";
        public static string SGT_COMM_OPTIONS = "Commodities-Options";
        public static string SGT_EQUITY_MF = "Mutual Funds";
        public static string SGT_EQUITY_BF = "";
        public static string SGT_EQUITY_BTST = "BTST";
        public static string SGT_FUTURES_DELIVERY = "Futures-Delivery";
        public static string SGT_OPTIONS_DELIVERY = "Options-Delivery";
        public static string SGT_COMM_CASH_DELIVERY = "Commodities-Cash-Delivery";
        public static string SGT_COMM_FUTURES_DELIVERY = "Commodities-Futures-Delivery";
        public static string SGT_COMM_OPTIONS_DELIVERY = "Commodities-Options-Delivery";


        //SEGMENTS VALUES
        public static string SGT_EQUITY_VALUE = "1";
        public static string SGT_FUTURES_VALUE = "2";
        public static string SGT_OPTIONS_VALUE = "3";
        public static string SGT_COMM_CASH_VALUE = "4";
        public static string SGT_COMM_FUTURES_VALUE = "5";
        public static string SGT_COMM_OPTIONS_VALUE = "6";
        public static string SGT_EQUITY_MF_VALUE = "7";
        public static string SGT_EQUITY_BF_VALUE = "8";
        public static string SGT_EQUITY_BTST_VALUE = "9";
        public static string SGT_FUTURES_DELIVERY_VALUE = "10";
        public static string SGT_OPTIONS_DELIVERY_VALUE = "11";
        public static string SGT_COMM_CASH_DELIVERY_VALUE = "12";
        public static string SGT_COMM_FUTURES_DELIVERY_VALUE = "13";
        public static string SGT_COMM_OPTIONS_DELIVERY_VALUE = "14";


        // EXCHANGES
        public static string EX_BSE = "BSE";
        public static string EX_NSE = "NSE";
        public static string EX_NCDEX = "NCDEX";
        public static string EX_NMCE = "NMCE";
        public static string EX_MCX = "MCX";
        public static string EX_DGCX = "DGCX";
        public static string EX_USE = "BSECDX";
        public static string EX_BSEINX = "BSEINX";

        // EXCHANGES VALUES
        public static int EX_BSE_VALUE = 1;
        public static int EX_NSE_VALUE = 2;
        public static int EX_NCDEX_VALUE = 3;
        public static int EX_NMCE_VALUE = 4;
        public static int EX_MCX_VALUE = 5;
        public static int EX_DGCX_VALUE = 8;
        //AGAR SERVER SIDE MAI KUCH NEHI FETAA TOO; USE EXCHANGE ID WILL BE DYNAMICALLY CHANGED FROM SERVER SIDE FROM 1 TO 7 
        public static int EX_USE_VALUE = 7;
        public static int EX_BSEINX_VALUE =9;

        // MARKETS
        public static string MKT_EQUITY = "Equity";
        public static string MKT_DERIVATIVE = "Derivative";
        public static string MKT_COMMODITIES = "Commodities";
        public static string MKT_CURRENCY = "Currency";
        public static string MKT_MF = "MF";
        public static string MKT_SLB = "SLB";
        public static string MKT_OFS = "OFS";
        public static string MKT_DEBT = "Debt";
        public static string MKT_DEBTT0 = "DebtT0";
        public static string MKT_ITP = "ITP";

        // MARKETS VALUES
        public static int MKT_EQUITY_VALUE = 1;
        public static int MKT_DERIVATIVE_VALUE = 2;
        public static int MKT_COMMODITIES_VALUE = 3;
        public static int MKT_CURRENCY_VALUE = 4;
        public static int MKT_MF_VALUE = 5;
        public static int MKT_SLB_VALUE = 6;
        public static int MKT_OFS_VALUE = 7;
        public static int MKT_DEBT_VALUE = 8;
        public static int MKT_DEBTT0_VALUE = 9;
        public static int MKT_ITP_VALUE = 10;

        public static string CTGRY_SECURITIES = "Securities";
        public static string CTGRY_COMMODITIES = "Commodities";

        public static string CTGRY_SECURITIES_VALUE = "1";
        public static string CTGRY_COMMODITIES_VALUE = "2";

        // BUY SELL 
        public static string BUY = "BUY";
        public static string SELL = "SELL";
        // BUY SELL VALUES
        public static string BUY_VALUE = "1";
        public static string SELL_VALUE = "2";

        // ORDER TYPE 
        public static string ORDER_TYPE_INTRADAY = "INTRADAY";
        public static string ORDER_TYPE_DELIVERY = "DELIVERY";
        public static string ORDER_TYPE_CNC = "CNC";
        public static string ORDER_TYPE_EXERCISE = "EXERCISE";
        // ORDER TYPE VALUE
        public static string ORDER_TYPE_INTRADAY_VALUE = "1";
        public static string ORDER_TYPE_DELIVERY_VALUE = "2";

        //ExcerciseType
        public static string EXERCISE_TYPE_DO = "Do";
        public static string EXERCISE_TYPE_DONT = "Don't";

        //ExcerciseType
        public static string EXERCISE_TYPE_DO_VALUE = "1";
        public static string EXERCISE_TYPE_DONT_VALUE = "0";

        // MARGIN TYPE
        public static string MARGIN_TYPE_BOARDLOTAMT = "AMOUNT/BOARD LOT";
        public static string MARGIN_TYPE_SINGLESECURITY = "AMOUNT FOR SINGLE SECURITY";
        public static string MARGIN_TYPE_PERCENTAGE = "PERCENTAGE";
        // MARGIN TYPE VALUES
        public static string MARGIN_TYPE_BOARDLOTAMT_VALUE = "1";
        public static string MARGIN_TYPE_SINGLESECURITY_VALUE = "2";
        public static string MARGIN_TYPE_PERCENTAGE_VALUE = "3";

        // INSTRUMENT TYPE
        public static string INSTRUMENT_TYPE_FUTURE_INDEX = "FUTIDX";
        public static string INSTRUMENT_TYPE_FUTURE_STOCK = "FUTSTK";
        public static string INSTRUMENT_TYPE_FUTURE_INT = "FUTINT";
        public static string INSTRUMENT_TYPE_FUTURE_IRD = "FUTIRD";
        public static string INSTRUMENT_TYPE_FUTURE_IRT = "FUTIRT";
        public static string INSTRUMENT_TYPE_FUTURE_CURRENCY = "FUTCUR";
        public static string INSTRUMENT_TYPE_SLB_FUTURE = "SLBINS";
        public static string INSTRUMENT_TYPE_WDM = "WDMINS";
        public static string INSTRUMENT_TYPE_OFS = "OFSINS";
        public static string INSTRUMENT_TYPE_ITP = "ITPINS";


        public static string INSTRUMENT_TYPE_OPTION_INDEX = "OPTIDX";
        public static string INSTRUMENT_TYPE_OPTION_STOCK = "OPTSTK";
        public static string INSTRUMENT_TYPE_OPTION_CURRENCY = "OPTCUR";

        public static string INSTRUMENT_TYPE_COMMODITY_CASH = "COMDTY";
        public static string INSTRUMENT_TYPE_COMMODITY_FUTURE = "FUTCOM";
        public static string INSTRUMENT_TYPE_COMMODITY_OPTION = "OPTCOM";

        // INSTRUMENT TYPE VALUE

        // Good Till
        public static string GOOD_TILL_DAYS = "GTDays";
        public static string GOOD_TILL_CANCEL = "GTC";
        public static string GOOD_TILL_DATE = "GTDate";
        public static string GOOD_FOR_A_DAY = "GFD";
        public static string GOOD_FOR_DAY = "DAY";
        public static string GOOD_TILL_TIME = "TIME";
        public static string CANCEL_ON_LOGOUT = "CancelOnLogout";

        public static string VERSION = "version";

        //USER-PROFILE LEVEL:
        public static string LEVEL_USER = "USER";
        public static string LEVEL_PROFILE = "PROFILE";

        //MARKET STATUS
        public static int STATUS_PREOPEN_VALUE = 1;
        public static int STATUS_OPENING_VALUE = 2;
        public static int STATUS_OPEN_VALUE = 3;
        public static int STATUS_CLOSE_VALUE = 4;
        public static int STATUS_POSTCLOSE_VALUE = 5;
        public static int STATUS_DAYEND_VALUE = 6;

        //MARKET STATUS VALUES
        public static string STATUS_PREOPEN = "PreOpen";
        public static string STATUS_OPENING = "Opening";
        public static string STATUS_OPEN = "Open";
        public static string STATUS_CLOSE = "Close";
        public static string STATUS_POSTCLOSE = "Post Close";
        public static string STATUS_DAYEND = "Day End";

        // Controls Type Name
        // 
        // button
        public static string TYPE_CHECK_BOX = "GUI.cmpCheckbox";
        public static string TYPE_COMBO_BOX = "GUI.cmpCombobox";
        public static string TYPE_RADIO_BUTTON = "GUI.cmpRadioButton";
        // form
        // label
        public static string TYPE_PANEL = "GUI.cmpPanel";
        public static string TYPE_TEXT_BOX = "GUI.cmpTextbox";
        public static string TYPE_AUTOCOMPLETE_TEXT_BOX = "GUI.AutoCompleteTextBox";
        //
        public static string TYPE_DATE_TIME_PICKER = "System.Windows.Forms.DateTimePicker";
        public static string TYPE_LIST_BOX = "System.Windows.Forms.ListBox";
        public static string TYPE_GROUP_BOX = "GUI.cmpGroupBox";
        // USED ONLY IN MW COLOR SELECTION FORM
        public static string TYPE_LABEL = "GUI.cmpLabel";
        // USED IN MODMAIN
        public static string TYPE_TAB_CONTROL = "GUI.cmpTabControl";
        public static string TYPE_TAB_PAGE = "GUI.cmpTabPage";
        // USED IN TRADING RIGHTS AND SECURITY
        public static string TYPE_FORM = "GUI.cmpForm";

        // For LocalDb
        //
        // Database Names
        public static string DATABASE_MASTERS = "Masters";
        public static string DATABASE_TRANSACTION = "Transactions";
        public static string DATABASE_SCREENS = "Screens";
        public static string DATABASE_USER_SCREEN_FIELDS = "UserScreenFields";
        public static string DATABASE_KEYMAP = "Keymap";
        public static string DATABASE_USER_ALERTS = "Alerts";
        // Table Names
        // Masters.db
        public static string TABLE_SECURITIES = "Securities";
        public static string TABLE_CONTRACTS = "Contracts";
        public static string TABLE_NCDEXCONTRACTS = "NCDEXContracts";
        public static string TABLE_NMCECONTRACTS = "NMCEContracts";
        public static string TABLE_MCXCONTRACTS = "MCXContracts";
        public static string TABLE_GBOTCONTRACTS = "GBOTContracts";
        public static string TABLE_BSECONTRACTS = "BSEContracts";
        public static string TABLE_NSECURRENCY_CONTRACTS = "NseCurrencyContracts";
        public static string TABLE_BSECURRENCY_CONTRACTS = "BSECurrencyContracts";
        public static string TABLE_MCXCURRENCY_CONTRACTS = "MCXCurrencyContracts";
        public static string TABLE_BSEINX_CONTRACTS = "BSEINXContracts";

        //Search Forms
        public static string SECURITIES = "0";
        public static string CONTRACTS = "1";
        public static string NCDEXCONTRACTS = "2";
        public static string NMCECONTRACTS = "3";
        public static string MCXCONTRACTS = "4";
        public static string NSECURRENCYCONTRACTS = "5";

        // Transaction.db
        public static string TABLE_ORDERS = "Orders";
        public static string TABLE_MyOrders = "MyOrders";
        public static string TABLE_TRADES = "Trades";
        public static string TABLE_MyAvgTrades = "MyAvgTrades";
        public static string TABLE_MARGIN_POSITIONS = "MarginPositions";
        public static string TABLE_ORDER_TRADE_MESSAGES = "OrderTradeMessages";
        public static string TABLE_USER_ALERT_MESSAGES = "UserAlertMessages";
        public static string TABLE_USER_DEPOSITS = "UserDeposits";
        public static string TABLE_START_OF_DAY_POSITIONS = "StartOfDayPositions";
        public static string TABLE_BROADCAST_MESSAGES = "BroadcastMessages";
        public static string TABLE_USERS = "Users";
        public static string TABLE_SECURITY_BY_MINUTE = "SecurityByMinute";
        public static string TABLE_QUOTE_MASTER = "QuoteMaster";
        public static string TABLE_QUOTE_TRANSACTION = "QuoteTransaction";
        public static string VIEW_VVSTRATEGYDETAILS = "VVStrategyDetails";

        public static string VIEW_VVNETPOSITON = "VVNetPosition";
        public static string VIEW_VVSCRIPTWISENETPOSITION = "VVScriptWiseNetPosition";
        public static string VIEW_VVCLIENTWISENETPOSITION = "VVClientWiseNetPosition";
        public static string VIEW_VVNETPOSITON_ID = "86";
        public static string VIEW_VVSCRIPTWISENETPOSITION_ID = "139";
        public static string VIEW_VVCLIENTWISENETPOSITION_ID = "144";
        public static string VIEW_DAYS_POSITION = "1005";
        public static string PROCEDURE_CREATE_TRADE_FILE_PRO = "PRC_PRO_CREATE_TRADE_FILE_AXIS";
        public static string PROCEDURE_CREATE_TRADE_FILE_ALL = "PRC_ALL_CREATE_TRADE_FILE_AXIS";
        public static string FUNCTION_IS_USE_SPREAD_SYMBOL = "FN_IsUSESpreadSymbol";

        // Screens.db
        public static string TABLE_SCREENS = "Screens";
        public static string TABLE_SCREEN_FIELDS = "ScreenFields";
        // UserScreenFields.db
        public static string TABLE_USER_SCREEN_FIELDS = "UserScreenFields";
        public static string TABLE_USER_COLORINFO_FIELDS = "ColorInfo";
        public static string TABLE_FORMULA_MASTER = "FormulaMaster";


        // For DeCompression in HTTP and Socket Connection
        public static int NO_OF_BYTES_TO_READ = 4;
        public static string COMPRESSED_PARAMETER = "Compress";
        public const string HTTP_COMPRESSED = "HTTPCOMPRESSED";
        public const string USE_SECURE_HTTP = "USESECUREHTTP";
        public const string SOCKET_COMPRESSED = "SOCKETCOMPRESSED";
        public const string COMBINED_MBP = "COMBINEDMBP";
        public static string BOOLEAN_TRUE_STRING = "TRUE";
        public static string BOOLEAN_FALSE_STRING = "FALSE";
        public static string YESNO_Y = "Y";
        public static string YESNO_N = "N";
        // For HTTP TimeOut
        public static int HTTP_TIMEOUT_MIN = 30000;

        // For Unzipping of Files - its a 32bit exe
        public static string PKUNZIP_FILE_NAME = "PKZIP25";
        public static string PKUNIZP_UNZIP_PARAMETER = "-extract";

        // For Charts
        public const string SHOW_CHARTS = "CHARTS";

        //For Arbitrage
        public const string SHOW_ARBITRAGE = "ARBITRAGE";

        public static string ARBITRAGE_ORDERPRICE_TYPE_LTP = "LTP";
        public static string ARBITRAGE_ORDERPRICE_TYPE_BESTBUYBESTSELL = "Best Buy/Best Sell";
        public static string ARBITRAGE_ORDERVOLUME_TYPE_FIXED = "Fixed Value";
        public static string ARBITRAGE_ORDERVOLUME_TYPE_MINOFBUYSELL = "Min of Buy/Sell";

        public static int RECONNECT_DISABLE_TIME = 5000;
        public static int USER_LOGGED_FROM_ELSEWHERE_WAIT_TIME_BEFORE_DISCONNECTING = 5000;

        // Arrays for NSE and NCDEX Instrument types...
        public static string[] NCDEX_INSTRUMENT_TYPES = { "COMDTY", "FUTCOM", "OPTCOM" };
        public static string[] NSE_INSTRUMENT_TYPES = { "OPTIDX", "OPTSTK", "FUTINT", "FUTSTK", "FUTIDX", "FUTCUR" };



        // FOR BSE
        public static string EOTODY = "EOTODY";
        public static string EOSESS = "EOSESS";
        public static string EOSTLM = "EOSTLM";

        //Password Expiry
        public static string PasswordExpiryIntimation = "15";
        public static string PasswordExpired = "16";
        public static string FirstTimeLogIn = "14";

        //VALUE
        public static string EOTODY_VALUE = "2";
        public static string EOSESS_VALUE = "1";
        public static string EOSTLM_VALUE = "3";

        public static string BANK = "BANK";
        public static string DFI = "DFI";
        public static string FII = "FII";
        public static string MF = "MF";
        public static string CLIENT = "CLIENT";
        public static string OWN = "OWN";
        public static string INSTITUTION = "INST";
        public static string NRI = "NRI";
        public static string QFI = "QFI";
        public static string HNI = "HNI";
        public static string BCO = "BCO";
        public static string OTHERS = "OTHERS";
        public static string SPLCLI = "SPLCLI";


        public static string BANK_VALUE = "50";
        public static string DFI_VALUE = "60";
        public static string FII_VALUE = "80";
        public static string MF_VALUE = "70";
        public static string CLIENT_VALUE = "30";
        public static string OWN_VALUE = "20";

        //NMCE

        public static string ORDER_TYPE_LIMIT = "LIMIT";
        public static string ORDER_TYPE_PF_CONV = "MARKET";
        public static string ORDER_TYPE_PF_KILL = "IOC";
        public static string ORDER_TYPE_STOPLOSS_CONV = "STOP LOSS-CONV";
        public static string ORDER_TYPE_STOPLOSS_NONCONV = "STOP LOSS-NONCONV";
        public static string ORDER_TYPE_RL = "Regular Lot";
        public static string ORDER_TYPE_SL = "Stop Loss";
        public static string ORDER_TYPE_OCO = "OCO";
        public static string ORDER_TYPE_BOC = "BOC";
        public static string ORDER_TYPE_BracketOrder = "Bracket Order";
        public static string ORDER_TYPE_RollOver = "RollOver";

        public static string ORDER_TYPE_RL_VALUE = "1";
        public static string ORDER_TYPE_SL_VALUE = "3";
        public static string ORDER_TYPE_OCO_VALUE = "8";
        public static string ORDER_TYPE_BOC_VALUE = "7";
        public static string ORDER_TYPE_BlockDeal_VALUE = "9";

        public static string ORDER_TYPE_LIMIT_VALUE = "1";
        public static string ORDER_TYPE_PF_CONV_VALUE = "2";
        public static string ORDER_TYPE_PF_KILL_VALUE = "3";
        public static string ORDER_TYPE_STOPLOSS_CON_VALUE = "4";
        public static string ORDER_TYPE_STOPLOSS_NONCONV_VALUE = "5";
        public static string ORDER_TYPE_BRACKETORDER_VALUE = "6";
        public static string ORDER_TYPE_ODDLOT_VALUE = "15";
        public static string ORDER_TYPE_ODDLOTGrab_VALUE = "16";
        public static string ORDER_TYPE_RollOver_VALUE = "8";


        public static string RETENTION_GTD = "GTD";
        public static string RETENTION_GTC = "GTC";
        public static string RETENTION_GFD = "GFD";

        public static string PARAMETER_THICK_CLIENT = "Thick Client";
        public static string PARAMETER_THICK_CLIENT_VALUE = "Y";
        public static string PARAMETER_LOGIN_KEY = "LoginKey";
        public static string INTERACTIVE_FONT = "Microsoft Sans Serif,8,1";
        public static string INDICES_FONT = "Verdana,12,1";

        public static string BUYConfirmMsgColor = "-16776961";
        public static string SELLConfirmMsgColor = "-65536";
        public static string BUYTradeMsgColor = "-16776961";
        public static string SELLTradeMsgColor = "-65536";
        public static string BUYBackGroundColor = "-1";
        public static string SELLBackGroundColor = "-1";

        public static string MBPBUYColor = "-16776961";
        public static string MBPSELLColor = "-65536";
        public static string MBPBUYBackGroundColor = "-1";
        public static string MBPSELLBackGroundColor = "-1";


        //Condition Operators for CompareConditions.
        public static string GreatorThen = ">";
        public static string SmallerThen = "<";
        public static string GreatorThenEqualTo = ">=";
        public static string SmallerThenEqualTo = "<=";
        public static string Equal = "=";

        // Bank Status
        public static string USER_BANKDETAILS_STATUS_Active = "ACTIVE";
        public static string USER_BANKDETAILS_STATUS_InActive = "INACTIVE";
        public static string USER_BANKDETAILS_STATUS_Suspended = "SUSPENDED";

        //FasTrade Vender Codes
        public static string BSE_VENDOR_CODE = "12";
        public static string BOLTPlus_VENDOR_CODE = "98";
        public static string NSE_VENDOR_CODE = "11";
        public static string NSE_FOW_VENDOR_CODE = "16";
        public static string USE_VENDOR_CODE = "01";
        public static string MCX_VENDOR_CODE = "06";
        public static string MCX_VENDOR_CODE_BOLTPlus = "13";
        public static string NCDEX_VENDOR_CODE = "105";


        //BSE SECURITIES-GSM, BY KIRAN(16-MAR-17)
        public static string BSE_GSM_NA = "100";
        public static string BSE_GSM_STAGE0 = "0";

        //NSE SECURITIES-GSM, BY KIRAN(12-APR-17)
        public static string NSE_GSM_NA = "0";
        public static string NSE_GSM_STAGE0 = "99";


        public enum ProductInfo
        {
            None = 0,
            BoltPlusOnWeb = 1,
            BoltPlus = 2,
            BoB = 3,
            Fastrade = 4
        }

        public enum KeyMaaping
        {
            Type1 = 1, //1 stand for BOW
            Type2 = 2, //2 stand for Bolt plus
            Type3 = 3, // 3 stand for odin
            Type4 = 4, //4 stand for now
        }


        public enum TransactionModeIndex
        {
            CDSLDemat = 0,
            NSDLDemat = 1,
            Physical = 2,
        }

        public enum SIPType
        {
            SIP = 1,
            XSIP = 2,
        }
#elif TWS
   

#endif
    }

}
