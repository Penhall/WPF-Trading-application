using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonFrontEnd.Constants
{
  public class MarketWatchConstants
    {
        public static string URL_LIST_ARBITRAGE_MARKET_WATCH = "/ListArbitrageMarketWatch";

        public static bool gblnConnectOnHTTP = false;

        public static string MODE_STRING = "MODE";
        public static string PROXY_STRING = "PROXY";

        public const string MESSAGE_STRING = "MESSAGE";
        public const string MESSAGECOUNT_STRING = "MESSAGECOUNT";
        //Broadcast Servlet url
        public static string URL_BROADCAST_SERVLET = "/BroadcastServlet";
        internal static ConcurrentQueue<SocketConnection.MessageArrivedEventArgs> mobjSync_BroadcastMsgQueue = new ConcurrentQueue<SocketConnection.MessageArrivedEventArgs>();
        public static List<Model.ScripDetails> objScripDetails = new List<Model.ScripDetails>();
        public   static int SelectedID=0;

        //BestFive Constants

        public const int RECORD_POSITION_UPDATE = 2;
        public const int RECORD_POSITION_SERVLETCALL = 5;
        public const int DISPLACEMENT_UPDATE = 6;
        public const int DISPLACEMENT_SERVLETCALL = 0;
        public static string lstrTokenString;
    }
}
