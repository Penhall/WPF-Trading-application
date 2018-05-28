using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonFrontEnd.Utility.Factory
{
    internal class ExchangeColumnName_Map
    {
        internal readonly string Key;
        internal readonly int ExchangeID;
        internal readonly int MarketID;
        internal readonly string TableName;
        internal readonly string ClassType;

        internal Dictionary<string, string> ColumnName_Map = new Dictionary<string, string>();
        internal ExchangeColumnName_Map(int pintExchangeID, int pintMarketID, string pstrClassType, string pstrTableName)
        {
            ExchangeID = pintExchangeID;
            MarketID = pintMarketID;
            Key = ExchangeID.ToString() + "^" + MarketID.ToString();
            ClassType = pstrClassType;
            TableName = pstrTableName;
        }
        static internal string GenerateKey(int pstrExchangeID, int pstrMarketID)
        {
            return pstrExchangeID + "^" + pstrMarketID;
        }
    }
}
