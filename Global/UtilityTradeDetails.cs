using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonFrontEnd.Global
{
    
    public class UtilityTradeDetails
    {
        public bool SaudasWindow_PdCompleted { get; set; }
        public bool LoadManual_SaudasWindow { get; set; }
        public bool Load_NetPositionClientWise { get; set; }
        public bool Load_NetPositionScripWise { get; set; }
        public int SelectedID { get; set; }

        private static UtilityTradeDetails oUtilityTradeDetails;
        public static UtilityTradeDetails GetInstance
        {
            get
            {
                if (oUtilityTradeDetails == null)
                {
                    oUtilityTradeDetails = new UtilityTradeDetails();
                }
                return oUtilityTradeDetails;
            }
        }
    }
}
