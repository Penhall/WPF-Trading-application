using CommonFrontEnd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonFrontEnd.Utility.Interfaces_Abstract
{
    //public abstract class ContractBase : IScript
    //{


    //    public long Id { get; set; }
    //    public long Token { get; set; }
    //    public string Symbol { get; set; }
    //    public string Name { get; set; }
    //    public long SequenceId { get; set; }
    //    public string Group { get; set; }

    //    public int ExchangeId { get; set; }
    //    public int MarketId { get; set; }

    //    public long TickSize { get; set; }
    //    public long BoardLotQuantity { get; set; }

    //    public string Field3 { get; set; }
    //    //added by KiranS 13-FEB-17

    //    //: This is the Common Id of the Security in the database.
    //    public string InstrumentName { get; set; }
    //    public long ExpiryDate { get; set; }
    //    private string mstrDisplayExpiryDate;
    //    public string DisplayExpiryDate
    //    {
    //        get { return mstrDisplayExpiryDate; }
    //        set { mstrDisplayExpiryDate = GetFormatedExpiryDate(value); }
    //    }
    //    public double StrikePrice { get; set; }
    //    public abstract double DisplayStrikePrice { get; }
    //    public string OptionType { get; set; }

    //    public string PQFactor { get; set; }
    //    public string TradingUnit { get; set; }

    //    public long Numerator { get; set; }
    //    public long Denominator { get; set; }
    //    public long QuotationUnit { get; set; }
    //    //Vijayalakshmi - 29Sep2016 - MCX contracts changes as suggested by Anjul
    //    public string TradingUnitDisplay { get; set; }
    //    //Vijayalakshmi - 07Oct2016 - MCX changes suggested by Avinash
    //    public string QuotationMetric { get; set; }
    //    //Vijayalakshmi - 07Oct2016 - MCX changes suggested by Avinash
    //    public abstract bool IsSpreadContract { get; }

    //    public string ExerciseStartDate { get; set; }
    //    public string ExerciseEndDate { get; set; }


    //    public bool IsDefaultOrdSettings { get; set; }
    //    public string defaultDQ { get; set; }
    //    public string defaultPrice { get; set; }
    //    public string defaultProtPer { get; set; }
    //    public string defaultQTY { get; set; }

    //    public object Clone()
    //    {
    //        return this.MemberwiseClone();
    //    }

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

    //    public static double GetDisplayStrikePrice(string MarketId, double pdblStrikePrice)
    //    {
    //        if (pdblStrikePrice > 0)
    //        {
    //            double lintStrikePrice = 0;
    //            if (MarketId == Constants.MKT_CURRENCY_VALUE)
    //            {
    //                lintStrikePrice = pdblStrikePrice / Constants.Currency_Divisor;
    //            }
    //            else
    //            {
    //                lintStrikePrice = pdblStrikePrice / 100;
    //            }
    //            return lintStrikePrice;
    //        }
    //        return pdblStrikePrice;
    //    }

    //}

}
