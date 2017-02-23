using System;
using System.Collections.Generic;

namespace Intro.Domain.PersistentModels
{
    public class Item : Entity
    {
        public string SerialNumber { get; set; }
        public IDictionary<string, string> Descriptors { get; set; }
        public IList<Exchange> Exchanges { get; set; }

        // TODO potentially group this date related properties into an IDated interface
        public DateTime DateCreated { get; set; }

        // TODO this may eventually need to be a history of edits
        public DateTime? DateLastEdited { get; set; }

        public string[] ItemCategories { get; set; }
    }
}
