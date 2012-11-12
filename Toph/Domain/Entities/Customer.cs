using System;
using System.Collections.Generic;
using Toph.Common;

namespace Toph.Domain.Entities
{
    public class Customer : EntityBase
    {
        protected Customer()
        {
        }

        internal Customer(UserProfile owner, string name)
        {
            Owner = owner;
            Name = name;
        }

        private readonly IList<Engagement> _engagements = new List<Engagement>();
        private readonly IList<Invoice> _invoices = new List<Invoice>();

        public virtual UserProfile Owner { get; protected set; }
        public virtual string Name { get; protected set; }

        public virtual IReadOnlyList<Engagement> Engagements
        {
            get { return _engagements.AsReadOnly(); }
        }

        public virtual IReadOnlyList<Invoice> Invoices
        {
            get { return _invoices.AsReadOnly(); }
        }
    }
}
