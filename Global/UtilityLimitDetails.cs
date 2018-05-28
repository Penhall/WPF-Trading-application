using CommonFrontEnd.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonFrontEnd.Global
{
    public static class UtilityLimitDetails
    {

        private static double _GrossBuyLimit = 10000;

        public static double GrossBuyLimit
        {
            get { return _GrossBuyLimit; }
            set { _GrossBuyLimit = value; }
        }


        private static double _GrossSellLimit = 10000;

        public static double GrossSellLimit
        {
            get { return _GrossSellLimit; }
            set { _GrossSellLimit = value; }
        }


        private static double _BuyValue = 10000;

        public static double BuyValue
        {
            get { return _BuyValue; }
            set { _BuyValue = value; }
        }


        private static double _SellValue = 10000;

        public static double SellValue
        {
            get { return _SellValue; }
            set { _SellValue = value;  }
        }

    }
}
