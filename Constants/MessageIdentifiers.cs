using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonFrontEnd.Constants
{
    class MessageIdentifiers
    {

        // Broadcast Messages
        public static string MESSAGE_MARKET_STATUS_CHANGE_TEXT = "71";
        public static string MESSAGE_Concise_Index = "69";
        public static string MESSAGE_INDEX_TEXT = "72";
        public static string MESSAGE_TIME_TEXT = "74";
        public static string MESSAGE_MARKET_BY_PRICE_TEXT = "60";
        public static string MESSAGE_MARKET_WATCH_TEXT = "61";
        public static string MESSAGE_OPEN_NETPOSITION = "62";
        public static string MESSAGE_MARKER_ALERT = "64";
        public static string MESSAGE_OPEN_INTEREST_TEXT = "70";

        //Order and Trade messages
        public static string MESSAGE_ORDER_STREAM = "75";
        public static string MESSAGE_TRADE_STREAM = "76";
        public static string MESSAGE_MARGIN_POSITION = "82";
    }
}
