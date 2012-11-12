using System;
using System.Collections.Generic;
using Toph.Common;

namespace Toph.Domain.Entities
{
    public class Engagement : EntityBase
    {
        protected Engagement()
        {
        }

        internal Engagement(Customer customer)
        {
            Customer = customer;
        }

        private readonly IList<TimeEntry> _timeEntries = new List<TimeEntry>();

        public virtual Customer Customer { get; protected set; }

        public virtual IReadOnlyList<TimeEntry> TimeEntries
        {
            get { return _timeEntries.AsReadOnly(); }
        }
    }
}
