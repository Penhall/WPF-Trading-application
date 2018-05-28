using CommonFrontEnd.Model.Order;
using CommonFrontEnd.SharedMemories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using CommonFrontEnd.Global;
using System.Data;
using CommonFrontEnd.Model.Trade;

namespace CommonFrontEnd.Common
{
    public abstract class Validations
    {
        static UtilityLoginDetails objUtilityLoginDetails = UtilityLoginDetails.GETInstance;

        #region OrderValdiation

        public static bool ValidateOrder(string rate, string trgPrice, OrderModel omodel, ref string Validate_Message, int DecimalPoint, bool ByPassMktProt = false)
        {
            try
            {
                if (omodel != null)
                {
                    //Validate Buy/Sell Indicator
                    if (string.IsNullOrEmpty(omodel.BuySellIndicator))
                    {
                        Validate_Message = "BUY/SELL Not Selected";
                        return false;
                    }

                    //Check ScripCode is empty and Matched against any ScripCode 
                    Validate_Message = ValdiateScripDetails(omodel.ScripCode, omodel.Symbol, omodel.Exchange, omodel.Segment);
                    if (Validate_Message.Trim().ToUpper() != "Success".ToUpper())
                    {
                        //Validate_Message =  Validate_Message;
                        return false;
                    }


                    //Validate Price only if the order type is not Market
#if BOW
                    if (!string.IsNullOrEmpty(omodel.OrderType) && omodel.OrderType.ToUpper() != Enumerations.Order.OrderTypes.MARKET.ToString())
                    {
                        //Validate Price
                        if (!ValidatePrice(rate, trgPrice, omodel, ref Validate_Message, DecimalPoint))
                            return false;

                    }
#elif TWS
                    if (!string.IsNullOrEmpty(omodel.OrderType))
                    {
                        if (omodel.OrderType == "K" || omodel.OrderType == "L" || omodel.OrderType.ToUpper() == "P" && !ByPassMktProt)
                        {
                            //Validate Price
                            if (!ValidatePrice(rate, trgPrice, omodel, ref Validate_Message, DecimalPoint))
                                return false;
                        }

                    }
#endif

                    //Validate Quantity
                    Validate_Message = ValidateVolume(omodel.OriginalQty, 9999999999L, 1, 0, false);
                    if (Validate_Message.Trim().ToUpper() != "Success".ToUpper())
                    {
                        Validate_Message = "Quantity : " + Validate_Message;
                        return false;
                    }

                    //Total Qty should be Multiple of Mkt Lot
                    if (omodel.MarketLot != 0)
                    {
                        if (omodel.OriginalQty % omodel.MarketLot != 0)
                        {
                            Validate_Message = "Quantity should be a multiple of Mkt lot";
                            return false;
                        }
                    }

                    else
                    {
                        Validate_Message = "Market Lot is Zero";
                        return false;
                    }

                    //Validate Reveal Qty
                    Validate_Message = ValidateRevlQty(omodel.RevealQty, 9999999999L, 1, 0, false);
                    if (Validate_Message.Trim().ToUpper() != "Success".ToUpper())
                    {
                        Validate_Message = "RevealQty : " + Validate_Message;
                        return false;
                    }

                    // Reveal Qty volume should not be greater than actual volume
                    if (omodel.RevealQty > omodel.OriginalQty)
                    {
                        Validate_Message = "Disclosed quantity can't be greater than actual quantity";
                        return false;
                    }

                    // if Reveal Qty is zero then send actual quantity as reveal qty
                    if (omodel.RevealQty == 0)
                    {
                        omodel.RevealQty = omodel.OriginalQty;
                    }

                    //Reveal Qty should be Greater than or Equal to 10% of Total QTY
                    if (omodel.RevealQty < (omodel.OriginalQty * 10) / 100)
                    {
                        Validate_Message = "Reveal quantity should > or =  to 10 % of total quantity";
                        return false;
                    }

                    //Reveal Qty should be Multiple of Mkt Lot
                    if (omodel.RevealQty % omodel.MarketLot != 0)
                    {
                        Validate_Message = " Reveal quantity is Not a Multiple of Mkt Lot";
                        return false;
                    }

                    //Check Market Protection is Not a number
#if BOW
                    if (!string.IsNullOrEmpty(omodel.OrderType) && (omodel.OrderType.ToUpper() == Enumerations.Order.OrderTypes.MARKET.ToString() || omodel.OrderType.ToUpper() == Enumerations.Order.OrderTypes.STOPLOSS.ToString()))
#elif TWS
                    if (!string.IsNullOrEmpty(omodel.OrderType) && (omodel.OrderType.ToUpper() == "G" || omodel.OrderType.ToUpper() == "P") && !ByPassMktProt)
#endif
                    {
                        if (!string.IsNullOrEmpty(omodel.ProtectionPercentage))
                        {
                            if (!Regex.IsMatch(omodel.ProtectionPercentage.Trim(), @"^[1-9]\d*(\.\d+)?$"))
                            {
                                if (omodel.ProtectionPercentage == "0" || omodel.ProtectionPercentage == string.Empty)
                                    Validate_Message = "Please Provide Market Protection";

                                ////Check Number after 2 decimal point
                                //if (omodel.ProtectionPercentage.Contains(".") && omodel.ProtectionPercentage.IndexOf(".") >= 0)
                                //    Validate_Message = "Cannot have Decimal Digits.";

                                //Check Number after 2 decimal point
                                if (omodel.ProtectionPercentage.Contains(".") && omodel.ProtectionPercentage.Substring(omodel.ProtectionPercentage.IndexOf(".") + 1).Length > 2)
                                    Validate_Message = "Market Protection More than 2 decimal places!";

                                else
                                    Validate_Message = "Invalid Market Protection";

                                return false;
                            }
                            //MarketProtection can't be greater than  99.99%
                            else if (Convert.ToDecimal(omodel.ProtectionPercentage.Trim()) > 9999)
                            {
                                Validate_Message = "MarketProtection can't be greater than  99.99%";
                                return false;
                            }
                        }

                        else
                        {
                            Validate_Message = "Please Provide Market Protection";
                            return false;
                        }
                    }

                    //Check if retention type is null or empty
                    if (string.IsNullOrEmpty(omodel.OrderRetentionStatus))
                    {
                        Validate_Message = "Retention Type Not selected";
                        return false;
                    }

                    // Client ID Shouldn't be Balnk
                    if (string.IsNullOrEmpty(omodel.ClientId))
                    {
                        Validate_Message = "No ClientId entered Or Invalid Client ID";
                        return false;
                    }

                    //Client ID length Shouldn't be  greater than 11
                    if (omodel.ClientId.Trim().Length > 11)
                    {
                        Validate_Message = "ClientId should not be more than 11 characters";
                        return false;
                    }

                    //Client ID should be OWN when Client type is selected as OWN
                    //TODO: Write Logic


                    //Check if the selected segment is Equity
                    // if (omodel.Segment == Enumerations.Segment.Equity.ToString())
                    //  {
                    //TODO: Write Validations 
                    //if (omodel.OrderType.ToUpper() == ConstantMessages.ORDER_TYPE_OCO)
                    //{
                    //    long llngTriggerPrice = Convert.ToInt64(Convert.ToDouble(omodel.TriggerPrice) * 100);
                    //    if (llngTriggerPrice > 0)
                    //    {
                    //        if (omodel.Price > 0)
                    //        {
                    //            long llngPrice = Convert.ToInt64(Convert.ToDouble(omodel.Price) * 100);
                    //            if (llngPrice != 0)
                    //            {
                    //                if (omodel.BuySellIndicator.ToUpper() == Enumerations.Side.Buy.ToString().ToUpper())
                    //                {
                    //                    if ((llngTriggerPrice > llngPrice))
                    //                    {
                    //                        Validate_Message = "Stop Price should be less than the Limit Price.";
                    //                        //pstrError = lstrMessage;
                    //                        //pintPropertyId = omodel.TriggerPrice;
                    //                        return false;
                    //                    }
                    //                }
                    //                else
                    //                {
                    //                    if ((llngTriggerPrice < llngPrice))
                    //                    {
                    //                        Validate_Message = "Stop Price should be greater than the Limit Price.";
                    //                        //pstrError = lstrMessage;
                    //                        return false;
                    //                    }
                    //                }
                    //            }
                    //        }
                    //        else
                    //        {
                    //            Validate_Message = "Please enter a valid Limit Price and Stop Price";
                    //            return false;
                    //        }
                    //    }
                    //}

                    //Validate Rate 
                    //if (!Regex.IsMatch(omodel.Price.ToString(), @"^[1-9]\d*(\.\d+)?$"))
                    //{
                    //    if (omodel.ProtectionPercentage == "0" || omodel.ProtectionPercentage == string.Empty)
                    //        Validate_Message = "Please Provide Market Protection";
                    //    else
                    //        Validate_Message = "Invalid Market Protection";

                    //    return false;
                    //}


                    // }

                    //Business Validations
                    //Check for Institutional/NRI Client
                    if (!string.IsNullOrEmpty(omodel.ClientType) && omodel.ClientType.Trim().ToUpper() == Enumerations.Order.ClientTypes.INST.ToString().Trim().ToUpper())
                    {
                        if (omodel.ScripCode >= 600000 && omodel.ScripCode <= 700000)
                        {
                            MessageBoxResult res = MessageBox.Show("Only Institutional / NRI clients are permitted to trade in this scrip.\nDo you wish to continue?", "Warning Message", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                            if (res == MessageBoxResult.Yes)
                                return true;
                            else
                                return false;

                        }
                    }
                    if (omodel.Exchange == Enumerations.Order.Exchanges.BSE.ToString())
                    {
                        //Check for Z group,T Group 
                        if (omodel.Segment == Enumerations.Order.ScripSegment.Equity.ToString())//omodel.Exchange == Enumerations.Order.Exchanges.BSE.ToString())
                        {
                            //if (omodel.ScripCode != 0 && MasterSharedMemory.objMastertxtDictBaseBSE[omodel.ScripCode].GroupName.Trim().Contains("Z"))//BSE
                            //{
                            //    MessageBoxResult res = MessageBox.Show("Please note that the Scrip is in 'Z' GROUP and Trades would be settled on 'Trade to Trade' basis.\nDo you wish to continue?", "Warning Message", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                            //    if (res == MessageBoxResult.Yes)
                            //        return true;
                            //    else
                            //        return false;
                            //}

                            //if (omodel.ScripCode != 0 && MasterSharedMemory.objMastertxtDictBaseBSE[omodel.ScripCode].GroupName.Trim().Contains("T"))//BSE
                            //{
                            //    MessageBoxResult res = MessageBox.Show("Please note that the Scrip is in 'T' GROUP and Trades would be settled on 'Trade to Trade' basis.\nDo you wish to continue?", "Warning Message", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                            //    if (res == MessageBoxResult.Yes)
                            //        return true;
                            //    else
                            //        return false;
                            //}

                            //Check GSM Level for Group SS and ST
                            //if (omodel.ScripCode != 0 && (MasterSharedMemory.objMastertxtDictBaseBSE[omodel.ScripCode].GroupName.Trim().ToUpper().Contains("SS") || MasterSharedMemory.objMastertxtDictBaseBSE[omodel.ScripCode].GroupName.Trim().ToUpper().Contains("ST")))//BSE
                            //{
                            //    //TODO: Write resp messages for Level 0, level 1 and Greater than 1
                            //    if (MasterSharedMemory.objMastertxtDictBaseBSE[omodel.ScripCode].Filler2_GSM == 0)//BSE
                            //    {


                            //        //MessageBoxResult res = MessageBox.Show("Please note that the Scrip is in 'T' GROUP and Trades would be settled on 'Trade to Trade' basis.\nDo you wish to continue?", "Warning Message", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                            //        //if (res == MessageBoxResult.Yes)
                            //        //    return true;
                            //        //else
                            //        //    return false;
                            //    }

                            //    else if (MasterSharedMemory.objMastertxtDictBaseBSE[omodel.ScripCode].Filler2_GSM == 1)//BSE
                            //    {


                            //        //MessageBoxResult res = MessageBox.Show("Please note that the Scrip is in 'T' GROUP and Trades would be settled on 'Trade to Trade' basis.\nDo you wish to continue?", "Warning Message", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                            //        //if (res == MessageBoxResult.Yes)
                            //        //    return true;
                            //        //else
                            //        //    return false;
                            //    }

                            //    else if (MasterSharedMemory.objMastertxtDictBaseBSE[omodel.ScripCode].Filler2_GSM > 1)//BSE
                            //    {


                            //        //MessageBoxResult res = MessageBox.Show("Please note that the Scrip is in 'T' GROUP and Trades would be settled on 'Trade to Trade' basis.\nDo you wish to continue?", "Warning Message", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                            //        //if (res == MessageBoxResult.Yes)
                            //        //    return true;
                            //        //else
                            //        //    return false;
                            //    }
                            //}

                        }
                        //Check if the selected segment is Debt
                        else if (omodel.Segment == Enumerations.Order.ScripSegment.Debt.ToString())
                        {
                            //TODO: Write Validations 
                        }

                        //Check if the selected segment is Currency
                        else if (omodel.Segment == Enumerations.Order.ScripSegment.Currency.ToString() || omodel.Segment == Enumerations.Order.ScripSegment.Derivative.ToString())
                        {
                            //TODO: Write Validations 

                            //For institution/ client type INST or SPCL
                            if (!string.IsNullOrEmpty(omodel.ClientType) && (omodel.ClientType.Trim().ToUpper() == Enumerations.Order.ClientTypes.INST.ToString().Trim().ToUpper() || omodel.ClientType.Trim().ToUpper() == Enumerations.Order.ClientTypes.SPLCLI.ToString().Trim().ToUpper()))
                            {
                                //If CPCode is space
                                //if (!string.IsNullOrEmpty(omodel.ParticipantCode))
                                //{
                                //    Validate_Message = "Invalid Participant Code";
                                //    return false;
                                //}

                                //If CPCode more than 12 characters
                                if (omodel.ParticipantCode.Length > 12)
                                {

                                    Validate_Message = "Participant Code more than 12 characters";
                                    return false;
                                }
                            }
                        }

                        //Check if the selected segment is Derivative
                        else if (omodel.Segment == Enumerations.Order.ScripSegment.Derivative.ToString())
                        {
                            //TODO: Write Validations 
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
                Validate_Message = "Error In Validation of order";
                return false;
            }

            return true;
        }

        private static string ValdiateScripDetails(long scripCode, string symbol, string exchange, string segment)
        {
            string lstrMessage = "Success";

            //Check ScripCode and ScripID is zero and empty respectively
            if (scripCode == 0)
                return "Invalid ScripCode";

            if (string.IsNullOrEmpty(symbol))
                return "Invalid ScripID";

            if (exchange == Enumerations.Order.Exchanges.BSE.ToString())
            {
                if (segment == Enumerations.Order.ScripSegment.Equity.ToString())
                {
                    //Check if the passed ScripCode and Symbol matches
                    if (symbol.Trim().ToUpper() != MasterSharedMemory.objMastertxtDictBaseBSE[scripCode].ScripId.Trim().ToUpper())//BSE
                        return "Please check Scrip Id and Scrip Code";
                }
                else if (segment == Enumerations.Order.ScripSegment.Derivative.ToString())
                {
                    //Check if the passed ScripCode and Symbol matches
                    if (symbol.Trim().ToUpper() != MasterSharedMemory.objMasterDerivativeDictBaseBSE[scripCode].ScripId.Trim().ToUpper())//BSE
                        return "Please check Scrip Id and Scrip Code";
                }
                else if (segment == Enumerations.Order.ScripSegment.Currency.ToString())
                {
                    //Check if the passed ScripCode and Symbol matches
                    if (symbol.Trim().ToUpper() != MasterSharedMemory.objMasterCurrencyDictBaseBSE[scripCode].ScripId.Trim().ToUpper())//BSE
                        return "Please check Scrip Id and Scrip Code";
                }

                if (segment == Enumerations.Order.ScripSegment.ITP.ToString())
                {

                    //Check if the passed ScripCode and Symbol matches
                    if (symbol.Trim().ToUpper() != MasterSharedMemory.objITPMasterDict[Convert.ToInt32(scripCode)].ITPScripID.Trim().ToUpper())//BSE
                        return "Please check Scrip Id and Scrip Code";

                }
                // SLB SLBMaster objSLBMasterDict
                if (segment == Enumerations.Order.ScripSegment.SLB.ToString())
                {
                    //Check if the passed ScripCode and Symbol matches
                    if (symbol.Trim().ToUpper() != MasterSharedMemory.objSLBMasterDict[Convert.ToInt32(scripCode)].SCScripId.Trim().ToUpper())//BSE
                        return "Please check Scrip Id and Scrip Code";
                }

            }
            else if (exchange == Enumerations.Order.Exchanges.NSE.ToString())
            {
                if (segment == Enumerations.Order.ScripSegment.Equity.ToString())
                {
                    //Check if passed scripID and ScripCode macthes
                    if (scripCode != Convert.ToInt64(MasterSharedMemory.objMastertxtDictBaseNSE.Where(x => x.Value.ScripId.Trim().ToUpper() == symbol.Trim().ToUpper()).Select(x => x.Key).FirstOrDefault()))//BSE
                        return "Please check Scrip Id and Scrip Code";
                }
                else if (segment == Enumerations.Order.ScripSegment.Derivative.ToString())
                {
                    //Check if passed scripID and ScripCode macthes
                    if (scripCode != Convert.ToInt64(MasterSharedMemory.objMasterDerivativeDictBaseNSE.Where(x => x.Value.ScripId.Trim().ToUpper() == symbol.Trim().ToUpper()).Select(x => x.Key).FirstOrDefault()))//BSE
                        return "Please check Scrip Id and Scrip Code";
                }
                else if (segment == Enumerations.Order.ScripSegment.Currency.ToString())
                {
                    //Check if passed scripID and ScripCode macthes
                    if (scripCode != Convert.ToInt64(MasterSharedMemory.objMasterCurrencyDictBaseNSE.Where(x => x.Value.ScripId.Trim().ToUpper() == symbol.Trim().ToUpper()).Select(x => x.Key).FirstOrDefault()))//BSE
                        return "Please check Scrip Id and Scrip Code";
                }
            }

            // else if (exchange == Enumerations.Order.Exchanges.NCDEX.ToString())
            //  {
            //    //Check if passed scripID and ScripCode macthes
            //   if (scripCode != Convert.ToInt64(MasterSharedMemory.objNCDEXMasterDict.Where(x => x.Value.mc.Trim().ToUpper() == symbol.Trim().ToUpper()).Select(x => x.Key).FirstOrDefault()))//BSE
            //        return "Please check Scrip Id and Scrip Code";
            // }
            //else if (exchange == Enumerations.Order.Exchanges.MCX.ToString())
            //{
            //    //Check if passed scripID and ScripCode macthes
            //    if (scripCode != Convert.ToInt64(MasterSharedMemory.objMCXMasterDict.Where(x => x.Value.mc.Trim().ToUpper() == symbol.Trim().ToUpper()).Select(x => x.Key).FirstOrDefault()))//BSE
            //        return "Please check Scrip Id and Scrip Code";

            //}
            return lstrMessage;
        }

        public static string ValidateVolume(long pstrValue, long plngMaxValue, long plngMinValue, int pintNumberOfDecimals, bool pblnNullable = false)
        {
            string lstrMessage = null;
            long llngValue = 0;
            lstrMessage = "Success";
            //pstrValue = pstrValue.Trim();
            if (pstrValue == 0)
            {
                if (!pblnNullable)
                {
                    lstrMessage = "Quantity can't be blank";
                }
            }
            //if ((lstrMessage.Trim().ToUpper() == "Success".Trim().ToUpper() && pstrValue > 0))
            //{
            //    //if (!pstrValue.All(pstrValue.IsNumber))
            //    //{
            //    //    lstrMessage = "Please enter a valid numeric value.";
            //    //}
            //    //else if ((pstrValue.IndexOf(".") >= 0))
            //    //{
            //    //    lstrMessage = "Cannot have Decimal Digits.";
            //    //}
            //}
            if ((lstrMessage.Trim().ToUpper() == "Success".Trim().ToUpper() & pstrValue > 0))
            {
                llngValue = Convert.ToInt64(pstrValue);
                if (!(llngValue >= plngMinValue && llngValue <= plngMaxValue))
                {
                    //lstrMessage = "Must be less than " & plngMaxValue & " and greater than " & plngMinValue
                    lstrMessage = "Maximum Qty Should be greater than or equal " + plngMinValue;
                }
            }
            return lstrMessage;
        }

        //public static string ValidateVolume(long pstrValue, double plngMaxValue, double plngMinValue, int pintNumberOfDecimals, bool pblnNullable = false)
        //{
        //    string lstrMessage = null;
        //    long llngValue = 0;
        //    lstrMessage = "Success";
        //    //pstrValue = pstrValue.Trim();
        //    if (pstrValue == 0)
        //    {
        //        if (!pblnNullable)
        //        {
        //            lstrMessage = "Quantity Cannot be blank.";
        //        }
        //    }
        //    //if ((lstrMessage.Trim().ToUpper() == "Success".Trim().ToUpper() && pstrValue > 0))
        //    //{
        //    //    //if (!pstrValue.All(pstrValue.IsNumber))
        //    //    //{
        //    //    //    lstrMessage = "Please enter a valid numeric value.";
        //    //    //}
        //    //    //else if ((pstrValue.IndexOf(".") >= 0))
        //    //    //{
        //    //    //    lstrMessage = "Cannot have Decimal Digits.";
        //    //    //}
        //    //}
        //    if ((lstrMessage.Trim().ToUpper() == "Success".Trim().ToUpper() & pstrValue > 0))
        //    {
        //        llngValue = Convert.ToInt64(pstrValue);
        //        if (!(llngValue >= plngMinValue && llngValue <= plngMaxValue))
        //        {
        //            //lstrMessage = "Must be less than " & plngMaxValue & " and greater than " & plngMinValue
        //            lstrMessage = "Maximum Qty Should be greater than or equal " + plngMinValue;
        //        }
        //    }
        //    return lstrMessage;
        //}

        public static string ValidateRevlQty(long pstrValue, long plngMaxValue, long plngMinValue, int pintNumberOfDecimals, bool pblnNullable = false)
        {
            string lstrMessage = null;
            long llngValue = 0;
            lstrMessage = "Success";

            if ((lstrMessage.Trim().ToUpper() == "Success".Trim().ToUpper() & pstrValue > 0))
            {
                llngValue = Convert.ToInt64(pstrValue);
                if (!(llngValue >= plngMinValue && llngValue <= plngMaxValue))
                {
                    //lstrMessage = "Must be less than " & plngMaxValue & " and greater than " & plngMinValue
                    lstrMessage = "Maximum Qty Should be greater than or equal " + plngMinValue;
                }
            }
            return lstrMessage;
        }

        public static bool ValidatePrice(string rate, string trgPrice, OrderModel omodel, ref string Validate_Message, int DecimalPoint)
        {
            if (omodel != null)
            {
                //if (omodel.Segment == Enumerations.Order.ScripSegment.Equity.ToString())
                {
                    //Check Price is Not a number
                    if (!string.IsNullOrEmpty(rate))
                    {
                        //if (!Regex.IsMatch(rate, @"^[1-9]\d*(\.\d+)?$"))
                        //{
                        //    //if (rate == "0" || rate == string.Empty)
                        //    //    Validate_Message = "Please provide Rate";
                        //    Validate_Message = "Illegal Characters in Rate";

                        //    return false;
                        //}
                        //if(rate.StartsWith(".") || rate.EndsWith("."))
                        //{
                        //    Validate_Message = "Illegal Characters in Rate";
                        //    return false;
                        //}
                    }
                    else
                    {
                        Validate_Message = "Rate Entered is Empty";
                        return false;
                    }
                    if (rate == null)
                    {
                        Validate_Message = "Rate Entered is Empty";
                        return false;
                    }

                    //Check Number after decimal point
                    if (rate.Contains(".") && rate.Substring(rate.IndexOf(".") + 1).Length > DecimalPoint)
                    {
                        Validate_Message = "Rate More than " + DecimalPoint + " decimal places!";
                        return false;
                    }

                    //Total Rate should be Multiple of Tick Size
                    if (!string.IsNullOrEmpty(omodel.TickSize))
                    {
                        if (Convert.ToDouble(omodel.TickSize) != 0)
                        {
                            //if (Convert.ToInt64(Convert.ToDouble(rate) * Math.Pow(10, DecimalPoint)) % Convert.ToInt64(Convert.ToDouble(omodel.TickSize) * Math.Pow(10, DecimalPoint)) != 0)
                            if (Convert.ToInt64(Convert.ToDouble(rate) * Math.Pow(10, DecimalPoint)) % Convert.ToInt64(Convert.ToDouble(omodel.TickSize)) != 0)
                            {
                                Validate_Message = "Rate should be a multiple of Tick Size.";
                                return false;
                            }
                        }
                        else
                        {
                            Validate_Message = "Tick size is Zero.";
                            return false;
                        }
                    }

                    else
                    {
                        Validate_Message = "Tick size is Zero.";
                        return false;
                    }

#if BOW
                    if (omodel.OrderType.ToUpper() == Enumerations.Order.OrderTypes.STOPLOSS.ToString().ToUpper())
#elif TWS
                    if (omodel.OrderType.ToUpper() == "P" || (omodel.OrderType.ToUpper() == "L" && omodel.IsOCOOrder))
#endif
                    {
                        if (string.IsNullOrEmpty(trgPrice) || trgPrice == "0")
                        {
                            Validate_Message = "Please enter Trg Rate";

                            return false;
                        }

                        //Check Trigger Price is Not a number
                        if (!Regex.IsMatch(trgPrice, @"^[1-9]\d*(\.\d+)?$"))
                        {
                            //if (rate == "0" || rate == string.Empty)
                            //    Validate_Message = "Please provide Rate";
                            Validate_Message = "Illegal Characters in Trigger Rate";

                            return false;
                        }

                        //Check Number after decimal point
                        if (trgPrice.Contains(".") && trgPrice.Substring(rate.IndexOf(".") + 1).Length > DecimalPoint)
                        {
                            Validate_Message = "Trigger Rate More than" + DecimalPoint + "decimal places!";
                            return false;
                        }

                        long llngTriggerPrice = Convert.ToInt64(Convert.ToDouble(trgPrice) * Math.Pow(10, DecimalPoint));

                        if (llngTriggerPrice > 0)
                        {
                            if (Convert.ToDouble(trgPrice) > 0)
                            {
                                long llngPrice = Convert.ToInt64(Convert.ToDouble(rate) * Math.Pow(10, DecimalPoint));
                                if (llngPrice != 0)
                                {
                                    if (omodel.BuySellIndicator.ToUpper() == Enumerations.Side.Buy.ToString().ToUpper())
                                    {
                                        if ((llngTriggerPrice > llngPrice))
                                        {
                                            Validate_Message = "Stop Price should be less than the Limit Price.";
                                            //pstrError = lstrMessage;
                                            //pintPropertyId = omodel.TriggerPrice;
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                        if ((llngTriggerPrice < llngPrice))
                                        {
                                            Validate_Message = "Stop Price should be greater than the Limit Price.";
                                            //pstrError = lstrMessage;
                                            return false;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                Validate_Message = "Please enter a valid Limit Price and Stop Price";
                                return false;
                            }
                        }

                        //Total Rate should be Multiple of Tick Size
                        //if (!string.IsNullOrEmpty(omodel.TickSize))
                        //{
                        //    if (Convert.ToDouble(omodel.TickSize) != 0)
                        //    {
                        //        if (Convert.ToInt64(Convert.ToDouble(rate) * Math.Pow(10, DecimalPoint)) % Convert.ToInt64(Convert.ToDouble(omodel.TickSize) * Math.Pow(10, DecimalPoint)) != 0)
                        //        {
                        //            Validate_Message = "Trigger Rate should be a multiple of Mkt lot.";
                        //            return false;
                        //        }
                        //    }
                        //    else
                        //    {
                        //        Validate_Message = "Tick size is Zero.";
                        //        return false;
                        //    }
                        //}

                        //else
                        //{
                        //    Validate_Message = "Tick size is Zero.";
                        //    return false;
                        //}
                    }

                }

            }

            return true;
        }

        public static string ValidatePrice(string pstrValue, double pdblMaxValue, double pdblMinValue, int pintNumberOfDecimals, bool pblnNullable = false)
        {
            string lstrMessage = null;
            string ldblValue = null;
            string lstrDecimalDigits = null;
            int lintCount = 0;
            string lstrNewValue = null;
            lstrMessage = "Success";
            pstrValue = pstrValue.Trim();
            if (pstrValue.Length == 0)
            {
                if (!pblnNullable)
                {
                    lstrMessage = "Price cannot be blank.";
                }
            }
            if ((lstrMessage.Trim().ToUpper() == "Success".Trim().ToUpper() && pstrValue.Trim().Length > 0))
            {
                if (!pstrValue.All(char.IsNumber))
                {
                    lstrMessage = "Please enter a valid numeric value.";
                }
                else if ((pstrValue.IndexOf(".") >= 0))
                {
                    lstrDecimalDigits = pstrValue.Substring(pstrValue.IndexOf(".") + 1);
                    //remove trailing blanks
                    lstrNewValue = lstrDecimalDigits;
                    for (lintCount = lstrDecimalDigits.Trim().Length - 1; lintCount >= 0; lintCount += -1)
                    {
                        if ((lstrDecimalDigits.Substring(lintCount, 1) == "0"))
                        {
                            lstrNewValue = lstrNewValue.Substring(0, lintCount);
                        }
                        else
                        {
                            break; // TODO: might not be correct. Was : Exit For
                        }
                    }
                    if ((lstrNewValue.Length > pintNumberOfDecimals))
                    {
                        lstrMessage = "Number of maximum decimal digits cannot exceed " + pintNumberOfDecimals;
                    }
                }
            }
            if ((lstrMessage.Trim().ToUpper() == "Success".Trim().ToUpper() && pstrValue.Trim().Length > 0))
            {
                //ldblValue = Convert.ToDouble(pstrValue);
                if (!(Convert.ToDouble(pstrValue) >= pdblMinValue && Convert.ToDouble(pstrValue) <= pdblMaxValue))
                {
                    lstrMessage = "Must be less than " + pdblMaxValue + " and greater than " + pdblMinValue;
                }
            }
            return lstrMessage;
        }
        //Bow Validate order for example
        //public bool ValidateOrders(ref string pstrError, ref int pintPropertyId = 0, bool pblnAllowQtyAboveMaxQty = false)
        //{
        //    string lstrMessage = null;
        //    Utilities.SecurityBase lSecurity = default(Utilities.SecurityBase);
        //    try
        //    {
        //        //: Validate Exchange
        //        if (this.Exchange == null || this.Exchange.Length == 0)
        //        {
        //            lstrMessage = "Please select a valid exchanage";
        //            pstrError = lstrMessage;
        //            pintPropertyId = CMBuySellParameters.Destination;
        //            return false;
        //        }

        //        //: Fetch Symbol
        //        string lstrSymbol = this.Symbol;
        //        string lstrSeries = this.Series;
        //        if (this.Mode != MODES.Edit)
        //        {
        //            if (gblnLDBSelf == true || gblnLDBAll == true)
        //            {
        //                lSecurity = gobjUtilitySQLScript.GetScript(mobjBusinessLogic.EMS.GetExchangeId(this.Exchange), mobjBusinessLogic.EMS.GetMarketId(this.Market), lstrSymbol, lstrSeries);
        //            }
        //            else
        //            {
        //                lSecurity = mobjBusinessLogic.ScriptDetails.GetScript(mobjBusinessLogic.EMS.GetExchangeId(this.Exchange), mobjBusinessLogic.EMS.GetMarketId(this.Market), lstrSymbol, lstrSeries);
        //            }
        //            if (lSecurity == null)
        //            {
        //                lstrMessage = "Symbol not found";
        //                pstrError = lstrMessage;
        //                pintPropertyId = CMBuySellParameters.Symbol;
        //                return false;
        //            }
        //        }
        //        else
        //        {
        //            //: This is done for the BSE Exhange which sends GroupName in the PreEditOrder 
        //            if (gblnLDBSelf == true || gblnLDBAll == true)
        //            {
        //                lSecurity = gobjUtilitySQLScript.GetScript(mobjBusinessLogic.EMS.GetExchangeId(this.Exchange), this.Token, mobjBusinessLogic.EMS.GetMarketId(this.Market));
        //            }
        //            else
        //            {
        //                lSecurity = mobjBusinessLogic.ScriptDetails.GetScript(mobjBusinessLogic.EMS.GetExchangeId(this.Exchange), this.Token, mobjBusinessLogic.EMS.GetMarketId(this.Market));
        //            }
        //            if ((lSecurity != null))
        //            {
        //                lstrSymbol = lSecurity.Symbol;
        //                lstrSeries = lSecurity.Series;
        //                this.Symbol = lSecurity.Symbol;
        //                this.Series = lSecurity.Series;
        //            }
        //        }
        //        //: 

        //        //: Validate Volume
        //        lstrMessage = ValidateVolume(this.Volume, 9999999999L, 1, 0, false);
        //        if (lstrMessage.Trim.ToUpper != "Success".ToUpper)
        //        {
        //            lstrMessage = "Volume : " + lstrMessage;
        //            pstrError = lstrMessage;
        //            pintPropertyId = CMBuySellParameters.Volume;
        //            return false;
        //        }

        //        //: Validate Volume w.r.t Max Qty that can be entered
        //        if ((Information.IsNumeric(gstrMaxOrderQuantity) == true && pblnAllowQtyAboveMaxQty == false))
        //        {
        //            if ((Convert.ToInt64(gstrMaxOrderQuantity) < long.Parse(this.Volume)))
        //            {
        //                lstrMessage = "The quantity entered is more than the Maximum Quantity " + gstrMaxOrderQuantity + " set.";
        //                pstrError = lstrMessage;
        //                pintPropertyId = CMBuySellParameters.Volume;
        //                return false;
        //            }
        //        }

        //        if ((Information.IsNumeric(gstrMinOrderQuantity) == true && pblnAllowQtyAboveMaxQty == false))
        //        {
        //            if ((Convert.ToInt64(gstrMinOrderQuantity) > long.Parse(this.Volume)))
        //            {
        //                lstrMessage = "The quantity entered is less than the Minimum Quantity " + gstrMinOrderQuantity + " set.";
        //                pstrError = lstrMessage;
        //                pintPropertyId = CMBuySellParameters.Volume;
        //                return false;
        //            }
        //        }
        //        //: Disclose volume should not be greater than actual volume
        //        if (this.DisclosedVolume.Trim.Length > 0 && this.Volume.Trim.Length > 0)
        //        {
        //            if (long.Parse(this.DisclosedVolume) > long.Parse(this.Volume))
        //            {
        //                pstrError = "The Disclosed quantity cannot be greater than the actual quantity.";
        //                pintPropertyId = CMBuySellParameters.DisclosedVolume;
        //                return false;
        //            }
        //        }
        //        if ((lSecurity != null))
        //        {
        //            //: Validate Volume w.r.t to Board Lot Quantity
        //            long llngBoardLotQuantity = 0;
        //            llngBoardLotQuantity = lSecurity.BoardLotQuantity;

        //            long llngQuantity = 0;
        //            if ((this.Volume != null) && this.Volume.Trim.Length > 0)
        //                llngQuantity = this.Volume;
        //            if ((llngBoardLotQuantity != 0 && llngQuantity % llngBoardLotQuantity != 0))
        //            {
        //                lstrMessage = "Please enter a valid Qty which is multiple of " + llngBoardLotQuantity;
        //                pstrError = lstrMessage;
        //                pintPropertyId = CMBuySellParameters.Volume;
        //                return false;
        //            }
        //            //: Validate Disclosed Volume w.r.t to Board Lot Quantity
        //            if ((this.DisclosedVolume.Length > 0))
        //            {
        //                if (!this.DisclosedVolume && this.DisclosedVolume.Trim.Length > 0)
        //                    llngQuantity = this.DisclosedVolume;
        //                if ((llngQuantity % llngBoardLotQuantity != 0))
        //                {
        //                    lstrMessage = "Please enter a valid Qty which is multiple of " + llngBoardLotQuantity;
        //                    pstrError = lstrMessage;
        //                    pintPropertyId = CMBuySellParameters.DisclosedVolume;
        //                    return false;
        //                }
        //            }
        //        }
        //        //:
        //        //: Since bse has removed dq validation for bigger quantities. So as per vishal sir removing dq validation entirly as BSE it self has not mentioned the quantity limit and the min dq to apply. The validation will be done on the exchange end.
        //        if (mobjBusinessLogic.EMS.GetExchangeId(this.Exchange) != BowConstants.EX_BSE_VALUE)
        //        {
        //            //: Validate Volumne w.r.t Disclosed Volume
        //            if ((!string.IsNullOrEmpty(this.DisclosedVolume.Trim) && this.DisclosedVolume.Trim != "0"))
        //            {
        //                lstrMessage = ValidateDQ(Convert.ToInt64(this.Volume), Convert.ToInt64(this.DisclosedVolume));
        //                if (!string.IsNullOrEmpty(lstrMessage))
        //                {
        //                    pstrError = lstrMessage;
        //                    pintPropertyId = CMBuySellParameters.DisclosedVolume;
        //                    return false;
        //                }
        //            }
        //        }

        //        //: Validate Price
        //        //:Validate for null value in case of bulk orders
        //        if (this.UsedForBulkOrders == true)
        //        {
        //            lstrMessage = ValidatePrice(this.Price, 9999999999L, 0, 2, false);
        //        }
        //        else
        //        {
        //            lstrMessage = ValidatePrice(this.Price, 9999999999L, 0, 2, true);
        //        }
        //        if (lstrMessage.Trim.ToUpper != "Success".ToUpper)
        //        {
        //            pstrError = lstrMessage;
        //            pintPropertyId = CMBuySellParameters.Price;
        //            return false;
        //        }
        //        //: Validate Value(Price*Qty) w.r.t Max Value that can be entered
        //        if ((Information.IsNumeric(gstrMaxOrderValue) == true))
        //        {
        //            if ((Convert.ToInt64(gstrMaxOrderValue) < (long.Parse(ShiftDecimalInText(this.Price, 2)) * long.Parse(this.Volume))))
        //            {
        //                lstrMessage = "The Order value entered is more than the Maximum Order Value " + Strings.Format(gstrMaxOrderValue / 100, "###0.00") + " set.";
        //                pstrError = lstrMessage;
        //                pintPropertyId = CMBuySellParameters.Price;
        //                return false;
        //            }
        //        }
        //        //: Validate Trigger Price 
        //        lstrMessage = ValidatePrice(this.TriggerPrice, 9999999999L, 0, 2, true);
        //        if (lstrMessage.Trim.ToUpper != "Success".ToUpper)
        //        {
        //            pstrError = lstrMessage;
        //            pintPropertyId = CMBuySellParameters.TriggerPrice;
        //            return false;
        //        }
        //        //: Validate DQ
        //        lstrMessage = ValidateVolume(this.DisclosedVolume, 9999999999L, 0, 0, true);
        //        if (lstrMessage.Trim.ToUpper != "Success".ToUpper)
        //        {
        //            pstrError = "Disclosed Volume : " + lstrMessage;
        //            pintPropertyId = CMBuySellParameters.DisclosedVolume;
        //            return false;
        //        }

        //        //: if SL order then Trigger price should be present
        //        if (this.BookType == BowConstants.ORDER_TYPE_SL)
        //        {
        //            if (this.TriggerPrice.Length == 0 || this.TriggerPrice.Trim == 0.0)
        //            {
        //                lstrMessage = "Please enter TriggerPrice for StopLoss order.";
        //                pstrError = lstrMessage;
        //                pintPropertyId = CMBuySellParameters.TriggerPrice;
        //                return false;
        //            }
        //        }
        //        // : By passing it for Protection Percentage which is placed at Market Order.
        //        if (gobjUtilityEMS.GetExchangeId(this.Exchange) != BowConstants.EX_BSE_VALUE)
        //        {
        //            //: Validate Price w.r.t Trigger Price 
        //            if (this.TriggerPrice.Length > 0)
        //            {
        //                long llngTriggerPrice = Convert.ToInt64(Convert.ToDouble(this.TriggerPrice) * 100);
        //                if (llngTriggerPrice > 0)
        //                {
        //                    if (this.Price.Length > 0)
        //                    {
        //                        long llngPrice = Convert.ToInt64(Convert.ToDouble(this.Price) * 100);
        //                        if (llngPrice != 0)
        //                        {
        //                            if (this.BuySellIndicator.ToUpper == BowConstants.BUY.ToUpper)
        //                            {
        //                                if ((llngTriggerPrice > llngPrice))
        //                                {
        //                                    lstrMessage = "The Trigger Price should be less than the Price.";
        //                                    pstrError = lstrMessage;
        //                                    pintPropertyId = CMBuySellParameters.TriggerPrice;
        //                                    return false;
        //                                }
        //                            }
        //                            else
        //                            {
        //                                if ((llngTriggerPrice < llngPrice))
        //                                {
        //                                    lstrMessage = "The Trigger Price should be greater than the Price.";
        //                                    pstrError = lstrMessage;
        //                                    return false;
        //                                }
        //                            }
        //                        }
        //                    }
        //                    else
        //                    {
        //                        lstrMessage = "Please enter a valid Price and Trigger Price";
        //                        pstrError = lstrMessage;
        //                        return false;
        //                    }
        //                }
        //            }
        //        }

        //        //viju 09Jun
        //        if (gobjUtilityEMS.GetExchangeId(this.Exchange) == BowConstants.EX_BSE_VALUE & this.OrderType == BowConstants.ORDER_TYPE_OCO)
        //        {
        //            long llngTriggerPrice = Convert.ToInt64(Convert.ToDouble(this.TriggerPrice) * 100);
        //            if (llngTriggerPrice > 0)
        //            {
        //                if (this.Price.Length > 0)
        //                {
        //                    long llngPrice = Convert.ToInt64(Convert.ToDouble(this.Price) * 100);
        //                    if (llngPrice != 0)
        //                    {
        //                        //:Changes suggested by Anjul Sharma - 13Jun2016
        //                        if (this.BuySellIndicator.ToUpper == BowConstants.SELL.ToUpper)
        //                        {
        //                            if ((llngTriggerPrice > llngPrice))
        //                            {
        //                                lstrMessage = "Stop Price should be less than the Limit Price.";
        //                                pstrError = lstrMessage;
        //                                pintPropertyId = CMBuySellParameters.TriggerPrice;
        //                                return false;
        //                            }
        //                        }
        //                        else
        //                        {
        //                            if ((llngTriggerPrice < llngPrice))
        //                            {
        //                                lstrMessage = "Stop Price should be greater than the Limit Price.";
        //                                pstrError = lstrMessage;
        //                                return false;
        //                            }
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    lstrMessage = "Please enter a valid Limit Price and Stop Price";
        //                    pstrError = lstrMessage;
        //                    return false;
        //                }
        //            }
        //        }
        //        //till here - viju 09Jun

        //        if ((lSecurity != null))
        //        {
        //            //: Validate Price and TriggerPrice w.r.t TickSize ONLY FOR Nse Exchange
        //            if (mobjBusinessLogic.EMS.GetExchangeId(this.Exchange) == BowConstants.EX_NSE_VALUE)
        //            {
        //                //: Validate Price w.r.t Tick Size
        //                if (this.Price.Length > 0)
        //                {
        //                    if (!(Convert.ToInt64(ShiftDecimalInText(this.Price, 2)) % lSecurity.TickSize == 0))
        //                    {
        //                        lstrMessage = "Please enter Price in multiples of " + lSecurity.TickSize + " paise";
        //                        pstrError = "Please enter Price in multiples of " + lSecurity.TickSize + " paise";
        //                        pintPropertyId = CMBuySellParameters.Price;
        //                        return false;
        //                    }
        //                }
        //                //: Validate Trigger Price w.r.t Tick Size
        //                if (this.TriggerPrice.Length > 0)
        //                {
        //                    if (!(Convert.ToInt64(ShiftDecimalInText(this.TriggerPrice, 2)) % lSecurity.TickSize == 0))
        //                    {
        //                        lstrMessage = "Please enter Price in multiples of " + lSecurity.TickSize + " paise";
        //                        pstrError = lstrMessage;
        //                        pintPropertyId = CMBuySellParameters.TriggerPrice;
        //                        return false;
        //                    }
        //                }
        //            }
        //        }
        //        //REM:  BackOffice Id
        //        if (base.BackOfficeId.Trim.Length == 0)
        //        {
        //            lstrMessage = "Please enter a BackOfficeId";
        //            pstrError = lstrMessage;
        //            pintPropertyId = CMBuySellParameters.BackOfficeId;
        //            return false;
        //        }
        //        //: If the Clearing or Trading Member has Logged on and is trying to place an order for him self then disallowing any such order.
        //        if (gblnDisallowOrderEntryForSelfFOR_C_OR_T == true && gobjUtilityLoginDetails.UserLoginId.Trim.ToUpper == base.BackOfficeId.Trim.ToUpper && this.ProClientIndicator == false)
        //        {
        //            lstrMessage = "Order entry for self not allowed.";
        //            pstrError = lstrMessage;
        //            pintPropertyId = CMBuySellParameters.BackOfficeId;
        //            return false;
        //        }

        //        //'------------kiran ''ENH:- BSE:GSM, 25-MAR-17-->

        //        if (gblnPromptIf_T_GroupScrip == true && this.OrderEntryState == CashOrderEntry.ORDER_ENTRY_STATE.ValidationRequired)
        //        {
        //            if (mobjBusinessLogic.EMS.GetExchangeId(this.Exchange) == BowConstants.EX_BSE_VALUE)
        //            {
        //                if ((this.SecurityInfo != null) && (((BSESecurity)this.SecurityInfo).Group.Trim.ToUpper == "T" | ((BSESecurity)this.SecurityInfo).Group.Trim.ToUpper == "TS" | ((BSESecurity)this.SecurityInfo).Group.Trim.ToUpper == "Z" | ((BSESecurity)this.SecurityInfo).Group.Trim.ToUpper == "MT" | ((BSESecurity)this.SecurityInfo).Group.Trim.ToUpper == "ZP" | ((BSESecurity)this.SecurityInfo).Group.Trim.ToUpper == "XC" | ((BSESecurity)this.SecurityInfo).Group.Trim.ToUpper == "XD" | ((BSESecurity)this.SecurityInfo).Group.Trim.ToUpper == "XT"))
        //                {
        //                    //'GROUP 'T','XC','XD','XT','TS' OR 'Z',(new group chk added for grp:'ZP','MT') BELOW NEW CHECK ADDED BY KIRANS,15-MAR-17 FOR ENH:BSE SECURITIES GSM.
        //                    if (((((BSESecurity)this.SecurityInfo).BSCFIELD4) != null))
        //                    {

        //                        if (((((BSESecurity)this.SecurityInfo).BSCFIELD4.Trim.ToUpper == BowConstants.BSE_GSM_NA) || (string.IsNullOrEmpty(((BSESecurity)this.SecurityInfo).BSCFIELD4.Trim.ToUpper))))
        //                        {
        //                            lstrMessage = "Please note that the scrip is in \"" + ((BSESecurity)this.SecurityInfo).Group.Trim.ToUpper + "\" group and trade would be settled on Trade to Trade basis. Do you want to continue?";
        //                            pstrError = lstrMessage;
        //                            pintPropertyId = CMBuySellParameters.Symbol;
        //                            return false;

        //                            //' ''If MessageBox_Show(Me.ParentForm, "Do you want to place an Order in 'T','XC','XD','XT','TS' OR 'Z' Group Scrip.", gstrMyTitle, "T,XC,XD,XT,TS,Z Group Scrip.", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then
        //                            //'  If MessageBox_Show(Me.ParentForm, "Please note that the scrip is in """ + CType(Me.SecurityInfo, BSESecurity).Group.Trim.ToUpper + """ group and trade would be settled on Trade to Trade basis. Do you want to continue?", gstrMyTitle, "T,XC,XD,XT,TS,Z,MT,ZP Group Scrip.", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then
        //                            //'Return False
        //                            //'  End If

        //                        }
        //                        else
        //                        {
        //                            //'if script is GSM(other than 100/NULL/BLANK), Stage0(onwards)
        //                            if (((((BSESecurity)this.SecurityInfo).BSCFIELD4) != null))
        //                            {
        //                                if (((((BSESecurity)this.SecurityInfo).BSCFIELD4.Trim.ToUpper == BowConstants.BSE_GSM_STAGE0)))
        //                                {
        //                                    //Stage0

        //                                    lstrMessage = "Please note that the scrip is in \"" + ((BSESecurity)this.SecurityInfo).Group.Trim.ToUpper + "\" group and under Graded Surveillance Measure (GSM).  Trade would be settled on Trade to Trade basis. Do you wish to continue?(For more Info refer notice no. 20170223-44)";
        //                                    pstrError = lstrMessage;
        //                                    pintPropertyId = CMBuySellParameters.Symbol;
        //                                    return false;

        //                                    //'If MessageBox_Show(Me.ParentForm, "Please note that the scrip is in """ + CType(Me.SecurityInfo, BSESecurity).Group.Trim.ToUpper + """ group and under Graded Surveillance Measure (GSM).  Trade would be settled on Trade to Trade basis. Do you wish to continue?(For more Info refer notice no. 20170223-44)", gstrMyTitle, "Graded Surveillance Measure (GSM).", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then
        //                                    //'Return False
        //                                    //'End If

        //                                }
        //                                else
        //                                {
        //                                    //Stage 1 To 6
        //                                    lstrMessage = "Please note that the scrip is in \"" + ((BSESecurity)this.SecurityInfo).Group.Trim.ToUpper + "\" group and under Graded Surveillance Measure (Stage: " + ((BSESecurity)this.SecurityInfo).BSCFIELD4.Trim.ToUpper + ").  Trade would be settled on Trade to Trade basis. Additional deposit shall be applicable for buyer (Stage 2 onwards). Do you wish to continue?(For more Info refer notice no. 20170223-44)";
        //                                    pstrError = lstrMessage;
        //                                    pintPropertyId = CMBuySellParameters.Symbol;
        //                                    return false;


        //                                    //' If MessageBox_Show(Me.ParentForm, "Please note that the scrip is in """ + CType(mobjOrderEntry.SecurityInfo, BSESecurity).Group.Trim.ToUpper + """ group and under Graded Surveillance Measure (Stage: " + CType(mobjOrderEntry.SecurityInfo, BSESecurity).BSCFIELD4.Trim.ToUpper + ").  Trade would be settled on Trade to Trade basis. Additional deposit shall be applicable for buyer (Stage 2 onwards). Do you wish to continue?(For more Info refer notice no. 20170223-44)", gstrMyTitle, "T,XC,XD,XT,TS,Z,ZP,MT Group Scrip.", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then
        //                                    //'Return False
        //                                    //'End If

        //                                }
        //                            }
        //                        }
        //                    }


        //                }
        //                else
        //                {
        //                    //Other than T group And Stage0,then New message GSM scrips (Stage 0) condition added by KiranS,15-MAR-17
        //                    if ((this.SecurityInfo != null))
        //                    {
        //                        if (((((BSESecurity)this.SecurityInfo).BSCFIELD4) != null))
        //                        {

        //                            if (((((BSESecurity)this.SecurityInfo).BSCFIELD4.Trim.ToUpper == BowConstants.BSE_GSM_STAGE0)))
        //                            {
        //                                lstrMessage = "Please note that the Scrip under Graded Surveillance Measure (GSM). Do you wish to continue?(For more Info refer notice no. 20170223-44)";
        //                                pstrError = lstrMessage;
        //                                pintPropertyId = CMBuySellParameters.Symbol;
        //                                return false;
        //                                //'  If MessageBox_Show(Me.ParentForm, "Please note that the Scrip under Graded Surveillance Measure (GSM). Do you wish to continue?(For more Info refer notice no. 20170223-44)", gstrMyTitle, "Graded Surveillance Measure (GSM).", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then                                       ''Return False
        //                                //'End If
        //                            }
        //                        }
        //                    }
        //                }

        //            }
        //            else if (mobjBusinessLogic.EMS.GetExchangeId(this.Exchange) == BowConstants.EX_NSE_VALUE)
        //            {

        //                if (this.SecurityInfo.Series.Trim.ToUpper == "BE")
        //                {
        //                    //-------------------KiranS, New Check for NSE GSM, Req by Anjul,12-APR-17 -----------------------------------------------

        //                    if (((((NSESecurity)this.SecurityInfo).NSCIssueRate) != null))
        //                    {

        //                        if (((((NSESecurity)this.SecurityInfo).NSCIssueRate.Trim.ToUpper == BowConstants.NSE_GSM_NA) || (string.IsNullOrEmpty(((NSESecurity)this.SecurityInfo).NSCIssueRate.Trim.ToUpper))))
        //                        {
        //                            lstrMessage = "Do you want to place an Order in \"BE\" Group Scrip.";
        //                            //'(earlier original Message for only for BE groups alert and nonGSM)
        //                            pstrError = lstrMessage;
        //                            pintPropertyId = CMBuySellParameters.Symbol;
        //                            return false;
        //                        }
        //                        else
        //                        {
        //                            //'if script is GSM(other than 100/NULL/BLANK), Stage0(onwards)
        //                            if (((((NSESecurity)this.SecurityInfo).NSCIssueRate) != null))
        //                            {
        //                                if (((((NSESecurity)this.SecurityInfo).NSCIssueRate.Trim.ToUpper == BowConstants.NSE_GSM_STAGE0)))
        //                                {
        //                                    //Stage0

        //                                    lstrMessage = "Please note that the scrip is in \"BE\" group and under Graded Surveillance Measure (GSM). Do you wish to continue?";
        //                                    pstrError = lstrMessage;
        //                                    pintPropertyId = CMBuySellParameters.Symbol;
        //                                    return false;

        //                                }
        //                                else
        //                                {
        //                                    //Stage 1 To 6
        //                                    lstrMessage = "Please note that the scrip is in \"BE\" group and under Graded Surveillance Measure (Stage: " + ((NSESecurity)this.SecurityInfo).NSCIssueRate.Trim.ToUpper + "). Do you wish to continue?";
        //                                    pstrError = lstrMessage;
        //                                    pintPropertyId = CMBuySellParameters.Symbol;
        //                                    return false;

        //                                }
        //                            }
        //                        }
        //                    }

        //                }
        //                else
        //                {
        //                    //Other than BE group And Stage0,then New message GSM scrips (Stage 0) condition added by KiranS,12-APR-17
        //                    if ((this.SecurityInfo != null))
        //                    {

        //                        if (((((NSESecurity)this.SecurityInfo).NSCIssueRate) != null))
        //                        {

        //                            if (!((((NSESecurity)this.SecurityInfo).NSCIssueRate.Trim.ToUpper == BowConstants.NSE_GSM_NA) || (string.IsNullOrEmpty(((NSESecurity)this.SecurityInfo).NSCIssueRate.Trim.ToUpper))))
        //                            {
        //                                if (((((NSESecurity)this.SecurityInfo).NSCIssueRate.Trim.ToUpper == BowConstants.NSE_GSM_STAGE0)))
        //                                {
        //                                    //Stage0= Stage99GSM
        //                                    lstrMessage = "Please note that the Scrip under Graded Surveillance Measure (GSM). Do you wish to continue?";
        //                                    pstrError = lstrMessage;
        //                                    pintPropertyId = CMBuySellParameters.Symbol;
        //                                    return false;
        //                                }
        //                                else
        //                                {
        //                                    //Stage 1 To 6
        //                                    lstrMessage = "Please note that the Scrip under Graded Surveillance Measure (GSM Stage: " + ((NSESecurity)this.SecurityInfo).NSCIssueRate.Trim.ToUpper + "). Do you wish to continue?";
        //                                    pstrError = lstrMessage;
        //                                    pintPropertyId = CMBuySellParameters.Symbol;
        //                                    return false;
        //                                }



        //                            }
        //                        }
        //                    }

        //                }
        //            }
        //        }

        //        //'--------------------kiran ''ENH:- BSE:GSM, 25-MAR-17-->

        //        //-------------------KiranS, New Check for NSE GSM, Req by Anjul,12-APR-17 -----------------------------------------------

        //    }
        //    catch (Exception ex)
        //    {
        //        Infrastructure.Logger.WriteLog("Error in CashOrderEntry ValidateOrder. " + ex.Message.ToString());
        //        Infrastructure.Logger.WriteLog("Error in CashOrderEntry ValidateOrder. " + ex.StackTrace.ToString());
        //        return false;
        //    }
        //    return true;
        //}

        //public static Validate_Bow(ref string pstrError,)
#if BOW

        public bool ValidateOrder(OrderModel omodel, string rate, string trgPrice, ref string pstrError, ref int pintPropertyId, bool pblnAllowQtyAboveMaxQty = false, bool pblnAllowValueAboveMaxValue = false, bool pblncheckProtPercn = true)
        {

            decimal ldecBoardLotQty = default(decimal);
            decimal ldecQuantity = default(decimal);
            long llngTodaysDate = 0;
            long llngDays = 0;
            long llngExpiry = 0;
            long llngTriggerPrice = 0;
            long llngPrice = 0;
            string lstrMessage = null;
            //ContractBase lContract = null;
            long llngOrderValue = 0;
            double ldblDQPercentageCheck = 10;
            int lintExchangeID = (int)Enum.Parse(typeof(Enumerations.Order.Exchanges), omodel.Exchange); //mobjBusinessLogic.EMS.GetExchangeId(this.Exchange);
            int lintMarketID = (int)Enum.Parse(typeof(Enumerations.Order.ScripSegment), omodel.Segment);//mobjBusinessLogic.EMS.GetMarketId(this.Market);
            try
            {
                //: Validate Exchange
                if (string.IsNullOrEmpty(omodel.Exchange))
                {
                    lstrMessage = "Please select a valid Exchange";
                    pstrError = lstrMessage;
                    // pintPropertyId = FNOBuySellParameters.Destination;
                    return false;
                }
                //: Validate Contract
                if (lintMarketID == BowConstants.MKT_DERIVATIVE_VALUE || lintMarketID == BowConstants.MKT_CURRENCY_VALUE || lintMarketID == BowConstants.MKT_COMMODITIES_VALUE || lintMarketID == BowConstants.MKT_ITP_VALUE || lintMarketID == BowConstants.MKT_SLB_VALUE)
                {
                    //if (modMDI.gblnLDBSelf == true || modMDI.gblnLDBAll == true)
                    //{
                    //    lContract = modMDI.gobjUtilitySQLScript.GetScript(lintExchangeId, lintMarketID, this.Symbol, this.InstrumentName, modMDI.gobjUtilitySQLScript.GetLongExpiryDateForScript(lintExchangeId, this.ExpiryDate, lintMarketID), Utilities.UtilitiesHelper.UtilityScript.GetActualStrikePrice(this.StrikePrice, lintMarketID), this.OptionType);
                    //}
                    //else
                    //{
                    //    lContract = modMDI.gobjUtilityScript.GetScript(lintExchangeId, lintMarketID, this.Symbol, this.InstrumentName, mobjBusinessLogic.ScriptDetails.GetLongExpiryDateForScript(lintExchangeId, this.ExpiryDate, lintMarketID), Utilities.UtilitiesHelper.UtilityScript.GetActualStrikePrice(this.StrikePrice, lintMarketID), this.OptionType);
                    //}

                    if (omodel == null)
                    {
                        lstrMessage = "Contract not found";
                        pstrError = lstrMessage;
                        //pintPropertyId = FNOBuySellParameters.Symbol;
                        Infrastructure.Logger.WriteLog("Error :" + lstrMessage);
                        Infrastructure.Logger.WriteLog("Exchange : -" + lintExchangeID);
                        Infrastructure.Logger.WriteLog("Market : -" + omodel.Segment);
                        Infrastructure.Logger.WriteLog("Symbol : -" + omodel.Symbol);
                        Infrastructure.Logger.WriteLog("InstrumentName : -" + omodel.InstrumentName);
                        Infrastructure.Logger.WriteLog("Expiry Date : -" + omodel.ExpiryDate);
                        //Infrastructure.Logger.WriteLog("Long Expiry Date : -" & gobjUtilitySQLScript.GetLongExpiryDateForScript(lintExchangeID, Me.ExpiryDate, lintMarketID))
                        Infrastructure.Logger.WriteLog("Strike Price : -" + omodel.StrikePrice);
                        Infrastructure.Logger.WriteLog("Option Type  : -" + omodel.OptionType);
                        return false;
                    }
                    else
                    {
                        if (lintMarketID != BowConstants.MKT_CURRENCY_VALUE && (lintExchangeID == BowConstants.EX_USE_VALUE || lintExchangeID == BowConstants.EX_BSE_VALUE))
                        {
                            if (omodel.Token >= 10000000)
                            {
                                lstrMessage = "Order Entry Not Allowed For The Scrip";
                                pstrError = lstrMessage;
                                // pintPropertyId = FNOBuySellParameters.Symbol;
                                return false;
                            }
                        }
                    }
                }
                //:

                //: Validate GTD  As Numeric
                //if (lintExchangeID != BowConstants.EX_NSE_VALUE && omodel.GoodTillCancel.Trim().ToUpper() == BowConstants.GOOD_TILL_DAYS.ToUpper())
                //{
                //    lstrMessage = ValidateVolume(omodel.GoodTillDate, 100, -1, 0, true);
                //    //Min Value can be -1 for GTC
                //    if (lstrMessage.Trim().ToUpper() != "Success".ToUpper())
                //    {
                //        pstrError = lstrMessage;
                //        //pintPropertyId = FNOBuySellParameters.GoodTillDate;
                //        return false;
                //    }
                //}
                //: Validate GTD w.r.t Retention/GoodTillCancel
                if (lintExchangeID == BowConstants.EX_NCDEX_VALUE || lintExchangeID == BowConstants.EX_NMCE_VALUE || lintExchangeID == BowConstants.EX_BSE_VALUE
                    || lintExchangeID == BowConstants.EX_USE_VALUE)
                {
                    if (string.IsNullOrEmpty(omodel.GoodTillCancel.Trim().ToUpper()))
                    {
                        //: No Validation
                    }
                    else if (omodel.GoodTillCancel.Trim().ToUpper() == BowConstants.GOOD_TILL_CANCEL.ToUpper())
                    {
                        //: No Validation
                    }
                    else if (omodel.GoodTillCancel.Trim().ToUpper() == BowConstants.GOOD_TILL_DAYS.ToUpper())
                    {
                        lstrMessage = ValidateGTD(omodel.GoodTillDate);
                        if (!string.IsNullOrEmpty(lstrMessage.Trim()))
                        {
                            pstrError = lstrMessage;
                            // pintPropertyId = FNOBuySellParameters.GoodTillDate;
                            return false;
                        }
                    }
                    else if (omodel.GoodTillCancel.Trim().ToUpper() == BowConstants.GOOD_TILL_DATE.ToUpper())
                    {
                        if (Convert.ToInt32(omodel.GoodTillDate) > 0)
                        {
                        }
                        else
                        {
                            lstrMessage = " Validate Good Till Date";
                            pstrError = lstrMessage;
                            //pintPropertyId = FNOBuySellParameters.GoodTillDate;
                            //DateTimePicker
                            return false;
                        }
                    }
                }

                //: Validate Contract w.r.t Maturity date
                //: Check whether Contract has Matured - now this will be done on server side
                if ((omodel != null))
                {
                    string lstrDefaultDate = "1980-01-01";
                    if (lintExchangeID == BowConstants.EX_MCX_VALUE || lintExchangeID == BowConstants.EX_DGCX_VALUE)
                    {
                        lstrDefaultDate = "1970-01-01";
                    }
                    llngTodaysDate = Convert.ToInt64((DateTime.Today - Convert.ToDateTime(lstrDefaultDate)).TotalSeconds);//   DateAndTime.DateDiff(DateInterval.Second, Convert.ToDateTime(lstrDefaultDate), DateAndTime.Today);
                    //: Get the long value of expiry date
                    //: Divide by number of seconds in a day to get number of days 
                    //: and add 1 day to it.
                    llngExpiry = (Convert.ToInt64(omodel.ExpiryDate) / 86400);
                    //: Multiply by 86400 to get back number of seconds for the expiry date
                    llngExpiry = llngExpiry * 86400;
                    //: Since divide rounds off the value if after multiplying the value we get is
                    //: greater than the original value then subtract number of seconds in one day to get correct date.
                    if (llngExpiry > Convert.ToInt64(omodel.ExpiryDate))
                    {
                        llngExpiry = llngExpiry - 86400;
                    }
                    if (llngExpiry - llngTodaysDate < 0)
                    {
                        //: Contract has Matured - This check will be done on server side
                    }
                    else
                    {
                        //: Check whether Contract has Matured w.r.t GoodTilldate
                        //: Check GoodTillDate w.r.t. Expiry Date
                        if (lintExchangeID != BowConstants.EX_NSE_VALUE && omodel.GoodTillCancel.Trim().ToUpper() == BowConstants.GOOD_TILL_DAYS.ToUpper())
                        {
                            if (omodel.GoodTillDate.Trim().Length > 0)
                            {
                                llngExpiry = llngExpiry + 86400;
                                if (Convert.ToInt64(omodel.GoodTillDate.Trim()) > 0)
                                {
                                    if ((Convert.ToInt64(omodel.GoodTillDate.Trim()) * 86400) + llngTodaysDate > llngExpiry)
                                    {
                                        //:  "The GTD Date is beyond the Maturity Date of the contract" - This check will be performed at the server side.
                                    }
                                    llngDays = (llngExpiry - llngTodaysDate) / 86400;
                                    if (llngDays <= 7)
                                    {
                                        //: "You cannot enter GTD orders for this Contract as the maturity is within a week." - This check will be performed at the server side.
                                    }
                                }
                            }
                        }
                    }
                }
                //:
                //: Validate Volume
                lstrMessage = ValidateVolume(omodel.Quantity, 9999999999L, 1, 0, false);
                if (lstrMessage.Trim().ToUpper() != "Success".ToUpper())
                {
                    pstrError = "Quantity : " + lstrMessage;
                    //pintPropertyId = FNOBuySellParameters.Volume;
                    return false;
                }
                ////: Validate for Maximum Order Volume if AllowQtyAboveMaxQty flag is false
                //if ((Information.IsNumeric(modMDI.gstrMaxOrderQuantity) == true) && pblnAllowQtyAboveMaxQty == false)
                //{
                //    if ((Convert.ToInt64(modMDI.gstrMaxOrderQuantity) < long.Parse(omodel.Volume)))
                //    {
                //        lstrMessage = "The quantity entered is more than the Maximum Quantity " + modMDI.gstrMaxOrderQuantity + " set.";
                //        pstrError = lstrMessage;
                //        //pintPropertyId = FNOBuySellParameters.Volume;
                //        return false;
                //    }
                //}
                ////: Validate for Minimum Order Volume
                //if ((Information.IsNumeric(modMDI.gstrMinOrderQuantity) == true) && pblnAllowQtyAboveMaxQty == false)
                //{
                //    if ((Convert.ToInt64(modMDI.gstrMinOrderQuantity) > long.Parse(omodel.Volume)))
                //    {
                //        lstrMessage = "The quantity entered is less than the Minimum Quantity " + modMDI.gstrMinOrderQuantity + " set.";
                //        pstrError = lstrMessage;
                //        pintPropertyId = FNOBuySellParameters.Volume;
                //        return false;
                //    }
                //}
                //:
                //: Validate for Maximum Order Disclosed Volume
                //if ((Information.IsNumeric(modMDI.gstrMaxOrderQuantity) == true) && pblnAllowQtyAboveMaxQty == false)
                //{
                //    if ((Convert.ToInt64(modMDI.gstrMaxOrderQuantity) < long.Parse(omodel.RevealQty)))
                //    {
                //        lstrMessage = "The Disclosed quantity entered is more than the Maximum Quantity " + modMDI.gstrMaxOrderQuantity + " set.";
                //        pstrError = lstrMessage;
                //        //pintPropertyId = FNOBuySellParameters.DisclosedVolume;
                //        return false;
                //    }
                //}
                //: Disclose volume should not be greater than actual volume
                if (omodel.RevealQty > 0 && omodel.Price > 0)
                {
                    if (omodel.RevealQty > omodel.Price)
                    {
                        pstrError = "The Disclosed quantity cannot be greater than the actual quantity.";
                        //pintPropertyId = FNOBuySellParameters.DisclosedVolume;
                        return false;
                    }
                }
                //: Disclosed volume should be in multiples of BoardLot
                if ((lintExchangeID != BowConstants.EX_MCX_VALUE && lintExchangeID != BowConstants.EX_DGCX_VALUE) && (lintExchangeID != BowConstants.EX_USE_VALUE && lintMarketID != BowConstants.MKT_CURRENCY_VALUE))
                {
                    if (omodel.RevealQty > 0)
                    {
                        if ((omodel != null))
                        {
                            if (omodel.RevealQty % omodel.MarketLot != 0)
                            {
                                lstrMessage = "Please enter a valid Disclosed Qty which is multiple of " + omodel.MarketLot;
                                pstrError = lstrMessage;
                                //pintPropertyId = FNOBuySellParameters.DisclosedVolume;
                                return false;
                            }
                        }
                    }
                }
                //: Disclosed volume should not be less than 10% of Volume (MCX 25%)
                if (lintMarketID == BowConstants.MKT_COMMODITIES_VALUE && (lintExchangeID == BowConstants.EX_MCX_VALUE || lintExchangeID == BowConstants.EX_DGCX_VALUE))
                {
                    ldblDQPercentageCheck = 25;
                }
                else if (lintMarketID == BowConstants.MKT_CURRENCY_VALUE && lintExchangeID == BowConstants.EX_MCX_VALUE)
                {
                    ldblDQPercentageCheck = 20;
                }
                else
                {
                    ldblDQPercentageCheck = 10;
                }
                if (lintExchangeID != BowConstants.EX_BSE_VALUE)
                {
                    if (omodel.RevealQty > 0)
                    {
                        if (omodel.RevealQty < omodel.Price * (ldblDQPercentageCheck / 100))
                        {
                            lstrMessage = "Please enter a valid Reveal Qty which more than " + ldblDQPercentageCheck + "% of Qty " + omodel.Quantity + ". ";
                            pstrError = lstrMessage;
                            //pintPropertyId = FNOBuySellParameters.DisclosedVolume;
                            return false;
                        }
                    }
                }
                //:
                //: Validate Price
                //if (lintMarketID == BowConstants.MKT_CURRENCY_VALUE)
                //{
                //    //If lintExchangeID = mobjBusinessLogic.EMS.GetExchangeId(BowConstants.EX_USE) Then
                //    lstrMessage = ValidatePrice(rate, 9999999999.0, -9999999999.0, 4, true);
                //    //Else
                //    //    lstrMessage = ValidatePrice(Me.Price, 9999999999.0, 0, 4, True)
                //    //End If
                //}
                //else
                //{
                //    lstrMessage = ValidatePrice(rate, 9999999999.0, -9999999999.0, 2, true);
                //}

                if (lstrMessage.Trim().ToUpper() != "Success".ToUpper())
                {
                    pstrError = lstrMessage;
                    // pintPropertyId = FNOBuySellParameters.Price;
                    return false;
                }
                //:For protection %
                if (pblncheckProtPercn == true)
                {
                    if ((lintExchangeID != BowConstants.EX_BSE_VALUE && lintMarketID != BowConstants.MKT_DERIVATIVE_VALUE && lintMarketID != BowConstants.MKT_CURRENCY_VALUE))
                    {
                        double ProtectionPercent;
                        if (omodel.Price > 0)
                        {
                            // if (lContract.IsDefaultOrdSettings == true)
                            //{
                            if (double.TryParse(omodel.ProtectionPercentage, out ProtectionPercent) == true)
                            {
                                if (validateForProtPercentage(omodel, ProtectionPercent, ref pstrError) == false)
                                {
                                    // pintPropertyId = FNOBuySellParameters.Price;
                                    return false;
                                }
                            }
                        }
                    }
                }
                //: Validate maximum order value (price*volume).
                //if ((Information.IsNumeric(modMDI.gstrMaxOrderValue) == true))
                //{
                //    if (lintMarketID == BowConstants.MKT_CURRENCY_VALUE)
                //    {
                //        if (lintExchangeID == BowConstants.EX_USE_VALUE)
                //        {
                //            //:Hardcoded 100 division as market condition not available during save in ldb
                //            llngOrderValue = (long.Parse(ShiftDecimalInText(omodel.Price, 4)) * omodel.Price * omodel.MarketLot) / 100;
                //        }
                //        else
                //        {
                //            llngOrderValue = long.Parse(ShiftDecimalInText(omodel.Price, 4)) * omodel.Price);
                //        }

                //    }
                //    else
                //    {
                //        llngOrderValue = long.Parse(ShiftDecimalInText(omodel.Price, 2)) * long.Parse(omodel.Price);
                //    }
                //    if ((Convert.ToInt64(modMDI.gstrMaxOrderValue) < llngOrderValue))
                //    {
                //        if (pblnAllowValueAboveMaxValue == false)
                //        {
                //            lstrMessage = "The value entered is more than the Maximum Value " + Strings.Format("###0.00", Convert.ToInt64(modMDI.gstrMaxOrderValue) / 100) + " set.";
                //            pstrError = lstrMessage;
                //            //pintPropertyId = (FNOBuySellParameters.Price * FNOBuySellParameters.Volume);
                //            return false;
                //        }
                //    }
                //}
                //: Validate Price w.r.t. TickSize
                if ((omodel != null))
                {
                    if (lintExchangeID == BowConstants.EX_NSE_VALUE || lintExchangeID == BowConstants.EX_NCDEX_VALUE ||
                        lintExchangeID == BowConstants.EX_NMCE_VALUE || lintExchangeID == BowConstants.EX_BSE_VALUE || lintExchangeID == BowConstants.EX_USE_VALUE)
                    {
                        if (omodel.Price > 0)
                        {
                            if (lintMarketID == BowConstants.MKT_CURRENCY_VALUE)
                            {
                                if (!(Convert.ToInt64(ShiftDecimalInText(rate, 7)) % Convert.ToInt64(omodel.TickSize) == 0))
                                {
                                    lstrMessage = "Please enter a valid Price which is a multiple of " + Convert.ToInt64(omodel.TickSize) / 10000000.0 + " Rupees ";
                                    pstrError = lstrMessage;
                                    //pintPropertyId = FNOBuySellParameters.Price;
                                    return false;
                                }
                            }
                            else
                            {
                                if (!(Convert.ToInt64(ShiftDecimalInText(rate, 2)) % Convert.ToInt64(omodel.TickSize) == 0))
                                {
                                    lstrMessage = "Please enter a valid Price which is a multiple of " + omodel.TickSize + " paise";
                                    pstrError = lstrMessage;
                                    //pintPropertyId = FNOBuySellParameters.Price;
                                    return false;
                                }
                            }
                        }
                    }
                }
                //: Validate Trigger Price as numeric
                if (lintExchangeID == BowConstants.EX_NMCE_VALUE)
                {
                    bool validate = ValidatePrice(rate, trgPrice, omodel, ref lstrMessage, 4);
                }
                else
                {
                    if (lintMarketID == BowConstants.MKT_CURRENCY_VALUE)
                    {
                        lstrMessage = ValidateVolume(omodel.TriggerPrice, 9999999999.0, -9999999999.0, 4, true);
                    }
                    else
                    {
                        lstrMessage = ValidateVolume(omodel.TriggerPrice, 9999999999.0, -9999999999.0, 2, true);
                    }
                }
                if (lstrMessage.Trim().ToUpper() != "Success".ToUpper())
                {
                    pstrError = lstrMessage;
                    //pintPropertyId = FNOBuySellParameters.TriggerPrice;
                    return false;
                }
                //: if SL order then Trigger price should be present
                if (omodel.OrderType == BowConstants.ORDER_TYPE_STOPLOSS_NONCONV_VALUE || omodel.OrderType == BowConstants.ORDER_TYPE_SL_VALUE)
                {
                    if (omodel.TriggerPrice == 0)
                    {
                        lstrMessage = "Please enter TriggerPrice for StopLoss order.";
                        pstrError = lstrMessage;
                        //pintPropertyId = FNOBuySellParameters.TriggerPrice;
                        return false;
                    }
                }
                //: Validate Trigger Price w.r.t Buy or Sell price.
                if (lintExchangeID != BowConstants.EX_NMCE_VALUE && lintExchangeID != BowConstants.EX_BSE_VALUE && lintExchangeID != BowConstants.EX_USE_VALUE)
                {
                    if (omodel.TriggerPrice > 0)
                    {
                        llngTriggerPrice = Convert.ToInt64(Convert.ToDouble(omodel.TriggerPrice) * 100);
                        if ((llngTriggerPrice > 0))
                        {
                            if (omodel.Price > 0)
                            {
                                llngPrice = Convert.ToInt64(Convert.ToDouble(omodel.Price) * 100);
                                if (llngPrice != 0)
                                {
                                    if ((omodel.BuySellIndicator.ToUpper() == BowConstants.BUY.ToUpper()))
                                    {
                                        if ((llngTriggerPrice > llngPrice))
                                        {
                                            lstrMessage = "The Trigger Price should be less than the Price.";
                                            pstrError = lstrMessage;
                                            //pintPropertyId = FNOBuySellParameters.TriggerPrice;
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                        if ((llngTriggerPrice < llngPrice))
                                        {
                                            lstrMessage = "The Trigger Price should be greater than the Price.";
                                            pstrError = lstrMessage;
                                            //pintPropertyId = FNOBuySellParameters.TriggerPrice;
                                            return false;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                lstrMessage = "Please enter a valid Price and Trigger Price";
                                pstrError = lstrMessage;
                                //pintPropertyId = FNOBuySellParameters.Price;
                                return false;
                            }
                        }
                    }
                }
                //: Validate TriggerPrice w.r.t. TickSize
                if ((omodel != null))
                {
                    if (lintExchangeID == BowConstants.EX_NSE_VALUE || lintExchangeID == BowConstants.EX_NCDEX_VALUE || lintExchangeID == BowConstants.EX_NMCE_VALUE || lintExchangeID == BowConstants.EX_BSE_VALUE || lintExchangeID == BowConstants.EX_USE_VALUE)
                    {
                        if ((omodel.TriggerPrice > 0))
                        {
                            if (lintMarketID == BowConstants.MKT_CURRENCY_VALUE)
                            {
                                if (!(Convert.ToInt64(ShiftDecimalInText(trgPrice, 7)) % Convert.ToInt64(omodel.TickSize) == 0))
                                {
                                    lstrMessage = "Please enter a valid Trigger Price which is a multiple of " + Convert.ToDouble(omodel.TickSize) / 10000000 + " Rupees ";
                                    pstrError = lstrMessage;
                                    // pintPropertyId = FNOBuySellParameters.TriggerPrice;
                                    return false;
                                }
                            }
                            else
                            {
                                if (!(Convert.ToInt64(ShiftDecimalInText(trgPrice, 2)) % Convert.ToInt64(omodel.TickSize) == 0))
                                {
                                    lstrMessage = "Please enter a valid Trigger Price which is a multiple of " + omodel.TickSize + " paise";
                                    pstrError = lstrMessage;
                                    //pintPropertyId = FNOBuySellParameters.TriggerPrice;
                                    return false;
                                }
                            }
                        }
                    }
                }
                //:
                //: Validate Volume w.r.t. BoardLot
                if ((lintExchangeID != BowConstants.EX_MCX_VALUE && lintExchangeID != BowConstants.EX_DGCX_VALUE) && (lintExchangeID != BowConstants.EX_USE_VALUE && lintMarketID != BowConstants.MKT_CURRENCY_VALUE))
                {
                    if ((omodel != null))
                    {
                        ldecQuantity = omodel.Price;
                        ldecBoardLotQty = omodel.MarketLot;
                        if ((ldecQuantity % ldecBoardLotQty != 0))
                        {
                            lstrMessage = "Please enter a valid Qty which is multiple of " + ldecBoardLotQty;
                            pstrError = lstrMessage;
                            // pintPropertyId = FNOBuySellParameters.Volume;
                            return false;
                        }
                    }
                }
                //: BackOffice Id
                if (objUtilityLoginDetails.UserBackOfficeId.Trim().Length == 0)
                {
                    lstrMessage = "Please enter a BackOfficeId";
                    pstrError = lstrMessage;
                    //pintPropertyId = FNOBuySellParameters.BackOfficeId;
                    return false;
                }
                //: If the Clearing or Trading Member has Logged on and is trying to place an order for him self then disallowing any such order.
                //if (modMDI.gblnDisallowOrderEntryForSelfFOR_C_OR_T == true &&
                //    modMDI.gobjUtilityLoginDetails.UserLoginId.Trim().ToUpper() == objUtilityLoginDetails.UserBackOfficeId.Trim().ToUpper() && omodel.ProClientIndicator == false)
                //{
                //    lstrMessage = "Order entry for self not allowed.";
                //    pstrError = lstrMessage;
                //    // pintPropertyId = FNOBuySellParameters.BackOfficeId;
                //    return false;
                //}

            }
            catch (Exception ex)
            {
                Infrastructure.Logger.WriteLog("Error in Derivative Validate Order." + ex.Message.ToString());
                Infrastructure.Logger.WriteLog("Error in Derivative Validate Order." + ex.StackTrace.ToString());
                return false;
            }
            return true;

        }
        public bool validateForProtPercentage(OrderModel omodel, double plngProtper, ref string pstrError)
        {

            bool lblnRetval = true;
            double ldblLtp = BroadcastMasterMemory.objScripDetailsConDict.Where(x => x.Key == omodel.ScripCode).Select(x => x.Value.lastTradeRateL).FirstOrDefault();
            double ldblval = 0;
            if (ldblLtp > 0)
                ldblval = ((ldblLtp / 100) * plngProtper);
            if (omodel.BuySellIndicator == BowConstants.BUY)
            {
                if (Convert.ToDouble(omodel.Price) >= (ldblLtp + ldblval))
                {
                    pstrError = "Buy Price is greater than set limit of prot %";
                    lblnRetval = false;
                }
            }
            else
            {
                if (Convert.ToDouble(omodel.Price) <= (ldblLtp - ldblval))
                {
                    pstrError = "Sell Price is less than set limit of prot %";
                    lblnRetval = false;
                }
            }
            return lblnRetval;

        }

        public static string ValidateGTD(string pstrGTDValue)
        {
            if (!string.IsNullOrEmpty(pstrGTDValue.Trim()))
            {
                if (Convert.ToInt64(pstrGTDValue) > 7)
                {
                    return "GTD should not be greater than 7 Days";
                }
            }
            return "";
        }


        public static string ShiftDecimalInText(string pstrNumberInText, long plngShiftBy)
        {
            int lintFoundAt;
            long lintShiftBy = 0;
            int lintCount;
            string lstrNumber;
            bool lblnFoundDecimal;
            lblnFoundDecimal = false;
            lstrNumber = "";
            if (!(pstrNumberInText == null))
            {
                pstrNumberInText = pstrNumberInText.Trim();
                lintFoundAt = pstrNumberInText.IndexOf(".");
                if ((plngShiftBy > 0))
                {
                    for (lintCount = 0; lintCount <= pstrNumberInText.Length - 1; lintCount++)
                    {
                        if ((lintShiftBy == plngShiftBy))
                        {
                            lstrNumber = lstrNumber + ".";
                        }

                        if (pstrNumberInText.ToCharArray().GetValue(lintCount).ToString() == ".")
                        {
                            lblnFoundDecimal = true;
                        }
                        else
                        {
                            lstrNumber = lstrNumber + pstrNumberInText.ToCharArray().GetValue(lintCount).ToString();
                            if ((lblnFoundDecimal))
                            {
                                lintShiftBy += 1;
                            }
                        }
                    }

                    for (lintCount = Convert.ToInt32(lintShiftBy); lintCount <= plngShiftBy - 1; lintCount++)
                    {
                        lstrNumber = lstrNumber + "0";
                    }
                }
                else if ((plngShiftBy < 0))
                {
                    plngShiftBy = plngShiftBy * -1;
                    lblnFoundDecimal = pstrNumberInText.IndexOf(".") < 0;
                    for (lintCount = pstrNumberInText.Length - 1; lintCount >= 0; lintCount += -1)
                    {
                        if ((lintShiftBy == plngShiftBy))
                        {
                            lstrNumber = "." + lstrNumber;
                        }

                        if (pstrNumberInText.ToCharArray().GetValue(lintCount).ToString() == ".")
                        {
                            lblnFoundDecimal = true;
                        }
                        else
                        {
                            lstrNumber = pstrNumberInText.ToCharArray().GetValue(lintCount).ToString() + lstrNumber;
                            if ((lblnFoundDecimal))
                            {
                                lintShiftBy += 1;
                            }
                        }
                    }

                    if (lintShiftBy < plngShiftBy)
                    {
                        for (lintCount = Convert.ToInt32(lintShiftBy); lintCount <= plngShiftBy - 1; lintCount++)
                        {
                            lstrNumber = "0" + lstrNumber;
                        }

                        lstrNumber = "0." + lstrNumber;
                    }
                }
            }

            return lstrNumber;
        }
#endif
        #endregion

    }
}
