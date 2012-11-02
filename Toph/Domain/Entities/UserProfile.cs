using System;
using Toph.Common.DataAccess;

namespace Toph.Domain.Entities
{
    public class UserProfile : Entity<int>
    {
        protected UserProfile()
        {
        }

        internal UserProfile(string username)
        {
            Username = username;
        }

        public virtual string Username { get; protected set; }
    }
}
