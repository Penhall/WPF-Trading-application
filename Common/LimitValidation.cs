using CommonFrontEnd.Global;
using CommonFrontEnd.Model.Order;
using CommonFrontEnd.Model.Trade;
using CommonFrontEnd.SharedMemories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CommonFrontEnd.Common
{
    class LimitValidation
    {
#if TWS

        //public static bool ValidateLimit(OrderModel omodel, int decimal_point, ref string Validate_Message)
        //{
        //    bool result = false;
        //    try
        //    {
        //        if (omodel != null)
        //        {
        //            #region when order is placed or added
        //            if (omodel.OrderAction == Enumerations.Order.Modes.A.ToString())
        //            {
        //                if (omodel.BuySellIndicator == Enumerations.Order.BuySellFlag.BUY.ToString())
        //                {
        //                    //omodel.Price = Convert.ToInt64(Convert.ToDouble(omodel.Price) / Math.Pow(10, decimal_point));
        //                    double Value = omodel.Price * omodel.Quantity;
        //                    Value = Value / Math.Pow(10, decimal_point);

        //                    //Validation for Chcecking Gross Buy Limits
        //                    if (Value <= UtilityLimitDetails.GrossBuyLimit)
        //                    {
        //                        UtilityLimitDetails.GrossBuyLimit = UtilityLimitDetails.GrossBuyLimit - Value;
        //                        result = true;
        //                    }
        //                    else
        //                    {
        //                        MessageBox.Show("Limit not available");
        //                        result = false;
        //                    }

        //                    //Validation for Chcecking GroupWise Buy Limits
        //                    //if ()
        //                    //{
        //                    //    MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlBuy = MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlBuy - omodel.Price;
        //                    //}
        //                }
        //                else if (omodel.BuySellIndicator == Enumerations.Order.BuySellFlag.SELL.ToString())
        //                {
        //                    double Value = omodel.Price * omodel.Quantity;
        //                    Value = Value / Math.Pow(10, decimal_point);

        //                    if (Value <= UtilityLimitDetails.GrossSellLimit)
        //                    {
        //                        UtilityLimitDetails.GrossSellLimit = UtilityLimitDetails.GrossSellLimit - Value;
        //                        result = true;
        //                    }
        //                    else
        //                    {
        //                        MessageBox.Show("Limit not available");
        //                        result = false;
        //                    }
        //                }
        //                //else if (omodel.Segment == Enumerations.Order.ScripSegment.Equity.ToString())
        //                //{
        //                //    if (omodel.Group = )
        //                //    {

        //                //    }
        //                //}

        //            }
        //            #endregion

        //            #region When order is updated
        //            else if (omodel.OrderAction == Enumerations.Order.Modes.U.ToString())
        //            {
        //                var price1 = MemoryManager.OrderDictionary[omodel.MessageTag].Price; //Main Memory
        //                var price2 = MemoryManager.OrderDictionaryBackupMemory[omodel.MessageTag].Price;//buffer
        //                var finalPrice = price2 - price1;

        //                var qty1 = MemoryManager.OrderDictionary[omodel.MessageTag].Quantity;
        //                var qty2 = MemoryManager.OrderDictionaryBackupMemory[omodel.MessageTag].Quantity;
        //                var finalqty = qty2 - qty1;

        //                #region Price is updated
        //                if (finalqty == 0)
        //                {
        //                    if (finalPrice > 0)
        //                    {
        //                        if (omodel.BuySellIndicator == Enumerations.Order.BuySellFlag.BUY.ToString())
        //                        {
        //                            //omodel.Price = Convert.ToInt64(Convert.ToDouble(omodel.Price) / Math.Pow(10, decimal_point));
        //                            double Value = finalPrice * omodel.Quantity;
        //                            Value = Value / Math.Pow(10, decimal_point);

        //                            if (Value <= UtilityLimitDetails.GrossBuyLimit)
        //                            {
        //                                UtilityLimitDetails.GrossBuyLimit = UtilityLimitDetails.GrossBuyLimit - Value;
        //                                result = true;
        //                            }
        //                            else
        //                            {
        //                                MessageBox.Show("Limit not available");
        //                                result = false;
        //                            }
        //                        }
        //                        else if (omodel.BuySellIndicator == Enumerations.Order.BuySellFlag.SELL.ToString())
        //                        {
        //                            double Value = finalPrice * omodel.Quantity;
        //                            Value = Value / Math.Pow(10, decimal_point);

        //                            if (Value <= UtilityLimitDetails.GrossSellLimit)
        //                            {
        //                                UtilityLimitDetails.GrossSellLimit = UtilityLimitDetails.GrossSellLimit - Value;
        //                                result = true;
        //                            }
        //                            else
        //                            {
        //                                MessageBox.Show("Limit not available");
        //                                result = false;
        //                            }
        //                        }
        //                    }
        //                    else if (finalPrice < 0)
        //                    {
        //                        if (omodel.BuySellIndicator == Enumerations.Order.BuySellFlag.BUY.ToString())
        //                        {
        //                            //omodel.Price = Convert.ToInt64(Convert.ToDouble(omodel.Price) / Math.Pow(10, decimal_point));
        //                            finalPrice = Math.Abs(finalPrice);
        //                            double Value = finalPrice * omodel.Quantity;
        //                            Value = Value / Math.Pow(10, decimal_point);

        //                            if (Value <= UtilityLimitDetails.GrossBuyLimit)
        //                            {
        //                                UtilityLimitDetails.GrossBuyLimit = UtilityLimitDetails.GrossBuyLimit + Value;
        //                                result = true;
        //                            }
        //                            else
        //                            {
        //                                MessageBox.Show("Limit not available");
        //                                result = false;
        //                            }
        //                        }
        //                        else if (omodel.BuySellIndicator == Enumerations.Order.BuySellFlag.SELL.ToString())
        //                        {
        //                            finalPrice = Math.Abs(finalPrice);
        //                            double Value = finalPrice * omodel.Quantity;
        //                            Value = Value / Math.Pow(10, decimal_point);

        //                            if (Value <= UtilityLimitDetails.GrossSellLimit)
        //                            {
        //                                UtilityLimitDetails.GrossSellLimit = UtilityLimitDetails.GrossSellLimit + Value;
        //                                result = true;
        //                            }
        //                            else
        //                            {
        //                                MessageBox.Show("Limit not available");
        //                                result = false;
        //                            }
        //                        }
        //                    }
        //                }
        //                #endregion

        //                #region Quantity is updated
        //                if (finalPrice == 0)
        //                {
        //                    if (finalqty > 0)
        //                    {
        //                        if (omodel.BuySellIndicator == Enumerations.Order.BuySellFlag.BUY.ToString())
        //                        {
        //                            //omodel.Price = Convert.ToInt64(Convert.ToDouble(omodel.Price) / Math.Pow(10, decimal_point));
        //                            double Value = omodel.Price * finalqty;
        //                            Value = Value / Math.Pow(10, decimal_point);

        //                            if (Value <= UtilityLimitDetails.GrossBuyLimit)
        //                            {
        //                                UtilityLimitDetails.GrossBuyLimit = UtilityLimitDetails.GrossBuyLimit - Value;
        //                                result = true;
        //                            }
        //                            else
        //                            {
        //                                MessageBox.Show("Limit not available");
        //                                result = false;
        //                            }
        //                        }
        //                        else if (omodel.BuySellIndicator == Enumerations.Order.BuySellFlag.SELL.ToString())
        //                        {
        //                            double Value = omodel.Price * finalqty;
        //                            Value = Value / Math.Pow(10, decimal_point);

        //                            if (Value <= UtilityLimitDetails.GrossSellLimit)
        //                            {
        //                                UtilityLimitDetails.GrossSellLimit = UtilityLimitDetails.GrossSellLimit - Value;
        //                                result = true;
        //                            }
        //                            else
        //                            {
        //                                MessageBox.Show("Limit not available");
        //                                result = false;
        //                            }
        //                        }
        //                    }
        //                    else if (finalqty < 0)
        //                    {
        //                        if (omodel.BuySellIndicator == Enumerations.Order.BuySellFlag.BUY.ToString())
        //                        {
        //                            //omodel.Price = Convert.ToInt64(Convert.ToDouble(omodel.Price) / Math.Pow(10, decimal_point));
        //                            finalqty = Math.Abs(finalqty);
        //                            double Value = omodel.Price * finalqty;
        //                            Value = Value / Math.Pow(10, decimal_point);

        //                            if (Value <= UtilityLimitDetails.GrossBuyLimit)
        //                            {
        //                                UtilityLimitDetails.GrossBuyLimit = UtilityLimitDetails.GrossBuyLimit + Value;
        //                                result = true;
        //                            }
        //                            else
        //                            {
        //                                MessageBox.Show("Limit not available");
        //                                result = false;
        //                            }
        //                        }
        //                        else if (omodel.BuySellIndicator == Enumerations.Order.BuySellFlag.SELL.ToString())
        //                        {
        //                            finalqty = Math.Abs(finalqty);
        //                            double Value = omodel.Price * finalqty;
        //                            Value = Value / Math.Pow(10, decimal_point);

        //                            if (Value <= UtilityLimitDetails.GrossSellLimit)
        //                            {
        //                                UtilityLimitDetails.GrossSellLimit = UtilityLimitDetails.GrossSellLimit + Value;
        //                                result = true;
        //                            }
        //                            else
        //                            {
        //                                MessageBox.Show("Limit not available");
        //                                result = false;
        //                            }
        //                        }
        //                    }
        //                }
        //                #endregion


        //                else if (finalPrice > 0 && finalqty > 0)
        //                {
        //                    if (omodel.BuySellIndicator == Enumerations.Order.BuySellFlag.BUY.ToString())
        //                    {
        //                        //omodel.Price = Convert.ToInt64(Convert.ToDouble(omodel.Price) / Math.Pow(10, decimal_point));
        //                        double Value = finalPrice * finalqty;
        //                        Value = Value / Math.Pow(10, decimal_point);

        //                        if (Value <= UtilityLimitDetails.GrossBuyLimit)
        //                        {
        //                            UtilityLimitDetails.GrossBuyLimit = UtilityLimitDetails.GrossBuyLimit - Value;
        //                            result = true;
        //                        }
        //                        else
        //                        {
        //                            MessageBox.Show("Limit not available");
        //                            result = false;
        //                        }
        //                    }
        //                    else if (omodel.BuySellIndicator == Enumerations.Order.BuySellFlag.SELL.ToString())
        //                    {
        //                        double Value = finalPrice * finalqty;
        //                        Value = Value / Math.Pow(10, decimal_point);

        //                        if (Value <= UtilityLimitDetails.GrossSellLimit)
        //                        {
        //                            UtilityLimitDetails.GrossSellLimit = UtilityLimitDetails.GrossSellLimit - Value;
        //                            result = true;
        //                        }
        //                        else
        //                        {
        //                            MessageBox.Show("Limit not available");
        //                            result = false;
        //                        }
        //                    }
        //                }

        //                else if (finalPrice < 0 && finalqty < 0)
        //                {
        //                    if (omodel.BuySellIndicator == Enumerations.Order.BuySellFlag.BUY.ToString())
        //                    {
        //                        //omodel.Price = Convert.ToInt64(Convert.ToDouble(omodel.Price) / Math.Pow(10, decimal_point));
        //                        finalPrice = Math.Abs(finalPrice);
        //                        finalqty = Math.Abs(finalqty);
        //                        double Value = finalPrice * finalqty;
        //                        Value = Value / Math.Pow(10, decimal_point);

        //                        if (Value <= UtilityLimitDetails.GrossBuyLimit)
        //                        {
        //                            UtilityLimitDetails.GrossBuyLimit = UtilityLimitDetails.GrossBuyLimit + Value;
        //                            result = true;
        //                        }
        //                        else
        //                        {
        //                            MessageBox.Show("Limit not available");
        //                            result = false;
        //                        }
        //                    }
        //                    else if (omodel.BuySellIndicator == Enumerations.Order.BuySellFlag.SELL.ToString())
        //                    {
        //                        finalPrice = Math.Abs(finalPrice);
        //                        finalqty = Math.Abs(finalqty);
        //                        double Value = finalPrice * finalqty;
        //                        Value = Value / Math.Pow(10, decimal_point);

        //                        if (Value <= UtilityLimitDetails.GrossSellLimit)
        //                        {
        //                            UtilityLimitDetails.GrossSellLimit = UtilityLimitDetails.GrossSellLimit + Value;
        //                            result = true;
        //                        }
        //                        else
        //                        {
        //                            MessageBox.Show("Limit not available");
        //                            result = false;
        //                        }
        //                    }
        //                }

        //            }
        //            #endregion

        //            #region When order is deleted
        //            else if (omodel.OrderAction == Enumerations.Order.Modes.D.ToString())
        //            {
        //                if (omodel.BuySellIndicator == Enumerations.Order.BuySellFlag.BUY.ToString())
        //                {
        //                    //omodel.Price = Convert.ToInt64(Convert.ToDouble(omodel.Price) / Math.Pow(10, decimal_point));
        //                    double Value = omodel.Price * omodel.Quantity;
        //                    Value = Value / Math.Pow(10, decimal_point);

        //                    if (Value <= UtilityLimitDetails.GrossBuyLimit)
        //                    {
        //                        UtilityLimitDetails.GrossBuyLimit = UtilityLimitDetails.GrossBuyLimit + Value;
        //                        result = true;
        //                    }
        //                    else
        //                    {
        //                        MessageBox.Show("Limit not available");
        //                        result = false;
        //                    }
        //                }
        //                else if (omodel.BuySellIndicator == Enumerations.Order.BuySellFlag.SELL.ToString())
        //                {
        //                    double Value = omodel.Price * omodel.Quantity;
        //                    Value = Value / Math.Pow(10, decimal_point);

        //                    if (Value <= UtilityLimitDetails.GrossSellLimit)
        //                    {
        //                        UtilityLimitDetails.GrossSellLimit = UtilityLimitDetails.GrossSellLimit + Value;
        //                        result = true;
        //                    }
        //                    else
        //                    {
        //                        MessageBox.Show("Limit not available");
        //                        result = false;
        //                    }
        //                }
        //            }
        //            #endregion
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionUtility.LogError(ex);
        //        Validate_Message = "Error In Validation of Limit";
        //        result = false;
        //    }

        //    return result;
        //}


        //public static bool InvokeValidateLimit(OrderModel omodel, int decimal_point)
        //{
        //    bool result = false;

        //    try
        //    {
        //        if (omodel != null)
        //        {
        //            #region Error in placing order
        //            if (omodel.OrderAction == Enumerations.Order.Modes.A.ToString())
        //            {
        //                if (omodel.BuySellIndicator == Enumerations.Order.BuySellFlag.BUY.ToString())
        //                {
        //                    //omodel.Price = Convert.ToInt64(Convert.ToDouble(omodel.Price) / Math.Pow(10, decimal_point));
        //                    double Value = omodel.Price * omodel.Quantity;
        //                    Value = Value / Math.Pow(10, decimal_point);

        //                    UtilityLimitDetails.GrossBuyLimit = UtilityLimitDetails.GrossBuyLimit + Value;
        //                    result = true;

        //                }
        //                else if (omodel.BuySellIndicator == Enumerations.Order.BuySellFlag.SELL.ToString())
        //                {
        //                    double Value = omodel.Price * omodel.Quantity;
        //                    Value = Value / Math.Pow(10, decimal_point);

        //                    UtilityLimitDetails.GrossSellLimit = UtilityLimitDetails.GrossSellLimit + Value;
        //                    result = true;
        //                }
        //            }

        //            #endregion

        //            #region Error in Deleting Order

        //            if (omodel.OrderAction == Enumerations.Order.Modes.D.ToString())
        //            {
        //                if (omodel.BuySellIndicator == Enumerations.Order.BuySellFlag.BUY.ToString())
        //                {
        //                    //omodel.Price = Convert.ToInt64(Convert.ToDouble(omodel.Price) / Math.Pow(10, decimal_point));
        //                    double Value = omodel.Price * omodel.Quantity;
        //                    Value = Value / Math.Pow(10, decimal_point);

        //                    UtilityLimitDetails.GrossBuyLimit = UtilityLimitDetails.GrossBuyLimit - Value;
        //                    result = true;
        //                }
        //                else if (omodel.BuySellIndicator == Enumerations.Order.BuySellFlag.SELL.ToString())
        //                {
        //                    double Value = omodel.Price * omodel.Quantity;
        //                    Value = Value / Math.Pow(10, decimal_point);

        //                    UtilityLimitDetails.GrossSellLimit = UtilityLimitDetails.GrossSellLimit - Value;
        //                    result = true;
        //                }
        //            }

        //            #endregion

        //            #region When order is updated
        //            else if (omodel.OrderAction == Enumerations.Order.Modes.U.ToString())
        //            {
        //                var price1 = MemoryManager.OrderDictionary[omodel.MessageTag].Price; //Main Memory
        //                var price2 = MemoryManager.OrderDictionaryBackupMemory[omodel.MessageTag].Price;//buffer
        //                var finalPrice = price2 - price1;

        //                var qty1 = MemoryManager.OrderDictionary[omodel.MessageTag].Quantity;
        //                var qty2 = MemoryManager.OrderDictionaryBackupMemory[omodel.MessageTag].Quantity;
        //                var finalqty = qty2 - qty1;

        //                #region Price is updated
        //                if (finalqty == 0)
        //                {
        //                    if (finalPrice > 0)
        //                    {
        //                        if (omodel.BuySellIndicator == Enumerations.Order.BuySellFlag.BUY.ToString())
        //                        {
        //                            //omodel.Price = Convert.ToInt64(Convert.ToDouble(omodel.Price) / Math.Pow(10, decimal_point));
        //                            double Value = finalPrice * omodel.Quantity;
        //                            Value = Value / Math.Pow(10, decimal_point);

        //                            if (Value <= UtilityLimitDetails.GrossBuyLimit)
        //                            {
        //                                UtilityLimitDetails.GrossBuyLimit = UtilityLimitDetails.GrossBuyLimit + Value;
        //                                result = true;
        //                            }
        //                            else
        //                            {
        //                                MessageBox.Show("Limit not available");
        //                                result = false;
        //                            }
        //                        }
        //                        else if (omodel.BuySellIndicator == Enumerations.Order.BuySellFlag.SELL.ToString())
        //                        {
        //                            double Value = finalPrice * omodel.Quantity;
        //                            Value = Value / Math.Pow(10, decimal_point);

        //                            if (Value <= UtilityLimitDetails.GrossSellLimit)
        //                            {
        //                                UtilityLimitDetails.GrossSellLimit = UtilityLimitDetails.GrossSellLimit + Value;
        //                                result = true;
        //                            }
        //                            else
        //                            {
        //                                MessageBox.Show("Limit not available");
        //                                result = false;
        //                            }
        //                        }
        //                    }
        //                    else if (finalPrice < 0)
        //                    {
        //                        if (omodel.BuySellIndicator == Enumerations.Order.BuySellFlag.BUY.ToString())
        //                        {
        //                            //omodel.Price = Convert.ToInt64(Convert.ToDouble(omodel.Price) / Math.Pow(10, decimal_point));
        //                            finalPrice = Math.Abs(finalPrice);
        //                            double Value = finalPrice * omodel.Quantity;
        //                            Value = Value / Math.Pow(10, decimal_point);

        //                            if (Value <= UtilityLimitDetails.GrossBuyLimit)
        //                            {
        //                                UtilityLimitDetails.GrossBuyLimit = UtilityLimitDetails.GrossBuyLimit - Value;
        //                                result = true;
        //                            }
        //                            else
        //                            {
        //                                MessageBox.Show("Limit not available");
        //                                result = false;
        //                            }
        //                        }
        //                        else if (omodel.BuySellIndicator == Enumerations.Order.BuySellFlag.SELL.ToString())
        //                        {
        //                            finalPrice = Math.Abs(finalPrice);
        //                            double Value = finalPrice * omodel.Quantity;
        //                            Value = Value / Math.Pow(10, decimal_point);

        //                            if (Value <= UtilityLimitDetails.GrossSellLimit)
        //                            {
        //                                UtilityLimitDetails.GrossSellLimit = UtilityLimitDetails.GrossSellLimit - Value;
        //                                result = true;
        //                            }
        //                            else
        //                            {
        //                                MessageBox.Show("Limit not available");
        //                                result = false;
        //                            }
        //                        }
        //                    }
        //                }
        //                #endregion

        //                #region Quantity is updated
        //                if (finalPrice == 0)
        //                {
        //                    if (finalqty > 0)
        //                    {
        //                        if (omodel.BuySellIndicator == Enumerations.Order.BuySellFlag.BUY.ToString())
        //                        {
        //                            //omodel.Price = Convert.ToInt64(Convert.ToDouble(omodel.Price) / Math.Pow(10, decimal_point));
        //                            double Value = omodel.Price * finalqty;
        //                            Value = Value / Math.Pow(10, decimal_point);

        //                            if (Value <= UtilityLimitDetails.GrossBuyLimit)
        //                            {
        //                                UtilityLimitDetails.GrossBuyLimit = UtilityLimitDetails.GrossBuyLimit + Value;
        //                                result = true;
        //                            }
        //                            else
        //                            {
        //                                MessageBox.Show("Limit not available");
        //                                result = false;
        //                            }
        //                        }
        //                        else if (omodel.BuySellIndicator == Enumerations.Order.BuySellFlag.SELL.ToString())
        //                        {
        //                            double Value = omodel.Price * finalqty;
        //                            Value = Value / Math.Pow(10, decimal_point);

        //                            if (Value <= UtilityLimitDetails.GrossSellLimit)
        //                            {
        //                                UtilityLimitDetails.GrossSellLimit = UtilityLimitDetails.GrossSellLimit + Value;
        //                                result = true;
        //                            }
        //                            else
        //                            {
        //                                MessageBox.Show("Limit not available");
        //                                result = false;
        //                            }
        //                        }
        //                    }
        //                    else if (finalqty < 0)
        //                    {
        //                        if (omodel.BuySellIndicator == Enumerations.Order.BuySellFlag.BUY.ToString())
        //                        {
        //                            //omodel.Price = Convert.ToInt64(Convert.ToDouble(omodel.Price) / Math.Pow(10, decimal_point));
        //                            finalqty = Math.Abs(finalqty);
        //                            double Value = omodel.Price * finalqty;
        //                            Value = Value / Math.Pow(10, decimal_point);

        //                            if (Value <= UtilityLimitDetails.GrossBuyLimit)
        //                            {
        //                                UtilityLimitDetails.GrossBuyLimit = UtilityLimitDetails.GrossBuyLimit - Value;
        //                                result = true;
        //                            }
        //                            else
        //                            {
        //                                MessageBox.Show("Limit not available");
        //                                result = false;
        //                            }
        //                        }
        //                        else if (omodel.BuySellIndicator == Enumerations.Order.BuySellFlag.SELL.ToString())
        //                        {
        //                            finalqty = Math.Abs(finalqty);
        //                            double Value = omodel.Price * finalqty;
        //                            Value = Value / Math.Pow(10, decimal_point);

        //                            if (Value <= UtilityLimitDetails.GrossSellLimit)
        //                            {
        //                                UtilityLimitDetails.GrossSellLimit = UtilityLimitDetails.GrossSellLimit - Value;
        //                                result = true;
        //                            }
        //                            else
        //                            {
        //                                MessageBox.Show("Limit not available");
        //                                result = false;
        //                            }
        //                        }
        //                    }
        //                }
        //                #endregion


        //                else if (finalPrice > 0 && finalqty > 0)
        //                {
        //                    if (omodel.BuySellIndicator == Enumerations.Order.BuySellFlag.BUY.ToString())
        //                    {
        //                        //omodel.Price = Convert.ToInt64(Convert.ToDouble(omodel.Price) / Math.Pow(10, decimal_point));
        //                        double Value = finalPrice * finalqty;
        //                        Value = Value / Math.Pow(10, decimal_point);

        //                        if (Value <= UtilityLimitDetails.GrossBuyLimit)
        //                        {
        //                            UtilityLimitDetails.GrossBuyLimit = UtilityLimitDetails.GrossBuyLimit + Value;
        //                            result = true;
        //                        }
        //                        else
        //                        {
        //                            MessageBox.Show("Limit not available");
        //                            result = false;
        //                        }
        //                    }
        //                    else if (omodel.BuySellIndicator == Enumerations.Order.BuySellFlag.SELL.ToString())
        //                    {
        //                        double Value = finalPrice * finalqty;
        //                        Value = Value / Math.Pow(10, decimal_point);

        //                        if (Value <= UtilityLimitDetails.GrossSellLimit)
        //                        {
        //                            UtilityLimitDetails.GrossSellLimit = UtilityLimitDetails.GrossSellLimit + Value;
        //                            result = true;
        //                        }
        //                        else
        //                        {
        //                            MessageBox.Show("Limit not available");
        //                            result = false;
        //                        }
        //                    }
        //                }

        //                else if (finalPrice < 0 && finalqty < 0)
        //                {
        //                    if (omodel.BuySellIndicator == Enumerations.Order.BuySellFlag.BUY.ToString())
        //                    {
        //                        //omodel.Price = Convert.ToInt64(Convert.ToDouble(omodel.Price) / Math.Pow(10, decimal_point));
        //                        finalPrice = Math.Abs(finalPrice);
        //                        finalqty = Math.Abs(finalqty);
        //                        double Value = finalPrice * finalqty;
        //                        Value = Value / Math.Pow(10, decimal_point);

        //                        if (Value <= UtilityLimitDetails.GrossBuyLimit)
        //                        {
        //                            UtilityLimitDetails.GrossBuyLimit = UtilityLimitDetails.GrossBuyLimit - Value;
        //                            result = true;
        //                        }
        //                        else
        //                        {
        //                            MessageBox.Show("Limit not available");
        //                            result = false;
        //                        }
        //                    }
        //                    else if (omodel.BuySellIndicator == Enumerations.Order.BuySellFlag.SELL.ToString())
        //                    {
        //                        finalPrice = Math.Abs(finalPrice);
        //                        finalqty = Math.Abs(finalqty);
        //                        double Value = finalPrice * finalqty;
        //                        Value = Value / Math.Pow(10, decimal_point);

        //                        if (Value <= UtilityLimitDetails.GrossSellLimit)
        //                        {
        //                            UtilityLimitDetails.GrossSellLimit = UtilityLimitDetails.GrossSellLimit - Value;
        //                            result = true;
        //                        }
        //                        else
        //                        {
        //                            MessageBox.Show("Limit not available");
        //                            result = false;
        //                        }
        //                    }
        //                }

        //            }
        //            #endregion

        //        }


        //    }
        //    catch (Exception ex)
        //    {

        //        ExceptionUtility.LogError(ex);
        //        //Validate_Message = "Error In Validation of Limit";
        //        result = false;

        //    }

        //    return result;
        //}


        //public static bool ValidateGroupWiseLimit(OrderModel omodel, int decimal_point)
        //{
        //    bool result = false;
        //    GroupWiseLimitsModel oGroupWiseLimitsModel = new GroupWiseLimitsModel();
        //    try
        //    {
        //        if (omodel != null)
        //        {
        //            #region Adding Order in Order Book

        //            if (omodel.OrderAction == Enumerations.Order.Modes.A.ToString())
        //            {
        //                if (omodel.BuySellIndicator == Enumerations.Order.BuySellFlag.BUY.ToString())
        //                {
        //                    if (MasterSharedMemory.GroupWiseLimitDict.ContainsKey(omodel.Group))
        //                    {
        //                        double Value = omodel.Price * omodel.Quantity;
        //                        Value = Value / Math.Pow(10, decimal_point);

        //                        if (MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlBuy != -1)
        //                        {
        //                            if (Value <= MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlBuy)
        //                            {
        //                                MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlBuy = MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlBuy - Value;
        //                                // bool update = MasterSharedMemory.GroupWiseLimitDict.TryUpdate(omodel.Group, oGroupWiseLimitsModel, MasterSharedMemory.GroupWiseLimitDict[omodel.Group]);
        //                                //OnGroupwiseLimitReceive?.Invoke(); on response
        //                                result = true;
        //                            }
        //                            else
        //                            {
        //                                MessageBox.Show("GroupWise Limit not available");
        //                                result = false;
        //                            }
        //                        }
        //                    }
        //                }
        //                else if (omodel.BuySellIndicator == Enumerations.Order.BuySellFlag.SELL.ToString())
        //                {
        //                    if (MasterSharedMemory.GroupWiseLimitDict.ContainsKey(omodel.Group))
        //                    {
        //                        double Value = omodel.Price * omodel.Quantity;
        //                        Value = Value / Math.Pow(10, decimal_point);

        //                        if (MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlSell != -1)
        //                        {
        //                            if (Value <= MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlSell)
        //                            {
        //                                MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlSell = MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlSell - Value;
        //                                //bool update = MasterSharedMemory.GroupWiseLimitDict.TryUpdate(omodel.Group, oGroupWiseLimitsModel, MasterSharedMemory.GroupWiseLimitDict[omodel.Group]);
        //                                //OnGroupwiseLimitReceive?.Invoke();
        //                                result = true;
        //                            }
        //                            else
        //                            {
        //                                MessageBox.Show("GroupWise Limit not available");
        //                                result = false;
        //                            }
        //                        }
        //                    }
        //                }
        //            }


        //            #endregion

        //            #region Updating Order Request
        //            if (omodel.OrderAction == Enumerations.Order.Modes.U.ToString())
        //            {
        //                var price1 = MemoryManager.OrderDictionary[omodel.MessageTag].Price; //Main Memory
        //                var price2 = MemoryManager.OrderDictionaryBackupMemory[omodel.MessageTag].Price;//buffer
        //                var finalPrice = price2 - price1;

        //                var qty1 = MemoryManager.OrderDictionary[omodel.MessageTag].Quantity;
        //                var qty2 = MemoryManager.OrderDictionaryBackupMemory[omodel.MessageTag].Quantity;
        //                var finalqty = qty2 - qty1;

        //                #region Price is updated
        //                if (finalqty == 0)
        //                {
        //                    if (finalPrice > 0)
        //                    {
        //                        if (omodel.BuySellIndicator == Enumerations.Order.BuySellFlag.BUY.ToString())
        //                        {
        //                            //omodel.Price = Convert.ToInt64(Convert.ToDouble(omodel.Price) / Math.Pow(10, decimal_point));
        //                            double Value = finalPrice * omodel.Quantity;
        //                            Value = Value / Math.Pow(10, decimal_point);

        //                            if (MasterSharedMemory.GroupWiseLimitDict.ContainsKey(omodel.Group))
        //                            {
        //                                if (MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlBuy != -1)
        //                                {
        //                                    if (Value <= MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlBuy)
        //                                    {
        //                                        MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlBuy = MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlBuy - Value;
        //                                        // bool update = MasterSharedMemory.GroupWiseLimitDict.TryUpdate(omodel.Group, oGroupWiseLimitsModel, MasterSharedMemory.GroupWiseLimitDict[omodel.Group]);
        //                                        //OnGroupwiseLimitReceive?.Invoke(); on response
        //                                        result = true;
        //                                    }
        //                                    else
        //                                    {
        //                                        MessageBox.Show("GroupWise Limit not available");
        //                                        result = false;
        //                                    }
        //                                }
        //                            }
        //                        }
        //                        else if (omodel.BuySellIndicator == Enumerations.Order.BuySellFlag.SELL.ToString())
        //                        {
        //                            double Value = finalPrice * omodel.Quantity;
        //                            Value = Value / Math.Pow(10, decimal_point);

        //                            if (MasterSharedMemory.GroupWiseLimitDict.ContainsKey(omodel.Group))
        //                            {
        //                                if (MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlSell != -1)
        //                                {
        //                                    if (Value <= MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlSell)
        //                                    {
        //                                        MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlSell = MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlSell - Value;
        //                                        // bool update = MasterSharedMemory.GroupWiseLimitDict.TryUpdate(omodel.Group, oGroupWiseLimitsModel, MasterSharedMemory.GroupWiseLimitDict[omodel.Group]);
        //                                        //OnGroupwiseLimitReceive?.Invoke(); on response
        //                                        result = true;
        //                                    }
        //                                    else
        //                                    {
        //                                        MessageBox.Show("GroupWise Limit not available");
        //                                        result = false;
        //                                    }
        //                                }
        //                            }
        //                        }
        //                    }
        //                    else if (finalPrice < 0)
        //                    {
        //                        if (omodel.BuySellIndicator == Enumerations.Order.BuySellFlag.BUY.ToString())
        //                        {
        //                            //omodel.Price = Convert.ToInt64(Convert.ToDouble(omodel.Price) / Math.Pow(10, decimal_point));
        //                            finalPrice = Math.Abs(finalPrice);
        //                            double Value = finalPrice * omodel.Quantity;
        //                            Value = Value / Math.Pow(10, decimal_point);

        //                            if (MasterSharedMemory.GroupWiseLimitDict.ContainsKey(omodel.Group))
        //                            {
        //                                if (MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlBuy != -1)
        //                                {
        //                                    if (Value <= MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlBuy)
        //                                    {
        //                                        MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlBuy = MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlBuy + Value;
        //                                        // bool update = MasterSharedMemory.GroupWiseLimitDict.TryUpdate(omodel.Group, oGroupWiseLimitsModel, MasterSharedMemory.GroupWiseLimitDict[omodel.Group]);
        //                                        //OnGroupwiseLimitReceive?.Invoke(); on response
        //                                        result = true;
        //                                    }
        //                                    else
        //                                    {
        //                                        MessageBox.Show("GroupWise Limit not available");
        //                                        result = false;
        //                                    }
        //                                }
        //                            }
        //                        }
        //                        else if (omodel.BuySellIndicator == Enumerations.Order.BuySellFlag.SELL.ToString())
        //                        {
        //                            finalPrice = Math.Abs(finalPrice);
        //                            double Value = finalPrice * omodel.Quantity;
        //                            Value = Value / Math.Pow(10, decimal_point);

        //                            if (MasterSharedMemory.GroupWiseLimitDict.ContainsKey(omodel.Group))
        //                            {
        //                                if (MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlSell != -1)
        //                                {
        //                                    if (Value <= MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlSell)
        //                                    {
        //                                        MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlSell = MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlSell + Value;
        //                                        // bool update = MasterSharedMemory.GroupWiseLimitDict.TryUpdate(omodel.Group, oGroupWiseLimitsModel, MasterSharedMemory.GroupWiseLimitDict[omodel.Group]);
        //                                        //OnGroupwiseLimitReceive?.Invoke(); on response
        //                                        result = true;
        //                                    }
        //                                    else
        //                                    {
        //                                        MessageBox.Show("GroupWise Limit not available");
        //                                        result = false;
        //                                    }
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //                #endregion

        //                #region Quantity is updated
        //                if (finalPrice == 0)
        //                {
        //                    if (finalqty > 0)
        //                    {
        //                        if (omodel.BuySellIndicator == Enumerations.Order.BuySellFlag.BUY.ToString())
        //                        {
        //                            //omodel.Price = Convert.ToInt64(Convert.ToDouble(omodel.Price) / Math.Pow(10, decimal_point));
        //                            double Value = omodel.Price * finalqty;
        //                            Value = Value / Math.Pow(10, decimal_point);

        //                            if (MasterSharedMemory.GroupWiseLimitDict.ContainsKey(omodel.Group))
        //                            {
        //                                if (MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlBuy != -1)
        //                                {
        //                                    if (Value <= MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlBuy)
        //                                    {
        //                                        MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlBuy = MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlBuy - Value;
        //                                        // bool update = MasterSharedMemory.GroupWiseLimitDict.TryUpdate(omodel.Group, oGroupWiseLimitsModel, MasterSharedMemory.GroupWiseLimitDict[omodel.Group]);
        //                                        //OnGroupwiseLimitReceive?.Invoke(); on response
        //                                        result = true;
        //                                    }
        //                                    else
        //                                    {
        //                                        MessageBox.Show("GroupWise Limit not available");
        //                                        result = false;
        //                                    }
        //                                }
        //                            }
        //                        }
        //                        else if (omodel.BuySellIndicator == Enumerations.Order.BuySellFlag.SELL.ToString())
        //                        {
        //                            double Value = omodel.Price * finalqty;
        //                            Value = Value / Math.Pow(10, decimal_point);

        //                            if (MasterSharedMemory.GroupWiseLimitDict.ContainsKey(omodel.Group))
        //                            {
        //                                if (MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlSell != -1)
        //                                {
        //                                    if (Value <= MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlSell)
        //                                    {
        //                                        MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlSell = MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlSell - Value;
        //                                        // bool update = MasterSharedMemory.GroupWiseLimitDict.TryUpdate(omodel.Group, oGroupWiseLimitsModel, MasterSharedMemory.GroupWiseLimitDict[omodel.Group]);
        //                                        //OnGroupwiseLimitReceive?.Invoke(); on response
        //                                        result = true;
        //                                    }
        //                                    else
        //                                    {
        //                                        MessageBox.Show("GroupWise Limit not available");
        //                                        result = false;
        //                                    }
        //                                }
        //                            }
        //                        }
        //                    }
        //                    else if (finalqty < 0)
        //                    {
        //                        if (omodel.BuySellIndicator == Enumerations.Order.BuySellFlag.BUY.ToString())
        //                        {
        //                            //omodel.Price = Convert.ToInt64(Convert.ToDouble(omodel.Price) / Math.Pow(10, decimal_point));
        //                            finalqty = Math.Abs(finalqty);
        //                            double Value = omodel.Price * finalqty;
        //                            Value = Value / Math.Pow(10, decimal_point);

        //                            if (MasterSharedMemory.GroupWiseLimitDict.ContainsKey(omodel.Group))
        //                            {
        //                                if (MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlBuy != -1)
        //                                {
        //                                    if (Value <= MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlBuy)
        //                                    {
        //                                        MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlBuy = MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlBuy + Value;
        //                                        // bool update = MasterSharedMemory.GroupWiseLimitDict.TryUpdate(omodel.Group, oGroupWiseLimitsModel, MasterSharedMemory.GroupWiseLimitDict[omodel.Group]);
        //                                        //OnGroupwiseLimitReceive?.Invoke(); on response
        //                                        result = true;
        //                                    }
        //                                    else
        //                                    {
        //                                        MessageBox.Show("GroupWise Limit not available");
        //                                        result = false;
        //                                    }
        //                                }
        //                            }
        //                        }
        //                        else if (omodel.BuySellIndicator == Enumerations.Order.BuySellFlag.SELL.ToString())
        //                        {
        //                            finalqty = Math.Abs(finalqty);
        //                            double Value = omodel.Price * finalqty;
        //                            Value = Value / Math.Pow(10, decimal_point);

        //                            if (MasterSharedMemory.GroupWiseLimitDict.ContainsKey(omodel.Group))
        //                            {
        //                                if (MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlSell != -1)
        //                                {
        //                                    if (Value <= MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlSell)
        //                                    {
        //                                        MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlSell = MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlSell + Value;
        //                                        // bool update = MasterSharedMemory.GroupWiseLimitDict.TryUpdate(omodel.Group, oGroupWiseLimitsModel, MasterSharedMemory.GroupWiseLimitDict[omodel.Group]);
        //                                        //OnGroupwiseLimitReceive?.Invoke(); on response
        //                                        result = true;
        //                                    }
        //                                    else
        //                                    {
        //                                        MessageBox.Show("GroupWise Limit not available");
        //                                        result = false;
        //                                    }
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //                #endregion


        //                else if (finalPrice > 0 && finalqty > 0)
        //                {
        //                    if (omodel.BuySellIndicator == Enumerations.Order.BuySellFlag.BUY.ToString())
        //                    {
        //                        //omodel.Price = Convert.ToInt64(Convert.ToDouble(omodel.Price) / Math.Pow(10, decimal_point));
        //                        double Value = finalPrice * finalqty;
        //                        Value = Value / Math.Pow(10, decimal_point);

        //                        if (MasterSharedMemory.GroupWiseLimitDict.ContainsKey(omodel.Group))
        //                        {
        //                            if (MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlBuy != -1)
        //                            {
        //                                if (Value <= MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlBuy)
        //                                {
        //                                    MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlBuy = MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlBuy - Value;
        //                                    // bool update = MasterSharedMemory.GroupWiseLimitDict.TryUpdate(omodel.Group, oGroupWiseLimitsModel, MasterSharedMemory.GroupWiseLimitDict[omodel.Group]);
        //                                    //OnGroupwiseLimitReceive?.Invoke(); on response
        //                                    result = true;
        //                                }
        //                                else
        //                                {
        //                                    MessageBox.Show("GroupWise Limit not available");
        //                                    result = false;
        //                                }
        //                            }
        //                        }
        //                    }
        //                    else if (omodel.BuySellIndicator == Enumerations.Order.BuySellFlag.SELL.ToString())
        //                    {
        //                        double Value = finalPrice * finalqty;
        //                        Value = Value / Math.Pow(10, decimal_point);

        //                        if (MasterSharedMemory.GroupWiseLimitDict.ContainsKey(omodel.Group))
        //                        {
        //                            if (MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlSell != -1)
        //                            {
        //                                if (Value <= MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlSell)
        //                                {
        //                                    MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlSell = MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlSell - Value;
        //                                    // bool update = MasterSharedMemory.GroupWiseLimitDict.TryUpdate(omodel.Group, oGroupWiseLimitsModel, MasterSharedMemory.GroupWiseLimitDict[omodel.Group]);
        //                                    //OnGroupwiseLimitReceive?.Invoke(); on response
        //                                    result = true;
        //                                }
        //                                else
        //                                {
        //                                    MessageBox.Show("GroupWise Limit not available");
        //                                    result = false;
        //                                }
        //                            }
        //                        }
        //                    }
        //                }

        //                else if (finalPrice < 0 && finalqty < 0)
        //                {
        //                    if (omodel.BuySellIndicator == Enumerations.Order.BuySellFlag.BUY.ToString())
        //                    {
        //                        //omodel.Price = Convert.ToInt64(Convert.ToDouble(omodel.Price) / Math.Pow(10, decimal_point));
        //                        finalPrice = Math.Abs(finalPrice);
        //                        finalqty = Math.Abs(finalqty);
        //                        double Value = finalPrice * finalqty;
        //                        Value = Value / Math.Pow(10, decimal_point);

        //                        if (MasterSharedMemory.GroupWiseLimitDict.ContainsKey(omodel.Group))
        //                        {
        //                            if (MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlBuy != -1)
        //                            {
        //                                if (Value <= MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlBuy)
        //                                {
        //                                    MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlBuy = MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlBuy + Value;
        //                                    // bool update = MasterSharedMemory.GroupWiseLimitDict.TryUpdate(omodel.Group, oGroupWiseLimitsModel, MasterSharedMemory.GroupWiseLimitDict[omodel.Group]);
        //                                    //OnGroupwiseLimitReceive?.Invoke(); on response
        //                                    result = true;
        //                                }
        //                                else
        //                                {
        //                                    MessageBox.Show("GroupWise Limit not available");
        //                                    result = false;
        //                                }
        //                            }
        //                        }
        //                    }
        //                    else if (omodel.BuySellIndicator == Enumerations.Order.BuySellFlag.SELL.ToString())
        //                    {
        //                        finalPrice = Math.Abs(finalPrice);
        //                        finalqty = Math.Abs(finalqty);
        //                        double Value = finalPrice * finalqty;
        //                        Value = Value / Math.Pow(10, decimal_point);

        //                        if (MasterSharedMemory.GroupWiseLimitDict.ContainsKey(omodel.Group))
        //                        {
        //                            if (MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlSell != -1)
        //                            {
        //                                if (Value <= MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlSell)
        //                                {
        //                                    MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlSell = MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlSell + Value;
        //                                    // bool update = MasterSharedMemory.GroupWiseLimitDict.TryUpdate(omodel.Group, oGroupWiseLimitsModel, MasterSharedMemory.GroupWiseLimitDict[omodel.Group]);
        //                                    //OnGroupwiseLimitReceive?.Invoke(); on response
        //                                    result = true;
        //                                }
        //                                else
        //                                {
        //                                    MessageBox.Show("GroupWise Limit not available");
        //                                    result = false;
        //                                }
        //                            }
        //                        }
        //                    }
        //                }

        //            }

        //            #endregion

        //            #region Deleting Order Request

        //            if (omodel.OrderAction == Enumerations.Order.Modes.D.ToString())
        //            {
        //                if (omodel.BuySellIndicator == Enumerations.Order.BuySellFlag.BUY.ToString())
        //                {
        //                    if (MasterSharedMemory.GroupWiseLimitDict.ContainsKey(omodel.Group))
        //                    {
        //                        double Value = omodel.Price * omodel.Quantity;
        //                        Value = Value / Math.Pow(10, decimal_point);

        //                        if (MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlBuy != -1)
        //                        {
        //                            if (Value <= MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlBuy)
        //                            {
        //                                MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlBuy = MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlBuy + Value;
        //                                // bool update = MasterSharedMemory.GroupWiseLimitDict.TryUpdate(omodel.Group, oGroupWiseLimitsModel, MasterSharedMemory.GroupWiseLimitDict[omodel.Group]);
        //                                //OnGroupwiseLimitReceive?.Invoke(); on response
        //                                result = true;
        //                            }
        //                            else
        //                            {
        //                                MessageBox.Show("GroupWise Limit not available");
        //                                result = false;
        //                            }
        //                        }
        //                    }
        //                }
        //                else if (omodel.BuySellIndicator == Enumerations.Order.BuySellFlag.SELL.ToString())
        //                {
        //                    if (MasterSharedMemory.GroupWiseLimitDict.ContainsKey(omodel.Group))
        //                    {
        //                        double Value = omodel.Price * omodel.Quantity;
        //                        Value = Value / Math.Pow(10, decimal_point);

        //                        if (MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlSell != -1)
        //                        {
        //                            if (Value <= MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlSell)
        //                            {
        //                                MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlSell = MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlSell + Value;
        //                                //bool update = MasterSharedMemory.GroupWiseLimitDict.TryUpdate(omodel.Group, oGroupWiseLimitsModel, MasterSharedMemory.GroupWiseLimitDict[omodel.Group]);
        //                                //OnGroupwiseLimitReceive?.Invoke();
        //                                result = true;
        //                            }
        //                            else
        //                            {
        //                                MessageBox.Show("GroupWise Limit not available");
        //                                result = false;
        //                            }
        //                        }
        //                    }
        //                }
        //            }

        //            #endregion

        //        }


        //        else
        //        {
        //            result = false;
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionUtility.LogError(ex);
        //        MessageBox.Show("Error In Validation of GroupWise Limit");
        //        result = false;
        //    }

        //    return result;
        //}


        //public static bool InvokeValidateGroupWiseLimitOnErrorReceived(OrderModel omodel, int decimal_point)
        //{
        //    bool result = false;
        //    GroupWiseLimitsModel oGroupWiseLimitsModel = new GroupWiseLimitsModel();
        //    try
        //    {
        //        if (omodel != null)
        //        {
        //            #region When error in adding order request

        //            if (omodel.OrderAction == Enumerations.Order.Modes.A.ToString())
        //            {
        //                if (omodel.BuySellIndicator == Enumerations.Order.BuySellFlag.BUY.ToString())
        //                {
        //                    if (MasterSharedMemory.GroupWiseLimitDict.ContainsKey(omodel.Group))
        //                    {
        //                        double Value = omodel.Price * omodel.Quantity;
        //                        Value = Value / Math.Pow(10, decimal_point);

        //                        if (MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlBuy != -1)
        //                        {
        //                            MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlBuy = MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlBuy + Value;
        //                            // bool update = MasterSharedMemory.GroupWiseLimitDict.TryUpdate(omodel.Group, oGroupWiseLimitsModel, MasterSharedMemory.GroupWiseLimitDict[omodel.Group]);
        //                            //OnGroupwiseLimitReceive?.Invoke(); on response
        //                            result = true;
        //                        }

        //                    }
        //                }
        //                else if (omodel.BuySellIndicator == Enumerations.Order.BuySellFlag.SELL.ToString())
        //                {
        //                    if (MasterSharedMemory.GroupWiseLimitDict.ContainsKey(omodel.Group))
        //                    {
        //                        double Value = omodel.Price * omodel.Quantity;
        //                        Value = Value / Math.Pow(10, decimal_point);

        //                        if (MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlSell != -1)
        //                        {
        //                            MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlSell = MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlSell + Value;
        //                            //bool update = MasterSharedMemory.GroupWiseLimitDict.TryUpdate(omodel.Group, oGroupWiseLimitsModel, MasterSharedMemory.GroupWiseLimitDict[omodel.Group]);
        //                            //OnGroupwiseLimitReceive?.Invoke();
        //                            result = true;
        //                        }
        //                    }
        //                }
        //            }
        //            #endregion

        //            #region Updating Order Request
        //            if (omodel.OrderAction == Enumerations.Order.Modes.U.ToString())
        //            {
        //                var price1 = MemoryManager.OrderDictionary[omodel.MessageTag].Price; //Main Memory
        //                var price2 = MemoryManager.OrderDictionaryBackupMemory[omodel.MessageTag].Price;//buffer
        //                var finalPrice = price2 - price1;

        //                var qty1 = MemoryManager.OrderDictionary[omodel.MessageTag].Quantity;
        //                var qty2 = MemoryManager.OrderDictionaryBackupMemory[omodel.MessageTag].Quantity;
        //                var finalqty = qty2 - qty1;

        //                #region Price is updated
        //                if (finalqty == 0)
        //                {
        //                    if (finalPrice > 0)
        //                    {
        //                        if (omodel.BuySellIndicator == Enumerations.Order.BuySellFlag.BUY.ToString())
        //                        {
        //                            //omodel.Price = Convert.ToInt64(Convert.ToDouble(omodel.Price) / Math.Pow(10, decimal_point));
        //                            double Value = finalPrice * omodel.Quantity;
        //                            Value = Value / Math.Pow(10, decimal_point);

        //                            if (MasterSharedMemory.GroupWiseLimitDict.ContainsKey(omodel.Group))
        //                            {
        //                                if (MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlBuy != -1)
        //                                {
        //                                    if (Value <= MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlBuy)
        //                                    {
        //                                        MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlBuy = MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlBuy + Value;
        //                                        // bool update = MasterSharedMemory.GroupWiseLimitDict.TryUpdate(omodel.Group, oGroupWiseLimitsModel, MasterSharedMemory.GroupWiseLimitDict[omodel.Group]);
        //                                        //OnGroupwiseLimitReceive?.Invoke(); on response
        //                                        result = true;
        //                                    }
        //                                    else
        //                                    {
        //                                        MessageBox.Show("GroupWise Limit not available");
        //                                        result = false;
        //                                    }
        //                                }
        //                            }
        //                        }
        //                        else if (omodel.BuySellIndicator == Enumerations.Order.BuySellFlag.SELL.ToString())
        //                        {
        //                            double Value = finalPrice * omodel.Quantity;
        //                            Value = Value / Math.Pow(10, decimal_point);

        //                            if (MasterSharedMemory.GroupWiseLimitDict.ContainsKey(omodel.Group))
        //                            {
        //                                if (MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlSell != -1)
        //                                {
        //                                    if (Value <= MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlSell)
        //                                    {
        //                                        MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlSell = MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlSell + Value;
        //                                        // bool update = MasterSharedMemory.GroupWiseLimitDict.TryUpdate(omodel.Group, oGroupWiseLimitsModel, MasterSharedMemory.GroupWiseLimitDict[omodel.Group]);
        //                                        //OnGroupwiseLimitReceive?.Invoke(); on response
        //                                        result = true;
        //                                    }
        //                                    else
        //                                    {
        //                                        MessageBox.Show("GroupWise Limit not available");
        //                                        result = false;
        //                                    }
        //                                }
        //                            }
        //                        }
        //                    }
        //                    else if (finalPrice < 0)
        //                    {
        //                        if (omodel.BuySellIndicator == Enumerations.Order.BuySellFlag.BUY.ToString())
        //                        {
        //                            //omodel.Price = Convert.ToInt64(Convert.ToDouble(omodel.Price) / Math.Pow(10, decimal_point));
        //                            finalPrice = Math.Abs(finalPrice);
        //                            double Value = finalPrice * omodel.Quantity;
        //                            Value = Value / Math.Pow(10, decimal_point);

        //                            if (MasterSharedMemory.GroupWiseLimitDict.ContainsKey(omodel.Group))
        //                            {
        //                                if (MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlBuy != -1)
        //                                {
        //                                    if (Value <= MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlBuy)
        //                                    {
        //                                        MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlBuy = MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlBuy - Value;
        //                                        // bool update = MasterSharedMemory.GroupWiseLimitDict.TryUpdate(omodel.Group, oGroupWiseLimitsModel, MasterSharedMemory.GroupWiseLimitDict[omodel.Group]);
        //                                        //OnGroupwiseLimitReceive?.Invoke(); on response
        //                                        result = true;
        //                                    }
        //                                    else
        //                                    {
        //                                        MessageBox.Show("GroupWise Limit not available");
        //                                        result = false;
        //                                    }
        //                                }
        //                            }
        //                        }
        //                        else if (omodel.BuySellIndicator == Enumerations.Order.BuySellFlag.SELL.ToString())
        //                        {
        //                            finalPrice = Math.Abs(finalPrice);
        //                            double Value = finalPrice * omodel.Quantity;
        //                            Value = Value / Math.Pow(10, decimal_point);

        //                            if (MasterSharedMemory.GroupWiseLimitDict.ContainsKey(omodel.Group))
        //                            {
        //                                if (MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlSell != -1)
        //                                {
        //                                    if (Value <= MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlSell)
        //                                    {
        //                                        MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlSell = MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlSell - Value;
        //                                        // bool update = MasterSharedMemory.GroupWiseLimitDict.TryUpdate(omodel.Group, oGroupWiseLimitsModel, MasterSharedMemory.GroupWiseLimitDict[omodel.Group]);
        //                                        //OnGroupwiseLimitReceive?.Invoke(); on response
        //                                        result = true;
        //                                    }
        //                                    else
        //                                    {
        //                                        MessageBox.Show("GroupWise Limit not available");
        //                                        result = false;
        //                                    }
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //                #endregion

        //                #region Quantity is updated
        //                if (finalPrice == 0)
        //                {
        //                    if (finalqty > 0)
        //                    {
        //                        if (omodel.BuySellIndicator == Enumerations.Order.BuySellFlag.BUY.ToString())
        //                        {
        //                            //omodel.Price = Convert.ToInt64(Convert.ToDouble(omodel.Price) / Math.Pow(10, decimal_point));
        //                            double Value = omodel.Price * finalqty;
        //                            Value = Value / Math.Pow(10, decimal_point);

        //                            if (MasterSharedMemory.GroupWiseLimitDict.ContainsKey(omodel.Group))
        //                            {
        //                                if (MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlBuy != -1)
        //                                {
        //                                    if (Value <= MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlBuy)
        //                                    {
        //                                        MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlBuy = MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlBuy + Value;
        //                                        // bool update = MasterSharedMemory.GroupWiseLimitDict.TryUpdate(omodel.Group, oGroupWiseLimitsModel, MasterSharedMemory.GroupWiseLimitDict[omodel.Group]);
        //                                        //OnGroupwiseLimitReceive?.Invoke(); on response
        //                                        result = true;
        //                                    }
        //                                    else
        //                                    {
        //                                        MessageBox.Show("GroupWise Limit not available");
        //                                        result = false;
        //                                    }
        //                                }
        //                            }
        //                        }
        //                        else if (omodel.BuySellIndicator == Enumerations.Order.BuySellFlag.SELL.ToString())
        //                        {
        //                            double Value = omodel.Price * finalqty;
        //                            Value = Value / Math.Pow(10, decimal_point);

        //                            if (MasterSharedMemory.GroupWiseLimitDict.ContainsKey(omodel.Group))
        //                            {
        //                                if (MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlSell != -1)
        //                                {
        //                                    if (Value <= MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlSell)
        //                                    {
        //                                        MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlSell = MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlSell + Value;
        //                                        // bool update = MasterSharedMemory.GroupWiseLimitDict.TryUpdate(omodel.Group, oGroupWiseLimitsModel, MasterSharedMemory.GroupWiseLimitDict[omodel.Group]);
        //                                        //OnGroupwiseLimitReceive?.Invoke(); on response
        //                                        result = true;
        //                                    }
        //                                    else
        //                                    {
        //                                        MessageBox.Show("GroupWise Limit not available");
        //                                        result = false;
        //                                    }
        //                                }
        //                            }
        //                        }
        //                    }
        //                    else if (finalqty < 0)
        //                    {
        //                        if (omodel.BuySellIndicator == Enumerations.Order.BuySellFlag.BUY.ToString())
        //                        {
        //                            //omodel.Price = Convert.ToInt64(Convert.ToDouble(omodel.Price) / Math.Pow(10, decimal_point));
        //                            finalqty = Math.Abs(finalqty);
        //                            double Value = omodel.Price * finalqty;
        //                            Value = Value / Math.Pow(10, decimal_point);

        //                            if (MasterSharedMemory.GroupWiseLimitDict.ContainsKey(omodel.Group))
        //                            {
        //                                if (MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlBuy != -1)
        //                                {
        //                                    if (Value <= MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlBuy)
        //                                    {
        //                                        MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlBuy = MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlBuy - Value;
        //                                        // bool update = MasterSharedMemory.GroupWiseLimitDict.TryUpdate(omodel.Group, oGroupWiseLimitsModel, MasterSharedMemory.GroupWiseLimitDict[omodel.Group]);
        //                                        //OnGroupwiseLimitReceive?.Invoke(); on response
        //                                        result = true;
        //                                    }
        //                                    else
        //                                    {
        //                                        MessageBox.Show("GroupWise Limit not available");
        //                                        result = false;
        //                                    }
        //                                }
        //                            }
        //                        }
        //                        else if (omodel.BuySellIndicator == Enumerations.Order.BuySellFlag.SELL.ToString())
        //                        {
        //                            finalqty = Math.Abs(finalqty);
        //                            double Value = omodel.Price * finalqty;
        //                            Value = Value / Math.Pow(10, decimal_point);

        //                            if (MasterSharedMemory.GroupWiseLimitDict.ContainsKey(omodel.Group))
        //                            {
        //                                if (MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlSell != -1)
        //                                {
        //                                    if (Value <= MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlSell)
        //                                    {
        //                                        MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlSell = MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlSell - Value;
        //                                        // bool update = MasterSharedMemory.GroupWiseLimitDict.TryUpdate(omodel.Group, oGroupWiseLimitsModel, MasterSharedMemory.GroupWiseLimitDict[omodel.Group]);
        //                                        //OnGroupwiseLimitReceive?.Invoke(); on response
        //                                        result = true;
        //                                    }
        //                                    else
        //                                    {
        //                                        MessageBox.Show("GroupWise Limit not available");
        //                                        result = false;
        //                                    }
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //                #endregion


        //                else if (finalPrice > 0 && finalqty > 0)
        //                {
        //                    if (omodel.BuySellIndicator == Enumerations.Order.BuySellFlag.BUY.ToString())
        //                    {
        //                        //omodel.Price = Convert.ToInt64(Convert.ToDouble(omodel.Price) / Math.Pow(10, decimal_point));
        //                        double Value = finalPrice * finalqty;
        //                        Value = Value / Math.Pow(10, decimal_point);

        //                        if (MasterSharedMemory.GroupWiseLimitDict.ContainsKey(omodel.Group))
        //                        {
        //                            if (MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlBuy != -1)
        //                            {
        //                                if (Value <= MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlBuy)
        //                                {
        //                                    MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlBuy = MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlBuy + Value;
        //                                    // bool update = MasterSharedMemory.GroupWiseLimitDict.TryUpdate(omodel.Group, oGroupWiseLimitsModel, MasterSharedMemory.GroupWiseLimitDict[omodel.Group]);
        //                                    //OnGroupwiseLimitReceive?.Invoke(); on response
        //                                    result = true;
        //                                }
        //                                else
        //                                {
        //                                    MessageBox.Show("GroupWise Limit not available");
        //                                    result = false;
        //                                }
        //                            }
        //                        }
        //                    }
        //                    else if (omodel.BuySellIndicator == Enumerations.Order.BuySellFlag.SELL.ToString())
        //                    {
        //                        double Value = finalPrice * finalqty;
        //                        Value = Value / Math.Pow(10, decimal_point);

        //                        if (MasterSharedMemory.GroupWiseLimitDict.ContainsKey(omodel.Group))
        //                        {
        //                            if (MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlSell != -1)
        //                            {
        //                                if (Value <= MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlSell)
        //                                {
        //                                    MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlSell = MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlSell + Value;
        //                                    // bool update = MasterSharedMemory.GroupWiseLimitDict.TryUpdate(omodel.Group, oGroupWiseLimitsModel, MasterSharedMemory.GroupWiseLimitDict[omodel.Group]);
        //                                    //OnGroupwiseLimitReceive?.Invoke(); on response
        //                                    result = true;
        //                                }
        //                                else
        //                                {
        //                                    MessageBox.Show("GroupWise Limit not available");
        //                                    result = false;
        //                                }
        //                            }
        //                        }
        //                    }
        //                }

        //                else if (finalPrice < 0 && finalqty < 0)
        //                {
        //                    if (omodel.BuySellIndicator == Enumerations.Order.BuySellFlag.BUY.ToString())
        //                    {
        //                        //omodel.Price = Convert.ToInt64(Convert.ToDouble(omodel.Price) / Math.Pow(10, decimal_point));
        //                        finalPrice = Math.Abs(finalPrice);
        //                        finalqty = Math.Abs(finalqty);
        //                        double Value = finalPrice * finalqty;
        //                        Value = Value / Math.Pow(10, decimal_point);

        //                        if (MasterSharedMemory.GroupWiseLimitDict.ContainsKey(omodel.Group))
        //                        {
        //                            if (MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlBuy != -1)
        //                            {
        //                                if (Value <= MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlBuy)
        //                                {
        //                                    MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlBuy = MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlBuy - Value;
        //                                    // bool update = MasterSharedMemory.GroupWiseLimitDict.TryUpdate(omodel.Group, oGroupWiseLimitsModel, MasterSharedMemory.GroupWiseLimitDict[omodel.Group]);
        //                                    //OnGroupwiseLimitReceive?.Invoke(); on response
        //                                    result = true;
        //                                }
        //                                else
        //                                {
        //                                    MessageBox.Show("GroupWise Limit not available");
        //                                    result = false;
        //                                }
        //                            }
        //                        }
        //                    }
        //                    else if (omodel.BuySellIndicator == Enumerations.Order.BuySellFlag.SELL.ToString())
        //                    {
        //                        finalPrice = Math.Abs(finalPrice);
        //                        finalqty = Math.Abs(finalqty);
        //                        double Value = finalPrice * finalqty;
        //                        Value = Value / Math.Pow(10, decimal_point);

        //                        if (MasterSharedMemory.GroupWiseLimitDict.ContainsKey(omodel.Group))
        //                        {
        //                            if (MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlSell != -1)
        //                            {
        //                                if (Value <= MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlSell)
        //                                {
        //                                    MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlSell = MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlSell - Value;
        //                                    // bool update = MasterSharedMemory.GroupWiseLimitDict.TryUpdate(omodel.Group, oGroupWiseLimitsModel, MasterSharedMemory.GroupWiseLimitDict[omodel.Group]);
        //                                    //OnGroupwiseLimitReceive?.Invoke(); on response
        //                                    result = true;
        //                                }
        //                                else
        //                                {
        //                                    MessageBox.Show("GroupWise Limit not available");
        //                                    result = false;
        //                                }
        //                            }
        //                        }
        //                    }
        //                }

        //            }

        //            #endregion

        //            #region When error in Deleting Order Request

        //            if (omodel.OrderAction == Enumerations.Order.Modes.D.ToString())
        //            {
        //                if (omodel.BuySellIndicator == Enumerations.Order.BuySellFlag.BUY.ToString())
        //                {
        //                    if (MasterSharedMemory.GroupWiseLimitDict.ContainsKey(omodel.Group))
        //                    {
        //                        double Value = omodel.Price * omodel.Quantity;
        //                        Value = Value / Math.Pow(10, decimal_point);

        //                        if (MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlBuy != -1)
        //                        {
        //                            MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlBuy = MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlBuy - Value;
        //                            // bool update = MasterSharedMemory.GroupWiseLimitDict.TryUpdate(omodel.Group, oGroupWiseLimitsModel, MasterSharedMemory.GroupWiseLimitDict[omodel.Group]);
        //                            //OnGroupwiseLimitReceive?.Invoke(); on response
        //                            result = true;
        //                        }

        //                    }
        //                }
        //                else if (omodel.BuySellIndicator == Enumerations.Order.BuySellFlag.SELL.ToString())
        //                {
        //                    if (MasterSharedMemory.GroupWiseLimitDict.ContainsKey(omodel.Group))
        //                    {
        //                        double Value = omodel.Price * omodel.Quantity;
        //                        Value = Value / Math.Pow(10, decimal_point);

        //                        if (MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlSell != -1)
        //                        {
        //                            MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlSell = MasterSharedMemory.GroupWiseLimitDict[omodel.Group].AvlSell - Value;
        //                            //bool update = MasterSharedMemory.GroupWiseLimitDict.TryUpdate(omodel.Group, oGroupWiseLimitsModel, MasterSharedMemory.GroupWiseLimitDict[omodel.Group]);
        //                            //OnGroupwiseLimitReceive?.Invoke();
        //                            result = true;
        //                        }
        //                    }
        //                }
        //            }
        //            #endregion
        //        }
        //        else
        //        {
        //            result = false;
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionUtility.LogError(ex);
        //        MessageBox.Show("Error In Validation of GroupWise Limit");
        //        result = false;
        //    }

        //    return result;
        //}

#endif
    }
}
