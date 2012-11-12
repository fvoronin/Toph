using System;
using System.Collections.Generic;
using Toph.Common;

namespace Toph.Domain.Entities
{
    public class UserProfile : EntityBase
    {
        protected UserProfile()
        {
        }

        internal UserProfile(string username)
        {
            Username = username;
        }

        private readonly IList<Customer> _customers = new List<Customer>();

        public virtual string Username { get; protected set; }

        public virtual IReadOnlyList<Customer> Customers
        {
            get { return _customers.AsReadOnly(); }
        }
    }
}
