using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonFrontEnd.ViewModel.Touchline
{
    public class MarketWatchCollection : System.Collections.CollectionBase
    {
       public List<MarketWatchHelper> objMarketWatchObj = new List<MarketWatchHelper>();
        public bool Add(MarketWatchHelper pobjMarketWatch)
        {
             objMarketWatchObj.Add(pobjMarketWatch);
            return true;
        }
    }
}
