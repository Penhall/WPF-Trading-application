using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonFrontEnd.Utility.Interfaces_Abstract
{
    public abstract class SecurityBase : IScript
    {


        public long Id { get; set; }
        public long Token { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }
        public long SequenceId { get; set; }

        public int ExchangeId { get; set; }
        public int MarketId { get; set; }

        public long TickSize { get; set; }
        public long BoardLotQuantity { get; set; }

        public string ISIN { get; set; }
        public string Series { get; set; }
        string IScript.Group
        {
            get { return Series; }
            set { Series = value; }
        }
        public string CompanyName { get; set; }

        public bool IsDefaultOrdSettings { get; set; }
        public string defaultDQ { get; set; }
        public string defaultPrice { get; set; }
        public string defaultProtPer { get; set; }
        public string defaultQTY { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

}
