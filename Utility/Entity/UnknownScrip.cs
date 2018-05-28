using CommonFrontEnd.Utility.Interfaces_Abstract;

namespace CommonFrontEnd.Utility.Entity
{
    class UnknownScrip : IScript
    {

        internal UnknownScrip()
        {
        }


        public long Id { get; set; }
        public int ExchangeId { get; set; }
        public int MarketId { get; set; }
        public string Name { get; set; }
        public long SequenceId { get; set; }
        public string Symbol { get; set; }
        public long Token { get; set; }
        public string Group { get; set; }
        public long TickSize { get; set; }
        public long BoardLotQuantity { get; set; }


        public string defaultDQ { get; set; }
        public string defaultQTY { get; set; }
        public string defaultPrice { get; set; }
        public string defaultProtPer { get; set; }
        public bool IsDefaultOrdSettings { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }



    }
}
