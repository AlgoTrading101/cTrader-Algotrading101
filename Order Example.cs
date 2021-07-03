using System;
using System.Linq;
using cAlgo.API;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;
using cAlgo.Indicators;
using System.Timers;

namespace cAlgo.Robots
{
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class NewcBot : Robot
    {
        [Parameter(DefaultValue = 0.0)]
        public double Parameter { get; set; }
        public double price_1;
        public double price_2;
        public double percent;

        protected override void OnStart()
        {
            price_1 = Symbol.Ask;
            // Timer will tick every Interval
            Timer.Start(TimeSpan.FromMinutes(5));
        }

        protected override void OnTimer()
        {
            price_2 = Symbol.Ask;
            percent = ((price_2 - price_1) / price_1) * 100;

            if (percent >= 0.0001)
            {
                Print("Requirement reached");
                var result = ExecuteMarketOrder(TradeType.Buy, "BTC/USD", 0.06, "order_1");
                if (LastResult.IsSuccessful)
                {
                    var position = LastResult.Position;
                    Print("Trade executed. Position price is {0}", position.EntryPrice);
                    Timer.Stop();
                    Stop();
                }
            }

            else
            {
                Print("Requirement not reached. The percentage move is at {0}", percent);
                price_1 = price_2;
            }
        }

        protected override void OnStop()
        {
            Print("Duty fulfilled");
        }
    }
}