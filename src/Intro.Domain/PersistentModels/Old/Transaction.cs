using System;
using System.Globalization;

namespace Intro.Domain.PersistentModels.Old
{
    // TODO: how do we represent transactions that don't involve any money?
    public class Transaction
    {
        /// <summary>
        /// Represents the transaction in cents ($1/100).
        /// A positive value represents a monetary gain
        /// A negative value represents a loss.
        /// </summary>
        public Int64 Amount { get; set; }

        // TODO: make sure Mongo doesn't store this
        public Decimal GetDollarValue()
        {
            return Decimal.Parse(this.Amount.ToString(CultureInfo.InvariantCulture));
        }
    }
}