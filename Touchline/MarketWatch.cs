using CommonFrontEnd.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonFrontEnd.ViewModel.Touchline
{
    public class MarketWatchHelper
    {
        RecordSplitter mobjRecordHelper;
        public enum MWType : int
        {
            Index = 2,
            Industrial = 3,
            UserDefined = 1
        }
        private int mintId;
        public int Id
        {
            get { return mintId; }
            set { mintId = value; }
        }

        private string mstrType;
        public string Type
        {
            get { return mstrType; }
            set { mstrType = value; }
        }

        private string mstrName;
        public string Name
        {
            get { return mstrName; }
            set { mstrName = value; }
        }

       public MarketWatchHelper(string marketDAta)
        {
            mobjRecordHelper = new RecordSplitter(marketDAta);
        }

        public MarketWatchHelper()
        {

        }
        public bool isServerError()
        {
            bool functionReturnValue = false;
            string lstrErrorMessage = null;
            if (!(Convert.ToInt32(mobjRecordHelper.getField(0, 0)) == 0))
            {
                lstrErrorMessage = mobjRecordHelper.getField(0, 1);
                throw new Exception("Tomcat Error : " + lstrErrorMessage);
                
                return functionReturnValue=false;
            }
           
            return functionReturnValue=true;
        }
    }
}
