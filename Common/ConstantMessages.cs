using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonFrontEnd.Common
{
   public class ConstantMessages
    {
        //Order Type messages 
        public static readonly string ORDER_TYPE_LIMIT = "LIMIT";
        public static readonly string ORDER_TYPE_PF_CONV = "MARKET";
        public static readonly string ORDER_TYPE_PF_KILL = "IOC";
        public static readonly string ORDER_TYPE_STOPLOSS_CONV = "STOP LOSS-CONV";
        public static readonly string ORDER_TYPE_STOPLOSS_NONCONV = "STOP LOSS-NONCONV";
        public static readonly string ORDER_TYPE_RL = "Regular Lot";
        public static readonly string ORDER_TYPE_SL = "Stop Loss";
        public static readonly string ORDER_TYPE_OCO = "OCO";
        public static readonly string ORDER_TYPE_BOC = "BOC";
        public static readonly string ORDER_TYPE_BracketOrder = "Bracket Order";
        public static readonly string ORDER_TYPE_RollOver = "RollOver";

        public static readonly string LIMIT_Request_code = "L";
        public static readonly string WaitOrderMessage = "Downloading Orders...";
        public static readonly string WaitRetOrderMessage = "Downloading Return Orders...";
        public static readonly string WaitStopLossMessage = "Downloading Stop Loss Orders...";
        public static readonly string WaitRetStopLossMessage = "Downloading Return Stop Loss Orders...";
        public static readonly string WaitTradeMessage = "Downloading Trades...";
        public static readonly string WaitLimitMessage = "Downloading Limits...";
        public static readonly string WaitGWLimitMessage = "Downloading GroupWise Limits...";

        public static readonly string CompletedMessage = "Personal Download Complete!";

        //Order Type messages
    }
}
