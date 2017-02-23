using System;

namespace Intro.Domain.PersistentModels.Old
{
    public class Pawn : Entity
    {
        // TODO: will customers exist as entities in the database?
        public string CustomerName { get; set; }

        public DateTime MaturityDate { get; set; }
        
        /// <summary>
        /// Calculated as x/100 for now
        /// </summary>
        public int InterestRate { get; set; }
    }
}