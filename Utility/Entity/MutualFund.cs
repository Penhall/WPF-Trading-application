using CommonFrontEnd.Utility.Interfaces_Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonFrontEnd.Utility.Entity
{
    public class MutualFund : IScript
    {

        private MutualFund()
        {
        }

        public long ID { get; set; }
        long IScript.Id
        {
            get { return ID; }
            set { ID = value; }
        }
        public long Token { get; set; }
        public string Symbol { get; set; }
        public int ExchangeId { get; set; }
        public int MarketId { get; set; }
        public string Group { get; set; }
        public string RTASchemeCode { get; set; }
        public string AMCSchemeCode { get; set; }
        public string ISIN { get; set; }
        public string AMCCode { get; set; }
        public string Name { get; set; }
        public string PurchaseTransactionmode { get; set; }
        public string MinimumPurchaseAmount { get; set; }
        public string AdditionalPurchaseAmountMultiple { get; set; }
        public string MaximumPurchaseAmount { get; set; }
        public string PurchaseAllowed { get; set; }
        public string PurchaseCutoffTime { get; set; }
        public string RedemptionTransactionMode { get; set; }
        public string MinimumRedemptionQty { get; set; }
        public string RedemptionQtyMultiplier { get; set; }
        public string MaximumRedemptionQty { get; set; }
        public string RedemptionAllowed { get; set; }
        public string RedemptionCutoffTime { get; set; }
        public string RTAAgentCode { get; set; }
        public int AMCActiveFlag { get; set; }
        public string DividendReinvestmentFlag { get; set; }
        public string SchemeType { get; set; }
        public char SIPFLAG { get; set; }
        public char STPFLAG { get; set; }
        public char SWPFLAG { get; set; }
        public string SETTLEMENTTYPE { get; set; }
        public string PurchaseAmountMultiplier { get; set; }
        public string NAV { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        /// <summary>
        /// This property dose not returns any thing,use PurchaseAmountMultiplier instead.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public long TickSize
        {
            get
            {
                throw new NotImplementedException(" PurchaseAmountMultiplier instead.");
            }

            set { }
        }
        public long BoardLotQuantity
        {
            get
            {
                throw new NotImplementedException(" PurchaseAmountMultiplier instead.");
            }

            set { }
        }
        public long SequenceId { get; set; }

        public bool IsDefaultOrdSettings { get; set; }
        public string defaultDQ { get; set; }
        public string defaultPrice { get; set; }
        public string defaultProtPer { get; set; }
        public string defaultQTY { get; set; }


    }
}
