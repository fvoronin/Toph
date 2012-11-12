using System;

namespace Toph.Domain.Entities
{
    public class TimeEntry : EntityBase
    {
        protected TimeEntry()
        {
        }

        internal TimeEntry(Engagement engagement, DateTimeOffset start, DateTimeOffset end)
        {
            Engagement = engagement;
            TimeEntryStart = start;
            TimeEntryEnd = end;
        }

        public virtual Engagement Engagement { get; protected set; }
        public virtual DateTimeOffset TimeEntryStart { get; protected set; }
        public virtual DateTimeOffset TimeEntryEnd { get; protected set; }
        public virtual InvoiceLineItem LineItem { get; protected set; }
    }
}
