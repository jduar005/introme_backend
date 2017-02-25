using System;
using System.Collections;
using System.Collections.Generic;

namespace Intro.Domain.PersistentModels
{
    public class UserProfile : Entity
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime Birthday { get; set; }

        public string Profile { get; set; }

        //        public string Images { get; set; }

        public List<string> Interests { get; set; }

        // TODO: consider making persistent model just one big list and computing each category into a view model
        public List<string> EventsAttending { get; set; }
        public List<string> EventsPending { get; set; }
        public List<string> EventsUpcoming { get; set; }
        public List<string> EventsCompleted { get; set; }
        public List<string> EventsCreated { get; set; }
    }
}