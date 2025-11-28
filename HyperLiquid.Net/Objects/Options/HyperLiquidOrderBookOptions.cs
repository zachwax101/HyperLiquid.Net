using CryptoExchange.Net.Objects.Options;
using System;
using CryptoExchange.Net.Objects.Sockets;
using HyperLiquid.Net.Objects.Models;

namespace HyperLiquid.Net.Objects.Options
{
    /// <summary>
    /// Options for the HyperLiquid SymbolOrderBook
    /// </summary>
    public class HyperLiquidOrderBookOptions : OrderBookOptions
    {
        /// <summary>
        /// Default options for new clients
        /// </summary>
        public static HyperLiquidOrderBookOptions Default { get; set; } = new HyperLiquidOrderBookOptions();

        /// <summary>
        /// The top amount of results to keep in sync. If for example limit=10 is used, the order book will contain the 10 best bids and 10 best asks. Leaving this null will sync the full order book
        /// </summary>
        public int? Limit { get; set; }

        /// <summary>
        /// After how much time we should consider the connection dropped if no data is received for this time after the initial subscriptions
        /// </summary>
        public TimeSpan? InitialDataTimeout { get; set; }
        
        /// <summary>
        /// Optional nSigFigs parameter in the order book subscription (see api docs)
        /// </summary>
        public int? NSigFigs { get; set; }
        
        /// <summary>
        /// Optional mantissa parameter in the order book subscription (see api docs)
        /// </summary>
        public int? Mantissa { get; set; }

        /// <summary>
        /// Optional additional handler for order book updates. This can be used to handle updates in a custom way, e.g. to log them or to perform additional processing.
        /// </summary>
        public Action<DataEvent<HyperLiquidOrderBook>>? AdditionalUpdateHandler { get; set; }

        internal HyperLiquidOrderBookOptions Copy()
        {
            var result = Copy<HyperLiquidOrderBookOptions>();
            result.Limit = Limit;
            result.InitialDataTimeout = InitialDataTimeout;
            return result;
        }
    }
}
