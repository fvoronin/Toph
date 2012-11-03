using System;
using System.Linq;
using Toph.Common.DataAccess;
using Toph.Domain.Entities;

namespace Toph.Domain.Queries
{
    public class UserExistsQuery : IDomainQuery<bool>
    {
        public string Username { get; set; }

        public bool Execute(IRepository repository)
        {
            return repository.Find<UserProfile>().Any(x => x.Username == Username);
        }
    }
}
