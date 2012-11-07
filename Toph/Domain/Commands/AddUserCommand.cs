using System;
using System.ComponentModel.DataAnnotations;
using Toph.Common.DataAccess;
using Toph.Domain.Entities;

namespace Toph.Domain.Commands
{
    public class AddUserCommand : IDomainCommand<DomainCommandResult>
    {
        [Required]
        public string Username { get; set; }

        public DomainCommandResult Execute(IRepository repository)
        {
            var result = new DomainCommandResult();

            result.AddValidationErrors(this);

            if (result.NoErrors())
                repository.Add(new UserProfile(Username));

            return result;
        }
    }
}
