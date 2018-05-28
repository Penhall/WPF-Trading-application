using CommonFrontEnd;
using CommonFrontEnd.Common;
using CommonFrontEnd.Utility.Entity;
using CommonFrontEnd.Utility.Factory;
using CommonFrontEnd.Utility.Interfaces_Abstract;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonFrontEnd.Utility
{
    [Serializable()]
    public class UtilityScript
    {

        //    public Hashtable gobjIndicesByName;
        //    public Hashtable gobjIndicesById;

        //    public Dictionary<string, Dictionary<long, long>> gobjSpreadScripIDTokenMap;

           public static MyExpiryDatesHash mobjMyExpiryDates = new MyExpiryDatesHash();
        //    //:
         private Hashtable mobjSecureServletList;
        //    //: For Smart Search
        //    private static MySymbols mobjMySymbols = new MySymbols();
        //    private string[] mobjBseScripCodes;
        //    private static Dictionary<string, string[]> mobjInstrumentTypes;

        private static Dictionary<string, IScript> mobjFrequentlyUsedScrips = new Dictionary<string, IScript>();
           private static UtilityScript mobjInstance;
        //    private static object lockObject = new object();
        //    public bool MastersDownloadInprocess
        //    {
        //        get { return GetScripFactory.MastersDownloadInProcess; }
        //        set { GetScripFactory.MastersDownloadInProcess = value; }
        //    }
        //    public bool IsMastersConnected { get; set; }
        //    /// <summary>
        //    /// Get Product Information From Master for which marster has been generated
        //    /// </summary>
        //    /// <value></value>
        //    /// <returns>return a product code</returns>
        //    /// <remarks></remarks>
        //    //public Constants.ProductInfo ProductInfoFromMaster
        //    //{
        //    //    get { return GetScripFactory.GetProductInfo(); }
        //    //}
        //    //: Do not delete this as we are creating a Singleton Object.
        private UtilityScript()
        {
        }
        public static UtilityScript GetInstance
        {
            get
            {
                if (mobjInstance == null)
                {
                    mobjInstance = new UtilityScript();
                }
                return mobjInstance;
            }
        }

        //public void InitializeCollections()
        //{
        //    mobjMyExpiryDates = new MyExpiryDatesHash();
        //    mobjMySymbols = new MySymbols();
        //    mobjFrequentlyUsedScrips = new Dictionary<string, IScript>();
        //    mobjInstrumentTypes = new Dictionary<string, string[]>();
        //    gobjSpreadScripIDTokenMap = new Dictionary<string, Dictionary<long, long>>();
        //    GetScripFactory.CacheTableNames();
        //}

        //    public static IDbConnection MastersConnection
        //    {
        //        get { return GetScripFactory.Connection; }
        //    }

        public Hashtable SecureServlets
        {
            get { return mobjSecureServletList; }
        }

        //    public string[] BseScriptCodes
        //    {
        //        get
        //        {
        //            if (mobjBseScripCodes == null || mobjBseScripCodes.Length == 0)
        //            {
        //                mobjBseScripCodes = GetBseScripCodes();
        //            }
        //            return mobjBseScripCodes;
        //        }
        //    }


        //    private MyExMktKey GetExMktKey(string pstrExchangeId, string pstrMarketId)
        //    {
        //        if (pstrExchangeId == null || pstrExchangeId.Trim().Length == 0)
        //            return null;
        //        if (pstrMarketId == null || pstrMarketId.Trim().Length == 0)
        //            return null;

        //        MyExMktKey lobjExMktKey = new MyExMktKey();
        //        lobjExMktKey.ExchangeId = int.Parse(pstrExchangeId);
        //        lobjExMktKey.MarketId = int.Parse(pstrMarketId);

        //        return lobjExMktKey;
        //    }


        //    #region " Connect and Disconnect Functions"
        //    public bool ConnectMasters()
        //    {
        //        return GetScripFactory.OpenConnection();
        //    }
        //    public bool DisConnectMasters()
        //    {
        //        return GetScripFactory.CloseConnection();
        //    }
        //    #endregion

        //    #region " Security "
        //    private bool ValidateSecurity(string pstrSymbol, string pstrSeries, int pintExchangeId)
        //    {
        //        IScript lobjSecurity = GetScript(pintExchangeId, (Constants.MKT_EQUITY_VALUE), pstrSymbol, pstrSeries);
        //        if ((lobjSecurity == null))
        //        {
        //            return false;
        //        }
        //        else
        //        {
        //            return true;
        //        }
        //    }
        //    /// -----------------------------------------------------------------------------
        //    /// <summary>
        //    /// The function fetches security details of NSE/BSE Securities on basis of Symbol, Series or Exchange.
        //    /// The data is fetched from SECURITIES table in the Masters database.
        //    /// </summary>
        //    /// <param name="pstrSymbol">
        //    /// The Symbol for which the security details are to be retrieved.
        //    /// </param>
        //    /// <param name="pstrSeries">
        //    /// The Series code for which the security details are to be retrieved.
        //    /// </param>
        //    /// <param name="pintExchange">
        //    /// The Exchange Name corresponding to which the security is to be retrieved.
        //    /// </param>
        //    /// <returns>
        //    /// Returns an object of type Security which contains details of NSE BSE Security.
        //    /// </returns>
        //    /// <remarks>
        //    /// The database connectivity could have been stored in a seperate file and called by 
        //    /// both this function and the following function thus reducing the code redundency as both r same.
        //    /// </remarks>
        //    /// <history>
        //    ///     [pranav.sah]    12/13/2006    Created
        //    /// </history>
        //    /// -----------------------------------------------------------------------------
        //    private SecurityBase GetSecurity(string pstrSymbol, string pstrSeries, int pintExchangeID)
        //    {
        //        IScript lobjSecurity = GetScript(pintExchangeID, (Constants.MKT_EQUITY_VALUE), pstrSymbol, pstrSeries);
        //        if ((lobjSecurity != null))
        //        {
        //            return (SecurityBase)lobjSecurity;
        //        }
        //        else
        //        {
        //            return null;
        //        }
        //    }
        //    private ScriptCollection GetSecurities(int pintExchange, int pintMarketId)
        //    {
        //        return GetScrips(pintExchange, pintMarketId);
        //    }
        //    /// -----------------------------------------------------------------------------
        //    /// <summary>
        //    /// The function fetches security details for NSE/BSE from the SECURITIES table in the Masters database, based on SECURITY ID which is a common token for security in both exchanges.
        //    /// </summary>
        //    /// <param name="plngSecurityId">
        //    /// Common Token for the Security to be retrieved.
        //    /// </param>
        //    /// <returns>
        //    /// Returns an object of type Security.
        //    /// </returns>
        //    /// <remarks>
        //    /// The database connectivity could have been stored in a seperate file and called by 
        //    /// both the above function and this function thus reducing the code redundency as both r same.
        //    /// </remarks>
        //    /// <history>
        //    ///     [pranav.sah]    12/13/2006    Created
        //    /// </history>
        //    /// -----------------------------------------------------------------------------
        //    private SecurityBase GetSecurity(long plngSecurityId)
        //    {
        //        IScript lobjIScrip = GetScript( (Constants.EX_BSE_VALUE), plngSecurityId,  (Constants.MKT_EQUITY_VALUE));
        //        if (lobjIScrip == null)
        //        {
        //            lobjIScrip = GetScript( (Constants.EX_NSE_VALUE), plngSecurityId,  (Constants.MKT_EQUITY_VALUE));
        //        }

        //        if ((lobjIScrip != null))
        //        {
        //            return (SecurityBase)lobjIScrip;
        //        }
        //        else
        //        {
        //            return null;
        //        }
        //    }

        //    #endregion

        //    #region " Contract "

        //    //Public Function GetContractDetail(ByVal plngTokenId As Long, ByVal lintFieldID As Int16) As String
        //    // Dim llngSecurityId As Long = plngTokenId
        //    // Dim lstrSFName As String = ""
        //    // Dim lobjSQLLiteDataReader As Finisar.SQLite.SQLiteDataReader = Nothing
        //    // Dim lobjSQLLiteCommand As Finisar.SQLite.SQLiteCommand
        //    // Dim lstrQuery As String
        //    // Try
        //    // If ConnectToSQLLiteMasters() = True Then
        //    // SyncLock lockObject
        //    // lstrQuery = "SELECT " & getDisplayName(lintFieldID) & " FROM Contracts WHERE " & Constants.SecurityBean.f_Id & " = " & llngSecurityId
        //    // lobjSQLLiteCommand = mobjSQLLiteMastersCon.CreateCommand()
        //    // lobjSQLLiteCommand.CommandType = CommandType.Text
        //    // lobjSQLLiteCommand.CommandText = lstrQuery
        //    // lobjSQLLiteDataReader = CType(lobjSQLLiteCommand.ExecuteReader(), Finisar.SQLite.SQLiteDataReader)
        //    // REM: Moving through records
        //    // REM: Only One record is to be returned
        //    // If lobjSQLLiteDataReader.Read() = True Then
        //    // Return lobjSQLLiteDataReader.Item(0)
        //    // Else
        //    // Throw New Exception("NO " & lstrSFName & " Found in Securities Table where SCID = " & llngSecurityId.ToString)
        //    // End If
        //    // End SyncLock
        //    // End If
        //    // Catch ex As Exception
        //    // Throw New Exception("Error While Finding " & lstrSFName & " in Securities Table where SCID = " & llngSecurityId.ToString & "....", ex)
        //    // Finally
        //    // If Not lobjSQLLiteDataReader Is Nothing Then
        //    // lobjSQLLiteDataReader.Close()
        //    // lobjSQLLiteDataReader = Nothing
        //    // End If
        //    // lobjSQLLiteCommand = Nothing
        //    // End Try
        //    // Return Nothing
        //    //End Function

        //    public StringBuilder GetStraMFContracts(ref StringBuilder lstrStringBuilder, string pstrSchemeCode, string pstrAMCCode, string pstrSchemeName, string pstrSchemeType)
        //    {

        //        MutualFund lobjScrip = default(MutualFund);
        //        ScriptCollection lobjScripColl = default(ScriptCollection);
        //        ExchangeColumnName_Map lobjExchangeColumnMap = ColumnNameFactory.GetExchangeColumnMap( (Constants.EX_BSE_VALUE),  (Constants.MKT_MF_VALUE));
        //        StringBuilder lobjWhereClause = new StringBuilder();

        //        lobjWhereClause.Append(" 2 = 2 ");
        //        if (string.IsNullOrWhiteSpace(pstrSchemeCode) == false)
        //        {
        //            lobjWhereClause.Append(string.Format(" and {0} like '{1}%'", lobjExchangeColumnMap.ColumnName_Map.Where(x=>x.Key==BaseColumnNames.Symbol).Select(x=>x.Value), pstrSchemeCode));
        //        }
        //        if (string.IsNullOrWhiteSpace(pstrSchemeName) == false)
        //        {
        //            lobjWhereClause.Append(string.Format(" and {0} like '{1}%'", lobjExchangeColumnMap.ColumnName_Map.Where(x => x.Key == BaseColumnNames.Name).Select(x => x.Value), pstrSchemeName));
        //        }
        //        if (string.IsNullOrWhiteSpace(pstrAMCCode) == false)
        //        {
        //            lobjWhereClause.Append(string.Format(" and {0} like '{1}%'", "AMCCode", pstrAMCCode));
        //        }
        //        if (string.IsNullOrWhiteSpace(pstrSchemeType) == false)
        //        {
        //            lobjWhereClause.Append(string.Format(" and {0} like '{1}%'", "SchemeType", pstrSchemeType));
        //        }

        //        lobjScripColl = GetScripFactory.GetScrips( (Constants.EX_BSE_VALUE),  (Constants.MKT_MF_VALUE), lobjWhereClause.ToString());
        //        if ((lobjScripColl != null))
        //        {
        //            for (int lintTemp = 0; lintTemp <= lobjScripColl.Count - 1; lintTemp++)
        //            {
        //                lobjScrip = (MutualFund)lobjScripColl[lintTemp];

        //                // "UniqueNo string,SchemeCode string,RTASchemeCode string,AMCSchemeCode string,ISIN string,AMCCode string,
        //                lstrStringBuilder.Append(lobjScrip.Token);
        //                lstrStringBuilder.Append("|");
        //                //SYMBOL
        //                if (!(string.IsNullOrWhiteSpace(lobjScrip.Symbol) == true))
        //                {
        //                    lstrStringBuilder.Append(lobjScrip.Symbol);
        //                }
        //                else
        //                {
        //                    lstrStringBuilder.Append("");
        //                }
        //                lstrStringBuilder.Append("|");
        //                //SERIES
        //                if (!(string.IsNullOrWhiteSpace(lobjScrip.RTASchemeCode) == true))
        //                {
        //                    lstrStringBuilder.Append(lobjScrip.RTASchemeCode);
        //                }
        //                else
        //                {
        //                    lstrStringBuilder.Append("");
        //                }
        //                lstrStringBuilder.Append("|");
        //                //COMPANYNAME
        //                if (!(string.IsNullOrWhiteSpace(lobjScrip.AMCSchemeCode) == true))
        //                {
        //                    lstrStringBuilder.Append(lobjScrip.AMCSchemeCode);
        //                }
        //                else
        //                {
        //                    lstrStringBuilder.Append("");
        //                }
        //                lstrStringBuilder.Append("|");
        //                //BSETOKEN
        //                if (!(string.IsNullOrWhiteSpace(lobjScrip.ISIN) == true))
        //                {
        //                    lstrStringBuilder.Append(lobjScrip.ISIN);
        //                }
        //                else
        //                {
        //                    lstrStringBuilder.Append("");
        //                }
        //                lstrStringBuilder.Append("|");

        //                //NSETOKEN
        //                if (!(string.IsNullOrWhiteSpace(lobjScrip.AMCCode) == true))
        //                {
        //                    lstrStringBuilder.Append(lobjScrip.AMCCode);
        //                }
        //                else
        //                {
        //                    lstrStringBuilder.Append("");
        //                }
        //                lstrStringBuilder.Append("|");

        //                //SchemeName string,PurchaseTransactionmode string,MinimumPurchaseAmount numeric,AdditionalPurchaseAmountMultiple numeric, 
        //                //BSEBOARDLOTQUANTITY
        //                if (!(string.IsNullOrWhiteSpace(lobjScrip.Name) == true))
        //                {
        //                    lstrStringBuilder.Append(lobjScrip.Name);
        //                }
        //                else
        //                {
        //                    lstrStringBuilder.Append("");
        //                }
        //                lstrStringBuilder.Append("|");

        //                //NSEBOARDLOTQUANTITY

        //                if (!(string.IsNullOrWhiteSpace(lobjScrip.PurchaseTransactionmode) == true))
        //                {
        //                    lstrStringBuilder.Append(lobjScrip.PurchaseTransactionmode);
        //                }
        //                else
        //                {
        //                    lstrStringBuilder.Append("");
        //                }
        //                lstrStringBuilder.Append("|");
        //                //BSETICKSIZE
        //                if (!(string.IsNullOrWhiteSpace(lobjScrip.MinimumPurchaseAmount) == true))
        //                {
        //                    lstrStringBuilder.Append(Strings.FormatNumber(lobjScrip.MinimumPurchaseAmount, 4));
        //                }
        //                else
        //                {
        //                    lstrStringBuilder.Append("");
        //                }
        //                lstrStringBuilder.Append("|");
        //                //NSETICKSIZE
        //                if (!(string.IsNullOrWhiteSpace(lobjScrip.AdditionalPurchaseAmountMultiple) == true))
        //                {
        //                    lstrStringBuilder.Append(Strings.FormatNumber(lobjScrip.AdditionalPurchaseAmountMultiple, 4));
        //                }
        //                else
        //                {
        //                    lstrStringBuilder.Append("");
        //                }
        //                lstrStringBuilder.Append("|");

        //                //MaximumPurchaseAmount numeric,PurchaseAllowed string,PurchaseCutoffTime string,RedemptionTransactionMode string,MinimumRedemptionQty numeric, 
        //                //BSESCRIPTID
        //                if (!(string.IsNullOrWhiteSpace(lobjScrip.MaximumPurchaseAmount) == true))
        //                {
        //                    lstrStringBuilder.Append(Strings.FormatNumber(lobjScrip.MaximumPurchaseAmount, 4));
        //                }
        //                else
        //                {
        //                    lstrStringBuilder.Append("");
        //                }
        //                lstrStringBuilder.Append("|");

        //                //BSESCRIPTNAME
        //                if (!(string.IsNullOrWhiteSpace(lobjScrip.PurchaseAllowed) == true))
        //                {
        //                    lstrStringBuilder.Append(lobjScrip.PurchaseAllowed);
        //                }
        //                else
        //                {
        //                    lstrStringBuilder.Append("");
        //                }
        //                lstrStringBuilder.Append("|");
        //                //BSEGROUPNAME
        //                if (!(string.IsNullOrWhiteSpace(lobjScrip.PurchaseCutoffTime) == true))
        //                {
        //                    lstrStringBuilder.Append(lobjScrip.PurchaseCutoffTime);
        //                }
        //                else
        //                {
        //                    lstrStringBuilder.Append("");
        //                }

        //                lstrStringBuilder.Append("|");
        //                //BSEGROUPNAME
        //                if (!(string.IsNullOrWhiteSpace(lobjScrip.RedemptionTransactionMode) == true))
        //                {
        //                    lstrStringBuilder.Append(lobjScrip.RedemptionTransactionMode);
        //                }
        //                else
        //                {
        //                    lstrStringBuilder.Append("");
        //                }
        //                lstrStringBuilder.Append("|");
        //                //BSEGROUPNAME
        //                if (!(string.IsNullOrWhiteSpace(lobjScrip.MinimumRedemptionQty) == true))
        //                {
        //                    lstrStringBuilder.Append(Strings.FormatNumber(lobjScrip.MinimumRedemptionQty, 4));
        //                }
        //                else
        //                {
        //                    lstrStringBuilder.Append("");
        //                }

        //                lstrStringBuilder.Append("|");
        //                //RedemptionQtyMultiplier numeric, MaximumRedemptionQty string,RedemptionAllowed string,RedemptionCutoffTime string,RTAAgentCode string,AMCActiveFlag INT, 
        //                //BSEGROUPNAME
        //                if (!(string.IsNullOrWhiteSpace(lobjScrip.RedemptionQtyMultiplier) == true))
        //                {
        //                    lstrStringBuilder.Append(Strings.FormatNumber(lobjScrip.RedemptionQtyMultiplier, 4));
        //                }
        //                else
        //                {
        //                    lstrStringBuilder.Append("");
        //                }


        //                lstrStringBuilder.Append("|");
        //                //BSEGROUPNAME
        //                if (!(string.IsNullOrWhiteSpace(lobjScrip.MaximumRedemptionQty) == true))
        //                {
        //                    lstrStringBuilder.Append(Strings.FormatNumber(lobjScrip.MaximumRedemptionQty, 4));
        //                }
        //                else
        //                {
        //                    lstrStringBuilder.Append("");
        //                }


        //                lstrStringBuilder.Append("|");
        //                //BSEGROUPNAME
        //                if (!(string.IsNullOrWhiteSpace(lobjScrip.RedemptionAllowed) == true))
        //                {
        //                    lstrStringBuilder.Append(lobjScrip.RedemptionAllowed);
        //                }
        //                else
        //                {
        //                    lstrStringBuilder.Append("");
        //                }

        //                lstrStringBuilder.Append("|");
        //                //BSEGROUPNAME
        //                if (!(string.IsNullOrWhiteSpace(lobjScrip.RedemptionCutoffTime) == true))
        //                {
        //                    lstrStringBuilder.Append(lobjScrip.RedemptionCutoffTime);
        //                }
        //                else
        //                {
        //                    lstrStringBuilder.Append("");
        //                }
        //                lstrStringBuilder.Append("|");
        //                //BSEGROUPNAME
        //                if (!(string.IsNullOrWhiteSpace(lobjScrip.RTAAgentCode) == true))
        //                {
        //                    lstrStringBuilder.Append(lobjScrip.RTAAgentCode);
        //                }
        //                else
        //                {
        //                    lstrStringBuilder.Append("");
        //                }
        //                lstrStringBuilder.Append("|");
        //                //BSEGROUPNAME
        //                if (!(string.IsNullOrWhiteSpace(lobjScrip.AMCActiveFlag.ToString()) == true))
        //                {
        //                    lstrStringBuilder.Append(lobjScrip.AMCActiveFlag);
        //                }
        //                else
        //                {
        //                    lstrStringBuilder.Append("");
        //                }
        //                lstrStringBuilder.Append("|");
        //                //DividendReinvestmentFlag string, SchemeType string,SIPFLAG char,STPFLAG char,SWPFLAG char,SETTLEMENTTYPE string, PurchaseAmountMultiplier numeric,ROWSTATE INT,NAV numeric"
        //                //BSEGROUPNAME
        //                if (!(string.IsNullOrWhiteSpace(lobjScrip.DividendReinvestmentFlag) == true))
        //                {
        //                    lstrStringBuilder.Append(lobjScrip.DividendReinvestmentFlag);
        //                }
        //                else
        //                {
        //                    lstrStringBuilder.Append("");
        //                }
        //                lstrStringBuilder.Append("|");
        //                //BSEGROUPNAME
        //                if (!(string.IsNullOrWhiteSpace(lobjScrip.SchemeType) == true))
        //                {
        //                    lstrStringBuilder.Append(lobjScrip.SchemeType);
        //                }
        //                else
        //                {
        //                    lstrStringBuilder.Append("");
        //                }
        //                lstrStringBuilder.Append("|");
        //                //BSEGROUPNAME
        //                if (!(string.IsNullOrWhiteSpace(lobjScrip.SIPFLAG.ToString()) == true))
        //                {
        //                    lstrStringBuilder.Append(lobjScrip.SIPFLAG);
        //                }
        //                else
        //                {
        //                    lstrStringBuilder.Append("");
        //                }
        //                lstrStringBuilder.Append("|");
        //                //BSEGROUPNAME
        //                if (!(string.IsNullOrWhiteSpace(lobjScrip.STPFLAG.ToString()) == true))
        //                {
        //                    lstrStringBuilder.Append(lobjScrip.STPFLAG);
        //                }
        //                else
        //                {
        //                    lstrStringBuilder.Append("");
        //                }
        //                lstrStringBuilder.Append("|");
        //                //BSEGROUPNAME
        //                if (!(string.IsNullOrWhiteSpace(lobjScrip.SWPFLAG.ToString()) == true))
        //                {
        //                    lstrStringBuilder.Append(lobjScrip.SWPFLAG);
        //                }
        //                else
        //                {
        //                    lstrStringBuilder.Append("");
        //                }

        //                lstrStringBuilder.Append("|");
        //                //BSEGROUPNAME
        //                if (!(string.IsNullOrWhiteSpace(lobjScrip.SETTLEMENTTYPE) == true))
        //                {
        //                    lstrStringBuilder.Append(lobjScrip.SETTLEMENTTYPE);
        //                }
        //                else
        //                {
        //                    lstrStringBuilder.Append("");
        //                }
        //                lstrStringBuilder.Append("|");
        //                //BSEGROUPNAME
        //                if (!(string.IsNullOrWhiteSpace(lobjScrip.PurchaseAmountMultiplier) == true))
        //                {
        //                    lstrStringBuilder.Append(Strings.FormatNumber(lobjScrip.PurchaseAmountMultiplier, 4));
        //                }
        //                else
        //                {
        //                    lstrStringBuilder.Append("");
        //                }
        //                lstrStringBuilder.Append("|");
        //                //BSEGROUPNAME
        //                if (!(string.IsNullOrWhiteSpace(lobjScrip.NAV) == true))
        //                {
        //                    lstrStringBuilder.Append(Strings.FormatNumber(lobjScrip.NAV, 4));
        //                }
        //                else
        //                {
        //                    lstrStringBuilder.Append("");
        //                }

        //                lstrStringBuilder.Append("~");
        //            }
        //        }
        //        return lstrStringBuilder;
        //    }
        //    public StringBuilder GetStraMFContracts(StringBuilder lstrStringBuilder, string pstrAMCCode, string pstrSchemeName, bool IsSip = false)
        //    {
        //        MutualFund lobjScrip = default(MutualFund);
        //        ScriptCollection lobjScripColl = default(ScriptCollection);
        //        ExchangeColumnName_Map lobjExchangeColumnMap = ColumnNameFactory.GetExchangeColumnMap( (Constants.EX_BSE_VALUE),  (Constants.MKT_MF_VALUE));
        //        StringBuilder lobjWhereClause = new StringBuilder();

        //        lobjWhereClause.Append(" 2 = 2 ");
        //        if (string.IsNullOrWhiteSpace(pstrSchemeName) == false)
        //        {
        //            lobjWhereClause.Append(string.Format(" and {0} like '{1}%'", lobjExchangeColumnMap.ColumnName_Map.Item(BaseColumnNames.Name), pstrSchemeName));
        //        }
        //        if (string.IsNullOrWhiteSpace(pstrAMCCode) == false)
        //        {
        //            lobjWhereClause.Append(string.Format(" and {0} like '{1}%'", "AMCCode", pstrAMCCode));
        //        }
        //        if (IsSip)
        //        {
        //            lobjWhereClause.Append(string.Format(" and {0} like '{1}%'", "SipFlag", "Y"));
        //        }
        //        if (lstrStringBuilder == null)
        //        {
        //            lstrStringBuilder = new StringBuilder();
        //        }
        //        lobjScripColl = GetScripFactory.GetScrips( (Constants.EX_BSE_VALUE),  (Constants.MKT_MF_VALUE), lobjWhereClause.ToString());
        //        if ((lobjScripColl != null))
        //        {
        //            for (int lintTemp = 0; lintTemp <= lobjScripColl.Count - 1; lintTemp++)
        //            {
        //                lobjScrip = (MutualFund)lobjScripColl[lintTemp];

        //                // "UniqueNo string,SchemeCode string,RTASchemeCode string,AMCSchemeCode string,ISIN string,AMCCode string,
        //                lstrStringBuilder.Append(lobjScrip.Token);
        //                lstrStringBuilder.Append("|");
        //                //SYMBOL
        //                if (!(string.IsNullOrWhiteSpace(lobjScrip.Symbol) == true))
        //                {
        //                    lstrStringBuilder.Append(lobjScrip.Symbol);
        //                }
        //                else
        //                {
        //                    lstrStringBuilder.Append("");
        //                }
        //                lstrStringBuilder.Append("|");
        //                //SERIES
        //                if (!(string.IsNullOrWhiteSpace(lobjScrip.RTASchemeCode) == true))
        //                {
        //                    lstrStringBuilder.Append(lobjScrip.RTASchemeCode);
        //                }
        //                else
        //                {
        //                    lstrStringBuilder.Append("");
        //                }
        //                lstrStringBuilder.Append("|");
        //                //COMPANYNAME
        //                if (!(string.IsNullOrWhiteSpace(lobjScrip.AMCSchemeCode) == true))
        //                {
        //                    lstrStringBuilder.Append(lobjScrip.AMCSchemeCode);
        //                }
        //                else
        //                {
        //                    lstrStringBuilder.Append("");
        //                }
        //                lstrStringBuilder.Append("|");
        //                //BSETOKEN
        //                if (!(string.IsNullOrWhiteSpace(lobjScrip.ISIN) == true))
        //                {
        //                    lstrStringBuilder.Append(lobjScrip.ISIN);
        //                }
        //                else
        //                {
        //                    lstrStringBuilder.Append("");
        //                }
        //                lstrStringBuilder.Append("|");

        //                //NSETOKEN
        //                if (!(string.IsNullOrWhiteSpace(lobjScrip.AMCCode) == true))
        //                {
        //                    lstrStringBuilder.Append(lobjScrip.AMCCode);
        //                }
        //                else
        //                {
        //                    lstrStringBuilder.Append("");
        //                }
        //                lstrStringBuilder.Append("|");

        //                //SchemeName string,PurchaseTransactionmode string,MinimumPurchaseAmount numeric,AdditionalPurchaseAmountMultiple numeric, 
        //                //BSEBOARDLOTQUANTITY
        //                if (!(string.IsNullOrWhiteSpace(lobjScrip.Name) == true))
        //                {
        //                    lstrStringBuilder.Append(lobjScrip.Name);
        //                }
        //                else
        //                {
        //                    lstrStringBuilder.Append("");
        //                }
        //                lstrStringBuilder.Append("|");

        //                //NSEBOARDLOTQUANTITY

        //                if (!(string.IsNullOrWhiteSpace(lobjScrip.PurchaseTransactionmode) == true))
        //                {
        //                    lstrStringBuilder.Append(lobjScrip.PurchaseTransactionmode);
        //                }
        //                else
        //                {
        //                    lstrStringBuilder.Append("");
        //                }
        //                lstrStringBuilder.Append("|");
        //                //BSETICKSIZE
        //                if (!(string.IsNullOrWhiteSpace(lobjScrip.MinimumPurchaseAmount) == true))
        //                {
        //                    lstrStringBuilder.Append(Strings.FormatNumber(lobjScrip.MinimumPurchaseAmount, 4));
        //                }
        //                else
        //                {
        //                    lstrStringBuilder.Append("");
        //                }
        //                lstrStringBuilder.Append("|");
        //                //NSETICKSIZE
        //                if (!(string.IsNullOrWhiteSpace(lobjScrip.AdditionalPurchaseAmountMultiple) == true))
        //                {
        //                    lstrStringBuilder.Append(Strings.FormatNumber(lobjScrip.AdditionalPurchaseAmountMultiple, 4));
        //                }
        //                else
        //                {
        //                    lstrStringBuilder.Append("");
        //                }
        //                lstrStringBuilder.Append("|");

        //                //MaximumPurchaseAmount numeric,PurchaseAllowed string,PurchaseCutoffTime string,RedemptionTransactionMode string,MinimumRedemptionQty numeric, 
        //                //BSESCRIPTID
        //                if (!(string.IsNullOrWhiteSpace(lobjScrip.MaximumPurchaseAmount) == true))
        //                {
        //                    lstrStringBuilder.Append(Strings.FormatNumber(lobjScrip.MaximumPurchaseAmount, 4));
        //                }
        //                else
        //                {
        //                    lstrStringBuilder.Append("");
        //                }
        //                lstrStringBuilder.Append("|");

        //                //BSESCRIPTNAME
        //                if (!(string.IsNullOrWhiteSpace(lobjScrip.PurchaseAllowed) == true))
        //                {
        //                    lstrStringBuilder.Append(lobjScrip.PurchaseAllowed);
        //                }
        //                else
        //                {
        //                    lstrStringBuilder.Append("");
        //                }
        //                lstrStringBuilder.Append("|");
        //                //BSEGROUPNAME
        //                if (!(string.IsNullOrWhiteSpace(lobjScrip.PurchaseCutoffTime) == true))
        //                {
        //                    lstrStringBuilder.Append(lobjScrip.PurchaseCutoffTime);
        //                }
        //                else
        //                {
        //                    lstrStringBuilder.Append("");
        //                }

        //                lstrStringBuilder.Append("|");
        //                //BSEGROUPNAME
        //                if (!(string.IsNullOrWhiteSpace(lobjScrip.RedemptionTransactionMode) == true))
        //                {
        //                    lstrStringBuilder.Append(lobjScrip.RedemptionTransactionMode);
        //                }
        //                else
        //                {
        //                    lstrStringBuilder.Append("");
        //                }
        //                lstrStringBuilder.Append("|");
        //                //BSEGROUPNAME
        //                if (!(string.IsNullOrWhiteSpace(lobjScrip.MinimumRedemptionQty) == true))
        //                {
        //                    lstrStringBuilder.Append(Strings.FormatNumber(lobjScrip.MinimumRedemptionQty, 4));
        //                }
        //                else
        //                {
        //                    lstrStringBuilder.Append("");
        //                }

        //                lstrStringBuilder.Append("|");
        //                //RedemptionQtyMultiplier numeric, MaximumRedemptionQty string,RedemptionAllowed string,RedemptionCutoffTime string,RTAAgentCode string,AMCActiveFlag INT, 
        //                //BSEGROUPNAME
        //                if (!(string.IsNullOrWhiteSpace(lobjScrip.RedemptionQtyMultiplier) == true))
        //                {
        //                    lstrStringBuilder.Append(Strings.FormatNumber(lobjScrip.RedemptionQtyMultiplier, 4));
        //                }
        //                else
        //                {
        //                    lstrStringBuilder.Append("");
        //                }


        //                lstrStringBuilder.Append("|");
        //                //BSEGROUPNAME
        //                if (!(string.IsNullOrWhiteSpace(lobjScrip.MaximumRedemptionQty) == true))
        //                {
        //                    lstrStringBuilder.Append(Strings.FormatNumber(lobjScrip.MaximumRedemptionQty, 4));
        //                }
        //                else
        //                {
        //                    lstrStringBuilder.Append("");
        //                }


        //                lstrStringBuilder.Append("|");
        //                //BSEGROUPNAME
        //                if (!(string.IsNullOrWhiteSpace(lobjScrip.RedemptionAllowed) == true))
        //                {
        //                    lstrStringBuilder.Append(lobjScrip.RedemptionAllowed);
        //                }
        //                else
        //                {
        //                    lstrStringBuilder.Append("");
        //                }

        //                lstrStringBuilder.Append("|");
        //                //BSEGROUPNAME
        //                if (!(string.IsNullOrWhiteSpace(lobjScrip.RedemptionCutoffTime) == true))
        //                {
        //                    lstrStringBuilder.Append(lobjScrip.RedemptionCutoffTime);
        //                }
        //                else
        //                {
        //                    lstrStringBuilder.Append("");
        //                }
        //                lstrStringBuilder.Append("|");
        //                //BSEGROUPNAME
        //                if (!(string.IsNullOrWhiteSpace(lobjScrip.RTAAgentCode) == true))
        //                {
        //                    lstrStringBuilder.Append(lobjScrip.RTAAgentCode);
        //                }
        //                else
        //                {
        //                    lstrStringBuilder.Append("");
        //                }
        //                lstrStringBuilder.Append("|");
        //                //BSEGROUPNAME
        //                if (!(string.IsNullOrWhiteSpace(lobjScrip.AMCActiveFlag.ToString()) == true))
        //                {
        //                    lstrStringBuilder.Append(lobjScrip.AMCActiveFlag);
        //                }
        //                else
        //                {
        //                    lstrStringBuilder.Append("");
        //                }
        //                lstrStringBuilder.Append("|");
        //                //DividendReinvestmentFlag string, SchemeType string,SIPFLAG char,STPFLAG char,SWPFLAG char,SETTLEMENTTYPE string, PurchaseAmountMultiplier numeric,ROWSTATE INT,NAV numeric"
        //                //BSEGROUPNAME
        //                if (!(string.IsNullOrWhiteSpace(lobjScrip.DividendReinvestmentFlag) == true))
        //                {
        //                    lstrStringBuilder.Append(lobjScrip.DividendReinvestmentFlag);
        //                }
        //                else
        //                {
        //                    lstrStringBuilder.Append("");
        //                }
        //                lstrStringBuilder.Append("|");
        //                //BSEGROUPNAME
        //                if (!(string.IsNullOrWhiteSpace(lobjScrip.SchemeType) == true))
        //                {
        //                    lstrStringBuilder.Append(lobjScrip.SchemeType);
        //                }
        //                else
        //                {
        //                    lstrStringBuilder.Append("");
        //                }
        //                lstrStringBuilder.Append("|");
        //                //BSEGROUPNAME
        //                if (!(string.IsNullOrWhiteSpace(lobjScrip.SIPFLAG) == true))
        //                {
        //                    lstrStringBuilder.Append(lobjScrip.SIPFLAG);
        //                }
        //                else
        //                {
        //                    lstrStringBuilder.Append("");
        //                }
        //                lstrStringBuilder.Append("|");
        //                //BSEGROUPNAME
        //                if (!(string.IsNullOrWhiteSpace(lobjScrip.STPFLAG) == true))
        //                {
        //                    lstrStringBuilder.Append(lobjScrip.STPFLAG);
        //                }
        //                else
        //                {
        //                    lstrStringBuilder.Append("");
        //                }
        //                lstrStringBuilder.Append("|");
        //                //BSEGROUPNAME
        //                if (!(string.IsNullOrWhiteSpace(lobjScrip.SWPFLAG) == true))
        //                {
        //                    lstrStringBuilder.Append(lobjScrip.SWPFLAG);
        //                }
        //                else
        //                {
        //                    lstrStringBuilder.Append("");
        //                }

        //                lstrStringBuilder.Append("|");
        //                //BSEGROUPNAME
        //                if (!(string.IsNullOrWhiteSpace(lobjScrip.SETTLEMENTTYPE) == true))
        //                {
        //                    lstrStringBuilder.Append(lobjScrip.SETTLEMENTTYPE);
        //                }
        //                else
        //                {
        //                    lstrStringBuilder.Append("");
        //                }
        //                lstrStringBuilder.Append("|");
        //                //BSEGROUPNAME
        //                if (!(string.IsNullOrWhiteSpace(lobjScrip.PurchaseAmountMultiplier) == true))
        //                {
        //                    lstrStringBuilder.Append(Strings.FormatNumber(lobjScrip.PurchaseAmountMultiplier, 4));
        //                }
        //                else
        //                {
        //                    lstrStringBuilder.Append("");
        //                }
        //                lstrStringBuilder.Append("|");
        //                //BSEGROUPNAME
        //                if (!(string.IsNullOrWhiteSpace(lobjScrip.NAV) == true))
        //                {
        //                    lstrStringBuilder.Append(Strings.FormatNumber(lobjScrip.NAV, 4));
        //                }
        //                else
        //                {
        //                    lstrStringBuilder.Append("");
        //                }

        //                lstrStringBuilder.Append("~");
        //            }
        //        }
        //        return lstrStringBuilder;
        //    }
        //    public string[] GetDistinctStarMFAMCCode()
        //    {
        //        ExchangeColumnName_Map lobjExchangeColumnMap = ColumnNameFactory.GetExchangeColumnMap( (Constants.EX_BSE_VALUE),  (Constants.MKT_MF_VALUE));
        //        return GetScripFactory.GetDistinct(Constants.EX_BSE_VALUE,Constants.MKT_MF_VALUE, "AMCCode", true);
        //    }

        //    public string[] GetDistinctStarMFSchemeType()
        //    {
        //        ExchangeColumnName_Map lobjExchangeColumnMap = ColumnNameFactory.GetExchangeColumnMap( (Constants.EX_BSE_VALUE),  (Constants.MKT_MF_VALUE));
        //        return GetScripFactory.GetDistinct(Constants.EX_BSE_VALUE,Constants.MKT_MF_VALUE, "SchemeType", true);
        //    }

        //    public string GetSchemeDetails(string pstrSchemeCode)
        //    {
        //        ExchangeColumnName_Map lobjExchangeColumnMap = ColumnNameFactory.GetExchangeColumnMap( (Constants.EX_BSE_VALUE),  (Constants.MKT_MF_VALUE));
        //        IScript lobjIScrip = GetScript(Constants.EX_BSE_VALUE,Constants.MKT_MF_VALUE, lobjExchangeColumnMap.ColumnName_Map.Values[BaseColumnNames.Symbol], null);

        //        if ((lobjIScrip != null))
        //        {
        //            return string.Format("{0}|{1}|{2}", lobjIScrip.Token, lobjIScrip.Token, ((MutualFund)lobjIScrip).NAV.ToString());
        //        }
        //        else
        //        {
        //            return null;
        //        }
        //    }

        //    public string[] GetStarMFSchemeName(string pstrAMCCode, bool IsSip = false)
        //    {
        //        ExchangeColumnName_Map lobjExchangeColumnMap = ColumnNameFactory.GetExchangeColumnMap(Constants.EX_BSE_VALUE, Constants.MKT_MF_VALUE);
        //        string lstrWhereClause = null;
        //        lstrWhereClause = string.Format(" {0} like '{1}%'", "AMCCode", pstrAMCCode);
        //        if (IsSip)
        //        {
        //            lstrWhereClause = lstrWhereClause + string.Format(" AND {0} like '{1}%'", "SipFlag", "Y%");
        //        }
        //        return GetScripFactory.GetDistinct(Constants.EX_BSE_VALUE,Constants.MKT_MF_VALUE, BaseColumnNames.Name, true, lstrWhereClause);
        //    }

        //    public string[] GetStarMFSchemeCode()
        //    {
        //        ExchangeColumnName_Map lobjExchangeColumnMap = ColumnNameFactory.GetExchangeColumnMap(Constants.EX_BSE_VALUE,Constants.MKT_MF_VALUE);
        //        return GetScripFactory.GetDistinct(Constants.EX_BSE_VALUE,Constants.MKT_MF_VALUE, BaseColumnNames.Symbol, true);
        //    }

        //    #endregion


        //    #region " DEPT"
        //    public BOND GetDeptBond(long plngId)
        //    {
        //        IScript lobjIScrp = GetScript( (Constants.EX_BSE_VALUE), plngId,  (Constants.MKT_DEBT_VALUE));
        //        if ((lobjIScrp != null))
        //        {
        //            return (BOND)lobjIScrp;
        //        }
        //        else
        //        {
        //            return null;
        //        }
        //    }

        //    public BOND GetDeptBond(string pstrSymbol)
        //    {
        //        ExchangeColumnName_Map lobjExchangeColumnMap = ColumnNameFactory.GetExchangeColumnMap(Constants.EX_BSE_VALUE,Constants.MKT_DEBT_VALUE);
        //        IScript lobjIScrip = GetScript( (Constants.EX_BSE_VALUE),  (Constants.MKT_DEBT_VALUE), pstrSymbol, null);

        //        if ((lobjIScrip != null))
        //        {
        //            return (BOND)lobjIScrip;
        //        }
        //        else
        //        {
        //            return null;
        //        }
        //    }
        //    public BOND GetDeptBond(string pstrSymbol, string pstrDisplayExpiryDate)
        //    {
        //        return GetDeptBond(pstrSymbol, GetLongExpiryDateForScript(Constants.EX_BSE_VALUE, pstrDisplayExpiryDate,Constants.MKT_DEBT_VALUE));
        //    }
        //    public BOND GetDeptBond(string pstrSymbol, long plngExpiryDate)
        //    {

        //        ExchangeColumnName_Map lobjExchangeColumnMap = ColumnNameFactory.GetExchangeColumnMap(Constants.EX_BSE_VALUE,Constants.MKT_DEBT_VALUE);
        //        string lstrWhereClause = null;

        //        lstrWhereClause = string.Format(" {0} Like '{1}%' and {2} = {3}", lobjExchangeColumnMap.ColumnName_Map.Item(BaseColumnNames.Symbol), pstrSymbol, lobjExchangeColumnMap.ColumnName_Map.Item(BaseColumnNames.ExpiryDate), plngExpiryDate);

        //        IScript lobjIScrip = GetScripFactory.GetScrip( (Constants.EX_BSE_VALUE),  (Constants.MKT_DEBT_VALUE), lstrWhereClause);
        //        if ((lobjIScrip != null))
        //        {
        //            return (BOND)lobjIScrip;
        //        }
        //        else
        //        {
        //            return null;
        //        }
        //    }

        //    public BOND GetDeptBondOnISIN(string pstrISIN)
        //    {
        //        ExchangeColumnName_Map lobjExchangeColumnMap = ColumnNameFactory.GetExchangeColumnMap(Constants.EX_BSE_VALUE,Constants.MKT_DEBT_VALUE);
        //        string lstrWhereClause = null;

        //        lstrWhereClause = string.Format(" {0} = '{1}'", lobjExchangeColumnMap.ColumnName_Map.Item(BaseColumnNames.ISIN), pstrISIN);

        //        IScript lobjIScrip = GetScripFactory.GetScrip( (Constants.EX_BSE_VALUE),  (Constants.MKT_DEBT_VALUE), lstrWhereClause);
        //        if ((lobjIScrip != null))
        //        {
        //            return (BOND)lobjIScrip;
        //        }
        //        else
        //        {
        //            return null;
        //        }
        //    }

        //    public string[] GetDeptSymbols(string pstrSegment = "")
        //    {
        //        ExchangeColumnName_Map lobjExchangeColumnMap = ColumnNameFactory.GetExchangeColumnMap(Constants.EX_BSE_VALUE,Constants.MKT_DEBT_VALUE);
        //        string lstrWhereClause = null;
        //        if (string.IsNullOrWhiteSpace(pstrSegment) == false)
        //        {
        //            lstrWhereClause = string.Format(" {0} ='{1}'", "WDMSegment", pstrSegment);
        //        }
        //        return GetScripFactory.GetDistinct(Constants.EX_BSE_VALUE,Constants.MKT_DEBT_VALUE, BaseColumnNames.Symbol, true, lstrWhereClause);
        //    }

        //    public string[] GetDeptISINs(string pstrSegment = "")
        //    {
        //        ExchangeColumnName_Map lobjExchangeColumnMap = ColumnNameFactory.GetExchangeColumnMap(Constants.EX_BSE_VALUE,Constants.MKT_DEBT_VALUE);
        //        string lstrWhereClause = null;
        //        if (string.IsNullOrWhiteSpace(pstrSegment) == false)
        //        {
        //            lstrWhereClause = string.Format(" {0} ='{1}'", "WDMSegment", pstrSegment);
        //        }
        //        return GetScripFactory.GetDistinct(Constants.EX_BSE_VALUE,Constants.MKT_DEBT_VALUE, BaseColumnNames.ISIN, true, lstrWhereClause);

        //    }

        //    public BOND GetDeptBondT0(long plngId)
        //    {
        //        IScript lobjIScrp = GetScript( (Constants.EX_BSE_VALUE), plngId,  (Constants.MKT_DEBTT0_VALUE));
        //        if (lobjIScrp == null)
        //        {
        //            return (BOND)lobjIScrp;
        //        }
        //        else
        //        {
        //            return null;
        //        }
        //    }

        //    public BOND GetDeptBondT0(string pstrSymbol)
        //    {
        //        ExchangeColumnName_Map lobjExchangeColumnMap = ColumnNameFactory.GetExchangeColumnMap(Constants.EX_BSE_VALUE,Constants.MKT_DEBTT0_VALUE);
        //        IScript lobjIScrip = GetScript( (Constants.EX_BSE_VALUE),  (Constants.MKT_DEBTT0_VALUE), pstrSymbol, null);

        //        if ((lobjIScrip != null))
        //        {
        //            return (BOND)lobjIScrip;
        //        }
        //        else
        //        {
        //            return null;
        //        }
        //    }

        //    public BOND GetDeptBondT0(string pstrSymbol, long plngExpiryDate)
        //    {
        //        ExchangeColumnName_Map lobjExchangeColumnMap = ColumnNameFactory.GetExchangeColumnMap(Constants.EX_BSE_VALUE,Constants.MKT_DEBTT0_VALUE);
        //        string lstrWhereClause = null;

        //        lstrWhereClause = string.Format(" {0} = '{1}' and {2} = {3}", lobjExchangeColumnMap.ColumnName_Map.Item(BaseColumnNames.Symbol), pstrSymbol, lobjExchangeColumnMap.ColumnName_Map.Item(BaseColumnNames.ExpiryDate), plngExpiryDate);

        //        IScript lobjIScrip = GetScripFactory.GetScrip( (Constants.EX_BSE_VALUE),  (Constants.MKT_DEBTT0_VALUE), lstrWhereClause);
        //        if ((lobjIScrip != null))
        //        {
        //            return (BOND)lobjIScrip;
        //        }
        //        else
        //        {
        //            return null;
        //        }
        //    }

        //    public BOND GetDeptBondT0OnISIN(string pstrISIN)
        //    {
        //        ExchangeColumnName_Map lobjExchangeColumnMap = ColumnNameFactory.GetExchangeColumnMap(Constants.EX_BSE_VALUE,Constants.MKT_DEBTT0_VALUE);
        //        string lstrWhereClause = null;

        //        lstrWhereClause = string.Format(" {0} = '{1}'", lobjExchangeColumnMap.ColumnName_Map.Item(BaseColumnNames.ISIN), pstrISIN);

        //        IScript lobjIScrip = GetScripFactory.GetScrip( (Constants.EX_BSE_VALUE),  (Constants.MKT_DEBTT0_VALUE), lstrWhereClause);
        //        if ((lobjIScrip != null))
        //        {
        //            return (BOND)lobjIScrip;
        //        }
        //        else
        //        {
        //            return null;
        //        }

        //    }

        //    public string[] GetDeptT0Symbols(string pstrSegment = "")
        //    {
        //        ExchangeColumnName_Map lobjExchangeColumnMap = ColumnNameFactory.GetExchangeColumnMap(Constants.EX_BSE_VALUE,Constants.MKT_DEBTT0_VALUE);
        //        string lstrWhereClause = null;
        //        if (string.IsNullOrWhiteSpace(pstrSegment) == false)
        //        {
        //            lstrWhereClause = string.Format(" {0} ='{1}'", "WDMSegment", pstrSegment);
        //        }
        //        return GetScripFactory.GetDistinct(Constants.EX_BSE_VALUE,Constants.MKT_DEBTT0_VALUE, BaseColumnNames.Symbol, true, lstrWhereClause);

        //    }

        //    public string[] GetDeptT0ISINs(string pstrSegment = "")
        //    {
        //        ExchangeColumnName_Map lobjExchangeColumnMap = ColumnNameFactory.GetExchangeColumnMap(Constants.EX_BSE_VALUE,Constants.MKT_DEBTT0_VALUE);
        //        string lstrWhereClause = null;
        //        if (string.IsNullOrWhiteSpace(pstrSegment) == false)
        //        {
        //            lstrWhereClause = string.Format(" {0} ='{1}'", "WDMSegment", pstrSegment);
        //        }
        //        return GetScripFactory.GetDistinct(Constants.EX_BSE_VALUE,Constants.MKT_DEBTT0_VALUE, BaseColumnNames.ISIN, true, lstrWhereClause);

        //    }
        //    #endregion

        //    #region "Generalized Property"

        //    public string[] GetScriptSymbols
        //    {
        //        get
        //        {
        //            StackTrace lobjStackTrace = new StackTrace();
        //            //: No need to populate when called from Clear or Initialize
        //            //If lobjStackTrace.ToString.IndexOf("Clear") = -1 AndAlso lobjStackTrace.ToString.IndexOf("Initialize") = -1 Then
        //            if (lobjStackTrace.ToString.IndexOf("Clear") == -1)
        //            {
        //                return GetSymbolsForScript(pintExchangeId, pintMarketId, pstrInstrumentName, pstrCompanyName, pblnUseNameForSymbol, pblnIsSpread);
        //            }
        //            lobjStackTrace = null;
        //            return null;
        //        }
        //    }
        //    public string[] GetScriptCompanyName
        //    {
        //        get { return GetCompanyNameForScrips(pintExchangeId, pintMarketId, pstrSymbol); }
        //    }
        //    public string[] FetchScriptLongExpiryDates
        //    {
        //        get
        //        {
        //            if (pintExchangeId == null || pintExchangeId.Trim.Length == 0)
        //                pintExchangeId = "-1";
        //            if (pintMaketId == null || pintMaketId.Trim.Length == 0)
        //                pintMaketId = "-1";
        //            return mobjMyExpiryDates.GetLongExpiryDates(pintExchangeId, pintMaketId);
        //        }
        //    }
        //    //'Set(ByVal value As String())
        //    //' If pintMaketId Is Nothing OrElse pintMaketId.Trim.Length = 0 Then pintMaketId = "-1"
        //    //' mobjMyExpiryDates.AddLongExpiryList(value, pintExchangeId, pintMaketId)
        //    //'End Set

        //    public string[] FetchScriptExpiryDates
        //    {
        //        get
        //        {
        //            if (pintExchangeId == null || pintExchangeId.Trim.Length == 0)
        //                pintExchangeId = "-1";
        //            if (pintMaketId == null || pintMaketId.Trim.Length == 0)
        //                pintMaketId = "-1";
        //            return mobjMyExpiryDates.GetStringExpiryDates(pintExchangeId, pintMaketId);
        //        }
        //    }
        //    //'Set(ByVal value As String())
        //    //' If pintMaketId Is Nothing OrElse pintMaketId.Trim.Length = 0 Then pintMaketId = "-1"
        //    //' mobjMyExpiryDates.AddStringExpiryList(value, pintExchangeId, pintMaketId)
        //    //'End Set

        //    public string[] FetchInstrumentNames
        //    {
        //        get
        //        {
        //            if (mobjInstrumentTypes.ContainsKey(pintExchangeId + "|" + pintMaketId) == false)
        //            {
        //                CacheDistinctInstrumentNames(pintExchangeId, pintMaketId);
        //            }
        //            if (pintSegmentId == 0)
        //            {
        //                if (mobjInstrumentTypes.Item(pintExchangeId + "|" + pintMaketId) == null)
        //                {
        //                    string[] lobjString = new string[1];
        //                    lobjString[0] = "";
        //                    return lobjString;
        //                }
        //                return mobjInstrumentTypes.Item(pintExchangeId + "|" + pintMaketId);
        //            }
        //            else
        //            {
        //                string[] lobjInstrumentNames = null;
        //                ArrayList lobjInstrumentColl = new ArrayList();
        //                lobjInstrumentNames = mobjInstrumentTypes.Item(pintExchangeId + "|" + pintMaketId);
        //                if ((lobjInstrumentNames != null))
        //                {
        //                    for (Int16 lintTemp = 0; lintTemp <= lobjInstrumentNames.Length - 1; lintTemp++)
        //                    {
        //                        if (pintSegmentId ==Constants.SGT_FUTURES_VALUE || pintSegmentId ==Constants.SGT_COMM_FUTURES_VALUE)
        //                        {
        //                            if (lobjInstrumentNames[lintTemp].StartsWith("FUT"))
        //                            {
        //                                lobjInstrumentColl.Add(lobjInstrumentNames[lintTemp]);
        //                            }
        //                        }
        //                        else if (pintSegmentId ==Constants.SGT_OPTIONS_VALUE || pintSegmentId ==Constants.SGT_COMM_OPTIONS_VALUE)
        //                        {
        //                            if (lobjInstrumentNames(lintTemp).StartsWith("OPT"))
        //                            {
        //                                lobjInstrumentColl.Add(lobjInstrumentNames[lintTemp]);
        //                            }
        //                        }
        //                        else if (pintSegmentId ==Constants.SGT_COMM_CASH_VALUE)
        //                        {
        //                            if (lobjInstrumentNames[lintTemp].StartsWith("COM"))
        //                            {
        //                                lobjInstrumentColl.Add(lobjInstrumentNames[lintTemp]);
        //                            }
        //                        }
        //                    }
        //                    return lobjInstrumentColl.ToArray(typeof(string));
        //                }
        //            }
        //            return null;
        //        }
        //    }

        //    #endregion

        //    #region "Generalized"

        //    public bool ValidateScript(int pintExchangeId, int pintMarketId, string pstrInstrumentName = null, string pstrExpiryDate = null, string pstrStrikePrice = null, string pstrOptionType = null, string pstrSymbol = null, string pstrSeries = null)
        //    {

        //        if (pintMarketId== Constants.MKT_EQUITY_VALUE && (pintExchangeId== Constants.EX_BSE_VALUE || pintExchangeId== Constants.EX_NSE_VALUE || pintExchangeId == 0))
        //        {
        //            return ValidateSecurity(pstrSymbol, pstrSeries, pintExchangeId);
        //        }

        //        //If (pintMarketId =Constants.MKT_DEBT_VALUE AndAlso pintExchangeId =Constants.EX_BSE_VALUE) Then
        //        // Dim lobjIScrip As IScript = GetDeptBond(pstrSymbol, pstrExpiryDate)
        //        //End If

        //        ContractBase lobjContract =(ContractBase) GetScript(pintExchangeId, pintMarketId, pstrSymbol, pstrInstrumentName, pstrExpiryDate, pstrStrikePrice, pstrOptionType);
        //        if ((lobjContract == null))
        //        {
        //            return false;
        //        }
        //        else
        //        {
        //            return true;
        //        }
        //    }
        //    /// <summary>
        //    /// Searches Scrip based on ID Column
        //    /// </summary>
        //    /// <param name="pintExchangeId">ExchangeID</param>
        //    /// <param name="plngScripToken">Exchange Token</param>
        //    /// <param name="pintMarketId">MarketID</param>
        //    /// <returns>Instance of IScrip</returns>
        //    /// <remarks>Use this if you want to search scrip based on Token assigned by Exchange . Use GetScript if you want to search by ID </remarks>
        //    public IScript GetScriptByToken(int pintExchangeId, long plngScripToken, int pintMarketId, bool ISBasedOnScriptCode = false)
        //    {

        //        ExchangeColumnName_Map lobjExchangeColumnMap = ColumnNameFactory.GetExchangeColumnMap(pintExchangeId, pintMarketId);
        //        string lstrWhereClause = null;
        //        if (ISBasedOnScriptCode)
        //        {
        //            lstrWhereClause = string.Format(" {0} = {1}", lobjExchangeColumnMap.ColumnName_Map.Item(BaseColumnNames.ID), plngScripToken.ToString());
        //        }
        //        else
        //        {
        //            lstrWhereClause = string.Format(" {0} = {1}", lobjExchangeColumnMap.ColumnName_Map.Item(BaseColumnNames.Token), plngScripToken.ToString());
        //        }


        //        return GetScripFactory.GetScrip(pintExchangeId, pintMarketId, lstrWhereClause);
        //    }
        /// <summary>
        /// Searches Scrip based on ID Column
        /// </summary>
        /// <param name="pintExchangeId">ExchangeID</param>
        /// <param name="plngScripID">ScripID</param>
        /// <param name="pintMarketId">MarketID</param>
        /// <returns>Instance of IScrip</returns>
        /// <remarks>Use this if you want to search scrip based on ID . Use GetScriptByToken if you want to search by Token </remarks>
        //public IScript GetScript(int pintExchangeId, long plngScripID, int pintMarketId)
        //{
        //    IScript lobjIScript = null;
        //    string lstrKey = plngScripID.ToString() + "^" + pintExchangeId.ToString() + "^" + pintMarketId.ToString();

        //    if (mobjFrequentlyUsedScrips.ContainsKey(lstrKey) == true)
        //    {
        //        return mobjFrequentlyUsedScrips.Item[lstrKey];
        //    }
        //    else
        //    {
        //        ExchangeColumnName_Map lobjExchangeColumnMap = ColumnNameFactory.GetExchangeColumnMap(pintExchangeId, pintMarketId);
        //        string lstrWhereClause = string.Format(" {0} = {1}", lobjExchangeColumnMap.ColumnName_Map.Item(BaseColumnNames.ID), plngScripID.ToString());


        //        lobjIScript = GetScripFactory.GetScrip(pintExchangeId, pintMarketId, lstrWhereClause);
        //        if ((lobjIScript != null))
        //        {
        //            mobjFrequentlyUsedScrips.Add(lstrKey, lobjIScript);
        //        }
        //        return lobjIScript;
        //    }
        //}

        //    public IScript GetScript(int pintExchangeId, int pintMarketId, string pstrSymbol, string pstrSeries)
        //    {

        //        if (string.IsNullOrWhiteSpace(pstrSymbol) == false)
        //        {
        //            ExchangeColumnName_Map lobjExchangeColumnMap = ColumnNameFactory.GetExchangeColumnMap(pintExchangeId, pintMarketId);
        //            StringBuilder lobjWhereClause = new StringBuilder();
        //            lobjWhereClause.Append(" 2=2");
        //            lobjWhereClause.Append(" and (( ");
        //            lobjWhereClause.Append(string.Format(" {0} = '{1}'", lobjExchangeColumnMap.ColumnName_Map.Item(BaseColumnNames.Symbol), pstrSymbol));
        //            if (string.IsNullOrWhiteSpace(pstrSeries) == false)
        //            {
        //                lobjWhereClause.Append(string.Format(" and {0} = '{1}' ", lobjExchangeColumnMap.ColumnName_Map.Item(BaseColumnNames.Series), pstrSeries));
        //            }
        //            lobjWhereClause.Append(" )");
        //            if (Information.IsNumeric(pstrSymbol))
        //            {
        //                lobjWhereClause.Append(" OR (");
        //                lobjWhereClause.Append(string.Format(" {0} = {1} ", lobjExchangeColumnMap.ColumnName_Map.Item(BaseColumnNames.Token), pstrSymbol));
        //                lobjWhereClause.Append(" )");
        //            }
        //            lobjWhereClause.Append(" )");

        //            lobjWhereClause.Append(string.Format(" and {0} > 0 ", lobjExchangeColumnMap.ColumnName_Map.Item(BaseColumnNames.Token)));

        //            return GetScripFactory.GetScrip(pintExchangeId, pintMarketId, lobjWhereClause.ToString());
        //        }
        //        else
        //        {
        //            return null;
        //        }
        //    }

        //    public IScript GetScript(int pintExchangeId, int pintMarketId, string pstrSymbol, string pstrInstrumentName, string pstrExpiryDate, string pstrStrikePrice, string pstrOptionType)
        //    {

        //        ExchangeColumnName_Map lobjExchangeColumnMap = ColumnNameFactory.GetExchangeColumnMap(pintExchangeId, pintMarketId);
        //        string lstrOptionTypePart = null;

        //        if (string.IsNullOrWhiteSpace(pstrStrikePrice))
        //        {
        //            pstrStrikePrice = "-1";
        //        }
        //        if (string.IsNullOrWhiteSpace(pstrOptionType) || (pstrOptionType.Trim.ToUpper == "XX"))
        //        {
        //            lstrOptionTypePart = " ( " + lobjExchangeColumnMap.ColumnName_Map.Item(BaseColumnNames.OptionType) + " = 'XX' OR " + lobjExchangeColumnMap.ColumnName_Map.Item(BaseColumnNames.OptionType) + "='')";
        //        }
        //        else
        //        {
        //            lstrOptionTypePart = lobjExchangeColumnMap.ColumnName_Map.Item(BaseColumnNames.OptionType) + " = '" + pstrOptionType + "' ";
        //        }

        //        string lstrWhereClause = string.Format(" {0} = '{1}' and {2} = '{3}' and {4} = {5} and {6} = {7} and ", lobjExchangeColumnMap.ColumnName_Map.Item(BaseColumnNames.InstrumentName), pstrInstrumentName, lobjExchangeColumnMap.ColumnName_Map.Item(BaseColumnNames.Symbol), pstrSymbol, lobjExchangeColumnMap.ColumnName_Map.Item(BaseColumnNames.ExpiryDate), pstrExpiryDate, lobjExchangeColumnMap.ColumnName_Map.Item(BaseColumnNames.StrikePrice), pstrStrikePrice);
        //        lstrWhereClause += lstrOptionTypePart;

        //        return GetScripFactory.GetScrip(pintExchangeId, pintMarketId, lstrWhereClause);
        //    }



        //    public IScript GetScript(int pintExchangeId, int pintMarketId, string pstrSymbol, string pstrInstrumentName, string pstrExpiryDate, string pstrStrikePrice, string pstrOptionType, bool pblnUseNameInSymbol)
        //    {

        //        ExchangeColumnName_Map lobjExchangeColumnMap = ColumnNameFactory.GetExchangeColumnMap(pintExchangeId, pintMarketId);
        //        string lstrOptionTypePart = null;
        //        if (string.IsNullOrWhiteSpace(pstrStrikePrice))
        //        {
        //            pstrStrikePrice = "-1";
        //        }
        //        if (string.IsNullOrWhiteSpace(pstrOptionType) || (pstrOptionType.Trim.ToUpper == "XX"))
        //        {
        //            lstrOptionTypePart = " ( " + lobjExchangeColumnMap.ColumnName_Map.Item(BaseColumnNames.OptionType) + " = 'XX' OR " + lobjExchangeColumnMap.ColumnName_Map.Item(BaseColumnNames.OptionType) + "='')";
        //        }
        //        else
        //        {
        //            lstrOptionTypePart = lobjExchangeColumnMap.ColumnName_Map.Item(BaseColumnNames.OptionType) + " = '" + pstrOptionType + "' ";
        //        }

        //        string lstrWhereClause = string.Format(" {0} = '{1}' and {2} = '{3}' and {4} = {5} and {6} = {7} and ", lobjExchangeColumnMap.ColumnName_Map.Item(BaseColumnNames.InstrumentName), pstrInstrumentName, lobjExchangeColumnMap.ColumnName_Map.Item(BaseColumnNames.Symbol), pstrSymbol, lobjExchangeColumnMap.ColumnName_Map.Item(BaseColumnNames.ExpiryDate), pstrExpiryDate, lobjExchangeColumnMap.ColumnName_Map.Item(BaseColumnNames.StrikePrice), pstrStrikePrice);
        //        lstrWhereClause += lstrOptionTypePart;

        //        return GetScripFactory.GetScrip(pintExchangeId, pintMarketId, lstrWhereClause);
        //    }

        //    public IScript GetScript(int pintExchangeId, int pintMarketId, string pstrSymbol, string pstrInstrumentName, string pstrExpiryDate, string pstrStrikePrice, string pstrOptionType, string pstrSeries)
        //    {
        //        if (pintMarketId.ToString() ==Constants.MKT_EQUITY_VALUE)
        //        {
        //            return GetScript(pintExchangeId, pintMarketId, pstrSymbol, pstrSeries);
        //        }
        //        else
        //        {
        //            return GetScript(pintExchangeId, pintMarketId, pstrSymbol, pstrInstrumentName, pstrExpiryDate, pstrStrikePrice, pstrOptionType);
        //        }
        //    }

        //    public string[] GetDistinctSeries(bool pblnAllowBlank = false)
        //    {
        //        ExchangeColumnName_Map lobjExchangeColumnMap = ColumnNameFactory.GetExchangeColumnMap(Constants.EX_NSE_VALUE,Constants.MKT_EQUITY_VALUE);
        //        return GetScripFactory.GetDistinct(Constants.EX_NSE_VALUE,Constants.MKT_EQUITY_VALUE, BaseColumnNames.Series, pblnAllowBlank);
        //    }

        //    public string[] GetDistinctGroup(int pintExchangeID, int pintMarketID, bool pblnAllowBlank = false)
        //    {
        //        ExchangeColumnName_Map lobjExchangeColumnMap = ColumnNameFactory.GetExchangeColumnMap(pintExchangeID, pintMarketID);
        //        return GetScripFactory.GetDistinct(pintExchangeID, pintMarketID, BaseColumnNames.Group, pblnAllowBlank);
        //    }



        //    public void GetScrips(int pintExchangeId, int pintMarketId, string pstrSymbol, ref ScriptCollection pobjScripCollection, string pstrInstrumnentName = "", bool IsSearchFromSeries = false)
        //    {
        //        ExchangeColumnName_Map lobjExchangeColumnMap = ColumnNameFactory.GetExchangeColumnMap(pintExchangeId, pintMarketId);
        //        StringBuilder lobjWhereClause = new StringBuilder();

        //        lobjWhereClause.Append(string.Format(" {0} = '{1}'", lobjExchangeColumnMap.ColumnName_Map.Item(BaseColumnNames.Symbol), pstrSymbol));
        //        if (string.IsNullOrWhiteSpace(pstrInstrumnentName) == false)
        //        {
        //            lobjWhereClause.Append(string.Format(" and {0} LIKE '{1}%' ", lobjExchangeColumnMap.ColumnName_Map.Item(BaseColumnNames.InstrumentName), pstrInstrumnentName));
        //        }
        //        lobjWhereClause.Append(" Order By " + lobjExchangeColumnMap.ColumnName_Map.Item(BaseColumnNames.ExpiryDate) + " , " + lobjExchangeColumnMap.ColumnName_Map.Item(BaseColumnNames.StrikePrice));

        //        GetScripFactory.GetScrips(pintExchangeId, pintMarketId, lobjWhereClause.ToString(), pobjScripCollection);
        //    }

        //    public ScriptCollection GetScrips(int pintExchangeId, int pintMarketId)
        //    {
        //        string lstrWhereClause = null;
        //        if (pintMarketId.ToString() == Constants.MKT_EQUITY_VALUE)
        //        {
        //            ExchangeColumnName_Map lobjExchangeColumnMap = ColumnNameFactory.GetExchangeColumnMap(pintExchangeId, pintMarketId);
        //            lstrWhereClause = string.Format(" {0} > 0", lobjExchangeColumnMap.ColumnName_Map.Item(BaseColumnNames.Token));
        //        }
        //        return GetScripFactory.GetScrips(pintExchangeId, pintMarketId, lstrWhereClause);
        //    }
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    /// <param name="pintExchangeId"></param>
        //    /// <param name="pintMarketId"></param>
        //    /// <param name="pstrSymbol"></param>
        //    /// <param name="pstrName"></param>
        //    /// <param name="pstrInstrumentName"></param>
        //    /// <param name="pstrDisplayExpiryDate"></param>
        //    /// <param name="pstrOptionType"></param>
        //    /// <param name="pstrGroup"></param>
        //    /// <returns></returns>
        //    /// <remarks>This is meant for frmSearchContract SearchContracts. Do not change it for other requirments</remarks>
        //    public ScriptCollection GetScrips(int pintExchangeId, int pintMarketId, string pstrSymbol, string pstrName, string pstrInstrumentName, string pstrDisplayExpiryDate, string pstrOptionType, string pstrGroup)
        //    {

        //        ExchangeColumnName_Map lobjExchangeColumnMap = ColumnNameFactory.GetExchangeColumnMap(pintExchangeId, pintMarketId);
        //        StringBuilder lobjWhereClause = new StringBuilder();

        //        lobjWhereClause.Append(" 2 = 2 ");
        //        if (string.IsNullOrWhiteSpace(pstrSymbol) == false)
        //        {
        //            lobjWhereClause.Append(string.Format(" and {0} LIKE '{1}%'", lobjExchangeColumnMap.ColumnName_Map.Item(BaseColumnNames.Symbol), pstrSymbol));
        //        }
        //        if (string.IsNullOrWhiteSpace(pstrName) == false)
        //        {
        //            lobjWhereClause.Append(string.Format(" and {0} LIKE '{1}%'", lobjExchangeColumnMap.ColumnName_Map.Item(BaseColumnNames.Name), pstrName));
        //        }
        //        if (string.IsNullOrWhiteSpace(pstrInstrumentName) == false)
        //        {
        //            lobjWhereClause.Append(string.Format(" and {0} = '{1}'", lobjExchangeColumnMap.ColumnName_Map.Item(BaseColumnNames.InstrumentName), pstrInstrumentName));
        //        }
        //        if (string.IsNullOrWhiteSpace(pstrDisplayExpiryDate) == false)
        //        {
        //            lobjWhereClause.Append(string.Format(" and {0} = {1}", lobjExchangeColumnMap.ColumnName_Map.Item(BaseColumnNames.ExpiryDate), GetLongExpiryDateForScript(pintExchangeId, pstrDisplayExpiryDate, pintMarketId)));
        //        }
        //        if (string.IsNullOrWhiteSpace(pstrOptionType) == false)
        //        {
        //            if (pstrOptionType.Trim.ToUpper == "XX")
        //            {
        //                lobjWhereClause.Append(" and ( " + lobjExchangeColumnMap.ColumnName_Map.Item(BaseColumnNames.OptionType) + " = 'XX' OR " + lobjExchangeColumnMap.ColumnName_Map.Item(BaseColumnNames.OptionType) + "= '')");
        //            }
        //            else
        //            {
        //                lobjWhereClause.Append(string.Format(" and {0} = '{1}'", lobjExchangeColumnMap.ColumnName_Map.Item(BaseColumnNames.OptionType), pstrOptionType));
        //            }
        //        }
        //        if (string.IsNullOrWhiteSpace(pstrGroup) == false)
        //        {
        //            lobjWhereClause.Append(string.Format(" and {0} = '{1}'", lobjExchangeColumnMap.ColumnName_Map.Item(BaseColumnNames.Group), pstrGroup));
        //        }
        //        if (pintMarketId.ToString() == Constants.MKT_CURRENCY_VALUE)
        //        {
        //            lobjWhereClause.Append(string.Format(" Order By Substr({0},1,3),{1},{2} Desc", lobjExchangeColumnMap.ColumnName_Map.Item(BaseColumnNames.Symbol), lobjExchangeColumnMap.ColumnName_Map.Item(BaseColumnNames.InstrumentName), lobjExchangeColumnMap.ColumnName_Map.Item(BaseColumnNames.ExpiryDate)));
        //        }
        //        else
        //        {
        //            lobjWhereClause.Append(string.Format(" Order By {0},{1},{2} Desc", lobjExchangeColumnMap.ColumnName_Map.Item(BaseColumnNames.Symbol), lobjExchangeColumnMap.ColumnName_Map.Item(BaseColumnNames.ExpiryDate), lobjExchangeColumnMap.ColumnName_Map.Item(BaseColumnNames.StrikePrice)));
        //        }

        //        return GetScripFactory.GetScrips(pintExchangeId, pintMarketId, lobjWhereClause.ToString());
        //    }
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    /// <param name="pintExchangeId"></param>
        //    /// <param name="pintMarketId"></param>
        //    /// <param name="pstrSymbol"></param>
        //    /// <param name="pstrName"></param>
        //    /// <param name="pstrCompanyName"></param>
        //    /// <param name="pstrGroupName"></param>
        //    /// <param name="pstrSeries"></param>
        //    /// <returns></returns>
        //    /// <remarks>This is meant for frmSearchContract SearchSecurities. Do not change it for other requirments</remarks>
        //    public ScriptCollection GetSecurities(int pintExchangeId, int pintMarketId, string pstrSymbol, string pstrName, string pstrCompanyName, string pstrGroupName, string pstrSeries)
        //    {

        //        ExchangeColumnName_Map lobjExchangeColumnMap = ColumnNameFactory.GetExchangeColumnMap(pintExchangeId, pintMarketId);
        //        StringBuilder lobjWhereClause = new StringBuilder();

        //        lobjWhereClause.Append(" 2 = 2 ");

        //        if (string.IsNullOrWhiteSpace(pstrName) == false)
        //        {
        //            lobjWhereClause.Append(string.Format(" and {0} LIKE '{1}%'", lobjExchangeColumnMap.ColumnName_Map.Item(BaseColumnNames.Name), pstrName));
        //        }
        //        else
        //        {
        //            if (string.IsNullOrWhiteSpace(pstrSymbol) == false)
        //            {
        //                lobjWhereClause.Append(string.Format(" and ( {0} LIKE '{1}%'", lobjExchangeColumnMap.ColumnName_Map.Item(BaseColumnNames.Symbol), pstrSymbol));
        //                lobjWhereClause.Append(string.Format(" or {0} = '{1}' )", lobjExchangeColumnMap.ColumnName_Map.Item(BaseColumnNames.Token), pstrSymbol));
        //            }
        //            if (string.IsNullOrWhiteSpace(pstrCompanyName) == false)
        //            {
        //                lobjWhereClause.Append(string.Format(" and {0} LIKE '{1}%'", lobjExchangeColumnMap.ColumnName_Map.Item(BaseColumnNames.CompanyName), pstrCompanyName));
        //            }
        //        }

        //        if (string.IsNullOrWhiteSpace(pstrSeries) == false)
        //        {
        //            lobjWhereClause.Append(string.Format(" and {0} = '{1}'", lobjExchangeColumnMap.ColumnName_Map.Item(BaseColumnNames.Series), pstrSeries));
        //        }
        //        if (string.IsNullOrWhiteSpace(pstrGroupName) == false)
        //        {
        //            lobjWhereClause.Append(string.Format(" and {0} = '{1}'", lobjExchangeColumnMap.ColumnName_Map.Item(BaseColumnNames.Group), pstrGroupName));
        //        }

        //        lobjWhereClause.Append(string.Format(" and {0} > 0 ", lobjExchangeColumnMap.ColumnName_Map.Item(BaseColumnNames.Token)));
        //        lobjWhereClause.Append(string.Format(" Order By {0}", lobjExchangeColumnMap.ColumnName_Map.Item(BaseColumnNames.Symbol)));

        //        return GetScripFactory.GetScrips(pintExchangeId, pintMarketId, lobjWhereClause.ToString());
        //    }

        //    public ScriptCollection GetContracts(int pintExchangeId, int pintMarketId, string pstrStringExpiryDate, long pintAfterToken = 0)
        //    {

        //        ExchangeColumnName_Map lobjExchangeColumnMap = ColumnNameFactory.GetExchangeColumnMap(pintExchangeId, pintMarketId);
        //        StringBuilder lobjWhereClause = new StringBuilder();

        //        lobjWhereClause.Append(string.Format(" {0} = {1}", lobjExchangeColumnMap.ColumnName_Map.Item(BaseColumnNames.ExpiryDate), GetLongExpiryDateForScript(pintExchangeId, pstrStringExpiryDate.Trim, pintMarketId)));
        //        if (pintAfterToken > 0)
        //        {
        //            lobjWhereClause.Append(string.Format(" and {0} > {1}", lobjExchangeColumnMap.ColumnName_Map.Item(BaseColumnNames.Token), pintAfterToken));
        //        }
        //        lobjWhereClause.Append(" Order By " + lobjExchangeColumnMap.ColumnName_Map.Item(BaseColumnNames.InstrumentName));

        //        return GetScripFactory.GetScrips(pintExchangeId, pintMarketId, lobjWhereClause.ToString());
        //    }

        //    public ScriptCollection GetOptionContracts(int pintExchangeId, int pintMarketId, string pstrSymbol, string pstrStringExpiryDate, int pintStrikePrice = 0)
        //    {

        //        ExchangeColumnName_Map lobjExchangeColumnMap = ColumnNameFactory.GetExchangeColumnMap(pintExchangeId, pintMarketId);
        //        StringBuilder lobjWhereClause = new StringBuilder();

        //        lobjWhereClause.Append(string.Format(" {0} = '{1}'", lobjExchangeColumnMap.ColumnName_Map.Item(BaseColumnNames.Symbol), pstrSymbol));
        //        if (string.IsNullOrWhiteSpace(pstrStringExpiryDate) == false)
        //        {
        //            lobjWhereClause.Append(string.Format(" and {0} > {1}", lobjExchangeColumnMap.ColumnName_Map.Item(BaseColumnNames.ExpiryDate), GetLongExpiryDateForScript(pintExchangeId, pstrStringExpiryDate.Trim, pintMarketId)));
        //        }
        //        if (pintStrikePrice != 0)
        //        {
        //            lobjWhereClause.Append(string.Format(" and {0} = '{1}'", lobjExchangeColumnMap.ColumnName_Map.Item(BaseColumnNames.StrikePrice), pintStrikePrice.ToString()));
        //        }

        //        lobjWhereClause.Append(string.Format(" and {0} <> '' and {0} <> 'XX'", lobjExchangeColumnMap.ColumnName_Map.Item(BaseColumnNames.OptionType)));

        //        if (string.IsNullOrWhiteSpace(pstrStringExpiryDate) == false)
        //        {
        //            lobjWhereClause.Append(" Order By " + lobjExchangeColumnMap.ColumnName_Map.Item(BaseColumnNames.StrikePrice) + "," + lobjExchangeColumnMap.ColumnName_Map.Item(BaseColumnNames.ExpiryDate));
        //        }
        //        else
        //        {
        //            lobjWhereClause.Append(" Order By " + lobjExchangeColumnMap.ColumnName_Map.Item(BaseColumnNames.ExpiryDate));
        //        }

        //        return GetScripFactory.GetScrips(pintExchangeId, pintMarketId, lobjWhereClause.ToString());
        //    }

        //    public void Initialize(int pintExchangeId, string pstrMarketId)
        //    {
        //        if (pstrMarketId == null || pstrMarketId.Trim.Length == 0)
        //            pstrMarketId = "-1";
        //        InitializeSymbolsForScript(pintExchangeId, pstrMarketId);
        //        if ((pstrMarketId != null) && pstrMarketId >Constants.MKT_EQUITY_VALUE && pstrMarketId !=Constants.MKT_MF_VALUE)
        //        {
        //            InitializeExpiryDateCollection(pintExchangeId, pstrMarketId);
        //        }
        //    }

        //    private void CacheDistinctInstrumentNames(int pintExchangeId, int pintMarketId)
        //    {
        //        if (mobjInstrumentTypes.ContainsKey(pintExchangeId + "|" + pintMarketId) == false)
        //        {
        //            ExchangeColumnName_Map lobjExchangeColumnMap = ColumnNameFactory.GetExchangeColumnMap(pintExchangeId, pintMarketId);
        //            mobjInstrumentTypes.Add(pintExchangeId + "|" + pintMarketId, GetScripFactory.GetDistinct(pintExchangeId, pintMarketId, BaseColumnNames.InstrumentName));
        //        }
        //    }
        //    private void InitializeExpiryDateCollection(int pintExchangeId, int pintMarketId)
        //    {
        //        StringBuilder lstrSql = new StringBuilder();
        //        StringBuilder lobjWhereClause = new StringBuilder();
        //        StringBuilder lstrDisplayExpiryDate = null;
        //        StringBuilder lstrLongExpiryDate = null;

        //        string[] lstrLongExpiryDatesArray = null;
        //        string[] lstrDisplayExpiryDatesArray = null;

        //        ExchangeColumnName_Map lobjExchangeColumnMap = ColumnNameFactory.GetExchangeColumnMap(pintExchangeId, pintMarketId);
        //        lobjWhereClause.Append(" 2=2 ");


        //        string[] lobjDistinctExpiries = null;
        //        lobjDistinctExpiries = GetScripFactory.GetScrips(pintExchangeId, pintMarketId, new string[] {
        //        BaseColumnNames.ExpiryDate,
        //        BaseColumnNames.DisplayExpiryDate
        //    }, lobjWhereClause.ToString(), " Distinct ");

        //        if ((lobjDistinctExpiries != null) && lobjDistinctExpiries.Length > 0)
        //        {
        //            string[] lstrValues = null;
        //            for (int lintTemp = 0; lintTemp <= lobjDistinctExpiries.Length - 1; lintTemp++)
        //            {
        //                lstrValues = lobjDistinctExpiries[lintTemp].Split('|');
        //                if (lstrDisplayExpiryDate == null)
        //                {
        //                    lstrDisplayExpiryDate = new StringBuilder();
        //                    lstrDisplayExpiryDate.Append(GetFormatedExpiryDate(lstrValues[1]));
        //                }
        //                else
        //                {
        //                    lstrDisplayExpiryDate.Append("|").Append(GetFormatedExpiryDate(lstrValues[1]));
        //                }

        //                if (lstrLongExpiryDate == null)
        //                {
        //                    lstrLongExpiryDate = new StringBuilder();
        //                    lstrLongExpiryDate.Append(lstrValues(0));
        //                }
        //                else
        //                {
        //                    lstrLongExpiryDate.Append("|").Append(lstrValues(0));
        //                }
        //            }
        //        }

        //        if ((lstrLongExpiryDate != null))
        //        {
        //            lstrLongExpiryDatesArray = lstrLongExpiryDate.ToString().Split('|');
        //            mobjMyExpiryDates.AddLongExpiryList(lstrLongExpiryDatesArray, pintExchangeId, pintMarketId);
        //        }
        //        if ((lstrDisplayExpiryDate != null))
        //        {
        //            lstrDisplayExpiryDatesArray = lstrDisplayExpiryDate.ToString().Split('|');
        //            mobjMyExpiryDates.AddStringExpiryList(lstrDisplayExpiryDatesArray, pintExchangeId, pintMarketId);
        //        }
        //        //: For Fast Search
        //        mobjMyExpiryDates.AddExpiryList(lstrLongExpiryDatesArray, lstrDisplayExpiryDatesArray, pintExchangeId, pintMarketId);
        //    }
        //    public string[] GetExpiryDates(int pintExchangeId, int pintMarketId, string pstrSymbol, string pstrInstType, bool pblnGetLongExpiryDates, bool pblnSearchByName = false)
        //    {
        //        List<string> lstrDisplayExpiryDate = null;
        //        List<string> lstrLongExpiryDate = null;
        //        StringBuilder lstrSql = new StringBuilder();
        //        string[] lstrLongExpiryDatesArray = null;
        //        string[] lstrDisplayExpiryDatesArray = null;


        //        //: Sending from the Hash if Symbol is not sent
        //        if (string.IsNullOrWhiteSpace(pstrSymbol) && string.IsNullOrWhiteSpace(pstrInstType))
        //        {
        //            if (pblnGetLongExpiryDates == true)
        //            {
        //                lstrLongExpiryDatesArray = FetchScriptLongExpiryDates[pintExchangeId, pintMarketId];
        //                if ((lstrLongExpiryDatesArray != null))
        //                {
        //                    return lstrLongExpiryDatesArray;
        //                }
        //            }
        //            else
        //            {
        //                lstrDisplayExpiryDatesArray = FetchScriptExpiryDates[pintExchangeId, pintMarketId];
        //                if ((lstrDisplayExpiryDatesArray != null))
        //                {
        //                    return lstrDisplayExpiryDatesArray;
        //                }
        //            }
        //        }

        //        ExchangeColumnName_Map lobjExchangeColumnMap = ColumnNameFactory.GetExchangeColumnMap(pintExchangeId, pintMarketId);

        //        StringBuilder lobjWhereClause = new StringBuilder();
        //        lobjWhereClause.Append(" 2=2 ");
        //        if (string.IsNullOrWhiteSpace(pstrSymbol) == false)
        //        {
        //            if (pintExchangeId == Constants.EX_BSE_VALUE && (pintMarketId == Constants.MKT_DERIVATIVE_VALUE || pintMarketId == Constants.MKT_COMMODITIES_VALUE))
        //            {
        //                lobjWhereClause.Append(string.Format(" and {0} Like '{1}%'", lobjExchangeColumnMap.ColumnName_Map.Item(BaseColumnNames.Symbol), pstrSymbol));
        //            }
        //            else
        //            {
        //                lobjWhereClause.Append(string.Format(" and {0} = '{1}'", lobjExchangeColumnMap.ColumnName_Map.Item(BaseColumnNames.Symbol), pstrSymbol));
        //            }
        //        }
        //        if (string.IsNullOrWhiteSpace(pstrInstType) == false)
        //        {
        //            lobjWhereClause.Append(string.Format(" and {0} = '{1}'", lobjExchangeColumnMap.ColumnName_Map.Item(BaseColumnNames.InstrumentName), pstrInstType));
        //        }

        //        string[] lobjDistinctExpiries = null;
        //        lobjDistinctExpiries = GetScripFactory.GetScrips(pintExchangeId, pintMarketId, new string[] {
        //        BaseColumnNames.ExpiryDate,
        //        BaseColumnNames.DisplayExpiryDate
        //    }, lobjWhereClause.ToString(), " Distinct ");

        //        if ((lobjDistinctExpiries != null) && lobjDistinctExpiries.Length > 0)
        //        {
        //            string[] lstrValues = null;
        //            for (int lintTemp = 0; lintTemp <= lobjDistinctExpiries.Length - 1; lintTemp++)
        //            {
        //                lstrValues = lobjDistinctExpiries[lintTemp].Split("|");
        //                if (lstrDisplayExpiryDate == null)
        //                {
        //                    lstrDisplayExpiryDate = new List<string>();
        //                    lstrDisplayExpiryDate.Add(GetFormatedExpiryDate(lstrValues[1]));
        //                }
        //                else
        //                {
        //                    lstrDisplayExpiryDate.Add(GetFormatedExpiryDate(lstrValues[1]));
        //                }

        //                if (lstrLongExpiryDate == null)
        //                {
        //                    lstrLongExpiryDate = new List<string>();
        //                    lstrLongExpiryDate.Add(lstrValues[0]);
        //                }
        //                else
        //                {
        //                    lstrLongExpiryDate.Add(lstrValues[0]);
        //                }
        //            }
        //        }

        //        if (pblnGetLongExpiryDates == true)
        //        {
        //            if (lstrLongExpiryDate == null)
        //            {
        //                return new string[] { "" };
        //            }
        //            else
        //            {
        //                return lstrLongExpiryDate.ToArray();
        //            }

        //        }
        //        else
        //        {
        //            if (lstrDisplayExpiryDate == null)
        //            {
        //                return new string[] { "" };
        //            }
        //            else
        //            {
        //                return lstrDisplayExpiryDate.ToArray();
        //            }
        //        }
        //    }

        //    public string GetLongExpiryDateForScript(int pintExchangeId, string pstrStringExpiryDate, int pstrMarketId)
        //    {
        //        if (pstrMarketId == null || pstrMarketId.Trim.Length == 0)
        //            pstrMarketId = "-1";
        //        return mobjMyExpiryDates.GetLongExpiryDate(pstrStringExpiryDate, pintExchangeId, pstrMarketId);
        //    }

        //    public string GetStringExpiryDate(int pintExchangeId, long plngLongExpiryDate, string pstrMarketId)
        //    {
        //        if (pstrMarketId == null || pstrMarketId.Trim.Length == 0)
        //            pstrMarketId = "-1";

        //        return mobjMyExpiryDates.GetStringExpiryDate(plngLongExpiryDate, pintExchangeId, pstrMarketId);

        //    }

        //    public long GetMaxSeqIdForScript(int pintExchangeId, int pintMarketId)
        //    {
        //        return GetScripFactory.GetMax(pintExchangeId, pintMarketId, BaseColumnNames.SequenceID, null);
        //    }

        //    private string[] GetSymbolsForScript(int pintExchangeId, int pintMarketId, string pstrInstrumentName = "", string pstrCompanyName = "", bool pblnUseNameForSymbol = false, bool pblnIsSpread = false)
        //    {
        //        string[] lstrSymbolArray = null;

        //        if (pstrInstrumentName.Trim.Length == 0 && pstrCompanyName.Trim.Length == 0)
        //        {
        //            lstrSymbolArray = mobjMySymbols.GetSymbols(pintExchangeId, pintMarketId);
        //            if ((lstrSymbolArray != null))
        //            {
        //                return lstrSymbolArray;
        //            }
        //        }

        //        ExchangeColumnName_Map lobjExchangeColumnMap = ColumnNameFactory.GetExchangeColumnMap(pintExchangeId, pintMarketId);
        //        StringBuilder lobjWhereClause = new StringBuilder();

        //        lobjWhereClause.Append(" 2 = 2 ");
        //        if (string.IsNullOrWhiteSpace(pstrInstrumentName) == false)
        //        {
        //            lobjWhereClause.Append(string.Format(" and {0} Like '{1}%'", lobjExchangeColumnMap.ColumnName_Map.Item(BaseColumnNames.InstrumentName), pstrInstrumentName));
        //        }

        //        //: Since currently only BSE FO & Cur supports spread contracts 
        //        if (pblnIsSpread == true && pintExchangeId ==Constants.EX_BSE_VALUE && (pintMarketId ==Constants.MKT_DERIVATIVE_VALUE || pintMarketId ==Constants.MKT_CURRENCY_VALUE))
        //        {
        //            lobjWhereClause.Append(string.Format(" and {0} = '{1}'", lobjExchangeColumnMap.ColumnName_Map.Item(BaseColumnNames.ProductType), "S"));
        //        }

        //        if (string.IsNullOrWhiteSpace(pstrCompanyName) == false)
        //        {
        //            lobjWhereClause.Append(string.Format(" and {0} > 0", lobjExchangeColumnMap.ColumnName_Map.Item(BaseColumnNames.Token)));
        //            lobjWhereClause.Append(string.Format(" and {0} LIKE '{1}%'", lobjExchangeColumnMap.ColumnName_Map.Item(BaseColumnNames.CompanyName), pstrCompanyName));
        //        }

        //        lstrSymbolArray = GetScripFactory.GetDistinct(pintExchangeId, pintMarketId, BaseColumnNames.Symbol, false, lobjWhereClause.ToString());
        //        return lstrSymbolArray;
        //    }

        //    //:For Fetching Company names for securties search
        //    private string[] GetCompanyNameForScrips(int pintExchangeId, int pintMarketId, string pstrSymbol = "")
        //    {

        //        ExchangeColumnName_Map lobjExchangeColumnMap = ColumnNameFactory.GetExchangeColumnMap(pintExchangeId, pintMarketId);
        //        StringBuilder lobjWhereClause = new StringBuilder();

        //        lobjWhereClause.Append(" 2 = 2 ");
        //        lobjWhereClause.Append(string.Format(" and {0} > {1}", lobjExchangeColumnMap.ColumnName_Map.Item(BaseColumnNames.Token), 0));
        //        if (string.IsNullOrWhiteSpace(pstrSymbol) == false)
        //        {
        //            lobjWhereClause.Append(string.Format(" and {0} = '{1}'", lobjExchangeColumnMap.ColumnName_Map.Item(BaseColumnNames.Symbol), pstrSymbol));
        //        }

        //        return GetScripFactory.GetDistinct(pintExchangeId, pintMarketId, BaseColumnNames.CompanyName, false, lobjWhereClause.ToString());
        //    }

        //    private void InitializeSymbolsForScript(int pintExchangeId, int pintMarketId)
        //    {
        //        ExchangeColumnName_Map lobjExchangeColumnMap = ColumnNameFactory.GetExchangeColumnMap(pintExchangeId, pintMarketId);
        //        StringBuilder lobjWhereClause = new StringBuilder();
        //        string[] lstrSymbolArray = null;
        //        if (pintExchangeId ==Constants.EX_BSE_VALUE && pintMarketId ==Constants.MKT_EQUITY_VALUE)
        //        {
        //            lobjWhereClause.Append(" 2 = 2 AND BSCScripCode > 0 ");
        //        }
        //        else if (pintExchangeId ==Constants.EX_NSE_VALUE && pintMarketId ==Constants.MKT_EQUITY_VALUE)
        //        {
        //            lobjWhereClause.Append(" 2 = 2 AND NSCToken > 0 ");
        //        }
        //        else
        //        {
        //            lobjWhereClause.Append(" 2 = 2 ");
        //        }

        //        lstrSymbolArray = GetScripFactory.GetDistinct(pintExchangeId, pintMarketId, BaseColumnNames.Symbol, false, lobjWhereClause.ToString());

        //        mobjMySymbols.Add(lstrSymbolArray, pintExchangeId, pintMarketId);
        //    }


        //    public void InitializeSpreadTokenCollection()
        //    {
        //        ScriptCollection lobjScripCollection = default(ScriptCollection);
        //        StringBuilder lobjWhereClause = new StringBuilder();
        //        ExchangeColumnName_Map lobjExchangeColumnMap = default(ExchangeColumnName_Map);
        //        Dictionary<long, long> lobjScripIDTokenMap = default(Dictionary<long, long>);

        //        gobjSpreadScripIDTokenMap = new Dictionary<string, Dictionary<long, long>>();

        //        lobjExchangeColumnMap = ColumnNameFactory.GetExchangeColumnMap(Constants.EX_BSE_VALUE,Constants.MKT_DERIVATIVE_VALUE);
        //        lobjWhereClause.Append(string.Format(" {0} = '{1}'", lobjExchangeColumnMap.ColumnName_Map.Item(BaseColumnNames.ProductType), "S"));

        //        lobjScripCollection = GetScripFactory.GetScrips(Constants.EX_BSE_VALUE,Constants.MKT_DERIVATIVE_VALUE, lobjWhereClause.ToString());
        //        if ((lobjScripCollection != null) && lobjScripCollection.Count > 0)
        //        {
        //            lobjScripIDTokenMap = new Dictionary<long, long>();
        //            gobjSpreadScripIDTokenMap.Add(ExchangeColumnName_Map.GenerateKey(Constants.EX_BSE_VALUE,Constants.MKT_DERIVATIVE_VALUE), lobjScripIDTokenMap);
        //            for (int lintTemp = 0; lintTemp <= lobjScripCollection.Count - 1; lintTemp++)
        //            {
        //                if (lobjScripIDTokenMap.ContainsKey(lobjScripCollection.Item(lintTemp).Token) == false)
        //                {
        //                    lobjScripIDTokenMap.Add(lobjScripCollection.Item(lintTemp).Token, ((BSEContract)lobjScripCollection.Item(lintTemp)).ScriptID);
        //                }
        //            }
        //        }

        //        lobjWhereClause = new StringBuilder();
        //        lobjExchangeColumnMap = ColumnNameFactory.GetExchangeColumnMap(Constants.EX_BSE_VALUE,Constants.MKT_CURRENCY_VALUE);
        //        lobjWhereClause.Append(string.Format(" {0} = '{1}'", lobjExchangeColumnMap.ColumnName_Map.Item(BaseColumnNames.ProductType), "S"));

        //        lobjScripCollection = GetScripFactory.GetScrips(Constants.EX_BSE_VALUE,Constants.MKT_CURRENCY_VALUE, lobjWhereClause.ToString());
        //        if ((lobjScripCollection != null) && lobjScripCollection.Count > 0)
        //        {
        //            lobjScripIDTokenMap = new Dictionary<long, long>();
        //            gobjSpreadScripIDTokenMap.Add(ExchangeColumnName_Map.GenerateKey(Constants.EX_BSE_VALUE,Constants.MKT_CURRENCY_VALUE), lobjScripIDTokenMap);
        //            for (int lintTemp = 0; lintTemp <= lobjScripCollection.Count - 1; lintTemp++)
        //            {
        //                if (lobjScripIDTokenMap.ContainsKey(lobjScripCollection.Item(lintTemp).Token) == false)
        //                {
        //                    lobjScripIDTokenMap.Add(lobjScripCollection.Item(lintTemp).Token, ((BSECurrencyContract)lobjScripCollection.Item(lintTemp)).ScriptID);
        //                }
        //            }
        //        }

        //    }
        //    #endregion

        //    /// -----------------------------------------------------------------------------
        //    /// <summary>
        //    /// The function fetches security details for BSE security from the SECURITIES table in the Masters database, based on BSCScripCode which is a unique code for security in BSE exchange.
        //    /// </summary>
        //    /// <param name="pstrBseScripCode">
        //    /// Unique code given to scrip by BSE
        //    /// </param>
        //    /// <returns></returns>
        //    /// <remarks>
        //    /// </remarks>
        //    /// <history>
        //    ///     [prasad.mahimkar]    9/5/2007    Created
        //    /// </history>
        //    /// -----------------------------------------------------------------------------
        //    public SecurityBase GetSecurityWithBseScripCode(string pstrBseScripCode)
        //    {
        //        SecurityBase lobjSecurity = default(SecurityBase);
        //        ExchangeColumnName_Map lobjExchangeColumnMap = ColumnNameFactory.GetExchangeColumnMap(Constants.EX_BSE_VALUE,Constants.MKT_EQUITY_VALUE);
        //        StringBuilder lobjWhereClause = new StringBuilder();

        //        lobjWhereClause.Append(string.Format(" {0} = {1}", lobjExchangeColumnMap.ColumnName_Map.Item(BaseColumnNames.Token), pstrBseScripCode));
        //        lobjSecurity = GetScripFactory.GetScrip(Constants.EX_BSE_VALUE,Constants.MKT_EQUITY_VALUE, lobjWhereClause.ToString());
        //        if ((lobjSecurity != null))
        //        {
        //            return (SecurityBase)lobjSecurity;
        //        }
        //        else
        //        {
        //            return null;
        //        }
        //    }

        //    /// -----------------------------------------------------------------------------
        //    /// <summary>
        //    /// Fetches all the BSE Scrip Codes.
        //    /// </summary>
        //    /// <returns>
        //    /// String Array containing distinct BSE Scrip Codes (BSCScripCode).
        //    /// </returns>
        //    /// <remarks>
        //    /// This function is utilized for Symbol Smart Search. 
        //    /// A specialized control takes this array as its input.
        //    /// </remarks>
        //    /// <history>
        //    ///     [prasad.mahimkar]    9/5/2007    Created
        //    /// </history>
        //    /// -----------------------------------------------------------------------------
        //    public string[] GetBseScripCodes()
        //    {

        //        ExchangeColumnName_Map lobjExchangeColumnMap = ColumnNameFactory.GetExchangeColumnMap(Constants.EX_BSE_VALUE,Constants.MKT_EQUITY_VALUE);
        //        StringBuilder lobjWhereClause = new StringBuilder();

        //        lobjWhereClause.Append(string.Format(" {0} > {1}", lobjExchangeColumnMap.ColumnName_Map.Item(BaseColumnNames.Token), 0));

        //        return GetScripFactory.GetDistinct(Constants.EX_BSE_VALUE,Constants.MKT_EQUITY_VALUE, BaseColumnNames.Token, false, lobjWhereClause.ToString());
        //    }

        //    public SecurityBase GetSecurityWithISIN_No(int pstrExchangeId, string pstrISIN_No)
        //    {

        //        ScriptCollection lobjSecurities = default(ScriptCollection);
        //        ExchangeColumnName_Map lobjExchangeColumnMap = ColumnNameFactory.GetExchangeColumnMap(pstrExchangeId,Constants.MKT_EQUITY_VALUE);
        //        StringBuilder lobjWhereClause = new StringBuilder();

        //        lobjWhereClause.Append(string.Format(" {0} = '{1}'", lobjExchangeColumnMap.ColumnName_Map.Item(BaseColumnNames.ISIN), pstrISIN_No));
        //        lobjWhereClause.Append(string.Format(" and {0} > {1}", lobjExchangeColumnMap.ColumnName_Map.Item(BaseColumnNames.Token), 0));

        //        lobjSecurities = GetScripFactory.GetScrips(pstrExchangeId,Constants.MKT_EQUITY_VALUE, lobjWhereClause.ToString());
        //        if ((lobjSecurities != null) && lobjSecurities.Count > 0)
        //        {
        //            if (lobjSecurities.Count > 1)
        //            {
        //                bool lbln5SeriesScriptCodePresent = false;
        //                bool lbln6SeriesScriptCodePresent = false;
        //                //: Handling the case in which both 6 series and 5 series are active for the same ISIN Number
        //                foreach (IScript lobjScrip in lobjSecurities)
        //                {
        //                    if (lobjScrip.Token.ToString().StartsWith("6"))
        //                    {
        //                        lbln6SeriesScriptCodePresent = true;
        //                    }
        //                    else if (lobjScrip.Token.ToString().StartsWith("5"))
        //                    {
        //                        lbln5SeriesScriptCodePresent = true;
        //                    }
        //                }

        //                if (lbln5SeriesScriptCodePresent == true && lbln6SeriesScriptCodePresent == true)
        //                {
        //                    foreach (IScript lobjScrip in lobjSecurities)
        //                    {
        //                        if (lobjScrip.Token.ToString().StartsWith("5"))
        //                        {
        //                            return lobjScrip;
        //                        }
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                return lobjSecurities(0);
        //            }
        //            return null;
        //        }
        //        else
        //        {
        //            return null;
        //        }
        //    }

        //    #region " Helper Functions "

        //    public static string GetFormatedExpiryDate(string pstrExpiryDate)
        //    {
        //        //: Converting the Expiry date coming to the specified format
        //        System.DateTime ldtExpiryDate = default(System.DateTime);
        //        string lstrExpiryDate = null;
        //        try
        //        {
        //            ldtExpiryDate = pstrExpiryDate;
        //            lstrExpiryDate = Strings.Format(ldtExpiryDate, Constants.StockConstants.EXPIRY_DATE_FORMAT);
        //            //"dd MMM YYYY"
        //            return lstrExpiryDate;
        //        }
        //        catch (Exception ex)
        //        {
        //            //: Skipping the error occured due to conversion failure from string to date
        //            return pstrExpiryDate;
        //        }
        //    }

        //public static string GetActualStrikePrice(string pstrDisplayStrikePrice, int pintMarket = 0)
        //{
        //    //: Since the Contract Selector Control store the Shortened Value of StrikePrice
        //    string lstrStrikePrice = null;
        //    lstrStrikePrice = pstrDisplayStrikePrice;
        //    if (!string.IsNullOrEmpty(lstrStrikePrice) && lstrStrikePrice != "-1")
        //    {
        //        if (pintMarket != 0 && pintMarket == BowConstants.MKT_CURRENCY_VALUE)
        //        {
        //            lstrStrikePrice = Convert.ToString(Convert.ToDouble(pstrDisplayStrikePrice) * 10000);
        //        }
        //        else
        //        {
        //            lstrStrikePrice = Convert.ToString(Convert.ToDouble(pstrDisplayStrikePrice) * 100);
        //        }
        //    }
        //    return lstrStrikePrice;
        //}

        //    #endregion

        //    public string[] GetStrikePrices(int pintExchangeId, string pstrSymbol, string pstrInstrumentName, long plngExpiryDate, int pintMarketId, bool pblnUseNameForSymbol = false)
        //    {

        //        ExchangeColumnName_Map lobjExchangeColumnMap = ColumnNameFactory.GetExchangeColumnMap(pintExchangeId, pintMarketId);
        //        StringBuilder lobjWhereClause = new StringBuilder();
        //        string lstrDivideBy = null;

        //        lobjWhereClause.Append(" 2 = 2 ");
        //        if (string.IsNullOrWhiteSpace(pstrSymbol) == false)
        //        {
        //            lobjWhereClause.Append(string.Format(" and {0} = '{1}'", lobjExchangeColumnMap.ColumnName_Map.Item(BaseColumnNames.Symbol), pstrSymbol));
        //        }
        //        if (string.IsNullOrWhiteSpace(pstrInstrumentName) == false)
        //        {
        //            lobjWhereClause.Append(string.Format(" and {0} = '{1}'", lobjExchangeColumnMap.ColumnName_Map.Item(BaseColumnNames.InstrumentName), pstrInstrumentName));
        //        }
        //        if (plngExpiryDate > 0)
        //        {
        //            lobjWhereClause.Append(string.Format(" and {0} = {1}", lobjExchangeColumnMap.ColumnName_Map.Item(BaseColumnNames.ExpiryDate), plngExpiryDate.ToString()));
        //        }

        //        lobjWhereClause.Append(string.Format(" and {0} <> '' and {0} <>'XX'", lobjExchangeColumnMap.ColumnName_Map.Item(BaseColumnNames.OptionType)));

        //        if (pintMarketId ==Constants.MKT_CURRENCY_VALUE)
        //        {
        //            lstrDivideBy = "/10000";
        //        }
        //        else
        //        {
        //            lstrDivideBy = "/100";
        //        }

        //        return GetScripFactory.GetDistinct(pintExchangeId, pintMarketId, BaseColumnNames.StrikePrice, true, lobjWhereClause.ToString(), lstrDivideBy);

        //    }

        //    public string[] GetOptionTypes(string pintExchangeId, string pstrSymbol, string pstrInstrumentName, long plngExpiryDate, string pintMarketId, bool pblnShowSeriesNameInSymbol = false)
        //    {

        //        ExchangeColumnName_Map lobjExchangeColumnMap = ColumnNameFactory.GetExchangeColumnMap(Convert.ToInt32(pintExchangeId, Convert.ToInt32(pintMarketId));
        //        StringBuilder lobjWhereClause = new StringBuilder();

        //        lobjWhereClause.Append(string.Format(" {0} = '{1}'", lobjExchangeColumnMap.ColumnName_Map.Item(BaseColumnNames.Symbol), pstrSymbol));
        //        lobjWhereClause.Append(string.Format(" and {0} = '{1}'", lobjExchangeColumnMap.ColumnName_Map.Item(BaseColumnNames.InstrumentName), pstrInstrumentName));

        //        if (plngExpiryDate > 0)
        //        {
        //            lobjWhereClause.Append(string.Format(" and {0} = {1}", lobjExchangeColumnMap.ColumnName_Map.Item(BaseColumnNames.ExpiryDate), plngExpiryDate.ToString()));
        //        }

        //        return GetScripFactory.GetDistinct(pintExchangeId, pintMarketId, BaseColumnNames.OptionType, false, lobjWhereClause.ToString());
        //    }
        //}

        //struct MyExMktKey
        //{
        //    public int ExchangeId;
        //    public int MarketId;
        //}

        //[Serializable()]
        //public class MyExpiryDates
        //{
        //    public long LongExpiryDate;
        //    public string StringExpiryDate;
        //}
        [Serializable()]
        public class MyExpiryDatesHash
        {
        //    //: These hash has Key as ExchangeId - Integer
        //    //: Its Item/Object is another Hash which contains all MyExpiryDates for that exchanges
        //    //: The Inner Hash has Key as Long/String Expirydates which contains all MyExpiryDates for that exchanges
        //    //: The HashTable also contains one record with Key "ALL" for StringHash and -1 for LongHash - which has Arrayt of expirydates
        //    private Hashtable mobjLongExpiryDates = new Hashtable();

        //    private Hashtable mobjStringExpiryDates = new Hashtable();
        //    public void Add(MyExpiryDates pobjMyExpiryDates, string pstrExchangeId, string pstrMarketId = "-1")
        //    {
        //        Hashtable lobjTempExpiryDates = mobjLongExpiryDates[pstrExchangeId + "|" + pstrMarketId];
        //        if ((lobjTempExpiryDates != null))
        //        {
        //            if (lobjTempExpiryDates.Contains(pobjMyExpiryDates.LongExpiryDate) == false)
        //            {
        //                //: For adding in the LONG ExpiryDates Hash
        //                lobjTempExpiryDates.Add(pobjMyExpiryDates.LongExpiryDate, pobjMyExpiryDates);
        //                //:
        //                //: reusing the above hash 
        //                //: For ADDING in STRING ExpiryDatesHash
        //                lobjTempExpiryDates = mobjStringExpiryDates[pstrExchangeId + "|" + pstrMarketId];
        //                lobjTempExpiryDates.Add(pobjMyExpiryDates.StringExpiryDate.ToUpper(), pobjMyExpiryDates);
        //            }
        //        }
        //    }

        //    public void AddLongExpiryList(string[] pstrExpiryDatesList, int pstrExchangeId, int pstrMarketId = -1)
        //    {
        //        Hashtable lobjTempExpiryDates = mobjLongExpiryDates[pstrExchangeId + "|" + pstrMarketId];
        //        //: - ADDING to the StringArray
        //        if (lobjTempExpiryDates == null || lobjTempExpiryDates.Contains("ALL") == false)
        //        {
        //            lobjTempExpiryDates = new Hashtable();
        //            mobjLongExpiryDates.Add(pstrExchangeId + "|" + pstrMarketId, lobjTempExpiryDates);
        //            lobjTempExpiryDates.Add("ALL", pstrExpiryDatesList);
        //        }
        //        else
        //        {
        //            lobjTempExpiryDates.Item("ALL") = pstrExpiryDatesList;
        //        }
        //    }

        //    public void AddStringExpiryList(string[] pstrExpiryDatesList, int pstrExchangeId, int pstrMarketId = -1)
        //    {
        //        Hashtable lobjTempExpiryDates = mobjStringExpiryDates[pstrExchangeId + "|" + pstrMarketId];
        //        //: - ADDING to the StringArray
        //        if (lobjTempExpiryDates == null || lobjTempExpiryDates.Contains("ALL") == false)
        //        {
        //            lobjTempExpiryDates = new Hashtable();
        //            mobjStringExpiryDates.Add(pstrExchangeId + "|" + pstrMarketId, lobjTempExpiryDates);
        //            lobjTempExpiryDates.Add("ALL", pstrExpiryDatesList);
        //        }
        //        else
        //        {
        //            lobjTempExpiryDates.Item("ALL") = pstrExpiryDatesList;
        //        }
        //    }


        //    public void AddExpiryList(string[] pstrLongExpiryDatesList, string[] pstrStringExpiryDatesList, int pstrExchangeId, int pstrMarketId = -1)
        //    {
        //        if (pstrLongExpiryDatesList == null || pstrStringExpiryDatesList == null)
        //            return;

        //        Hashtable lobjLongExpiryDates = mobjLongExpiryDates[pstrExchangeId + "|" + pstrMarketId];
        //        Hashtable lobjStringExpiryDates = mobjStringExpiryDates[pstrExchangeId + "|" + pstrMarketId];
        //        int lintLength = 0;

        //        lintLength = pstrLongExpiryDatesList.Length;
        //        for (int lintCount = 0; lintCount <= lintLength - 1; lintCount++)
        //        {
        //            MyExpiryDates lobjMyExpiryDate = new MyExpiryDates();

        //            lobjMyExpiryDate.StringExpiryDate = pstrStringExpiryDatesList[lintCount];
        //            lobjMyExpiryDate.LongExpiryDate =Convert.ToInt64(pstrLongExpiryDatesList[lintCount]);

        //            if ((lobjLongExpiryDates != null))
        //            {
        //                if (lobjLongExpiryDates.Contains(lobjMyExpiryDate.LongExpiryDate) == false)
        //                {
        //                    lobjLongExpiryDates.Add(lobjMyExpiryDate.LongExpiryDate, lobjMyExpiryDate);
        //                }
        //            }
        //            if ((lobjStringExpiryDates != null))
        //            {
        //                if (lobjStringExpiryDates.Contains(lobjMyExpiryDate.StringExpiryDate.Trim().ToUpper()) == false)
        //                {
        //                    lobjStringExpiryDates.Add(lobjMyExpiryDate.StringExpiryDate.Trim().ToUpper(), lobjMyExpiryDate);
        //                }
        //            }
        //        }
        //    }

        //    public string GetStringExpiryDate(long plngLongExpiryDate, string pstrExchangeId, string pstrMarketId = "-1")
        //    {
        //        Hashtable lobjTempExpiryDate = mobjLongExpiryDates.Item(pstrExchangeId + "|" + pstrMarketId);
        //        if ((lobjTempExpiryDate != null))
        //        {
        //            MyExpiryDates lobjMyExpiryDates = null;
        //            lobjMyExpiryDates = lobjTempExpiryDate.Item(plngLongExpiryDate);
        //            if ((lobjMyExpiryDates != null))
        //            {
        //                return lobjMyExpiryDates.StringExpiryDate;
        //            }
        //        }
        //        return "";
        //    }

        //    public string GetLongExpiryDate(string pstrStringExpiryDate, string pstrExchangeId, string pstrMarketId = "-1")
        //    {
        //        Hashtable lobjTempExpiryDate = mobjStringExpiryDates.Item(pstrExchangeId + "|" + pstrMarketId);
        //        if ((lobjTempExpiryDate != null))
        //        {
        //            MyExpiryDates lobjMyExpiryDates = null;
        //            lobjMyExpiryDates = lobjTempExpiryDate.Item(pstrStringExpiryDate.Trim().ToUpper);
        //            if ((lobjMyExpiryDates != null))
        //            {
        //                return lobjMyExpiryDates.LongExpiryDate.ToString();
        //            }
        //        }
        //        return "-1";
        //    }

        //    public string[] GetStringExpiryDates(int pintExchangeId, int pintMarketId = -1)
        //    {
        //        string[] lstrExpiryDates = null;
        //        Hashtable lobjTempExpiryDate = mobjStringExpiryDates.Item(pintExchangeId + "|" + pintMarketId);
        //        if ((lobjTempExpiryDate != null))
        //        {
        //            lstrExpiryDates = lobjTempExpiryDate.Item("ALL");
        //        }
        //        return lstrExpiryDates;
        //    }

        //    public string[] GetLongExpiryDates(int pintExchangeId, int pintMarketId = -1)
        //    {
        //        string[] lstrExpiryDates = null;
        //        Hashtable lobjTempExpiryDate = mobjLongExpiryDates.Item(pintExchangeId + "|" + pintMarketId);
        //        if ((lobjTempExpiryDate != null))
        //        {
        //            lstrExpiryDates = lobjTempExpiryDate.Item("ALL");
        //        }
        //        return lstrExpiryDates;
        //    }

        }
        //[Serializable()]
        //class MySymbols
        //{
        //    //: Exchange
        //    //: |-Market
        //    //: |-Symbols
        //    //: When specific to Instu or Expiry then direct query

        //    private Hashtable mobjSymbols = new Hashtable();
        //    public void Add(string[] pstrSymbols, int pintExchangeId, int pintMarketId)
        //    {
        //        Hashtable lobjMarket = default(Hashtable);
        //        lobjMarket = mobjSymbols.Item(pintExchangeId);
        //        if (lobjMarket == null)
        //        {
        //            lobjMarket = new Hashtable();
        //            mobjSymbols.Add(pintExchangeId, lobjMarket);
        //        }
        //        string[] lstrSymbols = null;
        //        lstrSymbols = lobjMarket.Item(pintMarketId);
        //        if (lstrSymbols == null)
        //        {
        //            lobjMarket.Add(pintMarketId, pstrSymbols);
        //        }
        //        else
        //        {
        //            lobjMarket.Item(pintMarketId) = pstrSymbols;
        //        }
        //    }

        //    public string[] GetSymbols(int pintExchangeId, int pintMarketId = -1)
        //    {
        //        string[] lstrSymbols = null;
        //        Hashtable lobjMarket = default(Hashtable);

        //        lobjMarket = mobjSymbols.Item(pintExchangeId);
        //        if ((lobjMarket != null))
        //        {
        //            if (pintMarketId > 0)
        //            {
        //                lstrSymbols = lobjMarket.Item(pintMarketId);
        //            }
        //            else
        //            {
        //                //: Check whether only One List exist for that Exchange
        //                if (lobjMarket.Count == 1)
        //                {
        //                    foreach (int lintMarketId in lobjMarket.Keys)
        //                    {
        //                        lstrSymbols = lobjMarket.Item(lintMarketId);
        //                    }
        //                }
        //                else if (lobjMarket.Count > 1)
        //                {
        //                    Infrastructure.Logger.WriteLog(" MySymbols.GetSymbols() : MarketId is Zero. Multiple Symbols exists for this Exchange : " + pintExchangeId);
        //                }
        //                else if (lobjMarket.Count == 0)
        //                {
        //                    Infrastructure.Logger.WriteLog(" MySymbols.GetSymbols() : Zero Symbols for this Exchange : " + pintExchangeId);
        //                }
        //            }
        //        }
        //        return lstrSymbols;
        //    }

    }
    }
