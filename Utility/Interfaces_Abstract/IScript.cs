using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonFrontEnd.Utility.Interfaces_Abstract
{
    public interface IScript : ICloneable
    {

        string Symbol { get; set; }
        string Name { get; set; }
        long Id { get; set; }
        long Token { get; set; }
        string Group { get; set; }
        long SequenceId { get; set; }
        long BoardLotQuantity { get; set; }
        long TickSize { get; set; }
        int ExchangeId { get; set; }
        int MarketId { get; set; }
        string defaultDQ { get; set; }
        string defaultQTY { get; set; }
        string defaultPrice { get; set; }
        string defaultProtPer { get; set; }
        bool IsDefaultOrdSettings { get; set; }
    }

}
