using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonFrontEnd.Common
{
#if BOW
    public  class Globals
    {
        public bool gblnMarketWatchLocal = false;
        public int gintNumberOfMarketWatch = 0;
        public int gintMaxNoOfMWsCreatable = 25;
        public bool gblnBroadcastMessageLog = false;
        public bool gblnIsNowModel = false;
        public bool gblnDirectBroadcastConfigured = false;
        public bool gblnUseConsizedMWMsg = true;
        public string MW_Rows_Array = "";
        private static Globals mobjInstance;
        public static Globals GETInstance
        {
            get
            {
                if (mobjInstance == null)
                {
                    mobjInstance = new Globals();
                }
                return mobjInstance;
            }
        }
    }
#endif

    public static class Globals
    {
       public static bool IsOiWindowOPen = false;
        public static bool VarWindowOpen = false;
        public static bool SessionWindowOpen = false;
        public static bool NewsWindowOpen = false;
        public static bool ClosePriceWindowOpen = false;

       // public static 
    }
    }
