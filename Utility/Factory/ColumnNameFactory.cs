using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonFrontEnd.Utility.Factory
{
    internal class ColumnNameFactory
    {

        /// <summary>
        /// Returns Exchange Column Map for the given Exchange and Market
        /// </summary>
        /// <param name="pintExchangeID">Exchange ID</param>
        /// <param name="pintMarketID">Market ID</param>
        /// <returns>An instance of ExchangeColumnName_Map</returns>
        /// <remarks></remarks>
        static internal ExchangeColumnName_Map GetExchangeColumnMap(int  pintExchangeID, int pintMarketID)
        {
            return GetExchangeColumnMap(ExchangeColumnName_Map.GenerateKey(pintExchangeID, pintMarketID));
        }
        /// <summary>
        /// Returns Exchange Column Map for the given Key
        /// </summary>
        /// <param name="pstrKey">ExchangeID^MarketID</param>
        /// <returns>ExchangeColumnName_Map</returns>
        /// <remarks>Will return an Unknown Object as per NullObject Pattern if no matching entry is found </remarks>
        static internal ExchangeColumnName_Map GetExchangeColumnMap(string pstrKey)
        {
            if (RegisteredExchangeColumnMap.ExchangeColumnMapColl.ContainsKey(pstrKey))
            {
                return RegisteredExchangeColumnMap.ExchangeColumnMapColl.Values.Where(x => x.Key == pstrKey).FirstOrDefault();
            }
            else
            {
                ExchangeColumnName_Map lobjUnknown = new ExchangeColumnName_Map(-1, -1, "", "");
                //: NullObjectPattern
                lobjUnknown.ColumnName_Map.Add("ID".ToUpper(), "");
                lobjUnknown.ColumnName_Map.Add("Token".ToUpper(), "");
                lobjUnknown.ColumnName_Map.Add("InstrumentName".ToUpper(), "");
                lobjUnknown.ColumnName_Map.Add("Symbol".ToUpper(), "");
                lobjUnknown.ColumnName_Map.Add("Series".ToUpper(), "");
                lobjUnknown.ColumnName_Map.Add("ExpiryDate".ToUpper(), "");
                lobjUnknown.ColumnName_Map.Add("StrikePrice".ToUpper(), "");
                lobjUnknown.ColumnName_Map.Add("OptionType".ToUpper(), "");
                lobjUnknown.ColumnName_Map.Add("DisplayExpiryDate".ToUpper(), "");
                lobjUnknown.ColumnName_Map.Add("BoardLotQuantity".ToUpper(), "");
                lobjUnknown.ColumnName_Map.Add("TickSize".ToUpper(), "");
                lobjUnknown.ColumnName_Map.Add("Name".ToUpper(), "");
                lobjUnknown.ColumnName_Map.Add("SequenceId".ToUpper(), "");
                return lobjUnknown;
            }
        }

        /// <summary>
        /// Will return and Enumerator of all the keys in the Exchange Column Map
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        static internal IEnumerator<string> GetExchangeColumnMapKeysEnum()
        {
            return RegisteredExchangeColumnMap.ExchangeColumnMapColl.Keys.GetEnumerator();
        }


    }
}
