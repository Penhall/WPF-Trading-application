using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonFrontEnd.Utility.Factory
{
    internal class RegisteredExchangeColumnMap
    {

        static internal Dictionary<string, ExchangeColumnName_Map> ExchangeColumnMapColl = new Dictionary<string, ExchangeColumnName_Map>();
        static RegisteredExchangeColumnMap()
        {
            InitializeFactory();
        }

        private static void InitializeFactory()
        {
            ExchangeColumnName_Map lobjExchangeColumnMap = default(ExchangeColumnName_Map);

            lobjExchangeColumnMap = new ExchangeColumnName_Map(1, 1, "BSESecurity", "Securities");
            lobjExchangeColumnMap.ColumnName_Map.Add("ID".ToUpper(), "SCID");
            lobjExchangeColumnMap.ColumnName_Map.Add("Symbol".ToUpper(), "SCSymbol");
            lobjExchangeColumnMap.ColumnName_Map.Add("Name".ToUpper(), "SCCompanyName");
            lobjExchangeColumnMap.ColumnName_Map.Add("SequenceId".ToUpper(), "SCSequenceId");
            lobjExchangeColumnMap.ColumnName_Map.Add("Token".ToUpper(), "BSCScripCode");
            lobjExchangeColumnMap.ColumnName_Map.Add("ISIN".ToUpper(), "ISIN");
            lobjExchangeColumnMap.ColumnName_Map.Add("Series".ToUpper(), "SCSeries");
            lobjExchangeColumnMap.ColumnName_Map.Add("CompanyName".ToUpper(), "SCCompanyName");
            lobjExchangeColumnMap.ColumnName_Map.Add("Group".ToUpper(), "BSCGroupName");
            lobjExchangeColumnMap.ColumnName_Map.Add("TickSize".ToUpper(), "BSCTickSize");
            lobjExchangeColumnMap.ColumnName_Map.Add("BoardLotQuantity".ToUpper(), "BSCBoardLot");
            lobjExchangeColumnMap.ColumnName_Map.Add("BSCFIELD4".ToUpper(), "BSCFIELD4");
            //added by KiranS 15-MAR-17, ENH:BSE Securities GSM.


            ExchangeColumnMapColl.Add(lobjExchangeColumnMap.Key, lobjExchangeColumnMap);

            lobjExchangeColumnMap = new ExchangeColumnName_Map(1, 2, "BSEContract", "BSEContracts");
            lobjExchangeColumnMap.ColumnName_Map.Add("ID".ToUpper(), "BCNID");
            lobjExchangeColumnMap.ColumnName_Map.Add("Token".ToUpper(), "BCNTOKEN");
            lobjExchangeColumnMap.ColumnName_Map.Add("InstrumentName".ToUpper(), "BCNINSTRUMENTTYPE");
            lobjExchangeColumnMap.ColumnName_Map.Add("Symbol".ToUpper(), "BCNASSETCODE");
            lobjExchangeColumnMap.ColumnName_Map.Add("ExpiryDate".ToUpper(), "BCNEXPIRYDATE");
            lobjExchangeColumnMap.ColumnName_Map.Add("StrikePrice".ToUpper(), "BCNSTRIKEPRICE");
            lobjExchangeColumnMap.ColumnName_Map.Add("OptionType".ToUpper(), "BCNOPTIONSTYLE");
            lobjExchangeColumnMap.ColumnName_Map.Add("DisplayExpiryDate".ToUpper(), "BCNDisplayExpiryDate");
            lobjExchangeColumnMap.ColumnName_Map.Add("BoardLotQuantity".ToUpper(), "BCNMarketLot");
            lobjExchangeColumnMap.ColumnName_Map.Add("TickSize".ToUpper(), "BCNTickSize");
            lobjExchangeColumnMap.ColumnName_Map.Add("Name".ToUpper(), "BCNSeriesName");
            lobjExchangeColumnMap.ColumnName_Map.Add("PQFactor".ToUpper(), "BCNPQFactor");
            lobjExchangeColumnMap.ColumnName_Map.Add("TradingUnit".ToUpper(), "BCNTradingUnit");
            lobjExchangeColumnMap.ColumnName_Map.Add("ProductType".ToUpper(), "BCNPRODUCTTYPE");
            lobjExchangeColumnMap.ColumnName_Map.Add("SequenceId".ToUpper(), "BCNSequenceId");
            lobjExchangeColumnMap.ColumnName_Map.Add("Group".ToUpper(), "BCNCapacityGroup");
            lobjExchangeColumnMap.ColumnName_Map.Add("InBannedPeriod".ToUpper(), "BCNInBannedPeriod");
            lobjExchangeColumnMap.ColumnName_Map.Add("Field3".ToUpper(), "BCNField3");
            //added by KiranS 13-FEB-17

            ExchangeColumnMapColl.Add(lobjExchangeColumnMap.Key, lobjExchangeColumnMap);

            lobjExchangeColumnMap = new ExchangeColumnName_Map(1, 3, "BSECommodityContract", "BSECommodityContracts");
            lobjExchangeColumnMap.ColumnName_Map.Add("ID".ToUpper(), "BCCID");
            lobjExchangeColumnMap.ColumnName_Map.Add("Token".ToUpper(), "BCCTOKEN");
            lobjExchangeColumnMap.ColumnName_Map.Add("InstrumentName".ToUpper(), "BCCINSTRUMENTNAME");
            lobjExchangeColumnMap.ColumnName_Map.Add("Symbol".ToUpper(), "BCCCONTRACTCODE");
            lobjExchangeColumnMap.ColumnName_Map.Add("ExpiryDate".ToUpper(), "BCCEXPIRYDATE");
            lobjExchangeColumnMap.ColumnName_Map.Add("StrikePrice".ToUpper(), "BCCSTRIKEPRICE");
            lobjExchangeColumnMap.ColumnName_Map.Add("OptionType".ToUpper(), "BCCOPTIONTYPE");
            lobjExchangeColumnMap.ColumnName_Map.Add("DisplayExpiryDate".ToUpper(), "BCCDisplayExpiryDate");
            lobjExchangeColumnMap.ColumnName_Map.Add("BoardLotQuantity".ToUpper(), "BCCBoardLot");
            lobjExchangeColumnMap.ColumnName_Map.Add("TickSize".ToUpper(), "BCCPriceTick");
            lobjExchangeColumnMap.ColumnName_Map.Add("Name".ToUpper(), "BCCContractCode");
            lobjExchangeColumnMap.ColumnName_Map.Add("PQFactor".ToUpper(), "BCCPQFactor");
            lobjExchangeColumnMap.ColumnName_Map.Add("TradingUnit".ToUpper(), "BCCTradingUnit");
            lobjExchangeColumnMap.ColumnName_Map.Add("SequenceId".ToUpper(), "BCCSequenceId");
            lobjExchangeColumnMap.ColumnName_Map.Add("Numerator".ToUpper(), "BCCGeneralNumerator");
            lobjExchangeColumnMap.ColumnName_Map.Add("Denominator".ToUpper(), "BCCGeneralDenominator");

            ExchangeColumnMapColl.Add(lobjExchangeColumnMap.Key, lobjExchangeColumnMap);

            lobjExchangeColumnMap = new ExchangeColumnName_Map(1, 4, "BSECurrencyContract", "NTACurrencyContracts");
            lobjExchangeColumnMap.ColumnName_Map.Add("ID".ToUpper(), "NTACCID");
            lobjExchangeColumnMap.ColumnName_Map.Add("Token".ToUpper(), "NTACCTOKEN");
            lobjExchangeColumnMap.ColumnName_Map.Add("ScriptID".ToUpper(), "NTACCScriptID");
            lobjExchangeColumnMap.ColumnName_Map.Add("InstrumentName".ToUpper(), "NTACCINSTRUMENTTYPE");
            lobjExchangeColumnMap.ColumnName_Map.Add("Symbol".ToUpper(), "NTACCASSETCODE");
            lobjExchangeColumnMap.ColumnName_Map.Add("ExpiryDate".ToUpper(), "NTACCEXPIRYDATE");
            lobjExchangeColumnMap.ColumnName_Map.Add("StrikePrice".ToUpper(), "NTACCSTRIKEPRICE");
            lobjExchangeColumnMap.ColumnName_Map.Add("OptionType".ToUpper(), "NTACCOPTIONSTYLE");
            lobjExchangeColumnMap.ColumnName_Map.Add("DisplayExpiryDate".ToUpper(), "NTACCDisplayExpiryDate");
            lobjExchangeColumnMap.ColumnName_Map.Add("BoardLotQuantity".ToUpper(), "NTACCMarketLot");
            lobjExchangeColumnMap.ColumnName_Map.Add("TickSize".ToUpper(), "NTACCTickSize");
            lobjExchangeColumnMap.ColumnName_Map.Add("Name".ToUpper(), "NTACCSeriesName");
            lobjExchangeColumnMap.ColumnName_Map.Add("PQFactor".ToUpper(), "NTACCPQFactor");
            lobjExchangeColumnMap.ColumnName_Map.Add("ProductType".ToUpper(), "NTACCProductType");
            lobjExchangeColumnMap.ColumnName_Map.Add("SequenceId".ToUpper(), "NTACCSequenceId");
            lobjExchangeColumnMap.ColumnName_Map.Add("Group".ToUpper(), "NTACCCapacityGroup");
            lobjExchangeColumnMap.ColumnName_Map.Add("Multiplier".ToUpper(), "NTACCMultiplier");

            ExchangeColumnMapColl.Add(lobjExchangeColumnMap.Key, lobjExchangeColumnMap);

            lobjExchangeColumnMap = new ExchangeColumnName_Map(1, 5, "MutualFund", "SCHEMEMASTER");

            lobjExchangeColumnMap.ColumnName_Map.Add("Token".ToUpper(), "UniqueNo");
            lobjExchangeColumnMap.ColumnName_Map.Add("Symbol".ToUpper(), "SchemeCode");
            lobjExchangeColumnMap.ColumnName_Map.Add("RTASchemeCode".ToUpper(), "RTASchemeCode");
            lobjExchangeColumnMap.ColumnName_Map.Add("AMCSchemeCode".ToUpper(), "AMCSchemeCode");
            lobjExchangeColumnMap.ColumnName_Map.Add("ISIN".ToUpper(), "ISIN");
            lobjExchangeColumnMap.ColumnName_Map.Add("AMCCode".ToUpper(), "AMCCode");
            lobjExchangeColumnMap.ColumnName_Map.Add("Name".ToUpper(), "SchemeName");
            lobjExchangeColumnMap.ColumnName_Map.Add("PurchaseTransactionmode".ToUpper(), "PurchaseTransactionmode");
            lobjExchangeColumnMap.ColumnName_Map.Add("MinimumPurchaseAmount".ToUpper(), "MinimumPurchaseAmount");
            lobjExchangeColumnMap.ColumnName_Map.Add("AdditionalPurchaseAmountMultiple".ToUpper(), "AdditionalPurchaseAmountMultiple");
            lobjExchangeColumnMap.ColumnName_Map.Add("MaximumPurchaseAmount".ToUpper(), "MaximumPurchaseAmount");
            lobjExchangeColumnMap.ColumnName_Map.Add("PurchaseAllowed".ToUpper(), "PurchaseAllowed");
            lobjExchangeColumnMap.ColumnName_Map.Add("PurchaseCutoffTime".ToUpper(), "PurchaseCutoffTime");
            lobjExchangeColumnMap.ColumnName_Map.Add("RedemptionTransactionMode".ToUpper(), "RedemptionTransactionMode");
            lobjExchangeColumnMap.ColumnName_Map.Add("MinimumRedemptionQty".ToUpper(), "MinimumRedemptionQty");
            lobjExchangeColumnMap.ColumnName_Map.Add("RedemptionQtyMultiplier".ToUpper(), "RedemptionQtyMultiplier");
            lobjExchangeColumnMap.ColumnName_Map.Add("MaximumRedemptionQty".ToUpper(), "MaximumRedemptionQty");
            lobjExchangeColumnMap.ColumnName_Map.Add("RedemptionAllowed".ToUpper(), "RedemptionAllowed");
            lobjExchangeColumnMap.ColumnName_Map.Add("RedemptionCutoffTime".ToUpper(), "RedemptionCutoffTime");
            lobjExchangeColumnMap.ColumnName_Map.Add("RTAAgentCode".ToUpper(), "RTAAgentCode");
            lobjExchangeColumnMap.ColumnName_Map.Add("AMCActiveFlag".ToUpper(), "AMCActiveFlag");
            lobjExchangeColumnMap.ColumnName_Map.Add("DividendReinvestmentFlag".ToUpper(), "DividendReinvestmentFlag");
            lobjExchangeColumnMap.ColumnName_Map.Add("SchemeType".ToUpper(), "SchemeType");
            lobjExchangeColumnMap.ColumnName_Map.Add("SIPFLAG".ToUpper(), "SIPFLAG");
            lobjExchangeColumnMap.ColumnName_Map.Add("STPFLAG".ToUpper(), "STPFLAG");
            lobjExchangeColumnMap.ColumnName_Map.Add("SWPFLAG".ToUpper(), "SWPFLAG");
            lobjExchangeColumnMap.ColumnName_Map.Add("SETTLEMENTTYPE".ToUpper(), "SETTLEMENTTYPE");
            lobjExchangeColumnMap.ColumnName_Map.Add("PurchaseAmountMultiplier".ToUpper(), "PurchaseAmountMultiplier");
            lobjExchangeColumnMap.ColumnName_Map.Add("NAV".ToUpper(), "NAV");


            ExchangeColumnMapColl.Add(lobjExchangeColumnMap.Key, lobjExchangeColumnMap);

            lobjExchangeColumnMap = new ExchangeColumnName_Map(1, 6, "SLBContract", "SLBContracts");
            lobjExchangeColumnMap.ColumnName_Map.Add("ID".ToUpper(), "SCID");
            lobjExchangeColumnMap.ColumnName_Map.Add("Token".ToUpper(), "SCScripCode");
            lobjExchangeColumnMap.ColumnName_Map.Add("InstrumentName".ToUpper(), "SCInstrumentName");
            lobjExchangeColumnMap.ColumnName_Map.Add("Symbol".ToUpper(), "SCScripId");
            lobjExchangeColumnMap.ColumnName_Map.Add("ExpiryDate".ToUpper(), "SCLongExpiryDate");
            lobjExchangeColumnMap.ColumnName_Map.Add("StrikePrice".ToUpper(), "SCStrikePrice");
            lobjExchangeColumnMap.ColumnName_Map.Add("OptionType".ToUpper(), "SCOptionType");
            lobjExchangeColumnMap.ColumnName_Map.Add("DisplayExpiryDate".ToUpper(), "SCDisplayExpiryDate");
            lobjExchangeColumnMap.ColumnName_Map.Add("BoardLotQuantity".ToUpper(), "SCMarketLot");
            lobjExchangeColumnMap.ColumnName_Map.Add("TickSize".ToUpper(), "SCTickSize");
            lobjExchangeColumnMap.ColumnName_Map.Add("Name".ToUpper(), "SCProductCode");
            lobjExchangeColumnMap.ColumnName_Map.Add("RollOver".ToUpper(), "SCRollOver");
            lobjExchangeColumnMap.ColumnName_Map.Add("SequenceId".ToUpper(), "SCProductId");

            ExchangeColumnMapColl.Add(lobjExchangeColumnMap.Key, lobjExchangeColumnMap);

            lobjExchangeColumnMap = new ExchangeColumnName_Map(1, 8, "BOND", "WDMContracts");
            lobjExchangeColumnMap.ColumnName_Map.Add("ID".ToUpper(), "WDMID");
            lobjExchangeColumnMap.ColumnName_Map.Add("Token".ToUpper(), "WDMScriptCode");
            lobjExchangeColumnMap.ColumnName_Map.Add("InstrumentName".ToUpper(), "WDMInstrumentName");
            lobjExchangeColumnMap.ColumnName_Map.Add("Symbol".ToUpper(), "WDMSymbol");
            lobjExchangeColumnMap.ColumnName_Map.Add("ExpiryDate".ToUpper(), "WDMLongExpiryDate");
            lobjExchangeColumnMap.ColumnName_Map.Add("StrikePrice".ToUpper(), "WDMStrikePrice");
            lobjExchangeColumnMap.ColumnName_Map.Add("OptionType".ToUpper(), "WDMOptionType");
            lobjExchangeColumnMap.ColumnName_Map.Add("DisplayExpiryDate".ToUpper(), "WDMDisplayExpiryDate");
            lobjExchangeColumnMap.ColumnName_Map.Add("BoardLotQuantity".ToUpper(), "WDMMarketLot");
            lobjExchangeColumnMap.ColumnName_Map.Add("TickSize".ToUpper(), "WDMTickSize");
            lobjExchangeColumnMap.ColumnName_Map.Add("Name".ToUpper(), "WDMSymbol");
            lobjExchangeColumnMap.ColumnName_Map.Add("Segment".ToUpper(), "WDMSegment");
            lobjExchangeColumnMap.ColumnName_Map.Add("SettlementType".ToUpper(), "WDMSettlementType");
            lobjExchangeColumnMap.ColumnName_Map.Add("ListingCategory".ToUpper(), "WDMListingCatagory");
            lobjExchangeColumnMap.ColumnName_Map.Add("Standard".ToUpper(), "WDMInstrumentStandard");
            lobjExchangeColumnMap.ColumnName_Map.Add("BondCategory".ToUpper(), "WDMBondCategory");
            lobjExchangeColumnMap.ColumnName_Map.Add("GSecType".ToUpper(), "WDMGSecType");
            lobjExchangeColumnMap.ColumnName_Map.Add("ISIN".ToUpper(), "WDMISIN");
            lobjExchangeColumnMap.ColumnName_Map.Add("FaceValue".ToUpper(), "WDMFaceValue");
            lobjExchangeColumnMap.ColumnName_Map.Add("IssueDate".ToUpper(), "WDMIssueDate");
            lobjExchangeColumnMap.ColumnName_Map.Add("Coupon".ToUpper(), "WDMCoupon");
            lobjExchangeColumnMap.ColumnName_Map.Add("IPPeriod".ToUpper(), "WDMIPPeriod");
            lobjExchangeColumnMap.ColumnName_Map.Add("IPDuration".ToUpper(), "WDMIPDuration");
            lobjExchangeColumnMap.ColumnName_Map.Add("IPPrevIPDate".ToUpper(), "WDMPrevIPDate");
            lobjExchangeColumnMap.ColumnName_Map.Add("IPNextIPDate".ToUpper(), "WDMNextIPDate");
            lobjExchangeColumnMap.ColumnName_Map.Add("AccruedIntrest".ToUpper(), "WDMAccruedIntrest");
            lobjExchangeColumnMap.ColumnName_Map.Add("LastTradeingDate".ToUpper(), "WDMLastTradingDate");
            lobjExchangeColumnMap.ColumnName_Map.Add("WDMExpiryDate".ToUpper(), "WDMExpiryDate");
            lobjExchangeColumnMap.ColumnName_Map.Add("Status".ToUpper(), "WDMActiveSuspendFlag");
            lobjExchangeColumnMap.ColumnName_Map.Add("ShutPeriodStartDate".ToUpper(), "WDMShutPeriodStartDate");
            lobjExchangeColumnMap.ColumnName_Map.Add("ShutPeriodEndDate".ToUpper(), "WDMShutPeriodEndDate");
            lobjExchangeColumnMap.ColumnName_Map.Add("RecordDate".ToUpper(), "WDMRecordDate");
            lobjExchangeColumnMap.ColumnName_Map.Add("FirstCallDate".ToUpper(), "WDMFirstCallDate");
            lobjExchangeColumnMap.ColumnName_Map.Add("FirstPutDate".ToUpper(), "WDMFirstPutDate");
            lobjExchangeColumnMap.ColumnName_Map.Add("CrisilRating".ToUpper(), "WDMCrisilRating");
            lobjExchangeColumnMap.ColumnName_Map.Add("CareRating".ToUpper(), "WDMCareRating");
            lobjExchangeColumnMap.ColumnName_Map.Add("IcraRating".ToUpper(), "WDMIcraRating");
            lobjExchangeColumnMap.ColumnName_Map.Add("FitchRating".ToUpper(), "WDMFitchRating");

            ExchangeColumnMapColl.Add(lobjExchangeColumnMap.Key, lobjExchangeColumnMap);

            lobjExchangeColumnMap = new ExchangeColumnName_Map(1, 9, "BOND", "WDMContractsT0");
            lobjExchangeColumnMap.ColumnName_Map.Add("ID".ToUpper(), "WDMID");
            lobjExchangeColumnMap.ColumnName_Map.Add("Token".ToUpper(), "WDMScriptCode");
            lobjExchangeColumnMap.ColumnName_Map.Add("InstrumentName".ToUpper(), "WDMInstrumentName");
            lobjExchangeColumnMap.ColumnName_Map.Add("Symbol".ToUpper(), "WDMSymbol");
            lobjExchangeColumnMap.ColumnName_Map.Add("ExpiryDate".ToUpper(), "WDMLongExpiryDate");
            lobjExchangeColumnMap.ColumnName_Map.Add("StrikePrice".ToUpper(), "WDMStrikePrice");
            lobjExchangeColumnMap.ColumnName_Map.Add("OptionType".ToUpper(), "WDMOptionType");
            lobjExchangeColumnMap.ColumnName_Map.Add("DisplayExpiryDate".ToUpper(), "WDMDisplayExpiryDate");
            lobjExchangeColumnMap.ColumnName_Map.Add("BoardLotQuantity".ToUpper(), "WDMMarketLot");
            lobjExchangeColumnMap.ColumnName_Map.Add("TickSize".ToUpper(), "WDMTickSize");
            lobjExchangeColumnMap.ColumnName_Map.Add("Name".ToUpper(), "WDMSymbol");
            lobjExchangeColumnMap.ColumnName_Map.Add("Segment".ToUpper(), "WDMSegment");
            lobjExchangeColumnMap.ColumnName_Map.Add("SettlementType".ToUpper(), "WDMSettlementType");
            lobjExchangeColumnMap.ColumnName_Map.Add("ListingCategory".ToUpper(), "WDMListingCatagory");
            lobjExchangeColumnMap.ColumnName_Map.Add("Standard".ToUpper(), "WDMInstrumentStandard");
            lobjExchangeColumnMap.ColumnName_Map.Add("BondCategory".ToUpper(), "WDMBondCategory");
            lobjExchangeColumnMap.ColumnName_Map.Add("GSecType".ToUpper(), "WDMGSecType");
            lobjExchangeColumnMap.ColumnName_Map.Add("ISIN".ToUpper(), "WDMISIN");
            lobjExchangeColumnMap.ColumnName_Map.Add("FaceValue".ToUpper(), "WDMFaceValue");
            lobjExchangeColumnMap.ColumnName_Map.Add("IssueDate".ToUpper(), "WDMIssueDate");
            lobjExchangeColumnMap.ColumnName_Map.Add("Coupon".ToUpper(), "WDMCoupon");
            lobjExchangeColumnMap.ColumnName_Map.Add("IPPeriod".ToUpper(), "WDMIPPeriod");
            lobjExchangeColumnMap.ColumnName_Map.Add("IPDuration".ToUpper(), "WDMIPDuration");
            lobjExchangeColumnMap.ColumnName_Map.Add("IPPrevIPDate".ToUpper(), "WDMPrevIPDate");
            lobjExchangeColumnMap.ColumnName_Map.Add("IPNextIPDate".ToUpper(), "WDMNextIPDate");
            lobjExchangeColumnMap.ColumnName_Map.Add("AccruedIntrest".ToUpper(), "WDMAccruedIntrest");
            lobjExchangeColumnMap.ColumnName_Map.Add("LastTradeingDate".ToUpper(), "WDMLastTradingDate");
            lobjExchangeColumnMap.ColumnName_Map.Add("WDMExpiryDate".ToUpper(), "WDMExpiryDate");
            lobjExchangeColumnMap.ColumnName_Map.Add("Status".ToUpper(), "WDMActiveSuspendFlag");
            lobjExchangeColumnMap.ColumnName_Map.Add("ShutPeriodStartDate".ToUpper(), "WDMShutPeriodStartDate");
            lobjExchangeColumnMap.ColumnName_Map.Add("ShutPeriodEndDate".ToUpper(), "WDMShutPeriodEndDate");
            lobjExchangeColumnMap.ColumnName_Map.Add("RecordDate".ToUpper(), "WDMRecordDate");
            lobjExchangeColumnMap.ColumnName_Map.Add("FirstCallDate".ToUpper(), "WDMFirstCallDate");
            lobjExchangeColumnMap.ColumnName_Map.Add("FirstPutDate".ToUpper(), "WDMFirstPutDate");
            lobjExchangeColumnMap.ColumnName_Map.Add("CrisilRating".ToUpper(), "WDMCrisilRating");
            lobjExchangeColumnMap.ColumnName_Map.Add("CareRating".ToUpper(), "WDMCareRating");
            lobjExchangeColumnMap.ColumnName_Map.Add("IcraRating".ToUpper(), "WDMIcraRating");
            lobjExchangeColumnMap.ColumnName_Map.Add("FitchRating".ToUpper(), "WDMFitchRating");


            ExchangeColumnMapColl.Add(lobjExchangeColumnMap.Key, lobjExchangeColumnMap);

            lobjExchangeColumnMap = new ExchangeColumnName_Map(1, 10, "ITPContract", "ITPContracts");
            lobjExchangeColumnMap.ColumnName_Map.Add("ID".ToUpper(), "ITPID");
            lobjExchangeColumnMap.ColumnName_Map.Add("Token".ToUpper(), "ITPScripCode");
            lobjExchangeColumnMap.ColumnName_Map.Add("InstrumentName".ToUpper(), "ITPInstrumenName");
            lobjExchangeColumnMap.ColumnName_Map.Add("Symbol".ToUpper(), "ITPScripID");
            lobjExchangeColumnMap.ColumnName_Map.Add("ExpiryDate".ToUpper(), "ITPExpiryDate");
            lobjExchangeColumnMap.ColumnName_Map.Add("StrikePrice".ToUpper(), "ITPStrikePrice");
            lobjExchangeColumnMap.ColumnName_Map.Add("OptionType".ToUpper(), "ITPOptionType");
            lobjExchangeColumnMap.ColumnName_Map.Add("Group".ToUpper(), "ITPGroupName");
            lobjExchangeColumnMap.ColumnName_Map.Add("DisplayExpiryDate".ToUpper(), "ITPDisplayExpiryDate");
            lobjExchangeColumnMap.ColumnName_Map.Add("BoardLotQuantity".ToUpper(), "ITPMarketLot");
            lobjExchangeColumnMap.ColumnName_Map.Add("TickSize".ToUpper(), "ITPTickSize");
            lobjExchangeColumnMap.ColumnName_Map.Add("Name".ToUpper(), "ITPScripName");
            lobjExchangeColumnMap.ColumnName_Map.Add("SequenceId".ToUpper(), "ITPSequenceID");


            ExchangeColumnMapColl.Add(lobjExchangeColumnMap.Key, lobjExchangeColumnMap);

            lobjExchangeColumnMap = new ExchangeColumnName_Map(2, 1, "NSESecurity", "Securities");
            lobjExchangeColumnMap.ColumnName_Map.Add("ID".ToUpper(), "SCID");
            lobjExchangeColumnMap.ColumnName_Map.Add("Symbol".ToUpper(), "SCSymbol");
            lobjExchangeColumnMap.ColumnName_Map.Add("Name".ToUpper(), "SCCompanyName");
            lobjExchangeColumnMap.ColumnName_Map.Add("SequenceId".ToUpper(), "SCSequenceId");
            lobjExchangeColumnMap.ColumnName_Map.Add("Token".ToUpper(), "NSCToken");
            lobjExchangeColumnMap.ColumnName_Map.Add("ISIN".ToUpper(), "ISIN");
            lobjExchangeColumnMap.ColumnName_Map.Add("Series".ToUpper(), "SCSeries");
            lobjExchangeColumnMap.ColumnName_Map.Add("CompanyName".ToUpper(), "SCCompanyName");
            lobjExchangeColumnMap.ColumnName_Map.Add("TickSize".ToUpper(), "NSCTickSize");
            lobjExchangeColumnMap.ColumnName_Map.Add("BoardLotQuantity".ToUpper(), "NSCBoardLotQuantity");
            lobjExchangeColumnMap.ColumnName_Map.Add("NSCIssueRate".ToUpper(), "BSCFIELD4");
            //added by KiranS 12-APR-17, ENH:NSE Securities GSM.

            ExchangeColumnMapColl.Add(lobjExchangeColumnMap.Key, lobjExchangeColumnMap);

            lobjExchangeColumnMap = new ExchangeColumnName_Map(2, 2, "NSEContract", "Contracts");
            lobjExchangeColumnMap.ColumnName_Map.Add("ID".ToUpper(), "NCNID");
            lobjExchangeColumnMap.ColumnName_Map.Add("Token".ToUpper(), "NCNTOKEN");
            lobjExchangeColumnMap.ColumnName_Map.Add("InstrumentName".ToUpper(), "NCNINSTRUMENTNAME");
            lobjExchangeColumnMap.ColumnName_Map.Add("Symbol".ToUpper(), "NCNSYMBOL");
            lobjExchangeColumnMap.ColumnName_Map.Add("ExpiryDate".ToUpper(), "NCNEXPIRYDATE");
            lobjExchangeColumnMap.ColumnName_Map.Add("StrikePrice".ToUpper(), "NCNSTRIKEPRICE");
            lobjExchangeColumnMap.ColumnName_Map.Add("OptionType".ToUpper(), "NCNOPTIONTYPE");
            lobjExchangeColumnMap.ColumnName_Map.Add("DisplayExpiryDate".ToUpper(), "DisplayExpiryDate");
            lobjExchangeColumnMap.ColumnName_Map.Add("BoardLotQuantity".ToUpper(), "NCNBoardLotQuantity");
            lobjExchangeColumnMap.ColumnName_Map.Add("TickSize".ToUpper(), "NCNTickSize");
            lobjExchangeColumnMap.ColumnName_Map.Add("Name".ToUpper(), "NCNName");
            lobjExchangeColumnMap.ColumnName_Map.Add("SequenceId".ToUpper(), "NCNSequenceId");
            lobjExchangeColumnMap.ColumnName_Map.Add("ProductType".ToUpper(), "NCNPRODUCTTYPE");

            ExchangeColumnMapColl.Add(lobjExchangeColumnMap.Key, lobjExchangeColumnMap);


            lobjExchangeColumnMap = new ExchangeColumnName_Map(2, 4, "NSECurrencyContract", "NseCurrencyContracts");
            lobjExchangeColumnMap.ColumnName_Map.Add("ID".ToUpper(), "NCCID");
            lobjExchangeColumnMap.ColumnName_Map.Add("Token".ToUpper(), "NCCCONTRACTTOKEN");
            lobjExchangeColumnMap.ColumnName_Map.Add("InstrumentName".ToUpper(), "NCCINSTRUMENT");
            lobjExchangeColumnMap.ColumnName_Map.Add("Symbol".ToUpper(), "NCCSYMBOL");
            lobjExchangeColumnMap.ColumnName_Map.Add("ExpiryDate".ToUpper(), "NCCEXPIRYDATE");
            lobjExchangeColumnMap.ColumnName_Map.Add("StrikePrice".ToUpper(), "NCCSTRIKEPRICE");
            lobjExchangeColumnMap.ColumnName_Map.Add("OptionType".ToUpper(), "NCCOPTIONTYPE");
            lobjExchangeColumnMap.ColumnName_Map.Add("DisplayExpiryDate".ToUpper(), "NCCDisplayExpiryDate");
            lobjExchangeColumnMap.ColumnName_Map.Add("BoardLotQuantity".ToUpper(), "NCCBoardLotQty");
            lobjExchangeColumnMap.ColumnName_Map.Add("TickSize".ToUpper(), "NCCTICKSIZE");
            lobjExchangeColumnMap.ColumnName_Map.Add("Name".ToUpper(), "NCCSYMBOL");
            lobjExchangeColumnMap.ColumnName_Map.Add("SequenceId".ToUpper(), "NCCSequenceId");
            lobjExchangeColumnMap.ColumnName_Map.Add("Multiplier".ToUpper(), "NCCMultiplier");

            ExchangeColumnMapColl.Add(lobjExchangeColumnMap.Key, lobjExchangeColumnMap);


            lobjExchangeColumnMap = new ExchangeColumnName_Map(3, 3, "NCDEXContract", "NCDEXContracts");
            lobjExchangeColumnMap.ColumnName_Map.Add("ID".ToUpper(), "NCDID");
            lobjExchangeColumnMap.ColumnName_Map.Add("Token".ToUpper(), "NCDTOKEN");
            lobjExchangeColumnMap.ColumnName_Map.Add("InstrumentName".ToUpper(), "NCDINSTRUMENTNAME");
            lobjExchangeColumnMap.ColumnName_Map.Add("Symbol".ToUpper(), "NCDSYMBOL");
            lobjExchangeColumnMap.ColumnName_Map.Add("ExpiryDate".ToUpper(), "NCDEXPIRYDATE");
            lobjExchangeColumnMap.ColumnName_Map.Add("StrikePrice".ToUpper(), "NCDSTRIKEPRICE");
            lobjExchangeColumnMap.ColumnName_Map.Add("OptionType".ToUpper(), "NCDOPTIONTYPE");
            lobjExchangeColumnMap.ColumnName_Map.Add("DisplayExpiryDate".ToUpper(), "NCDDisplayExpiryDate");
            lobjExchangeColumnMap.ColumnName_Map.Add("BoardLotQuantity".ToUpper(), "NCDBoardLotQuantity");
            lobjExchangeColumnMap.ColumnName_Map.Add("TickSize".ToUpper(), "NCDTickSize");
            lobjExchangeColumnMap.ColumnName_Map.Add("Name".ToUpper(), "NCDName");
            lobjExchangeColumnMap.ColumnName_Map.Add("SequenceId".ToUpper(), "NCDSequenceId");
            lobjExchangeColumnMap.ColumnName_Map.Add("Numerator".ToUpper(), "NCDPriceNumerator");
            lobjExchangeColumnMap.ColumnName_Map.Add("Denominator".ToUpper(), "NCDPriceDenominator");

            ExchangeColumnMapColl.Add(lobjExchangeColumnMap.Key, lobjExchangeColumnMap);

            lobjExchangeColumnMap = new ExchangeColumnName_Map(4, 3, "NMCEContract", "NMCEContracts");
            lobjExchangeColumnMap.ColumnName_Map.Add("ID".ToUpper(), "NMID");
            lobjExchangeColumnMap.ColumnName_Map.Add("Token".ToUpper(), "NMTOKEN");
            lobjExchangeColumnMap.ColumnName_Map.Add("InstrumentName".ToUpper(), "NMINSTRUMENTTYPE");
            lobjExchangeColumnMap.ColumnName_Map.Add("Symbol".ToUpper(), "NMASSETCODE");
            lobjExchangeColumnMap.ColumnName_Map.Add("ExpiryDate".ToUpper(), "NMEXPIRYDATE");
            lobjExchangeColumnMap.ColumnName_Map.Add("StrikePrice".ToUpper(), "NMSTRIKEPRICE");
            lobjExchangeColumnMap.ColumnName_Map.Add("OptionType".ToUpper(), "NMOPTIONSTYLE");
            lobjExchangeColumnMap.ColumnName_Map.Add("DisplayExpiryDate".ToUpper(), "NMDisplayExpiryDate");
            lobjExchangeColumnMap.ColumnName_Map.Add("BoardLotQuantity".ToUpper(), "NMMarketLot");
            lobjExchangeColumnMap.ColumnName_Map.Add("TickSize".ToUpper(), "NMTickSize");
            lobjExchangeColumnMap.ColumnName_Map.Add("Name".ToUpper(), "NMSeriesName");
            lobjExchangeColumnMap.ColumnName_Map.Add("PQFactor".ToUpper(), "NMPQFactor");
            lobjExchangeColumnMap.ColumnName_Map.Add("TradingUnit".ToUpper(), "NMTradingUnit");
            lobjExchangeColumnMap.ColumnName_Map.Add("SequenceId".ToUpper(), "NMSequenceId");

            ExchangeColumnMapColl.Add(lobjExchangeColumnMap.Key, lobjExchangeColumnMap);

            lobjExchangeColumnMap = new ExchangeColumnName_Map(5, 3, "MCXContract", "MCXContracts");
            lobjExchangeColumnMap.ColumnName_Map.Add("ID".ToUpper(), "MCID");
            lobjExchangeColumnMap.ColumnName_Map.Add("Token".ToUpper(), "MCTOKEN");
            lobjExchangeColumnMap.ColumnName_Map.Add("InstrumentName".ToUpper(), "MCINSTRUMENTNAME");
            lobjExchangeColumnMap.ColumnName_Map.Add("Symbol".ToUpper(), "MCCONTRACTCODE");
            lobjExchangeColumnMap.ColumnName_Map.Add("ExpiryDate".ToUpper(), "MCEXPIRYDATE");
            lobjExchangeColumnMap.ColumnName_Map.Add("StrikePrice".ToUpper(), "MCSTRIKEPRICE");
            lobjExchangeColumnMap.ColumnName_Map.Add("OptionType".ToUpper(), "MCOPTIONTYPE");
            lobjExchangeColumnMap.ColumnName_Map.Add("DisplayExpiryDate".ToUpper(), "MCDisplayExpiryDate");
            lobjExchangeColumnMap.ColumnName_Map.Add("BoardLotQuantity".ToUpper(), "MCBoardLot");
            lobjExchangeColumnMap.ColumnName_Map.Add("TickSize".ToUpper(), "MCPriceTick");
            lobjExchangeColumnMap.ColumnName_Map.Add("Name".ToUpper(), "MCContractCode");
            lobjExchangeColumnMap.ColumnName_Map.Add("PQFactor".ToUpper(), "MCPQFactor");
            lobjExchangeColumnMap.ColumnName_Map.Add("TradingUnit".ToUpper(), "MCTradingUnit");
            lobjExchangeColumnMap.ColumnName_Map.Add("SequenceId".ToUpper(), "MCSequenceId");
            lobjExchangeColumnMap.ColumnName_Map.Add("Numerator".ToUpper(), "MCGeneralNumerator");
            lobjExchangeColumnMap.ColumnName_Map.Add("Denominator".ToUpper(), "MCGeneralDenominator");
            lobjExchangeColumnMap.ColumnName_Map.Add("QuotationUnit".ToUpper(), "MCQuotationUnit");
            //Vijayalakshmi - 29Sep2016 - MCX changes suggested by Anjul
            lobjExchangeColumnMap.ColumnName_Map.Add("TradingUnitDisplay".ToUpper(), "MCField3");
            //Vijayalakshmi - 07Oct2016 - MCX changes suggested by Avinash
            lobjExchangeColumnMap.ColumnName_Map.Add("QuotationMetric".ToUpper(), "MCQuotationMetric");
            //Vijayalakshmi - 07Oct2016 - MCX changes suggested by Avinash

            ExchangeColumnMapColl.Add(lobjExchangeColumnMap.Key, lobjExchangeColumnMap);

            lobjExchangeColumnMap = new ExchangeColumnName_Map(5, 4, "MCXCurrencyContract", "MCXCurrencyContracts");
            lobjExchangeColumnMap.ColumnName_Map.Add("ID".ToUpper(), "MCCID");
            lobjExchangeColumnMap.ColumnName_Map.Add("Token".ToUpper(), "MCCTOKEN");
            lobjExchangeColumnMap.ColumnName_Map.Add("InstrumentName".ToUpper(), "MCCINSTRUMENTNAME");
            lobjExchangeColumnMap.ColumnName_Map.Add("Symbol".ToUpper(), "MCCCONTRACTCODE");
            lobjExchangeColumnMap.ColumnName_Map.Add("ExpiryDate".ToUpper(), "MCCEXPIRYDATE");
            lobjExchangeColumnMap.ColumnName_Map.Add("StrikePrice".ToUpper(), "MCCSTRIKEPRICE");
            lobjExchangeColumnMap.ColumnName_Map.Add("OptionType".ToUpper(), "MCCOPTIONTYPE");
            lobjExchangeColumnMap.ColumnName_Map.Add("DisplayExpiryDate".ToUpper(), "MCCDisplayExpiryDate");
            lobjExchangeColumnMap.ColumnName_Map.Add("BoardLotQuantity".ToUpper(), "MCCBoardLot");
            lobjExchangeColumnMap.ColumnName_Map.Add("TickSize".ToUpper(), "MCCPriceTick");
            lobjExchangeColumnMap.ColumnName_Map.Add("Name".ToUpper(), "MCCContractCode");
            lobjExchangeColumnMap.ColumnName_Map.Add("PQFactor".ToUpper(), "MCCPQFactor");
            lobjExchangeColumnMap.ColumnName_Map.Add("SequenceId".ToUpper(), "MCCSequenceId");


            ExchangeColumnMapColl.Add(lobjExchangeColumnMap.Key, lobjExchangeColumnMap);

            lobjExchangeColumnMap = new ExchangeColumnName_Map(8, 3, "DGCXContract", "DGCXContracts");
            lobjExchangeColumnMap.ColumnName_Map.Add("ID".ToUpper(), "DGCXID");
            lobjExchangeColumnMap.ColumnName_Map.Add("Token".ToUpper(), "DGCXOrderbookId");
            lobjExchangeColumnMap.ColumnName_Map.Add("InstrumentName".ToUpper(), "DGCXInstrument");
            lobjExchangeColumnMap.ColumnName_Map.Add("Symbol".ToUpper(), "DGCXContractIdentifire");
            lobjExchangeColumnMap.ColumnName_Map.Add("ExpiryDate".ToUpper(), "DGCXIntExpiryDate");
            lobjExchangeColumnMap.ColumnName_Map.Add("StrikePrice".ToUpper(), "DGCXStrikePrice");
            lobjExchangeColumnMap.ColumnName_Map.Add("OptionType".ToUpper(), "DGCXOptionType");
            lobjExchangeColumnMap.ColumnName_Map.Add("DisplayExpiryDate".ToUpper(), "DGCXDisplayExpiryDate");
            lobjExchangeColumnMap.ColumnName_Map.Add("BoardLotQuantity".ToUpper(), "DGCXTradablelot");
            lobjExchangeColumnMap.ColumnName_Map.Add("TickSize".ToUpper(), "DGCXPriceTick");
            lobjExchangeColumnMap.ColumnName_Map.Add("Name".ToUpper(), "DGCXSegmentName");
            lobjExchangeColumnMap.ColumnName_Map.Add("SequenceId".ToUpper(), "DGCXSequenceId");

            ExchangeColumnMapColl.Add(lobjExchangeColumnMap.Key, lobjExchangeColumnMap);

            lobjExchangeColumnMap = new ExchangeColumnName_Map(-1, -1, "", "");
            //: NullObjectPattern
            lobjExchangeColumnMap.ColumnName_Map.Add("ID".ToUpper(), "");
            lobjExchangeColumnMap.ColumnName_Map.Add("Token".ToUpper(), "");
            lobjExchangeColumnMap.ColumnName_Map.Add("InstrumentName".ToUpper(), "");
            lobjExchangeColumnMap.ColumnName_Map.Add("Symbol".ToUpper(), "");
            lobjExchangeColumnMap.ColumnName_Map.Add("Series".ToUpper(), "");
            lobjExchangeColumnMap.ColumnName_Map.Add("ExpiryDate".ToUpper(), "");
            lobjExchangeColumnMap.ColumnName_Map.Add("StrikePrice".ToUpper(), "");
            lobjExchangeColumnMap.ColumnName_Map.Add("OptionType".ToUpper(), "");
            lobjExchangeColumnMap.ColumnName_Map.Add("DisplayExpiryDate".ToUpper(), "");
            lobjExchangeColumnMap.ColumnName_Map.Add("BoardLotQuantity".ToUpper(), "");
            lobjExchangeColumnMap.ColumnName_Map.Add("TickSize".ToUpper(), "");
            lobjExchangeColumnMap.ColumnName_Map.Add("Name".ToUpper(), "");
            lobjExchangeColumnMap.ColumnName_Map.Add("SequenceId".ToUpper(), "");

            ExchangeColumnMapColl.Add(lobjExchangeColumnMap.Key, lobjExchangeColumnMap);
        }
    }
}
