using System;
using System.Collections.Generic;

namespace Intro.Domain.PersistentModels
{
    public class Event : Entity
    {
        public DateTime CreatedDate { get; set; }
        public DateTime StartDate { get; set; }
        public string CreatedBy { get; set; }

        public int AttendanceLimit { get; set; }

        public string Description { get; set; }

        public List<string> Tags { get; set; }

        public List<string> UsersAttending { get; set; }

        public List<string> UsersAccepted { get; set; }

        public List<string> UsersApplying { get; set; }
    }
}