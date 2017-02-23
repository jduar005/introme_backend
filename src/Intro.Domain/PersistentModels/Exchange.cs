using System;
using System.Collections.Generic;

namespace Intro.Domain.PersistentModels
{
    public abstract class Exchange : Entity
    {
        public DateTime DateCreated { get; set; }
        public DateTime? DateLastEdited { get; set; }
        public IEnumerable<Transaction> Transactions { get; set; }

        /// <summary>
        /// If there is a value here, it means the exchange is over, and an item associated with it is free from our inventory
        /// </summary>
        public DateTime? DateFinalized { get; set; }
    }
}