using CommonFrontEnd.Global;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonFrontEnd.ViewModel
{
   public class MarketReportsVM
    {
        public MarketReportsVM()
        {
           if( UtilityLoginDetails.GETInstance.IsPeerConnected==1)
            Process.Start("http://10.1.101.125:3000/twsreports/HomePage.aspx");
           else
                Process.Start("http://10.1.101.125:3000/twsreports/HomePage.aspx");
        }
    }
}
