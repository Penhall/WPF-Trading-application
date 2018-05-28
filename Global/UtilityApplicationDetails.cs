using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonFrontEnd.Global
{

    public class UtilityApplicationDetails
    {
        private string _CurrentDirectory;

        public string CurrentDirectory
        {
            get
            {
                return (_CurrentDirectory = Environment.CurrentDirectory);
            }
        }

        private int _maxDecimalPoint = 4;

        public int MaxDecimalPoint
        {
            get { return _maxDecimalPoint; }
            set { _maxDecimalPoint = value; }
        }

        private static UtilityApplicationDetails oUtilityApplicationDetails;
        public static UtilityApplicationDetails GetInstance
        {
            get
            {
                if (oUtilityApplicationDetails == null)
                {
                    oUtilityApplicationDetails = new UtilityApplicationDetails();
                }
                return oUtilityApplicationDetails;
            }
        }
    }
}
