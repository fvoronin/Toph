using System;
using Toph.Common.DataAccess;
using Toph.Domain.Entities;

namespace Toph.Domain.Commands
{
    public class AddUserCommand : IDomainCommand<DomainCommandResult>
    {
        public string Username { get; set; }

        public DomainCommandResult Execute(IRepository repository)
        {
            repository.Add(new UserProfile(Username));

            return new DomainCommandResult();
        }
    }
}
